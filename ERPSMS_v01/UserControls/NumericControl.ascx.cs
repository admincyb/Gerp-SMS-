using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.CommonManagement;
using System.Data;
using System.Globalization;

namespace ERPSMS_v01.UserControls
{
    public partial class NumericControl : System.Web.UI.UserControl
    {
        #region Properties
        /// <summary>
        /// Gets or sets the text content of the TextBox control.
        /// </summary>
        public string Text
        {
            get
            {
                return txtFormattedAmount.Text.Replace(",", "");
            }
            set
            {
                txtFormattedAmount.Text = GetFormattedCurrency(value);
            }
        }
        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
        /// </summary>
        public string CssClass
        {
            get
            {
                return txtFormattedAmount.CssClass;
            }
            set
            {
                txtFormattedAmount.CssClass = value;
            }
        }
        /// <summary>
        /// Gets or sets a value that indicates whether an automatic postback to the
        ///  server occurs when the TextBox control loses focus.
        /// </summary>
        public bool AutoPostBack
        {
            get
            {
                return txtFormattedAmount.AutoPostBack;
            }
            set
            {
                txtFormattedAmount.TextChanged += new EventHandler(TextBox_TextChanged);
                txtFormattedAmount.AutoPostBack = value;
            }
        }
        /// <summary>
        /// Gets or sets the maximum number of characters allowed in the text box.
        /// </summary>
        public int MaxLength
        {
            get
            {
                return txtFormattedAmount.MaxLength;
            }
            set
            {
                txtFormattedAmount.MaxLength = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the TextBox is enabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return txtFormattedAmount.Enabled;
            }
            set
            {
                txtFormattedAmount.Enabled = value;
            }
        }
        /// <summary>
        /// Gets or sets the width of the TextBox.
        /// </summary>
        public Unit Width
        {
            get
            {
                return txtFormattedAmount.Width;
            }
            set
            {
                txtFormattedAmount.Width = value;
            }
        }
        /// <summary>
        /// Gets or sets the height of the TextBox.
        /// </summary>
        public Unit Height
        {
            get
            {
                return txtFormattedAmount.Height;
            }
            set
            {
                txtFormattedAmount.Height = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount decimal digits
        /// </summary>
        public int DecimalDigits
        {
            get
            {
                return string.IsNullOrEmpty(hdfAmountDecimals.Value) ? 2 : Convert.ToInt32(hdfAmountDecimals.Value);
            }
            set
            {
                hdfAmountDecimals.Value = value.ToString();
            }
        }
        /// <summary>
        /// Get or Sets control type
        /// </summary>
        /// <summary>
        /// Get or Sets control type
        /// </summary>
        public NumericControlType ControlType
        {
            get
            {
                return this.ViewState["ControlType"] == null ? NumericControlType.Currency : (NumericControlType)(this.ViewState["ControlType"]);
            }
            set
            {
                this.ViewState["ControlType"] = value;
                //GetNumericFormat();
            }
        }
        /// <summary>
        /// Get or sets Amount currency format(eg:#,#0.00)
        /// </summary>
        private string CurrencyFormat
        {
            get
            {
                return this.ViewState["CurrencyFormat"] == null ? "#,#0.00" : Convert.ToString(this.ViewState["CurrencyFormat"]);
            }
            set
            {
                this.ViewState["CurrencyFormat"] = value;
            }
        }
        private int CurrencyGroup1
        {
            get
            {
                return this.ViewState["CurrencyGroup1"] == null ? 0 : Convert.ToInt32(this.ViewState["CurrencyGroup1"]);
            }
            set
            {
                this.ViewState["CurrencyGroup1"] = value;
            }
        }
        private int CurrencyGroup2
        {
            get
            {
                return this.ViewState["CurrencyGroup2"] == null ? 0 : Convert.ToInt32(this.ViewState["CurrencyGroup2"]);
            }
            set
            {
                this.ViewState["CurrencyGroup2"] = value;
            }
        }
        private int NumberGroup1
        {
            get
            {
                return this.ViewState["NumberGroup1"] == null ? 0 : Convert.ToInt32(this.ViewState["NumberGroup1"]);
            }
            set
            {
                this.ViewState["NumberGroup1"] = value;
            }
        }
        private int NumberGroup2
        {
            get
            {
                return this.ViewState["NumberGroup2"] == null ? 0 : Convert.ToInt32(this.ViewState["NumberGroup2"]);
            }
            set
            {
                this.ViewState["NumberGroup2"] = value;
            }
        }

        private DataTable dtConfigData
        {
            get
            {
                return Session["dtConfigData"] == null ? new DataTable() : (DataTable)(Session["dtConfigData"]);
            }
            set
            {
                Session["dtConfigData"] = value;
            }
        }

        #endregion
        #region Variables
        BusinessObject.User currentUser;
        //DataTable dtConfigData;
        HiddenField hdfCurrencyGroup1;
        HiddenField hdfCurrencyGroup2;
        HiddenField hdfNumberGroup1;
        HiddenField hdfNumberGroup2;
        #endregion
        #region Client Side Events
        public string onkeyup
        {
            set
            {
                txtFormattedAmount.Attributes.Add("onkeyup", value);
            }
        }
        public string onkeydown
        {
            set
            {
                txtFormattedAmount.Attributes.Add("onkeydown", value);
            }
        }
        public string onchange
        {
            set
            {
                txtFormattedAmount.Attributes.Add("onchange", value);
            }
        }
        public string onblur
        {
            set
            {
                txtFormattedAmount.Attributes.Add("onblur", "FormatAmount(this);" + value);
            }
        }
        #endregion
        #region Server Side Events
        public EventHandler OnTextChanged;
        #endregion
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetNumericFormat();
                if (string.IsNullOrEmpty(CssClass))
                    txtFormattedAmount.CssClass = "numeric";
                txtFormattedAmount.Text = GetFormattedCurrency(txtFormattedAmount.Text.Replace(",", ""));
            }
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitAmountControl", "$(document).ready(function(){InitAmountControl();});", true);
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region CONFIGDATA
                    case ControlsEnum.CONFIGDATA:
                        if (dtConfigData == null || dtConfigData.Rows.Count == 0)
                            dtConfigData = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("Currency Settings", string.Empty, currentUser.SBUID);
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
        #region Functions
        #region TextBox TextChanged
        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(sender, e);
        }
        #endregion
        #region Get Numeric Format
        /// <summary>
        /// Method to get currency format
        /// </summary>
        private void GetNumericFormat()
        {
            switch (ControlType)
            {
                case NumericControlType.Currency:
                    GenerateFormat("CurrencyDecimalDigit", true, false);
                    break;
                case NumericControlType.Number:
                    GenerateFormat("NumberDecimalDigit", true, true);
                    break;
                case NumericControlType.Rate:
                    GenerateFormat("RateDecimalDigit", true, false);
                    break;
                case NumericControlType.ExchangeRate:
                    GenerateFormat("ExchRateDecimalDigit", true, false);
                    break;
                case NumericControlType.RatePurchase:
                    GenerateFormat("RateDecimalDigitP2P", true, false);
                    break;
                case NumericControlType.Weight:
                    GenerateFormat("WeightDecimalDigit", true, false);
                    break;
                case NumericControlType.NumberPurchase:
                    GenerateFormat("NumberDecimalDigitP2P", true, true);
                    break;
                case NumericControlType.NumberCompounding:
                    GenerateFormat("NumberDecimalDigitCompounding", true, true);
                    break;
                case NumericControlType.MiscRate:
                    GenerateFormat("MiscRateDecimalDigit", true, false);
                    break;
                case NumericControlType.NumberBin:
                    GenerateFormat("NumberDecimalDigitBin", true, true);
                    break;
                case NumericControlType.AvgWeightPrd:
                    GenerateFormat("AvgWeightDecimalDigitPrd", true, false);
                    break;
                case NumericControlType.WeightPrd:
                    GenerateFormat("WeightDecimalDigitPrd", true, false);
                    break;
                case NumericControlType.NumberConstruction:
                    GenerateFormat("NumberDecimalDigitConstruction", true, true);
                    break;
                case NumericControlType.RateConstruction:
                    GenerateFormat("RateDecimalDigitConstruction", true, false);
                    break;
                case NumericControlType.NumericInteger:
                    GenerateFormat(string.Empty, true, false);
                    break;
                default:
                    GenerateFormat("CurrencyDecimalDigit", true, false);
                    break;
            }
        }
        #endregion
        #region Generate Format
        /// <summary>
        /// Method to generate currency format string
        /// </summary>
        /// <param name="ConfigData">Currency settings config data</param>
        /// <param name="isCommaSeperation">Comma seperation is needed or not </param>
        /// <param name="IsNumGrp">To identify whether the it a number or currency </param>       
        /// <returns></returns>
        private string GenerateFormat(string ConfigData, bool isCommaSeperation, bool IsNumGrp)
        {

            GetFieldValues(ControlsEnum.CONFIGDATA);
            if (dtConfigData != null && dtConfigData.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(hdfAmountDecimals.Value))
                {
                    if (!string.IsNullOrEmpty(ConfigData))
                        DecimalDigits = Convert.ToInt32(dtConfigData.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == ConfigData)["ACF_VALUE"].ToString());
                    else
                        DecimalDigits = 0;
                    hdfAmountDecimals.Value = DecimalDigits.ToString();
                    //((HiddenField)(this.Page.Form.FindControl("MainContent").FindControl("hdfPrivilage"))).Value = "0";
                    hdfCurrencyGroup1 = this.Page.Master.FindControl("hdfCurrencyGroup1") as HiddenField;
                    hdfCurrencyGroup2 = this.Page.Master.FindControl("hdfCurrencyGroup2") as HiddenField;
                    hdfNumberGroup1 = this.Page.Master.FindControl("hdfNumberGroup1") as HiddenField;
                    hdfNumberGroup2 = this.Page.Master.FindControl("hdfNumberGroup2") as HiddenField;
                    if (hdfCurrencyGroup1 != null)
                    {
                        hdfCurrencyGroup1.Value = dtConfigData.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyNumberGroup1")["ACF_VALUE"].ToString();
                        CurrencyGroup1 = string.IsNullOrEmpty(hdfCurrencyGroup1.Value) ? 0 : Convert.ToInt32(hdfCurrencyGroup1.Value);
                    }
                    if (hdfCurrencyGroup2 != null)
                    {
                        hdfCurrencyGroup2.Value = dtConfigData.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyNumberGroup2")["ACF_VALUE"].ToString();
                        CurrencyGroup2 = string.IsNullOrEmpty(hdfCurrencyGroup2.Value) ? 0 : Convert.ToInt32(hdfCurrencyGroup2.Value);
                    }

                    if (hdfNumberGroup1 != null)
                    {
                        hdfNumberGroup1.Value = dtConfigData.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberGroup1")["ACF_VALUE"].ToString();
                        NumberGroup1 = string.IsNullOrEmpty(hdfNumberGroup1.Value) ? 0 : Convert.ToInt32(hdfNumberGroup1.Value);
                    }
                    if (hdfNumberGroup2 != null)
                    {
                        hdfNumberGroup2.Value = dtConfigData.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberGroup2")["ACF_VALUE"].ToString();
                        NumberGroup2 = string.IsNullOrEmpty(hdfNumberGroup2.Value) ? 0 : Convert.ToInt32(hdfNumberGroup2.Value);
                    }

                }
                CurrencyFormat = "#,#0.";
                hdfIsCommaSep.Value = "1";
                hdfIsNumGrp.Value = "0";
                if (!isCommaSeperation)
                {
                    CurrencyFormat = "#0.";
                    hdfIsCommaSep.Value = "0";
                }
                else
                {
                    if ((IsNumGrp && NumberGroup1 == 0) || (!IsNumGrp && CurrencyGroup1 == 0))
                        CurrencyFormat = "#0.";
                }
                if (IsNumGrp)
                    hdfIsNumGrp.Value = "1";
                for (int i = 0; i < DecimalDigits; i++)
                {
                    CurrencyFormat += "0";
                }

            }
            return CurrencyFormat;
        }
        #endregion
        #region Get Formatted Currency
        /// <summary>
        /// Method to get formatted currency
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string GetFormattedCurrency(object number)
        {
            NumberFormatInfo info = new NumberFormatInfo();
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            if (hdfIsNumGrp.Value == "1" && NumberGroup1 > 0)
                info.NumberGroupSizes = new int[] { NumberGroup1, NumberGroup2 };
            else if (CurrencyGroup1 > 0)
                info.NumberGroupSizes = new int[] { CurrencyGroup1, CurrencyGroup2 };

            if (info.NumberGroupSizes != null && info.NumberGroupSizes.Count() > 0)
                return num.ToString(CurrencyFormat, info);
            else
                return num.ToString(CurrencyFormat);
        }
        #endregion
        #endregion
        #region Enums
        public enum ControlsEnum
        {
            CONFIGDATA
        }
        #endregion
    }
}