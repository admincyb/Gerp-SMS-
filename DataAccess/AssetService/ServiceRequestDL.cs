using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.AssetService;

namespace DataAccess.AssetService
{
    public class ServiceRequestDL
    {
        /// <summary>
        /// For get   lsit
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static DataTable GetServiceRequestList(GridPrams grid, User objUser, string trxNo, int vendorPk, int seriveTypePk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, grid.PageNumber), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, grid.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, objUser.SBUID),
                new DBService.Parameters(Parameters.P_SRH_NO, trxNo == string.Empty ? (object) DBNull.Value : trxNo),
                new DBService.Parameters(Parameters.P_SRH_VENDOR, vendorPk >0?vendorPk :(object)DBNull.Value), 
                new DBService.Parameters(Parameters.P_FROM_DT, grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
                new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),     
                new DBService.Parameters(Parameters.P_SRH_SERVICE_TYPE, seriveTypePk >0?seriveTypePk :(object)DBNull.Value) ,
                new DBService.Parameters(Parameters.P_SRH_STATUS, string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),
                new DBService.Parameters(Parameters.P_USER_PK, objUser.PKUser)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_REQUEST_GET_LIST, colParameters);

        } 
        public static int? SaveServiceRequestDetails(string strxml, out string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_REQUEST_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static string GetServiceRequestByPK(int itemPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(Parameters.P_SRH_PK, itemPK)
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_REQUEST_GET_XML, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }


        public static int DeleteServiceRequest(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_SRH_PK , pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_REQUEST_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// Function Used To Get all ServiceTypes
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetAssetServiceType(User objUser, int vtpPK, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(Parameters.P_ACTIVE ,  active),  
              new DBService.Parameters(Parameters.P_vtpPK,  vtpPK>0?vtpPK:(object)DBNull.Value),
              new DBService.Parameters(Parameters.P_BIZUNIT,   objUser.SBUID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPASR_MNTSERVICETYPEMST_GET_KV, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Asset Type
        /// </summary>
        /// <param name="sbuID"></param>
        /// <param name="assetPK"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetAssetType(int sbuID, string assetPK, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(Parameters.P_ACTIVE ,  active),  
              new DBService.Parameters(Parameters.P_asrPK, assetPK ),
              new DBService.Parameters(Parameters.P_BIZUNIT,   sbuID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPASR_GET_ASSET_TYPE, colParameters).Tables[0];
        }


        /// <summary>
        /// Method to get Auto Complete Search for Different Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetAssetServiceRequestNoAutocomplete(string searchBy, string searchValue, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(Parameters.P_FLD_NAME  ,  searchBy),  
              new DBService.Parameters(Parameters.P_VALUE  ,  searchValue),
              new DBService.Parameters(Parameters.P_BIZUNIT  ,  objUser.SBUID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_REQUEST_AUTO, colParameters).Tables[0];
        }
        /// Function Used To Get all store 
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetRequestingStores(User objUser, int sbuPk, int deptCategory, int deptPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("DPT_PK" ,  deptPk>0?deptPk:(object)DBNull.Value),
              new DBService.Parameters("P_ACTIVE" ,1),
              new DBService.Parameters("DPT_CATEGORY", deptCategory>0?deptCategory:(object)DBNull.Value),
              new DBService.Parameters("P_USER", objUser.PKUser>0? objUser.PKUser:(object)DBNull.Value), // 1 for scrap Store, it not assigned to any user, its default
              new DBService.Parameters("P_BIZUNIT",   sbuPk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_DEPT_STORE_GET_KV, colParameters).Tables[0];
        }

        public static DataTable GetRequestingStoresNew(User objUser, int sbuPk, int deptCategory, int deptPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("DPT_PK" ,  0),
              new DBService.Parameters("P_ACTIVE" ,1),
              new DBService.Parameters("DPT_CATEGORY", 0),
              new DBService.Parameters("P_USER", null), // 1 for scrap Store, it not assigned to any user, its default
              new DBService.Parameters("P_BIZUNIT",   sbuPk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_DEPT_STORE_GET_KV, colParameters).Tables[0];
        }


        /// <summary>
        ///Validation For Cancellation of ASR
        /// </summary>
        /// <param name="CurrPK"></param>    
        /// <returns></returns>
        public static bool ValidationForCancellationASR(int CurrPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {    
                new DBService.Parameters(Parameters.P_SRH_PK, CurrPK > 0 ? CurrPK : (object)DBNull.Value)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_REQUEST_CANCEL_CHECK, colParameters);
            return dsArchive == null || dsArchive.Tables.Count == 0 || dsArchive.Tables[0].Rows.Count == 0;
        }

        //Asset Service Request Output Report
        public static DataSet GetAssetServiceRequestReport(int RecPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(Parameters.P_SRH_PK , RecPK==0?(object) DBNull.Value:RecPK)
              };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_REQUEST_RPT, colParameters);
            return dsList;
        }

        public static DataSet GetAssetDisposalReport(int RecPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(Parameters.P_asrPK , RecPK==0?(object) DBNull.Value:RecPK)
              };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPASR_ASSET_WRITE_OFF_GET_RPT, colParameters);
            return dsList;
        }

        public static DataSet GetCWIPReport(int RecPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(Parameters.P_CWH_PK , RecPK==0?(object) DBNull.Value:RecPK)
              };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPFIN_TRX_CWIP_RPT_PRINT, colParameters);
            return dsList;
        }
    }
}
