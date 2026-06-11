using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using DataAccess.CommonManagement;
using BusinessLogic.Administration.Configurations;
using System.Xml;
using System.IO;
using System.Resources;
using System.Collections;
using System.Web.Security;
using BusinessLogic.CommonManagement;
using System.Configuration;
using CustomControls;
using System.Xml.Serialization;

namespace ERPSMS_v01.General
{
    public partial class Default1 : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        #region Variables
        private PageActionEnum commonActions;
        private DataTable dtFolders;
        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataTable dtDepartment;
        private DataTable dtProcess;
        private DataTable dtWkfTrx;
        private SequenceTaskActionBO sequenceTaskAction;
        private List<SequenceTaskActionList> tempSeqTaskActionList;
        #endregion

        #region Properties
        /// <summary>
        /// To Keep Root URL
        /// </summary>
        private DataTable dtTask
        {
            get { return this.ViewState["dtTask"] == null ? new DataTable() : (DataTable)this.ViewState["dtTask"]; }
            set { this.ViewState["dtTask"] = value; }
        }
        #endregion

        /// <summary>
        /// To validate User 
        /// </summary>
        private bool IsValidState
        {
            get { return this.ViewState["IsValidState"] == null ? false : Convert.ToBoolean(this.ViewState["IsValidState"].ToString()); }
            set { this.ViewState["IsValidState"] = value; }
        }
        private int WsqNextTaskPK
        {
            get { return this.ViewState["WsqNextTaskPK"] == null ? 0 : Convert.ToInt32(this.ViewState["WsqNextTaskPK"]); }
            set { this.ViewState["WsqNextTaskPK"] = value; }
        }
        private int TaskActionPK
        {
            get { return this.ViewState["TaskActionPK"] == null ? 0 : Convert.ToInt32(this.ViewState["TaskActionPK"]); }
            set { this.ViewState["TaskActionPK"] = value; }
        }
        private int TaskPK
        {
            get { return this.ViewState["TaskPK"] == null ? 0 : Convert.ToInt32(this.ViewState["TaskPK"]); }
            set { this.ViewState["TaskPK"] = value; }
        }

        private int WsqPK
        {
            get { return this.ViewState["WsqPK"] == null ? 0 : Convert.ToInt32(this.ViewState["WsqPK"]); }
            set { this.ViewState["WsqPK"] = value; }
        }
        private int WsqType
        {
            get { return this.ViewState["WsqType"] == null ? 0 : Convert.ToInt32(this.ViewState["WsqType"]); }
            set { this.ViewState["WsqType"] = value; }
        }
        /// <summary>
        /// To keep Task Action Mapping List in view state
        /// </summary>
        private List<SequenceTaskActionList> SeqTaskActionList
        {
            get { return this.Session["lstSequenceTaskActionList"] == null ? new List<SequenceTaskActionList>() : (List<SequenceTaskActionList>)this.Session["lstSequenceTaskActionList"]; }
            set { this.Session["lstSequenceTaskActionList"] = value; }
        }

        #endregion

