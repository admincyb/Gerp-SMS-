using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Administration.Masters;
using BusinessObject.Administration.Masters;

namespace DataAccess.Administration.Masters
{
    public class CostCenterMasterDA
    {
        /// <summary>
        /// Cost Center Listing
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bizUnit"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataTable GetCostCenterList(BusinessObject.GridPrams gridParam, int bizUnit, string code, string name)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, gridParam.PageNumber),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, gridParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_CNM_CODE, code == string.Empty ? DBNull.Value.ToString() : code),
                new DBService.Parameters(Parameters.P_CNM_NAME, name ==  string.Empty ? DBNull.Value.ToString() :name),
                //new DBService.Parameters(Parameters.P_CNM_ACTIVE, status.HasValue? status : (object)DBNull.Value),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_COST_CENTER_MST_GET_LIST, colParameters);
        }

        /// <summary>
        /// Save Cost Center
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SaveCostCenterMaster(CostCenterMasterBO objCostCenter)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_CNM_PK, objCostCenter.CNM_PK),
                new DBService.Parameters(Parameters.P_CNM_CODE, objCostCenter.CNM_CODE),
                new DBService.Parameters(Parameters.P_CNM_NAME, objCostCenter.CNM_NAME),
                new DBService.Parameters(Parameters.P_CNM_DESC, objCostCenter.CNM_DESC),
                new DBService.Parameters(Parameters.P_CNM_GROUP, objCostCenter.CNM_GROUP),
                new DBService.Parameters(Parameters.P_CNM_DEPART, objCostCenter.CNM_DEPARMENT > 0 ? objCostCenter.CNM_DEPARMENT : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_CNM_SUBDEPT, objCostCenter.CNM_SUBDEPARMENT > 0 ? objCostCenter.CNM_SUBDEPARMENT : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_ACTIVE, objCostCenter.CNM_ACTIVE),
                new DBService.Parameters(Parameters.P_CNM_DEPT, objCostCenter.CNM_DEPT),
                new DBService.Parameters(Parameters.P_CNM_COMPANY, objCostCenter.CNM_COMPANY > 0 ? objCostCenter.CNM_COMPANY : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_USER_PK, objCostCenter.USER_PK),
                new DBService.Parameters(Parameters.P_BIZUNIT, objCostCenter.CNM_BIZUNIT),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT, objCostCenter.LAST_MOD_DT),
                new DBService.Parameters(Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_COST_CENTER_MST_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Cost Center by PK
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bizUnit"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataTable GetCostCenterByPK(int cnmPk, int status, int bizUnit, int IssueSubDept = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_CNM_PK, cnmPk),
                new DBService.Parameters(Parameters.P_ACTIVE, status),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_ISSUE_SUB_DEPT, IssueSubDept>0 ? IssueSubDept : (object)DBNull.Value )

            };
            DataTable dt = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_COST_CENTER_MST_GET_KV, colParameters);
            return dt;
        }

        /// <summary>
        /// Delete Cost Center
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int? DeleteCostCenter(int CurrPK, DateTime dateTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_CNM_PK, CurrPK == 0 ? (object)DBNull.Value : CurrPK) ,
                new DBService.Parameters(Parameters.P_LAST_MOD_DT, dateTime) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_COST_CENTER_MST_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Cost Center Status
        /// </summary>
        /// <param name="PEL_PK"></param>
        /// <param name="Status"></param>
        /// <param name="UserPk"></param>
        /// <param name="LastModDate"></param>
        /// <returns></returns>
        public static int? UpdateCostCenterStatus(int CNM_PK, int Status, int UserPk, DateTime? LastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_CNM_PK, CNM_PK),
                new DBService.Parameters(Parameters.P_ACTIVE, Status),
                new DBService.Parameters(Parameters.P_USER_PK, UserPk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT, LastModDate.HasValue? LastModDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_COST_CENTER_MST_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get Group
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetCostCenterGroup(int bizUnit, int groupType, int groupValue, int active)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[]
                {
                new DBService.Parameters(Parameters.P_CONPK ,0),
                new DBService.Parameters(Parameters.P_CON_BIZUNIT , bizUnit),
                new DBService.Parameters(Parameters.P_GROUPTYPE  , groupType),
                new DBService.Parameters(Parameters.P_GROUPVALUE , groupValue),
                new DBService.Parameters(Parameters.P_CONACTIVE , active)
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPADM_CONST_MST_GET_KV, colParameters);
            }
            return ds.Tables[0];
        }

        /// <summary>
        /// Cost Center Listing
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bizUnit"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataTable GetCostCenterAccountList(BusinessObject.GridPrams gridParam, int cost_center_PK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, gridParam.PageNumber),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, gridParam.PageSize),
                // new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , gridParam.SortBy== string.Empty ? "%" : gridParam.SortBy),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , gridParam.SortDirection== string.Empty ? "%" : gridParam.SortDirection),
                new DBService.Parameters(Parameters.P_CNM_PK, cost_center_PK == 0 ? (object)DBNull.Value :cost_center_PK)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPFIN_COA_COST_CENTER_MPG_GET_LIST, colParameters);
        }


        /// <summary>
        /// Cost Center Listing
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bizUnit"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataTable GetCostCenterAccountListByGroup(int Group_PK, string SearchText, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

                new DBService.Parameters(Parameters.P_CNM_GROUP, Group_PK>0?Group_PK:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_VALUE, SearchText),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_COST_CENTER_MST_AUTO, colParameters);
        }
        public static DataTable GetBudgetCostCenterAccountListByGroup(int Group_PK, string SearchText, int bizUnit, int Budget)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

                new DBService.Parameters(Parameters.P_CNM_GROUP, Group_PK>0?Group_PK:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_VALUE, SearchText),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_BGH_PK, Budget>0?Budget:(object)DBNull.Value),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_COST_CENTER_MST_AUTO, colParameters);
        }
        /// <summary>
        /// Main Activity List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetMainActivityList(int bizUnit, int currPK, int? status)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_EAM_PK, currPK == 0 ? (object)DBNull.Value : currPK),
                new DBService.Parameters(Parameters.P_EAM_ACTIVE, status >0 ? status : 0),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPPRD_EMP_ACTIVITY_MST_AUTO, colParameters);
        }

        public static DataTable GetCostCenterAccountListWithOutGroup(string SearchText, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_CNM_GROUP,  (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_VALUE, SearchText),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_COST_CENTER_MST_AUTO, colParameters);
        }



        /// <summary>
        /// Save Activity
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SaveActivityMaster(string xmlDoc)//, out string trxNo, out DataTable dtErrorList
        {
            //dtErrorList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPPRD_EMP_ACTIVITY_MST_SAVE, colParameters);
            //if (dsResult != null && dsResult.Tables.Count > 0)
            //    dtErrorList = dsResult.Tables[0];
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            //trxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <returns></returns>
        public static string GetActivityDetails(int CurrPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_EAM_PK, CurrPK),
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPPRD_EMP_ACTIVITY_MST_GET_KV, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        /// <summary>
        /// Activity List
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bizUnit"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataTable GetActivityList(BusinessObject.GridPrams gridParam, int bizUnit, string code, string name, int teamPK, int mainActivityPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, gridParam.PageNumber),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, gridParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_EAM_CODE, code == string.Empty ? DBNull.Value.ToString() : code),
                new DBService.Parameters(Parameters.P_EAM_NAME, name ==  string.Empty ? DBNull.Value.ToString() :name),
                new DBService.Parameters(Parameters.P_EAM_TEAM, teamPK==0? (object)DBNull.Value :teamPK),
                new DBService.Parameters(Parameters.P_EAM_MAIN_ACTIVITY_PK, mainActivityPK>0?mainActivityPK:(object)DBNull.Value),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPPRD_EMP_ACTIVITY_MST_GET_LIST, colParameters);
        }

        ///

        public static int? DeleteActivity(int CurrPK, DateTime dateTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_EAM_PK, CurrPK == 0 ? (object)DBNull.Value : CurrPK) ,
                new DBService.Parameters(Parameters.P_LAST_MOD_DT, dateTime) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPPRD_EMP_ACTIVITY_MST_GET_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int? UpdateActivityStatus(int ActivityPK, int Status, int UserPk, DateTime? LastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_EAM_PK, ActivityPK),
                new DBService.Parameters(Parameters.P_ACTIVE, Status),
                new DBService.Parameters(Parameters.P_USER_PK, UserPk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT, LastModDate.HasValue? LastModDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPPRD_EMP_ACTIVITY_MST_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
