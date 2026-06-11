using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;

namespace ERPService.Administration
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICountryService" in both code and config file together.
    [ServiceContract]
    public interface ICountryService
    {
        [OperationContract]
        int SaveAdmCountryMst(List<AdmCountryMst> admCountryMstList);

        [OperationContract]
        int DeleteAdmCountryMst(List<AdmCountryMst> admCountryMstList);

        [OperationContract]
        List<AdmCountryMst> GetAdmCountryMst(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj = null);

        [OperationContract]
        ServiceUtility GetAdmCountryMstCount(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj = null);

        //[OperationContract]
        //List<AdmCurrencyMst> GetAdmCurrencyMst(AdmCurrencyMst admCurrencyMstObj, ServiceUtility utilityObj = null);

        [OperationContract]
        AdmCountryMst GetInitilizedAdmCountryMst();

        //[OperationContract]
        //AdmCurrencyMst GetInitilizedAdmCurrencyMst();

        [OperationContract]
        List<AdmCountryMst> GetAdmCountryMstAutoCompleteList(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj);
    }
}
