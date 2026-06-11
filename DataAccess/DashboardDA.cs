using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using System.Data;
using GTIService.Dashboard;

namespace DataAccess
{
    public class DashboardDA
    {
        /// <summary>
        /// To Get Filter Data
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static DataSet GetFilterData(string procedure, string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(Dashboards.P_XML, xml),
                new DBService.Parameters(Dashboards.P_TITLE, string.Empty, ParameterDirection.Output)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, procedure, colParameters);
        }
        /// <summary>
        /// To Get Report Data
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object[] GetChartData(string procedure, string xml)
        {
            object[] result = new object[2];
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(Dashboards.P_XML, xml),
                new DBService.Parameters(Dashboards.P_TITLE, string.Empty, 1000, ParameterDirection.InputOutput, DBService.ParameterType.NVarChar)
            };
            result[1] = dbService.DataAdapter(CommandType.StoredProcedure, procedure, colParameters);
            //result[0] = colParameters[1].ParamValue;
            result[0] = ((IDataParameter)dbService.oCommand.Parameters[Dashboards.P_TITLE]).Value.ToString();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <returns></returns>
        public static DataTable GetVendorContactDetailsReport(int vndID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtxml = new DataTable();
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( "VEN_PK" ,  vndID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPPUR_VENDOR_MST_RPT", colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static DataTable GetStockTransferRptDetails(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters("P_SFH_PK",pk)
            };
            // return dbService.ExecuteScalar(CommandType.StoredProcedure, "SPINV_STK_TRAN_RPT", colParameters).ToString();
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_STK_TRAN_RPT", colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static DataTable GetMaterialIssueDetails(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( "MIH_PK" , pk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_ITEM_ISSUE_GET", colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static DataTable GetStoreAdjustDetails(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtxml = new DataTable();
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( "P_SAH_PK" ,  pk)
            };
            //dtxml = dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_STK_ADJ_GET", colParameters).Tables[0];
            //return dtxml.Rows[0][0].ToString();
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_STK_ADJ_GET", colParameters).Tables[0];
        }
        
    }
}
