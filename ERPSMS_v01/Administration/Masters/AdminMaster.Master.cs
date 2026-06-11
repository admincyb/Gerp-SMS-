using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using BusinessObject.Common;
using System.Data;
using BusinessLogic.CommonManagement;
using BusinessObject.AccountManagement;

namespace ERPSMS_v01.Administration
{
    public partial class AdminMasterDtls : System.Web.UI.MasterPage
    {
        BusinessObject.User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
               
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
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/jquery-1.5.min.js.axd\" type='text/javascript'></script>");
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/UI/jquery-ui.min.js.axd\" type='text/javascript'></script>");
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/Validate/jquery.validate.js.axd\" type='text/javascript'></script>");
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "GrandGridMulti.js.axd\" type='text/javascript'></script>");
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "roundedbox.js.axd\" type='text/javascript'></script>");
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/json2.js.axd\" type='text/javascript'></script>");
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "Menu/fgmenu.js.axd\" type='text/javascript'></script>");
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "Menu/MenuScript.js.axd\" type='text/javascript'></script>");
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "GrandScriptUtils.js.axd\" type='text/javascript'></script>");
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "GrandTreeMulti.js.axd\" type='text/javascript'></script>");
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "PageScript/UserControlScript/AdvanceSearch.js.axd\" type='text/javascript'></script>");
                sbScript.AppendLine("<script src=\"" + ScriptRoot + "PageScript/MasterPage.js.axd\" type='text/javascript'></script>");
                //Adding script to the page assigning to the hidden field
                this.scriptSrc.Text = sbScript.ToString();
                if (!IsPostBack)
                {
                    if (Request.QueryString["Dep"] != null)
                    {
                        Session[SessionStrings.CurDept] = Convert.ToInt32(Request.QueryString["Dep"].ToString());
                        GetDepartmetBizUnit(Convert.ToInt32(Request.QueryString["Dep"].ToString()));
                    }

                    BusinessObject.User myUser = (BusinessObject.User)Context.User.Identity;
                    UserPk.Value = myUser.PKUser.ToString();
                    BizUnitPk.Value = myUser.SBUID.ToString();
                    lblUserName.Text = "Welcome " + myUser.EmpName;
                    FillMasterSBU(myUser.PKUser);
                }

        }
        private void GetDepartmetBizUnit(int dept)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtDept;
            dtDept = CommonBL.GetDepartmentDetails(dept);
            if (dtDept != null && dtDept.Rows.Count > 0)
                currentUser.SBUID = Convert.ToInt32(dtDept.Rows[0]["DPT_BIZUNIT"]);
            BusinessObject.SBU sbu = new BusinessObject.SBU();
            sbu.CurrentSBUPK = currentUser.SBUID;
            BusinessLogic.AccountManagement.UserAuthBL userAuth = new BusinessLogic.AccountManagement.UserAuthBL();
            userAuth.SetUserProperty(UserPropertyEnum.CurrentSBU, sbu);
        }
        /// <summary>
        /// Fill User Sbu's
        /// </summary>
        /// <param name="userPK"></param>
        private void FillMasterSBU(int userPK)
        {
            SBU.DataSource = BusinessLogic.Administration.Configurations.SBUConfiguartion.GetMasterSBU(userPK);
            SBU.DataTextField = GTIService.Constants.Configurations.SBUConfig.Fields.SBUNAME;
            SBU.DataValueField = GTIService.Constants.Configurations.SBUConfig.Fields.SBUPK;
            SBU.DataBind();
        }

        /// <summary>
        /// Fill User Sbu's
        /// </summary>
        /// <param name="userPK"></param>
        protected void SBU_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lnbLogout_Click(object sender, EventArgs e)
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

    }
}