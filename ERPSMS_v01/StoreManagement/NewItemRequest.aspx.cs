using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

namespace ERPSMS_v01.StoreManagement
{
    public partial class NewItemRequest : ERP.Store.UI.MyBasePage
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
        private void FillInitialData()
        {
            int refId = 0;
            int appId = 0;

            if (Request.QueryString["NewRequestID"] != null)
            {
                FillNewItemRequest(Convert.ToInt32(Request.QueryString["NewRequestID"].ToString()));
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillNewItemRequest(0);
            }
            else if (Request.QueryString["RefID"] != null)
            {
                ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                refId = int.Parse(Request.QueryString["RefID"]);
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                //DataTable dtApplication = wrkfService.GetApplicationID(refId);
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        appId = Convert.ToInt32((dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? 0 : dtApplication.Rows[0]["refApplication"]);
                    }
                }
                FillNewItemRequest(appId);
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
                    ucrWrkf.ViewType = 0;
                }
                else
                {
                    ucrWrkf.ViewType = 1;
                }
                if (Request.QueryString["RefID"] != null)
                {
                    btnSave.Visible = false;
                }
                else
                {
                    btnSave.Visible = true;
                }
                
                ucrWrkf.Visible = true;
                FillProcessId();
            }




        }
        private void FillNewItemRequest(int itemID)
        {
            if (itemID != 0)
            {
                //Assigning initialized Requisitionobject to hidden field (EvalDetailsList)
                NewItemDetailsList.Value = BusinessLogic.StoreManagement.NewItemRequestBL.GetNewItemRequestDetails(itemID,2);
            }
            
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FillNIRNumber(objUser);
        }
        /// <summary>
        ///Methord used to Fill SRSno Related data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="User object"></param>
        private void FillNIRNumber(BusinessObject.User objUser)
        {
            string nirNo;
            DataTable dtSrsNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "NIRNO", 0);
            if (dtSrsNoFormat.Rows.Count > 0)
                nirNo = dtSrsNoFormat.Rows[0]["DFT_VALUE"].ToString();
            else
                nirNo = GTIService.Constants.Common.CommonConstant.NIRNUMBERFORMAT;
            MatchCollection matchcol = Regex.Matches(nirNo, @"\#[\w]+\#");
            for (int i = 0; i < matchcol.Count; i++)
            {
                if (i == 0)
                {
                    nirNo = nirNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#", string.Empty)));
                }
                else if (i == 1)
                {
                    nirNo = nirNo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.NewItemRequestBL.GetNIRNo());
                }

            }
                 
            NIR_NO.Value = nirNo;
            lblNIR.Text = nirNo;

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
                string[] comments = new string[2];
                if (txtItemCode.Text.Trim() != string.Empty && txtPR.Text.Trim() != string.Empty)
                {
                    comments[0] = chkItemCode.Text + txtItemCode.Text.Trim();
                    comments[1] = chkPR.Text + txtPR.Text.Trim();
                    refId = ucrWrkf.DoWorkFlow();
                    ucrWrkf.AddAdditionalComments(comments);
                }
                else
                {
                    refId = ucrWrkf.DoWorkFlow();
                }
             }
            if (refId > 0)
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


        //===========================##### END Code For WorkFolw ###### =======================================
    }
}