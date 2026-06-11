using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Reports
{
    public sealed class JournalVoucher
    {
        #region Constructors
        public JournalVoucher()
        {
            this.PrinterSettings = new PrinterSettings { DocumentWidth = "21cm", DocumentHeight = "14cm", WindowWidth = "793", WindowHeight = "529" };
            ItemDetails = new List<VoucherDetails>();
        }
        #endregion

        #region Properties
        public string HideHeadTitle { get; set; }
        public string HideSubTitle { get; set; }
        public string HideFooterText { get; set; }
        public string HidePageNo { get; set; }
        public string HeadTitle { get; set; }
        public string SubTitle { get; set; }
        public string Logo { get; set; }
        public string JournalNoLabel { get; set; }
        public string FromTo { get; set; }
        public string DateFormat { get; set; }
        public string CurrencyFormat { get; set; }
        public string NumberFormat { get; set; }
        public string ExchangeRate { get; set; }
        public string RateFormat { get; set; }
        public string FromToValue { get; set; }
        public string TrxCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public string Remarks { get; set; }
        public string RemarksHide { get; set; }
        public string Narration { get; set; }
        public string NarrationHide { get; set; }
        public string PreparedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string PreparedDate { get; set; }
        public string ReviewedDate { get; set; }
        public string ApprovedDate { get; set; }
        public string DebitTotal { get; set; }
        public string AmountInWords { get; set; }
        public string FooterText { get; set; } public string HideLogo { get; set; }
        public string Date { get; set; }
        public string Amount { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string VoucherNo { get; set; }
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
        #region Commented by sreejith [Change - Show Footer items in page bottom]
        //private string reportTemplate = @"<div id='printContainer'>" +
        //                                      "<table border='0' cellpadding='0' cellspacing='0' style='font-family: Arial;width: 100%;'>" +
        //                                        "<tr>" +
        //                                            "<td style='padding-bottom: 0.12cm; color: #336699;'>" +
        //                                                "<img alt='' src='{0}' stlye='display: {24}'  />" +
        //                                            "</td>" +
        //                                            "<td style='font-size: 9pt;' align='center'>" +
        //                                                "<p style='display: {25},text-align:center;font-weight:Bold;font-size:12pt;' >{1}</p>" +
        //                                            "</td>" +
        //                                            "<td colspan='2' />" +
        //                                        "</tr>" +
        //                                        "<tr><td colspan='4' class='padding-bottom' ></td></tr> " +
        //                                        "<tr style='font-size: 9pt;'>" +
        //                                            "<td valign='top' colspan='2' >{2}<label>  : <b>{3}</b></label>" +
        //                                            "</td>" +
        //    //"<td></td>" +
        //                                            "<td valign='top'>{4}" +
        //                                            "</td>" +
        //                                            "<td valign='top'><label>: <b>{5}</b></label>" +
        //                                            "</td>" +
        //                                        "</tr>" +
        //                                        "<tr style='font-size: 9pt;'>" +
        //                                            "<td colspan='2' valign='top'>" +
        //                                            "</td>" +
        //                                            "<td valign='top'>Date" +
        //                                            "</td>" +
        //                                            "<td valign='top'><label>: <b>{6}</b></label>" +
        //                                            "</td>" +
        //                                        "</tr>" +
        //                                        "<tr style='font-size: 9pt;'>" +
        //                                            "<td style='width: 33%;' valign='top'>Currency :<label>{7}</label>" +
        //                                            "</td>" +
        //                                            "<td style='width: 33%;' valign='top'>Amount :<label>{8}</label>" +
        //                                            "</td>" +
        //                                            "<td style='width: 17%;' valign='top'>Ref. No" +
        //                                            "</td>" +
        //                                            "<td style='width: 16%;' valign='top'><label>: {9}</label>" +
        //                                            "</td>" +
        //                                        "</tr>" +
        //                                        "<tr style='font-size: 9pt;'>" +
        //                                            "<td colspan='2' class='padding-bottom'>" +
        //                                                "&nbsp;" +
        //                                            "</td>" +
        //                                            "<td valign='top' >" +
        //                                                "Ref. Date" +
        //                                            "</td>" +
        //                                            "<td valign='top'>" +
        //                                                "<label>: {10}</label>" +
        //                                            "</td>" +
        //                                        "</tr>" +
        //                                      "</table>" +
        //                                      "<table cellpadding='0' cellspacing='0' class='table-grid-6' >" +
        //                                        "<thead>" +
        //                                            "<tr>" +
        //                                                "<th class='table-grid-6-col-first text-left-align' >" +
        //                                                    "Acc Code" +
        //                                                "</th>" +
        //                                                "<th class='table-grid-6-col-second text-left-align' colspan='3' >" +
        //                                                    "Acc Name" +
        //                                                "</th>" +
        //    /*"<th style='width: 27.2%;' align='left'>" +
        //    "Description" +
        //    "</th>" +
        //    "<th align='right' style='width: 10%;'>" +
        //    "Ex. Rate" +
        //    "</th>" +*/
        //                                                "<th class='table-grid-6-col-fifth right-align-field'>" +
        //                                                    "Debit({11})" +
        //                                                "</th>" +
        //                                                "<th class='table-grid-6-col-sixth right-align-field'>" +
        //                                                    "Credit({11})" +
        //                                                "</th>" +
        //                                            "</tr>" +
        //                                        "</thead>" +
        //                                        "<tbody>" +
        //                                            "{28}" +
        //                                        "</tbody>" +
        //                                        "<tfoot>" +
        //                                            "<tr>" +
        //                                                "<th colspan='3'  class='text-left-align'>" +
        //                                                    "Total amount this voucher : " +
        //                                                "</th>" +
        //                                                "<th  class='right-align-field'>" +
        //                                                "</th>" +
        //                                                "<th class='right-align-field' >" +
        //                                                    "{12}" +
        //                                                "</th>" +
        //                                                "<th class='right-align-field' >" +
        //                                                    "{13}" +
        //                                                "</th>" +
        //                                            "</tr>" +
        //    /*"<tr>" +
        //    "<th colspan='6' align='left'>" +
        //    "Amount in words: {14}" +
        //    "</th>" +
        //    "</tr>" +*/
        //                                        "</tfoot>" +
        //                                      "</table>" +
        //                                      "<table border='0' class='table-remarks'>" +
        //                                        "<tr style='display: {26};'>" +
        //                                            "<td class='table-remarks-caption-col text-left-align'  valign='top'>" +
        //                                                "Remarks" +
        //                                            "</td>" +
        //                                            "<td class='col-separator' align='center' valign='top'>" +
        //                                                ":" +
        //                                            "</td>" +
        //                                            "<td class='text-left-align' valign='top'>" +
        //                                                "{15}" +
        //                                            "</td>" +
        //                                        "</tr>" +
        //    //"<tr style='display: {27};'>" +
        //    //    "<td class='table-remarks-caption-col text-left-align' valign='top'>" +
        //    //        "Narration" +
        //    //    "</td>" +
        //    //    "<td class='col-separator' align='center' valign='top'>" +
        //    //        ":" +
        //    //    "</td>" +
        //    //    "<td class='text-left-align' valign='top' >" +
        //    //        "{16}" +
        //    //    "</td>" +
        //    //"</tr>" +
        //                                      "</table>" +
        //                                      "<table border='0' class='table-approve-five' >" +
        //                                        "<tr>" +
        //                                            "<td class='table-approve-five-col-normal'>" +
        //                                                "{17}" +
        //                                                "<hr />" +
        //                                                "<b>Prepared By</b>" +
        //                                                "<p>" +
        //                                                    "Date : {20}</p>" +
        //                                            "</td>" +
        //                                            "<td class='table-approve-five-col-separator'>&nbsp;</td>" +
        //    /*"<td align='center' style='margin-left: 1cm; margin-right: 1cm; font-size: 7pt;width:32.33%'>" +
        //    "{18}" +
        //    "<hr />" +
        //    "<b>Reviewed By</b>" +
        //    "<p>" +
        //    "Date : {21}</p>" +
        //    "</td>" +*/
        //                                            "<td class='table-approve-five-col-normal' >" +
        //                                                "{19}" +
        //                                                "<hr />" +
        //                                                "<b>Approved By</b>" +
        //                                                "<p>" +
        //                                                    "Date : {22}</p>" +
        //                                            "</td>" +
        //                                        "</tr>" +
        //                                      "</table>" +
        //    //"<div class='footer' >" +
        //    //  "{23}" +
        //    //"</div>" +
        //                                  "</div>"; 
        #endregion
        private string reportTemplate = @"<div id='printContainer'>" +
                                            "<table border='0' cellpadding='0' cellspacing='0' style='height:14cm;'>" +
                                             "<tr>" +
                                             "<td style='height:2cm;'>" +
            //first START
        #region  first START
                                              "<table width='100%' border='0' cellpadding='0' cellspacing='0' style='font-family: Arial;'>" +
                                                "<tr>" +
                                                    "<td style='padding-bottom: 0.12cm; color: #336699;'>" +
                                                        "<img alt='' src='{0}' stlye='display: {24}'  />" +
                                                    "</td>" +
                                                    "<td style='font-size: 9pt;' align='center'>" +
                                                        "<p style='display: {25},text-align:center;font-weight:Bold;font-size:12pt;' >{1}</p>" +
                                                    "</td>" +
                                                    "<td colspan='2'></td>" +
                                                "</tr>" +
                                                "<tr><td colspan='4' class='padding-bottom' ></td></tr> " +
                                                "<tr style='font-size: 9pt;'>" +
                                                    "<td valign='top' colspan='2' >{2}<label>  : <b>{3}</b></label>" +
                                                    "</td>" +
                                                    "<td valign='top'>{4}" +
                                                    "</td>" +
                                                    "<td valign='top'><label>: <b>{5}</b></label>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr style='font-size: 9pt;'>" +
                                                    "<td colspan='2' valign='top'>" +
                                                    "</td>" +
                                                    "<td valign='top'>Date" +
                                                    "</td>" +
                                                    "<td valign='top'><label>: <b>{6}</b></label>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr style='font-size: 9pt;'>" +
                                                    "<td style='width: 33%;' valign='top'>Currency :<label>{7}</label>" +
                                                    "</td>" +
                                                    "<td style='width: 33%;' valign='top'>Amount :<label>{8}</label>" +
                                                    "</td>" +
                                                    "<td style='width: 17%;' valign='top'>Ref. No" +
                                                    "</td>" +
                                                    "<td style='width: 16%;' valign='top'><label>: {9}</label>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr style='font-size: 9pt;'>" +
                                                    "<td colspan='2' class='padding-bottom'>" +
                                                        "&nbsp;" +
                                                    "</td>" +
                                                    "<td valign='top' >" +
                                                        "Ref. Date" +
                                                    "</td>" +
                                                    "<td valign='top'>" +
                                                        "<label>: {10}</label>" +
                                                    "</td>" +
                                                "</tr>" +
                                              "</table>" +
        #endregion
            //first END
                                              "</td>" +
                                              "</tr>" +
                                              "<tr>" +
                                              "<td style='vertical-align:top;'>" +
            //second START
        #region second START
                                              "<table width='100%' cellpadding='0' cellspacing='0' class='table-grid-6' >" +
                                                "<thead>" +
                                                    "<tr>" +
                                                        "<th class='table-grid-6-col-first text-left-align' >" +
                                                            "Acc Code" +
                                                        "</th>" +
                                                        "<th class='table-grid-6-col-second text-left-align' colspan='3' >" +
                                                            "Acc Name" +
                                                        "</th>" +
                                                        "<th class='table-grid-6-col-fifth right-align-field'>" +
                                                            "Debit({11})" +
                                                        "</th>" +
                                                        "<th class='table-grid-6-col-sixth right-align-field'>" +
                                                            "Credit({11})" +
                                                        "</th>" +
                                                    "</tr>" +
                                                "</thead>" +
                                                "<tbody>" +
                                                    "{28}" +
                                                "</tbody>" +
                                                "<tfoot>" +
                                                    "<tr>" +
                                                        "<th colspan='3'  class='text-left-align'>" +
                                                            "Total amount this voucher : " +
                                                        "</th>" +
                                                        "<th  class='right-align-field'>" +
                                                        "</th>" +
                                                        "<th class='right-align-field' >" +
                                                            "{12}" +
                                                        "</th>" +
                                                        "<th class='right-align-field' >" +
                                                            "{13}" +
                                                        "</th>" +
                                                    "</tr>" +
                                                "</tfoot>" +
                                              "</table>" +
        #endregion
            //second END
                                             "</td>" +
                                             "</tr>" +
                                             "<tr>" +
                                             "<td>" +

            //third START
        #region third START
                                              "<table width='100%' border='0' class='table-remarks' >" +
                                                "<tr style='display: {26};'>" +
                                                    "<td class='table-remarks-caption-col text-left-align'  valign='top'>" +
                                                        "Remarks" +
                                                    "</td>" +
                                                    "<td class='col-separator' align='center' valign='top'>" +
                                                        ":" +
                                                    "</td>" +
                                                    "<td class='text-left-align' valign='top'>" +
                                                        "{15}" +
                                                    "</td>" +
                                                "</tr>" +
                                              "</table>" +
        #endregion
            //third END
                                               "</td>" +
                                               "</tr>" +
                                               "<tr>" +
                                               "<td style='vertical-align:bottom;'>" +
            //fourth START
        #region fourth START
                                              "<table width='100%' border='0' class='table-approve-five' >" +
                                                "<tr>" +
                                                    "<td class='table-approve-five-col-normal'>" +
                                                        "{17}" +
                                                        "<hr />" +
                                                        "<b>Prepared By</b>" +
                                                        "<p>" +
                                                            "Date : {20}</p>" +
                                                    "</td>" +
                                                    "<td class='table-approve-five-col-separator'>&nbsp;</td>" +
                                                    "<td class='table-approve-five-col-normal' >" +
                                                        "{19}" +
                                                        "<hr />" +
                                                        "<b>Approved By</b>" +
                                                        "<p>" +
                                                            "Date : {22}</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                              "</table>" +
        #endregion
            //forth END
                                               "</td>" +
                                               "</tr>" +
                                               "</table>" +
                                               "</div>";
        #endregion

        #region Row Template
        private string rowTemplate = "<tr>" +
                                        "<td style='padding-left:0.14cm;font-family:Arial;' valign='top'>{0} " +
                                        "</td>" +
                                        "<td style='padding-left:0.14cm;font-family:Arial;' colspan='3' valign='top'>{1} " +
                                        "</td>   " +
            /*"<td style='padding-left:0.14cm;font-family:Arial;'>{2} " +
            "</td>   " +
            "<td align='right' style='padding-right:0.14cm;font-family:Arial;'>{3} " +
            "</td>" +*/
                                        "<td align='right' style='padding-right:0.14cm;font-family:Arial;' valign='top'>{4} " +
                                        "</td>   " +
                                        "<td align='right' style='padding-right:0.14cm;font-family:Arial;' valign='top'>{5} " +
                                        "</td>" +
                                     "</tr>";
        #endregion
        #endregion
        // Tested With DotMatrix Printer
        #region Html Template
        /*private string reportTemplate_backUp = @"<html><head></head><body><div class='content-wrapper' id='printContainer' style='width: 19.94cm; height: 12.94cm; page-break-after: always;" +
                                          "      padding: 0cm; background-color: White !important; font-family: Courier New;'>                                     " +
                                          "      <table border='0' cellpadding='0' cellspacing='0' style='font-family: Courier New;                     " +
                                          "          width: 19.94cm;  background-color: White !important;'>                                                                                  " +
                                          "          <tr>                                                                                               " +
                                          "              <td style='font-size: 11pt; padding-bottom: 0.12cm; color: #336699;'>                          " +
                                          "                      <img alt='company name' src='{0}' stlye='display: {24}'  />                            " +
                                          "              </td>                                                                                          " +
                                          "              <td style='font-weight: bold' align='center'>                                                  " +
                                          "                  <p style='display: {25},text-align:center;'>                                                                " +
                                          "                      {1}                                                                                    " +
                                          "                  </p>                                                                                     " +
                                          "              </td>                                                                                          " +
                                          "              <td colspan='2' />                                                                             " +
                                          "          </tr>                                                                                              " +
                                          "          <tr style='font-size: 8pt;'>                                                                       " +
                                          "              <td>                                                                                           " +
                                          "                  <b>{2}</b>                                                                                 " +
                                          "                  <label>  :                                                                                 " +
                                          "                      {3}</label>                                                                            " +
                                          "              </td>                                                                                          " +
                                          "              <td>                                                                                           " +
                                          "              </td>                                                                                          " +
                                          "              <td>                                                                                           " +
                                          "                  <b>{4} </b>                                                                               " +
                                          "              </td>                                                                                          " +
                                          "              <td>                                                                                           " +
                                          "                  <label>                                                                                    " +
                                          "                      : {5}</label>                                                                          " +
                                          "              </td>                                                                                          " +
                                          "          </tr>                                                                                              " +
                                          "          <tr style='font-size: 8pt;'>                                                                       " +
                                          "              <td colspan='2'>                                                                               " +
                                          "              </td>                                                                                          " +
                                          "              <td>                                                                                           " +
                                          "                  <b>Date</b>                                                                              " +
                                          "              </td>                                                                                          " +
                                          "              <td>                                                                                           " +
                                          "                  <label>                                                                                    " +
                                          "                      : {6}</label>                                                                          " +
                                          "              </td>                                                                                          " +
                                          "          </tr>                                                                                              " +
                                          "          <tr style='font-size: 8pt;'>                                                                       " +
                                          "              <td style='width: 33%;'>                                                                       " +
                                          "                  Currency :                                                                                 " +
                                          "                  <label>                                                                                    " +
                                          "                      {7}</label>                                                                            " +
                                          "              </td>                                                                                          " +
                                          "              <td style='width: 33%;'>                                                                       " +
                                          "                  Amount :                                                                                   " +
                                          "                  <label>                                                                                    " +
                                          "                      {8}</label>                                                                            " +
                                          "              </td>                                                                                          " +
                                          "              <td style='width: 17%;'>                                                                       " +
                                          "                  <b>Ref. No</b>                                                                             " +
                                          "              </td>                                                                                          " +
                                          "              <td style='width: 16%;'>                                                                       " +
                                          "                  <label>                                                                                    " +
                                          "                      : {9}</label>                                                                          " +
                                          "              </td>                                                                                          " +
                                          "          </tr>                                                                                              " +
                                          "          <tr style='font-size: 8pt;'>                                                                       " +
                                          "              <td colspan='2'>                                                                               " +
                                          "              </td>                                                                                          " +
                                          "              <td>                                                                                           " +
                                          "                  <b>Ref. Date</b>                                                                           " +
                                          "              </td>                                                                                          " +
                                          "              <td>                                                                                           " +
                                          "                  <label>                                                                                    " +
                                          "                      : {10}</label>                                                                         " +
                                          "              </td>                                                                                          " +
                                          "          </tr>                                                                                              " +
                                          "      </table>                                                                                               " +
                                          "      <hr style='width: 19.94cm; margin-bottom: 0.07cm; margin-left: 0cm; padding-left: 0cm;' />             " +
                                          "      <table border='1' cellpadding='0' cellspacing='0' style='width: 19.94cm; margin-bottom: 0.79cm;        " +
                                          "          font-size: 8pt; font-family: Courier New;'>                                                        " +
                                          "          <thead>                                                                                            " +
                                          "              <tr>                                                                                           " +
                                          "                  <th style='width: 2.12cm; padding-left: 0.14cm;' align='left'>                             " +
                                          "                      Acc Code                                                                               " +
                                          "                  </th>                                                                                      " +
                                          "                  <th style='width: 5.02cm; padding-left: 0.14cm;' align='left'>                             " +
                                          "                      Acc Name                                                                               " +
                                          "                  </th>                                                                                      " +
                                          "                  <th style='width: 5.56cm; padding-left: 0.14cm;' align='left'>                             " +
                                          "                      Description                                                                            " +
                                          "                  </th>                                                                                      " +
                                          "                  <th align='right' style='padding-right: 0.14cm;'>                                          " +
                                          "                      Ex. Rate                                                                               " +
                                          "                  </th>                                                                                      " +
                                          "                  <th align='right' style='padding-right: 0.14cm;'>                                          " +
                                          "                      Debit({11})                                                                            " +
                                          "                  </th>                                                                                      " +
                                          "                  <th align='right' style='padding-right: 0.14cm;'>                                          " +
                                          "                      Creddit({11})                                                                          " +
                                          "                  </th>                                                                                      " +
                                          "              </tr>                                                                                          " +
                                          "          </thead>                                                                                           " +
                                          "          <tbody>                                                                                            " +
                                          "              {28}                                                                                           " +
                                          "          </tbody>                                                                                           " +
                                          "          <tfoot>                                                                                            " +
                                          "              <tr>                                                                                           " +
                                          "                  <th colspan='4' align='left'>                                                              " +
                                          "                      Total amount this voucher :                                                            " +
                                          "                  </th>                                                                                      " +
                                          "                  <th align='right' style='padding-right: 0.14cm'>                                           " +
                                          "                      {12}                                                                                   " +
                                          "                  </th>                                                                                      " +
                                          "                  <th align='right' style='padding-right: 0.14cm'>                                           " +
                                          "                      {13}                                                                                   " +
                                          "                  </th>                                                                                      " +
                                          "              </tr>                                                                                          " +
                                          "              <tr>                                                                                           " +
                                          "                  <th colspan='6' align='left'>                                                              " +
                                          "                      Amount in words: {14}                                                                  " +
                                          "                  </th>                                                                                      " +
                                          "              </tr>                                                                                          " +
                                          "          </tfoot>                                                                                           " +
                                          "      </table>                                                                                               " +
                                          "      <table border='0' style='width: 19.94cm; font-family: Courier New; font-size: 8pt'>                    " +
                                          "          <tr style='display: {26};'>                                                                        " +
                                          "              <td style='width: 2.12cm; padding-left: 0.14cm;' align='left'>                                 " +
                                          "                  <b>Remarks</b>                                                                             " +
                                          "              </td>                                                                                          " +
                                          "              <td style='width: 0.11cm;'>                                                                    " +
                                          "                  :                                                                                          " +
                                          "              </td>                                                                                          " +
                                          "              <td>                                                                                           " +
                                          "                  {15}                                                                                       " +
                                          "              </td>                                                                                          " +
                                          "          </tr>                                                                                              " +
                                          "          <tr style='display: {27};'>                                                                        " +
                                          "              <td style='width: 2.12cm; padding-left: 0.14cm;' align='left'>                                 " +
                                          "                  <b>Narration</b>                                                                           " +
                                          "              </td>                                                                                          " +
                                          "              <td style='width: 0.11cm;'>                                                                    " +
                                          "                  :                                                                                          " +
                                          "              </td>                                                                                          " +
                                          "              <td>                                                                                           " +
                                          "                  {16}                                                                                       " +
                                          "              </td>                                                                                          " +
                                          "          </tr>                                                                                              " +
                                          "      </table>                                                                                               " +
                                          "      <table border='0' style='width: 19.94cm; font-family: Courier New;'>                                   " +
                                          "          <tr>                                                                                               " +
                                          "              <td align='center' style='margin-left: 1cm; margin-right: 1cm; font-size: 8pt;'>               " +
                                          "                  {17}                                                                                       " +
                                          "                  <hr />                                                                                     " +
                                          "                  <b>Prepared By</b>                                                                         " +
                                          "                  <p>                                                                                        " +
                                          "                      Date : {20}</p>                                                                        " +
                                          "              </td>                                                                                          " +
                                          "              <td align='center' style='margin-left: 1cm; margin-right: 1cm; font-size: 8pt;'>               " +
                                          "                  {18}                                                                                       " +
                                          "                  <hr />                                                                                     " +
                                          "                  <b>Reviewed By</b>                                                                         " +
                                          "                  <p>                                                                                        " +
                                          "                      Date : {21}</p>                                                                        " +
                                          "              </td>                                                                                          " +
                                          "              <td align='center' style='margin-left: 1cm; margin-right: 1cm; font-size: 8pt;'>               " +
                                          "                  {19}                                                                                       " +
                                          "                  <hr />                                                                                     " +
                                          "                  <b>Approved By</b>                                                                         " +
                                          "                  <p>                                                                                        " +
                                          "                      Date : {22}</p>                                                                        " +
                                          "              </td>                                                                                          " +
                                          "          </tr>                                                                                              " +
                                          "      </table>                                                                                               " +
                                          "      <div style='position: absolute; top: 11.5cm; border-top; border-top: 1px solid black;                  " +
                                          "          width: 19.89cm; font-size: 7pt; font-family: Courier New;'>                                        " +
                                          "          {23}                                                                                               " +
                                          "      </div>                                                                                                 " +
                                          "  </div></body></html>";

        private string rowTemplate_backUp = "<tr>" +
                                     "    <td style='padding-left:0.14cm;font-family:Courier New;'>{0} " +
                                     "    </td>   " +
                                     "    <td style='padding-left:0.14cm;font-family:Courier New;'>{1} " +
                                     "    </td>   " +
                                     "    <td style='padding-left:0.14cm;font-family:Courier New;'>{2} " +
                                     "    </td>   " +
                                     "    <td align='right' style='padding-right:0.14cm;font-family:Courier New;'>{3} " +
                                     "    </td>  " +
                                     "    <td align='right' style='padding-right:0.14cm;font-family:Courier New;'>{4} " +
                                     "    </td>   " +
                                     "    <td align='right' style='padding-right:0.14cm;font-family:Courier New;'>{5} " +
                                     "    </td>   " +
                                     "</tr>"; */
        #endregion
    }
}
