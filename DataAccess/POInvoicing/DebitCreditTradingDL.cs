using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;


namespace DataAccess.POInvoicing
{
   public class DebitCreditTradingDL
    {
        /// <summary>
        /// Get DebitCredit Trading List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetDebitCreditTradingList(GridPrams grid, User objUser, int vendorID, int crdrPk, string invNo, string pageUrl, int type = 0, int status = 0, int cmpPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy =="CDH_PK"?grid.SearchValue:grid.SearchValue+"%")),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_USER_PK,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  pageUrl),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CDH_TYPE,  type == 0 ? (object) DBNull.Value :  type),             
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_STATUS_FILTER,  status),   
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_NO,  invNo == string.Empty ? (object) DBNull.Value : invNo),   
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CDH_VENDOR,  vendorID > 0 ? vendorID : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CDH_COMPANY,  cmpPk > 0 ? cmpPk : (object)DBNull.Value)
            };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_CRDR_VND_GET_LIST, colParameters);
            return dsList;
        }

        public static DataTable GetPendingInvList(User objUser, int venPK, int? invPK, int? drcrPk, int pageNumber, int pageSize, DateTime? FromDate, DateTime? ToDate,int invCategory, int invType)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_VENDOR, venPK>0?venPK:(object)DBNull.Value),      
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.IVH_PK, (invPK.HasValue && invPK > 0) ? invPK : (object)DBNull.Value),             
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO, pageNumber),      
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, pageSize),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_FROMDATE, FromDate.HasValue ? FromDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_TODATE, ToDate.HasValue ? ToDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_CATEGORY, invCategory > 0 ? invCategory : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_TYPE, invType > 0 ? invType : (object)DBNull.Value),    
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_TRADING_CRDR_PENDING_GET, colParameters);
            return dtResult;
        }
        public static DataTable GetPendingPurchaseInvNumbers(int bizUnit, string searchValue, string vendorPk,int? invCategory,int? invType=null)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_CATEGORY, (invCategory.HasValue && invCategory > 0) ? invCategory : (object)DBNull.Value),                     
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_TYPE, invType > 0 ? invType : (object)DBNull.Value),    
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_VENDOR, string.IsNullOrEmpty(vendorPk) ? (object)DBNull.Value : vendorPk),  
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_VALUE, searchValue),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_BIZUNIT, bizUnit)   
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_INVOICE_VND_TRADING_CRDR_AUTO, colParameters);
            return dtResult;
        }
        public static string GetDebitCreditHeaderMUL(string strxml, int cdhPK, byte IsTaxForOtherCharge)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, string.IsNullOrEmpty(strxml) ? (object) DBNull.Value : strxml),                
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CDH_PK,  cdhPK == 0 ? (object) DBNull.Value :  cdhPK),
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_OTHR_CHRG_TAX,  IsTaxForOtherCharge == 0 ? (object) DBNull.Value :  IsTaxForOtherCharge)  
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_CRDR_NOTE_VND_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// Save Credit/Debit Trading Workflow
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SaveCreditDebitTradingWkf(string strxml, out string CrDrNo)
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
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_CRDR_VND_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            CrDrNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            return result;
        }

        /// <summary>
        /// Delete PO Invoice Trading
        /// </summary>
        /// <param name="invPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteCreditDebitTradingDetails(int crdrPk, DateTime lastModDate, string appType, string currentUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CDH_PK, crdrPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),              
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_CRDR_NOTE_HDR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to get DirectInvoice No AutoComplete
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetDrCrNoteNoTradingAutoComplete(string searchValue, User objUser, int type)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE  ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT  ,  objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_TYPE  ,   type > 0 ? type : (object)DBNull.Value)              
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_CRDR_NOTE_NO_AUTO, colParameters).Tables[0];
        }
        public static bool ValidationForDRCRCancellation(int CurrPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CDH_PK, CurrPK > 0 ? CurrPK : (object)DBNull.Value) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_CRDR_NOTE_VND_CAN_CHECK, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result > 0 ;
        }
    }
}
