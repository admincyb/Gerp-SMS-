using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.HRMS.Admin.Masters;
using BusinessObject.HRMS.Admin.Masters;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class EmployeeTypeBL
    {
        public static DataTable GetEmployeeTypeList(int bizUnit, int pageNo, int pageSize, string typeName = null, string typeCode = null)
        {
            return EmployeeTypeDL.GetEmployeeTypeList(bizUnit, pageNo, pageSize, typeName, typeCode);
        }

        public static DataTable GetEmployeeTypeGetKV(int? empTypePk, int bizUnit, int active = 1)
        {
            return EmployeeTypeDL.GetEmployeeTypeGetKV(empTypePk, bizUnit, active);
        }

        public static EmployeeTypeBO GetEmployeeTypeByID(int id, int active)
        {
            return EmployeeTypeDL.GetEmployeeTypeByID(id, active);
        }

        public static int Save(EmployeeTypeBO empType)
        {
            return EmployeeTypeDL.Save(empType);
        }

        public static int DeleteEmployeeType(int pk, DateTime lastModifiedDate)
        {
            return EmployeeTypeDL.DeleteEmployeeType(pk, lastModifiedDate);
        }

        public static DataTable GetOTTemplateList(int bizUnit, int pk = 0, int active = 1)
        {
            return OTTemplateBL.GetOTTemplateList(bizUnit, pk, active);
        }

        public static DataTable GetLeaveTemplateList(int bizUnit, int pk = 0, int active = 1)
        {
            return LeaveTemplateBL.GetLeaveTemplateList(bizUnit, pk, active);
        }

        public static DataTable GetBranchOrLocation(int groupTypeValue, int groupValue, int bizUnit, int configPk = 0, int active = 1)
        {
            return Common.HRMSCommonBL.GetHrmsCommonConstMst(groupTypeValue, groupValue, bizUnit, configPk, active);
        }

        public static DataTable GetWorkingDays(int empTypePk)
        {
            return EmployeeTypeDL.GetWorkingDays(empTypePk);
        }


        public static int UpdateEmployeeTypeStatus(int currPK, int status, int userPK, string lastModDate)
        {
            return EmployeeTypeDL.UpdateEmployeeTypeStatus(currPK, status, userPK, lastModDate);
        }
    }
}
