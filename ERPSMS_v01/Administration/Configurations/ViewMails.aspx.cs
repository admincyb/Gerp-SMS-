using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.AccountManagement;
using MailCore.Utility;
using MailCore.BO;
using System.Configuration;
using ERP.Utilities.Constants.DA.Administration;
using BusinessLogic.Administration.Configurations;
using System.Reflection;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class ViewMails : ERP.Store.UI.MyBasePage
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
        private int CostCenterPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["CostCenterPK"]);
            }
            set
            {
                this.ViewState["CostCenterPK"] = value;
            }
        }
        #endregion
        // Holds the current logged in user
        private BusinessObject.User currentUser;


        //Data table for binding the  Grid
        private DataTable dtLocation;
        private DataTable dtCostCenter;
        private DataTable dtMailDetails;


        // Indicates the state as well as action
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        private int locationPK;
        private int mailStatus;
        private string createdMail;

        #endregion

        #region Set Page Variables
        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
            //Initialze the current logged in user to the currentUser variable
            currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;
            string[] dataKeyArray;
            dataKeyArray = new string[1];
            dataKeyArray[0] = ERP.Utilities.Constants.DA.Administration.ViewMail.F_PK;
            grdPage.DataKeyNames = dataKeyArray;
        }
        #endregion

        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ViewMailsBO.ControlsEnum controlType)
        {

            switch (controlType)
            {

                case ViewMailsBO.ControlsEnum.SUBAPP:
                    dtCostCenter = dtCostCenter = CostCenterBL.GetCostCenter(0, BusinessObject.CommonManagement.DbActiveStatus.ALL, currentUser.SBUID, locationPK);
                    break;
                case ViewMailsBO.ControlsEnum.GRID:
                    dtMailDetails = ViewMailsBL.GetMailDetails(CostCenterPK, CurrPK);
                    break;
                default: // if passed nothing or string.empty(), then Meand Default Bing Will Bind all the Data in initial stage
                    dtLocation = LocationBL.GetLocation(0, BusinessObject.CommonManagement.DbActiveStatus.ACTIVE, currentUser.SBUID);
                    break;
            }
        }

        #endregion

        #region Set Field Values

        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ViewMailsBO.ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ViewMailsBO.ControlsEnum.GRID:
                    if (commonActions ==BusinessObject.AccountManagement.ActionsEnum.EDIT_ACTION) // Edit mode. Fill the details of the edit record to the corresponding fields
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
                case ViewMailsBO.ControlsEnum.SUBAPP:
                    BindDropDown(ViewMailsBO.ControlsEnum.SUBAPP);
                    break;
                case ViewMailsBO.ControlsEnum.APPTYPE:
                    BindDropDown(ViewMailsBO.ControlsEnum.APPTYPE);
                    BindDropDown(ViewMailsBO.ControlsEnum.SUBAPP);
                    break;
                default: // if passed nothing or string.empty(), then Bind for Initail
                    BindDropDown(ViewMailsBO.ControlsEnum.APPTYPE);

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
                commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))//cheking gettype is DropDownList
            {
                commonActions = BusinessObject.AccountManagement.ActionsEnum.CHANGE;
            }

            pnlSave.Visible = true;
            switch (commonActions)
            {
                case BusinessObject.AccountManagement.ActionsEnum.SAVE:// SAVE .
                    if (!IsValid)//if page is not valid
                    {

                        litErrorMsg.Text = Resources.ErpRes.Msg_Save_Error;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + Resources.ErpRes.Title_Information + "');", true);
                    }
                    else//if page is valid.
                    {
                        SaveMailDetails();
                    }
                    break;
                case BusinessObject.AccountManagement.ActionsEnum.CANCEL:// SAVE .
                    Response.Redirect(CommonConstants.DEFAULTPAGE);
                    break;
                case BusinessObject.AccountManagement.ActionsEnum.VIEW:// SAVE .
                    ViewMailDetails();
                    break;
                case BusinessObject.AccountManagement.ActionsEnum.RESEND:// SAVE .
                    SendMailandSaveDetails();
                    break;
                case BusinessObject.AccountManagement.ActionsEnum.SHOW:
                    //To filter the exchange list based on the values in the data entry form
                    CurrPK = 0;
                    CostCenterPK = 0;
                    CostCenterPK = Convert.ToInt32(ddlCostCenter.SelectedValue);
                    GetFieldValues(ViewMailsBO.ControlsEnum.GRID);
                    SetFieldValues(ViewMailsBO.ControlsEnum.GRID);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide('ADD_DETAILS');", true);
                    break;
                case BusinessObject.AccountManagement.ActionsEnum.CHANGE://DROPDOWN Change mode.Filling cuurency wrp to location pk
                    if (((DropDownList)sender).ID.ToLower() == ERP.Utilities.Constants.DA.Administration.ViewMail.DROPDOWNLOCATION.ToLower())
                    {
                        if (ddlLocation.SelectedValue == CommonConstants.SELECTVAL)
                        {
                            ddlCostCenter.Items.Clear();
                            ddlCostCenter.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        }
                        else
                        {
                            locationPK = Convert.ToInt32(ddlLocation.SelectedValue);
                            GetFieldValues(ViewMailsBO.ControlsEnum.SUBAPP);
                            SetFieldValues(ViewMailsBO.ControlsEnum.SUBAPP);

                        }

                    }
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
            GetFieldValues(ViewMailsBO.ControlsEnum.GRID);
            PageIndex = e.NewPageIndex.ToString();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
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
            Type type = typeof(ERP.Utilities.Constants.DA.Administration.ViewMail);
            FieldInfo fieldInfo = type.GetField(e.SortExpression);
            SortExpression = Convert.ToString(fieldInfo.GetValue(0));
            if (SortOrder == "asc")
                SortOrder = "desc";
            else
                SortOrder = "asc";
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
            GetFieldValues(ViewMailsBO.ControlsEnum.GRID);
            BindGrid();

        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string subjectResourceFormat = "";
                string formatedSubject = "";
                Label lblSubject = (Label)e.Row.FindControl("lblSubject");
                Label lblContent = (Label)e.Row.FindControl("lblContent");
                string[] mailDetailsRow = new string[10];
                mailDetailsRow = lblContent.Text.Split(',');
                if (lblSubject.Text == CommonConstants.BUDGETRANDASUBJECT)
                {
                    subjectResourceFormat = this.GetGlobalResourceObject("EmailTemplates", CommonConstants.BUDGETRANDASUBJECT).ToString();//getting template using template name.
                    formatedSubject = string.Format(subjectResourceFormat, mailDetailsRow);
                    lblSubject.Text = HttpUtility.HtmlDecode(formatedSubject);
                }
                else if (lblSubject.Text == CommonConstants.SENDBACKSUBJECT)
                {
                    subjectResourceFormat = this.GetGlobalResourceObject("EmailTemplates", CommonConstants.SENDBACKSUBJECT).ToString();//getting template using template name.
                    formatedSubject = string.Format(subjectResourceFormat, mailDetailsRow);
                    lblSubject.Text = HttpUtility.HtmlDecode(formatedSubject);
                }

            }
        }

        #endregion
        #endregion

        #region Page Level Events
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
                GetFieldValues(ViewMailsBO.ControlsEnum.APPTYPE);
                SetFieldValues(ViewMailsBO.ControlsEnum.APPTYPE);
            }
        }

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.btnResend.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSendCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnShow.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //base.CheckBtnVisibility(sender);
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

            result = ViewMailsBL.SaveMailDetails(mailStatus, CurrPK);

            if (result > 0)// Success ! re-initialize the page
            {

                ResetForm();
                litErrorMsg.Text = this.GetLocalResourceObject("Msg_Success").ToString();
                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Title").ToString());
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide();", true);
                //register script to Show the corresponding error message
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);


            }
            else
            {
                saveStatus = (DbSaveStatus)result;
                switch (saveStatus)
                {
                    case DbSaveStatus.SQLERROR://SQl Error
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Title").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                        break;
                    case DbSaveStatus.CODEEXIST://CODEEXIST
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_Code_Dupl").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                        break;
                    case DbSaveStatus.CONCURRENCY://Cuncurrency Check
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Title").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                        break;
                    default:
                        litErrorMsg.Text = Resources.ErpRes.Msg_Save_Error;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
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
                    commonActions = BusinessObject.AccountManagement.ActionsEnum.VIEW;
                    commonActions = BusinessObject.AccountManagement.ActionsEnum.EDIT_ACTION;
                    CurrPK = Convert.ToInt32(grdPage.DataKeys[grdrow.RowIndex].Values[0]);
                    GetFieldValues(ViewMailsBO.ControlsEnum.GRID);
                    SetFieldValues(ViewMailsBO.ControlsEnum.GRID);
                    //script register to show the error details
                    if (dtMailDetails != null && dtMailDetails.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "MailDetails", "ShowPopUp();", true);
                    }
                    else
                    {
                        ResetForm();
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("CostCenter").ToString());
                        GetFieldValues(ViewMailsBO.ControlsEnum.DEFAULT);
                        SetFieldValues(ViewMailsBO.ControlsEnum.DEFAULT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                    }
                    return;
                }
            }
            litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
            //script register to show the error details
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
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
            SortExpression = SortExpression == null ? ERP.Utilities.Constants.DA.Administration.ViewMail.F_NAME : SortExpression;//TODO: get the value from constants
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
            //Codes for Clearing the controls in the page
            CurrPK = 0;
            CostCenterPK = 0;
            ddlCostCenter.Items.Clear();
            pnlView.Visible = false;
            ddlLocation.ClearSelection();
            dtMailDetails = null;
            grdPage.DataSource = dtMailDetails;
            grdPage.DataBind();
        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ViewMailsBO.ControlsEnum drpName)
        {
            switch (drpName)
            {
                case ViewMailsBO.ControlsEnum.SUBAPP: //if CURRENCY entering to this case
                    if ((dtCostCenter != null) && (dtCostCenter.Rows.Count > 0))//cheking datatable has count or not if has binding dropdown
                    {
                        ddlCostCenter.DataSource = CommonFunctions.HtmlDecodeDataTable(dtCostCenter, CostCenters.F_CODE_NAME);
                        ddlCostCenter.DataTextField = CostCenters.F_CODE_NAME;
                        ddlCostCenter.DataValueField = CostCenters.F_PK;
                        ddlCostCenter.DataBind();
                        ddlCostCenter.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        ddlCostCenter.SelectedIndex = ddlCostCenter.Items.IndexOf(ddlCostCenter.Items.FindByValue(CostCenterPK.ToString()));

                    }
                    else
                    {
                        ddlCostCenter.Items.Clear();
                        ddlCostCenter.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    }
                    break;
                case ViewMailsBO.ControlsEnum.APPTYPE://if APPTYPE entering to this case
                    if ((dtLocation != null) && (dtLocation.Rows.Count > 0))//cheking datatable has count or not if has binding dropdown
                    {
                        ddlLocation.DataSource = CommonFunctions.HtmlDecodeDataTable(dtLocation, Locations.F_CODE_NAME);
                        ddlLocation.DataTextField = Locations.F_CODE_NAME;
                        ddlLocation.DataValueField = Locations.F_PK;
                        ddlLocation.DataBind();
                        ddlLocation.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(locationPK.ToString()));

                    }
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            CryptoServices objCryptoServices;
            string subjectResourceFormat, formatedSubject;
            objCryptoServices = new CryptoServices();//creating object for password decription.
            string pwd;
            CurrPK = Convert.ToInt32(dtMailDetails.Rows[0][ERP.Utilities.Constants.DA.Administration.ViewMail.F_PK]);
            lblToText.Text = HttpUtility.HtmlDecode(dtMailDetails.Rows[0][ERP.Utilities.Constants.DA.Administration.ViewMail.F_TO].ToString());

            string[] mailDetails = new string[11];
            mailDetails = dtMailDetails.Rows[0][ERP.Utilities.Constants.DA.Administration.ViewMail.F_CONTENT].ToString().Split(',');//spliting content details with ,
            string emailTemplate = this.GetGlobalResourceObject("EmailTemplates", dtMailDetails.Rows[0][ERP.Utilities.Constants.DA.Administration.ViewMail.F_TEMPLATE].ToString()).ToString();//getting template using template name.
            if (dtMailDetails.Rows[0][ERP.Utilities.Constants.DA.Administration.ViewMail.F_TEMPLATE].ToString() == CommonConstants.BUDGETMASTER)//comparing template name is budget master
            {
                pwd = mailDetails[CommonConstants.BUDGETMASTERPWDPOS];//taking position of password
                mailDetails[CommonConstants.BUDGETMASTERPWDPOS] = Convert.ToString(CryptoServices.DecryptString(pwd, ConfigurationManager.AppSettings["salt"]));//decryppting password by using positon.
            }
            if (dtMailDetails.Rows[0][ERP.Utilities.Constants.DA.Administration.ViewMail.F_TEMPLATE].ToString() == CommonConstants.REVIEWANDAPPROOVAL || dtMailDetails.Rows[0][ERP.Utilities.Constants.DA.Administration.ViewMail.F_TEMPLATE].ToString() == CommonConstants.REVIEWANDAPPROOVALNOPWD)//comparing template name is review and approval.
            {

                subjectResourceFormat = this.GetGlobalResourceObject("EmailTemplates", CommonConstants.BUDGETRANDASUBJECT).ToString();//getting template using template name.
                formatedSubject = string.Format(subjectResourceFormat, mailDetails);
                lblSubjectText.Text = HttpUtility.HtmlDecode(formatedSubject);
                pwd = mailDetails[CommonConstants.REVIEWANDAPPROOVALPWDPOS];//taking position of password
                mailDetails[CommonConstants.REVIEWANDAPPROOVALPWDPOS] = Convert.ToString(CryptoServices.DecryptString(pwd, ConfigurationManager.AppSettings["salt"]));//decryppting password by using positon.
                mailDetails[5] = ConfigurationManager.AppSettings["AccessLink"];//taking access link from config file.
            }
            else if (dtMailDetails.Rows[0][ERP.Utilities.Constants.DA.Administration.ViewMail.F_TEMPLATE].ToString() == CommonConstants.SENDBACK)//comparing template name is review and approval.
            {
                subjectResourceFormat = this.GetGlobalResourceObject("EmailTemplates", CommonConstants.SENDBACKSUBJECT).ToString();//getting template using template name.
                formatedSubject = string.Format(subjectResourceFormat, mailDetails);
                lblSubjectText.Text = HttpUtility.HtmlDecode(formatedSubject);
            }
            else
            {
                lblSubjectText.Text = HttpUtility.HtmlDecode(dtMailDetails.Rows[0][ERP.Utilities.Constants.DA.Administration.ViewMail.F_SUBJECT].ToString());


            }
            //for html decoding
            for (int j = 0; j < mailDetails.Length; j++)
            {
                mailDetails[j] = HttpUtility.HtmlDecode(mailDetails[j].ToString());

            }
            createdMail = string.Format(emailTemplate, mailDetails);//formating template with string array.
            ltContent.Text = createdMail;

        }

    }
}
        #endregion