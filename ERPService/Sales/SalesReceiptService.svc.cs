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
using BusinessObject.CommonManagement;
using BusinessObject.SaleOrder;
using ERPManager.Sales;
namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SalesReceiptService" in code, svc and config file together.
    public class SalesReceiptService : ISalesReceiptService
    {
        #region Private Variables
        ERPEntities currentContext;
        private ReceiptAdjnAllocation ReceiptAdjnObj;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public SalesReceiptService()
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
        /// Get Payment Header Details
        /// </summary>
        /// <param name="finPaymentHdrObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_RECEIPT_CUS_HDR> GetReceiptHdr(FIN_RECEIPT_CUS_HDR finReceiptHdrObj, ServiceUtility serviceUtilityObj, int? Status = null, string invNo = null,int? PDCStatus = 0)
        {
            FinReceiptCusHdrManager finReceiptCusHdrManagerObj;
            try
            {
                finReceiptCusHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                return finReceiptCusHdrManagerObj.GetReceiptHdr(finReceiptHdrObj, serviceUtilityObj, Status, invNo, PDCStatus);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finReceiptCusHdrManagerObj = null;
            }
        }

        /// <summary>
        /// Get Payment Header Details
        /// </summary>
        /// <param name="paymentPK"></param>
        /// <returns></returns>
        public List<FIN_RECEIPT_CUS_HDR> GetReceiptHdr(long receiptPK)
        {
            FinReceiptCusHdrManager finReceiptCusHdrManagerObj;
            try
            {
                finReceiptCusHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                return finReceiptCusHdrManagerObj.GetReceiptHdr(receiptPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finReceiptCusHdrManagerObj = null;
            }
        }
        /// <summary>
        /// Save Payment Header and Details
        /// </summary>
        /// <param name="finPaymentHdrList"></param>
        /// <returns></returns>
        public long? SaveReceiptHdr(List<FIN_RECEIPT_CUS_HDR> finReceiptHdrList)
        {
            FinReceiptCusHdrManager finReceiptCusHdrManagerObj;
            long? finPaymentHdrPK;
            try
            {
                finReceiptCusHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                finPaymentHdrPK = finReceiptCusHdrManagerObj.SaveReceiptHdr(finReceiptHdrList);
                currentContext.SaveChanges();
                return finPaymentHdrPK.Value;
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
                finReceiptCusHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }

        /// <summary>
        /// Checks if Instr No already exists
        /// </summary>
        /// <param name="finPaymentHdrList"></param>
        /// <returns></returns>
        public bool? GetReceiptInstrNo(string instrNo, long receiptPK)
        {
            FinReceiptCusHdrManager finReceiptCusHdrManagerObj;
            bool? finPaymentHdrPK;
            try
            {
                finReceiptCusHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                finPaymentHdrPK = finReceiptCusHdrManagerObj.GetReceiptInstrNo(instrNo, receiptPK);
                //currentContext.SaveChanges();
                return finPaymentHdrPK.Value;
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
                finReceiptCusHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }

        /// <summary>
        /// Get Invoice Trx Mpg details;
        /// </summary>
        /// <param name="InvoicePK"></param>
        /// <returns>List of Invoice Trx Mpg</returns>
        public List<FIN_INVOICE_CUS_HDR> GetReceiptTrxMpg(List<long> InvoicePK)
        {
            FinReceiptCusTrxMpgManager finReceiptCusTrxMpgManagerObj;
            try
            {
                finReceiptCusTrxMpgManagerObj = new FinReceiptCusTrxMpgManager(currentContext);
                return finReceiptCusTrxMpgManagerObj.GetReceiptTrxMpg(InvoicePK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finReceiptCusTrxMpgManagerObj = null;
            }
        }
        /// <summary>
        /// Get Invoice Trx Mpg details;
        /// </summary>
        /// <param name="InvoicePK"></param>
        /// <returns>List of Invoice Trx Mpg</returns>
        public bool IsReceiptAdvDeducted(int reciptPK)
        {
            FinReceiptCusTrxMpgManager finReceiptCusTrxMpgManagerObj;
            try
            {
                finReceiptCusTrxMpgManagerObj = new FinReceiptCusTrxMpgManager(currentContext);
                return finReceiptCusTrxMpgManagerObj.IsReceiptAdvDeducted(reciptPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finReceiptCusTrxMpgManagerObj = null;
            }
        }
        public List<FIN_RECEIPT_CUS_HDR> GetReceiptNumberAutoCompleteList(FIN_RECEIPT_CUS_HDR salesReceiptObj, ServiceUtility serviceUtilityObj)
        {
            FinReceiptCusHdrManager finReceiptCusHdrManagerObj;
            try
            {
                finReceiptCusHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                return finReceiptCusHdrManagerObj.GetReceiptNumberAutoCompleteList(salesReceiptObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finReceiptCusHdrManagerObj = null;
            }
        }

        public List<FIN_RECEIPT_CUS_TRX_MPG> GetReceiptTrxMpg(long receiptPK)
        {
            FinReceiptCusTrxMpgManager FinPaymentVndTrxMpgManagerObj;
            try
            {
                FinPaymentVndTrxMpgManagerObj = new FinReceiptCusTrxMpgManager(currentContext);
                return FinPaymentVndTrxMpgManagerObj.GetReceiptTrxMpg(receiptPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                FinPaymentVndTrxMpgManagerObj = null;
            }
        }

        public string GetReceiptNo(string  aptCode, int  astVal, int dept, DateTime date, int user, bool update, int appPK,int? cmpanyPK=null,int? bizunit=null)
        {
            CommonFunctionsManager commonFunctionsManagerObj;
            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetTrxDocNo(aptCode, astVal, dept, date, user, update, appPK, cmpanyPK, bizunit);
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
                commonFunctionsManagerObj = null;
            }
        }
        public double GetConversionFactor(int FromCurrency, int ToCurrency, DateTime TrxDate, int BizUnit)
        {
            CommonFunctionsManager commonFunctionsMgr;
            try
            {
                commonFunctionsMgr = new CommonFunctionsManager(currentContext);
                return commonFunctionsMgr.GetConversionFactor(FromCurrency, ToCurrency, TrxDate, BizUnit);
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
                commonFunctionsMgr = null;
            }
        }
        public long? DeleteSalesReceiptHdr(long CurrPK)
        {
            FinReceiptCusHdrManager finReceiptCusHdrManagerObj;
            long? finReceiptHdrPK;
            try
            {
                finReceiptCusHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                finReceiptHdrPK = finReceiptCusHdrManagerObj.DeleteSalesReceiptHdr(CurrPK);
                currentContext.SaveChanges();
                return finReceiptHdrPK.Value;
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
                finReceiptCusHdrManagerObj = null;
                finReceiptHdrPK = null;
            }
        }
        /// <summary>
        /// Update Payment Hdr Jounalize Flag
        /// </summary>
        /// <param name="finInvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateReceiptHdrJounalizeFlag(int RecPK, bool JounalizeFlag)
        {
            FinReceiptCusHdrManager finReceiptCusHdrManagerObj;
            long? finReceiptHdrPK;
            try
            {
                finReceiptCusHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                finReceiptHdrPK = finReceiptCusHdrManagerObj.UpdateReceiptHdrJounalizeFlag(RecPK, JounalizeFlag);
                currentContext.SaveChanges();
                return finReceiptHdrPK.Value;
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
                finReceiptCusHdrManagerObj = null;
                finReceiptHdrPK = null;
            }
        }

        /// <summary>
        /// Update Payment Hdr Jounalize Flag
        /// </summary>
        /// <param name="finInvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateReceiptHdrPDCFlag(int RecPK, byte pdcFlag)
        {
            FinReceiptCusHdrManager finReceiptCusHdrManagerObj;
            long? finReceiptHdrPK;
            try
            {
                finReceiptCusHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                finReceiptHdrPK = finReceiptCusHdrManagerObj.UpdateReceiptHdrPDCFlag(RecPK, pdcFlag);
                currentContext.SaveChanges();
                return finReceiptHdrPK.Value;
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
                finReceiptCusHdrManagerObj = null;
                finReceiptHdrPK = null;
            }
        }

        /// <summary>
        /// Update Payment Hdr Jounalize Flag
        /// </summary>
        /// <param name="finInvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateReceiptHdrBounceFlag(int RecPK, byte bounceFlag)
        {
            FinReceiptCusHdrManager finReceiptCusHdrManagerObj;
            long? finReceiptHdrPK;
            try
            {
                finReceiptCusHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                finReceiptHdrPK = finReceiptCusHdrManagerObj.UpdateReceiptHdrBounceFlag(RecPK, bounceFlag);
                currentContext.SaveChanges();
                return finReceiptHdrPK.Value;
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
                finReceiptCusHdrManagerObj = null;
                finReceiptHdrPK = null;
            }
        }
        #endregion






        public List<FIN_RECEIPT_CUS_SO_MPG> GetFinReceiptCusSoMpgList(FIN_RECEIPT_CUS_SO_MPG finReceiptCusSoMpgObj)
        {
            FinReceiptCusSoMpgManager finPaymentVndPoMpgManagerObj;
            try
            {
                finPaymentVndPoMpgManagerObj = new FinReceiptCusSoMpgManager(currentContext);
                return finPaymentVndPoMpgManagerObj.GetFinReceiptCusSoMpgList(finReceiptCusSoMpgObj);
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
        public List<FIN_RECEIPT_CUS_ALCN_DTL> GetFinReceiptAlcnList(FIN_RECEIPT_CUS_ALCN_DTL finReceiptAlcnObj)
        {
            FinReceiptCusSoMpgManager finPaymentVndPoMpgManagerObj;
            try
            {
                finPaymentVndPoMpgManagerObj = new FinReceiptCusSoMpgManager(currentContext);
                return finPaymentVndPoMpgManagerObj.GetFinReceiptAlcnList(finReceiptAlcnObj);
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

        public List<FIN_RECEIPT_CUS_ALCN_DTL> GetFinReceiptAlcnListContext(List<FIN_RECEIPT_CUS_ALCN_DTL> finReceiptAlcnList)
        {
            FinReceiptCusSoMpgManager finPaymentVndPoMpgManagerObj;
            try
            {
                finPaymentVndPoMpgManagerObj = new FinReceiptCusSoMpgManager(currentContext);
                return finPaymentVndPoMpgManagerObj.GetFinReceiptAlcnListContext(finReceiptAlcnList);
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


        public List<FIN_INVOICE_CUS_TRX_MPG> GetInvoiceTrxMpg(FIN_INVOICE_CUS_TRX_MPG FinInvoiceCusTrxMpgObj)
        {
            FinInvoiceCusTrxMpgManager finInvoiceVndTrxMpgManagerObj;
            try
            {
                finInvoiceVndTrxMpgManagerObj = new FinInvoiceCusTrxMpgManager(currentContext);
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
        public List<FIN_RECEIPT_CUS_ALCN_DTL> GetReceiptCusAdjn(int cusId)
        {
            FinInvoiceCusTrxMpgManager finInvoiceVndTrxMpgManagerObj;
            try
            {
                finInvoiceVndTrxMpgManagerObj = new FinInvoiceCusTrxMpgManager(currentContext);
                return finInvoiceVndTrxMpgManagerObj.GetReceiptCusAdjn(cusId);
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
        public List<ReceiptAdjnAllocation> GetCrDrAdjn(int cusId,long ReceiptPk)
        {
            FinInvoiceCusTrxMpgManager finInvoiceVndTrxMpgManagerObj;
            try
            {
                finInvoiceVndTrxMpgManagerObj = new FinInvoiceCusTrxMpgManager(currentContext);
                return finInvoiceVndTrxMpgManagerObj.GetCrDrAdjn(cusId, ReceiptPk);
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
            FinInvoiceCusHdrManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
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

         /// <summary>
        ///check duplicate based on bank + InstrNo + InstrDate
        /// </summary>
        /// <param name="finReceiptCusHdrObj"></param>
        /// <returns></returns>
        public bool CheckReceiptHdr(FIN_RECEIPT_CUS_HDR finReceiptCusHdrObj)
        {
            FinReceiptCusHdrManager finreceiptVndHdrManagerObj;
            try
            {
                finreceiptVndHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                return finreceiptVndHdrManagerObj.CheckReceiptHdr(finReceiptCusHdrObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finreceiptVndHdrManagerObj = null;
            }
        }

        //public long? SaveReceiptSplit(List<FIN_RECEIPT_CUS_SO_MPG> finPaymentVndPoMpgList)
        //{
        //    FinReceiptCusSoMpgManager finPaymentVndPoMpgManagerObj;
        //    long? finPaymentVndPoMpgPK;
        //    try
        //    {
        //        finPaymentVndPoMpgManagerObj = new FinReceiptCusSoMpgManager(currentContext);
        //        finPaymentVndPoMpgPK = finPaymentVndPoMpgManagerObj.SaveReceiptSplit(finPaymentVndPoMpgList);
        //        currentContext.SaveChanges();
        //        return finPaymentVndPoMpgPK.Value;
        //    }
        //    catch (OptimisticConcurrencyException ex)
        //    {
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    catch (UpdateException ex)
        //    {
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    finally
        //    {
        //        finPaymentVndPoMpgManagerObj = null;
        //        finPaymentVndPoMpgPK = null;
        //    }
        //}



        public DataTable GetReceiptAmtSplitUp(long InvID, long ReceiptPK, string DraftNo)
        {
            FinReceiptCusTrxMpgManager finReceiptCustTrxMangerObj;
            try
            {
                finReceiptCustTrxMangerObj = new FinReceiptCusTrxMpgManager(currentContext);
                return finReceiptCustTrxMangerObj.GetReceiptAmtSplitUp(InvID, ReceiptPK, DraftNo);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finReceiptCustTrxMangerObj = null;
            }
        }

        public List<ReceiptCrdrMpg> GetCrDrAllocations(List<long> SelectedInvoicePks, long ReceiptPk)
        {
            FinReceiptCusHdrManager finreceiptVndHdrManagerObj;
            try
            {
                finreceiptVndHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                return finreceiptVndHdrManagerObj.GetCrDrAllocations(SelectedInvoicePks, ReceiptPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finreceiptVndHdrManagerObj = null;
            }
        }

        public List<FIN_CRDR_NOTE_MPG> GetCrDrMpgList(long InvoicePK)
        {
            FinReceiptCusHdrManager finreceiptVndHdrManagerObj;
            try
            {
                finreceiptVndHdrManagerObj = new FinReceiptCusHdrManager(currentContext);
                return finreceiptVndHdrManagerObj.GetCrDrMpgList(InvoicePK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finreceiptVndHdrManagerObj = null;
            }
        }
    }
}
