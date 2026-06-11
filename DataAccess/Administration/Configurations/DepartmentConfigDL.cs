using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.Administration.Configurations
{
    public class DepartmentConfigDL
    {
        /// <summary>
        ///  Function Used To save Department agianst sbu
        /// </summary>
        /// <param name="sbuConfiguartion"></param>
        /// <returns></returns>
        public static string SaveSBUDepartmentConfig(BusinessObject.Administration.Configurations.DepartmentConfig departmentConfig)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            int result = 0;
            foreach (BusinessObject.Administration.Configurations.DepartmentActiveList departmentActiveList in departmentConfig.DepartmentActive)
            {
                colParameters = new DBService.Parameters[] 
                {                
                new DBService.Parameters( GTIService.Constants.Configurations.DeptConfig.Parameters.DEPTPK , departmentActiveList.Department),
                new DBService.Parameters( GTIService.Constants.Configurations.DeptConfig.Parameters.DEPTACTIVE , departmentActiveList.Active),
                new DBService.Parameters( GTIService.Constants.Configurations.DeptConfig.Parameters.MODBY , departmentConfig.UserPk),
                new DBService.Parameters(GTIService.Constants.Configurations.DeptConfig.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
                };
                dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.DeptConfig.Procedures.SPACTIVEDEPTCONFIG, colParameters);
                result = int.Parse(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.DeptConfig.Parameters.RETVAL]).Value.ToString());
            }
            return Convert.ToString(result);
        }

        /// <summary>
        /// Function Used To Get Department Details For Fill Combo by sbu
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <returns></returns>
        public static DataTable GetSBUDeptList(int sbuPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUPK , sbuPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.DeptConfig.Procedures.SPGETSBUDEPT, colParameters).Tables[0];
        }

        /// <summary>
        /// Function Used To Get Department Details by sbu
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <returns></returns>
        public static DataSet GetSBUDepartmentConfig(int sbuPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
               new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUPK , sbuPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.DeptConfig.Procedures.SPGETSBUDEPTCONFIG, colParameters);
        }
    }
}
