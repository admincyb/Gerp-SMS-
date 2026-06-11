using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;

namespace ERPManager.Administration
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAdmCountryMstManager" in both code and config file together.
    [ServiceContract]
    public interface IAdmCountryMstManager
    {
        int SaveAdmCountryMst(List<AdmCountryMst> admCountryMstList);
        int DeleteAdmCountryMst(List<AdmCountryMst> admCountryMstList);
        List<AdmCountryMst> GetAdmCountryMst(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj = null);
        ServiceUtility GetAdmCountryMstCount(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj = null);
        AdmCountryMst GetInitilizedAdmCountryMst();
        List<AdmCountryMst> GetAdmCountryMstAutoCompleteList(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj);
    }
}
