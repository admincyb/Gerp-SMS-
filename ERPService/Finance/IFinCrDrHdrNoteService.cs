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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFinCrDrHdrNoteService" in both code and config file together.
    [ServiceContract]
    public interface IFinCrDrHdrNoteService
    {
        [OperationContract]
        List<FIN_CRDR_NOTE_HDR> GetCrDrNoteVndHdr(FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj, ServiceUtility serviceUtilityObj, int Type, string invoiceNo, int? Status = null);
        [OperationContract]
        List<FIN_CRDR_NOTE_HDR> GetCrDrNoteHdr(long finCrDrNotePK);
        [OperationContract]
        long? SaveCrDrNoteHdr(List<FIN_CRDR_NOTE_HDR> finCrDrNoteHdrList, bool isWkfSave);
        [OperationContract]
        List<FIN_CRDR_NOTE_HDR> GetCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj);
        [OperationContract]
        List<FIN_CRDR_NOTE_MPG> GetCrDrTrxMpg(long CrDrPK);
        [OperationContract]
        long UpdateCrDrHdrJounalizeFlag(int CrDrPK, bool JounalizeFlag);
        [OperationContract]
        long DeleteCrDrNoteHdr(long crdrPK, int CrDrType);
        [OperationContract]
        List<FIN_CRDR_NOTE_HDR> GetSalCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj);
        [OperationContract]
        List<FIN_CRDR_NOTE_HDR> GetPurCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj);
    }
}
