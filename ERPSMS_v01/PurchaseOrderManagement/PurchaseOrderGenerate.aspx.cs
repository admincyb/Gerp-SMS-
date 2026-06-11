using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using ERPData;
using GTIService;
using ERP.Utilities;
using ERPService;
using BusinessObject.PurchaseOrderManagement;
using BusinessObject.AccountManagement;
using BusinessObject.AlertManagement;
using System.Threading;
using Newtonsoft.Json;
using BusinessLogic.AccountManagement;
using BusinessObject;

namespace ERPSMS_v01.PurchaseOrderManagement
{
    public partial class PurchaseOrderGenerate : ERP.Store.UI.MyBasePage
    {
        #region properties
        private int CurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrPK] = value;
            }
        }
        #endregion
        #region Variables
        private ActionsEnum commonActions;
        BusinessObject.User currentUser;
        AlertSettingBO objAlert;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private List<ADM_APP_CONFIG_MST> admAppConstMstList;
        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> admConfigMstList;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        DataSet dsAlertList;
        private CommonService cm;
        AlertBO alertBoObj;
        int poPK = 0;
        private string appType;
        private DataTable dtCompany;
        int ReqDept = 0;
        int pohStatus = 0;
        #endregion
        /// <summary>
        /// On Pre  Load Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreLoad(EventArgs e)
        {
            string pageUrl = Resources.PageURL.PurchaseOrderGenerate;
            if (Request.QueryString["TYPE"] != null)
                pageUrl = Resources.PageURL.PurchaseOrderGenerate + "?TYPE=" + Request.QueryString["TYPE"].ToString();
            base.currentPageURL = pageUrl;
            base.OnPreLoad(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (!IsPostBack)
            {
                bool isAmend = false;
                if (Request.QueryString["TYPE"] != null)
                    POH_MENU_TYPE.Value = Request.QueryString["TYPE"].ToString();
                else
                    POH_MENU_TYPE.Value = "1";

                if (Request.QueryString["AMEND"] != null)
                    isAmend = Convert.ToInt32(Request.QueryString["AMEND"].ToString()) > 0 ? true : false;

                PageActionHandler(Convert.ToInt32(POH_MENU_TYPE.Value), isAmend);

            }
        }

        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler(int type, bool isAmend)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            POH_CURRENCY_BC.Value = currentUser.BaseCurrency.ToString();
            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
                hdfDeptID.Value = Session[BusinessObject.Common.SessionStrings.CurDept].ToString();
            else
                hdfDeptID.Value = "0";
            if (Request.QueryString["TYPE"] == "5")
                hdfAppType.Value = ApplicationType.POG;
            else
                hdfAppType.Value = ApplicationType.PO;
            hdfAppSubType.Value = string.Empty;
            APT_CODE.Value = ApplicationType.PO;
            FillInitialData(type, isAmend);
            GetFieldValues(ControlsEnum.ALERTCONFIG);
            if (admAppConstMstList != null && admAppConstMstList.Count > 0)
            {
                isAlert.Value = admAppConstMstList[0].ACF_VALUE.ToString();
            }
            // dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt32(hdfDeptID.Value));
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                hdfCompany.Value = hdfSBUcompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
                hdfSBUCountry.Value = dtCompany.Rows[0]["CMP_CNTRY"].ToString();
            }
            //Comma Separation for Quantity & Amount Based on Configuration(Table)
            hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
            hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
            hdfShowTransactionPort.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowTransactionPort").ToString();
            hdfPRtypeEnabled.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowPRtype").ToString();
            hdfPRGroupEnabled.Value = GetGlobalResourceObject("ConfigurationsRes", "EnableServicePR").ToString();
            hdfPoCommentPrefix.Value = GetGlobalResourceObject("ConfigurationsRes", "PoCommentPrefix").ToString();

            dvPoCreator.Visible = false;
            //if PO is Converted then not show amend button
            if (POH_CONVERTED.Value == "1" && IsShowAmendBt.Value == "0")
            {
                btnAmend.Visible = false;
            }
            else
            {
                btnAmend.Visible = true;
            }
            if (GetGlobalResourceObject("ConfigurationsRes", "UseEmployeeAsPOCreator").ToString() == "1")
            {
                hdfPohEmployee.Value = Convert.ToString(1);
                dvPoCreator.Visible = true;
            }
        }

        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            this.btnAmend.PreRender += new EventHandler(btnAction_PreRender);
        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("EnablePOPriceEdit", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                isPOEditable.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PURCHASE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                isTaxAdd.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "TAX")["ACF_VALUE"].ToString();
                isDiscountAdd.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "DISCOUNT")["ACF_VALUE"].ToString();
            }
            //For autocomplete search min length
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("AUTO COMPLETE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                AutoStartValue.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            //Checking: OtherChargeTax is needed for Tax Calculation            
            IsTaxForOtherCharge.Value = (GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxPurchase")).ToString();
            hdfIsGoToInbox.Value = GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString();
            hdfDisablePOCategory.Value = GetGlobalResourceObject("ConfigurationsRes", "DisablePOCategory").ToString();
            hdfEnableDirectPO.Value = (GetGlobalResourceObject("ConfigurationsRes", "EnableDirectPO")).ToString();
            hdfShowPoOtherVendor.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowPoOtherVendor").ToString();
            hdfShowPOAmendAfterInvoice.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowPOAmendAfterInvoice").ToString();
            hdfEnablePOCountryValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "EnablePOCountryValidation").ToString();
            hdfEnablePOCurrencyValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "EnablePOCurrencyValidation").ToString();
            POH_REQ_BUDG_VALID.Value = GetGlobalResourceObject("ConfigurationsRes", "POBudgetValidationReq").ToString();
            hdfShowInvestor.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowInvestor").ToString();

            IsShowAmendBt.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowAmendButton").ToString();

            //For enable Glove purchase Request
            hdfEnableGlovePR.Value = GetGlobalResourceObject("ConfigurationsRes", "GlovePurchaseRequest").ToString();

            //Enable/Disable company dropdown list  based on Config.
            if (GetGlobalResourceObject("ConfigurationsRes", "IsCompanyDisable").ToString() == "1")
            {
                POH_COMPANY.Enabled = false;
            }
            //Include/Exclude TAX_NOT_DUE  in Tax Poup ddl. 1- Include,0- Exclude          
            hdnIsTaxNotDue.Value = GetGlobalResourceObject("ConfigurationsRes", "IsTaxNotDueForMaterialPO").ToString();
            hdfIsShowAlert.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowAlertInPO").ToString();//For Setting Visibilty of Alert Button w. r. to client
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";

            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("WKF SETTING", "PurReqDept");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsWkfSettingPostBackReqd.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString();
            }
            dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("WKF SETTING", "POCentLocal");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfPOCatWkfChange.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString();
            }
            hdfRestrictEditOption.Value = GetGlobalResourceObject("ConfigurationsRes", "RestrictEditOption").ToString();
            hdfBackUrl.Value = GetLocalResourceObject("BackUrl").ToString();
            if (Request.QueryString["TYPE"] != null)
            {
                hdfBackUrl.Value = GetLocalResourceObject("BackUrl").ToString() + "?TYPE=" + Request.QueryString["TYPE"].ToString();
            }
            hdfIsValidateCurrencyPoType.Value = GetGlobalResourceObject("ConfigurationsRes", "CurrencyPOTypeValidation").ToString();
            hdfEnbleCostCenter.Value = GetGlobalResourceObject("ConfigurationsRes", "EnableCostCenter").ToString();

            if (GetGlobalResourceObject("ConfigurationsRes", "ShowPOReqDept").ToString() == "0")
            {
                thPRReqDept.Visible = false;
                thPOReqDept.Visible = false;
                thPOGroup.Visible = false;
            }

            if (hdfEnableGlovePR.Value == "1")
            {
                divBillingDpt.Visible = false;
                divBillingVendor.Visible = true;
            }
            else
            {
                divBillingDpt.Visible = true;
                divBillingVendor.Visible = false;
            }
        }

        private void FillInitialData(int type, bool IsAmend)
        {
            ConfigurationSettings();
            int refId = 0;
            int appId = 0;
            int prefID = 0;

            if (Request.QueryString["POID"] != null)
            {
                FillPODetails(Convert.ToInt32(Request.QueryString["POID"]), type);
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillPODetails(0, type);
                btnPrint.Visible = false;
            }
            else if (Request.QueryString["RefID"] != null)
            {
                ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                refId = int.Parse(Request.QueryString["RefID"]);
                hdfRefID.Value = refId.ToString();
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        appId = Convert.ToInt32((dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? 0 : dtApplication.Rows[0]["refApplication"]);
                        hdfAppID.Value = appId.ToString();
                    }
                }
                FillPODetails(appId, type);
                ucrWrkf.FillWorkFlowDetails(true);
                btnSave.Visible = false;
            }

            if (Request.QueryString["PRefID"] != null)
            {
                hdfIsPRFromInbox.Value = "1";
                prefID = int.Parse(Request.QueryString["PRefID"]);
                hdfRefID.Value = prefID.ToString();
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtApplication = wrkfService.GetApplicationID(prefID);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        hdfApplicationPK.Value = (dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? "0" : dtApplication.Rows[0]["refApplication"].ToString();
                    }
                }
            }

            FillProcessId(type, false);
            //if (type == 2)
            if (IsAmend)
            {
                ucrWrkf.ViewType = 1;
                ucrWrkf.Visible = true;
                ucrWrkf.RefID = 0;
                FillProcessId(type, true);
                ucrWrkf.ViewAction();
                Button btnWkfSubmit = (Button)ucrWrkf.FindControl("btnSubmitWrkf");
                btnWkfSubmit.Visible = true;
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.RefID = workflowCore.GetRefID(appId, ucrWrkf.ProcessID);
                RefID.Value = ucrWrkf.RefID.ToString();
                ucrWrkf.FillWorkFlowDetails(true);
                btnAmend.Visible = false;
                if (!ucrWrkf.HasActions)
                {
                    ucrWrkf.ViewType = 0;
                    ucrWrkf.ViewAction();
                }
            }
            else if (Request.QueryString["Status"] != null)
            {
                ucrWrkf.ViewType = 0;
                if (Request.QueryString["RefID"] != null)
                {
                    //btnSubmit.Visible = false;
                    ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                    ucrWrkf.FillWorkFlowDetails(true);
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
                FillProcessId(type, false);
            }
            #region Show RateChangeHistory In Workflow Popup
            if (GetGlobalResourceObject("ConfigurationsRes", "ShowPOChangeHistory").ToString() == "1" && pohStatus > 0)
            {
                GetUserRights(type);//Inside this function ,both hdfRateHistoryGridHide & hdfVerificationRequired values are setted .
                //if (hdfRateHistoryGridHide.Value != "1")
                //{
                DataTable dtChangeHistory = BusinessLogic.CommonManagement.CommonBL.GetChangeHistory(CurrPK);
                if (dtChangeHistory != null && dtChangeHistory.Rows.Count > 0)
                {
                    bool isVerified = POH_VERIFIED.Value == "1" ? true : false;
                    bool IsVerificationRequired = hdfVerificationRequired.Value == "1" ? true : false;
                    ucrWrkf.FillWorkFlowChangeHistoryDetails(dtChangeHistory, IsVerificationRequired, isVerified, hdfRateHistoryGridHide.Value == "1" ? false : true);
                }
                //}
            }
            #endregion
        }

        /// <summary>
        /// Function Used to fill Vendor details to hiddenfiled
        /// </summary>
        /// <param name="vendorID"></param>        
        private void FillPODetails(int pOID, int Type)
        {
            if (pOID != 0)
            {
                PurchaseOrderList.Value = BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPODetails(pOID);
                BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
                file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
                FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
                POrderDetails ob = JsonConvert.DeserializeObject<POrderDetails>(PurchaseOrderList.Value);
                if (hdfIsWkfSettingPostBackReqd.Value == "1")
                {
                    POH_ISSUE_DEPT.Value = string.IsNullOrEmpty(ob.POH_ISSUE_DEPT) ? "0" : ob.POH_ISSUE_DEPT;
                    lblRequestedDept.Text = string.IsNullOrEmpty(ob.POH_ISSUE_DEPT_TEXT) ? "" : ob.POH_ISSUE_DEPT_TEXT;
                    FillProcessId(Type, false);
                }
                if (hdfPOCatWkfChange.Value == "1")
                {
                    hdfPOCategoryVal.Value = string.IsNullOrEmpty(ob.POH_PO_CATEGORY) ? "0" : ob.POH_PO_CATEGORY;
                    FillProcessId(Type, false);
                }
                pohStatus = ob.POH_STATUS;
                POH_VERIFIED.Value = string.IsNullOrEmpty(ob.POH_VERIFIED) ? "0" : ob.POH_VERIFIED;

                POH_CONVERTED.Value = string.IsNullOrEmpty(ob.POH_CONVERTED) ? "0" : ob.POH_CONVERTED;

                AST_DOC_MODE.Value = "0";
                CurrPK = pOID;
            }
            else
            {
                if (Request.QueryString["PRefID"] != null)
                {
                    DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetTransactionID(Convert.ToInt32(Request.QueryString["PRefID"]));
                    if (dt.Rows.Count > 0)
                        FillPODetails(Convert.ToInt32(dt.Rows[0]["appPK"]), Type);
                    else
                    {
                        SetPrimaryInfo();
                    }
                }
                else
                {
                    SetPrimaryInfo();
                }
            }
        }

        private void SetPrimaryInfo()
        {
            BusinessObject.PurchaseOrderGeneration.PurchaseOrder poObject = new BusinessObject.PurchaseOrderGeneration.PurchaseOrder();
            poObject.MaterialDetails = new List<BusinessObject.PurchaseOrderGeneration.PurchaseOrderMaterials> { };
            PurchaseOrderList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(poObject);
            BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
            file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
            FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
            FillPoNumber(((BusinessObject.User)(HttpContext.Current.User.Identity)));
            BusinessObject.User objUser = (BusinessObject.User)(HttpContext.Current.User.Identity);
            CreatedBy.Text = objUser.EmpName.Length > 35 ? objUser.EmpName.Substring(0, 35) + "..." : objUser.EmpName;
            CreatedBy.ToolTip = objUser.EmpName;
            AST_DOC_MODE.Value = GetDOCMODE();
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.PO, 0, DateTime.Now);
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
        ///Methord used to Fill Po Related data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="User object"></param>
        private void FillPoNumber(BusinessObject.User objUser)
        {
            POH_NO.Value = "";
            lblPOH_NO.Text = "[NEW]";
            POH_DATE.Text = DateTime.Now.ToString("dd-MMM-yyyy");

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
        private void FillProcessId(int type, bool isAmend)
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();

            //base.WkfPageUrl = path + "?TYPE=1";
            //path += "?TYPE=" + type.ToString();

            base.WkfPageUrl = path + "?TYPE=" + type.ToString();
            if (isAmend)
                path += "?TYPE=" + type.ToString() + "&AMEND=" + type.ToString();
            else
                path += "?TYPE=" + type.ToString();


            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by page and user dept,
            //DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);Session[SessionStrings.CurDept]
            DataTable dtProcess;
            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
                dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept]));
            else
                dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);


            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                // pass proc Id to wrkflw user control and fill action details 
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcessID.Value = dtProcess.Rows[0]["PROCESS_PK"].ToString();
                ((HiddenField)this.Master.FindControl("hdnPageID")).Value = dtProcess.Rows[0]["PAG_PK"].ToString();
                // Fill workflow details
                ReqDept = 0;
                //if (type != 2)//AMEND.No need to pass RequestedDepartment
                //{
                int.TryParse(POH_ISSUE_DEPT.Value, out ReqDept);
                //}
                ucrWrkf.ReqDeptID = (hdfIsWkfSettingPostBackReqd.Value == "1") ? ReqDept : 0;
                if (hdfPOCatWkfChange.Value == "1")
                {
                    int.TryParse(hdfPOCategoryVal.Value, out ReqDept);
                    ucrWrkf.ReqDeptID = ReqDept;
                }
                ucrWrkf.FillWorkFlowDetails(true);
            }
        }


        //===========================##### END Code For WorkFolw ###### =======================================
        #region -- For Buttons ---
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            int result;
            int? alertresult;
            string saveXml;
            int selectedItemPK;
            string action;
            int wkStatus = 0;
            DropDownList ddlWkfAction;

            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    #region PRINT
                    case ActionsEnum.ALERT:
                        AlertSettingBO objAlert = new AlertSettingBO();
                        objAlert.PageURL = HttpContext.Current.Request.Url.AbsoluteUri;
                        objAlert.TypePK = CurrPK;
                        objAlert.TypeText = GetLocalResourceObject("Alert_Type_Text").ToString();
                        Session[ERP.Utilities.SessionStrings.PoAlert] = objAlert;
                        Response.Redirect(Resources.PageURL.AlertURL + ApplicationType.PO);
                        break;
                    #endregion

                    #region AMEND PO
                    case ActionsEnum.AMEND:
                        PageActionHandler(Convert.ToInt32(POH_MENU_TYPE.Value), true);
                        POH_IS_AMEND.Value = "1";
                        POH_AMEND_DATE.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                        hdfCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                        break;
                    #endregion

                    #region ALERTSAVE
                    case ActionsEnum.ALERTSAVE:
                        AlertBO alertBoObj = new AlertBO();
                        alertBoObj = (AlertBO)SetUIValuesToObject(ControlsEnum.ALERTSAVE);
                        if (alertBoObj != null)
                        {
                            alertresult = BusinessLogic.AlertManagement.Alerts.SaveAlertDetails(alertBoObj);
                        }
                        if (hdfIsGoInbox.Value == "0")
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + saveMsg.Value
                                             + "','" + Resources.ErpRes.Information + "','" + hdfBackUrl.Value + "');", true);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + saveMsg.Value
                            //                    + "','" + Resources.ErpRes.Information + "','" + Resources.PageURL.PurchaseOrderList + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + saveMsg.Value
                                                   + "','" + Resources.ErpRes.Information + "','" + Resources.PageURL.InboxURL + "');", true);
                        }
                        break;
                    #endregion
                    #region REQ DEPT CHANGE
                    case ActionsEnum.REQDEPTCHANGE:
                        ReqDept = 0;
                        int.TryParse(POH_ISSUE_DEPT.Value, out ReqDept);
                        //ucrWrkf.ReqDeptID = (hdfIsWkfSettingPostBackReqd.Value == "1") ? ReqDept : 0;
                        FillProcessId(Convert.ToInt32(POH_MENU_TYPE.Value), false);
                        //ucrWrkf.FillWorkFlowDetails(true);
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                //Process Exception and show error message
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + ERP.Utilities.CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
                //reset all objects
            }
        }


        #endregion

        #region helper methods
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            try
            {
                switch (controlType)
                {

                    #region ALERT
                    case ControlsEnum.ALERTSAVE:
                        string Typename = Resources.Constants.SystemAlertType;
                        int AlertPk = 0;
                        alertBoObj = new AlertBO();
                        appType = ApplicationType.PO;
                        CurrPK = Convert.ToInt32(hdfAppID.Value);
                        if (CurrPK > 0)
                        {
                            GetFieldValues(ControlsEnum.ALERTLIST);
                            if (dsAlertList != null && dsAlertList.Tables[0].Rows.Count == 1)
                            {
                                AlertPk = Convert.ToInt32(dsAlertList.Tables[0].Rows[0]["ATH_PK"].ToString());
                            }
                        }
                        alertBoObj.ATH_PK = AlertPk;
                        alertBoObj.ATH_NO = "";
                        alertBoObj.ATH_DATE = DateTime.Now;
                        alertBoObj.ATH_TRX_TYPE = appType;
                        alertBoObj.ATH_TRX_PK = CurrPK;
                        alertBoObj.ATH_TRX_DATE = POH_DEPT.Text.Trim() == string.Empty ? DateTime.Now : Convert.ToDateTime(POH_DEPT.Text.Trim());
                        string duedays = (Math.Floor((DateTime.Now - alertBoObj.ATH_TRX_DATE).TotalDays)).ToString();
                        alertBoObj.ATH_DUE_DAYS = Convert.ToInt16(duedays);
                        alertBoObj.ATH_DUE_DATE = string.IsNullOrEmpty(leastDate.Value) ? DateTime.Now : Convert.ToDateTime(leastDate.Value);
                        alertBoObj.ATH_NAME = Typename;
                        alertBoObj.ATH_ALERT_TYPE = null;
                        GetFieldValues(ControlsEnum.ALERTBASIS);
                        if (admConfigMstList != null && admConfigMstList.Count > 0)
                        {
                            alertBoObj.ATH_BASIS = admConfigMstList[0].CFG_PK;
                            alertBoObj.ATH_NOTIFY_BFR_UOM = admConfigMstList[0].CFG_PK;
                            GetFieldValues(ControlsEnum.NOTIFICATIONDAYS);
                            if (admAppConstMstList != null && admAppConstMstList.Count > 0)
                            {
                                alertBoObj.ATH_NOTIFY_BFR = admAppConstMstList[0].ACF_VALUE;
                            }
                            else
                            {
                                alertBoObj.ATH_NOTIFY_BFR = 0;
                            }
                        }
                        else
                        {
                            alertBoObj.ATH_BASIS = null;
                            alertBoObj.ATH_NOTIFY_BFR_UOM = null;
                            alertBoObj.ATH_NOTIFY_BFR = 0;
                        }
                        alertBoObj.ATH_REMARKS = string.Empty;
                        if (!string.IsNullOrEmpty(lblPOH_NO.Text) && !lblPOH_NO.Text.Equals("[NEW]"))
                        {
                            alertBoObj.ATH_NARRATION = lblPOH_NO.Text.Trim();
                        }
                        alertBoObj.ATH_NOTIFY_MESSAGE = true;
                        // alertBoObj.ATH_NOTIFY_USER = Convert.ToInt16(currentUser.PKUser);
                        alertBoObj.ATH_STATUS = (byte)0;
                        alertBoObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        alertBoObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        alertBoObj.BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        alertBoObj.LAST_MOD_DT = string.IsNullOrEmpty(LastModifiedTime.Value) ? DateTime.Now : Convert.ToDateTime(LastModifiedTime.Value);
                        retObject = alertBoObj;
                        break;
                        #endregion
                }
                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }


        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            try
            {
                switch (type)
                {
                    #region ALERTCONFIG
                    case ControlsEnum.ALERTCONFIG:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(CommonServiceClient);
                        admAppConfigMstObj = new ADM_APP_CONFIG_MST();
                        admAppConfigMstObj.ACF_PK = 0;
                        admAppConfigMstObj.ACF_SETTING = Resources.Constants.AUTO_ALERT_FROM_TRX;
                        admAppConfigMstObj.ACF_DATA = "1";
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(ERP.Utilities.CommonConstants.ACTIVE);
                        admAppConstMstList = CommonServiceClient.GetAlertNotify(admAppConfigMstObj);
                        break;
                    #endregion
                    #region ALERTDETAILS
                    case ControlsEnum.ALERTLIST:
                        dsAlertList = BusinessLogic.AlertManagement.Alerts.GetAlertDetails(0, Convert.ToByte(DbActiveStatus.ACTIVE), null, appType, CurrPK, currentUser.SBUID, currentUser.PKUser, (int)AlertType.System);
                        break;
                    #endregion
                    #region ALERTBASIS
                    case ControlsEnum.ALERTBASIS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = Resources.Constants.ALERT_BASIS;
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(ERP.Utilities.CommonConstants.ACTIVE);
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion
                    #region NOTIFICATIONDAYS
                    case ControlsEnum.NOTIFICATIONDAYS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(CommonServiceClient);
                        admAppConfigMstObj = new ADM_APP_CONFIG_MST();
                        admAppConfigMstObj.ACF_PK = 0;
                        admAppConfigMstObj.ACF_SETTING = Resources.Constants.ALERT_NOTIFY_BEFORE;
                        admAppConfigMstObj.ACF_DATA = ApplicationType.PO;
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(ERP.Utilities.CommonConstants.ACTIVE);
                        admAppConstMstList = CommonServiceClient.GetAlertNotify(admAppConfigMstObj);
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CommonServiceClient = null;
            }
        }
        /// <summary>
        /// For getting user action rights againist current record .1.Checking Verification Required .2. Show/Hide RateChangeHistoryGrid 
        /// </summary>
        private void GetUserRights(int type)
        {
            BusinessLogic.CommonManagement.CommonBL userAuth;
            //string pageURL = this.WkfPageUrl;

            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            path += "?TYPE=" + type.ToString();



            userAuth = new BusinessLogic.CommonManagement.CommonBL();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //// get the user WorkFlow Rights
            UserRightsBO UserVerifyRights = userAuth.GetWorkFlowUserRightsInfo(currentUser.PKUser, path, currentUser.CurrentDeptPK, ucrWrkf.RefID);
            if (UserVerifyRights != null || UserVerifyRights.Rights.Count > 0)
            {
                for (int i = 0; i < UserVerifyRights.Rights.Count; i++)
                {
                    if (UserVerifyRights.Rights[i].ActionName == "RateVerify" && UserVerifyRights.Rights[i].HasActionRight == true)
                    {
                        hdfVerificationRequired.Value = "1";
                    }
                    if (UserVerifyRights.Rights[i].ActionName == "RateGridHide" && UserVerifyRights.Rights[i].HasActionRight == true)
                    {
                        hdfRateHistoryGridHide.Value = "1";
                    }
                }
            }
        }
        #endregion
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            ALERTCONFIG,
            ALERTLIST,
            ALERTTYPES,
            ALERTBASIS,
            ALERTSAVE,
            NOTIFICATIONDAYS,
        }

    }
    public class POrderDetails
    {
        public string POH_ISSUE_DEPT { get; set; }
        public string POH_PO_CATEGORY { get; set; }
        public string POH_ISSUE_DEPT_TEXT { get; set; }
        public string POH_VERIFIED { get; set; }
        public int POH_STATUS { get; set; }

        public string POH_CONVERTED { get; set; }

    }
}