param(
    [string]$ConfigPath = (Join-Path $PSScriptRoot 'ERPSMS_v01\Web.config')
)

# Read-only verification of report 2104 using the reported item/store filters.
$ErrorActionPreference = 'Stop'
$config = [xml](Get-Content -LiteralPath $ConfigPath)
$connectionString = ($config.configuration.connectionStrings.add |
    Where-Object name -eq 'ConnectionString').connectionString
$builder = [System.Data.SqlClient.SqlConnectionStringBuilder]::new($connectionString)
if ($builder.InitialCatalog -ne 'MMT_LIVE_NEW_BKP_20260903') {
    throw 'Verification must use MMT_LIVE_NEW_BKP_20260903.'
}

$xml = @'
<FilterParameters xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <FromDate>01-Sep-2026</FromDate><ToDate>30-Sep-2026</ToDate>
  <BizUnit>1</BizUnit><Dept>8</Dept><UserPK>1</UserPK><RptPK>2104</RptPK><Currency>133</Currency>
  <Parameter><ParamName>ITM_CATEGORY</ParamName><Values><Value>28</Value></Values><Values><Value>129</Value></Values><Values><Value>130</Value></Values></Parameter>
  <Parameter><ParamName>ITM_PK</ParamName><Values><Value>21326</Value></Values></Parameter>
  <Parameter><ParamName>DPT_PK</ParamName><Values><Value>20004</Value></Values></Parameter>
  <Parameter><ParamName>TRAN_ONLY</ParamName><Values><Value>0</Value></Values></Parameter>
  <Parameter><ParamName>ALL_ITEMS</ParamName><Values><Value>0</Value></Values></Parameter>
  <Parameter><ParamName>IS_RETURN</ParamName><Values><Value>0</Value></Values></Parameter>
  <Parameter><ParamName>FIN_YEAR</ParamName><Values><Value>14</Value></Values></Parameter>
</FilterParameters>
'@

$connection = [System.Data.SqlClient.SqlConnection]::new($connectionString)
try {
    $connection.Open()
    $results = @{}
    foreach ($case in @(
        @{ Name = 'August'; From = '01-Aug-2026'; To = '31-Aug-2026' },
        @{ Name = 'September'; From = '01-Sep-2026'; To = '30-Sep-2026' },
        @{ Name = 'Combined'; From = '01-Aug-2026'; To = '30-Sep-2026' }
    )) {
        $command = $connection.CreateCommand()
        $command.CommandTimeout = 180
        $command.CommandText = @'
EXEC dbo.SPINV_STK_CARD_BATCH_CNDN_RPT_MMT_FIN_YEAR_NEW @P_XML=@xml;
EXEC dbo.SPINV_STK_CARD_BATCH_TRX_DTL_CNDN_RPT_MMT_FIN_YEAR_NEW @P_XML=@xml;
'@
        [void]$command.Parameters.Add('@xml', [System.Data.SqlDbType]::Xml)
        $command.Parameters['@xml'].Value = $xml.Replace('01-Sep-2026', $case.From).Replace('30-Sep-2026', $case.To)
        $data = [System.Data.DataSet]::new()
        [void][System.Data.SqlClient.SqlDataAdapter]::new($command).Fill($data)
        if ($data.Tables.Count -ne 2 -or $data.Tables[0].Rows.Count -ne 1 -or $data.Tables[1].Rows.Count -eq 0) {
            throw ('Unexpected report results for ' + $case.Name)
        }
        $results[$case.Name] = $data
        $summary = $data.Tables[0].Rows[0]
        $detail = $data.Tables[1].Rows[0]
        [pscustomobject]@{
            Period = $case.Name
            OpeningQty = $detail.STX_OPENING_QTY
            OpeningValue = $detail.STX_OPENING_VALUE_ROW
            OpeningPrice = [Math]::Round([decimal]$detail.STX_OPENING_RATE_ROW, 4)
            ClosingQty = $summary.STX_CLS_QTY
            ClosingValue = $summary.STX_CLS_VALUE
        }
    }

    $august = $results.August.Tables[0].Rows[0]
    $september = $results.September.Tables[0].Rows[0]
    $detail = $results.September.Tables[1].Rows[0]
    $combined = $results.Combined.Tables[0].Rows[0]
    if ($august.STX_CLS_QTY -ne [decimal]883.995 -or $august.STX_CLS_VALUE -ne [decimal]31190.27) {
        throw 'August closing differs from the reported baseline.'
    }
    if ($september.STX_OPN_QTY -ne $august.STX_CLS_QTY -or $september.STX_OPN_VALUE -ne $august.STX_CLS_VALUE) {
        throw 'Summary: September opening does not match August closing.'
    }
    if ($detail.STX_OPENING_QTY -ne $august.STX_CLS_QTY -or $detail.STX_OPENING_VALUE_ROW -ne $august.STX_CLS_VALUE) {
        throw 'Detail: September opening does not match August closing.'
    }
    if ([Math]::Round([decimal]$detail.STX_OPENING_RATE_ROW, 4) -ne [decimal]35.2833) {
        throw 'September opening unit price is incorrect.'
    }
    if ($combined.STX_CLS_QTY -ne $september.STX_CLS_QTY -or $combined.STX_CLS_VALUE -ne $september.STX_CLS_VALUE) {
        throw 'Combined date range does not match the monthly closing.'
    }
    Write-Host 'PASS: September opening matches August closing; combined date range agrees.'
}
finally {
    $connection.Dispose()
}
