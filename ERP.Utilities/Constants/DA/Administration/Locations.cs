using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants.DA.Administration
{
    public class Locations
    {

        #region SPS
        public const string SP_GET_BY_USER = "SPAD_USERS_LOCATION_GET_KV";
        public const string SP_GET = "SPAD_LOCATIONS_MST_GET_KV";
        public const string SP_SAVE = "SPAD_LOCATIONS_MST_SAVE";
        public const string SP_DELETE = "SPAD_LOCATIONS_MST_DEL";
        public const string SP_GETCURRENCY = "SPAD_CURRENCIES_MST_GET_KV";
        public const string SP_STATUSCHANGE = "SPAD_LOCATIONS_MST_ACTIVATE";

        #endregion

        #region Fields
        public const string F_PK = "LOC_PK";
        public const string F_CODE = "LOC_Code";
        public const string F_NAME = "LOC_Name";
        public const string F_CODE_NAME = "LOC_Text";
        public const string F_CURRENCY = "CURRENCY";
        public const string F_COUNTRY = "COUNTRY";
        public const string F_ACCOUNT1 = "LOC_Account1";
        public const string F_ACCOUNT2 = "LOC_Account2";
        public const string F_ACCOUNT3 = "LOC_Account3";
        public const string F_SALMONTH1 = "LOC_SalMon1";
        public const string F_SALMONTH2 = "LOC_SalMon2";
        public const string F_DESCRIPTION = "LOC_Desc";
        public const string F_CURRENCYPK = "LOC_Currency";
        public const string F_COUNTRYPK = "LOC_Country";
        public const string F_SALMONTHS = "LOC_SalMonths";
        public const string F_STATUS = "LOC_Active";
        public const string F_SALMONTHPERC1 = "LOC_SalPmt1";
        public const string F_SALMONTHPERC2 = "LOC_SalPmt2";

        #endregion

        #region Parameters
        public const string P_COUNTRYPK = "P_CNT_PK";
        public const string P_PK = "P_LOC_PK";
        public const string P_CODE = "P_LOC_Code";
        public const string P_NAME = "P_LOC_Name";
        public const string P_COUNTRY = "P_LOC_Country";
        public const string P_REGION = "P_LOC_AdmReg";
        public const string P_CURRENCY = "P_LOC_Currency";
        public const string P_SALMONTHS = "P_LOC_SalMonths";
        public const string P_SALMONTH1 = "P_LOC_SalMon1";
        public const string P_SALMONTHPERC = "P_LOC_SalPmt1";
        public const string P_SALMONTH2 = "P_LOC_SalMon2";
        public const string P_SALMONTHPERC2 = "P_LOC_SalPmt2";
        public const string P_ACCOUNT1 = "P_LOC_Account1";
        public const string P_ACCOUNT2 = "P_LOC_Account2";
        public const string P_ACCOUNT3 = "P_LOC_Account3";
        public const string P_DESCRIPTION = "P_LOC_Desc";
        public const string P_LAST_MOD_DATE = "P_LAST_MOD_DT";
        #endregion




    }
}
