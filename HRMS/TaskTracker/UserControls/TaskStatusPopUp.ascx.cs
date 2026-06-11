using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ERPData;
using ERP.Utilities;
using ERPService;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessLogic.HRMS.TaskTracker;
using BusinessObject.HRMS.TaskTracker;

namespace HRMS.TaskTracker.UserControls
{
    public partial class TaskStatusPopUp : System.Web.UI.UserControl
    {
        #region Event
        public event EventHandler StatusUpdate;
        #endregion
        #region Variables and Properties
        private BusinessObject.User currentUser;
        private ActionsEnum commonActions;
        private DataTable dtStatus;
        private DataSet dsTaskDetails;

        #region Properties
        /// <summary>
        /// Current Pk of Task
        /// </summary>
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
        /// TaskPk from source page
        /// </summary>
        public int TaskPk
        {
            get
            {
                return Convert.ToInt32(hdfTaskPK.Value);
            }
            set
            {
                hdfTaskPK.Value = value.ToString();
            }
        }

        #endregion
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
        #region PageActionHandler
        /// <summary>
        /// Handles page load
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    txtStatusDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    divExpDate.Visible = true;
                    GetFieldValues(ControlsEnum.TASKSTATUS);
                    SetFieldValues(ControlsEnum.TASKSTATUS);
                }
            }
            catch
            {
                throw;
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region GETTASKCATEGORY
                    case ControlsEnum.TASKSTATUS:
                        dtStatus = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.TaskCategory, 2, Convert.ToInt32(DbActiveStatus.ACTIVE), Convert.ToInt32(currentUser.SBUID));
                        break;
                    #endregion
                    #region GETTASKDETAILS
                    case ControlsEnum.SHOWTASKDETAIL:
                        dsTaskDetails = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetTaskInfo(TaskPk, Convert.ToInt32(DbActiveStatus.HASPK), null, currentUser.SBUID, 0, 0, 1);
                        break;
                    #endregion                   
                }
            }
            catch
            {
                throw;
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
                    #region SETTASKSTATUSDROPDOWN
                    case ControlsEnum.TASKSTATUS:
                        BindDropDown(ControlsEnum.TASKSTATUS);
                        break;
                    #endregion  
                    #region SET TASK INFO
                    case ControlsEnum.SHOWTASKDETAIL:                           
                        GetUIValuesFromObject(ControlsEnum.SHOWTASKDETAIL);
                        break;
                    #endregion
                }
            }
            catch
            {
            }
        }
        #endregion
        #region Action Handler
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActionHandler(object sender, EventArgs e)
        {
            TaskStatus taskStatusObj;
            string xmlDoc;
            int result;
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlStatus")
                    {
                        commonActions = ActionsEnum.CHANGESTATUS;
                    }
                }
                switch (commonActions)
                {
                    #region DEFAULT
                    case ActionsEnum.DEFAULT:
                        GetFieldValues(ControlsEnum.SHOWTASKDETAIL);
                        SetFieldValues(ControlsEnum.SHOWTASKDETAIL);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        taskStatusObj=(TaskStatus)SetUIValuesToObject(ActionsEnum.SAVE);
                        xmlDoc=CommonFunctions.XmlSerialize<TaskStatus>(taskStatusObj);
                        result = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.UpdateStatus(xmlDoc);
                        if (result > 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("StatusUpdate").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            TaskPk = Convert.ToInt32(hdfTaskPK.Value);
                            if (StatusUpdate != null)
                            {
                                StatusUpdate(this, EventArgs.Empty);
                            }
                            ResetForm();
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpStatus]','Update Status','916','350');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpStatus]','Update Status','916','350');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpStatus]','Update Status','916','350');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpStatus]','Update Status','916','350');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("Task").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpStatus]','Update Status','916','350');", true);
                            }                      
                        }
                        break;
                    #endregion                   
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                        break;
                    #endregion
                    #region StatusChange
                    case ActionsEnum.CHANGESTATUS:
                        if (ddlStatus.SelectedValue == "2")
                        {
                            divExpDate.Visible = false;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpStatus]','Update Status','916','350');", true);
                        }
                        else
                        {
                            divExpDate.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpStatus]','Update Status','916','350');", true);
                        }
                        break;
                    #endregion
                    #region CLOSE POP UP
                    case ActionsEnum.CANCELPOPUP:
                        ResetForm();
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region HelperMethods
        /// <summary>
        /// Method for DropDown binding
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region BIND TASK DDL
                    case ControlsEnum.TASKSTATUS:
                        ddlStatus.Items.Clear();
                        if (dtStatus != null && dtStatus.Rows.Count > 0)
                        {
                            ddlStatus.DataSource = dtStatus;
                            ddlStatus.DataTextField = GetLocalResourceObject("CategoryText").ToString();
                            ddlStatus.DataValueField = GetLocalResourceObject("CategoryValue").ToString();
                            ddlStatus.DataBind();
                        }
                        ddlStatus.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        break;
                    #endregion
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Reset Form data
        /// </summary>
        public void ResetForm()
        {
            txtExpDate.Text = DateTime.Now.AddDays(1).ToString(Resources.Constants.HRMSDateFormatShort);
            txtRemarks.Text = string.Empty;
            txtStatusDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
            ddlStatus.SelectedValue = CommonConstants.SELECTVAL;
            divExpDate.Visible = true;
            ddlStatus.SelectedValue = CommonConstants.SELECTVAL;
        }
       
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeTaskStatusPopUpControlComponents", "$(document).ready(function(){TaskStatusPopUpInitComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            TaskStatus TaskStatusObj = new TaskStatus();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            switch (mode)
            {
                #region TaskInfoData
                case ActionsEnum.SAVE:
                    TaskStatusObj.TKS_PK = CurrPK;
                    TaskStatusObj.TKS_TASK = hdfTaskPK.Value;
                    TaskStatusObj.TKS_DATE = txtStatusDate.Text;
                    TaskStatusObj.TKS_TRX_STATUS = ddlStatus.SelectedValue;
                    TaskStatusObj.TKS_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                    TaskStatusObj.TKS_EXP_DATE = txtExpDate.Text;
                    //TaskStatusObj.TKS_STATUS = HttpUtility.HtmlEncode(txtName.Text);
                    TaskStatusObj.BIZUNIT_PK = currentUser.SBUID.ToString();
                    TaskStatusObj.ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE).ToString();
                    TaskStatusObj.USER_PK = currentUser.PKUser.ToString();
                    //TaskStatusObj.LAST_MOD_DT = LastModifiedTime.ToString();                  
                    break;
                #endregion
            }
            returnObj = TaskStatusObj;
            return returnObj;
        }
        #endregion
        #region GetUIValuesFromObject
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.SHOWTASKDETAIL:                    
                    if (dsTaskDetails.Tables[0] != null && dsTaskDetails.Tables[0].Rows.Count > 0)      
                    {
                        lbShowTskExpdt.Text = (Convert.ToDateTime(dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"])).ToString(Resources.Constants.HRMSDateFormatShort);
                        lbShowTaskName.Text = ERP.Utilities.CommonFunctions.GetShortString(dsTaskDetails.Tables[0].Rows[0]["TSK_NAME"].ToString(),65);
                        lbShowTaskName.ToolTip = dsTaskDetails.Tables[0].Rows[0]["TSK_NAME"].ToString();
                        lbShowTaskNo.Text = dsTaskDetails.Tables[0].Rows[0]["TSK_NO"].ToString();
                        lbShowTskStatus.Text = ERP.Utilities.CommonFunctions.GetShortString(dsTaskDetails.Tables[0].Rows[0]["TSK_TRX_STATUS_TEXT"].ToString(),21);
                        lbShowTskStatus.ToolTip = dsTaskDetails.Tables[0].Rows[0]["TSK_TRX_STATUS_TEXT"].ToString();
                        lbShowTaskDate.Text = (Convert.ToDateTime(dsTaskDetails.Tables[0].Rows[0]["TSK_DATE"])).ToString(Resources.Constants.HRMSDateFormatShort);
                        //ddlStatus.SelectedValue = (dsTaskDetails.Tables[0].Rows[0]["TSK_NEXT_STATUS"]==null && dsTaskDetails.Tables[0].Rows[0]["TSK_NEXT_STATUS"].ToString()==string.Empty)?CommonConstants.SELECTVAL : dsTaskDetails.Tables[0].Rows[0]["TSK_NEXT_STATUS"].ToString();
                        ddlStatus.SelectedIndex = (dsTaskDetails.Tables[0].Rows[0]["TSK_NEXT_STATUS"] == null && dsTaskDetails.Tables[0].Rows[0]["TSK_NEXT_STATUS"].ToString() == string.Empty) ? 0 : ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(dsTaskDetails.Tables[0].Rows[0]["TSK_NEXT_STATUS"].ToString()));
                        txtExpDate.Text = dsTaskDetails.Tables[0].Rows[0]["TSK_EST_DATE"] != null && dsTaskDetails.Tables[0].Rows[0]["TSK_EST_DATE"].ToString() != string.Empty ?
                            (Convert.ToDateTime(dsTaskDetails.Tables[0].Rows[0]["TSK_EST_DATE"])).ToString(Resources.Constants.HRMSDateFormatShort) : (dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"] != null || dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"].ToString() != string.Empty ?
                            (Convert.ToDateTime(dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"])).ToString(Resources.Constants.HRMSDateFormatShort) : DateTime.Now.AddDays(1).ToString(Resources.Constants.HRMSDateFormatShort));
                    }
                    break;               
            }
        }
        #endregion
        #region Enum
        /// <summary>
        /// Controls Enum for the page
        /// </summary>
        private enum ControlsEnum
        {
            TASKSTATUS,
            SHOWTASKDETAIL,
            STATUSSELECTED
        }
        #endregion
    }
}