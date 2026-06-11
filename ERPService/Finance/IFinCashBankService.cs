using System.Collections.Generic;
using System.ServiceModel;
using ERPData;
using ERPManager;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFinCashBankService" in both code and config file together.
    [ServiceContract]
    public interface IFinCashBankService
    {
        [OperationContract]
        List<FIN_CASH_BANK_MST> GetFinCashBank(FIN_CASH_BANK_MST FinCashBankMstObj, ServiceUtility utilityObj);
        [OperationContract]
        int SaveFinCashBank(List<FIN_CASH_BANK_MST> finCashBankMstList);
        [OperationContract]
        int DeleteFinCashBank(List<FIN_CASH_BANK_MST> finCashBankMstList);
    }
}
