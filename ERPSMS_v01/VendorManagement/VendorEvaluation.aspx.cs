using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WorkflowCore;
using BusinessObject.Common;

namespace ERPSMS_v01.VendorManagement
{
    public partial class VendorEvaluation : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;
       
        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (!IsPostBack)
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillInitialData();
                ConfigurationSettings();
            }
        }
        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsGoToInbox.Value = GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString();           
        }
        /// <summary>
        /// Method to load inital data
        /// </summary>
        private void FillInitialData()
        {
            int refId = 0;
            int appId = 0;
            lblissuedby.Text = currentUser.EmpName;
            lblApprovedBy.Text = currentUser.EmpName;
            if (Request.QueryString["EvalID"] != null)
            {
                EvalDetailsList.Value = Request.QueryString["EvalID"].ToString();
                FillProcessId();
            }
            else if (Request.QueryString["RefID"] == null && Request.QueryString["PRefID"] == null)
            {
                BusinessObject.VendorManagement.VendorEvaluationMaster vendorEvaluationObject = new BusinessObject.VendorManagement.VendorEvaluationMaster();
                vendorEvaluationObject.EvalDetailsList = new List<BusinessObject.VendorManagement.VendorEvaluationDetail> { };
                //Assigning initialized VendorEvalobject to hidden field (EvalDetailsList)
                EvalDetailsList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(vendorEvaluationObject);
                FillProcessId();
            }
            else if (Request.QueryString["RefID"] != null)
            {
                ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                refId = int.Parse(Request.QueryString["RefID"]);
                hdfRefID.Value = refId.ToString();
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
               // Get Application Id, for Process Switching Case
                appId =BusinessLogic.VendorManagement.VendorEvaluation.GetEvalAppIDForRefID(refId);
                ucrWrkf.FillWorkFlowDetails(false);
                ucrWrkf.ProcessID = GetvendorEvalProcessId();
                hdfProcessID.Value = GetvendorEvalProcessId().ToString();
                ucrWrkf.ApplicationID = appId;
                EvalDetailsList.Value = appId.ToString();

                btnSave.Visible = false;
            }
            else if (Request.QueryString["PRefID"] != null)
            {
                ucrWrkf.RefID = int.Parse(Request.QueryString["PRefID"]);
                refId = int.Parse(Request.QueryString["PRefID"]);
                hdfRefID.Value = refId.ToString();
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        appId = Convert.ToInt32((dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? 0 : dtApplication.Rows[0]["refApplication"]);
                    }
                }
                VendorPk.Value = appId.ToString();
                DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetTransactionID(Convert.ToInt32(Request.QueryString["PRefID"]));
                if (dt.Rows.Count > 0)
                {
                    ucrWrkf.FillWorkFlowDetails(false);
                    ucrWrkf.ProcessID = GetvendorEvalProcessId();
                    hdfProcessID.Value = GetvendorEvalProcessId().ToString();
                    EvalDetailsList.Value = dt.Rows[0]["appPK"].ToString();
                }
                else
                {
                    SetPrimaryInfo();
                }
                FillProcessId();
                btnSave.Visible = true;
            }

            if (Request.QueryString["ReadOnly"] != null)
            {
                ucrWrkf.ViewType = 0;
                if (Request.QueryString["RefID"] != null )
                {
                    //btnSubmit.Visible = false;
                    ucrWrkf.FillWorkFlowDetails(false);
                    Button btnWkfSubmit = (Button)ucrWrkf.FindControl("btnSubmitWrkf");
                    btnWkfSubmit.Visible = false;
                }
                else
                {
                    ucrWrkf.Visible = false;
                }
                btnSave.Visible = false;
                
            }
            else
            {
                if (Request.QueryString["Flag"] != null)
                {
                    ucrWrkf.ViewType = 0;
                    btnSave.Visible = false;
                }
                else
                {
                    ucrWrkf.ViewType = 1;
                }
                ucrWrkf.Visible = true;
               
            }

            DataTable dtCompany;
            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                hdfSelCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
            }

        }

        private void SetPrimaryInfo()
        {
            BusinessObject.VendorManagement.VendorEvaluationMaster vendorEvaluationObject = new BusinessObject.VendorManagement.VendorEvaluationMaster();
            vendorEvaluationObject.EvalDetailsList = new List<BusinessObject.VendorManagement.VendorEvaluationDetail> { };
            //Assigning initialized VendorEvalobject to hidden field (EvalDetailsList)
            EvalDetailsList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(vendorEvaluationObject);
        }
        //===========================##### Add Code For WorkFolw , Update Code In Fill Initial Data ###### =======================================
        /// <summary>
        /// Method to Fire event when click submit button in a Workflow, user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WrkfSubmit(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Save();
            }
        }

        /// <summary>
        /// Method to do Action For Work Flow
        /// </summary>
        private void Save()
        {
            int refId=0;
            int appId=0;
            int result = 0;
            if (Convert.ToInt32(hdfAppID.Value == string.Empty ? "0" : hdfAppID.Value) > 0)
            {
                appId=Convert.ToInt32(Convert.ToInt32(hdfAppID.Value));
                ucrWrkf.ApplicationID = appId;
                refId = ucrWrkf.DoWorkFlow();
                
            }
            if (refId > 0 && result > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "wrkfMsg", "<script type='text/javascript'>$(document).ready(function () { ShowWorkflowSaveMsg(); });</script>", false);
            }
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessId()
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by page and user dept,
            DataTable dtProcess;
            if (Session[SessionStrings.CurDept] != null)
                dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[SessionStrings.CurDept]));
            else
                dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);

            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                // pass proc Id to wrkflw user control and fill action details 
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcessID.Value = dtProcess.Rows[0]["PROCESS_PK"].ToString();
                ((HiddenField)this.Master.FindControl("hdnPageID")).Value = dtProcess.Rows[0]["PAG_PK"].ToString();
                // Fill workflow details
                ucrWrkf.FillWorkFlowDetails(true);
            }
        }

        private int GetvendorEvalProcessId()
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            int procID = 0;
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by page and user dept,
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                // pass proc Id to wrkflw user control and fill action details 
                procID= int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
               
            }
            return procID;
        }
        //===========================##### END Code For WorkFolw ###### =======================================
    }
}