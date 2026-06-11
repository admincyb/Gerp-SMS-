using System;
using System.Collections.Generic;
using ERPData;
using ERPManager;
using System.Data;
using System.Diagnostics;

namespace ERPService
{

    public class SalDespatchDtlService : ISalDespatchDtlService
    {
       #region Private Variables
        ERPEntities currentContext;
        #endregion
       #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public SalDespatchDtlService()
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DespatchDtlObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<SAL_DESPATCH_DTL> GetDespatchDtl(SAL_DESPATCH_DTL DespatchDtlObj, ServiceUtility utilityObj = null)
        {
            SalDespatchDtlManager objSalDespatchDtlManager;
            try
            {
                objSalDespatchDtlManager = new SalDespatchDtlManager(currentContext);
                return objSalDespatchDtlManager.GetDespatchDtl(DespatchDtlObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSalDespatchDtlManager = null;
            }
        }
        public string GetDespatchQty(SAL_DESPATCH_DTL DespatchDtlObj, ServiceUtility utilityObj = null)
        {
            SalDespatchDtlManager objSalDespatchDtlManager;
            try
            {
                objSalDespatchDtlManager = new SalDespatchDtlManager(currentContext);
                return objSalDespatchDtlManager.GetDespatchQty(DespatchDtlObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSalDespatchDtlManager = null;
            }
        }
        #endregion
    }
}
