using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using ERPService;
using ERPData;
using System.Threading;
using BusinessObject.AccountManagement;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GTIService;
using BusinessLogic.AccountManagement;
using BusinessObject;

namespace ERPSMS_v01.PurchaseRequestManagement
{
    public partial class PurchaseRequestCreation : ERP.Store.UI.MyBasePage
    {

        #region Variables
        BusinessObject.User currentUser;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private DataTable dtCompany;
        private ActionsEnum commonActions;
        int ReqDept = 0;
        DataTable DtBudget;


        #endregion

        protected override void OnPreLoad(EventArgs e)
        {
            string pageUrl = Resources.PageURL.PurchaseRequest; // "/PurchaseRequestManagement/PurchaseRequestCreation.aspx"; 
            if (Request.QueryString["Type"] != null)
            {
                pageUrl += "?Type=" + Request.QueryString["Type"].ToString();
                if (Request.QueryString["Sbu"] != null)
                {
                    pageUrl += "&Sbu=" + Request.QueryString["Sbu"].ToString();
                }
            }
            base.currentPageURL = pageUrl;
            base.OnPreLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (!IsPostBack)
            {
                if (!(this.Master as ERPSMS_2).ValidatePageDept())
                    return;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                ConfigurationSettings();
                FillInitialData();
                hdfAppType.Value = ApplicationType.PR;
                hdfAppSubType.Value = string.Empty;
                APT_CODE.Value = ApplicationType.PR;
                BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
                file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
                FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
                TEMPFILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                hdfShowIONo.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowPRIoNumber").ToString();
                hdfShowType.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowPRtype").ToString();
                hdfIsShowDispatchedSCInPR.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowDispatchedSCInPR").ToString();
                hdfMaxOrderQtyEnabled.Value = GetGlobalResourceObject("ConfigurationsRes", "EnableMaxOrderQty").ToString();
                hdfshowbudgetValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowBudgetSummary").ToString();
                if (hdfshowbudgetValidation.Value == "1")
                    BUDGETVALIDATION.Value = "-1";
                GetUserRights();
            }
        }

        #region --- Action Handler----
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region REQ DEPT CHANGE
                    case ActionsEnum.REQDEPTCHANGE:
                        ReqDept = 0;
                        int.TryParse(hdfReqDeptPk.Value, out ReqDept);
                        ucrWrkf.ReqDeptID = (hdfIsWkfSettingPostBackReqd.Value == "1") ? ReqDept : 0;
                        ucrWrkf.FillWorkFlowDetails(true);
                        hdfIsSingleReqDeptWkfSetting.Value = "1";
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + ERP.Utilities.CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }






        #endregion

        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("AUTO COMPLETE SETTINGS", string.Empty, currentUser.SBUID);
            DataTable dtThreshold = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PRAdditionalQty", string.Empty, currentUser.SBUID);
            //For Enable Service PR Validation
            hdfEnableServicePR.Value = GetGlobalResourceObject("ConfigurationsRes", "EnableServicePR").ToString();

            //For enable Glove purchase Request
            hdfEnableGlovePR.Value = GetGlobalResourceObject("ConfigurationsRes", "GlovePurchaseRequest").ToString();
            //Hide or Show IO Numbwer DDL
            hdfIsIoNumberHide.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowIONoInPR").ToString();
            hdfEnbleCostCenter.Value = GetGlobalResourceObject("ConfigurationsRes", "EnableCostCenter").ToString();
            hdfIsPR_SCinBothSBU.Value = GetGlobalResourceObject("ConfigurationsRes", "IsPR_SCinBothSBU").ToString();
            hdfDataFromSC.Value = GetGlobalResourceObject("ConfigurationsRes", "DataFromSC").ToString();
            hdfPRGloveReqInSBU.Value = GetGlobalResourceObject("ConfigurationsRes", "PRGloveReqInSBU").ToString();
            hdfProductAddItemDisable.Value = GetGlobalResourceObject("ConfigurationsRes", "ProductAddItemDisable").ToString();
            hdfShowInvestor.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowInvestor").ToString();

            HAS_COST_CENTER.Value = hdfEnbleCostCenter.Value;

