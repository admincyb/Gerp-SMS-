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

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SalesInvoiceService" in code, svc and config file together.
    public class SalesInvoiceService : ISalesInvoiceService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

        #region Service Methods

        /// <summary>
        /// 
        /// </summary>
        public SalesInvoiceService()
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
        /// Get Conversion Factor
        /// </summary>
        /// <param name="FromCurrency"></param>
        /// <param name="ToCurrency"></param>
        /// <param name="TrxDate"></param>
        /// <param name="BizUnit"></param>
        /// <returns></returns>
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
        public string GetInvoiceNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK,int CpmanyPk)
        {
            CommonFunctionsManager commonFunctionsMgr;
            try
            {
                commonFunctionsMgr = new CommonFunctionsManager(currentContext);
                return commonFunctionsMgr.GetTrxDocNo(aptCode, astVal, dept, date, user, update, appPK,CpmanyPk);
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
        /// Get Sales Invoice Header Details
        /// </summary>
        /// <param name="finSalesInvoiceHdrObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_INVOICE_CUS_HDR> GetSalesInvoiceHdr(FIN_INVOICE_CUS_HDR finSalesInvoiceHdrObj, ServiceUtility serviceUtilityObj, int? Status = null, string siNo = null)
        {
            FinInvoiceCusHdrManager finInvoiceCusHdrManagerObj;
            try
            {
                finInvoiceCusHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                return finInvoiceCusHdrManagerObj.GetSalesInvoiceHdr(finSalesInvoiceHdrObj, serviceUtilityObj, Status,siNo);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceCusHdrManagerObj = null;
            }
        }


        /// <summary>
        /// Get Sales Invoice Header Details By PK
        /// </summary>
        /// <param name="finSalesInvoiceHdrObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_INVOICE_CUS_HDR> GetInvoiceCusHdrByPK(FIN_INVOICE_CUS_HDR InvoiceHdrObj)
        {
            FinInvoiceCusHdrManager finInvoiceCusHdrManagerObj;
            try
            {
                finInvoiceCusHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                return finInvoiceCusHdrManagerObj.GetInvoiceCusHdrByPK(InvoiceHdrObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceCusHdrManagerObj = null;
            }
        }

        /// <summary>
        /// Save Invoice Header and Details
        /// </summary>
        /// <param name="finSalesInvoiceHdrList"></param>
        /// <param name="isWkfSave"></param>
        /// <returns></returns>
        public long SaveSalesInvoiceHdr(List<FIN_INVOICE_CUS_HDR> finSalesInvoiceHdrList, bool isWkfSave = false)
        {
            FinInvoiceCusHdrManager finInvoiceCusHdrManagerObj;
            long? finInvoiceHdrPK;
            try
            {
                finInvoiceCusHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                finInvoiceHdrPK = finInvoiceCusHdrManagerObj.SaveSalesInvoiceHdr(finSalesInvoiceHdrList, isWkfSave);
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
                finInvoiceCusHdrManagerObj = null;
                finInvoiceHdrPK = null;
            }
        }

        /// <summary>
        /// Delete Invoice Header and Details
        /// </summary>
        /// <param name="finSalesInvoiceHdrList"></param>
        /// <returns></returns>
        public long DeleteSalesInvoice(long invoicePK)
        {
            FinInvoiceCusHdrManager finInvoiceCusHdrManagerObj;
            long? finInvoiceHdrPK;
            try
            {
                finInvoiceCusHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                finInvoiceHdrPK = finInvoiceCusHdrManagerObj.DeleteSalesInvoice(invoicePK);
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
                finInvoiceCusHdrManagerObj = null;
                finInvoiceHdrPK = null;
            }
        }

        public List<FIN_INVOICE_CUS_HDR> GetAdvanceSalesInvoiceNumberAutoCompleteList(FIN_INVOICE_CUS_HDR objSalesInvoice, ServiceUtility utilityObj)
        {
            FinInvoiceCusHdrManager objFinInvoiceCusHdrMgr;
            try
            {
                objFinInvoiceCusHdrMgr = new FinInvoiceCusHdrManager(currentContext);
                return objFinInvoiceCusHdrMgr.GetAdvanceSalesInvoiceNumberAutoCompleteList(objSalesInvoice, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinInvoiceCusHdrMgr = null;
            }
        }

        public List<FIN_INVOICE_CUS_HDR> GetInvoiceNumberAutoCompleteList(FIN_INVOICE_CUS_HDR objSalesInvoice, ServiceUtility utilityObj)
        {
            FinInvoiceCusHdrManager objFinInvoiceCusHdrMgr;
            try
            {
                objFinInvoiceCusHdrMgr = new FinInvoiceCusHdrManager(currentContext);
                return objFinInvoiceCusHdrMgr.GetSalesInvoiceNumberAutoCompleteList(objSalesInvoice, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinInvoiceCusHdrMgr = null;
            }
        }

        /// <summary>
        /// Update InvoiceHdr Jounalize Flag
        /// </summary>
        /// <param name="finInvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateInvoiceHdrJounalizeFlag(int InvPK, bool JounalizeFlag)
        {
            FinInvoiceCusHdrManager objFinInvoiceCusHdrMgr;
            long? finInvoiceHdrPK;
            try
            {
                objFinInvoiceCusHdrMgr = new FinInvoiceCusHdrManager(currentContext);
                finInvoiceHdrPK = objFinInvoiceCusHdrMgr.UpdateInvoiceHdrJounalizeFlag(InvPK, JounalizeFlag);
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
                objFinInvoiceCusHdrMgr = null;
                finInvoiceHdrPK = null;
            }
        }


        #endregion


        public List<POPayment> GetSalesInvoiceHdr(long invoicePK)
        {
            throw new NotImplementedException();
        }

        public long InActiveSalesInvoice(FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj)
        {
            FinInvoiceCusHdrManager finInvoiceCusHdrManagerObj;
            long? finInvoiceHdrPK;
            try
            {
                finInvoiceCusHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                finInvoiceHdrPK = finInvoiceCusHdrManagerObj.InActiveSalesInvoice(finInvoiceCusHdrObj);
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
                finInvoiceCusHdrManagerObj = null;
                finInvoiceHdrPK = null;
            }
        }

        public List<FIN_INVOICE_VND_HDR> GetInvoiceVndHdrByPK(FIN_INVOICE_VND_HDR objInvDetails)
        {
            FinInvoiceCusHdrManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetInvoiceVndHdrByPK(objInvDetails);
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

        public List<FIN_INVOICE_VND_DTL> GetInvoiceVndDtlByPK(FIN_INVOICE_VND_DTL objInvDetails)
        {
            FinInvoiceCusHdrManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetInvoiceVndDtlByPK(objInvDetails);
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

        public List<FIN_INVOICE_CUS_DTL> GetInvoiceCusDtlByPK(FIN_INVOICE_CUS_DTL objInvDetails)
        {
            FinInvoiceCusHdrManager finInvoiceVndHdrManagerObj;
            try
            {
                finInvoiceVndHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                return finInvoiceVndHdrManagerObj.GetInvoiceCusDtlByPK(objInvDetails);
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
        /// Get Invoice Received Amount Details
        /// </summary>
        /// <param name="InvoicePk"></param>
        /// <returns></returns>
        public DataTable GetInvCusReceivedAmntDetails(long InvoicePk)
        {
            FinInvoiceCusHdrManager finInvoiceCusHdrManagerObj;
            try
            {
                finInvoiceCusHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                return finInvoiceCusHdrManagerObj.GetInvCusReceivedAmntDetails(InvoicePk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceCusHdrManagerObj = null;
            }
        }


        public bool CheckReceiptCreated(FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj)
        {
            FinInvoiceCusHdrManager finInvoiceCusHdrManagerObj;
            try
            {
                finInvoiceCusHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                return finInvoiceCusHdrManagerObj.CheckReceiptCreated(finInvoiceCusHdrObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceCusHdrManagerObj = null;
            }
        }

        /// <summary>
        /// Get Sales Invoice Header Details By PK only (Not Consider Active and Delete Status)
        /// </summary>
        /// <param name="finSalesInvoiceHdrObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_INVOICE_CUS_HDR> GetInvoiceCusHdrByPKOnly(FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj)
        {
            FinInvoiceCusHdrManager finInvoiceCusHdrManagerObj;
            try
            {
                finInvoiceCusHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                return finInvoiceCusHdrManagerObj.GetInvoiceCusHdrByPKOnly(finInvoiceCusHdrObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceCusHdrManagerObj = null;
            }
        }

        public bool IsReceiptNotCreated(long InvPk)
        {
            FinInvoiceCusHdrManager finInvoiceCusHdrManagerObj;
            try
            {
                finInvoiceCusHdrManagerObj = new FinInvoiceCusHdrManager(currentContext);
                return finInvoiceCusHdrManagerObj.IsReceiptNotCreated(InvPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finInvoiceCusHdrManagerObj = null;
            }
        }
    }
}
