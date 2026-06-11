using System;
using System.Data;

namespace DataAccess.Administration.Configurations
{
    public class DepartmentSettingsDL
    {
        /// <summary>
        ///  Function Used To save Department config values
        /// </summary>
        /// <param name="configDetails"></param>
        /// <returns></returns>
        public static string SaveConfigValue(string configDetails)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.Config.Parameters.CONFIGTXML ,(object)configDetails ,DBService.ParameterType.XML),                 new DBService.Parameters( GTIService.Constants.Configurations.Config.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Config.Procedures.SPSAVECONFIG, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Config.Parameters.RETVAL]).Value);
        }

        /// <summary>
        ///  Function Used To get Department config values based on the dept and sbu
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="deptPK"></param>
        /// <returns></returns>
        public static string GetConfigValue(int sbuPK, int deptPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Configurations.Config.Parameters.SBUPK , sbuPK),
                new DBService.Parameters(GTIService.Constants.Configurations.Config.Parameters.DEPTPK  , deptPK)
            };
            return Convert.ToString(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Configurations.Config.Procedures.SPGETCONFIG, colParameters));
        }

        /// <summary>
        /// Function Used To delete Department config values based on the dept and sbu
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="deptPK"></param>
        /// <returns></returns>
        public static int DeleteConfigValue(int sbuPK, int deptPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Configurations.Config.Parameters.SBUPK , sbuPK),
                new DBService.Parameters(GTIService.Constants.Configurations.Config.Parameters.DEPTPK  , deptPK)
            };
            return dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Config.Procedures.SPDELETEONFIG, colParameters);
        }
    }
}
