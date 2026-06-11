using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Data;

namespace ERPManager
{
    interface ICrmCustomerMstManager
    {
        long? SaveCrmCustomerMst(List<CRM_CUSTOMER_MST> crmCustomerMstList);
        List<CRM_CUSTOMER_MST> GetCrmCustomerMst(CRM_CUSTOMER_MST crmCustomerMstObj, ServiceUtility serviceUtilityObj);
        void ChangeObjectState(object entityObj, EntityState entityState);
        List<ADM_FORM_TAB_CONTROL_DTL> FormTabControlDtl(int controlPK);
    }
}
