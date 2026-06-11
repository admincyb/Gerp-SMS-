using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessObject.CommonManagement;
using GTIService;
using System.IO;

namespace ERPSMS_v01
{
    public partial class ERPSMS_2 : System.Web.UI.MasterPage
    {
        #region Declaration
        string sContent;
        private string absoluteAppPath;
        #endregion

        #region Events
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            //HttpCookie CroseTabAttrib = Request.Cookies["CroseTabAttrib"];
            //HttpCookie AuthValue = Request.Cookies["authValue"];
            //hdfAuthValue.Value = AuthValue.Value;
            //if (CroseTabAttrib != null)
            //{
            //    Response.Cookies["CroseTabAttrib"].Expires = DateTime.Now.AddDays(-1);
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "uniqueIdSessionStorage", "$(document).ready(function(){if(typeof(Storage) !== 'undefined') { sessionStorage.setItem('uniqueIdSessionStorage'," + AuthValue.Value + " );}});", true);
            //}

            if (!Page.IsPostBack)
            {
                BusinessObject.User currentuser = new BusinessObject.User();
                if (HttpContext.Current.User.Identity != null)
                {
                    currentuser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    hdfCurrentDepartment.Value = currentuser.CurrentDeptPK.ToString();
                }
               
            }
        }
        /// <summary>
        /// Event page load
        /// </summary>
        /// <returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            string externalPort = string.Empty;
            string ScriptRoot = string.Empty;
            string RequestURL = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            {
                absoluteAppPath = Request.Url.GetLeftPart(UriPartial.Authority) + System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower();
            }
            else
            {
                absoluteAppPath = Request.Url.GetLeftPart(UriPartial.Authority);
            }
            AbsolutePath.Value = absoluteAppPath;
            hdfAbsolutePath.Value = System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString();
            BusinessObject.User currentuser = new BusinessObject.User();
            if (HttpContext.Current.User.Identity != null)
            {
                currentuser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                hdfIsCompanyShow.Value = this.GetGlobalResourceObject("ConfigurationsRes", "IsCompanyShow").ToString();
                hdfIsCompanyPlantShow.Value = this.GetGlobalResourceObject("ConfigurationsRes", "ShowCompanyLocation").ToString();
                hdfIsAlertSound.Value = currentuser.IsAlertSound.ToString();
                BizUnitPk.Value = currentuser.SBUID.ToString();
                BusinessObject.UserGroup usrgrp = new BusinessObject.UserGroup();
                usrgrp.GroupPK = 2;
                usrgrp.GroupName = "ADMIN GROUP";
                var Usergrp = from grps in currentuser.GroupList
                              where ((grps.GroupPK == 2) || (grps.GroupPK == 1))
                              select new
                              {
                                  GroupName = grps.GroupName,
                                  GroupID = grps.GroupPK
                              };
            }
            string pageURL = "";
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            {
                pageURL = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            }
            else
            {
                pageURL = Request.Url.AbsolutePath.ToLower();
            }
            int pageId = Convert.ToInt32(BusinessLogic.CommonManagement.CommonBL.GetPageId(pageURL));

            // BindRelatedPages(currentuser.PKUser, pageId);
            ((HiddenField)this.FindControl("hdnPageID")).Value = pageId.ToString();
            int HostIndex = 0;
            if (HttpContext.Current.Request.ApplicationPath == "/")
            {
                HostIndex = 9;
            }
            else
            {
                HostIndex = HttpContext.Current.Request.Url.ToString().ToUpper().IndexOf(HttpContext.Current.Request.ApplicationPath.ToUpper()) + 1;
            }

            if (System.Configuration.ConfigurationManager.AppSettings["EXTERNALPORT"] != null)
            {
                externalPort = System.Configuration.ConfigurationManager.AppSettings["EXTERNALPORT"].ToString();
                RequestURL = HttpContext.Current.Request.Url.ToString();
                RequestURL = RequestURL.Replace(HttpContext.Current.Request.ApplicationPath,
                    (":" + System.Configuration.ConfigurationManager.AppSettings["EXTERNALPORT"].ToString() + HttpContext.Current.Request.ApplicationPath));
                HostIndex = RequestURL.ToUpper().IndexOf(HttpContext.Current.Request.ApplicationPath.ToUpper()) + 1;
                ScriptRoot = RequestURL.Substring(0, RequestURL.IndexOf("/", HostIndex)) + "/Scripts/";
            }
            else
                ScriptRoot = HttpContext.Current.Request.Url.ToString().Substring(0, HttpContext.Current.Request.Url.ToString().IndexOf("/", HostIndex)) + "/Scripts/";

            System.Text.StringBuilder sbScript = new System.Text.StringBuilder();
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/jquery-1.5.min.js\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/moment.min.js\" type='text/javascript'></script>");

            sbScript.AppendLine("<script src=\"" + ScriptRoot + "PlaySound.js\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/UI/jquery-ui.min.js\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/UI/jquery.ui.datetimepicker.js\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/UI/timepicker.js\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/Validate/jquery.validate.js.axd\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "GrandGridMulti.js.axd\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/json2.js.axd\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "Menu/fgmenu.js.axd\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "Menu/MenuScript.js.axd\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "PageScript/UserControlScript/AdvanceSearch.js.axd\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "GrandScriptUtils.js.axd\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "GrandTreeMulti.js.axd\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "PageScript/MasterPage.js.axd\" type='text/javascript'></script>");
            this.scriptSrc.Text = sbScript.ToString();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCompany", "ShowCompany();", true);
            if (!Page.IsPostBack)
            {
                hdfCurrentDepartment.Value =  currentuser.CurrentDeptPK.ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCompany", "ShowCompany();", true);
                if (System.Web.HttpContext.Current.User != null)
                {
                    imbLogout.Visible = true;
                    BusinessObject.User myUser = (BusinessObject.User)Context.User.Identity;
                    UserPk.Value = myUser.PKUser.ToString();
                    WelcomeNote.InnerText = "User: " + HttpUtility.HtmlDecode(myUser.EmpName);
                }
                else
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    System.Web.HttpContext.Current.User = null;
                }

                SetDecimalForAmtAndQty();
                SetVersionName();
            }
            if (System.Configuration.ConfigurationManager.AppSettings["IsHelpVisible"] != null)
                if (System.Configuration.ConfigurationManager.AppSettings["IsHelpVisible"] == "0")
                {
                    imgBtnHelp.Visible = false;
                }
            if (System.Web.HttpContext.Current.User != null &&
                   ((BusinessObject.User)Context.User.Identity).PKUser == 1 &&
                   GetGlobalResourceObject("ProjectConfigRes", "IsShowRefresh").ToString() == "1")
            {
                imbRefresh.Visible = true;

            }
        }
        #endregion

        #region MenuCreationERPModel
        /// <summary>
        /// Filter table By Filter Text - Common Function
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filterText"></param>
        /// <returns></returns>
        public static DataTable FilterTable(DataTable dt, string filterText)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = filterText;
            DataTable dTable = dv.ToTable();
            return dTable;
        }

        /// <summary>
        /// GetRelated Pages
        /// </summary>
        /// <param name="userPK"></param>
        private void BindRelatedPages(int userPK, int pageID)
        {
            DataTable dtRelatedPages;
            dtRelatedPages = new DataTable();
            dtRelatedPages = BusinessLogic.Administration.Masters.RelatedLinkBL.GetUserRelatedPages(userPK, pageID, absoluteAppPath);
            if (dtRelatedPages != null && dtRelatedPages.Rows.Count > 0)
            {
                DataList dlRelatedPages;
                dlRelatedPages = this.FindControl("dtlstRecent") as DataList;
                if (dlRelatedPages != null)
                {
                    dlRelatedPages.DataSource = dtRelatedPages;
                    dlRelatedPages.DataBind();
                }
            }
        }
        #endregion

        public void imbRefresh_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            /* Delete all temp pdf in report folder */
            string tempPath = Server.MapPath("~/") + Resources.PageURL.OfflineTestDocs;
            string[] filePaths;
            if (Directory.Exists(tempPath))
            {
                filePaths = Directory.GetFiles(tempPath);
                foreach (string filePath in filePaths)
                    DeleteFile(filePath);
            }
            /* Delete all temp files in upload folder */
            tempPath = Server.MapPath("~/") + Resources.PageURL.TempUpload;
            if (Directory.Exists(tempPath))
            {
                filePaths = Directory.GetFiles(tempPath);
                foreach (string filePath in filePaths)
                    DeleteFile(filePath);
            }
            /* Delete all pdf in report folder */
            string PDFPath = Server.MapPath("~/") + Resources.PageURL.ExteralPdfURL;
            if (Directory.Exists(PDFPath))
            {
                string[] PDFfilePaths = Directory.GetFiles(PDFPath);
                foreach (string filePath in PDFfilePaths)
                    DeleteFile(filePath);
            }
        }
        private void DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        protected void Home_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dtUserData;
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            dtUserData = new DataTable();
            dtUserData = BusinessLogic.CommonManagement.CommonManagement.GetUserCustomer(currentUser.PKUser, currentUser.SBUID);
            if (dtUserData.Rows.Count > 0)
            {
                Response.Redirect("~/Dashboard/OrderTracker.aspx");
            }
            //else if (Session[ERP.Utilities.SessionStrings.IsDashBoardUser] != null && (bool)Session[ERP.Utilities.SessionStrings.IsDashBoardUser] == true)
            else if (BusinessLogic.CommonManagement.CommonBL.IsDashBoardUser(currentUser.PKUser, currentUser.SBUID, Convert.ToInt32(VisbleStatusEnum.TRUE)))
            {
                //DASHBOARDURL
                if (System.Configuration.ConfigurationManager.AppSettings["DASHBOARDURL"] != null)
                {
                    string redirectURL = System.Configuration.ConfigurationManager.AppSettings["DASHBOARDURL"];
                    Response.Redirect(redirectURL);
                }
                else
                    Response.Redirect("~/AccountManagement/WorkflowInbox.aspx");
            }
            else
            {
                Response.Redirect("~/AccountManagement/WorkflowInbox.aspx");
            }
        }

        protected void Inbox_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/AccountManagement/Inbox.aspx");
        }

        protected void WorkflowInbox_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/AccountManagement/WorkflowInbox.aspx");
        }
        /// <summary>
        /// Event for logout button
        /// </summary>
        /// <returns></returns>
        protected void Logout_Click(object sender, ImageClickEventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            //GC.Collect();
            string nextpage = "~/login.aspx";
            Response.Write("<SCRIPT LANGUAGE=javascript>");
            Response.Write("{");
            Response.Write(" var Backlen=history.length;");
            Response.Write(" history.go(-Backlen);");
            Response.Write(" window.location.href='" + nextpage + "'; ");
            Response.Write("}");
            Response.Write("</SCRIPT>");

            #region Old LogOut Code

            //if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
            //{
            //    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1");
            //}
            //else if (System.Configuration.ConfigurationManager.AppSettings["CUSTPORTALURL"] != null)
            //{
            //    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["CUSTPORTALURL"] + "?Logout=1");
            //}
            //else if (System.Configuration.ConfigurationManager.AppSettings["ERPURL"] != null)
            //{
            //    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ERPURL"] + "?Logout=1");
            //}
            //else
            //{
            //    FormsAuthentication.RedirectToLoginPage();
            //} 
            #endregion

            if (System.Configuration.ConfigurationManager.AppSettings["PORTALURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["PORTALURL"] + "?Logout=1");
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1");
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["PRODUCTIONURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["PRODUCTIONURL"] + "?Logout=1");
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["SMSURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["SMSURL"] + "?Logout=1");
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["CONSTRUCTIONURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["CONSTRUCTIONURL"] + "?Logout=1");
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }

        }

        public bool ValidatePageDept(string redirectURL = "../login.aspx")
        {
            bool result = true;

            if (hdfCurrentDepartment.Value != "-1" && hdfCurrentDepartment.Value != ((BusinessObject.User)(HttpContext.Current.User.Identity)).CurrentDeptPK.ToString() )
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
                {
                    redirectURL = System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1";
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetGlobalResourceObject("ErrorMessages", "Msg_Dept_Session_Expired").ToString()) + "','" + GetGlobalResourceObject("Messages", "Information").ToString() + "','" + redirectURL + "');", true);
                result = false;
            }
            return result;
        }

        /// <summary>
        ///  change password button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbtnChangePwd_Click(object sender, ImageClickEventArgs e)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["PROFILEURL"] != null)
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["PROFILEURL"].ToString() + GetGlobalResourceObject("PageURL", "UserProfileURL").ToString(), true);
        }
        private void SetDecimalForAmtAndQty()
        {
            //hdfAmtDecimal.Value = "3";
            //hdfQtyDecimal.Value = "3";
        }
        private void SetVersionName()
        {
            DataTable dtVesion = BusinessLogic.CommonManagement.CommonBL.GetVersionDetails();
            if (dtVesion != null && dtVesion.Rows.Count > 0)
            {
                lblVersion.Text = GetGlobalResourceObject("Captions", "Version").ToString() + " " + dtVesion.Rows[0]["SYS_VERSION"].ToString();
            }

        }


        protected void imgbtnHelp_Click(object sender, ImageClickEventArgs e)
        {
            string FilePath = string.Empty;
            string fileName = this.GetGlobalResourceObject("PageURL", "HelpFileName").ToString();
            if (System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"] != null)
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                {
                    FilePath = "~/Upload/" + fileName;
                    Response.ContentType = "Application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.TransmitFile(Server.MapPath(FilePath));
                    Response.End();
                }
                else
                {
                    FilePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + fileName;
                    Response.ContentType = "Application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            // Response.ContentType = "Application/pdf";
            // Response.AppendHeader("Content-Disposition", "attachment; filename=GERP-Customer Portal Manual.pdf");          
            //// Response.TransmitFile(Server.MapPath("~/Upload/GERP-Customer Portal Manual.pdf"));
            // //Response.TransmitFile(Server.MapPath(FilePath));
            // Response.TransmitFile(FilePath);
            // Response.End();
        }
    }
}