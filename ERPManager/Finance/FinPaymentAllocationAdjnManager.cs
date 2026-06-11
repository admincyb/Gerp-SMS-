using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;
using ERPManager.Sales;


namespace ERPManager.Finance
{
    public class FinPaymentAllocationAdjnManager : IFinPaymentAllocationAdjnManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// FinCashBankManager Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>

        public FinPaymentAllocationAdjnManager(ERPEntities currentEntity)
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




        #endregion

        public long? SavePaymentAdjn(List<FIN_PAYMENT_VND_ALCN_DTL> finAdjnHdr, ref  long? Maxid, long PVM_PK, long? InvoicePk)
        {
            //Holds save status
            long retval;
            //Holds last Payment Mpg master pk
            long? maxAdjnPK;
            maxAdjnPK = Maxid;
            decimal Amount = 0;
            decimal Old_Amount = 0;
            bool IsdrCrNote = false;
            //On update holds old Payment Mpg master details
            FIN_PAYMENT_VND_ALCN_DTL oldfinAdjnTaxObj;
            List<FIN_PAYMENT_VND_ALCN_DTL> oldfinAdjnListObj;
            List<FIN_PAYMENT_VND_ALCN_DTL> FIN_PAYMENT_ADJN_List = new List<FIN_PAYMENT_VND_ALCN_DTL>();
            FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;

            try
            {
                retval = 0;
                if (finAdjnHdr.Count > 0)
                { //Set save status zero,save failed
                    //Getting last Payment Mpg pk
                    //maxAdjnPK = currentEntity.FIN_PAYMENT_VND_ALCN_DTL.Max(v => (int?)v.PAD_PK);
                    //maxAdjnPK = (maxAdjnPK.HasValue) ? maxAdjnPK.Value + 1 : 1;
                    //maxID = currentEntity.FIN_CRDR_NOTE_DTL.Max(v => (long?)v.CDS_PK);
                    //Iterate through vedor list for save

                    finAdjnHdr = (from c in finAdjnHdr where c.PAD_AMOUNT > 0 select c).ToList();
                    List<long> pks = (from old1 in finAdjnHdr
                                      select old1.PAD_PK).ToList();


                    oldfinAdjnListObj = (from old in this.currentEntity.FIN_PAYMENT_VND_ALCN_DTL
                                         where !pks.Contains(old.PAD_PK) && PVM_PK == old.PAD_PAYMENT_TRX
                                         select old).ToList();

                    foreach (FIN_PAYMENT_VND_ALCN_DTL oldfinCrDrNoteTaxHdrObj in oldfinAdjnListObj)
                    {
                        finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == InvoicePk);
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


                    foreach (FIN_PAYMENT_VND_ALCN_DTL finAdjnObj in finAdjnHdr)
                    {
                        IsdrCrNote = false;
                        Amount = 0;
                        Old_Amount = 0;
                        //check Payment Mpg master pk is zero,save Payment Mpg master as new record
                        if (finAdjnObj.PAD_PK == 0)
                        {

                            FIN_PAYMENT_VND_ALCN_DTL objAdjn = new FIN_PAYMENT_VND_ALCN_DTL();
                            //Set next Payment Mpg pk
                            objAdjn.PAD_PK = (long)maxAdjnPK;
                            objAdjn.PAD_ALCN_PAYMENT_TRX = finAdjnObj.PAD_ALCN_PAYMENT_TRX == 0 ? null : finAdjnObj.PAD_ALCN_PAYMENT_TRX;
                            objAdjn.PAD_ALCN_CDH = finAdjnObj.PAD_ALCN_CDH == 0 ? null : finAdjnObj.PAD_ALCN_CDH;
                            objAdjn.PAD_ACTIVE = finAdjnObj.PAD_ACTIVE;
                            objAdjn.PAD_AMOUNT = finAdjnObj.PAD_AMOUNT;
                            objAdjn.PAD_PAYMENT_TRX = finAdjnObj.PAD_PAYMENT_TRX;
                            //Add new Payment Mpg to the db context
                            //FIN_RECEIPT_ADJN_List.Add(finAdjnObj);
                            currentEntity.FIN_PAYMENT_VND_ALCN_DTL.AddObject(objAdjn);

                            //Add new Payment Mpg to the db context
                            FIN_PAYMENT_ADJN_List.Add(objAdjn);
                            if (objAdjn.PAD_ALCN_CDH != null)
                            {
                                IsdrCrNote = true;
                            }
                            Amount = objAdjn.PAD_AMOUNT;

                            maxAdjnPK++;
                            Maxid = maxAdjnPK;
                            retval = objAdjn.PAD_PK;
                        }
                        //updating Payment Mpg details
                        else
                        {
                            //Get current Payment Mpg master details using Payment Mpg master pk
                            oldfinAdjnTaxObj = currentEntity.FIN_PAYMENT_VND_ALCN_DTL.Single(x => x.PAD_PK == finAdjnObj.PAD_PK);
                            if (oldfinAdjnTaxObj != null)
                            {
                                Old_Amount = oldfinAdjnTaxObj.PAD_AMOUNT;
                                if (oldfinAdjnTaxObj.PAD_ALCN_CDH != null)
                                {
                                    IsdrCrNote = true;
                                }
                                //if (oldfinAdjnTaxObj.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_STATUS > 0)
                                //{
                                //    Amount = finAdjnObj.PAD_AMOUNT - oldfinAdjnTaxObj.PAD_AMOUNT;
                                //}
                                //else
                                //{
                                    Amount = finAdjnObj.PAD_AMOUNT;
                                //}

                                //oldfinAdjnTaxObj.PAD_PK = finAdjnObj.PAD_PK;
                                //oldfinAdjnTaxObj.PAD_PAYMENT_TRX = PVM_PK;
                                //oldfinAdjnTaxObj.PAD_ALCN_CDH = finAdjnObj.PAD_ALCN_CDH == 0 ? null : finAdjnObj.PAD_ALCN_CDH;
                                //oldfinAdjnTaxObj.PAD_ALCN_PAYMENT_TRX = finAdjnObj.PAD_ALCN_PAYMENT_TRX == 0 ? null : finAdjnObj.PAD_ALCN_PAYMENT_TRX;
                                oldfinAdjnTaxObj.PAD_AMOUNT = finAdjnObj.PAD_AMOUNT;
                                //oldfinAdjnTaxObj.PAD_ACTIVE = finAdjnObj.PAD_ACTIVE;

                                retval = finAdjnObj.PAD_PK;
                            }
                        }

                        // Updating invoice
                        finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == InvoicePk);
                        if (finInvoiceVndHdrObj != null)
                        {
                           
                            //if (finAdjnObj.PAD_PK != 0)
                            //{
                                if (IsdrCrNote)
                                {
                                    //Updating Cr/Dr amount corresponding to invoice
                                    finInvoiceVndHdrObj.IVH_AMOUNT_DN_TC -= Old_Amount;
                                    finInvoiceVndHdrObj.IVH_AMOUNT_DN_TC += Amount;
                                }
                                else
                                {
                                    //Updating payment amount corresponding to invoice
                                    finInvoiceVndHdrObj.IVH_AMOUNT_PAID_TC -= Old_Amount;
                                    finInvoiceVndHdrObj.IVH_AMOUNT_PAID_TC += Amount;
                                }
                            //}
                            //else
                            //{
                            //    if (IsdrCrNote)
                            //    {
                            //        // Updating Cr/Dr amount corresponding to invoice
                            //        finInvoiceVndHdrObj.IVH_AMOUNT_DN_TC += Amount;
                            //    }
                            //    else
                            //    {
                            //        // Updating receipt amount corresponding to invoice
                            //        finInvoiceVndHdrObj.IVH_AMOUNT_PAID_TC += Amount;
                            //    }
                            //}

                        }
                    }
                    //FIN_PAYMENT_ADJN_List.ForEach(dtl => currentEntity.FIN_PAYMENT_VND_ALCN_DTL.AddObject(dtl));
                    //return Payment Mpg master pk
                }
                return retval;
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }


