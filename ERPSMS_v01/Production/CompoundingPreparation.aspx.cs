using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Production
{
    public partial class CompoundingPreparation : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (!IsPostBack)
            {  
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillInitialData();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("CompoundPlanEntry", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                isPlanRequired.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
        }

        /// <summary>
        /// fill details
        /// </summary>
        private void FillInitialData()
        {

            ConfigurationSettings();
            hdfDeptID.Value = currentUser.CurrentDeptPK.ToString();
           

            int refId = 0;
            int appId = 0;
            if (Request.QueryString["PK"] != null)
            {
                FillCompoundDetails(Convert.ToInt32(Request.QueryString["PK"].ToString()));
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillCompoundDetails(0);
            }
            else if (Request.QueryString["RefID"] != null)
            {
                ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                refId = int.Parse(Request.QueryString["RefID"]);
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        appId = Convert.ToInt32((dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? 0 : dtApplication.Rows[0]["refApplication"]);
                    }
                }
                FillCompoundDetails(appId);
                ucrWrkf.FillWorkFlowDetails(true);
                btnSave.Visible = false;
            }
            if (Request.QueryString["Status"] != null)
            {
                ucrWrkf.ViewType = 0;
                if (Request.QueryString["RefID"] != null)
                {
                    ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                    ucrWrkf.FillWorkFlowDetails(true);
                    Button btnSubmit = (Button)ucrWrkf.FindControl("btnSubmitWrkf");
                    btnSubmit.Visible = false;
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
                    btnSave.Visible = false;
                    ucrWrkf.ViewType = 0;
                }
                else
                {
                    ucrWrkf.ViewType = 1; 
                }
              
                ucrWrkf.Visible = true;
                FillProcessId();
            }
            if (btnSave.Visible == true)
            {
                btnSaveSubmit.Visible = true;
                btnSubmit.Visible = false;
            }
            else
            {
                btnSaveSubmit.Visible = false;
                btnSubmit.Visible = true;
            }

        }

        /// <summary>
        ///Methord used to Fill Po Related data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="User object"></param>
        private void FillCompoundNo(BusinessObject.User objUser)
        {
            DataTable dtponoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "COMPOUND BATCH", 0);
            string compBatchNo;
            if (dtponoFormat.Rows.Count > 0)
                compBatchNo = dtponoFormat.Rows[0]["DFT_VALUE"].ToString();
            else
                compBatchNo = GTIService.Constants.Common.CommonConstant.PONUMBERFORMAT;

            MatchCollection matchcol = Regex.Matches(compBatchNo, @"\#[\w]+\#");
            for (int i = 0; i < matchcol.Count; i++)
            {
                if (i == 0)
                {
                    compBatchNo = compBatchNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#", string.Empty)));
                }
                else if (i == 1)
                {
                    compBatchNo = compBatchNo.Replace(matchcol[i].ToString(), BusinessLogic.Production.CompoundPreparationBL.GetCompoundBatchNo());
                }

            }
            Batch_No.Text = compBatchNo;
            CTH_BATCH_NO.Value = compBatchNo;

        }



        /// <summary>
        /// Funtion used fill purchase request details
        /// </summary>
        /// <param name="pk"></param>
        private void FillCompoundDetails(int pk)
        {
          
            if (pk != 0)
            {
                CompoundDetail.Value = BusinessLogic.Production.CompoundPreparationBL.GetCompoundPreparationDetails(pk);
                //CompoundDetailsList.Value = BusinessLogic.Production.CompoundPreparationBL.GetCompoundPreparationDetails(pk);
                hdfAppID.Value = pk.ToString();
            }
            else
            {
                var user = (BusinessObject.User)(HttpContext.Current.User.Identity);
                BusinessObject.Production.CompoundPreparation compound = new BusinessObject.Production.CompoundPreparation();
                compound.Materials = new List<BusinessObject.Production.Materials>();
                CompoundDetailsList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(compound);
                lblPreparedBy.Text = user.EmpName;
                FillCompoundNo(user);
            }
        }
        /// <summary>
        /// Method to Fire event when click submit button in a Workflow, user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WrkfSubmit(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Save Details From Work Flow
                Save();
            }
        }

        /// <summary>
        /// Method to do Action For Work Flow
        /// </summary>
        private void Save()
        {
            int refId = 0;
            if (Convert.ToInt32(hdfAppID.Value == string.Empty ? "0" : hdfAppID.Value) > 0)
            {
                ucrWrkf.ApplicationID = Convert.ToInt32(Convert.ToInt32(hdfAppID.Value));
                refId = ucrWrkf.DoWorkFlow();
            }
            if (refId > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "wrkfMsg", "<script type='text/javascript'>$(document).ready(function () { ShowWorkflowSaveMsg(); });</script>", false);
            }
            else
            {

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
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
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
    }
   
}