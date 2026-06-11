using System;
using System.Collections.Generic;
using System.Diagnostics;
using ERPData;
using ERPManager;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FinVoucherEntryCfgService" in code, svc and config file together.
    public class FinVoucherEntryCfgService : IFinVoucherEntryCfgService, IFinVoucherEntryCfgManager
    {
        #region Private Variables
        ERPEntities  currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public FinVoucherEntryCfgService()
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

        public List<FIN_VOUCHER_ENTRY_CFG> GetVcoucherEntryCfg(FIN_VOUCHER_ENTRY_CFG FinVchrEntryCfgObj, ServiceUtility utilityObj)
        {
            FinVoucherEntryCfgManager objFinVcrEntryCfgManager;

            try
            {
                objFinVcrEntryCfgManager = new FinVoucherEntryCfgManager(currentContext);
                return objFinVcrEntryCfgManager.GetVcoucherEntryCfg(FinVchrEntryCfgObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinVcrEntryCfgManager = null;
            }
        }
        #endregion
    }
}