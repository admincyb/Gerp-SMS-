using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;
using BusinessObject.CommonManagement;

namespace ERPManager.POInvoicing
{
    public class FinPaymentVndTaxHdrmanager : IFinPaymentVndTaxHdrmanager
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

        public FinPaymentVndTaxHdrmanager(ERPEntities currentEntity)
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
    
        public long? SaveWHTTax(List<FIN_PAYMENT_VND_TAX_HDR> finPaymentVndTaxHdrList)
        {
            long? retval=0;
            long? maxID;
            List<FIN_PAYMENT_VND_TAX_HDR> Old_finPaymentVndTaxHdrList;
            FIN_PAYMENT_VND_TAX_HDR Old_finPaymentVndTaxHdr;
            try
            {
                
                
              if (finPaymentVndTaxHdrList.Count > 0)
                {
                    maxID = currentEntity.FIN_PAYMENT_VND_TAX_HDR.Max(v => (long?)v.WTH_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    List<long> pks = (from old1 in finPaymentVndTaxHdrList
                                      select old1.WTH_PK).ToList();
                    long? WTH_PAYMENT_HDR = finPaymentVndTaxHdrList[0].WTH_PAYMENT_HDR;

                    /**/
                    Old_finPaymentVndTaxHdrList = (from oldp in this.currentEntity.FIN_PAYMENT_VND_TAX_HDR
                                                  where oldp.WTH_PAYMENT_HDR == WTH_PAYMENT_HDR
                                                  && !pks.Contains(oldp.WTH_PK)
                                                  select oldp).ToList();

                    foreach (FIN_PAYMENT_VND_TAX_HDR old_FIN_PAYMENT_VND_TAX_HDR in Old_finPaymentVndTaxHdrList)
                    {
                        //obj_PUR_ORDER_HDR = currentEntity.PUR_ORDER_HDR.SingleOrDefault(a => a.POH_PK == old_FIN_PAYMENT_VND_PO_MPG.PPO_PO_HDR);
                         
                        //if (obj_PUR_ORDER_HDR != null)
                        //{
                        //    obj_PUR_ORDER_HDR.POH_AMT_PAID -= old_FIN_PAYMENT_VND_PO_MPG.PPO_PAID_AMOUNT;
                        //}

                        this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.DeleteObject(old_FIN_PAYMENT_VND_TAX_HDR);
                    }

                    foreach (FIN_PAYMENT_VND_TAX_HDR FIN_PAYMENT_VND_TAX_HDR_obj in finPaymentVndTaxHdrList)
                    {
                        if (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK != -1)
                        {
                            if (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK == 0) //  INSERT NEW RECORD
                            {
                                FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK = maxID.Value;
                                FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY == 0) ? (byte)WHTCategoryEnum.WHT : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY;
                                currentEntity.FIN_PAYMENT_VND_TAX_HDR.AddObject(FIN_PAYMENT_VND_TAX_HDR_obj);
                                maxID++;
                                retval = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK;
                            }
                            else //UPDATE EXISTING RECORD
                            {
                                Old_finPaymentVndTaxHdr = currentEntity.FIN_PAYMENT_VND_TAX_HDR.SingleOrDefault(a => a.WTH_PK == FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK);
                                Old_finPaymentVndTaxHdr.WTH_PAYMENT_HDR = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PAYMENT_HDR;
                                Old_finPaymentVndTaxHdr.WTH_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TYPE;
                                Old_finPaymentVndTaxHdr.WTH_TAX = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX;
                                Old_finPaymentVndTaxHdr.WTH_TAX_CATEGORY = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_CATEGORY;
                                Old_finPaymentVndTaxHdr.WTH_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_NAME;
                                Old_finPaymentVndTaxHdr.WTH_AMOUNT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_AMOUNT;
                                Old_finPaymentVndTaxHdr.WTH_TAX_AMT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_AMT;

                                Old_finPaymentVndTaxHdr.WTH_FORM_NO = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_FORM_NO;
                                Old_finPaymentVndTaxHdr.WTH_PARTY_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PARTY_NAME;
                                Old_finPaymentVndTaxHdr.WTH_ADDRESS = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_ADDRESS;
                                Old_finPaymentVndTaxHdr.WTH_DESC = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_DESC;
                                Old_finPaymentVndTaxHdr.WTH_TAX_ID = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_ID;

                                Old_finPaymentVndTaxHdr.WTH_CATEGORY = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY == 0) ? (byte)WHTCategoryEnum.WHT : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY;
                                Old_finPaymentVndTaxHdr.WTH_TAX_DATE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_DATE.HasValue) ? FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_DATE : null;
                                Old_finPaymentVndTaxHdr.WTH_REFUND_DATE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_REFUND_DATE.HasValue) ? FIN_PAYMENT_VND_TAX_HDR_obj.WTH_REFUND_DATE : null;
                                Old_finPaymentVndTaxHdr.WTH_TAX_INV_NO = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_INV_NO;
                                Old_finPaymentVndTaxHdr.WTH_INV_RECEIVED = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_INV_RECEIVED;
                                Old_finPaymentVndTaxHdr.WTH_PUR_INVOICE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PUR_INVOICE == 0) ? null : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PUR_INVOICE;
                                Old_finPaymentVndTaxHdr.WTH_BRANCH_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_TYPE;
                                Old_finPaymentVndTaxHdr.WTH_BRANCH_TEXT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_TEXT;
                                Old_finPaymentVndTaxHdr.WTH_ITEM_TEXT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_ITEM_TEXT;

                                Old_finPaymentVndTaxHdr.WTH_BRANCH = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH;
                                Old_finPaymentVndTaxHdr.WTH_BRANCH_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_NAME;
                                Old_finPaymentVndTaxHdr.WTH_VENDOR = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_VENDOR;
                                Old_finPaymentVndTaxHdr.WTH_PAYMENT_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PAYMENT_TYPE;

                                if (Old_finPaymentVndTaxHdr.EntityState == EntityState.Detached)
                                    this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.Attach(Old_finPaymentVndTaxHdr);
                                if (Old_finPaymentVndTaxHdr.EntityState != EntityState.Deleted)
                                    this.currentEntity.ObjectStateManager.ChangeObjectState(Old_finPaymentVndTaxHdr, EntityState.Modified);


                                retval = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK;

                            }

                            #region Update invoice for Original inv received or not
                            FIN_INVOICE_VND_HDR objInvHdr = this.currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(r => r.IVH_PK == FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PUR_INVOICE
                                                    && FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY == (byte)WHTCategoryEnum.VATBUY);
                            if (objInvHdr != null)
                            {
                                objInvHdr.IVH_ORGINAL_RCVD = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_INV_RECEIVED;
                            } 
                            #endregion
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
        public long? SaveDirectVatTaxForPOPayment(List<FIN_PAYMENT_VND_TAX_HDR> finPaymentVndTaxHdrList)
        {
            long? retval = 0;
            long? maxID;
            List<FIN_PAYMENT_VND_TAX_HDR> Old_finPaymentVndTaxHdrList;
            FIN_PAYMENT_VND_TAX_HDR Old_finPaymentVndTaxHdr;
            try
            {


                if (finPaymentVndTaxHdrList.Count > 0)
                {
                    maxID = currentEntity.FIN_PAYMENT_VND_TAX_HDR.Max(v => (long?)v.WTH_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    List<long> pks = (from old1 in finPaymentVndTaxHdrList
                                      select old1.WTH_PK).ToList();
                    long? WTH_PAYMENT_HDR = finPaymentVndTaxHdrList[0].WTH_PAYMENT_HDR;
                    byte WTH_CATEGORY = finPaymentVndTaxHdrList[0].WTH_CATEGORY;
                    /**/
                    Old_finPaymentVndTaxHdrList = (from oldp in this.currentEntity.FIN_PAYMENT_VND_TAX_HDR
                                                   where oldp.WTH_PAYMENT_HDR == WTH_PAYMENT_HDR
                                                   && !pks.Contains(oldp.WTH_PK) && oldp.WTH_CATEGORY == WTH_CATEGORY
                                                   select oldp).ToList();
                    // (byte)WHTCategoryEnum.VATBUY  
                    foreach (FIN_PAYMENT_VND_TAX_HDR old_FIN_PAYMENT_VND_TAX_HDR in Old_finPaymentVndTaxHdrList)
                    {
                        //obj_PUR_ORDER_HDR = currentEntity.PUR_ORDER_HDR.SingleOrDefault(a => a.POH_PK == old_FIN_PAYMENT_VND_PO_MPG.PPO_PO_HDR);

                        //if (obj_PUR_ORDER_HDR != null)
                        //{
                        //    obj_PUR_ORDER_HDR.POH_AMT_PAID -= old_FIN_PAYMENT_VND_PO_MPG.PPO_PAID_AMOUNT;
                        //}

                        this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.DeleteObject(old_FIN_PAYMENT_VND_TAX_HDR);
                    }

                    foreach (FIN_PAYMENT_VND_TAX_HDR FIN_PAYMENT_VND_TAX_HDR_obj in finPaymentVndTaxHdrList)
                    {
                        if (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK != -1)
                        {
                            if (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK == 0) //  INSERT NEW RECORD
                            {
                                FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK = maxID.Value;
                                FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY == 0) ? (byte)WHTCategoryEnum.WHT : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY;
                                currentEntity.FIN_PAYMENT_VND_TAX_HDR.AddObject(FIN_PAYMENT_VND_TAX_HDR_obj);
                                maxID++;
                                retval = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK;
                            }
                            else //UPDATE EXISTING RECORD
                            {
                                Old_finPaymentVndTaxHdr = currentEntity.FIN_PAYMENT_VND_TAX_HDR.SingleOrDefault(a => a.WTH_PK == FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK);
                                Old_finPaymentVndTaxHdr.WTH_PAYMENT_HDR = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PAYMENT_HDR;
                                Old_finPaymentVndTaxHdr.WTH_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TYPE;
                                Old_finPaymentVndTaxHdr.WTH_TAX = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX;
                                Old_finPaymentVndTaxHdr.WTH_TAX_CATEGORY = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_CATEGORY;
                                Old_finPaymentVndTaxHdr.WTH_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_NAME;
                                Old_finPaymentVndTaxHdr.WTH_AMOUNT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_AMOUNT;
                                Old_finPaymentVndTaxHdr.WTH_TAX_AMT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_AMT;

                                Old_finPaymentVndTaxHdr.WTH_FORM_NO = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_FORM_NO;
                                Old_finPaymentVndTaxHdr.WTH_PARTY_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PARTY_NAME;
                                Old_finPaymentVndTaxHdr.WTH_ADDRESS = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_ADDRESS;
                                Old_finPaymentVndTaxHdr.WTH_DESC = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_DESC;
                                Old_finPaymentVndTaxHdr.WTH_TAX_ID = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_ID;

                                Old_finPaymentVndTaxHdr.WTH_CATEGORY = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY == 0) ? (byte)WHTCategoryEnum.WHT : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY;
                                Old_finPaymentVndTaxHdr.WTH_TAX_DATE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_DATE.HasValue) ? FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_DATE : null;
                                Old_finPaymentVndTaxHdr.WTH_REFUND_DATE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_REFUND_DATE.HasValue) ? FIN_PAYMENT_VND_TAX_HDR_obj.WTH_REFUND_DATE : null;
                                Old_finPaymentVndTaxHdr.WTH_TAX_INV_NO = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_INV_NO;
                                Old_finPaymentVndTaxHdr.WTH_INV_RECEIVED = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_INV_RECEIVED;
                                Old_finPaymentVndTaxHdr.WTH_PUR_INVOICE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PUR_INVOICE == 0) ? null : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PUR_INVOICE;
                                Old_finPaymentVndTaxHdr.WTH_BRANCH_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_TYPE;
                                Old_finPaymentVndTaxHdr.WTH_BRANCH_TEXT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_TEXT;
                                Old_finPaymentVndTaxHdr.WTH_ITEM_TEXT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_ITEM_TEXT;

                                Old_finPaymentVndTaxHdr.WTH_BRANCH = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH;
                                Old_finPaymentVndTaxHdr.WTH_BRANCH_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_NAME;
                                Old_finPaymentVndTaxHdr.WTH_VENDOR = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_VENDOR;
                                Old_finPaymentVndTaxHdr.WTH_PAYMENT_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PAYMENT_TYPE;

                                if (Old_finPaymentVndTaxHdr.EntityState == EntityState.Detached)
                                    this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.Attach(Old_finPaymentVndTaxHdr);
                                if (Old_finPaymentVndTaxHdr.EntityState != EntityState.Deleted)
                                    this.currentEntity.ObjectStateManager.ChangeObjectState(Old_finPaymentVndTaxHdr, EntityState.Modified);


                                retval = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK;

                            }

                            #region Update invoice for Original inv received or not
                            FIN_INVOICE_VND_HDR objInvHdr = this.currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(r => r.IVH_PK == FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PUR_INVOICE
                                                    && FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY == (byte)WHTCategoryEnum.VATBUY);
                            if (objInvHdr != null)
                            {
                                objInvHdr.IVH_ORGINAL_RCVD = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_INV_RECEIVED;
                            }
                            #endregion
                        }
                    }

