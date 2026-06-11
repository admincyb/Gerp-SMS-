using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using System.Text.RegularExpressions; 


namespace ERPSMS_v01.Production
{
    public partial class DispersionPreparation :  ERP.Store.UI.MyBasePage
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
        /// Funtion used fill purchase request details
        /// </summary>
        /// <param name="pk"></param>
        private void FillPurchase(int pk)
        {

            BusinessObject.Production.DispersionPreparation dispersionPreparation = null;
            if (pk != 0)
            {
                MaterialList.Value = BusinessLogic.Production.DispersionPreparation.GetDispersionPreparation(pk);
            }
            else
            {
                dispersionPreparation = new BusinessObject.Production.DispersionPreparation();
                dispersionPreparation.MaterialList = new List<BusinessObject.Production.MaterialListDetails>();
                MaterialList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(dispersionPreparation);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillPurchseRequestNo(objUser);
            }
        }

        /// <summary>
        /// Funtion used fill Fill Initial Data
        /// </summary>
        private void FillInitialData()
        {
            hdfDeptID.Value = currentUser.CurrentDeptPK.ToString(); 

            int refId = 0;
            int appId = 0;
            if (Request.QueryString["PK"] != null)
            {
                FillPurchase(Convert.ToInt32(Request.QueryString["PK"].ToString()));
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillPurchase(0);
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
                FillPurchase(appId);
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
            if (Request.QueryString["Status"] != null && Request.QueryString["Status"] == "2")
            {
                btnCancelSubmit.Visible = true;
                btnSubmit.Visible = false;
            }
            else
            {
                btnCancelSubmit.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        private void FillPurchseRequestNo(BusinessObject.User objUser)
        {
            string purchseRequestNo;
            //DataTable dtSrsNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "DISPNO", 0);
            //if (dtSrsNoFormat.Rows.Count > 0)
            //    purchseRequestNo = dtSrsNoFormat.Rows[0]["DFT_VALUE"].ToString();
            //else
            //    purchseRequestNo = GTIService.Constants.Common.CommonConstant.DISPNUMBERFORMAT;
            //MatchCollection matchcol = Regex.Matches(purchseRequestNo, @"\#[\w]+\#");
            //for (int i = 0; i < matchcol.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        purchseRequestNo = purchseRequestNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#", string.Empty)));
            //    }
            //    else if (i == 1)
            //    {
            //        purchseRequestNo = purchseRequestNo.Replace(matchcol[i].ToString(), BusinessLogic.Production.DispersionPreparation.GetDISPNO());
            //    }

            //}
            DTH_BATCH_NO.Text = "[NEW]";
            DTH_DATE.Text = DateTime.Now.ToString("dd-MMM-yyyy");
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
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "wrkfMsg", "<script type='text/javascript'>ShowWorkflowSaveMsg();</script>", false);
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
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int RetFillProcessId()
        {
            int procId = 0;
            string path = "/Production/DispersionPreparation.aspx";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = Convert.ToInt32(hdfProcessID.Value);
                //hdfProcId.Value = procId.ToString();
            }
            return procId;

        }
    }
}