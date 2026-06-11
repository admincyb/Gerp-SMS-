using System.Data;
using BusinessObject;

using DataAccess.StoreManagement;
using System.Collections.Generic;

namespace BusinessLogic.StoreManagement
{
    /// <summary>
    ///This class is used to communicate with Dataaccess layer.
    /// </summary>
    public class NewItemRequestBL
   {

       #region Methods
       /// <summary>
       /// Saving New ItemRequest details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string SaveNewItemRequest(string requestData)
        {
            
            List<object> retvals = new List<object>();
            BusinessObject.StoreManagement.NewItemRequestBO newItem = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.StoreManagement.NewItemRequestBO>(requestData);
            retvals= DataAccess.StoreManagement.NewItemRequestDL.SaveNewItemRequest(newItem);
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }
        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue, int sbuPk, User objUser, int procId)
        {
            DataTable dtSearch = NewItemRequestDL.GetSearchValues(searchBy, searchValue, sbuPk, objUser, procId);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Store.Fields_NewItemRequest.SEARCHTEXTFIELD, GTIService.Constants.Store.Fields_NewItemRequest.SEARCHVALUEFIELD);
             
        }
        /// <summary>
        /// Returns NIR No in json string format
        /// </summary>
        /// <param name=""></param>
        /// <returns>String</returns>
        public static string GetNIRNo()
        {
            return DataAccess.StoreManagement.NewItemRequestDL.GetNIRNo();

        }
        /// <summary>
        /// Returns New Item Request list in json string format
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetNewItemRequestList(GridPrams grid, int bizUnit, User objUser, int procID)
        {
            DataSet dsStoreList = NewItemRequestDL.GetNewItemRequestList(grid, bizUnit, objUser, procID);
            string jString = string.Empty;
            if (dsStoreList.Tables.Count > 1 && dsStoreList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsStoreList);
            }
            return jString;
        }
        /// <summary>
        /// Returns New Item Request list in json string format
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetNewItemCheckList(int bizUnit,string itemName)
        {
            DataSet dsList = NewItemRequestDL.GetNewItemCheckList(bizUnit, itemName);
            string jString = string.Empty;
            if (dsList.Tables.Count > 1 && dsList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsList);
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
        /// Delete New Item Request Details
        /// </summary>
        /// <param name="storeID"></param>
        /// <returns>String</returns>
        public static string DeleteNewItemRequestDtls(int storeID)
        {
            return NewItemRequestDL.DeleteNewItemRequestDtls(storeID).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetNewItemRequestDetails(int item, int status)
        {
            DataTable dtStoreList = NewItemRequestDL.GetNewItemRequestDetails(item, status);
            string jString = string.Empty;
            if (dtStoreList!=null&&dtStoreList.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtStoreList);
            }
            return jString;
           
        }
   }
       #endregion
}
