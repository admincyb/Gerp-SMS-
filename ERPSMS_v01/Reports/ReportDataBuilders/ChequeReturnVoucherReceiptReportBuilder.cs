using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessObject.Reports;
using ERPSMS_v01.Reports.ReportDataBuilders.ParameterBinder;
using ERPData;
using System.Data;
using System.Threading;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using BusinessObject.Reports.Factory;

namespace ERPSMS_v01.Reports.ReportDataBuilders
{
    public sealed class ChequeReturnVoucherReceiptReportBuilder : ReportDataBuilder
    {
         ChequeReturnVoucherRecept reportData;
         public ChequeReturnVoucherReceiptReportBuilder(string rptType)
            : base(rptType)
        {
            reportData = new ChequeReturnVoucherRecept();
        }

        public override string GetReportTemplate(IReportParameter reportParameter)
        {
            if (reportParameter == null) throw new ArgumentNullException("Null Parameter Found for Report  Builder {0}", this.GetType().Name);
            ChequeReturnVoucherReceiptReportParameter parameter = (ChequeReturnVoucherReceiptReportParameter)reportParameter;
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
        private void SetReportData(ChequeReturnVoucherReceiptReportParameter parameter)
        {
            SPFIN_RECEIPT_VOUCHER_RPT_Result reportDataSource = parameter.HeaderDtls[0];
            //GetCompany Info
            SPADM_COMPANY_MST_GET_KV_Result company = GetCompanyDetails(reportDataSource.FTH_COMPANY ?? 0);

            reportData.CompanyName = HttpUtility.HtmlDecode(company.CMP_NAME ?? CommonConstants.HTML_SPACE);
            reportData.Logo = company == null || company.CMP_LOGO.IsNullOrEmptyOrWhitespace() ? string.Empty : company.CMP_LOGO_URL.Replace("~/", "../");
            if (reportData.Logo.IsNullOrEmptyOrWhitespace()) reportData.HideLogo = "none";
            reportData.HideCompanyName = "none";

            reportData.ReportHeaderName = reportDataSource.FTH_STATUS.ToString() == "0"
                                            ? HttpUtility.HtmlDecode(reportData.HeadTitle) + " - Draft"
                                            : HttpUtility.HtmlDecode(reportData.HeadTitle);

            reportData.CustomerName = HttpUtility.HtmlDecode(reportDataSource.CUSTOMER_TEXT);
            reportData.TaxNo = reportDataSource.INV_NO;
            reportData.InvoiceDate = reportDataSource.INV_DATE.IsNullOrEmptyOrWhitespace()
                                    ? "-"
                                    : reportDataSource.INV_DATE;
            reportData.VoucherNo = reportDataSource.FTH_VOUCHER_NO;
            reportData.SaleContractNo = reportDataSource.CONTRACT_NO.IsNullOrEmptyOrWhitespace() ? "-" : reportDataSource.CONTRACT_NO;
            reportData.ContractDate = reportDataSource.CONTRACT_DATE.IsNullOrEmptyOrWhitespace() ? "-" : reportDataSource.CONTRACT_DATE;
            reportData.Date = reportDataSource.FTH_DATE.HasValue
                              ? reportDataSource.FTH_DATE.Value.ToString(reportData.DateFormat) : "-";

            reportData.Currency = reportDataSource.CURRENCY_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.Amount = reportDataSource.RCH_NET_AMOUNT.ToString(reportData.CurrencyFormat);
            reportData.Mode = reportDataSource.RCH_MODE_TEXT.Trim();
            reportData.SalesVoucherNo = reportDataSource.INV_VOUCHER_NO.IsNullOrEmptyOrWhitespace() ? "-" : reportDataSource.INV_VOUCHER_NO;
            reportData.InvoiceVoucherDate = reportDataSource.INV_VOUCHER_NO.IsNullOrEmptyOrWhitespace() 
                                ? "-" : Convert.ToDateTime(reportDataSource.INV_VOUCHER_DATE).ToString(reportData.DateFormat);
            reportData.TranRef = reportDataSource.RCH_NO;

            reportData.BaseCurrency = parameter.HeaderDtls[0].CURRENCY_TEXT;
            //reportData.BaseCurrency = parameter.AccountDtls[0].FTH_BASE_CURR_TEXT;


            decimal _totalDebitAmt = ((List<SPFIN_TRX_VOUCHER_RPT_Result>)parameter.AccountDtls).Sum(x => x.FTR_DR_AMT_BC ?? (decimal?)0) ?? 0;
            decimal _totalCreditAmt = ((List<SPFIN_TRX_VOUCHER_RPT_Result>)parameter.AccountDtls).Sum(x => x.FTR_CR_AMT_BC ?? (decimal?)0) ?? 0;

            reportData.DebitTotal = _totalDebitAmt.ToString(reportData.CurrencyFormat);
            reportData.CreditTotal = _totalCreditAmt.ToString(reportData.CurrencyFormat);


            //Setting Child Data
            string _baseCurrencyFraction = string.Empty;

            foreach (SPFIN_TRX_VOUCHER_RPT_Result subItem in parameter.AccountDtls)
            {
                if (_baseCurrencyFraction == string.Empty) _baseCurrencyFraction = subItem.FTH_BASE_CURR_FRACTION;
                reportData.ItemDetails.Add(new VoucherDetails
                {
                    AccCode = subItem.FTR_ACCOUNT_CODE,
                    AccName = HttpUtility.HtmlDecode(subItem.FTR_ACCOUNT_NAME),
                    Description = HttpUtility.HtmlDecode(subItem.FTR_NARRATION),
                    ExRate = (!subItem.FTR_EXCHG_RATE.HasValue || subItem.FTR_EXCHG_RATE.Value == 0 || subItem.FTR_EXCHG_RATE.Value == 1)
                                ? CommonConstants.HTML_SPACE
                                : subItem.FTR_EXCHG_RATE.Value.ToString(reportData.ExchangeRate),
                    Debit = subItem.FTR_DR_AMT_BC.HasValue && subItem.FTR_DR_AMT_BC.Value > 0
                                ? subItem.FTR_DR_AMT_BC.Value.ToString(reportData.CurrencyFormat)
                                : CommonConstants.HTML_SPACE,
                    Credit = subItem.FTR_CR_AMT_BC.HasValue && subItem.FTR_CR_AMT_BC.Value > 0
                                ? subItem.FTR_CR_AMT_BC.Value.ToString(reportData.CurrencyFormat)
                                : CommonConstants.HTML_SPACE
                });
            }

            reportData.AmountInWords = new NumberToWordsConvertorFactory(reportData.BaseCurrency)
                                           .GetNumberToWordsConvertor()
                                           .ConvertNumberToWords(reportData.DebitTotal.Replace(",",string.Empty))
                                           .Replace("Paise", _baseCurrencyFraction);

            reportData.Remarks = HttpUtility.HtmlDecode(reportDataSource.FTH_REMARKS) ?? CommonConstants.HTML_SPACE;
            reportData.HideRemarks = string.IsNullOrWhiteSpace(reportData.Remarks) ? "none" : "table-row";
            reportData.Narration = HttpUtility.HtmlDecode(reportDataSource.FTH_NARRATION);
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

        }
        #endregion

        #region Build Report Body
        private string buildReportBody()
        {
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
                                       reportData.HideSubTitle,          // 6
                                       reportData.SubTitle,              // 7
                                       reportData.CustomerName,          // 8
                                       reportData.TaxNo,                 // 9
                                       reportData.InvoiceDate,           // 10
                                       reportData.VoucherNo,             // 11 // PU NO                          
                                       reportData.SaleContractNo,        // 12
                                       reportData.ContractDate,          // 13            
                                       reportData.Date,                  // 14
                                       reportData.Currency,              // 15    
                                       reportData.Amount,                // 16
                                       reportData.Mode,                  // 17
                                       reportData.SalesVoucherNo,        // 18
                                       reportData.InvoiceVoucherDate,    // 19,
                                       reportData.BaseCurrency,          // 20
                                       _rowData,                         // 21
                                       reportData.DebitTotal,            // 22
                                       reportData.CreditTotal,           // 23
                                       reportData.AmountInWords,         // 24
                                       reportData.HideRemarks,           // 25
                                       reportData.Remarks,               // 26
                                       reportData.HideNarration,         // 27
                                       reportData.Narration,             // 28
                                       reportData.PreparedBy,            // 29
                                       reportData.PreparedByDate,        // 30
                                       reportData.ReviewedBy,            // 31
                                       reportData.ReviewedByDate,        // 32
                                       reportData.ApprovedBy,            // 33
                                       reportData.ApprovedByDate,        // 34
                                       reportData.FooterText             // 35
                                );


            #region Return Value
            string returnToList = "backToList('../journalize/JournalizeListing.aspx?Type=" + RptType + "');";

            return returnToList + "printVoucher(\"" + reoportBody.Replace("\"", "&quot;") + "\",{ width:'" + reportData.PrinterSettings.DocumentWidth + "', height:'" + reportData.PrinterSettings.DocumentHeight + "', windowWidth:'" + reportData.PrinterSettings.WindowWidth + "', windowHeight:'" + reportData.PrinterSettings.WindowHeight + "' }," + "'../Css/domatrix-printer-friendly-report-styles.css'" + ");";
            #endregion
        }
        #endregion

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