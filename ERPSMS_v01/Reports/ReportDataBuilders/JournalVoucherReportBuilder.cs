using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BusinessObject.Reports;
using ERPData;
using System.Threading;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using BusinessObject.Reports.Factory;
using ERPSMS_v01.Reports.ReportDataBuilders.ParameterBinder;

namespace ERPSMS_v01.Reports.ReportDataBuilders
{
    public sealed class JournalVoucherReportBuilder : ReportDataBuilder
    {
        JournalVoucher reportData;
        public JournalVoucherReportBuilder(string rptType)
            : base(rptType)
        {
            reportData = new JournalVoucher();
        }
        public override string GetReportTemplate(IReportParameter reportParameter)
        {
            CommonReportParameter parameter = (CommonReportParameter)reportParameter;
            SetReportParameters(parameter.AppTypeDetailsList);
            SetFieldFormats(parameter.dataTable);
            SetReportData(parameter);
            return buildReportBody();
        }

        #region Getting Report Parameters
        private void SetReportParameters(List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList)
        {
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
        }
        #endregion

        #region FormatCalculation
        private void SetFieldFormats(DataTable dataTable)
        {
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
        }
        #endregion

        #region Setting Report Data
        private void SetReportData(CommonReportParameter parameter)
        {
            SPFIN_TRX_VOUCHER_RPT_Result reportDataSource = parameter.reportDataSourceList[0];

            SPADM_COMPANY_MST_GET_KV_Result company = GetCompanyDetails(reportDataSource.FTH_COMPANY);

            reportData.Logo = company == null || company.CMP_LOGO.IsNullOrEmptyOrWhitespace() ? string.Empty : company.CMP_LOGO_URL.Replace("~/", "../");
            reportData.FooterText = "Printed by " + parameter.PrintedUser + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");

            reportData.HeadTitle += reportDataSource.FTH_STATUS.ToString() == "0" ? " - Draft" : string.Empty;
            reportData.FromTo = this.RptType == ApplicationType.CNJ ? "From" : "To";
            reportData.FromToValue = HttpUtility.HtmlDecode(reportDataSource.FTH_CUS_VND_TEXT ?? CommonConstants.HTML_SPACE);
            reportData.JournalNoLabel = this.RptType == ApplicationType.JV
                                            ? "JV-Journal No"
                                            : this.RptType == ApplicationType.DNJ
                                                  ? "DN-Journal No"
                                                  : this.RptType == ApplicationType.CNJ
                                                    ? "CN-Journal No"
                                                    : CommonConstants.HTML_SPACE;
            reportData.VoucherNo = reportDataSource.FTH_VOUCHER_NO ?? CommonConstants.HTML_SPACE;
            reportData.Date = reportDataSource.FTH_DATE.HasValue ? reportDataSource.FTH_DATE.Value.ToString(reportData.DateFormat) : CommonConstants.HTML_SPACE;
            reportData.TrxCurrency = reportDataSource.FTH_TRX_CURR_TEXT ?? CommonConstants.HTML_SPACE;

            decimal? _totalDebitAmt = parameter.reportDataSourceList.Sum(x => x.FTR_DR_AMT_BC ?? (decimal?)0) ?? 0;
            reportData.DebitTotal = _totalDebitAmt.HasValue ? _totalDebitAmt.Value.ToString(reportData.CurrencyFormat) : CommonConstants.HTML_SPACE;
            reportData.Amount = reportData.DebitTotal == "0" ? CommonConstants.HTML_SPACE : reportData.DebitTotal;
            reportData.RefNo = reportDataSource.FTH_REF_NO ?? CommonConstants.HTML_SPACE;
            reportData.RefDate = reportDataSource.FTH_REF_DATE.HasValue
                ? reportDataSource.FTH_REF_DATE.Value.ToString(reportData.DateFormat)
                :CommonConstants.HTML_SPACE;

            //Setting Child Data

            foreach (SPFIN_TRX_VOUCHER_RPT_Result subItem in parameter.reportDataSourceList)
            {
                reportData.ItemDetails.Add(new VoucherDetails
                {
                    AccCode = subItem.FTR_ACCOUNT_CODE,
                    AccName = HttpUtility.HtmlDecode(subItem.FTR_ACCOUNT_NAME),
                    Description = HttpUtility.HtmlDecode(subItem.FTR_NARRATION ?? CommonConstants.HTML_SPACE),
                    ExRate = subItem.FTR_EXCHG_RATE.HasValue && subItem.FTR_EXCHG_RATE.Value == 1
                                ? CommonConstants.HTML_SPACE
                                : subItem.FTR_EXCHG_RATE.Value.ToString(reportData.ExchangeRate),
                    Credit = subItem.FTR_CR_AMT_BC.HasValue && subItem.FTR_CR_AMT_BC.Value > 0
                                ? subItem.FTR_CR_AMT_BC.Value.ToString(reportData.CurrencyFormat)
                                : CommonConstants.HTML_SPACE,
                    Debit = subItem.FTR_DR_AMT_BC.HasValue && subItem.FTR_DR_AMT_BC.Value > 0
                                ? subItem.FTR_DR_AMT_BC.Value.ToString(reportData.CurrencyFormat)
                                : CommonConstants.HTML_SPACE
                });
            }

            reportData.BaseCurrency = reportDataSource.FTH_BASE_CURR_TEXT ?? CommonConstants.HTML_SPACE;

            reportData.AmountInWords = new NumberToWordsConvertorFactory(reportData.BaseCurrency)
                                           .GetNumberToWordsConvertor()
                                           .ConvertNumberToWords(reportData.Amount.Replace(",", string.Empty))
                                           .Replace("Paise", reportDataSource.FTH_BASE_CURR_FRACTION);

            reportData.Remarks = reportDataSource.FTH_REMARKS ?? CommonConstants.HTML_SPACE;
            reportData.RemarksHide = reportData.Remarks.IsNullOrEmptyOrWhitespace() ? "none" : "inherit";
            reportData.Narration = reportDataSource.FTH_NARRATION ?? CommonConstants.HTML_SPACE;
            reportData.NarrationHide = reportData.Narration.IsNullOrEmptyOrWhitespace() ? "none" : "inherit";

            reportData.PreparedBy = reportDataSource.FTH_TASK1_BY_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.ReviewedBy = reportDataSource.FTH_TASK2_BY_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.ApprovedBy = reportDataSource.FTH_TASK3_BY_TEXT ?? CommonConstants.HTML_SPACE;

            reportData.PreparedDate = reportDataSource.FTH_TASK1_DT_TEXT.HasValue
                                        ? reportDataSource.FTH_TASK1_DT_TEXT.Value.ToString(reportData.DateFormat)
                                        : CommonConstants.HTML_SPACE;
            reportData.ReviewedDate = reportDataSource.FTH_TASK2_DT_TEXT.HasValue
                                        ? reportDataSource.FTH_TASK2_DT_TEXT.Value.ToString(reportData.DateFormat)
                                        : CommonConstants.HTML_SPACE;
            reportData.ApprovedDate = reportDataSource.FTH_TASK3_DT_TEXT.HasValue
                                        ? reportDataSource.FTH_TASK3_DT_TEXT.Value.ToString(reportData.DateFormat)
                                        : CommonConstants.HTML_SPACE;
        }
        #endregion

