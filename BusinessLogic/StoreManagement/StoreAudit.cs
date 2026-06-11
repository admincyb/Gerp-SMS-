using System.Collections.Generic;
using System.Data;
using BusinessObject;

namespace BusinessLogic.StoreManagement
{
    public class StoreAudit
    {
        /// <summary>
        /// Method to get Store Audit Details
        /// </summary>
        /// <param name="storeAuditID"></param>
        /// <returns></returns>
        public static string GetStoreAuditDetails(int storeAuditID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.StoreAuditDL.GetStoreAuditDetails(storeAuditID));
        }
        /// <summary>
        /// Method to get Store Audit Details For Report
        /// </summary>
        /// <param name="storeAuditID"></param>
        /// <returns></returns>
        public static DataSet GetStoreAuditReportDetails(int storeAuditID)
        {
            return DataAccess.StoreManagement.StoreAuditDL.GetStoreAuditReportDetails(storeAuditID);
        }
        /// <summary>
        /// Save Store Audit Details
        /// </summary>
        /// <param name="storeAuditDetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SaveStoreAuditDetails(string storeAuditDetails, User objUser)
        {
            string saID = string.Empty;
            List<object> retvals = new List<object>();
            string sANumber = string.Empty;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(storeAuditDetails);
            retvals = DataAccess.StoreManagement.StoreAuditDL.SaveStoreAuditDetails(xmlstr);
            //saID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }
        
      
        /// <summary>
        /// Get Store Audit Number
        /// </summary>
        /// <returns></returns>
       
        public static string GetStoreAuditNO()
        {
            return DataAccess.StoreManagement.StoreAuditDL.GetStoreAuditNo();
        }

        /// <summary>
        /// Get Sore Item Details
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="dept"></param>
        /// <param name="itemPK"></param>
        /// <returns></returns>
        public static string GetItemDetails(int dept, int itemPK)
        {
            DataTable dsPucReqList = DataAccess.StoreManagement.StoreAuditDL.GetItemDetails( dept, itemPK);
            string jString = string.Empty;
            if (dsPucReqList.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPucReqList);
            }
            return jString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dept"></param>
        /// <param name="categoryPK"></param>
        /// <param name="itemPK"></param>
        /// <param name="batchPK"></param>
        /// <returns></returns>
        public static string GetCategoryItemDetails(int dept, int categoryPK, int itemPK, int batchPK, int StkbatchPK=0)
        {
            DataTable dsPucReqList = DataAccess.StoreManagement.StoreAuditDL.GetCategoryItemDetails(dept, categoryPK, itemPK, batchPK, StkbatchPK);
            string jString = string.Empty;
            if (dsPucReqList.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPucReqList);
            }
            return jString;
        }

        /// <summary>
        /// Method to get Items
        /// </summary>
        /// <returns></returns>
        public static string GetItems(int bizUnit, int store)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.StoreManagement.StoreAuditDL.GetItems( bizUnit,  store), GTIService.Constants.Common.Fields.DEPTNAME, GTIService.Constants.Common.Fields.DEPTID);
        }
        /// <summary>
        /// Method to get Store
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetStore(int bizUnit)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.StoreManagement.StoreAuditDL.GetStore(bizUnit), GTIService.Constants.Common.Fields.DEPTNAME, GTIService.Constants.Common.Fields.DEPTID);
        }


        /// <summary>
        /// Methos th get Damage Types
        /// </summary>
        /// <returns></returns>
        public static string GetDamageTypes(int bizUnit)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.StoreManagement.StoreAuditDL.GetDamageTypes(bizUnit), GTIService.Constants.Common.Fields.DAMAGETYPE, GTIService.Constants.Common.Fields.DAMAGETYPEPK);
        }

    }
}
