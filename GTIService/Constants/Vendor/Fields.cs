using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Vendor
{
    public class Fields
    {

        public const int ACTIVESTATUS = 1;
        public const string ORDERBYASC = "asc";

        //public const string VENDORPK = "DnvPpadi";
        //public const string VENDORNAME = "DnvNpmc";
        public const string VENDORNAME = "VEN_NAME";
        public const string VENDORPK = "VEN_PK";

        public const string CURRENCY = "TncDc";
        public const string CURRENCYPK = "Tncp";

        public const string VNDSTATE = "RttTst";
        public const string VNDCOUNTRY = "TncYntc";

        //public const string VNDCODE = "DnvCd";
        //public const string VNDADDRESS = "Address";
        public const string VNDCODE = "VEN_CODE";
        public const string VNDADDRESS = "VEN_ADDR1";

        public class VND_SearchFields
        {
            public const string SEARCHVAL = "VALUE";
            public const string SEARCHID = "PK";

        }
        public const string PARAMETERPK = "TMD_PK";
        public const string PARAMETERNAME = "TMD_NAME";

        //NewEval Start
        public const string GROUPPK = "TMH_PK";
        public const string GROUPNAME = "TMH_NAME";

        public const string TMDPK = "TMD_PK";
        public const string TMDMAXPOINT = "TMD_MAX_POINT";
        //New End

        public const string EVALUATIONPK = "Nevp";
        public const string EVALUATIONNAME = "NevNem";

        // public const string SUPPLIEDMATERIALPK = "DvmPK";
        // public const string SUPPLIEDMATERIALNAME = "TmrNem";
        public const string SUPPLIEDMATERIALNAME = "ITM_TEXT";
        public const string SUPPLIEDMATERIALPK = "ITV_ITEM";
        public const string SUPPLIEDMATERIALCODE = "ITM_CODE";




        //Vineeth For Vendor Terms
        public const string VENDORTERMSPK = "VET_PK";
        public const string VENDORID = "VTD_VENDOR";
        public const string VENDORTERMS = "VET_NAME";
        public const string VENDORTERMSTITLE = "VET_TITLE";
        public const string VENDORTERMSDESCRIPTION = "VET_DESC";

        //Address Type
        public const string CONFIGVALUE = "CFG_VALUE";
        public const string CONFIGTEXT = "CFG_DATA";

        public const string BANKNAME = "VBD_NAME";
        public const string BANKID = "VBD_PK";

        public class TermsSearchFields
        {
            public const string SEARCHVAL = "LocNem";
            public const string SEARCHID = "Locp";

        }
    }
}
