using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.AssetService;
namespace DataAccess.AssetService
{
   public class ServiceOrderDL
    {
        /// <summary>
        /// For get   lsit
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
       public static DataTable GetServiceOrderList(GridPrams grid, User objUser, string trxNo, int vendorPk, int seriveTypePk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
               new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, grid.PageNumber), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, grid.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, objUser.SBUID),
                new DBService.Parameters(Parameters.P_OSH_NO, trxNo == string.Empty ? (object) DBNull.Value : trxNo),
                new DBService.Parameters(Parameters.P_OSH_VENDOR, vendorPk >0?vendorPk :(object)DBNull.Value), 
                new DBService.Parameters(Parameters.P_FROM_DT, grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
                new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),     
                new DBService.Parameters(Parameters.P_OSH_SERVICE_TYPE, seriveTypePk >0?seriveTypePk :(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_OSH_STATUS, string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),
                new DBService.Parameters(Parameters.P_USER_PK, objUser.PKUser)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_ORDER_GET_LIST, colParameters);
        }
        public static int? SaveServiceOrderDetails(string strxml, out string TrxNo)
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
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_ORDER_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static string GetServiceOrderByPK(int itemPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(Parameters.P_OSH_PK, itemPK)
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_ORDER_GET_XML, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }


        public static int DeleteServiceOrder(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_OSH_PK , pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_ORDER_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }
        /// <summary>
        /// Method to get Auto Complete Search 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetAssetServiceOrderNoAutocomplete(string searchBy, string searchValue, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(Parameters.P_FLD_NAME  ,  searchBy),  
              new DBService.Parameters(Parameters.P_VALUE  ,  searchValue),
              new DBService.Parameters(Parameters.P_BIZUNIT  ,  objUser.SBUID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_ORDER_AUTO, colParameters).Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataSet GetServiceRequestPending(int sbuID, int storeID, int vendor, int serviceType ,int sohPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_BIZUNIT, sbuID),                
                new DBService.Parameters(Parameters.P_SRH_VENDOR, vendor>0 ? vendor:(object)DBNull.Value ),
                new DBService.Parameters(Parameters.P_SRH_SERVICE_TYPE , serviceType>0 ?serviceType: (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_DEPT, storeID>0 ? storeID: (object)DBNull.Value)  ,
                new DBService.Parameters(Parameters.P_OSH_PK, sohPk>0 ? sohPk:(object)DBNull.Value ),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_REQ_PENDING_GET_LIST, colParameters);
        }
        public static string GetSelectedServiceRequestDetail(string strxml)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_ORDR_REQ_GET_XML, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// For get  SRVendorStoreDetails
        /// </summary>     
        /// <param name="SRPK"></param>
        /// <returns></returns>
        public static DataTable GetSRVendorStoreDetails(int srhPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(Parameters.P_SRH_PK, srhPk >0?srhPk :(object)DBNull.Value) 
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_REQ_DTL_GET, colParameters);
        }
        /// <summary>
        ///Validation For Cancellation of ASO
        /// </summary>
        /// <param name="CurrPK"></param>    
        /// <returns></returns>
        public static bool ValidationForCancellationASO(int CurrPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {    
                new DBService.Parameters(Parameters.P_OSH_PK, CurrPK > 0 ? CurrPK : (object)DBNull.Value)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_ORDER_CANCEL_CHECK, colParameters);
            return dsArchive == null || dsArchive.Tables.Count == 0 || dsArchive.Tables[0].Rows.Count == 0;
        }
        //Asset Service Order Output Report
        public static DataSet GetAssetServiceOrderReport(int RecPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(Parameters.P_OSH_PK , RecPK==0?(object) DBNull.Value:RecPK)
              };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPASR_SERVICE_ORDER_RPT, colParameters);
            return dsList;
        }

    }
}
