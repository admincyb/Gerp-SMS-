using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.Web.Hosting;
using System.Web;

namespace ERP.Utilities
{
    public class HTMLReportBuilder
    {
        public string Result1;
        public string Result2;
        public string Result3;
        private string DecimalFormat = "#0.";
        private string CurrencyFormat = "#0.";
        private string colStyle = string.Empty;
        private string ColorField = "ROW_COLOR";
        public HTMLReportBuilder()
        {
            int NoDecimalDigitsP2P = HttpContext.Current.Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(HttpContext.Current.Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
            for (int i = 0; i < NoDecimalDigitsP2P; i++)
            {
                DecimalFormat += "0";
            }

            for (int i = 0; i < System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
            {
                CurrencyFormat += "0";
            }
            ////hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();

            //hdfRateFormat.Value = "#0.";
            //int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
            //    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
            //    : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
            //for (int i = 0; i < rateDecimalDigits; i++)
            //{
            //    hdfRateFormat.Value += "0";
            //}


        }
        public string GetHtml(DataTable dtRptData, string xmlURL, string stripColor = "#dcdcdc", string rptType = "SC")
        {
            try
            {
                XDocument xDoc = XDocument.Load(HostingEnvironment.MapPath(xmlURL));

                StringBuilder sbHeader = new StringBuilder();
                StringBuilder sbBody = new StringBuilder();
                StringBuilder sbFooter = new StringBuilder();

                StringBuilder sbHeaderRow1 = new StringBuilder("<tr>");
                StringBuilder sbHeaderRow2 = new StringBuilder("<tr>");

                #region Body
                sbBody.Append("<div id=\"data_wrapper\"><table id=\"data_body\">");
                int rowNo = 1;
                string tableRow = "<tr>";
                foreach (DataRow dr in dtRptData.Rows)
                {
                    /*Row color style*/
                    if (rowNo % 2 == 0)
                        tableRow = "<tr style=\"background-color: "+ stripColor + ";\">";
                    else
                        tableRow = "<tr>";

                    DataColumnCollection columns = dtRptData.Columns;
                    if (columns.Contains(ColorField))
                    {
                        if (!string.IsNullOrEmpty(dr[ColorField].ToString()))
                        {
                            tableRow = "<tr style=\"background-color: " + dr[ColorField].ToString() + ";\">";
                        }
                    }
                    sbBody.Append(tableRow);

                    foreach (XElement xe in xDoc.Descendants("Fields"))
                    {
                        bool FieldNameExist = xe.Element("FieldName") == null ? false : true;
                        bool WidthExist = xe.Element("Width") == null ? false : true;
                        string fieldWidth = (!WidthExist || string.IsNullOrEmpty(xe.Element("Width").Value)) ? "100px" : xe.Element("Width").Value;
                        if (!FieldNameExist)
                        {
                            throw new Exception("xml field missing");
                        }

                        string widthStyle = ""; string numStyle = " style=\"text-align: right\"";
                        if (rowNo == 1)
                        {
                            widthStyle = " style=\"width: " + fieldWidth + "\"";
                            numStyle = " style=\"text-align: right; width:" + fieldWidth + "\"";
                        }
                        switch (xe.Element("DataType").Value.ToLower())
                        {
                            case "date":
                                DateTime dateVal = new DateTime();
                                DateTime.TryParse(dr[xe.Element("FieldName").Value].ToString(), out dateVal);
                                string fieldDate = dateVal == new DateTime() ? "" : dateVal.ToString("dd-MM-yyyy");
                                sbBody.Append("<td" + widthStyle + ">" + fieldDate + "</td>");
                                break;
                            case "number":
                                sbBody.Append("<td" + numStyle + ">" + GetFormattedNumber(dr[xe.Element("FieldName").Value]) + "</td>");
                                break;
                            case "currency":
                                sbBody.Append("<td" + numStyle + ">" + GetFormattedCurrencyWithComa(dr[xe.Element("FieldName").Value]) + "</td>");
                                break;
                            default:
                                sbBody.Append("<td" + widthStyle + ">" + dr[xe.Element("FieldName").Value] + "</td>");
                                break;
                        }
                    }
                    sbBody.Append("</tr>");
                    rowNo++;
                }
                sbBody.Append("</table></div>");
                #endregion

                #region Header & Footer
                sbHeader.Append("<table id=\"data_headers\">");
                sbFooter.Append("<table id=\"data_footer\"><tr>");
                int colNo = 0;
                foreach (XElement xe in xDoc.Descendants("Fields"))
                {
                    #region Validations
                    bool MainTittleExist = xe.Element("MainTittle") == null ? false : true;
                    bool MainTittleColSpanExist = xe.Element("MainTittleColSpan") == null ? false : true;
                    bool MainTitleBGColorExist = xe.Element("MainTitleBGColor") == null ? false : true;
                    bool FieldHeaderExist = xe.Element("FieldHeader") == null ? false : true;
                    bool FieldNameExist = xe.Element("FieldName") == null ? false : true;
                    bool HeaderBGColorExist = xe.Element("HeaderBGColor") == null ? false : true;
                    bool FooterTextExist = xe.Element("FooterText") == null ? false : true;
                    bool FunctionExist = xe.Element("Function") == null ? false : true;
                    bool FooterFieldExist = xe.Element("FooterField") == null ? false : true;
                    bool DataTypeExist = xe.Element("DataType") == null ? false : true;
                    bool WidthExist = xe.Element("Width") == null ? false : true;
                    bool FooterSpecialFieldExist = xe.Element("FooterSpecialField") == null ? false : true;
                    #endregion

                    #region Null checkig and Assign default value
                    string strColSpan = (!MainTittleColSpanExist || string.IsNullOrEmpty(xe.Element("MainTittleColSpan").Value)) ? "1" : xe.Element("MainTittleColSpan").Value;
                    string title = (!MainTittleExist || string.IsNullOrEmpty(xe.Element("MainTittle").Value)) ? "" : xe.Element("MainTittle").Value;
                    string titleBGColor = (!MainTitleBGColorExist || string.IsNullOrEmpty(xe.Element("MainTitleBGColor").Value)) ? "" : xe.Element("MainTitleBGColor").Value;
                    string headerBGColor = (!HeaderBGColorExist || string.IsNullOrEmpty(xe.Element("HeaderBGColor").Value)) ? "" : xe.Element("HeaderBGColor").Value;
                    string dataType = (!DataTypeExist || string.IsNullOrEmpty(xe.Element("DataType").Value)) ? "" : xe.Element("DataType").Value;
                    string fieldWidth = (!WidthExist || string.IsNullOrEmpty(xe.Element("Width").Value)) ? "100px" : xe.Element("Width").Value;
                    string fieldHeader = (!FieldHeaderExist || string.IsNullOrEmpty(xe.Element("FieldHeader").Value)) ? "" : xe.Element("FieldHeader").Value;
                    string function = (!FunctionExist || string.IsNullOrEmpty(xe.Element("Function").Value)) ? "" : xe.Element("Function").Value;
                    string footerText = (!FooterTextExist || string.IsNullOrEmpty(xe.Element("FooterText").Value)) ? "" : xe.Element("FooterText").Value;
                    string footerSpecialField = (!FooterSpecialFieldExist || string.IsNullOrEmpty(xe.Element("FooterSpecialField").Value)) ? "" : xe.Element("FooterSpecialField").Value;
                    #endregion

                    int colSpan = Convert.ToInt32(strColSpan);
                    int width = 0, totalWidth = 0;
                    string rowAttr1 = "", rowAttr2 = "";
                    string style = "";

                    if (colSpan > 0) //for row 1 in header
                    {
                        int j;
                        for (j = colNo; j < colSpan + colNo; j++)
                        {
                            XElement elm = xDoc.Descendants("Fields").ElementAt(j);
                            string colWidth = (elm.Element("Width") == null || string.IsNullOrEmpty(elm.Element("Width").Value)) ? "100px" : elm.Element("Width").Value;
                            int.TryParse(System.Text.RegularExpressions.Regex.Match(colWidth, @"\d+").Value, out width);
                            totalWidth += width;
                        }

                        if (colSpan > 1)
                        {
                            rowAttr1 = "colspan=\"" + strColSpan + "\"";
                            style = "width: " + totalWidth + "px;text-align: center;background: " + titleBGColor + ";";
                        }
                        else // colSpan == 1
                        {
                            rowAttr1 = "rowspan=\"2\"";
                            style = "width: " + totalWidth + "px;";
                            if (dataType == "Number" || dataType == "Currency")
                            {
                                style += "text-align: right;";
                            }
                            if (headerBGColor != "")
                            {
                                style += "background: " + headerBGColor + ";";
                            }
                            title = fieldHeader;
                        }

                        sbHeaderRow1.Append("<th " + rowAttr1 + " style=\"" + style + "\">" + title + "</th>");
                    }

                    style = "width: " + fieldWidth + ";";
                    string footerStyle = "width: " + fieldWidth + ";";
                    if (dataType == "Number" || dataType == "Currency")
                    {
                        style += "text-align: right;";
                        footerStyle += "text-align: right;";
                    }
                    if (colSpan != 1)
                    {
                        if (headerBGColor != "")
                        {
                            style += "background: " + headerBGColor + ";";
                        }
                        sbHeaderRow2.Append("<th " + rowAttr2 + " style=\"" + style + "\">" + fieldHeader + "</th>");
                    }

                    sbFooter.Append("<th style=\"" + footerStyle + "\">");

                    switch (function.ToLower())
                    {
                        case "sum":
                            decimal sum = dtRptData.AsEnumerable().Sum(s => s.Field<decimal>(xe.Element("FieldName").Value));
                            if (dataType == "Number")
                                sbFooter.Append(GetFormattedNumber(sum));
                            else
                                sbFooter.Append(GetFormattedCurrencyWithComa(sum));
                            break;
                        case "specialfield":
                            switch (footerSpecialField.ToLower())
                            {
                                case "result1":
                                    sbFooter.Append(Result1);
                                    break;
                                case "result2":
                                    sbFooter.Append(Result2);
                                    break;
                                case "result3":
                                    sbFooter.Append(Result3);
                                    break;
                            }
                            break;
                        default:
                            sbFooter.Append(footerText);
                            break;
                    }

                    sbFooter.Append("</th>");
                    colNo++;
                }

                sbHeaderRow1.Append("</tr>");
                sbHeaderRow2.Append("</tr>");

                sbHeader.Append(sbHeaderRow1).Append(sbHeaderRow2).Append("</table>");

                sbFooter.Append("</tr></table>");
                #endregion

                sbHeader.Append(sbBody.Append(sbFooter));

                return sbHeader.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(DecimalFormat);
        }

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(CurrencyFormat);
        }
        public string GetFormattedCurrencyWithComa(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            // return num.ToString(hdfCurrencyFormat.Value);
            string resultNum = string.Format("{0:c}", Convert.ToDecimal(num.ToString(CurrencyFormat)));
            return resultNum;
        }
        //public string GetFormattedRate(object number)
        //{
        //    double num = 0;
        //    double.TryParse(Convert.ToString(number), out num);
        //    return num.ToString(hdfRateFormat.Value);
        //}
    }
}
