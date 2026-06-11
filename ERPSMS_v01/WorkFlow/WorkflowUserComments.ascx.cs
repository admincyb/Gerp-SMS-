using System;
using System.Data;
using System.Web.UI;
using ERP.Utilities;
using MailSendCore;
using System.Configuration;
using BusinessObject.Common;
using BusinessLogic.CommonManagement;
using System.Web;
using MailSendCore;
using System.Collections.Generic;

namespace gComs.WorkFlow
{
    public partial class WorkflowUserComments : System.Web.UI.UserControl
    {
        public event EventHandler WrkfSubmit;
        BusinessObject.User currentUser = new BusinessObject.User();

        #region Properties
        public bool HasPageTaskPermission
        {
            get { return ViewState["HasPageTaskPermission"] == null ? false : Convert.ToBoolean(ViewState["HasPageTaskPermission"]); }
            set { ViewState["HasPageTaskPermission"] = value; }
        }
        public string PageUrl
        {
            get { return Convert.ToString(ViewState["PageUrl"]); }
            set { ViewState["PageUrl"] = value; }
        }
        /// <summary>
        /// To Get if has Actions
        /// </summary>
        public bool HasActions
        {
            get { return ViewState["HasActions"] == null ? false : Convert.ToBoolean(ViewState["HasActions"]); }
            set { ViewState["HasActions"] = value; }
        }
        public int ProcessID
        {
            get { return Convert.ToInt32(ViewState["ProcID"]); }
            set { ViewState["ProcID"] = value; }
        }
        /// <summary>
        /// workflow reference ID
        /// </summary>
        public int RefID
        {
            get { return Convert.ToInt32(ViewState["RefID"]); }
            set { ViewState["RefID"] = value; }
        }
        /// <summary>
        /// Caccel Referance ID
        /// </summary>
        public int CancelRefID
        {
            get { return ViewState["CancelRefID"] == null ? 0 : Convert.ToInt32(ViewState["CancelRefID"]); }
            set { ViewState["CancelRefID"] = value; }
        }

        public int TaskID
        {
            get { return Convert.ToInt32(ViewState["TaskID"]); }
            set { ViewState["TaskID"] = value; }
        }

        public string TaskName
        {
            get { return ViewState["TaskName"].ToString(); }
            set { ViewState["TaskName"] = value; }
        }
        /// <summary>
        /// workflow Application ID - PK field in the DB
        /// </summary>
        public int ApplicationID
        {
            get { return int.Parse(ViewState["AppID"].ToString()); }
            set { ViewState["AppID"] = value; }
        }
        /// <summary>
        /// workflow Submit validation group
        /// </summary>
        public string ValidationGroup
        {
            get { return ViewState["ValidationGroup"].ToString(); }
            set { ViewState["ValidationGroup"] = value; }
        }
        /// <summary>
        /// workflow view type
        /// <value>0 - hide workflow Actions</value>
        /// <value>1 - show workflow Actions</value>
        /// </summary>
        public int ViewType
        {
            get { return ViewState["ViewType"] == null ? 0 : int.Parse(ViewState["ViewType"].ToString()); }
            set { ViewState["ViewType"] = value; }
        }

        /// <summary>
        /// Workflow complete status
        /// </summary>
        public bool IsWkfCompleted
        {
            get { return ViewState["IsWkfCompleted"] == null ? false : Convert.ToBoolean(ViewState["IsWkfCompleted"]); }
            set { ViewState["IsWkfCompleted"] = value; }
        }

        public int ReqDeptID
        {
            get { return ViewState["ReqDeptID"] == null ? 0 : Convert.ToInt32(ViewState["ReqDeptID"]); }
            set { ViewState["ReqDeptID"] = value; }
        }

        public bool HasPageComments
        {
            get { return ViewState["HasPageComments"] == null ? false : Convert.ToBoolean(ViewState["HasPageComments"]); }
            set { ViewState["HasPageComments"] = value; }
        }

