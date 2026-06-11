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
using BusinessObject.Inventory;
using BusinessObject.Common;

namespace ERPSMS_v01.Inventory.Masters
{
    public partial class CheckListMaster : System.Web.UI.Page
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
        /// CheckList Item PK
        /// </summary>
        private int ItemCurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CheckListItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CheckListItemPK] = value;
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
        private ADM_CHECK_LIST_GROUP_MST admCheckListGroupMstObj;
        private ADM_CHECK_LIST_ITEM_MST admCheckListItemMstObj;
        private ADM_CHECK_LIST_TYPE_CFG admCheckListTypeCFGObj;
        private ADM_CONTROLS_CFG admControlListObj;
        private ADM_CONST_GRP admConstGrpListObj;
        private ServiceUtility serviceUtilityObj;

        private List<ADM_CHECK_LIST_GROUP_MST> admCheckListGroupMstList;
        private List<ADM_CHECK_LIST_ITEM_MST> admCheckListItemMstList;
        private List<CheckListItems> admCheckListItemMstListForGrid;
        private List<ADM_CHECK_LIST_TYPE_CFG> admCheckListTypeList;
        private List<ADM_CONTROLS_CFG> admControlList;
        private List<ADM_CONST_GRP> admConstGrpList;

        private BusinessObject.User currentUser;
        private int grpType;
        private string grpTypeCode;
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

                    GetFieldValues(ControlsEnum.TYPE);
                    SetFieldValues(ControlsEnum.TYPE);

                    if (CheckCheckListType())
                    {
                        
                        GetFieldValues(ControlsEnum.GROUP);
                        SetFieldValues(ControlsEnum.GROUP);

                        if (TypeCurrPK > 0)
                        {
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = Resources.DataFieldRes.CGMPK;
                            grdGroupsMst.DataKeyNames = datakeyarray;

                            string[] dataItemkeyarray;
                            dataItemkeyarray = new string[1];
                            dataItemkeyarray[0] = Resources.DataFieldRes.CHIPK;
                            grdItemMst.DataKeyNames = dataItemkeyarray;


                            GetFieldValues(ControlsEnum.CONTROL);
                            SetFieldValues(ControlsEnum.CONTROL);
                            GetFieldValues(ControlsEnum.KEY);
                            SetFieldValues(ControlsEnum.KEY);
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            EntryStatus = EntryStatus.LISTMODE;

                            txtItemCode.Focus();
                        }
                    }
                    else
                    {
                        DisableControls();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InvalidType", "InvalidType();", true);
                    }
                }
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
            AdmCheckListMstService AdmCheckListServiceClient = null;
            CommonService CommonServiceClient = null;
            try
            {
                AdmCheckListServiceClient = new AdmCheckListMstService();
                AdmCheckListServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCheckListServiceClient);
                CommonServiceClient = new CommonService();
                CommonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(CommonServiceClient);
                admCheckListGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_GROUP_MST>();
                admCheckListItemMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_ITEM_MST>();
                admCheckListTypeCFGObj=  ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TYPE_CFG>();
                admControlListObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONTROLS_CFG>();
                admConstGrpListObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_GRP>();
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        if (Convert.ToInt32(ddlGroup.SelectedValue) > Convert.ToInt32(CommonConstants.SELECTVAL))
                        {
                            serviceUtilityObj = new ServiceUtility();
                            serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                            serviceUtilityObj.PageSize = grdItemMst.PageSize;
                            serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.CHIPK : SortBy;
                            serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                            admCheckListItemMstObj.CHI_PK = 0;
                            admCheckListItemMstObj.CHI_GROUP= Convert.ToInt32(ddlGroup.SelectedValue);
                            admCheckListItemMstObj.CHI_ACTIVE = (byte)DbActiveStatus.ALL; //(byte)DbActiveStatus.ACTIVE;
                            admCheckListItemMstListForGrid = AdmCheckListServiceClient.GetCheckListItems(admCheckListItemMstObj, serviceUtilityObj);
                            TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                        (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                        (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        }
                        break;
                    case ControlsEnum.ITEMDATA:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        admCheckListItemMstObj.CHI_PK = ItemCurrPK;
                        admCheckListItemMstObj.CHI_GROUP = Convert.ToInt32(ddlGroup.SelectedValue);
                        admCheckListItemMstObj.CHI_ACTIVE = (byte)DbActiveStatus.HASPK; //(byte)DbActiveStatus.ACTIVE;
                        admCheckListItemMstListForGrid = AdmCheckListServiceClient.GetCheckListItems(admCheckListItemMstObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.GROUP:
                        serviceUtilityObj = new ServiceUtility();
                        //serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.CGMPK : SortBy;
                        //serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        admCheckListGroupMstObj.CGM_PK = GroupCurrPK;
                        admCheckListGroupMstObj.CGM_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                        admCheckListGroupMstObj.CGM_TYPE = TypeCurrPK;
                        admCheckListGroupMstList = AdmCheckListServiceClient.GetCheckListGroups(admCheckListGroupMstObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.CONTROL:
                        admControlListObj.CTL_SPL_COND = CommonConstants.CTL_SPL_COND;
                        admControlList = CommonServiceClient.GetControlsList(admControlListObj);
                        break;
                    case ControlsEnum.KEY:
                        admConstGrpListObj.CNG_GRP_TYPE = (int)ConstGroupType.CheckList;
                        admConstGrpList = CommonServiceClient.GetConstGrpList(admConstGrpListObj);
                        break;
                    case ControlsEnum.TYPE:
                        //Get CheckListType
                        GetCheckListType();
                        admCheckListTypeCFGObj.CLT_PK = grpType;
                        admCheckListTypeCFGObj.CLT_CODE = string.IsNullOrEmpty(grpTypeCode) ? string.Empty : grpTypeCode;
                        admCheckListTypeCFGObj.CLT_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                        admCheckListTypeList = AdmCheckListServiceClient.GetCheckListTypes(admCheckListTypeCFGObj);
                        break;

                } 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admCheckListGroupMstObj = null;
                serviceUtilityObj = null;
                CommonServiceClient = null;
                admControlListObj = null;
                admConstGrpListObj = null;
                AdmCheckListServiceClient = null;
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
                    case ControlsEnum.GROUPDATA:
                        GetGroupUIValuesFromObject();
                        break;
                    case ControlsEnum.DEFAULT:
                        BindItemGrid();
                        break;
                    case ControlsEnum.GROUP:
                        BindGroupGrid();
                        BindGroupDropDown();
                        break;
                    case ControlsEnum.CONTROL:
                        BindControlDropDown();
                        break;
                    case ControlsEnum.KEY:
                        BindKeyDropDown();
                        break;
                    case ControlsEnum.ITEMDATA:
                        GetItemUIValuesFromObject();
                        break;
                    case ControlsEnum.TYPE:
                        SetBreadcrumb();
                        SetCheckListType();
                        break;

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
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private ADM_CHECK_LIST_GROUP_MST SetGroupUIValuesToObject()
        {
            
            int sequence=0;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                admCheckListGroupMstObj.CGM_PK = GroupCurrPK;
                admCheckListGroupMstObj.CGM_CODE = HttpUtility.HtmlEncode(txtGroupCode.Text.Trim());
                admCheckListGroupMstObj.CGM_NAME = HttpUtility.HtmlEncode(txtGroupName.Text.Trim());
                admCheckListGroupMstObj.CGM_TYPE = TypeCurrPK;
                admCheckListGroupMstObj.CGM_SEQUENCE = Int32.TryParse(HttpUtility.HtmlEncode(txtGroupSequence.Text.Trim()), out sequence) == true ? sequence : CommonConstants.SequenceNumber ;
                admCheckListGroupMstObj.CGM_DESC = HttpUtility.HtmlEncode(txtGroupDesc.Text.Trim());
                admCheckListGroupMstObj.CGM_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                admCheckListGroupMstObj.CGM_BIZUNIT = currentUser.SBUID;
                admCheckListGroupMstObj.CGM_MOD_BY = currentUser.PKUser;
                return admCheckListGroupMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admCheckListGroupMstObj = null;
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private ADM_CHECK_LIST_ITEM_MST SetItemUIValuesToObject()
        {
            int sequence = 0;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                admCheckListItemMstObj.CHI_PK = ItemCurrPK;
                admCheckListItemMstObj.CHI_CODE = HttpUtility.HtmlEncode(txtItemCode.Text.Trim());
                admCheckListItemMstObj.CHI_NAME = HttpUtility.HtmlEncode(txtItemName.Text.Trim());
                admCheckListItemMstObj.CHI_DESC = HttpUtility.HtmlEncode(txtItemDesc.Text.Trim());
                admCheckListItemMstObj.CHI_GROUP = Convert.ToInt32(ddlGroup.SelectedValue);
                admCheckListItemMstObj.CHI_CONTROL = Convert.ToInt32(ddlControl.SelectedValue);
                if (Convert.ToInt32(ddlControl.SelectedValue) == 4)
                {
                    admCheckListItemMstObj.CHI_CONST_GROUP =Convert.ToInt32(ddlParameter.SelectedValue);
                }
                else
                {
                    admCheckListItemMstObj.CHI_CONST_GROUP = null;
                }
                admCheckListItemMstObj.CHI_SEQUENCE = Int32.TryParse(HttpUtility.HtmlEncode(txtItemSequence.Text.Trim()), out sequence) == true ? sequence : 0;
                admCheckListItemMstObj.CHI_ACTIVE = chkItemActive.Checked ? (byte)1 : (byte)0; //(byte)DbActiveStatus.ACTIVE;
                admCheckListItemMstObj.CHI_BIZUNIT = currentUser.SBUID;
                admCheckListItemMstObj.CHI_MOD_BY = currentUser.PKUser;
                admCheckListItemMstObj.CHI_MOD_DT = LastModifiedTime;
                return admCheckListItemMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admCheckListItemMstObj = null;
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
                if (admCheckListGroupMstList != null && admCheckListGroupMstList.Count() > 0)
                {
                    GroupCurrPK = admCheckListGroupMstList[0].CGM_PK;
                    txtGroupCode.Text = HttpUtility.HtmlDecode(admCheckListGroupMstList[0].CGM_CODE);
                    txtGroupName.Text = HttpUtility.HtmlDecode(admCheckListGroupMstList[0].CGM_NAME);
                    txtGroupDesc.Text = HttpUtility.HtmlDecode(admCheckListGroupMstList[0].CGM_DESC);
                    txtGroupSequence.Text = HttpUtility.HtmlDecode(admCheckListGroupMstList[0].CGM_SEQUENCE.ToString());
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
        /// Sets the Item UI input controls from the object values
        /// </summary>
        private void GetItemUIValuesFromObject()
        {
            try
            {
                //assigning the UI controls with the corresponding ListObject value AsrSovMstList
                if (admCheckListItemMstListForGrid != null && admCheckListItemMstListForGrid.Count() > 0)
                {
                    ItemCurrPK = admCheckListItemMstListForGrid[0].CHI_PK;
                    txtItemCode.Text = HttpUtility.HtmlDecode(admCheckListItemMstListForGrid[0].CHI_CODE);
                    txtItemName.Text = HttpUtility.HtmlDecode(admCheckListItemMstListForGrid[0].CHI_NAME);
                    txtItemDesc.Text = HttpUtility.HtmlDecode(admCheckListItemMstListForGrid[0].CHI_DESC);
                    ddlControl.SelectedValue = admCheckListItemMstListForGrid[0].CHI_CONTROL.ToString();
                    ddlParameter.SelectedValue = admCheckListItemMstListForGrid[0].CHI_CONST_GROUP != null ? admCheckListItemMstListForGrid[0].CHI_CONST_GROUP.ToString() : CommonConstants.SELECTVAL;
                    txtItemSequence.Text = HttpUtility.HtmlDecode(admCheckListItemMstListForGrid[0].CHI_SEQUENCE.ToString());
                    chkItemActive.Checked = admCheckListItemMstListForGrid[0].CHI_ACTIVE == 1 ? true : false;

                    ModifiedDatePnl.Visible = true;
                    LastModifiedTime = admCheckListItemMstListForGrid[0].CHI_MOD_DT;
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);


                }
                //Concurrency Account details Deleted By Another User
                else
                {
                    litItemErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litItemErrorMsg.Text = string.Format(litItemErrorMsg.Text.ToLower(), Resources.PageNameRes.CheckListItem);
                    ResetItemForm();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;
                    throw new Exception(litItemErrorMsg.Text);
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
        public void BindItemGrid()
        {
            try
            {
                ModifiedDatePnl.Visible = false;
                if (admCheckListItemMstListForGrid != null)
                {
                    uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdItemMst.DataSource = admCheckListItemMstListForGrid;
                    grdItemMst.DataBind();
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


        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGroupGrid()
        {
            try
            {
                if (admCheckListGroupMstList != null)
                {
                    grdGroupsMst.DataSource = admCheckListGroupMstList;
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
            if (admCheckListGroupMstList != null && admCheckListGroupMstList.Count > 0)
            {
                admCheckListGroupMstList = admCheckListGroupMstList.OrderBy(c => c.CGM_SEQUENCE).ToList();
                ddlGroup.DataSource = admCheckListGroupMstList;
                ddlGroup.DataTextField = Resources.DataFieldRes.CGMName;
                ddlGroup.DataValueField = Resources.DataFieldRes.CGMPK;
                ddlGroup.DataBind();
            }
            else
            {
                ddlGroup.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            }
        }

        /// <summary>
        /// Method for Control DropDown
        /// </summary>
        public void BindControlDropDown()
        {
            ddlControl.Items.Clear();
            if (admControlList != null && admControlList.Count > 0)
            {
                ddlControl.DataSource = admControlList;
                ddlControl.DataTextField = Resources.DataFieldRes.CTLName;
                ddlControl.DataValueField = Resources.DataFieldRes.CTLPK;
                ddlControl.DataBind();
            }

            ddlControl.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            
        }

        /// <summary>
        /// Method for Key DropDown
        /// </summary>
        public void BindKeyDropDown()
        {
            ddlParameter.Items.Clear();
            if (admConstGrpList != null && admConstGrpList.Count > 0)
            {
                ddlParameter.DataSource = admConstGrpList;
                ddlParameter.DataTextField = Resources.DataFieldRes.GroupName;
                ddlParameter.DataValueField = Resources.DataFieldRes.GroupPK;
                ddlParameter.DataBind();
            }

            ddlParameter.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

        }


        /// <summary>
        /// Method used to Reset Group form Controls
        /// </summary>
        private void ResetGroupForm()
        {
            GroupCurrPK = 0;
            txtGroupCode.Text = string.Empty;
            txtGroupName.Text = string.Empty;
            txtGroupDesc.Text = string.Empty;
            txtGroupSequence.Text = CommonConstants.SequenceNumber.ToString();
            PageIndex = CommonConstants.SELECT_VALUE_ONE;
        }

        /// <summary>
        /// Method used to Reset Item form Controls
        /// </summary>
        private void ResetItemForm()
        {
            ItemCurrPK = 0;
            txtItemCode.Text = string.Empty;
            txtItemName.Text = string.Empty;
            txtItemDesc.Text = string.Empty;
            chkItemActive.Checked = true;
            ddlControl.SelectedValue = CommonConstants.SELECTVAL;
            ddlParameter.SelectedValue = CommonConstants.SELECTVAL;
            txtItemSequence.Text = CommonConstants.SequenceNumber.ToString();
            PageIndex = CommonConstants.SELECT_VALUE_ONE;
        }

        /// <summary>
        /// Method used to set Sequence
        /// </summary>
        private void setSequence()
        {
            txtGroupSequence.Text = CommonConstants.SequenceNumber.ToString();
            txtItemSequence.Text = CommonConstants.SequenceNumber.ToString();
        }

        /// <summary>
        /// Method used to get CheckList Type
        /// </summary>
        private void GetCheckListType()
        {
            //get group type
            if (Request.QueryString[QueryStrings.Type] != null)
            {
                //if type is not PK check Typecode
                if (!Int32.TryParse(HttpUtility.HtmlDecode(Request.QueryString[QueryStrings.Type]), out grpType))
                {
                    grpTypeCode = HttpUtility.HtmlDecode(Request.QueryString[QueryStrings.Type]);
                }
            }
            else
            {
                grpTypeCode = string.Empty;
                grpType = 1;
            }
        }

        /// <summary>
        /// Method used to Check CheckList Type valid or not
        /// </summary>
        private bool CheckCheckListType()
        {
            if (admCheckListTypeList != null && admCheckListTypeList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Method used to set CheckList Type
        /// </summary>
        private void SetCheckListType()
        {
            if (admCheckListTypeList != null && admCheckListTypeList.Count > 0)
            {
                TypeCurrPK = admCheckListTypeList[0].CLT_PK;
            }            
        }


        /// <summary>
        /// Method used to set Breadcrumb
        /// </summary>
        private void SetBreadcrumb()
        {
            string breadCrumb;
            if (admCheckListTypeList != null && admCheckListTypeList.Count > 0)
            {
                breadCrumb = GetLocalResourceObject("Breadcrumb") + admCheckListTypeList[0].CLT_NAME;
            }
            else
            {
                breadCrumb = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "");
            }
            breadCrumb = breadCrumb.ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            lblBreadCrum.Text = breadCrumb;
        }

        /// <summary>
        /// Method used to Disable Controls
        /// </summary>
        private void DisableControls()
        {
            btnSave.Visible = false;
            addItem.Visible = false;
            Group.Visible = false;
            itemlist.Visible = false;
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
            AdmCheckListMstService AdmCheckListServiceClient;
            AdmCheckListServiceClient = null;
            try
            {
                int result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName)); ;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                switch (commonActions)
                {

                    #region ADDGROUP
                    case ActionsEnum.ADDGROUP:
                                    ResetGroupForm();
                                    GetFieldValues(ControlsEnum.GROUP);
                                    SetFieldValues(ControlsEnum.GROUP);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowGroup", "ShowGroup();", true);
                                    txtGroupCode.Focus();
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
                            
                                admCheckListGroupMstList = new List<ADM_CHECK_LIST_GROUP_MST>();
                                AdmCheckListServiceClient = new AdmCheckListMstService();
                                AdmCheckListServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCheckListServiceClient);
                                admCheckListGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_GROUP_MST>();
                                admCheckListGroupMstObj = SetGroupUIValuesToObject();
                                admCheckListGroupMstList.Add(admCheckListGroupMstObj);
                                result = AdmCheckListServiceClient.SaveCheckListGroups(admCheckListGroupMstList);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    //SortBy = Resources.DataFieldRes.CGMPK;
                                    //SortDirection = Resources.Report.SortDescending;
                                    ResetGroupForm();
                                    GetFieldValues(ControlsEnum.GROUP);
                                    SetFieldValues(ControlsEnum.GROUP);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowGroup", "ShowGroup();", true);
                                    litGroupErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                    litGroupErrorMsg.Text = string.Format(litGroupErrorMsg.Text.ToLower(), Resources.PageNameRes.CheckListGroup);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litGroupErrorMsg.Text+ "','" + Resources.Messages.Information + "');", true);

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
                        txtGroupCode.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowGroup", "ShowGroup();", true);
                        break;
                    #endregion

                    #region DELETEGROUP
                    case ActionsEnum.GROUPGRIDDELETE:
                        GroupCurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        AdmCheckListServiceClient = new AdmCheckListMstService();
                        AdmCheckListServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCheckListServiceClient);
                        admCheckListGroupMstList = new List<ADM_CHECK_LIST_GROUP_MST>();
                        admCheckListGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_GROUP_MST>();
                        admCheckListGroupMstObj.CGM_PK = GroupCurrPK;
                        admCheckListGroupMstList.Add(admCheckListGroupMstObj);

                        result = AdmCheckListServiceClient.DeleteCheckListGroups(admCheckListGroupMstList);
                        if (result > 0)
                        {
                          ResetGroupForm();
                          GetFieldValues(ControlsEnum.GROUP);
                          SetFieldValues(ControlsEnum.GROUP);
                          ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowGroup", "ShowGroup();", true);
                          litGroupErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                          litGroupErrorMsg.Text = string.Format(litGroupErrorMsg.Text.ToLower(), Resources.PageNameRes.CheckListGroup);
                          ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litGroupErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region ITEMSAVE
                    case ActionsEnum.ITEMSAVE:
                        if (!IsValid)
                        {
                            litItemErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litItemErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {

                            admCheckListItemMstList = new List<ADM_CHECK_LIST_ITEM_MST>();
                            AdmCheckListServiceClient = new AdmCheckListMstService();
                            AdmCheckListServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCheckListServiceClient);
                            admCheckListItemMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_ITEM_MST>();
                            admCheckListItemMstObj = SetItemUIValuesToObject();
                            admCheckListItemMstList.Add(admCheckListItemMstObj);
                            result = AdmCheckListServiceClient.SaveCheckListItems(admCheckListItemMstList);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                SortBy = Resources.DataFieldRes.CHIPK;
                                SortDirection = Resources.Report.SortDescending;
                                litItemErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litItemErrorMsg.Text = string.Format(litItemErrorMsg.Text.ToLower(), Resources.PageNameRes.CheckListItem);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litItemErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                ResetItemForm();
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            //Duplicate records
                            else if (result == -1)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("DuplicateError").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                txtItemCode.Focus();
                            }

                        }
                        break;
                    #endregion

                    #region EDITITEM
                    case ActionsEnum.ITEMGRIDEDIT:
                        ItemCurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        // Get And Set the Item Details
                        GetFieldValues(ControlsEnum.ITEMDATA);
                        SetFieldValues(ControlsEnum.ITEMDATA);
                        EntryStatus = EntryStatus.ENTRYMODE;
                        txtItemCode.Focus();
                        break;
                    #endregion

                    #region DELETEITEM
                    case ActionsEnum.ITEMGRIDDELETE:
                          ItemCurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                          AdmCheckListServiceClient = new AdmCheckListMstService();
                          AdmCheckListServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCheckListServiceClient);
                          admCheckListItemMstList = new List<ADM_CHECK_LIST_ITEM_MST>();
                          admCheckListItemMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_ITEM_MST>();
                          admCheckListItemMstObj.CHI_PK = ItemCurrPK;
                          admCheckListItemMstList.Add(admCheckListItemMstObj);

                          result = AdmCheckListServiceClient.DeleteCheckListItems(admCheckListItemMstList);
                          if (result > 0)
                           {
                             ResetItemForm();
                             btnSave.Focus();
                             GetFieldValues(ControlsEnum.DEFAULT);
                             SetFieldValues(ControlsEnum.DEFAULT);
                             EntryStatus = EntryStatus.LISTMODE;
                             litItemErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                             litItemErrorMsg.Text = string.Format(litItemErrorMsg.Text.ToLower(), Resources.PageNameRes.CheckListItem);
                             ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litItemErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                          }
                          //delete reference error
                          else if (result == -1)
                          {
                              ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("DeleteError").ToString()) + "','" + Resources.Messages.Information + "');", true);
                          }
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL :
                        ResetItemForm();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        this.btnSave.Focus();
                        break;
                    #endregion

                    #region SelectIndexChanged
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        //Drop down change event
                        ResetItemForm();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        this.btnSave.Focus();
                        break;
                    #endregion

                    #region NEW
                    case ActionsEnum.NEW:
                        if (Convert.ToInt32(ddlGroup.SelectedValue) > Convert.ToInt32(CommonConstants.SELECTVAL))
                        {
                            EntryStatus = EntryStatus.NEWMODE;
                            ResetItemForm();
                            ModifiedDatePnl.Visible = false;
                            txtItemCode.Focus();
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
            }
            finally
            {
                admCheckListGroupMstObj = null;
                admCheckListGroupMstList = null;
                AdmCheckListServiceClient = null;
                admCheckListItemMstList = null;
                admCheckListItemMstObj = null;
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
                else if (GridID == "grdItemMst")
                {
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;
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
                EntryStatus = EntryStatus.LISTMODE;
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
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EnableDisableKey", "EnableDisableKey();", true);
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
            ITEMDATA,
            GROUP,
            GROUPDATA,
            SEARCH,
            CONTROL,
            KEY,
            TYPE
        }
        #endregion

    }
}