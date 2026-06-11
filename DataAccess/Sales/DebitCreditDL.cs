using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.Sales
{
    public class DebitCreditDL
    {
        public static DataTable GetPendingInvoiceList(int custPK, long invPK, int invCategory, int invType, DateTime? FromDate, DateTime? ToDate, int bizUnit, int pageNumber, int pageSize)
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
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM, pageNumber),      
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, pageSize),     
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_BIZUNIT, bizUnit)   
            };
            dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_CRDR_CUS_PEND_INVOICE_GET, colParameters);
            return dtResult;
        }

        public static string GetTradingSalesDebitCreditNote(string xmlDoc, long DebitCreditPk, byte IsTaxForOtherCharge)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, string.IsNullOrEmpty(xmlDoc) ? (object) DBNull.Value : xmlDoc),               
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CDH_PK,  DebitCreditPk > 0 ? DebitCreditPk : (object) DBNull.Value ),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_OTHR_CHRG_TAX,  IsTaxForOtherCharge == 0 ? (object) DBNull.Value :  IsTaxForOtherCharge ) 
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_CRDR_NOTE_CUS_TRD_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static long? SaveCreditDebitSales(string xmlDoc, out string crdrNo)
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
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_CRDR_CUS_TRD_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            crdrNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            return result;
        }

        public static DataSet GetDebitCreditList(GridPrams grid, User objUser, int CusromerPk, int CrDrPk, string InvoiceNo, string PageUrl, int InvType, int CrDrType, int status, int cmpPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy =="CDH_PK"?grid.SearchValue:grid.SearchValue+"%")),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  PageUrl),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CDH_TYPE,  CrDrType == 0 ? (object) DBNull.Value :  CrDrType),             
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_STATUS_FILTER,  status),   
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_NO,  InvoiceNo == string.Empty ? (object) DBNull.Value : InvoiceNo),   
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CDH_CUSTOMER,  CusromerPk > 0 ? CusromerPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CDH_COMPANY,  cmpPk > 0 ? cmpPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CDH_INV_TYPE,  InvType > 0 ? InvType : (object)DBNull.Value)
            };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_CRDR_CUS_TRD_GET_LIST, colParameters);
            return dsList;
        }

        public static long? DeleteCreditDebitDetails(long CrDrPk, DateTime LastModifiedTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_CDH_PK, CrDrPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, LastModifiedTime.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),              
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.POInvoicing.Procedures.SPFIN_CRDR_NOTE_HDR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
