using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Reports
{
    public sealed class ChequeReturnVoucherRecept
    {
         #region Constructors
        public ChequeReturnVoucherRecept()
        {
            this.PrinterSettings = new PrinterSettings { DocumentWidth = "21cm", DocumentHeight = "14cm", WindowWidth = "793", WindowHeight = "529" };
            ItemDetails = new List<VoucherDetails>();
        }
        #endregion

        #region Properties
        public string HideLogo { get; set; }
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
        public string ContractDate { get; set; }

        public string CustomerName { get; set; }
        public string TaxNo { get; set; }
        public string InvoiceDate { get; set; }
        public string HideInvoiceNo { get; set; }
        public string InvoiceNo { get; set; }

        public string SaleContractNo { get; set; }
        public string Mode { get; set; }
        public string SalesVoucherNo { get; set; }
        public string InvoiceVoucherDate { get; set; }
        public string TranRef { get; set; }

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
                                                    "<img style='display: {0}' src='{1}' /><span style='display: {2}'>{3}<span>" +
                                                "</th>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td colspan='7' align='center'>" +
                                                   "<span style='display: {4};font-weight:Bold;font-size:12pt;' >{5}</span>" +
                                                "</td>" +
                                            "</tr>" +
                                             "<tr>" +
                                                "<td colspan='7' align='center'>" +
                                                   "<span style='display: {6}'>{7}</span>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr><td colspan='7' class='padding-bottom' ></td></tr> " +
                                            "<tr>" +
                                                "<td>" +
                                                    "Cust. Name" +
                                                "</td>" +
                                                "<td> : </td>"+
                                                "<td colspan='3'>" +
                                                   "<b>{8}</b>" +
                                                "</td>" +                                               
                                                "<td>" +
                                                "</td>" +
                                                "<td>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td class='table-top-sales-col-first'>" +
                                                    "Inv/ Tax No" +
                                                "</td>" +
                                                "<td class='table-col-seperator'> : </td>" +
                                                "<td class='table-top-sales-col-second'>" +
                                                    "{9}" +
                                                "</td>" +
                                                "<td class='table-top-sales-col-third'>" +
                                                    "Date"+
                                                "</td>" +
                                                "<td class='table-top-sales-col-forth'>" +
                                                    ": {10}" +
                                                "</td>" +
                                                "<td class='table-top-sales-col-fifth'>" +
                                                    "RV No" +
                                                "</td>" +
                                                "<td class='table-top-sales-col-sixth'>" +
                                                    ": <b>{11}</b>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "Sale Contract No" +
                                                "</td>" +
                                                "<td> : </td>" +
                                                "<td>" +
                                                    "{12}" +
                                                "</td>" +
                                                "<td>" +
                                                    "Date" +
                                                "</td>" +
                                                "<td>" +
                                                    ": {13}" +
                                                "</td>" +
                                                "<td>" +
                                                    "Date"+
                                                "</td>" +
                                                "<td>" +
                                                    ": <b>{14}</b>" +
                                                "</td>" +
                                            "</tr>" +
                                             "<tr>" +
                                                "<td>" +
                                                    "Currency" +
                                                "</td>" +
                                                "<td> : </td>" +
                                                "<td>" +
                                                    "{15}" +
                                                "</td>" +
                                                "<td>" +
                                                    "Amount" +
                                                "</td>" +
                                                "<td>" +
                                                    ": {16}" +
                                                "</td>" +
                                                "<td>" +
                                                    "Mode Of Receipt" +
                                                "</td>" +
                                                "<td>" +
                                                    ": {17}" +
                                                "</td>" +
                                            "</tr>" +
                                             "<tr>" +
                                                "<td class='padding-bottom' >" +
                                                    "Sale Voucher No" +
                                                "</td>" +
                                                "<td> : </td>" +
                                                "<td>" +
                                                    "{18}" +
                                                "</td>" +
                                                "<td>" +
                                                    "Date" +
                                                "</td>" +
                                                "<td>" +
                                                    ": {19}" +
                                                "</td>" +
                                                "<td>" +
                                                "</td>" +
                                                "<td>" +
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
                                                        "Debit({20})" +
                                                    "</th>" +
                                                    "<th class='table-grid-6-col-sixth right-align-field'>" +
                                                        "Credit({20})" +
                                                    "</th>" +
                                                "</tr>" +
                                            "</thead>" +
                                            "<tbody>" +
                                                "{21}" +
                                            "</tbody>" +
                                            "<tfoot>" +                                        
                                               "<tr>" +
                                                    "<th colspan='3'  class='text-left-align'>" +
                                                        "Total amount this voucher :" +
                                                    "</th>" +
                                                    "<th  class='right-align-field'>" +
                                                    "</th>" +
                                                    "<th  class='right-align-field'>" +
                                                        "{22}" +
                                                    "</th>" +
                                                    "<th  class='right-align-field'>" +
                                                        "{23}" +
                                                    "</th>" +
                                                "</tr>" +
                                                /*"<tr>" +
                                                    "<td colspan='6'>" +
                                                        "Amount in words : <span>{24}</span>" +
                                                    "</td>" +
                                                "</tr>" +*/
                                            "</tfoot>" +
                                        "</table>" +
                                        "<table border='0' class='table-remarks'>" +
                                            "<tr style='display: {25};'>" +
                                                "<td class='table-remarks-caption-col text-left-align'>" +
                                                    "Remarks" +
                                                "</td>" +
                                                "<td class='col-separator' align='center' valign='top'>" +
                                                   ":" +
                                                "</td>" +
                                                "<td class='text-left-align'>" +
                                                    "{26}" +
                                                "</td>" +
                                            "</tr>" +
                                            // "<tr style='display: {27};'>" +
                                            //    "<td class='table-remarks-caption-col text-left-align'>" +
                                            //        "Narration" +
                                            //    "</td>" +
                                            //    "<td class='col-separator'>" +
                                            //        ":" +
                                            //    "</td>" +
                                            //    "<td class='text-left-align'>" +
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
                                                "</td>" +*/                                        
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
                                        "<td>{0}</td>" +
                                        "<td colspan='3'>{1}</td>" +
                                        /*"<td>{2}</td>" +
                                        "<td class='right-align-field'>{3}</td>" +*/
                                        "<td class='right-align-field'>{4}</td>" +
                                        "<td class='right-align-field'>{5}</td>" +
                                     "</tr>";
        #endregion
        #endregion                     
    }
}
