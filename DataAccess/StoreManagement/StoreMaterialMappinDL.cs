using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTIService;
using System.Data;
using System.IO;
using BusinessObject;

namespace DataAccess.StoreManagement
{
    public  class StoreMaterialMappinDL
    {
        /// <summary>
        /// Get Store mapping  Details
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="deptParentPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>

        //public static DataTable GetMenuUsergrpDtls(int menuPK, int menuParentPK, int bizUnit, int userGroup)
        //{
        //    DBService dbService = new DBService();
        //    DBService.Parameters[] colParameters = null;
        //    colParameters = new DBService.Parameters[] 
        //    {        
        //          new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
        //          new DBService.Parameters(GTIService.Constants.Configurations.Menu.Parameters.MENUID, menuPK == 0 ? (object)DBNull.Value : menuPK),
        //          new DBService.Parameters(GTIService.Constants.Configurations.Menu.Parameters.MENUPARENT , menuParentPK),
        //          new DBService.Parameters(GTIService.Constants.Configurations.Menu.Parameters.MENUUSERGRP , userGroup == 0 ? (object)DBNull.Value : userGroup),
        //    };
        //    return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Menu.Procedures.SPMENUDETAILSBYGROUPTREE, colParameters).Tables[0];
        //}
    }
}
