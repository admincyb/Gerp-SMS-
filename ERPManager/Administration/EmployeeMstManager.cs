using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;

namespace ERPManager
{
    public class EmployeeMstManager:IEmployeeMstManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion
                 /// <summary>
        /// Activity Master Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public EmployeeMstManager(ERPEntities currentEntity)
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

        public List<EmpEmployeeMst> GetEmployeeMst(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj)
        {
            IQueryable<EmpEmployeeMst> qry;
            System.Data.Objects.ObjectSet<EmpEmployeeMst> loadQuery;
            string loadName;
            try
            {
                loadQuery = this.currentEntity.EmpEmployeeMsts;
                //var qry = this.currentEntity.WkfUserMsts.Include(DataTableRes.EmployeeMaster).Include(DataTableRes.XacAuthenticationMode).AsQueryable();
                //For Loading Entities
                if (utilityObj.LoadEntities != null && utilityObj.LoadEntities.Count > 0)
                {
                    foreach (LoadEntities load in utilityObj.LoadEntities)
                    {
                        loadName = DataTableRes.ResourceManager.GetObject(load.ToString()).ToString();
                        loadQuery.Include(loadName);
                    }
                }

                //Set page size one if not given
                utilityObj.PageSize = (utilityObj.PageSize == 0) ? 1 : utilityObj.PageSize;
                qry = loadQuery;
                qry = FilterEntity(EmployeeMstObj, qry, utilityObj);

                #region Sorting
                // Apply Paging And Sorting For AD_COUNTRIES_MST grid Purpose
                // Checking sorting criteria is given

                #endregion
                return qry.SortRecords<EmpEmployeeMst>(utilityObj).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                //Disposing objects
            }
        }

