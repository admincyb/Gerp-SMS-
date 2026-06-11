using System.Collections.Generic;
using System.ServiceModel;
using ERPData;
using ERPManager;
using System;


namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IVendorMstService" in both code and config file together.
    [ServiceContract]
    public interface IVendorMstService
    {
        #region Vendor Master Functions
        [OperationContract]
        List<PUR_VENDOR_MST> GetVendorListAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj);
        [OperationContract]
        List<PUR_VENDOR_MST> GetVendorNameCodeAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj);
        [OperationContract]
        List<PUR_VENDOR_MST> GetServiceVendorListAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj);
        #endregion
    }
}
