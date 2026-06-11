using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERPData;
using ERPSMS_v01.Reports.ReportDataBuilders.ParameterBinder;
using BusinessObject.Reports;
using ERP.Utilities;
using BusinessObject.CommonManagement;
using System.Data;
using System.Threading;
using BusinessObject.Reports.Factory;

namespace ERPSMS_v01.Reports.ReportDataBuilders
{
    public sealed class SalesVoucherReportBuilder : ReportDataBuilder
    {
        SalesVoucher reportData;
        public SalesVoucherReportBuilder(string rptType) : base(rptType) {
            reportData = new SalesVoucher();
        }

        public override string GetReportTemplate(IReportParameter reportParameter)
        {
            string _template = string.Empty;
            if (reportParameter is SalesVoucherReportParameter)
            {
                _template = GeneralSalesVoucher((SalesVoucherReportParameter)reportParameter);
            }
            return _template;
        }

        private string GeneralSalesVoucher(SalesVoucherReportParameter parameter)
        {
            SetReportParameters(parameter.AppTypeDetailsList);
            SetFieldFormats(parameter.dataTable);
            SetReportData(parameter);
            return buildReportBody();
        }
        private void SetReportParameters(List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList)
        {
            #region Getting Report Parameters
            DataSet dsParamSettings;
            string footer;
            string rptName = string.Empty;
            footer = string.Empty;
            string signaturePath = string.Empty;
            dsParamSettings = new DataSet();

            dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(AppTypeDetailsList[0].AST_RPT_SETTINGS)));

