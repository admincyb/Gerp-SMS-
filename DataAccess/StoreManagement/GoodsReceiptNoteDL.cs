using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.StoreManagement
{
    public class GoodsReceiptNoteDL
    {
        /// <summary>
        /// Function Used To get Goods Receipt Note xml details based on the id
        /// </summary>
        /// <param name="purchaseRequestID"></param>
        /// <returns></returns>
        public static string GetGoodsReceiptNoteDetails(int grnID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsReceiptNote.GOODRECEIPTPK , grnID)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsReceiptNote.GETGOODRECEIPT, colParameters).ToString();
        }

        /// <summary>
        /// Function Used To save Goods Receipt Note
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static List<object> SaveGoodsReceiptNote(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsReceiptNote.GOODSRECEIPTXML , xmlstr),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsReceiptNote.GRNNO , string.Empty, 4000, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsReceiptNote.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsReceiptNote.SAVEGOODRECEIPT, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_GoodsReceiptNote.RETVAL]).Value.ToString();
            string grnNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_GoodsReceiptNote.GRNNO]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(grnNo);
            return retvals;
        }

        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetPendingSearchAuto(string searchBy, string searchValue, int vendorPK, int grnPK, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.SEARCHBY , searchBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.SEARCHVALUE , searchValue),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.SEARCHCORR , vendorPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.GRNPK , grnPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.BIZUNIT,objUser.SBUID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsReceiptNote.GETPENDINGSEARCH, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataSet GetPurchaseOrderPending(GridPrams grid, int sbuID, int vendor, int grnID, int storeID, int pohPK=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsReceiptNote.GRNPK , grnID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "POH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.BIZUNIT, sbuID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.VENDOR, vendor),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.DEPARTMENT, storeID==0 ? (object)DBNull.Value: storeID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.P_POH_PK, pohPK==0 ? (object)DBNull.Value: pohPK),
              
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsReceiptNote.GETPENDINGPO, colParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataSet GetPreviousGRNDetailsView(int poID, int sbuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.BIZUNIT, sbuID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.POPK, poID),
            };

            DataSet dtProduct = new DataSet();
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsReceiptNote.GETPREVIOUSGRN, colParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataSet GetGoodsReceiptNoteList(GridPrams grid, User objUser, int procID, string pageUrl, string grnNo, string poNo, int venPK, string refNo, int filterStatus,int cmpPk=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "GRH_NO" : grid.SearchBy ),
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.SEARCHVAL , grid.SearchValue == "" ? "%" : "%" + grid.SearchValue + "%"),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.SORTBY ,  grid.SortBy == null ? "GRH_DATE" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.PROCESSID , procID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.USERPK ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS , filterStatus<0 ? (object)DBNull.Value : filterStatus),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  pageUrl==string.Empty ?(object)DBNull.Value:pageUrl),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.P_GRH_NO , grnNo==string.Empty?(object)DBNull.Value: grnNo),      
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.P_POH_NO , poNo==string.Empty?(object)DBNull.Value:poNo ), 
               new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.P_VEN_PK , venPK>0 ? venPK:(object)DBNull.Value), 
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.P_VEN_REF_NO , refNo==string.Empty?(object)DBNull.Value:refNo ), 
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.P_GRH_COMPANY , cmpPk>0 ? cmpPk:(object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsReceiptNote.GETGRNLIST, colParameters);
        }

        /// <summary>
        /// Function Used To get GRN no sequece
        /// </summary>
        /// <returns></returns>
        public static string GetGRN()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.GOODRECEIPTPK , 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsReceiptNote.GETGRNNO, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_GoodsReceiptNote.GOODRECEIPTPK]).Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetGRNSearchValue(string searchBy, string searchValue, User objUser, string pageUrl)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY  ,  searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE   ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK   ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  pageUrl==string.Empty ?(object)DBNull.Value:pageUrl)

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsReceiptNote.GRNTAUTO, colParameters).Tables[0];
        }
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grnPk"></param>
        /// <returns></returns>
        public static int DeleteGoodsReceiptNote(int grnPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsReceiptNote.GOODRECEIPTPK ,  grnPk == 0 ? (object)DBNull.Value :  grnPk),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsReceiptNote.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsReceiptNote.DELETEGRN, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_GoodsReceiptNote.RETVAL]).Value);

        }
        /// <summary>
        /// Get GIN Details
        /// </summary>
        /// <param name="ginID"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>

        public static DataSet GetGRNDetailsView(int ginID, int sbuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
                new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.GOODRECEIPTPK , ginID),      
                new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.BIZUNIT , sbuID), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsReceiptNote.GETGRNDTLSVIEW, colParameters);
        }

        public static DataSet GetGRNDetailsForNewReport(int grnID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsReceiptNote.GOODRECEIPTPK , grnID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsReceiptNote.SPINV_GRN_RPT, colParameters);
        }
    }

}
