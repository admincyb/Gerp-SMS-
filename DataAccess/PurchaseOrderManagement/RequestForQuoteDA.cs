using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.PurchaseOrderManagement;
using BusinessObject.CommonManagement;
using GTIService.Constants.Common;

namespace DataAccess.PurchaseOrderManagement
{
    public class RequestForQuoteDA
    {
        #region Methods
        /// <summary>
        /// Get Purchase Request List Details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPurchaseRequestList(GridPrams grid, User objUser, int procID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue == string.Empty ? "%" : grid.SearchValue + "%"),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) )
            };

            DataSet dsPurchaseRqstList = new DataSet();
            dsPurchaseRqstList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.GETRFQPRLIST, colParameters);
            return dsPurchaseRqstList;
        }

        /// <summary>
        /// 'Get Purchase Request AutoComplete Details 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetItemPRSearchValues(string searchBy, string searchValue, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY ,searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE ,searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT ,objUser.SBUID), 
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.GETITEMPURCHASEREQUESTAUTO, colParameters).Tables[0];
            return dtSearchValue;
        }

        /// <summary>
        /// Get Vendor List By RFQPK
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <returns></returns>
        public static DataSet GetVendorList(int? rfhPK, int? vendorPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RFH_PK,  rfhPK),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_VEN_PK,  vendorPK)
                                          
            };

            DataSet dsVendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.GETRFQVENDORLIST, colParameters);
            return dsVendor;


        }

        /// <summary>
        /// Get Vendor List By PR Details
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static DataSet GetVendorListbyPR(string xml)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xml)
            };
            DataSet dsVendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.GETVENDORBYPR, colParameters);
            return dsVendor;
        }

        /// <summary>
        /// Function to get RFQ response
        /// </summary>
        /// <param name="rfhPK"></param>
        /// <param name="vendorPK"></param>
        /// <param name="rrhPK"></param>
        /// <returns></returns>
        public static string GetRFQResponse(int rfhPK, int vendorPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RFH_PK, rfhPK),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RRH_VENDOR, vendorPK<=0?DBNull.Value:(object)vendorPK)
               
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.GETRFQRESPONSE, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        #endregion

        public static DataSet GetRFQHeader(int rfhPK, short active, int bizUnit)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RFH_PK,  rfhPK),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_ACTIVE,  active),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_BIZUNIT,  bizUnit)
                                          
            };

            DataSet dsVendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.GETRFQHEADER, colParameters);
            return dsVendor;

        }
        /// <summary>
        /// Get Purchase Request Header Details
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="rfqHdrPK"></param>
        /// <returns></returns>
        public static DataSet GetRFQHeader(string xml, int rfqHdrPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, rfqHdrPK > 0 ? (object)DBNull.Value : xml),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RFH_PK, rfqHdrPK > 0 ? rfqHdrPK : (object)DBNull.Value)
            };
            DataSet dsRFQ = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.GETRFQHDR, colParameters);
            return dsRFQ;
        }

        /// <summary>
        /// Save RFQ Response Details
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SaveRFQResponseDetails(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.SAVERFQRESPONSE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;

        }
        /// <summary>
        /// Save RFQ
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveRFQDetails(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.SAVERFQ, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Delete RFQ
        /// </summary>
        /// <param name="rfhPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteRFQDetails(int rfhPK, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RFH_PK, rfhPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, (object)DBNull.Value), 
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.DELETERFQ, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
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
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_APT_CODE, (int)AppType.RequestForQuote),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_AST_VALUE, subType > 0 ? subType : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DEPT,  dept > 0 ? dept : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER, user),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DATE, DateTime.Now),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_UPDATE, 0),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_APP_PK, 0),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            DataSet dsTrxNo = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.GET_TRX_DOC_NO, colParameters);
            string result = dsTrxNo != null && dsTrxNo.Tables.Count > 0 ? Convert.ToString(dsTrxNo.Tables[0].Rows[0][0]) : string.Empty;
            return result;
        }

        /// <summary>
        /// Get Item Rates
        /// </summary>
        /// <param name="itemPK"></param>
        /// <param name="vendorPK"></param>
        /// <returns></returns>
        public static DataSet GetItemRates(int itemPK, int vendorPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_ITM_PK, itemPK > 0 ? itemPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_VEN_PK,  vendorPK > 0 ? vendorPK : (object)DBNull.Value)
            };
            DataSet dsItemRates = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.GET_ITEM_RATES, colParameters);
            return dsItemRates;
        }

        /// <summary>
        /// Get Purchase Request List Details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetRFQList(GridPrams grid, User objUser, int procID,int userPK,int deptPK=0) 
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                         
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? "%" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? "%" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_PROC_ID,  procID),
              new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_USER_PK,  userPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RFH_DEPT,  deptPK>0?deptPK:(Object)DBNull.Value),
             
            };

            DataSet dsRFQList = new DataSet();
            dsRFQList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.RFQGETLIST, colParameters);
            return dsRFQList;
        }

        /// <summary>
        /// Get RFQ Tax Details
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="category"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataSet GetRFQTaxDetails(int taxPK, int category, int bizUnit, byte active,int subCategory)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, active),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXPK, taxPK == 0 ? (object) DBNull.Value : taxPK ),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXCATEGORY, category == 0 ? (object) DBNull.Value : category),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_SUB_CATEGORY, subCategory == 0 ? (object) DBNull.Value : subCategory),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)          
            };
            DataSet dsRFQTaxDetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.CATEGORYVALUEGET, colParameters);
            return dsRFQTaxDetails;
        }

        public static DataSet GetExchangeRate(int fromCurrency, int toCurrency, DateTime date)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_FROM, fromCurrency),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_TO,  toCurrency),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_DATE,  date)

            };
            DataSet dsExchangeRate = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.SPADM_CURRENCY_CONV_FACT_GET, colParameters);
            return dsExchangeRate;
        }
        

        //public static DataTable GetExchangeRates(int fromCurrency, int toCurrency, DateTime date)
        //{
        //    DBService dbService = new DBService();
        //    DBService.Parameters[] colParameters = null;
        //    colParameters = new DBService.Parameters[] 
        //    {   
        //        new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_FROM, fromCurrency),
        //        new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_TO,  toCurrency),
        //        new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_DATE,  date)

        //    };
        //    DataSet dsExchangeRate = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.SPADM_CURRENCY_CONV_FACT_GET, colParameters);
 
        //}
        public static DataSet GetRFQReportDetails(int RecPK, int VndPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RFH_PK, RecPK),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RRH_VENDOR,  VndPK),
                

            };
            DataSet dsRFQ = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.SPPUR_RFQ_RPT, colParameters);
            return dsRFQ;
        }

        public static DataSet GetRFQAmtCompReportDetails(int RecPK, int CmpType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RFH_PK, RecPK),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CMP_TYPE, CmpType),
            };
            DataSet dsRFQ = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.SPPUR_RFQ_AMT_CMP_RPT, colParameters);
            return dsRFQ;
        }
        /// <summary>
        /// 'Get Purchase Request AutoComplete Details 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetPURRFQAuto(string searchBy, string searchValue, User objUser, string pageURL)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY ,searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE ,searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT ,objUser.SBUID), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL  ,  pageURL),  
              new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_USER_PK,  objUser.PKUser)
   
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.SPPUR_RFQ_AUTO, colParameters).Tables[0];
            return dtSearchValue;
        }
    }
}
