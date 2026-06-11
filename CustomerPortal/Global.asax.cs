using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using MailSendCore;
using System.Collections;
using BusinessObject;
using System.IO;

namespace LatexERPV2
{
    public class Global : System.Web.HttpApplication
    {
        private static MailQueService MailQ;

        protected void Application_Start(object sender, EventArgs e)
        {
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

            fieldList.Add("CustomerRegistrationService", "Data");
            fieldList.Add("CrmCustomerMstManager", "Data");

            #endregion

            #region Page Names
            pageList.Add("AsrAssetActivityResourceDtlManager", Resources.PageNameRes.AssetActivityResource);

            pageList.Add("AccountMstService", Resources.PageNameRes.Accounts);
            pageList.Add("FinCoaMstManager", Resources.PageNameRes.Accounts);

            pageList.Add("POInvoiceService", Resources.PageNameRes.Invoice);
            pageList.Add("FinInvoiceVndHdrManager", Resources.PageNameRes.Invoice);

            pageList.Add("CustomerRegistrationService", "");
            pageList.Add("CrmCustomerMstManager", "");

            #endregion

            #region Error List
            errorList.Add("SqlException-2627", Resources.ErrorMessages.Msg_Save_Error_Code_Already_Exists);
            errorList.Add("SqlException-547", Resources.ErrorMessages.Msg_Delete_Error_Ref);
            errorList.Add("SqlException-2601", Resources.ErrorMessages.Msg_Save_Error_Code_Already_Exists);
            errorList.Add("SqlException-50000", Resources.ErrorMessages.Msg_Save_Error_Code_Already_Exists);
            errorList.Add("OptimisticConcurrencyException-000", Resources.ErrorMessages.Msg_Error_Concurrent);
            errorList.Add("RecordAlreadyExistsException-000", Resources.ErrorMessages.Msg_Save_Error_Code_Already_Exists);
            errorList.Add("DateRangeException-000", Resources.ErrorMessages.Msg_Save_Error_Date_Range_Exists);
            errorList.Add("OutofStockException-000", Resources.ErrorMessages.Msg_Error_OutofStock);
            #endregion

            #region Hash Tables
            Application[Resources.ErpRes.ErrorTable] = errorList;
            Application[Resources.ErpRes.PageTable] = pageList;
            Application[Resources.ErpRes.FieldTable] = fieldList;
            #endregion

            //start mailq service on application startup
            //if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableMailQService"]))
            //{
            //    MailQ = new MailQueService();
            //    MailQ.StartService();
            //}
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //start service if it is not not started or stopped by some issue

            //if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableMailQService"]))
            //{
            //    if (MailQ == null || !MailQ.IsStarted)
            //    {
            //        if (MailQ == null)
            //        {
            //            MailQ = new MailQueService();
            //        }
            //        MailQ.StartService();
            //    }
            //}
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

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
                    //currUser.GroupList = BusinessLogic.Login.GetUserGroup(currUser.PKUser);
                    List<UserGroup> groupList = BusinessLogic.Login.GetUserGroup(currUser.PKUser);
                    currUser.GroupList = groupList;
                    string strRoles = string.Empty;
                    if (groupList != null)
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

        protected void Application_Error(object sender, EventArgs e)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["EnableErrorLog"] != null && Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableErrorLog"]))
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
            //if (MailQ != null)
            //{
            //    MailQ.StopService();
            //}
        }
    }
}