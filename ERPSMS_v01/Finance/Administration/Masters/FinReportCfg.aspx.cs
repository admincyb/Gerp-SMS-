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
using BusinessLogic.Finance.Administration.Masters;
using BusinessObject.Finance.Administration.Masters;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.Finance.Administration.Masters
{
    public partial class FinReportCfg : ERP.Store.UI.MyBasePage
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
        /// Set SBU ID
        /// </summary>
        private int SBUID
        {
            get
            {
                return this.ViewState[ViewstateStrings.SBUID] == null ? 1 : Convert.ToInt32(this.ViewState[ViewstateStrings.SBUID]);
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
        private int ReportPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.ReportPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ReportPK] = value;
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
        private FIN_COA_MST accountMstObj;
        private ADM_CONFIG_MST admConfigMstObj;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private ServiceUtility serviceUtilityObj;
        private List<FIN_COA_MST> accountMstList;
        SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result rptTempObj;
        private List<ADM_CONFIG_MST> admConfigMstRptList;
        private List<ADM_APP_CONFIG_MST> admAppConfigMstList;
        private FinReportCfgBO objFinReportCfgBO;
        DataTable dtList;
        DataTable dtParentList;
        DataTable dtAccountTree;
        DataTable dtAccountDtls;
        DataTable dtType;
        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;

        private int CoaPk;
        private bool AntLevel;
        private bool AntParent;
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
            //CommonService CommonService = null;
            try
            {
                AccountMstService = new AccountMstService();
                AccountMstService = ERP.Utilities.CommonFunctions.InitiateClient(AccountMstService);
                accountMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_MST>();
                rptTempObj = new SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result();
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        SetReportPK();
                        int ParentID = Convert.ToInt32(ddlSearchParent.SelectedValue) > 0 ? Convert.ToInt32(ddlSearchParent.SelectedValue) : 0;
                        string name = txtSearchName.Text;
                        string dispName = txtSearchDispName.Text;
                        dtList = FinReportCfgBL.GetKVFinReportCfg(CurrPK, Convert.ToByte(DbActiveStatus.ALL), Convert.ToByte(ReportPK), null, SBUID, null, null, name, ParentID, dispName);
                        break;
                    case ControlsEnum.ACCOUNTS:
                        SetCurrPK();
                        dtAccountDtls = FinReportCfgBL.GetKVFinReportCfg(CurrPK, Convert.ToByte(DbActiveStatus.HASPK), Convert.ToByte(ReportPK), null, SBUID, null, null, string.Empty, 0, string.Empty);
                        break;
                    case ControlsEnum.TYPECATEGORY:
                        dtType = CommonBL.GetAppConfig(SBUID, GetLocalResourceObject("AccountType").ToString());
                        break;
                    case ControlsEnum.REPORTHEAD:
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("ReportTemplate").ToString();
                        admConfigMstRptList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    case ControlsEnum.TREE:
                        dtAccountTree = FinReportCfgBL.GetKVFinReportCfg(0, Convert.ToByte(DbActiveStatus.ALL), Convert.ToByte(ReportPK), null, SBUID, null, null, string.Empty, 0, string.Empty);
                        break;
                    case ControlsEnum.PARENTACNTS:
                        dtParentList = FinReportCfgBL.GetKVFinReportCfg(0, Convert.ToByte(DbActiveStatus.ALL), Convert.ToByte(ReportPK), 1, SBUID, null, null, string.Empty, null, string.Empty);
                        break;
                    case ControlsEnum.CONFIG:
                        CommonServiceClient = new CommonService();
                        admAppConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_CONFIG_MST>();
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppConfigMstObj.ACF_PK = 111;
                        admAppConfigMstObj.ACF_SETTING = GetLocalResourceObject("AccountLevel").ToString();
                        admAppConfigMstList = CommonServiceClient.GetADM_APP_CONFIG_MST(admAppConfigMstObj);
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
                        BindGrid(ControlsEnum.DEFAULT);
                        break;
                    case ControlsEnum.TREE:
                        BindTree();
                        break;
                    case ControlsEnum.PARENTACNTS:
                        BindDropDown(ControlsEnum.PARENTACNTS);
                        break;
                    case ControlsEnum.TYPECATEGORY:
                        BindDropDown(ControlsEnum.TYPECATEGORY);
                        break;
                    case ControlsEnum.REPORTHEAD:
                        BindGrid(ControlsEnum.REPORTHEAD);
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
        }
        private object SetUIValuesToObject(ControlsEnum ControlType)
        {
            object returnObj;
            returnObj = null;
            objFinReportCfgBO = new FinReportCfgBO();
            List<FinReportCfgDtls> lstFinReportCfgDtls = new List<FinReportCfgDtls>();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (ControlType)
                {
                    case ControlsEnum.SAVE:

                        objFinReportCfgBO.BIZUNIT_PK = SBUID;
                        objFinReportCfgBO.LAST_MOD_DT = LastModifiedTime;
                        objFinReportCfgBO.USER_PK = currentUser.PKUser;
                        FinReportCfgDtls dtlObj = new FinReportCfgDtls();
                        dtlObj.RTC_PK = this.CurrPK;
                        dtlObj.RTC_NAME = txtCoaName.Text;
                        dtlObj.RTC_DISPLAY_NAME = txtCoaDispName.Text;
                        if (ddlParent.SelectedIndex > 0)
                        {
                            dtlObj.RTC_PARENT = ddlParent.SelectedValue;
                        }
                        dtlObj.RTC_DESC = txtCoaDesc.Text;
                        dtlObj.RTC_IS_GROUP = Convert.ToByte(chkIsGroup.Checked);
                        dtlObj.RTC_IS_BOLD = Convert.ToByte(chkIsBold.Checked);
                        dtlObj.RTC_IS_TOTAL1 = Convert.ToByte(chkIsTotal1.Checked);
                        dtlObj.RTC_IS_TOTAL2 = Convert.ToByte(chkIsTotal2.Checked);
                        dtlObj.RTC_SEQUENCE = Convert.ToByte(txtSequence.Text);
                        dtlObj.RTC_SPL_COND = txtSplCondition.Text;
                        dtlObj.RTC_TYPE = Convert.ToInt32(ddlType.SelectedValue);
                        dtlObj.RTC_TEMPLATE = Convert.ToByte(this.ReportPK);
                        dtlObj.RTC_ACTIVE = Convert.ToByte(chkActive.Checked);
                        objFinReportCfgBO.FinReportCfgDtls = new List<FinReportCfgDtls>();
                        objFinReportCfgBO.FinReportCfgDtls.Add(dtlObj);
                        returnObj = objFinReportCfgBO;
                        break;
                }
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            try
            {
                if (dtAccountDtls != null && dtAccountDtls.Rows.Count > 0)
                {
                    CurrPK = Convert.ToInt32(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_PK].ToString());
                    if (dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_PARENT].ToString() != string.Empty && dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_PARENT].ToString() != null)
                    {
                        ddlParent.SelectedIndex = ddlParent.Items.IndexOf(ddlParent.Items.FindByValue(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_PARENT].ToString()));
                    }
                    if (dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_TYPE].ToString() != string.Empty && dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_TYPE].ToString() != null)
                    {
                        ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_TYPE].ToString()));

                    }
                    txtCoaName.Text = HttpUtility.HtmlDecode(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_NAME].ToString());
                    txtCoaDispName.Text = HttpUtility.HtmlDecode(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_DISPLAY_NAME].ToString());
                    txtCoaDesc.Text = HttpUtility.HtmlDecode(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_DESC].ToString());
                    chkIsGroup.Checked = Convert.ToInt32(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_IS_GROUP].ToString()) == 1 ? true : false;
                    if (Convert.ToInt32(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_CHILD_COUNT]) > 0)
                    {
                        chkIsGroup.Enabled = false;
                    }
                    txtSequence.Text = HttpUtility.HtmlDecode(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_SEQUENCE].ToString());
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                    LastModifiedTime = Convert.ToDateTime(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_MOD_DT].ToString());
                    chkIsBold.Checked = Convert.ToInt32(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_IS_BOLD].ToString()) == 1 ? true : false;
                    chkIsTotal1.Checked = Convert.ToInt32(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_IS_TOTAL1].ToString()) == 1 ? true : false;
                    chkIsTotal2.Checked = Convert.ToInt32(dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_IS_TOTAL2].ToString()) == 1 ? true : false;
                    txtSplCondition.Text = dtAccountDtls.Rows[0][Resources.DataFieldRes.RTC_SPL_COND].ToString();
                }
                //Concurrency Account details Deleted By Another User
                else
                {
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Accounts);
                    EntryStatus = EntryStatus.LISTMODE;
                    ResetForm(ControlsEnum.CANCEL);
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
                TreeNode root;

                trvAccounts.Nodes.Clear();
                root = new TreeNode("Accounts", "0");
                trvAccounts.Nodes.Add(root);

                if (dtAccountTree != null && dtAccountTree.Rows.Count > 0)
                {
                    DataTable dtLevel;
                    var rtcLevel = dtAccountTree.AsEnumerable().Min(x => x[Resources.DataFieldRes.RTC_LEVEL]);
                    var query = dtAccountTree.AsEnumerable().Where(x => x[Resources.DataFieldRes.RTC_LEVEL].Equals(rtcLevel));
                    dtLevel = query.CopyToDataTable();
                    foreach (DataRow dr in dtLevel.Rows)
                    {
                        root = new TreeNode();
                        root.Text = dr[Resources.DataFieldRes.RTC_NAME].ToString();
                        root.ToolTip = dr[Resources.DataFieldRes.RTC_NAME].ToString();
                        root.Value = dr[Resources.DataFieldRes.RTC_PK].ToString();
                        root.SelectAction = TreeNodeSelectAction.None;
                        CreateNode(root, dtAccountTree);
                        trvAccounts.Nodes.Add(root);
                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        public void CreateNode(TreeNode node, DataTable dtNode)
        {
            TreeNode childNode;
            int nodes = Convert.ToInt32(node.Value);
            var query = dtNode.AsEnumerable().Where(x => x[Resources.DataFieldRes.RTC_PARENT].Equals(nodes));
            if (query.Any())
            {
                DataTable dtChild = query.CopyToDataTable();
                if (dtChild != null && dtChild.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtChild.Rows)
                    {
                        childNode = new TreeNode();
                        childNode.Text = dr[Resources.DataFieldRes.RTC_NAME].ToString();
                        childNode.ToolTip = dr[Resources.DataFieldRes.RTC_NAME].ToString();
                        childNode.Value = dr[Resources.DataFieldRes.RTC_PK].ToString();
                        childNode.SelectAction = TreeNodeSelectAction.Select;
                        node.ChildNodes.Add(childNode);
                        CreateNode(childNode, dtNode);
                    }
                }
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region DEFAULT
                    case ControlsEnum.DEFAULT:
                        if (dtList != null && dtList.Rows.Count > 0)
                        {
                            grdAccounts.DataSource = dtList;
                            grdAccounts.DataBind();
                        }
                        break;
                    #endregion
                    #region REPORTHEAD
                    case ControlsEnum.REPORTHEAD:
                        if (admConfigMstRptList != null)
                        {
                            grdAccountReport.DataSource = admConfigMstRptList;
                            grdAccountReport.DataBind();
                        }
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
        /// Method for Parent DropDown
        /// </summary>
        public void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.PARENTACNTS:
                    ddlParent.Items.Clear();
                    ddlSearchParent.Items.Clear();
                    if (dtParentList != null && dtParentList.Rows.Count > 0)
                    {
                        ddlSearchParent.DataSource = dtParentList;
                        ddlSearchParent.DataTextField = dtParentList.Columns[Resources.DataFieldRes.RTC_NAME].ToString();
                        ddlSearchParent.DataValueField = dtParentList.Columns[Resources.DataFieldRes.RTC_PK].ToString();
                        ddlSearchParent.DataBind();
                        ddlParent.DataSource = dtParentList;
                        ddlParent.DataTextField = dtParentList.Columns[Resources.DataFieldRes.RTC_NAME].ToString();
                        ddlParent.DataValueField = dtParentList.Columns[Resources.DataFieldRes.RTC_PK].ToString();
                        ddlParent.DataBind();
                    }
                    ddlSearchParent.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlParent.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.TYPECATEGORY:
                    ddlType.Items.Clear();
                    if (dtType != null && dtType.Rows.Count > 0)
                    {
                        ddlType.DataSource = dtType;
                        ddlType.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlType.DataValueField = Resources.DataFieldRes.ConfigPK;
                        ddlType.DataBind();
                    }
                    break;
            }
        }


        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                PreEntryStatus = EntryStatus;
                if (EntryStatus != EntryStatus.TREEMODE)
                {
                    SetCurrPK();
                    if (CurrPK > 0)
                    {
                        // Get And Set the Location details
                        GetFieldValues(ControlsEnum.PARENTACNTS);
                        SetFieldValues(ControlsEnum.PARENTACNTS);
                        GetFieldValues(ControlsEnum.TYPECATEGORY);
                        SetFieldValues(ControlsEnum.TYPECATEGORY);
                        GetFieldValues(ControlsEnum.ACCOUNTS);
                        SetFieldValues(ControlsEnum.ACCOUNTS);
                        txtCoaName.Focus();
                        ModifiedDatePnl.Visible = true;
                        if (Mode == ActionsEnum.VIEW)
                            EntryStatus = EntryStatus.VIEWMODE;
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;
                        return;
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    }
                }
                else if (trvAccounts.SelectedNode != null)
                {
                    CurrPK = Convert.ToInt32(trvAccounts.SelectedNode.Value);
                    GetFieldValues(ControlsEnum.PARENTACNTS);
                    SetFieldValues(ControlsEnum.PARENTACNTS);
                    GetFieldValues(ControlsEnum.TYPECATEGORY);
                    SetFieldValues(ControlsEnum.TYPECATEGORY);
                    GetFieldValues(ControlsEnum.ACCOUNTS);
                    SetFieldValues(ControlsEnum.ACCOUNTS);
                    txtCoaName.Focus();
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
        private void SetReportPK()
        {
            foreach (GridViewRow grdrow in grdAccountReport.Rows)
            {
                RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                // check row selected or not
                if (rbtn.Checked)
                {
                    // get pk from the grid and assign to CurrPk
                    ReportPK = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfReportPK")).Value);
                    return;
                }
            }
        }
        private void SetCurrPK()
        {
            foreach (GridViewRow grdrow in grdAccounts.Rows)
            {
                RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                // check row selected or not
                if (rbtn.Checked)
                {
                    // get pk from the grid and assign to CurrPk
                    CurrPK = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfAccPK")).Value);
                    return;
                }
            }
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.CANCEL:
                    CurrPK = 0;
                    ReportPK = 0;
                    txtCoaName.Text = string.Empty;
                    txtCoaDispName.Text = string.Empty;
                    txtCoaDesc.Text = string.Empty;
                    txtSequence.Text = string.Empty;
                    txtSplCondition.Text = string.Empty;
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    lblLastModifiedHDR.Text = string.Empty;
                    ModifiedDatePnl.Visible = false;
                    ddlParent.Enabled = true;
                    chkIsGroup.Checked = false;
                    chkIsBold.Checked = false;
                    chkIsTotal1.Checked = false;
                    chkIsTotal2.Checked = false;
                    chkIsGroup.Enabled = true;
                    break;
                case ControlsEnum.CLEAR:
                    txtSearchName.Text = string.Empty;
                    txtSearchDispName.Text = string.Empty;
                    ddlSearchParent.SelectedIndex = Convert.ToInt32(CommonConstants.SELECTVAL);
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    break;
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
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept("../../../login.aspx"))
                return;

            AccountMstService AccountMstService;
            AccountMstService = null;
            try
            {
                int? result;
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
                            objFinReportCfgBO = (FinReportCfgBO)SetUIValuesToObject(ControlsEnum.SAVE);
                            if (objFinReportCfgBO != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<FinReportCfgBO>(objFinReportCfgBO);
                                result = FinReportCfgBL.SaveFinReportCfg(xmlDoc);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    SortBy = Resources.DataFieldRes.AccountPK;
                                    SortDirection = Resources.Report.SortDescending;
                                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Accounts);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = PreEntryStatus;
                                    GetFieldValues(ControlsEnum.TREE);
                                    SetFieldValues(ControlsEnum.TREE);
                                    ResetForm(ControlsEnum.CANCEL);
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);

                                    btnNew.Focus();
                                }
                                else if (result == -1)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("DuplicateError_NAME").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    //  this.txtCoaCode.Focus();
                                }
                                else if (result == -3)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("DuplicateError_NAME").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    //  this.txtCoaCode.Focus();
                                }
                                else if (result == -2)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Invalid_Parent").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    this.ddlParent.Focus();
                                }


                            }
                        }
                        break;
                    #endregion

                    #region New
                    case ActionsEnum.NEW:
                        ModifiedDatePnl.Visible = false;
                        this.txtCoaName.Focus();
                        GetFieldValues(ControlsEnum.PARENTACNTS);
                        SetFieldValues(ControlsEnum.PARENTACNTS);
                        GetFieldValues(ControlsEnum.TYPECATEGORY);
                        SetFieldValues(ControlsEnum.TYPECATEGORY);
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.CANCEL);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnNew.Focus();
                        EntryStatus = PreEntryStatus;
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
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
                        accountMstObj.COA_PARENT = Convert.ToInt32(ddlParent.SelectedValue);
                        accountMstObj.COA_MOD_DT = LastModifiedTime;
                        accountMstList.Add(accountMstObj);

                        result = AccountMstService.DeleteAccountsMaster(accountMstList);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            ResetForm(ControlsEnum.CANCEL);
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

                        // ResetForm();
                        SetReportPK();
                        if (ReportPK > 0)
                        {
                            GetFieldValues(ControlsEnum.PARENTACNTS);
                            SetFieldValues(ControlsEnum.PARENTACNTS);
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region TreeMode
                    case ActionsEnum.TREE_SELECT:
                        SetReportPK();
                        if (ReportPK > 0)
                        {
                            GetFieldValues(ControlsEnum.TREE);
                            SetFieldValues(ControlsEnum.TREE);
                            EntryStatus = EntryStatus.TREEMODE;
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region ReportMode
                    case ActionsEnum.REPORT:
                        EntryStatus = EntryStatus.REPORTMODE;
                        break;
                    #endregion
                    #region Print
                    case ActionsEnum.PRINT:
                        break;
                    #endregion

                    #region SelectChange
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(Resources.DataFieldRes.AccountName))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.Name)) + "','" + Resources.Messages.Information + "');", true);
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
            else if (EntryStatus == EntryStatus.REPORTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(0);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(0);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(0);", true);
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
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
                    ConfigurationSettings();
                    GetFieldValues(ControlsEnum.REPORTHEAD);
                    SetFieldValues(ControlsEnum.REPORTHEAD);
                    EntryStatus = EntryStatus.REPORTMODE;
                    //hdfAppType.Value = ApplicationType.COA;
                    //hdfAppSubType.Value = string.Empty;
                    //hdfHasChild.Value = string.Empty;
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
            PARENTACNTS,
            TYPECATEGORY,
            CONFIG,
            ACNTLEVEL,
            PINGROUP,
            REPORTHEAD,
            CANCEL,
            CLEAR,
            SAVE
        }
        #endregion
    }
}