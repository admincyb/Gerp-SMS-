using System.Collections.Generic;
using ERPData;

namespace ERPManager
{
   public interface ICustomerManager
    {
       List<CRM_CUSTOMER_MST> GetCustomerListAutoCompleteList(CRM_CUSTOMER_MST salCustomerObj, ServiceUtility utilityObj);
      
    }
}
