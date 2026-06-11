using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    interface IFinCashBankMstManager
    {
        List<FIN_CASH_BANK_MST> GetFinCashBankMstAutoCompleteList(FIN_CASH_BANK_MST finCashBankMstObj, ServiceUtility utilityObj);
    }
}
