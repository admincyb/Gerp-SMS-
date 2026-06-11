using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Reports
{
    public sealed class PettyCashVoucher
    {
        #region Constructor
        public PettyCashVoucher()
        {
            this.PrinterSettings = new PrinterSettings { DocumentWidth = "21cm", DocumentHeight = "14cm", WindowWidth = "793", WindowHeight = "529" };
            ItemDetails = new List<PettyCashVoucherItem>();
        }
        #endregion

        #region Head Session
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string ReportHeaderName { get; set; }
        public string PaidTo { get; set; }
        public string PaidFor { get; set; }
        public string PaidBy { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string VoucherNo { get; set; } // if ReportTitle="Petty Cash Voucher"=  PC.NO     else if ReportTitle="DIRECT PAYMENT VOUCHER" = PV.No : 
        public string Date { get; set; }
        public string VoucherNoLabelNmae { get; set; }
        public string BaseCurrency { get; set; }
        #endregion

        #region Body Session
        public List<PettyCashVoucherItem> ItemDetails { get; set; }

        public string AmountBeforeVat { get; set; }
        public string Vat { get; set; }
        public string Total { get; set; }
        public string NetPayment { get; set; }
        public string WithHoldingTax { get; set; }
        public string AmountInWords { get; set; }
        public string Remarks { get; set; }
        #endregion

        #region Footer Session
        public string PreparedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string VarifiedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string ReceivedBy { get; set; }

        public string PreparedByDate { get; set; }
        public string ReviewedByDate { get; set; }
        public string VarifiedByDate { get; set; }
        public string ApprovedByDate { get; set; }
        public string ReceivedByDate { get; set; }

        public string FooterText { get; set; }
        #endregion

        #region Visibility Flags
        public string HideRemarks { get; set; } // "inherit / none" 
        public string HideLogo { get; set; }
        public string HideHeadTitle { get; set; }
        public string HideSubTitle { get; set; }
        public string HideFooterText { get; set; }
        public string HidePageNo { get; set; }
        public string HeadTitle { get; set; }
        public string SubTitle { get; set; }
        public string HideVat { get; set; }
        public string HideTotal { get; set; }
        public string HideWithHolding { get; set; }
        public string HideVarifiedBy { get; set; }
        #endregion

        #region Format Attribuites
        public string DateFormat { get; set; }
        public string CurrencyFormat { get; set; }
        public string NumberFormat { get; set; }
        public string RateFormat { get; set; }
        public string ExchangeRate { get; set; } // No Of Digits 
        #endregion

        #region Printer Settings
        public PrinterSettings PrinterSettings { get; set; }
        #endregion        

        #region ReadOnly Properties
        public string BodyTemplate { get { return reportTemplate; } }
        public string RowTemplate { get { return rowTemplate; } } 
        #endregion

        #region HtmlTemplate
        private const string reportTemplate = "<div id='printContainer'>" +
                                "<table border='0' cellpadding='0' cellspacing='0' class='table-top'>" +
                                    //"<tr>" +
                                    //    "<th colspan='6' align='center' class='table-heading-large'>" +
                                    //        "{0}" +
                                    //    "</th>" +
                                    //"</tr>" +
                                    //"<tr>" +
                                    //    "<td colspan='6' align='center'>" +
                                    //       "{1}" +
                                    //    "</td>" +
                                    //"</tr>" +
                                    "<tr>" +
                                        "<th colspan='6' align='center' class='table-heading-large'>" +
                                           "<span class='title-font'><b>{2}</b></span>" +
                                        "</th>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td valign='top'>" +
                                            "Paid To&nbsp;&nbsp;:&nbsp;" +
                                        "</td>" +
                                        "<td valign='top'>" +
                                           "<b>{3}</b>" +
                                        "</td>" +
                                        "<td>" +
                                        "</td>" +
                                        "<td>" +
                                        "</td>" +
                                        "<td valign='top'>" +
                                            "{4}" + // Voucher Label Name { 'PC.No' , 'PV.No' }
                                        "</td>" +
                                        "<td valign='top' >" +
                                            ": <b>{5}</b>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td class='table-top-col-first' valign='top'>" +
                                            "Paid For&nbsp;:&nbsp;" +
                                        "</td>" +
                                        "<td class='table-top-col-second' valign='top'>" +
                                            "{6}" +
                                        "</td>" +
                                        "<td class='table-top-col-third' valign='top'>" +
                                        "</td>" +
                                        "<td class='table-top-col-forth' valign='top'>" +
                                        "</td>" +
                                        "<td class='table-top-col-fifth' valign='top'>" +
                                            "Date" +
                                        "</td>" +
                                        "<td class='table-top-col-sixth' valign='top'>" +
                                            ": <b>{7}</b>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td valign='top'>" +
                                            "Paid By&nbsp;&nbsp;:&nbsp;" +
                                        "</td>" +
                                        "<td valign='top'>" +
                                            "{8}" +
                                        "</td>" +
                                        "<td valign='top'>" +
                                        "</td>" +
                                        "<td>" +
                                        "</td>" +
                                        "<td>" +
                                        "</td>" +
                                        "<td>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td valign='top'>" +
                                            "Currency&nbsp;:&nbsp;" +
                                        "</td>" +
                                        "<td valign='top'>" +
                                            "{9}" +
                                        "</td>" +
                                        "<td>" +
                                            "Amount" +
                                        "</td>" +
                                        "<td valign='top'>" +
                                            ": {10}" +
                                        "</td>" +
                                        "<td>" +
                                        "</td>" +
                                        "<td>" +
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
                                "<div class='table-heading-large'>" +
                                 "Accounts Recording" +
                                "</div>" +
                                "<table cellpadding='0' cellspacing='0' class='table-grid'>" +
                                    "<thead>" +
                                        "<tr>" +
                                            "<th class='table-grid-col-first text-left-aligh'>" +
                                               " G/L Code" +
                                            "</th>" +
                                            "<th class='table-grid-col-second text-left-aligh' colspan='2'>" +
                                                "G/L Name" +
                                            "</th>" +
                                            /*"<th class='table-grid-col-third right-align-field'>" +
                                                "Ex.Rate" +
                                            "</th>" +*/
                                            "<th class='table-grid-col-forth right-align-field'>" +
                                                "Debit({11})" +
                                            "</th>" +
                                            "<th class='table-grid-col-fifth right-align-field'>" +
                                                "Credit({11})" +
                                            "</th>" +
                                        "</tr>" +
                                    "</thead>" +
                                    "<tbody>" +
                                        "{12}"+
                                    "</tbody>" +
                                    "<tfoot>" +
                                        "<tr>" +
                                            "<th colspan='4'  class='right-align-field'>" +
                                                "Amount Before Vat 7%" +
                                            "</th>" +
                                            "<th  class='right-align-field'>" +
                                                "{13}" +
                                            "</th>" +
                                        "</tr>" +
                                        "<tr style='display: {14}'>" +
                                            "<th colspan='4'  class='right-align-field'>" +
                                               "Vat 7%" +
                                            "</th>" +
                                            "<th  class='right-align-field'>" +
                                                "{15}" +
                                            "</th>" +
                                        "</tr>" +
                                        "<tr style='display: {16}'>" +
                                            "<th colspan='4'  class='right-align-field'>" +
                                                "Total" +
                                            "</th>" +
                                            "<th  class='right-align-field'>" +
                                                "{17}" +
                                            "</th>" +
                                        "</tr>" +
                                       "<tr style='display: {18}'>" +
                                            "<th colspan='4'  class='right-align-field'>" +
                                                "With Holding Tax" +
                                            "</th>" +
                                            "<th  class='right-align-field'>" +
                                                "{19}" +
                                            "</th>" +
                                        "</tr>" +
                                       "<tr>" +
                                            "<th colspan='4'  class='right-align-field'>" +
                                                "Net Payment" +
                                            "</th>" +
                                            "<th  class='right-align-field'>" +
                                                "{20}" +
                                            "</th>" +
                                        "</tr>" +
                                        /*"<tr>" +
                                            "<td colspan='5'>" +
                                                "Amount in words : <span>{21}</span>" +
                                            "</td>" +
                                        "</tr>" +*/
                                    "</tfoot>" +
                                "</table>" +
                                "<table border='0' class='table-remarks' >" +
                                    "<tr style='display: {22};'>" +
                                        "<td class='table-remarks-caption-col text-left-align' valign='top'>" +
                                            "Remarks" +
                                        "</td>" +
                                        "<td class='col-separator' align='center' valign='top'>" +
                                            ":" +
                                        "</td>" +
                                        "<td class='text-left-align' valign='top'>" +
                                            "{23}" +
                                        "</td>" +
                                    "</tr>" +     
                                "</table>" +
                                "<table class='table-approve-five'>" +
                                    "<tr>" +
                                        "<td class='table-approve-five-col-normal'>" +
                                            "{24}" +
                                            "<hr />" +
                                            "<b>Prepared By</b>" +
                                            "<p>" +
                                               " Date : {25}</p>" +
                                        "</td>" +
                                        /*"<td class='table-approve-five-col-separator'>&nbsp;</td>" +
                                        "<td class='table-approve-five-col-normal'>" +
                                            "{26}" +
                                            "<hr />" +
                                            "<b>Reviewed By</b>" +
                                            "<p>" +
                                                "Date : {27}</p>" +
                                        "</td>" +*/
                                        "<td class='table-approve-five-col-separator'>&nbsp;</td>" +
                                        "<td class='table-approve-five-col-normal' style='display: {35}'>" +
                                            "{28}" +
                                            "<hr />" +
                                            "<b>Varifeid By</b>" +
                                            "<p>" +
                                                "Date : {29}</p>" +
                                        "</td>" +
                                        "<td class='table-approve-five-col-separator'>&nbsp;</td>" +
                                        "<td class='table-approve-five-col-normal'>" +
                                            "{30}" +
                                            "<hr />" +
                                            "<b>Approved By</b>" +
                                            "<p>" +
                                                "Date : {31}</p>" +
                                        "</td>" +
                                        "<td class='table-approve-five-col-separator'>&nbsp;</td>" +
                                        "<td class='table-approve-five-col-normal'>" +
                                            "{32}" +
                                            "<hr />" +
                                            "<b>Received By</b>" +
                                            "<p>" +
                                                "Date : {33}</p>" +
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
                                //"<div class='footer'>" +
                                //    "{34}" +
                                //"</div>" +
                            "</div>";

        private const string rowTemplate = "<tr>" +
                                "<td valign='top'>{0}" +
                                "</td>" +
                                "<td colspan='2' valign='top'>{1}" +
                                "</td>" +
                                /*"<td class='right-align-field' >{2}" +
                                "</td>" +*/
                                "<td class='right-align-field' valign='top' >{3}" +
                                "</td>" +
                                "<td class='right-align-field' valign='top' >{4}" +
                                "</td>" +
                            "</tr>";
        
        #endregion
    }

    public class PettyCashVoucherItem
    {
        #region Properties
        public string GLCode { get; set; }
        public string GLName { get; set; }
        public string ExRate { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        #endregion
    }
}
