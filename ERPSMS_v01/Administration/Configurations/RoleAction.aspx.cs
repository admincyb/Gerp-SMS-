using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.CommonManagement;
using GTIService;
using GTIService.Constants.Common;
using BusinessObject.AccountManagement;
using System.Xml;
using System.Data;
using BusinessObject.Administration.Configurations;
using GTIService.Constants.Administration.Configurations;
using BusinessLogic.Administration.Configurations;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class RoleAction : System.Web.UI.Page
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
        ///// Current PK
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

        //#endregion
        //// Holds the current logged in user
        //private IdentityUser currentUser;

        ////Data table for binding the  Grid
        //private DataTable dtRoles;
        //private DataSet dsRoleActions;
        //private XmlDocument xdRoleAction;


        //// Indicates the state as well as action
        //private ActionsEnum commonActions;
        //private int rolePK;
        //#endregion

        //#region Set Page Variables
        ///// <summary>
        ///// Set the Page Level variables and properties
        ///// </summary>
        //private void SetPageVariables()
        //{
        //    //Initialze the current logged in user to the currentUser variable
        //    currentUser = (IdentityUser)HttpContext.Current.User.Identity;
        //    //Registering event for treeview selection 
        //    trvRoleAction.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");

        //    //Adding tooltip for dropdown.
        //    // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AddTooltipRoles", " GrandScriptUtils.AddToolTip($('[id$=ddlRoles]').attr('id'));", true);


        //}
        //#endregion

        //#region Get Field Values

        ///// <summary>
        ///// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        ///// pass string.empty to get all field values
        ///// Assign it to the page level variables
        ///// </summary>
        //private void GetFieldValues(RoleActionBO.ControlsEnum controlType)
        //{

        //    DbActiveStatus opMode; //Indicates which mode of operation need to be performed in DB
        //    switch (controlType)
        //    {
        //        case RoleActionBO.ControlsEnum.TREE:
        //            opMode = commonActions == ActionsEnum.EDIT_ACTION ? DbActiveStatus.HASPK : DbActiveStatus.INACTIVE; //2 - Edit mode ; 0 - Normal Lists all
        //            xdRoleAction = RoleActionBL.GetRoleActionsXml(CurrPK, opMode, currentUser.CurrentSBUPK);
        //            break;
        //        case RoleActionBO.ControlsEnum.ROLES:
        //            dtRoles = CommonBL.GetUsrGroups(0);
        //            break;
        //        default: // if passed nothing or string.empty(), then Meand Default Bing Will Bind all the Data in initial stage
        //            xdRoleAction = RoleActionBL.GetRoleActionsXml(0, DbActiveStatus.ALL, currentUser.CurrentSBUPK);
        //            dtRoles = CommonBL.GetUsrGroups(0);

        //            break;
        //    }
        //}

        //#endregion

        //#region Set Field Values

        ///// <summary>
        ///// All Field(Input controls, grids, dropdowns) values are assigned here.
        ///// To Set all fields, pass "string.Empty()"
        ///// </summary>
        //private void SetFieldValues(RoleActionBO.ControlsEnum controlType)
        //{
        //    switch (controlType)
        //    {
        //        case RoleActionBO.ControlsEnum.TREE:
        //            if (commonActions == ActionsEnum.EDIT_ACTION) // Edit mode. Fill the details of the edit record to the corresponding fields
        //            {
        //                if (dsRoleActions.Tables[0] != null)
        //                {
        //                    if (dsRoleActions.Tables[0].Rows.Count > 0)
        //                    {
        //                        GetUIValuesFromObject();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                BindTree();
        //            }
        //            break;
        //        case RoleActionBO.ControlsEnum.ROLES:
        //            BindDropDown(RoleActionBO.ControlsEnum.ROLES);
        //            break;
        //        default: // if passed nothing or string.empty(), then Bind for Initail
        //            BindTree();
        //            BindDropDown(RoleActionBO.ControlsEnum.ROLES);

        //            break;
        //    }
        //}

        //#endregion

        //#region Action Handlers

        //#region -- For Buttons & Image Buttons---
        ///// <summary>
        ///// For Button Click (Save/Cancel)
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ActionHandler(object sender, EventArgs e)
        //{

        //    if (sender.GetType().IsEquivalentTo(typeof(Button)))//cheking gettype is button
        //    {
        //        commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
        //    }
        //    else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))//cheking gettype is DropDownList
        //    {
        //        commonActions = ActionsEnum.CHANGE;
        //    }
        //    switch (commonActions)
        //    {
        //        case ActionsEnum.SAVE:// SAVE .
        //            if (!IsValid)//if page is not valid
        //            {
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_SavSuccess;
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
        //            }
        //            else//if page is valid.
        //            {
        //                SaveRoleAction();
        //            }
        //            break;
        //        case ActionsEnum.CHANGE:
        //            if (((DropDownList)sender).ID.ToLower() == RoleActions.DROPDOWNROLES.ToLower())
        //            {
        //                if (ddlRoles.SelectedValue != CommonConstants.SELECTVAL)
        //                {
        //                    CurrPK = Convert.ToInt32(ddlRoles.SelectedValue);
        //                    GetFieldValues(RoleActionBO.ControlsEnum.TREE);
        //                    SetFieldValues(RoleActionBO.ControlsEnum.TREE);
        //                }
        //                else
        //                {
        //                    GetFieldValues(RoleActionBO.ControlsEnum.DEFAULT);
        //                    SetFieldValues(RoleActionBO.ControlsEnum.DEFAULT);
        //                }
        //            }
        //            break;
        //    }
        //}

        //protected void ActionHandler(object sender, ImageClickEventArgs e)
        //{
        //    if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))//cheking gettype is button
        //    {
        //        commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
        //    }
        //    if (((ImageButton)sender).ID.ToLower() == "imgbtnsave")
        //    {
        //        SaveRoleAction();
        //    }
        //    if (((ImageButton)sender).ID.ToLower() == "imgbtncancel")
        //    {
        //        GetFieldValues(RoleActionBO.ControlsEnum.DEFAULT);
        //        SetFieldValues(RoleActionBO.ControlsEnum.DEFAULT);

        //    }
        //}
        //#endregion

        ///// <summary>
        ///// Mehide Used to set the field after the tree bind
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ActionHandler(object sender, TreeNodeEventArgs e)
        //{
        //    XmlElement xDataItem;
        //    xDataItem = (XmlElement)e.Node.DataItem;

        //    if (xDataItem.Attributes[RoleActions.F_ROLE_FLAG] != null)//cheking role flag is null or not
        //    {
        //        e.Node.Checked = Convert.ToBoolean(xDataItem.Attributes[RoleActions.F_ROLE_FLAG].Value);//checking checkbox

        //    }
        //    if (xDataItem.Attributes[RoleActions.F_SECTION_FLAG] != null)//cheking section flag is null or not
        //    {
        //        e.Node.Checked = Convert.ToBoolean(xDataItem.Attributes[RoleActions.F_SECTION_FLAG].Value);//checking checkbox

        //    }
        //    if (xDataItem.Attributes[RoleActions.F_PAGE_FLAG] != null)//cheking PAGE flag is null or not
        //    {
        //        e.Node.Checked = Convert.ToBoolean(xDataItem.Attributes[RoleActions.F_PAGE_FLAG].Value);//checking checkbox

        //    }



        //}
        //#endregion

        //#region Page Level Events
        ///// <summary>
        ///// To handle pageload event
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    SetPageVariables();
        //    if (!IsPostBack)
        //    {
        //        GetFieldValues(RoleActionBO.ControlsEnum.DEFAULT);
        //        SetFieldValues(RoleActionBO.ControlsEnum.DEFAULT);
        //        ddlRoles.Focus();//setting focus on role dropdown.
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

        //}

        ///// <summary>
        ///// To handle the visibility of the action corresponding to the users Privilage
        ///// Leave this section if using Master Screens
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void btnAction_PreRender(object sender, EventArgs e)
        //{

        //}

        //#endregion

        //#region Helper Methods

        ///// <summary>
        ///// Binds the RoleAction tree with XML data
        ///// </summary>
        //private void BindTree()
        //{
        //    trvRoleAction.Nodes.Clear();
        //    XmlDataSource xdsRoleAction;
        //    xdsRoleAction = new XmlDataSource();
        //    xdsRoleAction.ID = DateTime.Now.Ticks.ToString();
        //    xdsRoleAction.Data = xdRoleAction.OuterXml;
        //    trvRoleAction.DataSource = xdsRoleAction;
        //    trvRoleAction.DataBind();
        //    xdsRoleAction = new XmlDataSource();


        //}
        ///// <summary>
        ///// function used to bind Drop Downs Corresponding to the Drop Down passed
        ///// </summary>
        //private void BindDropDown(RoleActionBO.ControlsEnum drpName)
        //{
        //    switch (drpName)
        //    {
        //        case RoleActionBO.ControlsEnum.ROLES:
        //            if ((dtRoles != null) && (dtRoles.Rows.Count > 0))
        //            {
        //                ddlRoles.DataSource = CommonFunctions.HtmlDecodeDataTable(dtRoles, RoleActions.F_CODE_NAME);
        //                ddlRoles.DataTextField = RoleActions.F_CODE_NAME;
        //                ddlRoles.DataValueField = RoleActions.F_PK;
        //                ddlRoles.DataBind();
        //            }
        //            ddlRoles.Items.Insert(0, new ListItem("-Select-", "-1"));
        //            break;

        //        default:
        //            break;
        //    }

        //}
        ///// <summary>
        ///// method used for taking action pk if tree node checked..
        ///// </summary>
        //private string SetUseRoleAction()
        //{

        //    XmlDocument doc;
        //    doc = new XmlDocument();
        //    XmlElement root;
        //    XmlElement roleActionList;
        //    XmlElement roleActionElem;
        //    root = doc.CreateElement("Root");
        //    doc.AppendChild(root);
        //    foreach (TreeNode nodeRoleAction in trvRoleAction.CheckedNodes)
        //    {
        //        if (nodeRoleAction.Value != "0")
        //        {
        //            roleActionList = doc.CreateElement("ROLEACTIONS");
        //            roleActionElem = doc.CreateElement(RoleActions.F_ACT_PK);
        //            roleActionElem.InnerText = nodeRoleAction.Value;
        //            roleActionList.AppendChild(roleActionElem);
        //            root.AppendChild(roleActionList);

        //        }
        //    }
        //    return doc.InnerXml;

        //}

        ///// <summary>
        ///// Resets the form for a fresh entry
        ///// </summary>
        //private void ResetForm()
        //{
        //    //Codes for Clearing the controls in the page
        //    commonActions = ActionsEnum.ADD_ACTION;
        //    CurrPK = 0;
        //}

        ///// <summary>
        ///// Assigns the object with corresponding input control values
        ///// </summary>
        ///// <returns></returns>
        //private RoleActionBO SetUIValuesToObject()
        //{
        //    RoleActionBO objRoleAction;
        //    objRoleAction = new RoleActionBO();
        //    objRoleAction.BizUnit = currentUser.CurrentSBUPK;
        //    objRoleAction.ActiveStatus = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
        //    objRoleAction.RolePK = Convert.ToInt32(ddlRoles.SelectedValue);
        //    return objRoleAction;
        //}
        ///// <summary>
        ///// Sets the UI input controls from the object values
        ///// </summary>
        //private RoleActionBO GetUIValuesFromObject()
        //{
        //    RoleActionBO objRoleAction;
        //    objRoleAction = new RoleActionBO();
        //    objRoleAction.BizUnit = currentUser.CurrentSBUPK;
        //    objRoleAction.ActiveStatus = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
        //    objRoleAction.RolePK = Convert.ToInt32(ddlRoles.SelectedValue);
        //    return objRoleAction;
        //}
        ///// <summary>
        ///// Method used for saving action details.
        ///// </summary>
        //private void SaveRoleAction()
        //{
        //    int result;
        //    DbSaveStatus saveStatus;
        //    RoleActionBO objRoleAction;
        //    objRoleAction = SetUIValuesToObject();
        //    string xmlCostCenter = SetUseRoleAction();
        //    result = RoleActionBL.SaveRoleActions(objRoleAction, xmlCostCenter, currentUser.UserPK);

        //    if (result > 0)// Success ! re-initialize the page
        //    {
        //        ResetForm();
        //        GetFieldValues(RoleActionBO.ControlsEnum.DEFAULT);
        //        SetFieldValues(RoleActionBO.ControlsEnum.DEFAULT);
        //        litErrorMsg.Text = Resources.ErrorMessages.Msg_SavSuccess;
        //        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Role_Action").ToString());
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
        //    }
        //    else
        //    {
        //        saveStatus = (DbSaveStatus)result;
        //        switch (saveStatus)
        //        {

        //            case DbSaveStatus.SQLERROR://SQl Error
        //                //Scrip register for hiding the Details Part
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            case DbSaveStatus.CODEEXIST://CODEEXIST
        //                //Scrip register for hiding the Details Part
        //                litErrorMsg.Text = this.GetLocalResourceObject("Err_Code_Dupl").ToString();
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            case DbSaveStatus.CONCURRENCY://Cuncurrency Check
        //                //Scrip register for hiding the Details Part
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Concurrent;
        //                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Role_Action").ToString());
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            case DbSaveStatus.ALREADYDELETED://Cuncurrency Check
        //                //Scrip register for hiding the Details Part
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
        //                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Role_Action").ToString());
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //            default:
        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                break;
        //        }
        //    }


        //}

        //#endregion
    }
}