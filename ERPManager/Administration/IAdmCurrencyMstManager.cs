using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    interface IAdmCurrencyMstManager
    {
        List<ADM_CURRENCY_MST> GetCurrencyListAutoCompleteList(ADM_CURRENCY_MST admCurrencyMstObj, ServiceUtility utilityObj);
        string GetCurrencyCodeName(int currencyPk);
    }
}
