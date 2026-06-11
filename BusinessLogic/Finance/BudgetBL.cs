using BusinessObject;
using DataAccess.Finance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Finance
{
    public sealed class BudgetBL
    {
        public static DataTable GetFinYear(int bizUnit)
        {
            return BudgetDL.GetFinYear(bizUnit);
        }
        public static DataTable GetBudgetList(int userPK, int bizUnit, int BudgetNo, int? FinYear = null)
        {
            return BudgetDL.GetBudgetList(userPK, bizUnit, BudgetNo, FinYear);
        }

        public static DataTable GetCoaParent(string searchKey, string searchField, int active, int bizUnit, int isGroup, int AccountPk)
        {
            return BudgetDL.GetCoaParent(searchKey, searchField, active, bizUnit, isGroup, AccountPk);
        }
        public static int SaveBudgetDetails(string pXML, ref DataTable dtOut)
        {
            return BudgetDL.SaveBudgetDetails(pXML, ref dtOut);
        }
        public static string GetBudget(int BudgetPK)
        {
            return BudgetDL.GetBudget(BudgetPK);
        }
        public static int DeleteBudgetList(int BudgetPK)
        {
            return BudgetDL.DeleteBudgetList(BudgetPK);
        }
        public static DataTable GetBudgetHistory(int CurrPK)
        {
            return BudgetDL.GetBudgetHistory(CurrPK);
        }
        public static DataTable GetBudgetDtlHistory(int BudgetDtlPK)
        {
            return BudgetDL.GetBudgetDtlHistory(BudgetDtlPK);
        }
        public static DataTable GetBudgetNo(string searchKey)
        {
            return BudgetDL.GetBudgetNo(searchKey);
        }
        public static DataSet GetBudgetDetailsList(int BudgetPk, int PageIndex, int pageSize, int? Plant = null, int? CostCenter = null, int? Accno = null)
        {
            return BudgetDL.GetBudgetDetailsList(BudgetPk, PageIndex, pageSize, Plant, CostCenter, Accno);
        }
    }
}
