using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService;
using DataAccess;
using System.Configuration;

namespace DataAccess
{
    public class MenuDL
    {
        /// <summary>
        /// Method for Getting menu Corresponding to user Typing Key
        /// </summary>        
        /// <author>Tiju </author> 
        /// <createddate>03/Dec/2010</createddate>
        public static DataTable GetMenu(int usrId)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
                    {   
                        new DBService.Parameters(GTIService.Constants.Accounts.Fields.FUSERID, usrId == 0 ? (object)DBNull.Value : usrId)                      
                    };

            DataSet dsMenu = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Menus.Procedures.SPUSERWISEMENUDTLS, colParameters);
            return dsMenu.Tables[0];

        }
        /// <summary>
        /// Menu Search By MenuName - Auto Complete
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static DataTable GetMenuSearch(string searchText)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
                    {   
                        new DBService.Parameters(GTIService.Constants.Menus.Parameters.MenuNameSearch, searchText)                      
                    };

            DataSet dsMenu = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Menus.Procedures.SPMENUSEARCH, colParameters);
            return dsMenu.Tables[0];
        }
        /// <summary>
        /// Get All Menu Items
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMenuDetails(int userPk, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                    {   
                        new DBService.Parameters("MENU_ID", DBNull.Value),                      
                        new DBService.Parameters("RMSP",userPk),
                        new DBService.Parameters("P_BIZUNIT", bizUnit),
                    };
            DataSet dsMenu = dbService.DataAdapter(CommandType.StoredProcedure, "SPADM_MENU_MST_GET_LIST", colParameters);
            return dsMenu.Tables[0];
        }

        /// <summary>
        /// Get menu Search Details
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static DataTable GetMenuDetailsSearch(string searchText)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
                    {   
                        new DBService.Parameters("", searchText)                      
                    };

            DataSet dsMenu = dbService.DataAdapter(CommandType.StoredProcedure, "Procedure", colParameters);
            return dsMenu.Tables[0];
        }

        /// <summary>
        /// Get All Menu Items
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMenuDetails(int userPk, int bizUnit, int deptID, string menuType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                    {   
                        new DBService.Parameters("P_USER_PK",userPk),
                        new DBService.Parameters("P_BIZUNIT", bizUnit),
                        new DBService.Parameters("P_TYPE_XML", menuType),
                    };
            DataSet dsMenu = dbService.DataAdapter(CommandType.StoredProcedure, "SPADM_MENU_LIST_TREE", colParameters);
            return dsMenu.Tables[0];
        }
    }
}
