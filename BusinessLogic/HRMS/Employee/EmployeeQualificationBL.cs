using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.HRMS.Employee;
using GTIService;
using System.Data;

namespace BusinessLogic.HRMS.Employee
{
    public class EmployeeQualificationBL
    {
        //Save Employee Qualification
        public static int? SaveEmployeeQualification(string strxml)
        {
            return DataAccess.HRMS.Employee.EmployeeQualificationDL.SaveEmployeeQualifiaction(strxml);
        }


        //Get  Qualification Type
        public static DataTable GetQualificationType(int conPK)
        {
            return DataAccess.HRMS.Employee.EmployeeQualificationDL.GetQualificationType(conPK);
        }
        //Get  Qualification Status
        public static DataTable GetQualificationStatus(string configType, int configPK = 0, int active = 1, int bizUnit = 1)
        {
            return DataAccess.HRMS.Employee.EmployeeQualificationDL.GetQualificationStatus(configType, configPK, active, bizUnit);
        }
        //Get  Qualification List
        public static DataTable GetEmployeeQualificationList(int empPk)
        {
            return DataAccess.HRMS.Employee.EmployeeQualificationDL.GetEmployeeQualificationList(empPk);
        }

        //Fill employee qualification by Qual id in (XML)
        public static EmployeeQualifcn GetEmployeequalificationbyID(int qualfPk)
        {
            try
            {
                EmployeeQualifcn ObjEmployeeQualification = new EmployeeQualifcn();
                string employee = DataAccess.HRMS.Employee.EmployeeQualificationDL.GetEmployeeQualificationbyQualificationId(qualfPk);
                if (employee != string.Empty)
                {
                    ObjEmployeeQualification = (EmployeeQualifcn)CommonFunctions.DeserializeObject(employee, ObjEmployeeQualification);
                    return ObjEmployeeQualification;
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


        //Delete employee qualification by Qual id 
        public static int DeleteQualificationbyQualId(int qualfPk)
        {
            return DataAccess.HRMS.Employee.EmployeeQualificationDL.DeleteQualificationbyQualId(qualfPk);
        }
        
    }
}
