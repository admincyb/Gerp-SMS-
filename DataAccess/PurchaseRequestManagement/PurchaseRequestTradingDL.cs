using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.PurchaseRequestManagement
{
    public class PurchaseRequestTradingDL
    {
        /// <summary>
        /// Function Used To get purchase request Trading xml details based on the id
        /// </summary>
        /// <param name="purchaseRequestID"></param>
        /// <returns></returns>
        public static string GetPurchaseRequestTradingDetails(int purchaseRequestID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.PurchaseRequest.Parameters.PURCHASEREQUESTPK ,  purchaseRequestID)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASERQSTTRDDETAILS, colParameters).ToString();

            //DataSet dtRequisition = new DataSet();
            //dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASERQSTDETAILS, colParameters);
            //string strRetVal = "";
            //for (int i = 0; i < dtRequisition.Tables[0].Rows.Count; i++)
            //    strRetVal += dtRequisition.Tables[0].Rows[i][0].ToString();
            //return strRetVal;
        }

        /// <summary>
        /// Function Used To get the list of the rol / msl material qty list
        /// </summary>
        /// <returns></returns>
        public static DataSet GetPurchaseRequestTrading(int bizUnit, int dept, int type, int prPK, int rowCount, int itmCategory)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.BIZUNIT,bizUnit),              
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.DEPT, dept),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.TYPE, type),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PURCHASEREQUESTPK, prPK == 0 ? (object)DBNull.Value : prPK),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_ROW_COUNT, rowCount == 0 ? (object)DBNull.Value : rowCount),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_ITM_CATEGORY, itmCategory == 0 ? (object)DBNull.Value : itmCategory)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASEPENDINGREQUESTTRD, colParameters);
        }

        /// <summary>
        /// Function Used To save purchase request trading
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static List<object> SavePurchaseRequestTradingList(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.SPURCHASEREQUESTXML , xmlstr),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PRNO , string.Empty, 4000, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PRETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SAVEPURCHASEREQUESTTRD, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PRETVAL]).Value.ToString();
            string prNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PRNO]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(prNo);
            return retvals;
        }
    }
}
