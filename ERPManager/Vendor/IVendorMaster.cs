using System.Collections.Generic;
using ERPData;

namespace ERPManager
{
   public interface  IVendorMasterManager
    {
       List<PUR_VENDOR_MST> GetVendorListAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj);
       List<PUR_VENDOR_MST> GetVendorNameCodeAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj);
       List<PUR_VENDOR_MST> GetServiceVendorListAutoCompleteList(PUR_VENDOR_MST purVendorObj, ServiceUtility utilityObj);
    }
}
