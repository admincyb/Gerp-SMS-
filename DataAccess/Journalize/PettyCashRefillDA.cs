using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.Journalize
{
    public class PettyCashRefillDA
    {
        public static DataSet GetPettyCashRefillDate(int account)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_CVD_ACCOUNT,  account)
            };

            DataSet dsPettyCash = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SP_PETTYCASHREFILL_GET, colParameters);
            return dsPettyCash;

        }
        public static int? SavePettyCashRefill(int account, DateTime refillDate, User objUser, int? pk,DateTime? lastModDate=null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {          
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_CVD_PK,  pk.HasValue? pk.Value : (object)DBNull.Value),  
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_CVD_ACCOUNT,  account),  
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_REFILLDATE,  refillDate), 
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.USERPK,  objUser.PKUser ), 
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.BIZUNIT,  objUser.SBUID), 
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.DEPARTMENT,  objUser.CurrentDeptPK), //@P_LAST_MOD_DT
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.LASTMODDATE,  lastModDate.HasValue ? lastModDate.Value : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SP_PETTYCASHREFILL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Journalize.Parameter.P_RET_VAL]).Value);
            return result;

        }

        public static DataTable GetPettyCashList(DateTime? FromDate, DateTime? ToDate, int? CvdAccount, int? bizUnit, int PageNo = 0, int pageSize = 20)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.P_PAGE_NUM,  PageNo),
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.P_PAGESIZE,  pageSize),
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.BIZUNIT,  bizUnit.HasValue ? bizUnit.Value : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_CVD_ACCOUNT,  CvdAccount.HasValue ? CvdAccount.Value : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.P_FROM_DATE,  FromDate.HasValue ? FromDate.Value : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.P_TO_DATE,  ToDate.HasValue ? ToDate.Value : (object)DBNull.Value)
            };

            DataSet dsPettyCash = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_COA_VERIFICATION_DTL_GET_LIST, colParameters);
            return dsPettyCash.Tables[0];
        }
    }
}
