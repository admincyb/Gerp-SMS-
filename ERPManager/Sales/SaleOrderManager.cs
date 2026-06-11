using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;
using ERP.Utilities;


namespace ERPManager
{
    public class SaleOrderManager : ISaleOrderManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion
        #region Manager Methods

        /// <summary>
        /// Saleorder Master Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public SaleOrderManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Get Sale order Header
        /// </summary>
        /// <param name="objSAL_ORDER_HDR"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<SAL_ORDER_HDR> GetSaleOrderHeader(SAL_ORDER_HDR objSAL_ORDER_HDR, ServiceUtility utilityObj, ApplicationSubType applicationSubType, int? status, PageType pageType,
            int wrkfStatus = -1)
        {
            List<SAL_ORDER_HDR> ObjSAL_ORDER_HDRLst = new List<SAL_ORDER_HDR>();
            IQueryable<SAL_ORDER_HDR> SAL_ORDER_HDRQuery;
            int gonPK;
            try
            {
                List<int> Status = new List<int>();
                Status.AddRange(new int[] { 0, 1, 2, 4, 12, 20, 104, 21});
                List<int> ApprovedStatus = new List<int>();// Used for Sales/SOStatusTracking.aspx Page, To show records of all approved status 
                ApprovedStatus.AddRange(new int[] { 2, 7, 11, 12, 20 });
                if (objSAL_ORDER_HDR.SOH_PK > 0)
                {

                    SAL_ORDER_HDRQuery = (from poh in this.currentEntity.SAL_ORDER_HDR
                                          where poh.SOH_ACTIVE == objSAL_ORDER_HDR.SOH_ACTIVE
                                             && poh.SOH_BIZUNIT == objSAL_ORDER_HDR.SOH_BIZUNIT
                                             && poh.SOH_PK == objSAL_ORDER_HDR.SOH_PK
                                             && (poh.SOH_STATUS != 0 || objSAL_ORDER_HDR.SOH_CRTD_BY == 0 || poh.SOH_CRTD_BY == objSAL_ORDER_HDR.SOH_CRTD_BY)
                                             && poh.SOH_TRX_STATUS == (objSAL_ORDER_HDR.SOH_TRX_STATUS > 0 ? objSAL_ORDER_HDR.SOH_TRX_STATUS : poh.SOH_TRX_STATUS)
                                             && poh.SOH_CUSTOMER == (objSAL_ORDER_HDR.SOH_CUSTOMER > 0 ? objSAL_ORDER_HDR.SOH_CUSTOMER : poh.SOH_CUSTOMER)
                                          
                                          select poh);
                }
                else
                {
                    SAL_ORDER_HDRQuery = (from poh in this.currentEntity.SAL_ORDER_HDR
                                          join ccm in this.currentEntity.CRM_CUSTOMER_MST on poh.SOH_CUSTOMER equals ccm.CUS_PK
                                          where poh.SOH_ACTIVE == objSAL_ORDER_HDR.SOH_ACTIVE
                                             && (status == -1 ? true : poh.SOH_DEL_STATUS == ((objSAL_ORDER_HDR.SOH_ACTIVE == 1 && wrkfStatus != 4 ) ? 0 : poh.SOH_DEL_STATUS))
                                             && (status == -5 ? ApprovedStatus.Contains(poh.SOH_STATUS) : true)  //-5 for Showing all approved status from Sales/SOSTracking.aspx
                                             && (wrkfStatus == -1 ? true : poh.SOH_STATUS == wrkfStatus)// 104 Shortclosed
                                             && poh.SOH_PK == (objSAL_ORDER_HDR.SOH_PK > 0 ? objSAL_ORDER_HDR.SOH_PK : poh.SOH_PK)
                                             && poh.SOH_PK == (objSAL_ORDER_HDR.SOH_PK == -1 ? objSAL_ORDER_HDR.SOH_PK : poh.SOH_PK) //For invalid scno
                                             && poh.SOH_NO== (string.IsNullOrEmpty(objSAL_ORDER_HDR.SOH_NO) ? poh.SOH_NO : objSAL_ORDER_HDR.SOH_NO)
                                             && poh.SOH_CUSTOMER == (objSAL_ORDER_HDR.SOH_CUSTOMER > 0 ? objSAL_ORDER_HDR.SOH_CUSTOMER : poh.SOH_CUSTOMER)
                                             && (poh.SOH_STATUS != 0 || objSAL_ORDER_HDR.SOH_CRTD_BY == 0 || poh.SOH_CRTD_BY == objSAL_ORDER_HDR.SOH_CRTD_BY)
                                             && poh.SOH_DATE >= (utilityObj.FilterDate == null ? poh.SOH_DATE : utilityObj.FilterDate)
                                             && poh.SOH_DATE <= (utilityObj.FilterToDate == null ? poh.SOH_DATE : utilityObj.FilterToDate)
                                             && poh.SOH_TYPE == (utilityObj.InvoiceType > 0 ? utilityObj.InvoiceType : poh.SOH_TYPE)
                                             && poh.SOH_BIZUNIT == objSAL_ORDER_HDR.SOH_BIZUNIT //Bizunit Filteration Added 
                                             && poh.SOH_REFERENCE ==(objSAL_ORDER_HDR.SOH_REFERENCE!=null ? objSAL_ORDER_HDR.SOH_REFERENCE :poh.SOH_REFERENCE)
                                             && (status == 0 ? (
                                                                    Status.Contains(poh.SOH_STATUS) &&
                                                                   ((poh.SAL_ORDER_DTL.Any(d => d.SOD_QTY > d.SOD_QTY_DISPATCHED)) ||
                                                                   (poh.SAL_ORDER_DTL.Any(i => i.SOD_QTY > i.SOD_QTY_INVOICED)))
                                                               ) : status == 2 ? (
                                                               (poh.SAL_ORDER_DTL.All(d => d.SOD_QTY <= d.SOD_QTY_DISPATCHED)) &&
                                                                   (poh.SAL_ORDER_DTL.All(i => i.SOD_QTY <= i.SOD_QTY_INVOICED))
                                                               ) : status == 3 ? poh.SOH_STATUS != 2 : status == -1 ? poh.SOH_DEL_STATUS == 1 : true)
                                             && (pageType == PageType.SHIPPING || pageType == PageType.INVOICE ? (Status.Contains(poh.SOH_STATUS))
                                             : pageType == PageType.SALE ? (poh.SOH_STATUS != 0 && poh.SOH_DEL_STATUS == 0)
                                             : true)
                                              //For Internal Order
                                             && poh.SOH_TRX_STATUS == (objSAL_ORDER_HDR.SOH_TRX_STATUS > 0 ? objSAL_ORDER_HDR.SOH_TRX_STATUS : poh.SOH_TRX_STATUS)
                                             && poh.SOH_COMPANY == (objSAL_ORDER_HDR.SOH_COMPANY > 0 ? objSAL_ORDER_HDR.SOH_COMPANY : poh.SOH_COMPANY)
                                             && objSAL_ORDER_HDR.SOH_CONV_INV_NO == null ? true : poh.SOH_CONV_INV_NO.Contains(objSAL_ORDER_HDR.SOH_CONV_INV_NO)
                                          select poh);
                }
                if (objSAL_ORDER_HDR.SAL_DESPATCH_DTL != null && objSAL_ORDER_HDR.SAL_DESPATCH_DTL.Count() > 0 && objSAL_ORDER_HDR.SAL_DESPATCH_DTL.First().DPD_DESPATCH_HDR > 0)
                {
                    gonPK = objSAL_ORDER_HDR.SAL_DESPATCH_DTL.First().DPD_DESPATCH_HDR;
                    SAL_ORDER_HDRQuery = SAL_ORDER_HDRQuery.Where(poh => poh.SAL_DESPATCH_DTL.Where(desp => desp.DPD_QTY_DESPATCHED > 0 && desp.DPD_DESPATCH_HDR == gonPK).Count() > 0);
                }
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = SAL_ORDER_HDRQuery.Count();
                ObjSAL_ORDER_HDRLst = SAL_ORDER_HDRQuery.SortRecords<SAL_ORDER_HDR>(utilityObj).ToList();
            }
            catch
            {
            }

            return ObjSAL_ORDER_HDRLst;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Get Sale order Header
        /// </summary>
        /// <param name="objSAL_ORDER_HDR"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<SAL_ORDER_HDR> GetSaleOrderHeaderSales(SAL_ORDER_HDR objSAL_ORDER_HDR, ServiceUtility utilityObj, ApplicationSubType applicationSubType, int? status, PageType pageType, int wrkfStatus = -1)
        {
            List<SAL_ORDER_HDR> ObjSAL_ORDER_HDRLst = new List<SAL_ORDER_HDR>();
            IQueryable<SAL_ORDER_HDR> SAL_ORDER_HDRQuery;
            int gonPK;
            try
            {
                /*
                //if ( applicationSubType.Equals(ApplicationSubType.DELIVERYORDER))
                ObjSAL_ORDER_HDRLst = (from poh in this.currentEntity.SAL_ORDER_HDR
                                            where   poh.SOH_ACTIVE == objSAL_ORDER_HDR.SOH_ACTIVE
                                               && poh.SOH_PK == (objSAL_ORDER_HDR.SOH_PK > 0 ? objSAL_ORDER_HDR.SOH_PK : poh.SOH_PK)
                                               && poh.SOH_CUSTOMER == (objSAL_ORDER_HDR.SOH_CUSTOMER > 0 ? objSAL_ORDER_HDR.SOH_CUSTOMER : poh.SOH_CUSTOMER)
                                               && poh.SOH_DATE >= (utilityObj.FilterDate == null ? poh.SOH_DATE : utilityObj.FilterDate)
                                               && poh.SOH_DATE <= (utilityObj.FilterToDate == null ? poh.SOH_DATE : utilityObj.FilterToDate)
                                               //&& (poh.SOH_STATUS == 2 || poh.SOH_STATUS == 5)  //  Approved & Closed
                                               && ((status == 0 && ApplicationSubType.DELIVERYORDER == applicationSubType) ?
                                                        (poh.SAL_ORDER_DTL.Any(d => d.SOD_BAL_TO_DISPATCH > 0)) : true)
                                               && ((status == 0 && ApplicationSubType.INVOICE == applicationSubType) ?
                                                   (poh.SOH_STATUS != 4 && poh.SAL_ORDER_DTL.Any(i => i.SOD_QTY_APPROVED  > i.SOD_QTY_INVOICED))
                                                    : true)         //  Pending
                                               && ((status == 2 && ApplicationSubType.INVOICE == applicationSubType) ?
                                                   (poh.SOH_STATUS == 4 || poh.SAL_ORDER_DTL.All(i => i.SOD_QTY_APPROVED <= i.SOD_QTY_INVOICED))
                                                : true)             //  Completed
                                        select poh).ToList();
                */

                //  Filteration Modified by rasheed as per Syed / Savin Sir (LOAD PENDING TO INVOICE / DELIVER)

                //[CEH_CRTD_BY]	= '+CAST(@P_USER_PK AS VARCHAR)+' 
                //        --AND	[CEH_STATUS]	= 0

                List<int> Status = new List<int>();
                Status.AddRange(new int[] { 2, 12, 20 });//*Nimisha-dnt change without asking me :)
                //&& (utilityObj.NeedAdvanceFilter ? Status.Contains(po.SOH_STATUS) : true) //&& po.SOH_STATUS == (utilityObj.NeedAdvanceFilter ? objSAL_ORDER_HDR.SOH_STATUS : po.SOH_STATUS)

                if (objSAL_ORDER_HDR.SOH_PK > 0)
                {
                    SAL_ORDER_HDRQuery = (from poh in this.currentEntity.SAL_ORDER_HDR
                                          where poh.SOH_ACTIVE == objSAL_ORDER_HDR.SOH_ACTIVE
                                              //&& (status == -1 ? true : poh.SOH_DEL_STATUS == (objSAL_ORDER_HDR.SOH_ACTIVE == 1 ? 0 : poh.SOH_DEL_STATUS))
                                              // && (wrkfStatus == -1 ? true : poh.SOH_STATUS == wrkfStatus)//* nimisha
                                             && poh.SOH_PK == objSAL_ORDER_HDR.SOH_PK
                                             && poh.SOH_BIZUNIT == objSAL_ORDER_HDR.SOH_BIZUNIT
                                              //&& poh.SOH_CUSTOMER == (objSAL_ORDER_HDR.SOH_CUSTOMER > 0 ? objSAL_ORDER_HDR.SOH_CUSTOMER : poh.SOH_CUSTOMER)
                                             && (poh.SOH_STATUS != 0 || objSAL_ORDER_HDR.SOH_CRTD_BY == 0 || poh.SOH_CRTD_BY == objSAL_ORDER_HDR.SOH_CRTD_BY)
                                              //&& poh.SOH_DATE >= (utilityObj.FilterDate == null ? poh.SOH_DATE : utilityObj.FilterDate)
                                              //&& poh.SOH_DATE <= (utilityObj.FilterToDate == null ? poh.SOH_DATE : utilityObj.FilterToDate)
                                              //&& (status == 0 ? (
                                              //                       Status.Contains(poh.SOH_STATUS) &&
                                              //                      ((poh.SAL_ORDER_DTL.Any(d => d.SOD_QTY > d.SOD_QTY_DISPATCHED)) ||
                                              //                      (poh.SAL_ORDER_DTL.Any(i => i.SOD_QTY > i.SOD_QTY_INVOICED)))
                                              //                  ) : status == 2 ? (
                                              //                  (poh.SAL_ORDER_DTL.All(d => d.SOD_QTY <= d.SOD_QTY_DISPATCHED)) &&
                                              //                      (poh.SAL_ORDER_DTL.All(i => i.SOD_QTY <= i.SOD_QTY_INVOICED))
                                              //                  ) : status == 3 ? poh.SOH_STATUS != 2 : status == -1 ? poh.SOH_DEL_STATUS == 1 : true)
                                              //&& (pageType == PageType.SHIPPING || pageType == PageType.INVOICE ? (Status.Contains(poh.SOH_STATUS))
                                              //: pageType == PageType.SALE ? (poh.SOH_STATUS != 0 && poh.SOH_DEL_STATUS == 0)
                                              //: pageType == PageType.CUSTOMERUSER ? (poh.SOH_STATUS == 2 || poh.SOH_STATUS == 1) : true)
                                              //: true)
                                              //For Internal Order
                                             && poh.SOH_TRX_STATUS == (objSAL_ORDER_HDR.SOH_TRX_STATUS > 0 ? objSAL_ORDER_HDR.SOH_TRX_STATUS : poh.SOH_TRX_STATUS)
                                             && poh.SOH_COMPANY == (objSAL_ORDER_HDR.SOH_COMPANY > 0 ? objSAL_ORDER_HDR.SOH_COMPANY : poh.SOH_COMPANY)
                                          select poh);
                }
                else
                {
                    List<int> ShippingPklist = new List<int>();
                    ShippingPklist = (from pp in this.currentEntity.SAL_SHIPPING_PLAN_DTL join ssd in this.currentEntity.SAL_ORDER_DTL on pp.SND_SOD equals ssd.SOD_PK where pp.SAL_SHIPPING_PLAN_HDR.SNH_DEL_STATUS == 0 select ssd.SOD_SO).Distinct().ToList();
                    //ShippingPklist.AddRange(new int[] { 513,512,511 });
                    if (objSAL_ORDER_HDR.SOH_CUSTOMER == 0 && objSAL_ORDER_HDR.SOH_PK == 0 && utilityObj.FilterDate == null && utilityObj.FilterToDate == null && utilityObj.InvoiceType == 0 && wrkfStatus == -1 && objSAL_ORDER_HDR.SOH_TRX_STATUS == 0)
                    {
                        SAL_ORDER_HDRQuery = (from poh in this.currentEntity.SAL_ORDER_HDR
                                              join ccm in this.currentEntity.CRM_CUSTOMER_MST on poh.SOH_CUSTOMER equals ccm.CUS_PK
                                              where poh.SOH_ACTIVE == objSAL_ORDER_HDR.SOH_ACTIVE
                                                 && poh.SOH_BIZUNIT == objSAL_ORDER_HDR.SOH_BIZUNIT
                                                 && (status == -1 ? true : poh.SOH_DEL_STATUS == (objSAL_ORDER_HDR.SOH_ACTIVE == 1 ? 0 : poh.SOH_DEL_STATUS))
                                                 && (poh.SOH_STATUS != 0 || objSAL_ORDER_HDR.SOH_CRTD_BY == 0 || poh.SOH_CRTD_BY == objSAL_ORDER_HDR.SOH_CRTD_BY)
                                                 && (status == 0 ? (
                                                                        Status.Contains(poh.SOH_STATUS) &&
                                                                       ((poh.SAL_ORDER_DTL.Any(d => d.SOD_QTY > d.SOD_QTY_DISPATCHED || d.SOD_QTY > d.SOD_QTY_INVOICED)))
                                                                   ) : status == 2 ? (
                                                                   (poh.SAL_ORDER_DTL.All(d => d.SOD_QTY <= d.SOD_QTY_DISPATCHED && d.SOD_QTY <= d.SOD_QTY_INVOICED))
                                                                   ) : status == 3 ? poh.SOH_STATUS != 2 : status == -1 ? poh.SOH_DEL_STATUS == 1 ://&& poh.SOH_PK!= q.SOD_PK
                                                                   status == 9 ? !ShippingPklist.Contains(poh.SOH_PK) : true)
                                                 && (pageType == PageType.SHIPPING || pageType == PageType.INVOICE ? (Status.Contains(poh.SOH_STATUS))
                                                 : pageType == PageType.SALE ? (poh.SOH_STATUS != 0 && poh.SOH_DEL_STATUS == 0) : true)
                                                  && poh.SOH_COMPANY == (objSAL_ORDER_HDR.SOH_COMPANY > 0 ? objSAL_ORDER_HDR.SOH_COMPANY : poh.SOH_COMPANY)
                                              select poh);
                    }
                    else
                    {
                        SAL_ORDER_HDRQuery = (from poh in this.currentEntity.SAL_ORDER_HDR
                                              join ccm in this.currentEntity.CRM_CUSTOMER_MST on poh.SOH_CUSTOMER equals ccm.CUS_PK
                                              where poh.SOH_ACTIVE == objSAL_ORDER_HDR.SOH_ACTIVE
                                                 && poh.SOH_BIZUNIT == objSAL_ORDER_HDR.SOH_BIZUNIT
                                                 && (status == -1 ? true : poh.SOH_DEL_STATUS == (objSAL_ORDER_HDR.SOH_ACTIVE == 1 ? 0 : poh.SOH_DEL_STATUS))
                                                 && (wrkfStatus == -1 ? true : poh.SOH_STATUS == wrkfStatus)//* nimisha
                                                 && poh.SOH_PK == (objSAL_ORDER_HDR.SOH_PK > 0 ? objSAL_ORDER_HDR.SOH_PK : poh.SOH_PK)
                                                 && poh.SOH_CUSTOMER == (objSAL_ORDER_HDR.SOH_CUSTOMER > 0 ? objSAL_ORDER_HDR.SOH_CUSTOMER : poh.SOH_CUSTOMER)
                                                 && (poh.SOH_STATUS != 0 || objSAL_ORDER_HDR.SOH_CRTD_BY == 0 || poh.SOH_CRTD_BY == objSAL_ORDER_HDR.SOH_CRTD_BY)
                                                 && poh.SOH_DATE >= (utilityObj.FilterDate == null ? poh.SOH_DATE : utilityObj.FilterDate)
                                                 && poh.SOH_DATE <= (utilityObj.FilterToDate == null ? poh.SOH_DATE : utilityObj.FilterToDate)
                                                 && poh.SOH_TYPE == (utilityObj.InvoiceType > 0 ? utilityObj.InvoiceType : poh.SOH_TYPE)
                                                  //&& (status == 0 ? (
                                                  //                       Status.Contains(poh.SOH_STATUS) &&
                                                  //                      ((poh.SAL_ORDER_DTL.Any(d => d.SOD_QTY > d.SOD_QTY_DISPATCHED)) ||
                                                  //                      (poh.SAL_ORDER_DTL.Any(i => i.SOD_QTY > i.SOD_QTY_INVOICED)))
                                                  //                  ) : status == 2 ? (
                                                  //                  (poh.SAL_ORDER_DTL.All(d => d.SOD_QTY <= d.SOD_QTY_DISPATCHED)) &&
                                                  //                      (poh.SAL_ORDER_DTL.All(i => i.SOD_QTY <= i.SOD_QTY_INVOICED))
                                                  //                  ) : status == 3 ? poh.SOH_STATUS != 2 : status == -1 ? poh.SOH_DEL_STATUS == 1 ://&& poh.SOH_PK!= q.SOD_PK
                                                  //                  status == 9 ? !ShippingPklist.Contains(poh.SOH_PK) : true)
                                                  && (status == 0 ? (
                                                                        Status.Contains(poh.SOH_STATUS) &&
                                                                       ((poh.SAL_ORDER_DTL.Any(d => d.SOD_QTY > d.SOD_QTY_DISPATCHED || d.SOD_QTY > d.SOD_QTY_INVOICED)))
                                                                   ) : status == 2 ? (
                                                                   (poh.SAL_ORDER_DTL.All(d => d.SOD_QTY <= d.SOD_QTY_DISPATCHED && d.SOD_QTY <= d.SOD_QTY_INVOICED))
                                                                   ) : status == 3 ? poh.SOH_STATUS != 2 : status == -1 ? poh.SOH_DEL_STATUS == 1 ://&& poh.SOH_PK!= q.SOD_PK
                                                                   status == 9 ? !ShippingPklist.Contains(poh.SOH_PK) : true)
                                                  //(poh.SAL_ORDER_DTL.Any(q => q.SAL_SHIPPING_PLAN_DTL.Any(d => d.SND_SOD == q.SOD_PK &&  d.SND_PLAN_HDR == d.SAL_SHIPPING_PLAN_HDR.SNH_PK && d.SAL_SHIPPING_PLAN_HDR.SNH_DEL_STATUS == 0))) 


                                                  //(d => d.SAL_SHIPPING_PLAN_DTL.Any(g => g.SND_SOD == g.SAL_ORDER_DTL.SOD_PK && g.SAL_SHIPPING_PLAN_HDR.SNH_PK == g.SND_PLAN_HDR && g.SAL_SHIPPING_PLAN_HDR.SNH_DEL_STATUS==0) && d.SAL_ORDER_HDR.SOH_PK!=d.SOD_PK)): true)

                                                 && (pageType == PageType.SHIPPING || pageType == PageType.INVOICE ? (Status.Contains(poh.SOH_STATUS))
                                                 : pageType == PageType.SALE ? (poh.SOH_STATUS != 0 && poh.SOH_DEL_STATUS == 0)
                                                  //: pageType == PageType.CUSTOMERUSER ? (poh.SOH_STATUS == 2 || poh.SOH_STATUS == 1) : true)
                                                 : true)
                                                  //For Internal Order
                                                 && (objSAL_ORDER_HDR.SOH_TRX_STATUS > 0 ? poh.SOH_TRX_STATUS == objSAL_ORDER_HDR.SOH_TRX_STATUS : true)
                                                 && poh.SOH_COMPANY == (objSAL_ORDER_HDR.SOH_COMPANY > 0 ? objSAL_ORDER_HDR.SOH_COMPANY : poh.SOH_COMPANY)
                                              select poh);
                    }
                }
                if (objSAL_ORDER_HDR.SAL_DESPATCH_DTL != null && objSAL_ORDER_HDR.SAL_DESPATCH_DTL.Count() > 0 && objSAL_ORDER_HDR.SAL_DESPATCH_DTL.First().DPD_DESPATCH_HDR > 0)
                {
                    gonPK = objSAL_ORDER_HDR.SAL_DESPATCH_DTL.First().DPD_DESPATCH_HDR;
                    // SAL_ORDER_HDRQuery = SAL_ORDER_HDRQuery.Where(poh => poh.SAL_DESPATCH_DTL.Where(desp => desp.DPD_DESPATCH_HDR == gonPK).Count() > 0);
                    SAL_ORDER_HDRQuery = SAL_ORDER_HDRQuery.Where(poh => poh.SAL_DESPATCH_DTL.Where(desp => desp.DPD_QTY_DESPATCHED > 0 && desp.DPD_DESPATCH_HDR == gonPK).Count() > 0);
                }
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = SAL_ORDER_HDRQuery.GroupBy(s => s.SOH_PK).Count();
                ObjSAL_ORDER_HDRLst = SAL_ORDER_HDRQuery.SortRecords<SAL_ORDER_HDR>(utilityObj).ToList();
            }
            catch
            {
            }

            return ObjSAL_ORDER_HDRLst;
            //throw new NotImplementedException();
        }



        /// <summary>
        /// Get selected Sale Orders 
        /// </summary>
        /// <param name="soPkList"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<SAL_ORDER_HDR> GetSelectedSaleOrders(List<long> soPkList, ServiceUtility utilityObj)
        {
            List<SAL_ORDER_HDR> ObjSAL_ORDER_HDRLst = new List<SAL_ORDER_HDR>();
            try
            {
                ObjSAL_ORDER_HDRLst = (from poh in this.currentEntity.SAL_ORDER_HDR
                                       //join vnd in this.currentEntity.SAL_CUSTOMER_MASTER on poh.SOH_CUSTOMER equals vnd.CUS_PK
                                       where soPkList.Contains(poh.SOH_PK)
                                       select poh).ToList();

            }
            catch
            {
            }

            return ObjSAL_ORDER_HDRLst;

        }

        /// <summary>
        /// Get invoiced Sale Orders
        /// </summary>
        /// <param name="invoicePK"></param>
        /// <returns></returns>
        public List<FIN_INVOICE_CUS_TRX_MPG> GetInvoicedSaleOrders(long invoicePK)
        {
            List<FIN_INVOICE_CUS_TRX_MPG> ObjSAL_ORDER_HDRLst = new List<FIN_INVOICE_CUS_TRX_MPG>();
            try
            {
                ObjSAL_ORDER_HDRLst = (from ivd in this.currentEntity.FIN_INVOICE_CUS_TRX_MPG
                                       join poh in this.currentEntity.SAL_ORDER_HDR on ivd.ICM_SO_HDR equals poh.SOH_PK
                                       join vnd in this.currentEntity.CRM_CUSTOMER_MST on poh.SOH_CUSTOMER equals vnd.CUS_PK
                                       where ivd.ICM_INVOICE_HDR == invoicePK
                                         && ivd.ICM_ACTIVE == 1
                                       select ivd).ToList();

            }
            catch
            {
            }

            return ObjSAL_ORDER_HDRLst;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Get Sale Order Details
        /// </summary>
        /// <param name="soPK"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<SAL_ORDER_DTL> GetSaleOrderDetails(int soPK, ServiceUtility utilityObj)
        {
            List<SAL_ORDER_DTL> ObjSAL_ORDER_DTLLst = new List<SAL_ORDER_DTL>();
            try
            {
                ObjSAL_ORDER_DTLLst = (from pod in this.currentEntity.SAL_ORDER_DTL
                                       join itm in this.currentEntity.INV_ITEM_MST on pod.SOD_ITEM equals itm.ITM_PK
                                       join uom in this.currentEntity.INV_UOM_MST on pod.SOD_UOM equals uom.UOM_PK
                                       join cat in this.currentEntity.INV_ITEM_CATEGORY on itm.ITM_CATEGORY equals cat.ITC_PK
                                       join cfg in this.currentEntity.ADM_CONFIG_MST on pod.SOD_SALE_UOM equals cfg.CFG_PK
                                       where pod.SOD_SO == soPK
                                       orderby pod.SOD_NO, pod.SOD_SL_NO
                                       select pod).ToList();
            }
            catch { }

            return ObjSAL_ORDER_DTLLst;
        }

        /// <summary>
        /// Get Sale Order Numbers for auto complete 
        /// </summary>
        /// <param name="objSAL_ORDER_HDR"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<SAL_ORDER_HDR> GetSoNumberAutoCompleteList(SAL_ORDER_HDR objSAL_ORDER_HDR, ServiceUtility utilityObj)
        {
            List<SAL_ORDER_HDR> SAL_ORDER_HDR_List_Obj = new List<SAL_ORDER_HDR>();

            try
            {
                List<int> Status = new List<int>();
                Status.AddRange(new int[] { 0, 2, 12, 20 });
                SAL_ORDER_HDR_List_Obj = (from po in this.currentEntity.SAL_ORDER_HDR
                                          where po.SOH_ACTIVE == objSAL_ORDER_HDR.SOH_ACTIVE
                                                 && po.SOH_BIZUNIT == objSAL_ORDER_HDR.SOH_BIZUNIT
                                                 //&& (objSAL_ORDER_HDR.SOH_CONVERTED == 0 ? !po.SOH_CONVERTED.HasValue : true) //If 0 return not converted SC
                                                 && po.SOH_NO.Contains(utilityObj.FilterValue)
                                                 && po.SOH_CUSTOMER == (objSAL_ORDER_HDR.SOH_CUSTOMER > 0 ? objSAL_ORDER_HDR.SOH_CUSTOMER : po.SOH_CUSTOMER)
                                                 && (utilityObj.NeedAdvanceFilter ? Status.Contains(po.SOH_STATUS) : true) //&& po.SOH_STATUS == (utilityObj.NeedAdvanceFilter ? objSAL_ORDER_HDR.SOH_STATUS : po.SOH_STATUS)
                                                 && (objSAL_ORDER_HDR.SOH_TRX_STATUS == 0 ? true : (po.SOH_TRX_STATUS == objSAL_ORDER_HDR.SOH_TRX_STATUS))
                                                 && (objSAL_ORDER_HDR.SOH_CRTD_BY <= 0 || po.SOH_STATUS > 0 ? true : (po.SOH_CRTD_BY == objSAL_ORDER_HDR.SOH_CRTD_BY))
                                                
                                          orderby po.SOH_NO descending
                                          select po).ToList();
            }
            catch
            {

            }

            return SAL_ORDER_HDR_List_Obj;
        }



        /// <summary>
        /// Get Sale Order CustomerPo Numbers for auto complete 
        /// </summary>
        /// <param name="objSAL_ORDER_HDR"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<SAL_ORDER_HDR> GetCusPoNumberAutoCompleteList(SAL_ORDER_HDR objSAL_ORDER_HDR, ServiceUtility utilityObj)
        {
            List<SAL_ORDER_HDR> SAL_ORDER_HDR_List_Obj = new List<SAL_ORDER_HDR>();

            try
            {
                List<int> Status = new List<int>();
                Status.AddRange(new int[] { 0, 2, 12, 20 });
                SAL_ORDER_HDR_List_Obj = (from po in this.currentEntity.SAL_ORDER_HDR
                                          where po.SOH_ACTIVE == objSAL_ORDER_HDR.SOH_ACTIVE
                                                 && po.SOH_BIZUNIT == objSAL_ORDER_HDR.SOH_BIZUNIT
                                                 && po.SOH_REFERENCE.Contains(utilityObj.FilterValue)
                                                 && po.SOH_CUSTOMER == (objSAL_ORDER_HDR.SOH_CUSTOMER > 0 ? objSAL_ORDER_HDR.SOH_CUSTOMER : po.SOH_CUSTOMER)
                                                 && (utilityObj.NeedAdvanceFilter ? Status.Contains(po.SOH_STATUS) : true) //&& po.SOH_STATUS == (utilityObj.NeedAdvanceFilter ? objSAL_ORDER_HDR.SOH_STATUS : po.SOH_STATUS)
                                                 && (objSAL_ORDER_HDR.SOH_TRX_STATUS == 0 ? true : (po.SOH_TRX_STATUS == objSAL_ORDER_HDR.SOH_TRX_STATUS))
                                                 && (objSAL_ORDER_HDR.SOH_CRTD_BY <= 0 || po.SOH_STATUS > 0 ? true : (po.SOH_CRTD_BY == objSAL_ORDER_HDR.SOH_CRTD_BY))
                                          orderby po.SOH_NO descending
                                          select po).ToList();
            }
            catch
            {

            }

            return SAL_ORDER_HDR_List_Obj;
        }


        #endregion


        public List<SAL_ORDER_DTL> GetSelectedSaleOrderDetails(List<long> SelectedSos, ServiceUtility utilityObj, int shippingPlanPK = 0)
        {
            List<SAL_ORDER_DTL> ObjSAL_ORDER_DTLLst = new List<SAL_ORDER_DTL>();
            try
            {
                if (SelectedSos != null && SelectedSos.Count > 0)
                {
                    ObjSAL_ORDER_DTLLst = (from pod in this.currentEntity.SAL_ORDER_DTL
                                           where SelectedSos.Contains(pod.SOD_SO)
                                           select pod).ToList();
                }
                else
                {
                    ObjSAL_ORDER_DTLLst = (from shp in this.currentEntity.SAL_SHIPPING_PLAN_DTL
                                           where shp.SND_PLAN_HDR == shippingPlanPK && shp.SND_PLAN_QTY > 0 //checking done to avoid items without qty add "&& shp.SND_PLAN_QTY>0"
                                           select shp.SAL_ORDER_DTL).ToList();
                }
            }
            catch(Exception ex)
            {
            }

            return ObjSAL_ORDER_DTLLst;
        }




        public List<SAL_ORDER_HDR> GetSaleOrderHdrByPK(SAL_ORDER_HDR objSalesOrderHeader)
        {
            List<SAL_ORDER_HDR> SAL_ORDER_HDRLst = new List<SAL_ORDER_HDR>();
            IQueryable<SAL_ORDER_HDR> SAL_ORDER_HDRQuery;
            //int pageSize;
            //int totalCount;
            try
            {

                SAL_ORDER_HDRQuery = (from soh in this.currentEntity.SAL_ORDER_HDR
                                      where soh.SOH_ACTIVE == objSalesOrderHeader.SOH_ACTIVE
                                              && soh.SOH_PK == (objSalesOrderHeader.SOH_PK > 0 ? objSalesOrderHeader.SOH_PK : soh.SOH_PK)
                                      select soh);


                // Apply Paging And Sorting for grid Purpose
                SAL_ORDER_HDRLst = SAL_ORDER_HDRQuery.ToList();

                return SAL_ORDER_HDRLst;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        public string GetPkFromScNo(string scNo)
        {
            int tmpPk = 0;
            try
            {
                var xx = (from soh in this.currentEntity.SAL_ORDER_HDR where soh.SOH_NO == scNo select soh).ToList();
                if (xx.Count > 0)
                    tmpPk = (from soh in this.currentEntity.SAL_ORDER_HDR where soh.SOH_NO == scNo select soh).ToList().FirstOrDefault().SOH_PK;
                return tmpPk.ToString();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        public string GetPkFromDONo(string DONo)
        {
            int tmpPk = 0;
            try
            {
                var xx = (from soh in this.currentEntity.SAL_DESPATCH_HDR where soh.DPH_NO == DONo select soh).ToList();
                if (xx.Count > 0)
                    tmpPk = (from soh in this.currentEntity.SAL_DESPATCH_HDR where soh.DPH_NO == DONo select soh).ToList().FirstOrDefault().DPH_PK;
                return tmpPk.ToString();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public SAL_SHIPPING_PLAN_HDR GetShippingPlanHeader(int ShippingPlanPK)
        {

            try
            {

                return this.currentEntity.SAL_SHIPPING_PLAN_HDR.SingleOrDefault(sal => sal.SNH_PK == ShippingPlanPK);
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public DataTable GetSaleOrderCopyEligibleDetails(int ScPk)
        {
            DataTable dtResult = new DataTable();
            try
            {
                var query = from c in currentEntity.SAL_ORDER_HDR
                            where c.SOH_PK == ScPk
                            select new
                            {
                                TOTAL_INACTIVE_COUNT = c.SAL_ORDER_DTL.Where(r => r.CRM_CUST_ITEM_MAP.CIM_ACTIVE == 0).Count(),
                                TOTAL_COUNT = c.SAL_ORDER_DTL.Count()
                            };
                dtResult = query.ToList().ToDataTable();
                return dtResult;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
    }
}
