using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using System.Diagnostics;
using ERPManager;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BankMstService" in code, svc and config file together.
    public class BankMstService : IBankMstService
    {
         #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public BankMstService()
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
        /// Gets list of Bank Master after filtering,sorting for filling auto complete list
        /// </summary>
        /// <param name="finCashBankMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Bank Master</returns>
        public List<FIN_CASH_BANK_MST> GetFinCashBankMstAutoCompleteList(FIN_CASH_BANK_MST finCashBankMstObj, ServiceUtility utilityObj)
        {
            FinCashBankMstManager finCashBankMstManagerMgr;
            try
            {
                finCashBankMstManagerMgr = new FinCashBankMstManager(currentContext);
                return finCashBankMstManagerMgr.GetFinCashBankMstAutoCompleteList(finCashBankMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finCashBankMstManagerMgr = null;
            }
        }

        public List<FIN_CASH_BANK_MST> GetFinBankMstByPK(short bankPk)
        {
            FinCashBankMstManager finCashBankMstManagerMgr;
            try
            {
                finCashBankMstManagerMgr = new FinCashBankMstManager(currentContext);
                return finCashBankMstManagerMgr.GetFinBankMstByPK(bankPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finCashBankMstManagerMgr = null;
            }
        }
        #endregion

        
    }
}
