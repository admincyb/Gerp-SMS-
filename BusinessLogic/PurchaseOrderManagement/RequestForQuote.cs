using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using DataAccess;
using BusinessObject.PurchaseOrderManagement;
using GTIService;
using BusinessObject.CommonManagement;

namespace BusinessLogic.PurchaseOrderManagement
{
    public class RequestForQuote
    {
        /// <summary>
        /// Get Purchase Request List Details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPurchaseRequestList(GridPrams grid, User objUser, int procID)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetPurchaseRequestList(grid, objUser, procID);
        }
        /// <summary>
        /// Returns the search result list for Autocomplete for RFQ Item request
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryPK"></param>
        /// <returns>string</returns>
        public static List<AutoCompleteBO> GetItemPRSearchValues(string searchBy, string searchValue, User objUser)
        {
            DataTable dtSearch;
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
            dtSearch = DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetItemPRSearchValues(searchBy, searchValue, objUser);
            result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
            {
                Key = row.Field<int>(GTIService.Constants.Common.Fields.PK),
                Name = row.Field<string>(GTIService.Constants.Common.Fields.VALUE)
            }).ToList();
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Get Purchase Request Header Details
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="rfqHdrPK"></param>
        /// <returns></returns>
        public static DataSet GetRFQHeader(string xml, int rfqHdrPK)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetRFQHeader(xml, rfqHdrPK);
        }

        /// <summary>
        /// Function used to get Vendor List 
        /// </summary>
        /// <param name="rfhPK"></param>
        /// <returns></returns>
        public static DataSet GetVendorList(int? rfhPK = null, int? vendorPK = null)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetVendorList(rfhPK, vendorPK);
        }
        /// <summary>
        /// Get Vendor List By PR Details
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static DataSet GetVendorListbyPR(string xml)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetVendorListbyPR(xml);
        }
        public static RFQResponseHeader GetRFQResponse(int rfhPK, int vendorPK)
        {
            try
            {
                RFQResponseHeader rfqResponseHeaderObj = new RFQResponseHeader();
                string rfqResponse = DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetRFQResponse(rfhPK, vendorPK);
                if (rfqResponse != string.Empty)
                {
                    rfqResponseHeaderObj = (RFQResponseHeader)CommonFunctions.DeserializeObject(rfqResponse, rfqResponseHeaderObj);
                    return rfqResponseHeaderObj;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }

        }

        public static DataSet GetRFQHeader(int rfhPK, short active, int bizUnit)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetRFQHeader(rfhPK, active, bizUnit);
        }

        public static int? SaveRFQResponseDetails(string strxml)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.SaveRFQResponseDetails(strxml);
        }

        /// <summary>
        /// Save RFQ
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveRFQDetails(string strxml)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.SaveRFQDetails(strxml);
        }
        /// <summary>
        /// Delete RFQ
        /// </summary>
        /// <param name="rfhPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteRFQDetails(int rfhPK, DateTime lastModDate)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.DeleteRFQDetails(rfhPK, lastModDate);
        }
        /// <summary>
        /// Get RFQ Trx No
        /// </summary>
        /// <param name="dept"></param>
        /// <param name="user"></param>
        /// <param name="subType"></param>
        /// <returns></returns>
        public static string GetRFQTrxNo(int dept, int user, int subType)
        {
             return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetRFQTrxNo(dept, user, subType);
        }

        /// <summary>
        /// Get Item Rates
        /// </summary>
        /// <param name="itemPK"></param>
        /// <param name="vendorPK"></param>
        /// <returns></returns>
        public static DataSet GetItemRates(int itemPK, int vendorPK)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetItemRates(itemPK, vendorPK);
        }

        public static DataSet GetRFQList(GridPrams grid, User objUser, int procID, int userPK, int deptPK = 0)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetRFQList(grid, objUser, procID,userPK,deptPK) ;
        }

        public static DataSet GetRFQTaxDetails(int TaxPK, int category, int bizUnit, byte active,int subCategory)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetRFQTaxDetails(TaxPK, category, bizUnit, active, subCategory);
        }

        public static DataSet GetExchangeRate(int fromCurrency, int toCurrency, DateTime date)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetExchangeRate(fromCurrency, toCurrency, date);
        }

        public static DataSet GetRFQReportDetails(int RecPK, int VndPK)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetRFQReportDetails(RecPK, VndPK);
        }

        public static DataSet GetRFQAmtCompReportDetails(int RecPK, int CmpType)
        {
            return DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetRFQAmtCompReportDetails(RecPK, CmpType);
        }
        /// <summary>
        /// Returns the search result list for Autocomplete for RFQ 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryPK"></param>
        /// <returns>string</returns>
        public static List<AutoCompleteBO> GetPURRFQAuto(string searchBy, string searchValue, User objUser, string pageURL)
        {
            DataTable dtSearch;
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                dtSearch = DataAccess.PurchaseOrderManagement.RequestForQuoteDA.GetPURRFQAuto(searchBy, searchValue, objUser, pageURL);
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>(GTIService.Constants.Common.Fields.PK),
                    Name = row.Field<string>(GTIService.Constants.Common.Fields.VALUE)
                }).ToList();
            }
            catch
            {
            }
            return result;
        }
    }
}
