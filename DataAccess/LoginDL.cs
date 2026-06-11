using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService;
using BusinessObject;
using BusinessObject.AccountManagement;

namespace DataAccess
{
    public class LoginDL
    {


        /// <summary>
        /// Method to authenticate a user login
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static User Authenticate(string userID, string password, int backgroundSPcall = 0, int BizUnit = 0)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtGroups;
            User user = new User();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Accounts.Parameters.PUSERID,  userID),
                new DBService.Parameters(GTIService.Constants.Accounts.Parameters.PPASSWORD, password),
                new DBService.Parameters(GTIService.Constants.Accounts.Parameters.PBACKGROUNDSPCALL, backgroundSPcall > 0 ? backgroundSPcall :(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Accounts.Parameters.PBIZUNIT, BizUnit > 0 ? BizUnit : (object)DBNull.Value)
            };
            try
            {
                System.Data.Common.DbDataReader reader = dbService.ExecuteReader(System.Data.CommandType.StoredProcedure, GTIService.Constants.Accounts.Procedures.SPCHECKAUTHENTICATION, colParameters);
                while (reader.Read())
                {
                    user = new User();
                    user.PKUser = Convert.ToInt32(reader[GTIService.Constants.Accounts.Fields.FUSERPK]);
                    user.UserID = reader[GTIService.Constants.Accounts.Fields.FUSERID].ToString();
                    user.EmpCode = reader[GTIService.Constants.Accounts.Fields.FEMPLOYEECODE] != null ? reader[GTIService.Constants.Accounts.Fields.FEMPLOYEECODE].ToString() : "0";
                    user.IsAlertSound =reader[GTIService.Constants.Accounts.Fields.ALERTSOUND].Equals(DBNull.Value) ? 0 : Convert.ToInt32(reader[GTIService.Constants.Accounts.Fields.ALERTSOUND]);
                    user.UserCulture = reader[GTIService.Constants.Accounts.Fields.CULTURE].Equals(DBNull.Value) ? "en-US" : reader[GTIService.Constants.Accounts.Fields.CULTURE].ToString();
                    user.IsPublicUser = reader[GTIService.Constants.Accounts.Fields.FISPUBLIC].Equals(DBNull.Value) ? true : Convert.ToBoolean(reader[GTIService.Constants.Accounts.Fields.FISPUBLIC]);
                    user.EmpName = reader[GTIService.Constants.Accounts.Fields.FEMPLOYEENAME] != null ? reader[GTIService.Constants.Accounts.Fields.FEMPLOYEENAME].ToString() : "0";
                    user.PKEmployee = Convert.ToInt32(reader[GTIService.Constants.Accounts.Fields.FEMPLOYEEID] != null ? reader[GTIService.Constants.Accounts.Fields.FEMPLOYEEID].ToString() : "0");
                    user.Roles = reader[GTIService.Constants.Accounts.Fields.FUSERGROUP] != null ? reader[GTIService.Constants.Accounts.Fields.FUSERGROUP].ToString() : string.Empty;
                    user.Theme = reader[GTIService.Constants.Accounts.Fields.FTHEMENAME] != null ? reader[GTIService.Constants.Accounts.Fields.FTHEMENAME].ToString() : string.Empty;
                    user.CurrentDeptPK = user.ActiveDepID = (reader[GTIService.Constants.Accounts.Fields.FUSERDEFAULTDEPT] == null
                        || reader[GTIService.Constants.Accounts.Fields.FUSERDEFAULTDEPT].Equals(DBNull.Value) ?
                        0 : Convert.ToInt32(reader[GTIService.Constants.Accounts.Fields.FUSERDEFAULTDEPT]));
                    user.IsFirstLogin = Convert.ToInt32(reader[GTIService.Constants.Accounts.Fields.ISFIRSTLOGIN]) == 1 ? true : false;
                    user.IsSysUser = Convert.ToInt32(reader[GTIService.Constants.Accounts.Fields.ISSYSUSER]);
                    user.usrPwdModOn = Convert.ToDateTime(reader[GTIService.Constants.Accounts.Fields.PWDMODON]);
                }
                reader.NextResult();
                reader.Close();
                dbService.CloseFactoryConnection();
                colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters("PusrPK" ,user.PKUser),
                
            };
                dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, "SpWkfUserGroupGet", colParameters).Tables[0];
                if (dtGroups.Rows.Count > 0)
                {
                    user.GroupList = new System.Collections.Generic.List<UserGroup>();
                    foreach (DataRow drow in dtGroups.Rows)
                    {
                        UserGroup usrgrp = new UserGroup();
                        usrgrp.GroupName = drow[1].ToString();
                        usrgrp.GroupPK = int.Parse(drow[0].ToString());
                        user.GroupList.Add(usrgrp);
                    }
                }
                if (user.PKUser > 0)
                    return user;
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get User Group List based on User PK
        /// </summary>
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static List<UserGroup> GetUserGroup(int userPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            DataTable dtGroups;
            List<UserGroup> groupList;
            groupList = null;

            try
            {
                colParameters = new DBService.Parameters[]
                {
                    new DBService.Parameters("PusrPK" ,userPK)
                };
                dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, "SpWkfUserGroupGet", colParameters).Tables[0];
                if (dtGroups.Rows.Count > 0)
                {
                    groupList = new System.Collections.Generic.List<UserGroup>();
                    foreach (DataRow drow in dtGroups.Rows)
                    {
                        groupList.Add(new UserGroup()
                        {
                            GroupName = drow[1].ToString(),
                            GroupPK = int.Parse(drow[0].ToString())
                        });
                    }
                }
                return groupList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to authenticate a user login
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static User AdminAuthenticate(string userID, string password)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            User user = new User();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Accounts.Parameters.PUSERID,  userID),
                new DBService.Parameters(GTIService.Constants.Accounts.Parameters.PPASSWORD, password)
            };

            try
            {
                System.Data.Common.DbDataReader reader = dbService.ExecuteReader(System.Data.CommandType.StoredProcedure, GTIService.Constants.Accounts.Procedures.SPCHECKAUTHENTICATION, colParameters);
                while (reader.Read())
                {
                    user = new User();
                    user.PKUser = Convert.ToInt32(reader[GTIService.Constants.Accounts.Fields.FUSERPK]);
                    user.UserID = reader[GTIService.Constants.Accounts.Fields.FUSERID].ToString();
                    user.EmpCode = reader[GTIService.Constants.Accounts.Fields.FEMPLOYEECODE] != null ? reader[GTIService.Constants.Accounts.Fields.FEMPLOYEECODE].ToString() : "0";
                    user.EmpName = reader[GTIService.Constants.Accounts.Fields.FEMPLOYEENAME] != null ? reader[GTIService.Constants.Accounts.Fields.FEMPLOYEENAME].ToString() : "0";
                    user.PKEmployee = Convert.ToInt32(reader[GTIService.Constants.Accounts.Fields.FEMPLOYEEID] != null ? reader[GTIService.Constants.Accounts.Fields.FEMPLOYEEID].ToString() : "0");
                    user.Roles = reader[GTIService.Constants.Accounts.Fields.FUSERGROUP] != null ? reader[GTIService.Constants.Accounts.Fields.FUSERGROUP].ToString() : string.Empty;
                    user.ThemeName = reader[GTIService.Constants.Accounts.Fields.FTHEMENAME] != null ? reader[GTIService.Constants.Accounts.Fields.FTHEMENAME].ToString() : string.Empty;
                }
                reader.Close();
                dbService.CloseFactoryConnection();
                if (user.PKUser > 0)
                    return user;
                return null;
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
        public static string GetRoles(string userID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            User user = new User();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Accounts.Parameters.PUSERID,  userID),
            };
            try
            {
                System.Data.Common.DbDataReader reader = dbService.ExecuteReader(System.Data.CommandType.StoredProcedure, GTIService.Constants.Accounts.Procedures.SPGETUSERROLES, colParameters);
                while (reader.Read())
                {
                    user.Roles = reader["UserGroups"] != null ? reader["UserGroups"].ToString() : string.Empty;
                }
                reader.Close();
                dbService.CloseFactoryConnection();
                if (user.Roles.Length > 0)
                    return user.Roles;
                return string.Empty;
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }
    }
}
