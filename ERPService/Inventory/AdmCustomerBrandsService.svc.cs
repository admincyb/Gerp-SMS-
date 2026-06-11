using System;
using System.Collections.Generic;
using ERPData;
using System.Diagnostics;
using ERPManager;
using System.Data;
using ERPManager.Inventory;
using BusinessObject.Inventory;

namespace ERPService.Inventory
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AdmProductionMstService" in code, svc and config file together.
    public class AdmCustomerBrandsService : IAdmCustomerBrandsService, IAdmCustomerBrandsManager
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// Constructor for AdmPackingSpecMstService Service
        /// </summary>
        public AdmCustomerBrandsService()
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

        public List<CustomerBrands> GetCustomerBrands(CustomerBrands AdmCustomerBrandsMstObj, ServiceUtility utilityObj)
        {
            AdmCustomerBrandsManager customerBrandsMgr;
            try
            {
                customerBrandsMgr = new AdmCustomerBrandsManager(currentContext);
                return customerBrandsMgr.GetCustomerBrands(AdmCustomerBrandsMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                customerBrandsMgr = null;
            }
        }
        public List<CRM_CUSTOMER_MST> GetCustomers()
        {
            AdmCustomerBrandsManager customerBrandsMgr;
            try
            {
                customerBrandsMgr = new AdmCustomerBrandsManager(currentContext);
                return customerBrandsMgr.GetCustomers();
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                customerBrandsMgr = null;
            }
        }

        public List<INV_ITEM_MST> GetProducts(int itemPK)
        {
            AdmCustomerBrandsManager customerBrandsMgr;
            try
            {
                customerBrandsMgr = new AdmCustomerBrandsManager(currentContext);
                return customerBrandsMgr.GetProducts(itemPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                customerBrandsMgr = null;
            }
        }

        public int SaveCustomerBrands(List<CRM_CUST_ITEM_MAP> admCustomerBrandsMstList)
        {
            AdmCustomerBrandsManager customerBrandsMgr;
            int? accountMstPK;
            try
            {
                customerBrandsMgr = new AdmCustomerBrandsManager(currentContext);
                accountMstPK = customerBrandsMgr.SaveCustomerBrands(admCustomerBrandsMstList);
                currentContext.SaveChanges();
                return accountMstPK.Value;
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
                customerBrandsMgr = null;
                accountMstPK = null;
            }
        }

        public int DeleteCustomerBrands(List<CRM_CUST_ITEM_MAP> admCustomerBrandsMstList)
        {
            AdmCustomerBrandsManager customerBrandsMgr;
            try
            {
                customerBrandsMgr = new AdmCustomerBrandsManager(currentContext);
                int result = customerBrandsMgr.DeleteCustomerBrands(admCustomerBrandsMstList);
                currentContext.SaveChanges();
                return result;
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
                customerBrandsMgr = null;
            }
        }
        #endregion
    }
}
