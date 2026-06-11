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

namespace HRMS.TaskTracker
{
    public partial class TaskCategory : System.Web.UI.Page
    {
        #region Variables and Properties
        private ActionsEnum commonActions;
        private BusinessObject.User currentUser;
        private TaskDetail TaskDtlObj;
        List<TaskDetail> TaskDtlList;
        private DataTable dtCategoryList;
        private DataSet dsCategoryDtl;
        int result = 0;
        private int wrkflag = 0;
        private DataTable dtGroup;

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
        /// To maintain and keep Task Details
        /// </summary>
        private List<TaskDetail> TaskInfoSession
        {
            get
            {
                return (List<TaskDetail>)Session[ERP.Utilities.SessionStrings.TaskInfoSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.TaskInfoSession] = value;
            }
        }

        /// <summary>
        /// Current Sl No.
        /// </summary>
        private int CurrSqNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrSqNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrSqNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrSqNo] = value;
            }
        }

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
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.CATEGORYLIST);
                    SetFieldValues(ControlsEnum.CATEGORYLIST);

                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = "TCI_SEQUENCE";
                    grdItemDetails.DataKeyNames = datakeyarray;
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
                    case ControlsEnum.CATEGORYLIST:
                        dtCategoryList = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetCategoryList(CurrPK, currentUser.SBUID, null);
                        dtGroup = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.TaskCategory, 3, Convert.ToInt32(DbActiveStatus.ACTIVE), Convert.ToInt32(currentUser.SBUID));
                        break;
                    #endregion
                    #region CATEGORYEDITINFO
                    case ControlsEnum.CATEGORYEDITINFO:
                        dsCategoryDtl = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetCategoryDetails(CurrPK);
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
                    #region FILLTASKGRID
                    case ControlsEnum.TASKDETAILS:
                        BindGrid(ControlsEnum.TASKDETAILS);
                        break;
                    #endregion
                    #region CATEGORYLSIT
                    case ControlsEnum.CATEGORYLIST:
                        BindDropDown(ControlsEnum.TASKGROUP);
                        BindGrid(ControlsEnum.CATEGORYLIST);
                        break;
                    #endregion
                    #region CATEGORYEDITINFO
                    case ControlsEnum.CATEGORYEDITINFO:
                        GetUIValuesFromObject(ControlsEnum.CATEGORYEDITINFO);
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
            TaskCategoryDetails taskCategoryObj;
            string xmlDoc;
            bool bIsChecked = false;
            try
            {
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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                switch (commonActions)
                {
                    #region Grid Item Selected
                    case ActionsEnum.ITEMSELECTED:
                        foreach (GridViewRow grdrow in grdCategory.Rows)
                        {
                            RadioButton rbtn;
                            int selectedCategoryPK;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedCategoryPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCategoryPK")).Value);
                                hdfSelectedItemPk.Value = selectedCategoryPK.ToString();
                            }
                        }
                        //SetResetColour
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                        break;
                    #endregion
                    #region ADDITEM
                    case ActionsEnum.ADDITEM:

                        TaskDtlList = TaskInfoSession;
                        TaskDtlList = (List<TaskDetail>)SetUIValuesToObject(ControlsEnum.TASKDETAILS);
                        if (TaskDtlList != null && TaskDtlList.Count > 0)
                        {
                            SetFieldValues(ControlsEnum.TASKDETAILS);
                        }
                        TaskInfoSession = TaskDtlList;
                        ResetForm(ControlsEnum.TASKDETAILS);

                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        ResetForm(ControlsEnum.TASKDETAILS);
                        TaskDtlList = TaskInfoSession;
                        if (TaskDtlList != null && TaskDtlList.Count > 0)
                        {
                            CurrSqNo = Convert.ToInt32(grdItemDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSqNo > 0)
                            {
                                TaskDtlObj = TaskDtlList.SingleOrDefault(row => CurrSqNo == row.TCI_SEQUENCE);
                                GetUIValuesFromObject(ControlsEnum.TASKDETAILS);
                            }
                        }
                        hdfTaskDtl.Value = CommonConstants.SELECT_VALUE_ONE;
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideItemDetails", "$(document).ready(function(){ShowHideTaskDetails(1);});", true);
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        // Not Completed
                        // TaskDtlList = TaskInfoSession;
                        //TaskDtlList = (List<TaskDetail>)SetUIValuesToObject(ControlsEnum.TASKDETAILS);
                        //if (TaskDtlList != null && TaskDtlList.Count > 0)
                        //{
                        //    SetFieldValues(ControlsEnum.TASKDETAILS);
                        //}
                        //TaskInfoSession = TaskDtlList;
                        //ResetForm(ControlsEnum.TASKDETAILS);

                        TaskDtlList = TaskInfoSession;
                        if (TaskDtlList != null && TaskDtlList.Count > 0)
                        {
                            CurrSqNo = Convert.ToInt32(grdItemDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSqNo > 0)
                            {
                                TaskDtlObj = TaskDtlList.SingleOrDefault(row => CurrSqNo == row.TCI_SEQUENCE);
                                TaskDtlList.Remove(TaskDtlObj);
                            }
                        }
                        TaskInfoSession = TaskDtlList;
                        SetFieldValues(ControlsEnum.TASKDETAILS);
                        ResetForm(ControlsEnum.TASKDETAILS);
                        break;
                    #endregion
                    #region CLEARITEM
                    case ActionsEnum.CLEARITEM:
                        ResetForm(ControlsEnum.TASKDETAILS);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        taskCategoryObj = (TaskCategoryDetails)SetUIValuesToObject(ControlsEnum.CATEGORYDETAILS);
                        xmlDoc = CommonFunctions.XmlSerialize<TaskCategoryDetails>(taskCategoryObj);
                        result = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.SaveTaskCategory(xmlDoc);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, "Category");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            ResetForm(ControlsEnum.CATEGORYDETAILS);
                            GetFieldValues(ControlsEnum.CATEGORYLIST);
                            SetFieldValues(ControlsEnum.CATEGORYLIST);
                            EntryStatus = EntryStatus.LISTMODE;
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
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("Task").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        wrkflag = 1;
                        taskCategoryObj = (TaskCategoryDetails)SetUIValuesToObject(ControlsEnum.CATEGORYDETAILS);
                        xmlDoc = CommonFunctions.XmlSerialize<TaskCategoryDetails>(taskCategoryObj);
                        result = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.SaveTaskCategory(xmlDoc);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, "Category");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            ResetForm(ControlsEnum.CATEGORYDETAILS);
                            GetFieldValues(ControlsEnum.CATEGORYLIST);
                            SetFieldValues(ControlsEnum.CATEGORYLIST);
                            EntryStatus = EntryStatus.LISTMODE;
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
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("Task").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        TaskInfoSession = new List<TaskDetail>();
                        EntryStatus = EntryStatus.NEWMODE;
                        hdfTaskDtl.Value = "0";
                        chkCategoryActive.Checked = true;
                        chkTaskActive.Checked = true;
                        break;
                    #endregion
                    #region EDIT
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdCategory.Rows)
                        {
                            RadioButton rbtn;
                            int selectedCategoryPK;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedCategoryPK = CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCategoryPK")).Value);
                                hdfSelectedItemPk.Value = selectedCategoryPK.ToString();
                            }
                        }
                        if (bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.CATEGORYEDITINFO);
                            SetFieldValues(ControlsEnum.CATEGORYEDITINFO);
                        }
                        else
                        {
                            litErrorMsg.Text = "Select a row to perform an action";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.CATEGORYDETAILS);
                        GetFieldValues(ControlsEnum.CATEGORYLIST);
                        SetFieldValues(ControlsEnum.CATEGORYLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region CATEGORYLIST
                    case ActionsEnum.CATEGORYLIST:
                        ResetForm(ControlsEnum.CATEGORYDETAILS);
                        GetFieldValues(ControlsEnum.CATEGORYLIST);
                        SetFieldValues(ControlsEnum.CATEGORYLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region CATEGORYDETAIL
                    case ActionsEnum.CATEGORYDETAIL:
                        foreach (GridViewRow grdrow in grdCategory.Rows)
                        {
                            RadioButton rbtn;
                            int selectedCategoryPK;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedCategoryPK = CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCategoryPK")).Value);
                                hdfSelectedItemPk.Value = selectedCategoryPK.ToString();
                            }
                        }
                        if (bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.CATEGORYEDITINFO);
                            SetFieldValues(ControlsEnum.CATEGORYEDITINFO);
                        }
                        else
                        {
                            litErrorMsg.Text = "Select a row to perform an action";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                else if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);

                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
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
        private object SetUIValuesToObject(ControlsEnum controltype)
        {
            object returnObj;
            returnObj = null;
            TaskCategoryDetails TaskCategoryObj = new TaskCategoryDetails();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            switch (controltype)
            {
                #region TASKCATEGORYDATA
                case ControlsEnum.CATEGORYDETAILS:
                    TaskCategoryObj.TCT_PK = CurrPK;
                    //TaskCategoryObj.TCT_CODE = 0;
                    TaskCategoryObj.TCT_NAME = HttpUtility.HtmlEncode(txtCategory.Text);
                    TaskCategoryObj.TCT_DESC = HttpUtility.HtmlEncode(txtCategoryDescription.Text);
                    TaskCategoryObj.BIZUNIT_PK = currentUser.SBUID;
                    TaskCategoryObj.ACTIVE = chkCategoryActive.Checked ? 1 : 0;
                    TaskCategoryObj.USER_PK = currentUser.PKUser;
                    TaskCategoryObj.WKF_FLAG = wrkflag;
                    TaskCategoryObj.LAST_MOD_DT = LastModifiedTime;
                    if (Convert.ToInt32(ddlGroup.SelectedValue)>0)
	                {
                        TaskCategoryObj.TCT_GROUP = ddlGroup.SelectedValue;
	                }
                    else
                    {
                        TaskCategoryObj.TCT_GROUP = null;// string.Empty;
                    }
                    
                    TaskCategoryObj.Detail = TaskInfoSession;
                    
                    returnObj = TaskCategoryObj;
                    break;
                #endregion
                #region TASK DETAILS
                case ControlsEnum.TASKDETAILS:
                    if (CurrSqNo != 0 && TaskDtlList != null)
                    {
                        TaskDtlObj = TaskDtlList.SingleOrDefault(itm => itm.TCI_SEQUENCE == CurrSqNo);
                        if (TaskDtlObj != null)
                        {
                            TaskDtlObj.TCI_PK = Convert.ToInt32(hdfTaskPK.Value);
                            TaskDtlObj.TCI_ITEM = HttpUtility.HtmlEncode(txtTaskName.Text);
                            TaskDtlObj.TCI_DESC = HttpUtility.HtmlEncode(txtTaskDesc.Text);
                            TaskDtlObj.TCI_DURATION = Convert.ToInt16(txtDuration.Text);
                            TaskDtlObj.TCI_SEQUENCE = Convert.ToInt16(CurrSqNo);
                            TaskDtlObj.TCI_ACTIVE = chkTaskActive.Checked ? (byte)1 : (byte)0;
                        }
                    }
                    else
                    {
                        int slno = 1;
                        if (TaskDtlList == null || TaskDtlList.Count == 0)
                        {
                            TaskDtlList = new List<TaskDetail>();
                            slno = 1;
                        }
                        else
                        {
                            slno = TaskDtlList.Max(itm => itm.TCI_SEQUENCE);
                            slno++;
                        }
                        TaskDtlObj = new TaskDetail();
                        TaskDtlObj.TCI_SEQUENCE = Convert.ToInt16(slno);
                        TaskDtlObj.TCI_ITEM = HttpUtility.HtmlEncode(txtTaskName.Text);
                        TaskDtlObj.TCI_DESC = HttpUtility.HtmlEncode(txtTaskDesc.Text);
                        TaskDtlObj.TCI_DURATION = GetShortValue(txtDuration.Text.Trim());
                        //Int16 duration;
                        //TaskDtlObj.TCI_DURATION = Int16.TryParse(txtDuration.Text.Trim(), out duration) ? duration : (Int16)0;
                        TaskDtlObj.TCI_ACTIVE = chkTaskActive.Checked ? (byte)1 : (byte)0;
                        TaskDtlList.Add(TaskDtlObj);
                    }
                    returnObj = TaskDtlList;
                    break;
                #endregion
            }
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
                #region TASKDETAILS
                case ControlsEnum.TASKDETAILS:
                    if (TaskDtlObj != null)
                    {
                        hdfTaskPK.Value = TaskDtlObj.TCI_PK.ToString();
                        txtTaskName.Text = HttpUtility.HtmlDecode(TaskDtlObj.TCI_ITEM.ToString());
                        txtTaskDesc.Text = HttpUtility.HtmlDecode(TaskDtlObj.TCI_DESC.ToString());
                        txtDuration.Text = TaskDtlObj.TCI_DURATION.ToString();
                        if (TaskDtlObj.TCI_ACTIVE == 1)
                            chkTaskActive.Checked = true;
                        else
                            chkTaskActive.Checked = false;                        
                    }
                    break;
                #endregion
                #region CATEGORYEDITINFO
                case ControlsEnum.CATEGORYEDITINFO:
                    if (dsCategoryDtl != null && dsCategoryDtl.Tables[0].Rows.Count > 0)
                    {
                        txtCategory.Text = HttpUtility.HtmlDecode(dsCategoryDtl.Tables[0].Rows[0]["TCT_NAME"].ToString());
                        txtCategoryDescription.Text = HttpUtility.HtmlDecode(dsCategoryDtl.Tables[0].Rows[0]["TCT_DESC"].ToString());
                        hdfStatus.Value = dsCategoryDtl.Tables[0].Rows[0]["TCT_STATUS"].ToString();
                        if (dsCategoryDtl.Tables[0].Rows[0]["TCT_ACTIVE"].ToString() == "1")
                            chkCategoryActive.Checked = true;
                        else
                            chkCategoryActive.Checked = false;
                        chkTaskActive.Checked = true;

                        ddlGroup.SelectedIndex = (dsCategoryDtl.Tables[0].Rows[0]["TCT_GROUP"] != null && dsCategoryDtl.Tables[0].Rows[0]["TCT_GROUP"].ToString() != string.Empty) ?ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(dsCategoryDtl.Tables[0].Rows[0]["TCT_GROUP"].ToString())):0;                       
                        if (dsCategoryDtl.Tables[1] != null && dsCategoryDtl.Tables[1].Rows.Count > 0)
                        {
                            TaskDtlList = dsCategoryDtl.Tables[1].ToList<TaskDetail>();
                            TaskInfoSession = TaskDtlList;
                            SetFieldValues(ControlsEnum.TASKDETAILS);
                        }
                    }
                    break;
                #endregion

            }
        }
        #endregion
        #region HelperMethods
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.TASKDETAILS:
                        if (TaskDtlList != null)
                        {
                            grdItemDetails.DataSource = TaskDtlList;
                            grdItemDetails.DataBind();
                        }
                        break;
                    case ControlsEnum.CATEGORYLIST:
                        if (dtCategoryList != null && dtCategoryList.Rows.Count > 0)
                        {
                            grdCategory.DataSource = dtCategoryList;
                            grdCategory.DataBind();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int16 GetShortValue(string str)
        {
            Int16 result = (Int16)0;
            Int16.TryParse(str, out result);
            return result;
        }

        public void ResetForm(ControlsEnum controltype)
        {
            switch (controltype)
            {
                #region RESET TASKDETAIL
                case ControlsEnum.TASKDETAILS:
                    CurrSqNo = 0;
                    txtTaskName.Text = string.Empty;
                    txtTaskDesc.Text = string.Empty;
                    txtDuration.Text = string.Empty;
                    chkTaskActive.Checked = true;                    
                    break;
                #endregion
                #region RESET CATEGORYDETAILS
                case ControlsEnum.CATEGORYDETAILS:
                    CurrPK = 0;
                    hdfSelectedItemPk.Value = "0";
                    hdfTaskDtl.Value = "0";
                    txtCategory.Text = string.Empty;
                    txtCategoryDescription.Text = string.Empty;
                    chkCategoryActive.Checked = false;
                    grdItemDetails.DataSource = null;
                    grdItemDetails.DataBind();                   
                    hdfStatus.Value = "0";
                    ResetForm(ControlsEnum.TASKDETAILS);
                    if (TaskInfoSession != null)
                        TaskInfoSession.Clear();
                    break;
                #endregion
            }
        }

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
        #endregion
        #region Enum
        /// <summary>
        /// Controls Enum for the page
        /// </summary>
        public enum ControlsEnum
        {
            CATEGORYDETAILS,
            CATEGORYLIST,
            TASKDETAILS,
            CATEGORYEDITINFO,
            TASKGROUP
        }
        #endregion
       
    }
}