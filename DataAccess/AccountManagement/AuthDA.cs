using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.AccountManagement;
using System.Data;
using GTIService.Constants.Accounts;
using BusinessObject;
using ERP.Utilities;

namespace DataAccess.AccountManagement
{
    public class AuthDA
    {
        /// <summary>
        /// Validates User Credentials
        /// </summary>
        /// <param name="UserID">UserID - Supplied By User</param>
        /// <param name="Password">Password - Supplied By User</param>
        /// <returns>UserName if valid user</returns>
        public User ValidateUserLogin(string UserID, string Password)
        {

            User iUser;
            DBService dbManager;
            DataTable dtAuthUser;
            DataTable dtGroups;
            DBService.Parameters[] colParameters = null;

            dbManager = new DBService();
            iUser = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ConstUserAuth.P_USR_ID ,UserID),
                new DBService.Parameters(ConstUserAuth.P_USR_PASSWORD,Password)
            };
            dtAuthUser = dbManager.DataAdapter(CommandType.StoredProcedure, ConstUserAuth.SP_USR_VALIDATE, colParameters).Tables[0];
            if (dtAuthUser.Rows.Count > 0)
            {
                iUser = new User();
                //iUser.LastLoginFailure = dtAuthUser.Rows[0][ConstUserAuth.C_USR_LASTLOGIN_FAIL] != DBNull.Value ? Convert.ToDateTime(dtAuthUser.Rows[0][ConstUserAuth.C_USR_LASTLOGIN_FAIL]) : DateTime.MinValue;
                //iUser.LastLoginSuccess = dtAuthUser.Rows[0][ConstUserAuth.C_USR_LASTLOGIN_SUCCESS] != DBNull.Value ? Convert.ToDateTime(dtAuthUser.Rows[0][ConstUserAuth.C_USR_LASTLOGIN_SUCCESS]) : DateTime.MinValue;
                iUser.Theme = dtAuthUser.Rows[0][ConstUserAuth.C_USR_THEME] != DBNull.Value ? Convert.ToString(dtAuthUser.Rows[0][ConstUserAuth.C_USR_THEME]) : string.Empty;
                //iUser.TimeZone = dtAuthUser.Rows[0][ConstUserAuth.C_USR_TIMEZONE] != DBNull.Value ? float.Parse(dtAuthUser.Rows[0][ConstUserAuth.C_USR_TIMEZONE].ToString()) : 0;
                iUser.PKUser = Convert.ToInt32(dtAuthUser.Rows[0][ConstUserAuth.C_USR_PK]);
                iUser.UserID = dtAuthUser.Rows[0][ConstUserAuth.C_USR_FULLNAME] != DBNull.Value ? Convert.ToString(dtAuthUser.Rows[0][ConstUserAuth.C_USR_FULLNAME]) : string.Empty;
                iUser.UserID = dtAuthUser.Rows[0][ConstUserAuth.C_USR_ID] != DBNull.Value ? Convert.ToString(dtAuthUser.Rows[0][ConstUserAuth.C_USR_ID]) : string.Empty;



                colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ConstUserAuth.P_USR_ID ,iUser.PKUser),
                
            };
                dtGroups = dbManager.DataAdapter(CommandType.StoredProcedure, ConstUserAuth.SP_GETUSERGROUPS, colParameters).Tables[0];
                if (dtGroups.Rows.Count > 0)
                {
                    iUser.GroupList = new System.Collections.Generic.List<UserGroup>();
                    foreach (DataRow drow in dtGroups.Rows)
                    {
                        UserGroup usrgrp = new UserGroup();
                        usrgrp.GroupName = drow[1].ToString();
                        usrgrp.GroupPK = int.Parse(drow[0].ToString());
                        iUser.GroupList.Add(usrgrp);
                    }
                }


            }

            return iUser;
        }

        /// <summary>
        /// Get Authentication Types
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetAuthenticationTypes(int status)
        {
            DataTable dtAutMode;
            DBService dbManager = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(ConstUserAuth.P_AUTPK, 0),
                new DBService.Parameters(ConstUserAuth.P_STATUS, status)
            };
            dtAutMode = dbManager.DataAdapter(CommandType.StoredProcedure, ConstUserAuth.SP_AUTHENTICATIONMODE, colParameters).Tables[0]; ;
            return dtAutMode;
        }

        /// <summary>
        /// Returns UserRights: Use for Menu Level Access Rights
        /// </summary>
        /// <param name="UserPK"></param>
        /// <param name="DeptPK"></param>
        /// <returns></returns>
        public UserRightsBO GetUserRightsInfo(int UserPK, string PageURL)
        {
            DBService dbManager;
            DBService.Parameters[] colParameters;
            UserRightsBO usrRights;
            UserRightBO usrRight;
            DataTable dtUserRights;

            usrRights = new UserRightsBO();
            colParameters = null;
            dbManager = new DBService();

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ConstUserAuth.P_USR_PK,UserPK),
                new DBService.Parameters(ConstUserAuth.P_PAGE_URL,PageURL)
            };
            dtUserRights = dbManager.DataAdapter(CommandType.StoredProcedure, ConstUserAuth.SP_GET_USR_PRIVILEDGE, colParameters).Tables[0];
            usrRights.UserPK = UserPK;
            if (dtUserRights.Rows.Count > 0)
            {
                foreach (DataRow drRight in dtUserRights.Rows)
                {
                    usrRight = new UserRightBO();
                    usrRight.PagePK = drRight[ConstUserAuth.C_PAGE_PK] != DBNull.Value ? Convert.ToInt32(drRight[ConstUserAuth.C_PAGE_PK]) : 0;
                    usrRight.ActionName = drRight[ConstUserAuth.C_ACTION] != DBNull.Value ? drRight[ConstUserAuth.C_ACTION].ToString() : string.Empty;
                    usrRight.PageURL = drRight[ConstUserAuth.C_PAGE_URL] != DBNull.Value ? drRight[ConstUserAuth.C_PAGE_URL].ToString() : string.Empty;
                    usrRight.SectionName = drRight[ConstUserAuth.C_SECTION] != DBNull.Value ? drRight[ConstUserAuth.C_SECTION].ToString() : string.Empty;
                    usrRight.HasActionRight = drRight[ConstUserAuth.C_SECTION] != DBNull.Value ? Convert.ToBoolean(Convert.ToInt32(drRight["UserActionRight"])) : false;

                    usrRights.Rights.Add(usrRight);
                }
            }
            return usrRights;
        }

        public UserRightsBO GetUserRightsInfo(int UserPK, string PageURL, int bizUnit,int deptPK=0)
        {
            DBService dbManager;
            DBService.Parameters[] colParameters;
            UserRightsBO usrRights;
            UserRightBO usrRight;
            DataTable dtUserRights;

            usrRights = new UserRightsBO();
            colParameters = null;
            dbManager = new DBService();

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ConstUserAuth.PusrPK,UserPK),
                new DBService.Parameters(ConstUserAuth.P_PAGE_URL,PageURL),
                new DBService.Parameters(ConstUserAuth.P_BIZUNIT,bizUnit),
                 new DBService.Parameters(ConstUserAuth.P_DEPT, deptPK ==0 ? (object)DBNull.Value : deptPK)
            };
            DataSet dsUserRights = dbManager.DataAdapter(CommandType.StoredProcedure, ConstUserAuth.SPADM_USER_PRIVILEGES_GET, colParameters);
            usrRights.UserPK = UserPK;
            if (dsUserRights != null && dsUserRights.Tables.Count > 0)
            {
                dtUserRights = dsUserRights.Tables[0];
                if (dtUserRights.Rows.Count > 0)
                {
                    foreach (DataRow drRight in dtUserRights.Rows)
                    {
                        usrRight = new UserRightBO();
                        usrRight.PagePK = drRight[ConstUserAuth.C_PAGE_PK] != DBNull.Value ? Convert.ToInt32(drRight[ConstUserAuth.C_PAGE_PK]) : 0;
                        usrRight.ActionName = drRight[ConstUserAuth.C_ACTION] != DBNull.Value ? drRight[ConstUserAuth.C_ACTION].ToString() : string.Empty;
                        usrRight.PageURL = drRight[ConstUserAuth.C_PAGE_URL] != DBNull.Value ? drRight[ConstUserAuth.C_PAGE_URL].ToString() : string.Empty;
                        usrRight.SectionName = drRight[ConstUserAuth.C_SECTION] != DBNull.Value ? drRight[ConstUserAuth.C_SECTION].ToString() : string.Empty;
                        usrRight.HasActionRight = drRight[ConstUserAuth.C_SECTION] != DBNull.Value ? Convert.ToBoolean(Convert.ToInt32(drRight["UserActionRight"])) : false;
                        usrRight.UserDeptRight = drRight[ConstUserAuth.C_SECTION] != DBNull.Value ? Convert.ToBoolean(Convert.ToInt32(drRight["UserDeptRight"])) : false;

                        usrRights.Rights.Add(usrRight);
                    }
                }
            }
            return usrRights;
        }

        /// <summary>
        /// Update Password
        /// </summary>
        /// <param name="UserPK"></param>
        /// <param name="OldPwd"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        public int ChangePassword(int UserPK, string OldPwd, string NewPwd)
        {
            DBService dbManager = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(ConstUserAuth.P_USR_PK, UserPK),
                new DBService.Parameters(ConstUserAuth.P_USR_PASSWORD_OLD, OldPwd),
                new DBService.Parameters(ConstUserAuth.P_USR_PASSWORD_NEW, NewPwd),
                new DBService.Parameters(CommonConstants.RETURNVALUE ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbManager.ExecuteNonQuery(CommandType.StoredProcedure, ConstUserAuth.SP_UPDT_USRPASWORD, colParameters);
            return Convert.ToInt32(((IDataParameter)dbManager.oCommand.Parameters[CommonConstants.RETURNVALUE]).Value);
        }
        /// <summary>
        /// Get Workflow Section Action Rights
        /// </summary>
        /// <param name="UserPK"></param>
        /// <param name="PageURL"></param>
        /// <param name="department"></param>
        /// <param name="refID"></param>
        /// <param name="pageType" value="0: if Transaction and 1: if Listing"></param>
        /// <returns></returns>
        public UserRightsBO GetWorkFlowUserRightsInfo(int UserPK, string PageURL, int department, int refID = 0, int pageType = 0)
        {
            DBService dbManager;
            DBService.Parameters[] colParameters;
            UserRightsBO usrRights;
            UserRightBO usrRight;
            DataTable dtUserRights;

            usrRights = new UserRightsBO();
            colParameters = null;
            dbManager = new DBService();

            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ConstUserAuth.P_User,UserPK),
                new DBService.Parameters(ConstUserAuth.P_PageUrl,PageURL),
                new DBService.Parameters(ConstUserAuth.P_Reference,refID > 0 ? refID : (object)DBNull.Value),
                new DBService.Parameters(ConstUserAuth.P_Dept,department),
                new DBService.Parameters(ConstUserAuth.P_Type, pageType),
            };
            dtUserRights = dbManager.DataAdapter(CommandType.StoredProcedure, ConstUserAuth.SP_GET_WKF_USR_PRIVILEDGE, colParameters).Tables[0];
            usrRights.UserPK = UserPK;
            if (dtUserRights.Rows.Count > 0)
            {
                foreach (DataRow drRight in dtUserRights.Rows)
                {
                    usrRight = new UserRightBO();
                    usrRight.PagePK = drRight[ConstUserAuth.C_PAGE_PK] != DBNull.Value ? Convert.ToInt32(drRight[ConstUserAuth.C_PAGE_PK]) : 0;
                    usrRight.ActionName = drRight[ConstUserAuth.C_ACTION] != DBNull.Value ? drRight[ConstUserAuth.C_ACTION].ToString() : string.Empty;
                    usrRight.PageURL = drRight[ConstUserAuth.C_PAGE_URL] != DBNull.Value ? drRight[ConstUserAuth.C_PAGE_URL].ToString() : string.Empty;
                    usrRight.SectionName = drRight[ConstUserAuth.C_SECTION] != DBNull.Value ? drRight[ConstUserAuth.C_SECTION].ToString() : string.Empty;
                    usrRight.HasActionRight = drRight[ConstUserAuth.C_RIGHTS] != DBNull.Value ? Convert.ToBoolean(Convert.ToInt32(drRight[ConstUserAuth.C_RIGHTS])) : false;
                    usrRight.IsTab = drRight[ConstUserAuth.C_TAB] != DBNull.Value ? Convert.ToBoolean(drRight[ConstUserAuth.C_TAB]) : false;
                    usrRight.PBIReportID= drRight[ConstUserAuth.C_REPORT_ID] != DBNull.Value ? drRight[ConstUserAuth.C_REPORT_ID].ToString() : string.Empty; 
                    usrRights.Rights.Add(usrRight);
                }
            }
            return usrRights;
        }

        public UserRightsBO GetUserPageRights(int UserPK, string PageURL, int bizUnit, int deptPK = 0,string PageURL2 = null)
        {
            DBService dbManager;
            DBService.Parameters[] colParameters;
            UserRightsBO usrRights;
            UserRightBO usrRight;
            DataTable dtUserRights;

            usrRights = new UserRightsBO();
            colParameters = null;
            dbManager = new DBService();

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ConstUserAuth.PusrPK,UserPK),
                new DBService.Parameters(ConstUserAuth.P_PAGE_URL,PageURL),
                new DBService.Parameters(ConstUserAuth.P_PageURL2,!string.IsNullOrEmpty(PageURL2)? PageURL2 :(object)DBNull.Value),
                new DBService.Parameters(ConstUserAuth.P_BIZUNIT,bizUnit),
                 new DBService.Parameters(ConstUserAuth.P_DEPT, deptPK ==0 ? (object)DBNull.Value : deptPK)
            };
            DataSet dsUserRights = dbManager.DataAdapter(CommandType.StoredProcedure, ConstUserAuth.SPADM_USER_PAGE_PRIVILEGE_GET, colParameters);
            usrRights.UserPK = UserPK;
            if (dsUserRights != null && dsUserRights.Tables.Count > 0)
            {
                dtUserRights = dsUserRights.Tables[0];
                if (dtUserRights.Rows.Count > 0)
                {
                    usrRight = new UserRightBO();
                    usrRight.UserDeptRight = dtUserRights.Rows[0]["UserDeptPageRight"] != DBNull.Value ? Convert.ToBoolean(Convert.ToInt32(dtUserRights.Rows[0]["UserDeptPageRight"])) : false;
                    usrRights.Rights.Add(usrRight);
                }
            }
            return usrRights;
        }
        /// <summary>
        /// Get Password History
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataTable GetPasswordHistory(string newPassword, int UserPK, int pwdHistoryCount)
        {
            DataTable dtAutMode;
            DBService dbManager = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(ConstUserAuth.P_USR_PK, UserPK),
                new DBService.Parameters(ConstUserAuth.P_USR_PWD, newPassword),
                 new DBService.Parameters(ConstUserAuth.P_LAST_COUNT, pwdHistoryCount)
            };
            dtAutMode = dbManager.DataAdapter(CommandType.StoredProcedure, ConstUserAuth.SP_PASSWORDHISTORY, colParameters).Tables[0]; ;
            return dtAutMode;
        }
    }
}
