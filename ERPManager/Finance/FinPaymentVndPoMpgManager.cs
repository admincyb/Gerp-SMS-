using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;
using ERPManager.Finance;
using BusinessObject.POInvoicing;
using BusinessObject.CommonManagement;

namespace ERPManager
{
    public class FinPaymentVndPoMpgManager : IFinPaymentVndPoMpgManager
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

        public FinPaymentVndPoMpgManager(ERPEntities currentEntity)
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


        public List<FIN_PAYMENT_VND_PO_MPG> GetFinPatmentVndPoMpgList(FIN_PAYMENT_VND_PO_MPG finPaymentVndPoMpgObj)
        {
            List<FIN_PAYMENT_VND_PO_MPG> FinPaymentVndPoMpgListObj = new List<FIN_PAYMENT_VND_PO_MPG>();
            try
            {
                FinPaymentVndPoMpgListObj = (from pvh in this.currentEntity.FIN_PAYMENT_VND_PO_MPG
                                             where pvh.PPO_PAYMENT_TRX_MPG == (finPaymentVndPoMpgObj.PPO_PAYMENT_TRX_MPG <= 0 ? pvh.PPO_PAYMENT_TRX_MPG : finPaymentVndPoMpgObj.PPO_PAYMENT_TRX_MPG)
                                             && pvh.PPO_PAYMENT_HDR == (finPaymentVndPoMpgObj.PPO_PAYMENT_HDR <= 0 ? pvh.PPO_PAYMENT_HDR : finPaymentVndPoMpgObj.PPO_PAYMENT_HDR)

                                             select pvh
                              ).ToList();
                return FinPaymentVndPoMpgListObj;
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
                FinPaymentVndPoMpgListObj = null;
            }
        }

