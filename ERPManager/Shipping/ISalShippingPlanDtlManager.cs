using System.Collections.Generic;
using ERPData;

namespace ERPManager
{
    public interface ISalShippingPlanDtlManager
    {
        List<SAL_SHIPPING_PLAN_DTL> GetShippingPlanDtl(SAL_SHIPPING_PLAN_DTL salShippingPlanDtlObj, ServiceUtility utilityObj = null);
    }
}
