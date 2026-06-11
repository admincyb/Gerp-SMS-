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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FinInvoiceCusHdrManager" in both code and config file together.
    public class FinInvoiceCusHdrManager : IFinInvoiceCusHdrManager
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
        public FinInvoiceCusHdrManager(ERPEntities currentEntity)
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
        public long SaveSalesInvoiceHdr(List<FIN_INVOICE_CUS_HDR> InvoiceHdrList, bool isWkfSave = false)
        {
            long retval = 0;
            FIN_INVOICE_CUS_HDR OldFIN_INVOICE_CUS_HDR_Obj;
            List<FIN_INVOICE_CUS_TRX_MPG> FIN_INVOICE_CUS_TRX_MPG_LST_Obj;
            FinInvoiceCusTrxMpgManager FinInvoiceCusTrxMpgManagerObj;
            long? maxINVPK;

            //List<FIN_TRX> finTrxList = new List<FIN_TRX>();
            //FIN_TRX finTrxObj;
            //FIN_TRX finTrxObj1;
            FinTrxManager FinTrxManagerobj = new FinTrxManager(this.currentEntity);
            FinYearMstManager FinYearMstManagerObj = new FinYearMstManager(this.currentEntity);
            CommonFunctionsManager ComnFnManagerObj = new CommonFunctionsManager(this.currentEntity);

            try
            {
                retval = 0;
                foreach (FIN_INVOICE_CUS_HDR FIN_INVOICE_CUS_HDR_Obj in InvoiceHdrList)
                {

                    //ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();

                    FIN_INVOICE_CUS_TRX_MPG_LST_Obj = FIN_INVOICE_CUS_HDR_Obj.FIN_INVOICE_CUS_TRX_MPG.ToList();
                    FIN_INVOICE_CUS_HDR_Obj.FIN_INVOICE_CUS_TRX_MPG.Clear();

                    if (FIN_INVOICE_CUS_HDR_Obj.ICH_PK == 0)
                    {
                        // Gets last FIN_PAYMENT_VND_HDR pk
                        maxINVPK = this.currentEntity.FIN_INVOICE_CUS_HDR.Max(pvh => (int?)pvh.ICH_PK);

                        // Sets return value as next FIN_PAYMENT_VND_HDR pk
                        retval = (maxINVPK.HasValue ? maxINVPK.Value + 1 : 1);



                        FIN_INVOICE_CUS_HDR_Obj.ICH_PK = retval;
                        //FIN_INVOICE_CUS_HDR_Obj.ICH_NO = retval.ToString();
                        FIN_INVOICE_CUS_HDR_Obj.ICH_CRTD_DT = DateTime.Now;
                        FIN_INVOICE_CUS_HDR_Obj.ICH_MOD_DT = DateTime.Now;

                        //Normal transaction=1,trading=2
                        FIN_INVOICE_CUS_HDR_Obj.ICH_TRX_TYPE = 1;

                        // Add new FIN_INVOICE_CUS_HDR to the db context
                        this.currentEntity.FIN_INVOICE_CUS_HDR.AddObject(FIN_INVOICE_CUS_HDR_Obj);
                        //set the FIN_INVOICE_CUS_HDR pk as the fk of  FIN_INVOICE_CUS_TRX_MPG
                        FIN_INVOICE_CUS_TRX_MPG_LST_Obj.ForEach(dtl => dtl.ICM_INVOICE_HDR = retval);
                        FinInvoiceCusTrxMpgManagerObj = new FinInvoiceCusTrxMpgManager(this.currentEntity);
                        //Save Shift Attendance Details
                        FinInvoiceCusTrxMpgManagerObj.SaveSalesInvoiceTrxMpg(FIN_INVOICE_CUS_TRX_MPG_LST_Obj, FIN_INVOICE_CUS_HDR_Obj.ICH_TYPE, isWkfSave);
                    }
                    else
                    {
                        // updating FIN_INVOICE_CUS_HDR
                        // Get current FIN_PAYMENT_VND_HDR using FIN_PAYMENT_VND_HDR pk and last modified date time,used for concurrency checking
                        OldFIN_INVOICE_CUS_HDR_Obj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == FIN_INVOICE_CUS_HDR_Obj.ICH_PK && sah.ICH_MOD_DT == FIN_INVOICE_CUS_HDR_Obj.ICH_MOD_DT);

                        //IQueryable<FIN_INVOICE_CUS_TRX_MPG> oldMpgList = this.currentEntity.FIN_INVOICE_CUS_TRX_MPG.Where(mpg => mpg.ICM_INVOICE_HDR == FIN_INVOICE_CUS_HDR_Obj.ICH_PK);



                        //// Delete existsing detail entries
                        ////this.currentEntity.FIN_INVOICE_CUS_TRX_MPG.DeleteObjects(OldFIN_INVOICE_CUS_HDR_Obj.FIN_INVOICE_CUS_TRX_MPG);
                        //foreach (FIN_INVOICE_CUS_TRX_MPG FIN_INVOICE_CUS_TRX_MPG_tObj in oldMpgList)
                        //{
                        //    this.currentEntity.FIN_INVOICE_CUS_TRX_MPG.DeleteObject(FIN_INVOICE_CUS_TRX_MPG_tObj);
                        //}

                        // If oldPaymentHdrObj is null then,anyone modified or deleted the record
                        if (OldFIN_INVOICE_CUS_HDR_Obj != null)
                        {
                            // Update FIN_PAYMENT_VND_HDR
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_DATE = FIN_INVOICE_CUS_HDR_Obj.ICH_DATE;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_NO = FIN_INVOICE_CUS_HDR_Obj.ICH_NO;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER = FIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_ACCOUNT = FIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_ACCOUNT;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_REFERENCE = FIN_INVOICE_CUS_HDR_Obj.ICH_REFERENCE;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_DATE_PAY_BY = FIN_INVOICE_CUS_HDR_Obj.ICH_DATE_PAY_BY;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CURRENCY = FIN_INVOICE_CUS_HDR_Obj.ICH_CURRENCY;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_TC = FIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_TC;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_DISCOUNT_TC = FIN_INVOICE_CUS_HDR_Obj.ICH_DISCOUNT_TC;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_TAX_TC = FIN_INVOICE_CUS_HDR_Obj.ICH_TAX_TC;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_NET_TC = FIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_NET_TC;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_BASE_CURR = FIN_INVOICE_CUS_HDR_Obj.ICH_BASE_CURR;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_EXCHG_RATE = FIN_INVOICE_CUS_HDR_Obj.ICH_EXCHG_RATE;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_NET_BC = FIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_NET_BC;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_REMARKS = FIN_INVOICE_CUS_HDR_Obj.ICH_REMARKS;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_ACTIVE = FIN_INVOICE_CUS_HDR_Obj.ICH_ACTIVE;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_MOD_BY = FIN_INVOICE_CUS_HDR_Obj.ICH_MOD_BY;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_MOD_DT = DateTime.Now;

                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CATEGORY = FIN_INVOICE_CUS_HDR_Obj.ICH_CATEGORY;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_TYPE = FIN_INVOICE_CUS_HDR_Obj.ICH_TYPE;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_ADDRESS = FIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_ADDRESS;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_COUNTRY = FIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_COUNTRY;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_EMAIL = FIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_EMAIL;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_FAX = FIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_FAX;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_MOBILE = FIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_MOBILE;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_NAME = FIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_NAME;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_PHONE = FIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_PHONE;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_ZIP = FIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_ZIP;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_PAYMENT_TERM = FIN_INVOICE_CUS_HDR_Obj.ICH_PAYMENT_TERM;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_PAYMENT_TERM_TEXT = FIN_INVOICE_CUS_HDR_Obj.ICH_PAYMENT_TERM_TEXT;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_SHIP_CHARGE = FIN_INVOICE_CUS_HDR_Obj.ICH_SHIP_CHARGE;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_ADJUST = FIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_ADJUST;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_COMPANY = FIN_INVOICE_CUS_HDR_Obj.ICH_COMPANY;

                            //Adding CustomerTypes Fileds
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_TAX_ID = FIN_INVOICE_CUS_HDR_Obj.ICH_TAX_ID;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_BRANCH = FIN_INVOICE_CUS_HDR_Obj.ICH_BRANCH;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_GST_TYPE = FIN_INVOICE_CUS_HDR_Obj.ICH_GST_TYPE;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_BRANCH_TYPE = FIN_INVOICE_CUS_HDR_Obj.ICH_BRANCH_TYPE;
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_BRANCH_TEXT = FIN_INVOICE_CUS_HDR_Obj.ICH_BRANCH_TEXT;

                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_SUB_TYPE = FIN_INVOICE_CUS_HDR_Obj.ICH_SUB_TYPE;

                            //Normal transaction=1,trading=2
                            OldFIN_INVOICE_CUS_HDR_Obj.ICH_TRX_TYPE = 1;

                            // Sets return value as FIN_PAYMENT_VND_HDR pk
                            retval = FIN_INVOICE_CUS_HDR_Obj.ICH_PK;
                            FinInvoiceCusTrxMpgManagerObj = new FinInvoiceCusTrxMpgManager(this.currentEntity);
                            //Save Shift Attendance Details
                            FinInvoiceCusTrxMpgManagerObj.SaveSalesInvoiceTrxMpg(FIN_INVOICE_CUS_TRX_MPG_LST_Obj, FIN_INVOICE_CUS_HDR_Obj.ICH_TYPE, isWkfSave);

                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }

                    //Save Fin Trx against Vendor coa

                    //Dr
                    //finTrxObj  = new FIN_TRX(); //common function to initate object??
                    //finTrxObj1 = new FIN_TRX(); //common function to initate object??

                    //finTrxObj1.FTR_PK = finTrxObj.FTR_PK = 0;
                    //finTrxObj1.FTR_DATE = finTrxObj.FTR_DATE = FIN_INVOICE_CUS_HDR_Obj.ICH_DATE;
                    //finTrxObj1.FTR_REF_TYPE = finTrxObj.FTR_REF_TYPE = "SALES INVOICE";
                    //finTrxObj1.FTR_REF_PK = finTrxObj.FTR_REF_PK = FIN_INVOICE_CUS_HDR_Obj.ICH_PK;
                    //finTrxObj1.FTR_REF_NO = finTrxObj.FTR_REF_NO = FIN_INVOICE_CUS_HDR_Obj.ICH_NO;
                    //finTrxObj.FTR_SEQUENCE = 1;
                    //finTrxObj.FTR_ACCOUNT = FIN_INVOICE_CUS_HDR_Obj.ICH_CUSTOMER_ACCOUNT; 
                    //finTrxObj1.FTR_NARRATION = finTrxObj.FTR_NARRATION = "";
                    //finTrxObj1.FTR_TRX_CURR = finTrxObj.FTR_TRX_CURR = FIN_INVOICE_CUS_HDR_Obj.ICH_CURRENCY;
                    //finTrxObj.FTR_DR_AMT_TC = FIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_NET_TC;
                    //finTrxObj.FTR_CR_AMT_TC = 0;
                    //finTrxObj1.FTR_BASE_CURR = finTrxObj.FTR_BASE_CURR = FIN_INVOICE_CUS_HDR_Obj.ICH_BASE_CURR; // get base cuurency??
                    //finTrxObj1.FTR_EXCHG_RATE = finTrxObj.FTR_EXCHG_RATE = FIN_INVOICE_CUS_HDR_Obj.ICH_EXCHG_RATE;
                    //finTrxObj.FTR_DR_AMT_BC = FIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_NET_BC;
                    //finTrxObj.FTR_CR_AMT_BC = 0;
                    //finTrxObj1.FTR_REMARKS = finTrxObj.FTR_REMARKS = "";
                    //finTrxObj1.FTR_FIN_YEAR = finTrxObj.FTR_FIN_YEAR = FinYearMstManagerObj.GetFinYear(FIN_INVOICE_CUS_HDR_Obj.ICH_DATE, FIN_INVOICE_CUS_HDR_Obj.ICH_BIZUNIT);// Get fin year??
                    //finTrxObj1.FTR_STATUS = finTrxObj.FTR_STATUS = 0;
                    //finTrxObj1.FTR_ACTIVE = finTrxObj.FTR_ACTIVE = 1;
                    //finTrxObj1.FTR_CRTD_BY = finTrxObj.FTR_CRTD_BY = FIN_INVOICE_CUS_HDR_Obj.ICH_CRTD_BY;
                    //finTrxObj1.FTR_MOD_BY = finTrxObj.FTR_MOD_BY = FIN_INVOICE_CUS_HDR_Obj.ICH_MOD_BY;
                    //finTrxObj1.FTR_DEPT = finTrxObj.FTR_DEPT = FIN_INVOICE_CUS_HDR_Obj.ICH_DEPT;
                    //finTrxObj1.FTR_BIZUNIT = finTrxObj.FTR_BIZUNIT = FIN_INVOICE_CUS_HDR_Obj.ICH_BIZUNIT;

                    //finTrxList.Add(finTrxObj);

                    ////Cr
                    //finTrxObj1.FTR_SEQUENCE  = 2;
                    //finTrxObj1.FTR_ACCOUNT   = 3; // Invoice Exp Acc
                    //finTrxObj1.FTR_DR_AMT_TC = 0;
                    //finTrxObj1.FTR_CR_AMT_TC = FIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_NET_TC;
                    //finTrxObj1.FTR_DR_AMT_BC = 0;
                    //finTrxObj1.FTR_CR_AMT_BC = FIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_NET_BC;

                    //finTrxList.Add(finTrxObj1);

                    //long finRetVal = FinTrxManagerobj.SaveFinTrx(finTrxList);
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
        /// Deletes list of Invoice Header
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        public long DeleteSalesInvoice(long invoicePK)
        {
            long retval = 0;

            try
            {
                CommonFunctionsManager ComnFnManagerObj = new CommonFunctionsManager(this.currentEntity);

                FIN_INVOICE_CUS_HDR FIN_INVOICE_CUS_HDR_Obj;
                List<FIN_INVOICE_CUS_TRX_MPG> FIN_INVOICE_CUS_TRX_MPG_LST_Obj;
                SAL_ORDER_HDR SAL_ORDER_HDR_Obj;
                FinCoaMstManager FinCoaMstManagerDrAccObj = new FinCoaMstManager(this.currentEntity);
                //IQueryable<FIN_TRX> oldFIN_TRX;
                string refType;
                long refPK;

                // Get Header Object
                FIN_INVOICE_CUS_HDR_Obj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == invoicePK);

                // Get Details Object
                FIN_INVOICE_CUS_TRX_MPG_LST_Obj = FIN_INVOICE_CUS_HDR_Obj.FIN_INVOICE_CUS_TRX_MPG.ToList();

                // Reverse Invoice amount and delete details
                foreach (FIN_INVOICE_CUS_TRX_MPG FIN_INVOICE_CUS_TRX_MPG_Obj in FIN_INVOICE_CUS_TRX_MPG_LST_Obj)
                {
                    //SAL_ORDER_HDR_Obj = currentEntity.SAL_ORDER_HDR.SingleOrDefault(sah => sah.SOH_PK == FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_SO_HDR);

                    //if (SAL_ORDER_HDR_Obj != null)
                    //{
                    //    SAL_ORDER_HDR_Obj.SOH_AMT_INVOICED -= FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_AMOUNT;
                    //}

                    this.currentEntity.FIN_INVOICE_CUS_TRX_MPG.DeleteObject(FIN_INVOICE_CUS_TRX_MPG_Obj);
                }

                refType = "SALES INVOICE";
                refPK = invoicePK;

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

                // Delete Header
                if (FIN_INVOICE_CUS_HDR_Obj != null)
                {
                    this.currentEntity.FIN_INVOICE_CUS_HDR.DeleteObject(FIN_INVOICE_CUS_HDR_Obj);
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
        /// Perform searching,filtering,sorting,and paging on Invoice Header;
        /// </summary>
        /// <param name="AD_COUNTRIES_MSTObj" value="Contract master object with Contract master pk and active status"></param>
        /// <param name="utilityObj" value="Search criteria object"></param>
        /// <returns>List of Contract master</returns>
        public List<FIN_INVOICE_CUS_HDR> GetSalesInvoiceHdr(FIN_INVOICE_CUS_HDR InvoiceHdrObj, ServiceUtility utilityObj = null, int? Status = null, string siNo = null)
        {
            List<FIN_INVOICE_CUS_HDR> InvoiceHdrList = null;
            IQueryable<FIN_INVOICE_CUS_HDR> FIN_INVOICE_CUS_HDRQuery;
            int pageSize;

            try
            {

                pageSize = Convert.ToInt32(utilityObj.PageSize);
                //Filtering
                //Selecting

                if (InvoiceHdrObj.ICH_PK > 0)
                {
                    FIN_INVOICE_CUS_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_CUS_HDR
                                                //join vnd in this.currentEntity.SAL_CUSTOMER_MASTER on inv.ICH_CUSTOMER equals vnd.CUS_PK
                                                //join invtrx in this.currentEntity.FIN_INVOICE_CUS_TRX_MPG on inv.ICH_PK equals invtrx.ICM_INVOICE_HDR
                                                //join sal in this.currentEntity.SAL_ORDER_HDR on invtrx.ICM_SO_HDR equals sal.SOH_PK
                                                where inv.ICH_ACTIVE == InvoiceHdrObj.ICH_ACTIVE
                                                  && inv.ICH_PK == InvoiceHdrObj.ICH_PK
                                                    //&& inv.ICH_CUSTOMER == (InvoiceHdrObj.ICH_CUSTOMER > 0 ? InvoiceHdrObj.ICH_CUSTOMER : inv.ICH_CUSTOMER)
                                                    //&& inv.ICH_DATE >= (utilityObj.FilterDate == null ? inv.ICH_DATE : utilityObj.FilterDate)
                                                    //&& inv.ICH_DATE <= (utilityObj.FilterToDate == null ? inv.ICH_DATE : utilityObj.FilterToDate)
                                                  && (InvoiceHdrObj.ICH_CATEGORY > 0 ? inv.ICH_CATEGORY == InvoiceHdrObj.ICH_CATEGORY : true)
                                                //&& (Status.HasValue ? (Status.Value == 0 ? inv.ICH_HAS_JRNL_ENTRY == false : (Status.Value == 1 ?
                                                //  inv.ICH_HAS_JRNL_ENTRY == true : (Status.Value == 2 ? inv.ICH_STATUS == 0 : true))) : true)
                                                //&& (Status.HasValue ? (Status == -1 ? inv.ICH_DEL_STATUS == 1 : inv.ICH_DEL_STATUS == 0) : true)
                                                //&& ((Status.HasValue && Status.Value == -1) ? inv.ICH_DEL_STATUS == 1 : inv.ICH_DEL_STATUS == InvoiceHdrObj.ICH_DEL_STATUS)
                                                //&& (InvoiceHdrObj.ICH_REFERENCE == string.Empty ? sal.SOH_NO.Contains(sal.SOH_NO) : sal.SOH_NO.Contains(InvoiceHdrObj.ICH_REFERENCE))
                                                //&& (siNo == null ? sal.SOH_NO.Contains(sal.SOH_NO) : sal.SOH_NO.Contains(siNo))
                                                //&& (siNo == null ? true : inv.FIN_INVOICE_CUS_TRX_MPG.Any(act => act.SAL_ORDER_HDR.SOH_NO.Contains(siNo)))
                                                select inv);
                }
                else
                {
                    FIN_INVOICE_CUS_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_CUS_HDR
                                                //join vnd in this.currentEntity.SAL_CUSTOMER_MASTER on inv.ICH_CUSTOMER equals vnd.CUS_PK
                                                //join invtrx in this.currentEntity.FIN_INVOICE_CUS_TRX_MPG on inv.ICH_PK equals invtrx.ICM_INVOICE_HDR
                                                //join sal in this.currentEntity.SAL_ORDER_HDR on invtrx.ICM_SO_HDR equals sal.SOH_PK
                                                where inv.ICH_ACTIVE == InvoiceHdrObj.ICH_ACTIVE
                                                  && inv.ICH_PK == (InvoiceHdrObj.ICH_PK > 0 ? InvoiceHdrObj.ICH_PK : inv.ICH_PK)
                                                  && inv.ICH_CUSTOMER == (InvoiceHdrObj.ICH_CUSTOMER > 0 ? InvoiceHdrObj.ICH_CUSTOMER : inv.ICH_CUSTOMER)
                                                  && (inv.ICH_STATUS != 0 || InvoiceHdrObj.ICH_CRTD_BY == 0 || inv.ICH_CRTD_BY == InvoiceHdrObj.ICH_CRTD_BY)
                                                  && inv.ICH_DATE >= (utilityObj.FilterDate == null ? inv.ICH_DATE : utilityObj.FilterDate)
                                                  && inv.ICH_DATE <= (utilityObj.FilterToDate == null ? inv.ICH_DATE : utilityObj.FilterToDate)
                                                  && (InvoiceHdrObj.ICH_CATEGORY > 0 ? inv.ICH_CATEGORY == InvoiceHdrObj.ICH_CATEGORY : true)
                                                  && (Status.HasValue ? (Status.Value == 0 ? inv.ICH_HAS_JRNL_ENTRY == false : (Status.Value == 1 ?
                                                    inv.ICH_HAS_JRNL_ENTRY == true : (Status.Value == 2 ? inv.ICH_STATUS == 0 : true))) : true)
                                                  && (Status == 3 ? inv.ICH_DEL_STATUS == 0 : true)
                                                  && (Status == -1 ? inv.ICH_DEL_STATUS == 1 : true)
                                                    //&& (Status.HasValue ? (Status == -1 ? inv.ICH_DEL_STATUS == 1 : inv.ICH_DEL_STATUS == 0) : true)
                                                    //&& ((Status.HasValue && Status.Value == -1) ? inv.ICH_DEL_STATUS == 1 : inv.ICH_DEL_STATUS == InvoiceHdrObj.ICH_DEL_STATUS)
                                                  && (utilityObj.InvoiceType > 0 ? inv.ICH_TYPE == utilityObj.InvoiceType : true)
                                                    //&& (InvoiceHdrObj.ICH_REFERENCE == string.Empty ? sal.SOH_NO.Contains(sal.SOH_NO) : sal.SOH_NO.Contains(InvoiceHdrObj.ICH_REFERENCE))
                                                    //&& (siNo == null ? sal.SOH_NO.Contains(sal.SOH_NO) : sal.SOH_NO.Contains(siNo))
                                                  && (siNo == null ? true : inv.FIN_INVOICE_CUS_TRX_MPG.Any(act => act.SAL_ORDER_HDR.SOH_NO.Contains(siNo)))
                                                  && (inv.ICH_BIZUNIT == (InvoiceHdrObj.ICH_BIZUNIT > 0 ? InvoiceHdrObj.ICH_BIZUNIT : inv.ICH_BIZUNIT))
                                                   && inv.ICH_COMPANY == (InvoiceHdrObj.ICH_COMPANY > 0 ? InvoiceHdrObj.ICH_COMPANY : inv.ICH_COMPANY)
                                                select inv);
                }
                //FIN_INVOICE_CUS_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_CUS_HDR
                //                            //join vnd in this.currentEntity.SAL_CUSTOMER_MASTER on inv.ICH_CUSTOMER equals vnd.CUS_PK
                //                            join invtrx in this.currentEntity.FIN_INVOICE_CUS_TRX_MPG on inv.ICH_PK equals invtrx.ICM_INVOICE_HDR
                //                            join sal in this.currentEntity.SAL_ORDER_HDR on invtrx.ICM_SO_HDR equals sal.SOH_PK
                //                            where inv.ICH_ACTIVE == InvoiceHdrObj.ICH_ACTIVE
                //                              && inv.ICH_PK == (InvoiceHdrObj.ICH_PK > 0 ? InvoiceHdrObj.ICH_PK : inv.ICH_PK)
                //                              && inv.ICH_CUSTOMER == (InvoiceHdrObj.ICH_CUSTOMER > 0 ? InvoiceHdrObj.ICH_CUSTOMER : inv.ICH_CUSTOMER)
                //                              && inv.ICH_DATE >= (utilityObj.FilterDate == null ? inv.ICH_DATE : utilityObj.FilterDate)
                //                              && inv.ICH_DATE <= (utilityObj.FilterToDate == null ? inv.ICH_DATE : utilityObj.FilterToDate)
                //                              && (InvoiceHdrObj.ICH_CATEGORY > 0 ? inv.ICH_CATEGORY == InvoiceHdrObj.ICH_CATEGORY : true)
                //                              && (Status.HasValue ? (Status.Value == 0 ? inv.ICH_HAS_JRNL_ENTRY == false : (Status.Value == 1 ?
                //                                inv.ICH_HAS_JRNL_ENTRY == true : (Status.Value == 2 ? inv.ICH_STATUS == 0 : true))) : true)
                //                              && (Status.HasValue ? (Status == -1 ? inv.ICH_DEL_STATUS == 1 : inv.ICH_DEL_STATUS == 0) : true)
                //                              && ((Status.HasValue && Status.Value == -1) ? inv.ICH_DEL_STATUS == 1 : inv.ICH_DEL_STATUS == InvoiceHdrObj.ICH_DEL_STATUS)
                //                                //&& (InvoiceHdrObj.ICH_REFERENCE == string.Empty ? sal.SOH_NO.Contains(sal.SOH_NO) : sal.SOH_NO.Contains(InvoiceHdrObj.ICH_REFERENCE))
                //                           && (siNo == null ? sal.SOH_NO.Contains(sal.SOH_NO) : sal.SOH_NO.Contains(siNo))
                //                            select inv);


                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = FIN_INVOICE_CUS_HDRQuery.Count();
                //Filter Query

                //paymentHdrQuery = FilterEntity(finPaymentVndHdrListObj, paymentHdrQuery, serviceUtilityObj);

                #region Sorting
                // Apply Paging And Sorting For AD_TAX_GROUPS_MST grid Purpose
                // Checking sorting criteria is given

                #endregion
                //return Tax master details;
                InvoiceHdrList = FIN_INVOICE_CUS_HDRQuery.SortRecords<FIN_INVOICE_CUS_HDR>(utilityObj).ToList();


                //return Invoice Hdr List
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

        //public List<FIN_INVOICE_CUS_HDR> GetSalesInvoiceHdrDetails(long invoicePK)
        //{
        //    throw new NotImplementedException();
        //}

        public List<FIN_INVOICE_CUS_HDR> GetAdvanceSalesInvoiceNumberAutoCompleteList(FIN_INVOICE_CUS_HDR objSalesHeader, ServiceUtility utilityObj)
        {
            List<FIN_INVOICE_CUS_HDR> FIN_INVOICE_CUS_HDRListObj = new List<FIN_INVOICE_CUS_HDR>();
            try
            {
                FIN_INVOICE_CUS_HDRListObj = (from inv in this.currentEntity.FIN_INVOICE_CUS_HDR
                                              where inv.ICH_NO.Contains(utilityObj.FilterValue) && inv.ICH_STATUS > 0 && inv.ICH_CATEGORY == 2
                                              && inv.ICH_GROUP == 0
                                              && !string.IsNullOrEmpty(inv.ICH_NO)
                                              select inv).ToList();
                FIN_INVOICE_CUS_HDRListObj = FIN_INVOICE_CUS_HDRListObj.OrderBy(o => o.ICH_NO).ToList();
            }
            catch
            {
            }
            return FIN_INVOICE_CUS_HDRListObj;
        }

        public List<FIN_INVOICE_CUS_HDR> GetSalesInvoiceNumberAutoCompleteList(FIN_INVOICE_CUS_HDR objSalesHeader, ServiceUtility utilityObj)
        {
            List<FIN_INVOICE_CUS_HDR> FIN_INVOICE_CUS_HDRListObj = new List<FIN_INVOICE_CUS_HDR>();
            try
            {
                FIN_INVOICE_CUS_HDRListObj = (from inv in this.currentEntity.FIN_INVOICE_CUS_HDR
                                              where inv.ICH_NO.Contains(utilityObj.FilterValue) && inv.ICH_STATUS > 0 && inv.ICH_CATEGORY == 1//Changed ICH_STATUS ==2 to >0
                                              && inv.ICH_GROUP == objSalesHeader.ICH_GROUP
                                              && !string.IsNullOrEmpty(inv.ICH_NO)
                                              &&inv.ICH_BIZUNIT==objSalesHeader.ICH_BIZUNIT
                                              select inv).ToList();
                FIN_INVOICE_CUS_HDRListObj = FIN_INVOICE_CUS_HDRListObj.OrderBy(o => o.ICH_NO).ToList();
            }
            catch
            {
            }
            return FIN_INVOICE_CUS_HDRListObj;
        }


        /// <summary>
        /// Update Invoice Hdr Jounalize Flag
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateInvoiceHdrJounalizeFlag(int InvPK, bool JounalizeFlag)
        {
            long retval = 0;
            FIN_INVOICE_CUS_HDR OldFIN_INVOICE_CUS_HDR_Obj;

            try
            {
                retval = 0;
                OldFIN_INVOICE_CUS_HDR_Obj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == InvPK);
                if (OldFIN_INVOICE_CUS_HDR_Obj != null)
                {
                    OldFIN_INVOICE_CUS_HDR_Obj.ICH_HAS_JRNL_ENTRY = JounalizeFlag;

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

        //public string GetFIN_INVOICE_CUS_HDRNo(ApplicationSubType astPK, int dept, DateTime date, int user, bool update, int appPK)
        //{
        //    try
        //    {
        //        string trxNo = string.Empty;
        //        List<SPADM_TRX_DOC_NO_GENERATE_Result> resultTrxNo;
        //        ObjectParameter paramReturn = new ObjectParameter("P_RET_VAL", typeof(int));
        //        resultTrxNo = this.currentEntity.SPADM_TRX_DOC_NO_GENERATE((int)astPK, dept, date, user, update, appPK, paramReturn).ToList();
        //        if (resultTrxNo != null && resultTrxNo.Count > 0)
        //            trxNo = resultTrxNo.First().NEXT_NO;
        //        return trxNo;
        //    }
        //    catch (OptimisticConcurrencyException ex)
        //    {
        //        //Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    //Exception handler for Delete - Delete Concurrency
        //    catch (ArgumentNullException ex)
        //    {
        //        //Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //}

        #endregion




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
                                            && inv.ICH_DEL_STATUS == InvoiceHdrObj.ICH_DEL_STATUS
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

        public long? InActiveSalesInvoice(FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj)
        {
            long retval = 0;
            try
            {
                FIN_INVOICE_CUS_HDR OldFIN_INVOICE_CUS_HDR_Obj;
                List<FIN_INVOICE_CUS_TRX_MPG> finInvoiceCusTrxMpgListObj;
                FIN_INVOICE_CUS_TRX_MPG OLD_FIN_INVOICE_CUS_TRX_MPG_Obj;
                SAL_ORDER_HDR salOrderHdrObj;

                OldFIN_INVOICE_CUS_HDR_Obj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == finInvoiceCusHdrObj.ICH_PK && sah.ICH_MOD_DT == finInvoiceCusHdrObj.ICH_MOD_DT);
                if (OldFIN_INVOICE_CUS_HDR_Obj != null)
                {
                    OldFIN_INVOICE_CUS_HDR_Obj.ICH_DEL_STATUS = 1;
                    OldFIN_INVOICE_CUS_HDR_Obj.ICH_REASON_FOR_DELETE = finInvoiceCusHdrObj.ICH_REASON_FOR_DELETE;
                    finInvoiceCusTrxMpgListObj = finInvoiceCusHdrObj.FIN_INVOICE_CUS_TRX_MPG.ToList();
                    foreach (FIN_INVOICE_CUS_TRX_MPG FIN_INVOICE_CUS_TRX_MPG_Obj in finInvoiceCusTrxMpgListObj)
                    {
                        if (FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_PK > 0)
                        {
                            OLD_FIN_INVOICE_CUS_TRX_MPG_Obj = currentEntity.FIN_INVOICE_CUS_TRX_MPG.SingleOrDefault(v => v.ICM_PK == FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_PK);
                            if (OLD_FIN_INVOICE_CUS_TRX_MPG_Obj != null)
                            {
                                if (OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0)
                                {
                                    salOrderHdrObj = currentEntity.SAL_ORDER_HDR.SingleOrDefault(sah => sah.SOH_PK == OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_SO_HDR);
                                    if (salOrderHdrObj != null)
                                    {
                                        if (finInvoiceCusHdrObj.ICH_TYPE != (byte)SalesInvoiceType.Proforma)
                                            salOrderHdrObj.SOH_AMT_INVOICED -= OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_AMOUNT;
                                        else
                                            salOrderHdrObj.SOH_AMT_PINVOICED -= OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_AMOUNT;
                                    }
                                    OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_AMOUNT = FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_AMOUNT;
                                    OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_ACTIVE = FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_ACTIVE;
                                    OLD_FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_SO_HDR = FIN_INVOICE_CUS_TRX_MPG_Obj.ICM_SO_HDR;
                                }
                            }
                        }
                    }
                    retval = finInvoiceCusHdrObj.ICH_PK;
                }
                else
                {
                    // throws exception already deleted or modified by other user
                    throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
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

        public List<FIN_INVOICE_VND_HDR> GetInvoiceVndHdrByPK(FIN_INVOICE_VND_HDR objInvDetails)
        {
            List<FIN_INVOICE_VND_HDR> InvoiceHdrList = null;
            IQueryable<FIN_INVOICE_VND_HDR> FIN_INVOICE_VND_HDRQuery;
            try
            {

                FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_VND_HDR
                                            where inv.IVH_ACTIVE == objInvDetails.IVH_ACTIVE
                                            && inv.IVH_DEL_STATUS == objInvDetails.IVH_DEL_STATUS
                                              && inv.IVH_PK == objInvDetails.IVH_PK
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

        public List<FIN_INVOICE_VND_DTL> GetInvoiceVndDtlByPK(FIN_INVOICE_VND_DTL objInvDetails)
        {
            List<FIN_INVOICE_VND_DTL> InvoiceHdrList = null;
            IQueryable<FIN_INVOICE_VND_DTL> FIN_INVOICE_VND_HDRQuery;
            try
            {

                FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_VND_DTL
                                            where inv.VID_PK == objInvDetails.VID_PK
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

        public List<FIN_INVOICE_CUS_DTL> GetInvoiceCusDtlByPK(FIN_INVOICE_CUS_DTL objInvDetails)
        {
            List<FIN_INVOICE_CUS_DTL> InvoiceHdrList = null;
            IQueryable<FIN_INVOICE_CUS_DTL> FIN_INVOICE_VND_HDRQuery;
            try
            {

                FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_CUS_DTL
                                            where inv.CID_PK == objInvDetails.CID_PK
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

        /// <summary>
        /// Get Invoice Received Amount Details
        /// </summary>
        /// <param name="InvoicePk"></param>
        /// <returns></returns>
        public DataTable GetInvCusReceivedAmntDetails(long InvoicePk)
        {
            DataTable dtResult = null;
            try
            {

                var Receipt = from rcpt in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG
                              where rcpt.RCM_INVOICE_HDR == InvoicePk && rcpt.FIN_RECEIPT_CUS_HDR.RCH_STATUS != 0 && rcpt.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 && rcpt.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                              select new
                              {
                                  TRX_NO = rcpt.FIN_RECEIPT_CUS_HDR.RCH_NO,
                                  DATE = rcpt.FIN_RECEIPT_CUS_HDR.RCH_DATE,
                                  AMOUNT = rcpt.RCM_RCVD_AMOUNT,
                                  RECEIPT = ""
                              };
                //var DebitCredit = from drcr in this.currentEntity.FIN_CRDR_NOTE_MPG
                //                  where drcr.CDM_INVOICE_CUS_HDR == InvoicePk && drcr.FIN_CRDR_NOTE_HDR.CDH_TYPE == (byte)DebitCreditModeEnum.CREDIT
                //                  && drcr.FIN_CRDR_NOTE_HDR.CDH_STATUS != 0 && drcr.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED == false
                //                  select new
                //                  {
                //                      TRX_NO = drcr.FIN_CRDR_NOTE_HDR.CDH_NO,
                //                      DATE = drcr.FIN_CRDR_NOTE_HDR.CDH_DATE,
                //                      AMOUNT = drcr.CDM_AMOUNT,
                //                      RECEIPT = ""
                //                  };

                var DebitCredit = from drcr in this.currentEntity.FIN_RECEIPT_CUS_ALCN_DTL
                                  where drcr.RAD_ALCN_CDH != null && drcr.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePk && drcr.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_STATUS != 0
                                  && drcr.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED == false
                                  && drcr.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                  select new
                                  {
                                      TRX_NO = drcr.FIN_CRDR_NOTE_HDR.CDH_NO,
                                      DATE = drcr.FIN_CRDR_NOTE_HDR.CDH_DATE,
                                      AMOUNT = drcr.RAD_AMOUNT,
                                      RECEIPT = drcr.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_NO
                                  };

                var AdvDeduct = from adv in this.currentEntity.FIN_INVOICE_CUS_ADV_DED_DTL
                                where adv.IAD_INVOICE_HDR == InvoicePk
                                where adv.FIN_INVOICE_CUS_HDR1.ICH_TYPE != (byte)SalesInvoiceType.Domestic
                                select new
                                {
                                    TRX_NO = adv.FIN_INVOICE_CUS_HDR.ICH_NO,
                                    DATE = adv.FIN_INVOICE_CUS_HDR.ICH_DATE,
                                    AMOUNT = adv.IAD_AMOUNT,//- (adv.IAD_ADJUST_AMOUNT + adv.IAD_OTHER_AMOUNT + adv.IAD_TAX_AMOUNT),
                                    RECEIPT = adv.FIN_RECEIPT_CUS_HDR.RCH_NO
                                };
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

        public bool CheckReceiptCreated(FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj)
        {
            bool result = false;
            try
            {

                var receiptlist = (from inv in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG
                                   where inv.RCM_INVOICE_HDR == finInvoiceCusHdrObj.ICH_PK && inv.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 && inv.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                   select inv);


                if (receiptlist != null && receiptlist.Count() > 0)
                {
                    result = true;
                }
                return result;
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

        public List<FIN_INVOICE_CUS_HDR> GetInvoiceCusHdrByPKOnly(FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj)
        {
            List<FIN_INVOICE_CUS_HDR> InvoiceHdrList = null;
            IQueryable<FIN_INVOICE_CUS_HDR> FIN_INVOICE_VND_HDRQuery;
            try
            {
                FIN_INVOICE_VND_HDRQuery = (from inv in this.currentEntity.FIN_INVOICE_CUS_HDR
                                            where inv.ICH_PK == (finInvoiceCusHdrObj.ICH_PK > 0 ? finInvoiceCusHdrObj.ICH_PK : inv.ICH_PK)
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

        public bool IsReceiptNotCreated(long InvPk)
        {
            List<FIN_RECEIPT_CUS_TRX_MPG> InvoiceHdrList = null;
            try
            {
                bool blnResult = false;
                InvoiceHdrList = (from c in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG
                                  where c.RCM_INVOICE_HDR == InvPk
                                  && c.FIN_RECEIPT_CUS_HDR.RCH_HAS_JRNL_ENTRY == true
                                  && c.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                  select c).ToList();
                if (InvoiceHdrList == null || InvoiceHdrList.Count() == 0)
                {
                    blnResult = true;
                }
                return blnResult;
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
