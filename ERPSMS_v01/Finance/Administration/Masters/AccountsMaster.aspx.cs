using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPService.Administration;
using ERPSMS_v01.UserControls;
using System.Data;
using BusinessLogic.Administration.Masters;
using BusinessObject.Finance;
using System.Xml;
using System.Transactions;

namespace ERPSMS_v01.Finance.Administration.Masters
{
    public partial class AccountsMaster : ERP.Store.UI.MyBasePage
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

        private List<FIN_COA_MST> AccountTreeList
        {
            get
            {
                return Session["AccountMstList"] == null ? null : (List<FIN_COA_MST>)Session["AccountMstList"];
            }
            set
            {
                Session["AccountMstList"] = value;
            }
        }

        /// <summary>
        /// Set SBU ID
        /// </summary>
        private int SBUID
        {
            get
            {
                return this.ViewState[ViewstateStrings.SBUID] == null ? -1 : Convert.ToInt32(this.ViewState[ViewstateStrings.SBUID]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SBUID] = value;
            }

        }

        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus PreEntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.PreEntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.PreEntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PreEntryState] = value;
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
        /// Current PK
        /// </summary>
        private int AllocateCurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.AllocateCurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.AllocateCurrPK] = value;
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
        private FIN_COA_MST accountMstObj;
        private ADM_COST_CENTER_MST costcenterMstObj;
        private FIN_COA_COST_CENTER_MPG finCistCenterMpgObj;
        private CostCenterMpg CostCenterMpgObj;
        private ADM_CONFIG_MST admConfigMstObj;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private FIN_COA_SUB_TYPE_CFG finCoaSubTypeCfgObj;
        private ServiceUtility serviceUtilityObj;
        //List for binding details to controls
        private List<FIN_COA_MST> accountMstList;
        private List<ADM_COST_CENTER_MST> costcenterMstList;
        private DataTable dtFinGroup;
        private DataTable dtReportColumn;
        private DataTable dtCompany;
        private DataTable dtCostCenterList;
        private List<ADM_CONFIG_MST> admConfigMstList;
        private List<ADM_APP_CONFIG_MST> admAppConfigMstList;
        private List<FIN_COA_SUB_TYPE_CFG> finCoaSubTypeCfgList;

        //private int AllocateCurrPK;

        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;
        private int CompanyPk = 0;
        XmlDocument xmlDoc;

        private int CoaPk;
        private bool AntLevel;
        private bool AntParent;

        private COATreeBO treeObj;
        private TreeNode CurrentNode;
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

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            AccountMstService AccountMstService = null;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                SBUID = currentUser.SBUID;
                AccountMstService = new AccountMstService();
                AccountMstService = ERP.Utilities.CommonFunctions.InitiateClient(AccountMstService);
                accountMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_MST>();
                costcenterMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COST_CENTER_MST>();
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdAccounts.PageSize;
                        serviceUtilityObj.FilterBy = ddlFilterBy.SelectedValue == CommonConstants.SELECT_VALUE_ZERO ? null : ddlFilterBy.SelectedValue;
                        serviceUtilityObj.FilterValue = HttpUtility.HtmlDecode(txtSearchBy.Text.Trim());
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.AccountSequence : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        accountMstObj.COA_PK = CurrPK;
                        accountMstObj.COA_BIZUNIT = SBUID;
                        accountMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        accountMstList = AccountMstService.GetFinCoaMst(accountMstObj, serviceUtilityObj);
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    case ControlsEnum.ACCOUNTS:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        accountMstObj.COA_PK = CurrPK;
                        accountMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        accountMstList = AccountMstService.GetFinCoaMst(accountMstObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.TREE:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        accountMstObj.COA_PK = CurrPK;
                        accountMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        accountMstList = AccountMstService.GetFinCoaMst(accountMstObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.TYPECATEGORY:
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("AccountType").ToString();
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    case ControlsEnum.CONFIG:
                        CommonServiceClient = new CommonService();
                        admAppConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_CONFIG_MST>();
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppConfigMstObj.ACF_PK = 111;
                        admAppConfigMstObj.ACF_SETTING = GetLocalResourceObject("AccountLevel").ToString();
                        admAppConfigMstList = CommonServiceClient.GetADM_APP_CONFIG_MST(admAppConfigMstObj);
                        break;
                    case ControlsEnum.ACNTLEVEL:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        accountMstObj.COA_PK = CoaPk;
                        accountMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        accountMstList = AccountMstService.GetFinCoaMst(accountMstObj, serviceUtilityObj);
                        break;
                    #region PINGROUP
                    case ControlsEnum.PINGROUP:
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        dtFinGroup = AccountMstService.GetFinGroup(0, (int)DbActiveStatus.ACTIVE, (int)ReportTemplate.All, 0, currentUser.SBUID);
                        break;
                    #endregion
                    #region REPORTCOLUMN
                    case ControlsEnum.REPORTCOLUMN:
                        dtReportColumn = CostCenterMasterBL.GetCostCenterGroup(currentUser.SBUID, 27, 3);
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), currentUser.SBUID, CompanyPk);
                        break;
                    #endregion
                    #region Cost Center
                    case ControlsEnum.COSTCENTER:
                        CommonServiceClient = new CommonService();
                        costcenterMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COST_CENTER_MST>();
                        costcenterMstObj.CNM_PK = CurrPK;
                        if (AllocateCurrPK != null && CurrPK == 0)
                        {
                            costcenterMstObj.CNM_PK = AllocateCurrPK;
                            dtCostCenterList = AccountMapingBL.GetCostCenter(AllocateCurrPK);
                        }
                        //costcenterMstObj.CNM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        //costcenterMstList = AccountMstService.GetADM_COST_CENTER_MST(costcenterMstObj);
                        else
                        {
                            dtCostCenterList = AccountMapingBL.GetCostCenter(CurrPK);
                        }
                        break;
                    #endregion
                    case ControlsEnum.TREENODES:
                        int nodePK = 0;
                        if (CurrentNode != null)
                            nodePK = Convert.ToInt32(CurrentNode.Value);
                        string xmlResult = BusinessLogic.Finance.CommSetupBL.GetCOATreeNodes(nodePK, currentUser.SBUID);
                        treeObj = CommonFunctions.XmlDeserialize<COATreeBO>(xmlResult);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accountMstObj = null;
                serviceUtilityObj = null;
                AccountMstService = null;
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
                    case ControlsEnum.ACCOUNTS:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        //BindTree();
                        break;
                    case ControlsEnum.TREE:
                        //BindTree();
                        break;
                    case ControlsEnum.TYPECATEGORY:
                        BindTypeCategoryDDLs();
                        break;
                    case ControlsEnum.PINGROUP:
                        BindFinGroupDDL();
                        break;
                    case ControlsEnum.REPORTCOLUMN:
                        BindRptColumnDDL();
                        break;
                    case ControlsEnum.COMPANY:
                        BindCompanyDDL();
                        break;
                    case ControlsEnum.COSTCENTER:
                        BindCostCenter();
                        break;
                    case ControlsEnum.TREENODES:
                        BindTree_New(null);
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
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtConfig = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "COA");
            if (dtConfig != null && dtConfig.Rows.Count > 0)
                SBUID = dtConfig.Rows[0]["ACF_VALUE"].ToString() == "1" ? -1 : currentUser.SBUID;

            hdfAddParentChildValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "AddParentChildValidation").ToString();

        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private FIN_COA_MST SetUIValuesToObject()
        {
            try
            {

                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                accountMstObj.COA_PK = CurrPK;
                accountMstObj.COA_CODE = HttpUtility.HtmlEncode(txtCoaCode.Text.Trim());
                //accountMstObj.COA_PARENT = Convert.ToInt32(hdfCoaParent.Value);
                if (Convert.ToInt32(hdfCoaParent.Value) > 0)
                    accountMstObj.COA_PARENT = Convert.ToInt32(hdfCoaParent.Value);
                else
                    accountMstObj.COA_PARENT = null;
                accountMstObj.COA_NAME = HttpUtility.HtmlEncode(txtCoaName.Text.Trim());
                accountMstObj.COA_SHORT_NAME = HttpUtility.HtmlEncode(txtCoaShortName.Text.Trim());
                accountMstObj.COA_DESC = HttpUtility.HtmlEncode(txtCoaDesc.Text.Trim());
                accountMstObj.COA_TYPE = Convert.ToInt32(ddlCoaType.SelectedValue);
                accountMstObj.COA_IS_GROUP = chkIsGroup.Checked;
                accountMstObj.COA_SUB_TYPE = Convert.ToInt32(ddlSubType.SelectedValue);
                if (ddlFinGroup.SelectedIndex > 0)
                    accountMstObj.COA_REPORT_TEMPLATE = Convert.ToInt32(ddlFinGroup.SelectedValue);
                else
                    accountMstObj.COA_REPORT_TEMPLATE = null;

                if (ddlCompany.SelectedIndex > 0)
                    accountMstObj.COA_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                else
                    accountMstObj.COA_COMPANY = null;
                if (ddlReportColumn.SelectedIndex > 0)
                    accountMstObj.COA_REPORT_COLUMN = Convert.ToInt32(ddlReportColumn.SelectedValue);
                else
                    accountMstObj.COA_REPORT_COLUMN = null;

                accountMstObj.COA_SEQUENCE = Convert.ToInt16(txtSequence.Text.Trim());
                accountMstObj.COA_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());

                accountMstObj.COA_MOD_DT = LastModifiedTime;
                accountMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                accountMstObj.COA_BIZUNIT = currentUser.SBUID;
                accountMstObj.COA_DEPT = currentUser.CurrentDeptPK;

                //#region CostCenter Save
                //if (GetGlobalResourceObject("ConfigurationsRes", "IsShowAccountCCMapping").ToString() == "1")
                //{
                //    if (hdfIsAllocate.Value == CommonConstants.SELECT_VALUE_ONE)
                //    {
                //        int rowID = 0;
                //        HiddenField hdfCostCenterID;
                //        TextBox txtPercentage;
                //        foreach (GridViewRow grdrow in grdCostCenter.Rows)
                //        {
                //            finCistCenterMpgObj = CommonFunctions.Initilize<FIN_COA_COST_CENTER_MPG>();
                //            hdfCostCenterID = (HiddenField)grdCostCenter.Rows[rowID].FindControl("hdfCostCenterID");
                //            txtPercentage = (TextBox)grdCostCenter.Rows[rowID].FindControl("txtPercentage");
                //            finCistCenterMpgObj.FCM_CNM_PK = Convert.ToInt32(hdfCostCenterID.Value);
                //            finCistCenterMpgObj.FCM_VALUE = Convert.ToDouble(txtPercentage.Text);
                //            finCistCenterMpgObj.FCM_COA_PK = CurrPK;
                //            accountMstObj.FIN_COA_COST_CENTER_MPG.Add(finCistCenterMpgObj);
                //        }
                //    }
                //}
                //#endregion
                return accountMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accountMstObj = null;
            }
        }


        private object SetUIValuesToObjectAllocate(ControlsEnum controlType)
        {
            object returnObj;
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            #region CostCenter Save
            if (GetGlobalResourceObject("ConfigurationsRes", "IsShowAccountCCMapping").ToString() == "1")
            {
                if (hdfIsAllocate.Value == CommonConstants.SELECT_VALUE_ONE)
                {
                    HiddenField hdfCostCenterID;
                    HiddenField hdfCostCenterPK;
                    TextBox txtPercentage;
                    CheckBox chkSelectCC;
                    CostCenterMpgObj = new CostCenterMpg();
                    CostCenterMpgObj.FCM_COA_PK = AllocateCurrPK;
                    CostCenterMpgObj.CC_PERCNT_IS_REQD = GetGlobalResourceObject("ConfigurationsRes", "CCPercentageRequired").ToString();
                    CostCenterMpgObj.DetailsList = new List<DetailsBO>();
                    DetailsBO objDetail;
                    decimal sum = 0;
                    var footerRow = grdCostCenter.FooterRow;
                    foreach (GridViewRow grdrow in grdCostCenter.Rows)
                    {
                        chkSelectCC = (CheckBox)grdrow.FindControl("chkCCselect");
                        txtPercentage = (TextBox)grdrow.FindControl("txtPercentage");
                        if (txtPercentage.Text != string.Empty && txtPercentage.Text != ".")
                        {
                            sum = sum + Convert.ToDecimal(txtPercentage.Text);
                        }
                        //if (sum <= 100)
                        //{
                        if (chkSelectCC.Checked)
                        {
                            hdfCostCenterID = (HiddenField)grdrow.FindControl("hdfCostCenterID");
                            hdfCostCenterPK = (HiddenField)grdrow.FindControl("hdfCCPK");
                            txtPercentage = (TextBox)grdrow.FindControl("txtPercentage");
                            objDetail = new DetailsBO();
                            if (txtPercentage.Text != ".")
                            {
                                float Percentage = txtPercentage.Text == string.Empty ? 0 : float.Parse(txtPercentage.Text);
                                objDetail.FCM_VALUE = Percentage;
                            }
                            objDetail.FCM_CNM_PK = Convert.ToInt32(hdfCostCenterID.Value);
                            GetFieldValues(ControlsEnum.COSTCENTER);
                            //if (dtCostCenterList != null && dtCostCenterList.Rows.Count > 0)
                            //{
                            //    objDetail.FCM_PK =  Convert.ToInt32(dtCostCenterList.Rows[0]["FCM_PK"].ToString());
                            //}
                            if (hdfCostCenterPK.Value != "")
                            {
                                objDetail.FCM_PK = Convert.ToInt32(hdfCostCenterPK.Value);
                            }
                            CostCenterMpgObj.DetailsList.Add(objDetail);
                        }
                        //}
                        //else
                        //{
                        //    litErrorMsg.Text = this.GetLocalResourceObject("TotalPercentageMustBeLessThanHundred").ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                        // }
                    }
                }
            }
            #endregion
            returnObj = CostCenterMpgObj;
            return returnObj;
        }






        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            try
            {
                //assigning the UI controls with the corresponding ListObject value AsrSovMstList
                if (accountMstList != null && accountMstList.Count() > 0)
                {
                    CurrPK = accountMstList[0].COA_PK;
                    txtCoaCode.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_CODE);
                    if (string.IsNullOrEmpty(accountMstList[0].COA_PARENT.ToString()) || accountMstList[0].COA_PARENT == 0)
                    {
                        txtCoaParent.Text = string.Empty;
                        hdfCoaParent.Value = "0";
                        txtCoaParent.Enabled = false;
                        vcfCoaParent.Enabled = false;
                    }
                    else
                    {
                        txtCoaParent.Text = HttpUtility.HtmlDecode(accountMstList[0].FIN_COA_MST2.COA_NAME.ToString() + " (" + accountMstList[0].FIN_COA_MST2.COA_CODE.ToString() + ")");
                        hdfCoaParent.Value = accountMstList[0].COA_PARENT.ToString();
                        vcfCoaParent.Enabled = true;
                        txtCoaParent.Enabled = true;
                    }
                    txtCoaName.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_NAME);
                    txtCoaShortName.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_SHORT_NAME);
                    txtCoaDesc.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_DESC);
                    ddlCoaType.SelectedIndex = Convert.ToInt32(ddlCoaType.Items.IndexOf(ddlCoaType.Items.FindByValue(accountMstList[0].COA_TYPE.ToString())));
                    ddlCoaType.Enabled = false;
                    chkIsGroup.Checked = Convert.ToBoolean(accountMstList[0].COA_IS_GROUP.ToString());
                    hdfHasChild.Value = (accountMstList[0].FIN_COA_MST1.Count > 0 ? 1 : 0).ToString();
                    ddlSubType.SelectedIndex = Convert.ToInt32(ddlSubType.Items.IndexOf(ddlSubType.Items.FindByValue(accountMstList[0].COA_SUB_TYPE.ToString())));
                    ddlReportColumn.SelectedIndex = Convert.ToInt32(ddlReportColumn.Items.IndexOf(ddlReportColumn.Items.FindByValue(accountMstList[0].COA_REPORT_COLUMN.ToString())));

                    ddlFinGroup.SelectedIndex = -1;
                    if (accountMstList[0].COA_REPORT_TEMPLATE != null)
                        if (accountMstList[0].COA_REPORT_TEMPLATE.ToString() != string.Empty)
                            if (accountMstList[0].COA_REPORT_TEMPLATE != 0)
                                ddlFinGroup.SelectedIndex = Convert.ToInt32(ddlFinGroup.Items.IndexOf(ddlFinGroup.Items.FindByValue(accountMstList[0].COA_REPORT_TEMPLATE.ToString())));

                    ddlCompany.SelectedIndex = -1;
                    if (accountMstList[0].COA_COMPANY != null)
                        if (accountMstList[0].COA_COMPANY.ToString() != string.Empty)
                            if (accountMstList[0].COA_COMPANY != 0)
                                ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(accountMstList[0].COA_COMPANY.ToString())));

                    txtSequence.Text = accountMstList[0].COA_SEQUENCE.ToString();
                    txtRemarks.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_REMARKS);
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                    LastModifiedTime = accountMstList[0].COA_MOD_DT;


                }
                //Concurrency Account details Deleted By Another User
                else
                {
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Accounts);
                    EntryStatus = EntryStatus.LISTMODE;
                    ResetForm();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    ModifiedDatePnl.Visible = false;
                    throw new Exception(litErrorMsg.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Tree Binding
        /// </summary>
        public void BindTree()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                SBUID = currentUser.SBUID;
                AccountMstService AccountMstService = null;
                AccountMstService = new AccountMstService();
                AccountMstService = ERP.Utilities.CommonFunctions.InitiateClient(AccountMstService);
                accountMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_MST>();
                serviceUtilityObj = new ServiceUtility();
                serviceUtilityObj.CurrentPage = -1;
                serviceUtilityObj.PageSize = -1;
                serviceUtilityObj.SortBy = Resources.DataFieldRes.AccountSequence;
                serviceUtilityObj.SortDirection = Resources.ErpRes.SortAscending;
                accountMstObj.COA_PK = CurrPK;
                accountMstObj.COA_BIZUNIT = SBUID;
                accountMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                AccountTreeList = AccountMstService.GetFinCoaMst(accountMstObj, serviceUtilityObj);

                AccountMstService = null;
                TreeNode root;

                trvAccounts.Nodes.Clear();
                //root = new TreeNode("Accounts", "0");
                //trvAccounts.Nodes.Add(root);
                if (accountMstList != null)
                {
                    List<FIN_COA_MST> acntList = (from accountList in AccountTreeList
                                                  where accountList.COA_LEVEL == 1
                                                  select accountList).ToList();
                    foreach (FIN_COA_MST accounts in acntList)
                    {
                        root = new TreeNode();
                        //root.Text = ERP.Utilities.CommonFunctions.GetShortString(accounts.COA_NAME + " (" + accounts.COA_CODE + ")", 50);
                        //root.ToolTip = HttpUtility.HtmlDecode(accounts.COA_NAME + " (" + accounts.COA_CODE + ")");                        
                        root.Text = HttpUtility.HtmlDecode(accounts.COA_CODE + " - " + accounts.COA_NAME);
                        root.ToolTip = HttpUtility.HtmlDecode(accounts.COA_DESC);
                        root.Value = accounts.COA_PK.ToString();
                        root.PopulateOnDemand = true;
                        root.Collapse();
                        root.SelectAction = TreeNodeSelectAction.None;
                        // root.ChildNodes.Add(new TreeNode());
                        // CreateNode(root, accountMstList);
                        trvAccounts.Nodes.Add(root);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }

        private void BindTree_New(TreeNode node)
        {
            if (node == null)
            {
                TreeNode root;
                trvAccounts.Nodes.Clear();
                if (treeObj != null)
                {
                    foreach (COATreeNode accounts in treeObj.TreeNodes)
                    {
                        root = new TreeNode();
                        root.Text = HttpUtility.HtmlDecode(accounts.COA_TEXT);
                        root.ToolTip = HttpUtility.HtmlDecode(accounts.COA_DESC);
                        root.Value = accounts.COA_PK.ToString();
                        if (accounts.COA_IS_GROUP == 1)
                            root.PopulateOnDemand = true;
                        root.Collapse();
                        root.SelectAction = TreeNodeSelectAction.None;
                        trvAccounts.Nodes.Add(root);
                    }
                }
            }
            else
            {
                TreeNode childNode;
                if (treeObj == null) { return; }
                foreach (COATreeNode accounts in treeObj.TreeNodes)
                {
                    childNode = new TreeNode();
                    childNode.Text = HttpUtility.HtmlDecode(accounts.COA_TEXT);
                    childNode.ToolTip = HttpUtility.HtmlDecode(accounts.COA_DESC);
                    childNode.Value = accounts.COA_PK.ToString();
                    if (accounts.COA_IS_GROUP == 1)
                        childNode.PopulateOnDemand = true;
                    childNode.SelectAction = TreeNodeSelectAction.Select;
                    node.ChildNodes.Add(childNode);
                }
            }
        }

        protected void ActionHandler(object sender, TreeNodeEventArgs e)
        {
            ////currentUser = (IdentityUser)HttpContext.Current.User.Identity;
            //int pk = Convert.ToInt32(e.Node.Value);
            //CreateNode(e.Node, AccountTreeList);
            ////dtElementHead = ElementBL.GetElementsTreeOnDemand(pk, 0, 0, currentUser.SBU, CurrPK);
            ////foreach (DataRow rowElement in dtElementHead.Rows)
            ////{
            ////    TreeNode node = new TreeNode();
            ////    node.Text = rowElement[CapexBudgets.F_ELEMENTCODETEXT].ToString();
            ////    node.ToolTip = rowElement[CapexBudgets.F_ELEMENTCODETEXT].ToString();
            ////    node.Value = rowElement[CapexBudgets.F_PK].ToString();
            ////    if (rowElement[CapexBudgets.F_HASCHILD].ToString() == "true")
            ////        node.PopulateOnDemand = true;
            ////    e.Node.ChildNodes.Add(node);
            ////}

            CurrentNode = e.Node;
            GetFieldValues(ControlsEnum.TREENODES);
            BindTree_New(e.Node);
        }

        public void CreateNode(TreeNode node, List<FIN_COA_MST> acntMstList)
        {
            TreeNode childNode;

            List<FIN_COA_MST> acntList = (from accountList in acntMstList
                                          where accountList.COA_PARENT == Convert.ToInt32(node.Value)
                                          select accountList).ToList();

            if (acntList.Count == 0) { return; }

            foreach (FIN_COA_MST accounts in acntList)
            {
                childNode = new TreeNode();
                //childNode.Text = ERP.Utilities.CommonFunctions.GetShortString(accounts.COA_NAME + " (" + accounts.COA_CODE + ")", 50);
                //childNode.ToolTip = HttpUtility.HtmlDecode(accounts.COA_NAME + " (" + accounts.COA_CODE + ")");                
                childNode.Text = HttpUtility.HtmlDecode(accounts.COA_CODE + " - " + accounts.COA_NAME);
                childNode.ToolTip = HttpUtility.HtmlDecode(accounts.COA_DESC);
                childNode.Value = accounts.COA_PK.ToString();
                childNode.SelectAction = TreeNodeSelectAction.Select;
                node.ChildNodes.Add(childNode);
                CreateNode(childNode, acntMstList);
            }
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid()
        {
            try
            {
                if (accountMstList != null)
                {
                    uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdAccounts.DataSource = accountMstList;
                    grdAccounts.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Binding Cost Center grid
        /// </summary>
        private void BindCostCenter()
        {
            if (dtCostCenterList != null && dtCostCenterList.Rows.Count > 0)
            {
                grdCostCenter.DataSource = dtCostCenterList;
                if (GetGlobalResourceObject("ConfigurationsRes", "CCPercentageRequired").ToString() == "1")
                    grdCostCenter.Columns[2].Visible = true;
                else
                    grdCostCenter.Columns[2].Visible = false;
                grdCostCenter.DataBind();
            }
            else
            {
                grdCostCenter.DataSource = null;
                grdCostCenter.DataBind();
            }
        }

        /// <summary>
        /// Method for Fin Group DropDown
        /// </summary>
        public void BindFinGroupDDL()
        {
            ddlFinGroup.Items.Clear();
            if (dtFinGroup != null && dtFinGroup.Rows.Count > 0)//accountMstList != null && accountMstList.Count > 0
            {
                ddlFinGroup.DataSource = dtFinGroup;
                ddlFinGroup.DataTextField = "RTC_NAME";
                ddlFinGroup.DataValueField = "RTC_PK";
                ddlFinGroup.DataBind();
            }
            ddlFinGroup.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        public void BindRptColumnDDL()
        {
            ddlReportColumn.Items.Clear();
            if (dtReportColumn != null && dtReportColumn.Rows.Count > 0)
            {
                ddlReportColumn.DataSource = dtReportColumn;
                ddlReportColumn.DataTextField = Resources.DataFieldRes.ConstName;
                ddlReportColumn.DataValueField = Resources.DataFieldRes.ConstPK;
                ddlReportColumn.DataBind();
            }
            ddlReportColumn.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        public void BindCompanyDDL()
        {
            ddlCompany.Items.Clear();
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                ddlCompany.DataTextField = "CMP_NAME_CODE";
                ddlCompany.DataValueField = "CMP_PK";
                ddlCompany.DataSource = dtCompany;
                ddlCompany.DataBind();
            }
            ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        /// <summary>
        /// Method for Type & Category Dropdowns
        /// </summary>
        public void BindTypeCategoryDDLs()
        {
            ddlCoaType.Items.Clear();
            if (admConfigMstList != null && admConfigMstList.Count > 0)
            {
                ddlCoaType.DataSource = admConfigMstList;
                ddlCoaType.DataTextField = Resources.DataFieldRes.ConfigName;
                ddlCoaType.DataValueField = Resources.DataFieldRes.ConfigPK;
                ddlCoaType.DataBind();
            }
            ddlCoaType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

            CommonServiceClient = new CommonService();
            finCoaSubTypeCfgObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_SUB_TYPE_CFG>();
            finCoaSubTypeCfgObj.CST_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
            finCoaSubTypeCfgObj.CST_PK = -1;
            finCoaSubTypeCfgList = CommonServiceClient.GetSubTypeCfgValues(finCoaSubTypeCfgObj);
            ddlSubType.Items.Clear();
            if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
            {
                ddlSubType.DataSource = finCoaSubTypeCfgList;
                ddlSubType.DataTextField = Resources.DataFieldRes.CoaSubTypeName;
                ddlSubType.DataValueField = Resources.DataFieldRes.CoaSubTypePK;
                ddlSubType.DataBind();
            }
            ddlSubType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                PreEntryStatus = EntryStatus;
                if (EntryStatus == EntryStatus.LISTMODE)
                {
                    foreach (GridViewRow grdrow in grdAccounts.Rows)
                    {
                        RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        // check row selected or not
                        if (rbtn.Checked)
                        {
                            // get pk from the grid and assign to CurrPk
                            CurrPK = Convert.ToInt16(grdAccounts.DataKeys[grdrow.RowIndex].Values[0]);
                            // Get And Set the Location details 
                            GetFieldValues(ControlsEnum.PINGROUP);
                            SetFieldValues(ControlsEnum.PINGROUP);
                            GetFieldValues(ControlsEnum.REPORTCOLUMN);
                            SetFieldValues(ControlsEnum.REPORTCOLUMN);
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            GetFieldValues(ControlsEnum.TYPECATEGORY);
                            SetFieldValues(ControlsEnum.TYPECATEGORY);
                            GetFieldValues(ControlsEnum.ACCOUNTS);
                            SetFieldValues(ControlsEnum.ACCOUNTS);
                            txtCoaCode.Focus();
                            ModifiedDatePnl.Visible = true;
                            if (Mode == ActionsEnum.VIEW)
                                EntryStatus = EntryStatus.VIEWMODE;
                            else
                                EntryStatus = EntryStatus.ENTRYMODE;
                            return;
                        }
                    }
                }
                else if (trvAccounts.SelectedNode != null)
                {
                    CurrPK = Convert.ToInt32(trvAccounts.SelectedNode.Value);
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.PINGROUP);
                    SetFieldValues(ControlsEnum.PINGROUP);
                    GetFieldValues(ControlsEnum.ACCOUNTS);
                    SetFieldValues(ControlsEnum.ACCOUNTS);
                    txtCoaCode.Focus();
                    ModifiedDatePnl.Visible = true;
                    if (Mode == ActionsEnum.VIEW)
                        EntryStatus = EntryStatus.VIEWMODE;
                    else
                        EntryStatus = EntryStatus.ENTRYMODE;
                    return;
                }
                else if (CurrPK > 0)
                {
                    GetFieldValues(ControlsEnum.PINGROUP);
                    SetFieldValues(ControlsEnum.PINGROUP);
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.TYPECATEGORY);
                    SetFieldValues(ControlsEnum.TYPECATEGORY);
                    GetFieldValues(ControlsEnum.REPORTCOLUMN);
                    SetFieldValues(ControlsEnum.REPORTCOLUMN);
                    GetFieldValues(ControlsEnum.ACCOUNTS);
                    SetFieldValues(ControlsEnum.ACCOUNTS);
                    txtCoaCode.Focus();
                    ModifiedDatePnl.Visible = true;
                    if (Mode == ActionsEnum.VIEW)
                        EntryStatus = EntryStatus.VIEWMODE;
                    else
                        EntryStatus = EntryStatus.ENTRYMODE;
                    return;
                }
                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                EntryStatus = PreEntryStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method used to Check Group 
        /// </summary>
        private void CheckGroup()
        {

            //if (chkIsGroup.Checked == false && CurrPK == 0)
            if (chkIsGroup.Checked == false)
            {
                GetFieldValues(ControlsEnum.CONFIG);
                CoaPk = hdfCoaParent.Value != string.Empty ? Convert.ToInt32(hdfCoaParent.Value) : 0;
                GetFieldValues(ControlsEnum.ACNTLEVEL);
                if (admAppConfigMstList != null && accountMstList != null)
                {
                    if ((accountMstList[0].COA_LEVEL + 1) >= admAppConfigMstList[0].ACF_VALUE)
                    {
                        AntLevel = true;
                    }
                    else
                    {
                        AntLevel = false;
                    }
                }
                else
                {
                    AntLevel = false;
                }
            }
            else
            {
                AntLevel = true;
            }

        }
        /// <summary>
        /// Method used to Check Parent 
        /// </summary>
        private void CheckParent()
        {
            //if (chkIsGroup.Checked == true && chkIsGroup.Enabled == true && CurrPK > 0)
            if (chkIsGroup.Enabled == true && CurrPK > 0)
            {
                if (hdfCoaParent.Value == CurrPK.ToString())
                {
                    AntParent = false;
                }
                else
                {
                    AntParent = true;
                }
            }
            else
            {
                AntParent = true;
            }
        }

        /// <summary>
        /// Method used to Set SubType
        /// </summary>
        private void setCategory()
        {
            if (accountMstList != null)
            {
                ddlSubType.SelectedValue = accountMstList[0].COA_SUB_TYPE.ToString();
            }
        }

        /// <summary>
        /// Method used to Set Tpe
        /// </summary>
        private void setType()
        {
            if (accountMstList != null)
            {
                if (accountMstList.Count == 1)
                {
                    ddlCoaType.SelectedValue = accountMstList[0].COA_TYPE.ToString();
                    ddlCoaType.Enabled = false;
                }
                else
                {
                    ddlCoaType.Enabled = true;
                    ddlCoaType.SelectedValue = CommonConstants.SELECTVAL;
                }
            }
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            AllocateCurrPK = 0;
            txtSearchBy.Text = string.Empty;
            txtCoaCode.Text = string.Empty;
            txtCoaParent.Text = string.Empty;
            hdfCoaParent.Value = "0";
            txtCoaName.Text = string.Empty;
            txtCoaShortName.Text = string.Empty;
            txtCoaDesc.Text = string.Empty;
            ddlCoaType.SelectedIndex = 0;
            ddlSubType.SelectedIndex = 0;
            ddlReportColumn.SelectedIndex = -1;
            ddlFinGroup.SelectedIndex = -1;
            ddlCompany.SelectedIndex = -1;
            txtSequence.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            PageIndex = CommonConstants.SELECT_VALUE_ONE;
            lblLastModifiedHDR.Text = string.Empty;
            ModifiedDatePnl.Visible = false;
            ddlCoaType.Enabled = true;
            vcfCoaParent.Enabled = true;
            txtCoaParent.Enabled = true;
            chkIsGroup.Checked = false;
            chkIsGroup.Enabled = true;
            hdfHasChild.Value = string.Empty;
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
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept("../../../login.aspx"))
                return;

            AccountMstService AccountMstService;
            AccountMstService = null;
            try
            {
                bool IsSuccess = false;
                int result;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                switch (commonActions)
                {
                    #region Save
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else//valid
                        {

                            CheckParent();
                            if (AntParent)
                            {
                                CheckGroup();
                                if (AntLevel)
                                {
                                    accountMstList = new List<FIN_COA_MST>();
                                    AccountMstService = new AccountMstService();
                                    AccountMstService = ERP.Utilities.CommonFunctions.InitiateClient(AccountMstService);
                                    accountMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_MST>();
                                    accountMstObj = SetUIValuesToObject();
                                    accountMstList.Add(accountMstObj);
                                    int parentChildValidation = 0;
                                    Int32.TryParse(hdfAddParentChildValidation.Value, out parentChildValidation);
                                    result = AccountMstService.SaveAccountsMaster(accountMstList, parentChildValidation);
                                    if (result > 0 && EntryStatus == EntryStatus.ALLOCATEMODE)
                                    {
                                        CurrPK = result;
                                        CostCenterMpgObj = (CostCenterMpg)SetUIValuesToObjectAllocate(ControlsEnum.COSTCENTERSAVE);
                                        if (CostCenterMpgObj != null)
                                        {
                                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(CostCenterMpgObj);
                                            result = AccountMapingBL.SaveCostCenter(xmlDoc.InnerXml);
                                        }
                                    }
                                    if (result > 0) // Success ! re-initialize the page
                                    {
                                        //SortBy = Resources.DataFieldRes.AccountPK;
                                        //SortDirection = Resources.Report.SortDescending;
                                        SortBy = Resources.DataFieldRes.AccountSequence;
                                        SortDirection = Resources.Report.SortAscending;
                                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Accounts);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        ResetForm();
                                        GetFieldValues(ControlsEnum.DEFAULT);
                                        SetFieldValues(ControlsEnum.DEFAULT);
                                        btnNew.Focus();
                                    }
                                    else if (result == -1)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("DuplicateError").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        this.txtCoaCode.Focus();
                                    }
                                    else if (result == -3)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("DuplicateError_NAME").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        this.txtCoaCode.Focus();
                                    }
                                    else if (result == -2)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Invalid_Parent").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        this.txtCoaParent.Focus();
                                    }
                                    else if (result == (int)DbSaveStatus.INCORRECT)
                                    {
                                        GetFieldValues(ControlsEnum.ACCOUNTS);
                                        SetFieldValues(ControlsEnum.ACCOUNTS);
                                        litErrorMsg.Text = this.GetLocalResourceObject("Err_TotalPercentage").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                    }

                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("CannotAdd").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("ParentError").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);

                            }



                            //if (hdfIsAllocate.Value == CommonConstants.SELECT_VALUE_ONE)
                            //{
                            //    CostCenterMpgObj = (CostCenterMpg)SetUIValuesToObjectAllocate(ControlsEnum.COSTCENTERSAVE);
                            //    if (CostCenterMpgObj != null)
                            //    {
                            //        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(CostCenterMpgObj);
                            //        result = AccountMapingBL.SaveCostCenter(xmlDoc.InnerXml);
                            //        if (result >= 0) // Success ! re-initialize the page
                            //        {
                            //            SortBy = Resources.DataFieldRes.AccountPK;
                            //            SortDirection = Resources.Report.SortDescending;
                            //            litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                            //            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CostCenter);
                            //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                            //            EntryStatus = PreEntryStatus;
                            //            ResetForm();
                            //            GetFieldValues(ControlsEnum.DEFAULT);
                            //            SetFieldValues(ControlsEnum.DEFAULT);
                            //            btnNew.Focus();
                            //        }
                            //        else//fail
                            //        {
                            //            if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.SQLERROR)
                            //            {
                            //                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //                    + "','" + Resources.ErpRes.Information + "');", true);
                            //            }
                            //            else if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.CONCURRENCY)
                            //            {
                            //                litErrorMsg.Text = Resources.PageNameRes.CostCenter + " " + Resources.Messages.EditUsedByAnotherUser;
                            //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //                + "','" + Resources.ErpRes.Information + "');", true);
                            //            }
                            //            else if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.CODEEXIST)
                            //            {
                            //                litErrorMsg.Text = Resources.PageNameRes.CostCenter + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            //            }
                            //            else if (result == (int)DbSaveStatus.ALREADYCREATED)
                            //            {
                            //                litErrorMsg.Text = Resources.PageNameRes.CostCenter + " " + GetLocalResourceObject("AlreadyCreated").ToString();
                            //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //                + "','" + Resources.ErpRes.Information + "');", true);
                            //            }
                            //            else if (result == (int)DbSaveStatus.INCORRECT)
                            //            {
                            //                litErrorMsg.Text = this.GetLocalResourceObject("TotalPercentageMustBeLessThanHundred").ToString();
                            //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true); 
                            //            }
                            //            else
                            //            {
                            //                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);
                            //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //                    + "','" + Resources.ErpRes.Information + "');", true);
                            //            }
                            //        }

                            //    }
                            //}
                        }
                        break;
                    #endregion


                    case ActionsEnum.SAVEALLOCATE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else//valid
                        {
                            if (hdfIsAllocate.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                CostCenterMpgObj = (CostCenterMpg)SetUIValuesToObjectAllocate(ControlsEnum.COSTCENTERSAVE);
                                if (CostCenterMpgObj != null)
                                {
                                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(CostCenterMpgObj);
                                    result = AccountMapingBL.SaveCostCenter(xmlDoc.InnerXml);
                                    if (result >= 0) // Success ! re-initialize the page
                                    {
                                        SortBy = Resources.DataFieldRes.AccountPK;
                                        SortDirection = Resources.Report.SortDescending;
                                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CostCenter);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        EntryStatus = PreEntryStatus;
                                        ResetForm();
                                        GetFieldValues(ControlsEnum.DEFAULT);
                                        SetFieldValues(ControlsEnum.DEFAULT);
                                        btnNew.Focus();
                                    }
                                    else//fail
                                    {
                                        if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.CostCenter + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.CostCenter + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.ALREADYCREATED)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.CostCenter + " " + GetLocalResourceObject("AlreadyCreated").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.INCORRECT)
                                        {
                                            litErrorMsg.Text = this.GetLocalResourceObject("TotalPercentageMustBeLessThanHundred").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }

                                }
                            }
                        }
                        break;


                    #region New
                    case ActionsEnum.NEW:
                        ModifiedDatePnl.Visible = false;
                        vcfCoaParent.Enabled = true;
                        txtCoaParent.Enabled = true;
                        this.txtCoaCode.Focus();
                        GetFieldValues(ControlsEnum.PINGROUP);
                        SetFieldValues(ControlsEnum.PINGROUP);
                        GetFieldValues(ControlsEnum.TYPECATEGORY);
                        SetFieldValues(ControlsEnum.TYPECATEGORY);
                        GetFieldValues(ControlsEnum.REPORTCOLUMN);
                        SetFieldValues(ControlsEnum.REPORTCOLUMN);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        btnSearch.Focus();
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnNew.Focus();
                        //EntryStatus = PreEntryStatus;
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Edit
                    case ActionsEnum.EDIT:
                        SetUIEditView(commonActions);
                        break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        AccountMstService = new AccountMstService();
                        AccountMstService = ERP.Utilities.CommonFunctions.InitiateClient(AccountMstService);
                        accountMstList = new List<FIN_COA_MST>();
                        accountMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_MST>();
                        accountMstObj.COA_PK = CurrPK;
                        accountMstObj.COA_PARENT = hdfCoaParent.Value != string.Empty ? Convert.ToInt32(hdfCoaParent.Value) : 0;
                        accountMstObj.COA_MOD_DT = LastModifiedTime;
                        accountMstList.Add(accountMstObj);

                        result = AccountMstService.DeleteAccountsMaster(accountMstList);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            ResetForm();
                            btnNew.Focus();
                            EntryStatus = PreEntryStatus;
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Accounts);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else if (result == -3)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("CannotdeleteAlreadyasigned").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(commonActions);
                        btnCancel.Focus();
                        break;
                    #endregion

                    #region ListMode
                    case ActionsEnum.SELECT:
                        ResetForm();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region TreeMode
                    case ActionsEnum.TREE_SELECT:
                        GetFieldValues(ControlsEnum.TYPECATEGORY);
                        SetFieldValues(ControlsEnum.TYPECATEGORY);
                        GetFieldValues(ControlsEnum.TREENODES);
                        SetFieldValues(ControlsEnum.TREENODES);
                        EntryStatus = EntryStatus.TREEMODE;
                        break;
                    #endregion

                    #region Print
                    case ActionsEnum.PRINT:
                        break;
                    #endregion


                    #region Allocate
                    case ActionsEnum.ALLOCATE:
                        PreEntryStatus = EntryStatus;
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            foreach (GridViewRow grdrow in grdAccounts.Rows)
                            {
                                RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    CurrPK = Convert.ToInt16(grdAccounts.DataKeys[grdrow.RowIndex].Values[0]);
                                    AllocateCurrPK = CurrPK;
                                    hdfIsAllocate.Value = CommonConstants.SELECT_VALUE_ONE;

                                    GetFieldValues(ControlsEnum.ACCOUNTS);
                                    if (accountMstList != null && accountMstList.Count() > 0)
                                    {
                                        lblhdrAccountCodeText.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_CODE);
                                        lblhdrAccountNameText.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(accountMstList[0].COA_NAME), 60);
                                        lblhdrAccountCodeText.ToolTip = HttpUtility.HtmlDecode(accountMstList[0].COA_CODE);
                                        lblhdrAccountNameText.ToolTip = HttpUtility.HtmlDecode(accountMstList[0].COA_NAME);
                                    }
                                    //SetFieldValues(ControlsEnum.ACCOUNTS);
                                    GetFieldValues(ControlsEnum.COSTCENTER);
                                    SetFieldValues(ControlsEnum.COSTCENTER);
                                    SetUIEditView(commonActions);

                                    ModifiedDatePnl.Visible = false;
                                    EntryStatus = EntryStatus.ALLOCATEMODE;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(4);", true);
                                    return;
                                }
                            }
                        }
                        else if (trvAccounts.SelectedNode != null)
                        {
                            CurrPK = Convert.ToInt32(trvAccounts.SelectedNode.Value);
                            AllocateCurrPK = CurrPK;
                            hdfIsAllocate.Value = CommonConstants.SELECT_VALUE_ONE;
                            GetFieldValues(ControlsEnum.ACCOUNTS);
                            if (accountMstList != null && accountMstList.Count() > 0)
                            {
                                lblhdrAccountCodeText.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_CODE);
                                lblhdrAccountNameText.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(accountMstList[0].COA_NAME), 60);
                                lblhdrAccountCodeText.ToolTip = HttpUtility.HtmlDecode(accountMstList[0].COA_CODE);
                                lblhdrAccountNameText.ToolTip = HttpUtility.HtmlDecode(accountMstList[0].COA_NAME);
                            }
                            GetFieldValues(ControlsEnum.COSTCENTER);
                            SetFieldValues(ControlsEnum.COSTCENTER);
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = false;
                            EntryStatus = EntryStatus.ALLOCATEMODE;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(4);", true);
                            return;
                        }
                        else if (CurrPK > 0)
                        {
                            AllocateCurrPK = CurrPK;
                            hdfIsAllocate.Value = CommonConstants.SELECT_VALUE_ONE;
                            GetFieldValues(ControlsEnum.ACCOUNTS);
                            if (accountMstList != null && accountMstList.Count() > 0)
                            {
                                lblhdrAccountCodeText.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_CODE);
                                lblhdrAccountNameText.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(accountMstList[0].COA_NAME), 60);
                                lblhdrAccountCodeText.ToolTip = HttpUtility.HtmlDecode(accountMstList[0].COA_CODE);
                                lblhdrAccountNameText.ToolTip = HttpUtility.HtmlDecode(accountMstList[0].COA_NAME);
                            }
                            GetFieldValues(ControlsEnum.COSTCENTER);
                            SetFieldValues(ControlsEnum.COSTCENTER);
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = false;
                            EntryStatus = EntryStatus.ALLOCATEMODE;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(4);", true);
                            return;
                        }
                        // if no items selected, Show Error Message
                        litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        EntryStatus = PreEntryStatus;
                        break;
                    #endregion

                    #region CHANGE
                    case ActionsEnum.CHANGE:
                        CoaPk = hdfCoaParent.Value != string.Empty ? Convert.ToInt32(hdfCoaParent.Value) : 0;
                        GetFieldValues(ControlsEnum.ACNTLEVEL);
                        setCategory();
                        setType();
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(Resources.DataFieldRes.AccountName))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.Name)) + "','" + Resources.Messages.Information + "');", true);
                else if (CommonFunctions.ProcessException(ex) == "547")
                {
                    litErrorMsg.Text = Resources.PageNameRes.Account + " " + Resources.Messages.UsedInAnotherPlace;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {
                accountMstObj = null;
                accountMstList = null;
                AccountMstService = null;
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                //{
                //    Label lblParent = e.Row.FindControl("lblParent") as Label;
                //    HiddenField hdfParent = e.Row.FindControl("hdfParent") as HiddenField;

                //    if (accountMstList != null && accountMstList.Count > 0)
                //    {
                //        if (hdfParent.Value != "")
                //        {
                //            accountMstObj = accountMstList.SingleOrDefault(acc => acc.COA_PK == Convert.ToInt32(hdfParent.Value));
                //            if (accountMstObj != null)
                //                lblParent.Text = accountMstObj.COA_NAME;
                //        }

                //    }

                //}
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (((GridView)sender).ID == "grdAccounts")
                    {
                        #region grdAccounts                        
                        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                        {
                            HiddenField hdfAccIsGroup = e.Row.FindControl("hdfAccIsGroup") as HiddenField;
                            if (hdfAccIsGroup.Value == "1") //	Is Group RowColor Setting
                            {
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("ErpRes", "selectedRowColor").ToString());
                            }
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
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
                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
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
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
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
                EntryStatus = EntryStatus.LISTMODE;
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
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(3);", true);
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(3);", true);
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(3);", true);
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.TREEMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
            }
            else if (EntryStatus == EntryStatus.ALLOCATEMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(4);", true);
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
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

                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    SBUID = currentUser.SBUID;
                    ConfigurationSettings();
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.AccountPK;
                    grdAccounts.DataKeyNames = datakeyarray;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;
                    hdfAppType.Value = ApplicationType.COA;
                    hdfAppSubType.Value = string.Empty;
                    hdfHasChild.Value = string.Empty;
                    ddlFinGroup.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsShowAccountCCMapping").ToString() == "1")
                        lbnAllocation.Visible = true;
                    else
                        lbnAllocation.Visible = false;
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsAdditionalDetails").ToString() == "1")
                        DivAdditionalDetails.Visible = true;
                    else
                        DivAdditionalDetails.Visible = false;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            ACCOUNTS,
            TREE,
            TYPECATEGORY,
            CONFIG,
            ACNTLEVEL,
            PINGROUP,
            REPORTCOLUMN,
            COMPANY,
            COSTCENTER,
            COSTCENTERSAVE,
            TREENODES
        }
        #endregion
    }
}