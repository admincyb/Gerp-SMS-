using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Data;
using System.Web.Script.Serialization;
using BusinessObject;
using System.Xml.Serialization;
using System.Xml;
using System.Net.Mail;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Text.RegularExpressions;
namespace GTIService
{
    /// <remarks>
    /// This class is for specifying common functions
    /// </remarks>
    /// <createdby>AMS Team</createdby>
    /// <creadteddate>25/05/2010</creadteddate> 

    public class CommonFunctions
    {
        #region Public Methods

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
        /// This method insert a space in between a "Capital case letter" and small case leeter 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>        
        public static string ProperSpace(string text)
        {
            StringBuilder sb = new StringBuilder();
            string lowered = text.ToLower();

            for (int i = 0; i < text.Length; i++)
            {
                string a = text.Substring(i, 1);

                string b = lowered.Substring(i, 1);
                if (a != b)
                    sb.Append(" ");

                sb.Append(a);
            }

            return sb.ToString().Trim();
        }

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
        /// format the input string;
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string FormatInputString(string inputString)
        {
            return "'" + inputString.Trim().Replace("'", "''") + "'";
        }

        #region Encrtion and Decryption

        /// <summary>
        /// to encript 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string Encrypt(string queryString)
        {
            return EncryptString(queryString, "56565656");
        }

