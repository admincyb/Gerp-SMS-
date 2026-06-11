using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.Finance;
using BusinessObject;
using DataAccess.Finance;

namespace BusinessLogic.Finance
{
    public class AuditTrialsBL
    {
        public static DataTable GetGTSReportTypeList(int CfgPk, byte Active, string CFG_Type, int bizUnit, string CfgSplCond)
        {
            return AuditTrialsDL.GetGSTReportTypeList(CfgPk, Active, CFG_Type, bizUnit, CfgSplCond);
        }

        public static DataSet GetReportData(int Active, string frmDate, string toDate, string reportType, int bizUnit, int userPK)
        {
            return AuditTrialsDL.GetReportData(Active, frmDate, toDate, reportType, bizUnit, userPK);
        }

        public static DataSet GetAuditTrailInvReport(string frmDate, string toDate, bool reportType, int bizUnit, int userPK)
        {
            return AuditTrialsDL.GetAuditTrailInvReport(frmDate, toDate, reportType, bizUnit, userPK);
        }
    }
}
