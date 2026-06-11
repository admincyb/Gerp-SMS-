using System;
using System.Data;
using BusinessObject;

namespace DataAccess.StoreManagement
{
    /// <summary>
    /// This class is used to communicate with data access layer.
    /// </summary>
    public class StoreMasterDL
    {
        #region Methods

        /// <summary>
        /// SAVING STORE DETAILS
        /// </summary>
        /// <param name="store"></param>
        /// <returns> INT</returns>
        public static string SaveStore(BusinessObject.StoreManagement.StoreMaster store)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
              
                new DBService.Parameters(GTIService.Constants.Store.Parameters.STOREPK,   store.StoreMasterID == 0 ? (object)DBNull.Value: store.StoreMasterID) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters.STORETYPEPK ,   store.StoreTypeID) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters.STORENAME,   store.StoreName) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters.STORESTATUS,1) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters.STOREMODIFIED,   store.UserPk) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters.STORERETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures.SAVESTOREDETAILS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters.STORERETURNVALUE]).Value).ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>
        public static DataTable GetInventoryStores(int sbuPk, int deptType, int userPK, int deptCompany = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("P_ACTIVE",1),
              new DBService.Parameters("DPT_CATEGORY", deptType),
              new DBService.Parameters("P_USER", userPK == 0 ? (object)DBNull.Value : userPK), 
              new DBService.Parameters("P_BIZUNIT", sbuPk),
              new DBService.Parameters("P_DPT_COMPANY" ,deptCompany==0?(object)DBNull.Value:deptCompany),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_DEPT_STORE_GET_KV", colParameters).Tables[0];
        }
        /// <summary>
        /// Methord to get the Search Values Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Store.Parameters.STOREID, searchBy),
              new DBService.Parameters( GTIService.Constants.Store.Parameters.STORESRCH , searchValue),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures.GETSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Get Inventory Location
        /// </summary>
        /// <param name="DeptPK"></param>
        /// <param name="Type"></param>
        /// <param name="Category"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static DataTable GetInventoryLocation(string DeptPK, string Type, string Category, string searchValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Store.Parametes_Department.P_DPT_PARENT, DeptPK),
              new DBService.Parameters( GTIService.Constants.Store.Parametes_Department.P_DPT_TYPE , Type),
              new DBService.Parameters( GTIService.Constants.Store.Parametes_Department.P_DPT_CATEGORY , Category),
              new DBService.Parameters( GTIService.Constants.Store.Parametes_Department.P_DPT_NAME , searchValue),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures.SPADM_DEPT_MST_GET_KV, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// Function Used To Get all storetype and store pk
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetStoreTypes()
        {
            DBService dbService = new DBService();


            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures.GETSTORETYPE).Tables[0];
        }
        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetStoreList(GridPrams grid)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters.STOREID , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters.STORESRCH , grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
             
              new DBService.Parameters(GTIService.Constants.Common.Parameters.PGNUMR,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters.PGSIZ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters.SORT,  grid.SortBy == null ? (object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters.SORTDRTION, grid.SortDirection == null ? "asc" : grid.SortDirection),
            };

            DataSet dtStore = new DataSet();
            dtStore = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures.GETSTORELISTBYSRH, colParameters);
            return dtStore;


        }
        /// <summary>
        /// Delete Store Details By StoreID
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns>int- 1(Success)</returns>
        public static int DeleteStoreDtls(int storeID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters.STOREPK,  storeID == 0 ? (object)DBNull.Value :  storeID),
                new DBService.Parameters(GTIService.Constants.Store.Parameters.STORERETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures.DELETESTOREDETAILS, colParameters);

            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters.STORERETURNVALUE]).Value);



        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetStockDetails(int store, int item)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters.STORE,  store == 0 ? (object)DBNull.Value :  store),
                new DBService.Parameters(GTIService.Constants.Store.Parameters.ITEM,  item == 0 ? (object)DBNull.Value :  item),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures.GETSTOCKDETAILS, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbu"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        public static DataTable GetCumilativeStockDetails(int sbu, int item, string fromDate, string toDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.BIZUNIT,  sbu),
                new DBService.Parameters(GTIService.Constants.Store.Parameters.ITEM,  item == 0 ? (object)DBNull.Value :  item),
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.P_SEARCH_FROM_DATE,  fromDate),
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.P_SEARCH_TO_DATE,  toDate)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures.GETCUMILATIVESTOCKDETAILS, colParameters).Tables[0];
        }
    }
        #endregion
}
