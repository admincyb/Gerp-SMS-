using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;

namespace ERPManager
{
    public class InvItemGroupMstManager :IInvItemGroupMstManager
    {

        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Bank AdmPackingMaster Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public InvItemGroupMstManager(ERPEntities currentEntity)
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

        public List<INV_ITEM_GROUP_MST> GetInvItemGroupMst(INV_ITEM_GROUP_MST InvItemMstGroupObj, ServiceUtility utilityObj)
        {
            List<INV_ITEM_GROUP_MST> INV_ITEM_GROUP_MST_Obj = new List<INV_ITEM_GROUP_MST>();
            IQueryable<INV_ITEM_GROUP_MST> INV_ITEM_GROUP_MST_qry;
            try
            {
                INV_ITEM_GROUP_MST_qry = (from inv in this.currentEntity.INV_ITEM_GROUP_MST
                                    where inv.IGM_PK == (InvItemMstGroupObj.IGM_PK > 0 ? InvItemMstGroupObj.IGM_PK : inv.IGM_PK)
                                    && inv.IGM_NAME.StartsWith(InvItemMstGroupObj.IGM_NAME)
                                    && inv.IGM_CODE.StartsWith(InvItemMstGroupObj.IGM_CODE)
                                    && inv.IGM_GROUP_TYPE == InvItemMstGroupObj.IGM_GROUP_TYPE                                    
                                    select inv);

                if (utilityObj.FilterBy == DataFieldRes.ProductGroupCode)
                    INV_ITEM_GROUP_MST_qry = INV_ITEM_GROUP_MST_qry.Where(a => a.IGM_CODE.StartsWith(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.ProductGroupName)
                    INV_ITEM_GROUP_MST_qry = INV_ITEM_GROUP_MST_qry.Where(a => a.IGM_NAME.StartsWith(utilityObj.FilterValue));

                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = INV_ITEM_GROUP_MST_qry.Count();

                //Apply Paging And Sorting for grid Purpose
                INV_ITEM_GROUP_MST_Obj = INV_ITEM_GROUP_MST_qry.SortRecords<INV_ITEM_GROUP_MST>(utilityObj).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return INV_ITEM_GROUP_MST_Obj;
        }

        public int SaveInvItemGroupMst(List<INV_ITEM_GROUP_MST> InvItemMstGroupList, List<INV_ITEM_MST> InvItemMstList)
        {
            // Gets or sets save status
            int retval;
            int indpk;

            int? max_INV_ITEM_GROUP_PK;

            INV_ITEM_GROUP_MST old_INV_ITEM_GROUP_MST_Obj;
            INV_ITEM_MST old_INV_ITEM_MST_Obj;
            try
            {
                // Sets save status zero,save failed
                retval = 0;

                // Iterating through InvItemGroupMst list
                foreach (INV_ITEM_GROUP_MST INV_ITEM_GROUP_MST_Obj in InvItemMstGroupList)
                {
                    // check InvItemGroupMst pk is zero,insert InvItemGroupMst to db context
                    if (INV_ITEM_GROUP_MST_Obj.IGM_PK == 0)
                    {
                        // Gets last InvItemGroupMst pk
                        max_INV_ITEM_GROUP_PK = this.currentEntity.INV_ITEM_GROUP_MST.Max(inv => (int?)inv.IGM_PK);

                        // Sets return value as next InvItemGroupMst pk
                        retval = Convert.ToInt16((max_INV_ITEM_GROUP_PK.HasValue ? max_INV_ITEM_GROUP_PK.Value + 1 : 1));

                        // Sets next InvItemGroupMst pk
                        INV_ITEM_GROUP_MST_Obj.IGM_PK = retval;

                        // Sets InvItemGroupMst modified date time as current date time
                        INV_ITEM_GROUP_MST_Obj.IGM_MOD_DT = DateTime.Now;

                        INV_ITEM_GROUP_MST_Obj.IGM_CRTD_DT = DateTime.Now;

                        // Add new InvItemGroupMst to the db context
                        this.currentEntity.INV_ITEM_GROUP_MST.AddObject(INV_ITEM_GROUP_MST_Obj);
                    }
                    else
                    {
                        // updating InvItemGroupMst
                        // Get current InvItemGroupMst using InvItemGroupMst pk and last modified date time,used for concurrency checking
                        old_INV_ITEM_GROUP_MST_Obj = currentEntity.INV_ITEM_GROUP_MST.SingleOrDefault(inv => inv.IGM_PK == INV_ITEM_GROUP_MST_Obj.IGM_PK && inv.IGM_MOD_DT == INV_ITEM_GROUP_MST_Obj.IGM_MOD_DT);

                        // If oldInvItemGroupMstObj is null then,anyone modified or deleted the record
                        if (old_INV_ITEM_GROUP_MST_Obj != null)
                        {
                            // Update InvItemGroupMst

                            old_INV_ITEM_GROUP_MST_Obj.IGM_CODE = INV_ITEM_GROUP_MST_Obj.IGM_CODE;
                            old_INV_ITEM_GROUP_MST_Obj.IGM_NAME = INV_ITEM_GROUP_MST_Obj.IGM_NAME;
                            old_INV_ITEM_GROUP_MST_Obj.IGM_DESC = INV_ITEM_GROUP_MST_Obj.IGM_DESC;
                            old_INV_ITEM_GROUP_MST_Obj.IGM_ACTIVE = INV_ITEM_GROUP_MST_Obj.IGM_ACTIVE;
                            old_INV_ITEM_GROUP_MST_Obj.IGM_BIZUNIT = INV_ITEM_GROUP_MST_Obj.IGM_BIZUNIT;
                            old_INV_ITEM_GROUP_MST_Obj.IGM_MOD_BY = INV_ITEM_GROUP_MST_Obj.IGM_MOD_BY;
                            old_INV_ITEM_GROUP_MST_Obj.IGM_MOD_DT = DateTime.Now;
                            old_INV_ITEM_GROUP_MST_Obj.IGM_GROUP_TYPE = INV_ITEM_GROUP_MST_Obj.IGM_GROUP_TYPE;//Biju
                            // Sets return value as InvItemGroupMst pk
                            retval = old_INV_ITEM_GROUP_MST_Obj.IGM_PK;
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }
                }
                foreach (INV_ITEM_MST INV_ITEM_MST_Obj in InvItemMstList)
                {
                    if (INV_ITEM_MST_Obj.ITM_PK != 0)
                    {

                        old_INV_ITEM_MST_Obj = currentEntity.INV_ITEM_MST.SingleOrDefault(ind => ind.ITM_PK == INV_ITEM_MST_Obj.ITM_PK);

                        if (old_INV_ITEM_MST_Obj != null)
                        {
                            old_INV_ITEM_MST_Obj.ITM_GROUP = INV_ITEM_MST_Obj.ITM_GROUP == 0 ? retval : INV_ITEM_MST_Obj.ITM_GROUP;
                            old_INV_ITEM_MST_Obj.ITM_BIZUNIT = INV_ITEM_MST_Obj.ITM_BIZUNIT;
                            old_INV_ITEM_MST_Obj.ITM_MOD_BY = INV_ITEM_MST_Obj.ITM_MOD_BY;
                            old_INV_ITEM_MST_Obj.ITM_MOD_DT = DateTime.Now;

                        }
                    }

                }

                // return InvItemGroupMst pk
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

        public int DeleteInvItemGroupMst(List<INV_ITEM_GROUP_MST> InvItemMstGroupList)
        {
            int retval = 0;
            INV_ITEM_GROUP_MST Old_INV_ITEM_GROUP_MST_Obj;

            try
            {
                foreach (INV_ITEM_GROUP_MST INV_ITEM_GROUP_MST_Obj in InvItemMstGroupList)
                {
                    Old_INV_ITEM_GROUP_MST_Obj = currentEntity.INV_ITEM_GROUP_MST.SingleOrDefault(sah => sah.IGM_PK == INV_ITEM_GROUP_MST_Obj.IGM_PK && sah.IGM_MOD_DT == INV_ITEM_GROUP_MST_Obj.IGM_MOD_DT);
                    if (Old_INV_ITEM_GROUP_MST_Obj != null)
                    {
                        this.currentEntity.INV_ITEM_GROUP_MST.DeleteObject(Old_INV_ITEM_GROUP_MST_Obj);
                        retval = 1;
                    }
                    else
                    {
                        // throws exception already deleted or modified by other user
                        throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
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
        public List<INV_ITEM_MST> GetInvItemMst(INV_ITEM_MST InvItemMstObj,ItemCategory itemCategory = ItemCategory.FinishedGoods)
        {
            int? invCodePK = null;
            string itCode="FG";
            if(itemCategory == ItemCategory.Former)
                itCode="FORMER";
            INV_ITEM_CATEGORY invItemCategoryObj = this.currentEntity.INV_ITEM_CATEGORY.SingleOrDefault(itm => itm.ITC_CODE == itCode);
            if (invItemCategoryObj != null)
            {
                invCodePK = invItemCategoryObj.ITC_PK;

            }
         

            List<INV_ITEM_MST> INV_ITEM_MSTLst = new List<INV_ITEM_MST>();
            try
            {
                INV_ITEM_MSTLst = (from INV in this.currentEntity.INV_ITEM_MST
                                   where INV.ITM_ACTIVE == InvItemMstObj.ITM_ACTIVE
                                         && (InvItemMstObj.ITM_GROUP > 0 
                                            ? (INV.ITM_GROUP == InvItemMstObj.ITM_GROUP || INV.ITM_GROUP == null ) 
                                            : INV.ITM_GROUP == null)
                                       //&& (INV.ITM_GROUP == (InvItemMstObj.ITM_GROUP > 0 ? InvItemMstObj.ITM_GROUP : null)
                                       //   || INV.ITM_GROUP == null
                                       //  )                                       
                                          && INV.ITM_CATEGORY == (invCodePK.HasValue ? invCodePK : INV.ITM_CATEGORY)
                                   select INV).OrderBy(c => c.ITM_NAME).ToList();

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

            return INV_ITEM_MSTLst;
            //throw new NotImplementedException();
        }

        public long? SaveDocAttachemts(List<ADM_DOC_ATTACH> DocAttachList, int DocTaskId, int Task)
        {
            long? retval = 0;
            int? maxID;
            List<ADM_DOC_ATTACH> Old_DocList;
            ADM_DOC_ATTACH OldADM_DOC_ATTACH_Obj;
            try
            {


                if (DocAttachList!= null && DocAttachList.Count > 0)
                {
                    maxID = currentEntity.ADM_DOC_ATTACH.Max(v => (int?)v.DOC_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    List<int> pks = (from old1 in DocAttachList
                                     select old1.DOC_PK).ToList();
                    //long? DOC_TASK_ID = DocAttachList[0].DOC_TASK_ID;

                    /**/
                    Old_DocList = (from oldp in this.currentEntity.ADM_DOC_ATTACH
                                   where oldp.DOC_TASK_ID == DocTaskId
                                   && !pks.Contains(oldp.DOC_PK)
                                   select oldp).ToList();

                    foreach (ADM_DOC_ATTACH old_ADM_DOC_ATTACH in Old_DocList)
                    {
                        this.currentEntity.ADM_DOC_ATTACH.DeleteObject(old_ADM_DOC_ATTACH);
                    }

                    foreach (ADM_DOC_ATTACH ADM_DOC_ATTACH_Obj in DocAttachList)
                    {
                        if (ADM_DOC_ATTACH_Obj.DOC_PK == -1)
                            break;
                        if (ADM_DOC_ATTACH_Obj.DOC_PK == 0) //  INSERT NEW RECORD
                        {
                            ADM_DOC_ATTACH_Obj.DOC_PK = maxID.Value;
                            ADM_DOC_ATTACH_Obj.DOC_TASK_ID = DocTaskId;
                            currentEntity.ADM_DOC_ATTACH.AddObject(ADM_DOC_ATTACH_Obj);
                            maxID++;
                            retval = ADM_DOC_ATTACH_Obj.DOC_PK;
                        }
                        else //UPDATE EXISTING RECORD
                        {
                            // updating ADM_DOC_ATTACH
                            // Get current ADM_DOC_ATTACH using ADM_DOC_ATTACH pk and last modified date time,used for concurrency checking
                            OldADM_DOC_ATTACH_Obj = currentEntity.ADM_DOC_ATTACH.SingleOrDefault(sah => sah.DOC_PK == ADM_DOC_ATTACH_Obj.DOC_PK);

                            // If oldAdmDocAttachObj is null then,anyone modified or deleted the record
                            if (OldADM_DOC_ATTACH_Obj != null)
                            {
                                // Update ADM_DOC_ATTACH
                                OldADM_DOC_ATTACH_Obj.DOC_ACTIVE = ADM_DOC_ATTACH_Obj.DOC_ACTIVE;
                                OldADM_DOC_ATTACH_Obj.DOC_BIZUNIT = ADM_DOC_ATTACH_Obj.DOC_BIZUNIT;
                                //OldADM_DOC_ATTACH_Obj.DOC_CRTD_BY = ADM_DOC_ATTACH_Obj.DOC_CRTD_BY;
                                OldADM_DOC_ATTACH_Obj.DOC_DESC = ADM_DOC_ATTACH_Obj.DOC_DESC;
                                OldADM_DOC_ATTACH_Obj.DOC_MOD_BY = ADM_DOC_ATTACH_Obj.DOC_MOD_BY;
                                OldADM_DOC_ATTACH_Obj.DOC_MOD_DT = ADM_DOC_ATTACH_Obj.DOC_MOD_DT;
                                OldADM_DOC_ATTACH_Obj.DOC_MODULE = ADM_DOC_ATTACH_Obj.DOC_MODULE;
                                OldADM_DOC_ATTACH_Obj.DOC_NAME = ADM_DOC_ATTACH_Obj.DOC_NAME;
                                OldADM_DOC_ATTACH_Obj.DOC_NAME = ADM_DOC_ATTACH_Obj.DOC_NAME;
                                OldADM_DOC_ATTACH_Obj.DOC_PATH = ADM_DOC_ATTACH_Obj.DOC_PATH;
                                OldADM_DOC_ATTACH_Obj.DOC_SEQ_NO = ADM_DOC_ATTACH_Obj.DOC_SEQ_NO;
                                OldADM_DOC_ATTACH_Obj.DOC_TASK = ADM_DOC_ATTACH_Obj.DOC_TASK;
                                //OldADM_DOC_ATTACH_Obj.DOC_TASK_ID = ADM_DOC_ATTACH_Obj.DOC_TASK_ID;
                                OldADM_DOC_ATTACH_Obj.DOC_TITLE = ADM_DOC_ATTACH_Obj.DOC_TITLE;
                                OldADM_DOC_ATTACH_Obj.DOC_TYPE = ADM_DOC_ATTACH_Obj.DOC_TYPE;

                                // Sets return value as ADM_DOC_ATTACH pk
                                retval = ADM_DOC_ATTACH_Obj.DOC_PK;
                            }
                        }
                    }

                }
                else
                {
                    Old_DocList = (from oldp in this.currentEntity.ADM_DOC_ATTACH
                                   where oldp.DOC_TASK_ID == DocTaskId
                                   && oldp.DOC_TASK == Task
                                   select oldp).ToList();
                    if (Old_DocList != null && Old_DocList.Count > 0)
                    {
                        foreach (ADM_DOC_ATTACH old_ADM_DOC_ATTACH in Old_DocList)
                        {
                            this.currentEntity.ADM_DOC_ATTACH.DeleteObject(old_ADM_DOC_ATTACH);
                        }
                    }
                }
                return retval;

            } //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }


        }

        public List<ADM_DOC_ATTACH> GetDocAttachments(ADM_DOC_ATTACH admDocAttachObj, ServiceUtility serviceUtilityObj)
        {
            List<ADM_DOC_ATTACH> DocList = null;
            IQueryable<ADM_DOC_ATTACH> ADM_DOC_ATTACHQuery;
            //int pageSize;
            //int totalCount;
            try
            {

                ADM_DOC_ATTACHQuery = (from inv in this.currentEntity.ADM_DOC_ATTACH
                                       where inv.DOC_TASK_ID == admDocAttachObj.DOC_TASK_ID && inv.DOC_TASK == admDocAttachObj.DOC_TASK
                                       select inv);
                DocList = ADM_DOC_ATTACHQuery.ToList();
                return DocList;
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
        /// Function Used For Product Group Duplication Checking
        /// </summary>
        /// <param name="invItemGroupMstMgrObj"></param>
        /// <returns></returns>
        public bool IsProductGroupExist(INV_ITEM_GROUP_MST invItemGroupMstMgrObj)
        {
            try
            {
                bool IsExist = false;

                var resultFieldsObj = (from grp in this.currentEntity.INV_ITEM_GROUP_MST
                                       where grp.IGM_PK != invItemGroupMstMgrObj.IGM_PK && grp.IGM_NAME == invItemGroupMstMgrObj.IGM_NAME
                                       select grp).ToList();
                if (resultFieldsObj != null && resultFieldsObj.Count > 0)
                    IsExist = true;
                return IsExist;
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



    }
}
