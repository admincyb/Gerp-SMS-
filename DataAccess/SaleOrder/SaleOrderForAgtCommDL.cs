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

namespace DataAccess.SaleOrder
{
    public class SaleOrderForAgtCommDL
    {
        /// <summary>
        /// Get Purchase Invoice List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetAgentCommInvoiceList(GridPrams grid, User objUser, string pageUrl, int? Status = null, string SP_NAME = null, int? AgentCMSelectType = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {   
                         
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              //new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_USER_PK,  objUser.PKUser),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  pageUrl),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_FIELDS,  "*"),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IVH_TYPE,  type == 0 ? (object) DBNull.Value :  type),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.STATUSFILTER,  Status == null ? (object) DBNull.Value :  Status), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LIST_TYPE,  AgentCMSelectType == null ? (object) DBNull.Value :  AgentCMSelectType) 
              //new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_CUSTOMER_TEXT,  Customer == string.Empty ? (object) DBNull.Value :  Customer), 
              //new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SOH_NO,  ScNo == string.Empty ? (object) DBNull.Value :  ScNo              )
            };

            DataSet dsRFQList = new DataSet();
            //dsRFQList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_CUS_AGENT_GET_LIST, colParameters);
            dsRFQList = dbService.DataAdapter(CommandType.StoredProcedure, SP_NAME, colParameters);
            return dsRFQList;
        }
        public static DataTable GetUsers(string value, int bizunit, int SEARCHBY)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, value) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY, SEARCHBY==1?"ICH_CUSTOMER_TEXT":SEARCHBY==2?"SOH_NO":"ICH_NO"),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,bizunit)
            };

            DataTable dtUsers = new DataTable();
            dtUsers = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_CUS_AGENT_AUTO, colParameters).Tables[0];
            return dtUsers;
        }

        public static DataTable GetAutoCusScInvForAgentComm(string value, int bizunit, int SEARCHBY)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, value) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY, SEARCHBY==1?"ICH_CUSTOMER_TEXT":SEARCHBY==2?"SOH_NO":"ICH_NO"),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,bizunit)
            };

            DataTable dtUsers = new DataTable();
            dtUsers = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPSAL_INVOICE_CUS_AGENT_AUTO, colParameters).Tables[0];
            return dtUsers;
        }

        public static DataTable GetAgtCommAuto(string value, int bizunit, int SEARCHBY)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, value) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY, SEARCHBY==1?"IVH_VENDOR_TEXT":"IVH_NO"),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,bizunit)
            };

            DataTable dtUsers = new DataTable();
            dtUsers = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_VND_AGENT_AUTO, colParameters).Tables[0];
            return dtUsers;
        }



        /// <summary>
        /// Get Sales Invoice Details
        /// </summary>
        /// <param name="soPK"></param>
        /// <param name="invPK"></param>
        /// <returns></returns>
        public static string GetAgentCommInvHeader(string XML, int venPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML,  XML==string.Empty? DBNull.Value.ToString():XML), 
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_IVH_PK,  venPK == 0 ? (object) DBNull.Value :  venPK)                
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_VND_AGENT_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// Save Sales Invoice agt Comm
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SaveSalesInvoiceAgtComm(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_VND_AGENT_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }



        /// <summary>
        /// Check Agent Comm Invoiced complete or not
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>

        public static int CheckInvoiceAgtComm(int ichpk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.V_ICH_PK, ichpk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_VND_AGENT_DTL_VAL, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
            /// <summary>
            /// Get Agt comm Invoice List 
            /// </summary>
            /// <param name="grid"></param>
            /// <param name="bizUnit"></param>
            /// <returns>DataSet</returns>
            /// 
            public static DataSet GetAGTList(GridPrams grid, User objUseruserPK, string Customer, int status = 0, byte? pending = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            if (grid != null)
            {

                colParameters = new DBService.Parameters[] 
                {            
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUseruserPK.SBUID),  
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, objUseruserPK.PKUser),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_STATUS_FILTER,  status == null ? (object) DBNull.Value : status ),
                    new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_FIELDS,  "*"),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
                    new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IS_PENDING,  pending == null ? (object) DBNull.Value : pending),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy == "IVH_PK"?grid.SearchValue:grid.SearchValue+"%")),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
                    new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.P_IVH_VENDOR_TEXT,  Customer == string.Empty ? (object) DBNull.Value : Customer),                    
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
                    
                };
            }

            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_VND_AGENT_GET_LIST, colParameters);
            return dsList;
        }
        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="hrhPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteAgtCommDetails(int ivhPK, DateTime lastModDate, string appType, int userPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_IVH_PK, ivhPK==0?(object) DBNull.Value:ivhPK), 
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.App_Type, appType), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER, userPK==0?(object) DBNull.Value:userPK), 
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_VND_AGENT_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataSet GetAgentCommInvoicePrint(int ivhPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                {            
                    new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_IVH_PK,  ivhPK == 0 ? (object) DBNull.Value :  ivhPK),
                };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_INVOICE_VND_AGENT_PRINT, colParameters);
            return dsList;
        }
    }
}
