using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data.Objects;
using System.Data;
using ERPManager.Sales;
using BusinessObject.CommonManagement;

namespace ERPManager
{
    public class FinReceiptCusHdrManager : IFinReceiptCusHdrManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Receipt Header Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public FinReceiptCusHdrManager(ERPEntities currentEntity)
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
        /// Get Receipt Header List
        /// </summary>
        /// <param name="finRecipetHdrObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_RECEIPT_CUS_HDR> GetReceiptHdr(FIN_RECEIPT_CUS_HDR finRecipetHdrObj, ServiceUtility serviceUtilityObj, int? Status = null, string invNo = null, int? PDCStatus = 0)
        {
            IQueryable<FIN_RECEIPT_CUS_HDR> receiptHdrQuery;
            List<FIN_RECEIPT_CUS_HDR> finReceiptCusHdrListObj;

            int pageSize;
            try
            {
                pageSize = Convert.ToInt32(serviceUtilityObj.PageSize);
                //Selecting
                if (finRecipetHdrObj.RCH_PK != 0)
                {
                    receiptHdrQuery
                               = (from rch in this.currentEntity.FIN_RECEIPT_CUS_HDR
                                  join cus in this.currentEntity.CRM_CUSTOMER_MST on rch.RCH_CUSTOMER equals cus.CUS_PK
                                  //join invtrx in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG on rch.RCH_PK equals invtrx.RCM_RECEIPT_HDR
                                  //join sal in this.currentEntity.FIN_INVOICE_CUS_HDR on invtrx.RCM_INVOICE_HDR equals sal.ICH_PK
                                  //join cbm in this.currentEntity.FIN_CASH_BANK_MST on rch.RCH_BANK equals cbm.CBM_PK
                                  where rch.RCH_ACTIVE == finRecipetHdrObj.RCH_ACTIVE
                                      //&& rch.RCH_DATE >= (serviceUtilityObj.FilterDate == DateTime.MinValue ? rch.RCH_DATE : serviceUtilityObj.FilterDate)
                                      //&& rch.RCH_DATE <= (serviceUtilityObj.FilterToDate == DateTime.MinValue ? rch.RCH_DATE : serviceUtilityObj.FilterToDate)
                                      //&& (Status == 0 ? rch.RCH_HAS_JRNL_ENTRY == false : (Status == 1 ? rch.RCH_HAS_JRNL_ENTRY == true : (Status == 2 ? rch.RCH_STATUS != 2 : (Status == 4 ? rch.RCH_PDC == 1 : true))))
                                      //&& (Status == -1 ? rch.RCH_DEL_STATUS == 1 : rch.RCH_DEL_STATUS == 0)
                                      //&& (PDCStatus == 0 || PDCStatus == null ? true : rch.RCH_PDC == (byte)PDCStatus)
                                     && (rch.RCH_STATUS != 0 || finRecipetHdrObj.RCH_CRTD_BY == 0 || rch.RCH_CRTD_BY == finRecipetHdrObj.RCH_CRTD_BY)
                                  //&& (invNo == null ? sal.ICH_NO.Contains(sal.ICH_NO) : sal.ICH_NO.Contains(invNo))
                                  //&& (invNo == null ? true : rch.FIN_RECEIPT_CUS_TRX_MPG.Any(act => act.FIN_INVOICE_CUS_HDR.ICH_NO.Contains(invNo)))
                                  orderby rch.RCH_PK descending
                                  select rch
                                 );
                }
                else
                {
                    receiptHdrQuery
                               = (from rch in this.currentEntity.FIN_RECEIPT_CUS_HDR
                                  join cus in this.currentEntity.CRM_CUSTOMER_MST on rch.RCH_CUSTOMER equals cus.CUS_PK
                                  //join invtrx in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG on rch.RCH_PK equals invtrx.RCM_RECEIPT_HDR
                                  //join sal in this.currentEntity.FIN_INVOICE_CUS_HDR on invtrx.RCM_INVOICE_HDR equals sal.ICH_PK
                                  //join cbm in this.currentEntity.FIN_CASH_BANK_MST on rch.RCH_BANK equals cbm.CBM_PK
                                  where rch.RCH_ACTIVE == finRecipetHdrObj.RCH_ACTIVE
                                     && rch.RCH_DATE >= (serviceUtilityObj.FilterDate == DateTime.MinValue ? rch.RCH_DATE : serviceUtilityObj.FilterDate)
                                     && rch.RCH_DATE <= (serviceUtilityObj.FilterToDate == DateTime.MinValue ? rch.RCH_DATE : serviceUtilityObj.FilterToDate)
                                      //&& (Status == 0 ? rch.RCH_HAS_JRNL_ENTRY == false : (Status == 1 ? rch.RCH_HAS_JRNL_ENTRY == true : (Status == 2 ? rch.RCH_STATUS != 2 : (Status == 4 ? rch.RCH_PDC == 1 : true))))
                                     && (Status == 0 ? rch.RCH_HAS_JRNL_ENTRY == false : (Status == 1 ? rch.RCH_HAS_JRNL_ENTRY == true : (Status == 2 ? rch.RCH_STATUS == 0 : (Status == 4 ? rch.RCH_PDC == 1 : true)))) // RCH_STATUS ==0 - Means,only drafted records
                                     && (Status == -1 ? rch.RCH_DEL_STATUS == 1 : rch.RCH_DEL_STATUS == 0)
                                     && (PDCStatus == 0 || PDCStatus == null ? true : rch.RCH_PDC == (byte)PDCStatus)
                                     && (rch.RCH_STATUS != 0 || finRecipetHdrObj.RCH_CRTD_BY == 0 || rch.RCH_CRTD_BY == finRecipetHdrObj.RCH_CRTD_BY)
                                      //&& (invNo == null ? sal.ICH_NO.Contains(sal.ICH_NO) : sal.ICH_NO.Contains(invNo))
                                     && (invNo == null ? true : rch.FIN_RECEIPT_CUS_TRX_MPG.Any(act => act.FIN_INVOICE_CUS_HDR.ICH_NO.Contains(invNo)))
                                     && (rch.RCH_BIZUNIT == (finRecipetHdrObj.RCH_BIZUNIT > 0 ? finRecipetHdrObj.RCH_BIZUNIT : rch.RCH_BIZUNIT))
                                  orderby rch.RCH_PK descending
                                  select rch
                                 );
                }

                //receiptHdrQuery
                //            = (from rch in this.currentEntity.FIN_RECEIPT_CUS_HDR
                //               join cus in this.currentEntity.CRM_CUSTOMER_MST on rch.RCH_CUSTOMER equals cus.CUS_PK
                //               join invtrx in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG on rch.RCH_PK equals invtrx.RCM_RECEIPT_HDR
                //               join sal in this.currentEntity.FIN_INVOICE_CUS_HDR on invtrx.RCM_INVOICE_HDR equals sal.ICH_PK
                //               join cbm in this.currentEntity.FIN_CASH_BANK_MST on rch.RCH_BANK equals cbm.CBM_PK
                //               where rch.RCH_ACTIVE == finRecipetHdrObj.RCH_ACTIVE
                //                  && rch.RCH_DATE >= (serviceUtilityObj.FilterDate == DateTime.MinValue ? rch.RCH_DATE : serviceUtilityObj.FilterDate)
                //                  && rch.RCH_DATE <= (serviceUtilityObj.FilterToDate == DateTime.MinValue ? rch.RCH_DATE : serviceUtilityObj.FilterToDate)
                //                  && (Status == 0 ? rch.RCH_HAS_JRNL_ENTRY == false : (Status == 1 ? rch.RCH_HAS_JRNL_ENTRY == true : (Status == 2 ? rch.RCH_STATUS != 2 : (Status == 4 ? rch.RCH_PDC == 1 : true))))
                //                  && (Status == -1 ? rch.RCH_DEL_STATUS == 1 : rch.RCH_DEL_STATUS == 0)
                //                  && (PDCStatus == 0 || PDCStatus == null ? true : rch.RCH_PDC == (byte)PDCStatus)
                //                  && (invNo == null ? sal.ICH_NO.Contains(sal.ICH_NO) : sal.ICH_NO.Contains(invNo))
                //                  && (rch.RCH_STATUS != 0 || finRecipetHdrObj.RCH_CRTD_BY == 0 || rch.RCH_CRTD_BY == finRecipetHdrObj.RCH_CRTD_BY)
                //               orderby rch.RCH_PK descending
                //               select rch
                //              );

                //Get total row count


                //-------------------------------

                //Set page size one if not given


                //Filter Query

                receiptHdrQuery = FilterEntity(finRecipetHdrObj, receiptHdrQuery, serviceUtilityObj);

                serviceUtilityObj.TotalRecords = receiptHdrQuery.Count();

                serviceUtilityObj.PageSize = serviceUtilityObj.PageSize == 0 ? 1 : serviceUtilityObj.PageSize;

                #region Sorting
                // Apply Paging And Sorting For AD_TAX_GROUPS_MST grid Purpose
                // Checking sorting criteria is given

                #endregion
                //return Tax master details;
                finReceiptCusHdrListObj = receiptHdrQuery.SortRecords<FIN_RECEIPT_CUS_HDR>(serviceUtilityObj).ToList();
                //if (finReceiptCusHdrListObj.Count > 0)
                //    finReceiptCusHdrListObj.First().ROW_COUNT = totalCount;
                return finReceiptCusHdrListObj;
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
                finReceiptCusHdrListObj = null;
            }

            //return finReceiptCusHdrListObj;
        }

