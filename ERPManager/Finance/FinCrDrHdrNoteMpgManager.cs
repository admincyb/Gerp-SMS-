using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;

namespace ERPManager
{
    public class FinCrDrHdrNoteMpgManager:IFinCrDrHdrNoteMpgManager 
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// FinCrDrHdr Maping Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public FinCrDrHdrNoteMpgManager(ERPEntities currentEntity)
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
        /// Save Mapping details
        /// </summary>
        /// <param name="finCrDrNoteMpgList"></param>
        /// <returns></returns>
        public long? SaveFinCrDrNoteMpg(List<FIN_CRDR_NOTE_MPG> finCrDrNoteMpgList)
        {
            //Holds save status
            long retval;
            //Holds last Payment Mpg master pk
            long? maxCrDrNoteMpgPK;
            decimal Amount = 0;
            long CrDrNoteHdr = 0;
            long? maxID;
            long? maxTaxHdrId=null;
            //On update holds old Payment Mpg master details
            FIN_CRDR_NOTE_MPG oldfinCrDrNoteMpgObj;
            FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;
            FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj;

            List<FIN_CRDR_NOTE_MPG> oldfinCrDrNoteMpgListObj;

            List<FIN_CRDR_NOTE_DTL> finReceiptCusSoMpgList;
           
            FinCrDrNoteDtlManager finReceiptCusSOMpgManagerObj;
            List<FIN_CRDR_NOTE_TAX_DTL> finTaxList;
            FinCrDrNoteTaxManager finCrDrTaxManagerObj;
            

            try
            {
                //Set save status zero,save failed
                retval = 0;
                //Getting last Payment Mpg pk
                maxCrDrNoteMpgPK = currentEntity.FIN_CRDR_NOTE_MPG.Max(v => (int?)v.CDM_PK);
                maxCrDrNoteMpgPK = (maxCrDrNoteMpgPK.HasValue) ? maxCrDrNoteMpgPK.Value + 1 : 1;


                long? maxTaxID = currentEntity.FIN_CRDR_NOTE_TAX_DTL.Max(v => (long?)v.NTD_PK);
                maxTaxID = (maxTaxID.HasValue) ? maxTaxID.Value + 1 : 1;


                //Iterate through vedor list for save
                maxID = currentEntity.FIN_CRDR_NOTE_DTL.Max(v => (long?)v.CDS_PK);
                if (finCrDrNoteMpgList.Count > 0)
                {
                    List<long> pks = (from old1 in finCrDrNoteMpgList
                                      select old1.CDM_PK).ToList();

                    CrDrNoteHdr = finCrDrNoteMpgList[0].CDM_CRDR_NOTE_HDR;

                    oldfinCrDrNoteMpgListObj = (from old in this.currentEntity.FIN_CRDR_NOTE_MPG
                                                where CrDrNoteHdr == old.CDM_CRDR_NOTE_HDR
                                                   && !pks.Contains(old.CDM_PK)
                                                select old).ToList();

                    foreach (FIN_CRDR_NOTE_MPG oldfinCrDrNoteHdrObjObject in oldfinCrDrNoteMpgListObj)
                    {
                        //finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == oldfinCrDrNoteHdrObjObject.CDM_CRDR_NOTE_HDR);

                        //if (finInvoiceVndHdrObj != null)
                        //{
                        //    finInvoiceVndHdrObj.IVH_AMOUNT_PAID_TC -= oldfinCrDrNoteHdrObjObject.CDM_AMOUNT;
                        //}

                        try
                        {
                            //Delete Allocation details
                            List<FIN_CRDR_NOTE_DTL> fin_FIN_CRDR_NOTE_DTL_List_Obj = oldfinCrDrNoteHdrObjObject.FIN_CRDR_NOTE_DTL.ToList();
                            foreach (FIN_CRDR_NOTE_DTL fin_FIN_CRDR_NOTE_DTL_obj in fin_FIN_CRDR_NOTE_DTL_List_Obj)
                            {

                                //delete details tax split up
                                List<FIN_CRDR_NOTE_TAX_DTL> fin_TaxDet_List_Obj = fin_FIN_CRDR_NOTE_DTL_obj.FIN_CRDR_NOTE_TAX_DTL.ToList();
                                if (fin_TaxDet_List_Obj != null && fin_TaxDet_List_Obj.Count > 0)
                                {
                                    foreach (FIN_CRDR_NOTE_TAX_DTL finTaxObj in fin_TaxDet_List_Obj)
                                    {
                                        FIN_CRDR_NOTE_TAX_DTL obj_TAX_DTL = currentEntity.FIN_CRDR_NOTE_TAX_DTL.SingleOrDefault(tax => tax.NTD_PK == finTaxObj.NTD_PK);
                                        this.currentEntity.FIN_CRDR_NOTE_TAX_DTL.DeleteObject(obj_TAX_DTL);
                                    }
                                }
                                //////////////
                                this.currentEntity.FIN_CRDR_NOTE_DTL.DeleteObject(fin_FIN_CRDR_NOTE_DTL_obj);
                            }

                            //delete header tax split up
                            List<FIN_CRDR_NOTE_TAX_DTL> fin_TaxHdr_List_Obj = oldfinCrDrNoteHdrObjObject.FIN_CRDR_NOTE_TAX_DTL.ToList();
                            if (fin_TaxHdr_List_Obj != null && fin_TaxHdr_List_Obj.Count > 0)
                            {
                                foreach (FIN_CRDR_NOTE_TAX_DTL finTaxObj in fin_TaxHdr_List_Obj)
                                {
                                    FIN_CRDR_NOTE_TAX_DTL obj_TAX_DTL = currentEntity.FIN_CRDR_NOTE_TAX_DTL.SingleOrDefault(tax => tax.NTD_PK == finTaxObj.NTD_PK);
                                    this.currentEntity.FIN_CRDR_NOTE_TAX_DTL.DeleteObject(obj_TAX_DTL);
                                }
                            }

                            // Delete mapping
                            this.currentEntity.FIN_CRDR_NOTE_MPG.DeleteObject(oldfinCrDrNoteHdrObjObject);
                        }
                        catch { }
                    }
                }

                foreach (FIN_CRDR_NOTE_MPG finCrDrNoteMpgObj in finCrDrNoteMpgList)
                {
                    
                    finReceiptCusSoMpgList = finCrDrNoteMpgObj.FIN_CRDR_NOTE_DTL == null ?
                        new List<FIN_CRDR_NOTE_DTL>() : finCrDrNoteMpgObj.FIN_CRDR_NOTE_DTL.ToList();
                    finCrDrNoteMpgObj.FIN_CRDR_NOTE_DTL.Clear();

                    finTaxList = finCrDrNoteMpgObj.FIN_CRDR_NOTE_TAX_DTL == null ?
                        new List<FIN_CRDR_NOTE_TAX_DTL>() : finCrDrNoteMpgObj.FIN_CRDR_NOTE_TAX_DTL.ToList();
                    finCrDrNoteMpgObj.FIN_CRDR_NOTE_TAX_DTL.Clear();

                    //check Payment Mpg master pk is zero,save Payment Mpg master as new record
                    if (finCrDrNoteMpgObj.CDM_PK == 0)
                    {
                        //Set next Payment Mpg pk
                        finCrDrNoteMpgObj.CDM_PK = (long)maxCrDrNoteMpgPK;
                        //Add new Payment Mpg to the db context
                        currentEntity.FIN_CRDR_NOTE_MPG.AddObject(finCrDrNoteMpgObj);
                        maxCrDrNoteMpgPK++;
                        retval = finCrDrNoteMpgObj.CDM_PK;
                        Amount = finCrDrNoteMpgObj.CDM_AMOUNT;

                        if (finTaxList.Count > 0)
                        {
                            finTaxList.ForEach(dtl =>
                            {
                                dtl.NTD_CRDR_DTL = null;
                                dtl.NTD_CRDR_MPG = finCrDrNoteMpgObj.CDM_PK;
                            });
                            finCrDrTaxManagerObj = new FinCrDrNoteTaxManager(this.currentEntity);
                            maxTaxHdrId = finCrDrTaxManagerObj.SaveCRDRTaxHdrSplit(finTaxList, ref maxTaxID);
                            //maxTaxHdrId = SaveCRDRTaxHdrSplit(finTaxList, maxTaxID);
                        }

                        if (finReceiptCusSoMpgList.Count > 0)
                        {
                            finReceiptCusSoMpgList.ForEach(dtl =>
                            {
                                dtl.CDS_CRDR_NOTE_HDR = finCrDrNoteMpgObj.CDM_CRDR_NOTE_HDR;
                                dtl.CDS_CRDR_NOTE_MPG = finCrDrNoteMpgObj.CDM_PK;
                            });
                            finReceiptCusSOMpgManagerObj = new FinCrDrNoteDtlManager(this.currentEntity);
                            finReceiptCusSOMpgManagerObj.SaveCRDRSplit(finReceiptCusSoMpgList, ref maxTaxID, ref maxID);
                            //soMpgPK = finReceiptCusSOMpgManagerObj.SaveReceiptSplit(finReceiptCusSoMpgList, (long)soMpgPK);
                            //soMpgPK = soMpgPK == null ? 0 : soMpgPK;
                        }
                        

                    }
                    //updating Payment Mpg details
                    else
                    {
                        //Get current Payment Mpg master details using Payment Mpg master pk
                        oldfinCrDrNoteMpgObj = currentEntity.FIN_CRDR_NOTE_MPG.SingleOrDefault(v => v.CDM_PK == finCrDrNoteMpgObj.CDM_PK);
                        if (oldfinCrDrNoteMpgObj != null)
                        {
                            Amount = finCrDrNoteMpgObj.CDM_AMOUNT - oldfinCrDrNoteMpgObj.CDM_AMOUNT;
                            //Update Payment Mpg details
                            oldfinCrDrNoteMpgObj.CDM_AMOUNT = finCrDrNoteMpgObj.CDM_AMOUNT;
                            oldfinCrDrNoteMpgObj.CDM_ACTIVE = finCrDrNoteMpgObj.CDM_ACTIVE;
                            oldfinCrDrNoteMpgObj.CDM_TAX_AMOUNT = finCrDrNoteMpgObj.CDM_TAX_AMOUNT;
                            oldfinCrDrNoteMpgObj.CDM_SHIP_CHARGE = finCrDrNoteMpgObj.CDM_SHIP_CHARGE;
                            oldfinCrDrNoteMpgObj.CDM_OTHER_CHARGE = finCrDrNoteMpgObj.CDM_OTHER_CHARGE;
                            //Set return value as Payment Mpg pk                        
                            retval = finCrDrNoteMpgObj.CDM_PK;
                            if (finTaxList.Count > 0)
                            {
                                finTaxList.ForEach(dtl =>
                                {
                                    dtl.NTD_CRDR_DTL = null;
                                    dtl.NTD_CRDR_MPG = finCrDrNoteMpgObj.CDM_PK;
                                });
                                finCrDrNoteMpgObj.FIN_CRDR_NOTE_TAX_DTL.Clear();
                                finCrDrTaxManagerObj = new FinCrDrNoteTaxManager(this.currentEntity);
                                maxTaxHdrId=finCrDrTaxManagerObj.SaveCRDRTaxHdrSplit(finTaxList, ref maxTaxID);
                                //maxTaxHdrId = SaveCRDRTaxHdrSplit(finTaxList, maxTaxID);
                            }
                            if (finReceiptCusSoMpgList.Count > 0)
                            {
                                finReceiptCusSoMpgList.ForEach(dtl =>
                                {
                                    dtl.CDS_CRDR_NOTE_HDR = finCrDrNoteMpgObj.CDM_CRDR_NOTE_HDR;
                                    dtl.CDS_CRDR_NOTE_MPG = finCrDrNoteMpgObj.CDM_PK;
                                });
                                finReceiptCusSOMpgManagerObj = new FinCrDrNoteDtlManager(this.currentEntity);
                                finReceiptCusSOMpgManagerObj.SaveCRDRSplit(finReceiptCusSoMpgList, ref maxTaxID, ref maxID);
                            }
                           
                        }
                    }
                   

                }
                //return Payment Mpg master pk
                return retval;
            }
            /*catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }*/
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        /// <summary>
        /// Save Mapping details
        /// </summary>
        /// <param name="finCrDrNoteMpgList"></param>
        /// <returns></returns>
        public long? SaveFinCrDrNoteMpg(List<FIN_CRDR_NOTE_MPG> finCrDrNoteMpgList,byte CrDrType)
        {
            //Holds save status
            long retval;
            //Holds last Payment Mpg master pk
            long? maxCrDrNoteMpgPK;
            decimal Amount = 0;
            long CrDrNoteHdr = 0;
            long? maxID;
            long? maxTaxHdrId = null;
            //On update holds old Payment Mpg master details
            FIN_CRDR_NOTE_MPG oldfinCrDrNoteMpgObj;
            FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;
            FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj;

            List<FIN_CRDR_NOTE_MPG> oldfinCrDrNoteMpgListObj;
            List<FIN_CRDR_NOTE_DTL> finReceiptCusSoMpgList;
            FinCrDrNoteDtlManager finReceiptCusSOMpgManagerObj;
            List<FIN_CRDR_NOTE_TAX_DTL> finTaxList;
            FinCrDrNoteTaxManager finCrDrTaxManagerObj;


            try
            {
                //Set save status zero,save failed
                retval = 0;
                //Getting last Payment Mpg pk
                maxCrDrNoteMpgPK = currentEntity.FIN_CRDR_NOTE_MPG.Max(v => (int?)v.CDM_PK);
                maxCrDrNoteMpgPK = (maxCrDrNoteMpgPK.HasValue) ? maxCrDrNoteMpgPK.Value + 1 : 1;
                maxID = currentEntity.FIN_CRDR_NOTE_DTL.Max(v => (long?)v.CDS_PK);

                long? maxTaxID = currentEntity.FIN_CRDR_NOTE_TAX_DTL.Max(v => (long?)v.NTD_PK);
                maxTaxID = (maxTaxID.HasValue) ? maxTaxID.Value + 1 : 1;

                //Iterate through vedor list for save

                if (finCrDrNoteMpgList.Count > 0)
                {
                    List<long> pks = (from old1 in finCrDrNoteMpgList
                                      select old1.CDM_PK).ToList();

                    CrDrNoteHdr = finCrDrNoteMpgList[0].CDM_CRDR_NOTE_HDR;

                    oldfinCrDrNoteMpgListObj = (from old in this.currentEntity.FIN_CRDR_NOTE_MPG
                                                     where CrDrNoteHdr == old.CDM_CRDR_NOTE_HDR
                                                        && !pks.Contains(old.CDM_PK)
                                                     select old).ToList();

                    foreach (FIN_CRDR_NOTE_MPG oldfinCrDrNoteHdrObjObject in oldfinCrDrNoteMpgListObj)
                    {
                        //vendor
                        finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == oldfinCrDrNoteHdrObjObject.CDM_INVOICE_VND_HDR);

                        if (finInvoiceVndHdrObj != null)
                        {
                            if (CrDrType == 1)
                            {
                                ////finInvoiceVndHdrObj.IVH_AMOUNT_DN_TC -= oldfinCrDrNoteHdrObjObject.CDM_AMOUNT;
                            }
                            else
                            {
                                finInvoiceVndHdrObj.IVH_AMOUNT_CN_TC -= oldfinCrDrNoteHdrObjObject.CDM_AMOUNT;
                            }
                        }
                        //Customer
                        finInvoiceCusHdrObj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == oldfinCrDrNoteHdrObjObject.CDM_INVOICE_CUS_HDR);

                        if (finInvoiceCusHdrObj != null)
                        {
                            if (CrDrType == 1)
                            {
                                finInvoiceCusHdrObj.ICH_AMOUNT_DN_TC -= (oldfinCrDrNoteHdrObjObject.CDM_AMOUNT + oldfinCrDrNoteHdrObjObject.CDM_SHIP_CHARGE + oldfinCrDrNoteHdrObjObject.CDM_OTHER_CHARGE);
                            }
                            else
                            {
                                ////finInvoiceCusHdrObj.ICH_AMOUNT_CN_TC -= oldfinCrDrNoteHdrObjObject.CDM_AMOUNT;
                            }
                        }
                    }
                }

                foreach (FIN_CRDR_NOTE_MPG finCrDrNoteMpgObj in finCrDrNoteMpgList)
                {
                    finReceiptCusSoMpgList = finCrDrNoteMpgObj.FIN_CRDR_NOTE_DTL == null ?
                    new List<FIN_CRDR_NOTE_DTL>() : finCrDrNoteMpgObj.FIN_CRDR_NOTE_DTL.ToList();
                    finCrDrNoteMpgObj.FIN_CRDR_NOTE_DTL.Clear();

                    finTaxList = finCrDrNoteMpgObj.FIN_CRDR_NOTE_TAX_DTL == null ?
                        new List<FIN_CRDR_NOTE_TAX_DTL>() : finCrDrNoteMpgObj.FIN_CRDR_NOTE_TAX_DTL.ToList();
                    finCrDrNoteMpgObj.FIN_CRDR_NOTE_TAX_DTL.Clear();

                    //check Payment Mpg master pk is zero,save Payment Mpg master as new record
                    if (finCrDrNoteMpgObj.CDM_PK == 0)
                    {
                        //Set next Payment Mpg pk
                        finCrDrNoteMpgObj.CDM_PK = (long)maxCrDrNoteMpgPK;
                        //Add new Payment Mpg to the db context
                        currentEntity.FIN_CRDR_NOTE_MPG.AddObject(finCrDrNoteMpgObj);
                        maxCrDrNoteMpgPK++;
                        retval = finCrDrNoteMpgObj.CDM_PK;
                        Amount = finCrDrNoteMpgObj.CDM_AMOUNT + finCrDrNoteMpgObj.CDM_SHIP_CHARGE + finCrDrNoteMpgObj.CDM_OTHER_CHARGE;
                        if (finTaxList.Count > 0)
                        {
                            finTaxList.ForEach(dtl =>
                            {
                                dtl.NTD_CRDR_DTL = null;
                                dtl.NTD_CRDR_MPG = finCrDrNoteMpgObj.CDM_PK;
                            });
                            finCrDrTaxManagerObj = new FinCrDrNoteTaxManager(this.currentEntity);
                            maxTaxHdrId = finCrDrTaxManagerObj.SaveCRDRTaxHdrSplit(finTaxList, ref maxTaxID);
                        }
                        if (finReceiptCusSoMpgList.Count > 0)
                        {
                            finReceiptCusSoMpgList.ForEach(dtl =>
                            {
                                dtl.CDS_CRDR_NOTE_HDR = finCrDrNoteMpgObj.CDM_CRDR_NOTE_HDR;
                                dtl.CDS_CRDR_NOTE_MPG = finCrDrNoteMpgObj.CDM_PK;
                            });
                            finReceiptCusSOMpgManagerObj = new FinCrDrNoteDtlManager(this.currentEntity);
                            finReceiptCusSOMpgManagerObj.SaveCRDRSplit(finReceiptCusSoMpgList, ref maxTaxID, ref maxID);
                        }
                    }
                    //updating Payment Mpg details
                    else
                    {
                        //Get current Payment Mpg master details using Payment Mpg master pk
                        oldfinCrDrNoteMpgObj = currentEntity.FIN_CRDR_NOTE_MPG.SingleOrDefault(v => v.CDM_PK == finCrDrNoteMpgObj.CDM_PK);
                        if (oldfinCrDrNoteMpgObj != null)
                        {
                            if (oldfinCrDrNoteMpgObj.FIN_CRDR_NOTE_HDR.CDH_STATUS > 0)
                            {
                                Amount = (finCrDrNoteMpgObj.CDM_AMOUNT + finCrDrNoteMpgObj.CDM_SHIP_CHARGE + finCrDrNoteMpgObj.CDM_OTHER_CHARGE) - (oldfinCrDrNoteMpgObj.CDM_AMOUNT + oldfinCrDrNoteMpgObj.CDM_SHIP_CHARGE + oldfinCrDrNoteMpgObj.CDM_OTHER_CHARGE);
                            }
                            else
                            {
                                Amount = finCrDrNoteMpgObj.CDM_AMOUNT + finCrDrNoteMpgObj.CDM_SHIP_CHARGE + finCrDrNoteMpgObj.CDM_OTHER_CHARGE;
                            }
                            //Amount = finCrDrNoteMpgObj.CDM_AMOUNT - oldfinCrDrNoteMpgObj.CDM_AMOUNT;
                            //Update Payment Mpg details
                            //Amount = finCrDrNoteMpgObj.CDM_AMOUNT;
                            //Amount = finCrDrNoteMpgObj.CDM_AMOUNT - oldfinCrDrNoteMpgObj.CDM_AMOUNT;
                            oldfinCrDrNoteMpgObj.CDM_AMOUNT = finCrDrNoteMpgObj.CDM_AMOUNT;
                            oldfinCrDrNoteMpgObj.CDM_ACTIVE = finCrDrNoteMpgObj.CDM_ACTIVE;
                            oldfinCrDrNoteMpgObj.CDM_TAX_AMOUNT = finCrDrNoteMpgObj.CDM_TAX_AMOUNT;
                            oldfinCrDrNoteMpgObj.CDM_SHIP_CHARGE = finCrDrNoteMpgObj.CDM_SHIP_CHARGE;
                            oldfinCrDrNoteMpgObj.CDM_OTHER_CHARGE = finCrDrNoteMpgObj.CDM_OTHER_CHARGE;
                            //Set return value as Payment Mpg pk
                            retval = finCrDrNoteMpgObj.CDM_PK;
                            if (finTaxList.Count > 0)
                            {
                                finTaxList.ForEach(dtl =>
                                {
                                    dtl.NTD_CRDR_DTL = null;
                                    dtl.NTD_CRDR_MPG = finCrDrNoteMpgObj.CDM_PK;
                                });
                                finCrDrTaxManagerObj = new FinCrDrNoteTaxManager(this.currentEntity);
                                finCrDrTaxManagerObj.SaveCRDRTaxHdrSplit(finTaxList, ref maxTaxID);
                            }
                            if (finReceiptCusSoMpgList.Count > 0)
                            {
                                finReceiptCusSoMpgList.ForEach(dtl =>
                                {
                                    dtl.CDS_CRDR_NOTE_HDR = finCrDrNoteMpgObj.CDM_CRDR_NOTE_HDR;
                                    dtl.CDS_CRDR_NOTE_MPG = finCrDrNoteMpgObj.CDM_PK;
                                });
                                finReceiptCusSOMpgManagerObj = new FinCrDrNoteDtlManager(this.currentEntity);
                                finReceiptCusSOMpgManagerObj.SaveCRDRSplit(finReceiptCusSoMpgList, ref maxTaxID, ref maxID);
                            }
                        }
                    }
                    //Update Cr/Dr amount corresponding to invoice
                    finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == finCrDrNoteMpgObj.CDM_INVOICE_VND_HDR);

                    if (finInvoiceVndHdrObj != null)
                    {
                        if (CrDrType == 1)
                        {
                            ////finInvoiceVndHdrObj.IVH_AMOUNT_DN_TC += Amount;
                        }
                        else
                        {                          
                            finInvoiceVndHdrObj.IVH_AMOUNT_CN_TC += Amount;
                        }
                    }

                    //Update Cr/Dr amount corresponding to invoice
                    finInvoiceCusHdrObj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == finCrDrNoteMpgObj.CDM_INVOICE_CUS_HDR);

