using System;
using System.Collections.Generic;
using ERPData;
using ERPManager;
using System.Data;
using System.Diagnostics;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SaleOrderService" in code, svc and config file together.
    public class SaleOrderService : ISaleOrderService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public SaleOrderService()
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
        /// 
        /// </summary>
        /// <param name="objSaleOrderHeader"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>

        public List<SAL_ORDER_HDR> GetSaleOrderHeader(SAL_ORDER_HDR objSaleOrderHeader, ServiceUtility utilityObj, ApplicationSubType applicationSubType, int? status, PageType pageType, int wrkfStatus=-1)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetSaleOrderHeader(objSaleOrderHeader, utilityObj, applicationSubType, status, pageType, wrkfStatus);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objSaleOrderHeader"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>

        public List<SAL_ORDER_HDR> GetSaleOrderHeaderSales(SAL_ORDER_HDR objSaleOrderHeader, ServiceUtility utilityObj, ApplicationSubType applicationSubType, int? status, PageType pageType, int wrkfStatus = -1)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetSaleOrderHeaderSales(objSaleOrderHeader, utilityObj, applicationSubType, status, pageType, wrkfStatus);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }

        }
        
        
        public List<SAL_ORDER_HDR> GetSelectedSaleOrders(List<long> soPkList, ServiceUtility utilityObj)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetSelectedSaleOrders(soPkList, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }
        }

        public List<FIN_INVOICE_CUS_TRX_MPG> GetInvoicedSaleOrders(long invoicePK)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetInvoicedSaleOrders(invoicePK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }
        }


        public List<SAL_ORDER_DTL> GetSaleOrderDetails(int soPK, ServiceUtility utilityObj)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetSaleOrderDetails(soPK, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }
        }
        public List<SAL_ORDER_HDR> GetSoNumberAutoCompleteList(SAL_ORDER_HDR objSaleOrderHeader, ServiceUtility utilityObj)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetSoNumberAutoCompleteList(objSaleOrderHeader, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }
        }
        public List<SAL_ORDER_HDR> GetCusPoNumberAutoCompleteList(SAL_ORDER_HDR objSaleOrderHeader, ServiceUtility utilityObj)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetCusPoNumberAutoCompleteList(objSaleOrderHeader, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }
        }
        #endregion


        public List<SAL_ORDER_DTL> GetSelectedSaleOrderDetails(List<long> SelectedSos, ServiceUtility utilityObj, int shippingPlanPK = 0)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetSelectedSaleOrderDetails(SelectedSos, utilityObj, shippingPlanPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }
        }


        public List<SAL_ORDER_HDR> GetSaleOrderHdrByPK(SAL_ORDER_HDR objSalesOrderHeader)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetSaleOrderHdrByPK(objSalesOrderHeader);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }
        }
        public string GetPkFromScNo(string scNo)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetPkFromScNo(scNo);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }
        }
        public string GetPkFromDONo(string DONo)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetPkFromDONo(DONo);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }
        }

        public SAL_SHIPPING_PLAN_HDR GetShippingPlanHeader(int ShippingPlanPK)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetShippingPlanHeader(ShippingPlanPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }
        }

        public DataTable GetSaleOrderCopyEligibleDetails(int SoId)
        {
            SaleOrderManager objSaleOrderManager;
            try
            {
                objSaleOrderManager = new SaleOrderManager(currentContext);
                return objSaleOrderManager.GetSaleOrderCopyEligibleDetails(SoId);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objSaleOrderManager = null;
            }
        }
    }
}
