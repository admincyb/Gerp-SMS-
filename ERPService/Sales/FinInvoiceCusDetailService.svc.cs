using System;
using System.Collections.Generic;
using ERPData;
using ERPManager;
using System.Data;
using System.Diagnostics;

namespace ERPService
{

    public class FinInvoiceCusDetailService : IFinInvoiceCusDetailService
    {
    #region Private Variables
        ERPEntities currentContext;
   #endregion
       #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public FinInvoiceCusDetailService()
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
        /// Get Invoice Details
        /// </summary>
        /// <param name="InvoiceDtlObj"></param>
        /// <returns></returns>
        public List<FIN_INVOICE_CUS_DTL> GetInvoiceCusDtlByPK(FIN_INVOICE_CUS_DTL InvoiceDtlObj)
        {
            FinInvoiceCusDetailsManager FinInvoiceCusDetailsMgr;
            try
            {
                FinInvoiceCusDetailsMgr = new FinInvoiceCusDetailsManager(currentContext);
                return FinInvoiceCusDetailsMgr.GetInvoiceCusDtlByPK(InvoiceDtlObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                FinInvoiceCusDetailsMgr = null;
            }
        }



        public List<FIN_INVOICE_CUS_DTL> GetInvoiceCusDtlBySCPK(int DOpk, int SCpk)
        {
            FinInvoiceCusDetailsManager FinInvoiceCusDetailsMgr;
            try
            {
                FinInvoiceCusDetailsMgr = new FinInvoiceCusDetailsManager(currentContext);
                return FinInvoiceCusDetailsMgr.GetInvoiceCusDtlBySCPK(DOpk, SCpk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                FinInvoiceCusDetailsMgr = null;
            }
        }

        #endregion
    }
}