        public long? SavePaymentCrdrAllocation(List<FIN_PAYMENT_VND_CRDR_MPG> finPaymentCrdrAlcnList, long PVM_PK, ref  long? Maxid)
        {
            long retval;
            //Holds last Payment CR/DR Mpg master pk
            long? maxAdjnPK;
            maxAdjnPK = Maxid;
           
            //On update holds old Payment CR/DR Mpg master details
            FIN_PAYMENT_VND_CRDR_MPG oldfinCrdrAlcnObj;
            List<FIN_PAYMENT_VND_CRDR_MPG> oldfinCrdrAlcnListObj;            
            try
            {
                retval = 0;
                if (finPaymentCrdrAlcnList.Count > 0)
                {   
                 
                    List<long> pks = (from old1 in finPaymentCrdrAlcnList
                                      select old1.PNM_PK).ToList();
                    long PaymentPk = finPaymentCrdrAlcnList[0].PNM_PAYMENT_HDR;
                    oldfinCrdrAlcnListObj = (from old in this.currentEntity.FIN_PAYMENT_VND_CRDR_MPG
                                             where !pks.Contains(old.PNM_PK) && old.PNM_PAYMENT_HDR == PaymentPk
                                             && PVM_PK == old.PNM_PAYMENT_TRX_MPG
                                         select old).ToList();

                    #region Delete old payment credit note allocations
                    if (oldfinCrdrAlcnListObj != null)
                    {
                        foreach (FIN_PAYMENT_VND_CRDR_MPG oldfinCrDrNoteTaxHdrObj in oldfinCrdrAlcnListObj)
                        {
                            this.currentEntity.FIN_PAYMENT_VND_CRDR_MPG.DeleteObject(oldfinCrDrNoteTaxHdrObj);
                        }
                    } 
                    #endregion

                    foreach (FIN_PAYMENT_VND_CRDR_MPG finCrdrAlcnObj in finPaymentCrdrAlcnList)
                    {                        
                        //check Payment CR/DR Mpg master pk is zero,save Payment CR/DR Mpg master as new record
                        if (finCrdrAlcnObj.PNM_PK == 0)
                        {                           
                            //Set next Payment CR/DR Mpg pk
                            finCrdrAlcnObj.PNM_PK = (long)maxAdjnPK;                            
                            //Add new Payment Mpg to the db context                           
                            currentEntity.FIN_PAYMENT_VND_CRDR_MPG.AddObject(finCrdrAlcnObj);
                            maxAdjnPK++;                           
                            retval = finCrdrAlcnObj.PNM_PK;
                        }
                        //updating Payment CR/DR Mpg details
                        else
                        {
                            //Get current Payment CR/DR Mpg master details using Payment CR/DR Mpg master pk
                            oldfinCrdrAlcnObj = currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Single(x => x.PNM_PK == finCrdrAlcnObj.PNM_PK);
                            if (oldfinCrdrAlcnObj != null)
                            {

                                oldfinCrdrAlcnObj.PNM_ADJ_AMOUNT = finCrdrAlcnObj.PNM_ADJ_AMOUNT;
                                oldfinCrdrAlcnObj.PNM_PAID_AMOUNT = finCrdrAlcnObj.PNM_PAID_AMOUNT;
                                //oldfinCrdrAlcnObj.PNM_ACTIVE = finCrdrAlcnObj.PNM_ACTIVE;
                                //oldfinCrdrAlcnObj.PNM_CRDR_HDR = finCrdrAlcnObj.PNM_CRDR_HDR;
                                //oldfinCrdrAlcnObj.PNM_CRDR_MPG = finCrdrAlcnObj.PNM_CRDR_MPG;
                                //oldfinCrdrAlcnObj.PNM_PAYMENT_HDR = finCrdrAlcnObj.PNM_PAYMENT_HDR;
                                //oldfinCrdrAlcnObj.PNM_PAYMENT_TRX_MPG = finCrdrAlcnObj.PNM_PAYMENT_TRX_MPG;
                                retval = finCrdrAlcnObj.PNM_PK;
                            }
                        }                       
                    }                   
                }
                Maxid = retval + 1;
                return retval;
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
