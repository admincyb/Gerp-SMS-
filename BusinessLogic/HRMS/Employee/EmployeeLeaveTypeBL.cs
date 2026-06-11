using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.HRMS.Employee;
using GTIService;
using System.Data;
using ERP.Utilities.HRMS;
using DataAccess.HRMS.Employee;

namespace BusinessLogic.HRMS.Employee
{
    public class EmployeeLeaveTypeBL
    {
        //Get Skill Category List
        public static DataSet GetSkillCategoryList(int empPk,string showAllFlag,int bizUnit, byte Active)
        {
            return EmployeeSkillsDL.GetSkillCategoryList(empPk, showAllFlag,bizUnit, Active);
        }
        //Get Skill List By Category Id
        public static EmployeeSkillsDetails GetSkillDetails(int empId, int skillCategoryId,string showAllFlag ,int bizUnit)
        {
            try
            {
                EmployeeSkillsDetails EmployeeSkillsObj = new EmployeeSkillsDetails();
                string EmployeeSkillsList = EmployeeSkillsDL.GetSkillDetails(empId, skillCategoryId, showAllFlag, bizUnit);
                if (EmployeeSkillsList != string.Empty)
                {
                    EmployeeSkillsObj = (EmployeeSkillsDetails)CommonFunctions.DeserializeObject(EmployeeSkillsList, EmployeeSkillsObj);
                    return EmployeeSkillsObj;
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
        //Get Skill Levels List
        public static DataTable GetSkillLevelsList(int cfgPk,byte Active,string cfgType,int bizUnit)
        {
            return EmployeeSkillsDL.GetSkillLevelsList(cfgPk,Active,cfgType,bizUnit);
        }
        //Save Employee Skills
        public static long SaveEmployeeSkills(string xmlDoc)
        {
            return EmployeeSkillsDL.SaveEmployeeSkills(xmlDoc);
        }


        public static DataTable GetEmpLeaveTypeList(int? EmpLeavePk, int EmpPk, int Active, int bizUnit)
        {
            return EmployeeLeaveTypeDL.GetEmpLeaveTypeList(EmpLeavePk, EmpPk, Active, bizUnit);
        }

        public static int SaveEmployeeLeaveTypes(string xmlDoc)
        {
            return EmployeeLeaveTypeDL.SaveEmployeeLeaveTypes(xmlDoc);
        }

        public static DataTable GetEmpLeaveCreditDetails(int empPk, int leaveType, int bizUnit)
        {
            return EmployeeLeaveTypeDL.GetEmpLeaveCreditDetails(empPk, leaveType, bizUnit);
        }
    }
}
