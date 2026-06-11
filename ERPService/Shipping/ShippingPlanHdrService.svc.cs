using System;
using System.Collections.Generic;
using ERPData;
using System.Diagnostics;
using ERPManager;


namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ShippingPlanHdrService" in code, svc and config file together.
    public class ShippingPlanHdrService : IShippingPlanHdrService, ISalShippingPlanDtlManager
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// Constructor for ContainerEvaluationService Service
        /// </summary>
        public ShippingPlanHdrService()
        {
            try
            {
                currentContext = new ERPEntities();
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        public List<SAL_SHIPPING_PLAN_DTL> GetShippingPlanDtl(SAL_SHIPPING_PLAN_DTL salShippingPlanDtlObj, ServiceUtility utilityObj = null)
        {
            SalShippingPlanDtlManager salShippingPlanDtlManager;
            try
            {
                salShippingPlanDtlManager = new SalShippingPlanDtlManager(currentContext);
                return salShippingPlanDtlManager.GetShippingPlanDtl(salShippingPlanDtlObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                salShippingPlanDtlManager = null;
            }
        }
        #endregion
    }
}
