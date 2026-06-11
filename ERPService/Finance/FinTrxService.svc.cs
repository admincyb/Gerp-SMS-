using System;
using System.Collections.Generic;
using System.Diagnostics;
using ERPData;
using ERPManager;
using System.Data;
using System.Linq;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FinTrxService" in code, svc and config file together.
    public class FinTrxService : IFinTrxService, IFinTrxManager
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public FinTrxService()
        {
            try
            {
                currentContext = new ERPEntities();
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Save Finance Details
        /// </summary>
        /// <param name="finTrxHdrList"></param>
        /// <returns></returns>
        public long SaveFinTrx(List<FIN_TRX_HDR> finTrxHdrList)
        {
            FinTrxHdrManager objFinTrxHdrManager;
            long? FinTrxHdrPK;
            try
            {
                objFinTrxHdrManager = new FinTrxHdrManager(currentContext);
                FinTrxHdrPK = objFinTrxHdrManager.SaveFinTrxHdr(finTrxHdrList);
                if (FinTrxHdrPK > 0)
                    currentContext.SaveChanges();
                return FinTrxHdrPK.Value;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxHdrManager = null;
            }
        }


        /// <summary>
        /// Save Finance Details
        /// </summary>
        /// <param name="finTrxHdrList"></param>
        /// <returns></returns>
        public long SaveDirectFinTrx(List<FIN_TRX_HDR> finTrxHdrList)
        {
            FinTrxHdrManager objFinTrxHdrManager;
            long? FinTrxHdrPK;
            try
            {
                objFinTrxHdrManager = new FinTrxHdrManager(currentContext);
                FinTrxHdrPK = objFinTrxHdrManager.SaveDirectFinTrxHdr(finTrxHdrList);
                if (FinTrxHdrPK > 0)
                    currentContext.SaveChanges();
                return FinTrxHdrPK.Value;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxHdrManager = null;
            }
        }
        /// <summary>
        /// Delete Finance Details
        /// </summary>
        /// <param name="JournalizePK"></param>
        /// <returns></returns>
        public long DeleteFinTrx(string REFTYPE, int REFPK, int FTHPK)
        {
            FinTrxHdrManager objFinTrxHdrManager;
            long? FinTrxHdrPK;
            try
            {
                objFinTrxHdrManager = new FinTrxHdrManager(currentContext);
                FinTrxHdrPK = objFinTrxHdrManager.DeleteFinTrx(REFTYPE, REFPK, FTHPK);
                currentContext.SaveChanges();
                return FinTrxHdrPK.Value;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxHdrManager = null;
            }
        }

        /// <summary>
        /// Delete Finance Details
        /// </summary>
        /// <param name="JournalizePK"></param>
        /// <returns></returns>
        public bool IsDummyEntry(string REFTYPE, int REFPK, int FTHPK)
        {
            FinTrxHdrManager objFinTrxHdrManager; 
            try
            {
                objFinTrxHdrManager = new FinTrxHdrManager(currentContext);
                return objFinTrxHdrManager.IsDummyEntry(REFTYPE, REFPK, FTHPK);

            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxHdrManager = null;
            }
        }


        /// <summary>
        /// Perform searching,filtering,sorting,and paging on AccoutPayables ;
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<FIN_TRX> GetAccoutPayables(long VendorID, ServiceUtility utilityObj)
        {
            FinTrxManager objFinTrxManager;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                return objFinTrxManager.GetAccoutPayables(VendorID, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
            }
        }

        public List<FIN_TRX> GetAccoutReceivables(long VendorID, ServiceUtility utilityObj)
        {
            FinTrxManager objFinTrxManager;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                return objFinTrxManager.GetAccoutReceivables(VendorID, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
            }
        }

        public int GetAccoutPayablesCount(long VendorID, ServiceUtility utilityObj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Perform searching,filtering,sorting,and paging on fin header ;
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<FIN_TRX_HDR> GetfinTxtHdrList(FIN_TRX_HDR finTrxHdrObj, ServiceUtility utilityObj)
        {
            List<FIN_TRX_HDR> finTrxHdrList = null;
            FinTrxHdrManager objFinTrxHdrManager;
            try
            {
                objFinTrxHdrManager = new FinTrxHdrManager(currentContext);
                return objFinTrxHdrManager.GetfinTxtHdrList(finTrxHdrObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxHdrManager = null;
            }
        }
        public List<FIN_TRX_HDR> GetSatusByAppPK(FIN_TRX_HDR finTrxHdrObj)
        {
            FinTrxHdrManager objFinTrxHdrManager;
            try
            {
                objFinTrxHdrManager = new FinTrxHdrManager(currentContext);
                return objFinTrxHdrManager.GetSatusByAppPK(finTrxHdrObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxHdrManager = null;
            }
        }

        public List<FIN_TRX_HDR> GetfinTxtHdrListByPK(FIN_TRX_HDR finTrxHdrObj)
        {
            List<FIN_TRX_HDR> finTrxHdrList = null;
            FinTrxHdrManager objFinTrxHdrManager;
            try
            {
                objFinTrxHdrManager = new FinTrxHdrManager(currentContext);
                return objFinTrxHdrManager.GetfinTxtHdrListByPK(finTrxHdrObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxHdrManager = null;
            }
        }

        /// <summary>
        /// Perform searching,filtering,sorting,and paging on fin header ;
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<FIN_TRX_HDR> GetfinTxtHdrListCRDR(FIN_TRX_HDR finTrxHdrObj, ServiceUtility utilityObj, string SubType)
        {
            List<FIN_TRX_HDR> finTrxHdrList = null;
            FinTrxHdrManager objFinTrxHdrManager;
            try
            {
                objFinTrxHdrManager = new FinTrxHdrManager(currentContext);
                return objFinTrxHdrManager.GetfinTxtHdrListCRDR(finTrxHdrObj, utilityObj, SubType);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxHdrManager = null;
            }
        }

        public long SaveFinTrx(List<FIN_TRX> finTrxList)
        {
            throw new NotImplementedException();
        }

        public int UpdateBankReconciliation(List<FIN_TRX> finTrxList)
        {
            FinTrxManager objFinTrxManager;
            int? FinTrxPK;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                FinTrxPK = objFinTrxManager.UpdateBankReconciliation(finTrxList);
                currentContext.SaveChanges();
                return FinTrxPK.Value;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
            }
        }

        public List<FIN_TRX> GetAccoutPayablesList(long ID, string Type, ServiceUtility utilityObj)
        {
            List<FIN_TRX> finTrxList = null;
            FinTrxManager objFinTrxManager;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                return objFinTrxManager.GetAccoutPayablesList(ID, Type, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
            }
        }

        public List<FIN_YEAR_MST> GetCurrentFinPeriod(DateTime TransDate, Int32 BizUnit)
        {
            FinYearMstManager finYearMstManagerObj;
            try
            {
                finYearMstManagerObj = new FinYearMstManager(currentContext);
                return finYearMstManagerObj.GetCurrentFinPeriod(TransDate, BizUnit);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                finYearMstManagerObj = null;
            }
        }

        public List<FIN_TRX> GetBankReconcileList(FIN_TRX finTrxObj, int isReconciled, ServiceUtility utilityObj, ref decimal TotalDebit, ref decimal TotalCredit)
        {
            FinTrxManager objFinTrxManager;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                return objFinTrxManager.GetBankReconcileList(finTrxObj, isReconciled, utilityObj,ref TotalDebit, ref TotalCredit);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
            }
        }

        public int GetFinType(string type, int refPK)
        {
            FinTrxManager objFinTrxManager;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                return objFinTrxManager.GetFinType(type, refPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
            }
        }
        #endregion



        public bool CheckForVoucherNoDuplication(int currPK, string voucherNo)
        {
            FinTrxManager objFinTrxManager;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                return objFinTrxManager.CheckForVoucherNoDuplication(currPK, voucherNo);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
            }
        }

        public int GenerateDummyEntry(int appID, string appType)
        {
            FinTrxHdrManager objFinTrxHdrManager;
            try
            {
                objFinTrxHdrManager = new FinTrxHdrManager(currentContext);
                return objFinTrxHdrManager.GenerateDummyEntry(appID, appType);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxHdrManager = null;
            }
        }

        /// <summary>
        /// Perform searching,filtering,sorting,and paging on fin header ;
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<FIN_TRX_HDR> GetPDCVoucherList(FIN_TRX_HDR finTrxHdrObj)
        {
            FinTrxHdrManager objFinTrxHdrManager;
            try
            {
                objFinTrxHdrManager = new FinTrxHdrManager(currentContext);
                return objFinTrxHdrManager.GetPDCVoucherList(finTrxHdrObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxHdrManager = null;
            }
        }


        public List<FIN_PAYMENT_VND_TAX_HDR> GetfinPymntVndTxtHdrList(FIN_PAYMENT_VND_TAX_HDR finTrxHdrObj, ServiceUtility serviceUtilityObj)
        {
            FinTrxManager objFinTrxManager;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                return objFinTrxManager.GetfinPymntVndTxtHdrList(finTrxHdrObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="finTrxHdrObj"></param>
        /// <returns></returns>
        public bool IsRefereceNoEsixt(FIN_TRX_HDR finTrxHdrObj)
        {
            FinTrxManager objFinTrxManager;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                return objFinTrxManager.IsRefereceNoEsixt(finTrxHdrObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
            }
        }

        /// <summary>
        /// Check for receipt voucher is created or not
        /// </summary>
        /// <param name="finTrxHdrObj"></param>
        /// <returns></returns>
        public bool CheckForReceiptVoucherCreated(FIN_TRX_HDR finTrxHdrObj)
        {
            FinTrxManager objFinTrxManager;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                return objFinTrxManager.CheckForReceiptVoucherCreated(finTrxHdrObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
            }
        }

        public bool CheckForYearendVoucherCreated(FIN_TRX_HDR finTrxHdrObj)
        {
            FinTrxManager objFinTrxManager;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                return objFinTrxManager.CheckForYearendVoucherCreated(finTrxHdrObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
            }
        }

        /// <summary>
        /// Update Direct Payment Hdr Jounalize Flag
        /// </summary>
        /// <param name="finInvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateDirectPaymentHdrPDC(int DirPaymentPK, byte pdcFlag)
        {
            FinTrxManager objFinTrxManager;
            long? finDirectPaymentHdrPK;
            try
            {
                objFinTrxManager = new FinTrxManager(currentContext);
                finDirectPaymentHdrPK = objFinTrxManager.UpdateDirectPaymentHdrPDC(DirPaymentPK, pdcFlag);
                currentContext.SaveChanges();
                return finDirectPaymentHdrPK.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxManager = null;
                finDirectPaymentHdrPK = null;
            }
        }

        public DateTime? ChangeTaxDate(FIN_TRX_HDR finTrxObjParam)
        {
            try
            {
                
                FIN_TRX_HDR finTraHdrObj = currentContext.FIN_TRX_HDR.SingleOrDefault(x => x.FTH_PK == finTrxObjParam.FTH_PK);
                                                    /*&& x.FTH_MOD_DT == finTrxObjParam.FTH_MOD_DT => Sometimes Millisecond changed while getting LastModDt From ViewState */
                if (finTraHdrObj.FTH_MOD_DT.ToString("dd-MM-yyyy hh:mm:ss") != finTrxObjParam.FTH_MOD_DT.ToString("dd-MM-yyyy hh:mm:ss"))
                    throw new OptimisticConcurrencyException("Already Edited By Another User");
               
                DateTime currDate = DateTime.Now;
                finTraHdrObj.FTH_TAX_DATE = finTrxObjParam.FTH_TAX_DATE;
                finTraHdrObj.FTH_MOD_DT = currDate;
                finTraHdrObj.FTH_MOD_BY = finTrxObjParam.FTH_MOD_BY;
                this.currentContext.SaveChanges();
                return currDate;//finTraHdrObj.FTH_MOD_DT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FIN_TRX_HDR> GetVoucherSearchList(FIN_TRX_HDR finTrxHdrObj, ServiceUtility utilityObj)
        {            
            FinTrxHdrManager objFinTrxHdrManager;
            try
            {
                objFinTrxHdrManager = new FinTrxHdrManager(currentContext);
                return objFinTrxHdrManager.GetVoucherSearchList(finTrxHdrObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                objFinTrxHdrManager = null;
            }
        }
    }
}