        public int DeleteEmployeeMst(List<EmpEmployeeMst> EmployeeMstList)
        {
            //Gets or sets old User details
            EmpEmployeeMst oldEmpEmployeeMstObj;
            List<EmpEmployeeMst> EmpEmployeeMstLista;
            try
            {
                //Iterating through Users list for delete
                foreach (EmpEmployeeMst EmpEmployeeMstObj in EmployeeMstList)
                {
                    //Get User country details with pk and last modified date time used for concurrency checking
                    oldEmpEmployeeMstObj = currentEntity.EmpEmployeeMsts.SingleOrDefault(rms => rms.empPK == EmpEmployeeMstObj.empPK);//&& rms.mod == EmpEmployeeMstObj.empMod_On
                    //If oldRmsuObj is null then,anyone modified or deleted the record
                    if (oldEmpEmployeeMstObj != null)
                    {
                        currentEntity.EmpEmployeeMsts.DeleteObject(oldEmpEmployeeMstObj);
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

        public ServiceUtility GetEmployeeMstCount(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj)
        {
            // Gets or sets current page index and first record position
            IQueryable<EmpEmployeeMst> qry;
            System.Data.Objects.ObjectSet<EmpEmployeeMst> loadQuery;
            string loadName;
            try
            {
                loadQuery = this.currentEntity.EmpEmployeeMsts;
                //For Loading Entities
                if (utilityObj.LoadEntities != null && utilityObj.LoadEntities.Count > 0)
                {
                    foreach (LoadEntities load in utilityObj.LoadEntities)
                    {
                        loadName = DataTableRes.ResourceManager.GetObject(load.ToString()).ToString();
                        loadQuery.Include(loadName);
                    }
                }

                //Set page size one if not given
                utilityObj.PageSize = (utilityObj.PageSize == 0) ? 1 : utilityObj.PageSize;
                qry = loadQuery;
                qry = FilterEntity(EmployeeMstObj, qry, utilityObj);

                #region Sorting
                // Apply Paging And Sorting For AD_COUNTRIES_MST grid Purpose
                // Checking sorting criteria is given

                #endregion
                //return Country master details;
                //if (utilityObj.SortBy != null && utilityObj.SortDirection != null)
                //{
                //    if (utilityObj.SortBy == DataFieldRes.FRM_Name)
                //        utilityObj.SortBy = DataTableRes.FuelRegionMst + "." + DataFieldRes.FRM_Name;
                //    if (utilityObj.SortBy == DataFieldRes.CurrencyName)
                //        utilityObj.SortBy = DataTableRes.CurrenciesMst + "." + DataFieldRes.CurrencyName;
                //    if (utilityObj.SortBy == DataFieldRes.UOMName)
                //        utilityObj.SortBy = DataTableRes.UOMMst + "." + DataFieldRes.UOMName;
                //}
                qry.SortRecords<EmpEmployeeMst>(utilityObj).ToList();
                utilityObj.TotalRecords = qry.Count();

                // Return utilityObj with total pages
                return utilityObj;
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {

            }
        }

        public int SaveEmployeeMst(List<EmpEmployeeMst> EmployeeMstList)
        {
            //Holds save status
            int retval;
            //Holds last Contract Basis Details pk
            long? maxempPK;
            //On update holds old Contract Basis Details details
            EmpEmployeeMst oldFlightMst;

            try
            {
                //Set save status zero,save failed
                retval = 0;
                //Iterate through vedor list for save
                foreach (EmpEmployeeMst adFlightMstObj in EmployeeMstList)
                {

                        //check Contract Basis Details pk is zero,save Contract Basis Details as new record
                        if (adFlightMstObj.empPK == 0)
                        {

                            //Gets last User pk
                            maxempPK = currentEntity.EmpEmployeeMsts.Max(rms => (int?)rms.empPK);
                            //Sets return value as next User pk
                            retval = Convert.ToInt16(((maxempPK.HasValue) ? maxempPK.Value + 1 : 1));
                            //Sets next User pk
                            adFlightMstObj.empPK= (byte)retval;
                            //Sets UserMaster Modified date time as current date time
                            //adFlightMstObj.empMod_On = DateTime.Now;
                            //adFlightMstObj.empCrtd_On = DateTime.Now;
                            //Add new User to the db context
                            currentEntity.EmpEmployeeMsts.AddObject(adFlightMstObj);
                        }
                        //updating Contract Basis Details details
                        else
                        {
                            //Get current Contract Basis Details details using Contract Basis Details pk
                            oldFlightMst = currentEntity.EmpEmployeeMsts.SingleOrDefault(v => v.empPK == adFlightMstObj.empPK);//&& v.empMod_On == adFlightMstObj.empMod_On
                            if (oldFlightMst != null)
                            {


                                //Update Contract Basis Details details
                                oldFlightMst.empActive = adFlightMstObj.empActive;
                                //oldFlightMst.empBizUnit = adFlightMstObj.empBizUnit;
                                oldFlightMst.empCode = adFlightMstObj.empCode;
                                //oldFlightMst.empDesc = adFlightMstObj.empDesc;
                                //oldFlightMst.empFrom_Date = adFlightMstObj.empFrom_Date;
                                oldFlightMst.empName = adFlightMstObj.empName;
                                //oldFlightMst.empCode = adFlightMstObj.empCode;
                                //oldFlightMst.empTo_Date = adFlightMstObj.empTo_Date;

                                //oldFlightMst.empMod_By = adFlightMstObj.empMod_By;
                                //oldFlightMst.empMod_On = adFlightMstObj.empMod_On = DateTime.Now;

                                //Set return value as Contract Basis Details pk
                                retval = adFlightMstObj.empPK;
                            }
                            else
                            {
                                //throws exception already deleted or modified by other user
                                throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                            }
                        }
                }
                //return Contract Basis Details pk
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

        public List<EmpEmployeeMst> GetEmployeeMstAutoCompleteList(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj)
        {
            IQueryable<EmpEmployeeMst> qry;
            List<EmpEmployeeMst> EmpEmployeeMstList;
            EmpEmployeeMst tempEmpEmployeeMstObj = null;
            int pageSize;
            try
            {
                EmpEmployeeMstList = new List<EmpEmployeeMst>();
                pageSize = Convert.ToInt32(utilityObj.PageSize);
                qry = currentEntity.EmpEmployeeMsts;

                if (utilityObj.FilterBy == DataFieldRes.EmployeeName)
                {
                    qry = qry.Where(cch => cch.empActive == 1 && cch.empName.StartsWith(utilityObj.FilterValue))
                                           .OrderBy(cch => cch.empName);
                }
                else
                {
                    qry = qry.Where(cch => cch.empActive == 1)
                                           .OrderBy(cch => cch.empName);
                }
                EmpEmployeeMstList = qry.ToList();

                if (EmpEmployeeMstList.Count > pageSize)
                {
                    //EmpEmployeeMstList = EmpEmployeeMstList.Take(pageSize).ToList();
                    //tempEmpEmployeeMstObj = new EmpEmployeeMst();
                    //tempEmpEmployeeMstObj.empPK = 0;
                    //tempEmpEmployeeMstObj. = gComsManagerRes.MoreAutoComplete;
                    //EmpEmployeeMstList.Add(tempFuelRegionMstObj);

                    return EmpEmployeeMstList;
                }
                else
                {
                    return EmpEmployeeMstList;
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
                EmpEmployeeMstList = null;
                tempEmpEmployeeMstObj = null;
            }
        }
        #region private methods

        private static IQueryable<EmpEmployeeMst> FilterEntity(EmpEmployeeMst EmpEmployeeMstObj, IQueryable<EmpEmployeeMst> qry, ServiceUtility utilityObj)
        {
            #region Filtering
            if (EmpEmployeeMstObj.empPK != 0)
                qry = qry.Where(cnt => cnt.empPK == EmpEmployeeMstObj.empPK);
            //if (EmpEmployeeMstObj.empBizUnit != -1)
            //{
            //    qry = qry.Where(cnt => cnt.empBizUnit == EmpEmployeeMstObj.empBizUnit);
            //}
            if (EmpEmployeeMstObj.empActive < 1 && EmpEmployeeMstObj.empActive == 1) //select all active records
                qry = qry.Where(cnt => cnt.empActive == 1);
            else if (EmpEmployeeMstObj.empPK > 0 && EmpEmployeeMstObj.empActive == 1) // select all active +(union) having given pk
                qry = qry.Where(cnt => cnt.empActive == EmpEmployeeMstObj.empActive || cnt.empPK == EmpEmployeeMstObj.empPK);

            //if (adfuelrateMstObj.CAM_ACTIVITY_HDR != null && adfuelrateMstObj.CAM_ACTIVITY_HDR.Count > 0)
            //{
            //    short? location = adfuelrateMstObj.CAM_ACTIVITY_HDR.First().ACH_Airport_Landing;
            //    qry = qry.Where(cnt => cnt.CAM_ACTIVITY_HDR.Where(ahdr => ahdr.ACH_Airport_Landing == location).Count() > 0);
            //}

            if (utilityObj.FilterBy != null || utilityObj.FilterValue != null)
            {
                // Filter AD_COUNTRIES_MST with Name
                if (utilityObj.FilterBy == DataFieldRes.EmployeeName)
                    qry = qry.Where(jbm => jbm.empName.Contains(utilityObj.FilterValue));
                 //Filter AD_COUNTRIES_MST with Code
                if (utilityObj.FilterBy == DataFieldRes.EmployeeCode)
                    qry = qry.Where(jbm => jbm.empCode.Contains(utilityObj.FilterValue));

                //// Filter AD_COUNTRIES_MST with Active FuelTrd_Active		

                //if (utilityObj.FilterBy == DataFieldRes.FuelTrd_Region)
                //    qry = qry.Where(jbm => jbm.FTR_Fuel_Region == byte.Parse(utilityObj.FilterValue));
                //if (utilityObj.FilterBy == DataFieldRes.FuelTrd_Active)
                //    qry = qry.Where(jbm => jbm.FTR_Active == byte.Parse(utilityObj.FilterValue));
            }
            #endregion
            return qry;
        }
        #endregion
    }
}
