using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;

namespace ERPService
{
    [ServiceContract]
    public interface ISalDespatchDtlService
    {
       
        [OperationContract]
        List<SAL_DESPATCH_DTL> GetDespatchDtl(SAL_DESPATCH_DTL DespatchDtlObj, ServiceUtility utilityObj = null);
      
    }
}