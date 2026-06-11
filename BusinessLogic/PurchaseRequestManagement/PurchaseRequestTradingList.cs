using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using System.Data;
using BusinessObject.CommonManagement;

namespace BusinessLogic.PurchaseRequestManagement
{
    public class PurchaseRequestTradingList
    {
        #region Methods

        /// <summary>
        /// Get Purchase Request Trading List Details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetPurchaseRequestTradingList(GridPrams grid, User objUser, string pageURL, int procId)
        {
            DataSet dsReqstList = DataAccess.PurchaseRequestManagement.PurchaseRequestTradingListDL.GetPurchaseRequestTradingList(grid, objUser, pageURL, procId);
            string jString = string.Empty;
            if (dsReqstList.Tables.Count > 1 && dsReqstList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsReqstList);
            }
            return jString;
        }

        /// <summary>
        /// Delete Purchase Request Trading List Details
        /// </summary>
        /// <param name="desigID"></param>
        /// <returns></returns>
        public static string DeletePurchaseRequestTradingList(int requestPk, string remarks)
        {
            return DataAccess.PurchaseRequestManagement.PurchaseRequestTradingListDL.DeletePurchaseRequestTradingList(requestPk, remarks).ToString();
        }

        /// <summary>
        /// Get AuoComplete Search Details
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetSearchValues(string searchBy, string searchValue, int processPK, User objUser)
        {
            DataTable dtSearch = DataAccess.PurchaseRequestManagement.PurchaseRequestTradingListDL.GetSearchValues(searchBy, searchValue, processPK, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);

        }

        #endregion
    }
}
