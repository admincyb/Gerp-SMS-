using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Common;

namespace DataAccess.Finance
{
    public class CommSetupDL
    {
        /// <summary>
        /// Get Agents
        /// </summary>
        /// <param name="agentPk"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetAgentDetails(int? agentPk, int bizUnit, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_VEN_PK, agentPk??0),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.Active, active)
            };

            DataTable dtAgents = new DataTable();
            dtAgents = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETAGENTLIST, colParameters).Tables[0];
            return dtAgents;
        }
        /// <summary>
        /// Delete Customer 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static int DeleteCustomer(int CustId, int AgentId, int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_CUS_PK, CustId),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_AGENT_PK, AgentId),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, sbu),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                //new DBService.Parameters(CommonConstants.RETMSG, string.Empty, 4000,ParameterDirection.Output, DBService.ParameterType.NVarChar),

                };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_AGENT_COMMISION_DTL_DELETE, colParameters);
            //msg = (((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETMSG]).Value).ToString();
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }
        /// <summary>
        /// <summary>
        /// Get Mapped Customers
        /// </summary>
        /// <param name="agentPk"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetMappedCustomerList(int agentPk, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_AGENT, agentPk),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizUnit)
            };

            DataTable dtCustomers = new DataTable();
            dtCustomers = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETMAPPEDCUSTOMERLIST, colParameters).Tables[0];
            return dtCustomers;
        }

        /// <summary>
        /// Get All Customers
        /// </summary>
        /// <param name="agentPk"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetAlldCustomerList(int? custPk, int bizUnit, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_CUS_PK, custPk??0),
                  new DBService.Parameters(GTIService.Constants.Finance.Parameters.Active, active),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizUnit)
            };

            DataTable dtCustomers = new DataTable();
            dtCustomers = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETALLCUSTOMERLIST, colParameters).Tables[0];
            return dtCustomers;
        }

        /// <summary>
        /// Get Rate Type
        /// </summary>
        /// <param name="ratePk"></param>
        /// <param name="active"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetRateType(int? ratePk, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.CFG_PK, ratePk??0),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.Active, active),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.CFG_TYPE, "COMMISSION RATE TYPE")
            };

            DataTable dtRateTypes = new DataTable();
            dtRateTypes = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETGSTREPORTLIST, colParameters).Tables[0];
            return dtRateTypes;
        }

        /// <summary>
        /// Get Mapped Commision List
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="customerId"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetMappedCommisionList(int agentId, int customerId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_AGENT, agentId),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_CUSTOMER, customerId)
            };

            DataTable dtMappedCommisions = new DataTable();
            dtMappedCommisions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_AGENT_COMMISION_GET, colParameters).Tables[0];
            return dtMappedCommisions;
        }

        /// <summary>
        /// Save Commision Details
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveCommisionDetails(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_AGENT_COMMISION_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get Margin Setup Type
        /// </summary>
        /// <param name="ratePk"></param>
        /// <param name="active"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetMarginSetupType(int? ratePk, int active, int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.CFG_PK, ratePk??0),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.Active, active),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.CFG_TYPE, "MARGIN SETUP"),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT_ADD, bizunit)
            };

            DataTable dtRateTypes = new DataTable();
            dtRateTypes = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETGSTREPORTLIST, colParameters).Tables[0];
            return dtRateTypes;
        }

        public static DataSet GetCOAOpeningBalanceList(int bizunit, int finYearPk, int transNoPK, string transDate, BusinessObject.GridPrams gridParamObj)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_PK, finYearPk > 0 ? finYearPk : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_COH_PK, transNoPK > 0 ? transNoPK : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_COH_DATE, string.IsNullOrEmpty(transDate) ? (Object)DBNull.Value : transDate),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizunit),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PAGE_NUM, gridParamObj.PageNumber),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PAGE_SIZE, gridParamObj.PageSize),
            };

            DataSet dsResult = new DataSet();
            dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_COA_OPENING_BAL_GET_LIST, colParameters);
            return dsResult;
        }

        public static string GetCOAOpeningBalance(int FinYearPK, int bizunit)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_PK, FinYearPK),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizunit),
            };

            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_COA_OPENING_BAL_GET, colParameters).Tables[0];
            for (int i = 0; i < dtResult.Rows.Count; i++)
                strRetVal += dtResult.Rows[i][0].ToString();
            return strRetVal;
        }

        public static int? SaveCOAFinYearOpeninBalanceWkf(string xmlDoc, out int refID, out string transNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_XML, xmlDoc),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_NO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_COA_OPENING_BAL_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_VAL]).Value);
            refID = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_REF_PK]).Value);
            transNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_NO]).Value);
            return result;
        }

        public static string GetCOAOpeningBalanceByPK(int COH_PK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_COH_PK, COH_PK)
            };

            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_COA_OPENING_BAL_GET_KV, colParameters).Tables[0];
            for (int i = 0; i < dtResult.Rows.Count; i++)
                strRetVal += dtResult.Rows[i][0].ToString();
            return strRetVal;
        }

        public static DataTable GetFinYearOpeningNumbers(int bizunit, string searchKey)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizunit),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_VALUE, searchKey)
            };

            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_COA_OPENING_BAL_AUTO, colParameters).Tables[0];
            return dtResult;
        }

        public static int DeleteFinYearOpening(int COH_PK, int UserPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_COH_PK, COH_PK),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_USERPK, UserPK),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_COA_OPENING_BAL_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static string GetCOATreeNodes(int P_COA_PK, int P_BIZUNIT)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_COA_PK, P_COA_PK > 0 ? P_COA_PK : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, P_BIZUNIT)
            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_COA_MST_TREE_GET, colParameters).Tables[0];
            for (int i = 0; i < dtResult.Rows.Count; i++)
                strRetVal += dtResult.Rows[i][0].ToString();
            return strRetVal;
        }

        public static DataTable GetAccountGroup(int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizunit)
            };

            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_COA_MST_CWIP_ACCOUNT_GET, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetCOAByParent(int P_COA_PK, int CWIPAssetPK, string SearchValue = "")
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_COA_PK, P_COA_PK),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_CWH_PK, CWIPAssetPK > 0 ? CWIPAssetPK : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_VALUE, string.IsNullOrEmpty(SearchValue) ? "%" : SearchValue)
            };

            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_COA_MST_CHILD_ACCOUNT_GET, colParameters).Tables[0];
            return dtResult;
        }

        public static string GetCOATransactionList(int P_COA_PK, int CWIPAssetPK, string poNumber = null)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_COA_PK, P_COA_PK),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_CWH_PK, CWIPAssetPK > 0 ? CWIPAssetPK : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PO_NUMBER, poNumber != null ?poNumber:(Object)DBNull.Value)
            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_TRX_COA_WISE_GET_LIST, colParameters).Tables[0];
            for (int i = 0; i < dtResult.Rows.Count; i++)
                strRetVal += dtResult.Rows[i][0].ToString();
            return strRetVal;
        }

        public static int? SaveCWIPAssetWkf(string xmlDoc, out int refID, out string transNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_XML, xmlDoc),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_NO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_TRX_CWIP_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_VAL]).Value);
            refID = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_REF_PK]).Value);
            transNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_NO]).Value);
            return result;
        }

        public static DataSet GetCWIPAssetList(BusinessObject.GridPrams gridParamObj, int BizunitPK, string pageUrl, int AccPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_SER_VAL, gridParamObj.SearchValue == string.Empty ? (Object)DBNull.Value : gridParamObj.SearchValue),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_SORT_DIR, gridParamObj.SortDirection),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_SORT_BY, gridParamObj.SortBy== string.Empty ? (Object)DBNull.Value : gridParamObj.SortBy),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PAGE_NUM, gridParamObj.PageNumber),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PAGE_SIZE, gridParamObj.PageSize),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_USERPK, gridParamObj.UserPK),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, BizunitPK),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.FROM_DATE, gridParamObj.FromDate == string.Empty ? (Object)DBNull.Value : gridParamObj.FromDate),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.TO_DATE, gridParamObj.ToDate ==  string.Empty ? (Object)DBNull.Value : gridParamObj.ToDate),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_STATUS, gridParamObj.statusPk > -1 ? gridParamObj.statusPk : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_COA_PK, AccPK > 0 ? AccPK : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PAGE_URL, pageUrl),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PO_NUMBER, gridParamObj.SearchValue == string.Empty ? (Object)DBNull.Value : gridParamObj.PoNumber)
            };

            DataSet dsResult = new DataSet();
            dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_TRX_CWIP_GET_LIST, colParameters);
            return dsResult;
        }

        public static string GetCWIPAsset(int CWIPAssetPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_CWH_PK, CWIPAssetPK)
            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_TRX_CWIP_GET_KV, colParameters).Tables[0];
            for (int i = 0; i < dtResult.Rows.Count; i++)
                strRetVal += dtResult.Rows[i][0].ToString();
            return strRetVal;
        }

        public static int DeleteCWIPAsset(int CWIPAssetPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_CWH_PK, CWIPAssetPK),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_TRX_CWIP_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static DataTable GetCWIPFieldValues(string FieldName, string SearchKey, int Bizunit, int DeptPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FLD_NAME, FieldName),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_VALUE, SearchKey == string.Empty ? "%" : SearchKey),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT, Bizunit),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_DEPT, DeptPK),
            };
            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_TRX_CWIP_HDR_AUTO, colParameters).Tables[0];
            return dtResult;
        }
    }
}
