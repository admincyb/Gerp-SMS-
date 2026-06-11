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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DeliveryReportService" in code, svc and config file together.
    public class DeliveryOrderService : IDeliveryOrderService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

        #region Service Methods

        /// <summary>
        /// 
        /// </summary>
        public DeliveryOrderService()
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
        public string GetInvoiceNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK)
        {
            CommonFunctionsManager commonFunctionsMgr;
            try
            {
                commonFunctionsMgr = new CommonFunctionsManager(currentContext);
                return commonFunctionsMgr.GetTrxDocNo(aptCode, astVal, dept, date, user, update, appPK);
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
        public List<SAL_DESPATCH_HDR> GetSaleDespatchHdr(SAL_DESPATCH_HDR finSalesInvoiceHdrObj, ServiceUtility serviceUtilityObj)
        {
            SalDespatchHdrManager salDespatchHdrManagerObj;
            try
            {
                salDespatchHdrManagerObj = new SalDespatchHdrManager(currentContext);
                return salDespatchHdrManagerObj.GetSaleDespatchHdr(finSalesInvoiceHdrObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                salDespatchHdrManagerObj = null;
            }
        }

      

        /// <summary>
        /// Save Invoice Header and Details
        /// </summary>
        /// <param name="finSalesInvoiceHdrList"></param>
        /// <returns></returns>
        public long SaveDespatchHdr(List<SAL_DESPATCH_HDR> finSalesInvoiceHdrList)
        {
            SalDespatchHdrManager salDespatchHdrManagerObj;
            long? salDespatchHdrPK;
            try
            {
                salDespatchHdrManagerObj = new SalDespatchHdrManager(currentContext);
                salDespatchHdrPK = salDespatchHdrManagerObj.SaveDespatchHdr(finSalesInvoiceHdrList);
                currentContext.SaveChanges();
                return salDespatchHdrPK.Value;
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
                salDespatchHdrManagerObj = null;
                salDespatchHdrPK = null;
            }
        }

        /// <summary>
        /// Delete Invoice Header and Details
        /// </summary>
        /// <param name="finSalesInvoiceHdrList"></param>
        /// <returns></returns>
        public long DeleteSalDespatch(long invoicePK)
        {
            SalDespatchHdrManager salDespatchHdrManagerObj;
            long? salDespatchHdrPK;
            try
            {
                salDespatchHdrManagerObj = new SalDespatchHdrManager(currentContext);
                salDespatchHdrPK = salDespatchHdrManagerObj.DeleteSalDespatch(invoicePK);
                currentContext.SaveChanges();
                return salDespatchHdrPK.Value;
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
                salDespatchHdrManagerObj = null;
                salDespatchHdrPK = null;
            }
        }

        public List<SAL_DESPATCH_HDR> GetSaleDespatchNumberAutoCompleteList(SAL_DESPATCH_HDR objSalesInvoice, ServiceUtility utilityObj)
        {
            SalDespatchHdrManager salDespatchHdrManagerObj;
            try
            {
                salDespatchHdrManagerObj = new SalDespatchHdrManager(currentContext);
                return salDespatchHdrManagerObj.GetSaleDespatchNumberAutoCompleteList(objSalesInvoice, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                salDespatchHdrManagerObj = null;
            }
        }

        ///// <summary>
        ///// Update InvoiceHdr Jounalize Flag
        ///// </summary>
        ///// <param name="finInvoiceHdrList"></param>
        ///// <returns></returns>
        //public long UpdateInvoiceHdrJounalizeFlag(int InvPK, bool JounalizeFlag)
        //{
        //    FinInvoiceCusHdrManager objFinInvoiceCusHdrMgr;
        //    long? finInvoiceHdrPK;
        //    try
        //    {
        //        objFinInvoiceCusHdrMgr = new FinInvoiceCusHdrManager(currentContext);
        //        finInvoiceHdrPK = objFinInvoiceCusHdrMgr.UpdateInvoiceHdrJounalizeFlag(InvPK, JounalizeFlag);
        //        currentContext.SaveChanges();
        //        return finInvoiceHdrPK.Value;
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
        //        objFinInvoiceCusHdrMgr = null;
        //        finInvoiceHdrPK = null;
        //    }
        //}


        #endregion


        public List<SAL_DESPATCH_DTL> GetDespatchedSaleOrders(int despatchID,ServiceUtility serviceUtilityObj, int shippingPlanPK = 0)
        {
            SalDespatchDtlManager salDespatchDtlManagerObj;
            try
            {
                salDespatchDtlManagerObj = new SalDespatchDtlManager(currentContext);
                return salDespatchDtlManagerObj.GetDespatchedSaleOrders(despatchID,serviceUtilityObj, shippingPlanPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                salDespatchDtlManagerObj = null;
            }
        }
        public List<SAL_DESPATCH_DTL> GetDespatchedSaleOrderDetails(int despatchID, ServiceUtility serviceUtilityObj, int shippingPlanPK = 0)
        {
            SalDespatchDtlManager salDespatchDtlManagerObj;
            try
            {
                salDespatchDtlManagerObj = new SalDespatchDtlManager(currentContext);
                return salDespatchDtlManagerObj.GetDespatchedSaleOrderDetails(despatchID, serviceUtilityObj, shippingPlanPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                salDespatchDtlManagerObj = null;
            }
        }

        public string GetDespatchNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK,int? cmpanyPK=null)
        {
            CommonFunctionsManager commonFunctionsMgr;
            try
            {
                commonFunctionsMgr = new CommonFunctionsManager(currentContext);
                return commonFunctionsMgr.GetTrxDocNo(aptCode, astVal, dept, date, user, update, appPK,cmpanyPK);
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
        public void UpdateInvoiceDtls(SAL_DESPATCH_HDR salDespatchHdrList)
        {
            SalDespatchDtlManager salDespatchDtlManagerObj;
            try
            {
                salDespatchDtlManagerObj = new SalDespatchDtlManager(currentContext);
                salDespatchDtlManagerObj.UpdateInvoiceDtls(salDespatchHdrList);
                currentContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                salDespatchDtlManagerObj = null;
            }
        }


        public int CheckforAlreadyDespatch(Int32? shipPlanPK)
        {
            SalDespatchDtlManager salDespatchDtlManagerObj;
            try
            {
                salDespatchDtlManagerObj = new SalDespatchDtlManager(currentContext);
                return salDespatchDtlManagerObj.CheckforAlreadyDespatch(shipPlanPK);              
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                salDespatchDtlManagerObj = null;
            }
        }
    }
}
