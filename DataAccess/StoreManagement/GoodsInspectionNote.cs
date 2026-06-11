using System;
using System.Collections.Generic;
using System.Data;
using BusinessObject;
using GTIService.Constants.Store;

namespace DataAccess.StoreManagement
{
    public class GoodsInspectionNote
    {
        # region Methods
        /// <summary>
        /// Get pending Grn Number Auto Complete Search
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetPendingGRNSearchAuto(string searchBy, string searchValue, int searchCorr, User objUser, int IsStockItem)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.SEARCHBY , searchBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.SEARCHVALUE , searchValue),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.BIZUNIT,objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.DEPTSTOREPK,searchCorr),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.ISSTK_ITEM,IsStockItem),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsInspectionNote.GETPENDINGSEARCH, colParameters).Tables[0];
        }
        /// <summary>
        /// Get Grn Store
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        public static DataTable GetGrnStore(int appID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.GRNPK , appID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsInspectionNote.SPINV_GRN_DTL_GET, colParameters).Tables[0];
        }

       /// <summary>
       /// get Gin Store
       /// </summary>
       /// <param name="appID"></param>
       /// <returns></returns>
        public static DataTable GetGinStore(int appID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.GINID , appID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsInspectionNote.SPINV_GIN_DTL_GET, colParameters).Tables[0];
        }

        /// <summary>
        /// Get GIN Number
        /// </summary>
        /// <returns></returns>
        public static string GetGINNumber()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.GOODINSPECTIONPK , 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsInspectionNote.GENERATEGINNO, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_GoodsInspectionNote.GOODINSPECTIONPK]).Value);
        }

        /// <summary>
        /// Get GRN Pending Items List To Inspection
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        public static DataSet GetGRNItemPending(GridPrams grid, int sbuID, int store, int ginPk, int grhPk,int IsStockItem)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "GRH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.BIZUNIT, sbuID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.DEPTSTOREPK, store),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.GINID, ginPk),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.P_GRH_PK,grhPk==0 ? (object)DBNull.Value : grhPk ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.ISSTK_ITEM,IsStockItem),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsInspectionNote.GETPENDINGGRNITEMDTLS, colParameters);
        }

        /// <summary>
        /// Save Gin Details
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static List<object> SaveGoodsInspectionNote(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsInspectionNote.GOODSINSPXML , xmlstr),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsInspectionNote.RETGINNUMBER , string.Empty, 4000, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsInspectionNote.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsInspectionNote.SAVEGINDETAILS, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_GoodsInspectionNote.RETVAL]).Value.ToString();
            string ginNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_GoodsInspectionNote.RETGINNUMBER]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(ginNo);
            return retvals;
        }

        /// <summary>
        /// Get Gin Details By GIN Pk
        /// </summary>
        /// <param name="ginID"></param>
        /// <returns></returns>
        public static string GetGoodsInspectionNoteDetails(int ginID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsInspectionNote.GOODINSPECTIONPK , ginID)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsInspectionNote.GETGINDTLS, colParameters).ToString();
        }

        /// <summary>
        /// Get AutoComplete Search Result For GIN
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetGINSearchValue(string searchBy, string searchValue, User objUser,string pageUrl)
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
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsInspectionNote.GINDTLSAUTO, colParameters).Tables[0];
        }

        /// <summary>
        /// Delete GIn Details By GIN PK
        /// </summary>
        /// <param name="ginPk"></param>
        /// <returns></returns>
        public static int DeleteGoodsInspectionNote(int ginPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsInspectionNote.GINPK ,  ginPk == 0 ? (object)DBNull.Value :  ginPk),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsInspectionNote.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsInspectionNote.DELETEGINDTLS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_GoodsInspectionNote.RETVAL]).Value);

        }

        /// <summary>
        /// Get GIN Details For Print
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataSet GetGoodsInspectionNoteTables(int ginPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_GoodsInspectionNote.GINID , ginPk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsInspectionNote.GETINV_GIN_RPT, colParameters);
        }

        /// <summary>
        /// Get GIN Details List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataSet GetGoodsInspectionNoteList(GridPrams grid, User objUser, int procID, string PageUrl, string ginNo, string grnNo, string poNo, int venPK, string depName, int filterStatus, int cmpPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.SEARCHNAME , grid.SearchBy == Constants.Value_Zero || grid.SearchBy ==Constants.Date ? Constants.GINNo : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.SEARCHVAL , grid.SearchValue == Constants.Value_Empty ? Constants.Value_Perc : Constants.Value_Perc + grid.SearchValue + Constants.Value_Perc),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.FIELDS , grid.Fields == Constants.Value_Empty ? Constants.Value_Star : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.PAGESIZE ,  grid.PageSize),
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.SORTBY ,  grid.SortBy == null ||grid.SortBy == Constants.GINDate|| grid.SortBy == Constants.GINNo ?Constants.GINPK : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.SORTBY ,  grid.SortBy== null? Constants.GINDate:grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.PROCESSID , procID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.USERPK ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS , filterStatus<0 ? (object)DBNull.Value : filterStatus),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.P_GIH_NO , ginNo==string.Empty?(object)DBNull.Value: ginNo),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.P_GRH_NO , grnNo==string.Empty?(object)DBNull.Value: grnNo),      
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.P_POH_NO , poNo==string.Empty?(object)DBNull.Value:poNo ), 
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.P_VEN_PK , venPK>0 ? venPK:(object)DBNull.Value), 
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.P_DEP_NAME , depName==string.Empty?(object)DBNull.Value:depName ), 
              new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsInspectionNote.P_GIH_COMPANY , cmpPk>0 ? cmpPk:(object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_GoodsInspectionNote.GINDETAILSLIST, colParameters);
        }

        /// <summary>
        /// Method to get GRN already  inspected details
        /// </summary>
        /// <param name="grnDtlPk"></param>
        /// <returns></returns>
        public static DataSet GetGINAlreadyInspectedDetails(int grnDtlPk, int status, int sbu,int store, int ginPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters_GoodsInspectionNote.P_GIN_DTL_PK , grnDtlPk==0?(object)DBNull.Value:grnDtlPk),
                new DBService.Parameters(Parameters_GoodsInspectionNote.P_GIN_STATUS , status==0?(object)DBNull.Value:status),
                new DBService.Parameters(Parameters_GoodsInspectionNote.P_GIN_DEPT , store==0?(object)DBNull.Value:store),
                new DBService.Parameters(Parameters_GoodsInspectionNote.GINID , ginPk==0?(object)DBNull.Value:ginPk),
                new DBService.Parameters(Parameters_GoodsInspectionNote.BIZUNIT , sbu)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure,Procedures_GoodsInspectionNote.GIN_ALREADY_INSPECTED_DTLS, colParameters);
        }

        # endregion
    }
}
