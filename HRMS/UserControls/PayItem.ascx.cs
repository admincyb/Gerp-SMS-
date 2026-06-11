using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using System.Data;
using BusinessObject.HRMS.Admin.Masters;
using BusinessObject.CommonManagement;
using System.Threading;

namespace HRMS.UserControls
{
    public partial class PayItem : System.Web.UI.UserControl
    {
        #region Event
        public event EventHandler AfterApply;
        #endregion

        #region Variables and Properties

        #region Properties

        private Dictionary<string, int> PayElementConfigurationViewState
        {
            get
            {
                return this.ViewState["PayElementConfigurationViewState"] == null ? new Dictionary<string, int>() : (Dictionary<string, int>)(this.ViewState["PayElementConfigurationViewState"]);
            }
            set
            {
                this.ViewState["PayElementConfigurationViewState"] = value;
            }
        }

        public int PayClassification
        {
            get
            {
                return ViewState["PayClassification"] == null ? -1 : GetNullableInt(Convert.ToString(ViewState["PayClassification"])).Value;
            }
            set
            {
                ViewState["PayClassification"] = value;
            }
        }
        public int PayElement
        {
            get
            {
                return ViewState["PayElement"] == null ? -1 : GetNullableInt(Convert.ToString(ViewState["PayElement"])).Value;
            }
            set
            {
                ViewState["PayElement"] = value;
            }
        }

        public int ElementTypeCalcMode
        {
            get
            {
                return ViewState["ElementTypeCalcMode"] == null ? -1 : GetNullableInt(Convert.ToString(ViewState["ElementTypeCalcMode"])).Value;
            }
            set
            {
                ViewState["ElementTypeCalcMode"] = value;
            }
        }

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
        public string PayElementName { get; set; }
        public bool PayElementDeduction { get; set; }
        public string dummyPayElement_PK { get; set; }
        public string dummySlab_PK { get; set; }

        public decimal MinimunAmount
        {
            get
            {
                return ViewState["MinimunAmount"] == null ? 0 : Convert.ToInt32(ViewState["MinimunAmount"]);
            }
            set
            {
                ViewState["MinimunAmount"] = value;
            }
        }
        public decimal MaximumAmount
        {
            get
            {
                return ViewState["MaximumAmount"] == null ? 0 : Convert.ToInt32(ViewState["MaximumAmount"]);
            }
            set
            {
                ViewState["MaximumAmount"] = value;
            }
        }       

        private List<SalaryTemplateBO.PayElement> PayElementsViewState
        {
            get
            {
                return ViewState["PayElementsViewState"] == null ? new List<SalaryTemplateBO.PayElement>() : (List<SalaryTemplateBO.PayElement>)ViewState["PayElementsViewState"];
            }
            set
            {
                ViewState["PayElementsViewState"] = value;
            }
        }


        #endregion
        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private decimal minAmnt = 0;
        private decimal maxAmnt = 0;


        #endregion

