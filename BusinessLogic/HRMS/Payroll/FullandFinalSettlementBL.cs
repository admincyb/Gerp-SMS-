using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.HRMS.Payroll;
using BusinessObject.HRMS.Payroll;
using GTIService;

namespace BusinessLogic.HRMS.Payroll
{
    public class FullandFinalSettlementBL
    {
        public static EmpPayrollHeader GetEmployeePayrollList(string strxml)
        {
            try
            {
                EmpPayrollHeader empPayrollObj = new EmpPayrollHeader();
                string payrolldtl = FullandFinalSettlementDL.GetEmployeePayrollList(strxml);
                if (payrolldtl != string.Empty)
                {
                    empPayrollObj = (EmpPayrollHeader)CommonFunctions.DeserializeObject(payrolldtl, empPayrollObj);
                    return empPayrollObj;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static int? ProcessPayrollDetails(string strxml, out string TrxNo, out DataTable dtErrorList)
        {
            return FullandFinalSettlementDL.ProcessPayrollDetails(strxml, out TrxNo, out dtErrorList);
        }

        public static DataTable GetPayrollProcessPeriodList(ERP.Utilities.HRMS.FilterParameters gridParam, int bizUnit, int processMode)
        {
            return FullandFinalSettlementDL.GetPayrollProcessPeriodList(gridParam, bizUnit, processMode);
        }

        public static EmpPayrollPayHeader GetEmployeePayrollDetailList(int epsPK)
        {
            try
            {
                EmpPayrollPayHeader empPayObj = new EmpPayrollPayHeader();
                string empPaydtl = FullandFinalSettlementDL.GetEmployeePayrollDetailList(epsPK);
                if (empPaydtl != string.Empty)
                {
                    empPayObj = (EmpPayrollPayHeader)CommonFunctions.DeserializeObject(empPaydtl, empPayObj);
                    return empPayObj;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static int? SaveEmployeePayroll(string strxml)
        {
            return FullandFinalSettlementDL.SaveEmployeePayroll(strxml);
        }

        public static DataTable GetPayrollType(int PK, int bizUnit, int status, int processMode = 0, int? UserPK = null)
        {
            return FullandFinalSettlementDL.GetPayrollType(PK, bizUnit, status, processMode, UserPK);
        }

        public static PeriodWorkingDayHdr GetWorkingDaysDetails(int PayrollPK, int bizUnit, int status, int itemPK)
        {
            try
            {
                PeriodWorkingDayHdr empWorkdayObj = new PeriodWorkingDayHdr();
                string wrkdayDtl = FullandFinalSettlementDL.GetWorkingDaysDetails(PayrollPK, bizUnit, status, itemPK);
                if (wrkdayDtl != string.Empty)
                {
                    empWorkdayObj = (PeriodWorkingDayHdr)CommonFunctions.DeserializeObject(wrkdayDtl, empWorkdayObj);
                    return empWorkdayObj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static int DeleteProcessPeriod(int pk, string lastModifiedDate)
        {
            return FullandFinalSettlementDL.DeleteProcessPeriod(pk, lastModifiedDate);
        }

        public static int DeleteEmployeePayroll(string xml)
        {
            return FullandFinalSettlementDL.DeleteEmployeePayroll(xml);
        }


        public static DataSet GetPayslipRPT(int currPK)
        {
            return FullandFinalSettlementDL.GetPayslipRPT(currPK);
        }

        public static DataSet GetSalaryStatementRPT(int currPK)
        {
            return FullandFinalSettlementDL.GetSalaryStatementRPT(currPK);
        }
        public static DataSet GetPaySlipMultipleReport(string xmlPaySlip, string payslipSp)
        {
            return FullandFinalSettlementDL.GetPaySlipMultipleReport(xmlPaySlip, payslipSp);
        }

        public static DataSet GetPayrollPreprocess(string xmlPreprocess)
        {
            return FullandFinalSettlementDL.GetPayrollPreprocess(xmlPreprocess);
        }

        public static DataSet GetPayrollSalaryOutRPT(int currPK)
        {
            return FullandFinalSettlementDL.GetPayrollSalaryOutRPT(currPK);
        }

        public static DataTable GetPayroll(int PayrollPk, int bizUnit)
        {
            return FullandFinalSettlementDL.GetPayroll(PayrollPk, bizUnit);
        }
        public static DataTable GetNonPayrollEmployees( int payrollType, string fromDate, string toDate)
        {
            return FullandFinalSettlementDL.GetNonPayrollEmployees(payrollType, fromDate, toDate);
        }
    }
}


