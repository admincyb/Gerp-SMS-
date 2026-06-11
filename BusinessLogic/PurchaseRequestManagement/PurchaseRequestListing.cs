using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using System.Data;
using BusinessObject.CommonManagement;

namespace BusinessLogic.PurchaseRequestManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class PurchaseRequestListing
    {

        #region Methods

        /// <summary>
       /// Get Purchase Request Details
       /// </summary>
       /// <param name="grid"></param>
       /// <param name="bizUnit"></param>
       /// <returns></returns>
        public static string GetPurchaseRequestList(GridPrams grid, User objUser, string pageURL, int procId, int transactionStatus = 0, int reqStore = 0, string ioNo = null, string ItemName = null, int reqDept = 0, string reqBy = null, int FilterStatus = 0, string prNo = null, int cmpPk = 0)
        {
            DataSet dsReqstList = DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.GetPurchaseRequestList(grid, objUser, pageURL, procId, transactionStatus,reqStore,ioNo,ItemName,reqDept,reqBy,FilterStatus,prNo,cmpPk);
            string jString = string.Empty;
            if (dsReqstList.Tables.Count > 1 && dsReqstList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsReqstList);
            }
            return jString;
        }

        /// <summary>
      /// Get AuoComplete Search Details
      /// </summary>
      /// <param name="searchBy"></param>
      /// <param name="searchValue"></param>
      /// <param name="bizUnit"></param>
      /// <returns></returns>
        public static string GetSearchValues(string searchBy, string searchValue, int processPK, User objUser, string pageUrl = null, int type = 0)
        {
            DataTable dtSearch = DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.GetSearchValues(searchBy, searchValue, processPK, objUser, pageUrl, type);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="processPK"></param>
        /// <param name="objUser"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static string GetMaterialSearchValues(string searchBy, string searchValue, int processPK, User objUser, string pageUrl = null, int MenuType = 0)
        {
            DataTable dtSearch = DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.GetMaterialSearchValues(searchBy, searchValue, processPK, objUser, pageUrl, MenuType);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);

        }

        public static string ValidateItemStock(int Dept, int MaterialPK, decimal Qty)
        {
            DataTable dtResult = DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.ValidateItemStock(Dept, MaterialPK, Qty);
            string jString = string.Empty;
            if (dtResult.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtResult);
            }
            return jString;
        }

        /// <summary>
        /// Get AuoComplete Search Details
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetPurchaseRequestAutocomplete(string searchBy, string searchValue, int processPK, User objUser, string pageURL = null)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.GetSearchValues(searchBy, searchValue, processPK, objUser,pageURL);
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>(GTIService.Constants.Designation.Fields.PK),
                    Name = row.Field<string>(GTIService.Constants.Designation.Fields.VALUE)
                }).ToList();
            }
            catch
            {
            }
            return result;
        }
        /// <summary>
        /// Delete Purchase Request Details
        /// </summary>
        /// <param name="desigID"></param>
        /// <returns></returns>
        public static string DeletePurchaseRequest(int requestPk,string remarks)
        {
            return DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.DeletePurchaseRequestDtls(requestPk, remarks).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseRequestID"></param>
        /// <returns></returns>
        public static DataSet GetPurchaseRequestReportDetails(int purchaseRequestID)
        {
            return DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.GetPurchaseRequestReportDetails(purchaseRequestID);
        }

        public static DataSet GetPurchaseRequestReportDetailsDOCNOREVISION(int purchaseRequestID,int reportpk)
        {
            return DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.GetPurchaseRequestReportDetailsDOCNOREVISION(purchaseRequestID,reportpk);
        }


        /// <summary>
        /// To Get Material Request Trx Print Details
        /// </summary>
        /// <param name="materialRequestID"></param>
        /// <returns></returns>
        public static DataSet GetMaterialRequestReportDetails(int materialRequestID)
        {
            return DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.GetMaterialRequestReportDetails(materialRequestID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseRequestID"></param>
        /// <returns></returns>
        public static DataSet GetCompoundUsageSummary(int RecPK, int SbuID, DateTime startOfMonth, DateTime endOfMonth, int Size = 0)
        {
            return DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.GetCompoundUsageSummary(RecPK, SbuID, startOfMonth, endOfMonth,Size);
        }

        public static DataSet GetCompoundYieldCost(int RecPK, int SbuID, DateTime startOfMonth, DateTime endOfMonth)
        {
            return DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.GetCompoundYieldCost(RecPK, SbuID, startOfMonth, endOfMonth);
        }
        #endregion
    }
}