        #region PageEvents
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
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
                ucFormulaMaster.AfterApply += new EventHandler(ucFormulaMaster_AfterApply1);                
                if (!IsPostBack)
                {
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }
                    GetFieldValues(ControlsEnum.PAYCLASIFICATION);
                    SetFieldValues(ControlsEnum.PAYCLASIFICATION);
                    GetFieldValues(ControlsEnum.PAYTYPE);
                    SetFieldValues(ControlsEnum.PAYTYPE);
                   // GetFieldValues(ControlsEnum.SLAB);
                   // SetFieldValues(ControlsEnum.SLAB);
                    GetFieldValues(ControlsEnum.CUSTOM);
                    SetFieldValues(ControlsEnum.CUSTOM);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region ucFormulaMaster_AfterApply
        // Formula Popup After Apply
        void ucFormulaMaster_AfterApply1(object sender, EventArgs e)
        {
            this.FormulaText = txtAmountOrFormula_PayItem.Text = ucFormulaMaster.FormulaText;
            this.FormulaValue = hdfFormula.Value = ucFormulaMaster.FormulaValue;           
            this.MinimunAmount = ucFormulaMaster.MinAmount;
            this.MaximumAmount = ucFormulaMaster.MaxAmount;
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(1);", true);
        }
        #endregion

        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                
                #region Getting Command Action
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if ((((DropDownList)sender).ID == "ddlPayClassification_PayItem"))
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }
                    if ((((DropDownList)sender).ID == "ddlPayElement_PayItem"))
                    {
                        commonActions = ActionsEnum.ELEMENTTYPE;
                    }
                    if ((((DropDownList)sender).ID == "ddlType"))
                    {
                        commonActions = ActionsEnum.CHANGETYPE;
                    }
                }

                #endregion
                switch (commonActions)
                {
                    #region SHOWPOPUP
                    case ActionsEnum.SHOWPOPUP:
                        string popupElementText = Convert.ToInt32(ddlPayElement_PayItem.SelectedValue) > 0 ? ddlPayElement_PayItem.SelectedItem.ToString() : string.Empty;
                        if (Convert.ToInt32(ddlPayElement_PayItem.SelectedValue) > 0)
                        {
                            this.PayElement = GetNullableInt(ddlPayElement_PayItem.SelectedValue).Value;
                            SalaryTemplateBO.PayElement payEelement = PayElementsViewState
                                .Where(x => x.PelPk == PayElement)
                                .Single();                            
                            ucFormulaMaster.IsDeduction = payEelement.PelDeduction ? 1 : 0;
                        }
                        else
                        {                           
                            ucFormulaMaster.IsDeduction = 0;
                        }
                        
                        ucFormulaMaster.GetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);
                        ucFormulaMaster.SetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);
                        ucFormulaMaster.SetData(txtAmountOrFormula_PayItem.Text, popupElementText, this.MinimunAmount, this.MaximumAmount);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','230');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(1);", true);
                        break;
                    #endregion
                    #region CHANGE
                    case ActionsEnum.CHANGE:
                        txtAmountOrFormula_PayItem.Text = string.Empty;
                        ddlPayElement_PayItem.ClearSelection();
                        ddlType.ClearSelection();
                        if (ddlPayClassification_PayItem.SelectedIndex > 0)
                        {
                            GetFieldValues(ControlsEnum.PAYELEMENTS);
                            SetFieldValues(ControlsEnum.PAYELEMENTS);
                        }
                        else
                        {
                            BindDropDown(ControlsEnum.PAYELEMENTS);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(1);", true);                     
                        break;
                    #endregion
                    #region ADDTOLIST
                    case ActionsEnum.ADDTOLIST:
                        if (AfterApply != null)
                        {                           
                            if (Convert.ToInt32(ddlType.SelectedValue) > Convert.ToInt32(CommonConstants.SELECTVAL))
                            {
                                if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.FixedAmount)
                                {
                                    this.FormulaValue = txtPayItemAmount.Text;
                                    this.FormulaText = txtPayItemAmount.Text;
                                }
                                else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Formula)
                                {
                                    this.FormulaValue = ucFormulaMaster.FormulaValue;
                                    this.FormulaText = txtAmountOrFormula_PayItem.Text.Trim().HtmlEncode();
                                }
                                else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Slab)
                                {
                                    this.FormulaValue = ddlSlab.SelectedValue;
                                    this.FormulaText = Convert.ToInt32(ddlSlab.SelectedValue) > 0 ? ddlSlab.SelectedItem.ToString() : string.Empty;
                                }
                                else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Custom)
                                {
                                    this.FormulaValue = ddlCustome.SelectedValue; ;
                                    this.FormulaText = Convert.ToInt32(ddlCustome.SelectedValue) > 0 ? ddlCustome.SelectedItem.ToString() : string.Empty;
                                }
                                else
                                {
                                    this.FormulaValue = null;
                                    this.FormulaText = null;
                                }
                            }                            
                           
                            this.PayClassification = GetNullableInt(ddlPayClassification_PayItem.SelectedValue).Value;
                            this.PayElement = GetNullableInt(ddlPayElement_PayItem.SelectedValue).Value;
                            this.ElementTypeCalcMode = GetNullableInt(ddlType.SelectedValue).Value;
                            SalaryTemplateBO.PayElement payEelement = PayElementsViewState
                                .Where(x => x.PelPk == PayElement)
                                .Single();
                            this.PayElementName = payEelement.PelName;
                            this.PayElementDeduction = payEelement.PelDeduction;                            
                            AfterApply(this, EventArgs.Empty);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(" + ddlType.SelectedValue + ");", true);
                        break;
                    #endregion

                    #region ELEMENTTYPE
                    case ActionsEnum.ELEMENTTYPE:
                        txtAmountOrFormula_PayItem.Text = string.Empty;
                        if (Convert.ToInt32(ddlPayElement_PayItem.SelectedValue) > 0)
                        {
                            this.PayElement = GetNullableInt(ddlPayElement_PayItem.SelectedValue).Value;
                            SalaryTemplateBO.PayElement payEelement = PayElementsViewState
                                .Where(x => x.PelPk == PayElement)
                                .Single();
                            ddlType.SelectedValue = payEelement.PelCalcMode.ToString();
                            ucFormulaMaster.IsDeduction = payEelement.PelDeduction ? 1 : 0;
                        }
                        else
                        {
                            ddlType.SelectedValue = CommonConstants.SELECTVAL;
                            ucFormulaMaster.IsDeduction = 0;
                        }                      

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(" + ddlType.SelectedValue + ");", true);
                        if (Convert.ToInt32(ddlType.SelectedValue) > Convert.ToInt32(CommonConstants.SELECTVAL))
                        {
                            if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.FixedAmount)
                            {
                                txtPayItemAmount.Text = GetFormattedCurrency(0);
                            }
                            else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.FixedAmount)
                            {
                                txtPayItemAmount.Text = string.Empty;
                            }
                            else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Slab)
                            {
                                GetFieldValues(ControlsEnum.SLAB);
                                SetFieldValues(ControlsEnum.SLAB);
                            }
                            else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Custom)
                            {
                                GetFieldValues(ControlsEnum.CUSTOM);
                                SetFieldValues(ControlsEnum.CUSTOM);
                            }
                        }
                        break;
                    #endregion
                    #region CHANGE ELEMENT TYPE
                    case ActionsEnum.CHANGETYPE:
                        txtAmountOrFormula_PayItem.Text = string.Empty;
                        if (Convert.ToInt32(ddlType.SelectedValue) > Convert.ToInt32(CommonConstants.SELECTVAL))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(" + ddlType.SelectedValue + ");", true);
                            if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.FixedAmount)
                            {
                                txtPayItemAmount.Text = GetFormattedCurrency(0);
                            }
                            else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Formula)
                            {
                                txtPayItemAmount.Text = string.Empty;
                            }
                            else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Slab)
                            {
                                GetFieldValues(ControlsEnum.SLAB);
                                SetFieldValues(ControlsEnum.SLAB);
                            }
                            else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Custom)
                            {
                                GetFieldValues(ControlsEnum.CUSTOM);
                                SetFieldValues(ControlsEnum.CUSTOM);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(1);", true);
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            try
            {
                switch (type)
                {
                    #region PAYCLASIFICATION
                    case ControlsEnum.PAYCLASIFICATION:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PAY ELEMENT CLASSIFICATIONS");
                        break;
                    #endregion
                    #region PAYELEMENTS
                    case ControlsEnum.PAYELEMENTS:
                        int pelClass = (GetNullableInt(ddlPayClassification_PayItem.SelectedValue) ?? 0);
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(Convert.ToInt32(dummyPayElement_PK == string.Empty ? "0" : dummyPayElement_PK), Convert.ToInt32(CommonConstants.ACTIVE)
                            , currentUser.SBUID, 0, 0, pelClass);
                        break;
                    #endregion
                    #region Pay Type
                    case ControlsEnum.PAYTYPE:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PAY ELEMENT CALCULATION MODE");
                        break;
                    case ControlsEnum.SLAB:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetPayElementValueSingleList(Convert.ToInt32(ddlPayElement_PayItem.SelectedValue), (int)DbActiveStatus.ACTIVE, Convert.ToInt32(dummySlab_PK == string.Empty ? "0" : dummySlab_PK));
                       // dtResult = BusinessLogic.CommonManagement.CommonBL.GetSlab(Convert.ToInt32(dummySlab_PK == string.Empty ? "0" : dummySlab_PK), Convert.ToInt32(CommonConstants.ACTIVE), currentUser.SBUID, (ddlPayElement_PayItem.SelectedValue));
                        break;
                    case ControlsEnum.CUSTOM:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PAY ELEMENT CUSTOM CALCULATION");
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
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region PAYCLASIFICATION
                    case ControlsEnum.PAYCLASIFICATION:
                        BindDropDown(ControlsEnum.PAYCLASIFICATION);
                        break;
                    #endregion
                    #region PAYELEMENTS
                    case ControlsEnum.PAYELEMENTS:
                        BindDropDown(ControlsEnum.PAYELEMENTS);
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
                            if (row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_IS_DEDUCTION] != null)
                                element.PelDeduction = (GetNullableInt((row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_IS_DEDUCTION]).ToString()) ?? 0) == 0 ? false : true;
                            element.PelCalcMode = Convert.ToInt32(row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_CALC_MODE]);
                            tempPayElementList.Add(element);
                        }
                        PayElementsViewState = tempPayElementList;
                        break;
                    #endregion
                    #region PAYELEMENTCONFIGURATION
                    case ControlsEnum.PAYELEMENTCONFIGURATION:
                        Dictionary<string, int> tempPayElementConfiguration = new Dictionary<string, int>();
                        foreach (DataRow row in dtResult.Rows)
                        {
                            if (row["CFG_DATA"] != null && row["CFG_VALUE"] != null)
                                tempPayElementConfiguration.Add(row["CFG_DATA"].ToString(), GetNullableInt(row["CFG_VALUE"].ToString()).Value);
                        }
                        PayElementConfigurationViewState = tempPayElementConfiguration;
                        break;
                    #endregion
                    #region Pay Type
                    case ControlsEnum.PAYTYPE:
                        BindDropDown(ControlsEnum.PAYTYPE);
                        break;
                    #endregion
                    #region Slab
                    case ControlsEnum.SLAB:
                        BindDropDown(ControlsEnum.SLAB);
                        break;
                    #endregion
                    #region Custome
                    case ControlsEnum.CUSTOM:
                        BindDropDown(ControlsEnum.CUSTOM);
                        break;
                    #endregion
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
                #region PAYCLASIFICATION
                case ControlsEnum.PAYCLASIFICATION:
                    ddlPayClassification_PayItem.Items.Clear();
                    ddlPayClassification_PayItem.DataSource = dtResult;
                    ddlPayClassification_PayItem.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
                    ddlPayClassification_PayItem.DataValueField = GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
                    ddlPayClassification_PayItem.DataBind();
                    ddlPayClassification_PayItem.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlPayClassification_PayItem.Items.HtmlDecode();
                    break;
                #endregion
                #region PAYELEMENTS
                case ControlsEnum.PAYELEMENTS:
                    ddlPayElement_PayItem.Items.Clear();
                    if (dtResult != null)
                    {
                        ddlPayElement_PayItem.DataSource = dtResult;
                        ddlPayElement_PayItem.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME;
                        ddlPayElement_PayItem.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK;

                    }
                    else
                    {
                        ddlPayElement_PayItem.DataSource = null;
                    }
                    ddlPayElement_PayItem.DataBind();
                    ddlPayElement_PayItem.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlPayElement_PayItem.Items.HtmlDecode();
                    break;
                #endregion
                #region Pay Type
                case ControlsEnum.PAYTYPE:
                    ddlType.Items.Clear();
                    ddlType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD);
                    ddlType.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                    ddlType.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                    ddlType.DataBind();
                    ddlType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Slab
                case ControlsEnum.SLAB:
                    ddlSlab.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlSlab.DataSource = dtResult;
                        ddlSlab.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_VAL_NAME;
                        ddlSlab.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_VAL_PK;
                        ddlSlab.DataBind();
                        ddlSlab.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    }
                    break;
                #endregion
                #region Custome
                case ControlsEnum.CUSTOM:
                    ddlCustome.Items.Clear();
                    ddlCustome.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD);
                    ddlCustome.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                    ddlCustome.DataValueField = GTIService.Constants.Common.Common.CFG_PK_FIELD;
                    ddlCustome.DataBind();
                    ddlCustome.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

            }
        }
        #endregion

        #region UtitlityMethods
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        #endregion

        #region SetData
        public void SetData(string formulaText, string formula, int payClassifiction, int payElement, int payCalcmode, decimal minAmount, decimal maxAmount)
        {
            this.FormulaText = formulaText;           
            this.PayClassification = PayClassification;
            this.PayElement = PayElement;
            this.ElementTypeCalcMode = payCalcmode;
            this.MinimunAmount = minAmount;
            this.MaximumAmount = maxAmount;           
            ddlPayClassification_PayItem.SelectedIndex = ddlPayClassification_PayItem.Items.IndexOf(ddlPayClassification_PayItem.Items.FindByValue(payClassifiction.ToString()));
            //ActionHandler(ddlPayClassification_PayItem, EventArgs.Empty);
            if (ddlPayClassification_PayItem.SelectedIndex > 0)
            {
                dummySlab_PK = formula;
                dummyPayElement_PK =Convert.ToString(payElement);
                GetFieldValues(ControlsEnum.PAYELEMENTS);
                SetFieldValues(ControlsEnum.PAYELEMENTS);
            }
            else
            {
                BindDropDown(ControlsEnum.PAYELEMENTS);
            }
            ddlPayElement_PayItem.SelectedIndex = ddlPayElement_PayItem.Items.IndexOf(ddlPayElement_PayItem.Items.FindByValue(payElement.ToString()));
            ddlType.SelectedValue = payCalcmode.ToString();
            if (ddlPayClassification_PayItem.SelectedIndex > 0 &&  (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Slab))
            {
                GetFieldValues(ControlsEnum.SLAB);
                SetFieldValues(ControlsEnum.SLAB);
            }
            if (Convert.ToInt32(ddlType.SelectedValue) > Convert.ToInt32(CommonConstants.SELECTVAL))
            {
                if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.FixedAmount)
                {
                    txtPayItemAmount.Text = formulaText;
                    txtAmountOrFormula_PayItem.Text = string.Empty;
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Formula)
                {
                    txtPayItemAmount.Text = string.Empty;
                    txtAmountOrFormula_PayItem.Text = formulaText;
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Slab)
                {
                    ddlSlab.SelectedIndex = ddlSlab.Items.IndexOf(ddlSlab.Items.FindByValue(formula.ToString()));
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == (int)PayElementCalcMode.Custom)
                {
                    ddlCustome.SelectedIndex = ddlCustome.Items.IndexOf(ddlCustome.Items.FindByValue(formula.ToString()));
                }
                else
                {
                    txtPayItemAmount.Text = string.Empty;
                    txtAmountOrFormula_PayItem.Text = string.Empty;
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(" + ddlType.SelectedValue + ");", true);
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Reset Form data
        /// </summary>
        public void ResetForm()
        {
            ddlPayClassification_PayItem.SelectedIndex = ddlPayElement_PayItem.SelectedIndex = 0;
            txtAmountOrFormula_PayItem.Text = hdfFormula.Value = string.Empty;
            txtPayItemAmount.Text = string.Empty;
            this.MaximumAmount = this.MaximumAmount = 0;
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(1);", true);
        }
        public void ResetMode()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(" + ddlType.SelectedValue + ");", true);
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            PAYCLASIFICATION,
            PAYELEMENTS,
            CLEAR,
            ADD,
            PAYELEMENTCONFIGURATION,
            PAYTYPE,
            SLAB,
            CUSTOM
        }
        #endregion
    }
}