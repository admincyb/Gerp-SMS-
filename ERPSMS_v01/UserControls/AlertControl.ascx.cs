using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPData;
using ERP.Utilities;
using ERPService;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.AlertManagement;
using BusinessObject.AccountManagement;
using BusinessObject.Common;


namespace ERPSMS_v01.UserControls
{
    public partial class AlertControl : System.Web.UI.UserControl
    {
        #region Variables and Properties
        private ActionsEnum commonActions;
        /// <summary>
        ///
        /// </summary>
        public int? TypePK
        {
            get
            {
                return this.ViewState[ViewstateStrings.TypePK] != null ? (int)this.ViewState[ViewstateStrings.TypePK] : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.TypePK] = value;
            }
        }

        public string TypeCode
        {
            get
            {
                return this.ViewState[ViewstateStrings.TypeCode].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.TypeCode] = value;
            }
        }

        public string TypeRef
        {
            get
            {
                return this.ViewState[ViewstateStrings.TypeRef].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.TypeRef] = value;
            }
        }

        public string TypeText
        {
            get
            {
                return this.ViewState[ViewstateStrings.TypeText].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.TypeText] = value;
            }
        }
        public string TypePartyName
        {
            get
            {
                return this.ViewState[ViewstateStrings.TypePartyName].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.TypePartyName] = value;
            }
        }
        
        public int CurrPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrPK] != null ? (int)this.ViewState[ViewstateStrings.CurrPK] : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime TrxDate
        {
            get
            {
                return this.ViewState[ViewstateStrings.TrxDate] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.TrxDate];
            }
            set
            {
                this.ViewState[ViewstateStrings.TrxDate] = value;
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

        public string ReturnURL
        {
            get
            {
                return this.ViewState[ViewstateStrings.ReturnURL] == null ? string.Empty : this.ViewState[ViewstateStrings.ReturnURL].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.ReturnURL] = value;
                hdfAlertReturnURL.Value = value.ToString();
            }
        }
        private BusinessObject.User currentUser;
        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> admConfigMstList;
        private List<ADM_CONST_MST> admConstMstList;

        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConstMstList;

        DataSet dsAlertList;
        #endregion
        #region Page Level Events
        /// <summary>
        /// handles page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        #endregion
        #region Page Action Handler
        /// <summary>
        /// Handles page load
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    CurrPK = 0;
                    GetFieldValues(ControlsEnum.ALERTTYPES);
                    SetFieldValues(ControlsEnum.ALERTTYPES);
                    GetFieldValues(ControlsEnum.ALERTBASIS);
                    SetFieldValues(ControlsEnum.ALERTBASIS);
                    GetFieldValues(ControlsEnum.NOTIFICATIONTYPES);
                    SetFieldValues(ControlsEnum.NOTIFICATIONTYPES);
                    GetFieldValues(ControlsEnum.ALERTLIST);
                    SetFieldValues(ControlsEnum.ALERTLIST);
                    txtTrxDate.Text = TrxDate.ToString(Resources.Constants.DateFormatShort);
                    txtDueDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
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
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    case ControlsEnum.NOTIFICATIONTYPES:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("ALERT_NOTIFICATION_TYPES").ToString();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    case ControlsEnum.ALERTBASIS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("ALERT_BASIS").ToString();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    case ControlsEnum.ALERTTYPES:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, null, null, 16, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.ALERTLIST:
                        if (TypePK > 0 && !string.IsNullOrEmpty(TypeCode))
                        {
                            if (CurrPK > 0)
                            {
                                dsAlertList = BusinessLogic.AlertManagement.Alerts.GetAlertDetails(CurrPK, Convert.ToByte(DbActiveStatus.HASPK), null, TypeCode, TypePK, currentUser.SBUID,currentUser.PKUser);
                            }
                            else
                            {
                                dsAlertList = BusinessLogic.AlertManagement.Alerts.GetAlertDetails(0, Convert.ToByte(DbActiveStatus.ACTIVE), null, TypeCode, TypePK, currentUser.SBUID, currentUser.PKUser);
                            }
                        }

                        break;
                    case ControlsEnum.NOTIFICATIONDAYS:
                        if (!string.IsNullOrEmpty(TypeCode))
                        {
                            CommonServiceClient = new CommonService();
                            CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                            admAppConfigMstObj = new ADM_APP_CONFIG_MST();
                            admAppConfigMstObj.ACF_PK = 0;
                            admAppConfigMstObj.ACF_SETTING = GetLocalResourceObject("ALERT_NOTIFY_BEFORE").ToString();
                            admAppConfigMstObj.ACF_DATA = TypeCode;
                            admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                            admAppConstMstList = CommonServiceClient.GetAlertNotify(admAppConfigMstObj);
                        }
                        break;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CommonServiceClient = null;
            }
        }
        #endregion
        #region Set Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void SetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    case ControlsEnum.NOTIFICATIONTYPES:
                        //BindTree(ControlsEnum.NOTIFICATIONTYPES);
                        BindCheckBoxList(ControlsEnum.NOTIFICATIONTYPES);
                        break;
                    case ControlsEnum.ALERTLIST:
                        BindGrid(ControlsEnum.ALERTLIST);
                        break;
                    case ControlsEnum.ALERTTYPES:
                        BindDropDown(ControlsEnum.ALERTTYPES);
                        break;
                    case ControlsEnum.ALERTBASIS:
                        BindDropDown(ControlsEnum.ALERTBASIS);
                        break;
                    case ControlsEnum.NOTIFICATIONDAYS:
                        if (admAppConstMstList != null && admAppConstMstList.Count > 0)
                        {
                            txtNotifyBefore.Text = admAppConstMstList[0].ACF_VALUE.ToString();
                        }
                        break;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
        #region Action Handlers

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            int? result;
            bool bIsChecked = false;
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
                switch (commonActions)
                {
                    #region Save
                    case ActionsEnum.ALERTSAVE:
                        if (!Page.IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            AlertBO alertBoObj = new AlertBO();
                            alertBoObj = (AlertBO)SetUIValuesToObject(ActionsEnum.ALERTSAVE);
                            if (alertBoObj != null)
                            {
                                result = BusinessLogic.AlertManagement.Alerts.SaveAlertDetails(alertBoObj);
                                if (result > 0)
                                {
                                    // Show Save Message and redired to listing page                                        
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, "Alert");
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.ALERTLIST);
                                    SetFieldValues(ControlsEnum.ALERTLIST);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                            }
                        }

                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.ALERTEDIT:
                        foreach (GridViewRow grdrow in grdAlert.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfAlertPK")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            GetFieldValues(ControlsEnum.ALERTLIST);
                            GetUIValuesFromObject(ControlsEnum.ALERTDETAILS);
                            EntryStatus = EntryStatus.ENTRYMODE;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Alert").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.ALERTDELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.AlertManagement.Alerts.DeleteAlertDetails(CurrPK, LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, "Alert");
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                ResetForm();
                                GetFieldValues(ControlsEnum.ALERTLIST);
                                SetFieldValues(ControlsEnum.ALERTLIST);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Alert").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.ALERTCANCEL:
                        ResetForm();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                           "ClosePopup();", true);
                        if (!string.IsNullOrEmpty(ReturnURL))
                            Response.Redirect(ReturnURL);
                        break;
                    #endregion
                    #region Reset
                    case ActionsEnum.ALERTRESET:
                        GetAlertList();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                           "ClosePopup();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region --- For Grid Actions----
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
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
                SortBy = e.SortExpression;
                if (SortDirection == Resources.Report.SortAscending)
                    SortDirection = Resources.Report.SortDescending;
                else
                    SortDirection = Resources.Report.SortAscending;
                GetFieldValues(ControlsEnum.ALERTLIST);
                SetFieldValues(ControlsEnum.ALERTLIST);
                //EntryStatus = EntryStatus.LISTMODE;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion


        #endregion
        #region Pager Methods + Init
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
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            //this.lnkFuel.PreRender += new EventHandler(btnAction_PreRender);
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //(this.Page as MyBasePage).CheckBtnVisibility(sender);
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
                if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAlertListing", "$(document).ready(function(){ShowAlertListing();});", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlertListing", "$(document).ready(function(){ShowAlertListing(1);});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeAlertUserControlComponents", "$(document).ready(function(){AlertUserControlInitComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Helper Mathods
        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        public void ResetForm()
        {
            CurrPK = 0;
            txtAlertName.Text = string.Empty;
            txtNotifyBefore.Text = string.Empty;
            txtNoOfDays.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            chkCompleted.Checked = false;
            chkNotifyAll.Checked = false;
            foreach (ListItem chkObj in chkNotifyThrough.Items)
            {
                chkObj.Selected = true;
            }
            txtTrxDate.Text = TrxDate.ToString(Resources.Constants.DateFormatShort);
            txtDueDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            //ddlAlertType.Items[0].Text = Resources.Report.Select;
            if (ddlAlertType.Items.Count > 1)
            {
                ddlAlertType.SelectedValue = ddlAlertType.Items[1].Value;
                if (string.IsNullOrEmpty(txtAlertName.Text.Trim()))
                {
                    txtAlertName.Text = ddlAlertType.Items[1].Text;
                }
            }
            if (ddlNotifyBefore.Items.Count > 1)
            {
                ddlNotifyBefore.SelectedValue = ddlNotifyBefore.Items[1].Value;
            }

            GetFieldValues(ControlsEnum.NOTIFICATIONDAYS);
            SetFieldValues(ControlsEnum.NOTIFICATIONDAYS);
            vrfAlertType.Enabled = true;
            EntryStatus = EntryStatus.LISTMODE;

        }

        /// <summary>
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        private void SetUIEditView(ActionsEnum mode)
        {
            try
            {
                switch (mode)
                {
                }
            }
            catch
            {
                throw;
            }
        }

        public void GetAlertList()
        {
            ResetForm();
            lblTrxRef.Text = TypeRef;
            lblTrxType.Text = TypeText;
            GetFieldValues(ControlsEnum.ALERTLIST);
            SetFieldValues(ControlsEnum.ALERTLIST);
            txtTrxDate.Text = TrxDate.ToString(Resources.Constants.DateFormatShort);
            hdfTrxDate.Value = TrxDate.ToString();
            txtDueDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfDueDate.Value = DateTime.Now.ToString();
            txtNoOfDays.Text = (Math.Floor((DateTime.Now - TrxDate).TotalDays)).ToString();
            if (ddlAlertType.Items.Count > 1)
            {
                ddlAlertType.SelectedValue = ddlAlertType.Items[1].Value;
            }
            if (ddlNotifyBefore.Items.Count > 1)
            {
                ddlNotifyBefore.SelectedValue = ddlNotifyBefore.Items[1].Value;
            } 

        }

        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.ALERTDETAILS:
                        if (dsAlertList != null && dsAlertList.Tables[0].Rows.Count == 1)
                        {
                            DataTable dtAlert = dsAlertList.Tables[0];
                            txtAlertName.Text = HttpUtility.HtmlDecode(dtAlert.Rows[0]["ATH_NAME"].ToString());
                            txtTrxDate.Text = Convert.ToDateTime(dtAlert.Rows[0]["ATH_TRX_DATE"]).ToString(Resources.Constants.DateFormatShort);
                            txtNoOfDays.Text = dtAlert.Rows[0]["ATH_DUE_DAYS"].ToString();
                            txtDueDate.Text = Convert.ToDateTime(dtAlert.Rows[0]["ATH_DUE_DATE"]).ToString(Resources.Constants.DateFormatShort);
                            if (dtAlert.Rows[0]["ATH_ALERT_TYPE"] != null && !string.IsNullOrEmpty(dtAlert.Rows[0]["ATH_ALERT_TYPE"].ToString()))
                            {
                                ddlAlertType.SelectedValue = dtAlert.Rows[0]["ATH_ALERT_TYPE"].ToString();
                                vrfAlertType.Enabled = true;
                            }
                            else
                            {
                                ddlAlertType.SelectedValue = "-1";
                                ddlAlertType.Items[0].Text = dtAlert.Rows[0]["ATH_ALERT_TYPE_TEXT"].ToString(); ;
                                vrfAlertType.Enabled = false;
                            }
                            txtNotifyBefore.Text = dtAlert.Rows[0]["ATH_NOTIFY_BFR"].ToString();
                            if (dtAlert.Rows[0]["ATH_NOTIFY_BFR_UOM"] != null && !string.IsNullOrEmpty(dtAlert.Rows[0]["ATH_NOTIFY_BFR_UOM"].ToString()))
                                ddlNotifyBefore.SelectedValue = dtAlert.Rows[0]["ATH_NOTIFY_BFR_UOM"].ToString();
                            else
                                ddlNotifyBefore.SelectedValue = "-1";
                            txtRemarks.Text = HttpUtility.HtmlDecode(dtAlert.Rows[0]["ATH_REMARKS"].ToString());
                            chkNotifyAll.Checked = dtAlert.Rows[0]["ATH_NOTIFY_USER"] == null
                                || (dtAlert.Rows[0]["ATH_NOTIFY_USER"] != null && string.IsNullOrEmpty(dtAlert.Rows[0]["ATH_NOTIFY_USER"].ToString())) 
                                ? true : false;
                            chkCompleted.Checked = dtAlert.Rows[0]["ATH_STATUS"].ToString() == "0" ? false : true;
                            for (int i = 0; i < chkNotifyThrough.Items.Count; i++)
                            {
                                if (Convert.ToInt32(chkNotifyThrough.Items[i].Value) == (int)AlertNotificationTypesEnum.MESSAGE)
                                {
                                    if (dtAlert.Rows[0]["ATH_NOTIFY_MESSAGE"].ToString() == "0"
                                        || dtAlert.Rows[0]["ATH_NOTIFY_MESSAGE"].ToString().ToLower() == "False".ToLower())
                                        chkNotifyThrough.Items[i].Selected = false;
                                    else
                                        chkNotifyThrough.Items[i].Selected = true;
                                }
                                if (Convert.ToInt32(chkNotifyThrough.Items[i].Value) == (int)AlertNotificationTypesEnum.EMAIL)
                                {
                                    if (dtAlert.Rows[0]["ATH_NOTIFY_EMAIL"].ToString() == "0"
                                        || dtAlert.Rows[0]["ATH_NOTIFY_EMAIL"].ToString().ToLower() == "False".ToLower())
                                        chkNotifyThrough.Items[i].Selected = false;
                                    else
                                        chkNotifyThrough.Items[i].Selected = true;
                                }
                                if (Convert.ToInt32(chkNotifyThrough.Items[i].Value) == (int)AlertNotificationTypesEnum.SMS)
                                {
                                    if (dtAlert.Rows[0]["ATH_NOTIFY_SMS"].ToString() == "0"
                                        || dtAlert.Rows[0]["ATH_NOTIFY_SMS"].ToString().ToLower() == "False".ToLower())
                                        chkNotifyThrough.Items[i].Selected = false;
                                    else
                                        chkNotifyThrough.Items[i].Selected = true;
                                }
                            }
                            LastModifiedTime = Convert.ToDateTime(dtAlert.Rows[0]["LAST_MOD_DT"]);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            AlertBO alertBoObj = new AlertBO();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (mode)
                {
                    case ActionsEnum.ALERTSAVE:
                        alertBoObj.ATH_PK = CurrPK;
                        alertBoObj.ATH_NO = "";
                        alertBoObj.ATH_DATE = DateTime.Now;
                        alertBoObj.ATH_TRX_TYPE = TypeCode;
                        alertBoObj.ATH_TRX_PK = (int)TypePK;
                        alertBoObj.ATH_TRX_DATE = Convert.ToDateTime(txtTrxDate.Text.Trim());
                        alertBoObj.ATH_DUE_DAYS = string.IsNullOrEmpty(txtNoOfDays.Text.Trim()) ? Convert.ToInt16(0) : Convert.ToInt16(txtNoOfDays.Text.Trim());
                        alertBoObj.ATH_DUE_DATE = Convert.ToDateTime(txtDueDate.Text.Trim());
                        alertBoObj.ATH_NAME = HttpUtility.HtmlEncode(txtAlertName.Text.Trim());
                        if (Convert.ToInt32(ddlNotifyBefore.SelectedValue) > 0)
                            alertBoObj.ATH_BASIS = Convert.ToInt32(ddlNotifyBefore.SelectedValue);
                        if (Convert.ToInt32(ddlAlertType.SelectedValue) > 0)
                            alertBoObj.ATH_ALERT_TYPE = Convert.ToInt32(ddlAlertType.SelectedValue);
                        if (!string.IsNullOrEmpty(txtNotifyBefore.Text))
                            alertBoObj.ATH_NOTIFY_BFR = Convert.ToInt16(txtNotifyBefore.Text.Trim());
                        if (Convert.ToInt32(ddlNotifyBefore.SelectedValue) > 0)
                            alertBoObj.ATH_NOTIFY_BFR_UOM = Convert.ToInt32(ddlNotifyBefore.SelectedValue);
                        alertBoObj.ATH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        if (!string.IsNullOrEmpty(TypeRef) && !TypeRef.Equals("[New]".ToLower()))
                        {
                            alertBoObj.ATH_NARRATION = TypeRef.Trim();
                            if (!string.IsNullOrEmpty(TypePartyName))
                            {
                                alertBoObj.ATH_NARRATION = !string.IsNullOrEmpty(alertBoObj.ATH_NARRATION) ? alertBoObj.ATH_NARRATION + " - " + TypePartyName :
                                    TypePartyName;
                            }
                        }


                        foreach (ListItem chkObj in chkNotifyThrough.Items)
                        {
                            if (Convert.ToInt32(chkObj.Value) == (int)AlertNotificationTypesEnum.MESSAGE)
                            {
                                if (chkObj.Selected)
                                    alertBoObj.ATH_NOTIFY_MESSAGE = true;
                                else
                                    alertBoObj.ATH_NOTIFY_MESSAGE = false;
                            }
                            else if (Convert.ToInt32(chkObj.Value) == (int)AlertNotificationTypesEnum.EMAIL)
                            {
                                if (chkObj.Selected)
                                    alertBoObj.ATH_NOTIFY_EMAIL = true;
                                else
                                    alertBoObj.ATH_NOTIFY_EMAIL = false;
                            }
                            else if (Convert.ToInt32(chkObj.Value) == (int)AlertNotificationTypesEnum.SMS)
                            {
                                if (chkObj.Selected)
                                    alertBoObj.ATH_NOTIFY_SMS = true;
                                else
                                    alertBoObj.ATH_NOTIFY_SMS = false;
                            }
                        }

                        alertBoObj.ATH_NOTIFY_USER =chkNotifyAll.Checked?0:Convert.ToInt16(currentUser.PKUser);
                        alertBoObj.ATH_STATUS = chkCompleted.Checked ? (byte)1 : (byte)0;
                        alertBoObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        alertBoObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        alertBoObj.BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        alertBoObj.LAST_MOD_DT = LastModifiedTime;
                        
                        break;
                }
                returnObj = alertBoObj;
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
        /// Method for Grid binding
        /// </summary>
        private void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.ALERTLIST:
                        if (dsAlertList != null)
                        {
                            grdAlert.DataSource = dsAlertList.Tables[0].DefaultView;
                            grdAlert.DataBind();
                        }
                        break;
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
        private void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.ALERTTYPES:
                        ddlAlertType.Items.Clear();
                        if (admConstMstList != null && admConstMstList.Count > 0)
                        {
                            ddlAlertType.DataSource = admConstMstList;
                            ddlAlertType.DataTextField = Resources.DataFieldRes.ConstName;
                            ddlAlertType.DataValueField = Resources.DataFieldRes.ConstPK;
                            ddlAlertType.DataBind();
                        }
                        ddlAlertType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        if (ddlAlertType.Items.Count > 1)
                        {
                             ddlAlertType.SelectedValue= ddlAlertType.Items[1].Value;
                             if (string.IsNullOrEmpty(txtAlertName.Text.Trim()))
                             {
                                 txtAlertName.Text = ddlAlertType.Items[1].Text;
                             }
                        }
                        break;
                    case ControlsEnum.ALERTBASIS:
                        ddlNotifyBefore.Items.Clear();
                        if (admConfigMstList != null && admConfigMstList.Count>0)
                        {
                            ddlNotifyBefore.DataSource = admConfigMstList;
                            ddlNotifyBefore.DataTextField = Resources.DataFieldRes.cfgData;
                            ddlNotifyBefore.DataValueField = Resources.DataFieldRes.ConfigPK;
                            ddlNotifyBefore.DataBind();
                        }
                        ddlNotifyBefore.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        if (ddlNotifyBefore.Items.Count > 1)
                        {
                            ddlNotifyBefore.SelectedValue = ddlNotifyBefore.Items[1].Value;
                        }
                        break;
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
        private void BindCheckBoxList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.NOTIFICATIONTYPES:
                        chkNotifyThrough.Items.Clear();
                        if (admConfigMstList != null && admConfigMstList.Count > 0)
                        {
                            chkNotifyThrough.DataSource = admConfigMstList;
                            chkNotifyThrough.DataTextField = Resources.DataFieldRes.cfgData;
                            chkNotifyThrough.DataValueField = Resources.DataFieldRes.cfgValue;
                            chkNotifyThrough.DataBind();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ///// <summary>
        ///// Method for Grid binding
        ///// </summary>
        //private void BindTree(ControlsEnum controlType)
        //{
        //    try
        //    {
        //        switch (controlType)
        //        {

        //            case ControlsEnum.NOTIFICATIONTYPES:
        //                treeNotifyThrough.Nodes.Clear();
        //                TreeNode root;
        //                root = new TreeNode(GetLocalResourceObject("NotifyThrough").ToString(), "0");
        //                treeNotifyThrough.Nodes.Add(root);
        //                root.Collapse();
        //                if (admConfigMstList != null)
        //                {
        //                    TreeNode child;
        //                    root.ChildNodes.Clear();
        //                    foreach (ADM_CONFIG_MST obj in admConfigMstList)
        //                    {
        //                        child = new TreeNode();
        //                        child.ShowCheckBox = true;
        //                        child.Text = obj.CFG_DATA;
        //                        child.ToolTip = obj.CFG_DATA;
        //                        child.Value = obj.CFG_VALUE.ToString();
        //                        root.ChildNodes.Add(child);
        //                    }
        //                }
        //                else
        //                {
        //                    root.ChildNodes.Clear();
        //                }
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion
        #region Enum
        /// <summary>
        /// Controls Enum for the page
        /// </summary>
        private enum ControlsEnum
        {
            DEFAULT,
            NOTIFICATIONTYPES,
            ALERTTYPES,
            ALERTBASIS,
            ALERTLIST,
            ALERTDETAILS,
            NOTIFICATIONDAYS
        }
       

        private enum AlertNotificationTypesEnum
        {
            MESSAGE = 1,
            EMAIL = 1,
            SMS = 3
        }
        #endregion

    }
}