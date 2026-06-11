using System.Collections.Generic;
using ERPData;

namespace ERPManager
{
    public interface IFinCashBankManager
    {
        #region Private Variables
        List<FIN_CASH_BANK_MST> GetFinCashBank(FIN_CASH_BANK_MST FinCashBankMstObj, ServiceUtility utilityObj);
        int SaveFinCashBank(List<FIN_CASH_BANK_MST> finCashBankMstList);
        int DeleteFinCashBank(List<FIN_CASH_BANK_MST> finCashBankMstList);
        #endregion
    }
}
