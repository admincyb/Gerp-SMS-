using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Accounts
{
    public class ConstUserAuth
    {
        #region ---------------ValidateUserLogin--------------------
        public const string P_USR_ID = "pUrs";
        public const string P_USR_PASSWORD = "pPdw";

        public const string C_USR_LASTLOGIN_FAIL = "USR_Last_Login_Fail";
        public const string C_USR_LASTLOGIN_SUCCESS = "USR_Last_Login_Succ";
        public const string C_USR_THEME = "MhTmn";
        public const string C_USR_TIMEZONE = "USR_TimeZone";
        public const string C_USR_PK = "PmrUrs";
        public const string C_USR_FULLNAME = "EmpNm";
        public const string C_USR_ID = "RmsDI";
        public const string SP_USR_VALIDATE = "SpTLVgn";
        public const string SP_GETUSERGROUPS = "SpGMPrgRsu";

        //Authentication Type
        public const string P_AUTPK = "P_xamPK";
        public const string P_STATUS = "P_STATUS";
        public const string C_AUTTEXT = "xamData";
        public const string C_AUTVALUE = "xamValue";

        #endregion

        #region ---------------GetUserRightsInfo--------------------
        public const string P_USR_PK = "PusrPK";
        public const string P_USR_PWD = "PusrPassword";
        public const string P_LAST_COUNT = "PLastCount";
        public const string P_PAGE_URL = "P_PageURL";
        public const string P_PageURL2 = "P_PageURL2";
        public const string PusrPK = "PusrPK";
        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_DEPT = "P_DEPT"; 

        public const string P_USR_PASSWORD_OLD = "PusrPasswordOld";
        public const string P_USR_PASSWORD_NEW = "PusrPasswordNew";

        public const string C_PAGE_PK = "PAG_PK";
        public const string C_PAGE_URL = "PAG_URL";
        public const string C_ACTION_PK = "ACT_PK";
        public const string C_ACTION = "ACT_ACTION";
        public const string C_SECTION = "ACT_SECTION";
        public const string C_RIGHTS = "ACT_FLAG";
        public const string C_TAB = "ACT_IS_TAB";
        public const string C_REPORT_ID = "PBI_URL_ID"; 

        public const string SP_GET_USR_PRIVILEDGE = "SPADM_USER_PRIVILEGES_GET";
        public const string SP_AUTHENTICATIONMODE = "spXacAutModeGetKV";
        public const string SP_PASSWORDHISTORY = "SpWkfUserPwdChangeCount";
        public const string SPADM_USER_PRIVILEGES_GET = "SPADM_USER_PRIVILEGES_GET";
        public const string SPADM_USER_PAGE_PRIVILEGE_GET = "SPADM_USER_PAGE_PRIVILEGE_GET";


        public const string SP_UPDT_USRPASWORD = "SpWkfUserPwdChange";
        public const string SP_GET_WKF_USR_PRIVILEDGE = "SpWkfUserPrivilegeGet";

        #endregion

        #region Workflow User Rights
        public const string P_User = "pUser";
        public const string P_PageUrl = "pPageUrl";
        public const string P_Reference = "pReference";
        public const string P_Dept = "pDept";
        public const string P_Type = "pType";
        #endregion

    }
}