            if (GetGlobalResourceObject("ConfigurationsRes", "IsEnableExtraPer").ToString() == "0")
            {
                PRH_PERCENTAGE_EXTRA.Enabled = false;
                imbCalculatePer.Enabled = false;

            }
            if (dt != null && dt.Rows.Count > 0)
            {
                AutoStartValue.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            if (dtThreshold != null && dtThreshold.Rows.Count > 0)
            {
                hdfThreshold.Value = dtThreshold.Rows[0]["ACF_DATA"].ToString();
            }
            hdfIsGoToInbox.Value = GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString();
            //Enable/Disable company dropdown list  based on Config.
            if (GetGlobalResourceObject("ConfigurationsRes", "IsCompanyDisable").ToString() == "1")
            {
                PRH_COMPANY.Enabled = false;
            }
            //Showing Lot no/description of material in PR Specification field based on configuration.
            if (GetGlobalResourceObject("ConfigurationsRes", "LotNoPRSpecification").ToString() == "1")
            {
                hdfShowLotno.Value = "1";
            }
            hdfPRRowCount.Value = GetGlobalResourceObject("ConfigurationsRes", "PRRowCount").ToString();

            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            hdfValidatePurpose.Value = GetGlobalResourceObject("ConfigurationsRes", "PRPurposeValidation").ToString();
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("WKF SETTING", "PurReqDept");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsWkfSettingPostBackReqd.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString();
            }
            string ReturnURL = GetLocalResourceObject("BackUrl").ToString();
            if (Request.QueryString["Type"] != null)
            {
                ReturnURL += "?Type=" + Request.QueryString["Type"].ToString();
                if (Request.QueryString["Sbu"] != null)
                {
                    ReturnURL += "&Sbu=" + Request.QueryString["Sbu"].ToString();
                }
            }
            hdfBackUrl.Value = ReturnURL;

            #region Show/Hide Multiple UOM's (Purchase & Sale)
            if (GetGlobalResourceObject("ConfigurationsRes", "ShowMultipleUOM").ToString() == "1")
            {
                hdfShowMultipleUOM.Value = "1";
            }
            else
            {
                hdfShowMultipleUOM.Value = "0";
            }
            hdfShowPurchaseUOM.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowPurchaseUOM").ToString();
            hdfProjectRequired.Value = GetGlobalResourceObject("ConfigurationsRes", "EnablePRProjectValidation").ToString();
            #endregion

