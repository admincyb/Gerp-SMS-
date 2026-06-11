using System.Collections.Generic;
using ERPData;

namespace ERPManager
{
    public interface IFinCrDrHdrNoteMpgManager
    {
        long? SaveFinCrDrNoteMpg(List<FIN_CRDR_NOTE_MPG> finCrDrNoteMpgList, byte CrDrType);
        long? SaveFinCrDrNoteMpg(List<FIN_CRDR_NOTE_MPG> finCrDrNoteMpgList);
        List<FIN_CRDR_NOTE_MPG> GetFinCrDrNoteMpg(long finCrDrNotePK);
        List<FIN_INVOICE_VND_HDR> GetFinCrDrNoteMpg(List<long> InvoicePK); //Vendor Invoice
        //List<FIN_INVOICE_CUS_HDR> GetFinCrDrNoteMpg(List<long> InvoicePK); //Customer Invoice
    }
}
