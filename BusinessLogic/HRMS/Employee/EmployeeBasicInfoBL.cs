using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.HRMS.Employee;
using GTIService;
using System.Data;
using ERP.Utilities.HRMS;

namespace BusinessLogic.HRMS.Employee
{
    public class EmployeeBasicInfoBL
    {

        //Save Employee Basic Info
        public static int SaveEmployeeBasicInfo(string strxml, out string EmpCode, out DataTable dtErrors)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.SaveEmployeeBasicInformation(strxml, out EmpCode, out dtErrors);
        }

        //Auto fill Nationality
        public static DataTable GetAutoNationality(string searchValue, string OpParam)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetAutoNationality(searchValue,OpParam);
        }
        //Auto fill Country
        public static DataTable GetAutoCountry(string searchValue)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetAutoCountry(searchValue);
        }

        //Auto fill Country
        public static DataTable GetTransactionNo(string searchKey)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetTransactionNo(searchKey);
        }

        //Auto fill State
        public static DataTable GetAutoState(string searchValue, int countryPk)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetAutoState(searchValue, countryPk);
        }

        //Auto fill Employee
        public static DataTable GetAutoFillEmployee(string searchValue)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetAutoEmployeeList(searchValue);
        }
        //Auto fill Employee
        public static DataTable GetEmployeeAuto(string searchValue, string empBranchLoc, string empType)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeAuto(searchValue, empBranchLoc, empType);
        }

        //Auto fill Profession
        public static DataTable GetAutoFillProfession(int conPK, string searchValue)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetAutoProfession(conPK, searchValue);
        }
        //Auto fill Religion
        public static DataTable GetAutoFillReligion(int conPK, string searchValue)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetAutoReligion(conPK, searchValue);
        }

        //Auto fill Team
        public static DataTable GetAutoFillTeam(string searchValue)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetAutoTeam(searchValue);
        }

        public static DataTable GetdropdownFillReligion(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetDropDownReligion(conPK);
        }

        //Auto fill SubReligion
        public static DataTable GetAutoSubReligion(int conPK, string searchValue)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetAutoSubReligion(conPK, searchValue);
        }
        public static DataTable GetdropDownSubReligion(int conPK, int conParentPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetDropDownSubReligion(conPK, conParentPK);
        }


        //Auto fill Department
        public static DataTable GetAutoFillDepartment(int conPK, string searchValue)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetAutoDepartment(conPK, searchValue);
        }

        //Auto fill Department
        public static DataTable GetFillDepartmentAutocomplete(int conPK, string searchValue)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetFillDepartmentAutocomplete(conPK, searchValue);
        }


        //Auto fill BranchLocation
        public static DataTable GetAutoFillBranchLocation(int conPK, string searchValue, int? UserPk = null)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetAutoFillBranchLocation(conPK, searchValue, UserPk);
        }


        //Fill marital Status
        public static DataTable GetMaritalStatus(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetMaritalStatus(conPK);
        }

        //Fill joblevel
        public static DataTable Getjoblevel(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetJobLevel(conPK);
        }
        //Fill job Category
        public static DataTable GetjobCategory(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetJobCategory(conPK);
        }
        //Fill job Stream
        public static DataTable Getjobstream(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetjobStream(conPK);
        }
        //Fill skill Level
        public static DataTable GetSkilLevel(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetSkillLevel(conPK);
        }

        //Fill Employee gender
        public static DataTable GetEmployeeGender(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeGender(conPK);
        }
        //Fill Salutation
        public static DataTable GetSalutation(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetSalutation(conPK);
        }


        //Fill BranchLocationType
        public static DataTable GetBranchLocationType(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetBranchLocationType(conPK);
        }
        //Fill Bloodgroup
        public static DataTable GetBloodgroup(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetBloodGroup(conPK);
        }

        //Fill EmploymentType
        public static DataTable GetEmploymentType(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmploymentType(conPK);
        }

        //Get  Employee Status CFG
        public static DataTable GetEmployeeStatus(string configType, int configPK = 0, int active = 1, int bizUnit = 1)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeStatus(configType, configPK, active, bizUnit);
        }



        //Fill Employee Designation
        public static DataTable GetEmployeeDesignation(int desPK, string searchKey = "")
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeDesignation(desPK, searchKey);
        }

        //Fill Employee Designation By Job Category and Job Level
        public static DataTable GetEmployeeDesignationByJob( string searchKey = "", int? JobCategory=null, int? JobLevel=null)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeDesignationByJob(searchKey, JobCategory, JobLevel);
        }

        //Fill Employee Detail list
        public static DataTable GetEmployeeDetailList()
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeDetailList();
        }

        //Fill Employee By Id
        //public static DataTable GetEmployeebyID(int empPk)
        //{
        //    return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeebyID(empPk);
        //}


        //Fill employee in (XML)
        public static EmployeeBasicInfomtn GetEmployeebyID(int empPk, int? UserPk = null)
        {
            try
            {
                EmployeeBasicInfomtn ObjEmployeeBasicInfomtn = new EmployeeBasicInfomtn();
                string employee = DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeebyID(empPk, UserPk);
                if (employee != string.Empty)
                {
                    ObjEmployeeBasicInfomtn = (EmployeeBasicInfomtn)CommonFunctions.DeserializeObject(employee, ObjEmployeeBasicInfomtn);
                    return ObjEmployeeBasicInfomtn;
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

        //Get Employee Training by ID
        public static DataTable GetEmployeeTrainingByID(int employeePK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeTrainingByID(employeePK);
        }

        //Delete Employee
        public static int DeleteEmployee(int employeePK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.DeleteEmployee(employeePK);
        }


        //Get Employee Details Header
        public static DataTable GetEmployeeDetailListHeader(int employeePK, string sortBy, int active = 1, int ispayroll = 0)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeDetailListHeader(employeePK, sortBy, active, ispayroll);
        }

        //Get Employee Filter List
        public static DataTable GetEmpListByFilterOptions(EmpDocumentFilterParameterBinder filterOptions)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmpListByFilterOptions(filterOptions);
        }

        //Save JobDetailsStatus
        public static int SaveJobDetailStatus(int pk, int employeeId, DateTime date, Int16 status, Int16 fromStatus, string reason, int userPk, int bizUnit,int cancelStatus)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.SaveJobDetailStatus(pk, employeeId, date, status, fromStatus, reason, userPk, bizUnit,cancelStatus);
        }

        //Save JobDetailsStatus
        public static int SaveJobDetailDepartment(int pk, int employeeId, DateTime date, Int16 fromDept, Int16 toDept, string reason, int userPk, int bizUnit)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.SaveJobDetailDepartment(pk, employeeId, date, fromDept, toDept, reason, userPk, bizUnit);
        }

        //Save JobDetailsStatus
        public static int SaveJobDetailDesignation(int pk, int employeeId, DateTime date, Int16 fromDesig, Int16 toDesig, string reason, int userPk, int bizUnit)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.SaveJobDetailDesignation(pk, employeeId, date, fromDesig, toDesig, reason, userPk, bizUnit);
        }

        //Get Employee Status Detail
        public static DataTable GetEmployeeStatusDetails(int pk, int bizUnit, int active = 1)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeStatusDetails(pk, bizUnit, active);
        }

        //Get Employee Status History
        public static DataTable GetEmployeeStatusHistory(int pk)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeStatusHistory(pk);
        }

        //Get Employee Department History
        public static DataTable GetEmployeeDepartmentHistory(int pk)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeDepartmentHistory(pk);
        }

        //Get Employee Designation History
        public static DataTable GetEmployeeDesignationHistory(int pk)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeDesignationHistory(pk);
        }

        //Save JobDetails - Branch
        public static int SaveJobDetailBranch(int pk, int employeeId, DateTime date, int branch, int FromBranch, string reason, int userPk, int bizUnit)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.SaveJobDetailBranch(pk, employeeId, date, branch,FromBranch, reason, userPk, bizUnit);
        }

        //Get Employee Branch History
        public static DataTable GetJobDetailsBranchHistory(int pk)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetJobDetailsBranchHistory(pk);
        }

        public static DataTable GetMaritalStatusConst(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetMaritalStatusConst(conPK);
        }


        public static DataTable GetEmploymentTypeHistory(int CurrPK)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmploymentTypeHistory(CurrPK);
        }

        public static int? SaveEmploymentTypeHistory(FilterParameters objParametrs, short FromEmploymentType, string Remarks, int IsContract, int MailBeforeDays)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.SaveEmploymentTypeHistory(objParametrs, FromEmploymentType, Remarks, IsContract, MailBeforeDays);
        }

        public static DataTable GetEmpDesignationDtl(int DesigPk, int Status)
        {
            return DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmpDesignationDtl(DesigPk, Status);
        }
    }
}
