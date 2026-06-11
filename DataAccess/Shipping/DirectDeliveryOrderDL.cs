using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Shipping;
using BusinessObject;

namespace DataAccess.Shipping
{
    public class DirectDeliveryOrderDL
    {
        /// <summary>
        /// Method to Save DirectDeliveryOrder Details
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static List<object> SaveDirectDeliveryOrder(string xmlstr, ref DataTable dtOut)
        {       
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML , xmlstr),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(Parameters.P_RET_NO, string.Empty, 4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)
                
            };
            dtOut = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPSAL_DESPATCH_DIR_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            string admissionNo = ((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_NO]).Value.ToString();  
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(admissionNo);
            return retvals;                   
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static int SaveDOAllocation(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,xmlstr),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPSAL_DESPATCH_CARTON_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataTable GetPendingSalesOrder(int sbuID, int storeID, int customer, GridPrams grid, int doPk, string ItemName, string sohNo, int sohPK = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_BIZUNIT, sbuID),
                new DBService.Parameters(Parameters.P_DEPT, storeID==0 ? (object)DBNull.Value: storeID),
                new DBService.Parameters(Parameters.P_SOH_CUSTOMER, customer==0 ? (object)DBNull.Value: customer),
                new DBService.Parameters(Parameters.P_SOH_NO , sohNo==string.Empty ?(object)DBNull.Value :sohNo ),
                new DBService.Parameters(Parameters.P_ITM_NAME , ItemName==string.Empty ?(object)DBNull.Value :ItemName ),            
                new DBService.Parameters(Parameters.P_FROM_DT , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
                new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
                new DBService.Parameters(Parameters.P_DPH_PK , doPk==0 ? (object)DBNull.Value: doPk)
                
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPSAL_ORDER_DIR_PEND_GET, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Stock Transfer Details Get
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataSet GetDirectDOGetList(GridPrams grid, User objUser, string doNoSearch, int cusPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {           
              
              new DBService.Parameters(Parameters.P_PAGE_NUM ,  grid.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  grid.PageSize),            
              new DBService.Parameters(Parameters.P_FROM_DT, grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),  
              new DBService.Parameters(Parameters.P_BIZUNIT, objUser.SBUID),             
              new DBService.Parameters(Parameters.P_DPH_NO, doNoSearch == string.Empty ? (object)DBNull.Value :doNoSearch),
              new DBService.Parameters(Parameters.P_DPH_CUSTOMER,  cusPk==0? (object)DBNull.Value:cusPk),
              new DBService.Parameters(Parameters.P_DPH_STATUS, string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPSAL_DESPATCH_DIR_GET_LIST, colParameters);
        }

        /// <summary>
        /// Get Allocate Carton DO Details
        /// </summary>
        /// <param name="dpdPK"></param>
        /// <returns></returns>
        public static DataTable GetAllocateCartonDODetails(int dpdPK)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(Parameters.P_DPD_PK, dpdPK == 0 ? (object) DBNull.Value :  dpdPK),
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPSAL_DESPATCH_CARTON_DTL_GET, colParameters).Tables[0];
            return dtResult;
        }
        /// <summary>
        /// Method to Save StockTransfer Details
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static string GetDirectDOByPk(int dphPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_DPH_PK , dphPk)
            };
            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, Procedures.SPSAL_DESPATCH_DIR_GET, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;           
        }

        /// Function Used To Get all store and store pk By Type
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDeptStores(User objUser, int sbuPk, int deptType, int deptPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("DPT_PK" ,  deptPk),
              new DBService.Parameters("P_ACTIVE" ,1),
              new DBService.Parameters("DPT_CATEGORY", deptType ),
              new DBService.Parameters("P_USER",  deptType==1?(object)DBNull.Value:objUser.PKUser), // 1 for scrap Store, it not assigned to any user, its default
              new DBService.Parameters("P_BIZUNIT",   sbuPk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_DEPT_STORE_GET_KV", colParameters).Tables[0];
        }

        /// <summary>
        /// Get Batch No
        /// </summary>
        /// <param name="itemPK"></param>
        /// <param name="vendorPK"></param>
        /// <returns></returns>
        public static DataTable GetBatchNo(int itemPK, int deptPK, int batchPK, DateTime? date = null, int BatchType = 0)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_ITM_PK, itemPK > 0 ? itemPK : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_DEPT,  deptPK > 0 ? deptPK : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_SBD_PK,  batchPK > 0 ? batchPK : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_SBD_MOD_DT,  date),
                new DBService.Parameters(Parameters.P_SBD_BATCH_TYPE,  BatchType > 0 ? BatchType :  (object)DBNull.Value)
            };
            DataTable dtItemRates = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_STK_BATCH_GET_KV, colParameters).Tables[0];
            return dtItemRates;
        }
        /// <summary>
        /// Get Batch Details
        /// </summary>
        /// <param name="itemPK"></param>
        /// <param name="vendorPK"></param>
        /// <returns></returns>
        public static DataTable GetBatchDetails(int batchPK,int ToUOM)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_SBD_PK, batchPK > 0 ? batchPK : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_SBD_TO_UOM, ToUOM > 0 ? ToUOM : (object)DBNull.Value)
            };
            DataTable dtBatchDetails = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_STK_BATCH_DTL_GET, colParameters).Tables[0];
            return dtBatchDetails;
        }

        /// <summary>
        /// Method to Delete StockTransfer Details
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteDirectDO(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_DPH_PK , pk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPSAL_DESPATCH_DIR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// <summary>
        /// Get DirectDODetail SaleOrder Details List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetDirectDOSaleOrderDetailList(int dphPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(Parameters.P_DPH_PK,  dphPk==0? (object)DBNull.Value:dphPk)              
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPSAL_DESPATCH_DIR_DTL_GET, colParameters).Tables[0];
        }

        /// <summary>
        /// Method to get Auto Complete Search 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetDirectDONoAutocomplete(string searchBy, string searchValue, User objUser, string pageURl)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(Parameters.P_FLD_NAME  ,  searchBy),  
              new DBService.Parameters(Parameters.P_VALUE  ,  searchValue),            
              new DBService.Parameters(Parameters.P_USER_PK  ,  objUser.PKUser > 0 ? objUser.PKUser : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_BIZUNIT  ,  objUser.SBUID),
              new DBService.Parameters(Parameters.P_PAGE_URL  ,  pageURl),  
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPSAL_DESPATCH_DIR_AUTO, colParameters).Tables[0];
        }

        /// <summary>
        /// Method to get DirectSOPending Item/SONO Auto Search 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetDirectSOPendingAutoComplete(string searchBy, string searchValue,User objUser, string cusPK, int dphPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(Parameters.P_FLD_NAME  ,  searchBy),  
              new DBService.Parameters(Parameters.P_VALUE  ,  searchValue),
              new DBService.Parameters(Parameters.P_BIZUNIT  ,  objUser.SBUID),
              new DBService.Parameters(Parameters.P_SOH_CUSTOMER  ,string.IsNullOrEmpty(cusPK)? (object)DBNull.Value:cusPK) ,
              new DBService.Parameters(Parameters.P_DPH_PK  ,  dphPK > 0 ? dphPK : (object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPSAL_ORDER_DIR_PEND_AUTO, colParameters).Tables[0];
        }

        /// <summary>
        ///Validation For Cancellation of Direct DO cancel 
        /// </summary>
        /// <param name="CurrPK"></param>    
        /// <returns></returns>
        public static bool ValidationForCancellationDO(int CurrPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {    
                new DBService.Parameters(Parameters.P_DPH_PK, CurrPK > 0 ? CurrPK : (object)DBNull.Value)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPSAL_DO_DIRECT_CANCEL_CHECK, colParameters);
            return dsArchive == null || dsArchive.Tables.Count == 0 || dsArchive.Tables[0].Rows.Count == 0;
        }
        public static int CheckforValidMultipleSO(string strxml)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            DataSet ds = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPSAL_DESPATCH_DIR_SO_VALDATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
