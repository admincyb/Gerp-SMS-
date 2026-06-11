using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;
using ERP.Utilities;

namespace ERPManager
{
  
    public interface IFinTrxHdrManager
    {
        #region Private Variables
        long SaveFinTrxHdr(List<FIN_TRX_HDR> finTrxhdrList);
        List<FIN_TRX_HDR> GetfinTxtHdrList(FIN_TRX_HDR finTrxHdrObj, ServiceUtility utilityObj);
        long DeleteFinTrx(string REFTYPE, int REFPK, int FTHPK);
        #endregion
    }
}
