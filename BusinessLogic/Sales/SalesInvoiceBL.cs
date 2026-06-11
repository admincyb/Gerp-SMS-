using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.SaleOrder;
using GTIService;
using System.Data;

namespace BusinessLogic.Sales
{
    public class SalesInvoiceBL
    {
        /// <summary>
        /// Get Sales Invoice Details
        /// </summary>
        /// <param name="soPK"></param>
        /// <param name="invPK"></param>
        /// <returns></returns>
        public static SOInvoiceHeader GetSalesInvoiceHeader(int soPK, int invPK, int DespatchID = 0, int CustAllAdv=0)
        {
            try
            {
                SOInvoiceHeader invoiceHeaderObj = new SOInvoiceHeader();
                string invoice = DataAccess.SaleOrder.SalesInvoiceDL.GetSalesInvoiceHeader(soPK, invPK, DespatchID,CustAllAdv);
                if (invoice != string.Empty)
                {
                    invoiceHeaderObj = (SOInvoiceHeader)CommonFunctions.DeserializeObject(invoice, invoiceHeaderObj);
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
        public static SOInvoiceHeaderMul GetSalesInvoiceHeaderMUL(string soPK, int invPK, int DespatchID = 0, int CustAllAdv = 0)
        {
            try
            {
                SOInvoiceHeaderMul invoiceHeadermulObj = new SOInvoiceHeaderMul();
                string invoice = DataAccess.SaleOrder.SalesInvoiceDL.GetSalesInvoiceHeaderMul(soPK, invPK, DespatchID, CustAllAdv);
                if (invoice != string.Empty)
                {
                    invoiceHeadermulObj = (SOInvoiceHeaderMul)CommonFunctions.DeserializeObject(invoice, invoiceHeadermulObj);
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
        public static FinInvoiceHeaderMul GetVerificationDetailsMUL(int invPK)
        {
            try
            {
                FinInvoiceHeaderMul invoiceHeadermulObj = new FinInvoiceHeaderMul();
                string invoice = DataAccess.SaleOrder.SalesInvoiceDL.GetVerificationDetailsMUL(invPK);
                if (invoice != string.Empty)
                {
                    invoiceHeadermulObj = (FinInvoiceHeaderMul)CommonFunctions.DeserializeObject(invoice, invoiceHeadermulObj);
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
        /// Save Sales Invoice
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SaveSalesInvoiceHeader(string strxml)
        {
            return DataAccess.SaleOrder.SalesInvoiceDL.SaveSalesInvoiceHeader(strxml);
        }
        /// <summary>
        /// Delete Sales Invoice
        /// </summary>
        /// <param name="invPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteSalesInvoiceDetails(string currentUser, int invPK, DateTime lastModDate, string reasonForDelete, string appType)
        {
            return DataAccess.SaleOrder.SalesInvoiceDL.DeleteSalesInvoiceDetails(currentUser, invPK, lastModDate, reasonForDelete, appType);
        }
        public static DataSet GetDueDateByPaymentTerms(string strxml)
        {

            return DataAccess.SaleOrder.SalesInvoiceDL.GetDueDateByPaymentTerms(strxml);
        }
        public static DataSet GetInvCusReceivedAmntDetails(int InvPk)
        {

            return DataAccess.SaleOrder.SalesInvoiceDL.GetInvCusReceivedAmntDetails(InvPk);
        }
        public static int GetSOValidityCheck(string strxml, int IsDiscountCheckShp)
        {

            return DataAccess.SaleOrder.SalesInvoiceDL.GetSOValidityCheck(strxml, IsDiscountCheckShp);
        }

        public static SOInvoiceHeaderMul GetIssueDetail(int cusPK, int itemPk, int IssueType)
        {
            try
            {
                SOInvoiceHeaderMul invoiceHeadermulObj = new SOInvoiceHeaderMul();
                string invoice = DataAccess.SaleOrder.SalesInvoiceDL.GetIssueDetail(cusPK, itemPk, IssueType);
                if (invoice != string.Empty)
                {
                    invoiceHeadermulObj = (SOInvoiceHeaderMul) CommonFunctions.DeserializeObject(invoice, invoiceHeadermulObj);
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



        public static DataTable GetPendingSOList(int CUSTPK, int? DOPK, int? InvoicePk, int pageNumber, int pageSize)
        {
            return DataAccess.SaleOrder.SalesInvoiceDL.GetPendingSOList(CUSTPK, DOPK, InvoicePk, pageNumber, pageSize);
        }

        public static DirectSOInvoiceHeader GetDirectSalesInvoiceHeaderMUL(string xmlDOSOPK, int invPK, int DespatchID = 0)
        {
            try
            {
                DirectSOInvoiceHeader invoiceHeadermulObj = new DirectSOInvoiceHeader();
                string invoice = DataAccess.SaleOrder.SalesInvoiceDL.GetDirectSalesInvoiceHeaderMUL(xmlDOSOPK, invPK, DespatchID);
                if (invoice != string.Empty)
                {
                    invoiceHeadermulObj = (DirectSOInvoiceHeader)CommonFunctions.DeserializeObject(invoice, invoiceHeadermulObj);
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

        public static int? SaveDirectSalesInvoiceHeader(string xmlDoc, out string invNumber)
        {
            return DataAccess.SaleOrder.SalesInvoiceDL.SaveDirectSalesInvoiceHeader(xmlDoc, out invNumber);
        }
        public static int DeleteDirectSalesInvoiceDetails(string currentUser, int invPK, DateTime lastModDate, string reasonForDelete, string appType)
        {
            return DataAccess.SaleOrder.SalesInvoiceDL.DeleteDirectSalesInvoiceDetails(currentUser, invPK, lastModDate, reasonForDelete, appType);
        }

        public static DataTable GetDONumbers(int bizUnit, string searchValue, string customerId, int? InvoicePk)
        {
            return DataAccess.SaleOrder.SalesInvoiceDL.GetDONumbers(bizUnit, searchValue, customerId, InvoicePk);
        }


        public static DataTable GetPendingSalesOrderList(int custPK, int? salesOrderPK, int? InvoicePk, string SaleOrderNumber, int bizUnit, int pageNumber, int pageSize)
        {
            return DataAccess.SaleOrder.SalesInvoiceDL.GetPendingSalesOrderList(custPK, salesOrderPK, InvoicePk, SaleOrderNumber, bizUnit, pageNumber, pageSize);
        }

        public static DataTable GetSONumbers(int bizUnit, string searchValue, string customerId, int? InvoicePk)
        {
            return DataAccess.SaleOrder.SalesInvoiceDL.GetSONumbers(bizUnit, searchValue, customerId, InvoicePk);
        }

        public static DataTable GetTradingInvoiceNumbers(int bizUnit, string searchValue, int? InvCategory, int? InvGroup, int? CustomerPk = null, int? InvType = null)
        {
            return DataAccess.SaleOrder.SalesInvoiceDL.GetTradingInvoiceNumbers(bizUnit, searchValue, InvCategory, InvGroup, CustomerPk, InvType);
        }

        public static DataTable GetPendingInvoiceList(long CurrPK, int custPK, int invPK, int invCategory, int invType, DateTime? FromDate, DateTime? ToDate, int bizUnit, int pageNumber, int pageSize)
        {
            return DataAccess.SaleOrder.SalesInvoiceDL.GetPendingInvoiceList(CurrPK, custPK, invPK, invCategory, invType, FromDate, ToDate, bizUnit, pageNumber, pageSize);
        }

        public static BusinessObject.Sales.ReceiptHeader GetTradingSalesReceiptHeaderMUL(string xmlDoc, long ReceiptPk)
        {
            try
            {
                BusinessObject.Sales.ReceiptHeader receiptHeadermulObj = new BusinessObject.Sales.ReceiptHeader();
                string result = DataAccess.SaleOrder.SalesInvoiceDL.GetTradingSalesReceiptHeaderMUL(xmlDoc, ReceiptPk);
                if (result != string.Empty)
                {
                    receiptHeadermulObj = (BusinessObject.Sales.ReceiptHeader)CommonFunctions.DeserializeObject(result, receiptHeadermulObj);
                    return receiptHeadermulObj;
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

        public static DataTable GetCustomerAdjAllocation(int CustomerPk, long ReceiptPk)
        {
            return DataAccess.SaleOrder.SalesInvoiceDL.GetCustomerAdjAllocation(CustomerPk, ReceiptPk);
        }
    }
}
