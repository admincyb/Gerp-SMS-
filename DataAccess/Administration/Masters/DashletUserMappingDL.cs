using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Administration.Masters;
namespace DataAccess.Administration.Masters
{
   public class DashletUserMappingDL
    {
       public static DataTable GetDashletUserMappingList(int bizUnit, int pageNo, int pageSize, string name, int type, int module, int currPk = 0)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.BIZUNIT , bizUnit),                
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.DLC_PK , 0), //DBNull.Value
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.DLC_USER ,currPk<=0?(object)DBNull.Value:currPk ),
                new DBService.Parameters(Parameters.P_DLC_TYPE ,type<0?(object)DBNull.Value:type ),
                new DBService.Parameters(Parameters.P_MODULE ,module<0?(object)DBNull.Value:module ),
                new DBService.Parameters(Parameters.P_DLC_NAME ,name ==  string.Empty ? null : name), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, pageNo), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, pageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, 1),
            };
           return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_DASHLET_CFG_GET_KV, colParameters);
       }


       public static DataTable GetUserRoles(int deptPk, int userPk=0)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.DEPARTMENT , deptPk<=0?(object)DBNull.Value:deptPk),                
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.USER , userPk<=0?(object)DBNull.Value:userPk),
            };
           return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Configurations.Users.Procedure.SP_GET_USERDEPARTMENT, colParameters);
       }

       public static int? SaveDashletUserDetails(string strxml)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_DASHLET_USER_GRP_MAP_SAVE, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
           return result;
       }

       public static string DashletUserRolesByPK(int currPK)
       {
           string strRetVal = "";
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(Parameters.P_DLC_PK, currPK),
            };
           DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_DASHLET_USER_GRP_MAP_GET_XML, colParameters);
           foreach (DataRow dr in dtxml.Rows)
           {
               strRetVal += dr[0].ToString();
           }
           return strRetVal;
       }

    }
}
