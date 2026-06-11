using BusinessLogic.CommonManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Dashboard
{
    public partial class PBIDashboard : ERP.Store.UI.WorkFlowBasePage
    {
        public string PBIReportID
        {
            get
            {
                return (this.ViewState["PBIReportID"] == null ? string.Empty : this.ViewState["PBIReportID"].ToString());
            }
            set
            {
                this.ViewState["PBIReportID"] = value;
            }
        }
        string strSRC = "https://app.powerbi.com/reportEmbed?reportId=";
        protected void Page_Load(object sender, EventArgs e)
        {
            GetWorkflowUserRights();
            iframePBI.Src = strSRC + this.PBIReportID;
            //if()
            //  string str = GetReportURL();
        }
        public void GetWorkflowUserRights()
        {
            #region Variables
            string pageURL;
            CommonBL userAuth;
            BusinessObject.User currentUser;
            #endregion
            pageURL = GetPageUrlWithoutParams();

            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                pageURL = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") ;
            else
                pageURL = Request.Url.AbsolutePath.ToLower() ;
            //}
            pageURL += ((string[])Request.Url.Query.Split('&'))[0];
            userAuth = new CommonBL();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //// get the user WorkFlow Rights
            UserRights = userAuth.GetWorkFlowUserRightsInfo(currentUser.PKUser, pageURL, currentUser.CurrentDeptPK, WkfRefID, WkfPageType);
            this.PBIReportID = string.Empty;
            if (UserRights.Rights.Count > 0)
                this.PBIReportID = UserRights.Rights[0].PBIReportID;
        }
    }
}