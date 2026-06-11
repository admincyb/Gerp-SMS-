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
    public class FinReceiptAllocationAdjnManager : IFinReceiptAllocationAdjnManager
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

        public FinReceiptAllocationAdjnManager(ERPEntities currentEntity)
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

        public long? SaveReceiptAdjn(List<FIN_RECEIPT_CUS_ALCN_DTL> finAdjnHdr, ref  long? Maxid, long RCM_PK,long? InvoicePk)
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
            FIN_RECEIPT_CUS_ALCN_DTL oldfinAdjnTaxObj;
            List<FIN_RECEIPT_CUS_ALCN_DTL> oldfinAdjnListObj;
            List<FIN_RECEIPT_CUS_ALCN_DTL> FIN_RECEIPT_ADJN_List = new List<FIN_RECEIPT_CUS_ALCN_DTL>();
            FIN_INVOICE_CUS_HDR finInvoiceVndHdrObj;

            try
            {
                retval = 0;
                if (finAdjnHdr.Count > 0)
                { 
                    //Set save status zero,save failed
                     
                    //maxAdjnPK = currentEntity.FIN_RECEIPT_CUS_ALCN_DTL.Max(v => (int?)v.RAD_PK);
                    //maxAdjnPK = (maxAdjnPK.HasValue) ? maxAdjnPK.Value + 1 : 1;

                    finAdjnHdr = (from c in finAdjnHdr where c.RAD_AMOUNT > 0 select c).ToList();
                    List<long> pks = (from old1 in finAdjnHdr
                                      select old1.RAD_PK).ToList();


                    oldfinAdjnListObj = (from old in this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL
                                         where !pks.Contains(old.RAD_PK) && RCM_PK == old.RAD_RECEIPT_TRX
                                         select old).ToList();

                    foreach (FIN_RECEIPT_CUS_ALCN_DTL oldfinCrDrNoteTaxHdrObj in oldfinAdjnListObj)
                    {
                        finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == InvoicePk);
                        if (finInvoiceVndHdrObj != null)
                        {
                            if (oldfinCrDrNoteTaxHdrObj.RAD_ALCN_CDH != null)
                            {
                                finInvoiceVndHdrObj.ICH_AMOUNT_CN_TC -= oldfinCrDrNoteTaxHdrObj.RAD_AMOUNT;
                            }
                            else
                            {
                                finInvoiceVndHdrObj.ICH_AMOUNT_RCVD_TC -= oldfinCrDrNoteTaxHdrObj.RAD_AMOUNT;
                            }
                        }
                        this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL.DeleteObject(oldfinCrDrNoteTaxHdrObj);
                    }


                    foreach (FIN_RECEIPT_CUS_ALCN_DTL finAdjnObj in finAdjnHdr)
                    {
                        Amount = 0;
                        Old_Amount = 0;
                        IsdrCrNote = false;
                        //check Payment Mpg master pk is zero,save Payment Mpg master as new record
                        if (finAdjnObj.RAD_PK == 0)
                        {
                            FIN_RECEIPT_CUS_ALCN_DTL objAdjn = new FIN_RECEIPT_CUS_ALCN_DTL();
                            //Set next Payment Mpg pk
                            objAdjn.RAD_PK = (long)maxAdjnPK;
                            //finAdjnObj.RAD_RECEIPT_TRX = RCM_PK;
                            objAdjn.RAD_ALCN_RECEIPT_TRX = finAdjnObj.RAD_ALCN_RECEIPT_TRX == 0 ? null : finAdjnObj.RAD_ALCN_RECEIPT_TRX;
                            objAdjn.RAD_ALCN_CDH = finAdjnObj.RAD_ALCN_CDH == 0 ? null : finAdjnObj.RAD_ALCN_CDH;
                            objAdjn.RAD_ACTIVE = finAdjnObj.RAD_ACTIVE;
                            objAdjn.RAD_AMOUNT = finAdjnObj.RAD_AMOUNT;
                            objAdjn.RAD_RECEIPT_TRX = finAdjnObj.RAD_RECEIPT_TRX;
                            //Add new Payment Mpg to the db context
                            //FIN_RECEIPT_ADJN_List.Add(finAdjnObj);
                            currentEntity.FIN_RECEIPT_CUS_ALCN_DTL.AddObject(objAdjn);

                            if (objAdjn.RAD_ALCN_CDH != null)
                            {
                                IsdrCrNote = true;
                            }
                            Amount = objAdjn.RAD_AMOUNT;

                            maxAdjnPK++;
                            retval = objAdjn.RAD_PK;
                            Maxid = maxAdjnPK;
                        }
                        //updating Payment Mpg details
                        else
                        {
                            //Get current Payment Mpg master details using Payment Mpg master pk
                            oldfinAdjnTaxObj = currentEntity.FIN_RECEIPT_CUS_ALCN_DTL.Single(x => x.RAD_PK == finAdjnObj.RAD_PK);
                            if (oldfinAdjnTaxObj != null)
                            {
                                Old_Amount =oldfinAdjnTaxObj.RAD_AMOUNT;
                                if (oldfinAdjnTaxObj.RAD_ALCN_CDH != null)
                                {
                                    IsdrCrNote = true;                                   
                                }
                                //if (oldfinAdjnTaxObj.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_STATUS > 0)
                                //{
                                //    Amount = finAdjnObj.RAD_AMOUNT - oldfinAdjnTaxObj.RAD_AMOUNT;
                                //}
                                //else
                                //{
                                    Amount = finAdjnObj.RAD_AMOUNT;
                                //}

                                //oldfinAdjnTaxObj.RAD_PK = finAdjnObj.RAD_PK;
                                //oldfinAdjnTaxObj.RAD_RECEIPT_TRX = RCM_PK;
                                //oldfinAdjnTaxObj.RAD_ALCN_CDH = finAdjnObj.RAD_ALCN_CDH == 0 ? null : finAdjnObj.RAD_ALCN_CDH;
                                //oldfinAdjnTaxObj.RAD_ALCN_RECEIPT_TRX = finAdjnObj.RAD_ALCN_RECEIPT_TRX == 0 ? null : finAdjnObj.RAD_ALCN_RECEIPT_TRX;
                                oldfinAdjnTaxObj.RAD_AMOUNT = finAdjnObj.RAD_AMOUNT;
                                //oldfinAdjnTaxObj.RAD_ACTIVE = finAdjnObj.RAD_ACTIVE;

                                retval = finAdjnObj.RAD_PK;
                                //Maxid = maxAdjnPK;
                            }
                        }
                        // Updating invoice
                        finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == InvoicePk);
                        if (finInvoiceVndHdrObj != null)
                        {
                            //if (finAdjnObj.RAD_PK != 0)
                            //{
                                if (IsdrCrNote)
                                {
                                    // Updating Cr/Dr amount corresponding to invoice

                                    finInvoiceVndHdrObj.ICH_AMOUNT_CN_TC -= Old_Amount;
                                    finInvoiceVndHdrObj.ICH_AMOUNT_CN_TC += Amount;
                                }
                                else
                                {
                                    // Updating receipt amount corresponding to invoice
                                    finInvoiceVndHdrObj.ICH_AMOUNT_RCVD_TC -= Old_Amount;
                                    finInvoiceVndHdrObj.ICH_AMOUNT_RCVD_TC += Amount;
                                }
                            //}
                            //else
                            //{
                            //    if (IsdrCrNote)
                            //    {
                            //        // Updating Cr/Dr amount corresponding to invoice
                            //        finInvoiceVndHdrObj.ICH_AMOUNT_CN_TC += Amount;
                            //    }
                            //    else
                            //    {
                            //        // Updating receipt amount corresponding to invoice
                            //        finInvoiceVndHdrObj.ICH_AMOUNT_RCVD_TC += Amount;
                            //    }
                            //}
                        }
                    }
                    //FIN_RECEIPT_ADJN_List.ForEach(dtl => currentEntity.FIN_RECEIPT_CUS_ALCN_DTL.AddObject(dtl));
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


        public long? SaveReceiptCrdrAllocation(List<FIN_RECEIPT_CUS_CRDR_MPG> finReceiptCrdrAlcnList,long RCM_PK, ref long? Maxid)
        {
            long retval;
            //Holds last Payment CR/DR Mpg master pk
            long? maxAdjnPK;
            maxAdjnPK = Maxid;

            //On update holds old Payment CR/DR Mpg master details
            FIN_RECEIPT_CUS_CRDR_MPG oldfinCrdrAlcnObj;
            List<FIN_RECEIPT_CUS_CRDR_MPG> oldfinCrdrAlcnListObj;
            try
            {
                retval = 0;
                if (finReceiptCrdrAlcnList.Count > 0)
                {

                    List<long> pks = (from old1 in finReceiptCrdrAlcnList
                                      select old1.RNM_PK).ToList();
                    long ReceiptPk = finReceiptCrdrAlcnList[0].RNM_RECEIPT_HDR;
                    oldfinCrdrAlcnListObj = (from old in this.currentEntity.FIN_RECEIPT_CUS_CRDR_MPG
                                             where !pks.Contains(old.RNM_PK) && old.RNM_RECEIPT_HDR == ReceiptPk
                                             && RCM_PK == old.RNM_RECEIPT_TRX_MPG
                                             select old).ToList();

                    #region Delete old payment credit note allocations
                    if (oldfinCrdrAlcnListObj != null)
                    {
                        foreach (FIN_RECEIPT_CUS_CRDR_MPG oldfinCrDrNoteTaxHdrObj in oldfinCrdrAlcnListObj)
                        {
                            this.currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.DeleteObject(oldfinCrDrNoteTaxHdrObj);
                        }
                    }
                    #endregion

                    foreach (FIN_RECEIPT_CUS_CRDR_MPG finCrdrAlcnObj in finReceiptCrdrAlcnList)
                    {
                        //check Payment CR/DR Mpg master pk is zero,save Payment CR/DR Mpg master as new record
                        if (finCrdrAlcnObj.RNM_PK == 0)
                        {
                            //Set next Payment CR/DR Mpg pk
                            finCrdrAlcnObj.RNM_PK = (long)maxAdjnPK;
                            //Add new Payment Mpg to the db context                           
                            currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.AddObject(finCrdrAlcnObj);
                            maxAdjnPK++;
                            retval = finCrdrAlcnObj.RNM_PK;
                        }
                        //updating Payment CR/DR Mpg details
                        else
                        {
                            //Get current Payment CR/DR Mpg master details using Payment CR/DR Mpg master pk
                            oldfinCrdrAlcnObj = currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Single(x => x.RNM_PK == finCrdrAlcnObj.RNM_PK);
                            if (oldfinCrdrAlcnObj != null)
                            {

                                oldfinCrdrAlcnObj.RNM_ADJ_AMOUNT = finCrdrAlcnObj.RNM_ADJ_AMOUNT;
                                oldfinCrdrAlcnObj.RNM_PAID_AMOUNT = finCrdrAlcnObj.RNM_PAID_AMOUNT;
                                //oldfinCrdrAlcnObj.RNM_ACTIVE = finCrdrAlcnObj.RNM_ACTIVE;
                                //oldfinCrdrAlcnObj.RNM_CRDR_HDR = finCrdrAlcnObj.RNM_CRDR_HDR;
                                //oldfinCrdrAlcnObj.RNM_CRDR_MPG = finCrdrAlcnObj.RNM_CRDR_MPG;
                                //oldfinCrdrAlcnObj.RNM_RECEIPT_HDR = finCrdrAlcnObj.RNM_RECEIPT_HDR;
                                //oldfinCrdrAlcnObj.RNM_RECEIPT_TRX_MPG = finCrdrAlcnObj.RNM_RECEIPT_TRX_MPG;
                                retval = finCrdrAlcnObj.RNM_PK;
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
