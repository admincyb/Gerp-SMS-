using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using BusinessObject.Common;
using ERPData;
using ERPService;
using BusinessObject.CommonManagement;
using System.Threading;
using BusinessLogic.CommonManagement;
using BusinessLogic.AccountManagement;
using BusinessObject;

namespace ERPSMS_v01.StoreManagement
{
    public partial class EMRCreate : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User currentUser;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private DataTable dtCompany;
        public string ReceivingStore = string.Empty;
        public string MaterialReceiptNo = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);

            if (!IsPostBack)
            {
                if (Request.QueryString["TYPE"] != null)
                {
                    transactionType.Value = Request.QueryString["TYPE"].ToString();
                }
                SetResourse(transactionType.Value);
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                hdfSbuCurrency.Value = currentUser.BaseCurrency.ToString();
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                FillInitialData();
                ConfigurationSettings();
                // ICH_DEPT.Value = currentUser.CurrentDeptPK.ToString();
                if (Convert.ToInt16(transactionType.Value) == 4)
                {
                    APT_CODE.Value = ApplicationType.OS;
                }
                else
                {
                    APT_CODE.Value = ApplicationType.EMR;
                }
                GetWeightedAverage();
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                hdfCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
        private void FillInitialData()
        {
            int refId = 0;
            int appId = 0;
            int prefID = 0;

            if (Request.QueryString["IssueID"] != null)
            {
                FillRequisitionData(Convert.ToInt32(Request.QueryString["IssueID"].ToString()));
            }
            else if (Request.QueryString["CrDr"] != null)
            {
                FillDatasFromCreditNote(Convert.ToInt32(Request.QueryString["CrDr"].ToString()));
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillRequisitionData(0);
            }
            // Check Querystring have REFID
            else if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE] != null)
            {
                // Assign RefID 
                ucrWrkf.RefID = int.Parse(Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE]);
                refId = int.Parse(Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE]);
                hdfRefID.Value = refId.ToString();
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                // get Application details by RefID
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        // Assign AppID
                        appId = Convert.ToInt32((dtApplication.Rows[0][GTIService.Constants.StockTransfer.Fields.APPID] == DBNull.Value) ? 0 : dtApplication.Rows[0][GTIService.Constants.StockTransfer.Fields.APPID]);
                    }
                }
                // Fill EMR Details By appID
                FillRequisitionData(appId);
                // Fill workflow details
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
                else if (Request.QueryString["Status"] == "2")
                {
                    int procId = GetEMRProcessID();
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
                    btnSave.Visible = true;
                }
                else
                {
                    ucrWrkf.Visible = false;
                }
                if (Request.QueryString["IsModify"] == "1")
                    btnSave.Visible = GetUserRights();
                else
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
                FillProcessId();
            }
            //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt32(hdfDeptID.Value));
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                hdfSelCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
            }
        }
        //}
        private void FillRequisitionData(int materialIssueID)
        {
            if (materialIssueID != 0)
            {
                //Assigning initialized Requisitionobject to hidden field (EvalDetailsList)
                ConsumptionDtl.Value = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetMaterialIssueDetails(materialIssueID);
                AST_DOC_MODE.Value = "0";
            }
            else
            {
                BusinessObject.StoreManagement.ExternalMaterialIssue MaterialIssueObject = new BusinessObject.StoreManagement.ExternalMaterialIssue();
                MaterialIssueObject.MaterialIssueDetailsList = new List<BusinessObject.StoreManagement.ExternalMaterialIssueDetails>();
                //Assigning initialized Requisitionobject to hidden field (EvalDetailsList)
                ConsumptionDtl.Value = Newtonsoft.Json.JsonConvert.SerializeObject(MaterialIssueObject);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillSRSNumber(objUser);
                AST_DOC_MODE.Value = GetDOCMODE();
            }
        }

        private int GetEMRProcessID()
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by page and user dept,
            //DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            DataTable dtProcess;
            if (Session[SessionStrings.CurDept] != null)
                dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[SessionStrings.CurDept]));
            else
                dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);

            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                // pass proc Id to wrkflw user control and fill action details 
                return int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());

            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Get User Rights for Modification
        /// </summary>
        /// <returns></returns>
        private bool GetUserRights()
        {
            bool Flag = false;
            //string path = "/StoreManagement/ExternalMaterialReceive.aspx";
            string path = GetLocalResourceObject("PageURL").ToString();// +"?TYPE=" + transactionType.Value;
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.SBUID, currentUser.CurrentDeptPK);
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                   
                    if (item.ActionName == "MODIFY" && item.HasActionRight == true && item.UserDeptRight == true)
                    {
                        Flag = true;
                    }
                }
            }
            return Flag;

        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.EMR, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        ///Methord used to Fill SRSno Related data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="User object"></param>
        private void FillSRSNumber(BusinessObject.User objUser)
        {
            //string srsNo;
            //DataTable dtSrsNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "ERCNO", 0);
            //if (dtSrsNoFormat.Rows.Count > 0)
            //    srsNo = dtSrsNoFormat.Rows[0]["DFT_VALUE"].ToString();
            //else
            //    srsNo = GTIService.Constants.Common.CommonConstant.SRSNUMBERFORMAT;
            //MatchCollection matchcol = Regex.Matches(srsNo, @"\#[\w]+\#");
            //for (int i = 0; i < matchcol.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        srsNo = srsNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#", string.Empty)));
            //    }
            //    else if (i == 1)
            //    {
            //        srsNo = srsNo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetICHNo(currentUser.SBUID, 2));
            //    }

            //}
            ICH_NO.Value = "";
            lblMaterilaConsumptionNo.Text = Resources.Messages.DocGenerationNew;

        }
        private void FillDatasFromCreditNote(int crdrPK)
        {
            if (crdrPK != 0)
            {
                ConsumptionDtl.Value = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetDetailsFromCRDR(crdrPK);
                AST_DOC_MODE.Value = GetDOCMODE();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("SRSStockValidation", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdnIsNeededStockValidation.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            //For some clients  the category Semi Finished Goods should not be listed in category ddl
            hdnShowSFGCategory.Value = (GetGlobalResourceObject("ConfigurationsRes", "ShowSFGCategory")).ToString();
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            DataTable dtAuto = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("AUTO COMPLETE SETTINGS", string.Empty, currentUser.SBUID);
            if (dtAuto != null && dtAuto.Rows.Count > 0)
            {
                AutoStartValue.Value = dtAuto.Rows[0]["ACF_VALUE"].ToString();
            }
        }

        private void GetWeightedAverage()
        {
            hdfWeightedAverage.Value = "0";
            DataTable dtWeightedAverage = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetWeightedAverage(1, "INVENTORY SETTINGS", "EnableWA", currentUser.SBUID);
            if (dtWeightedAverage.Rows.Count > 0)
            {
                if (dtWeightedAverage.Rows[0].ItemArray[3].ToString() == "1")
                {
                    hdfWeightedAverage.Value = "1";
                }
            }
        }

        /// <summary>
        /// Set Label based on Type
        /// </summary>
        /// <param name="type"></param>
        private void SetResourse(string type)
        {
            switch (type)
            {
                case "2":
                    ReceivingStore = Resources.Controls.ReceivingStore;
                    MaterialReceiptNo = Resources.Controls.MaterialReceiptNo;
                    break;
                case "4":
                    ReceivingStore = Resources.Controls.Store;
                    MaterialReceiptNo = Resources.Controls.OpeningStockNo;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                base.OnLoadComplete(e);
                AssignLocalBreadCrumb();
            }
        }
        /// <summary>
        /// Assign breadCrumb
        /// </summary>
        public void AssignLocalBreadCrumb()
        {
            try
            {
                //Breadcrumb Material Return
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "4")
                {
                    if (this.GetLocalResourceObject("BreadcrumbOpeningStock") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbOpeningStock").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("OpeningStockTitle").ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #region WorkFlow

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



        #endregion

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessId(bool SetProcessID = false)
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
            if (SetProcessID && dtProcess != null && dtProcess.Rows.Count > 0)
            {
                hdfProcessID.Value = dtProcess.Rows[0]["PROCESS_PK"].ToString();
            }
            else
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