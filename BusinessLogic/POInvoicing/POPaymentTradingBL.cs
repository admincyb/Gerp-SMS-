using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.POInvoicing;
using GTIService;
using DataAccess.POInvoicing;
using BusinessObject.CommonManagement;

namespace BusinessLogic.POInvoicing
{
    public class POPaymentTradingBL
    {
        /// <summary>
        /// Get Payment Trading List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="cusID"></param>
        /// <param name="InvPk"></param>
        /// <param name="PoPk"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static DataSet GetPaymentTradingList(GridPrams grid, User objUser, int vendorID, int paymentPk, string invNo, string pageUrl, int status = 0, int PDCStatus = 0, int cmpPk = 0)
        {
            return PoPaymentTradingDL.GetPaymentTradingList(grid, objUser, vendorID, paymentPk, invNo, pageUrl, status,PDCStatus, cmpPk);
        }
        public static DataTable GetPendingInvList(User currentUser, int vendPK, int invPK, int pageNumber, int pageSize, DateTime? FromDate, DateTime? ToDate, int invCategory, int invType)
        {
            return DataAccess.POInvoicing.PoPaymentTradingDL.GetPendingInvList(currentUser, vendPK, invPK, pageNumber, pageSize,FromDate,ToDate,invCategory,invType);
        }
        public static POPaymentTradingBO GetTradingPurchaseInvoiceHeaderMUL(string xmlDocInv, int cdhPK)
        {
            try
            {
                POPaymentTradingBO invoiceHeadermulObj = new POPaymentTradingBO();
                string invoice = DataAccess.POInvoicing.PoPaymentTradingDL.GetPaymentHeaderMUL(xmlDocInv, cdhPK);
                if (invoice != string.Empty)
                {
                    invoiceHeadermulObj = (POPaymentTradingBO)CommonFunctions.DeserializeObject(invoice, invoiceHeadermulObj);
                    return invoiceHeadermulObj;
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
        /// <summary>
        /// Save Payment Trading
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SavePaymentTradingWkf(string strxml, out string paymentNo)
        {
            return PoPaymentTradingDL.SavePaymentTradingWkf(strxml, out paymentNo);
        }

        public static DataTable GetVendorDebitNoteAlcnForPaymentAdjn(int vendorPk, int pvmPk)
        {
            return DataAccess.POInvoicing.PoPaymentTradingDL.GetVendorDebitNoteAlcnForPaymentAdjn(vendorPk, pvmPk);
        }
        /// <summary>
        /// Delete Payment Trading
        /// </summary>
        /// <param name="invPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeletePaymentTradingDetails(int pvhPK, DateTime lastModDate, string appType, string currentUser)
        {
            return PoPaymentTradingDL.DeletePaymentTradingDetails(pvhPK, lastModDate, appType, currentUser);
        }
           /// <summary>
       /// Get Direct(Trading) invoice number auto
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
       /// <param name="objUser"></param>
       /// <returns></returns>
        public static List<AutoCompleteBO> GetPaymentNoTradingAutoComplete(string searchValue, User objUser)
       {
           List<AutoCompleteBO> result;
           result = new List<AutoCompleteBO>();
           try
           {
               DataTable dtSearch = DataAccess.POInvoicing.PoPaymentTradingDL.GetPaymentNoTradingAutoComplete(searchValue, objUser);
               result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
               {
                   LongKey = row.Field<long>(GTIService.Constants.Designation.Fields.PK),
                   Name = row.Field<string>(GTIService.Constants.Designation.Fields.VALUE)
               }).ToList();
           }
           catch
           {
           }
           return result;
       }
    }
}
