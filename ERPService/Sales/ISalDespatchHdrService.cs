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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISalDespatchHdrService" in both code and config file together.
    [ServiceContract]
    public interface ISalDespatchHdrService
    {
        [OperationContract]
        List<SAL_DESPATCH_HDR> GetDespatchHdr(SAL_ORDER_HDR SaleorderHdrObj, ServiceUtility utilityObj = null);
    }
}
