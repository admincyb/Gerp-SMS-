using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPData;
using ERPService;
using System.Web.UI.HtmlControls;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.OrderToCash.UserControls
{
    public partial class CustomerRegistrationTabs : System.Web.UI.UserControl
    {
        #region Variables and Properties
        private ActionsEnum commonActions;
        
        /// <summary>
        /// Dynamic Tab Name
        /// </summary>
        public string DynamicTabName
        {
            get
            {
                return this.ViewState[ViewstateStrings.DynamicTabName].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.DynamicTabName] = value;
            }
        }
        /// <summary>
        /// Dynamic Tab Desc
        /// </summary>
        public string DynamicTabDesc
        {
            get
            {
                return this.ViewState[ViewstateStrings.DynamicTabDesc].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.DynamicTabDesc] = value;
            }
        }


        //private AD_APP_CONST_CFG adAppConstCfgObj;

        private List<SPADM_FORM_TAB_CFG_GET_Result> spAdmFormTabCfgGetResultList;
        #endregion
        #region Page Level Events
        /// <summary>
        /// handles page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        #endregion
        #region Page Action Handler
        /// <summary>
        /// Handles page load
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    //Get Dynamic Tab Details
                    GetFieldValues(ControlsEnum.DEFAULT);
                    //Set Dynamic Tab Details
                    SetFieldValues(ControlsEnum.DEFAULT);
                    if (Session[SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                    {
                        string tabCode = Session[SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString();
                        foreach (RepeaterItem item in rtrDynamicTab.Items)//Iterate tab list
                        {
                            if (((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CommandArgument == tabCode)
                            {
                                HtmlGenericControl Span = (HtmlGenericControl)item.FindControl("spnCustomerRegistrationTab");
                                //update session
                                Session[SessionStrings.DYNAMICTAB_SELECTEDTAB_NAME] = DynamicTabName = ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).Text;
                                //set tab desc
                                DynamicTabDesc = ((HiddenField)item.FindControl("hdfDynamicTabDesc")).Value;
                                if (tabCode.Equals(TabType.CLST))//Main list tab
                                {
                                    ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CssClass = "list-active";
                                    Span.Attributes.Add("class", "list-active");
                                    ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).Text = string.Empty;
                                }
                                else//normal tabs
                                {
                                    ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CssClass = "tab-active";
                                    Span.Attributes.Add("class", "tab-active");
                                }
                                break;
                                
                            }
                            else if (((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CommandArgument.Equals(TabType.CLST))//Main list tab
                            {
                                HtmlGenericControl Span = (HtmlGenericControl)item.FindControl("spnCustomerRegistrationTab");
                                ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CssClass = "list-active";
                                Span.Attributes.Add("class", "list-active");
                                ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).Text = string.Empty;
                            }
                        }
                    }
                    else//If no tab selected
                    {
                        foreach (RepeaterItem item in rtrDynamicTab.Items)//Iterate tab list
                        {
                            if (((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CommandArgument.Equals(TabType.CLST))//main list tab
                            {
                                ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CssClass = "list-active";
                                HtmlGenericControl Span = (HtmlGenericControl)item.FindControl("spnCustomerRegistrationTab");
                                Span.Attributes.Add("class", "list-active");
                            }
                            else//normal tabs
                            {
                                ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CssClass = "tab-active";
                                HtmlGenericControl Span = (HtmlGenericControl)item.FindControl("spnCustomerRegistrationTab");
                                Span.Attributes.Add("class", "tab-active");
                            }
                            //set session tab name
                            Session[SessionStrings.DYNAMICTAB_SELECTEDTAB_NAME] = DynamicTabName = ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).Text;
                            //set tab desc 
                            DynamicTabDesc = ((HiddenField)item.FindControl("hdfDynamicTabDesc")).Value;
                            //set session tab code
                            Session[SessionStrings.DYNAMICTAB_SELECTED_CODE] = ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CommandArgument;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            DynamicPageService DynamicPageServiceClient;
            DynamicPageServiceClient = null;
            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        DynamicPageServiceClient = new DynamicPageService();
                        DynamicPageServiceClient = CommonFunctions.InitiateClient(DynamicPageServiceClient);
                        spAdmFormTabCfgGetResultList = DynamicPageServiceClient.GetFormTabList(FormType.CUS);
                        break;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                DynamicPageServiceClient = null;
            }
        }
        #endregion
        #region Set Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void SetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        BindRepeater(ControlsEnum.DEFAULT);
                        break;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
        #region Action Handlers

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                Dictionary<int, string> dictionary;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                switch (commonActions)
                {

                    #region Tabs
                    case ActionsEnum.DYNAMICTAB:
                        //Get customerPK. IF no row selected from the main list , then can't go to any other tab
                        if (Session[SessionStrings.CUSTOMERPK] == null)
                        {
                            HiddenField hdfCustomerPK = (HiddenField)this.Parent.FindControl("hdfCustomerPK");
                            if (hdfCustomerPK != null && !string.IsNullOrEmpty(hdfCustomerPK.Value))
                            {
                                Session[SessionStrings.CUSTOMERPK] = hdfCustomerPK.Value;
                            }
                            else if(Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK]!=null)
                            {
                                Session[SessionStrings.CUSTOMERPK] = Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK];
                            }
                        }
                        
                        LinkButton lnkDynamicTab = sender as LinkButton;
                        if (Session[SessionStrings.CUSTOMERPK] != null || lnkDynamicTab.CommandArgument.Equals(TabType.CLST))
                        {
                            foreach (RepeaterItem item in rtrDynamicTab.Items)
                            {
                                if (((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CommandArgument.Equals(TabType.CLST))
                                {
                                    ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CssClass = "list-inactive";
                                }
                                else
                                {
                                    ((LinkButton)item.FindControl("lnkCustomerRegistrationTab")).CssClass = "tab-inactive";
                                }
                            }
                            if (lnkDynamicTab.CommandArgument.Equals(TabType.CLST))
                            {
                                lnkDynamicTab.CssClass = "list-active";
                            }
                            else
                            {
                                lnkDynamicTab.CssClass = "tab-active";
                            }
                            Session[SessionStrings.DYNAMICTAB_SELECTEDTAB_NAME] = lnkDynamicTab.Text;
                            Session[SessionStrings.DYNAMICTAB_SELECTED_CODE] = lnkDynamicTab.CommandArgument;
                            //set tab desc
                            DynamicTabDesc=((HiddenField)lnkDynamicTab.Parent.FindControl("hdfDynamicTabDesc")).Value;
                            //set tab name
                            DynamicTabName = lnkDynamicTab.Text;
                            Session[SessionStrings.SERVICETAB_ACTIVETAB] = ActiveTab.DYNAMICTAB;
                            if (Session["EntityByGroup"] != null)
                            {
                                dictionary = (Dictionary<int, string>)Session["EntityByGroup"];
                                foreach (KeyValuePair<int, string> item in dictionary)
                                {
                                    string[] stringArray = item.Value.Split('.');
                                    foreach (string sessionString in stringArray)
                                    {
                                        if (Session[sessionString] != null)
                                        {
                                            Session[sessionString] = null;
                                        }
                                    }
                                }
                            }
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.CustomerRegistration + "?Tab=" + lnkDynamicTab.CommandArgument), false);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('"+CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Select_Row").ToString())+"','" + Resources.Messages.Information + "');", true);
                        }
                        
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion


        #endregion
        #region Pager Methods + Init
        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
            
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            //this.lnkFuel.PreRender += new EventHandler(btnAction_PreRender);
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //(this.Page as MyBasePage).CheckBtnVisibility(sender);
        }
        #endregion
        #region Helper Mathods
       
        private void BindRepeater(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Default
                    case ControlsEnum.DEFAULT:
                        if (spAdmFormTabCfgGetResultList != null && spAdmFormTabCfgGetResultList.Count() > 0)
                        {
                            if (Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK] != null)
                            {
                                if (spAdmFormTabCfgGetResultList.SingleOrDefault(aa => aa.ATC_CODE == TabType.CLST) != null)
                                    spAdmFormTabCfgGetResultList.Remove(spAdmFormTabCfgGetResultList.SingleOrDefault(aa => aa.ATC_CODE == TabType.CLST));
                            }
                            rtrDynamicTab.DataSource = spAdmFormTabCfgGetResultList;
                            rtrDynamicTab.DataBind();
                        }
                        break;
                    #endregion
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
        #region ControlsEnum
        /// <summary>
        /// Controls Enum for the page
        /// </summary>
        private enum ControlsEnum
        {
            DEFAULT
        }
        /// <summary>
        /// Active Tab Enum for the page
        /// </summary>
        private enum ActiveTab
        {
            DYNAMICTAB=1
        }
        #endregion
    }
}