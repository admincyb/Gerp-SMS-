using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.HRMS.Payroll
{
    public class AttendanceInfoBL
    {
        public static DataTable GetBranchOrLocation(int groupTypeValue, int groupValue, int bizUnit, int configPk = 0, int active = 1)
        {
            return Common.HRMSCommonBL.GetHrmsCommonConstMst(groupTypeValue, groupValue, bizUnit, configPk, active);
        }
    }
}
