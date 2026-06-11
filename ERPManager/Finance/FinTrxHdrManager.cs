using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;
using ERPManager;
using BusinessObject.CommonManagement;
using System.Data.Objects;
using ERPManager.POInvoicing;

namespace ERPManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FinTrxHdrManager" in both code and config file together.
    public class FinTrxHdrManager : IFinTrxHdrManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods

        /// <summary>
        /// FinTrxHdr Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public FinTrxHdrManager(ERPEntities currentEntity)
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

        public long SaveFinTrxHdr(List<FIN_TRX_HDR> finTrxhdrList)
        {
            //Holds save status
            long retval = 0;
            long logPk = 0;
            long? maxPK;
            long? maxlogPk;

            FIN_TRX_HDR oldFinTrxHdrObj;

            List<FIN_TRX> FIN_TRX_List;
            FinTrxManager FinTrxManagerobj = new FinTrxManager(this.currentEntity);
            FinYearMstManager FinYearMstManagerObj = new FinYearMstManager(this.currentEntity);
            CommonFunctionsManager ComnFnManagerObj = new CommonFunctionsManager(this.currentEntity);
            List<FIN_PAYMENT_VND_TAX_HDR> finPaymentVndTaxHdrList;
            FinPaymentVndTaxHdrmanager finPaymentVndTaxHdrmanagerObj;
            List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
            short? FTH_FIN_YEAR;
            try
            {
                foreach (FIN_TRX_HDR FIN_TRX_HDR_Obj in finTrxhdrList)
                {
                    ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();

                    FIN_TRX_List = FIN_TRX_HDR_Obj.FIN_TRX.ToList();
                    FIN_TRX_HDR_Obj.FIN_TRX.Clear();


                    if (FIN_TRX_HDR_Obj.FTH_PK == 0)
                    {
                        AdmTrxLogDet.ATL_ACTION = (byte)LogAction.NEW;
                        // Gets last FIN_PAYMENT_VND_HDR pk
                        maxPK = this.currentEntity.FIN_TRX_HDR.Max(pvh => (int?)pvh.FTH_PK);

                        // Sets return value as next FIN_PAYMENT_VND_HDR pk
                        retval = (maxPK.HasValue ? maxPK.Value + 1 : 1);

                        FIN_TRX_HDR_Obj.FTH_PK = retval;
                        //FIN_INVOICE_VND_HDR_Obj.IVH_NO = retval.ToString();

                        FIN_TRX_HDR_Obj.FTH_CRTD_DT = DateTime.Now;
                        FIN_TRX_HDR_Obj.FTH_MOD_DT = DateTime.Now;
                        FTH_FIN_YEAR = FinYearMstManagerObj.GetFinYear((DateTime)(FIN_TRX_HDR_Obj.FTH_DATE.HasValue ? FIN_TRX_HDR_Obj.FTH_DATE : DateTime.MinValue), FIN_TRX_HDR_Obj.FTH_BIZUNIT);
                        if (FTH_FIN_YEAR != null)
                            FIN_TRX_HDR_Obj.FTH_FIN_YEAR = (short)FTH_FIN_YEAR;
                        else
                            return -111;
                        // Add new FIN_TRX_HDR to the db context
                        this.currentEntity.FIN_TRX_HDR.AddObject(FIN_TRX_HDR_Obj);
                        //set the FIN_TRX_HDR pk as the fk of  FIN_TRX
                        FIN_TRX_List.ForEach(dtl => dtl.FTR_TRX_HDR = retval);
                        //FinTrxManagerobj = new FinTrxManager(this.currentEntity);
                        //Save Mapping Details
                        FinTrxManagerobj.SaveFinTrx(FIN_TRX_List);

                        finPaymentVndTaxHdrList = FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR == null ?
                            new List<FIN_PAYMENT_VND_TAX_HDR>() : FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR.ToList();
                        FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR.Clear();
                        finPaymentVndTaxHdrList.ForEach(dtl => dtl.WTH_TRX_HDR = retval);
                        if (finPaymentVndTaxHdrList.Count > 0)
                        {
                            finPaymentVndTaxHdrmanagerObj = new FinPaymentVndTaxHdrmanager(this.currentEntity);
                            finPaymentVndTaxHdrmanagerObj.SaveDirectWHTTax(finPaymentVndTaxHdrList);
                        }

                    }
                    else
                    {
                        //oldFinTrxHdrObj = currentEntity.FIN_TRX_HDR.SingleOrDefault(sah => sah.FTH_PK == FIN_TRX_HDR_Obj.FTH_PK && sah.FTH_MOD_DT == FIN_TRX_HDR_Obj.FTH_MOD_DT);
                        oldFinTrxHdrObj = currentEntity.FIN_TRX_HDR.SingleOrDefault(sah => sah.FTH_PK == FIN_TRX_HDR_Obj.FTH_PK);
                        // sah.FTH_MOD_DT == FIN_TRX_HDR_Obj.FTH_MOD_DT
                        //.ToString("dd-MM-yyyy hh:mm:ss")
                        //if (oldFinTrxHdrObj != null)
                        // For avoid MilliSeconds Issue in Cuncurrncy  

                        if (oldFinTrxHdrObj != null && oldFinTrxHdrObj.FTH_MOD_DT.ToString("dd-MM-yyyy hh:mm:ss") == FIN_TRX_HDR_Obj.FTH_MOD_DT.ToString("dd-MM-yyyy hh:mm:ss"))
                        {
                            AdmTrxLogDet.ATL_ACTION = (byte)LogAction.UPDATE;
                            oldFinTrxHdrObj.FTH_DATE = FIN_TRX_HDR_Obj.FTH_DATE;
                            oldFinTrxHdrObj.FTH_TRX_DATE = FIN_TRX_HDR_Obj.FTH_TRX_DATE;
                            oldFinTrxHdrObj.FTH_VOUCHER_NO = FIN_TRX_HDR_Obj.FTH_VOUCHER_NO;
                            oldFinTrxHdrObj.FTH_REF_TYPE = FIN_TRX_HDR_Obj.FTH_REF_TYPE;
                            oldFinTrxHdrObj.FTH_REF_PK = FIN_TRX_HDR_Obj.FTH_REF_PK;
                            oldFinTrxHdrObj.FTH_REF_DATE = FIN_TRX_HDR_Obj.FTH_REF_DATE;
                            oldFinTrxHdrObj.FTH_TAX_DATE = FIN_TRX_HDR_Obj.FTH_TAX_DATE;
                            oldFinTrxHdrObj.FTH_REF_NO = FIN_TRX_HDR_Obj.FTH_REF_NO;
                            oldFinTrxHdrObj.FTH_NARRATION = FIN_TRX_HDR_Obj.FTH_NARRATION;
                            oldFinTrxHdrObj.FTH_TRX_CURR = FIN_TRX_HDR_Obj.FTH_TRX_CURR;
                            oldFinTrxHdrObj.FTH_EXCHG_RATE = FIN_TRX_HDR_Obj.FTH_EXCHG_RATE;
                            oldFinTrxHdrObj.FTH_BASE_CURR = FIN_TRX_HDR_Obj.FTH_BASE_CURR;
                            FTH_FIN_YEAR = FinYearMstManagerObj.GetFinYear((DateTime)(FIN_TRX_HDR_Obj.FTH_DATE.HasValue ? FIN_TRX_HDR_Obj.FTH_DATE : DateTime.MinValue), FIN_TRX_HDR_Obj.FTH_BIZUNIT);
                            if (FTH_FIN_YEAR != null)
                                oldFinTrxHdrObj.FTH_FIN_YEAR = (short)FTH_FIN_YEAR;
                            else
                                return -111;
                            oldFinTrxHdrObj.FTH_REMARKS = FIN_TRX_HDR_Obj.FTH_REMARKS;
                            if (!oldFinTrxHdrObj.FTH_IS_JRNLD)
                                oldFinTrxHdrObj.FTH_IS_JRNLD = FIN_TRX_HDR_Obj.FTH_IS_JRNLD;
                            //oldFinTrxHdrObj.FTH_STATUS = FIN_TRX_HDR_Obj.FTH_STATUS;
                            oldFinTrxHdrObj.FTH_ACTIVE = FIN_TRX_HDR_Obj.FTH_ACTIVE;
                            oldFinTrxHdrObj.FTH_MOD_BY = FIN_TRX_HDR_Obj.FTH_MOD_BY;
                            oldFinTrxHdrObj.FTH_MOD_DT = DateTime.Now;
                            oldFinTrxHdrObj.FTH_DEPT = FIN_TRX_HDR_Obj.FTH_DEPT;
                            oldFinTrxHdrObj.FTH_BIZUNIT = FIN_TRX_HDR_Obj.FTH_BIZUNIT;
                            oldFinTrxHdrObj.FTH_IS_DELETED = FIN_TRX_HDR_Obj.FTH_IS_DELETED;
                            oldFinTrxHdrObj.FTH_COMPANY = FIN_TRX_HDR_Obj.FTH_COMPANY;
                            if ((FIN_TRX_HDR_Obj.FTH_PARTY_NAME != null) && (FIN_TRX_HDR_Obj.FTH_PARTY_NAME != ""))
                                oldFinTrxHdrObj.FTH_PARTY_NAME = FIN_TRX_HDR_Obj.FTH_PARTY_NAME;
                            FinTrxManagerobj.SaveFinTrx(FIN_TRX_List);
                            retval = FIN_TRX_HDR_Obj.FTH_PK;

                            finPaymentVndTaxHdrList = FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR == null ?
                                new List<FIN_PAYMENT_VND_TAX_HDR>() : FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR.ToList();
                            FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR.Clear();
                            if (finPaymentVndTaxHdrList.Count > 0)
                            {
                                finPaymentVndTaxHdrmanagerObj = new FinPaymentVndTaxHdrmanager(this.currentEntity);
                                finPaymentVndTaxHdrmanagerObj.SaveDirectWHTTax(finPaymentVndTaxHdrList);
                            }

                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }

                    }

                    AdmTrxLogDet.ATL_APP_TRX_CODE = FIN_TRX_HDR_Obj.FTH_VOUCHER_NO;
                    AdmTrxLogDet.ATL_APP_TYPE = FIN_TRX_HDR_Obj.FTH_REF_TYPE;
                    AdmTrxLogDet.ATL_MOD_BY = FIN_TRX_HDR_Obj.FTH_MOD_BY;
                    AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                    AdmTrxLogDet.ATL_BIZUNIT = FIN_TRX_HDR_Obj.FTH_BIZUNIT;
                    AdmTrxLogDet.ATL_APP_TRX_PK = FIN_TRX_HDR_Obj.FTH_PK;
                    AdmTrxLogDet.ATL_PK = 0;
                    AdmTrxLogList.Add(AdmTrxLogDet);
                }

                // For saving transaction log
                ComnFnManagerObj.SaveLog(AdmTrxLogList);

                //return FIN_TRX_HDR pk
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

        public long SaveDirectFinTrxHdr(List<FIN_TRX_HDR> finTrxhdrList)
        {
            //Holds save status
            long retval = 0;
            long? maxPK;

            FIN_TRX_HDR oldFinTrxHdrObj;

            List<FIN_TRX> FIN_TRX_List;
            FinTrxManager FinTrxManagerobj = new FinTrxManager(this.currentEntity);
            FinYearMstManager FinYearMstManagerObj = new FinYearMstManager(this.currentEntity);
            CommonFunctionsManager ComnFnManagerObj = new CommonFunctionsManager(this.currentEntity);
            List<FIN_PAYMENT_VND_TAX_HDR> finPaymentVndTaxHdrList;
            FinPaymentVndTaxHdrmanager finPaymentVndTaxHdrmanagerObj;
            FinPaymentVndHdrManager finPaymentVndHdrManagerObj = new FinPaymentVndHdrManager(this.currentEntity);
            List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
            short? FTH_FIN_YEAR;
            try
            {
                foreach (FIN_TRX_HDR FIN_TRX_HDR_Obj in finTrxhdrList)
                {
                    ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();

                    FIN_TRX_List = FIN_TRX_HDR_Obj.FIN_TRX.ToList();
                    FIN_TRX_HDR_Obj.FIN_TRX.Clear();

                    if (FIN_TRX_HDR_Obj.FTH_PK == 0)
                    {
                        AdmTrxLogDet.ATL_ACTION = (byte)LogAction.NEW;
                        // Gets last FIN_PAYMENT_VND_HDR pk
                        maxPK = this.currentEntity.FIN_TRX_HDR.Max(pvh => (int?)pvh.FTH_PK);

                        // Sets return value as next FIN_PAYMENT_VND_HDR pk
                        retval = (maxPK.HasValue ? maxPK.Value + 1 : 1);

                        FIN_TRX_HDR_Obj.FTH_PK = retval;
                        //FIN_INVOICE_VND_HDR_Obj.IVH_NO = retval.ToString();

                        FIN_TRX_HDR_Obj.FTH_CRTD_DT = DateTime.Now;
                        FIN_TRX_HDR_Obj.FTH_MOD_DT = DateTime.Now;
                        FTH_FIN_YEAR = FinYearMstManagerObj.GetFinYear((DateTime)(FIN_TRX_HDR_Obj.FTH_DATE.HasValue ? FIN_TRX_HDR_Obj.FTH_DATE : DateTime.MinValue), FIN_TRX_HDR_Obj.FTH_BIZUNIT);
                        if (FTH_FIN_YEAR != null)
                            FIN_TRX_HDR_Obj.FTH_FIN_YEAR = (short)FTH_FIN_YEAR;
                        else
                            return -111;
                        // Add new FIN_TRX_HDR to the db context
                        this.currentEntity.FIN_TRX_HDR.AddObject(FIN_TRX_HDR_Obj);
                        //set the FIN_TRX_HDR pk as the fk of  FIN_TRX
                        FIN_TRX_List.ForEach(dtl => dtl.FTR_TRX_HDR = retval);
                        //FinTrxManagerobj = new FinTrxManager(this.currentEntity);
                        //Save Mapping Details
                        FinTrxManagerobj.SaveFinTrx(FIN_TRX_List);

                        finPaymentVndTaxHdrList = FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR == null ?
                            new List<FIN_PAYMENT_VND_TAX_HDR>() : FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR.ToList();
                        FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR.Clear();
                        finPaymentVndTaxHdrList.ForEach(dtl => dtl.WTH_TRX_HDR = retval);
                        if (finPaymentVndTaxHdrList.Count > 0)
                        {
                            finPaymentVndTaxHdrmanagerObj = new FinPaymentVndTaxHdrmanager(this.currentEntity);
                            finPaymentVndTaxHdrmanagerObj.SaveDirectWHTTax(finPaymentVndTaxHdrList);
                        }

                    }
                    else
                    {
                        oldFinTrxHdrObj = currentEntity.FIN_TRX_HDR.SingleOrDefault(sah => sah.FTH_PK == FIN_TRX_HDR_Obj.FTH_PK && sah.FTH_MOD_DT == FIN_TRX_HDR_Obj.FTH_MOD_DT);

                        if (oldFinTrxHdrObj != null)
                        {
                            AdmTrxLogDet.ATL_ACTION = (byte)LogAction.UPDATE;
                            oldFinTrxHdrObj.FTH_DATE = FIN_TRX_HDR_Obj.FTH_DATE;
                            oldFinTrxHdrObj.FTH_TRX_DATE = FIN_TRX_HDR_Obj.FTH_TRX_DATE;
                            oldFinTrxHdrObj.FTH_VOUCHER_NO = FIN_TRX_HDR_Obj.FTH_VOUCHER_NO;
                            oldFinTrxHdrObj.FTH_REF_TYPE = FIN_TRX_HDR_Obj.FTH_REF_TYPE;
                            oldFinTrxHdrObj.FTH_REF_PK = FIN_TRX_HDR_Obj.FTH_REF_PK;
                            oldFinTrxHdrObj.FTH_REF_DATE = FIN_TRX_HDR_Obj.FTH_REF_DATE;
                            oldFinTrxHdrObj.FTH_REF_NO = FIN_TRX_HDR_Obj.FTH_REF_NO;
                            oldFinTrxHdrObj.FTH_NARRATION = FIN_TRX_HDR_Obj.FTH_NARRATION;
                            oldFinTrxHdrObj.FTH_TRX_CURR = FIN_TRX_HDR_Obj.FTH_TRX_CURR;
                            oldFinTrxHdrObj.FTH_EXCHG_RATE = FIN_TRX_HDR_Obj.FTH_EXCHG_RATE;
                            oldFinTrxHdrObj.FTH_BASE_CURR = FIN_TRX_HDR_Obj.FTH_BASE_CURR;
                            FTH_FIN_YEAR = FinYearMstManagerObj.GetFinYear((DateTime)(FIN_TRX_HDR_Obj.FTH_DATE.HasValue ? FIN_TRX_HDR_Obj.FTH_DATE : DateTime.MinValue), FIN_TRX_HDR_Obj.FTH_BIZUNIT);
                            if (FTH_FIN_YEAR != null)
                                oldFinTrxHdrObj.FTH_FIN_YEAR = (short)FTH_FIN_YEAR;
                            else
                                return -111;
                            oldFinTrxHdrObj.FTH_REMARKS = FIN_TRX_HDR_Obj.FTH_REMARKS;
                            if (!oldFinTrxHdrObj.FTH_IS_JRNLD)
                                oldFinTrxHdrObj.FTH_IS_JRNLD = FIN_TRX_HDR_Obj.FTH_IS_JRNLD;
                            //oldFinTrxHdrObj.FTH_STATUS = FIN_TRX_HDR_Obj.FTH_STATUS;
                            oldFinTrxHdrObj.FTH_ACTIVE = FIN_TRX_HDR_Obj.FTH_ACTIVE;
                            oldFinTrxHdrObj.FTH_MOD_BY = FIN_TRX_HDR_Obj.FTH_MOD_BY;
                            oldFinTrxHdrObj.FTH_MOD_DT = DateTime.Now;
                            oldFinTrxHdrObj.FTH_DEPT = FIN_TRX_HDR_Obj.FTH_DEPT;
                            oldFinTrxHdrObj.FTH_BIZUNIT = FIN_TRX_HDR_Obj.FTH_BIZUNIT;
                            oldFinTrxHdrObj.FTH_IS_DELETED = FIN_TRX_HDR_Obj.FTH_IS_DELETED;
                            oldFinTrxHdrObj.FTH_COMPANY = FIN_TRX_HDR_Obj.FTH_COMPANY;
                            oldFinTrxHdrObj.FTH_PDC = FIN_TRX_HDR_Obj.FTH_PDC;
                            //if ((FIN_TRX_HDR_Obj.FTH_PARTY_NAME != null) && (FIN_TRX_HDR_Obj.FTH_PARTY_NAME != ""))
                            oldFinTrxHdrObj.FTH_PARTY_NAME = FIN_TRX_HDR_Obj.FTH_PARTY_NAME;
                            FinTrxManagerobj.SaveFinTrx(FIN_TRX_List);
                            retval = FIN_TRX_HDR_Obj.FTH_PK;

                            finPaymentVndTaxHdrList = FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR == null ?
                                new List<FIN_PAYMENT_VND_TAX_HDR>() : FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR.ToList();
                            FIN_TRX_HDR_Obj.FIN_PAYMENT_VND_TAX_HDR.Clear();

                            long? resuslt = finPaymentVndHdrManagerObj.DeleteDirectVatTaxForDirectPayment(oldFinTrxHdrObj.FTH_PK, (byte)WHTCategoryEnum.WHT);
                            long? resuslt1 = finPaymentVndHdrManagerObj.DeleteDirectVatTaxForDirectPayment(oldFinTrxHdrObj.FTH_PK, (byte)WHTCategoryEnum.VATBUY);

                            if (finPaymentVndTaxHdrList.Count > 0)
                            {
                                finPaymentVndTaxHdrmanagerObj = new FinPaymentVndTaxHdrmanager(this.currentEntity);
                                finPaymentVndTaxHdrmanagerObj.SaveDirectWHTTax(finPaymentVndTaxHdrList);
                            }

                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }

                    }
                    AdmTrxLogDet.ATL_APP_TRX_CODE = FIN_TRX_HDR_Obj.FTH_VOUCHER_NO;
                    AdmTrxLogDet.ATL_APP_TYPE = FIN_TRX_HDR_Obj.FTH_REF_TYPE;
                    AdmTrxLogDet.ATL_MOD_BY = FIN_TRX_HDR_Obj.FTH_MOD_BY;
                    AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                    AdmTrxLogDet.ATL_BIZUNIT = FIN_TRX_HDR_Obj.FTH_BIZUNIT;
                    AdmTrxLogDet.ATL_APP_TRX_PK = FIN_TRX_HDR_Obj.FTH_PK;
                    AdmTrxLogDet.ATL_PK = 0;
                    AdmTrxLogList.Add(AdmTrxLogDet);
                }

                // For saving transaction log
                ComnFnManagerObj.SaveLog(AdmTrxLogList);

                //return FIN_TRX_HDR pk
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
        /// Delete Journalize
        /// </summary>
        /// <param name="JournalizePK"></param>
        /// <returns></returns>
        public bool IsDummyEntry(string REFTYPE, int REFPK, int FTHPK)
        {

            FIN_TRX_HDR Old_FIN_TRX_HDR_Obj;
            List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
            //FinTrxManager FinTrxManagerobj = new FinTrxManager(this.currentEntity);
            CommonFunctionsManager ComnFnManagerObj = new CommonFunctionsManager(this.currentEntity);
            try
            {
                if (REFPK > 0)
                {
                    Old_FIN_TRX_HDR_Obj = currentEntity.FIN_TRX_HDR.SingleOrDefault(sah => sah.FTH_REF_TYPE == REFTYPE && sah.FTH_REF_PK == REFPK && sah.FTH_IS_DELETED == false);
                }
                else
                {
                    Old_FIN_TRX_HDR_Obj = currentEntity.FIN_TRX_HDR.SingleOrDefault(sah => sah.FTH_REF_TYPE == REFTYPE && sah.FTH_PK == FTHPK && sah.FTH_IS_DELETED == false);
                }
                if (Old_FIN_TRX_HDR_Obj != null)
                {
                    return true;
                }
                else
                {
                    return false;
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
        }

        /// <summary>
        /// Delete Journalize
        /// </summary>
        /// <param name="JournalizePK"></param>
        /// <returns></returns>
        public long DeleteFinTrx(string REFTYPE, int REFPK, int FTHPK)
        {
            long retval = 0;
            FIN_TRX_HDR Old_FIN_TRX_HDR_Obj;
            List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
            //FinTrxManager FinTrxManagerobj = new FinTrxManager(this.currentEntity);
            CommonFunctionsManager ComnFnManagerObj = new CommonFunctionsManager(this.currentEntity);
            try
            {
                if (REFPK > 0)
                {
                    Old_FIN_TRX_HDR_Obj = currentEntity.FIN_TRX_HDR.SingleOrDefault(sah => sah.FTH_REF_TYPE == REFTYPE && sah.FTH_REF_PK == REFPK && sah.FTH_IS_DELETED == false);
                }
                else
                {
                    Old_FIN_TRX_HDR_Obj = currentEntity.FIN_TRX_HDR.SingleOrDefault(sah => sah.FTH_REF_TYPE == REFTYPE && sah.FTH_PK == FTHPK && sah.FTH_IS_DELETED == false);
                }

                if (Old_FIN_TRX_HDR_Obj != null)
                {
                    if (Old_FIN_TRX_HDR_Obj.FTH_VOUCHER_NO == null) //  Not submitted
                    {
                        //Mark as Deleted
                        Old_FIN_TRX_HDR_Obj.FTH_IS_DELETED = true;
                        Old_FIN_TRX_HDR_Obj.FTH_MOD_DT = DateTime.Now;

                        foreach (FIN_TRX Obj_FIN_TRX in Old_FIN_TRX_HDR_Obj.FIN_TRX)
                        {
                            Obj_FIN_TRX.FTR_ACCOUNT = null;
                        }
                    }
                    else // Submitted
                    {
                        Old_FIN_TRX_HDR_Obj.FTH_IS_DELETED = true;
                        Old_FIN_TRX_HDR_Obj.FTH_MOD_DT = DateTime.Now;

                        //Execute the SP [SPWKF_JOB_AUTO_INVOICE_VND_SUB_STATUS] to Insert New Dummy Voucher
                        //

                        //
                    }

                    retval = Old_FIN_TRX_HDR_Obj.FTH_PK;

                    // Save Log

                    ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                    AdmTrxLogDet.ATL_APP_TRX_CODE = Old_FIN_TRX_HDR_Obj.FTH_VOUCHER_NO;
                    AdmTrxLogDet.ATL_APP_TYPE = Old_FIN_TRX_HDR_Obj.FTH_REF_TYPE;
                    AdmTrxLogDet.ATL_MOD_BY = Old_FIN_TRX_HDR_Obj.FTH_MOD_BY;
                    AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                    AdmTrxLogDet.ATL_BIZUNIT = Old_FIN_TRX_HDR_Obj.FTH_BIZUNIT;
                    AdmTrxLogDet.ATL_APP_TRX_PK = Old_FIN_TRX_HDR_Obj.FTH_PK;
                    AdmTrxLogDet.ATL_ACTION = (byte)LogAction.DELETE;
                    AdmTrxLogDet.ATL_PK = 0;
                    AdmTrxLogList.Add(AdmTrxLogDet);

                    // Save log
                    ComnFnManagerObj.SaveLog(AdmTrxLogList);
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

        public List<FIN_TRX_HDR> GetfinTxtHdrList(FIN_TRX_HDR finTrxHdrObj, ServiceUtility utilityObj)
        {
            List<FIN_TRX_HDR> finTrxHdrList = null;
            IQueryable<FIN_TRX_HDR> fintrxHdrQuery;
            //int pageSize;
            //int totalCount;
            //string []Filter={"CRJ","MSIRJ"};
            try
            {
                if (!string.IsNullOrEmpty(utilityObj.SearchValue) || finTrxHdrObj.FTH_PK > 0)
                {
                    if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CRJ) //For MSIRJ && CRJ Types in search
                    {
                        fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                          where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE
                                              && fth.FTH_PK == (finTrxHdrObj.FTH_PK > 0 ? finTrxHdrObj.FTH_PK : fth.FTH_PK)
                                              //Condition for MSIRJ && CRJ Types for search
                                              && (string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_TYPE) ? true
                                              : finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CRJ ?
                                              (fth.FTH_REF_TYPE == ApplicationType.MSIRJ || fth.FTH_REF_TYPE == ApplicationType.CRJ) : true)
                                          && (utilityObj.SearchValue != null ? fth.FTH_VOUCHER_NO.Contains(utilityObj.SearchValue) : true)
                                          && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO) ? fth.FTH_REF_NO.Contains(finTrxHdrObj.FTH_REF_NO) : true)
                                          && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) ? fth.FTH_PARTY_NAME.Contains(finTrxHdrObj.FTH_PARTY_NAME) : true)
                                          select fth
                                      );
                    }
                    else if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CRTJ) //For MSIRTJ && CRTJ Types in search
                    {
                        fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                          where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE
                                              && fth.FTH_PK == (finTrxHdrObj.FTH_PK > 0 ? finTrxHdrObj.FTH_PK : fth.FTH_PK)
                                              //Condition for MSIRJ && CRJ Types for search
                                              && (string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_TYPE) ? true
                                              : finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CRTJ ?
                                              (fth.FTH_REF_TYPE == ApplicationType.MSIRTJ || fth.FTH_REF_TYPE == ApplicationType.CRTJ) : true)
                                          && (utilityObj.SearchValue != null ? fth.FTH_VOUCHER_NO.Contains(utilityObj.SearchValue) : true)
                                          && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO) ? fth.FTH_REF_NO.Contains(finTrxHdrObj.FTH_REF_NO) : true)
                                          && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) ? fth.FTH_PARTY_NAME.Contains(finTrxHdrObj.FTH_PARTY_NAME) : true)
                                          select fth
                                      );
                    }
                    else if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.VPTJ) //Payment Trading Journal (ie,Payment done against Goods,Expense Invoice,Service Invoice,Agent Invoice)
                    {
                        fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                          where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE                                            
                                              && (fth.FTH_REF_TYPE == ApplicationType.VPTJ || fth.FTH_REF_TYPE == ApplicationType.AIPTJ || fth.FTH_REF_TYPE == ApplicationType.SIPTJ || fth.FTH_REF_TYPE == ApplicationType.EIPTJ)
                                              && fth.FTH_PK == (finTrxHdrObj.FTH_PK > 0 ? finTrxHdrObj.FTH_PK : fth.FTH_PK)                                            
                                              && ((fth.FTH_REF_TYPE == ApplicationType.YCV || fth.FTH_REF_TYPE == ApplicationType.CLSTJ) ?
                                                  (finTrxHdrObj.FTH_IS_DELETED ? fth.FTH_IS_DELETED : !fth.FTH_IS_DELETED) : true)
                                              && (utilityObj.SearchValue != null ? fth.FTH_VOUCHER_NO.Contains(utilityObj.SearchValue) : true)
                                              && (fth.FTH_BIZUNIT == (finTrxHdrObj.FTH_BIZUNIT > 0 ? finTrxHdrObj.FTH_BIZUNIT : fth.FTH_BIZUNIT))
                                              && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO) ? fth.FTH_REF_NO.Contains(finTrxHdrObj.FTH_REF_NO) : true)
                                              && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) ? fth.FTH_PARTY_NAME.Contains(finTrxHdrObj.FTH_PARTY_NAME) : true)
                                          select fth
                                       );
                    }
                    else
                    {
                        fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                          where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE
                                              && (string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_TYPE) ? true
                                              : finTrxHdrObj.FTH_REF_TYPE == ApplicationType.PIJ ?
                                              (fth.FTH_REF_TYPE == ApplicationType.PIJ || fth.FTH_REF_TYPE == ApplicationType.PSIJ)
                                              : finTrxHdrObj.FTH_REF_TYPE == ApplicationType.VPJ ?
                                              (fth.FTH_REF_TYPE == ApplicationType.VPJ || fth.FTH_REF_TYPE == ApplicationType.AIPJ || fth.FTH_REF_TYPE == ApplicationType.SIPJ || fth.FTH_REF_TYPE == ApplicationType.EIPJ)
                                              : fth.FTH_REF_TYPE == finTrxHdrObj.FTH_REF_TYPE)
                                              && fth.FTH_PK == (finTrxHdrObj.FTH_PK > 0 ? finTrxHdrObj.FTH_PK : fth.FTH_PK)
                                             && ((fth.FTH_REF_TYPE == ApplicationType.YCV || fth.FTH_REF_TYPE == ApplicationType.CLSTJ) ?
                                                (finTrxHdrObj.FTH_IS_DELETED ? fth.FTH_IS_DELETED : !fth.FTH_IS_DELETED) : true)
                                             && (utilityObj.SearchValue != null ? fth.FTH_VOUCHER_NO.Contains(utilityObj.SearchValue) : true)
                                             && (fth.FTH_BIZUNIT == (finTrxHdrObj.FTH_BIZUNIT > 0 ? finTrxHdrObj.FTH_BIZUNIT : fth.FTH_BIZUNIT))
                                             && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO) ? fth.FTH_REF_NO.Contains(finTrxHdrObj.FTH_REF_NO) : true)
                                             && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) ? fth.FTH_PARTY_NAME.Contains(finTrxHdrObj.FTH_PARTY_NAME) : true)
                                          select fth
                                       );
                    }
                }
                else
                {
                    if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CRJ) //For MSIRJ && CRJ Types
                    {
                        fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                          where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE
                                              //Condition for MSIRJ && CRJ Types
                                              && (string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_TYPE) ? true
                                              : finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CRJ ?
                                              (fth.FTH_REF_TYPE == ApplicationType.MSIRJ || fth.FTH_REF_TYPE == ApplicationType.CRJ) : true)
                                              && fth.FTH_REF_PK == (finTrxHdrObj.FTH_REF_PK > 0 ? finTrxHdrObj.FTH_REF_PK : fth.FTH_REF_PK)
                                              && fth.FTH_PK == (finTrxHdrObj.FTH_PK > 0 ? finTrxHdrObj.FTH_PK : fth.FTH_PK)
                                              && (finTrxHdrObj.FTH_IS_DELETED ? true : fth.FTH_STATUS == (finTrxHdrObj.FTH_STATUS > 0 ? finTrxHdrObj.FTH_STATUS : fth.FTH_STATUS))
                                              && (utilityObj.FilterDate != null ? fth.FTH_DATE >= utilityObj.FilterDate : true)
                                              && (utilityObj.FilterToDate != null ? fth.FTH_DATE <= utilityObj.FilterToDate : true)
                                              && (utilityObj.NeedAdvanceFilter ? fth.FTH_IS_JRNLD == finTrxHdrObj.FTH_IS_JRNLD : true)
                                              && (finTrxHdrObj.FTH_IS_DELETED ? fth.FTH_IS_DELETED : !fth.FTH_IS_DELETED)
                                              && (utilityObj.SearchValue != null ? fth.FTH_VOUCHER_NO.Contains(utilityObj.SearchValue) : true)
                                              && (fth.FTH_BIZUNIT == (finTrxHdrObj.FTH_BIZUNIT > 0 ? finTrxHdrObj.FTH_BIZUNIT : fth.FTH_BIZUNIT))
                                              && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO) ? fth.FTH_REF_NO.Contains(finTrxHdrObj.FTH_REF_NO) : true)
                                              && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) ? fth.FTH_PARTY_NAME.Contains(finTrxHdrObj.FTH_PARTY_NAME) : true)
                                          select fth
                                       );
                    }
                    else if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CRTJ) //For MSIRTJ && CRTJ Types
                    {
                        fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                          where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE
                                              //Condition for MSIRJ && CRJ Types
                                              && (string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_TYPE) ? true
                                              : finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CRTJ ?
                                              (fth.FTH_REF_TYPE == ApplicationType.MSIRTJ || fth.FTH_REF_TYPE == ApplicationType.CRTJ) : true)
                                              && fth.FTH_REF_PK == (finTrxHdrObj.FTH_REF_PK > 0 ? finTrxHdrObj.FTH_REF_PK : fth.FTH_REF_PK)
                                              && fth.FTH_PK == (finTrxHdrObj.FTH_PK > 0 ? finTrxHdrObj.FTH_PK : fth.FTH_PK)
                                              && (finTrxHdrObj.FTH_IS_DELETED ? true : fth.FTH_STATUS == (finTrxHdrObj.FTH_STATUS > 0 ? finTrxHdrObj.FTH_STATUS : fth.FTH_STATUS))
                                              && (utilityObj.FilterDate != null ? fth.FTH_DATE >= utilityObj.FilterDate : true)
                                              && (utilityObj.FilterToDate != null ? fth.FTH_DATE <= utilityObj.FilterToDate : true)
                                              && (utilityObj.NeedAdvanceFilter ? fth.FTH_IS_JRNLD == finTrxHdrObj.FTH_IS_JRNLD : true)
                                              && (finTrxHdrObj.FTH_IS_DELETED ? fth.FTH_IS_DELETED : !fth.FTH_IS_DELETED)
                                              && (utilityObj.SearchValue != null ? fth.FTH_VOUCHER_NO.Contains(utilityObj.SearchValue) : true)
                                              && (fth.FTH_BIZUNIT == (finTrxHdrObj.FTH_BIZUNIT > 0 ? finTrxHdrObj.FTH_BIZUNIT : fth.FTH_BIZUNIT))
                                              && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO) ? fth.FTH_REF_NO.Contains(finTrxHdrObj.FTH_REF_NO) : true)
                                              && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) ? fth.FTH_PARTY_NAME.Contains(finTrxHdrObj.FTH_PARTY_NAME) : true) 
                                          select fth
                                       );
                    }
                    else if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.VPTJ) //Payment Trading Journal (ie,Payment done against Goods,Expense Invoice,Service Invoice,Agent Invoice)
                    {
                        fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                          where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE                                            
                                              && (fth.FTH_REF_TYPE == ApplicationType.VPTJ || fth.FTH_REF_TYPE == ApplicationType.SIPTJ || fth.FTH_REF_TYPE == ApplicationType.AIPTJ || fth.FTH_REF_TYPE == ApplicationType.EIPTJ)                                              
                                              && fth.FTH_REF_PK == (finTrxHdrObj.FTH_REF_PK > 0 ? finTrxHdrObj.FTH_REF_PK : fth.FTH_REF_PK)
                                              && fth.FTH_PK == (finTrxHdrObj.FTH_PK > 0 ? finTrxHdrObj.FTH_PK : fth.FTH_PK)
                                              && (finTrxHdrObj.FTH_IS_DELETED? true:fth.FTH_STATUS == (finTrxHdrObj.FTH_STATUS > 0 ? finTrxHdrObj.FTH_STATUS : fth.FTH_STATUS))
                                              && (utilityObj.NeedAdvanceFilter ? (!finTrxHdrObj.FTH_IS_JRNLD ? fth.FTH_STATUS == finTrxHdrObj.FTH_STATUS : true) : true)
                                              && (utilityObj.FilterDate != null ? fth.FTH_DATE >= utilityObj.FilterDate : true)
                                              && (utilityObj.FilterToDate != null ? fth.FTH_DATE <= utilityObj.FilterToDate : true)
                                              && (utilityObj.NeedAdvanceFilter ? fth.FTH_IS_JRNLD == finTrxHdrObj.FTH_IS_JRNLD : true)
                                              && (finTrxHdrObj.FTH_IS_DELETED ? fth.FTH_IS_DELETED : !fth.FTH_IS_DELETED)                                            
                                              && (utilityObj.SearchValue != null ? fth.FTH_VOUCHER_NO.Contains(utilityObj.SearchValue) : true)
                                              && (fth.FTH_BIZUNIT == (finTrxHdrObj.FTH_BIZUNIT > 0 ? finTrxHdrObj.FTH_BIZUNIT : fth.FTH_BIZUNIT))
                                              && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO) ? fth.FTH_REF_NO.Contains(finTrxHdrObj.FTH_REF_NO) : true)
                                              && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) ? fth.FTH_PARTY_NAME.Contains(finTrxHdrObj.FTH_PARTY_NAME) : true)
                                          select fth
                                       );
                    }
                    else
                    {
                        fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                          where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE
                                              && (string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_TYPE) ? true
                                              : finTrxHdrObj.FTH_REF_TYPE == ApplicationType.PIJ ?
                                              (fth.FTH_REF_TYPE == ApplicationType.PIJ || fth.FTH_REF_TYPE == ApplicationType.PSIJ)
                                              : finTrxHdrObj.FTH_REF_TYPE == ApplicationType.VPJ ?
                                              (fth.FTH_REF_TYPE == ApplicationType.VPJ || fth.FTH_REF_TYPE == ApplicationType.SIPJ || fth.FTH_REF_TYPE == ApplicationType.AIPJ || fth.FTH_REF_TYPE == ApplicationType.EIPJ)
                                              : fth.FTH_REF_TYPE == finTrxHdrObj.FTH_REF_TYPE)
                                              && fth.FTH_REF_PK == (finTrxHdrObj.FTH_REF_PK > 0 ? finTrxHdrObj.FTH_REF_PK : fth.FTH_REF_PK)
                                              && fth.FTH_PK == (finTrxHdrObj.FTH_PK > 0 ? finTrxHdrObj.FTH_PK : fth.FTH_PK)
                                              && (finTrxHdrObj.FTH_IS_DELETED? true:fth.FTH_STATUS == (finTrxHdrObj.FTH_STATUS > 0 ? finTrxHdrObj.FTH_STATUS : fth.FTH_STATUS))
                                              && (utilityObj.NeedAdvanceFilter ? (!finTrxHdrObj.FTH_IS_JRNLD ? fth.FTH_STATUS == finTrxHdrObj.FTH_STATUS : true) : true)
                                              && (utilityObj.FilterDate != null ? fth.FTH_DATE >= utilityObj.FilterDate : true)
                                              && (utilityObj.FilterToDate != null ? fth.FTH_DATE <= utilityObj.FilterToDate : true)
                                              && (utilityObj.NeedAdvanceFilter ? fth.FTH_IS_JRNLD == finTrxHdrObj.FTH_IS_JRNLD : true)
                                              && (finTrxHdrObj.FTH_IS_DELETED ? fth.FTH_IS_DELETED : !fth.FTH_IS_DELETED)
                                              && (fth.FTH_BIZUNIT == (finTrxHdrObj.FTH_BIZUNIT > 0 ? finTrxHdrObj.FTH_BIZUNIT : fth.FTH_BIZUNIT))
                                              && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO) ? fth.FTH_REF_NO.Contains(finTrxHdrObj.FTH_REF_NO) : true)
                                              && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) ? fth.FTH_PARTY_NAME.Contains(finTrxHdrObj.FTH_PARTY_NAME) : true)
                                          select fth
                                       );
                    }
                }
                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = fintrxHdrQuery.Count();

                //Filter Query
                //fintrxHdrQuery = FilterEntity(finTrxHdrObj, fintrxHdrQuery, utilityObj);

                // Apply Paging And Sorting for grid Purpose
                finTrxHdrList = fintrxHdrQuery.SortRecords<FIN_TRX_HDR>(utilityObj).ToList();

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

        public List<FIN_TRX_HDR> GetfinTxtHdrListByPK(FIN_TRX_HDR finTrxHdrObj)
        {
            List<FIN_TRX_HDR> finTrxHdrList = null;
            IQueryable<FIN_TRX_HDR> fintrxHdrQuery;
            //int pageSize;
            //int totalCount;
            try
            {
                fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                  where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE
                                      && fth.FTH_IS_DELETED == finTrxHdrObj.FTH_IS_DELETED
                                      && fth.FTH_PK == (finTrxHdrObj.FTH_PK > 0 ? finTrxHdrObj.FTH_PK : fth.FTH_PK)
                                      && fth.FTH_STATUS == (finTrxHdrObj.FTH_STATUS > 0 ? finTrxHdrObj.FTH_STATUS : fth.FTH_STATUS)
                                  select fth
                               );

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
        public List<FIN_TRX_HDR> GetSatusByAppPK(FIN_TRX_HDR finTrxHdrObj)
        {
            List<FIN_TRX_HDR> finTrxHdrList = null;
            IQueryable<FIN_TRX_HDR> fintrxHdrQuery;
            //int pageSize;
            //int totalCount;
            try
            {
                fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                  where fth.FTH_IS_DELETED == finTrxHdrObj.FTH_IS_DELETED
                                      && fth.FTH_REF_PK == (finTrxHdrObj.FTH_REF_PK > 0 ? finTrxHdrObj.FTH_REF_PK : fth.FTH_REF_PK)
                                   && fth.FTH_REF_TYPE == finTrxHdrObj.FTH_REF_TYPE
                                  select fth
                               );

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

        public List<FIN_TRX_HDR> GetfinTxtHdrListCRDR(FIN_TRX_HDR finTrxHdrObj, ServiceUtility utilityObj, string SubType)
        {
            List<FIN_TRX_HDR> finTrxHdrList = null;
            IQueryable<FIN_TRX_HDR> fintrxHdrQuery;
            //int pageSize;
            //int totalCount;
            try
            {
                if (!string.IsNullOrEmpty(utilityObj.SearchValue) || finTrxHdrObj.FTH_PK > 0)
                {
                    fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                      join crdr in this.currentEntity.FIN_CRDR_NOTE_HDR on fth.FTH_REF_PK equals crdr.CDH_PK
                                      where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE
                                          //&& fth.FTH_IS_DELETED == finTrxHdrObj.FTH_IS_DELETED
                                          && fth.FTH_REF_TYPE == (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_TYPE) ? finTrxHdrObj.FTH_REF_TYPE : fth.FTH_REF_TYPE)
                                          && fth.FTH_REF_PK == (finTrxHdrObj.FTH_REF_PK > 0 ? finTrxHdrObj.FTH_REF_PK : fth.FTH_REF_PK)
                                          && fth.FTH_PK == (finTrxHdrObj.FTH_PK > 0 ? finTrxHdrObj.FTH_PK : fth.FTH_PK)
                                          //&& fth.FTH_STATUS == (finTrxHdrObj.FTH_STATUS > 0 ? finTrxHdrObj.FTH_STATUS : fth.FTH_STATUS)
                                          //&& (utilityObj.FilterDate != null ? fth.FTH_DATE >= utilityObj.FilterDate : true)
                                          //&& (utilityObj.FilterToDate != null ? fth.FTH_DATE <= utilityObj.FilterToDate : true)
                                          //&& (utilityObj.NeedAdvanceFilter ? fth.FTH_IS_JRNLD == finTrxHdrObj.FTH_IS_JRNLD : true)
                                          //&& (SubType == ApplicationType.PI ? crdr.CDH_VENDOR > 0 : true)
                                          //&& (SubType == ApplicationType.SI ? crdr.CDH_CUSTOMER > 0 : true)
                                          //&& (fth.FTH_BIZUNIT == (finTrxHdrObj.FTH_BIZUNIT > 0 ? finTrxHdrObj.FTH_BIZUNIT : fth.FTH_BIZUNIT))
                                          && (utilityObj.SearchValue != null ? fth.FTH_VOUCHER_NO.Contains(utilityObj.SearchValue) : true)
                                          //&& fth.FTH_DATE >= (utilityObj.FilterDate == null ? fth.FTH_DATE : utilityObj.FilterDate)
                                          //&& fth.FTH_DATE <= (utilityObj.FilterToDate == null ? fth.FTH_DATE : utilityObj.FilterToDate)
                                      && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO) ? fth.FTH_REF_NO.Contains(finTrxHdrObj.FTH_REF_NO) : true)
                                      && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) ? fth.FTH_PARTY_NAME.Contains(finTrxHdrObj.FTH_PARTY_NAME) : true)
                                      select fth
                                   );
                }
                else
                {

                    fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                      join crdr in this.currentEntity.FIN_CRDR_NOTE_HDR on fth.FTH_REF_PK equals crdr.CDH_PK
                                      where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE
                                          && fth.FTH_IS_DELETED == finTrxHdrObj.FTH_IS_DELETED
                                          && fth.FTH_REF_TYPE == (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_TYPE) ? finTrxHdrObj.FTH_REF_TYPE : fth.FTH_REF_TYPE)
                                          && fth.FTH_REF_PK == (finTrxHdrObj.FTH_REF_PK > 0 ? finTrxHdrObj.FTH_REF_PK : fth.FTH_REF_PK)
                                          && fth.FTH_PK == (finTrxHdrObj.FTH_PK > 0 ? finTrxHdrObj.FTH_PK : fth.FTH_PK)
                                          && fth.FTH_STATUS == (finTrxHdrObj.FTH_STATUS > 0 ? finTrxHdrObj.FTH_STATUS : fth.FTH_STATUS)
                                          && (utilityObj.FilterDate != null ? fth.FTH_DATE >= utilityObj.FilterDate : true)
                                          && (utilityObj.FilterToDate != null ? fth.FTH_DATE <= utilityObj.FilterToDate : true)
                                          && (utilityObj.NeedAdvanceFilter ? fth.FTH_IS_JRNLD == finTrxHdrObj.FTH_IS_JRNLD : true)
                                          && (SubType == ApplicationType.PI ? crdr.CDH_VENDOR > 0 : true)
                                          && (SubType == ApplicationType.SI ? crdr.CDH_CUSTOMER > 0 : true)
                                          && (fth.FTH_BIZUNIT == (finTrxHdrObj.FTH_BIZUNIT > 0 ? finTrxHdrObj.FTH_BIZUNIT : fth.FTH_BIZUNIT))
                                          && (utilityObj.SearchValue != null ? fth.FTH_VOUCHER_NO.Contains(utilityObj.SearchValue) : true)
                                          && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO) ? fth.FTH_REF_NO.Contains(finTrxHdrObj.FTH_REF_NO) : true)
                                          && (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) ? fth.FTH_PARTY_NAME.Contains(finTrxHdrObj.FTH_PARTY_NAME) : true)
                                      //&& fth.FTH_DATE >= (utilityObj.FilterDate == null ? fth.FTH_DATE : utilityObj.FilterDate)
                                      //&& fth.FTH_DATE <= (utilityObj.FilterToDate == null ? fth.FTH_DATE : utilityObj.FilterToDate)
                                      select fth
                                   );
                }
                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = fintrxHdrQuery.Count();

                //Filter Query
                //fintrxHdrQuery = FilterEntity(finTrxHdrObj, fintrxHdrQuery, utilityObj);

                // Apply Paging And Sorting for grid Purpose
                finTrxHdrList = fintrxHdrQuery.SortRecords<FIN_TRX_HDR>(utilityObj).ToList();

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

        public int GenerateDummyEntry(int appID, string appType)
        {
            try
            {
                List<SPFIN_TRX_DUMMY_ENTRY_SAVE_Result> dummyObj = new List<SPFIN_TRX_DUMMY_ENTRY_SAVE_Result>();
                ObjectParameter paramReturn = new ObjectParameter("pRetVal", typeof(int));
                dummyObj = this.currentEntity.SPFIN_TRX_DUMMY_ENTRY_SAVE(appID, appType, paramReturn).ToList();
                return int.Parse(paramReturn.Value.ToString());
                //return dummyObj[0].Ret_Val;
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


        public List<FIN_TRX_HDR> GetPDCVoucherList(FIN_TRX_HDR finTrxHdrObj)
        {
            List<FIN_TRX_HDR> finTrxHdrList = null;
            IQueryable<FIN_TRX_HDR> fintrxHdrQuery;
            //int pageSize;
            //int totalCount;
            try
            {
                fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                  where fth.FTH_ACTIVE == finTrxHdrObj.FTH_ACTIVE
                                      && fth.FTH_REF_TYPE == finTrxHdrObj.FTH_REF_TYPE
                                      && fth.FTH_IS_DELETED == false
                                      && (fth.FTH_VOUCHER_NO == string.Empty || fth.FTH_VOUCHER_NO == null)
                                  select fth
                               );
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

        public List<FIN_TRX_HDR> GetVoucherSearchList(FIN_TRX_HDR finTrxHdrObj, ServiceUtility utilityObj)
        {
            List<FIN_TRX_HDR> finTrxHdrList = null;
            IQueryable<FIN_TRX_HDR> fintrxHdrQuery;
            try
            {
                if (!string.IsNullOrEmpty(utilityObj.SearchValue))
                {
                    fintrxHdrQuery = (from fth in this.currentEntity.FIN_TRX_HDR
                                      where fth.FTH_VOUCHER_NO == utilityObj.SearchValue
                                      select fth
                                     );
                    finTrxHdrList = fintrxHdrQuery.ToList();
                    return finTrxHdrList;
                }
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
    }
}
