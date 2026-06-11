using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.HRMS.Employee;
using DataAccess.HRMS.Employee;
using BusinessLogic.HRMS.Admin.Masters;
using DataAccess.HRMS.Admin.Masters;

namespace BusinessLogic.HRMS.Employee
{
    public class EmployeePayDetailsBL
    {
        public static DataTable GetEmployeeHeaderInfo(int employeePk, string sortBy = "")
        {
            return EmployeeBasicInfoBL.GetEmployeeDetailListHeader(employeePk, sortBy, (int)DbActiveStatus.HASPK);
        }

        public static EmployeePayDetailsBO GetEmployeePayDetailsByID(int payDetailsID, int active)
        {
            return EmployeePayDetailsDL.GetEmployeePayDetailsByID(payDetailsID, active);
        }

        public static int Save(EmployeePayDetailsBO payDetails,out DateTime EmpLastModDate)
        {
            return EmployeePayDetailsDL.Save(payDetails,out EmpLastModDate);
        }

        public static int DeletePayDetails(int payDetailsID, DateTime lastModifiedDate)
        {
            return EmployeePayDetailsDL.DeletePayDetails(payDetailsID, lastModifiedDate);
        }

        public static DataTable GetWorkingDays(int empTypePk)
        {
            return EmployeeTypeBL.GetWorkingDays(empTypePk);
        }

        //DropDowns
        public static DataTable GetPaymentMode(int? bizUnit)
        {
            //string configType = null, string configSplCond = null, int? bizUnit = null, int configPk = 0, int active = 1
            return HRMS.Common.HRMSCommonBL.GetHrmsCommonConfigMst("PAYMENT MODE", null, bizUnit, 0, 1);
        }

        public static DataTable GetPaymentModeSpl(int? bizUnit)
        {
            //string configType = null, string configSplCond = null, int? bizUnit = null, int configPk = 0, int active = 1
            //return HRMS.Common.HRMSCommonBL.GetHrmsCommonConfigMst("PAYMENT MODE", null, bizUnit, 0, 1);
            return HRMS.Common.HRMSCommonBL.GetHrmsCommonConfigMst("PAYMENT MODE", "SAL_PAY", bizUnit, 0, 1);
        }

        public static DataTable GetBankNames(int? bizUnit, int? type, int? accType, int pk = 0, int active = 1)
        {
            return HRMS.Common.HRMSCommonBL.GetCashOrBank(bizUnit, type, accType, pk, active);
        }

        public static DataTable GetSalaryTemplate(int bizUnit, int pk = 0, int active = 1, int payrolltype = 0)
        {
            return SalaryTemplateDL.GetSalaryTemplateKv(bizUnit, pk, active, payrolltype);
        }

        public static DataTable GetOTTemplateList(int bizUnit, int pk = 0, int active = 1)
        {
            return OTTemplateBL.GetOTTemplateList(bizUnit, pk, active);
        }

        public static DataTable GetLeaveTemplateList(int bizUnit, int pk = 0, int active = 1)
        {
            return LeaveTemplateBL.GetLeaveTemplateList(bizUnit, pk, active);
        }

        public static DataTable GetEmployeeTypeGetKV(int? empTypePk, int bizUnit, int active = 1)
        {
            return EmployeeTypeBL.GetEmployeeTypeGetKV(empTypePk, active, bizUnit);
        }



        public static DataTable GetPayrollTypeUserMapping(int? CurrUserPk, int EmployeePk, out int EmpPayrollType)
        {
            return EmployeePayDetailsDL.GetPayrollTypeUserMapping(CurrUserPk, EmployeePk, out EmpPayrollType);
        }
    }
}
