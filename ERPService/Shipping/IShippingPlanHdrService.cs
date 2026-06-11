using System.Collections.Generic;
using System.ServiceModel;
using ERPData;
using ERPManager;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IShippingPlanHdrService" in both code and config file together.
    [ServiceContract]
    public interface IShippingPlanHdrService
    {
        [OperationContract]
        List<SAL_SHIPPING_PLAN_DTL> GetShippingPlanDtl(SAL_SHIPPING_PLAN_DTL salShippingPlanDtlObj, ServiceUtility utilityObj = null);
    }
}
