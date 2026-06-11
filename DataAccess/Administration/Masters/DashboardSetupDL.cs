using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Administration.Masters;
using BusinessObject.CommonManagement;
namespace DataAccess.Administration.Masters
{
   public class DashboardSetupDL
    {
        /// <summary>
        /// For get   lsit
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
       public static DataTable GetDashboardSetupList(int user, string name,string code,  int pageNo, int pageSize, int Status = -1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, pageNo), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, pageSize),
                new DBService.Parameters(Parameters.P_IND_NAME, name ==  string.Empty ? (object)DBNull.Value : name),
                new DBService.Parameters(Parameters.P_IND_CODE, code ==  string.Empty ? (object)DBNull.Value : code),
                new DBService.Parameters(Parameters.P_IND_USER, user ),
                new DBService.Parameters(Parameters.P_IND_ACTIVE,Status == -1 ? (object)DBNull.Value: Status)

            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_DASHBOARD_CFG_GET_LIST, colParameters);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="sbu"></param>
       /// <param name="department"></param>
       /// <param name="userPk"></param>
       /// <returns></returns>
       public static DataTable GetMenuDetails(int sbu, int department, int userPk, Int16 module)
       {
            DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.BIZUNIT , sbu),                
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.DEPARTMENT , department),
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.USERPK, userPk),
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.MODULE, module)
            };
           return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_DASHBOARD_ITEM_DTL_GET, colParameters);
       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="sbu"></param>
       /// <param name="department"></param>
       /// <param name="userPk"></param>
       /// <returns></returns>
       public static DataTable GetReportListDetails(int grpPk)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.P_REPORT_GRP , grpPk),                
            };
           return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_REPORT_CFG_GET_KV, colParameters);
       }
       /// <summary>
       /// method for Get BizUnit and MIS Reports
       /// </summary>
       /// <param name="userID" Type=int></param>
       /// <returns>DataTable</returns>
       public static DataTable GetMISReportDtls(int userID, int sbuID, int mngPK,int deptPk)
       {
           DataTable dtReport;
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters( GTIService.Constants.Common.Common.P_BIZUNIT, sbuID==0?(object)DBNull.Value:sbuID),                
                new DBService.Parameters( GTIService.Constants.Common.Common.P_USERPK, userID),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_MNG_PK, mngPK),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_DEPT, deptPk)
            };
           dtReport = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_DASHBOARD_ITEM_DTL_GET, colParameters);
           return dtReport;
       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="sbu"></param>
       /// <param name="department"></param>
       /// <param name="userPk"></param>
       /// <returns></returns>
       public static DataTable GetDashletDetails(int userPK, int bizUnit,int currPk)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.BIZUNIT , bizUnit == 0 ? (object)DBNull.Value : bizUnit),                
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.DLC_PK , currPk),
                 new DBService.Parameters(GTIService.Constants.Common.CommonConstants.DLC_USER , userPK == 0 ? (object)DBNull.Value : userPK),
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.ACTIVESTATUS , Convert.ToInt32(DbActiveStatus.ACTIVE)),
            };
           return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_DASHLET_CFG_GET_KV, colParameters);
       }


       public static string DashboardSetupByPK(int currPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
              //  new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) , 
               // new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_IND_PK, currPK),
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_DASHBOARD_GET_XML, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
       public static int? SaveDashboardSetupDetails(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_DASHBOARD_CFG_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

       public static int? GetDashletDetails(int CurrPK,string ModDate)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
           {                
              new DBService.Parameters(Parameters.P_IND_PK, CurrPK),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, ModDate == string.Empty ? (object)DBNull.Value : ModDate),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
           };
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_DASHBOARD_CFG_DELETE, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
           return result;
       }
       public static DataTable GetDashBoardDefultValues(string Query)
       {
           DBService dbService = new DBService();  
           return dbService.DataAdapter(CommandType.Text, Query).Tables[0]; 
       }
    }
}
