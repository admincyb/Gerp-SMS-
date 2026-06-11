using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using BusinessObject.DispersionManagement;
using System.Xml;
using System.Xml.Serialization;

using System.Data;
using System.IO;

namespace DataAccess.DispersionManagement
{
    public class DispersionMasterDL
    {
        /// <summary>
        /// Save Dispersion - Save Dispersion header and Dispersion Material Details - Pass as Xml Format
        /// </summary>
        /// <param name="DispersionMaster"></param>
        /// <returns>DispersionPK/ -1 Exception </returns>
        public static int SaveDispersionDetails(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Dispersion.Parameters.xmlStringDispersionSave , strxml),  
                 new DBService.Parameters(GTIService.Constants.Dispersion.Parameters.Retval, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Dispersion.Procedures.SAVEDISPERSIONDETAILS, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Dispersion.Parameters.Retval]).Value);
            return result;
        }

        /// <summary>
        /// method for search based on the Criteria
        /// <param name="Grid">Grid parameters</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDispersionList(GridPrams grid, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy == null ? "DSP_NAME" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "asc" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT ,bizUnit),
            };
            DataSet dtDispersions = new DataSet();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Dispersion.Procedures.GETDISPERSIONLIST, colParameters);
            return dtDispersions;
        }

        /// <summary>
        /// Get Dispersions list for filling combo
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDispersionListCombo()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Dispersion.Parameters.PK, DBNull.Value),
            };
            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Dispersion.Procedures.GETDISPERSIONFORCOMB, colParameters).Tables[0];
            return dtDispersions;
        }

        /// <summary>
        /// Get Dispersion Details By Dispersion Id as A Xml Format
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns>Xml Formatted Dispersion Details</returns>
        public static string GetDispersionDetails(int DispersionID, int dept)
        {

            DBService dbService = new DBService();
            DataSet dsData;
            string result;
            result = string.Empty;
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Dispersion.Parameters.DISPERSIONID ,  DispersionID), 
            };
            //return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Dispersion.Procedures.GETDISPERSIONDETAILS, colParameters).ToString();
            dsData = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Dispersion.Procedures.GETDISPERSIONDETAILS, colParameters);
            if (dsData != null && dsData.Tables.Count > 0)
            {
                foreach (DataRow dr in dsData.Tables[0].Rows)
                {
                    result += dr[0].ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// Delete Dispersion Details By DispersionID
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns>int- 1(Success)</returns>
        public static int DeleteDispersionDtls(int DispersionID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters( GTIService.Constants.Dispersion.Parameters.DISPERSIONPK,  DispersionID == 0 ? (object)DBNull.Value :  DispersionID),
                new DBService.Parameters(GTIService.Constants.Dispersion.Parameters.Retval, string.Empty,4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Dispersion.Procedures.DELETEDISPERSION, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Dispersion.Parameters.Retval]).Value);
        }

        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Seatch Type
        /// </summary>
        /// <returns>Datatable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int bizUnit)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY,searchBy),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchValue),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
            };
            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Dispersion.Procedures.GETAUTOCOMPLETE, colParameters).Tables[0];
            return dtDispersions;
        }


        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Seatch Type
        /// </summary>
        /// <returns>Datatable</returns>
        public static DataTable GetDispersionForDropdown(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
            };
            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Dispersion.Procedures.GETDISPERSIONFORDDL, colParameters).Tables[0];
            return dtDispersions;
        }
        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDispersionsForAuto(string searchValue, User objUser,int dept = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.SEARCHVALUE , searchValue),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.BIZUNIT,objUser.CurrentSBUPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.DEPARTMENT,dept > 0 ? dept :(object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Dispersion.Procedures.GETDISPERSIONAUTO, colParameters).Tables[0];
        }

        /// Get Dispersion Types
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDispersionTypes(int cfgPK, string cfgType, int active, int bizUnit)
        {
            DataTable dtData = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_CfgPK,cfgPK),   
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_CFG_TYPE, cfgType),     
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),                
            };
            dtData = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPADM_CONFIG_MST_GET_KV, colParameters).Tables[0];
            return dtData;
        }
    }
}
