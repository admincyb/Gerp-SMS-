using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using MailSendCore;
using System.Collections;
using System.IO;
using BusinessObject;
using SMSMailSendManager;
using NotificationManager;

namespace LatexERPV2
{
    public class Global : System.Web.HttpApplication
    {
        private static MailQueService MailQ;
        private static SMSQueService SMSQ;
        private static PushNotificationService NotificationQueue;
        protected void Application_Start(object sender, EventArgs e)
        {
            Telerik.Reporting.Services.WebApi.ReportsControllerConfiguration.RegisterRoutes(System.Web.Http.GlobalConfiguration.Configuration);
            // Holds list of errors
            Hashtable errorList = new Hashtable();
            // Holds list of pages in the application
            Hashtable pageList = new Hashtable();
            // Holds list of special fields (used for showing in messages)
            Hashtable fieldList = new Hashtable();

            #region FieldList
            fieldList.Add("AsrAssetSovDtlManager", "Parameter");
            fieldList.Add("StdOperatingValuesService", "Parameter");

            fieldList.Add("AdmUomConvTrxManager", " ");
            fieldList.Add("UomConvertionService", " ");

            fieldList.Add("AdmCurrencyRateTrxManager", " ");
            fieldList.Add("CurrencyExchangeRateService", " ");

            fieldList.Add("VndVendorModelDtlManager", "Model");
            fieldList.Add("VendorModelService", "Model");

            fieldList.Add("AdmAttachmentDtlManager", "Title");
            fieldList.Add("AttachmentsService", "Title");

            fieldList.Add("AdmAlertDtlManager", "Name");
            fieldList.Add("AlertsService", "Name");
            #endregion

            #region Page Names
            pageList.Add("AsrAssetActivityResourceDtlManager", Resources.PageNameRes.AssetActivityResource);

            pageList.Add("AccountMstService", Resources.PageNameRes.Accounts);
            pageList.Add("FinCoaMstManager", Resources.PageNameRes.Accounts);

            pageList.Add("POInvoiceService", Resources.PageNameRes.Invoice);
            pageList.Add("FinInvoiceVndHdrManager", Resources.PageNameRes.Invoice);

            #endregion

            #region Error List
            errorList.Add("SqlException|2627", Resources.ErrorMessages.Msg_Save_Error_Code_Already_Exists);
            errorList.Add("SqlException|547", Resources.ErrorMessages.Msg_Delete_Error_Ref);
            errorList.Add("SqlException|2601", Resources.ErrorMessages.Msg_Save_Error_Code_Already_Exists);
            errorList.Add("OptimisticConcurrencyException|000", Resources.ErrorMessages.Msg_Error_Concurrent);
            errorList.Add("RecordAlreadyExistsException|000", Resources.ErrorMessages.Msg_Save_Error_Code_Already_Exists);
            errorList.Add("DateRangeException|000", Resources.ErrorMessages.Msg_Save_Error_Date_Range_Exists);
            errorList.Add("OutofStockException|000", Resources.ErrorMessages.Msg_Error_OutofStock);
            #endregion

            #region Hash Tables
            Application[Resources.ErpRes.ErrorTable] = errorList;
            Application[Resources.ErpRes.PageTable] = pageList;
            Application[Resources.ErpRes.FieldTable] = fieldList;
            #endregion

            // start mailq service on application startup
            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableMailQService"]))
            {
                MailQ = new MailQueService();
                MailQ.StartService();
            }

            //start sms service on application startup
            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableSMSQService"]))
            {
                if (SMSQ == null || !SMSQ.IsStarted)
                {
                    if (SMSQ == null)
                    {
                        SMSQ = new SMSQueService();
                    }
                    SMSQ.StartService();
                }
            }
            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableNotificationService"]))
            {
                if (NotificationQueue == null || !NotificationQueue.IsStarted)
                {
                    if (NotificationQueue == null)
                    {
                        NotificationQueue = new PushNotificationService();
                    }
                    NotificationQueue.StartService();
                }
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //start service if it is not not started or stopped by some issue

            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableMailQService"]))
            {
                if (MailQ == null || !MailQ.IsStarted)
                {
                    if (MailQ == null)
                    {
                        MailQ = new MailQueService();
                    }
                    MailQ.StartService();
                }
            }

            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableSMSQService"]))
            {
                if (SMSQ == null || !SMSQ.IsStarted)
                {
                    if (SMSQ == null)
                    {
                        SMSQ = new SMSQueService();
                    }
                    SMSQ.StartService(); 
                }
            }
            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableNotificationService"]))
            {
                if (NotificationQueue == null || !NotificationQueue.IsStarted)
                {
                    if (NotificationQueue == null)
                    {
                        NotificationQueue = new PushNotificationService();
                    }
                    NotificationQueue.StartService();
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            #region RDLC Line Control Issue Solved
            //Biju
            //The following code is a hack for stopping a broken image from magically appearing on SSRS reports in chrome
            //where ever a line is used in the report.
            Uri u = HttpContext.Current.Request.Url;

            //If the request is from a Chrome or Firefox browser 
            //AND a report is being generated 
            //AND there is no QSP entry named "IterationId"
            string browserName = HttpContext.Current.Request.Browser.Browser.ToLower();

            if ((browserName.Contains("chrome") || browserName.Contains("firefox")) &&
                    u.AbsolutePath.ToLower().Contains("reserved.reportviewerwebcontrol.axd") &&
                    !u.Query.ToLower().Contains("iterationid"))
                HttpContext.Current.RewritePath(u.PathAndQuery + "&IterationId=0");
            #endregion
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.Name != string.Empty)
            {
                if (HttpContext.Current.User.Identity.AuthenticationType != "Forms")
                    throw new Exception("Only forms authentication is supported, not " + HttpContext.Current.User.Identity.AuthenticationType);
                FormsIdentity UserIdentity = (FormsIdentity)HttpContext.Current.User.Identity;
                //Do we have some roles to retrieve?  If so, replace the user object
                if (UserIdentity.Name != null)
                {
                    BusinessObject.User currUser = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.User>(UserIdentity.Ticket.UserData);
                    List<UserGroup> groupList = BusinessLogic.Login.GetUserGroup(currUser.PKUser);
                    currUser.GroupList = groupList;
                    string strRoles = string.Empty;
                    if (groupList != null && groupList.Count > 0)
                    {
                        foreach (var r in groupList)
                            strRoles += r.GroupPK + ",";
                        strRoles = strRoles.Substring(0, strRoles.Length - 1);
                        currUser.Roles = strRoles;
                    }
                    HttpContext.Current.User = new BusinessObject.ERPPrincipal(currUser);
                }
            }
        }
        /// <summary>
        /// Application Error Log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["EnableErrorLog"]!=null && Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableErrorLog"]))
            {
                try
                {
                    Exception exc = Server.GetLastError();
                    string url = Server.MapPath("~/ErrorLogs/") + "ApplicationErr.txt";
                    FileStream fs1 = new FileStream(url, FileMode.Append, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(fs1);
                    //writer.Write(exc.StackTrace);
                    //writer.Write(Environment.NewLine);
                    //writer.Write("..........................");
                    //writer.Write(Environment.NewLine);
                    //writer.Write(exc.InnerException);
                    //writer.Close();

                    writer.WriteLine("********** {0} **********", DateTime.Now);
                    if (exc.InnerException != null)
                    {
                        writer.Write("Inner Exception Type: ");
                        writer.WriteLine(exc.InnerException.GetType().ToString());
                        writer.Write("Inner Exception: ");
                        writer.WriteLine(exc.InnerException.Message);
                        writer.Write("Inner Source: ");
                        writer.WriteLine(exc.InnerException.Source);
                        if (exc.InnerException.StackTrace != null)
                        {
                            writer.WriteLine("Inner Stack Trace: ");
                            writer.WriteLine(exc.InnerException.StackTrace);
                        }
                    }
                    writer.Write("Exception Type: ");
                    writer.WriteLine(exc.GetType().ToString());
                    writer.WriteLine("Exception: " + exc.Message); ;
                    // writer.WriteLine("Hash Code: " + exc.GetHashCode());
                    writer.WriteLine("Stack Trace: ");
                    if (exc.StackTrace != null)
                    {
                        writer.WriteLine(exc.StackTrace);
                        writer.WriteLine();
                    }
                    string strExp = exc.ToString();
                    writer.WriteLine("********** {0} **********", "Exception To String");
                    writer.WriteLine(strExp);
                    writer.Close();
                }
                catch (Exception) { }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
            //stop mailq service on application end
            if (MailQ != null)
            {
                MailQ.StopService();
            }

            // stop sms service on application end
            if (SMSQ != null)
            {
                SMSQ.StopService();
            }
        }
    }
}