        public long? SavePaymentSplit(List<FIN_PAYMENT_VND_PO_MPG> finPaymentVndPoMpgList, ref long? maxID, ref long? MaxTaxId, Byte Category, long? invoiceHdr, decimal? discTotAmount)
        {
            long? retval;
            retval = 0;
            long? taxMaxID;
            int? soHdr;
            int? woHdr;
            soHdr = 0;
            woHdr = 0;

            //long? maxPK;

            List<FIN_PAYMENT_VND_PO_MPG> Old_finPaymentVndPoMpgList;

            FIN_PAYMENT_VND_PO_MPG Old_finPaymentVndPoMpg;
            PUR_ORDER_HDR obj_PUR_ORDER_HDR;
            INV_WORK_ORDER_ITEM_HDR obj_INV_WORK_ORDER_ITEM_HDR;

            List<FIN_PAYMENT_VND_TAX_DTL> finPaymentVndTaxDtlList;
            FinPaymentVndTaxDtlManager finPaymentVndTaxDtlManagerObj;

            try
            {
                if (finPaymentVndPoMpgList.Count > 0)
                {
                    if (!maxID.HasValue || maxID.Value == 0)
                        maxID = currentEntity.FIN_PAYMENT_VND_PO_MPG.Max(v => (long?)v.PPO_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;





                    //  Delete Removed Entries

                    List<long> pks = (from old1 in finPaymentVndPoMpgList
                                      select old1.PPO_PK).ToList();


                    long PPO_PAYMENT_TRX_MPG = finPaymentVndPoMpgList[0].PPO_PAYMENT_TRX_MPG;

                    /**/
                    Old_finPaymentVndPoMpgList = (from oldp in this.currentEntity.FIN_PAYMENT_VND_PO_MPG
                                                  where oldp.PPO_PAYMENT_TRX_MPG == PPO_PAYMENT_TRX_MPG
                                                  && !pks.Contains(oldp.PPO_PK)
                                                  select oldp).ToList();

                    foreach (FIN_PAYMENT_VND_PO_MPG old_FIN_PAYMENT_VND_PO_MPG in Old_finPaymentVndPoMpgList)
                    {
                        obj_PUR_ORDER_HDR = currentEntity.PUR_ORDER_HDR.SingleOrDefault(a => a.POH_PK == old_FIN_PAYMENT_VND_PO_MPG.PPO_PO_HDR);

                        if (obj_PUR_ORDER_HDR != null)
                        {
                            obj_PUR_ORDER_HDR.POH_AMT_PAID -= old_FIN_PAYMENT_VND_PO_MPG.PPO_PAID_AMOUNT;
                        }

                        obj_INV_WORK_ORDER_ITEM_HDR = currentEntity.INV_WORK_ORDER_ITEM_HDR.SingleOrDefault(a => a.WIH_PK == old_FIN_PAYMENT_VND_PO_MPG.PPO_WO_HDR);

                        if (obj_INV_WORK_ORDER_ITEM_HDR != null)
                        {
                            obj_INV_WORK_ORDER_ITEM_HDR.WIH_AMT_PAID -= old_FIN_PAYMENT_VND_PO_MPG.PPO_PAID_AMOUNT;
                        }

                        //this.currentEntity.FIN_PAYMENT_VND_PO_MPG.DeleteObject(old_FIN_PAYMENT_VND_PO_MPG);
                    }


                    foreach (FIN_PAYMENT_VND_PO_MPG FIN_PAYMENT_VND_PO_MPG_obj in finPaymentVndPoMpgList)
                    {
                        finPaymentVndTaxDtlList = FIN_PAYMENT_VND_PO_MPG_obj.FIN_PAYMENT_VND_TAX_DTL == null ?
                            new List<FIN_PAYMENT_VND_TAX_DTL>() : FIN_PAYMENT_VND_PO_MPG_obj.FIN_PAYMENT_VND_TAX_DTL.ToList();
                        FIN_PAYMENT_VND_PO_MPG_obj.FIN_PAYMENT_VND_TAX_DTL.Clear();

                        FIN_PAYMENT_VND_PO_MPG PymntPoMpgObj = new FIN_PAYMENT_VND_PO_MPG();
                        if (FIN_PAYMENT_VND_PO_MPG_obj.PPO_PK == 0) //  INSERT NEW RECORD
                        {
                            PymntPoMpgObj.PPO_PK = maxID.Value;
                            PymntPoMpgObj.PPO_ACTIVE = FIN_PAYMENT_VND_PO_MPG_obj.PPO_ACTIVE;
                            PymntPoMpgObj.PPO_BOUNCED = FIN_PAYMENT_VND_PO_MPG_obj.PPO_BOUNCED;
                            PymntPoMpgObj.PPO_OTHER_AMOUNT = FIN_PAYMENT_VND_PO_MPG_obj.PPO_OTHER_AMOUNT;
                            PymntPoMpgObj.PPO_PAID_AMOUNT = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PAID_AMOUNT;
                            PymntPoMpgObj.PPO_PAYMENT_HDR = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PAYMENT_HDR;
                            PymntPoMpgObj.PPO_PAYMENT_TRX_MPG = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PAYMENT_TRX_MPG;
                            PymntPoMpgObj.PPO_PO_HDR = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PO_HDR;
                            PymntPoMpgObj.PPO_WO_HDR = FIN_PAYMENT_VND_PO_MPG_obj.PPO_WO_HDR;
                            PymntPoMpgObj.PPO_TAX_AMOUNT = FIN_PAYMENT_VND_PO_MPG_obj.PPO_TAX_AMOUNT;


                            currentEntity.FIN_PAYMENT_VND_PO_MPG.AddObject(PymntPoMpgObj);

                            //FIN_PAYMENT_VND_PO_MPG_obj.PPO_PK = maxID.Value;
                            //currentEntity.FIN_PAYMENT_VND_PO_MPG.AddObject(FIN_PAYMENT_VND_PO_MPG_obj);
                            retval = maxID;
                            if (FIN_PAYMENT_VND_PO_MPG_obj.PPO_PO_HDR != null)
                            {
                                obj_PUR_ORDER_HDR = currentEntity.PUR_ORDER_HDR.SingleOrDefault(a => a.POH_PK == FIN_PAYMENT_VND_PO_MPG_obj.PPO_PO_HDR);
                                obj_PUR_ORDER_HDR.POH_AMT_PAID += FIN_PAYMENT_VND_PO_MPG_obj.PPO_PAID_AMOUNT;
                            }
                            if (FIN_PAYMENT_VND_PO_MPG_obj.PPO_WO_HDR != null)
                            {
                                obj_INV_WORK_ORDER_ITEM_HDR = currentEntity.INV_WORK_ORDER_ITEM_HDR.SingleOrDefault(a => a.WIH_PK == FIN_PAYMENT_VND_PO_MPG_obj.PPO_WO_HDR);
                                obj_INV_WORK_ORDER_ITEM_HDR.WIH_AMT_PAID += FIN_PAYMENT_VND_PO_MPG_obj.PPO_PAID_AMOUNT;
                            }

                            maxID++;
                            decimal splitTaxAMOUNT = FIN_PAYMENT_VND_PO_MPG_obj.PPO_TAX_AMOUNT;
                            if (finPaymentVndTaxDtlList.Count > 0)
                            {
                                finPaymentVndTaxDtlList.ForEach(dtl =>
                                {
                                    dtl.PDT_PAYMENT_TRX = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PAYMENT_TRX_MPG;
                                    dtl.PDT_VND_PO_MPG = PymntPoMpgObj.PPO_PK;
                                    soHdr = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PO_HDR;
                                    woHdr = FIN_PAYMENT_VND_PO_MPG_obj.PPO_WO_HDR;
                                });
                                finPaymentVndTaxDtlManagerObj = new FinPaymentVndTaxDtlManager(this.currentEntity);

                                finPaymentVndTaxDtlManagerObj.SavePaymentTax(finPaymentVndTaxDtlList, ref MaxTaxId, soHdr, splitTaxAMOUNT, Category, invoiceHdr, discTotAmount, woHdr);
                                //soMpgPK = finReceiptCusSOMpgManagerObj.SaveReceiptSplit(finReceiptCusSoMpgList, (long)soMpgPK);200 

                                //soMpgPK = soMpgPK == null ? 0 : soMpgPK;
                            }
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            Old_finPaymentVndPoMpg = currentEntity.FIN_PAYMENT_VND_PO_MPG.SingleOrDefault(a => a.PPO_PK == FIN_PAYMENT_VND_PO_MPG_obj.PPO_PK);
                            if (FIN_PAYMENT_VND_PO_MPG_obj.PPO_PO_HDR != null)
                            {
                                obj_PUR_ORDER_HDR = currentEntity.PUR_ORDER_HDR.SingleOrDefault(a => a.POH_PK == FIN_PAYMENT_VND_PO_MPG_obj.PPO_PO_HDR);
                                if (obj_PUR_ORDER_HDR != null)
                                {
                                    obj_PUR_ORDER_HDR.POH_AMT_PAID += FIN_PAYMENT_VND_PO_MPG_obj.PPO_PAID_AMOUNT - Old_finPaymentVndPoMpg.PPO_PAID_AMOUNT;
                                }
                            }
                            if (FIN_PAYMENT_VND_PO_MPG_obj.PPO_WO_HDR != null)
                            {
                                obj_INV_WORK_ORDER_ITEM_HDR = currentEntity.INV_WORK_ORDER_ITEM_HDR.SingleOrDefault(a => a.WIH_PK == FIN_PAYMENT_VND_PO_MPG_obj.PPO_WO_HDR);
                                if (obj_INV_WORK_ORDER_ITEM_HDR != null)
                                {
                                    obj_INV_WORK_ORDER_ITEM_HDR.WIH_AMT_PAID += FIN_PAYMENT_VND_PO_MPG_obj.PPO_PAID_AMOUNT - Old_finPaymentVndPoMpg.PPO_PAID_AMOUNT;
                                }
                            }

                            Old_finPaymentVndPoMpg.PPO_PAID_AMOUNT = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PAID_AMOUNT;
                            Old_finPaymentVndPoMpg.PPO_TAX_AMOUNT = FIN_PAYMENT_VND_PO_MPG_obj.PPO_TAX_AMOUNT;

                            retval = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PK;

                            decimal splitTaxAMOUNT = FIN_PAYMENT_VND_PO_MPG_obj.PPO_TAX_AMOUNT;
                            if (finPaymentVndTaxDtlList.Count > 0)
                            {
                                finPaymentVndTaxDtlList.ForEach(dtl =>
                                {
                                    dtl.PDT_PAYMENT_TRX = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PAYMENT_TRX_MPG;
                                    dtl.PDT_VND_PO_MPG = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PK;
                                    soHdr = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PO_HDR;
                                    woHdr = FIN_PAYMENT_VND_PO_MPG_obj.PPO_PO_HDR;
                                });
                                finPaymentVndTaxDtlManagerObj = new FinPaymentVndTaxDtlManager(this.currentEntity);
                                finPaymentVndTaxDtlManagerObj.SavePaymentTax(finPaymentVndTaxDtlList, ref MaxTaxId, soHdr, splitTaxAMOUNT, Category, invoiceHdr, discTotAmount, woHdr);
                                //soMpgPK = finReceiptCusSOMpgManagerObj.SaveReceiptSplit(finReceiptCusSoMpgList, (long)soMpgPK);200 

                                //soMpgPK = soMpgPK == null ? 0 : soMpgPK;
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

        public List<PaymentAdjnAllocation> GetCrDrAdjn(int VendorId)
        {
            List<PaymentAdjnAllocation> CrDrAdjnList = null;
            List<PaymentAdjnAllocation> PaymentVndTrxAdjnList = null;
            try
            {
                byte isActive = Convert.ToByte(DbActiveStatus.ACTIVE);
                byte mode = Convert.ToByte(DebitCreditModeEnum.DEBIT);

                CrDrAdjnList = (from pvh in this.currentEntity.FIN_CRDR_NOTE_HDR
                                where pvh.CDH_IS_DELETED == false
                                          && pvh.CDH_VENDOR == VendorId
                                          && pvh.CDH_STATUS > 0
                                          && pvh.CDH_ACTIVE == isActive
                                          && pvh.CDH_TYPE == mode
                                select pvh).ToList().
                                Select(s => new PaymentAdjnAllocation
                                {
                                    PAA_AMOUNT = s.CDH_AMOUNT_TC,
                                    PAA_CRDRPK = s.CDH_PK,
                                    PAA_NO = s.CDH_NO,
                                    PAA_TRXPK = 0,
                                    PAA_DATE = s.CDH_DATE,
                                    PAA_TYPE = "DN"

                                }).ToList();

                PaymentVndTrxAdjnList = (from rch in this.currentEntity.FIN_PAYMENT_VND_TRX_MPG
                                         where rch.FIN_PAYMENT_VND_HDR.PVH_VENDOR == VendorId
                                         && rch.PVM_ACTIVE == isActive
                                         && rch.FIN_PAYMENT_VND_HDR.PVH_STATUS == 2
                                         && rch.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                         && rch.PVM_EXCESS_AMOUNT > 0
                                         && rch.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0
                                         select rch).ToList().
                                         Select(s => new PaymentAdjnAllocation
                                         {
                                             PAA_AMOUNT = s.PVM_EXCESS_AMOUNT,
                                             PAA_CRDRPK = 0,
                                             PAA_NO = s.FIN_PAYMENT_VND_HDR.PVH_NO,
                                             PAA_TRXPK = s.PVM_PK,
                                             PAA_DATE = s.FIN_PAYMENT_VND_HDR.PVH_DATE,
                                             PAA_TYPE = "PAY"
                                         }).ToList();

                var allocations = (CrDrAdjnList.Union(PaymentVndTrxAdjnList)).ToList();

                //return Invoice Trx Mpg List
                return allocations;
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

        public List<FIN_PAYMENT_VND_ALCN_DTL> GetPaymentVndAdjn(int VendorId)
        {
            List<FIN_PAYMENT_VND_ALCN_DTL> PaymentVndAdjnList = null;
            try
            {
                byte isActive = Convert.ToByte(DbActiveStatus.ACTIVE);
                byte mode = Convert.ToByte(DebitCreditModeEnum.CREDIT);


                PaymentVndAdjnList = (from rca in this.currentEntity.FIN_PAYMENT_VND_ALCN_DTL
                                      where rca.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                      && rca.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0
                                          && rca.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_VENDOR == VendorId
                                          && rca.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_ACTIVE == isActive
                                      select rca).ToList();


                return PaymentVndAdjnList;
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
        public List<FIN_PAYMENT_VND_ALCN_DTL> GetFinPaymentAlcnListContext(List<FIN_PAYMENT_VND_ALCN_DTL> finPaymentAlcnList)
        {

            try
            {
                long? tempId = null;
                finPaymentAlcnList = (from c in finPaymentAlcnList
                                      select new FIN_PAYMENT_VND_ALCN_DTL
                                      {
                                          PAD_ACTIVE = c.PAD_ACTIVE,
                                          PAD_ALCN_CDH = c.PAD_ALCN_CDH == 0 ? tempId : c.PAD_ALCN_CDH,
                                          PAD_ALCN_PAYMENT_TRX = c.PAD_ALCN_PAYMENT_TRX == 0 ? tempId : c.PAD_ALCN_PAYMENT_TRX,
                                          PAD_AMOUNT = c.PAD_AMOUNT,
                                          PAD_PAYMENT_TRX = c.PAD_PAYMENT_TRX,
                                          PAD_PK = c.PAD_PK,
                                          FIN_CRDR_NOTE_HDR = this.currentEntity.FIN_CRDR_NOTE_HDR.SingleOrDefault(r => r.CDH_PK == c.PAD_ALCN_CDH),
                                          FIN_PAYMENT_VND_TRX_MPG1 = this.currentEntity.FIN_PAYMENT_VND_TRX_MPG.SingleOrDefault(r => r.PVM_PK == c.PAD_ALCN_PAYMENT_TRX)
                                      }).ToList();


                return finPaymentAlcnList;
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
                finPaymentAlcnList = null;
            }
        }

        public List<FIN_PAYMENT_VND_ALCN_DTL> GetFinPaymentAlcnList(FIN_PAYMENT_VND_ALCN_DTL finPaymentAlcnObj)
        {
            List<FIN_PAYMENT_VND_ALCN_DTL> FinPaymentAlcnListObj = new List<FIN_PAYMENT_VND_ALCN_DTL>();
            List<FIN_PAYMENT_VND_ALCN_DTL> FinAlcnListObj = new List<FIN_PAYMENT_VND_ALCN_DTL>();
            List<FIN_PAYMENT_VND_ALCN_DTL> ResultListObj = new List<FIN_PAYMENT_VND_ALCN_DTL>();
            List<long> CrDrPksLst = new List<long>();
            List<long> PaymentPksLst = new List<long>();
            try
            {
                //FinPaymentAlcnListObj = (from pvh in this.currentEntity.FIN_PAYMENT_VND_ALCN_DTL
                //                         where pvh.PAD_PAYMENT_TRX == (finPaymentAlcnObj.PAD_PAYMENT_TRX <= 0 ? pvh.PAD_PAYMENT_TRX : finPaymentAlcnObj.PAD_PAYMENT_TRX)
                //                         select pvh
                //              ).ToList();
                //return FinPaymentAlcnListObj;
                FinPaymentAlcnListObj = (from pvh in this.currentEntity.FIN_PAYMENT_VND_ALCN_DTL
                                         where pvh.PAD_PAYMENT_TRX == (finPaymentAlcnObj.PAD_PAYMENT_TRX <= 0 ? pvh.PAD_PAYMENT_TRX : finPaymentAlcnObj.PAD_PAYMENT_TRX)
                                         select pvh
                             ).ToList();
                if (FinPaymentAlcnListObj != null && FinPaymentAlcnListObj.Count > 0)
                {
                    CrDrPksLst = (from c in FinPaymentAlcnListObj where c.PAD_ALCN_CDH.HasValue select Convert.ToInt64(c.PAD_ALCN_CDH)).ToList();
                    PaymentPksLst = (from c in FinPaymentAlcnListObj where c.PAD_ALCN_PAYMENT_TRX.HasValue select Convert.ToInt64(c.PAD_ALCN_PAYMENT_TRX)).ToList();

                    List<PaymentAdjnAllocation> CrDrAdjnList = GetCrDrAdjn(FinPaymentAlcnListObj[0].FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_VENDOR);

                    List<FIN_PAYMENT_VND_ALCN_DTL> FinPaymentVndAdjnDupCheckList = GetPaymentVndAdjn(FinPaymentAlcnListObj[0].FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_VENDOR);
                    if (FinPaymentVndAdjnDupCheckList != null && FinPaymentVndAdjnDupCheckList.Count > 0)
                    {
                        CrDrAdjnList.ForEach(CrDtl =>
                        {
                            if (FinPaymentVndAdjnDupCheckList.Where(recT => recT.PAD_ALCN_PAYMENT_TRX == CrDtl.PAA_TRXPK).Sum(a => a.PAD_AMOUNT) > 0)
                            {
                                CrDtl.PAA_AMOUNT_PAID = FinPaymentVndAdjnDupCheckList.Where(recT => recT.PAD_ALCN_PAYMENT_TRX == CrDtl.PAA_TRXPK).Sum(a => a.PAD_AMOUNT);
                                CrDtl.PAA_AMOUNT_BAL = CrDtl.PAA_AMOUNT - FinPaymentVndAdjnDupCheckList.Where(recT => recT.PAD_ALCN_PAYMENT_TRX == CrDtl.PAA_TRXPK).Sum(a => a.PAD_AMOUNT);
                            }
                            else if (FinPaymentVndAdjnDupCheckList.Where(recT => recT.PAD_ALCN_CDH == CrDtl.PAA_CRDRPK).Sum(a => a.PAD_AMOUNT) > 0)
                            {
                                CrDtl.PAA_AMOUNT_PAID = FinPaymentVndAdjnDupCheckList.Where(recT => recT.PAD_ALCN_CDH == CrDtl.PAA_CRDRPK).Sum(a => a.PAD_AMOUNT);
                                CrDtl.PAA_AMOUNT_BAL = CrDtl.PAA_AMOUNT - FinPaymentVndAdjnDupCheckList.Where(recT => recT.PAD_ALCN_CDH == CrDtl.PAA_CRDRPK).Sum(a => a.PAD_AMOUNT);
                            }
                            else
                            {
                                CrDtl.PAA_AMOUNT_PAID = 0;
                                CrDtl.PAA_AMOUNT_BAL = CrDtl.PAA_AMOUNT;
                            }
                        });
                    }
                    else
                    {
                        CrDrAdjnList.ForEach(CrDtl =>
                        {
                            CrDtl.PAA_AMOUNT_PAID = 0;
                            CrDtl.PAA_AMOUNT_BAL = CrDtl.PAA_AMOUNT;
                        });
                    }

                    long? tempId = null;

                    FinAlcnListObj = (from c in CrDrAdjnList
                                      where !CrDrPksLst.Contains(c.PAA_CRDRPK) && !PaymentPksLst.Contains(c.PAA_TRXPK)
                                      select new FIN_PAYMENT_VND_ALCN_DTL
                                      {
                                          PAD_ACTIVE = 1,
                                          PAD_ALCN_CDH = c.PAA_CRDRPK == 0 ? tempId : c.PAA_CRDRPK,
                                          PAD_ALCN_PAYMENT_TRX = c.PAA_TRXPK == 0 ? tempId : c.PAA_TRXPK,
                                          PAD_AMOUNT = 0,
                                          PAD_PAYMENT_TRX = FinPaymentAlcnListObj[0].PAD_PAYMENT_TRX,
                                          PAD_PK = 0,
                                          FIN_CRDR_NOTE_HDR = this.currentEntity.FIN_CRDR_NOTE_HDR.SingleOrDefault(r => r.CDH_PK == c.PAA_CRDRPK),
                                          FIN_PAYMENT_VND_TRX_MPG1 = this.currentEntity.FIN_PAYMENT_VND_TRX_MPG.SingleOrDefault(r => r.PVM_PK == c.PAA_TRXPK)
                                      }).ToList();
                    var query = FinPaymentAlcnListObj.Union(FinAlcnListObj);
                    ResultListObj = query.ToList();
                    ResultListObj = ResultListObj.OrderBy(c => c.PAD_ALCN_CDH).ThenBy(s => s.PAD_ALCN_PAYMENT_TRX).ToList();
                }
                return ResultListObj;
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
                FinPaymentAlcnListObj = null;
            }
        }
    }
}
