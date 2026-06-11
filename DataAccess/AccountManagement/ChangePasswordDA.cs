using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.AccountManagement;
using ERP.Utilities.Constants.DA.AccountManagement;
using ERP.Utilities;
using System.Data;

namespace DataAccess.AccountManagement
{
    public class ChangePasswordDA
    {
        /// <summary>
        /// Method for Update the password based on the usedPK
        /// </summary>
        /// <param name="UserPK"></param>
        /// <param name="ChangPwd"></param>
        /// <returns></returns>
        public static int ChangePassword(int UserPK, ChangePasswordBO ChangPwd)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                        
                new DBService.Parameters(ChangePwd.P_PK , UserPK),                
                new DBService.Parameters(ChangePwd.P_OLD_PWD , ChangPwd.OldPassword==string.Empty?(object)DBNull.Value:ChangPwd.OldPassword),                
                new DBService.Parameters(ChangePwd.P_NEW_PWD , ChangPwd.NewPassword==string.Empty?(object)DBNull.Value:ChangPwd.NewPassword),                
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, ChangePwd.SP_CHANGE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }
        /// <summary>
        /// Method to help Reset the Password corresponding UserCode
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="EntryptPwd"></param>
        /// <returns></returns>
        public static ChangePasswordBO ResetPassword(string UserCode, string EntryptPwd)
        {
            ChangePasswordBO ChangeObj;
            ChangeObj = new ChangePasswordBO();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                        
                new DBService.Parameters(ChangePwd.P_USR_Code , UserCode),   
                new DBService.Parameters(ChangePwd.P_USR_Pwd , EntryptPwd),                                                    
                new DBService.Parameters(ChangePwd.P_RET_Email ,  "0", 50,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(ChangePwd.P_RET_VAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, ChangePwd.SP_RESET, colParameters);
            ChangeObj.EmailId = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[ChangePwd.P_RET_Email]).Value);
            ChangeObj.ReturnVal = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ChangePwd.P_RET_VAL]).Value);
            return ChangeObj;
        }

        /// <summary>
        /// Method to check userid exists
        /// </summary>
        /// <param name="UserPk"></param>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public static int CheckUserIdExists(int UserPk, string UserCode, out string oldPassword)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                        
                new DBService.Parameters(ChangePwd.PusrPK , UserPk),   
                new DBService.Parameters(ChangePwd.PusrName , UserCode), 
                new DBService.Parameters(ChangePwd.P_RET_VAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(ChangePwd.PRetPass, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, ChangePwd.SP_USERIDCHECK, colParameters);
            oldPassword = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[ChangePwd.PRetPass]).Value);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ChangePwd.P_RET_VAL]).Value); ;
        }

    }
}
