using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants.DA.AccountManagement
{
    public class ChangePwd
    {
        #region SPS
        public const string SP_CHANGE = "SPAD_PWD_CHANGE";
        // For Reset password
        public const string SP_RESET = "SpWkfUserResetPwd";
        // For Check UserId
        public const string SP_USERIDCHECK= "SpWkfUserNameExist";
        #endregion

        #region Fields
        public const string F_PK = "USR_PK";
        public const string F_OLD_PWD = "USR_Pwd";
        public const string F_NEW_PWD = "USR_Pwd";


        public const string F_CODE = "USR_Code";

        #endregion


        #region Parameters
        public const string P_PK = "P_USR_PK";
        public const string P_OLD_PWD = "P_OLD_PWD";
        public const string P_NEW_PWD = "P_NEW_PWD";


        // For Reset password                

        public const string P_USR_Code = "pUsrName";
        public const string P_USR_Pwd = "pUsrPassword";
        public const string P_RET_Email = "pRetEmail";
        public const string P_RET_VAL = "pRetVal";

        //For Check UserId
        public const string PusrPK = "PusrPK";
        public const string PusrName = "PusrName";
        public const string PRetVal = "PRetVal ";
        public const string PRetPass = "PRetPass ";

        #endregion
    }
}
