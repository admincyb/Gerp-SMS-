using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.Administration.Masters
{
    public class StoreLocationMaster
    {
        #region Methods
        /// <summary>
        /// Get Department Details 
        /// </summary>
        /// <param name="mainStoreID"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public static string GetProductionDepartmentDtls(int BizUnit)
        {
            DataTable dtSearch = DataAccess.Administration.Masters.StoreLocationMasterDL.GetProductionDepartmentDtls(BizUnit);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK).ToString();

        }

        /// <summary>
        /// Save Sub Department Details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>string</returns>
        public static string SaveStoreLocationDtls(string requestData)
        {
            BusinessObject.Administration.Masters.StoreLocationMaster subDept = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Administration.Masters.StoreLocationMaster>(requestData);
            subDept.Status = 1;
            return DataAccess.Administration.Masters.StoreLocationMasterDL.SaveStoreLocationDtls(subDept);
        }

        /// <summary>
        /// Get StoreLocation List Details
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetStoreLocationList(GridPrams grid, int bizUnit, int deptCategory)
        {
            DataSet dsDesigList = DataAccess.Administration.Masters.StoreLocationMasterDL.GetStoreLocationList(grid, bizUnit, deptCategory);
            string jString = string.Empty;
            if (dsDesigList.Tables.Count > 1 && dsDesigList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsDesigList);
            }
            return jString;
        }

        /// <summary>
        /// Get StoreLocation Details for PrintLabel
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static DataTable GetStoreLocationPrintLabelList(string searchName, string searchValue, int bizUnit, int deptCategory)
        {
            DataTable dtDesigList = DataAccess.Administration.Masters.StoreLocationMasterDL.GetStoreLocationPrintLabelList(searchName, searchValue, bizUnit, deptCategory);           
            return dtDesigList;
        }

        /// <summary>
        /// Delete Sub Dept Details by storeLocID
        /// </summary>
        /// <param name="storeLocID"></param>
        /// <returns>string</returns>
        public static string DeleteStoreLOcation(int storeLocID)
        {
            return DataAccess.Administration.Masters.StoreLocationMasterDL.DeleteStoreLocationDtls(storeLocID).ToString();
        }

        /// <summary>
        /// Get Sub Department Details By SubDept ID 
        /// </summary>
        /// <param name="storeLocID"></param>
        /// <returns>string</returns>
        public static string GetStoreLocDetailsByID(int storeLocID)
        {
            DataSet dsDesigList = DataAccess.Administration.Masters.StoreLocationMasterDL.GetStoreLocDtlsByID(storeLocID);
            string jString = string.Empty;
            jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsDesigList);
            return jString;
        }

        /// <summary>
        /// Get AutoCompleted value For Search
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue, int bizUnit)
        {
            DataTable dtSearch = DataAccess.Administration.Masters.StoreLocationMasterDL.GetSearchValues(searchBy, searchValue, bizUnit);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);

        }
        #endregion
    }
       
}