            if (hdfEnableGlovePR.Value == "1")
            {
                thPRRoL.Visible = false;
                thPRScQty.Visible = true;
            }
            else
            {
                thPRRoL.Visible = true;
                thPRScQty.Visible = false;
            }
        }


        private void FillForMaterialPlanningPurchase(int pk)
        {
            BusinessObject.PurchaseRequestManagement.PurchaseRequest purchaseRequest = null;
            if (pk != 0)
            {
                PurchaseRequestList.Value = BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.FillForMaterialPlanningPurchase(pk);
                PurchaseRequestDetails ob = JsonConvert.DeserializeObject<PurchaseRequestDetails>(PurchaseRequestList.Value);
                hdfReqDeptPk.Value = string.IsNullOrEmpty(ob.PRH_ISSUE_DEPT) ? "0" : ob.PRH_ISSUE_DEPT;
                FillProcessId();
                AST_DOC_MODE.Value = "0";
                hdfSlNo.Value = "-1";
            }

        }
        private void FillForPlanningPurchase(int pk)
        {
            BusinessObject.PurchaseRequestManagement.PurchaseRequest purchaseRequest = null;
            if (pk != 0)
            {
                PurchaseRequestList.Value = BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.GetForPlanningRequestDetails(pk);
                PurchaseRequestDetails ob = JsonConvert.DeserializeObject<PurchaseRequestDetails>(PurchaseRequestList.Value);
                hdfReqDeptPk.Value = string.IsNullOrEmpty(ob.PRH_ISSUE_DEPT) ? "0" : ob.PRH_ISSUE_DEPT;
                FillProcessId();
                AST_DOC_MODE.Value = "0";
                hdfSlNo.Value = "-1";
            }

        }

        /// <summary>
        /// Funtion used fill purchase request details
        /// </summary>
        /// <param name="pk"></param>
        private void FillPurchase(int pk)
        {
            BusinessObject.PurchaseRequestManagement.PurchaseRequest purchaseRequest = null;
            if (pk != 0)
            {
                PurchaseRequestList.Value = BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.GetPurchaseRequestDetails(pk);
                if (hdfIsMultiplePlant.Value == "1")
                {
                    PurchaseRequestDetails ob = JsonConvert.DeserializeObject<PurchaseRequestDetails>(PurchaseRequestList.Value);
                    hdfReqDeptPk.Value = string.IsNullOrEmpty(ob.PRH_ISSUE_DEPT) ? "0" : ob.PRH_ISSUE_DEPT;
                    FillProcessId();
                }
                AST_DOC_MODE.Value = "0";
                hdfSlNo.Value = "-1";
            }
            else
            {
                purchaseRequest = new BusinessObject.PurchaseRequestManagement.PurchaseRequest();
                purchaseRequest.PurchaseRequestList = new List<BusinessObject.PurchaseRequestManagement.PurchaseRequestDetailsList>();
                PurchaseRequestList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(purchaseRequest);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillPurchseRequestNo(objUser);
                btnPrint.Visible = false;
                AST_DOC_MODE.Value = GetDOCMODE();

            }
        }

        private void FillBudgetTotal(int Refid)
        {
            BusinessObject.PurchaseRequestManagement.PurchaseRequest purchaseRequest = null;
            if (Refid != 0)
            {
                DtBudget = BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.GetBudgetDetails(Refid);
            }
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.PR, 0, DateTime.Now);
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
        /// Funtion used fill Initial Data
        /// </summary>
        private void FillInitialData()
        {

            int refId = 0;
            int appId = 0;
            ReqDept = 0;
            int.TryParse(hdfReqDeptPk.Value, out ReqDept);

            if (Request.QueryString["Type"] != null)
            {
                PRH_TYPE.Value = int.Parse(Request.QueryString["Type"]).ToString();
            }
            if (Request.QueryString["Sbu"] != null)
            {
                PRH_SO_BIZUNIT.Value = int.Parse(Request.QueryString["Sbu"]).ToString();
            }
            FillProcessId(true, PRH_TYPE.Value, PRH_SO_BIZUNIT.Value);
            if (Request.QueryString["MPLID"] != null)
            {
                FillForMaterialPlanningPurchase(Convert.ToInt32(Request.QueryString["MPLID"].ToString()));
            }
            else
           if (Request.QueryString["PLID"] != null)
            {
                FillForPlanningPurchase(Convert.ToInt32(Request.QueryString["PLID"].ToString()));
            }
            else
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
                hdfRefID.Value = refId.ToString();
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
                FillPurchase(appId);
                ucrWrkf.ReqDeptID = (hdfIsMultiplePlant.Value == "1") ? ReqDept : 0;
                ucrWrkf.FillWorkFlowDetails(true);
                btnSave.Visible = false;
                hdfshowbudgetValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowBudgetSummary").ToString();
                if (hdfshowbudgetValidation.Value == "1")
                {
                    FillBudgetTotal(ucrWrkf.RefID);
                    if (DtBudget != null && DtBudget.Rows.Count > 0)
                        ucrWrkf.FillBudgetDetails(DtBudget);
                }
                //  btnAddSelectedItems.Visible = false;
            }
            if (Request.QueryString["Status"] != null)
            {
                ucrWrkf.ViewType = 0;
                if (Request.QueryString["RefID"] != null)
                {
                    //btnSubmit.Visible = false;
                    ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                    ucrWrkf.ReqDeptID = (hdfIsMultiplePlant.Value == "1") ? ReqDept : 0;
                    ucrWrkf.FillWorkFlowDetails(true);
                    Button btnWkfSubmit = (Button)ucrWrkf.FindControl("btnSubmitWrkf");
                    btnWkfSubmit.Visible = false;
                }
                else
                {
                    ucrWrkf.Visible = false;
                }
                btnSave.Visible = false;
                //  btnAddSelectedItems.Visible = false;
            }
            else
            {
                if (Request.QueryString["Flag"] != null)
                {
                    ucrWrkf.ViewType = 0;
                    btnSave.Visible = false;
                    //  btnAddSelectedItems.Visible = false;
                }
                else
                {
                    ucrWrkf.ViewType = 1;
                }
                ucrWrkf.Visible = true;
                FillProcessId(false, PRH_TYPE.Value, PRH_SO_BIZUNIT.Value);
            }
            //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt32(hdfDeptID.Value));
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                hdfSelCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
            }
            if (Request.QueryString["IsModify"] != null)
            {
                btnSave.Visible = true;
            }
            if (Request.QueryString["IsFromPO"] != null) //Coming from pending PR list of PO page
            {
                //for setting Requested Dept ,RefID is required
                #region Done all these for getting RefID
                int processId = 0, currPK = 0, prhDept = 0;
                string path = string.Empty;
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                {
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                }
                else
                {
                    path = Request.Url.AbsolutePath.ToLower();
                }
                if (Request.QueryString["PrhDept"] != null)
                {
                    prhDept = Convert.ToInt32(Request.QueryString["PrhDept"].ToString());
                }
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = null;
                dtProcess = wrkfService.GetProcessID(path, prhDept);
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    processId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                }
                if (Request.QueryString["PK"] != null)
                {
                    currPK = Convert.ToInt32(Request.QueryString["PK"].ToString());
                }
                #endregion
                hdfRefID.Value = wrkfService.GetRefID(currPK, processId).ToString();
            }
            if (Request.QueryString["Modify"] != null)
            {
                btnSave.Visible = true;
            }
        }

        /// <summary>
        /// Methode used to fill purchase request number
        /// </summary>
        /// <param name="objUser"></param>
        private void FillPurchseRequestNo(BusinessObject.User objUser)
        {
            //string purchseRequestNo;
            //DataTable dtSrsNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "PRNO", 0);
            //if (dtSrsNoFormat.Rows.Count > 0)
            //    purchseRequestNo = dtSrsNoFormat.Rows[0]["DFT_VALUE"].ToString();
            //else
            //    purchseRequestNo = GTIService.Constants.Common.CommonConstant.PRNUMBERFORMAT;
            //MatchCollection matchcol = Regex.Matches(purchseRequestNo, @"\#[\w]+\#");
            //for (int i = 0; i < matchcol.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        purchseRequestNo = purchseRequestNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#", string.Empty)));
            //    }
            //    else if (i == 1)
            //    {
            //        purchseRequestNo = purchseRequestNo.Replace(matchcol[i].ToString(), BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.GetPRNO());
            //    }

            //}
            PRH_NO.Text = Resources.Messages.DocGenerationNew;
            PRH_DATE.Text = DateTime.Now.ToString("dd-MMM-yyyy");
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

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessId(bool SetProcessID = false, string Type = "", string SbuID = "")
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            {
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            }
            else
            {
                path = Request.Url.AbsolutePath.ToLower();
            }
            if (!string.IsNullOrEmpty(Type) && Type != "0")
            {
                path += "?Type=" + Type;
                if (!string.IsNullOrEmpty(SbuID) && SbuID != "0")
                {
                    path += "&Sbu=" + SbuID;
                }
            }

            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            //DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            DataTable dtProcess = null;
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
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcessID.Value = ucrWrkf.ProcessID.ToString();
                ((HiddenField)this.Master.FindControl("hdnPageID")).Value = dtProcess.Rows[0]["PAG_PK"].ToString();
                ReqDept = 0;
                int.TryParse(hdfReqDeptPk.Value, out ReqDept);
                ucrWrkf.ReqDeptID = (hdfIsWkfSettingPostBackReqd.Value == "1") ? ReqDept : 0;
                ucrWrkf.FillWorkFlowDetails(true);
            }
        }


        #endregion

        private void GetUserRights()
        {
            string path = "/PurchaseRequestManagement/PurchaseRequestCreation.aspx";
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.SBUID);
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "ADDITEMS" && item.HasActionRight == true)
                    {
                        hdnAddMaterialsFromPR.Value = "1";
                    }
                }

            }
            hdfPrTypeRequired.Value = "0";
            BusinessLogic.CommonManagement.CommonBL objCommon = new BusinessLogic.CommonManagement.CommonBL();
            UserRightsBO UserVerifyRights = objCommon.GetWorkFlowUserRightsInfo(currentUser.PKUser, path, currentUser.CurrentDeptPK, ucrWrkf.RefID);
            if (UserVerifyRights != null || UserVerifyRights.Rights.Count > 0)
            {
                for (int i = 0; i < UserVerifyRights.Rights.Count; i++)
                {
                    if (UserVerifyRights.Rights[i].ActionName == "PRTYPE" && UserVerifyRights.Rights[i].HasActionRight == true)
                    {
                        hdfPrTypeRequired.Value = "1";
                    }
                }
            }
        }
    }
    public class PurchaseRequestDetails
    {
        public string PRH_ISSUE_DEPT
        { get; set; }
    }
}