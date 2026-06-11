using System.Collections.Generic;
using System.ServiceModel;
using ERPData;
using ERPManager;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFinVoucherEntryCfgService" in both code and config file together.
    [ServiceContract]
    public interface IFinVoucherEntryCfgService
    {
        [OperationContract]
        List<FIN_VOUCHER_ENTRY_CFG> GetVcoucherEntryCfg(FIN_VOUCHER_ENTRY_CFG FinVchrEntryCfgObj, ServiceUtility utilityObj);
    }
}
