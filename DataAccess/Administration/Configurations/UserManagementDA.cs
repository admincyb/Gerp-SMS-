using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using System.Data;
using GTIService.Constants.Common;
using GTIService.Constants.Administration.Configurations;
using BusinessObject;
namespace DataAccess.Administration.Configurations
{
    public class UserManagementDA
    {
        #region Methods
        /// <summary>
        /// method for Get Role Actions  Details
        /// </summary>
        /// <param name="roleActionPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        public static DataSet GetUsers(int userPK, DbActiveStatus status, int sbu)
        {
            DataSet dsRoleActions;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            //colParameters = new DBService.Parameters[] 
            //{ 
            //    new DBService.Parameters(CommonConstants.BIZUNIT , sbu),                
            //    new DBService.Parameters(RoleActions.PK , userPK==0?(object)DBNull.Value:userPK),
            //    new DBService.Parameters(RoleActions.P_ACTIVEPAGE, (object)DBNull.Value),
            //};
            //dsRoleActions = dbService.DataAdapter(CommandType.StoredProcedure, RoleActions.SP_GETTREE, colParameters);
            return new DataSet();
            //dsRoleActions;
        }

        /// <summary>
        /// Save User details
        /// </summary>
        /// <param name="objUserDtl"></param>
        /// <param name="user"></param>
        /// <returns> INT</returns>
        public static int SaveUser(UserManagementBO objUserDtl, User objUser)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.USERID, objUserDtl.PK),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.USERNAME, objUserDtl.Name),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.PASSWORD, objUserDtl.Password),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.EMPLOYEE, objUserDtl.Employee),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.STATUS, objUserDtl.Status),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.THEME, objUserDtl.Theme),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.TYPE, objUserDtl.UserType),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.SID, objUserDtl.Sid),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.MODBY, objUser.PKUser),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.EMAIL, objUserDtl.Email),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.SIGNATURE, objUserDtl.Signature),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.SOUNDALERT, objUserDtl.IsAlertSound),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.ISPUBLIC, objUserDtl.IsPublicUser),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.ISSYSTEM_USER, objUserDtl.IsSysUser),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.usrDefInbox, objUserDtl.usrDefInbox),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.DEFAULTDEPT, objUserDtl.DefaultDepartment== string.Empty ? (object)DBNull.Value : objUserDtl.DefaultDepartment),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_RET_VAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.CULTURE, string.IsNullOrEmpty(objUserDtl.usrCulture) ? (object)DBNull.Value : objUserDtl.usrCulture),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.CULTURESEC, string.IsNullOrEmpty(objUserDtl.usrCultureSec) ? (object)DBNull.Value : objUserDtl.usrCultureSec),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.PusrPhone, objUserDtl.usrPhone),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.BIZUNIT, objUser.SBUID),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_SAVE_USERDETAILS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Users.Parameters.P_RET_VAL]).Value);

        }

        ///// <summary>
        ///// SAVING Role Action DETAILS 
        ///// </summary>
        ///// <param name="strxml"></param>
        ///// <param name="user"></param>
        ///// <returns> INT</returns>
        //public static int SaveUserRole(string strxml)
        //{

        //    DBService dbService = new DBService();
        //    DBService.Parameters[] colParameters = null;
        //    colParameters = new DBService.Parameters[] 
        //    {     
        //        new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_XML  , strxml),
        //        new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_RET_VAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
        //    };
        //    dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_SAVE_USERROLEDETAILS, colParameters);
        //    return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Users.Parameters.P_RET_VAL]).Value);

        //}
        /// <summary>
        /// Save User Roles Actions
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static DataSet SaveUserRole(string strxml, out int RetVal)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_XML  , strxml),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_RET_VAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            //dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_SAVE_USERROLEDETAILS, colParameters);
            //return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Users.Parameters.P_RET_VAL]).Value);
            DataSet dsResult = new DataSet();
            dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_SAVE_USERROLEDETAILS, colParameters);
            RetVal = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Users.Parameters.P_RET_VAL]).Value);
            return dsResult;

        }

        /// <summary>
        /// Save User Roles Actions
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static long? SaveUserGroup(string strxml, out int RetVal)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.PXML  , strxml),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.PRET_VAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            //dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_SAVE_USERROLEDETAILS, colParameters);
            //return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Users.Parameters.P_RET_VAL]).Value);
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_USER_GROUP_SAVE, colParameters);
            RetVal = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Users.Parameters.PRET_VAL]).Value);
            return RetVal;

        }




        /// <summary>
        /// method for get user group department
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <param name="objUser"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetUserGroupDepartment(int department, int user)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.DEPARTMENT,  department),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.USER,  user)
            };

            DataSet dtUserDept = new DataSet();
            dtUserDept = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_USERDEPARTMENT, colParameters);
            return dtUserDept;

        }

        /// <summary>
        /// method for get user Details
        /// </summary>
        /// <param name="userID"></param>

        /// <returns>DataSet</returns>
        public static DataTable GetUserDetails(int userID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.USERID,  userID)
            };

            DataSet dtUser = new DataSet();
            dtUser = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_USERDETAILS, colParameters);
            return dtUser.Tables[0];

        }
        /// <summary>
        /// Get User Module Details
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable GetUserModuleDetails(int userID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.USERID,  userID)
            };

            DataSet dtUser = new DataSet();
            dtUser = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_SYSTEM_USER_MODULE_GET_LIST, colParameters);
            return dtUser.Tables[0];

        }
        /// <summary>
        /// Get User Locations
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable GetUserLocations(int userID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.WUL_USER,  userID)
            };

            DataSet dtUserLoc = new DataSet();
            dtUserLoc = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_USER_LOCATION_MPG_GET, colParameters);
            return dtUserLoc.Tables[0];

        }

        /// <summary>
        /// Save User Locations
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveUserLocations(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.PXML  , strxml),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.PRET_VAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_USER_LOCATION_MPG_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Users.Parameters.PRET_VAL]).Value);
        }

        /// <summary>
        /// Get Super Admin  
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable SuperAdminMstGet(int userID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.USERID,  userID)
            };

            DataSet dtUser = new DataSet();
            dtUser = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_USERMST_GET, colParameters);
            return dtUser.Tables[0];

        }
        /// <summary>
        /// Get User Module List
        /// </summary>
        /// <param name="modulePK"></param>
        /// <param name="moduleConfig"></param>
        /// <returns></returns>
        public static DataTable GetUserModuleList(int modulePK, int activeStatus, string moduleConfig,int sbuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.MODULEPK,  modulePK),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.ACTIVE,  activeStatus),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.MODULECONFIG,  moduleConfig),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.BIZUNIT,  sbuID > 0 ? sbuID : (object)DBNull.Value),
            };

            DataSet dtUser = new DataSet();
            dtUser = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_SYSTEM_MODULE_CFG, colParameters);
            return dtUser.Tables[0];

        }
        /// <summary>
        /// Ge tUser Count
        /// </summary>
        /// <returns></returns>
        public static DataSet GetUserCount(int sbuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.BIZUNIT,  sbuID > 0 ? sbuID : (object)DBNull.Value),
            };
            DataSet dtUser = new DataSet();
            dtUser = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SPADM_GET_USER_COUNT, colParameters);
            return dtUser;

        }
        public static DataTable GetTranscationLicense(int sbuID,string license)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.BIZUNIT, sbuID > 0 ? sbuID : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.License,license!=string.Empty ? license : (object)DBNull.Value),
            };
            DataSet dtTranscation = new DataSet();
            dtTranscation = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SPADM_LICENCE_GET, colParameters);
            return dtTranscation.Tables[0];
        }

        /// <summary>
        /// Get Module Users
        /// </summary>
        /// <param name="modulePK"></param>
        /// <returns></returns>
        public static DataSet GetModuleUsers(int modulePK,int sbuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.MODULEPK,  modulePK),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.BIZUNIT,  sbuID > 0 ? sbuID : (object)DBNull.Value)

            };

            DataSet dsUserCount = new DataSet();
            dsUserCount = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_SYSTEM_MODULE_USER, colParameters);
            return dsUserCount;

        }
        /// <summary>
        /// Get Users Group List
        /// </summary>
        /// <param name="modulePK"></param>
        /// <returns></returns>
        public static DataSet GetUsersGroupList(int modulePK, int userPK, int userGroupPK, int userLogin, int searchDepartment)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_GUM_MODULE, modulePK >= 0 ? modulePK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_GUM_USER, userPK > 0 ? userPK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_GUM_USER_GROUP, userGroupPK > 0 ? userGroupPK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_LOGIN_USER, userLogin > 0 ? userLogin : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_DPT_PK, searchDepartment)
            };

            DataSet dsUserCount = new DataSet();
            dsUserCount = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_ADM_USER_GROUP_GET_LIST, colParameters);
            return dsUserCount;

        }

        /// <summary>
        /// method for get department
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <param name="objUser"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetDepartment(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.BIZUNIT,  bizUnit)

            };

            DataSet dtDept = new DataSet();
            dtDept = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_DEPARTMENT, colParameters);
            return dtDept;

        }

        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <param name="objUser"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetUsersList(GridPrams grid, int bizUnit, int IsActive, User objUser, int userType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.SEARCHNAME, grid.SearchBy == ((object)DBNull.Value).ToString()? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.SEARCHVAL , grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
         
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.SORTBY,   grid.SortBy==null ? "usrName" :grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.SORTDIR,  grid.SortDirection==null ? "ASC" :  grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.USERTYPE,  userType),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.BIZUNIT,  bizUnit > 0 ? bizUnit :  (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_USR_STATUS,  IsActive)
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_USERS_LIST, colParameters);
            return dtRequisition;

        }

        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="sbuPk"></param>
        /// <param name="objUser"></param>
        /// <returns>DataTable</returns>
        public static DataTable UsersListGetSearchValue(string searchBy, string searchValue, User objUser, int? userType = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Configurations.Users.Parameters.FIELDNAME ,  searchBy),
              new DBService.Parameters( GTIService.Constants.Configurations.Users.Parameters.VALUE ,  searchValue),
              new DBService.Parameters( GTIService.Constants.Configurations.Users.Parameters.USERTYPE ,  userType.HasValue ?userType: (object)DBNull.Value )
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_USERS_AUTO, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Get User Count
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static DataTable GetUserCount(int userType, int avtive,int sbuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Configurations.Users.Parameters.USERTYPE ,  userType),
              new DBService.Parameters( GTIService.Constants.Configurations.Users.Parameters.ACTIVE ,  avtive),
              new DBService.Parameters( GTIService.Constants.Configurations.Users.Parameters.BIZUNIT ,  sbuID>0 ? sbuID: (object)DBNull.Value )
            };
            DataTable dtCount = new DataTable();
            dtCount = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_USERS_COUNT, colParameters).Tables[0];
            return dtCount;

        }

        /// <summary>
        /// Delete User Details By UserPK
        /// </summary>
        /// <param name="MRHPK"></param>
        /// <returns>int= 1(Success)</returns>
        public static string DeleteUser(int UserPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.USERPK ,  UserPK),
                new DBService.Parameters( GTIService.Constants.Configurations.Users.Parameters.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_DELETE_USER, colParameters);
            return ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Users.Parameters.PRETVAL]).Value.ToString();
        }
        #endregion

        public static DataTable GetUserMasterDetails(int UserPK, int Active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.pUsrPK , UserPK),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.pActive , Active)
            };
            DataTable dtUsers = new DataTable();
            dtUsers = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_USERMST, colParameters).Tables[0];
            return dtUsers;
        }

        /// <summary>
        /// Get Item Rates
        /// </summary>
        /// <param name="itemPK"></param>
        /// <param name="vendorPK"></param>
        /// <returns></returns>
        public static DataTable GetUserRoles(int userPk)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_USER_PK, userPk > 0 ? userPk : (object)DBNull.Value)               
            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SPADM_DEPT_USER_GET_LIST, colParameters).Tables[0];
            return dtResult;
        }
        /// <summary>
        /// Get Module Dropdown
        /// </summary>
        /// <param name="itemPK"></param>
        /// <param name="vendorPK"></param>
        /// <returns></returns>

        public static DataTable GetModuleDDL(int modulePK)
        {
            DBService dbService = new DBService();


            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_SYM_PK, modulePK > 0 ? modulePK : (object)DBNull.Value)               
            };
            DataTable dtModule = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_SYSTEM_MODULE_CFG, colParameters).Tables[0];
            return dtModule;
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
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            string strRetVal = "";
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_USER_PK, User),
               new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.P_RPT_GROUP, ReportGroup > 0 ? ReportGroup : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.BIZUNIT, ReportGroup > 0 ? ReportGroup : (object)DBNull.Value)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_USER_REPORT_CFG_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// Save User Reports
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveUserReports(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.PXML  , strxml),
                new DBService.Parameters(GTIService.Constants.Configurations.Users.Parameters.PRET_VAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_SPADM_USER_REPORT_MAP_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Users.Parameters.PRET_VAL]).Value);
        }



    }
}

