using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Reports
{
    public sealed class SalesVoucher
    {
        #region Constructors
        public SalesVoucher()
        {
            this.PrinterSettings = new PrinterSettings { DocumentWidth = "21cm", DocumentHeight = "14cm", WindowWidth = "793", WindowHeight = "529" };
            ItemDetails = new List<SalesVoucherItem>();
        }
        #endregion

        #region Properties
        public string HideHeadTitle { get; set; }
        public string HideSubTitle { get; set; }
        public string HideFooterText { get; set; }
        public string HeadTitle { get; set; }
        public string ExchangeRateValue { get; set; }

        public string HidePageNo { get; set; }
        public string SubTitle { get; set; }
        public string ReportHeaderName { get; set; }
        public string HideCompanyName { get; set; }

        public string DateFormat { get; set; }
        public string CurrencyFormat { get; set; }
        public string NumberFormat { get; set; }
        public string ExchangeRate { get; set; }
        public string RateFormat { get; set; }

        public string Logo { get; set; }
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceOrTaxNo { get; set; }
        public string SalesVoucherNo { get; set; }
        public string Amount { get; set; }
        public string Date { get; set; }
        public string Currency { get; set; }
        public string InventoryDate { get; set; }


        public string BaseCurrency { get; set; }
        public string Remarks { get; set; }
        public string HideRemarks { get; set; }
        public string Narration { get; set; }
        public string HideNarration { get; set; }
        public string PreparedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string PreparedByDate { get; set; }
        public string ReviewedByDate { get; set; }
        public string ApprovedByDate { get; set; }
        public string DebitTotal { get; set; }
        public string CreditTotal { get; set; }
        public string AmountInWords { get; set; }
        public string FooterText { get; set; } public string HideLogo { get; set; }
        public List<SalesVoucherItem> ItemDetails { get; set; }

        //Printer Settings
        public PrinterSettings PrinterSettings { get; set; }

        #endregion

        #region ReadOnly Properties
        public string BodyTemplate { get { return reportTemplate; } }
        public string RowTemplate { get { return rowTemplate; } }
        #endregion

        #region Html Template
        #region Body Template
        private string reportTemplate = @"<div id='printContainer'>" +
                                        "<table border='0' cellpadding='0' cellspacing='0' class='table-top-sales'>" +
                                            "<tr>" +
                                                "<th colspan='6' align='center' class='table-heading-large'>" +
                                                    "<img style='display: {0}' src='{1}' /><span style='display: {2}'>{3}<span>" +
                                                "</th>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td colspan='6' align='center'>" +
                                                   "<span style='display: {4};font-weight:Bold;font-size:12pt;' >{5}</span>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr><td colspan='6' class='padding-bottom' ></td></tr> " +
                                            "<tr>" +
                                                "<td valign='top'>" +
                                                    "Cust.Name&nbsp;:" +
                                                "</td>" +
                                                "<td colspan='3' valign='top'>" +
                                                   " <b>{6}</b>" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    "SV.No" + 
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    ": <b>{7}</b>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td class='table-top-sales-col-first' valign='top'>" +
                                                    "Inv/Tax No&nbsp;&nbsp;&nbsp;:" +
                                                "</td>" +
                                                "<td class='table-top-sales-col-second' valign='top'>" +
                                                    " {8}" +
                                                "</td>" +
                                                "<td class='table-top-sales-col-third' valign='top'>" +
                                                    "Inv.Date"+
                                                "</td>" +
                                                "<td class='table-top-sales-col-forth' valign='top'>" +
                                                    ": {9}" +
                                                "</td>" +
                                                "<td class='table-top-sales-col-fifth' valign='top'>" +
                                                    "Date" +
                                                "</td>" +
                                                "<td class='table-top-sales-col-sixth' valign='top'>" +
                                                    ": <b>{10}</b>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td valign='top' class='padding-bottom' >" +
                                                    "Currency&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    " {11}" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    "Amount" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    ": {13}" +
                                                "</td>" +
                                                "<td>" +
                                                  // "Exchange Rate" + // Hided in report
                                                "</td>" +
                                                "<td>" +
                                                  // ": {12}" + // Hided in report
                                                "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                        "<table cellpadding='0' cellspacing='0' class='table-grid-6'>" +
                                            "<thead>" +
                                                "<tr>" +
                                                    "<th class='table-grid-6-col-first text-left-align'>" +
                                                       "Acc Code" +
                                                    "</th>" +
                                                    "<th class='table-grid-6-col-second text-left-align' colspan='3'>" +
                                                        "Acc Name" +
                                                    "</th>" +
                                                    /*"<th class='table-grid-6-col-third text-left-align'>" +
                                                        "Description" +
                                                    "</th>" +
                                                    "<th class='table-grid-6-col-forth right-align-field'>" +
                                                        "Ex.Rate" +
                                                    "</th>" +*/
                                                    "<th class='table-grid-6-col-fifth right-align-field'>" +
                                                        "Debit({14})" +
                                                    "</th>" +
                                                    "<th class='table-grid-6-col-sixth right-align-field'>" +
                                                        "Credit({14})" +
                                                    "</th>" +
                                                "</tr>" +
                                            "</thead>" +
                                            "<tbody>" +
                                                "{15}" +
                                            "</tbody>" +
                                            "<tfoot>" +                                        
                                               "<tr>" +
                                                   "<th colspan='3'  class='text-left-align'>" +
                                                        "Total amount this voucher :" +
                                                    "</th>" +
                                                    "<th  class='right-align-field'>" +
                                                    "</th>" +
                                                    "<th  class='right-align-field'>" +
                                                        "{16}" +
                                                    "</th>" +
                                                    "<th  class='right-align-field'>" +
                                                        "{17}" +
                                                    "</th>" +
                                                "</tr>" +
                                                /*"<tr>" +
                                                    "<td colspan='6'>" +
                                                        "Amount in words : <span>{18}</span>" +
                                                    "</td>" +
                                                "</tr>" +*/
                                            "</tfoot>" +
                                        "</table>" +
                                        "<table border='0' class='table-remarks' >" +
                                            "<tr style='display: {19};'>" +
                                                "<td class='table-remarks-caption-col text-left-align' valign='top' >" +
                                                    "Remarks" +
                                                "</td>" +
                                                "<td class='col-separator' align='center' valign='top'>" +
                                                    ":" +
                                                "</td>" +
                                                "<td class='text-left-align' >" +
                                                    "{20}" +
                                                "</td>" +
                                            "</tr>" +
                                            // "<tr style='display: {21};' >" +
                                            //    "<td class='table-remarks-caption-col text-left-align' valign='top' >" +
                                            //        "Narration" +
                                            //    "</td>" +
                                            //    "<td class='col-separator' align='center' valign='top'>" +
                                            //        ":" +
                                            //    "</td>" +
                                            //    "<td class='text-left-align' valign='top' >" +
                                            //        "{22}" +
                                            //    "</td>" +
                                            //"</tr>" +
                                        "</table>" +
                                        "<table class='table-approve-five'>" +
                                            "<tr>" +
                                                "<td class='table-approve-five-col-normal'>" +
                                                    "{23}" +
                                                    "<hr />" +
                                                    "<b>Prepared By</b>" +
                                                    "<p>" +
                                                       " Date : {24}</p>" +
                                                "</td>" +
                                                /*"<td class='table-approve-five-col-separator' style='display:none;'>&nbsp;</td>" +
                                                "<td class='table-approve-five-col-normal' style='display:none;'>" +
                                                    "{25}" +
                                                    "<hr />" +
                                                    "<b>Reviewed By</b>" +
                                                    "<p>" +
                                                        "Date : {26}</p>" +
                                                "</td>" +*/                                        
                                                "<td class='table-approve-five-col-separator'>&nbsp;</td>" +
                                                "<td class='table-approve-five-col-normal'>" +
                                                    "{27}" +
                                                    "<hr />" +
                                                    "<b>Approved By</b>" +
                                                    "<p>" +
                                                        "Date : {28}</p>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                        //"<div class='footer'>" +
                                        //    "Printed By: {29}" +
                                        //"</div>" +
                                    "</div>";
        #endregion

        #region Row Template
        private string rowTemplate = "<tr>" +
                                        "<td valign='top'>{0}</td>" +
                                        "<td valign='top' colspan='3'>{1}</td>" +
                                       /*"<td>{2}</td>" +
                                        "<td class='right-align-field'>{3}</td>" +*/
                                        "<td valign='top' class='right-align-field'>{4}</td>" +
                                        "<td valign='top' class='right-align-field'>{5}</td>" +
                                     "</tr>";
        #endregion
        #endregion
    }

    public class SalesVoucherItem
    {
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string Description { get; set; }
        public string ExRate { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
    }
}
