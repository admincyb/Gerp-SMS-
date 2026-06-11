using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using System.Diagnostics;
using ERPManager;
using System.Data;
using ERPManager.Finance;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FinCrDrHdrNoteService" in code, svc and config file together.
    public class FinCrDrHdrNoteService : IFinCrDrHdrNoteService,IFinCrDrHdrNoteManager
    {
        #region Private Variables
        ERPEntities  currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public FinCrDrHdrNoteService()
        {
            try
            {
                currentContext = new ERPEntities();
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        /// <summary>
        /// Get CrDr Note Header List
        /// </summary>
        /// <param name="finCrDrNoteHdrObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_CRDR_NOTE_HDR> GetCrDrNoteVndHdr(FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj, ServiceUtility serviceUtilityObj, int Type, string invoiceNo, int? Status = null)
        {
            FinCrDrHdrNoteManager objFinCrDrHdrNoteManager;
            try
            {
                objFinCrDrHdrNoteManager = new FinCrDrHdrNoteManager(currentContext);
                return objFinCrDrHdrNoteManager.GetCrDrNoteVndHdr(finCrDrNoteHdrObj, serviceUtilityObj, Type,invoiceNo,Status);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteManager = null;
            }
        }
        public List<FIN_CRDR_NOTE_HDR> GetCrDrNoteHdr(long finCrDrNotePK)
        {
            FinCrDrHdrNoteManager objFinCrDrHdrNoteManager;
            try
            {
                objFinCrDrHdrNoteManager = new FinCrDrHdrNoteManager(currentContext);
                return objFinCrDrHdrNoteManager.GetCrDrNoteHdr(finCrDrNotePK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteManager = null;
            }
        }

        public List<FIN_CRDR_NOTE_HDR> GetCustomerCrDrNoteHdr(long finCrDrNotePK)
        {
            FinCrDrHdrNoteManager objFinCrDrHdrNoteManager;
            try
            {
                objFinCrDrHdrNoteManager = new FinCrDrHdrNoteManager(currentContext);
                return objFinCrDrHdrNoteManager.GetCustomerCrDrNoteHdr(finCrDrNotePK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteManager = null;
            }
        }

        public List<FIN_CRDR_NOTE_HDR> GetCrDrNoteCusHdr(FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj, ServiceUtility serviceUtilityObj)
        {
            FinCrDrHdrNoteManager objFinCrDrHdrNoteManager;
            try
            {
                objFinCrDrHdrNoteManager = new FinCrDrHdrNoteManager(currentContext);
                return objFinCrDrHdrNoteManager.GetCrDrNoteCusHdr(finCrDrNoteHdrObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteManager = null;
            }
        }

   
        public long? SaveCrDrNoteHdr(List<FIN_CRDR_NOTE_HDR> finCrDrNoteHdrList,bool isWkfSave)
        {
            FinCrDrHdrNoteManager objFinCrDrHdrNoteManager;
            long? finCrDrHdrPK;
            try
            {
                objFinCrDrHdrNoteManager = new FinCrDrHdrNoteManager(currentContext);
                finCrDrHdrPK = objFinCrDrHdrNoteManager.SaveCrDrNoteHdr(finCrDrNoteHdrList, isWkfSave);
                currentContext.SaveChanges();
                return finCrDrHdrPK.Value;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteManager = null;
            }
        }

        /// <summary>
        /// Delete Cr/Dr Header and Details
        /// </summary>
        /// <param name="finSalesInvoiceHdrList"></param>
        /// <returns></returns>
        public long DeleteCrDrNoteHdr(long crdrPK, int CrDrType)
        {
            FinCrDrHdrNoteManager objFinCrDrHdrNoteManager;
            long? finCrDrHdrPK;
            try
            {
                objFinCrDrHdrNoteManager = new FinCrDrHdrNoteManager(currentContext);
                finCrDrHdrPK = objFinCrDrHdrNoteManager.DeleteCrDrNoteHdr(crdrPK,CrDrType);
                currentContext.SaveChanges();
                return finCrDrHdrPK.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteManager = null;
            }
        }

        public List<FIN_CRDR_NOTE_HDR> GetCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj)
        {
            FinCrDrHdrNoteManager objFinCrDrHdrNoteManager;
            try
            {
                objFinCrDrHdrNoteManager = new FinCrDrHdrNoteManager(currentContext);
                return objFinCrDrHdrNoteManager.GetCrDrNoteAutoCompleteList(finCrDrNoteObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteManager = null;
            }
        }

        public List<FIN_CRDR_NOTE_HDR> GetSalCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj)
        {
            FinCrDrHdrNoteManager objFinCrDrHdrNoteManager;
            try
            {
                objFinCrDrHdrNoteManager = new FinCrDrHdrNoteManager(currentContext);
                return objFinCrDrHdrNoteManager.GetSalCrDrNoteAutoCompleteList(finCrDrNoteObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteManager = null;
            }
        }

        public List<FIN_CRDR_NOTE_HDR> GetPurCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj)
        {
            FinCrDrHdrNoteManager objFinCrDrHdrNoteManager;
            try
            {
                objFinCrDrHdrNoteManager = new FinCrDrHdrNoteManager(currentContext);
                return objFinCrDrHdrNoteManager.GetPurCrDrNoteAutoCompleteList(finCrDrNoteObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinCrDrHdrNoteManager = null;
            }
        }

        public List<FIN_CRDR_NOTE_MPG> GetCrDrTrxMpg(long CrDrPK)
        {
            FinCrDrHdrNoteMpgManager FinCrDrHdrNoteMpgManagerObj;
            try
            {
                FinCrDrHdrNoteMpgManagerObj = new FinCrDrHdrNoteMpgManager(currentContext);
                return FinCrDrHdrNoteMpgManagerObj.GetFinCrDrNoteMpg(CrDrPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                FinCrDrHdrNoteMpgManagerObj = null;
            }
        }


        /// <summary>
        /// Update InvoiceHdr Jounalize Flag
        /// </summary>
        /// <param name="finInvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateCrDrHdrJounalizeFlag(int CrDrPK, bool JounalizeFlag)
        {
            FinCrDrHdrNoteManager FinCrDrHdrNoteManagerObj;
            long? finInvoiceHdrPK;
            try
            {
                FinCrDrHdrNoteManagerObj = new FinCrDrHdrNoteManager(currentContext);
                finInvoiceHdrPK = FinCrDrHdrNoteManagerObj.UpdateCrDrHdrJounalizeFlag(CrDrPK, JounalizeFlag);
                currentContext.SaveChanges();
                return finInvoiceHdrPK.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                FinCrDrHdrNoteManagerObj = null;
                finInvoiceHdrPK = null;
            }
        }
        #endregion

        public List<FIN_CRDR_NOTE_DTL> GetCrDrSplitList(FIN_CRDR_NOTE_DTL finReceiptCusSoMpgObj)
        {
            FinCrDrNoteDtlManager finPaymentVndPoMpgManagerObj;
            try
            {
                finPaymentVndPoMpgManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finPaymentVndPoMpgManagerObj.GetCrDrSplitList(finReceiptCusSoMpgObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finPaymentVndPoMpgManagerObj = null;
            }
        }

        public List<FIN_INVOICE_CUS_DTL> GetInvoiceTrxMpg(FIN_INVOICE_CUS_DTL FinInvoiceCusTrxMpgObj)
        {
            FinCrDrNoteDtlManager finInvoiceVndTrxMpgManagerObj;
            try
            {
                finInvoiceVndTrxMpgManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finInvoiceVndTrxMpgManagerObj.GetInvoiceTrxMpg(FinInvoiceCusTrxMpgObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceVndTrxMpgManagerObj = null;
            }
        }

        public List<FIN_INVOICE_CUS_HDR> GetInvoiceCusHdrByPK(FIN_INVOICE_CUS_HDR finInvoiceVndHdrObjForPaymentSplit)
        {
            FinCrDrNoteDtlManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetInvoiceCusHdrByPK(finInvoiceVndHdrObjForPaymentSplit);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceVndHdrManagerObj = null;
            }
        }



        public List<FIN_CRDR_NOTE_TAX_DTL> GetCrDrNoteTaxList(long CrDrPK)
        {
            FinCrDrNoteTaxManager FinCrDrHdrNoteTaxManagerObj;
            try
            {
                FinCrDrHdrNoteTaxManagerObj = new FinCrDrNoteTaxManager(currentContext);
                return FinCrDrHdrNoteTaxManagerObj.GetCrDrNoteTaxList(CrDrPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                FinCrDrHdrNoteTaxManagerObj = null;
            }
        }

        public List<FIN_INVOICE_VND_DTL> GetInvoiceVndTrxMpg(FIN_INVOICE_VND_DTL FinCrDrCusTrxMpgObj)
        {
            FinCrDrNoteDtlManager finInvoiceVndTrxMpgManagerObj;
            try
            {
                finInvoiceVndTrxMpgManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finInvoiceVndTrxMpgManagerObj.GetInvoiceVndTrxMpg(FinCrDrCusTrxMpgObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceVndTrxMpgManagerObj = null;
            }
        }

        public List<FIN_INVOICE_VND_HDR> GetInvoiceVndHdrByPK(FIN_INVOICE_VND_HDR finCrDrVndHdrObjForPaymentSplit)
        {
            FinCrDrNoteDtlManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetInvoiceVndHdrByPK(finCrDrVndHdrObjForPaymentSplit);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceVndHdrManagerObj = null;
            }
        }

        public bool IsDrCrUsedInOtherTrnsactions(long CrdrPk)
        {
            FinCrDrNoteDtlManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finInvoiceVndHdrManagerObj.IsDrCrUsedInOtherTrnsactions(CrdrPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceVndHdrManagerObj = null;
            }
        }

        public bool IsRefnoExist(FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj)
        {
            FinCrDrNoteDtlManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finInvoiceVndHdrManagerObj.IsRefnoExist(finCrDrNoteHdrObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceVndHdrManagerObj = null;
            }
        }

        public List<FIN_PAYMENT_VND_ALCN_DTL> GetDnsAllocationInPayment(long CrDrPk)
        {
            FinCrDrNoteDtlManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetDnsAllocationInPayment(CrDrPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceVndHdrManagerObj = null;
            }
        }

        public List<FIN_RECEIPT_CUS_ALCN_DTL> GetCnsAllocationInReceipt(long CrDrPk)
        {
            FinCrDrNoteDtlManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetCnsAllocationInReceipt(CrDrPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceVndHdrManagerObj = null;
            }
        }

        public decimal GetCrDrAllocatedAmount(long CrDrPk, int Mode)
        {
            FinCrDrNoteDtlManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetCrDrAllocatedAmount(CrDrPk, Mode);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceVndHdrManagerObj = null;
            }
        }

        public List<CrdrAllocations> GetBalanceAmountSplit(long CrDrPk)
        {
            FinCrDrNoteDtlManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetBalanceAmountSplit(CrDrPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceVndHdrManagerObj = null;
            }
        }

        public List<CrdrAllocations> GetBalanceAmountSplitSales(long CrDrPk)
        {
            FinCrDrNoteDtlManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetBalanceAmountSplitSales(CrDrPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceVndHdrManagerObj = null;
            }
        }

        public List<FIN_RECEIPT_CUS_CRDR_MPG> CheckDRCRforCancel(FIN_RECEIPT_CUS_CRDR_MPG finReceiptCusCrDrMpgObj)
        {
            FinCrDrNoteDtlManager finCRDRMpgManagerObj;
            try
            {
                finCRDRMpgManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finCRDRMpgManagerObj.CheckDRCRforCancel(finReceiptCusCrDrMpgObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finCRDRMpgManagerObj = null;
            }
        }

        public List<FIN_PAYMENT_VND_CRDR_MPG> CheckPaymentDRCRforCancel(FIN_PAYMENT_VND_CRDR_MPG finPaymentCusCrDrMpgObj)
        {
            FinCrDrNoteDtlManager finCRDRMpgManagerObj;
            try
            {
                finCRDRMpgManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finCRDRMpgManagerObj.CheckPaymentDRCRforCancel(finPaymentCusCrDrMpgObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finCRDRMpgManagerObj = null;
            }
        }


        public decimal GetTotalCNAmount(long InvoicePk, long CrDrPk)
        {
            FinCrDrNoteDtlManager finCRDRMpgManagerObj;
            try
            {
                finCRDRMpgManagerObj = new FinCrDrNoteDtlManager(currentContext);
                return finCRDRMpgManagerObj.GetTotalCNAmount(InvoicePk, CrDrPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finCRDRMpgManagerObj = null;
            }
        }
    }
}
