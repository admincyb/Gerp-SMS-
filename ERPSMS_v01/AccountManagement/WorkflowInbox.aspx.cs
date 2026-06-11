using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using System.Data;
using WorkflowCore;
using BusinessLogic.CommonManagement;
using BusinessLogic.AccountManagement;
using BusinessObject;
using System.Configuration;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using System.Globalization;
using System.Threading;


namespace ERPSMS_v01.AccountManagement
{
    public partial class WorkflowInbox : System.Web.UI.Page
    {
        // Indicates the state as well as action
        ActionsEnum commonActions;
        User currentUser = new User();
        private WorkflowInboxBO objWorkflowInbox;
        List<ERP.Utilities.ClientCulture> lstLanguage;

        //Production.
        private static readonly int pageSize = 25;
        private int fromDateRange = -1;

        protected void Page_Init(object sender, System.EventArgs e)
        {
            ucrPager.CurrentPage = 1;
        }

        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
            //Initialze the current logged in user to the currentUser variable
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                currentUser = (User)HttpContext.Current.User.Identity;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AddDate", "$(document).ready(function () {PageInt();});", true);
            }
            else
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.HttpContext.Current.User = null;
                Response.Redirect("~/Login.aspx", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageVariables();
            if (!IsPostBack)
            {
                SaveMessageTime();
                FillSBU();
                FillLanguage();
                //   ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "play_song('notify.wav');", true);
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;
                // Set the User Theme
                if (!string.IsNullOrEmpty(currentUser.Theme))
                {
                    Page.Theme = currentUser.Theme;
                }
                else
                {
                    Page.Theme = "ClassicExt";
                }
                //Page.Theme = "NewTheme";
            }
            else
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.HttpContext.Current.User = null;
                Response.Redirect("~/Login.aspx", true);
            }

        }

        private void SaveMessageTime()
        {
            int result = 0;
            currentUser = (User)(HttpContext.Current.User.Identity);
            result = CommonBL.SaveMessageTime(currentUser.PKUser, null);
            ConfigurationSettings();
            //PeriodFrom.Text = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);
            PeriodFrom.Text = DateTime.Now.AddMonths(fromDateRange).AddDays(1 - DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);
            //   defaultDate == DateDefaultEnum.FirstDate ? 1 - DateTime.Now.Day : -DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);
            PeriodTo.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
        }
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("INBOX DATE RANGE", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                fromDateRange = Convert.ToInt32(dt.Rows[0]["ACF_VALUE"].ToString()) * -1;
            }
        }

        private void FillSBU()
        {
            DataTable sbuDetails;
            sbuDetails = CommonBL.GetBizUnit(currentUser.PKUser, 0);
            ddlSbu.Items.Clear();
            if (sbuDetails.Rows.Count > 1) ddlSbu.Items.Add(new ListItem("-All-", "-1"));
            ddlSbu.AppendDataBoundItems = true;

            if (sbuDetails.Rows.Count > 0)
            {
                ddlSbu.DataSource = sbuDetails;
                ddlSbu.DataTextField = GTIService.Constants.Common.Common.F_BIZUNITNAME;
                ddlSbu.DataValueField = GTIService.Constants.Common.Common.F_BIZUNIT;
                ddlSbu.DataBind();
                if (currentUser.SBUID > 0)
                    ddlSbu.SelectedValue = "-1";//CommonConstants.SELECT_VALUE_ZERO;//Default SBU should be 'All'// currentUser.SBUID.ToString();
                currentUser = (User)(HttpContext.Current.User.Identity);
                // FillDepartment(currentUser.PKUser, Convert.ToInt32(ddlSbu.SelectedValue));
                FillInbox();
                FillTrxType();
            }
            else
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.HttpContext.Current.User = null;
                Response.Redirect("~/Login.aspx", true);
            }
        }

        //private void FillDepartment(int userPK, int sbuID)
        //{
        //    DataTable dtDept;
        //    ddlDepartment.Items.Clear();
        //    ddlDepartment.Items.Add(new ListItem("-All-", "-1"));
        //    ddlDepartment.AppendDataBoundItems = true;
        //   dtDept = CommonBL.GetDepartment(userPK, sbuID);
        //    ddlDepartment.DataSource = dtDept;
        //    ddlDepartment.DataTextField = GTIService.Constants.Common.Common.F_DEPARTMENTNAME;
        //    ddlDepartment.DataValueField = GTIService.Constants.Common.Common.F_DEPARTMENT;
        //    ddlDepartment.DataBind();
        //    //FillProcess();
        //    FillInbox();
        //    FillTrxType();
        //}

        private void FillInbox()
        {
            string fromDate, toDate;
            int type = 0;
            int totalCount = 0;
            int processID = 0;

            type = int.Parse(hdfInboxType.Value);
            if (type != 4)
                processID = int.Parse(hdfProcess.Value);
            if (PeriodFrom.Text == string.Empty)
            {
                fromDate = (Convert.ToDateTime(System.DateTime.Now.Month + "/" + "01" + "/" + System.DateTime.Now.Year)).ToString("dd-MMM-yyyy");
                toDate = (Convert.ToDateTime(System.DateTime.Now.ToShortDateString())).ToString("dd-MMM-yyyy");
                PeriodFrom.Text = fromDate;
                hdfPrdFrm.Value = fromDate;
                PeriodTo.Text = toDate;
                hdfPrdTo.Value = toDate;
            }
            else
            {
                fromDate = PeriodFrom.Text;
                toDate = PeriodTo.Text;
            }

            CoreService wrkfService = new CoreService();
            DataSet dsInbox = new DataSet();
            if (type < 3)
            {
                dsInbox = wrkfService.GetInboxTaskAndCompleted(currentUser.PKUser, fromDate, toDate, type, ucrPager.CurrentPage, pageSize, "WtlMtd", "Asc", processID, int.Parse(hdnDept.Value), int.Parse(ddlSbu.SelectedValue));
                dgInbox.Columns[4].Visible = true;
            }
            else if (type == 3)
            {
                dsInbox = wrkfService.GetIntimations(currentUser.PKUser, fromDate, toDate, ucrPager.CurrentPage, pageSize, "WtlMtd", "Asc", int.Parse(hdnDept.Value), int.Parse(ddlSbu.SelectedValue), processID);
                dgInbox.Columns[4].Visible = false;
            }
            else if (type == 4)
            {
                string typeCode = string.Empty;
                if (ddlTrxType.SelectedValue != "0")
                {
                    typeCode = ddlTrxType.SelectedValue;
                }
                dsInbox = BusinessLogic.AlertManagement.Alerts.GetAlertInbox(currentUser.PKUser, ucrPager.CurrentPage, pageSize, fromDate, toDate, typeCode, null, currentUser.SBUID);
            }

            if (type < 4)
            {

                if (dsInbox.Tables.Count > 0 && dsInbox.Tables[1].Rows.Count > 0)
                {
                    var result = from dr in dsInbox.Tables[1].AsEnumerable()
                                 select new
                                 {
                                     ProcessName = dr.Field<string>("prcName"),
                                     GroupName = dr.Field<string>("GrpName"),
                                     TaskName = dr.Field<string>("tskName"),
                                     Message = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(dr.Field<string>("intMsg"))),
                                     //Message = HttpUtility.HtmlDecode(dr.Field<string>("intMsg")),
                                     RedirectUrl = (ConfigurationManager.AppSettings["ERP"] == null
                                     || !dsInbox.Tables[1].Columns.Contains("PAG_SERVER")) ?
                                     dr.Field<string>("PageUrl") : (dr.Field<string>("PAG_SERVER") + dr.Field<string>("PageUrl")
                                      + (dr.Field<string>("PAG_SERVER") != null && dr.Field<string>("PAG_SERVER").Replace('/', ' ').Trim().ToLower() != ConfigurationManager.AppSettings["VirtualDirectory"].Replace('/', ' ').Trim().ToLower() ? "&MnChange=1" : "")),
                                     Revocable = dr.Field<int>("Revocable"),
                                     RefID = dr.Field<int>("wtdReference"),
                                     ProcessID = dr.Field<int>("wtdProcess"),
                                     ProcessDept = dr.Field<int>("prcDept"),
                                     TaskDate = dr.Field<string>("wtdDate"),
                                     Dept = dr.Field<string>("prcDeptText"),
                                     Comment = Server.HtmlDecode(dr.Field<string>("wtdComment")),
                                     NextTaskAction = dr.Field<string>("wtdNextTaskAction"),
                                     IsApproveVisible = GetGlobalResourceObject("ConfigurationsRes", "IsDirectApproveVisible").ToString() == "0" ? 0 : (hdfInboxType.Value == "2" || string.IsNullOrEmpty(dr.Field<string>("wtdNextTaskAction"))
                                                        || dr.Field<string>("PageUrl").Contains("PRefID=")) ? 0 : 1,
                                     PrcIsInboxPrint = dr.Field<byte>("prcIsInboxPrint"),
                                     PrcAppType = dr.Field<string>("prcAppType"),
                                     RefApplication = dr.Field<int>("refApplication"),
                                     PageServer = dsInbox.Tables[1].Columns.Contains("PAG_SERVER") ?
                                     dr.Field<string>("PAG_SERVER") : string.Empty

                                 };

                    dgInbox.DataSource = result;
                    dgInbox.DataBind();
                    totalCount = int.Parse(dsInbox.Tables[1].Rows[0]["TOTAL_ROW_COUNT"].ToString());
                    if (totalCount > 0)
                    {
                        ucrPager.Visible = true;
                        if ((totalCount % pageSize) == 0)
                            ucrPager.TotalPages = totalCount / pageSize;
                        else
                            ucrPager.TotalPages = (totalCount / pageSize) + 1;

                        if ((dgInbox.PageIndex <= ucrPager.TotalPages) && (ucrPager.TotalPages > 1))
                        {
                            ucrPager.NextButtonEnabled = true;
                        }
                    }
                    else
                    {
                        ucrPager.Visible = false;
                        ucrPager.TotalPages = 1;
                    }
                }
                else
                {
                    dgInbox.DataSource = null;
                    dgInbox.DataBind();
                    ucrPager.Visible = false;
                    ucrPager.TotalPages = 1;
                }
            }
            else if (type == 4)
            {
                if (dsInbox.Tables[0].Rows.Count > 0)
                {
                    var result = from dr in dsInbox.Tables[0].AsEnumerable()
                                 select new
                                 {
                                     DueDate = dr.Field<DateTime>("ATH_DUE_DATE") != null ? dr.Field<DateTime>("ATH_DUE_DATE").ToString(Resources.Constants.DateFormatShort) : string.Empty,
                                     AlertName = dr.Field<string>("ATH_NAME"),
                                     Type = dr.Field<string>("ATH_TRX_TYPE_TEXT"),
                                     AlertOn = dr.Field<DateTime>("ATH_ALERT_ON") != null ? dr.Field<DateTime>("ATH_ALERT_ON").ToString(Resources.Constants.DateFormatShort) : string.Empty,
                                     Message = HttpUtility.HtmlDecode(dr.Field<string>("TRX_NO_TEXT")),
                                     Status = dr.Field<string>("ATH_STATUS_TEXT")
                                 };
                    dgAlerts.DataSource = result;
                    dgAlerts.DataBind();
                    totalCount = int.Parse(dsInbox.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString());
                    if (totalCount > 0)
                    {
                        ucrPager.Visible = true;
                        if ((totalCount % pageSize) == 0)
                            ucrPager.TotalPages = totalCount / pageSize;
                        else
                            ucrPager.TotalPages = (totalCount / pageSize) + 1;

                        if ((dgAlerts.PageIndex <= ucrPager.TotalPages) && (ucrPager.TotalPages > 1))
                        {
                            ucrPager.NextButtonEnabled = true;
                        }
                    }
                    else
                    {
                        ucrPager.Visible = false;
                        ucrPager.TotalPages = 1;
                    }
                }
                else
                {
                    dgAlerts.DataSource = null;
                    dgAlerts.DataBind();
                    ucrPager.Visible = false;
                    ucrPager.TotalPages = 1;
                }
            }

            ucrPager.BindPager();
        }

        private void FillLanguage()
        {
            HttpCookie cookie = Request.Cookies["Culture"];
            lstLanguage = new List<ERP.Utilities.ClientCulture>();
            lstLanguage = ERP.Utilities.CommonFunctions.GetClientCulture(GetGlobalResourceObject("ConfigurationsRes", "Culture").ToString());
            ddlLanguage.DataSource = lstLanguage;
            ddlLanguage.DataTextField = GTIService.Constants.Common.Common.F_LNG_KEY;
            ddlLanguage.DataValueField = GTIService.Constants.Common.Common.F_LNG_VAL;
            ddlLanguage.DataBind();
            ddlLanguage.SelectedIndex = ddlLanguage.Items.IndexOf(ddlLanguage.Items.FindByValue(cookie == null ? currentUser.UserCulture : cookie.Value));
        }
        //  private void FillProcess()
        //  {
        //ddlProcess.Items.Clear();
        //DataTable dtProcess = new DataTable();
        //dtProcess = CommonBL.GetProcessList(currentUser.PKUser, Convert.ToInt32(ddlDepartment.SelectedValue));
        //txtProcess.DataSource = dtProcess;
        //txtProcess.DataTextField = GTIService.Constants.Common.Common.F_VALUE;
        //ddlProcess.DataValueField = GTIService.Constants.Common.Common.F_PK;
        //ddlProcess.Items.Add(new ListItem("-Select-", "0"));
        //ddlProcess.AppendDataBoundItems = true;
        //ddlProcess.DataBind();
        //ucrPager.CurrentPage = 1;
        //    FillInbox();

        //}
        private void FillTrxType()
        {
            ddlTrxType.Items.Clear();
            DataTable dtTrxTypeList = new DataTable();
            dtTrxTypeList = CommonBL.GetTrxTypeList(0, null, Convert.ToByte(DbActiveStatus.ACTIVE), currentUser.SBUID, "INBOX");
            ddlTrxType.DataSource = dtTrxTypeList;
            ddlTrxType.DataTextField = GTIService.Constants.Common.Common.APT_NAME;
            ddlTrxType.DataValueField = GTIService.Constants.Common.Common.APT_CODE;
            ddlTrxType.Items.Add(new ListItem("-Select-", "0"));
            ddlTrxType.AppendDataBoundItems = true;
            ddlTrxType.DataBind();
            ucrPager.CurrentPage = 1;
            //FillInbox();
        }

        protected void ActionHandler(object sender, EventArgs e)
        {
            if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
            {
                commonActions = ActionsEnum.CHANGE;
            }
            else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
            {
                commonActions = ActionsEnum.LINKBUTTONCLICK;
            }
            if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
            }

            switch (commonActions)
            {
                #region Quick Approval Popup
                case ActionsEnum.APPROVEITEM:
                    int deptPK;
                    int refID = 0;
                    DataTable dtWorkflow;
                    int result = 0;
                    int _actionID = 0;
                    int _processID = 0;
                    int _taskID = 0;
                    int _applicationID = 0;
                    int _userPK = 0;
                    int _referrenceID = 0;
                    if (hdfPopupRefPK != null && int.TryParse(hdfPopupRefPK.Value, out refID))
                    {
                        objWorkflowInbox = new WorkflowInboxBO();
                        objWorkflowInbox.RefID = Convert.ToInt32(hdfPopupRefPK.Value);
                        dtWorkflow = new DataTable();
                        dtWorkflow = WorkflowInboxBL.GetWorkflowDetails(objWorkflowInbox, currentUser);
                        if (dtWorkflow != null && dtWorkflow.Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdTaskAction"].ToString()))
                            {
                                _actionID = Convert.ToInt32(dtWorkflow.Rows[0]["wtdTaskAction"]);
                            }
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdProcess"].ToString()))
                            {
                                _processID = Convert.ToInt32(dtWorkflow.Rows[0]["wtdProcess"]);
                            }
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdTask"].ToString()))
                            {
                                _taskID = Convert.ToInt32(dtWorkflow.Rows[0]["wtdTask"]);
                            }
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdApplication"].ToString()))
                            {
                                _applicationID = Convert.ToInt32(dtWorkflow.Rows[0]["wtdApplication"]);
                            }
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdUser"].ToString()))
                            {
                                _userPK = Convert.ToInt32(dtWorkflow.Rows[0]["wtdUser"]);
                            }
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdReference"].ToString()))
                            {
                                _referrenceID = Convert.ToInt32(dtWorkflow.Rows[0]["wtdReference"]);
                            }

                            result = ucrWrkf.DoWorkFlow(_actionID, _processID, _taskID, _applicationID, _userPK, _referrenceID);
                            if (result > 0)
                            {
                                // Show Workflow Message and bind grid                               

                                //if (!string.IsNullOrEmpty(hdfNextTaskAction.Value))
                                //{
                                //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Success;
                                //    litErrorMsg.Text = string.Format(litErrorMsg.Text, hdfNextTaskAction.Value);
                                //}
                                //else
                                //{
                                //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Approve_Success;
                                //    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SelectedItem);
                                //}
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Wrkflw_Action;
                                FillInbox();
                                ucrPager.FillPager();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                       + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Approve_Error;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                       + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Approve_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "');", true);
                        }
                    }

                    break;
                #endregion
                #region LinButtonClick
                case ActionsEnum.LINKBUTTONCLICK:

                    //lbnCompletedTask.CssClass = "tab-inactive";
                    //lbnTask.CssClass = "tab-inactive";
                    //lbnIntimations.CssClass = "tab-inactive";
                    spnCompletedTask.Attributes.Add("class", "tab-inactive");
                    spnTask.Attributes.Add("class", "tab-inactive");
                    spnIntimation.Attributes.Add("class", "tab-inactive");
                    spnAlerts.Attributes.Add("class", "tab-inactive");
                    if (((LinkButton)sender).ID == "lbnTask")
                    {
                        spnTask.Attributes.Add("class", "tab-active");
                        ucrPager.CurrentPage = 1;
                        hdfInboxType.Value = "1";
                        dgAlerts.Visible = false;
                        dgInbox.Visible = true;
                        txtdept.Visible = true;
                        txtProcess.Visible = true;
                        //   ddlProcess.Visible = true;
                        ddlTrxType.Visible = false;
                        //    ddlDepartment.Visible = true;
                        ddlSbu.Visible = true;
                        spnSBU.Visible = true;
                        spnDepartment.Visible = true;
                        spnProcess.Visible = true;
                        spnTrxType.Visible = false;
                        FillInbox();
                        ucrPager.FillPager();


                    }
                    else if (((LinkButton)sender).ID == "lbnIntimations")
                    {
                        //lbnIntimations.CssClass = "tab-active";
                        spnIntimation.Attributes.Add("class", "tab-active");
                        ucrPager.CurrentPage = 1;
                        hdfInboxType.Value = "3";
                        dgAlerts.Visible = false;
                        dgInbox.Visible = true;
                        txtdept.Visible = true;
                        txtProcess.Visible = true;
                        //       ddlProcess.Visible = true;
                        ddlTrxType.Visible = false;
                        //     ddlDepartment.Visible = true;
                        ddlSbu.Visible = true;
                        spnSBU.Visible = true;
                        spnDepartment.Visible = true;
                        spnProcess.Visible = true;
                        spnTrxType.Visible = false;
                        FillInbox();
                        ucrPager.FillPager();

                    }
                    else if (((LinkButton)sender).ID == "lbnCompletedTask")
                    {
                        //lbnCompletedTask.CssClass = "tab-active";
                        spnCompletedTask.Attributes.Add("class", "tab-active");
                        ucrPager.CurrentPage = 1;
                        hdfInboxType.Value = "2";
                        dgAlerts.Visible = false;
                        dgInbox.Visible = true;
                        hdnDept.Value = "0";
                        hdfProcess.Value = "0";
                        txtdept.Text = string.Empty;
                        txtProcess.Text = string.Empty;
                        txtdept.Visible = true;
                        txtProcess.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitAuto", "$(document).ready(function(){InitAuto()});", true);
                        //      ddlProcess.Visible = true;
                        ddlTrxType.Visible = false;
                        //   ddlDepartment.Visible = true;
                        ddlSbu.Visible = true;
                        spnSBU.Visible = true;
                        spnDepartment.Visible = true;
                        spnProcess.Visible = true;
                        spnTrxType.Visible = false;
                        //   FillInbox();
                        dgInbox.DataSource = null;
                        dgInbox.DataBind();
                        ucrPager.Visible = false;
                        ucrPager.TotalPages = 0;
                        // Completed Task
                    }
                    else if (((LinkButton)sender).ID == "lbnAlerts")
                    {
                        //lbnCompletedTask.CssClass = "tab-active";
                        spnAlerts.Attributes.Add("class", "tab-active");
                        ucrPager.CurrentPage = 1;
                        hdfInboxType.Value = "4";
                        dgAlerts.Visible = true;
                        dgInbox.Visible = false;
                        txtdept.Visible = false;
                        txtProcess.Visible = false;
                        //        ddlProcess.Visible = false;
                        ddlTrxType.Visible = true;
                        // ddlDepartment.Visible = false;
                        ddlSbu.Visible = false;
                        spnSBU.Visible = false;
                        spnDepartment.Visible = false;
                        spnProcess.Visible = false;
                        spnTrxType.Visible = true;
                        FillInbox();
                        ucrPager.FillPager();
                        // Completed Taska
                    }
                    //else if (((LinkButton)sender).CommandName == "Culture")
                    //{
                    //    LinkButton lnkBtn = (LinkButton)sender;
                    //    System.Configuration.ConfigurationManager.AppSettings["Culture"] = lnkBtn.CommandArgument;
                    //    Server.Transfer(Request.Url.PathAndQuery, false);
                    //    break;
                    //}
                    break;
                #endregion
                #region DROPDOWN CHNAGE

                // Do Action when dropdown selected index changed
                case ActionsEnum.CHANGE:

                    if (((DropDownList)sender).ID == "ddlSbu")
                    {
                        currentUser = (User)(HttpContext.Current.User.Identity);
                        //      FillDepartment(currentUser.PKUser, Convert.ToInt32(ddlSbu.SelectedValue));
                    }
                    //else if (((DropDownList)sender).ID == "ddlDepartment")
                    //{
                    //    currentUser = (User)(HttpContext.Current.User.Identity);
                    //  //  FillProcess();
                    //    FillTrxType();
                    //}
                    else if (((DropDownList)sender).ID == "ddlLanguage")
                    {
                        HttpCookie cookie = Request.Cookies["Culture"];
                        if (cookie == null)
                        {
                            // no cookie found, create it
                            cookie = new HttpCookie("Culture");
                            cookie.Value = ddlLanguage.SelectedValue;
                        }
                        else
                        {
                            Request.Cookies.Remove("Culture");
                            // update the cookie values
                            cookie.Value = ddlLanguage.SelectedValue;
                        }
                        cookie.Expires = DateTime.UtcNow.AddDays(1);
                        Response.Cookies.Add(cookie);
                        Response.Redirect(Request.Url.PathAndQuery, false);
                    }
                    break;
                #endregion


            }
        }

        /// <summary>
        /// Method to redirect to corresponding pages
        /// </summary>
        /// <param name="ProcessDept"></param>
        /// <param name="RedirectUrl"></param>
        private void DoAction(string ProcessDept, string RedirectUrl)
        {
            int deptPK;
            if (ProcessDept != null && int.TryParse(ProcessDept, out deptPK))
            {
                MenuControl ctrlMenu = (MenuControl)this.Master.FindControl("MenuControl1");
                if (ctrlMenu != null)
                {
                    DropDownList ddlCostCenter = (DropDownList)ctrlMenu.FindControl("ddlCostCenter");
                    if (ddlCostCenter != null)
                    {
                        Session[SessionStrings.CurDept] = ProcessDept;
                        ddlCostCenter.SelectedIndex = ddlCostCenter.Items.IndexOf(ddlCostCenter.Items.FindByValue(Session[SessionStrings.CurDept].ToString()));
                        ctrlMenu.FillMenu(true);
                    }
                }
            }

            if (int.Parse(hdfInboxType.Value) == 1 || int.Parse(hdfInboxType.Value) == 3)
            {
                if (ConfigurationManager.AppSettings["ERP"] != null)
                {
                    string[] qryStr = RedirectUrl.ToString().Split('?');
                    if (qryStr.Length > 1)
                        Response.Redirect(RedirectUrl.ToString() + "&ProcessDept=" + ProcessDept + "&Dep=" + ProcessDept, true);
                    else
                        Response.Redirect(RedirectUrl.ToString() + "?ProcessDept=" + ProcessDept + "&Dep=" + ProcessDept, true);
                }
                else
                {
                    Response.Redirect("~" + RedirectUrl.ToString() + "&Dep=" + ProcessDept, true);
                }

            }
            else if (int.Parse(hdfInboxType.Value) == 2)
            {
                if (ConfigurationManager.AppSettings["ERP"] != null)
                {
                    Response.Redirect(RedirectUrl.ToString() + "&Flag=1&ProcessDept=" + ProcessDept + "&Dep=" + ProcessDept, true);
                }
                else
                {
                    Response.Redirect("~" + RedirectUrl.ToString() + "&Flag=1" + "&Dep=" + ProcessDept, true);
                }
            }
        }

        protected void ActionHandler(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "imbSearch")
            {
                ucrPager.CurrentPage = 1;
                FillInbox();
                ucrPager.FillPager();
            }
            //else
            //    if (((ImageButton)sender).ID == "imbPrint")
            //    {
            //        PrintProcess(hdnAppType.Value);
            //    }
            #region Action Popup
            else if (((ImageButton)sender).ID == "imgPopupAction")
                if (!string.IsNullOrEmpty(hdfProcessDeptPopup.Value) && !string.IsNullOrEmpty(hdfRedirectUrlPopup.Value))
                    DoAction(hdfProcessDeptPopup.Value, hdfRedirectUrlPopup.Value);
            #endregion

        }

        //private void PrintProcess(string appType)
        //{
        //    string reportURL = string.Empty;
        //    switch (appType)
        //    {
        //        case ApplicationType.SWO:
        //            // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
        //            reportURL = hdnPageServer.Value + "/Reports/GenerateReport.aspx?ID=" + hdnApplicationPK.Value + "&APPTYPE=SWO&APPSUBTYPE=4" + "&ProcessDept=" + hdfProcessDeptPopup.Value + "&Dep=" + hdfProcessDeptPopup.Value;
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();OpenPDF(\"" + reportURL + "\");", true);
        //            break;
        //    }
        //}
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            HiddenField hdfProcessDept;
            HiddenField hdfRefID;
            //HiddenField hdfNextTaskAction;
            int deptPK;
            int refID = 0;
            DataTable dtWorkflow;
            int result = 0;
            int _actionID = 0;
            int _processID = 0;
            int _taskID = 0;
            int _applicationID = 0;
            int _userPK = 0;
            int _referrenceID = 0;
            if (e.CommandName == "Action")
            {

                hdfProcessDept = ((ImageButton)e.CommandSource).Parent.FindControl("hdfProcessDept") as HiddenField;
                DoAction(hdfProcessDept.Value, e.CommandArgument.ToString());
                //if (hdfProcessDept != null && int.TryParse(hdfProcessDept.Value, out deptPK))
                //{
                //    MenuControl ctrlMenu = (MenuControl)this.Master.FindControl("MenuControl1");
                //    if (ctrlMenu != null)
                //    {
                //        DropDownList ddlCostCenter = (DropDownList)ctrlMenu.FindControl("ddlCostCenter");
                //        if (ddlCostCenter != null)
                //        {
                //            Session[SessionStrings.CurDept] = hdfProcessDept.Value;
                //            ddlCostCenter.SelectedIndex = ddlCostCenter.Items.IndexOf(ddlCostCenter.Items.FindByValue(Session[SessionStrings.CurDept].ToString()));
                //            ctrlMenu.FillMenu(true);
                //        }
                //    }
                //}

                //if (int.Parse(hdfInboxType.Value) == 1 || int.Parse(hdfInboxType.Value) == 3)
                //{
                //    if (ConfigurationManager.AppSettings["ERP"] != null)
                //    {
                //        string[] qryStr = e.CommandArgument.ToString().Split('?');
                //        if (qryStr.Length > 1)
                //            Response.Redirect(e.CommandArgument.ToString() + "&ProcessDept=" + hdfProcessDept.Value + "&Dep=" + hdfProcessDept.Value, true);
                //        else
                //            Response.Redirect(e.CommandArgument.ToString() + "?ProcessDept=" + hdfProcessDept.Value + "&Dep=" + hdfProcessDept.Value, true);
                //    }
                //    else
                //    {
                //        Response.Redirect("~" + e.CommandArgument.ToString() + "&Dep=" + hdfProcessDept.Value, true);
                //    }

                //}
                //else if (int.Parse(hdfInboxType.Value) == 2)
                //{
                //    if (ConfigurationManager.AppSettings["ERP"] != null)
                //    {
                //        Response.Redirect(e.CommandArgument.ToString() + "&Flag=1&ProcessDept=" + hdfProcessDept.Value + "&Dep=" + hdfProcessDept.Value, true);
                //    }
                //    else
                //    {
                //        Response.Redirect("~" + e.CommandArgument.ToString() + "&Flag=1" + "&Dep=" + hdfProcessDept.Value, true);
                //    }
                //}
            }
            if (e.CommandName == ActionsEnum.APPROVEITEM.ToString())
            {
                if (IsValid)
                {
                    hdfRefID = ((ImageButton)e.CommandSource).Parent.FindControl("hdfRefID") as HiddenField;
                    //hdfNextTaskAction = ((ImageButton)e.CommandSource).Parent.FindControl("hdfNextTaskAction") as HiddenField;
                    if (hdfRefID != null && int.TryParse(hdfRefID.Value, out refID))
                    {
                        objWorkflowInbox = new WorkflowInboxBO();
                        objWorkflowInbox.RefID = Convert.ToInt32(hdfRefID.Value);
                        dtWorkflow = new DataTable();
                        dtWorkflow = WorkflowInboxBL.GetWorkflowDetails(objWorkflowInbox, currentUser);
                        if (dtWorkflow != null && dtWorkflow.Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdTaskAction"].ToString()))
                            {
                                _actionID = Convert.ToInt32(dtWorkflow.Rows[0]["wtdTaskAction"]);
                            }
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdProcess"].ToString()))
                            {
                                _processID = Convert.ToInt32(dtWorkflow.Rows[0]["wtdProcess"]);
                            }
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdTask"].ToString()))
                            {
                                _taskID = Convert.ToInt32(dtWorkflow.Rows[0]["wtdTask"]);
                            }
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdApplication"].ToString()))
                            {
                                _applicationID = Convert.ToInt32(dtWorkflow.Rows[0]["wtdApplication"]);
                            }
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdUser"].ToString()))
                            {
                                _userPK = Convert.ToInt32(dtWorkflow.Rows[0]["wtdUser"]);
                            }
                            if (!string.IsNullOrEmpty(dtWorkflow.Rows[0]["wtdReference"].ToString()))
                            {
                                _referrenceID = Convert.ToInt32(dtWorkflow.Rows[0]["wtdReference"]);
                            }

                            result = ucrWrkf.DoWorkFlow(_actionID, _processID, _taskID, _applicationID, _userPK, _referrenceID);
                            if (result > 0)
                            {
                                // Show Workflow Message and bind grid                               

                                //if (!string.IsNullOrEmpty(hdfNextTaskAction.Value))
                                //{
                                //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Success;
                                //    litErrorMsg.Text = string.Format(litErrorMsg.Text, hdfNextTaskAction.Value);
                                //}
                                //else
                                //{
                                //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Approve_Success;
                                //    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SelectedItem);
                                //}
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Wrkflw_Action;
                                FillInbox();
                                ucrPager.FillPager();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                       + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Approve_Error;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                       + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Approve_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "');", true);
                        }
                    }
                }
            }

        }


        protected void PageChanged(object sender, DataNavigatorEventArgs e)
        {
            ucrPager.CurrentPage = e.CurrentPage;
            FillInbox();
            EnableDisableButtons(e.TotalPages);
        }

        protected void FirstPage(object sender, DataNavigatorEventArgs e)
        {
            // Decrement the current page index.
            if (e.CurrentPage > 1)
            {
                ucrPager.CurrentPage = 1;

                // Get the data for the DataGrid.
                FillInbox();

                EnableDisableButtons(e.TotalPages);
            }
        }

        protected void PreviousPage(object sender, DataNavigatorEventArgs e)
        {
            // Decrement the current page index.
            if (e.CurrentPage > 1)
            {
                ucrPager.CurrentPage--;

                // Get the data for the DataGrid.
                FillInbox();

                EnableDisableButtons(e.TotalPages);
            }
        }

        protected void NextPage(object sender, DataNavigatorEventArgs e)
        {
            // Decrement the current page index.
            if (e.CurrentPage <= e.TotalPages)
            {
                ucrPager.CurrentPage++;

                // Get the data for the DataGrid.
                FillInbox();

                EnableDisableButtons(e.TotalPages);
            }
        }

        protected void LastPage(object sender, DataNavigatorEventArgs e)
        {
            // Decrement the current page index.
            if (e.CurrentPage <= e.TotalPages)
            {
                ucrPager.CurrentPage = e.TotalPages;

                // Get the data for the DataGrid.
                FillInbox();

                EnableDisableButtons(e.TotalPages);
            }
        }

        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?

            // Should we disable the previous link?
            ucrPager.PreviousButtonEnabled = (ucrPager.CurrentPage == 1) ? false : true;
            //ucrPager.PreviousButtonImageUrl = (ucrPager.CurrentPage == 1) ? "Images/NavPreviousPageDisabled.gif" : "Images/NavPreviousPage.gif";

            // Should we enable the next link?
            ucrPager.NextButtonEnabled = (ucrPager.CurrentPage < iTotalPages) ? true : false;
            //ucrPager.NextButtonImageUrl = (ucrPager.CurrentPage < iTotalPages) ? "Images/NavNextPage.gif" : "Images/NavNextPageDisabled.gif";

        }

        /// <summary>
        /// for set design for culture change
        /// </summary>

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ucrPager.FirstPage += new FirstPageEventHandler(this.FirstPage);
            this.ucrPager.PreviousPage += new PreviousPageEventHandler(this.PreviousPage);
            this.ucrPager.NextPage += new NextPageEventHandler(this.NextPage);
            this.ucrPager.LastPage += new LastPageEventHandler(this.LastPage);
            this.ucrPager.PageChanged += new PageChangedEventHandler(this.PageChanged);
            this.Init += new EventHandler(this.Page_Init);
        }

        #endregion

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideTabs", "$(document).ready(function(){ShowHideTabs();InitAuto()});", true);
        }
        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            var culture = CultureInfo.CreateSpecificCulture(GetGlobalResourceObject("ConfigurationsRes", "DefaultCulture").ToString());
            Thread.CurrentThread.CurrentCulture = culture;

            HttpCookie userCulture = Request.Cookies["Culture"];
            culture = userCulture != null ?
              CultureInfo.CreateSpecificCulture(userCulture.Value) :
              CultureInfo.CreateSpecificCulture(GetGlobalResourceObject("ConfigurationsRes", "DefaultCulture").ToString());
            Page.UICulture = culture.ToString();
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}