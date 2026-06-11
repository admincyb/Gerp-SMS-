using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPService.Inventory;
using ERPSMS_v01.UserControls;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using System.Data;

namespace ERPSMS_v01.Inventory.Masters
{
    public partial class GeneralProperties : ERP.Store.UI.MyBasePage
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
        /// CheckList Type PK
        /// </summary>
        private int TypeCurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CheckListTypePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CheckListTypePK] = value;
            }
        }


        /// <summary>
        /// CheckList Group PK
        /// </summary>
        private int GroupCurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CheckListGroupPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CheckListGroupPK] = value;
            }
        }
        /// <summary>
        /// ParentPK
        /// </summary>
        private int ParentPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.ParentPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ParentPK] = value;
            }
        }
        /// <summary>
        /// HasParentPK
        /// </summary>
        private int HasParentPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.HasParentPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.HasParentPK] = value;
            }
        }

        private bool IsSbu
        {
            get { return this.ViewState["IsSbu"] == null ? false : Convert.ToBoolean(this.ViewState["IsSbu"].ToString()); }
            set { this.ViewState["IsSbu"] = value; }
          
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
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }

        /// <summary>
        /// To maintain the SortExpression or sort By in viewstate
        /// </summary>
        private string SortBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortBy] = value;
            }
        }

        /// <summary>
        /// To maintain the Sort Direction in viewstate
        /// </summary>
        private string SortDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortDirection] = value;
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

        #endregion

        private ActionsEnum commonActions;
        //page related class objects      
        private ADM_CONST_MST admConstMstObj;
        private ADM_CONST_GRP admConstGrpObj;
        private ADM_CONST_MST admConstMstCheckObj;
        private ADM_CONST_GRP admConstGrpCheckObj;
        private ADM_CONST_GRP_TYPE admConstGrpTypeObj;
        private ServiceUtility serviceUtilityObj;

        private List<ADM_CONST_MST> admConstMstList;
        private List<ADM_CONST_GRP> admConstGrtList;
        private List<ADM_CONST_MST> admConstMstCheckList;
        private List<ADM_CONST_GRP> admConstGrtCheckList;
        private List<ADM_CONST_MST> admConstGrtList1;

        private List<ADM_CONST_GRP_TYPE> admConstGrpTypeList;
        private CommonService CommonServiceClient;

        private BusinessObject.User currentUser;
        private int grpType;
        private DataTable dtResult;
       
        #endregion

        #region PageLevel Events
        /// <summary>
        /// page Load event
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
            try
            {
                
                if (!IsPostBack)
                {
                    setSequence();
                    ConfigSettings();
                    GetFieldValues(ControlsEnum.TYPE);
                    SetFieldValues(ControlsEnum.TYPE);
                    //Check Type is valid or not
                    if (CheckGroupType(TypeCurrPK))
                    {
                        GetFieldValues(ControlsEnum.GROUP);
                        SetFieldValues(ControlsEnum.GROUP);
                        if (TypeCurrPK > 0)
                        {                           
                           
                            string[] dataGroupkeyarray;
                            dataGroupkeyarray = new string[1];
                            dataGroupkeyarray[0] = Resources.DataFieldRes.GroupPK;
                            grdGroupsMst.DataKeyNames = dataGroupkeyarray;
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = Resources.DataFieldRes.ConstPK;
                            grdProductionMst.DataKeyNames = datakeyarray;
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            EntryStatus = EntryStatus.LISTMODE;
                            txtProductCode.Focus();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InvalidType", "InvalidType();", true);
                        DisableControls();
                    }
                }
                //If Selected Group have Enable Data then Show CBM Textbox and Label ( Container Type)
                #region CBM Show
                GetFieldValues(ControlsEnum.CHECKGROUP);
                ADM_CONST_GRP objContainerType = admConstGrtList.Where(f => f.CNG_PK == Convert.ToInt32(ddlGroup.SelectedValue)).SingleOrDefault();
                if (objContainerType != null)
                {
                    if (objContainerType.CNG_SPL_COND == "ENABLE_DATA")
                    {
                        txtCBM.Visible = true;
                        lblCBM.Visible = true;
                    }
                    else
                    {
                        txtCBM.Visible = false;
                        lblCBM.Visible = false;
                    }

                    if (objContainerType.CNG_PK == 1200)
                    {
                        lblSaleEffectDate.Visible = true;
                        ddlSaleEffectDate.Visible = true;
                    }
                    else
                    {
                        lblSaleEffectDate.Visible = false;
                        ddlSaleEffectDate.Visible = false;
                    }
                }
                #endregion

                #region Sequence Properties

                if (ddlGroup.SelectedValue == "79")
                {
                    ltrGroupDetails.Visible = true;
                    divGroupDetails.Visible = true;
                    imbShowGroupDetails.Visible = true;
                    imbHideGroupDetails.Visible = true;
                }

                else
                {
                    ltrGroupDetails.Visible = false;
                    divGroupDetails.Visible = false;
                    imbShowGroupDetails.Visible = false;
                    imbHideGroupDetails.Visible = false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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

            AdmProductPropertiesService AdmProductPropertiesServiceClient = null;

            try
            {
                AdmProductPropertiesServiceClient = new AdmProductPropertiesService();
                AdmProductPropertiesServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmProductPropertiesServiceClient);
                admConstMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_MST>();
                admConstGrpObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_GRP>();
                admConstGrpTypeObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_GRP_TYPE>();
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdProductionMst.PageSize;
                        serviceUtilityObj.FilterBy = ddlFilterBy.SelectedValue == CommonConstants.SELECT_VALUE_ZERO ? null : ddlFilterBy.SelectedValue;
                        serviceUtilityObj.FilterValue = HttpUtility.HtmlEncode(txtSearchBy.Text.Trim());
                        GetGeneralPropertyType();
                        if ((grpType == (int)GroupDetails.TAXFORMS || grpType == (int)GroupDetails.SHIPMENTSETUP || grpType == (int)GroupDetails.OTHERREDSETUP || grpType == (int)GroupDetails.CUSTPROPSETUP) && IsSbu == true)//Tax
                        {
                            admConstMstObj.CON_BIZUNIT = currentUser.SBUID;
                        }

                        //serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.ConstPK : (SortBy.Substring(0, 3) == "CNG" ? Resources.DataFieldRes.ConstPK : SortBy);
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.ConstCode : (SortBy.Substring(0, 3) == "CNG" ? Resources.DataFieldRes.ConstCode : SortBy);
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        admConstMstObj.CON_PK = 0;
                        admConstMstObj.CON_GROUP = Convert.ToInt32(ddlGroup.SelectedValue);
                        if (ParentPK != 0)
                        {
                            admConstMstObj.CON_PARENT = ddlGroupParent.SelectedItem != null ? Convert.ToInt32(ddlGroupParent.SelectedItem.Value) : 0;
                        }
                        else
                        {
                            admConstMstObj.CON_PARENT = 0;
                        }
                        admConstMstList = AdmProductPropertiesServiceClient.GetGeneralProperties(admConstMstObj, serviceUtilityObj);
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    case ControlsEnum.GROUP:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("GrdGroupsMstPageSize").ToString());//grdGroupsMst.PageSize; (Bug Id : 20393)
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.GroupPK : (SortBy.Substring(0, 3) == "CON" ? Resources.DataFieldRes.GroupPK : SortBy);
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        admConstGrpObj.CNG_PK = GroupCurrPK;
                        admConstGrpObj.CNG_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                        admConstGrpObj.CNG_GRP_TYPE = TypeCurrPK;
                        admConstGrtList = AdmProductPropertiesServiceClient.GetGeneralPropertiesGroups(admConstGrpObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.PRODUCTPROPERTY:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        admConstMstObj.CON_PK = CurrPK;
                        admConstMstObj.CON_GROUP = Convert.ToInt32(ddlGroup.SelectedValue);
                        admConstMstList = AdmProductPropertiesServiceClient.GetGeneralProperties(admConstMstObj, serviceUtilityObj);

                        GetFieldValues(ControlsEnum.CHECKHASPARENT);
                        SetFieldValues(ControlsEnum.CHECKHASPARENT);
                        break;
                    case ControlsEnum.PAKINGTYPE:
                        CommonServiceClient = new CommonService();
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        admConstGrtList1 = CommonServiceClient.GetConstMstValues(null, null, null, (int)ConstGroupType.Packing, ERP.Utilities.CommonConstants.PackingMaterialType, null);
                        break;
                    case ControlsEnum.TYPE:
                        //Get CheckListType
                        GetGeneralPropertyType();
                        admConstGrpTypeObj.CGT_PK = grpType;
                        admConstGrpTypeObj.CGT_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                        admConstGrpTypeList = AdmProductPropertiesServiceClient.GetGeneralPropertiesTypes(admConstGrpTypeObj);
                        break;
                    case ControlsEnum.GROUPDROPDOWN:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = 1;
                        serviceUtilityObj.PageSize = -1;
                        //serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("DrpPageSize").ToString());
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.GroupPK;
                        serviceUtilityObj.SortDirection = Resources.ErpRes.SortAscending;
                        admConstGrpObj.CNG_PK = 0;
                        admConstGrpObj.CNG_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                        admConstGrpObj.CNG_GRP_TYPE = TypeCurrPK;
                        admConstGrtList = AdmProductPropertiesServiceClient.GetGeneralPropertiesGroups(admConstGrpObj, serviceUtilityObj);
                        break;
                    #region CHECKHASPARENT
                    case ControlsEnum.CHECKHASPARENT:
                        if (ddlGroup.SelectedValue != null)
                        {
                            ParentPK = 0;
                            serviceUtilityObj = new ServiceUtility();
                            serviceUtilityObj.CurrentPage = 1;
                            serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("DrpPageSize").ToString());
                            serviceUtilityObj.SortBy = Resources.DataFieldRes.GroupPK;
                            serviceUtilityObj.SortDirection = Resources.ErpRes.SortAscending;
                            admConstMstCheckList = new List<ADM_CONST_MST>();
                            admConstGrtCheckList = new List<ADM_CONST_GRP>();
                            AdmProductPropertiesServiceClient = new AdmProductPropertiesService();
                            AdmProductPropertiesServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmProductPropertiesServiceClient);
                            admConstMstCheckObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_MST>();
                            admConstGrpCheckObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_GRP>();
                            admConstGrpCheckObj.CNG_PK = Convert.ToInt32(ddlGroup.SelectedValue);
                            admConstGrpCheckObj.CNG_GRP_TYPE = TypeCurrPK;
                            admConstGrpCheckObj.CNG_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                            admConstGrtCheckList = AdmProductPropertiesServiceClient.GetGeneralPropertiesParentGroup(admConstGrpCheckObj, serviceUtilityObj);
                        }
                        break;
                    #endregion
                    #region PARENT
                    case ControlsEnum.PARENT:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;//PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = -1;//grdProductionMst.PageSize;
                        serviceUtilityObj.FilterBy = ddlFilterBy.SelectedValue == CommonConstants.SELECT_VALUE_ZERO ? null : ddlFilterBy.SelectedValue;
                        serviceUtilityObj.FilterValue = string.Empty;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.ConstPK : (SortBy.Substring(0, 3) == "CNG" ? Resources.DataFieldRes.ConstPK : SortBy);
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        admConstMstCheckObj.CON_PK = 0;
                        admConstMstCheckObj.CON_GROUP = ParentPK;
                        admConstMstCheckObj.CON_PARENT = 0;
                        admConstMstCheckObj.CON_ACTIVE = 1;
                        admConstMstCheckList = AdmProductPropertiesServiceClient.GetGeneralProperties(admConstMstCheckObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region PRODUCTPARENTPROPERTY
                    case ControlsEnum.PRODUCTPARENTPROPERTY:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        admConstMstObj.CON_PK = CurrPK;
                        admConstMstObj.CON_GROUP = Convert.ToInt32(ddlGroup.SelectedValue);
                        admConstMstList = AdmProductPropertiesServiceClient.GetGeneralProperties(admConstMstObj, serviceUtilityObj);
                        break;
                    #endregion

                    case ControlsEnum.CHECKGROUP:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        admConstGrpObj.CNG_PK = GroupCurrPK;
                        admConstGrpObj.CNG_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                        admConstGrpObj.CNG_GRP_TYPE = TypeCurrPK;
                        admConstGrtList = AdmProductPropertiesServiceClient.GetGeneralPropertiesGroups(admConstGrpObj, serviceUtilityObj);
                        break;

                    #region SALE_EFFECT_DATE
                    case ControlsEnum.SALE_EFFECT_DATE:
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SALE EFFECT DATE");
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
                admConstMstObj = null;
                serviceUtilityObj = null;
                admConstGrpObj = null;
                admConstGrpTypeObj = null;
                AdmProductPropertiesServiceClient = null;
                //admConstMstCheckObj = null;
                //admConstGrpCheckObj = null;
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
                    case ControlsEnum.PRODUCTPROPERTY:
                        GetUIValuesFromObject();


                        break;
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        break;
                    case ControlsEnum.GROUP:
                        BindGroupGrid();
                        GetFieldValues(ControlsEnum.GROUPDROPDOWN);
                        BindGroupDropDown();
                        break;
                    case ControlsEnum.GROUPDATA:
                        GetGroupUIValuesFromObject();
                        break;
                    case ControlsEnum.TYPE:
                        SetCheckListType();
                        SetBreadcrumb(GetBreadcrumb());
                        SetManageGroup();
                        SetPageTitle();
                        break;
                    case ControlsEnum.CHECKHASPARENT:
                        if (admConstGrtCheckList.Count > 0)
                        {
                            lblGroupParent.Visible = true;//Listing Page
                            ddlGroupParent.Visible = true;//Listing Page
                            ddlParent.Visible = true;
                            lblParent.Visible = true;


                            ParentPK = Convert.ToInt32(admConstGrtCheckList[0].CNG_PARENT);
                            GroupCurrPK = ParentPK;
                            GetFieldValues(ControlsEnum.GROUP);
                            lblParent.Text = admConstGrtList[0].CNG_NAME;
                            lblGroupParent.Text = admConstGrtList[0].CNG_NAME;//Listing Page
                            GetFieldValues(ControlsEnum.PARENT);
                            BindParentGroupDropDown();
                        }
                        else
                        {
                            lblGroupParent.Visible = false;//Listing Page
                            ddlGroupParent.Visible = false;//Listing Page
                            ddlParent.Visible = false;
                            lblParent.Visible = false;
                        }
                        break;
                    #region SALE_EFFECT_DATE
                    case ControlsEnum.SALE_EFFECT_DATE:
                        BindSaleEffectDateDropDown();
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

        #region Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        private void ConfigSettings()
        {
            if (GetGlobalResourceObject("ConfigurationsRes", "IsSBUSpecificTax").ToString() == "1")
            {
                IsSbu = true;

            }
            else
            {
                IsSbu = false;
            }
        }
        private string GetTypeSequence()
        {
            string result = string.Empty;
            if (ddlSequence1.SelectedValue != "-1")
                result = ddlSequence1.SelectedValue;
            if (ddlSequence2.SelectedValue != "-1")
                result += "-" + ddlSequence2.SelectedValue;
            if (ddlSequence3.SelectedValue != "-1")
                result += "-" + ddlSequence3.SelectedValue;
            if (ddlSequence4.SelectedValue != "-1")
                result += "-" + ddlSequence4.SelectedValue;

            if (ddlSequence5.SelectedValue != "-1")
                result += "-" + ddlSequence5.SelectedValue;
            if (ddlSequence6.SelectedValue != "-1")
                result += "-" + ddlSequence6.SelectedValue;
            if (ddlSequence7.SelectedValue != "-1")
                result += "-" + ddlSequence7.SelectedValue;
            if (ddlSequence8.SelectedValue != "-1")
                result += "-" + ddlSequence8.SelectedValue;
            if (ddlSequence9.SelectedValue != "-1")
                result += "-" + ddlSequence9.SelectedValue;

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        private void SetTypeSequence(string val)
        {
            string[] arr = val.Split('-');
            for (int i = 0; i < arr.Length; i++)
            {

                switch (i + 1)
                {
                    case 1:
                        ddlSequence1.SelectedIndex = ddlSequence1.Items.IndexOf(ddlSequence1.Items.FindByValue(arr[i]));
                        break;
                    case 2:
                        ddlSequence2.SelectedIndex = ddlSequence2.Items.IndexOf(ddlSequence2.Items.FindByValue(arr[i]));
                        break;
                    case 3:
                        ddlSequence3.SelectedIndex = ddlSequence3.Items.IndexOf(ddlSequence3.Items.FindByValue(arr[i]));
                        break;
                    case 4:
                        ddlSequence4.SelectedIndex = ddlSequence4.Items.IndexOf(ddlSequence4.Items.FindByValue(arr[i]));
                        break;
                    case 5:
                        ddlSequence5.SelectedIndex = ddlSequence5.Items.IndexOf(ddlSequence5.Items.FindByValue(arr[i]));
                        break;
                    case 6:
                        ddlSequence6.SelectedIndex = ddlSequence6.Items.IndexOf(ddlSequence6.Items.FindByValue(arr[i]));
                        break;
                    case 7:
                        ddlSequence7.SelectedIndex = ddlSequence7.Items.IndexOf(ddlSequence7.Items.FindByValue(arr[i]));
                        break;
                    case 8:
                        ddlSequence8.SelectedIndex = ddlSequence8.Items.IndexOf(ddlSequence8.Items.FindByValue(arr[i]));
                        break;
                    case 9:
                        ddlSequence9.SelectedIndex = ddlSequence9.Items.IndexOf(ddlSequence9.Items.FindByValue(arr[i]));
                        break;

                }
            }
            // 
        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private ADM_CONST_MST SetUIValuesToObject()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                admConstMstObj.CON_PK = CurrPK;
                admConstMstObj.CON_CODE = HttpUtility.HtmlEncode(txtProductCode.Text.Trim());
                admConstMstObj.CON_NAME = HttpUtility.HtmlEncode(txtProductName.Text.Trim());
                admConstMstObj.CON_GROUP = Convert.ToInt32(ddlGroup.SelectedValue);
                admConstMstObj.CON_DESC = HttpUtility.HtmlEncode(txtProductDesc.Text.Trim());
                admConstMstObj.CON_DEFAULT = CommonConstants.CON_DEFAULT;
                admConstMstObj.CON_DATA = txtCBM.Text.Trim();//CBM
                admConstMstObj.CON_SEQUENCE = Convert.ToInt32(txtSequence.Text);
                admConstMstObj.CON_ACTIVE = Convert.ToByte(chkProductActive.Checked);
                admConstMstObj.CON_BIZUNIT = currentUser.SBUID;
                admConstMstObj.CON_MOD_BY = currentUser.PKUser;
                admConstMstObj.CON_MOD_DT = LastModifiedTime;
                if (ddlGroup.SelectedValue == "79")
                {
                    admConstMstObj.CON_SPL_COND = GetTypeSequence();
                }
                else if (ddlGroup.SelectedValue == "1200")
                {
                    admConstMstObj.CON_SPL_COND = Convert.ToInt32(ddlSaleEffectDate.SelectedValue) > 0 ? ddlSaleEffectDate.SelectedValue : null;
                }
                if (ParentPK != 0)
                {
                    admConstMstObj.CON_PARENT = Convert.ToInt32(ddlParent.SelectedItem.Value);
                }
                return admConstMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admConstMstObj = null;
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private ADM_CONST_GRP SetGroupUIValuesToObject()
        {
            int sequence = 0;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                admConstGrpObj.CNG_PK = GroupCurrPK;
                admConstGrpObj.CNG_CODE = HttpUtility.HtmlEncode(txtGroupCode.Text.Trim());
                admConstGrpObj.CNG_NAME = HttpUtility.HtmlEncode(txtGroupName.Text.Trim());
                admConstGrpObj.CNG_GRP_TYPE = TypeCurrPK;
                admConstGrpObj.CNG_SEQUENCE = Int32.TryParse(HttpUtility.HtmlEncode(txtGroupSequence.Text.Trim()), out sequence) == true ? sequence : CommonConstants.SequenceNumber;
                admConstGrpObj.CNG_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                admConstGrpObj.CNG_DEFAULT = CommonConstants.CON_DEFAULT;
                admConstGrpObj.CNG_BIZUNIT = currentUser.SBUID;
                admConstGrpObj.CNG_MOD_BY = currentUser.PKUser;
                return admConstGrpObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admConstGrpObj = null;
            }
        }

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            try
            {
                //assigning the UI controls with the corresponding ListObject value AsrSovMstList
                if (admConstMstList != null && admConstMstList.Count() > 0)
                {
                    CurrPK = admConstMstList[0].CON_PK;
                    txtProductCode.Text = HttpUtility.HtmlDecode(admConstMstList[0].CON_CODE);
                    txtProductName.Text = HttpUtility.HtmlDecode(admConstMstList[0].CON_NAME);
                    txtCBM.Text = admConstMstList[0].CON_DATA;//CBM
                    txtSequence.Text = Convert.ToString(admConstMstList[0].CON_SEQUENCE);
                    txtProductDesc.Text = HttpUtility.HtmlDecode(admConstMstList[0].CON_DESC);
                    chkProductActive.Checked = admConstMstList[0].CON_ACTIVE == 1 ? true : false;
                    ModifiedDatePnl.Visible = true;
                    LastModifiedTime = admConstMstList[0].CON_MOD_DT;
                    if (admConstMstList[0].CON_GROUP == 79)
                    {
                        GetFieldValues(ControlsEnum.PAKINGTYPE);
                        BindPackTypeDropDown();
                        if (!string.IsNullOrWhiteSpace(admConstMstList[0].CON_SPL_COND))
                            SetTypeSequence(admConstMstList[0].CON_SPL_COND);
                    }
                    else if (admConstMstList[0].CON_GROUP == 1200)
                    {
                        GetFieldValues(ControlsEnum.SALE_EFFECT_DATE);
                        SetFieldValues(ControlsEnum.SALE_EFFECT_DATE);
                        if (!string.IsNullOrEmpty(admConstMstList[0].CON_SPL_COND.ToString()))
                            ddlSaleEffectDate.SelectedIndex = ddlSaleEffectDate.Items.IndexOf(ddlSaleEffectDate.Items.FindByValue(admConstMstList[0].CON_SPL_COND.ToString()));
                    }

                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                }
                //Concurrency Account details Deleted By Another User
                else
                {
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.ProductProperties);
                    ResetForm();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    throw new Exception(litErrorMsg.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets the Group UI input controls from the object values
        /// </summary>
        private void GetGroupUIValuesFromObject()
        {
            try
            {
                //assigning the UI controls with the corresponding ListObject value AsrSovMstList
                if (admConstGrtList != null && admConstGrtList.Count() > 0)
                {
                    GroupCurrPK = admConstGrtList[0].CNG_PK;
                    txtGroupCode.Text = HttpUtility.HtmlDecode(admConstGrtList[0].CNG_CODE);
                    txtGroupName.Text = HttpUtility.HtmlDecode(admConstGrtList[0].CNG_NAME);
                    txtGroupSequence.Text = HttpUtility.HtmlDecode(admConstGrtList[0].CNG_SEQUENCE.ToString());

                }
                //Concurrency Account details Deleted By Another User
                else
                {
                    litGroupErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litGroupErrorMsg.Text = string.Format(litGroupErrorMsg.Text.ToLower(), Resources.PageNameRes.CheckListGroup);
                    ResetGroupForm();
                    GetFieldValues(ControlsEnum.GROUP);
                    SetFieldValues(ControlsEnum.GROUP);
                    throw new Exception(litGroupErrorMsg.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid()
        {
            try
            {
                ModifiedDatePnl.Visible = false;
                if (admConstMstList != null)
                {
                    uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdProductionMst.DataSource = admConstMstList;
                    grdProductionMst.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                }
                else
                {
                    uclPaging.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void BindPackTypeDropDown()
        {
            ddlSequence1.Items.Clear();
            ddlSequence2.Items.Clear();
            ddlSequence3.Items.Clear();
            ddlSequence4.Items.Clear();
            ddlSequence5.Items.Clear();
            ddlSequence6.Items.Clear();
            ddlSequence7.Items.Clear();
            ddlSequence8.Items.Clear();
            ddlSequence9.Items.Clear();
            ddlSequence1.Visible = false;
            ddlSequence2.Visible = false;
            ddlSequence3.Visible = false;
            ddlSequence4.Visible = false;
            ddlSequence5.Visible = false;
            ddlSequence6.Visible = false;
            ddlSequence7.Visible = false;
            ddlSequence8.Visible = false;
            ddlSequence9.Visible = false;
            lblSeq1.Visible = false;
            lblSeq2.Visible = false;
            lblSeq3.Visible = false;
            lblSeq4.Visible = false;
            lblSeq5.Visible = false;
            lblSeq6.Visible = false;
            lblSeq7.Visible = false;
            lblSeq8.Visible = false;
            lblSeq9.Visible = false;
            if (admConstGrtList1 != null && admConstGrtList1.Count > 0)
            {
                for (int i = 1; i <= Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "PackingMaterialSequenceddl").ToString()); i++)
                {
                    switch (i)
                    {
                        case 1:
                            ddlSequence1.Visible = true;
                            lblSeq1.Visible = true;
                            ddlSequence1.DataSource = admConstGrtList1;
                            ddlSequence1.DataTextField = Resources.DataFieldRes.ConstName;
                            ddlSequence1.DataValueField = Resources.DataFieldRes.ConstCode;
                            ddlSequence1.DataBind();
                            break;
                        case 2:
                            ddlSequence2.Visible = true;
                            lblSeq2.Visible = true;
                            ddlSequence2.DataSource = admConstGrtList1;
                            ddlSequence2.DataTextField = Resources.DataFieldRes.ConstName;
                            ddlSequence2.DataValueField = Resources.DataFieldRes.ConstCode;
                            ddlSequence2.DataBind();

                            break;
                        case 3:
                            lblSeq3.Visible = true;
                            ddlSequence3.Visible = true;
                            ddlSequence3.DataSource = admConstGrtList1;
                            ddlSequence3.DataTextField = Resources.DataFieldRes.ConstName;
                            ddlSequence3.DataValueField = Resources.DataFieldRes.ConstCode;
                            ddlSequence3.DataBind();

                            break;
                        case 4:
                            ddlSequence4.Visible = true;
                            lblSeq4.Visible = true;
                            ddlSequence4.DataSource = admConstGrtList1;
                            ddlSequence4.DataTextField = Resources.DataFieldRes.ConstName;
                            ddlSequence4.DataValueField = Resources.DataFieldRes.ConstCode;
                            ddlSequence4.DataBind();

                            break;
                        case 5:
                            ddlSequence5.Visible = true;
                            lblSeq5.Visible = true;
                            ddlSequence5.DataSource = admConstGrtList1;
                            ddlSequence5.DataTextField = Resources.DataFieldRes.ConstName;
                            ddlSequence5.DataValueField = Resources.DataFieldRes.ConstCode;
                            ddlSequence5.DataBind();

                            break;
                        case 6:
                            ddlSequence6.Visible = true;
                            lblSeq6.Visible = true;
                            ddlSequence6.DataSource = admConstGrtList1;
                            ddlSequence6.DataTextField = Resources.DataFieldRes.ConstName;
                            ddlSequence6.DataValueField = Resources.DataFieldRes.ConstCode;
                            ddlSequence6.DataBind();

                            break;
                        case 7:
                            ddlSequence7.Visible = true;
                            lblSeq7.Visible = true;
                            ddlSequence7.DataSource = admConstGrtList1;
                            ddlSequence7.DataTextField = Resources.DataFieldRes.ConstName;
                            ddlSequence7.DataValueField = Resources.DataFieldRes.ConstCode;
                            ddlSequence7.DataBind();

                            break;
                        case 8:
                            ddlSequence8.Visible = true;
                            lblSeq8.Visible = true;
                            ddlSequence8.DataSource = admConstGrtList1;
                            ddlSequence8.DataTextField = Resources.DataFieldRes.ConstName;
                            ddlSequence8.DataValueField = Resources.DataFieldRes.ConstCode;
                            ddlSequence8.DataBind();

                            break;
                        case 9:
                            ddlSequence9.Visible = true;
                            lblSeq9.Visible = true;
                            ddlSequence9.DataSource = admConstGrtList1;
                            ddlSequence9.DataTextField = Resources.DataFieldRes.ConstName;
                            ddlSequence9.DataValueField = Resources.DataFieldRes.ConstCode;
                            ddlSequence9.DataBind();

                            break;
                        default:
                            break;

                    }
                }
                ddlSequence1.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));
                ddlSequence2.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));
                ddlSequence3.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));
                ddlSequence4.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));
                ddlSequence5.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));
                ddlSequence6.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));
                ddlSequence7.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));
                ddlSequence8.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));
                ddlSequence9.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));

            }
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGroupGrid()
        {
            try
            {
                if (admConstGrtList != null)
                {
                    grdGroupsMst.DataSource = admConstGrtList;
                    grdGroupsMst.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Group DropDown
        /// </summary>
        public void BindGroupDropDown()
        {
            ddlGroup.Items.Clear();
            if (admConstGrtList != null && admConstGrtList.Count > 0)
            {
                List<ADM_CONST_GRP> admConstGrtListOrder = null;
                if (hdfAddGroup.Value == "1")
                {
                    admConstGrtListOrder = admConstGrtList.OrderBy(c => c.CNG_SEQUENCE).ToList();
                }
                else
                {
                    admConstGrtListOrder = admConstGrtList.OrderBy(c => c.CNG_NAME).ToList();
                }
                ddlGroup.DataSource = admConstGrtListOrder;
                ddlGroup.DataTextField = Resources.DataFieldRes.GroupName;
                ddlGroup.DataValueField = Resources.DataFieldRes.GroupPK;
                ddlGroup.DataBind();
            }
            else
            {
                ddlGroup.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            }
        }
        /// <summary>
        /// Method for Parent Group DropDown
        /// </summary>
        public void BindParentGroupDropDown()
        {
            ddlParent.Items.Clear();
            ddlGroupParent.Items.Clear();
            if (admConstMstCheckList != null && admConstMstCheckList.Count > 0)
            {
                List<ADM_CONST_MST> admConstMSTListOrder = null;
                admConstMSTListOrder = admConstMstCheckList.OrderBy(c => c.CON_NAME).ToList();
                ddlGroupParent.DataSource = CommonFunctions.HtmlDecode(admConstMSTListOrder, Resources.DataFieldRes.ConstName);//Listing Page
                ddlGroupParent.DataTextField = Resources.DataFieldRes.ConstName;//Listing Page
                ddlGroupParent.DataValueField = Resources.DataFieldRes.ConstPK;//Listing Page
                ddlGroupParent.DataBind();//Listing Page
                ddlParent.DataSource = CommonFunctions.HtmlDecode(admConstMSTListOrder, Resources.DataFieldRes.ConstName);
                ddlParent.DataTextField = Resources.DataFieldRes.ConstName;
                ddlParent.DataValueField = Resources.DataFieldRes.ConstPK;
                ddlParent.DataBind();
                if (CurrPK != 0)
                {
                    GetFieldValues(ControlsEnum.PRODUCTPARENTPROPERTY);
                    if (admConstMstList.Count > 0)
                        ddlParent.SelectedIndex = ddlParent.Items.IndexOf(ddlParent.Items.FindByValue(admConstMstList[0].CON_PARENT.ToString()));
                }
                else
                {
                    if (HasParentPK != 0)
                    {
                        ddlParent.SelectedIndex = ddlParent.Items.IndexOf(ddlParent.Items.FindByValue(HasParentPK.ToString()));
                        ddlGroupParent.SelectedIndex = ddlGroupParent.Items.IndexOf(ddlGroupParent.Items.FindByValue(HasParentPK.ToString()));
                    }
                }
            }
            else
            {
                ddlParent.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            }
        }

        public void BindSaleEffectDateDropDown()
        {
            ddlSaleEffectDate.Items.Clear();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                ddlSaleEffectDate.DataSource = dtResult;
                ddlSaleEffectDate.DataTextField = Resources.DataFieldRes.cfgData;
                ddlSaleEffectDate.DataValueField = Resources.DataFieldRes.cfgValue;
                ddlSaleEffectDate.DataBind();
            }
            ddlSaleEffectDate.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            ParentPK = 0;
            txtSearchBy.Text = string.Empty;
            txtProductCode.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtProductDesc.Text = string.Empty;
            txtSequence.Text = string.Empty;
            PageIndex = CommonConstants.SELECT_VALUE_ONE;
            chkProductActive.Checked = true;
        }

        /// <summary>
        /// Method used to Reset Group form Controls
        /// </summary>
        private void ResetGroupForm()
        {
            GroupCurrPK = 0;
            txtGroupCode.Text = string.Empty;
            txtGroupName.Text = string.Empty;
            txtGroupSequence.Text = CommonConstants.SequenceNumber.ToString();
            PageIndex = CommonConstants.SELECT_VALUE_ONE;
        }

        /// <summary>
        /// Method used to set Sequence
        /// </summary>
        private void setSequence()
        {
            txtGroupSequence.Text = CommonConstants.SequenceNumber.ToString();
        }

        /// <summary>
        /// Method used to get general property Type
        /// </summary>
        private void GetGeneralPropertyType()
        {
            //get group type
            if (Request.QueryString[QueryStrings.Type] != null)
            {
                grpType = Int32.TryParse(HttpUtility.HtmlDecode(Request.QueryString[QueryStrings.Type]), out grpType) == true ? grpType : 0;
            }
            else
            {
                grpType = (int)ConstGroupType.Product;
            }
        }

        /// <summary>
        /// Method used to set Group Type
        /// </summary>
        private void SetCheckListType()
        {
            if (admConstGrpTypeList != null && admConstGrpTypeList.Count > 0)
            {
                TypeCurrPK = admConstGrpTypeList[0].CGT_PK;
            }
            else
            {
                TypeCurrPK = 0;
            }
        }


        /// <summary>
        /// Method used to Check Type is valid or not
        /// </summary>
        private bool CheckGroupType(int TypeCurrPK)
        {
            if (admConstGrpTypeList != null && admConstGrpTypeList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method used to Check Type is valid or not
        /// </summary>
        private void SetManageGroup()
        {
            if (admConstGrpTypeList != null && admConstGrpTypeList.Count > 0)
            {
                btnAddGroup.Visible = admConstGrpTypeList[0].CGT_SPL_COND != null ? (admConstGrpTypeList[0].CGT_SPL_COND.ToLower().Contains("edit") == true ? true : false) : false;
            }
            else
            {
                btnAddGroup.Visible = false;
            }
            hdfAddGroup.Value = btnAddGroup.Visible ? "1" : "0";
        }

        /// <summary>
        /// Method used to set Page Title
        /// </summary>
        private void SetPageTitle()
        {
            if (admConstGrpTypeList != null && admConstGrpTypeList.Count > 0)
            {
                this.Title = Resources.Captions.Title_gERPSetup + " " + admConstGrpTypeList[0].CGT_NAME;
            }
            else
            {
                this.Title = Resources.Captions.Title_GeneralProperties;
            }
        }

        /// <summary>
        /// Method used to Disable Controls
        /// </summary>
        private void DisableControls()
        {
            btnSave.Visible = false;
            PageAction_Entry.Visible = false;
            PageAction_List.Visible = false;
            Group.Visible = false;
        }

        /// <summary>
        /// Method used to get Breadcrumb
        /// </summary>
        private string GetBreadcrumb()
        {
            string breadCrumb = string.Empty;
            if (admConstGrpTypeList != null && admConstGrpTypeList.Count > 0)
            {
                breadCrumb = admConstGrpTypeList[0].CGT_NAME;
            }
            return breadCrumb;
        }


        /// <summary>
        /// Method used to set Breadcrumb
        /// </summary>
        private void SetBreadcrumb(string breadCrumb)
        {
            if (string.IsNullOrEmpty(breadCrumb))
            {
                breadCrumb = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "");
            }
            else
            {
                breadCrumb = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>") + breadCrumb.ToString();
            }
            lblBreadCrum.Text = breadCrumb;
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
            AdmProductPropertiesService AdmProductPropertiesServiceClient;
            AdmProductPropertiesServiceClient = null;
            try
            {
                GetFieldValues(ControlsEnum.TYPE);

                int result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlGroup")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                    if (((DropDownList)sender).ID == "ddlGroupParent")
                    {
                        commonActions = ActionsEnum.PARENTSELECTEDINDEXCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                switch (commonActions)
                {

                    #region ADDGROUP
                    case ActionsEnum.ADDGROUP:
                        ResetGroupForm();
                        GetFieldValues(ControlsEnum.GROUP);
                        SetFieldValues(ControlsEnum.GROUP);
                        txtGroupCode.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowGroup1", "ShowAddGroup();", true);
                        break;
                    #endregion

                    #region GROUPSAVE
                    case ActionsEnum.GROUPSAVE:
                        if (!IsValid)
                        {
                            litGroupErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litGroupErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            admConstGrtList = new List<ADM_CONST_GRP>();
                            AdmProductPropertiesServiceClient = new AdmProductPropertiesService();
                            AdmProductPropertiesServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmProductPropertiesServiceClient);
                            admConstGrpObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_GRP>();
                            admConstGrpObj = SetGroupUIValuesToObject();
                            admConstGrtList.Add(admConstGrpObj);
                            result = AdmProductPropertiesServiceClient.SaveGeneralPropertiesGroups(admConstGrtList);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                //SortBy = Resources.DataFieldRes.GroupPK;
                                //SortDirection = Resources.Report.SortDescending;
                                ResetGroupForm();
                                GetFieldValues(ControlsEnum.GROUP);
                                SetFieldValues(ControlsEnum.GROUP);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowGroup", "ShowGroup();", true);
                                litGroupErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litGroupErrorMsg.Text = string.Format(litGroupErrorMsg.Text.ToLower(), Resources.PageNameRes.Group);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litGroupErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            //Duplicate records
                            else if (result == -1)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("DuplicateError").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                txtGroupCode.Focus();
                            }
                        }
                        break;
                    #endregion

                    #region EDITGROUP
                    case ActionsEnum.GROUPGRIDEDIT:
                        GroupCurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        GetFieldValues(ControlsEnum.GROUP);
                        SetFieldValues(ControlsEnum.GROUPDATA);
                        //if (ddlGroup.SelectedValue == "83") { txtProductName.Focus(); }// for packing material type 
                        //else { txtGroupCode.Focus(); }

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowGroup", "ShowGroup();", true);
                        break;
                    #endregion

                    #region DELETEGROUP
                    case ActionsEnum.GROUPGRIDDELETE:
                        GroupCurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        AdmProductPropertiesServiceClient = new AdmProductPropertiesService();
                        AdmProductPropertiesServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmProductPropertiesServiceClient);
                        admConstGrtList = new List<ADM_CONST_GRP>();
                        admConstGrpObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_GRP>();
                        admConstGrpObj.CNG_PK = GroupCurrPK;
                        admConstGrtList.Add(admConstGrpObj);
                        result = AdmProductPropertiesServiceClient.DeleteGeneralPropertiesGroups(admConstGrtList);
                        if (result > 0)
                        {
                            ResetGroupForm();
                            GetFieldValues(ControlsEnum.GROUP);
                            SetFieldValues(ControlsEnum.GROUP);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowGroup", "ShowGroup();", true);
                            litGroupErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litGroupErrorMsg.Text = string.Format(litGroupErrorMsg.Text.ToLower(), Resources.PageNameRes.Group);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litGroupErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            if (Convert.ToInt32(ddlGroup.SelectedValue) > 0)
                            {
                                admConstMstList = new List<ADM_CONST_MST>();
                                AdmProductPropertiesServiceClient = new AdmProductPropertiesService();
                                AdmProductPropertiesServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmProductPropertiesServiceClient);
                                admConstMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_MST>();
                                admConstMstObj = SetUIValuesToObject();
                                admConstMstList.Add(admConstMstObj);
                                result = AdmProductPropertiesServiceClient.SaveGeneralProperties(admConstMstList);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    SortBy = Resources.DataFieldRes.ConstPK;
                                    SortDirection = Resources.Report.SortDescending;
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetBreadcrumb());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    ResetForm();
                                    if (ddlParent.Visible == true)
                                    {
                                        HasParentPK = Convert.ToInt32(ddlParent.SelectedItem.Value);
                                    }
                                    GetFieldValues(ControlsEnum.CHECKHASPARENT);
                                    SetFieldValues(ControlsEnum.CHECKHASPARENT);
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    btnSave.Focus();
                                }
                                //Duplicate records
                                else if (result == -1)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("DuplicateError").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    txtProductCode.Focus();
                                }
                            }
                        }
                        break;
                    #endregion

                    #region GRIDEDIT
                    case ActionsEnum.GRIDEDIT:
                        CurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        GetFieldValues(ControlsEnum.PRODUCTPROPERTY);
                        SetFieldValues(ControlsEnum.PRODUCTPROPERTY);
                        if (ddlGroup.SelectedValue == "83") { txtProductName.Focus(); }// for packing material type 
                        else { txtProductCode.Focus(); }

                        //  txtProductCode.Focus();
                        EntryStatus = EntryStatus.ENTRYMODE;
                        break;
                    #endregion

                    #region GRIDDELETE
                    case ActionsEnum.GRIDDELETE:
                        CurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        AdmProductPropertiesServiceClient = new AdmProductPropertiesService();
                        AdmProductPropertiesServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmProductPropertiesServiceClient);
                        admConstMstList = new List<ADM_CONST_MST>();
                        admConstMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_MST>();
                        admConstMstObj.CON_PK = CurrPK;
                        admConstMstList.Add(admConstMstObj);
                        result = AdmProductPropertiesServiceClient.DeleteGeneralProperties(admConstMstList);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            ResetForm();
                            btnSave.Focus();
                            if (ddlGroupParent.Visible == true)
                            {
                                HasParentPK = Convert.ToInt32(ddlGroupParent.SelectedItem.Value);
                            }
                            GetFieldValues(ControlsEnum.CHECKHASPARENT);
                            SetFieldValues(ControlsEnum.CHECKHASPARENT);
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetBreadcrumb());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        //delete reference error
                        else if (result == -1)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("DeleteError").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        btnSearch.Focus();
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.CHECKHASPARENT);
                        SetFieldValues(ControlsEnum.CHECKHASPARENT);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        if (ddlParent.Visible == true)
                        {
                            HasParentPK = Convert.ToInt32(ddlParent.SelectedItem.Value);
                        }
                        GetFieldValues(ControlsEnum.CHECKHASPARENT);
                        SetFieldValues(ControlsEnum.CHECKHASPARENT);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        this.btnSave.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseGroup", "CloseGroup();", true);
                        break;
                    #endregion

                    #region SelectIndexChanged
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        //Drop down change event
                        ResetForm();
                        GetFieldValues(ControlsEnum.CHECKHASPARENT);
                        SetFieldValues(ControlsEnum.CHECKHASPARENT);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnSave.Focus();
                        if (ddlGroup.SelectedValue == "83") { btnNew.Visible = false; txtProductCode.Enabled = false; }// for packing material type 
                        else { btnNew.Visible = true; txtProductCode.Enabled = true; }
                        break;
                    #endregion
                    #region ParentSelectIndexChangedParent
                    case ActionsEnum.PARENTSELECTEDINDEXCHANGED:
                        //Drop down change event
                        //ResetForm();
                        //GetFieldValues(ControlsEnum.CHECKHASPARENT);
                        //SetFieldValues(ControlsEnum.CHECKHASPARENT);
                        ParentPK = Convert.ToInt32(ddlGroupParent.SelectedItem.Value);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnSave.Focus();
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        if (Convert.ToInt32(ddlGroup.SelectedValue) > Convert.ToInt32(CommonConstants.SELECTVAL))
                        {
                            if (ddlGroupParent.Visible == true)
                            {
                                if (ddlGroupParent.SelectedIndex > -1)
                                {
                                    EntryStatus = EntryStatus.NEWMODE;
                                    ResetForm();
                                    ModifiedDatePnl.Visible = false;
                                    txtProductCode.Focus();
                                    if (ddlGroupParent.Visible == true)
                                    {
                                        HasParentPK = Convert.ToInt32(ddlGroupParent.SelectedItem.Value);
                                    }
                                    GetFieldValues(ControlsEnum.CHECKHASPARENT);
                                    SetFieldValues(ControlsEnum.CHECKHASPARENT);
                                }
                                else
                                {
                                    GetFieldValues(ControlsEnum.CHECKHASPARENT);
                                    if (admConstGrtCheckList.Count > 0)
                                    {
                                        ParentPK = Convert.ToInt32(admConstGrtCheckList[0].CNG_PARENT);
                                        GroupCurrPK = ParentPK;
                                        GetFieldValues(ControlsEnum.GROUP);
                                        litErrorMsg.Text = GetLocalResourceObject("ParentSelectError").ToString();
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, admConstGrtList[0].CNG_NAME.Trim());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                EntryStatus = EntryStatus.NEWMODE;
                                ResetForm();
                                ModifiedDatePnl.Visible = false;
                                txtProductCode.Focus();

                            }
                            if (ddlGroup.SelectedValue == "79")
                            {
                                GetFieldValues(ControlsEnum.PAKINGTYPE);
                                BindPackTypeDropDown();
                            }

                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("ProductPropertiesDuplicate").ToString()))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.ProductProperties)) + "','" + Resources.Messages.Information + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
                txtProductCode.Focus();
            }
            finally
            {
                admConstMstObj = null;
                admConstMstList = null;
                admConstGrpObj = null;
                admConstGrtList = null;
                AdmProductPropertiesServiceClient = null;
                //admConstMstCheckObj = null;
                //admConstGrpCheckObj = null;
                //SetPageTitle();
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            string GridID = ((GridView)sender).ID;
            try
            {
                GetFieldValues(ControlsEnum.TYPE);

                if (SortBy == e.SortExpression)
                {
                    if (SortDirection == Resources.Report.SortAscending)
                        SortDirection = Resources.Report.SortDescending;
                    else
                        SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.Report.SortAscending;
                }
                this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
                if (GridID == "grdGroupsMst")
                {
                    GetFieldValues(ControlsEnum.GROUP);
                    SetFieldValues(ControlsEnum.GROUP);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowGroup", "ShowGroup();", true);
                }
                else if (GridID == "grdProductionMst")
                {
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                }
                //SetPageTitle();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Binding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdProductionMst")
                {
                    GetFieldValues(ControlsEnum.CHECKGROUP);
                    ADM_CONST_GRP objContainerType = admConstGrtList.Where(f => f.CNG_PK == Convert.ToInt32(ddlGroup.SelectedValue)).SingleOrDefault();
                    if (objContainerType != null)
                    {
                        if (objContainerType.CNG_SPL_COND == "ENABLE_DATA")
                        {

                            grdProductionMst.Columns[2].Visible = true;
                            grdProductionMst.Columns[2].Visible = true;
                        }
                        else
                        {
                            grdProductionMst.Columns[2].Visible = false;
                            grdProductionMst.Columns[2].Visible = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            uclPaging.CurrentPage = 1;
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //////base.CheckBtnVisibility(sender);
        }

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        uclPaging.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        // Decrement the first page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Increment the last page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the next page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the previous page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }
                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link?
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link?
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link?
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "NewGroup", "NewGroup();", true);
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
            }

        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            PRODUCTPROPERTY,
            GROUP,
            GROUPDATA,
            TYPE,
            GROUPDROPDOWN,
            CHECKHASPARENT,
            PARENT,
            PRODUCTPARENTPROPERTY,
            CHECKGROUP,
            PAKINGTYPE,
            SALE_EFFECT_DATE
        }
        #endregion
        public enum GroupDetails
        {
            TAXFORMS = 19,
            CUSTPROPSETUP = 9,
            OTHERREDSETUP = 17,
            SHIPMENTSETUP = 13,
           
        }
    }
}