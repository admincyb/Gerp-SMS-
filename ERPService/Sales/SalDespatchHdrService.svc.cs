using System;
using System.Collections.Generic;
using ERPData;
using ERPManager;
using System.Data;
using System.Diagnostics;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SalDespatchHdrService" in code, svc and config file together.
    public class SalDespatchHdrService : ISalDespatchHdrService
    {
         #region Private Variables
        ERPEntities currentContext;
        #endregion
       #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public SalDespatchHdrService()
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
        /// <param name="SaleorderHdrObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<SAL_DESPATCH_HDR> GetDespatchHdr(SAL_ORDER_HDR SaleorderHdrObj, ServiceUtility utilityObj = null)
        {
            SalDespatchHdrManager objSalDespatchHdrManager;
            try
            {
                objSalDespatchHdrManager = new SalDespatchHdrManager(currentContext);
                return objSalDespatchHdrManager.GetDespatchHdr(SaleorderHdrObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSalDespatchHdrManager = null;
            }
        }


       
        #endregion
    }
}
