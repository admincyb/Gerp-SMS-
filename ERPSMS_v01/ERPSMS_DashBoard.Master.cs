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

namespace ERPSMS_v01
{
    public partial class ERPSMS_DashBoard : System.Web.UI.MasterPage
    {
        #region Declaration
        string sContent;
        private string absoluteAppPath;
        #endregion

        #region Events
        /// <summary>
        /// Event page load
        /// </summary>
        /// <returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
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
            BindRelatedPages(currentuser.PKUser, pageId);
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
            string ScriptRoot = HttpContext.Current.Request.Url.ToString().Substring(0, HttpContext.Current.Request.Url.ToString().IndexOf("/", HostIndex)) + "/Scripts/";
            System.Text.StringBuilder sbScript = new System.Text.StringBuilder();
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/jquery-1.5.min.js\" type='text/javascript'></script>");
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
            if (!Page.IsPostBack)
            {
                if (System.Web.HttpContext.Current.User != null)
                {
                    imbLogout.Visible = true;
                    BusinessObject.User myUser = (BusinessObject.User)Context.User.Identity;
                    UserPk.Value = myUser.PKUser.ToString();
                    WelcomeNote.InnerText = "Welcome " + myUser.EmpName;
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

            DataAccess.DashboardManagement.DashBoardDataService service = new DataAccess.DashboardManagement.DashBoardDataService();
            DataTable dtCustomer = service.ExecuteSPCustomer("SPCRM_CUSTOMER_GET_FROM_USER", ((BusinessObject.User)Context.User.Identity).PKUser);
            if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            {
                //lblUserName.Text = Convert.ToString(dtCustomer.Rows[0]["CUS_NAME"]);
                txtCustomerSearch.Text = HttpUtility.HtmlDecode(dtCustomer.Rows[0]["CUS_NAME"].ToString());
                hdfCustomerSearch.Value = dtCustomer.Rows[0]["CUS_PK"].ToString();
                txtCustomerSearch.Enabled = false;
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

        protected void Home_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/AccountManagement/WorkflowInbox.aspx");
        }

        protected void Inbox_Click(object sender, ImageClickEventArgs e)
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
            string nextpage = "~/login.aspx";
            Response.Write("<SCRIPT LANGUAGE=javascript>");
            Response.Write("{");
            Response.Write(" var Backlen=history.length;");
            Response.Write(" history.go(-Backlen);");
            Response.Write(" window.location.href='" + nextpage + "'; ");
            Response.Write("}");
            Response.Write("</SCRIPT>");
            //if (System.Configuration.ConfigurationManager.AppSettings["ERP"] == null)
            //{
            //    FormsAuthentication.RedirectToLoginPage();
            //}
            //else
            //{
            //    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ERPURL"]);
            //}
            if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1");
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["CUSTPORTALURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["CUSTPORTALURL"] + "?Logout=1");
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["ERPURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ERPURL"] + "?Logout=1");
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        private void SetDecimalForAmtAndQty()
        {
            hdfAmtDecimal.Value = "3";
            hdfQtyDecimal.Value = "3";
        }
        private void SetVersionName()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["Version"] != null)
            {
                lblVersion.Text = this.GetGlobalResourceObject("Captions", "Version").ToString() + " " + System.Configuration.ConfigurationManager.AppSettings["Version"].ToLower();
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_InitDate", "$(document).ready(function () {InitDate();});", true);
        }

        protected void RefreshDashBoard(object sender, EventArgs e)
        {
            Type contentType = this.Page.GetType();
            System.Reflection.MethodInfo mi = contentType.GetMethod("RefreshAllGrids");
            if (mi != null)
                mi.Invoke(this.Page, null);
        }

        protected void ActionHandler(object sender, EventArgs e) {
            string Action=string.Empty;
            if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
            {
                Action = ((ImageButton)sender).CommandName;
            }
            switch (Action)
            {
                case "Profile":
                    if (Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK] != null)
                        Response.Redirect(Page.ResolveClientUrl("~/OrderToCash/CustomerRegistration.aspx?Tab=CUS"), true);
                    else
                        Response.Redirect(Page.ResolveClientUrl("~/OrderToCash/CustomerRegistration.aspx?Tab=CLST"), true);
                    break;
                case "Enquiries":
                    Response.Redirect(Page.ResolveClientUrl("~/Sales/EnquiryListing.aspx?STATUS=1"), true);
                    break;
                case "Quote":
                    Response.Redirect(Page.ResolveClientUrl("~/Sales/EnquiryListing.aspx?STATUS=2"), true);
                    break;
                case "Contracts":
                    Response.Redirect(Page.ResolveClientUrl("~/Sales/SaleOrderListing.aspx"), true);
                    break;
                case "Inbox":
                    Response.Redirect(Page.ResolveClientUrl("~/AccountManagement/WorkflowInbox.aspx"), true);
                    break;
                case "Orders":
                    Response.Redirect(Page.ResolveClientUrl("~/Sales/DOListing.aspx?STATUS=3"), true);
                    break;
            }
        }
    }
}