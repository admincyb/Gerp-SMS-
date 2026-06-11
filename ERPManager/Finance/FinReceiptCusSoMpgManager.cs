using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;
using ERPManager.Sales;
using BusinessObject.SaleOrder;

namespace ERPManager
{
    public class FinReceiptCusSoMpgManager : IFinReceiptCusSoMpgManager
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

        public FinReceiptCusSoMpgManager(ERPEntities currentEntity)
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


        public List<FIN_RECEIPT_CUS_SO_MPG> GetFinPatmentVndPoMpgList(FIN_RECEIPT_CUS_SO_MPG finReceiptCusSOMpgObj)
        {
            List<FIN_RECEIPT_CUS_SO_MPG> FinReceiptCusSOMpgListObj = new List<FIN_RECEIPT_CUS_SO_MPG>();
            try
            {
                FinReceiptCusSOMpgListObj = (from pvh in this.currentEntity.FIN_RECEIPT_CUS_SO_MPG
                                             where pvh.RSO_RECEIPT_TRX_MPG == (finReceiptCusSOMpgObj.RSO_RECEIPT_TRX_MPG == 0 ? pvh.RSO_RECEIPT_TRX_MPG : finReceiptCusSOMpgObj.RSO_RECEIPT_TRX_MPG)
                                             select pvh
                              ).ToList();
                return FinReceiptCusSOMpgListObj;
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
                FinReceiptCusSOMpgListObj = null;
            }
        }
        public long? SaveReceiptSplit(List<FIN_RECEIPT_CUS_SO_MPG> finReceiptCusSOMpgList, ref long? maxID, ref long? MaxTaxId, Byte Category, long? invoiceHdr, decimal? discTotAmount, decimal? Excess, decimal? old_Excess)
        {
            long? retval;
            retval = 0;

            //long? maxPK;
            long? taxMaxID;
            int soHdr;
            soHdr = 0;

            List<FIN_RECEIPT_CUS_SO_MPG> Old_finReceiptCusSOMpgList;

            FIN_RECEIPT_CUS_SO_MPG Old_finReceiptCusSOMpg;
            SAL_ORDER_HDR obj_SAL_ORDER_HDR;

            List<FIN_RECEIPT_CUS_TAX_DTL> finReceiptCusTaxDtlList;
            FinReceiptCustaxDtlManager finReceiptCusTaxDtlManagerObj;



            try
            {
                if (finReceiptCusSOMpgList.Count > 0)
                {
                    if (!maxID.HasValue || maxID.Value == 0)
                        maxID = currentEntity.FIN_RECEIPT_CUS_SO_MPG.Max(v => (long?)v.RSO_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    //maxPK = soPK == 0 ? currentEntity.FIN_RECEIPT_CUS_SO_MPG.Max(v => (long?)v.RSO_PK) : soPK;
                    //maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;
                    //taxMaxID = currentEntity.FIN_RECEIPT_CUS_TAX_DTL.Max(v => (long?)v.RDT_PK);

                    //  Delete Removed Entries

                    List<long> pks = (from old1 in finReceiptCusSOMpgList
                                      select old1.RSO_PK).ToList();
                    /**/
                    long RSO_RECEIPT_TRX_MPG = finReceiptCusSOMpgList[0].RSO_RECEIPT_TRX_MPG;

                    Old_finReceiptCusSOMpgList = (from oldp in this.currentEntity.FIN_RECEIPT_CUS_SO_MPG
                                                  where oldp.RSO_RECEIPT_TRX_MPG == RSO_RECEIPT_TRX_MPG
                                                  //&& !pks.Contains(oldp.RSO_PK)
                                                  select oldp).ToList();

                    foreach (FIN_RECEIPT_CUS_SO_MPG old_FIN_RECEIPT_CUS_SO_MPG in Old_finReceiptCusSOMpgList)
                    {
                        obj_SAL_ORDER_HDR = currentEntity.SAL_ORDER_HDR.SingleOrDefault(a => a.SOH_PK == old_FIN_RECEIPT_CUS_SO_MPG.RSO_SO_HDR);

                        if (obj_SAL_ORDER_HDR != null)
                        {
                            obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED -= (old_FIN_RECEIPT_CUS_SO_MPG.RSO_RECEIVED_AMOUNT - (decimal)old_Excess);
                        }

                        //this.currentEntity.FIN_RECEIPT_CUS_SO_MPG.DeleteObject(old_FIN_RECEIPT_CUS_SO_MPG);
                    }


                    foreach (FIN_RECEIPT_CUS_SO_MPG FIN_RECEIPT_CUS_SO_MPG_obj in finReceiptCusSOMpgList)
                    {

                        finReceiptCusTaxDtlList = FIN_RECEIPT_CUS_SO_MPG_obj.FIN_RECEIPT_CUS_TAX_DTL == null ?
       new List<FIN_RECEIPT_CUS_TAX_DTL>() : FIN_RECEIPT_CUS_SO_MPG_obj.FIN_RECEIPT_CUS_TAX_DTL.ToList();
                        FIN_RECEIPT_CUS_SO_MPG_obj.FIN_RECEIPT_CUS_TAX_DTL.Clear();


                        if (FIN_RECEIPT_CUS_SO_MPG_obj.RSO_PK == 0) //  INSERT NEW RECORD
                        {
                            FIN_RECEIPT_CUS_SO_MPG_obj.RSO_PK = maxID.Value;

                            currentEntity.FIN_RECEIPT_CUS_SO_MPG.AddObject(FIN_RECEIPT_CUS_SO_MPG_obj);
                            retval = maxID;

                            obj_SAL_ORDER_HDR = currentEntity.SAL_ORDER_HDR.SingleOrDefault(a => a.SOH_PK == FIN_RECEIPT_CUS_SO_MPG_obj.RSO_SO_HDR);
                            obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED += (FIN_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIVED_AMOUNT - (decimal)Excess);

                            maxID++;
                            decimal splitTaxAMOUNT = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_TAX_AMOUNT;
                            if (finReceiptCusTaxDtlList.Count > 0)
                            {
                                finReceiptCusTaxDtlList.ForEach(dtl =>
                                {
                                    dtl.RDT_RECEIPT_TRX = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIPT_TRX_MPG;
                                    dtl.RDT_CUS_SO_MPG = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_PK;
                                    soHdr = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_SO_HDR;
                                });
                                finReceiptCusTaxDtlManagerObj = new FinReceiptCustaxDtlManager(this.currentEntity);
                                taxMaxID = finReceiptCusTaxDtlManagerObj.SaveReceiptTax(finReceiptCusTaxDtlList, ref MaxTaxId, soHdr, splitTaxAMOUNT, Category, invoiceHdr, discTotAmount);
                                taxMaxID = taxMaxID + 1;
                                //soMpgPK = finReceiptCusSOMpgManagerObj.SaveReceiptSplit(finReceiptCusSoMpgList, (long)soMpgPK);200 

                                //soMpgPK = soMpgPK == null ? 0 : soMpgPK;
                            }
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            obj_SAL_ORDER_HDR = currentEntity.SAL_ORDER_HDR.SingleOrDefault(a => a.SOH_PK == FIN_RECEIPT_CUS_SO_MPG_obj.RSO_SO_HDR);
                            Old_finReceiptCusSOMpg = currentEntity.FIN_RECEIPT_CUS_SO_MPG.SingleOrDefault(a => a.RSO_PK == FIN_RECEIPT_CUS_SO_MPG_obj.RSO_PK);

                            if (obj_SAL_ORDER_HDR != null)
                            {
                                obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED += (FIN_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIVED_AMOUNT - (decimal)Excess);
                            }

                            Old_finReceiptCusSOMpg.RSO_RECEIVED_AMOUNT = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIVED_AMOUNT;
                            Old_finReceiptCusSOMpg.RSO_TAX_AMOUNT = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_TAX_AMOUNT;
                            Old_finReceiptCusSOMpg.RSO_OTHER_AMOUNT = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_OTHER_AMOUNT;
                            retval = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_PK;

                            decimal splitTaxAMOUNT = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_TAX_AMOUNT;
                            if (finReceiptCusTaxDtlList.Count > 0)
                            {
                                finReceiptCusTaxDtlList.ForEach(dtl =>
                                {
                                    dtl.RDT_RECEIPT_TRX = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIPT_TRX_MPG;
                                    dtl.RDT_CUS_SO_MPG = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_PK;
                                    soHdr = FIN_RECEIPT_CUS_SO_MPG_obj.RSO_SO_HDR;
                                });
                                finReceiptCusTaxDtlManagerObj = new FinReceiptCustaxDtlManager(this.currentEntity);
                                taxMaxID = finReceiptCusTaxDtlManagerObj.SaveReceiptTax(finReceiptCusTaxDtlList, ref MaxTaxId, soHdr, splitTaxAMOUNT, Category, invoiceHdr, discTotAmount);
                                taxMaxID = taxMaxID + 1;
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

        public List<FIN_RECEIPT_CUS_SO_MPG> GetFinReceiptCusSoMpgList(FIN_RECEIPT_CUS_SO_MPG finReceiptCusSoMpgObj)
        {
            List<FIN_RECEIPT_CUS_SO_MPG> FinReceiptCusSoMpgListObj = new List<FIN_RECEIPT_CUS_SO_MPG>();
            try
            {
                FinReceiptCusSoMpgListObj = (from pvh in this.currentEntity.FIN_RECEIPT_CUS_SO_MPG
                                             where pvh.RSO_RECEIPT_TRX_MPG == (finReceiptCusSoMpgObj.RSO_RECEIPT_TRX_MPG <= 0 ? pvh.RSO_RECEIPT_TRX_MPG : finReceiptCusSoMpgObj.RSO_RECEIPT_TRX_MPG)
                                             && pvh.RSO_RECEIPT_HDR == (finReceiptCusSoMpgObj.RSO_RECEIPT_HDR <= 0 ? pvh.RSO_RECEIPT_HDR : finReceiptCusSoMpgObj.RSO_RECEIPT_HDR)
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

        public List<FIN_RECEIPT_CUS_ALCN_DTL> GetFinReceiptAlcnList(FIN_RECEIPT_CUS_ALCN_DTL finReceiptAlcnObj)
        {
            List<FIN_RECEIPT_CUS_ALCN_DTL> FinReceiptAlcnListObj = new List<FIN_RECEIPT_CUS_ALCN_DTL>();
            List<FIN_RECEIPT_CUS_ALCN_DTL> FinAlcnListObj = new List<FIN_RECEIPT_CUS_ALCN_DTL>();
            List<FIN_RECEIPT_CUS_ALCN_DTL> ResultListObj = new List<FIN_RECEIPT_CUS_ALCN_DTL>();
            List<long> CrDrPksLst = new List<long>();
            List<long> PaymentPksLst = new List<long>();
            FinInvoiceCusTrxMpgManager finInvoiceVndTrxMpgManagerObj;
            finInvoiceVndTrxMpgManagerObj = new FinInvoiceCusTrxMpgManager(currentEntity);



            try
            {
                //FinReceiptAlcnListObj = (from pvh in this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL
                //                         where pvh.RAD_RECEIPT_TRX == (finReceiptAlcnObj.RAD_RECEIPT_TRX <= 0 ? pvh.RAD_RECEIPT_TRX : finReceiptAlcnObj.RAD_RECEIPT_TRX)
                //                         && pvh.FIN_RECEIPT_CUS_TRX_MPG.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS==0
                //                         select pvh
                //              ).ToList();
                FinReceiptAlcnListObj = (from pvh in this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL
                                         where pvh.RAD_RECEIPT_TRX == (finReceiptAlcnObj.RAD_RECEIPT_TRX <= 0 ? pvh.RAD_RECEIPT_TRX : finReceiptAlcnObj.RAD_RECEIPT_TRX)
                                         select pvh
                             ).ToList();
                if (FinReceiptAlcnListObj != null && FinReceiptAlcnListObj.Count > 0)
                {
                    CrDrPksLst = (from c in FinReceiptAlcnListObj where c.RAD_ALCN_CDH.HasValue select Convert.ToInt64(c.RAD_ALCN_CDH)).ToList();
                    PaymentPksLst = (from c in FinReceiptAlcnListObj where c.RAD_ALCN_RECEIPT_TRX.HasValue select Convert.ToInt64(c.RAD_ALCN_RECEIPT_TRX)).ToList();

                    List<ReceiptAdjnAllocation> CrDrAdjnList = finInvoiceVndTrxMpgManagerObj.GetCrDrAdjn(FinReceiptAlcnListObj[0].FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_CUSTOMER, FinReceiptAlcnListObj[0].FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_PK);

                    List<FIN_RECEIPT_CUS_ALCN_DTL> FinPaymentVndAdjnDupCheckList = finInvoiceVndTrxMpgManagerObj.GetReceiptCusAdjn(FinReceiptAlcnListObj[0].FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_CUSTOMER);
                    if (FinPaymentVndAdjnDupCheckList != null && FinPaymentVndAdjnDupCheckList.Count > 0)
                    {
                        CrDrAdjnList.ForEach(CrDtl =>
                        {
                            if (FinPaymentVndAdjnDupCheckList.Where(recT => recT.RAD_ALCN_RECEIPT_TRX == CrDtl.RAA_TRXPK).Sum(a => a.RAD_AMOUNT) > 0)
                            {
                                CrDtl.RAA_AMOUNT_RCVD = FinPaymentVndAdjnDupCheckList.Where(recT => recT.RAD_ALCN_RECEIPT_TRX == CrDtl.RAA_TRXPK).Sum(a => a.RAD_AMOUNT);
                                CrDtl.RAA_AMOUNT_BAL = CrDtl.RAA_AMOUNT - FinPaymentVndAdjnDupCheckList.Where(recT => recT.RAD_ALCN_RECEIPT_TRX == CrDtl.RAA_TRXPK).Sum(a => a.RAD_AMOUNT);
                            }
                            else if (FinPaymentVndAdjnDupCheckList.Where(recT => recT.RAD_ALCN_CDH == CrDtl.RAA_CRDRPK).Sum(a => a.RAD_AMOUNT) > 0)
                            {
                                CrDtl.RAA_AMOUNT_RCVD = FinPaymentVndAdjnDupCheckList.Where(recT => recT.RAD_ALCN_CDH == CrDtl.RAA_CRDRPK).Sum(a => a.RAD_AMOUNT);
                                CrDtl.RAA_AMOUNT_BAL = CrDtl.RAA_AMOUNT - FinPaymentVndAdjnDupCheckList.Where(recT => recT.RAD_ALCN_CDH == CrDtl.RAA_CRDRPK).Sum(a => a.RAD_AMOUNT);
                            }
                            else
                            {
                                CrDtl.RAA_AMOUNT_RCVD = 0;
                                CrDtl.RAA_AMOUNT_BAL = CrDtl.RAA_AMOUNT;
                            }
                        });
                    }
                    else
                    {
                        CrDrAdjnList.ForEach(CrDtl =>
                        {
                            CrDtl.RAA_AMOUNT_RCVD = 0;
                            CrDtl.RAA_AMOUNT_BAL = CrDtl.RAA_AMOUNT;
                        });
                    }

                    long? tempId = null;

                    FinAlcnListObj = (from c in CrDrAdjnList
                                      where !CrDrPksLst.Contains(c.RAA_CRDRPK) && !PaymentPksLst.Contains(c.RAA_TRXPK)
                                      select new FIN_RECEIPT_CUS_ALCN_DTL
                                      {
                                          RAD_ACTIVE = 1,
                                          RAD_ALCN_CDH = c.RAA_CRDRPK == 0 ? tempId : c.RAA_CRDRPK,
                                          RAD_ALCN_RECEIPT_TRX = c.RAA_TRXPK == 0 ? tempId : c.RAA_TRXPK,
                                          RAD_AMOUNT = 0,
                                          RAD_RECEIPT_TRX = FinReceiptAlcnListObj[0].RAD_RECEIPT_TRX,
                                          RAD_PK = 0,
                                          FIN_CRDR_NOTE_HDR = this.currentEntity.FIN_CRDR_NOTE_HDR.SingleOrDefault(r => r.CDH_PK == c.RAA_CRDRPK),
                                          FIN_RECEIPT_CUS_TRX_MPG1 = this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG.SingleOrDefault(r => r.RCM_PK == c.RAA_TRXPK)
                                      }).ToList();
                    var query = FinReceiptAlcnListObj.Union(FinAlcnListObj);
                    ResultListObj = query.ToList();

                    ResultListObj = ResultListObj.OrderBy(c => c.RAD_ALCN_RECEIPT_TRX).ThenBy(s => s.RAD_ALCN_CDH).ToList();
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
                FinReceiptAlcnListObj = null;
            }
        }
        public List<FIN_RECEIPT_CUS_ALCN_DTL> GetFinReceiptAlcnListContext(List<FIN_RECEIPT_CUS_ALCN_DTL> finReceiptAlcnList)
        {

            try
            {
                long? tempId = null;
                finReceiptAlcnList = (from c in finReceiptAlcnList
                                      select new FIN_RECEIPT_CUS_ALCN_DTL
                                      {
                                          RAD_ACTIVE = c.RAD_ACTIVE,
                                          RAD_ALCN_CDH = c.RAD_ALCN_CDH == 0 ? tempId : c.RAD_ALCN_CDH,
                                          RAD_ALCN_RECEIPT_TRX = c.RAD_ALCN_RECEIPT_TRX == 0 ? tempId : c.RAD_ALCN_RECEIPT_TRX,
                                          RAD_AMOUNT = c.RAD_AMOUNT,
                                          RAD_RECEIPT_TRX = c.RAD_RECEIPT_TRX,
                                          RAD_PK = c.RAD_PK,
                                          FIN_CRDR_NOTE_HDR = this.currentEntity.FIN_CRDR_NOTE_HDR.SingleOrDefault(r => r.CDH_PK == c.RAD_ALCN_CDH),
                                          FIN_RECEIPT_CUS_TRX_MPG1 = this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG.SingleOrDefault(r => r.RCM_PK == c.RAD_ALCN_RECEIPT_TRX)
                                      }).ToList();

                //finReceiptAlcnList.ForEach(d=>
                //  {
                //      d.FIN_CRDR_NOTE_HDR = this.currentEntity.FIN_CRDR_NOTE_HDR.SingleOrDefault(r => r.CDH_PK==d.RAD_ALCN_CDH);
                //      d.FIN_RECEIPT_CUS_TRX_MPG1 = this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG.SingleOrDefault(r => r.RCM_PK == d.RAD_ALCN_RECEIPT_TRX);
                //      d.FIN_RECEIPT_CUS_TRX_MPG = this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG.SingleOrDefault(r => r.RCM_PK == d.RAD_RECEIPT_TRX);
                //  });


                return finReceiptAlcnList;
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
                finReceiptAlcnList = null;
            }
        }

        public long? SavePaymentSplit(List<FIN_PAYMENT_VND_PO_MPG> finPaymentVndPoMpgList)
        {
            throw new NotImplementedException();
        }
    }
}
