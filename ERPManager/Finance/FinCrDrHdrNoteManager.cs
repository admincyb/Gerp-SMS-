using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data.Objects;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using ERPManager.Finance;
using BusinessObject.CommonManagement;

namespace ERPManager
{
    public class FinCrDrHdrNoteManager : IFinCrDrHdrNoteManager
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
        public FinCrDrHdrNoteManager(ERPEntities currentEntity)
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
        /// Get CrDr Note Header Details
        /// </summary>
        /// <param name="finCrDrNoteHdrObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_CRDR_NOTE_HDR> GetCrDrNoteVndHdr(FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj, ServiceUtility serviceUtilityObj, int Type,string invoiceNo, int? Status = null)
        {
            IQueryable<FIN_CRDR_NOTE_HDR> paymentHdrQuery;
            List<FIN_CRDR_NOTE_HDR> finPaymentVndHdrListObj;
            int pageSize;
            try
            {
                pageSize = Convert.ToInt32(serviceUtilityObj.PageSize);
                //Selecting
                if (finCrDrNoteHdrObj.CDH_PK > 0)
                {
                    paymentHdrQuery
                                = (from cdh in this.currentEntity.FIN_CRDR_NOTE_HDR.Include("FIN_CRDR_NOTE_TAX_HDR")
                                   //join pty in this.currentEntity.PUR_VENDOR_MST on cdh.CDH_VENDOR equals pty.VEN_PK
                                   where cdh.CDH_ACTIVE == finCrDrNoteHdrObj.CDH_ACTIVE
                                       //&& cdh.CDH_DATE >= (serviceUtilityObj.FilterDate == DateTime.MinValue ? cdh.CDH_DATE : serviceUtilityObj.FilterDate)
                                       //&& cdh.CDH_DATE <= (serviceUtilityObj.FilterToDate == DateTime.MinValue ? cdh.CDH_DATE : serviceUtilityObj.FilterToDate)
                                      && cdh.CDH_PK == finCrDrNoteHdrObj.CDH_PK
                                       //&& (finCrDrNoteHdrObj.CDH_VENDOR > 0 ? cdh.CDH_VENDOR == finCrDrNoteHdrObj.CDH_VENDOR : true)
                                       //&& (finCrDrNoteHdrObj.CDH_CUSTOMER > 0 ? cdh.CDH_CUSTOMER == finCrDrNoteHdrObj.CDH_CUSTOMER : true)
                                      && (Type == 1 ? cdh.CDH_VENDOR > 0 : true)
                                      && (Type == 2 ? cdh.CDH_CUSTOMER > 0 : true)
                                       //&& (Status == 0 ? cdh.CDH_HAS_JRNL_ENTRY == false : (Status == 1 ? cdh.CDH_HAS_JRNL_ENTRY == true : (Status == 2 ? cdh.CDH_STATUS != 2 : true)))
                                      && (cdh.CDH_STATUS != 0 || finCrDrNoteHdrObj.CDH_CRTD_BY == 0 || cdh.CDH_CRTD_BY == finCrDrNoteHdrObj.CDH_CRTD_BY)
                                   select cdh
                                  );
                }
                else
                {
                    if (finCrDrNoteHdrObj.CDH_PK == 0 && finCrDrNoteHdrObj.CDH_VENDOR == 0 && finCrDrNoteHdrObj.CDH_CUSTOMER==0)
                    {
                        paymentHdrQuery
                                   = (from cdh in this.currentEntity.FIN_CRDR_NOTE_HDR
                                      //join pty in this.currentEntity.PUR_VENDOR_MST on cdh.CDH_VENDOR equals pty.VEN_PK
                                      where cdh.CDH_ACTIVE == finCrDrNoteHdrObj.CDH_ACTIVE
                                         && cdh.CDH_DATE >= (serviceUtilityObj.FilterDate == DateTime.MinValue ? cdh.CDH_DATE : serviceUtilityObj.FilterDate)
                                         && cdh.CDH_DATE <= (serviceUtilityObj.FilterToDate == DateTime.MinValue ? cdh.CDH_DATE : serviceUtilityObj.FilterToDate)
                                         && (Type == 1 ? cdh.CDH_VENDOR > 0 : true)
                                         && (Type == 2 ? cdh.CDH_CUSTOMER > 0 : true)
                                         && (Status == 0 ? cdh.CDH_HAS_JRNL_ENTRY == false : (Status == 1 ? cdh.CDH_HAS_JRNL_ENTRY == true : (Status == 2 ? cdh.CDH_STATUS == 0 : (Status == 4 ? cdh.CDH_IS_DELETED : true))))
                                         && (Status == 2 || cdh.CDH_STATUS != 0 || finCrDrNoteHdrObj.CDH_CRTD_BY == 0 || cdh.CDH_CRTD_BY == finCrDrNoteHdrObj.CDH_CRTD_BY)
                                         && (Status == 3 ? cdh.CDH_IS_DELETED == false : Status == 4 ? cdh.CDH_IS_DELETED == true : cdh.CDH_IS_DELETED == false)
                                         && (Type == 1 ? (invoiceNo == string.Empty ? true : cdh.FIN_CRDR_NOTE_MPG.Any(act => act.FIN_INVOICE_VND_HDR.IVH_NO.Contains(invoiceNo))) : true)
                                         && (Type == 2 ? (invoiceNo == string.Empty ? true : cdh.FIN_CRDR_NOTE_MPG.Any(act => act.FIN_INVOICE_CUS_HDR.ICH_NO.Contains(invoiceNo))) : true)
                                         && (cdh.CDH_BIZUNIT == (finCrDrNoteHdrObj.CDH_BIZUNIT > 0 ? finCrDrNoteHdrObj.CDH_BIZUNIT : cdh.CDH_BIZUNIT))
                                         && (cdh.CDH_STATUS == 0 ? cdh.CDH_CRTD_BY == finCrDrNoteHdrObj.CDH_CRTD_BY : true)//Filter for drafted records only for creator
                                         && (finCrDrNoteHdrObj.CDH_TYPE > 0 ? cdh.CDH_TYPE == finCrDrNoteHdrObj.CDH_TYPE : true)
                                         && (finCrDrNoteHdrObj.CDH_COMPANY > 0 ? cdh.CDH_COMPANY == finCrDrNoteHdrObj.CDH_COMPANY : true)
                                      select cdh
                                     );
                    }
                    else
                    {
                    paymentHdrQuery
                                = (from cdh in this.currentEntity.FIN_CRDR_NOTE_HDR
                                   //join pty in this.currentEntity.PUR_VENDOR_MST on cdh.CDH_VENDOR equals pty.VEN_PK
                                   where cdh.CDH_ACTIVE == finCrDrNoteHdrObj.CDH_ACTIVE
                                      && cdh.CDH_DATE >= (serviceUtilityObj.FilterDate == DateTime.MinValue ? cdh.CDH_DATE : serviceUtilityObj.FilterDate)
                                      && cdh.CDH_DATE <= (serviceUtilityObj.FilterToDate == DateTime.MinValue ? cdh.CDH_DATE : serviceUtilityObj.FilterToDate)
                                      && cdh.CDH_PK == (finCrDrNoteHdrObj.CDH_PK > 0 ? finCrDrNoteHdrObj.CDH_PK : cdh.CDH_PK)
                                      && (finCrDrNoteHdrObj.CDH_VENDOR > 0 ? cdh.CDH_VENDOR == finCrDrNoteHdrObj.CDH_VENDOR : true)
                                      && (finCrDrNoteHdrObj.CDH_CUSTOMER > 0 ? cdh.CDH_CUSTOMER == finCrDrNoteHdrObj.CDH_CUSTOMER : true)
                                      && (Type == 1 ? cdh.CDH_VENDOR > 0 : true)
                                      && (Type == 2 ? cdh.CDH_CUSTOMER > 0 : true)
                                      && (Status == 0 ? cdh.CDH_HAS_JRNL_ENTRY == false : (Status == 1 ? cdh.CDH_HAS_JRNL_ENTRY == true : (Status == 2 ? cdh.CDH_STATUS == 0 : (Status == 4 ? cdh.CDH_IS_DELETED : true))))
                                      && (Status==2 || cdh.CDH_STATUS != 0 || finCrDrNoteHdrObj.CDH_CRTD_BY == 0 || cdh.CDH_CRTD_BY == finCrDrNoteHdrObj.CDH_CRTD_BY)
                                      && (Status == 3 ? cdh.CDH_IS_DELETED == false : Status == 4 ? cdh.CDH_IS_DELETED == true : cdh.CDH_IS_DELETED == false) 
                                      && (Type == 1 ? (invoiceNo == string.Empty ? true : cdh.FIN_CRDR_NOTE_MPG.Any(act => act.FIN_INVOICE_VND_HDR.IVH_NO.Contains(invoiceNo))) : true)
                                      && (Type == 2 ? (invoiceNo == string.Empty ? true : cdh.FIN_CRDR_NOTE_MPG.Any(act => act.FIN_INVOICE_CUS_HDR.ICH_NO.Contains(invoiceNo))) : true)
                                      && (cdh.CDH_BIZUNIT == (finCrDrNoteHdrObj.CDH_BIZUNIT > 0 ? finCrDrNoteHdrObj.CDH_BIZUNIT : cdh.CDH_BIZUNIT))
                                      && (cdh.CDH_STATUS == 0 ? cdh.CDH_CRTD_BY == finCrDrNoteHdrObj.CDH_CRTD_BY : true)//Filter for drafted records only for creator
                                      && (finCrDrNoteHdrObj.CDH_COMPANY > 0 ? cdh.CDH_COMPANY == finCrDrNoteHdrObj.CDH_COMPANY : true)
                                      && (finCrDrNoteHdrObj.CDH_TYPE > 0 ? cdh.CDH_TYPE == finCrDrNoteHdrObj.CDH_TYPE : true)
                                   select cdh
                                  );
                    }
                    if (serviceUtilityObj.InvoiceType > 0) //For domestic or export type filtration, from base ie from sales contract !
                    { 
                       paymentHdrQuery=(from p in paymentHdrQuery
                                        join mpg in this.currentEntity.FIN_CRDR_NOTE_MPG on 
                                        p.CDH_PK equals mpg.CDM_CRDR_NOTE_HDR
                                        join crhdr in this.currentEntity.FIN_INVOICE_CUS_DTL on
                                        mpg.CDM_INVOICE_CUS_HDR equals crhdr.CID_INVOICE_HDR
                                        join sldtl in this.currentEntity.SAL_ORDER_DTL on
                                        crhdr.CID_SO_DTL equals sldtl.SOD_PK
                                        join sohdr in this.currentEntity.SAL_ORDER_HDR on
                                        sldtl.SOD_SO equals sohdr.SOH_PK
                                        where sohdr.SOH_TYPE==serviceUtilityObj.InvoiceType select p).Distinct();
                    }
                }

                //Get total row count
                serviceUtilityObj.TotalRecords = paymentHdrQuery.GroupBy(p=> p.CDH_PK).Count();
                //-------------------------------

                //Set page size one if not given
                serviceUtilityObj.PageSize = serviceUtilityObj.PageSize == 0 ? 1 : serviceUtilityObj.PageSize;

                //Filter Query

                //paymentHdrQuery = FilterEntity(finCrDrNoteHdrObj, paymentHdrQuery, serviceUtilityObj);

                #region Sorting
                // Apply Paging And Sorting For AD_TAX_GROUPS_MST grid Purpose
                // Checking sorting criteria is given

                #endregion
                //return Tax master details;
                finPaymentVndHdrListObj = paymentHdrQuery.SortRecords<FIN_CRDR_NOTE_HDR>(serviceUtilityObj).ToList();
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
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finCrDrNotePK"></param>
        /// <returns></returns>
        public List<FIN_CRDR_NOTE_HDR> GetCrDrNoteHdr(long finCrDrNotePK)
        {
            List<FIN_CRDR_NOTE_HDR> FinPaymentHdrListObj = new List<FIN_CRDR_NOTE_HDR>();
            try
            {
                FinPaymentHdrListObj = (from cdh in this.currentEntity.FIN_CRDR_NOTE_HDR
                                        //join pty in this.currentEntity.PUR_VENDOR_MST on cdh.CDH_VENDOR equals pty.VEN_PK
                                        where cdh.CDH_PK == finCrDrNotePK
                                        select cdh

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
        public List<FIN_CRDR_NOTE_HDR> GetCustomerCrDrNoteHdr(long finCrDrNotePK)
        {
            List<FIN_CRDR_NOTE_HDR> FinPaymentHdrListObj = new List<FIN_CRDR_NOTE_HDR>();
            try
            {
                FinPaymentHdrListObj = (from cdh in this.currentEntity.FIN_CRDR_NOTE_HDR
                                        join pty in this.currentEntity.CRM_CUSTOMER_MST on cdh.CDH_CUSTOMER equals pty.CUS_PK
                                        where cdh.CDH_PK == finCrDrNotePK
                                        select cdh

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
        /// 
        /// </summary>
        /// <param name="finCrDrNoteHdrList"></param>
        /// <returns></returns>
        public long? SaveCrDrNoteHdr(System.Collections.Generic.List<FIN_CRDR_NOTE_HDR> finCrDrNoteHdrList, bool isWkfSave)
        {
            //Holds save status
            long retval;
            FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj;
            List<FIN_CRDR_NOTE_MPG> finCrDrNoteMpgList;
            FinCrDrHdrNoteMpgManager FinCrDrHdrNoteMpgManagerObj;
            FinCrDrNoteTaxHdrManager finCrDrNoteTaxHdrManager;

            List<FIN_CRDR_NOTE_TAX_HDR> finCrDrNoteTaxHdrList;
        
            FinTrxManager FinTrxManagerobj;
            FinYearMstManager FinYearMstManagerObj = new FinYearMstManager(this.currentEntity);

            CommonFunctionsManager ComnFnManagerObj = new CommonFunctionsManager(this.currentEntity);
            //Save log
            List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();

            int? maxCrDrPk;
            try
            {
                //Set save status zero,save failed
                retval = 0;
                //Iterate through shift attendance list for save
                foreach (FIN_CRDR_NOTE_HDR finCrDrNoteObj in finCrDrNoteHdrList)
                {
                    ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();

                    finCrDrNoteMpgList = finCrDrNoteObj.FIN_CRDR_NOTE_MPG.ToList();
                    finCrDrNoteObj.FIN_CRDR_NOTE_MPG.Clear();

                    finCrDrNoteTaxHdrList = finCrDrNoteObj.FIN_CRDR_NOTE_TAX_HDR.ToList();
                    finCrDrNoteObj.FIN_CRDR_NOTE_TAX_HDR.Clear();

                    if (finCrDrNoteObj.CDH_PK == 0)
                    {

                        #region New Record

                        AdmTrxLogDet.ATL_ACTION = (byte)LogAction.NEW;
                        //check for whether a vendor for the same shift and date already saved or not
                        // Gets last FIN_CRDR_NOTE_HDR pk
                        maxCrDrPk = this.currentEntity.FIN_CRDR_NOTE_HDR.Max(cdh => (int?)cdh.CDH_PK);

                        // Sets return value as next FIN_CRDR_NOTE_HDR pk
                        retval = Convert.ToInt16((maxCrDrPk.HasValue ? maxCrDrPk.Value + 1 : 1));

                        // Sets next FIN_CRDR_NOTE_HDR pk
                        finCrDrNoteObj.CDH_PK = retval;

                        // Sets FIN_CRDR_NOTE_HDR created date time as current date time
                        finCrDrNoteObj.CDH_CRTD_DT = DateTime.Now;

                        // Sets FIN_CRDR_NOTE_HDR modified date time as current date time
                        finCrDrNoteObj.CDH_MOD_DT = DateTime.Now;

                        //Normal transaction=1,trading=2
                        finCrDrNoteObj.CDH_TRX_TYPE = 1;

                        // Add new FIN_CRDR_NOTE_HDR to the db context
                        this.currentEntity.FIN_CRDR_NOTE_HDR.AddObject(finCrDrNoteObj);
                        //set the FIN_CRDR_NOTE_HDR pk as the fk of  CAM_SHIFT_ATTENDANCE_Dtl
                        finCrDrNoteMpgList.ForEach(dtl => dtl.CDM_CRDR_NOTE_HDR = retval);

                        finCrDrNoteTaxHdrList.ForEach(dt1 => dt1.NTH_CRDR_NOTE_HDR = retval);

                        finCrDrNoteTaxHdrManager = new FinCrDrNoteTaxHdrManager(this.currentEntity);
                        finCrDrNoteTaxHdrManager.SaveCrDrNoteTaxHdr(finCrDrNoteTaxHdrList, retval);

                        FinCrDrHdrNoteMpgManagerObj = new FinCrDrHdrNoteMpgManager(this.currentEntity);

                       
                        //Save Shift Attendance Details
                        if (isWkfSave)
                        {
                            FinCrDrHdrNoteMpgManagerObj.SaveFinCrDrNoteMpg(finCrDrNoteMpgList, finCrDrNoteObj.CDH_TYPE);
                        }
                        else
                        {
                            FinCrDrHdrNoteMpgManagerObj.SaveFinCrDrNoteMpg(finCrDrNoteMpgList);
                        }                        
                        
                        //Save Fin Trx against Vendor coa
                        FinTrxManagerobj = new FinTrxManager(this.currentEntity);
                        #endregion
                    }
                    else
                    {
                        // updating FIN_CRDR_NOTE_HDR
                        // Get current FIN_CRDR_NOTE_HDR using FIN_CRDR_NOTE_HDR pk and last modified date time,used for concurrency checking
                        finCrDrNoteHdrObj = currentEntity.FIN_CRDR_NOTE_HDR.SingleOrDefault(sah => sah.CDH_PK == finCrDrNoteObj.CDH_PK && sah.CDH_MOD_DT == finCrDrNoteObj.CDH_MOD_DT);
                        // If finCrDrNoteHdrObj is null then,anyone modified or deleted the record
                        if (finCrDrNoteHdrObj != null)
                        {
                            // Update FIN_CRDR_NOTE_HDR

                            AdmTrxLogDet.ATL_ACTION = (byte)LogAction.UPDATE;
                            finCrDrNoteHdrObj.CDH_TYPE = finCrDrNoteObj.CDH_TYPE;
                            finCrDrNoteHdrObj.CDH_NO = finCrDrNoteObj.CDH_NO;
                            finCrDrNoteHdrObj.CDH_DATE = finCrDrNoteObj.CDH_DATE;
                            finCrDrNoteHdrObj.CDH_VENDOR = finCrDrNoteObj.CDH_VENDOR;
                            finCrDrNoteHdrObj.CDH_CUSTOMER = finCrDrNoteObj.CDH_CUSTOMER;
                            finCrDrNoteHdrObj.CDH_VND_CUS_ACCOUNT = finCrDrNoteObj.CDH_VND_CUS_ACCOUNT;
                            finCrDrNoteHdrObj.CDH_CURRENCY = finCrDrNoteObj.CDH_CURRENCY;
                            finCrDrNoteHdrObj.CDH_AMOUNT_TC = finCrDrNoteObj.CDH_AMOUNT_TC;
                            finCrDrNoteHdrObj.CDH_BASE_CURR = finCrDrNoteObj.CDH_BASE_CURR;
                            finCrDrNoteHdrObj.CDH_EXCHG_RATE = finCrDrNoteObj.CDH_EXCHG_RATE;
                            finCrDrNoteHdrObj.CDH_AMOUNT_BC = finCrDrNoteObj.CDH_AMOUNT_BC;
                            finCrDrNoteHdrObj.CDH_REMARKS = finCrDrNoteObj.CDH_REMARKS;
                            finCrDrNoteHdrObj.CDH_REMARKS2 = finCrDrNoteObj.CDH_REMARKS2;
                            finCrDrNoteHdrObj.CDH_ACTIVE = finCrDrNoteObj.CDH_ACTIVE;
                            finCrDrNoteHdrObj.CDH_MOD_BY = finCrDrNoteObj.CDH_MOD_BY;
                            finCrDrNoteHdrObj.CDH_MOD_DT = DateTime.Now;
                            //finCrDrNoteHdrObj.CDH_STATUS = finCrDrNoteObj.CDH_STATUS;
                            finCrDrNoteHdrObj.CDH_REF_NO = finCrDrNoteObj.CDH_REF_NO;
                            finCrDrNoteHdrObj.CDH_REF_DATE = finCrDrNoteObj.CDH_REF_DATE;
                            finCrDrNoteHdrObj.CDH_TAX_AMOUNT = finCrDrNoteObj.CDH_TAX_AMOUNT;
                            finCrDrNoteHdrObj.CDH_COMPANY = finCrDrNoteObj.CDH_COMPANY;
                            finCrDrNoteHdrObj.CDH_SHIP_CHARGE = finCrDrNoteObj.CDH_SHIP_CHARGE;
                            finCrDrNoteHdrObj.CDH_OTHER_CHARGE = finCrDrNoteObj.CDH_OTHER_CHARGE;
                            finCrDrNoteHdrObj.CDH_IS_DELETED = finCrDrNoteObj.CDH_IS_DELETED;   // Sets return value as FIN_CRDR_NOTE_HDR pk
                            finCrDrNoteHdrObj.CDH_IMP_DECL_NO = finCrDrNoteObj.CDH_IMP_DECL_NO;
                            finCrDrNoteHdrObj.CDH_IS_AFFECT_STK = finCrDrNoteObj.CDH_IS_AFFECT_STK;
                            finCrDrNoteHdrObj.CDH_NO_RCP_ALLOC = finCrDrNoteObj.CDH_NO_RCP_ALLOC;
                            //Normal transaction=1,trading=2
                            finCrDrNoteHdrObj.CDH_TRX_TYPE = 1;

                            retval = finCrDrNoteObj.CDH_PK;

                            finCrDrNoteTaxHdrManager = new FinCrDrNoteTaxHdrManager(this.currentEntity);
                            finCrDrNoteTaxHdrManager.SaveCrDrNoteTaxHdr(finCrDrNoteTaxHdrList, retval);
                            
                            FinCrDrHdrNoteMpgManagerObj = new FinCrDrHdrNoteMpgManager(this.currentEntity);
                            //Save FinCrDrNoteMpg Details
                            if (isWkfSave || finCrDrNoteHdrObj.CDH_STATUS > 0)
                            {
                                FinCrDrHdrNoteMpgManagerObj.SaveFinCrDrNoteMpg(finCrDrNoteMpgList, finCrDrNoteHdrObj.CDH_TYPE);
                            }
                            else
                            {
                                FinCrDrHdrNoteMpgManagerObj.SaveFinCrDrNoteMpg(finCrDrNoteMpgList);
                            }
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }


                    AdmTrxLogDet.ATL_APP_TRX_CODE = finCrDrNoteObj.CDH_NO;
                    if (finCrDrNoteObj.CDH_TYPE == (int)DebitCreditModeEnum.DEBIT)
                    {
                        AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.DN;
                    }
                    else if (finCrDrNoteObj.CDH_TYPE == (int)DebitCreditModeEnum.CREDIT)
                    {
                        AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.CN;
                    }                       
                    AdmTrxLogDet.ATL_MOD_BY = finCrDrNoteObj.CDH_MOD_BY;
                    AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                    AdmTrxLogDet.ATL_BIZUNIT = finCrDrNoteObj.CDH_BIZUNIT;
                    AdmTrxLogDet.ATL_APP_TRX_PK = finCrDrNoteObj.CDH_PK;
                    AdmTrxLogDet.ATL_PK = 0;
                    AdmTrxLogList.Add(AdmTrxLogDet);

                }
                ComnFnManagerObj.SaveLog(AdmTrxLogList);
                //return FIN_CRDR_NOTE_HDR pk
                return retval;
            }
            /* catch (OptimisticConcurrencyException ex)
             {
                 //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                 throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
             }*/
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finCrDrNoteObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_CRDR_NOTE_HDR> GetCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj)
        {

            List<FIN_CRDR_NOTE_HDR> FinCrDrNoteHdrListObj = new List<FIN_CRDR_NOTE_HDR>();
            try
            {
                FinCrDrNoteHdrListObj = (from cdh in this.currentEntity.FIN_CRDR_NOTE_HDR
                                         where cdh.CDH_NO.StartsWith(serviceUtilityObj.FilterValue) && cdh.CDH_STATUS == 2
                                         select cdh
                ).ToList();
            }
            catch
            {
            }
            return FinCrDrNoteHdrListObj;
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="finCrDrNoteObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_CRDR_NOTE_HDR> GetSalCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj)
        {

            List<FIN_CRDR_NOTE_HDR> FinCrDrNoteHdrListObj = new List<FIN_CRDR_NOTE_HDR>();
            try
            {
                FinCrDrNoteHdrListObj = (from cdh in this.currentEntity.FIN_CRDR_NOTE_HDR
                                         where cdh.CDH_NO.Contains(serviceUtilityObj.FilterValue) && cdh.CDH_STATUS > 0 && cdh.CDH_CUSTOMER > 0 && !string.IsNullOrEmpty(cdh.CDH_NO)
                                         select cdh
                ).ToList();
                FinCrDrNoteHdrListObj = FinCrDrNoteHdrListObj.OrderBy(odr => odr.CDH_NO).ToList();
            }
            catch
            {
            }
            return FinCrDrNoteHdrListObj;
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="finCrDrNoteObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<FIN_CRDR_NOTE_HDR> GetPurCrDrNoteAutoCompleteList(FIN_CRDR_NOTE_HDR finCrDrNoteObj, ServiceUtility serviceUtilityObj)
        {

            List<FIN_CRDR_NOTE_HDR> FinCrDrNoteHdrListObj = new List<FIN_CRDR_NOTE_HDR>();
            try
            {
                FinCrDrNoteHdrListObj = (from cdh in this.currentEntity.FIN_CRDR_NOTE_HDR
                                         where cdh.CDH_NO.StartsWith(serviceUtilityObj.FilterValue) && cdh.CDH_STATUS > 0
                                         && cdh.CDH_BIZUNIT == finCrDrNoteObj.CDH_BIZUNIT
                                         && cdh.CDH_VENDOR > 0 && !string.IsNullOrEmpty(cdh.CDH_NO)
                                         select cdh
                ).ToList();
                FinCrDrNoteHdrListObj = FinCrDrNoteHdrListObj.OrderBy(odr => odr.CDH_NO).ToList();
            }
            catch
            {
            }
            return FinCrDrNoteHdrListObj;
            throw new NotImplementedException();
        }

        public List<FIN_CRDR_NOTE_HDR> GetCrDrNoteCusHdr(FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj, ServiceUtility serviceUtilityObj)
        {
            throw new NotImplementedException();
        }

        private static IQueryable<FIN_CRDR_NOTE_HDR> FilterEntity(FIN_CRDR_NOTE_HDR finCrDrNoteObj, IQueryable<FIN_CRDR_NOTE_HDR> qry, ServiceUtility utilityObj)
        {
            #region Filtering
            //if (finCrDrNoteObj.CDH_PK != 0)
            //    qry = qry.Where(cdh => cdh.CDH_PK == finCrDrNoteObj.CDH_PK);

            /*
            if (finCrDrNoteObj.CDH_BIZUNIT != -1)
            {
                qry = qry.Where(cdh => cdh.CDH_BIZUNIT == finCrDrNoteObj.CDH_BIZUNIT);
            }
            if (finCrDrNoteObj.CDH_PK < 1 && finCrDrNoteObj.CDH_ACTIVE == 1) //select all active records
                qry = qry.Where(cdh => cdh.CDH_ACTIVE == 1);
            else if (finCrDrNoteObj.CDH_PK > 0 && finCrDrNoteObj.CDH_ACTIVE == 1) // select all active +(union) having given pk
                qry = qry.Where(cdh => cdh.CDH_ACTIVE == finCrDrNoteObj.CDH_ACTIVE || cdh.CDH_PK == finCrDrNoteObj.TXG_PK);
            */

            //if (finCrDrNoteObj.CDH_VENDOR != 0)
            //    qry = qry.Where(cdh => cdh.CDH_VENDOR == finCrDrNoteObj.CDH_VENDOR);

            //// Filter by fault logged Date Range
            //if (utilityObj.FilterDate.HasValue == true && utilityObj.FilterToDate.HasValue == true )
            //{
            //    qry = qry.Where(cdh => cdh.CDH_DATE >= utilityObj.FilterDate && cdh.CDH_DATE <= utilityObj.FilterToDate);
            //}

            #endregion
            // return qry;
            return null;
        }


        /// <summary>
        /// Update Cr Dr Hdr Jounalize Flag
        /// </summary>
        /// <param name="InvoiceHdrList"></param>
        /// <returns></returns>
        public long UpdateCrDrHdrJounalizeFlag(int CrDrPK, bool JounalizeFlag)
        {
            long retval = 0;
            FIN_CRDR_NOTE_HDR OldFIN_CRDR_NOTE_HDR_Obj;

            try
            {
                retval = 0;
                OldFIN_CRDR_NOTE_HDR_Obj = currentEntity.FIN_CRDR_NOTE_HDR.SingleOrDefault(sah => sah.CDH_PK == CrDrPK);
                if (OldFIN_CRDR_NOTE_HDR_Obj != null)
                {
                    OldFIN_CRDR_NOTE_HDR_Obj.CDH_HAS_JRNL_ENTRY = JounalizeFlag;

                    retval = CrDrPK;
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
        public long DeleteCrDrNoteHdr(long crdrPK, int CrDrType)
        {
            long retval = 0;

            try
            {

                List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                CommonFunctionsManager ComnFnManagerObj = new CommonFunctionsManager(this.currentEntity);
                FIN_CRDR_NOTE_HDR FIN_CRDR_NOTE_HDR_Obj;
                List<FIN_CRDR_NOTE_MPG> FIN_CRDR_NOTE_MPGLST_Obj;
                FIN_INVOICE_CUS_HDR FIN_INVOICE_CUS_HDR_Obj;
                List<FIN_CRDR_NOTE_DTL> fin_RECEIPT_CUS_SO_MPG_List_Obj;
                FIN_INVOICE_CUS_DTL obj_SAL_ORDER_HDR;
                //IQueryable<FIN_TRX> oldFIN_TRX;
                string refType;
                long refPK;

                // Get Header Object
                FIN_CRDR_NOTE_HDR_Obj = currentEntity.FIN_CRDR_NOTE_HDR.SingleOrDefault(sah => sah.CDH_PK == crdrPK);

                // Get Details Object
                FIN_CRDR_NOTE_MPGLST_Obj = FIN_CRDR_NOTE_HDR_Obj.FIN_CRDR_NOTE_MPG.ToList();

                // Reverse Invoice amount and delete details
                foreach (FIN_CRDR_NOTE_MPG FIN_CRDR_NOTE_MPG_Obj in FIN_CRDR_NOTE_MPGLST_Obj)
                {
                    FIN_INVOICE_CUS_HDR_Obj = currentEntity.FIN_INVOICE_CUS_HDR.SingleOrDefault(sah => sah.ICH_PK == FIN_CRDR_NOTE_MPG_Obj.CDM_INVOICE_CUS_HDR);

                    //if (FIN_INVOICE_CUS_HDR_Obj != null)
                    //{
                    //    FIN_INVOICE_CUS_HDR_Obj.ICH_AMOUNT_RCVD_TC -= FIN_CRDR_NOTE_MPG_Obj.RCM_RCVD_AMOUNT;
                    //}

                    //Delete Allocation details
                    fin_RECEIPT_CUS_SO_MPG_List_Obj = FIN_CRDR_NOTE_MPG_Obj.FIN_CRDR_NOTE_DTL.ToList();
                    foreach (FIN_CRDR_NOTE_DTL fin_RECEIPT_CUS_SO_MPG_obj in fin_RECEIPT_CUS_SO_MPG_List_Obj)
                    {
                        obj_SAL_ORDER_HDR = currentEntity.FIN_INVOICE_CUS_DTL.SingleOrDefault(a => a.CID_PK == fin_RECEIPT_CUS_SO_MPG_obj.CDS_INVOICE_CUS_DTL);

                        //delete details tax split up
                        List<FIN_CRDR_NOTE_TAX_DTL> fin_TaxDet_List_Obj = fin_RECEIPT_CUS_SO_MPG_obj.FIN_CRDR_NOTE_TAX_DTL.ToList();
                        if (fin_TaxDet_List_Obj != null && fin_TaxDet_List_Obj.Count > 0)
                        {
                            foreach (FIN_CRDR_NOTE_TAX_DTL finTaxObj in fin_TaxDet_List_Obj)
                            {
                                FIN_CRDR_NOTE_TAX_DTL obj_TAX_DTL = currentEntity.FIN_CRDR_NOTE_TAX_DTL.SingleOrDefault(tax => tax.NTD_PK == finTaxObj.NTD_PK);
                                this.currentEntity.FIN_CRDR_NOTE_TAX_DTL.DeleteObject(obj_TAX_DTL);
                            }
                        }
                        //////////////
                        this.currentEntity.FIN_CRDR_NOTE_DTL.DeleteObject(fin_RECEIPT_CUS_SO_MPG_obj);
                    }

                    //delete header tax split up
                    List<FIN_CRDR_NOTE_TAX_DTL> fin_TaxHdr_List_Obj = FIN_CRDR_NOTE_MPG_Obj.FIN_CRDR_NOTE_TAX_DTL.ToList();
                    if (fin_TaxHdr_List_Obj != null && fin_TaxHdr_List_Obj.Count > 0)
                    {
                        foreach (FIN_CRDR_NOTE_TAX_DTL finTaxObj in fin_TaxHdr_List_Obj)
                        {
                            FIN_CRDR_NOTE_TAX_DTL obj_TAX_DTL = currentEntity.FIN_CRDR_NOTE_TAX_DTL.SingleOrDefault(tax => tax.NTD_PK == finTaxObj.NTD_PK);
                            this.currentEntity.FIN_CRDR_NOTE_TAX_DTL.DeleteObject(obj_TAX_DTL);
                        }
                    }
                    //////////////

                    this.currentEntity.FIN_CRDR_NOTE_MPG.DeleteObject(FIN_CRDR_NOTE_MPG_Obj);
                }

                refType = "SALES INVOICE";
                refPK = crdrPK;


                // Delete Header
                if (FIN_CRDR_NOTE_HDR_Obj != null)
                {
                    // Save Log

                    ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                    AdmTrxLogDet.ATL_APP_TRX_CODE = FIN_CRDR_NOTE_HDR_Obj.CDH_NO;
                    if (FIN_CRDR_NOTE_HDR_Obj.CDH_TYPE == (int)DebitCreditModeEnum.DEBIT)
                    {
                        AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.DN;
                    }
                    else if (FIN_CRDR_NOTE_HDR_Obj.CDH_TYPE == (int)DebitCreditModeEnum.CREDIT)
                    {
                        AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.CN;
                    }
                    AdmTrxLogDet.ATL_MOD_BY = FIN_CRDR_NOTE_HDR_Obj.CDH_MOD_BY;
                    AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                    AdmTrxLogDet.ATL_BIZUNIT = FIN_CRDR_NOTE_HDR_Obj.CDH_BIZUNIT;
                    AdmTrxLogDet.ATL_APP_TRX_PK = FIN_CRDR_NOTE_HDR_Obj.CDH_PK;
                    AdmTrxLogDet.ATL_PK = 0;
                    AdmTrxLogDet.ATL_ACTION = (byte)LogAction.DELETE;
                    AdmTrxLogList.Add(AdmTrxLogDet);
                    ComnFnManagerObj.SaveLog(AdmTrxLogList);

                    this.currentEntity.FIN_CRDR_NOTE_HDR.DeleteObject(FIN_CRDR_NOTE_HDR_Obj);
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

        #endregion



    }
}
