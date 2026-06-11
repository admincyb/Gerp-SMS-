using System;
using System.Collections.Generic;
using ERPData;
using ERPManager;
using System.Data;
using System.Diagnostics;


namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "VendorMstService" in code, svc and config file together.
    public class VendorMstService : IVendorMstService, IVendorMasterManager
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public VendorMstService()
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
        /// Gets list of Vendor Master after filtering,sorting for filling auto complete list
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

        /// <summary>
        /// Gets list of Vendor Master after filtering by vendor name or vendor code for auto complete list
        /// </summary>
        /// <param name="purVendorObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Vendor Master</returns>
        public List<PUR_VENDOR_MST> GetVendorNameCodeAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj)
        {
            VendorMasterManager vendorMasterMgr;
            try
            {
                vendorMasterMgr = new VendorMasterManager(currentContext);
                return vendorMasterMgr.GetVendorNameCodeAutoCompleteList(purVendorObj, utilityObj);
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

        /// <summary>
        /// Gets list of Vendor Master after filtering,sorting for filling auto complete list
        /// </summary>
        /// <param name="camVendorMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Vendor Master</returns>
        public List<PUR_VENDOR_MST> GetServiceVendorListAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj)
        {
            VendorMasterManager vendorMasterMgr;
            try
            {
                vendorMasterMgr = new VendorMasterManager(currentContext);
                return vendorMasterMgr.GetServiceVendorListAutoCompleteList(purVendorObj, utilityObj);
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

        /// <summary>
        /// Gets list of Vendor Master after filtering,sorting for filling auto complete list
        /// </summary>
        /// <param name="camVendorMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Vendor Master</returns>
        public List<PUR_VENDOR_MST> GetVendorRoleListAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj)
        {
            VendorMasterManager vendorMasterMgr;
            try
            {
                vendorMasterMgr = new VendorMasterManager(currentContext);
                return vendorMasterMgr.GetVendorRoleListAutoCompleteList(purVendorObj, utilityObj);
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
