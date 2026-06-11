using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.Production
{
    public class DispersionPreparationDL
    {
        /// <summary>
        /// Dispersion Preparation Print Report
        /// </summary>
        public static DataSet GetDispPreparationReport(int RequistID)
        {
            DataSet dsProcess = new DataSet();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                {                
                    new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.DISPPREPPK, RequistID),  
                };
            dsProcess = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures_DispersionPreparation.GETDISPPRINTREPORT, colParameters);
            return dsProcess;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static string GetDispersionMaterialDetail(int dispersionID, int dept)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.DISPPK, dispersionID),
              new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.DEPTPK, dept),
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures_DispersionPreparation.GETDISPERSIONDETAILS, colParameters).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispersionID"></param>
        /// <returns></returns>
        public static string GetDispersionPreparation(int dispersionID)
        {
            DBService dbService = new DBService();
            DataSet dsData;
            string result;
            result = string.Empty;
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.DISPPREPPK, dispersionID),
            };
            dsData = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures_DispersionPreparation.GETDISPERSIONPREPARATION, colParameters);
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
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataSet GetDispersionPreparationList(GridPrams grid, BusinessObject.User objUser, int procID,string pageURL=null, int dept = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                         
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ||grid.SortBy == "DTH_DATE" ? "DTH_DATE_NEW": grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.Common.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DEPTPK,dept >0?dept : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , procID ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.P_DTH_DEL_STATUS ,  grid.CancelFlag),
              new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.P_DTH_BAL_STATUS ,  grid.ShowAll),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,pageURL== string.Empty ?(object)DBNull.Value : pageURL)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures_DispersionPreparation.GETDISPERSIONPREPARATIONLIST, colParameters);
        }

        /// <summary>
        /// Function Used To save purchase request
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static List<object> SaveDispersionPreparation(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.DISPXML , xmlstr),
                new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.DISPBATCHNO , string.Empty, 4000,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures_DispersionPreparation.SAVEDISPERSIONPREPARATION, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Production.Parameters_DispersionPreparation.RETVAL]).Value.ToString();
            string dispBatchNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Production.Parameters_DispersionPreparation.DISPBATCHNO]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(dispBatchNo);
            return retvals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispersionPk"></param>
        /// <returns></returns>
        public static int DeleteDispersionPreparation(int dispersionPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.DISPPREPPK,  dispersionPk ),
                new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures_DispersionPreparation.DELETEDISPERSIONPREP, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Production.Parameters_DispersionPreparation.RETVAL]).Value);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetDISPNO()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.DISPPREPPK , 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures_DispersionPreparation.GETDISPNO, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Production.Parameters_DispersionPreparation.DISPPREPPK]).Value);
        }

        /// <summary>
        /// 'Get Purchase Request AutoComplete Details 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, BusinessObject.User objUser, int procID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY  ,  searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE   ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK   ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , procID ),

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures_DispersionPreparation.GETDISPRSIONPREPAUTO, colParameters).Tables[0];
        }

        /// <summary>
        /// 'Get Purchase Request AutoComplete Details 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataSet GetInspectionDetails(int batchPK, int batchType, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
             
              new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.BATCH ,  batchPK),  
              new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.BATCHTYPE,  batchType),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures_DispersionPreparation.GETINSPECTIONDTLS, colParameters);
        }

        /// <summary>
        /// Get Purchase Request AutoComplete Details 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetRawMaterialInspectionDetails(int trxPK, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
               new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.TRXPK,  trxPK),
               new DBService.Parameters(GTIService.Constants.Production.Parameters_DispersionPreparation.BIZUNIT,  bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures_DispersionPreparation.GETRAWMATERIALINSP, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Material Name by Material Category for Autocomplete
        /// </summary>
        /// <param name="sBU"></param>
        /// <returns></returns>
        public static DataTable GetMaterialNameAuto(int sBU, int catg, int cat_type = 0, string SearchVal = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {    
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.BizUnit, sBU),
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.CATEGORYTYPE, catg),
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.P_CAT_TYPE, cat_type),
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.P_SEARCHVAl, string.IsNullOrEmpty(SearchVal)?(object)DBNull.Value:SearchVal)
            };
            DataTable dtMaterialName = new DataTable();
            dtMaterialName = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_GetMaterialName, colParameters).Tables[0];
            return dtMaterialName;
        }
    }
}
