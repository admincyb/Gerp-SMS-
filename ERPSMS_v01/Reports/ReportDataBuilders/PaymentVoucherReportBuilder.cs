using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERPData;
using System.Data;
using ERP.Utilities;
using BusinessObject.Reports;
using BusinessObject.Reports.Factory;
using BusinessObject.CommonManagement;
using BusinessObject;
using System.Threading;
using ERPSMS_v01.Reports.ReportDataBuilders.ParameterBinder;

namespace ERPSMS_v01.Reports.ReportDataBuilders
{
    public sealed class PaymentVoucherReportBuilder : ReportDataBuilder
    {
        #region Properties
        PaymentVoucher reportData; 
        #endregion

        #region Constructor
        public PaymentVoucherReportBuilder(string rptType)
            : base(rptType)
        {
            reportData = new PaymentVoucher();
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
            PaymentVoucherReportParameter parameter = (PaymentVoucherReportParameter)reportParameter;
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

            reportData.FooterText = "Printed by " + parameter.PrintedUser+ " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");

            //List<SPFIN_TRX_VOUCHER_RPT_Result> reportDataSourceList = (List<SPFIN_TRX_VOUCHER_RPT_Result>)reportDataSourceListDynamic;
            //List<SPFIN_PAYMENT_VND_VOUCHER_RPT_Result> reportDataSourceList = reportDataSourceListDynamic;

            //reportDataSourceList = currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK).ToList();
            SPFIN_PAYMENT_VND_VOUCHER_RPT_Result reportDataSource = parameter.reportDataSourceList[0];
            //List<SPFIN_TRX_VOUCHER_RPT_Result> additionalReportDataSourceList = additionalDataSource;
            SPFIN_TRX_VOUCHER_RPT_Result additionalReportDataSource = parameter.additionalReportDataSourceList[0];
            //=========================================
            #region Setting Report Parameters
            decimal Vat = 0;
            decimal beforeVat = 0;
            decimal WithHolding = 0;
            decimal VatBuyNotYetDue = 0;
            decimal NetAmount = 0;
            if (parameter.additionalReportDataSourceList != null && parameter.additionalReportDataSourceList.Count > 0)
            {
                for (int i = 0; i < parameter.additionalReportDataSourceList.Count ; i++)
                {
                    if (parameter.additionalReportDataSourceList[i].FTR_ACC_SUB_TYPE == (int)AccSubType.VatBuy)
                    {
                        decimal VatBuy = parameter.additionalReportDataSourceList[i].FTR_DR_AMT_BC ?? 0;
                        Vat = Vat + VatBuy;
                        decimal NotYetDue = parameter.additionalReportDataSourceList[i].FTR_CR_AMT_BC ?? 0;
                        VatBuyNotYetDue = VatBuyNotYetDue + NotYetDue;
                    }
                    if (parameter.additionalReportDataSourceList[i].FTR_ACC_SUB_TYPE != (int)AccSubType.VatBuy
                        && parameter.additionalReportDataSourceList[i].FTR_ACC_SUB_TYPE != (int)AccSubType.GainLossSales
                        && parameter.additionalReportDataSourceList[i].FTR_ACC_SUB_TYPE != (int)AccSubType.BankCharge
                        && parameter.additionalReportDataSourceList[i].FTR_ACC_SUB_TYPE != (int)AccSubType.GainLossPurchase
                        && parameter.additionalReportDataSourceList[i].FTR_ACC_SUB_TYPE != (int)AccSubType.WHT)
                    {
                        decimal BeforeVat7 = parameter.additionalReportDataSourceList[i].FTR_DR_AMT_BC ?? 0;
                        beforeVat = beforeVat + BeforeVat7;

                        decimal NetAmtCR = parameter.additionalReportDataSourceList[i].FTR_CR_AMT_BC ?? 0;
                        NetAmount = NetAmount + NetAmtCR;
                    }
                    if (parameter.additionalReportDataSourceList[i].FTR_ACC_SUB_TYPE == (int)AccSubType.WHT)
                    {
                        decimal WithHoldingTax = parameter.additionalReportDataSourceList[i].FTR_CR_AMT_BC ?? 0;
                        WithHolding = WithHolding + WithHoldingTax;
                    }
                }
                beforeVat = beforeVat - VatBuyNotYetDue;
            }
            #endregion
            // ========================================

