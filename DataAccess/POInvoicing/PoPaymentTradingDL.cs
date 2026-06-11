using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.POInvoicing
{
   public class PoPaymentTradingDL
    {
        /// <summary>
        /// Get Payment Trading List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
       public static DataSet GetPaymentTradingList(GridPrams grid, User objUser, int vendorID, int paymentPk, string invNo, string pageUrl,int status = 0, int PDCStatus = 0,int cmpPk = 0)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy =="PVH_PK"?grid.SearchValue:grid.SearchValue+"%")),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),      
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  pageUrl),
              new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_USER_PK,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),        
               new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_STATUS_FILTER,  status),  
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PDC_STATUS,  PDCStatus> 0 ? PDCStatus : (object)DBNull.Value),             
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_NO,  invNo == string.Empty ? (object) DBNull.Value : invNo),   
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PVH_VENDOR,  vendorID > 0 ? vendorID : (object)DBNull.Value),
             // new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PVH_CATEGORY,  vendorID > 0 ? vendorID : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PVH_COMPANY,  cmpPk > 0 ? cmpPk : (object)DBNull.Value)
            };
           DataSet dsList = new DataSet();
           dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_PAYMENT_VND_HDR_GET_LIST, colParameters);
           return dsList;
       }

       public static DataTable GetPendingInvList(User objUser, int venPK, int? invPK, int pageNumber, int pageSize, DateTime? FromDate, DateTime? ToDate, int invCategory, int invType)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_VENDOR,venPK>0?venPK:(object)DBNull.Value),      
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK, (invPK.HasValue && invPK > 0) ? invPK : (object)DBNull.Value),               
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM, pageNumber),      
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, pageSize),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_FROMDATE, FromDate.HasValue ? FromDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_TODATE, ToDate.HasValue ? ToDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_CATEGORY, invCategory > 0 ? invCategory : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_TYPE, invType > 0 ? invType : (object)DBNull.Value),    
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_PAYMENT_VND_PEND_INVOICE_GET, colParameters);
            return dtResult;
        }

        public static string GetPaymentHeaderMUL(string strxml, int cdhPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, string.IsNullOrEmpty(strxml) ? (object) DBNull.Value : strxml),                
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PVH_PK,  cdhPK == 0 ? (object) DBNull.Value :  cdhPK)               
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_PAYMENT_VND_HDR_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        /// <summary>
        /// Save Payment Trading Workflow
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SavePaymentTradingWkf(string strxml, out string paymentNo)
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
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_PAYMENT_VND_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            paymentNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            return result;
        }

        public static DataTable GetVendorDebitNoteAlcnForPaymentAdjn(int venPK, int pvmPk)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;          
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_VEN_PK, venPK>0?venPK:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PVM_PK, (pvmPk> 0) ? pvmPk : (object)DBNull.Value) 
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_PAYMENT_VND_ALCN_GET, colParameters);
            return dtResult;           
        }
        /// <summary>
        /// Delete Payment Trading
        /// </summary>
        /// <param name="invPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeletePaymentTradingDetails(int pvhPK, DateTime lastModDate, string appType, string currentUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_PVH_PK, pvhPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER , currentUser),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.APP_TYPE, appType),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_PAYMENT_VND_HDR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to get Payment Trading No AutoComplete
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetPaymentNoTradingAutoComplete(string searchValue, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE  ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT  ,  objUser.SBUID)                
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_PAYMENT_TRADING_NO_AUTO, colParameters).Tables[0];
        }
    }
}
