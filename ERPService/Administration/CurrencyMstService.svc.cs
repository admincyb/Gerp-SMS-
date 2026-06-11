using System;
using System.Collections.Generic;
using System.Diagnostics;
using ERPData;
using ERPManager;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CurrencyMstService" in code, svc and config file together.
    public class CurrencyMstService : ICurrencyMstService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public CurrencyMstService()
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

        public List<ADM_CURRENCY_MST> GetCurrencyListAutoCompleteList(ADM_CURRENCY_MST admCurrencyMstObj, ERPManager.ServiceUtility serviceUtilityObj)
        {
            AdmCurrencyMstManager admCurrencyMstManagerObj;
            try
            {
                admCurrencyMstManagerObj = new AdmCurrencyMstManager(currentContext);
                return admCurrencyMstManagerObj.GetCurrencyListAutoCompleteList(admCurrencyMstObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admCurrencyMstManagerObj = null;
            }
        }
                
        public string GetCurrencyCodeName(int currencyPk)
        {
            AdmCurrencyMstManager admCurrencyMstManagerObj;
            try
            {
                admCurrencyMstManagerObj = new AdmCurrencyMstManager(currentContext);
                return admCurrencyMstManagerObj.GetCurrencyCodeName(currencyPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admCurrencyMstManagerObj = null;
            }
        }

        public string GetCurrencyCode(int currencyPk)
        {
            AdmCurrencyMstManager admCurrencyMstManagerObj;
            try
            {
                admCurrencyMstManagerObj = new AdmCurrencyMstManager(currentContext);
                return admCurrencyMstManagerObj.GetCurrencyCode(currencyPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admCurrencyMstManagerObj = null;
            }
        }
        #endregion
    }
}
