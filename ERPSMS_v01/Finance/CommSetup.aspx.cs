using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BusinessObject.AccountManagement;
using System.Data;
using ERP.Utilities;
//using BusinessObject.Finance;
using System.Xml;
using System.Threading;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.Finance
{
    public partial class CommSetup : ERP.Store.UI.MyBasePage //System.Web.UI.Page  ERP.Store.UI.MyBasePage//
    {
        #region "Variables And Properties"
        #region "Properties"
        /// <summary>
        /// To maintain the RateType Collection in viewstate
        /// </summary>
        private DataTable RateTypes
        {
            get
            {
                return (DataTable)this.ViewState["RateTypes"];
            }
            set
            {
                this.ViewState["RateTypes"] = value;
            }
        }

        /// <summary>
        /// To maintain the FormulaType Collection in viewstate
        /// </summary>
        private DataTable FormulaTypes
        {
            get
            {
                return (DataTable)this.ViewState["FormulaTypes"];
            }
            set
            {
                this.ViewState["FormulaTypes"] = value;
            }
        }

        /// <summary>
        /// Brand Item Insert for COV
        /// </summary>
        private bool IsAgentCommissionFormula
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsAgentCommissionFormula] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsAgentCommissionFormula].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsAgentCommissionFormula] = value;
            }
        }

        #endregion "Properties"

        private ActionsEnum commonActions;
        private BusinessObject.User currentUser;
        private DataTable dtCommision;
        private DataTable dtAgent;
        private DataTable dtFormula;
        private DataTable dtCustomers;
        private DataTable dtRateTypes;
        private DataTable dtMappedCommisions;
        private DataTable dtCurrency;
        private int customerId;
        private BusinessObject.Finance.CommSetupBO.AgentInfo agentInfoObj;
        XmlDocument xmlDoc;
        #endregion "Variables And Properties"

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }


        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    if (IsAgentCommissionFormula == true)
                    {
                        grdCommision.Columns[6].Visible = true;
                        // grdCommision.Columns[7].Visible = true;
                        grdCommisionView.Columns[5].Visible = true;
                        pop.Visible = true;
                    }
                    else
                    {
                        pop.Visible = false;
                    }
                    hfAgentPk.Value = Request.QueryString["AgentId"].ToString();

                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    List<object> objList = new List<object>();
                    for (int i = 0; i < this.Session.Count; i++)
                    {
                        object obj = Session[i];
                        objList.Add(obj);
                    }


                    GetFieldValues(ControlsEnum.AGENTDETAIL);
                    SetFieldValues(ControlsEnum.AGENTDETAIL);
                    GetFieldValues(ControlsEnum.MAPPEDCUSTOMERS);
                    SetFieldValues(ControlsEnum.MAPPEDCUSTOMERS);
                    // GetFieldValues(ControlsEnum.FROMULA);  
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }
        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>    
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                string senderId = string.Empty;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                    senderId = ((Button)sender).ID;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlCustomer")
                    {
                        commonActions = ActionsEnum.CHANGESTATUS;
                    }
                    if (((DropDownList)sender).ID == "ddlRateType")
                    {
                        //senderId = ((GridView)sender).ID;
                        commonActions = ActionsEnum.CHANGEVALUE;
                        //if (senderId == "grdCommision")
                        //{
                        //    commonActions = ActionsEnum.CHANGEVALUE;
                        //}
                    }
                    if (((DropDownList)sender).ID == "ddlFormulaAll")
                    {
                        commonActions = ActionsEnum.RESET;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtCommissionAll")
                    {
                        commonActions = ActionsEnum.RESET;
                    }
                }


                else if (sender.GetType().IsEquivalentTo(typeof(GridView)))
                {
                    senderId = ((GridView)sender).ID;
                    commonActions = ActionsEnum.DATABOUND;
                    if (senderId == "grdCommision" || senderId == "grdCommisionView")
                    {
                        commonActions = ActionsEnum.DATABOUND;
                    }


                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    senderId = ((RadioButton)sender).ID;
                    if (senderId == "rbtSelect")
                    {
                        commonActions = ActionsEnum.TASKSELECTROW;
                    }
                }
                switch (commonActions)
                {
                    case ActionsEnum.RESET:
                        //if (txtCommissionAll.Text.Length > 0)
                        //{
                        //    ddlFormulaAll.SelectedIndex = 0;
                        //}
                        //else if (Convert.ToInt16(ddlFormulaAll.SelectedValue) > 0)
                        //{
                        //    txtCommissionAll.Text = string.Empty;
                        //}
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDivWkf('[id$=divFormulaSettAll]','" + GetLocalResourceObject("FormulaSettings").ToString() + "','550');", true);   
                        break;


                    case ActionsEnum.NEW:
                        ddlCustomer.Enabled = true;
                        GetFieldValues(ControlsEnum.NEW);
                        SetFieldValues(ControlsEnum.NEW);
                        foreach (GridViewRow row in grdCustomers.Rows)
                        {
                            if (((RadioButton)row.FindControl("rbtSelect")).Checked)
                            {
                                ((RadioButton)row.FindControl("rbtSelect")).Checked = false;
                                customerId = 0;
                                break;
                            }
                        }
                        SetFieldValues(ControlsEnum.COMMISIONVIEWBIND);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                        break;
                    case ActionsEnum.EDIT:
                        GetFieldValues(ControlsEnum.NEW);
                        SetFieldValues(ControlsEnum.NEW);
                        bool isSelected = false;
                        foreach (GridViewRow row in grdCustomers.Rows)
                        {
                            if (((RadioButton)row.FindControl("rbtSelect")).Checked)
                            {
                                isSelected = true;
                                HiddenField hfCustomerPk = (HiddenField)row.FindControl("hfCustomerPk");
                                ddlCustomer.SelectedValue = hfCustomerPk.Value;
                                customerId = GetNullableInt(hfCustomerPk.Value).Value;
                                ActionHandler(ddlCustomer, EventArgs.Empty);
                                ddlCustomer.Enabled = false;
                                break;
                            }
                        }
                        if (isSelected)
                        {
                            BindGrid(ControlsEnum.EDIT);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                        }
                        else
                        {
                            string msg = GetLocalResourceObject("SelectCustomer").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
                        }
                        //SetFieldValues(ControlsEnum.EDIT);                        
                        break;
                    case ActionsEnum.REMOVE:
                      
                        bool isCustSelected = false;
                        int custid=0;
                        foreach (GridViewRow row in grdCustomers.Rows)
                        {
                            if (((RadioButton)row.FindControl("rbtSelect")).Checked)
                            {
                                isCustSelected = true;
                                HiddenField hdfCustomerPk = (HiddenField)row.FindControl("hfCustomerPk");
                                custid = Convert.ToInt32(hdfCustomerPk.Value);
                                break;
                            }
                        }
                        if (isCustSelected)
                        {
                            DeleteCustomer(custid);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                        }
                        else
                        {
                            string msg = GetLocalResourceObject("SelectCustomer").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
                        }
                        //SetFieldValues(ControlsEnum.EDIT);                        
                        break;
                    case ActionsEnum.CHANGESTATUS:
                        if (ddlCustomer.SelectedIndex > 0)
                        {
                            customerId = Convert.ToInt32(ddlCustomer.SelectedValue);
                            GetFieldValues(ControlsEnum.MAPPEDCOMMISION);
                            GetFieldValues(ControlsEnum.FROMULA);
                            SetFieldValues(ControlsEnum.MAPPEDCOMMISION);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                        }
                        else
                        {
                            customerId = 0;
                            SetFieldValues(ControlsEnum.MAPPEDCOMMISION);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                        }
                        break;


                    //case ActionsEnum.CHANGEVALUE:

                    //    bool flag = true;
                    //    foreach (GridViewRow row in grdCommision.Rows)
                    //    {
                    //    //int columnIndex = grdCommision.CurrentCell.ColumnIndex
                    //        DropDownList ddl = (DropDownList)row.FindControl("ddlRateType");
                    //        TextBox txtcommission = (TextBox)row.FindControl("txtCommision");
                    //        DropDownList ddl_Formula = (DropDownList)row.FindControl("ddlFormula");
                    //        if (ddl.SelectedItem.Text == "FRL")
                    //        {                               
                    //            grdCommision.Columns[7].Visible = true;
                    //            foreach (GridViewRow row1 in grdCommision.Rows)
                    //            {
                    //                DropDownList ddl1 = (DropDownList)row1.FindControl("ddlRateType");
                    //                TextBox txtcommission1 = (TextBox)row1.FindControl("txtCommision");
                    //                DropDownList ddl_Formula1 = (DropDownList)row1.FindControl("ddlFormula");
                    //                if (ddl1.SelectedItem.Text == "FRL")
                    //                {
                    //                    txtcommission1.Enabled = false;
                    //                    ddl_Formula1.Enabled = true;
                    //                }
                    //                else
                    //                {
                    //                    txtcommission1.Enabled = true;
                    //                    ddl_Formula1.Enabled = false;
                    //                }
                    //            }
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            grdCommision.Columns[7].Visible = false;
                    //            txtcommission.Enabled = true;
                    //            ddl_Formula.Enabled = false;
                    //        }
                    //    }
                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                    //    break;

                    case ActionsEnum.DATABOUND:
                        foreach (GridViewRow row in ((GridView)sender).Rows)
                        {
                            senderId = ((GridView)sender).ID;
                            DropDownList ddl = (DropDownList)row.FindControl("ddlRateType");
                            ddl.DataTextField = "CFG_DATA";
                            ddl.DataValueField = "CFG_VALUE";
                            ddl.DataSource = RateTypes;
                            ddl.DataBind();
                            ddl.Items.Insert(0, new ListItem { Value = "0", Text = GetLocalResourceObject("Select").ToString() });
                            ddl.Width = new Unit("61px");
                            HiddenField hdfRateType = (HiddenField)row.FindControl("hdfRateType");
                            ddl.SelectedValue = hdfRateType.Value == string.Empty ? "0" : hdfRateType.Value;

                            DropDownList ddl_Formula = (DropDownList)row.FindControl("ddlFormula");
                            ddl_Formula.DataTextField = "FRL_NAME";
                            ddl_Formula.DataValueField = "FRL_PK";
                            ddl_Formula.DataSource = FormulaTypes;
                            ddl_Formula.DataBind();
                            ddl_Formula.Items.Insert(0, new ListItem { Value = "0", Text = GetLocalResourceObject("Select").ToString() });
                            ddl_Formula.Width = new Unit("75px");
                            HiddenField hdfFromula = (HiddenField)row.FindControl("hdfFromula");
                            ddl_Formula.SelectedValue = hdfFromula.Value == string.Empty ? "0" : hdfFromula.Value;


                            if (IsAgentCommissionFormula == true)
                            {
                                if (senderId == "grdCommision")
                                {
                                    if (ddl.SelectedValue == "3")
                                    {
                                        TextBox txtcommission = (TextBox)row.FindControl("txtCommision");
                                        //Button btnPopupFormula = (Button)row.FindControl("btnPopupFormula");  
                                        ddl_Formula.Enabled = true;
                                        txtcommission.Enabled = false;
                                        // btnPopupFormula.Enabled = false;

                                    }
                                    //if (ddl.SelectedItem.Text != "FRL")
                                    //{
                                    //    Button btn = (Button)row.FindControl("btnPopupFormula");
                                    //    btn.Enabled = false;
                                    //}
                                }
                            }

                            // Get GridView selected rowindex in Javascript
                            System.Text.StringBuilder click = new System.Text.StringBuilder();
                            click.AppendLine(String.Format("ResetComm('{0}')", row.RowIndex));
                            ddl.Attributes.Add("onchange", click.ToString());
                            //End
                        }
                        break;
                    case ActionsEnum.CANCEL:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);
                        break;
                    case ActionsEnum.SAVE:
                        if (ValidateForSave())
                        {
                            GetFieldValues(ControlsEnum.SAVE);
                            int result = BusinessLogic.Finance.CommSetupBL.SaveCommisionDetails(xmlDoc.InnerXml);
                            if (result > 0)
                            {
                                string msg = GetLocalResourceObject("AgentCommisionSavedSuccessfully").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + msg + "','" + Resources.Messages.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);
                                grdCommisionView.DataSource = null;
                                grdCommisionView.DataBind();
                                GetFieldValues(ControlsEnum.MAPPEDCUSTOMERS);
                                SetFieldValues(ControlsEnum.MAPPEDCUSTOMERS);
                                foreach (GridViewRow row in grdCustomers.Rows)
                                {
                                    if (((HiddenField)row.FindControl("hfCustomerPk")).Value == customerId.ToString())
                                    {
                                        RadioButton rbtSelect = (RadioButton)row.FindControl("rbtSelect");
                                        rbtSelect.Checked = true;
                                        GetFieldValues(ControlsEnum.COMMISIONVIEWBIND);
                                        //GetFieldValues(ControlsEnum.FROMULA);
                                        SetFieldValues(ControlsEnum.COMMISIONVIEWBIND);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                                string msg = GetLocalResourceObject("InformationNotSaved").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }

                        //    GetFieldValues(ControlsEnum.MAPPEDCUSTOMERS);
                        //SetFieldValues(ControlsEnum.MAPPEDCUSTOMERS);
                        break;
                    case ActionsEnum.TASKSELECTROW:
                        RadioButton rbTask = sender as RadioButton;
                        GridViewRow parentRow = rbTask.NamingContainer as GridViewRow;
                        string pk = ((HiddenField)parentRow.FindControl("hfCustomerPk")).Value;
                        customerId = GetNullableInt(pk).Value;
                        GetFieldValues(ControlsEnum.COMMISIONVIEWBIND);
                        GetFieldValues(ControlsEnum.FROMULA);
                        SetFieldValues(ControlsEnum.COMMISIONVIEWBIND);
                        break;

                    case ActionsEnum.CANCEL_ACTION:
                        Response.Redirect(Resources.PageURL.AgentListing);
                        break;

                    case ActionsEnum.SHOWGRIDHEADERPOPUP:
                        Reset(ControlsEnum.FROMULA);
                        BindDropDown(ControlsEnum.FROMULA); 
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDivWkf('[id$=divFormulaSettAll]','" + GetLocalResourceObject("FormulaSettings").ToString() + "','550');", true);   
                        break;

                    case ActionsEnum.CANCELPOPUP:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "window.close()", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                        break;

                    case ActionsEnum.FROMULAPPLYALL:
                        foreach (GridViewRow row in grdCommision.Rows)
                        {
                            if (Convert.ToInt32(ddlFormulaAll.SelectedValue) > 0)
                            {
                                ((DropDownList)row.FindControl("ddlFormula")).SelectedValue = ddlFormulaAll.SelectedValue;
                                ((DropDownList)row.FindControl("ddlRateType")).SelectedValue = ddlRateTypeAll.SelectedValue;
                                ((TextBox)row.FindControl("txtCommision")).Text = string.Empty;
                                ((TextBox)row.FindControl("txtCommision")).Enabled = false;
                            }
                            if (txtCommissionAll.Text.Length > 0)
                            {
                                ((DropDownList)row.FindControl("ddlFormula")).SelectedValue = "0";
                                ((DropDownList)row.FindControl("ddlFormula")).Enabled=false;
                                ((DropDownList)row.FindControl("ddlRateType")).SelectedValue = ddlRateTypeAll.SelectedValue;
                                ((TextBox)row.FindControl("txtCommision")).Text = txtCommissionAll.Text;  
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                        break;

                    case ActionsEnum.CLOSEPOPUP:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion ActionHandler

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                switch (type)
                {
                    case ControlsEnum.NEW:
                        dtCustomers = BusinessLogic.Finance.CommSetupBL.GetAllCustomerList(currentUser.SBUID);
                        break;
                    case ControlsEnum.EDIT:
                        dtCommision = new DataTable();
                        break;
                    case ControlsEnum.AGENTDETAIL:
                        dtAgent = BusinessLogic.Finance.CommSetupBL.GetAgentDetails(GetNullableInt(hfAgentPk.Value), currentUser.SBUID, 2);
                        break;
                    case ControlsEnum.MAPPEDCUSTOMERS:
                        dtCustomers = BusinessLogic.Finance.CommSetupBL.GetMappedCustomerList(Convert.ToInt32(hfAgentPk.Value), currentUser.SBUID);
                        break;
                    //case ControlsEnum.ALLCUSTOMERS:
                    //    dtCustomers = BusinessLogic.Finance.CommSetupBL.GetAllCustomerList(currentUser.SBUID);
                    //    break;
                    case ControlsEnum.MAPPEDCOMMISION:
                        dtRateTypes = BusinessLogic.Finance.CommSetupBL.GetRateType(null, 1);
                        RateTypes = dtRateTypes;
                        dtMappedCommisions = BusinessLogic.Finance.CommSetupBL.GetMappedCommisionList(GetNullableInt(hfAgentPk.Value).Value, customerId);
                        break;
                    case ControlsEnum.SAVE:
                        customerId = Convert.ToInt32(ddlCustomer.SelectedValue);
                        agentInfoObj = (BusinessObject.Finance.CommSetupBO.AgentInfo)SetUIValuesToObject(ActionsEnum.SAVE);
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(agentInfoObj);
                        break;
                    case ControlsEnum.COMMISIONVIEWBIND:
                        dtRateTypes = BusinessLogic.Finance.CommSetupBL.GetRateType(null, 1);
                        RateTypes = dtRateTypes;
                        dtMappedCommisions = BusinessLogic.Finance.CommSetupBL.GetMappedCommisionList(GetNullableInt(hfAgentPk.Value).Value, customerId);
                        break;

                    case ControlsEnum.FROMULA:
                        //dtFormula = new DataTable();
                        dtFormula = BusinessLogic.CommonManagement.CommonBL.GetFormulaList(currentUser.SBUID);
                        FormulaTypes = dtFormula;
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        #endregion Get Field Values

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.AGENTDETAIL:
                        lblAgentName.Text = dtAgent.Rows[0]["VEN_NAME"].ToString();
                        break;
                    case ControlsEnum.NEW:
                        BindDropDown(ControlsEnum.ALLCUSTOMERS);
                        BindGrid(ControlsEnum.NEW);
                        break;
                    case ControlsEnum.EDIT:
                        bool isSelected = false;
                        foreach (GridViewRow row in grdCustomers.Rows)
                        {
                            if (((RadioButton)row.FindControl("rbtSelect")).Checked)
                            {
                                isSelected = true;
                                HiddenField hfCustomerPk = (HiddenField)row.FindControl("hfCustomerPk");
                                ddlCustomer.SelectedValue = hfCustomerPk.Value;
                                ActionHandler(ddlCustomer, EventArgs.Empty);
                                break;
                            }
                        }
                        if (isSelected)
                        {
                            BindGrid(ControlsEnum.EDIT);
                        }
                        else
                        {
                            string msg = GetLocalResourceObject("SelectCustomer").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    case ControlsEnum.MAPPEDCUSTOMERS:
                        BindGrid(ControlsEnum.MAPPEDCUSTOMERS);
                        break;
                    case ControlsEnum.ALLCUSTOMERS:
                        BindDropDown(ControlsEnum.ALLCUSTOMERS);
                        break;
                    case ControlsEnum.MAPPEDCOMMISION:
                        BindGrid(ControlsEnum.MAPPEDCOMMISION);
                        break;
                    case ControlsEnum.COMMISIONVIEWBIND:
                        BindGrid(ControlsEnum.COMMISIONVIEWBIND);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Set Field Values

        #region BindGrid
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
            try
            {
                int rowCount = 0;
                switch (controlType)
                {
                    case ControlsEnum.NEW:
                        if (ddlCustomer.SelectedValue == 0.ToString())
                        {
                            grdCommision.DataSource = null;
                            grdCommision.DataBind();
                        }
                        break;
                    case ControlsEnum.MAPPEDCUSTOMERS:
                        grdCustomers.DataSource = dtCustomers;
                        grdCustomers.DataBind();
                        break;
                    case ControlsEnum.MAPPEDCOMMISION:
                        if (customerId == 0)
                        {
                            grdCommision.DataSource = null;
                            grdCommision.DataBind();
                        }
                        else
                        {
                            grdCommision.DataSource = dtMappedCommisions;
                            grdCommision.DataBind();
                        }
                        break;
                    case ControlsEnum.COMMISIONVIEWBIND:
                        if (customerId == 0)
                        {
                            grdCommisionView.DataSource = null;
                            grdCommisionView.DataBind();
                        }
                        else
                        {
                            grdCommisionView.DataSource = dtMappedCommisions;
                            grdCommisionView.DataBind();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion BindGrid
        /// <summary>
        /// Delete Customer
        /// </summary>
        private void DeleteCustomer(int id)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            string msg = string.Empty;
            //int retVal = BinRegisterDataBL.DeleteBin(CurrPK, 1, currentUser.CurrentSBUPK, currentUser.PKUser, ref RetMsg);
            int retVal = BusinessLogic.Finance.CommSetupBL.DeleteCustomer(id,Convert.ToInt32 (hfAgentPk.Value), currentUser.SBUID);
            if (retVal >0)
            {
                msg = GetLocalResourceObject("CustomerDelete").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + msg + "','" + Resources.Messages.Information + "');", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);
                GetFieldValues(ControlsEnum.MAPPEDCUSTOMERS);
                SetFieldValues(ControlsEnum.MAPPEDCUSTOMERS);
                grdCommisionView.DataSource = null;
                grdCommisionView.DataBind();
            }
            else
            {
                if (retVal == (int)DbDeleteStatus.SQLERROR)
                {
                    msg = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (retVal == (int)DbDeleteStatus.CONCURRENCY)
                {
                    msg = GetLocalResourceObject("Customer").ToString()+" "+GetLocalResourceObject("AlreadyUsed");
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (retVal == (int)DbDeleteStatus.ALREADYEXIST)
                {
                    msg =Resources.Messages.CustomerNotExsist;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                            }
        }
        #region BindGrid
        /// <summary>
        /// Method for Dropdown Binding
        /// </summary>
        public void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.ALLCUSTOMERS:
                        ddlCustomer.DataValueField = "CUS_PK";
                        ddlCustomer.DataTextField = "CUS_TEXT";
                        ddlCustomer.DataSource = dtCustomers;
                        ddlCustomer.DataBind();
                        ddlCustomer.Items.Insert(0, new ListItem { Value = "0", Text = GetLocalResourceObject("Select").ToString() });
                        foreach (ListItem item in ddlCustomer.Items)
                        {
                            item.Text = HttpUtility.HtmlDecode(item.Text);
                        }
                        break;

                    case ControlsEnum.FROMULA:
                        ddlFormulaAll.DataTextField = "FRL_NAME";
                        ddlFormulaAll.DataValueField = "FRL_PK";
                        ddlFormulaAll.DataSource = FormulaTypes;
                        ddlFormulaAll.DataBind();
                        ddlFormulaAll.Items.Insert(0, new ListItem { Value = "0", Text = GetLocalResourceObject("Select").ToString() });

                        ddlRateTypeAll.DataTextField = "CFG_DATA";
                        ddlRateTypeAll.DataValueField = "CFG_VALUE";
                        ddlRateTypeAll.DataSource = RateTypes;
                        ddlRateTypeAll.DataBind();
                        ddlRateTypeAll.Items.Insert(0, new ListItem { Value = "0", Text = GetLocalResourceObject("Select").ToString() });
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion BindGrid


        private void Reset(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.FROMULA:
                        txtCommissionAll.Text = string.Empty;
                       // ddlFormulaAll.SelectedIndex = 0;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #region "Helper Methods"
        private int? GetNullableInt(string str)
        {
            int result;
            if (int.TryParse(str, out result))
            {
                return (int?)result;
            }
            return null;
        }

        private decimal? GetNullableDecimal(string str)
        {
            decimal result;
            if (decimal.TryParse(str, out result))
            {
                return (decimal?)result;
            }
            return null;
        }

        //private string GenerateXmlForSave()
        //{
        //    //<Root>
        //    //                         <ACH_CUSTOMER></ACH_CUSTOMER>
        //    //                         <ACH_AGENT></ACH_AGENT>
        //    //                         <BIZUNIT_PK>1</BIZUNIT_PK>
        //    //                         <ACTIVE>1</ACTIVE>
        //    //                         <USER_PK>1</USER_PK>
        //    //                         <LAST_MOD_DT>2013-01-01T10:10:10.010</LAST_MOD_DT>
        //    //                         <Detail>
        //    //                             <ACD_PK></ACD_PK>
        //    //                             <ACD_CUST_ITEM></ACD_CUST_ITEM>
        //    //                             <ACD_COMMISION></ACD_COMMISION>
        //    //                             <ACD_COMMISION_TYPE></ACD_COMMISION_TYPE>
        //    //                             <ACD_CURRENCY></ACD_CURRENCY>
        //    //                             <ACD_ACTIVE></ACD_ACTIVE>
        //    //                             <ACD_BIZUNIT></ACD_BIZUNIT>
        //    //                             <ACD_CRTD_BY></ACD_CRTD_BY>
        //    //                             <ACD_CRTD_DT></ACD_CRTD_DT>
        //    //                             <ACD_MOD_BY></ACD_MOD_BY>
        //    //                             <ACD_MOD_DT></ACD_MOD_DT>
        //    //                         </Detail>
        //    //                         </Root>


        //    agentInfoObj = (AgentInfo)SetUIValuesToObject(commonActions);
        //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(agentInfoObj);

        //    string result = string.Empty;


        //    return result;
        //}

        private List<BusinessObject.Finance.CommSetupBO.Detail> GetCommisionList()
        {
            List<BusinessObject.Finance.CommSetupBO.Detail> details = new List<BusinessObject.Finance.CommSetupBO.Detail>();
            foreach (GridViewRow row in grdCommision.Rows)
            {
                BusinessObject.Finance.CommSetupBO.Detail detailObj = new BusinessObject.Finance.CommSetupBO.Detail();
                TextBox txtCommision = (TextBox)row.FindControl("txtCommision");
                DropDownList ddlFormula = (DropDownList)row.FindControl("ddlFormula");
                //  HiddenField hdfCur = (HiddenField)row.FindControl("hdfCurrency");
                if (GetNullableDecimal(txtCommision.Text.Trim()).HasValue && GetNullableDecimal(txtCommision.Text.Trim()).Value > 0)
                {

                    //BusinessObject.Finance.CommSetupBO.Detail detailObj = new BusinessObject.Finance.CommSetupBO.Detail();
                    detailObj.acdPk = GetNullableInt(((HiddenField)row.FindControl("hdfAcd_Pk")).Value).Value;
                    detailObj.acdCustItem = GetNullableInt(((HiddenField)row.FindControl("hdfCust_Item")).Value).Value;
                    detailObj.acdCommision = GetNullableDecimal(txtCommision.Text.Trim()).Value;
                    detailObj.acdCommisionType = GetNullableInt(((DropDownList)row.FindControl("ddlRateType")).SelectedValue).Value; //  GetNullableInt(((HiddenField)row.FindControl("hdfRateType")).Value).Value;
                    detailObj.acdCurrency = GetNullableInt(((HiddenField)row.FindControl("hdfCurrency")).Value).Value;
                    detailObj.acdformula = GetNullableInt(((DropDownList)row.FindControl("ddlFormula")).SelectedValue).Value;
                    detailObj.acdActive = 1;
                    detailObj.acdBizUnit = currentUser.SBUID;
                    details.Add(detailObj);
                    //<ACD_CRTD_BY></ACD_CRTD_BY>
                    //                    <ACD_CRTD_DT></ACD_CRTD_DT>
                    //                    <ACD_MOD_BY></ACD_MOD_BY>
                    //                    <ACD_MOD_DT></ACD_MOD_DT>                    
                }
                else if (Convert.ToInt32(ddlFormula.SelectedValue) > 0)
                {
                    //BusinessObject.Finance.CommSetupBO.Detail detailObj = new BusinessObject.Finance.CommSetupBO.Detail();
                    detailObj.acdPk = GetNullableInt(((HiddenField)row.FindControl("hdfAcd_Pk")).Value).Value;
                    detailObj.acdCustItem = GetNullableInt(((HiddenField)row.FindControl("hdfCust_Item")).Value).Value;
                    detailObj.acdCommision = GetNullableDecimal(txtCommision.Text.Trim() != string.Empty ? txtCommision.Text.Trim() : "0").Value;
                    detailObj.acdCommisionType = GetNullableInt(((DropDownList)row.FindControl("ddlRateType")).SelectedValue).Value; //  GetNullableInt(((HiddenField)row.FindControl("hdfRateType")).Value).Value;
                    detailObj.acdCurrency = GetNullableInt(((HiddenField)row.FindControl("hdfCurrency")).Value).Value;
                    detailObj.acdformula = GetNullableInt(((DropDownList)row.FindControl("ddlFormula")).SelectedValue).Value;
                    detailObj.acdActive = 1;
                    detailObj.acdBizUnit = currentUser.SBUID;
                    details.Add(detailObj);
                }
                
            }
            return details;
        }

        private bool ValidateForSave()
        {
            bool result = true;
            string message = string.Empty;
            if (ddlCustomer.SelectedIndex < 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                message = GetLocalResourceObject("SelectCustomer").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message) + "','" + Resources.Messages.Information + "');", true);
                result = false;
                return result;
            }
            if (grdCommision.Rows.Count < 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                message = GetLocalResourceObject("NoRecordsFound").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message) + "','" + Resources.Messages.Information + "');", true);
                result = false;
                return result;
            }
            foreach (GridViewRow row in grdCommision.Rows)
            {
                TextBox txtCommision = (TextBox)row.FindControl("txtCommision");
                DropDownList ddlType = (DropDownList)row.FindControl("ddlRateType");
                DropDownList ddlFormula = (DropDownList)row.FindControl("ddlFormula");
                //txtCommision.Attributes.Remove("class");
                if (txtCommision.Text.Trim() != string.Empty && GetNullableDecimal(txtCommision.Text).HasValue == false)
                {
                    //txtCommision.Attributes.Add("class", "error");
                    message = GetLocalResourceObject("EnterValidCommision").ToString();
                    result = false;
                    break;
                }
                else if (GetNullableDecimal(txtCommision.Text).HasValue == true && ddlType.SelectedItem.Text == "%")
                {
                    if (GetNullableDecimal(txtCommision.Text).Value >= 100)
                    {
                        message = GetLocalResourceObject("CommisionPercentageShouldBeLess").ToString();
                        result = false;
                        break;
                    }
                }
                else if (GetNullableDecimal(txtCommision.Text).HasValue == true && ddlType.SelectedValue == "0")
                {
                    //txtCommision.Attributes.Add("class", "error");
                    message = GetLocalResourceObject("SelectValidRateType").ToString();
                    result = false;
                    break;
                }

                #region NewCode

                if (Convert.ToInt32(ddlType.SelectedValue) > 0 && txtCommision.Text.Trim() == string.Empty)
                {
                    if (Convert.ToInt32(ddlType.SelectedValue) != 3) // 3 = FRL
                    {
                        message = GetLocalResourceObject("EnterValidCommision").ToString();
                        result = false;
                        break;
                    }
                }

                if (IsAgentCommissionFormula == true)
                {
                    if (Convert.ToInt32(ddlType.SelectedValue) == 3 && Convert.ToInt32(ddlFormula.SelectedValue) <= 0) //FORMULA
                    {
                        message = GetLocalResourceObject("SelectCommisionFormula").ToString();
                        result = false;
                        break;
                    }
                }

                #endregion

                //else if (Convert.ToInt32(ddlType.SelectedValue) > 0 && txtCommision.Text.Trim() == string.Empty)
                //{
                //    //txtCommision.Attributes.Add("class", "error");
                //    if (IsAgentCommissionFormula == false)
                //    {
                //        message = GetLocalResourceObject("EnterValidCommision").ToString();
                //        result = false;
                //        break;
                //    }
                //}
                //else if (Convert.ToInt32(ddlType.SelectedValue) > 0 && Convert.ToInt32(ddlFormula.SelectedValue)<=0)
                //{
                //    if (IsAgentCommissionFormula == true)
                //    {
                //        message = GetLocalResourceObject("SelectCommisionFormula").ToString();
                //        result = false;
                //        break;
                //    }
                //}

            }
            if (grdCommision.Rows.Count > 0)
            {
                decimal CommAmt = 0;
                foreach (GridViewRow row in grdCommision.Rows)
                {
                    TextBox txtCommision = (TextBox)row.FindControl("txtCommision");
                    DropDownList ddlType = (DropDownList)row.FindControl("ddlRateType");
                    if (txtCommision.Text.Trim() != string.Empty && GetNullableDecimal(txtCommision.Text).HasValue == true)
                    {
                        CommAmt = CommAmt + Convert.ToDecimal(txtCommision.Text);
                    }
                }
                if (IsAgentCommissionFormula == false)
                {
                    if (CommAmt == 0)
                    {
                        message = GetLocalResourceObject("EnterValidCommision").ToString();
                        result = false;
                    }
                }
            }
            if (result == false)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpDetails]','" + GetLocalResourceObject("Commision").ToString() + "','950','550');", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message) + "','" + Resources.Messages.Information + "');", true);
            }
            return result;
        }

        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            if (num > 0)
            {
                string str = num.ToString(hdfRateFormat.Value);
                return str;
            }
            return string.Empty;

        }
        #endregion "Helper Methods"

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            try
            {
                switch (mode)
                {
                    #region Save
                    case ActionsEnum.SAVE:
                        agentInfoObj = new BusinessObject.Finance.CommSetupBO.AgentInfo();
                        agentInfoObj.Customer = customerId;
                        agentInfoObj.Agent = Convert.ToInt32(hfAgentPk.Value);
                        agentInfoObj.BizUnit = currentUser.SBUID;
                        agentInfoObj.Active = 1;
                        agentInfoObj.UserPk = currentUser.PKUser;
                        //<LAST_MOD_DT>2013-01-01T10:10:10.010</LAST_MOD_DT>
                        agentInfoObj.CommisionList = GetCommisionList();
                        returnObj = agentInfoObj;
                        break;
                    #endregion
                }
                return returnObj;
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }

        private void ConfigurationSettings()
        {
            IsAgentCommissionFormula = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "AgentCommissionFormula")));
        }


        public enum ControlsEnum
        {
            NEW,
            EDIT,
            AGENTDETAIL,
            MAPPEDCUSTOMERS,
            ALLCUSTOMERS,
            MAPPEDCOMMISION,
            SAVE,
            COMMISIONVIEWBIND,
            FROMULA
        }

    }
}