        #region PageLevel Events

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            SetPageVariables();
            if (!IsSuperAdminUser(currentUser.PKUser))
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx");
            }
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                ResetForm();
                GetFieldValues(ControlEnum.FILLDEPARTMANT);
                SetFieldValues(ControlEnum.FILLDEPARTMANT);
            }
        }
        #endregion

        #region Page_PreRender
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            if (IsValidState)
            {
                divWorkflowDetails.Visible = true;
                divUserLogin.Visible = false;
            }
            else
            {
                divWorkflowDetails.Visible = false;
                divUserLogin.Visible = true;
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
        }
        #endregion

        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlEnum type)
        {
            try
            {
                switch (type)
                {
                    #region FILL DEPARTMANT
                    case ControlEnum.FILLDEPARTMANT:
                        dtDepartment = DataAccess.CommonManagement.CommonDL.GetWorkFlowDepartment();
                        break;
                    #endregion
                    #region FILL PROCESS
                    case ControlEnum.FILLPROCESS:
                        dtProcess = DataAccess.CommonManagement.CommonDL.GetProcess(Convert.ToInt32(ddlDepartment.SelectedValue));
                        break;
                    #endregion
                    #region TASK
                    case ControlEnum.TASK:
                        dtTask = DataAccess.CommonManagement.CommonDL.GetSequenceTask(Convert.ToInt32(ddlProcess.SelectedValue));
                        break;
                    #endregion
                    #region WKF TRANSACTIONS
                    case ControlEnum.WKFTRANSACTIONS:
                        dtWkfTrx = DataAccess.CommonManagement.CommonDL.GetWorkflowTransactions(TaskPK);
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

        #region Set Field Values
        private void SetFieldValues(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region FILL DEPARTMANT
                    case ControlEnum.FILLDEPARTMANT:
                        BindDropDown(ControlEnum.FILLDEPARTMANT);
                        break;
                    #endregion
                    #region FILL PROCESS
                    case ControlEnum.FILLPROCESS:
                        BindDropDown(ControlEnum.FILLPROCESS);
                        break;
                    #endregion
                    #region TASK
                    case ControlEnum.TASK:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region WKF TRANSACTIONS
                    case ControlEnum.WKFTRANSACTIONS:
                        BindGrid(controlType);
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Method
        #region Is Super Admin User
        /// <summary>
        /// Is Super Admin User
        /// </summary>
        /// <param name="pkUser"></param>
        /// <returns></returns>
        private bool IsSuperAdminUser(int pkUser)
        {
            bool retVal = false;
            DataTable dtResult = UserManagementBL.SuperAdminMstGet(pkUser);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                retVal = dtResult.Rows[0]["usrIsSuperAdmin"].ToString() == "1" ? true : false;
            }
            return retVal;
        }
        #endregion

        #region SetPageVariables
        private void SetPageVariables()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }
        #endregion

        #region IsValidUser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool IsValidUser(string password)
        {

            bool result = false;
            dtResult = CommonBL.GetApplicaitonConfiguaration("MAIL STATUS", "MTTB", currentUser.SBUID);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                result = dtResult.Rows[0]["ACF_DATA"].ToString().ToLower() == password.ToLower();
            }
            return result;
        }
        #endregion

        #region BindGrid
        /// <summary>
        /// BindGrid
        /// </summary>
        /// <param name="controlType"></param>
        protected void BindGrid(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region SEQUENCE MAPP LIST
                    case ControlEnum.SEQUENCEMAPPLIST:
                        if (sequenceTaskAction != null && sequenceTaskAction.SeqActionDetailsList != null && sequenceTaskAction.SeqActionDetailsList.Count > 0)
                            grdSeqActionDetails.DataSource = sequenceTaskAction.SeqActionDetailsList;
                        else
                            grdSeqActionDetails.DataSource = null;
                        grdSeqActionDetails.DataBind();
                        break;
                    #endregion
                    #region TASK
                    case ControlEnum.TASK:
                        if (dtTask != null && dtTask.Rows.Count > 0)
                        {
                            grdTask.DataSource = dtTask;
                        }
                        else
                            grdTask.DataSource = new DataTable();
                        grdTask.DataBind();
                        break;
                    #endregion
                    #region WKF TRANSACTIONS
                    case ControlEnum.WKFTRANSACTIONS:
                        if (dtWkfTrx != null && dtWkfTrx.Rows.Count > 0)
                            grdWkfTrx.DataSource = dtWkfTrx;
                        else
                            grdWkfTrx.DataSource = null;
                        grdWkfTrx.DataBind();
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

        #region BindDropdown
        /// <summary>
        /// Bind DropDown  as per type 
        /// </summary>
        /// <param name="drpName"></param>
        private void BindDropDown(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    //Fill Department to DropDown
                    case ControlEnum.FILLDEPARTMANT:
                        if (dtDepartment != null)
                        {
                            ddlDepartment.DataSource = CommonFunctions.HtmlDecodeDataTable(dtDepartment);
                            ddlDepartment.DataTextField = "DPT_NAME";
                            ddlDepartment.DataValueField = "DPT_PK";
                            ddlDepartment.DataBind();
                        }
                        ddlDepartment.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    //Fill Process to DropDown
                    case ControlEnum.FILLPROCESS:
                        if (dtProcess != null)
                        {
                            ddlProcess.DataSource = CommonFunctions.HtmlDecodeDataTable(dtProcess);
                            ddlProcess.DataTextField = "prcName";
                            ddlProcess.DataValueField = "prcPK";
                            ddlProcess.DataBind();
                        }
                        ddlProcess.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            ddlDepartment.Items.Clear();
            ddlProcess.Items.Clear();
            ddlDepartment.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
            ddlProcess.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
            SeqTaskActionList = null;
        }
        #endregion

        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            GridViewRow gvr;
            GridView grd;
            ExtGridView egrd;
            HiddenField hdfTskPK;
            ExtGridViewRow exgrvrow;
            string arg;
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (PageActionEnum)(Enum.Parse(typeof(PageActionEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (PageActionEnum)(Enum.Parse(typeof(PageActionEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlDepartment")
                    {
                        commonActions = PageActionEnum.PROCESS;
                    }
                    else if (((DropDownList)sender).ID == "ddlProcess")
                    {
                        commonActions = PageActionEnum.TASK;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (PageActionEnum)(Enum.Parse(typeof(PageActionEnum), ((ImageButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    #region Task Action Mapping
                    case PageActionEnum.ACTIONMAPPING:
                        arg = ((ImageButton)sender).CommandArgument;
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        GridViewRow gvrRow = ((ImageButton)sender).Parent.Parent.Parent.Parent.Parent.Parent as ExtGridViewRow;
                        int ParentRowIndex = gvrRow.RowIndex;
                        WsqNextTaskPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfNextTask")).Value);
                        TaskActionPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfTaskAction")).Value);

                        WsqPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfWsqPK")).Value);
                        WsqType = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfType")).Value);
                        TaskPK = Convert.ToInt32(((HiddenField)grdTask.Rows[ParentRowIndex].FindControl("hdfTskPK")).Value);
                        sequenceTaskAction = new SequenceTaskActionBO();
                        string XMLResult = DataAccess.CommonManagement.CommonDL.GetSequenceTaskActionList(WsqPK, WsqNextTaskPK, WsqType);
                        sequenceTaskAction = CommonFunctions.XmlDeserialize<SequenceTaskActionBO>(XMLResult);
                        BindGrid(ControlEnum.SEQUENCEMAPPLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "MappingDetails", "ShowMappingDetails();", true);
                        break;
                    #endregion
                    #region CANCEL POPUP
                    case PageActionEnum.CANCELPOPUP:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region extra grid ondemand data population
                    case PageActionEnum.TASKACTIONS:
                        arg = ((Button)sender).CommandArgument;
                        gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                        if (gvr != null)
                        {
                            grd = gvr.FindControl("grdAction") as GridView;
                            if (string.IsNullOrEmpty(arg))
                            {
                                dtResult = null;
                            }
                            else
                            {
                                TaskPK = Convert.ToInt32(arg);
                                dtResult = DataAccess.CommonManagement.CommonDL.GetSequenceTaskActions(TaskPK);
                                if (dtResult != null && dtResult.Rows.Count > 0)
                                {
                                    dtResult.Columns.Add(new DataColumn() { ColumnName = "wsqNextTaskLevel", DataType = typeof(string) });
                                    foreach (DataRow row in dtResult.Rows)
                                    {
                                        if (row["wsqNextTask"] == DBNull.Value || row["wsqNextTask"] == null)
                                        {
                                            row["wsqNextTaskLevel"] = "End";
                                            continue;
                                        }
                                        var levelText = (from customer in dtTask.AsEnumerable()
                                                         where customer.Field<int>("wsqTask") == (int)row["wsqNextTask"]
                                                         select new
                                                         {
                                                             wsqLevel = customer.Field<int>("wsqLevel")
                                                         }).ToList();

                                        if (levelText != null && levelText.Count > 0)
                                        {
                                            row["wsqNextTaskLevel"] = levelText[0].wsqLevel;
                                        }
                                        else
                                            row["wsqNextTaskLevel"] = "End";
                                    }
                                }
                                //
                            }
                            grd.Visible = true;
                            grd.DataSource = dtResult;
                            grd.DataBind();
                            (gvr.FindControl("hdfIsExpandedTask") as HiddenField).Value = "1";
                        }
                        break;



                    #endregion
                    #region Get Process
                    case PageActionEnum.PROCESS:
                        GetFieldValues(ControlEnum.FILLPROCESS);
                        SetFieldValues(ControlEnum.FILLPROCESS);
                        dtTask = null;
                        SetFieldValues(ControlEnum.TASK);
                        break;
                    #endregion
                    #region TASK
                    case PageActionEnum.TASK:
                        dtTask = null;
                        GetFieldValues(ControlEnum.TASK);
                        SetFieldValues(ControlEnum.TASK);
                        break;
                    #endregion
                    #region Submit
                    case PageActionEnum.SUBMIT:
                        IsValidState = IsValidUser(txtPassword.Text);
                        break;
                    #endregion
                    #region SAVE
                    case PageActionEnum.SAVE:
                        XmlDocument xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan((WKFSequenceConfig)SetUIValuesToObject());
                        int result = DataAccess.CommonManagement.CommonDL.SaveSequenceConfig(xmlDoc.InnerXml);
                        if (result >= 0) // Success ! re-initialize the page
                        {
                            SeqTaskActionList = null;
                            GetFieldValues(ControlEnum.TASK);
                            SetFieldValues(ControlEnum.TASK);
                            //Show Save success message and reset Contract Entry
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SequenceConfiguration);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SequenceConfiguration + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SequenceConfiguration + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SequenceConfiguration);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region APPLY MAPPING
                    case PageActionEnum.APPLYMAPPING:
                        List<SequenceTaskActionList> lstActionMap;
                        if (SeqTaskActionList != null && SeqTaskActionList.Count > 0)
                        {
                            tempSeqTaskActionList = SeqTaskActionList;
                            tempSeqTaskActionList.RemoveAll(item => item.TaskPK == TaskPK && item.TaskActionPK == TaskActionPK);
                            lstActionMap = tempSeqTaskActionList;
                        }
                        else
                            lstActionMap = new List<SequenceTaskActionList>();
                        SequenceTaskActionList objSequenceTaskAction;
                        foreach (GridViewRow grv in grdSeqActionDetails.Rows)
                        {
                            //if (((CheckBox)grv.FindControl("chkIsMapped")).Checked)
                            //{
                            objSequenceTaskAction = new SequenceTaskActionList();
                            objSequenceTaskAction.TaskPK = TaskPK;
                            objSequenceTaskAction.TaskActionPK = TaskActionPK;
                            objSequenceTaskAction.sqaSequence = WsqPK;
                            objSequenceTaskAction.sqaTaskAction = Convert.ToInt32(((HiddenField)grv.FindControl("hdfSqaTaskAction")).Value);
                            objSequenceTaskAction.sqaIsMapped = ((CheckBox)grv.FindControl("chkIsMapped")).Checked ? 1 : 0; ;
                            objSequenceTaskAction.sqaType = WsqType;
                            objSequenceTaskAction.sqaIsDefault = ((RadioButton)grv.FindControl("rbtIsDefault")).Checked ? 1 : 0;
                            lstActionMap.Add(objSequenceTaskAction);
                            //}
                        }
                        SeqTaskActionList = lstActionMap;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region VIEW
                    case PageActionEnum.VIEW:
                        hdfTskPK = (HiddenField)((ExtGridViewRow)((ImageButton)(sender)).Parent.Parent).FindControl("hdfTskPK");
                        TaskPK = Convert.ToInt32(hdfTskPK.Value);
                        GetFieldValues(ControlEnum.WKFTRANSACTIONS);
                        SetFieldValues(ControlEnum.WKFTRANSACTIONS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWorkflowTransactions", "ShowWorkflowTransactions();", true);
                        break;
                    #endregion
                    #region DELETETASK
                    case PageActionEnum.DELETETASK:
                        exgrvrow = (ExtGridViewRow)((ImageButton)(sender)).Parent.Parent;
                        hdfTskPK = (HiddenField)exgrvrow.FindControl("hdfTskPK");
                        Label lblLevel = (Label)exgrvrow.FindControl("lblLevel");
                        TaskPK = Convert.ToInt32(hdfTskPK.Value);
                        if (TaskPK > 0)
                        {
                            result = DataAccess.CommonManagement.CommonDL.DeleteWorkflowTask(TaskPK, Convert.ToInt32(lblLevel.Text));
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SequenceConfiguration);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                GetFieldValues(ControlEnum.TASK);
                                SetFieldValues(ControlEnum.TASK);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SequenceConfiguration + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SequenceConfiguration + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SequenceConfiguration);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        /// <summary>
        /// Set UI Values To Object
        /// </summary>
        /// <returns></returns>
        public object SetUIValuesToObject()
        {
            WKFSequenceConfig objPage = new WKFSequenceConfig();
            objPage.DeptPK = Convert.ToInt32(ddlDepartment.SelectedValue);
            objPage.ProcessPK = Convert.ToInt32(ddlProcess.SelectedValue);
            objPage.UserPK = currentUser.PKUser;
            objPage.LastModDate = DateTime.Now;
            objPage.lstTaskDetails = new List<TaskDetails>();
            foreach (GridViewRow grvTask in grdTask.Rows)
            {
                TaskDetails objTask = new TaskDetails();
                objTask.tskName = ((TextBox)grvTask.FindControl("txtTaskName")).Text;
                objTask.tskPK = Convert.ToInt32(((HiddenField)grvTask.FindControl("hdfTskPK")).Value);
                objTask.tskOrderSeq = ((Label)grvTask.FindControl("lblLevel")).Text;
                GridView gdAction = (GridView)grvTask.FindControl("grdAction") as GridView;
                objTask.lstSeqDetail = new List<SeqDetail>();
                foreach (GridViewRow grvAction in gdAction.Rows)
                {
                    SeqDetail objSequence = new SeqDetail();
                    objSequence.wsqPK = Convert.ToInt32(((HiddenField)grvAction.FindControl("hdfWsqPK")).Value);
                    objSequence.wsqTaskAction = Convert.ToInt32(((HiddenField)grvAction.FindControl("hdfTaskAction")).Value);
                    objSequence.wsqTaskActionText = ((TextBox)grvAction.FindControl("txtTaskAction")).Text;
                    objSequence.wsqTaskActionSeq = Convert.ToInt32(((TextBox)grvAction.FindControl("txtSequence")).Text);
                    objSequence.wsqType = Convert.ToInt32(((HiddenField)grvAction.FindControl("hdfType")).Value);
                    if (SeqTaskActionList != null && SeqTaskActionList.Count > 0)
                    {
                        List<SequenceTaskActionList> objTaskActionList = SeqTaskActionList.Where(r => r.TaskPK == objTask.tskPK
                                                                                                        && r.TaskActionPK == objSequence.wsqTaskAction
                                                                                                        && r.sqaIsMapped == 1
                                                                                                     ).ToList();
                        if (objTaskActionList != null && objTaskActionList.Count > 0)
                        {
                            objSequence.lstSeqActionDetails = objTaskActionList;
                            objSequence.wsqNextActionFlag = 1;
                        }
                    }
                    objTask.lstSeqDetail.Add(objSequence);
                }
                objPage.lstTaskDetails.Add(objTask);
            }
            return objPage;
        }
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (((GridView)sender).ID == "grdAction")
                    {
                        TextBox txtSequence = (TextBox)e.Row.FindControl("txtSequence");
                        if (!string.IsNullOrEmpty(txtSequence.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtSequence.ID + "", "$('[id$=" + txtSequence.ID + "]').ForceToNumeric();", true);
                    }
                    if (((GridView)sender).ID == "grdSeqActionDetails")
                    {
                        CheckBox chkIsMapped = (CheckBox)e.Row.FindControl("chkIsMapped");
                        RadioButton rbtIsDefault = (RadioButton)e.Row.FindControl("rbtIsDefault");
                        HiddenField hdfSqaTaskAction = (HiddenField)e.Row.FindControl("hdfSqaTaskAction");
                        HiddenField hdfsqaType = (HiddenField)e.Row.FindControl("hdfsqaType");
                        HiddenField hdfsqaSequence = (HiddenField)e.Row.FindControl("hdfsqaSequence");
                        if (SeqTaskActionList != null
                            && SeqTaskActionList.Count > 0
                            )
                        {
                            SequenceTaskActionList objTskAction = SeqTaskActionList.SingleOrDefault(item => item.TaskPK == TaskPK
                                                                                                        && item.TaskActionPK == TaskActionPK
                                                                                                        && item.sqaSequence == Convert.ToInt32(hdfsqaSequence.Value)
                                                                                                        && item.sqaTaskAction == Convert.ToInt32(hdfSqaTaskAction.Value)
                                                                                                        && item.sqaType == Convert.ToInt32(hdfsqaType.Value)
                                                                                                        );
                            if (objTskAction != null)
                            {
                                chkIsMapped.Checked = objTskAction.sqaIsMapped == 1 ? true : false;
                                rbtIsDefault.Checked = objTskAction.sqaIsDefault == 1 ? true : false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
        }
        #endregion

        #region ControlEnum
        public enum ControlEnum
        {
            FILLDEPARTMANT,
            FILLPROCESS,
            MODULE,
            FILLGRID,
            BINDGRID,
            SAVE,
            TASK,
            TASKACTIONS,
            SEQUENCEMAPPLIST,
            WKFTRANSACTIONS
        }
        private enum PageActionEnum
        {
            DEPARTMENT,
            PROCESS,
            TASK,
            SUBMIT,
            TASKACTIONS,
            SAVE,
            ACTIONMAPPING,
            APPLYMAPPING,
            CANCELPOPUP,
            VIEW,
            DELETETASK
        }
        #endregion
    }

}