                    //commented for multiple mode(if any change in payable amount then need to update the transaction, so this updation is not required)
                    //// update wht amount in payment
                    //if (WTH_CATEGORY == (byte)WHTCategoryEnum.WHT)
                    //{
                    //    decimal whtamount = 0;
                    //    try
                    //    {
                    //        whtamount = finPaymentVndTaxHdrList.Where(r => r.WTH_CATEGORY == (byte)WHTCategoryEnum.WHT).Sum(wht => wht.WTH_TAX_AMT);
                    //        FIN_PAYMENT_VND_HDR paymenthdr = currentEntity.FIN_PAYMENT_VND_HDR.SingleOrDefault(a => a.PVH_PK == WTH_PAYMENT_HDR);
                    //        paymenthdr.PVH_WHT_AMOUNT = whtamount;
                    //    }
                    //    catch { }
                    //}

                }
                return retval;
            }//Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        public long? SaveDirectWHTTax(List<FIN_PAYMENT_VND_TAX_HDR> finPaymentVndTaxHdrList)
        {
            long? retval = 0;
            long? maxID;
            List<FIN_PAYMENT_VND_TAX_HDR> Old_finPaymentVndTaxHdrList;
            FIN_PAYMENT_VND_TAX_HDR Old_finPaymentVndTaxHdr;
            try
            {


                if (finPaymentVndTaxHdrList.Count > 0)
                {
                    maxID = currentEntity.FIN_PAYMENT_VND_TAX_HDR.Max(v => (long?)v.WTH_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    List<long> pks = (from old1 in finPaymentVndTaxHdrList
                                      select old1.WTH_PK).ToList();
                    long? WTH_TRX_HDR = finPaymentVndTaxHdrList[0].WTH_TRX_HDR;

                    /**/
                    Old_finPaymentVndTaxHdrList = (from oldp in this.currentEntity.FIN_PAYMENT_VND_TAX_HDR
                                                   where oldp.WTH_TRX_HDR == WTH_TRX_HDR
                                                   && !pks.Contains(oldp.WTH_PK)
                                                   select oldp).ToList();

                    foreach (FIN_PAYMENT_VND_TAX_HDR old_FIN_PAYMENT_VND_TAX_HDR in Old_finPaymentVndTaxHdrList)
                    {
                        //obj_PUR_ORDER_HDR = currentEntity.PUR_ORDER_HDR.SingleOrDefault(a => a.POH_PK == old_FIN_PAYMENT_VND_PO_MPG.PPO_PO_HDR);

                        //if (obj_PUR_ORDER_HDR != null)
                        //{
                        //    obj_PUR_ORDER_HDR.POH_AMT_PAID -= old_FIN_PAYMENT_VND_PO_MPG.PPO_PAID_AMOUNT;
                        //}

                        this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.DeleteObject(old_FIN_PAYMENT_VND_TAX_HDR);
                    }

                    foreach (FIN_PAYMENT_VND_TAX_HDR FIN_PAYMENT_VND_TAX_HDR_obj in finPaymentVndTaxHdrList)
                    {
                        if (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK == -1)
                            break;
                        if (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK == 0) //  INSERT NEW RECORD
                        {
                            FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK = maxID.Value;
                            FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY == 0) ? (byte)WHTCategoryEnum.WHT : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY;
                            currentEntity.FIN_PAYMENT_VND_TAX_HDR.AddObject(FIN_PAYMENT_VND_TAX_HDR_obj);
                            maxID++;
                            retval = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK;
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            Old_finPaymentVndTaxHdr = currentEntity.FIN_PAYMENT_VND_TAX_HDR.SingleOrDefault(a => a.WTH_PK == FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK);
                           // Old_finPaymentVndTaxHdr.WTH_PAYMENT_HDR = null;
                            Old_finPaymentVndTaxHdr.WTH_TRX_HDR = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TRX_HDR;
                            Old_finPaymentVndTaxHdr.WTH_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TYPE;
                            Old_finPaymentVndTaxHdr.WTH_TAX = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX;
                            Old_finPaymentVndTaxHdr.WTH_TAX_CATEGORY = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_CATEGORY;
                            Old_finPaymentVndTaxHdr.WTH_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_NAME;
                            Old_finPaymentVndTaxHdr.WTH_AMOUNT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_AMOUNT;
                            Old_finPaymentVndTaxHdr.WTH_TAX_AMT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_AMT;

                            Old_finPaymentVndTaxHdr.WTH_FORM_NO = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_FORM_NO;
                            Old_finPaymentVndTaxHdr.WTH_PARTY_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PARTY_NAME;
                            Old_finPaymentVndTaxHdr.WTH_ADDRESS = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_ADDRESS;
                            Old_finPaymentVndTaxHdr.WTH_DESC = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_DESC;
                            Old_finPaymentVndTaxHdr.WTH_TAX_ID = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_ID;

                            Old_finPaymentVndTaxHdr.WTH_CATEGORY = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY == 0) ? (byte)WHTCategoryEnum.WHT : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY;
                            Old_finPaymentVndTaxHdr.WTH_TAX_DATE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_DATE.HasValue)? FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_DATE : null;
                            Old_finPaymentVndTaxHdr.WTH_REFUND_DATE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_REFUND_DATE.HasValue) ? FIN_PAYMENT_VND_TAX_HDR_obj.WTH_REFUND_DATE : null;
                            Old_finPaymentVndTaxHdr.WTH_TAX_INV_NO = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_INV_NO;
                            Old_finPaymentVndTaxHdr.WTH_INV_RECEIVED = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_INV_RECEIVED;
                            Old_finPaymentVndTaxHdr.WTH_PUR_INVOICE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PUR_INVOICE == 0) ? null : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PUR_INVOICE;
                            Old_finPaymentVndTaxHdr.WTH_BRANCH_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_TYPE;
                            Old_finPaymentVndTaxHdr.WTH_BRANCH_TEXT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_TEXT;
                            Old_finPaymentVndTaxHdr.WTH_ITEM_TEXT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_ITEM_TEXT;

                            Old_finPaymentVndTaxHdr.WTH_BRANCH = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH;
                            Old_finPaymentVndTaxHdr.WTH_BRANCH_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_NAME;
                            Old_finPaymentVndTaxHdr.WTH_VENDOR = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_VENDOR;
                            Old_finPaymentVndTaxHdr.WTH_PAYMENT_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PAYMENT_TYPE;

                            if (Old_finPaymentVndTaxHdr.EntityState == EntityState.Detached)
                                this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.Attach(Old_finPaymentVndTaxHdr);
                            if (Old_finPaymentVndTaxHdr.EntityState != EntityState.Deleted)
                                this.currentEntity.ObjectStateManager.ChangeObjectState(Old_finPaymentVndTaxHdr, EntityState.Modified);


                            retval = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK;

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

        public long? SaveDirectVatTaxForDirectPayment(List<FIN_PAYMENT_VND_TAX_HDR> finPaymentVndTaxHdrList)
        {
            long? retval = 0;
            long? maxID;
            List<FIN_PAYMENT_VND_TAX_HDR> Old_finPaymentVndTaxHdrList;
            FIN_PAYMENT_VND_TAX_HDR Old_finPaymentVndTaxHdr;
            try
            {


                if (finPaymentVndTaxHdrList.Count > 0)
                {
                    maxID = currentEntity.FIN_PAYMENT_VND_TAX_HDR.Max(v => (long?)v.WTH_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    List<long> pks = (from old1 in finPaymentVndTaxHdrList
                                      select old1.WTH_PK).ToList();
                    long? WTH_TRX_HDR = finPaymentVndTaxHdrList[0].WTH_TRX_HDR;
                    byte WTH_CATEGORY = finPaymentVndTaxHdrList[0].WTH_CATEGORY;                    
                    /**/
                    Old_finPaymentVndTaxHdrList = (from oldp in this.currentEntity.FIN_PAYMENT_VND_TAX_HDR
                                                   where oldp.WTH_TRX_HDR == WTH_TRX_HDR
                                                   && !pks.Contains(oldp.WTH_PK) && oldp.WTH_CATEGORY == WTH_CATEGORY
                                                   select oldp).ToList();
                    //(byte)WHTCategoryEnum.VATBUY
                    foreach (FIN_PAYMENT_VND_TAX_HDR old_FIN_PAYMENT_VND_TAX_HDR in Old_finPaymentVndTaxHdrList)
                    {
                        //obj_PUR_ORDER_HDR = currentEntity.PUR_ORDER_HDR.SingleOrDefault(a => a.POH_PK == old_FIN_PAYMENT_VND_PO_MPG.PPO_PO_HDR);

                        //if (obj_PUR_ORDER_HDR != null)
                        //{
                        //    obj_PUR_ORDER_HDR.POH_AMT_PAID -= old_FIN_PAYMENT_VND_PO_MPG.PPO_PAID_AMOUNT;
                        //}

                        this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.DeleteObject(old_FIN_PAYMENT_VND_TAX_HDR);
                    }

                    foreach (FIN_PAYMENT_VND_TAX_HDR FIN_PAYMENT_VND_TAX_HDR_obj in finPaymentVndTaxHdrList)
                    {
                        if (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK == -1)
                            break;
                        if (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK == 0) //  INSERT NEW RECORD
                        {
                            FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK = maxID.Value;
                            FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY == 0) ? (byte)WHTCategoryEnum.WHT : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY;
                            currentEntity.FIN_PAYMENT_VND_TAX_HDR.AddObject(FIN_PAYMENT_VND_TAX_HDR_obj);
                            maxID++;
                            retval = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK;
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            Old_finPaymentVndTaxHdr = currentEntity.FIN_PAYMENT_VND_TAX_HDR.SingleOrDefault(a => a.WTH_PK == FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK);
                            // Old_finPaymentVndTaxHdr.WTH_PAYMENT_HDR = null;
                            Old_finPaymentVndTaxHdr.WTH_CERT_NO=FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CERT_NO;
                            Old_finPaymentVndTaxHdr.WTH_TRX_HDR = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TRX_HDR;
                            Old_finPaymentVndTaxHdr.WTH_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TYPE;
                            Old_finPaymentVndTaxHdr.WTH_TAX = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX;
                            Old_finPaymentVndTaxHdr.WTH_TAX_CATEGORY = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_CATEGORY;
                            Old_finPaymentVndTaxHdr.WTH_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_NAME;
                            Old_finPaymentVndTaxHdr.WTH_AMOUNT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_AMOUNT;
                            Old_finPaymentVndTaxHdr.WTH_TAX_AMT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_AMT;

                            Old_finPaymentVndTaxHdr.WTH_FORM_NO = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_FORM_NO;
                            Old_finPaymentVndTaxHdr.WTH_PARTY_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PARTY_NAME;
                            Old_finPaymentVndTaxHdr.WTH_ADDRESS = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_ADDRESS;
                            Old_finPaymentVndTaxHdr.WTH_DESC = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_DESC;
                            Old_finPaymentVndTaxHdr.WTH_TAX_ID = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_ID;

                            Old_finPaymentVndTaxHdr.WTH_CATEGORY = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY == 0) ? (byte)WHTCategoryEnum.WHT : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_CATEGORY;
                            Old_finPaymentVndTaxHdr.WTH_TAX_DATE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_DATE.HasValue) ? FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_DATE : null;
                            Old_finPaymentVndTaxHdr.WTH_REFUND_DATE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_REFUND_DATE.HasValue) ? FIN_PAYMENT_VND_TAX_HDR_obj.WTH_REFUND_DATE : null;
                            Old_finPaymentVndTaxHdr.WTH_TAX_INV_NO = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_TAX_INV_NO;
                            Old_finPaymentVndTaxHdr.WTH_INV_RECEIVED = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_INV_RECEIVED;
                            Old_finPaymentVndTaxHdr.WTH_PUR_INVOICE = (FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PUR_INVOICE == 0) ? null : FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PUR_INVOICE;
                            Old_finPaymentVndTaxHdr.WTH_BRANCH_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_TYPE;
                            Old_finPaymentVndTaxHdr.WTH_BRANCH_TEXT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_TEXT;
                            Old_finPaymentVndTaxHdr.WTH_ITEM_TEXT = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_ITEM_TEXT;

                            Old_finPaymentVndTaxHdr.WTH_BRANCH = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH;
                            Old_finPaymentVndTaxHdr.WTH_BRANCH_NAME = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_BRANCH_NAME;
                            Old_finPaymentVndTaxHdr.WTH_VENDOR = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_VENDOR;
                            Old_finPaymentVndTaxHdr.WTH_PAYMENT_TYPE = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PAYMENT_TYPE;

                            if (Old_finPaymentVndTaxHdr.EntityState == EntityState.Detached)
                                this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.Attach(Old_finPaymentVndTaxHdr);
                            if (Old_finPaymentVndTaxHdr.EntityState != EntityState.Deleted)
                                this.currentEntity.ObjectStateManager.ChangeObjectState(Old_finPaymentVndTaxHdr, EntityState.Modified);


                            retval = FIN_PAYMENT_VND_TAX_HDR_obj.WTH_PK;

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
