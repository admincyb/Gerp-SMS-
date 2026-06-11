using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.HRMS.Admin.Masters;
using System.Data;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class LeaveTypeMasterBL
    {
        public static int? SaveLeaveType(string strxml)
        {
            return LeaveTypeMasterDL.SaveLeaveType(strxml);
        }
        public static DataTable GetLeaveType(int? ltmPK, string code, string name, int active, int bizUnit, int pageNo, int pageSize, int accural,string sortOrder=null)
        {
            return LeaveTypeMasterDL.GetLeaveType(ltmPK, code, name, active, bizUnit, pageNo, pageSize, accural, sortOrder);
        }
        public static DataTable GetLeaveTypeDDL(int? ltmPK, string code, string name, int active, int bizUnit, int pageNo, int pageSize, int accural, string sortOrder = null, int? IsCredit = null, int? EmployeePk = null)
        {
            return LeaveTypeMasterDL.GetLeaveTypeDDL(ltmPK, code, name, active, bizUnit, pageNo, pageSize, accural, sortOrder, IsCredit , EmployeePk );
        }
        public static int DeleteLeaveType(int ltmPK, DateTime lastModDate)
        {
            return LeaveTypeMasterDL.DeleteLeaveType(ltmPK, lastModDate);
        }

        public static int UpdateLeaveTypeMasterStatus(int currPK, int status, int userPK, string lastModDate)
        {
            return LeaveTypeMasterDL.UpdateLeaveTypeMasterStatus(currPK, status, userPK, lastModDate);
        }
    }
}
