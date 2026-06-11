using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTIService.Constants.DirectStockTransfer;
using System.Data;
using BusinessObject;

namespace DataAccess.StoreManagement
{
    public class DirectStockTransferDA
    {
        /// <summary>
        /// Method to Save StockTransfer Details
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static List<object> SaveStockTransferDetails(string xmlstr)
        //public static List<object> SaveStockTransferDetails(string xmlstr, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.XML_VALUES , xmlstr),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(Parameters.P_RET_NO, string.Empty, 4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_SAVE, colParameters);
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
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataSet GetPurchaseOrderPending(int sbuID, int storeID, int vendor, GridPrams grid, int grnID, int pohPK = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_BIZUNIT, sbuID),
                new DBService.Parameters(Parameters.P_DEPT, storeID==0 ? (object)DBNull.Value: storeID),
                new DBService.Parameters(Parameters.P_VENDOR, vendor),
                new DBService.Parameters(Parameters.P_SER_NAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "POH_NO" : grid.SearchBy ),
                new DBService.Parameters(Parameters.P_SER_VAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
                new DBService.Parameters(Parameters.P_FIELDS , grid.Fields == "" ? "*" : grid.Fields),
                new DBService.Parameters(Parameters.P_FROM_DT , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
                new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
                new DBService.Parameters(Parameters.P_GRH_PK , grnID),
                new DBService.Parameters(Parameters.P_POH_PK, pohPK==0 ? (object)DBNull.Value: pohPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPPUR_ORDER_DIR_GET_LIST, colParameters);
        }


        public static DataSet GetWOPending(int sbuID, int storeID, int vendor, GridPrams grid, int grnID, int pohPK = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_BIZUNIT, sbuID),
                new DBService.Parameters(Parameters.P_DEPT, storeID==0 ? (object)DBNull.Value: storeID),
                new DBService.Parameters(Parameters.P_VENDOR, vendor),
                new DBService.Parameters(Parameters.P_SER_NAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "WIH_NO" : grid.SearchBy ),
                new DBService.Parameters(Parameters.P_SER_VAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
                new DBService.Parameters(Parameters.P_FIELDS , grid.Fields == "" ? "*" : grid.Fields),
                new DBService.Parameters(Parameters.P_FROM_DT , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
                new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
                new DBService.Parameters(Parameters.P_GRH_PK , grnID),
                new DBService.Parameters(Parameters.P_POH_PK, pohPK==0 ? (object)DBNull.Value: pohPK),
                new DBService.Parameters(Parameters.P_PENDING_WO, grid.PendingWO),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_WORK_ORDER_DIR_GET_LIST, colParameters);
        }
        /// <summary>
        /// Get Stock Transfer Details Get
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataSet GetStockTransferList(GridPrams grid, User objUser, int procID, string PageUrl, int? deptSearch, string admissionNoSearch, string poNoSearch, int? vendorSearch, int cmpPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(Parameters.P_SER_NAME , grid.SearchBy ==Fields.STRINGEMPTY ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(Parameters.P_SER_VAL , grid.SearchValue == Fields.STRINGEMPTY ? (object)DBNull.Value : Fields.VALUE_PERC + grid.SearchValue + Fields.VALUE_PERC),
              new DBService.Parameters(Parameters.P_PAGE_NO ,  grid.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  grid.PageSize),
              new DBService.Parameters(Parameters.P_FIELDS, grid.Fields == Fields.STRINGEMPTY ? Fields.VALUE_STAR : grid.Fields),
              new DBService.Parameters(Parameters.P_SORT_BY,  grid.SortBy == null ||grid.SortBy ==GTIService.Constants.DirectStockTransfer.Fields.GRH_DATE|| grid.SortBy == GTIService.Constants.DirectStockTransfer.Fields.GRH_NO ?GTIService.Constants.DirectStockTransfer.Fields.GRH_PK : grid.SortBy),
              new DBService.Parameters(Parameters.P_SORT_DIR , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(Parameters.P_FROM_DT, grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              new DBService.Parameters(Parameters.P_USER_PK ,  objUser.PKUser),
              new DBService.Parameters(Parameters.P_BIZUNIT, objUser.SBUID),

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS ,  string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),
              new DBService.Parameters(Parameters.P_DEPT,  (deptSearch??0)==0? (object)DBNull.Value:deptSearch.Value),
              new DBService.Parameters(Parameters.P_GRH_NO, admissionNoSearch == string.Empty ? (object)DBNull.Value :admissionNoSearch),
              new DBService.Parameters(Parameters.P_POH_NO, poNoSearch == string.Empty ? (object)DBNull.Value :poNoSearch),
              new DBService.Parameters(Parameters.P_VENDOR,  (vendorSearch??0)==0? (object)DBNull.Value:vendorSearch.Value),
              new DBService.Parameters(Parameters.P_GRH_COMPANY,  cmpPk>0 ? cmpPk:(object)DBNull.Value)

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_GET_LIST, colParameters);
        }

        public static DataSet GetStockTransferListByType(GridPrams grid, User objUser, int procID, string PageUrl, int? deptSearch, string admissionNoSearch, string poNoSearch, int? vendorSearch, int cmpPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(Parameters.P_SER_NAME , grid.SearchBy ==Fields.STRINGEMPTY ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(Parameters.P_SER_VAL , grid.SearchValue == Fields.STRINGEMPTY ? (object)DBNull.Value : Fields.VALUE_PERC + grid.SearchValue + Fields.VALUE_PERC),
              new DBService.Parameters(Parameters.P_PAGE_NO ,  grid.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  grid.PageSize),
              new DBService.Parameters(Parameters.P_FIELDS, grid.Fields == Fields.STRINGEMPTY ? Fields.VALUE_STAR : grid.Fields),
              new DBService.Parameters(Parameters.P_SORT_BY,  grid.SortBy == null ||grid.SortBy ==GTIService.Constants.DirectStockTransfer.Fields.GRH_DATE|| grid.SortBy == GTIService.Constants.DirectStockTransfer.Fields.GRH_NO ?GTIService.Constants.DirectStockTransfer.Fields.GRH_PK : grid.SortBy),
              new DBService.Parameters(Parameters.P_SORT_DIR , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(Parameters.P_FROM_DT, grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              new DBService.Parameters(Parameters.P_USER_PK ,  objUser.PKUser),
              new DBService.Parameters(Parameters.P_BIZUNIT, objUser.SBUID),

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS ,  string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),
              new DBService.Parameters(Parameters.P_DEPT,  (deptSearch??0)==0? (object)DBNull.Value:deptSearch.Value),
              new DBService.Parameters(Parameters.P_GRH_NO, admissionNoSearch == string.Empty ? (object)DBNull.Value :admissionNoSearch),
              new DBService.Parameters(Parameters.P_POH_NO, poNoSearch == string.Empty ? (object)DBNull.Value :poNoSearch),
              new DBService.Parameters(Parameters.P_VENDOR,  (vendorSearch??0)==0? (object)DBNull.Value:vendorSearch.Value),
              new DBService.Parameters(Parameters.P_GRH_COMPANY,  cmpPk>0 ? cmpPk:(object)DBNull.Value)

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_WORK_ORDER_DIR_GET_LIST, colParameters);
        }

        /// <summary>
        /// Method to Save StockTransfer Details
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static string GetDirectStockAdmissionByPk(int grhPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_GRH_PK , grhPk)
            };
            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_GET, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
            //return DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_GET, colParameters).Tables[0];
        }

        /// <summary>
        /// Method to Delete StockTransfer Details
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteStockTransfer(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_GRH_PK , pk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// <summary>
        /// Methode used to get the purhase order vendor 
        /// </summary>
        /// <param name="bizUnitPk"></param>
        /// <returns></returns>
        public static DataTable GetPurchaseOrderVendors(int bizUnitPk, int grhPK,int Role=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPk),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.GRNPK, grhPK == 0 ? (object)DBNull.Value : grhPK),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.P_VRM_ROLE, Role == 0 ? (object)DBNull.Value : Role)
            };
            DataTable dtVendor = new DataTable();
            dtVendor = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPPUR_VENDOR_DSA_GET_KV, colParameters).Tables[0];
            return dtVendor;
        }
        public static DataSet GetDirectStockAdmissionReport(int RccPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsReceiptNote.GOODRECEIPTPK , RccPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_RPT, colParameters);
        }


        public static DataSet GetDirectStockAdmissionReportDOCNOREVISION(int RccPK, int reportpk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsReceiptNote.GOODRECEIPTPK , RccPK),
                 new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsReceiptNote.P_AST_PK , reportpk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_RPT, colParameters);
        }
        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="vendorPK"></param>
        /// <param name="grnPK"></param>
        /// <param name="objUser"></param>
        /// <param name="deptPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetPendingDirectGRNSearchAuto(string searchBy, string searchValue, int vendorPK, int grnPK, User objUser, int deptPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(Parameters.P_FLD_NAME, searchBy),
              new DBService.Parameters(Parameters.P_VALUE, searchValue),
              new DBService.Parameters(Parameters.P_VENDOR, vendorPK),
              new DBService.Parameters(Parameters.P_GRH_PK, grnPK),
              new DBService.Parameters(Parameters.P_BIZUNIT,objUser.SBUID),
              new DBService.Parameters(Parameters.P_DEPT, deptPk),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPPUR_ORDER_DIR_ITEMS_AUTO, colParameters).Tables[0];
        }

        public static DataTable GetPendingWOGRNSearchAuto(string searchBy, string searchValue, int vendorPK, int grnPK, User objUser, int deptPk,int PendingWo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(Parameters.P_FLD_NAME, searchBy),
              new DBService.Parameters(Parameters.P_VALUE, searchValue),
              new DBService.Parameters(Parameters.P_VENDOR, vendorPK),
              new DBService.Parameters(Parameters.P_GRH_PK, grnPK),
              new DBService.Parameters(Parameters.P_BIZUNIT,objUser.SBUID),
              new DBService.Parameters(Parameters.P_DEPT, deptPk),
              new DBService.Parameters(Parameters.P_PENDING_WO, PendingWo),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_WORK_ORDER_ITEM_DIR_AUTO, colParameters).Tables[0];
        }
        /// <summary>
        /// Method to get Auto Complete Search for Different Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetDetailsForAutoSearch(string searchBy, string searchValue, User objUser,int? MenuType=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(Parameters.P_FLD_NAME  ,  searchBy),
              new DBService.Parameters(Parameters.P_VALUE  ,  searchValue),
              new DBService.Parameters(Parameters.P_BIZUNIT  ,  objUser.SBUID),
              new DBService.Parameters(Parameters.P_MENUTYPE  ,  MenuType)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_AUTO, colParameters).Tables[0];
        }
        /// <summary>
        ///Validation For Cancellation of Direct GRN cancel 
        /// </summary>
        /// <param name="CurrPK"></param>    
        /// <returns></returns>
        public static bool ValidationForCancellationDSA(int CurrPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_GRH_PK, CurrPK > 0 ? CurrPK : (object)DBNull.Value)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_CANCEL_CHECK, colParameters);
            return dsArchive == null || dsArchive.Tables.Count == 0 || dsArchive.Tables[0].Rows.Count == 0;
        }

        public static List<object> SaveStockTransferWkf(string strXml, out string strTrxNumber)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML , strXml),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(Parameters.P_RET_NO, string.Empty, 4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            string admissionNo = ((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_NO]).Value.ToString();
            strTrxNumber = admissionNo;
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(admissionNo);
            return retvals;
        }

        public static DataSet GetGRNForConvert(int grhPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_GRH_PK , grhPk)
            };
            DataSet dsResult = new DataSet();
            dsResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_CONVERT_ITEM_GET, colParameters);
            return dsResult;
        }

        public static int ConvertGRN(string strXml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_GRH_XML , strXml),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_GRN_ITEM_CONVERSION_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static int DeleteGRNConversion(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_GRH_PK , pk),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_GRN_ITEM_CONVERSION_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static string GetWOBOMForReturn(string strXml, int supplierPK, int MaterialRetturn = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_WIH_PK, strXml),
                new DBService.Parameters(Parameters.P_VENDOR, supplierPK > 0 ? supplierPK : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_MAT_RET, MaterialRetturn > 0 ? MaterialRetturn : (object)DBNull.Value)
            };
            DataTable dtXml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_WORK_ORDER_MATERIAL_GET, colParameters).Tables[0];
            string retStr = "";
            for (int i = 0; i < dtXml.Rows.Count; i++)
                retStr += dtXml.Rows[i][0].ToString();
            return retStr;
        }
    }
}
