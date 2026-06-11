using System;
using System.Collections.Generic;
using System.Diagnostics;
using ERPData;
using ERPManager;
using System.Data;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FinCashBankService" in code, svc and config file together.
    public class FinCashBankService : IFinCashBankService, IFinCashBankManager
    {
        #region Private Variables
        ERPEntities  currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public FinCashBankService()
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

        public List<FIN_CASH_BANK_MST> GetFinCashBank(FIN_CASH_BANK_MST FinCashBankMstObj, ServiceUtility utilityObj)
        {
            FinCashBankManager objFinCashBankManager;

            try
            {
                objFinCashBankManager = new FinCashBankManager(currentContext);
                return objFinCashBankManager.GetFinCashBank(FinCashBankMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCashBankManager = null;
            }
        }

        public int SaveFinCashBank(List<FIN_CASH_BANK_MST> finCashBankMstList)
        {
            FinCashBankManager FinCashBankMgr;
            int? cashbankPK;
            try
            {
                FinCashBankMgr = new FinCashBankManager(currentContext);
                cashbankPK = FinCashBankMgr.SaveFinCashBank(finCashBankMstList);
                currentContext.SaveChanges();
                return cashbankPK.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                FinCashBankMgr = null;
                cashbankPK = null;
            }
        }

        public int DeleteFinCashBank(List<FIN_CASH_BANK_MST> finCashBankMstList)
        {
            FinCashBankManager FinCashBankMgr;
            try
            {
                FinCashBankMgr = new FinCashBankManager(currentContext);
                int result = FinCashBankMgr.DeleteFinCashBank(finCashBankMstList);
                currentContext.SaveChanges();
                return result;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                //throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
                return -1;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                FinCashBankMgr = null;
            }
        }

        #endregion
    }
}