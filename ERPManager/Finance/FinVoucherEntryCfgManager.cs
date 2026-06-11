using ERPData;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using ERP.Utilities;
using System.Linq;

namespace ERPManager
{
    public class FinVoucherEntryCfgManager : IFinVoucherEntryCfgManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Bank Voucher Entry Config Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public FinVoucherEntryCfgManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public List<FIN_VOUCHER_ENTRY_CFG> GetVcoucherEntryCfg(FIN_VOUCHER_ENTRY_CFG FinVchrEntryCfgObj, ServiceUtility utilityObj)
        {
            List<FIN_VOUCHER_ENTRY_CFG> Obj_FIN_VOUCHER_ENTRY_CFG_List = null;
            IQueryable<FIN_VOUCHER_ENTRY_CFG> Obj_FIN_VOUCHER_ENTRY_CFG_Qry;
            try
            {
                Obj_FIN_VOUCHER_ENTRY_CFG_Qry=(from vc in this.currentEntity.FIN_VOUCHER_ENTRY_CFG
                                               where    vc.VEC_ACTIVE == FinVchrEntryCfgObj.VEC_ACTIVE
                                                    &&  vc.VEC_TYPE == FinVchrEntryCfgObj.VEC_TYPE
                                               select vc 
                                            );

                Obj_FIN_VOUCHER_ENTRY_CFG_List = Obj_FIN_VOUCHER_ENTRY_CFG_Qry.SortRecords<FIN_VOUCHER_ENTRY_CFG>(utilityObj).ToList();
                
                return Obj_FIN_VOUCHER_ENTRY_CFG_List;
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //throw new NotImplementedException();
        }
        #endregion
    }
}
