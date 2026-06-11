using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.PurchaseRequestManagement
{
    public class PurchaseRequestCreationDL
    {

        /// <summary>
        /// Function Used To get purchase request xml details based on the id
        /// </summary>
        /// <param name="purchaseRequestID"></param>
        /// <returns></returns>
        public static string GetPurchaseRequestDetails(int purchaseRequestID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.PurchaseRequest.Parameters.PURCHASEREQUESTPK ,  purchaseRequestID)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASERQSTDETAILS, colParameters).ToString();

            //DataSet dtRequisition = new DataSet();
            //dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASERQSTDETAILS, colParameters);
            //string strRetVal = "";
            //for (int i = 0; i < dtRequisition.Tables[0].Rows.Count; i++)
            //    strRetVal += dtRequisition.Tables[0].Rows[i][0].ToString();
            //return strRetVal;
        }

        public static string GetForPlanningRequestDetails(int RequestID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.PurchaseRequest.Parameters.PURCHASEREQUESTPK ,  RequestID)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASERQSTDETAILSPLAN, colParameters).ToString();

            //DataSet dtRequisition = new DataSet();
            //dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASERQSTDETAILS, colParameters);
            //string strRetVal = "";
            //for (int i = 0; i < dtRequisition.Tables[0].Rows.Count; i++)
            //    strRetVal += dtRequisition.Tables[0].Rows[i][0].ToString();
            //return strRetVal;
        }
        
        public static string GetMaterialRequestDetails(int materialRequestID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.PurchaseRequest.Parameters.PURCHASEREQUESTPK ,  materialRequestID)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETMATERIALRQSTDETAILS, colParameters).ToString();
        }
        public static string FillForMaterialPlanningPurchase(int materialRequestID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.PurchaseRequest.Parameters.MPURCHASEREQUESTPK ,  materialRequestID)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASERQSTDETAILSPLAN, colParameters).ToString();
        }
        

        /// <summary>
        /// Function Used To get puchase request no sequece
        /// </summary>
        /// <returns></returns>
        public static string GetPRNO()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PURCHASEREQUESTPK , 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPRNO, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PURCHASEREQUESTPK]).Value);
        }

        /// <summary>
        /// Function Used To get the list of the rol / msl material qty list
        /// </summary>
        /// <returns></returns>
        public static DataSet GetPurchaseRequest(int bizUnit, int dept, int type, int prPK, int rowCount, int itmCategory)
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
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASEPENDINGREQUEST, colParameters);
        }
        /// <summary>
        /// Get Packing Materials
        /// </summary>
        /// <param name="sohPK"></param>
        /// <param name="prhPK"></param>
        /// <returns></returns>
        /// 
        public static DataSet PackingMaterials(int sohPK, int prhPK, int dept, int type,int itmCatSC)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_SOH_PK,sohPK),              
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_PRH_PK,  prhPK == 0 ? (object)DBNull.Value : prhPK),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_IPD_TYPE,  type == 0 ? (object)DBNull.Value : type),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.DEPT, dept == 0 ? (object)DBNull.Value : dept ),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_IB_MC, itmCatSC == 0 ? 0 : itmCatSC )
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SPINV_PACK_MAT_REQ_GET, colParameters);
        }
        /// <summary>
        /// Customer Item Request Get
        /// </summary>
        /// <param name="sohPK"></param>
        /// <param name="prhPK"></param>
        /// <param name="dept"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataSet CustomerItemRequestGet(int sohPK, int prhPK, int dept, int type)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_SOH_PK,sohPK),              
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_PRH_PK,  prhPK == 0 ? (object)DBNull.Value : prhPK),
            //  new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_IPD_TYPE,  type == 0 ? (object)DBNull.Value : type),
               new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.DEPT, dept == 0 ? (object)DBNull.Value : dept )
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SPINV_ITEM_CUST_ITEM_REQ_GET, colParameters);
        }
        /// <summary>
        /// Get Io Numbers
        /// </summary>
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static DataTable GetIONumber(int sbuPk, int prhPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_PRH_PK,  prhPK == 0 ? (object)DBNull.Value : prhPK),
              new DBService.Parameters("P_ACTIVE",1),
              new DBService.Parameters("P_SOH_PK", 0),
              new DBService.Parameters("P_BIZUNIT", sbuPk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPSAL_ORDER_INT_PEND_GET_KV", colParameters).Tables[0];
        }

        /// <summary>
        /// Function Used To save purchase request
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static List<object> SavePurchaseRequestDetails(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.SPURCHASEREQUESTXML , xmlstr),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PRNO , string.Empty, 4000, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PRETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SAVEPURCHASEREQUEST, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PRETVAL]).Value.ToString();
            string prNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PRNO]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(prNo);
            return retvals;
        }

        public static List<object> SaveMaterialRequestDetails(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.SPURCHASEREQUESTXML , xmlstr),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PRNO , string.Empty, 4000, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PRETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SAVEMATERIALREQUEST, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PRETVAL]).Value.ToString();
            string prNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PRNO]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(prNo);
            return retvals;
        }

        public static List<object> SaveMaterialRequestPlantToPlant(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.SPURCHASEREQUESTXML , xmlstr),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PRNO , string.Empty, 4000, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PRETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SAVEMATERIALREQUESTP2P, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PRETVAL]).Value.ToString();
            string prNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PRNO]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(prNo);
            return retvals;
        }

        public static DataTable GetIONumberAuto(int bizUnit, int prhPK, int sohPK, string srchValue, int deptPk = 0, int isGlove = 0, int showAll = 0,int IsDispatched=1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_PRH_PK,  prhPK == 0 ? (object)DBNull.Value : prhPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,1),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_SOH_PK, sohPK > 0 ? sohPK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.BIZUNIT, bizUnit > 0 ? bizUnit : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_SOH_NO, srchValue),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_IS_DESP, IsDispatched > 0 ? IsDispatched : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_IS_FULL, showAll),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_IS_GLOVE, isGlove),
               new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_DEPT_PK, deptPk > 0 ? deptPk : (object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SPSAL_ORDER_INT_PEND_GET_KV, colParameters).Tables[0];
        }
        public static DataTable GetPRListItemDetails(int transTYPE, int transPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_POH_TRX_TYPE, transTYPE),
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_POH_TRX_PK, transPK)          
            };
            DataTable dtTable = new DataTable();
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SPPUR_REQUEST_LIST_DTL_GET, colParameters);
            if (dsResult != null && dsResult.Tables.Count != 0 && dsResult.Tables[0].Rows.Count != 0)
                dtTable = dsResult.Tables[0];
            return dtTable;
        }
        public static DataTable GetBudgetDetails(int Refid)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.P_TRAN_ID, Refid),
            };
            DataTable dtTable = new DataTable();
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SPPUR_BUDGET_BALANCE_GET, colParameters);
            if (dsResult != null && dsResult.Tables.Count != 0 && dsResult.Tables[0].Rows.Count != 0)
                dtTable = dsResult.Tables[0];
            return dtTable;
        }
    }
}
