using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;

namespace ERPManager.Inventory
{
    public class AdmProductPropertiesManager : IAdmProductPropertiesManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Bank AdmProductProperties Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public AdmProductPropertiesManager(ERPEntities currentEntity)
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

        public List<ADM_CONST_GRP_TYPE> GetGeneralPropertiesTypes(ADM_CONST_GRP_TYPE AdmConstGrpType)
        {
            List<ADM_CONST_GRP_TYPE> ADM_CONST_GRP_TYPE_Obj = new List<ADM_CONST_GRP_TYPE>();

            try
            {
                ADM_CONST_GRP_TYPE_Obj = (from clt in this.currentEntity.ADM_CONST_GRP_TYPE
                                          where (clt.CGT_PK == AdmConstGrpType.CGT_PK)
                                               && clt.CGT_ACTIVE == AdmConstGrpType.CGT_ACTIVE
                                          select clt).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);

            }

            return ADM_CONST_GRP_TYPE_Obj;
        }

        public List<ADM_CONST_MST> GetGeneralProperties(ADM_CONST_MST AdmProductionMstObj, ServiceUtility utilityObj)
        {
            List<ADM_CONST_MST> Obj_ADM_CONST_MST_List;
            IQueryable<ADM_CONST_MST> Obj_ADM_CONST_MST_Qry;
            try
            {
                Obj_ADM_CONST_MST_Qry = (from pac in this.currentEntity.ADM_CONST_MST
                                         where pac.CON_GROUP == AdmProductionMstObj.CON_GROUP
                                               && pac.CON_PK == (AdmProductionMstObj.CON_PK > 0 ? AdmProductionMstObj.CON_PK : pac.CON_PK)
                                               && (AdmProductionMstObj.CON_PARENT > 0 ? pac.CON_PARENT ==AdmProductionMstObj.CON_PARENT : true)
                                               && pac.CON_BIZUNIT == (AdmProductionMstObj.CON_BIZUNIT > 0 ? AdmProductionMstObj.CON_BIZUNIT : pac.CON_BIZUNIT)
                                               //&& pac.CON_PARENT == (AdmProductionMstObj.CON_PARENT > 0 ? AdmProductionMstObj.CON_PARENT : pac.CON_PARENT)
                                             && pac.CON_ACTIVE == (AdmProductionMstObj.CON_ACTIVE > 0 ? AdmProductionMstObj.CON_ACTIVE : pac.CON_ACTIVE)
                                               && pac.CON_CODE.StartsWith(AdmProductionMstObj.CON_CODE)
                                               && pac.CON_NAME.StartsWith(AdmProductionMstObj.CON_NAME)
                                         select pac);

                if (utilityObj.FilterBy == DataFieldRes.ConstCode)
                    Obj_ADM_CONST_MST_Qry = Obj_ADM_CONST_MST_Qry.Where(a => a.CON_CODE.Contains(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.ConstName)
                    Obj_ADM_CONST_MST_Qry = Obj_ADM_CONST_MST_Qry.Where(a => a.CON_NAME.Contains(utilityObj.FilterValue));

                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = Obj_ADM_CONST_MST_Qry.Count();

                //Filter Query
                //paymentHdrQuery = FilterEntity(finPaymentVndHdrListObj, paymentHdrQuery, serviceUtilityObj);

                //Apply Paging And Sorting for grid Purpose
                Obj_ADM_CONST_MST_List = Obj_ADM_CONST_MST_Qry.SortRecords<ADM_CONST_MST>(utilityObj).ToList();
            }
            catch (Exception ex)
            {

                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return Obj_ADM_CONST_MST_List;

        }

        public int SaveGeneralProperties(List<ADM_CONST_MST> admProductionMstList)
        {
            int retval = 0;
            int? maxPK;
            int? maxconvalue;
            int convalue = 0;

            ADM_CONST_MST Old_ADM_CONST_MST_Obj;

            try
            {
                foreach (ADM_CONST_MST ADM_CONST_MST_Obj in admProductionMstList)
                {
                    if (ADM_CONST_MST_Obj.CON_PK == 0)   //SAVE
                    {
                        // Gets last pk
                        maxPK = this.currentEntity.ADM_CONST_MST.Max(a => (int?)a.CON_PK);

                        // Sets return value as next pk
                        retval = (maxPK.HasValue ? maxPK.Value + 1 : 1);

                        // Gets last CON_VALUE
                        maxconvalue = this.currentEntity.ADM_CONST_MST.Where(a => a.CON_GROUP == ADM_CONST_MST_Obj.CON_GROUP).Max(a => (int?)a.CON_GROUP);

                        // Sets return value as next con_value
                        convalue = (maxconvalue.HasValue ? maxconvalue.Value + 1 : 1);

                        ADM_CONST_MST_Obj.CON_PK = retval;
                        ADM_CONST_MST_Obj.CON_VALUE = convalue;
                        ADM_CONST_MST_Obj.CON_MOD_DT = DateTime.Now;

                        this.currentEntity.ADM_CONST_MST.AddObject(ADM_CONST_MST_Obj);
                    }
                    else //UPDATION
                    {
                        Old_ADM_CONST_MST_Obj = this.currentEntity.ADM_CONST_MST.SingleOrDefault(a => a.CON_PK == ADM_CONST_MST_Obj.CON_PK && a.CON_MOD_DT == ADM_CONST_MST_Obj.CON_MOD_DT);

                        if (Old_ADM_CONST_MST_Obj != null)
                        {
                            Old_ADM_CONST_MST_Obj.CON_NAME = ADM_CONST_MST_Obj.CON_NAME;
                            Old_ADM_CONST_MST_Obj.CON_CODE = ADM_CONST_MST_Obj.CON_CODE;
                            Old_ADM_CONST_MST_Obj.CON_DATA = ADM_CONST_MST_Obj.CON_DATA;//CBM
                            Old_ADM_CONST_MST_Obj.CON_DESC = ADM_CONST_MST_Obj.CON_DESC;
                            Old_ADM_CONST_MST_Obj.CON_GROUP = ADM_CONST_MST_Obj.CON_GROUP;
                            if (ADM_CONST_MST_Obj.CON_GROUP == 79 || ADM_CONST_MST_Obj.CON_GROUP == 1200)
                            {
                                Old_ADM_CONST_MST_Obj.CON_SPL_COND = ADM_CONST_MST_Obj.CON_SPL_COND;
                            }
                            Old_ADM_CONST_MST_Obj.CON_DEFAULT = ADM_CONST_MST_Obj.CON_DEFAULT;
                            Old_ADM_CONST_MST_Obj.CON_SEQUENCE = ADM_CONST_MST_Obj.CON_SEQUENCE;
                            Old_ADM_CONST_MST_Obj.CON_ACTIVE = ADM_CONST_MST_Obj.CON_ACTIVE;
                            Old_ADM_CONST_MST_Obj.CON_BIZUNIT = ADM_CONST_MST_Obj.CON_BIZUNIT;
                            Old_ADM_CONST_MST_Obj.CON_MOD_BY = ADM_CONST_MST_Obj.CON_MOD_BY;
                            Old_ADM_CONST_MST_Obj.CON_PARENT = ADM_CONST_MST_Obj.CON_PARENT;
                            Old_ADM_CONST_MST_Obj.CON_MOD_DT = DateTime.Now;

                            retval = ADM_CONST_MST_Obj.CON_PK;
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

            throw new NotImplementedException();
        }

        public int DeleteGeneralProperties(List<ADM_CONST_MST> admProductionMstList)
        {
            int retval = 0;
            ADM_CONST_MST Old_ADM_CONST_MST_Obj;

            try
            {
                foreach (ADM_CONST_MST ADM_CONST_MST_Obj in admProductionMstList)
                {
                    Old_ADM_CONST_MST_Obj = currentEntity.ADM_CONST_MST.SingleOrDefault(sah => sah.CON_PK == ADM_CONST_MST_Obj.CON_PK);
                    if (Old_ADM_CONST_MST_Obj != null)
                    {
                        this.currentEntity.ADM_CONST_MST.DeleteObject(Old_ADM_CONST_MST_Obj);
                    }
                    else
                    {
                        // throws exception already deleted or modified by other user
                        throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                    }
                    retval = 1;
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

        public List<ADM_CONST_GRP> GetGeneralPropertiesGroups(ADM_CONST_GRP AdmConstGrpobj, ServiceUtility utilityObj)
        {
            List<ADM_CONST_GRP> Obj_ADM_CONST_GRP_List;
            IQueryable<ADM_CONST_GRP> Obj_ADM_CONST_GRP_Qry;
            try
            {
                Obj_ADM_CONST_GRP_Qry = (from pac in this.currentEntity.ADM_CONST_GRP
                                         where pac.CNG_GRP_TYPE == AdmConstGrpobj.CNG_GRP_TYPE
                                               && pac.CNG_ACTIVE == AdmConstGrpobj.CNG_ACTIVE
                                               && pac.CNG_PK == (AdmConstGrpobj.CNG_PK > 0 ? AdmConstGrpobj.CNG_PK : pac.CNG_PK)
                                         select pac);
                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = Obj_ADM_CONST_GRP_Qry.Count();


                Obj_ADM_CONST_GRP_List = Obj_ADM_CONST_GRP_Qry.SortRecords<ADM_CONST_GRP>(utilityObj).ToList();
            }
            catch (Exception ex)
            {

                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return Obj_ADM_CONST_GRP_List;
        }
        
        //Select the parent group
        public List<ADM_CONST_GRP> GetGeneralPropertiesParentGroup(ADM_CONST_GRP AdmConstGrpobj, ServiceUtility utilityObj)
        {
            List<ADM_CONST_GRP> Obj_ADM_CONST_GRP_List;
            IQueryable<ADM_CONST_GRP> Obj_ADM_CONST_GRP_Qry;
            try
            {
                Obj_ADM_CONST_GRP_Qry = (from pac in this.currentEntity.ADM_CONST_GRP
                                         where pac.CNG_GRP_TYPE == AdmConstGrpobj.CNG_GRP_TYPE
                                               && pac.CNG_ACTIVE == AdmConstGrpobj.CNG_ACTIVE
                                               && pac.CNG_PARENT != null
                                               && pac.CNG_PK == (AdmConstGrpobj.CNG_PK > 0 ? AdmConstGrpobj.CNG_PK : pac.CNG_PK)
                                         select pac);
                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = Obj_ADM_CONST_GRP_Qry.Count();

                Obj_ADM_CONST_GRP_List = Obj_ADM_CONST_GRP_Qry.SortRecords<ADM_CONST_GRP>(utilityObj).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return Obj_ADM_CONST_GRP_List;
        }

        public int SaveGeneralPropertiesGroups(List<ADM_CONST_GRP> AdmConstGroupsList)
        {
            int retval;
            int? max_CNG_PK;
            int? maxcngvalue;
            int cngvalue = 0;
            ADM_CONST_GRP old_ADM_CONST_GRP_Obj;
            try
            {
                // Sets save status zero,save failed
                retval = 0;
                foreach (ADM_CONST_GRP ADM_CONST_GRP_Obj in AdmConstGroupsList)
                {
                    if (ADM_CONST_GRP_Obj.CNG_PK == 0)
                    {
                        max_CNG_PK = this.currentEntity.ADM_CONST_GRP.Max(inv => (int?)inv.CNG_PK);

                        retval = Convert.ToInt16((max_CNG_PK.HasValue ? max_CNG_PK.Value + 1 : 1));

                        // Gets last CNG_VALUE
                        maxcngvalue = this.currentEntity.ADM_CONST_GRP.Where(a => a.CNG_GRP_TYPE == ADM_CONST_GRP_Obj.CNG_GRP_TYPE && a.CNG_ACTIVE ==ADM_CONST_GRP_Obj.CNG_ACTIVE ).Max(a => (int?)a.CNG_VALUE);

                        // Sets return value as next CNG_VALUE
                        cngvalue = (maxcngvalue.HasValue ? maxcngvalue.Value + 1 : 1);

                        ADM_CONST_GRP_Obj.CNG_PK = retval;
                        ADM_CONST_GRP_Obj.CNG_VALUE = cngvalue;

                        ADM_CONST_GRP_Obj.CNG_MOD_DT = DateTime.Now;

                        // Add new AdmConstGrp to the db context
                        this.currentEntity.ADM_CONST_GRP.AddObject(ADM_CONST_GRP_Obj);
                    }
                    else
                    {
                        old_ADM_CONST_GRP_Obj = currentEntity.ADM_CONST_GRP.SingleOrDefault(cgm => cgm.CNG_PK == ADM_CONST_GRP_Obj.CNG_PK);

                        if (old_ADM_CONST_GRP_Obj != null)
                        {
                            // Update AdmConstGrp

                            old_ADM_CONST_GRP_Obj.CNG_CODE = ADM_CONST_GRP_Obj.CNG_CODE;
                            old_ADM_CONST_GRP_Obj.CNG_NAME = ADM_CONST_GRP_Obj.CNG_NAME;
                            old_ADM_CONST_GRP_Obj.CNG_DEFAULT = ADM_CONST_GRP_Obj.CNG_DEFAULT;
                            old_ADM_CONST_GRP_Obj.CNG_GRP_TYPE = ADM_CONST_GRP_Obj.CNG_GRP_TYPE;
                            old_ADM_CONST_GRP_Obj.CNG_SEQUENCE = ADM_CONST_GRP_Obj.CNG_SEQUENCE;
                            old_ADM_CONST_GRP_Obj.CNG_ACTIVE = ADM_CONST_GRP_Obj.CNG_ACTIVE;
                            old_ADM_CONST_GRP_Obj.CNG_BIZUNIT = ADM_CONST_GRP_Obj.CNG_BIZUNIT;
                            old_ADM_CONST_GRP_Obj.CNG_MOD_BY = ADM_CONST_GRP_Obj.CNG_MOD_BY;
                            old_ADM_CONST_GRP_Obj.CNG_MOD_DT = DateTime.Now;
                            // Sets return value as AdmConstGrp pk
                            retval = ADM_CONST_GRP_Obj.CNG_PK;
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

        public int DeleteGeneralPropertiesGroups(List<ADM_CONST_GRP> AdmConstGroupsList)
        {
            //Gets or sets old User details
            ADM_CONST_GRP old_ADM_CONST_GRP_Obj;
            try
            {
                //Iterating through Users list for delete
                foreach (ADM_CONST_GRP ADM_CONST_GRP_Obj in AdmConstGroupsList)
                {
                    //Get User country details with pk and last modified date time used for concurrency checking
                    old_ADM_CONST_GRP_Obj = currentEntity.ADM_CONST_GRP.SingleOrDefault(chi => chi.CNG_PK == ADM_CONST_GRP_Obj.CNG_PK);
                    //If oldRmsuObj is null then,anyone modified or deleted the record
                    if (old_ADM_CONST_GRP_Obj != null)
                    {
                        currentEntity.ADM_CONST_GRP.DeleteObject(old_ADM_CONST_GRP_Obj);
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
