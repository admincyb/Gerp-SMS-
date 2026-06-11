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
    public partial class IFrameMaster : System.Web.UI.MasterPage
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCompany", "ShowCompany();", true);
                if (System.Web.HttpContext.Current.User != null)
                {
                    BusinessObject.User myUser = (BusinessObject.User)Context.User.Identity;
                    UserPk.Value = myUser.PKUser.ToString();
                }
                else
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    System.Web.HttpContext.Current.User = null;
                }

               
            }
        }
        
        #endregion

    }
}