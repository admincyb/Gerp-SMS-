using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data.Objects;
using System.Data;
using ERPManager.POInvoicing;
using BusinessObject.CommonManagement;

namespace ERPManager
{
    public class FinPaymentVndHdrManager : IFinPaymentVndHdrManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion
        #region Manager Methods
        /// <summary>
        /// Payment Header Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public FinPaymentVndHdrManager(ERPEntities currentEntity)
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
        /// Get Payment Header List
        /// </summary>
        /// <param name="finPaymentHdrObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_PAYMENT_VND_HDR> GetPaymentHdr(FIN_PAYMENT_VND_HDR finPaymentHdrObj, ServiceUtility serviceUtilityObj, int? Status = null, string invNo = null, int? PDCStatus=0)
        {
            IQueryable<FIN_PAYMENT_VND_HDR> paymentHdrQuery;
            List<FIN_PAYMENT_VND_HDR> finPaymentVndHdrListObj;

            int pageSize;

            try
            {
                pageSize = Convert.ToInt32(serviceUtilityObj.PageSize);
                //Selecting
                if (finPaymentHdrObj.PVH_PK > 0)
                {
                    paymentHdrQuery
                                = (from pvh in this.currentEntity.FIN_PAYMENT_VND_HDR
                                   join vnd in this.currentEntity.PUR_VENDOR_MST on pvh.PVH_VENDOR equals vnd.VEN_PK
                                   //join cbm in this.currentEntity.FIN_CASH_BANK_MST on pvh.PVH_BANK equals cbm.CBM_PK
                                   //join trx in this.currentEntity.FIN_PAYMENT_VND_TRX_MPG on pvh.PVH_PK equals trx.PVM_PAYMENT_HDR
                                   //join ivh in this.currentEntity.FIN_INVOICE_VND_HDR on trx.PVM_INVOICE_HDR equals ivh.IVH_PK
                                   where pvh.PVH_ACTIVE == finPaymentHdrObj.PVH_ACTIVE
                                    //&& pvh.PVH_DATE >= (serviceUtilityObj.FilterDate == DateTime.MinValue ? pvh.PVH_DATE : serviceUtilityObj.FilterDate)
                                    //&& pvh.PVH_DATE <= (serviceUtilityObj.FilterToDate == DateTime.MinValue ? pvh.PVH_DATE : serviceUtilityObj.FilterToDate)
                                    //&& (Status == 0 ? pvh.PVH_HAS_JRNL_ENTRY == false : (Status == 1 ? pvh.PVH_HAS_JRNL_ENTRY == true : (Status == 2 ? pvh.PVH_STATUS != 2 : (Status == 4 ? pvh.PVH_PDC == 1 : true))))
                                    //&& (Status == -1 ? pvh.PVH_DEL_STATUS == 1 : pvh.PVH_DEL_STATUS == 0)
                                    //&& (PDCStatus == 0 || PDCStatus == null ? true : pvh.PVH_PDC == (byte)PDCStatus)
                                    && (finPaymentHdrObj.PVH_CATEGORY > 0 ? pvh.PVH_CATEGORY == finPaymentHdrObj.PVH_CATEGORY : true)
                                    //&& (invNo == null ? true : pvh.FIN_PAYMENT_VND_TRX_MPG.Any(act => act.FIN_INVOICE_VND_HDR.IVH_NO.Contains(invNo)))
                                   orderby pvh.PVH_PK descending
                                   select pvh
                                  );
                }
                else
                {
                    paymentHdrQuery
                                = (from pvh in this.currentEntity.FIN_PAYMENT_VND_HDR
                                   join vnd in this.currentEntity.PUR_VENDOR_MST on pvh.PVH_VENDOR equals vnd.VEN_PK
                                   //join cbm in this.currentEntity.FIN_CASH_BANK_MST on pvh.PVH_BANK equals cbm.CBM_PK
                                   //join trx in this.currentEntity.FIN_PAYMENT_VND_TRX_MPG on pvh.PVH_PK equals trx.PVM_PAYMENT_HDR
                                   //join ivh in this.currentEntity.FIN_INVOICE_VND_HDR on trx.PVM_INVOICE_HDR equals ivh.IVH_PK
                                   where pvh.PVH_ACTIVE == finPaymentHdrObj.PVH_ACTIVE
                                    && pvh.PVH_DATE >= (serviceUtilityObj.FilterDate == DateTime.MinValue ? pvh.PVH_DATE : serviceUtilityObj.FilterDate)
                                    && pvh.PVH_DATE <= (serviceUtilityObj.FilterToDate == DateTime.MinValue ? pvh.PVH_DATE : serviceUtilityObj.FilterToDate)
                                    && (Status == 0 ? pvh.PVH_HAS_JRNL_ENTRY == false : (Status == 1 ? pvh.PVH_HAS_JRNL_ENTRY == true : (Status == 2 ? pvh.PVH_STATUS != 2 : (Status == 4 ? pvh.PVH_PDC == 1 : true))))
                                    && (Status == -1 ? pvh.PVH_DEL_STATUS == 1 : pvh.PVH_DEL_STATUS == 0)
                                    && (PDCStatus == 0 || PDCStatus == null ? true : pvh.PVH_PDC == (byte)PDCStatus)
                                    && (finPaymentHdrObj.PVH_CATEGORY > 0 ? pvh.PVH_CATEGORY == finPaymentHdrObj.PVH_CATEGORY : true)
                                    && (invNo == null ? true : pvh.FIN_PAYMENT_VND_TRX_MPG.Any(act => act.FIN_INVOICE_VND_HDR.IVH_NO.Contains(invNo)))
                                    && (pvh.PVH_BIZUNIT == (finPaymentHdrObj.PVH_BIZUNIT > 0 ? finPaymentHdrObj.PVH_BIZUNIT : pvh.PVH_BIZUNIT))
                                    && (pvh.PVH_STATUS == 0 ? pvh.PVH_CRTD_BY == finPaymentHdrObj.PVH_CRTD_BY : true)
                                   orderby pvh.PVH_PK descending
                                   select pvh
                                  );
                }

                //paymentHdrQuery
                //            = (from pvh in this.currentEntity.FIN_PAYMENT_VND_HDR
                //               join vnd in this.currentEntity.PUR_VENDOR_MST on pvh.PVH_VENDOR equals vnd.VEN_PK
                //               join cbm in this.currentEntity.FIN_CASH_BANK_MST on pvh.PVH_BANK equals cbm.CBM_PK
                //               join trx in this.currentEntity.FIN_PAYMENT_VND_TRX_MPG on pvh.PVH_PK equals trx.PVM_PAYMENT_HDR
                //               join ivh in this.currentEntity.FIN_INVOICE_VND_HDR on trx.PVM_INVOICE_HDR equals ivh.IVH_PK
                //               where pvh.PVH_ACTIVE == finPaymentHdrObj.PVH_ACTIVE
                //                && pvh.PVH_DATE >= (serviceUtilityObj.FilterDate == DateTime.MinValue ? pvh.PVH_DATE : serviceUtilityObj.FilterDate)
                //                && pvh.PVH_DATE <= (serviceUtilityObj.FilterToDate == DateTime.MinValue ? pvh.PVH_DATE : serviceUtilityObj.FilterToDate)
                //                && (Status == 0 ? pvh.PVH_HAS_JRNL_ENTRY == false : (Status == 1 ? pvh.PVH_HAS_JRNL_ENTRY == true : (Status == 2 ? pvh.PVH_STATUS != 2 :(Status == 4 ? pvh.PVH_PDC==1 : true))))
                //                && (Status == -1 ? pvh.PVH_DEL_STATUS == 1 : pvh.PVH_DEL_STATUS == 0)
                //                && (PDCStatus == 0 || PDCStatus == null ? true : pvh.PVH_PDC == (byte)PDCStatus)
                //                && (finPaymentHdrObj.PVH_CATEGORY > 0 ? pvh.PVH_CATEGORY == finPaymentHdrObj.PVH_CATEGORY : true)
                //              && (invNo == null ? ivh.IVH_NO.Contains(ivh.IVH_NO) : ivh.IVH_NO.Contains(invNo))
                //               orderby pvh.PVH_PK descending
                //               select pvh
                //              );


                //Set page size one if not given
                serviceUtilityObj.PageSize = serviceUtilityObj.PageSize == 0 ? 1 : serviceUtilityObj.PageSize;

                //Filter Query

                paymentHdrQuery = FilterEntity(finPaymentHdrObj, paymentHdrQuery, serviceUtilityObj);

                #region Sorting
                // Apply Paging And Sorting For AD_TAX_GROUPS_MST grid Purpose
                // Checking sorting criteria is given

                #endregion
                //return Tax master details;
                finPaymentVndHdrListObj = paymentHdrQuery.SortRecords<FIN_PAYMENT_VND_HDR>(serviceUtilityObj).ToList();

                //Get total row count
                serviceUtilityObj.TotalRecords = paymentHdrQuery.GroupBy(p=>p.PVH_PK).Count();

                return finPaymentVndHdrListObj;
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
                finPaymentVndHdrListObj = null;
            }

            //return finPaymentVndHdrListObj;
        }

