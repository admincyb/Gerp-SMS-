using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.Administration.Configurations
{
    public class MarginSetupDA
    {
        /// <summary>
        /// Save Commision Details
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveMarginSetup(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Configurations.MarginSetup.Procedures.SPINV_MARGIN_SETUP_MST_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get Margin Setup
        /// </summary>
        /// <param name="ratePk"></param>
        /// <param name="active"></param>
        /// <returns>DataTable</returns>     
        public static DataSet GetMarginSetup(GridPrams pageParam)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_SER_NAME, pageParam.SearchBy == string.Empty ? (Object)DBNull.Value : pageParam.SearchBy),
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_SER_VAL, pageParam.SearchValue == string.Empty ? (Object)DBNull.Value : pageParam.SearchValue),
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_PAGE_NO, pageParam.PageNumber),
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_PAGE_SIZE, pageParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_SORT_BY, pageParam.SortBy == string.Empty ? (Object)DBNull.Value : pageParam.SortBy),
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_MAS_FROM_DT, pageParam.FromDate == string.Empty ? (Object)DBNull.Value : pageParam.FromDate),
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_MAS_TO_DT, pageParam.ToDate == string.Empty ? (Object)DBNull.Value : pageParam.ToDate),
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_MAS_BIZUNIT, pageParam.BizUnit),
            };

            DataSet dsMarginSetup = new DataSet();
            dsMarginSetup = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Configurations.MarginSetup.Procedures.SPINV_MARGIN_SETUP_MST_GET_LIST, colParameters);//.Tables[1];
            return dsMarginSetup;
        }

        /// Get Margin Setup By PK
        /// </summary>
        /// <param name="locationPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetMarginSetupByPK(int marginSetupPK)
        {
            DataTable dtMarginSetup;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_MAS_PK ,marginSetupPK),
            };
            dtMarginSetup = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Configurations.MarginSetup.Procedures.SPINV_MARGIN_SETUP_MST_GET_KV, colParameters).Tables[0];
            return dtMarginSetup;
        }

        /// <summary>
        /// Delete Margin Setup by PK : Success return 1 , Fail Return 0
        /// </summary>
        /// <param name="currPK"></param>
        /// <param name="user"></param>
        /// <returns>int</returns>
        public static int DeleteMarginSetup(int marginSetupPK, DateTime? lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_MAS_PK ,marginSetupPK),
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.P_LAST_MOD_DT, lastModDate == null ? (Object)DBNull.Value : lastModDate),
                new DBService.Parameters(GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Configurations.MarginSetup.Procedures.SPINV_MARGIN_SETUP_MST_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Administration.Configurations.MarginSetup.Parameters.RETVAL]).Value);
        }
    }
}
