using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.HRMS.Employee;
using GTIService;
using System.Data;

namespace BusinessLogic.HRMS.Employee
{
    public class EmployeeExperienceBL
    {
        //Save Employee Experience
        public static int? SaveEmployeeExperience(string strxml)
        {
            return DataAccess.HRMS.Employee.EmployeeExperienceDL.SaveEmployeeExperience(strxml);
        }

        //Fill ExitReason
        public static DataTable GetExitReason(int conPK,string SearchKey)
        {
            return DataAccess.HRMS.Employee.EmployeeExperienceDL.GetExitReason(conPK, SearchKey);
        }

        //Get  Qualification List
        public static DataTable GetEmployeeExperienceList(int empPk)
        {
            return DataAccess.HRMS.Employee.EmployeeExperienceDL.GetEmployeeExperienceList(empPk);
        }


        //Fill employee experience by exp id in (XML)
        public static EmployeeExprnc GetEmployeeexperiencebyID(int expPk)
        {
            try
            {
                EmployeeExprnc ObjEmployeeExprnc = new EmployeeExprnc();
                string employee = DataAccess.HRMS.Employee.EmployeeExperienceDL.GetEmployeeExperiencebyExperienceId(expPk);
                if (employee != string.Empty)
                {
                    ObjEmployeeExprnc = (EmployeeExprnc)CommonFunctions.DeserializeObject(employee, ObjEmployeeExprnc);
                    return ObjEmployeeExprnc;
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

        //Delete employee experience by experience id 
        public static int DeleteexperiencebyexpId(int expPk)
        {
            return DataAccess.HRMS.Employee.EmployeeExperienceDL.DeleteExperiencebyExpId(expPk);
        }

        //Fill Currency
        public static DataTable GetCurrency(int bizUnit)
        {
            return DataAccess.HRMS.Employee.EmployeeExperienceDL.GetCurrency(bizUnit);
        }

        //Fill BaseCurrency
        public static DataTable GetBaseCurrency()
        {
            return DataAccess.HRMS.Employee.EmployeeExperienceDL.GetBaseCurrency();
        }
    }
}