            //GetCompany Info
            SPADM_COMPANY_MST_GET_KV_Result company = GetCompanyDetails(reportDataSource.PVH_COMPANY);
            reportData.CompanyName = HttpUtility.HtmlDecode(company.CMP_NAME ?? CommonConstants.HTML_SPACE);
            reportData.CompanyAddress = GetCompanyAddress(company);


            reportData.ReportHeaderName = reportDataSource.FTH_STATUS.ToString() == "0"
                                            ? HttpUtility.HtmlDecode(reportData.HeadTitle) + " - Draft"
                                            : HttpUtility.HtmlDecode(reportData.HeadTitle);

            reportData.PaidTo = HttpUtility.HtmlDecode(reportDataSource.PVH_VENDOR_TEXT ?? CommonConstants.HTML_SPACE);
            reportData.VoucherLabel = "PV.No";
            reportData.VoucherNo = reportDataSource.FTH_VOUCHER_NO ?? " -";
            reportData.PaidFor = HttpUtility.HtmlDecode(reportDataSource.IVH_PAID_FOR) ?? CommonConstants.HTML_SPACE;
            reportData.Date = reportDataSource.FTH_DATE.HasValue ? reportDataSource.FTH_DATE.Value.ToString(reportData.DateFormat) : " -";
            reportData.PaidBy = HttpUtility.HtmlDecode(reportDataSource.PVH_MODE_TEXT) +
                                    (reportDataSource.PVH_MODE == 2
                                        ? " - " + HttpUtility.HtmlDecode(reportDataSource.PVH_BANK_NAME)
                                            : CommonConstants.HTML_SPACE);
            reportData.ChqLabel = reportDataSource.PVH_MODE.ToString() == "2"
                                        ? "Chq.# "
                                        : reportDataSource.PVH_MODE.ToString() == "3"
                                            ? "DD# "
                                            : CommonConstants.HTML_SPACE;
            reportData.ChequeNo = reportDataSource.PVH_MODE.ToString() == "2"
                                        ? reportDataSource.PVH_INSTR_NO
                                        : reportDataSource.PVH_MODE.ToString() == "3"
                                            ? reportDataSource.PVH_INSTR_NO
                                            : CommonConstants.HTML_SPACE;
            reportData.ChqDateLabel = reportDataSource.PVH_MODE.ToString() == "2"
                                        ? "Date"
                                        : reportDataSource.PVH_MODE.ToString() == "3"
                                            ? "Date"
                                            : CommonConstants.HTML_SPACE;
            reportData.ChqDate = reportDataSource.PVH_MODE.ToString() == "2" || reportDataSource.PVH_MODE.ToString() == "3"
                                        ? reportDataSource.PVH_INSTR_DATE.Value.ToString(reportData.DateFormat)
                                        : CommonConstants.HTML_SPACE;

            reportData.Currency = additionalReportDataSource.FTH_TRX_CURR_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.BaseCurrency = additionalReportDataSource.FTH_BASE_CURR_TEXT;

            reportData.Amount = parameter.reportDataSourceList.Sum(x => x.PVM_PAID_AMOUNT_TC??0).ToString(reportData.CurrencyFormat); 
            reportData.TrnRefNo = reportDataSource.PVH_NO ?? CommonConstants.HTML_SPACE;
            reportData.BillItemBaseCurrency = reportDataSource.PVM_BASE_CURR_TEXT;

