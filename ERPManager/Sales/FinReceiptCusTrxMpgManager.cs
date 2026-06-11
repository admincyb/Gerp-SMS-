using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using ERPManager.Sales;
using BusinessObject.CommonManagement;
using ERPManager.Finance;
using System.Data;
using ERP.Utilities;

namespace ERPManager
{
    public class FinReceiptCusTrxMpgManager : IFinReceiptCusTrxMpgManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Receipt Maping Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public FinReceiptCusTrxMpgManager(ERPEntities currentEntity)
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
        /// Save Receipt Maping 
        /// </summary>
        /// <param name="FinReceiptCusTrxMpgList"></param>
        /// <returns></returns>
        public long? SaveReceiptDtl(List<FIN_RECEIPT_CUS_TRX_MPG> finReceiptCusTrxMpgList, Byte Category)
        {
            //Holds save status
            long retval;
            //Holds last Receipt Mpg master pk
            long? maxReceiptMpgPK;
            decimal ReceivedAmt = 0;
            long ReceiptHdr = 0;
            long? maxID;
            long? AdjnMaxID;
            long? TaxMaxID;
            long? CrdrAlcnMaxID;
            //On update holds old Receipt Mpg master details
            FIN_RECEIPT_CUS_TRX_MPG oldFinReceiptCusTrxMpgObj;
            FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj;
            List<FIN_RECEIPT_CUS_TRX_MPG> oldFinPaymentCusTrxMpgListObj;

            List<FIN_RECEIPT_CUS_SO_MPG> finReceiptCusSoMpgList;
            FinReceiptCusSoMpgManager finReceiptCusSOMpgManagerObj;

            List<FIN_RECEIPT_CUS_ALCN_DTL> finReceiptAlcnAdjnList;
            FinReceiptAllocationAdjnManager finReceiptAlcnAdjnManagerObj;

            List<FIN_RECEIPT_CUS_CRDR_MPG> finReceiptCrdrAlcnList;

            SAL_ORDER_HDR obj_SAL_ORDER_HDR;
           

            try
            {
                //Set save status zero,save failed
                retval = 0;
                //Getting last Receipt Mpg pk
                maxReceiptMpgPK = currentEntity.FIN_RECEIPT_CUS_TRX_MPG.Max(v => (int?)v.RCM_PK);
                maxReceiptMpgPK = (maxReceiptMpgPK.HasValue) ? maxReceiptMpgPK.Value + 1 : 1;
                //Iterate through mpg list for save
                maxID = currentEntity.FIN_RECEIPT_CUS_SO_MPG.Max(v => (long?)v.RSO_PK);

                AdjnMaxID = currentEntity.FIN_RECEIPT_CUS_ALCN_DTL.Max(v => (long?)v.RAD_PK).HasValue? currentEntity.FIN_RECEIPT_CUS_ALCN_DTL.Max(v => (long?)v.RAD_PK) + 1:1;
                TaxMaxID = currentEntity.FIN_RECEIPT_CUS_TAX_DTL.Max(v => (long?)v.RDT_PK);

                CrdrAlcnMaxID = currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Max(v => (long?)v.RNM_PK).HasValue ? +currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Max(v => (long?)v.RNM_PK) + 1 : 1;

                //Delete
                if (finReceiptCusTrxMpgList.Count > 0)
                {
                    List<long> pks = (from old1 in finReceiptCusTrxMpgList
                                      select old1.RCM_PK).ToList();

                    ReceiptHdr = finReceiptCusTrxMpgList[0].RCM_RECEIPT_HDR;

                    oldFinPaymentCusTrxMpgListObj = (from old in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG
                                                     where ReceiptHdr == old.RCM_RECEIPT_HDR
                                                        && !pks.Contains(old.RCM_PK)
                                                     select old).ToList();

                    foreach (FIN_RECEIPT_CUS_TRX_MPG oldFinReceiptCusTrxMpgObject in oldFinPaymentCusTrxMpgListObj)
                    {
                        finInvoiceCusHdrObj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == oldFinReceiptCusTrxMpgObject.RCM_INVOICE_HDR);

                        if (finInvoiceCusHdrObj != null)
                        {
                            finInvoiceCusHdrObj.ICH_AMOUNT_RCVD_TC -= oldFinReceiptCusTrxMpgObject.RCM_RCVD_AMOUNT;
                        }

                        #region Delete Receipt Split
                        List<FIN_RECEIPT_CUS_SO_MPG> Old_finReceiptCusSOMpgList = oldFinReceiptCusTrxMpgObject.FIN_RECEIPT_CUS_SO_MPG.ToList();
                        if (Old_finReceiptCusSOMpgList != null)
                        {
                            foreach (FIN_RECEIPT_CUS_SO_MPG old_FIN_RECEIPT_CUS_SO_MPG in Old_finReceiptCusSOMpgList)
                            {
                                obj_SAL_ORDER_HDR = currentEntity.SAL_ORDER_HDR.SingleOrDefault(a => a.SOH_PK == old_FIN_RECEIPT_CUS_SO_MPG.RSO_SO_HDR);

                                if (obj_SAL_ORDER_HDR != null)
                                {
                                    obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED -= (old_FIN_RECEIPT_CUS_SO_MPG.RSO_RECEIVED_AMOUNT - (decimal)oldFinReceiptCusTrxMpgObject.RCM_EXCESS_AMOUNT);
                                }

                                this.currentEntity.FIN_RECEIPT_CUS_SO_MPG.DeleteObject(old_FIN_RECEIPT_CUS_SO_MPG);
                            }
                        } 
                        #endregion

                        #region Delete Adjustments
                        List<FIN_RECEIPT_CUS_ALCN_DTL> oldfinAdjnListObj = oldFinReceiptCusTrxMpgObject.FIN_RECEIPT_CUS_ALCN_DTL.ToList();
                        if (oldfinAdjnListObj != null)
                        {
                            foreach (FIN_RECEIPT_CUS_ALCN_DTL oldfinCrDrNoteTaxHdrObj in oldfinAdjnListObj)
                            {                              
                                if (finInvoiceCusHdrObj != null)
                                {
                                    if (oldfinCrDrNoteTaxHdrObj.RAD_ALCN_CDH != null)
                                    {
                                        finInvoiceCusHdrObj.ICH_AMOUNT_CN_TC -= oldfinCrDrNoteTaxHdrObj.RAD_AMOUNT;
                                    }
                                    else
                                    {
                                        finInvoiceCusHdrObj.ICH_AMOUNT_RCVD_TC -= oldfinCrDrNoteTaxHdrObj.RAD_AMOUNT;
                                    }
                                }
                                this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL.DeleteObject(oldfinCrDrNoteTaxHdrObj);
                            }
                        }
                        #endregion

                        #region Delete Detail Tax
                        List<FIN_RECEIPT_CUS_TAX_DTL> Old_finReceiptCusTaxDtlList = oldFinReceiptCusTrxMpgObject.FIN_RECEIPT_CUS_TAX_DTL.ToList();
                        if (Old_finReceiptCusTaxDtlList != null)
                        {
                            foreach (FIN_RECEIPT_CUS_TAX_DTL Old_finReceiptCusTaxDtlObj in Old_finReceiptCusTaxDtlList)
                            {
                                this.currentEntity.FIN_RECEIPT_CUS_TAX_DTL.DeleteObject(Old_finReceiptCusTaxDtlObj);
                            }
                        } 
                        #endregion

                        #region Delete CR/DR Allocations
                        List<FIN_RECEIPT_CUS_CRDR_MPG> Old_finReceiptCusCrdrAlcnList = oldFinReceiptCusTrxMpgObject.FIN_RECEIPT_CUS_CRDR_MPG.ToList();
                        if (Old_finReceiptCusCrdrAlcnList != null)
                        {
                            foreach (FIN_RECEIPT_CUS_CRDR_MPG Old_finReceiptCusCrdrAlcnObj in Old_finReceiptCusCrdrAlcnList)
                            {
                                this.currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.DeleteObject(Old_finReceiptCusCrdrAlcnObj);
                            }
                        }
                        #endregion
                        
                        this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG.DeleteObject(oldFinReceiptCusTrxMpgObject);
                    }
                }
                //long? soMpgPK = 0;
                foreach (FIN_RECEIPT_CUS_TRX_MPG FinReceiptCusTrxMpgObj in finReceiptCusTrxMpgList)
                {
                    finReceiptCusSoMpgList = FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_SO_MPG == null ?
                    new List<FIN_RECEIPT_CUS_SO_MPG>() : FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_SO_MPG.ToList();
                    FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_SO_MPG.Clear();

                    finReceiptAlcnAdjnList = FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_ALCN_DTL == null ?
                    new List<FIN_RECEIPT_CUS_ALCN_DTL>() : FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_ALCN_DTL.ToList();
                    FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_ALCN_DTL.Clear();

                    finReceiptCrdrAlcnList = FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_CRDR_MPG == null ?
                    new List<FIN_RECEIPT_CUS_CRDR_MPG>() : FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_CRDR_MPG.ToList();
                    FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_CRDR_MPG.Clear();

                    //check Receipt Mpg master pk is zero,save Receipt Mpg master as new record
                    //Insert
                    if (FinReceiptCusTrxMpgObj.RCM_PK == 0)
                    {


                        //finReceiptCusSoMpgList = FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_SO_MPG == null ?
                        //    new List<FIN_RECEIPT_CUS_SO_MPG>() : FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_SO_MPG.ToList();
                        //FinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_SO_MPG.Clear();

                        //Set next Receipt Mpg pk
                        FinReceiptCusTrxMpgObj.RCM_PK = (long)maxReceiptMpgPK;
                        //Add new Receipt Mpg to the db context
                        currentEntity.FIN_RECEIPT_CUS_TRX_MPG.AddObject(FinReceiptCusTrxMpgObj);
                        maxReceiptMpgPK++;
                        retval = FinReceiptCusTrxMpgObj.RCM_PK;
                        ReceivedAmt = FinReceiptCusTrxMpgObj.RCM_RCVD_AMOUNT;
                        decimal? discTotAmount = 0;

                        if (Category != (Byte)(SalesInvoiceCategory.Invoice))
                        {
                            discTotAmount = FinReceiptCusTrxMpgObj.FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_TRX_MPG.Select(ICM => (decimal?)ICM.ICM_DISCOUNT_AMOUNT).FirstOrDefault();
                        }
                        else
                        {
                            discTotAmount = FinReceiptCusTrxMpgObj.FIN_INVOICE_CUS_HDR.ICH_DISCOUNT_TC + FinReceiptCusTrxMpgObj.FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_DTL.Sum(ss => ss.CID_DISCOUNT);//).Select(d=>d.FIN_INVOICE_CUS_TAX_DTL.Sum(j=>j.c)) .FIN_INVOICE_CUS_TRX_MPG.Select(ICM => (decimal?)ICM.ICM_DISCOUNT_AMOUNT).FirstOrDefault();
                        }

                        if (finReceiptCusSoMpgList.Count > 0)
                        {
                            finReceiptCusSoMpgList.ForEach(dtl =>
                            {
                                dtl.RSO_RECEIPT_HDR = FinReceiptCusTrxMpgObj.RCM_RECEIPT_HDR;
                                dtl.RSO_RECEIPT_TRX_MPG = FinReceiptCusTrxMpgObj.RCM_PK;
                            });
                            finReceiptCusSOMpgManagerObj = new FinReceiptCusSoMpgManager(this.currentEntity);
                            finReceiptCusSOMpgManagerObj.SaveReceiptSplit(finReceiptCusSoMpgList, ref maxID, ref TaxMaxID, Category, FinReceiptCusTrxMpgObj.RCM_INVOICE_HDR, Convert.ToDecimal(discTotAmount), FinReceiptCusTrxMpgObj.RCM_EXCESS_AMOUNT, 0);
                        }

                        finReceiptAlcnAdjnManagerObj = new FinReceiptAllocationAdjnManager(this.currentEntity);
                        if (finReceiptAlcnAdjnList.Count > 0)
                        {
                            finReceiptAlcnAdjnList.ForEach(dtl =>
                            {
                                dtl.RAD_RECEIPT_TRX = FinReceiptCusTrxMpgObj.RCM_PK;
                            });
                            //finReceiptAlcnAdjnManagerObj = new FinReceiptAllocationAdjnManager(this.currentEntity);
                            //AdjnMaxID = finReceiptAlcnAdjnManagerObj.SaveReceiptAdjn(finReceiptAlcnAdjnList, ref AdjnMaxID, (long)maxReceiptMpgPK, FinReceiptCusTrxMpgObj.RCM_INVOICE_HDR);
                            //AdjnMaxID =
                            finReceiptAlcnAdjnManagerObj.SaveReceiptAdjn(finReceiptAlcnAdjnList, ref AdjnMaxID, FinReceiptCusTrxMpgObj.RCM_PK, FinReceiptCusTrxMpgObj.RCM_INVOICE_HDR);
                            //AdjnMaxID = AdjnMaxID + (long)1;
                        }
                        if (finReceiptCrdrAlcnList.Count > 0)
                        {
                            finReceiptCrdrAlcnList.ForEach(dtl =>
                            {
                                dtl.RNM_RECEIPT_TRX_MPG = FinReceiptCusTrxMpgObj.RCM_PK;
                                dtl.RNM_RECEIPT_HDR = FinReceiptCusTrxMpgObj.RCM_RECEIPT_HDR;
                            });
                            //finReceiptAlcnAdjnManagerObj = new FinReceiptAllocationAdjnManager(this.currentEntity);
                            finReceiptAlcnAdjnManagerObj.SaveReceiptCrdrAllocation(finReceiptCrdrAlcnList, FinReceiptCusTrxMpgObj.RCM_PK, ref CrdrAlcnMaxID);
                        }

                    }
                    //updating Receipt Mpg details
                    //Update
                    else
                    {
                        //Get current Receipt Mpg master details using Receipt Mpg master pk
                        oldFinReceiptCusTrxMpgObj = currentEntity.FIN_RECEIPT_CUS_TRX_MPG.SingleOrDefault(v => v.RCM_PK == FinReceiptCusTrxMpgObj.RCM_PK);
                        if (oldFinReceiptCusTrxMpgObj != null)
                        {
                            ReceivedAmt = FinReceiptCusTrxMpgObj.RCM_RCVD_AMOUNT - oldFinReceiptCusTrxMpgObj.RCM_RCVD_AMOUNT;
                            //Update Receipt Mpg details
                            oldFinReceiptCusTrxMpgObj.RCM_RCVD_AMOUNT = FinReceiptCusTrxMpgObj.RCM_RCVD_AMOUNT;
                            oldFinReceiptCusTrxMpgObj.RCM_DISC_AMOUNT = FinReceiptCusTrxMpgObj.RCM_DISC_AMOUNT;
                            oldFinReceiptCusTrxMpgObj.RCM_TAX_AMOUNT = FinReceiptCusTrxMpgObj.RCM_TAX_AMOUNT;
                            oldFinReceiptCusTrxMpgObj.RCM_ACTIVE = FinReceiptCusTrxMpgObj.RCM_ACTIVE;
                            decimal old_Excess = oldFinReceiptCusTrxMpgObj.RCM_EXCESS_AMOUNT;
                            oldFinReceiptCusTrxMpgObj.RCM_EXCESS_AMOUNT = FinReceiptCusTrxMpgObj.RCM_EXCESS_AMOUNT;
                            //Set return value as Receipt Mpg pk
                            retval = FinReceiptCusTrxMpgObj.RCM_PK;
                            decimal? discTotAmount = 0;
                            if (Category != (Byte)(SalesInvoiceCategory.Invoice))
                            {
                                if (FinReceiptCusTrxMpgObj.FIN_INVOICE_CUS_HDR != null)
                                    discTotAmount = FinReceiptCusTrxMpgObj.FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_TRX_MPG.Select(ICM => (decimal?)ICM.ICM_DISCOUNT_AMOUNT).FirstOrDefault();
                            }
                            else
                            {
                                if (FinReceiptCusTrxMpgObj.FIN_INVOICE_CUS_HDR != null)
                                    discTotAmount = FinReceiptCusTrxMpgObj.FIN_INVOICE_CUS_HDR.ICH_DISCOUNT_TC + FinReceiptCusTrxMpgObj.FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_DTL.Sum(ss => ss.CID_DISCOUNT);//).Select(d=>d.FIN_INVOICE_CUS_TAX_DTL.Sum(j=>j.c)) .FIN_INVOICE_CUS_TRX_MPG.Select(ICM => (decimal?)ICM.ICM_DISCOUNT_AMOUNT).FirstOrDefault();
                            }

                            if (finReceiptCusSoMpgList.Count > 0)
                            {
                                finReceiptCusSoMpgList.ForEach(dtl =>
                                {
                                    dtl.RSO_RECEIPT_HDR = FinReceiptCusTrxMpgObj.RCM_RECEIPT_HDR;
                                    dtl.RSO_RECEIPT_TRX_MPG = FinReceiptCusTrxMpgObj.RCM_PK;
                                });
                                finReceiptCusSOMpgManagerObj = new FinReceiptCusSoMpgManager(this.currentEntity);
                                finReceiptCusSOMpgManagerObj.SaveReceiptSplit(finReceiptCusSoMpgList, ref maxID, ref TaxMaxID, Category, oldFinReceiptCusTrxMpgObj.RCM_INVOICE_HDR, discTotAmount, FinReceiptCusTrxMpgObj.RCM_EXCESS_AMOUNT, old_Excess);

                            }
                            finReceiptAlcnAdjnManagerObj = new FinReceiptAllocationAdjnManager(this.currentEntity);
                            if (finReceiptAlcnAdjnList.Count > 0)
                            {
                                finReceiptAlcnAdjnList.ForEach(dtl =>
                                {
                                    dtl.RAD_RECEIPT_TRX = FinReceiptCusTrxMpgObj.RCM_PK;
                                });
                                //finReceiptAlcnAdjnManagerObj = new FinReceiptAllocationAdjnManager(this.currentEntity);
                                finReceiptAlcnAdjnManagerObj.SaveReceiptAdjn(finReceiptAlcnAdjnList, ref AdjnMaxID, (long)FinReceiptCusTrxMpgObj.RCM_PK, oldFinReceiptCusTrxMpgObj.RCM_INVOICE_HDR);

                            }
                            if (finReceiptCrdrAlcnList.Count > 0)
                            {
                                finReceiptCrdrAlcnList.ForEach(dtl =>
                                {
                                    dtl.RNM_RECEIPT_TRX_MPG = FinReceiptCusTrxMpgObj.RCM_PK;
                                    dtl.RNM_RECEIPT_HDR = FinReceiptCusTrxMpgObj.RCM_RECEIPT_HDR;
                                });
                                //finReceiptAlcnAdjnManagerObj = new FinReceiptAllocationAdjnManager(this.currentEntity);
                                finReceiptAlcnAdjnManagerObj.SaveReceiptCrdrAllocation(finReceiptCrdrAlcnList, (long)FinReceiptCusTrxMpgObj.RCM_PK, ref CrdrAlcnMaxID);
                            }
                            else
                            {
                                #region Delete CR/DR Allocations
                                List<FIN_RECEIPT_CUS_CRDR_MPG> Old_finReceiptCusCrdrAlcnList = oldFinReceiptCusTrxMpgObj.FIN_RECEIPT_CUS_CRDR_MPG.ToList();
                                if (Old_finReceiptCusCrdrAlcnList != null)
                                {
                                    foreach (FIN_RECEIPT_CUS_CRDR_MPG Old_finReceiptCusCrdrAlcnObj in Old_finReceiptCusCrdrAlcnList)
                                    {
                                        this.currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.DeleteObject(Old_finReceiptCusCrdrAlcnObj);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    //Update Paid amount corresponding to invoice
                    finInvoiceCusHdrObj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == FinReceiptCusTrxMpgObj.RCM_INVOICE_HDR);

                    if (finInvoiceCusHdrObj != null)
                    {
                        finInvoiceCusHdrObj.ICH_AMOUNT_RCVD_TC += ReceivedAmt;
                    }
                }
                //return Receipt Mpg master pk
                return retval;
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }


        public long? SaveReceiptSuspList(List<FIN_RECEIPT_CUS_SUSP_DTL> finReceiptCusSuspList, long RecieptHdr)
        {
            //Holds save status
            long retval;
            int? maxPk;
            try
            {
                List<FIN_RECEIPT_CUS_SUSP_DTL> oldReceiptCusSuspList;
                FIN_RECEIPT_CUS_SUSP_DTL oldRecSuspObj;
                //Set save status zero,save failed
                retval = 0;
                maxPk = this.currentEntity.FIN_RECEIPT_CUS_SUSP_DTL.Max(rsc => (int?)rsc.RSC_PK);
                maxPk = (maxPk.HasValue) ? maxPk.Value + 1 : 1;

                List<long> pks = (from old1 in finReceiptCusSuspList
                                  select old1.RSC_PK).ToList();
              

                oldReceiptCusSuspList = (from old in this.currentEntity.FIN_RECEIPT_CUS_SUSP_DTL
                                         where !pks.Contains(old.RSC_PK) && RecieptHdr == old.RSC_RECEIPT_HDR
                                            select old).ToList();

                #region Delete Existing Entried for the Receipt
                foreach (FIN_RECEIPT_CUS_SUSP_DTL oldfinReceiptSuspObj in oldReceiptCusSuspList)
                {
                    this.currentEntity.FIN_RECEIPT_CUS_SUSP_DTL.DeleteObject(oldfinReceiptSuspObj);
                }
                #endregion
                #region Add items to table
                foreach (FIN_RECEIPT_CUS_SUSP_DTL finReceiptSuspObj in finReceiptCusSuspList)
                {
                    if (finReceiptSuspObj.RSC_PK == 0)
                    {
                        finReceiptSuspObj.RSC_PK = maxPk.Value;
                        finReceiptSuspObj.RSC_RECEIPT_HDR = RecieptHdr;
                        this.currentEntity.FIN_RECEIPT_CUS_SUSP_DTL.AddObject(finReceiptSuspObj);
                        maxPk++;
                        retval = finReceiptSuspObj.RSC_PK;
                    }
                    else
                    {
                        oldRecSuspObj = currentEntity.FIN_RECEIPT_CUS_SUSP_DTL.Single(x => x.RSC_PK == finReceiptSuspObj.RSC_PK);
                        if (oldRecSuspObj != null)
                        {
                            oldRecSuspObj.RSC_RECEIPT_HDR = finReceiptSuspObj.RSC_RECEIPT_HDR;
                            oldRecSuspObj.RSC_VOUCHER_HDR = finReceiptSuspObj.RSC_VOUCHER_HDR;
                            oldRecSuspObj.RSC_AMOUNT = finReceiptSuspObj.RSC_AMOUNT;
                            retval = finReceiptSuspObj.RSC_PK;
                        }
                    }
                }
                #endregion
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

            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Invoice Trx Mpg details;
        /// </summary>
        /// <param name="AD_COUNTRIES_MSTObj" value="Contract master object with Contract master pk and active status"></param>
        /// <param name="utilityObj" value="Search criteria object"></param>
        /// <returns>List of Invoice Trx Mpg</returns>
        public List<FIN_RECEIPT_CUS_TRX_MPG> GetReceiptTrxMpg(long receiptPK)
        {
            List<FIN_RECEIPT_CUS_TRX_MPG> ReceiptTrxMpgListObj = null;
            try
            {
                ReceiptTrxMpgListObj = (from rcm in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG
                                        where rcm.RCM_RECEIPT_HDR == receiptPK
                                        select rcm
                    //{
                    //    ICH_PK = inv.ICH_PK
                    //      ,
                    //    ICH_DATE = inv.ICH_DATE
                    //      ,
                    //    ICH_NO = inv.ICH_NO
                    //      ,
                    //    ICH_CUSTOMER = inv.ICH_CUSTOMER
                    //      ,
                    //    ICH_CUSTOMER_TEXT = cus.CUS_NAME
                    //      ,
                    //    ICH_CUSTOMER_ACCOUNT = inv.ICH_CUSTOMER_ACCOUNT
                    //      ,
                    //    ICH_AMOUNT_BC = /*Convert.ToDecimal(inv.ICH_EXCHG_RATE) * */ inv.ICH_AMOUNT_TC
                    //      ,
                    //    ICH_DISCOUNT_BC = /*Convert.ToDecimal(inv.ICH_EXCHG_RATE) * */ inv.ICH_DISCOUNT_TC
                    //      ,
                    //    ICH_TAX_BC =/*Convert.ToDecimal(inv.ICH_EXCHG_RATE) * */ inv.ICH_TAX_TC
                    //      ,
                    //    ICH_AMOUNT_NET_BC = inv.ICH_AMOUNT_NET_BC
                    //      ,
                    //    ICH_INVOICED_AMOUNT = inv.ICH_AMOUNT_NET_BC //check invoiced amount
                    //      ,
                    //    ICH_RCVD_AMOUNT = (from prcm in currentEntity.FIN_RECEIPT_CUS_TRX_MPG
                    //                       join ppvh in currentEntity.FIN_RECEIPT_CUS_HDR on prcm.RCM_RECEIPT_HDR equals ppvh.RCH_PK
                    //                       where prcm.RCM_TRX_TYPE == 1
                    //                           && prcm.RCM_TRX_PK == inv.ICH_PK
                    //                           && prcm.RCM_RECEIPT_HDR != rcm.RCM_RECEIPT_HDR
                    //                       select prcm
                    //                       ).Sum(dtl => (decimal?)dtl.RCM_RCVD_AMOUNT) ?? 0// in base currency
                    //    ,
                    //    ICH_BAL_AMOUNT = inv.ICH_AMOUNT_NET_BC - (from prcm in currentEntity.FIN_RECEIPT_CUS_TRX_MPG
                    //                                              join ppvh in currentEntity.FIN_RECEIPT_CUS_HDR on prcm.RCM_RECEIPT_HDR equals ppvh.RCH_PK
                    //                                              where prcm.RCM_TRX_TYPE == 1 &&
                    //                                                    prcm.RCM_TRX_PK == inv.ICH_PK
                    //                                              select prcm
                    //                                              ).Sum(dtl => (decimal?)dtl.RCM_RCVD_AMOUNT) ?? 0  //calculate bal amt

                                        //    ,
                    //    RCM_PK = rcm.RCM_PK
                    //    ,
                    //    RCM_RECEIPT_HDR = rcm.RCM_RECEIPT_HDR
                    //    ,
                    //    RCM_TRX_TYPE = rcm.RCM_TRX_TYPE
                    //    ,
                    //    RCM_TRX_PK = rcm.RCM_TRX_PK
                    //    ,
                    //    RCM_RCVD_AMOUNT = rcm.RCM_RCVD_AMOUNT
                    //}
            ).ToList();
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            return ReceiptTrxMpgListObj;
        }

        public DataTable GetReceiptAmtSplitUp(long invoicePK, long ReceiptPK, string DraftNo)
        {
            DataTable dtRecAmt = new DataTable();
           // long recPK = ReceiptPK == 0 ?(Oject)DBNull.Value : ReceiptPK;
            try
            {
                var ReciptAmt = (from rcm in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG
                                 where rcm.RCM_INVOICE_HDR == invoicePK && rcm.FIN_RECEIPT_CUS_HDR.RCH_PK != ReceiptPK && rcm.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 && rcm.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                              select new 
                              {
                                  TrxNo = string.IsNullOrEmpty(rcm.FIN_RECEIPT_CUS_HDR.RCH_NO) ? DraftNo : rcm.FIN_RECEIPT_CUS_HDR.RCH_NO,
                                  TrxDate = rcm.FIN_RECEIPT_CUS_HDR.RCH_DATE,
                                  TrxAmt = rcm.RCM_RCVD_AMOUNT

                              });
                var AllocaAmt = (from rcm in this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL
                                 where rcm.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == invoicePK && rcm.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_PK != ReceiptPK && rcm.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 && rcm.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                              select new
                              {
                                  TrxNo = (string.IsNullOrEmpty(rcm.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_NO) ? DraftNo : rcm.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_NO)+"("+(rcm.RAD_ALCN_RECEIPT_TRX.HasValue?rcm.FIN_RECEIPT_CUS_TRX_MPG1.FIN_RECEIPT_CUS_HDR.RCH_NO:rcm.FIN_CRDR_NOTE_HDR.CDH_NO)+")",
                                  TrxDate = rcm.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DATE,
                                  TrxAmt = rcm.RAD_AMOUNT

                              });
                var TotalAmt = ReciptAmt.Union(AllocaAmt);
                return TotalAmt.ToList().ToDataTable();

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
        public List<FIN_INVOICE_CUS_HDR> GetReceiptTrxMpg(List<long> InvoicePK)
        {
            List<FIN_INVOICE_CUS_HDR> ReceiptTrxMpgListObj = null;
            try
            {
                ReceiptTrxMpgListObj = (from inv in this.currentEntity.FIN_INVOICE_CUS_HDR
                                        join cus in this.currentEntity.CRM_CUSTOMER_MST on inv.ICH_CUSTOMER equals cus.CUS_PK
                                        where InvoicePK.Contains(inv.ICH_PK)
                                        select inv
                    //{
                    //    ICH_PK = inv.ICH_PK
                    //      ,
                    //    ICH_DATE = inv.ICH_DATE
                    //      ,
                    //    ICH_NO = inv.ICH_NO
                    //      ,
                    //    ICH_CUSTOMER = inv.ICH_CUSTOMER
                    //      ,
                    //    ICH_CUSTOMER_ACCOUNT = inv.ICH_CUSTOMER_ACCOUNT
                    //      ,
                    //    ICH_CUSTOMER_TEXT = cus.CUS_NAME
                    //      ,
                    //    ICH_AMOUNT_BC = /*Convert.ToDecimal(inv.ICH_EXCHG_RATE) * */inv.ICH_AMOUNT_TC
                    //      ,
                    //    ICH_DISCOUNT_BC = /*Convert.ToDecimal(inv.ICH_EXCHG_RATE) **/ inv.ICH_DISCOUNT_TC
                    //      ,
                    //    ICH_TAX_BC = /*Convert.ToDecimal(inv.ICH_EXCHG_RATE) **/ inv.ICH_TAX_TC
                    //      ,
                    //    ICH_AMOUNT_NET_BC = inv.ICH_AMOUNT_NET_BC
                    //      ,
                    //    ICH_INVOICED_AMOUNT = inv.ICH_AMOUNT_NET_BC //check invoiced amount
                    //      ,
                    //    ICH_RCVD_AMOUNT = (from prcm in currentEntity.FIN_RECEIPT_CUS_TRX_MPG
                    //                       join ppvh in currentEntity.FIN_RECEIPT_CUS_HDR on prcm.RCM_RECEIPT_HDR equals ppvh.RCH_PK
                    //                       where prcm.RCM_TRX_TYPE == 1 &&
                    //                               prcm.RCM_TRX_PK == inv.ICH_PK
                    //                       select prcm
                    //                       ).Sum(dtl => (decimal?)dtl.RCM_RCVD_AMOUNT) ?? 0 //calculate paid amt in BC
                    //      ,
                    //    ICH_BAL_AMOUNT = inv.ICH_AMOUNT_NET_BC - (from prcm in currentEntity.FIN_RECEIPT_CUS_TRX_MPG
                    //                                              join ppvh in currentEntity.FIN_RECEIPT_CUS_HDR on prcm.RCM_RECEIPT_HDR equals ppvh.RCH_PK
                    //                                              where prcm.RCM_TRX_TYPE == 1 &&
                    //                                                     prcm.RCM_TRX_PK == inv.ICH_PK
                    //                                              select prcm
                    //                                               ).Sum(dtl => (decimal?)dtl.RCM_RCVD_AMOUNT) ?? 0 //calculate bal amt in BC
                    //      ,
                    //    RCM_PK = 0
                    //}
                        ).ToList();
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            return ReceiptTrxMpgListObj;
        }

        /// <summary>
        /// Get adv Invoice deducted details;
        /// </summary>
        /// <param name="AD_COUNTRIES_MSTObj" value="Contract master object with Contract master pk and active status"></param>
        /// <param name="utilityObj" value="Search criteria object"></param>
        /// <returns>List of Invoice Trx Mpg</returns>
        public bool IsReceiptAdvDeducted(int reciptPK)
        {
            List<FIN_INVOICE_CUS_ADV_DED_DTL> ReceiptAdvDedListObj = null;
            try
            {
                ReceiptAdvDedListObj = (from rcpt in this.currentEntity.FIN_INVOICE_CUS_ADV_DED_DTL
                                        where rcpt.IAD_RECEIPT_HDR == reciptPK
                                        && rcpt.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0
                                        select rcpt
                                        ).ToList();
                if (ReceiptAdvDedListObj.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

        }
        #endregion
    }
}
