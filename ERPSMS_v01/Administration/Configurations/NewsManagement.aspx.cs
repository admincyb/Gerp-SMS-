using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.AccountManagement;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using BusinessLogic.Administration.Configurations;
using ERP.Utilities;
using GTIService.Constants.Administration.Configurations;
using System.Reflection;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class NewsManagement : ERP.Store.UI.MyBasePage
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
        private int currPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["currPK"]);
            }
            set
            {
                this.ViewState["currPK"] = value;
            }
        }

        #endregion
        // Holds the current logged in user
        private BusinessObject.User currentUser;
            

        //Data table for binding the Template Grid
        private DataTable dtNews;

        // Indicates the state as well as action
        private ActionsEnum commonActions;

        #endregion

        #region Set Page Variables
        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
            //Initialze the current logged in user to the currentUser variable
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack || (base.GetPostBackControl() != null && base.GetPostBackControl().GetType().IsEquivalentTo(typeof(Button))))
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails1", "ShowHide('ADD_DETAILS');", true);
            }
            else if (!IsPostBack || (base.GetPostBackControl() != null && base.GetPostBackControl().GetType().IsEquivalentTo(typeof(GridView))))
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails1", "ShowHide('ADD_DETAILS');", true);
            }
         
            // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AddDate", "$(document).ready(function () {AddDatePicker();});", true);   
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitDate1", "$(document).ready(function () {InitDate();});", true);

        }
        #endregion

        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(NewsManagementBO.ControlsEnum type)
        {

            DbActiveStatus opMode; //Indicates which mode of operation need to be performed in DB
            switch (type)
            {
                case NewsManagementBO.ControlsEnum.GRID:
                    opMode = commonActions == ActionsEnum.EDIT_ACTION ? DbActiveStatus.HASPK : DbActiveStatus.ALL; //2 - Edit mode ; 0 - Normal Lists all
                    dtNews = NewsManagementBL.GetNews(currPK, opMode);
                    break;
                default: // if passed nothing or string.empty(), then Meand Default Bing Will Bind all the Data in initial stage
                    dtNews = NewsManagementBL.GetNews(currPK, DbActiveStatus.ALL);
                    break;
            }
        }

        #endregion

        #region Set Field Values

        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(NewsManagementBO.ControlsEnum type)
        {
            switch (type)
            {
                case NewsManagementBO.ControlsEnum.GRID:
                    if (commonActions == ActionsEnum.EDIT_ACTION) // Edit mode. Fill the details of the edit record to the corresponding fields
                    {
                        if (dtNews.Rows.Count > 0)
                        {
                            GetUIValuesFromObject();
                        }
                    }
                    else
                    {
                        BindGrid();
                    }
                    break;
                default: // if passed nothing or string.empty(), then Bind for Initail
                    BindGrid();
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
            int result;
            if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
            }
            NewsManagementBO newsManagementBO;
            pnlEdit.Visible = false;
            if (base.GetPostBackControl() == null)
            {
                pnlSave.Visible = false;
            }

            switch (commonActions)
            {
                case ActionsEnum.SAVE:
                    if (!IsValid)
                    {
                        litErrorMsg.Text = Resources.ErpRes.Msg_Save_Error.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Title_Information + "');", true);
                    }
                    else
                    {
                        newsManagementBO = SetUIValuesToObject();
                        result = NewsManagementBL.SaveNews(newsManagementBO, currentUser.PKUser, currentUser.SBUID);

                        if (result > 0) // Success ! re-initialize the page
                        {
                            ResetForm();
                            lnkList.CssClass = "tab-active";
                            lnkDetail.CssClass = "tab-inactive";
                            spnList.Attributes.Remove("class");
                            spnDetail.Attributes.Remove("class");
                            spnList.Attributes.Add("class", "tab-active");
                            spnDetail.Attributes.Add("class", "tab-inactive");
                            GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                            SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                            litErrorMsg.Text = Resources.Messages.Msg_Save_Success.ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                            //register script for showing the message
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Title_Information.ToString() + "');", true);
                        }
                        else
                            switch (result)
                            {
                                case (int)DbSaveStatus.SQLERROR: //sql error
                                    pnlSave.Visible = true;
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error.ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide();", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information.ToString() + "');", true);
                                    break;

                                case (int)DbSaveStatus.CODEEXIST: //Check for the existence of code to avoid duplication
                                    litErrorMsg.Text = this.GetLocalResourceObject("Err_Code_Dupl").ToString();
                                    pnlSave.Visible = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide();", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information.ToString() + "');", true);
                                    break;
                                case (int)DbSaveStatus.CONCURRENCY: //Check for concurrent save
                                    ResetForm();
                                    GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                    SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide('ADD_DETAILS');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                    break;
                                case (int)DbSaveStatus.ALREADYDELETED: //Check whether the code is already deleted
                                    ResetForm();
                                    GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                    SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide('ADD_DETAILS');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErpRes.Msg_Save_Error;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                    }
                    break;
                case ActionsEnum.CANCEL:
                    ResetForm();
                    GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                    SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                    lnkList.CssClass = "tab-active";
                    lnkDetail.CssClass = "tab-inactive";
                    spnList.Attributes.Remove("class");
                    spnDetail.Attributes.Remove("class");
                    spnList.Attributes.Add("class","tab-active");
                    spnDetail.Attributes.Add("class", "tab-inactive");
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide('ADD_DETAILS');", true);
                    break;
                case ActionsEnum.LIST:
                    ResetForm();
                    GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                    SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);                 
                    lnkList.CssClass = "tab-active";
                    lnkDetail.CssClass = "tab-inactive";
                    spnList.Attributes.Remove("class");
                    spnDetail.Attributes.Remove("class");
                    spnList.Attributes.Add("class","tab-active");
                    spnDetail.Attributes.Add("class", "tab-inactive");
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide('ADD_DETAILS');", true);
                    break;
                case ActionsEnum.EDIT:
                    pnlEdit.Visible = true;
                    pnlSave.Visible = true;
                    foreach (GridViewRow grdrow in grdPage.Rows)
                    {
                        if (grdrow.FindControl("rbtSelect") != null)
                            if (((RadioButton)grdrow.FindControl("rbtSelect")).Checked) //Check for a selected row from the list
                            {
                                commonActions = ActionsEnum.EDIT_ACTION;
                                currPK = Convert.ToInt32(grdPage.DataKeys[grdrow.RowIndex].Values[0]);
                                GetFieldValues(NewsManagementBO.ControlsEnum.GRID);
                                txtTitle.Focus();
                                if (dtNews != null && dtNews.Rows.Count > 0)
                                {
                                    lnkList.CssClass = "tab-inactive";
                                    lnkDetail.CssClass = "tab-active";
                                    spnList.Attributes.Remove("class");
                                    spnDetail.Attributes.Remove("class");
                                    spnList.Attributes.Add("class", "tab-inactive");
                                    spnDetail.Attributes.Add("class", "tab-active");
                                    SetFieldValues(NewsManagementBO.ControlsEnum.GRID);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide();", true);
                                }
                                else
                                {
                                    ResetForm();
                                    GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                    SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                }
                                return;
                            }
                    }
                    //Error Shown when no Record is selected
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                    litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                    break;
                case ActionsEnum.DETAILS:
                    pnlEdit.Visible = true;
                    pnlSave.Visible = true;
                    foreach (GridViewRow grdrow in grdPage.Rows)
                    {
                        if (grdrow.FindControl("rbtSelect") != null)
                            if (((RadioButton)grdrow.FindControl("rbtSelect")).Checked) //Check for a selected row from the list
                            {
                                commonActions = ActionsEnum.EDIT_ACTION;
                                currPK = Convert.ToInt32(grdPage.DataKeys[grdrow.RowIndex].Values[0]);
                                GetFieldValues(NewsManagementBO.ControlsEnum.GRID);
                                txtTitle.Focus();
                                if (dtNews != null && dtNews.Rows.Count > 0)
                                {
                                    lnkList.CssClass = "tab-inactive";
                                    lnkDetail.CssClass = "tab-active";
                                    spnList.Attributes.Remove("class");
                                    spnDetail.Attributes.Remove("class");
                                    spnList.Attributes.Add("class", "tab-inactive");
                                    spnDetail.Attributes.Add("class", "tab-active");
                                    SetFieldValues(NewsManagementBO.ControlsEnum.GRID);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide();", true);
                                }
                                else
                                {
                                    ResetForm();
                                    GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                    SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                }
                                return;
                            }
                    }
                    //Error Shown when no Record is selected
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                    litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                    break;
                case ActionsEnum.VIEW:
                    foreach (GridViewRow grdrow in grdPage.Rows)
                    {
                        if (grdrow.FindControl("rbtSelect") != null)
                            if (((RadioButton)grdrow.FindControl("rbtSelect")).Checked) //Check for a selected row from the list
                            {
                                commonActions = ActionsEnum.EDIT_ACTION;
                                currPK = Convert.ToInt32(grdPage.DataKeys[grdrow.RowIndex].Values[0]);
                                GetFieldValues(NewsManagementBO.ControlsEnum.GRID);
                                if (dtNews != null && dtNews.Rows.Count > 0)
                                {
                                    lnkList.CssClass = "tab-inactive";
                                    lnkDetail.CssClass = "tab-active";
                                    spnList.Attributes.Remove("class");
                                    spnDetail.Attributes.Remove("class");
                                    spnList.Attributes.Add("class", "tab-inactive");
                                    spnDetail.Attributes.Add("class", "tab-active");
                                    SetFieldValues(NewsManagementBO.ControlsEnum.GRID);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide();", true);
                                }
                                else
                                {
                                    ResetForm();
                                    GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                    SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                }
                                return;
                            }
                    }
                    litErrorMsg.Text = Resources.Messages.Msg_Not_Selected.ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                    break;
                case ActionsEnum.NEW:
                    txtTitle.Focus();
                    pnlSave.Visible = true;
                    ResetForm();
                    lnkList.CssClass = "tab-inactive";
                    lnkDetail.CssClass = "tab-active";
                    spnList.Attributes.Remove("class");
                    spnDetail.Attributes.Remove("class");
                    spnList.Attributes.Add("class","tab-inactive");
                    spnDetail.Attributes.Add("class","tab-active");
                    GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                    SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide();", true);
                    break;
                case ActionsEnum.DELETE:
                    result = NewsManagementBL.DeleteNews(currPK, currentUser.PKUser);
                    switch (result)
                    {
                        case (int)DbDeleteStatus.DELETED: //deleted successfully
                            ResetForm();
                            GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                            SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide('ADD_DETAILS');", true);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success.ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Title_Information + "');", true);
                            break;
                        case (int)DbDeleteStatus.SQLERROR: //sql error
                            pnlSave.Visible = true;
                            pnlEdit.Visible = true;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error.ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                            break;
                        case (int)DbDeleteStatus.REFERRED: //Check whether the selected record is referred
                            pnlSave.Visible = true;
                            pnlEdit.Visible = true;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref.ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                            break;
                        case (int)DbDeleteStatus.DELETECONCURRENCY:
                            ResetForm();
                            GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                            SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                            break;
                    }
                    break;
                case ActionsEnum.ACTIVATE:
                    //Activate the status of a selected record
                    if (currPK != 0) //Check for the primary key of a selected row
                    {
                        result = NewsManagementBL.StatusUpdate(currPK, DbActiveStatus.ACTIVE, currentUser.PKUser, HttpUtility.HtmlEncode(hdfMofidiedOn.Value));
                        switch (result)
                        {
                            case (int)DBActiveInactiveStatus.SUCCESS:
                                ResetForm();
                                litErrorMsg.Text = Resources.Messages.Msg_Activate_Success;
                                GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide('ADD_DETAILS');", true);

                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                //register script for showing the error message
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Title_Information.ToString() + "');", true);
                                break;
                            case (int)DBActiveInactiveStatus.SQLERROR:
                                pnlSave.Visible = true;
                                pnlEdit.Visible = true;
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                                //register script for showing the error message
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                break;
                            case (int)DBActiveInactiveStatus.CONCURRENCY:
                                //pnlSave.Visible = true;
                                //pnlEdit.Visible = true;
                                ResetForm();
                                GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Concurrent;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                                //register script for showing the error message
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                break;
                            case (int)DBActiveInactiveStatus.DELETECONCURRENCY:
                                //pnlSave.Visible = true;
                                //pnlEdit.Visible = true;
                                ResetForm();
                                GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                                //register script for showing the error message
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                break;
                        }
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.ErpRes.Msg_Save_Error.ToString();
                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                    }
                    break;
                case ActionsEnum.DEACTIVATE:
                    //Deactivate the selected record
                    if (currPK != 0) //Check for the primary key of a selected row
                    {
                        result = NewsManagementBL.StatusUpdate(currPK, DbActiveStatus.INACTIVE, currentUser.PKUser, HttpUtility.HtmlEncode(hdfMofidiedOn.Value));
                        switch (result)
                        {
                            case (int)DBActiveInactiveStatus.SUCCESS:
                                ResetForm();
                                GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide('ADD_DETAILS');", true);
                                litErrorMsg.Text = Resources.Messages.Msg_Dectivate_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                //register script for showing the error message
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Title_Information.ToString() + "');", true);
                                break;
                            case (int)DBActiveInactiveStatus.SQLERROR:
                                pnlSave.Visible = true;
                                pnlEdit.Visible = true;
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                                //register script for showing the error message
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                break;
                            case (int)DBActiveInactiveStatus.CONCURRENCY:
                                ResetForm();
                                GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Concurrent;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                                //register script for showing the error message
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                break;
                            case (int)DBActiveInactiveStatus.DELETECONCURRENCY:
                                ResetForm();
                                GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                                //register script for showing the error message
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                break;
                        }
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.ErpRes.Msg_Save_Error.ToString();
                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                    }
                    break;

                case ActionsEnum.PRINT:
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Print_Error;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Msg_Delete").ToString());
                    //register script for showing the error message
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide('ADD_DETAILS');", true);
                    break;
            }
        }
        #endregion

        #region --- For Grid Actions----
        /// <summary>
        /// Page Index Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            ResetForm();
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
            BindGrid();
            
        }
        /// <summary>
        /// Sorting Event Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            Type type = typeof(NewsManagements);
            FieldInfo fieldInfo = type.GetField(e.SortExpression);
            SortExpression = Convert.ToString(fieldInfo.GetValue(0));
            if (SortOrder == "asc")
                SortOrder = "desc";
            else
                SortOrder = "asc";
            ResetForm();
            GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
            BindGrid();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide('ADD_DETAILS');", true);

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
                string[] dataKeyArray;
                dataKeyArray = new string[1];
                dataKeyArray[0] = NewsManagements.F_PK;
                grdPage.DataKeyNames = dataKeyArray; //Assign DataKey for Grid Here

                GetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);
                SetFieldValues(NewsManagementBO.ControlsEnum.DEFAULT);

                
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
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnActivate.PreRender += new EventHandler(btnAction_PreRender);
            this.btnInActivate.PreRender += new EventHandler(btnAction_PreRender);

            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);

            this.btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCanel.PreRender += new EventHandler(btnAction_PreRender);
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
        /// Binds the Currency grid with data
        /// </summary>
        private void BindGrid()
        {
            if (dtNews != null && dtNews.Rows.Count > 0)
            {
                SortExpression = SortExpression == null ? NewsManagements.F_TITLE : SortExpression;//TODO: get the value from constants
                SortOrder = SortOrder == null ? "asc" : SortOrder;
                PageIndex = PageIndex == null ? "0" : PageIndex;
                SortFilter = SortExpression + " " + SortOrder;
                dtNews.DefaultView.Sort = SortFilter;
                grdPage.PageIndex = Convert.ToInt32(PageIndex);
            }
            grdPage.DataSource = dtNews.DefaultView;
            grdPage.DataBind();
        }

        /// <summary>
        /// Resets the form for a fresh entry
        /// </summary>
        private void ResetForm()
        {
            //Codes for Clearing the controls in the page
            commonActions = ActionsEnum.ADD_ACTION;
            currPK = 0;
            txtTitle.Text = string.Empty;
            txtShortDesc.Text = string.Empty;
            txtPublishedDt.Text = string.Empty;
            txtDetails.Value = string.Empty;
            lblLastModBy.Text = string.Empty;
            hdfMofidiedOn.Value = string.Empty;
            //hdfPublishedDt.Value = string.Empty;
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private NewsManagementBO SetUIValuesToObject()
        {
            NewsManagementBO newsManagementBO;
            newsManagementBO = new NewsManagementBO();

            newsManagementBO.NewsPK = currPK;
            newsManagementBO.Title = HttpUtility.HtmlEncode(txtTitle.Text.Trim());
            newsManagementBO.PublishedDt = Convert.ToDateTime(txtPublishedDt.Text.Trim());
            newsManagementBO.ShortDesc = HttpUtility.HtmlEncode(txtShortDesc.Text.Trim());
            newsManagementBO.Details = HttpUtility.HtmlEncode(txtDetails.Value.Trim());
            newsManagementBO.CreatedBy = currentUser.PKUser;
            newsManagementBO.LastModDate = hdfMofidiedOn.Value == string.Empty ? "0" : hdfMofidiedOn.Value;
            //newsManagementBO.ActiveStatus = 1;
            return newsManagementBO;
        }

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            txtTitle.Text = HttpUtility.HtmlDecode(dtNews.Rows[0][NewsManagements.F_TITLE].ToString());
            txtShortDesc.Text = HttpUtility.HtmlDecode(dtNews.Rows[0][NewsManagements.F_SHORTDESC].ToString());
            txtPublishedDt.Text = Convert.ToDateTime(dtNews.Rows[0][NewsManagements.F_PUBLISHEDDT]).ToString(Resources.Constants.DateFormatShort);
            txtDetails.Value = HttpUtility.HtmlDecode(dtNews.Rows[0][NewsManagements.F_DETAILS].ToString());
            currPK = Convert.ToInt32(dtNews.Rows[0][NewsManagements.F_PK]);
            lblLastModBy.Text = Resources.ErpRes.LastModBy.ToString();
            lblLastModBy.Text = string.Format(Resources.ErpRes.LastModBy, Convert.ToDateTime(dtNews.Rows[0][NewsManagements.F_LASTMODON]).ToString());
            hdfMofidiedOn.Value = dtNews.Rows[0][NewsManagements.F_LASTMODON].ToString();

        }


        #endregion
    }
}