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

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "POInvoiceService" in code, svc and config file together.
    public class POInvoiceService : IPOInvoiceService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public POInvoiceService()
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
        /// Get Invoice Header Details
        /// </summary>
        /// <param name="finInvoiceHdrObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_INVOICE_VND_HDR> GetInvoiceHdr(FIN_INVOICE_VND_HDR finInvoiceHdrObj, ServiceUtility serviceUtilityObj,
            DateTime? PayByDateFrom = null, DateTime? PayByDateTo = null, int? Status = null, string poNo = null) 
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetInvoiceHdr(finInvoiceHdrObj, serviceUtilityObj, PayByDateFrom, PayByDateTo,Status,poNo); 
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
        /// Get Invoice Header count after filtering
        /// </summary>
        /// <param name="finInvoiceHdrObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public ServiceUtility GetInvoiceHdrCount(FIN_INVOICE_VND_HDR finInvoiceHdrObj, ServiceUtility serviceUtilityObj)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                //return finPaymentVndHdrManagerObj.GetPaymentHdrCount(finPaymentHdrObj, serviceUtilityObj); 
                return null;
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
        /// Get Invoice Header Details
        /// </summary>
        /// <param name="invoicePK"></param>
        /// <returns></returns>
        public List<POInvoice> GetPaymentHdr(long invoicePK)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                //return finPaymentVndHdrManagerObj.GetPaymentHdr(paymentPK); 
                return null;
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
        /// Save Invoice Header and Details
        /// </summary>
        /// <param name="finInvoiceHdrList"></param>
        /// <param name="isWkfSave"></param>
        /// <returns></returns>
        public long? SaveInvoiceHdr(List<FIN_INVOICE_VND_HDR> finInvoiceHdrList, bool isWkfSave = false, bool IsAdvInvHasTax = true)
        { 
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            long? finInvoiceHdrPK;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                finInvoiceHdrPK = finInvoiceVndHdrManagerObj.SaveInvoiceHdr(finInvoiceHdrList, isWkfSave, IsAdvInvHasTax);
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
                finInvoiceVndHdrManagerObj = null;
                finInvoiceHdrPK =null;
            }
        }

        /// <summary>
        /// Check vendor Invoice Exist
        /// </summary>
        /// <param name="vendorInvoive"></param>
        /// <returns></returns>
        public bool IsExistVendorInvoice(string vendorInvoiveNo, int vendorID, long invoicePK)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            bool result = false;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                result = finInvoiceVndHdrManagerObj.IsExistVendorInvoice(vendorInvoiveNo, vendorID, invoicePK);
                return result;
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
        /// Update InvoiceHdr Jounalize Flag
        /// </summary>
        /// <param name="finInvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateInvoiceHdrJounalizeFlag(int InvPK, bool JounalizeFlag)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            long? finInvoiceHdrPK;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                finInvoiceHdrPK = finInvoiceVndHdrManagerObj.UpdateInvoiceHdrJounalizeFlag(InvPK, JounalizeFlag);
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
                finInvoiceVndHdrManagerObj = null;
                finInvoiceHdrPK = null;
            }
        }


        /// <summary>
        /// Save Invoice Header and Details
        /// </summary>
        /// <param name="finInvoiceHdrList"></param>
        /// <returns></returns>
        public long SaveFinTrxDetails(int? pAppID, string pAppType)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            long? finInvoiceHdrPK;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                finInvoiceHdrPK = finInvoiceVndHdrManagerObj.SaveFinTrxDetails(pAppID, pAppType);
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
                finInvoiceVndHdrManagerObj = null;
                finInvoiceHdrPK = null;
            }
        }

        /// <summary>
        /// Save Invoice Header and Details
        /// </summary>
        /// <param name="finInvoiceHdrList"></param>
        /// <returns></returns>
        public long DeleteInvoice(long invoicePK)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            long? finInvoiceHdrPK;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                finInvoiceHdrPK = finInvoiceVndHdrManagerObj.DeleteInvoice(invoicePK);
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
                finInvoiceVndHdrManagerObj = null;
                finInvoiceHdrPK = null;
            }
        }
        public List<FIN_INVOICE_VND_HDR> GetInvoiceNumberAutoCompleteList(FIN_INVOICE_VND_HDR objPoInvoice, ServiceUtility utilityObj)
        {
            FinInvoiceVndHdrManager objFinInvoiceVndHdrMgr;
            try
            {
                objFinInvoiceVndHdrMgr = new FinInvoiceVndHdrManager(currentContext);
                return objFinInvoiceVndHdrMgr.GetInvoiceNumberAutoCompleteList(objPoInvoice, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinInvoiceVndHdrMgr = null;
            }
        }

        public List<FIN_INVOICE_VND_HDR> GetInvNumberAutoCompleteList(FIN_INVOICE_VND_HDR objPoInvoice, ServiceUtility utilityObj)
        {
            FinInvoiceVndHdrManager objFinInvoiceVndHdrMgr;
            try
            {
                objFinInvoiceVndHdrMgr = new FinInvoiceVndHdrManager(currentContext);
                return objFinInvoiceVndHdrMgr.GetInvNumberAutoCompleteList(objPoInvoice, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinInvoiceVndHdrMgr = null;
            }
        }

        public List<FIN_INVOICE_VND_HDR> GetConvertedInvNumbersAutoCompleteList(FIN_INVOICE_VND_HDR objPoInvoice, ServiceUtility utilityObj)
        {
            FinInvoiceVndHdrManager objFinInvoiceVndHdrMgr;
            try
            {
                objFinInvoiceVndHdrMgr = new FinInvoiceVndHdrManager(currentContext);
                return objFinInvoiceVndHdrMgr.GetConvertedInvNumbersAutoCompleteList(objPoInvoice, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinInvoiceVndHdrMgr = null;
            }
        }


        /// <summary>
        /// Get Invoice No
        /// </summary>
        /// <param name="astVal"></param>
        /// <param name="dept"></param>
        /// <param name="date"></param>
        /// <param name="user"></param>
        /// <param name="update"></param>
        /// <param name="appPK"></param>
        /// <returns></returns>
        public string GetInvoiceNo(string  aptCode,int  astVal, int dept, DateTime date, int user, bool update, int appPK,int? cmpanyPK=null)
        {
            CommonFunctionsManager  commonFunctionsMgr;
            try
            {
                commonFunctionsMgr = new CommonFunctionsManager(currentContext);
                return commonFunctionsMgr.GetTrxDocNo(aptCode,astVal, dept, date, user, update, appPK,cmpanyPK);
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

        /// <summary>
        /// Get Conversion Factor
        /// </summary>
        /// <param name="FromCurrency"></param>
        /// <param name="ToCurrency"></param>
        /// <param name="TrxDate"></param>
        /// <param name="BizUnit"></param>
        /// <returns></returns>
        public double  GetConversionFactor(int FromCurrency, int ToCurrency, DateTime TrxDate, int BizUnit)
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

        public List<FIN_INVOICE_VND_HDR> GetInvoiceHdrByPK(FIN_INVOICE_VND_HDR finInvoiceVndHdrObj)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetInvoiceHdrByPK(finInvoiceVndHdrObj);
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






        public long? SaveDocAttachemts(List<ADM_DOC_ATTACH> DocAttachList, int DocTaskId)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            long? DocPk;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                DocPk = finInvoiceVndHdrManagerObj.SaveDocAttachemts(DocAttachList, DocTaskId);
                currentContext.SaveChanges();
                return DocPk.Value;
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
                finInvoiceVndHdrManagerObj = null;
                DocPk = null;
            }
        }

        public List<ADM_DOC_ATTACH> GetDocAttachments(ADM_DOC_ATTACH admDocAttachObj, ServiceUtility serviceUtilityObj)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetDocAttachments(admDocAttachObj, serviceUtilityObj);
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

        public DataTable GetInvVndReceivedAmntDetails(long InvoicePk)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetInvVndReceivedAmntDetails(InvoicePk);
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


        public int AttachDocumentDelete(int? docPk, int? docTask, int? docTaskId)
        {
            FinInvoiceVndHdrManager finInvoiceVndHdrManagerObj;
            int retVal;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceVndHdrManager(currentContext);
                retVal = finInvoiceVndHdrManagerObj.AttachDocumentDelete(docPk, docTask, docTaskId);
                currentContext.SaveChanges();
                return retVal;
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
                finInvoiceVndHdrManagerObj = null;
            }
            return retVal;
        }
    }
}
