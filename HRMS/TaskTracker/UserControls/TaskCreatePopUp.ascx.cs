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
    public partial class TaskCreatePopUp : System.Web.UI.UserControl
    {
        #region Event
        //public delegate void DelActionHandler(object sender, EventArgs e);
        public event EventHandler TaskSave;

        #endregion
        #region Variables and Properties
        private ActionsEnum commonActions;
        private BusinessObject.User currentUser;
        private RetValues retvalObj;

        private DataTable dtTaskCategory;
        private DataSet dsTaskDetails;

        private int wrkflag = 0;
        private int ParentPK = 0;
        private int _CurrentTask = 0;
        private DataTable dtGroup;
        private DataTable dtCategoryDtl;

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
        /// TaskPk from source page
        /// </summary>
        public int TaskPk
        {
            get
            {
                return Convert.ToInt32(hdfParentPK.Value);
            }
            set
            {
                hdfParentPK.Value = value.ToString();
            }
        }

        /// <summary>
        /// Task Type -Parent task or Subtask
        /// </summary>
        public int IsSubtask
        {
            get
            {
                return Convert.ToInt32(hdfSubTask.Value);
            }
            set
            {
                hdfSubTask.Value = value.ToString();
            }
        }

        /// <summary>
        /// Category of Task
        /// </summary>
        public int CategoryPk
        {
            get
            {
                return Convert.ToInt32(hdfCategoryPK.Value);
            }
            set
            {
                hdfCategoryPK.Value = value.ToString();
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

        //For Integration with source page
        public int PkId
        {
            get
            {
                return _CurrentTask;
            }
            set
            {
                _CurrentTask = value;
            }
        }

        /// <summary>
        /// Category of Task
        /// </summary>
        public string PopupHeader
        {
            get
            {
                return Session["PopHeader"].ToString();
            }
            set
            {
                Session["PopHeader"] = value;
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
                    case ControlsEnum.TASKCATEGORY:
                        int? group = GetNullableInt(ddlGroup.SelectedValue)>0?GetNullableInt(ddlGroup.SelectedValue):null;  // Convert.ToInt32(ddlGroup.SelectedValue) > 0 ? Convert.ToInt32(ddlGroup.SelectedValue) : (int?)null;
                        dtTaskCategory = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetCategoryList(CategoryPk, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE), group);
                        break;
                    #endregion
                    #region GETTASKDETAILS
                    case ControlsEnum.SHOWTASKDETAIL:
                        dsTaskDetails = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetTaskInfo(CurrPK, Convert.ToInt32(DbActiveStatus.HASPK), null, currentUser.SBUID, 0, 0, 1);
                        break;
                    #endregion
                    #region TASKGROUP
                    case ControlsEnum.TASKGROUP:
                        dtGroup = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.TaskCategory, 3, Convert.ToInt32(DbActiveStatus.ACTIVE), Convert.ToInt32(currentUser.SBUID));
                        break;
                    #endregion
                    #region TASKCATEGORYPOPUPDETAILS
                    case ControlsEnum.TASKCATEGORYPOPUPDETAILS:
                        int? category = GetNullableInt(ddlCategory.SelectedValue) > 0 ? GetNullableInt(ddlCategory.SelectedValue) : null;
                        if (category!=null)
                        {
                            dtCategoryDtl = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetCategoryDetails(category.Value).Tables[1];                            
                        }
                        else
                        {
                            dtCategoryDtl = null;
                        }                        
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
                    #region SETTASKCATEGORY
                    case ControlsEnum.TASKCATEGORY:
                        BindDropDown(ControlsEnum.TASKCATEGORY);
                        break;
                    #endregion
                    #region SET TASK INFO
                    case ControlsEnum.SHOWSUBTASK:
                        GetUIValuesFromObject(ControlsEnum.SHOWSUBTASK);
                        break;
                    case ControlsEnum.SHOWPARENTTASK:
                        GetUIValuesFromObject(ControlsEnum.SHOWPARENTTASK);
                        break;
                    #endregion
                    #region SETTASKCATEGORY
                    case ControlsEnum.TASKGROUP:
                        BindDropDown(ControlsEnum.TASKGROUP);
                        break;
                    #endregion
                    #region TASKCATEGORYPOPUPDETAILS
                    case ControlsEnum.TASKCATEGORYPOPUPDETAILS:
                        BindGrid(ControlsEnum.TASKCATEGORYPOPUPDETAILS);
                        break;
                    #endregion
                }
            }
            catch
            {
            }
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
                    //GetFieldValues(ControlsEnum.TASKCATEGORY);
                    //SetFieldValues(ControlsEnum.TASKCATEGORY);
                }
            }
            catch
            {
                throw;
            }
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeTaskCreatePopUpControlComponents", "$(document).ready(function(){TaskCreatePopUpInitComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
            TaskInfo taskinfoObj;
            string xmlDoc;
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlGroup")
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }                    
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
               
                switch (commonActions)
                {
                    #region DEFAULT
                    case ActionsEnum.DEFAULT:
                        CurrPK = TaskPk;
                        if (CurrPK == 0)
                        {
                            txtSubTaskDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                            txtSubTaskNo.Text = GetLocalResourceObject("DocNo").ToString();
                            txtSubTaskExpDate.Text = DateTime.Now.AddDays(1).ToString(Resources.Constants.HRMSDateFormatShort);
                            GetFieldValues(ControlsEnum.TASKGROUP);
                            SetFieldValues(ControlsEnum.TASKGROUP);
                            GetFieldValues(ControlsEnum.TASKCATEGORY);
                            SetFieldValues(ControlsEnum.TASKCATEGORY);
                        }
                        GetFieldValues(ControlsEnum.SHOWTASKDETAIL);                        
                        if (IsSubtask == 1)
                        {
                            CurrPK = 0;
                            SetFieldValues(ControlsEnum.SHOWPARENTTASK);
                        }
                        else
                        {                           
                            SetFieldValues(ControlsEnum.SHOWSUBTASK);
                        }
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (hdfSubTask.Value != "0")
                            ParentPK = Convert.ToInt32(hdfParentPK.Value);
                        else
                            ParentPK = 0;
                        taskinfoObj = (TaskInfo)SetUIValuesToObject(ActionsEnum.SAVE);
                        xmlDoc = CommonFunctions.XmlSerialize<TaskInfo>(taskinfoObj);
                        retvalObj = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.SaveTaskInfo(xmlDoc);
                        if (retvalObj.RetVal != -1)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("TaskNo") + " " + retvalObj.RetNumber + " ");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            if (hdfSubTask.Value != "0")
                            {
                                _CurrentTask = Convert.ToInt32(hdfParentPK.Value);
                            }
                            else
                            {
                                _CurrentTask = retvalObj.RetVal;
                            }
                            if (TaskSave != null)
                            {
                                TaskSave(this, EventArgs.Empty);
                            }
                            ResetForm();
                        }
                        else
                        {
                            if (retvalObj.RetVal == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','Edit Task','916','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Edit Task";                                
                            }
                            else if (retvalObj.RetVal == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','Edit Task','916','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Edit Task";
                            }
                            else if (retvalObj.RetVal == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','Edit Task','916','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Edit Task";
                            }
                            else if (retvalObj.RetVal == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + GetLocalResourceObject("RefNoExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','Edit Task','916','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Edit Task";
                            }
                            else if (retvalObj.RetVal == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','Edit Task','916','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Edit Task";
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("Task").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','Edit Task','916','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Edit Task";
                            }
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        wrkflag = 1;
                        if (hdfSubTask.Value != "0")
                            ParentPK = Convert.ToInt32(hdfParentPK.Value);
                        else
                            ParentPK = 0;
                        taskinfoObj = (TaskInfo)SetUIValuesToObject(ActionsEnum.SAVE);
                        xmlDoc = CommonFunctions.XmlSerialize<TaskInfo>(taskinfoObj);
                        retvalObj = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.SaveTaskInfo(xmlDoc);
                        if (retvalObj.RetVal != -1)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("TaskNo") + " " + retvalObj.RetNumber + " ");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            if (hdfSubTask.Value != "0")
                            {
                                _CurrentTask = Convert.ToInt32(hdfParentPK.Value);
                            }
                            else
                            {
                                _CurrentTask = retvalObj.RetVal;
                            }
                            if (TaskSave != null)
                            {
                                TaskSave(this, EventArgs.Empty);
                            }
                            ResetForm();
                        }
                        else
                        {
                            if (retvalObj.RetVal == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divPopUp]','Create Task','700','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Create Task";
                            }
                            else if (retvalObj.RetVal == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divPopUp]','Create Task','700','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Create Task";
                            }
                            else if (retvalObj.RetVal == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divPopUp]','Create Task','700','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Create Task";
                            }
                            else if (retvalObj.RetVal == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + GetLocalResourceObject("RefNoExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divPopUp]','Create Task','700','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Create Task";
                            }
                            else if (retvalObj.RetVal == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divPopUp]','Create Task','700','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Create Task";
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("Task").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divPopUp]','Create Task','700','400');", true);
                                hdfShowContainerDiv.Value = PopupHeader = "Create Task";
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
                    #region CLOSE POP UP
                    case ActionsEnum.CANCELPOPUP:
                        ResetForm();
                        break;
                    #endregion
                    #region CHANGE
                    case ActionsEnum.CHANGE:
                        if (((DropDownList)sender).ID == "ddlGroup")
                        {
                            GetFieldValues(ControlsEnum.TASKCATEGORY);
                            SetFieldValues(ControlsEnum.TASKCATEGORY);
                           // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','Edit Task','916','400');", true);
                            if (hdfShowContainerDiv.Value == string.Empty)
                            {
                                hdfShowContainerDiv.Value = PopupHeader;
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','" + hdfShowContainerDiv.Value + "','916','400');", true);                        
                        }                       
                        break;
                    #endregion
                    case ActionsEnum.DETAILS:
                        GetFieldValues(ControlsEnum.TASKCATEGORYPOPUPDETAILS);
                        SetFieldValues(ControlsEnum.TASKCATEGORYPOPUPDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divCategoryDetails]','Category Details','700','400');", true);                       
                        break;
                    case ActionsEnum.SHOW:
                        grdItemDetails.DataSource = null;
                        grdItemDetails.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','" + PopupHeader + "','916','400');", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
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
            TaskInfo TaskBoObj = new TaskInfo();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            switch (mode)
            {
                #region TaskInfoData
                case ActionsEnum.SAVE:
                    TaskBoObj.TSK_PK = CurrPK;
                    TaskBoObj.TSK_PARENT = ParentPK.ToString();
                    TaskBoObj.TSK_DATE = txtSubTaskDate.Text;
                    TaskBoObj.TSK_NO = HttpUtility.HtmlEncode(txtSubTaskNo.Text);
                    TaskBoObj.TSK_EXP_DATE = txtSubTaskExpDate.Text;
                    TaskBoObj.TSK_CATEGORY = ddlCategory.SelectedValue;
                    TaskBoObj.TSK_NAME = HttpUtility.HtmlEncode(txtName.Text);
                    TaskBoObj.TSK_DESC = HttpUtility.HtmlEncode(txtDescription.Text);
                    TaskBoObj.TSK_ASSIGN_TO = hdfUserPK.Value.ToString();
                    TaskBoObj.BIZUNIT_PK = currentUser.SBUID;
                    TaskBoObj.ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                    TaskBoObj.USER_PK = currentUser.PKUser;
                    TaskBoObj.LAST_MOD_DT = String.Format("{0:G}", LastModifiedTime);
                    TaskBoObj.WKF_FLAG = wrkflag;
                    TaskBoObj.TSK_CATEGORY_GROUP = Convert.ToInt32(ddlGroup.SelectedValue);
                    break;
                #endregion
            }
            returnObj = TaskBoObj;
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
                case ControlsEnum.SHOWPARENTTASK:
                    if (dsTaskDetails.Tables[0] != null && dsTaskDetails.Tables[0].Rows.Count > 0)
                    {
                        MainTask.Visible = true;
                        lbShowMainTaskExpDate.Text = (dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"] == null && dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"].ToString() == string.Empty) ? string.Empty : (Convert.ToDateTime(dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"])).ToString(Resources.Constants.HRMSDateFormatShort);
                        lbShowTaskName.Text = ERP.Utilities.CommonFunctions.GetShortString(dsTaskDetails.Tables[0].Rows[0]["TSK_NAME"].ToString(), 65);
                        lbShowTaskName.ToolTip = dsTaskDetails.Tables[0].Rows[0]["TSK_NAME"].ToString();
                        lbShowTaskNo.Text = dsTaskDetails.Tables[0].Rows[0]["TSK_NO"].ToString();
                        lbShowStatus.Text = ERP.Utilities.CommonFunctions.GetShortString(dsTaskDetails.Tables[0].Rows[0]["TSK_TRX_STATUS_TEXT"].ToString(), 20);
                        lbShowStatus.ToolTip = dsTaskDetails.Tables[0].Rows[0]["TSK_TRX_STATUS_TEXT"].ToString();
                        lbShowTaskDate.Text = (dsTaskDetails.Tables[0].Rows[0]["TSK_DATE"] == null && dsTaskDetails.Tables[0].Rows[0]["TSK_DATE"].ToString() == string.Empty) ? string.Empty : (Convert.ToDateTime(dsTaskDetails.Tables[0].Rows[0]["TSK_DATE"])).ToString(Resources.Constants.HRMSDateFormatShort);
                        txtSubTaskDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                        //txtSubTaskExpDate.Text = (dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"] == null && dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"].ToString() == string.Empty) ? string.Empty:(Convert.ToDateTime(dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"])).ToString(CommonConstants.DATEFORMAT);
                        txtSubTaskExpDate.Text = (dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"] != null && dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"].ToString() != string.Empty) ?
                           (Convert.ToDateTime(dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"])).ToString(Resources.Constants.HRMSDateFormatShort) : DateTime.Now.AddDays(1).ToString(Resources.Constants.HRMSDateFormatShort);                        
                        txtName.Text = dsTaskDetails.Tables[0].Rows[0]["TSK_NAME"].ToString();
                        if (IsSubtask == 0)
                            hdfTaskMode.Value = dsTaskDetails.Tables[0].Rows[0]["TSK_STATUS"].ToString();                        
                        LastModifiedTime = Convert.ToDateTime(dsTaskDetails.Tables[0].Rows[0]["TSK_MOD_DT"]);
                        hdfCategoryPK.Value = string.IsNullOrEmpty(dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY"].ToString()) ? "0" : dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY"].ToString();
                        GetFieldValues(ControlsEnum.TASKCATEGORY);
                        SetFieldValues(ControlsEnum.TASKCATEGORY);
                        GetFieldValues(ControlsEnum.TASKGROUP);
                        SetFieldValues(ControlsEnum.TASKGROUP);
                        ddlGroup.SelectedIndex = string.IsNullOrEmpty(dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY_GROUP"].ToString()) ? 0 : ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY_GROUP"].ToString()));
                        ActionHandler(ddlGroup, EventArgs.Empty);
                        ddlCategory.SelectedIndex = string.IsNullOrEmpty(dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY"].ToString()) ? 0 : ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY"].ToString()));
                    }
                    break;

                case ControlsEnum.SHOWSUBTASK:
                    if (dsTaskDetails.Tables[3] != null && dsTaskDetails.Tables[3].Rows.Count > 0)
                    {
                        MainTask.Visible = true;
                        lbShowMainTaskExpDate.Text = (dsTaskDetails.Tables[3].Rows[0]["TSK_EXP_DATE"] == null && dsTaskDetails.Tables[3].Rows[0]["TSK_EXP_DATE"].ToString() == string.Empty) ? string.Empty : (Convert.ToDateTime(dsTaskDetails.Tables[3].Rows[0]["TSK_EXP_DATE"])).ToString(Resources.Constants.HRMSDateFormatShort);
                        lbShowTaskName.Text = ERP.Utilities.CommonFunctions.GetShortString(dsTaskDetails.Tables[3].Rows[0]["TSK_NAME"].ToString(), 65);
                        lbShowTaskName.ToolTip = dsTaskDetails.Tables[3].Rows[0]["TSK_NAME"].ToString();
                        lbShowTaskNo.Text = dsTaskDetails.Tables[3].Rows[0]["TSK_NO"].ToString();
                        lbShowStatus.Text = ERP.Utilities.CommonFunctions.GetShortString(dsTaskDetails.Tables[3].Rows[0]["TSK_TRX_STATUS_TEXT"].ToString(), 20);
                        lbShowStatus.ToolTip = dsTaskDetails.Tables[3].Rows[0]["TSK_TRX_STATUS_TEXT"].ToString();
                        lbShowTaskDate.Text = (dsTaskDetails.Tables[3].Rows[0]["TSK_DATE"] == null && dsTaskDetails.Tables[3].Rows[0]["TSK_DATE"].ToString() == string.Empty) ? string.Empty : (Convert.ToDateTime(dsTaskDetails.Tables[3].Rows[0]["TSK_DATE"])).ToString(Resources.Constants.HRMSDateFormatShort);
                    }
                    else
                    {
                        MainTask.Visible = false;
                    }

                    if (dsTaskDetails.Tables[0] != null && dsTaskDetails.Tables[0].Rows.Count > 0)
                    {
                        txtSubTaskDate.Text = (dsTaskDetails.Tables[0].Rows[0]["TSK_DATE"] == null && dsTaskDetails.Tables[0].Rows[0]["TSK_DATE"].ToString() == string.Empty) ? string.Empty : (Convert.ToDateTime(dsTaskDetails.Tables[0].Rows[0]["TSK_DATE"])).ToString(Resources.Constants.HRMSDateFormatShort);
                        txtSubTaskNo.Text = string.IsNullOrEmpty(dsTaskDetails.Tables[0].Rows[0]["TSK_NO"].ToString()) ? "[New]" : dsTaskDetails.Tables[0].Rows[0]["TSK_NO"].ToString();                        
                        txtSubTaskExpDate.Text = (dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"] != null && dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"].ToString() != string.Empty) ?
                            (Convert.ToDateTime(dsTaskDetails.Tables[0].Rows[0]["TSK_EXP_DATE"])).ToString(Resources.Constants.HRMSDateFormatShort) : DateTime.Now.AddDays(1).ToString(Resources.Constants.HRMSDateFormatShort);
                        txtName.Text = dsTaskDetails.Tables[0].Rows[0]["TSK_NAME"].ToString();
                        txtDescription.Text = dsTaskDetails.Tables[0].Rows[0]["TSK_DESC"].ToString();
                        txtUser.Text = dsTaskDetails.Tables[0].Rows[0]["TSK_ASSIGN_TO_TEXT"].ToString();
                        hdfUserPK.Value = dsTaskDetails.Tables[0].Rows[0]["TSK_ASSIGN_TO"].ToString();
                        hdfTaskMode.Value = dsTaskDetails.Tables[0].Rows[0]["TSK_STATUS"].ToString();
                        hdfCategoryPK.Value = string.IsNullOrEmpty(dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY"].ToString()) ? "0" : dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY"].ToString();
                        GetFieldValues(ControlsEnum.TASKGROUP);
                        SetFieldValues(ControlsEnum.TASKGROUP);
                        GetFieldValues(ControlsEnum.TASKCATEGORY);
                        SetFieldValues(ControlsEnum.TASKCATEGORY);
                        ddlGroup.SelectedIndex = string.IsNullOrEmpty(dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY_GROUP"].ToString()) ? 0 : ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY_GROUP"].ToString()));
                        ActionHandler(ddlGroup, EventArgs.Empty);
                        ddlCategory.SelectedIndex = string.IsNullOrEmpty(dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY"].ToString()) ? 0 : ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(dsTaskDetails.Tables[0].Rows[0]["TSK_CATEGORY"].ToString()));
                     }
                    break;
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
                    case ControlsEnum.TASKCATEGORY:
                        ddlCategory.Items.Clear();
                        if (dtTaskCategory != null && dtTaskCategory.Rows.Count > 0)
                        {
                            ddlCategory.DataSource = dtTaskCategory;
                            ddlCategory.DataTextField = GetLocalResourceObject("CategoryText").ToString();
                            ddlCategory.DataValueField = GetLocalResourceObject("CategoryValue").ToString();
                            ddlCategory.DataBind();
                        }
                        //ddlCategory.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        break;
                    #endregion
                    #region TASKGROUP
                    case ControlsEnum.TASKGROUP:
                        ddlGroup.Items.Clear();
                        if (dtGroup != null && dtGroup.Rows.Count > 0)
                        {
                            ddlGroup.DataSource = dtGroup;
                            ddlGroup.DataTextField = GetLocalResourceObject("CON_NAME").ToString();
                            ddlGroup.DataValueField = GetLocalResourceObject("CON_PK").ToString();
                            ddlGroup.DataBind();
                        }
                        ddlGroup.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
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
            CurrPK = 0;
            txtDescription.Text = string.Empty;
            lbShowMainTaskExpDate.Text = string.Empty;
            txtName.Text = string.Empty;
            lbShowStatus.Text = string.Empty;
            txtSubTaskDate.Text = string.Empty;
            txtSubTaskExpDate.Text = DateTime.Now.AddDays(1).ToString(Resources.Constants.HRMSDateFormatShort);
            txtSubTaskNo.Text = string.Empty;
            lbShowTaskDate.Text = string.Empty;
            txtUser.Text = string.Empty;
            //hdfUserPK.Value = null;
            lbShowTaskNo.Text = string.Empty;
            lbShowTaskName.Text = string.Empty;
            //ddlCategory.SelectedIndex = 0;
            txtSubTaskDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
            txtSubTaskNo.Text = GetLocalResourceObject("DocNo").ToString();
            hdfCategoryPK.Value = "0";
            hdfTaskMode.Value = "0";
            hdfUserPK.Value = "0";
            hdfSubTask.Value = "0";
            hdfParentPK.Value = "0";

        }

        private int? GetNullableInt(string str)
        {
            int result;
            if (int.TryParse(str, out result))
            {
                return (int?)result;
            }
            return null;
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
                    case ControlsEnum.TASKCATEGORYPOPUPDETAILS:
                         grdItemDetails.DataSource = null;
                         grdItemDetails.DataBind();
                        if (dtCategoryDtl != null)
                        {
                            var t = dtCategoryDtl.AsEnumerable()
                                .Where(a => a.Field<byte>("TCI_ACTIVE") == 1)
                                .Select(s=> new
                                {
                                   TCI_ITEM= s.Field<string>("TCI_ITEM"),
                                   TCI_DESC=s.Field<string>("TCI_DESC"),
                                   TCI_DURATION=s.Field<Int16>("TCI_DURATION"),
                                   TCI_SEQUENCE = s.Field<Int16>("TCI_SEQUENCE")
                                })
                                .ToList();
                            grdItemDetails.DataSource = t;
                            grdItemDetails.DataBind();            
                        }
                        else
                        {
                            grdItemDetails.DataSource = null;
                            grdItemDetails.DataBind();
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
        #region Enum
        /// <summary>
        /// Controls Enum for the page
        /// </summary>
        public enum ControlsEnum
        {
            TASKCATEGORY,
            ASSIGNTASKUSER,
            SHOWTASKDETAIL,
            SHOWPARENTTASK,
            SHOWSUBTASK,
            TASKGROUP,
            TASKCATEGORYPOPUPDETAILS
        }
        #endregion
    }
}