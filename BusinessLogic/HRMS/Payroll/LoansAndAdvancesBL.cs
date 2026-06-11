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
    public class LoansAndAdvancesBL
    {
        //To get Loans & Advance details for listing page
        public static DataSet GetLoansAndAdvancesList(BusinessObject.GridPrams gridParam, int branchPK, int emptypePK, int categoryPK = 0,int SearchEmp = 0)
        {
            return LoansAndAdvancesDL.GetLoansAndAdvancesList(gridParam, branchPK, emptypePK, categoryPK, SearchEmp);
        }

        public static DataTable GetLoanAdvanceItemType(int TypePK, int status, int bizUnit, int payelementPK)
        {
            return LoansAndAdvancesDL.GetLoanAdvanceItemType(TypePK, status, bizUnit, payelementPK);
        }

        public static int? SaveLoanAdvanceHeader(string strxml, out string TrxNo)
        {
            return LoansAndAdvancesDL.SaveLoanAdvanceHeader(strxml, out TrxNo);
        }

        public static EmpLoanHeader GetEmpLoanDetails(int elmPK)
        {
            try
            {
                EmpLoanHeader empLoanObj = new EmpLoanHeader();
                string loandtl = LoansAndAdvancesDL.GetEmpLoanDetails(elmPK);
                if (loandtl != string.Empty)
                {
                    empLoanObj = (EmpLoanHeader)CommonFunctions.DeserializeObject(loandtl, empLoanObj);
                    return empLoanObj;
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

        public static DataTable GetBranchLocation()
        {
            return LoansAndAdvancesDL.GetBranchLocation();
        }

        //Fill Employee Loan And Advance History
        public static DataTable GetEMPLoanHistory(int EmpPK, int bizUnit)
        {
            return LoansAndAdvancesDL.GetEMPLoanHistory(EmpPK,bizUnit);
        }

        public static int DeleteLoansAndAdvances(int pk, string lastModifiedDate)
        {
            return LoansAndAdvancesDL.DeleteLoansAndAdvances(pk, lastModifiedDate);
        }
    }
}
