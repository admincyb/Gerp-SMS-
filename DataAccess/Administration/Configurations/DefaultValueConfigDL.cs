using System;
using System.Data;

namespace DataAccess.Administration.Configurations
{
    public class DefaultValueConfigDL
    {
        /// <summary>
        /// Function Used To Save Default Values
        /// </summary>
        /// <param name="defaultDetails"></param>
        /// <returns></returns>
        public static string SaveDefaultValueConfig(string defaultDetails)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.DefaultConfig.Parameters.DEFAULTXML ,(object)defaultDetails ,DBService.ParameterType.XML),                 new DBService.Parameters( GTIService.Constants.Configurations.DefaultConfig.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.DefaultConfig.Procedures.SPSAVEDEFAULTCONFIG, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.DefaultConfig.Parameters.RETVAL]).Value);
        }
            
        /// <summary>
        /// Function Used To Get Default Values Based on the business unit,department and group
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="deptPK"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GetDefaultValueConfig(int sbuPK, int deptPK, string group)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Configurations.DefaultConfig.Parameters.SBUPK , sbuPK),
                new DBService.Parameters(GTIService.Constants.Configurations.DefaultConfig.Parameters.DEPTPK  , deptPK),
                new DBService.Parameters(GTIService.Constants.Configurations.DefaultConfig.Parameters.GROUP, group == string.Empty ? (object)DBNull.Value : group)
            };
            return Convert.ToString(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Configurations.DefaultConfig.Procedures.SPGETDEFAULTCONFIG, colParameters));
        }

        /// <summary>
        /// Function Used To Delete Default Values Based on the business unit,department and group
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="deptPK"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static int DeleteDefaultValueConfig(int sbuPK, int deptPK, string group)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Configurations.DefaultConfig.Parameters.SBUPK , sbuPK),
                new DBService.Parameters(GTIService.Constants.Configurations.DefaultConfig.Parameters.DEPTPK  , deptPK),
                new DBService.Parameters(GTIService.Constants.Configurations.DefaultConfig.Parameters.GROUP, group)
            };
            return dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.DefaultConfig.Procedures.SPDELETEDEFAULTCONFIG, colParameters);
        }

        /// <summary>
        /// Function Used To get the group in the dept. for fill auto complete
        /// </summary>
        /// <param name="group"></param>
        /// <param name="deptPK"></param>
        /// <returns></returns>
        public static DataTable GetAllGroupList(string group,int deptPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.DefaultConfig.Parameters.DEFAULTGROUP , group),
                new DBService.Parameters( GTIService.Constants.Configurations.DefaultConfig.Parameters.DEPTPK , deptPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.DefaultConfig.Procedures.SPGETGROUP, colParameters).Tables[0];
        }

        /// <summary>
        /// Function Used To get the Def Value by passing name sbu dep and ID
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="deptPK"></param>
        /// <param name="defName"></param>
        /// <param name="defID"></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>purchase order</for>
        /// Used in getting the default value of Po number format
        /// <returns></returns>
        public static DataTable GetDefaultValue(int sbuPK, int deptPK,string defName, int defID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, sbuPK),
               new DBService.Parameters(GTIService.Constants.Configurations.DefaultConfig.Parameters.DEPTPK  , deptPK),
               new DBService.Parameters(GTIService.Constants.Configurations.DefaultConfig.Parameters.DEFTNAME, defName),
               new DBService.Parameters(GTIService.Constants.Configurations.DefaultConfig.Parameters.DEFTID, defID==0?(object)DBNull.Value:defID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.DefaultConfig.Procedures.SPGETDEFAULTVALUE, colParameters).Tables[0];
            
        }

        
    }
}
