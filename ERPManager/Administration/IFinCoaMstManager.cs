using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    public interface IFinCoaMstManager
    {
        List<FIN_COA_MST> GetFinCoaMst(FIN_COA_MST FinCoaMstObj, ServiceUtility utilityObj);
        int FinCoaBalanceSave(int coaPK, decimal coaAmount);
        int SaveAccountsMaster(List<FIN_COA_MST> accountMstList,int AddParentChildValidation);
        int DeleteAccountsMaster(List<FIN_COA_MST> accountMstList);
        List<FIN_COA_MST> GetCoaMstAutoCompleteList(FIN_COA_MST finCoaMstObj, string voucherType, int accType, ServiceUtility utilityObj,int CompanyPK);
        List<FIN_COA_MST> GetAccountCoaMstAutoCompleteList(FIN_COA_MST finCoaMstObj, int accType, ServiceUtility utilityObj);
    }
}