            foreach (SPFIN_PAYMENT_VND_VOUCHER_RPT_Result subItem in parameter.reportDataSourceList)
            {
                reportData.BillItems.Add(new BillDetails
                {
                    No = subItem.PVM_SL_NO.HasValue ? subItem.PVM_SL_NO.Value.ToString() : CommonConstants.HTML_SPACE,
                    BillNo = subItem.PVM_INV_VENDOR_NO,
                    Date = Convert.ToDateTime(subItem.PVM_INV_DATE).ToString(reportData.DateFormat),
                    DueDate = Convert.ToDateTime(subItem.PVM_INV_DATE_PAY_BY).ToString(reportData.DateFormat),
                    TotalAmtTax = subItem.PVM_IVH_TAX_TC.HasValue
                                    ? subItem.PVM_IVH_TAX_TC.Value.ToString(reportData.CurrencyFormat)
                                    : CommonConstants.HTML_SPACE,
                    TotalAmtAmount = subItem.PVM_INV_AMOUNT_NET_TC.HasValue
                                    ? subItem.PVM_INV_AMOUNT_NET_TC.Value.ToString(reportData.CurrencyFormat)
                                    : CommonConstants.HTML_SPACE,
                    TotalAmtRate = subItem.PVM_EXCHG_RATE.HasValue
                                    ? subItem.PVM_EXCHG_RATE.Value.ToString(reportData.ExchangeRate)
                                    : CommonConstants.HTML_SPACE,
                    TotalAmtCurrncy = subItem.PVM_INV_AMOUNT_NET_TC.HasValue
                                        ? (subItem.PVM_INV_AMOUNT_NET_TC.Value * Convert.ToDecimal(subItem.PVM_EXCHG_RATE)).ToString(reportData.CurrencyFormat)
                                        : CommonConstants.HTML_SPACE,
                    BalAmtAmount = subItem.PVM_INV_AMOUNT_BAL_TC.HasValue
                                    ? subItem.PVM_INV_AMOUNT_BAL_TC.Value.ToString(reportData.CurrencyFormat)
                                    : CommonConstants.HTML_SPACE,
                    BalAmtRate = subItem.PVM_EXCHG_RATE.HasValue
                                    ? subItem.PVM_EXCHG_RATE.Value.ToString(reportData.ExchangeRate)
                                    : CommonConstants.HTML_SPACE,
                    BalAmtCurrncy = subItem.PVM_INV_AMOUNT_BAL_TC.HasValue
                                    ? (subItem.PVM_INV_AMOUNT_BAL_TC.Value * Convert.ToDecimal(subItem.PVM_EXCHG_RATE)).ToString(reportData.CurrencyFormat)
                                    : CommonConstants.HTML_SPACE,
                    PaymentAmtAmount = subItem.PVM_PAID_AMOUNT_TC.HasValue
                                    ? subItem.PVM_PAID_AMOUNT_TC.Value.ToString(reportData.CurrencyFormat)
                                    : CommonConstants.HTML_SPACE,
                    PaymentAmtRate = subItem.PVM_EXCHG_RATE.HasValue
                                    ? subItem.PVM_EXCHG_RATE.Value.ToString(reportData.ExchangeRate)
                                    : CommonConstants.HTML_SPACE,
                    PaymentAmtCurrncy = (subItem.PVM_PAID_AMOUNT_TC * Convert.ToDecimal(subItem.PVM_EXCHG_RATE)).Value.ToString(reportData.CurrencyFormat)
                });
            }

