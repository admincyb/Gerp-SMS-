using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using BusinessObject.Common;
using System.Configuration;
using BusinessLogic.CommonManagement;
using MailSendCore;
using System.Threading;

namespace ERPSMS_v01.WorkFlow
{
    public partial class WorkflowComments : System.Web.UI.UserControl
    {
        public event EventHandler WrkfSubmit;
        BusinessObject.User currentUser;


        #region Properties
        public int ProcessID
        {
            get { return Convert.ToInt32(ViewState["ProcID"]); }
            set { ViewState["ProcID"] = value; }
        }
        public int RefID
        {
            get { return Convert.ToInt32(ViewState["RefID"]); }
            set { ViewState["RefID"] = value; }
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
        public int ApplicationID
        {
            get { return int.Parse(ViewState["AppID"].ToString()); }
            set { ViewState["AppID"] = value; }
        }

        public string ValidationGroup
        {
            get { return ViewState["ValidationGroup"].ToString(); }
            set { ViewState["ValidationGroup"] = value; }
        }
        public int ViewType
        {
            get { return int.Parse(ViewState["ViewType"].ToString()); }
            set { ViewState["ViewType"] = value; }
        }

        public int ReqDeptID
        {
            get { return ViewState["ReqDeptID"] == null ? 0 : Convert.ToInt32(ViewState["ReqDeptID"]); }
            set { ViewState["ReqDeptID"] = value; }
        }

        public bool HasActions
        {
            get { return ViewState["HasActions"] == null ? false : Convert.ToBoolean(ViewState["HasActions"]); }
            set { ViewState["HasActions"] = value; }
        }
        public const int Counter = 2;

        /// <summary>
        /// Widget: an array of widgets.
        /// </summary>
        public string[] _CommentsFromPage = new string[Counter];
        public string[] Widget
        {
            get { return _CommentsFromPage; }
            set { _CommentsFromPage = value; }
        }



        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                try
                {
                    if (!string.IsNullOrEmpty(ValidationGroup.ToString()))
                    {
                        btnSubmitWrkf.Attributes.Add("onclick", "javascript:return ValidateNow('" + ValidationGroup.ToString() + "')");
                        btnSubmitWrkf.ValidationGroup = ValidationGroup.ToString();
                        vvswrkfSummary.ValidationGroup = ValidationGroup.ToString();

                    }
                }
                catch
                {
                    btnSubmitWrkf.Attributes.Add("onclick", "javascript:return ValidateNow('Save')");
                    btnSubmitWrkf.ValidationGroup = "Save";
                    vvswrkfSummary.ValidationGroup = "Save";
                }
                ViewAction();

                //FillWorkFlowDetails(true); 
            }

        }

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
        /// Function Used to fill Vendor details to hiddenfiled
        /// </summary>
        /// <param name="vendorID"></param>  
        /// status==false , Action from Process to process Actions
        public void FillWorkFlowDetails(bool status)
        {
            this.HasActions = false;
            DataSet dsComments;
            WorkflowCore.CoreService obj = new WorkflowCore.CoreService();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //*************************************RefID is used to get the details of application and its state in Workflow**********************************
            if (RefID != 0)
            {
                //*********************************************************Request Workflow to Get the Application Satus By Providing the RefID************
                ReferenceID.Value = RefID.ToString();
                DataTable dtAppStatus = obj.GetAppStatus(RefID, ((User)HttpContext.Current.User.Identity).PKUser);
                if (dtAppStatus.Rows.Count > 0)
                {
                    divActionComments.Visible = true;
                    DataRow drow = dtAppStatus.Rows[0];
                    ProcessID = int.Parse(drow["wtdProcess"].ToString());
                    TaskID = int.Parse(drow["ugtTask"].ToString());
                    TaskPK.Value = drow["ugtTask"].ToString();
                    TaskName = drow["tskName"].ToString();
                    FillActions(dtAppStatus);
                    //false:- for Process to process Switching 
                    //true:- for normal flow
                    if (status)
                    {
                        ApplicationID = int.Parse(drow["refApplication"].ToString());
                    }
                }
                //COMPLETED TASK NEED TO FILL THE COMMENTS FIRST GET THE APPLICATIONID
                else
                {
                    //false:- for Process to process Switching 
                    //true:- for normal flow
                    if (status)
                    {
                        DataTable dtAppID = obj.GetApplicationID(RefID);
                        if (dtAppID.Rows.Count > 0)
                        {
                            DataRow drow = dtAppID.Rows[0];
                            ApplicationID = int.Parse(drow["refApplication"].ToString());
                            ProcessID = int.Parse(drow["refProcess"].ToString());
                        }
                    }
                    divActionComments.Visible = false;
                }
                //************************************************************Used to Fill the Comments Corresponding to the application ID*************************
                dsComments = obj.GetComments(RefID, 0, 0);
                grdComments.DataSource = CommonFunctions.HtmlDecodeDataTable(dsComments.Tables[1]);//grdComments.DataSource = CommonFunctions.HtmlDecodeDataTable(dsComments.Tables[1], "cmtDesc");
                grdComments.DataBind();
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
                    TaskPK.Value = dtTask.Rows[0]["tskPK"].ToString();
                    TaskName = dtTask.Rows[0]["tskName"].ToString();
                    ApplicationID = 0;
                    FillActions(dtTask);
                }

            }
        }

        /// <summary>
        /// Function Used to fill Vendor details to hiddenfiled
        /// </summary>
        /// <param name="vendorID"></param>  
        /// status==false , Action from Process to process Actions
        public void FillWorkFlowChangeHistoryDetails(DataTable dtChangeHistory, bool verificationRequired, bool isVerified, bool RateGridShow)
        {
            #region Currency,Rate Format
            string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            hdfCurrencyFormatWithSeperator.Value = "#" + currencysep + "#0.";
            for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
            {
                hdfCurrencyFormatWithSeperator.Value += "0";
            }

            hdfRateFormat.Value = "#0.";
            int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
            for (int i = 0; i < rateDecimalDigits; i++)
            {
                hdfRateFormat.Value += "0";
            }
            #endregion

            divOuterWkfChangeHistory.Visible = RateGridShow;
            grdChangeHistory.DataSource = CommonFunctions.HtmlDecodeDataTable(dtChangeHistory);
            grdChangeHistory.DataBind();
            //chkVerifyChanges.Checked = isVerified;
            // chkVerifyChanges.Enabled = verificationRequired;
            if (isVerified)
            {
                spnVerify.Visible = false;
            }
            else
            {
                if (verificationRequired)
                {
                    spnVerify.Visible = true;
                }
                else
                {
                    spnVerify.Visible = false;
                }
            }

            //hdnVerificationRequired.Value = verificationRequired == true ? "1" : "0"; //Need to compire isVerified for value 1
            hdnVerificationRequired.Value = (verificationRequired == true && !isVerified) ? "1" : "0"; //From verify status again try to review,it shows validation (With rate change)

            divWrkfComment.Attributes.Add("class", "gridwrap max-200");
            hdfDefaultActionPk.Value = string.IsNullOrEmpty(WRKFACT_ID.SelectedValue) ? "0" : WRKFACT_ID.SelectedValue;
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
                if (((GridView)sender).ID == "grdChangeHistory")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        #region If New Rate is greater than old rate then set the fontColor to Red ,otherwise green.
                        decimal prevRate = Convert.ToDecimal(e.Row.Cells[1].Text);
                        decimal newRate = Convert.ToDecimal(e.Row.Cells[2].Text);
                        if (newRate > prevRate)
                        {
                            e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
                        }
                        e.Row.Cells[1].Text = GetFormattedRate(prevRate);
                        e.Row.Cells[2].Text = GetFormattedRate(newRate);
                        #endregion
                        #region If New Amount is greater than old amount then set the fontColor to Red ,otherwise green.
                        decimal prevAmount = Convert.ToDecimal(e.Row.Cells[3].Text);
                        decimal newAmount = Convert.ToDecimal(e.Row.Cells[4].Text);
                        if (newAmount > prevAmount)
                        {
                            e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            e.Row.Cells[4].ForeColor = System.Drawing.Color.Green;
                        }
                        e.Row.Cells[3].Text = GetFormattedCurrencyWithSeperation(prevAmount);
                        e.Row.Cells[4].Text = GetFormattedCurrencyWithSeperation(newAmount);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {

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
        WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq;
        /// <summary>
        /// Methord used to Do Workflow
        /// </summary>
        /// <returns></returns>
        public int DoWorkFlow()
        {
            int result;
            int refId = 0;
            try
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                wrkfReq = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
                wrkfReq.ActionID = int.Parse(WRKFACT_ID.SelectedValue);
                wrkfReq.ProcessID = ProcessID;
                wrkfReq.TaskID = TaskID;
                wrkfReq.ApplicationID = ApplicationID;
                wrkfReq.UserPK = currentUser.PKUser;
                wrkfReq.ReferenceID = RefID;
                wrkfReq.Comments = Server.HtmlEncode(WrkfComments.Text);
                RefID = wrkfService.DoWorkFlow(wrkfReq);
                refId = RefID;
                WrkfComments.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseWkfPop1", "if(typeof(ClosePopup) == 'function'){ ClosePopup();}else{ $('#divmodel').hide(); $('html').css({ 'overflow': 'auto' });}", true);
                ////if(WrkfComments.Text!=string.Empty)
                ////{
                //    WorkflowCore.CoreObjects.WorkFlowComment wrkfcmts = new WorkflowCore.CoreObjects.WorkFlowComment();
                //    wrkfcmts.ActionID = wrkfReq.ActionID;
                //    wrkfcmts.ApplicationID = wrkfReq.ApplicationID;
                //    wrkfcmts.ProcessID = wrkfReq.ProcessID;
                //    wrkfcmts.ReferenceID = RefID;
                //    wrkfcmts.UserPk = currentUser.PKUser;
                //    wrkfcmts.WrkfComment = WrkfComments.Text;
                //    wrkfcmts.TaskID = wrkfReq.TaskID;
                //    result = wrkfService.SaveComments(wrkfcmts);
                ////}
                if (refId <= 0)
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowModalID('" + "sasa" + "');", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>ShowWorkFlowFailMessage();</script>", false);
                    return 0;
                }
                else
                {
                    //SendMail();
                    return refId;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }


        }

        protected void wrkfSubmit_Click(object sender, EventArgs e)
        {
            WrkfSubmit(sender, e);
        }

        public void AddAdditionalComments(string[] addnComments)
        {
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            //WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
            WorkflowCore.CoreObjects.WorkFlowComment wrkfcmts = new WorkflowCore.CoreObjects.WorkFlowComment();
            wrkfcmts.ActionID = wrkfReq.ActionID;
            wrkfcmts.ApplicationID = wrkfReq.ApplicationID;
            wrkfcmts.ProcessID = wrkfReq.ProcessID;
            wrkfcmts.ReferenceID = RefID;
            wrkfcmts.UserPk = currentUser.PKUser;
            wrkfcmts.TaskID = wrkfReq.TaskID;
            for (int i = 0; i < addnComments.Length; i++)
            {
                if (addnComments[i].ToString() != string.Empty)
                {
                    wrkfcmts.WrkfComment = addnComments[i].ToString();
                    wrkfService.SaveComments(wrkfcmts);
                }
            }

        }


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

        public string GetFormattedCurrencyWithSeperation(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithSeperator.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }

        public void FillBudgetDetails(DataTable DtBudget)
        {
            divbudget.Visible = true;
            grdChangeBudget.DataSource = CommonFunctions.HtmlDecodeDataTable(DtBudget);
            grdChangeBudget.DataBind();
        }
    }
}