using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.POInvoicing;
using GTIService;
using DataAccess.POInvoicing;
using BusinessObject;
using System.Data;
using BusinessObject.CommonManagement;

namespace BusinessLogic.POInvoicing
{
    public class POInvoiceBL
    {
        /// <summary>
        /// Get PO Invoice Details
        /// </summary>
        /// <param name="poPK"></param>
        /// <param name="invPK"></param>
        /// <returns></returns>
        public static POInvoiceHeader GetPOInvoiceHeader(int poPK, int invPK)
        {
            try
            {
                POInvoiceHeader invoiceHeaderObj = new POInvoiceHeader();
                string invoice = POInvoiceDL.GetPOInvoiceHeader(poPK, invPK);
                if (invoice != string.Empty)
                {
                    invoiceHeaderObj = (POInvoiceHeader)CommonFunctions.DeserializeObject(invoice, invoiceHeaderObj);
                    return invoiceHeaderObj;
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
        /// Save PO Invoice
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SavePOInvoiceHeader(string strxml)
        {
            return POInvoiceDL.SavePOInvoiceHeader(strxml);
        }
        /// <summary>
        /// Save PO Invoice
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SavePOInvoiceWkf(string strxml,out string POInvoiceNo)
        {
            return POInvoiceDL.SavePOInvoiceWkf(strxml, out POInvoiceNo);
        }
        /// <summary>
        /// Save expense settlement
        /// </summary>
        /// <param name="strxml"></param>
        /// <param name="POInvoiceNo"></param>
        /// <returns></returns>
        public static int? SaveExpSettlementInvoiceWkf(string strxml, out string InvoiceNo)
        {
            return POInvoiceDL.SaveExpSettlementInvoiceWkf(strxml, out InvoiceNo);
        }
        /// <summary>
        /// Delete PO Invoice
        /// </summary>
        /// <param name="invPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeletePOInvoiceDetails(int invPK, DateTime lastModDate, string appType, string currentUser)
        {
            return POInvoiceDL.DeletePOInvoiceDetails(invPK, lastModDate, appType, currentUser);
        }
        /// <summary>
        /// Get PO Invoice List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="cusID"></param>
        /// <param name="InvPk"></param>
        /// <param name="PoPk"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static DataSet GetPOInvoiceList(GridPrams grid, User objUser, int cusID, int InvPk, int PoPk, string Customer, string PoNo, string pageUrl, int type = 0, int status = 0, int group = 0, byte category = 1, byte? pending = null, string grnNo = null, string due = null, int cmpPk = 0)
        {
            return POInvoiceDL.GetPOInvoiceList(grid, objUser, cusID, InvPk, PoPk, Customer, PoNo, pageUrl, type, status, group, category, pending, grnNo, due, cmpPk);
        }

        /// <summary>
        /// Get PO Invoice List
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="VendorId"></param>
        /// <param name="XmlInvPks"></param>
        /// <returns></returns>
        public static DataSet GetPOInvoiceList(User objUser, int VendorId, string XmlInvPks)
        {
            return POInvoiceDL.GetPOInvoiceList(objUser, VendorId, XmlInvPks);
        }

        /// <summary>
        /// Get PO List
        /// </summary>
        /// <param name="objUser"></param>       
        /// <param name="XmlInvPks"></param>
        /// <returns></returns>
        public static DataSet GetPOList(User objUser, string XmlInvPks, int pohGroup = 0)
        {
            return POInvoiceDL.GetPOList(objUser, XmlInvPks, pohGroup);
        }
        /// <summary>
        /// Get PO/WO details 
        /// </summary>
        /// <param name="pohGroup"></param>       
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetOrderDetails(int id, int pohGroup = 0)
        {
            return POInvoiceDL.GetOrderDetails(id,pohGroup);
        }
        /// <summary>
        /// Get GRN details 
        /// </summary>
        /// <param name="pohGroup"></param>       
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetGRNDetails(int id, int pohGroup = 0)
        {
            return POInvoiceDL.GetGRNDetails(id, pohGroup);
        }

        /// <summary>
        /// Get Purchase Invoice OrderDetails For Report
        /// </summary>
        /// <param name="Pid"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPurchaseInvoiceDtls(int Pid, string PurchaseInvoiceOutRPTSP)
        {
            return POInvoiceDL.GetPurchaseInvoiceDtls(Pid, PurchaseInvoiceOutRPTSP);
        }
        /// <summary>
        /// Get Purchase Invoice OrderDetails For Report
        /// </summary>
        /// <param name="Pid"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetAddressTypeDtls(User objUser, int ACTIVE, int Vendor, int type)
        {
            return POInvoiceDL.GetAddressTypeDtls(objUser, ACTIVE, Vendor, type);
        }
        /// <summary>
        /// Get Expense Advances
        /// </summary>
        /// <param name="venPK"></param>
        /// <param name="transactionPK"></param>
        /// <returns></returns>
        public static DataTable GetExpenseAdvances(int venPK, int transactionPK)
        {
            return POInvoiceDL.GetExpenseAdvances(venPK, transactionPK);
        }
        public static DataSet GetVendorAddressTypes(User objUser, short ACTIVE, int VncPk, int VendorPK, int type)
        {
            return POInvoiceDL.GetVendorAddressTypes(objUser, ACTIVE, VncPk, VendorPK, type);
        }
        public static DataSet GetLineitemTaxList(int paymentPK)
        {
            return POInvoiceDL.GetLineitemTaxList(paymentPK);
        }
        public static int GetDebitCreditPost(string PKXml)
        {
            return POInvoiceDL.GetDebitCreditPost(PKXml);
        }


        #region Invoice Multiple PO

        /// <summary>
        /// Check for valid PO
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int CheckforValidPO(string strxml)
        {
            return POInvoiceDL.CheckforValidPO(strxml);
        }

        /// <summary>
        /// Get Multiple PO Invoice Details
        /// </summary>      
        /// <returns></returns>
        public static MultiplePOInvoiceHeader GetMultiplePOInvoiceHeader(int poPK, int invPK, string strxml)
        {
            try
            {
                MultiplePOInvoiceHeader invoiceHeaderObj = new MultiplePOInvoiceHeader();
                string invoice = POInvoiceDL.GetMultiplePOInvoiceHeader(poPK, invPK, strxml);
                if (invoice != string.Empty)
                {
                    invoiceHeaderObj = (MultiplePOInvoiceHeader)CommonFunctions.DeserializeObject(invoice, invoiceHeaderObj);
                    return invoiceHeaderObj;
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
        #endregion

        /// <summary>
        /// Get payment types
        /// </summary>
        /// <param name="ACTIVE"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataTable GetPaymentTypes(int ACTIVE, string type)
        {
            return POInvoiceDL.GetPaymentTypes(ACTIVE, type);
        }

        public static DataTable GetBalanceAmountDetails(long InvoicePK)
        {
            return POInvoiceDL.GetBalanceAmountDetails(InvoicePK);
        }


        public static DataTable GetPoWoNumbers(string searchKey,int ? EnblWO,int BizUnit)
        {
            return POInvoiceDL.GetPoWoNumbers(searchKey,EnblWO,BizUnit);
        }

        public static int SaveInvoiceArchiveDetails(long InvoicePK)
        {
            return POInvoiceDL.SaveInvoiceArchiveDetails(InvoicePK);
        }

        public static int SaveSalesInvoiceArchiveDetails(long InvoicePK)
        {
            return POInvoiceDL.SaveSalesInvoiceArchiveDetails(InvoicePK);
        }

        public static int ValidateAdvanceInvType(string XML,int InvoicePK)
        {
            return POInvoiceDL.ValidateAdvanceInvType(XML,InvoicePK);
        }

        public static int SaveDRCR_Note_ArchiveDetails(long InvoicePK)
        {
            return POInvoiceDL.SaveDRCR_Note_ArchiveDetails(InvoicePK);
        }
        /// <summary>
        /// Get Purchase Order List
        /// </summary>
        /// <param name="@P_POH_PK"></param>
        /// <param name="@P_ACTIVE"></param>
        /// <param name="@P_POH_GROUP"></param>
        /// <param name="@P_POH_VENDOR"></param>     
        /// <param name="@P_TRX_STATUS"></param>
        /// <param name="@P_BIZUNIT"></param>
        /// <param name="@P_FROMDATE"></param>
        /// <param name="@P_TODATE"></param>
        public static DataTable GetPurchaseOrderList(int POH_PK, int ACTIVE, int POH_GROUP, int POH_VENDOR, int TRX_STATUS, int BIZUNIT, DateTime? FROMDATE, DateTime? TODATE, string SOH_No, int PNO, int PSize, int POH_COMPANY,int P_IsEnableWo, int POH_DEPT=0, int user = 0)
        {
            return POInvoiceDL.GetPurchaseOrderList(POH_PK, ACTIVE, POH_GROUP, POH_VENDOR, TRX_STATUS, BIZUNIT, FROMDATE, TODATE, SOH_No, PNO, PSize, POH_COMPANY, P_IsEnableWo, POH_DEPT, user);
        }

        #region Trading
        /// <summary>
        /// Save Expense Invoice Trading
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SavePOInvoiceTradingWkf(string strxml, out string POInvoiceNo)
        {
            return POInvoiceDL.SavePOInvoiceTradingWkf(strxml, out POInvoiceNo);
        }
        /// <summary>
        /// Delete PO Invoice Trading
        /// </summary>
        /// <param name="invPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeletePOInvoiceTradingDetails(int invPK, DateTime lastModDate, string appType, string currentUser)
        {
            return POInvoiceDL.DeletePOInvoiceTradingDetails(invPK, lastModDate, appType, currentUser);
        }
        #endregion

        /// <summary>
        /// Get PO Invoice Trading Details
        /// </summary>
        /// <param name="poPK"></param>
        /// <param name="invPK"></param>
        /// <returns></returns>
        public static POInvoiceHeader GetPOInvoiceTradingHeader(int poPK, int invPK)
        {
            try
            {
                POInvoiceHeader invoiceHeaderObj = new POInvoiceHeader();
                string invoice = POInvoiceDL.GetPOInvoiceTradingHeader(poPK, invPK);
                if (invoice != string.Empty)
                {
                    invoiceHeaderObj = (POInvoiceHeader)CommonFunctions.DeserializeObject(invoice, invoiceHeaderObj);
                    return invoiceHeaderObj;
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
        /// Get PO Invoice Trading List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="cusID"></param>
        /// <param name="InvPk"></param>
        /// <param name="PoPk"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static DataSet GetPOInvoiceTradingList(GridPrams grid, User objUser, int cusID, int InvPk, int PoPk, string Customer, string PoNo, string pageUrl, int type = 0, int status = 0, byte group = 0, byte category = 1, byte? pending = null, string grnNo = null, string due = null, int cmpPk = 0)
        {
            return POInvoiceDL.GetPOInvoiceTradingList(grid, objUser, cusID, InvPk, PoPk, Customer, PoNo, pageUrl, type, status, group, category, pending, grnNo, due, cmpPk);
        }

        /// <summary>
        /// Get Direct(Trading) invoice number auto
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetDirectInvoiceNoAutoComplete(string searchValue, User objUser, int? InvCategory, int? InvGroup)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = DataAccess.POInvoicing.POInvoiceDL.GetDirectInvoiceNoAutoComplete(searchValue, objUser,InvCategory, InvGroup);
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


        #region Purchase Invoice Trading
        public static DataTable GetPendingPOList(int CUSTPK, int? DOPK, int? InvoicePk, int pageNumber, int pageSize, DateTime? FromDate, DateTime? ToDate)
        {
            return DataAccess.POInvoicing.POInvoiceDL.GetPendingPOList(CUSTPK, DOPK, InvoicePk, pageNumber, pageSize,FromDate,ToDate);
        }
        public static DataTable GetPurchaseOrderNumbers(int bizUnit, string searchValue, int vendorPk, int? InvoicePk)
        {
            return DataAccess.POInvoicing.POInvoiceDL.GetPurchaseOrderNumbers(bizUnit, searchValue, vendorPk, InvoicePk);
        }
        public static DataTable GetPendingInvoiceNoTrading(int bizUnit, string searchValue, int? invCategory, int? invGroup,int? invType=null, int? vendorPk = null)
        {
            return DataAccess.POInvoicing.POInvoiceDL.GetPendingInvoiceNoTrading(bizUnit, searchValue, invCategory, invGroup,invType, vendorPk);
        }
        #endregion

        #region Purchase Advance Invoice Trading

        public static DirectPOInvoiceHeader GetDirectPurchaseInvoiceHeaderMUL(string xmlDOSOPK, int invPK, int isAdvanceInvoice = 0)
        {
            try
            {
                DirectPOInvoiceHeader invoiceHeadermulObj = new DirectPOInvoiceHeader();
                string invoice = DataAccess.POInvoicing.POInvoiceDL.GetDirectPurchaseInvoiceHeaderMUL(xmlDOSOPK, invPK, isAdvanceInvoice);
                if (invoice != string.Empty)
                {
                    invoiceHeadermulObj = (DirectPOInvoiceHeader)CommonFunctions.DeserializeObject(invoice, invoiceHeadermulObj);
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
      
        #endregion

        public static DataTable GetInvPayments(int InvPk)
        {
            return POInvoiceDL.GetInvPayments(InvPk);
        }
        public static DataTable GetVendorDetails(int venPk)
        {
            return POInvoiceDL.GetVendorDetails(venPk);
        }

        public static DataTable GetPOCostCenterDetails(int dtlPk)
        {
            return POInvoiceDL.GetPOCostCenterDetails(dtlPk);
        }

        public static DataTable GetPOTypes(int ACTIVE, int pk, string type)
        {
            return POInvoiceDL.GetPOTypes(ACTIVE, pk, type);
        }

        public static Dictionary<string, object> SaveInvoiceConvert(string strxml, int POSBU, int SCSBU)
        {
            return POInvoiceDL.SaveInvoiceConvert(strxml, POSBU, SCSBU);
        }

        public static string GetPOListByPK(string strxml, int BizUnit)
        {
            return POInvoiceDL.GetPOListByPK(strxml, BizUnit);
        }

        public static int SaveInvoice(string strXml)
        {
            return POInvoiceDL.SaveInvoice(strXml);
        }

        public static int SaveAdvanceInvoiceWkf(string strXml, out string invNo)
        {
            return POInvoiceDL.SaveAdvanceInvoiceWkf(strXml, out invNo);
        }

        public static string GetAdvInvoiceHeader(int poPK, int invPK)
        {
            return POInvoiceDL.GetAdvInvoiceHeader(poPK, invPK);
        }

        public static string GetAdvInvoiceList(GridPrams grid, User objUser, int cusID, int InvPk, int PoPk, string Customer, string PoNo, string pageUrl, string payByFrom, string payByTo, out int TotalRecords, int type = 0, int status = 0, int group = 0, byte category = 1, byte? pending = null, string grnNo = null, string due = null, int cmpPk = 0)
        {
            return POInvoiceDL.GetAdvInvoiceList(grid, objUser, cusID, InvPk, PoPk, Customer, PoNo, pageUrl, payByFrom, payByTo, out TotalRecords, type, status, group, category, pending, grnNo, due, cmpPk);
        }
        public static DataSet GetDailyInspectionDetails(int pk)
        {
            return POInvoiceDL.GetDailyInspectionDetails(pk);
        }
        public static DataSet GetRMIPMDetails(int pk)
        {
            return POInvoiceDL.GetRMIPMDetails(pk);
        }
    }
}
