using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.HRMS.Employee;
using System.Data;
using BusinessObject.HRMS.Employee;
using ERP.Utilities.HRMS;

namespace BusinessLogic.HRMS.Employee
{
    public sealed class EmployeeDocBL
    {
        public static DataTable GetEmployeeDocs(int employeePK, int bizUnit, string sortBy = "empPK", string sortDirection = "ASC")
        {
            return EmployeeDocDL.GetEmployeeDocs(employeePK, bizUnit, sortBy, sortDirection);
        }

        public static EmployeeDoc GetEmployeeDocByDocID(int docID, int active)
        {
            return EmployeeDocDL.GetEmployeeDocByDocID(docID, active);
        }

        public static DataTable GetEmployeeDocTypeList(string configType, int configPK = 0, int active = 1, int bizUnit = 1)
        {
            return EmployeeDocDL.GetEmployeeDocTypeList(configType, configPK, active, bizUnit);
        }

        public static DataTable GetAutoCompleteEmployeeList(int? empPK, string searchKey = "", int active = 1, int? empCategory = null, int? empBranch = null, int? empType = null, int? employmentType = null, int? empCompany = null, int? EmpDept = null, int? empDesignation = null, int? PaymentMode = null, int? EmpCurrency = null, string toDate = null)
        {
            return EmployeeDocDL.GetAutoCompleteEmployeeList(empPK, searchKey, active, empCategory, empBranch, empType, employmentType, empCompany, EmpDept, empDesignation, PaymentMode, EmpCurrency,toDate);
        }
        public static DataTable GetAutoCompleteEmployeeListByFilter(int? empPK, string searchKey = "", string Filter = "", int active = 1, int? empCategory = null, int? EmpDept = null, int? EmpType = null, int? ProcessMode = null, string toDate = null)
        {
            return EmployeeDocDL.GetAutoCompleteEmployeeListByFilter(empPK, searchKey, active, empCategory, Filter, EmpDept, EmpType, ProcessMode, toDate);
        }

        public static DataTable GetAutoCompleteEmployeePayrollList(int? empPK,string FromDate,string ToDate, string searchKey = "", int active = 1, int? empCategory = null, int? empBranch = null, int? empType = null,
            int? employmentType = null, int? empCompany = null, int? EmpDept = null, int? empDesignation = null, int? PaymentMode = null, int? EmpCurrency = null, int? EmpPayrollType=null)
        {
            return EmployeeDocDL.GetAutoCompleteEmployeePayrollList(empPK,  FromDate, ToDate,searchKey, active, empCategory, empBranch, empType, employmentType, empCompany, EmpDept, empDesignation,
                PaymentMode, EmpCurrency, EmpPayrollType);
        }

        public static int Save(EmployeeDoc doc, out string empName)
        {
            return EmployeeDocDL.Save(doc, out  empName);
        }

        public static int DeleteEmployeeDoc(int docID, DateTime lastModDate)
        {
            return EmployeeDocDL.DeleteEmployeeDoc(docID, lastModDate);
        }

        public static DataTable GetEmployeeDocsLog(int docID)
        {
            return EmployeeDocDL.GetEmployeeDocsLog(docID);
        }

        public static DataTable GetEmpDocByFilterOptions(EmpDocumentFilterParameterBinder filterOptions)
        {
            return EmployeeDocDL.GetEmpDocByFilterOptions(filterOptions);
        }

        public static int CheckInOrCheckOut(EmployeeDocumentCheckInCheckOutParameterBinder parameter)
        {
            return EmployeeDocDL.CheckInOrCheckOut(parameter);
        }

        public static DataTable GetInternalOrExtenalType(ConfigurationParameterBinder parameter)
        {
            return EmployeeDocDL.GetInternalOrExtenalType(parameter);
        }

        public static DataTable GetIssuedFor(ConfigurationParameterBinder parameter)
        {
            return EmployeeDocDL.GetIssuedFor(parameter);
        }

        public static DataTable GetFilterForList(ConfigurationParameterBinder parameter)
        {
            return EmployeeDocDL.GetFilterForList(parameter);
        }

    }
}