        #region Build Report Body
        private string buildReportBody()
        {
            #region Build Report Body

            string _rowData = string.Empty;
            string temp = reportData.RowTemplate;
            foreach (var subItem in reportData.ItemDetails)
            {
                _rowData += string.Format(
                    reportData.RowTemplate,
                    subItem.AccCode,
                    subItem.AccName,
                    string.IsNullOrWhiteSpace(subItem.Description) ? CommonConstants.HTML_SPACE : subItem.Description,
                    subItem.ExRate,
                    subItem.Debit,
                    subItem.Credit
                );
            };

            string reoportBody = string.Format(
                                        reportData.BodyTemplate,
                                        reportData.Logo,
                                        reportData.HeadTitle,
                                        reportData.FromTo,
                                        reportData.FromToValue,
                                        reportData.JournalNoLabel,
                                        reportData.VoucherNo,
                                        reportData.Date,
                                        reportData.TrxCurrency,
                                        reportData.Amount,
                                        reportData.RefNo,
                                        reportData.RefDate,
                                        reportData.BaseCurrency,
                                        reportData.DebitTotal,
                                        reportData.DebitTotal,
                                        reportData.AmountInWords,
                                        reportData.Remarks,
                                        reportData.Narration,
                                        reportData.PreparedBy,
                                        reportData.ReviewedBy,
                                        reportData.ApprovedBy,
                                        reportData.PreparedDate,
                                        reportData.ReviewedDate,
                                        reportData.ApprovedDate,
                                        reportData.FooterText,
                                        reportData.HideLogo,
                                        reportData.HideHeadTitle,
                                        reportData.RemarksHide,
                                        reportData.NarrationHide,
                                        _rowData
                                );
            #endregion

            #region Return Value
            string returnToList = "backToList('../journalize/JournalizeListing.aspx?Type=" + RptType + "');";

            return returnToList + "printVoucher(\"" + reoportBody.Replace("\"", "&quot;") + "\",{ width:'" + reportData.PrinterSettings.DocumentWidth + "', height:'" + reportData.PrinterSettings.DocumentHeight + "', windowWidth:'" + reportData.PrinterSettings.WindowWidth + "', windowHeight:'" + reportData.PrinterSettings.WindowHeight + "' }," + "'../Css/domatrix-printer-friendly-report-styles.css'" + ");";
            #endregion
        }
        #endregion

        #region Helper Methods
        private SPADM_COMPANY_MST_GET_KV_Result GetCompanyDetails(int CmpnyPk)
        {
            ERPEntities currentEntity = new ERPEntities();
            return currentEntity.SPADM_COMPANY_MST_GET_KV(CmpnyPk, Convert.ToByte(DbActiveStatus.HASPK), null,null,null).FirstOrDefault();
        }
        #endregion
    }
}