        /// <summary>
        /// Checks if Instr No already exists
        /// </summary>
        /// <param name="receiptPK"></param>
        /// <returns></returns>
        public bool? GetReceiptInstrNo(string instrNo, long receiptPK)
        {

            //List<FIN_RECEIPT_CUS_HDR> FinReceiptHdrListObj = new List<FIN_RECEIPT_CUS_HDR>();
            try
            {
                int result = (from rch in this.currentEntity.FIN_RECEIPT_CUS_HDR
                              where rch.RCH_INSTR_NO == instrNo && rch.RCH_PK != receiptPK && rch.RCH_INSTR_NO != null
                              && rch.RCH_DEL_STATUS == 0
                              select rch
                              ).Count();

                if (result <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Get Receipt Header Details
        /// </summary>
        /// <param name="receiptPK"></param>
        /// <returns></returns>
        public List<FIN_RECEIPT_CUS_HDR> GetReceiptHdr(long receiptPK)
        {

            List<FIN_RECEIPT_CUS_HDR> FinReceiptHdrListObj = new List<FIN_RECEIPT_CUS_HDR>();
            try
            {
                FinReceiptHdrListObj = (from rch in this.currentEntity.FIN_RECEIPT_CUS_HDR
                                        join cus in this.currentEntity.CRM_CUSTOMER_MST on rch.RCH_CUSTOMER equals cus.CUS_PK
                                        //join cbm in this.currentEntity.FIN_CASH_BANK_MST on rch.RCH_BANK equals cbm.CBM_PK
                                        where rch.RCH_PK == receiptPK
                                        //&& cfg.CFG_TYPE == "FINANCE TRX MODES"
                                        select rch
                    //{
                    //    RCH_PK = rch.RCH_PK
                    //  ,
                    //    RCH_DATE = rch.RCH_DATE
                    //  ,
                    //    RCH_NO = rch.RCH_NO
                    //  ,
                    //    RCH_CUSTOMER = rch.RCH_CUSTOMER
                    //  ,
                    //    RCH_CUSTOMER_TEXT = cus.CUS_NAME
                    //  ,
                    //    RCH_CUSTOMER_ACCOUNT = rch.RCH_CUSTOMER_ACCOUNT
                    //  ,
                    //    RCH_BANK = rch.RCH_BANK
                    //  ,
                    //    RCH_BANK_TEXT = cbm.CBM_NAME
                    //  ,
                    //    RCH_ACC_NO = cbm.CBM_ACC_NO
                    //  ,
                    //    RCH_MODE = rch.RCH_MODE
                    //  ,
                    //    RCH_MODE_TEXT = rch.RCH_MODE
                    //  ,
                    //    RCH_BRANCH = cbm.CBM_BRANCH
                    //  ,
                    //    RCH_INSTRUMENT = rch.RCH_INSTRUMENT
                    //  ,
                    //    RCH_BANK_CASH_ACCOUNT = rch.RCH_BANK_CASH_ACCOUNT
                    //  ,
                    //    RCH_BANK_CASH_ACCOUNT_TEXT = acv.COA_NAME
                    //  ,
                    //    RCH_RCVD_AMOUNT = rch.RCH_RCVD_AMOUNT
                    //  ,
                    //    RCH_REMARKS = rch.RCH_REMARKS
                    //  ,
                    //    RCH_STATUS = rch.RCH_STATUS
                    //  ,
                    //    RCH_MOD_DT = rch.RCH_MOD_DT
                    //}
                              ).ToList();
                return FinReceiptHdrListObj;
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
                FinReceiptHdrListObj = null;
            }
        }
        /// <summary>
        /// Save Receipt Header and Details
        /// </summary>
        /// <param name="finReceiptHdrList"></param>
        /// <returns></returns>
        public long? SaveReceiptHdr(List<FIN_RECEIPT_CUS_HDR> finReceiptCusHdrList)
        {
            //Holds save status
            long retval;
            FIN_RECEIPT_CUS_HDR oldReceiptHdrObj;
            List<FIN_RECEIPT_CUS_TRX_MPG> finReceiptCusTrxMpgList;
            FinReceiptCusTrxMpgManager FinReceiptCusTrxMpgManagerObj;
            List<FIN_RECEIPT_CUS_SUSP_DTL> finReceiptSuspList;

            //Fin Trx Objects
            //List<FIN_TRX> finTrxList;
            //FIN_TRX finTrxObj;
            //FIN_TRX finTrxObj2;
            FinTrxManager FinTrxManagerobj;
            FinYearMstManager FinYearMstManagerObj = new FinYearMstManager(this.currentEntity);
            int? maxReceiptPk;
            try
            {
                //Set save status zero,save failed
                retval = 0;
                //Iterate through shift attendance list for save
                foreach (FIN_RECEIPT_CUS_HDR finReceiptCusHdrObj in finReceiptCusHdrList)
                {
                    finReceiptCusTrxMpgList = finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.ToList();
                    finReceiptSuspList = finReceiptCusHdrObj.FIN_RECEIPT_CUS_SUSP_DTL.ToList();
                    finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.Clear();
                    finReceiptCusHdrObj.FIN_RECEIPT_CUS_SUSP_DTL.Clear();
                    if (finReceiptCusHdrObj.RCH_PK == 0)
                    {
                        // Gets last FIN_RECEIPT_CUS_HDR pk
                        maxReceiptPk = this.currentEntity.FIN_RECEIPT_CUS_HDR.Max(rch => (int?)rch.RCH_PK);

                        // Sets return value as next FIN_RECEIPT_CUS_HDR pk
                        retval = Convert.ToInt16((maxReceiptPk.HasValue ? maxReceiptPk.Value + 1 : 1));

                        // Sets next FIN_RECEIPT_CUS_HDR pk
                        finReceiptCusHdrObj.RCH_PK = retval;

                        // Sets FIN_RECEIPT_CUS_HDR created date time as current date time
                        finReceiptCusHdrObj.RCH_CRTD_DT = DateTime.Now;

                        // Sets FIN_RECEIPT_CUS_HDR modified date time as current date time
                        finReceiptCusHdrObj.RCH_MOD_DT = DateTime.Now;

                        //Normal transaction=1,trading=2
                        finReceiptCusHdrObj.RCH_TRX_TYPE = 1;

                        // Add new FIN_RECEIPT_CUS_HDR to the db context
                        this.currentEntity.FIN_RECEIPT_CUS_HDR.AddObject(finReceiptCusHdrObj);
                        //set the FIN_RECEIPT_CUS_HDR pk as the fk of  CAM_SHIFT_ATTENDANCE_Dtl
                        finReceiptCusTrxMpgList.ForEach(dtl => dtl.RCM_RECEIPT_HDR = retval);
                        FinReceiptCusTrxMpgManagerObj = new FinReceiptCusTrxMpgManager(this.currentEntity);
                        //Save mapping Details
                        FinReceiptCusTrxMpgManagerObj.SaveReceiptDtl(finReceiptCusTrxMpgList, finReceiptCusHdrObj.RCH_CATEGORY);
                        FinReceiptCusTrxMpgManagerObj.SaveReceiptSuspList(finReceiptSuspList, retval);
                    }
                    else
                    {
                        // updating FIN_RECEIPT_CUS_HDR
                        // Get current FIN_RECEIPT_CUS_HDR using FIN_RECEIPT_CUS_HDR pk and last modified date time,used for concurrency checking
                        oldReceiptHdrObj = currentEntity.FIN_RECEIPT_CUS_HDR.SingleOrDefault(sah => sah.RCH_PK == finReceiptCusHdrObj.RCH_PK && sah.RCH_MOD_DT == finReceiptCusHdrObj.RCH_MOD_DT);
                        // If oldReceiptHdrObj is null then,anyone modified or deleted the record

                        if (oldReceiptHdrObj != null)
                        {
                            // Update FIN_RECEIPT_CUS_HDR
                            oldReceiptHdrObj.RCH_CUSTOMER = finReceiptCusHdrObj.RCH_CUSTOMER;
                            oldReceiptHdrObj.RCH_NO = finReceiptCusHdrObj.RCH_NO;
                            oldReceiptHdrObj.RCH_DATE = finReceiptCusHdrObj.RCH_DATE;
                            oldReceiptHdrObj.RCH_CUSTOMER_ACCOUNT = finReceiptCusHdrObj.RCH_CUSTOMER_ACCOUNT;
                            oldReceiptHdrObj.RCH_MODE = finReceiptCusHdrObj.RCH_MODE;
                            oldReceiptHdrObj.RCH_BANK = finReceiptCusHdrObj.RCH_BANK;
                            oldReceiptHdrObj.RCH_INSTR_NO = finReceiptCusHdrObj.RCH_INSTR_NO;
                            oldReceiptHdrObj.RCH_INSTR_DATE = finReceiptCusHdrObj.RCH_INSTR_DATE;
                            oldReceiptHdrObj.RCH_BANK_CASH_ACCOUNT = finReceiptCusHdrObj.RCH_BANK_CASH_ACCOUNT;
                            oldReceiptHdrObj.RCH_CURRENCY = finReceiptCusHdrObj.RCH_CURRENCY;
                            oldReceiptHdrObj.RCH_RCVD_AMOUNT = finReceiptCusHdrObj.RCH_RCVD_AMOUNT;
                            oldReceiptHdrObj.RCH_REMARKS = finReceiptCusHdrObj.RCH_REMARKS;
                            oldReceiptHdrObj.RCH_ACTIVE = finReceiptCusHdrObj.RCH_ACTIVE;
                            oldReceiptHdrObj.RCH_MOD_BY = finReceiptCusHdrObj.RCH_MOD_BY;
                            oldReceiptHdrObj.RCH_MOD_DT = DateTime.Now;
                            oldReceiptHdrObj.RCH_CATEGORY = finReceiptCusHdrObj.RCH_CATEGORY;
                            oldReceiptHdrObj.RCH_GROUP = finReceiptCusHdrObj.RCH_GROUP;
                            //
                            oldReceiptHdrObj.RCH_DISCOUNT = finReceiptCusHdrObj.RCH_DISCOUNT;
                            oldReceiptHdrObj.RCH_DISC_AMOUNT = finReceiptCusHdrObj.RCH_DISC_AMOUNT;
                            oldReceiptHdrObj.RCH_TAX_AMOUNT = finReceiptCusHdrObj.RCH_TAX_AMOUNT;
                            oldReceiptHdrObj.RCH_PDC = finReceiptCusHdrObj.RCH_PDC;
                            oldReceiptHdrObj.RCH_BANK_CHARGE = finReceiptCusHdrObj.RCH_BANK_CHARGE;
                            oldReceiptHdrObj.RCH_BANK_CHARGE_CURR = finReceiptCusHdrObj.RCH_BANK_CHARGE_CURR;
                            oldReceiptHdrObj.RCH_COMPANY = finReceiptCusHdrObj.RCH_COMPANY;
                            oldReceiptHdrObj.RCH_BANK_OF_CHEQUE = finReceiptCusHdrObj.RCH_BANK_OF_CHEQUE;
                            //
                            oldReceiptHdrObj.RCH_RETURN_STATUS = finReceiptCusHdrObj.RCH_RETURN_STATUS;
                            oldReceiptHdrObj.RCH_RETURN_DATE = finReceiptCusHdrObj.RCH_RETURN_DATE;
                            oldReceiptHdrObj.RCH_RETURN_REMARKS = finReceiptCusHdrObj.RCH_RETURN_REMARKS;

                            //Normal transaction=1,trading=2
                            oldReceiptHdrObj.RCH_TRX_TYPE = 1;

                            // Sets return value as FIN_RECEIPT_CUS_HDR pk
                            retval = finReceiptCusHdrObj.RCH_PK;
                            FinReceiptCusTrxMpgManagerObj = new FinReceiptCusTrxMpgManager(this.currentEntity);
                            //Save Shift Attendance Details
                            FinReceiptCusTrxMpgManagerObj.SaveReceiptDtl(finReceiptCusTrxMpgList, finReceiptCusHdrObj.RCH_GROUP);
                            FinReceiptCusTrxMpgManagerObj.SaveReceiptSuspList(finReceiptSuspList, retval);
                        }
                        else
                        {
                            //throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }


                    }
                    //Dr - bank or cash
                    FinTrxManagerobj = new FinTrxManager(this.currentEntity);

                    //finTrxList = new List<FIN_TRX>();
                    //finTrxObj = new FIN_TRX(); //common function to initate object??
                    //finTrxObj2 = new FIN_TRX();//common function to initate object??

                    //finTrxObj2.FTR_PK = finTrxObj.FTR_PK = 0;
                    //finTrxObj2.FTR_DATE = finTrxObj.FTR_DATE = finReceiptCusHdrObj.RCH_DATE;
                    //finTrxObj2.FTR_REF_TYPE = finTrxObj.FTR_REF_TYPE = "RECEIPT";
                    //finTrxObj2.FTR_REF_PK = finTrxObj.FTR_REF_PK = finReceiptCusHdrObj.RCH_PK;
                    //finTrxObj2.FTR_REF_NO = finTrxObj.FTR_REF_NO = finReceiptCusHdrObj.RCH_NO;
                    //finTrxObj.FTR_SEQUENCE = 1;
                    //finTrxObj.FTR_ACCOUNT = finReceiptCusHdrObj.RCH_BANK_CASH_ACCOUNT;
                    //finTrxObj2.FTR_NARRATION = finTrxObj.FTR_NARRATION = "";
                    //finTrxObj2.FTR_TRX_CURR = finTrxObj.FTR_TRX_CURR = finReceiptCusHdrObj.RCH_CURRENCY;
                    //finTrxObj.FTR_DR_AMT_TC = finReceiptCusHdrObj.RCH_RCVD_AMOUNT;
                    //finTrxObj.FTR_CR_AMT_TC = 0;
                    //finTrxObj2.FTR_BASE_CURR = finTrxObj.FTR_BASE_CURR = finReceiptCusHdrObj.RCH_BASE_CURR;
                    //finTrxObj2.FTR_EXCHG_RATE = finTrxObj.FTR_EXCHG_RATE = finReceiptCusHdrObj.RCH_EXCHG_RATE;
                    //finTrxObj.FTR_DR_AMT_BC = finReceiptCusHdrObj.RCH_RCVD_AMOUNT_BC;
                    //finTrxObj.FTR_CR_AMT_BC = 0;
                    //finTrxObj2.FTR_REMARKS = finTrxObj.FTR_REMARKS = "";
                    //finTrxObj2.FTR_FIN_YEAR = finTrxObj.FTR_FIN_YEAR = FinYearMstManagerObj.GetFinYear(finReceiptCusHdrObj.RCH_DATE, finReceiptCusHdrObj.RCH_BIZUNIT);
                    //finTrxObj2.FTR_STATUS = finTrxObj.FTR_STATUS = 0;
                    //finTrxObj2.FTR_ACTIVE = finTrxObj.FTR_ACTIVE = 1;
                    //finTrxObj2.FTR_CRTD_BY = finTrxObj.FTR_CRTD_BY = finReceiptCusHdrObj.RCH_CRTD_BY;
                    //finTrxObj2.FTR_MOD_BY = finTrxObj.FTR_MOD_BY = finReceiptCusHdrObj.RCH_MOD_BY;
                    //finTrxObj2.FTR_DEPT = finTrxObj.FTR_DEPT = finReceiptCusHdrObj.RCH_DEPT;
                    //finTrxObj2.FTR_BIZUNIT = finTrxObj.FTR_BIZUNIT = finReceiptCusHdrObj.RCH_BIZUNIT;

                    //finTrxList.Add(finTrxObj);

                    ////Cr - customer

                    //finTrxObj2.FTR_SEQUENCE = 2;
                    //finTrxObj2.FTR_ACCOUNT = finReceiptCusHdrObj.RCH_CUSTOMER_ACCOUNT;
                    //finTrxObj2.FTR_DR_AMT_TC = 0;
                    //finTrxObj2.FTR_CR_AMT_TC = finReceiptCusHdrObj.RCH_RCVD_AMOUNT;
                    //finTrxObj2.FTR_DR_AMT_BC = 0;
                    //finTrxObj2.FTR_CR_AMT_BC = finReceiptCusHdrObj.RCH_RCVD_AMOUNT_BC;

                    //finTrxList.Add(finTrxObj2);

                    //long finRetVal = FinTrxManagerobj.SaveFinTrx(finTrxList);

                }
                //return FIN_RECEIPT_CUS_HDR pk
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

            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Receipt Number List by autocomplete
        /// </summary>
        /// <param name="poReceiptObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_RECEIPT_CUS_HDR> GetReceiptNumberAutoCompleteList(FIN_RECEIPT_CUS_HDR salesReceiptObj, ServiceUtility serviceUtilityObj)
        {
            List<FIN_RECEIPT_CUS_HDR> SalesReceiptListObj = new List<FIN_RECEIPT_CUS_HDR>();
            try
            {
                SalesReceiptListObj = (from rch in this.currentEntity.FIN_RECEIPT_CUS_HDR
                                       where rch.RCH_NO.Contains(serviceUtilityObj.FilterValue) && (rch.RCH_STATUS == 2 || rch.RCH_STATUS == 4) && !string.IsNullOrEmpty(rch.RCH_NO)
                                       && rch.RCH_BIZUNIT == serviceUtilityObj.BizUnit
                                       select rch
                                     ).ToList();//RCH_STATUS == 4 (Canceled Records)
                //SalesReceiptListObj = (from rch in this.currentEntity.FIN_RECEIPT_CUS_HDR
                //                       where rch.RCH_NO.Contains(serviceUtilityObj.FilterValue) && rch.RCH_STATUS ==2
                //                       select rch
                //                     ).ToList();

                SalesReceiptListObj = SalesReceiptListObj.OrderByDescending(odr => odr.RCH_NO).ToList();
            }
            catch
            {
            }
            return SalesReceiptListObj;
        }

        private static IQueryable<FIN_RECEIPT_CUS_HDR> FilterEntity(FIN_RECEIPT_CUS_HDR finReceiptCusHdrObj, IQueryable<FIN_RECEIPT_CUS_HDR> qry, ServiceUtility utilityObj)
        {
            #region Filtering
            //if (finReceiptCusHdrObj.RCH_PK != 0)
            //    qry = qry.Where(rch => rch.RCH_PK == finReceiptCusHdrObj.RCH_PK);

            //if (finReceiptCusHdrObj.RCH_CUSTOMER != 0)
            //    qry = qry.Where(rch => rch.RCH_CUSTOMER == finReceiptCusHdrObj.RCH_CUSTOMER);

            if (finReceiptCusHdrObj.RCH_PK != 0)
            {
                qry = qry.Where(rch => rch.RCH_PK == finReceiptCusHdrObj.RCH_PK);
            }
            else
            {
                if (finReceiptCusHdrObj.RCH_CUSTOMER != 0)
                    qry = qry.Where(rch => rch.RCH_CUSTOMER == finReceiptCusHdrObj.RCH_CUSTOMER);
            }

            // Filter by fault logged Date Range
            //if (utilityObj.FilterDate.HasValue == true && utilityObj.FilterToDate.HasValue == true)
            //{
            //    qry = qry.Where(rch => rch.RCH_DATE >= utilityObj.FilterDate && rch.RCH_DATE <= utilityObj.FilterToDate);
            //}

            //#endregion
            return qry;
        }
            #endregion

        public long? DeleteSalesReceiptHdr(long CurrPK)
        {
            long retval = 0;

            try
            {
                FIN_RECEIPT_CUS_HDR FIN_RECEIPT_CUS_HDR_Obj;
                List<FIN_RECEIPT_CUS_TRX_MPG> FIN_RECEIPT_CUS_TRX_MPG_LST_Obj;
                FIN_INVOICE_CUS_HDR FIN_INVOICE_CUS_HDR_Obj;
                List<FIN_RECEIPT_CUS_SO_MPG> fin_RECEIPT_CUS_SO_MPG_List_Obj;
                List<FIN_RECEIPT_CUS_TAX_DTL> fin_RECEIPT_CUS_TAX_DTL_List_Obj;
                List<FIN_RECEIPT_CUS_ALCN_DTL> fin_RECEIPT_CUS_ALCN_DTL_List_Obj;
                List<FIN_RECEIPT_CUS_ALCN_DTL> fin_FIN_RECEIPT_CUS_ALCN_DTL_List_Obj;
                FIN_RECEIPT_CUS_ALCN_DTL fin_FIN_RECEIPT_CUS_ALCN_DTL_Obj;
                SAL_ORDER_HDR obj_SAL_ORDER_HDR;
                List<FIN_RECEIPT_CUS_CRDR_MPG> fin_RECEIPT_CUS_CRDR_MPG_List_Obj;
                List<FIN_RECEIPT_CUS_SUSP_DTL> fin_RECEIPT_CUS_SUSP_DTL_List_Obj;
                //FinCoaMstManager FinCoaMstManagerDrAccObj = new FinCoaMstManager(this.currentEntity);
                //IQueryable<FIN_TRX> oldFIN_TRX;
                string refType;
                long refPK;

                // Get Header Object
                FIN_RECEIPT_CUS_HDR_Obj = currentEntity.FIN_RECEIPT_CUS_HDR.SingleOrDefault(sah => sah.RCH_PK == CurrPK);

                // Get Details Object
                FIN_RECEIPT_CUS_TRX_MPG_LST_Obj = FIN_RECEIPT_CUS_HDR_Obj.FIN_RECEIPT_CUS_TRX_MPG.ToList();

                // Reverse Invoice amount and delete details
                foreach (FIN_RECEIPT_CUS_TRX_MPG FIN_RECEIPT_CUS_TRX_MPG_Obj in FIN_RECEIPT_CUS_TRX_MPG_LST_Obj)
                {
                    FIN_INVOICE_CUS_HDR_Obj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == FIN_RECEIPT_CUS_TRX_MPG_Obj.RCM_INVOICE_HDR);

                    if (FIN_INVOICE_CUS_HDR_Obj != null)
                    {
                        FIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_RCVD_TC -= (FIN_RECEIPT_CUS_TRX_MPG_Obj.RCM_RCVD_AMOUNT + FIN_RECEIPT_CUS_TRX_MPG_Obj.FIN_RECEIPT_CUS_ALCN_DTL.Where(a => a.RAD_ALCN_RECEIPT_TRX != null).Sum(s => s.RAD_AMOUNT));
                    }

                    //Delete Allocation details
                    fin_RECEIPT_CUS_SO_MPG_List_Obj = FIN_RECEIPT_CUS_TRX_MPG_Obj.FIN_RECEIPT_CUS_SO_MPG.ToList();
                    foreach (FIN_RECEIPT_CUS_SO_MPG fin_RECEIPT_CUS_SO_MPG_obj in fin_RECEIPT_CUS_SO_MPG_List_Obj)
                    {
                        obj_SAL_ORDER_HDR = currentEntity.SAL_ORDER_HDR.SingleOrDefault(a => a.SOH_PK == fin_RECEIPT_CUS_SO_MPG_obj.RSO_SO_HDR);
                        if (obj_SAL_ORDER_HDR != null)
                        {
                            obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED -= (fin_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIVED_AMOUNT - fin_RECEIPT_CUS_SO_MPG_obj.FIN_RECEIPT_CUS_TRX_MPG.RCM_EXCESS_AMOUNT);
                        }
                        this.currentEntity.FIN_RECEIPT_CUS_SO_MPG.DeleteObject(fin_RECEIPT_CUS_SO_MPG_obj);
                    }
                    //Delete Tax details
                    fin_RECEIPT_CUS_TAX_DTL_List_Obj = FIN_RECEIPT_CUS_TRX_MPG_Obj.FIN_RECEIPT_CUS_TAX_DTL.ToList();
                    foreach (FIN_RECEIPT_CUS_TAX_DTL fin_RECEIPT_CUS_TAX_DTL_obj in fin_RECEIPT_CUS_TAX_DTL_List_Obj)
                    {
                        //obj_SAL_ORDER_HDR = currentEntity.SAL_ORDER_HDR.SingleOrDefault(a => a.SOH_PK == fin_RECEIPT_CUS_SO_MPG_obj.RSO_SO_HDR);
                        //if (obj_SAL_ORDER_HDR != null)
                        //{
                        //    obj_SAL_ORDER_HDR.SOH_AMT_RECEIVED -= fin_RECEIPT_CUS_SO_MPG_obj.RSO_RECEIVED_AMOUNT;
                        //}
                        this.currentEntity.FIN_RECEIPT_CUS_TAX_DTL.DeleteObject(fin_RECEIPT_CUS_TAX_DTL_obj);
                    }


                    //Delete Adj allocation details
                    if (FIN_RECEIPT_CUS_TRX_MPG_Obj.FIN_RECEIPT_CUS_ALCN_DTL != null)
                    {
                        fin_FIN_RECEIPT_CUS_ALCN_DTL_List_Obj = FIN_RECEIPT_CUS_TRX_MPG_Obj.FIN_RECEIPT_CUS_ALCN_DTL.ToList();
                        foreach (FIN_RECEIPT_CUS_ALCN_DTL fIN_FIN_RECEIPT_CUS_ALCN_DTL_obj in fin_FIN_RECEIPT_CUS_ALCN_DTL_List_Obj)
                        {
                            if (fIN_FIN_RECEIPT_CUS_ALCN_DTL_obj.RAD_ALCN_CDH.HasValue)
                            {
                                FIN_INVOICE_CUS_HDR finInvoiceVndHdrObj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == FIN_RECEIPT_CUS_TRX_MPG_Obj.RCM_INVOICE_HDR);
                                if (finInvoiceVndHdrObj != null)
                                {
                                    finInvoiceVndHdrObj.ICH_AMOUNT_CN_TC -= fIN_FIN_RECEIPT_CUS_ALCN_DTL_obj.RAD_AMOUNT;
                                }
                            }
                            this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL.DeleteObject(fIN_FIN_RECEIPT_CUS_ALCN_DTL_obj);
                        }
                    }

                    //Delete Alocation adjn details
                    fin_RECEIPT_CUS_ALCN_DTL_List_Obj = FIN_RECEIPT_CUS_TRX_MPG_Obj.FIN_RECEIPT_CUS_ALCN_DTL.ToList();
                    foreach (FIN_RECEIPT_CUS_ALCN_DTL fin_RECEIPT_CUS_ALCN_DTL_Obj in fin_RECEIPT_CUS_ALCN_DTL_List_Obj)
                    {
                        this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL.DeleteObject(fin_RECEIPT_CUS_ALCN_DTL_Obj);
                    }

                    //Delete debit note allocation details
                    fin_RECEIPT_CUS_CRDR_MPG_List_Obj = FIN_RECEIPT_CUS_TRX_MPG_Obj.FIN_RECEIPT_CUS_CRDR_MPG.ToList();
                    foreach (FIN_RECEIPT_CUS_CRDR_MPG fin_FIN_RECEIPT_CUS_CRDR_MPG_Obj in fin_RECEIPT_CUS_CRDR_MPG_List_Obj)
                    {
                        this.currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.DeleteObject(fin_FIN_RECEIPT_CUS_CRDR_MPG_Obj);
                    }

                    this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG.DeleteObject(FIN_RECEIPT_CUS_TRX_MPG_Obj);


                }


                refType = "RECEIPT";
                refPK = CurrPK;

                // Get Finance Posting details
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


                //GET Susp List Obj
                fin_RECEIPT_CUS_SUSP_DTL_List_Obj = FIN_RECEIPT_CUS_HDR_Obj.FIN_RECEIPT_CUS_SUSP_DTL.ToList();
                foreach (FIN_RECEIPT_CUS_SUSP_DTL fin_FIN_RECEIPT_SUSP_LIST_Obj in fin_RECEIPT_CUS_SUSP_DTL_List_Obj)
                {
                    this.currentEntity.FIN_RECEIPT_CUS_SUSP_DTL.DeleteObject(fin_FIN_RECEIPT_SUSP_LIST_Obj);
                }


                // Delete Header
                if (FIN_RECEIPT_CUS_HDR_Obj != null)
                {
                    this.currentEntity.FIN_RECEIPT_CUS_HDR.DeleteObject(FIN_RECEIPT_CUS_HDR_Obj);
                }
                else
                {
                    // throws exception already deleted or modified by other user
                    throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                }

                retval = 1;

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
        /// Update Receipt Hdr Jounalize Flag
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateReceiptHdrJounalizeFlag(int RecPK, bool JounalizeFlag)
        {
            long retval = 0;
            FIN_RECEIPT_CUS_HDR OldFIN_RECEIPT_CUS_HDR_Obj;
            try
            {

                retval = 0;
                OldFIN_RECEIPT_CUS_HDR_Obj = currentEntity.FIN_RECEIPT_CUS_HDR.SingleOrDefault(sah => sah.RCH_PK == RecPK);
                if (OldFIN_RECEIPT_CUS_HDR_Obj != null)
                {
                    OldFIN_RECEIPT_CUS_HDR_Obj.RCH_HAS_JRNL_ENTRY = JounalizeFlag;

                    retval = RecPK;
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
        /// Update Receipt Hdr Jounalize Flag
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateReceiptHdrPDCFlag(int RecPK, byte pdcFlag)
        {
            long retval = 0;
            FIN_RECEIPT_CUS_HDR OldFIN_RECEIPT_CUS_HDR_Obj;
            try
            {

                retval = 0;
                OldFIN_RECEIPT_CUS_HDR_Obj = currentEntity.FIN_RECEIPT_CUS_HDR.SingleOrDefault(sah => sah.RCH_PK == RecPK);
                if (OldFIN_RECEIPT_CUS_HDR_Obj != null)
                {
                    OldFIN_RECEIPT_CUS_HDR_Obj.RCH_PDC = pdcFlag;

                    retval = RecPK;
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
        /// Update Receipt Hdr Jounalize Flag
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateReceiptHdrBounceFlag(int RecPK, byte bounceFlag)
        {
            long retval = 0;
            FIN_RECEIPT_CUS_HDR OldFIN_RECEIPT_CUS_HDR_Obj;

            try
            {

                retval = 0;
                OldFIN_RECEIPT_CUS_HDR_Obj = currentEntity.FIN_RECEIPT_CUS_HDR.SingleOrDefault(sah => sah.RCH_PK == RecPK);
                if (OldFIN_RECEIPT_CUS_HDR_Obj != null)
                {
                    OldFIN_RECEIPT_CUS_HDR_Obj.RCH_BOUNCED = bounceFlag;
                    OldFIN_RECEIPT_CUS_HDR_Obj.FIN_RECEIPT_CUS_TRX_MPG.ToList().ForEach(dtl => dtl.RCM_BOUNCED = bounceFlag);
                    //OldFIN_RECEIPT_CUS_HDR_Obj.FIN_RECEIPT_CUS_TRX_MPG.ToList().ForEach(dtl => dtl.FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC = (dtl.FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC - dtl.RCM_RCVD_AMOUNT));

                    retval = RecPK;
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
        /// <param name="finReceiptCusHdrObj"></param>
        /// <returns></returns>
        public bool CheckReceiptHdr(FIN_RECEIPT_CUS_HDR finReceiptCusHdrObj)
        {
            try
            {
                List<FIN_RECEIPT_CUS_HDR> oldReceiptHdrList = currentEntity.FIN_RECEIPT_CUS_HDR.Where(sah => sah.RCH_PK != finReceiptCusHdrObj.RCH_PK &&
                                                                                                         sah.RCH_BANK == finReceiptCusHdrObj.RCH_BANK &&
                                                                                                         sah.RCH_INSTR_NO == finReceiptCusHdrObj.RCH_INSTR_NO &&
                                                                                                         sah.RCH_INSTR_DATE == finReceiptCusHdrObj.RCH_INSTR_DATE).ToList();
                if (oldReceiptHdrList.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

        }


        public List<ReceiptCrdrMpg> GetCrDrAllocations(List<long> SelectedInvoicePks, long ReceiptPk)
        {
            try
            {
                List<ReceiptCrdrMpg> objCrdrList = new List<ReceiptCrdrMpg>();
                List<ReceiptCrdrMpg> objCrdrListFromReceipt = new List<ReceiptCrdrMpg>();
                List<ReceiptCrdrMpg> objCrdrListFromCreditNote = new List<ReceiptCrdrMpg>();
                List<long> InvoicePks = new List<long>();
                List<long> CrdrMpgPks = new List<long>();
                byte mode = Convert.ToByte(DebitCreditModeEnum.DEBIT);
                InvoicePks = SelectedInvoicePks;
                if ((InvoicePks == null || InvoicePks.Count == 0) && ReceiptPk > 0)
                    InvoicePks = (from c in currentEntity.FIN_RECEIPT_CUS_TRX_MPG where c.RCM_RECEIPT_HDR == ReceiptPk && c.RCM_INVOICE_HDR.HasValue select c.RCM_INVOICE_HDR.Value).ToList();
                if (ReceiptPk > 0)
                {
                    var ResultListFrmPayment = (from pmtcrdr in currentEntity.FIN_RECEIPT_CUS_CRDR_MPG
                                                where pmtcrdr.RNM_RECEIPT_HDR == ReceiptPk
                                                select new ReceiptCrdrMpg
                                                {
                                                    RNM_ACTIVE = pmtcrdr.RNM_ACTIVE,
                                                    RNM_RECEIPT_TRX_MPG = pmtcrdr.RNM_RECEIPT_TRX_MPG,
                                                    RNM_RECEIPT_HDR = pmtcrdr.RNM_RECEIPT_HDR,
                                                    RNM_ADJ_AMOUNT = pmtcrdr.RNM_ADJ_AMOUNT,
                                                    RNM_CRDR_AMOUNT = pmtcrdr.FIN_CRDR_NOTE_MPG.CDM_AMOUNT + pmtcrdr.FIN_CRDR_NOTE_MPG.CDM_SHIP_CHARGE + pmtcrdr.FIN_CRDR_NOTE_MPG.CDM_OTHER_CHARGE,
                                                    RNM_PAID_AMOUNT = pmtcrdr.RNM_PAID_AMOUNT,
                                                    RNM_ALLOCATED_AMOUNT = (currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Where(r => r.RNM_CRDR_MPG == pmtcrdr.RNM_CRDR_MPG
                                                       && r.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                       && r.RNM_PK != pmtcrdr.RNM_PK).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT)) == null ? 0 : currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Where(r => r.RNM_CRDR_MPG == pmtcrdr.RNM_CRDR_MPG
                                                       && r.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                       && r.RNM_PK != pmtcrdr.RNM_PK).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT),
                                                    RNM_BALANCE_AMOUNT = (pmtcrdr.FIN_CRDR_NOTE_MPG.CDM_AMOUNT + pmtcrdr.FIN_CRDR_NOTE_MPG.CDM_SHIP_CHARGE + pmtcrdr.FIN_CRDR_NOTE_MPG.CDM_OTHER_CHARGE) - ((currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Where(r => r.RNM_CRDR_MPG == pmtcrdr.RNM_CRDR_MPG
                                                       && r.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                       && r.RNM_PK != pmtcrdr.RNM_PK).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT)) == null ? 0 : currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Where(r => r.RNM_CRDR_MPG == pmtcrdr.RNM_CRDR_MPG
                                                       && r.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                       && r.RNM_PK != pmtcrdr.RNM_PK).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT)),
                                                    RNM_CRDR_CURRENCY_TEXT = pmtcrdr.FIN_CRDR_NOTE_HDR.ADM_CURRENCY_MST1.CUR_CODE,
                                                    RNM_CRDR_DATE = pmtcrdr.FIN_CRDR_NOTE_HDR.CDH_DATE,
                                                    RNM_CRDR_HDR = pmtcrdr.FIN_CRDR_NOTE_HDR.CDH_PK,
                                                    RNM_CRDR_NO = pmtcrdr.FIN_CRDR_NOTE_HDR.CDH_NO,
                                                    RNM_INVOICE_HDR = pmtcrdr.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR.Value,
                                                    RNM_PK = pmtcrdr.RNM_PK,
                                                    RNM_CRDR_MPG = pmtcrdr.FIN_CRDR_NOTE_MPG.CDM_PK
                                                }).ToList();
                    objCrdrListFromReceipt = ResultListFrmPayment.ToList();
                }

                if (objCrdrListFromReceipt != null && objCrdrListFromReceipt.Count > 0)
                    CrdrMpgPks = (from c in objCrdrListFromReceipt select c.RNM_CRDR_MPG.Value).ToList();

                var ResultListFromCrdr = (from crdr in currentEntity.FIN_CRDR_NOTE_MPG
                                          where crdr.FIN_CRDR_NOTE_HDR.CDH_TYPE == mode
                                          && !CrdrMpgPks.Contains(crdr.CDM_PK)
                                          && crdr.CDM_INVOICE_CUS_HDR.HasValue
                                          && InvoicePks.Contains(crdr.CDM_INVOICE_CUS_HDR.Value)
                                          && !crdr.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                              //&& crdr.FIN_CRDR_NOTE_HDR.CDH_HAS_JRNL_ENTRY  // bug : 21812 as per Manoj sir
                                           && crdr.FIN_CRDR_NOTE_HDR.CDH_STATUS > 0    //Submitted DN will displayed in DN allocation popup
                                          select new ReceiptCrdrMpg
                                          {
                                              RNM_ACTIVE = 1,
                                              RNM_RECEIPT_TRX_MPG = 0,
                                              RNM_RECEIPT_HDR = 0,
                                              RNM_ADJ_AMOUNT = 0,
                                              RNM_CRDR_AMOUNT = crdr.CDM_AMOUNT + crdr.CDM_SHIP_CHARGE + crdr.CDM_OTHER_CHARGE,
                                              //RNM_PAID_AMOUNT = 0,
                                              RNM_PAID_AMOUNT = ReceiptPk > 0 ? 0 : (crdr.CDM_AMOUNT + crdr.CDM_SHIP_CHARGE + crdr.CDM_OTHER_CHARGE) - ((currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Where(r => r.RNM_CRDR_MPG == crdr.CDM_PK
                                                && r.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                                && r.RNM_RECEIPT_HDR != ReceiptPk).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT)) == null ? 0 : currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Where(r => r.RNM_CRDR_MPG == crdr.CDM_PK
                                                && r.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                                && r.RNM_RECEIPT_HDR != ReceiptPk).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT)),
                                              RNM_ALLOCATED_AMOUNT = (currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Where(r => r.RNM_CRDR_MPG == crdr.CDM_PK
                                                 && r.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                 && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT)) == null ? 0 : currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Where(r => r.RNM_CRDR_MPG == crdr.CDM_PK
                                                 && r.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                 && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                                 && r.RNM_RECEIPT_HDR != ReceiptPk).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT),
                                              RNM_BALANCE_AMOUNT = (crdr.CDM_AMOUNT + crdr.CDM_SHIP_CHARGE + crdr.CDM_OTHER_CHARGE) - ((currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Where(r => r.RNM_CRDR_MPG == crdr.CDM_PK
                                                 && r.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                 && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                                 && r.RNM_RECEIPT_HDR != ReceiptPk).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT)) == null ? 0 : currentEntity.FIN_RECEIPT_CUS_CRDR_MPG.Where(r => r.RNM_CRDR_MPG == crdr.CDM_PK
                                                 && r.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                 && !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                                 && r.RNM_RECEIPT_HDR != ReceiptPk).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT)),
                                              RNM_CRDR_CURRENCY_TEXT = crdr.FIN_CRDR_NOTE_HDR.ADM_CURRENCY_MST1.CUR_CODE,
                                              RNM_CRDR_DATE = crdr.FIN_CRDR_NOTE_HDR.CDH_DATE,
                                              RNM_CRDR_HDR = crdr.FIN_CRDR_NOTE_HDR.CDH_PK,
                                              RNM_CRDR_NO = crdr.FIN_CRDR_NOTE_HDR.CDH_NO,
                                              RNM_INVOICE_HDR = crdr.CDM_INVOICE_CUS_HDR.Value,
                                              RNM_PK = 0,
                                              RNM_CRDR_MPG = crdr.CDM_PK
                                          }).ToList();
                var ResultList = (objCrdrListFromReceipt.Union(ResultListFromCrdr)).ToList();
                objCrdrList = (from c in ResultList where c.RNM_BALANCE_AMOUNT > 0 orderby c.RNM_CRDR_NO ascending select c).ToList();
                return objCrdrList;

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



        public List<FIN_CRDR_NOTE_MPG> GetCrDrMpgList(long InvoicePK)
        {

            try
            {

                var objCrdrPngList = from crdr in this.currentEntity.FIN_CRDR_NOTE_MPG
                                     where
                                         crdr.CDM_INVOICE_CUS_HDR == InvoicePK
                                         && !crdr.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED
                                         && !crdr.FIN_CRDR_NOTE_HDR.CDH_HAS_JRNL_ENTRY
                                         && crdr.FIN_CRDR_NOTE_HDR.CDH_STATUS == 0
                                         && crdr.FIN_CRDR_NOTE_HDR.CDH_TYPE == (byte)DebitCreditModeEnum.DEBIT
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
    }
        #endregion
}
