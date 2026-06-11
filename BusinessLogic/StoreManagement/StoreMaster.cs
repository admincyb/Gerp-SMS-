using System.Data;
using BusinessObject;

using DataAccess.StoreManagement;

namespace BusinessLogic.StoreManagement
{
    /// <summary>
    ///This class is used to communicate with Dataaccess layer.
    /// </summary>
   public class StoreMaster
   {

       #region Methods
       /// <summary>
        /// Saving store details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string SaveStore(string requestData)
        {
            BusinessObject.StoreManagement.StoreMaster store = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.StoreManagement.StoreMaster >(requestData);
            return DataAccess.StoreManagement.StoreMasterDL.SaveStore(store);
        }
        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue)
        {
            DataTable dtSearch = StoreMasterDL.GetSearchValues(searchBy, searchValue);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Store.Fields.STORESEARCHTEXTFIELD, GTIService.Constants.Store.Fields.STORESEARCHVALUEFIELD);
             
        }
       /// <summary>
        /// Get Inventory Stores
       /// </summary>
       /// <param name="sbuPk"></param>
       /// <param name="deptType"></param>
       /// <param name="userPK"></param>
       /// <param name="deptCompany"></param>
       /// <returns></returns>
        public static DataTable GetInventoryStores(int sbuPk, int deptType, int userPK, int deptCompany = 0)
        {
            return StoreMasterDL.GetInventoryStores(sbuPk, deptType, userPK, deptCompany);
        }

      /// <summary>
        /// Get Inventory Location
      /// </summary>
      /// <param name="DeptPK"></param>
      /// <param name="Type"></param>
      /// <param name="Category"></param>
      /// <param name="searchValue"></param>
      /// <returns></returns>
        public static string GetInventoryLocation(string DeptPK,string Type,string Category, string searchValue)
        {
            DataTable dtSearch = StoreMasterDL.GetInventoryLocation(DeptPK, Type, Category, searchValue);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Store.Fields.DPT_NAME, GTIService.Constants.Store.Fields.DPT_PK);

        }
        public static DataTable GetInventoryLocationList(string DeptPK, string Type, string Category, string searchValue)
        {
            DataTable dtSearch = StoreMasterDL.GetInventoryLocation(DeptPK, Type, Category, searchValue);
            return dtSearch;

        }

        /// <summary>
        /// Returns Store list in json string format
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetStoreList(GridPrams grid)
        {
            DataSet dsStoreList = StoreMasterDL.GetStoreList(grid);
            string jString = string.Empty;
            if (dsStoreList.Tables.Count > 1 && dsStoreList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsStoreList);
            }
            return jString;
        }
        /// <summary>
        /// Returns Store type and store type pk in json string format
        /// </summary>
        /// <param name=""></param>
        /// <returns>String</returns>
        public static string GetStoreTypes()
        {
            DataTable dtStoreType = StoreMasterDL.GetStoreTypes();
            string jString = string.Empty;
            if (dtStoreType.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtStoreType, GTIService.Constants.Store.Fields.STORENAME, GTIService.Constants.Store.Fields.STOREPK);
            }
            return jString;
        }
        /// <summary>
        /// Delete Store Details
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns>String</returns>
        public static string DeleteStore(int storeID)
        {
            return StoreMasterDL.DeleteStoreDtls(storeID).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetStockDetails(int store, int item)
        {
            return StoreMasterDL.GetStockDetails(store, item);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="sbu"></param>
       /// <param name="store"></param>
       /// <returns></returns>
        public static DataTable GetCumilativeStockDetails(int sbu, int item, string fromDate, string toDate)
        {
            return StoreMasterDL.GetCumilativeStockDetails(sbu, item, fromDate, toDate);
        }
   }
       #endregion
}