            reportData.AmountBeforeVat = beforeVat.ToString(reportData.CurrencyFormat);
            string baseCurrencyFraction=string.Empty;
            foreach (SPFIN_TRX_VOUCHER_RPT_Result subItem in parameter.additionalReportDataSourceList)
            {
                if(baseCurrencyFraction == string.Empty) baseCurrencyFraction=subItem.FTH_BASE_CURR_FRACTION;
                reportData.ItemDetails.Add(new PaymentVoucherItem
                {
                    GLCode = subItem.FTR_ACCOUNT_CODE,
                    GLName = subItem.FTR_ACCOUNT_NAME
                             + (CommonFunctions.IsNullOrEmptyOrWhitespace(subItem.FTR_NARRATION)
                                    ? CommonConstants.HTML_SPACE
                                    : HttpUtility.HtmlDecode(subItem.FTR_NARRATION)),
                    ExRate = subItem.FTR_EXCHG_RATE.HasValue || subItem.FTR_EXCHG_RATE.Value == 0 || subItem.FTR_EXCHG_RATE.Value == 1
                                    ? CommonConstants.HTML_SPACE
                                    : subItem.FTR_EXCHG_RATE.Value.ToString(reportData.ExchangeRate),
                    Debit = subItem.FTR_DR_AMT_BC.HasValue && subItem.FTR_DR_AMT_BC.Value > 0
                                    ? subItem.FTR_DR_AMT_BC.Value.ToString(currencyformat)
                                    : CommonConstants.HTML_SPACE,
                    Credit = subItem.FTR_CR_AMT_BC.HasValue && subItem.FTR_CR_AMT_BC.Value > 0 ? subItem.FTR_CR_AMT_BC.Value.ToString(currencyformat) : CommonConstants.HTML_SPACE
                });
            }
            reportData.AmountBeforeVat = beforeVat.ToString(reportData.CurrencyFormat);
            reportData.Vat = Vat.ToString(reportData.CurrencyFormat);
            reportData.Total = (beforeVat + Vat).ToString(reportData.CurrencyFormat);
            reportData.WithHoldingTax = WithHolding.ToString(reportData.CurrencyFormat);
            reportData.NetPayment = NetAmount.ToString(reportData.CurrencyFormat);

            reportData.Amount = NetAmount.ToString(reportData.CurrencyFormat);

            reportData.AmountInWords = new NumberToWordsConvertorFactory(reportData.BaseCurrency)
                                           .GetNumberToWordsConvertor()
                                           .ConvertNumberToWords(reportData.Amount.Replace(",",string.Empty))
                                           .Replace("Paise", baseCurrencyFraction);

            reportData.Remarks = reportDataSource.FTH_REMARKS ?? CommonConstants.HTML_SPACE;
            reportData.HideRemarks = string.IsNullOrWhiteSpace(reportData.Remarks) ? "none" : "table-row";
            reportData.Narration = reportDataSource.FTH_NARRATION ?? CommonConstants.HTML_SPACE;
            reportData.HideNarration = string.IsNullOrWhiteSpace(reportData.Narration) ? "none" : "table-row";

            //reportData.PreparedBy = reportDataSource.FTH_TASK1_BY_TEXT ?? CommonConstants.HTML_SPACE;
            //reportData.ReviewedBy = reportDataSource.FTH_TASK2_BY_TEXT ?? CommonConstants.HTML_SPACE;
            //reportData.VarifiedBy = reportDataSource.FTH_TASK3_BY_TEXT ?? CommonConstants.HTML_SPACE;
            //reportData.ApprovedBy = reportDataSource.FTH_TASK4_BY_TEXT ?? CommonConstants.HTML_SPACE;
            //reportData.ReceivedBy = CommonConstants.HTML_SPACE;

            reportData.PreparedBy = reportDataSource.FTH_TASK1_BY_TEXT ?? CommonConstants.HTML_SPACE;            
            reportData.ApprovedBy = reportDataSource.FTH_TASK3_BY_TEXT ?? CommonConstants.HTML_SPACE;
            reportData.ReceivedBy = CommonConstants.HTML_SPACE;

            /*reportData.PreparedByDate = reportDataSource.FTH_TASK1_DT_TEXT.HasValue
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
            reportData.ReceivedByDate = CommonConstants.HTML_SPACE;*/

