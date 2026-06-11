using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;
using System.Data.Objects;
using BusinessObject.CommonManagement;

namespace ERPManager
{
    public class FinTrxManager : IFinTrxManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Currency Master Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public FinTrxManager(ERPEntities currentEntity)
        {
            try
            {
                string EntityTimeout = System.Configuration.ConfigurationManager.AppSettings["EntityTimeout"];
                int Timeout = 600;
                this.currentEntity = currentEntity;
                if (!string.IsNullOrEmpty(EntityTimeout))
                {
                    Int32.TryParse(EntityTimeout, out Timeout);
                }
                this.currentEntity.CommandTimeout = Convert.ToInt32(Timeout);
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public long SaveFinTrx(List<FIN_TRX> finTrxList)
        {
            //Holds save status
            long retval;

            List<FIN_TRX_COC_DTL> FIN_TRX_COC_DTL_List;
            //FIN_TRX oldFinTrxObj;
            FinCoaMstManager FinCoaMstManagerDrAccObj = new FinCoaMstManager(this.currentEntity);

            long? maxFinTrxPk;
            try
            {
                //Set save status zero,save failed
                retval = 0;

                if (finTrxList.Count > 0)
                {
                    long hdrPK = finTrxList[0].FTR_TRX_HDR;
                    IQueryable<FIN_TRX> oldFIN_TRX = this.currentEntity.FIN_TRX.Where(mpg => mpg.FTR_TRX_HDR == hdrPK);

                    foreach (FIN_TRX oldMpg in oldFIN_TRX)
                    {
                        //if (oldMpg.FTR_ACCOUNT.HasValue)
                        //{
                        //    if (oldMpg.FTR_DR_AMT_BC > 0)
                        //    {
                        //        long finCoaVen = FinCoaMstManagerDrAccObj.FinCoaBalanceSave((int)(oldMpg.FTR_ACCOUNT), oldMpg.FTR_DR_AMT_BC * -1);
                        //    }
                        //    else
                        //    {
                        //        long finCoaVen = FinCoaMstManagerDrAccObj.FinCoaBalanceSave((int)(oldMpg.FTR_ACCOUNT), oldMpg.FTR_CR_AMT_BC);
                        //    }

                        //}

                        #region Delete Cost Centers
                        if (oldMpg.FIN_TRX_COC_DTL != null && oldMpg.FIN_TRX_COC_DTL.Count > 0)
                        {
                            IQueryable<FIN_TRX_COC_DTL> oldFIN_TRX_COC_DTL = this.currentEntity.FIN_TRX_COC_DTL.Where(mpg => mpg.FTD_FTR_PK == oldMpg.FTR_PK);
                            if (oldFIN_TRX_COC_DTL != null && oldFIN_TRX_COC_DTL.Count() > 0)
                            {
                                foreach (FIN_TRX_COC_DTL objCostCenter in oldFIN_TRX_COC_DTL)
                                {
                                    this.currentEntity.FIN_TRX_COC_DTL.DeleteObject(objCostCenter);
                                }
                            }
                        }
                        #endregion

                        this.currentEntity.FIN_TRX.DeleteObject(oldMpg);

                    }
                }

                // Gets last FIN_TRX pk
                maxFinTrxPk = this.currentEntity.FIN_TRX.Max(ftr => (long?)ftr.FTR_PK);

                // Sets return value as next FIN_TRX pk
                retval = Convert.ToInt64((maxFinTrxPk.HasValue ? maxFinTrxPk.Value : 1));

                // Gets last FIN_TRX_COC_DTL pk
                long? maxFinTrxCocPk = this.currentEntity.FIN_TRX_COC_DTL.Max(ftr => (long?)ftr.FTD_PK);
                long? maxFinCocPk = Convert.ToInt64((maxFinTrxCocPk.HasValue ? maxFinTrxCocPk.Value : 1));
                //Iterate through Fin Trx list for save
                foreach (FIN_TRX finTrxObj in finTrxList)
                {

                    FIN_TRX_COC_DTL_List = finTrxObj.FIN_TRX_COC_DTL.ToList();
                    finTrxObj.FIN_TRX_COC_DTL.Clear();

                    // Sets FIN_TRX created date time as current date time
                    finTrxObj.FTR_CRTD_DT = DateTime.Now;

                    // Sets FIN_TRX modified date time as current date time
                    finTrxObj.FTR_MOD_DT = DateTime.Now;

                    if (finTrxObj.FTR_PK == 0)
                    {
                        retval++;
                        // Sets next FIN_TRX pk
                        finTrxObj.FTR_PK = retval;


                        finTrxObj.FTR_EXCHG_RATE_YE = finTrxObj.FTR_EXCHG_RATE;

                        // Add new FIN_TRX to the db context
                        this.currentEntity.FIN_TRX.AddObject(finTrxObj);

                        #region Save Cost Center Details
                        if (FIN_TRX_COC_DTL_List != null && FIN_TRX_COC_DTL_List.Count > 0)
                        {
                            FIN_TRX_COC_DTL_List.ForEach(dtl => dtl.FTD_FTR_PK = retval);
                            SaveFinTrxCostCenter(FIN_TRX_COC_DTL_List, ref maxFinCocPk);

                        }
                        #endregion
                    }


                    //if (finTrxObj.FTR_ACCOUNT.HasValue)
                    //{

                    //    if (finTrxObj.FTR_DR_AMT_BC > 0)
                    //    {
                    //        long finCoaVen = FinCoaMstManagerDrAccObj.FinCoaBalanceSave((int)(finTrxObj.FTR_ACCOUNT), finTrxObj.FTR_DR_AMT_BC);
                    //    }
                    //    else
                    //    {
                    //        long finCoaVen = FinCoaMstManagerDrAccObj.FinCoaBalanceSave((int)(finTrxObj.FTR_ACCOUNT), finTrxObj.FTR_CR_AMT_BC * -1);
                    //    }
                    //}


                    /*else
                    {
                        // updating FIN_TRX
                        // Get current FIN_TRX using FIN_TRX pk and last modified date time,used for concurrency checking
                        oldFinTrxObj = currentEntity.FIN_TRX.SingleOrDefault(sah => sah.FTR_PK == finTrxObj.FTR_PK && sah.FTR_MOD_DT == finTrxObj.FTR_MOD_DT);
                        // If oldFinTrxObj is null then,anyone modified or deleted the record
                        if (oldFinTrxObj != null)
                        {
                            // Update FIN_TRX
                            oldFinTrxObj.FTR_VENDOR = finTrxObj.FTR_VENDOR;
                            oldFinTrxObj.FTR_VENDOR_ACCOUNT = finTrxObj.FTR_VENDOR_ACCOUNT;
                            oldFinTrxObj.FTR_MODE = finTrxObj.FTR_MODE;
                            oldFinTrxObj.FTR_BANK = finTrxObj.FTR_BANK;
                            oldFinTrxObj.FTR_BRANCH = finTrxObj.FTR_BRANCH;
                            oldFinTrxObj.FTR_INSTRUMENT = finTrxObj.FTR_INSTRUMENT;
                            oldFinTrxObj.FTR_BANK_CASH_ACCOUNT = finTrxObj.FTR_BANK_CASH_ACCOUNT;
                            oldFinTrxObj.FTR_CURRENCY = finTrxObj.FTR_CURRENCY;
                            oldFinTrxObj.FTR_PAID_AMOUNT = finTrxObj.FTR_PAID_AMOUNT;
                            oldFinTrxObj.FTR_REMARKS = finTrxObj.FTR_REMARKS;
                            oldFinTrxObj.FTR_ACTIVE = finTrxObj.FTR_ACTIVE;
                            oldFinTrxObj.FTR_MOD_BY = finTrxObj.FTR_MOD_BY;
                            oldFinTrxObj.FTR_MOD_DT = DateTime.Now;

                            // Sets return value as FIN_TRX pk
                            retval = finTrxObj.FTR_PK;
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                           //throw new OptimisticConcurrencyException(gComsManagerRes.EditConcurrencyException);
                        }
                    }*/

                }


                //return FIN_TRX pk
                return retval;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
            }

        }

        /// <summary>
        /// Methos to save Cost Center Details
        /// </summary>
        /// <param name="finTrxList"></param>
        /// <returns></returns>
        private long SaveFinTrxCostCenter(List<FIN_TRX_COC_DTL> finTrxList, ref long? maxFinTrxCocPk)
        {
            //Holds save status
            long retval;
            //long? maxFinTrxPk;
            try
            {
                //Set save status zero,save failed
                retval = 0;
                if (finTrxList.Count > 0)
                {
                    long hdrPK = finTrxList[0].FTD_FTR_PK;
                    IQueryable<FIN_TRX_COC_DTL> oldFIN_TRX_COC_DTL = this.currentEntity.FIN_TRX_COC_DTL.Where(mpg => mpg.FTD_FTR_PK == hdrPK);
                    if (oldFIN_TRX_COC_DTL != null && oldFIN_TRX_COC_DTL.Count() > 0)
                    {
                        foreach (FIN_TRX_COC_DTL oldMpg in oldFIN_TRX_COC_DTL)
                        {
                            this.currentEntity.FIN_TRX_COC_DTL.DeleteObject(oldMpg);
                        }
                    }
                }

                if (!maxFinTrxCocPk.HasValue)
                {
                    maxFinTrxCocPk = this.currentEntity.FIN_TRX_COC_DTL.Max(ftr => (long?)ftr.FTD_PK);
                    retval = Convert.ToInt64((maxFinTrxCocPk.HasValue ? maxFinTrxCocPk.Value : 1));
                }
                else
                    retval = maxFinTrxCocPk.Value;

                //Iterate through Fin Trx list for save
                foreach (FIN_TRX_COC_DTL finTrxObj in finTrxList)
                {
                    if (finTrxObj.FTD_PK == 0)
                    {
                        retval++;
                        // Sets next FIN_TRX pk
                        finTrxObj.FTD_PK = retval;
                        // Add new FIN_TRX to the db context
                        this.currentEntity.FIN_TRX_COC_DTL.AddObject(finTrxObj);

                    }

                }
                //return FIN_TRX pk
                maxFinTrxCocPk = retval;
                return retval;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
            }
        }

        public int UpdateBankReconciliation(List<FIN_TRX> finTrxList)
        {
            List<FIN_TRX_HDR> oldFinTrxHdrlist;
            //Holds save status
            int retval;
            FIN_TRX oldFinTrxObj;

            //FIN_TRX oldFinTrxObj;
            FinCoaMstManager FinCoaMstManagerDrAccObj = new FinCoaMstManager(this.currentEntity);
            try
            {
                //Set save status zero,save failed
                retval = 0;

                if (finTrxList.Count > 0)
                {
                    foreach (FIN_TRX finTrxObj in finTrxList)
                    {
                        oldFinTrxObj = currentEntity.FIN_TRX.SingleOrDefault(sah => sah.FTR_PK == finTrxObj.FTR_PK);    // && sah.FTR_MOD_DT == finTrxObj.FTR_MOD_DT);
                        if (oldFinTrxObj != null)
                        {
                            oldFinTrxObj.FTR_IS_RECONCILED = finTrxObj.FTR_IS_RECONCILED;
                            oldFinTrxObj.FTR_CLEAR_DATE = finTrxObj.FTR_CLEAR_DATE;

                            oldFinTrxObj.FTR_MOD_BY = finTrxObj.FTR_MOD_BY;
                            oldFinTrxObj.FTR_MOD_DT = DateTime.Now;

                            retval += 1;

                            //update header table
                            if (finTrxObj.FTR_CLEAR_DATE != null)
                            {
                                oldFinTrxHdrlist = currentEntity.FIN_TRX_HDR.Where(c => c.FTH_PK == oldFinTrxObj.FTR_TRX_HDR).ToList();
                                foreach (FIN_TRX_HDR item in oldFinTrxHdrlist)
                                {
                                    //item.FTH_DATE = finTrxObj.FTR_CLEAR_DATE;

                                    #region update pdc voucher with pdc cleared voucher for reconcillation
                                    try
                                    {
                                        string FTH_REF_TYPE = string.Empty;
                                        string FTR_TYPE = string.Empty;
                                        if (item.FTH_REF_TYPE == ApplicationType.PPCCJ)
                                        {
                                            FTH_REF_TYPE = ApplicationType.VPJ;
                                            FTR_TYPE = AccountSubType.PPC.ToString();
                                        }
                                        else if (item.FTH_REF_TYPE == ApplicationType.PDCCJ)
                                        {
                                            FTH_REF_TYPE = ApplicationType.CRJ;
                                            FTR_TYPE = AccountSubType.PDC.ToString();
                                        }

                                        List<FIN_TRX_HDR> fintrxlist = currentEntity.FIN_TRX_HDR.Where(c => c.FTH_REF_PK == item.FTH_REF_PK && c.FTH_REF_NO == item.FTH_REF_NO && c.FTH_REF_TYPE == FTH_REF_TYPE && !c.FTH_IS_DELETED).ToList();
                                        foreach (FIN_TRX_HDR finhdr in fintrxlist)
                                        {
                                            List<FIN_TRX> trxlist = finhdr.FIN_TRX.Where(c => c.FTR_TYPE == FTR_TYPE).ToList();
                                            foreach (FIN_TRX TrxObj in trxlist)
                                            {
                                                FIN_TRX objFinTrx = currentEntity.FIN_TRX.SingleOrDefault(c => c.FTR_PK == TrxObj.FTR_PK);
                                                if (objFinTrx != null)
                                                {
                                                    objFinTrx.FTR_IS_RECONCILED = finTrxObj.FTR_IS_RECONCILED;
                                                    objFinTrx.FTR_CLEAR_DATE = finTrxObj.FTR_CLEAR_DATE;
                                                    objFinTrx.FTR_MOD_BY = finTrxObj.FTR_MOD_BY;
                                                    objFinTrx.FTR_MOD_DT = DateTime.Now;
                                                }
                                            }
                                            //update header
                                            FIN_TRX_HDR objTrxHdr = currentEntity.FIN_TRX_HDR.SingleOrDefault(c => c.FTH_PK == finhdr.FTH_PK);
                                            //if (objTrxHdr != null)
                                            //{
                                            //    objTrxHdr.FTH_DATE = finTrxObj.FTR_CLEAR_DATE;
                                            //}

                                        }
                                    }
                                    catch { }
                                    #endregion

                                }
                            }

                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }

                    }
                }

                //return FIN_TRX pk
                return retval;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Get Page Count
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<FIN_TRX> GetAccoutReceivables(long VendorID, ServiceUtility utilityObj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<FIN_TRX> GetAccoutPayables(long VendorAccID, ServiceUtility utilityObj)
        {
            IQueryable<FIN_TRX> FIN_TRX_List_Qry;
            List<FIN_TRX> FIN_TRX_List_Obj = null; // = new List<FinTrxData>();
            int pageSize;
            int totalCount;
            decimal OB = 0;
            try
            {
                //pageSize = Convert.ToInt32(utilityObj.PageSize);
                //FIN_TRX_List_Qry = (from t in this.currentEntity.FIN_TRX
                //                    where t.FTR_DATE >= (utilityObj.FilterDate == null ? t.FTR_DATE : utilityObj.FilterDate)
                //                        && t.FTR_DATE <= (utilityObj.FilterToDate == null ? t.FTR_DATE : utilityObj.FilterToDate)
                //                        && t.FTR_ACCOUNT == (VendorAccID > 0 ? VendorAccID : t.FTR_ACCOUNT)
                //                    select t
                //                    );

                ////Get total row count
                //totalCount = FIN_TRX_List_Qry.Count();

                ////Set page size one if not given
                //utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize-1;

                ////Filter Query
                ////FIN_TRX_List_Qry = FilterEntity(FIN_TRX_List_Obj, FIN_TRX_List_Qry, utilityObj);

                //FIN_TRX_List_Obj = FIN_TRX_List_Qry.SortRecords<FIN_TRX>(utilityObj).ToList();

                //// Opening Balance Calculation
                //var OBList = (from t in this.currentEntity.FIN_TRX
                //              where t.FTR_DATE < utilityObj.FilterDate
                //              select new { FTR_DR_AMT_BC = t.FTR_DR_AMT_BC, FTR_CR_AMT_BC = t.FTR_CR_AMT_BC }
                //    );

                //if (OBList.Count() > 0)
                //    OB= OBList.Sum(w => w.FTR_DR_AMT_BC - w.FTR_CR_AMT_BC);

                //FIN_TRX FIN_TRX_Obj = new FIN_TRX();

                //FIN_TRX_Obj.FTR_PK = -1;
                //FIN_TRX_Obj.FTR_DATE =Convert.ToDateTime(utilityObj.FilterDate);
                //FIN_TRX_Obj.FTR_REF_TYPE = "Opening Balance";

                //if (OB > 0)
                //    FIN_TRX_Obj.FTR_DR_AMT_BC = OB;
                //else
                //    FIN_TRX_Obj.FTR_CR_AMT_BC = OB;


                //FIN_TRX_List_Obj.Add(FIN_TRX_Obj);

                //if (FIN_TRX_List_Obj.Count > 0)
                //    utilityObj.TotalRecords  = totalCount / utilityObj.PageSize;

                ////FIN_TRX_List_Obj.Sort();
                //return FIN_TRX_List_Obj.OrderBy(xx => xx.FTR_DATE).ThenBy(vvv => vvv.FTR_PK).ToList();
                ////return FIN_TRX_List_Obj.OrderBy(xx => xx.FTR_DATE).ToList();


                return FIN_TRX_List_Obj;
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
                FIN_TRX_List_Obj = null;
            }

        }

        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<FIN_TRX> GetAccoutPayablesList(long ID, string Type, ServiceUtility utilityObj)
        {
            IQueryable<FIN_TRX> FIN_TRX_List_Qry;
            List<FIN_TRX> FIN_TRX_List_Obj = null; // = new List<FinTrxData>();
            int pageSize;
            int totalCount;
            decimal OB = 0;
            try
            {
                pageSize = Convert.ToInt32(utilityObj.PageSize);
                FIN_TRX_List_Qry = (from t in this.currentEntity.FIN_TRX
                                    where t.FIN_TRX_HDR.FTH_DATE >= (utilityObj.FilterDate == null ? t.FIN_TRX_HDR.FTH_DATE : utilityObj.FilterDate)
                                        && t.FIN_TRX_HDR.FTH_DATE <= (utilityObj.FilterToDate == null ? t.FIN_TRX_HDR.FTH_DATE : utilityObj.FilterToDate)
                                        && t.FTR_TYPE_PK == (ID > 0 ? (int)ID : t.FTR_TYPE_PK)
                                        //&& t.FTR_TYPE == (!string.IsNullOrEmpty(Type) ? Type : t.FTR_TYPE)
                                        && (string.IsNullOrEmpty(Type) ?
                                            true
                                            : (Type == ApplicationType.AR ? (t.FTR_TYPE == ApplicationType.AR || t.FTR_TYPE == ApplicationType.ADR)
                                            : (t.FTR_TYPE == ApplicationType.AP || t.FTR_TYPE == ApplicationType.ADP))
                                            )
                                        //&& t.FIN_TRX_HDR.FTH_STATUS==2
                                        && t.FIN_TRX_HDR.FTH_IS_DELETED == false
                                        && t.FIN_TRX_HDR.FTH_IS_JRNLD == true
                                    select t
                                    );

                //Get total row count
                totalCount = FIN_TRX_List_Qry.Count();

                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize - 1;

                //Filter Query
                //FIN_TRX_List_Qry = FilterEntity(FIN_TRX_List_Obj, FIN_TRX_List_Qry, utilityObj);

                //FIN_TRX_List_Obj = FIN_TRX_List_Qry.SortRecords<FIN_TRX>(utilityObj).ToList();

                FIN_TRX_List_Obj = FIN_TRX_List_Qry.ToList();

                // Opening Balance Calculation
                var OBList = (from t in this.currentEntity.FIN_TRX
                              where t.FIN_TRX_HDR.FTH_DATE < utilityObj.FilterDate
                                    && t.FTR_TYPE_PK == (ID > 0 ? (int)ID : t.FTR_TYPE_PK)
                                    && (string.IsNullOrEmpty(Type) ?
                                            true
                                            : (Type == ApplicationType.AR ? (t.FTR_TYPE == ApplicationType.AR || t.FTR_TYPE == ApplicationType.ADR)
                                            : (t.FTR_TYPE == ApplicationType.AP || t.FTR_TYPE == ApplicationType.ADP)))
                                     && t.FIN_TRX_HDR.FTH_IS_DELETED == false
                                     && t.FIN_TRX_HDR.FTH_IS_JRNLD == true

                              select new { FTR_DR_AMT_BC = t.FTR_DR_AMT_BC, FTR_CR_AMT_BC = t.FTR_CR_AMT_BC }
                    );

                if (OBList.Count() > 0)
                    OB = OBList.Sum(w => w.FTR_DR_AMT_BC - w.FTR_CR_AMT_BC);

                FIN_TRX FIN_TRX_Obj = new FIN_TRX();
                FIN_TRX_HDR FIN_TRX_HDR_Obj = new FIN_TRX_HDR();

                FIN_TRX_Obj.FTR_PK = -1;
                //FIN_TRX_Obj.FIN_TRX_HDR.FTH_DATE = Convert.ToDateTime(utilityObj.FilterDate);   //  FTR_DATE 
                //FIN_TRX_Obj.FIN_TRX_HDR.FTH_REF_TYPE = "Opening Balance";   //FTR_REF_TYPE
                FIN_TRX_HDR_Obj.FTH_DATE = Convert.ToDateTime(utilityObj.FilterDate);
                FIN_TRX_HDR_Obj.FTH_REF_TYPE = "Opening Balance";   //FTR_REF_TYPE

                FIN_TRX_Obj.FIN_TRX_HDR = FIN_TRX_HDR_Obj;

                if (OB > 0)
                    FIN_TRX_Obj.FTR_DR_AMT_BC = OB;
                else
                    FIN_TRX_Obj.FTR_CR_AMT_BC = OB * -1;


                FIN_TRX_List_Obj.Add(FIN_TRX_Obj);

                if (FIN_TRX_List_Obj.Count > 0)
                    utilityObj.TotalRecords = totalCount / utilityObj.PageSize;

                //FIN_TRX_List_Obj.Sort();
                if (utilityObj.SortBy == "FTH_DATE")
                {
                    if (utilityObj.SortDirection == "asc")
                    {
                        return FIN_TRX_List_Obj.OrderBy(xx => xx.FIN_TRX_HDR.FTH_DATE).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                    else
                    {
                        return FIN_TRX_List_Obj.OrderByDescending(xx => xx.FIN_TRX_HDR.FTH_DATE).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                }
                else if (utilityObj.SortBy == "FTH_REF_NO")
                {
                    if (utilityObj.SortDirection == "asc")
                    {
                        return FIN_TRX_List_Obj.OrderBy(xx => xx.FIN_TRX_HDR.FTH_REF_NO).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                    else
                    {
                        return FIN_TRX_List_Obj.OrderByDescending(xx => xx.FIN_TRX_HDR.FTH_REF_NO).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                }
                else if (utilityObj.SortBy == "FTH_REF_TYPE")
                {
                    if (utilityObj.SortDirection == "asc")
                    {
                        return FIN_TRX_List_Obj.OrderBy(xx => xx.FIN_TRX_HDR.FTH_REF_TYPE).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                    else
                    {
                        return FIN_TRX_List_Obj.OrderByDescending(xx => xx.FIN_TRX_HDR.FTH_REF_TYPE).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                }
                else if (utilityObj.SortBy == "FTR_DR_AMT_BC")
                {
                    if (utilityObj.SortDirection == "asc")
                    {
                        return FIN_TRX_List_Obj.OrderBy(xx => xx.FTR_DR_AMT_BC).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                    else
                    {
                        return FIN_TRX_List_Obj.OrderByDescending(xx => xx.FTR_DR_AMT_BC).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                }
                else if (utilityObj.SortBy == "FTR_CR_AMT_BC")
                {
                    if (utilityObj.SortDirection == "asc")
                    {
                        return FIN_TRX_List_Obj.OrderBy(xx => xx.FTR_CR_AMT_BC).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                    else
                    {
                        return FIN_TRX_List_Obj.OrderByDescending(xx => xx.FTR_CR_AMT_BC).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                }
                else
                {
                    if (utilityObj.SortDirection == "asc")
                    {
                        return FIN_TRX_List_Obj.OrderBy(xx => xx.FIN_TRX_HDR.FTH_DATE).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                    else
                    {
                        return FIN_TRX_List_Obj.OrderByDescending(xx => xx.FIN_TRX_HDR.FTH_DATE).ThenBy(vvv => vvv.FTR_PK).ToList();
                    }
                }
                //return FIN_TRX_List_Obj.OrderBy(xx => xx.FTR_DATE).ToList();


                //return FIN_TRX_List_Obj;
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
                FIN_TRX_List_Obj = null;
            }

        }

        public int GetAccoutPayablesCount(long VendorID, ServiceUtility utilityObj)
        {
            throw new NotImplementedException();
        }

        public List<FIN_TRX> GetBankReconcileList(FIN_TRX finTrxObj, int isReconciled, ServiceUtility utilityObj, ref decimal  TotalDebit, ref decimal TotalCredit)
        {
            IQueryable<FIN_TRX> FIN_TRX_List_Qry;
            List<FIN_TRX> FIN_TRX_List_Obj = null; // = new List<FinTrxData>();
            int pageSize;
            int totalCount;
            decimal OB = 0;
            try
            {
                pageSize = Convert.ToInt32(utilityObj.PageSize);
                FIN_TRX_List_Qry = (from t in this.currentEntity.FIN_TRX
                                    where t.FIN_TRX_HDR.FTH_DATE >= (utilityObj.FilterDate == null ? t.FIN_TRX_HDR.FTH_DATE : utilityObj.FilterDate)
                                        && t.FIN_TRX_HDR.FTH_DATE <= (utilityObj.FilterToDate == null ? t.FIN_TRX_HDR.FTH_DATE : utilityObj.FilterToDate)
                                        && t.FTR_TYPE_PK == (finTrxObj.FTR_TYPE_PK > 0 ? finTrxObj.FTR_TYPE_PK : t.FTR_TYPE_PK)
                                        && t.FTR_TYPE == (!string.IsNullOrEmpty(finTrxObj.FTR_TYPE) ? finTrxObj.FTR_TYPE : t.FTR_TYPE)
                                        && t.FTR_ACCOUNT == (finTrxObj.FTR_ACCOUNT > 0 ? finTrxObj.FTR_ACCOUNT : t.FTR_ACCOUNT)
                                        //&& t.FIN_TRX_HDR.FTH_STATUS == 2 
                                        && t.FIN_TRX_HDR.FTH_IS_JRNLD == true
                                        //&& t.FTR_IS_RECONCILED == finTrxObj.FTR_IS_RECONCILED
                                        && (!string.IsNullOrEmpty(finTrxObj.FTR_INSTR_NO) ? t.FTR_INSTR_NO.StartsWith(finTrxObj.FTR_INSTR_NO) : true)
                                        && t.FIN_TRX_HDR.FTH_IS_DELETED == false
                                    select t
                                    );

                if (isReconciled == 0)
                    FIN_TRX_List_Qry = FIN_TRX_List_Qry.Where(a => a.FTR_IS_RECONCILED == false);
                else if (isReconciled == 1)
                    FIN_TRX_List_Qry = FIN_TRX_List_Qry.Where(a => a.FTR_IS_RECONCILED == true);

                //Get total row count
                totalCount = FIN_TRX_List_Qry.Count();

                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize - 1;

                //Filter Query
                //FIN_TRX_List_Qry = FilterEntity(FIN_TRX_List_Obj, FIN_TRX_List_Qry, utilityObj);

                FIN_TRX_List_Obj = FIN_TRX_List_Qry.SortRecords<FIN_TRX>(utilityObj).ToList();

                // Opening Balance Calculation
                var OBList = (from t in this.currentEntity.FIN_TRX
                              where t.FIN_TRX_HDR.FTH_DATE <= utilityObj.FilterToDate
                                    && t.FTR_TYPE_PK == (finTrxObj.FTR_TYPE_PK > 0 ? finTrxObj.FTR_TYPE_PK : t.FTR_TYPE_PK)
                                    && t.FTR_TYPE == (!string.IsNullOrEmpty(finTrxObj.FTR_TYPE) ? finTrxObj.FTR_TYPE : t.FTR_TYPE)
                                    //&& (!string.IsNullOrEmpty(finTrxObj.FTR_TYPE) ? (t.FIN_COA_MST.FIN_COA_SUB_TYPE_CFG.CST_CODE == finTrxObj.FTR_TYPE) : true)
                                     //&& t.FIN_COA_MST.FIN_COA_SUB_TYPE_CFG.CST_CODE == (!string.IsNullOrEmpty(finTrxObj.FTR_TYPE) ? finTrxObj.FTR_TYPE : t.FIN_COA_MST.FIN_COA_SUB_TYPE_CFG.CST_CODE)
                                    && t.FTR_IS_RECONCILED == true
                                    && t.FIN_TRX_HDR.FTH_IS_DELETED == false
                                    && t.FIN_TRX_HDR.FTH_IS_JRNLD == true
                                    && t.FIN_TRX_HDR.FTH_ACTIVE == 1
                              select new { FTR_DR_AMT_BC = t.FTR_DR_AMT_BC, FTR_CR_AMT_BC = t.FTR_CR_AMT_BC }
                    );

                if (OBList.Count() > 0)
                    OB = OBList.Sum(w => w.FTR_DR_AMT_BC - w.FTR_CR_AMT_BC);
                else
                    OB = 0;

                //OBList = (from t in this.currentEntity.FIN_TRX
                //          where t.FIN_TRX_HDR.FTH_DATE >= (utilityObj.FilterDate == null ? t.FIN_TRX_HDR.FTH_DATE : utilityObj.FilterDate)
                //                && t.FIN_TRX_HDR.FTH_DATE <= (utilityObj.FilterToDate == null ? t.FIN_TRX_HDR.FTH_DATE : utilityObj.FilterToDate)
                //                && t.FTR_TYPE_PK == (finTrxObj.FTR_TYPE_PK > 0 ? finTrxObj.FTR_TYPE_PK : t.FTR_TYPE_PK)
                //                && t.FTR_TYPE == (!string.IsNullOrEmpty(finTrxObj.FTR_TYPE) ? finTrxObj.FTR_TYPE : t.FTR_TYPE)
                //                && t.FTR_IS_RECONCILED == true
                //          select new { FTR_DR_AMT_BC = t.FTR_DR_AMT_BC, FTR_CR_AMT_BC = t.FTR_CR_AMT_BC }
                //          );

                //if (OBList.Count() > 0)
                //    OB += OBList.Sum(w => w.FTR_DR_AMT_BC - w.FTR_CR_AMT_BC);

                FIN_TRX FIN_TRX_Obj = new FIN_TRX();
                FIN_TRX_HDR FIN_TRX_HDR_Obj = new FIN_TRX_HDR();

                FIN_TRX_Obj.FTR_PK = -1;
                //FIN_TRX_Obj.FIN_TRX_HDR.FTH_DATE = Convert.ToDateTime(utilityObj.FilterDate);   //  FTR_DATE 
                //FIN_TRX_Obj.FIN_TRX_HDR.FTH_REF_TYPE = "Opening Balance";   //FTR_REF_TYPE
                FIN_TRX_HDR_Obj.FTH_DATE = Convert.ToDateTime(utilityObj.FilterDate);
                FIN_TRX_HDR_Obj.FTH_REF_TYPE = "Opening Balance";   //FTR_REF_TYPE

                FIN_TRX_Obj.FIN_TRX_HDR = FIN_TRX_HDR_Obj;

                if (OB > 0)
                    FIN_TRX_Obj.FTR_DR_AMT_BC = OB;
                else
                    FIN_TRX_Obj.FTR_CR_AMT_BC = OB * -1;


                FIN_TRX_List_Obj.Add(FIN_TRX_Obj);

                if (FIN_TRX_List_Obj.Count > 0)
                    utilityObj.TotalRecords = totalCount / utilityObj.PageSize;

                //Total Debit/Credit Amount 
                var QueryTotal = (from t in this.currentEntity.FIN_TRX
                                    where t.FIN_TRX_HDR.FTH_DATE <= (utilityObj.FilterToDate == null ? t.FIN_TRX_HDR.FTH_DATE : utilityObj.FilterToDate)
                                        && t.FTR_TYPE_PK == (finTrxObj.FTR_TYPE_PK > 0 ? finTrxObj.FTR_TYPE_PK : t.FTR_TYPE_PK)
                                        && t.FTR_TYPE == (!string.IsNullOrEmpty(finTrxObj.FTR_TYPE) ? finTrxObj.FTR_TYPE : t.FTR_TYPE)
                                        && t.FTR_ACCOUNT == (finTrxObj.FTR_ACCOUNT > 0 ? finTrxObj.FTR_ACCOUNT : t.FTR_ACCOUNT)
                                        && t.FIN_TRX_HDR.FTH_IS_DELETED == false
                                        && t.FIN_TRX_HDR.FTH_IS_JRNLD == true
                                        && t.FTR_IS_RECONCILED == false
                                       select new { FTR_DR_AMT_BC = t.FTR_DR_AMT_BC, FTR_CR_AMT_BC = t.FTR_CR_AMT_BC }                                   
                                  );
                if (QueryTotal != null && QueryTotal.Count() > 0)
                {
                    TotalDebit = QueryTotal.Sum(r => r.FTR_DR_AMT_BC);
                    TotalCredit = QueryTotal.Sum(r => r.FTR_CR_AMT_BC);
                }

                //FIN_TRX_List_Obj.Sort();
                return FIN_TRX_List_Obj.OrderBy(xx => xx.FIN_TRX_HDR.FTH_DATE).ThenBy(vvv => vvv.FTR_PK).ToList();
                //return FIN_TRX_List_Obj.OrderBy(xx => xx.FTR_DATE).ToList();


                //return FIN_TRX_List_Obj;
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
                FIN_TRX_List_Obj = null;
            }
        }

        public int GetFinType(string type, int refPK)
        {
            try
            {
                int retValue = 0;
                //List<SPFIN_INV_TYPE_GET_Result> finTypeObj = null;
                //finTypeObj = this.currentEntity.SPFIN_INV_TYPE_GET(type, refPK).ToList();
                retValue = this.currentEntity.SPFIN_INV_TYPE_GET(type, refPK).AsEnumerable().SingleOrDefault().FIN_TYPE;
                //if (finTypeObj != null && finTypeObj.Count > 0)
                //    retValue = finTypeObj.FirstOrDefault() != null ? finTypeObj.FirstOrDefault().FIN_TYPE : 0;
                return retValue;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Exception handler for Delete - Delete Concurrency
            catch (ArgumentNullException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        #endregion

        #region Private Methods
        private static IQueryable<FinTrxData> FilterEntity(FinTrxData finPaymentVndHdrObj, IQueryable<FinTrxData> qry, ServiceUtility utilityObj)
        {
            #region Filtering
            if (finPaymentVndHdrObj.FTR_PK != 0)
                qry = qry.Where(pvh => pvh.FTR_PK == finPaymentVndHdrObj.FTR_PK);

            /*
            if (finPaymentVndHdrObj.PVH_BIZUNIT != -1)
            {
                qry = qry.Where(pvh => pvh.PVH_BIZUNIT == finPaymentVndHdrObj.PVH_BIZUNIT);
            }
            if (finPaymentVndHdrObj.PVH_PK < 1 && finPaymentVndHdrObj.PVH_ACTIVE == 1) //select all active records
                qry = qry.Where(pvh => pvh.PVH_ACTIVE == 1);
            else if (finPaymentVndHdrObj.PVH_PK > 0 && finPaymentVndHdrObj.PVH_ACTIVE == 1) // select all active +(union) having given pk
                qry = qry.Where(pvh => pvh.PVH_ACTIVE == finPaymentVndHdrObj.PVH_ACTIVE || pvh.PVH_PK == finPaymentVndHdrObj.TXG_PK);
            */

            //if (finPaymentVndHdrObj.PVH_VENDOR != 0)
            //    qry = qry.Where(pvh => pvh.PVH_VENDOR == finPaymentVndHdrObj.PVH_VENDOR);

            //// Filter by fault logged Date Range
            //if (utilityObj.FilterDate.HasValue == true && utilityObj.FilterToDate.HasValue == true)
            //{
            //    qry = qry.Where(pvh => pvh.PVH_DATE >= utilityObj.FilterDate && pvh.PVH_DATE <= utilityObj.FilterToDate);
            //}

            #endregion
            return qry;
        }
        #endregion



        public bool CheckForVoucherNoDuplication(int currPK, string voucherNo)
        {
            IQueryable<FIN_TRX_HDR> FIN_TRX_HDR_List_Qry;
            List<FIN_TRX_HDR> FIN_TRX_HDR_List_Obj = null;
            try
            {
                FIN_TRX_HDR_List_Qry = (from t in this.currentEntity.FIN_TRX_HDR
                                        where t.FTH_PK != currPK
                                            && t.FTH_VOUCHER_NO == voucherNo
                                        select t
                                       );
                FIN_TRX_HDR_List_Obj = FIN_TRX_HDR_List_Qry.ToList();
                if (FIN_TRX_HDR_List_Obj != null && FIN_TRX_HDR_List_Obj.Count > 0)
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
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                // Disposing used objects
                FIN_TRX_HDR_List_Obj = null;
            }
        }



        public List<FIN_PAYMENT_VND_TAX_HDR> GetfinPymntVndTxtHdrList(FIN_PAYMENT_VND_TAX_HDR finTrxHdrObj, ServiceUtility serviceUtilityObj)
        {
            List<FIN_PAYMENT_VND_TAX_HDR> finTrxHdrList = null;
            IQueryable<FIN_PAYMENT_VND_TAX_HDR> fintrxHdrQuery;
            //int pageSize;
            //int totalCount;
            try
            {
                fintrxHdrQuery = from fth in this.currentEntity.FIN_PAYMENT_VND_TAX_HDR where fth.WTH_TAX_INV_NO == finTrxHdrObj.WTH_TAX_INV_NO select fth;
                if (finTrxHdrObj.WTH_TAX_DATE.HasValue)
                    fintrxHdrQuery = from fth in fintrxHdrQuery where fth.WTH_TAX_DATE == finTrxHdrObj.WTH_TAX_DATE select fth;
                // Apply Paging And Sorting for grid Purpose
                finTrxHdrList = fintrxHdrQuery.ToList();

                return finTrxHdrList;
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

        public bool IsRefereceNoEsixt(FIN_TRX_HDR finTrxHdrObj)
        {
            bool IsRefnoExist = false;
            List<FIN_TRX_HDR> finTrxHdrList = null;
            try
            {
                finTrxHdrList = (from fth in this.currentEntity.FIN_TRX_HDR where fth.FTH_PARTY_NAME == finTrxHdrObj.FTH_PARTY_NAME && fth.FTH_REF_NO == finTrxHdrObj.FTH_REF_NO && fth.FTH_PK != finTrxHdrObj.FTH_PK && fth.FTH_REF_TYPE == finTrxHdrObj.FTH_REF_TYPE && fth.FTH_IS_DELETED == false select fth).ToList();
                if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                {
                    IsRefnoExist = true;
                }
                else
                {
                    IsRefnoExist = false;
                }
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
            return IsRefnoExist;
        }



        public bool CheckForReceiptVoucherCreated(FIN_TRX_HDR finTrxHdrObj)
        {
            bool IsRcptExist = false;
            try
            {
                var result = from rcpt in this.currentEntity.FIN_RECEIPT_CUS_TRX_MPG where rcpt.RCM_INVOICE_HDR == finTrxHdrObj.FTH_REF_PK && rcpt.RCM_BOUNCED == 0 && rcpt.FIN_RECEIPT_CUS_HDR.RCH_HAS_JRNL_ENTRY && rcpt.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 select rcpt;
                if (result != null && result.Count() > 0)
                {
                    IsRcptExist = true;
                }
                else
                {
                    IsRcptExist = false;
                }

                return IsRcptExist;
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

        public bool CheckForYearendVoucherCreated(FIN_TRX_HDR finTrxHdrObj)
        {
            bool IsYearendExist = false;
            try
            {
                var result = from fth in this.currentEntity.FIN_TRX_HDR where fth.FTH_REF_NO == finTrxHdrObj.FTH_VOUCHER_NO && fth.FTH_IS_DELETED == false && (fth.FTH_REF_TYPE == ApplicationType.PIJYE || fth.FTH_REF_TYPE == ApplicationType.VPJYE || fth.FTH_REF_TYPE == ApplicationType.SIJYE || fth.FTH_REF_TYPE == ApplicationType.CRJYE || fth.FTH_REF_TYPE == ApplicationType.EIJYE || fth.FTH_REF_TYPE == ApplicationType.PSIJYE || fth.FTH_REF_TYPE == ApplicationType.EIPJYE || fth.FTH_REF_TYPE == ApplicationType.SIPJYE || fth.FTH_REF_TYPE == ApplicationType.MSIJYE || fth.FTH_REF_TYPE == ApplicationType.MSIRJYE) select fth;
                if (result != null && result.Count() > 0)
                {
                    IsYearendExist = true;
                }
                else
                {
                    IsYearendExist = false;
                }

                return IsYearendExist;
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
        /// Update Direct Payment Hdr Jounalize Flag
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateDirectPaymentHdrPDC(int DirPaymentPK, byte pdcFlag)
        {
            long retval = 0;
            FIN_TRX_HDR FIN_TRX_HDR_Obj;
            List<FIN_TRX> FIN_TRX_Obj = new List<FIN_TRX>();
            try
            {

                retval = 0;
                FIN_TRX_HDR_Obj = currentEntity.FIN_TRX_HDR.SingleOrDefault(sah => sah.FTH_PK == DirPaymentPK);
                if (FIN_TRX_HDR_Obj != null)
                {
                    FIN_TRX_HDR_Obj.FTH_PDC = pdcFlag;
                    FIN_TRX_Obj = currentEntity.FIN_TRX.Where(pdc => pdc.FTR_PDC == (pdcFlag==1 ? 2 : 3)).ToList();
                    foreach (FIN_TRX pdcObj in FIN_TRX_Obj)
                    {
                        FIN_TRX TRX_Obj = currentEntity.FIN_TRX.SingleOrDefault(sah => sah.FTR_PK == pdcObj.FTR_PK);
                        TRX_Obj.FTR_PDC = pdcFlag;
                    }
                    retval = DirPaymentPK;
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
    }
}
