using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataAccess;
using BusinessObject;
using BusinessObject.AccountManagement;

namespace BusinessLogic
{
   
    public class Login
    {
        
        /// <summary>
        /// Method to authenticate a user login
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public User Authenticate(string userID, string password, int backgroundSPcall=0, int BizUnit = 0)
        {
            User user = new User();
            try
            {
                user = LoginDL.Authenticate(userID, password,backgroundSPcall, BizUnit);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Check version
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        //public bool IsValidVersion(string version)
        //{
        //    //User user = new User();
        //    //try
        //    //{
        //    //    user = LoginDL.Authenticate(userID, password);
        //    //    return user;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw ex;
        //    //}
        //}


        /// <summary>
        /// To get User Group List based on User PK
        /// </summary>
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static List<UserGroup> GetUserGroup(int userPK)
        {
            return LoginDL.GetUserGroup(userPK);
        }

        /// <summary>
        /// Method to authenticate a user login
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public User AdminAuthenticate(string userID, string password)
        {
            User user = new User();
            try
            {
                user = LoginDL.AdminAuthenticate(userID, password);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get all the roles assaigned for a particular user
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string GetRoles(string userID)
        {
            try
            {
                string roles = string.Empty;
                roles = LoginDL.GetRoles(userID);
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}
