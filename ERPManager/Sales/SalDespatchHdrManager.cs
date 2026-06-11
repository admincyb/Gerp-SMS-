using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;
using ERPManager;
using System.Data.Objects;


namespace ERPManager
{
    public class SalDespatchHdrManager : ISalDespatchHdrManager
    {
        #region Private Variables
        /// <summary>
        /// Gets or sets current db context
        /// </summary>
        private ERPEntities currentEntity;
        #endregion

        #region Manager Methods

          /// <summary>
        /// Initializes a new instance of the DespatchListManager class
        /// </summary>
        /// <param name="currentEntity">Current db context</param>
        public SalDespatchHdrManager(ERPEntities currentEntity)
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
        /// Saves list of Despatch Header
        /// </summary>
        /// <param name="DespatchHdrList"></param>
        /// <returns></returns>
        public long SaveDespatchHdr(List<SAL_DESPATCH_HDR> SalDespatchHdrList)
        {
            int retval;
            SAL_DESPATCH_HDR OldSAL_DESPATCH_HDR_Obj;
            List<SAL_DESPATCH_DTL> SAL_DESPATCH_DTL_LST_Obj;
            SalDespatchDtlManager SAL_DESPATCH_DTLManagerObj;
            int? max_DPH_PK;

            try
            {
                retval = 0;
                foreach (SAL_DESPATCH_HDR SAL_DESPATCH_HDR_Obj in SalDespatchHdrList)
                {   
                    SAL_DESPATCH_DTL_LST_Obj = SAL_DESPATCH_HDR_Obj.SAL_DESPATCH_DTL.ToList();                   

                    if (SAL_DESPATCH_HDR_Obj.DPH_PK == 0)
                    {
                        SAL_DESPATCH_HDR_Obj.SAL_DESPATCH_DTL.Clear();
                        // Gets last SAL_DESPATCH_HDR pk
                        max_DPH_PK = this.currentEntity.SAL_DESPATCH_HDR.Max(dph => (int?)dph.DPH_PK);

                        // Sets return value as next SAL_DESPATCH_HDR pk
                        retval = (max_DPH_PK.HasValue ? max_DPH_PK.Value + 1 : 1);

                        SAL_DESPATCH_HDR_Obj.DPH_PK = retval;
                        SAL_DESPATCH_HDR_Obj.DPH_CRTD_DT = DateTime.Now;
                        SAL_DESPATCH_HDR_Obj.DPH_MOD_DT = DateTime.Now;
                        SAL_DESPATCH_HDR_Obj.DPH_TOTAL_QTY = SAL_DESPATCH_DTL_LST_Obj == null ? 0 : SAL_DESPATCH_DTL_LST_Obj.Sum(dtl => dtl.DPD_QTY_DESPATCHED);

                        // Add new SAL_DESPATCH_HDR to the db context
                        this.currentEntity.SAL_DESPATCH_HDR.AddObject(SAL_DESPATCH_HDR_Obj);

                        //set the SAL_DESPATCH_HDR pk as the fk of  SAL_DESPATCH_DTL
                        SAL_DESPATCH_DTL_LST_Obj.ForEach(dtl => dtl.DPD_DESPATCH_HDR = retval);
                        SAL_DESPATCH_DTLManagerObj = new SalDespatchDtlManager(this.currentEntity);

                        //Save despatch details
                        SAL_DESPATCH_DTLManagerObj.SaveSalDespatchDtl(SAL_DESPATCH_DTL_LST_Obj);
                    }
                    else
                    {
                        // updating SAL_DESPATCH_HDR
                        // Get current SAL_DESPATCH_HDR using SAL_DESPATCH_HDR pk and last modified date time,used for concurrency checking
                        OldSAL_DESPATCH_HDR_Obj = currentEntity.SAL_DESPATCH_HDR.SingleOrDefault(dph => dph.DPH_PK == SAL_DESPATCH_HDR_Obj.DPH_PK && dph.DPH_MOD_DT == SAL_DESPATCH_HDR_Obj.DPH_MOD_DT);

                        // If oldPaymentHdrObj is null then,anyone modified or deleted the record
                        if (OldSAL_DESPATCH_HDR_Obj != null)
                        {
                            //#region SPSAL_DESPATCH_CR_UPDATE Sp for to revert 'Container Release', 'Stock' According to 'Delivery Order'
                            //// SPSAL_DESPATCH_CR_UPDATE Sp for to revert 'Container Release', 'Stock' According to 'Delivery Order'
                            //int pDphPk = OldSAL_DESPATCH_HDR_Obj.DPH_PK;
                            //var pRetVal = new ObjectParameter(DataFieldRes.P_RET_VAL, typeof(int));
                            //var result = this.currentEntity.SPSAL_DESPATCH_CR_UPDATE(pDphPk, pRetVal); //ctx.MyFunction("XYZ", oMyString).ToList();
                            //int spResult = Convert.ToInt32(pRetVal.Value);  // .Value.ToString();
                            //if (spResult<1)  
                            //{
                            //    throw new Exception(ERPManagerRes.DespatchUpdateFailed);
                            //} 
                            //#endregion

                            // Update SAL_DESPATCH_HDR
                            OldSAL_DESPATCH_HDR_Obj.DPH_SHIPPING_PLAN       = SAL_DESPATCH_HDR_Obj.DPH_SHIPPING_PLAN;
                            OldSAL_DESPATCH_HDR_Obj.DPH_VERSION				= SAL_DESPATCH_HDR_Obj.DPH_VERSION;				
                            OldSAL_DESPATCH_HDR_Obj.DPH_DATE				= SAL_DESPATCH_HDR_Obj.DPH_DATE;		
                            OldSAL_DESPATCH_HDR_Obj.DPH_NO					= SAL_DESPATCH_HDR_Obj.DPH_NO;
                           	
                            OldSAL_DESPATCH_HDR_Obj.DPH_REF_NO				= SAL_DESPATCH_HDR_Obj.DPH_REF_NO;				
                            OldSAL_DESPATCH_HDR_Obj.DPH_CONTAINER			= SAL_DESPATCH_HDR_Obj.DPH_CONTAINER;			
                            OldSAL_DESPATCH_HDR_Obj.DPH_REFERENCE			= SAL_DESPATCH_HDR_Obj.DPH_REFERENCE;			
                            OldSAL_DESPATCH_HDR_Obj.DPH_FROM_PORT			= SAL_DESPATCH_HDR_Obj.DPH_FROM_PORT;			
                            OldSAL_DESPATCH_HDR_Obj.DPH_TO_PORT				= SAL_DESPATCH_HDR_Obj.DPH_TO_PORT;				
                            OldSAL_DESPATCH_HDR_Obj.DPH_FINAL_DESTINATION   = SAL_DESPATCH_HDR_Obj.DPH_FINAL_DESTINATION ; 
                            OldSAL_DESPATCH_HDR_Obj.DPH_ORG_GOODS			= SAL_DESPATCH_HDR_Obj.DPH_ORG_GOODS;		
                            OldSAL_DESPATCH_HDR_Obj.DPH_PAYMENT_TERM		= SAL_DESPATCH_HDR_Obj.DPH_PAYMENT_TERM;		
                            OldSAL_DESPATCH_HDR_Obj.DPH_PAYMENT_TERM_TEXT	= SAL_DESPATCH_HDR_Obj.DPH_PAYMENT_TERM_TEXT;	
                            OldSAL_DESPATCH_HDR_Obj.DPH_DEL_TERM			= SAL_DESPATCH_HDR_Obj.DPH_DEL_TERM;			
                            OldSAL_DESPATCH_HDR_Obj.DPH_DEL_TERM_TEXT		= SAL_DESPATCH_HDR_Obj.DPH_DEL_TERM_TEXT;		
                            OldSAL_DESPATCH_HDR_Obj.DPH_FEEDER_VESSEL		= SAL_DESPATCH_HDR_Obj.DPH_FEEDER_VESSEL;		
                            OldSAL_DESPATCH_HDR_Obj.DPH_MOTHER_VESSEL		= SAL_DESPATCH_HDR_Obj.DPH_MOTHER_VESSEL;		
                            OldSAL_DESPATCH_HDR_Obj.DPH_CY_DATE				= SAL_DESPATCH_HDR_Obj.DPH_CY_DATE;				
                            OldSAL_DESPATCH_HDR_Obj.DPH_RTN_DATE			= SAL_DESPATCH_HDR_Obj.DPH_RTN_DATE	;		
                            OldSAL_DESPATCH_HDR_Obj.DPH_ETD					= SAL_DESPATCH_HDR_Obj.DPH_ETD	;				
                            OldSAL_DESPATCH_HDR_Obj.DPH_ETA					= SAL_DESPATCH_HDR_Obj.DPH_ETA;					
                            OldSAL_DESPATCH_HDR_Obj.DPH_SHIPMENT_DATE		= SAL_DESPATCH_HDR_Obj.DPH_SHIPMENT_DATE;		
                            OldSAL_DESPATCH_HDR_Obj.DPH_SHIPPING_MARK		= SAL_DESPATCH_HDR_Obj.DPH_SHIPPING_MARK;		
                            OldSAL_DESPATCH_HDR_Obj.DPH_CARRIER				= SAL_DESPATCH_HDR_Obj.DPH_CARRIER	;			
                            OldSAL_DESPATCH_HDR_Obj.DPH_CONTAINER_NO		= SAL_DESPATCH_HDR_Obj.DPH_CONTAINER_NO	;	
                            OldSAL_DESPATCH_HDR_Obj.DPH_SEAL_NO				= SAL_DESPATCH_HDR_Obj.DPH_SEAL_NO	;			
                            OldSAL_DESPATCH_HDR_Obj.DPH_COMP				= SAL_DESPATCH_HDR_Obj.DPH_COMP	;			
                            OldSAL_DESPATCH_HDR_Obj.DPH_DRIVER				= SAL_DESPATCH_HDR_Obj.DPH_DRIVER	;			
                            OldSAL_DESPATCH_HDR_Obj.DPH_LORRY_NO			= SAL_DESPATCH_HDR_Obj.DPH_LORRY_NO	;		
                            OldSAL_DESPATCH_HDR_Obj.DPH_REMARKS				= SAL_DESPATCH_HDR_Obj.DPH_REMARKS;				
                            OldSAL_DESPATCH_HDR_Obj.DPH_INSP_BY				= SAL_DESPATCH_HDR_Obj.DPH_INSP_BY;
                            OldSAL_DESPATCH_HDR_Obj.DPH_APRD_BY             = SAL_DESPATCH_HDR_Obj.DPH_APRD_BY;
                            OldSAL_DESPATCH_HDR_Obj.DPH_BOOKING_DATE        = SAL_DESPATCH_HDR_Obj.DPH_BOOKING_DATE;
                            OldSAL_DESPATCH_HDR_Obj.DPH_BOOKING_NO          = SAL_DESPATCH_HDR_Obj.DPH_BOOKING_NO;

                            OldSAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE           = SAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_NAME      = SAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_NAME;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_ADDRESS   = SAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_ADDRESS;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_COUNTRY   = SAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_COUNTRY;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_FAX       = SAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_FAX;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_PHONE     = SAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_PHONE;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_MOBILE    = SAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_MOBILE;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_EMAIL     = SAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_EMAIL;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_ZIP       = SAL_DESPATCH_HDR_Obj.DPH_CONSIGNEE_ZIP;

                            OldSAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY        = SAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY;
                            OldSAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_NAME   = SAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_NAME;
                            OldSAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_ADDRESS= SAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_ADDRESS;
                            OldSAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_COUNTRY= SAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_COUNTRY;
                            OldSAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_FAX    = SAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_FAX;
                            OldSAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_PHONE  = SAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_PHONE;
                            OldSAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_MOBILE = SAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_MOBILE;
                            OldSAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_EMAIL  = SAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_EMAIL;
                            OldSAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_ZIP    = SAL_DESPATCH_HDR_Obj.DPH_NOTIFY_PARTY_ZIP;

                            OldSAL_DESPATCH_HDR_Obj.DPH_PAYMENT_TERM        = SAL_DESPATCH_HDR_Obj.DPH_PAYMENT_TERM;
                            OldSAL_DESPATCH_HDR_Obj.DPH_PAYMENT_TERM_TEXT   = SAL_DESPATCH_HDR_Obj.DPH_PAYMENT_TERM_TEXT;

                            OldSAL_DESPATCH_HDR_Obj.DPH_SUPP_DTL            = SAL_DESPATCH_HDR_Obj.DPH_SUPP_DTL;
                            
                            OldSAL_DESPATCH_HDR_Obj.DPH_ACTIVE				= SAL_DESPATCH_HDR_Obj.DPH_ACTIVE;				
                            OldSAL_DESPATCH_HDR_Obj.DPH_DEPT				= SAL_DESPATCH_HDR_Obj.DPH_DEPT;				
                            OldSAL_DESPATCH_HDR_Obj.DPH_BIZUNIT				= SAL_DESPATCH_HDR_Obj.DPH_BIZUNIT;
                            OldSAL_DESPATCH_HDR_Obj.DPH_MOD_BY              = SAL_DESPATCH_HDR_Obj.DPH_MOD_BY;
                            OldSAL_DESPATCH_HDR_Obj.DPH_MOD_DT              = DateTime.Now;
                            OldSAL_DESPATCH_HDR_Obj.DPH_SHIP_BY             = SAL_DESPATCH_HDR_Obj.DPH_SHIP_BY;

                            OldSAL_DESPATCH_HDR_Obj.DPH_CUSTOMER = SAL_DESPATCH_HDR_Obj.DPH_CUSTOMER;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_NAME = SAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_NAME;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_ADDRESS = SAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_ADDRESS;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_COUNTRY = SAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_COUNTRY;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_FAX = SAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_FAX;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_PHONE = SAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_PHONE;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_MOBILE = SAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_MOBILE;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_EMAIL = SAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_EMAIL;
                            OldSAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_ZIP = SAL_DESPATCH_HDR_Obj.DPH_CUSTOMER_ZIP;

                            OldSAL_DESPATCH_HDR_Obj.DPH_ENCLOS_TERM = SAL_DESPATCH_HDR_Obj.DPH_ENCLOS_TERM;
                            OldSAL_DESPATCH_HDR_Obj.DPH_ENCLOS_TERM_TEXT = SAL_DESPATCH_HDR_Obj.DPH_ENCLOS_TERM_TEXT;
                            OldSAL_DESPATCH_HDR_Obj.DPH_PORT_OF_DISCHARGE = SAL_DESPATCH_HDR_Obj.DPH_PORT_OF_DISCHARGE;

                            OldSAL_DESPATCH_HDR_Obj.DPH_TOTAL_QTY = SAL_DESPATCH_DTL_LST_Obj == null ? 0 : SAL_DESPATCH_DTL_LST_Obj.Sum(dtl => dtl.DPD_QTY_DESPATCHED);

                            OldSAL_DESPATCH_HDR_Obj.DPH_COMPANY = SAL_DESPATCH_HDR_Obj.DPH_COMPANY;

                            OldSAL_DESPATCH_HDR_Obj.DPH_SHIPPING_ADDRESS = SAL_DESPATCH_HDR_Obj.DPH_SHIPPING_ADDRESS;
                            OldSAL_DESPATCH_HDR_Obj.DPH_SHIPPING_NAME = SAL_DESPATCH_HDR_Obj.DPH_SHIPPING_NAME;
                            OldSAL_DESPATCH_HDR_Obj.DPH_HIS_CODE = SAL_DESPATCH_HDR_Obj.DPH_HIS_CODE;
                            OldSAL_DESPATCH_HDR_Obj.DPH_SHIPPED_BOARD = SAL_DESPATCH_HDR_Obj.DPH_SHIPPED_BOARD;
                            OldSAL_DESPATCH_HDR_Obj.DPH_TRANSHIPMENT = SAL_DESPATCH_HDR_Obj.DPH_TRANSHIPMENT;
                            OldSAL_DESPATCH_HDR_Obj.DPH_ADNL_BUYER = SAL_DESPATCH_HDR_Obj.DPH_ADNL_BUYER;
                            OldSAL_DESPATCH_HDR_Obj.DPH_SWAP_BUYER = SAL_DESPATCH_HDR_Obj.DPH_SWAP_BUYER;

                            OldSAL_DESPATCH_HDR_Obj.DPH_PRINT_SHIP_TO = SAL_DESPATCH_HDR_Obj.DPH_PRINT_SHIP_TO;

                            // Sets return value as SAL_DESPATCH_HDR pk
                            retval = SAL_DESPATCH_HDR_Obj.DPH_PK;
                            SAL_DESPATCH_DTLManagerObj = new SalDespatchDtlManager(this.currentEntity);
                            //Save despatch Details
                            SAL_DESPATCH_DTLManagerObj.SaveSalDespatchDtl(SAL_DESPATCH_DTL_LST_Obj);

                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }

                    
                }

                //return Despatch Hdr Pk
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
        /// Deletes list of Despatch Header
        /// </summary>
        /// <param name="DespatchHdrList"></param>
        /// <returns></returns>
        public long DeleteSalDespatch(long despatchPK)
        {
            long retval = 0;
          
            try
            {
                SAL_DESPATCH_HDR SAL_DESPATCH_HDR_Obj;
                List<SAL_DESPATCH_DTL> SAL_DESPATCH_DTL_LST_Obj;
                SAL_ORDER_DTL SAL_ORDER_DTL_Obj;

                // Get Header Object
                SAL_DESPATCH_HDR_Obj = currentEntity.SAL_DESPATCH_HDR.SingleOrDefault(dph => dph.DPH_PK == despatchPK);

                if (SAL_DESPATCH_HDR_Obj != null)
                {
                    //// Get Details Object
                    SAL_DESPATCH_DTL_LST_Obj = SAL_DESPATCH_HDR_Obj.SAL_DESPATCH_DTL.ToList();

                    //// Reverse despatched qty and delete details
                    foreach (SAL_DESPATCH_DTL SAL_DESPATCH_DTL_LST_MPG_Obj in SAL_DESPATCH_DTL_LST_Obj)
                    {
                        SAL_ORDER_DTL_Obj = currentEntity.SAL_ORDER_DTL.SingleOrDefault(sod => sod.SOD_PK == SAL_DESPATCH_DTL_LST_MPG_Obj.DPD_SO_DTL);

                        /*if (SAL_ORDER_DTL_Obj != null)
                        {
                            SAL_ORDER_DTL_Obj.SOD_QTY_DISPATCHED -= SAL_DESPATCH_DTL_LST_MPG_Obj.DPD_QTY_DESPATCHED;
                            SAL_ORDER_DTL_Obj.SOD_BAL_TO_DISPATCH += SAL_DESPATCH_DTL_LST_MPG_Obj.DPD_QTY_DESPATCHED;
                        }*/

                        this.currentEntity.SAL_DESPATCH_DTL.DeleteObject(SAL_DESPATCH_DTL_LST_MPG_Obj);
                    }

                    // Delete Header
                    if (SAL_DESPATCH_HDR_Obj != null)
                    {
                        this.currentEntity.SAL_DESPATCH_HDR.DeleteObject(SAL_DESPATCH_HDR_Obj);
                    }
                    else
                    {
                        // throws exception already deleted or modified by other user
                        throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                    }
                    retval = 1;
                }
                else
                {
                    // throws exception already deleted or modified by other user
                    throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                }
                //return Despatch Hdr Pk
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
        /// Perform searching,filtering,sorting,and paging on Despatch Header;
        /// </summary>
        /// <param name="AD_COUNTRIES_MSTObj" value="Contract master object with Contract master pk and active status"></param>
        /// <param name="utilityObj" value="Search criteria object"></param>
        /// <returns>List of Contract master</returns>
        public List<SAL_DESPATCH_HDR> GetSaleDespatchHdr(SAL_DESPATCH_HDR SaleDespatchObj, ServiceUtility utilityObj = null)
        {
            List<SAL_DESPATCH_HDR> SalDispatchHdrList = null;
            IQueryable<SAL_DESPATCH_HDR> SAL_DESPATCH_HDRQuery; 
            int pageSize;

            try
            {

                pageSize = Convert.ToInt32(utilityObj.PageSize);

                if (SaleDespatchObj.DPH_PK > 0)
                {
                    SAL_DESPATCH_HDRQuery = (from dph in this.currentEntity.SAL_DESPATCH_HDR
                                             join ssh in this.currentEntity.SAL_SHIPPING_PLAN_HDR on dph.DPH_SHIPPING_PLAN equals ssh.SNH_PK
                                             where dph.DPH_ACTIVE == SaleDespatchObj.DPH_ACTIVE
                                                  && dph.DPH_PK == SaleDespatchObj.DPH_PK
                                                  && ssh.SNH_DEL_STATUS == 0
                                                  //&& dph.DPH_CUSTOMER == (SaleDespatchObj.DPH_CUSTOMER > 0 ? SaleDespatchObj.DPH_CUSTOMER : dph.DPH_CUSTOMER)
                                                  //&& (SaleDespatchObj.DPH_COMP == 0) ^ (SaleDespatchObj.DPH_COMP > 0 && dph.DPH_COMP == SaleDespatchObj.DPH_COMP)
                                                  //&& dph.DPH_DATE >= (utilityObj.FilterDate == null || (utilityObj.FilterDate != null && utilityObj.FilterDate == DateTime.MinValue) ? dph.DPH_DATE : utilityObj.FilterDate)
                                                  //&& dph.DPH_DATE <= (utilityObj.FilterToDate == null || (utilityObj.FilterToDate != null && utilityObj.FilterToDate == DateTime.MinValue) ? dph.DPH_DATE : utilityObj.FilterToDate)
                                                  && (utilityObj.NeedAdvanceFilter ? dph.DPH_STATUS == (SaleDespatchObj.DPH_STATUS >= 0 ? SaleDespatchObj.DPH_STATUS : dph.DPH_STATUS) : true)
                                             select dph);
                }
                else
                {
                    SAL_DESPATCH_HDRQuery = (from dph in this.currentEntity.SAL_DESPATCH_HDR
                                             join ssh in this.currentEntity.SAL_SHIPPING_PLAN_HDR on dph.DPH_SHIPPING_PLAN equals ssh.SNH_PK
                                             where dph.DPH_ACTIVE == SaleDespatchObj.DPH_ACTIVE
                                                  && dph.DPH_PK == (SaleDespatchObj.DPH_PK > 0 ? SaleDespatchObj.DPH_PK : dph.DPH_PK)
                                                  && ssh.SNH_DEL_STATUS == 0 
                                                  && dph.DPH_CUSTOMER == (SaleDespatchObj.DPH_CUSTOMER > 0 ? SaleDespatchObj.DPH_CUSTOMER : dph.DPH_CUSTOMER)
                                                  && (SaleDespatchObj.DPH_COMP == 0) ^ (SaleDespatchObj.DPH_COMP > 0 && dph.DPH_COMP == SaleDespatchObj.DPH_COMP)
                                                  && dph.DPH_DATE >= (utilityObj.FilterDate == null || (utilityObj.FilterDate != null && utilityObj.FilterDate == DateTime.MinValue) ? dph.DPH_DATE : utilityObj.FilterDate)
                                                  && dph.DPH_DATE <= (utilityObj.FilterToDate == null || (utilityObj.FilterToDate != null && utilityObj.FilterToDate == DateTime.MinValue) ? dph.DPH_DATE : utilityObj.FilterToDate)
                                                  && (utilityObj.NeedAdvanceFilter ? dph.DPH_STATUS == (SaleDespatchObj.DPH_STATUS >= 0 ? SaleDespatchObj.DPH_STATUS : dph.DPH_STATUS) : true)
                                                  && (dph.DPH_BIZUNIT == (SaleDespatchObj.DPH_BIZUNIT > 0 ? SaleDespatchObj.DPH_BIZUNIT : dph.DPH_BIZUNIT))
                                             select dph);
                }

                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = SAL_DESPATCH_HDRQuery.Count();
                //Filter Query

                #region Sorting
                // Apply Paging And Sorting
                // Checking sorting criteria is given
                if (utilityObj.SortBy != null && utilityObj.SortDirection != null)
                {
                    if (utilityObj.SortBy == DataFieldRes.VendorName)
                        utilityObj.SortBy = DataTableRes.VendorMst + "." + DataFieldRes.VendorName;
                    if (utilityObj.SortBy == DataFieldRes.CustomerName)
                        utilityObj.SortBy = DataTableRes.CustomerMst + "." + DataFieldRes.CustomerName;
                    if (utilityObj.SortBy == DataFieldRes.CarrierName)
                        utilityObj.SortBy = DataTableRes.AdmCosntMst + "." + DataFieldRes.CarrierName;
                //    if (utilityObj.SortBy == DataFieldRes.UOMName)
                //        utilityObj.SortBy = DataTableRes.UOMMst + "." + DataFieldRes.UOMName;
                }
                #endregion
                //return details;
                SalDispatchHdrList = SAL_DESPATCH_HDRQuery.SortRecords<SAL_DESPATCH_HDR>(utilityObj).ToList();

                //return Despatch Hdr List
                return SalDispatchHdrList;

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

        public List<SAL_DESPATCH_HDR> GetSaleDespatchNumberAutoCompleteList(SAL_DESPATCH_HDR SaleDespatchHeader, ServiceUtility utilityObj)
        {
            List<SAL_DESPATCH_HDR> SAL_DESPATCH_HDRListObj = new List<SAL_DESPATCH_HDR>();
            try
            {
                SAL_DESPATCH_HDRListObj = (from dph in this.currentEntity.SAL_DESPATCH_HDR
                                           join ssh in this.currentEntity.SAL_SHIPPING_PLAN_HDR on dph.DPH_SHIPPING_PLAN equals ssh.SNH_PK
                                           where dph.DPH_NO.Contains(utilityObj.FilterValue)
                                                 && ssh.SNH_DEL_STATUS == 0 
                                                 && dph.DPH_STATUS == (SaleDespatchHeader.DPH_STATUS > 0 ? SaleDespatchHeader.DPH_STATUS : dph.DPH_STATUS)
                                           select dph).ToList();
                SAL_DESPATCH_HDRListObj = SAL_DESPATCH_HDRListObj.OrderBy(odr => odr.DPH_NO).ToList();
            }
            catch
            {
            }
            return SAL_DESPATCH_HDRListObj;
        }
        /// <summary>
        /// Get Despatch hdr values w.r.t sale order
        /// </summary>
        /// <param name="SaleorderHdrObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<SAL_DESPATCH_HDR> GetDespatchHdr(SAL_ORDER_HDR SaleorderHdrObj, ServiceUtility utilityObj = null)
        {
            List<SAL_DESPATCH_HDR> ObjDespatchHeaderLst = new List<SAL_DESPATCH_HDR>();
            IQueryable<SAL_DESPATCH_HDR> SAL_DESPATCH_HDRQuery;

            try
            {
                SAL_DESPATCH_HDRQuery = (from dsh in this.currentEntity.SAL_DESPATCH_HDR
                                        join dsd in this.currentEntity.SAL_DESPATCH_DTL on  dsh.DPH_PK equals dsd.DPD_DESPATCH_HDR
                                        where dsd.DPD_SALE_ORDER == SaleorderHdrObj.SOH_PK
                                        && dsh.DPH_STATUS == 2
                                        && dsh.DPH_DEL_STATUS == 0 // To avoid DO's for cancelled shipping plan
                                        select dsh).Distinct();
                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = SAL_DESPATCH_HDRQuery.Count();

                ObjDespatchHeaderLst = SAL_DESPATCH_HDRQuery.SortRecords<SAL_DESPATCH_HDR>(utilityObj).ToList();

            }
            catch
            {
            }

            return ObjDespatchHeaderLst;
        }
    #endregion

        
    }
}
