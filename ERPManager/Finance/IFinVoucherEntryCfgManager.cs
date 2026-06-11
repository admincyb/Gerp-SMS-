using System.Collections.Generic;
using ERPData;

namespace ERPManager
{
    public interface IFinVoucherEntryCfgManager
    {
        List<FIN_VOUCHER_ENTRY_CFG> GetVcoucherEntryCfg(FIN_VOUCHER_ENTRY_CFG FinVchrEntryCfgObj, ServiceUtility utilityObj);
    }
}