            reportData.PreparedByDate = reportDataSource.FTH_TASK1_DT_TEXT.HasValue
                                       ? reportDataSource.FTH_TASK1_DT_TEXT.Value.ToString(reportData.DateFormat)
                                       : CommonConstants.HTML_SPACE;        
           reportData.ApprovedByDate = reportDataSource.FTH_TASK3_DT_TEXT.HasValue
                                      ? reportDataSource.FTH_TASK3_DT_TEXT.Value.ToString(reportData.DateFormat)
                                      : CommonConstants.HTML_SPACE;
           reportData.ReceivedByDate = CommonConstants.HTML_SPACE;
            reportData.FooterText = string.Format("Printed by {0} On {1}", parameter.PrintedUser, DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));
            #endregion
            #region Build Report Body

            string _rowData = string.Empty;
            string _billItemRowData = string.Empty;

            foreach (var subItem in reportData.BillItems)
            {
                _billItemRowData += string.Format(
                    reportData.BillItemRowTemplate,
                    subItem.No,
                    subItem.BillNo,
                    subItem.Date,
                    subItem.DueDate,
                    subItem.TotalAmtTax,
                    subItem.TotalAmtAmount,
                    subItem.TotalAmtRate,
                    subItem.TotalAmtCurrncy,
                    subItem.BalAmtAmount,
                    subItem.BalAmtRate,
                    subItem.BalAmtCurrncy,
                    subItem.PaymentAmtAmount,
                    subItem.PaymentAmtRate,
                    subItem.PaymentAmtCurrncy
                );
            };


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
                                       reportData.CompanyName,           // 0
                                       reportData.CompanyAddress,        // 1
                                       reportData.HeadTitle,             // 2
                                       reportData.PaidTo,                // 3
                                       reportData.VoucherLabel,          // 4
                                       reportData.VoucherNo,             // 5
                                       reportData.PaidFor,               // 6
                                       reportData.Date,                  // 7
                                       reportData.PaidBy,                // 8
                                       reportData.ChqLabel,              // 9
                                       reportData.ChequeNo,              // 10
                                       reportData.ChqDateLabel,          // 11
                                       reportData.ChqDate,               // 12 
                                       reportData.Currency,              // 13
                                       reportData.Amount,                // 14
                                       reportData.TrnRefNo,              // 15  
                                       reportData.BillItemBaseCurrency,  // 16  
                                       _billItemRowData,                 // 17
                                       reportData.BaseCurrency,          // 18
                                       _rowData,                         // 19
                                       reportData.AmountBeforeVat,       // 20
                                       reportData.Vat,                   // 21
                                       reportData.Total,                 // 22
                                       reportData.WithHoldingTax,        // 23
                                       reportData.NetPayment,            // 24
                                       reportData.AmountInWords,         // 25
                                       reportData.HideRemarks,           // 26
                                       reportData.Remarks,               // 27
                                       reportData.HideNarration,         // 28
                                       reportData.Narration,             // 29
                                       reportData.PreparedBy,            // 30
                                       reportData.PreparedByDate,        // 31
                                       reportData.ReviewedBy,            // 32
                                       reportData.ReviewedByDate,        // 33
                                       reportData.VarifiedBy,            // 34
                                       reportData.VarifiedByDate,        // 35
                                       reportData.ApprovedBy,            // 36
                                       reportData.ApprovedByDate,        // 37
                                       reportData.ReceivedBy,            // 38
                                       reportData.ReceivedByDate,        // 39
                                       reportData.FooterText             // 40
                                );
            #endregion
            #region Return Value
            string returnToList = "backToList('../journalize/JournalizeListing.aspx?Type=" + RptType + "');";

            return returnToList + "printVoucher(\"" + reoportBody.Replace("\"", "&quot;") + "\",{ width:'" + reportData.PrinterSettings.DocumentWidth + "', height:'" + reportData.PrinterSettings.DocumentHeight + "', windowWidth:'" + reportData.PrinterSettings.WindowWidth + "', windowHeight:'" + reportData.PrinterSettings.WindowHeight + "' }," + "'../Css/domatrix-printer-friendly-report-styles.css'" + ");";
            #endregion
        }
    }
}