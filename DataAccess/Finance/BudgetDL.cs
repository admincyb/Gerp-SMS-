using BusinessObject;
using GTIService.Constants.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Finance
{
    public sealed class BudgetDL
    {
        public static DataTable GetFinYear(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_YEAR_GET_KV, colParameters);
            return dsSet.Tables[0];
        }
        public static DataTable GetBudgetList(int userPk, int bizUnit, int BudgetNo, int? FinYear = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BGH_BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BGH_PK, BudgetNo>0?BudgetNo:(Object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BGH_YEAR,FinYear>0? FinYear:(Object)DBNull.Value),
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_BUDGET_LIST, colParameters);
            return dsSet.Tables[0];
        }

        public static DataTable GetCoaParent(string searchKey, string searchField, int active, int bizUnit, int isGroup, int AccountPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtData = new DataTable();
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_NAME, searchField),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchKey),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, bizUnit),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COA_PK, AccountPk>0?AccountPk:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COA_IS_GROUP, isGroup < 0 ?  (object)DBNull.Value: isGroup),
            };
            dtData = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_COA_MST_AUTO, colParameters).Tables[0];
            return dtData;
        }

        public static int SaveBudgetDetails(string strxml, ref DataTable dtOut)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_XML  , strxml),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.BUDGETDETAILVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.BUDGETDETAILNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.BUDGETDETAILREFPK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

             };
            dtOut = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPFIN_BUDGET_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters.BUDGETDETAILVALUE]).Value);
            return result;

        }
        public static string GetBudget(int itemPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataSet dsMapping;
            string result;
            result = string.Empty;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_BGH_PK,itemPk)
             };
            dsMapping = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPFIN_BUDGET_GET, colParameters);
            if (dsMapping != null && dsMapping.Tables.Count > 0)
            {
                foreach (DataRow dr in dsMapping.Tables[0].Rows)
                {
                    result += dr[0].ToString();
                }
            }
            return result;
        }

        public static int DeleteBudgetList(int BudgetPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_BGH_PK  , BudgetPK),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.BUDGETDETAILVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

             };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPFIN_BUDGET_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters.BUDGETDETAILVALUE]).Value);
            return result;

        }
        public static DataTable GetBudgetHistory(int CurrPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BGH_PK, CurrPK),
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_BUDGET_VERSION_GET, colParameters);
            return dsSet.Tables[0];
        }
        public static DataTable GetBudgetDtlHistory(int BudgetDtlPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BDG_PK, BudgetDtlPK),
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_BUDGET_DTL_VERSION_GET, colParameters);
            return dsSet.Tables[0];
        }
        public static DataTable GetBudgetNo(string searchKey)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL,searchKey),
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_BUDGET_NO_AUTO, colParameters);
            return dsSet.Tables[0];
        }
        public static DataSet GetBudgetDetailsList(int BudgetPk, int PageIndex, int pageSize, int? Plant = null, int? CostCenter = null, int? Accno = null)
        {
            DBService dbService = new DBService();
            StringBuilder strRetVal = new StringBuilder();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,PageIndex),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE,pageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BGH_PK,BudgetPk),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BDG_PLANT,Plant>0?Plant:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BDG_COST_CENTER,CostCenter>0?CostCenter:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BDG_COA,Accno>0?Accno:(object)DBNull.Value)
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_BUDGET_DTL_GET, colParameters);
            strRetVal.Append("");
            foreach (DataRow dr in dsSet.Tables[1].Rows)
            {
                strRetVal.Append(dr[0].ToString());
            }
            if (strRetVal.ToString() != "")
            {
                dsSet.Tables[1].Rows.Clear();
                dsSet.Tables[1].Rows.Add();
                dsSet.Tables[1].Rows[0][0] = strRetVal;
            }
            return dsSet;
        }
    }
}
