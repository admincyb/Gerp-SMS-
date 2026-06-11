    using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;

namespace ERPManager
{
    public class AdmCurrencyMstManager : IAdmCurrencyMstManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Bank Master Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public AdmCurrencyMstManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Gets list of Currency Master after filtering,sorting for filling auto complete list
        /// </summary>
        /// <param name="admCurrencyMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Bank Master</returns>
        public List<ADM_CURRENCY_MST> GetCurrencyListAutoCompleteList(ADM_CURRENCY_MST admCurrencyMstObj, ServiceUtility utilityObj)
        {
            List<ADM_CURRENCY_MST> ADM_CURRENCY_MST_obj = new List<ADM_CURRENCY_MST>();

            IQueryable<ADM_CURRENCY_MST> ADM_CURRENCY_MST_Qry;

            try
            {
                ADM_CURRENCY_MST_Qry = (from cbm in this.currentEntity.ADM_CURRENCY_MST
                                        where cbm.CUR_ACTIVE  == admCurrencyMstObj.CUR_ACTIVE 
                                            && cbm.CUR_CODE.StartsWith(utilityObj.FilterValue)
                                       select cbm);

                ADM_CURRENCY_MST_obj = ADM_CURRENCY_MST_Qry.ToList();
            }
            catch
            {

            }

            return ADM_CURRENCY_MST_obj;
        }

        public string GetCurrencyCodeName(int currencyPk)
        {
            string currency ;

            currency = (from c in this.currentEntity.ADM_CURRENCY_MST 
                        where   c.CUR_PK == currencyPk
                        select  c.CUR_CODE + " - " + c.CUR_NAME ).FirstOrDefault();

            //if(!string.IsNullOrEmpty(currency))
            return currency;
            
        }

        public string GetCurrencyCode(int currencyPk)
        {
            string currency;

            currency = (from c in this.currentEntity.ADM_CURRENCY_MST
                        where c.CUR_PK == currencyPk
                        select c.CUR_CODE ).FirstOrDefault();

            //if(!string.IsNullOrEmpty(currency))
            return currency;

        }
        #endregion
    }
}
