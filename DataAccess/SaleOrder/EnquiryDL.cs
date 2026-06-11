using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Common;

namespace DataAccess.SaleOrder
{
    public class EnquiryDL
    {
        public static int SaveEnquiryDetails(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SAVE_ENQUIRY, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        public static DataSet GetEnquiryList(GridPrams grid, User objUser, int procID, int userPK, short status, int enqPK, int cusPK, string pageURL)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                         
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID,  procID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  userPK),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.STATUS,  status),
              //new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_BIZUNIT,  bizunit)
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) )
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ENQ_NO  , enqPK > 0 ? enqPK : (Object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CEH_CUS  , cusPK > 0 ? cusPK : (Object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_PAGE_URL  , pageURL)
            };

            DataSet dsEnqList = new DataSet();
            dsEnqList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_ENQUIRY_LIST, colParameters);
            return dsEnqList;
        }

        public static DataSet GetEnquiryDetails(int enqPK, int custPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.CEH_PK, enqPK > 0 ? enqPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.CUST_PK, custPK > 0 ? custPK : (object)DBNull.Value)
            };
            DataSet dsEnq = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_ENQUIRY_DTL, colParameters);
            return dsEnq;
        }
        public static int DeleteEnquiryDetails(int enqPK, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.CEH_PK, enqPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.DELETEENQUIRY, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        public static DataTable GetBankDetails(int bankPK, int active, int bizunit, int bankType = 0,int fcHold=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               //new DBService.Parameters(GTIService.Constants.Sales.Parameters.CBM_PK, bankPK > 0 ? bankPK : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.CBM_PK, bankPK >= 0 ? bankPK : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.FC_HOLD, fcHold > 0 ? fcHold : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.BIZUNIT, bizunit),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.CBM_TYPE, bankType > 0 ? bankType : (object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETBANKDETAILS, colParameters).Tables[0];
        }
        public static DataSet GetEnquiryNoAuto(string field, string name, int bizUnit, int userPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, userPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.VALUE  , name == string.Empty ? "%" : name + "%"),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.FLD_NAME, field),
            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_ENQUIRYNO_AUTO, colParameters);
            return dsDesig;
        }
    }
}
