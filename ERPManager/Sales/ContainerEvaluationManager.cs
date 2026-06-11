using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;
using ERPManager;
using System.Data.Objects;
using BusinessObject.CommonManagement;

namespace ERPManager.Sales
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ContainerEvaluationManager" in both code and config file together.
    public class ContainerEvaluationManager : IContainerEvaluationManager 
    {
        #region Private Variables
        /// <summary>
        /// Gets or sets current db context
        /// </summary>
        private ERPEntities currentEntity;
        #endregion

        #region Manager Methods

          /// <summary>
        /// Initializes a new instance of the ContainerInspectionManager class
        /// </summary>
        /// <param name="currentEntity">Current db context</param>
        public ContainerEvaluationManager(ERPEntities currentEntity)
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
        /// Get list of Container Evaluation Header
        /// </summary>
        /// <param name="SALCONTAINEREVALHDRobj"></param>
        /// <returns></returns>
        public List<SAL_CONTAINER_EVAL_HDR> GetContainerEvaluationList(SAL_CONTAINER_EVAL_HDR SALCONTAINEREVALHDRobj, ServiceUtility utilityObj)
        {
            List<SAL_CONTAINER_EVAL_HDR> SAL_CONTAINER_EVAL_HDR_Obj;
            IQueryable<SAL_CONTAINER_EVAL_HDR> Obj_SAL_CONTAINER_EVAL_HDR_Qry;
            try
            {
               

                Obj_SAL_CONTAINER_EVAL_HDR_Qry = (from cvh in this.currentEntity.SAL_CONTAINER_EVAL_HDR
                                                   where cvh.CVH_ACTIVE == SALCONTAINEREVALHDRobj.CVH_ACTIVE
                                                       //&& cvh.CVH_PK == (SALCONTAINEREVALHDRobj.CVH_PK > 0 ? SALCONTAINEREVALHDRobj.CVH_PK : cvh.CVH_PK)
                                                        && cvh.CVH_SHIPPING_PLAN == (SALCONTAINEREVALHDRobj.CVH_SHIPPING_PLAN > 0 ? SALCONTAINEREVALHDRobj.CVH_SHIPPING_PLAN : cvh.CVH_SHIPPING_PLAN)
                                                   select cvh);
                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = Obj_SAL_CONTAINER_EVAL_HDR_Qry.Count();

                //Apply Paging And Sorting for grid Purpose
                SAL_CONTAINER_EVAL_HDR_Obj = Obj_SAL_CONTAINER_EVAL_HDR_Qry.SortRecords<SAL_CONTAINER_EVAL_HDR>(utilityObj).ToList();
                

                //return (from cvh in this.currentEntity.SAL_CONTAINER_EVAL_HDR
                //        where cvh.CVH_ACTIVE == SALCONTAINEREVALHDRobj.CVH_ACTIVE
                //            &&  cvh.CVH_PK == (SALCONTAINEREVALHDRobj.CVH_PK > 0 ? SALCONTAINEREVALHDRobj.CVH_PK : cvh.CVH_PK)
                //            select cvh).ToList();
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
            return SAL_CONTAINER_EVAL_HDR_Obj;
        }

        /// <summary>
        /// save list of Container Evaluation Header
        /// </summary>
        /// <param name="SALCONTAINEREVALHDRobj"></param>
        /// <returns></returns>
        public int SaveContainerEvaluation(List<SAL_CONTAINER_EVAL_HDR> SALCONTAINEREVALHDRobj)
        {
            int retval=0;
            int? max_PK;

            SAL_CONTAINER_EVAL_HDR  Old_SAL_CONTAINER_EVAL_HDR_Obj=null;

            try
            {
                foreach(SAL_CONTAINER_EVAL_HDR Obj_SAL_CONTAINER_EVAL_HDR in SALCONTAINEREVALHDRobj)
                {
                    if(Obj_SAL_CONTAINER_EVAL_HDR.CVH_PK == 0)
                    {
                        #region Check whether the Container Evaluation is already created or not
                        var objContainerEval = (from cntevl in this.currentEntity.SAL_CONTAINER_EVAL_HDR where cntevl.CVH_SHIPPING_PLAN == Obj_SAL_CONTAINER_EVAL_HDR.CVH_SHIPPING_PLAN select cntevl).ToList();
                        if (objContainerEval != null && objContainerEval.Count > 0)
                        {
                            retval = -8;// Container evaluation already created
                            break;
                        } 
                        #endregion

                        // Gets last SAL_DESPATCH_HDR pk
                        max_PK = this.currentEntity.SAL_CONTAINER_EVAL_HDR.Max(dph => (int?)dph.CVH_PK);

                        // Sets return value as next SAL_DESPATCH_HDR pk
                        retval = (max_PK.HasValue ? max_PK.Value + 1 : 1);

                        Obj_SAL_CONTAINER_EVAL_HDR.CVH_PK = retval;
                        Obj_SAL_CONTAINER_EVAL_HDR.CVH_CRTD_DT = DateTime.Now;
                        Obj_SAL_CONTAINER_EVAL_HDR.CVH_MOD_DT  = DateTime.Now;

                        // Add new SAL_DESPATCH_HDR to the db context
                        this.currentEntity.SAL_CONTAINER_EVAL_HDR.AddObject(Obj_SAL_CONTAINER_EVAL_HDR);
                    }
                    else
                    {
                        Old_SAL_CONTAINER_EVAL_HDR_Obj = currentEntity.SAL_CONTAINER_EVAL_HDR.FirstOrDefault(dph => dph.CVH_PK == Obj_SAL_CONTAINER_EVAL_HDR.CVH_PK);
                        if (Old_SAL_CONTAINER_EVAL_HDR_Obj != null && Obj_SAL_CONTAINER_EVAL_HDR.CVH_MOD_DT == Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_MOD_DT)
                        {
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_NO   =   Obj_SAL_CONTAINER_EVAL_HDR.CVH_NO;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_DATE =   Obj_SAL_CONTAINER_EVAL_HDR.CVH_DATE;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_CARRIER = Obj_SAL_CONTAINER_EVAL_HDR.CVH_CARRIER;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_BOOKING_DATE = Obj_SAL_CONTAINER_EVAL_HDR.CVH_BOOKING_DATE;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_BOOKING_NO = Obj_SAL_CONTAINER_EVAL_HDR.CVH_BOOKING_NO;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_TRANS_COMP = Obj_SAL_CONTAINER_EVAL_HDR.CVH_TRANS_COMP;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_CONTAINER_NO   =   Obj_SAL_CONTAINER_EVAL_HDR.CVH_CONTAINER_NO;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_SERIAL_NO   =   Obj_SAL_CONTAINER_EVAL_HDR.CVH_SERIAL_NO;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_REMARKS   =   Obj_SAL_CONTAINER_EVAL_HDR.CVH_REMARKS;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_CHECK_LIST_GROUP   =   Obj_SAL_CONTAINER_EVAL_HDR.CVH_CHECK_LIST_GROUP;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_ACTIVE   =   Obj_SAL_CONTAINER_EVAL_HDR.CVH_ACTIVE;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_STATUS = Obj_SAL_CONTAINER_EVAL_HDR.CVH_STATUS;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_MOD_BY   =   Obj_SAL_CONTAINER_EVAL_HDR.CVH_MOD_BY;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_IN_TIME = Obj_SAL_CONTAINER_EVAL_HDR.CVH_IN_TIME;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_MOD_DT  = DateTime.Now;
                            Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_COMPANY=Obj_SAL_CONTAINER_EVAL_HDR.CVH_COMPANY;

                            retval = Old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_PK;
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
        /// delete list of Container Evaluation Header
        /// </summary>
        /// <param name="SALCONTAINEREVALHDRobj"></param>
        /// <returns></returns>
        public int DeleteContainerEvaluation(List<SAL_CONTAINER_EVAL_HDR> SalContainerEvelHdrList)
        {
            //Gets or sets old User details
            SAL_CONTAINER_EVAL_HDR old_SAL_CONTAINER_EVAL_HDR_Obj;
             List<ADM_CHECK_LIST_TRX_DTL> old_ADM_CHECK_LIST_TRX_DTL_Obj;
           ADM_CHECK_LIST_TRX_HDR old_ADM_CHECK_LIST_TRX_HDR_Obj;
            try
            {
                //Iterating through Users list for delete
                foreach (SAL_CONTAINER_EVAL_HDR SAL_CONTAINER_EVAL_HDR_Obj in SalContainerEvelHdrList)
                {
                    //Get User country details with pk and last modified date time used for concurrency checking
                    old_SAL_CONTAINER_EVAL_HDR_Obj = currentEntity.SAL_CONTAINER_EVAL_HDR.SingleOrDefault(chi => chi.CVH_PK == SAL_CONTAINER_EVAL_HDR_Obj.CVH_PK);//&& rms.mod == EmpEmployeeMstObj.empMod_On
                    //If oldRmsuObj is null then,anyone modified or deleted the record
                    if (old_SAL_CONTAINER_EVAL_HDR_Obj != null)
                    {
                        old_ADM_CHECK_LIST_TRX_HDR_Obj = currentEntity.ADM_CHECK_LIST_TRX_HDR.SingleOrDefault(hdr => hdr.CLH_TRX_PK == old_SAL_CONTAINER_EVAL_HDR_Obj.CVH_PK && hdr.CLH_TRX_TYPE == ApplicationType.CNTEVAL);
                        if(old_ADM_CHECK_LIST_TRX_HDR_Obj!=null)
                        {
                            old_ADM_CHECK_LIST_TRX_DTL_Obj= currentEntity.ADM_CHECK_LIST_TRX_DTL.Where(dtl =>dtl.CLD_TRX_HDR == old_ADM_CHECK_LIST_TRX_HDR_Obj.CLH_PK).ToList();
                            foreach(ADM_CHECK_LIST_TRX_DTL item in old_ADM_CHECK_LIST_TRX_DTL_Obj)
                            {
                                currentEntity.ADM_CHECK_LIST_TRX_DTL.DeleteObject(item);
                            }
                            currentEntity.ADM_CHECK_LIST_TRX_HDR.DeleteObject(old_ADM_CHECK_LIST_TRX_HDR_Obj);
                        }
                        currentEntity.SAL_CONTAINER_EVAL_HDR.DeleteObject(old_SAL_CONTAINER_EVAL_HDR_Obj);
                    }
                    else
                    {
                        //Throws already modified or deleted by other user exception
                        throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                    }
                }
                //delete successful
                return 1;
            }
            //Handler for concurrency exception
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
            //Handler for unknown exception
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Get list of Vendors
        /// </summary>
        /// <param name="PUR_VENDOR_MSTobj"></param>
        /// <returns></returns>
        public List<PUR_VENDOR_MST> GetCompany(PUR_VENDOR_MST PUR_VENDOR_MSTobj)
        {
            List<PUR_VENDOR_MST> PUR_VENDOR_MSTLst = new List<PUR_VENDOR_MST>();
            try
            {
                PUR_VENDOR_MSTLst = (from ven in this.currentEntity.PUR_VENDOR_MST
                                     where ven.VEN_ACTIVE == PUR_VENDOR_MSTobj.VEN_ACTIVE
                                           && ven.VEN_PK == (PUR_VENDOR_MSTobj.VEN_PK > 0 ? PUR_VENDOR_MSTobj.VEN_PK : ven.VEN_PK)
                                     select ven).ToList();

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

            return PUR_VENDOR_MSTLst;
            //throw new NotImplementedException();
        }

        
        #endregion
    }
}
