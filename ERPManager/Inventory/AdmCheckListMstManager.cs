using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;
using BusinessObject.Inventory;

namespace ERPManager.Inventory
{
    public class AdmCheckListMstManager : IAdmCheckListMstManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// AdmCheckListMstManager Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>

        public AdmCheckListMstManager(ERPEntities currentEntity)
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

        public List<ADM_CHECK_LIST_TYPE_CFG> GetCheckListTypes(ADM_CHECK_LIST_TYPE_CFG CheckListTypesObj)
        {
            List<ADM_CHECK_LIST_TYPE_CFG> ADM_CHECK_LIST_TYPE_CFG_Obj = new List<ADM_CHECK_LIST_TYPE_CFG>();

            try
            {
                ADM_CHECK_LIST_TYPE_CFG_Obj = (from  clt in this.currentEntity.ADM_CHECK_LIST_TYPE_CFG
                                               where (clt.CLT_PK == CheckListTypesObj.CLT_PK || clt.CLT_CODE == CheckListTypesObj.CLT_CODE)
                                               && clt.CLT_ACTIVE == CheckListTypesObj.CLT_ACTIVE
                                               select clt).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);

            }

            return ADM_CHECK_LIST_TYPE_CFG_Obj;
        }

        public List<ADM_CHECK_LIST_GROUP_MST> GetCheckListGroups(ADM_CHECK_LIST_GROUP_MST CheckListGroupsObj, ServiceUtility utilityObj)
        {
            List<ADM_CHECK_LIST_GROUP_MST> ADM_CHECK_LIST_GROUP_MST_Obj = new List<ADM_CHECK_LIST_GROUP_MST>();

            try
            {
                ADM_CHECK_LIST_GROUP_MST_Obj = (from cgm in this.currentEntity.ADM_CHECK_LIST_GROUP_MST
                                                where cgm.CGM_PK == (CheckListGroupsObj.CGM_PK > 0 ? CheckListGroupsObj.CGM_PK : cgm.CGM_PK)
                                                && cgm.CGM_ACTIVE == CheckListGroupsObj.CGM_ACTIVE
                                                && cgm.CGM_TYPE == CheckListGroupsObj.CGM_TYPE 
                                                select cgm).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);

            }

            return ADM_CHECK_LIST_GROUP_MST_Obj;
        }

        public int SaveCheckListGroups(List<ADM_CHECK_LIST_GROUP_MST> CheckListGroupsList)
        {
            int retval;
            int? max_CGM_PK;
            ADM_CHECK_LIST_GROUP_MST old_ADM_CHECK_LIST_GROUP_MST_Obj;
            try
            {
                // Sets save status zero,save failed
                retval = 0;
                foreach (ADM_CHECK_LIST_GROUP_MST ADM_CHECK_LIST_GROUP_MST_Obj in CheckListGroupsList)
                {
                    if (ADM_CHECK_LIST_GROUP_MST_Obj.CGM_PK == 0)
                    {
                        max_CGM_PK = this.currentEntity.ADM_CHECK_LIST_GROUP_MST.Max(inv => (int?)inv.CGM_PK);

                        retval = Convert.ToInt16((max_CGM_PK.HasValue ? max_CGM_PK.Value + 1 : 1));

                        ADM_CHECK_LIST_GROUP_MST_Obj.CGM_PK = retval;

                        ADM_CHECK_LIST_GROUP_MST_Obj.CGM_MOD_DT = DateTime.Now;

                        // Add new InvMaterialMst to the db context
                        this.currentEntity.ADM_CHECK_LIST_GROUP_MST.AddObject(ADM_CHECK_LIST_GROUP_MST_Obj);
                    }
                    else
                    {
                        old_ADM_CHECK_LIST_GROUP_MST_Obj = currentEntity.ADM_CHECK_LIST_GROUP_MST.SingleOrDefault(cgm => cgm.CGM_PK == ADM_CHECK_LIST_GROUP_MST_Obj.CGM_PK );

                        if (old_ADM_CHECK_LIST_GROUP_MST_Obj != null)
                        {
                            // Update CheckListGrouMst

                            old_ADM_CHECK_LIST_GROUP_MST_Obj.CGM_CODE = ADM_CHECK_LIST_GROUP_MST_Obj.CGM_CODE;
                            old_ADM_CHECK_LIST_GROUP_MST_Obj.CGM_NAME = ADM_CHECK_LIST_GROUP_MST_Obj.CGM_NAME;
                            old_ADM_CHECK_LIST_GROUP_MST_Obj.CGM_DESC = ADM_CHECK_LIST_GROUP_MST_Obj.CGM_DESC;
                            old_ADM_CHECK_LIST_GROUP_MST_Obj.CGM_SEQUENCE = ADM_CHECK_LIST_GROUP_MST_Obj.CGM_SEQUENCE;
                            old_ADM_CHECK_LIST_GROUP_MST_Obj.CGM_ACTIVE = ADM_CHECK_LIST_GROUP_MST_Obj.CGM_ACTIVE;
                            old_ADM_CHECK_LIST_GROUP_MST_Obj.CGM_BIZUNIT = ADM_CHECK_LIST_GROUP_MST_Obj.CGM_BIZUNIT;
                            old_ADM_CHECK_LIST_GROUP_MST_Obj.CGM_MOD_BY = ADM_CHECK_LIST_GROUP_MST_Obj.CGM_MOD_BY;
                            old_ADM_CHECK_LIST_GROUP_MST_Obj.CGM_MOD_DT = DateTime.Now;
                            // Sets return value as InvMaterialMst pk
                            retval = ADM_CHECK_LIST_GROUP_MST_Obj.CGM_PK;
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }
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


        public int DeleteCheckListGroups(List<ADM_CHECK_LIST_GROUP_MST> CheckListGroupsList)
        {
            //Gets or sets old User details
            ADM_CHECK_LIST_GROUP_MST old_ADM_CHECK_LIST_GROUP_MST_Obj;
            try
            {
                //Iterating through Users list for delete
                foreach (ADM_CHECK_LIST_GROUP_MST ADM_CHECK_LIST_GROUP_MST_Obj in CheckListGroupsList)
                {
                    //Get User country details with pk and last modified date time used for concurrency checking
                    old_ADM_CHECK_LIST_GROUP_MST_Obj = currentEntity.ADM_CHECK_LIST_GROUP_MST.SingleOrDefault(cgm => cgm.CGM_PK == ADM_CHECK_LIST_GROUP_MST_Obj.CGM_PK);//&& rms.mod == EmpEmployeeMstObj.empMod_On
                    //If oldRmsuObj is null then,anyone modified or deleted the record
                    if (old_ADM_CHECK_LIST_GROUP_MST_Obj != null)
                    {
                        currentEntity.ADM_CHECK_LIST_GROUP_MST.DeleteObject(old_ADM_CHECK_LIST_GROUP_MST_Obj);
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

        public List<CheckListItems> GetCheckListItems(ADM_CHECK_LIST_ITEM_MST CheckListItemsObj, ServiceUtility utilityObj)
        {
            List<CheckListItems> ADM_CHECK_LIST_ITEM_MST_Obj ;
            IQueryable<CheckListItems> Obj_ADM_CHECK_LIST_ITEM_MST_Qry;

            try
            {
                Obj_ADM_CHECK_LIST_ITEM_MST_Qry = (from chi in this.currentEntity.ADM_CHECK_LIST_ITEM_MST
                                                   where chi.CHI_PK == (CheckListItemsObj.CHI_PK > 0 ? CheckListItemsObj.CHI_PK : chi.CHI_PK)
                                                   && chi.CHI_GROUP == CheckListItemsObj.CHI_GROUP
                                                   && chi.CHI_ACTIVE == (
                                                                            ( CheckListItemsObj.CHI_ACTIVE == (byte)BusinessObject.CommonManagement.DbActiveStatus.ALL 
                                                                              || CheckListItemsObj.CHI_ACTIVE == (byte)BusinessObject.CommonManagement.DbActiveStatus.HASPK
                                                                             )
                                                                          ? chi.CHI_ACTIVE 
                                                                          : CheckListItemsObj.CHI_ACTIVE)
                                                   select new CheckListItems
                                                   {
                                                       CHI_PK = chi.CHI_PK,
                                                       CHI_CODE = chi.CHI_CODE,
                                                       CHI_NAME = chi.CHI_NAME,
                                                       CHI_GROUP = chi.CHI_GROUP,
                                                       CHI_DESC = chi.CHI_DESC,
                                                       CHI_SEQUENCE = chi.CHI_SEQUENCE,
                                                       CHI_CONTROL = chi.CHI_CONTROL,
                                                       CHI_ACTIVE = chi.CHI_ACTIVE,
                                                       CHI_MOD_BY = chi.CHI_MOD_BY,
                                                       CHI_MOD_DT = chi.CHI_MOD_DT,
                                                       CHI_BIZUNIT = chi.CHI_BIZUNIT,
                                                       CHI_CONST_GROUP = chi.CHI_CONST_GROUP,
                                                       CTL_NAME = chi.ADM_CONTROLS_CFG.CTL_NAME
                                                   });
                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = Obj_ADM_CHECK_LIST_ITEM_MST_Qry.Count();

                //Apply Paging And Sorting for grid Purpose
                ADM_CHECK_LIST_ITEM_MST_Obj = Obj_ADM_CHECK_LIST_ITEM_MST_Qry.SortRecords<CheckListItems>(utilityObj).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);

            }

            return ADM_CHECK_LIST_ITEM_MST_Obj;
        }

        public int SaveCheckListItems(List<ADM_CHECK_LIST_ITEM_MST> CheckListItemsList)
        {
            int retval;
            int? max_CHI_PK;
            ADM_CHECK_LIST_ITEM_MST old_ADM_CHECK_LIST_ITEM_MST_Obj;
            try
            {
                // Sets save status zero,save failed
                retval = 0;
                foreach (ADM_CHECK_LIST_ITEM_MST ADM_CHECK_LIST_ITEM_MST_Obj in CheckListItemsList)
                {
                    if (ADM_CHECK_LIST_ITEM_MST_Obj.CHI_PK == 0)
                    {
                        max_CHI_PK = this.currentEntity.ADM_CHECK_LIST_ITEM_MST.Max(inv => (int?)inv.CHI_PK);

                        retval = Convert.ToInt16((max_CHI_PK.HasValue ? max_CHI_PK.Value + 1 : 1));

                        ADM_CHECK_LIST_ITEM_MST_Obj.CHI_PK = retval;

                        ADM_CHECK_LIST_ITEM_MST_Obj.CHI_MOD_DT = DateTime.Now;

                        // Add new InvMaterialMst to the db context
                        this.currentEntity.ADM_CHECK_LIST_ITEM_MST.AddObject(ADM_CHECK_LIST_ITEM_MST_Obj);
                    }
                    else
                    {
                        old_ADM_CHECK_LIST_ITEM_MST_Obj = currentEntity.ADM_CHECK_LIST_ITEM_MST.SingleOrDefault(CHI => CHI.CHI_PK == ADM_CHECK_LIST_ITEM_MST_Obj.CHI_PK && CHI.CHI_MOD_DT == ADM_CHECK_LIST_ITEM_MST_Obj.CHI_MOD_DT);

                        if (old_ADM_CHECK_LIST_ITEM_MST_Obj != null)
                        {
                            // Update InvMaterialMst

                            old_ADM_CHECK_LIST_ITEM_MST_Obj.CHI_CODE = ADM_CHECK_LIST_ITEM_MST_Obj.CHI_CODE;
                            old_ADM_CHECK_LIST_ITEM_MST_Obj.CHI_NAME = ADM_CHECK_LIST_ITEM_MST_Obj.CHI_NAME;
                            old_ADM_CHECK_LIST_ITEM_MST_Obj.CHI_GROUP = ADM_CHECK_LIST_ITEM_MST_Obj.CHI_GROUP;
                            old_ADM_CHECK_LIST_ITEM_MST_Obj.CHI_DESC = ADM_CHECK_LIST_ITEM_MST_Obj.CHI_DESC;
                            old_ADM_CHECK_LIST_ITEM_MST_Obj.CHI_SEQUENCE = ADM_CHECK_LIST_ITEM_MST_Obj.CHI_SEQUENCE;
                            old_ADM_CHECK_LIST_ITEM_MST_Obj.CHI_CONST_GROUP = ADM_CHECK_LIST_ITEM_MST_Obj.CHI_CONST_GROUP;
                            old_ADM_CHECK_LIST_ITEM_MST_Obj.CHI_CONTROL = ADM_CHECK_LIST_ITEM_MST_Obj.CHI_CONTROL;
                            old_ADM_CHECK_LIST_ITEM_MST_Obj.CHI_ACTIVE = ADM_CHECK_LIST_ITEM_MST_Obj.CHI_ACTIVE;
                            old_ADM_CHECK_LIST_ITEM_MST_Obj.CHI_BIZUNIT = ADM_CHECK_LIST_ITEM_MST_Obj.CHI_BIZUNIT;
                            old_ADM_CHECK_LIST_ITEM_MST_Obj.CHI_MOD_BY = ADM_CHECK_LIST_ITEM_MST_Obj.CHI_MOD_BY;
                            old_ADM_CHECK_LIST_ITEM_MST_Obj.CHI_MOD_DT = DateTime.Now;

                            // Sets return value as InvMaterialMst pk
                            retval = ADM_CHECK_LIST_ITEM_MST_Obj.CHI_PK;
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }
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
        public int DeleteCheckListItems(List<ADM_CHECK_LIST_ITEM_MST> CheckListItemsList)
        {
            //Gets or sets old User details
            ADM_CHECK_LIST_ITEM_MST old_ADM_CHECK_LIST_ITEM_MST_Obj;
            try
            {
                //Iterating through Users list for delete
                foreach (ADM_CHECK_LIST_ITEM_MST ADM_CHECK_LIST_ITEM_MST_Obj in CheckListItemsList)
                {
                    //Get User country details with pk and last modified date time used for concurrency checking
                    old_ADM_CHECK_LIST_ITEM_MST_Obj = currentEntity.ADM_CHECK_LIST_ITEM_MST.SingleOrDefault(chi => chi.CHI_PK == ADM_CHECK_LIST_ITEM_MST_Obj.CHI_PK);//&& rms.mod == EmpEmployeeMstObj.empMod_On
                    //If oldRmsuObj is null then,anyone modified or deleted the record
                    if (old_ADM_CHECK_LIST_ITEM_MST_Obj != null)
                    {
                        currentEntity.ADM_CHECK_LIST_ITEM_MST.DeleteObject(old_ADM_CHECK_LIST_ITEM_MST_Obj);
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

        #endregion
    }
}
