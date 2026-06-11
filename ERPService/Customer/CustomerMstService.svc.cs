using System;
using System.Collections.Generic;
using ERPData;
using ERPManager;
using System.Data;
using System.Diagnostics;


namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CustomerMstService" in code, svc and config file together.
    public class CustomerMstService : ICustomerMstService, ICustomerManager
    {
          #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public CustomerMstService()
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
        /// Gets list of Customer Master after filtering,sorting for filling auto complete list
        /// </summary>
        /// <param name="salCustomerObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<CRM_CUSTOMER_MST> GetCustomerListAutoCompleteList(CRM_CUSTOMER_MST salCustomerObj, ServiceUtility utilityObj)
        {
            CustomerManager customerManager;
            try
            {
                customerManager = new CustomerManager(currentContext);
                return customerManager.GetCustomerListAutoCompleteList(salCustomerObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                customerManager = null;
            }
        }

        /// <summary>
        /// Gets list of Customer and vendor auto complete list
        /// </summary>
        /// <param name="salCustomerObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<BusinessObject.CommonManagement.AutoCompleteBO> GetCustomerVendorAutoCompleteList(CRM_CUSTOMER_MST salCustomerObj, ServiceUtility utilityObj)
        {
            CustomerManager customerManager;
            try
            {
                customerManager = new CustomerManager(currentContext);
                return customerManager.GetCustomerVendorAutoCompleteList(salCustomerObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                customerManager = null;
            }
        }

        /// <summary>
       
        /// </summary>
        /// <param name="camVendorMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Vendor Master</returns>
        public List<PUR_VENDOR_MST> GetVendorListAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj)
        {
            VendorMasterManager vendorMasterMgr;
            try
            {
                vendorMasterMgr = new VendorMasterManager(currentContext);
                return vendorMasterMgr.GetVendorListAutoCompleteList(purVendorObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                vendorMasterMgr = null;
            }
        }
        #endregion



        
    }
}
