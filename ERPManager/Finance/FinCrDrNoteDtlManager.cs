using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using ERPManager.Finance;

namespace ERPManager
{
    public class FinCrDrNoteDtlManager : IFinCrDrNoteDtlManager
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

        public FinCrDrNoteDtlManager(ERPEntities currentEntity)
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


        public List<FIN_CRDR_NOTE_DTL> GetCrDrSplitList(FIN_CRDR_NOTE_DTL finReceiptCusSoMpgObj)
        {
            List<FIN_CRDR_NOTE_DTL> FinReceiptCusSoMpgListObj = new List<FIN_CRDR_NOTE_DTL>();
            try
            {
                FinReceiptCusSoMpgListObj = (from pvh in this.currentEntity.FIN_CRDR_NOTE_DTL
                                             where pvh.CDS_CRDR_NOTE_MPG == (finReceiptCusSoMpgObj.CDS_CRDR_NOTE_MPG <= 0 ? pvh.CDS_CRDR_NOTE_MPG : finReceiptCusSoMpgObj.CDS_CRDR_NOTE_MPG)
                                             && pvh.CDS_CRDR_NOTE_HDR == (finReceiptCusSoMpgObj.CDS_CRDR_NOTE_HDR <= 0 ? pvh.CDS_CRDR_NOTE_HDR : finReceiptCusSoMpgObj.CDS_CRDR_NOTE_HDR)
                                             select pvh
                              ).ToList();
                return FinReceiptCusSoMpgListObj;
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                // Disposing used objects
                FinReceiptCusSoMpgListObj = null;
            }
        }

