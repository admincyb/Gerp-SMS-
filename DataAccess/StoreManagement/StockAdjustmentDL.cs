using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.StoreManagement
{
    public class StockAdjustmentDL
    {
        /// <summary>
        /// Get Store Audit Details By 
        /// </summary>
        /// <param name="storeAuditID"></param>
        /// <returns></returns>
        public static string GetStoreAuditDetails(int storeAuditID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtxml = new DataTable();
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreAudit.SAHPK ,  storeAuditID)
            };
            dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_StockAdjustment.GETSTOCKADJUSTMENT, colParameters).Tables[0];
            return dtxml.Rows[0][0].ToString();
        }

        /// <summary>
        /// Save Stock Adjustment Details
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static string SaveStockAdjustmentDetails(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StockAdjustment.SAXML , xmlstr),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PRETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_StockAdjustment.SAVESTOCKADJUSTMENT, colParameters);
            return ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PRETVAL]).Value.ToString();
        }


        /// <summary>
        /// Get Stock Adjustment List Details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetStockAdjustmentList(GridPrams grid, User objUser, int procID,string PageUrl)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                         
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "SAH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ||grid.SortBy == "SAH_DATE"|| grid.SortBy == "SAH_NO" ? "SAH_PK": grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID ,procID ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
          
            };

            DataSet dsPurchaseRqstList = new DataSet();
            dsPurchaseRqstList = dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_STK_ADJ_GET_LIST_WRKF", colParameters);
            return dsPurchaseRqstList;


        }
        //====================== 30-09-2011  For Process to Process Switching  ===========================================
        /// <summary>
        /// Get Application ID against Refid
        /// </summary>
        /// <param name="evaluationID"></param>
        /// <returns>ApplicationID</returns>
        public static int GetEvalAppIDForRefID(int refID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            int Appid = 0;
            colParameters = new DBService.Parameters[] 
           {   
                new DBService.Parameters("REF_ID",  refID),
           };
            Appid = Convert.ToInt32(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_StockAdjustment.GETEVALAPPIDFORREFID, colParameters));
            return Appid;
        }
        /// <summary>
        /// Delete Evaluation Details By evaluationID
        /// </summary>
        /// <param name="evaluationID"></param>
        /// <returns>int- 1(Success)</returns>
        public static int UpdateEvaluationDtls(WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
           {   
                new DBService.Parameters("SDH_PK",  wrkfReq.ApplicationID ),
                new DBService.Parameters("SDH_REF_ID",  wrkfReq.ReferenceID ),
                 new DBService.Parameters("P_USER_PK",  objUser.PKUser ),
                new DBService.Parameters(GTIService.Constants.Store.Parameters.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
           };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_StockAdjustment.UPDATEEVALREFID, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters.PRETVAL]).Value);

        }
    }
}
