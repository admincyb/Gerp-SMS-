using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Reports
{
    public sealed class PurchaseVoucher
    {
        #region Constructors
        public PurchaseVoucher()
        {
            this.PrinterSettings = new PrinterSettings { DocumentWidth = "21cm", DocumentHeight = "14cm", WindowWidth = "793", WindowHeight = "529" };
            ItemDetails = new List<VoucherDetails>();
        }
        #endregion

        #region Properties
        public string HideHeadTitle { get; set; }
        public string HideSubTitle { get; set; }
        public string HideFooterText { get; set; }
        public string HeadTitle { get; set; }

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
        public string VoucherNo { get; set; }
        public string Amount { get; set; }
        public string Date { get; set; }
        public string Currency { get; set; }
        public string ReceivedDate { get; set; }

        public string From { get; set; }
        public string PoNo { get; set; }
        public string PoDate { get; set; }
        public string TaxNo { get; set; }
        public string PINo { get; set; }
        public string PIDate { get; set; }
        public string PRNo { get; set; }


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
        public string FooterText { get; set; } 
        public string HideLogo { get; set; }
        public List<VoucherDetails> ItemDetails { get; set; }

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
                                                "<th colspan='7' align='center' class='table-heading-large'>" +
                                                    "<img style='display: {0}' src='{1}'  /><span style='display: {2}'>{3}<span>" +
                                                "</th>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td colspan='7' align='center'>" +
                                                   "<span style='display: {4};font-weight:Bold;font-size:12pt;' >{5}</span>" +
                                                "</td>" +
                                            "</tr>" +
                                             "<tr>" +
                                                "<td colspan='7' align='center' >" +
                                                   "<span style='display: {6}' >{7}</span>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr><td colspan='7' class='padding-bottom' ></td></tr> " +
                                            "<tr>" +
                                                "<td valign='top' >" +
                                                    "From" +
                                                "</td>" +
                                                "<td valign='top'> : </td>" +
                                                "<td colspan='3' valign='top'>" +
                                                   " <b>{8}</b>" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    "PU.No" + 
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    ": <b>{9}</b>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td class='table-top-sales-col-first' valign='top'>" +
                                                    "PO No" +
                                                "</td>" +
                                                "<td class='table-col-seperator' valign='top'> : </td>" +
                                                "<td class='table-top-sales-col-second' valign='top'>" +
                                                    " {10}" +
                                                "</td>" +
                                                "<td class='table-top-sales-col-third' valign='top'>" +
                                                    "Date"+
                                                "</td>" +
                                                "<td class='table-top-sales-col-forth' valign='top'>" +
                                                    ": {11}" +
                                                "</td>" +
                                                "<td class='table-top-sales-col-fifth' valign='top'>" +
                                                    "Date" +
                                                "</td>" +
                                                "<td class='table-top-sales-col-sixth' valign='top'>" +
                                                    ": <b>{12}</b>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td valign='top'>" +
                                                    "Tax No" +
                                                "</td>" +
                                                "<td valign='top'> : </td>" +
                                                "<td valign='top'>" +
                                                    " {13}" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    "Date" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    ": {14}" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    "Amount"+
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    ": {15}" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td valign='top' class='padding-bottom' >" +
                                                    "PI No" +
                                                "</td>" +
                                                "<td valign='top'> : </td>" +
                                                "<td valign='top'>" +
                                                    " {16}" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    "Date" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    ": {17}" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    "Currency" +
                                                "</td>" +
                                                "<td valign='top'>" +
                                                    ": {18}" +
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
                                                        "Debit({19})" +
                                                    "</th>" +
                                                    "<th class='table-grid-6-col-sixth right-align-field'>" +
                                                        "Credit({19})" +
                                                    "</th>" +
                                                "</tr>" +
                                            "</thead>" +
                                            "<tbody>" +
                                                "{20}" +
                                            "</tbody>" +
                                            "<tfoot>" +                                        
                                               "<tr>" +
                                                    "<th colspan='3'  class='text-left-align'>" +
                                                        "Total amount this voucher :" +
                                                    "</th>" +
                                                    "<th  class='right-align-field'>" +
                                                    "</th>" +
                                                    "<th  class='right-align-field'>" +
                                                        "{21}" +
                                                    "</th>" +
                                                    "<th  class='right-align-field'>" +
                                                        "{22}" +
                                                    "</th>" +
                                                "</tr>" +
                                                /*"<tr>" +
                                                    "<td colspan='6'>" +
                                                        "Amount in words : <span>{23}</span>" +
                                                    "</td>" +
                                                "</tr>" +*/
                                            "</tfoot>" +
                                        "</table>" +
                                        "<div class='aditional-data-block'>PR No : {24}</div>" +
                                        "<table border='0' class='table-remarks'>" +                                             
                                            "<tr style='display: {25};'>" +
                                                "<td class='table-remarks-caption-col text-left-align' valign='top'>" +
                                                    "Remarks" +
                                                "</td>" +
                                                "<td class='col-separator' align='center' valign='top'>" +
                                                    ":" +
                                                "</td>" +
                                                "<td class='text-left-align' valign='top'>" +
                                                    "{26}" +
                                                "</td>" +
                                            "</tr>" +
                                            // "<tr style='display: {27};'>" +
                                            //    "<td class='table-remarks-caption-col text-left-align' valign='top'>" +
                                            //        "Narration" +
                                            //    "</td>" +
                                            //    "<td class='col-separator' align='center' valign='top'>" +
                                            //        ":" +
                                            //    "</td>" +
                                            //    "<td class='text-left-align' valign='top'>" +
                                            //        "{28}" +
                                            //    "</td>" +
                                            //"</tr>" +
                                        "</table>" +
                                        "<table class='table-approve-five'>" +
                                            "<tr>" +
                                                "<td class='table-approve-five-col-normal'>" +
                                                    "{29}" +
                                                    "<hr />" +
                                                    "<b>Prepared By</b>" +
                                                    "<p>" +
                                                       " Date : {30}</p>" +
                                                "</td>" +
                                                /*"<td class='table-approve-five-col-separator'>&nbsp;</td>" +
                                                "<td class='table-approve-five-col-normal'>" +
                                                    "{31}" +
                                                    "<hr />" +
                                                    "<b>Reviewed By</b>" +
                                                    "<p>" +
                                                        "Date : {32}</p>" +
                                                "</td>" + */                                       
                                                "<td class='table-approve-five-col-separator'>&nbsp;</td>" +
                                                "<td class='table-approve-five-col-normal'>" +
                                                    "{33}" +
                                                    "<hr />" +
                                                    "<b>Approved By</b>" +
                                                    "<p>" +
                                                        "Date : {34}</p>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                        //"<div class='footer'>" +
                                        //    "Printed By: {35}" +
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
}
