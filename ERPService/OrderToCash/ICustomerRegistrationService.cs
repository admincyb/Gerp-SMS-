using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;
using System.Data;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICustomerRegistrationService" in both code and config file together.
    [ServiceContract]
    public interface ICustomerRegistrationService
    {
        [OperationContract]
        long SaveCrmCustomerMst(List<CRM_CUSTOMER_MST> crmCustomerMstList);
        [OperationContract]
        List<CRM_CUSTOMER_MST> GetCrmCustomerMst(CRM_CUSTOMER_MST crmCustomerMstObj, ServiceUtility serviceUtilityObj);
        [OperationContract]
        void ChangeObjectState(Object entityObj, EntityState entityState);
        [OperationContract]
        List<ADM_FORM_TAB_CONTROL_DTL> FormTabControlDtl(int controlPK);
        
    }
}
