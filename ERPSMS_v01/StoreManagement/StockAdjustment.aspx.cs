using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using BusinessObject.CommonManagement;
using BusinessLogic.CommonManagement;
using BusinessObject.Common;
using System.Threading;

namespace ERPSMS_v01.StoreManagement
{
    public partial class StockAdjustment :ERP.Store.UI.MyBasePage
    {
        int AppPk = 0;
        BusinessObject.User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (!IsPostBack)
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillInitialData();
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
            }
        }

        /// <summary>
        /// Funtion used fill purchase request details
        /// </summary>
        /// <param name="pk"></param>
        private void FillStoreAudit(int pk)
        {
            BusinessObject.StoreManagement.StoreAudit storeAudit = null;
            if (pk != 0)
            {
                ItemList.Value = BusinessLogic.StoreManagement.StockAdjustment.GetStockAdjustmentDetails(pk);
            }
            else
            {
                storeAudit = new BusinessObject.StoreManagement.StoreAudit();
                storeAudit.ItemList = new List<BusinessObject.StoreManagement.StoreMaterialDtls>();
                ItemList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(storeAudit);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillStoreAuditNo(objUser);
                SAHTXT_DATE.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                SAH_DATE.Value = System.DateTime.Now.ToString("dd-MMM-yyyy");

            }
        }

        /// <summary>
        /// Funtion used fill Fill Initial Data
        /// </summary>
        private void FillInitialData()
        {
           
            //======================================== ########################========================================================

            int refId = 0;
            int appId = 0;
            if (Request.QueryString["PK"] != null)
            {
                FillStoreAudit(Convert.ToInt32(Request.QueryString["PK"].ToString()));
                FillProcessId();
            }
             else if (Request.QueryString["PRefID"] != null)
            {
                ucrWrkf.RefID = int.Parse(Request.QueryString["PRefID"]);
                refId = int.Parse(Request.QueryString["PRefID"]);
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        appId = Convert.ToInt32((dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? 0 : dtApplication.Rows[0]["refApplication"]);
                    }
                }
                PRefID.Value = Request.QueryString["PRefID"].ToString();
                FillStoreAudit(appId);
                FillProcessId();
            }
            
            else if (Request.QueryString["RefID"] != null)
            {
                ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                refId = int.Parse(Request.QueryString["RefID"]);
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                appId = BusinessLogic.StoreManagement.StockAdjustment.GetEvalAppIDForRefID(refId);
                ucrWrkf.FillWorkFlowDetails(false);
                hdfProcessID.Value = GetStoreAdjusProcessId().ToString();
                FillStoreAudit(appId);
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillStoreAudit(0);
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

                else if (Request.QueryString["Status"] == "2")
                {
                    int procId = GetStoreAdjusProcessId();
                    WorkFlowDetails wrkFlowDtls = CommonBL.GetWorkflowDetails(int.Parse(Request.QueryString["PK"].ToString()), procId);
                    if (wrkFlowDtls != null)
                    {
                        ucrWrkf.RefID = wrkFlowDtls.ReferenceID;
                        ucrWrkf.ViewType = Request.QueryString["Status"].ToString() == "2" ? 0 : 0;

                    }
                    else
                    {
                        ucrWrkf.RefID = 0;
                        ucrWrkf.ViewType = 0;
                    }
                    ucrWrkf.FillWorkFlowDetails(true);
                }
                else
                {
                    ucrWrkf.Visible = false;
                }
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
                ucrWrkf.Visible = true;
            }
           
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        private void FillStoreAuditNo(BusinessObject.User objUser)
        {
            string sANo;
            DataTable dtSrsNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "SANO", 0);
            if (dtSrsNoFormat.Rows.Count > 0)
                sANo = dtSrsNoFormat.Rows[0]["DFT_VALUE"].ToString();
            else
                sANo = GTIService.Constants.Common.CommonConstant.SANUMBERFORMAT;
            MatchCollection matchcol = Regex.Matches(sANo, @"\#[\w]+\#");
            for (int i = 0; i < matchcol.Count; i++)
            {
                if (i == 0)
                {
                    sANo = sANo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#", string.Empty)));
                }
                else if (i == 1)
                {
                    sANo = sANo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.StoreAudit.GetStoreAuditNO());
                }

            }
            SAHTXT_NO.Text = sANo;
            SAH_NO.Value = sANo;
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
            int appId = 0;
            int result = 0;
            if (Convert.ToInt32(hdfAppID.Value == string.Empty ? "0" : hdfAppID.Value) > 0)
            {
                appId = Convert.ToInt32(Convert.ToInt32(hdfAppID.Value));
                ucrWrkf.ApplicationID = appId;
                refId = ucrWrkf.DoWorkFlow();
            }
            if (refId > 0 && result > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "wrkfMsg", "<script type='text/javascript'> $(document).ready(function () { ShowWorkflowSaveMsg(); });</script>", false);
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
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int GetStoreAdjusProcessId()
        {
            int procId = 0;
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
               
            }
            return procId;

        }
      
    }
}