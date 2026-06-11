using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.SaleOrder
{
    public class SalesInvoiceDL
    {
        /// <summary>
        /// Get Sales Invoice Details
        /// </summary>
        /// <param name="soPK"></param>
        /// <param name="invPK"></param>
        /// <returns></returns>
        public static string GetSalesInvoiceHeader(int soPK, int invPK, int DespatchID = 0, int CustAllAdv = 0)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK,  soPK == 0 ? (object) DBNull.Value :  soPK),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK,  invPK == 0 ? (object) DBNull.Value :  invPK) ,
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_DPH_PK ,  DespatchID == 0 ? (object) DBNull.Value :  DespatchID),
                 new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CUS_ADV ,  CustAllAdv)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETINVOICEDETAILS, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        public static string GetSalesInvoiceHeaderMul(string strxml, int invPK, int DespatchID = 0, int CustAllAdv = 0)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml==""?(object) DBNull.Value:strxml),  
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK,  (object) DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK,  invPK == 0 ? (object) DBNull.Value :  invPK) ,
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_DPH_PK ,  DespatchID == 0 ? (object) DBNull.Value :  DespatchID),
                 new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CUS_ADV ,  CustAllAdv)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETINVOICEDETAILSMUL, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        public static string GetVerificationDetailsMUL(int invPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            { 
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK,  invPK == 0 ? (object) DBNull.Value :  invPK) ,
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETVERIFICATIONDETAILSMUL, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        /// <summary>
        /// Save Sales Invoice
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SaveSalesInvoiceHeader(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SAVEINVOICEDETAILS, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Delete Sales Invoice
        /// </summary>
        /// <param name="invPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteSalesInvoiceDetails(string currentUser, int invPK, DateTime lastModDate, string reasonForDelete, string appType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK, invPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER ,currentUser),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_REASON_FOR_DELETE, string.IsNullOrEmpty(reasonForDelete)?(object)DBNull.Value:reasonForDelete),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.App_Type, appType),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.DELETEINVOICEDETAILS, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        public static DataSet GetDueDateByPaymentTerms(string strxml)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml) 
            };
            DataSet dsDueDate = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_DUEDATE_BY_PAYMENTTERMS, colParameters);
            return dsDueDate;


        }

        public static DataSet GetInvCusReceivedAmntDetails(int InvPk)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters( GTIService.Constants.Sales.Parameters.ICH_PK, InvPk) 
            };
            DataSet dsDueDate = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_CUS_ALCN_GET, colParameters);
            return dsDueDate;


        }
        public static int GetSOValidityCheck(string strxml, int IsDiscountCheckShp)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.IS_DISCOUNT_EXIST, IsDiscountCheckShp),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            DataSet dsDueDate = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INV_SO_VALD, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        public static string GetIssueDetail(int cusPK, int itemPk, int IssueType)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CII_CUSTOMER,  cusPK == 0 ? (object) DBNull.Value :  cusPK),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CII_ITEM,  itemPk == 0 ? (object) DBNull.Value :  itemPk) ,
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CII_TYPE ,  IssueType == 0 ? (object) DBNull.Value :  IssueType)                
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_CUS_ITEM_ISSUE_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static DataTable GetPendingSOList(int CUSTPK, int? DOPK,int? InvoicePk, int pageNumber, int pageSize)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CUSTOMER, CUSTPK),      
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_DPH_PK, (DOPK.HasValue && DOPK > 0) ? DOPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK, (InvoicePk.HasValue && InvoicePk > 0) ? InvoicePk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM, pageNumber),      
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, pageSize)
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_CUS_PENDING_GET, colParameters);
            return dtResult;
        }

        public static string GetDirectSalesInvoiceHeaderMUL(string strxml, int invPK, int DespatchID = 0)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, string.IsNullOrEmpty(strxml) ? (object) DBNull.Value : strxml),  
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK,  (object) DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK,  invPK == 0 ? (object) DBNull.Value :  invPK) ,
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_DPH_PK ,  DespatchID == 0 ? (object) DBNull.Value :  DespatchID)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_CUS_MULTIPLE_SO_TRADING_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int? SaveDirectSalesInvoiceHeader(string xmlDoc, out string invNumber)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_CUS_TRADING_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            invNumber = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            return result;
        }

        public static int DeleteDirectSalesInvoiceDetails(string currentUser, int invPK, DateTime lastModDate, string reasonForDelete, string appType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK, invPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER ,currentUser),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_REASON_FOR_DELETE, string.IsNullOrEmpty(reasonForDelete)?(object)DBNull.Value:reasonForDelete),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.App_Type, appType),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_CUS_TRADING_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetDONumbers(int bizUnit, string searchValue, string customerId, int? InvoicePk)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_DPH_CUSTOMER, string.IsNullOrEmpty(customerId) ? (object)DBNull.Value : customerId),     
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK, (InvoicePk.HasValue && InvoicePk > 0) ? InvoicePk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_VALUE, searchValue),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_BIZUNIT, bizUnit)   
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_CUS_PENDING_DO_AUTO, colParameters);
            return dtResult;
        }

        public static DataTable GetPendingSalesOrderList(int custPK, int? salesOrderPK, int? InvoicePk, string SaleOrderNumber,int bizUnit, int pageNumber, int pageSize)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SOH_CUSTOMER, custPK),      
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, (salesOrderPK.HasValue && salesOrderPK > 0) ? salesOrderPK : (object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK, (InvoicePk.HasValue && InvoicePk > 0) ? InvoicePk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SOH_NO, string.IsNullOrEmpty(SaleOrderNumber.Trim()) ? (object)DBNull.Value : SaleOrderNumber.Trim()),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_BIZUNIT, bizUnit),   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM, pageNumber),      
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, pageSize)
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPSAL_ORDER_DIR_PEND_SO_GET, colParameters);
            return dtResult;
        }

        public static DataTable GetSONumbers(int bizUnit, string searchValue, string customerId, int? InvoicePk)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SOH_CUSTOMER, string.IsNullOrEmpty(customerId) ? (object)DBNull.Value : customerId),     
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK, (InvoicePk.HasValue && InvoicePk > 0) ? InvoicePk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_VALUE, searchValue),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_BIZUNIT, bizUnit)   
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPSAL_ORDER_DIR_SO_PEND_AUTO, colParameters);
            return dtResult;
        }

        public static DataTable GetTradingInvoiceNumbers(int bizUnit, string searchValue, int? InvCategory, int? InvGroup,int? CustomerPk = null, int? InvType = null)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_CATEGORY, (InvCategory.HasValue && InvCategory > 0) ? InvCategory : (object)DBNull.Value),     
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_GROUP, (InvGroup.HasValue && InvGroup > 0) ? InvGroup : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_TYPE, (InvType.HasValue && InvType > 0) ? InvType : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_CUSTOMER, (CustomerPk.HasValue && CustomerPk > 0) ? CustomerPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_VALUE, searchValue),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_BIZUNIT, bizUnit)   
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_SAL_DIR_INVOICE_NO_AUTO, colParameters);
            return dtResult;
        }

        public static DataTable GetPendingInvoiceList(long CurrPK, int custPK, int invPK,int invCategory,int invType, DateTime? FromDate, DateTime? ToDate, int bizUnit, int pageNumber, int pageSize)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK, invPK > 0 ? invPK : (object)DBNull.Value),     
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_CUSTOMER, custPK > 0 ? custPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_CATEGORY, invCategory > 0 ? invCategory : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_TYPE, invType > 0 ? invType : (object)DBNull.Value),                       
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_FROM_DT, FromDate.HasValue ? FromDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_TO_DT, ToDate.HasValue ? ToDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM, pageNumber),      
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, pageSize),     
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_BIZUNIT, bizUnit)   
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_TRADING_PEND_INV_GET, colParameters);
            return dtResult;
        }

        public static string GetTradingSalesReceiptHeaderMUL(string xmlDoc, long ReceiptPk)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, string.IsNullOrEmpty(xmlDoc) ? (object) DBNull.Value : xmlDoc),               
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_RCH_PK,  ReceiptPk > 0 ? ReceiptPk : (object) DBNull.Value ) 
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_RECEIPT_CUS_HDR_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static DataTable GetCustomerAdjAllocation(int CustomerPk, long ReceiptPk)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_RCH_PK, ReceiptPk > 0 ? ReceiptPk : (object)DBNull.Value),     
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CUS_PK, CustomerPk)
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_RECEIPT_CUS_ALCN_GET, colParameters);
            return dtResult;
        }
    }
}
