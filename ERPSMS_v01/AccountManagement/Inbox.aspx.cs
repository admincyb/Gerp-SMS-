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
 
namespace ERPSMS_v01.AccountManagement
{
    public partial class Inbox : System.Web.UI.Page
    {
        // Indicates the state as well as action
        ActionsEnum commonActions;
        User currentUser = new User();


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
                fromDateRange =Convert.ToInt32(dt.Rows[0]["ACF_VALUE"].ToString())*-1;
            }
        }

        private void FillSBU()
        {
            DataTable sbuDetails;
            sbuDetails = CommonBL.GetBizUnit(currentUser.PKUser, 0);
            if (sbuDetails.Rows.Count > 0)
            {
                ddlSbu.DataSource = sbuDetails;
                ddlSbu.DataTextField = GTIService.Constants.Common.Common.F_BIZUNITNAME;
                ddlSbu.DataValueField = GTIService.Constants.Common.Common.F_BIZUNIT;
                ddlSbu.DataBind();
                if (currentUser.SBUID != null)
                    ddlSbu.SelectedValue = currentUser.SBUID.ToString();
                currentUser = (User)(HttpContext.Current.User.Identity);
                FillDepartment(currentUser.PKUser, Convert.ToInt32(ddlSbu.SelectedValue));
            }
            else
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.HttpContext.Current.User = null;
               Response.Redirect("~/Login.aspx", true);
            }
        }

        private void FillDepartment(int userPK, int sbuID)
        {
            DataTable dtDept;
            ddlDepartment.Items.Clear();
            ddlDepartment.Items.Add(new ListItem("-All-", "-1"));
            ddlDepartment.AppendDataBoundItems = true;
            dtDept = CommonBL.GetDepartment(userPK, sbuID);
            ddlDepartment.DataSource = dtDept;
            ddlDepartment.DataTextField = GTIService.Constants.Common.Common.F_DEPARTMENTNAME;
            ddlDepartment.DataValueField = GTIService.Constants.Common.Common.F_DEPARTMENT;
            ddlDepartment.DataBind();
            FillProcess();
            FillTrxType();
        }

        private void FillInbox()
        {
            string fromDate, toDate;
            int type = 0;
            int totalCount = 0;
            int processID = 0;
            type = int.Parse(hdfInboxType.Value);
            if (type != 4)
                processID = int.Parse(ddlProcess.SelectedValue);
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
            if (type <3)
            {
                dsInbox = wrkfService.GetInboxTaskAndCompleted(currentUser.PKUser, fromDate, toDate, type, ucrPager.CurrentPage, pageSize, "WtlMtd", "Asc", processID, int.Parse(ddlDepartment.SelectedValue), int.Parse(ddlSbu.SelectedValue));
                dgInbox.Columns[4].Visible = true;
            }
            else if (type == 3)
            {
                dsInbox = wrkfService.GetIntimations(currentUser.PKUser, fromDate, toDate, ucrPager.CurrentPage, pageSize, "WtlMtd", "Asc", int.Parse(ddlDepartment.SelectedValue), int.Parse(ddlSbu.SelectedValue), processID);
                dgInbox.Columns[4].Visible = false;
            }
            else if (type == 4)
            {
                string typeCode = string.Empty;
                if (ddlTrxType.SelectedValue != "0")
                {
                    typeCode = ddlTrxType.SelectedValue;
                }
                dsInbox = BusinessLogic.AlertManagement.Alerts.GetAlertInbox(currentUser.PKUser,ucrPager.CurrentPage, pageSize, fromDate, toDate, typeCode, null, currentUser.SBUID);
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
                                     Message =HttpUtility.HtmlDecode(dr.Field<string>("intMsg")),
                                     RedirectUrl = (ConfigurationManager.AppSettings["ERP"] == null
                                                    || !dsInbox.Tables[1].Columns.Contains("PAG_SERVER")) ?
                                                    dr.Field<string>("PageUrl") 
                                                        :  dr.Field<string>("PAG_SERVER") + dr.Field<string>("PageUrl"),
                                     Revocable = dr.Field<int>("Revocable"),
                                     RefID = dr.Field<int>("wtdReference"),
                                     ProcessID = dr.Field<int>("wtdProcess"),
                                     ProcessDept = dr.Field<int>("prcDept"),
                                     TaskDate = dr.Field<string>("wtdDate"),
                                     Dept = dr.Field<string>("prcDeptText")
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
            else if(type==4)
            {
                if (dsInbox.Tables[0].Rows.Count > 0 )
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

        private void FillProcess()
        {
            ddlProcess.Items.Clear();
            DataTable dtProcess = new DataTable();
            dtProcess = CommonBL.GetProcessList(currentUser.PKUser, Convert.ToInt32(ddlDepartment.SelectedValue));
            ddlProcess.DataSource = dtProcess;
            ddlProcess.DataTextField = GTIService.Constants.Common.Common.F_VALUE;
            ddlProcess.DataValueField = GTIService.Constants.Common.Common.F_PK;
            ddlProcess.Items.Add(new ListItem("-Select-", "0"));
            ddlProcess.AppendDataBoundItems = true;
            ddlProcess.DataBind();
            ucrPager.CurrentPage = 1;
            FillInbox();

        }
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

            switch (commonActions)
            {

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
                        spnTask.Attributes.Add("class","tab-active");
                        ucrPager.CurrentPage = 1;
                        hdfInboxType.Value = "1";
                        dgAlerts.Visible = false;
                        dgInbox.Visible = true;
                        ddlProcess.Visible = true;
                        ddlTrxType.Visible = false;
                        ddlDepartment.Visible = true;
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
                        ddlProcess.Visible = true;
                        ddlTrxType.Visible = false;
                        ddlDepartment.Visible = true;
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
                        ddlProcess.Visible = true;
                        ddlTrxType.Visible = false;
                        ddlDepartment.Visible = true;
                        ddlSbu.Visible = true;
                        spnSBU.Visible = true;
                        spnDepartment.Visible = true;
                        spnProcess.Visible = true;
                        spnTrxType.Visible = false;
                        FillInbox();
                        ucrPager.FillPager();
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
                        ddlProcess.Visible = false;
                        ddlTrxType.Visible = true;
                        ddlDepartment.Visible = false;
                        ddlSbu.Visible = false;
                        spnSBU.Visible = false;
                        spnDepartment.Visible = false;
                        spnProcess.Visible = false;
                        spnTrxType.Visible = true;
                        FillInbox();
                        ucrPager.FillPager();
                        // Completed Task
                    }

                    break;
                #endregion
                #region DROPDOWN CHNAGE

                // Do Action when dropdown selected index changed
                case ActionsEnum.CHANGE:
                    if (((DropDownList)sender).ID == "ddlSbu")
                    {
                        currentUser = (User)(HttpContext.Current.User.Identity);
                        FillDepartment(currentUser.PKUser, Convert.ToInt32(ddlSbu.SelectedValue));
                    }
                    else if (((DropDownList)sender).ID == "ddlDepartment")
                    {
                        currentUser = (User)(HttpContext.Current.User.Identity);
                        FillProcess();
                        FillTrxType();



                    }
                    break;
                #endregion
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

        }

        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            HiddenField hdfProcessDept;
            int deptPK;
            if (e.CommandName == "Action")
            {
                hdfProcessDept = ((ImageButton)e.CommandSource).Parent.FindControl("hdfProcessDept") as HiddenField;
                if (hdfProcessDept != null && int.TryParse(hdfProcessDept.Value, out deptPK))
                {
                    MenuControl ctrlMenu = (MenuControl)this.Master.FindControl("MenuControl1");
                    if (ctrlMenu != null)
                    {
                        DropDownList ddlCostCenter = (DropDownList)ctrlMenu.FindControl("ddlCostCenter");
                        if (ddlCostCenter != null)
                        {
                            Session[SessionStrings.CurDept] = hdfProcessDept.Value;
                            ddlCostCenter.SelectedIndex = ddlCostCenter.Items.IndexOf(ddlCostCenter.Items.FindByValue(Session[SessionStrings.CurDept].ToString()));
                            ctrlMenu.FillMenu(true);
                        }
                    }
                }

                if (int.Parse(hdfInboxType.Value) == 1 || int.Parse(hdfInboxType.Value) == 3)
                {
                    if (ConfigurationManager.AppSettings["ERP"] != null)
                    {
                        string[] qryStr = e.CommandArgument.ToString().Split('?');
                        if(qryStr.Length>1)
                            Response.Redirect(e.CommandArgument.ToString() + "&ProcessDept=" + hdfProcessDept.Value, true);
                        else
                            Response.Redirect(e.CommandArgument.ToString() + "?ProcessDept=" + hdfProcessDept.Value, true);
                    }
                    else
                    {
                        Response.Redirect("~" + e.CommandArgument.ToString() , true);
                    }

                }
                else if(int.Parse(hdfInboxType.Value) == 2)
                {
                    if (ConfigurationManager.AppSettings["ERP"] != null)
                    {
                        Response.Redirect(e.CommandArgument.ToString() + "&Flag=1&ProcessDept=" + hdfProcessDept.Value, true);
                    }
                    else
                    {
                        Response.Redirect("~" + e.CommandArgument.ToString() + "&Flag=1", true);
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

    }

}
