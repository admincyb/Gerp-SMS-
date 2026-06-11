using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.Administration.Configurations
{
    public class SBUConfiguartionDL
    {
        /// <summary>
        /// Function Used To save sbu details
        /// </summary>
        /// <param name="sbuConfiguartion"></param>
        /// <returns></returns>
        public static string SaveSBUConfig(BusinessObject.Administration.Configurations.SBUConfiguartion sbuConfiguartion)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUPK  , sbuConfiguartion.SBUPk == 0 ? (object)DBNull.Value: sbuConfiguartion.SBUPk),  
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUNAME , sbuConfiguartion.SBUName),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUCODE , sbuConfiguartion.SBUCode),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUADDR1 , sbuConfiguartion.SBUAddr1),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUAddr2 , sbuConfiguartion.SBUAddr2),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUCity , sbuConfiguartion.SBUCity),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUPhone , sbuConfiguartion.SBUPhone),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUMobile , sbuConfiguartion.SBUMobile),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUFax , sbuConfiguartion.SBUFax),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUEmail , sbuConfiguartion.SBUEmail),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUState , sbuConfiguartion.SBUState > 0 ? sbuConfiguartion.SBUState : (object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUCountry , sbuConfiguartion.SBUCountry > 0 ? sbuConfiguartion.SBUCountry : (object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUTaxNo , sbuConfiguartion.SBUTaxNo),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUCREATEDBY , sbuConfiguartion.UserPk),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUMODBY , sbuConfiguartion.UserPk),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.BZUCURRENCY , sbuConfiguartion.BZU_CURRENCY),

                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.BZU_REG_NO , sbuConfiguartion.BZU_REG_NO),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.BZU_GST_NO , sbuConfiguartion.BZU_GST_NO),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.BZU_FIN_START_DT , sbuConfiguartion.BZU_FIN_START_DT== string.Empty? (object)DBNull.Value :sbuConfiguartion.BZU_FIN_START_DT),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.BZU_FIN_END_DT , sbuConfiguartion.BZU_FIN_END_DT== string.Empty? (object)DBNull.Value :sbuConfiguartion.BZU_FIN_END_DT),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.BZU_THEME , sbuConfiguartion.BZU_THEME== string.Empty? (object)DBNull.Value :sbuConfiguartion.BZU_THEME), 
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.BZU_YEAR_CTRL , sbuConfiguartion.BZU_YEAR_CTRL),

                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.SBUConfig.Procedures.SPSAVESBUCONFIG, colParameters);
            return ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.SBUConfig.Parameters.RETVAL]).Value.ToString();
        }
        /// <summary>
        /// Function Used To get sbu details
        /// </summary>
        /// <param name="gridPrams"></param>
        /// <returns></returns>
        public static DataSet GetSBUConfig(BusinessObject.GridPrams gridPrams)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.PAGENO  , gridPrams.PageNumber),  
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.PAGESIZE , gridPrams.PageSize),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.FIELDS , gridPrams.Fields),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SORTBY , gridPrams.SortBy),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SORTDIR , gridPrams.SortDirection),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.ISACTIVE , 1)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.SBUConfig.Procedures.SPGETSBUCONFIG, colParameters);
        }
        /// <summary>
        /// Function Used To active / inactive sbu 
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <returns></returns>
        public static string ActiveSBUConfig(int sbuPK, int userPK, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUPK , sbuPK),  
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.ISACTIVE , active),  
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.SBUMODBY , userPK),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.SBUConfig.Procedures.SPINACTIVESBUCONFIG, colParameters);
            return ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.SBUConfig.Parameters.RETVAL]).Value.ToString();
        }

        /// <summary>
        /// Function Used To get all active sbu details for fill combo based on the user 
        /// if userpk =0 then get all sbu other his sbu
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllSBU(int userPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.P_USER_PK , userPK == 0 ? (object)DBNull.Value : userPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.SBUConfig.Procedures.SPGETALLSBU, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetBizUnit(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Configurations.SBUConfig.Parameters.P_BIZ_UNIT, bizUnit),   
                 new DBService.Parameters(GTIService.Constants.Configurations.SBUConfig.Parameters.P_ACTIVE, 2)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.SBUConfig.Procedures.SP_GET_BIZUNIT, colParameters).Tables[0];
        }


        /// <summary>
        /// Function Used To get sbu details
        /// </summary>
        /// <param name="currPk"></param>
        public static DataSet GetSBUConfigDetails(int currPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Configurations.SBUConfig.Parameters.P_BIZ_UNIT ,  currPk),  
                new DBService.Parameters(GTIService.Constants.Configurations.SBUConfig.Parameters.P_ACTIVE, 2)
                // new DBService.Parameters(GTIService.Constants.Dispersion.Parameters.Retval, string.Empty,4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };

            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.SBUConfig.Procedures.SP_GET_BIZUNIT, colParameters);
            // return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Dispersion.Parameters.Retval]).Value);
        }


        /// <summary>
        /// Function Used To get  footer details 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSBUFooterDetails()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Configurations.SBUConfig.Parameters.P_SYS_PK ,  DBNull.Value)  
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.SBUConfig.Procedures.SP_SYSTEM_CFG_GET, colParameters).Tables[0];
        }
    }
}
