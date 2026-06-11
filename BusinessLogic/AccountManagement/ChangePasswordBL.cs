using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.AccountManagement;
using DataAccess.AccountManagement;

namespace BusinessLogic.AccountManagement
{
    public class ChangePasswordBL
    {
        /// <summary>
        /// Pass User Pk and Password object to data layer for saving
        /// </summary>
        /// <param name="UserPK"></param>
        /// <param name="ChangPwd"></param>
        /// <returns></returns>
        public static int ChangePassword(int UserPK, ChangePasswordBO ChangPwd)
        {
            return ChangePasswordDA.ChangePassword(UserPK, ChangPwd);

        }
        /// <summary>
        /// Pass the UserPk and EntryptPwd to data layer for saving
        /// </summary>
        /// <param name="UserPK"></param>
        /// <param name="EntryptPwd"></param>
        /// <returns></returns>
        public static ChangePasswordBO ResetPassword(string UserPK, string EntryptPwd)
        {
            return ChangePasswordDA.ResetPassword(UserPK, EntryptPwd);

        }

        /// <summary>
        /// Method to check userid exists
        /// </summary>
        /// <param name="UserPk"></param>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public static int CheckUserIdExists(int UserPk, string UserCode, out string oldPassword)
        {
            return ChangePasswordDA.CheckUserIdExists(UserPk, UserCode,out oldPassword);

        }

    }
}
