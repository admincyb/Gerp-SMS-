using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERPData;
using System.Data;
using BusinessObject.Reports;
using System.Threading;
using BusinessObject.CommonManagement;
using BusinessObject.Reports.Factory;
using ERP.Utilities;
using BusinessObject;
using ERPService;
using System.IO;
using ERPSMS_v01.Reports.ReportDataBuilders.ParameterBinder;

namespace ERPSMS_v01.Reports.ReportDataBuilders
{
    //Used Reports [Petty Cash, Direct Payment Voucher]
    public sealed class PettyCashReportBuilder : ReportDataBuilder //<List<SPFIN_TRX_VOUCHER_RPT_Result>>
    {
        #region Properties
        PettyCashVoucher reportData; // List<SPFIN_TRX_VOUCHER_RPT_Result> 
        #endregion

        #region Constructor
        public PettyCashReportBuilder(string rptType)
            : base(rptType)
        {
            reportData = new PettyCashVoucher();
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

        public override string GetReportTemplate(IReportParameter reportParameter)
        {
            CommonReportParameter parameter = (CommonReportParameter)reportParameter;
            #region Getting Report Parameters
            DataSet dsParamSettings;
            string footer;
            string rptName = string.Empty;
            footer = string.Empty;
            string signaturePath = string.Empty;
            dsParamSettings = new DataSet();

            dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(parameter.AppTypeDetailsList[0].AST_RPT_SETTINGS)));

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
            #region FormatCalculation
            string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            //string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
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
            int QuantityComma;
            int RateComma;
            int CurrencyComma;
            //DataTable dt = ConfigurationSettings();
            if (parameter.dataTable != null && parameter.dataTable.Rows.Count > 0)
            {
                int curdigit = Convert.ToInt32(parameter.dataTable.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                for (int i = 0; i < curdigit; i++)
                {
                    currencydecimals += "0";
                }

                int NoDigit = Convert.ToInt32(parameter.dataTable.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());
                for (int i = 0; i < NoDigit; i++)
                {
                    Nodecimal += "0";
                }
                int ExchRate = Convert.ToInt32(parameter.dataTable.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                for (int i = 0; i < ExchRate; i++)
                {
                    ExchRateDigit += "0";
                }

                int RateDecimal = Convert.ToInt32(parameter.dataTable.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                for (int i = 0; i < RateDecimal; i++)
                {
                    RateDecimalDigit += "0";
                }
                int RateDecimalPP = Convert.ToInt32(parameter.dataTable.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
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
            #endregion
            #region Setting Report Data
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

            reportData.FooterText = "Printed by " + parameter.PrintedUser + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");

            //List<SPFIN_TRX_VOUCHER_RPT_Result> reportDataSourceList = (List<SPFIN_TRX_VOUCHER_RPT_Result>)reportDataSourceListDynamic;
            //dynamic reportDataSourceList = reportDataSourceListDynamic;

            ////reportDataSourceList = currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK).ToList();
            SPFIN_TRX_VOUCHER_RPT_Result reportDataSource = parameter.reportDataSourceList[0];

            //=========================================
            #region Setting Report Parameters
            decimal Vat = 0;
            decimal beforeVat = 0;
            decimal WithHolding = 0;
            decimal VatBuyNotYetDue = 0;
            decimal NetAmount = 0;
            string PaidBy = string.Empty;
            if (parameter.reportDataSourceList != null && parameter.reportDataSourceList.Count > 0)
            {
                for (int i = 0; i <= parameter.reportDataSourceList.Count - 1; i++)
                {
                    if (parameter.reportDataSourceList[i].FTR_ACC_SUB_TYPE == (int)AccSubType.VatBuy)
                    {
                        decimal VatBuy = parameter.reportDataSourceList[i].FTR_DR_AMT_BC ?? 0;
                        Vat = Vat + VatBuy;
                        decimal NotYetDue = parameter.reportDataSourceList[i].FTR_CR_AMT_BC ?? 0;
                        VatBuyNotYetDue = VatBuyNotYetDue + NotYetDue;
                    }
                    if (parameter.reportDataSourceList[i].FTR_ACC_SUB_TYPE != (int)AccSubType.VatBuy
                        && parameter.reportDataSourceList[i].FTR_ACC_SUB_TYPE != (int)AccSubType.GainLossSales
                        && parameter.reportDataSourceList[i].FTR_ACC_SUB_TYPE != (int)AccSubType.BankCharge
                        && parameter.reportDataSourceList[i].FTR_ACC_SUB_TYPE != (int)AccSubType.GainLossPurchase
                        && parameter.reportDataSourceList[i].FTR_ACC_SUB_TYPE != (int)AccSubType.WHT)
                    {
                        decimal BeforeVat7 = parameter.reportDataSourceList[i].FTR_DR_AMT_BC ?? 0;
                        beforeVat = beforeVat + BeforeVat7;

                        decimal NetAmtCR = parameter.reportDataSourceList[i].FTR_CR_AMT_BC ?? 0;
                        NetAmount = NetAmount + NetAmtCR;
                    }
                    if (parameter.reportDataSourceList[i].FTR_ACC_SUB_TYPE == (int)AccSubType.WHT)
                    {
                        decimal WithHoldingTax = parameter.reportDataSourceList[i].FTR_CR_AMT_BC ?? 0;
                        WithHolding = WithHolding + WithHoldingTax;
                    }
                }
                beforeVat = beforeVat - VatBuyNotYetDue;

                // Find Paid By
                if (parameter.reportDataSourceList.Count > 0)
                {
                    for (int i = 0; i < parameter.reportDataSourceList.Count; i++)
                    {
                        if (parameter.reportDataSourceList[i].FTR_INSTR_NO != null && parameter.reportDataSourceList[i].FTR_INSTR_NO != string.Empty)
                        {
                            string strPaidBy = parameter.reportDataSourceList[i].FTR_INSTR_NO + " : " + Convert.ToDateTime(parameter.reportDataSourceList[i].FTR_INSTR_DATE).ToString(Resources.Constants.ReportDateFormat) + " : " + parameter.reportDataSourceList[i].FTR_INSTR_FAVOUR;
                            if (PaidBy == string.Empty)
                            {
                                PaidBy = strPaidBy;
                            }
                            else
                            {
                                PaidBy = PaidBy + CommonConstants.HTML_NEW_LINE + strPaidBy;
                            }
                        }
                    }
                }
                if (PaidBy == string.Empty)
                {
                    PaidBy = CommonConstants.HTML_SPACE;
                }
            }
            #endregion
            // ========================================

            //GetCompany Info
            SPADM_COMPANY_MST_GET_KV_Result company = GetCompanyDetails(reportDataSource.FTH_COMPANY);

            reportData.CompanyName = HttpUtility.HtmlDecode(company.CMP_NAME ?? CommonConstants.HTML_SPACE);
            reportData.CompanyAddress = GetCompanyAddress(company);
            reportData.ReportHeaderName = reportDataSource.FTH_STATUS.ToString() == "0"
                                            ? HttpUtility.HtmlDecode(reportData.HeadTitle) + " - Draft"
                                            : HttpUtility.HtmlDecode(reportData.HeadTitle);

            reportData.PaidTo = HttpUtility.HtmlDecode(reportDataSource.FTH_PARTY_NAME ?? CommonConstants.HTML_SPACE);
            reportData.VoucherNoLabelNmae = reportData.HeadTitle == "PETTY CASH VOUCHER" ? "Pc.No" : "PV.No";
            reportData.VoucherNo = reportDataSource.FTH_VOUCHER_NO.IsNullOrEmptyOrWhitespace() ? " -" : reportDataSource.FTH_VOUCHER_NO;
            reportData.PaidFor = HttpUtility.HtmlDecode(reportDataSource.FTH_REMARKS) ?? CommonConstants.HTML_SPACE;
            reportData.Date = reportDataSource.FTH_DATE.HasValue ? reportDataSource.FTH_DATE.Value.ToString(reportData.DateFormat) : " -";
            reportData.PaidBy = PaidBy == "0" ? CommonConstants.HTML_SPACE : HttpUtility.HtmlDecode(PaidBy);
            reportData.Currency = reportDataSource.FTH_TRX_CURR_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.BaseCurrency = reportDataSource.FTH_BASE_CURR_TEXT;


            //decimal? _totalDebitAmt = ((List<SPFIN_TRX_VOUCHER_RPT_Result>)reportDataSourceList).Sum(x => x.FTR_DR_AMT_BC ?? (decimal?)0) ?? 0;

            //Setting Child Data

            foreach (SPFIN_TRX_VOUCHER_RPT_Result subItem in parameter.reportDataSourceList)
            {
                reportData.ItemDetails.Add(new PettyCashVoucherItem
                {
                    GLCode = subItem.FTR_ACCOUNT_CODE,
                    GLName = subItem.FTR_ACCOUNT_NAME
                             + (CommonFunctions.IsNullOrEmptyOrWhitespace(subItem.FTR_NARRATION) ? CommonConstants.HTML_SPACE : HttpUtility.HtmlDecode(subItem.FTR_NARRATION))
                             + (CommonFunctions.IsNullOrEmptyOrWhitespace(subItem.FTR_INSTR_NO)
                                             ? CommonConstants.HTML_SPACE : " Cheque No. : " + HttpUtility.HtmlDecode(subItem.FTR_INSTR_NO) + " - ")
                             + ((!subItem.FTR_INSTR_DATE.HasValue || CommonFunctions.IsNullOrEmptyOrWhitespace(Convert.ToString(subItem.FTR_INSTR_DATE)))
                                             ? CommonConstants.HTML_SPACE : " Date : " + Convert.ToDateTime(subItem.FTR_INSTR_DATE).ToString(reportData.DateFormat)),
                    ExRate = subItem.FTR_EXCHG_RATE.HasValue && subItem.FTR_EXCHG_RATE.Value > 0 ? subItem.FTR_EXCHG_RATE.Value.ToString(reportData.ExchangeRate) : CommonConstants.HTML_SPACE,
                    Debit = subItem.FTR_DR_AMT_BC.HasValue && subItem.FTR_DR_AMT_BC.Value > 0 ? subItem.FTR_DR_AMT_BC.Value.ToString(currencyformat) : CommonConstants.HTML_SPACE,
                    Credit = subItem.FTR_CR_AMT_BC.HasValue && subItem.FTR_CR_AMT_BC.Value > 0 ? subItem.FTR_CR_AMT_BC.Value.ToString(currencyformat) : CommonConstants.HTML_SPACE
                });
            }
            reportData.AmountBeforeVat = beforeVat.ToString(reportData.CurrencyFormat);
            reportData.Vat = Vat.ToString(reportData.CurrencyFormat);
            reportData.Total = (beforeVat + Vat).ToString(reportData.CurrencyFormat);
            reportData.WithHoldingTax = WithHolding.ToString(reportData.CurrencyFormat);
            reportData.NetPayment = NetAmount.ToString(reportData.CurrencyFormat);

            reportData.HideVat = "table-row";//Vat > 0 ? "table-row" : "none";
            reportData.HideTotal = "table-row";//WithHolding > 0 || Vat > 0 ? "table-row" : "none";
            reportData.HideWithHolding = "table-row";//WithHolding > 0 ? "table-row" : "none";

            reportData.Amount = NetAmount.ToString(reportData.CurrencyFormat);

            reportData.AmountInWords = new NumberToWordsConvertorFactory(reportData.BaseCurrency)
                                           .GetNumberToWordsConvertor()
                                           .ConvertNumberToWords(reportData.Amount.Replace(",", string.Empty))
                                           .Replace("Paise", reportDataSource.FTH_BASE_CURR_FRACTION);

            reportData.Remarks = reportDataSource.FTH_REMARKS ?? CommonConstants.HTML_SPACE;
            reportData.HideRemarks = string.IsNullOrWhiteSpace(reportData.Remarks) ? "none" : "inherit";

            reportData.PreparedBy = reportDataSource.FTH_TASK1_BY_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.ReviewedBy = reportDataSource.FTH_TASK2_BY_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.VarifiedBy = reportDataSource.FTH_TASK3_BY_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.ApprovedBy = reportDataSource.FTH_TASK4_BY_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.ReceivedBy = CommonConstants.HTML_SPACE;
            reportData.HideVarifiedBy = reportData.HeadTitle == "PETTY CASH VOUCHER" ? "none" : "table-cell";

            reportData.PreparedByDate = reportDataSource.FTH_TASK1_DT_TEXT.HasValue
                                        ? reportDataSource.FTH_TASK1_DT_TEXT.Value.ToString(reportData.DateFormat)
                                        : CommonConstants.HTML_SPACE;
            reportData.ReviewedByDate = reportDataSource.FTH_TASK2_DT_TEXT.HasValue
                                        ? reportDataSource.FTH_TASK2_DT_TEXT.Value.ToString(reportData.DateFormat)
                                        : CommonConstants.HTML_SPACE;
            reportData.VarifiedByDate = reportDataSource.FTH_TASK3_DT_TEXT.HasValue
                                        ? reportDataSource.FTH_TASK3_DT_TEXT.Value.ToString(reportData.DateFormat)
                                        : CommonConstants.HTML_SPACE;
            reportData.ApprovedByDate = reportDataSource.FTH_TASK4_DT_TEXT.HasValue
                                       ? reportDataSource.FTH_TASK4_DT_TEXT.Value.ToString(reportData.DateFormat)
                                       : CommonConstants.HTML_SPACE;
            reportData.ReceivedByDate = CommonConstants.HTML_SPACE;
            reportData.FooterText = string.Format("Printed by {0} On {1}", parameter.PrintedUser, DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));
            #endregion
            #region Build Report Body

            string _rowData = string.Empty;
            foreach (var subItem in reportData.ItemDetails)
            {
                _rowData += string.Format(
                    reportData.RowTemplate,
                    subItem.GLCode,
                    subItem.GLName,
                    subItem.ExRate,
                    subItem.Debit,
                    subItem.Credit
                );
            };

            string reoportBody = string.Format(
                                       reportData.BodyTemplate,
                                       reportData.CompanyName,          // 0
                                       reportData.CompanyAddress,       // 1
                                       reportData.ReportHeaderName,     // 2
                                       reportData.PaidTo,               // 3
                                       reportData.VoucherNoLabelNmae,   // 4
                                       reportData.VoucherNo,            // 5
                                       reportData.PaidFor,              // 6
                                       reportData.Date,                 // 7
                                       reportData.PaidBy,               // 8
                                       reportData.Currency,             // 9
                                       reportData.Amount,               // 10
                                       reportData.BaseCurrency,         // 11
                                       _rowData,                        // 12
                                       reportData.AmountBeforeVat,      // 13
                                       reportData.HideVat,              // 14
                                       reportData.Vat,                  // 15
                                       reportData.HideTotal,            // 16
                                       reportData.Total,                // 17
                                       reportData.HideWithHolding,      // 18
                                       reportData.WithHoldingTax,       // 19
                                       reportData.NetPayment,           // 20
                                       reportData.AmountInWords,        // 21
                                       reportData.HideRemarks,          // 22
                                       reportData.Remarks,              // 23
                                       reportData.PreparedBy,           // 24
                                       reportData.PreparedByDate,       // 25
                                       reportData.ReviewedBy,           // 26
                                       reportData.ReviewedByDate,       // 27
                                       reportData.VarifiedBy,           // 28
                                       reportData.VarifiedByDate,       // 29
                                       reportData.ApprovedBy,           // 30
                                       reportData.ApprovedByDate,       // 31
                                       reportData.ReceivedBy,           // 32
                                       reportData.ReceivedByDate,       // 33
                                       reportData.FooterText,           // 34
                                       reportData.HideVarifiedBy        // 35
                                );
            #endregion
            #region Return Value
            string returnToList = "backToList('../journalize/JournalizeListing.aspx?Type=" + RptType + "');";

            return returnToList + "printVoucher(\"" + reoportBody + "\",{ width:'" + reportData.PrinterSettings.DocumentWidth + "', height:'" + reportData.PrinterSettings.DocumentHeight + "', windowWidth:'" + reportData.PrinterSettings.WindowWidth + "', windowHeight:'" + reportData.PrinterSettings.WindowHeight + "' }," + "'../Css/domatrix-printer-friendly-report-styles.css'" + ");";
            #endregion
        }
    }
}