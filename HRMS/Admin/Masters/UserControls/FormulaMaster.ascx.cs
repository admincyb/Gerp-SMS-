using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using System.Data;
using System.Text;
using BusinessLogic.CommonManagement;
using BusinessObject.HRMS.Admin.Masters;
using ERP.Utilities.Constants.DA;
using System.Threading;

namespace HRMS.Admin.Masters.UserControls
{
    public partial class FormulaMaster : System.Web.UI.UserControl
    {
        #region Event
        public event EventHandler AfterApply;       
        #endregion

        #region Variables and Properties
        #region Properties
        public string FormulaText
        {
            get
            {
                return ViewState["FormulaText"] == null ? string.Empty : Convert.ToString(ViewState["FormulaText"]);
            }
            set
            {
                ViewState["FormulaText"] = value;
            }
        }
        public string FormulaValue
        {
            get
            {
                return ViewState["FormulaValue"] == null ? string.Empty : Convert.ToString(ViewState["FormulaValue"]);
            }
            set
            {
                ViewState["FormulaValue"] = value;
            }
        }
        public string PayelementText
        {
            get
            {
                return ViewState["PayelementText"] == null ? string.Empty : Convert.ToString(ViewState["PayelementText"]);
            }
            set
            {
                ViewState["PayelementText"] = value;
            }
        }

        private List<SalaryTemplateBO.PayElement> PayElementsViewState
        {
            get
            {
                return ViewState["PayElements"] == null ? new List<SalaryTemplateBO.PayElement>() : (List<SalaryTemplateBO.PayElement>)ViewState["PayElements"];
            }
            set
            {
                ViewState["PayElements"] = value;
            }
        }
        public string wordStart
        {
            get
            {
                return ViewState["wordStart"] == null ? "{" : Convert.ToString(ViewState["wordStart"]);
            }
            set
            {
                ViewState["wordStart"] = value;
            }
        }
        public string wordEnd
        {
            get
            {
                return ViewState["wordEnd"] == null ? "}" : Convert.ToString(ViewState["wordEnd"]);
            }
            set
            {
                ViewState["wordEnd"] = value;
            }
        }

        public int IsDeduction
        {
            get
            {
                return ViewState["IsDeduction"] == null ? 0 : Convert.ToInt32(ViewState["IsDeduction"]);
            }
            set
            {
                ViewState["IsDeduction"] = value;
            }
        }
        public int IsSlab
        {
            get
            {
                return ViewState["IsSlab"] == null ? 0 : Convert.ToInt32(ViewState["IsSlab"]);
            }
            set
            {
                ViewState["IsSlab"] = value;
            }
        }
        public int ShowMinMaxAmount
        {
            get
            {
                return ViewState["ShowMinMaxAmount"] == null ? 0 : Convert.ToInt32(ViewState["ShowMinMaxAmount"]);
            }
            set
            {
                ViewState["ShowMinMaxAmount"] = value;
            }
        }

        public decimal MinAmount
        {
            get
            {
                return ViewState["FormulaMinAmount"] == null ? 0 : Convert.ToInt32(ViewState["FormulaMinAmount"]);
            }
            set
            {
                ViewState["FormulaMinAmount"] = value;
            }
        }
        public decimal MaxAmount
        {
            get
            {
                return ViewState["FormulaMaxAmount"] == null ? 0 : Convert.ToInt32(ViewState["FormulaMaxAmount"]);
            }
            set
            {
                ViewState["FormulaMaxAmount"] = value;
            }
        }
        /// <summary>
        /// Popup div ID
        /// </summary>
        public string PopupDivId
        {
            get
            {
                return ViewState["PopupDivId"] == null ? "divPopUpFormula" : Convert.ToString(ViewState["PopupDivId"]);
            }
            set
            {
                ViewState["PopupDivId"] = value;
            }
        }

        public string HeaderText
        {
            get
            {
                return ViewState["HeaderText"] == null ? Resources.Controls.PayElement : Convert.ToString(ViewState["HeaderText"]);
            }
            set
            {
                ViewState["HeaderText"] = value;
            }
        }
        #endregion

        private ActionsEnum commonActions;
        private DataTable dtResult;
        private DataTable dtParameters;
        private BusinessObject.User currentUser;
        #endregion

