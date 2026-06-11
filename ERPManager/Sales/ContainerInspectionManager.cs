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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ContainerInspectionManager" in both code and config file together.
    public class ContainerInspectionManager :IContainerInspectionManager 
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
        public ContainerInspectionManager(ERPEntities currentEntity)
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
        /// Get list of Container Inspection Header
        /// </summary>
        /// <param name="SALCONTAINEREVALHDRobj"></param>
        /// <returns></returns>
        public List<SAL_CONTAINER_INSP_HDR> GetContainerInspectionList(SAL_CONTAINER_INSP_HDR SALCONTAINERINSPHDRobj, ServiceUtility utilityObj)
        {
            List<SAL_CONTAINER_INSP_HDR> SAL_CONTAINER_INSP_HDR_Obj;
            IQueryable<SAL_CONTAINER_INSP_HDR> Obj_SAL_CONTAINER_INSP_HDR_Qry;
            try
            {


                Obj_SAL_CONTAINER_INSP_HDR_Qry = (from cvh in this.currentEntity.SAL_CONTAINER_INSP_HDR
                                                  where cvh.CSH_ACTIVE == SALCONTAINERINSPHDRobj.CSH_ACTIVE
                                                      && cvh.CSH_SHIPPING_PLAN == (SALCONTAINERINSPHDRobj.CSH_SHIPPING_PLAN > 0 ? SALCONTAINERINSPHDRobj.CSH_SHIPPING_PLAN : cvh.CSH_SHIPPING_PLAN)
                                                  select cvh);
                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = Obj_SAL_CONTAINER_INSP_HDR_Qry.Count();

                //Apply Paging And Sorting for grid Purpose
                SAL_CONTAINER_INSP_HDR_Obj = Obj_SAL_CONTAINER_INSP_HDR_Qry.SortRecords<SAL_CONTAINER_INSP_HDR>(utilityObj).ToList();


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
            return SAL_CONTAINER_INSP_HDR_Obj;
        }

        /// <summary>
        /// save list of Container Inspection Header
        /// </summary>
        /// <param name="SALCONTAINEREVALHDRobj"></param>
        /// <returns></returns>
        public int SaveContainerInspection(List<SAL_CONTAINER_INSP_HDR> SALCONTAINERINSPHDRobj)
        {
            int retval = 0;
            int? max_PK;

            SAL_CONTAINER_INSP_HDR Old_SAL_CONTAINER_INSP_HDR_Obj;

            try
            {
                foreach (SAL_CONTAINER_INSP_HDR Obj_SAL_CONTAINER_INSP_HDR in SALCONTAINERINSPHDRobj)
                {
                    if (Obj_SAL_CONTAINER_INSP_HDR.CSH_PK == 0)
                    {
                        #region Check whether the Container Inspection is already created or not
                        var objShipping = (from shp in this.currentEntity.SAL_CONTAINER_INSP_HDR where shp.CSH_SHIPPING_PLAN == Obj_SAL_CONTAINER_INSP_HDR.CSH_SHIPPING_PLAN select shp).ToList();
                        if (objShipping != null && objShipping.Count > 0)
                        {
                            retval = -8;//Concurrency : Container inspection already created
                            break;
                        }
                        #endregion

                        // Gets last SAL_DESPATCH_HDR pk
                        max_PK = this.currentEntity.SAL_CONTAINER_INSP_HDR.Max(dph => (int?)dph.CSH_PK);

                        // Sets return value as next SAL_DESPATCH_HDR pk
                        retval = (max_PK.HasValue ? max_PK.Value + 1 : 1);

                        Obj_SAL_CONTAINER_INSP_HDR.CSH_PK = retval;
                        Obj_SAL_CONTAINER_INSP_HDR.CSH_CRTD_DT = DateTime.Now;
                        Obj_SAL_CONTAINER_INSP_HDR.CSH_MOD_DT = DateTime.Now;

                        // Add new SAL_DESPATCH_HDR to the db context
                        this.currentEntity.SAL_CONTAINER_INSP_HDR.AddObject(Obj_SAL_CONTAINER_INSP_HDR);
                    }
                    else
                    {
                        Old_SAL_CONTAINER_INSP_HDR_Obj = currentEntity.SAL_CONTAINER_INSP_HDR.SingleOrDefault(dph => dph.CSH_PK == Obj_SAL_CONTAINER_INSP_HDR.CSH_PK && dph.CSH_MOD_DT == Obj_SAL_CONTAINER_INSP_HDR.CSH_MOD_DT);

                        if (Old_SAL_CONTAINER_INSP_HDR_Obj != null)
                        {
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_NO = Obj_SAL_CONTAINER_INSP_HDR.CSH_NO;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_DATE = Obj_SAL_CONTAINER_INSP_HDR.CSH_DATE;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_CONTAINER_NO = Obj_SAL_CONTAINER_INSP_HDR.CSH_CONTAINER_NO;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_SEAL_NO = Obj_SAL_CONTAINER_INSP_HDR.CSH_SEAL_NO;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_REMARKS = Obj_SAL_CONTAINER_INSP_HDR.CSH_REMARKS;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_DESPATCH = Obj_SAL_CONTAINER_INSP_HDR.CSH_DESPATCH;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_ISO_PAS = Obj_SAL_CONTAINER_INSP_HDR.CSH_ISO_PAS; 
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_INSPECTOR = Obj_SAL_CONTAINER_INSP_HDR.CSH_INSPECTOR;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_INSPECTOR_NAME = Obj_SAL_CONTAINER_INSP_HDR.CSH_INSPECTOR_NAME;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_INSPECTED_ON = Obj_SAL_CONTAINER_INSP_HDR.CSH_INSPECTED_ON;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_SUPERVISOR = Obj_SAL_CONTAINER_INSP_HDR.CSH_SUPERVISOR;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_SUPERVISOR_NAME = Obj_SAL_CONTAINER_INSP_HDR.CSH_SUPERVISOR_NAME;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_SUPERVISED_ON = Obj_SAL_CONTAINER_INSP_HDR.CSH_SUPERVISED_ON;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_CHECK_LIST_GROUP = Obj_SAL_CONTAINER_INSP_HDR.CSH_CHECK_LIST_GROUP;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_ACTIVE = Obj_SAL_CONTAINER_INSP_HDR.CSH_ACTIVE;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_STATUS = Obj_SAL_CONTAINER_INSP_HDR.CSH_STATUS;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_MOD_BY = Obj_SAL_CONTAINER_INSP_HDR.CSH_MOD_BY;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_MOD_DT = DateTime.Now;
                            Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_COMPANY = Obj_SAL_CONTAINER_INSP_HDR.CSH_COMPANY;

                            retval = Old_SAL_CONTAINER_INSP_HDR_Obj.CSH_PK;
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
        /// save list of Container Inspection Header
        /// </summary>
        /// <param name="SALCONTAINEREVALHDRobj"></param>
        /// <returns></returns>
        public int DeleteContainerInspection(List<SAL_CONTAINER_INSP_HDR> SalContainerInspHdrList)
        {
            //Gets or sets old User details
            SAL_CONTAINER_INSP_HDR old_SAL_CONTAINER_INSP_HDR_Obj;
            List<ADM_CHECK_LIST_TRX_DTL> old_ADM_CHECK_LIST_TRX_DTL_Obj;
            ADM_CHECK_LIST_TRX_HDR old_ADM_CHECK_LIST_TRX_HDR_Obj;
            try
            {
                //Iterating through Users list for delete
                foreach (SAL_CONTAINER_INSP_HDR SAL_CONTAINER_INSP_HDR_Obj in SalContainerInspHdrList)
                {
                    //Get User country details with pk and last modified date time used for concurrency checking
                    old_SAL_CONTAINER_INSP_HDR_Obj = currentEntity.SAL_CONTAINER_INSP_HDR.SingleOrDefault(chi => chi.CSH_PK == SAL_CONTAINER_INSP_HDR_Obj.CSH_PK);//&& rms.mod == EmpEmployeeMstObj.empMod_On
                    //If oldRmsuObj is null then,anyone modified or deleted the record
                    if (old_SAL_CONTAINER_INSP_HDR_Obj != null)
                    {
                        old_ADM_CHECK_LIST_TRX_HDR_Obj = currentEntity.ADM_CHECK_LIST_TRX_HDR.SingleOrDefault(hdr => hdr.CLH_TRX_PK == old_SAL_CONTAINER_INSP_HDR_Obj.CSH_PK && hdr.CLH_TRX_TYPE == ApplicationType.CNTINSP);
                        if (old_ADM_CHECK_LIST_TRX_HDR_Obj != null)
                        {
                            old_ADM_CHECK_LIST_TRX_DTL_Obj = currentEntity.ADM_CHECK_LIST_TRX_DTL.Where(dtl => dtl.CLD_TRX_HDR == old_ADM_CHECK_LIST_TRX_HDR_Obj.CLH_PK).ToList();
                            foreach (ADM_CHECK_LIST_TRX_DTL item in old_ADM_CHECK_LIST_TRX_DTL_Obj)
                            {
                                currentEntity.ADM_CHECK_LIST_TRX_DTL.DeleteObject(item);
                            }
                            currentEntity.ADM_CHECK_LIST_TRX_HDR.DeleteObject(old_ADM_CHECK_LIST_TRX_HDR_Obj);
                        }
                        currentEntity.SAL_CONTAINER_INSP_HDR.DeleteObject(old_SAL_CONTAINER_INSP_HDR_Obj);
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

        

        public List<SAL_DESPATCH_HDR> GetDeliveryOrders(SAL_DESPATCH_HDR SAL_DESPATCH_HDRobj)
        {
            List<SAL_DESPATCH_HDR> SAL_DESPATCH_HDRLst = new List<SAL_DESPATCH_HDR>();
            try
            {
                SAL_DESPATCH_HDRLst = (from dph in this.currentEntity.SAL_DESPATCH_HDR
                                       where dph.DPH_ACTIVE == SAL_DESPATCH_HDRobj.DPH_ACTIVE
                                           && dph.DPH_PK == (SAL_DESPATCH_HDRobj.DPH_PK > 0 ? SAL_DESPATCH_HDRobj.DPH_PK : dph.DPH_PK)
                                           && dph.DPH_STATUS == (SAL_DESPATCH_HDRobj.DPH_STATUS > 0 ? SAL_DESPATCH_HDRobj.DPH_STATUS : dph.DPH_STATUS)
                                       select dph).ToList();

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

            return SAL_DESPATCH_HDRLst;
            //throw new NotImplementedException();
        }
        #endregion
      
    }
}
