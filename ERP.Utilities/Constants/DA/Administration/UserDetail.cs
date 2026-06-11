using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants.DA.Administration
{
    public class UserDetail
    {
        #region SPS
        public const string SP_GET_ROL_COC = "SPAD_USERS_ROL_COC_GET_DATA";
        public const string SP_GET = "SPAD_USERS_MST_GET_KV";
        public const string SP_SAVE = "SPAD_USERS_MST_SAVE";
        public const string SP_SAVE_MAP = "SPAD_USR_COC_MPG_SAVE";
        public const string SP_DELETE = "SPAD_USERS_MST_DEL";
        public const string SP_STATUSCHANGE = "SPAD_USERS_MST_ACTIVATE";
        public const string SP_GETCONSTANT = "SPAD_APP_CONST_CFG_GET_KV";
        public const string SP_GETCATEGORY = "SPAD_APP_LOV_CFG_GET_KV";
        public const string SP_GETTIMEZONE = "SPAD_TIME_ZONE_CFG_KV";
        public const string SP_GETLOCATIONCOSTCENTER = "SPAD_LOCATION_COST_CENTER_TREE";

        #endregion

        #region Fields

        public const string F_PK = "USR_PK";
        public const string F_LOCCODE = "USR_Location_Code";
        public const string F_CODE = "USR_Code";
        public const string F_LOC_CODE = "USR_LOC_Code";
        public const string F_NAME = "USR_Name";
        public const string F_CODE_NAME = "USR_Text";
        public const string F_CODE_NAME_TEXT = "User_Text";
        public const string F_PASSWORD = "USR_Pwd";
        public const string F_EMAIL = "USR_Email";
        public const string F_TIMEZONE = "USR_TimeZone";
        public const string F_THEME = "USR_Theme";
        public const string F_LANGUAGE = "USR_Lang";
        public const string F_COSTCENTER = "USR_CostCenter";
        public const string F_COSTCENTERTEXT = "CostCenter";
        public const string F_LASTMODDATE = "LAST_MOD_DT";
        public const string F_STATUS = "USR_Active";
        public const string F_COSTANTDATA = "CNS_Data";
        public const string F_COSTANTVALUE = "CNS_Value";
        public const string F_CATEGORYVALUE = "LOV_Value";
        public const string F_CATEGORYDATA = "LOV_Data";
        public const string F_TIMEZONEVALUE = "TMZ_PK";
        public const string F_TIMEZONEDATA = "TMZ_Name";
        public const string F_TREEPK = "PK";
        public const string F_TREENAME = "NAME";
        public const string F_TREEPARENT = "PARENT";
        public const string F_ISCHECKED = "Checked";
        #endregion

        #region Parameters

        public const string P_PK = "P_USR_PK";
        public const string P_LOCCODE = "P_USR_Location_Code";
        public const string P_CODE = "P_USR_Code";
        public const string P_NAME = "P_USR_Name";
        public const string P_PASSWORD = "P_USR_Pwd";
        public const string P_EMAIL = "P_USR_Email";
        public const string P_TIMEZONE = "P_USR_TimeZone";
        public const string P_THEME = "P_USR_Theme";
        public const string P_LANGUAGE = "P_USR_Lang";
        public const string P_COSTCENTER = "P_USR_CostCenter";
        public const string P_LASTMODDATE = "P_LAST_MOD_DT";
        public const string P_USERPK = "P_USR_PK";
        public const string P_UCCXML = "P_UCC_XML";
        public const string P_USERCOSTPK = "P_UCC_User";
        public const string P_CONSTANT = "P_CNS_Setting";
        public const string P_CATEGORY = "P_LOV_Category";
        public const string P_SPCONDITION = "P_CNS_SplCond";


        #endregion

        #region Others

        public const string V_SETTING_EMPTYPE = "Employment Type";
        public const string V_SPCONDITION_HR = "HR432";

        #endregion
    }
}
