using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.StoreManagement
{
    public class StoreAuditList
    {

        #region Methods

        /// <summary>
        /// Method to get Store Audit List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="procID"></param>
        /// <returns></returns>
        public static string GetStoreAuditList(GridPrams grid, User objUser, int procID, string PageUrl)
        {
            DataSet dsReqstList = DataAccess.StoreManagement.StoreAuditListDL.GetStoreAuditList(grid, objUser, procID, PageUrl);
            string jString = string.Empty;
            if (dsReqstList.Tables.Count > 1 && dsReqstList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsReqstList);
            }
            return jString;
        }

        /// <summary>
        /// Method to get Store Audit AutoComplete List
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetSearchValues(string searchBy, string searchValue, User objUser)
        {
            DataTable dtSearch = DataAccess.StoreManagement.StoreAuditListDL.GetSearchValues(searchBy, searchValue, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);

        }
        /// <summary>
        /// Method to delete store audit details
        /// </summary>
        /// <param name="requestPk"></param>
        /// <returns></returns>

        public static string DeleteStoreAuditDtls(int requestPk)
        {
            return DataAccess.StoreManagement.StoreAuditListDL.DeleteStoreAuditDtls(requestPk).ToString();
        }

        /// <summary>
        /// Method to Get Store Audit report details
        /// </summary>
        /// <param name="purchaseRequestID"></param>
        /// <returns></returns>
        public static DataSet GetStoreAuditReportDetails(int purchaseRequestID)
        {
            return DataAccess.StoreManagement.StoreAuditListDL.GetStoreAuditReportDetails(purchaseRequestID);
        }
        #endregion
    }
}