                    if (finInvoiceCusHdrObj != null)
                    {
                        if (CrDrType == 1)
                        {
                            finInvoiceCusHdrObj.ICH_AMOUNT_DN_TC += Amount;                            
                        }
                        else
                        {                           
                            ////finInvoiceCusHdrObj.ICH_AMOUNT_CN_TC += Amount;
                        }
                    }

                }
                //return Payment Mpg master pk
                return retval;
            }
            /*catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }*/
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finCrDrNotePK"></param>
        /// <returns></returns>
        public List<FIN_CRDR_NOTE_MPG> GetFinCrDrNoteMpg(long finCrDrNotePK)
        {
            List<FIN_CRDR_NOTE_MPG> PaymentTrxMpgListObj = null;
            try
            {
                PaymentTrxMpgListObj = (from cdm in this.currentEntity.FIN_CRDR_NOTE_MPG
                                        where cdm.CDM_CRDR_NOTE_HDR == finCrDrNotePK
                                        select cdm
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
        /// 
        /// </summary>
        /// <param name="finCrDrNotePK"></param>
        /// <returns></returns>
        public List<FIN_INVOICE_VND_HDR> GetFinCrDrNoteMpg(List<long> InvoicePK)
        {
            List<FIN_INVOICE_VND_HDR> PaymentTrxMpgListObj = null;
            try
            {
                PaymentTrxMpgListObj = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                        join vnd in this.currentEntity.PUR_VENDOR_MST on inv.IVH_VENDOR equals vnd.VEN_PK
                                        where InvoicePK.Contains(inv.IVH_PK)
                                        select inv
                                        ).ToList();
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            return PaymentTrxMpgListObj;
            throw new NotImplementedException();
        }



        public long? SaveCRDRTaxHdrSplit(List<FIN_CRDR_NOTE_TAX_DTL> finCrDrTaxList, long? maxID)
        {
            long? retval;
            retval = 0;

            //long? maxPK;

            List<FIN_CRDR_NOTE_TAX_DTL> Old_finReceiptCusSOMpgList;

            FIN_CRDR_NOTE_TAX_DTL Old_finReceiptCusSOMpg;
            FIN_INVOICE_CUS_DTL obj_SAL_ORDER_HDR;

            try
            {
                if (finCrDrTaxList.Count > 0)
                {
                    if (!maxID.HasValue || maxID.Value == 0)
                        maxID = currentEntity.FIN_CRDR_NOTE_DTL.Max(v => (long?)v.CDS_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    //maxPK = soPK == 0 ? currentEntity.FIN_RECEIPT_CUS_SO_MPG.Max(v => (long?)v.RSO_PK) : soPK;
                    //maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;

                    //  Delete Removed Entries

                    List<long> pks = (from old1 in finCrDrTaxList
                                      select old1.NTD_PK).ToList();
                    /**/
                    long NTD_CRDR_MPG = finCrDrTaxList[0].NTD_CRDR_MPG;

                    Old_finReceiptCusSOMpgList = (from oldp in this.currentEntity.FIN_CRDR_NOTE_TAX_DTL
                                                  where oldp.NTD_CRDR_MPG == NTD_CRDR_MPG
                                                  && !pks.Contains(oldp.NTD_PK) && !oldp.NTD_CRDR_DTL.HasValue
                                                  select oldp).ToList();
                    foreach (FIN_CRDR_NOTE_TAX_DTL oldfinCrDrNoteHdrObjObject in Old_finReceiptCusSOMpgList)
                    {
                        this.currentEntity.FIN_CRDR_NOTE_TAX_DTL.DeleteObject(oldfinCrDrNoteHdrObjObject);
                    }
                    foreach (FIN_CRDR_NOTE_TAX_DTL FIN_CRDR_NOTE_TAX_DTL_obj in finCrDrTaxList)
                    {
                        if (FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK == 0) //  INSERT NEW RECORD
                        {
                            FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK = maxID.Value;                            
                            currentEntity.FIN_CRDR_NOTE_TAX_DTL.AddObject(FIN_CRDR_NOTE_TAX_DTL_obj);
                            retval = maxID;
                            maxID++;
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            Old_finReceiptCusSOMpg = currentEntity.FIN_CRDR_NOTE_TAX_DTL.SingleOrDefault(a => a.NTD_PK == FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK);
                            Old_finReceiptCusSOMpg.NTD_AMOUNT = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_AMOUNT;
                            Old_finReceiptCusSOMpg.NTD_CRDR_DTL = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_CRDR_DTL;
                            Old_finReceiptCusSOMpg.NTD_CRDR_MPG = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_CRDR_MPG;
                            Old_finReceiptCusSOMpg.NTD_TAX = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_TAX;
                            retval = FIN_CRDR_NOTE_TAX_DTL_obj.NTD_PK;
                        }

                    }

                }

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
        #endregion
    }
}
