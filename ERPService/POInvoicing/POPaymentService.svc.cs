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
using BusinessObject.POInvoicing;
using ERPManager.POInvoicing;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "POPaymentService" in code, svc and config file together.
    public class POPaymentService : IPOPaymentService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public POPaymentService()
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
        public List<FIN_PAYMENT_VND_HDR> GetPaymentHdr(FIN_PAYMENT_VND_HDR finPaymentHdrObj, ServiceUtility serviceUtilityObj, int? Status = null, string invNo = null, int? PDCStatus=0) 
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                return finPaymentVndHdrManagerObj.GetPaymentHdr(finPaymentHdrObj, serviceUtilityObj, Status, invNo, PDCStatus);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finPaymentVndHdrManagerObj = null;
            }
        }

        /// <summary>
        /// Get Payment Header Details
        /// </summary>
        /// <param name="paymentPK"></param>
        /// <returns></returns>
        public List<FIN_PAYMENT_VND_HDR> GetPaymentHdr(long paymentPK)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                return finPaymentVndHdrManagerObj.GetPaymentHdr(paymentPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finPaymentVndHdrManagerObj = null;
            }
        }
        /// <summary>
        /// Save Payment Header and Details
        /// </summary>
        /// <param name="finPaymentHdrList"></param>
        /// <returns></returns>
        public long? SavePaymentHdr(List<FIN_PAYMENT_VND_HDR> finPaymentHdrList)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            long? finPaymentHdrPK;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                finPaymentHdrPK = finPaymentVndHdrManagerObj.SavePaymentHdr(finPaymentHdrList);
                
                currentContext.SaveChanges();
                List<FIN_PAYMENT_VND_TAX_DTL> ZeroPk_finPaymentVndTaxDtlList;
                ZeroPk_finPaymentVndTaxDtlList = (from oldp in this.currentContext.FIN_PAYMENT_VND_TAX_DTL
                                                  where oldp.PDT_PK == 0
                                                  //&& !pks.Contains(oldp.PDT_PK)
                                                  select oldp).ToList();
                //Delete existing records against receipt
                foreach (FIN_PAYMENT_VND_TAX_DTL ZeroPk_finPaymentVndTaxDtlObj in ZeroPk_finPaymentVndTaxDtlList)
                {
                    this.currentContext.FIN_PAYMENT_VND_TAX_DTL.DeleteObject(ZeroPk_finPaymentVndTaxDtlObj);
                }

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
                finPaymentVndHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }
        /// <summary>
        /// Get Invoice Trx Mpg details;
        /// </summary>
        /// <param name="InvoicePK"></param>
        /// <returns>List of Invoice Trx Mpg</returns>
        public List<FIN_INVOICE_VND_HDR> GetPaymentTrxMpg(List<long> InvoicePK)
        {
            FinPaymentVndTrxMpgManager finPaymentVndTrxMpgManagerObj;
            try
            {
                finPaymentVndTrxMpgManagerObj = new FinPaymentVndTrxMpgManager(currentContext);
                return finPaymentVndTrxMpgManagerObj.GetPaymentTrxMpg(InvoicePK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finPaymentVndTrxMpgManagerObj = null;
            }
        }
        public List<FIN_PAYMENT_VND_HDR> GetPaymentNumberAutoCompleteList(FIN_PAYMENT_VND_HDR poPaymentObj, ServiceUtility serviceUtilityObj)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                return finPaymentVndHdrManagerObj.GetPaymentNumberAutoCompleteList(poPaymentObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finPaymentVndHdrManagerObj = null;
            }
        }
        public List<FIN_PAYMENT_VND_TRX_MPG> GetPaymentTrxMpg(long paymentPK)
        {
            FinPaymentVndTrxMpgManager FinPaymentVndTrxMpgManagerObj;
            try
            {
                FinPaymentVndTrxMpgManagerObj = new FinPaymentVndTrxMpgManager(currentContext);
                return FinPaymentVndTrxMpgManagerObj.GetPaymentTrxMpg(paymentPK);
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
        public List<FIN_PAYMENT_VND_TRX_MPG> GetInvoicePaymentTrxMpg(long invoicePK)
        {
            FinPaymentVndTrxMpgManager FinPaymentVndTrxMpgManagerObj;
            try
            {
                FinPaymentVndTrxMpgManagerObj = new FinPaymentVndTrxMpgManager(currentContext);
                return FinPaymentVndTrxMpgManagerObj.GetInvoicePaymentTrxMpg(invoicePK);
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

        public string GetPaymentNo(string  aptCode, int  astVal, int dept, DateTime date, int user, bool update, int appPK,int? cmpanyPK=null)
        {
            CommonFunctionsManager commonFunctionsManagerObj;
            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetTrxDocNo(aptCode,astVal, dept, date, user, update, appPK,cmpanyPK);
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
        #endregion
        
        public long? DeletePaymentHdr(long CurrPK)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            long? finPaymentHdrPK;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                finPaymentHdrPK = finPaymentVndHdrManagerObj.DeletePaymentHdr(CurrPK);
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
                finPaymentVndHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }

        /// <summary>
        /// Update Payment Hdr Jounalize Flag
        /// </summary>
        /// <param name="finInvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdatePaymentHdrJounalizeFlag(int PayPK, bool JounalizeFlag)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            long? finPaymentHdrPK;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                finPaymentHdrPK = finPaymentVndHdrManagerObj.UpdatePaymentHdrJounalizeFlag(PayPK, JounalizeFlag);
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
                finPaymentVndHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }


        public List<FIN_PAYMENT_VND_PO_MPG> GetFinPatmentVndPoMpgList(FIN_PAYMENT_VND_PO_MPG finPaymentVndPoMpgObj)
        {
            FinPaymentVndPoMpgManager finPaymentVndPoMpgManagerObj;
            try
            {
                finPaymentVndPoMpgManagerObj = new FinPaymentVndPoMpgManager(currentContext);
                return finPaymentVndPoMpgManagerObj.GetFinPatmentVndPoMpgList(finPaymentVndPoMpgObj);
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

        public List<FIN_INVOICE_VND_TRX_MPG> GetInvoiceTrxMpg(FIN_INVOICE_VND_TRX_MPG FinInvoiceVndTrxMpgObj)
        {
            FinInvoiceVndTrxMpgManager finInvoiceVndTrxMpgManagerObj;
            try
            {
                finInvoiceVndTrxMpgManagerObj = new FinInvoiceVndTrxMpgManager(currentContext);
                return finInvoiceVndTrxMpgManagerObj.GetInvoiceTrxMpg(FinInvoiceVndTrxMpgObj);
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

        public List<FIN_INVOICE_VND_HDR> GetInvoiceHdrByPK(FIN_INVOICE_VND_HDR finInvoiceVndHdrObjForPaymentSplit)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetInvoiceHdrByPK(finInvoiceVndHdrObjForPaymentSplit);
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

        public long? SavePaymentSplit(List<FIN_PAYMENT_VND_PO_MPG> finPaymentVndPoMpgList,ref long? maxTaxID)
        {
            FinPaymentVndPoMpgManager finPaymentVndPoMpgManagerObj;
            long? finPaymentVndPoMpgPK;
            try
            {
                long? maxID = 0;                
                finPaymentVndPoMpgManagerObj = new FinPaymentVndPoMpgManager(currentContext);
                finPaymentVndPoMpgPK = finPaymentVndPoMpgManagerObj.SavePaymentSplit(finPaymentVndPoMpgList,ref maxID,ref maxTaxID,0,0,0);
                currentContext.SaveChanges();
                return finPaymentVndPoMpgPK.Value;
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
                finPaymentVndPoMpgManagerObj = null;
                finPaymentVndPoMpgPK = null;
            }
        }

        public long? UpdatePaymentHdrPDCFlag(int paymentPK, byte pdcFlag)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            long? finPaymentHdrPK;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                finPaymentHdrPK = finPaymentVndHdrManagerObj.UpdatePaymentHdrPDCFlag(paymentPK, pdcFlag);
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
                finPaymentVndHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }

        public long? UpdatePaymentHdrBounceFlag(int paymentPK, byte bounceFlag)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            long? finPaymentHdrPK;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                finPaymentHdrPK = finPaymentVndHdrManagerObj.UpdatePaymentHdrBounceFlag(paymentPK, bounceFlag);
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
                finPaymentVndHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }

        /// <summary>
        ///check duplicate based on bank + InstrNo + InstrDate
        /// </summary>
        /// <param name="finPaymentVndHdrObj"></param>
        /// <returns></returns>
        public bool CheckPaymentHdr(FIN_PAYMENT_VND_HDR finPaymentVndHdrObj)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                return finPaymentVndHdrManagerObj.CheckPaymentHdr(finPaymentVndHdrObj);
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
                finPaymentVndHdrManagerObj = null;
            }
        }



        public long DeleteDirectPaymentHdr(int CurrPK)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            long? finPaymentHdrPK;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                finPaymentHdrPK = finPaymentVndHdrManagerObj.DeleteDirectPaymentHdr(CurrPK);
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
                finPaymentVndHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }
        public long? DeleteDirectVatTaxForPOPayment(long PaymentPK, byte WthCategory)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            long? finPaymentHdrPK;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                finPaymentHdrPK = finPaymentVndHdrManagerObj.DeleteDirectVatTaxForPOPayment(PaymentPK,WthCategory);
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
                finPaymentVndHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }
        public long? DeleteDirectVatTaxForDirectPayment(long WthTrxPk, byte WthCategoty)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            long? finPaymentHdrPK;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                finPaymentHdrPK = finPaymentVndHdrManagerObj.DeleteDirectVatTaxForDirectPayment( WthTrxPk, WthCategoty);
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
                finPaymentVndHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }
        public long? SaveDirectVatTaxForPOPayment(List<FIN_PAYMENT_VND_TAX_HDR> tempWHTTaxDetails)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            long? finPaymentHdrPK;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                finPaymentHdrPK = finPaymentVndHdrManagerObj.SaveDirectVatTaxForPOPayment(tempWHTTaxDetails);
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
                finPaymentVndHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }
        public long? SaveWhtTaxDirectForDirectPayment(List<FIN_PAYMENT_VND_TAX_HDR> tempWHTTaxDetails)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            long? finPaymentHdrPK;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                finPaymentHdrPK = finPaymentVndHdrManagerObj.SaveWhtTaxDirectForDirectPayment(tempWHTTaxDetails);
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
                finPaymentVndHdrManagerObj = null;
                finPaymentHdrPK = null;
            }
        }

        public List<ADM_DOC_ATTACH> GetDocAttachments(ADM_DOC_ATTACH admDocAttachObj, ServiceUtility serviceUtilityObj)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                return finPaymentVndHdrManagerObj.GetDocAttachments(admDocAttachObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finPaymentVndHdrManagerObj = null;
            }
        }

        public List<PaymentAdjnAllocation> GetCrDrAdjn(int VendorId)
        {
            FinPaymentVndPoMpgManager finPaymentVndPoMpgManagerObj;            
            try
            {                
                finPaymentVndPoMpgManagerObj = new FinPaymentVndPoMpgManager(currentContext);
                return finPaymentVndPoMpgManagerObj.GetCrDrAdjn(VendorId);                
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
                finPaymentVndPoMpgManagerObj = null;                
            }
           
        }

        public List<FIN_PAYMENT_VND_ALCN_DTL> GetPaymentVndAdjn(int VendorId)
        {
            FinPaymentVndPoMpgManager finPaymentVndPoMpgManagerObj;
            try
            {
                finPaymentVndPoMpgManagerObj = new FinPaymentVndPoMpgManager(currentContext);
                return finPaymentVndPoMpgManagerObj.GetPaymentVndAdjn(VendorId);
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
                finPaymentVndPoMpgManagerObj = null;
            }
        }
        public List<FIN_PAYMENT_VND_ALCN_DTL> GetFinPaymentAlcnListContext(List<FIN_PAYMENT_VND_ALCN_DTL> finPaymentAlcnList)
        {
            FinPaymentVndPoMpgManager finPaymentVndPoMpgManagerObj;
            try
            {
                finPaymentVndPoMpgManagerObj = new FinPaymentVndPoMpgManager(currentContext);
                return finPaymentVndPoMpgManagerObj.GetFinPaymentAlcnListContext(finPaymentAlcnList); 
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

        public List<FIN_PAYMENT_VND_ALCN_DTL> GetFinPaymentAlcnList(FIN_PAYMENT_VND_ALCN_DTL FinPaymentVndAllocationObj)
        {
            FinPaymentVndPoMpgManager finPaymentVndPoMpgManagerObj;
            try
            {
                finPaymentVndPoMpgManagerObj = new FinPaymentVndPoMpgManager(currentContext);
                return finPaymentVndPoMpgManagerObj.GetFinPaymentAlcnList(FinPaymentVndAllocationObj);
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
                finPaymentVndPoMpgManagerObj = null;
            }
        }

        public List<FIN_INVOICE_VND_ADV_DED_DTL> GetAdvDeductList(long InvoicePk, int? PoPk)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                return finPaymentVndHdrManagerObj.GetAdvDeductList(InvoicePk, PoPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finPaymentVndHdrManagerObj = null;
            }
        }

        public List<PaymentCrdrMpg> GetCrDrAllocations(List<long> SelectedInvoicePks, long PaymentPk)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                return finPaymentVndHdrManagerObj.GetCrDrAllocations(SelectedInvoicePks, PaymentPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finPaymentVndHdrManagerObj = null;
            }
        }

        public List<FIN_CRDR_NOTE_MPG> GetCrDrList(long InvoicePK)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                return finPaymentVndHdrManagerObj.GetCrDrList(InvoicePK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finPaymentVndHdrManagerObj = null;
            }
        }

        public FIN_INVOICE_VND_HDR GetInvoiceDetails(long NewInvPk)
        {
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj;
            try
            {
                finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(currentContext);
                return finPaymentVndHdrManagerObj.GetInvoiceDetails(NewInvPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finPaymentVndHdrManagerObj = null;
            }
        }
    }
}