        #region PageEvents
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "FormulaInitComponents", "$(document).ready(function(){FormulaInitComponents();});", true);
        }
        #endregion
        #region Helper Methods
        private void SetConfigData()
        {
            DataTable dtConfig = CommonBL.GetApplicaitonConfiguaration("HRMS FORMULA SEPARATOR", string.Empty, currentUser.SBUID);
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                wordStart = hdfBeginWord.Value = ((string[])dtConfig.Rows[0]["ACF_DATA"].ToString().Split(','))[0];
                wordEnd = hdfEndWord.Value = ((string[])dtConfig.Rows[0]["ACF_DATA"].ToString().Split(','))[1];
            }
        }
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (!IsPostBack)
                {
                    hdfFormulaDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;                   
                    hdfFormulaCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfFormulaCurrencyFormat.Value += "0";                        
                    }
                    if (ShowMinMaxAmount > 0)
                        divMinMaxAmnt.Visible = true;
                    else
                        divMinMaxAmnt.Visible = false;
                    SetConfigData();
                    GetFieldValues(ControlsEnum.PAYELEMENT);
                    SetFieldValues(ControlsEnum.PAYELEMENT);
                    GetFieldValues(ControlsEnum.PARAMETERS);
                    SetFieldValues(ControlsEnum.PARAMETERS);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Action Handler
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                decimal minAmnt = 0;
                decimal maxAmnt = 0;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlPayElement")
                    {
                        commonActions = ActionsEnum.CHANGEPAYELEMENT;
                    }
                    if (((DropDownList)sender).ID == "ddlParameters")
                    {
                        commonActions = ActionsEnum.CHANGEPARAMETER;
                    }
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    #region APPLY
                    case ActionsEnum.APPLY:                 
                            string formulaCode=string.Empty;
                            string formulaName=string.Empty;
                            formulaName= this.FormulaText = txtFormula.Text.Trim().HtmlEncode();
                            int result = BusinessLogic.HRMS.Common.HRMSCommonBL.ValidateFormula(string.Empty,this.FormulaText, out formulaCode, out formulaName);
                            if (result<0)
                            {
                                ShowFormulaPupup();
                               // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','200');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +GetGlobalResourceObject("ErrorMessages","Msg_InvalidFormula").ToString() + "','" + Resources.Messages.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(1);", true);
                            }
                            else if (AfterApply != null)
                            {
                                this.FormulaText = txtFormula.Text.Trim().HtmlEncode();
                                //StringBuilder temp = new StringBuilder(this.FormulaText);
                                //foreach (var item in PayElementsViewState)
                                //{
                                //    temp = temp.Replace(item.PelName, item.PelFormulaCode);
                                //}
                                this.FormulaValue = formulaCode;
                                decimal.TryParse(txtFormulaMinAmount.Text, out minAmnt);
                                decimal.TryParse(txtFormulaMaxAmount.Text, out maxAmnt);
                                this.MinAmount = minAmnt;
                                this.MaxAmount = maxAmnt;
                                AfterApply(this, EventArgs.Empty);
                            }
                        ResetForm();
                        break;
                    #endregion
                    #region Change Events
                    case ActionsEnum.CHANGEPAYELEMENT:
                        if (!string.IsNullOrEmpty(ddlPayElement.SelectedItem.Value) && Convert.ToInt32(ddlPayElement.SelectedIndex) > 0)
                        {
                            if (!string.IsNullOrEmpty(ddlPayElement.SelectedItem.Value) && Convert.ToInt32(ddlPayElement.SelectedIndex) > 0)
                            {
                                //this.FormulaText = string.IsNullOrEmpty(txtFormula.Text) ? txtFormula.Text.Trim() + ddlPayElement.SelectedItem.Text : txtFormula.Text.Trim() + " " + ddlPayElement.SelectedItem.Text;
                                this.FormulaText = string.IsNullOrEmpty(txtFormula.Text) ? (txtFormula.Text.Trim() + wordStart + ddlPayElement.SelectedItem.Text + wordEnd) : (txtFormula.Text.Trim() + wordStart + ddlPayElement.SelectedItem.Text + wordEnd);
                                txtFormula.Text = this.FormulaText;
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','200');", true);
                                ShowFormulaPupup();
                            }
                        }
                        else
                        {
                            ShowFormulaPupup();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','200');", true);
                            //commented for bug 14902
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetGlobalResourceObject("ErrorMessages", "Msg_FormulaCodeNotFound").ToString() + "','" + Resources.Messages.Information + "');", true);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(1);", true);
                        break;
                    case ActionsEnum.CHANGEPARAMETER:
                        if (!string.IsNullOrEmpty(ddlParameters.SelectedItem.Value) && Convert.ToInt32(ddlParameters.SelectedIndex) > 0)
                        {
                            //this.FormulaText = string.IsNullOrEmpty(txtFormula.Text) ? txtFormula.Text.Trim() + ddlParameters.SelectedItem.Text : txtFormula.Text.Trim() + " " + ddlParameters.SelectedItem.Text;
                            this.FormulaText = string.IsNullOrEmpty(txtFormula.Text) ? (txtFormula.Text.Trim() + wordStart + ddlParameters.SelectedItem.Text + wordEnd) : (txtFormula.Text.Trim() + wordStart + ddlParameters.SelectedItem.Text + wordEnd);
                            txtFormula.Text = this.FormulaText;
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','200');", true);
                            ShowFormulaPupup();
                        }
                        else
                        {
                            ShowFormulaPupup();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','200');", true);
                            //commented for bug 14902
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetGlobalResourceObject("ErrorMessages", "Msg_FormulaCodeNotFound").ToString() + "','" + Resources.Messages.Information + "');", true);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(1);", true);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        private void ShowFormulaPupup()
        {
            if (string.IsNullOrEmpty(PopupDivId))
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','230');", true);
            else
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=" + PopupDivId + "]','Formula','660','230');", true);
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        public void GetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    #region PAYELEMENT
                    case ControlsEnum.PAYELEMENT:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetFormulaElement(0, Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE), currentUser.SBUID, IsDeduction);
                        //dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(0, Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE), currentUser.SBUID, 0, 0);
                        break;
                    #endregion
                    #region Parameter
                    case ControlsEnum.PARAMETERS:
                        dtParameters = CommonBL.GetAppParameters(0, (int)ApplicationModule.HRMS, (int)ActiveStatus.ACTIVE, currentUser.SBUID, IsSlab);
                        break;
                    #endregion
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
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        public void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region PAYELEMENT
                    case ControlsEnum.PAYELEMENT:
                        BindDropDown(ControlsEnum.PAYELEMENT);
                        List<SalaryTemplateBO.PayElement> tempPayElementList = new List<SalaryTemplateBO.PayElement>();
                        foreach (DataRow row in dtResult.Rows)
                        {
                            SalaryTemplateBO.PayElement element = new SalaryTemplateBO.PayElement();
                            if (row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK] != null)
                                element.PelPk = Convert.ToInt32(row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK]);
                            if (row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_CODE] != null)
                                element.PelCode = Convert.ToString(row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_CODE]).HtmlDecode();
                            if (row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME] != null)
                                element.PelName = Convert.ToString(row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME]).HtmlDecode();
                            if (row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_FORMULA_CODE] != null)
                                element.PelFormulaCode = Convert.ToString(row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_FORMULA_CODE]).HtmlDecode();
                            tempPayElementList.Add(element);
                        }
                        PayElementsViewState = tempPayElementList;
                        break;
                    #endregion
                    case ControlsEnum.PARAMETERS:
                        BindDropDown(ControlsEnum.PARAMETERS);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region BindDropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region PAYELEMENT
                case ControlsEnum.PAYELEMENT:
                    ddlPayElement.Items.Clear();
                    ddlPayElement.DataSource = dtResult;
                    ddlPayElement.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME;
                    ddlPayElement.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_FORMULA_CODE;
                    ddlPayElement.DataBind();
                    ddlPayElement.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO));
                    ddlPayElement.Items.HtmlDecode();
                    break;
                #endregion
                case ControlsEnum.PARAMETERS:
                    ddlParameters.Items.Clear();
                    ddlParameters.DataSource = dtParameters;
                    ddlParameters.DataTextField = GTIService.Constants.Common.Fields.APC_NAME;
                    ddlParameters.DataValueField = GTIService.Constants.Common.Fields.APC_CODE;
                    ddlParameters.DataBind();
                    ddlParameters.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO));
                    ddlPayElement.Items.HtmlDecode();
                    break;
            }
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Reset Form data
        /// </summary>
        public void ResetForm()
        {
            ddlPayElement.SelectedIndex = 0;
            ddlParameters.SelectedIndex = 0;
            txtFormula.Text = string.Empty;
            txtFormulaMinAmount.Text = GetFormattedCurrency(0);
            txtFormulaMaxAmount.Text = GetFormattedCurrency(0);
        }
        #endregion

        #region SetData
        public void SetData(string foumulaText, string payelement = "", decimal minAmount = 0, decimal maxAmount = 0)
        {
            this.FormulaText = foumulaText;
            txtFormula.Text = foumulaText;
            this.PayelementText = payelement;
            lblPayElement.Text = payelement;
            lblPayElementheaderTxt.Text = HeaderText;
            this.MinAmount = minAmount;
            this.MaxAmount = maxAmount;
            txtFormulaMinAmount.Text = GetFormattedCurrency(minAmount);
            txtFormulaMaxAmount.Text = GetFormattedCurrency(maxAmount);
        }

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfFormulaCurrencyFormat.Value);
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            PAYELEMENT,
            PARAMETERS
        }
        #endregion
    }

}