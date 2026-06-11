using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTIService;
using System.Data;
using System.IO;
using GTIService.Constants.Common;

namespace DataAccess.Administration.Configurations
{
    public class MenuManagement
    {
        /// <summary>
        ///  Function Used To Save  Menu Details
        /// </summary>
        /// <param name="menuManagement"></param>
        /// <returns></returns>
        public static string SaveMenuDetails(BusinessObject.Administration.Configurations.MenuManagement menuManagement)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.PMENUID  , menuManagement.MenuPK == 0 ? (object)DBNull.Value: menuManagement.MenuPK),  
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.PMENUNAME , menuManagement.MenuName),
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.PMENUPARENTID , menuManagement.MenuParentPK),
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.PMENUPOSITION , menuManagement.MenuPosition),
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.PMENUURL , menuManagement.MenuUrl),
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.PMENUICON , "ICO"),
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.PMENUCREATEDBY , menuManagement.UserPk),
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.BIZUNIT ,menuManagement.SBU),
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SPSAVEMENU, colParameters);
            return ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Menu.Parameters.RETVAL]).Value.ToString();
        }

        /// <summary>
        /// Function Used To Get All Child Menu details by Menu id
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public static DataTable GetMenuListDtls(int menuParentID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.PMENUPARENTID , menuParentID),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SPGETMENULISTDTLS, colParameters).Tables[0];
        }

        /// <summary>
        /// Function Used To Get Details Menu details by Menu id
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public static BusinessObject.Administration.Configurations.MenuManagement GetMenuDetails(int menuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.PMENUID  , menuID)  
            };
            System.Data.Common.DbDataReader reader = dbService.ExecuteReader(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SPGETMENULISTDTLS, colParameters);
            BusinessObject.Administration.Configurations.MenuManagement menuManagement = null;
            if (reader != null && reader.Read())
            {
                menuManagement = new BusinessObject.Administration.Configurations.MenuManagement();
                menuManagement.MenuPK = reader[GTIService.Constants.Configurations.Menu.Fields.MENUID] == null ? 0 : Convert.ToInt32(reader[GTIService.Constants.Configurations.Menu.Fields.MENUID]);
                menuManagement.MenuName = reader[GTIService.Constants.Configurations.Menu.Fields.MENUNAME] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Configurations.Menu.Fields.MENUNAME]);
                menuManagement.MenuParentPK = reader[GTIService.Constants.Configurations.Menu.Fields.MENUPARENT] == null ? 0 : Convert.ToInt32(reader[GTIService.Constants.Configurations.Menu.Fields.MENUPARENT]);
                menuManagement.MenuParentName = reader[GTIService.Constants.Configurations.Menu.Fields.MENUPARENTNAME] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Configurations.Menu.Fields.MENUPARENTNAME]);
                menuManagement.MenuPosition = reader[GTIService.Constants.Configurations.Menu.Fields.MENUPOSITION] == null ? 0 : Convert.ToInt32(reader[GTIService.Constants.Configurations.Menu.Fields.MENUPOSITION]);
                menuManagement.MenuUrl = reader[GTIService.Constants.Configurations.Menu.Fields.MENULINK] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Configurations.Menu.Fields.MENULINK]);
                menuManagement.MenuIcon = reader[GTIService.Constants.Configurations.Menu.Fields.MENUICON] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Configurations.Menu.Fields.MENUICON]);
                menuManagement.HasChild = reader[GTIService.Constants.Configurations.Menu.Fields.HASCHILD] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Configurations.Menu.Fields.HASCHILD]);
            }
            return menuManagement;
        }

        /// <summary>
        /// Method Used to Delete  Menu details by Menu id
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public static string DeleteMenuDetails(int menuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Configurations.Menu.Parameters.PMENUID ,  menuID == 0 ? (object)DBNull.Value :  menuID)
            };
            return dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SPDELETEMENULISTDTLS, colParameters).ToString();
        }

        /// <summary>
        /// Save Menu Group Details 
        /// </summary>
        /// <param name="menuGroupDetails"></param>
        /// <returns></returns>
        public static string SaveMenuGroupDetails(string menuGroupDetails)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Configurations.Menu.Parameters. MENUGRPXML ,(object)menuGroupDetails ,DBService.ParameterType.XML),                 
                new DBService.Parameters( GTIService.Constants.Configurations.GroupDept.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SPMENUGROUPSAVE, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.GroupDept.Parameters.RETVAL]).Value);
        }
        /// <summary>
        /// Get Menu grp Details
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="deptParentPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>

        public static DataTable GetMenuUsergrpDtls(int menuPK, int menuParentPK, int bizUnit, int userGroup)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                  new DBService.Parameters(GTIService.Constants.Configurations.Menu.Parameters.MENUID, menuPK == 0 ? (object)DBNull.Value : menuPK),
                  new DBService.Parameters(GTIService.Constants.Configurations.Menu.Parameters.MENUPARENT , menuParentPK),
                  new DBService.Parameters(GTIService.Constants.Configurations.Menu.Parameters.MENUUSERGRP , userGroup == 0 ? (object)DBNull.Value : userGroup),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SPMENUDETAILSBYGROUPTREE, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbu"></param>
        /// <param name="department"></param>
        /// <param name="userPk"></param>
        /// <returns></returns>
        public static DataSet GetMenuDetails(int sbu, int department, int userPk, Int16 module)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.BIZUNIT , sbu),                
                new DBService.Parameters(CommonConstants.DEPARTMENT , department),
                new DBService.Parameters(CommonConstants.USERPK, userPk),
                new DBService.Parameters(CommonConstants.MODULE, module)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SP_GETMENUDETAILS, colParameters);
        }

        /// <summary>
        /// function for autocomplete , string searchField, string searchValue)
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="searchType"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetSearchMenuAuto(string searchKey, int bizUnit, int department, int userPk,short module)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.USERPK, userPk),
                new DBService.Parameters(CommonConstants.DEPARTMENT , department),
                new DBService.Parameters(CommonConstants.SEARCH_MENU, "%" + searchKey + "%"),
                new DBService.Parameters(CommonConstants.MODULE, module),
                new DBService.Parameters(CommonConstants.BIZUNIT, bizUnit)   
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SP_GETSEARCHAUTO, colParameters).Tables[0];
        }

        /// <summary>
        /// To Get Section list for Menu Edit
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="Active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetSectionList(int SectionPK, int Active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.P_MNS_PK, SectionPK),
                new DBService.Parameters(CommonConstants.P_ACTIVE , Active != -1 ? Active : (object)DBNull.Value ),
                new DBService.Parameters(CommonConstants.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SP_SPADM_MENU_SECTIONS_CFG_GET_KV, colParameters).Tables[0];
        }
        /// <summary>
        /// To Save/Update Section Details
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="SectionName"></param>
        /// <param name="ActiveStatus"></param>
        /// <returns></returns>
        public static int UpdateSectionDetails(int SectionPK, string LastModDate, string SectionName = null, string SectionNameForeign = null, string ActiveStatus = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.P_MNS_PK, SectionPK),
                new DBService.Parameters(CommonConstants.P_ACTIVE , !string.IsNullOrEmpty(ActiveStatus)? ActiveStatus : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.P_MNS_NAME, !string.IsNullOrEmpty(SectionName)? SectionName : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.P_MNS_NAME2, SectionNameForeign != null? SectionNameForeign : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.V_LAST_MOD_DT, LastModDate),
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SP_SPADM_MENU_SECTIONS_CFG_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.GroupDept.Parameters.RETVAL]).Value);
        }

        /// <summary>
        /// To Get Menu Group list for Menu Edit
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="Active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetMenuGroupList(int MenuPK, int Active,int SectionPK = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.P_MNG_PK, MenuPK),
                new DBService.Parameters(CommonConstants.P_ACTIVE , Active != -1 ? Active : (object)DBNull.Value ),
                new DBService .Parameters(CommonConstants.P_MNG_SECTION,SectionPK>0?SectionPK:(Object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SP_SPADM_MENU_GROUPS_CFG_GET_KV, colParameters).Tables[0];
        }

        /// <summary>
        /// To Save/Update Section Details
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="SectionName"></param>
        /// <param name="ActiveStatus"></param>
        /// <returns></returns>
        public static int UpdateMenuGroupDetails(int MenuGroupPK, string LastModDate, string MenuGroupName = null, string MenuGroupNameForeign = null,string ActiveStatus = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.P_MNG_PK, MenuGroupPK),
                new DBService.Parameters(CommonConstants.P_ACTIVE , !string.IsNullOrEmpty(ActiveStatus)? ActiveStatus : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.P_MNG_NAME, !string.IsNullOrEmpty(MenuGroupName)? MenuGroupName : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.P_MNG_NAME2, MenuGroupNameForeign != null ? MenuGroupNameForeign : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.V_LAST_MOD_DT, LastModDate),
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SP_SPADM_MENU_GROUPS_CFG_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.GroupDept.Parameters.RETVAL]).Value);
        }

        /// <summary>
        /// To Get Menu list for Menu Edit
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="Active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetMenuList(int MenuPK, int Active, int MenuGroupPK= 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.P_MNU_PK, MenuPK),
                new DBService.Parameters(CommonConstants.P_ACTIVE , Active != -1 ? Active : (object)DBNull.Value ),
                new DBService .Parameters(CommonConstants.P_MNU_GROUP , MenuGroupPK > 0 ? MenuGroupPK :(Object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SP_SPADM_MENU_CFG_GET_KV, colParameters).Tables[0];
        }

        /// <summary>
        /// To Save/Update Menu Details
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="SectionName"></param>
        /// <param name="ActiveStatus"></param>
        /// <returns></returns>
        public static int UpdateMenuDetails(int MenuPK, string LastModDate, string MenuName = null, string MenuNameForeign = null, string ActiveStatus = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(CommonConstants.P_MNU_PK, MenuPK),
                new DBService.Parameters(CommonConstants.P_ACTIVE , !string.IsNullOrEmpty(ActiveStatus)? ActiveStatus : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.P_MNU_NAME, !string.IsNullOrEmpty(MenuName)? MenuName : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.P_MNU_NAME2, MenuNameForeign != null ? MenuNameForeign : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.V_LAST_MOD_DT, LastModDate),
                new DBService.Parameters( GTIService.Constants.Configurations.Menu.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SP_SPADM_MENU_CFG_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.GroupDept.Parameters.RETVAL]).Value);
        }

    }
}
