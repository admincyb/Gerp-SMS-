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
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);

            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }

        /// <summary>
        /// Current PK
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
                return this.ViewState[ViewstateStrings.RelatedControlID]==null?string.Empty:(this.ViewState[ViewstateStrings.RelatedControlID]).ToString();
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
        /// TabCode--Used to store tab code
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
        #endregion
        User currentUser;
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private CRM_CUSTOMER_MST crmCustomerMstObj;
        ServiceUtility serviceUtilityObj;
        //page related Entity Objects
        //private AD_APP_CONST_CFG adAppConstCfgObj;

        //List for binding details to controls            
        private List<SPADM_FORM_TAB_CONTROL_CFG_GET_Result> spAdmFormTabControlCfgGetResultList;
        private List<SPADM_FORM_TAB_CFG_GET_Result> spAdmFormTabCfgGetResultList;
        private List<CRM_CUSTOMER_MST> crmCustomerMstList;
        private List<ADM_FORM_TAB_CONTROL_DTL> admFormTabControlDtlList;
        CustomerRegistrationService customerRegistrationServiceClient;
        private const string CustomerStatusFilterControlId = "DDL_CUSTOMER_STATUS_FILTER";
        private const string CustomerStatusFilterSessionKey = "CustomerListActiveStatus";
        private const string CustomerListGridControlId = "GRD_LST";
        string retVal;
        Object retEntityObj = null;
        bool doSave = false; 
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                        if (Session[ERP.Utilities.SessionStrings.CUSTOMERPK] != null)
                        {
                            CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CUSTOMERPK].ToString());//Main Entity PK
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
                                    case TabType.CCD:
                                        break;
                                    case TabType.CRD:
                                        break;
                                    case TabType.CBD:
                                        break;
                                    case TabType.CIM:
                                        break;
                                    case TabType.TCH:
                                        break;
                                    default:
                                        break;
                                }

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
                        spAdmFormTabControlCfgGetResultList = DynamicPageServiceClient.GetFormTabControlsList(FormType.CUS, tabCode);
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
                        crmCustomerMstObj.CUS_ACTIVE = TabCode == TabType.CLST || CurrPK > 0
                            ? GetCustomerListActiveStatus()
                            : Convert.ToByte(DbActiveStatus.ACTIVE);
                        crmCustomerMstList = customerRegistrationServiceClient.GetCrmCustomerMst(crmCustomerMstObj, serviceUtilityObj);
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
        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
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
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
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
                                        if (entityName.Equals("CRM_CUSTOMER_MST"))//If the entity is customer Master
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
                                            }
                                            else
                                            {
                                                //create new instance of customer master 
                                                crmCustomerMstObj = new CRM_CUSTOMER_MST();
                                            }
                                            //Get values from UI controls to entity
                                            crmCustomerMstObj = (CRM_CUSTOMER_MST)SetUIValuesToObject(ActionsEnum.SAVE, crmCustomerMstObj);
                                            if (CurrPK == 0)
                                            {
                                                crmCustomerMstList.Add(crmCustomerMstObj);
                                            }
                                            //Save customer master operation
                                            result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                            if (result >= 0) // Success ! re-initialize the page
                                            {
                                                litErrorMsg.Text = Resources.Report.Msg_Save_Success;
                                                //set dynamic message
                                                message = CustomerRegistrationTabs.DynamicTabDesc;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), message);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration) + "');", true);
                                            }
                                        }
                                        else
                                        {
                                            doSave = false;
                                            //Set the UI values to the curresponding entity. If this succesfully completed return true, otherwise return false
                                            SaveOrUpdateEntity(crmCustomerMstList[0], entityName);
                                            if (doSave)
                                            {
                                                //Save operation. For saving we will pass only the master entity, ie, Customer master entity. 
                                                //It wil save/update/delete the whole subentities(only status changed ones). 
                                                //If saving is succesfully completed return value will be the selected customer master pk
                                                result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                                if (result >= 0) // Success ! re-initialize the page
                                                {
                                                    litErrorMsg.Text = Resources.Report.Msg_Save_Success;
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
                                                        //Show save success message and redirect to the same page
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration) + "');", true);
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
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
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
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                            }
                            else//valid
                            {
                                //create new instance of service. This object should be maintain until all the save operation is completed. This is for maintaining the entity object context
                                customerRegistrationServiceClient = new CustomerRegistrationService();
                                customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                //Get the customer Master details
                                GetFieldValues(ControlsEnum.CUSTOMER);
                                if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                {
                                    //Get the entity name of the continue button. The details will be saved to this entity
                                    HiddenField hdfEntity = (HiddenField)pnlControls.FindControl("hdf" + ((Button)sender).ID + "Entity");
                                    if (hdfEntity != null && !string.IsNullOrEmpty(hdfEntity.Value))
                                    {
                                        string entityName = hdfEntity.Value;
                                        if (entityName.Equals("CRM_CUSTOMER_MST"))//If the entity is customer Master
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
                                            }
                                            else
                                            {
                                                //create new instance of customer master 
                                                crmCustomerMstObj = new CRM_CUSTOMER_MST();
                                            }
                                            //Get values from UI controls to entity
                                            crmCustomerMstObj = (CRM_CUSTOMER_MST)SetUIValuesToObject(ActionsEnum.SAVE, crmCustomerMstObj);
                                            if (CurrPK == 0)
                                            {
                                                crmCustomerMstList.Add(crmCustomerMstObj);
                                            }
                                            //Save customer master operation
                                            result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                            if (result >= 0) // Success ! re-initialize the page
                                            {
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
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration) + "');", true);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            doSave = false;
                                            //Set the UI values to the curresponding entity. If this succesfully completed return true, otherwise return false
                                            SaveOrUpdateEntity(crmCustomerMstList[0], entityName);
                                            if (doSave)
                                            {
                                                //Save operation. For saving we will pass only the master entity, ie, Customer master entity. 
                                                //It wil save/update/delete the whole subentities which the status is change. 
                                                //If saving is succesfully completed return value will be the selected customer master pk
                                                result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                                if (result >= 0) // Success ! re-initialize the page
                                                {
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
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration) + "');", true);
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
                                                        if (entityName.Equals("CRM_CUSTOMER_MST"))//If the entity is customer Master
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
                                                                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration), false);
                                                                            Context.ApplicationInstance.CompleteRequest();
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
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){return hideHdr();});", true);
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
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('Please select a row to perform the action','" + Resources.Messages.Information + "');", true);
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
                                                        if (entityName.Equals("CRM_CUSTOMER_MST"))//If the entity is customer Master
                                                        {
                                                            //set the customerpk session
                                                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = SelectedPK;
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
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration) + "');", true);
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
                                                                                result = customerRegistrationServiceClient.SaveCrmCustomerMst(crmCustomerMstList);
                                                                                if (result >= 0) // Success ! re-initialize the page
                                                                                {
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
                                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration) + "');", true);
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
                                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                                                    }
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
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('Please select a row to perform the action','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            break;
                        #endregion
                        #region New
                        case ActionsEnum.NEW:
                            //Get the Group number of the save button.
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
                                            if (entityName.Equals("CRM_CUSTOMER_MST"))//If the entity is customer master
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
                                                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration), false);
                                                                Context.ApplicationInstance.CompleteRequest();
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
                                                        //Js for hide the header part
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){return hideHdr();});", true);
                                                    }
                                                    else//If the tab doesn't have subtab
                                                    {
                                                        //Redirect to the same tab
                                                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration), false);
                                                        Context.ApplicationInstance.CompleteRequest();
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
                            if (((DropDownList)sender).ID == CustomerStatusFilterControlId)
                            {
                                Session[CustomerStatusFilterSessionKey] = ((DropDownList)sender).SelectedValue;
                                RebindCustomerListingGrid();
                                break;
                            }

                            commonServiceClient = new CommonService();
                            commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                            if (!string.IsNullOrEmpty(((DropDownList)sender).SelectedValue))
                            {
                                //get the selected value from the dropdown
                                int itemPK = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                                //Get the Spname curresponding to the dropdown
                                HiddenField hdfAction = (HiddenField)pnlControls.FindControl("hdf" + ((DropDownList)sender).ID + "Action");
                                if (hdfAction != null && !string.IsNullOrEmpty(hdfAction.Value))
                                {
                                    //Get the sp name
                                    string spName = hdfAction.Value;
                                    // set the parameters of the sp
                                    object[] methodParams = new object[] { itemPK };
                                    //Execute SP
                                    Object retObj = commonServiceClient.ExecuteSP(spName, methodParams);
                                    if (retObj != null)
                                    {
                                        //SP retuurns Objectresult
                                        ObjectResult objResult = (ObjectResult)retObj;
                                        int count = 0;
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
                                                        if (entityName.Equals("CRM_CUSTOMER_MST"))//If the entity is customer Master
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
                                                                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration), false);
                                                                            Context.ApplicationInstance.CompleteRequest();
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
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('Please select a row to perform the action','" + Resources.Messages.Information + "');", true);
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
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration), false);
                            Context.ApplicationInstance.CompleteRequest();
                            break;
                        #endregion
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                customerRegistrationServiceClient = null;
                commonServiceClient = null;
            }
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
                    //check hidden column list and hide the curresponding column of the grid
                    if (Session["HiddenColumnList"] != null)
                    {
                        List<int> hiddenColumnList = (List<int>)Session["HiddenColumnList"];
                        foreach (int columnIndex in hiddenColumnList)
                        {
                            e.Row.Cells[columnIndex].Visible = false;
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
                BindGrid(controlPK, "CRM_CUSTOMER_MST", customerListGrid);
            }
        }

        private void ApplyRequestedTabFromQuery()
        {
            string requestedTabCode = Request.QueryString["Tab"];
            if (string.IsNullOrEmpty(requestedTabCode))
            {
                return;
            }

            GetFieldValues(ControlsEnum.DYNAMICTABS);
            if (spAdmFormTabCfgGetResultList == null || spAdmFormTabCfgGetResultList.Count == 0)
            {
                return;
            }

            SPADM_FORM_TAB_CFG_GET_Result requestedTab = spAdmFormTabCfgGetResultList
                .FirstOrDefault(tab => tab.ATC_CODE.Equals(requestedTabCode, StringComparison.OrdinalIgnoreCase));
            if (requestedTab == null)
            {
                return;
            }

            Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] = requestedTab.ATC_CODE;
            Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTEDTAB_NAME] = requestedTab.ATC_NAME;
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
            try
            {
                switch (mode)
                {
                    case ActionsEnum.SAVE:
                        Type targetTable = srcObj.GetType();
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
                                        #region Text
                                        case ControlTypes.Text:
                                            TextBox txtCtrlId = (TextBox)pnlControls.FindControl(controlID);
                                            if (txtCtrlId != null)
                                            {
                                                SetValue(srcObj, p, txtCtrlId.Text.Trim());
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
                                                    }
                                                    else
                                                    {
                                                        HiddenField hdfRelId = (HiddenField)pnlControls.FindControl("hdf" + controlID + "HasRelatedId");
                                                        if (hdfRelId != null)
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
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("MOD_DT"))
                                                {
                                                    SetValue(srcObj, p, DateTime.Now.ToString());
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("ACTIVE"))
                                                {
                                                    SetValue(srcObj, p, "1");
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("STATUS"))
                                                {
                                                    SetValue(srcObj, p, "0");
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("BIZUNIT"))
                                                {
                                                    SetValue(srcObj, p, currentUser.SBUID.ToString());
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("CRTD_BY"))
                                                {
                                                    SetValue(srcObj, p, currentUser.PKUser.ToString());
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("CRTD_DT"))
                                                {
                                                    SetValue(srcObj, p, DateTime.Now.ToString());
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("DELETED"))
                                                {
                                                    SetValue(srcObj, p, false.ToString());
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(hdfCtrlId.Value))
                                                    {
                                                        SetValue(srcObj, p, hdfCtrlId.Value);
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
                                            }
                                            break;
                                        #endregion
                                        #region FileUpload
                                        case ControlTypes.FileUpload:
                                            FileUpload fileUploadCtrlId = (FileUpload)pnlControls.FindControl(controlID);
                                            if (fileUploadCtrlId != null)
                                            {
                                                
                                                //if (fileUploadCtrlId.HasFile)
                                                //{
                                                    //SetValue(srcObj, p, fileUploadCtrlId.FileName);
                                                //}
                                                /////Test start
                                                HttpPostedFile po = fileUploadCtrlId.PostedFile;
                                                string SavePath;
                                                FileInfo tempFileInfoObj;
                                                string attachmentFileFormat;
                                                string attachmentFilePath;
                                                FileInfo attachedFileInfo;
                                                if (fileUploadCtrlId.HasFile)
                                                {
                                                    SetValue(srcObj, p, fileUploadCtrlId.FileName);

                                                    SavePath = HttpContext.Current.Request.PhysicalApplicationPath + "/";
                                                    tempFileInfoObj = new FileInfo(fileUploadCtrlId.PostedFile.FileName);

                                                    if (!Directory.Exists(SavePath + "Upload/"))
                                                        Directory.CreateDirectory(SavePath + "Upload/");
                                                    attachmentFileFormat = tempFileInfoObj.Extension;
                                                    AttachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                                    attachmentFilePath = "Upload/" + AttachmentFileName;
                                                    attachedFileInfo = new FileInfo(SavePath + attachmentFilePath);

                                                    HttpContext.Current.Request.Files[0].SaveAs(attachedFileInfo.FullName);
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage("No file!") + "','" + Resources.ErpRes.Information + "');", true);
                                                }

                                                if (AttachmentFileName == Guid.Empty.ToString() || AttachmentFileName == string.Empty)
                                                {
                                                    AttachmentFileName = string.Empty;
                                                }
                                                ////Test End
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
                        break;
                    
                    default:
                        break;
                }
                returnObj = srcObj;
                return returnObj;
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
            if (ptype == typeof(byte))
                p.SetValue(src, Convert.ToByte(value), null);
            if (ptype == typeof(string))
                p.SetValue(src, HttpUtility.HtmlEncode(value), null);
            else if (ptype == typeof(int) || ptype == typeof(int?))
                p.SetValue(src, Convert.ToInt32(value), null);
            else if (ptype == typeof(Int64) || ptype == typeof(Int64?))
                p.SetValue(src, Convert.ToInt64(value), null);
            else if (ptype == typeof(Int32) || ptype == typeof(Int32?))
                p.SetValue(src, Convert.ToInt32(value), null);
            else if (ptype == typeof(short) || ptype == typeof(short?))
                p.SetValue(src, Convert.ToInt16(value), null);
            else if (ptype == typeof(float) || ptype == typeof(float?))
                p.SetValue(src, float.Parse(value), null);
            else if (ptype == typeof(Double) || ptype == typeof(Double?))
                p.SetValue(src, Double.Parse(value), null);
            else if (ptype == typeof(decimal) || ptype == typeof(decimal?))
                p.SetValue(src, Convert.ToDecimal(value), null);
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
                                        //fileUploadCtrlId.FileName = p.GetValue(srcObj, null) == null ? string.Empty : (string)p.GetValue(srcObj, null);
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
            int tempSequence=0;
            Dictionary<int, string> dicEntityGroup;
            dicEntityGroup = new Dictionary<int, string>();
            List<string> validationGroupList;
            validationGroupList = new List<string>();
            TableCell hiddenTableCell;
            hiddenTableCell = new TableCell();
            hiddenTableCell.Visible = false;
            Table hiddenTable= new Table();
            hiddenTable.Visible = false;
            TableRow hiddenTableRow = new TableRow();
            hiddenTableRow.Visible = false;
            string tempDivGroupStyle = "";
            string tempDivColStyle = "";
            bool isButtonGroup = true;
            string validaionGroup;
            string[] valdationGroupArray;
            //Get the column number of UI
            Cols = 2;//Convert.ToInt32(adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("ColLayoutActivity").ToString()).CNS_Value);
            if (Cols == 1)//One column UI
            {
                divColStyle = "divcolmiddle-S";// adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("SingleColStyle").ToString()).CNS_Data;
                tempDivColStyle = divColStyle;
                divGroupStyle = "fields-grpwrap single";// adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("SingleColGroupStyle").ToString()).CNS_Data;
                tempDivGroupStyle = divGroupStyle;
            }
            else//Greater than one column UI
            {
                divColStyle = "div2col-M";// adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("DoubleColStyle").ToString()).CNS_Data;
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
                        for (int k = 0; k < drFieldControls.Count(); )
                        {
                            //Create new Table Object
                            Table tbControls = new Table();
                            if (drFieldControls.Count() > 1)
                            {
                                tbControls.CssClass = "table-devide";
                            }
                            else if (drFieldControls.Count() == 1 && Cols > 1
                                && ((drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()).Equals("TextArea")
                                || (drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()).Equals("Label")))//TextArea and the Label can only come in full length row(column number >1)
                            {
                                tbControls.CssClass = "";
                                divColStyle = "";
                            }
                            else
                            {
                                tbControls.CssClass = "table-devide";
                                divColStyle = tempDivColStyle;
                            }
                            //Create new TableRow Object
                            TableRow trControls = new TableRow();
                            int j = 0;
                            //Column Iterate
                            for (j = 0; j < Cols && k < drFieldControls.Count(); )
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
                                            || tempDrFieldControls[k - 1]["ACC_CONTROL_TEXT"].ToString().Equals("LinkButton")))//If control is not the first one in the current group and it is a button or link button
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
                                                    && !tempDr[x]["ACC_CONTROL_TEXT"].ToString().Equals("LinkButton"))//check the control is a button or linkbutton. if any control is not a but or lnkbut, its not a button group
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
                                            || tempDrFieldControls[k]["ACC_CONTROL_TEXT"].ToString().Equals("LinkButton")))
                                        {
                                            DataRow[] tempDr = tempDT.Select("ACC_CONTROL_GROUP = " + order);
                                            isButtonGroup = true;
                                            for (int x = 0; x < tempDr.Count(); x++)
                                            {
                                                if (!tempDr[x]["ACC_CONTROL_TEXT"].ToString().Equals("Button")
                                                    && !tempDr[x]["ACC_CONTROL_TEXT"].ToString().Equals("LinkButton"))
                                                {
                                                    isButtonGroup = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                //set the temsequence
                                tempSequence = int.Parse(drFieldControls[k]["ACC_SEQUENCE"].ToString());
                                bool isFullLengthControl = drFieldControls[k]["ACC_IS_FULL_LENGTH"] != null
                                    && drFieldControls[k]["ACC_IS_FULL_LENGTH"].ToString() == "1";
                                //set style for the inner div
                                div.Attributes.Add("class", divColStyle);
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
                                                    AssociatedControlID = "lbl" + drFieldControls[k]["ACC_CONTROL_ID"].ToString()
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
                                        if (drFieldControls[k]["ACC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_STYLE"].ToString() != string.Empty)
                                                txt.CssClass = drFieldControls[k]["ACC_STYLE"].ToString();
                                        }
                                        if (drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "CIM_BRAND_SPECIFICATION"
                                            || drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "CIM_PACKING_SPEC_DESC")
                                        {
                                            txt.CssClass = string.IsNullOrEmpty(txt.CssClass) ? "custom-product-attributes" : txt.CssClass + " custom-product-attributes";
                                            if (drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "CIM_PACKING_SPEC_DESC")
                                            {
                                                txt.Attributes["style"] = "width:min(75%, calc(100% - 260px)) !important; min-width:0 !important; max-width:none !important; box-sizing:border-box;";
                                            }
                                            else
                                            {
                                                txt.Attributes["style"] = "width:100% !important; min-width:0 !important; max-width:none !important; box-sizing:border-box;";
                                                txt.Width = Unit.Percentage(100);
                                            }
                                            div.Attributes["style"] = "display:grid !important; grid-template-columns:240px minmax(0,1fr) !important; column-gap:10px !important; align-items:center !important; width:100% !important; box-sizing:border-box;";
                                        }
                                        if (drFieldControls[k]["ACC_LENGTH"] != null)
                                        {
                                            if (drFieldControls[k]["ACC_LENGTH"].ToString() != string.Empty)
                                                txt.MaxLength = Convert.ToInt32(drFieldControls[k]["ACC_LENGTH"].ToString());
                                        }
                                        div.Controls.Add(txt);
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
                                                        ErrorMessage = "Enter " + drFieldControls[k]["ACC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrftxt);
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
                                                    ErrorMessage = "Invalid " + drFieldControls[k]["ACC_NAME"].ToString(),
                                                    ValidationExpression = drFieldControls[k]["ACC_FORMAT"].ToString().Trim(),
                                                    EnableClientScript = true,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*"
                                                };
                                                div.Controls.Add(regExObj);
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
                                                    ErrorMessage = "Mismatch " + drFieldControls[k]["ACC_NAME"].ToString()
                                                };
                                                div.Controls.Add(comValObj);
                                            }
                                        }

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
                                            div.Controls.Add(rev);
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
                                                        ErrorMessage = "Enter " + drFieldControls[k]["ACC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrfHrtxt);
                                                }
                                            }
                                        }
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
                                        if (drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "CIM_BRAND_SPECIFICATION"
                                            || drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "CIM_PACKING_SPEC_DESC")
                                        {
                                            txtArea.CssClass = string.IsNullOrEmpty(txtArea.CssClass) ? "custom-product-attributes" : txtArea.CssClass + " custom-product-attributes";
                                            if (drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "CIM_PACKING_SPEC_DESC")
                                            {
                                                txtArea.Attributes["style"] = "width:min(75%, calc(100% - 260px)) !important; min-width:0 !important; max-width:none !important; box-sizing:border-box;";
                                            }
                                            else
                                            {
                                                txtArea.Attributes["style"] = "width:100% !important; min-width:0 !important; max-width:none !important; box-sizing:border-box;";
                                                txtArea.Width = Unit.Percentage(100);
                                            }
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
                                                        ErrorMessage = "Enter " + drFieldControls[k]["ACC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrfTxtArea);
                                                }
                                            }
                                        }
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
                                            div.Attributes.Add("class", "divcol-M");
                                        }
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
                                        date.Attributes.Add("onkeydown", "return false");
                                        date.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(date);
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
                                                        ErrorMessage = "Enter " + drFieldControls[k]["ACC_NAME"].ToString() + " Date"
                                                    };
                                                    div.Controls.Add(vrfDate);
                                                }
                                            }
                                        }

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
                                        dateRange.Attributes.Add("onkeydown", "return false");
                                        dateRange.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(dateRange);
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
                                                        ErrorMessage = "Enter " + drFieldControls[k]["ACC_NAME"].ToString() + " Date"
                                                    };
                                                    div.Controls.Add(vrfDate);
                                                }
                                            }
                                        }

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
                                            string hdfFrmDate = "hdf"+drFieldControls[k]["ACC_REL_CONTROL_ID"].ToString();
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
                                        dttxt.Attributes.Add("onkeydown", "return false");
                                        dttxt.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(dttxt);
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
                                                        ErrorMessage = "Enter " + drFieldControls[k]["ACC_NAME"].ToString() + " Date"
                                                    };
                                                    div.Controls.Add(vrfDate);
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
                                                ErrorMessage = "Enter valid " + drFieldControls[k]["ACC_NAME"].ToString() + " Time"
                                            };
                                            div.Controls.Add(vreTime);
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
                                                        ErrorMessage = "Enter " + drFieldControls[k]["ACC_NAME"].ToString() + " Time"
                                                    };
                                                    div.Controls.Add(vrfTime);
                                                }
                                            }
                                        }
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
                                        PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls[k]["ACC_CONTROL_TEXT"].ToString(), dttxt.ID,null,null,null);
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
                                                ErrorMessage = "Enter valid " + drFieldControls[k]["ACC_NAME"].ToString()
                                            };
                                            div.Controls.Add(vreTimePick);
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
                                                        ErrorMessage = "Enter " + drFieldControls[k]["ACC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrfTimePick);
                                                }
                                            }
                                        }
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
                                                    integerPart = (Convert.ToInt32(drFieldControls[k]["ACC_LENGTH"].ToString())-(Convert.ToInt32(decimalPart)+1)).ToString();
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
                                                    ErrorMessage = "Invalid " + drFieldControls[k]["ACC_NAME"].ToString(),
                                                    ValidationExpression = regex,
                                                    EnableClientScript = true,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*"
                                                };
                                                div.Controls.Add(regEx);
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
                                                        ErrorMessage = "Enter " + drFieldControls[k]["ACC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrf);
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
                                                    ErrorMessage = "Mismatch " + drFieldControls[k]["ACC_NAME"].ToString()
                                                };
                                                div.Controls.Add(comValObj);
                                            }
                                        }
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
                                                ddlDrop.SelectedIndexChanged += new EventHandler(ActionHandler);
                                                ddlDrop.AutoPostBack = true;
                                                HiddenField hdfDDLAction = new HiddenField()
                                                {
                                                    ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Action",
                                                    ClientIDMode = ClientIDMode.Static,
                                                    Value = drFieldControls[k]["ACC_ACTION"].ToString()
                                                };
                                                div.Controls.Add(hdfDDLAction);
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
                                                        DropDownList parent = (DropDownList)pnlControls.FindControl(relId);
                                                        if (parent != null && parent.Items.Count > 1)
                                                        {
                                                            if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                            {
                                                                value = parent.SelectedValue;
                                                            }
                                                        }
                                                    }
                                                }
                                                foreach (string condition in conditionList)
                                                {
                                                    if (Session[condition] != null)//parameter name is same as any session name
                                                    {
                                                        query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                    }
                                                    else if (condition == ERP.Utilities.SessionStrings.CUSTOMERPK)
                                                    {
                                                        query = query.Replace("@" + condition + "@", CurrPK > 0 ? CurrPK.ToString() : "0");
                                                    }
                                                    else if (condition == "BRANDPK")
                                                    {
                                                        query = query.Replace("@" + condition + "@", SelectedPK > 0 ? SelectedPK.ToString() : "0");
                                                    }
                                                    else if (condition == "BIZUNITPK")
                                                    {
                                                        User loggedInUser = HttpContext.Current.User.Identity as User;
                                                        if (loggedInUser != null && loggedInUser.SBUID > 0)
                                                        {
                                                            query = query.Replace("@" + condition + "@", loggedInUser.SBUID.ToString());
                                                        }
                                                    }
                                                    else if (condition == relId)// parameter is value of any other control
                                                    {
                                                        query = query.Replace("@" + condition + "@", value);
                                                    }
                                                }
                                            }
                                            commonService = new CommonService();
                                            commonService = CommonFunctions.InitiateClient(commonService);
                                            //Execute query
                                            List<DDLMaster> ddlValues = commonService.ExecuteQuery(query);
                                            commonService = null;
                                            ddlDrop.DataTextField = "Value";
                                            ddlDrop.DataValueField = "PK";
                                            ddlDrop.DataSource = ddlValues;
                                            ddlDrop.DataBind();
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
                                        div.Controls.Add(ddlDrop);
                                        div.Controls.Add(hdnDrop);
                                        div.Controls.Add(hdnDropPk);
                                        div.Controls.Add(hdnDropUICPk);

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
                                                        ErrorMessage = "Enter " + drFieldControls[k]["ACC_NAME"].ToString(),
                                                        InitialValue = CommonConstants.SELECTVAL
                                                    };
                                                    div.Controls.Add(vrfddl);
                                                }
                                            }
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
                                        btn.ToolTip = drFieldControls[k]["ACC_NAME"].ToString();
                                        if (drFieldControls[k]["ACC_VALD_GROUP"] != null && !string.IsNullOrEmpty(drFieldControls[k]["ACC_VALD_GROUP"].ToString()))
                                        {
                                            btn.ValidationGroup = drFieldControls[k]["ACC_VALD_GROUP"].ToString();
                                        }
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["ACC_ACTION"].ToString()))
                                        {
                                            btn.CommandName = drFieldControls[k]["ACC_ACTION"].ToString();
                                            btn.CausesValidation = true;
                                            btn.Click += new EventHandler(ActionHandler);
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
                                            btn.ToolTip = drFieldControls[k]["ACC_TOOLTIP"].ToString().Trim();
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
                                        lnkbtn.ToolTip = drFieldControls[k]["ACC_NAME"].ToString();
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
                                            TabIndex = tabIndex
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
                                                if (entityName.Equals("CRM_CUSTOMER_MST"))//if the entity is the master entity
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
                                                                //clear the HiddenColumnList session
                                                                Session["HiddenColumnList"] = null;
                                                                IEnumerable entityList = null;
                                                                Object customerObj = null;
                                                                if (crmCustomerMstList != null && crmCustomerMstList.Count == 1)
                                                                {
                                                                    customerObj = crmCustomerMstList[0];//set the current selected customer master obj
                                                                }
                                                                retEntityObj = null;
                                                                Object retEntity = null;
                                                                if (entityName.Equals("CRM_CUSTOMER_MST"))
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
                                                                            string value = GetPropertyValue(enitityObj, admFormTabControlDtlObj.ACD_CONTROL_ID);
                                                                            if (countRow == 0)//first row
                                                                            {
                                                                                dcGrid = new DataColumn();
                                                                                dcGrid.ColumnName = admFormTabControlDtlObj.ACD_CONTROL_ID;//set column name
                                                                                dtGrid.Columns.Add(dcGrid);
                                                                            }
                                                                            //set value to the cell
                                                                            drGrid[admFormTabControlDtlObj.ACD_CONTROL_ID] = value;
                                                                            if (admFormTabControlDtlObj.ACD_IS_HIDDEN == 1)//check the cell is set to hidden or not
                                                                            {
                                                                                hiddenColumnList.Add(columnCount);//add to the hiddenfield list
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
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //Bind grid
                                                        //~Test Start
                                                        //PageIndex = PageIndex == null ? "0" : PageIndex;
                                                        //grd.PageIndex = Convert.ToInt32(PageIndex);
                                                        //~Test End
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
                                        div.Controls.Add(chk);
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["ACC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["ACC_CONTROL_TEXT"].ToString()
                                        };
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
                                                        ControlToValidate = fup.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["ACC_NAME"].ToString()
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
                                    if (isFullLengthControl)
                                    {
                                        tcControl.ColumnSpan = Cols;
                                    }
                                    if ((drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "CIM_BRAND_SPECIFICATION"
                                        || drFieldControls[k]["ACC_CONTROL_ID"].ToString() == "CIM_PACKING_SPEC_DESC") && Cols > 1)
                                    {
                                        tcControl.ColumnSpan = Cols;
                                        tcControl.Attributes["style"] = "width:100%;box-sizing:border-box;";
                                        tbControls.Attributes["style"] = "width:100%;table-layout:fixed;";
                                    }
                                    //add inner div to the table cell
                                    tcControl.Controls.Add(div);
                                    //update temptable cell
                                    tempTableCell = tcControl;
                                    //update tempdiv
                                    tempDiv = div;
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
                                    if (isFullLengthControl)
                                    {
                                        j = Cols;
                                    }
                                    //Check any controls have same order and sequence
                                    else if (k < drFieldControls.Count())
                                    {
                                        if (drFieldControls[k]["ACC_CONTROL_GROUP"].ToString() != order.ToString()
                                            || drFieldControls[k]["ACC_SEQUENCE"].ToString() != tempSequence.ToString())//increment column only if any coming control have the same sequence and order of the current control
                                        {
                                            j++;
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
                    }
                    //Add validation summary
                    divValidationSummary.Controls.Clear();
                    foreach (string valGroup in validationGroupList)
                    {
                        ValidationSummary vsObj = new ValidationSummary()
                        {
                            ID = "vsPage" + valGroup,
                            ValidationGroup = valGroup,
                            EnableViewState = false
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
                customerRegistrationServiceClient = null;
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
                if (entityName.Equals("CRM_CUSTOMER_MST"))//if the entity is the master entity
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
                        //List for store the column numbers which should hide
                        List<int> hiddenColumnList = new List<int>();
                        //clear the HiddenColumnList session
                        Session["HiddenColumnList"] = null;
                        IEnumerable entityList = null;
                        Object customerObj = null;
                        if (crmCustomerMstList != null && crmCustomerMstList.Count == 1)
                        {
                            customerObj = crmCustomerMstList[0];//set the current selected customer master obj
                        }
                        retEntityObj = null;
                        Object retEntity = null;
                        if (entityName.Equals("CRM_CUSTOMER_MST"))
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
                                    string value = GetPropertyValue(enitityObj, admFormTabControlDtlObj.ACD_CONTROL_ID);
                                    if (countRow == 0)//first row
                                    {
                                        dcGrid = new DataColumn();
                                        dcGrid.ColumnName = admFormTabControlDtlObj.ACD_CONTROL_ID;//set column name
                                        dtGrid.Columns.Add(dcGrid);
                                    }
                                    //set value to the cell
                                    drGrid[admFormTabControlDtlObj.ACD_CONTROL_ID] = value;
                                    if (admFormTabControlDtlObj.ACD_IS_HIDDEN == 1)//check the cell is set to hidden or not
                                    {
                                        hiddenColumnList.Add(columnCount);//add to the hiddenfield list
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
                            }
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
            customerRegistrationServiceClient = null;
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
                    retEntityObj = propObj.GetValue(entityObj, null) == null ? string.Empty : propObj.GetValue(entityObj, null);
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
            //Get the object type
            Type objType = entityObj.GetType();
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
                if (retEntity != null)
                {
                    int childPK;
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
        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
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
            //base.CheckBtnVisibility(sender);
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
            ApplyRequestedTabFromQuery();
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
            DateRange

        }

        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            DYNAMICTABS,
            CUSTOMER
        }
        
        #endregion
    }
}
