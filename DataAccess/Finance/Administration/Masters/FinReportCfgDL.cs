using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.Finance.Administration.Masters
{
    public class FinReportCfgDL
    {
        public static int? SaveFinReportCfg(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_REPORT_TEMPLATE_CFG_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        public static DataTable GetKVFinReportCfg(int pk, byte active, byte template, byte? group, int bizUnit, int? type, byte? level, string name, int? parent, string dispName)
        {
            DataTable dtData;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RTC_PK,pk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,active == 3 ? (object)DBNull.Value : active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RTC_TEMPLATE,template),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RTC_IS_GROUP,group),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RTC_TYPE,type),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RTC_LEVEL,level),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RTC_NAME,name),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RTC_PARENT,parent == 0?(object)DBNull.Value:parent),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RTC_DISPLAY_NAME,dispName==string.Empty ? (object)DBNull.Value : dispName),
            };
            dtData = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_REPORT_TEMPLATE_CFG_GET_KV, colParameters).Tables[0];
            return dtData;
        }
    }
}
