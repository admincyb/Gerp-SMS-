using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using MailCore;
using MailCore.BO;
using MailCore.Utility;
using MailCore;
using System.Configuration;
using MailSendCore;
using System.Reflection;
using BusinessLogic.CommonManagement;
using BusinessObject;
using BusinessLogic.Administration.Configurations;
namespace ERPSMS_v01.UserControl
{
    public partial class Mail : System.Web.UI.UserControl
    {
        #region Variables and Properties

        #region Properties

        /// <summary>
        /// To maintain the sort order in viewstate
        /// </summary>
        private string SortOrder
        {
            get
            {
                return (string)this.ViewState["SortOrder"];
            }
            set
            {
                this.ViewState["SortOrder"] = value;
            }
        }
        /// <summary>
        /// To maintain the UserCode in viewstate
        /// </summary>
        private string UserCode
        {
            get
            {
                return (string)this.ViewState["UserCode"];
            }
            set
            {
                this.ViewState["UserCode"] = value;
            }
        }
        /// <summary>
        /// To maintain the sort expression in viewstate
        /// </summary>
        private string SortExpression
        {
            get
            {
                return (string)this.ViewState["SortExpression"];
            }
            set
            {
                this.ViewState["SortExpression"] = value;
            }
        }
        /// <summary>
        /// To maintain the sort field in viewstate
        /// </summary>
        private string SortFilter
        {
            get
            {
                return (string)this.ViewState["SortFilter"];
            }
            set
            {
                this.ViewState["SortFilter"] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndex
        {
            get
            {
                return (string)this.ViewState["PageIndex"];
            }
            set
            {
                this.ViewState["PageIndex"] = value;
            }
        }
        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["CurrPK"]);
            }
            set
            {
                this.ViewState["CurrPK"] = value;
            }
        }
        /// <summary>
        /// CostCenter PK
        /// </summary>
        private int ParameterPK1
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ParameterPK1"]);
            }
            set
            {
                this.ViewState["ParameterPK1"] = value;
            }
        }
        #endregion

        User currentUser;

        //Data table for binding the  Grid

        private DataTable dtMailDetails;


        // Indicates the state as well as action
        private  ActionsEnum commonActions;
        public int ModuleId
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ModuleId"]);
            }
            set
            {
                this.ViewState["ModuleId"] = value;
            }
        }
        public int ApplicationId
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ApplicationId"]);
            }
            set
            {
                this.ViewState["ApplicationId"] = value;
            }
        }
        public int SubAppId
        {
            get
            {
                return Convert.ToInt32(this.ViewState["SubAppId"]);
            }
            set
            {
                this.ViewState["SubAppId"] = value;
            }
        }
        public int ActionId
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ActionId"]);
            }
            set
            {
                this.ViewState["ActionId"] = value;
            }
        }
        public int TemplateType
        {
            get
            {
                return Convert.ToInt32(this.ViewState["TemplateType"]);
            }
            set
            {
                this.ViewState["TemplateType"] = value;
            }
        }
        private int mailStatus;
        private string createdMail;
        private DataTable dtAppType;
        private DataTable dtSubApp;
        private DataTable dtActionType;
        private DataTable dtProcess;
        #endregion

        #region Set Page Variables
        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {

            string[] dataKeyArray;
            dataKeyArray = new string[1];
            dataKeyArray[0] = ViewMail.F_PK;
            grdPage.DataKeyNames = dataKeyArray;
        }
        #endregion

        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.ACTIONTYPE:
                    dtActionType = MailSendManager.GetActionType();
                    break;
                case ControlsEnum.GRID:
                    dtMailDetails = MailSendBL.GetMailDetails(CurrPK
                        , CurrPK > 0 ? 0 : (Convert.ToInt32(ddlProcess.SelectedValue) > 0 ? Convert.ToInt32(ddlProcess.SelectedValue) : 0)
                        , CurrPK > 0 ? string.Empty : txtFromDate.Text, CurrPK > 0 ? string.Empty : txtToDate.Text, currentUser.SBUID);
                    break;
                case ControlsEnum.SUBAPP:
                    dtSubApp = MailSendManager.GetSubApp(Convert.ToInt32(ddlApp.SelectedValue));
                    break; 
                case ControlsEnum.PROCESS:
                    dtProcess = MailSendBL.GetMailFilterFieldAuto(ERP.Utilities.Constants.DA.Administration.MailSend.F_WKF_PROCESS, "%", currentUser.SBUID);
                    break;
                default: // 
                    dtAppType = MailSendManager.GetApplicationType(Convert.ToInt32(ddlmod.SelectedValue));
                    break;

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
            switch (controlType)
            {
                case ControlsEnum.GRID:
                    if (commonActions == ActionsEnum.EDIT_ACTION) // Edit mode. Fill the details of the edit record to the corresponding fields
                    {
                        if (dtMailDetails != null)
                        {
                            if (dtMailDetails.Rows.Count > 0)
                            {
                                GetUIValuesFromObject();
                            }
                        }
                    }
                    else
                    {
                        BindGrid();
                    }
                    break;

                case ControlsEnum.APPTYPE:
                    BindDropDown(ControlsEnum.APPTYPE);
                    break;

                case ControlsEnum.ACTIONTYPE:
                    BindDropDown(ControlsEnum.ACTIONTYPE);
                    break;
                case ControlsEnum.SUBAPP:
                    BindDropDown(ControlsEnum.SUBAPP);
                    break;
                case ControlsEnum.PROCESS:
                    BindDropDown(controlType);
                    break;
                default: // if passed nothing or string.empty(), then Bind for Initail

                    break;

            }
        }

        #endregion

        #region Action Handlers

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click (Save/Cancel)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {

            if (sender.GetType().IsEquivalentTo(typeof(Button)))//cheking gettype is button
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))//cheking gettype is DropDownList
            {
                commonActions = ActionsEnum.CHANGE;
            }

           // pnlSave.Visible = true;
            switch (commonActions)
            {
                case ActionsEnum.SAVE:// SAVE .
                    //if (!IsValid)//if page is not valid
                    //{

                    //    //litMailErrorMsg.Text = Resources.MailRes.Msg_Save_Error;
                    //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowMailError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litMailErrorMsg.Text) + Resources.MailRes.Title_Information + "');", true);
                    //}
                    //else//if page is valid.
                    //{
                        SaveMailDetails();
                    //}
                    break;
                case ActionsEnum.CANCEL:// SAVE .
                    Response.Redirect(CommonConstants.DEFAULTPAGE);
                    break;
                case ActionsEnum.VIEW:// SAVE .
                    ViewMailDetails();
                    break;
                case ActionsEnum.RESEND:// SAVE .
                    SendMailandSaveDetails();
                    break;
                case ActionsEnum.SHOW:
                    ///To filter the exchange list based on the values in the data entry form
                    CurrPK = 0;
                    ModuleId = Convert.ToInt32(ddlmod.SelectedValue);
                    ApplicationId = Convert.ToInt32(ddlApp.SelectedValue);
                    ActionId = Convert.ToInt32(ddlAction.SelectedValue);
                    SubAppId = Convert.ToInt32(ddlsubApp.SelectedValue);
                    GetFieldValues(ControlsEnum.GRID);
                    SetFieldValues(ControlsEnum.GRID);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide('ADD_DETAILS');", true);
                    break;
                case ActionsEnum.CHANGE://DROPDOWN Change mode.Filling cuurency wrp to location pk
                    //if (((DropDownList)sender).ID.ToLower() == ViewMail.DROPDOWNLOCATION.ToLower())
                    //{
                    //    if (ddlLocation.SelectedValue == CommonConstants.SELECTVAL)
                    //    {
                    //        ddlCostCenter.Items.Clear();
                    //        ddlCostCenter.Items.Insert(0, new ListItem(Resources.MailRes.Select, CommonConstants.SELECTVAL));
                    //    }
                    //    else
                    //    {
                    //        locationPK = Convert.ToInt32(ddlLocation.SelectedValue);
                    //        GetFieldValues(ControlsEnum.COSTCENTER);
                    //        SetFieldValues(ControlsEnum.COSTCENTER);

                    //    }

                    //}
                    break;
            }
        }
        #endregion
        #region --- For Grid Actions----
        /// <summary>
        /// Page Index Handler for grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            CurrPK = 0;
            GetFieldValues(ControlsEnum.GRID);
            PageIndex = e.NewPageIndex.ToString();
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
            BindGrid();
        }
        /// <summary>
        /// Sorting Event Handler for grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {

            /// getting field values for Sorting Event Handler.
            CurrPK = 0;
            Type type = typeof(ERP.Utilities.Constants.DA.Administration.MailSend);
            FieldInfo fieldInfo = type.GetField(e.SortExpression);
            SortExpression = Convert.ToString(fieldInfo.GetValue(0));
            if (SortOrder == "asc")
                SortOrder = "desc";
            else
                SortOrder = "asc";
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
            GetFieldValues(ControlsEnum.GRID);
            BindGrid();

        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    string subjectResourceFormat = "";
            //    string formatedSubject = "";
            //    Label lblSubject = (Label)e.Row.FindControl("lblSubject");
            //    Label lblContent = (Label)e.Row.FindControl("lblContent");
            //    string[] mailDetailsRow = new string[10];
            //    mailDetailsRow = lblContent.Text.Split(',');
            //    if (lblSubject.Text == CommonConstants.BUDGETRANDASUBJECT)
            //    {
            //        subjectResourceFormat = this.GetGlobalResourceObject("EmailTemplates", CommonConstants.BUDGETRANDASUBJECT).ToString();//getting template using template name.
            //        formatedSubject = string.Format(subjectResourceFormat, mailDetailsRow);
            //        lblSubject.Text = HttpUtility.HtmlDecode(formatedSubject);
            //    }
            //    else if (lblSubject.Text == CommonConstants.SENDBACKSUBJECT)
            //    {
            //        subjectResourceFormat = this.GetGlobalResourceObject("EmailTemplates", CommonConstants.SENDBACKSUBJECT).ToString();//getting template using template name.
            //        formatedSubject = string.Format(subjectResourceFormat, mailDetailsRow);
            //        lblSubject.Text = HttpUtility.HtmlDecode(formatedSubject);
            //    }

            //}
        }

        #endregion
        #endregion

        #region Page Level Events
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }

        /// <summary>
        /// To handle pageload event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageVariables();
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                hdfToDate.Value = DateTime.Now.ToString();

                GetFieldValues(ControlsEnum.APPTYPE);
                SetFieldValues(ControlsEnum.APPTYPE);
                GetFieldValues(ControlsEnum.SUBAPP);
                SetFieldValues(ControlsEnum.SUBAPP);
                GetFieldValues(ControlsEnum.ACTIONTYPE);
                SetFieldValues(ControlsEnum.ACTIONTYPE);
                GetFieldValues(ControlsEnum.PROCESS);
                SetFieldValues(ControlsEnum.PROCESS);
                GetFieldValues(ControlsEnum.GRID);
                SetFieldValues(ControlsEnum.GRID);
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitControlComponents", "$(document).ready(function(){InitControlComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + ERP.Utilities.CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion

        #region Helper Methods


        /// <summary>
        /// Method used for saving action details.
        /// </summary>
        private void SaveMailDetails()
        {
            int result;
            DbSaveStatus saveStatus;

            result = MailSendManager.SaveMailDetails(mailStatus, CurrPK);

            if (result > 0)// Success ! re-initialize the page
            {

                ResetForm();
                litMailErrorMsg.Text = this.GetLocalResourceObject("Msg_Success").ToString();
                litMailErrorMsg.Text = string.Format(litMailErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Title").ToString());
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopUp", "ClosePopUp();", true);
                //register script to Show the corresponding error message//
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowMailError", "ShowErrorMessage1('" + CommonFunctions.FormatErrorMessage(litMailErrorMsg.Text) + "','" + Resources.MailRes.Title_Information + "');", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowMailError", "ShowMailError('" + litMailErrorMsg.Text + "','" + Resources.MailRes.Title_Information + "');", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "MailDetails", "ShowMailError();", true);



            }
            else
            {
                saveStatus = (DbSaveStatus)result;
                switch (saveStatus)
                {
                    case DbSaveStatus.SQLERROR://SQl Error
                        litMailErrorMsg.Text = Resources.MailRes.Msg_Sql_Error;
                        litMailErrorMsg.Text = string.Format(litMailErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Title").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowMailError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litMailErrorMsg.Text) + "','" + Resources.MailRes.Title_Information + "');", true);
                        break;
                    case DbSaveStatus.CODEEXIST://CODEEXIST
                        litMailErrorMsg.Text = this.GetLocalResourceObject("Err_Code_Dupl").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowMailError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litMailErrorMsg.Text) + "','" + Resources.MailRes.Title_Information + "');", true);
                        break;
                    case DbSaveStatus.CONCURRENCY://Cuncurrency Check
                        litMailErrorMsg.Text = Resources.MailRes.Msg_Delete_Error_Concurrent;
                        litMailErrorMsg.Text = string.Format(litMailErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Title").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowMailError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litMailErrorMsg.Text) + "','" + Resources.MailRes.Title_Information + "');", true);
                        break;
                    default:
                        litMailErrorMsg.Text = Resources.MailRes.Msg_Save_Error;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowMailError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litMailErrorMsg.Text) + "','" + Resources.MailRes.Title_Information + "');", true);
                        break;
                }
            }


        }
        /// <summary>
        /// Method to View Mail Details
        /// </summary>
        private void ViewMailDetails()
        {
            foreach (GridViewRow grdrow in grdPage.Rows)//loop through grid to find the selected Record for Viewing
            {
                RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");//if radio button is checked get the Pk of corresponding record
                if (rbtn.Checked)
                {
                    commonActions = ActionsEnum.VIEW;
                    commonActions = ActionsEnum.EDIT_ACTION;
                    CurrPK = Convert.ToInt32(grdPage.DataKeys[grdrow.RowIndex].Values[0]);
                    GetFieldValues(ControlsEnum.GRID);
                    SetFieldValues(ControlsEnum.GRID);
                    //script register to show the error details
                    if (dtMailDetails != null && dtMailDetails.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "MailDetails", "ShowMailPopUp('#divPopUp');", true);
                    }
                    else
                    {
                        litMailErrorMsg.Text = Resources.MailRes.Msg_Delete_Error_Concurrent;
                        litMailErrorMsg.Text = string.Format(litMailErrorMsg.Text.ToLower(), this.GetLocalResourceObject("MailQueItem").ToString());
                        GetFieldValues(ControlsEnum.GRID);
                        SetFieldValues(ControlsEnum.GRID);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowMailError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litMailErrorMsg.Text) + "','" + Resources.MailRes.Title_Information + "');", true);
                    }
                    return;
                }
            }
            litMailErrorMsg.Text = Resources.MailRes.Msg_Not_Selected;
            //script register to show the error details
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowMailError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litMailErrorMsg.Text) + "','" + Resources.MailRes.Title_Information + "');", true);
        }
        /// <summary>
        /// method for sending mail and save mail status.
        /// </summary>
        public void SendMailandSaveDetails()
        {
            bool status = false;// CommonFunctions.SendMail(lblSubjectText.Text, ltContent.Text, lblToText.Text);//calling common function for sending mails.
            if (status)//if status =true.
            {
                mailStatus = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);//setting mail status to one

            }
            else// status =false.
            {
                mailStatus = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);//setting mail status to zero.
            }
            SaveMailDetails();//calling function for saving mail details.

        }
        /// <summary>
        /// Binds the  grid with data
        /// </summary>
        /// 
        private void BindGrid()
        {

            if ((dtMailDetails != null) && (dtMailDetails.Rows.Count > 0))
            {
                pnlView.Visible = true;
            }
            else
            {
                pnlView.Visible = false;
            }
            SortExpression = SortExpression == null ? ViewMail.F_PK : SortExpression;//TODO: get the value from constants
            SortOrder = SortOrder == null ? "asc" : SortOrder;
            PageIndex = PageIndex == null ? "0" : PageIndex;
            SortFilter = SortExpression + " " + SortOrder;
            dtMailDetails.DefaultView.Sort = SortFilter;
            grdPage.PageIndex = Convert.ToInt32(PageIndex);
            grdPage.DataSource = dtMailDetails.DefaultView;
            grdPage.DataBind();

        }

        /// <summary>
        /// Resets the form for a fresh entry
        /// </summary>
        private void ResetForm()
        {

            dtMailDetails = null;
            grdPage.DataSource = dtMailDetails;
            grdPage.DataBind();
        }



        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            CurrPK = Convert.ToInt32(dtMailDetails.Rows[0][ViewMail.F_PK]);
            lblToText.Text = HttpUtility.HtmlDecode(dtMailDetails.Rows[0][ViewMail.F_TO].ToString());

            string[] mailDetails = new string[11];
            lblSubjectText.Text = HttpUtility.HtmlDecode(dtMailDetails.Rows[0][ViewMail.F_SUBJECT].ToString());
            ltContent.Text = dtMailDetails.Rows[0][ViewMail.F_CONTENT].ToString();

        }
        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum drpName)
        {
            switch (drpName)
            {
                case ControlsEnum.ACTIONTYPE: //if CURRENCY entering to this case
                    ddlAction.Items.Clear();
                    if ((dtActionType != null) && (dtActionType.Rows.Count > 0))//cheking datatable has count or not if has binding dropdown
                    {
                        ddlAction.DataSource = CommonFunctions.HtmlDecodeDataTable(dtActionType, "ACT_NAME");
                        ddlAction.DataTextField = "ACT_NAME";
                        ddlAction.DataValueField = "ACT_PK";
                        ddlAction.DataBind();
                        ddlAction.Items.Insert(0, new ListItem(Resources.MailRes.All, CommonConstants.SELECTVAL));
                        // ddlAction.SelectedIndex = ddlAction.Items.IndexOf(ddlAction.Items.FindByValue(CostCenterPK.ToString()));
                    }
                    else
                    {
                        ddlAction.Items.Clear();
                        ddlAction.Items.Insert(0, new ListItem(Resources.MailRes.All, CommonConstants.SELECTVAL));
                    }
                    break;
                case ControlsEnum.APPTYPE://if LOCATION entering to this case
                    if ((dtAppType != null) && (dtAppType.Rows.Count > 0))//cheking datatable has count or not if has binding dropdown
                    {
                        ddlApp.DataSource = CommonFunctions.HtmlDecodeDataTable(dtAppType, "APT_NAME");
                        ddlApp.DataTextField = "APT_NAME";
                        ddlApp.DataValueField = "APT_PK";
                        ddlApp.DataBind();
                        ddlApp.Items.Insert(0, new ListItem(Resources.MailRes.All, CommonConstants.SELECTVAL));
                        //ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(locationPK.ToString()));
                    }
                    else
                    {
                        ddlApp.Items.Clear();
                        ddlApp.Items.Insert(0, new ListItem(Resources.MailRes.All, CommonConstants.SELECTVAL));
                    }
                    break;
                case ControlsEnum.SUBAPP:
                    if ((dtSubApp != null) && (dtSubApp.Rows.Count > 0))
                    {
                        ddlsubApp.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSubApp, "AST_NAME");
                        ddlsubApp.DataTextField = "AST_NAME";
                        ddlsubApp.DataValueField = "AST_PK";
                        ddlsubApp.DataBind();
                        ddlsubApp.Items.Insert(0, new ListItem(Resources.MailRes.All, CommonConstants.SELECTVAL));
                        //ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(locationPK.ToString()));
                    }
                    else
                    {
                        ddlsubApp.Items.Clear();
                        ddlsubApp.Items.Insert(0, new ListItem(Resources.MailRes.All, CommonConstants.SELECTVAL));
                    }
                    break;
                case ControlsEnum.PROCESS:
                    ddlProcess.Items.Clear();
                    ddlProcess.DataSource = dtProcess;
                    ddlProcess.DataTextField = GTIService.Constants.Common.Common.F_VALUE;
                    ddlProcess.DataValueField = GTIService.Constants.Common.Common.F_PK;
                    ddlProcess.DataBind();
                    ddlProcess.Items.Insert(0, new ListItem(Resources.MailRes.All, CommonConstants.SELECTVAL));
                    break;
                default:
                    break;
            }

        }

        #endregion
        #region ControlsEnum
        public enum ControlsEnum
        {
            GRID,
            APPTYPE,
            SUBAPP,
            ACTIONTYPE,
            TEMPLATETYPE,
            GRIDTAG,
            DEFAULT,
            PROCESS
        }
        #endregion
    }
}