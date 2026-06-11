using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using MailSendCore;
using System.Threading;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing;
using MailCore.Service;
namespace ERP.Utilities
{
    class TextValue
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    class TextValueComboBox
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
    }
    class PairedValue
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string PairText { get; set; }
    }

    /// <summary>
    /// Client Culture
    /// </summary>
    public class ClientCulture
    {
        public string Culture
        { get; set; }
        public string Name
        { get; set; }
        public string Code
        { get; set; }
    }
    //public class ChkListMaster
    //{
    //    public int? PK { get; set; }
    //    public string Value { get; set; }

    //}
    //public class DDLMaster
    //{
    //    public int? PK { get; set; }
    //    public string Value { get; set; }

    //}

    public class CommonFunctions
    {
        /// <summary>
        /// This method creates and returns a DataTable with given field 
        /// </summary>
        ///<param name="lstFields"></param>
        ///<returns>DataTable</returns>
        public static DataTable CreateDataTable(object[] lstFields)
        {
            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            foreach (object obj in lstFields)
            {
                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = obj.ToString();
                //myDataColumn.DefaultValue = obj.ToString();
                //myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = obj.ToString();

                if (obj.ToString() == "pk")
                {
                    myDataColumn.AutoIncrementSeed = 1;
                    myDataColumn.AutoIncrement = true;

                }
                else
                    myDataColumn.DefaultValue = obj.ToString();
                myDataTable.Columns.Add(myDataColumn);

            }
            return myDataTable;
        }
        /// <summary>
        /// Password Expiry Checking
        /// </summary>
        /// <param name="pwdModOn"></param>
        /// <param name="expiryDays"></param>
        /// <returns></returns>
        public static bool IsPasswordExpired(DateTime pwdModOn, int expiryDays)
        {
            bool result = false;
            DateTime expiryDate = pwdModOn.AddDays(expiryDays);
            if (DateTime.Now > expiryDate)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// Get Client Culture
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public static List<ClientCulture> GetClientCulture(string strVal)
        {
            string[] cultureArray;
            List<ClientCulture> lstCulture = new List<ClientCulture>();
            if (!string.IsNullOrEmpty(strVal))
            {
                cultureArray = strVal.Split(',');
                string[] cultureFields;
                foreach (string flVal in cultureArray)
                {
                    cultureFields = flVal.Split('|');
                    if (cultureFields.Length >= 3)
                        lstCulture.Add(new ClientCulture { Culture = cultureFields[0], Name = cultureFields[1], Code = cultureFields[2] });
                }
            }
            else
            {
                lstCulture.Add(new ClientCulture { Culture = "en-US", Name = "English", Code = "EN" });
            }
            return lstCulture;

        }
        /// <summary>
        /// Convert Image To Byte
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        /// <summary>
        /// This method Format Error Message
        /// </summary>
        ///<param name="lstFields"></param>
        ///<returns>DataTable</returns>
        public static string FormatErrorMessage(string errMsg)
        {
            return "<ul><li>" + errMsg + "</li></ul>";
        }

        /// <summary>
        /// No need to use this. 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime FormatDate(string date)
        {
            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();
            dateFormat.ShortDatePattern = "dd/MM/yyyy";
            return Convert.ToDateTime(date, dateFormat);
        }

        /// <summary>
        /// Methord used to create Xml By Passing the Object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// 

        /// <summary>
        /// Methord used to create Xml By Passing the Object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static XmlDocument ObjectTOXml(object obj)
        {
            string xmlStr;
            string startBlock;
            string endBlock;
            startBlock = "{'root':";
            endBlock = "}";
            xmlStr = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //Adding A common Parent Node to the json String
            xmlStr = startBlock + xmlStr + endBlock;
            //Converting the json String to Xml
            XmlDocument xDoc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(xmlStr);

            return xDoc;
        }
        /// <summary>
        /// Methord used to create Xml By Passing the Object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static XmlDocument ObjectTOXml(object obj, int val)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream msXml = new MemoryStream();
            XmlDocument xDoc = new XmlDocument();
            serializer.Serialize(msXml, obj);
            msXml.Position = 0;
            xDoc.Load(msXml);
            msXml.Close();
            return xDoc;
        }
        /// <summary>
        /// Methord used to create Xml By Passing the Object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static XmlDocument ObjectTOXmlForPrdPlan(object obj)
        {

            //Newtonsoft Method
            //string xmlStr;
            //string startBlock;
            //string endBlock;
            //startBlock = "{'root':";
            //endBlock = "}";
            //xmlStr = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            ////Adding A common Parent Node to the json String
            //xmlStr = startBlock + xmlStr + endBlock;
            ////Converting the json String to Xml
            //XmlDocument xDoc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(xmlStr);
            //return xDoc;
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream msXml = new MemoryStream();
            XmlDocument xDoc = new XmlDocument();
            serializer.Serialize(msXml, obj);
            msXml.Position = 0;
            xDoc.Load(msXml);
            msXml.Close();
            return xDoc;
        }
        /*
         
         */

        /// <summary>
        /// Methord used to create Xml By Passing the Object without root node
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static XmlDocument ObjectTOXmlTemp(object obj)
        {
            XmlDocument xDoc;
            MemoryStream tempStream = new MemoryStream();
            xDoc = new XmlDocument();
            try
            {
                // Create an XmlSerializer instance using the method below.
                XmlSerializer myXmlSerializer = CreateOverrider(obj.GetType());
                myXmlSerializer.Serialize(tempStream, obj);
                tempStream.Position = 0;
                xDoc.Load(tempStream);
            }
            catch
            {
            }
            finally
            {
                tempStream.Close();
                tempStream.Dispose();
            }
            return xDoc;
        }

        /// <summary>
        /// Serialize Class Object to XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string XmlSerialize<T>(T obj)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = new XmlTextWriter(sw);
            ser.Serialize(tw, obj);

            string result = sw.ToString();
            return result;
        }

        /// <summary>
        /// DeSerialize Class Object from XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xml)
        {
            //deserialize to object
            XmlSerializer ser = new XmlSerializer(typeof(T));
            T obj = (T)ser.Deserialize(XmlReader.Create(new StringReader(xml)));
            return obj;
        }

        /// <summary>
        /// Return an XmlSerializer to override the root serialization.
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        private static XmlSerializer CreateOverrider(Type objType)
        {
            // Create an XmlRootAttribute overloaded constructer 
            //and set its namespace.
            XmlRootAttribute myXmlRootAttribute =
                           new XmlRootAttribute("root");
            myXmlRootAttribute.Namespace = "";
            // Create the Serializer, and return it.
            XmlSerializer myXmlSerializer = new XmlSerializer
               (objType, myXmlRootAttribute);
            return myXmlSerializer;
        }

        /// <summary>
        /// HTML Decode a collection of column names in a Data Table
        /// </summary>
        /// <param name="dTable"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DataTable HtmlDecodeDataTable(DataTable dTable, List<string> fieldNames)
        {
            foreach (DataRow drow in dTable.Rows)
            {
                foreach (string fieldName in fieldNames)
                {
                    if (drow[fieldName] != null && drow[fieldName].GetType().IsEquivalentTo(typeof(string)))
                    {
                        drow[fieldName] = System.Web.HttpUtility.HtmlDecode((drow[fieldName].ToString()));
                    }
                }
            }
            dTable.AcceptChanges();
            return dTable;
        }

        /// <summary>
        /// HTML Decode all string column of string field in a Data Table
        /// </summary>
        /// <param name="dTable"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DataTable HtmlDecodeDataTable(DataTable dTable)
        {
            foreach (DataRow drow in dTable.Rows)
            {
                for (int i = 0; i < drow.ItemArray.Length; i++)
                    if (drow[i].GetType() == typeof(System.String))
                        drow[i] = System.Web.HttpUtility.HtmlDecode((drow[i].ToString()));
            }
            dTable.AcceptChanges();
            return dTable;
        }

        /// <summary>
        /// HTML Decode a single column of string field in a Data Table
        /// </summary>-
        /// <param name="dTable"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DataTable HtmlDecodeDataTable(DataTable dTable, string fieldName)
        {
            foreach (DataRow drow in dTable.Rows)
            {
                if (drow[fieldName] != null && drow[fieldName] is string)
                {
                    drow[fieldName] = System.Web.HttpUtility.HtmlDecode((drow[fieldName].ToString()));
                }
            }
            dTable.AcceptChanges();
            return dTable;
        }
        /// <summary>
        /// Encode Table
        /// </summary>
        /// <param name="dTable"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DataTable HtmlEncodeDataTable(DataTable dTable, string fieldName)
        {
            foreach (DataRow drow in dTable.Rows)
            {
                if (drow[fieldName] != null && drow[fieldName] is string)
                {
                    drow[fieldName] = System.Web.HttpUtility.HtmlEncode((drow[fieldName].ToString()));
                }
            }
            dTable.AcceptChanges();
            return dTable;
        }

        /// <summary>
        /// To Format Column Names for Rdlc Binding
        /// </summary>
        /// <param name="reportsTable"></param>
        /// <returns></returns>
        public static DataTable GetDrlcFormattedTable(DataTable reportsTable)
        {
            try
            {
                foreach (DataColumn column in reportsTable.Columns)
                {
                    if (column.Ordinal == reportsTable.Columns.Count - 1)
                        column.ColumnName = column.Caption = "Cost";
                    else
                        column.ColumnName = column.Caption = "Field" + (column.Ordinal + 1).ToString();
                }
                return reportsTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Format Column Names for Rdlc Binding
        /// </summary>
        /// <param name="reportsTable"></param>
        /// <returns></returns>
        public static DataTable GetDrlcFormattedTableForActivity(DataTable reportsTable)
        {
            try
            {
                foreach (DataColumn column in reportsTable.Columns)
                {
                    if (column.Ordinal == reportsTable.Columns.Count - 1)
                        column.ColumnName = column.Caption = "ArrivalDelay";
                    else
                        if (column.Ordinal == reportsTable.Columns.Count - 2)
                            column.ColumnName = column.Caption = "DepartureDelay";
                        else
                            column.ColumnName = column.Caption = "Field" + (column.Ordinal + 1).ToString();
                }
                return reportsTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save Mail Queue
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="to"></param>
        /// <param name="applicationID"></param>
        /// <param name="bizUnit"></param>
        /// <param name="userPK"></param>
        /// <param name="mailContent"></param>
        /// <param name="tempateId"></param>
        /// <returns></returns>
        public static int SaveMailQue(string subject, string mailContent, string to, int applicationID, int bizUnit, int userPK, string applicationType, int partyType, int partyPK, int status, int? tempateId = 0, DateTime? lastModDate = null, string toCC = "", string toBCC = "", string mailFrom = "")
        {
            int resultQue;
            DataTable dtMailContent;
            resultQue = 0;
            try
            {
                MailCore.BO.MailQueueData data = new MailCore.BO.MailQueueData();
                if (tempateId.HasValue && tempateId > 0)
                {
                    dtMailContent = TemplateManager.TemplateContentGet(tempateId.Value, applicationID, bizUnit);
                    if (dtMailContent != null && dtMailContent.Rows.Count > 0)
                    {
                        //TML_APP_TYPE TML_APP_SUB_TYPE TML_TYPE TML_ACTION  TML_NAME  
                        data.CONTENT = dtMailContent.Rows[0]["CONTENT"].ToString();
                        if (!dtMailContent.Rows[0]["TML_ACTION"].Equals(DBNull.Value))
                            data.ACTION = Convert.ToInt16(dtMailContent.Rows[0]["TML_ACTION"].ToString());
                        if (!dtMailContent.Rows[0]["TML_APP_SUB_TYPE"].Equals(DBNull.Value))
                            data.APP_SUB_TYPE_VAL = Convert.ToInt16(dtMailContent.Rows[0]["TML_APP_SUB_TYPE"].ToString());
                        if (!dtMailContent.Rows[0]["TML_APP_TYPE"].Equals(DBNull.Value))
                            data.APP_TYPE = dtMailContent.Rows[0]["TML_APP_TYPE"].ToString();
                    }
                }
                else
                    data.CONTENT = mailContent;
                data.APP_TYPE = applicationType;
                data.PARTY_TYPE = partyType;
                data.PARTY_PK = partyPK;
                data.STATUS = status;
                data.CONTENT = mailContent;
                data.SUBJECT = subject;
                //data.FROM = ConfigurationManager.AppSettings["SmtpEmail"];
                data.FROM = string.IsNullOrEmpty(mailFrom) ? ConfigurationManager.AppSettings["FrmMailResetPwd"] : mailFrom;
                data.TO = to;
                data.PK = applicationID;
                data.BIZUNIT = bizUnit;
                data.CRTD_BY = userPK;
                data.LAST_MOD_DT = lastModDate == null ? DateTime.Now : (DateTime)lastModDate;
                data.CC = toCC;
                data.BCC = toBCC;
                resultQue = MailSendManager.SaveMailQueue(data);
            }
            catch
            {

            }
            return resultQue;
        }

        public static string GetOtherChargeCatXML(string CatPks)
        {
            string retXML = string.Empty;
            ChargeCategoryBO objChargeCategory = new ChargeCategoryBO();
            objChargeCategory.lstCategory = new List<CategoryList>();
            foreach (string pk in CatPks.Split(','))
                objChargeCategory.lstCategory.Add(new CategoryList() { CAT_PK = Convert.ToInt32(pk) });
            retXML = CommonFunctions.XmlSerialize<ChargeCategoryBO>(objChargeCategory);
            return retXML;
        }
        [Serializable]
        [XmlRoot("Root")]
        public class ChargeCategoryBO
        {
            [XmlElement("Catrgory")]
            public List<CategoryList> lstCategory { get; set; }
        }

        [Serializable]
        public class CategoryList
        {
            [XmlElement("PK")]
            public int CAT_PK { get; set; }
        }
        /// <summary>
        /// To Format Column Names for Rdlc Binding
        /// </summary>
        /// <param name="reportsTable"></param>
        /// <returns></returns>
        public static DataTable GetDrlcFormattedTableForOTP(DataTable reportsTable)
        {
            try
            {
                foreach (DataColumn column in reportsTable.Columns)
                {
                    if (column.Ordinal < reportsTable.Columns.Count - 15)
                        column.ColumnName = column.Caption = "Field" + (column.Ordinal + 1).ToString();
                }
                return reportsTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Format Column Names for Rdlc Binding
        /// </summary>
        /// <param name="reportsTable"></param>
        /// <returns></returns>
        public static DataTable GetDrlcFormattedTableForDelay(DataTable reportsTable)
        {
            try
            {
                foreach (DataColumn column in reportsTable.Columns)
                {
                    if (column.Ordinal < reportsTable.Columns.Count - 13)
                        column.ColumnName = column.Caption = "Field" + (column.Ordinal + 1).ToString();
                }
                return reportsTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to check if theme exists
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns></returns>
        public static bool ThemeExists(string themeName)
        {
            bool hasTheme = false;
            try
            {
                System.IO.DirectoryInfo themeInfo = new System.IO.DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/App_Themes"));
                System.IO.DirectoryInfo[] dirInfo = themeInfo.GetDirectories();
                var theme = dirInfo.AsEnumerable().Where(dir => dir.Name == themeName);
                hasTheme = (theme != null && theme.Count() > 0);
            }
            catch
            {
            }
            return hasTheme;
        }

        /// <summary>
        /// For formatting lengthy string in Grid
        /// </summary>
        /// <param name="orginalString"></param>
        /// <param name="limit"></param>
        /// <returns>string</returns>
        public static string GetShortString(object evelOrginalString, int limit)
        {
            string orginalString = HttpUtility.HtmlDecode(Convert.ToString(evelOrginalString));
            return (orginalString.Length <= limit) ? orginalString : (orginalString.Substring(0, limit) + "..");
        }
        /// <summary>
        /// For formatting lengthy string From last
        /// </summary>
        /// <param name="evelOrginalString"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static string GetShortStringFromLast(object evelOrginalString, int limit)
        {
            string orginalString = HttpUtility.HtmlDecode(Convert.ToString(evelOrginalString));
            return (orginalString.Length <= limit) ? orginalString : ".." + (orginalString.Substring(orginalString.Length - limit, limit));
        }

        /// <summary>
        /// For formatting lengthy string in Grid
        /// </summary>
        /// <param name="orginalString"></param>
        /// <param name="limit"></param>
        /// <param name="sep"></param>
        /// <returns>string</returns>
        public static string GetShortString(object evelOrginalString, int limit, string sep)
        {
            string orginalString = HttpUtility.HtmlDecode(Convert.ToString(evelOrginalString));
            return ((orginalString.Length <= limit) ? orginalString : (orginalString.Substring(0, limit) + sep)).ToUpper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string ProcessException(Exception exception)
        {
            // Gets or sets exception mode;stored in web.config;Admin/User
            string exceptionMode = ConfigurationManager.AppSettings[ERPUtilityResources.ExceptionMode];
            // Gets or sets exception details
            string[] exceptionDetails = exception.Message.Split(ERPUtilityResources.ExceptionToken.ToCharArray());
            // Gets or sets exception source;where from exception occured
            string exceptionSource = exceptionDetails[0];
            // Gets or sets exception details count;client exception have only 1 details
            int count = exceptionDetails.Count();
            // Gets or sets error message
            string errorMessage = String.Empty;
            // Gets or sets page name
            string pageName = String.Empty;
            // Gets or sets field name (eg.pageName fieldName already exists)
            string fieldName = String.Empty;
            // Checking exception details ends with GTI_EXCEPTION;then it comes from service side
            if (exceptionDetails[count - 1].Equals(ERPUtilityResources.GTIException))
            {
                // Gets or sets exception key;key is stored as count-4 to count-3 position in exception message
                string exceptionKey = exceptionDetails[count - 4] + ERPUtilityResources.ExceptionToken + exceptionDetails[count - 3];
                // Checking exception mode is admin
                if (exceptionMode.Equals(ERPUtilityResources.Admin))
                {
                    // Gets exception message from exception details;replacing GTI_EXCEPTION_DETAILS token from message
                    errorMessage = exceptionDetails[count - 2].Replace(ERPUtilityResources.GTIExceptionDetails, String.Empty);
                    // Checking no exception details found
                    if (errorMessage.Equals(String.Empty))
                    {
                        // Sets exception message to unknown
                        errorMessage = ERPUtilityResources.UnHandledException;
                    }
                    // Return exception message to client;by encoding it;shows using jquery message
                    //return errorMessage.Replace(ERPUtilityResources.SingleQuoteCharacter, ERPUtilityResources.SpaceCharacter).Replace(ERPUtilityResources.DoubleQuoteCharacter, ERPUtilityResources.SpaceCharacter).Replace(ERPUtilityResources.EnterCharacter, ERPUtilityResources.BreakTag).Replace(ERPUtilityResources.CommaCharacter, ERPUtilityResources.BreakTag).Replace(ERPUtilityResources.EditEditConcurrencyException, ERPUtilityResources.Msg_Save_Error_Concurrent);
                    return errorMessage.Replace(ERPUtilityResources.SingleQuoteWithoutEscape, ERPUtilityResources.SingleQuoteCharacter).Replace(ERPUtilityResources.DoubleQuoteCharacter, ERPUtilityResources.SpaceCharacter).Replace(ERPUtilityResources.EnterCharacter, ERPUtilityResources.BreakTag).Replace(ERPUtilityResources.CommaCharacter, ERPUtilityResources.BreakTag).Replace(ERPUtilityResources.EditEditConcurrencyException, ERPUtilityResources.Msg_Save_Error_Concurrent);
                }
                else
                {
                    // Gets error list hash table;stored in applicaton
                    Hashtable errorList = (Hashtable)HttpContext.Current.Application[ERPUtilityResources.ErrorTable];
                    // Gets  page list hash table;stored in applicaton
                    Hashtable pageList = (Hashtable)HttpContext.Current.Application[ERPUtilityResources.PageTable];
                    // Gets field list hash table;stored in applicaton
                    Hashtable fieldList = (Hashtable)HttpContext.Current.Application[ERPUtilityResources.FieldTable];
                    // Gets pageName from application using exception source as key in error hash table
                    pageName = (pageList == null || pageList[exceptionSource] == null) ? String.Empty : pageList[exceptionSource].ToString();
                    // Gets fieldName from application using exception source as key in field hash table
                    fieldName = (fieldList == null || fieldList[exceptionSource] == null) ? "Code" : fieldList[exceptionSource].ToString();
                    // Constucting error message by appending  page name + field name + error message
                    errorMessage = String.Format((errorList == null || errorList[exceptionKey] == null) ? String.Empty : errorList[exceptionKey].ToString(),
                                pageName, fieldName);
                    if (Convert.ToString(exceptionDetails[count - 3]) == "547")
                    {
                        errorMessage = "547";
                    }
                    // Checking no error message is empty
                    if (errorMessage.Equals(String.Empty))
                    {
                        // Sets error message as unhandled exception
                        errorMessage = ERPUtilityResources.UnHandledException;
                    }
                    // Return exception message to client;by encoding it;shows using jquery message
                    return errorMessage.Replace(ERPUtilityResources.EnterCharacter, ERPUtilityResources.BreakTag).Replace(ERPUtilityResources.CommaCharacter, ERPUtilityResources.BreakTag);
                }
            }
            else
            {
                // Return client exception message to client;by encoding it;shows using jquery message
                //return exception.Message.Replace(ERPUtilityResources.SingleQuoteCharacter, ERPUtilityResources.SpaceCharacter).Replace(ERPUtilityResources.DoubleQuoteCharacter, ERPUtilityResources.SpaceCharacter).Replace(ERPUtilityResources.EnterCharacter, ERPUtilityResources.BreakTag).Replace(ERPUtilityResources.CommaCharacter, ERPUtilityResources.BreakTag);
                return exception.Message.Replace(ERPUtilityResources.SingleQuoteWithoutEscape, ERPUtilityResources.SingleQuoteCharacter).Replace(ERPUtilityResources.DoubleQuoteCharacter, ERPUtilityResources.SpaceCharacter).Replace(ERPUtilityResources.EnterCharacter, ERPUtilityResources.BreakTag).Replace(ERPUtilityResources.CommaCharacter, ERPUtilityResources.BreakTag);
            }
        }

        /// <summary>
        /// Get Hour Text from Double Value
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetHourText(double time)
        {
            string[] timeParts = new string[2];
            timeParts[0] = (((int)time) / 60).ToString("00");
            timeParts[1] = (((int)time) % 60).ToString("00");
            return string.Format(ERPUtilityResources.FormatTime, timeParts[0], timeParts[1]);
        }

        /// <summary>
        /// To Decrypt Eval Key
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string DecryptKey(string queryString)
        {
            CryptoServices cryptoServicesObj;
            cryptoServicesObj = new CryptoServices();
            if (!string.IsNullOrEmpty(queryString))
            {
                return cryptoServicesObj.DecryptString(queryString, "56565656");
            }
            else
            {
                return queryString;
            }
        }

        public static string Encrypt(string queryString)
        {
            CryptoServices cryptoServicesObj;
            cryptoServicesObj = new CryptoServices();
            if (!string.IsNullOrEmpty(queryString))
            {
                return cryptoServicesObj.EncryptString(queryString, "56565656");
            }
            else
            {
                return queryString;
            }
        }

        /// <summary>
        /// format the input string;
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string FormatInputString(string inputString)
        {
            return "'" + inputString.Trim().Replace("'", "''") + "'";
        }

        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="toEmailId"></param>      
        /// <modifiedby></modifiedby>
        /// <modifieddate></modifieddate>
        public static bool SendMail(string subject, string message, string toEmailId, string fromMailID)
        {
            if ((toEmailId != string.Empty))
            {

                DataTable dtMailCfg = GetMailCFGDA();
                string SmtpHost = Convert.ToString(dtMailCfg.Rows[0]["SmtpHost"]);
                string SmtpPort = Convert.ToString(dtMailCfg.Rows[0]["SmtpPort"]);
                string EnableSsl = Convert.ToString(dtMailCfg.Rows[0]["EnableSsl"]);
                string SmtpEmail = Convert.ToString(dtMailCfg.Rows[0]["SmtpEmail"]);
                string SmtpPassword = Convert.ToString(dtMailCfg.Rows[0]["SmtpPassword"]);


                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                //client.Host = ConfigurationManager.AppSettings["SmtpHost"];
                //client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpEmail"], ConfigurationManager.AppSettings["SmtpPassword"]);
                client.EnableSsl = Convert.ToBoolean(EnableSsl);
                client.Host = SmtpHost;
                client.Port = Convert.ToInt32(SmtpPort);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(SmtpEmail, SmtpPassword);
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;

                MailMessage msg = new MailMessage();
                //msg.Priority = MailPriority.High;
                msg.From = new MailAddress(fromMailID);
                string to = toEmailId;
                msg.To.Add(new MailAddress(to));
                msg.IsBodyHtml = true;
                msg.Subject = subject;
                msg.Body = message;
                try
                {
                    client.Send(msg);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #region For Entity
        /// <summary>
        ///
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static Exception ProcessServerException(string className, string methodName, Exception exception)
        {
            // Gets or sets update exception
            UpdateException updateException;

            // Checking Exception message ends with gti exception token (GTI_EXCEPTION)
            if (exception.Message.EndsWith(ERPUtilityResources.GTIExceptionWithToken) == false)
            {
                // Checking Exception is sql exception
                SqlException sqlException = exception.InnerException as SqlException;
                if (sqlException != null)
                {
                    // Returns a new exception to service class with class name - method name - sql exception exception with corresponding exception number as exception message
                    return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.SqlException + sqlException.Number.ToString() + ERPUtilityResources.GTIExceptionDetailsWithToken + sqlException.Message + ERPUtilityResources.GTIExceptionWithToken);
                }
                else
                {
                    // Checking Exception is optimistic concurrency exception
                    OptimisticConcurrencyException concurrencyException = exception.InnerException as OptimisticConcurrencyException;
                    if (concurrencyException != null)
                    {
                        // Returns a new exception to service class with class name - method name - concurrency exception with corresponding exception number as exception message
                        return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.OptimisticConcurrencyException + concurrencyException.GetHashCode().ToString() + ERPUtilityResources.GTIExceptionDetailsWithToken + concurrencyException.Message + ERPUtilityResources.GTIExceptionWithToken);
                    }
                    else
                    {
                        // Checking Exception is argument null exception
                        ArgumentNullException argException = exception as ArgumentNullException;
                        if (argException != null)
                        {
                            // Returns a new exception to service class with class name - method name - argument null exception with default exception number as exception message
                            return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.ArgumentNullException + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + argException.Message + ERPUtilityResources.GTIExceptionWithToken);
                        }
                        else
                        {
                            // Checking Exception is optimistic concurrency exception (concurrency exception from manager class)
                            concurrencyException = exception as OptimisticConcurrencyException;

                            if (concurrencyException != null)
                            {
                                // Returns a new exception to service class with class name - method name - concurrency exception with default exception number as exception message
                                return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.OptimisticConcurrencyException + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + concurrencyException.Message + ERPUtilityResources.GTIExceptionWithToken);
                            }
                            else
                            {
                                // Checking Exception is update exception (record already exists exception from manager class.thrown from manager class with page requirments)
                                updateException = exception as UpdateException;

                                if (updateException != null)
                                {
                                    if (updateException.Message.Equals(ERPUtilityResources.DateRangeException))
                                    {
                                        // Returns a new exception to service class with class name - method name - record already exists exception with default exception number as exception message
                                        return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.DateRangeException + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + updateException.Message + ERPUtilityResources.GTIExceptionWithToken);
                                    }
                                    else if (updateException.Message.Equals(ERPUtilityResources.OutofStockException))
                                    {
                                        // Returns a new exception to service class with class name - method name - out of stock exception with default exception number as exception message
                                        return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.OutofStockException + ERPUtilityResources.ExceptionToken + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + updateException.Message + ERPUtilityResources.GTIExceptionWithToken);
                                    }
                                    else if (updateException.Message.Equals(ERPUtilityResources.FromToRangeException))
                                    {
                                        // Returns a new exception to service class with class name - method name - out of stock exception with default exception number as exception message
                                        return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.FromToRangeException + ERPUtilityResources.ExceptionToken + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + updateException.Message + ERPUtilityResources.GTIExceptionWithToken);
                                    }
                                    else if (updateException.Message.Equals(ERPUtilityResources.ItemExistException))
                                    {
                                        // Returns a new exception to service class with class name - method name - out of stock exception with default exception number as exception message
                                        return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.ItemExistException + ERPUtilityResources.ExceptionToken + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + updateException.Message + ERPUtilityResources.GTIExceptionWithToken);
                                    }
                                    else if (updateException.Message.Equals(ERPUtilityResources.NoItemException))
                                    {
                                        // Returns a new exception to service class with class name - method name - out of stock exception with default exception number as exception message
                                        return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.NoItemException + ERPUtilityResources.ExceptionToken + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + updateException.Message + ERPUtilityResources.GTIExceptionWithToken);
                                    }
                                    else if (updateException.Message.Equals(ERPUtilityResources.ManyItemException))
                                    {
                                        // Returns a new exception to service class with class name - method name - out of stock exception with default exception number as exception message
                                        return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.ManyItemException + ERPUtilityResources.ExceptionToken + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + updateException.Message + ERPUtilityResources.GTIExceptionWithToken);
                                    }
                                    else if (updateException.Message.Equals(ERPUtilityResources.ActivityAircraftException))
                                    {
                                        // Returns a new exception to service class with class name - method name - out of stock exception with default exception number as exception message
                                        return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.ActivityAircraftException + ERPUtilityResources.ExceptionToken + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + updateException.Message + ERPUtilityResources.GTIExceptionWithToken);
                                    }
                                    else if (updateException.Message.Equals(ERPUtilityResources.ShiftAttendanceExistException))
                                    {
                                        // Returns a new exception to service class with class name - method name - out of stock exception with default exception number as exception message
                                        return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.ShiftAttendanceExistException + ERPUtilityResources.ExceptionToken + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + updateException.Message + ERPUtilityResources.GTIExceptionWithToken);
                                    }
                                    else
                                    {
                                        // Returns a new exception to service class with class name - method name - record already exists exception with default exception number as exception message
                                        return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.RecordAlreadyExistsException + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + updateException.Message + ERPUtilityResources.GTIExceptionWithToken);
                                    }
                                }
                                else
                                {
                                    // Returns a new exception to service class with class name - method name - unknown error with default exception number as exception message
                                    return new Exception(className + ERPUtilityResources.ExceptionToken + methodName + ERPUtilityResources.ExceptionToken + ERPUtilityResources.UnknownError + ERPUtilityResources.DefaultErrorNumber + ERPUtilityResources.GTIExceptionDetailsWithToken + exception.Message + ERPUtilityResources.GTIExceptionWithToken);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public static dynamic InitiateClient(dynamic Client)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CommonFunctions.IgnoreCertificateErrorHandler);
                Client.ClientCredentials.UserName.UserName = "gti";
                Client.ClientCredentials.UserName.Password = "123456";
                return Client;
            }
            catch
            {
                return Client;
            }
        }

        /// <summary>
        /// Checking Security Exception using certificate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public static bool IgnoreCertificateErrorHandler(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Method to initialize Entity Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Initilize<T>()
        {
            T src = Activator.CreateInstance<T>();
            Type targetType = src.GetType();

            //	Loop through the source properties
            foreach (PropertyInfo p in targetType.GetProperties())
            {
                if (p.CanWrite)
                {
                    Type ptype = p.PropertyType;

                    if (ptype == typeof(byte))
                        p.SetValue(src, byte.MinValue, null);
                    if (ptype == typeof(string))
                        p.SetValue(src, string.Empty, null);
                    else if (ptype == typeof(int))
                        p.SetValue(src, (int)-1, null);
                    //else if (ptype == typeof(int?))
                    //    p.SetValue(src, (int)-1, null);
                    else if (ptype == typeof(Int64))
                        p.SetValue(src, (Int64)(-1), null);
                    //else if (ptype == typeof(Int64?))
                    //    p.SetValue(src, (Int64)(-1), null);
                    else if (ptype == typeof(Int32))
                        p.SetValue(src, (Int32)(-1), null);
                    //else if (ptype == typeof(Int32?))
                    //    p.SetValue(src, (Int32)(-1), null);
                    else if (ptype == typeof(short))
                        //p.SetValue(src, (short)255, null);
                        p.SetValue(src, (short)-1, null);
                    //else if (ptype == typeof(short?))
                    //    p.SetValue(src, (short)255, null);
                    else if (ptype == typeof(float))
                        p.SetValue(src, (float)(0), null);
                    //else if (ptype == typeof(float?))
                    //    p.SetValue(src, (float)(-1), null);
                    else if (ptype == typeof(decimal))
                        p.SetValue(src, Convert.ToDecimal(0), null);
                    //else if (ptype == typeof(decimal?))
                    //    p.SetValue(src, Convert.ToDecimal(-1), null);
                    else if (ptype == typeof(bool))
                        p.SetValue(src, false, null);
                    //else if (ptype == typeof(bool?))
                    //    p.SetValue(src, false, null);
                    else if (ptype == typeof(DateTime))
                        p.SetValue(src, DateTime.MinValue, null);
                    //else if (ptype == typeof(DateTime?))
                    //    p.SetValue(src, DateTime.MinValue, null);
                }
            }

            return src;
        }


        /// <summary>
        /// Get formated list of objects for auto complete
        /// </summary>
        /// <param name="dynamicList">List for formatting</param>
        /// <param name="textProperty">Text property name</param>
        /// <param name="valueProperty">Value property name</param>
        /// <returns></returns>
        public static dynamic GetFormatedAutoCompleteComboBoxList(dynamic dynamicList, string textProperty, string valueProperty, string textPropertyCode)
        {
            // Gets or sets list of objects for auto complete
            List<TextValueComboBox> textValueList;
            // Gets the type of dynamic object in the list
            Type type;
            // Gets or sets text property information from dynamic object
            PropertyInfo textPropertyInfo;
            // Gets or sets value property information from dynamic object
            PropertyInfo valuePropertyInfo;

            PropertyInfo textPropertyForCodeInfo;
            try
            {
                // initilize new instance of TextValue class
                textValueList = new List<TextValueComboBox>();
                // Iterate through each object in the dynamic list
                foreach (dynamic dynamicObject in dynamicList)
                {
                    // Sets type of dynamic object
                    type = dynamicObject.GetType();
                    // Sets text property information
                    textPropertyInfo = type.GetProperty(textProperty);
                    // Sets value property information
                    valuePropertyInfo = type.GetProperty(valueProperty);

                    textPropertyForCodeInfo = type.GetProperty(textPropertyCode);

                    // Add object to text value list
                    textValueList.Add(new TextValueComboBox()
                    {
                        // Sets text propert after html decoding
                        Text = HttpUtility.HtmlDecode(Convert.ToString(textPropertyInfo.GetValue(dynamicObject, null))),
                        // Sets value property
                        Value = HttpUtility.HtmlDecode(Convert.ToString(valuePropertyInfo.GetValue(dynamicObject, null))),
                        Code = HttpUtility.HtmlDecode(Convert.ToString(textPropertyForCodeInfo.GetValue(dynamicObject, null)))
                    });
                }
                // Returns formated list for auto complete
                return textValueList;
            }
            catch (Exception ex)
            {
                // throws exception to calling function
                throw ex;
            }
            finally
            {
                // Disposing used objects
                textValueList = null;
                type = null;
                textPropertyInfo = null;
                valuePropertyInfo = null;
            }
        }

        /// <summary>
        /// Get formated list of objects for auto complete
        /// </summary>
        /// <param name="dynamicList">List for formatting</param>
        /// <param name="textProperty">Text property name</param>
        /// <param name="valueProperty">Value property name</param>
        /// <returns></returns>
        public static dynamic GetFormatedAutoCompleteList(dynamic dynamicList, string textProperty, string valueProperty)
        {
            // Gets or sets list of objects for auto complete
            List<TextValue> textValueList;
            // Gets the type of dynamic object in the list
            Type type;
            // Gets or sets text property information from dynamic object
            PropertyInfo textPropertyInfo;
            // Gets or sets value property information from dynamic object
            PropertyInfo valuePropertyInfo;
            try
            {
                // initilize new instance of TextValue class
                textValueList = new List<TextValue>();
                // Iterate through each object in the dynamic list
                foreach (dynamic dynamicObject in dynamicList)
                {
                    // Sets type of dynamic object
                    type = dynamicObject.GetType();
                    // Sets text property information
                    textPropertyInfo = type.GetProperty(textProperty);
                    // Sets value property information
                    valuePropertyInfo = type.GetProperty(valueProperty);
                    // Add object to text value list
                    textValueList.Add(new TextValue()
                    {
                        // Sets text propert after html decoding
                        Text = HttpUtility.HtmlDecode(Convert.ToString(textPropertyInfo.GetValue(dynamicObject, null))),
                        // Sets value property
                        Value = HttpUtility.HtmlDecode(Convert.ToString(valuePropertyInfo.GetValue(dynamicObject, null))),
                    });
                }
                // Returns formated list for auto complete
                return textValueList;
            }
            catch (Exception ex)
            {
                // throws exception to calling function
                throw ex;
            }
            finally
            {
                // Disposing used objects
                textValueList = null;
                type = null;
                textPropertyInfo = null;
                valuePropertyInfo = null;
            }
        }

        /// <summary>
        /// Get formated list of objects for auto complete
        /// </summary>
        /// <param name="dynamicList">List for formatting</param>
        /// <param name="textProperty1">Text property name</param>
        /// <param name="pairedProperty">Paired Text property name</param>
        /// <param name="valueProperty">Value property name</param>
        /// <returns></returns>
        public static dynamic GetFormatedAutoCompleteList(dynamic dynamicList, string textProperty, string pairedProperty, string valueProperty)
        {
            // Gets or sets list of objects for auto complete
            List<PairedValue> pairedValueList;
            // Gets the type of dynamic object in the list
            Type type;
            // Gets or sets text property information from dynamic object
            PropertyInfo textPropertyInfo;
            // Gets or sets value property information from dynamic object
            PropertyInfo valuePropertyInfo;
            // Gets or sets PairText property information from dynamic object
            PropertyInfo pairedPropertyInfo;
            try
            {
                // initilize new instance of TextValue class
                pairedValueList = new List<PairedValue>();
                // Iterate through each object in the dynamic list
                foreach (dynamic dynamicObject in dynamicList)
                {
                    // Sets type of dynamic object
                    type = dynamicObject.GetType();
                    // Sets text property information
                    textPropertyInfo = type.GetProperty(textProperty);
                    // Sets value property information
                    valuePropertyInfo = type.GetProperty(valueProperty);
                    // Add object to text value list
                    pairedPropertyInfo = type.GetProperty(pairedProperty);
                    pairedValueList.Add(new PairedValue()
                    {
                        // Sets text propert after html decoding
                        Text = HttpUtility.HtmlDecode(Convert.ToString(textPropertyInfo.GetValue(dynamicObject, null))),
                        // Sets value property
                        Value = HttpUtility.HtmlDecode(Convert.ToString(valuePropertyInfo.GetValue(dynamicObject, null))),
                        // Sets PairText property
                        PairText = HttpUtility.HtmlDecode(Convert.ToString(pairedPropertyInfo.GetValue(dynamicObject, null))),
                    });
                }
                // Returns formated list for auto complete
                return pairedValueList;
            }
            catch (Exception ex)
            {
                // throws exception to calling function
                throw ex;
            }
            finally
            {
                // Disposing used objects
                pairedValueList = null;
                type = null;
                textPropertyInfo = null;
                valuePropertyInfo = null;
                pairedPropertyInfo = null;
            }
        }

        /// <summary>
        /// Get formated list of objects for auto complete
        /// </summary>
        /// <param name="dynamicList">List for formatting</param>
        /// <param name="textProperty1">First Text property name</param>
        /// <param name="textProperty2">Second Text property name</param>
        /// <param name="valueProperty">Value property name</param>
        /// <param name="isCombined">whether the two Text Properties are concantinated</param>
        /// <returns></returns>
        public static dynamic GetFormatedAutoCompleteList(dynamic dynamicList, string textProperty1, string textProperty2, string valueProperty, bool isCombined)
        {
            // Gets or sets list of objects for auto complete
            List<TextValue> textValueList;
            // Gets the type of dynamic object in the list
            Type type;
            // Gets or sets text property information from dynamic object
            PropertyInfo textPropertyInfo1;
            // Gets or sets text property information from dynamic object
            PropertyInfo textPropertyInfo2;
            // Gets or sets value property information from dynamic object
            PropertyInfo valuePropertyInfo;
            try
            {
                // initilize new instance of TextValue class
                textValueList = new List<TextValue>();
                // Iterate through each object in the dynamic list
                foreach (dynamic dynamicObject in dynamicList)
                {
                    // Sets type of dynamic object
                    type = dynamicObject.GetType();
                    // Sets text property information
                    textPropertyInfo1 = type.GetProperty(textProperty1);
                    // Sets text property information
                    textPropertyInfo2 = type.GetProperty(textProperty2);
                    // Sets value property information
                    valuePropertyInfo = type.GetProperty(valueProperty);
                    // Add object to text value list
                    textValueList.Add(new TextValue()
                    {
                        // Sets text propert after html decoding
                        Text = HttpUtility.HtmlDecode(string.Format(ERPUtilityResources.NameCodeFormat, Convert.ToString(textPropertyInfo1.GetValue(dynamicObject, null)),
                        Convert.ToString(textPropertyInfo2.GetValue(dynamicObject, null)))),
                        // Sets value property
                        Value = HttpUtility.HtmlDecode(Convert.ToString(valuePropertyInfo.GetValue(dynamicObject, null))),
                    });
                }
                // Returns formated list for auto complete
                return textValueList;
            }
            catch (Exception ex)
            {
                // throws exception to calling function
                throw ex;
            }
            finally
            {
                // Disposing used objects
                textValueList = null;
                type = null;
                textPropertyInfo1 = null;
                textPropertyInfo2 = null;
                valuePropertyInfo = null;
            }
        }

        /// <summary>
        /// Get formated list of objects for auto complete
        /// </summary>
        /// <param name="dynamicList">List for formatting</param>
        /// <param name="textProperty1">First Text property name</param>
        /// <param name="textProperty2">Second Text property name</param>
        /// <param name="valueProperty">Value property name</param>
        /// <param name="isCombined">whether the two Text Properties are concantinated</param>
        /// <param name="format">string format </param>
        /// <returns></returns>
        public static dynamic GetFormatedAutoCompleteList(dynamic dynamicList, string textProperty1, string textProperty2, string valueProperty, bool isCombined, string format)
        {
            // Gets or sets list of objects for auto complete
            List<TextValue> textValueList;
            // Gets the type of dynamic object in the list
            Type type;
            // Gets or sets text property information from dynamic object
            PropertyInfo textPropertyInfo1;
            // Gets or sets text property information from dynamic object
            PropertyInfo textPropertyInfo2;
            // Gets or sets value property information from dynamic object
            PropertyInfo valuePropertyInfo;
            try
            {
                // initilize new instance of TextValue class
                textValueList = new List<TextValue>();
                // Iterate through each object in the dynamic list
                foreach (dynamic dynamicObject in dynamicList)
                {
                    // Sets type of dynamic object
                    type = dynamicObject.GetType();
                    // Sets text property information
                    textPropertyInfo1 = type.GetProperty(textProperty1);
                    // Sets text property information
                    textPropertyInfo2 = type.GetProperty(textProperty2);
                    // Sets value property information
                    valuePropertyInfo = type.GetProperty(valueProperty);
                    // Add object to text value list
                    textValueList.Add(new TextValue()
                    {
                        // Sets text propert after html decoding
                        Text = HttpUtility.HtmlDecode(string.Format(format, Convert.ToString(textPropertyInfo1.GetValue(dynamicObject, null)),
                        Convert.ToString(textPropertyInfo2.GetValue(dynamicObject, null)))),
                        // Sets value property
                        Value = HttpUtility.HtmlDecode(Convert.ToString(valuePropertyInfo.GetValue(dynamicObject, null))),
                    });
                }
                // Returns formated list for auto complete
                return textValueList;
            }
            catch (Exception ex)
            {
                // throws exception to calling function
                throw ex;
            }
            finally
            {
                // Disposing used objects
                textValueList = null;
                type = null;
                textPropertyInfo1 = null;
                textPropertyInfo2 = null;
                valuePropertyInfo = null;
            }
        }


        /// <summary>
        /// HTML Decode field in List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static List<T> HtmlDecode<T>(List<T> lst, string field)
        {
            lst.ForEach(itm => HtmlDecode(itm, field));
            return lst;
        }

        /// <summary>
        /// HTML Decode field of object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="field"></param>
        public static void HtmlDecode<T>(T item, string field)
        {
            Type type;
            PropertyInfo fieldPropertyInfo;
            type = item.GetType();
            fieldPropertyInfo = type.GetProperty(field);
            if (fieldPropertyInfo.PropertyType == typeof(string))
                fieldPropertyInfo.SetValue(item, HttpUtility.HtmlDecode(Convert.ToString(fieldPropertyInfo.GetValue(item, null))), null);
        }

        /// <summary>
        /// Add Date Time/Time script dynamically
        /// </summary>
        /// <param name="controlType"></param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        public static string GenerateDynamicScript(string controlType, string controlID, string hdfToRange, string fromDate, string hdfFrmDate)
        {
            string script = string.Empty;
            switch ((ControlTypes)(Enum.Parse(typeof(ControlTypes), controlType)))
            {
                case ControlTypes.TimePicker:
                    script = "$('input:text[id$=" + controlID + "]').timepicker();";
                    break;
                case ControlTypes.DateTime:
                    script = "GrandScriptUtils.DatePickerCommon('" + controlID + "');";
                    script = script + "$('input:text[id$=" + controlID + "_Time]').timepicker();";
                    break;
                case ControlTypes.Date:
                    script = "GrandScriptUtils.DatePickerCommon('" + controlID + "');";
                    break;
                case ControlTypes.DateRange:
                    script = "GrandScriptUtils.AddDateRangeCommon('" + fromDate + "', '" + hdfFrmDate + "', '" + controlID + "', '" + hdfToRange + "', false, false);";
                    break;
                case ControlTypes.MonthPicker:
                    script = "ControlID= '" + controlID + "';";
                    break;
            }

            return script;
        }

        /// <summary>
        /// Process multiline message
        /// </summary>
        /// <param name="heading"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static string ProcessMessageList(string heading, List<string> messages)
        {
            StringBuilder messageText;
            messageText = new StringBuilder();
            messageText.Append(heading);
            messageText.Append(messages.Count > 0 ? ERPUtilityResources.MsgStart : string.Empty);
            foreach (string message in messages)
            {
                messageText.Append(string.Format(ERPUtilityResources.MsgFormat, message));
            }
            messageText.Append(messages.Count > 0 ? ERPUtilityResources.MsgEnd : string.Empty);
            return messageText.ToString();
        }

        public static List<T> GetResults<T>(T element, ObjectContext ctx, string cmd)
        {
            // List of T elements to be returned.
            List<T> results = null;

            // Execute the query
            if (cmd == "")
            {
                results = null;
            }
            else
            {
                results = ctx.ExecuteStoreQuery<T>(cmd, null).ToList();
            }

            // Return the results
            return results;
        }

        public static int? NullableInt(string str)
        {
            int i;
            if (int.TryParse(str, out i))
                return i;
            return null;
        }

        #region enums
        enum ControlTypes
        {
            Page,
            Label,
            Text,
            DropDown,
            DateTime,
            Numeric,
            Button,
            Spacer,
            GridView,
            CheckBox,
            TextArea,
            TimePicker,
            Header,
            Table,
            Iframe,
            HiddenField,
            HourText,
            Date,
            DateRange,
            MonthPicker

        }
        #endregion
        #endregion

        /// <summary>
        /// For getting string as Html decoded
        /// </summary>
        /// <param name="orginalString"></param>
        /// <param name="limit"></param>
        /// <returns>string</returns>
        public static string GetDecodedString(object evelOrginalString)
        {
            string orginalString = HttpUtility.HtmlDecode(Convert.ToString(evelOrginalString));
            return orginalString;
        }

        /// <summary>
        /// For getting string as Html encoded
        /// </summary>
        /// <param name="orginalString"></param>
        /// <param name="limit"></param>
        /// <returns>string</returns>
        public static string GetEncodedString(object evelOrginalString)
        {
            string orginalString = HttpUtility.HtmlEncode(Convert.ToString(evelOrginalString));
            return orginalString;
        }

        public static string GetCurrency(object value)
        {
            string currency = string.Empty;
            decimal amount;
            if (value != null && Decimal.TryParse(value.ToString(), out amount))
            {
                currency = amount.ToString("c");
            }
            return currency;
        }
        public static string GetQty(object value)
        {
            string qty = string.Empty;
            decimal amount;
            if (value != null && Decimal.TryParse(value.ToString(), out amount))
            {
                qty = amount.ToString("N");
            }
            return qty;
        }

        /// <summary>
        /// Returns the Sum of specified column in the datatable
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dtSource"></param>
        /// <returns>Decimal. The source column should be in decimal format</returns>
        public static decimal GetColumnTotal(string columnName, DataTable dtSource)
        {
            decimal total = 0;
            try
            { total = dtSource.AsEnumerable().Sum(o => Convert.ToDecimal(o.Field<object>(columnName))); }
            catch
            { }
            return total;
        }

        public static decimal DecimalFormat(decimal value, int power)
        {
            //decimal result = Math.Truncate((decimal)((float)value * (float)Math.Pow(10, (double)power)));
            decimal result = Math.Truncate((decimal)value * (decimal)Math.Pow(10, (double)power));
            if (result > 0)
                result /= (decimal)Math.Pow(10, (double)power);
            return result;
        }

        public static double DoubleFormat(double value, int power)
        {
            double result = Math.Truncate(Math.Round(Convert.ToDouble(value) * Math.Pow(10, (double)power)));
            if (result > 0)
                result /= Math.Pow(10, (double)power);
            return result;
        }
        public static double DoubleFormatRound(double value, int power)
        {
            double result = Math.Truncate(Math.Round(Convert.ToDouble(value) * Math.Pow(10, (double)power)));
            if (result > 0)
                result /= Math.Pow(10, (double)power);
            return result;
        }

        public static DateTime SetDateWithConfiguration(string config)
        {
            string[] configs = config.Split('/');
            DateTime dateTime = DateTime.Now;
            int? relWeek = null;
            for (int index = 0; index < configs.Length; index++)
            {
                switch (index)
                {
                    case 0:
                        if (!string.IsNullOrEmpty(configs[index]))
                        {
                            dateTime = dateTime.AddYears(Convert.ToInt32(configs[index]));
                        }
                        break;
                    case 1:
                        string[] monthConfigs = configs[index].Split('|');
                        if (!string.IsNullOrEmpty(monthConfigs[0]))
                        {
                            dateTime = dateTime.AddMonths(Convert.ToInt32(monthConfigs[0]));
                        }
                        if (monthConfigs.Length == 2)
                        {
                            if (!string.IsNullOrEmpty(monthConfigs[1]))
                            {
                                int absMonth = Convert.ToInt32(monthConfigs[1]);
                                if (absMonth > 0 && absMonth <= 12)
                                {
                                    dateTime = dateTime.AddMonths(-dateTime.Month).AddMonths(absMonth);
                                }
                            }
                        }
                        break;
                    case 2:
                        if (!string.IsNullOrEmpty(configs[index]))
                        {
                            relWeek = Convert.ToInt32(configs[index]);
                            dateTime = dateTime.AddDays(relWeek.Value * 7);
                            //dateTime = dateTime.AddDays(-(int)DateTime.Now.DayOfWeek).AddDays(relWeek * 7);
                        }
                        break;
                    case 3:
                        if (!string.IsNullOrEmpty(configs[index]))
                        {
                            int relDays = Convert.ToInt32(configs[index]);
                            switch (relDays)
                            {
                                case 1:
                                    if (relWeek.HasValue)
                                    {
                                        //int dayofWeek = (int)DateTime.Now.DayOfWeek;
                                        int dayofWeek = (int)dateTime.DayOfWeek;
                                        dateTime = dateTime.AddDays(dayofWeek == 7 ? 0 : -dayofWeek);
                                    }
                                    else
                                    {
                                        dateTime = dateTime.AddDays(1 - dateTime.Day);
                                    }
                                    break;
                                case 2:
                                    if (relWeek.HasValue)
                                    {
                                        //int dayofWeek = (int)DateTime.Now.DayOfWeek;
                                        int dayofWeek = (int)dateTime.DayOfWeek;
                                        dateTime = dateTime.AddDays(dayofWeek == 7 ? 6 : 6 - dayofWeek);
                                    }
                                    else
                                    {
                                        dateTime = dateTime.AddMonths(1);
                                        dateTime = dateTime.AddDays(-dateTime.Day);
                                    }
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return dateTime;
        }

        /// <summary>
        /// For rate format O2C
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public static string GetFormattedRateO2C(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalDigits = HttpContext.Current.Session[ERP.Utilities.SessionStrings.RateDecimalDigit] != null ? Convert.ToInt32(HttpContext.Current.Session[ERP.Utilities.SessionStrings.RateDecimalDigit]) : Convert.ToInt32(ERPUtilityResources.DefaultRateO2C);
            nfi.NumberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            nfi.NumberGroupSizes = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes;
            nfi.NumberGroupSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            return num.ToString("N", nfi);
        }
        /// <summary>
        /// For rate format P2P
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public static string GetFormattedRateP2P(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalDigits = HttpContext.Current.Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] != null ? Convert.ToInt32(HttpContext.Current.Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]) : Convert.ToInt32(ERPUtilityResources.DefaultRateP2P);
            nfi.NumberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            nfi.NumberGroupSizes = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes;
            nfi.NumberGroupSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            return num.ToString("N", nfi);
        }

        public static bool IsNullOrEmptyOrWhitespace(string value)
        {
            return string.IsNullOrEmpty(value) || value.Trim() == "";
        }

        /// <summary>
        /// Function to add comma seperation to a number
        /// </summary>
        /// <param name="number">Number to add comma seperation</param>
        /// <param name="curGroup1">First seperation after how many digits</param>
        /// <param name="curGroup2">Remaining seperations after how many digits each</param>
        /// <returns></returns>
        public static string AddCommaSeperations(object number, int curGroup1 = 3, int curGroup2 = 3)
        {
            string NumberToFormat = Convert.ToString(number);
            string DecimalPart = string.Empty;
            string NumericPart = string.Empty;
            string LastNumericPart = string.Empty;
            string[] SplitedNmbrs = NumberToFormat.Split('.');
            if (SplitedNmbrs.Length > 1)
                DecimalPart = "." + SplitedNmbrs[1];

            NumericPart = SplitedNmbrs[0];
            if (NumericPart.Length > curGroup1)
            {
                LastNumericPart = NumericPart.Substring(NumericPart.Length - curGroup1, curGroup1);
                LastNumericPart = "," + LastNumericPart;
            }
            if ((NumericPart.Length - curGroup1) > 0)
            {
                NumericPart = NumericPart.Substring(0, NumericPart.Length - curGroup1);
                string pattern = "\\B(?=(\\d{" + curGroup2 + "})+(?!\\d))";
                Regex expression = new Regex(pattern);
                NumericPart = expression.Replace(NumericPart, ",");
            }
            NumberToFormat = NumericPart + LastNumericPart + DecimalPart;
            return NumberToFormat;

            //string NumberToFormat = Convert.ToString(number);
            //string DecimalPart = string.Empty;
            //string NumericPart = string.Empty;
            //string[] SplitedNmbrs = NumberToFormat.Split('.');
            //if (SplitedNmbrs.Length > 1)
            //    DecimalPart = "." + SplitedNmbrs[1];

            //NumericPart = SplitedNmbrs[0];
            //if (NumericPart.Length > curGroup1)
            //{
            //    DecimalPart = "," + NumericPart.Substring(NumericPart.Length - curGroup1, curGroup1) + DecimalPart;
            //    NumericPart = NumericPart.Substring(0, NumericPart.Length - curGroup1);
            //    while (NumericPart.Length > curGroup2)
            //    {
            //        DecimalPart = "," + NumericPart.Substring(NumericPart.Length - curGroup2, curGroup2) + DecimalPart;
            //        NumericPart = NumericPart.Substring(0, NumericPart.Length - curGroup2);
            //    }
            //}

            //return NumericPart + DecimalPart;
        }
        /// <summary>
        /// For Set database connection to crystal report engine
        /// </summary>
        /// <param name="reportDocument"></param>
        public static void SetCrystalReportDataBaseConnection(ReportDocument reportDocument,bool IsReportServer=false)
        {
            string connectString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if(IsReportServer)
                connectString = ConfigurationManager.ConnectionStrings["ReportConnectionString"].ToString();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);

            ConnectionInfo connectionInf = new ConnectionInfo();
            connectionInf.ServerName = builder.DataSource;
            connectionInf.UserID = builder.UserID;
            connectionInf.Password = builder.Password;
            connectionInf.DatabaseName = builder.InitialCatalog;
            TableLogOnInfo crtablelogoninfo = new TableLogOnInfo();
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in reportDocument.Database.Tables)
            {
                crtablelogoninfo = CrTable.LogOnInfo;
                crtablelogoninfo.ConnectionInfo = connectionInf;
                CrTable.ApplyLogOnInfo(crtablelogoninfo);
            }
            ////Connection Time Out
            //for (int i = 0; i < reportDocument.DataSourceConnections.Count; i++)
            //{
            //    NameValuePairs2 lp = reportDocument.DataSourceConnections[i].LogonProperties;
            //    lp.Set("Connection Timeout", "100");
            //    reportDocument.DataSourceConnections[i].SetLogonProperties(lp);
            //}

            //Subreports subReports = reportDocument.Subreports;
            //for (int i = 0; i < subReports.Count; i++)
            //{
            //    for (int j = 0; j < subReports[i].DataSourceConnections.Count; j++)
            //    {
            //        NameValuePairs2 lp = subReports[i].DataSourceConnections[j].LogonProperties;
            //        lp.Set("Connection Timeout", "100");
            //        subReports[i].DataSourceConnections[j].SetLogonProperties(lp);
            //    }
            //}
        }

        /// <summary>
        /// Method to remove HTML tags from a string
        /// </summary>
        /// <param name="input">String</param>
        /// <returns>String without HTML tags</returns>
        public static string RemoveHTML(object inputString)
        {
            return Regex.Replace(Convert.ToString(inputString), "<.*?>", String.Empty);
        }

        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        public static Object DeserializeObject(String pXmlizedString, Object pObject)
        {
            XmlSerializer xs = new XmlSerializer(pObject.GetType());
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            object rObject = xs.Deserialize(memoryStream);
            memoryStream.Close();
            memoryStream.Dispose();
            return rObject;
        }

        public static bool IsMultyCurrencyEnabled()
        {
            try
            {
                return Convert.ToBoolean(Convert.ToInt32(HttpContext.GetGlobalResourceObject("ConfigurationsRes", "HrmsMultiCurrencyEnabled")));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// For currency format for grid
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public static string GetFormattedCurrencyGrid(object number)
        {
            double retValue = 0;
            double.TryParse(Convert.ToString(number), out retValue);
            string format = "#,##0.";
            int rateDecimalDigits = Convert.ToInt32(Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString());
            for (int i = 0; i < rateDecimalDigits; i++)
            {
                format += "0";
            }
            return retValue.ToString(format);
        }


        /// <summary>
        /// Get Mail CFG
        /// BL Not Access from here bcs round call conflict
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMailCFGDA()
        {
            DataTable dtMailCfg;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            dtMailCfg = dbService.DataAdapter(CommandType.StoredProcedure, "SPADM_MAIL_CONFIG_GET", colParameters).Tables[0];
            return dtMailCfg;
        }

        /// <summary>
        /// Get Report File URL
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="clientCode"></param>
        /// <param name="roortFolder"></param>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        public static string GetReportFileURL(string fileName, string clientCode, string roortFolder = "Reports", string subFolder = "")
        {
            string retURL = string.Empty;
            string reportFilePath = string.Empty;
            string rptFileName = string.IsNullOrEmpty(subFolder) ? fileName : (subFolder + @"\" + fileName);
            //~Reports/WARM/xyz.rdlc"
            //~Reports/WARM/CrystalReportFiles/xyz.rpt

            //~Reports/xyz.rdlc"
            //~Reports/CrystalReportFiles/xyz.rpt

            string clientCodeURL =System.Web.HttpContext.Current.Server.MapPath(string.Format(@"~/{0}/{1}/{2}", roortFolder, clientCode, rptFileName));
           
            #region Client Specific File Location
            if (!string.IsNullOrEmpty(clientCode))
                if (File.Exists(clientCodeURL))
                    return clientCodeURL;
            #endregion

            #region Default File Location
            string fallBackURL = System.Web.HttpContext.Current.Server.MapPath(string.Format(@"~/{0}/{1}", roortFolder, rptFileName));
            if (File.Exists(fallBackURL))
                return fallBackURL;
            #endregion

            return retURL;
        }
        /// <summary>
        /// For exchange rate format 
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public static string GetFormattedExchangeRate(object number)
        {
            double retValue = 0;
            double.TryParse(Convert.ToString(number), out retValue);
            string format = "#0.";
            int exchrateDecimalDigits = (HttpContext.Current.Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(HttpContext.Current.Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
            for (int i = 0; i < exchrateDecimalDigits; i++)
            {
                format += "0";
            }
            return retValue.ToString(format);
        }
        #region Decimal Digit Format
        /// <summary>
        ///  For number format
        /// </summary>
        /// <param name="number"></param>
        /// <returns>string</returns>
        public static string GetFormattedNumber(object number)
        {
            double retValue = 0;
            double.TryParse(Convert.ToString(number), out retValue);
            string format = "#0.";
            int numberDecimalDigits = Convert.ToInt32(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString());
            for (int i = 0; i < numberDecimalDigits; i++)
            {
                format += "0";
            }
            return retValue.ToString(format);
        }
        #endregion
        /// <summary>
        /// this Method is used to Decode object 
        /// </summary>
        /// <param name="val">Object</param>
        /// <returns>string</returns>
        public static string HtmlDecodeUtility(object val)
        {
            if (val == null) return string.Empty;
            return HttpUtility.HtmlDecode(val.ToString());
        }
       
        
    }
}
