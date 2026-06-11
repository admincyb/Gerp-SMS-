using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;
using System.Reflection;
using System.Data.Objects;
using System.Collections;
using System.ComponentModel;
using BusinessObject.CommonManagement;

namespace ERPManager
{
    public class CrmCustomerMstManager : ICrmCustomerMstManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion
        #region Manager Methods
        /// <summary>
        /// Payment Header Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public CrmCustomerMstManager(ERPEntities currentEntity)
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
        /// Save CustomerMaster Details
        /// </summary>
        /// <param name="crmCustomerMstList"></param>
        /// <returns></returns>
        public long? SaveCrmCustomerMst(List<CRM_CUSTOMER_MST> crmCustomerMstList)
        {
            int retval;
            int? maxCountryPK;
            int? maxCusAddrPK;
            long? maxTaxItmPk = 0;
            try
            {
                retval = 0;
                foreach (CRM_CUSTOMER_MST crmCustomerMstObj in crmCustomerMstList)
                {
                    if (crmCustomerMstObj.CUS_PK == 0)
                    {
                        maxCountryPK = currentEntity.CRM_CUSTOMER_MST.Max(v => (int?)v.CUS_PK);
                        maxCusAddrPK = currentEntity.CRM_CUST_ADDRESS.Max(x => (int?)x.CAD_PK);
                        retval = Convert.ToInt16(((maxCountryPK.HasValue) ? maxCountryPK.Value + 1 : 1));
                        crmCustomerMstObj.CUS_PK = retval;
                        crmCustomerMstObj.CUS_CRTD_DT = DateTime.Now;
                        byte active = ERP.Utilities.CommonConstants.ACTIVE == "1" ? 1:0;
                        crmCustomerMstObj.CRM_CUST_ADDRESS.Add(new CRM_CUST_ADDRESS
                        {
                            CAD_PK = (maxCusAddrPK + 1) ?? 1,
                            CAD_ACTIVE = crmCustomerMstObj.CUS_ACTIVE,
                            CAD_ADDRESS = crmCustomerMstObj.CUS_BUILDING,
                            CAD_CITY = crmCustomerMstObj.CUS_STREET,
                            CAD_COUNTRY = crmCustomerMstObj.CUS_COUNTRY,
                            CAD_EMAIL = crmCustomerMstObj.CUS_EMAIL,
                            CAD_FAX = crmCustomerMstObj.CUS_FAX,
                            CAD_MOBILE = crmCustomerMstObj.CUS_MOBILE,
                            CAD_MOD_BY = crmCustomerMstObj.CUS_MOD_BY,
                            CAD_MOD_DT = crmCustomerMstObj.CUS_MOD_DT,
                            CAD_NAME = crmCustomerMstObj.CUS_NAME,
                            CAD_PHONE = crmCustomerMstObj.CUS_PHONE,
                            CAD_STATE = crmCustomerMstObj.CUS_STATE,
                            CAD_STATE_OTHER = crmCustomerMstObj.CUS_STATE_OTHER,
                            CAD_TAX_NO = crmCustomerMstObj.CUS_TAX_NO,
                            CAD_TYPE = currentEntity.ADM_CONFIG_MST.SingleOrDefault(x => 
                                        x.CFG_TYPE == ERP.Utilities.CommonConstants.CUS_ADDRESS_TYPE 
                                        && x.CFG_VALUE == (int) CustomerAddressType.HeadOfficeAddress
                                        && x.CFG_ACTIVE == active
                                        ).CFG_PK,
                            CAD_ZIP = crmCustomerMstObj.CUS_ZIP
                        });
                        currentEntity.CRM_CUSTOMER_MST.AddObject(crmCustomerMstObj);
                    }
                    else
                    {
                        if (crmCustomerMstObj.EntityState == EntityState.Detached)
                            this.currentEntity.CRM_CUSTOMER_MST.Attach(crmCustomerMstObj);

                        if (crmCustomerMstObj.EntityState != EntityState.Deleted)
                        {
                            if (crmCustomerMstObj.CRM_CUST_TAX_DTL.Any())
                            {
                                crmCustomerMstObj.CRM_CUST_TAX_DTL.ToList().ForEach(dtl =>
                                {
                                    if (dtl.CMT_PK == 0)
                                    {
                                        if (maxTaxItmPk == 0) maxTaxItmPk = currentEntity.CRM_CUST_TAX_DTL.Max(x => (long?)x.CMT_PK);
                                        else maxTaxItmPk++;
                                        maxTaxItmPk = ((maxTaxItmPk + 1) ?? 1);
                                        dtl.CMT_PK = maxTaxItmPk.Value;
                                    }
                                    else
                                    {
                                        this.currentEntity.ObjectStateManager.ChangeObjectState(dtl, EntityState.Modified);
                                    }
                                });
                            }
                            else
                            {
                                foreach (var itemMap in crmCustomerMstObj.CRM_CUST_ITEM_MAP.ToList())
                                {
                                    itemMap.CRM_CUST_TAX_DTL.ToList().ForEach(dtl =>
                                    {
                                        if (dtl.CMT_PK == 0)
                                        {
                                            if (maxTaxItmPk == 0) maxTaxItmPk = currentEntity.CRM_CUST_TAX_DTL.Max(x => (long?)x.CMT_PK);
                                            else maxTaxItmPk++;
                                            maxTaxItmPk = ((maxTaxItmPk + 1) ?? 1);
                                            dtl.CMT_PK = maxTaxItmPk.Value;
                                        }
                                        else
                                        {
                                            this.currentEntity.ObjectStateManager.ChangeObjectState(dtl, EntityState.Modified);
                                        }
                                    });
                                }
                            }
                            this.currentEntity.ObjectStateManager.ChangeObjectState(crmCustomerMstObj, EntityState.Modified);
                        }
                        retval = crmCustomerMstObj.CUS_PK;
                    }
                }
                return retval;
            }
            //Handler for concurrency exception
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Exception handler for Delete - Delete Concurrency
            catch (ArgumentNullException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Get Customer Master Details
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <returns></returns>
        public List<CRM_CUSTOMER_MST> GetCrmCustomerMst(CRM_CUSTOMER_MST crmCustomerMstObj, ServiceUtility serviceUtilityObj)
        {
            List<CRM_CUSTOMER_MST> OBJ_CRM_CUSTOMER_MST_List = null; ;
            IQueryable<CRM_CUSTOMER_MST> OBJ_CRM_CUSTOMER_MST_qry;
            try
            {

                OBJ_CRM_CUSTOMER_MST_qry = (from cst in this.currentEntity.CRM_CUSTOMER_MST
                                            where cst.CUS_ACTIVE == crmCustomerMstObj.CUS_ACTIVE
                                              && (serviceUtilityObj.IsSBUSpecific==true ? cst.CUS_BIZUNIT== crmCustomerMstObj.CUS_BIZUNIT : true)
                                              && cst.CUS_PK == (crmCustomerMstObj.CUS_PK > 0 ? crmCustomerMstObj.CUS_PK : cst.CUS_PK)
                                            select cst);

                //Set page size one if not given
                serviceUtilityObj.PageSize = serviceUtilityObj.PageSize == 0 ? 1 : serviceUtilityObj.PageSize;
                serviceUtilityObj.TotalRecords = OBJ_CRM_CUSTOMER_MST_qry.Count();

                // Apply Paging And Sorting for grid Purpose
                OBJ_CRM_CUSTOMER_MST_List = OBJ_CRM_CUSTOMER_MST_qry.SortRecords<CRM_CUSTOMER_MST>(serviceUtilityObj).ToList();

                return OBJ_CRM_CUSTOMER_MST_List;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                //Disposing objects

            }
        }

        /// <summary>
        /// Change Object State
        /// </summary>
        /// <param name="entityObj"></param>
        /// <param name="entityState"></param>
        public void ChangeObjectState(object entityObj, EntityState entityState)
        {
            try
            {
                this.currentEntity.ObjectStateManager.ChangeObjectState(entityObj, entityState);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                //Disposing objects

            }
        }

        public List<ADM_FORM_TAB_CONTROL_DTL> FormTabControlDtl(int controlPK)
        {
            List<ADM_FORM_TAB_CONTROL_DTL> OBJ_ADM_FORM_TAB_CONTROL_DTL_List = null;
            try
            {
                OBJ_ADM_FORM_TAB_CONTROL_DTL_List = (from cst in this.currentEntity.ADM_FORM_TAB_CONTROL_DTL
                                                     where cst.ACD_CONTROL_HDR == controlPK && cst.ACD_ACTIVE == 1
                                                     select cst).OrderBy(adm => adm.ACD_SEQUENCE).ToList();
                return OBJ_ADM_FORM_TAB_CONTROL_DTL_List;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                //Disposing objects

            }
        }
        #endregion
    }
}
