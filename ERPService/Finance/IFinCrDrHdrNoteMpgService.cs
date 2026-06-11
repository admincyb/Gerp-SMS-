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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFinCrDrHdrNoteMpgService" in both code and config file together.
    [ServiceContract]
    public interface IFinCrDrHdrNoteMpgService
    {
        [OperationContract]
        long? SaveFinCrDrNoteMpg(List<FIN_CRDR_NOTE_MPG> finCrDrNoteMpgList, byte CrDrType);
        [OperationContract]
        List<FIN_CRDR_NOTE_MPG> GetFinCrDrNoteMpg(long finCrDrNotePK);
        [OperationContract]
        List<FIN_INVOICE_VND_HDR> GetFinCrDrNoteMpg(List<long> InvoicePK);
    }
}
