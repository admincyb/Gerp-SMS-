using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.POInvoicing
{
    public class POInvoiceDL
    {
        /// <summary>
        /// Get PO Invoice Details
        /// </summary>
        /// <param name="poPK"></param>
        /// <param name="invPK"></param>
        /// <returns></returns>
        public static string GetPOInvoiceHeader(int poPK, int invPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.POH_PK,  poPK == 0 ? (object) DBNull.Value :  poPK),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK,  invPK == 0 ? (object) DBNull.Value :  invPK)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.GETINVOICEDETAILS, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        /// <summary>
        /// Save PO Invoice
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SavePOInvoiceHeader(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SAVEINVOICEDETAILS, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Save PO Invoice Wokrflow
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SavePOInvoiceWkf(string strxml, out string POInvoiceNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SAVEINVOICEDETAILSWKF, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            POInvoiceNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            return result;
        }
        /// <summary>
        /// Save expense settlement
        /// </summary>
        /// <param name="strxml"></param>
        /// <param name="POInvoiceNo"></param>
        /// <returns></returns>
        public static int? SaveExpSettlementInvoiceWkf(string strxml, out string InvoiceNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SAVEEXPSETTLEMENTWKF, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            InvoiceNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            return result;
        }

        /// <summary>
        /// Get Purchase Invoice List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPOInvoiceList(GridPrams grid, User objUser, int cusID, int InvPk, int PoPk, string Customer, string PoNo, string pageUrl, int type = 0, int status = 0, int group = 0, byte category = 1, byte? pending = null, string grnNo = null, string due = null, int cmpPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy =="IVH_PK"?grid.SearchValue:grid.SearchValue+"%")),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_USER_PK,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  pageUrl),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.POD_PO,  PoPk == 0 ? (object) DBNull.Value :  PoPk),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_TYPE,  type == 0 ? (object) DBNull.Value :  type),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_STATUS_FILTER,  status),
              //new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_STATUS_FILTER,  status),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CATEGORY,  category == 0 ? (object) DBNull.Value : category),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_GROUP,  group == 0 ? (object) DBNull.Value : group),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IS_PENDING,  pending == null ? (object) DBNull.Value : pending),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_VENDOR_TEXT,  Customer == string.Empty ? (object) DBNull.Value : Customer),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_PO_NO,  PoNo == string.Empty ? (object) DBNull.Value : PoNo),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_GRN_NO,  grnNo == null ? (object) DBNull.Value : grnNo),
              //new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IS_FILTER_DUE_DT,  due == null ? (object) DBNull.Value : due),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_DUE_DATE, due == null || due == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(due)),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_COMPANY,  cmpPk > 0 ? cmpPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_CONVERT,  grid.ConvertTo < 0 ? (object)DBNull.Value : grid.ConvertTo),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_POH_MENU_TYPE,  grid.POType > 0 ? grid.POType : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CONV_SOH_NO,  grid.SCNo == string.Empty ? (object) DBNull.Value : "%" + grid.SCNo + "%"),
            };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.GETINVOICELIST, colParameters);
            return dsList;
        }

        /// <summary>
        /// Get Purchase Invoice List 
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="VendorPk"></param>
        /// <param name="XmlInvPks"></param>
        /// <returns></returns>
        public static DataSet GetPOInvoiceList(User objUser, int VendorPk, string XmlInvPks)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_VENDOR,  VendorPk),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, XmlInvPks)
            };

            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_PYMT_GET_LIST, colParameters);
            return dsList;
        }

        /// <summary>
        /// Get Purchase Invoice List 
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="XmlInvPks"></param>
        /// <param name="pohGroup"></param>
        /// <returns></returns>
        public static DataSet GetPOList(User objUser, string XmlInvPks, int pohGroup = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, XmlInvPks),
              new DBService.Parameters("P_POH_GROUP", pohGroup>0 ? pohGroup : (object) DBNull.Value),
            };

            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPPUR_ORDER_INVOICE_VND_GET_LIST, colParameters);
            return dsList;
        }
        /// <summary>
        /// Get Po/Wo details 
        /// </summary>
        /// <param name="pohGroup"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetOrderDetails(int id, int pohGroup = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters("P_PK",id),
              new DBService.Parameters("P_GROUP", pohGroup>0 ? pohGroup : (object) DBNull.Value),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPPUR_ORDER_DTL_WO_ITEM_BOM_DTL_GET_LIST, colParameters);
        }
        /// <summary>
        /// Get Po/Wo details 
        /// </summary>
        /// <param name="pohGroup"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetGRNDetails(int id, int pohGroup = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters("P_PK",id),
              new DBService.Parameters("P_GROUP", pohGroup>0 ? pohGroup : (object) DBNull.Value),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPINV_GRN_DTL_PO_WO_GET_LIST, colParameters);
        }


        /// <summary>
        /// Delete PO Invoice
        /// </summary>
        /// <param name="invPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeletePOInvoiceDetails(int invPK, DateTime lastModDate, string appType, string currentUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK, invPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER , currentUser),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.APP_TYPE, appType),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.DELETEINVOICEDETAILS, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get Purchase Invoice Details  For Report
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPurchaseInvoiceDtls(int pid, string PurchaseInvoiceOutRPTSP)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("P_IVH_PK",  pid)

            };
            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, PurchaseInvoiceOutRPTSP, colParameters);
            return dsActualPlndLinAloc;
        }


        /// <summary>
        /// Get Purchase Invoice Details  For Filling ddladdressstype
        /// </summary>
        /// <param name=" "></param>
        /// <returns>Datatable</returns>
        public static DataSet GetAddressTypeDtls(User objUser, int ACTIVE, int VendorPk, int type)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, ACTIVE),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
               new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_VEN_PK,  VendorPk == 0 ? (object) DBNull.Value :  VendorPk),
                 new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_VNC_TYPE,  type == 0 ? (object) DBNull.Value :  type),
                 new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_VNC_PK,   (object) DBNull.Value)

            };
            DataSet dsAddressType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.GETADDRESSTYPE, colParameters);
            return dsAddressType;
        }

        public static DataSet GetVendorAddressTypes(User objUser, short ACTIVE, int VncPk, int VendorPK, int type)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, ACTIVE),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_VEN_PK,  VendorPK == 0 ? (object) DBNull.Value :  VendorPK),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_VNC_TYPE,  type == 0 ? (object) DBNull.Value :  type),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_VNC_PK,  VncPk == 0 ? (object) DBNull.Value :  VncPk)

            };
            DataSet dsAddressType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.GETADDRESSTYPE, colParameters);
            return dsAddressType;
        }
        public static DataSet GetLineitemTaxList(int paymentPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PDT_PAYMENT_TRX, paymentPK > 0 ? paymentPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PDT_PK,  (object)DBNull.Value)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_PAYMENT_VND_TAX_DTL_GET_KV, colParameters);
            return dsArchive;
        }

        public static int GetDebitCreditPost(string PKXml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK, PKXml),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_CRDR_NOTE_PURCHASE_VALIDATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }




        #region Invoice Multiple PO
        /// <summary>
        /// Check for valid PO
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int CheckforValidPO(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPCHECKVALIDPOs, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get Multiple PO Invoice Details
        /// </summary>       
        /// <returns></returns>
        public static string GetMultiplePOInvoiceHeader(int poPK, int invPK, string strxml)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.POH_PK,  poPK == 0 ? (object) DBNull.Value :  poPK),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK,  invPK == 0 ? (object) DBNull.Value :  invPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPMULTIPLEPOGET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        /// <summary>
        /// Get Multiple Adv. PO Invoice Details
        /// </summary>       
        /// <returns></returns>
        public static string GetMultiplePOInvoiceHeader(int invPK, string strxml)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            { 
                //new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.POH_PK,  poPK == 0 ? (object) DBNull.Value :  poPK),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK,  invPK == 0 ? (object) DBNull.Value :  invPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPMULTIPLEADVPOGET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
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
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, ACTIVE),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CFG_TYPE, type)

            };
            DataSet dsAddressType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.GETPAYMENTTYPE, colParameters);
            DataTable dtPaymentType = dsAddressType.Tables[0];
            return dtPaymentType;
        }
        /// <summary>
        /// Get Expense Advances
        /// </summary>
        /// <param name="venPK"></param>
        /// <param name="transactionPK"></param>
        /// <returns></returns>
        public static DataTable GetExpenseAdvances(int venPK, int transactionPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_VEN_PK,  venPK == 0 ? (object) DBNull.Value :  venPK),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK,  transactionPK == 0 ? (object) DBNull.Value :  transactionPK),

            };
            DataSet dsAddressType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.GETEXPENSEADVANCE, colParameters);
            DataTable dtPaymentType = dsAddressType.Tables[0];
            return dtPaymentType;
        }


        public static DataTable GetBalanceAmountDetails(long InvoicePK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_ICH_PK,  InvoicePK == 0 ? (object) DBNull.Value :  InvoicePK)
            };
            DataSet dsBalanceDetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.GETBALANCEDETAILS, colParameters);
            DataTable dtBalanceDetails = dsBalanceDetails.Tables[0];
            return dtBalanceDetails;
        }


        public static DataTable GetPoWoNumbers(string searchKey, int? EnblWO, int BizUnit)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.POInvoicing.Parameters.P_IsEnableWo, EnblWO) ,
                new DBService.Parameters( GTIService.Constants.POInvoicing.Parameters.P_BIZUNIT, BizUnit) ,
                new DBService.Parameters( GTIService.Constants.POInvoicing.Parameters.P_VALUE , searchKey )
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPPUR_ORDER_WORK_ORDER_AUTO, colParameters);
        }



        public static int SaveInvoiceArchiveDetails(long InvoicePK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK, InvoicePK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_ARCHIVE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int SaveSalesInvoiceArchiveDetails(long InvoicePK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_ICH_PK, InvoicePK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_CUS_ARCHIVE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        public static int ValidateAdvanceInvType(string XML, int InvoicePK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_XML, XML),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK, InvoicePK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPPUR_ORDER_INVOICE_GROUP_VAL, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int SaveDRCR_Note_ArchiveDetails(long InvoicePK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CDH_PK, InvoicePK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_CRDR_NOTE_ARCHIVE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Get PO list
        /// </summary>
        /// <param name="@P_POH_PK"></param>
        /// <param name="@P_ACTIVE"></param>
        /// <param name="@P_POH_GROUP"></param>
        /// <param name="@P_POH_VENDOR"></param>     
        /// <param name="@P_TRX_STATUS"></param>
        /// <param name="@P_BIZUNIT"></param>
        /// <param name="@P_FROMDATE"></param>
        /// <param name="@P_TODATE"></param>
        /// <returns></returns>
        public static DataTable GetPurchaseOrderList(int POH_PK, int ACTIVE, int POH_GROUP, int POH_VENDOR, int TRX_STATUS, int BIZUNIT, DateTime? FROMDATE, DateTime? TODATE, string SOH_NO, int PNO, int PSize, int POH_COMPANY, int P_IsEnableWo, int POH_DEPT = 0, int user = 0)
        {
            DataTable dtPOList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.POH_PK, POH_PK==0? (object) DBNull.Value:POH_PK),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_ACTIVE, ACTIVE),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_POH_GROUP, POH_GROUP),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_POH_VENDOR, POH_VENDOR==0?(object)DBNull.Value:POH_VENDOR),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_TRX_STATUS, TRX_STATUS),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_BIZUNIT, BIZUNIT),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_FROMDATE  ,FROMDATE.HasValue?FROMDATE: (object) DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_TODATE  , FROMDATE.HasValue?TODATE: (object) DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_SOH_NO,  string.IsNullOrEmpty(SOH_NO)? (object) DBNull.Value :  SOH_NO),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PAGE_NUM,  PNO == 0 ? (object) DBNull.Value :  PNO),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PAGE_SIZE,  PSize == 0 ? (object) DBNull.Value :  PSize),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_POH_COMPANY, POH_COMPANY == -1 ? (object)DBNull.Value : POH_COMPANY),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_POH_DEPT, POH_DEPT >0 ? POH_DEPT: (object)DBNull.Value ),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_USER_PK, user > 0 ? user : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IsEnableWo,P_IsEnableWo),
            };
            dtPOList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPPUR_ORDER_GET_LIST, colParameters).Tables[0];
            return dtPOList;
        }

        #region Trading
        /// <summary>
        /// Save PO Invoice Trading Workflow
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SavePOInvoiceTradingWkf(string strxml, out string POInvoiceNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_TRADING_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            POInvoiceNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            return result;
        }
        /// <summary>
        /// Delete PO Invoice Trading
        /// </summary>
        /// <param name="invPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeletePOInvoiceTradingDetails(int invPK, DateTime lastModDate, string appType, string currentUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK, invPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER , currentUser),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.APP_TYPE, appType),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_TRADING_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get PO Invoice Trading Details
        /// </summary>
        /// <param name="poPK"></param>
        /// <param name="invPK"></param>
        /// <returns></returns>
        public static string GetPOInvoiceTradingHeader(int poPK, int invPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                //new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.POH_PK,  poPK == 0 ? (object) DBNull.Value :  poPK),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK,  invPK == 0 ? (object) DBNull.Value :  invPK)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_TRADING_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// Get Purchase Invoice Trading List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPOInvoiceTradingList(GridPrams grid, User objUser, int cusID, int InvPk, int PoPk, string Customer, string PoNo, string pageUrl, int type = 0, int status = 0, byte group = 0, byte category = 1, byte? pending = null, string grnNo = null, string due = null, int cmpPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy =="IVH_PK"?grid.SearchValue:grid.SearchValue+"%")),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber==0?1:grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize==0?200: grid.PageSize),
              new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_USER_PK,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  pageUrl),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.POD_PO,  PoPk == 0 ? (object) DBNull.Value :  PoPk),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_TYPE,  type == 0 ? (object) DBNull.Value :  type),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_STATUS_FILTER,  status),
              //new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_STATUS_FILTER,  status),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CATEGORY,  category == 0 ? (object) DBNull.Value : category),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_GROUP,  group == 0 ? (object) DBNull.Value : group),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IS_PENDING,  pending == null ? (object) DBNull.Value : pending),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_VENDOR_TEXT,  Customer == string.Empty ? (object) DBNull.Value : Customer),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_PO_NO,  PoNo == string.Empty ? (object) DBNull.Value : PoNo),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_GRN_NO,  grnNo == null ? (object) DBNull.Value : grnNo),
              //new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IS_FILTER_DUE_DT,  due == null ? (object) DBNull.Value : due),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_DUE_DATE, due == null || due == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(due)),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_COMPANY,  cmpPk > 0 ? cmpPk : (object)DBNull.Value),
            };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_TRADING_GET_LIST, colParameters);
            return dsList;
        }
        /// <summary>
        /// Method to get DirectInvoice No AutoComplete
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetDirectInvoiceNoAutoComplete(string searchValue, User objUser, int? InvCategory, int? InvGroup)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE  ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT  ,  objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_GROUP  ,  (InvGroup.HasValue && InvGroup > 0) ? InvGroup : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CATEGORY  , (InvCategory.HasValue && InvCategory > 0) ? InvCategory : (object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_PUR_DIR_INVOICE_NO_AUTO, colParameters).Tables[0];
        }

        public static string GetDirectPurchaseInvoiceHeaderMUL(string strxml, int invPK, int isAdvanceInvoice = 0)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, string.IsNullOrEmpty(strxml) ? (object) DBNull.Value : strxml),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.POH_PK,  (object) DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK,  invPK == 0 ? (object) DBNull.Value :  invPK),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_IS_ADV,  isAdvanceInvoice)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_MULTIPLE_PO_TRADING_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        #endregion

        #region Purchase Invoice Trading
        public static DataTable GetPendingPOList(int venPK, int? poPK, int? InvoicePk, int pageNumber, int pageSize, DateTime? FromDate, DateTime? ToDate)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_POH_VENDOR, venPK),  // > 0 ? venPK : (object)DBNull.Value  [Bug ID:  37685]
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.POH_PK, (poPK.HasValue && poPK > 0) ? poPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK, (InvoicePk.HasValue && InvoicePk > 0) ? InvoicePk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM, pageNumber),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, pageSize),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_FROMDATE, FromDate.HasValue ? FromDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_TODATE, ToDate.HasValue ? ToDate : (object)DBNull.Value),
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_TRADING_PEND_PO_GET, colParameters);
            return dtResult;
        }


        public static DataTable GetPurchaseOrderNumbers(int bizUnit, string searchValue, int vendorPk, int? InvoicePk)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_POH_VENDOR,  vendorPk > 0 ? vendorPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.POH_PK, (InvoicePk.HasValue && InvoicePk > 0) ? InvoicePk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_VALUE, searchValue),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_BIZUNIT, bizUnit)
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_TRADING_PEND_PO_AUTO, colParameters);
            return dtResult;
        }
        public static DataTable GetPendingInvoiceNoTrading(int bizUnit, string searchValue, int? invCategory, int? invGroup, int? invType = null, int? vendorPk = null)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_CATEGORY, (invCategory.HasValue && invCategory > 0) ? invCategory : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_GROUP, (invGroup.HasValue && invGroup > 0) ? invGroup : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_TYPE, invType > 0 ? invType : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_VENDOR, (vendorPk.HasValue && vendorPk > 0) ? vendorPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_VALUE, searchValue),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_BIZUNIT, bizUnit)
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_TRADING_INVOICE_NO_AUTO, colParameters);
            return dtResult;
        }
        #endregion


        public static DataTable GetInvPayments(int InvPk)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK, InvPk)
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_PAYMENT_GET, colParameters).Tables[0];
            return dtResult;
        }
        public static DataTable GetVendorDetails(int venPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters( GTIService.Constants.Vendor.Parameters.VENDRSPK ,  venPk),

            };
            DataTable dtvendor = new DataTable();
            dtvendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORDTL, colParameters).Tables[0];
            return dtvendor;
        }

        public static DataTable GetPOCostCenterDetails(int dtlPk)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_POD_PK, dtlPk)
            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INV_COST_CENTER_GET, colParameters).Tables[0];
            return dtResult;
        }

        /// <summary>
        /// Get PO types
        /// </summary>
        /// <param name="ACTIVE"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static DataTable GetPOTypes(int ACTIVE, int pk, string type)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, ACTIVE),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CfgPK, pk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CFG_TYPE, type)
            };
            DataSet dsAddressType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.GETPAYMENTTYPE, colParameters);
            DataTable dtPaymentType = dsAddressType.Tables[0];
            return dtPaymentType;
        }

        public static Dictionary<string, object> SaveInvoiceConvert(string strxml, int POSBU, int SCSBU) //int invoicePK
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK, strxml),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PO_BIZUNIT, POSBU),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_SO_BIZUNIT, SCSBU),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PO_RET_NO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_SO_RET_NO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPINV_INVOICE_CONVERT_SAVE, colParameters);
            //DataSet dsData = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPINV_INVOICE_CONVERT_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            string PONo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.POInvoicing.Parameters.P_PO_RET_NO]).Value.ToString();
            string SCNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.POInvoicing.Parameters.P_SO_RET_NO]).Value.ToString();

            Dictionary<string, object> returnData = new Dictionary<string, object>();
            returnData.Add("result", result);
            returnData.Add("PONo", PONo);
            returnData.Add("SCNo", SCNo);

            return returnData;// result;
        }

        public static string GetPOListByPK(string strxml, int BizUnit)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, BizUnit)
            };

            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPPUR_ORDER_DTL_NOS_GET, colParameters).Tables[0];
            for (int i = 0; i < dtResult.Rows.Count; i++)
                strRetVal += dtResult.Rows[i][0].ToString();
            return strRetVal;
        }

        public static int SaveInvoice(string strXml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_XML, strXml),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_RET_VAL, 0, 20, ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_RET_NO , string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
             };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_ADV_WKF_SAVE, colParameters);
            string invNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_NO]).Value.ToString();
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static int SaveAdvanceInvoiceWkf(string strXml, out string invNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_XML, strXml),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_RET_VAL, 0, 20, ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_RET_NO , string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
             };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_ADV_WKF_SAVE, colParameters);
            invNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_NO]).Value.ToString();
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.WorkOrder.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static string GetAdvInvoiceHeader(int poPK, int invPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK,  invPK == 0 ? (object) DBNull.Value :  invPK)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_ADV_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static string GetAdvInvoiceList(GridPrams grid, User objUser, int cusID, int InvPk, int PoPk, string Customer, string PoNo, string pageUrl, string payByFrom, string payByTo, out int TotalRecords, int type = 0, int status = 0, int group = 0, byte category = 1, byte? pending = null, string grnNo = null, string due = null, int cmpPk = 0)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy =="IVH_PK"?grid.SearchValue:grid.SearchValue+"%")),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber==0?1:grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize==0?200: grid.PageSize),
              new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_USER_PK,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  pageUrl),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.POD_PO,  PoPk == 0 ? (object) DBNull.Value :  PoPk),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_TYPE,  type == 0 ? (object) DBNull.Value :  type),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_STATUS_FILTER,  status),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CATEGORY,  category == 0 ? (object) DBNull.Value : category),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_GROUP,  group == 0 ? (object) DBNull.Value : group),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IS_PENDING,  pending == null ? (object) DBNull.Value : pending),
             // new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_VENDOR_TEXT,  Customer == string.Empty ? (object) DBNull.Value : Customer),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_VEN_PK,  cusID > 0 ?cusID: (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_PO_NO,  PoNo == string.Empty ? (object) DBNull.Value : PoNo),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_GRN_NO,  grnNo == null ? (object) DBNull.Value : grnNo),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_DUE_DATE, due == null || due == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(due)),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_COMPANY,  cmpPk > 0 ? cmpPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_CONVERT,  grid.ConvertTo < 0 ? (object)DBNull.Value : grid.ConvertTo),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_POH_MENU_TYPE,  grid.POType > 0 ? grid.POType : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CONV_SOH_NO,  grid.SCNo == string.Empty ? (object) DBNull.Value : "%" + grid.SCNo + "%"),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_DATE_PAY_BY_FROM  , payByFrom == string.Empty ? (object) DBNull.Value : Convert.ToDateTime(payByFrom)),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_DATE_PAY_BY_TO  , payByTo == string.Empty ? (object) DBNull.Value : Convert.ToDateTime(payByTo))
            };
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_ADV_GET_LIST, colParameters);
            DataTable dtxml = dsResult.Tables[1];
            TotalRecords = Convert.ToInt32(dsResult.Tables[0].Rows[0][0]);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        public static DataSet GetDailyInspectionDetails(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters("@P_TIH_PK", pk),
            };
            DataSet dsReport = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SP_GET_DAILY_INSPECTION_DETAILS_REPORT, colParameters);
            return dsReport;
        }
        public static DataSet GetRMIPMDetails(int pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters("@P_TIH_PK", pk),
            };
            DataSet dsReport = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPQUC_TEST_TRX_ITEM_PM_DTL_RPT, colParameters);
            return dsReport;
        }
    }
}
