using System;
using System.Collections.Generic;
using ERPData;
using ERPManager;
using System.Data;
using System.Diagnostics;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "POListService" in code, svc and config file together.
    public class POListService : IPOListService, IPOListManager
    {
       
        #region Private Variables
        ERPEntities  currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public POListService()
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
        /// Perform searching,filtering,sorting,and paging on PO Master ;
        /// </summary>
        /// <param name="objPoHeader" value="PoHeader object with PoHeader pk and active status"></param>
        /// <param name="utilityObj" value="Search criteria object"></param>
        /// <returns>List of PoHeader</returns>
        public List<PUR_ORDER_HDR> GetPoHeader(PUR_ORDER_HDR objPoHeader, ServiceUtility utilityObj, int Status)
        {
            POListManager objPOListMgr;
            try
            {
                objPOListMgr = new POListManager(currentContext);
                return objPOListMgr.GetPoHeader(objPoHeader, utilityObj, Status);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objPOListMgr = null;
            }
        }

        //<summary>
        //Gets list of Po details
        //</summary>
        //<param name=" poPK"></param>
        //<param name="utilityObj"></param>
        //<returns>List of PoDetails</returns>
        public List<PUR_ORDER_DTL > GetPoDetails(int poPK, ServiceUtility utilityObj)
        {
            POListManager objPOListMgr;
            try
            {
                objPOListMgr = new POListManager(currentContext);
                return objPOListMgr.GetPoDetails(poPK, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objPOListMgr = null;
            }
        }


        public List<INV_GRN_HDR > GetGRNDetails(int poPK, ServiceUtility utilityObj)
        {
            POListManager objPOListMgr;
            try
            {
                objPOListMgr = new POListManager(currentContext);
                return objPOListMgr.GetGRNDetails(poPK, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objPOListMgr = null;
            }
        }

        public List<INV_GIN_HDR> GetGinDetails(int grnPK, ServiceUtility utilityObj)
        {
            POListManager objPOListMgr;
            try
            {
                objPOListMgr = new POListManager(currentContext);
                return objPOListMgr.GetGinDetails(grnPK, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objPOListMgr = null;
            }
        }

        public List<INV_GRN_DTL> GetGRNDetailsList(int poDtlPK, ServiceUtility utilityObj)
        {
            POListManager objPOListMgr;
            try
            {
                objPOListMgr = new POListManager(currentContext);
                return objPOListMgr.GetGRNDetailsList(poDtlPK, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objPOListMgr = null;
            }
        }

        public List<INV_GIN_DTL> GetGINDetailsList(int grnDtlPK, ServiceUtility utilityObj)
        {
            POListManager objPOListMgr;
            try
            {
                objPOListMgr = new POListManager(currentContext);
                return objPOListMgr.GetGINDetailsList(grnDtlPK, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objPOListMgr = null;
            }
        }

        public List<INV_STK_TRAN_HDR> GetStockTransferDetails(int ginPK, ServiceUtility utilityObj)
        {
            POListManager objPOListMgr;
            try
            {
                objPOListMgr = new POListManager(currentContext);
                return objPOListMgr.GetStockTransferDetails(ginPK, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objPOListMgr = null;
            }
        }

        public List<PUR_ORDER_HDR> GetPoNumberAutoCompleteList(PUR_ORDER_HDR objPoHeader, ServiceUtility utilityObj)
        {
            POListManager objPOListMgr;
            try
            {
                objPOListMgr = new POListManager(currentContext);
                return objPOListMgr.GetPoNumberAutoCompleteList(objPoHeader, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objPOListMgr = null;
            }
        }
        public List<PUR_ORDER_HDR> GetSelectedPOs(List<long> poPkList, ServiceUtility utilityObj)
        {
            POListManager objPOListMgr;
            try
            {
                objPOListMgr = new POListManager(currentContext);
                return objPOListMgr.GetSelectedPOs(poPkList, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objPOListMgr = null;
            }
        }
        public List<FIN_INVOICE_VND_TRX_MPG> GetInvoicedPOs(long invoicePK)
        {
            POListManager objPOListMgr;
            try
            {
                objPOListMgr = new POListManager(currentContext);
                return objPOListMgr.GetInvoicedPOs(invoicePK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objPOListMgr = null;
            }
        }

        public DataTable GetPurOrderTaxDetails(long invoicePK)
        {
            POListManager objPOListMgr;
            try
            {
                objPOListMgr = new POListManager(currentContext);
                return objPOListMgr.GetPurOrderTaxDetails(invoicePK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objPOListMgr = null;
            }
        }


        #endregion


     
    }
}
