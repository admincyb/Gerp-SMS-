using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Finance;

namespace DataAccess.Finance
{
    public class AuditTrialsDL
    {
        //Get Report list for dropdown bind
        public static DataTable GetGSTReportTypeList(int CfgPk, byte Active, string CFG_Type, int bizUnit, string CfgSplCond)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                {
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.CFG_PK,CfgPk ),
                    new DBService.Parameters( GTIService.Constants.Common.Common.P_ACTIVE, Active),
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.CFG_TYPE, CFG_Type),
                    new DBService.Parameters( GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.CFG_SPL_COND, CfgSplCond == string.Empty ?(Object)DBNull.Value : CfgSplCond),
                };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETGSTREPORTLIST, colParameters).Tables[0];
            return dtProcess;
        }

        //Get Report for Report viewer
        public static DataSet GetReportData(int Active, string frmDate, string toDate, string reportType, int bizUnit, int userPK)
        {
            DataTable dtProcess = new DataTable();
            DataSet dsProccess = new DataSet();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.Active, Active),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FROM_DATE , frmDate== string.Empty ? (Object)DBNull.Value : Convert.ToDateTime(frmDate)),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_TO_DATE , toDate== string.Empty ? (Object)DBNull.Value : Convert.ToDateTime(toDate)),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_REPORT, reportType),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizUnit), 
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_USERPK, userPK == 0 ? (Object)DBNull.Value : userPK)
            };
            dsProccess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETREPORTDATA_AUDIT_TRIALS, colParameters);
            return dsProccess;
        }

        public static DataSet GetAuditTrailInvReport(string frmDate, string toDate, bool reportType, int bizUnit, int userPK)
        {
            DataSet dsAudit = new DataSet();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FROM_DATE , frmDate == string.Empty ? (Object)DBNull.Value : Convert.ToDateTime(frmDate)),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_TO_DATE , toDate == string.Empty ? (Object)DBNull.Value : Convert.ToDateTime(toDate)),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_REC_STATUS, reportType == true ? (Object)DBNull.Value : reportType),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizUnit), 
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_USERPK, userPK == 0 ? (Object)DBNull.Value : userPK)
            };
            dsAudit = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETAUDITTRIALINVOICERPT, colParameters);
            return dsAudit;
        }
    }
}
