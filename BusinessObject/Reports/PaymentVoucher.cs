using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Reports
{
    public sealed class PaymentVoucher
    {
        #region Constructor
        public PaymentVoucher()
        {
            BillItems = new List<BillDetails>();
            ItemDetails = new List<PaymentVoucherItem>();
            this.PrinterSettings = new PrinterSettings { DocumentWidth = "21cm", DocumentHeight = "14cm", WindowWidth = "793", WindowHeight = "529" };
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
        public string VoucherLabel { get; set; }
        public string BaseCurrency { get; set; }
        public string ChqLabel { get; set; }
        public string ChequeNo { get; set; }
        public string ChqDateLabel { get; set; }
        public string ChqDate { get; set; }
        public string TrnRefNo { get; set; }
        #endregion

        #region Body Session
        public string BillItemBaseCurrency { get; set; }
        public List<BillDetails> BillItems { get; set; }
        public List<PaymentVoucherItem> ItemDetails { get; set; }

        public string AmountBeforeVat { get; set; }
        public string Vat { get; set; }
        public string Total { get; set; }
        public string NetPayment { get; set; }
        public string WithHoldingTax { get; set; }
        public string AmountInWords { get; set; }
        public string Remarks { get; set; }
        public string Narration { get; set; }
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
        public string HideRemarks { get; set; }     // "table-row / none" 
        public string HideNarration { get; set; }   // "table-row / none" 
        public string HideLogo { get; set; }        // "inherit / none" 
        public string HideHeadTitle { get; set; }
        public string HideSubTitle { get; set; }
        public string HideFooterText { get; set; }
        public string HidePageNo { get; set; }
        public string HeadTitle { get; set; }
        public string SubTitle { get; set; }
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
        public string BillItemRowTemplate { get { return billItemRowTemplate; } } 
        #endregion

        #region HtmlTemplate
        private const string reportTemplate = "<div id='printContainer' >" +
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
                                           "<span style='font-size:12pt;font-weight:bold;'>{2}</span>" +
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
                                        "<td>" +
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
                                        "<td class='table-top-col-third' valign='top' >" +
                                        "</td>" +
                                        "<td class='table-top-col-forth' valign='top' >" +
                                        "</td>" +
                                        "<td class='table-top-col-fifth' valign='top'>" +
                                            "Date" +
                                        "</td>" +
                                        "<td class='table-top-col-sixth' valign='top' >" +
                                            ": <b>{7}</b>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td valign='top' valign='top'>" +
                                            "Paid By&nbsp;&nbsp;:&nbsp;" +
                                        "</td>" +
                                        "<td valign='top' >" +
                                            "{8}" +
                                        "</td>" +
                                        "<td valign='top' >" +
                                            "{9}" +
                                        "</td>" +
                                        "<td valign='top' >" +
                                            ": {10}" +
                                        "</td>" +
                                        "<td valign='top' >" +
                                            "{11}" +
                                        "</td>" +
                                        "<td valign='top' >" +
                                            ": {12}" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td valign='top' >" +
                                            "Currency&nbsp;&nbsp;:" +
                                        "</td>" +
                                        "<td valign='top' >" +
                                            "{13}" +
                                        "</td>" +
                                        "<td valign='top' >" +
                                            "Amount" +
                                        "</td>" +
                                        "<td valign='top' >" +
                                            ": {14}" +
                                        "</td>" +
                                        "<td valign='top' >" +
                                            "Trn.Ref."+
                                        "</td>" +
                                        "<td valign='top' >" +
                                            ": {15}" +
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
                                 "<table border='1' class='table-forteen'>" +
                                    "<thead>" +
                                        "<tr>" +
                                            "<th rowspan='2' valign='top' class='forteen-col-small'>" +
                                                "No" +
                                            "</th>" +
                                            "<th rowspan='2' valign='top'  class='forteen-col-large'>" +
                                                "Bill#" +
                                            "</th>" +
                                            "<th rowspan='2' valign='top'  class='forteen-col-large'>" +
                                                "Date" +
                                            "</th>" +
                                            /*"<th rowspan='2' valign='top'  class='forteen-col-large'>" +
                                                "Due Date" +
                                            "</th>" +*/
                                            "<th colspan='3' align='center'>" +
                                                "Total Amount" +
                                            "</th>"+
                                            "<th colspan='3' align='center'>" +
                                                "Balance Amount" +
                                            "</th>" +
                                            "<th colspan='3' align='center'>" +
                                                "Payment Amount" +
                                            "</th>" +
                                        "</tr>" +
                                        "<tr>" +                
                                            "<td class='forteen-col-medium right-align-field'>" +
                                                "Tax" +
                                            "</td>" +
                                            "<td class='forteen-col-large right-align-field'>" +
                                                "Tot.Amt" +
                                            "</td>" +
                                            /*"<td class='forteen-col-large right-align-field'>" +
                                                "Rate" +
                                            "</td>" +*/
                                            "<td class='forteen-col-large right-align-field'>" +
                                                "{16}" +
                                            "</td>" +
                                            "<td class='forteen-col-large right-align-field'>" +
                                                "Amt" +
                                            "</td>" +
                                            /*"<td  class='forteen-col-large right-align-field'>" +
                                                "Rate" +
                                            "</td>" +*/
                                            "<td  class='forteen-col-large right-align-field'>" +
                                                "{16}" +
                                            "</td>" +
                                            "<td  class='forteen-col-large right-align-field'>" +
                                                "Amt" +
                                            "</td>" +
                                            /*"<td  class='forteen-col-large right-align-field'>" +
                                                "Rate" +
                                            "</td>" +*/
                                            "<td  class='forteen-col-large right-align-field'>" +
                                                "{16}" +
                                            "</td>" +
                                        "</tr>" +
                                    "</thead>" +
                                    "<tbody>" +
                                        "{17}"+                                    
                                    "</tbody>" +
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
                                                "Debit({18})" +
                                            "</th>" +
                                            "<th class='table-grid-col-fifth right-align-field'>" +
                                                "Credit({18})" +
                                            "</th>" +
                                        "</tr>" +
                                    "</thead>" +
                                    "<tbody>" +
                                        "{19}"+
                                    "</tbody>" +
                                    "<tfoot>" +
                                        "<tr>" +
                                            "<th colspan='4'  class='right-align-field'>" +
                                                "Amount Before Vat 7%" +
                                            "</th>" +
                                            "<th  class='right-align-field'>" +
                                                "{20}" +
                                            "</th>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<th colspan='4'  class='right-align-field'>" +
                                               "Vat 7%" +
                                            "</th>" +
                                            "<th  class='right-align-field'>" +
                                                "{21}" +
                                            "</th>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<th colspan='4'  class='right-align-field'>" +
                                                "Total" +
                                            "</th>" +
                                            "<th  class='right-align-field'>" +
                                                "{22}" +
                                            "</th>" +
                                        "</tr>" +
                                       "<tr>" +
                                            "<th colspan='4'  class='right-align-field'>" +
                                                "With Holding Tax" +
                                            "</th>" +
                                            "<th  class='right-align-field'>" +
                                                "{23}" +
                                            "</th>" +
                                        "</tr>" +
                                       "<tr>" +
                                            "<th colspan='4'  class='right-align-field'>" +
                                                "Net Payment" +
                                            "</th>" +
                                            "<th  class='right-align-field'>" +
                                                "{24}" +
                                            "</th>" +
                                        "</tr>" +
                                        /*"<tr>" +
                                            "<td colspan='5'>" +
                                                "Amount in words : <span>{25}</span>" +
                                            "</td>" +
                                        "</tr>" +*/
                                    "</tfoot>" +
                                "</table>" +
                                "<table border='0' class='table-remarks' >" +
                                    "<tr style='display: {26};'>" +
                                        "<td class='table-remarks-caption-col text-left-align'>" +
                                            "<b>Remarks</b>" +
                                        "</td>" +
                                        "<td style='width: 0.11cm;'>" +
                                            ":" +
                                        "</td>" +
                                        "<td class='text-left-align'>" +
                                            "{27}" +
                                        "</td>" +
                                    "</tr>" +
                                    //"<tr style='display: {28};'>" +
                                    //    "<td class='table-remarks-caption-col text-left-align'>" +
                                    //        "<b>Remarks</b>" +
                                    //    "</td>" +
                                    //    "<td style='width: 0.11cm;'>" +
                                    //        ":" +
                                    //    "</td>" +
                                    //    "<td class='text-left-align'>" +
                                    //        "{29}" +
                                    //    "</td>" +
                                    //"</tr>" +  
                                "</table>" +
                                "<table class='table-approve-five'>" +
                                    "<tr>" +
                                        "<td class='table-approve-five-col-normal'>" +
                                            "{30}" +
                                            "<hr />" +
                                            "<b>Prepared By</b>" +
                                            "<p>" +
                                               " Date : {31}</p>" +
                                        "</td>" +
                                        /*"<td class='table-approve-five-col-separator'>&nbsp;</td>"+
                                        "<td class='table-approve-five-col-normal'>" +
                                            "{32}" +
                                            "<hr />" +
                                            "<b>Reviewed By</b>" +
                                            "<p>" +
                                                "Date : {33}</p>" +
                                        "</td>" +
                                        "<td class='table-approve-five-col-separator'>&nbsp;</td>" +
                                        "<td class='table-approve-five-col-normal'>" +
                                            "{34}" +
                                            "<hr />" +
                                            "<b>Varifeid By</b>" +
                                            "<p>" +
                                                "Date : {35}</p>" +
                                        "</td>" +*/
                                        "<td class='table-approve-five-col-separator'>&nbsp;</td>" +
                                        "<td class='table-approve-five-col-normal'>" +
                                            "{36}" +
                                            "<hr />" +
                                            "<b>Approved By</b>" +
                                            "<p>" +
                                                "Date : {37}</p>" +
                                        "</td>" +
                                        "<td class='table-approve-five-col-separator'>&nbsp;</td>" +
                                        "<td class='table-approve-five-col-normal'>" +
                                            "{38}" +
                                            "<hr />" +
                                            "<b>Received By</b>" +
                                            "<p>" +
                                                "Date : {39}</p>" +
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
                                //"<div  class='footer'>" +
                                //    "{40}" +
                                //"</div>" +
                            "</div>";

        private const string rowTemplate = "<tr>" +
                                "<td valign='top' >{0}</td>" +
                                "<td colspan='2' valign='top' >{1}</td>" +
                                /*"<td class='right-align-field' >{2}</td>" +*/
                                "<td class='right-align-field' valign='top' >{3}</td>" +
                                "<td class='right-align-field' valign='top' >{4}</td>" +
                            "</tr>";

        private const string billItemRowTemplate = "<tr>" +
                                                     "<td valign='top' class='forteen-col-small text-left-aligh'>{0}</td>" +
                                                     "<td valign='top' class='forteen-col-large text-left-aligh'>{1}</td>" +
                                                     "<td valign='top' class='forteen-col-large text-left-aligh'>{2}</td>" +
                                                     /*"<td valign='top' class='forteen-col-large text-left-aligh'>{3}</td>" +*/
                                                     "<td valign='top' class='forteen-col-medium right-align-field'>{4}</td>" +
                                                     "<td valign='top' class='forteen-col-large right-align-field'>{5}</td>" +
                                                     /*"<td valign='top' class='forteen-col-large right-align-field'>{6}</td>" +*/
                                                     "<td valign='top' class='forteen-col-large right-align-field'>{7}</td>" +
                                                     "<td valign='top' class='forteen-col-large right-align-field'>{8}</td>" +
                                                     /*"<td valign='top' class='forteen-col-large right-align-field'>{9}</td>" +*/
                                                     "<td valign='top' class='forteen-col-large right-align-field'>{10}</td>" +
                                                     "<td valign='top' class='forteen-col-large right-align-field'>{11}</td>" +
                                                     /*"<td valign='top' class='forteen-col-large right-align-field'>{12}</td>" +*/
                                                     "<td valign='top' class='forteen-col-large right-align-field'>{13}</td>" +
                                                  "</tr>";
        
        #endregion
    }

    public class BillDetails
    {
        public string No { get; set; }
        public string BillNo { get; set; }
        public string Date { get; set; }
        public string DueDate { get; set; }
        public string TotalAmtTax { get; set; }
        public string TotalAmtAmount { get; set; }
        public string TotalAmtRate { get; set; }
        public string TotalAmtCurrncy { get; set; }
        public string BalAmtAmount { get; set; }
        public string BalAmtRate { get; set; }
        public string BalAmtCurrncy { get; set; }
        public string PaymentAmtAmount { get; set; }
        public string PaymentAmtRate { get; set; }
        public string PaymentAmtCurrncy { get; set; }
    }

    public class PaymentVoucherItem
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
