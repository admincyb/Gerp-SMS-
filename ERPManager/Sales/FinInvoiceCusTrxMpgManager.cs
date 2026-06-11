using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;

using ERPManager;
using BusinessObject.CommonManagement;
using BusinessObject.SaleOrder;

namespace ERPManager
{

    public class FinInvoiceCusTrxMpgManager : IFinInvoiceCusTrxMpgManager
    {
        #region Private Variables
        /// <summary>
        /// Gets or sets current db context
        /// </summary>
        private ERPEntities currentEntity;
        private ReceiptAdjnAllocation ReceiptAdjnObj;

        #endregion

        #region Manager Methods
        /// <summary>
        /// Initializes a new instance of the InvoiceListManager class
        /// </summary>
        /// <param name="currentEntity">Current db context</param>
        public FinInvoiceCusTrxMpgManager(ERPEntities currentEntity)
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
        /// <summary>
        /// Save Sales Invoice Transaction Mapping Details
        /// </summary>
        /// <param name="InvoiceTrxMpgList"></param>
        /// <param name="isWkfSave"></param>
        /// <returns></returns>
        public long SaveSalesInvoiceTrxMpg(List<FIN_INVOICE_CUS_TRX_MPG> InvoiceTrxMpgList, byte type, bool isWkfSave = false)
        {
            long retval = 0;
            long? maxPK;
            long InvoiceHdr = 0;

            FIN_INVOICE_CUS_TRX_MPG OLD_FIN_INVOICE_CUS_TRX_MPG_Obj;
            SAL_ORDER_HDR salOrderHdrObj;

            List<FIN_INVOICE_CUS_TRX_MPG> OLD_FIN_INVOICE_CUS_TRX_MPG_List_Obj;
            FIN_INVOICE_CUS_HDR finInvoiceCusHdr;
            try
            {
                //Set save status zero,save failed
                retval = 0;
                //Getting last Payment Mpg pk
                maxPK = currentEntity.FIN_INVOICE_CUS_TRX_MPG.Max(v => (int?)v.ICM_PK);
                maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;

                if (InvoiceTrxMpgList.Count > 0)
                {
                    List<long> pks = (from old1 in InvoiceTrxMpgList
                                      select old1.ICM_PK).ToList();

                    InvoiceHdr = InvoiceTrxMpgList[0].ICM_INVOICE_HDR;

                    OLD_FIN_INVOICE_CUS_TRX_MPG_List_Obj = (from old in this.currentEntity.FIN_INVOICE_CUS_TRX_MPG
                                                            where InvoiceHdr == old.ICM_INVOICE_HDR
                                                                && !pks.Contains(old.ICM_PK)
                                                            select old).ToList();

                    foreach (FIN_INVOICE_CUS_TRX_MPG old_FIN_INVOICE_CUS_TRX_MPG in OLD_FIN_INVOICE_CUS_TRX_MPG_List_Obj)
                    {
                        //Reduce old Inviced Amount if Invoice was Submitted
                        if (old_FIN_INVOICE_CUS_TRX_MPG.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0)
                        {
                            salOrderHdrObj = currentEntity.SAL_ORDER_HDR.SingleOrDefault(sah => sah.SOH_PK == old_FIN_INVOICE_CUS_TRX_MPG.ICM_SO_HDR);
                            if (salOrderHdrObj != null)
                            {
                                if (type != (byte)SalesInvoiceType.Proforma)
                                    salOrderHdrObj.SOH_AMT_INVOICED -= old_FIN_INVOICE_CUS_TRX_MPG.ICM_AMOUNT;
                                else
                                    salOrderHdrObj.SOH_AMT_PINVOICED -= old_FIN_INVOICE_CUS_TRX_MPG.ICM_AMOUNT;
                            }
                        }
                        this.currentEntity.FIN_INVOICE_CUS_TRX_MPG.DeleteObject(old_FIN_INVOICE_CUS_TRX_MPG);
                    }
                }


                foreach (FIN_INVOICE_CUS_TRX_MPG FIN_INVOICE_CUS_TRX_MPG_Obj in InvoiceTrxMpgList)
                {
                    //check invoice Mpg pk is zero,save invoice Mpg as new record
                    if (FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_PK == 0)
                    {
                        //Set next invoice Mpg pk
                        FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_PK = (long)maxPK;

                        //Add new invoice Mpg to the db context
                        currentEntity.FIN_INVOICE_CUS_TRX_MPG.AddObject(FIN_INVOICE_CUS_TRX_MPG_Obj);
                        maxPK++;
                        retval = FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_PK;
                    }
                    //updating invoice Mpg details
                    else
                    {
                        //Get current invoice Mpg details using invoice Mpg  pk
                        OLD_FIN_INVOICE_CUS_TRX_MPG_Obj = currentEntity.FIN_INVOICE_CUS_TRX_MPG.SingleOrDefault(v => v.ICM_PK == FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_PK);
                        if (OLD_FIN_INVOICE_CUS_TRX_MPG_Obj != null)
                        {
                            //Reduce old Inviced Amount if Invoice was Submitted
                            if (OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0)
                            {
                                salOrderHdrObj = currentEntity.SAL_ORDER_HDR.SingleOrDefault(sah => sah.SOH_PK == OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_SO_HDR);
                                if (salOrderHdrObj != null)
                                {
                                    if (type != (byte)SalesInvoiceType.Proforma)
                                        salOrderHdrObj.SOH_AMT_INVOICED -= OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_AMOUNT;
                                    else
                                        salOrderHdrObj.SOH_AMT_PINVOICED -= OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_AMOUNT;
                                }
                            }
                            //Update invoice Mpg details
                            OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_AMOUNT = FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_AMOUNT;
                            OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_ACTIVE = FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_ACTIVE;

                            OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_SO_HDR = FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_SO_HDR;

                            //Set return value as invoice Mpg pk
                            retval = FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_PK;
                        }
                        else
                        {
                            //throws exception already deleted or modified by other user
                            //throw new OptimisticConcurrencyException(gComsManagerRes.EditConcurrencyException);
                        }
                    }

                    //Update Invoiced amount of PO : PUR_ORDER_HDR
                    finInvoiceCusHdr = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(inv => inv.ICH_PK == FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_INVOICE_HDR);
                    if (isWkfSave || (finInvoiceCusHdr != null && finInvoiceCusHdr.ICH_STATUS > 0))
                    {
                        salOrderHdrObj = currentEntity.SAL_ORDER_HDR.SingleOrDefault(sah => sah.SOH_PK == FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_SO_HDR);
                        if (salOrderHdrObj != null)
                        {
                            if (type != (byte)SalesInvoiceType.Proforma)
                                salOrderHdrObj.SOH_AMT_INVOICED += FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_AMOUNT;
                            else
                                salOrderHdrObj.SOH_AMT_PINVOICED += FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_AMOUNT;
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

        public List<FIN_INVOICE_CUS_TRX_MPG> GetSalesInvoiceTrxMpg(FIN_INVOICE_CUS_TRX_MPG InvoiceHdrObj, ServiceUtility utilityObj = null)
        {
            List<FIN_INVOICE_CUS_TRX_MPG> InvoiceTrxMpgList = null;
            try
            {

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

        #endregion


        #region Private Methods
        #endregion

        public List<FIN_INVOICE_CUS_TRX_MPG> GetInvoiceTrxMpg(FIN_INVOICE_CUS_TRX_MPG FinInvoiceCusTrxMpgObj)
        {
            List<FIN_INVOICE_CUS_TRX_MPG> InvoiceTrxMpgList = null;
            try
            {

                InvoiceTrxMpgList = (from pvh in this.currentEntity.FIN_INVOICE_CUS_TRX_MPG
                                     where pvh.ICM_INVOICE_HDR == (FinInvoiceCusTrxMpgObj.ICM_INVOICE_HDR == 0 ? pvh.ICM_INVOICE_HDR : FinInvoiceCusTrxMpgObj.ICM_INVOICE_HDR)
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
        public List<FIN_RECEIPT_CUS_ALCN_DTL> GetReceiptCusAdjn(int cusId)
        {
            List<FIN_RECEIPT_CUS_ALCN_DTL> ReceiptCusAdjnList = null;
            try
            {
                byte isActive = Convert.ToByte(DbActiveStatus.ACTIVE);
                byte mode = Convert.ToByte(DebitCreditModeEnum.CREDIT);


                ReceiptCusAdjnList = (from rca in this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL
                                      where rca.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS==0
                                      && rca.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0 
                                          && rca.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_CUSTOMER == cusId
                                          && rca.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_ACTIVE == isActive
                                      select rca).ToList();

               
                return ReceiptCusAdjnList;
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

        public List<ReceiptAdjnAllocation> GetCrDrAdjn(int cusId,long ReceiptPk)
        {
            //List<FIN_CRDR_NOTE_HDR> CrDrAdjnList = null;
            //List<FIN_RECEIPT_CUS_TRX_MPG> ReceiptCusTrxAdjnList = null;
            List<ReceiptAdjnAllocation> CrDrAdjnList = null;
            List<ReceiptAdjnAllocation> ReceiptCusTrxAdjnList = null;
            try
            {
                byte isActive = Convert.ToByte(DbActiveStatus.ACTIVE);
                byte mode = Convert.ToByte(DebitCreditModeEnum.CREDIT);

                CrDrAdjnList = (from pvh in this.currentEntity.FIN_CRDR_NOTE_HDR
                                where pvh.CDH_IS_DELETED == false
                                          && pvh.CDH_CUSTOMER == cusId
                                          && pvh.CDH_STATUS == 2
                                          && pvh.CDH_ACTIVE == isActive
                                          && pvh.CDH_TYPE == mode
                                          && pvh.CDH_NO_RCP_ALLOC == 0
                                          && pvh.FIN_CRDR_NOTE_MPG.Where(r=>r.FIN_INVOICE_CUS_HDR.ICH_CATEGORY == (int)SalesInvoiceCategory.Advanced).Count() == 0 // Exclude CN, that created from an advance invoice
                                select pvh).ToList().
                                Select(s => new ReceiptAdjnAllocation
                                {
                                    RAA_AMOUNT = s.CDH_AMOUNT_TC + s.CDH_OTHER_CHARGE + s.CDH_SHIP_CHARGE,
                                    RAA_CRDRPK = s.CDH_PK,
                                    RAA_NO = s.CDH_NO,
                                    RAA_TRXPK = 0,
                                    RAA_DATE=s.CDH_DATE,
                                    RAA_TYPE="CN"
                                }).ToList();

                ReceiptCusTrxAdjnList = (from rch in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG
                                         where rch.FIN_RECEIPT_CUS_HDR.RCH_CUSTOMER == cusId
                                         && rch.RCM_ACTIVE == isActive
                                         && rch.FIN_RECEIPT_CUS_HDR.RCH_STATUS == 2
                                         && rch.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                         && rch.FIN_RECEIPT_CUS_HDR.RCH_HAS_JRNL_ENTRY == true
                                         && rch.RCM_EXCESS_AMOUNT > 0
                                         && rch.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED==0
                                         //Allocation needed for both export and domestic
                                         //&& rch.FIN_INVOICE_CUS_HDR.ICH_TYPE!=1//In case of dom,no allocation
                                         && rch.RCM_RECEIPT_HDR !=ReceiptPk
                                         select rch).ToList().
                                         Select(s => new ReceiptAdjnAllocation
                                {
                                    RAA_AMOUNT = s.RCM_EXCESS_AMOUNT,
                                    RAA_CRDRPK = 0,
                                    RAA_NO = s.FIN_RECEIPT_CUS_HDR.RCH_NO,
                                    RAA_TRXPK = s.RCM_PK,
                                    RAA_DATE = s.FIN_RECEIPT_CUS_HDR.RCH_DATE,
                                    RAA_TYPE = "REC"
                                }).ToList();

                var allocations = (CrDrAdjnList.Union(ReceiptCusTrxAdjnList))
                  .ToList();
                //var a=CrDrAdjnList.Union(
                //    .ToList()
                //    .Select(s=> new
                //    {
                //      Raa







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
    }
}