        public List<FIN_INVOICE_CUS_DTL> GetInvoiceTrxMpg(FIN_INVOICE_CUS_DTL FinInvoiceCusTrxMpgObj)
        {
            List<FIN_INVOICE_CUS_DTL> InvoiceTrxMpgList = null;
            try
            {

                InvoiceTrxMpgList = (from pvh in this.currentEntity.FIN_INVOICE_CUS_DTL
                                     where pvh.CID_INVOICE_HDR == (FinInvoiceCusTrxMpgObj.CID_INVOICE_HDR == 0 ? pvh.CID_INVOICE_HDR : FinInvoiceCusTrxMpgObj.CID_INVOICE_HDR)
                                     select pvh
                             ).ToList();
                //return Invoice Trx Mpg List
                return InvoiceTrxMpgList;
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

        public List<FIN_INVOICE_CUS_HDR> GetInvoiceCusHdrByPK(FIN_INVOICE_CUS_HDR InvoiceHdrObj)
        {
            List<FIN_INVOICE_CUS_HDR> InvoiceHdrList = null;
            IQueryable<FIN_INVOICE_CUS_HDR> FIN_INVOICE_VND_HDRQuery;
            //int pageSize;
            //int totalCount;
            try
            {

                FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_CUS_HDR
                                            where inv.ICH_ACTIVE == InvoiceHdrObj.ICH_ACTIVE
                                                //&& inv.ICH_DEL_STATUS == InvoiceHdrObj.ICH_DEL_STATUS
                                              && inv.ICH_PK == (InvoiceHdrObj.ICH_PK > 0 ? InvoiceHdrObj.ICH_PK : inv.ICH_PK)
                                            select inv);


                // Apply Paging And Sorting for grid Purpose
                InvoiceHdrList = FIN_INVOICE_VND_HDRQuery.ToList();

                return InvoiceHdrList;
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



        public long? SaveCRDRSplit(List<FIN_CRDR_NOTE_DTL> finReceiptCusSoMpgList, ref long? maxID)
        {
            long? retval;
            retval = 0;

            //long? maxPK;

            List<FIN_CRDR_NOTE_DTL> Old_finReceiptCusSOMpgList;

            FIN_CRDR_NOTE_DTL Old_finReceiptCusSOMpg;
            FIN_INVOICE_CUS_DTL obj_SAL_ORDER_HDR;
            List<FIN_CRDR_NOTE_TAX_DTL> finTaxList;
            FinCrDrNoteTaxManager finCrDrTaxManagerObj;

            try
            {
                if (finReceiptCusSoMpgList.Count > 0)
                {
                    if (!maxID.HasValue || maxID.Value == 0)
                        maxID = currentEntity.FIN_CRDR_NOTE_DTL.Max(v => (long?)v.CDS_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    //maxPK = soPK == 0 ? currentEntity.FIN_RECEIPT_CUS_SO_MPG.Max(v => (long?)v.RSO_PK) : soPK;
                    //maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;

                    long? maxTaxID = currentEntity.FIN_CRDR_NOTE_TAX_DTL.Max(v => (long?)v.NTD_PK);
                    maxTaxID = (maxTaxID.HasValue) ? maxTaxID.Value + 1 : 1;
                    //  Delete Removed Entries

                    List<long> pks = (from old1 in finReceiptCusSoMpgList
                                      select old1.CDS_PK).ToList();
                    /**/
                    long RSO_RECEIPT_TRX_MPG = finReceiptCusSoMpgList[0].CDS_CRDR_NOTE_MPG;

                    Old_finReceiptCusSOMpgList = (from oldp in this.currentEntity.FIN_CRDR_NOTE_DTL
                                                  where oldp.CDS_CRDR_NOTE_MPG == RSO_RECEIPT_TRX_MPG
                                                  && !pks.Contains(oldp.CDS_PK)
                                                  select oldp).ToList();

                    //foreach (FIN_CRDR_NOTE_DTL old_FIN_RECEIPT_CUS_SO_MPG in Old_finReceiptCusSOMpgList)
                    //{
                    //    obj_SAL_ORDER_HDR = currentEntity.FIN_INVOICE_CUS_DTL.SingleOrDefault(a => a.CID_PK == old_FIN_RECEIPT_CUS_SO_MPG.CDS_INVOICE_CUS_DTL);

                    //    if (obj_SAL_ORDER_HDR != null)
                    //    {
                    //        obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED -= old_FIN_RECEIPT_CUS_SO_MPG.RSO_RECEIVED_AMOUNT;
                    //    }

                    //    //this.currentEntity.FIN_RECEIPT_CUS_SO_MPG.DeleteObject(old_FIN_RECEIPT_CUS_SO_MPG);
                    //}


                    foreach (FIN_CRDR_NOTE_DTL FIN_RECEIPT_CUS_SO_MPG_obj in finReceiptCusSoMpgList)
                    {
                        finTaxList = FIN_RECEIPT_CUS_SO_MPG_obj.FIN_CRDR_NOTE_TAX_DTL == null ?
                        new List<FIN_CRDR_NOTE_TAX_DTL>() : FIN_RECEIPT_CUS_SO_MPG_obj.FIN_CRDR_NOTE_TAX_DTL.ToList();
                        FIN_RECEIPT_CUS_SO_MPG_obj.FIN_CRDR_NOTE_TAX_DTL.Clear();

                        if (FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK == 0) //  INSERT NEW RECORD
                        {
                            FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK = maxID.Value;

                            currentEntity.FIN_CRDR_NOTE_DTL.AddObject(FIN_RECEIPT_CUS_SO_MPG_obj);
                            retval = maxID;

                            //obj_SAL_ORDER_HDR = currentEntity.FIN_INVOICE_CUS_DTL.SingleOrDefault(a => a.SOH_PK == FIN_RECEIPT_CUS_SO_MPG_obj.RSO_SO_HDR);
                            //obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED += FIN_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIVED_AMOUNT;

                            maxID++;
                            if (finTaxList.Count > 0)
                            {
                                finTaxList.ForEach(dtl =>
                                {
                                    dtl.NTD_CRDR_DTL = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK;
                                    dtl.NTD_CRDR_MPG = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_CRDR_NOTE_MPG;
                                });
                                finCrDrTaxManagerObj = new FinCrDrNoteTaxManager(this.currentEntity);
                                finCrDrTaxManagerObj.SaveCRDRTaxDetSplit(finTaxList, ref maxID);

                            }
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            //obj_SAL_ORDER_HDR = currentEntity.FIN_INVOICE_CUS_DTL.SingleOrDefault(a => a.SOH_PK == FIN_RECEIPT_CUS_SO_MPG_obj.RSO_SO_HDR);
                            Old_finReceiptCusSOMpg = currentEntity.FIN_CRDR_NOTE_DTL.SingleOrDefault(a => a.CDS_PK == FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK);

                            //if (obj_SAL_ORDER_HDR != null)
                            //{
                            //    obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED += FIN_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIVED_AMOUNT - Old_finReceiptCusSOMpg.RSO_RECEIVED_AMOUNT;
                            //}

                            Old_finReceiptCusSOMpg.CDS_QTY = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_QTY;
                            Old_finReceiptCusSOMpg.CDS_RATE = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_RATE;
                            Old_finReceiptCusSOMpg.CDS_AMOUNT = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_AMOUNT;
                            retval = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK;
                            if (finTaxList.Count > 0)
                            {
                                finTaxList.ForEach(dtl =>
                                {
                                    dtl.NTD_CRDR_DTL = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK;
                                    dtl.NTD_CRDR_MPG = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_CRDR_NOTE_MPG;
                                });
                                finCrDrTaxManagerObj = new FinCrDrNoteTaxManager(this.currentEntity);
                                finCrDrTaxManagerObj.SaveCRDRTaxDetSplit(finTaxList, ref maxTaxID);


                            }
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
        public long? SaveCRDRSplit(List<FIN_CRDR_NOTE_DTL> finReceiptCusSoMpgList, ref long? maxTaxHdrId, ref long? maxID)
        {
            long? retval;
            retval = 0;

            //long? maxPK;

            List<FIN_CRDR_NOTE_DTL> Old_finReceiptCusSOMpgList;

            FIN_CRDR_NOTE_DTL Old_finReceiptCusSOMpg;
            FIN_INVOICE_CUS_DTL obj_SAL_ORDER_HDR;
            List<FIN_CRDR_NOTE_TAX_DTL> finTaxList;
            FinCrDrNoteTaxManager finCrDrTaxManagerObj;

            try
            {
                if (finReceiptCusSoMpgList.Count > 0)
                {
                    if (!maxID.HasValue || maxID.Value == 0)
                        maxID = currentEntity.FIN_CRDR_NOTE_DTL.Max(v => (long?)v.CDS_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    //maxPK = soPK == 0 ? currentEntity.FIN_RECEIPT_CUS_SO_MPG.Max(v => (long?)v.RSO_PK) : soPK;
                    //maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;


                    //  Delete Removed Entries

                    List<long> pks = (from old1 in finReceiptCusSoMpgList
                                      select old1.CDS_PK).ToList();
                    /**/
                    long RSO_RECEIPT_TRX_MPG = finReceiptCusSoMpgList[0].CDS_CRDR_NOTE_MPG;

                    Old_finReceiptCusSOMpgList = (from oldp in this.currentEntity.FIN_CRDR_NOTE_DTL
                                                  where oldp.CDS_CRDR_NOTE_MPG == RSO_RECEIPT_TRX_MPG
                                                  && !pks.Contains(oldp.CDS_PK)
                                                  select oldp).ToList();

                    //foreach (FIN_CRDR_NOTE_DTL old_FIN_RECEIPT_CUS_SO_MPG in Old_finReceiptCusSOMpgList)
                    //{
                    //    obj_SAL_ORDER_HDR = currentEntity.FIN_INVOICE_CUS_DTL.SingleOrDefault(a => a.CID_PK == old_FIN_RECEIPT_CUS_SO_MPG.CDS_INVOICE_CUS_DTL);

                    //    if (obj_SAL_ORDER_HDR != null)
                    //    {
                    //        obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED -= old_FIN_RECEIPT_CUS_SO_MPG.RSO_RECEIVED_AMOUNT;
                    //    }

                    //    //this.currentEntity.FIN_RECEIPT_CUS_SO_MPG.DeleteObject(old_FIN_RECEIPT_CUS_SO_MPG);
                    //}


                    foreach (FIN_CRDR_NOTE_DTL FIN_RECEIPT_CUS_SO_MPG_obj in finReceiptCusSoMpgList)
                    {
                        finTaxList = FIN_RECEIPT_CUS_SO_MPG_obj.FIN_CRDR_NOTE_TAX_DTL == null ?
                        new List<FIN_CRDR_NOTE_TAX_DTL>() : FIN_RECEIPT_CUS_SO_MPG_obj.FIN_CRDR_NOTE_TAX_DTL.ToList();
                        FIN_RECEIPT_CUS_SO_MPG_obj.FIN_CRDR_NOTE_TAX_DTL.Clear();

                        if (FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK == 0) //  INSERT NEW RECORD
                        {
                            FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK = maxID.Value;

                            currentEntity.FIN_CRDR_NOTE_DTL.AddObject(FIN_RECEIPT_CUS_SO_MPG_obj);
                            retval = maxID;

                            //obj_SAL_ORDER_HDR = currentEntity.FIN_INVOICE_CUS_DTL.SingleOrDefault(a => a.SOH_PK == FIN_RECEIPT_CUS_SO_MPG_obj.RSO_SO_HDR);
                            //obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED += FIN_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIVED_AMOUNT;

                            maxID++;
                            if (finTaxList.Count > 0)
                            {
                                finTaxList.ForEach(dtl =>
                                {
                                    dtl.NTD_CRDR_DTL = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK;
                                    dtl.NTD_CRDR_MPG = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_CRDR_NOTE_MPG;
                                });
                                finCrDrTaxManagerObj = new FinCrDrNoteTaxManager(this.currentEntity);
                                finCrDrTaxManagerObj.SaveCRDRTaxDetSplit(finTaxList, ref maxTaxHdrId);
                            }
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            //obj_SAL_ORDER_HDR = currentEntity.FIN_INVOICE_CUS_DTL.SingleOrDefault(a => a.SOH_PK == FIN_RECEIPT_CUS_SO_MPG_obj.RSO_SO_HDR);
                            Old_finReceiptCusSOMpg = currentEntity.FIN_CRDR_NOTE_DTL.SingleOrDefault(a => a.CDS_PK == FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK);

                            //if (obj_SAL_ORDER_HDR != null)
                            //{
                            //    obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED += FIN_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIVED_AMOUNT - Old_finReceiptCusSOMpg.RSO_RECEIVED_AMOUNT;
                            //}

                            Old_finReceiptCusSOMpg.CDS_QTY = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_QTY;
                            Old_finReceiptCusSOMpg.CDS_RATE = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_RATE;
                            Old_finReceiptCusSOMpg.CDS_AMOUNT = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_AMOUNT;
                            Old_finReceiptCusSOMpg.CDS_TAX = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_TAX;
                            Old_finReceiptCusSOMpg.CDS_NET_AMOUNT = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_NET_AMOUNT;
                            Old_finReceiptCusSOMpg.CDS_IS_AFFECT_STK = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_IS_AFFECT_STK;
                            Old_finReceiptCusSOMpg.CDS_DISCOUNT = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_DISCOUNT;
                            retval = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK;
                            if (finTaxList.Count > 0 && FIN_RECEIPT_CUS_SO_MPG_obj.CDS_TAX > 0)
                            {
                                finTaxList.ForEach(dtl =>
                                {
                                    dtl.NTD_CRDR_DTL = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_PK;
                                    dtl.NTD_CRDR_MPG = FIN_RECEIPT_CUS_SO_MPG_obj.CDS_CRDR_NOTE_MPG;
                                });
                                finCrDrTaxManagerObj = new FinCrDrNoteTaxManager(this.currentEntity);
                                finCrDrTaxManagerObj.SaveCRDRTaxDetSplit(finTaxList, ref maxTaxHdrId);
                            }
                            else
                            {
                                //delete item tax split up
                                List<FIN_CRDR_NOTE_TAX_DTL> fin_TaxHdr_List_Obj = Old_finReceiptCusSOMpg.FIN_CRDR_NOTE_TAX_DTL.ToList();
                                if (fin_TaxHdr_List_Obj != null && fin_TaxHdr_List_Obj.Count > 0)
                                {
                                    foreach (FIN_CRDR_NOTE_TAX_DTL finTaxObj in fin_TaxHdr_List_Obj)
                                    {
                                        FIN_CRDR_NOTE_TAX_DTL obj_TAX_DTL = currentEntity.FIN_CRDR_NOTE_TAX_DTL.SingleOrDefault(tax => tax.NTD_PK == finTaxObj.NTD_PK);
                                        this.currentEntity.FIN_CRDR_NOTE_TAX_DTL.DeleteObject(obj_TAX_DTL);
                                    }
                                }

                            }
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





        public List<FIN_INVOICE_VND_DTL> GetInvoiceVndTrxMpg(FIN_INVOICE_VND_DTL FinCrDrCusTrxMpgObj)
        {
            List<FIN_INVOICE_VND_DTL> InvoiceTrxMpgList = null;
            try
            {

                InvoiceTrxMpgList = (from pvh in this.currentEntity.FIN_INVOICE_VND_DTL
                                     where pvh.VID_INVOICE_HDR == (FinCrDrCusTrxMpgObj.VID_INVOICE_HDR == 0 ? pvh.VID_INVOICE_HDR : FinCrDrCusTrxMpgObj.VID_INVOICE_HDR)
                                     select pvh
                             ).ToList();
                //return Invoice Trx Mpg List
                return InvoiceTrxMpgList;
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

        public List<FIN_INVOICE_VND_HDR> GetInvoiceVndHdrByPK(FIN_INVOICE_VND_HDR InvoiceHdrObj)
        {
            List<FIN_INVOICE_VND_HDR> InvoiceHdrList = null;
            IQueryable<FIN_INVOICE_VND_HDR> FIN_INVOICE_VND_HDRQuery;

            try
            {

                FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                            where inv.IVH_ACTIVE == InvoiceHdrObj.IVH_ACTIVE
                                                //&& inv.ICH_DEL_STATUS == InvoiceHdrObj.ICH_DEL_STATUS
                                              && inv.IVH_PK == (InvoiceHdrObj.IVH_PK > 0 ? InvoiceHdrObj.IVH_PK : inv.IVH_PK)
                                            select inv);


                // Apply Paging And Sorting for grid Purpose
                InvoiceHdrList = FIN_INVOICE_VND_HDRQuery.ToList();

                return InvoiceHdrList;
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

        public bool IsDrCrUsedInOtherTrnsactions(long CrdrPk)
        {

            try
            {
                bool IsExist = false;
                FIN_CRDR_NOTE_HDR objFinCrDr = this.currentEntity.FIN_CRDR_NOTE_HDR.SingleOrDefault(crdr => crdr.CDH_PK == CrdrPk);
                //var crdrQuery = from crdr in this.currentEntity.FIN_CRDR_NOTE_HDR where crdr.CDH_PK == CrdrPk && crdr.CDH_HAS_JRNL_ENTRY == true select crdr;
                if (objFinCrDr != null && objFinCrDr.CDH_HAS_JRNL_ENTRY && !objFinCrDr.CDH_IS_DELETED)
                {
                    IsExist = true;
                }
                else
                {
                    if (objFinCrDr.CDH_VENDOR.HasValue && objFinCrDr.CDH_TYPE == (byte)DebitCreditModeEnum.DEBIT)
                    {
                        var pymntQuery = from pmt in this.currentEntity.FIN_PAYMENT_VND_ALCN_DTL where pmt.PAD_ALCN_CDH == CrdrPk && pmt.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 select pmt;
                        if (pymntQuery != null && pymntQuery.Count() > 0)
                        {
                            IsExist = true;
                        }
                    }
                    else if (objFinCrDr.CDH_CUSTOMER.HasValue && objFinCrDr.CDH_TYPE == (byte)DebitCreditModeEnum.CREDIT)
                    {
                        var recptQuery = from pmt in this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL where pmt.RAD_ALCN_CDH == CrdrPk && pmt.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 select pmt;
                        if (recptQuery != null && recptQuery.Count() > 0)
                        {
                            IsExist = true;
                        }
                    }
                }
                return IsExist;

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

        public bool IsRefnoExist(FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj)
        {
            bool IsRefExist = false;
            List<FIN_CRDR_NOTE_HDR> finTrxHdrList = null;
            try
            {
                if (finCrDrNoteHdrObj.CDH_VENDOR.HasValue)
                {
                    finTrxHdrList = (from fth in this.currentEntity.FIN_CRDR_NOTE_HDR where !string.IsNullOrEmpty(fth.CDH_REF_NO) && fth.CDH_REF_NO == finCrDrNoteHdrObj.CDH_REF_NO && fth.CDH_VENDOR == finCrDrNoteHdrObj.CDH_VENDOR && fth.CDH_PK != finCrDrNoteHdrObj.CDH_PK && fth.CDH_IS_DELETED == false select fth).ToList();
                }
                else if (finCrDrNoteHdrObj.CDH_CUSTOMER.HasValue)
                {
                    finTrxHdrList = (from fth in this.currentEntity.FIN_CRDR_NOTE_HDR where !string.IsNullOrEmpty(fth.CDH_REF_NO) && fth.CDH_REF_NO == finCrDrNoteHdrObj.CDH_REF_NO && fth.CDH_CUSTOMER == finCrDrNoteHdrObj.CDH_CUSTOMER && fth.CDH_PK != finCrDrNoteHdrObj.CDH_PK && fth.CDH_IS_DELETED == false select fth).ToList();

                }
                if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                {
                    IsRefExist = true;
                }
                else
                {
                    IsRefExist = false;
                }
                return IsRefExist;
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

        public List<FIN_PAYMENT_VND_ALCN_DTL> GetDnsAllocationInPayment(long CrDrPk)
        {
            List<FIN_PAYMENT_VND_ALCN_DTL> AllocationList = null;
            IQueryable<FIN_PAYMENT_VND_ALCN_DTL> FIN_PAYMENT_VND_ALCN_DTLQuery;

            try
            {

                FIN_PAYMENT_VND_ALCN_DTLQuery = (from inv in this.currentEntity.FIN_PAYMENT_VND_ALCN_DTL
                                                 where inv.PAD_ALCN_CDH == CrDrPk
                                                 && inv.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                                 && inv.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0
                                                 select inv);


                // Apply Paging And Sorting for grid Purpose
                AllocationList = FIN_PAYMENT_VND_ALCN_DTLQuery.ToList();

                return AllocationList;
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

        public List<FIN_RECEIPT_CUS_ALCN_DTL> GetCnsAllocationInReceipt(long CrDrPk)
        {
            List<FIN_RECEIPT_CUS_ALCN_DTL> AllocationList = null;
            IQueryable<FIN_RECEIPT_CUS_ALCN_DTL> FIN_RECEIPT_CUS_ALCN_DTLQuery;

            try
            {

                FIN_RECEIPT_CUS_ALCN_DTLQuery = (from inv in this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL
                                                 where inv.RAD_ALCN_CDH == CrDrPk
                                                 && inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                 && inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                                 select inv);


                // Apply Paging And Sorting for grid Purpose
                AllocationList = FIN_RECEIPT_CUS_ALCN_DTLQuery.ToList();

                return AllocationList;
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

        public decimal GetCrDrAllocatedAmount(long CrDrPk, int Mode)
        {
            decimal AllocatedAmount = 0;
            List<FIN_RECEIPT_CUS_ALCN_DTL> AllocationList = null;
            IQueryable<FIN_RECEIPT_CUS_ALCN_DTL> FIN_RECEIPT_CUS_ALCN_DTLQuery;

            try
            {
                if (Mode == (int)DebitCreditModeEnum.CREDIT)
                {
                    FIN_RECEIPT_CUS_ALCN_DTLQuery = (from inv in this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL
                                                     where inv.RAD_ALCN_CDH == CrDrPk
                                                     && inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                     && inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                                     select inv);
                    AllocationList = FIN_RECEIPT_CUS_ALCN_DTLQuery.ToList();
                    if (AllocationList != null && AllocationList.Count > 0)
                    {
                        AllocatedAmount = AllocationList.Sum(r => r.RAD_AMOUNT);
                    }
                }
                else
                {
                    List<FIN_CRDR_NOTE_MPG> objCrDrMpgList = (from crdr in this.currentEntity.FIN_CRDR_NOTE_MPG where crdr.CDM_CRDR_NOTE_HDR == CrDrPk select crdr).ToList();
                    foreach (FIN_CRDR_NOTE_MPG objCrDrMpg in objCrDrMpgList)
                    {
                        List<FIN_RECEIPT_CUS_TRX_MPG> objRcptTrxList = (from rcpt in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG where rcpt.RCM_INVOICE_HDR == objCrDrMpg.CDM_INVOICE_CUS_HDR & rcpt.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 & rcpt.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0 select rcpt).ToList();
                        if (objRcptTrxList != null)
                        {
                            decimal RcvdAmount = 0;
                            RcvdAmount = objRcptTrxList.Sum(r => r.RCM_RCVD_AMOUNT);
                            if (objCrDrMpg.FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC <= RcvdAmount)
                            {
                                AllocatedAmount += ((RcvdAmount - objCrDrMpg.FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC) > objCrDrMpg.FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC) ? objCrDrMpg.FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC : (RcvdAmount - objCrDrMpg.FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC);
                            }
                        }
                    }
                }
                return AllocatedAmount;
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

        public List<CrdrAllocations> GetBalanceAmountSplit(long CrDrPk)
        {

            try
            {
                List<CrdrAllocations> objCrdrAlcnList = new List<CrdrAllocations>();
                FIN_CRDR_NOTE_HDR CrdrHdr = currentEntity.FIN_CRDR_NOTE_HDR.SingleOrDefault(r => r.CDH_PK == CrDrPk);
                if (CrdrHdr.CDH_TYPE == (byte)DebitCreditModeEnum.CREDIT)
                {
                    objCrdrAlcnList = (from inv in this.currentEntity.FIN_PAYMENT_VND_CRDR_MPG
                                       where inv.PNM_CRDR_HDR == CrDrPk
                                       && inv.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                       && inv.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0
                                       select new CrdrAllocations
                                       {
                                           TRX_NO = inv.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_NO + " (" + inv.FIN_PAYMENT_VND_TRX_MPG.FIN_INVOICE_VND_HDR.IVH_NO + ")",
                                           TRX_DATE = inv.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DATE,
                                           TRX_AMOUNT = inv.PNM_PAID_AMOUNT + inv.PNM_ADJ_AMOUNT
                                       }).ToList();
                }
                else
                {
                    objCrdrAlcnList = (from inv in this.currentEntity.FIN_PAYMENT_VND_ALCN_DTL
                                       where inv.PAD_ALCN_CDH == CrDrPk
                                       && inv.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                       && inv.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0
                                       select new CrdrAllocations
                                      {
                                          TRX_NO = inv.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_NO + " (" + inv.FIN_PAYMENT_VND_TRX_MPG.FIN_INVOICE_VND_HDR.IVH_NO + ")",
                                          TRX_DATE = inv.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DATE,
                                          TRX_AMOUNT = inv.PAD_AMOUNT
                                      }).ToList();
                }

                //return queryDebitAlcn.Union(queryCreditAlcn).ToList().ToDataTable();
                return objCrdrAlcnList;
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

        public List<CrdrAllocations> GetBalanceAmountSplitSales(long CrDrPk)
        {
            try
            {
                List<CrdrAllocations> objCrdrAlcnList = new List<CrdrAllocations>();
                FIN_CRDR_NOTE_HDR CrdrHdr = currentEntity.FIN_CRDR_NOTE_HDR.SingleOrDefault(r => r.CDH_PK == CrDrPk);
                if (CrdrHdr.CDH_TYPE == (byte)DebitCreditModeEnum.DEBIT)
                {
                    objCrdrAlcnList = (from inv in this.currentEntity.FIN_RECEIPT_CUS_CRDR_MPG
                                       where inv.RNM_CRDR_HDR == CrDrPk
                                       && inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                       && inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                       select new CrdrAllocations
                                       {
                                           TRX_NO = inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_NO + " (" + inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_INVOICE_CUS_HDR.ICH_NO + ")",
                                           TRX_DATE = inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DATE,
                                           TRX_AMOUNT = inv.RNM_PAID_AMOUNT + inv.RNM_ADJ_AMOUNT
                                       }).ToList();
                }
                else
                {
                    objCrdrAlcnList = (from inv in this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL
                                       where inv.RAD_ALCN_CDH == CrDrPk
                                       && inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                       && inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                       select new CrdrAllocations
                                       {
                                           TRX_NO = inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_NO + " (" + inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_INVOICE_CUS_HDR.ICH_NO + ")",
                                           TRX_DATE = inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DATE,
                                           TRX_AMOUNT = inv.RAD_AMOUNT
                                       }).ToList();
                }

                return objCrdrAlcnList;

                //var query = (from inv in this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL
                //             where inv.RAD_ALCN_CDH == CrDrPk
                //             && inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                //             && inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                //             select new
                //             {
                //                 TRX_NO = inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_NO + " (" + inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_INVOICE_CUS_HDR.ICH_NO + ")",
                //                 TRX_DATE = inv.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DATE,
                //                 TRX_AMOUNT = inv.RAD_AMOUNT
                //             });

                //return query.ToList().ToDataTable();
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

        public List<FIN_RECEIPT_CUS_CRDR_MPG> CheckDRCRforCancel(FIN_RECEIPT_CUS_CRDR_MPG finReceiptCusCRDRMpgObj)
        {
            List<FIN_RECEIPT_CUS_CRDR_MPG> FinReceiptCusCRDRMpgListObj = new List<FIN_RECEIPT_CUS_CRDR_MPG>();
            try
            {
                FinReceiptCusCRDRMpgListObj = (from frccm in this.currentEntity.FIN_RECEIPT_CUS_CRDR_MPG
                                               where frccm.RNM_CRDR_HDR == finReceiptCusCRDRMpgObj.RNM_CRDR_HDR
                                               && frccm.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                               && frccm.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                               select frccm
                              ).ToList();
                return FinReceiptCusCRDRMpgListObj;
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                // Disposing used objects
                FinReceiptCusCRDRMpgListObj = null;
            }
        }

        public List<FIN_PAYMENT_VND_CRDR_MPG> CheckPaymentDRCRforCancel(FIN_PAYMENT_VND_CRDR_MPG finPaymentCusCRDRMpgObj)
        {
            List<FIN_PAYMENT_VND_CRDR_MPG> FinPaymentCusCRDRMpgListObj = new List<FIN_PAYMENT_VND_CRDR_MPG>();
            try
            {
                FinPaymentCusCRDRMpgListObj = (from frccm in this.currentEntity.FIN_PAYMENT_VND_CRDR_MPG
                                               where frccm.PNM_CRDR_HDR == finPaymentCusCRDRMpgObj.PNM_CRDR_HDR && frccm.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS==0
                                               select frccm
                              ).ToList();
                return FinPaymentCusCRDRMpgListObj;
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                // Disposing used objects
                FinPaymentCusCRDRMpgListObj = null;
            }
        }

        public decimal GetTotalCNAmount(long InvoicePk, long CrDrPk)
        {
            List<FIN_CRDR_NOTE_MPG> objCrList = new List<FIN_CRDR_NOTE_MPG>();     
            try
            {
                decimal amountCN = 0;                        
                objCrList = (from c in currentEntity.FIN_CRDR_NOTE_MPG
                                               where c.CDM_INVOICE_CUS_HDR == InvoicePk
                                                && c.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED == false
                                                && c.FIN_CRDR_NOTE_HDR.CDH_TYPE == 2 //Credit
                                                && c.FIN_CRDR_NOTE_HDR.CDH_PK != CrDrPk
                                               select c).ToList();
                if (objCrList != null && objCrList.Count > 0)
                    amountCN = objCrList.Sum(r => r.CDM_AMOUNT);
                return amountCN;
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                // Disposing used objects      
                objCrList = null;
            }
        }
    }
}
