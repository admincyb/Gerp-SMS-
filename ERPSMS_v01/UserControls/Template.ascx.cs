using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MailCore.BO;
using MailCore.Utility;
using MailSendCore;
using System.Reflection;

namespace MailSend.UserControl
{
    public partial class Template : System.Web.UI.UserControl
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
        #endregion





        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private int userId = 0;
        private string templateString = string.Empty;
         private DataTable dtAppType;
        private DataTable dtActionType;
        private DataTable dttemplateType;
        private DataTable dtTemplates;
        private DataTable dtTemplateTags;
        private DataTable dtSubApp;
        #endregion

        #region Set Page Variables
        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {

            string[] dataKeyArray;
            dataKeyArray = new string[1];
            //dataKeyArray[0] = ViewMail.F_PK;
            //grdPage.DataKeyNames = dataKeyArray;
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
                //DataTable dtTemplateTags = TemplateManager.TemplateTagsGet(0,Convert.ToInt32(ddlApp.SelectedValue));
                case ViewMailsBO.ControlsEnum.ACTIONTYPE:
                    dtActionType = MailSendManager.GetActionType();
                    break;
                case ViewMailsBO.ControlsEnum.GRID:
                    dtTemplates = TemplateManager.TemplateDetailsGet(0, Convert.ToInt32(ddlmod.SelectedValue == "-1" ? "0" : ddlmod.SelectedValue), Convert.ToInt32(ddlApp.SelectedValue == "-1" ? "0" : ddlApp.SelectedValue), Convert.ToInt32(ddlsubApp.SelectedValue == "-1" ? "0" : ddlsubApp.SelectedValue), Convert.ToInt32(ddlTempType.SelectedValue == "-1" ? "0" : ddlTempType.SelectedValue), Convert.ToInt32(ddlAction.SelectedValue == "-1" ? "0" : ddlAction.SelectedValue));
                    break;
                case ViewMailsBO.ControlsEnum.GRIDTag:

                    dtTemplateTags = TemplateManager.TemplateTagsGet(0, ApplicationId);
                    break;
                case ViewMailsBO.ControlsEnum.TEMPLATETYPE:
                    dttemplateType = TemplateManager.GetCFG("Template Type");
                    break;
                case ViewMailsBO.ControlsEnum.SUBAPP:
                    dtSubApp = MailSendManager.GetSubApp(Convert.ToInt32(ddlApp.SelectedValue));
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
        private void SetFieldValues(ViewMailsBO.ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ViewMailsBO.ControlsEnum.GRID:
                    if (commonActions == ActionsEnum.EDIT_ACTION) // Edit mode. Fill the details of the edit record to the corresponding fields
                    {
                        if (dtTemplates != null)
                        {
                            if (dtTemplates.Rows.Count > 0)
                            {
                                //GetUIValuesFromObject();
                            }
                        }
                    }
                    else
                    {
                        BindGrid();
                    }
                    break;

                case ViewMailsBO.ControlsEnum.APPTYPE:
                    BindDropDown(ViewMailsBO.ControlsEnum.APPTYPE);
                    break;
                case ViewMailsBO.ControlsEnum.TEMPLATETYPE:
                    BindDropDown(ViewMailsBO.ControlsEnum.TEMPLATETYPE);
                    break;
                case ViewMailsBO.ControlsEnum.GRIDTag:
                    FillTemplateTags();
                    break;
                case ViewMailsBO.ControlsEnum.ACTIONTYPE:
                    BindDropDown(ViewMailsBO.ControlsEnum.ACTIONTYPE);
                    break;
                case ViewMailsBO.ControlsEnum.SUBAPP:
                    BindDropDown(ViewMailsBO.ControlsEnum.SUBAPP);
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

            switch (commonActions)
            {
                case ActionsEnum.Editrow:
                    
                    LinkButton lnkEditTemplate = (LinkButton)sender;
                    TemplateType = Convert.ToInt32(lnkEditTemplate.CommandArgument);
                    TemplateType = Convert.ToInt32(lnkEditTemplate.CommandArgument);
                    TemplateBO template = TemplateManager.GetTemplateDetails(TemplateType,2);
                    if (template != null)
                    {
                        divEditor.Visible = true;
                        pnlEntry.Visible = true;
                        // ddlProcess.SelectedValue = template.Process_ID.ToString();
                        //  ddlTemplatetype.SelectedValue = template.TemplateType_ID.ToString();
                        FCKeditor1.Value = template.TemplateValue;
                    }
                    //Fill the Tags Corresponding to the template
                    GetFieldValues(ViewMailsBO.ControlsEnum.GRIDTag);
                    SetFieldValues(ViewMailsBO.ControlsEnum.GRIDTag);
                    // ScriptManager.GetCurrent(this).SetFocus(fckEditor1);
                    break;
                case ActionsEnum.SAVE:
                    if(Session["UserPk"]!=null)
                        userId = Convert.ToInt16(Session["UserPk"].ToString());
                    else
                        userId=0;
                    TemplateType = Convert.ToInt16(TemplateType);
                    templateString = FCKeditor1.Value;
                    TemplateManager.TemplateSave(TemplateType, templateString, userId);
                    ClearControls();
                    GetFieldValues(ViewMailsBO.ControlsEnum.GRID);
                    SetFieldValues(ViewMailsBO.ControlsEnum.GRID);
                    divEditor.Visible = false;
                    pnlEntry.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + string.Format(Resources.Messages.Msg_Save_Success.ToString(), "Template") + "','" + Resources.Messages.Information + "');", true);
                    //ScriptManager.GetCurrent(this).SetFocus(ddlProcess);
                    break;
                case ActionsEnum.SHOW:
                    GetFieldValues(ViewMailsBO.ControlsEnum.GRID);
                    SetFieldValues(ViewMailsBO.ControlsEnum.GRID);
                    divEditor.Visible = false;
                    pnlEntry.Visible = false;
                    //ScriptManager.GetCurrent(this).SetFocus(ddlProcess);
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
            grdTemplate.PageIndex = e.NewPageIndex;
            PageIndex = e.NewPageIndex.ToString();
            int ParameterPK1 = 0;
            if (ParameterPK1 == 0)
                ParameterPK1 = 0;
            else
                ParameterPK1 = Convert.ToInt32(ParameterPK1);

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
           // Type type = typeof(ViewMail);
            //FieldInfo fieldInfo = type.GetField(e.SortExpression);
            SortExpression = e.SortExpression.ToString(); //Convert.ToString(fieldInfo.GetValue(0));
            if (SortOrder == "asc")
                SortOrder = "desc";
            else
                SortOrder = "asc";
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
            GetFieldValues(ViewMailsBO.ControlsEnum.GRID);
            BindGrid();

        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                LinkButton lbnProcess = (LinkButton)e.Row.FindControl("lbnProcess");

                // For general Process, It have no Process Name in Process Table
                if (lbnProcess.Text == string.Empty)
                {
                    lbnProcess.Text = "General";
                }
            }
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {

            ApplicationId = Int32.Parse(grdTemplate.DataKeys[Int32.Parse(e.NewEditIndex.ToString())].Values["TML_PK"].ToString());
            GetFieldValues(ViewMailsBO.ControlsEnum.GRIDTag);
            SetFieldValues(ViewMailsBO.ControlsEnum.GRIDTag);

            TemplateBO template = TemplateManager.GetTemplateDetails(TemplateType, 2);
            if (template != null)
            {
                divEditor.Visible = true;
                imbSave.Visible = true;
                pnlEntry.Visible = true;
                // ddlProcess.SelectedValue = template.Process_ID.ToString();
                //  ddlTemplatetype.SelectedValue = template.TemplateType_ID.ToString();
                FCKeditor1.Value = template.TemplateValue;
            }
            e.NewEditIndex=-1;
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
            if (!Page.IsPostBack)
            {
                
                pnlEntry.Visible = false;
                divEditor.Visible = false;

                GetFieldValues(ViewMailsBO.ControlsEnum.APPTYPE);
                SetFieldValues(ViewMailsBO.ControlsEnum.APPTYPE);
                GetFieldValues(ViewMailsBO.ControlsEnum.SUBAPP);
                SetFieldValues(ViewMailsBO.ControlsEnum.SUBAPP);
                GetFieldValues(ViewMailsBO.ControlsEnum.TEMPLATETYPE);
                SetFieldValues(ViewMailsBO.ControlsEnum.TEMPLATETYPE);
                GetFieldValues(ViewMailsBO.ControlsEnum.ACTIONTYPE);
                SetFieldValues(ViewMailsBO.ControlsEnum.ACTIONTYPE);
                GetFieldValues(ViewMailsBO.ControlsEnum.GRID);
                SetFieldValues(ViewMailsBO.ControlsEnum.GRID);
            }
        }




        #endregion

        #region HelperMethods




        public static Dictionary<int, string> GetEnumTemplateTypes()
        {
            Dictionary<int, string> ret = new Dictionary<int, string>();
            foreach (TemplateType templateType in Enum.GetValues(typeof(TemplateType)))
                ret.Add((int)templateType, CommonFunctions.ProperSpace(templateType.ToString()));
            return ret;
        }



        /// <summary>
        /// Method to get the TemplateList
        /// </summary>
        /// <author>Vineeth Babu</author>
        /// <createdon>03/Feb/10</createdon>
        public void BindGrid()
        {

            //This will return all Templates List
            if(dtTemplates!=null){
            //grdTemplate.DataSource = dtTemplates;
            //dtTemplates.DefaultView.Sort = "TML_NAME";
            SortExpression = SortExpression == null ? "TML_NAME" : SortExpression;//TODO: get the value from constants
            SortOrder = SortOrder == null ? "asc" : SortOrder;
            PageIndex = PageIndex == null ? "0" : PageIndex;
            SortFilter = SortExpression + " " + SortOrder;
            dtTemplates.DefaultView.Sort = SortFilter;
            grdTemplate.PageIndex = Convert.ToInt32(PageIndex);
            grdTemplate.DataSource = dtTemplates;
            grdTemplate.DataBind();
            foreach (GridViewRow grow in grdTemplate.Rows)
            {
                LinkButton lnkprocess = (LinkButton)grow.FindControl("lbnProcess");
                lnkprocess.Text = CommonFunctions.ProperSpace(lnkprocess.Text);
            }
            }
        }

        /// <summary>
        /// Method to Fill the TemplateTags
        /// </summary>
        /// <author>Vineeth Babu</author>
        /// <createdon>05/Feb/10</createdon>
        public void FillTemplateTags()
        {
            //This will return all Templates List
            
            if (dtTemplateTags != null)
            {
                grdTemplateTags.DataSource = dtTemplateTags.DefaultView;
                dtTemplateTags.DefaultView.Sort = "ATG_PK";
                grdTemplateTags.DataBind();
            }
        }

        /// <summary>
        /// Method to make the form in initial state
        /// </summary>
        /// <author>Vineeth Babu</author>
        /// <createdon>05/Feb/10</createdon>
        private void ClearControls()
        {
            TemplateType = 0;
            divEditor.Visible = false;
            pnlEntry.Visible = false;
        }
        private void BindDropDown(ViewMailsBO.ControlsEnum drpName)
        {
            switch (drpName)
            {
                case ViewMailsBO.ControlsEnum.ACTIONTYPE: //if CURRENCY entering to this case
                    if ((dtActionType != null) && (dtActionType.Rows.Count > 0))//cheking datatable has count or not if has binding dropdown
                    {
                        ddlAction.DataSource = CommonFunctions.HtmlDecodeDataTable(dtActionType, "ACT_NAME");
                        ddlAction.DataTextField = "ACT_NAME";
                        ddlAction.DataValueField = "ACT_PK";
                        ddlAction.DataBind();
                        ddlAction.Items.Insert(0, new ListItem(Resources.MailRes.Select, CommonConstants.SELECTVAL));
                        // ddlAction.SelectedIndex = ddlAction.Items.IndexOf(ddlAction.Items.FindByValue(CostCenterPK.ToString()));

                    }
                    else
                    {
                        ddlAction.Items.Clear();
                        ddlAction.Items.Insert(0, new ListItem(Resources.MailRes.Select, CommonConstants.SELECTVAL));
                    }
                    break;
                case ViewMailsBO.ControlsEnum.APPTYPE://if LOCATION entering to this case
                    if ((dtAppType != null) && (dtAppType.Rows.Count > 0))//cheking datatable has count or not if has binding dropdown
                    {
                        ddlApp.DataSource = CommonFunctions.HtmlDecodeDataTable(dtAppType, "APT_NAME");
                        ddlApp.DataTextField = "APT_NAME";
                        ddlApp.DataValueField = "APT_PK";
                        ddlApp.DataBind();
                        ddlApp.Items.Insert(0, new ListItem(Resources.MailRes.Select, CommonConstants.SELECTVAL));
                        //ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(locationPK.ToString()));

                    }
                    break;
                case ViewMailsBO.ControlsEnum.TEMPLATETYPE:
                    if ((dttemplateType != null) && (dttemplateType.Rows.Count > 0))
                    {
                        ddlTempType.DataSource = CommonFunctions.HtmlDecodeDataTable(dttemplateType, "CFG_DATA");
                        ddlTempType.DataTextField = "CFG_DATA";
                        ddlTempType.DataValueField = "CFG_PK";
                        ddlTempType.DataBind();
                        ddlTempType.Items.Insert(0, new ListItem(Resources.MailRes.Select, CommonConstants.SELECTVAL));
                        //ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(locationPK.ToString()));

                    }
                    break;
                case ViewMailsBO.ControlsEnum.SUBAPP:
                    if ((dtSubApp != null) && (dtSubApp.Rows.Count > 0))
                    {
                        ddlsubApp.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSubApp, "AST_NAME");
                        ddlsubApp.DataTextField = "AST_NAME";
                        ddlsubApp.DataValueField = "AST_PK";
                        ddlsubApp.DataBind();
                        ddlsubApp.Items.Insert(0, new ListItem(Resources.MailRes.Select, CommonConstants.SELECTVAL));
                        //ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(locationPK.ToString()));

                    }
                    break;
                default:
                    break;
            }

        }

        #endregion
    }
}