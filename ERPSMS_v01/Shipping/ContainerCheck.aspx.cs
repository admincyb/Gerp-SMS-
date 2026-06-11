using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using ERPService;
using ERPData;
using BusinessObject.CommonManagement;
using System.Data;
using ERPService.Inventory;
using ERPManager;
using BusinessObject.Inventory;
using System.Web.UI.HtmlControls;
using System.Reflection;
using ERPService.Sales;
using BusinessObject.Sales;
using BusinessLogic.Shipping;
using System.IO;
using BusinessObject.Shipping;
//using BusinessObject.Shipping;


namespace ERPSMS_v01.Shipping
{
    public partial class ContainerCheck : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Shipping Plan PK
        /// </summary>
        private int ShippingPlanPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK] = value;
            }
        }
        /// <summary>
        /// Current PK
        /// </summary>
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

        /// <summary>
        /// Type Current PK
        /// </summary>
        private int TypeCurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CheckListTypePK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CheckListTypePK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CheckListTypePK] = value;
            }
        }

        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);

            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }
        /// <summary>
        /// To maintain the LastModifiedTime in viewstate
        /// </summary>
        private DateTime LastModifiedTime
        {
            get
            {
                return this.ViewState[ViewstateStrings.LastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.LastModifiedTime];

            }
            set
            {
                this.ViewState[ViewstateStrings.LastModifiedTime] = value;
            }
        }
        /// <summary>
        /// Aplication referance ID
        /// </summary>
        private int ReferanceID
        {
            get
            {
                return this.ViewState["ReferanceID"] == null ? 0 : Convert.ToInt32(this.ViewState["ReferanceID"].ToString());
            }
            set
            {
                this.ViewState["ReferanceID"] = value;
            }
        }

        private int CurrSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] = value;
            }
        }

        private List<BusinessObject.Shipping.ShippingUploadsBO> ShippingUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.ShippingUploadList] == null ? null : (List<BusinessObject.Shipping.ShippingUploadsBO>)ViewState[ViewstateStrings.ShippingUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.ShippingUploadList] = value;
            }
        }

        private List<BusinessObject.Shipping.FileDetails> FileDetailsList
        {
            get
            {
                return Session["FileDetailsList"] == null ? null : (List<BusinessObject.Shipping.FileDetails>)Session["FileDetailsList"];
            }
            set
            {
                Session["FileDetailsList"] = value;
            }
        }


        #endregion

        #region Variables
        // Indicates the state as well as action
        private ActionsEnum commonActions;

        //page related Entity Object
        //Object for Container Eval Header
        private SAL_CONTAINER_EVAL_HDR SalContainerEvalHdrObj;
        //Object for Checklist group master
        private ADM_CHECK_LIST_GROUP_MST admCheckListGroupMstObj;
        //Object for Checklist item master
        private ADM_CHECK_LIST_ITEM_MST admCheckListItemMstObj;
        //Object for Checklist item header
        private ADM_CHECK_LIST_TRX_HDR admCheckListTrxHdrObj;
        //Object for Checklist item Details
        private ADM_CHECK_LIST_TRX_DTL admCheckListTrxDtlObj;
        //Object for vendor master
        private PUR_VENDOR_MST PurVedorMstObj;

        //List for binding details to controls  
        //List for Container Eval Number
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        //List for Cont Eval Header
        private List<SAL_CONTAINER_EVAL_HDR> SalContainerEvalHdrList;
        //List for check list items header
        private List<ADM_CHECK_LIST_TRX_HDR> admCheckListTrxHdrList;
        //List for check list items details
        private List<ADM_CHECK_LIST_TRX_DTL> admCheckListTrxDtlList;
        //List for Vendor master
        private List<PUR_VENDOR_MST> PurVedorMstList;
        //List for Carrier
        private List<ADM_CONST_MST> admConstMstList;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private int CompanyPkByUserSBU = 0;

        //object for user
        private BusinessObject.User currentUser;
        private BusinessObject.Shipping.ShippingUploadsBO shippingUploadObj;

        private CommonService cm;

        //workflow variables
        private string refID;
        private string inboxFlag;

        //For Cont Eval Number
        private string ContainerEvaluationNo;

        private ServiceUtility serviceUtilityObj;

        //Dataset for binding details to controls  
        private DataSet dsShippingPlanHDR;
        private DataSet dsLoadingPlan;
        private DataSet dsShippingUploads;

        private DataTable dtPageData;

        private int tabLevel;
        private int prevCompany = 0;
        #endregion

        #endregion

        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            string resultXml;
            string xmlHeader;
            resultXml = string.Empty;
            xmlHeader = string.Empty;
            ContainerEvaluationNo = string.Empty;
            try
            {
                //workflow integration purpose
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.COMPANYLIST);
                    SetFieldValues(ControlsEnum.COMPANYLIST);
                    //set the shipping plan PK from session
                    ShippingPlanPK = Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK]) : 0;

                    FileDetailsList = null;
                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "SCD_SL_NO";
                    grdUploads.DataKeyNames = itemkeyarray;
                    //txtUploadDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);
                    //get and set the Carrier
                    GetFieldValues(ControlsEnum.CARRIER);
                    SetFieldValues(ControlsEnum.CARRIER);

                    //For Cont Eval Number
                    AST_DOC_MODE.Value = ((int)DOCMODE.Submit).ToString();

                    //Used for Integration purpose
                    FillProcessID();

                    //set Checklist Type and Title
                    uclCheckList.CLTitle = GetLocalResourceObject("EvaluationCheckList").ToString();
                    TypeCurrPK = GetCheckListType();
                    uclCheckList.CLType = TypeCurrPK;
                    uclCheckList.CLTypeCode = ApplicationType.CNTEVAL;

                    //Used for Integration purpose
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                       : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        ReferanceID = int.Parse(refID);
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                            //btnSave.Visible = false;
                            //btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(refID);
                        Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = ShippingPlanPK = GetApplicationID(ucrWrkf.RefID);
                    }
                    //if shippingPlan Pk >0  the create the Condition Check otherwise go to shipping plan list
                    if (ShippingPlanPK > 0)
                    {
                        //Used for Integration purpose
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        EntryStatus = EntryStatus.ENTRYMODE;
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                        }

                        //Set Header info 
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);

                        //set the Condition Check Info
                        GetFieldValues(ControlsEnum.CONTAINEREVALUATION);
                        SetFieldValues(ControlsEnum.CONTAINEREVALUATION);
                        uclCheckList.HeaderCurrPK = CurrPK;
                        AST_DOC_MODE.Value = ((int)DOCMODE.Submit).ToString();

                        GetFieldValues(ControlsEnum.UPLOADTYPE);
                        SetFieldValues(ControlsEnum.UPLOADTYPE);
                        GetFieldValues(ControlsEnum.UPLOADEDFILES);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);

                        //if (hdfStatus.Value == ((int)WorkFlowStatus.APPROVED).ToString())
                        //{
                        //    EntryStatus = EntryStatus.VIEWMODE;
                        //}                        
                    }
                    else
                    {
                        Response.Redirect(Resources.PageURL.ShippingPlan);
                    }
                    //set the tab for user
                    SetTabVisibility();

                    hdfAppType.Value = ApplicationType.CNTEVAL;
                    hdfAppSubType.Value = string.Empty;
                    if (GetGlobalResourceObject("ConfigurationsRes", "SealNoShippingPlan").ToString() == "1")
                    {
                        lblSealNo.Visible = true;
                        txtSealNo.Visible = true;
                    }
                    else
                    {
                        lblSealNo.Visible = false;
                        txtSealNo.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            //For Check list Master Service
            AdmCheckListMstService AdmCheckListServiceClient = null;
            //For Commmon Service
            CommonService CommonServiceClient = null;
            //For Cont Eval Service
            ContainerEvaluationService ContainerEvaluationServiceClient = null;
            try
            {
                //initialize the Cont Eval Service
                ContainerEvaluationServiceClient = new ContainerEvaluationService();
                ContainerEvaluationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerEvaluationServiceClient);
                //initialize the Check list Master Service
                AdmCheckListServiceClient = new AdmCheckListMstService();
                AdmCheckListServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCheckListServiceClient);
                //initialize the Common Service
                CommonServiceClient = new CommonService();
                //initialize the Check list group
                admCheckListGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_GROUP_MST>();
                //initialize the Check list item master           
                admCheckListItemMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_ITEM_MST>();
                //initialize the Cont Eval Header 
                SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                //initialize the Check list item header     
                admCheckListTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_HDR>();
                //initialize the Check list item details     
                admCheckListTrxDtlObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_DTL>();
                //initialize the vendor
                PurVedorMstObj = ERP.Utilities.CommonFunctions.Initilize<PUR_VENDOR_MST>();
                switch (type)
                {
                    #region Get SalContainerEvalHdr List
                    case ControlsEnum.CONTAINEREVALUATION:
                        //initialize the Service Utility for paging ,sorting ,fillter etc
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        //SalContainerEvalHdrObj.CVH_PK = CurrPK;
                        SalContainerEvalHdrObj.CVH_SHIPPING_PLAN = ShippingPlanPK;
                        SalContainerEvalHdrObj.CVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        //get the Cont Eval list
                        SalContainerEvalHdrList = ContainerEvaluationServiceClient.GetContainerEvaluationList(SalContainerEvalHdrObj, serviceUtilityObj);
                        break;
                    #endregion

                    #region Generate Container Evaluation No
                    case ControlsEnum.CONTAINEREVALUATIONNO:
                        //get Cont Eval Number
                        ContainerEvaluationNo = ContainerEvaluationServiceClient.GetContainerEvaluationNo(ApplicationType.CNTEVAL, 0, 1,
                            Convert.ToDateTime(txtDate.Text.Trim()), currentUser.PKUser, true, 0, Convert.ToInt32(ddlCompany.SelectedValue));
                        break;
                    #endregion

                    #region Get Vendor List
                    case ControlsEnum.COMPANY:
                        //get the active vendors
                        PurVedorMstObj.VEN_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        PurVedorMstList = ContainerEvaluationServiceClient.GetCompany(PurVedorMstObj).OrderBy(p => p.VEN_NAME).ToList();
                        break;
                    #endregion

                    #region Shipping Plan Hdr
                    case ControlsEnum.SHIPPINGPLANLEVEL:
                        //get the shipping header
                        dsShippingPlanHDR = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanHDR(currentUser, ShippingPlanPK, Convert.ToInt32(CommonConstants.ACTIVE));

                        break;
                    #endregion

                    #region Carrier
                    case ControlsEnum.CARRIER:
                        //get the carriers
                        CommonServiceClient = new CommonService();
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, Convert.ToByte(DbActiveStatus.ACTIVE), null, 13, 5, null);
                        break;
                    #endregion

                    #region Company
                    case ControlsEnum.COMPANYLIST:
                        //gets Company List                        
                        AdmCompanyMstService admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        break;
                    #endregion

                    #region Default
                    case ControlsEnum.DEFAULT:
                        //get the loading plan details
                        dsLoadingPlan = new DataSet();
                        dsLoadingPlan = LoadingPlanBL.GetLoadingPlan(ShippingPlanPK, currentUser.SBUID, 1);
                        //dsLoadingPlan = BusinessLogic.Shipping.ShippingPlanBL.GetPlanInfo(ShippingPlanPK);
                        //To get prev transaction company
                        if (CurrPK == 0)
                        {
                            prevCompany = BusinessLogic.Shipping.ShippingPlanBL.GetPrevCompany(ShippingPlanPK);
                        }
                        break;
                    #endregion

                    #region UPLOADTYPE
                    case ControlsEnum.UPLOADTYPE:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ShippingUploadsEnum.ContainerEvaluation, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID);
                        break;
                    #endregion

                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        dsShippingUploads = BusinessLogic.Shipping.ShippingUploadsBL.GetShippingUploads(0, ShippingPlanPK, Convert.ToByte(DbActiveStatus.ACTIVE), (int)ShippingUploadsEnum.ContainerEvaluation);
                        if (dsShippingUploads != null && dsShippingUploads.Tables[0].Rows.Count > 0)
                        {
                            var ShippingUpload = from myRow in dsShippingUploads.Tables[0].AsEnumerable()
                                                 select new ShippingUploadsBO()
                                                 {
                                                     SCD_PK = myRow.Field<int>("SCD_PK"),
                                                     SCD_SL_NO = myRow.Field<int>("SCD_PK"),
                                                     SCD_PLAN_HDR = myRow.Field<int>("SCD_PLAN_HDR"),
                                                     SCD_PLAN_HDR_NO = myRow.Field<string>("SCD_PLAN_HDR_NO"),
                                                     SCD_TYPE = myRow.Field<byte>("SCD_TYPE"),
                                                     SCD_ITEM = myRow.Field<int>("SCD_ITEM"),
                                                     SCD_ITEM_Text = myRow.Field<string>("SCD_ITEM_Text"),
                                                     SCD_DATE = myRow.Field<string>("SCD_DATE"),
                                                     SCD_TITLE = myRow.Field<string>("SCD_TITLE"),
                                                     SCD_DESC = myRow.Field<string>("SCD_DESC"),
                                                     SCD_FILE = myRow.Field<string>("SCD_FILE"),
                                                     SCD_FILE_PATH = myRow.Field<string>("SCD_FILE_PATH"),
                                                     SCD_ACTIVE = myRow.Field<byte>("SCD_ACTIVE"),
                                                     SCD_MOD_BY = myRow.Field<int>("SCD_MOD_BY"),
                                                     SCD_MOD_DT = myRow.Field<DateTime>("SCD_MOD_DT"),
                                                     SCD_COMPANY = myRow.Field<int>("SCD_COMPANY")

                                                 };
                            if (ShippingUpload != null)
                                ShippingUploadList = ShippingUpload.ToList();

                        }

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
                admCheckListGroupMstObj = null;
                AdmCheckListServiceClient = null;
                CommonServiceClient = null;
                admCheckListItemMstObj = null;
                SalContainerEvalHdrObj = null;
                ContainerEvaluationServiceClient = null;
                admCheckListTrxHdrObj = null;
                admCheckListTrxDtlObj = null;
                PurVedorMstObj = null;
            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Bind Container Evaluation Details
                    case ControlsEnum.CONTAINEREVALUATION:
                        GetUIValuesFromObject(ControlsEnum.CONTAINEREVALUATION);
                        break;
                    #endregion

                    #region Bind Vendor List
                    case ControlsEnum.COMPANY:
                        BindDropDownList(controlType);
                        break;
                    #endregion

                    #region Bind Carrier List
                    case ControlsEnum.CARRIER:
                        BindCarrierDropDown();
                        break;
                    #endregion

                    #region Bind Company
                    case ControlsEnum.COMPANYLIST:
                        BindDropDownList(ControlsEnum.COMPANYLIST);
                        break;
                    #endregion

                    #region DEFAULT
                    case ControlsEnum.DEFAULT:
                        if (dsLoadingPlan != null && dsLoadingPlan.Tables[0] != null && dsLoadingPlan.Tables[0].Rows.Count > 0)
                        {

                            lblDestinationPortValue.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_SHIP_TO_PORT].ToString(), 20); ;
                            lblDestinationPortValue.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_SHIP_TO_PORT].ToString(), 300);

                            lblCustomerHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUS_TEXT].ToString(), 25);
                            lblCustomerHdr.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUS_TEXT].ToString(), 300);

                            lblShippingPlanNoHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_NO].ToString(), 20);
                            lblShippingPlanNoHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_NO].ToString();

                            lblShippingPlanDateHdr.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString()).ToString(Resources.Constants.DateFormatShort), 20) : string.Empty;
                            lblShippingPlanDateHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString() != string.Empty ? Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString()).ToString(Resources.Constants.DateFormatShort) : string.Empty;

                        }
                        break;
                    #endregion

                    case ControlsEnum.UPLOADTYPE:
                        BindDropDownList(ControlsEnum.UPLOADTYPE);
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.UPLOADEDFILES:
                        if (ShippingUploadList != null)
                        {
                            grdUploads.DataSource = ShippingUploadList;
                            grdUploads.DataBind();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Action Handlers
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            int resultPK, result, selectedItemPK;
            bool completed;
            string action;
            DropDownList ddlWkfAction;
            ContainerEvaluationService ContainerEvaluationServiceClient = null;
            CommonService CommonServiceClient = null;
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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                switch (commonActions)
                {
                    #region Save
                    // Do Action for , when click Save Button
                    case ActionsEnum.SAVE:
                        hdfChangeFocus.Value = "0";
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            SalContainerEvalHdrList = new List<SAL_CONTAINER_EVAL_HDR>();
                            ContainerEvaluationServiceClient = new ContainerEvaluationService();
                            ContainerEvaluationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerEvaluationServiceClient);
                            CommonServiceClient = new CommonService();
                            CommonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(CommonServiceClient);

                            //if the Condition Check No generate in Draft Mode then get the Condition Check No
                            //If already generated in Condition Check No then that Condition Check No is used
                            if (hdfEvaluationNo.Value == string.Empty && AST_DOC_MODE.Value == ((int)DOCMODE.Draft).ToString())
                            {
                                GetFieldValues(ControlsEnum.CONTAINEREVALUATIONNO);
                            }
                            else
                            {
                                ContainerEvaluationNo = hdfEvaluationNo.Value;
                            }
                            SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                            SalContainerEvalHdrObj = SetUIValuesToObject(commonActions);
                            SalContainerEvalHdrList.Add(SalContainerEvalHdrObj);
                            //Save Condition Check Info
                            resultPK = ContainerEvaluationServiceClient.SaveContainerEvaluation(SalContainerEvalHdrList);
                            if (resultPK >= 0) // Success ! re-initialize the page
                            {

                                //Save Check List Header Info
                                admCheckListTrxHdrList = new List<ADM_CHECK_LIST_TRX_HDR>();
                                admCheckListTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_HDR>();
                                admCheckListTrxHdrObj.CLH_TRX_PK = resultPK;
                                admCheckListTrxHdrObj.CLH_NO = SalContainerEvalHdrObj.CVH_NO;
                                admCheckListTrxHdrObj.CLH_DATE = SalContainerEvalHdrObj.CVH_DATE;
                                admCheckListTrxHdrObj.CLH_TRX_TYPE = ApplicationType.CNTEVAL;
                                admCheckListTrxHdrObj.CLH_MOD_DT = LastModifiedTime;
                                //set the CheckList Header Object
                                admCheckListTrxHdrObj = uclCheckList.SetCheckListHeaderValuesToObject(admCheckListTrxHdrObj);
                                admCheckListTrxHdrList.Add(admCheckListTrxHdrObj);
                                result = CommonServiceClient.SaveCheckListTrxHdr(admCheckListTrxHdrList);
                                if (result > 0)
                                {
                                    //Save Check List Details Info
                                    admCheckListTrxDtlObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_DTL>();
                                    admCheckListTrxDtlList = uclCheckList.SetCheckListDetailsValuesToObject(result);
                                    result = CommonServiceClient.SaveCheckListTrxDtl(admCheckListTrxDtlList, result);
                                    if (result > 0)
                                    {
                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("ContainerEvaluation").ToString());
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        //    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                                        //completed = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                           + "','" + Resources.ErpRes.Information + "');", true);
                                        completed = true;
                                        GetFieldValues(ControlsEnum.CONTAINEREVALUATION);
                                        SetFieldValues(ControlsEnum.CONTAINEREVALUATION);
                                    }
                                    else
                                    {
                                        completed = false;
                                    }
                                }
                                else
                                {
                                    completed = false;
                                }

                                #region ATTACHMENT SAVE
                                SaveAttachments();
                                #endregion



                                //IF any error occured in save Check List Header and Deatisl Info then delete saved items
                                if (!completed)
                                {
                                    SalContainerEvalHdrObj = null;
                                    SalContainerEvalHdrList = null;
                                    SalContainerEvalHdrList = new List<SAL_CONTAINER_EVAL_HDR>();
                                    SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                                    SalContainerEvalHdrObj.CVH_PK = resultPK;
                                    SalContainerEvalHdrList.Add(SalContainerEvalHdrObj);
                                    result = ContainerEvaluationServiceClient.DeleteContainerEvaluation(SalContainerEvalHdrList);
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                                //Shipping Plan Summary save for mobile app
                                int resultSummary = BusinessLogic.Shipping.ShippingPlanBL.SaveSummary(ShippingPlanPK, Convert.ToInt32(hdfProcessID.Value));
                                if (resultSummary <= 0)
                                {

                                    litErrorMsg.Text = Resources.Messages.SummaryActionFailed;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                            }
                            else
                            {
                                if (resultPK == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                                else if (resultPK == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                                }
                                else if (resultPK == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                                }
                                else if (resultPK == (int)DbSaveStatus.ALREADYCREATED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + GetLocalResourceObject("AlreadyCreated").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }

                        }
                        break;
                    #endregion

                    #region DELETE
                    // Do Action for , when click Delete Button
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            ContainerEvaluationServiceClient = new ContainerEvaluationService();
                            ContainerEvaluationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerEvaluationServiceClient);
                            SalContainerEvalHdrList = new List<SAL_CONTAINER_EVAL_HDR>();
                            SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                            SalContainerEvalHdrObj.CVH_PK = CurrPK;
                            SalContainerEvalHdrList.Add(SalContainerEvalHdrObj);

                            result = ContainerEvaluationServiceClient.DeleteContainerEvaluation(SalContainerEvalHdrList);

                            if (result >= 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);

                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region SAVESUBMIT
                    // Do Action for , when click Save&Submit Button
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        TextBox comments1 = (TextBox)ucrWrkf.FindControl("WrkfComments");
                        if (comments1 != null)
                        {
                            comments1.Text = string.Empty;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region SUBMIT
                    // Do Action for , when click Submit Button
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        TextBox comments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                        if (comments != null)
                        {
                            comments.Text = string.Empty;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region WRKSUBMIT
                    // Do Action for , when click Workflow Submit Button
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        hdfChangeFocus.Value = "0";
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                //if the Condition Check No generate in Draft Mode then get the Condition Check No
                                //If already generated in Condition Check No then that Condition Check No is used
                                if (hdfEvaluationNo.Value == string.Empty)
                                {
                                    GetFieldValues(ControlsEnum.CONTAINEREVALUATIONNO);
                                }
                                else
                                {
                                    ContainerEvaluationNo = hdfEvaluationNo.Value;
                                }

                                SalContainerEvalHdrList = new List<SAL_CONTAINER_EVAL_HDR>();
                                ContainerEvaluationServiceClient = new ContainerEvaluationService();
                                ContainerEvaluationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerEvaluationServiceClient);
                                CommonServiceClient = new CommonService();
                                CommonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(CommonServiceClient);
                                SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                                SalContainerEvalHdrObj = SetUIValuesToObject(commonActions);
                                SalContainerEvalHdrList.Add(SalContainerEvalHdrObj);
                                //Save Condition Check Info
                                resultPK = ContainerEvaluationServiceClient.SaveContainerEvaluation(SalContainerEvalHdrList);

                                if (resultPK >= 0) // Success ! re-initialize the page
                                {
                                    //Save CheckList Header Info
                                    admCheckListTrxHdrList = new List<ADM_CHECK_LIST_TRX_HDR>();
                                    admCheckListTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_HDR>();
                                    admCheckListTrxHdrObj.CLH_TRX_PK = resultPK;
                                    admCheckListTrxHdrObj.CLH_NO = SalContainerEvalHdrObj.CVH_NO;
                                    admCheckListTrxHdrObj.CLH_DATE = SalContainerEvalHdrObj.CVH_DATE;
                                    admCheckListTrxHdrObj.CLH_TRX_TYPE = ApplicationType.CNTEVAL;
                                    admCheckListTrxHdrObj.CLH_MOD_DT = LastModifiedTime;
                                    admCheckListTrxHdrObj = uclCheckList.SetCheckListHeaderValuesToObject(admCheckListTrxHdrObj);
                                    admCheckListTrxHdrList.Add(admCheckListTrxHdrObj);
                                    result = CommonServiceClient.SaveCheckListTrxHdr(admCheckListTrxHdrList);
                                    if (result > 0)
                                    {
                                        //Save CheckList Details Info
                                        admCheckListTrxDtlObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_DTL>();
                                        admCheckListTrxDtlList = uclCheckList.SetCheckListDetailsValuesToObject(result);
                                        result = CommonServiceClient.SaveCheckListTrxDtl(admCheckListTrxDtlList, result);
                                        if (result > 0)
                                        {
                                            //Workflow submission
                                            ucrWrkf.ApplicationID = ShippingPlanPK;
                                            completed = true;
                                        }
                                        else
                                        {
                                            completed = false;
                                        }
                                    }
                                    else
                                    {
                                        completed = false;
                                    }

                                    #region ATTACHMENT SAVE
                                    SaveAttachments();
                                    #endregion

                                    if (!completed)
                                    {
                                        SalContainerEvalHdrObj = null;
                                        SalContainerEvalHdrList = null;
                                        SalContainerEvalHdrList = new List<SAL_CONTAINER_EVAL_HDR>();
                                        SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                                        SalContainerEvalHdrObj.CVH_PK = resultPK;
                                        SalContainerEvalHdrList.Add(SalContainerEvalHdrObj);
                                        result = ContainerEvaluationServiceClient.DeleteContainerEvaluation(SalContainerEvalHdrList);
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);

                                    }
                                    //Shipping Plan Summary save for mobile app
                                    int resultSummary = BusinessLogic.Shipping.ShippingPlanBL.SaveSummary(ShippingPlanPK, Convert.ToInt32(hdfProcessID.Value));
                                    if (resultSummary <= 0)
                                    {

                                        litErrorMsg.Text = Resources.Messages.SummaryActionFailed;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    }

                                }
                                else
                                {
                                    if (resultPK == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);

                                    }
                                    else if (resultPK == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                                    }
                                    else if (resultPK == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                                    }
                                    else if (resultPK == (int)DbSaveStatus.ALREADYCREATED)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + GetLocalResourceObject("AlreadyCreated").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                            else
                                ucrWrkf.ApplicationID = ShippingPlanPK;
                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result > 0)
                                    {

                                        SetTabVisibility();
                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("ContainerEvaluation").ToString());
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        //    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        //   + "','" + Resources.ErpRes.Information + "');", true);

                                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanPK, ucrWrkf.ProcessID);
                                        ucrWrkf.FillWorkFlowDetails();
                                        EntryStatus = EntryStatus.ENTRYMODE;
                                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                            ucrWrkf.ViewType = 1;
                                        else
                                        {
                                            ucrWrkf.ViewType = 0;
                                            //EntryStatus = EntryStatus.VIEWMODE;
                                        }
                                        ucrWrkf.ViewAction();

                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            GetFieldValues(ControlsEnum.CONTAINEREVALUATION);
                                            SetFieldValues(ControlsEnum.CONTAINEREVALUATION);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Trx not saved
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }

                        }
                        break;
                    #endregion

                    #region CANCEL
                    // Do Action for , when click Cancel Button
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ShippingPlan), false);
                        break;
                    #endregion

                    #region PRINT
                    // Do Action for , when click Print Button
                    case ActionsEnum.PRINT:
                        PrinterControl1.ShippingPlanID = ShippingPlanPK;
                        PrinterControl1.SetCommericalInvoice(PrinterControl1.ShippingPlanID);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);
                        break;
                    #endregion

                    #region Tab navigation
                    // Do Action for , when click SaleContract tab
                    case ActionsEnum.DEFAULT:
                        Response.Redirect(Resources.PageURL.SalesOrderListing);
                        break;
                    // Do Action for , when click Shipping Plan tab
                    case ActionsEnum.SHIPPINGPLAN:
                        Response.Redirect(Resources.PageURL.ShippingPlan);
                        break;
                    // Do Action for , when click Cont. Eval. tab
                    case ActionsEnum.CONTAINEREVALUATION:
                        Response.Redirect(Resources.PageURL.ContainerEvaulation);
                        break;
                    // Do Action for , when click Cont. Insp. tab
                    case ActionsEnum.CONTAINERINSPECTION:
                        Response.Redirect(Resources.PageURL.ContainerInspection);
                        break;
                    // Do Action for , when click QA Docs tab
                    case ActionsEnum.UPLOADQA:
                        Response.Redirect(Resources.PageURL.UploadQa);
                        break;
                    // Do Action for , when click Exp.Docs tab
                    case ActionsEnum.UPLOADEXPORT:
                        Response.Redirect(Resources.PageURL.UploadExport);
                        break;
                    // Do Action for , when click Load Plan tab
                    case ActionsEnum.LOADINGPLAN:
                        Response.Redirect(Resources.PageURL.LoadingPlan);
                        break;
                    // Do Action for , when click Photos tab
                    case ActionsEnum.UPLOADPHOTOGRAPHS:
                        Response.Redirect(Resources.PageURL.UploadPhotographs);
                        break;
                    // Do Action for , when click GON tab
                    case ActionsEnum.GOODOUTWARD:
                        Response.Redirect(Resources.PageURL.GoodOutward);
                        break;
                    // Do Action for , when click Cont. Release tab
                    case ActionsEnum.CONTAINERRELEASE:
                        Response.Redirect(Resources.PageURL.ContainerRelease);
                        break;
                    // Do Action for , when click B/L tab
                    case ActionsEnum.BL:
                        Response.Redirect(Resources.PageURL.BillofLoading);
                        break;
                    #endregion

                    #region ADDITEM
                    case ActionsEnum.ADDITEM:
                        hdfChangeFocus.Value = "1";
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {

                            if (CurrSlNo != 0)
                            {
                                if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                {
                                    shippingUploadObj = ShippingUploadList.SingleOrDefault(itm => itm.SCD_SL_NO == CurrSlNo);
                                    if (shippingUploadObj != null)
                                    {
                                        if (FileDetailsList == null)
                                        {
                                            FileDetailsList = new List<BusinessObject.Shipping.FileDetails>();
                                        }
                                        int UploadTypeId = 0;
                                        int.TryParse(ddlType.SelectedValue, out UploadTypeId);
                                        shippingUploadObj.SCD_TYPE = Convert.ToByte(ShippingUploadsEnum.ContainerEvaluation);
                                        shippingUploadObj.SCD_ITEM = UploadTypeId;// Convert.ToInt32(ddlType.SelectedValue);
                                        shippingUploadObj.SCD_ITEM_Text = UploadTypeId > 0 ? ddlType.SelectedItem.Text : string.Empty;
                                        shippingUploadObj.SCD_DATE = DateTime.Now.ToString();
                                        shippingUploadObj.SCD_TITLE = txtTitle.Text.Trim();
                                        //shippingUploadObj.SCD_DESC = txtDescription.Text.Trim();
                                        shippingUploadObj.SCD_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                                        if (fupUpload.HasFile)
                                        {
                                            FileInfo tempFileInfoObj;
                                            //string SavePath = string.Empty;
                                            //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                            //{
                                            //    SavePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                            //}
                                            //else
                                            //{
                                            //    //SavePath = Server.MapPath("../Upload");
                                            //    SavePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                                            //}
                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                            shippingUploadObj.AttachmentFileName = attachmentFileName;
                                            shippingUploadObj.FileExtension = tempFileInfoObj.Extension;
                                            shippingUploadObj.SCD_FILE = fupUpload.FileName;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                shippingUploadObj.SCD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }
                                            //shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                            //shippingUploadObj.SCD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()+ "/"+ attachmentFileName;

                                            BusinessObject.Shipping.FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                            if (fileDetailsObj == null)
                                            {
                                                FileDetailsList.Add(new BusinessObject.Shipping.FileDetails() { SlNo = CurrSlNo, ShippingFile = HttpContext.Current.Request.Files[0] });
                                            }
                                            else
                                            {
                                                fileDetailsObj.ShippingFile = HttpContext.Current.Request.Files[0];
                                            }
                                        }
                                        shippingUploadObj.SCD_MOD_BY = currentUser.PKUser;
                                    }
                                }

                            }
                            else
                            {
                                if (fupUpload.HasFile)
                                {

                                    int slno = 1;
                                    if (ShippingUploadList == null || ShippingUploadList.Count == 0)
                                    {
                                        ShippingUploadList = new List<BusinessObject.Shipping.ShippingUploadsBO>();
                                        slno = 1;
                                    }
                                    else
                                    {
                                        slno = ShippingUploadList.Max(itm => itm.SCD_SL_NO);
                                        slno++;
                                    }
                                    if (FileDetailsList == null)
                                    {
                                        FileDetailsList = new List<BusinessObject.Shipping.FileDetails>();
                                    }

                                    int UploadTypeId = 0;
                                    int.TryParse(ddlType.SelectedValue, out UploadTypeId);

                                    shippingUploadObj = new ShippingUploadsBO();
                                    shippingUploadObj.SCD_PK = 0;
                                    shippingUploadObj.SCD_PLAN_HDR = ShippingPlanPK;
                                    shippingUploadObj.SCD_SL_NO = slno;
                                    shippingUploadObj.SCD_TYPE = Convert.ToByte(ShippingUploadsEnum.ContainerEvaluation);
                                    shippingUploadObj.SCD_ITEM = UploadTypeId;//Convert.ToInt32(ddlType.SelectedValue);
                                    shippingUploadObj.SCD_ITEM_Text = UploadTypeId > 0 ? ddlType.SelectedItem.Text : string.Empty;
                                    shippingUploadObj.SCD_DATE = DateTime.Now.ToString();
                                    shippingUploadObj.SCD_TITLE = txtTitle.Text.Trim();
                                    //shippingUploadObj.SCD_DESC = txtDescription.Text.Trim();
                                    shippingUploadObj.SCD_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                                    FileInfo tempFileInfoObj;
                                    //string SavePath = string.Empty;
                                    //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                    //{
                                    //    SavePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                    //}
                                    //else
                                    //{
                                    //    //SavePath = Server.MapPath("../Upload");
                                    //    SavePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                                    //}
                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                    string attachmentFileFormat = tempFileInfoObj.Extension;
                                    string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                    shippingUploadObj.AttachmentFileName = attachmentFileName;
                                    shippingUploadObj.FileExtension = tempFileInfoObj.Extension;
                                    shippingUploadObj.SCD_FILE = fupUpload.FileName;
                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                    {
                                        shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    }
                                    else
                                    {
                                        //SavePath = Server.MapPath("../Upload");
                                        //shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                        shippingUploadObj.SCD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                    }

                                    // shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    shippingUploadObj.SCD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    shippingUploadObj.SCD_MOD_BY = currentUser.PKUser;
                                    
                                    //While submitting container evaluation for first time getting action failed error with attachment. 
                                    //While doing second time the attachment is getting saved. 
                                    shippingUploadObj.WKF_PROCESS = Convert.ToInt16(hdfProcessID.Value);
                                    //Issue resolved by adding WKF_PROCESS (Bug ID:36568)
                                    
                                    FileDetailsList.Add(new BusinessObject.Shipping.FileDetails() { SlNo = slno, ShippingFile = HttpContext.Current.Request.Files[0] });
                                    ShippingUploadList.Add(shippingUploadObj);

                                }
                            }
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ResetForm(ActionsEnum.ADDITEM);
                        }
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        hdfChangeFocus.Value = "1";
                        if (ShippingUploadList != null && ShippingUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                shippingUploadObj = ShippingUploadList.SingleOrDefault(row => selectedItemPK == row.SCD_SL_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        hdfChangeFocus.Value = "1";
                        if (ShippingUploadList != null && ShippingUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                ShippingUploadList = ShippingUploadList.Where(row => selectedItemPK != row.SCD_SL_NO).ToList();
                                if (FileDetailsList != null)
                                {
                                    FileDetailsList = FileDetailsList.Where(fl => selectedItemPK != fl.SlNo).ToList();
                                }
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ActionsEnum.ADDITEM);
                            }
                        }
                        break;
                    #endregion


                }
            }
            catch (Exception ex)
            {
                //Process Exception and show error message
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
                ContainerEvaluationServiceClient = null;
                CommonServiceClient = null;
                admCheckListTrxHdrObj = null;
                admCheckListTrxDtlObj = null;
                SalContainerEvalHdrObj = null;
            }
        }
        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            int slno;
            try
            {
                if (((GridView)sender).ID == "grdUploads")
                {
                    if (EntryStatus == EntryStatus.VIEWMODE)
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                        {
                            //e.Row.Cells[4].Visible = false;
                            e.Row.Cells[5].Visible = false;
                            e.Row.Cells[6].Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        slno = Convert.ToInt32(grdUploads.DataKeys[e.Row.RowIndex][0]);
                        if (slno > 0)
                        {
                            e.Row.FindControl("fileView").Visible = FileDetailsList == null || FileDetailsList.Where(fle => fle.SlNo == slno).Count() == 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Process Exception and show error message
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
                //reset all objects
            }
        }
        private void SaveAttachments()
        {
            if (ShippingUploadList != null && ShippingUploadList.Count > 0)
            {
                ShippingUploadsBOHeader ShippingUploadHdrObj = new ShippingUploadsBOHeader();
                ShippingUploadHdrObj.ShippingUploadsBOList = ShippingUploadList;
                string saveXml = CommonFunctions.XmlSerialize<ShippingUploadsBOHeader>(ShippingUploadHdrObj);
                int result = BusinessLogic.Shipping.ShippingUploadsBL.SaveShippingUploads(saveXml);
                if (result >= 0) // Success ! re-initialize the page
                {
                    string savePath = string.Empty;
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                        if (!Directory.Exists(savePath))
                            Directory.CreateDirectory(savePath);
                        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                    }
                    else
                    {
                        savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                    }

                    foreach (ShippingUploadsBO obj in ShippingUploadList)
                    {
                        string filePath = savePath + obj.AttachmentFileName;
                        FileInfo attachedFileInfo = new FileInfo(filePath);
                        if (FileDetailsList != null)
                        {
                            BusinessObject.Shipping.FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.SCD_SL_NO);
                            if (fileDetailsObj != null)
                            {
                                fileDetailsObj.ShippingFile.SaveAs(attachedFileInfo.FullName);

                            }
                        }
                    }
                    ResetForm(ActionsEnum.SAVE);
                    GetFieldValues(ControlsEnum.UPLOADEDFILES);
                    SetFieldValues(ControlsEnum.UPLOADEDFILES);
                    //Show Save success message and reset Contract Entry
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);

                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    if (result == (int)DbSaveStatus.SQLERROR)
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                    {
                        litErrorMsg.Text = (Resources.PageNameRes.ContainerEvaluation) + " " + Resources.Messages.EditUsedByAnotherUser;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.CODEEXIST)
                    {
                        litErrorMsg.Text = (Resources.PageNameRes.ContainerEvaluation) + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                }
            }

        }

        private void ResetForm(ActionsEnum action)
        {
            switch (action)
            {
                case ActionsEnum.ADDITEM:
                    //ddlType.ClearSelection();
                    txtTitle.Text = string.Empty;
                    //txtDescription.Text = string.Empty;
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
                case ActionsEnum.SAVE:
                    FileDetailsList = null;
                    ShippingUploadList = null;
                    ResetForm(ActionsEnum.ADDITEM);
                    break;
            }
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Method for Dropdownlist binding
        /// </summary>
        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.COMPANY:
                        break;
                    case ControlsEnum.COMPANYLIST:
                        ddlCompany.Items.Clear();
                        if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                        {
                            ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                            ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                            ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                            ddlCompany.DataBind();
                        }
                        break;

                    #region Upload Type
                    case ControlsEnum.UPLOADTYPE:
                        ddlType.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                            ddlType.DataTextField = "CON_NAME";
                            ddlType.DataValueField = "CON_PK";
                            ddlType.DataBind();
                        }
                        //ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        dtPageData = null;
                        //if (ddlType.Items.Count > 1)
                        //{
                        //    ddlType.SelectedValue = ddlType.Items[1].Value;
                        //}

                        break;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Bind Carrier dropdown
        /// </summary>
        /// <returns></returns>    
        private void BindCarrierDropDown()
        {
            ddlCarrier.Items.Clear();
            if (admConstMstList != null && admConstMstList.Count > 0)
            {
                ddlCarrier.DataSource = admConstMstList;
                ddlCarrier.DataTextField = "CON_NAME";
                ddlCarrier.DataValueField = "CON_PK";
                ddlCarrier.DataBind();
            }
            ddlCarrier.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
        }


        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private SAL_CONTAINER_EVAL_HDR SetUIValuesToObject(ActionsEnum Actions)
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                SalContainerEvalHdrObj.CVH_PK = CurrPK;
                SalContainerEvalHdrObj.CVH_NO = ContainerEvaluationNo;
                SalContainerEvalHdrObj.CVH_DATE = txtDate.Text.Trim() != string.Empty ? Convert.ToDateTime(txtDate.Text.Trim()) : DateTime.Now;
                SalContainerEvalHdrObj.CVH_SHIPPING_PLAN = ShippingPlanPK;
                SalContainerEvalHdrObj.CVH_IN_TIME = txtInTime.Text.Trim() != string.Empty ? Convert.ToDateTime(txtInTime.Text.Trim()) : DateTime.Now;

                if (hdfTransCompany.Value == string.Empty || hdfTransCompany.Value == "0" || hdfTransCompany.Value == "-1")
                {
                    SalContainerEvalHdrObj.CVH_TRANS_COMP = null;
                }
                else
                {
                    SalContainerEvalHdrObj.CVH_TRANS_COMP = Convert.ToInt32(hdfTransCompany.Value);
                }
                if (Convert.ToInt32(ddlCarrier.SelectedValue) >= 0)
                {
                    SalContainerEvalHdrObj.CVH_CARRIER = Convert.ToInt32(ddlCarrier.SelectedValue);
                }
                if (txtDate.Text.Trim() != string.Empty)
                {
                    SalContainerEvalHdrObj.CVH_BOOKING_DATE = Convert.ToDateTime(txtBookingDate.Text.Trim());
                }
                SalContainerEvalHdrObj.CVH_BOOKING_NO = HttpUtility.HtmlEncode(txtBookingNo.Text.Trim());
                SalContainerEvalHdrObj.CVH_CONTAINER_NO = HttpUtility.HtmlEncode(txtContainerNo.Text.Trim());
                //SalContainerEvalHdrObj.CVH_SERIAL_NO = HttpUtility.HtmlEncode(txtSerialNo.Text.Trim());
                SalContainerEvalHdrObj.CVH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                SalContainerEvalHdrObj.CVH_CHECK_LIST_GROUP = uclCheckList.CLGroup;
                SalContainerEvalHdrObj.CVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                SalContainerEvalHdrObj.CVH_BIZUNIT = currentUser.SBUID;
                SalContainerEvalHdrObj.CVH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                SalContainerEvalHdrObj.CVH_CRTD_DT = DateTime.Now;
                SalContainerEvalHdrObj.CVH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                SalContainerEvalHdrObj.CVH_MOD_DT = LastModifiedTime;
                SalContainerEvalHdrObj.CVH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                SalContainerEvalHdrObj.CVH_SEAL_NO = HttpUtility.HtmlEncode(txtSealNo.Text.Trim());
                switch (commonActions)
                {

                    case ActionsEnum.SAVE:
                        SalContainerEvalHdrObj.CVH_STATUS = (int)WorkFlowStatus.DRAFT;
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        SalContainerEvalHdrObj.CVH_STATUS = (int)WorkFlowStatus.APPROVED;
                        break;
                }
                return SalContainerEvalHdrObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SalContainerEvalHdrObj = null;
            }
        }

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        /// <param name="controlType">Controls to Bind</param>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.CONTAINEREVALUATION:
                        if (SalContainerEvalHdrList != null && SalContainerEvalHdrList.Count > 0)
                        {
                            CurrPK = SalContainerEvalHdrList[0].CVH_PK;
                            hdfStatus.Value = SalContainerEvalHdrList[0].CVH_STATUS.ToString();
                            lblEvaluationNoTxt.Text = ((SalContainerEvalHdrList[0].CVH_NO == null || SalContainerEvalHdrList[0].CVH_NO == "") ? Resources.Messages.DocGenerationNew : SalContainerEvalHdrList[0].CVH_NO);
                            hdfEvaluationNo.Value = (SalContainerEvalHdrList[0].CVH_NO == null ? string.Empty : SalContainerEvalHdrList[0].CVH_NO);
                            txtDate.Text = SalContainerEvalHdrList[0].CVH_DATE.ToString(Resources.ErpRes.DateFormatShort);
                            txtInTime.Text = Convert.ToDateTime(SalContainerEvalHdrList[0].CVH_IN_TIME).ToString(Resources.Constants.TimeFormatShort);
                            hdfTransCompany.Value = SalContainerEvalHdrList[0].CVH_TRANS_COMP.ToString();
                            if (SalContainerEvalHdrList[0].PUR_VENDOR_MST != null)
                            {
                                txtCompany.Text = SalContainerEvalHdrList[0].PUR_VENDOR_MST.VEN_NAME;
                            }
                            txtContainerNo.Text = txtContainerNo.ToolTip = HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].CVH_CONTAINER_NO);
                            lblContainerTypeTxt.Text = SalContainerEvalHdrList[0].SAL_SHIPPING_PLAN_HDR.SNH_CONTAINER_TYPE != null ? SalContainerEvalHdrList[0].SAL_SHIPPING_PLAN_HDR.ADM_CONST_MST.CON_NAME : string.Empty;
                            lblDestinationPortTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(SalContainerEvalHdrList[0].SAL_SHIPPING_PLAN_HDR.SNH_SHIP_TO_PORT, 45);
                            lblDestinationPortTxt.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(SalContainerEvalHdrList[0].SAL_SHIPPING_PLAN_HDR.SNH_SHIP_TO_PORT, 300);
                            //txtSerialNo.Text = HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].CVH_SERIAL_NO);
                            ddlCarrier.SelectedValue = SalContainerEvalHdrList[0].CVH_CARRIER != null ? SalContainerEvalHdrList[0].CVH_CARRIER.ToString() : CommonConstants.SELECTVAL;
                            txtBookingDate.Text = SalContainerEvalHdrList[0].CVH_BOOKING_DATE != null ? Convert.ToDateTime(SalContainerEvalHdrList[0].CVH_BOOKING_DATE.ToString()).ToString(Resources.ErpRes.DateFormatShort) : string.Empty;
                            txtBookingNo.Text = HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].CVH_BOOKING_NO);
                            txtRemarks.Text = HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].CVH_REMARKS);
                            uclCheckList.CLGroup = (int)SalContainerEvalHdrList[0].CVH_CHECK_LIST_GROUP;
                            ModifiedDatePnl.Visible = true;
                            LastModifiedTime = SalContainerEvalHdrList[0].CVH_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            //txtDate.Focus();
                            hdfDelstatus.Value = SalContainerEvalHdrList[0].SAL_SHIPPING_PLAN_HDR.SNH_DEL_STATUS.ToString();
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(SalContainerEvalHdrList[0].CVH_COMPANY.ToString()));
                            txtSealNo.Text = ((SalContainerEvalHdrList[0].CVH_SEAL_NO == null || SalContainerEvalHdrList[0].CVH_SEAL_NO == "") ? "" : HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].CVH_SEAL_NO));
                        }
                        else
                        {
                            EntryStatus = EntryStatus.NEWMODE;

                            txtDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);
                            txtBookingDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);


                            AST_DOC_MODE.Value = GetDOCMODE();

                            lblEvaluationNoTxt.Text = Resources.Messages.DocGenerationNew;

                            Session[ERP.Utilities.SessionStrings.RefID] = null;
                            Session[ERP.Utilities.SessionStrings.InboxFlag] = null;

                            txtBookingNo.Text = dsLoadingPlan.Tables[0].Rows[0][Resources.DataFieldRes.BookingRefNo].ToString();
                            //ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_COMPANY"].ToString()));
                            if (prevCompany != null && prevCompany != 0)
                            {
                                ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(prevCompany.ToString())));
                            }
                            // txtDate.Focus();
                            //Show Concurrency and bind Listing if Query yield no results
                            //litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                            //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            // + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                        }
                        break;
                    case ControlsEnum.SELECTEDDOC:
                        if (shippingUploadObj != null)
                        {
                            CurrSlNo = shippingUploadObj.SCD_SL_NO;
                            ddlType.SelectedValue = shippingUploadObj.SCD_ITEM.ToString();
                            //txtUploadDate.Text = shippingUploadObj.SCD_DATE;
                            txtTitle.Text = shippingUploadObj.SCD_TITLE;
                            //txtDescription.Text = shippingUploadObj.SCD_DESC;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = shippingUploadObj.SCD_FILE;
                            anchorFile.HRef = shippingUploadObj.SCD_FILE_PATH;


                            if (FileDetailsList != null && FileDetailsList.Where(fle => fle.SlNo == CurrSlNo).Count() > 0)
                            {
                                anchorFile.Attributes.Add("onclick", "return false;");
                                anchorFile.Attributes.Add("class", "removedownloadClass");
                            }
                            else
                            {
                                anchorFile.Attributes.Add("onclick", "return true;");
                                anchorFile.Attributes.Add("class", "downloadClass");
                            }

                        }
                        else
                        {
                            vrfFileUpload.Enabled = true;
                        }
                        break;
                }

            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.CNTEVAL, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        private int GetCheckListType()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.CNTEVAL, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return (int)AppTypeDetailsList[0].AST_CHECK_LIST_TYPE;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Method for Reset Controls
        /// </summary>

        private void ResetForm()
        {
            CurrPK = 0;
            uclCheckList.CLGroup = 0;
            Session[ERP.Utilities.SessionStrings.ContainerEvaluationPK] = null;
            Session[ERP.Utilities.SessionStrings.ContainerEvaluationMode] = null;
        }

        /// <summary>
        /// Set Tab Visibility
        /// </summary>
        private void SetTabVisibility()
        {
            GetFieldValues(ControlsEnum.SHIPPINGPLANLEVEL);
            if (dsShippingPlanHDR != null && dsShippingPlanHDR.Tables.Count > 0 && dsShippingPlanHDR.Tables[0].Rows.Count > 0)
            {
                tabLevel = Convert.ToInt32(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_TRX_STATUS"]);
                lblContainerTypeTxt.Text = dsShippingPlanHDR.Tables[0].Rows[0]["SNH_CONTAINER_TYPE_TEXT"].ToString();
                lblDestinationPortTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_SHIP_TO_PORT"].ToString(), 45);
                lblDestinationPortTxt.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_SHIP_TO_PORT"].ToString(), 300);
                hdfCompany.Value = dsShippingPlanHDR.Tables[0].Rows[0]["SNH_COMPANY"].ToString();
            }
            //spnShippingPlan.Visible = lnkShippingPlan.Visible = tabLevel >= (int)ShippingTabsEnum.ShippingPlan;
            spnContainerEval.Visible = lnkContainerEval.Visible = tabLevel >= (int)ShippingTabsEnum.PaymentCleared;
            spnContainerInspection.Visible = lnkContainerInspection.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerEvaluated;
            spnUploadQADocs.Visible = lnkUploadQADocs.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerInspected;
            spnUploadExportDocs.Visible = lnkUploadExportDocs.Visible = tabLevel >= (int)ShippingTabsEnum.QADocsUploaded;
            spnLoadingPlan.Visible = lnkLoadingPlan.Visible = tabLevel >= (int)ShippingTabsEnum.ExportDocsUploaded;
            spnUploadPhotographs.Visible = lnkUploadPhotographs.Visible = tabLevel >= (int)ShippingTabsEnum.LoadingPlanCompleted;
            spnDeliveryOrder.Visible = lnkDeliveryOrder.Visible = tabLevel >= (int)ShippingTabsEnum.PhotographsUploaded;
            spnContainerRelease.Visible = lnkContainerRelease.Visible = tabLevel >= (int)ShippingTabsEnum.DeliveryOrderCompleted;
            spnBillofLoading.Visible = lnkBillofLoading.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerReleased;
        }

        #endregion

        #region WorkFlow Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private int GetApplicationID(int refId)
        {
            int appId = 0;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtApplication = wrkfService.GetApplicationID(refId);
            if (dtApplication != null)
            {
                if (dtApplication.Rows.Count > 0)
                {
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID()
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                //((HiddenField)this.Master.FindControl("hdfPageID")).Value = dtProcess.Rows[0][CommonConstants.F_PAGE].ToString();
            }
        }

        #endregion

        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkShippingPlan.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerEval.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerInspection.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadQADocs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadExportDocs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkLoadingPlan.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadPhotographs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerRelease.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkBillofLoading.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkPrintShippingDocs.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnDelete.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.lnkShippingPlan.Load += new EventHandler(btnAction_Load);
            this.lnkContainerEval.Load += new EventHandler(btnAction_Load);
            this.lnkContainerInspection.Load += new EventHandler(btnAction_Load);
            this.lnkUploadQADocs.Load += new EventHandler(btnAction_Load);
            this.lnkUploadExportDocs.Load += new EventHandler(btnAction_Load);
            this.lnkLoadingPlan.Load += new EventHandler(btnAction_Load);
            this.lnkUploadPhotographs.Load += new EventHandler(btnAction_Load);
            this.lnkDeliveryOrder.Load += new EventHandler(btnAction_Load);
            this.lnkContainerRelease.Load += new EventHandler(btnAction_Load);
            this.lnkBillofLoading.Load += new EventHandler(btnAction_Load);
            this.lnkPrintShippingDocs.Load += new EventHandler(btnAction_Load);
            // uclPaging.CurrentPage = 1;
        }

        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            //this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            //this.Init += new EventHandler(this.Page_Init);
        }

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();

        }
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDateComponents();});", true);
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideForView", "$(document).ready(function(){HideForView();});", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
                if (grdUploads.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowAttachmentDetails", "$(document).ready(function(){ShowHideAttachmentDetails(1);});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowAttachmentDetails", "$(document).ready(function(){ShowHideAttachmentDetails();});", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }










        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }

        #endregion


        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            CONTAINEREVALUATION,
            TYPE,
            CONTROL,
            GROUP,
            CHECKLISTDATA,
            CONTAINEREVALUATIONNO,
            COMPANY,
            SHIPPINGPLANLEVEL,
            CARRIER,
            COMPANYLIST,
            PREVCOMPANY,
            UPLOADEDFILES,
            ADDITEM,
            SELECTEDDOC,
            UPLOADTYPE
        }

        /// <summary>
        /// Define Controltype Enum
        /// </summary>
        enum ControlTypes
        {
            Page,
            Label,
            Text,
            DropDown,
            DateTime,
            Numeric,
            Button,
            Spacer,
            GridView,
            CheckBox,
            TextArea,
            TimePicker,
            Header,
            Table,
            Iframe,
            HiddenField,
            HourText,
            FileUpload,
            Date,
            LinkButton,
            ImageButton,
            ValidationSummary,
            DateRange

        }

        #endregion
    }
}

