using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDynamicPageService" in both code and config file together.
    [ServiceContract]
    public interface IDynamicPageService
    {
        [OperationContract]
        List<SPADM_FORM_TAB_CFG_GET_Result> GetFormTabList(string formCode);
        [OperationContract]
        List<SPADM_FORM_TAB_CONTROL_CFG_GET_Result> GetFormTabControlsList(string formCode, string tabCode,int userPk=0,int sbuPK=1);
        
    }
}
