using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Administration.Masters.TaxSettings
{
    public class Procedures
    {
        public const string TAXPARAMETERSGET = "SPFIN_TAX_PARAM_MST_KV";
        public const string TAXCATEGORYGET = "SPADM_CONFIG_MST_GET_KV";
        public const string SPFIN_TAX_TYPE_GET_KV = "SPFIN_TAX_TYPE_GET_KV";
        public const string SAVETAXSETTINGS = "SPFIN_TAX_MST_SAVE";
        public const string GETTAXSETTINGS = "SPFIN_TAX_MST_GET_LIST";
        public const string GETSEARCHVALUE = "SPFIN_TAX_MST_AUTO";

        public const string GETACTIVETAX = "SPFIN_TAX_MST_GET_TAX";
        public const string CATEGORYVALUEGET = "SPFIN_TAX_MST_GET_KV";
        public const string CATEGORYVALUEWITHDATEGET = "SPFIN_TAX_GET_LIST";
        public const string DELETETAXDETAILS = "SPFIN_TAX_MST_DELETE";

        public const string GETVENDOR_TAX_DTL_GET_KV = "SPPUR_VENDOR_TAX_DTL_GET_KV";
        
    }
}
