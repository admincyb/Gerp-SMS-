using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;
using ERPManager;
using System.Data.SqlClient;
using System.Reflection;
using System.Transactions;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CustomerRegistrationService" in code, svc and config file together.
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public CustomerRegistrationService()
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


        public long SaveCrmCustomerMst(List<CRM_CUSTOMER_MST> crmCustomerMstList)
        {
            CrmCustomerMstManager crmCustomerMstMgr;
            long? retVal = 0;
            long? maxTaxItmPk;
            try
            {
                crmCustomerMstMgr = new CrmCustomerMstManager(currentContext);
                retVal = crmCustomerMstMgr.SaveCrmCustomerMst(crmCustomerMstList);
                if (retVal > 0)
                {
                    currentContext.SaveChanges();
                    var orphanTaxItms = currentContext.CRM_CUST_TAX_DTL.Where(x => x.CMT_CUST_ITEM == null && x.CMT_CUSTOMER == null);
                    orphanTaxItms.ToList().ForEach(dtl =>
                    {
                        currentContext.DeleteObject(dtl);
                    });
                    currentContext.SaveChanges();
                }

                return retVal.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                SqlException sqlException = ex.InnerException as SqlException;
                if (sqlException.Number == 547)
                {
                    return -1;
                }
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                crmCustomerMstMgr = null;
                retVal = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crmCustomerMstList"></param>
        /// <param name="custItem"></param>
        /// <param name="IsSave">true for Save, false for delete</param>
        /// <returns></returns>
        public long SaveCrmCustomerMst(List<CRM_CUSTOMER_MST> crmCustomerMstList, CustomerInvItem custItem, bool IsSave = true)
        {
            CrmCustomerMstManager crmCustomerMstMgr;
            long? retVal = 0;
            long? maxTaxItmPk;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    crmCustomerMstMgr = new CrmCustomerMstManager(currentContext);
                    retVal = crmCustomerMstMgr.SaveCrmCustomerMst(crmCustomerMstList);
                    if (retVal > 0)
                    {
                        currentContext.SaveChanges();
                        var orphanTaxItms = currentContext.CRM_CUST_TAX_DTL.Where(x => x.CMT_CUST_ITEM == null && x.CMT_CUSTOMER == null);
                        orphanTaxItms.ToList().ForEach(dtl =>
                        {
                            currentContext.DeleteObject(dtl);
                        });
                        currentContext.SaveChanges();
                        if (IsSave)
                        {
                            List<ERPData.SPINV_ITEM_CUST_ITEM_SAVE_Result> result = currentContext.SPINV_ITEM_CUST_ITEM_SAVE(custItem.CIM_PK, custItem.CIM_BRAND_CODE, custItem.CIM_BRAND_NAME, custItem.CIM_CUSTOMER, custItem.ACTIVE, custItem.USER_PK, custItem.BIZUNIT).ToList();
                            if (result != null && result.Count > 0)
                            {
                                if (result[0].RET_VAL == -7)
                                    throw new DuplicateNameException();
                                else if (result[0].RET_VAL < 0)
                                    throw new Exception();

                            }
                        }
                    }
                    scope.Complete();
                }

                return retVal.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                SqlException sqlException = ex.InnerException as SqlException;
                if (sqlException.Number == 547)
                {
                    return -1;
                }
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (DuplicateNameException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                crmCustomerMstMgr = null;
                retVal = 0;
            }
        }


        public long DeleteSaveCrmCustomerMst(List<CRM_CUSTOMER_MST> crmCustomerMstList, CustomerInvItem custItem)
        {
            CrmCustomerMstManager crmCustomerMstMgr;
            long? retVal = 0;
            long? maxTaxItmPk;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    crmCustomerMstMgr = new CrmCustomerMstManager(currentContext);
                    List<ERPData.SPINV_ITEM_CUST_ITEM_MAP_DELETE_Result> result = currentContext.SPINV_ITEM_CUST_ITEM_MAP_DELETE(custItem.CIM_PK, custItem.CIM_BRAND_CODE, custItem.CIM_BRAND_NAME, custItem.CIM_CUSTOMER).ToList();
                    if (result != null && result.Count > 0)
                    {
                        if (result[0].RET_VAL < 0)
                            throw new Exception();

                    }
                    retVal = crmCustomerMstMgr.SaveCrmCustomerMst(crmCustomerMstList);
                    if (retVal > 0)
                    {
                        currentContext.SaveChanges();
                        var orphanTaxItms = currentContext.CRM_CUST_TAX_DTL.Where(x => x.CMT_CUST_ITEM == null && x.CMT_CUSTOMER == null);
                        orphanTaxItms.ToList().ForEach(dtl =>
                        {
                            currentContext.DeleteObject(dtl);
                        });
                        currentContext.SaveChanges();
                    }
                    scope.Complete();
                }

                return retVal.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                SqlException sqlException = ex.InnerException as SqlException;
                if (sqlException.Number == 547)
                {
                    return -1;
                }
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                crmCustomerMstMgr = null;
                retVal = 0;
            }
        }

        /// <summary>
        /// Get Customer Master List
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <returns></returns>
        public List<CRM_CUSTOMER_MST> GetCrmCustomerMst(CRM_CUSTOMER_MST crmCustomerMstObj, ServiceUtility serviceUtilityObj)
        {
            CrmCustomerMstManager crmCustomerMstManagerMgr;
            try
            {
                crmCustomerMstManagerMgr = new CrmCustomerMstManager(currentContext);
                return crmCustomerMstManagerMgr.GetCrmCustomerMst(crmCustomerMstObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                crmCustomerMstManagerMgr = null;
            }
        }

        /// <summary>
        /// Change Object State
        /// </summary>
        /// <param name="entityObj"></param>
        /// <param name="entityState"></param>
        public void ChangeObjectState(Object entityObj, EntityState entityState)
        {
            CrmCustomerMstManager crmCustomerMstManagerMgr;
            try
            {
                crmCustomerMstManagerMgr = new CrmCustomerMstManager(currentContext);
                crmCustomerMstManagerMgr.ChangeObjectState(entityObj, entityState);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                crmCustomerMstManagerMgr = null;
            }
        }

        public List<ADM_FORM_TAB_CONTROL_DTL> FormTabControlDtl(int controlPK)
        {
            CrmCustomerMstManager crmCustomerMstManagerMgr;
            try
            {
                crmCustomerMstManagerMgr = new CrmCustomerMstManager(currentContext);
                return crmCustomerMstManagerMgr.FormTabControlDtl(controlPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                crmCustomerMstManagerMgr = null;
            }
        }

        public List<INV_ITEM_SPEC_DTL> SearchProducts(INV_ITEM_SPEC_DTL invItemSpecDtlObj, int sbuPK = 0)
        {

            InvItemSpecDtlManager invItemSpecDtlManagerObj;
            try
            {
                invItemSpecDtlManagerObj = new InvItemSpecDtlManager(currentContext);
                return invItemSpecDtlManagerObj.GetInvItemSpecDtl(invItemSpecDtlObj, sbuPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemSpecDtlManagerObj = null;
            }

        }
        #endregion




    }
}
