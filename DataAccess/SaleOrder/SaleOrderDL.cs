using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using gErpProductionPlanning.ClassLibrary;
using ERP.Utilities;
using DataAccess;
using BusinessObject.Constants;
using BusinessObject;
using BusinessObject.Sales;


namespace DataAccess.ProductionDL
{
    public class SaleOrderDL
    {
        /// <summary>
        /// method to get saleorder details
        /// </summary>
        /// <param name="pk"></param>
        public static DataSet GetSaleOrder(int wid, string[] XML)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(SaleOrderDA.P_WID, wid),
                new DBService.Parameters(SaleOrderDA.P_XML_ORD_LST, XML[0]==string.Empty? DBNull.Value.ToString():XML[0]),
                new DBService.Parameters(SaleOrderDA.P_XML_PLD_DTL, XML[1]==string.Empty? DBNull.Value.ToString():XML[1]),
                new DBService.Parameters(SaleOrderDA.P_XML_PDT_MST, XML[2]==string.Empty? DBNull.Value.ToString():XML[2]),
                new DBService.Parameters(SaleOrderDA.P_XML_SIZ_MST, XML[3]==string.Empty? DBNull.Value.ToString():XML[3]),
                new DBService.Parameters(SaleOrderDA.P_XML_CLR_MST, XML[4]==string.Empty? DBNull.Value.ToString():XML[4] ),
                new DBService.Parameters(SaleOrderDA.P_XML_PRO_MST, XML[5]==string.Empty? DBNull.Value.ToString():XML[5]),
                new DBService.Parameters(SaleOrderDA.P_XML_ORD_HDR, XML[6]==string.Empty? DBNull.Value.ToString():XML[6]),
                new DBService.Parameters(SaleOrderDA.P_XML_PLN_SSN, XML[7]==string.Empty? DBNull.Value.ToString():XML[7]),
                new DBService.Parameters(SaleOrderDA.P_XML_PGP_MST, XML[8]==string.Empty? DBNull.Value.ToString():XML[8]),
                new DBService.Parameters(SaleOrderDA.P_XML_CFG_LST, XML[9]==string.Empty? DBNull.Value.ToString():XML[9])
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SP_GetSaleOrder, colParameters);
            }
            return ds;

        }

        /// <summary>
        /// Save Sale Order for planning
        /// </summary>
        /// <param name="strxml"></param>
        public static DataSet SaveSaleorder(int wid, string[] XML)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                 {                
                    new DBService.Parameters(AllocationDA.P_WID, wid),
                    new DBService.Parameters(AllocationDA.P_XML_PLN_SOD,  XML[0]==string.Empty? DBNull.Value.ToString():XML[0]),
                 };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SP_SavePlanningDtl, colParameters);
            }
            return ds;

        }

        /// <summary>
        /// Save Sale Order for planning
        /// </summary>
        /// <param name="strxml"></param>
        public static DataSet SaveOrderList(int wid, string[] XML)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                {                
                 new DBService.Parameters(AllocationDA.P_WID, wid),
                 new DBService.Parameters(AllocationDA.P_XML_ORD_LST,  XML[0]==string.Empty? DBNull.Value.ToString():XML[0])
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SP_SaveOrderList, colParameters);
            }
            return ds;

        }
        /// <summary>
        /// Monthly Sales List
        /// </summary>
        /// <returns></returns>
        public static DataSet MonthlySalesList()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            //colParameters = new DBService.Parameters[] 
            //{   
            //    new DBService.Parameters("SOH_PK",  saleOrderID)
                                          
            //};
            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, "SPPBI_FIN_MONTHLY_SALE_RPT", colParameters);
            return dsActualPlndLinAloc;


        }
        /// <summary>
        /// Get Sale Order Details By SO Number For Report
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSaleOrderDtls(int saleOrderID)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters("SOH_PK",  saleOrderID)
                                          
            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, "SPSAL_ORDER_GET_RPT", colParameters);
            return dsActualPlndLinAloc;


        }

        public static DataSet SaleOrderDetailsDOCNOREVISION(int saleOrderID,int reportpk)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("SOH_PK",  saleOrderID),
                new DBService.Parameters("P_AST_PK",  reportpk)

            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, "SPSAL_ORDER_GET_RPT", colParameters);
            return dsActualPlndLinAloc;


        }
        public static DataSet GetSaleOrderArchiveDtls(int saleOrderID, int versonID)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters("P_SOH_PK",  saleOrderID),
                new DBService.Parameters("P_SOH_VERSION ",  versonID)
                                          
            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, "SPSAL_ORDER_ARCHIVE_RPT", colParameters);
            return dsActualPlndLinAloc;


        }


        /// <summary>
        /// Get Sales Invoice Details By SO Number For Report
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalesInvoiceDtls(int saleOrderID, string SalesInvRptSp)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters("P_ICH_PK",  saleOrderID)
                                          
            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, SalesInvRptSp, colParameters);
            return dsActualPlndLinAloc;
        }


        public static DataSet GetSalesInvoiceDtlsDOCNOREVISION(int saleOrderID, string SalesInvRptSp, int reportpk)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("P_ICH_PK",  saleOrderID),
                new DBService.Parameters("P_AST_PK",  reportpk)

            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, SalesInvRptSp, colParameters);
            return dsActualPlndLinAloc;
        }
        /// <summary>
        /// Get Sales Invoice Details For Customs Report
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalesInvoiceCustomsDtls(int saleOrderID, string SalesInvRptSp, int reportpk)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters("P_ICH_PK",  saleOrderID),
                new DBService.Parameters("P_AST_PK",  reportpk==0?(object)DBNull.Value:reportpk)

            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, SalesInvRptSp, colParameters);
            return dsActualPlndLinAloc;
        }

        /// <summary>
        /// Get Sales Invoice Details By SO Number For Trading Report
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalesInvoiceTrdDtls(int saleOrderID, string SalesInvRptSp)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters("P_ICH_PK",  saleOrderID)
                                          
            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, SalesInvRptSp, colParameters);
            return dsActualPlndLinAloc;
        }
        /// <summary>
        /// Get Sale Order List
        /// </summary>
        /// <param name="objPageFilter"></param>
        /// <param name="objFields"></param>
        /// <returns></returns>
        public static DataSet GetSaleOrderList(ERP.Utilities.FilterUtility objPageFilter, SalesOrderBO objFields)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters("P_SOH_PK",  (objFields.PK !="0" ||objFields.PK !="") ? objFields.PK:(object)DBNull.Value),
                new DBService.Parameters("P_ACTIVE",  objFields.Active),
                new DBService.Parameters("P_PAGE_NUM",  objPageFilter.CurrentPage),
                new DBService.Parameters("P_PAGE_SIZE",  objPageFilter.PageSize),
                new DBService.Parameters("P_BIZUNIT",  objPageFilter.BizUnit),
                new DBService.Parameters("P_FROM_DT",  objPageFilter.FilterDate),
                new DBService.Parameters("P_TO_DT",  objPageFilter.FilterToDate),
                new DBService.Parameters("P_SOH_CUSTOMER",  objFields.CustomerPK),
                new DBService.Parameters("P_STATUS",  objFields.SOH_STATUS),//
                new DBService.Parameters("P_SOH_TYPE",  objFields.SOH_TYPE > 0 ? objFields.SOH_TYPE : (object)DBNull.Value),
                new DBService.Parameters("P_SOH_COMPANY",  objFields.SOH_COMPANY > 0 ? objFields.SOH_COMPANY : (object) DBNull.Value),
                new DBService.Parameters("P_USER_PK",  objFields.UserPK),
                new DBService.Parameters("P_SOH_NO",  (objFields.ScNoPK !="0"|| objFields.ScNoPK !="") ? objFields.ScNoPK:(object)DBNull.Value),
                new DBService.Parameters("P_HIDE_CONVERTED",  objFields.HideConverted == true ? 1 : 0)
            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SPSAL_ORDER_GET_LISTING, colParameters);
            return dsActualPlndLinAloc;

        }

        /// <summary>
        /// Get Sale Order Header
        /// </summary>
        /// <param name="quotPK"></param>
        /// <param name="sohPK"></param>
        /// <returns></returns>
        public static string GetSaleOrderHeader(int quotPK, int sohPK, int customerPK = 0)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.QTH_PK,  quotPK == 0 ? (object) DBNull.Value :  quotPK),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK,  sohPK == 0 ? (object) DBNull.Value :  sohPK),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.CUST_PK,  customerPK == 0 ? (object) DBNull.Value :  customerPK)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.GETSALEORDER, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }


        public static int? SaveSaleOrderDetails(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, SaleOrderDA.SAVESALEORDER, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Save Sale Order Short Close
        /// </summary>
        /// <param name="sohPK"></param>
        /// <param name="reason"></param>
        /// <param name="refNo"></param>
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static int? SaveSaleOrderShortClose(int sohPK, string reason, string refNo, int userPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK,  sohPK),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SHORT_CLS_REASON, reason),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SHORT_CLS_REFNO, refNo),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  userPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, SaleOrderDA.SAVESALEORDER_SHORT_CLS, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Save Sale Order Workflow Details
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static int? SaveSaleOrderWkfDetails(string xmlDoc, out int refID, out string retMsg)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RET_MSG,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, SaleOrderDA.SAVESALEORDERWKF, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            refID = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            retMsg=Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.P_RET_MSG]).Value);
            return result;
        }

        public static int DeleteSaleContractDetails(int scPK, DateTime lastModDate, string deleteReason = "")
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, scPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DELETEREASON, string.IsNullOrEmpty(deleteReason) ? (object) DBNull.Value :  deleteReason),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, SaleOrderDA.DELETESALEORDER, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Check Sale Contract Cancel
        /// </summary>
        /// <param name="scPK"></param>
        /// <returns></returns>
        public static bool SaleContractCancelCheck(int scPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, scPK > 0 ? scPK : (object)DBNull.Value)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SALEORDERCANCELCHECK, colParameters);
            return dsArchive == null || dsArchive.Tables.Count == 0 || dsArchive.Tables[0].Rows.Count == 0;
        }

        /// <summary>
        /// Get Purchase Invoice List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalesInvoiceList(GridPrams grid, User objUser, int cusID, int InvPk, int SoPk, string Customer, int InvType, string ScNo, string pageUrl, string DrCrNo, int? Status = null, int? Pending = null, byte group = 0, byte category = 1, string due = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {   
                         
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy =="ICH_PK"?grid.SearchValue:grid.SearchValue+"%")),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ICH_COMPANY, grid.CompanyPK> 0 ? grid.CompanyPK: (object) DBNull.Value),
              new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_USER_PK,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  pageUrl),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOD_SO,  SoPk == 0 ? (object) DBNull.Value :  SoPk),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IVH_TYPE,  type == 0 ? (object) DBNull.Value :  type),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.STATUSFILTER,  Status == null ? (object) DBNull.Value :  Status),
              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PendingAmt,  Pending == null ? (object) DBNull.Value :  Pending),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CATEGORY,  category == 0 ? (object) DBNull.Value : category),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_GROUP,  group ),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_CUSTOMER_TEXT,  Customer == string.Empty ? (object) DBNull.Value :  Customer),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_TYPE,  InvType < 0 ? (object) DBNull.Value :  InvType),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SOH_NO,  ScNo == string.Empty ? (object) DBNull.Value :  ScNo),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CRDR_NO,  DrCrNo == string.Empty ? (object) DBNull.Value :  DrCrNo),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_DUE_DATE, due == null || due == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(due))
            };

            DataSet dsRFQList = new DataSet();
            dsRFQList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETSALESINVOICELIST, colParameters);
            return dsRFQList;
        }
        public static DataSet GetRevisionHistory(int itemPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, itemPK > 0 ? itemPK : (object)DBNull.Value)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.GETARCHIVE, colParameters);
            return dsArchive;
        }
        public static DataSet GetLineitemTaxList(int receiptPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_RDT_RECEIPT_TRX, receiptPK > 0 ? receiptPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_RDT_PK,  (object)DBNull.Value)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SPFIN_RECEIPT_CUS_TAX_DTL_GET_KV, colParameters);
            return dsArchive;
        }

        public static string GetCustomsMapingDetails(int InvId)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK ,  InvId),  

            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.FIN_INVOICE_CUS_CUSTOMS_CHRG_GET_KV, colParameters).Tables[0];
            string xml = String.Empty;
            foreach (DataRow drBinInspection in dtResult.Rows)
            {
                xml += Convert.ToString(drBinInspection[0]);
            }
            return xml;

        }

        /// <summary>
        /// Gets Sale Order Dtls for Packing Hdr in SC listing
        /// </summary>
        /// <param name="strxml"></param>
        public static DataTable GetSaleOrderDetails(int ScPk)
        {
            DataTable dtSODtls = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                 {                
                    new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, ScPk)
                   
                 };
            dtSODtls = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SP_SALE_ORDER_DTL_GET, colParameters).Tables[0];
            return dtSODtls;
        }

        /// <summary>
        /// Gets Packing Dtls for SC listing
        /// </summary>
        /// <param name="strxml"></param>
        public static DataTable GetPackingDtls(int active, int SODtlPk, int pk = 0)
        {
            DataTable dtPackingDtls = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                 {                
                    new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_BPD_PK, pk),
                    new DBService.Parameters(GTIService.Constants.Sales.Parameters.ACTIVE, active),
                    new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_BPD_SO_DTL, SODtlPk),
                 };
            dtPackingDtls = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SP_BIN_CARD_PACK_DTL_GET_KV, colParameters).Tables[0];
            return dtPackingDtls;
        }


        public static int? SaleOrderMailSave(int SCPk, int RefId, int Type)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, SCPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_REF_PK, RefId),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_TYPE, Type),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, SaleOrderDA.SPSAL_ORDER_MAIL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable CheckReceiptAllocationInvoice(long receiptPk)
        {
            DataTable dtAllocationDtls = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                 {                
                    new DBService.Parameters(GTIService.Constants.Sales.Parameters.ICH_PK, receiptPk),                   
                 };
            dtAllocationDtls = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SPFIN_RECEIPT_CUS_RETURN_CHECK, colParameters).Tables[0];
            return dtAllocationDtls;
        }

        public static DataSet GetDirectSalesInvoiceList(GridPrams grid, User objUser, int cusID, int InvPk, int SoPk, string Customer, int InvType, string ScNo, string pageUrl, string DrCrNo, int? Status = null, int? Pending = null, byte group = 0, byte category = 1, string due = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {   
                         
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy =="ICH_PK"?grid.SearchValue:grid.SearchValue+"%")),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_USER_PK,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  pageUrl),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOD_SO,  SoPk == 0 ? (object) DBNull.Value :  SoPk),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IVH_TYPE,  type == 0 ? (object) DBNull.Value :  type),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.STATUSFILTER,  Status == null ? (object) DBNull.Value :  Status),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PendingAmt,  Pending == null ? (object) DBNull.Value :  Pending),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CATEGORY,  category == 0 ? (object) DBNull.Value : category),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_GROUP,  group ),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_CUSTOMER_TEXT,  Customer == string.Empty ? (object) DBNull.Value :  Customer),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_TYPE,  InvType < 0 ? (object) DBNull.Value :  InvType),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SOH_NO,  ScNo == string.Empty ? (object) DBNull.Value :  ScNo),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CRDR_NO,  DrCrNo == string.Empty ? (object) DBNull.Value :  DrCrNo),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_DUE_DATE, due == null || due == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(due)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO, grid.PageNumber),      
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, grid.PageSize)
            };

            DataSet dsRFQList = new DataSet();
            dsRFQList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_CUS_TRADING_GET_LIST, colParameters);
            return dsRFQList;
        }

        #region UpdateSaleOrderPlant
        /// <summary>
        /// Save Sale Order Short Close
        /// </summary>
        /// <param name="sohPK"></param>
        /// <param name="reason"></param>
        /// <param name="refNo"></param>
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static int? UpdateSaleOrderPlant(int sohPK, int userPK,int plantPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK,  sohPK),              
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  userPK),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_SOH_COMPANY,  plantPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, SaleOrderDA.SPSAL_ORDER_UPDATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        } 
        #endregion

        /// <summary>
        /// Method to get sales contract item reference details
        /// </summary>
        /// <param name="SohPk"></param>
        /// <param name="SodPk"></param>
        /// <returns></returns>
        public static string GetSaleContractItemRef(int SohPk, int SodPk)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK,  SohPk > 0 ? SohPk : (object) DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOD_PK,  SodPk > 0 ? SodPk : (object) DBNull.Value)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SPSAL_ORDER_DTL_REF_CHECK, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// To get Sales cost
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static DataTable GetSalesCostDetails(string xmlDoc)
        {
            DataTable dtSCcost = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                 {                
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc)
                   
                 };
            dtSCcost = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SPSAL_ORDER_COST_GET, colParameters).Tables[0];
            return dtSCcost;
        }

        /// <summary>
        /// To get suspence list for reciept 
        /// </summary>
        /// <param name="Bankpk"></param>
        /// <param name="receiptPk"></param>
        /// <returns></returns>
        public static DataTable GetSuspenceListforReciept(int Bankpk ,long receiptPk = 0)
        {
            DataTable dtList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                 {                
                    new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CBM_PK, Bankpk),                   
                    new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_RCH_PK, receiptPk > 0 ? receiptPk : (object)DBNull.Value),
                 };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SPFIN_RECEIPT_CUS_SUSP_LIST, colParameters).Tables[0];
            return dtList;
        }

        public static DataTable GetShipmentTermsBySaleOrder(int saleOrderPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, saleOrderPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, SaleOrderDA.SPSAL_ORDER_HDR_SHIPMENT_TERM_GET, colParameters).Tables[0];
        }
        /// <summary>
        /// Get Sales Invoice Details For Customs Report Shipping
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalesInvoiceCustomsDtlsShipping(int saleOrderID, string SalesInvRptSp)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("P_ICH_PK",  saleOrderID)

            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, SalesInvRptSp, colParameters);
            return dsActualPlndLinAloc;
        }
        public static DataSet GetSalesInvoiceCustomsShippingPlan(int saleOrderID, string SalesInvRptSp)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("P_SNH_PK",  saleOrderID)

            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, SalesInvRptSp, colParameters);
            return dsActualPlndLinAloc;
        }
        public static int GetDebitCreditPost(string PKXml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_RCH_PK, PKXml),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, SaleOrderDA.GETDEBITCREDITPOST, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}