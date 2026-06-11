using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;

using ERPManager;
using System.Data.Objects;
using System.Data.SqlClient;

namespace ERPManager
{
    public class SalDespatchDtlManager : ISalDespatchDtlManager
    {
        #region Private Variables
        /// <summary>
        /// Gets or sets current db context
        /// </summary>
        private ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Initializes a new instance of the InvoiceListManager class
        /// </summary>
        /// <param name="currentEntity">Current db context</param>
        public SalDespatchDtlManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public long SaveSalDespatchDtl(List<SAL_DESPATCH_DTL> SalDespatchDtlList)
        {
            long retval = 0;
            long? maxPK;
            long DespatchHdr = 0;
            double DespatchQty = 0;
            SAL_DESPATCH_DTL OLD_SAL_DESPATCH_DTL_Obj;
            List<SAL_DESPATCH_DTL> OLD_SAL_DESPATCH_DTL_List_Obj;
            List<SAL_DESPATCH_DTL> OLD_SAL_DESPATCH_DTL_List_dummy_Obj;

            SAL_SHIPPING_PLAN_HDR SAL_SHIPPING_PLAN_HDR_obj;

            SAL_SHIPPING_PLAN_DTL SAL_SHIPPING_PLAN_DTL_obj;

            SAL_ORDER_DTL SAL_ORDER_DTL_obj;

            try
            {
                ////Set save status zero,save failed
                retval = 0;
                ////Getting last Payment Mpg pk
                maxPK = currentEntity.SAL_DESPATCH_DTL.Max(v => (int?)v.DPD_PK);
                maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;

                if (SalDespatchDtlList.Count > 0)
                {
                    List<int> pks = (from old1 in SalDespatchDtlList
                                     select old1.DPD_PK).ToList();

                    DespatchHdr = SalDespatchDtlList[0].DPD_DESPATCH_HDR;

                    OLD_SAL_DESPATCH_DTL_List_Obj = (from old in this.currentEntity.SAL_DESPATCH_DTL
                                                     where DespatchHdr == old.DPD_DESPATCH_HDR
                                                                && !pks.Contains(old.DPD_PK)
                                                     select old).ToList();
                    foreach (SAL_DESPATCH_DTL old_SAL_DESPATCH_DTL in OLD_SAL_DESPATCH_DTL_List_Obj)
                    {
                        SAL_ORDER_DTL_obj = currentEntity.SAL_ORDER_DTL.SingleOrDefault(sah => sah.SOD_PK == old_SAL_DESPATCH_DTL.DPD_SO_DTL);

                        SAL_SHIPPING_PLAN_HDR_obj = currentEntity.SAL_SHIPPING_PLAN_HDR.SingleOrDefault(sah => sah.SNH_PK == old_SAL_DESPATCH_DTL.SAL_DESPATCH_HDR.DPH_SHIPPING_PLAN);

                        if ((SAL_SHIPPING_PLAN_HDR_obj.SNH_STATUS == 2) || (SAL_SHIPPING_PLAN_HDR_obj.SNH_STATUS == 92) || (SAL_SHIPPING_PLAN_HDR_obj.SNH_STATUS >= 99))
                        {

                            if (SAL_ORDER_DTL_obj != null)
                            {
                                SAL_ORDER_DTL_obj.SOD_QTY_DISPATCHED -= old_SAL_DESPATCH_DTL.DPD_QTY_DESPATCHED;
                                SAL_ORDER_DTL_obj.SOD_BAL_TO_DISPATCH += old_SAL_DESPATCH_DTL.DPD_QTY_DESPATCHED;
                            }
                        }

                        this.currentEntity.SAL_DESPATCH_DTL.DeleteObject(old_SAL_DESPATCH_DTL);
                    }
                }


                foreach (SAL_DESPATCH_DTL SAL_DESPATCH_DTL_Obj in SalDespatchDtlList)
                {
                    //check despatch pk is zero,save despatch as new record
                    if (SAL_DESPATCH_DTL_Obj.DPD_PK == 0)
                    {

                        SAL_DESPATCH_DTL_Obj.SAL_DESPATCH_HDR = null;
                        //Set next despatch pk
                        SAL_DESPATCH_DTL_Obj.DPD_PK = (Int32)maxPK;

                        //Add new despatch to the db context
                        currentEntity.SAL_DESPATCH_DTL.AddObject(SAL_DESPATCH_DTL_Obj);
                        maxPK++;
                        retval = SAL_DESPATCH_DTL_Obj.DPD_PK;

                        DespatchQty = SAL_DESPATCH_DTL_Obj.DPD_QTY_DESPATCHED;

                    }
                    //updating despatch details
                    else
                    {
                        //Get current despatch details using despatch  pk
                        OLD_SAL_DESPATCH_DTL_Obj = currentEntity.SAL_DESPATCH_DTL.SingleOrDefault(v => v.DPD_PK == SAL_DESPATCH_DTL_Obj.DPD_PK);
                        if (OLD_SAL_DESPATCH_DTL_Obj != null)
                        {
                            //If approved, then Rever Despatched qty from Sal Order Dtl.
                            if (OLD_SAL_DESPATCH_DTL_Obj.SAL_DESPATCH_HDR.DPH_STATUS == 2)
                                DespatchQty = SAL_DESPATCH_DTL_Obj.DPD_QTY_DESPATCHED - OLD_SAL_DESPATCH_DTL_Obj.DPD_QTY_DESPATCHED;
                            else
                                DespatchQty = 0;

                            //Update despatch details
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_VERSION = SAL_DESPATCH_DTL_Obj.DPD_VERSION;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_SL_NO = SAL_DESPATCH_DTL_Obj.DPD_SL_NO;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_DATE = SAL_DESPATCH_DTL_Obj.DPD_DATE;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_NO = SAL_DESPATCH_DTL_Obj.DPD_NO;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_IS_PACK_MAT = SAL_DESPATCH_DTL_Obj.DPD_IS_PACK_MAT;

                            OLD_SAL_DESPATCH_DTL_Obj.DPD_CUST_ITEM = SAL_DESPATCH_DTL_Obj.DPD_CUST_ITEM;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_ITEM = SAL_DESPATCH_DTL_Obj.DPD_ITEM;
                            #region DO quantity with Container release quantity check
                            if (OLD_SAL_DESPATCH_DTL_Obj.SAL_CONTAINER_RELEASE_DTL != null && OLD_SAL_DESPATCH_DTL_Obj.SAL_CONTAINER_RELEASE_DTL.Count > 0)
                            {
                                if (SAL_DESPATCH_DTL_Obj.DPD_QTY_DESPATCHED < OLD_SAL_DESPATCH_DTL_Obj.SAL_CONTAINER_RELEASE_DTL.Sum(x => x.CDR_QTY_DESPATCHED)) // DPD_QTY_APPROVED)
                                {
                                    throw new Exception(ERPManagerRes.Err_DoQtyLessThanContainerRelease);
                                }
                            }
                            #endregion
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_QTY_DESPATCHED = SAL_DESPATCH_DTL_Obj.DPD_QTY_DESPATCHED;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_UOM = SAL_DESPATCH_DTL_Obj.DPD_UOM;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_PACKING_SPEC = SAL_DESPATCH_DTL_Obj.DPD_PACKING_SPEC;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_REMARKS = SAL_DESPATCH_DTL_Obj.DPD_REMARKS;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_PROD_BATCH = SAL_DESPATCH_DTL_Obj.DPD_PROD_BATCH;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_DEPT = SAL_DESPATCH_DTL_Obj.DPD_DEPT;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_BIZUNIT = SAL_DESPATCH_DTL_Obj.DPD_BIZUNIT;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_NET_WT = SAL_DESPATCH_DTL_Obj.DPD_NET_WT;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_GROSS_WT = SAL_DESPATCH_DTL_Obj.DPD_GROSS_WT;

                            OLD_SAL_DESPATCH_DTL_Obj.DPD_SALE_QTY = SAL_DESPATCH_DTL_Obj.DPD_SALE_QTY;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_SALE_UOM = SAL_DESPATCH_DTL_Obj.DPD_SALE_UOM;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_SALE_UOM_CONV = SAL_DESPATCH_DTL_Obj.DPD_SALE_UOM_CONV;
                            OLD_SAL_DESPATCH_DTL_Obj.DPD_LOT_NO = SAL_DESPATCH_DTL_Obj.DPD_LOT_NO;

                            //Set return value as despatch pk
                            retval = SAL_DESPATCH_DTL_Obj.DPD_PK;
                        }
                        else
                        {
                            //throws exception already deleted or modified by other user
                            //throw new OptimisticConcurrencyException(gComsManagerRes.EditConcurrencyException);
                        }
                    }

                    //Update despatched qty in SO : SAL_ORDER_DTL
                    SAL_ORDER_DTL_obj = currentEntity.SAL_ORDER_DTL.SingleOrDefault(sah => sah.SOD_PK == SAL_DESPATCH_DTL_Obj.DPD_SO_DTL);

                    //while modify save case
                    if (SAL_DESPATCH_DTL_Obj.SAL_DESPATCH_HDR != null)
                    {
                        SAL_SHIPPING_PLAN_HDR_obj = currentEntity.SAL_SHIPPING_PLAN_HDR.SingleOrDefault(sah => sah.SNH_PK == SAL_DESPATCH_DTL_Obj.SAL_DESPATCH_HDR.DPH_SHIPPING_PLAN);

                        if ((SAL_SHIPPING_PLAN_HDR_obj.SNH_STATUS == 2) || (SAL_SHIPPING_PLAN_HDR_obj.SNH_STATUS == 92) || (SAL_SHIPPING_PLAN_HDR_obj.SNH_STATUS >= 99))
                        {
                            if (SAL_ORDER_DTL_obj != null)
                            {
                                SAL_ORDER_DTL_obj.SOD_QTY_DISPATCHED = SAL_ORDER_DTL_obj.SOD_QTY_DISPATCHED + DespatchQty;
                                SAL_ORDER_DTL_obj.SOD_BAL_TO_DISPATCH = SAL_ORDER_DTL_obj.SOD_BAL_TO_DISPATCH - DespatchQty;
                            }
                        }
                    }
                }

                //return Invoice Trx Mpg Pk
                return retval;
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
        /// <summary>
        /// Get Despatch details w.r.t despatch hdr PK
        /// </summary>
        /// <param name="despatchID"></param>
        /// <returns></returns>
        public List<SAL_DESPATCH_DTL> GetDespatchedSaleOrders(int despatchID, ServiceUtility utilityObj, int shippingPlanPK)
        {
            IQueryable<SAL_DESPATCH_DTL> qry = null;
            List<SAL_DESPATCH_DTL> SAL_DESPATCH_DTLLst = new List<SAL_DESPATCH_DTL>();
            try
            {
                qry = this.currentEntity.SAL_DESPATCH_DTL;
                if (shippingPlanPK > 0)
                {
                    qry = qry.Where(desp => desp.SAL_DESPATCH_HDR.DPH_SHIPPING_PLAN == shippingPlanPK);
                }
                else if (despatchID > 0)
                {
                    qry = qry.Where(desp => desp.DPD_DESPATCH_HDR == despatchID);
                }
                SAL_DESPATCH_DTLLst = qry.ToList();
            }
            catch
            {
            }
            SAL_DESPATCH_DTLLst = qry.SortRecords<SAL_DESPATCH_DTL>(utilityObj).ToList();
            return SAL_DESPATCH_DTLLst;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Get Despatch details w.r.t despatch hdr PK
        /// </summary>
        /// <param name="despatchID"></param>
        /// <returns></returns>
        public List<SAL_DESPATCH_DTL> GetDespatchedSaleOrderDetails(int despatchID, ServiceUtility utilityObj, int shippingPlanPK)
        {
            IQueryable<SAL_DESPATCH_DTL> qry = null;
            IQueryable<SAL_DESPATCH_DTL> resultqry = null;
            List<SAL_DESPATCH_DTL> SAL_DESPATCH_DTLLst = new List<SAL_DESPATCH_DTL>();
            try
            {
                qry = this.currentEntity.SAL_DESPATCH_DTL;
                if (shippingPlanPK > 0)
                {
                    qry = qry.Where(desp => desp.SAL_DESPATCH_HDR.DPH_SHIPPING_PLAN == shippingPlanPK);
                }
                else if (despatchID > 0)
                {
                    qry = qry.Where(desp => desp.DPD_DESPATCH_HDR == despatchID);
                }
                SAL_DESPATCH_DTLLst = qry.ToList();
                if (SAL_DESPATCH_DTLLst != null && SAL_DESPATCH_DTLLst.Count > 0)
                {
                    int? ShippingPk = SAL_DESPATCH_DTLLst[0].SAL_DESPATCH_HDR.DPH_SHIPPING_PLAN;
                    List<int> doSoDtlPks = SAL_DESPATCH_DTLLst.Select(r => r.DPD_SO_DTL).ToList();
                    List<SAL_SHIPPING_PLAN_DTL> shplst = (from dtl in this.currentEntity.SAL_SHIPPING_PLAN_DTL where dtl.SAL_SHIPPING_PLAN_HDR.SNH_PK == ShippingPk && !doSoDtlPks.Contains(dtl.SND_SOD) select dtl).ToList();
                    if (shplst != null && shplst.Count > 0)
                    {
                        foreach (SAL_SHIPPING_PLAN_DTL shpdtl in shplst)
                        {
                            SAL_DESPATCH_DTL despdtl = new SAL_DESPATCH_DTL();
                            despdtl.DPD_PACKING_SPEC = shpdtl.SAL_ORDER_DTL.ADM_PACK_SPEC_MST == null ? (int?)null : shpdtl.SAL_ORDER_DTL.ADM_PACK_SPEC_MST.APS_PK;
                            despdtl.ADM_PACK_SPEC_MST = shpdtl.SAL_ORDER_DTL.ADM_PACK_SPEC_MST;
                            despdtl.DPD_DEPT = shpdtl.SAL_SHIPPING_PLAN_HDR.SNH_DEPT;
                            despdtl.ADM_CONFIG_MST = shpdtl.ADM_CONFIG_MST;
                            despdtl.DPD_CUST_ITEM = shpdtl.SAL_ORDER_DTL.CRM_CUST_ITEM_MAP == null ? (int?)null : shpdtl.SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_PK;
                            despdtl.CRM_CUST_ITEM_MAP = shpdtl.SAL_ORDER_DTL.CRM_CUST_ITEM_MAP;
                            despdtl.DPD_ART_WORK = shpdtl.SAL_ORDER_DTL.SOD_ART_WORK;
                            despdtl.DPD_BIZUNIT = shpdtl.SAL_SHIPPING_PLAN_HDR.SNH_BIZUNIT;
                            despdtl.DPD_SO_DTL = shpdtl.SND_SOD;
                            despdtl.SAL_ORDER_DTL = shpdtl.SAL_ORDER_DTL;
                            despdtl.SAL_ORDER_HDR = shpdtl.SAL_ORDER_DTL.SAL_ORDER_HDR;
                            despdtl.INV_ITEM_MST = shpdtl.SAL_ORDER_DTL.INV_ITEM_MST;
                            despdtl.INV_UOM_MST = shpdtl.SAL_ORDER_DTL.INV_UOM_MST;
                            despdtl.DPD_SALE_UOM = shpdtl.SND_SALE_UOM;
                           

                            despdtl.DPD_SALE_QTY = 0;//shpdtl.SND_SALE_QTY;//0;
                            despdtl.DPD_SALE_UOM_CONV = shpdtl.SND_SALE_UOM_CONV;
                            despdtl.SAL_DESPATCH_HDR = SAL_DESPATCH_DTLLst[0].SAL_DESPATCH_HDR;
                            despdtl.DPD_QTY_DESPATCHED = shpdtl.SND_PLAN_QTY;
                            despdtl.DPD_NO = SAL_DESPATCH_DTLLst[0].SAL_DESPATCH_HDR == null ? string.Empty : SAL_DESPATCH_DTLLst[0].SAL_DESPATCH_HDR.DPH_NO;
                            despdtl.DPD_SL_NO = shpdtl.SAL_ORDER_DTL.SOD_SL_NO.HasValue ? shpdtl.SAL_ORDER_DTL.SOD_SL_NO.Value : (short)(SAL_DESPATCH_DTLLst.Max(sl => sl.DPD_SL_NO) + 1);
                            despdtl.DPD_LOT_NO = shpdtl.SAL_ORDER_DTL.SOD_LOT_NO == null ? string.Empty : shpdtl.SAL_ORDER_DTL.SOD_LOT_NO;
                            SAL_DESPATCH_DTLLst.Add(despdtl);
                        }
                    }
                }
            }
            catch
            {
            }
            resultqry = SAL_DESPATCH_DTLLst.AsQueryable();
            resultqry = resultqry.OrderBy(c => c.SAL_ORDER_DTL.SOD_NO).ThenBy(r => r.SAL_ORDER_DTL.SOD_SL_NO);  
            SAL_DESPATCH_DTLLst = resultqry.SortRecords<SAL_DESPATCH_DTL>(utilityObj).ToList();
            return SAL_DESPATCH_DTLLst;
            //throw new NotImplementedException();
        }

        private double GetVal(int a)
        {
            double result = this.currentEntity.CreateQuery<double>(
             "SELECT VALUE ERPModel.Store.FNINV_ITEM_NETWT_GET(@someParameter) FROM {1}",
             new ObjectParameter("someParameter", a)
            ).First();
            return result;
        }
        public void TotalCupom(int cupom)
        {
            //DBService dbService = new DBService();

            //float SAIDA;
            //SqlDataAdapter da2 = new SqlDataAdapter();
            //if (conex1.State == ConnectionState.Closed)
            //{
            //    conex1.Open();
            //}
            //SqlCommand Totalf = new SqlCommand("SELECT dbo.Tcupom(@code)", conex1);
            //SqlParameter code1 = new SqlParameter("@code", SqlDbType.Int);
            //code1.Value = cupom;
            //SAIDA = Totalf.ExecuteScalar();

            //return SAIDA;
        }

        public List<SAL_DESPATCH_DTL> GetSalesInvoiceTrxMpg(SAL_DESPATCH_DTL InvoiceHdrObj, ServiceUtility utilityObj = null)
        {
            List<SAL_DESPATCH_DTL> SaleDespatchDtlList = null;
            try
            {

                //return Invoice Trx Mpg List
                return SaleDespatchDtlList;
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

        public List<SAL_DESPATCH_DTL> GetDespatchDtl(SAL_DESPATCH_DTL DespatchDtlObj, ServiceUtility utilityObj = null)
        {
            List<SAL_DESPATCH_DTL> ObjDespatchHeaderLst = new List<SAL_DESPATCH_DTL>();
            try
            {
                ObjDespatchHeaderLst = (from dsd in this.currentEntity.SAL_DESPATCH_DTL
                                        where dsd.DPD_SALE_ORDER == DespatchDtlObj.DPD_SALE_ORDER
                                            && dsd.DPD_DESPATCH_HDR == DespatchDtlObj.DPD_DESPATCH_HDR
                                        select dsd).Distinct().ToList();
            }
            catch
            {
            }

            return ObjDespatchHeaderLst;
        }
        public string GetDespatchQty(SAL_DESPATCH_DTL DespatchDtlObj, ServiceUtility utilityObj = null)
        {
            string delQty = "";
            try
            {
                delQty = (from dsd in this.currentEntity.SAL_DESPATCH_DTL
                          where dsd.DPD_SALE_ORDER == DespatchDtlObj.DPD_SALE_ORDER
                                  && dsd.DPD_DESPATCH_HDR == DespatchDtlObj.DPD_DESPATCH_HDR
                          select dsd.DPD_QTY_DESPATCHED).FirstOrDefault().ToString();
            }
            catch
            {
            }
            return delQty;
        }
        #endregion


        #region Private Methods
        #endregion

        public void UpdateInvoiceDtls(SAL_DESPATCH_HDR salDespatchHdrList)
        {
            List<FIN_INVOICE_CUS_HDR> invList = new List<FIN_INVOICE_CUS_HDR>();

            try
            {

                invList = this.currentEntity.FIN_INVOICE_CUS_HDR.Where(r => r.ICH_DESPATCH_HDR == salDespatchHdrList.DPH_PK).ToList();
                if (invList != null && invList.Count > 0)
                {
                    foreach (FIN_INVOICE_CUS_HDR InvHdr in invList)
                    {
                        FIN_INVOICE_CUS_HDR objInvDet = this.currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(r => r.ICH_PK == InvHdr.ICH_PK);
                        objInvDet.ICH_FROM_PORT = salDespatchHdrList.DPH_FROM_PORT;
                        objInvDet.ICH_TO_PORT = salDespatchHdrList.DPH_FINAL_DESTINATION;
                    }
                }

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



        public int CheckforAlreadyDespatch(Int32? shipPlanPK)
        {
            int result = 0;
            result = (from despHdr in currentEntity.SAL_DESPATCH_HDR
                      where despHdr.DPH_SHIPPING_PLAN == shipPlanPK
                      select despHdr.DPH_PK).Count();
            return result;
        }
    }
}
