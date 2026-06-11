using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTIService.Constants.Stock;
using System.Data;
using BusinessObject;
using ERP.Utilities.Dashboard;

namespace DataAccess.Stock
{
    public class StockDL
    {
        public static int? StockTakingSave(string Xml, ref string RefNO)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.XML_VALUES, Xml),   
                new DBService.Parameters(Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
                //new DBService.Parameters(CommonConstants.RETURNNO, string.Empty, 50,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_BIN_CARD_STK_TAKE_INIT_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            //RefNO = ((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNNO]).Value.ToString();
            return result;
        }

        public static DataSet GetStockTakeList(FilterParameters objFilterParam, string FromDate,string ToDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {           
                new DBService.Parameters(Parameters.P_PAGE_NO ,  objFilterParam.PageNumber),
                new DBService.Parameters(Parameters.P_PAGE_SIZE,  objFilterParam.PageSize),
                new DBService.Parameters(Parameters.P_BIZUNIT, objFilterParam.BizUnit),
                new DBService.Parameters(Parameters.P_FROM_DT, FromDate == string.Empty ? (object)DBNull.Value: FromDate),
                new DBService.Parameters(Parameters.P_TO_DT, ToDate == string.Empty ? (object)DBNull.Value: ToDate)         
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_BIN_CARD_STK_TAKE_INIT_GET_LIST, colParameters);
        }

        public static string GetStockTakeDetails(int currPK, int? bizUnit)
        {
            DataTable dtData;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(Parameters.P_BIT_PK, currPK)
            };

            dtData = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_BIN_CARD_STK_TAKE_INIT_GET, colParameters).Tables[0];

            string xml = String.Empty;
            foreach (DataRow drBinInspection in dtData.Rows)
            {
                xml += Convert.ToString(drBinInspection[0]);
            }
            return xml;
        }

        public static int? DeleteStockTaking(int currPK, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {           
                new DBService.Parameters(Parameters.P_BIT_PK ,  currPK),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT, lastModDate),
                new DBService.Parameters(Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_BIN_CARD_STK_TAKE_INIT_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static DataTable GetActiveRecord(int? BizUnit)
        {
            DataTable dtResult;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                 new DBService.Parameters(Parameters.P_BIT_IS_OPEN , 1), 
                 new DBService.Parameters(Parameters.P_BIZUNIT, BizUnit.HasValue?BizUnit : (object)DBNull.Value)
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_BIN_CARD_STK_TAKE_INIT_GET_KV, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetAllLots(int deptPK, string LocationText, int StockInitPk)
        {
            DataTable dtResult;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                 new DBService.Parameters(Parameters.DEPARTMENT , deptPK), 
                 new DBService.Parameters(Parameters.P_LOCATION_TEXT, LocationText.Trim()),
                 new DBService.Parameters(Parameters.P_BIT_PK, StockInitPk)
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_BIN_CARD_STK_TAKE_LOT_GET, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetBincardList(int deptPK, int LocationPk, string BinNo, string StockTakePk)
        {
            DataTable dtResult;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            int BhtPk = 0;
            int.TryParse(StockTakePk, out BhtPk);
            colParameters = new DBService.Parameters[] 
            {  
                 new DBService.Parameters(Parameters.DEPARTMENT , deptPK), 
                 new DBService.Parameters(Parameters.P_LOCATION, LocationPk),
                 new DBService.Parameters(Parameters.P_BCH_NO, string.IsNullOrEmpty(BinNo.Trim())? (object)DBNull.Value : BinNo.Trim()),
                 new DBService.Parameters(Parameters.P_BHT_PK, BhtPk > 0? BhtPk : (object)DBNull.Value)
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_BIN_CARD_STK_TAKE_GET, colParameters).Tables[0];
            return dtResult;
        }

        public static List<string> SaveBincardList(string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.XML_VALUES, xml),  
                new DBService.Parameters(Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_BIN_CARD_STK_TAKE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            //string TrxNo = (((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNNO]).Value).ToString();
            List<string> retvals = new List<string>();
            retvals.Add(result.ToString());
            //retvals.Add(TrxNo);
            return retvals;
        }

        public static DataTable ScanAndSaveBincard(string xml)
        {
            DataTable dtResult;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.XML_VALUES, xml),  
                new DBService.Parameters(Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_BIN_CARD_STK_TAKE_GET, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetStockReconciliation(int StockInitPk, int? RecnType)
        {
            DataTable dtResult;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.P_BIT_PK, StockInitPk),  
                new DBService.Parameters(Parameters.P_BDT_RECON_TYPE, RecnType.HasValue? RecnType : (object)DBNull.Value)  
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_BIN_CARD_STK_TAKE_RECON_GET, colParameters).Tables[0];
            return dtResult;
        }
    }
}
