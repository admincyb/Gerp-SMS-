using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;

namespace ERPManager
{
    public class FinCashBankMstManager : IFinCashBankMstManager
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
        public FinCashBankMstManager(ERPEntities currentEntity)
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
        /// Gets list of Bank Master after filtering,sorting for filling auto complete list
        /// </summary>
        /// <param name="finCashBankMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Bank Master</returns>
        public List<FIN_CASH_BANK_MST> GetFinCashBankMstAutoCompleteList(FIN_CASH_BANK_MST finCashBankMstObj, ServiceUtility utilityObj) 
        {
            List<FIN_CASH_BANK_MST> FinCashBankListObj = new List<FIN_CASH_BANK_MST>();

            IQueryable<FIN_CASH_BANK_MST> finCashBankMstQuery;

            try
            {
                finCashBankMstQuery = (from cbm in this.currentEntity.FIN_CASH_BANK_MST
                                       where cbm.CBM_ACTIVE  == finCashBankMstObj.CBM_ACTIVE
                                            && (utilityObj.IsSBUSpecific==true ? cbm.CBM_BIZUNIT==finCashBankMstObj.CBM_BIZUNIT : true)
                                            //&& cbm.CBM_NAME.StartsWith(utilityObj.FilterValue)
                                            && (cbm.CBM_CODE + " - " + cbm.CBM_NAME).Contains(utilityObj.FilterValue)
                                            && cbm.CBM_TYPE == finCashBankMstObj.CBM_TYPE
                                       select cbm);

                FinCashBankListObj = finCashBankMstQuery.ToList();
            }
            catch
            {

            }

            return FinCashBankListObj;
        }

        public List<FIN_CASH_BANK_MST> GetFinBankMstByPK(short bankPk)
        {
            List<FIN_CASH_BANK_MST> FinCashBankListObj = new List<FIN_CASH_BANK_MST>();

            try
            {
                FinCashBankListObj = (from cbm in this.currentEntity.FIN_CASH_BANK_MST
                                      where cbm.CBM_PK == bankPk
                                      select cbm).ToList();
            }
            catch
            {

            }

            return FinCashBankListObj;
        }
        #endregion

        #region Private Methods

        #endregion

        
    }
}
