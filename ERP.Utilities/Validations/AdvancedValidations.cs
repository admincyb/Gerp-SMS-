using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.SessionState;
using System.Web;
using System.Web.UI.WebControls;

namespace ERP.Utilities.Validations
{
    /// <summary>
    /// Rate Validation Control
    /// </summary>
    public class RateValidation : DecimalValidation, IRequiresSessionState
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.DecimalDigits == 0)
            {
                this.DecimalDigits = (HttpContext.Current.Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                    : Convert.ToInt32(HttpContext.Current.Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
            }
        }
    }


    /// <summary>
    /// Exchange Rate Validation Control
    /// </summary>
    public class ExchangeRateValidation : DecimalValidation, IRequiresSessionState
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.DecimalDigits == 0)
            {
                this.DecimalDigits = (HttpContext.Current.Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                    : Convert.ToInt32(HttpContext.Current.Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
            }
        }
    }

    /// <summary>
    /// Quantity Validation Control
    /// </summary>
    public class QuantityValidation : DecimalValidation
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.DecimalDigits == 0)
                this.DecimalDigits = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits;
        }
    }

    /// <summary>
    /// Quantity Validation Control (Qty Decimal Digit for Purchase)
    /// </summary>
    public class QuantityValidationP2P : DecimalValidation
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.DecimalDigits == 0)
            {
                int NoDecimalDigitsP2P = HttpContext.Current.Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(HttpContext.Current.Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                this.DecimalDigits = NoDecimalDigitsP2P;
            }
        }
    }

    /// <summary>
    /// Amount Validation Control
    /// </summary>
    public class AmountValidation : DecimalValidation
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.DecimalDigits == 0)
                this.DecimalDigits = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits;
        }
    }

    /// <summary>
    /// Decimal Validation Control
    /// </summary>
    public class DecimalValidation : System.Web.UI.WebControls.RegularExpressionValidator
    {
        #region Control State
        private Dictionary<string, object> validationControlState;
        private const String cstBaseControls = "BaseControls";
        #endregion

        #region Expression
        private const String expressionStart = "^$|^";
        private const String expressionNegative = "(-{0,1})?";
        private const String expressionNonZero = "(?=.*[1-9].*)";
        private const String expressionDecimal1 = "(([0-9]{0,";
        private const String expressionDecimal2 = "})|([0-9]{0,14})(\\.[0-9]{0,";
        private const String expressionDecimal3 = "}))?$";
        #endregion

        #region Publoc properties
        /// <summary>
        /// ValidationExpression Read only
        /// </summary>
        public new string ValidationExpression
        {
            get
            {
                return base.ValidationExpression;
            }
        }

        /// <summary>
        /// Allow Negative Values
        /// </summary>
        public bool AllowNegative
        {
            get
            {
                return _AllowNegative;
            }
            set
            {
                _AllowNegative = value;
                //this.ValidationExpression = string.Concat("^\\$?", _AllowNegative ? "(-{0,1})?" : string.Empty
                //    , "([0-9]{0,", _NumberDigits.ToString(), "})?(\\.[0-9]{0,", _DecimalDigits.ToString(), "})?$");
                //this.ValidationExpression = string.Concat(expressionStart, _AllowNegative ? expressionNegative : string.Empty
                //    , _NonZero ? expressionNonZero : string.Empty, string.Format(expressionDecimal, _NumberDigits.ToString(), _DecimalDigits.ToString()));
            }
        }
        private bool _AllowNegative;
        private const String cstAllowNegative = "AllowNegative";

        /// <summary>
        /// Make NonZero
        /// </summary>
        public bool NonZero
        {
            get
            {
                return _NonZero;
            }
            set
            {
                _NonZero = value;
                //this.ValidationExpression = string.Concat("^\\$?", _AllowNegative ? "(-{0,1})?" : string.Empty
                //    , "([0-9]{0,", _NumberDigits.ToString(), "})?(\\.[0-9]{0,", _DecimalDigits.ToString(), "})?$");
                //this.ValidationExpression = string.Concat(expressionStart, _AllowNegative ? expressionNegative : string.Empty
                //    , _NonZero ? expressionNonZero : string.Empty, string.Format(expressionDecimal, _NumberDigits.ToString(), _DecimalDigits.ToString()));
            }
        }
        private bool _NonZero;
        private const String cstNonZero = "NonZero";

        /// <summary>
        /// Number Digit Positions
        /// </summary>
        public int NumberDigits
        {
            get
            {
                return _NumberDigits;
            }
            set
            {
                _NumberDigits = value;
                //this.ValidationExpression = string.Concat("^\\$?", _AllowNegative ? "(-{0,1})?" : string.Empty
                //    , "([0-9]{0,", _NumberDigits.ToString(), "})?(\\.[0-9]{0,", _DecimalDigits.ToString(), "})?$");
                //this.ValidationExpression = string.Concat(expressionStart, _AllowNegative ? expressionNegative : string.Empty
                //    , _NonZero ? expressionNonZero : string.Empty, string.Format(expressionDecimal, _NumberDigits.ToString(), _DecimalDigits.ToString()));
            }
        }
        private int _NumberDigits;
        private const String cstNumberDigits = "NumberDigits";

        /// <summary>
        /// Decimal Digit Positions
        /// </summary>
        public int DecimalDigits
        {
            get
            {
                return _DecimalDigits;
            }
            set
            {
                _DecimalDigits = value;
                //this.ValidationExpression = string.Concat("^\\$?", _AllowNegative ? "(-{0,1})?" : string.Empty
                //    , "([0-9]{0,", _NumberDigits.ToString(), "})?(\\.[0-9]{0,", _DecimalDigits.ToString(), "})?$");
                //this.ValidationExpression = string.Concat(expressionStart, _AllowNegative ? expressionNegative : string.Empty
                //    , _NonZero ? expressionNonZero : string.Empty, string.Format(expressionDecimal, _NumberDigits.ToString(), _DecimalDigits.ToString()));
            }
        }
        private int _DecimalDigits;
        private const String cstDecimalDigits = "DecimalDigits";
        #endregion

        #region Control Events
        protected override void OnInit(EventArgs e)
        {
            this.Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }
        protected override void LoadControlState(object savedState)
        {
            validationControlState = savedState as Dictionary<string, object>;
            if (validationControlState != null)
            {
                _AllowNegative = Convert.ToBoolean(validationControlState[cstAllowNegative]);
                _NonZero = Convert.ToBoolean(validationControlState[cstNonZero]);
                _NumberDigits = Convert.ToInt32(validationControlState[cstNumberDigits]);
                _DecimalDigits = Convert.ToInt32(validationControlState[cstDecimalDigits]);
                base.LoadControlState(validationControlState[cstBaseControls]);
            }
            else
                base.LoadControlState(savedState);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.ValidationExpression = string.Concat(expressionStart, _AllowNegative ? expressionNegative : string.Empty
                    , _NonZero ? expressionNonZero : string.Empty, expressionDecimal1, _NumberDigits.ToString()
                    , expressionDecimal2, _DecimalDigits.ToString(), expressionDecimal3);
            base.OnPreRender(e);
        }
        protected override object SaveControlState()
        {
            object baseControlState = base.SaveControlState();
            validationControlState = new Dictionary<string, object>()
            {
                {cstAllowNegative,_AllowNegative},
                {cstNonZero,_NonZero},
                {cstNumberDigits,_NumberDigits},
                {cstDecimalDigits,_DecimalDigits},
                {cstBaseControls,baseControlState}
            };
            return validationControlState;
        }
        #endregion
    }

    /// <summary>
    /// Rate Validation Control
    /// </summary>
    public class RateValidationP2P : DecimalValidation, IRequiresSessionState
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.DecimalDigits == 0)
            {
                int RateDecimalDigitP2P = HttpContext.Current.Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] != null ? Convert.ToInt32(HttpContext.Current.Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P].ToString()) : 2;
                this.DecimalDigits = RateDecimalDigitP2P;
            }
        }
    }
}
