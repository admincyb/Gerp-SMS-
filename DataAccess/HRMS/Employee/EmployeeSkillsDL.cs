using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities.HRMS;
using BusinessObject.HRMS.Employee;
using ERP.Utilities;
using GTIService.Constants.HRMS.Employee;

namespace DataAccess.HRMS.Employee
{
    public class EmployeeSkillsDL
    {
        //Get Skill Category List
        public static DataSet GetSkillCategoryList(int empPk, string showAllFlag, int bizUnit, byte Active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters( Parameters.EMPLOYEE, empPk),
                new DBService.Parameters( Parameters.BIZUNIT, bizUnit),
                new DBService.Parameters( Parameters.ACTIVE, Active),
                new DBService.Parameters( Parameters.ESD_FLAG, showAllFlag=="1" ? (Object)DBNull.Value : "1"),
            };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_SKILL_CATEGORY, colParameters);
            return dsList;
        }
        //Get Skill List By Category Id
        public static string GetSkillDetails(int empId, int skillCategoryId,string showAllFlag, int bizUnit)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            string strRetVal = "";
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                {
                    new DBService.Parameters(Parameters.EMPLOYEE, empId),
                    new DBService.Parameters(Parameters.ESD_SKILL_CATEGORY, skillCategoryId),
                    new DBService.Parameters( Parameters.ESD_FLAG, showAllFlag=="1" ? (Object)DBNull.Value : "1"),
                    //new DBService.Parameters( Parameters.ESD_FLAG, showAllFlag=="1" ? (Object)DBNull.Value : "1"),
                    new DBService.Parameters(Parameters.BIZUNIT, bizUnit),
                };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_SKILLS_BY_CATEGORYPK, colParameters).Tables[0];
            foreach (DataRow dr in dtProcess.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        //Get Skill Levels List
        public static DataTable GetSkillLevelsList(int cfgPk, byte Active, string cfgType, int bizUnit)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                {
                    new DBService.Parameters(Parameters.CFG_PK, cfgPk),
                    new DBService.Parameters(Parameters.ACTIVE, Active),
                    new DBService.Parameters(Parameters.CFG_TYPE, cfgType),
                };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GET_SKILL_LEVELS, colParameters).Tables[0];
            return dtProcess;
        }
        //Save Employee Skills
        public static long SaveEmployeeSkills(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SAVE_EMPLOYEE_SKILL, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
