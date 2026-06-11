using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Finance;

namespace DataAccess.Finance
{
    public class MonthlyProductionDL
    {
        /// <summary>
        /// For Monthly Production List
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static DataTable GetMonthlyProductionList(string fromDate, string ToDate, int bizUnit, int pageNo, int pageSize)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(Parameters.P_PAGE_NUM, pageNo), 
                new DBService.Parameters(Parameters.P_PAGE_SIZE, pageSize),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_FPH_FROM, fromDate  ==  string.Empty ? null : fromDate),
                new DBService.Parameters(Parameters.P_FPH_TO, ToDate  ==  string.Empty  ? null : ToDate),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPFIN_MTHLY_PRODUCTION_GET_LIST, colParameters);

        }

        /// <summary>
        /// For Monthly Production Get
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static string GetMonthlyProductionItemsList(int FphPk)
        {
            string strRetVal = "";
            DataTable dtItemXml = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters( Parameters.P_FPH_PK,FphPk > 0 ? FphPk : (object)DBNull.Value) 
            };
            dtItemXml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPFIN_MTHLY_PRODUCTION_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtItemXml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// Save Monthly Production 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static int? SaveMonthlyProductionDetails(string xmlDoc, ref string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.P_XML, xmlDoc),  
                new DBService.Parameters(Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(Parameters.P_RET_NO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPFIN_MTHLY_PRODUCTION_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        /// <summary>
        /// Delete Monthly Production 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static int? DeleteMonthlyProduction(int CurrPK, DateTime LastModifiedTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.P_FPH_PK, CurrPK == 0 ? (object) DBNull.Value : CurrPK), 
                new DBService.Parameters(Parameters.LastModDate, LastModifiedTime),  
                new DBService.Parameters(Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPFIN_MTHLY_PRODUCTION_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
