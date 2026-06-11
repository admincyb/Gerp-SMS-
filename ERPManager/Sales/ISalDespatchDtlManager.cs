using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
namespace ERPManager
{
    interface ISalDespatchDtlManager
    {
        List<SAL_DESPATCH_DTL> GetDespatchDtl(SAL_DESPATCH_DTL DespatchDtlObj, ServiceUtility utilityObj = null);
    }
}
