using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.HRMS.Payroll;
using ERP.Utilities.HRMS;

namespace BusinessLogic.HRMS.Payroll
{
    public class HRYearCloseBL
    {
        public static DataSet GetLeaveList(FilterParameters objFilterParam)
        {
            return HRYearCloseDL.GetLeaveList(objFilterParam);
        }


        public static DataTable GetLeaveType(int? ltmPK ,int active, int bizUnit)
        {
            return HRYearCloseDL.GetLeaveType(ltmPK, active, bizUnit);
        }

        public static DataTable GetSalaryDetails(int active, int bizUnit, int? curPk = null, string curDate = null)
        {
            return HRYearCloseDL.GetSalaryDetails(active, bizUnit, curPk, curDate);
        }

        public static int SaveHRYearClause(string xmlDoc)
        {
            return HRYearCloseDL.SaveHRYearClause(xmlDoc);
        }
    }

}
