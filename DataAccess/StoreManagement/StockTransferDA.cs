using System;
using System.Collections.Generic;
using System.Data;
using BusinessObject;
using GTIService.Constants.StockTransfer;

namespace DataAccess.StoreManagement
{
    public class StockTransferDA
    {
        #region Method

        #region  ==================================== Entry Section ======================================
        /// <summary>
        /// Method to get next Store Transfer Pk, to Set StoreTransfer Number Format Creation
        /// </summary>
        /// <returns></returns>
        public static string GetStockTransferNumber()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.STORE_TRANSFER_PK , 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.GET_STOCK_TRANSFER_NO, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[Parameters.STORE_TRANSFER_PK]).Value);
        }
        /// <summary>
        /// Method to get GRN already  inspected details
        /// </summary>
        /// <param name="grnDtlPk"></param>
        /// <returns></returns>
        public static DataSet GetGINDetails(int grnDtlPk, int status, int sbu, int store, int ginPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.GIN_DTL_PK , grnDtlPk==0?(object)DBNull.Value:grnDtlPk),
                new DBService.Parameters(Parameters.GIN_STATUS , status==0?(object)DBNull.Value:status),
                new DBService.Parameters(Parameters.GIN_DEPT , store==0?(object)DBNull.Value:store),
                new DBService.Parameters(Parameters.GIN_PK , ginPk==0?(object)DBNull.Value:ginPk),
                new DBService.Parameters(Parameters.SBU , sbu)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_GIN_DTLS, colParameters);
        }

        /// <summary>
        /// Get MaterialName By material category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <param name="itemID"></param>
        /// <param name="sbuPk"></param>
        /// <returns>int- 1(Success)</returns>
        public static DataTable GetMaterialByCategoryAndStore(int categoryID, int itemID, int sbuPk, int type, int userPK, int store)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMCATEGORYID , categoryID==0?(object)DBNull.Value:categoryID) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMPK , itemID) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.TYPE ,type ==0?(object)DBNull.Value:type) ,
                 new DBService.Parameters( GTIService.Constants.Material.Parameters.STORE ,store ==0?(object)DBNull.Value:store) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  userPK)
                
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALBYCATEGORYANDSTORE, colParameters).Tables[0];

        }

        /// <summary>
        /// Get Stock Trasnfer details , for Click Add Selected Item To list, get Details - Transfer Details and Allocated Details
        /// </summary>
        /// <param name="grnID"></param>
        /// <returns>string</returns>
        public static DataTable GetPRPODetails(int sbu, int type, string xmlPkDtls, int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.SBU,sbu),
                new DBService.Parameters(Parameters.IS_PO , type ),
                new DBService.Parameters(Parameters.XML_VALUES ,xmlPkDtls)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_PO_PR_DTLS, colParameters).Tables[0];
        }
        /// <summary>
        /// Get Allocated Store Details
        /// </summary>
        /// <param name="storeTransferPk"></param>
        /// <param name="selectedGINDtls"></param>
        /// <returns></returns>
        public static DataTable GetAllocatedItems(int sbu, int type, int itemList, string xmlPkDtls)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.SBU,sbu),
                new DBService.Parameters(Parameters.IS_PO,type),
                new DBService.Parameters(Parameters.ITEM_LIST , itemList),
                new DBService.Parameters(Parameters.XML_VALUES , xmlPkDtls)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_ALLOCATED_ITEMS, colParameters).Tables[0];
        }
        /// <summary>
        /// Method to Save StockTransfer Details
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static List<object> SaveStockTransferDetails(string xmlstr, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.XML_VALUES , xmlstr),
                new DBService.Parameters(Parameters.LAST_MOD_DATE , lastModDate==string.Empty?(object)DBNull.Value: lastModDate),
                new DBService.Parameters(Parameters.RETURN_NO, string.Empty, 4000, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(Parameters.RETURN_VALUE , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SAVE_STOCK_TRANSFER_DTLS, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[Parameters.RETURN_VALUE]).Value.ToString();
            string ginNo = ((IDataParameter)dbService.oCommand.Parameters[Parameters.RETURN_NO]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(ginNo);
            return retvals;
        }
        /// <summary>
        /// Method to Get Stock Transfer details
        /// </summary>
        /// <param name="sbu"></param>
        /// <param name="type"></param>
        /// <param name="xmlPkDtls"></param>
        /// <returns></returns>
        public static DataTable GetStockTransferDetails(int sbu, int type, string xmlPkDtls)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.SBU,sbu),
                new DBService.Parameters(Parameters.IS_PO , type ),
                new DBService.Parameters(Parameters.XML_VALUES ,xmlPkDtls)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_STOCK_TRANSFER_DETAILS, colParameters).Tables[0];
        }
        /// <summary>
        /// Method to Get Material UOM by selected UOM
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataTable GetMaterialUOMDetails(int itemID, int status)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.ITEM_PK,itemID) ,
                new DBService.Parameters(Parameters.ITEM_ACTIVE, status > 0 ? status : (object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_MATERIAL_UOM_DTLS, colParameters).Tables[0];

        }
        /// <summary>
        /// Method to get Stock Transfer Details in Edit Mode
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static DataTable GetStockTransferDetails(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.STOCK_TRANSFER_PK,pk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_STOCK_TRANSFER_DTLS, colParameters).Tables[0];
        }
        #endregion

        #region  ====================================   Listing Section ======================================
        /// <summary>
        /// Method to delete Stock Transfer Details
        /// </summary>
        /// <param name="stockTransferPk"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static List<object> DeleteStockTransferDetails(int stockTransferPk, string lastModDate)
        {          

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.STORE_TRANSFER_PK ,  stockTransferPk == 0 ? (object)DBNull.Value :  stockTransferPk),
                new DBService.Parameters(Parameters.LAST_MOD_DT ,  lastModDate==string.Empty?(object)DBNull.Value: Convert.ToDateTime(lastModDate) ),
                new DBService.Parameters(Parameters.RETURN_VALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(Parameters.P_RET_VAL_TEXT, string.Empty, 500, ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.DELETE_STOCK_TRANSFER_DELETE, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[Parameters.RETURN_VALUE]).Value.ToString();
            string referenceNo = ((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL_TEXT]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(referenceNo);
            return retvals;

        }
        /// <summary>
        /// Method to get Auto Complete Search for Different Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetDetailsForAutoSearch(string searchBy, string searchValue, User objUser, int procID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(Parameters.FIELD_NAME  ,  searchBy),  
              new DBService.Parameters(Parameters.FIELD_VALUE  ,  searchValue),
              new DBService.Parameters(Parameters.USER_PK  ,  objUser.PKUser),
              new DBService.Parameters(Parameters.SBU, objUser.SBUID), 
              new DBService.Parameters(Parameters.PROC_ID, procID),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_STOCK_TRANSFER_AUTO, colParameters).Tables[0];
        }
        /// <summary>
        /// Get Stock Transfer Details Get
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataSet GetStockTransferList(GridPrams grid, User objUser, int procID, string PageUrl, string saNo, string ginNo, string grnNo, string poNo, int venPK, string depName, int filterStatus, int cmpPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(Parameters.SEARCH_NAME , grid.SearchBy ==Fields.VALUE_ZERO|| grid.SearchBy == GTIService.Constants.StockTransfer.Fields.Date ? GTIService.Constants.StockTransfer.Fields.StockTransferNo : grid.SearchBy ),
              new DBService.Parameters(Parameters.SERCH_VALUE , grid.SearchValue == Fields.STRINGEMPTY ? Fields.VALUE_PERC : Fields.VALUE_PERC + grid.SearchValue + Fields.VALUE_PERC),
              new DBService.Parameters(Parameters.FIELDS, grid.Fields == Fields.STRINGEMPTY ? Fields.VALUE_STAR : grid.Fields),
              new DBService.Parameters(Parameters.PAGE_NO ,  grid.PageNumber),
              new DBService.Parameters(Parameters.PAGE_SIZE,  grid.PageSize),
              new DBService.Parameters(Parameters.SORT_BY,  grid.SortBy == null ||grid.SortBy ==GTIService.Constants.StockTransfer.Fields.StockTransferDate|| grid.SortBy == GTIService.Constants.StockTransfer.Fields.StockTransferNo ?GTIService.Constants.StockTransfer.Fields.StockTransferPK : grid.SortBy),
              new DBService.Parameters(Parameters.SORT_DIR , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(Parameters.PROC_ID , procID),
              new DBService.Parameters(Parameters.USER_PK ,  objUser.PKUser),
              new DBService.Parameters(Parameters.SBU, objUser.SBUID),
              new DBService.Parameters(Parameters.FROM_DT, grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(Parameters.TO_DT, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS ,   filterStatus<0 ? (object)DBNull.Value : filterStatus),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              new DBService.Parameters(Parameters.P_SFH_NO , saNo==string.Empty?(object)DBNull.Value: saNo),
              new DBService.Parameters(Parameters.P_GIN_NO , ginNo==string.Empty?(object)DBNull.Value: ginNo),
              new DBService.Parameters(Parameters.P_GRN_NO , grnNo==string.Empty?(object)DBNull.Value: grnNo),      
              new DBService.Parameters(Parameters.P_POD_NO , poNo==string.Empty?(object)DBNull.Value:poNo ), 
              new DBService.Parameters(Parameters.P_VEN_PK , venPK>0 ? venPK:(object)DBNull.Value), 
              new DBService.Parameters(Parameters.P_DPT_NAME , depName==string.Empty?(object)DBNull.Value:depName ), 
              new DBService.Parameters(Parameters.P_SFH_COMPANY , cmpPk>0 ? cmpPk:(object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_STOCK_TRANSFER_LIST, colParameters);
        }
        #endregion

        #endregion


        public static DataTable GetStockTransferRptDetails(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.STOCK_TRANSFER_PK,pk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_STOCK_TRANSFER_RPT_DTLS, colParameters).Tables[0];
        }
    }
}