        /// <summary>
        /// to decript
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string Decrypt(string queryString)
        {
            if (!(queryString == null))
            {
                queryString = queryString.Replace(" ", "+");
                return DecryptString(queryString, "56565656");
            }

            else
            {
                return queryString;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string DecryptKey(string queryString)
        {
            if (!(queryString == null))
            {
                return DecryptString(queryString, "56565656");
            }

            else
            {
                return queryString;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sInputstring"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string EncryptString(string sInputstring, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            ICryptoTransform desencrypt = DES.CreateEncryptor();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptostream = new CryptoStream(memoryStream, desencrypt, CryptoStreamMode.Write);

            byte[] bytearrayinput = Encoding.UTF8.GetBytes(sInputstring);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.FlushFinalBlock();
            byte[] bytes = memoryStream.ToArray();
            cryptostream.Close();
            memoryStream.Close();
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sInputString"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string DecryptString(string sInputString, string sKey)
        {
            string text = string.Empty;
            try
            {
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                byte[] inputByteArray = Convert.FromBase64String(sInputString);
                MemoryStream memoryStream = new MemoryStream(inputByteArray);
                ICryptoTransform desdecrypt = DES.CreateDecryptor();

                CryptoStream cryptostream = new CryptoStream(memoryStream, desdecrypt, CryptoStreamMode.Read);

                byte[] textbyte = new byte[inputByteArray.Length];
                int decryptedByteCount = cryptostream.Read(textbyte, 0, inputByteArray.Length);
                memoryStream.Close();
                cryptostream.Close();
                return text = Encoding.UTF8.GetString(textbyte, 0, decryptedByteCount);

            }
            catch
            {
                return text;
            }

        }

        #endregion

        /// <summary>
        /// Returns Key Value List based on supplied data table
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TextField"></param>
        /// <param name="ValueField"></param>
        /// <returns></returns>
        public static string GetTextValueList(DataTable dt, string TextField, string ValueField)
        {
            JavaScriptSerializer mySerializer = new JavaScriptSerializer();
            List<BusinessObject.Common.TextValue> TextValueList = new List<BusinessObject.Common.TextValue>();
            foreach (DataRow dr in dt.Rows)
            {
                TextValueList.Add(new BusinessObject.Common.TextValue() { Text =HttpUtility.HtmlDecode(Convert.ToString(dr[TextField])), Value = Convert.ToString(dr[ValueField]) });
            }
            return mySerializer.Serialize(TextValueList);
        }
        
        /// <summary>
        /// Returns Key Value List based on supplied data table
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TextField1"></param>
        /// <param name="TextField2"></param>
        /// <param name="ValueField"></param>
        /// <returns></returns>
        public static string GetTextValueList(DataTable dt, string TextField1,string TextField2, string ValueField)
        {
            JavaScriptSerializer mySerializer = new JavaScriptSerializer();
            List<BusinessObject.Common.TextValue> TextValueList = new List<BusinessObject.Common.TextValue>();
            foreach (DataRow dr in dt.Rows)
            {
                TextValueList.Add(new BusinessObject.Common.TextValue() { Text = Convert.ToString(dr[TextField1]) + Convert.ToString(dr[TextField2]), Value = Convert.ToString(dr[ValueField]) });
            }
            return mySerializer.Serialize(TextValueList);
        }

        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="toEmailId"></param>      
        /// <modifiedby></modifiedby>
        /// <modifieddate></modifieddate>
        //public static bool SendMail(string subject, string message, string toEmailId)
        //{
        //    if ((toEmailId != string.Empty))
        //    {
        //        SmtpClient client = new SmtpClient();
        //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
        //        client.Host = ConfigurationManager.AppSettings["SmtpHost"];
        //        client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
        //        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpEmail"], ConfigurationManager.AppSettings["SmtpPassword"]);
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = credentials;

        //        MailMessage msg = new MailMessage();
        //        //msg.Priority = MailPriority.High;
        //        msg.From = new MailAddress(ConfigurationManager.AppSettings["SmtpEmail"]);
        //        string to = toEmailId;
        //        msg.To.Add(new MailAddress(to));
        //        msg.IsBodyHtml = true;
        //        msg.Subject = subject;
        //        msg.Body = message;
        //        try
        //        {
        //            client.Send(msg);
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// Returns Key Value List based on supplied data table and add Initail Text And val if providied
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TextField"></param>
        /// <param name="ValueField"></param>
        /// <returns></returns>
        public static string GetTextValueList(DataTable dt, string TextField, string ValueField, string InitialText, string Initialval)
        {
            JavaScriptSerializer mySerializer = new JavaScriptSerializer();
            List<BusinessObject.Common.TextValue> TextValueList = new List<BusinessObject.Common.TextValue>();
            int flag = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (flag == 0)
                {
                    TextValueList.Add(new BusinessObject.Common.TextValue() { Text = InitialText, Value = Initialval });
                    flag = 1;
                }
                TextValueList.Add(new BusinessObject.Common.TextValue() { Text = Convert.ToString(dr[TextField]), Value = Convert.ToString(dr[ValueField]) });
            }
            return mySerializer.Serialize(TextValueList);
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ValueField"></param>
        /// <param name="TextField"></param>
        /// <param name="ParentValueField"></param>
        /// <param name="HasChildField"></param>
        /// <param name="CheckedField"></param>
        /// <param name="EditField"></param>
        /// <returns></returns>
        public static string GetTreeList(DataTable dt, string ValueField, string TextField, string ParentValueField, string HasChildField, string CheckedField, string EditField, string IsSave)
        {
            List<BusinessObject.Common.JsonMultiTreeList> jsonMultiTreeList = new List<BusinessObject.Common.JsonMultiTreeList>();
            foreach (DataRow dr in dt.Rows)
            {
                jsonMultiTreeList.Add(new BusinessObject.Common.JsonMultiTreeList()
                {
                    TreeValue = Convert.ToString(dr[ValueField]),
                    TreeText = Convert.ToString(dr[TextField]),
                    TreeParentValue = Convert.ToString(dr[ParentValueField]),
                    TreeHasChild = (HasChildField != string.Empty) ? Convert.ToString(dr[HasChildField]) : string.Empty,
                    TreeChecked = (CheckedField != string.Empty) ? Convert.ToString(dr[CheckedField]) : string.Empty,
                    TreeShowEdit = (EditField != string.Empty) ? Convert.ToString(dr[EditField]) : string.Empty,
                    TreeSave = (IsSave != string.Empty) ? Convert.ToString(dr[IsSave]) : string.Empty,
                });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(jsonMultiTreeList);
        }

        /// <summary>
        /// Returns the Grid paging and other parameters from request
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static GridPrams GetGridParams(HttpRequest Request)
        {
            GridPrams grid = new GridPrams();
            string searchVal = string.Empty;
            if (Request.Params["Page"] != null)
            {
                grid.PageNumber = int.Parse(Request.Params["Page"].Trim());
            }
            if (Request.Params["PageSize"] != null)
            {
                grid.PageSize = int.Parse(Request.Params["PageSize"].Trim());
            }

            if (Request.Params["Columns"] != null)
            {
                grid.Fields = Request.Params["Columns"].Trim();
                grid.Fields = grid.Fields.Substring(0, grid.Fields.LastIndexOf(","));
                char[] str = new char[1];
                str[0] = ',';
                grid.Fields = grid.Fields.TrimStart(str);

            }
            if (Request.Params["SortColumn"] != null)
            {
                grid.SortBy = Request.Params["SortColumn"].Trim();
            }
            if (Request.Params["SortOrder"] != null)
            {
                grid.SortDirection = Request.Params["SortOrder"].Trim();
            }
            if (Request.Params["SortColumn1"] != null)
            {
                grid.SortBy1 = Request.Params["SortColumn1"].Trim();
            }
            if (Request.Params["SortOrder1"] != null)
            {
                grid.SortDirection1 = Request.Params["SortOrder1"].Trim();
            }
            if (Request.Params["FromDate"] != null)
            {
                grid.FromDate = Request.Params["FromDate"].Trim();
            }
            if (Request.Params["ToDate"] != null)
            {
                grid.ToDate = Request.Params["ToDate"].Trim();
            }
            if (Request.Params["FilterStatus"] != null)
            {
                grid.FilterStatus = Request.Params["FilterStatus"].Trim();
            }
            if (Request.Params["Status"] != null)
            {
                grid.SearchBy = Request.Params["Status"].Trim();
            }
            if (Request.Params["SearchValue"] != null)
            {
                #region Old Code
                /* 
                  if (Request.Params["SearchValue"].Contains("'"))
                {
                    grid.SearchValue = Request.Params["SearchValue"].ToString().Replace("'", "\'\'");
                }
                else if (Request.Params["SearchValue"].Contains("ampersand"))
                {
                    string SearchValueUndo = "";
                    SearchValueUndo = Request.Params["SearchValue"].ToString();
                    grid.SearchValue = SearchValueUndo.Replace("ampersand", "&");
                }
                else if (Request.Params["SearchValue"].Contains("singlequote;"))
                {
                    string SearchValueUndo = "";
                    SearchValueUndo = Request.Params["SearchValue"].ToString();
                    grid.SearchValue = SearchValueUndo.Replace("singlequote;", "\'\'");
                }
                else
                {
                    grid.SearchValue = Request.Params["SearchValue"].ToString();
                }
                 */
                #endregion
                searchVal = Request.Params["SearchValue"].ToString();
                searchVal = HttpUtility.UrlDecode(searchVal);//In js page encodeURIComponent(SearchVal)
                if (searchVal.Contains("'"))
                {
                    searchVal = searchVal.Replace("'", "\'\'");
                }
                if (searchVal.Contains("ampersand"))
                {
                    searchVal = searchVal.Replace("ampersand", "&");
                }
                    grid.SearchValue = searchVal;
            }
            if (Request.Params["Flag"] != null)
            {
                grid.Flag = int.Parse(Request.Params["Flag"].Trim());
            }
            if (Request.Params["UserPK"] != null)
            {
                grid.UserPK = int.Parse(Request.Params["UserPK"].Trim());
            }
            if (Request.Params["DeptPK"] != null)
            {
                grid.DeptPK = int.Parse(Request.Params["DeptPK"].Trim());
            }
            if (Request.Params["P_DeptPK"] != null)
            {
                grid.P_DeptPK = int.Parse(Request.Params["P_DeptPK"].Trim());
            }
            if (Request.Params["CancelFlag"] != null)
            {
                grid.CancelFlag = int.Parse(Request.Params["CancelFlag"].Trim());
            }
            if (Request.Params["ShowAll"] != null)
            {
                grid.ShowAll = int.Parse(Request.Params["ShowAll"].Trim());
            }
            if (Request.Params["MenuType"] != null)
            {
                grid.POH_MENU_TYPE = int.Parse(Request.Params["MenuType"].Trim());
            }

            return grid;
        }

        /// <summary>
        /// Returns the JSON string formated to support auto complete text box
        /// </summary>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public static string GetAutocompleteString(DataTable dtMenu)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dtMenu.Rows)
            {
                sb.Append(string.Format("{0}~${1}",
                         dr["SearchValue"], Environment.NewLine));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Prepares the response with provided content type
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="ContentType"></param>
        public static void PrepareResponse(HttpResponse Response, string ContentType)
        {
            Response.Clear();
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            Response.ContentType = ContentType;
            Response.Charset = "utf-8";
        }

        /// <summary>
        /// Used to Evaluate an Expression
        /// </summary>
        /// <param name="expression"></param>
        public static bool Evaluateexpression(string expression)
        {
            bool status = false;
            try
            {
                Evaluator.EvaluateToDouble(expression);
                status = true;
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        #region XmlSerialize/DeSerialize

        private static String UTF8ByteArrayToString(Byte[] characters)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        public static String SerializeObject(Object pObject)
        {
            try
            {
                String XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(pObject.GetType());
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xs.Serialize(xmlTextWriter, pObject);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                XmlizedString = XmlizedString.Replace("encoding=\"utf-8\"", "");
                XmlizedString = XmlizedString.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                memoryStream.Close();
                memoryStream.Dispose();

                return XmlizedString;
            }
            catch (Exception e) { System.Console.WriteLine(e); return null; }
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

        public static XmlDocument ObjectTOXml(object obj)
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


        #endregion


        #region JsonToXml/XmlToJson
        public static string XmlToJson(string xDocReceiving)
        {
            string startBlock = "{\"root\":";
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xDocReceiving);
            var xmlString = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xDoc.FirstChild);
            xmlString = xmlString.Replace(startBlock, "");
            return xmlString.Substring(0, xmlString.LastIndexOf("}"));
        }

        public static string XmlToJson1(string xDocReceiving)
        {
            string startBlock = "{\"Root\":";
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xDocReceiving);
            var xmlString = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xDoc.FirstChild);
            xmlString = xmlString.Replace(startBlock, "");
            return xmlString.Substring(0, xmlString.LastIndexOf("}"));
        }

        public static string JsonToXml(string jsonString)
        {
            string startBlock = "{'root':";
            string endBlock = "}";
            //Adding A common Parent Node to the json String
            jsonString = startBlock + jsonString + endBlock;
            //Converting the json String to Xml
            XmlDocument xDoc = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(jsonString);
            return xDoc.InnerXml;
        }

        public static string removeInvalidElement(string xmlData, string elements)
        {
            StringBuilder patternBuilder = new StringBuilder();
            foreach (string element in elements.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                if (patternBuilder.Length == 0) patternBuilder.Append("(");
                else patternBuilder.Append("|");
                patternBuilder.Append("(<" + element + @"(>([A-Za-z0-9 ~`!@#$%&.\(\)\{\}\+\\\/\t\r\n\/<>_]+)?<\/" + element + ">| ?/>))");
            }
            patternBuilder.Append(")");
            Regex regex = new Regex(patternBuilder.ToString(), RegexOptions.Multiline);
            return regex.Replace(xmlData, String.Empty);
        }
        #endregion


        public static string GetMimeType(string strFileName)
        {
            string retval = "";
            switch (strFileName)
            {

                case "image/bmp": retval = ".bmp"; break;
                case "image/gif": retval = ".gif"; break;
                case "image/jpeg": retval = ".jpeg"; break;
                case "image/png": retval = ".png"; break;
                case "image/tiff": retval = ".tiff"; break;
                case "image/pjpeg": retval = ".jpeg"; break;
                case "image/x-png": retval = ".gif"; break;

                default: retval = "Invalid"; break;
            }
            return retval;
        }

        public static string checkValidFileType(string extn)
        {
            string retval = "";
            switch (extn.ToLower())
            {
                //case ".bmp": break;
                //case ".gif": break;
                //case ".jpeg": break;
                //case ".jpg": break;
                //case ".png": break;
                //case ".tiff": break;
                //case ".xls": break;
                //case ".pdf": break;
                //case ".doc": break;
                //case ".txt": break;
                //case ".docx": break;
                //case ".xlsx": break;
                //case ".odt": break;
                //case ".odp": break;
                //case ".ods": break;
                case ".jpg": break;
                case ".jpeg": break;
                case ".gif": break;
                case ".png": break;
                case ".tiff": break;
                case ".doc": break;
                case ".docx": break;
                case ".xls": break;
                case ".xlsx": break;
                case ".ppt": break;
                case ".pdf": break;
                case ".txt": break;
                default: retval = "Invalid"; break;
            }
            return retval;
        }

        public static string GetContentType(string extension)
        {
            string retval = "";
            switch (extension.ToLower())
            {

                case ".bmp": retval = "image/bmp"; break;
                case ".gif": retval = "image/gif"; break;
                case ".jpeg": retval = "image/jpeg"; break;
                case ".png": retval = "image/png"; break;
                case ".tiff": retval = "image/tiff"; break;
                case ".doc": retval = "application/octet-stream"; break;
                case ".xls": retval = "application/octet-stream"; break;
                case ".pdf": retval = "application/pdf"; break;
                default: retval = "Invalid"; break;
            }
            return retval;
        }

        /// <summary>
        /// Methord Used to Encript The DB Parameters And Procedures
        /// </summary>
        /// <param name="toEncript"></param>
        /// <param name="key"></param>
        public static string EncriptDB(string toEncript, int key)
        {
            //Declare a string with wovels Seperated by ','
            string wovels = "A,E,I,O,U";
            //Splitting the string with ',' and assigned to an array
            string[] arrWovel = wovels.Split(',');
            //loop through arrWovwl to find the string containes the wovel
            for (int i = 0; i < arrWovel.Length; i++)
            {
                //Check the string contains the current wovel in the array element
                if (toEncript.ToUpper().Contains(arrWovel[i]))
                {
                    //Replacing the Wovel in the string with the Array position + Key Provided
                    toEncript = toEncript.ToUpper().Replace(arrWovel[i], (i + key).ToString());
                }
            }
            //This is Used to Reverse the Encripted string
            char[] arr = toEncript.ToUpper().ToCharArray();
            Array.Reverse(arr);
            return new string(arr);

        }


        /// <summary>
        /// Methord Used to Decript The DB Parameters And Procedures
        /// </summary>
        /// <param name="toDecript"></param>
        /// <param name="key"></param>
        public static string DecriptDB(string toDecript, int key)
        {
            //This is Used to Reverse the Decripted String 
            char[] arr = toDecript.ToUpper().ToCharArray();
            Array.Reverse(arr);
            toDecript = new string(arr);

            //Declare a string with wovels Seperated by ','
            string wovels = "A,E,I,O,U";
            //Splitting the string with ',' and assigned to an array
            string[] arrWovel = wovels.Split(',');
            //loop through arrWovwl to find the string containes the wovel
            for (int i = 0; i < arrWovel.Length; i++)
            {
                //Check the string contains the (current wovel + Key) Number
                if (toDecript.ToUpper().Contains((i + key).ToString()))
                {
                    //Replacing the Wovel + Key with the Wovel in the array element
                    toDecript = toDecript.ToUpper().Replace((i + key).ToString(), arrWovel[i]);
                }
            }
            //Returning the Decripted Strinig
            return toDecript;


        }
        /// <summary>
        /// This Function is used to send mail
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="toEmailId"></param>
        /// <returns>bool:success=true,fail:false</returns>
        public bool SendMailOld(string subject, string message, string toEmailId, string fromMailID)
        {
            bool status = false;
            if ((toEmailId != string.Empty))
            {

                //   DataTable dtMailCfg =CommonBL Comm MailCore.DL.ViewMailsDA.GetMailCFGDA();
                DataTable dtMailCfg = ERP.Utilities.CommonFunctions.GetMailCFGDA();
                string SmtpHost = Convert.ToString(dtMailCfg.Rows[0]["SmtpHost"]);
                string SmtpPort = Convert.ToString(dtMailCfg.Rows[0]["SmtpPort"]);
                string EnableSsl = Convert.ToString(dtMailCfg.Rows[0]["EnableSsl"]);
                string SmtpEmail = Convert.ToString(dtMailCfg.Rows[0]["SmtpEmail"]);
                string SmtpPassword = Convert.ToString(dtMailCfg.Rows[0]["SmtpPassword"]);

                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
               //client.EnableSsl = true;
               // client.Host = ConfigurationManager.AppSettings["SmtpHost"];
               // client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
               // System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpEmail"], ConfigurationManager.AppSettings["SmtpPassword"]);
                client.EnableSsl = Convert.ToBoolean(EnableSsl);
                client.Host = SmtpHost;
                client.Port = Convert.ToInt32(SmtpPort);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(SmtpEmail, SmtpPassword);
                

                client.UseDefaultCredentials = false;
                client.Credentials = credentials;
                MailMessage msg = new MailMessage();
                //msg.Priority = MailPriority.High;
                //msg.From = new MailAddress(ConfigurationManager.AppSettings["SmtpEmail"]);
                msg.From = new MailAddress(fromMailID);
                string to = toEmailId;
                msg.To.Add(new MailAddress(to));
                msg.IsBodyHtml = true;
                msg.Subject = subject;
                msg.Body = message;
                try
                {

                    client.Send(msg);
                    status = true;

                }

                catch
                {
                    status = false;
                }

            }

            return status;


        }
        /// <summary>
        /// This Function is used to send mail template.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="templateID"></param>
        /// <param name="toEmailId"></param>
        /// <returns>bool:success=true,fail:false</returns>
        public bool SendMailTemplate(string subject, int templateID, string toEmailId,string fromMailID)
        {
            bool status = false;
            if ((toEmailId != string.Empty))
            {
                string template = string.Empty;
                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                DataTable dtMailCfg = ERP.Utilities.CommonFunctions.GetMailCFGDA();
                string SmtpHost = Convert.ToString(dtMailCfg.Rows[0]["SmtpHost"]);
                string SmtpPort = Convert.ToString(dtMailCfg.Rows[0]["SmtpPort"]);
                string EnableSsl = Convert.ToString(dtMailCfg.Rows[0]["EnableSsl"]);
                string SmtpEmail = Convert.ToString(dtMailCfg.Rows[0]["SmtpEmail"]);
                string SmtpPassword = Convert.ToString(dtMailCfg.Rows[0]["SmtpPassword"]);

                //client.Host = ConfigurationManager.AppSettings["SmtpHost"];
                //client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpEmail"], ConfigurationManager.AppSettings["SmtpPassword"]);
                client.Host = SmtpHost;
                client.Port = Convert.ToInt32(SmtpPort);
                client.EnableSsl = Convert.ToBoolean(EnableSsl);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(SmtpEmail,SmtpPassword);


                client.UseDefaultCredentials = false;
                client.Credentials = credentials;
                MailMessage msg = new MailMessage();
                //msg.Priority = MailPriority.High;
                //msg.From = new MailAddress(ConfigurationManager.AppSettings["SmtpEmail"]);
                msg.From = new MailAddress(fromMailID);
                string to = toEmailId;
                msg.To.Add(new MailAddress(to));
                msg.IsBodyHtml = true;
                msg.Subject = subject;
                msg.Body = template;
                try
                {
                    client.Send(msg);
                    status = true;
                }

                catch
                {
                    status = false;
                }


            }

            return status;
        }

        /// <summary>
        /// This Function is used to send mail template.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="templateTitle"></param>
        /// <param name="toEmailId"></param>
        /// <returns>bool:success=true,fail:false</returns>
        public bool SendMailTemplate(string subject, string templateTitle, string toEmailId, string fromMailID)
        {
            bool status = false;
            if ((toEmailId != string.Empty))
            {

                string template = string.Empty;
                DataTable dtMailCfg = ERP.Utilities.CommonFunctions.GetMailCFGDA();
                string SmtpHost = Convert.ToString(dtMailCfg.Rows[0]["SmtpHost"]);
                string SmtpPort = Convert.ToString(dtMailCfg.Rows[0]["SmtpPort"]);
                string EnableSsl = Convert.ToString(dtMailCfg.Rows[0]["EnableSsl"]);
                string SmtpEmail = Convert.ToString(dtMailCfg.Rows[0]["SmtpEmail"]);
                string SmtpPassword = Convert.ToString(dtMailCfg.Rows[0]["SmtpPassword"]);

                //template=
                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                //client.EnableSsl = true;
                //client.Host = ConfigurationManager.AppSettings["SmtpHost"];
                //client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpEmail"], ConfigurationManager.AppSettings["SmtpPassword"]);
                client.EnableSsl = Convert.ToBoolean(EnableSsl);
                client.Host = SmtpHost;
                client.Port = Convert.ToInt32(SmtpPort);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(SmtpEmail,SmtpPassword);
                
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;
                MailMessage msg = new MailMessage();
                //msg.Priority = MailPriority.High;
                //msg.From = new MailAddress(ConfigurationManager.AppSettings["SmtpEmail"]);
                msg.From = new MailAddress(fromMailID);
                string to = toEmailId;
                msg.To.Add(new MailAddress(to));
                msg.IsBodyHtml = true;
                msg.Subject = subject;
                msg.Body = template;

                try
                {

                    client.Send(msg);
                    status = true;

                }

                catch
                {
                    status = false;
                }


            }

            return status;


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
        /// This method format array of error messages
        /// </summary>
        ///<param name="errMsgs">Array of error messages</param>
        ///<returns>string : formated list of error message</returns>
        public static string FormatErrorMessage(string[] errMsgs)
        {
            // Get or sets error message after formating
            string formatedMessage;
            try
            {
                // Sets starting of formated list 
                formatedMessage = "<ul>";
                // Iterate through the error messages to add error message to the formated list
                foreach (string errMsg in errMsgs)
                {
                    // Adding error message to the formated list
                    formatedMessage += "<li>" + errMsg + "</li>";
                }
                // Sets ending of formated list 
                formatedMessage += "</ul>";

                // Returns formted error message
                return formatedMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// HTML Decode a single column of string field in a Data Table
        /// </summary>
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
        /// 
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public static dynamic InitiateClient(dynamic Client)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CommonFunctions.IgnoreCertificateErrorHandler);
            Client.ClientCredentials.UserName.UserName = "gti";
            Client.ClientCredentials.UserName.Password = "123456";
            return Client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string ProcessException(Exception exception)
        {
            // Gets or sets exception mode;stored in web.config;Admin/User
            string exceptionMode = ConfigurationManager.AppSettings["ExceptionMode"];
            // Gets or sets exception details
            string[] exceptionDetails = exception.Message.Split(UtilityResources.ExceptionToken.ToCharArray());
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
            if (exceptionDetails[count - 1].Equals(UtilityResources.GTIException))
            {
                // Gets or sets exception key;key is stored as count-4 to count-3 position in exception message
                string exceptionKey = exceptionDetails[count - 4] + UtilityResources.ExceptionToken + exceptionDetails[count - 3];
                // Checking exception mode is admin
                if (exceptionMode.Equals(UtilityResources.Admin))
                {
                    // Gets exception message from exception details;replacing GTI_EXCEPTION_DETAILS token from message
                    errorMessage = exceptionDetails[count - 2].Replace(UtilityResources.GTIExceptionDetails, String.Empty);
                    // Checking no exception details found
                    if (errorMessage.Equals(String.Empty))
                    {
                        // Sets exception message to unknown
                        errorMessage = UtilityResources.UnHandledException;
                    }
                    // Return exception message to client;by encoding it;shows using jquery message
                    return errorMessage.Replace(UtilityResources.SingleQuoteCharacter, UtilityResources.SpaceCharacter).Replace(UtilityResources.DoubleQuoteCharacter, UtilityResources.SpaceCharacter).Replace(UtilityResources.EnterCharacter, UtilityResources.BreakTag).Replace(UtilityResources.CommaCharacter, UtilityResources.BreakTag).Replace(UtilityResources.EditEditConcurrencyException, UtilityResources.Msg_Save_Error_Concurrent);
                }
                else
                {
                    // Gets error list hash table;stored in applicaton
                    Hashtable errorList = (Hashtable)HttpContext.Current.Application[UtilityResources.ErrorTable];
                    // Gets  page list hash table;stored in applicaton
                    Hashtable pageList = (Hashtable)HttpContext.Current.Application[UtilityResources.PageTable];
                    // Gets field list hash table;stored in applicaton
                    Hashtable fieldList = (Hashtable)HttpContext.Current.Application[UtilityResources.FieldTable];
                    // Gets pageName from application using exception source as key in error hash table
                    pageName = (pageList == null || pageList[exceptionSource] == null) ? String.Empty : pageList[exceptionSource].ToString();
                    // Gets fieldName from application using exception source as key in field hash table
                    fieldName = (fieldList == null || fieldList[exceptionSource] == null) ? "Code" : fieldList[exceptionSource].ToString();
                    // Constucting error message by appending  page name + field name + error message
                    errorMessage = String.Format((errorList == null || errorList[exceptionKey] == null) ? String.Empty : errorList[exceptionKey].ToString(),
                                pageName, fieldName);
                    // Checking no error message is empty
                    if (errorMessage.Equals(String.Empty))
                    {
                        // Sets error message as unhandled exception
                        errorMessage = UtilityResources.UnHandledException;
                    }
                    // Return exception message to client;by encoding it;shows using jquery message
                    return errorMessage.Replace(UtilityResources.EnterCharacter, UtilityResources.BreakTag).Replace(UtilityResources.CommaCharacter, UtilityResources.BreakTag);
                }
            }
            else
            {
                // Return client exception message to client;by encoding it;shows using jquery message
                return exception.Message.Replace(UtilityResources.SingleQuoteCharacter, UtilityResources.SpaceCharacter).Replace(UtilityResources.DoubleQuoteCharacter, UtilityResources.SpaceCharacter).Replace(UtilityResources.EnterCharacter, UtilityResources.BreakTag).Replace(UtilityResources.CommaCharacter, UtilityResources.BreakTag);
            }
        }

        public static string GetShortString(object evelOrginalString, int limit)
        {
            string orginalString = HttpUtility.HtmlDecode(Convert.ToString(evelOrginalString));
            return (orginalString.Length <= limit) ? orginalString : (orginalString.Substring(0, limit) + "..");
        }
        /// <summary>
        /// Check the IP is Local Or Public
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="PrivateIPRanges">',' Seperate bit block range '-' Seperate Start and End</param>
        /// <returns></returns>
        public static bool IsLocalHost(string IP, string PrivateIPRanges = "10.0.0.1-10.255.255.254,172.16.0.1-172.31.255.254,192.168.0.1-192.168.255.254")
        {
            //IANA-reserved private IPv4 network ranges
            //                                    Start           End                No. of addresses 
            //24-bit block (/8 prefix, 1 × A)     10.0.0.0        10.255.255.255    16777216 
            //20-bit block (/12 prefix, 16 × B)   172.16.0.0      172.31.255.255    1048576 
            //16-bit block (/16 prefix, 256 × C)  192.168.0.0     192.168.255.255   65536 

            bool isLocal = true;
            if (IP.Contains("localhost"))
                isLocal = true;
            else
            {
                string[] IPRange = PrivateIPRanges.Trim().Split(',');
                int index = 0;
                for (index = 0; index < IPRange.Length; index++)
                    if (((string[])IP.Split('.'))[0] == ((string[])IPRange[index].Split('.'))[0]) break;
                if (index > (IPRange.Length-1))
                {
                    isLocal = false;
                }
                else
                {
                    string[] lowerRange = ((string[])IPRange[index].Split('-'))[0].Split('.');
                    string[] upperRange = ((string[])IPRange[index].Split('-'))[1].Split('.');
                    int lowerVal = 0;
                    int upperVal = 0;
                    int currentVal = 0;
                    for (int i = 1; i < upperRange.Length; i++)
                    {
                        currentVal = Convert.ToInt32(((string[])IP.Split('.'))[i]);
                        lowerVal = Convert.ToInt32(lowerRange[i]);
                        upperVal = Convert.ToInt32(upperRange[i]);
                        if (currentVal < lowerVal || currentVal > upperVal)
                        {
                            isLocal = false;
                            break;
                        }
                    }
                }
            }
            return isLocal;

        }
        #endregion 
    }


}

