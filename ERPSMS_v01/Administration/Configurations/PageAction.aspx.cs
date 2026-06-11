using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Administration.Configurations;
using System.Data;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using GTIService.Constants.Administration.Configurations;
using GTIService.Constants.Common;
using BusinessObject.AccountManagement;
using GTIService;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class PageAction : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //#region Variables and Properties
        //#region Properties
        ///// <summary>
        ///// To maintain the sort order in viewstate
        ///// </summary>
        //private string SortOrder
        //{
        //    get
        //    {
        //        return (string)this.ViewState["SortOrder"];
        //    }
        //    set
        //    {
        //        this.ViewState["SortOrder"] = value;
        //    }
        //}

        ///// <summary>
        ///// To maintain the sort expression in viewstate
        ///// </summary>
        //private string SortExpression
        //{
        //    get
        //    {
        //        return (string)this.ViewState["SortExpression"];
        //    }
        //    set
        //    {
        //        this.ViewState["SortExpression"] = value;
        //    }
        //}

        ///// <summary>
        ///// To maintain the sort field in viewstate
        ///// </summary>
        //private string SortFilter
        //{
        //    get
        //    {
        //        return (string)this.ViewState["SortFilter"];
        //    }
        //    set
        //    {
        //        this.ViewState["SortFilter"] = value;
        //    }
        //}

        ///// <summary>
        ///// To maintain the PageIndex in viewstate
        ///// </summary>
        //private string PageIndex
        //{
        //    get
        //    {
        //        return (string)this.ViewState["PageIndex"];
        //    }
        //    set
        //    {
        //        this.ViewState["PageIndex"] = value;
        //    }
        //}

        ///// <summary>
        ///// Current PK (Primary Key of the current)
        ///// </summary>
        //private int CurrPK
        //{
        //    get
        //    {
        //        return Convert.ToInt32(this.ViewState["CurrPK"]);
        //    }
        //    set
        //    {
        //        this.ViewState["CurrPK"] = value;
        //    }
        //}
        ///// <summary>
        ///// GetDate  
        ///// </summary>
        //private string GetDate
        //{
        //    get
        //    {
        //        return Convert.ToString(this.ViewState["GetDate"]);
        //    }
        //    set
        //    {
        //        this.ViewState["GetDate"] = value;
        //    }
        //}
        ///// <summary>
        ///// Current ParentPK (Primary Key of the current Category Parent)
        ///// </summary>
        //private int CurrParent
        //{
        //    get
        //    {
        //        return Convert.ToInt32(this.ViewState["CurrParent"]);
        //    }
        //    set
        //    {
        //        this.ViewState["CurrParent"] = value;
        //    }
        //}
        //#endregion



        ////Data table for binding Actions grid
        //private DataSet dsActions;

        ////Datat Table for Bindng Page Details
        //private DataTable dtPage;

        //private string pagePK;

        //// Indicates the state as well as action
        //private ActionsEnum commonActions;

        //// Holds the current logged in user
        //private BusinessObject.AccountManagement.IdentityUser currentUser;

        //#endregion

        //#region Set Page Variables
        ///// <summary>
        ///// Set the Page Level variables and properties
        ///// </summary>
        //private void SetPageVariables()
        //{
        //    //Initialze the current logged in user to the currentUser variable
        //    currentUser = (BusinessObject.AccountManagement.IdentityUser)HttpContext.Current.User.Identity;
        //}
        //#endregion

        //#region Get Field Values

        ///// <summary>
        ///// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        ///// pass string.empty to get all field values
        ///// Assign it to the page level variables
        ///// </summary>
        //private void GetFieldValues(PageActionBO.ControlsEnum type)
        //{
        //    switch (type)
        //    {
        //        case PageActionBO.ControlsEnum.DEFAULT:
        //            dsActions = PageActionBL.GetPageActions(0, DbActiveStatus.ALL, currentUser.CurrentSBUPK);
        //            dtPage = PageActionBL.GetPages(0, DbActiveStatus.ACTIVE);
        //            break;
        //        case PageActionBO.ControlsEnum.EDITING:
        //            dsActions = PageActionBL.GetPageAction(int.Parse(hdfcurrPK.Value), DbActiveStatus.HASPK, currentUser.CurrentSBUPK);

        //            break;
        //        case PageActionBO.ControlsEnum.PAGEACTIONS:
        //            dsActions = PageActionBL.GetPageActions(0, DbActiveStatus.ALL, currentUser.CurrentSBUPK);
        //            break;
        //        case PageActionBO.ControlsEnum.ACTIONS:
        //            dsActions = PageActionBL.GetPageActions(0, DbActiveStatus.ALL, currentUser.CurrentSBUPK);
        //            break;

        //    }

        //}
        //#endregion

        //#region Set Field Values

        ///// <summary>
        ///// All Field(Input controls) values are assigned here.
        ///// To Set all fields, pass "string.Empty()"
        ///// </summary>
        //private void SetFieldValues(PageActionBO.ControlsEnum type)
        //{
        //    switch (type)
        //    {
        //        //to bind the Category DropDown
        //        case PageActionBO.ControlsEnum.DEFAULT:
        //            FillPages();
        //            BindGrid();
        //            break;
        //        case PageActionBO.ControlsEnum.PAGEACTIONS:
        //            BindGrid();
        //            break;
        //        case PageActionBO.ControlsEnum.EDITING:
        //            if (dsActions.Tables[0].Rows.Count > 0)
        //            {
        //                ddlPage.SelectedValue = dsActions.Tables[0].Rows[0][PageActions.F_ACTIONPAGEPK].ToString();
        //                txtAction.Text = dsActions.Tables[0].Rows[0][PageActions.F_ACTION].ToString();
        //                txtDesc.Text = dsActions.Tables[0].Rows[0][PageActions.F_DESCRIPTION].ToString();
        //                txtSection.Text = dsActions.Tables[0].Rows[0][PageActions.F_SECTION].ToString();
        //                hdfLastModDt.Value = dsActions.Tables[0].Rows[0][CommonConstants.LASTMODDATETIME].ToString();
        //            }
        //            break;
        //        case PageActionBO.ControlsEnum.ACTIONS:
        //            BindGrid();
        //            break;

        //    }
        //}

        //#endregion

        //#region Page Level Methords
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    SetPageVariables();
        //    if (!IsPostBack)
        //    {
        //        GetFieldValues(PageActionBO.ControlsEnum.DEFAULT);
        //        SetFieldValues(PageActionBO.ControlsEnum.DEFAULT);
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide1", "ShowListing('1')", true);
        //        //SetDisplaySection(PageActionBO.SectionsEnum.ListingSection);
        //    }
        //}


        ///// <summary>
        ///// To handle OnInit event Used to assign the Event for all the actions used in this page
        ///// Leave this section if using Master Screens
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    this.imbAdd.PreRender += new EventHandler(btnAction_PreRender);
        //    this.imbCancel.PreRender += new EventHandler(btnAction_PreRender);
        //    this.imbSave.PreRender += new EventHandler(btnAction_PreRender);

        //}

        ///// <summary>
        ///// To handle the visibility of the action corresponding to the users Privilage
        ///// Leave this section if using Master Screens
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void btnAction_PreRender(object sender, EventArgs e)
        //{
        //    //base.CheckBtnVisibility(sender);
        //}
        //#endregion

        //#region HelperMethords

        ///// <summary>
        ///// Methord used to to fill Page details to a drop down
        ///// </summary>
        //private void FillPages()
        //{
        //    ddlPage.Items.Clear();
        //    ddlPage.AppendDataBoundItems = true;
        //    ddlPage.Items.Add(new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECT_VALUE_ZERO));
        //    ddlPage.DataSource = dtPage;
        //    ddlPage.DataTextField = PageActions.F_PAGENAME;
        //    ddlPage.DataValueField = PageActions.F_PAGEPK;
        //    ddlPage.DataBind();
        //    ddlPage.SelectedValue = pagePK;

        //}

        ///// <summary>
        ///// Methord used to Bind Grid
        ///// </summary>
        //private void BindGrid()
        //{
        //    DataTable dtActions = dsActions.Tables[0];
        //    SortExpression = SortExpression == null ? "ACT_PK" : SortExpression;  //F_PK 4 temp
        //    SortOrder = SortOrder == null ? PageActions.C_DESC : SortOrder;
        //    PageIndex = PageIndex == null ? PageActions.C_ZERO_VAL : PageIndex;
        //    SortFilter = SortExpression + PageActions.C_EMPTY_VAL + SortOrder;
        //    dtActions.DefaultView.Sort = SortFilter;
        //    grdActions.PageIndex = Convert.ToInt32(PageIndex);
        //    grdActions.DataSource = dtActions.DefaultView;
        //    grdActions.DataBind();
        //}

        ///// <summary>
        ///// Methord used to save Actions
        ///// </summary>
        //private void SaveAction()
        //{
        //    DbSaveStatus saveStatus;
        //    int result;
        //    PageActionBO pageAct = SetUIValuesToObject();
        //    result = PageActionBL.SavePageAction(pageAct);
        //    if (result > 0) // Success ! re-initialize the page
        //    {
        //        ResetForm();
        //        GetFieldValues(PageActionBO.ControlsEnum.DEFAULT);
        //        SetFieldValues(PageActionBO.ControlsEnum.DEFAULT);
        //        litErrorMsg.Text = Resources.ErrorMessages.Msg_SavSuccess;
        //        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Page_Actions").ToString());
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide2", "ShowListing('1')", true);
        //    }
        //    else
        //    {
        //        saveStatus = (DbSaveStatus)result;
        //        switch (saveStatus)
        //        {
        //            case DbSaveStatus.REFERRED://SQl Error
        //                //Scrip register for hiding the Details Part
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowListing('1')", true);
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Ref;
        //                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Page_Actions").ToString());
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            case DbSaveStatus.SQLERROR://SQl Error
        //                //Scrip register for hiding the Details Part
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowListing();", true);
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            case DbSaveStatus.CODEEXIST://CODEEXIST
        //                //Scrip register for hiding the Details Part
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowListing('1');", true);
        //                litErrorMsg.Text = this.GetLocalResourceObject("Err_Code_Dupl").ToString();
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            case DbSaveStatus.CONCURRENCY://Cuncurrency Check
        //                //Scrip register for hiding the Details Part
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowListing('1');", true);
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Concurrent;
        //                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Page_Actions").ToString());
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            case DbSaveStatus.ALREADYDELETED://Cuncurrency Check
        //                //Scrip register for hiding the Details Part
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowListing('1');", true);
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
        //                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Page_Actions").ToString());
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            default:
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //        }

        //    }
        //}

        ///// <summary>
        ///// Methord used to Reset form
        ///// </summary>
        //private void ResetForm()
        //{
        //    hdfcurrPK.Value = "0";
        //    hdfLastModDt.Value = string.Empty;
        //    txtAction.Text = string.Empty;
        //    txtSection.Text = string.Empty;
        //    txtDesc.Text = string.Empty;
        //    ddlPage.SelectedValue = "0";
        //    //SetDisplaySection(PageActionBO.SectionsEnum.ListingSection);
        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide2", "ShowListing('1')", true);

        //}


        ///// <summary>
        ///// Methord used to delete Page Action
        ///// </summary>
        //private void DeletePageAction()
        //{
        //    int result;
        //    result = PageActionBL.DeletePageAction(int.Parse(hdfcurrPK.Value), Convert.ToDateTime(hdfLastModDt.Value));
        //    if (result > 0)
        //    {
        //        //litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowListing('1');", true);
        //        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
        //        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Page_Actions").ToString());
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
        //        ResetForm();
        //        GetFieldValues(PageActionBO.ControlsEnum.PAGEACTIONS);
        //        SetFieldValues(PageActionBO.ControlsEnum.PAGEACTIONS);
        //    }
        //    else
        //    {
        //        // DbDeleteStatus dbDeleteStatus = (Production.BO.CommonBO.DbDeleteStatus)(result);
        //        switch (result)
        //        {
        //            case (int)DbDeleteStatus.CONCURRENCY:
        //                //Scrip register for hiding the Details Part
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowListing('1');", true);
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
        //                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Page_Actions").ToString());
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            case (int)DbDeleteStatus.DELETECONCURRENCY:
        //                //Scrip register for hiding the Details Part
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowListing('1');", true);
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
        //                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Page_Actions").ToString());
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            case (int)DbDeleteStatus.SQLERROR:
        //                //Scrip register for hiding the Details Part
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowListing('1');", true);
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            case (int)DbDeleteStatus.REFERRED:
        //                //Scrip register for hiding the Details Part
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowListing('1');", true);
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
        //                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Page_Actions").ToString());
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            default:
        //                litErrorMsg.Text = this.GetLocalResourceObject("Err_Delete").ToString();
        //                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Page_Actions").ToString());
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //        }
        //    }


        //}

        ///// <summary>
        ///// Assigns the object with corresponding input control values
        ///// </summary>
        ///// <returns></returns>
        //private PageActionBO SetUIValuesToObject()
        //{
        //    PageActionBO pageAct = new PageActionBO();
        //    pageAct.PageActionPK = int.Parse(hdfcurrPK.Value);
        //    pageAct.Page = int.Parse(ddlPage.SelectedValue);
        //    pageAct.Section = txtSection.Text.Trim();
        //    pageAct.Action = txtAction.Text.Trim();
        //    pageAct.ActionDesc = txtDesc.Text.Trim();
        //    pageAct.User = currentUser.UserPK;
        //    pageAct.BizUnit = currentUser.CurrentSBUPK;
        //    if (int.Parse(hdfcurrPK.Value) != 0)
        //        pageAct.LastModDt = Convert.ToDateTime(hdfLastModDt.Value);
        //    return pageAct;
        //}


        ///// <summary>
        ///// Methord used to set the Display section
        ///// </summary>
        ///// <param name="Section"></param>
        //private void SetDisplaySection(PageActionBO.SectionsEnum Section)
        //{
        //    switch (Section)
        //    {
        //        case PageActionBO.SectionsEnum.EntrySection:
        //            PageAction_Entry.Visible = true;
        //            PageAction_List.Visible = false;
        //            imbAdd.Visible = false;
        //            imbSave.Visible = true;
        //            ddlPage.Focus();
        //            break;
        //        case PageActionBO.SectionsEnum.ListingSection:
        //            PageAction_List.Visible = true;
        //            PageAction_Entry.Visible = false;
        //            imbSave.Visible = false;
        //            imbAdd.Visible = true;
        //            break;
        //    }
        //}

        //#endregion

        //#region Action Handler
        //protected void ActionHandler(object sender, EventArgs e)
        //{
        //    if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
        //    {
        //        commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
        //    }
        //    switch (commonActions)
        //    {
        //        //Add mode to enter details to save
        //        case ActionsEnum.ADD:
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide2", "ShowListing()", true);
        //            //SetDisplaySection(PageActionBO.SectionsEnum.EntrySection);
        //            break;

        //        case ActionsEnum.SAVE:
        //            SaveAction();
        //            break;
        //        case ActionsEnum.GRIDEDIT:
        //            hdfcurrPK.Value = ((ImageButton)sender).CommandArgument.ToString();
        //            hdfLastModDt.Value = grdActions.DataKeys[((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex].Values[1].ToString();
        //            GetFieldValues(PageActionBO.ControlsEnum.EDITING);
        //            SetFieldValues(PageActionBO.ControlsEnum.EDITING);
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide3", "ShowListing()", true);
        //            if (dsActions.Tables[0].Rows.Count == 0)
        //            {
        //                GetFieldValues(PageActionBO.ControlsEnum.DEFAULT);
        //                SetFieldValues(PageActionBO.ControlsEnum.DEFAULT);
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide1", "ShowListing('1')", true);
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowNoData", "ShowNoData();", true);
        //            }
        //            break;
        //        case ActionsEnum.GRIDDELETE:
        //            hdfcurrPK.Value = ((ImageButton)sender).CommandArgument.ToString();
        //            hdfLastModDt.Value = grdActions.DataKeys[((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex].Values[1].ToString();
        //            DeletePageAction();
        //            break;
        //        case ActionsEnum.CANCEL:
        //            ResetForm();
        //            break;
        //    }

        //}

        //#region For Grid Actions----
        ///// <summary>
        ///// Sorting Event Handler for for Former grid
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ActionHandler(object sender, GridViewSortEventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide1", "ShowListing('1')", true);
        //    //Type obj = sender.GetType();
        //    //Type type = typeof(LinesMaster);
        //    //FieldInfo fieldInfo = type.GetField(e.SortExpression);
        //    SortExpression = e.SortExpression;
        //    if (SortOrder == "asc")
        //        SortOrder = "desc";
        //    else
        //        SortOrder = "asc";
        //    GetFieldValues(PageActionBO.ControlsEnum.PAGEACTIONS);
        //    SetFieldValues(PageActionBO.ControlsEnum.PAGEACTIONS);
        //}
        ///// <summary>
        ///// Page Index Handler for Former grid(grdLines)
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ActionHandler(object sender, GridViewPageEventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide1", "ShowListing('1')", true);
        //    PageIndex = e.NewPageIndex.ToString();
        //    GetFieldValues(PageActionBO.ControlsEnum.PAGEACTIONS);
        //    SetFieldValues(PageActionBO.ControlsEnum.PAGEACTIONS);
        //}
        //#endregion
        //#endregion
    }
}