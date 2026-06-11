using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Admin.Masters;

namespace DataAccess.HRMS.Admin.Masters
{
    public class BonusTypeMasterDL
    {

        /// <summary>
        /// Method to Save Bonus Type
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveBonusTypeMaster(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_XML , xmlstr),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_BONUS_TYPE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
            //return 0;
        }

        /// <summary>
        /// Method to Get Bonus Type List
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetBonusTypeList(int? currPK, int active, int pageNo, int pageSize, int bizUnit, string BonusTypeCode = null, string BonusTypeName = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_BON_PK, currPK.HasValue?currPK:(object)DBNull.Value),
              new DBService.Parameters(Parameters.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PAGE_NO , pageNo),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  pageSize),
              new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
              new DBService.Parameters(Parameters.P_SORT_BY, "BON_CODE"),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_BON_CODE, BonusTypeCode!=null ? BonusTypeCode!=string.Empty?BonusTypeCode:(object)DBNull.Value:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_BON_NAME, BonusTypeName!=null ? BonusTypeName!=string.Empty?BonusTypeName:(object)DBNull.Value:(object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_BONUS_TYPE_GET_KV, colParameters);
            //return null;
        }

        /// <summary>
        /// Method to Get Bonus Type
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetBonusType(int? ltmPK, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_BON_PK, ltmPK.HasValue?ltmPK:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_ACTIVE, active),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_BONUS_TYPE_GET_KV, colParameters).Tables[0];
            //return null;
        }

        /// <summary>
        /// Method to Delete Bonus Type Details
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteBonusTypeMaster(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_BON_PK , pk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_BONUS_TYPE_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
            //return 0;
        }

        /// <summary>
        /// Method to Change Bonus Type Status
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int UpdateBonusTypeMasterStatus(int currPK, int status, int userPK, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_BON_PK  , currPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE  , status),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK  , userPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT  , lastModDate == string.Empty  ? (object)DBNull.Value : lastModDate),  
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_BONUS_TYPE_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL]).Value);
            return result;
            //return 0;
        }
    }
}
