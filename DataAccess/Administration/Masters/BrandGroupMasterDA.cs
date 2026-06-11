using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.Administration.Masters;
using BusinessObject.CommonManagement;
using GTIService.Constants.Common;
using GTIService.Constants.Administration.Masters;

namespace DataAccess.Administration.Masters
{
    public class BrandGroupMasterDA
    {
        
        public static int? SaveBrandGroupMaster(BrandGroupMasterBO objBrandGroupMasterBO)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CIG_PK, objBrandGroupMasterBO.CIG_PK),  
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CIG_CODE,objBrandGroupMasterBO.CIG_CODE),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CIG_NAME,objBrandGroupMasterBO.CIG_NAME),              
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_ACTIVE,objBrandGroupMasterBO.CIG_ACTIVE),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_USER_PK,objBrandGroupMasterBO.USER_PK),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_BIZUNIT,objBrandGroupMasterBO.CIG_BIZUNIT),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_LAST_MOD_DT,objBrandGroupMasterBO.LAST_MOD_DT),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPCRM_CUST_ITEM_GROUP_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Administration.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static int? DeleteBrandGroupMaster(int CurrPK, DateTime LastModifiedTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CIG_PK, CurrPK),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_LAST_MOD_DT, LastModifiedTime), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPCRM_CUST_ITEM_GROUP_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetBrandGroupMaster(int CurrPK, short Status, int bizUnit, string BrandGrpCode, string BrandGrpName, int PageNo, int PageSize)
        {
            DBService dbService = new DBService();
            DataTable dtBrandGroupList = new DataTable();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CIG_PK, CurrPK > 0 ? CurrPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_ACTIVE,  Status),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CIG_CODE, string.IsNullOrEmpty(BrandGrpCode)?(object)DBNull.Value:BrandGrpCode),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CIG_NAME,  string.IsNullOrEmpty(BrandGrpName)?(object)DBNull.Value:BrandGrpName),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PAGE_NO,  PageNo),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PAGE_SIZE,  PageSize)
            };
            dtBrandGroupList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPCRM_CUST_ITEM_GROUP_GET_KV, colParameters).Tables[0];
            return dtBrandGroupList;
        }
    }
}
