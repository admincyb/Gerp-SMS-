using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
namespace ERPManager
{
    interface ISalDespatchHdrManager
    {
        List<SAL_DESPATCH_HDR > GetDespatchHdr(SAL_ORDER_HDR SaleorderHdrObj, ServiceUtility utilityObj = null);
    }
}
