using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic.Administration.Configurations;
using ERP.Utilities;
using BusinessObject.CommonManagement;
using System.IO;
using HRMS.Reports;

namespace HRMS
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Logout session
            Session.Abandon();
            System.Web.Security.FormsAuthentication.SignOut();
            #endregion
            if (!IsPostBack)
            {

                /*Request frm Mobile App or from thread for generate Report*/
                if (Request.QueryString["Token"] != null)
                {
                    if (Request.QueryString["ClearPDF"] != null && Request.QueryString["ClearPDF"] == "1")
                        ClearPDF();
                    else
                        GenerateRequestedReport();
                }
                else
                    if (Request.QueryString["PostUser"] == null && Request.QueryString["PostPassword"] == null)//If the request not coming from an integrated solution
                    {
                        //Method call to get active News
                        GetNewsAndEvents();
                    }
            }

        }

        /// <summary>
        /// Method To Get News And Events
        /// </summary>
        private void GetNewsAndEvents()
        {
            DataTable dtNews;
            dtNews = new DataTable();
            dtNews = NewsManagementBL.GetNews(Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO), DbActiveStatus.ACTIVE);
            if (dtNews != null && dtNews.Rows.Count > 0)
            {
                repeaterLatestNews.DataSource = dtNews;
                repeaterLatestNews.DataBind();
            }
            //Get Version Information 
            DataTable dtVesion = BusinessLogic.CommonManagement.CommonBL.GetVersionDetails();
            if (dtVesion != null && dtVesion.Rows.Count > 0)
            {
                lblReleaseDate.Text = Convert.ToDateTime(dtVesion.Rows[0]["SYS_RELEASE_DATE"].ToString()).ToString(Resources.Constants.DateFormatShort);
                lblVersion.Text = GetGlobalResourceObject("Captions", "Version").ToString() +" "+ dtVesion.Rows[0]["SYS_VERSION"].ToString();

            }
        }
        /// <summary>
        /// Clear PDF
        /// </summary>
        private void ClearPDF()
        {
            try
            {
                if (Request.QueryString["FilePath"] != null)
                    File.Delete(Server.MapPath("~/" + Request.QueryString["FilePath"].ToString()));
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
            }
        }

        /// <summary>
        /// Generate Requested Report
        /// </summary>
        private void GenerateRequestedReport()
        {
            int RecPK = -1;
            string RptType = string.Empty;
            int RptSubType = -1;
            string EmpName = string.Empty;
            int SbuID = -1;
            string RptName = string.Empty;
            int DeptPK = -1;
            string ExternalPDFURL = string.Empty;
            if (Request.QueryString["RecPK"] != null)
                RecPK = Convert.ToInt32(Request.QueryString["RecPK"].ToString());
            if (Request.QueryString["RptType"] != null)
                RptType = Request.QueryString["RptType"].ToString();
            if (Request.QueryString["RptSubType"] != null)
                RptSubType = string.IsNullOrEmpty(Request.QueryString["RptSubType"]) ? 0 : Convert.ToInt32(Request.QueryString["RptSubType"].ToString());
            if (Request.QueryString["EmpName"] != null)
                EmpName = Request.QueryString["EmpName"].ToString();
            if (Request.QueryString["SbuID"] != null)
                SbuID = Convert.ToInt32(Request.QueryString["SbuID"].ToString());
            if (Request.QueryString["RptName"] != null)
                RptName = Request.QueryString["RptName"].ToString();
            if (Request.QueryString["Dept"] != null)
                DeptPK = Convert.ToInt32(Request.QueryString["Dept"].ToString());
            if (Request.QueryString["FilePath"] != null)
                ExternalPDFURL = Request.QueryString["FilePath"].ToString();

            GenerateReport objRpt = new GenerateReport();
            objRpt.RecPK = RecPK;
            objRpt.RptType = RptType;
            objRpt.RptSubType = RptSubType;
            objRpt.EmpName = EmpName;
            objRpt.SbuID = SbuID;
            objRpt.DeptPK = DeptPK;
            objRpt.FromExternal = true;
            objRpt.ExternalPDFName = RptName;
            objRpt.ExternalPDFURL = ExternalPDFURL;
            if (objRpt.GenerateReports())
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            else
                Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;

            //GenerateReport r = new GenerateReport();
            //r.RecPK = 302;
            //r.RptType = "PAYRL";
            //r.EmpName = "Aju";
            //r.RptSubType = 1;
            //r.SbuID = 1;
            //r.DeptPK = 1;
            //r.FromExternal = true;
            //r.ExternalPDFName = "MyDoc.pdf";
            //r.ExternalPDFURL = @"D:\Upload\";
            //if(r.GenerateReports())
            //    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            //else
            //    Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
            
        }
    }
}