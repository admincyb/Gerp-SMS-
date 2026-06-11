using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Admin.Masters;
namespace DataAccess.HRMS.Admin.Masters
{
   public class HolidayMasterDL
    {
        /// <summary>
        /// For get   lsit
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
       public static DataTable GetHolidayMasterList(int bizUnit, string caption, int status, int pageNo, int pageSize)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, pageNo), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, pageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_HDR_CAPTION, caption ==  string.Empty ? null : caption),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_HOLIDAY_GET_LIST, colParameters);
        }

       public static int? SaveHolidayMasterDetails(string strxml)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_HOLIDAY_HDR_SAVE, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
           return result;
       }

       public static string GetHolidayMasterByPK(int bizUnit, int status, int itemPK)
       {
           string strRetVal = "";
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) , 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_HDR_PK, itemPK),
            };
           DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_HOLIDAY_GET_KV, colParameters);
           foreach (DataRow dr in dtxml.Rows)
           {
               strRetVal += dr[0].ToString();
           }
           return strRetVal;
       }

       public static DataTable GetHolidayType(int CurrPK, int status, int bizUnit)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) , 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_CFG_PK, CurrPK == 0 ? (object)DBNull.Value : CurrPK) ,
            };
           return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_HOLIDAY_TYPE_GET, colParameters);
       }

       public static int DeleteHolidayMaster(int pk, string lastModifiedDate)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_HDR_PK , pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
           dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_HOLIDAY_DELETE, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
           return result;
       }

       public static DataSet GetHolidayListRPT(int currPk)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.HDR_PK, currPk) , 
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_HOLIDAY_OUTPUT_RPT, colParameters);
       }
    }
}
