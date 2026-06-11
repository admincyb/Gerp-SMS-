using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BusinessObject.CommonManagement;
using BusinessObject.Administration.Configurations;
using System.Data;
using DataAccess.Administration.Configurations;
using BusinessObject;
namespace BusinessLogic.Administration.Configurations
{
    public class UserManagementBL
    {
        /// <summary>
        /// method for  Get Role Actions Details
        /// </summary>
        /// <param name="roleActionPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataSet GetUsers(int userPK, DbActiveStatus status, int sbu)
        {
            return UserManagementDA.GetUsers(userPK, status, sbu);
        }
        /// <summary>
        /// method for  Get Department
        /// </summary>
        /// <param name="roleActionPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataSet GetDepartment(int bizUnit)
        {
            return UserManagementDA.GetDepartment(bizUnit);
        }
        
        /// <summary>
        /// method for  Get Department
        /// </summary>
        /// <param name="roleActionPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataSet GetUserGroupDepartment(int department,int user)
        {
            return UserManagementDA.GetUserGroupDepartment(department, user);
        }

        /// <summary>
        /// method for  Get User Details
        /// </summary>
        /// <param name="roleActionPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable  GetUserDetails(int userID)
        {
            return UserManagementDA.GetUserDetails(userID);
        }
        /// <summary>
        /// Get User Module Details
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable GetUserModuleDetails(int userID)
        {
            return UserManagementDA.GetUserModuleDetails(userID);
        }
        /// <summary>
        /// Get User Locations
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable GetUserLocations(int userID)
        {
            return UserManagementDA.GetUserLocations(userID);
        }
        /// <summary>
        /// Save User Locations
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveUserLocations(string strxml)
        {
            return UserManagementDA.SaveUserLocations(strxml);
        }
        /// <summary>
        /// Get Super Admin  
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable SuperAdminMstGet(int userID)
        {
            return UserManagementDA.SuperAdminMstGet(userID);
        }
        /// <summary>
        /// Get Module Users
        /// </summary>
        /// <param name="modulePK"></param>
        /// <returns></returns>
        public static DataSet GetModuleUsers(int modulePK, int sbuID)
        {
            return UserManagementDA.GetModuleUsers(modulePK, sbuID);
        }
        /// <summary>
        /// Get Users Group List
        /// </summary>
        /// <param name="modulePK"></param>
        /// <param name="userPK"></param>
        /// <param name="userGroupPK"></param>
        /// <returns></returns>
        public static DataSet GetUsersGroupList(int modulePK, int userPK, int userGroupPK, int userLogin, int searchDepartment)
        {
            return UserManagementDA.GetUsersGroupList(modulePK, userPK, userGroupPK, userLogin, searchDepartment);
        }
        /// <summary>
        /// Get User Module List
        /// </summary>
        /// <param name="modulePK"></param>
        /// <param name="activeStatus"></param>
        /// <param name="moduleConfig"></param>
        /// <returns></returns>
        public static DataTable GetUserModuleList(int modulePK, int activeStatus, string moduleConfig,int sbuID)
        {
            return UserManagementDA.GetUserModuleList(modulePK, activeStatus, moduleConfig, sbuID);
        }
        /// <summary>
        /// Get User Count
        /// </summary>
        /// <returns></returns>
        public static DataSet GetUserCount(int sbuID)
        {
            return UserManagementDA.GetUserCount(sbuID);
        }
        public static DataTable  GetTranscation(int sbuID,string license)
        {
            return UserManagementDA.GetTranscationLicense(sbuID,license);
        }

        /// <summary>
        /// method for saving roleaction details.
        /// </summary>
        /// <param name="strXml"></param>
      
        /// <returns></returns>
        public static DataSet SaveUserRole(string strXml, out int RetVal)
        {
            return UserManagementDA.SaveUserRole(strXml, out RetVal);
        }
        /// <summary>
        /// method for save User details
        /// </summary>
        /// <param name="strXml"></param>

        /// <returns></returns>
        public static int SaveUser(UserManagementBO objUserDtl, User objUser)
        {
            return UserManagementDA.SaveUser(objUserDtl, objUser);
        }
         /// <summary>
        /// method for saving roleaction details.
        /// </summary>
        /// <param name="strXml"></param>
      
        /// <returns></returns>
        public static long? SaveUserGroup(string strXml, out int RetVal)
        {
            return UserManagementDA.SaveUserGroup(strXml, out RetVal);
        }
        


        /// <summary>
        /// Get ProjectSite Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="RequisitionID"></param>
        /// <returns></returns>
        public static string GetUsersList(GridPrams grid, int bizUnit, int IsActive, User objUser, int userType)
        {
            DataSet dsSiteList = UserManagementDA.GetUsersList(grid, bizUnit,IsActive, objUser, userType);
            string jString = string.Empty;
            if (dsSiteList.Tables.Count > 1 && dsSiteList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSiteList);
            }
            return jString;
        }
        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string UsersListGetSearchValue(string searchBy, string searchValue, User objUser,int? userType=null)
        {
            DataTable dtSearch = UserManagementDA.UsersListGetSearchValue(searchBy, searchValue, objUser, userType);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Configurations.Users.Fields.TEXT, GTIService.Constants.Configurations.Users.Fields.VALUE);

        }
        /// <summary>
        /// Get User Count
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="avtive"></param>
        /// <returns></returns>
        public static DataTable GetUserCount(int userType, int avtive,int sbuID)
        { 
          return UserManagementDA.GetUserCount(userType,avtive, sbuID);
        }
        /// <summary>
        /// Delete User Details
        /// </summary>
        /// <param name="MRHPK"></param>
        /// <returns>String</returns>
        public static string DeleteUser(int UserPK)
        {
            return UserManagementDA.DeleteUser(UserPK);
        }

        public static List<UsersBO> GetUserMasterDetails(int UserPK, int Active)
        {
            List<UsersBO> result;
            result = new List<UsersBO>();
            try
            {
                DataTable dtUsers = UserManagementDA.GetUserMasterDetails(UserPK, Active);
                result = dtUsers.AsEnumerable().Select(row => new UsersBO()
                {
                    usrPK = row.Field<int>(GTIService.Constants.Configurations.Users.Fields.usrPK),
                    usrEmployeeText = row.Field<string>(GTIService.Constants.Configurations.Users.Fields.usrEmployeeText)
                }).OrderBy(o => o.usrEmployeeText).ToList();
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Get all roles of an User
        /// </summary>
        /// <param name="RequisitionID"></param>
        /// <returns></returns>
        public static string GetUserRoles(int userPk)
        {
            DataTable dtUserRoles = UserManagementDA.GetUserRoles(userPk);
            string jString = string.Empty;
            if (dtUserRoles.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtUserRoles);
            }
            return jString;
        }
            /// <summary>
        /// Get Module Dropdown
        /// </summary>
        /// <param name="modulePK"></param>
        /// <returns></returns>
        public static DataTable GetModuleDDL(int modulePK)
        {
            return UserManagementDA.GetModuleDDL(modulePK);
        }

         /// <summary>
        /// To get MIS Report list for Binding Tree view
        /// </summary>
        /// <param name="User"></param>
        /// <param name="ReportGroup"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static string GetMISReportList(int User, int ReportGroup = 0, int Bizunit = 0)
        {
            return UserManagementDA.GetMISReportList(User, ReportGroup, Bizunit);
        }

        /// <summary>
        /// Save User Reports
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveUserReports(string strxml)
        {
            return UserManagementDA.SaveUserReports(strxml);
        }
        

    }
}
