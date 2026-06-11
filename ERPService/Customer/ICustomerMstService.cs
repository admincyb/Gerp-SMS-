using System.Collections.Generic;
using System.ServiceModel;
using ERPData;
using ERPManager;
using System;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICustomerMstService" in both code and config file together.
    [ServiceContract]
    public interface ICustomerMstService
    {
        #region Customer Master Functions
        [OperationContract]
        List<CRM_CUSTOMER_MST> GetCustomerListAutoCompleteList(CRM_CUSTOMER_MST salCustomerObj, ServiceUtility utilityObj);
        #endregion
    }
}
