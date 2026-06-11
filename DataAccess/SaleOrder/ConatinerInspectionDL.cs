using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using gErpProductionPlanning.ClassLibrary;
using ERP.Utilities;
using DataAccess;
using BusinessObject.Constants;

namespace DataAccess.ProductionDL
{
    public class ConatinerInspectionDL
    {
    /// <summary>
        /// Get Sale Order Details By SO Number For Report
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetContainerInspectionDtls(int ContainerInspectionID)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters("P_CSH_PK",  ContainerInspectionID)
                                          
            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, "SPFIN_CONTAINER_INSP_RPT", colParameters);
            return dsActualPlndLinAloc;
        }
    }
}