        public string PageComments
        {
            get { return ViewState["PageComments"] == null ? string.Empty : ViewState["PageComments"].ToString(); }
            set { ViewState["PageComments"] = value; }
        }
        #endregion

        /// <summary>
        /// Method to View Actions Control in User Controls
        /// </summary>
        public void ViewAction()
        {
            if (ViewType == 1)
            {
                divActionComments.Visible = true;
            }
            else if (ViewType == 0)
            {
                divActionComments.Visible = false;
            }
        }
        /// <summary>
        /// Load event of the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                try
                {
                    if (!string.IsNullOrEmpty(ValidationGroup.ToString()))
                    {
                        btnSubmit.Attributes.Add("onclick", "javascript:return ValidateNow('" + ValidationGroup.ToString() + "')");
                        btnSubmit.ValidationGroup = ValidationGroup.ToString();
                        vvswrkfSummary.ValidationGroup = ValidationGroup.ToString();
                    }
                }
                catch
                {
                    btnSubmit.Attributes.Add("onclick", "javascript:return ValidateNow('Save')");
                    btnSubmit.ValidationGroup = "Save";
                    vvswrkfSummary.ValidationGroup = "Save";
                }



                FillWorkFlowDetails();
                ViewAction();
            }

        }


        /// <summary>
        /// Function Used to fill Vendor details to hiddenfiled
        /// </summary>
        /// <param name="vendorID"></param>        
        public void FillWorkFlowDetails()
        {
            this.HasActions = false;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataSet dsComments;
            DataSet dsCancelComments;
            WorkflowCore.CoreService obj = new WorkflowCore.CoreService();
            ViewAction();
            HasPageTaskPermission = CommonBL.GetPageTaskPermission(ProcessID, PageUrl, RefID, currentUser.PKUser);
            //*************************************RefID is used to get the details of application and its state in Workflow**********************************
            if (RefID != 0)
            {

                //***************************Assigning Ref ID To Hidden Field*********************************************************************************
                if (RefID == 0)//**************Means its is a fresh application Need to do the workflow from base*******************************
                {
                    //********************************Call Workflow TO get the Intitial Task And Action By Providing the the ProcessID************************
                    //Assign the Task And Action According to Process Intial Task Given By Work Flow
                    DataTable dtTask = obj.GetInitialTaskAction(ProcessID); ;
                    if (dtTask.Rows.Count > 0)
                    {
                        TaskID = int.Parse(dtTask.Rows[0]["tskPK"].ToString());
                        TaskName = dtTask.Rows[0]["tskName"].ToString();
                        ApplicationID = 0;
                        FillActions(dtTask);
                    }

                }
                else//*******************************************************Means its in Workflow and application once Saved*********************************
                {


                    //*********************************************************Request Workflow to Get the Application Satus By Providing the RefID************
                    DataTable dtAppStatus = obj.GetAppStatus(RefID, currentUser.PKUser);
                    if (dtAppStatus.Rows.Count > 0 && (this.ProcessID == 0 || Convert.ToInt16(dtAppStatus.Rows[0]["wtdProcess"]) == this.ProcessID))
                    {
                        divActionComments.Visible = true;
                        DataRow drow = dtAppStatus.Rows[0];
                        ProcessID = int.Parse(drow["wtdProcess"].ToString());
                        IsWkfCompleted = drow["wtdIsComplete"].ToString() == "1" ? true : false;
                        //ProcessID = 0;
                        TaskID = int.Parse(drow["ugtTask"].ToString());
                        TaskName = drow["tskName"].ToString();
                        if (ViewType == 1)
                        {
                            FillActions(dtAppStatus);
                        }
                        else
                        {
                            divActionComments.Visible = false;
                        }

                        ApplicationID = int.Parse(drow["refApplication"].ToString());
                        //ApplicationID = 0;
                    }
                    //COMPLETED TASK NEED TO FILL THE COMMENTS FIRST GET THE APPLICATIONID
                    else
                    {
                        DataTable dtAppID = obj.GetApplicationID(RefID);
                        if (dtAppID.Rows.Count > 0)
                        {
                            DataRow drow = dtAppID.Rows[0];
                            ApplicationID = int.Parse(drow["refApplication"].ToString());
                            // ApplicationID = 0;
                            ProcessID = int.Parse(drow["refProcess"].ToString());
                            //ProcessID = 0;
                        }
                        divActionComments.Visible = false;
                    }

                    //************************************************************Used to Fill the Comments Corresponding to the application ID*************************
                    dsComments = obj.GetComments(RefID, 0, 0);
                    //************************************************************To Merge Cancelation comments***********************************
                    if (CancelRefID > 0)
                    {
                        dsCancelComments = obj.GetComments(CancelRefID, 0, 0);
                        if (dsCancelComments != null && dsCancelComments.Tables[1].Rows.Count > 0)
                        {
                            dsCancelComments.Tables[1].Merge(dsComments.Tables[1]);
                            dsComments = dsCancelComments;
                        }
                    }
                    List<string> fieldNames = new List<string> { "cmtDesc", "cmtModByText" };
                    grdComments.DataSource = CommonFunctions.HtmlDecodeDataTable(dsComments.Tables[1], fieldNames);
                    grdComments.DataBind();
                }
            }
            //*******************************Automatically Assigned the referenec No as 0******************************************************
            else
            {
                //********************************Call Workflow TO get the Intitial Task And Action By Providing the the ProcessID************************
                //Assign the Task And Action According to Process Intial Task Given By Work Flow
                DataTable dtTask = new DataTable();
                if (ReqDeptID > 0)
                    dtTask = CommonBL.GetInitialTaskAction(ProcessID, ReqDeptID, currentUser.PKUser);
                else
                    dtTask = obj.GetInitialTaskAction(ProcessID);
                if (dtTask.Rows.Count > 0)
                {
                    TaskID = int.Parse(dtTask.Rows[0]["tskPK"].ToString());
                    TaskName = dtTask.Rows[0]["tskName"].ToString();
                    ApplicationID = 0;
                    FillActions(dtTask);
                }
                grdComments.DataSource = null;
                grdComments.DataBind();
            }
        }

        //Summary
        //CreatedBy Vineeth Babu
        //CreatedOn 01-April-2011
        //Method Used to fill the Action Drop Down Using the Datatable Provided
        private void FillActions(DataTable dtTask)
        {
            WRKFACT_ID.DataTextField = "acnName";
            WRKFACT_ID.DataValueField = "acnPK";
            WRKFACT_ID.DataSource = dtTask;
            WRKFACT_ID.DataBind();
            HasActions = (dtTask != null && dtTask.Rows.Count != 0);
        }

        /// <summary>
        /// Reset properties
        /// </summary>
        public void Reset()
        {
            RefID = 0;
            HasPageTaskPermission = false;
            PageUrl = string.Empty;
            HasActions = false;
            ProcessID = 0;
            TaskID = 0;
            TaskName = string.Empty;
            ApplicationID = 0;
            //ValidationGroup
            ViewType = 0;
            IsWkfCompleted = false;
        }
        /// <summary>
        /// Get Workflow Details
        /// </summary>
        /// <returns></returns>
        public WorkflowDetails GetWorkflowDetails()
        {
            WorkflowDetails retObj = new WorkflowDetails();
            retObj.ActionID = int.Parse(WRKFACT_ID.SelectedValue);
            retObj.ActionText = WRKFACT_ID.SelectedItem.Text;
            retObj.ProcessID = ProcessID;
            retObj.TaskID = TaskID;
            retObj.UserPK = currentUser.PKUser;
            retObj.ReferenceID = RefID;
            retObj.Comments = Server.HtmlEncode(WrkfComments.Text);
            return retObj;
        }
        /// <summary>
        /// Methord used to Do Workflow
        /// </summary>
        /// <returns></returns>
        public int DoWorkFlow()
        {
            int result;
            try
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
                wrkfReq.ActionID = int.Parse(WRKFACT_ID.SelectedValue);
                wrkfReq.ProcessID = ProcessID;
                wrkfReq.TaskID = TaskID;
                wrkfReq.ApplicationID = ApplicationID;
                wrkfReq.UserPK = currentUser.PKUser;
                wrkfReq.ReferenceID = RefID;
                wrkfReq.Comments = Server.HtmlEncode(WrkfComments.Text);
                RefID = wrkfService.DoWorkFlow(wrkfReq);
                result = RefID;
                //if (result > 0)
                //{
                //    //if (Server.HtmlEncode(WrkfComments.Text) != string.Empty)
                //    //{
                //    WorkflowCore.CoreObjects.WorkFlowComment wrkfcmts = new WorkflowCore.CoreObjects.WorkFlowComment();
                //    wrkfcmts.ActionID = wrkfReq.ActionID;
                //    wrkfcmts.ApplicationID = wrkfReq.ApplicationID;
                //    wrkfcmts.ProcessID = wrkfReq.ProcessID;
                //    wrkfcmts.ReferenceID = RefID;
                //    wrkfcmts.UserPk = currentUser.PKUser;
                //    wrkfcmts.WrkfComment = Server.HtmlEncode(WrkfComments.Text);
                //    wrkfcmts.TaskID = wrkfReq.TaskID;
                //    result = wrkfService.SaveComments(wrkfcmts);
                //    WrkfComments.Text = string.Empty;
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseWkfPop1", "if(typeof(ClosePopup) == 'function'){ ClosePopup();}else{ $('#divmodel').hide(); $('html').css({ 'overflow': 'auto' });}", true);
                //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "$('#divmodel').hide();", true);
                //    //}
                //    //    SendMail();
                //}
                //else
                //{
                 //   ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Wrkflw_Error) + "','" + Resources.ErpRes.Information + "');", true);
                  //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "$('#divmodel').hide();", true);
                //}
                WrkfComments.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseWkfPop1", "if(typeof(ClosePopup) == 'function'){ ClosePopup();}else{ $('#divmodel').hide(); $('html').css({ 'overflow': 'auto' });}", true);
                return RefID;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Wrkflw_Error) + "','" + Resources.ErpRes.Information + "');", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "$('#divmodel').hide();", true);
                return 0;
            }
        }

        /// <summary>
        /// Methord used to Do Workflow
        /// </summary>
        /// <returns></returns>
        public int DoWorkFlow(int actionID, int processID, int taskID, int applicationID, int userPK, int refID, string comments)
        {
            int result;
            try
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
                wrkfReq.ActionID = actionID;
                wrkfReq.ProcessID = processID;
                wrkfReq.TaskID = taskID;
                wrkfReq.ApplicationID = applicationID;
                wrkfReq.UserPK = userPK;
                wrkfReq.ReferenceID = refID;
                refID = wrkfService.DoWorkFlow(wrkfReq);

                result = refID;
                if (result != -1)
                {
                    WorkflowCore.CoreObjects.WorkFlowComment wrkfcmts = new WorkflowCore.CoreObjects.WorkFlowComment();
                    wrkfcmts.ActionID = actionID;
                    wrkfcmts.ApplicationID = applicationID;
                    wrkfcmts.ProcessID = processID;
                    wrkfcmts.ReferenceID = refID;
                    wrkfcmts.UserPk = userPK;
                    wrkfcmts.WrkfComment = comments;
                    wrkfcmts.TaskID = taskID;
                    result = wrkfService.SaveComments(wrkfcmts);
                }
                return refID;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Methord used to do direct Workflow from inbox
        /// </summary>
        /// <returns></returns>
        public int DoWorkFlow(int actionID, int processID, int taskID, int applicationID, int userPK, int refID)
        {
            int result;
            try
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
                wrkfReq.ActionID = actionID;
                wrkfReq.ProcessID = processID;
                wrkfReq.TaskID = taskID;
                wrkfReq.ApplicationID = applicationID;
                wrkfReq.UserPK = userPK;
                wrkfReq.ReferenceID = refID;
                wrkfReq.Comments = Server.HtmlEncode(WrkfComments.Text);
                RefID = wrkfService.DoWorkFlow(wrkfReq);
                result = RefID;
                //if (result > 0)
                //{
                //    //if (Server.HtmlEncode(WrkfComments.Text) != string.Empty)
                //    //{
                //    WorkflowCore.CoreObjects.WorkFlowComment wrkfcmts = new WorkflowCore.CoreObjects.WorkFlowComment();
                //    wrkfcmts.ActionID = wrkfReq.ActionID;
                //    wrkfcmts.ApplicationID = wrkfReq.ApplicationID;
                //    wrkfcmts.ProcessID = wrkfReq.ProcessID;
                //    wrkfcmts.ReferenceID = RefID;
                //    wrkfcmts.UserPk = userPK;
                //    wrkfcmts.WrkfComment = Server.HtmlEncode(WrkfComments.Text);
                //    wrkfcmts.TaskID = wrkfReq.TaskID;
                //    result = wrkfService.SaveComments(wrkfcmts);
                //    WrkfComments.Text = string.Empty;
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseWkfPop1", "if(typeof(ClosePopup) == 'function'){ ClosePopup();}else{ $('#divmodel').hide(); $('html').css({ 'overflow': 'auto' });}", true);
                //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "$('#divmodel').hide();", true);
                //    //}
                //    //    SendMail();
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Wrkflw_Error) + "','" + Resources.ErpRes.Information + "');", true);
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "$('#divmodel').hide();", true);
                //}
                WrkfComments.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseWkfPop1", "if(typeof(ClosePopup) == 'function'){ ClosePopup();}else{ $('#divmodel').hide(); $('html').css({ 'overflow': 'auto' });}", true);
                return RefID;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Wrkflw_Error) + "','" + Resources.ErpRes.Information + "');", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "$('#divmodel').hide();", true);
                return 0;
            }
        }


        /// <summary>
        /// Workflow Submit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wrkfSubmit_Click(object sender, EventArgs e)
        {
            WrkfSubmit(sender, e);
        }

        //private void SendMail()
        //{
        //    int resultQue;
        //    DataTable dtInboxMail;
        //    DataTable dtIntimationMail;
        //    int? templateId;
        //    string mailContent;
        //    templateId = 0;
        //    mailContent = "";
        //    WorkFlowMailType mailType;
        //    try
        //    {
        //        //Mail sending code
        //        if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableWkfMail"]))
        //        {
        //            //Inbox mail
        //            dtInboxMail = CommonBL.GetInboxMail(this.RefID);
        //            if (dtInboxMail != null)
        //            {
        //                foreach (DataRow drInbox in dtInboxMail.Rows)
        //                {
        //                    mailType = drInbox["usrMsgType"].Equals(DBNull.Value) ? WorkFlowMailType.CustomMsg :
        //                       (WorkFlowMailType)Enum.Parse(typeof(WorkFlowMailType), drInbox["usrMsgType"].ToString(), true);
        //                    if (!drInbox["usrMailTemplate"].Equals(DBNull.Value) && mailType == WorkFlowMailType.MailTemplate)
        //                        templateId = Convert.ToInt32(drInbox["usrMailTemplate"].ToString());
        //                    else
        //                        mailContent = drInbox["usrMessage"].ToString();
        //                    resultQue = SaveMailQue(drInbox["usrSubject"].ToString(), drInbox["usrEmail"].ToString(), templateId, mailContent);
        //                }
        //            }
        //            //Intimation Mail
        //            dtIntimationMail = CommonBL.GetIntimationMail(this.RefID);
        //            if (dtIntimationMail != null)
        //            {
        //                foreach (DataRow drIntimation in dtIntimationMail.Rows)
        //                {
        //                    mailType = drIntimation["usrMsgType"].Equals(DBNull.Value) ? WorkFlowMailType.CustomMsg :
        //                       (WorkFlowMailType)Enum.Parse(typeof(WorkFlowMailType), drIntimation["usrMsgType"].ToString(), true);
        //                    if (!drIntimation["usrMailTemplate"].Equals(DBNull.Value) && mailType == WorkFlowMailType.MailTemplate)
        //                        templateId = Convert.ToInt32(drIntimation["usrMailTemplate"].ToString());
        //                    else
        //                        mailContent = drIntimation["usrMessage"].ToString();
        //                    resultQue = SaveMailQue(drIntimation["usrSubject"].ToString(), drIntimation["usrEmail"].ToString(), templateId, mailContent);
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        //private int SaveMailQue(string subject, string to, int? tempateId = 0, string mailContent = "")
        //{
        //    int resultQue;
        //    DataTable dtMailContent;
        //    int module;
        //    resultQue = 0;
        //    module = 0;
        //    try
        //    {
        //        MailCore.BO.MailQueueData data = new MailCore.BO.MailQueueData();
        //        if (tempateId.HasValue && tempateId > 0)
        //        {
        //            dtMailContent = TemplateManager.TemplateContentGet(tempateId.Value, this.ApplicationID, currentUser.CurrentSBUPK);
        //            if (dtMailContent != null && dtMailContent.Rows.Count > 0)
        //            {
        //                //TML_APP_TYPE TML_APP_SUB_TYPE TML_TYPE TML_ACTION  TML_NAME  
        //                data.CONTENT = dtMailContent.Rows[0]["CONTENT"].ToString();
        //                if (!dtMailContent.Rows[0]["TML_ACTION"].Equals(DBNull.Value))
        //                    data.ACTION = Convert.ToInt16(dtMailContent.Rows[0]["TML_ACTION"].ToString());
        //                if (!dtMailContent.Rows[0]["TML_APP_SUB_TYPE"].Equals(DBNull.Value))
        //                    data.APP_SUB_TYPE = Convert.ToInt16(dtMailContent.Rows[0]["TML_APP_SUB_TYPE"].ToString());
        //                if (!dtMailContent.Rows[0]["TML_APP_TYPE"].Equals(DBNull.Value))
        //                    data.APP_TYPE = Convert.ToInt16(dtMailContent.Rows[0]["TML_APP_TYPE"].ToString());
        //            }
        //        }
        //        else
        //            data.CONTENT = mailContent;
        //        int.TryParse(ConfigurationManager.AppSettings["gComsModule"], out module);
        //        if (module > 0)
        //            data.MODULE = module;
        //        data.SUBJECT = subject;
        //        data.FROM = ConfigurationManager.AppSettings["SmtpEmail"];
        //        data.TO = to;

        //        data.PK = 0;
        //        data.BIZUNIT = currentUser.CurrentSBUPK;
        //        data.CRTD_BY = currentUser.PKUser;
        //        data.LAST_MOD_DT = DateTime.Now;
        //        resultQue = MailSendManager.SaveMailQueue(data);
        //    }
        //    catch
        //    {

        //    }
        //    return resultQue;
        //}

        #region Mail Sending
        private void SendMail()
        {
            int resultQue;
            DataTable dtInboxMail;
            DataTable dtIntimationMail;
            int? templateId;
            string mailContent;
            templateId = 0;
            mailContent = "";
            WorkFlowMailType mailType;
            try
            {
                //Mail sending code
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableWkfMail"]))
                {
                    //Inbox mail
                    dtInboxMail = CommonBL.GetInboxMail(this.RefID);
                    if (dtInboxMail != null)
                    {
                        foreach (DataRow drInbox in dtInboxMail.Rows)
                        {
                            mailType = drInbox["usrMsgType"].Equals(DBNull.Value) ? WorkFlowMailType.CustomMsg :
                               (WorkFlowMailType)Enum.Parse(typeof(WorkFlowMailType), drInbox["usrMsgType"].ToString(), true);
                            if (!drInbox["usrMailTemplate"].Equals(DBNull.Value) && mailType == WorkFlowMailType.MailTemplate)
                                templateId = Convert.ToInt32(drInbox["usrMailTemplate"].ToString());
                            else
                                mailContent = drInbox["usrMessage"].ToString();
                            resultQue = SaveMailQue(drInbox["usrSubject"].ToString(), drInbox["usrEmail"].ToString(), templateId, mailContent);
                        }
                    }
                    //Intimation Mail
                    dtIntimationMail = CommonBL.GetIntimationMail(this.RefID);
                    if (dtIntimationMail != null)
                    {
                        foreach (DataRow drIntimation in dtIntimationMail.Rows)
                        {
                            mailType = drIntimation["usrMsgType"].Equals(DBNull.Value) ? WorkFlowMailType.CustomMsg :
                               (WorkFlowMailType)Enum.Parse(typeof(WorkFlowMailType), drIntimation["usrMsgType"].ToString(), true);
                            if (!drIntimation["usrMailTemplate"].Equals(DBNull.Value) && mailType == WorkFlowMailType.MailTemplate)
                                templateId = Convert.ToInt32(drIntimation["usrMailTemplate"].ToString());
                            else
                                mailContent = drIntimation["usrMessage"].ToString();
                            resultQue = SaveMailQue(drIntimation["usrSubject"].ToString(), drIntimation["usrEmail"].ToString(), templateId, mailContent);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private int SaveMailQue(string subject, string to, int? tempateId = 0, string mailContent = "")
        {
            int resultQue;
            DataTable dtMailContent;
            int module;
            resultQue = 0;
            module = 0;
            try
            {
                MailCore.BO.MailQueueData data = new MailCore.BO.MailQueueData();
                if (tempateId.HasValue && tempateId > 0)
                {
                    dtMailContent = TemplateManager.TemplateContentGet(tempateId.Value, this.ApplicationID, currentUser.SBUID);
                    if (dtMailContent != null && dtMailContent.Rows.Count > 0)
                    {
                        //TML_APP_TYPE TML_APP_SUB_TYPE TML_TYPE TML_ACTION  TML_NAME  
                        data.CONTENT = dtMailContent.Rows[0]["CONTENT"].ToString();
                        if (!dtMailContent.Rows[0]["TML_ACTION"].Equals(DBNull.Value))
                            data.ACTION = Convert.ToInt16(dtMailContent.Rows[0]["TML_ACTION"].ToString());
                        if (!dtMailContent.Rows[0]["TML_APP_SUB_TYPE"].Equals(DBNull.Value))
                            data.APP_SUB_TYPE_VAL = Convert.ToInt16(dtMailContent.Rows[0]["TML_APP_SUB_TYPE"].ToString());
                        if (!dtMailContent.Rows[0]["TML_APP_TYPE"].Equals(DBNull.Value))
                            data.APP_TYPE = dtMailContent.Rows[0]["TML_APP_TYPE"].ToString();
                    }
                }
                else
                    data.CONTENT = mailContent;
                int.TryParse(ConfigurationManager.AppSettings["gComsModule"], out module);
                if (module > 0)
                    data.MODULE = module;
                data.SUBJECT = subject;
                data.FROM = ConfigurationManager.AppSettings["FrmMailCusMailer"];
                data.TO = to;

                data.PK = 0;
                data.BIZUNIT = currentUser.SBUID;
                data.CRTD_BY = currentUser.PKUser;
                data.LAST_MOD_DT = DateTime.Now;
                resultQue = MailSendManager.SaveMailQueue(data);
            }
            catch
            {

            }
            return resultQue;
        }

        #endregion

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            hdfIsWkfMultipleClick.Value = "0";

            divPageComments.Visible = HasPageComments;
            lblPageComment.Text = PageComments;
        }
    }
}