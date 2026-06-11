using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

using GTIService.Constants;

namespace BusinessLogic.DesignationManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class DesignationMaster
    {

        #region Methods

        /// <summary>
        /// Get Designation List For Paging
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetDesignationList(GridPrams grid, int bizUnit)
        {
            DataSet dsDesigList = DataAccess.DesignationManagement.DesignationMasterDL.GetDesignationList(grid, bizUnit);
            string jString = string.Empty;
            if (dsDesigList.Tables.Count > 1 && dsDesigList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsDesigList);
            }
            return jString;
        }
            
        /// <summary>
       /// Get Search value For AutoComplete
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue, int bizUnit)
        {
            DataTable dtSearch = DataAccess.DesignationManagement.DesignationMasterDL.GetSearchValues(searchBy, searchValue, bizUnit);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);

        }

        /// <summary>
        /// Save Designation Details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>string</returns>
        public static string SaveDesignation(string requestData)
        {
            BusinessObject.DesignationManagement.Designation designation = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.DesignationManagement.Designation>(requestData);
            return DataAccess.DesignationManagement.DesignationMasterDL.SaveDesignation(designation);
        }

        /// <summary>
        /// Delete Designation Details
        /// </summary>
        /// <param name="desigID"></param>
        /// <returns>string</returns>
        public static string DeleteDesignation(int desigID)
        {
            return DataAccess.DesignationManagement.DesignationMasterDL.DeleteDesignationDtls(desigID).ToString();
        }

        #endregion

    }
}
