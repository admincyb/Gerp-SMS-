using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.HRMS.Employee;
using GTIService;
using DataAccess.HRMS.Employee;

namespace BusinessLogic.HRMS.Employee
{
    public class EmployeeSalaryBL
    {
        //Get Salary Details
        public static EmpTemplateHeader GetSalaryDetails(int pk,int empPk=0)
        {
            try
            {
                EmpTemplateHeader objTemplate = new EmpTemplateHeader();
                string xmlDtls = DataAccess.HRMS.Admin.Masters.SalaryTemplateDL.GetSalaryTemplate(pk, empPk);
                if (!string.IsNullOrEmpty(xmlDtls))
                {
                    objTemplate = (EmpTemplateHeader)CommonFunctions.DeserializeObject(xmlDtls, objTemplate);
                    return objTemplate;
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

        public static EmpTemplateHeader GetEmployeeSalaryDetails(int empPk, string toDate,string trxDate=null)
        {
            try
            {
                EmpTemplateHeader objTemplate = new EmpTemplateHeader();
                string xmlDtls = EmployeeSalaryDL.GetEmployeeSalaryDetails(empPk, toDate,trxDate);
                if (xmlDtls != null && xmlDtls != string.Empty)
                {
                    objTemplate = (EmpTemplateHeader)CommonFunctions.DeserializeObject(xmlDtls, objTemplate);
                    return objTemplate;
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

        public static int SaveEmployeeSalaryDetails(string xmlstr)
        {
            return EmployeeSalaryDL.SaveEmployeeSalaryDetails(xmlstr);
        }

        //Delete Employee  Salary
        public static int DeleteEmployeeSalary(int EmployeePK, DateTime? LastModDate)
        {
            return EmployeeSalaryDL.DeleteEmployeeSalary(EmployeePK, LastModDate);
        }
    }
}