            if (dsParamSettings.Tables.Count > 0)
            {
                if (dsParamSettings.Tables[0].Columns.Contains("LOGO_HIDE"))
                {
                    reportData.HideLogo = dsParamSettings.Tables[0].Rows[0]["LOGO_HIDE"].ToString();
                    reportData.HideLogo = reportData.HideLogo == "True" ? "none" : "inherit";
                }
                if (dsParamSettings.Tables[0].Columns.Contains("HEADING_HIDE"))
                {
                    reportData.HideHeadTitle = dsParamSettings.Tables[0].Rows[0]["HEADING_HIDE"].ToString();
                    reportData.HideHeadTitle = reportData.HideHeadTitle == "True" ? "none" : "inherit";
                }
                if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING_HIDE"))
                {
                    reportData.HideSubTitle = dsParamSettings.Tables[0].Rows[0]["SUB_HEADING_HIDE"].ToString();
                    reportData.HideSubTitle = reportData.HideSubTitle == "True" ? "none" : "inherit";
                }
                if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_LEFT_HIDE"))
                {
                    reportData.HideFooterText = dsParamSettings.Tables[0].Rows[0]["FOOTER_LEFT_HIDE"].ToString();
                    reportData.HideFooterText = reportData.HideFooterText == "True" ? "none" : "inherit";
                }
                if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_RIGHT_HIDE"))
                {
                    reportData.HidePageNo = dsParamSettings.Tables[0].Rows[0]["FOOTER_RIGHT_HIDE"].ToString();
                    reportData.HidePageNo = reportData.HidePageNo == "True" ? "none" : "inherit";
                }
                if (dsParamSettings.Tables[0].Columns.Contains("HEADING"))
                {
                    reportData.HeadTitle = dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString();
                }
                if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING"))
                {
                    reportData.SubTitle = dsParamSettings.Tables[0].Rows[0]["SUB_HEADING"].ToString();
                }

                if (dsParamSettings.Tables[0].Columns.Contains("REPORT_WIDTH"))
                {
                    reportData.PrinterSettings.DocumentWidth = dsParamSettings.Tables[0].Rows[0]["REPORT_WIDTH"].ToString();
                }
                if (dsParamSettings.Tables[0].Columns.Contains("REPORT_HEIGHT"))
                {
                    reportData.PrinterSettings.DocumentHeight = dsParamSettings.Tables[0].Rows[0]["REPORT_HEIGHT"].ToString();
                }
                if (dsParamSettings.Tables[0].Columns.Contains("REPORT_WIDTH_PX"))
                {
                    reportData.PrinterSettings.WindowWidth = dsParamSettings.Tables[0].Rows[0]["REPORT_WIDTH_PX"].ToString();
                }
                if (dsParamSettings.Tables[0].Columns.Contains("REPORT_HEIGHT_PX"))
                {
                    reportData.PrinterSettings.WindowHeight = dsParamSettings.Tables[0].Rows[0]["REPORT_HEIGHT_PX"].ToString();
                }
            }
            #endregion
        }

        private void SetFieldFormats(DataTable dataTable)
        {
            #region FormatCalculation
            string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            string currencyformat = "#" + currencysep + "#0.";
            string NoFormat = "#" + currencysep + "#0.";
            string ExchRateDigt = "#" + currencysep + "#0.";
            string RateDeciDigt = "#" + currencysep + "#0.";
            string RateDecDigitPP = "#" + currencysep + "#0.";
            string currencydecimals = "";
            string Nodecimal = string.Empty;
            string ExchRateDigit = string.Empty;
            string RateDecimalDigit = string.Empty;
            string RateDecimalDigitPP = string.Empty;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                int curdigit = Convert.ToInt32(dataTable.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                for (int i = 0; i < curdigit; i++)
                {
                    currencydecimals += "0";
                }

                int NoDigit = Convert.ToInt32(dataTable.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());
                for (int i = 0; i < NoDigit; i++)
                {
                    Nodecimal += "0";
                }
                int ExchRate = Convert.ToInt32(dataTable.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                for (int i = 0; i < ExchRate; i++)
                {
                    ExchRateDigit += "0";
                }

                int RateDecimal = Convert.ToInt32(dataTable.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                for (int i = 0; i < RateDecimal; i++)
                {
                    RateDecimalDigit += "0";
                }
                int RateDecimalPP = Convert.ToInt32(dataTable.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
                for (int i = 0; i < RateDecimalPP; i++)
                {
                    RateDecimalDigitPP += "0";
                }
            }
            else
            {
                currencydecimals = "00";
                Nodecimal = "00";
            }
            currencyformat = currencyformat + currencydecimals;
            NoFormat = NoFormat + Nodecimal;
            ExchRateDigt = ExchRateDigt + ExchRateDigit;
            RateDeciDigt = RateDeciDigt + RateDecimalDigit;
            RateDecDigitPP = RateDecDigitPP + RateDecimalDigitPP;

            reportData.DateFormat = Resources.Constants.ReportDateFormat;
            reportData.CurrencyFormat = currencyformat;
            reportData.NumberFormat = NoFormat;

            if (RptType == ApplicationType.VSE || RptType == ApplicationType.PI || RptType == ApplicationType.CN ||
                            RptType == ApplicationType.DN || RptType == ApplicationType.SI || RptType == ApplicationType.MSI ||
                            RptType == ApplicationType.SIJ || RptType == ApplicationType.MSIJ || RptType == ApplicationType.CRJ ||
                            RptType == ApplicationType.JV || RptType == ApplicationType.SO || RptType == ApplicationType.PIJ ||
                            RptType == ApplicationType.PSIJ || RptType == ApplicationType.DNJ || RptType == ApplicationType.CNJ ||
                            RptType == ApplicationType.PDCCJ || RptType == ApplicationType.RCBJ || RptType == ApplicationType.PSAS ||
                            RptType == ApplicationType.PCS || RptType == ApplicationType.VPJ || RptType == ApplicationType.SIPJ ||
                            RptType == ApplicationType.EIPJ || RptType == ApplicationType.DPVJ || RptType == ApplicationType.PCVJ ||
                            RptType == ApplicationType.EIJ || RptType == ApplicationType.PPCCJ || RptType == ApplicationType.PCBJ ||
                            RptType == ApplicationType.PO || RptType == ApplicationType.MSIRJ || RptType == ApplicationType.MI ||
                            RptType == ApplicationType.FCHRJ)//@@
            {
                reportData.ExchangeRate = ExchRateDigt;
                reportData.RateFormat = RateDeciDigt;
            }
            
            #endregion
        }

        private void SetReportData(SalesVoucherReportParameter parameter)
        {
            #region Setting Report Data

            reportData.FooterText = "Printed by " + parameter.PrintedUser + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
            SPFIN_SALES_VOUCHER_RPT_Result reportDataSource = parameter.SvHeaderDataSourceList[0];  
            //GetCompany Info
            SPADM_COMPANY_MST_GET_KV_Result company = GetCompanyDetails(reportDataSource.FTH_COMPANY);

            reportData.CompanyName = HttpUtility.HtmlDecode(company.CMP_NAME ?? CommonConstants.HTML_SPACE);
            reportData.Logo = company == null || company.CMP_LOGO.IsNullOrEmptyOrWhitespace() ? string.Empty : company.CMP_LOGO_URL.Replace("~/","../");
            if (reportData.Logo.IsNullOrEmptyOrWhitespace()) reportData.HideLogo = "none";
            reportData.HideCompanyName = "none";
            reportData.ReportHeaderName = reportDataSource.FTH_STATUS.ToString() == "0"
                                            ? HttpUtility.HtmlDecode(reportData.HeadTitle) + " - Draft"
                                            : HttpUtility.HtmlDecode(reportData.HeadTitle);

            reportData.SalesVoucherNo = reportDataSource.FTH_VOUCHER_NO ?? " -";
            reportData.Date = reportDataSource.FTH_DATE.Value.ToString(reportData.DateFormat);
            reportData.CustomerName = reportDataSource.CUSTOMER_TEXT;
            reportData.InvoiceOrTaxNo = reportDataSource.ICH_NO;
            reportData.InventoryDate = reportDataSource.ICH_DATE.ToString(reportData.DateFormat);
            reportData.Currency = reportDataSource.CURRENCY_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.Amount = reportDataSource.ICH_NET_AMOUNT.ToString(reportData.CurrencyFormat);
            reportData.BaseCurrency = parameter.SVAccountDtlsDataSourceList[0].FTH_BASE_CURR_TEXT;
            reportData.ExchangeRateValue = reportDataSource.ICH_EXCHG_RATE.ToString(reportData.ExchangeRate);

            decimal _totalDebitAmt = ((List<SPFIN_TRX_VOUCHER_RPT_Result>)parameter.SVAccountDtlsDataSourceList).Sum(x => x.FTR_DR_AMT_BC ?? (decimal?)0) ?? 0;
            decimal _totalCreditAmt = ((List<SPFIN_TRX_VOUCHER_RPT_Result>)parameter.SVAccountDtlsDataSourceList).Sum(x => x.FTR_CR_AMT_BC ?? (decimal?)0) ?? 0;
            
            reportData.DebitTotal = _totalDebitAmt.ToString(reportData.CurrencyFormat);
            reportData.CreditTotal = _totalCreditAmt.ToString(reportData.CurrencyFormat);

            //Setting Child Data
            string baseCurrencyFraction=string.Empty;

            foreach (SPFIN_TRX_VOUCHER_RPT_Result subItem in parameter.SVAccountDtlsDataSourceList)
            {
                if(baseCurrencyFraction == string.Empty)baseCurrencyFraction = subItem.FTH_BASE_CURR_FRACTION??CommonConstants.HTML_SPACE;
                reportData.ItemDetails.Add(new SalesVoucherItem
                {
                    AccCode = subItem.FTR_ACCOUNT_CODE,
                    AccName = HttpUtility.HtmlDecode(subItem.FTR_ACCOUNT_NAME),                            
                    Description=HttpUtility.HtmlDecode(subItem.FTR_NARRATION),
                    ExRate = (!subItem.FTR_EXCHG_RATE.HasValue || subItem.FTR_EXCHG_RATE.Value == 0 || subItem.FTR_EXCHG_RATE.Value == 1) 
                                ?  CommonConstants.HTML_SPACE
                                : subItem.FTR_EXCHG_RATE.Value.ToString(reportData.ExchangeRate),
                    Debit = subItem.FTR_DR_AMT_BC.HasValue && subItem.FTR_DR_AMT_BC.Value > 0 ? subItem.FTR_DR_AMT_BC.Value.ToString(reportData.CurrencyFormat) : CommonConstants.HTML_SPACE,
                    Credit = subItem.FTR_CR_AMT_BC.HasValue && subItem.FTR_CR_AMT_BC.Value > 0 ? subItem.FTR_CR_AMT_BC.Value.ToString(reportData.CurrencyFormat) : CommonConstants.HTML_SPACE
                });
            }            

            reportData.AmountInWords = new NumberToWordsConvertorFactory(reportData.BaseCurrency)
                                           .GetNumberToWordsConvertor()
                                           .ConvertNumberToWords(_totalDebitAmt.ToString(reportData.CurrencyFormat).Replace(",",string.Empty))
                                           .Replace("Paise", baseCurrencyFraction);

            reportData.Remarks = reportDataSource.FTH_REMARKS ?? CommonConstants.HTML_SPACE;
            reportData.HideRemarks = string.IsNullOrWhiteSpace(reportData.Remarks) ? "none" : "table-row";
            reportData.Narration = reportDataSource.FTH_NARRATION;
            reportData.HideNarration = string.IsNullOrWhiteSpace(reportData.Narration) ? "none" : "table-row";

            reportData.PreparedBy = reportDataSource.FTH_TASK1_BY_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.ReviewedBy = reportDataSource.FTH_TASK2_BY_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.ApprovedBy = reportDataSource.FTH_TASK3_BY_TEXT ?? CommonConstants.HTML_SPACE;

            reportData.PreparedByDate = reportDataSource.FTH_TASK1_DT_TEXT.HasValue
                                        ? reportDataSource.FTH_TASK1_DT_TEXT.Value.ToString(reportData.DateFormat)
                                        : CommonConstants.HTML_SPACE;
            reportData.ReviewedByDate = reportDataSource.FTH_TASK2_DT_TEXT.HasValue
                                        ? reportDataSource.FTH_TASK2_DT_TEXT.Value.ToString(reportData.DateFormat)
                                        : CommonConstants.HTML_SPACE;
            reportData.ApprovedByDate = reportDataSource.FTH_TASK3_DT_TEXT.HasValue
                                       ? reportDataSource.FTH_TASK3_DT_TEXT.Value.ToString(reportData.DateFormat)
                                       : CommonConstants.HTML_SPACE;
            reportData.FooterText = string.Format("Printed by {0} On {1}", parameter.PrintedUser, DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));
            #endregion
        }

        private string buildReportBody()
        {
            #region Build Report Body

            string _rowData = string.Empty;
            foreach (var subItem in reportData.ItemDetails)
            {
                _rowData += string.Format(
                    reportData.RowTemplate,
                    subItem.AccCode,
                    subItem.AccName,
                    subItem.Description,
                    subItem.ExRate,
                    subItem.Debit,
                    subItem.Credit
                );
            };

            string reoportBody = string.Format(
                                       reportData.BodyTemplate,
                                       reportData.HideLogo,              // 0
                                       reportData.Logo,                  // 1
                                       reportData.HideCompanyName,       // 2
                                       reportData.CompanyName,           // 3
                                       reportData.HideHeadTitle,         // 4
                                       reportData.HeadTitle,             // 5
                                       reportData.CustomerName,          // 6
                                       reportData.SalesVoucherNo,        // 7
                                       reportData.InvoiceOrTaxNo,        // 8
                                       reportData.InventoryDate,         // 9
                                       reportData.Date,                  // 10
                                       reportData.Currency,              // 11
                                       reportData.ExchangeRateValue,     // 12
                                       reportData.Amount,                // 13  
                                       reportData.BaseCurrency,          // 14                                         
                                       _rowData,                         // 15
                                       reportData.DebitTotal,            // 16
                                       reportData.CreditTotal,           // 17
                                       reportData.AmountInWords,         // 18
                                       reportData.HideRemarks,           // 19
                                       reportData.Remarks,               // 20
                                       reportData.HideNarration,         // 21
                                       reportData.Narration,             // 22
                                       reportData.PreparedBy,            // 23
                                       reportData.PreparedByDate,        // 24
                                       reportData.ReviewedBy,            // 25
                                       reportData.ReviewedByDate,        // 26
                                       reportData.ApprovedBy,            // 27
                                       reportData.ApprovedByDate,        // 28
                                       reportData.FooterText             // 29
                                );
            #endregion

            #region Return Value
            string returnToList = "backToList('../journalize/JournalizeListing.aspx?Type=" + RptType + "');";

            return returnToList + "printVoucher(\"" + reoportBody.Replace("\"", "&quot;") + "\",{ width:'" + reportData.PrinterSettings.DocumentWidth + "', height:'" + reportData.PrinterSettings.DocumentHeight + "', windowWidth:'" + reportData.PrinterSettings.WindowWidth + "', windowHeight:'" + reportData.PrinterSettings.WindowHeight + "' }," + "'../Css/domatrix-printer-friendly-report-styles.css'" + ");";
            #endregion
        }

        #region Helper Methods
        private string GetCompanyAddress(SPADM_COMPANY_MST_GET_KV_Result company)
        {
            return String.Format(
                        "{1} {2}{0}{3}{4}",
                        CommonConstants.HTML_NEW_LINE,
                        HttpUtility.HtmlDecode(company.CMP_ADDR1),
                        HttpUtility.HtmlDecode(company.CMP_ADDR2),
                        CommonFunctions.IsNullOrEmptyOrWhitespace(company.CMP_PHONE) ? CommonConstants.HTML_SPACE : company.CMP_PHONE,
                        CommonFunctions.IsNullOrEmptyOrWhitespace(company.CMP_FAX) ? CommonConstants.HTML_SPACE : company.CMP_FAX
                        )
                        .Replace(Environment.NewLine, CommonConstants.HTML_NEW_LINE);
        }

        private SPADM_COMPANY_MST_GET_KV_Result GetCompanyDetails(int CmpnyPk)
        {
            ERPEntities currentEntity = new ERPEntities();
            return currentEntity.SPADM_COMPANY_MST_GET_KV(CmpnyPk, Convert.ToByte(DbActiveStatus.HASPK), null,null,null).FirstOrDefault();
        }
        #endregion
    }
}