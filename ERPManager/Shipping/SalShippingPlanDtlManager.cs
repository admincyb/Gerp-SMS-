using System;
using System.Collections.Generic;
using System.Linq;
using ERPData;
using ERP.Utilities;
using System.Diagnostics;


namespace ERPManager
{
    public class SalShippingPlanDtlManager : ISalShippingPlanDtlManager
    {
        #region Private Variables
        /// <summary>
        /// Gets or sets currency db context
        /// </summary>
        ERPEntities currentEntity;
        #endregion
        #region Manager Methods
        
        /// <summary>
        /// Initializes a new instance of country master manager
        /// </summary>
        /// <param name="currentEntity" value="Current db context"></param>
        public SalShippingPlanDtlManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public List<SAL_SHIPPING_PLAN_DTL> GetShippingPlanDtl(SAL_SHIPPING_PLAN_DTL salShippingPlanDtlObj, ServiceUtility utilityObj = null)
        {
            IQueryable<SAL_SHIPPING_PLAN_DTL> qry;
            try
            {
                //Set page size one if not given
                utilityObj.PageSize = (utilityObj.PageSize == 0) ? 1 : utilityObj.PageSize;

                qry = this.currentEntity.SAL_SHIPPING_PLAN_DTL;
                if (salShippingPlanDtlObj.SND_PK != 0)
                    qry = qry.Where(snd => snd.SND_PK == salShippingPlanDtlObj.SND_PK);
                if (salShippingPlanDtlObj.SND_PK < 1 && salShippingPlanDtlObj.SND_ACTIVE == 1) //select all active records
                    qry = qry.Where(snd => snd.SND_ACTIVE == 1);
                else if (salShippingPlanDtlObj.SND_PK > 0 && salShippingPlanDtlObj.SND_ACTIVE == 1) // select all active +(union) having given pk
                    qry = qry.Where(snd => snd.SND_ACTIVE == salShippingPlanDtlObj.SND_ACTIVE || snd.SND_PK == salShippingPlanDtlObj.SND_PK);
                if (salShippingPlanDtlObj.SND_MOD_BY > 0)
                    qry = qry.Where(snd => (snd.SND_MOD_BY == salShippingPlanDtlObj.SND_MOD_BY));
                if (salShippingPlanDtlObj.SND_PLAN_HDR > 0)
                {
                    qry = qry.Where(snd => snd.SND_PLAN_HDR == salShippingPlanDtlObj.SND_PLAN_HDR && snd.SND_PLAN_QTY > 0);//qty checking added to avoid items without qty "&& snd.SND_PLAN_QTY > 0"
                }
                qry = qry.OrderBy(c => c.SAL_ORDER_DTL.SOD_NO).ThenBy(r => r.SAL_ORDER_DTL.SOD_SL_NO);  
                return qry.SortRecords<SAL_SHIPPING_PLAN_DTL>(utilityObj).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                //Disposing objects
            }
        }
        #endregion
    }
}
