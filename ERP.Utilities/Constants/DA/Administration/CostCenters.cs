using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants.DA.Administration
{
    public class CostCenters
    {
        #region SPS
        public const string SP_GET_BY_USER = "SPAD_USERS_COST_CENTER_GET_KV";

        public const string SP_GET = "SPAD_COST_CENTERS_MST_GET_KV";
        public const string SP_SAVE = "SPAD_COST_CENTERS_MST_SAVE";
        public const string SP_DELETE = "SPAD_COST_CENTERS_MST_DEL";
        public const string SP_STATUSCHANGE = "SPAD_COST_CENTERS_MST_ACTIVATE";


        public const string SP_GETAIRPORTBYLOCATION = "SPAD_AIRPORTS_BY_LOCATION_GET";
        public const string SP_GETCURRENCYLOCATION = "SPAD_LOCATIONS_CURRENCY_GET";
        #endregion

        #region Fields



        public const string F_PK = "COC_PK";
        public const string F_CODE = "COC_Code";
        public const string F_NAME = "COC_Name";
        public const string F_CODE_NAME = "Cost_Center_Text";
        public const string F_APT_CODE_NAME = "APT_Text";
        public const string F_LOCATION = "COC_Location";
        public const string F_CURRENCY = "COC_Currency";

        public const string F_OTRate = "COC_OTRate";
        public const string F_MAXOTHrs = "COC_MAXOTHrs";

        public const string F_ACCOUNT1 = "COC_Account1";
        public const string F_ACCOUNT2 = "COC_Account2";
        public const string F_ACCOUNT3 = "COC_Account3";
        public const string F_DESCRIPTION = "COC_Desc";
        public const string F_LASTMODBY = "";
        public const string F_LASTMODDATETIME = "LAST_MOD_DT";
        public const string F_COCAPT = "COC_Airport";
        public const string F_APT_PK = "APT_PK";
        public const string F_APT_Code = "APT_Code";
        public const string F_APT_Name = "APT_Name";

        public const string F_LOCATION_CODE = "Location";
        public const string F_CURRENCY_CODE = "Currency";
        public const string F_AIRPORT_CODE = "Airport";
        public const string F_STATUS = "COC_Active";

        #endregion

        #region
        public const string CHECKBOXAIRPORT = "chkIsAirport";
        public const string DROPDOWNLOCATION = "ddlLocation";
        #endregion

        #region Parameters
        public const string P_PK = "P_COC_PK";
        public const string P_CODE = "P_COC_Code";
        public const string P_NAME = "P_COC_Name";
        public const string P_APT_PK = "P_APT_PK";
        public const string P_LOCATION = "P_COC_Location";
        public const string P_CURRENCY = "P_COC_Currency";
        public const string P_AIRPORT = "P_COC_Airport";
        public const string P_LOCATION_PK = "P_LOC_PK";
        public const string P_OTRate = "P_COC_OTRate";
        public const string P_MAXOTHrs = "P_COC_MAXOTHrs";
        public const string P_ACCOUNT1 = "P_COC_Account1";
        public const string P_ACCOUNT2 = "P_COC_Account2";
        public const string P_ACCOUNT3 = "P_COC_Account3";
        public const string P_DESCRIPTION = "P_COC_Desc";
        public const string P_LASTMODDATETIME = "P_LAST_MOD_DT";



        #endregion
    }
}
