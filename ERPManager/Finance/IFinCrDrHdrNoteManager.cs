using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;
namespace ERPManager
{
    public interface IFinCrDrHdrNoteManager
    {
        List<FIN_CRDR_NOTE_HDR> GetCrDrNoteVndHdr(FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj, ServiceUtility serviceUtilityObj, int Type, string invoiceNo, int? Status = null);
        List<FIN_CRDR_NOTE_HDR> GetCrDrNoteCusHdr(FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj, ServiceUtility serviceUtilityObj);
        List<FIN_CRDR_NOTE_HDR> GetCrDrNoteHdr(long finCrDrNotePK);
        long? SaveCrDrNoteHdr(List<FIN_CRDR_NOTE_HDR> finCrDrNoteHdrList, bool isWkfSave);
        List<FIN_CRDR_NOTE_HDR> GetCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj);
        long UpdateCrDrHdrJounalizeFlag(int CrDrPK, bool JounalizeFlag);
        long DeleteCrDrNoteHdr(long crdrPK, int CrDrType);
        List<FIN_CRDR_NOTE_HDR> GetSalCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj);
        List<FIN_CRDR_NOTE_HDR> GetPurCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj);
    }
}
