using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using ERPData;
using ERPService;
using ERP.Utilities;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using ERPManager;
using BusinessObject;
using BusinessObject.CommonManagement;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.Data.Objects;
using System.IO;
using System.Xml.Serialization;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.OrderToCash
{
    public partial class CustomerRegistration : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        #region Properties
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.ENTRYMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);

            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }
        /// <summary>
        /// Current PK-- Used to keep the selected customers PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }

        private string prefID
        {
            get
            {
                return this.ViewState["prefID"]!=null ? this.ViewState["prefID"].ToString() : string.Empty;
            }
            set
            {
                this.ViewState["prefID"] = value;
            }
        }

        /// <summary>
        /// Used to keep Brand Item insert for COV
        /// </summary>
        private BrandItemInsertBO objBrandItemInsert
        {
            get
            {
                return ViewState[ViewstateStrings.BrandItemInsert] == null ? new BrandItemInsertBO() : (BrandItemInsertBO)ViewState[ViewstateStrings.BrandItemInsert];
            }
            set
            {
                ViewState[ViewstateStrings.BrandItemInsert] = value;
            }
        }

        /// <summary>
        /// SelectedPK PK-- Used to keep the selected pk from a grid
        /// </summary>
        private int SelectedPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPK] = value;
            }
        }
        /// <summary>
        /// SelectedParentPK PK--Used to keep the selected pk from a parent grid
        /// </summary>
        private int SelectedParentPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedParentPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedParentPK] = value;
            }
        }
        /// <summary>
        /// SelectedParentGrid--Used to keep the selected grid name of the parent grid
        /// </summary>
        private string SelectedParentGrid
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedParentGrid] == null ? string.Empty : (this.ViewState[ViewstateStrings.SelectedParentGrid]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedParentGrid] = value;
            }
        }
        /// <summary>
        /// ControlPK--used to keep the PK of a control
        /// </summary>
        private int ControlPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.ControlPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ControlPK] = value;
            }
        }
        /// <summary>
        /// EntityName--used to keep the name of an Entity
        /// </summary>
        private string EntityName
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntityName] == null ? string.Empty : (this.ViewState[ViewstateStrings.EntityName]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.EntityName] = value;
            }
        }
        /// <summary>
        /// RelatedControlID--Used to store the RelatedControlId of a control
        /// </summary>
        private string RelatedControlID
        {
            get
            {
                return this.ViewState[ViewstateStrings.RelatedControlID] == null ? string.Empty : (this.ViewState[ViewstateStrings.RelatedControlID]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.RelatedControlID] = value;
            }
        }
        /// <summary>
        /// ChildGridName-- used to store the name of a child grid
        /// </summary>
        private string ChildGridName
        {
            get
            {
                return (this.ViewState[ViewstateStrings.ChildGridName]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.ChildGridName] = value;
            }
        }
        /// <summary>
        /// TabCode--Used to store the selected tab code
        /// </summary>
        private string TabCode
        {
            get
            {
                return (this.ViewState[ViewstateStrings.TabCode]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.TabCode] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndex
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
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
        /// To maintain Attachment File Name
        /// </summary>
        private string AttachmentFileName
        {
            get
            {
                return Convert.ToString(this.ViewState[ViewstateStrings.AttachmentFileName]);
            }
            set
            {
                this.ViewState[ViewstateStrings.AttachmentFileName] = value;
            }
        }

        private List<CRM_CUST_TAX_DTL> CrmCustTaxDetails
        {
            get
            {
                return (List<CRM_CUST_TAX_DTL>)this.ViewState[ERP.Utilities.SessionStrings.CrmCustTaxDetails];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.CrmCustTaxDetails] = value;
            }
        }

        private List<CRM_CUST_TAX_DTL> CrmCustTaxDetailsTemp
        {
            get
            {
                return (List<CRM_CUST_TAX_DTL>)this.ViewState[ERP.Utilities.SessionStrings.CrmCustTaxDetailsTemp];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.CrmCustTaxDetailsTemp] = value;
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

        /// <summary>
        /// Brand Item Insert for COV
        /// </summary>
        private bool IsBrandItemInsert
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsBrandItemInsert] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsBrandItemInsert].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsBrandItemInsert] = value;
            }
        }

        #endregion
        User currentUser;
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private CRM_CUSTOMER_MST crmCustomerMstObj;
        ServiceUtility serviceUtilityObj;

        private List<SPADM_FORM_TAB_CONTROL_CFG_GET_Result> spAdmFormTabControlCfgGetResultList;
        private List<SPADM_FORM_TAB_CFG_GET_Result> spAdmFormTabCfgGetResultList;
        private List<CRM_CUSTOMER_MST> crmCustomerMstList;
        private List<ADM_FORM_TAB_CONTROL_DTL> admFormTabControlDtlList;
        private List<CRM_CUST_TAX_DTL> crmCustTaxDtlList;
        CustomerRegistrationService customerRegistrationServiceClient;
        private const string CustomerStatusFilterControlId = "DDL_CUSTOMER_STATUS_FILTER";
        private const string CustomerStatusFilterSessionKey = "CustomerListActiveStatus";
        private const string CustomerListGridControlId = "GRD_LST";
        BrandItemInsertBO BrandItemInsertObj;
        string retVal;
        Object retEntityObj = null;
        bool doSave = false;
        #region Seperate File Upload Table- Now it doesn't use
        //private CRM_CUST_DOCUMENT_DTL crmCustDocumentDtlObj;
        //Dictionary<string, string> dicFileDetails;
        //Dictionary<string, int> dicFileParentPK;
        #endregion
        private string refID;
        private string inboxFlag;

        private DataTable dtTaxDetails;
        private DataTable dtPackingSpec;
        int packingSpecPk = 0;
        DropDownList ddlPackingSpecCtrl;
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            int preferenceID;
            int appId;
            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                ucrSearchProducts.SELECT += new EventHandler(ActionHandler);
                if (!IsPostBack)
                {
                    //Used for Integration purpose
                    ConfigurationSettings();
                    FillProcessID();
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                      : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
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
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        ucrWrkf.RefID = int.Parse(refID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);

                        if (CurrPK > 0)
                        {
                            //Get the Customer Master Details
                            customerRegistrationServiceClient = new CustomerRegistrationService();
                            customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                            GetFieldValues(ControlsEnum.CUSTOMER);
                            if (crmCustomerMstList != null && crmCustomerMstList.Count == 1)
                            {
                                //Set Session value (only in the case of inbox)
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = CurrPK;
                                //Edit Mode
                                crmCustomerMstObj = crmCustomerMstList[0];
                            }
                            else
                            {
                                //New Mode
                                crmCustomerMstObj = new CRM_CUSTOMER_MST();
                            }
                            TabCode = Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString();
                            switch (TabCode)
                            {
                                case TabType.CLST://List tab
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = null;
                                    break;
                                case TabType.CUS://Basic Details tab
                                    GetUIValuesFromObject(ActionsEnum.FILL_CUSTOMER, crmCustomerMstObj);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(prefID))
                        {
                            preferenceID = int.Parse(prefID);
                            appId = GetApplicationID(preferenceID);
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = appId;
                        }
                        if (Session[ERP.Utilities.SessionStrings.CUSTOMERPK] != null || Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK] != null)
                        {
                            if (Session[ERP.Utilities.SessionStrings.CUSTOMERPK] != null)
                                CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CUSTOMERPK].ToString());//Main Entity PK
                            else if (Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK] != null)//Customer Portal User. This session will set at the time of login
                                CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK].ToString());//Main Entity PK
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted)//If the workflow is completed, then also the user who have the permmission for approval can do any operations.
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                EntryStatus = EntryStatus.VIEWMODE;
                            }
                            if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                            {
                                GetFieldValues(ControlsEnum.DYNAMICTABS);
                                if (spAdmFormTabCfgGetResultList != null && spAdmFormTabCfgGetResultList.Count > 0)
                                {
                                    TabCode = Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString();
                                    if (CurrPK > 0)
                                    {
                                        //Get the Customer Master Details
                                        customerRegistrationServiceClient = new CustomerRegistrationService();
                                        customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                        GetFieldValues(ControlsEnum.CUSTOMER);
                                    }
                                    if (crmCustomerMstList != null && crmCustomerMstList.Count == 1)
                                    {
                                        //Edit Mode
                                        crmCustomerMstObj = crmCustomerMstList[0];
                                        if (crmCustomerMstObj.CUS_STATUS == 3)
                                        {
                                            ucrWrkf.ViewType = 0;
                                        }
                                    }
                                    else
                                    {
                                        //New Mode
                                        crmCustomerMstObj = new CRM_CUSTOMER_MST();
                                    }
                                    switch (TabCode)
                                    {
                                        case TabType.CLST://List tab
                                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = null;
                                            break;
                                        case TabType.CUS://Basic Details tab
                                            GetUIValuesFromObject(ActionsEnum.FILL_CUSTOMER, crmCustomerMstObj);
                                            break;
                                        default:
                                            break;
                                    }

                                }
                            }
                        }
                        else
                        {
                            Session[ERP.Utilities.SessionStrings.RefID] = null;
                            Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                customerRegistrationServiceClient = null;
            }
        }
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

        /// <summary>
        /// Get the User Rights, Checks Page Level Rights, 
        /// Hides sections in which user don't have access rights
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!IsPostBack)
            {
                //Set breadcrumb
                if ((WebControl)(Page.Form.FindControl("MainContent").FindControl("lblBreadCrum")) != null)
                {
                    string breadCrumb = string.Format(GetLocalResourceObject("Breadcrumb").ToString(), Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTEDTAB_NAME].ToString());
                    breadCrumb = breadCrumb.Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                    ((Label)(Page.Form.FindControl("MainContent").FindControl("lblBreadCrum"))).Text = breadCrumb;
                }

            }
        }
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        /// 
        private void GetFieldValues(ControlsEnum type)
        {
            //Services
            DynamicPageService DynamicPageServiceClient;
            DynamicPageServiceClient = null;
            try
            {
                switch (type)
                {
                    #region Default
                    case ControlsEnum.DEFAULT:
                        //Get UI controls
                        DynamicPageServiceClient = new DynamicPageService();
                        DynamicPageServiceClient = CommonFunctions.InitiateClient(DynamicPageServiceClient);
                        string tabCode = Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString();
                        spAdmFormTabControlCfgGetResultList = DynamicPageServiceClient.GetFormTabControlsList(FormType.CUS, tabCode, currentUser.PKUser, currentUser.SBUID);
                        break;
                    #endregion
                    #region Dynamic Tabs
                    case ControlsEnum.DYNAMICTABS:
                        //Get UI tabs
                        DynamicPageServiceClient = new DynamicPageService();
                        DynamicPageServiceClient = CommonFunctions.InitiateClient(DynamicPageServiceClient);
                        spAdmFormTabCfgGetResultList = DynamicPageServiceClient.GetFormTabList(FormType.CUS);
                        break;
                    #endregion
                    #region Customer
                    case ControlsEnum.CUSTOMER:
                        //Get customer master details
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        crmCustomerMstObj = ERP.Utilities.CommonFunctions.Initilize<CRM_CUSTOMER_MST>();
                        crmCustomerMstObj.CUS_PK = CurrPK;
                        crmCustomerMstObj.CUS_ACTIVE = TabCode == TabType.CLST
                            ? GetCustomerListActiveStatus()
                            : string.IsNullOrEmpty(prefID) ? Convert.ToByte(DbActiveStatus.ACTIVE) : Convert.ToByte(DbActiveStatus.INACTIVE);
                 
                        if (IsSBUCustomer())
                        {
                            crmCustomerMstObj.CUS_BIZUNIT = currentUser.SBUID;
                            serviceUtilityObj.IsSBUSpecific = true;
                        }
                        crmCustomerMstList = customerRegistrationServiceClient.GetCrmCustomerMst(crmCustomerMstObj, serviceUtilityObj);
                        
                        break;
                    #endregion
                    #region Tax Popup
                    case ControlsEnum.TAXTYPES:
                        int _category = Convert.ToInt32(TaxType.Tax);
                        int _subCategory = 0; // Convert.ToInt32(TaxSubCategory.VATSale);
                        int _isTaxSale = 1;
                        dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster
                                .GetTaxTypes(_category, _subCategory, currentUser.SBUID, Convert.ToInt32(CommonConstants.ACTIVE), _isTaxSale);
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        if (this.CrmCustTaxDetails != null)
                        {
                            CRM_CUST_TAX_DTL[] tempArry = CrmCustTaxDetails.ToArray();
                            CrmCustTaxDetailsTemp = tempArry.ToList();
                        }
                        else
                        {
                            customerRegistrationServiceClient = new CustomerRegistrationService();
                            customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                            GetFieldValues(ControlsEnum.CUSTOMER);
                            var tmp = crmCustomerMstList.FirstOrDefault(x => x.CUS_PK == CurrPK);
                            if (tmp != null)
                            {
                                CrmCustTaxDetails = this.TabCode == TabType.CUS
                                                    ? tmp.CRM_CUST_TAX_DTL.ToList()
                                                    : tmp.CRM_CUST_ITEM_MAP.Any(x => x.CIM_PK == this.SelectedPK)
                                                        ? tmp.CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK).CRM_CUST_TAX_DTL.ToList()
                                                        : new List<CRM_CUST_TAX_DTL>();
                                CRM_CUST_TAX_DTL[] tempArry = CrmCustTaxDetails.ToArray();
                                CrmCustTaxDetailsTemp = tempArry.ToList();
                            }
                            else
                            {
                                CrmCustTaxDetails = new List<CRM_CUST_TAX_DTL>();
                                CRM_CUST_TAX_DTL[] tempArry = CrmCustTaxDetails.ToArray();
                                CrmCustTaxDetailsTemp = tempArry.ToList();
                            }
                        }
                        break;
                    #endregion
                    #region Packing Spec
                    case ControlsEnum.PACKINGSPEC:
                        dtPackingSpec = BusinessLogic.Inventory.PackingMasterBL.GetPackingMaster(Convert.ToInt16(CommonConstants.SELECT_VALUE_ZERO), Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, string.Empty, string.Empty, string.Empty).Tables[0];
                        break;
                        #endregion
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                DynamicPageServiceClient = null;
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
                    case ControlsEnum.TAXPOPUPGRID://Biju
                        GetFieldValues(ControlsEnum.TAXPOPUPGRID);
                        ddlPopupTaxType.Focus();
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.TAXPOPUPGRIDTEMP:
                        ddlPopupTaxType.Focus();
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.TAXTYPES:
                        GetFieldValues(ControlsEnum.TAXTYPES);
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.TAXPOPUP:
                        SetFieldValues(ControlsEnum.TAXTYPES);
                        SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                        break;
                    #region Packing Spec
                    case ControlsEnum.PACKINGSPEC:
                        BindDropDown(controlType);
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!sender.GetType().IsEquivalentTo(typeof(GridView)))//'OnPreRender' event registered in Gridview, so every action on page calling Action Handler
                if (!(this.Master as ERPSMS_2).ValidatePageDept())
                    return;
            long result;
            result = 0;
            HiddenField hdfGrid;
            Dictionary<int, string> dictionary;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            CommonService commonServiceClient;
            commonServiceClient = null;
            bool rbtnChecked;
            string message = string.Empty;
            DropDownList ddlWkfAction;
            string action;
            string spName;
            object[] methodParams;
            Object retObj;
            int valid;
            valid = 1;
            string[] paramControls;
            int count;
            HiddenField hdfValdSP;
            HiddenField hdfValdSPParm;
            bool IsRefExist = false;
            hdfIsNew.Value = "0";
            try
            {
                //Get Action from CommandName
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                {
                    string tabCode = Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString();
                    switch (commonActions)
                    {
                        #region General
                        #region Save
                        case ActionsEnum.SAVE:
                            //Save Details
                            if (!IsValid)
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else//valid
                            {
                                //create new instance of service. This object should be maintain until all the save operation is completed. This is for maintaining the entity object context
                                customerRegistrationServiceClient = new CustomerRegistrationService();
                                customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                //Get the customer master details
                                GetFieldValues(ControlsEnum.CUSTOMER);
                                if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                {
                                    //Get the entity name of the save button. The details will be saved to this entity
                                    HiddenField hdfEntity = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Entity");
                                    if (hdfEntity != null && !string.IsNullOrEmpty(hdfEntity.Value))
                                    {
                                        string entityName = hdfEntity.Value;
                                        if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString())) //If the entity is customer Master
                                        #region Seperate File Upload Table- Now it doesn't use
                                        //|| entityName.Equals("CRM_CUSTOMER_MST.CRM_CUST_DOCUMENT_DTL"))
                                        #endregion
                                        {
                                            if (CurrPK > 0)
                                            {
                                                //Get customer master list
                                                GetFieldValues(ControlsEnum.CUSTOMER);
                                            }
                                            if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                            {
                                                //set the customer master object
                                                crmCustomerMstObj = crmCustomerMstList[0];
                                                #region Seperate File Upload Table- Now it doesn't use
                                                //CRM_CUST_DOCUMENT_DTL is the table to keep the files uploaded in customer registration section. But now we dont use this tale
                                                //crmCustDocumentDtlObj = crmCustomerMstObj.CRM_CUST_DOCUMENT_DTL.FirstOrDefault(ss => ss.CDD_CUSTOMER == crmCustomerMstObj.CUS_PK && ss.CDD_ART_WORK == null);
                                                //if (crmCustDocumentDtlObj == null)
                                                //{
                                                //    crmCustDocumentDtlObj = new CRM_CUST_DOCUMENT_DTL();
                                                //}
                                                #endregion
                                            }
                                            else
                                            {
                                                //create new instance of customer master 
                                                crmCustomerMstObj = new CRM_CUSTOMER_MST();
                                                crmCustomerMstList = new List<CRM_CUSTOMER_MST>();
                                                #region Seperate File Upload Table- Now it doesn't use
                                                //crmCustDocumentDtlObj = new CRM_CUST_DOCUMENT_DTL();
                                                #endregion

                                            }
                                            //Get values from UI controls to entity
                                            crmCustomerMstObj = (CRM_CUSTOMER_MST)SetUIValuesToObject(ActionsEnum.SAVE, crmCustomerMstObj);

                                            #region Seperate File Upload Table- Now it doesn't use
                                            //if (dicFileDetails != null && dicFileDetails.Count > 0)
                                            //{
                                            //    crmCustDocumentDtlObj = (CRM_CUST_DOCUMENT_DTL)SetUIValuesToObject(ActionsEnum.SAVE, crmCustDocumentDtlObj);
                                            //    crmCustomerMstObj.CRM_CUST_DOCUMENT_DTL.Add(crmCustDocumentDtlObj);
                                            //}
                                            #endregion
                                            if (CurrPK == 0)
                                            {
                                                #region Add Tax Details
                                                if (this.CrmCustTaxDetails != null && CrmCustTaxDetails.Count > 0)
                                                {
                                                    crmCustomerMstObj.CRM_CUST_TAX_DTL = new System.Data.Objects.DataClasses.EntityCollection<CRM_CUST_TAX_DTL>();
                                                    CrmCustTaxDetails.ForEach(dtl =>
                                                    {
                                                        crmCustomerMstObj.CRM_CUST_TAX_DTL.Add(dtl);
                                                    });
                                                }

                                                #endregion
                                                crmCustomerMstList.Add(crmCustomerMstObj);
                                            }
                                            else
                                            {
                                                #region Add Tax Details

                                                if (this.CrmCustTaxDetails != null && CrmCustTaxDetails.Count > 0)
                                                {
                                                    var crmCustItemMapObj = crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK);
                                                    long[] idArry = CrmCustTaxDetails.Select(x => x.CMT_PK).ToArray();

                                                    if (crmCustItemMapObj.CRM_CUST_TAX_DTL != null)
                                                    {
                                                        foreach (CRM_CUST_TAX_DTL dtl in crmCustItemMapObj.CRM_CUST_TAX_DTL.Where(x => !idArry.Contains(x.CMT_PK)).ToList())
                                                        {
                                                            crmCustItemMapObj.CRM_CUST_TAX_DTL.Remove(dtl);
                                                        }
                                                    }

                                                    foreach (CRM_CUST_TAX_DTL dtl in this.CrmCustTaxDetails)
                                                    {
                                                        if (dtl.CMT_PK == 0)
                                                        {
                                                            crmCustItemMapObj.CRM_CUST_TAX_DTL.Add(dtl);
                                                        }
                                                        else if (!idArry.Contains(dtl.CMT_PK))
                                                        {
                                                            crmCustItemMapObj.CRM_CUST_TAX_DTL.Remove(dtl);
                                                        }
                                                    }
                                                }
                                                else if (this.CrmCustTaxDetails != null
                                                    && crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK)
                                                                .CRM_CUST_TAX_DTL.Any())
                                                {
                                                    crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK).CRM_CUST_TAX_DTL.Clear();
                                                }

                                                #endregion
                                            }
                                            //call validation SP if any
                                            hdfValdSP = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "VALD_SP");
                                            if (hdfValdSP != null && !string.IsNullOrEmpty(hdfValdSP.Value))
                                            {
                                                #region Validation Chk
                                                hdfValdSPParm = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "VALD_SP_PARAM");
                                                if (hdfValdSPParm != null && !string.IsNullOrEmpty(hdfValdSPParm.Value))
                                                {
                                                    //Get the sp name
                                                    spName = hdfValdSP.Value;
                                                    // set the parameters of the sp
                                                    paramControls = hdfValdSPParm.Value.Split(',');

                                                    if (paramControls.Length > 0)//sp have paremeters
                                                    {
                                                        methodParams = new object[paramControls.Length + 2];//length+2 is because one is for storing currentpk and another is for bizunit
                                                        methodParams[0] = CurrPK;//set current pk as the first parameter
                                                        count = 1;
                                                        foreach (string controlId in paramControls)
                                                        {
                                                            if ((pnlControls.FindControl(controlId)) != null)
                                                            {
                                                                string controlType = pnlControls.FindControl(controlId).GetType().Name;
                                                                if (controlType.Equals("TextBox"))
                                                                {
                                                                    TextBox txtBox = (TextBox)pnlControls.FindControl(controlId);
                                                                    if (txtBox != null && !string.IsNullOrEmpty(txtBox.Text.Trim()))
                                                                    {
                                                                        methodParams[count] = txtBox.Text.Trim();//set the parameter value
                                                                    }
                                                                    else
                                                                    {
                                                                        methodParams[count] = null;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    methodParams[count] = null;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                methodParams[count] = null;
                                                            }
                                                            count++;
                                                        }
                                                        methodParams[count] = currentUser.SBUID;//set the bizunit value
                                                        if (methodParams.Count() > 2)//have parametrs other than currpk and bizunit
                                                        {
                                                            commonServiceClient = new CommonService();
                                                            commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                                                            retObj = commonServiceClient.ExecuteSP(spName, methodParams);//execute sp
                                                            if (retObj != null)
                                                            {
                                                                string retVal = string.Empty;
                                                                string errorMsg = string.Empty;
                                                                ObjectResult objResult = (ObjectResult)retObj;
                                                                foreach (Object srcObj in objResult)
                                                                {
                                                                    Type targetTable = srcObj.GetType();
                                                                    foreach (PropertyInfo p in targetTable.GetProperties())
                                                                    {
                                                                        if (p.Name.Equals("RET_VAL"))
                                                                        {
                                                                            retVal = p.GetValue(srcObj, null).ToString();
                                                                        }
                                                                        else if (p.Name.Equals("RET_TEXT"))
                                                                        {
                                                                            errorMsg = p.GetValue(srcObj, null).ToString();
                                                                        }
                                                                    }
                                                                    break;
                                                                }
                                                                if (!string.IsNullOrEmpty(retVal))
                                                                {
                                                                    if (retVal.Equals("0"))//error
                                                                    {
                                                                        valid = 0;
                                                                        litErrorMsg.Text = GetLocalResourceObject("msg_Error").ToString();
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                    }
                                                                    else if (retVal.Equals("-1"))//error
                                                                    {
                                                                        valid = 0;
                                                                        litErrorMsg.Text = errorMsg;
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                    }
                                                                    else if (retVal.Equals("1"))//success
                                                                    {
                                                                        valid = 1;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            if (valid == 1)
                                            {
                                                #region Save customer master operation
                                                result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                                if (result >= 0) // Success ! re-initialize the page
                                                {
                                                    ResetTaxPopUpSessionData();
                                                    litErrorMsg.Text = Resources.Report.Msg_Save_Success;
                                                    //set dynamic message
                                                    message = CustomerRegistrationTabs.DynamicTabDesc;
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), message);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode) + "');", true);
                                                }
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            doSave = false;
                                            #region Seperate File Upload Table- Now it doesn't use
                                            //test start
                                            //dicFileParentPK = new Dictionary<string, int>();
                                            //
                                            #endregion
                                            //Set the UI values to the curresponding entity. If this succesfully completed return true, otherwise return false
                                            SaveOrUpdateEntity(crmCustomerMstList[0], entityName);
                                            if (doSave)
                                            {
                                                //call validation SP if any
                                                hdfValdSP = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "VALD_SP");
                                                if (hdfValdSP != null && !string.IsNullOrEmpty(hdfValdSP.Value))
                                                {
                                                    #region Validation chk
                                                    hdfValdSPParm = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "VALD_SP_PARAM");
                                                    if (hdfValdSPParm != null && !string.IsNullOrEmpty(hdfValdSPParm.Value))
                                                    {
                                                        //Get the sp name
                                                        spName = hdfValdSP.Value;
                                                        // set the parameters of the sp
                                                        paramControls = hdfValdSPParm.Value.Split(',');
                                                        if (paramControls.Length > 0)
                                                        {
                                                            methodParams = new object[paramControls.Length + 1];//length+1 is because one is for currpk
                                                            methodParams[0] = CurrPK;
                                                            count = 1;
                                                            foreach (string controlId in paramControls)
                                                            {
                                                                if ((pnlControls.FindControl(controlId)) != null)
                                                                {
                                                                    string controlType = pnlControls.FindControl(controlId).GetType().Name;
                                                                    if (controlType.Equals("TextBox"))
                                                                    {
                                                                        TextBox txtBox = (TextBox)pnlControls.FindControl(controlId);
                                                                        if (txtBox != null && !string.IsNullOrEmpty(txtBox.Text.Trim()))
                                                                        {
                                                                            methodParams[count] = txtBox.Text.Trim();//assign value
                                                                        }
                                                                        else
                                                                        {
                                                                            methodParams[count] = null;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        methodParams[count] = null;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (controlId.EndsWith("PK"))
                                                                    {
                                                                        string[] entityNameSplit = entityName.Split('.');
                                                                        if (entityNameSplit.Length > 0)
                                                                        {
                                                                            if (Session[entityNameSplit[entityNameSplit.Length - 1]] != null)
                                                                            {
                                                                                methodParams[count] = Convert.ToInt32(Session[entityNameSplit[entityNameSplit.Length - 1]].ToString());
                                                                            }
                                                                            else
                                                                            {
                                                                                methodParams[count] = null;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            methodParams[count] = null;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        methodParams[count] = null;
                                                                    }
                                                                }
                                                                count++;
                                                            }
                                                            if (methodParams.Count() > 1)//have parametrs other than currpk
                                                            {
                                                                commonServiceClient = new CommonService();
                                                                commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                                                                retObj = commonServiceClient.ExecuteSP(spName, methodParams);//Execute SP
                                                                if (retObj != null)
                                                                {
                                                                    string retVal = string.Empty;
                                                                    string errorMsg = string.Empty;
                                                                    ObjectResult objResult = (ObjectResult)retObj;
                                                                    foreach (Object srcObj in objResult)
                                                                    {
                                                                        Type targetTable = srcObj.GetType();
                                                                        foreach (PropertyInfo p in targetTable.GetProperties())
                                                                        {
                                                                            if (p.Name.Equals("RET_VAL"))
                                                                            {
                                                                                retVal = p.GetValue(srcObj, null).ToString();
                                                                            }
                                                                            else if (p.Name.Equals("RET_TEXT"))
                                                                            {
                                                                                errorMsg = p.GetValue(srcObj, null).ToString();
                                                                            }
                                                                        }
                                                                        break;
                                                                    }
                                                                    if (!string.IsNullOrEmpty(retVal))
                                                                    {
                                                                        if (retVal.Equals("0"))//Error
                                                                        {
                                                                            valid = 0;
                                                                            litErrorMsg.Text = GetLocalResourceObject("msg_Error").ToString();
                                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                        }
                                                                        else if (retVal.Equals("-1"))//Failed
                                                                        {
                                                                            valid = 0;
                                                                            litErrorMsg.Text = errorMsg;
                                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                        }
                                                                        else if (retVal.Equals("1"))//Success
                                                                        {
                                                                            valid = 1;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                }

                                                if (valid == 1)
                                                {
                                                    //Save operation. For saving we will pass only the master entity, ie, Customer master entity. 
                                                    //It wil save/update/delete the whole subentities(only status changed ones). 
                                                    //If saving is succesfully completed return value will be the selected customer master pk                                                    
                                                    if (this.SelectedPK == 0)
                                                    {
                                                        #region Add Tax Details

                                                        if (this.CrmCustTaxDetails != null && CrmCustTaxDetails.Count > 0)
                                                        {
                                                            //var obj = crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK).CRM_CUST_TAX_DTL;
                                                            //obj = new System.Data.Objects.DataClasses.EntityCollection<CRM_CUST_TAX_DTL>();
                                                            //crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK).CRM_CUST_TAX_DTL = new System.Data.Objects.DataClasses.EntityCollection<CRM_CUST_TAX_DTL>();
                                                            CrmCustTaxDetails.ForEach(dtl =>
                                                            {
                                                                crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK).CRM_CUST_TAX_DTL.Add(dtl);
                                                            });
                                                        }

                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region Add Tax Details

                                                        if (this.CrmCustTaxDetails != null && CrmCustTaxDetails.Count > 0)
                                                        {
                                                            var crmCustItemMapObj = crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK);
                                                            long[] idArry = CrmCustTaxDetails.Select(x => x.CMT_PK).ToArray();

                                                            if (crmCustItemMapObj.CRM_CUST_TAX_DTL != null)
                                                            {
                                                                foreach (CRM_CUST_TAX_DTL dtl in crmCustItemMapObj.CRM_CUST_TAX_DTL.Where(x => !idArry.Contains(x.CMT_PK)).ToList())
                                                                {
                                                                    crmCustItemMapObj.CRM_CUST_TAX_DTL.Remove(dtl);
                                                                }
                                                            }

                                                            foreach (CRM_CUST_TAX_DTL dtl in this.CrmCustTaxDetails)
                                                            {
                                                                if (dtl.CMT_PK == 0)
                                                                {
                                                                    crmCustItemMapObj.CRM_CUST_TAX_DTL.Add(dtl);
                                                                }
                                                                else if (!idArry.Contains(dtl.CMT_PK))
                                                                {
                                                                    crmCustItemMapObj.CRM_CUST_TAX_DTL.Remove(dtl);
                                                                }
                                                            }
                                                        }
                                                        else if (this.CrmCustTaxDetails != null
                                                            && crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK)
                                                                        .CRM_CUST_TAX_DTL.Any())
                                                        {
                                                            crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK).CRM_CUST_TAX_DTL.Clear();
                                                        }

                                                        #endregion
                                                    }

                                                    if (IsBrandItemInsert && objBrandItemInsert.CIM_BRAND_CODE != null && objBrandItemInsert.CIM_BRAND_NAME != null)
                                                    {

                                                        CustomerInvItem custItem = new CustomerInvItem()
                                                        {
                                                            BIZUNIT = currentUser.SBUID,
                                                            ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE),
                                                            CIM_CUSTOMER = objBrandItemInsert.CIM_CUSTOMER_PK,
                                                            CIM_BRAND_NAME = HttpUtility.HtmlEncode(objBrandItemInsert.CIM_BRAND_NAME),
                                                            CIM_BRAND_CODE = objBrandItemInsert.CIM_BRAND_CODE,
                                                            CIM_PK = 0,
                                                            USER_PK = currentUser.PKUser,
                                                            CIM_BIZUNIT = currentUser.SBUID
                                                        };
                                                        crmCustomerMstList[0].CRM_CUST_ITEM_MAP.ToList().ForEach(b =>
                                                        b.CIM_BIZUNIT = currentUser.SBUID);
                                                        result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList, custItem, true);

                                                    }
                                                    else
                                                    {
                                                        crmCustomerMstList[0].CRM_CUST_ITEM_MAP.ToList().ForEach(b =>
                                                       b.CIM_BIZUNIT = currentUser.SBUID);
                                                        result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                                    }
                                                    if (result >= 0) // Success ! re-initialize the page
                                                    {

                                                        ResetTaxPopUpSessionData();
                                                        litErrorMsg.Text = Resources.Report.Msg_Save_Success;
                                                        //set dynamic message
                                                        message = CustomerRegistrationTabs.DynamicTabDesc;
                                                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), message);
                                                        if (!hdfSubTabValue.Value.Equals("Dtl"))//check whether the tab have subtab. if yes, check current subtab is not details subtab
                                                        {
                                                            #region Clear Entity Session
                                                            //Clear the entity session
                                                            if (Session["EntityByGroup"] != null)
                                                            {
                                                                //Get the dictionary from session. The dictionary have order and entity name as it's key value pair
                                                                dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                                foreach (KeyValuePair<int, string> item in dictionary)
                                                                {
                                                                    //Split the entity name with '.', because sometimes the entity name may be a referenced entity seperated by '.'
                                                                    string[] stringArray = item.Value.Split('.');
                                                                    foreach (string sessionString in stringArray)
                                                                    {
                                                                        if (Session[sessionString] != null)
                                                                        {
                                                                            Session[sessionString] = null;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //Show save success message and redirect to the same page
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode) + "');", true);
                                                            #endregion
                                                        }
                                                        else//If the tab have subtab and subtab is details subtab
                                                        {
                                                            //Get the Group number of the save button.
                                                            HiddenField hdfGroup = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Group");
                                                            if (hdfGroup != null && !string.IsNullOrEmpty(hdfGroup.Value))
                                                            {
                                                                int intOut;
                                                                if (Int32.TryParse(hdfGroup.Value, out intOut))
                                                                {
                                                                    if (Session["EntityByGroup"] != null)
                                                                    {
                                                                        //Get the dictionary from the session
                                                                        dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                                        entityName = string.Empty;
                                                                        //Get the entity name of the group
                                                                        entityName = dictionary.FirstOrDefault(dic => dic.Key == Convert.ToInt32(hdfGroup.Value)).Value;
                                                                        string tempEntityName = entityName;
                                                                        if (!string.IsNullOrEmpty(entityName))
                                                                        {
                                                                            //Get customer Master Details
                                                                            GetFieldValues(ControlsEnum.CUSTOMER);
                                                                            if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                                                            {
                                                                                Object retEntity = null;
                                                                                //Split the entity name with '.'
                                                                                string[] entityNameArray = entityName.Split('.');
                                                                                if (entityNameArray.Length > 1)
                                                                                {
                                                                                    //clear the session with the child entityname
                                                                                    Session[entityNameArray[entityNameArray.Length - 1]] = null;
                                                                                    //set the entity name as the 1st parent of the child entity
                                                                                    entityName = entityNameArray[entityNameArray.Length - 2];
                                                                                }
                                                                                //Get the EntityCollection object from the entityname
                                                                                retEntity = GetEntityCollection(crmCustomerMstList[0], entityName);
                                                                                if (retEntity != null)
                                                                                {
                                                                                    IEnumerable entityEnumList = null;
                                                                                    //Get the Ienumerable list from the entityCollection
                                                                                    entityEnumList = (IEnumerable)retEntity;
                                                                                    if (entityEnumList != null)
                                                                                    {
                                                                                        foreach (Object entityobj in entityEnumList)
                                                                                        {
                                                                                            PropertyInfo propObj;
                                                                                            propObj = null;
                                                                                            //Get the property that ends with "PK"
                                                                                            propObj = entityobj.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                                                                                            if (propObj != null && propObj.GetValue(entityobj, null) != null)
                                                                                            {
                                                                                                //check the PK value is equal to the selectedparentpk(selected Parent gird PK)
                                                                                                if (Convert.ToInt32(propObj.GetValue(entityobj, null)) == SelectedParentPK)
                                                                                                {
                                                                                                    //Set the values from object to UI controls
                                                                                                    GetUIValuesFromObject(ActionsEnum.SAVE, entityobj);
                                                                                                    //split the entity name with '.'
                                                                                                    string[] entityNameArray1 = tempEntityName.Split('.');
                                                                                                    if (entityNameArray1.Length > 1)//If has a child entity 
                                                                                                    {
                                                                                                        //Create new instance for the child entity. It for clear the detail section only
                                                                                                        string namespaceString = "ERPData";
                                                                                                        string className = entityNameArray[entityNameArray.Length - 1];
                                                                                                        className = namespaceString + "." + className;
                                                                                                        Assembly currentAssembly = Assembly.Load(namespaceString);
                                                                                                        Type baseEntity = currentAssembly.GetType(className);
                                                                                                        Object childEntityObj = Activator.CreateInstance(baseEntity, null);
                                                                                                        //Set the values from object to UI controls. Here the values will be null. so the controls will be reset
                                                                                                        GetUIValuesFromObject(ActionsEnum.SAVE, childEntityObj);
                                                                                                    }
                                                                                                    if (!string.IsNullOrEmpty(RelatedControlID))
                                                                                                    {
                                                                                                        if (RelatedControlID.Equals(SelectedParentGrid))//Check RelatedControlID==SelectedParentGrid for bind the child grid
                                                                                                        {
                                                                                                            //Get the grid object from the child grid name
                                                                                                            GridView childGrid = (GridView)pnlControls.FindControl(ChildGridName);
                                                                                                            if (childGrid != null && ControlPK > 0 && !string.IsNullOrEmpty(EntityName))
                                                                                                            {
                                                                                                                //Bind grid . parameters is gridPK, Entityname(entity that to be bind to the grid),gridName
                                                                                                                BindGrid(ControlPK, EntityName, childGrid);
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //If it is in Edit Mode
                                                            if (SelectedParentPK > 0)
                                                            {
                                                                HideFileAnchorControlsIfAny();
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                            }
                                                            else//If New Mode
                                                            {
                                                                //clear subtab hidden field value
                                                                hdfSubTabValue.Value = string.Empty;
                                                                //Clear entity session
                                                                if (Session["EntityByGroup"] != null)
                                                                {
                                                                    //Get the dictionary from the session
                                                                    dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                                    foreach (KeyValuePair<int, string> item in dictionary)
                                                                    {
                                                                        string[] stringArray = item.Value.Split('.');
                                                                        foreach (string sessionString in stringArray)
                                                                        {
                                                                            if (Session[sessionString] != null)
                                                                            {
                                                                                Session[sessionString] = null;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //Redirect to the same tab
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode) + "');", true);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            break;
                        #endregion
                        #region CONTINUE
                        case ActionsEnum.CONTINUE:
                            //Save Details
                            if (!IsValid)
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else//valid
                            {
                                //create new instance of service. This object should be maintain until all the save operation is completed. This is for maintaining the entity object context
                                customerRegistrationServiceClient = new CustomerRegistrationService();
                                customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                //Get the entity name of the continue button. The details will be saved to this entity
                                HiddenField hdfEntity = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Entity");
                                if (hdfEntity != null && !string.IsNullOrEmpty(hdfEntity.Value))
                                {
                                    string entityName = hdfEntity.Value;
                                    if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString()))//If the entity is customer Master
                                    #region Seperate File Upload Table- Now it doesn't use
                                    //|| entityName.Equals("CRM_CUSTOMER_MST.CRM_CUST_DOCUMENT_DTL"))
                                    #endregion
                                    {
                                        if (CurrPK > 0)
                                        {
                                            //Get customer master list
                                            GetFieldValues(ControlsEnum.CUSTOMER);
                                        }
                                        if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                        {
                                            //set the customer master object
                                            crmCustomerMstObj = crmCustomerMstList[0];
                                            #region Seperate File Upload Table- Now it doesn't use
                                            //crmCustDocumentDtlObj = crmCustomerMstObj.CRM_CUST_DOCUMENT_DTL.FirstOrDefault(ss => ss.CDD_CUSTOMER == crmCustomerMstObj.CUS_PK && ss.CDD_ART_WORK == null);
                                            //if (crmCustDocumentDtlObj == null)
                                            //{
                                            //    crmCustDocumentDtlObj = new CRM_CUST_DOCUMENT_DTL();
                                            //}
                                            #endregion
                                        }
                                        else
                                        {
                                            //create new instance of customer master 
                                            crmCustomerMstObj = new CRM_CUSTOMER_MST();
                                            crmCustomerMstList = new List<CRM_CUSTOMER_MST>();
                                            #region Seperate File Upload Table- Now it doesn't use
                                            //crmCustDocumentDtlObj = new CRM_CUST_DOCUMENT_DTL();
                                            #endregion
                                        }
                                        //Get values from UI controls to entity
                                        crmCustomerMstObj = (CRM_CUSTOMER_MST)SetUIValuesToObject(ActionsEnum.SAVE, crmCustomerMstObj);
                                        #region Seperate File Upload Table- Now it doesn't use
                                        //if (dicFileDetails != null && dicFileDetails.Count > 0)
                                        //{
                                        //    crmCustDocumentDtlObj = (CRM_CUST_DOCUMENT_DTL)SetUIValuesToObject(ActionsEnum.SAVE, crmCustDocumentDtlObj);
                                        //    crmCustomerMstObj.CRM_CUST_DOCUMENT_DTL.Add(crmCustDocumentDtlObj);
                                        //}
                                        #endregion
                                        if (CurrPK == 0)
                                        {
                                            #region Add Tax Details
                                            if (this.CrmCustTaxDetails != null && CrmCustTaxDetails.Count > 0)
                                            {
                                                crmCustomerMstObj.CRM_CUST_TAX_DTL = new System.Data.Objects.DataClasses.EntityCollection<CRM_CUST_TAX_DTL>();
                                                CrmCustTaxDetails.ForEach(dtl =>
                                                {
                                                    crmCustomerMstObj.CRM_CUST_TAX_DTL.Add(dtl);
                                                });
                                            }
                                            #endregion
                                            crmCustomerMstList.Add(crmCustomerMstObj);
                                        }
                                        else
                                        {
                                            #region Add Tax Details

                                            if (this.CrmCustTaxDetails != null && CrmCustTaxDetails.Count > 0)
                                            {
                                                long[] idArry = CrmCustTaxDetails.Select(x => x.CMT_PK).ToArray();
                                                //long[] tmpArry = crmCustomerMstObj.CRM_CUST_TAX_DTL != null ? crmCustomerMstObj.CRM_CUST_TAX_DTL.Select(x => x.CMT_PK).ToArray() : null;

                                                //long[] 
                                                if (crmCustomerMstObj.CRM_CUST_TAX_DTL != null)
                                                {
                                                    foreach (CRM_CUST_TAX_DTL dtl in crmCustomerMstObj.CRM_CUST_TAX_DTL.Where(x => !idArry.Contains(x.CMT_PK)).ToList())
                                                    {
                                                        crmCustomerMstObj.CRM_CUST_TAX_DTL.Remove(dtl);
                                                    }
                                                }

                                                foreach (CRM_CUST_TAX_DTL dtl in this.CrmCustTaxDetails)
                                                {
                                                    if (dtl.CMT_PK == 0)
                                                    {
                                                        crmCustomerMstObj.CRM_CUST_TAX_DTL.Add(dtl);
                                                    }
                                                    else if (!idArry.Contains(dtl.CMT_PK))
                                                    {
                                                        crmCustomerMstObj.CRM_CUST_TAX_DTL.Remove(dtl);
                                                    }
                                                }
                                            }
                                            else if (this.CrmCustTaxDetails != null && crmCustomerMstObj.CRM_CUST_TAX_DTL.Any())
                                            {
                                                crmCustomerMstObj.CRM_CUST_TAX_DTL.Clear();
                                            }

                                            #endregion
                                        }
                                        //call validation SP if any
                                        hdfValdSP = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "VALD_SP");
                                        if (hdfValdSP != null && !string.IsNullOrEmpty(hdfValdSP.Value))
                                        {
                                            hdfValdSPParm = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "VALD_SP_PARAM");
                                            if (hdfValdSPParm != null && !string.IsNullOrEmpty(hdfValdSPParm.Value))
                                            {
                                                //Get the sp name
                                                spName = hdfValdSP.Value;
                                                // set the parameters of the sp
                                                paramControls = hdfValdSPParm.Value.Split(',');
                                                if (paramControls.Length > 0)
                                                {
                                                    methodParams = new object[paramControls.Length + 2];
                                                    methodParams[0] = CurrPK;
                                                    count = 1;
                                                    foreach (string controlId in paramControls)
                                                    {
                                                        if ((pnlControls.FindControl(controlId)) != null)
                                                        {
                                                            string controlType = pnlControls.FindControl(controlId).GetType().Name;
                                                            if (controlType.Equals("TextBox"))
                                                            {
                                                                TextBox txtBox = (TextBox)pnlControls.FindControl(controlId);
                                                                if (txtBox != null && !string.IsNullOrEmpty(txtBox.Text.Trim()))
                                                                {
                                                                    methodParams[count] = txtBox.Text.Trim();
                                                                }
                                                                else
                                                                {
                                                                    methodParams[count] = null;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                methodParams[count] = null;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            methodParams[count] = null;
                                                        }
                                                        count++;
                                                    }
                                                    methodParams[count] = currentUser.SBUID;
                                                    if (methodParams.Count() > 2)
                                                    {
                                                        commonServiceClient = new CommonService();
                                                        commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                                                        retObj = commonServiceClient.ExecuteSP(spName, methodParams);
                                                        if (retObj != null)
                                                        {
                                                            string retVal = string.Empty;
                                                            string errorMsg = string.Empty;
                                                            ObjectResult objResult = (ObjectResult)retObj;
                                                            foreach (Object srcObj in objResult)
                                                            {
                                                                Type targetTable = srcObj.GetType();
                                                                foreach (PropertyInfo p in targetTable.GetProperties())
                                                                {
                                                                    if (p.Name.Equals("RET_VAL"))
                                                                    {
                                                                        retVal = p.GetValue(srcObj, null).ToString();
                                                                    }
                                                                    else if (p.Name.Equals("RET_TEXT"))
                                                                    {
                                                                        errorMsg = p.GetValue(srcObj, null).ToString();
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                            if (!string.IsNullOrEmpty(retVal))
                                                            {
                                                                if (retVal.Equals("0"))
                                                                {
                                                                    valid = 0;
                                                                    litErrorMsg.Text = GetLocalResourceObject("msg_Error").ToString();
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                }
                                                                else if (retVal.Equals("-1"))
                                                                {
                                                                    valid = 0;
                                                                    litErrorMsg.Text = errorMsg;
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                }
                                                                else if (retVal.Equals("1"))
                                                                {
                                                                    valid = 1;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (valid == 1)
                                        {
                                            //Save customer master operation
                                            result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                            if (result >= 0) // Success ! re-initialize the page
                                            {
                                                if (GetGlobalResourceObject("ConfigurationsRes", "IsCrmEnabled").ToString() == "1")
                                                {
                                                    int pRefId = 0;
                                                    if (!string.IsNullOrEmpty(prefID))
                                                        pRefId = Int32.Parse(prefID);
                                                    int crmResult = CommonBL.SaveAsCRMLead(result, pRefId);
                                                    if (crmResult <= 0)
                                                    {
                                                        litErrorMsg.Text = GetLocalResourceObject("msg_ErrorCRM").ToString();
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                        return;
                                                    }
                                                    prefID = string.Empty;
                                                }
                                                ResetTaxPopUpSessionData();
                                                litErrorMsg.Text = Resources.Report.Msg_Save_Success;
                                                //set dynamic message
                                                message = CustomerRegistrationTabs.DynamicTabDesc;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), message);
                                                //Get the dynamic tab list
                                                GetFieldValues(ControlsEnum.DYNAMICTABS);
                                                if (spAdmFormTabCfgGetResultList != null && spAdmFormTabCfgGetResultList.Count > 0)
                                                {
                                                    for (int i = 0; i < spAdmFormTabCfgGetResultList.Count; i++)
                                                    {
                                                        if (spAdmFormTabCfgGetResultList[i].ATC_CODE.Equals(tabCode))
                                                        {
                                                            if (spAdmFormTabCfgGetResultList[i + 1] != null)
                                                            {
                                                                Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = spAdmFormTabCfgGetResultList[i + 1].ATC_CODE;
                                                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = result;
                                                                //Show save success message and redirect to the next tab
                                                                if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                                                                {
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString()) + "');", true);
                                                                }
                                                                else
                                                                {
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode) + "');", true);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        GetFieldValues(ControlsEnum.CUSTOMER);
                                        if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                        {
                                            doSave = false;
                                            //Set the UI values to the curresponding entity. If this succesfully completed return true, otherwise return false
                                            SaveOrUpdateEntity(crmCustomerMstList[0], entityName);
                                            if (doSave)
                                            {
                                                ////
                                                /////
                                                //call validation SP if any
                                                hdfValdSP = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "VALD_SP");
                                                if (hdfValdSP != null && !string.IsNullOrEmpty(hdfValdSP.Value))
                                                {
                                                    hdfValdSPParm = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "VALD_SP_PARAM");
                                                    if (hdfValdSPParm != null && !string.IsNullOrEmpty(hdfValdSPParm.Value))
                                                    {
                                                        //Get the sp name
                                                        spName = hdfValdSP.Value;
                                                        // set the parameters of the sp
                                                        paramControls = hdfValdSPParm.Value.Split(',');
                                                        if (paramControls.Length > 0)
                                                        {
                                                            methodParams = new object[paramControls.Length + 1];
                                                            methodParams[0] = CurrPK;
                                                            count = 1;
                                                            foreach (string controlId in paramControls)
                                                            {
                                                                if ((pnlControls.FindControl(controlId)) != null)
                                                                {
                                                                    string controlType = pnlControls.FindControl(controlId).GetType().Name;
                                                                    if (controlType.Equals("TextBox"))
                                                                    {
                                                                        TextBox txtBox = (TextBox)pnlControls.FindControl(controlId);
                                                                        if (txtBox != null && !string.IsNullOrEmpty(txtBox.Text.Trim()))
                                                                        {
                                                                            methodParams[count] = txtBox.Text.Trim();
                                                                        }
                                                                        else
                                                                        {
                                                                            methodParams[count] = null;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        methodParams[count] = null;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (controlId.EndsWith("PK"))
                                                                    {
                                                                        string[] entityNameSplit = entityName.Split('.');
                                                                        if (entityNameSplit.Length > 0)
                                                                        {
                                                                            if (Session[entityNameSplit[entityNameSplit.Length - 1]] != null)
                                                                            {
                                                                                methodParams[count] = Convert.ToInt32(Session[entityNameSplit[entityNameSplit.Length - 1]].ToString());
                                                                            }
                                                                            else
                                                                            {
                                                                                methodParams[count] = null;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            methodParams[count] = null;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        methodParams[count] = null;
                                                                    }
                                                                }
                                                                count++;
                                                            }
                                                            if (methodParams.Count() > 1)
                                                            {
                                                                commonServiceClient = new CommonService();
                                                                commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                                                                retObj = commonServiceClient.ExecuteSP(spName, methodParams);
                                                                if (retObj != null)
                                                                {
                                                                    string retVal = string.Empty;
                                                                    string errorMsg = string.Empty;
                                                                    ObjectResult objResult = (ObjectResult)retObj;
                                                                    foreach (Object srcObj in objResult)
                                                                    {
                                                                        Type targetTable = srcObj.GetType();
                                                                        foreach (PropertyInfo p in targetTable.GetProperties())
                                                                        {
                                                                            if (p.Name.Equals("RET_VAL"))
                                                                            {
                                                                                retVal = p.GetValue(srcObj, null).ToString();
                                                                            }
                                                                            else if (p.Name.Equals("RET_TEXT"))
                                                                            {
                                                                                errorMsg = p.GetValue(srcObj, null).ToString();
                                                                            }
                                                                        }
                                                                        break;
                                                                    }
                                                                    if (!string.IsNullOrEmpty(retVal))
                                                                    {
                                                                        if (retVal.Equals("0"))
                                                                        {
                                                                            valid = 0;
                                                                            litErrorMsg.Text = GetLocalResourceObject("msg_Error").ToString();
                                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                        }
                                                                        else if (retVal.Equals("-1"))
                                                                        {
                                                                            valid = 0;
                                                                            litErrorMsg.Text = errorMsg;
                                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                        }
                                                                        else if (retVal.Equals("1"))
                                                                        {
                                                                            valid = 1;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                ////
                                                if (valid == 1)
                                                {
                                                    //Save operation. For saving we will pass only the master entity, ie, Customer master entity. 
                                                    //It wil save/update/delete the whole subentities which the status is change. 
                                                    //If saving is succesfully completed return value will be the selected customer master pk

                                                    if (this.SelectedPK == 0)
                                                    {
                                                        #region Add Tax Details

                                                        if (this.CrmCustTaxDetails != null && CrmCustTaxDetails.Count > 0)
                                                        {
                                                            //var obj = crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK).CRM_CUST_TAX_DTL;
                                                            //obj = new System.Data.Objects.DataClasses.EntityCollection<CRM_CUST_TAX_DTL>();
                                                            //crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK).CRM_CUST_TAX_DTL = new System.Data.Objects.DataClasses.EntityCollection<CRM_CUST_TAX_DTL>();
                                                            CrmCustTaxDetails.ForEach(dtl =>
                                                            {
                                                                crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK).CRM_CUST_TAX_DTL.Add(dtl);
                                                            });
                                                        }

                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region Add Tax Details

                                                        if (this.CrmCustTaxDetails != null && CrmCustTaxDetails.Count > 0)
                                                        {
                                                            var crmCustItemMapObj = crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK);
                                                            long[] idArry = CrmCustTaxDetails.Select(x => x.CMT_PK).ToArray();

                                                            if (crmCustItemMapObj.CRM_CUST_TAX_DTL != null)
                                                            {
                                                                foreach (CRM_CUST_TAX_DTL dtl in crmCustItemMapObj.CRM_CUST_TAX_DTL.Where(x => !idArry.Contains(x.CMT_PK)).ToList())
                                                                {
                                                                    crmCustItemMapObj.CRM_CUST_TAX_DTL.Remove(dtl);
                                                                }
                                                            }

                                                            foreach (CRM_CUST_TAX_DTL dtl in this.CrmCustTaxDetails)
                                                            {
                                                                if (dtl.CMT_PK == 0)
                                                                {
                                                                    crmCustItemMapObj.CRM_CUST_TAX_DTL.Add(dtl);
                                                                }
                                                                else if (!idArry.Contains(dtl.CMT_PK))
                                                                {
                                                                    crmCustItemMapObj.CRM_CUST_TAX_DTL.Remove(dtl);
                                                                }
                                                            }
                                                        }
                                                        else if (this.CrmCustTaxDetails != null
                                                            && crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK)
                                                                        .CRM_CUST_TAX_DTL.Any())
                                                        {
                                                            crmCustomerMstList[0].CRM_CUST_ITEM_MAP.Single(x => x.CIM_PK == this.SelectedPK).CRM_CUST_TAX_DTL.Clear();
                                                        }

                                                        #endregion
                                                    }

                                                    if (IsBrandItemInsert && objBrandItemInsert.CIM_BRAND_CODE != null && objBrandItemInsert.CIM_BRAND_NAME != null)
                                                    {
                                                        CustomerInvItem custItem = new CustomerInvItem()
                                                        {
                                                            BIZUNIT = currentUser.SBUID,
                                                            ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE),
                                                            CIM_CUSTOMER = objBrandItemInsert.CIM_CUSTOMER_PK,
                                                            CIM_BRAND_NAME = HttpUtility.HtmlEncode(objBrandItemInsert.CIM_BRAND_NAME),
                                                            CIM_BRAND_CODE = objBrandItemInsert.CIM_BRAND_CODE,
                                                            CIM_PK = 0,
                                                            USER_PK = currentUser.PKUser
                                                        };

                                                        result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList, custItem, true);

                                                    }
                                                    else
                                                    {
                                                        result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                                    }

                                                    if (result >= 0) // Success ! re-initialize the page
                                                    {
                                                        ResetTaxPopUpSessionData();
                                                        litErrorMsg.Text = Resources.Report.Msg_Save_Success;
                                                        //set dynamic message
                                                        message = CustomerRegistrationTabs.DynamicTabDesc;
                                                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), message);
                                                        //Clear the entity session
                                                        if (Session["EntityByGroup"] != null)
                                                        {
                                                            //Get the dictionary from session. The dictionary have order and entity name as it's key value pair
                                                            dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                            foreach (KeyValuePair<int, string> item in dictionary)
                                                            {
                                                                //Split the entity name with '.', because sometimes the entity name may be a referenced entity seperated by '.'
                                                                string[] stringArray = item.Value.Split('.');
                                                                foreach (string sessionString in stringArray)
                                                                {
                                                                    if (Session[sessionString] != null)
                                                                    {
                                                                        Session[sessionString] = null;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //Get the dynamic tab list
                                                        GetFieldValues(ControlsEnum.DYNAMICTABS);
                                                        if (spAdmFormTabCfgGetResultList != null && spAdmFormTabCfgGetResultList.Count > 0)
                                                        {
                                                            for (int i = 0; i < spAdmFormTabCfgGetResultList.Count; i++)
                                                            {
                                                                if (spAdmFormTabCfgGetResultList[i].ATC_CODE.Equals(tabCode))
                                                                {
                                                                    if (spAdmFormTabCfgGetResultList[i + 1] != null)
                                                                    {
                                                                        Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = spAdmFormTabCfgGetResultList[i + 1].ATC_CODE;
                                                                        //Show save success message and redirect to the next tab
                                                                        if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                                                                        {
                                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString()) + "');", true);
                                                                        }
                                                                        else
                                                                        {
                                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode) + "');", true);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                    }
                                }
                            }
                            break;
                        #endregion
                        #region EDIT
                        case ActionsEnum.EDIT:
                            //Get the grid name of the Edit button. 
                            hdfGrid = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Entity");
                            if (hdfGrid != null && !string.IsNullOrEmpty(hdfGrid.Value))
                            {
                                //Get the grid object from UI
                                GridView grid = (GridView)pnlControls.FindControl(hdfGrid.Value);
                                int rowId = 0;
                                rbtnChecked = false;
                                foreach (GridViewRow grdrow in grid.Rows)
                                {
                                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID + rowId);
                                    // check row selected or not
                                    if (rbtn.Checked)
                                    {
                                        rbtnChecked = true;
                                        // get pk from the grid and assign to CurrPk
                                        SelectedPK = Convert.ToInt32(grid.Rows[rowId].Cells[1].Text);
                                        ResetTaxPopUpSessionData();
                                        //Get the group number of the Edit button. 
                                        HiddenField hdfGroup = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Group");
                                        if (hdfGroup != null && !string.IsNullOrEmpty(hdfGroup.Value))
                                        {
                                            int intOut;
                                            if (Int32.TryParse(hdfGroup.Value, out intOut))
                                            {
                                                if (Session["EntityByGroup"] != null)
                                                {
                                                    //Get the dictionary from the session
                                                    dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                    //Get the entity name of the group
                                                    string entityName = dictionary.FirstOrDefault(dic => dic.Key == Convert.ToInt32(hdfGroup.Value)).Value;
                                                    if (!string.IsNullOrEmpty(entityName))
                                                    {
                                                        if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString()))//If the entity is customer Master
                                                        #region Seperate File Upload Table- Now it doesn't use
                                                        //|| entityName.Equals("CRM_CUSTOMER_MST.CRM_CUST_DOCUMENT_DTL"))
                                                        #endregion
                                                        {
                                                            //set the customerpk session
                                                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = SelectedPK;
                                                            //Get the dynamic tab list
                                                            GetFieldValues(ControlsEnum.DYNAMICTABS);
                                                            if (spAdmFormTabCfgGetResultList != null && spAdmFormTabCfgGetResultList.Count > 0)
                                                            {
                                                                for (int i = 0; i < spAdmFormTabCfgGetResultList.Count; i++)
                                                                {
                                                                    if (spAdmFormTabCfgGetResultList[i].ATC_CODE.Equals(tabCode))
                                                                    {
                                                                        if (spAdmFormTabCfgGetResultList[i + 1] != null)
                                                                        {
                                                                            Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = spAdmFormTabCfgGetResultList[i + 1].ATC_CODE;
                                                                            //Redirect to the next tab
                                                                            if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                                                                            {
                                                                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString()), false);
                                                                            }
                                                                            else
                                                                            {
                                                                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode), false);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //create new instance of service. This object should be maintain until the operation is completed. This is for maintaining the entity object context
                                                            customerRegistrationServiceClient = new CustomerRegistrationService();
                                                            customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                                            //Get the Customer Master details
                                                            GetFieldValues(ControlsEnum.CUSTOMER);
                                                            if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                                            {
                                                                Object retEntity = null;
                                                                //Get the EntityCollection object from the entityname
                                                                retEntity = GetEntityCollection(crmCustomerMstList[0], entityName);
                                                                if (retEntity != null)
                                                                {
                                                                    IEnumerable entityEnumList = null;
                                                                    //Get the Ienumerable list from the entityCollection
                                                                    entityEnumList = (IEnumerable)retEntity;
                                                                    if (entityEnumList != null)
                                                                    {
                                                                        foreach (Object entityobj in entityEnumList)
                                                                        {
                                                                            PropertyInfo propObj;
                                                                            propObj = null;
                                                                            //Get the property that ends with "PK"
                                                                            propObj = entityobj.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                                                                            if (propObj != null && propObj.GetValue(entityobj, null) != null)
                                                                            {
                                                                                //check the PK value is equal to the SelectedPK(selected Parent gird PK)
                                                                                if (Convert.ToInt32(propObj.GetValue(entityobj, null)) == SelectedPK)
                                                                                {
                                                                                    //Set the values from object to UI controls
                                                                                    GetUIValuesFromObject(ActionsEnum.SAVE, entityobj);
                                                                                    string[] entityNameArray = entityName.Split('.');
                                                                                    //set the session , name as the entity name and value as the selected pk
                                                                                    Session[entityNameArray[entityNameArray.Length - 1]] = SelectedPK;
                                                                                    if (!string.IsNullOrEmpty(RelatedControlID))
                                                                                    {
                                                                                        if (RelatedControlID.Equals(grid.ID))
                                                                                        {
                                                                                            SelectedParentGrid = grid.ID;//Parent Grid ID
                                                                                            SelectedParentPK = SelectedPK;//Selected parent entity pk
                                                                                            //Get the grid object from the child grid name
                                                                                            GridView childGrid = (GridView)pnlControls.FindControl(ChildGridName);
                                                                                            if (childGrid != null && ControlPK > 0 && !string.IsNullOrEmpty(EntityName))
                                                                                            {
                                                                                                //Bind grid . parameters is gridPK, Entityname(entity that to be bind to the grid),gridName
                                                                                                BindGrid(ControlPK, EntityName, childGrid);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                        string controlQuery = hdfQry.Value;
                                                                        TextBox txt = (TextBox)pnlControls.FindControl(hdfPopUpAssociatedControl.Value);
                                                                        if (!string.IsNullOrEmpty(controlQuery))
                                                                        {

                                                                            if (txt != null)
                                                                            {
                                                                                List<string> _result = GetTextBoxValueBasedOnStoredQuery(controlQuery);
                                                                                if (_result != null) txt.Text = _result[0] ?? string.Empty;
                                                                            }
                                                                        }
                                                                        //JS functioncall for hide the hdr part(works if it has subtab)
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){return hideHdr(1);});", true);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    rowId++;
                                }
                                if (!rbtnChecked)//no rows selected from the grid
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_select_row").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }

                            break;
                        #endregion
                        #region VIEW
                        case ActionsEnum.VIEW:
                            //Get the grid name of the View button. 
                            hdfGrid = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Entity");
                            if (hdfGrid != null && !string.IsNullOrEmpty(hdfGrid.Value))
                            {
                                //Get the grid object from UI
                                GridView grid = (GridView)pnlControls.FindControl(hdfGrid.Value);
                                int rowId = 0;
                                rbtnChecked = false;
                                foreach (GridViewRow grdrow in grid.Rows)
                                {
                                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID + rowId);
                                    // check row selected or not
                                    if (rbtn.Checked)
                                    {
                                        rbtnChecked = true;
                                        // get pk from the grid and assign to CurrPk
                                        SelectedPK = Convert.ToInt32(grid.Rows[rowId].Cells[1].Text);
                                        //Get the group number of the Edit button. 
                                        HiddenField hdfGroup = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Group");
                                        if (hdfGroup != null && !string.IsNullOrEmpty(hdfGroup.Value))
                                        {
                                            int intOut;
                                            if (Int32.TryParse(hdfGroup.Value, out intOut))
                                            {
                                                if (Session["EntityByGroup"] != null)
                                                {
                                                    //Get the dictionary from the session
                                                    dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                    //Get the entity name of the group
                                                    string entityName = dictionary.FirstOrDefault(dic => dic.Key == Convert.ToInt32(hdfGroup.Value)).Value;
                                                    if (!string.IsNullOrEmpty(entityName))
                                                    {
                                                        if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString()))//If the entity is customer Master
                                                        #region Seperate File Upload Table- Now it doesn't use
                                                        //|| entityName.Equals("CRM_CUSTOMER_MST.CRM_CUST_DOCUMENT_DTL"))
                                                        #endregion
                                                        {
                                                            //set the customerpk session
                                                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = SelectedPK;
                                                            //Get the dynamic tab list
                                                            GetFieldValues(ControlsEnum.DYNAMICTABS);
                                                            if (spAdmFormTabCfgGetResultList != null && spAdmFormTabCfgGetResultList.Count > 0)
                                                            {
                                                                for (int i = 0; i < spAdmFormTabCfgGetResultList.Count; i++)
                                                                {
                                                                    if (spAdmFormTabCfgGetResultList[i].ATC_CODE.Equals(tabCode))
                                                                    {
                                                                        if (spAdmFormTabCfgGetResultList[i + 1] != null)
                                                                        {
                                                                            Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = spAdmFormTabCfgGetResultList[i + 1].ATC_CODE;
                                                                            //Redirect to the next tab
                                                                            if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                                                                            {
                                                                                //EntryStatus = EntryStatus.VIEWMODE;
                                                                                //Session["EntryStatus"] = EntryStatus;
                                                                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString()), false);
                                                                            }
                                                                            else
                                                                            {
                                                                                //EntryStatus = EntryStatus.VIEWMODE;
                                                                                //Session["EntryStatus"] = EntryStatus;
                                                                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode), false);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //create new instance of service. This object should be maintain until the operation is completed. This is for maintaining the entity object context
                                                            customerRegistrationServiceClient = new CustomerRegistrationService();
                                                            customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                                            //Get the Customer Master details
                                                            GetFieldValues(ControlsEnum.CUSTOMER);
                                                            if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                                            {
                                                                Object retEntity = null;
                                                                //Get the EntityCollection object from the entityname
                                                                retEntity = GetEntityCollection(crmCustomerMstList[0], entityName);
                                                                if (retEntity != null)
                                                                {
                                                                    IEnumerable entityEnumList = null;
                                                                    //Get the Ienumerable list from the entityCollection
                                                                    entityEnumList = (IEnumerable)retEntity;
                                                                    if (entityEnumList != null)
                                                                    {
                                                                        foreach (Object entityobj in entityEnumList)
                                                                        {
                                                                            PropertyInfo propObj;
                                                                            propObj = null;
                                                                            //Get the property that ends with "PK"
                                                                            propObj = entityobj.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                                                                            if (propObj != null && propObj.GetValue(entityobj, null) != null)
                                                                            {
                                                                                //check the PK value is equal to the SelectedPK(selected Parent gird PK)
                                                                                if (Convert.ToInt32(propObj.GetValue(entityobj, null)) == SelectedPK)
                                                                                {
                                                                                    //Set the values from object to UI controls
                                                                                    GetUIValuesFromObject(ActionsEnum.SAVE, entityobj);
                                                                                    string[] entityNameArray = entityName.Split('.');
                                                                                    //set the session , name as the entity name and value as the selected pk
                                                                                    Session[entityNameArray[entityNameArray.Length - 1]] = SelectedPK;
                                                                                    if (!string.IsNullOrEmpty(RelatedControlID))
                                                                                    {
                                                                                        if (RelatedControlID.Equals(grid.ID))
                                                                                        {
                                                                                            SelectedParentGrid = grid.ID;//Parent Grid ID
                                                                                            SelectedParentPK = SelectedPK;//Selected parent entity pk
                                                                                            //Get the grid object from the child grid name
                                                                                            GridView childGrid = (GridView)pnlControls.FindControl(ChildGridName);
                                                                                            if (childGrid != null && ControlPK > 0 && !string.IsNullOrEmpty(EntityName))
                                                                                            {
                                                                                                //Bind grid . parameters is gridPK, Entityname(entity that to be bind to the grid),gridName
                                                                                                BindGrid(ControlPK, EntityName, childGrid);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                        //JS functioncall for hide the hdr part(works if it has subtab)
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){return hideHdr(1);});", true);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    rowId++;
                                }
                                if (!rbtnChecked)//no rows selected from the grid
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_select_row").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }


                            break;
                        #endregion
                        #region REMOVE
                        case ActionsEnum.REMOVE:
                            //Get the grid name of the Remove button. 
                            hdfGrid = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Entity");
                            if (hdfGrid != null && !string.IsNullOrEmpty(hdfGrid.Value))
                            {
                                //Get the grid object from UI
                                GridView grid = (GridView)pnlControls.FindControl(hdfGrid.Value);
                                int rowId = 0;
                                rbtnChecked = false;
                                foreach (GridViewRow grdrow in grid.Rows)
                                {
                                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID + rowId);
                                    // check row selected or not
                                    if (rbtn.Checked)
                                    {
                                        rbtnChecked = true;
                                        // get pk from the grid and assign to CurrPk
                                        SelectedPK = Convert.ToInt32(grid.Rows[rowId].Cells[1].Text);
                                        //Get the group number of the Remove button. 
                                        HiddenField hdfGroup = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Group");
                                        if (hdfGroup != null && !string.IsNullOrEmpty(hdfGroup.Value))
                                        {
                                            int intOut;
                                            if (Int32.TryParse(hdfGroup.Value, out intOut))
                                            {
                                                if (Session["EntityByGroup"] != null)
                                                {
                                                    //Get the dictionary from the session
                                                    dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                    //Get the entity name of the group
                                                    string entityName = dictionary.FirstOrDefault(dic => dic.Key == Convert.ToInt32(hdfGroup.Value)).Value;
                                                    if (!string.IsNullOrEmpty(entityName))
                                                    {
                                                        if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString()))//If the entity is customer Master
                                                        #region Seperate File Upload Table- Now it doesn't use
                                                        //|| entityName.Equals("CRM_CUSTOMER_MST.CRM_CUST_DOCUMENT_DTL"))
                                                        #endregion
                                                        {
                                                            //set the customerpk session
                                                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = SelectedPK;

                                                            ResetTaxPopUpSessionData();

                                                            //create new instance of service. This object should be maintain until the operation is completed. This is for maintaining the entity object context
                                                            customerRegistrationServiceClient = new CustomerRegistrationService();
                                                            customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                                            CurrPK = SelectedPK;
                                                            //Get the customer master details
                                                            GetFieldValues(ControlsEnum.CUSTOMER);
                                                            if (crmCustomerMstList != null && crmCustomerMstList.Count == 1)
                                                            {
                                                                //set the EntityState of the object as deleted
                                                                customerRegistrationServiceClient.ChangeObjectState(crmCustomerMstList[0], EntityState.Deleted);
                                                                //Remove operation. For Remove we will pass only the master entity, ie, Customer master entity. 
                                                                //It wil save/update/delete the whole subentities which the status is change. 
                                                                //If Remove is succesfully completed return value will be the selected customer master pk
                                                                result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                                                if (result >= 0) // Success ! re-initialize the page
                                                                {
                                                                    CurrPK = 0;
                                                                    ResetTaxPopUpSessionData();
                                                                    litErrorMsg.Text = Resources.Report.Msg_Remove_Success;
                                                                    //set dynamic message
                                                                    message = CustomerRegistrationTabs.DynamicTabDesc;
                                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), message);
                                                                    if (!hdfSubTabValue.Value.Equals("Dtl"))//check whether the tab have subtab. if yes, check current subtab is not details subtab
                                                                    {
                                                                        //Clear the entity session
                                                                        if (Session["EntityByGroup"] != null)
                                                                        {
                                                                            //Get the dictionary from session. The dictionary have order and entity name as it's key value pair
                                                                            dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                                            foreach (KeyValuePair<int, string> item in dictionary)
                                                                            {
                                                                                //Split the entity name with '.', because sometimes the entity name may be a referenced entity seperated by '.'
                                                                                string[] stringArray = item.Value.Split('.');
                                                                                foreach (string sessionString in stringArray)
                                                                                {
                                                                                    if (Session[sessionString] != null)
                                                                                    {
                                                                                        Session[sessionString] = null;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    //Show save success message and redirect to the same Tab
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode) + "');", true);
                                                                }
                                                                else if (result == -1)//you tried to delete is already used in other places. Please try Inactivate the reference
                                                                {
                                                                    litErrorMsg.Text = GetLocalResourceObject("msg_delete_reference_error").ToString();
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode) + "');", true);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //create new instance of service. This object should be maintain until the operation is completed. This is for maintaining the entity object context
                                                            customerRegistrationServiceClient = new CustomerRegistrationService();
                                                            customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                                            //Get the customer master details
                                                            GetFieldValues(ControlsEnum.CUSTOMER);
                                                            if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                                            {
                                                                //Get the EntityCollection object from the entityname
                                                                Object retEntity = GetEntityCollection(crmCustomerMstList[0], entityName);
                                                                IEnumerable entityEnumList = null;
                                                                //Get the Ienumerable list from the entityCollection
                                                                entityEnumList = (IEnumerable)retEntity;
                                                                if (entityEnumList != null)
                                                                {
                                                                    foreach (Object entityobj in entityEnumList)
                                                                    {
                                                                        PropertyInfo propObj;
                                                                        propObj = null;
                                                                        //Get the property that ends with "PK"
                                                                        propObj = entityobj.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                                                                        if (propObj != null && propObj.GetValue(entityobj, null) != null)
                                                                        {
                                                                            //check the PK value is equal to the SelectedPK
                                                                            if (Convert.ToInt32(propObj.GetValue(entityobj, null)) == SelectedPK)
                                                                            {
                                                                                //set the EntityState of the object as deleted
                                                                                customerRegistrationServiceClient.ChangeObjectState(entityobj, EntityState.Deleted);
                                                                                //Remove operation. For Remove we will pass only the master entity, ie, Customer master entity. 
                                                                                //It wil save/update/delete the whole subentities which the status is change. 
                                                                                //If Remove is succesfully completed return value will be the selected customer master pk
                                                                                //if (IsBrandItemInsert && objBrandItemInsert.CIM_BRAND_CODE != null && objBrandItemInsert.CIM_BRAND_NAME!=null)
                                                                                if (IsBrandItemInsert && entityobj.ToString() != "ERPData.CRM_CUST_ADDRESS" && entityobj.ToString() != "ERPData.CRM_CUST_TERM_HDR")
                                                                                {
                                                                                    CustomerInvItem custItem = new CustomerInvItem()
                                                                                    {
                                                                                        BIZUNIT = currentUser.SBUID,
                                                                                        CIM_CUSTOMER = ((ERPData.CRM_CUST_ITEM_MAP)(entityobj)).CIM_CUSTOMER.ToString(),
                                                                                        CIM_BRAND_NAME = HttpUtility.HtmlEncode(((ERPData.CRM_CUST_ITEM_MAP)(entityobj)).CIM_BRAND_NAME),
                                                                                        CIM_BRAND_CODE = ((ERPData.CRM_CUST_ITEM_MAP)(entityobj)).CIM_BRAND_CODE,
                                                                                        CIM_PK = 0,
                                                                                        USER_PK = currentUser.PKUser
                                                                                    };
                                                                                    result = customerRegistrationServiceClient.DeleteSaveCrmCustomerMst(crmCustomerMstList, custItem);
                                                                                }
                                                                                else
                                                                                {
                                                                                    result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                                                                }
                                                                                if (result >= 0) // Success ! re-initialize the page
                                                                                {
                                                                                    ResetTaxPopUpSessionData();
                                                                                    litErrorMsg.Text = Resources.Report.Msg_Remove_Success;
                                                                                    //set dynamic message
                                                                                    message = CustomerRegistrationTabs.DynamicTabDesc;
                                                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), message);
                                                                                    if (!hdfSubTabValue.Value.Equals("Dtl"))//check whether the tab have subtab. if yes, check current subtab is not details subtab
                                                                                    {
                                                                                        if (Session["EntityByGroup"] != null)
                                                                                        {
                                                                                            //Get the dictionary from session. The dictionary have order and entity name as it's key value pair
                                                                                            dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                                                            foreach (KeyValuePair<int, string> item in dictionary)
                                                                                            {
                                                                                                //Split the entity name with '.', because sometimes the entity name may be a referenced entity seperated by '.'
                                                                                                string[] stringArray = item.Value.Split('.');
                                                                                                foreach (string sessionString in stringArray)
                                                                                                {
                                                                                                    if (Session[sessionString] != null)
                                                                                                    {
                                                                                                        Session[sessionString] = null;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        //Show the remove success message and redirect to the same tab
                                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode) + "');", true);
                                                                                    }
                                                                                    else//If the tab have subtab and subtab is details subtab
                                                                                    {
                                                                                        if (Session["EntityByGroup"] != null)
                                                                                        {
                                                                                            //Get the dictionary from session. The dictionary have order and entity name as it's key value pair
                                                                                            dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                                                            entityName = string.Empty;
                                                                                            //Get the entity name of the group
                                                                                            entityName = dictionary.FirstOrDefault(dic => dic.Key == Convert.ToInt32(hdfGroup.Value)).Value;
                                                                                            string tempEntityName = entityName;
                                                                                            if (!string.IsNullOrEmpty(entityName))
                                                                                            {
                                                                                                //Get the customer master details
                                                                                                GetFieldValues(ControlsEnum.CUSTOMER);
                                                                                                if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                                                                                {
                                                                                                    Object retHdrEntity = null;
                                                                                                    string[] entityNameArray = entityName.Split('.');
                                                                                                    if (entityNameArray.Length > 1)
                                                                                                    {
                                                                                                        //clear the session with the child entityname
                                                                                                        Session[entityNameArray[entityNameArray.Length - 1]] = null;
                                                                                                        //set the entity name as the 1st parent of the child entity
                                                                                                        entityName = entityNameArray[entityNameArray.Length - 2];
                                                                                                    }
                                                                                                    //Get the EntityCollection object from the entityname
                                                                                                    retHdrEntity = GetEntityCollection(crmCustomerMstList[0], entityName);
                                                                                                    if (retHdrEntity != null)
                                                                                                    {
                                                                                                        IEnumerable entityEnumListTemp = null;
                                                                                                        entityEnumListTemp = (IEnumerable)retHdrEntity;
                                                                                                        if (entityEnumListTemp != null)
                                                                                                        {
                                                                                                            foreach (Object entityobjtemp in entityEnumListTemp)
                                                                                                            {
                                                                                                                PropertyInfo propObjTemp;
                                                                                                                propObjTemp = null;
                                                                                                                //Get the property that ends with "PK"
                                                                                                                propObjTemp = entityobjtemp.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                                                                                                                if (propObjTemp != null && propObjTemp.GetValue(entityobjtemp, null) != null)
                                                                                                                {
                                                                                                                    //check the PK value is equal to the selectedparentpk(selected Parent gird PK)
                                                                                                                    if (Convert.ToInt32(propObjTemp.GetValue(entityobjtemp, null)) == SelectedParentPK)
                                                                                                                    {
                                                                                                                        //Set the values from object to UI controls
                                                                                                                        GetUIValuesFromObject(ActionsEnum.SAVE, entityobjtemp);
                                                                                                                        string[] entityNameArray1 = tempEntityName.Split('.');
                                                                                                                        if (entityNameArray1.Length > 1)//If has a child entity 
                                                                                                                        {
                                                                                                                            //Create new instance for the child entity. It for clear the detail section only
                                                                                                                            string namespaceString = "ERPData";
                                                                                                                            string className = entityNameArray[entityNameArray.Length - 1];
                                                                                                                            className = namespaceString + "." + className;
                                                                                                                            Assembly currentAssembly = Assembly.Load(namespaceString);
                                                                                                                            Type baseEntity = currentAssembly.GetType(className);
                                                                                                                            Object childEntityObj = Activator.CreateInstance(baseEntity, null);
                                                                                                                            //Set the values from object to UI controls. Here the values will be null. so the controls will be reset
                                                                                                                            GetUIValuesFromObject(ActionsEnum.SAVE, childEntityObj);
                                                                                                                        }
                                                                                                                        if (!string.IsNullOrEmpty(RelatedControlID))
                                                                                                                        {
                                                                                                                            if (RelatedControlID.Equals(SelectedParentGrid))//Check RelatedControlID==SelectedParentGrid for bind the child grid
                                                                                                                            {
                                                                                                                                //Get the grid object from the child grid name
                                                                                                                                GridView childGrid = (GridView)pnlControls.FindControl(ChildGridName);
                                                                                                                                if (childGrid != null && ControlPK > 0 && !string.IsNullOrEmpty(EntityName))
                                                                                                                                {
                                                                                                                                    //Bind grid . parameters is gridPK, Entityname(entity that to be bind to the grid),gridName
                                                                                                                                    BindGrid(ControlPK, EntityName, childGrid);
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        //If it is in Edit Mode
                                                                                        if (SelectedParentPK > 0)
                                                                                        {
                                                                                            HideFileAnchorControlsIfAny();
                                                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                                                        }
                                                                                        else//If New Mode
                                                                                        {
                                                                                            //clear subtab hidden field value
                                                                                            hdfSubTabValue.Value = string.Empty;
                                                                                            //Clear entity session
                                                                                            if (Session["EntityByGroup"] != null)
                                                                                            {
                                                                                                //Get the dictionary from the session
                                                                                                dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                                                                foreach (KeyValuePair<int, string> item in dictionary)
                                                                                                {
                                                                                                    string[] stringArray = item.Value.Split('.');
                                                                                                    foreach (string sessionString in stringArray)
                                                                                                    {
                                                                                                        if (Session[sessionString] != null)
                                                                                                        {
                                                                                                            Session[sessionString] = null;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            //Redirect to the same tab
                                                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode) + "');", true);
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else if (result == -1)//you tried to delete is already used in other places. Please try Inactivate the reference
                                                                                {
                                                                                    litErrorMsg.Text = GetLocalResourceObject("msg_delete_reference_error").ToString();
                                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode) + "');", true);
                                                                                }
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    rowId++;
                                }
                                if (!rbtnChecked)//no rows selected from the grid
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_select_row").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            break;
                        #endregion
                        #region New
                        case ActionsEnum.NEW:
                            //Get the Group number of the New button.
                            hdfIsNew.Value = "1";
                            HiddenField hdfGroup1 = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Group");
                            if (hdfGroup1 != null && !string.IsNullOrEmpty(hdfGroup1.Value))
                            {
                                int intOut;
                                if (Int32.TryParse(hdfGroup1.Value, out intOut))
                                {
                                    if (Session["EntityByGroup"] != null)
                                    {
                                        //Get the dictionary from session. The dictionary have order and entity name as it's key value pair
                                        dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                        //Get the entity name of the group
                                        string entityName = dictionary.FirstOrDefault(dic => dic.Key == Convert.ToInt32(hdfGroup1.Value)).Value;
                                        if (!string.IsNullOrEmpty(entityName))
                                        {
                                            if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString()))//If the entity is customer master
                                            #region Seperate File Upload Table- Now it doesn't use
                                            //|| entityName.Equals("CRM_CUSTOMER_MST.CRM_CUST_DOCUMENT_DTL"))
                                            #endregion
                                            {
                                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = null;//clear customerpk session
                                                //get the dynamic tab list
                                                GetFieldValues(ControlsEnum.DYNAMICTABS);
                                                if (spAdmFormTabCfgGetResultList != null && spAdmFormTabCfgGetResultList.Count > 0)
                                                {
                                                    for (int i = 0; i < spAdmFormTabCfgGetResultList.Count; i++)
                                                    {
                                                        if (spAdmFormTabCfgGetResultList[i].ATC_CODE.Equals(tabCode))
                                                        {
                                                            if (spAdmFormTabCfgGetResultList[i + 1] != null)
                                                            {
                                                                Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = spAdmFormTabCfgGetResultList[i + 1].ATC_CODE;
                                                                //Redirect to next tab
                                                                if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                                                                {
                                                                    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString()), false);
                                                                }
                                                                else
                                                                {
                                                                    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode), false);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else//If not master entity
                                            {
                                                //Check the new button is main new button or child new button.Only the New button in the child section have the checking value. the value is not using anywhere
                                                HiddenField hdfEntity = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Entity");
                                                if (hdfEntity != null && !string.IsNullOrEmpty(hdfEntity.Value))//Child New Button
                                                {
                                                    string[] entityNameArray = entityName.Split('.');
                                                    if (entityNameArray.Length > 0)
                                                    {
                                                        //clear the child entity session 
                                                        Session[entityNameArray[entityNameArray.Length - 1]] = null;
                                                        //create new instance for the child entity
                                                        string namespaceString = "ERPData";
                                                        string className = entityNameArray[entityNameArray.Length - 1];
                                                        className = namespaceString + "." + className;
                                                        Assembly currentAssembly = Assembly.Load(namespaceString);
                                                        Type baseEntity = currentAssembly.GetType(className);
                                                        Object childEntityObj = Activator.CreateInstance(baseEntity, null);
                                                        //Set the values from object to UI controls. Here the values will be null. so the controls will be reset
                                                        GetUIValuesFromObject(ActionsEnum.SAVE, childEntityObj);
                                                        if (!string.IsNullOrEmpty(RelatedControlID))
                                                        {
                                                            if (RelatedControlID.Equals(SelectedParentGrid))//Check RelatedControlID==SelectedParentGrid for bind the child grid
                                                            {
                                                                //Get the grid object from the child grid name
                                                                GridView childGrid = (GridView)pnlControls.FindControl(ChildGridName);
                                                                if (childGrid != null && ControlPK > 0 && !string.IsNullOrEmpty(EntityName))
                                                                {
                                                                    //Bind grid . parameters is gridPK, Entityname(entity that to be bind to the grid),gridName
                                                                    BindGrid(ControlPK, EntityName, childGrid);
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                                else//Main New Button
                                                {
                                                    //clear the entity session
                                                    if (Session["EntityByGroup"] != null)
                                                    {
                                                        //Get the dictionary from session. The dictionary have order and entity name as it's key value pair
                                                        dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                        foreach (KeyValuePair<int, string> item in dictionary)
                                                        {
                                                            string[] stringArray = item.Value.Split('.');
                                                            foreach (string sessionString in stringArray)
                                                            {
                                                                if (Session[sessionString] != null)
                                                                {
                                                                    Session[sessionString] = null;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(hdfSubTabValue.Value))//If the tab have subtab
                                                    {
                                                        HideFileAnchorControlsIfAny();
                                                        //Js for hide the header part
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){return hideHdr(1);});", true);
                                                        ////////////////// To be checked. this is because in the case of main new button in a page with subtab is not clearing the fields
                                                        if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                                                        {
                                                            if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString().Equals(TabType.CIM))
                                                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode), false);
                                                        }

                                                    }
                                                    else//If the tab doesn't have subtab
                                                    {
                                                        //Redirect to the same tab
                                                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode), false);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion
                        #region SELECTEDINDEXCHANGED
                        case ActionsEnum.SELECTEDINDEXCHANGED:
                            if (sender.GetType().IsEquivalentTo(typeof(DropDownList))
                                && ((DropDownList)sender).ID == CustomerStatusFilterControlId)
                            {
                                Session[CustomerStatusFilterSessionKey] = ((DropDownList)sender).SelectedValue;
                                RebindCustomerListingGrid();
                                break;
                            }

                            commonServiceClient = new CommonService();
                            commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                            DropDownList ddlControl = (DropDownList)pnlControls.FindControl(((Button)sender).CommandArgument);
                            if (ddlControl != null)
                            {
                                if (!string.IsNullOrEmpty(ddlControl.SelectedValue))
                                {
                                    //get the selected value from the dropdown
                                    int itemPK = Convert.ToInt32(ddlControl.SelectedValue);
                                    //Get the Spname curresponding to the dropdown
                                    HiddenField hdfAction = (HiddenField)pnlControls.FindControl("hdf" + ddlControl.ID + "Action");
                                    if (hdfAction != null && !string.IsNullOrEmpty(hdfAction.Value))
                                    {
                                        if (hdfAction.Value.Equals("SELECTEDINDEXCHANGED"))
                                        {
                                            //string ddlControlId = spAdmFormTabControlCfgGetResultList.SingleOrDefault(rel => rel.ACC_REL_CONTROL_ID == ddlControl.ID && rel.ACC_CONTROL_TEXT.Equals("DropDown")) == null
                                            //                        ? string.Empty : spAdmFormTabControlCfgGetResultList.SingleOrDefault(rel => rel.ACC_REL_CONTROL_ID == ddlControl.ID && rel.ACC_CONTROL_TEXT.Equals("DropDown")).ACC_CONTROL_ID;
                                            List<SPADM_FORM_TAB_CONTROL_CFG_GET_Result> ddlControlIdList = null;
                                            ddlControlIdList = spAdmFormTabControlCfgGetResultList.Where(rel => rel.ACC_REL_CONTROL_ID == ddlControl.ID && rel.ACC_CONTROL_TEXT.Equals("DropDown")) == null
                                                                    ? null : spAdmFormTabControlCfgGetResultList.Where(rel => rel.ACC_REL_CONTROL_ID == ddlControl.ID && rel.ACC_CONTROL_TEXT.Equals("DropDown")).ToList();
                                            string ddlControlId = "";
                                            foreach (SPADM_FORM_TAB_CONTROL_CFG_GET_Result ddlControlIdListObj in ddlControlIdList)
                                            {
                                                ddlControlId = ddlControlIdListObj.ACC_CONTROL_ID;
                                                if (!string.IsNullOrEmpty(ddlControlId))
                                                {
                                                    DropDownList child = (DropDownList)pnlControls.FindControl(ddlControlId);
                                                    if (child != null)
                                                    {
                                                        string query = spAdmFormTabControlCfgGetResultList.SingleOrDefault(rel => rel.ACC_CONTROL_ID == ddlControlId) == null
                                                            ? string.Empty : spAdmFormTabControlCfgGetResultList.SingleOrDefault(rel => rel.ACC_CONTROL_ID == ddlControlId).ACC_QUERY_TEXT;
                                                        if (!string.IsNullOrEmpty(query))
                                                        {
                                                            CommonService commonService;
                                                            commonService = null;
                                                            string value = string.Empty;
                                                            //Replace query with the values if any condition is there
                                                            List<string> conditionList = new List<string>();
                                                            string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                                            if (splitWithAt.Count() > 1)
                                                            {
                                                                for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                                {
                                                                    conditionList.Add(splitWithAt[arrayCount].Trim());
                                                                }
                                                                DropDownList parent = ddlControl;
                                                                if (parent != null && parent.Items.Count > 1)
                                                                {
                                                                    if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                    {
                                                                        value = parent.SelectedValue;
                                                                    }
                                                                }
                                                                foreach (string condition in conditionList)
                                                                {
                                                                    if (Session[condition] != null)//parameter name is same as any session name
                                                                    {
                                                                        if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                                            query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                                        else
                                                                        {
                                                                            query = string.Empty;
                                                                            break;
                                                                        }
                                                                    }
                                                                    else if (condition == parent.ID)// parameter is value of any other control
                                                                    {
                                                                        if (!string.IsNullOrEmpty(value))
                                                                            query = query.Replace("@" + condition + "@", value);
                                                                        else
                                                                        {
                                                                            query = string.Empty;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (!string.IsNullOrEmpty(query))
                                                            {
                                                                commonService = new CommonService();
                                                                commonService = CommonFunctions.InitiateClient(commonService);
                                                                //Execute query
                                                                query = query.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                                                List<DDLMaster> ddlValues = commonService.ExecuteQuery(query);
                                                                child.DataTextField = "Value";
                                                                child.DataValueField = "PK";
                                                                child.DataSource = ddlValues;
                                                                child.DataBind();
                                                            }
                                                            else
                                                            {
                                                                child.Items.Clear();
                                                            }
                                                        }
                                                        //add "select" to the dropdown
                                                        child.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Get the sp name
                                            spName = hdfAction.Value;
                                            // set the parameters of the sp
                                            methodParams = new object[] { itemPK };
                                            //Execute SP
                                            retObj = commonServiceClient.ExecuteSP(spName, methodParams);
                                            if (retObj != null)
                                            {
                                                //SP retuurns Objectresult
                                                ObjectResult objResult = (ObjectResult)retObj;
                                                count = 0;
                                                foreach (Object srcObj in objResult)
                                                {
                                                    //set the UI controls with the SP return list
                                                    GetUIValuesFromObject(ActionsEnum.SELECTEDINDEXCHANGED, srcObj);
                                                    count++;
                                                }
                                                if (count == 0)//If the sp return list is empty
                                                {
                                                    //Create a new instance of SP Complex type
                                                    string namespaceString = "ERPData";
                                                    string className = objResult.ElementType.Name;
                                                    className = namespaceString + "." + className;
                                                    Assembly currentAssembly = Assembly.Load(namespaceString);
                                                    Type baseEntity = currentAssembly.GetType(className);
                                                    Object entityObj = Activator.CreateInstance(baseEntity, null);
                                                    //set the UI controls with the SP Complextype object
                                                    GetUIValuesFromObject(ActionsEnum.SELECTEDINDEXCHANGED, entityObj);
                                                }
                                            }
                                        }
                                    }
                                }
                                ddlControl.Focus();//keep the focus for continue select controls using tabindex
                            }

                            break;
                        #endregion
                        #region Detail
                        case ActionsEnum.DETAILS:
                            //Get the grid name of the DETAILS button. 
                            hdfGrid = (HiddenField)pnlControls.FindControl("hdf" + ((LinkButton)sender).ID + "Entity");
                            if (hdfGrid != null && !string.IsNullOrEmpty(hdfGrid.Value))
                            {
                                //Get the grid object from UI
                                GridView grid = (GridView)pnlControls.FindControl(hdfGrid.Value);
                                int rowId = 0;
                                rbtnChecked = false;
                                foreach (GridViewRow grdrow in grid.Rows)
                                {
                                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID + rowId);
                                    // check row selected or not
                                    if (rbtn.Checked)
                                    {
                                        rbtnChecked = true;
                                        // get pk from the grid and assign to CurrPk
                                        SelectedPK = Convert.ToInt32(grid.Rows[rowId].Cells[1].Text);
                                        //Get the group number of the DETAILS button. 
                                        HiddenField hdfGroup = (HiddenField)pnlControls.FindControl("hdf" + ((LinkButton)sender).ID + "Group");
                                        if (hdfGroup != null && !string.IsNullOrEmpty(hdfGroup.Value))
                                        {
                                            int intOut;
                                            if (Int32.TryParse(hdfGroup.Value, out intOut))
                                            {
                                                if (Session["EntityByGroup"] != null)
                                                {
                                                    //Get the dictionary from the session
                                                    dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                                    //Get the entity name of the group
                                                    string entityName = dictionary.FirstOrDefault(dic => dic.Key == Convert.ToInt32(hdfGroup.Value)).Value;
                                                    if (!string.IsNullOrEmpty(entityName))
                                                    {
                                                        if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString()))//If the entity is customer Master
                                                        #region Seperate File Upload Table- Now it doesn't use
                                                        //|| entityName.Equals("CRM_CUSTOMER_MST.CRM_CUST_DOCUMENT_DTL"))
                                                        #endregion
                                                        {
                                                            //set the customerpk session
                                                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = SelectedPK;
                                                            //Get the dynamic tab list
                                                            GetFieldValues(ControlsEnum.DYNAMICTABS);
                                                            if (spAdmFormTabCfgGetResultList != null && spAdmFormTabCfgGetResultList.Count > 0)
                                                            {
                                                                for (int i = 0; i < spAdmFormTabCfgGetResultList.Count; i++)
                                                                {
                                                                    if (spAdmFormTabCfgGetResultList[i].ATC_CODE.Equals(tabCode))
                                                                    {
                                                                        if (spAdmFormTabCfgGetResultList[i + 1] != null)
                                                                        {
                                                                            Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = spAdmFormTabCfgGetResultList[i + 1].ATC_CODE;
                                                                            //Redirect to the next tab
                                                                            if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                                                                            {
                                                                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString()), false);
                                                                            }
                                                                            else
                                                                            {
                                                                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode), false);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //create new instance of service. This object should be maintain until the operation is completed. This is for maintaining the entity object context
                                                            customerRegistrationServiceClient = new CustomerRegistrationService();
                                                            customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                                            //Get the Customer Master details
                                                            GetFieldValues(ControlsEnum.CUSTOMER);
                                                            if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                                            {
                                                                Object retEntity = null;
                                                                //Get the EntityCollection object from the entityname
                                                                retEntity = GetEntityCollection(crmCustomerMstList[0], entityName);
                                                                if (retEntity != null)
                                                                {
                                                                    IEnumerable entityEnumList = null;
                                                                    entityEnumList = (IEnumerable)retEntity;
                                                                    if (entityEnumList != null)
                                                                    {
                                                                        foreach (Object entityobj in entityEnumList)
                                                                        {
                                                                            PropertyInfo propObj;
                                                                            propObj = null;
                                                                            //Get the property that ends with "PK"
                                                                            propObj = entityobj.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                                                                            if (propObj != null && propObj.GetValue(entityobj, null) != null)
                                                                            {
                                                                                //check the PK value is equal to the SelectedPK(selected Parent gird PK)
                                                                                if (Convert.ToInt32(propObj.GetValue(entityobj, null)) == SelectedPK)
                                                                                {
                                                                                    //Set the values from object to UI controls
                                                                                    GetUIValuesFromObject(ActionsEnum.SAVE, entityobj);
                                                                                    string[] entityNameArray = entityName.Split('.');
                                                                                    //set the session , name as the entity name and value as the selected pk
                                                                                    Session[entityNameArray[entityNameArray.Length - 1]] = SelectedPK;
                                                                                    if (!string.IsNullOrEmpty(RelatedControlID))
                                                                                    {
                                                                                        if (RelatedControlID.Equals(grid.ID))
                                                                                        {
                                                                                            SelectedParentGrid = grid.ID;//Parent Grid ID
                                                                                            SelectedParentPK = SelectedPK;//Selected parent entity pk
                                                                                            //Get the grid object from the child grid name
                                                                                            GridView childGrid = (GridView)pnlControls.FindControl(ChildGridName);
                                                                                            if (childGrid != null && ControlPK > 0 && !string.IsNullOrEmpty(EntityName))
                                                                                            {
                                                                                                //Bind grid . parameters is gridPK, Entityname(entity that to be bind to the grid),gridName
                                                                                                BindGrid(ControlPK, EntityName, childGrid);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    //JS functioncall for hide the hdr part
                                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){return hideHdr();});", true);
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    rowId++;
                                }
                                if (!rbtnChecked)//no rows selected from the grid
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_select_row").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            break;
                        #endregion
                        #region List
                        case ActionsEnum.LIST:
                            //clear subtab hidden field value
                            hdfSubTabValue.Value = string.Empty;
                            //Clear entity session
                            if (Session["EntityByGroup"] != null)
                            {
                                //Get the dictionary from the session
                                dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                foreach (KeyValuePair<int, string> item in dictionary)
                                {
                                    string[] stringArray = item.Value.Split('.');
                                    foreach (string sessionString in stringArray)
                                    {
                                        if (Session[sessionString] != null)
                                        {
                                            Session[sessionString] = null;
                                        }
                                    }
                                }
                            }
                            //Redirect to the same tab
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode), false);
                            break;
                        #endregion
                        #region SUBMIT
                        case ActionsEnum.SUBMIT:
                            //Show WorkFlow Popup
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                            break;
                        case ActionsEnum.WRKFSUBMIT:
                            //Submit Activity
                            //validate Page
                            if (!IsValid)
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else//valid
                            {
                                //create new instance of service. This object should be maintain until all the save operation is completed. This is for maintaining the entity object context
                                customerRegistrationServiceClient = new CustomerRegistrationService();
                                customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                //Get the entity name of the save button. The details will be saved to this entity
                                HiddenField hdfEntity = (HiddenField)pnlControls.FindControl("hdf" + "BTN_SUBMIT" + "Entity");
                                if (hdfEntity != null && !string.IsNullOrEmpty(hdfEntity.Value))
                                {
                                    string entityName = hdfEntity.Value;
                                    if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString()))//If the entity is customer Master
                                    #region Seperate File Upload Table- Now it doesn't use
                                    //|| entityName.Equals("CRM_CUSTOMER_MST.CRM_CUST_DOCUMENT_DTL"))
                                    #endregion
                                    {
                                        if (CurrPK > 0)
                                        {
                                            //Get customer master list
                                            GetFieldValues(ControlsEnum.CUSTOMER);
                                        }
                                        if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                        {
                                            //set the customer master object
                                            crmCustomerMstObj = crmCustomerMstList[0];
                                            #region Seperate File Upload Table- Now it doesn't use
                                            //crmCustDocumentDtlObj = crmCustomerMstObj.CRM_CUST_DOCUMENT_DTL.FirstOrDefault(ss => ss.CDD_CUSTOMER == crmCustomerMstObj.CUS_PK && ss.CDD_ART_WORK == null);
                                            //if (crmCustDocumentDtlObj == null)
                                            //{
                                            //    crmCustDocumentDtlObj = new CRM_CUST_DOCUMENT_DTL();
                                            //}
                                            #endregion

                                        }
                                        else
                                        {
                                            //create new instance of customer master 
                                            crmCustomerMstObj = new CRM_CUSTOMER_MST();
                                            crmCustomerMstList = new List<CRM_CUSTOMER_MST>();
                                            #region Seperate File Upload Table- Now it doesn't use
                                            //crmCustDocumentDtlObj = new CRM_CUST_DOCUMENT_DTL();
                                            #endregion

                                        }
                                        //Get values from UI controls to entity
                                        crmCustomerMstObj = (CRM_CUSTOMER_MST)SetUIValuesToObject(ActionsEnum.SAVE, crmCustomerMstObj);
                                        #region Seperate File Upload Table- Now it doesn't use
                                        //if (dicFileDetails != null && dicFileDetails.Count > 0)
                                        //{
                                        //    crmCustDocumentDtlObj = (CRM_CUST_DOCUMENT_DTL)SetUIValuesToObject(ActionsEnum.SAVE, crmCustDocumentDtlObj);
                                        //    crmCustomerMstObj.CRM_CUST_DOCUMENT_DTL.Add(crmCustDocumentDtlObj);
                                        //}
                                        #endregion

                                        if (CurrPK == 0)
                                        {
                                            #region Add Tax Details
                                            if (this.CrmCustTaxDetails != null && CrmCustTaxDetails.Count > 0)
                                            {
                                                crmCustomerMstObj.CRM_CUST_TAX_DTL = new System.Data.Objects.DataClasses.EntityCollection<CRM_CUST_TAX_DTL>();
                                                CrmCustTaxDetails.ForEach(dtl =>
                                                {
                                                    crmCustomerMstObj.CRM_CUST_TAX_DTL.Add(dtl);
                                                });
                                            }

                                            #endregion
                                            crmCustomerMstList.Add(crmCustomerMstObj);
                                        }
                                        else
                                        {
                                            #region Add Tax Details

                                            if (this.CrmCustTaxDetails != null && CrmCustTaxDetails.Count > 0)
                                            {
                                                long[] idArry = CrmCustTaxDetails.Select(x => x.CMT_PK).ToArray();
                                                //long[] tmpArry = crmCustomerMstObj.CRM_CUST_TAX_DTL != null ? crmCustomerMstObj.CRM_CUST_TAX_DTL.Select(x => x.CMT_PK).ToArray() : null;

                                                //long[] 
                                                if (crmCustomerMstObj.CRM_CUST_TAX_DTL != null)
                                                {
                                                    foreach (CRM_CUST_TAX_DTL dtl in crmCustomerMstObj.CRM_CUST_TAX_DTL.Where(x => !idArry.Contains(x.CMT_PK)).ToList())
                                                    {
                                                        crmCustomerMstObj.CRM_CUST_TAX_DTL.Remove(dtl);
                                                    }
                                                }

                                                foreach (CRM_CUST_TAX_DTL dtl in this.CrmCustTaxDetails)
                                                {
                                                    if (dtl.CMT_PK == 0)
                                                    {
                                                        crmCustomerMstObj.CRM_CUST_TAX_DTL.Add(dtl);
                                                    }
                                                    else if (!idArry.Contains(dtl.CMT_PK))
                                                    {
                                                        crmCustomerMstObj.CRM_CUST_TAX_DTL.Remove(dtl);
                                                    }
                                                }
                                            }
                                            else if (this.CrmCustTaxDetails != null && crmCustomerMstObj.CRM_CUST_TAX_DTL.Any())
                                            {
                                                crmCustomerMstObj.CRM_CUST_TAX_DTL.Clear();
                                            }

                                            #endregion
                                        }


                                        //
                                        #region Sp Validation
                                        //call validation SP if any
                                        hdfValdSP = (HiddenField)pnlControls.FindControl("hdf" + "BTN_SUBMIT" + "VALD_SP");
                                        if (hdfValdSP != null && !string.IsNullOrEmpty(hdfValdSP.Value))
                                        {
                                            hdfValdSPParm = (HiddenField)pnlControls.FindControl("hdf" + "BTN_SUBMIT" + "VALD_SP_PARAM");
                                            if (hdfValdSPParm != null && !string.IsNullOrEmpty(hdfValdSPParm.Value))
                                            {
                                                //Get the sp name
                                                spName = hdfValdSP.Value;
                                                // set the parameters of the sp
                                                paramControls = hdfValdSPParm.Value.Split(',');
                                                if (paramControls.Length > 0)
                                                {
                                                    methodParams = new object[paramControls.Length + 2];
                                                    methodParams[0] = CurrPK;
                                                    count = 1;
                                                    foreach (string controlId in paramControls)
                                                    {
                                                        if ((pnlControls.FindControl(controlId)) != null)
                                                        {
                                                            string controlType = pnlControls.FindControl(controlId).GetType().Name;
                                                            if (controlType.Equals("TextBox"))
                                                            {
                                                                TextBox txtBox = (TextBox)pnlControls.FindControl(controlId);
                                                                if (txtBox != null && !string.IsNullOrEmpty(txtBox.Text.Trim()))
                                                                {
                                                                    methodParams[count] = txtBox.Text.Trim();
                                                                }
                                                                else
                                                                {
                                                                    methodParams[count] = null;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                methodParams[count] = null;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            methodParams[count] = null;
                                                        }
                                                        count++;
                                                    }
                                                    methodParams[count] = currentUser.SBUID;
                                                    if (methodParams.Count() > 2)
                                                    {
                                                        commonServiceClient = new CommonService();
                                                        commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                                                        retObj = commonServiceClient.ExecuteSP(spName, methodParams);
                                                        if (retObj != null)
                                                        {
                                                            string retVal = string.Empty;
                                                            string errorMsg = string.Empty;
                                                            ObjectResult objResult = (ObjectResult)retObj;
                                                            foreach (Object srcObj in objResult)
                                                            {
                                                                Type targetTable = srcObj.GetType();
                                                                foreach (PropertyInfo p in targetTable.GetProperties())
                                                                {
                                                                    if (p.Name.Equals("RET_VAL"))
                                                                    {
                                                                        retVal = p.GetValue(srcObj, null).ToString();
                                                                    }
                                                                    else if (p.Name.Equals("RET_TEXT"))
                                                                    {
                                                                        errorMsg = p.GetValue(srcObj, null).ToString();
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                            if (!string.IsNullOrEmpty(retVal))
                                                            {
                                                                if (retVal.Equals("0"))
                                                                {
                                                                    valid = 0;
                                                                    litErrorMsg.Text = GetLocalResourceObject("msg_Error").ToString();

                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDiv('#divWkfSubmit','" + Resources.ErpRes.Submit + "','950','400');", true);
                                                                }
                                                                else if (retVal.Equals("-1"))
                                                                {
                                                                    valid = 0;
                                                                    litErrorMsg.Text = errorMsg;
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDiv('#divWkfSubmit','" + Resources.ErpRes.Submit + "','950','400');", true);
                                                                }
                                                                else if (retVal.Equals("1"))
                                                                {
                                                                    valid = 1;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        //
                                        if (valid == 1)
                                        {
                                            //Save customer master operation
                                            result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                            if (result >= 0) // Success ! re-initialize the page
                                            {
                                                ResetTaxPopUpSessionData();
                                                CurrPK = (int)result;
                                            }
                                        }

                                    }

                                    //else
                                    //{
                                    //    //Get master entity
                                    //    GetFieldValues(ControlsEnum.CUSTOMER);
                                    //    if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                    //    {
                                    //        doSave = false;
                                    //        //Set the UI values to the curresponding entity. If this succesfully completed return true, otherwise return false
                                    //        SaveOrUpdateEntity(crmCustomerMstList[0], entityName);
                                    //        if (doSave)
                                    //        {
                                    //            //Save operation. For saving we will pass only the master entity, ie, Customer master entity. 
                                    //            //It wil save/update/delete the whole subentities which the status is change. 
                                    //            //If saving is succesfully completed return value will be the selected customer master pk
                                    //            result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                    //        }
                                    //    }
                                    //}
                                    if (valid == 1)
                                    {
                                        if (CurrPK >= 0) // Success ! re-initialize the page
                                        {
                                            ResetTaxPopUpSessionData();
                                            //Workflow submission
                                            ucrWrkf.ApplicationID = CurrPK;
                                            ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                            //Do WorkFlow if WorkFlow has Actions
                                            if (ddlWkfAction.Items.Count > 0)
                                            {
                                                action = ddlWkfAction.SelectedItem.ToString();
                                                result = ucrWrkf.DoWorkFlow();

                                                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                                                {
                                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CustomerRegistration);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL + "?Tab=" + tabCode) + "');", true);
                                                }
                                                else
                                                {
                                                    //Show Save success message and reset Customer Entry
                                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CustomerRegistration);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + tabCode) + "');", true);
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            break;
                        #endregion
                        #endregion
                        #region POPUP
                        #region Edit Packing Spec
                        case ActionsEnum.EDIT_PACK_SPEC:
                            result = BusinessLogic.Inventory.PackingMasterBL.SavePackingSpec(this.SelectedPK > 0 ? this.SelectedPK : 0, 0);
                            IsRefExist = result > 0 ? false : true;
                            ddlPackingSpecCtrl = (DropDownList)pnlControls.FindControl("CIM_PACKING_SPEC");
                            if (ddlPackingSpecCtrl.SelectedIndex > 0)
                                packingSpecPk = Convert.ToInt32(ddlPackingSpecCtrl.SelectedItem.Value);
                            GetFieldValues(ControlsEnum.PACKINGSPEC);
                            SetFieldValues(ControlsEnum.PACKINGSPEC);
                            if (IsRefExist)
                            {
                                btnPackingSpecApply.Visible = false;
                                //ddlPackingSpecCtrl.Enabled = false;
                                ddlPackingSpec.Enabled = false;
                            }
                            else
                            {
                                btnPackingSpecApply.Visible = true;
                                //ddlPackingSpecCtrl.Enabled = true;
                                ddlPackingSpec.Enabled = true;
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSearchProducts", "ShowContainerDiv('#divPackingSpec','" + "Edit Packing Spec" + "','500','150');", true);
                            break;
                        #endregion
                        case ActionsEnum.EDITITEM:
                            if (ddlPackingSpec.SelectedValue == null || ddlPackingSpec.SelectedValue == "-1")
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_PackingSpecNotSelected").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg"
                                    , "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                                return;
                            }

                            string pageUrl = GetLocalResourceObject("PackingMasterPageUrl").ToString();

                            BusinessLogic.AccountManagement.UserAuthBL userAuth = new BusinessLogic.AccountManagement.UserAuthBL();
                            int adminDeptPk = 8;
                            UserRightsBO userRights = userAuth.GetUserRights(currentUser.PKUser, pageUrl, currentUser.SBUID, adminDeptPk);

                            if (userRights == null || userRights.Rights == null || userRights.Rights.Count == 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_InvalidUserRights").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg"
                                    , "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                                return;
                            }

                            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetPageServer(pageUrl);
                            if (dt == null || dt.Rows.Count < 1)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_InvalidPageUrl").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg"
                                    , "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                                return;
                            }

                            string pageServer = dt.Rows[0].Field<string>("PAG_SERVER");

                            #region Set Temp Session
                            Session[ERP.Utilities.SessionStrings.PackingPk] = string.IsNullOrWhiteSpace(ddlPackingSpec.SelectedValue) ? 0 : Convert.ToInt32(ddlPackingSpec.SelectedValue);
                            #endregion
                            string packingSpecUrl = string.Format("{0}{1}?Dept=8&{2}={3}&{4}={5}"
                                                                        , pageServer
                                                                        , GetLocalResourceObject("PackingSpecUrl")
                                                                        , QueryStrings.CustPk
                                                                        , this.CurrPK
                                                                        , QueryStrings.BrankPk
                                                                        , this.SelectedPK
                                                                        );

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "NavigateToPackingSpec", "NavigateToPackingSpec('" + packingSpecUrl + "');", true);
                            break;
                        #region Apply Packing Spec
                        case ActionsEnum.APPLYPACKSPEC:
                            if (ddlPackingSpec.SelectedIndex > 0)
                            {
                                ddlPackingSpecCtrl = (DropDownList)pnlControls.FindControl("CIM_PACKING_SPEC");
                                if (ddlPackingSpecCtrl != null)
                                {
                                    ddlPackingSpecCtrl.SelectedIndex = ddlPackingSpecCtrl.Items.IndexOf(ddlPackingSpecCtrl.Items.FindByValue(ddlPackingSpec.SelectedItem.Value.ToString()));
                                    Button btnPackingSpecCtrl = (Button)pnlControls.FindControl("btnCIM_PACKING_SPECAction");
                                    if (btnPackingSpecCtrl != null)
                                    {
                                        btnPackingSpecCtrl.CommandArgument = "CIM_PACKING_SPEC";
                                        ActionHandler(btnPackingSpecCtrl, EventArgs.Empty);
                                        TextBox txtPackingSpecName = (TextBox)pnlControls.FindControl("CIM_PACKING_SPEC_NAME");
                                        if (txtPackingSpecName != null)
                                            txtPackingSpecName.Text = ddlPackingSpec.SelectedItem.Text;
                                    }
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                            }
                            break;
                        #endregion
                        case ActionsEnum.SEARCH:
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSearchProducts", "ShowContainerDiv('#divSearchProducts','" + "Search" + "','950','400');", true);
                            break;
                        #region Tax PopUp
                        case ActionsEnum.TAXPOPUPDISPLAY:
                            CommonService commonServiceProxy = new CommonService();
                            commonServiceProxy = CommonFunctions.InitiateClient(commonServiceProxy);
                            if (commonServiceProxy.CanShowTaxPopUp(CurrPK, SelectedPK, TabCode))
                            {
                                SetFieldValues(ControlsEnum.TAXPOPUP);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','500','300');", true);
                            }
                            else
                            {
                                if (TabCode == TabType.CUS)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg",
                                                               "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_BrandTaxNameAlredyExist").ToString()) + "','" + Resources.ErpRes.Information + "');",
                                                               true);
                                }
                                else if (TabCode == TabType.CIM)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg",
                                                               "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CusTaxNameAlredyExist").ToString()) + "','" + Resources.ErpRes.Information + "');",
                                                               true);
                                }
                            }
                            break;
                        case ActionsEnum.TAXADD:
                            bool errorTaxNameAlreadyExist = false;
                            // string taxName = ddlPopupTaxType.SelectedItem.Text;

                            byte selectedTaxCategoryType = Convert.ToByte(ddlPopupTaxType.SelectedValue);

                            foreach (CRM_CUST_TAX_DTL tax in this.CrmCustTaxDetailsTemp)
                            {
                                if (tax.CMT_TAX == selectedTaxCategoryType)
                                {
                                    errorTaxNameAlreadyExist = true;
                                }
                            }

                            if (!errorTaxNameAlreadyExist)
                            {
                                //byte taxCategory = Convert.ToByte(hdfTaxCategory.);
                                this.CrmCustTaxDetailsTemp.Add(new CRM_CUST_TAX_DTL
                                {
                                    CMT_PK = 0,
                                    CMT_CUSTOMER = this.TabCode == TabType.CUS ? CurrPK : (int?)null,
                                    CMT_CUST_ITEM = this.TabCode == TabType.CIM ? this.SelectedPK : (int?)null,
                                    CMT_TAX = selectedTaxCategoryType
                                });


                                SetFieldValues(ControlsEnum.TAXPOPUPGRIDTEMP);
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','500','300');", true);
                            if (errorTaxNameAlreadyExist)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_TaxNameAlreadyExist").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            break;
                        case ActionsEnum.TAXDELETE:
                            if (this.CrmCustTaxDetailsTemp != null)
                            {
                                //invoiceHeaderObj = TempSOInvoiceHeaderSession;
                                HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                                HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxCategory") as HiddenField);
                                if (hdfTaxPK != null)
                                {
                                    int taxPK = string.IsNullOrWhiteSpace(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                    string taxID = string.IsNullOrWhiteSpace(hdfTaxName.Value) ? "0" : hdfTaxName.Value;
                                    int taxCategory = Convert.ToInt32(taxID);
                                    this.CrmCustTaxDetailsTemp.Remove(this.CrmCustTaxDetailsTemp.SingleOrDefault(x => x.CMT_PK == Convert.ToInt32(taxPK) && x.CMT_TAX == taxCategory));
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRIDTEMP);
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','500','300');", true);
                            }
                            break;
                        case ActionsEnum.TAXAPPLY:
                            SetCustomerTax();
                            ResetForm(ControlsEnum.TAXPOPUPGRID);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            break;
                        #endregion
                        #endregion
                        #region Select
                        case ActionsEnum.SELECT:
                            commonServiceClient = new CommonService();
                            commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                            //Get the sp name
                            spName = "SPINV_ITEM_SPEC_GET_FROM_CUST_MAP";
                            DropDownList ddlProduct = (DropDownList)pnlControls.FindControl("CIM_ITEM");
                            ddlProduct.SelectedValue = ucrSearchProducts.ItemPK.ToString();
                            // set the parameters of the sp
                            methodParams = new object[] { ucrSearchProducts.ItemPK };
                            //Execute SP
                            retObj = commonServiceClient.ExecuteSP(spName, methodParams);
                            if (retObj != null)
                            {
                                //SP retuurns Objectresult
                                ObjectResult objResult = (ObjectResult)retObj;
                                count = 0;
                                foreach (Object srcObj in objResult)
                                {
                                    //set the UI controls with the SP return list
                                    GetUIValuesFromObject(ActionsEnum.SELECTEDINDEXCHANGED, srcObj);
                                    count++;
                                }
                                if (count == 0)//If the sp return list is empty
                                {
                                    //Create a new instance of SP Complex type
                                    string namespaceString = "ERPData";
                                    string className = objResult.ElementType.Name;
                                    className = namespaceString + "." + className;
                                    Assembly currentAssembly = Assembly.Load(namespaceString);
                                    Type baseEntity = currentAssembly.GetType(className);
                                    Object entityObj = Activator.CreateInstance(baseEntity, null);
                                    //set the UI controls with the SP Complextype object
                                    GetUIValuesFromObject(ActionsEnum.SELECTEDINDEXCHANGED, entityObj);
                                }
                            }
                            break;
                        #endregion
                        #region ClearSearch
                        case ActionsEnum.CLEARSEARCH:
                            ucrSearchProducts.ResetForm();
                            break;
                        #endregion
                        #region default
                        default:
                            break;
                            #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Resources.Messages.DuplicateItemMsg))
                {
                    litErrorMsg.Text = Resources.Messages.DuplicateItemValidation;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {
                customerRegistrationServiceClient = null;
                commonServiceClient = null;
            }
        }

        private void ResetTaxPopUpSessionData()
        {
            //Reset Tax Details in Session For Popup
            this.CrmCustTaxDetails = null;
            this.CrmCustTaxDetailsTemp = null;
        }
        #region --- For Grid Actions----
        /// <summary>
        /// Handling Grid events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (e.Row.Cells[0].Text.Trim().Equals(string.Empty) || e.Row.Cells[0].Text.Trim().Equals("&nbsp;"))//Check radio button column or not
                    {
                        RadioButton rbtSelect = new RadioButton()
                        {
                            ID = "rbtSelect" + e.Row.RowIndex,
                            GroupName = "SelectOne",
                            CssClass = "rdoSelection"
                        };
                        if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                        {
                            if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString().Equals(TabType.CLST))//if the Master list tab
                            {
                                //set the selected pk to the hidden field
                                rbtSelect.Attributes.Add("onClick", "javascript:ShowSelectedRow(this);");
                            }
                            else
                            {
                                rbtSelect.Attributes.Add("onClick", "GrandScriptUtils.EnableRbtnGrouping(this)");
                            }
                        }
                        else
                        {
                            rbtSelect.Attributes.Add("onClick", "GrandScriptUtils.EnableRbtnGrouping(this)");
                        }
                        e.Row.Cells[0].Controls.Add(rbtSelect);
                        //set the pk of the record to a hidden field and add it to the grid
                        if (!string.IsNullOrEmpty(e.Row.Cells[1].Text))
                        {
                            HiddenField hdfPK = new HiddenField()
                            {
                                ID = "hdfPK" + e.Row.RowIndex,
                                ClientIDMode = ClientIDMode.Static,
                                Value = e.Row.Cells[1].Text
                            };
                            e.Row.Cells[0].Controls.Add(hdfPK);
                        }
                    }

                    if (Session["GridControlsList"] != null)
                    {
                        Dictionary<string, string> dicGridControls = (Dictionary<string, string>)Session["GridControlsList"];
                        foreach (KeyValuePair<string, string> gridControl in dicGridControls)
                        {
                            //e.Row.Cells[columnIndex].Visible = false;
                            string[] splitRowColumn = gridControl.Key.Split(',');

                            int rowIndex = Convert.ToInt32(splitRowColumn[0]);
                            if (rowIndex == e.Row.RowIndex)
                            {
                                int colIndex = Convert.ToInt32(splitRowColumn[1]);
                                string fileName = e.Row.Cells[colIndex].Text.ToString();
                                HtmlAnchor lnkFile = new HtmlAnchor()
                                {
                                    ID = "lnkFile" + e.Row.RowIndex,
                                    HRef = gridControl.Value,
                                    InnerText = fileName,
                                    Target = "_blank"
                                };
                                e.Row.Cells[colIndex].Text = "";
                                e.Row.Cells[colIndex].Controls.Add(lnkFile);
                            }
                        }
                    }

                    //check hidden column list and hide the curresponding column of the grid
                    if (Session["HiddenColumnList"] != null)
                    {
                        List<int> hiddenColumnList = (List<int>)Session["HiddenColumnList"];
                        foreach (int columnIndex in hiddenColumnList)
                        {
                            e.Row.Cells[columnIndex].Visible = false;
                        }
                    }

                    //set the width of the column
                    if (Session["GridColumnWidth"] != null)
                    {
                        Dictionary<int, double> dicGridColumnWidth = (Dictionary<int, double>)Session["GridColumnWidth"];
                        foreach (KeyValuePair<int, double> pair in dicGridColumnWidth)
                        {
                            e.Row.Cells[pair.Key].Width = Unit.Percentage(pair.Value);
                            if (!e.Row.Cells[pair.Key].Text.Trim().Equals(string.Empty) && !e.Row.Cells[pair.Key].Text.Trim().Equals("&nbsp;"))
                            {
                                e.Row.Cells[pair.Key].ToolTip = HttpUtility.HtmlDecode(e.Row.Cells[pair.Key].Text);
                            }
                        }
                    }
                    //set the length of the column Data
                    if (Session["GridColumnLength"] != null)
                    {
                        Dictionary<int, int> dicGridColumnLength = (Dictionary<int, int>)Session["GridColumnLength"];
                        foreach (KeyValuePair<int, int> pair in dicGridColumnLength)
                        {
                            e.Row.Cells[pair.Key].Text = ERP.Utilities.CommonFunctions.GetShortString(e.Row.Cells[pair.Key].Text, pair.Value);
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //Set the Header Text
                    if (admFormTabControlDtlList != null)
                    {
                        for (int colIndex = 0; colIndex < e.Row.Cells.Count; colIndex++)
                        {
                            string hdrText = (e.Row.Cells[colIndex] as DataControlFieldHeaderCell).ContainingField.HeaderText;
                            ADM_FORM_TAB_CONTROL_DTL admFormTabControlDtlObj = admFormTabControlDtlList.AsEnumerable().SingleOrDefault(xx => xx.ACD_CONTROL_ID == hdrText);
                            if (admFormTabControlDtlObj != null)
                            {
                                e.Row.Cells[colIndex].Text = admFormTabControlDtlObj.ACD_NAME;
                                if (!e.Row.Cells[colIndex].Text.Trim().Equals(string.Empty) && !e.Row.Cells[colIndex].Text.Trim().Equals("&nbsp;"))
                                {
                                    e.Row.Cells[colIndex].ToolTip = HttpUtility.HtmlEncode(e.Row.Cells[colIndex].Text);
                                }
                            }
                        }
                    }
                    //Hide the columns
                    if (Session["HiddenColumnList"] != null)
                    {
                        List<int> hiddenColumnList = (List<int>)Session["HiddenColumnList"];
                        foreach (int columnIndex in hiddenColumnList)
                        {
                            e.Row.Cells[columnIndex].Visible = false;
                        }
                    }

                    //set the width of the column
                    if (Session["GridColumnWidth"] != null)
                    {
                        Dictionary<int, double> dicGridColumnWidth = (Dictionary<int, double>)Session["GridColumnWidth"];
                        foreach (KeyValuePair<int, double> pair in dicGridColumnWidth)
                        {
                            e.Row.Cells[pair.Key].Width = Unit.Percentage(pair.Value);
                        }
                    }

                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Page Index Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();

            //GetFieldValues(ControlsEnum.DEFAULT);
            //SetFieldValues(ControlsEnum.DEFAULT);
        }

        #endregion
        #endregion
        #region Helper Methods

        private void SetCustomerTax()
        {
            //GetFieldValues(ControlsEnum.TAXPOPUPGRID);
            if (dtTaxDetails == null)
            {
                GetFieldValues(ControlsEnum.TAXTYPES);
            }
            if (this.CrmCustTaxDetailsTemp != null)
            {
                string compltTx = string.Join(", ", this.CrmCustTaxDetailsTemp
                                    .Select(x =>
                                            x.FIN_TAX_MST != null
                                                ? x.FIN_TAX_MST.TAX_HEAD
                                                : dtTaxDetails.Select("TAX_PK=" + x.CMT_TAX.ToString()).FirstOrDefault()["TAX_HEAD"]
                                        )
                                    .ToArray());
                TextBox txtAssociated = (TextBox)pnlControls.FindControl(hdfPopUpAssociatedControl.Value);
                txtAssociated.Text = compltTx;
                CrmCustTaxDetails = this.CrmCustTaxDetailsTemp;
                this.CrmCustTaxDetailsTemp = null;
            }
        }

        private void ResetForm(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.TAXPOPUPGRID:
                    grdTaxDetails.DataSource = null;
                    grdTaxDetails.DataBind();
                    break;
            }
        }

        /// <summary>
        /// Method for Load the dynamic controls
        /// </summary>
        public void LoadControls()
        {
            try
            {
                pnlControls.Controls.Clear();
                BindControls();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        private byte GetCustomerListActiveStatus()
        {
            if (!IsCustomerStatusFilterEnabled())
            {
                Session[CustomerStatusFilterSessionKey] = null;
                return Convert.ToByte(DbActiveStatus.ACTIVE);
            }

            string selectedStatus = Request.Form[CustomerStatusFilterControlId];
            if (!string.IsNullOrEmpty(selectedStatus))
            {
                Session[CustomerStatusFilterSessionKey] = selectedStatus;
            }
            else if (Session[CustomerStatusFilterSessionKey] != null)
            {
                selectedStatus = Session[CustomerStatusFilterSessionKey].ToString();
            }

            byte activeStatus;
            if (byte.TryParse(selectedStatus, out activeStatus)
                && (activeStatus == Convert.ToByte(DbActiveStatus.ACTIVE)
                    || activeStatus == Convert.ToByte(DbActiveStatus.INACTIVE)))
            {
                return activeStatus;
            }

            return Convert.ToByte(DbActiveStatus.ACTIVE);
        }

        private bool IsCustomerStatusFilterEnabled()
        {
            object resourceValue = null;
            try
            {
                resourceValue = GetGlobalResourceObject("ConfigurationsRes", "ShowActiveInActiveFilterInCusReg")
                    ?? GetGlobalResourceObject("ConfigurationsRes", "ShowActiveInActive filterInCusReg");
            }
            catch
            {
                resourceValue = null;
            }

            string flagValue = resourceValue == null ? string.Empty : resourceValue.ToString();
            return flagValue.Equals("true", StringComparison.OrdinalIgnoreCase)
                || flagValue == CommonConstants.SELECT_VALUE_ONE;
        }

        private void AddCustomerStatusFilter(HtmlGenericControl div)
        {
            if (!IsCustomerStatusFilterEnabled())
            {
                return;
            }

            Label lblStatus = new Label()
            {
                ID = "lbl" + CustomerStatusFilterControlId,
                Text = "Status",
                AssociatedControlID = CustomerStatusFilterControlId,
                ClientIDMode = ClientIDMode.Static,
                CssClass = "customer-status-filter-label"
            };

            DropDownList ddlStatus = new DropDownList()
            {
                ID = CustomerStatusFilterControlId,
                ClientIDMode = ClientIDMode.Static,
                AutoPostBack = true,
                CssClass = "customer-status-filter"
            };
            ddlStatus.SelectedIndexChanged += new EventHandler(ActionHandler);
            ddlStatus.Items.Add(new ListItem("Active", Convert.ToByte(DbActiveStatus.ACTIVE).ToString()));
            ddlStatus.Items.Add(new ListItem("Inactive", Convert.ToByte(DbActiveStatus.INACTIVE).ToString()));
            ddlStatus.SelectedValue = GetCustomerListActiveStatus().ToString();

            div.Controls.Add(lblStatus);
            div.Controls.Add(ddlStatus);
        }

        private void RebindCustomerListingGrid()
        {
            if (TabCode != TabType.CLST)
            {
                return;
            }

            GridView customerListGrid = pnlControls.FindControl(CustomerListGridControlId) as GridView;
            HiddenField hdfGrid = pnlControls.FindControl("hdf" + CustomerListGridControlId) as HiddenField;
            int controlPK;
            if (customerListGrid != null && hdfGrid != null && int.TryParse(hdfGrid.Value, out controlPK))
            {
                BindGrid(controlPK, GetLocalResourceObject("CRM_CUSTOMER_MST").ToString(), customerListGrid);
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode, Object srcObj)
        {
            object returnObj;
            returnObj = null;
            string value;
            value = string.Empty;
            bool hasUIValue = false;
            ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
            try
            {
                switch (mode)
                {
                    case ActionsEnum.SAVE:
                        Type targetTable = srcObj.GetType();
                        objBrandItemInsert = new BrandItemInsertBO();
                        #region Seperate File Upload Table- Now it doesn't use
                        //if (!targetTable.Name.Equals("CRM_CUST_DOCUMENT_DTL"))
                        //{
                        #endregion

                        foreach (PropertyInfo p in targetTable.GetProperties())
                        {
                            if (p.CanWrite)
                            {
                                string controlID = p.Name;
                                #region Seperate File Upload Table- Now it doesn't use
                                //if (p.Name.EndsWith("_FILE_PATH"))//For fileupload path
                                //{
                                //    if (!string.IsNullOrEmpty(attachmentFilePath))
                                //    {
                                //        SetValue(srcObj, p, attachmentFilePath);
                                //    }
                                //}
                                #endregion
                                //Check the control is in UI, if it is Get the type of the control
                                HiddenField hdfType = (HiddenField)pnlControls.FindControl("hdf" + controlID + "Type");
                                if (p.Name.Contains("BIZUNIT"))
                                    SetValue(srcObj, p, currentUser.SBUID.ToString());
                                if (hdfType != null && !string.IsNullOrEmpty(hdfType.Value))
                                {
                                    #region case
                                    switch ((ControlTypes)(Enum.Parse(typeof(ControlTypes), hdfType.Value)))
                                    {
                                        #region Text
                                        case ControlTypes.Text:
                                            TextBox txtCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                            if (txtCtrlId != null)
                                            {
                                                SetValue(srcObj, p, txtCtrlId.Text.Trim());
                                                if (IsBrandItemInsert)
                                                {
                                                    if (p.Name == "CIM_BRAND_CODE")
                                                    {
                                                        objBrandItemInsert.CIM_BRAND_CODE = txtCtrlId.Text;
                                                    }
                                                    if (p.Name == "CIM_BRAND_NAME")
                                                    {
                                                        objBrandItemInsert.CIM_BRAND_NAME = txtCtrlId.Text;
                                                    }
                                                }
                                                hasUIValue = true;
                                            }
                                            break;
                                        #endregion
                                        #region Password Text
                                        case ControlTypes.Password:
                                            TextBox txtPwdCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                            if (txtPwdCtrlId != null)
                                            {
                                                string password = HttpUtility.HtmlEncode(crypto.EncryptString(txtPwdCtrlId.Text.Trim(), System.Configuration.ConfigurationManager.AppSettings["salt"]));//Password Encryption
                                                SetValue(srcObj, p, password);
                                                hasUIValue = true;
                                            }
                                            break;
                                        #endregion
                                        #region HourText
                                        case ControlTypes.HourText:
                                            TextBox hrtCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                            if (hrtCtrlId != null)
                                            {
                                                if (hrtCtrlId.Text != string.Empty)
                                                {
                                                    string hrt = hrtCtrlId.Text.Trim().Replace('_', '0');
                                                    string[] time = hrt.Split(':');
                                                    value = ((string.IsNullOrEmpty(time[0]) ? 0 : (Convert.ToInt32(time[0]) * 60)) + (string.IsNullOrEmpty(time[1]) ? 0 : Convert.ToInt32(time[1]))).ToString();
                                                    SetValue(srcObj, p, value);
                                                    hasUIValue = true;
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region Date
                                        case ControlTypes.Date:
                                            TextBox dateCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                            if (dateCtrlId != null)
                                            {
                                                string date = string.IsNullOrEmpty(dateCtrlId.Text) ? string.Empty : dateCtrlId.Text.Trim();
                                                if (date != string.Empty)
                                                {
                                                    SetValue(srcObj, p, date);
                                                    hasUIValue = true;
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region DateRange
                                        case ControlTypes.DateRange:
                                            TextBox dateRangeCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                            if (dateRangeCtrlId != null)
                                            {
                                                string date = string.IsNullOrEmpty(dateRangeCtrlId.Text) ? string.Empty : dateRangeCtrlId.Text.Trim();
                                                if (date != string.Empty)
                                                {
                                                    SetValue(srcObj, p, date);
                                                    hasUIValue = true;
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region DateTime
                                        case ControlTypes.DateTime:
                                            TextBox dateTimeCtrlId1 = (TextBox)pnlControls.FindControl(controlID);
                                            TextBox dateTimeCtrlId2 = (TextBox)pnlControls.FindControl(controlID + "_Time");
                                            if (dateTimeCtrlId1 != null && dateTimeCtrlId2 != null)
                                            {
                                                string dateTime = string.IsNullOrEmpty(dateTimeCtrlId1.Text) ? string.Empty : dateTimeCtrlId1.Text.Trim();
                                                dateTime = dateTime + " " + (string.IsNullOrEmpty(dateTimeCtrlId2.Text) ? string.Empty : dateTimeCtrlId2.Text.Trim());
                                                if (dateTime != string.Empty)
                                                {
                                                    SetValue(srcObj, p, dateTime);
                                                    hasUIValue = true;
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region TimePicker
                                        case ControlTypes.TimePicker:
                                            TextBox txtTime = (TextBox)pnlControls.FindControl(controlID);
                                            if (txtTime != null && !string.IsNullOrEmpty(txtTime.Text.Trim()))
                                            {
                                                value = string.IsNullOrEmpty(txtTime.Text.Trim()) ? string.Empty : txtTime.Text.Trim();
                                                SetValue(srcObj, p, value);
                                                hasUIValue = true;
                                            }
                                            break;
                                        #endregion
                                        #region Numeric
                                        case ControlTypes.Numeric:
                                            TextBox txtNumCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                            if (txtNumCtrlId != null)
                                            {
                                                string numVal = string.IsNullOrEmpty(txtNumCtrlId.Text.Trim()) ? "0" : txtNumCtrlId.Text.Trim();
                                                SetValue(srcObj, p, numVal);
                                                hasUIValue = true;
                                            }
                                            break;
                                        #endregion
                                        #region DropDown
                                        case ControlTypes.DropDown:
                                            DropDownList ddlCtrlId = (DropDownList)pnlControls.FindControl(controlID);
                                            if (ddlCtrlId != null)
                                            {
                                                int intOut;
                                                if (Int32.TryParse(ddlCtrlId.SelectedValue, out intOut))
                                                {
                                                    if (Convert.ToInt32(ddlCtrlId.SelectedValue) > 0)
                                                    {
                                                        SetValue(srcObj, p, ddlCtrlId.SelectedValue);
                                                        hasUIValue = true;
                                                    }
                                                    else
                                                    {
                                                        //p.SetValue(srcObj, null, null);
                                                        HiddenField hdfRelId = (HiddenField)pnlControls.FindControl("hdf" + controlID + "HasRelatedId");
                                                        if (hdfRelId != null)
                                                        {
                                                            p.SetValue(srcObj, null, null);
                                                        }
                                                        else
                                                        {
                                                            p.SetValue(srcObj, null, null);
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region Label
                                        case ControlTypes.Label:
                                            Label lblCtrlId = (Label)pnlControls.FindControl(controlID);
                                            if (lblCtrlId != null)
                                            {
                                                SetValue(srcObj, p, lblCtrlId.Text.Trim());
                                            }
                                            break;
                                        #endregion
                                        #region HiddenField
                                        case ControlTypes.HiddenField:
                                            HiddenField hdfCtrlId = (HiddenField)pnlControls.FindControl(controlID);
                                            if (hdfCtrlId != null)
                                            {
                                                if (hdfCtrlId.ID.EndsWith("MOD_BY"))
                                                {
                                                    SetValue(srcObj, p, currentUser.PKUser.ToString());
                                                    hasUIValue = true;
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("MOD_DT"))
                                                {
                                                    SetValue(srcObj, p, DateTime.Now.ToString());
                                                    hasUIValue = true;
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("ACTIVE"))
                                                {
                                                    SetValue(srcObj, p, "1");
                                                    hasUIValue = true;
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("STATUS"))
                                                {
                                                    SetValue(srcObj, p, string.IsNullOrEmpty(hdfCtrlId.Value) ? "0" : hdfCtrlId.Value);
                                                    hasUIValue = true;
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("BIZUNIT"))
                                                {
                                                    SetValue(srcObj, p, currentUser.SBUID.ToString());
                                                    hasUIValue = true;
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("CRTD_BY"))
                                                {
                                                    SetValue(srcObj, p, currentUser.PKUser.ToString());
                                                    hasUIValue = true;
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("CRTD_DT"))
                                                {
                                                    SetValue(srcObj, p, DateTime.Now.ToString());
                                                    hasUIValue = true;
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("DELETED"))
                                                {
                                                    SetValue(srcObj, p, false.ToString());
                                                    hasUIValue = true;
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(hdfCtrlId.Value))
                                                    {
                                                        SetValue(srcObj, p, hdfCtrlId.Value);
                                                        hasUIValue = true;
                                                    }
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region TextArea
                                        case ControlTypes.TextArea:
                                            TextBox txtAreaCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                            if (txtAreaCtrlId != null)
                                            {
                                                SetValue(srcObj, p, txtAreaCtrlId.Text.Trim());
                                                hasUIValue = true;
                                            }
                                            break;
                                        #endregion
                                        #region FileUpload
                                        case ControlTypes.FileUpload:
                                            FileUpload fileUploadCtrlId = (FileUpload)pnlControls.FindControl(controlID);
                                            if (fileUploadCtrlId != null)
                                            {

                                                HttpPostedFile po = fileUploadCtrlId.PostedFile;
                                                string savePath;
                                                FileInfo tempFileInfoObj;
                                                string attachmentFileFormat;

                                                FileInfo attachedFileInfo;
                                                if (fileUploadCtrlId.HasFile)
                                                {
                                                    SetValue(srcObj, p, fileUploadCtrlId.FileName);
                                                    hasUIValue = true;
                                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                                    {
                                                        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                                        if (!Directory.Exists(savePath))
                                                            Directory.CreateDirectory(savePath);
                                                        savePath = savePath + "\\";
                                                    }
                                                    else
                                                    {
                                                        savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower(); //savePath = Server.MapPath("../Upload");
                                                    }

                                                    tempFileInfoObj = new FileInfo(fileUploadCtrlId.PostedFile.FileName);
                                                    //if (!Directory.Exists(savePath))
                                                    //    Directory.CreateDirectory(savePath);
                                                    attachmentFileFormat = tempFileInfoObj.Extension;
                                                    AttachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                                    string attachmentFilePath = savePath + AttachmentFileName;
                                                    attachedFileInfo = new FileInfo(attachmentFilePath);
                                                    HttpContext.Current.Request.Files[0].SaveAs(attachedFileInfo.FullName);
                                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                                    {
                                                        attachmentFilePath = "~/Upload/" + AttachmentFileName;
                                                    }
                                                    else
                                                    {
                                                        attachmentFilePath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + AttachmentFileName;
                                                    }

                                                    ////Save Filepath
                                                    string filepath = p.Name.Replace("FILE_NAME", "FILE_PATH");
                                                    PropertyInfo pFilePath = targetTable.GetProperties().AsEnumerable().SingleOrDefault(aa => aa.Name == filepath);
                                                    if (pFilePath != null)
                                                    {
                                                        SetValue(srcObj, pFilePath, attachmentFilePath);
                                                    }
                                                }
                                                else
                                                {
                                                    HtmlAnchor htmlAnchorCtrlId = (HtmlAnchor)pnlControls.FindControl("anchor" + controlID);
                                                    HiddenField hdfFileNameCtrlId = (HiddenField)pnlControls.FindControl("hdf" + controlID + "FileName");
                                                    if (hdfFileNameCtrlId != null && htmlAnchorCtrlId != null)
                                                    {
                                                        if (!string.IsNullOrEmpty(hdfFileNameCtrlId.Value) && !string.IsNullOrEmpty(htmlAnchorCtrlId.HRef))
                                                        {
                                                            SetValue(srcObj, p, hdfFileNameCtrlId.Value);
                                                            hasUIValue = true;
                                                            ////Save Filepath
                                                            string filepath = p.Name.Replace("FILE_NAME", "FILE_PATH");
                                                            PropertyInfo pFilePath = targetTable.GetProperties().AsEnumerable().SingleOrDefault(aa => aa.Name == filepath);
                                                            if (pFilePath != null)
                                                            {
                                                                SetValue(srcObj, pFilePath, htmlAnchorCtrlId.HRef);
                                                            }
                                                        }
                                                    }
                                                }
                                                if (AttachmentFileName == Guid.Empty.ToString() || AttachmentFileName == string.Empty)
                                                {
                                                    AttachmentFileName = string.Empty;
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region CheckBox
                                        case ControlTypes.CheckBox:
                                            CheckBox chkCtrlId = (CheckBox)pnlControls.FindControl(controlID);
                                            if (chkCtrlId != null)
                                            {
                                                if (p.PropertyType == typeof(bool))
                                                {
                                                    value = chkCtrlId.Checked == true ? "True" : "False";
                                                }
                                                else if (p.PropertyType == typeof(byte) || p.PropertyType == typeof(byte?))
                                                {
                                                    value = chkCtrlId.Checked == true ? "1" : "0";
                                                }
                                                else if (p.PropertyType == typeof(Int16) || p.PropertyType == typeof(Int16?))
                                                {
                                                    value = chkCtrlId.Checked == true ? "1" : "0";
                                                }
                                                SetValue(srcObj, p, value);

                                            }
                                            break;
                                        #endregion
                                        #region  Default
                                        default:
                                            break;
                                            #endregion

                                    }
                                    #endregion
                                }
                            }
                        }
                        objBrandItemInsert.CIM_CUSTOMER_PK = CurrPK.ToString();
                        objBrandItemInsert.CIM_BIZUNIT = currentUser.SBUID;

                        #region Seperate File Upload Table- Now it doesn't use
                        //}
                        //else
                        //{
                        //    if (dicFileDetails != null && dicFileDetails.Count > 0)
                        //    {
                        //        foreach (PropertyInfo p in targetTable.GetProperties())
                        //        {
                        //            if (p.CanWrite)
                        //            {
                        //                string controlID = p.Name;
                        //                foreach (KeyValuePair<string, string> fileInfo in dicFileDetails)
                        //                {
                        //                    if (fileInfo.Key.Equals(p.Name))
                        //                    {
                        //                        SetValue(srcObj, p, fileInfo.Value);
                        //                        hasUIValue = true;
                        //                        break;
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        dicFileDetails = new Dictionary<string, string>();
                        //        dicFileDetails.Add("CDD_CUSTOMER", CurrPK.ToString());
                        //        dicFileDetails.Add("CDD_SEQUENCE", "1");
                        //        dicFileDetails.Add("CDD_TITLE", "");
                        //        dicFileDetails.Add("CDD_FILE_NAME", "");
                        //        dicFileDetails.Add("CDD_ACTIVE", "1");
                        //        dicFileDetails.Add("CDD_UPLOAD_BY", currentUser.PKUser.ToString());
                        //        dicFileDetails.Add("CDD_UPLOAD_ON", DateTime.Now.ToString());
                        //        dicFileDetails.Add("CDD_MOD_BY", currentUser.PKUser.ToString());
                        //        dicFileDetails.Add("CDD_MOD_ON", DateTime.Now.ToString());
                        //        foreach (PropertyInfo p in targetTable.GetProperties())
                        //        {
                        //            if (p.CanWrite)
                        //            {
                        //                string controlID = p.Name;
                        //                foreach (KeyValuePair<string, string> fileInfo in dicFileDetails)
                        //                {
                        //                    if (fileInfo.Key.Equals(p.Name))
                        //                    {
                        //                        SetValue(srcObj, p, fileInfo.Value);
                        //                        hasUIValue = true;
                        //                        break;
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                        break;

                    default:
                        break;
                }
                //if (hasUIValue)
                //{

                returnObj = srcObj;
                return returnObj;
                //}
                //else
                //{
                //    return null;
                //}
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Set Values To Object
        /// </summary>
        /// <param name="src"></param>
        /// <param name="p"></param>
        /// <param name="value"></param>
        private void SetValue(object src, PropertyInfo p, string value)
        {
            Type ptype = p.PropertyType;
            if (ptype == typeof(byte) || ptype == typeof(byte?))
                p.SetValue(src, Convert.ToByte(value), null);
            if (ptype == typeof(string))
                if (!string.IsNullOrEmpty(value))
                    p.SetValue(src, HttpUtility.HtmlEncode(value), null);
                else
                    p.SetValue(src, null, null);
            else if (ptype == typeof(int) || ptype == typeof(int?))
                p.SetValue(src, Convert.ToInt32(value), null);
            else if (ptype == typeof(Int64) || ptype == typeof(Int64?))
                p.SetValue(src, Convert.ToInt64(value), null);
            else if (ptype == typeof(Int32) || ptype == typeof(Int32?))
                p.SetValue(src, Convert.ToInt32(value), null);
            else if (ptype == typeof(short) || ptype == typeof(short?))
                if (!string.IsNullOrEmpty(value))
                    p.SetValue(src, Convert.ToInt16(value), null);
                else
                    p.SetValue(src, null, null);
            else if (ptype == typeof(float) || ptype == typeof(float?))
                p.SetValue(src, float.Parse(value), null);
            else if (ptype == typeof(Double) || ptype == typeof(Double?))
                p.SetValue(src, Double.Parse(value), null);
            else if (ptype == typeof(decimal) || ptype == typeof(decimal?))
                if (!string.IsNullOrEmpty(value))
                    p.SetValue(src, Convert.ToDecimal(value), null);
                else
                    p.SetValue(src, null, null);
            else if (ptype == typeof(bool) || ptype == typeof(bool?))
                p.SetValue(src, Convert.ToBoolean(value), null);
            else if (ptype == typeof(DateTime) || ptype == typeof(DateTime?))
                p.SetValue(src, Convert.ToDateTime(value), null);

        }

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ActionsEnum mode, Object srcObj)
        {
            try
            {
                Type targetTable;
                targetTable = srcObj.GetType();
                ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
                foreach (PropertyInfo p in targetTable.GetProperties())
                {
                    if (p.CanWrite)
                    {
                        string controlID = p.Name;
                        //Check the control is in UI, if it is Get the type of the control
                        HiddenField hdfType = (HiddenField)pnlControls.FindControl("hdf" + controlID + "Type");
                        if (hdfType != null && !string.IsNullOrEmpty(hdfType.Value))
                        {
                            #region case
                            switch ((ControlTypes)(Enum.Parse(typeof(ControlTypes), hdfType.Value)))
                            {
                                #region Fill
                                #region Text
                                case ControlTypes.Text:
                                    TextBox txtCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                    if (txtCtrlId != null)
                                    {
                                        txtCtrlId.Text = p.GetValue(srcObj, null) == null ? string.Empty : HttpUtility.HtmlDecode(p.GetValue(srcObj, null).ToString());
                                    }
                                    break;
                                #endregion
                                #region Password Text
                                case ControlTypes.Password:
                                    TextBox txtPwdCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                    if (txtPwdCtrlId != null)
                                    {

                                        string password = p.GetValue(srcObj, null) == null ? string.Empty : HttpUtility.HtmlDecode(p.GetValue(srcObj, null).ToString());
                                        password = crypto.DecryptString(HttpUtility.HtmlDecode(password), System.Configuration.ConfigurationManager.AppSettings["salt"]);
                                        txtPwdCtrlId.Attributes.Add("value", password);
                                    }
                                    break;
                                #endregion
                                #region HourText
                                case ControlTypes.HourText:
                                    TextBox hrtCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                    if (hrtCtrlId != null)
                                    {
                                        int hour = p.GetValue(srcObj, null) == null ? 0 : p.GetValue(srcObj, null).ToString() == string.Empty ? 0 : Convert.ToInt32(p.GetValue(srcObj, null)) / 60;
                                        int min = p.GetValue(srcObj, null) == null ? 0 : p.GetValue(srcObj, null).ToString() == string.Empty ? 0 : Convert.ToInt32(p.GetValue(srcObj, null)) % 60;
                                        hrtCtrlId.Text = hour + min > 0 ? hour + ":" + min : "000:00";
                                    }
                                    break;
                                #endregion
                                #region Date
                                case ControlTypes.Date:
                                    TextBox dateCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                    if (dateCtrlId != null)
                                    {
                                        string ss = Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                                        dateCtrlId.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).Equals(string.Empty) ? string.Empty : Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                                    }
                                    break;
                                #endregion
                                #region DateRange
                                case ControlTypes.DateRange:
                                    TextBox dateRangeCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                    if (dateRangeCtrlId != null)
                                    {
                                        string ss = Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                                        dateRangeCtrlId.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).Equals(string.Empty) ? string.Empty : Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                                    }
                                    break;
                                #endregion
                                #region DateTime
                                case ControlTypes.DateTime:
                                    TextBox dateTimeCtrlId1 = (TextBox)pnlControls.FindControl(controlID);
                                    TextBox dateTimeCtrlId2 = (TextBox)pnlControls.FindControl(controlID + "_Time");
                                    if (dateTimeCtrlId1 != null)
                                    {
                                        dateTimeCtrlId1.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).Equals(string.Empty) ? string.Empty : Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                                    }
                                    if (dateTimeCtrlId2 != null)
                                    {
                                        dateTimeCtrlId2.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).Equals(string.Empty) ? string.Empty : Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.TimeFormat);
                                    }
                                    break;
                                #endregion
                                #region TimePicker
                                case ControlTypes.TimePicker:
                                    TextBox txtTime = (TextBox)pnlControls.FindControl(controlID);
                                    if (txtTime != null)
                                    {
                                        txtTime.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).Equals(string.Empty) ? string.Empty : Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.TimeFormat);
                                    }
                                    break;
                                #endregion
                                #region Numeric
                                case ControlTypes.Numeric:
                                    TextBox txtNumCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                    if (txtNumCtrlId != null)
                                    {
                                        string decValInDB = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).ToString();
                                        if (!string.IsNullOrEmpty(decValInDB))
                                        {
                                            HiddenField hdfDecimalPart = (HiddenField)pnlControls.FindControl("hdf" + controlID + "DecimalPart");
                                            if (hdfDecimalPart != null)
                                            {
                                                int decVal = string.IsNullOrEmpty(hdfDecimalPart.Value) ? 0 : Convert.ToInt32(hdfDecimalPart.Value);
                                                if (decVal == 0)
                                                {
                                                    txtNumCtrlId.Text = decValInDB;
                                                }
                                                else
                                                {
                                                    decimal val = Convert.ToDecimal(decValInDB);
                                                    txtNumCtrlId.Text = Math.Round(val, decVal).ToString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            txtNumCtrlId.Text = string.Empty;
                                        }
                                    }
                                    break;
                                #endregion
                                #region DropDown
                                case ControlTypes.DropDown:
                                    DropDownList ddlCtrlId = (DropDownList)pnlControls.FindControl(controlID);
                                    if (ddlCtrlId != null)
                                    {
                                        ddlCtrlId.SelectedValue = p.GetValue(srcObj, null) == null ? CommonConstants.SELECTVAL : p.GetValue(srcObj, null).ToString();
                                    }
                                    break;
                                #endregion
                                #region Label
                                case ControlTypes.Label:
                                    Label lblCtrlId = (Label)pnlControls.FindControl(controlID);
                                    if (lblCtrlId != null)
                                    {
                                        lblCtrlId.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).ToString();
                                    }
                                    break;
                                #endregion
                                #region HiddenField
                                case ControlTypes.HiddenField:
                                    HiddenField hdfCtrlId = (HiddenField)pnlControls.FindControl(controlID);
                                    if (hdfCtrlId != null)
                                    {
                                        hdfCtrlId.Value = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).ToString();
                                    }
                                    break;
                                #endregion
                                #region TextArea
                                case ControlTypes.TextArea:
                                    TextBox txtAreaCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                    if (txtAreaCtrlId != null)
                                    {
                                        txtAreaCtrlId.Text = p.GetValue(srcObj, null) == null ? string.Empty : HttpUtility.HtmlDecode(p.GetValue(srcObj, null).ToString());
                                    }
                                    break;
                                #endregion
                                #region FileUpload
                                case ControlTypes.FileUpload:
                                    FileUpload fileUploadCtrlId = (FileUpload)pnlControls.FindControl(controlID);
                                    if (fileUploadCtrlId != null)
                                    {
                                        HtmlAnchor htmlAnchorCtrlId = (HtmlAnchor)pnlControls.FindControl("anchor" + controlID);
                                        HiddenField hdfFileNameCtrlId = (HiddenField)pnlControls.FindControl("hdf" + controlID + "FileName");
                                        if (hdfFileNameCtrlId != null && htmlAnchorCtrlId != null)
                                        {
                                            hdfFileNameCtrlId.Value = p.GetValue(srcObj, null) == null ? string.Empty : (string)p.GetValue(srcObj, null);
                                            htmlAnchorCtrlId.Attributes.Add("Title", hdfFileNameCtrlId.Value);
                                            string filepath = p.Name.Replace("FILE_NAME", "FILE_PATH");
                                            PropertyInfo pFilePath = targetTable.GetProperties().AsEnumerable().SingleOrDefault(aa => aa.Name == filepath);
                                            if (pFilePath != null)
                                            {
                                                htmlAnchorCtrlId.HRef = pFilePath.GetValue(srcObj, null) == null ? string.Empty : (string)pFilePath.GetValue(srcObj, null);
                                                if (!string.IsNullOrEmpty(htmlAnchorCtrlId.HRef))
                                                {
                                                    htmlAnchorCtrlId.Visible = true;
                                                    Button btnFileUpload = (Button)pnlControls.FindControl("btn" + controlID);
                                                    if (btnFileUpload != null)
                                                    {
                                                        //btnFileUpload.Visible = true;
                                                        //fileUploadCtrlId.Visible = false;
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideFileUpload", "$(document).ready(function(){return hideFileUpload();});", true);
                                                    }
                                                    HiddenField hdfValidationGroup = (HiddenField)pnlControls.FindControl("hdf" + controlID + "ValidationGroup");
                                                    if (hdfValidationGroup != null && !string.IsNullOrEmpty(hdfValidationGroup.Value))
                                                    {
                                                        string[] valGroup = hdfValidationGroup.Value.Split(',');
                                                        for (int valCount = 0; valCount < valGroup.Length; valCount++)
                                                        {
                                                            RequiredFieldValidator vrfFileUpload = (RequiredFieldValidator)pnlControls.FindControl("vrf" + valCount + controlID);
                                                            if (vrfFileUpload != null)
                                                            {
                                                                vrfFileUpload.Enabled = false;
                                                            }
                                                        }


                                                    }

                                                }
                                            }
                                        }
                                    }
                                    break;
                                #endregion
                                #region CheckBox
                                case ControlTypes.CheckBox:
                                    CheckBox chkCtrlId = (CheckBox)pnlControls.FindControl(controlID);
                                    if (chkCtrlId != null)
                                    {
                                        if (p.GetValue(srcObj, null) == null)
                                        {
                                            chkCtrlId.Checked = false;
                                        }
                                        else
                                        {
                                            if (p.PropertyType == typeof(byte) || p.PropertyType == typeof(byte?))
                                            {
                                                if (Convert.ToByte(p.GetValue(srcObj, null)) == 1)
                                                {
                                                    chkCtrlId.Checked = true;
                                                }
                                                else
                                                {
                                                    chkCtrlId.Checked = false;
                                                }
                                            }
                                            else if (p.PropertyType == typeof(Int16) || p.PropertyType == typeof(Int16?))
                                            {
                                                short intOut;
                                                if (Int16.TryParse(p.GetValue(srcObj, null).ToString(), out intOut))
                                                {
                                                    if (intOut == 1)
                                                    {
                                                        chkCtrlId.Checked = true;
                                                    }
                                                    else
                                                    {
                                                        chkCtrlId.Checked = false;
                                                    }
                                                }
                                            }
                                            else if (p.PropertyType == typeof(bool) || p.PropertyType == typeof(bool?))
                                            {
                                                if ((bool)p.GetValue(srcObj, null))
                                                {
                                                    chkCtrlId.Checked = true;
                                                }
                                                else
                                                {
                                                    chkCtrlId.Checked = false;
                                                }
                                            }
                                            else
                                            {
                                                chkCtrlId.Checked = false;
                                            }
                                        }
                                    }
                                    break;
                                #endregion
                                #region  Default
                                default:
                                    break;
                                    #endregion
                                    #endregion
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hide File Anchor controls If any
        /// </summary>
        private void HideFileAnchorControlsIfAny()
        {
            if (Session["fileanchorcontrollist"] != null)
            {
                List<string> fileAnchorControlList = (List<string>)Session["fileanchorcontrollist"];
                if (fileAnchorControlList != null && fileAnchorControlList.Count > 0)
                {
                    foreach (string anchorControlId in fileAnchorControlList)
                    {
                        HtmlAnchor anchorCtrol = (HtmlAnchor)pnlControls.FindControl(anchorControlId);
                        if (anchorCtrol != null)
                        {
                            anchorCtrol.Visible = false;
                            string controlID = anchorControlId.Replace("anchor", "");
                            HiddenField hdfValidationGroup = (HiddenField)pnlControls.FindControl("hdf" + controlID + "ValidationGroup");
                            if (hdfValidationGroup != null && !string.IsNullOrEmpty(hdfValidationGroup.Value))
                            {
                                string[] valGroup = hdfValidationGroup.Value.Split(',');
                                for (int valCount = 0; valCount < valGroup.Length; valCount++)
                                {
                                    RequiredFieldValidator vrfFileUpload = (RequiredFieldValidator)pnlControls.FindControl("vrf" + valCount + controlID);
                                    if (vrfFileUpload != null)
                                    {
                                        vrfFileUpload.Enabled = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Bind the dynamic controls 
        /// </summary>
        /// <returns></returns>  
        private void BindControls()
        {
            Panel pnl = new Panel();
            int Cols;
            string PageScript = "";
            string divGroupStyle = "";
            string divColStyle = "";
            short tabIndex = 1;
            HiddenField hdnType;
            TableCell tempTableCell;
            HtmlGenericControl tempDiv;
            int tempSequence = 0;
            Dictionary<int, string> dicEntityGroup;
            dicEntityGroup = new Dictionary<int, string>();
            List<string> validationGroupList;
            validationGroupList = new List<string>();
            TableCell hiddenTableCell;
            hiddenTableCell = new TableCell();
            hiddenTableCell.Visible = false;
            Table hiddenTable = new Table();
            hiddenTable.Visible = false;
            TableRow hiddenTableRow = new TableRow();
            hiddenTableRow.Visible = false;
            string tempDivGroupStyle = "";
            string tempDivColStyle = "";
            bool isButtonGroup = true;
            string validaionGroup;
            string[] valdationGroupArray;
            Dictionary<string, Control> tempDict;
            List<string> fileAnchorControlList = new List<string>();
            bool fullLengthControl = false;
            HtmlGenericControl divValidation;
            divValidation = null;
            //Get the column number of UI
            Cols = 2;//Convert.ToInt32(adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("ColLayoutActivity").ToString()).CNS_Value);
            if (Cols == 1)//One column UI
            {
                divColStyle = GetLocalResourceObject("div1colstyle").ToString(); //"divcolmiddle-S";// adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("SingleColStyle").ToString()).CNS_Data;
                tempDivColStyle = divColStyle;
                divGroupStyle = "fields-grpwrap single";// adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("SingleColGroupStyle").ToString()).CNS_Data;
                tempDivGroupStyle = divGroupStyle;
            }
            else//Greater than one column UI
            {
                divColStyle = GetLocalResourceObject("div2colstyle").ToString();// "div2col-M";// adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("DoubleColStyle").ToString()).CNS_Data;
                tempDivColStyle = divColStyle;
                divGroupStyle = "fields-grpwrap";// adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("DoubleColGroupStyle").ToString()).CNS_Data;
                tempDivGroupStyle = divGroupStyle;
            }
            PageScript = " function InitComponents(flag) {";//starting of initcomponent script


            DataTable dtFieldControls = LINQToDataTable(spAdmFormTabControlCfgGetResultList);
            DataTable tempDT = dtFieldControls.Copy();
            try
            {
                if (dtFieldControls.Rows.Count > 0)
                {
                    tempDict = new Dictionary<string, Control>();
                    //Data Table Iterate
                    for (int i = 0; i <= dtFieldControls.Rows.Count; i++)
                    {
                        divGroupStyle = tempDivGroupStyle;
                        divColStyle = tempDivColStyle;
                        //create temp table cell
                        tempTableCell = new TableCell();
                        //set temp div
                        tempDiv = new HtmlGenericControl("div");
                        tempSequence = 0;
                        //Get the group number of the controls
                        int order = int.Parse(dtFieldControls.Rows[0]["ACC_CONTROL_GROUP"].ToString());
                        //Filter table with the current group number
                        DataRow[] drFieldControls = dtFieldControls.Select("ACC_CONTROL_GROUP = " + order);
                        DataRow[] tempDrFieldControls = null;
                        HtmlGenericControl divMainGroup = new HtmlGenericControl("div");
                        //Set the Group Div ID
                        if (dtFieldControls.Rows[0]["CGC_CONTROL_ID"] != null && !string.IsNullOrEmpty(dtFieldControls.Rows[0]["CGC_CONTROL_ID"].ToString()))
                        {
                            divMainGroup.ID = dtFieldControls.Rows[0]["CGC_CONTROL_ID"].ToString();
                            divMainGroup.ClientIDMode = ClientIDMode.Static;
                        }
                        HtmlGenericControl divSubGroup = new HtmlGenericControl("div");
                        //Get and Set the Div Group Style
                        if (dtFieldControls.Rows[0]["ACC_CONTROL_GROUP_STYLE"] != null && !string.IsNullOrEmpty(dtFieldControls.Rows[0]["ACC_CONTROL_GROUP_STYLE"].ToString()))
                        {
                            divGroupStyle = dtFieldControls.Rows[0]["ACC_CONTROL_GROUP_STYLE"].ToString();
                        }
                        divMainGroup.Attributes.Add("class", divGroupStyle);
                        divSubGroup.Attributes.Add("class", "fields-group");
                        if (drFieldControls.Count() > 0)
                        {
                            tempDrFieldControls = dtFieldControls.Copy().Select("ACC_CONTROL_GROUP = " + order);
                            //Set the header text of the main group
                            if (drFieldControls[0]["ACC_CONTROL_GROUP_TEXT"] != null && !string.IsNullOrEmpty(drFieldControls[0]["ACC_CONTROL_GROUP_TEXT"].ToString()))
                            {
                                HtmlGenericControl groupHead = new HtmlGenericControl("h1");
                                groupHead.InnerText = drFieldControls[0]["ACC_CONTROL_GROUP_TEXT"].ToString();
                                HtmlGenericControl divGroupHeadClear = new HtmlGenericControl("div");
                                divGroupHeadClear.Attributes.Add("class", GetLocalResourceObject("Class_Clear").ToString());
                                divMainGroup.Controls.Add(groupHead);
                                divMainGroup.Controls.Add(divGroupHeadClear);
                            }
                            //for Set the entity group session
                            if (drFieldControls[0]["CGC_QUERY_TEXT"] != null)
                            {
                                if (!string.IsNullOrEmpty(drFieldControls[0]["CGC_QUERY_TEXT"].ToString()))
                                {
                                    string myValue = string.Empty;
                                    myValue = dicEntityGroup.FirstOrDefault(x => x.Key == order).Value;
                                    if (string.IsNullOrEmpty(myValue))
                                    {
                                        //The dictionary contains Group number as the key and entity name as the value
                                        dicEntityGroup.Add(order, drFieldControls[0]["CGC_QUERY_TEXT"].ToString());
                                    }
                                }
                            }
                        }
                        //Data Row Iterate
                        for (int k = 0; k < drFieldControls.Count();)
                        {
                            //Create new Table Object
                            Table tbControls = new Table();
                            if (drFieldControls[k]["ACC_IS_FULL_LENGTH"] != null && drFieldControls[k]["ACC_IS_FULL_LENGTH"].ToString() == "1")//If Full Length Control
                            {
                                tbControls.CssClass = "";
                                divColStyle = GetLocalResourceObject("div1colstyle").ToString();
                            }
                            else
                            {
                                if (drFieldControls.Count() > 1)
                                {
                                    if (drFieldControls[k]["ACC_CONTROL_GROUP"].ToString() != order.ToString()
                                            || drFieldControls[k]["ACC_SEQUENCE"].ToString() != tempSequence.ToString())
                                    {
                                        tbControls.CssClass = "table-devide";
                                        divColStyle = tempDivColStyle;
                                    }
                                }
                                else if (drFieldControls.Count() == 1 && Cols > 1
                                    && ((drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()).Equals("TextArea")
                                    || (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()).Equals("Label")))
                                {
                                    tbControls.CssClass = "";
                                    divColStyle = GetLocalResourceObject("div1colstyle").ToString();
                                }
                                else
                                {
                                    tbControls.CssClass = "table-devide";
                                    divColStyle = tempDivColStyle;
                                }
                            }

                            //Create new TableRow Object
                            TableRow trControls = new TableRow();
                            int j = 0;
                            //Column Iterate
                            for (j = 0; j < Cols && k < drFieldControls.Count();)
                            {
                                TableCell tcControl;
                                HtmlGenericControl div;
                                tcControl = new TableCell();
                                div = new HtmlGenericControl("div");
                                if (!(drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()).Equals("HiddenField"))
                                {
                                    if ((int.Parse(drFieldControls[k]["ACC_SEQUENCE"].ToString()) == tempSequence &&
                                        int.Parse(drFieldControls[k]["ACC_CONTROL_GROUP"].ToString()) == order))//If more than one controls have same sequence and same order, that controls sholud come in the same cell. 
                                    {
                                        tcControl = tempTableCell;//will not create new table cell, but keep the last cell as the current cell so that the new control can add to the same cell
                                        div = tempDiv;//Div-same as cell
                                    }
                                    else
                                    {
                                        //Create new table cell
                                        tcControl = new TableCell();
                                        //create new inner div
                                        div = new HtmlGenericControl("div");
                                        bool isGridButton = false;
                                        if (k > 0 && (tempDrFieldControls[k - 1]["ACC_CONTROL_TEXT"].ToString().Equals("Button")
                                            || tempDrFieldControls[k - 1]["ACC_CONTROL_TEXT"].ToString().Equals("LinkButton")
                                            || tempDrFieldControls[k - 1]["ACC_CONTROL_TEXT"].ToString().Equals("ImageButton")))//If control is not the first one in the current group and it is a button or link button
                                        {
                                            if (drFieldControls[k]["ACC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString()))//check the current control has a related control id
                                            {
                                                if (tempDrFieldControls[k - 1]["ACC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(tempDrFieldControls[k - 1]["ACC_REL_CONTROL_ID"].ToString()))//check the previous control has a related control id
                                                {
                                                    if (tempDrFieldControls[k - 1]["ACC_REL_CONTROL_ID"].ToString().Equals(drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString()))//check the current and previous controls have the same related id
                                                    {
                                                        isGridButton = true;//It is a grid button
                                                    }
                                                    else
                                                    {
                                                        isGridButton = false;
                                                    }
                                                }
                                                else
                                                {
                                                    isGridButton = false;
                                                }
                                            }
                                            else
                                            {
                                                isGridButton = false;
                                            }

                                            //Check whether the group only contains buttons or linkbuttons or not
                                            DataRow[] tempDr = tempDT.Select("ACC_CONTROL_GROUP = " + order);
                                            isButtonGroup = true;
                                            for (int x = 0; x < tempDr.Count(); x++)
                                            {
                                                if (!tempDr[x]["ACC_CONTROL_TEXT"].ToString().Equals("Button")
                                                    && !tempDr[x]["ACC_CONTROL_TEXT"].ToString().Equals("LinkButton")
                                                    && !tempDr[x]["ACC_CONTROL_TEXT"].ToString().Equals("ImageButton"))//check the control is a button or linkbutton. if any control is not a but or lnkbut, its not a button group
                                                {
                                                    isButtonGroup = false;
                                                    break;
                                                }
                                            }
                                        }
                                        else//either the control is the first control in the group or its not button or linkbutton
                                        {
                                            isGridButton = false;
                                            isButtonGroup = false;
                                        }

                                        if (isGridButton || isButtonGroup)//if true will not create new table cell, but keep the last cell as the current cell so that the new control can add to the same cell
                                        {
                                            tcControl = tempTableCell;
                                            div = tempDiv;
                                        }
                                        else//if false create new table cell and inner div
                                        {
                                            tempTableCell = new TableCell();
                                            tempDiv = new HtmlGenericControl("div");
                                        }

                                        //To confirm its a button group or not .. if it is, then there will not be a label in front of the button.. 
                                        if (k == 0 && (tempDrFieldControls[k]["ACC_CONTROL_TEXT"].ToString().Equals("Button")
                                            || tempDrFieldControls[k]["ACC_CONTROL_TEXT"].ToString().Equals("LinkButton")
                                            || tempDrFieldControls[k]["ACC_CONTROL_TEXT"].ToString().Equals("ImageButton")))
                                        {
                                            DataRow[] tempDr = tempDT.Select("ACC_CONTROL_GROUP = " + order);
                                            isButtonGroup = true;
                                            for (int x = 0; x < tempDr.Count(); x++)
                                            {
                                                if (!tempDr[x]["ACC_CONTROL_TEXT"].ToString().Equals("Button")
                                                    && !tempDr[x]["ACC_CONTROL_TEXT"].ToString().Equals("LinkButton")
                                                    && !tempDr[x]["ACC_CONTROL_TEXT"].ToString().Equals("ImageButton"))
                                                {
                                                    isButtonGroup = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                //set style for the inner div
                                if (drFieldControls[k]["ACC_CONTROL_GROUP"].ToString() != order.ToString()
                                            || drFieldControls[k]["ACC_SEQUENCE"].ToString() != tempSequence.ToString())
                                {
                                    div.Attributes.Add("class", divColStyle);
                                }
                                //set the temsequence
                                tempSequence = int.Parse(drFieldControls[k]["ACC_SEQUENCE"].ToString());
                                //Get Validation Group
                                if (drFieldControls[k]["ACC_VALD_GROUP"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_VALD_GROUP"].ToString()))
                                {
                                    validaionGroup = drFieldControls[k]["ACC_VALD_GROUP"].ToString();
                                    valdationGroupArray = validaionGroup.Split(',');
                                    foreach (string valGroup in valdationGroupArray)
                                    {
                                        if (!validationGroupList.Contains(valGroup))
                                        {
                                            validationGroupList.Add(valGroup);
                                        }
                                    }
                                }
                                else
                                {
                                    validaionGroup = string.Empty;
                                    valdationGroupArray = new string[0];
                                }
                                switch ((ControlTypes)(Enum.Parse(typeof(ControlTypes), drFieldControls[k]["ACC_CONTROL_TEXT"].ToString())))
                                {
                                    #region Controls
                                    #region Header
                                    case ControlTypes.Header:
                                        div.Controls.Add(new HtmlGenericControl("h3")
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            InnerText = drFieldControls[k]["ACC_NAME"].ToString()
                                        });
                                        tabIndex--;
                                        break;
                                    #endregion
                                    #region Label
                                    case ControlTypes.Label:
                                        Label lbl = new Label()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                            AssociatedControlID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static
                                        };
                                        div.Controls.Add(lbl);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        tabIndex--;
                                        divSubGroup.Attributes.Add("class", "group-note");//Label have a defferent style
                                        if (drFieldControls[k]["ACC_IS_FULL_LENGTH"] != null && drFieldControls[k]["ACC_IS_FULL_LENGTH"].ToString() == "1")
                                        {
                                            if (Cols > 1)
                                            {
                                                div.Attributes.Add("class", "");
                                            }
                                            //For style incriment j.
                                            j++;
                                        }
                                        else
                                        {
                                            div.Attributes.Add("class", "");
                                        }
                                        break;
                                    #endregion
                                    #region Text
                                    case ControlTypes.Text:
                                        if (tcControl != tempTableCell)//add Label only if it is not related to any other control
                                        {
                                            if (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString() != "")
                                            {
                                                div.Controls.Add(new Label()
                                                {
                                                    ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                                    AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    ClientIDMode = ClientIDMode.Static
                                                });
                                            }
                                        }
                                        TextBox txt = new TextBox()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            MaxLength = 50,
                                            TabIndex = tabIndex
                                        };

                                        #region Setting Default Value

                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_QUERY_TEXT"].ToString()))
                                        {
                                            string query = drFieldControls[k]["ACC_QUERY_TEXT"].ToString().Trim();
                                            hdfQry.Value = query; // Store Control Query for Execute After Binding Control in Brand Item Tab
                                            List<string> _result = GetTextBoxValueBasedOnStoredQuery(query);
                                            if (_result != null) txt.Text = HttpUtility.HtmlDecode(_result[0] ?? string.Empty);
                                            txt.Enabled = false;
                                        }
                                        #endregion

                                        if (drFieldControls[k]["ACC_REL_CONTROL_ID"] != null && drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString() == "divItemTax")
                                        {
                                            hdfPopUpAssociatedControl.Value = txt.ID;
                                        }
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                txt.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        if (drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "CIM_BRAND_SPECIFICATION")
                                        {
                                            txt.CssClass = string.IsNullOrEmpty(txt.CssClass) ? "custom-product-attributes" : txt.CssClass + " custom-product-attributes";
                                            txt.Attributes["style"] = "width:100% !important; min-width:0 !important; max-width:none !important; box-sizing:border-box;";
                                            txt.Width = Unit.Percentage(100);
                                            div.Attributes["style"] = "display:grid !important; grid-template-columns:240px minmax(0,1fr) !important; column-gap:10px !important; align-items:center !important; width:100% !important; box-sizing:border-box;";
                                        }
                                        if (drFieldControls[k]["ACC_LENGTH"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_LENGTH"].ToString() != string.Empty)
                                                txt.MaxLength = Convert.ToInt32(drFieldControls[k]["ACC_LENGTH"].ToString());
                                        }
                                        div.Controls.Add(txt);
                                        divValidation = new HtmlGenericControl("div");
                                        divValidation.Attributes.Add("class", "starwrap");
                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrftxt = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = txt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Enter").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                    };
                                                    divValidation.Controls.Add(vrftxt);
                                                }
                                            }
                                        }
                                        HiddenField hdnText = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_PK"].ToString()
                                        };
                                        div.Controls.Add(hdnText);
                                        //Set the Regular expression for the text. ex: email
                                        if (drFieldControls[k]["ACC_FORMAT"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_FORMAT"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                RegularExpressionValidator regExObj = new RegularExpressionValidator()
                                                {
                                                    ID = "rev" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    CssClass = "star",
                                                    ControlToValidate = txt.ID,
                                                    SetFocusOnError = true,
                                                    ErrorMessage = "Invalid " + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", ""),
                                                    ValidationExpression = drFieldControls[k]["ACC_FORMAT"].ToString().Trim(),
                                                    EnableClientScript = true,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*"
                                                };
                                                divValidation.Controls.Add(regExObj);
                                            }
                                        }

                                        //Add the custom validator if needed
                                        if (drFieldControls[k]["ACC_CUSTOM_VALD"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_CUSTOM_VALD"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                CustomValidator cusValObj = new CustomValidator()
                                                {
                                                    ID = "cusVal" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    CssClass = "star",
                                                    SetFocusOnError = true,
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    EnableClientScript = true,
                                                    ControlToValidate = txt.ID,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*",
                                                    ValidateEmptyText = true,
                                                    ClientValidationFunction = drFieldControls[k]["ACC_CUSTOM_VALD"].ToString(),
                                                    ErrorMessage = GetLocalResourceObject("Select").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                };
                                                divValidation.Controls.Add(cusValObj);
                                            }
                                        }

                                        //add the compare validator if needed
                                        if (drFieldControls[k]["ACC_CMP_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_CMP_CONTROL_ID"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                CompareValidator comValObj = new CompareValidator()
                                                {
                                                    ID = "comVal" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    CssClass = "star",
                                                    SetFocusOnError = true,
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    EnableClientScript = true,
                                                    ControlToValidate = drFieldControls[k]["ACC_CMP_CONTROL_ID"].ToString(),
                                                    ControlToCompare = txt.ID,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*",
                                                    ErrorMessage = GetLocalResourceObject("Mismatch").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                };
                                                divValidation.Controls.Add(comValObj);
                                            }
                                        }
                                        div.Controls.Add(divValidation);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);

                                        if (drFieldControls[k]["ACC_IS_FULL_LENGTH"] != null && drFieldControls[k]["ACC_IS_FULL_LENGTH"].ToString() == "1")
                                        {
                                            if (Cols > 1)
                                            {
                                                div.Attributes.Add("class", GetLocalResourceObject("div1colstyle").ToString());
                                            }
                                            //For style incriment j.
                                            j++;
                                        }
                                        break;
                                    #endregion
                                    #region Password Text
                                    case ControlTypes.Password:
                                        if (tcControl != tempTableCell)//add Label only if it is not related to any other control
                                        {
                                            if (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString() != "")
                                            {
                                                div.Controls.Add(new Label()
                                                {
                                                    ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                                    AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString()
                                                });
                                            }
                                        }
                                        TextBox txtPwd = new TextBox()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            MaxLength = 50,
                                            TabIndex = tabIndex,
                                            TextMode = TextBoxMode.Password,
                                            EnableViewState = true

                                        };
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                txtPwd.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        if (drFieldControls[k]["ACC_LENGTH"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_LENGTH"].ToString() != string.Empty)
                                                txtPwd.MaxLength = Convert.ToInt32(drFieldControls[k]["ACC_LENGTH"].ToString());
                                        }
                                        div.Controls.Add(txtPwd);
                                        divValidation = new HtmlGenericControl("div");
                                        divValidation.Attributes.Add("class", "starwrap");
                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrftxtPwd = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = txtPwd.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Enter").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                    };
                                                    divValidation.Controls.Add(vrftxtPwd);
                                                }
                                            }
                                        }
                                        HiddenField hdnPwd = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_PK"].ToString()
                                        };
                                        div.Controls.Add(hdnPwd);
                                        //Set the Regular expression for the text. ex: email
                                        if (drFieldControls[k]["ACC_FORMAT"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_FORMAT"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                RegularExpressionValidator regExObj = new RegularExpressionValidator()
                                                {
                                                    ID = "rev" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    CssClass = "star",
                                                    ControlToValidate = txtPwd.ID,
                                                    SetFocusOnError = true,
                                                    ErrorMessage = "Invalid " + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", ""),
                                                    ValidationExpression = drFieldControls[k]["ACC_FORMAT"].ToString().Trim(),
                                                    EnableClientScript = true,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*"
                                                };
                                                divValidation.Controls.Add(regExObj);
                                            }
                                        }

                                        //Add the custom validator if needed
                                        if (drFieldControls[k]["ACC_CUSTOM_VALD"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_CUSTOM_VALD"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                CustomValidator cusValObj = new CustomValidator()
                                                {
                                                    ID = "cusVal" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    CssClass = "star",
                                                    SetFocusOnError = true,
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    EnableClientScript = true,
                                                    ControlToValidate = txtPwd.ID,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*",
                                                    ValidateEmptyText = true,
                                                    ClientValidationFunction = drFieldControls[k]["ACC_CUSTOM_VALD"].ToString(),
                                                    ErrorMessage = GetLocalResourceObject("Select").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                };
                                                divValidation.Controls.Add(cusValObj);
                                            }
                                        }

                                        //add the compare validator if needed
                                        if (drFieldControls[k]["ACC_CMP_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_CMP_CONTROL_ID"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                CompareValidator comValObj = new CompareValidator()
                                                {
                                                    ID = "comVal" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    CssClass = "star",
                                                    SetFocusOnError = true,
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    EnableClientScript = true,
                                                    ControlToValidate = drFieldControls[k]["ACC_CMP_CONTROL_ID"].ToString(),
                                                    ControlToCompare = txtPwd.ID,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*",
                                                    ErrorMessage = GetLocalResourceObject("Mismatch").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                };
                                                divValidation.Controls.Add(comValObj);
                                            }
                                        }

                                        div.Controls.Add(divValidation);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);

                                        if (drFieldControls[k]["ACC_IS_FULL_LENGTH"] != null && drFieldControls[k]["ACC_IS_FULL_LENGTH"].ToString() == "1")
                                        {
                                            if (Cols > 1)
                                            {
                                                div.Attributes.Add("class", GetLocalResourceObject("div1colstyle").ToString());
                                            }
                                            //For style incriment j.
                                            j++;
                                        }

                                        break;
                                    #endregion
                                    #region HourText
                                    case ControlTypes.HourText:
                                        if (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        TextBox hrtxt = new TextBox()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                hrtxt.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        if (drFieldControls[k]["ACC_LENGTH"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_LENGTH"].ToString() != string.Empty)
                                                hrtxt.MaxLength = Convert.ToInt32(drFieldControls[k]["ACC_LENGTH"].ToString());
                                        }
                                        hrtxt.Attributes.Add("onkeypress", "ValidateText(event,this)");
                                        div.Controls.Add(hrtxt);
                                        MaskedEditExtender cc1 = new MaskedEditExtender();
                                        cc1.ID = "mee" + drFieldControls[k]["ACC_CONTROL_ID"].ToString();
                                        cc1.AutoComplete = false;
                                        cc1.Mask = "999:99";
                                        cc1.MaskType = AjaxControlToolkit.MaskedEditType.Time;
                                        cc1.TargetControlID = hrtxt.ID;
                                        div.Controls.Add(cc1);
                                        divValidation = new HtmlGenericControl("div");
                                        divValidation.Attributes.Add("class", "starwrap");
                                        for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                        {
                                            RegularExpressionValidator rev = new RegularExpressionValidator()
                                            {
                                                ID = "rev" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                ValidationGroup = valdationGroupArray[valCount],
                                                CssClass = "star",
                                                ControlToValidate = hrtxt.ID,
                                                SetFocusOnError = true,
                                                ErrorMessage = "Invalid Time",
                                                ValidationExpression = "^([0-9]{0,3}):([0-5][0-9])?$",
                                                EnableClientScript = true,
                                                Display = ValidatorDisplay.Dynamic,
                                                Text = "*"
                                            };
                                            divValidation.Controls.Add(rev);
                                        }
                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfHrtxt = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = hrtxt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Enter").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                    };
                                                    divValidation.Controls.Add(vrfHrtxt);
                                                }
                                            }
                                        }
                                        div.Controls.Add(divValidation);
                                        //set the PK of the control
                                        HiddenField hdnhrText = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnhrText);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region TextArea
                                    case ControlTypes.TextArea:
                                        if (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        TextBox txtArea = new TextBox()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            TextMode = TextBoxMode.MultiLine,
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                txtArea.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        if (drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "CIM_BRAND_SPECIFICATION")
                                        {
                                            txtArea.CssClass = string.IsNullOrEmpty(txtArea.CssClass) ? "custom-product-attributes" : txtArea.CssClass + " custom-product-attributes";
                                            txtArea.Attributes["style"] = "width:100% !important; min-width:0 !important; max-width:none !important; box-sizing:border-box;";
                                            txtArea.Width = Unit.Percentage(100);
                                            div.Attributes["style"] = "display:grid !important; grid-template-columns:240px minmax(0,1fr) !important; column-gap:10px !important; align-items:center !important; width:100% !important; box-sizing:border-box;";
                                        }
                                        if (drFieldControls[k]["ACC_LENGTH"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_LENGTH"].ToString()))
                                        {
                                            int maxLength = Convert.ToInt32(drFieldControls[k]["ACC_LENGTH"].ToString());
                                            txtArea.Attributes.Add("onkeydown", "limitText(this," + maxLength + ");");
                                            txtArea.Attributes.Add("onkeyup", "limitText(this," + maxLength + ");");
                                        }

                                        int cols = Convert.ToInt32(GetLocalResourceObject("TextAreaCols").ToString());
                                        txtArea.Columns = cols;
                                        div.Controls.Add(txtArea);
                                        divValidation = new HtmlGenericControl("div");
                                        divValidation.Attributes.Add("class", "starwrap");
                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfTxtArea = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = txtArea.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Enter").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                    };
                                                    divValidation.Controls.Add(vrfTxtArea);
                                                }
                                            }
                                        }
                                        div.Controls.Add(divValidation);
                                        HiddenField hdnTextArea = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnTextArea);
                                        //Set style for div if only the textarea comes under a group in 2 col style
                                        if (drFieldControls.Count() == 1 && Cols > 1)
                                        {
                                            div.Attributes.Add("class", GetLocalResourceObject("div1colstyle").ToString());
                                        }
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        if (drFieldControls[k]["ACC_IS_FULL_LENGTH"] != null && drFieldControls[k]["ACC_IS_FULL_LENGTH"].ToString() == "1")
                                        {
                                            if (Cols > 1)
                                            {
                                                div.Attributes.Add("class", GetLocalResourceObject("div1colstyle").ToString());
                                            }
                                            //For style incriment j.
                                            j++;
                                        }
                                        break;
                                    #endregion
                                    #region Date
                                    case ControlTypes.Date:
                                        if (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        TextBox date = new TextBox()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                date.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        tabIndex++;
                                        date.Attributes.Add("onkeydown", "return CheckKey(event)");
                                        date.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(date);
                                        divValidation = new HtmlGenericControl("div");
                                        divValidation.Attributes.Add("class", "starwrap");
                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfDate = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = date.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Select").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "") + " Date"
                                                    };
                                                    divValidation.Controls.Add(vrfDate);
                                                }
                                            }
                                        }
                                        div.Controls.Add(divValidation);

                                        HiddenField hdnDateP = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnDateP);
                                        HiddenField hdfDateP = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdfDateP);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls[k]["ACC_CONTROL_TEXT"].ToString(), date.ID, null, null, null);
                                        break;
                                    #endregion
                                    #region DateRange
                                    case ControlTypes.DateRange:
                                        if (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        TextBox dateRange = new TextBox()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                dateRange.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        tabIndex++;
                                        dateRange.Attributes.Add("onkeydown", "return CheckKey(event)");
                                        dateRange.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(dateRange);
                                        divValidation = new HtmlGenericControl("div");
                                        divValidation.Attributes.Add("class", "starwrap");
                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfDate = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = dateRange.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Select").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "") + " Date"
                                                    };
                                                    divValidation.Controls.Add(vrfDate);
                                                }
                                            }
                                        }
                                        div.Controls.Add(divValidation);

                                        HiddenField hdnDateRange = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnDateRange);
                                        HiddenField hdfDateRange = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdfDateRange);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        if (drFieldControls[k]["ACC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString()))
                                        {
                                            string fromDate = drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString();
                                            string hdfFrmDate = "hdf" + drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString();
                                            PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls[k]["ACC_CONTROL_TEXT"].ToString(), dateRange.ID, hdfDateRange.ID, fromDate, hdfFrmDate);
                                        }
                                        break;
                                    #endregion
                                    #region DateTime
                                    case ControlTypes.DateTime:
                                        if (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        TextBox dttxt = new TextBox()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                dttxt.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        tabIndex++;
                                        dttxt.Attributes.Add("onkeydown", "return CheckKey(event)");
                                        dttxt.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(dttxt);
                                        divValidation = new HtmlGenericControl("div");
                                        divValidation.Attributes.Add("class", "starwrap");
                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfDate = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = dttxt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Select").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "") + " Date"
                                                    };
                                                    divValidation.Controls.Add(vrfDate);
                                                }
                                            }
                                        }

                                        TextBox dttimetxt = new TextBox()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "_Time",
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",

                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                dttimetxt.CssClass = GetLocalResourceObject("TimePickerStyle_DateTime").ToString();
                                        }
                                        dttimetxt.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(dttimetxt);
                                        for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                        {
                                            RegularExpressionValidator vreTime = new RegularExpressionValidator()
                                            {
                                                ID = "vre" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                ControlToValidate = dttimetxt.ID,
                                                CssClass = "star",
                                                SetFocusOnError = true,
                                                ValidationGroup = valdationGroupArray[valCount],
                                                EnableClientScript = true,
                                                ValidationExpression = "^([01]?[0-9]|2[0-3]):[0-5][0-9]?$",
                                                Display = ValidatorDisplay.Dynamic,
                                                Text = "*",
                                                ErrorMessage = GetLocalResourceObject("EnterValid").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "") + " Time"
                                            };
                                            divValidation.Controls.Add(vreTime);
                                        }
                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfTime = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "_Time",
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = dttimetxt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Select").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "") + " Time"
                                                    };
                                                    divValidation.Controls.Add(vrfTime);
                                                }
                                            }
                                        }
                                        div.Controls.Add(divValidation);
                                        HiddenField hdnDate = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnDate);

                                        HiddenField hdnDateTime = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "_Time",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnDateTime);

                                        HiddenField hdfDate = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdfDate);
                                        HiddenField hdfTime = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "_Time",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdfTime);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls[k]["ACC_CONTROL_TEXT"].ToString(), dttxt.ID, null, null, null);
                                        break;
                                    #endregion
                                    #region TimePicker
                                    case ControlTypes.TimePicker:
                                        if (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        TextBox tmtxt = new TextBox()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                tmtxt.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        tmtxt.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(tmtxt);
                                        divValidation = new HtmlGenericControl("div");
                                        divValidation.Attributes.Add("class", "starwrap");
                                        for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                        {
                                            RegularExpressionValidator vreTimePick = new RegularExpressionValidator()
                                            {
                                                ID = "vre" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                ControlToValidate = tmtxt.ID,
                                                CssClass = "star",
                                                SetFocusOnError = true,
                                                ValidationGroup = valdationGroupArray[valCount],
                                                EnableClientScript = true,
                                                ValidationExpression = "^([01]?[0-9]|2[0-3]):[0-5][0-9]?$",
                                                Display = ValidatorDisplay.Dynamic,
                                                Text = "*",
                                                ErrorMessage = GetLocalResourceObject("EnterValid").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                            };
                                            divValidation.Controls.Add(vreTimePick);
                                        }
                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfTimePick = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = tmtxt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Select").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                    };
                                                    divValidation.Controls.Add(vrfTimePick);
                                                }
                                            }
                                        }
                                        div.Controls.Add(divValidation);

                                        HiddenField hdnTime = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnTime);
                                        //tmtxt.Attributes.Add("onkeydown", "return false");
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls[k]["ACC_CONTROL_TEXT"].ToString(), tmtxt.ID, null, null, null);
                                        break;
                                    #endregion
                                    #region Numeric
                                    case ControlTypes.Numeric:
                                        if (tcControl != tempTableCell)//add Label only if it is not related to any other control
                                        {
                                            if (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString() != "")
                                            {
                                                div.Controls.Add(new Label()
                                                {
                                                    ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                                    AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    ClientIDMode = ClientIDMode.Static
                                                });
                                            }
                                        }
                                        TextBox nutxt = new TextBox()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            Text = "",
                                            MaxLength = 5,
                                            ClientIDMode = ClientIDMode.Static,
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                nutxt.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        if (drFieldControls[k]["ACC_LENGTH"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_LENGTH"].ToString() != string.Empty)
                                                nutxt.MaxLength = Convert.ToInt32(drFieldControls[k]["ACC_LENGTH"].ToString());
                                        }
                                        div.Controls.Add(nutxt);
                                        //Set the integer and decimal part of the text in the numeric textbox
                                        string integerPart = "10";
                                        string decimalPart = "0";
                                        string regex = "^\\d{1,10}(?:\\.\\d{1,0}){0,1}$";
                                        //string maxValue = "";
                                        //if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_VALUE"].ToString()))
                                        //{
                                        //    regex = "^\\d{1,10}(?:\\.\\d{1,0}){0,1}?[" + drFieldControls[k]["ACC_VALUE"].ToString() + "]?$";//Eg://^\\d{1,2}(?:\\.\\d{1,2}){0,1}?[100]?$
                                        //    maxValue = "?[" + drFieldControls[k]["ACC_VALUE"].ToString() + "]?";
                                        //}
                                        divValidation = new HtmlGenericControl("div");
                                        divValidation.Attributes.Add("class", "starwrap");
                                        if (drFieldControls[k]["ACC_FORMAT"] == null || string.IsNullOrEmpty(drFieldControls[k]["ACC_FORMAT"].ToString()))
                                        {
                                            if (drFieldControls[k]["ACC_LENGTH"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_LENGTH"].ToString()))
                                            {
                                                integerPart = drFieldControls[k]["ACC_LENGTH"].ToString();
                                            }
                                            decimalPart = "0";
                                        }
                                        else if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_FORMAT"].ToString()))
                                        {
                                            string[] intDec = drFieldControls[k]["ACC_FORMAT"].ToString().Split(',');//ex: 10,3  means 10 integer num and 3 decimal num
                                            if (intDec.Length == 2)
                                            {
                                                integerPart = intDec[0];
                                                decimalPart = intDec[1];
                                            }
                                            else
                                            {
                                                decimalPart = drFieldControls[k]["ACC_FORMAT"].ToString();
                                                if (drFieldControls[k]["ACC_LENGTH"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_LENGTH"].ToString()))
                                                {
                                                    integerPart = (Convert.ToInt32(drFieldControls[k]["ACC_LENGTH"].ToString()) - (Convert.ToInt32(decimalPart) + 1)).ToString();
                                                }
                                            }
                                        }
                                        if (decimalPart.Equals("0"))//if only the integer number
                                        {
                                            FilteredTextBoxExtender fte = new FilteredTextBoxExtender()
                                            {
                                                ID = "fte" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                TargetControlID = nutxt.ID,
                                                Enabled = true,
                                                FilterType = FilterTypes.Numbers
                                            };
                                            div.Controls.Add(fte);
                                        }
                                        else//if number with decimal part
                                        {
                                            regex = "^\\d{1," + integerPart + "}(?:\\.\\d{1," + decimalPart + "}){0,1}$";
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                RegularExpressionValidator regEx = new RegularExpressionValidator()
                                                {
                                                    ID = "rev" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    CssClass = "star",
                                                    ControlToValidate = nutxt.ID,
                                                    SetFocusOnError = true,
                                                    ErrorMessage = GetLocalResourceObject("Invalid").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", ""),
                                                    ValidationExpression = regex,
                                                    EnableClientScript = true,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*"
                                                };
                                                divValidation.Controls.Add(regEx);
                                            }
                                        }
                                        //set the number of integerpart value for later use
                                        HiddenField hdnIntegerPart = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "IntegerPart",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = integerPart
                                        };
                                        div.Controls.Add(hdnIntegerPart);
                                        //set the number of decimalpart value for later use
                                        HiddenField hdnDecimalPart = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "DecimalPart",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = decimalPart
                                        };
                                        div.Controls.Add(hdnDecimalPart);

                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrf = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = nutxt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Enter").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                    };
                                                    divValidation.Controls.Add(vrf);
                                                }
                                            }
                                        }
                                        //add the compare validator if needed
                                        if (drFieldControls[k]["ACC_CMP_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_CMP_CONTROL_ID"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                CompareValidator comValObj = new CompareValidator()
                                                {
                                                    ID = "comVal" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    CssClass = "star",
                                                    SetFocusOnError = true,
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    EnableClientScript = true,
                                                    ControlToValidate = drFieldControls[k]["ACC_CMP_CONTROL_ID"].ToString(),
                                                    ControlToCompare = nutxt.ID,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*",
                                                    ErrorMessage = GetLocalResourceObject("Mismatch").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                };
                                                divValidation.Controls.Add(comValObj);
                                            }
                                        }

                                        //Add the custom validator if needed
                                        if (drFieldControls[k]["ACC_CUSTOM_VALD"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_CUSTOM_VALD"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                CustomValidator cusValObj = new CustomValidator()
                                                {
                                                    ID = "cusVal" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    CssClass = "star",
                                                    SetFocusOnError = true,
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    EnableClientScript = true,
                                                    ControlToValidate = nutxt.ID,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*",
                                                    ValidateEmptyText = true,
                                                    ClientValidationFunction = drFieldControls[k]["ACC_CUSTOM_VALD"].ToString(),
                                                    ErrorMessage = GetLocalResourceObject("Invalid").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                };
                                                divValidation.Controls.Add(cusValObj);
                                            }
                                        }

                                        div.Controls.Add(divValidation);
                                        HiddenField hdnNum = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnNum);
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region DropDown
                                    case ControlTypes.DropDown:
                                        if (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString().Replace("DDL", "Lbl"),
                                                Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString().Replace("DDL", "Lbl"),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        DropDownList ddlDrop = new DropDownList()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["ACC_ACTION"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_ACTION"].ToString() != string.Empty)
                                            {
                                                //ddlDrop.SelectedIndexChanged += new EventHandler(ActionHandler);
                                                //ddlDrop.AutoPostBack = true;
                                                HiddenField hdfDDLAction = new HiddenField()
                                                {
                                                    ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Action",
                                                    ClientIDMode = ClientIDMode.Static,
                                                    Value = drFieldControls[k]["ACC_ACTION"].ToString()
                                                };
                                                div.Controls.Add(hdfDDLAction);
                                                Button btnDDL = new Button()
                                                {
                                                    ID = "btn" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Action",
                                                    ClientIDMode = ClientIDMode.Static,
                                                    CommandName = "SELECTEDINDEXCHANGED",
                                                    CommandArgument = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    EnableTheming = false
                                                };
                                                btnDDL.Attributes.Add("style", "display:none;");
                                                btnDDL.Click += new EventHandler(ActionHandler);
                                                div.Controls.Add(btnDDL);
                                            }

                                        }
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                ddlDrop.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        HiddenField hdnDrop = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "at",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        HiddenField hdnDropPk = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString().Replace("DDL", "Txt"),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };

                                        HiddenField hdnDropUICPk = new HiddenField()
                                        {
                                            ID = "hdfUICPk_" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_PK"].ToString()
                                        };

                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_QUERY_TEXT"].ToString()))
                                        {
                                            CommonService commonService;
                                            commonService = null;
                                            string query = drFieldControls[k]["ACC_QUERY_TEXT"].ToString().Trim();
                                            string relId = string.Empty;
                                            string value = string.Empty;
                                            //Replace query with the values if any condition is there
                                            List<string> conditionList = new List<string>();
                                            string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                            if (splitWithAt.Count() > 1)
                                            {
                                                for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                {
                                                    conditionList.Add(splitWithAt[arrayCount].Trim());
                                                }
                                                if (drFieldControls[k]["ACC_REL_CONTROL_ID"] != null
                                                    && !string.IsNullOrEmpty(drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString()))//If it is a child of any other control. ex:country-state
                                                {
                                                    relId = drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString();
                                                    string controlType = (from dr in tempDT.AsEnumerable()
                                                                          where dr.Field<string>("ACC_CONTROL_ID") == relId
                                                                          select dr.Field<string>("ACC_CONTROL_TEXT")).First();
                                                    if (controlType == "DropDown")
                                                    {
                                                        foreach (KeyValuePair<string, Control> control in tempDict)
                                                        {
                                                            DropDownList parent = (DropDownList)control.Value;
                                                            if (parent != null && parent.Items.Count > 1)
                                                            {
                                                                if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                {
                                                                    value = parent.SelectedValue;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                foreach (string condition in conditionList)
                                                {
                                                    if (Session[condition] != null)//parameter name is same as any session name
                                                    {
                                                        if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                            query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                        else
                                                        {
                                                            query = string.Empty;
                                                            break;
                                                        }
                                                    }
                                                    else if (condition == relId)// parameter is value of any other control
                                                    {
                                                        if (!string.IsNullOrEmpty(value))
                                                            query = query.Replace("@" + condition + "@", value);
                                                        else
                                                        {
                                                            query = string.Empty;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(query))
                                            {
                                                commonService = new CommonService();
                                                commonService = CommonFunctions.InitiateClient(commonService);
                                                //Execute query
                                                query = query.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                                List<DDLMaster> ddlValues = commonService.ExecuteQuery(query);
                                                ddlDrop.DataTextField = "Value";
                                                ddlDrop.DataValueField = "PK";
                                                ddlDrop.DataSource = CommonFunctions.HtmlDecode(ddlValues, "Value");
                                                ddlDrop.DataBind();
                                            }
                                            else
                                            {
                                                ddlDrop.Items.Clear();
                                            }
                                        }
                                        //add "select" to the dropdown
                                        ddlDrop.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                                        string tempControlType = (from dr in tempDT.AsEnumerable()
                                                                  where dr.Field<string>("ACC_REL_CONTROL_ID") == ddlDrop.ID
                                                                  select dr.Field<string>("ACC_CONTROL_TEXT")).FirstOrDefault();
                                        //check the dropdown has the related control id of any text or numeric or textarea control. then add "Other" to the dropdown
                                        if (!string.IsNullOrEmpty(tempControlType) && tempControlType == "Text" || tempControlType == "Numeric" || tempControlType == "TextArea")
                                        {
                                            int otherValue = -2;
                                            ddlDrop.Items.Add(new ListItem("Other", otherValue.ToString()));
                                            //Create a hiddenfield to know it has a related control field. It is used when saving values from the dropdown. 
                                            //if it has a related id and it's value is less than zero, then will save null
                                            HiddenField hdfRelId = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "HasRelatedId",
                                                ClientIDMode = ClientIDMode.Static
                                            };
                                            div.Controls.Add(hdfRelId);
                                        }

                                        if (drFieldControls[k]["ACC_VALUE"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_VALUE"].ToString().Trim()))
                                        {
                                            int index = -3;
                                            if (Int32.TryParse(drFieldControls[k]["ACC_VALUE"].ToString().Trim(), out index))
                                            {
                                                if (index > -3)
                                                {
                                                    ListItem li = ddlDrop.Items.FindByValue(index.ToString());
                                                    if (li != null)
                                                    {
                                                        ddlDrop.SelectedValue = index.ToString();
                                                    }
                                                }
                                            }
                                        }

                                        div.Controls.Add(ddlDrop);
                                        div.Controls.Add(hdnDrop);
                                        div.Controls.Add(hdnDropPk);
                                        div.Controls.Add(hdnDropUICPk);
                                        tempDict.Add(ddlDrop.ID, ddlDrop);
                                        divValidation = new HtmlGenericControl("div");
                                        divValidation.Attributes.Add("class", "starwrap");
                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfddl = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = ddlDrop.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Select").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", ""),
                                                        InitialValue = CommonConstants.SELECTVAL
                                                    };
                                                    divValidation.Controls.Add(vrfddl);
                                                }
                                            }
                                        }
                                        div.Controls.Add(divValidation);

                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);

                                        if (drFieldControls[k]["ACC_IS_FULL_LENGTH"] != null && drFieldControls[k]["ACC_IS_FULL_LENGTH"].ToString() == "1")
                                        {
                                            if (Cols > 1)
                                            {
                                                div.Attributes.Add("class", GetLocalResourceObject("div1colstyle").ToString());
                                            }
                                            //For style incriment j.
                                            j++;
                                        }
                                        break;
                                    #endregion
                                    #region Button
                                    case ControlTypes.Button:
                                        if (isButtonGroup)//Checking whether it is a in a buttongroup or not
                                        {
                                            tbControls.CssClass = "";
                                            div.Attributes.Add("class", "button-fieldsgrp");
                                        }
                                        else
                                        {
                                            //Checking whether it is a grid button or not
                                            if (drFieldControls[k]["ACC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString()))
                                            {
                                                bool isFirstGridButton = false;
                                                if (k > 0 && tempDrFieldControls[k - 1]["ACC_CONTROL_TEXT"].ToString().Equals("Button"))//check previous control is button
                                                {
                                                    if (tempDrFieldControls[k - 1]["ACC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(tempDrFieldControls[k - 1]["ACC_REL_CONTROL_ID"].ToString()))//check previous button have related control 
                                                    {
                                                        if (tempDrFieldControls[k - 1]["ACC_REL_CONTROL_ID"].ToString().Equals(drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString()))//check previous button's related control is same as the current button's related control
                                                        {
                                                            isFirstGridButton = false;//its not the first grid button
                                                        }
                                                        else//previous button's related control is not same as the current button's related control
                                                        {
                                                            if (j > 0)//if it is not in the first cell
                                                            {
                                                                isFirstGridButton = false;
                                                            }
                                                            else//if the button is in the first cell
                                                            {
                                                                isFirstGridButton = true;
                                                            }
                                                        }
                                                    }
                                                    else//previous button doesn't have related control 
                                                    {
                                                        if (j > 0)//if it is not in the first cell
                                                        {
                                                            isFirstGridButton = false;
                                                        }
                                                        else//if the button is in the first cell
                                                        {
                                                            isFirstGridButton = true;
                                                        }
                                                    }
                                                }
                                                else//previous control is not a button
                                                {
                                                    if (j > 0)//if it is not in the first cell
                                                    {
                                                        isFirstGridButton = false;
                                                    }
                                                    else//if the button is in the first cell
                                                    {
                                                        isFirstGridButton = true;
                                                    }
                                                }
                                                if (isFirstGridButton)//if it is the first grid button
                                                {
                                                    for (; j < Cols; j++)//if it is not in the first cell, fill the row with dummy cells
                                                    {
                                                        TableCell tcControl2 = new TableCell();
                                                        HtmlGenericControl dummydiv = new HtmlGenericControl("div");
                                                        div.Attributes.Add("class", divColStyle);
                                                        tcControl2.Controls.Add(dummydiv);
                                                        trControls.Cells.Add(tcControl2);
                                                    }
                                                    //add new table
                                                    tbControls = new Table();
                                                    tbControls.CssClass = "";
                                                    //add new row
                                                    trControls = new TableRow();
                                                    //initialize j
                                                    j = 0;
                                                    //add new cell
                                                    tcControl = new TableCell();
                                                    //add new innerdiv
                                                    div = new HtmlGenericControl("div");
                                                    div.Attributes.Add("class", "button-fieldsgrp");
                                                }
                                                else//if it is not the first grid button
                                                {
                                                    tbControls.CssClass = "";
                                                    div.Attributes.Add("class", "button-fieldsgrp");
                                                }
                                            }
                                            if (tcControl != tempTableCell)//The label wil add only if it as an individual button in the table
                                            {
                                                div.Controls.Add(new Label()
                                                {
                                                    ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    Text = "",
                                                    AssociatedControlID = drFieldControls[k]["ACC_CONTROL_ID"].ToString()
                                                });
                                            }
                                        }

                                        if (drFieldControls.Count() == 1)//If the group contains only one control
                                        {
                                            tbControls.CssClass = "";
                                            div.Attributes.Add("class", "button-fieldsgrp");
                                        }

                                        if (TabCode == TabType.CLST && drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "BTN_NEW")
                                        {
                                            AddCustomerStatusFilter(div);
                                        }

                                        Button btn = new Button();
                                        btn.ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString();
                                        btn.ClientIDMode = ClientIDMode.Static;
                                        btn.Text = drFieldControls[k]["ACC_NAME"].ToString();
                                        btn.ToolTip = HttpUtility.HtmlDecode(drFieldControls[k]["ACC_NAME"].ToString());
                                        if (drFieldControls[k]["ACC_VALD_GROUP"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_VALD_GROUP"].ToString()))
                                        {
                                            btn.ValidationGroup = drFieldControls[k]["ACC_VALD_GROUP"].ToString();
                                            if (btn.Text.ToLower().Equals("submit"))
                                            {
                                                ucrWrkf.ValidationGroup = btn.ValidationGroup;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_ACTION"].ToString()))
                                        {
                                            btn.CommandName = drFieldControls[k]["ACC_ACTION"].ToString();
                                            btn.CausesValidation = true;
                                            btn.Click += new EventHandler(ActionHandler);
                                        }
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_VALD_SP"].ToString()))
                                        {
                                            HiddenField hdnValidSP = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "VALD_SP",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["ACC_VALD_SP"].ToString()
                                            };
                                            div.Controls.Add(hdnValidSP);
                                        }
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_VALD_SP_PARAM"].ToString()))
                                        {
                                            HiddenField hdnValidSPParam = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "VALD_SP_PARAM",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["ACC_VALD_SP_PARAM"].ToString()
                                            };
                                            div.Controls.Add(hdnValidSPParam);
                                        }

                                        if (drFieldControls[k]["ACC_SCRIPT"] != null)
                                        {
                                            if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_SCRIPT"].ToString()))
                                            {
                                                btn.OnClientClick = drFieldControls[k]["ACC_SCRIPT"].ToString();
                                            }
                                        }
                                        btn.TabIndex = tabIndex;
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_STYLE"].ToString().Trim()))
                                        {
                                            btn.SkinID = drFieldControls[k]["ACC_STYLE"].ToString().Trim();
                                        }
                                        if (drFieldControls[k]["ACC_TOOLTIP"] != null
                                            && !string.IsNullOrEmpty(drFieldControls[k]["ACC_TOOLTIP"].ToString().Trim()))
                                        {
                                            btn.ToolTip = HttpUtility.HtmlDecode(drFieldControls[k]["ACC_TOOLTIP"].ToString()).Trim();
                                        }
                                        //Check the button is workflow related or not. if yes then add event for the current button.
                                        if (drFieldControls[k]["ACC_IS_WORKFLOW"] != null
                                            && !string.IsNullOrEmpty(drFieldControls[k]["ACC_IS_WORKFLOW"].ToString().Trim())
                                            && drFieldControls[k]["ACC_IS_WORKFLOW"].ToString().Equals("1"))
                                        {
                                            btn.CommandArgument = "PageAction_Entry";
                                            btn.PreRender += new EventHandler(btnAction_PreRender);
                                        }

                                        div.Controls.Add(btn);
                                        //For Grid related buttons "ACC_QUERY_TEXT" will be the grid name. and the other buttons it will be the entity name 
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_QUERY_TEXT"].ToString()))
                                        {
                                            HiddenField hdnEntity = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Entity",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["ACC_QUERY_TEXT"].ToString()
                                            };
                                            div.Controls.Add(hdnEntity);
                                        }
                                        //The group number of the button. it is used to find the entity name from the dictionary
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_CONTROL_GROUP"].ToString()))
                                        {
                                            HiddenField hdnGroup = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Group",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["ACC_CONTROL_GROUP"].ToString()
                                            };
                                            div.Controls.Add(hdnGroup);
                                        }
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region LinkButton
                                    case ControlTypes.LinkButton:

                                        if (drFieldControls.Count() == 1)//If the group contains only one control
                                        {
                                            tbControls.CssClass = "";
                                            div.Attributes.Add("class", "tab-container-grp");
                                            divSubGroup.Attributes.Add("class", "");
                                        }

                                        if (isButtonGroup)//If it is a button group
                                        {
                                            tbControls.CssClass = "";
                                            div.Attributes.Add("class", "tab-container-grp");
                                            divSubGroup.Attributes.Add("class", "");
                                        }

                                        LinkButton lnkbtn = new LinkButton();
                                        lnkbtn.ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString();
                                        lnkbtn.ClientIDMode = ClientIDMode.Static;
                                        lnkbtn.Text = drFieldControls[k]["ACC_NAME"].ToString();
                                        lnkbtn.ToolTip = HttpUtility.HtmlEncode(drFieldControls[k]["ACC_NAME"].ToString());
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_ACTION"].ToString()))
                                        {
                                            lnkbtn.CommandName = drFieldControls[k]["ACC_ACTION"].ToString();
                                            lnkbtn.CausesValidation = true;
                                            lnkbtn.Click += new EventHandler(ActionHandler);
                                        }
                                        if (drFieldControls[k]["ACC_SCRIPT"] != null)
                                        {
                                            if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_SCRIPT"].ToString()))
                                            {
                                                lnkbtn.OnClientClick = drFieldControls[k]["ACC_SCRIPT"].ToString();
                                            }
                                        }
                                        lnkbtn.TabIndex = tabIndex;
                                        div.Controls.Add(lnkbtn);
                                        //For Grid related buttons "ACC_QUERY_TEXT" will be the grid name. and the other buttons it will be the entity name 
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_QUERY_TEXT"].ToString()))
                                        {
                                            HiddenField hdnEntity = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Entity",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["ACC_QUERY_TEXT"].ToString()
                                            };
                                            div.Controls.Add(hdnEntity);
                                        }
                                        //The group number of the button. it is used to find the entity name from the dictionary
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_CONTROL_GROUP"].ToString()))
                                        {
                                            HiddenField hdnGroup = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Group",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["ACC_CONTROL_GROUP"].ToString()
                                            };
                                            div.Controls.Add(hdnGroup);
                                        }
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region Spacer
                                    case ControlTypes.Spacer:
                                        LiteralControl ltc = new LiteralControl()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "&nbsp"
                                        };
                                        div.Controls.Add(ltc);
                                        tabIndex--;
                                        break;
                                    #endregion
                                    #region GridView
                                    case ControlTypes.GridView:
                                        //The grid sholud be in 0th TD
                                        string gridStyle = "gridwrap grid-w930";
                                        //Get the grid style
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_STYLE"].ToString().Trim()))
                                        {
                                            gridStyle = drFieldControls[k]["ACC_STYLE"].ToString().Trim();
                                        }
                                        if (j != 0)//If the current cell is not 0
                                        {
                                            for (; j < Cols; j++)//add dummy column to the row
                                            {
                                                TableCell tcControl2 = new TableCell();
                                                HtmlGenericControl dummydiv = new HtmlGenericControl("div");
                                                div.Attributes.Add("class", divColStyle);
                                                tcControl2.Controls.Add(dummydiv);
                                                trControls.Cells.Add(tcControl2);
                                            }
                                            tbControls = new Table();
                                            tbControls.CssClass = "";
                                            trControls = new TableRow();
                                            j = 0;
                                            tcControl = new TableCell();
                                            div = new HtmlGenericControl("div");
                                            div.Attributes.Add("class", gridStyle);
                                        }
                                        div.Attributes.Remove("class");
                                        div.Attributes.Add("class", gridStyle);
                                        tbControls.CssClass = "";
                                        GridView grd = new GridView()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            PageSize = int.Parse("10"),
                                            AllowSorting = true,
                                            TabIndex = tabIndex,
                                            Width = Unit.Percentage(100)
                                        };
                                        //set rowdatabound action
                                        grd.RowDataBound += new GridViewRowEventHandler(ActionHandler);
                                        div.Controls.Add(grd);
                                        //~Test start
                                        //set paging action
                                        //grd.AllowPaging = true;
                                        //grd.PageIndexChanging += new GridViewPageEventHandler(ActionHandler);
                                        //~Test end
                                        //Add Empty Grid Template
                                        TemplateBuilder tmpEmptyDataTemplate = new TemplateBuilder();
                                        tmpEmptyDataTemplate.AppendLiteralString("No Record Found");
                                        grd.EmptyDataTemplate = tmpEmptyDataTemplate;
                                        grd.EmptyDataRowStyle.ForeColor = Color.Black;
                                        grd.EmptyDataRowStyle.Font.Bold = true;
                                        grd.EmptyDataRowStyle.HorizontalAlign = HorizontalAlign.Center;
                                        grd.EmptyDataRowStyle.BackColor = ColorTranslator.FromHtml("#EFF0F1");
                                        grd.EmptyDataRowStyle.CssClass = "emptytable";
                                        HiddenField hdfGrid = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_PK"].ToString()
                                        };
                                        div.Controls.Add(hdfGrid);
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_QUERY_TEXT"].ToString()))
                                        {
                                            //Get the entity name of the grid
                                            string entityName = drFieldControls[k]["ACC_QUERY_TEXT"].ToString().Trim();
                                            if (!string.IsNullOrEmpty(entityName))
                                            {
                                                if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString()))//if the entity is the master entity
                                                {
                                                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = null;//clear the customerpk session
                                                }
                                                if (Session[ERP.Utilities.SessionStrings.CUSTOMERPK] != null)
                                                {
                                                    CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CUSTOMERPK].ToString());//get the master entity pk
                                                }
                                                //create new instance of service. This object should be maintain until all the operation is completed. This is for maintaining the entity object context
                                                customerRegistrationServiceClient = new CustomerRegistrationService();
                                                customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                                //Get the customer master details


                                                GetFieldValues(ControlsEnum.CUSTOMER);
                                                if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                                {
                                                    if (hdfGrid != null && !string.IsNullOrEmpty(hdfGrid.Value))
                                                    {
                                                        DataTable dtGrid = null;
                                                        DataRow drGrid;
                                                        DataColumn dcGrid;
                                                        int countRow = 0;
                                                        int controlPK = Convert.ToInt32(hdfGrid.Value);
                                                        dtGrid = new DataTable();
                                                        //Get the grid reference table details. this will tell which all data to show in the grid, which shold hide,etc
                                                        admFormTabControlDtlList = customerRegistrationServiceClient.FormTabControlDtl(controlPK);
                                                        if (admFormTabControlDtlList != null && admFormTabControlDtlList.Count > 0)
                                                        {
                                                            //Check whether the grid is a child grid or not
                                                            bool bindGrid = false;
                                                            if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString()))
                                                            {
                                                                EntityName = entityName;
                                                                ControlPK = controlPK;
                                                                //Get the parent grid id
                                                                RelatedControlID = drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString();
                                                                ChildGridName = grd.ID;
                                                                string[] entityArray = entityName.Split('.');
                                                                if (entityArray.Length > 1)
                                                                {
                                                                    if (entityArray[entityArray.Length - 2] != null)
                                                                    {
                                                                        string hdrEntity = entityArray[entityArray.Length - 2];
                                                                        if (Session[hdrEntity] != null)
                                                                        {
                                                                            bindGrid = true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                bindGrid = true;
                                                            }
                                                            if (bindGrid)
                                                            {
                                                                //List for store the column numbers which should hide
                                                                List<int> hiddenColumnList = new List<int>();
                                                                Dictionary<string, string> dicGridControls = new Dictionary<string, string>();
                                                                Dictionary<int, double> dicGridColumnWidth = new Dictionary<int, double>();
                                                                Dictionary<int, int> dicGridColumnLength = new Dictionary<int, int>();
                                                                //Clear the GridControlsList session
                                                                Session["GridControlsList"] = null;
                                                                //clear the HiddenColumnList session
                                                                Session["HiddenColumnList"] = null;
                                                                //Clear the GridColumnWidth session
                                                                Session["GridColumnWidth"] = null;
                                                                //Clear the GridColumnLength session
                                                                Session["GridColumnLength"] = null;
                                                                IEnumerable entityList = null;
                                                                Object customerObj = null;
                                                                if (crmCustomerMstList != null && crmCustomerMstList.Count == 1)
                                                                {
                                                                    customerObj = crmCustomerMstList[0];//set the current selected customer master obj
                                                                }
                                                                retEntityObj = null;
                                                                Object retEntity = null;
                                                                if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString()))
                                                                {
                                                                    //set the master entity list for bind the grid
                                                                    retEntity = crmCustomerMstList;
                                                                }
                                                                else
                                                                {
                                                                    if (customerObj != null)
                                                                    {
                                                                        //Get the EntityCollection that shold be bind to the grid
                                                                        retEntity = GetEntityCollection(customerObj, entityName);
                                                                    }
                                                                }
                                                                if (retEntity != null)//If EntityCollection is not null
                                                                {
                                                                    entityList = (IEnumerable)retEntity;
                                                                    foreach (Object enitityObj in entityList)
                                                                    {
                                                                        drGrid = dtGrid.NewRow();
                                                                        int columnCount = 0;
                                                                        foreach (ADM_FORM_TAB_CONTROL_DTL admFormTabControlDtlObj in admFormTabControlDtlList)
                                                                        {
                                                                            retVal = string.Empty;
                                                                            //Get the value 
                                                                            string value = string.Empty;
                                                                            string filePath = string.Empty;
                                                                            if (admFormTabControlDtlObj.ACD_CONTROL == null ||
                                                                                (admFormTabControlDtlObj.ACD_CONTROL != null && !admFormTabControlDtlObj.ADM_CONTROLS_CFG.CTL_NAME.Equals(ControlTypes.LinkButton.ToString())))
                                                                            {
                                                                                if (admFormTabControlDtlObj.ACD_CONTROL_ID.Contains('+'))
                                                                                {
                                                                                    string[] splitFile = admFormTabControlDtlObj.ACD_CONTROL_ID.Split('+');
                                                                                    if (splitFile.Length == 2)
                                                                                    {
                                                                                        value = GetPropertyValue(enitityObj, splitFile[0]) + GetPropertyValue(enitityObj, splitFile[1]);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    value = GetPropertyValue(enitityObj, admFormTabControlDtlObj.ACD_CONTROL_ID);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (admFormTabControlDtlObj.ACD_CONTROL_ID.Contains(','))
                                                                                {
                                                                                    string[] splitFile = admFormTabControlDtlObj.ACD_CONTROL_ID.Split(',');

                                                                                    if (splitFile.Length == 2)
                                                                                    {
                                                                                        value = GetPropertyValue(enitityObj, splitFile[0]);
                                                                                        filePath = GetPropertyValue(enitityObj, splitFile[1]);
                                                                                    }
                                                                                }
                                                                                else if (admFormTabControlDtlObj.ACD_CONTROL_ID.Contains('+'))
                                                                                {
                                                                                    string[] splitFile = admFormTabControlDtlObj.ACD_CONTROL_ID.Split('+');
                                                                                    if (splitFile.Length == 2)
                                                                                    {
                                                                                        value = GetPropertyValue(enitityObj, splitFile[0]) + GetPropertyValue(enitityObj, splitFile[1]);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    value = GetPropertyValue(enitityObj, admFormTabControlDtlObj.ACD_CONTROL_ID);
                                                                                }
                                                                            }
                                                                            if (countRow == 0)//first row
                                                                            {
                                                                                dcGrid = new DataColumn();
                                                                                dcGrid.ColumnName = admFormTabControlDtlObj.ACD_CONTROL_ID;//set column name
                                                                                dtGrid.Columns.Add(dcGrid);
                                                                            }
                                                                            //check the value have to replace or not
                                                                            if (!string.IsNullOrEmpty(admFormTabControlDtlObj.ACD_VALUE_TEXT))
                                                                            {
                                                                                string[] valueTextArray = admFormTabControlDtlObj.ACD_VALUE_TEXT.Split(',');
                                                                                foreach (string str in valueTextArray)
                                                                                {
                                                                                    string[] valueArray = str.Split('=');
                                                                                    if (valueArray.Length > 1 && valueArray[0].Equals(value))
                                                                                    {
                                                                                        value = valueArray[1];
                                                                                        break;
                                                                                    }
                                                                                }
                                                                            }

                                                                            //check the value has to be formatted or not
                                                                            if (!string.IsNullOrEmpty(admFormTabControlDtlObj.ACD_FORMAT) && !string.IsNullOrEmpty(value))
                                                                            {
                                                                                decimal decOut;
                                                                                if (Decimal.TryParse(value, out decOut))
                                                                                    value = String.Format(admFormTabControlDtlObj.ACD_FORMAT, Decimal.Parse(value));
                                                                            }

                                                                            //set value to the cell
                                                                            drGrid[admFormTabControlDtlObj.ACD_CONTROL_ID] = HttpUtility.HtmlDecode(value);
                                                                            if (admFormTabControlDtlObj.ACD_IS_HIDDEN == 1)//check the cell is set to hidden or not
                                                                            {
                                                                                if (!hiddenColumnList.Contains(columnCount))
                                                                                    hiddenColumnList.Add(columnCount);//add to the hiddenfield list
                                                                            }
                                                                            else
                                                                            {
                                                                                //set the width of the column
                                                                                double columnWidth = admFormTabControlDtlObj.ACD_WIDTH == null ? 0 : Convert.ToDouble(admFormTabControlDtlObj.ACD_WIDTH);
                                                                                if (!dicGridColumnWidth.ContainsKey(columnCount) && columnWidth > 0)
                                                                                    dicGridColumnWidth.Add(columnCount, columnWidth);
                                                                                //set the Length of the column
                                                                                int columnLength = admFormTabControlDtlObj.ACD_LENGTH == null ? 0 : Convert.ToInt32(admFormTabControlDtlObj.ACD_LENGTH);
                                                                                if (!dicGridColumnLength.ContainsKey(columnCount) && columnLength > 0)
                                                                                    dicGridColumnLength.Add(columnCount, columnLength);
                                                                            }
                                                                            //set controls

                                                                            if (admFormTabControlDtlObj.ACD_CONTROL != null
                                                                                && admFormTabControlDtlObj.ADM_CONTROLS_CFG.CTL_NAME.Equals(ControlTypes.LinkButton.ToString())
                                                                                && !string.IsNullOrEmpty(filePath))
                                                                            {
                                                                                if (!dicGridControls.ContainsKey(countRow + "," + columnCount))
                                                                                    dicGridControls.Add(countRow + "," + columnCount, filePath);
                                                                            }
                                                                            //iterate column
                                                                            columnCount++;
                                                                        }
                                                                        //iterate row
                                                                        countRow++;
                                                                        //add rows to the data table
                                                                        dtGrid.Rows.Add(drGrid);
                                                                        dtGrid.AcceptChanges();
                                                                        if (hiddenColumnList.Count > 0)
                                                                        {
                                                                            Session["HiddenColumnList"] = hiddenColumnList;
                                                                        }
                                                                        if (dicGridControls.Count > 0)
                                                                        {
                                                                            Session["GridControlsList"] = dicGridControls;
                                                                        }
                                                                        if (dicGridColumnWidth.Count > 0)
                                                                        {
                                                                            Session["GridColumnWidth"] = dicGridColumnWidth;
                                                                        }
                                                                        if (dicGridColumnLength.Count > 0)
                                                                        {
                                                                            Session["GridColumnLength"] = dicGridColumnLength;
                                                                        }

                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //Bind grid
                                                        //~Test Start
                                                        //PageIndex = PageIndex == null ? "0" : PageIndex;
                                                        //grd.PageIndex = Convert.ToInt32(PageIndex);
                                                        //~Test End
                                                        if (dtGrid.Rows.Count > 0)
                                                        {
                                                            if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_VALUE"].ToString()))
                                                            {
                                                                HiddenField hdfGridSortValue = new HiddenField()//For grid in detail section
                                                                {
                                                                    ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "SortValue",
                                                                    ClientIDMode = ClientIDMode.Static,
                                                                    Value = drFieldControls[k]["ACC_VALUE"].ToString()
                                                                };
                                                                div.Controls.Add(hdfGridSortValue);
                                                                string colName = drFieldControls[k]["ACC_VALUE"].ToString();
                                                                string direction = Resources.Report.SortAscending;
                                                                dtGrid.DefaultView.Sort = colName + " " + direction;
                                                                dtGrid = dtGrid.DefaultView.ToTable();
                                                            }
                                                        }
                                                        grd.DataSource = dtGrid;
                                                        grd.DataBind();
                                                        //For style incriment j. THe grid sholuld be shown in one row
                                                        j++;
                                                    }
                                                    #region pager
                                                    //string usercontrolpath = System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString() == "" ?
                                                    //                        "~/UserControls/PagerControl.ascx" :
                                                    //                        "/" + System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString() +
                                                    //                        "UserControls/PagerControl.ascx";
                                                    //string sql = "";
                                                    //string countsql = "";
                                                    //countsql = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString();
                                                    //if (Session["ParentPK"] != null)
                                                    //{
                                                    //    sql = sql.Replace("Key", Session["ParentPK"].ToString());
                                                    //}
                                                    //else
                                                    //{
                                                    //    sql = sql.Replace("Key", "0");
                                                    //}
                                                    //if (countsql == "")
                                                    //{
                                                    //    countsql = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString();
                                                    //}
                                                    //if (Session["ParentPK"] != null)
                                                    //{
                                                    //    countsql = countsql.Replace("Key", Session["ParentPK"].ToString());
                                                    //}
                                                    //else
                                                    //{
                                                    //    countsql = countsql.Replace("Key", "0");
                                                    //}
                                                    //div.Controls.Add(grd);
                                                    #endregion
                                                }
                                            }
                                        }
                                        break;
                                    #endregion
                                    #region CheckBox
                                    case ControlTypes.CheckBox:
                                        CheckBox chk = new CheckBox()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                            TabIndex = tabIndex,
                                            TextAlign = TextAlign.Left

                                        };
                                        #region Enabling/Disabling Checkbox
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_QUERY"].ToString()))
                                        {
                                            chk.Enabled = false;
                                            chk.CssClass = "style-none disp-inline";
                                        }
                                        #endregion
                                        div.Controls.Add(chk);
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        if (drFieldControls[k]["ACC_VALUE"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_VALUE"].ToString()))
                                        {
                                            if (drFieldControls[k]["ACC_VALUE"].ToString().Equals("1"))
                                                chk.Checked = true;
                                            else
                                                chk.Checked = false;
                                        }
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region Iframe
                                    case ControlTypes.Iframe:
                                        HtmlGenericControl iframe = new HtmlGenericControl("iframe");
                                        iframe.ID = "frmReview";
                                        iframe.Attributes.Add("frameborder", "0");
                                        iframe.Attributes.Add("scrolling", "no");
                                        iframe.Attributes.Add("width", "100%");
                                        iframe.Attributes.Add("style", "border: none;min-height: 300px;");
                                        div.Controls.Add(iframe);
                                        tabIndex--;
                                        break;
                                    #endregion
                                    #region HiddenField
                                    case ControlTypes.HiddenField:
                                        HiddenField hdfPK = new HiddenField()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        //Hidden field will add directly to the div sub group.
                                        divSubGroup.Controls.Add(hdfPK);
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        divSubGroup.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region FileUpload
                                    case ControlTypes.FileUpload:
                                        if (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["ACC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }

                                        FileUpload fup = new FileUpload()
                                        {
                                            ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            TabIndex = tabIndex
                                        };
                                        //Set PostBackTrigger for the buttons that related to the current fileupload
                                        if (drFieldControls[k]["ACC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString()))
                                        {
                                            //May be more than one button that related to the fileupload
                                            string[] triggerControlsArray = drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString().Split(',');
                                            foreach (string controlId in triggerControlsArray)
                                            {
                                                //Create new postbacktrigger
                                                PostBackTrigger po = new PostBackTrigger();
                                                po.ControlID = controlId;
                                                bool hasTrigger = false;
                                                //check for duplication of postbacktrigger.if not then add it to the Trigger section
                                                foreach (PostBackTrigger trigger in aupdpnlCustomerRegistration.Triggers)
                                                {
                                                    if (trigger.ControlID.Equals(po.ControlID))
                                                    {
                                                        hasTrigger = true;
                                                        break;
                                                    }
                                                }
                                                //Add postbacktrigger
                                                if (!hasTrigger)
                                                    aupdpnlCustomerRegistration.Triggers.Add(po);
                                            }
                                        }
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                fup.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        div.Controls.Add(fup);

                                        Button btnFileUpload = new Button()
                                        {
                                            ID = "btn" + fup.ID,
                                            Text = GetLocalResourceObject("Change").ToString(),
                                            ToolTip = GetLocalResourceObject("Change").ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            SkinID = "btnInner-save"
                                        };
                                        btnFileUpload.Attributes.Add("style", "display:none;");
                                        div.Controls.Add(btnFileUpload);

                                        //To show the selected file
                                        HtmlAnchor anchorFile = new HtmlAnchor()
                                        {
                                            ID = "anchor" + fup.ID,
                                            Target = "_blank",
                                            ClientIDMode = ClientIDMode.Static,
                                            Visible = false
                                        };
                                        anchorFile.Attributes.Add("class", "download-icon");
                                        div.Controls.Add(anchorFile);
                                        if (!fileAnchorControlList.Contains(anchorFile.ID))
                                            fileAnchorControlList.Add(anchorFile.ID);

                                        HiddenField hdfFileName = new HiddenField()
                                        {
                                            ID = "hdf" + fup.ID + "FileName",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdfFileName);

                                        if (drFieldControls[k]["ACC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["ACC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                if (drFieldControls[k]["ACC_VALD_GROUP"].ToString() != null)
                                                {
                                                    HiddenField hdfValidationGroup = new HiddenField()//Used for to find Requirefieldvalidator control when disable it
                                                    {
                                                        ID = "hdf" + fup.ID + "ValidationGroup",
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Value = drFieldControls[k]["ACC_VALD_GROUP"].ToString()
                                                    };
                                                    div.Controls.Add(hdfValidationGroup);
                                                }
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfTxtArea = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = fup.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = GetLocalResourceObject("Select").ToString() + drFieldControls[k]["ACC_NAME"].ToString().Replace("*", "")
                                                    };
                                                    div.Controls.Add(vrfTxtArea);
                                                }
                                            }
                                        }
                                        HiddenField hdnFileUpload = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnFileUpload);

                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        if (drFieldControls[k]["ACC_IS_FULL_LENGTH"] != null && drFieldControls[k]["ACC_IS_FULL_LENGTH"].ToString() == "1")
                                        {
                                            if (Cols > 1)
                                            {
                                                div.Attributes.Add("class", GetLocalResourceObject("div1colstyle").ToString());
                                            }
                                            //For style incriment j.
                                            j++;
                                        }
                                        break;
                                    #endregion
                                    #region ImageButton
                                    case ControlTypes.ImageButton:
                                        if (isButtonGroup)//Checking whether it is a in a buttongroup or not
                                        {
                                            tbControls.CssClass = "";
                                            div.Attributes.Add("class", "button-fieldsgrp");
                                        }
                                        else
                                        {
                                            //Checking whether it is a grid button or not
                                            if (drFieldControls[k]["ACC_REL_CONTROL_ID"] != null
                                                && !string.IsNullOrEmpty(drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString())
                                                && drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString() != "divItemTax")
                                            {
                                                bool isFirstGridButton = false;
                                                if (k > 0 && tempDrFieldControls[k - 1]["ACC_CONTROL_TEXT"].ToString().Equals("ImageButton"))//check previous control is button
                                                {
                                                    if (tempDrFieldControls[k - 1]["ACC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(tempDrFieldControls[k - 1]["ACC_REL_CONTROL_ID"].ToString()))//check previous button have related control 
                                                    {
                                                        if (tempDrFieldControls[k - 1]["ACC_REL_CONTROL_ID"].ToString().Equals(drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString()))//check previous button's related control is same as the current button's related control
                                                        {
                                                            isFirstGridButton = false;//its not the first grid button
                                                        }
                                                        else//previous button's related control is not same as the current button's related control
                                                        {
                                                            if (j > 0)//if it is not in the first cell
                                                            {
                                                                isFirstGridButton = false;
                                                            }
                                                            else//if the button is in the first cell
                                                            {
                                                                isFirstGridButton = true;
                                                            }
                                                        }
                                                    }
                                                    else//previous button doesn't have related control 
                                                    {
                                                        if (j > 0)//if it is not in the first cell
                                                        {
                                                            isFirstGridButton = false;
                                                        }
                                                        else//if the button is in the first cell
                                                        {
                                                            isFirstGridButton = true;
                                                        }
                                                    }
                                                }
                                                else//previous control is not a button
                                                {
                                                    if (j > 0)//if it is not in the first cell
                                                    {
                                                        isFirstGridButton = false;
                                                    }
                                                    else//if the button is in the first cell
                                                    {
                                                        isFirstGridButton = true;
                                                    }
                                                }
                                                if (isFirstGridButton)//if it is the first grid button
                                                {
                                                    for (; j < Cols; j++)//if it is not in the first cell, fill the row with dummy cells
                                                    {
                                                        TableCell tcControl2 = new TableCell();
                                                        HtmlGenericControl dummydiv = new HtmlGenericControl("div");
                                                        div.Attributes.Add("class", divColStyle);
                                                        tcControl2.Controls.Add(dummydiv);
                                                        trControls.Cells.Add(tcControl2);
                                                    }
                                                    //add new table
                                                    tbControls = new Table();
                                                    tbControls.CssClass = "";
                                                    //add new row
                                                    trControls = new TableRow();
                                                    //initialize j
                                                    j = 0;
                                                    //add new cell
                                                    tcControl = new TableCell();
                                                    //add new innerdiv
                                                    div = new HtmlGenericControl("div");
                                                    div.Attributes.Add("class", "button-fieldsgrp");
                                                }
                                                else//if it is not the first grid button
                                                {
                                                    tbControls.CssClass = "";
                                                    div.Attributes.Add("class", "button-fieldsgrp");
                                                }
                                            }
                                            if (tcControl != tempTableCell)//The label wil add only if it as an individual button in the table
                                            {
                                                div.Controls.Add(new Label()
                                                {
                                                    ID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString(),
                                                    Text = "",
                                                    AssociatedControlID = drFieldControls[k]["ACC_CONTROL_ID"].ToString()
                                                });
                                            }
                                        }

                                        if (drFieldControls.Count() == 1)//If the group contains only one control
                                        {
                                            tbControls.CssClass = "";
                                            div.Attributes.Add("class", "button-fieldsgrp");
                                        }

                                        ImageButton imgBtn = new ImageButton();
                                        imgBtn.ID = drFieldControls[k]["ACC_CONTROL_ID"].ToString();
                                        imgBtn.ClientIDMode = ClientIDMode.Static;
                                        //imgBtn. = drFieldControls[k]["ACC_NAME"].ToString();
                                        imgBtn.ToolTip = HttpUtility.HtmlEncode(drFieldControls[k]["ACC_NAME"].ToString());
                                        if (drFieldControls[k]["ACC_VALD_GROUP"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_VALD_GROUP"].ToString()))
                                        {
                                            imgBtn.ValidationGroup = drFieldControls[k]["ACC_VALD_GROUP"].ToString();
                                            //if (imgBtn.Text.ToLower().Equals("submit"))
                                            //{
                                            //    ucrWrkf.ValidationGroup = btn.ValidationGroup;
                                            //}
                                        }
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_ACTION"].ToString()))
                                        {
                                            imgBtn.CommandName = drFieldControls[k]["ACC_ACTION"].ToString();
                                            imgBtn.CausesValidation = true;
                                            imgBtn.Click += new ImageClickEventHandler(ActionHandler);
                                        }
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_VALD_SP"].ToString()))
                                        {
                                            HiddenField hdnValidSP = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "VALD_SP",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["ACC_VALD_SP"].ToString()
                                            };
                                            div.Controls.Add(hdnValidSP);
                                        }
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_VALD_SP_PARAM"].ToString()))
                                        {
                                            HiddenField hdnValidSPParam = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "VALD_SP_PARAM",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["ACC_VALD_SP_PARAM"].ToString()
                                            };
                                            div.Controls.Add(hdnValidSPParam);
                                        }

                                        if (drFieldControls[k]["ACC_SCRIPT"] != null)
                                        {
                                            if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_SCRIPT"].ToString()))
                                            {
                                                imgBtn.OnClientClick = drFieldControls[k]["ACC_SCRIPT"].ToString();
                                            }
                                        }
                                        imgBtn.TabIndex = tabIndex;
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_STYLE"].ToString().Trim()))
                                        {
                                            imgBtn.SkinID = drFieldControls[k]["ACC_STYLE"].ToString().Trim();
                                        }
                                        if (drFieldControls[k]["ACC_TOOLTIP"] != null
                                            && !string.IsNullOrEmpty(drFieldControls[k]["ACC_TOOLTIP"].ToString().Trim()))
                                        {
                                            imgBtn.ToolTip = HttpUtility.HtmlEncode(drFieldControls[k]["ACC_TOOLTIP"].ToString().Trim());
                                        }
                                        //Check the button is workflow related or not. if yes then add event for the current button.
                                        if (drFieldControls[k]["ACC_IS_WORKFLOW"] != null
                                            && !string.IsNullOrEmpty(drFieldControls[k]["ACC_IS_WORKFLOW"].ToString().Trim())
                                            && drFieldControls[k]["ACC_IS_WORKFLOW"].ToString().Equals("1"))
                                        {
                                            imgBtn.CommandArgument = "PageAction_Entry";
                                            imgBtn.PreRender += new EventHandler(btnAction_PreRender);
                                        }

                                        div.Controls.Add(imgBtn);
                                        //For Grid related buttons "ACC_QUERY_TEXT" will be the grid name. and the other buttons it will be the entity name 
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_QUERY_TEXT"].ToString()))
                                        {
                                            HiddenField hdnEntity = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Entity",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["ACC_QUERY_TEXT"].ToString()
                                            };
                                            div.Controls.Add(hdnEntity);
                                        }
                                        //The group number of the button. it is used to find the entity name from the dictionary
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_CONTROL_GROUP"].ToString()))
                                        {
                                            HiddenField hdnGroup = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Group",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["ACC_CONTROL_GROUP"].ToString()
                                            };
                                            div.Controls.Add(hdnGroup);
                                        }
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                        #endregion

                                        #endregion
                                }
                                if ((drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()).Equals("HiddenField"))//If the control is a hidden field it is directly add to the divSubGroup. It doesn't need to add any cell
                                {
                                    //remove the current control from the control datatable
                                    dtFieldControls.Rows.RemoveAt(0);
                                    dtFieldControls.AcceptChanges();
                                    k++;
                                }
                                else
                                {
                                    //add inner div to the table cell
                                    tcControl.Controls.Add(div);
                                    //update temptable cell
                                    tempTableCell = tcControl;
                                    //update tempdiv
                                    tempDiv = div;
                                    if (drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "CIM_BRAND_SPECIFICATION" && Cols > 1)
                                    {
                                        tcControl.ColumnSpan = Cols;
                                        tcControl.Attributes["style"] = "width:100%;box-sizing:border-box;";
                                        tbControls.Attributes["style"] = "width:100%;table-layout:fixed;";
                                        j = Cols;
                                    }
                                    //add table cell to table row
                                    trControls.Cells.Add(tcControl);
                                    //add table row to table
                                    tbControls.Rows.Add(trControls);
                                    //add table to divsubgroup
                                    divSubGroup.Controls.Add(tbControls);
                                    tabIndex++;
                                    //remove the current control from the control datatable
                                    dtFieldControls.Rows.RemoveAt(0);
                                    dtFieldControls.AcceptChanges();
                                    k++;
                                    //Check any controls have same order and sequence
                                    if (k < drFieldControls.Count())
                                    {
                                        if (drFieldControls[k]["ACC_CONTROL_GROUP"].ToString() != order.ToString()
                                            || drFieldControls[k]["ACC_SEQUENCE"].ToString() != tempSequence.ToString())//increment column only if not any coming control have the same sequence and order of the current control
                                        {
                                            j++;
                                        }
                                        if (drFieldControls[k]["ACC_IS_FULL_LENGTH"] != null && drFieldControls[k]["ACC_IS_FULL_LENGTH"].ToString() == "1")
                                        {
                                            for (; j < Cols; j++)//add the dummy cells to the last row of the table if needed
                                            {
                                                TableCell tcControl2 = new TableCell();
                                                HtmlGenericControl div1 = new HtmlGenericControl("div");
                                                div1.Attributes.Add("class", divColStyle);
                                                tcControl2.Controls.Add(div1);
                                                trControls.Cells.Add(tcControl2);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        j++;
                                    }
                                }
                            }
                            if (tempDrFieldControls.Count() > 1)
                            {
                                for (; j < Cols; j++)//add the dummy cells to the last row of the table if needed
                                {
                                    TableCell tcControl2 = new TableCell();
                                    HtmlGenericControl div = new HtmlGenericControl("div");
                                    div.Attributes.Add("class", divColStyle);
                                    tcControl2.Controls.Add(div);
                                    trControls.Cells.Add(tcControl2);
                                }
                            }
                            else if (tempDrFieldControls.Count() == 1)
                            {
                                if (!tempDrFieldControls[0]["ACC_CONTROL_TEXT"].ToString().Equals("HiddenField")
                                    && !tempDrFieldControls[0]["ACC_CONTROL_TEXT"].ToString().Equals("TextArea")
                                    && !tempDrFieldControls[0]["ACC_CONTROL_TEXT"].ToString().Equals("Label")
                                    && !tempDrFieldControls[0]["ACC_CONTROL_TEXT"].ToString().Equals("GridView")
                                    )
                                {
                                    for (; j < Cols; j++)//add the dummy cells to the last row of the table if needed
                                    {
                                        TableCell tcControl2 = new TableCell();
                                        HtmlGenericControl div = new HtmlGenericControl("div");
                                        div.Attributes.Add("class", divColStyle);
                                        tcControl2.Controls.Add(div);
                                        trControls.Cells.Add(tcControl2);
                                    }
                                }
                            }
                            //Initialize i as 0
                            if (i >= dtFieldControls.Rows.Count)
                            {
                                i = 0;
                            }
                        }
                        //add divsubgroup to div main group
                        divMainGroup.Controls.Add(divSubGroup);
                        //add div main group to panel
                        pnlControls.Controls.Add(divMainGroup);
                        if (fileAnchorControlList != null && fileAnchorControlList.Count > 0)
                        {
                            Session["fileanchorcontrollist"] = fileAnchorControlList;
                        }
                    }
                    //Add validation summary
                    divValidationSummary.Controls.Clear();
                    foreach (string valGroup in validationGroupList)
                    {
                        ValidationSummary vsObj = new ValidationSummary()
                        {
                            ID = "vsPage" + valGroup,
                            ValidationGroup = valGroup
                        };
                        divValidationSummary.Controls.Add(vsObj);
                    }
                    string script = "";
                    //Get the dynamic tab list
                    GetFieldValues(ControlsEnum.DYNAMICTABS);
                    if (spAdmFormTabCfgGetResultList != null && spAdmFormTabCfgGetResultList.Count > 0)
                    {
                        foreach (SPADM_FORM_TAB_CFG_GET_Result tab in spAdmFormTabCfgGetResultList)
                        {
                            if (tab.ATC_CODE.Equals(TabCode))
                            {
                                //Get the script curresponding to the tab if any
                                script = string.IsNullOrEmpty(tab.ATC_SCRIPT) ? string.Empty : tab.ATC_SCRIPT;
                                break;
                            }
                        }
                    }
                    PageScript = PageScript + "}";//End of initcomponents
                    PageScript = PageScript + script;
                    //Register page script
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "pagescript", PageScript, true);
                    //set the dictionary<order,entityname> to the session
                    if (dicEntityGroup.Count > 0)
                    {
                        Session["EntityByGroup"] = dicEntityGroup;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        private List<string> GetTextBoxValueBasedOnStoredQuery(string query)
        {
            CommonService commonService;
            commonService = null;
            string relId = string.Empty;
            string value = string.Empty;
            //Replace query with the values if any condition is there
            List<string> conditionList = new List<string>();
            string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
            if (splitWithAt.Count() > 1)
            {
                for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                {
                    conditionList.Add(splitWithAt[arrayCount].Trim());
                }
                foreach (string condition in conditionList)
                {
                    if (Session[condition] != null)//parameter name is same as any session name
                    {
                        if (!string.IsNullOrEmpty(Session[condition].ToString()))
                            query = query.Replace("@" + condition + "@", Session[condition].ToString());
                        else
                        {
                            query = string.Empty;
                            break;
                        }
                    }
                    else if (this.SelectedPK > 0)
                    {
                        query = query.Replace("@" + condition + "@", SelectedPK.ToString());
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            if (!string.IsNullOrEmpty(query))
            {
                commonService = new CommonService();
                commonService = CommonFunctions.InitiateClient(commonService);
                //Execute query
                return commonService.ExecuteTextQuery(query);
            }
            return null;
        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.TAXTYPES:
                    //Bind Tax dropdown
                    ddlPopupTaxType.Items.Clear();
                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                    {
                        ddlPopupTaxType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtTaxDetails, Resources.DataFieldRes.RFQResponseTaxHead);
                        ddlPopupTaxType.DataTextField = Resources.DataFieldRes.RFQResponseTaxHead;
                        ddlPopupTaxType.DataValueField = Resources.DataFieldRes.RFQResponseTaxPK;
                        ddlPopupTaxType.DataBind();
                    }
                    //ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                    break;
                #region Packing Spec
                case ControlsEnum.PACKINGSPEC:
                    if (dtPackingSpec != null && dtPackingSpec.Rows.Count > 0)
                    {
                        ddlPackingSpec.Items.Clear();
                        ddlPackingSpec.DataSource = dtPackingSpec;
                        ddlPackingSpec.DataTextField = Resources.DataFieldRes.PackingSpecs;
                        ddlPackingSpec.DataValueField = Resources.DataFieldRes.PackingMstPK;
                        ddlPackingSpec.DataBind();
                        //ddlPackingSpec.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (ddlPackingSpec.Items.Count > 0 && packingSpecPk > 0)
                            ddlPackingSpec.SelectedIndex = ddlPackingSpec.Items.IndexOf(ddlPackingSpec.Items.FindByValue(packingSpecPk.ToString()));
                    }
                    break;
                    #endregion
            }
        }

        /// <summary>
        /// Method for Bind Grid
        /// </summary>
        /// <param name="controlPK"></param>
        /// <param name="entityName"></param>
        /// <param name="grd"></param>
        public void BindGrid(int controlPK, string entityName, GridView grd)
        {
            #region GridView
            if (!string.IsNullOrEmpty(entityName))
            {
                if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString()))//if the entity is the master entity
                {
                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = null; ;//clear the customerpk session
                }
                if (Session[ERP.Utilities.SessionStrings.CUSTOMERPK] != null)
                {
                    CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CUSTOMERPK].ToString());//get the master entity pk
                }
                //create new instance of service. This object should be maintain until all the operation is completed. This is for maintaining the entity object context
                customerRegistrationServiceClient = new CustomerRegistrationService();
                customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                //Get the customer master details
                GetFieldValues(ControlsEnum.CUSTOMER);
                if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                {
                    DataTable dtGrid = null;
                    DataRow drGrid;
                    DataColumn dcGrid;
                    int countRow = 0;
                    dtGrid = new DataTable();
                    //Get the grid reference table details. this will tell which all data to show in the grid, which shold hide,etc
                    admFormTabControlDtlList = customerRegistrationServiceClient.FormTabControlDtl(controlPK);
                    if (admFormTabControlDtlList != null && admFormTabControlDtlList.Count > 0)
                    {
                        Dictionary<string, string> dicGridControls = new Dictionary<string, string>();
                        Dictionary<int, double> dicGridColumnWidth = new Dictionary<int, double>();
                        Dictionary<int, int> dicGridColumnLength = new Dictionary<int, int>();
                        Session["GridControlsList"] = null;
                        //List for store the column numbers which should hide
                        List<int> hiddenColumnList = new List<int>();
                        //clear the HiddenColumnList session
                        Session["HiddenColumnList"] = null;
                        //Clear the GridColumnWidth session
                        Session["GridColumnWidth"] = null;
                        //Clear the GridColumnLength session
                        Session["GridColumnLength"] = null;

                        IEnumerable entityList = null;
                        Object customerObj = null;
                        if (crmCustomerMstList != null && crmCustomerMstList.Count == 1)
                        {
                            customerObj = crmCustomerMstList[0];//set the current selected customer master obj
                        }
                        retEntityObj = null;
                        Object retEntity = null;
                        if (entityName.Equals(GetLocalResourceObject("CRM_CUSTOMER_MST").ToString()))
                        {
                            //set the master entity list for bind the grid
                            retEntity = crmCustomerMstList;
                        }
                        else
                        {
                            if (customerObj != null)
                            {
                                //Get the EntityCollection that shold be bind to the grid
                                retEntity = GetEntityCollection(customerObj, entityName);
                            }
                        }
                        if (retEntity != null)//If EntityCollection is not null
                        {
                            entityList = (IEnumerable)retEntity;
                            foreach (Object enitityObj in entityList)
                            {
                                drGrid = dtGrid.NewRow();
                                int columnCount = 0;
                                foreach (ADM_FORM_TAB_CONTROL_DTL admFormTabControlDtlObj in admFormTabControlDtlList)
                                {
                                    retVal = string.Empty;
                                    //Get the value 
                                    string value = string.Empty;
                                    string filePath = string.Empty;
                                    if (admFormTabControlDtlObj.ACD_CONTROL == null ||
                                        (admFormTabControlDtlObj.ACD_CONTROL != null && !admFormTabControlDtlObj.ADM_CONTROLS_CFG.CTL_NAME.Equals(ControlTypes.LinkButton.ToString())))
                                    {
                                        if (admFormTabControlDtlObj.ACD_CONTROL_ID.Contains('+'))
                                        {
                                            string[] splitFile = admFormTabControlDtlObj.ACD_CONTROL_ID.Split('+');
                                            if (splitFile.Length == 2)
                                            {
                                                value = GetPropertyValue(enitityObj, splitFile[0]) + GetPropertyValue(enitityObj, splitFile[1]);
                                            }
                                        }
                                        else
                                        {
                                            value = GetPropertyValue(enitityObj, admFormTabControlDtlObj.ACD_CONTROL_ID);
                                        }
                                    }
                                    else
                                    {
                                        if (admFormTabControlDtlObj.ACD_CONTROL_ID.Contains(','))
                                        {
                                            string[] splitFile = admFormTabControlDtlObj.ACD_CONTROL_ID.Split(',');

                                            if (splitFile.Length == 2)
                                            {
                                                value = GetPropertyValue(enitityObj, splitFile[0]);
                                                filePath = GetPropertyValue(enitityObj, splitFile[1]);
                                            }
                                        }
                                        else if (admFormTabControlDtlObj.ACD_CONTROL_ID.Contains('+'))
                                        {
                                            string[] splitFile = admFormTabControlDtlObj.ACD_CONTROL_ID.Split('+');
                                            if (splitFile.Length == 2)
                                            {
                                                value = GetPropertyValue(enitityObj, splitFile[0]) + GetPropertyValue(enitityObj, splitFile[1]);
                                            }
                                        }
                                        else
                                        {
                                            value = GetPropertyValue(enitityObj, admFormTabControlDtlObj.ACD_CONTROL_ID);
                                        }
                                    }
                                    if (countRow == 0)//first row
                                    {
                                        dcGrid = new DataColumn();
                                        dcGrid.ColumnName = admFormTabControlDtlObj.ACD_CONTROL_ID;//set column name
                                        dtGrid.Columns.Add(dcGrid);
                                    }

                                    //check the value have to replace or not
                                    if (!string.IsNullOrEmpty(admFormTabControlDtlObj.ACD_VALUE_TEXT))
                                    {
                                        string[] valueTextArray = admFormTabControlDtlObj.ACD_VALUE_TEXT.Split(',');
                                        foreach (string str in valueTextArray)
                                        {
                                            string[] valueArray = str.Split('=');
                                            if (valueArray.Length > 1 && valueArray[0].Equals(value))
                                            {
                                                value = valueArray[1];
                                                break;
                                            }
                                        }
                                    }

                                    //check the value has to be formatted or not
                                    if (!string.IsNullOrEmpty(admFormTabControlDtlObj.ACD_FORMAT) && !string.IsNullOrEmpty(value))
                                    {
                                        decimal decOut;
                                        if (Decimal.TryParse(value, out decOut))
                                            value = String.Format(admFormTabControlDtlObj.ACD_FORMAT, Decimal.Parse(value));
                                    }

                                    //set value to the cell
                                    drGrid[admFormTabControlDtlObj.ACD_CONTROL_ID] = HttpUtility.HtmlDecode(value);
                                    if (admFormTabControlDtlObj.ACD_IS_HIDDEN == 1)//check the cell is set to hidden or not
                                    {
                                        if (!hiddenColumnList.Contains(columnCount))
                                            hiddenColumnList.Add(columnCount);//add to the hiddenfield list
                                    }
                                    else
                                    {
                                        //set the width of the column
                                        double columnWidth = admFormTabControlDtlObj.ACD_WIDTH == null ? 0 : Convert.ToDouble(admFormTabControlDtlObj.ACD_WIDTH);
                                        if (!dicGridColumnWidth.ContainsKey(columnCount))
                                            dicGridColumnWidth.Add(columnCount, columnWidth);
                                        //set the Length of the column
                                        int columnLength = admFormTabControlDtlObj.ACD_LENGTH == null ? 0 : Convert.ToInt32(admFormTabControlDtlObj.ACD_LENGTH);
                                        if (!dicGridColumnLength.ContainsKey(columnCount) && columnLength > 0)
                                            dicGridColumnLength.Add(columnCount, columnLength);

                                    }

                                    if (admFormTabControlDtlObj.ACD_CONTROL != null
                                                                                && admFormTabControlDtlObj.ADM_CONTROLS_CFG.CTL_NAME.Equals(ControlTypes.LinkButton.ToString())
                                                                                && !string.IsNullOrEmpty(filePath))
                                    {
                                        if (!dicGridControls.ContainsKey(countRow + "," + columnCount))
                                            dicGridControls.Add(countRow + "," + columnCount, filePath);
                                    }
                                    //iterate column
                                    columnCount++;
                                }
                                //iterate row
                                countRow++;
                                //add rows to the data table
                                dtGrid.Rows.Add(drGrid);
                                dtGrid.AcceptChanges();
                                if (hiddenColumnList.Count > 0)
                                {
                                    Session["HiddenColumnList"] = hiddenColumnList;
                                }
                                if (dicGridControls.Count > 0)
                                {
                                    Session["GridControlsList"] = dicGridControls;
                                }
                                if (dicGridColumnWidth.Count > 0)
                                {
                                    Session["GridColumnWidth"] = dicGridColumnWidth;
                                }
                                if (dicGridColumnLength.Count > 0)
                                {
                                    Session["GridColumnLength"] = dicGridColumnLength;
                                }


                            }
                        }

                    }
                    if (dtGrid.Rows.Count > 0)
                    {
                        HiddenField hdfGridSortValue = (HiddenField)pnlControls.FindControl("hdf" + grd.ID + "SortValue");
                        if (hdfGridSortValue != null && !string.IsNullOrEmpty(hdfGridSortValue.Value))
                        {
                            string colName = hdfGridSortValue.Value;
                            string direction = Resources.Report.SortAscending;
                            dtGrid.DefaultView.Sort = colName + " " + direction;
                            dtGrid = dtGrid.DefaultView.ToTable();
                        }
                    }
                    //Bind grid
                    grd.DataSource = dtGrid;
                    grd.DataBind();
                    #region pager
                    //string usercontrolpath = System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString() == "" ?
                    //                        "~/UserControls/PagerControl.ascx" :
                    //                        "/" + System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString() +
                    //                        "UserControls/PagerControl.ascx";
                    //string sql = "";
                    //string countsql = "";
                    //countsql = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString();
                    //if (Session["ParentPK"] != null)
                    //{
                    //    sql = sql.Replace("Key", Session["ParentPK"].ToString());
                    //}
                    //else
                    //{
                    //    sql = sql.Replace("Key", "0");
                    //}
                    //if (countsql == "")
                    //{
                    //    countsql = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString();
                    //}
                    //if (Session["ParentPK"] != null)
                    //{
                    //    countsql = countsql.Replace("Key", Session["ParentPK"].ToString());
                    //}
                    //else
                    //{
                    //    countsql = countsql.Replace("Key", "0");
                    //}
                    //div.Controls.Add(grd);
                    #endregion
                }
            }
            #endregion
        }

        /// <summary>
        /// Method for Bind Grid
        /// </summary>
        /// <param name="controlType"></param>
        public void BindGrid(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.TAXPOPUPGRID:
                case ControlsEnum.TAXPOPUPGRIDTEMP:
                    if (dtTaxDetails == null)
                    {
                        GetFieldValues(ControlsEnum.TAXTYPES);
                    }
                    if (CrmCustTaxDetailsTemp != null)
                    {
                        grdTaxDetails.DataSource = CrmCustTaxDetailsTemp.Select(x => new
                        {
                            x.CMT_PK,
                            x.CMT_TAX,
                            x.CMT_CUSTOMER,
                            x.CMT_CUST_ITEM,
                            CMT_NAME = x.FIN_TAX_MST != null
                                        ? x.FIN_TAX_MST.TAX_HEAD
                                        : dtTaxDetails.Select("TAX_PK=" + x.CMT_TAX.ToString()).FirstOrDefault()["TAX_HEAD"]
                        });
                    }
                    else
                    {
                        grdTaxDetails.DataSource = null;
                    }
                    grdTaxDetails.DataBind();
                    break;
            }
        }

        /// <summary>
        /// Method for creating datatable from list
        /// </summary>
        /// <returns></returns>      
        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others          will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        /// <summary>
        /// Get Entity Collection Object to be bind to the grid
        /// </summary>
        /// <param name="entityObj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private Object GetEntityCollection(Object entityObj, string property)
        {
            //Get type of the object
            Type objType = entityObj.GetType();
            string[] split = property.Split('.');
            PropertyInfo propObj;
            if (split.Length > 1)//If the property is referenced one
            {
                //Get the parent obj
                propObj = objType.GetProperties().AsEnumerable().SingleOrDefault(zz => zz.Name == split[0]);
                if (propObj != null && propObj.GetValue(entityObj, null) != null)//If an entity is there
                {
                    //Get the Ienumerable of the obj
                    IEnumerable tempEnum = (IEnumerable)propObj.GetValue(entityObj, null);
                    foreach (Object tempObj in tempEnum)
                    {
                        if (Session[split[0]] != null)//if the session is not null
                        {
                            PropertyInfo tempPropObj;
                            tempPropObj = null;
                            //Get the property that ends with "PK"
                            tempPropObj = tempObj.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                            if (tempPropObj != null && tempPropObj.GetValue(tempObj, null) != null)
                            {
                                //If the session value matches to any of the object in the enumerable obj list
                                if (Convert.ToInt32(tempPropObj.GetValue(tempObj, null)) == Convert.ToInt32(Session[split[0]].ToString()))
                                {
                                    //Recursively call the same function with that selected obj
                                    GetEntityCollection(tempObj, property.Replace(property.Remove(property.IndexOf('.') + 1), ""));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //Recursively call the same function with the first obj in the list
                            GetEntityCollection(tempObj, property.Replace(property.Remove(property.IndexOf('.') + 1), ""));
                            break;
                        }
                    }
                }
                else//if not get any entity
                {
                    return null;
                }
            }
            else//if it the last level property
            {
                propObj = objType.GetProperties().AsEnumerable().SingleOrDefault(zz => zz.Name == property);
                if (propObj != null)
                {
                    //get and set the entity collection
                    retEntityObj = propObj.GetValue(entityObj, null) == null ? null : propObj.GetValue(entityObj, null);
                }
                else
                {
                    return null;
                }
            }
            return retEntityObj;
        }

        /// <summary>
        /// Get the property value to be show
        /// </summary>
        /// <param name="entityObj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private string GetPropertyValue(Object entityObj, string property)
        {
            //Check whether the object is EntityCollection or not
            if (entityObj.GetType().Name.ToLower().StartsWith("EntityCollection".ToLower()))
            {
                IList entitlyListObj = ((IListSource)entityObj).GetList();
                if (entitlyListObj != null && entitlyListObj.Count > 0)
                {
                    entityObj = entitlyListObj[0];
                }
            }
            //Get the object type
            Type objType;
            objType = entityObj.GetType();
            string[] split = property.Split('.');
            PropertyInfo propObj;
            if (split.Length > 1)//if it is not the last level property
            {
                // get the entity object
                propObj = objType.GetProperties().AsEnumerable().SingleOrDefault(prop => prop.Name == split[0]);
                if (propObj != null && propObj.GetValue(entityObj, null) != null)
                {
                    //recursively call the same function
                    GetPropertyValue(propObj.GetValue(entityObj, null), property.Replace(property.Remove(property.IndexOf('.') + 1), ""));
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                // get the property obj
                propObj = objType.GetProperties().AsEnumerable().SingleOrDefault(prop => prop.Name == property);
                if (propObj != null)
                {
                    //get and set the value from the property obj
                    if (propObj.GetValue(entityObj, null) != null && !string.IsNullOrEmpty(propObj.GetValue(entityObj, null).ToString()))
                    {
                        if (propObj.GetValue(entityObj, null).GetType().Name.ToUpper().Equals(ControlTypes.DateTime.ToString().ToUpper())
                            || propObj.GetValue(entityObj, null).GetType().Name.ToUpper().Equals(ControlTypes.Date.ToString().ToUpper()))
                        {
                            //Format Datetime
                            retVal = Convert.ToDateTime(propObj.GetValue(entityObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                        }
                        else
                        {
                            retVal = propObj.GetValue(entityObj, null).ToString();
                        }
                    }
                    else
                    {
                        retVal = string.Empty;
                    }

                }
                else
                {
                    return string.Empty;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Set the entity for Save or Update
        /// </summary>
        /// <param name="parentEntity"></param>
        /// <param name="entityName"></param>
        private void SaveOrUpdateEntity(Object parentEntity, string entityName)
        {
            string namespaceString = "ERPData";
            string[] entityNameArray = entityName.Split('.');
            string className = "";
            Object retEntity = null;
            if (entityNameArray.Length > 1)//it is not the last level entity
            {
                className = entityNameArray[0];
                className = namespaceString + "." + className;
                retEntityObj = null;
                //get the entitycollection object
                retEntity = GetEntityCollection(parentEntity, entityNameArray[0]);
                if (retEntity != null)//if has an entitycollection object
                {
                    int childPK;
                    if (Session[entityNameArray[0]] == null)//if session is null
                    {
                        //create new instance for the current level entity
                        Assembly currentAssembly = Assembly.Load(namespaceString);
                        Type baseEntity = currentAssembly.GetType(className);
                        object entityObj = Activator.CreateInstance(baseEntity, null);
                        if (entityObj != null)// if instance created
                        {
                            //set values from UI to the object
                            object entity = SetUIValuesToObject(ActionsEnum.SAVE, entityObj);
                            if (entity != null)
                            {
                                //get the entitycollection
                                IListSource entitySourceList = (IListSource)retEntity;
                                if (entitySourceList != null)
                                {
                                    //add the new instance to the current entitycollection
                                    entitySourceList.GetList().Add(entity);
                                    //Recurcively call the same function
                                    SaveOrUpdateEntity(entity, entityName.Replace(entityName.Remove(entityName.IndexOf('.') + 1), ""));
                                }
                            }
                        }
                    }
                    else//if session is not null
                    {
                        //get the pk from the session
                        childPK = Convert.ToInt32(Session[entityNameArray[0]].ToString());
                        IEnumerable entityEnumList = null;
                        entityEnumList = (IEnumerable)retEntity;
                        Object updateEntity = null;
                        if (entityEnumList != null)
                        {
                            foreach (Object entityobj in entityEnumList)
                            {
                                PropertyInfo propObj;
                                propObj = null;
                                //Get the property obj that ends with "PK"
                                propObj = entityobj.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                                if (propObj != null && propObj.GetValue(entityobj, null) != null)
                                {
                                    if (Convert.ToInt32(propObj.GetValue(entityobj, null)) == childPK)//if pk is same
                                    {
                                        #region Seperate File Upload Table- Now it doesn't use
                                        //Test
                                        //if (entityName.Contains("CRM_CUST_DOCUMENT_DTL"))
                                        //{
                                        //    PropertyInfo propDocument;
                                        //    propDocument = null;
                                        //    propDocument = entityobj.GetType().GetProperties().AsEnumerable().LastOrDefault(zz => zz.Name.EndsWith("_DOCUMENT"));
                                        //    if (propDocument != null && propDocument.GetValue(entityobj, null) != null)
                                        //    {
                                        //        if(dicFileParentPK.ContainsKey("CDD_PK"))
                                        //            dicFileParentPK.Remove("CDD_PK");
                                        //        dicFileParentPK.Add("CDD_PK", Convert.ToInt32(propDocument.GetValue(entityobj, null)));
                                        //    }
                                        //}
                                        //
                                        #endregion

                                        updateEntity = entityobj;
                                        break;
                                    }
                                }
                            }
                            if (updateEntity != null)//if updation
                            {
                                //set values from UI to the object
                                object entity = SetUIValuesToObject(ActionsEnum.SAVE, updateEntity);
                                if (entity != null)
                                {
                                    //Recurcively call the same function
                                    SaveOrUpdateEntity(entity, entityName.Replace(entityName.Remove(entityName.IndexOf('.') + 1), ""));
                                }
                            }
                        }
                    }
                }
            }
            else if (entityNameArray.Length == 1)//If it is the last level entity
            {
                className = entityNameArray[0];
                className = namespaceString + "." + className;
                retEntityObj = null;
                //Get entitycollection obj
                retEntity = GetEntityCollection(parentEntity, entityNameArray[0]);
                #region Seperate File Upload Table- Now it doesn't use
                //if (entityNameArray[0].Equals("CRM_CUST_DOCUMENT_DTL"))
                //{
                //    retEntity = new System.Data.Objects.DataClasses.EntityCollection<CRM_CUST_DOCUMENT_DTL>();
                //}
                #endregion


                if (retEntity != null)
                {
                    int childPK;
                    #region Seperate File Upload Table- Now it doesn't use
                    //Test start
                    //if (entityNameArray[0].Equals("CRM_CUST_DOCUMENT_DTL"))
                    //{
                    //    if (dicFileParentPK != null && dicFileParentPK.Count > 0 && dicFileParentPK.ContainsKey("CDD_PK"))
                    //    {
                    //        foreach (KeyValuePair<string, int> pk in dicFileParentPK)
                    //        {
                    //            if (pk.Key.Equals("CDD_PK"))
                    //            {
                    //                Session[entityNameArray[0]]=pk.Value;
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                    //
                    //
                    #endregion

                    if (Session[entityNameArray[0]] == null)//If session is null
                    {
                        //create new instance for the current level entity
                        Assembly currentAssembly = Assembly.Load(namespaceString);
                        Type baseEntity = currentAssembly.GetType(className);
                        object entityObj = Activator.CreateInstance(baseEntity, null);
                        if (entityObj != null)
                        {
                            //set values from UI to the object
                            object entity = SetUIValuesToObject(ActionsEnum.SAVE, entityObj);
                            if (entity != null)
                            {
                                //get the entitycollection
                                IListSource entitySourceList = (IListSource)retEntity;
                                if (entitySourceList != null)
                                {
                                    //add the new instance to the current entitycollection
                                    entitySourceList.GetList().Add(entity);
                                    doSave = true;
                                }
                            }
                        }
                    }
                    else//If session is not null
                    {
                        //Get the PK value from session
                        childPK = Convert.ToInt32(Session[entityNameArray[0]].ToString());
                        IEnumerable entityEnumList = null;
                        entityEnumList = (IEnumerable)retEntity;
                        Object updateEntity = null;
                        if (entityEnumList != null)
                        {
                            foreach (Object entityobj in entityEnumList)
                            {
                                PropertyInfo propObj;
                                propObj = null;
                                //Get the property obj that ends with "PK"
                                propObj = entityobj.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                                if (propObj != null && propObj.GetValue(entityobj, null) != null)
                                {
                                    if (Convert.ToInt32(propObj.GetValue(entityobj, null)) == childPK)//if pk is same
                                    {
                                        updateEntity = entityobj;
                                        break;
                                    }
                                }
                            }
                            if (updateEntity != null)//if updation
                            {
                                //set values from UI to the object
                                object entity = SetUIValuesToObject(ActionsEnum.SAVE, updateEntity);
                                if (entity != null)
                                {
                                    doSave = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Set Configuration settings
        /// </summary>
        private void ConfigurationSettings()
        {
            IsBrandItemInsert = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "BrandItemInsert")));

        }
        private bool IsSBUCustomer()
        {
            bool result = false;
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                result = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? true : false;
            }
            return result;
        }


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
            //if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            //    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            //else
            //    path = Request.Url.AbsolutePath.ToLower();
            path = Resources.PageURL.ActivityWkfURL;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
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
            string s = "";
            //this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            //this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
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
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
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
            bool isWkf;
            isWkf = false;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //If the page load from menu, need to reset session to List page
            if (Request.QueryString["Tab"] != null && Request.QueryString["Tab"].ToString().Equals(TabType.CLST))
                Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = TabType.CLST;
            //first time this session will set from login itself
            if (Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK] != null)//If it is a customer user
            {
                if (Request.QueryString["Tab"] != null && Request.QueryString["Tab"].ToString().Equals(TabType.CLST))//if navigation comes from menu, then the tabcode in the url will be CLST. so redirect to CUS 
                {
                    Response.Redirect(Page.ResolveClientUrl("~/OrderToCash/CustomerRegistration.aspx?Tab=CUS"), true);//Redirect to Basic Info
                }
                CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK].ToString());
                if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] == null)
                {
                    Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = TabType.CUS;
                    TabCode = Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString();
                    isWkf = true;
                }
                //Repeater rtrDynamicTab= (Repeater)CustomerRegistrationTabs.FindControl("rtrDynamicTab");
                //if (rtrDynamicTab != null)
                //{
                //    foreach (RepeaterItem item in rtrDynamicTab.Items)//Iterate tab list
                //    {
                //        if (((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CommandArgument.Equals(TabType.CLST))
                //        {
                //            ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).Visible = false;
                //            break;
                //        }
                //    }
                //}
            }
            //get tabcode from query string
            if (Request.QueryString["Tab"] != null && Request.QueryString["Tab"].ToString().Equals(TabType.CUS))
            {
                TabCode = Request.QueryString["Tab"].ToString();
                Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = TabCode;
                isWkf = true;
            }
            if (!isWkf)
            {
                if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] == null)
                {
                    //Get dynamic tab list
                    GetFieldValues(ControlsEnum.DYNAMICTABS);
                    if (spAdmFormTabCfgGetResultList != null)
                    {
                        //set tabcode as the tab code of first tab
                        Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = spAdmFormTabCfgGetResultList[0].ATC_CODE;
                        TabCode = Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString();
                    }
                }
                else
                {
                    TabCode = Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString();
                }
            }
            ////first time this session will set from login itself
            //if (Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK] != null)
            //{
            //    CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK].ToString());
            //    Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = TabType.CUS;
            //    TabCode = Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString();
            //}
            //Get dynamic controls
            GetFieldValues(ControlsEnum.DEFAULT);
            //Load Controls to UI
            LoadControls();
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
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
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
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                }
                //else if (EntryStatus == EntryStatus.SAVEONLY)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        [Serializable]
        [XmlRoot("ROOT")]
        public class BrandItemInsertBO
        {
            [XmlElement("CIM_PK")]
            public int CIM_PK { get; set; }
            [XmlElement("CIM_BRAND_CODE")]
            public string CIM_BRAND_CODE { get; set; }
            [XmlElement("CIM_BRAND_NAME")]
            public string CIM_BRAND_NAME { get; set; }
            [XmlElement("CIM_PRODUCT_CODE")]
            public string CIM_PRODUCT_CODE { get; set; }
            [XmlElement("CIM_PRODUCT_NAME")]
            public string CIM_PRODUCT_NAME { get; set; }
            [XmlElement("CIM_CUSTOMER_PK")]
            public string CIM_CUSTOMER_PK { get; set; }
            [XmlElement("CIM_BIZUNIT")]
            public int CIM_BIZUNIT { get; set; }

        }
        #region Enum
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
            DateRange,
            Password

        }

        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            DYNAMICTABS,
            CUSTOMER,
            TAXTYPES,
            TAXPOPUPGRID,
            TAXPOPUP,
            TAXPOPUPGRIDTEMP,
            PACKINGSPEC
        }

        public enum PopUpType
        {
            CUSTOMERTAX,
            BRANDITEMTAX
        }

        #endregion
    }
}
#region Notes
//CRM_CUST_DOCUMENT_DTL is the table to keep the files uploaded in customer registration section. But now we dont use this tale
#endregion