        /// <summary>
        /// Get Payment Header Details
        /// </summary>
        /// <param name="paymentPK"></param>
        /// <returns></returns>
        public List<FIN_PAYMENT_VND_HDR> GetPaymentHdr(long paymentPK)
        {

            List<FIN_PAYMENT_VND_HDR> FinPaymentHdrListObj = new List<FIN_PAYMENT_VND_HDR>();
            try
            {
                FinPaymentHdrListObj = (from pvh in this.currentEntity.FIN_PAYMENT_VND_HDR
                                        join vnd in this.currentEntity.PUR_VENDOR_MST on pvh.PVH_VENDOR equals vnd.VEN_PK
                                        //join cbm in this.currentEntity.FIN_CASH_BANK_MST on pvh.PVH_BANK equals cbm.CBM_PK
                                        where pvh.PVH_PK == paymentPK
                                        select pvh
                              ).ToList();
                return FinPaymentHdrListObj;
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
                FinPaymentHdrListObj = null;
            }
        }
        /// <summary>
        /// Save Payment Header and Details
        /// </summary>
        /// <param name="finPaymentHdrList"></param>
        /// <returns></returns>
        public long? SavePaymentHdr(List<FIN_PAYMENT_VND_HDR> finPaymentVndHdrList)
        {
            //Holds save status
            long retval;
            FIN_PAYMENT_VND_HDR oldPaymentHdrObj;
            List<FIN_PAYMENT_VND_TRX_MPG> finPaymentVndTrxMpgList;
            FinPaymentVndTrxMpgManager FinPaymentVndTrxMpgManagerObj;

            //fin trx objects
            //List<FIN_TRX> finTrxList;
            //FIN_TRX finTrxObj;
            //FIN_TRX finTrxObj2;
            FinTrxManager FinTrxManagerobj;
            FinYearMstManager FinYearMstManagerObj = new FinYearMstManager(this.currentEntity);
            List<FIN_PAYMENT_VND_TAX_HDR> finPaymentVndTaxHdrList;
            List<FIN_PAYMENT_VND_MODE_DTL> finPaymentModeDtlList;
            FinPaymentVndTaxHdrmanager finPaymentVndTaxHdrmanagerObj;
            int? maxPaymentPk;
            try
            {
                //Set save status zero,save failed
                retval = 0;
                //Iterate through shift attendance list for save
                foreach (FIN_PAYMENT_VND_HDR finPaymentVndHdrObj in finPaymentVndHdrList)
                {
                    finPaymentVndTrxMpgList = finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG.ToList();
                    finPaymentVndTaxHdrList = finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.ToList();
                    finPaymentModeDtlList = finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL.ToList();
                    finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG.Clear();
                    finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Clear();
                    if (finPaymentVndHdrObj.PVH_PK == 0)
                    {

                        #region Amount checking to avoid excess payment.
                        foreach (FIN_PAYMENT_VND_TRX_MPG objTrxMpg in finPaymentVndTrxMpgList)
                        {
                            decimal balToPay = 0;                           
                            FIN_INVOICE_VND_HDR objInvHdr = this.currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(r => r.IVH_PK == objTrxMpg.PVM_INVOICE_HDR);
                            if (objInvHdr != null)
                            {
                                balToPay = objInvHdr.IVH_AMOUNT_NET_TC -
                                           objInvHdr.IVH_AMOUNT_PAID_TC +
                                           objInvHdr.IVH_AMOUNT_CN_TC -
                                           objInvHdr.IVH_AMOUNT_DN_TC;
                                if ((objTrxMpg.PVM_PAID_AMOUNT + objTrxMpg.PVM_ADJUST_AMOUNT) > balToPay)
                                {
                                    retval = (long)DbSaveStatus.AMOUNTEXCEEDS;
                                    break;
                                }
                            }                           
                        }
                        if (retval == (long)DbSaveStatus.AMOUNTEXCEEDS)
                            break; 
                        #endregion


                        //check for whether a vendor for the same shift and date already saved or not
                        // Gets last FIN_PAYMENT_VND_HDR pk
                        maxPaymentPk = this.currentEntity.FIN_PAYMENT_VND_HDR.Max(pvh => (int?)pvh.PVH_PK);

                        // Sets return value as next FIN_PAYMENT_VND_HDR pk
                        retval = Convert.ToInt16((maxPaymentPk.HasValue ? maxPaymentPk.Value + 1 : 1));

                        // Sets next FIN_PAYMENT_VND_HDR pk
                        finPaymentVndHdrObj.PVH_PK = retval;

                        // Sets FIN_PAYMENT_VND_HDR created date time as current date time
                        finPaymentVndHdrObj.PVH_CRTD_DT = DateTime.Now;

                        // Sets FIN_PAYMENT_VND_HDR modified date time as current date time
                        finPaymentVndHdrObj.PVH_MOD_DT = DateTime.Now;

                        //Normal transaction=1,trading=2
                        finPaymentVndHdrObj.PVH_TRX_TYPE = 1;

                        //Here we removw the dummyobject created for WHTTax.This for avoiding a default entry to that table
                        FIN_PAYMENT_VND_TAX_HDR DummyObj= finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.FirstOrDefault();
                        if (DummyObj!=null && DummyObj.WTH_PK == -1)
                        {
                            finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.Remove(DummyObj); 
                        }

                        // Add new FIN_PAYMENT_VND_HDR to the db context
                        this.currentEntity.FIN_PAYMENT_VND_HDR.AddObject(finPaymentVndHdrObj);
                        //set the FIN_PAYMENT_VND_HDR pk as the fk of  CAM_SHIFT_ATTENDANCE_Dtl
                        finPaymentVndTrxMpgList.ForEach(dtl => dtl.PVM_PAYMENT_HDR = retval);
                        FinPaymentVndTrxMpgManagerObj = new FinPaymentVndTrxMpgManager(this.currentEntity);
                        //Save Shift Attendance Details
                        FinPaymentVndTrxMpgManagerObj.SavePaymentDtl(finPaymentVndTrxMpgList, finPaymentVndHdrObj.PVH_CATEGORY);
                        finPaymentVndTaxHdrList = finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR == null ?
                            new List<FIN_PAYMENT_VND_TAX_HDR>() : finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.ToList();
                        finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.Clear();
                        finPaymentVndTaxHdrList.ForEach(dtl => dtl.WTH_PAYMENT_HDR = retval);
                        if (finPaymentVndTaxHdrList.Count > 0)
                        {
                            finPaymentVndTaxHdrmanagerObj = new FinPaymentVndTaxHdrmanager(this.currentEntity);
                            finPaymentVndTaxHdrmanagerObj.SaveWHTTax(finPaymentVndTaxHdrList);
                        }

                        //Save payment mode details
                        finPaymentModeDtlList = finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL == null ?
                           new List<FIN_PAYMENT_VND_MODE_DTL>() : finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL.ToList();
                        finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL.Clear();
                        finPaymentModeDtlList.ForEach(dtl => dtl.PDM_PAYMENT_HDR = retval);
                        if (finPaymentModeDtlList.Count > 0)
                        {
                            FinPaymentVndTrxMpgManagerObj.SavePaymentModeDetails(finPaymentModeDtlList);
                        }
                    }
                    else
                    {
                        // updating FIN_PAYMENT_VND_HDR

                        // Get current FIN_PAYMENT_VND_HDR using FIN_PAYMENT_VND_HDR pk and last modified date time,used for concurrency checking
                        oldPaymentHdrObj = currentEntity.FIN_PAYMENT_VND_HDR.SingleOrDefault(sah => sah.PVH_PK == finPaymentVndHdrObj.PVH_PK && sah.PVH_MOD_DT == finPaymentVndHdrObj.PVH_MOD_DT);
                        // If oldPaymentHdrObj is null then,anyone modified or deleted the record

                        if (oldPaymentHdrObj != null)
                        {
                            #region Amount checking to avoid excess payment.
                            //foreach (FIN_PAYMENT_VND_TRX_MPG objTrxMpg in finPaymentVndTrxMpgList)
                            //{
                            //    decimal balToPay = 0;
                            //    FIN_PAYMENT_VND_TRX_MPG objOldTrxMpg = this.currentEntity.FIN_PAYMENT_VND_TRX_MPG.Where(r => r.PVM_INVOICE_HDR == objTrxMpg.PVM_INVOICE_HDR && r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0).FirstOrDefault();
                            //    balToPay = (objOldTrxMpg.FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC) -
                            //                 (objOldTrxMpg.FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC + objOldTrxMpg.FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC) +
                            //                 (objOldTrxMpg.PVM_BOUNCED == 1 ? 0 : objOldTrxMpg.PVM_PAID_AMOUNT) +
                            //                 objOldTrxMpg.FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC;
                            //    if ((objTrxMpg.PVM_PAID_AMOUNT + objTrxMpg.PVM_ADJUST_AMOUNT) > balToPay)
                            //    {
                            //        retval = (long)DbSaveStatus.AMOUNTEXCEEDS;
                            //        break;
                            //    }
                            //}
                            //if (retval == (long)DbSaveStatus.AMOUNTEXCEEDS)
                            //    break;
                            #endregion

                            // Update FIN_PAYMENT_VND_HDR
                            oldPaymentHdrObj.PVH_NO = finPaymentVndHdrObj.PVH_NO;
                            oldPaymentHdrObj.PVH_CATEGORY = finPaymentVndHdrObj.PVH_CATEGORY;
                            oldPaymentHdrObj.PVH_GROUP = finPaymentVndHdrObj.PVH_GROUP;
                            oldPaymentHdrObj.PVH_DATE = finPaymentVndHdrObj.PVH_DATE;
                            oldPaymentHdrObj.PVH_VENDOR = finPaymentVndHdrObj.PVH_VENDOR;
                            oldPaymentHdrObj.PVH_VENDOR_ACCOUNT = finPaymentVndHdrObj.PVH_VENDOR_ACCOUNT;
                            oldPaymentHdrObj.PVH_MODE = finPaymentVndHdrObj.PVH_MODE;
                            oldPaymentHdrObj.PVH_BANK = finPaymentVndHdrObj.PVH_BANK;
                            oldPaymentHdrObj.PVH_BRANCH = finPaymentVndHdrObj.PVH_BRANCH;
                            oldPaymentHdrObj.PVH_INSTR_NO = finPaymentVndHdrObj.PVH_INSTR_NO;
                            oldPaymentHdrObj.PVH_INSTR_DATE = finPaymentVndHdrObj.PVH_INSTR_DATE;
                            oldPaymentHdrObj.PVH_INSTR_FAVOUR = finPaymentVndHdrObj.PVH_INSTR_FAVOUR;
                            oldPaymentHdrObj.PVH_BANK_CASH_ACCOUNT = finPaymentVndHdrObj.PVH_BANK_CASH_ACCOUNT;
                            oldPaymentHdrObj.PVH_CURRENCY = finPaymentVndHdrObj.PVH_CURRENCY;
                            oldPaymentHdrObj.PVH_PAID_AMOUNT = finPaymentVndHdrObj.PVH_PAID_AMOUNT;
                            oldPaymentHdrObj.PVH_PAID_AMOUNT_BC = finPaymentVndHdrObj.PVH_PAID_AMOUNT_BC;
                            oldPaymentHdrObj.PVH_REMARKS = finPaymentVndHdrObj.PVH_REMARKS;
                            oldPaymentHdrObj.PVH_ACTIVE = finPaymentVndHdrObj.PVH_ACTIVE;
                            oldPaymentHdrObj.PVH_MOD_BY = finPaymentVndHdrObj.PVH_MOD_BY;
                            oldPaymentHdrObj.PVH_MOD_DT = DateTime.Now;

                            oldPaymentHdrObj.PVH_PDC = finPaymentVndHdrObj.PVH_PDC;
                            oldPaymentHdrObj.PVH_DISCOUNT = finPaymentVndHdrObj.PVH_DISCOUNT;
                            oldPaymentHdrObj.PVH_DISC_AMOUNT = finPaymentVndHdrObj.PVH_DISC_AMOUNT;
                            oldPaymentHdrObj.PVH_TAX_AMOUNT = finPaymentVndHdrObj.PVH_TAX_AMOUNT;
                            oldPaymentHdrObj.PVH_WHT_AMOUNT = finPaymentVndHdrObj.PVH_WHT_AMOUNT;
                            oldPaymentHdrObj.PVH_WHT_TAX = finPaymentVndHdrObj.PVH_WHT_TAX;
                            oldPaymentHdrObj.PVH_PAY_FOR_VENDOR = finPaymentVndHdrObj.PVH_PAY_FOR_VENDOR;
                            oldPaymentHdrObj.PVH_BANK_CHARGE = finPaymentVndHdrObj.PVH_BANK_CHARGE;
                            oldPaymentHdrObj.PVH_BANK_CHARGE_TYPE = finPaymentVndHdrObj.PVH_BANK_CHARGE_TYPE;
                            oldPaymentHdrObj.PVH_BANK_CHARGE_CURR = finPaymentVndHdrObj.PVH_BANK_CHARGE_CURR;
                            oldPaymentHdrObj.PVH_WHT_BOOK_NO = finPaymentVndHdrObj.PVH_WHT_BOOK_NO;
                            oldPaymentHdrObj.PVH_WHT_NO = finPaymentVndHdrObj.PVH_WHT_NO;
                            oldPaymentHdrObj.PVH_COMPANY = finPaymentVndHdrObj.PVH_COMPANY;
                            oldPaymentHdrObj.PVH_VENDOR_BANK = finPaymentVndHdrObj.PVH_VENDOR_BANK;
                            oldPaymentHdrObj.PVH_EXCHG_RATE = finPaymentVndHdrObj.PVH_EXCHG_RATE;
                            oldPaymentHdrObj.PVH_IS_WORK_ORDER = finPaymentVndHdrObj.PVH_IS_WORK_ORDER;
                            //Normal transaction=1,trading=2
                            oldPaymentHdrObj.PVH_TRX_TYPE = 1;
                            
                            // Sets return value as FIN_PAYMENT_VND_HDR pk
                            retval = finPaymentVndHdrObj.PVH_PK;
                            FinPaymentVndTrxMpgManagerObj = new FinPaymentVndTrxMpgManager(this.currentEntity);
                            //Save mapping Details                          
                            FinPaymentVndTrxMpgManagerObj.SavePaymentDtl(finPaymentVndTrxMpgList, finPaymentVndHdrObj.PVH_CATEGORY);

                            finPaymentVndTaxHdrList = finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR == null ?
                                new List<FIN_PAYMENT_VND_TAX_HDR>() : finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.ToList();
                            finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.Clear();
                            if (finPaymentVndTaxHdrList.Count > 0)
                            {
                                finPaymentVndTaxHdrmanagerObj = new FinPaymentVndTaxHdrmanager(this.currentEntity);
                                finPaymentVndTaxHdrmanagerObj.SaveWHTTax(finPaymentVndTaxHdrList);
                            }

                            //Save/update payment mode details
                            finPaymentModeDtlList = finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL == null ? new List<FIN_PAYMENT_VND_MODE_DTL>() : finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL.ToList();
                            finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL.Clear();
                            finPaymentModeDtlList.ForEach(dtl => dtl.PDM_PAYMENT_HDR = retval);
                            if (finPaymentModeDtlList.Count > 0)
                            {
                                FinPaymentVndTrxMpgManagerObj.SavePaymentModeDetails(finPaymentModeDtlList);
                            }
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }

                    //Dr - Vendor
                //    FinTrxManagerobj = new FinTrxManager(this.currentEntity);

                    //finTrxList = new List<FIN_TRX>();
                    //finTrxObj = new FIN_TRX(); //common function to initate object??
                    //finTrxObj2 = new FIN_TRX();//common function to initate object??

                    //finTrxObj2.FTR_PK = finTrxObj.FTR_PK = 0;
                    //finTrxObj2.FTR_DATE = finTrxObj.FTR_DATE = finPaymentVndHdrObj.PVH_DATE;
                    //finTrxObj2.FTR_REF_TYPE = finTrxObj.FTR_REF_TYPE = "PAYMENT";
                    //finTrxObj2.FTR_REF_PK = finTrxObj.FTR_REF_PK = finPaymentVndHdrObj.PVH_PK;
                    //finTrxObj2.FTR_REF_NO = finTrxObj.FTR_REF_NO = finPaymentVndHdrObj.PVH_NO;
                    //finTrxObj.FTR_SEQUENCE = 1;
                    //finTrxObj.FTR_ACCOUNT = finPaymentVndHdrObj.PVH_VENDOR_ACCOUNT;
                    //finTrxObj2.FTR_NARRATION = finTrxObj.FTR_NARRATION = "";
                    //finTrxObj2.FTR_TRX_CURR = finTrxObj.FTR_TRX_CURR = finPaymentVndHdrObj.PVH_CURRENCY;
                    //finTrxObj.FTR_DR_AMT_TC = finPaymentVndHdrObj.PVH_PAID_AMOUNT;
                    //finTrxObj.FTR_CR_AMT_TC = 0;
                    //finTrxObj2.FTR_BASE_CURR = finTrxObj.FTR_BASE_CURR = finPaymentVndHdrObj.PVH_BASE_CURR;
                    //finTrxObj2.FTR_EXCHG_RATE = finTrxObj.FTR_EXCHG_RATE = finPaymentVndHdrObj.PVH_EXCHG_RATE;
                    //finTrxObj.FTR_DR_AMT_BC = finPaymentVndHdrObj.PVH_PAID_AMOUNT_BC;
                    //finTrxObj.FTR_CR_AMT_BC = 0;
                    //finTrxObj2.FTR_REMARKS = finTrxObj.FTR_REMARKS = "";
                    //finTrxObj2.FTR_FIN_YEAR = finTrxObj.FTR_FIN_YEAR = FinYearMstManagerObj.GetFinYear(finPaymentVndHdrObj.PVH_DATE, finPaymentVndHdrObj.PVH_BIZUNIT);
                    //finTrxObj2.FTR_STATUS = finTrxObj.FTR_STATUS = 0;
                    //finTrxObj2.FTR_ACTIVE = finTrxObj.FTR_ACTIVE = 1;
                    //finTrxObj2.FTR_CRTD_BY = finTrxObj.FTR_CRTD_BY = finPaymentVndHdrObj.PVH_CRTD_BY;
                    //finTrxObj2.FTR_MOD_BY = finTrxObj.FTR_MOD_BY = finPaymentVndHdrObj.PVH_MOD_BY;
                    //finTrxObj2.FTR_DEPT = finTrxObj.FTR_DEPT = finPaymentVndHdrObj.PVH_DEPT;
                    //finTrxObj2.FTR_BIZUNIT = finTrxObj.FTR_BIZUNIT = finPaymentVndHdrObj.PVH_BIZUNIT;

                    //finTrxList.Add(finTrxObj);

                    ////Cr - Bank or Cash

                    //finTrxObj2.FTR_SEQUENCE = 2;
                    //finTrxObj2.FTR_ACCOUNT = finPaymentVndHdrObj.PVH_BANK_CASH_ACCOUNT;
                    //finTrxObj2.FTR_DR_AMT_TC = 0;
                    //finTrxObj2.FTR_CR_AMT_TC = finPaymentVndHdrObj.PVH_PAID_AMOUNT;
                    //finTrxObj2.FTR_DR_AMT_BC = 0;
                    //finTrxObj2.FTR_CR_AMT_BC = finPaymentVndHdrObj.PVH_PAID_AMOUNT_BC;

                    //finTrxList.Add(finTrxObj2);

                    //long finRetVal = FinTrxManagerobj.SaveFinTrx(finTrxList);

                }
                //return FIN_PAYMENT_VND_HDR pk
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

            //throw new NotImplementedException();
        }
        /// <summary>
        /// Get Payment Number List by autocomplete
        /// </summary>
        /// <param name="poPaymentObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_PAYMENT_VND_HDR> GetPaymentNumberAutoCompleteList(FIN_PAYMENT_VND_HDR poPaymentObj, ServiceUtility serviceUtilityObj)
        {
            //throw new NotImplementedException();

            List<FIN_PAYMENT_VND_HDR> POPaymentListObj = new List<FIN_PAYMENT_VND_HDR>();
            try
            {
                POPaymentListObj = (from pvh in this.currentEntity.FIN_PAYMENT_VND_HDR
                                    where pvh.PVH_NO.Contains(serviceUtilityObj.FilterValue) && pvh.PVH_STATUS == 2 
                                    && pvh.PVH_BIZUNIT ==( poPaymentObj.PVH_BIZUNIT>0 ? poPaymentObj.PVH_BIZUNIT : pvh.PVH_BIZUNIT)
                                    select pvh
                                    ).ToList();
            }
            catch
            {
            }
            return POPaymentListObj;
        }

        private static IQueryable<FIN_PAYMENT_VND_HDR> FilterEntity(FIN_PAYMENT_VND_HDR finPaymentVndHdrObj, IQueryable<FIN_PAYMENT_VND_HDR> qry, ServiceUtility utilityObj)
        {
            #region Filtering
            //if (finPaymentVndHdrObj.PVH_PK != 0)
            //    qry = qry.Where(pvh => pvh.PVH_PK == finPaymentVndHdrObj.PVH_PK);

            //if (finPaymentVndHdrObj.PVH_VENDOR != 0)
            //    qry = qry.Where(pvh => pvh.PVH_VENDOR == finPaymentVndHdrObj.PVH_VENDOR);

            if (finPaymentVndHdrObj.PVH_PK != 0)
            {
                qry = qry.Where(pvh => pvh.PVH_PK == finPaymentVndHdrObj.PVH_PK);
            }
            else
            {
                if (finPaymentVndHdrObj.PVH_VENDOR != 0)
                    qry = qry.Where(pvh => pvh.PVH_VENDOR == finPaymentVndHdrObj.PVH_VENDOR);
            }
            #endregion
            return qry;
        }
        #endregion

        public long? DeletePaymentHdr(long CurrPK)
        {
            long retval = 0;

            try
            {
                FIN_PAYMENT_VND_HDR FIN_PAYMENT_VND_HDR_Obj;
                List<FIN_PAYMENT_VND_TRX_MPG> FIN_PAYMENT_VND_TRX_MPG_LST_Obj;
                List<FIN_PAYMENT_VND_PO_MPG> fin_PAYMENT_VND_PO_MPG_List_Obj;
                FIN_INVOICE_VND_HDR FIN_INVOICE_VND_HDR_Obj;
                PUR_ORDER_HDR obj_PUR_ORDER_HDR;
                INV_WORK_ORDER_ITEM_HDR objINV_WORK_ORDER_ITEM_HDR;

                FIN_PAYMENT_VND_TAX_HDR objFIN_PAYMENT_VND_TAX_HDR;
                List<FIN_PAYMENT_VND_TAX_HDR> FIN_PAYMENT_VND_TAX_HDR_LST_obj;
                List<FIN_PAYMENT_VND_TAX_DTL> fin_PAYMENT_VND_TAX_DTL_List_Obj;
                List<FIN_PAYMENT_VND_ALCN_DTL> fin_FIN_PAYMENT_VND_ALCN_DTL_List_Obj;
                FIN_PAYMENT_VND_ALCN_DTL fin_FIN_PAYMENT_VND_ALCN_DTL_Obj;
                List<FIN_PAYMENT_VND_MODE_DTL> fin_FIN_PAYMENT_VND_MODE_DTL_List_Obj;
                List<FIN_PAYMENT_VND_CRDR_MPG> fin_FIN_PAYMENT_VND_CRDR_MPG_List_Obj;
                FinCoaMstManager FinCoaMstManagerDrAccObj = new FinCoaMstManager(this.currentEntity);
                //IQueryable<FIN_TRX> oldFIN_TRX;
                string refType;
                long refPK;
                //Get Wht Details & delete
                FIN_PAYMENT_VND_TAX_HDR_LST_obj=currentEntity.FIN_PAYMENT_VND_TAX_HDR.Where(tx=> tx.WTH_PAYMENT_HDR==CurrPK).ToList();

                foreach (FIN_PAYMENT_VND_TAX_HDR FIN_PAYMENT_VND_TAX_HDR_Obj in FIN_PAYMENT_VND_TAX_HDR_LST_obj)
                {
                    if (FIN_PAYMENT_VND_TAX_HDR_Obj != null)
                        this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.DeleteObject(FIN_PAYMENT_VND_TAX_HDR_Obj);
                }

                //objFIN_PAYMENT_VND_TAX_HDR = currentEntity.FIN_PAYMENT_VND_TAX_HDR.SingleOrDefault(sah => sah.WTH_PAYMENT_HDR == CurrPK);
                //if(objFIN_PAYMENT_VND_TAX_HDR!=null)
                //    this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.DeleteObject(objFIN_PAYMENT_VND_TAX_HDR);
               
                // Get Header Object
                FIN_PAYMENT_VND_HDR_Obj = currentEntity.FIN_PAYMENT_VND_HDR.SingleOrDefault(sah => sah.PVH_PK == CurrPK);

                // Get Details Object
                FIN_PAYMENT_VND_TRX_MPG_LST_Obj = FIN_PAYMENT_VND_HDR_Obj.FIN_PAYMENT_VND_TRX_MPG.ToList();
               
                // Reverse Invoice amount and delete details
                foreach (FIN_PAYMENT_VND_TRX_MPG FIN_PAYMENT_VND_TRX_MPG_Obj in FIN_PAYMENT_VND_TRX_MPG_LST_Obj)
                {
                    FIN_INVOICE_VND_HDR_Obj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == FIN_PAYMENT_VND_TRX_MPG_Obj.PVM_INVOICE_HDR);

                    if (FIN_INVOICE_VND_HDR_Obj != null)
                    {
                        FIN_INVOICE_VND_HDR_Obj.IVH_AMOUNT_PAID_TC -= FIN_PAYMENT_VND_TRX_MPG_Obj.PVM_PAID_AMOUNT;
                    }
                    //Delete Allocation details
                    fin_PAYMENT_VND_PO_MPG_List_Obj = FIN_PAYMENT_VND_TRX_MPG_Obj.FIN_PAYMENT_VND_PO_MPG.ToList();
                    foreach (FIN_PAYMENT_VND_PO_MPG fin_PAYMENT_VND_PO_MPG_obj in fin_PAYMENT_VND_PO_MPG_List_Obj)
                    {
                        if (fin_PAYMENT_VND_PO_MPG_obj.PPO_PO_HDR != null) {
                            obj_PUR_ORDER_HDR = currentEntity.PUR_ORDER_HDR.SingleOrDefault(a => a.POH_PK == fin_PAYMENT_VND_PO_MPG_obj.PPO_PO_HDR);
                            if (obj_PUR_ORDER_HDR != null)
                            {
                                obj_PUR_ORDER_HDR.POH_AMT_PAID -= fin_PAYMENT_VND_PO_MPG_obj.PPO_PAID_AMOUNT;
                            }
                        }
                        if (fin_PAYMENT_VND_PO_MPG_obj.PPO_WO_HDR != null)
                        {
                            objINV_WORK_ORDER_ITEM_HDR = currentEntity.INV_WORK_ORDER_ITEM_HDR.SingleOrDefault(a => a.WIH_PK == fin_PAYMENT_VND_PO_MPG_obj.PPO_WO_HDR);
                            if (objINV_WORK_ORDER_ITEM_HDR != null)
                            {
                                objINV_WORK_ORDER_ITEM_HDR.WIH_AMT_PAID -= fin_PAYMENT_VND_PO_MPG_obj.PPO_PAID_AMOUNT;
                            }
                        }
                        
                        this.currentEntity.FIN_PAYMENT_VND_PO_MPG.DeleteObject(fin_PAYMENT_VND_PO_MPG_obj);
                    }
                    //Delete Tax details
                    fin_PAYMENT_VND_TAX_DTL_List_Obj = FIN_PAYMENT_VND_TRX_MPG_Obj.FIN_PAYMENT_VND_TAX_DTL.ToList();
                    foreach (FIN_PAYMENT_VND_TAX_DTL fIN_PAYMENT_VND_TAX_DTL_obj in fin_PAYMENT_VND_TAX_DTL_List_Obj)
                    {
                        //obj_SAL_ORDER_HDR = currentEntity.SAL_ORDER_HDR.SingleOrDefault(a => a.SOH_PK == fin_RECEIPT_CUS_SO_MPG_obj.RSO_SO_HDR);
                        //if (obj_SAL_ORDER_HDR != null)
                        //{
                        //    obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED -= fin_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIVED_AMOUNT;
                        //}
                        this.currentEntity.FIN_PAYMENT_VND_TAX_DTL.DeleteObject(fIN_PAYMENT_VND_TAX_DTL_obj);
                    }

                    //Delete Adj allocation details
                    if (FIN_PAYMENT_VND_TRX_MPG_Obj.FIN_PAYMENT_VND_ALCN_DTL != null)
                    {
                        fin_FIN_PAYMENT_VND_ALCN_DTL_List_Obj = FIN_PAYMENT_VND_TRX_MPG_Obj.FIN_PAYMENT_VND_ALCN_DTL.ToList();
                        foreach (FIN_PAYMENT_VND_ALCN_DTL fIN_FIN_PAYMENT_VND_ALCN_DTL_obj in fin_FIN_PAYMENT_VND_ALCN_DTL_List_Obj)
                        {
                            if (fIN_FIN_PAYMENT_VND_ALCN_DTL_obj.PAD_ALCN_CDH.HasValue)
                            {
                                FIN_INVOICE_VND_HDR finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(sah => sah.IVH_PK == FIN_PAYMENT_VND_TRX_MPG_Obj.PVM_INVOICE_HDR);
                                if (finInvoiceVndHdrObj != null)
                                {
                                    finInvoiceVndHdrObj.IVH_AMOUNT_DN_TC -= fIN_FIN_PAYMENT_VND_ALCN_DTL_obj.PAD_AMOUNT;
                                }
                            }
                            this.currentEntity.FIN_PAYMENT_VND_ALCN_DTL.DeleteObject(fIN_FIN_PAYMENT_VND_ALCN_DTL_obj);
                        }
                    }                   


                    //Delete credit note allocation details
                    fin_FIN_PAYMENT_VND_CRDR_MPG_List_Obj = FIN_PAYMENT_VND_TRX_MPG_Obj.FIN_PAYMENT_VND_CRDR_MPG.ToList();
                    foreach (FIN_PAYMENT_VND_CRDR_MPG fin_FIN_PAYMENT_VND_CRDR_MPG_Obj in fin_FIN_PAYMENT_VND_CRDR_MPG_List_Obj)
                    {
                        this.currentEntity.FIN_PAYMENT_VND_CRDR_MPG.DeleteObject(fin_FIN_PAYMENT_VND_CRDR_MPG_Obj);
                    }


                    this.currentEntity.FIN_PAYMENT_VND_TRX_MPG.DeleteObject(FIN_PAYMENT_VND_TRX_MPG_Obj);
                }

               
                refType = "PAYMENT";
                refPK = CurrPK;

                //// Get Finance Posting details
                //oldFIN_TRX = this.currentEntity.FIN_TRX.Where(mpg => mpg.FTR_REF_TYPE.Equals(refType) && mpg.FTR_REF_PK == refPK);

                //// Reverse Posting and Update account balance
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
                if (FIN_PAYMENT_VND_HDR_Obj != null)
                {
                    //Delete payment modes
                    fin_FIN_PAYMENT_VND_MODE_DTL_List_Obj = FIN_PAYMENT_VND_HDR_Obj.FIN_PAYMENT_VND_MODE_DTL.ToList();
                    foreach (FIN_PAYMENT_VND_MODE_DTL fin_FIN_PAYMENT_VND_MODE_DTL_Obj in fin_FIN_PAYMENT_VND_MODE_DTL_List_Obj)
                    {
                        this.currentEntity.FIN_PAYMENT_VND_MODE_DTL.DeleteObject(fin_FIN_PAYMENT_VND_MODE_DTL_Obj);
                    }

                    this.currentEntity.FIN_PAYMENT_VND_HDR.DeleteObject(FIN_PAYMENT_VND_HDR_Obj);
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
        /// Update Payment Hdr Jounalize Flag
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdatePaymentHdrJounalizeFlag(int PayPK, bool JounalizeFlag)
        {
            long retval = 0;
            FIN_PAYMENT_VND_HDR OldFIN_PAYMENT_VND_HDR_Obj;
            try
            {
                retval = 0;
                OldFIN_PAYMENT_VND_HDR_Obj = currentEntity.FIN_PAYMENT_VND_HDR.SingleOrDefault(sah => sah.PVH_PK == PayPK);
                if (OldFIN_PAYMENT_VND_HDR_Obj != null)
                {
                    OldFIN_PAYMENT_VND_HDR_Obj.PVH_HAS_JRNL_ENTRY = JounalizeFlag;

                    retval = PayPK;
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

        public long? UpdatePaymentHdrPDCFlag(int paymentPK, byte pdcFlag)
        {
            long retval = 0;
            FIN_PAYMENT_VND_HDR oldFIN_PAYMENT_VND_HDR_Obj;
            try
            {

                retval = 0;
                oldFIN_PAYMENT_VND_HDR_Obj = currentEntity.FIN_PAYMENT_VND_HDR.SingleOrDefault(sah => sah.PVH_PK == paymentPK);
                if (oldFIN_PAYMENT_VND_HDR_Obj != null)
                {
                    oldFIN_PAYMENT_VND_HDR_Obj.PVH_PDC = pdcFlag;

                    //For multiple cheque
                    if (oldFIN_PAYMENT_VND_HDR_Obj.FIN_PAYMENT_VND_MODE_DTL != null && oldFIN_PAYMENT_VND_HDR_Obj.FIN_PAYMENT_VND_MODE_DTL.Count > 0)
                    {
                        foreach (FIN_PAYMENT_VND_MODE_DTL objModeDtl in oldFIN_PAYMENT_VND_HDR_Obj.FIN_PAYMENT_VND_MODE_DTL.Where(r=>r.PDM_PDC > 0).ToList())
                        {
                            FIN_PAYMENT_VND_MODE_DTL OldModeDtl = currentEntity.FIN_PAYMENT_VND_MODE_DTL.SingleOrDefault(mod => mod.PDM_PK == objModeDtl.PDM_PK);
                            OldModeDtl.PDM_PDC = pdcFlag;
                        }
                    }
                    
                    retval = paymentPK;
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


        public long? UpdatePaymentHdrBounceFlag(int paymentPK, byte bounceFlag)
        {
            long retval = 0;
            FIN_PAYMENT_VND_HDR oldFIN_PAYMENT_VND_HDR_Obj;
            try
            {

                retval = 0;
                oldFIN_PAYMENT_VND_HDR_Obj = currentEntity.FIN_PAYMENT_VND_HDR.SingleOrDefault(sah => sah.PVH_PK == paymentPK);
                if (oldFIN_PAYMENT_VND_HDR_Obj != null)
                {
                    oldFIN_PAYMENT_VND_HDR_Obj.PVH_BOUNCED = bounceFlag;

                    retval = paymentPK;
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
        ///check duplicate based on bank + InstrNo + InstrDate
        /// </summary>
        /// <param name="finPaymentVndHdrObj"></param>
        /// <returns></returns>
        public bool CheckPaymentHdr(FIN_PAYMENT_VND_HDR finPaymentVndHdrObj)
        {
            List<FIN_PAYMENT_VND_HDR> oldPaymentHdrList = currentEntity.FIN_PAYMENT_VND_HDR.Where(sah => sah.PVH_PK != finPaymentVndHdrObj.PVH_PK &&
                                                                                                     sah.PVH_BANK == finPaymentVndHdrObj.PVH_BANK &&
                                                                                                     sah.PVH_INSTR_NO == finPaymentVndHdrObj.PVH_INSTR_NO &&
                                                                                                     sah.PVH_INSTR_DATE == finPaymentVndHdrObj.PVH_INSTR_DATE
                                                                                                     && sah.PVH_DEL_STATUS==0).ToList();
            if (oldPaymentHdrList.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public long? DeleteDirectPaymentHdr(int CurrPK)
        {
            long retval = 0;

            try
            {
                FIN_TRX_HDR FIN_TRX_HDR_Obj;
                List<FIN_TRX> FIN_TRX_LST_Obj;              
                List<FIN_PAYMENT_VND_TAX_HDR> FIN_PAYMENT_VND_TAX_HDR_LST_obj;
                List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                FinCoaMstManager FinCoaMstManagerDrAccObj = new FinCoaMstManager(this.currentEntity);
                FinTrxManager FinTrxManagerobj = new FinTrxManager(this.currentEntity);
                CommonFunctionsManager ComnFnManagerObj = new CommonFunctionsManager(this.currentEntity);

                //IQueryable<FIN_TRX> oldFIN_TRX;
                //string refType;
                long refPK;
                //Get Wht Details & delete
                FIN_PAYMENT_VND_TAX_HDR_LST_obj=currentEntity.FIN_PAYMENT_VND_TAX_HDR.Where(tx=> tx.WTH_TRX_HDR==CurrPK).ToList();

                foreach (FIN_PAYMENT_VND_TAX_HDR FIN_PAYMENT_VND_TAX_HDR_Obj in FIN_PAYMENT_VND_TAX_HDR_LST_obj)
                {
                    if (FIN_PAYMENT_VND_TAX_HDR_Obj != null)
                        this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.DeleteObject(FIN_PAYMENT_VND_TAX_HDR_Obj);
                }
                               
                // Get Header Object
                FIN_TRX_HDR_Obj = currentEntity.FIN_TRX_HDR.SingleOrDefault(sah => sah.FTH_PK == CurrPK);

                // Get Details Object
                FIN_TRX_LST_Obj = FIN_TRX_HDR_Obj.FIN_TRX.ToList();

                // Delete Details
                List<FIN_TRX_COC_DTL> FIN_TRX_COC_DTL_LST_Obj;
                foreach (FIN_TRX FIN_TRX_Obj in FIN_TRX_LST_Obj)
                {
                    if (FIN_TRX_Obj != null)
                    {
                        #region Delete Cost Center Mapping
                        if (FIN_TRX_Obj.FIN_TRX_COC_DTL != null && FIN_TRX_Obj.FIN_TRX_COC_DTL.Count > 0)
                        {
                            FIN_TRX_COC_DTL_LST_Obj = FIN_TRX_Obj.FIN_TRX_COC_DTL.ToList();
                            foreach (FIN_TRX_COC_DTL FIN_TRX_COC_DTL_Obj in FIN_TRX_COC_DTL_LST_Obj)
                            {
                                if (FIN_TRX_COC_DTL_Obj != null)
                                {
                                    this.currentEntity.FIN_TRX_COC_DTL.DeleteObject(FIN_TRX_COC_DTL_Obj);
                                }
                            }
                        }
                        #endregion
                        this.currentEntity.FIN_TRX.DeleteObject(FIN_TRX_Obj);
                    }
                }
               
                //refType = "PAYMENT";
                refPK = CurrPK;

                // Delete Header
                if (FIN_TRX_HDR_Obj != null)
                {
                    // Save Log

                    ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                    AdmTrxLogDet.ATL_APP_TRX_CODE = FIN_TRX_HDR_Obj.FTH_VOUCHER_NO;
                    AdmTrxLogDet.ATL_APP_TRX_PK = FIN_TRX_HDR_Obj.FTH_PK;
                    AdmTrxLogDet.ATL_APP_TYPE = FIN_TRX_HDR_Obj.FTH_REF_TYPE;
                    AdmTrxLogDet.ATL_MOD_BY = FIN_TRX_HDR_Obj.FTH_MOD_BY;
                    AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                    AdmTrxLogDet.ATL_BIZUNIT = FIN_TRX_HDR_Obj.FTH_BIZUNIT;
                    AdmTrxLogDet.ATL_APP_TRX_PK = FIN_TRX_HDR_Obj.FTH_PK;
                    AdmTrxLogDet.ATL_ACTION = (byte)LogAction.DELETE;
                    AdmTrxLogDet.ATL_PK = 0;

                    AdmTrxLogList.Add(AdmTrxLogDet);
                    ComnFnManagerObj.SaveLog(AdmTrxLogList);

                    this.currentEntity.FIN_TRX_HDR.DeleteObject(FIN_TRX_HDR_Obj);
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
        public long? DeleteDirectVatTaxForPOPayment(long PaymentPk,byte WthCategory)
        {
            long? retval = 0;

            try
            {
               
                List<FIN_PAYMENT_VND_TAX_HDR> FIN_PAYMENT_VND_TAX_HDR_LST_obj; 
               
                //Get Wht Details & delete
                FIN_PAYMENT_VND_TAX_HDR_LST_obj = currentEntity.FIN_PAYMENT_VND_TAX_HDR.Where(tx => tx.WTH_PAYMENT_HDR == PaymentPk && tx.WTH_CATEGORY == WthCategory).ToList();

                foreach (FIN_PAYMENT_VND_TAX_HDR FIN_PAYMENT_VND_TAX_HDR_Obj in FIN_PAYMENT_VND_TAX_HDR_LST_obj)
                {
                    if (FIN_PAYMENT_VND_TAX_HDR_Obj != null)
                        this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.DeleteObject(FIN_PAYMENT_VND_TAX_HDR_Obj);
                }

                // update wht amount in payment
                if (WthCategory == (byte)WHTCategoryEnum.WHT)
                {                   
                    try
                    {                        
                        FIN_PAYMENT_VND_HDR paymenthdr = currentEntity.FIN_PAYMENT_VND_HDR.SingleOrDefault(a => a.PVH_PK == PaymentPk);
                        paymenthdr.PVH_WHT_AMOUNT = 0;
                    }
                    catch { }
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
        public long? DeleteDirectVatTaxForDirectPayment(long WthTrxPk,byte WthCategoty)
        {
            long? retval = 0;

            try
            {

                List<FIN_PAYMENT_VND_TAX_HDR> FIN_PAYMENT_VND_TAX_HDR_LST_obj;

                //Get Wht Details & delete
                FIN_PAYMENT_VND_TAX_HDR_LST_obj = currentEntity.FIN_PAYMENT_VND_TAX_HDR.Where(tx => tx.WTH_TRX_HDR == WthTrxPk && tx.WTH_CATEGORY == WthCategoty).ToList();

                foreach (FIN_PAYMENT_VND_TAX_HDR FIN_PAYMENT_VND_TAX_HDR_Obj in FIN_PAYMENT_VND_TAX_HDR_LST_obj)
                {
                    if (FIN_PAYMENT_VND_TAX_HDR_Obj != null)
                        this.currentEntity.FIN_PAYMENT_VND_TAX_HDR.DeleteObject(FIN_PAYMENT_VND_TAX_HDR_Obj);
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

        public long? SaveDirectVatTaxForPOPayment(List<FIN_PAYMENT_VND_TAX_HDR> tempWHTTaxDetails)
        {
            
            long? retval;            
            FinPaymentVndTaxHdrmanager finPaymentVndTaxHdrmanagerObj;
            try
            {
                finPaymentVndTaxHdrmanagerObj = new FinPaymentVndTaxHdrmanager(this.currentEntity);
                retval = finPaymentVndTaxHdrmanagerObj.SaveDirectVatTaxForPOPayment(tempWHTTaxDetails);
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
        public long? SaveWhtTaxDirectForDirectPayment(List<FIN_PAYMENT_VND_TAX_HDR> tempWHTTaxDetails)
        {

            long? retval;
            FinPaymentVndTaxHdrmanager finPaymentVndTaxHdrmanagerObj;
            try
            {
                finPaymentVndTaxHdrmanagerObj = new FinPaymentVndTaxHdrmanager(this.currentEntity);
                retval = finPaymentVndTaxHdrmanagerObj.SaveDirectVatTaxForDirectPayment(tempWHTTaxDetails);
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

        public List<FIN_INVOICE_VND_ADV_DED_DTL> GetAdvDeductList(long InvoicePk, int? PoPk)
        {
           
            try
            {
                var advDeductLst = from adv in this.currentEntity.FIN_INVOICE_VND_ADV_DED_DTL where adv.VAD_INVOICE_HDR == InvoicePk && adv.VAD_PO == ((PoPk.HasValue) ? PoPk : adv.VAD_PO) select adv;
                return advDeductLst.ToList();
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

        public List<PaymentCrdrMpg> GetCrDrAllocations(List<long> SelectedInvoicePks, long PaymentPk)
        {
            try
            {
                List<PaymentCrdrMpg> objCrdrList = new List<PaymentCrdrMpg>();
                List<PaymentCrdrMpg> objCrdrListFromPayment = new List<PaymentCrdrMpg>();
                List<PaymentCrdrMpg> objCrdrListFromCreditNote = new List<PaymentCrdrMpg>();
                List<long> InvoicePks = new List<long>();
                List<long> CrdrMpgPks = new List<long>();
                byte mode = Convert.ToByte(DebitCreditModeEnum.CREDIT);
                InvoicePks = SelectedInvoicePks;
                if ((InvoicePks == null || InvoicePks.Count == 0) && PaymentPk > 0)
                    InvoicePks = (from c in currentEntity.FIN_PAYMENT_VND_TRX_MPG where c.PVM_PAYMENT_HDR == PaymentPk && c.PVM_INVOICE_HDR.HasValue select c.PVM_INVOICE_HDR.Value).ToList();
                if (PaymentPk > 0)
                {
                    var ResultListFrmPayment = (from pmtcrdr in currentEntity.FIN_PAYMENT_VND_CRDR_MPG where
                                     pmtcrdr.PNM_PAYMENT_HDR == PaymentPk 
                                      select new PaymentCrdrMpg
                                      {
                                          PNM_ACTIVE = pmtcrdr.PNM_ACTIVE,
                                          PNM_PAYMENT_TRX_MPG = pmtcrdr.PNM_PAYMENT_TRX_MPG,
                                          PNM_PAYMENT_HDR = pmtcrdr.PNM_PAYMENT_HDR,
                                          PNM_ADJ_AMOUNT = pmtcrdr.PNM_ADJ_AMOUNT,
                                          PNM_CRDR_AMOUNT = pmtcrdr.FIN_CRDR_NOTE_MPG.CDM_AMOUNT,
                                          PNM_PAID_AMOUNT = pmtcrdr.PNM_PAID_AMOUNT,
                                          PNM_ALLOCATED_AMOUNT = (currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == pmtcrdr.PNM_CRDR_MPG
                                             && r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                             && r.PNM_PK != pmtcrdr.PNM_PK).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT)) == null ? 0 : currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == pmtcrdr.PNM_CRDR_MPG
                                             && r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                             && r.PNM_PK != pmtcrdr.PNM_PK).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT),
                                          PNM_BALANCE_AMOUNT = pmtcrdr.FIN_CRDR_NOTE_MPG.CDM_AMOUNT - ((currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == pmtcrdr.PNM_CRDR_MPG
                                             && r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                             && r.PNM_PK != pmtcrdr.PNM_PK).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT)) == null ? 0 : currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == pmtcrdr.PNM_CRDR_MPG
                                             && r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                             && r.PNM_PK != pmtcrdr.PNM_PK).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT)),
                                          PNM_CRDR_CURRENCY_TEXT = pmtcrdr.FIN_CRDR_NOTE_HDR.ADM_CURRENCY_MST1.CUR_CODE,
                                          PNM_CRDR_DATE = pmtcrdr.FIN_CRDR_NOTE_HDR.CDH_DATE,
                                          PNM_CRDR_HDR = pmtcrdr.FIN_CRDR_NOTE_HDR.CDH_PK,
                                          PNM_CRDR_NO = pmtcrdr.FIN_CRDR_NOTE_HDR.CDH_NO,
                                          PNM_INVOICE_HDR = pmtcrdr.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR.Value,
                                          PNM_PK = pmtcrdr.PNM_PK,
                                          PNM_CRDR_MPG = pmtcrdr.FIN_CRDR_NOTE_MPG.CDM_PK
                                      }).ToList();
                    objCrdrListFromPayment = ResultListFrmPayment.ToList();
                }

                if (objCrdrListFromPayment != null && objCrdrListFromPayment.Count > 0)
                    CrdrMpgPks = (from c in objCrdrListFromPayment select c.PNM_CRDR_MPG.Value).ToList();

                var ResultListFromCrdr = (from crdr in currentEntity.FIN_CRDR_NOTE_MPG                                  
                                  where crdr.FIN_CRDR_NOTE_HDR.CDH_TYPE == mode
                                  && !CrdrMpgPks.Contains(crdr.CDM_PK)
                                  && crdr.CDM_INVOICE_VND_HDR.HasValue
                                  && InvoicePks.Contains(crdr.CDM_INVOICE_VND_HDR.Value)
                                  && !crdr.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                  //&& crdr.FIN_CRDR_NOTE_HDR.CDH_HAS_JRNL_ENTRY  // bug : 21812 as per Manoj sir
                                  && crdr.FIN_CRDR_NOTE_HDR.CDH_STATUS > 0  //Submitted CN will displayed in CN allocation popup
                                  select new PaymentCrdrMpg
                                  {
                                      PNM_ACTIVE = 1,
                                      PNM_PAYMENT_TRX_MPG = 0,
                                      PNM_PAYMENT_HDR = 0,
                                      PNM_ADJ_AMOUNT = 0,
                                      PNM_CRDR_AMOUNT = crdr.CDM_AMOUNT,
                                      //PNM_PAID_AMOUNT = 0,
                                      PNM_PAID_AMOUNT = PaymentPk > 0 ? 0 : crdr.CDM_AMOUNT - ((currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == crdr.CDM_PK
                                         && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                         && r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                         && r.PNM_PAYMENT_HDR != PaymentPk).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT)) == null ? 0 : currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == crdr.CDM_PK
                                         && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                         && r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                         && r.PNM_PAYMENT_HDR != PaymentPk).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT)),
                                      PNM_ALLOCATED_AMOUNT = (currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == crdr.CDM_PK
                                         && r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                         && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT)) == null ? 0 : currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == crdr.CDM_PK
                                         && r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                         && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED                                         
                                         && r.PNM_PAYMENT_HDR != PaymentPk).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT),
                                      PNM_BALANCE_AMOUNT = crdr.CDM_AMOUNT - ((currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == crdr.CDM_PK
                                         && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                         && r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                         && r.PNM_PAYMENT_HDR != PaymentPk).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT)) == null ? 0 : currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == crdr.CDM_PK
                                         && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                         && r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                         && r.PNM_PAYMENT_HDR != PaymentPk).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT)),
                                      PNM_CRDR_CURRENCY_TEXT = crdr.FIN_CRDR_NOTE_HDR.ADM_CURRENCY_MST1.CUR_CODE,
                                      PNM_CRDR_DATE = crdr.FIN_CRDR_NOTE_HDR.CDH_DATE,
                                      PNM_CRDR_HDR = crdr.FIN_CRDR_NOTE_HDR.CDH_PK,
                                      PNM_CRDR_NO = crdr.FIN_CRDR_NOTE_HDR.CDH_NO,
                                      PNM_INVOICE_HDR = crdr.CDM_INVOICE_VND_HDR.Value,
                                      PNM_PK = 0,
                                      PNM_CRDR_MPG = crdr.CDM_PK
                                  }).ToList();
                var ResultList = (objCrdrListFromPayment.Union(ResultListFromCrdr)).ToList();
                objCrdrList = (from c in ResultList where c.PNM_BALANCE_AMOUNT > 0 orderby c.PNM_CRDR_NO ascending select c).ToList();
                return objCrdrList;

                //var ResultList = (from crdr in currentEntity.FIN_CRDR_NOTE_MPG
                //                 join pmtcrdr in currentEntity.FIN_PAYMENT_VND_CRDR_MPG
                //                 on crdr.CDM_PK equals pmtcrdr.PNM_CRDR_MPG into gj
                //                 from result in gj.DefaultIfEmpty()
                //                 where crdr.FIN_CRDR_NOTE_HDR.CDH_TYPE == mode && crdr.CDM_INVOICE_VND_HDR.HasValue
                //                 && InvoicePks.Contains(crdr.CDM_INVOICE_VND_HDR.Value)
                //                 && !crdr.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED && crdr.FIN_CRDR_NOTE_HDR.CDH_HAS_JRNL_ENTRY
                //                 //&& (result != null ? result.PNM_PAYMENT_HDR == (PaymentPk > 0 ? PaymentPk : result.PNM_PAYMENT_HDR) : true)
                //                 select new PaymentCrdrMpg
                //                 {
                //                     PNM_ACTIVE = 1,
                //                     PNM_PAYMENT_TRX_MPG = (result == null ? 0 : result.PNM_PAYMENT_TRX_MPG),
                //                     PNM_PAYMENT_HDR = (result == null ? 0 : result.PNM_PAYMENT_HDR),
                //                     PNM_ADJ_AMOUNT = (result == null ? 0 : result.PNM_ADJ_AMOUNT),
                //                     PNM_CRDR_AMOUNT = crdr.CDM_AMOUNT,
                //                     PNM_PAID_AMOUNT = (result == null ? 0 : result.PNM_PAID_AMOUNT),
                //                     PNM_ALLOCATED_AMOUNT = currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == crdr.CDM_PK
                //                        && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                //                        && r.PNM_PAYMENT_HDR != PaymentPk).Sum(sm => sm.PNM_PAID_AMOUNT) == null ? 0 : currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == crdr.CDM_PK
                //                        && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                //                        && r.PNM_PAYMENT_HDR != PaymentPk).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT),
                //                     PNM_BALANCE_AMOUNT = crdr.CDM_AMOUNT - (currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == crdr.CDM_PK
                //                        && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                //                        && r.PNM_PAYMENT_HDR != PaymentPk).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT) == null ? 0 : currentEntity.FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.PNM_CRDR_MPG == crdr.CDM_PK
                //                        && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                //                        && r.PNM_PAYMENT_HDR != PaymentPk).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT)),
                //                     PNM_CRDR_CURRENCY_TEXT = crdr.FIN_CRDR_NOTE_HDR.ADM_CURRENCY_MST1.CUR_CODE,
                //                     PNM_CRDR_DATE = crdr.FIN_CRDR_NOTE_HDR.CDH_DATE,
                //                     PNM_CRDR_HDR = crdr.FIN_CRDR_NOTE_HDR.CDH_PK,
                //                     PNM_CRDR_NO = crdr.FIN_CRDR_NOTE_HDR.CDH_NO,
                //                     PNM_INVOICE_HDR = crdr.CDM_INVOICE_VND_HDR.Value,
                //                     PNM_PK = (result == null ? 0 : result.PNM_PK),
                //                     PNM_CRDR_MPG = crdr.CDM_PK
                //                 }).ToList();

                //    objCrdrList = (from c in ResultList where c.PNM_BALANCE_AMOUNT > 0 orderby c.PNM_CRDR_NO ascending select c).ToList();
                //    return objCrdrList;
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


        public List<FIN_CRDR_NOTE_MPG> GetCrDrList(long InvoicePK)
        {
            try
            {
                var objCrdrPngList = from crdr in this.currentEntity.FIN_CRDR_NOTE_MPG
                                     where
                                         crdr.CDM_INVOICE_VND_HDR == InvoicePK
                                         && !crdr.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                         && !crdr.FIN_CRDR_NOTE_HDR.CDH_HAS_JRNL_ENTRY
                                         && crdr.FIN_CRDR_NOTE_HDR.CDH_STATUS == 0
                                         && crdr.FIN_CRDR_NOTE_HDR.CDH_TYPE == (byte)DebitCreditModeEnum.CREDIT
                                     select crdr;
                return objCrdrPngList.ToList();
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

        public FIN_INVOICE_VND_HDR GetInvoiceDetails(long NewInvPk)
        {
            try
            {
                FIN_INVOICE_VND_HDR objInvHdr = this.currentEntity.FIN_INVOICE_VND_HDR.SingleOrDefault(inv => inv.IVH_PK == NewInvPk);
                return objInvHdr;
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
