using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using BusinessObject.CommonManagement;
using ERPManager.Finance;

namespace ERPManager
{
    public class FinPaymentVndTrxMpgManager : IFinPaymentVndTrxMpgManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Payment Maping Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public FinPaymentVndTrxMpgManager(ERPEntities currentEntity)
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
        /// Save Payment Maping 
        /// </summary>
        /// <param name="finPaymentVndTrxMpgList"></param>
        /// <returns></returns>
        public long? SavePaymentDtl(List<FIN_PAYMENT_VND_TRX_MPG> finPaymentVndTrxMpgList, Byte Category)
        {
            //Holds save status
            long retval;
            //Holds last Payment Mpg master pk
            long? maxPaymentMpgPK;
            decimal paidAmt = 0;
            long PaymentHdr = 0;
            long? maxID;
            long? AdjnMaxID;
            long? TaxMaxID;
            long? CrdrAlcnMaxID;
            //On update holds old Payment Mpg master details
            FIN_PAYMENT_VND_TRX_MPG oldFinPaymentVndTrxMpgObj;
            FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;
            List<FIN_PAYMENT_VND_TRX_MPG> oldFinPaymentVndTrxMpgListObj;

            List<FIN_PAYMENT_VND_PO_MPG> finPaymentVndPoMpgList;
            FinPaymentVndPoMpgManager finPaymentVndPoMpgManagerObj;

            List<FIN_PAYMENT_VND_ALCN_DTL> finPaymentAlcnAdjnList;
            FinPaymentAllocationAdjnManager finPaymentAlcnAdjnManagerObj;

            List<FIN_PAYMENT_VND_CRDR_MPG> finPaymentCrdrAlcnList;

            PUR_ORDER_HDR obj_PUR_ORDER_HDR;
            try
            {
                //Set save status zero,save failed
                retval = 0;
                //Getting last Payment Mpg pk
                maxPaymentMpgPK = currentEntity.FIN_PAYMENT_VND_TRX_MPG.Max(v => (int?)v.PVM_PK);
                maxPaymentMpgPK = (maxPaymentMpgPK.HasValue) ? maxPaymentMpgPK.Value + 1 : 1;
                maxID = currentEntity.FIN_PAYMENT_VND_PO_MPG.Max(v => (long?)v.PPO_PK);

                AdjnMaxID = currentEntity.FIN_PAYMENT_VND_ALCN_DTL.Max(v => (long?)v.PAD_PK).HasValue ? currentEntity.FIN_PAYMENT_VND_ALCN_DTL.Max(v => (long?)v.PAD_PK) + 1 : 1;

                TaxMaxID = currentEntity.FIN_PAYMENT_VND_TAX_DTL.Max(v => (long?)v.PDT_PK);

                CrdrAlcnMaxID = currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Max(v => (long?)v.PNM_PK).HasValue? + currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Max(v => (long?)v.PNM_PK) + 1 : 1;
                //Iterate through mapping list for save

                if (finPaymentVndTrxMpgList.Count > 0)
                {
                    List<long> pks = (from old1 in finPaymentVndTrxMpgList
                                      select old1.PVM_PK).ToList();

                    PaymentHdr = finPaymentVndTrxMpgList[0].PVM_PAYMENT_HDR;

                    oldFinPaymentVndTrxMpgListObj = (from old in this.currentEntity.FIN_PAYMENT_VND_TRX_MPG
                                                     where PaymentHdr == old.PVM_PAYMENT_HDR
                                                        && !pks.Contains(old.PVM_PK)
                                                        select old).ToList();

                    foreach (FIN_PAYMENT_VND_TRX_MPG oldFinPaymentVndTrxMpgObject in oldFinPaymentVndTrxMpgListObj)
                    {
                        finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == oldFinPaymentVndTrxMpgObject.PVM_INVOICE_HDR);

                        if (finInvoiceVndHdrObj != null)
                        {
                            finInvoiceVndHdrObj.IVH_AMOUNT_PAID_TC -= oldFinPaymentVndTrxMpgObject.PVM_PAID_AMOUNT;
                        }

                        #region Delete Payment Split
                        List<FIN_PAYMENT_VND_PO_MPG> Old_finPaymentVndPoMpgList = oldFinPaymentVndTrxMpgObject.FIN_PAYMENT_VND_PO_MPG.ToList();
                        if (Old_finPaymentVndPoMpgList != null)
                        {
                            foreach (FIN_PAYMENT_VND_PO_MPG old_FIN_PAYMENT_VND_PO_MPG in Old_finPaymentVndPoMpgList)
                            {
                                obj_PUR_ORDER_HDR = currentEntity.PUR_ORDER_HDR.SingleOrDefault(a => a.POH_PK == old_FIN_PAYMENT_VND_PO_MPG.PPO_PO_HDR);

                                if (obj_PUR_ORDER_HDR != null)
                                {
                                    obj_PUR_ORDER_HDR.POH_AMT_PAID -= old_FIN_PAYMENT_VND_PO_MPG.PPO_PAID_AMOUNT;
                                }
                                this.currentEntity.FIN_PAYMENT_VND_PO_MPG.DeleteObject(old_FIN_PAYMENT_VND_PO_MPG);
                            }
                        } 
                        #endregion                       

                        #region Delete Payment Adjustments
                        List<FIN_PAYMENT_VND_ALCN_DTL> oldfinAdjnListObj = oldFinPaymentVndTrxMpgObject.FIN_PAYMENT_VND_ALCN_DTL.ToList();
                        if (oldfinAdjnListObj != null)
                        {
                            foreach (FIN_PAYMENT_VND_ALCN_DTL oldfinCrDrNoteTaxHdrObj in oldfinAdjnListObj)
                            {
                                if (finInvoiceVndHdrObj != null)
                                {
                                    if (oldfinCrDrNoteTaxHdrObj.PAD_ALCN_CDH != null)
                                    {
                                        finInvoiceVndHdrObj.IVH_AMOUNT_DN_TC -= oldfinCrDrNoteTaxHdrObj.PAD_AMOUNT;
                                    }
                                    else
                                    {
                                        finInvoiceVndHdrObj.IVH_AMOUNT_PAID_TC -= oldfinCrDrNoteTaxHdrObj.PAD_AMOUNT;
                                    }
                                }

                                this.currentEntity.FIN_PAYMENT_VND_ALCN_DTL.DeleteObject(oldfinCrDrNoteTaxHdrObj);
                            }
                        }
                        #endregion

                        #region Delete detail tax
                        List<FIN_PAYMENT_VND_TAX_DTL> Old_finPaymentVndTaxDtlList = oldFinPaymentVndTrxMpgObject.FIN_PAYMENT_VND_TAX_DTL.ToList();
                        if (Old_finPaymentVndTaxDtlList != null)
                        {
                            foreach (FIN_PAYMENT_VND_TAX_DTL Old_finPaymentVndTaxDtlObj in Old_finPaymentVndTaxDtlList)
                            {
                                this.currentEntity.FIN_PAYMENT_VND_TAX_DTL.DeleteObject(Old_finPaymentVndTaxDtlObj);
                            }
                        }
                        #endregion

                        #region Delete CR/DR Allocations
                        List<FIN_PAYMENT_VND_CRDR_MPG> Old_finPaymentVndCrdrAlcnList = oldFinPaymentVndTrxMpgObject.FIN_PAYMENT_VND_CRDR_MPG.ToList();
                        if (Old_finPaymentVndCrdrAlcnList != null)
                        {
                            foreach (FIN_PAYMENT_VND_CRDR_MPG Old_finPaymentVndCrdrAlcnObj in Old_finPaymentVndCrdrAlcnList)
                            {
                                this.currentEntity.FIN_PAYMENT_VND_CRDR_MPG.DeleteObject(Old_finPaymentVndCrdrAlcnObj);
                            }
                        }
                        #endregion

                        this.currentEntity.FIN_PAYMENT_VND_TRX_MPG.DeleteObject(oldFinPaymentVndTrxMpgObject);
                    }
                }

                foreach (FIN_PAYMENT_VND_TRX_MPG finPaymentVndTrxMpgObj in finPaymentVndTrxMpgList)
                {
                    finPaymentVndPoMpgList = finPaymentVndTrxMpgObj.FIN_PAYMENT_VND_PO_MPG == null ?
                    new List<FIN_PAYMENT_VND_PO_MPG>() : finPaymentVndTrxMpgObj.FIN_PAYMENT_VND_PO_MPG.ToList();
                    finPaymentVndTrxMpgObj.FIN_PAYMENT_VND_PO_MPG.Clear();

                    finPaymentAlcnAdjnList = finPaymentVndTrxMpgObj.FIN_PAYMENT_VND_ALCN_DTL == null ?
                    new List<FIN_PAYMENT_VND_ALCN_DTL>() : finPaymentVndTrxMpgObj.FIN_PAYMENT_VND_ALCN_DTL.ToList();
                    finPaymentVndTrxMpgObj.FIN_PAYMENT_VND_ALCN_DTL.Clear();

                    finPaymentCrdrAlcnList = finPaymentVndTrxMpgObj.FIN_PAYMENT_VND_CRDR_MPG == null ?
                    new List<FIN_PAYMENT_VND_CRDR_MPG>() : finPaymentVndTrxMpgObj.FIN_PAYMENT_VND_CRDR_MPG.ToList();
                    finPaymentVndTrxMpgObj.FIN_PAYMENT_VND_CRDR_MPG.Clear();

                    //check Payment Mpg master pk is zero,save Payment Mpg master as new record
                    if (finPaymentVndTrxMpgObj.PVM_PK == 0)
                    {
                        //Set next Payment Mpg pk
                        finPaymentVndTrxMpgObj.PVM_PK = (long)maxPaymentMpgPK;
                        //Add new Payment Mpg to the db context
                        currentEntity.FIN_PAYMENT_VND_TRX_MPG.AddObject(finPaymentVndTrxMpgObj);
                        maxPaymentMpgPK++;
                        retval = finPaymentVndTrxMpgObj.PVM_PK;
                        paidAmt = finPaymentVndTrxMpgObj.PVM_PAID_AMOUNT;
                        decimal? discTotAmount = 0;
                        if (Category != (Byte)(POInvoiceCategory.Invoice))
                        { 
                            discTotAmount = finPaymentVndTrxMpgObj.FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TRX_MPG.Select(ICM => (decimal?)ICM.IVM_DISCOUNT_AMOUNT).FirstOrDefault();
                        }
                        else
                        {
                            discTotAmount = finPaymentVndTrxMpgObj.FIN_INVOICE_VND_HDR.IVH_DISCOUNT_TC + finPaymentVndTrxMpgObj.FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL.Sum(ss => ss.VID_DISCOUNT);//).Select(d=>d.FIN_INVOICE_CUS_TAX_DTL.Sum(j=>j.c)) .FIN_INVOICE_CUS_TRX_MPG.Select(ICM => (decimal?)ICM.ICM_DISCOUNT_AMOUNT).FirstOrDefault();
                        }
                        if (finPaymentVndPoMpgList.Count > 0)
                        {
                            finPaymentVndPoMpgList.ForEach(dtl =>
                            {
                                dtl.PPO_PAYMENT_HDR = finPaymentVndTrxMpgObj.PVM_PAYMENT_HDR;
                                dtl.PPO_PAYMENT_TRX_MPG = finPaymentVndTrxMpgObj.PVM_PK;                                
                            });
                            finPaymentVndPoMpgManagerObj = new FinPaymentVndPoMpgManager(this.currentEntity);
                            finPaymentVndPoMpgManagerObj.SavePaymentSplit(finPaymentVndPoMpgList, ref maxID, ref TaxMaxID, Category, finPaymentVndTrxMpgObj.PVM_INVOICE_HDR, discTotAmount);
                        }

                        finPaymentAlcnAdjnManagerObj = new FinPaymentAllocationAdjnManager(this.currentEntity);
                        if (finPaymentAlcnAdjnList.Count > 0)
                        {
                            finPaymentAlcnAdjnList.ForEach(dtl =>
                            {
                                dtl.PAD_PAYMENT_TRX = finPaymentVndTrxMpgObj.PVM_PK;
                            });
                            //finPaymentAlcnAdjnManagerObj = new FinPaymentAllocationAdjnManager(this.currentEntity);
                            //finPaymentAlcnAdjnManagerObj.SavePaymentAdjn(finPaymentAlcnAdjnList, ref AdjnMaxID, (long)maxPaymentMpgPK, finPaymentVndTrxMpgObj.PVM_INVOICE_HDR);
                            finPaymentAlcnAdjnManagerObj.SavePaymentAdjn(finPaymentAlcnAdjnList, ref AdjnMaxID, finPaymentVndTrxMpgObj.PVM_PK, finPaymentVndTrxMpgObj.PVM_INVOICE_HDR);
                        }

                        if (finPaymentCrdrAlcnList.Count > 0)
                        {
                            finPaymentCrdrAlcnList.ForEach(dtl =>
                            {
                                dtl.PNM_PAYMENT_TRX_MPG = finPaymentVndTrxMpgObj.PVM_PK;
                                dtl.PNM_PAYMENT_HDR = finPaymentVndTrxMpgObj.PVM_PAYMENT_HDR;
                            });
                            //finPaymentAlcnAdjnManagerObj = new FinPaymentAllocationAdjnManager(this.currentEntity);
                            finPaymentAlcnAdjnManagerObj.SavePaymentCrdrAllocation(finPaymentCrdrAlcnList, finPaymentVndTrxMpgObj.PVM_PK, ref CrdrAlcnMaxID);
                        }
                    }
                    //updating Payment Mpg details
                    else
                    {
                        //Get current Payment Mpg master details using Payment Mpg master pk
                        oldFinPaymentVndTrxMpgObj = currentEntity.FIN_PAYMENT_VND_TRX_MPG.SingleOrDefault(v => v.PVM_PK == finPaymentVndTrxMpgObj.PVM_PK);
                        if (oldFinPaymentVndTrxMpgObj != null)
                        {
                            paidAmt = finPaymentVndTrxMpgObj.PVM_PAID_AMOUNT - oldFinPaymentVndTrxMpgObj.PVM_PAID_AMOUNT;
                            //Update Payment Mpg details
                            oldFinPaymentVndTrxMpgObj.PVM_PAID_AMOUNT = finPaymentVndTrxMpgObj.PVM_PAID_AMOUNT;
                            oldFinPaymentVndTrxMpgObj.PVM_OTHER_AMOUNT = finPaymentVndTrxMpgObj.PVM_OTHER_AMOUNT;
                            oldFinPaymentVndTrxMpgObj.PVM_ACTIVE = finPaymentVndTrxMpgObj.PVM_ACTIVE;
                            oldFinPaymentVndTrxMpgObj.PVM_DISC_AMOUNT = finPaymentVndTrxMpgObj.PVM_DISC_AMOUNT;
                            oldFinPaymentVndTrxMpgObj.PVM_TAX_AMOUNT = finPaymentVndTrxMpgObj.PVM_TAX_AMOUNT;
                            //Set return value as Payment Mpg pk
                            retval = finPaymentVndTrxMpgObj.PVM_PK;
                            //decimal? discTotAmount = finPaymentVndTrxMpgObj.FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TRX_MPG.Select(ICM => (decimal?)ICM.IVM_DISCOUNT_AMOUNT).FirstOrDefault();
                             finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == finPaymentVndTrxMpgObj.PVM_INVOICE_HDR);
                             decimal? discTotAmount = 0;
                             if (Category != (Byte)(POInvoiceCategory.Invoice))
                             {
                                discTotAmount = finInvoiceVndHdrObj.FIN_INVOICE_VND_TRX_MPG.Select(ICM => (decimal?)ICM.IVM_DISCOUNT_AMOUNT).FirstOrDefault();
                             }
                             else
                             {
                                 //discTotAmount = finPaymentVndTrxMpgObj.FIN_INVOICE_VND_HDR.IVH_DISCOUNT_TC + finPaymentVndTrxMpgObj.FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL.Sum(ss => ss.VID_DISCOUNT);//).Select(d=>d.FIN_INVOICE_CUS_TAX_DTL.Sum(j=>j.c)) .FIN_INVOICE_CUS_TRX_MPG.Select(ICM => (decimal?)ICM.ICM_DISCOUNT_AMOUNT).FirstOrDefault();
                                 discTotAmount = finInvoiceVndHdrObj.IVH_DISCOUNT_TC + finInvoiceVndHdrObj.FIN_INVOICE_VND_DTL.Sum(ss => ss.VID_DISCOUNT);//).Select(d=>d.FIN_INVOICE_CUS_TAX_DTL.Sum(j=>j.c)) .FIN_INVOICE_CUS_TRX_MPG.Select(ICM => (decimal?)ICM.ICM_DISCOUNT_AMOUNT).FirstOrDefault();
                             }

                            if (finPaymentVndPoMpgList.Count > 0)
                            {
                                finPaymentVndPoMpgList.ForEach(dtl =>
                                {
                                    dtl.PPO_PAYMENT_HDR = finPaymentVndTrxMpgObj.PVM_PAYMENT_HDR;
                                    dtl.PPO_PAYMENT_TRX_MPG = finPaymentVndTrxMpgObj.PVM_PK;                                    
                                });
                                finPaymentVndPoMpgManagerObj = new FinPaymentVndPoMpgManager(this.currentEntity);
                                finPaymentVndPoMpgManagerObj.SavePaymentSplit(finPaymentVndPoMpgList, ref maxID, ref TaxMaxID, Category, finPaymentVndTrxMpgObj.PVM_INVOICE_HDR, discTotAmount);
                            }

                            finPaymentAlcnAdjnManagerObj = new FinPaymentAllocationAdjnManager(this.currentEntity);
                            if (finPaymentAlcnAdjnList.Count > 0)
                            {
                                finPaymentAlcnAdjnList.ForEach(dtl =>
                                {
                                    dtl.PAD_PAYMENT_TRX = finPaymentVndTrxMpgObj.PVM_PK;
                                });
                                //finPaymentAlcnAdjnManagerObj = new FinPaymentAllocationAdjnManager(this.currentEntity);
                                //finPaymentAlcnAdjnManagerObj.SavePaymentAdjn(finPaymentAlcnAdjnList, ref AdjnMaxID, (long)maxPaymentMpgPK, oldFinPaymentVndTrxMpgObj.PVM_INVOICE_HDR);
                                finPaymentAlcnAdjnManagerObj.SavePaymentAdjn(finPaymentAlcnAdjnList, ref AdjnMaxID, finPaymentVndTrxMpgObj.PVM_PK, oldFinPaymentVndTrxMpgObj.PVM_INVOICE_HDR);
                                
                            }
                            if (finPaymentCrdrAlcnList.Count > 0)
                            {
                                finPaymentCrdrAlcnList.ForEach(dtl =>
                                {
                                    dtl.PNM_PAYMENT_TRX_MPG = finPaymentVndTrxMpgObj.PVM_PK;
                                    dtl.PNM_PAYMENT_HDR = finPaymentVndTrxMpgObj.PVM_PAYMENT_HDR;
                                });
                                //finPaymentAlcnAdjnManagerObj = new FinPaymentAllocationAdjnManager(this.currentEntity);
                                finPaymentAlcnAdjnManagerObj.SavePaymentCrdrAllocation(finPaymentCrdrAlcnList, finPaymentVndTrxMpgObj.PVM_PK, ref CrdrAlcnMaxID);
                            }
                            else
                            {
                                #region Delete CR/DR Allocations
                                List<FIN_PAYMENT_VND_CRDR_MPG> Old_finPaymentVndCrdrAlcnList = oldFinPaymentVndTrxMpgObj.FIN_PAYMENT_VND_CRDR_MPG.ToList();
                                if (Old_finPaymentVndCrdrAlcnList != null)
                                {
                                    foreach (FIN_PAYMENT_VND_CRDR_MPG Old_finPaymentVndCrdrAlcnObj in Old_finPaymentVndCrdrAlcnList)
                                    {
                                        this.currentEntity.FIN_PAYMENT_VND_CRDR_MPG.DeleteObject(Old_finPaymentVndCrdrAlcnObj);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    //Update Paid amount corresponding to invoice
                    finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == finPaymentVndTrxMpgObj.PVM_INVOICE_HDR);

                    if (finInvoiceVndHdrObj != null)
                    {
                        finInvoiceVndHdrObj.IVH_AMOUNT_PAID_TC += paidAmt;
                    }
                }
                //return Payment Mpg master pk
                return retval;
            }

            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Get Invoice Trx Mpg details;
        /// </summary>
        /// <param name="AD_COUNTRIES_MSTObj" value="Contract master object with Contract master pk and active status"></param>
        /// <param name="utilityObj" value="Search criteria object"></param>
        /// <returns>List of Invoice Trx Mpg</returns>
        public List<FIN_PAYMENT_VND_TRX_MPG> GetPaymentTrxMpg(long paymentPK)
        {
            List<FIN_PAYMENT_VND_TRX_MPG> PaymentTrxMpgListObj = null;
            try
            {
                PaymentTrxMpgListObj = (from pvm in this.currentEntity.FIN_PAYMENT_VND_TRX_MPG
                                        join inv in this.currentEntity.FIN_INVOICE_VND_HDR on pvm.PVM_INVOICE_HDR equals inv.IVH_PK
                                        join vnd in this.currentEntity.PUR_VENDOR_MST on inv.IVH_VENDOR equals vnd.VEN_PK
                                        where pvm.PVM_PAYMENT_HDR == paymentPK
                                        select pvm
                                        //select new POInvoice()
                                        //{
                                        //    IVH_PK = inv.IVH_PK
                                        //      ,
                                        //    IVH_DATE = inv.IVH_DATE
                                        //      ,
                                        //    IVH_NO = inv.IVH_NO
                                        //      ,
                                        //    IVH_VENDOR = inv.IVH_VENDOR
                                        //      ,
                                        //    IVH_VENDOR_TEXT = vnd.VEN_NAME
                                        //      ,
                                        //    IVH_VENDOR_ACCOUNT = inv.IVH_VENDOR_ACCOUNT
                                        //      ,
                                        //    IVH_AMOUNT_BC = /*Convert.ToDecimal(inv.IVH_EXCHG_RATE) * */ inv.IVH_AMOUNT_TC
                                        //      ,
                                        //    IVH_DISCOUNT_BC = /*Convert.ToDecimal(inv.IVH_EXCHG_RATE) * */ inv.IVH_DISCOUNT_TC
                                        //      ,
                                        //    IVH_TAX_BC =/*Convert.ToDecimal(inv.IVH_EXCHG_RATE) * */ inv.IVH_TAX_TC
                                        //      ,
                                        //    IVH_AMOUNT_NET_BC = inv.IVH_AMOUNT_NET_BC
                                        //      ,
                                        //    IVH_INVOICED_AMOUNT = inv.IVH_AMOUNT_NET_BC //check invoiced amount
                                        //      ,
                                        //    IVH_PAID_AMOUNT = (from   ppvm in currentEntity.FIN_PAYMENT_VND_TRX_MPG
                                        //                              join ppvh in currentEntity.FIN_PAYMENT_VND_HDR on ppvm.PVM_PAYMENT_HDR equals ppvh.PVH_PK
                                        //                       where  ppvm.PVM_TRX_TYPE == 1
                                        //                           && ppvm.PVM_TRX_PK == inv.IVH_PK 
                                        //                           && ppvm.PVM_PAYMENT_HDR != pvm.PVM_PAYMENT_HDR
                                        //                       select ppvm
                                        //                       ).Sum(dtl => (decimal?)dtl.PVM_PAID_AMOUNT) ?? 0// in base currency
                                        //    ,
                                        //    IVH_BAL_AMOUNT = inv.IVH_AMOUNT_NET_BC - (from  ppvm in currentEntity.FIN_PAYMENT_VND_TRX_MPG
                                        //                                                    join ppvh in currentEntity.FIN_PAYMENT_VND_HDR on ppvm.PVM_PAYMENT_HDR equals ppvh.PVH_PK
                                        //                                              where ppvm.PVM_TRX_TYPE == 1 &&
                                        //                                                    ppvm.PVM_TRX_PK == inv.IVH_PK
                                        //                                              select ppvm
                                        //                                              ).Sum(dtl => (decimal?)dtl.PVM_PAID_AMOUNT) ?? 0 //calculate bal amt

                                        //    ,
                                        //    PVM_PK = pvm.PVM_PK
                                        //    ,
                                        //    PVM_PAYMENT_HDR = pvm.PVM_PAYMENT_HDR
                                        //    ,
                                        //    PVM_TRX_TYPE = pvm.PVM_TRX_TYPE
                                        //    ,
                                        //    PVM_TRX_PK = pvm.PVM_TRX_PK
                                        //    ,
                                        //    PVM_PAID_AMOUNT = pvm.PVM_PAID_AMOUNT
                                        ).ToList();
            }            
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            return PaymentTrxMpgListObj;
        }

        public List<FIN_PAYMENT_VND_TRX_MPG> GetInvoicePaymentTrxMpg(long invoicePK)
        {
            List<FIN_PAYMENT_VND_TRX_MPG> PaymentTrxMpgListObj = null;
            try
            {
                PaymentTrxMpgListObj = (from pvm in this.currentEntity.FIN_PAYMENT_VND_TRX_MPG
                                        where pvm.PVM_INVOICE_HDR == invoicePK
                                        select pvm
                                        ).ToList();
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            return PaymentTrxMpgListObj;
        }

        /// <summary>
        /// Get Invoice Trx Mpg details;
        /// </summary>
        /// <param name="AD_COUNTRIES_MSTObj" value="Contract master object with Contract master pk and active status"></param>
        /// <param name="utilityObj" value="Search criteria object"></param>
        /// <returns>List of Invoice Trx Mpg</returns>
        public List<FIN_INVOICE_VND_HDR> GetPaymentTrxMpg(List<long> InvoicePK)
        {
            List<FIN_INVOICE_VND_HDR> PaymentTrxMpgListObj = null;
            try
            {
                PaymentTrxMpgListObj = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                        where InvoicePK.Contains(inv.IVH_PK)
                                        select inv
                                        //select new POInvoice()
                                        //{
                                        //    IVH_PK = inv.IVH_PK
                                        //      ,
                                        //    IVH_DATE = inv.IVH_DATE
                                        //      ,
                                        //    IVH_NO = inv.IVH_NO
                                        //      ,
                                        //    IVH_VENDOR = inv.IVH_VENDOR
                                        //      ,
                                        //    IVH_VENDOR_ACCOUNT = inv.IVH_VENDOR_ACCOUNT
                                        //      ,
                                        //    IVH_VENDOR_TEXT = vnd.VEN_NAME
                                        //      ,
                                        //    IVH_AMOUNT_BC = /*Convert.ToDecimal(inv.IVH_EXCHG_RATE) * */inv.IVH_AMOUNT_TC
                                        //      ,
                                        //    IVH_DISCOUNT_BC = /*Convert.ToDecimal(inv.IVH_EXCHG_RATE) **/ inv.IVH_DISCOUNT_TC
                                        //      ,
                                        //    IVH_TAX_BC = /*Convert.ToDecimal(inv.IVH_EXCHG_RATE) **/ inv.IVH_TAX_TC
                                        //      ,
                                        //    IVH_AMOUNT_NET_BC = inv.IVH_AMOUNT_NET_BC
                                        //      ,
                                        //    IVH_INVOICED_AMOUNT = inv.IVH_AMOUNT_NET_BC //check invoiced amount
                                        //      ,
                                        //    IVH_PAID_AMOUNT = (from ppvm in currentEntity.FIN_PAYMENT_VND_TRX_MPG
                                        //                       join ppvh in currentEntity.FIN_PAYMENT_VND_HDR on ppvm.PVM_PAYMENT_HDR equals ppvh.PVH_PK
                                        //                       where ppvm.PVM_TRX_TYPE == 1 &&
                                        //                               ppvm.PVM_TRX_PK == inv.IVH_PK
                                        //                       select ppvm
                                        //                       ).Sum(dtl => (decimal?) dtl.PVM_PAID_AMOUNT) ?? 0 //calculate paid amt in BC

                                        //      ,
                                        //    IVH_BAL_AMOUNT = inv.IVH_AMOUNT_NET_BC - (from ppvm in currentEntity.FIN_PAYMENT_VND_TRX_MPG
                                        //                                              join ppvh in currentEntity.FIN_PAYMENT_VND_HDR on ppvm.PVM_PAYMENT_HDR equals ppvh.PVH_PK
                                        //                                              where ppvm.PVM_TRX_TYPE == 1 &&
                                        //                                                     ppvm.PVM_TRX_PK == inv.IVH_PK
                                        //                                              select ppvm
                                        //                                               ).Sum(dtl => (decimal?)dtl.PVM_PAID_AMOUNT) ?? 0 //calculate bal amt in BC
                                        //      ,
                                        //    PVM_PK = 0
                                        ).ToList();
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            return PaymentTrxMpgListObj;
        }

        #endregion



        public long? SavePaymentModeDetails(List<FIN_PAYMENT_VND_MODE_DTL> finPaymentModeDtlList)
        {
            long? retval = 0;
            long? maxID;
            List<FIN_PAYMENT_VND_MODE_DTL> Old_finPaymentModeList;
            FIN_PAYMENT_VND_MODE_DTL Old_finPaymentMode;
            try
            {

                if (finPaymentModeDtlList.Count > 0)
                {
                    maxID = currentEntity.FIN_PAYMENT_VND_MODE_DTL.Max(v => (long?)v.PDM_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    List<long> pks = (from old1 in finPaymentModeDtlList select old1.PDM_PK).ToList();
                    long? PDM_PAYMENT_HDR = finPaymentModeDtlList[0].PDM_PAYMENT_HDR;
                    Old_finPaymentModeList = (from oldp in this.currentEntity.FIN_PAYMENT_VND_MODE_DTL
                                                   where oldp.PDM_PAYMENT_HDR == PDM_PAYMENT_HDR && !pks.Contains(oldp.PDM_PK)
                                                   select oldp).ToList();

                    if (Old_finPaymentModeList != null)
                    {
                        foreach (FIN_PAYMENT_VND_MODE_DTL old_FIN_PAYMENT_VND_TAX_HDR in Old_finPaymentModeList)
                        {
                            this.currentEntity.FIN_PAYMENT_VND_MODE_DTL.DeleteObject(old_FIN_PAYMENT_VND_TAX_HDR);
                        }
                    }

                    foreach (FIN_PAYMENT_VND_MODE_DTL FIN_PAYMENT_VND_TAX_HDR_obj in finPaymentModeDtlList)
                    {

                        if (FIN_PAYMENT_VND_TAX_HDR_obj.PDM_PK == 0) //  INSERT NEW RECORD
                        {
                            FIN_PAYMENT_VND_TAX_HDR_obj.PDM_PK = maxID.Value;
                            currentEntity.FIN_PAYMENT_VND_MODE_DTL.AddObject(FIN_PAYMENT_VND_TAX_HDR_obj);
                            maxID++;
                            retval = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_PK;
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            Old_finPaymentMode = currentEntity.FIN_PAYMENT_VND_MODE_DTL.SingleOrDefault(a => a.PDM_PK == FIN_PAYMENT_VND_TAX_HDR_obj.PDM_PK);
                            Old_finPaymentMode.PDM_ACCOUNT = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_ACCOUNT;
                            Old_finPaymentMode.PDM_BANK = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_BANK;
                            Old_finPaymentMode.PDM_BANK_CHARGE = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_BANK_CHARGE;
                            Old_finPaymentMode.PDM_BANK_CHARGE_CURR = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_BANK_CHARGE_CURR;
                            Old_finPaymentMode.PDM_BANK_CHARGE_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_BANK_CHARGE_TYPE;
                            Old_finPaymentMode.PDM_BRANCH = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_BRANCH;
                            Old_finPaymentMode.PDM_EXCHG_RATE = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_EXCHG_RATE;
                            Old_finPaymentMode.PDM_INSTR_DATE = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_INSTR_DATE;
                            Old_finPaymentMode.PDM_INSTR_FAVOUR = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_INSTR_FAVOUR;
                            Old_finPaymentMode.PDM_INSTR_NO = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_INSTR_NO;
                            Old_finPaymentMode.PDM_MODE = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_MODE;
                            Old_finPaymentMode.PDM_PAID_AMOUNT = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_PAID_AMOUNT;
                            Old_finPaymentMode.PDM_PAID_AMOUNT_BC = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_PAID_AMOUNT_BC;
                            Old_finPaymentMode.PDM_PDC = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_PDC;
                            retval = FIN_PAYMENT_VND_TAX_HDR_obj.PDM_PK;
                        }
                    }
                }
                return retval;
            }//Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
    }
}
