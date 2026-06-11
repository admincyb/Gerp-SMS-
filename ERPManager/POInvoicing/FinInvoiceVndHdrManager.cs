using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;
using ERPManager;
using System.Data.Objects;
using BusinessObject.CommonManagement;
using ERP.Utilities;

namespace ERPManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InvoiceListManager" in both code and config file together.
    public class FinInvoiceVndHdrManager : IFinInvoiceVndHdrManager
    {
        #region Private Variables
        /// <summary>
        /// Gets or sets current db context
        /// </summary>
        private ERPEntities currentEntity;
        #endregion

        #region Manager Methods

        /// <summary>
        /// Initializes a new instance of the InvoiceListManager class
        /// </summary>
        /// <param name="currentEntity">Current db context</param>
        public FinInvoiceVndHdrManager(ERPEntities currentEntity)
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
        /// Saves list of Invoice Header
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <param name="isWkfSave"></param>
        /// <returns></returns>
        public long SaveInvoiceHdr(List<FIN_INVOICE_VND_HDR> InvoiceHdrList, bool isWkfSave = false, bool IsAdvInvHasTax = true)
        {
            long retval = 0;
            FIN_INVOICE_VND_HDR OldFIN_INVOICE_VND_HDR_Obj;
            List<FIN_INVOICE_VND_TRX_MPG> FIN_INVOICE_VND_TRX_MPG_LST_Obj;
            FinInvoiceVndTrxMpgManager FinInvoiceVndTrxMpgManagerObj;
            long? maxINVPK;
            CommonFunctionsManager ComnFnManagerObj = new CommonFunctionsManager(this.currentEntity);

            try
            {
                retval = 0;
                foreach (FIN_INVOICE_VND_HDR FIN_INVOICE_VND_HDR_Obj in InvoiceHdrList)
                {
                    FIN_INVOICE_VND_TRX_MPG_LST_Obj = FIN_INVOICE_VND_HDR_Obj.FIN_INVOICE_VND_TRX_MPG.ToList();
                    FIN_INVOICE_VND_HDR_Obj.FIN_INVOICE_VND_TRX_MPG.Clear();

                    if (FIN_INVOICE_VND_HDR_Obj.IVH_PK == 0)
                    {
                        #region Amount Checking
                        foreach (FIN_INVOICE_VND_TRX_MPG objTrxMpg in FIN_INVOICE_VND_TRX_MPG_LST_Obj)
                        {
                            PUR_ORDER_HDR objPODet = this.currentEntity.PUR_ORDER_HDR.SingleOrDefault(r => r.POH_PK == objTrxMpg.IVM_PO_HDR);
                            decimal InvoicedAmount = 0;
                            decimal AdvInvoicedAmount = 0;
                            decimal AllocatedAdvAmount = 0;
                            decimal BalanceAmount = 0;
                            if (objPODet.FIN_INVOICE_VND_TRX_MPG != null && objPODet.FIN_INVOICE_VND_TRX_MPG.Count > 0)
                            {
                                AdvInvoicedAmount = objPODet.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT);
                                InvoicedAmount = objPODet.FIN_INVOICE_VND_TRX_MPG.FirstOrDefault().FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 ? objPODet.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY != (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT - c.IVM_DISCOUNT_AMOUNT + c.IVM_TAX_AMOUNT + c.IVM_OTHER_AMOUNT) : 0;
                                InvoicedAmount = InvoicedAmount + AdvInvoicedAmount;

                                if (objPODet.POH_TYPE == (byte)PurchaseType.Import || !IsAdvInvHasTax)
                                {
                                    InvoicedAmount = objPODet.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY != (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT - c.IVM_DISCOUNT_AMOUNT + c.IVM_TAX_AMOUNT + c.IVM_OTHER_AMOUNT);
                                    AdvInvoicedAmount = objPODet.FIN_INVOICE_VND_TRX_MPG.FirstOrDefault().FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 ? objPODet.FIN_INVOICE_VND_TRX_MPG.FirstOrDefault().PUR_ORDER_HDR.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT) : objPODet.FIN_INVOICE_VND_TRX_MPG.FirstOrDefault().PUR_ORDER_HDR.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT);
                                    AllocatedAdvAmount = objPODet.FIN_INVOICE_VND_TRX_MPG.FirstOrDefault().FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_ADV_DED_DTL.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.VAD_AMOUNT);
                                    InvoicedAmount = InvoicedAmount + AllocatedAdvAmount;
                                    if (InvoicedAmount < 0)
                                        InvoicedAmount = 0;
                                    if ((objPODet.POH_TOTAL_VALUE - InvoicedAmount) > (objPODet.POH_TOTAL_VALUE - AdvInvoicedAmount))
                                    {
                                        BalanceAmount = objPODet.POH_TOTAL_VALUE.Value - AdvInvoicedAmount;
                                    }
                                    else
                                    {
                                        BalanceAmount = objPODet.POH_TOTAL_VALUE.Value - InvoicedAmount;
                                    }
                                }
                                else
                                {
                                    AllocatedAdvAmount = objPODet.FIN_INVOICE_VND_ADV_DED_DTL.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.VAD_AMOUNT - c.VAD_OTHER_AMOUNT - c.VAD_TAX_AMOUNT);
                                    InvoicedAmount -= AllocatedAdvAmount;
                                    BalanceAmount = (Convert.ToDecimal(objPODet.POH_TOTAL_VALUE) - InvoicedAmount);
                                }

                                if (objTrxMpg.IVM_AMOUNT > BalanceAmount)
                                {
                                    retval = (long)DbSaveStatus.AMOUNTEXCEEDS;
                                    break;
                                }
                            }
                        }

                        if (retval == (long)DbSaveStatus.AMOUNTEXCEEDS)
                            break; 
                        #endregion


                        // Gets last FIN_PAYMENT_VND_HDR pk
                        maxINVPK = this.currentEntity.FIN_INVOICE_VND_HDR.Max(pvh => (int?)pvh.IVH_PK);

                        // Sets return value as next FIN_PAYMENT_VND_HDR pk
                        retval = (maxINVPK.HasValue ? maxINVPK.Value + 1 : 1);

                        FIN_INVOICE_VND_HDR_Obj.IVH_PK = retval;
                        FIN_INVOICE_VND_HDR_Obj.IVH_CRTD_DT = DateTime.Now;
                        FIN_INVOICE_VND_HDR_Obj.IVH_MOD_DT = DateTime.Now;

                        //Normal transaction=1,trading=2
                        FIN_INVOICE_VND_HDR_Obj.IVH_TRX_TYPE = 1;

                        // Add new FIN_INVOICE_VND_HDR to the db context
                        this.currentEntity.FIN_INVOICE_VND_HDR.AddObject(FIN_INVOICE_VND_HDR_Obj);
                        //set the FIN_INVOICE_VND_HDR pk as the fk of  FIN_INVOICE_VND_TRX_MPG
                        FIN_INVOICE_VND_TRX_MPG_LST_Obj.ForEach(dtl => dtl.IVM_INVOICE_HDR = retval);
                        FinInvoiceVndTrxMpgManagerObj = new FinInvoiceVndTrxMpgManager(this.currentEntity);
                        //Save Mapping Details
                        FinInvoiceVndTrxMpgManagerObj.SaveInvoiceTrxMpg(FIN_INVOICE_VND_TRX_MPG_LST_Obj, isWkfSave);
                    }
                    else
                    {
                        // updating FIN_INVOICE_VND_HDR
                        // Get current FIN_PAYMENT_VND_HDR using FIN_PAYMENT_VND_HDR pk and last modified date time,used for concurrency checking
                        OldFIN_INVOICE_VND_HDR_Obj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == FIN_INVOICE_VND_HDR_Obj.IVH_PK && sah.IVH_MOD_DT == FIN_INVOICE_VND_HDR_Obj.IVH_MOD_DT);

                        // If oldPaymentHdrObj is null then,anyone modified or deleted the record
                        if (OldFIN_INVOICE_VND_HDR_Obj != null)
                        {

                            #region Amount Checking
                            foreach (FIN_INVOICE_VND_TRX_MPG objTrxMpg in FIN_INVOICE_VND_TRX_MPG_LST_Obj)
                            {
                                FIN_INVOICE_VND_TRX_MPG objOldTrxMpg = OldFIN_INVOICE_VND_HDR_Obj.FIN_INVOICE_VND_TRX_MPG.Where(r => r.IVM_PO_HDR == objTrxMpg.IVM_PO_HDR && r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).FirstOrDefault();
                                PUR_ORDER_HDR objPODet = this.currentEntity.PUR_ORDER_HDR.SingleOrDefault(r => r.POH_PK == objTrxMpg.IVM_PO_HDR);
                                decimal InvoicedAmount = 0;
                                decimal AdvInvoicedAmount = 0;
                                decimal AllocatedAdvAmount = 0;
                                decimal BalanceAmount = 0;
                                if (objPODet.FIN_INVOICE_VND_TRX_MPG != null && objPODet.FIN_INVOICE_VND_TRX_MPG.Count > 0)
                                {
                                    AdvInvoicedAmount = objPODet.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT);
                                    InvoicedAmount = objPODet.FIN_INVOICE_VND_TRX_MPG.FirstOrDefault().FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 ? objPODet.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY != (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT - c.IVM_DISCOUNT_AMOUNT + c.IVM_TAX_AMOUNT + c.IVM_OTHER_AMOUNT) : 0;
                                    InvoicedAmount = OldFIN_INVOICE_VND_HDR_Obj.IVH_STATUS > 0 ? (InvoicedAmount + AdvInvoicedAmount) - (objOldTrxMpg.IVM_AMOUNT) : InvoicedAmount + AdvInvoicedAmount;

                                    if (objPODet.POH_TYPE == (byte)PurchaseType.Import || !IsAdvInvHasTax)
                                    {
                                        InvoicedAmount = objPODet.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY != (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT - c.IVM_DISCOUNT_AMOUNT + c.IVM_TAX_AMOUNT + c.IVM_OTHER_AMOUNT);
                                        AdvInvoicedAmount = objPODet.FIN_INVOICE_VND_TRX_MPG.FirstOrDefault().FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 ? objPODet.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT) - (OldFIN_INVOICE_VND_HDR_Obj.IVH_STATUS > 0 ? objOldTrxMpg.IVM_AMOUNT : 0) : objPODet.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT);
                                        AllocatedAdvAmount = objPODet.FIN_INVOICE_VND_TRX_MPG.FirstOrDefault().FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_ADV_DED_DTL.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.VAD_AMOUNT);
                                        InvoicedAmount = InvoicedAmount + AllocatedAdvAmount;
                                        if (InvoicedAmount < 0)
                                            InvoicedAmount = 0;
                                        if ((objPODet.POH_TOTAL_VALUE - InvoicedAmount) > (objPODet.POH_TOTAL_VALUE - AdvInvoicedAmount))
                                        {
                                            BalanceAmount = objPODet.POH_TOTAL_VALUE.Value - AdvInvoicedAmount;
                                        }
                                        else
                                        {
                                            BalanceAmount = objPODet.POH_TOTAL_VALUE.Value - InvoicedAmount;
                                        }
                                    }
                                    else
                                    {
                                        AllocatedAdvAmount = objPODet.FIN_INVOICE_VND_ADV_DED_DTL.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.VAD_AMOUNT - c.VAD_OTHER_AMOUNT - c.VAD_TAX_AMOUNT);
                                        InvoicedAmount -= AllocatedAdvAmount;
                                        BalanceAmount = (Convert.ToDecimal(objPODet.POH_TOTAL_VALUE) - InvoicedAmount);
                                    }

                                    if (objTrxMpg.IVM_AMOUNT > BalanceAmount)
                                    {
                                        retval = (long)DbSaveStatus.AMOUNTEXCEEDS;
                                        break;
                                    }
                                }
                            }

                            if (retval == (long)DbSaveStatus.AMOUNTEXCEEDS)
                                return retval;
                            #endregion
                            

                            // Update FIN_PAYMENT_VND_HDR
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_DATE = FIN_INVOICE_VND_HDR_Obj.IVH_DATE;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_VENDOR = FIN_INVOICE_VND_HDR_Obj.IVH_VENDOR;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_NO = FIN_INVOICE_VND_HDR_Obj.IVH_NO;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_CATEGORY = FIN_INVOICE_VND_HDR_Obj.IVH_CATEGORY;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_GROUP = FIN_INVOICE_VND_HDR_Obj.IVH_GROUP;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_VENDOR_INV_NO = FIN_INVOICE_VND_HDR_Obj.IVH_VENDOR_INV_NO;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_VENDOR_ACCOUNT = FIN_INVOICE_VND_HDR_Obj.IVH_VENDOR_ACCOUNT;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_REFERENCE = FIN_INVOICE_VND_HDR_Obj.IVH_REFERENCE;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_DATE_RECEIVED = FIN_INVOICE_VND_HDR_Obj.IVH_DATE_RECEIVED;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_DATE_PAY_BY = FIN_INVOICE_VND_HDR_Obj.IVH_DATE_PAY_BY;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_CURRENCY = FIN_INVOICE_VND_HDR_Obj.IVH_CURRENCY;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_AMOUNT_TC = FIN_INVOICE_VND_HDR_Obj.IVH_AMOUNT_TC;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_DISCOUNT_TC = FIN_INVOICE_VND_HDR_Obj.IVH_DISCOUNT_TC;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_TAX_TC = FIN_INVOICE_VND_HDR_Obj.IVH_TAX_TC;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_AMOUNT_NET_TC = FIN_INVOICE_VND_HDR_Obj.IVH_AMOUNT_NET_TC;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_BASE_CURR = FIN_INVOICE_VND_HDR_Obj.IVH_BASE_CURR;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_EXCHG_RATE = FIN_INVOICE_VND_HDR_Obj.IVH_EXCHG_RATE;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_AMOUNT_NET_BC = FIN_INVOICE_VND_HDR_Obj.IVH_AMOUNT_NET_BC;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_REMARKS = FIN_INVOICE_VND_HDR_Obj.IVH_REMARKS;
                            //OldFIN_INVOICE_VND_HDR_Obj.IVH_STATUS = FIN_INVOICE_VND_HDR_Obj.IVH_STATUS;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_ACTIVE = FIN_INVOICE_VND_HDR_Obj.IVH_ACTIVE;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_MOD_BY = FIN_INVOICE_VND_HDR_Obj.IVH_MOD_BY;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_INVESTOR = FIN_INVOICE_VND_HDR_Obj.IVH_INVESTOR;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_MOD_DT = DateTime.Now;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_CATEGORY = FIN_INVOICE_VND_HDR_Obj.IVH_CATEGORY;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_TYPE = FIN_INVOICE_VND_HDR_Obj.IVH_TYPE;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_COMPANY = FIN_INVOICE_VND_HDR_Obj.IVH_COMPANY;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_VENDOR_CONTACT = FIN_INVOICE_VND_HDR_Obj.IVH_VENDOR_CONTACT;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_ORGINAL_RCVD = FIN_INVOICE_VND_HDR_Obj.IVH_ORGINAL_RCVD;

                            OldFIN_INVOICE_VND_HDR_Obj.IVH_SHIP_CHARGE = FIN_INVOICE_VND_HDR_Obj.IVH_SHIP_CHARGE;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_BRANCH_TYPE = FIN_INVOICE_VND_HDR_Obj.IVH_BRANCH_TYPE;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_TAX_ID = FIN_INVOICE_VND_HDR_Obj.IVH_TAX_ID;
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_BRANCH_TEXT = FIN_INVOICE_VND_HDR_Obj.IVH_BRANCH_TEXT;

                            OldFIN_INVOICE_VND_HDR_Obj.IVH_GST_TYPE = FIN_INVOICE_VND_HDR_Obj.IVH_GST_TYPE;

                            //Normal transaction=1,trading=2
                            OldFIN_INVOICE_VND_HDR_Obj.IVH_TRX_TYPE = 1;

                            // Sets return value as FIN_PAYMENT_VND_HDR pk
                            retval = FIN_INVOICE_VND_HDR_Obj.IVH_PK;
                            FinInvoiceVndTrxMpgManagerObj = new FinInvoiceVndTrxMpgManager(this.currentEntity);
                            //Save Shift Attendance Details
                            FinInvoiceVndTrxMpgManagerObj.SaveInvoiceTrxMpg(FIN_INVOICE_VND_TRX_MPG_LST_Obj, isWkfSave);

                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }
                }
                //return Invoice Hdr Pk
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

        /// <summary>
        /// Update Invoice Hdr Jounalize Flag
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateInvoiceHdrJounalizeFlag(int InvPK, bool JounalizeFlag)
        {
            long retval = 0;
            FIN_INVOICE_VND_HDR OldFIN_INVOICE_VND_HDR_Obj;

            try
            {
                retval = 0;
                OldFIN_INVOICE_VND_HDR_Obj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == InvPK);
                if (OldFIN_INVOICE_VND_HDR_Obj != null)
                {
                    OldFIN_INVOICE_VND_HDR_Obj.IVH_HAS_JRNL_ENTRY = JounalizeFlag;

                    retval = InvPK;
                }

                //return Invoice Hdr Pk
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

        /// <summary>
        /// Saves list of Invoice Header
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        public long SaveFinTrxDetails(int? pAppID, string pAppType)
        {
            long retval = 0;
            try
            {
                string trxNo = string.Empty;
                List<SPFIN_TRX_DUMMY_ENTRY_SAVE_Result> resultTrxNo;
                ObjectParameter paramReturn = new ObjectParameter("pRetVal", typeof(int));
                resultTrxNo = this.currentEntity.SPFIN_TRX_DUMMY_ENTRY_SAVE(pAppID, pAppType, paramReturn).ToList();
                retval = int.Parse(paramReturn.Value.ToString());
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

        /// <summary>
        /// Delete list of Invoice Header
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        public long DeleteInvoice(long invoicePK)
        {
            long retval = 0;

            try
            {
                CommonFunctionsManager ComnFnManagerObj = new CommonFunctionsManager(this.currentEntity);

                FIN_INVOICE_VND_HDR FIN_INVOICE_VND_HDR_Obj;
                List<FIN_INVOICE_VND_TRX_MPG> FIN_INVOICE_VND_TRX_MPG_LST_Obj;
                PUR_ORDER_HDR PUR_ORDER_HDR_Obj;
                FinCoaMstManager FinCoaMstManagerDrAccObj = new FinCoaMstManager(this.currentEntity);
                //IQueryable<FIN_TRX> oldFIN_TRX;

                string refType;
                long refPK;

                // Get Header Object
                FIN_INVOICE_VND_HDR_Obj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == invoicePK);

                // Get Details Object
                FIN_INVOICE_VND_TRX_MPG_LST_Obj = FIN_INVOICE_VND_HDR_Obj.FIN_INVOICE_VND_TRX_MPG.ToList();

                // Reverse Invoice amount and delete details
                foreach (FIN_INVOICE_VND_TRX_MPG FIN_INVOICE_VND_TRX_MPG_Obj in FIN_INVOICE_VND_TRX_MPG_LST_Obj)
                {
                    //PUR_ORDER_HDR_Obj = currentEntity.PUR_ORDER_HDR.SingleOrDefault(sah => sah.POH_PK == FIN_INVOICE_VND_TRX_MPG_Obj.IVM_PO_HDR);

                    //if (PUR_ORDER_HDR_Obj != null)
                    //{
                    //    PUR_ORDER_HDR_Obj.POH_AMT_INVOICED -= FIN_INVOICE_VND_TRX_MPG_Obj.IVM_AMOUNT;
                    //}

                    this.currentEntity.FIN_INVOICE_VND_TRX_MPG.DeleteObject(FIN_INVOICE_VND_TRX_MPG_Obj);
                }

                refType = "PURCHASE INVOICE";
                refPK = invoicePK;

                // Get Finance Posting details
                //oldFIN_TRX = this.currentEntity.FIN_TRX.Where(mpg => mpg.FTR_REF_TYPE.Equals(refType) && mpg.FTR_REF_PK == refPK);

                // Reverse Posting and Update account balance
                //foreach (FIN_TRX oldMpg in oldFIN_TRX)
                //{
                //    if (oldMpg.FTR_DR_AMT_BC > 0)
                //    {
                //        long finCoaVen = FinCoaMstManagerDrAccObj.FinCoaBalanceSave(oldMpg.FTR_ACCOUNT, oldMpg.FTR_DR_AMT_BC * -1);
                //    }
                //    else
                //    {
                //        long finCoaVen = FinCoaMstManagerDrAccObj.FinCoaBalanceSave(oldMpg.FTR_ACCOUNT, oldMpg.FTR_CR_AMT_BC);
                //    }

                //    this.currentEntity.FIN_TRX.DeleteObject(oldMpg);

                //}

                // Delete Header
                if (FIN_INVOICE_VND_HDR_Obj != null)
                {
                    this.currentEntity.FIN_INVOICE_VND_HDR.DeleteObject(FIN_INVOICE_VND_HDR_Obj);
                }
                else
                {
                    // throws exception already deleted or modified by other user
                    throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                }

                retval = 1;

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

        /// <summary>
        /// Perform searching,filtering,sorting,and paging on Invoice Header;
        /// </summary>
        /// <param name="AD_COUNTRIES_MSTObj" value="Contract master object with Contract master pk and active status"></param>
        /// <param name="utilityObj" value="Search criteria object"></param>
        /// <returns>List of Contract master</returns>
        public List<FIN_INVOICE_VND_HDR> GetInvoiceHdr(FIN_INVOICE_VND_HDR InvoiceHdrObj, ServiceUtility utilityObj = null,
            DateTime? PayByDateFrom = null, DateTime? PayByDateTo = null, int? Status = null, string poNo = null)
        {
            List<FIN_INVOICE_VND_HDR> InvoiceHdrList = null;
            IQueryable<FIN_INVOICE_VND_HDR> FIN_INVOICE_VND_HDRQuery;
            //int pageSize;
            //int totalCount;
            try
            {
                if (InvoiceHdrObj.IVH_PK > 0)
                {
                    FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                                join vnd in this.currentEntity.PUR_VENDOR_MST on inv.IVH_VENDOR equals vnd.VEN_PK
                                                //join trx in this.currentEntity.FIN_INVOICE_VND_TRX_MPG on inv.IVH_PK equals trx.IVM_INVOICE_HDR
                                                //join poh in this.currentEntity.PUR_ORDER_HDR on trx.IVM_PO_HDR equals poh.POH_PK
                                                where inv.IVH_ACTIVE == InvoiceHdrObj.IVH_ACTIVE
                                                  && inv.IVH_PK == InvoiceHdrObj.IVH_PK
                                                    //&& inv.IVH_VENDOR == (InvoiceHdrObj.IVH_VENDOR > 0 ? InvoiceHdrObj.IVH_VENDOR : inv.IVH_VENDOR)
                                                    //&& inv.IVH_DATE >= (utilityObj.FilterDate == null ? inv.IVH_DATE : utilityObj.FilterDate)
                                                    //&& inv.IVH_DATE <= (utilityObj.FilterToDate == null ? inv.IVH_DATE : utilityObj.FilterToDate)
                                                    //&& inv.IVH_DATE_PAY_BY >= (PayByDateFrom == null ? inv.IVH_DATE_PAY_BY : PayByDateFrom)
                                                    //&& inv.IVH_DATE_PAY_BY <= (PayByDateTo == null ? inv.IVH_DATE_PAY_BY : PayByDateTo)
                                                    //&& (Status == 0 ? inv.IVH_HAS_JRNL_ENTRY == false : (Status == 1 ? inv.IVH_HAS_JRNL_ENTRY == true : (Status == 2 ? inv.IVH_STATUS == 0 : true)))
                                                    //&& (Status == -1 ? inv.IVH_DEL_STATUS == 1 : inv.IVH_DEL_STATUS == 0)
                                                  && (InvoiceHdrObj.IVH_CATEGORY > 0 ? inv.IVH_CATEGORY == InvoiceHdrObj.IVH_CATEGORY : true)
                                                  && (InvoiceHdrObj.IVH_GROUP > 0 ? inv.IVH_GROUP == InvoiceHdrObj.IVH_GROUP : true)
                                                //&& (poNo == null ? true : inv.FIN_INVOICE_VND_TRX_MPG.Any(act => act.PUR_ORDER_HDR.POH_NO.Contains(poNo)))
                                                select inv);
                }
                else
                {
                    // Not consider any field if poNo or vendor is in filter
                    if (!string.IsNullOrWhiteSpace(poNo) || InvoiceHdrObj.IVH_VENDOR > 0)
                    {
                        FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                                    join vnd in this.currentEntity.PUR_VENDOR_MST on inv.IVH_VENDOR equals vnd.VEN_PK
                                                    where inv.FIN_INVOICE_VND_TRX_MPG.Any(act => act.PUR_ORDER_HDR.POH_NO.Contains(poNo))
                                                          && (InvoiceHdrObj.IVH_CATEGORY > 0 ? inv.IVH_CATEGORY == InvoiceHdrObj.IVH_CATEGORY : true)
                                                          && (InvoiceHdrObj.IVH_VENDOR > 0  ? (inv.IVH_VENDOR == InvoiceHdrObj.IVH_VENDOR) : true)
                                                          && (inv.IVH_BIZUNIT == (InvoiceHdrObj.IVH_BIZUNIT > 0 ? InvoiceHdrObj.IVH_BIZUNIT : inv.IVH_BIZUNIT))
                                                          && (inv.IVH_STATUS == 0 ? inv.IVH_CRTD_BY == InvoiceHdrObj.IVH_CRTD_BY : true)//Filter for drafted records only for creator
                                                    select inv);
                    }
                    else if (InvoiceHdrObj.IVH_VENDOR == 0 && InvoiceHdrObj.IVH_PK==0)
                    {
                        FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                                    join vnd in this.currentEntity.PUR_VENDOR_MST on inv.IVH_VENDOR equals vnd.VEN_PK
                                                    //join trx in this.currentEntity.FIN_INVOICE_VND_TRX_MPG on inv.IVH_PK equals trx.IVM_INVOICE_HDR
                                                    //join poh in this.currentEntity.PUR_ORDER_HDR on trx.IVM_PO_HDR equals poh.POH_PK
                                                    where inv.IVH_ACTIVE == InvoiceHdrObj.IVH_ACTIVE
                                                      && inv.IVH_DATE >= (utilityObj.FilterDate == null ? inv.IVH_DATE : utilityObj.FilterDate)
                                                      && inv.IVH_DATE <= (utilityObj.FilterToDate == null ? inv.IVH_DATE : utilityObj.FilterToDate)
                                                      && inv.IVH_DATE_PAY_BY >= (PayByDateFrom == null ? inv.IVH_DATE_PAY_BY : PayByDateFrom)
                                                      && inv.IVH_DATE_PAY_BY <= (PayByDateTo == null ? inv.IVH_DATE_PAY_BY : PayByDateTo)
                                                      && (Status == 0 ? inv.IVH_HAS_JRNL_ENTRY == false : (Status == 1 ? inv.IVH_HAS_JRNL_ENTRY == true : (Status == 2 ? inv.IVH_STATUS == 0 : true)))
                                                      && (Status == -1 ? inv.IVH_DEL_STATUS == 1 : inv.IVH_DEL_STATUS == 0)
                                                      && (InvoiceHdrObj.IVH_CATEGORY > 0 ? inv.IVH_CATEGORY == InvoiceHdrObj.IVH_CATEGORY : true)
                                                      && (InvoiceHdrObj.IVH_GROUP > 0 ? inv.IVH_GROUP == InvoiceHdrObj.IVH_GROUP : true)
                                                      && (poNo == null ? true : inv.FIN_INVOICE_VND_TRX_MPG.Any(act => act.PUR_ORDER_HDR.POH_NO.Contains(poNo)))
                                                      && (inv.IVH_BIZUNIT == (InvoiceHdrObj.IVH_BIZUNIT > 0 ? InvoiceHdrObj.IVH_BIZUNIT : inv.IVH_BIZUNIT))
                                                      && (inv.IVH_STATUS == 0 ? inv.IVH_CRTD_BY == InvoiceHdrObj.IVH_CRTD_BY : true)//Filter for drafted records only for creator
                                                      && (InvoiceHdrObj.IVH_COMPANY > 0 ? inv.IVH_COMPANY == InvoiceHdrObj.IVH_COMPANY :true)
                                                    select inv);
                    }
                    else
                    {
                        FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                                    join vnd in this.currentEntity.PUR_VENDOR_MST on inv.IVH_VENDOR equals vnd.VEN_PK
                                                    //join trx in this.currentEntity.FIN_INVOICE_VND_TRX_MPG on inv.IVH_PK equals trx.IVM_INVOICE_HDR
                                                    //join poh in this.currentEntity.PUR_ORDER_HDR on trx.IVM_PO_HDR equals poh.POH_PK
                                                    where inv.IVH_ACTIVE == InvoiceHdrObj.IVH_ACTIVE
                                                      && (InvoiceHdrObj.IVH_PK > 0 ? (inv.IVH_PK == InvoiceHdrObj.IVH_PK) : true)
                                                      && (InvoiceHdrObj.IVH_VENDOR > 0 ? (inv.IVH_VENDOR == InvoiceHdrObj.IVH_VENDOR) : true)
                                                      && inv.IVH_DATE >= (utilityObj.FilterDate == null ? inv.IVH_DATE : utilityObj.FilterDate)
                                                      && inv.IVH_DATE <= (utilityObj.FilterToDate == null ? inv.IVH_DATE : utilityObj.FilterToDate)
                                                      && inv.IVH_DATE_PAY_BY >= (PayByDateFrom == null ? inv.IVH_DATE_PAY_BY : PayByDateFrom)
                                                      && inv.IVH_DATE_PAY_BY <= (PayByDateTo == null ? inv.IVH_DATE_PAY_BY : PayByDateTo)
                                                      && (Status == 0 ? inv.IVH_HAS_JRNL_ENTRY == false : (Status == 1 ? inv.IVH_HAS_JRNL_ENTRY == true : (Status == 2 ? inv.IVH_STATUS == 0 : true)))
                                                      && (Status == -1 ? inv.IVH_DEL_STATUS == 1 : inv.IVH_DEL_STATUS == 0)
                                                      && (InvoiceHdrObj.IVH_CATEGORY > 0 ? inv.IVH_CATEGORY == InvoiceHdrObj.IVH_CATEGORY : true)
                                                      && (InvoiceHdrObj.IVH_GROUP > 0 ? inv.IVH_GROUP == InvoiceHdrObj.IVH_GROUP : true)
                                                      && (poNo == null ? true : inv.FIN_INVOICE_VND_TRX_MPG.Any(act => act.PUR_ORDER_HDR.POH_NO.Contains(poNo)))
                                                      && (inv.IVH_BIZUNIT == (InvoiceHdrObj.IVH_BIZUNIT > 0 ? InvoiceHdrObj.IVH_BIZUNIT : inv.IVH_BIZUNIT))
                                                      && (inv.IVH_STATUS == 0 ? inv.IVH_CRTD_BY == InvoiceHdrObj.IVH_CRTD_BY : true)//Filter for drafted records only for creator
                                                    select inv);
                    }
                }

                //FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                //                            join vnd in this.currentEntity.PUR_VENDOR_MST on inv.IVH_VENDOR equals vnd.VEN_PK
                //                            join trx in this.currentEntity.FIN_INVOICE_VND_TRX_MPG on inv.IVH_PK equals trx.IVM_INVOICE_HDR
                //                            join poh in this.currentEntity.PUR_ORDER_HDR on trx.IVM_PO_HDR equals poh.POH_PK
                //                            where inv.IVH_ACTIVE == InvoiceHdrObj.IVH_ACTIVE
                //                              && inv.IVH_PK == (InvoiceHdrObj.IVH_PK > 0 ? InvoiceHdrObj.IVH_PK : inv.IVH_PK)
                //                              && inv.IVH_VENDOR == (InvoiceHdrObj.IVH_VENDOR > 0 ? InvoiceHdrObj.IVH_VENDOR : inv.IVH_VENDOR)
                //                              && inv.IVH_DATE >= (utilityObj.FilterDate == null ? inv.IVH_DATE : utilityObj.FilterDate)
                //                              && inv.IVH_DATE <= (utilityObj.FilterToDate == null ? inv.IVH_DATE : utilityObj.FilterToDate)
                //                              && inv.IVH_DATE_PAY_BY >= (PayByDateFrom == null ? inv.IVH_DATE_PAY_BY : PayByDateFrom)
                //                              && inv.IVH_DATE_PAY_BY <= (PayByDateTo == null ? inv.IVH_DATE_PAY_BY : PayByDateTo)
                //                              && (Status == 0 ? inv.IVH_HAS_JRNL_ENTRY == false : (Status == 1 ? inv.IVH_HAS_JRNL_ENTRY == true : (Status == 2 ? inv.IVH_STATUS == 0 : true)))
                //                              && (Status == -1 ? inv.IVH_DEL_STATUS == 1 : inv.IVH_DEL_STATUS == 0)
                //                              && (InvoiceHdrObj.IVH_CATEGORY > 0 ? inv.IVH_CATEGORY == InvoiceHdrObj.IVH_CATEGORY : true)
                //                              && (InvoiceHdrObj.IVH_GROUP > 0 ? inv.IVH_GROUP == InvoiceHdrObj.IVH_GROUP : true)
                //                           && (poNo == null ? poh.POH_NO.Contains(poh.POH_NO) : poh.POH_NO.Contains(poNo))
                //                            select inv);

                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = FIN_INVOICE_VND_HDRQuery.GroupBy(f=> f.IVH_PK).Count();

                //Filter Query
                //paymentHdrQuery = FilterEntity(finPaymentVndHdrListObj, paymentHdrQuery, serviceUtilityObj);

                // Apply Paging And Sorting for grid Purpose

                //return Country master details;
                if (utilityObj.SortBy != null && utilityObj.SortDirection != null)
                {
                    if (utilityObj.SortBy == DataFieldRes.VendorName)
                        utilityObj.SortBy = DataTableRes.VendorMst + "." + DataFieldRes.VendorName;
                }
                InvoiceHdrList = FIN_INVOICE_VND_HDRQuery.SortRecords<FIN_INVOICE_VND_HDR>(utilityObj).ToList();

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

        public bool IsExistVendorInvoice(string vendorInvoiveNo, int vendorID, long invoicePK)
        {
            bool result = false;
            List<FIN_INVOICE_VND_HDR> InvoiceHdrList = null;
            IQueryable<FIN_INVOICE_VND_HDR> FIN_INVOICE_VND_HDRQuery;
            try
            {
                FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                            where inv.IVH_VENDOR_INV_NO == vendorInvoiveNo
                                                && inv.IVH_VENDOR == vendorID
                                                && inv.IVH_DEL_STATUS == 0
                                                && inv.IVH_PK != invoicePK
                                            select inv);

                InvoiceHdrList = FIN_INVOICE_VND_HDRQuery.ToList();

                return InvoiceHdrList.Count > 0 ? true : false;
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            return result;
        }

        public List<FIN_INVOICE_VND_HDR> GetInvoiceHdrByPK(FIN_INVOICE_VND_HDR InvoiceHdrObj)
        {
            List<FIN_INVOICE_VND_HDR> InvoiceHdrList = null;
            IQueryable<FIN_INVOICE_VND_HDR> FIN_INVOICE_VND_HDRQuery;
            //int pageSize;
            //int totalCount;
            try
            {

                FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                            where inv.IVH_ACTIVE == InvoiceHdrObj.IVH_ACTIVE
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



        public List<FIN_INVOICE_VND_HDR> GetInvoiceNumberAutoCompleteList(FIN_INVOICE_VND_HDR objPoHeader, ServiceUtility utilityObj)
        {
            List<FIN_INVOICE_VND_HDR> FIN_INVOICE_VND_HDRListObj = new List<FIN_INVOICE_VND_HDR>();
            try
            {
                FIN_INVOICE_VND_HDRListObj = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                              where (inv.IVH_NO.Contains(utilityObj.FilterValue)
                                              && (objPoHeader.IVH_STATUS > 0 ? inv.IVH_STATUS == objPoHeader.IVH_STATUS : true)
                                              && (objPoHeader.IVH_CATEGORY > 0 ? inv.IVH_CATEGORY == objPoHeader.IVH_CATEGORY : true)
                                              && (objPoHeader.IVH_GROUP > 0 ? inv.IVH_CATEGORY == objPoHeader.IVH_GROUP : true)
                                              && (!string.IsNullOrEmpty(inv.IVH_NO))
                                              &&
                                              (inv.IVH_BIZUNIT == (objPoHeader.IVH_BIZUNIT > 0 ? objPoHeader.IVH_BIZUNIT : inv.IVH_BIZUNIT)))
                                              select inv).Take(utilityObj.PageSize).ToList();
            }
            catch
            {
            }
            return FIN_INVOICE_VND_HDRListObj;
        }

        public List<FIN_INVOICE_VND_HDR> GetInvNumberAutoCompleteList(FIN_INVOICE_VND_HDR objPoHeader, ServiceUtility utilityObj)
        {
            List<FIN_INVOICE_VND_HDR> FIN_INVOICE_VND_HDRListObj = new List<FIN_INVOICE_VND_HDR>();

            List<int> Group = new List<int>();
            Group.Add(Convert.ToInt16(objPoHeader.IVH_GROUP));
            Group.Add(Convert.ToInt16(POInvoiceGroup.AgtInvoice));

            try
            {
                FIN_INVOICE_VND_HDRListObj = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                              where (inv.IVH_NO.Contains(utilityObj.FilterValue)
                                              && (objPoHeader.IVH_STATUS > 0 ? inv.IVH_STATUS == objPoHeader.IVH_STATUS : true)
                                              && (objPoHeader.IVH_CATEGORY > 0 ? inv.IVH_CATEGORY == objPoHeader.IVH_CATEGORY : true)
                                              && (objPoHeader.IVH_GROUP > 0 ? objPoHeader.IVH_GROUP == 3 ? Group.Contains(inv.IVH_GROUP) : inv.IVH_GROUP == objPoHeader.IVH_GROUP : true)
                                              && (!string.IsNullOrEmpty(inv.IVH_NO))
                                              && (inv.IVH_BIZUNIT == (objPoHeader.IVH_BIZUNIT > 0 ? objPoHeader.IVH_BIZUNIT : inv.IVH_BIZUNIT)))
                                              select inv).ToList();
            }
            catch
            {
            }
            return FIN_INVOICE_VND_HDRListObj;
        }

        public List<FIN_INVOICE_VND_HDR> GetConvertedInvNumbersAutoCompleteList(FIN_INVOICE_VND_HDR objPoHeader, ServiceUtility utilityObj)
        {
            List<FIN_INVOICE_VND_HDR> FIN_INVOICE_VND_HDRListObj = new List<FIN_INVOICE_VND_HDR>();

            List<int> Group = new List<int>();
            Group.Add(Convert.ToInt16(objPoHeader.IVH_GROUP));
            Group.Add(Convert.ToInt16(POInvoiceGroup.AgtInvoice));

            try
            {
                FIN_INVOICE_VND_HDRListObj = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                              where (inv.IVH_NO.Contains(utilityObj.FilterValue)
                                              && (objPoHeader.IVH_STATUS > 0 ? inv.IVH_STATUS == objPoHeader.IVH_STATUS : true)
                                              && (objPoHeader.IVH_CATEGORY > 0 ? inv.IVH_CATEGORY == objPoHeader.IVH_CATEGORY : true)
                                              && (objPoHeader.IVH_GROUP > 0 ? objPoHeader.IVH_GROUP == 3 ? Group.Contains(inv.IVH_GROUP) : inv.IVH_GROUP == objPoHeader.IVH_GROUP : true)
                                              && (!string.IsNullOrEmpty(inv.IVH_NO))
                                              //&& (inv.IVH_BIZUNIT == (objPoHeader.IVH_BIZUNIT > 0 ? objPoHeader.IVH_BIZUNIT : inv.IVH_BIZUNIT))
                                              && (inv.IVH_CONVERT == 1)
                                              )
                                              select inv).ToList();
            }
            catch
            {
            }
            return FIN_INVOICE_VND_HDRListObj;
        }

        //public List<FIN_INVOICE_VND_HDR> GetInvoiceHdrDetails(long invoicePK)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region Private Methods
        #endregion

        public long? SaveDocAttachemts(List<ADM_DOC_ATTACH> DocAttachList, int DocTaskId)
        {
            long? retval = 0;
            int? maxID;
            List<ADM_DOC_ATTACH> Old_DocList;
            ADM_DOC_ATTACH OldADM_DOC_ATTACH_Obj;
            try
            {


                if (DocAttachList.Count > 0)
                {
                    maxID = currentEntity.ADM_DOC_ATTACH.Max(v => (int?)v.DOC_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    List<int> pks = (from old1 in DocAttachList
                                     select old1.DOC_PK).ToList();
                    //long? DOC_TASK_ID = DocAttachList[0].DOC_TASK_ID;

                    /**/
                    Old_DocList = (from oldp in this.currentEntity.ADM_DOC_ATTACH
                                   where oldp.DOC_TASK_ID == DocTaskId
                                   && !pks.Contains(oldp.DOC_PK)
                                   select oldp).ToList();

                    foreach (ADM_DOC_ATTACH old_ADM_DOC_ATTACH in Old_DocList)
                    {
                        this.currentEntity.ADM_DOC_ATTACH.DeleteObject(old_ADM_DOC_ATTACH);
                    }

                    foreach (ADM_DOC_ATTACH ADM_DOC_ATTACH_Obj in DocAttachList)
                    {
                        if (ADM_DOC_ATTACH_Obj.DOC_PK == -1)
                            break;
                        if (ADM_DOC_ATTACH_Obj.DOC_PK == 0) //  INSERT NEW RECORD
                        {
                            ADM_DOC_ATTACH_Obj.DOC_PK = maxID.Value;
                            ADM_DOC_ATTACH_Obj.DOC_TASK_ID = DocTaskId;
                            currentEntity.ADM_DOC_ATTACH.AddObject(ADM_DOC_ATTACH_Obj);
                            maxID++;
                            retval = ADM_DOC_ATTACH_Obj.DOC_PK;
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            // updating ADM_DOC_ATTACH
                            // Get current ADM_DOC_ATTACH using ADM_DOC_ATTACH pk and last modified date time,used for concurrency checking
                            OldADM_DOC_ATTACH_Obj = currentEntity.ADM_DOC_ATTACH.SingleOrDefault(sah => sah.DOC_PK == ADM_DOC_ATTACH_Obj.DOC_PK);

                            // If oldAdmDocAttachObj is null then,anyone modified or deleted the record
                            if (OldADM_DOC_ATTACH_Obj != null)
                            {
                                // Update ADM_DOC_ATTACH
                                OldADM_DOC_ATTACH_Obj.DOC_ACTIVE = ADM_DOC_ATTACH_Obj.DOC_ACTIVE;
                                OldADM_DOC_ATTACH_Obj.DOC_BIZUNIT = ADM_DOC_ATTACH_Obj.DOC_BIZUNIT;
                                //OldADM_DOC_ATTACH_Obj.DOC_CRTD_BY = ADM_DOC_ATTACH_Obj.DOC_CRTD_BY;
                                OldADM_DOC_ATTACH_Obj.DOC_DESC = ADM_DOC_ATTACH_Obj.DOC_DESC;
                                OldADM_DOC_ATTACH_Obj.DOC_MOD_BY = ADM_DOC_ATTACH_Obj.DOC_MOD_BY;
                                OldADM_DOC_ATTACH_Obj.DOC_MOD_DT = ADM_DOC_ATTACH_Obj.DOC_MOD_DT;
                                OldADM_DOC_ATTACH_Obj.DOC_MODULE = ADM_DOC_ATTACH_Obj.DOC_MODULE;
                                OldADM_DOC_ATTACH_Obj.DOC_NAME = ADM_DOC_ATTACH_Obj.DOC_NAME;
                                OldADM_DOC_ATTACH_Obj.DOC_NAME = ADM_DOC_ATTACH_Obj.DOC_NAME;
                                OldADM_DOC_ATTACH_Obj.DOC_PATH = ADM_DOC_ATTACH_Obj.DOC_PATH;
                                OldADM_DOC_ATTACH_Obj.DOC_SEQ_NO = ADM_DOC_ATTACH_Obj.DOC_SEQ_NO;
                                OldADM_DOC_ATTACH_Obj.DOC_TASK = ADM_DOC_ATTACH_Obj.DOC_TASK;
                                //OldADM_DOC_ATTACH_Obj.DOC_TASK_ID = ADM_DOC_ATTACH_Obj.DOC_TASK_ID;
                                OldADM_DOC_ATTACH_Obj.DOC_TITLE = ADM_DOC_ATTACH_Obj.DOC_TITLE;
                                OldADM_DOC_ATTACH_Obj.DOC_TYPE = ADM_DOC_ATTACH_Obj.DOC_TYPE;

                                // Sets return value as ADM_DOC_ATTACH pk
                                retval = ADM_DOC_ATTACH_Obj.DOC_PK;
                            }
                        }
                    }

                }
                return retval;

            } //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }


        }

        public List<ADM_DOC_ATTACH> GetDocAttachments(ADM_DOC_ATTACH admDocAttachObj, ServiceUtility serviceUtilityObj)
        {
            List<ADM_DOC_ATTACH> DocList = null;
            IQueryable<ADM_DOC_ATTACH> ADM_DOC_ATTACHQuery;
            //int pageSize;
            //int totalCount;
            try
            {

                ADM_DOC_ATTACHQuery = (from inv in this.currentEntity.ADM_DOC_ATTACH
                                       where inv.DOC_TASK_ID == admDocAttachObj.DOC_TASK_ID && inv.DOC_TASK == admDocAttachObj.DOC_TASK
                                       select inv);
                DocList = ADM_DOC_ATTACHQuery.ToList();
                return DocList;
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

        /// <summary>
        /// Get Invoice Received Amount Details
        /// </summary>
        /// <param name="InvoicePk"></param>
        /// <returns></returns>
        public DataTable GetInvVndReceivedAmntDetails(long InvoicePk)
        {
            DataTable dtResult = null;
            try
            {

                var Receipt = from rcpt in this.currentEntity.FIN_PAYMENT_VND_TRX_MPG
                              where rcpt.PVM_INVOICE_HDR == InvoicePk && rcpt.FIN_PAYMENT_VND_HDR.PVH_STATUS != 0 && rcpt.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 && rcpt.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0
                              select new
                              {
                                  TRX_NO = rcpt.FIN_PAYMENT_VND_HDR.PVH_NO,
                                  DATE = rcpt.FIN_PAYMENT_VND_HDR.PVH_DATE,
                                  AMOUNT = rcpt.PVM_PAID_AMOUNT,
                                  PAYMENT = ""
                              };
                //var DebitCredit = from drcr in this.currentEntity.FIN_CRDR_NOTE_MPG
                //                  where drcr.CDM_INVOICE_VND_HDR == InvoicePk && drcr.FIN_CRDR_NOTE_HDR.CDH_TYPE == (byte)DebitCreditModeEnum.DEBIT
                //                   && drcr.FIN_CRDR_NOTE_HDR.CDH_STATUS != 0 && drcr.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED == false
                //                  select new
                //                  {
                //                      TRX_NO = drcr.FIN_CRDR_NOTE_HDR.CDH_NO,
                //                      DATE = drcr.FIN_CRDR_NOTE_HDR.CDH_DATE,
                //                      AMOUNT = drcr.CDM_AMOUNT,
                //                      PAYMENT = ""
                //                  };

                var DebitCredit = from drcr in this.currentEntity.FIN_PAYMENT_VND_ALCN_DTL
                                  where drcr.PAD_ALCN_CDH != null && drcr.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePk && drcr.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_STATUS != 0
                                  && drcr.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                  select new
                                  {
                                      TRX_NO = drcr.FIN_CRDR_NOTE_HDR.CDH_NO,
                                      DATE = drcr.FIN_CRDR_NOTE_HDR.CDH_DATE,
                                      AMOUNT = drcr.PAD_AMOUNT,
                                      PAYMENT = drcr.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_NO
                                  };

                var AdvDeduct = from adv in this.currentEntity.FIN_INVOICE_VND_ADV_DED_DTL
                                where adv.VAD_INVOICE_HDR == InvoicePk
                                select new
                                {
                                    TRX_NO = adv.FIN_INVOICE_VND_HDR.IVH_NO,
                                    DATE = adv.FIN_INVOICE_VND_HDR.IVH_DATE,
                                    AMOUNT = adv.VAD_AMOUNT,
                                    PAYMENT = adv.FIN_PAYMENT_VND_HDR.PVH_NO
                                };
                //AMOUNT = adv.VAD_AMOUNT - (adv.VAD_ADJUST_AMOUNT + adv.VAD_OTHER_AMOUNT + adv.VAD_TAX_AMOUNT),
                var Result = Receipt.Union(DebitCredit).Union(AdvDeduct);
                Result = from amt in Result where amt.AMOUNT > 0 select amt;
                dtResult = Result.ToList().ToDataTable();
                return dtResult;
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


        public int AttachDocumentDelete(int? docPk, int? docTask, int? docTaskId)
        {
            var retVal = new ObjectParameter(DataFieldRes.P_RET_VAL, typeof(int));
            var result = this.currentEntity.SPADM_DOC_ATTACH_DELETE(docPk, docTask, docTaskId, retVal); 
            return Convert.ToInt32(retVal.Value);
        }
    }
}
