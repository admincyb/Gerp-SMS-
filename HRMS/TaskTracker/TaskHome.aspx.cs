using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.HRMS.TaskTracker;
using ERP.Utilities;
using System.Data;
using BusinessObject.AccountManagement;
using ERPSMS_v01.UserControls;
using BusinessObject.Common;
using BusinessObject.CommonManagement;


namespace HRMS.TaskTracker
{
    public partial class TaskHome : System.Web.UI.Page
    {
        #region "Variables And Properties"
        #region "Properties"
        public List<BreadCrumb> BreadCrumbProp
        {
            get
            {
                if (ViewState["BreadCrumb"] != null) return (List<BreadCrumb>)ViewState["BreadCrumb"];
                return null;
            }
            set
            { ViewState["BreadCrumb"] = value; }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndexMyTask
        {
            get
            {
                return (string)this.ViewState["PageIndexMyTask"];
            }
            set
            {
                this.ViewState["PageIndexMyTask"] = value;
            }
        }
        private string PageIndexPendingTask
        {
            get
            {
                return (string)this.ViewState["PageIndexPendingTask"];
            }
            set
            {
                this.ViewState["PageIndexPendingTask"] = value;
            }
        }
        private string PageIndexCompletedTask
        {
            get
            {
                return (string)this.ViewState["PageIndexCompletedTask"];
            }
            set
            {
                this.ViewState["PageIndexCompletedTask"] = value;
            }
        }
        public int? AssignTo
        {
            get
            {
                if (ViewState["AssignTo"] != null) return (int?)ViewState["AssignTo"];
                return null;
            }
            set
            { ViewState["AssignTo"] = value; }
        }
        public int? AssignBy
        {
            get
            {
                if (ViewState["AssignBy"] != null) return (int?)ViewState["AssignBy"];
                return null;
            }
            set
            { ViewState["AssignBy"] = value; }
        }

        public int? IsCompleted
        {
            get
            {
                if (ViewState["IsCompleted"] != null) return (int?)ViewState["IsCompleted"];
                return null;
            }
            set
            { ViewState["IsCompleted"] = value; }
        }
        //  private int? isCompleted;

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

        #endregion "Properties"

        private ActionsEnum commonActions;
        private BusinessObject.User currentUser;
        private DataTable dtTasks;
        private DataTable dtHistory;
        DataSet dsMainTask;
        private int resultDel;

        //private int? isCompleted;
        //private int? assignTo;
        //private int? assignBy;

        #endregion "Variables And Properties"

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                InitializeComponent();
                if (!IsPostBack)
                {
                    uclPagingMyTask.CurrentPage = 1;
                    uclPagingPendingTask.CurrentPage = 1;
                    uclPagingCompletedTask.CurrentPage = 1;


                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    hdfCurrentUserPK.Value = currentUser.PKUser.ToString();
                    if (Request.QueryString["Action"] != null)
                    {
                        GetFieldValues(ControlsEnum.PENDINGWORKS);
                        SetFieldValues(ControlsEnum.PENDINGWORKS);
                        Button dummyBtn = new Button();
                        dummyBtn.CommandName = ActionsEnum.NEWTASKLISTPAGE.ToString();
                        ActionHandler(dummyBtn, EventArgs.Empty);
                    }
                    else if (Request.QueryString["TabID"] != null)
                    {
                        switch (Request.QueryString[QueryStrings.TabID])
                        {
                            case "ByMe":
                                GetFieldValues(ControlsEnum.MYTASKS);
                                SetFieldValues(ControlsEnum.MYTASKS);
                                break;
                            case "ForMe":
                                GetFieldValues(ControlsEnum.PENDINGWORKS);
                                SetFieldValues(ControlsEnum.PENDINGWORKS);
                                break;
                            case "Completed":
                                GetFieldValues(ControlsEnum.COMPLETEDTASKS);
                                SetFieldValues(ControlsEnum.COMPLETEDTASKS);
                                break;
                            default:
                                GetFieldValues(ControlsEnum.PENDINGWORKS);
                                SetFieldValues(ControlsEnum.PENDINGWORKS);
                                break;
                        }
                    }
                    else
                    {
                        GetFieldValues(ControlsEnum.PENDINGWORKS);
                        SetFieldValues(ControlsEnum.PENDINGWORKS);
                    }


                    BreadCrumbProp = new List<BreadCrumb>
                                {
                                     new BreadCrumb { Order = 1, TaskId = 0, Name =  GetLocalResourceObject("PendingWorks").ToString()}
                                };

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }
        #endregion

        #region PreRender
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeControlComponents", "$(document).ready(function(){InitComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
                    case ControlsEnum.MYTASKS:
                        BindGrid(ControlsEnum.MYTASKS);
                        EnableListPage();
                        ActivateMyTaskListingPage();
                        break;
                    case ControlsEnum.PENDINGWORKS:
                        BindGrid(ControlsEnum.PENDINGWORKS);
                        EnableListPage();
                        ActivatePendingWorksListingPage();
                        break;
                    case ControlsEnum.COMPLETEDTASKS:
                        BindGrid(ControlsEnum.COMPLETEDTASKS);
                        EnableListPage();
                        ActivateCompletedTaskListingPage();
                        break;
                    case ControlsEnum.TASKDETAILPAGE:
                        GetUIValuesFromObject(ControlsEnum.TASKDETAILPAGE);
                        BindGrid(ControlsEnum.BINDSUBTASK);
                        BindGrid(ControlsEnum.BINDTASKHISTORY);
                        break;
                    case ControlsEnum.BINDTASKHISTORYLISTPAGE:
                        BindGrid(ControlsEnum.BINDTASKHISTORYLISTPAGE);
                        divHistoryListPage.Visible = true;
                        break;
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
        protected void ActionHandler(object sender, GridViewSelectEventArgs e)
        {
            //try
            //{
            //    if (((GridView)sender).ID == "grdSubTask")
            //    {
            //        GridView gvSender = (GridView)sender;
            //        gvSender.SelectedIndex = e.NewSelectedIndex;
            //        hfCurrentTask.Value = ((HiddenField)gvSender.SelectedRow.FindControl("hfTaskPK")).Value;
            //        int order = (BreadCrumbProp.Max(s => s.Order)) + 1;
            //        BreadCrumbProp.Add(
            //                         new BreadCrumb { Order = order, TaskId = Convert.ToInt32(hfCurrentTask.Value), Name = ((Label)gvSender.SelectedRow.FindControl("lblTaskName")).Text }
            //                    );
            //        CreateBreadCrumb(Convert.ToInt32(hfCurrentTask.Value));
            //        GetFieldValues(ControlsEnum.TASKDETAILPAGE);
            //        SetFieldValues(ControlsEnum.TASKDETAILPAGE);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            //}
        }

        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridView senderGridView = (GridView)sender;
                if (senderGridView.ID == "grdMyTasks")
                {

                }
                else if (senderGridView.ID == "grdSubTask")
                {
                    if (e.CommandName == "SubTaskClick")
                    {
                        GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);

                        HiddenField hdfTaskPK = row.FindControl("hfTaskPK") as HiddenField;
                        if (hdfTaskPK != null && !string.IsNullOrEmpty(hdfTaskPK.Value))
                        {
                            //// ImageButton imgNew = e.Row.FindControl("imgNew") as ImageButton;
                            // if (!hdfTaskPK.Value.ToString().Equals("1"))
                            // {
                            //     imgNew.Visible = false;
                            // }
                            hfCurrentTask.Value = hdfTaskPK.Value;
                            int order = (BreadCrumbProp.Max(s => s.Order)) + 1;
                            BreadCrumbProp.Add(
                                //new BreadCrumb { Order = order, TaskId = Convert.ToInt32(hfCurrentTask.Value), Name = ((Label)row.FindControl("lblTaskName")).ToolTip }
                                             new BreadCrumb { Order = order, TaskId = Convert.ToInt32(hfCurrentTask.Value), Name = ((LinkButton)row.FindControl("lbnTaskNo")).ToolTip }
                                        );
                            CreateBreadCrumb(Convert.ToInt32(hfCurrentTask.Value));
                            GetFieldValues(ControlsEnum.TASKDETAILPAGE);
                            SetFieldValues(ControlsEnum.TASKDETAILPAGE);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                string senderId = string.Empty;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                    senderId = ((Button)sender).ID;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                    senderId = ((LinkButton)sender).ID;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(GridView)))
                {
                    senderId = ((GridView)sender).ID;
                    if (senderId == "grdMyTasks")
                    {
                        commonActions = ActionsEnum.MYTASKGRIDDATABOUND;
                    }
                    else if (senderId == "grdPendingTasks")
                    {
                        commonActions = ActionsEnum.PENDINGWORKSGRIDDATABOUND;
                    }
                    else if (senderId == "grdCompletedTasks")
                    {
                        commonActions = ActionsEnum.COMPLETEDTASKGRIDDATABOUND;
                    }
                    else if (senderId == "grdSubTask")
                    {
                        commonActions = ActionsEnum.SUBTASKGRIDDATABOUND;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    senderId = ((RadioButton)sender).ID;
                    if (senderId == "rbtSelect")
                    {
                        commonActions = ActionsEnum.TASKSELECTROW;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                    senderId = ((ImageButton)sender).ID;
                }
                switch (commonActions)
                {
                    // Do Action for , when click MYTASKS Tab
                    case ActionsEnum.MYTASKS:
                        //ActivateMyTaskListingPage();
                        uclPagingMyTask.CurrentPage = 1;
                        PageIndexMyTask = 1.ToString();
                        AssignTo = null;
                        IsCompleted = null;
                        chkShowCompleted.Checked = false;
                        txtAssignedUser.Text = "Select/Type";
                        hdfUserPK.Value = null;
                        // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeControlComponents", "toggleMyTaskSearch();", true);
                        GetFieldValues(ControlsEnum.MYTASKS);
                        SetFieldValues(ControlsEnum.MYTASKS);
                        BreadCrumbProp = new List<BreadCrumb>
                                {
                                     new BreadCrumb { Order = 1, TaskId = 0, Name =  GetLocalResourceObject("MyTasks").ToString()}
                                };
                        break;
                    case ActionsEnum.PENDINGWORKS:
                        // ActivatePendingWorksListingPage();
                        uclPagingPendingTask.CurrentPage = 1;
                        PageIndexPendingTask = 1.ToString();
                        AssignBy = null;
                        txtAssignedUserPendingSearch.Text = "Select/Type";
                        hdfUserPKPendingSearch.Value = null;
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeControlComponents", "togglePendingTaskSearch();", true);
                        GetFieldValues(ControlsEnum.PENDINGWORKS);
                        SetFieldValues(ControlsEnum.PENDINGWORKS);
                        BreadCrumbProp = new List<BreadCrumb>
                                {
                                     new BreadCrumb { Order = 1, TaskId = 0, Name =  GetLocalResourceObject("PendingWorks").ToString()}
                                };
                        break;
                    case ActionsEnum.COMPLETEDTASKS:
                        // ActivateCompletedTaskListingPage();
                        uclPagingCompletedTask.CurrentPage = 1;
                        PageIndexCompletedTask = 1.ToString();
                        GetFieldValues(ControlsEnum.COMPLETEDTASKS);
                        SetFieldValues(ControlsEnum.COMPLETEDTASKS);
                        BreadCrumbProp = new List<BreadCrumb>
                                {
                                     new BreadCrumb { Order = 1, TaskId = 0, Name =  GetLocalResourceObject("CompletedTasks").ToString()}
                                };
                        break;
                    case ActionsEnum.NEWTASKLISTPAGE:
                        ucPopUpTask.TaskPk = 0;
                        ucPopUpTask.IsSubtask = 0;
                        ((ImageButton)sender).CommandName = ActionsEnum.DEFAULT.ToString();
                        ucPopUpTask.ActionHandler(sender, e);
                        ucPopUpTask.PopupHeader = "Create Task";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','Create Task','916','350');", true);
                        break;
                    case ActionsEnum.EDITTASKLISTPAGE:
                        //SetUIValuesToObject(ControlsEnum.PICKFOREDIT);
                        bool bIsChecked = false;
                        int taskPk;
                        GridView gvList = new GridView();
                        string breadCrump = string.Empty;
                        if (hfCurrentTaskTypeEnum.Value == TasksTypes.MyTasks.ToString())
                        {
                            gvList = grdMyTasks;
                            breadCrump = GetLocalResourceObject("MyTasks").ToString();
                        }
                        else if (hfCurrentTaskTypeEnum.Value == TasksTypes.PendingTasks.ToString())
                        {
                            gvList = grdPendingTasks;
                            breadCrump = GetLocalResourceObject("PendingWorks").ToString();
                        }
                        else if (hfCurrentTaskTypeEnum.Value == TasksTypes.CompletedTasks.ToString())
                        {
                            gvList = grdCompletedTasks;
                            breadCrump = GetLocalResourceObject("CompletedTasks").ToString();
                        }
                        int order = 0;
                        foreach (GridViewRow grdrow in gvList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                string taskpk = ((HiddenField)grdrow.FindControl("hfTaskPK")).Value;
                                taskPk = Convert.ToInt32(((HiddenField)grdrow.FindControl("hfTaskPK")).Value);
                                hfCurrentTask.Value = taskPk.ToString();
                                order = (BreadCrumbProp.Max(s => s.Order)) + 1;
                                BreadCrumbProp.Add(new BreadCrumb { Order = order, TaskId = taskPk, Name = ((Label)grdrow.FindControl("lblTaskName")).ToolTip });
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            //hfCurrentTaskTypeEnum.Value = TasksTypes.MyTasks.ToString();
                            EnableDetailsPage();
                            GetFieldValues(ControlsEnum.TASKDETAILPAGE);
                            SetFieldValues(ControlsEnum.TASKDETAILPAGE);
                            CreateBreadCrumb(Convert.ToInt32(hfCurrentTask.Value));
                        }
                        else
                        {
                            string msg = GetLocalResourceObject("Msg_Select_Task").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    case ActionsEnum.SHOWTASKPOPUPEDIT:
                        ucPopUpTask.TaskPk = ToNullableInt32(hfCurrentTask.Value) ?? 0;
                        ucPopUpTask.IsSubtask = 0;
                        ((Button)sender).CommandName = ActionsEnum.DEFAULT.ToString();
                        ucPopUpTask.ActionHandler(sender, e);
                        ucPopUpTask.PopupHeader = "Edit Task";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','Edit Task','916','350');", true);
                        break;
                    case ActionsEnum.SHOWTASKPOPUPADDSUB:
                        ucPopUpTask.TaskPk = ToNullableInt32(hfCurrentTask.Value) ?? 0;
                        ucPopUpTask.IsSubtask = 1;
                        ((Button)sender).CommandName = ActionsEnum.DEFAULT.ToString();
                        ucPopUpTask.ActionHandler(sender, e);
                        ucPopUpTask.PopupHeader = "Add SubTask";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpTask]','Add SubTask','916','400');", true);
                        break;
                    case ActionsEnum.BREADCRUMBCLICK:
                        string toolTip = ((LinkButton)sender).ToolTip;
                        if (toolTip == "0")
                        {
                            if (hfCurrentTaskTypeEnum.Value == TasksTypes.MyTasks.ToString()) ActionHandler(lbnMyTasks, new EventArgs());
                            if (hfCurrentTaskTypeEnum.Value == TasksTypes.PendingTasks.ToString()) ActionHandler(lbnPendingWorks, new EventArgs());
                            if (hfCurrentTaskTypeEnum.Value == TasksTypes.CompletedTasks.ToString()) ActionHandler(lbnCompletedTasks, new EventArgs());
                        }
                        else
                        {
                            hfCurrentTask.Value = toolTip;
                            CreateBreadCrumb(Convert.ToInt32(hfCurrentTask.Value));
                            GetFieldValues(ControlsEnum.TASKDETAILPAGE);
                            SetFieldValues(ControlsEnum.TASKDETAILPAGE);
                        }
                        break;
                    case ActionsEnum.SHOWTASKPOPUPUPDATESTATUS:
                        ucPopUpStatus.TaskPk = ToNullableInt32(hfCurrentTask.Value) ?? 0;
                        ((Button)sender).CommandName = ActionsEnum.DEFAULT.ToString();
                        ucPopUpStatus.ActionHandler(sender, e);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divPopUpStatus]','Update Status','916','300');", true);
                        break;
                    case ActionsEnum.BACKTOLISTPAGE:
                        if (hfCurrentTaskTypeEnum.Value == TasksTypes.MyTasks.ToString()) ActionHandler(lbnMyTasks, new EventArgs());
                        if (hfCurrentTaskTypeEnum.Value == TasksTypes.PendingTasks.ToString()) ActionHandler(lbnPendingWorks, new EventArgs());
                        if (hfCurrentTaskTypeEnum.Value == TasksTypes.CompletedTasks.ToString()) ActionHandler(lbnCompletedTasks, new EventArgs());
                        break;
                    case ActionsEnum.MYTASKGRIDDATABOUND:
                        foreach (GridViewRow row in grdMyTasks.Rows)
                        {
                            string imgValue = ((HiddenField)row.FindControl("hfImageFlag")).Value;
                            //string imgValue = "4";
                            Button img = ((Button)row.FindControl("imgListFlg"));
                            img.ToolTip = 
                            img.CssClass = GetListImage(imgValue); // Because of CssClass the image dont shows as we expect, I tried it...
                            //System.Web.UI.HtmlControls.HtmlImage img = (System.Web.UI.HtmlControls.HtmlImage)row.FindControl("imgListFlgHtml");
                            //img.Attributes.Add("class", GetListImage(imgValue));
                            //img.SkinID = GetListImage(imgValue);
                        }
                        break;
                    case ActionsEnum.PENDINGWORKSGRIDDATABOUND:
                        foreach (GridViewRow row in grdPendingTasks.Rows)
                        {
                            string imgValue = ((HiddenField)row.FindControl("hfImageFlag")).Value;
                            Button img = ((Button)row.FindControl("imgListFlg"));
                            img.CssClass = GetListImage(imgValue);
                            //img.SkinID = GetListImage(imgValue);
                        }
                        break;
                    case ActionsEnum.COMPLETEDTASKGRIDDATABOUND:
                        foreach (GridViewRow row in grdCompletedTasks.Rows)
                        {
                            string imgValue = ((HiddenField)row.FindControl("hfImageFlag")).Value;
                            Button img = ((Button)row.FindControl("imgListFlg"));
                            img.CssClass = GetListImage(imgValue);
                            //img.SkinID = GetListImage(imgValue);
                        }
                        break;
                    case ActionsEnum.SUBTASKGRIDDATABOUND:
                        foreach (GridViewRow row in grdSubTask.Rows)
                        {
                            string imgValue = ((HiddenField)row.FindControl("hfImageFlag")).Value;
                            Button img = ((Button)row.FindControl("imgListFlg"));
                            img.CssClass = GetListImage(imgValue);
                            //img.SkinID = GetListImage(imgValue);
                        }
                        break;

                    case ActionsEnum.SEARCH:
                        if (senderId == "btnSearch")
                        {
                            SearchList(ControlsEnum.MYTASKS);
                        }
                        else if (senderId == "btnSearchPendingSearch")
                        {
                            SearchList(ControlsEnum.PENDINGWORKS);
                        }
                        break;

                    case ActionsEnum.CLEAR:
                        if (senderId == "btnClear")
                        {
                            SearchClear(ControlsEnum.MYTASKS);
                        }
                        else if (senderId == "btnClearPendingSearch")
                        {
                            SearchClear(ControlsEnum.PENDINGWORKS);
                        }
                        break;
                    case ActionsEnum.TASKSELECTROW:
                        RadioButton rbTask = sender as RadioButton;
                        GridViewRow parentRow = rbTask.NamingContainer as GridViewRow;
                        string pk = ((HiddenField)parentRow.FindControl("hfTaskPK")).Value;
                        hfCurrentTask.Value = pk;
                        GetFieldValues(ControlsEnum.BINDTASKHISTORYLISTPAGE);
                        SetFieldValues(ControlsEnum.BINDTASKHISTORYLISTPAGE);
                        break;

                    case ActionsEnum.DELETE:
                        GetFieldValues(ControlsEnum.TASKDELETE);
                        if (resultDel > 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Delete_Success").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            ActionHandler(lbnMyTasks, EventArgs.Empty);
                        }
                        else
                        {
                            if (resultDel == (int)DbDeleteStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (resultDel == (int)DbDeleteStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (resultDel == (int)DbDeleteStatus.REFERRED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Referred").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (resultDel == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Task").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.miscellaneous);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                //        if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("PackingSpecDuplicate").ToString()))
                //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.PackingSpecs)) + "','" + Resources.Messages.Information + "');", true);
                //        else
                //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                //        packingSpecMstObj = null;
                //        // packingSpecMstList = null;
                //        //AdmPackingMstServiceClient = null;
            }
        }

        protected void ActionHandler(object sender, DataListCommandEventArgs e)
        {
            try
            {
                DataList dl = (DataList)sender;
                if (dl.ID == "dlBreadCrumb")
                {
                    if (e.CommandName == "BREADCRUMBCLICK")
                    {
                        DataListItem item = (DataListItem)(((Control)e.CommandSource).NamingContainer);

                        HiddenField breadCrumbTaskId = item.FindControl("hfBreadCrumbTaskId") as HiddenField;
                        if (breadCrumbTaskId != null && !string.IsNullOrEmpty(breadCrumbTaskId.Value))
                        {
                            if (breadCrumbTaskId.Value == "0")
                            {
                                if (hfCurrentTaskTypeEnum.Value == TasksTypes.MyTasks.ToString()) ActionHandler(lbnMyTasks, new EventArgs());
                                if (hfCurrentTaskTypeEnum.Value == TasksTypes.PendingTasks.ToString()) ActionHandler(lbnPendingWorks, new EventArgs());
                                if (hfCurrentTaskTypeEnum.Value == TasksTypes.CompletedTasks.ToString()) ActionHandler(lbnCompletedTasks, new EventArgs());
                            }
                            else
                            {
                                hfCurrentTask.Value = breadCrumbTaskId.Value;
                                CreateBreadCrumb(Convert.ToInt32(hfCurrentTask.Value));
                                GetFieldValues(ControlsEnum.TASKDETAILPAGE);
                                SetFieldValues(ControlsEnum.TASKDETAILPAGE);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion ActionHandler

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GridDataPropertyBinder grdDataPropertyBinder = new GridDataPropertyBinder();
                switch (type)
                {
                    case ControlsEnum.MYTASKS:
                        grdDataPropertyBinder.CurrentPage = uclPagingMyTask.CurrentPage == 0 ? 1 : uclPagingMyTask.CurrentPage;
                        grdDataPropertyBinder.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        dtTasks = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetTaskHdrList(grdDataPropertyBinder, currentUser, (int)TasksTypes.MyTasks, AssignTo, IsCompleted, null);
                        break;
                    case ControlsEnum.PENDINGWORKS:
                        grdDataPropertyBinder.CurrentPage = uclPagingPendingTask.CurrentPage == 0 ? 1 : uclPagingPendingTask.CurrentPage;
                        grdDataPropertyBinder.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        dtTasks = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetTaskHdrList(grdDataPropertyBinder, currentUser, (int)TasksTypes.PendingTasks, null, null, AssignBy);
                        break;
                    case ControlsEnum.COMPLETEDTASKS:
                        grdDataPropertyBinder.CurrentPage = uclPagingCompletedTask.CurrentPage == 0 ? 1 : uclPagingCompletedTask.CurrentPage;
                        grdDataPropertyBinder.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        dtTasks = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetTaskHdrList(grdDataPropertyBinder, currentUser, (int)TasksTypes.CompletedTasks, null, null, null);
                        break;
                    case ControlsEnum.TASKDETAILPAGE:
                        int taskPk = ToNullableInt32(hfCurrentTask.Value) ?? 0;
                        dsMainTask = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetSingleTask(taskPk, 2, null, currentUser.SBUID, 1, 1);
                        break;
                    case ControlsEnum.BINDTASKHISTORYLISTPAGE:
                        grdDataPropertyBinder.CurrentPage = 0;//uclPagingMyTask.CurrentPage == 0 ? 1 : uclPagingMyTask.CurrentPage;
                        grdDataPropertyBinder.PageSize = 0; //Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        dtHistory = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetTaskHistoryList(grdDataPropertyBinder, ToNullableInt32(hfCurrentTask.Value));
                        break;
                    case ControlsEnum.TASKDELETE:
                        resultDel = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.DeleteTask(Convert.ToInt32(hfCurrentTask.Value), LastModifiedTime);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        #endregion Get Field Values

        #region BindGrid
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
            try
            {
                int rowCount = 0;
                switch (controlType)
                {
                    case ControlsEnum.MYTASKS:
                        rowCount = 0;
                        if (dtTasks.Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dtTasks.Rows[0]["REC_COUNT"].ToString());
                        }
                        uclPagingMyTask.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexMyTask = PageIndexMyTask == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexMyTask;
                        uclPagingMyTask.CurrentPage = Convert.ToInt32(PageIndexMyTask);
                        grdMyTasks.DataSource = dtTasks;
                        grdMyTasks.DataBind();
                        uclPagingMyTask.Visible = true;
                        uclPagingMyTask.BindPager();



                        break;
                    case ControlsEnum.PENDINGWORKS:
                        rowCount = 0;
                        if (dtTasks.Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dtTasks.Rows[0]["REC_COUNT"].ToString());
                        }
                        uclPagingPendingTask.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexPendingTask = PageIndexPendingTask == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexPendingTask;
                        uclPagingPendingTask.CurrentPage = Convert.ToInt32(PageIndexPendingTask);
                        grdPendingTasks.DataSource = dtTasks;
                        grdPendingTasks.DataBind();
                        uclPagingPendingTask.Visible = true;
                        uclPagingPendingTask.BindPager();
                        break;
                    case ControlsEnum.COMPLETEDTASKS:
                        rowCount = 0;
                        if (dtTasks.Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dtTasks.Rows[0]["REC_COUNT"].ToString());
                        }
                        uclPagingCompletedTask.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexCompletedTask = PageIndexCompletedTask == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexCompletedTask;
                        uclPagingCompletedTask.CurrentPage = Convert.ToInt32(PageIndexCompletedTask);
                        grdCompletedTasks.DataSource = dtTasks;
                        grdCompletedTasks.DataBind();
                        uclPagingCompletedTask.Visible = true;
                        uclPagingCompletedTask.BindPager();
                        break;
                    case ControlsEnum.BINDSUBTASK:
                        grdSubTask.DataSource = dsMainTask.Tables[1];
                        grdSubTask.DataBind();
                        break;
                    case ControlsEnum.BINDTASKHISTORY:
                        grdHistory.DataSource = dsMainTask.Tables[2];
                        grdHistory.DataBind();
                        break;

                    case ControlsEnum.BINDTASKHISTORYLISTPAGE:
                        grdHistoryListPage.DataSource = dtHistory;
                        grdHistoryListPage.DataBind();
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion BindGrid

        #region Helper Methods

        private string GetFormatedDate(DateTime? dateTime)
        {
            if (dateTime == null) return string.Empty;
            //return dateTime.Value.ToString("dd-MMM-yyy : hh:mm");
            return dateTime.Value.ToString("dd-MMM-yyy");
        }

        private int? ToNullableInt32(string str)
        {
            int result;
            return int.TryParse(str, out result) ? (int?)result : default(int?);
        }

        private void EnableListPage()
        {
            divDetailsPage.Visible = false;
            divListPage.Visible = true;
            divHistoryListPage.Visible = false;
            pnlList.Visible = true;
            pnlEntry.Visible = false;
        }

        private void EnableDetailsPage()
        {
            divDetailsPage.Visible = true;
            divListPage.Visible = false;
            pnlList.Visible = false;
            pnlEntry.Visible = true;
        }

        private void ActivateMyTaskListingPage()
        {
            spnMyTasks.Attributes["class"] = "tab-active";
            spnPendingWorks.Attributes["class"] = "tab-inactive";
            spnCompletedTasks.Attributes["class"] = "tab-inactive";
            lbnMyTasks.CssClass = "tab-active my-task";
            lbnPendingWorks.CssClass = "tab-inactive pending";
            lbnCompletedTasks.CssClass = "tab-inactive completed-task";
            divMyTaskList.Visible = true;
            divPendingTaskList.Visible = false;
            divCompletedTaskList.Visible = false;
            hfCurrentTaskTypeEnum.Value = TasksTypes.MyTasks.ToString();
            pnlEntry.Visible = false;
            pnlList.Visible = true;
            //uclPagingMyTask.CurrentPage = 1;
            //uclPagingMyTask.BindPager();
        }

        private void ActivatePendingWorksListingPage()
        {
            spnMyTasks.Attributes["class"] = "tab-inactive";
            spnPendingWorks.Attributes["class"] = "tab-active";
            spnCompletedTasks.Attributes["class"] = "tab-inactive";
            lbnMyTasks.CssClass = "tab-inactive my-task";
            lbnPendingWorks.CssClass = "tab-active pending";
            lbnCompletedTasks.CssClass = "tab-inactive completed-task";
            divMyTaskList.Visible = false;
            divPendingTaskList.Visible = true;
            divCompletedTaskList.Visible = false;
            hfCurrentTaskTypeEnum.Value = TasksTypes.PendingTasks.ToString();
            pnlEntry.Visible = false;
            pnlList.Visible = true;
            //uclPagingPendingTask.CurrentPage = 1;
            //uclPagingPendingTask.BindPager();
        }

        private void ActivateCompletedTaskListingPage()
        {
            spnMyTasks.Attributes["class"] = "tab-inactive";
            spnPendingWorks.Attributes["class"] = "tab-inactive";
            spnCompletedTasks.Attributes["class"] = "tab-active";
            lbnMyTasks.CssClass = "tab-inactive my-task";
            lbnPendingWorks.CssClass = "tab-inactive pending";
            lbnCompletedTasks.CssClass = "tab-active completed-task";
            divMyTaskList.Visible = false;
            divPendingTaskList.Visible = false;
            divCompletedTaskList.Visible = true;
            hfCurrentTaskTypeEnum.Value = TasksTypes.CompletedTasks.ToString();
            pnlEntry.Visible = false;
            pnlList.Visible = true;
            //uclPagingCompletedTask.CurrentPage = 1;
            //uclPagingCompletedTask.BindPager();
        }

        private void CreateBreadCrumb(int currentTask = 0)
        {
            if (currentTask > 0)
            {
                var q = BreadCrumbProp.Where(l => l.TaskId == currentTask).
                    OrderBy(l => l.Order)
                    .FirstOrDefault();
                if (q != null)
                {
                    int order = q.Order;// BreadCrumbProp.Where(l => l.TaskId == currentTask).Single().Order;
                    BreadCrumbProp.RemoveAll(x => x.Order > order);
                }
            }
            dlBreadCrumb.DataSource = BreadCrumbProp;
            dlBreadCrumb.DataBind();
        }
        //TaskPopUp After Save 
        protected void ucPopUpTask_TaskSave(object sender, EventArgs e)
        {
            if (ucPopUpTask.IsSubtask == 1)
            {
                int i = ucPopUpTask.PkId;
                hfCurrentTask.Value = ucPopUpTask.PkId.ToString();
                EnableDetailsPage();
                GetFieldValues(ControlsEnum.TASKDETAILPAGE);
                SetFieldValues(ControlsEnum.TASKDETAILPAGE);
                // CreateBreadCrumb(Convert.ToInt32(hfCurrentTask.Value));
            }
            else
            {
                ActionHandler(lbnMyTasks, EventArgs.Empty);
            }
        }

        //StatusPopUp After Update
        protected void ucPopUpStatus_StatusUpdate(object sender, EventArgs e)
        {
            int i = ucPopUpStatus.TaskPk;
            hfCurrentTask.Value = ucPopUpStatus.TaskPk.ToString();
            EnableDetailsPage();
            GetFieldValues(ControlsEnum.TASKDETAILPAGE);
            SetFieldValues(ControlsEnum.TASKDETAILPAGE);
        }

        protected string GetListImage(string str)
        {
            var result = "";

            if (str == "0")
            {
                //green
                result = "green-Task";
            }
            if (str == "1")
            {
                //gray
                result = "grey-Task";
            }
            if (str == "2")
            {
                //yellow
                result = "yellow-Task";
            }
            if (str == "3")
            {
                //orange
                result = "orange-Task";
            }
            if (str == "4")
            {
                //red
                result = "red-Task";
            }
            return result;
        }

        private void SearchClear(ControlsEnum enumName)
        {
            switch (enumName)
            {
                case ControlsEnum.MYTASKS:
                    txtAssignedUser.Text = "Select/Type";
                    hdfUserPK.Value = null;
                    chkShowCompleted.Checked = false;
                    AssignTo = IsCompleted = null;
                    uclPagingMyTask.CurrentPage = 1;
                    PageIndexMyTask = 1.ToString();
                    GetFieldValues(ControlsEnum.MYTASKS);
                    SetFieldValues(ControlsEnum.MYTASKS);
                    break;
                case ControlsEnum.PENDINGWORKS:
                    txtAssignedUserPendingSearch.Text = "Select/Type";
                    hdfUserPKPendingSearch.Value = null;
                    AssignBy = null;
                    uclPagingPendingTask.CurrentPage = 1;
                    PageIndexPendingTask = 1.ToString();
                    GetFieldValues(ControlsEnum.PENDINGWORKS);
                    SetFieldValues(ControlsEnum.PENDINGWORKS);
                    break;
                default:
                    break;
            }
        }

        private void SearchList(ControlsEnum enumName)
        {
            switch (enumName)
            {
                case ControlsEnum.MYTASKS:
                    AssignTo = ToNullableInt32(hdfUserPK.Value) ?? 0;
                    IsCompleted = chkShowCompleted.Checked ? 1 : 0;
                    uclPagingMyTask.CurrentPage = 1;
                    PageIndexMyTask = 1.ToString();
                    // uclPagingMyTask.TotalPages = 1;
                    GetFieldValues(ControlsEnum.MYTASKS);
                    SetFieldValues(ControlsEnum.MYTASKS);
                    BreadCrumbProp = new List<BreadCrumb>
                                {
                                     new BreadCrumb { Order = 1, TaskId = 0, Name =  GetLocalResourceObject("MyTasks").ToString()}
                                };
                    break;
                case ControlsEnum.PENDINGWORKS:
                    AssignBy = ToNullableInt32(hdfUserPKPendingSearch.Value) ?? 0;
                    uclPagingPendingTask.CurrentPage = 1;
                    PageIndexPendingTask = 1.ToString();
                    GetFieldValues(ControlsEnum.PENDINGWORKS);
                    SetFieldValues(ControlsEnum.PENDINGWORKS);
                    BreadCrumbProp = new List<BreadCrumb>
                                {
                                     new BreadCrumb { Order = 1, TaskId = 0, Name =  GetLocalResourceObject("PendingWorks").ToString()}
                                };
                    break;
                default:
                    break;
            }
        }

        #endregion Helper Methods

        //#region SetUIValuesToObject
        ///// <summary>
        ///// Assigns the object with corresponding input control values
        ///// </summary>
        ///// <returns></returns>
        //private Object SetUIValuesToObject(ControlsEnum controlType)
        //{
        //    Object retObject;
        //    retObject = null;
        //    try
        //    {
        //        switch (controlType)
        //        {
        //            //case ControlsEnum.PICKFOREDIT:
        //            //    bool bIsChecked = false;
        //            //    int taskPk;
        //            //    foreach (GridViewRow grdrow in grdMyTasks.Rows)
        //            //    {
        //            //        RadioButton rbtn;
        //            //        rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
        //            //        if (rbtn.Checked)
        //            //        {
        //            //            bIsChecked = true;
        //            //            string taskpk = ((HiddenField)grdrow.FindControl("hfTaskPK")).Value;
        //            //            taskPk = Convert.ToInt32(((HiddenField)grdrow.FindControl("hfTaskPK")).Value);
        //            //            hfCurrentTask.Value = taskPk.ToString();
        //            //            break;
        //            //        }
        //            //    }
        //            //    if (bIsChecked)
        //            //    {
        //            //        hfCurrentTaskTypeEnum.Value = TasksTypes.MyTasks.ToString();
        //            //        EnableDetailsPage();
        //            //        // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);              
        //            //    }
        //            //    else
        //            //    {

        //            //        string msg = GetLocalResourceObject("Msg_Select_Invoice").ToString();
        //            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
        //            //    }
        //            //    break;
        //        }
        //        return retObject;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}
        //#endregion SetUIValuesToObject

        #region GetUIValuesFromObject
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.TASKDETAILPAGE:
                        txtDate.Text = (dsMainTask.Tables[0].Rows[0]["TSK_DATE"] == null || dsMainTask.Tables[0].Rows[0]["TSK_DATE"].ToString() == string.Empty) 
                                            ? "" 
                                            : GetFormatedDate((DateTime?)dsMainTask.Tables[0].Rows[0]["TSK_DATE"]);
                        txtDate.ToolTip = txtDate.Text;
                        txtNo.Text = dsMainTask.Tables[0].Rows[0]["TSK_NO"].ToString();
                        txtNo.ToolTip = txtNo.Text;
                        txtExpCompleteDate.Text = (dsMainTask.Tables[0].Rows[0]["TSK_EXP_DATE"] == null || dsMainTask.Tables[0].Rows[0]["TSK_EXP_DATE"].ToString() == string.Empty) ? "" : GetFormatedDate((DateTime?)dsMainTask.Tables[0].Rows[0]["TSK_EXP_DATE"]);
                        txtExpCompleteDate.ToolTip= txtExpCompleteDate.Text;
                        if (dsMainTask.Tables[0].Rows[0]["TSK_EST_DATE"] == null || dsMainTask.Tables[0].Rows[0]["TSK_EST_DATE"].ToString() == string.Empty)
                        {
                            txtEstimatedDate.Text = string.Empty;
                        }
                        else
                        {
                            txtEstimatedDate.Text = GetFormatedDate((DateTime?)dsMainTask.Tables[0].Rows[0]["TSK_EST_DATE"]);
                            txtEstimatedDate.ToolTip = txtEstimatedDate.Text;
                        }

                        txtCategory.Text = dsMainTask.Tables[0].Rows[0]["TSK_CATEGORY_TEXT"].ToString();
                        txtCategory.ToolTip = txtCategory.Text;
                        txtStatus.Text = dsMainTask.Tables[0].Rows[0]["TSK_TRX_STATUS_TEXT"].ToString();
                        txtStatus.ToolTip = txtStatus.Text;
                        txtName.Text = dsMainTask.Tables[0].Rows[0]["TSK_NAME"].ToString();
                        txtName.ToolTip = dsMainTask.Tables[0].Rows[0]["TSK_NAME"].ToString();
                        txtDescription.Text = dsMainTask.Tables[0].Rows[0]["TSK_DESC"].ToString();
                        txtDescription.ToolTip = txtDescription.Text;
                        txtAssignedTo.Text = dsMainTask.Tables[0].Rows[0]["TSK_ASSIGN_TO_TEXT"].ToString();
                        txtAssignedTo.ToolTip = txtAssignedTo.Text;
                        //if (dsMainTask.Tables[0].Rows[0]["TSK_CRTD_BY"].ToString() == currentUser.PKUser.ToString()) btnEdit.Visible = true;
                        //else btnEdit.Visible = false;                        
                        hdfassignedUserPK.Value = dsMainTask.Tables[0].Rows[0]["TSK_CRTD_BY"].ToString();
                        hdfCurrTaskStatus.Value = dsMainTask.Tables[0].Rows[0]["TSK_TRX_STATUS"].ToString();
                        LastModifiedTime = Convert.ToDateTime(dsMainTask.Tables[0].Rows[0]["TSK_MOD_DT"]);
                        if (dsMainTask.Tables[0].Rows[0]["TSK_CMP_DATE"] != null && dsMainTask.Tables[0].Rows[0]["TSK_CMP_DATE"].ToString() != string.Empty)
                        {
                            txtCompletedDate.Text = GetFormatedDate((DateTime?)dsMainTask.Tables[0].Rows[0]["TSK_CMP_DATE"]);
                            txtCompletedDate.ToolTip = txtCompletedDate.Text;
                        }
                        //if (dsMainTask.Tables[0].Rows[0]["TSK_TRX_STATUS"] != null && dsMainTask.Tables[0].Rows[0]["TSK_TRX_STATUS"].ToString() == TaskTrxStatus.Completed.ToString())
                        //{
                        //    lblCompletedDate.Visible = txtCompletedDate.Visible = true;
                        //    //if (!lblCompletedDate.Text.Contains(':'))
                        //    //{
                        //    //    lblCompletedDate.Text += ":";
                        //    //}
                        //    txtEstimatedDate.Text = string.Empty;
                        //    txtCompletedDate.Text = GetFormatedDate((DateTime?)dsMainTask.Tables[0].Rows[0]["TSK_CMP_DATE"]);
                        //}
                        //else
                        //{
                        //    lblCompletedDate.Visible = txtCompletedDate.Visible = false;
                        //    txtCompletedDate.Text = string.Empty;
                        //}

                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion GetUIValuesFromObject

        #region "Pagination"


        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPagingMyTask.CurrentPage = 1;
            uclPagingPendingTask.CurrentPage = 1;
            uclPagingCompletedTask.CurrentPage = 1;
        }

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.uclPagingMyTask.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingMyTask.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingMyTask.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingMyTask.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingMyTask.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);

            this.uclPagingPendingTask.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPendingTask.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPendingTask.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPendingTask.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPendingTask.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);

            this.uclPagingCompletedTask.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingCompletedTask.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingCompletedTask.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingCompletedTask.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingCompletedTask.PageChanged += new ActionHandler(this.ActionHandler);
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
                PgerControlNew pagerControl = (PgerControlNew)sender;
                string senderId = pagerControl.ID;


                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        pagerControl.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        // Assignment the first page index.
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assignment the last page index.
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the next page index.
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the previous page index.
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage--;
                        break;
                }
                if (senderId == PagerIds.uclPagingMyTask.ToString())
                {
                    PageIndexMyTask = uclPagingMyTask.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.MYTASKS);
                    SetFieldValues(ControlsEnum.MYTASKS);
                    BreadCrumbProp = new List<BreadCrumb>
                                {
                                     new BreadCrumb { Order = 1, TaskId = 0, Name =  GetLocalResourceObject("MyTasks").ToString()}
                                };
                    EnableDisableButtons(e.TotalPages, PagerIds.uclPagingMyTask);
                }
                else if (senderId == PagerIds.uclPagingPendingTask.ToString())
                {
                    PageIndexPendingTask = uclPagingPendingTask.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.PENDINGWORKS);
                    SetFieldValues(ControlsEnum.PENDINGWORKS);
                    BreadCrumbProp = new List<BreadCrumb>
                                {
                                     new BreadCrumb { Order = 1, TaskId = 0, Name =  GetLocalResourceObject("PendingWorks").ToString()}
                                };
                    EnableDisableButtons(e.TotalPages, PagerIds.uclPagingPendingTask);
                }
                else if (senderId == PagerIds.uclPagingCompletedTask.ToString())
                {
                    PageIndexCompletedTask = uclPagingCompletedTask.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.COMPLETEDTASKS);
                    SetFieldValues(ControlsEnum.COMPLETEDTASKS);
                    BreadCrumbProp = new List<BreadCrumb>
                                {
                                     new BreadCrumb { Order = 1, TaskId = 0, Name =  GetLocalResourceObject("CompletedTasks").ToString()}
                                };
                    EnableDisableButtons(e.TotalPages, PagerIds.uclPagingCompletedTask);
                }



                // EntryStatus = EntryStatus.LISTMODE;
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
        private void EnableDisableButtons(int iTotalPages, PagerIds pagerId)
        {
            switch (pagerId)
            {
                case PagerIds.uclPagingMyTask:
                    // Should we disable the first link
                    uclPagingMyTask.FirstButtonEnabled = (uclPagingMyTask.CurrentPage == 1) ? false : true;
                    // Should we disable the previous link
                    uclPagingMyTask.PreviousButtonEnabled = (uclPagingMyTask.CurrentPage == 1) ? false : true;
                    // Should we enable the next link
                    uclPagingMyTask.NextButtonEnabled = (uclPagingMyTask.CurrentPage < iTotalPages) ? true : false;
                    // Should we enable the last link
                    uclPagingMyTask.LastButtonEnabled = (uclPagingMyTask.CurrentPage < iTotalPages) ? true : false;
                    break;
                case PagerIds.uclPagingPendingTask:
                    // Should we disable the first link
                    uclPagingPendingTask.FirstButtonEnabled = (uclPagingPendingTask.CurrentPage == 1) ? false : true;
                    // Should we disable the previous link
                    uclPagingPendingTask.PreviousButtonEnabled = (uclPagingPendingTask.CurrentPage == 1) ? false : true;
                    // Should we enable the next link
                    uclPagingPendingTask.NextButtonEnabled = (uclPagingPendingTask.CurrentPage < iTotalPages) ? true : false;
                    // Should we enable the last link
                    uclPagingPendingTask.LastButtonEnabled = (uclPagingPendingTask.CurrentPage < iTotalPages) ? true : false;
                    break;
                case PagerIds.uclPagingCompletedTask:
                    // Should we disable the first link
                    uclPagingCompletedTask.FirstButtonEnabled = (uclPagingCompletedTask.CurrentPage == 1) ? false : true;
                    // Should we disable the previous link
                    uclPagingCompletedTask.PreviousButtonEnabled = (uclPagingCompletedTask.CurrentPage == 1) ? false : true;
                    // Should we enable the next link
                    uclPagingCompletedTask.NextButtonEnabled = (uclPagingCompletedTask.CurrentPage < iTotalPages) ? true : false;
                    // Should we enable the last link
                    uclPagingCompletedTask.LastButtonEnabled = (uclPagingCompletedTask.CurrentPage < iTotalPages) ? true : false;
                    break;
            }

        }

        #endregion "Pagination"

    }

    public enum ControlsEnum
    {
        MYTASKS,
        PICKFOREDIT,
        TASKDETAILPAGE,
        BINDMAINTASK,
        BINDSUBTASK,
        BINDTASKHISTORY,
        SHOWTASKPOPUP,
        SHOWSTATUSPOPUP,
        SHOWTASKPOPUPEDIT,
        SHOWSTATUSPOPUPEDIT,
        SHOWTASKPOPUPADDSUB,
        BREADCRUMBCLICK,
        SHOWTASKPOPUPUPDATESTATUS,
        BACKTOLISTPAGE,
        MYTASKGRIDDATABOUND,
        PENDINGWORKSGRIDDATABOUND,
        COMPLETEDTASKGRIDDATABOUND,
        SUBTASKGRIDDATABOUND,
        BINDTASKHISTORYLISTPAGE,
        TASKDELETE,
        //PENDINGWORKSSEARCHCLEAR,
        //MYTASKSSEARCHCLEAR,


        // May be not need
        PENDINGWORKS,
        COMPLETEDTASKS,
        NEWTASKLISTPAGE,
        EDITTASKLISTPAGE


        ////ADD,
        ////NEW,
        ////EDIT,
        ////SAVE,
        ////SUBMIT,
        ////SAVESUBMIT,
        //TASKCATEGORY
    }


    public enum TasksTypes
    {
        MyTasks = 0,
        PendingTasks = 1,
        CompletedTasks = 2
    }

    enum TaskTrxStatus
    {
        Completed = 2
    }

    enum PagerIds
    {
        uclPagingMyTask,
        uclPagingPendingTask,
        uclPagingCompletedTask
    }

    [Serializable]
    public class BreadCrumb
    {
        string name;
        public int Order { get; set; }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                //if (name.Length > 15)
                //{         
                //    name.Remove(11, 4);
                //    name += "...";
                //}
            }
        }
        public int TaskId { get; set; }
    }

}
