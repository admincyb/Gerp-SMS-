using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;

namespace ERPManager.Finance
{
    public interface IFinCrDrNoteTaxHdrManager
    {
        long? SaveCrDrNoteTaxHdr(List<FIN_CRDR_NOTE_TAX_HDR> finCrDrNoteTaxHdr, long? headerPK);
    }
}
