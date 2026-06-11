using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERP.Utilities;
using System.Diagnostics;
using System.Data;

namespace ERPManager.Administration
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AdmCountryMstManager" in both code and config file together.
    public class AdmCountryMstManager : IAdmCountryMstManager
    {

        #region Private Variables
        /// <summary>
        /// Gets or sets currency db context
        /// </summary>
        ERPEntities currentEntity;
        #endregion
        #region Manager Methods

        /// <summary>
        /// Initializes a new instance of country master manager
        /// </summary>
        /// <param name="currentEntity" value="Current db context"></param>
        public AdmCountryMstManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Insert or update list of country master
        /// </summary>
        /// <param name="admCountryMstList" value="List of country master"></param>
        /// <returns>primary key of last inserted or updated country master</returns>
        public int SaveAdmCountryMst(List<AdmCountryMst> admCountryMstList)
        {

            //Gets or sets save status
            short retval;
            //Gets or sets country pk
            int? maxCountryPK;
            //Gets or sets old country masters.Used for updating country masters and for checking concurrency
            AdmCountryMst oldCountryMstObj;
            try
            {
                //Sets save status zero,save failed
                retval = 0;
                //Iterating through country list
                foreach (AdmCountryMst admCountryMstObj in admCountryMstList)
                {
                    //check country detail's pk is zero,insert country details to db context
                    if (admCountryMstObj.cntPK == 0)
                    {
                        //Gets last country pk
                        maxCountryPK = currentEntity.AdmCountryMsts.Max(v => (int?)v.cntPK);
                        //Sets return value as next country pk
                        retval = Convert.ToInt16(((maxCountryPK.HasValue) ? maxCountryPK.Value + 1 : 1));
                        //Sets next country pk
                        admCountryMstObj.cntPK = retval;
                        //Sets country master created date time as current date time
                        admCountryMstObj.cntCrtdDt = DateTime.Now;
                        //Sets country master modified date time as current date time          
                        admCountryMstObj.cntModDt = DateTime.Now;
                        //Add new country to the db context
                        currentEntity.AdmCountryMsts.AddObject(admCountryMstObj);
                    }
                    //updating country details
                    else
                    {
                        //Get current country details using country pk and last modified date time,used for concurrency checking
                        oldCountryMstObj = currentEntity.AdmCountryMsts.SingleOrDefault(cnt => cnt.cntPK == admCountryMstObj.cntPK && cnt.cntModDt == admCountryMstObj.cntModDt);
                        //If oldCountryMst is null then,anyone modified or deleted the record
                        if (oldCountryMstObj != null)
                        {
                            /*
                             //Update country details
                             oldCountryMstObj.cntActive = admCountryMstObj.cntActive;
                             oldCountryMstObj.cntCode = admCountryMstObj.cntCode;
                             oldCountryMstObj.cntCrtdBy = admCountryMstObj.cntCrtdBy;
                             oldCountryMstObj.cntCrtdDt = admCountryMstObj.cntCrtdDt;
                             oldCountryMstObj.cntCurrency = admCountryMstObj.cntCurrency;
                             oldCountryMstObj.cntDesc = admCountryMstObj.cntDesc;
                             oldCountryMstObj.cntModBy = admCountryMstObj.cntModBy;
                             oldCountryMstObj.cntModDt = DateTime.Now;
                             oldCountryMstObj.cntName = admCountryMstObj.cntName;
                             oldCountryMstObj.cntPK = admCountryMstObj.cntPK;
                             * */

                            ManagerUtil.copyEntity<AdmCountryMst>(admCountryMstObj, oldCountryMstObj);
                            oldCountryMstObj.cntModDt = DateTime.Now;

                            //Sets return value as country pk
                            retval = admCountryMstObj.cntPK;
                        }
                        else
                        {
                            //throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }
                }
                //return country pk
                return retval;
            }
            //Handler for concurrency exception
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }


        /// <summary>
        /// Delete list of country masters
        /// </summary>
        /// <param name="admCountryMstList" value="List of country masters"></param>
        /// <returns>1 if successfully deleted</returns>
        public int DeleteAdmCountryMst(List<AdmCountryMst> admCountryMstList)
        {
            //Gets or sets old country details
            AdmCountryMst oldAdmCountryMstObj;
            try
            {
                //Iterating through country list for delete
                foreach (AdmCountryMst admCountryMstObj in admCountryMstList)
                {
                    //Get current country details with pk and last modified date time used for concurrency checking
                    oldAdmCountryMstObj = currentEntity.AdmCountryMsts.SingleOrDefault(cnt => cnt.cntPK == admCountryMstObj.cntPK && cnt.cntModDt == admCountryMstObj.cntModDt);
                    //If oldAdmCountryMstObj is null then,anyone modified or deleted the record
                    if (oldAdmCountryMstObj != null)
                    {
                        //Delete country details from db context
                        currentEntity.AdmCountryMsts.DeleteObject(oldAdmCountryMstObj);
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
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Exception handler for Delete - Delete Concurrency
            catch (ArgumentNullException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exception
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        ///// <summary>
        ///// Gets list of country masters after filtering,sorting
        ///// </summary>
        ///// <param name="admCountryMstObj" value="Country master object"></param>
        ///// <param name="utilityObj" value="Service utility object"></param>
        ///// <returns>List of country master</returns>
        //public List<AdmCountryMst> GetAdmCountryMst(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj)
        //{
        //    //Gets or sets temporary list of countries,filtering,sorting results
        //    List<AdmCountryMst> loadEntityQry;
        //    //Gets or sets searching,filtering definitions
        //    Action<ServiceUtility> action;
        //    //Gets or sets current page index and first record position
        //    int currentPage, firstRec;
        //    try
        //    {
        //            //Set page size one if not given
        //            utilityObj.PageSize = (utilityObj.PageSize == 0) ? 1 : utilityObj.PageSize;
        //            //Set current page index one if not given
        //            currentPage = utilityObj.CurrentPage;
        //            //Set first record position to previous page index * page size
        //            firstRec = (currentPage - 1) * utilityObj.PageSize;
        //            //Filtering country with pk and active status
        //            loadEntityQry = currentEntity.AdmCountryMsts.Include(DataTableRes.CurrencyMaster)
        //                            .Where(cnt=>cnt.cntPK == (admCountryMstObj.cntPK == 0 ? cnt.cntPK : admCountryMstObj.cntPK) 
        //                            || cnt.cntActive == (admCountryMstObj.cntActive == 0 ? cnt.cntActive : admCountryMstObj.cntActive)).ToList();
        //            //Definition for searching,filtering,utility parameter holds search criteria
        //            action = utility =>
        //            {
        //                //Checking filtering criteria is given
        //                if (utility.FilterBy != null || utility.FilterValue != string.Empty)
        //                {
        //                    //Filter country with country name
        //                    if (utility.FilterBy == DataFieldRes.CountryName)
        //                        loadEntityQry = loadEntityQry.Where(cnt=>cnt.cntName.IndexOf(utility.FilterValue, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
        //                    //Filter country with country code
        //                    else if (utility.FilterBy == DataFieldRes.CountryCode)
        //                        loadEntityQry = loadEntityQry.Where(cnt=>cnt.cntCode.IndexOf(utility.FilterValue, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
        //                    //Code block for adding new filter criteria

        //                }
        //                //Checking sorting criteria is given
        //                if (utility.SortBy != null || utility.SortDirection != null)
        //                {
        //                    //Apply Paging And Sorting For country grid Purpose
        //                    if (utility.CurrentPage != -1 && utility.PageSize != -1)
        //                    {
        //                        //Sort with country name and apply paging
        //                        if (utility.SortBy == DataFieldRes.CountryName)
        //                        {
        //                            //Sort countries in asending order with country name;take record from firstRec to page size of records
        //                            if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
        //                                loadEntityQry = loadEntityQry.OrderBy(cnt=>cnt.cntName).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                            //Sort countries in descending order with country name;take record from firstRec to page size of records
        //                            else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
        //                                loadEntityQry = loadEntityQry.OrderByDescending(o=>o.cntName).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                        }
        //                        //Sort with country code and apply paging
        //                        else if (utility.SortBy == DataFieldRes.CountryCode)
        //                        {
        //                            //Sort countries in asending order with country code;take record from firstRec to page size of records
        //                            if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
        //                                loadEntityQry = loadEntityQry.OrderBy(cnt=>cnt.cntCode).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                            //Sort countries in descending order with country code;take record from firstRec to page size of records
        //                            else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
        //                                loadEntityQry = loadEntityQry.OrderByDescending(cnt=>cnt.cntCode).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                        }
        //                        //Sort with country active and apply paging
        //                        else if (utility.SortBy == DataFieldRes.CountryActive)
        //                        {
        //                            //Sort countries in asending order with country active;take record from firstRec to page size of records
        //                            if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
        //                                loadEntityQry = loadEntityQry.OrderBy(cnt=>cnt.cntActive).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                            //Sort countries in descending order with country active;take record from firstRec to page size of records
        //                            else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
        //                                loadEntityQry = loadEntityQry.OrderByDescending(cnt=>cnt.cntActive).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                        }
        //                        //Sort with country pk and apply paging
        //                        else if (utility.SortBy == DataFieldRes.CountryPK)
        //                        {
        //                            //Sort countries in asending order with country pk;take record from firstRec to page size of records
        //                            if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
        //                                loadEntityQry = loadEntityQry.OrderBy(cnt=>cnt.cntPK).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                            //Sort countries in descending order with country pk;take record from firstRec to page size of records
        //                            else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
        //                                loadEntityQry = loadEntityQry.OrderByDescending(cnt=>cnt.cntPK).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                        }
        //                        //Sort with country Currency Name and apply paging
        //                        else if (utility.SortBy == DataFieldRes.CurrencyName)
        //                        {
        //                            //Sort countries in asending order with currency Name;take record from firstRec to page size of records
        //                            if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
        //                                loadEntityQry = loadEntityQry.OrderBy(cnt => cnt.AdmCurrencyMst == null ? string.Empty :cnt.AdmCurrencyMst.curName).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                            //Sort countries in descending order with Curency Name;take record from firstRec to page size of records
        //                            else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
        //                                loadEntityQry = loadEntityQry.OrderByDescending(cnt => cnt.AdmCurrencyMst == null ? string.Empty : cnt.AdmCurrencyMst.curName).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                        }
        //                        //Sort with country Country Desc and apply paging
        //                        else if (utility.SortBy == DataFieldRes.CountryDesc)
        //                        {
        //                            //Sort countries in asending order with currency Name;take record from firstRec to page size of records
        //                            if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
        //                                loadEntityQry = loadEntityQry.OrderBy(cnt => cnt.cntDesc).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                            //Sort countries in descending order with Curency Name;take record from firstRec to page size of records
        //                            else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
        //                                loadEntityQry = loadEntityQry.OrderByDescending(cnt => cnt.cntDesc).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                        }


        //                        //Code block for adding new sorting criteria with paging
        //                    }
        //                    //Current Page is -1 Used to List the all countries with sorting and with out paging
        //                    else
        //                    {
        //                        //Sort with country name
        //                        if (utility.SortBy == DataFieldRes.CountryName)
        //                        {
        //                            //Sort countries in asending order with country name
        //                            if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
        //                                loadEntityQry = loadEntityQry.OrderBy(cnt=>cnt.cntName).ToList();
        //                            //Sort countries in descending order with country name
        //                            else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
        //                                loadEntityQry = loadEntityQry.OrderByDescending(cnt=>cnt.cntName).ToList();
        //                        }
        //                        //Sort with country code
        //                        else if (utility.SortBy == DataFieldRes.CountryCode)
        //                        {
        //                            //Sort countries in asending order with country code
        //                            if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
        //                                loadEntityQry = loadEntityQry.OrderBy(cnt=>cnt.cntCode).ToList();
        //                            //Sort countries in descending order with country code
        //                            else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
        //                                loadEntityQry = loadEntityQry.OrderByDescending(cnt=>cnt.cntCode).ToList();
        //                        }
        //                        //Sort with country active
        //                        else if (utility.SortBy == DataFieldRes.CountryActive)
        //                        {
        //                            //Sort countries in asending order with country active
        //                            if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
        //                                loadEntityQry = loadEntityQry.OrderBy(cnt=>cnt.cntActive).ToList();
        //                            //Sort countries in descending order with country active
        //                            else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
        //                                loadEntityQry = loadEntityQry.OrderByDescending(cnt=>cnt.cntActive).ToList();
        //                        }
        //                        //Sort with country pk
        //                        else if (utility.SortBy == DataFieldRes.CountryPK)
        //                        {
        //                            //Sort countries in asending order with country pk
        //                            if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
        //                                loadEntityQry = loadEntityQry.OrderBy(cnt=>cnt.cntPK).ToList();
        //                            //Sort countries in descending order with country pk
        //                            else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
        //                                loadEntityQry = loadEntityQry.OrderByDescending(cnt=>cnt.cntPK).ToList();
        //                        }
        //                        //Code block for adding new sorting criteria with out paging
        //                    }
        //                }
        //                //No sort criteria is given;default sort;
        //                else
        //                {
        //                    //Apply Paging And Sorting For country grid Purpose
        //                    if (utilityObj.PageSize != -1 && utilityObj.CurrentPage != -1)
        //                    {
        //                        loadEntityQry = loadEntityQry.OrderBy(cnt=>cnt.cntPK).Skip(firstRec).Take(utilityObj.PageSize).ToList();
        //                    }
        //                    //Current Page is -1 Used to List the all countries with sorting
        //                    else
        //                    {
        //                        loadEntityQry = loadEntityQry.OrderBy(cnt=>cnt.cntPK).ToList();
        //                    }
        //                }
        //            };
        //            //Triggering searching,filtering operation
        //            action(utilityObj);

        //        //return country details
        //        return loadEntityQry;

        //    }
        //    //Handler for unknown exceptions
        //    catch (Exception ex)
        //    {
        //        //Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //       throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name,ex);
        //    }
        //    finally
        //    {
        //        //Disposing used objects
        //        loadEntityQry = null;
        //        action = null;
        //    }
        //}

        /// <summary>
        /// Gets list of country masters after filtering,sorting
        /// </summary>
        /// <param name="admCountryMstObj" value="Country master object"></param>
        /// <param name="utilityObj" value="Service utility object"></param>
        /// <returns>List of country master</returns>
        public List<AdmCountryMst> GetAdmCountryMst(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj)
        {
            //Gets or sets temporary list of countries,filtering,sorting results
            List<AdmCountryMst> loadEntityQry;
            //Gets or sets searching,filtering definitions
            Action<ServiceUtility> action;
            //Gets or sets current page index and first record position
            int currentPage, firstRec;
            try
            {
                //Set page size one if not given
                utilityObj.PageSize = (utilityObj.PageSize == 0) ? 1 : utilityObj.PageSize;
                //Set current page index one if not given
                currentPage = utilityObj.CurrentPage;
                //Set first record position to previous page index * page size
                firstRec = (currentPage - 1) * utilityObj.PageSize;
                //Filtering country with pk and active status

                loadEntityQry = currentEntity.AdmCountryMsts.Include(DataTableRes.CurrencyMaster)
                                .Where(cnt =>
                                    (cnt.cntPK == (admCountryMstObj.cntPK == 0 ? cnt.cntPK : admCountryMstObj.cntPK))
                                    && (cnt.cntName.Contains((utilityObj.FilterBy == DataFieldRes.CountryName && !string.IsNullOrEmpty(utilityObj.FilterValue) ? (utilityObj.FilterValue) : cnt.cntName)))
                                    && (cnt.cntCode.Contains((utilityObj.FilterBy == DataFieldRes.CountryCode && !string.IsNullOrEmpty(utilityObj.FilterValue) ? (utilityObj.FilterValue) : cnt.cntCode)))

                                || cnt.cntActive == (admCountryMstObj.cntActive == 0 ? cnt.cntActive : admCountryMstObj.cntActive)).AsQueryable().SortRecords<AdmCountryMst>(utilityObj).ToList();



                //loadEntityQry = EntityFilter<AdmCountryMst>.AsQueryable()
                //                .Where(cnt =>
                //                    (cnt.cntPK == (admCountryMstObj.cntPK == 0 ? cnt.cntPK : admCountryMstObj.cntPK))
                //                    && (cnt.cntName.Contains( (utilityObj.FilterBy == DataFieldRes.CountryName && !string.IsNullOrEmpty(utilityObj.FilterValue) ? (utilityObj.FilterValue) : cnt.cntName))
                //                    && (cnt.cntCode.Contains( (utilityObj.FilterBy == DataFieldRes.CountryCode && !string.IsNullOrEmpty(utilityObj.FilterValue) ? (utilityObj.FilterValue) : cnt.cntCode))))
                //                    || (cnt.cntActive == (admCountryMstObj.cntActive == 0 ? cnt.cntActive : admCountryMstObj.cntActive))).Filter(currentEntity.AdmCountryMsts.Include(DataTableRes.CurrencyMaster)).AsQueryable().SortRecords<AdmCountryMst>(utilityObj).ToList();



                //return country details
                return loadEntityQry;

            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                //Disposing used objects
                loadEntityQry = null;
                action = null;
            }
        }

        ///// <summary>
        ///// Gets count of country masters
        ///// </summary>
        ///// <param name="admCountryMstObj" value="Country master object"></param>
        ///// <param name="utilityObj" value="Service utility object"></param>
        ///// <returns>Count of country masters</returns>
        //public ServiceUtility GetAdmCountryMstCount(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj)
        //{
        //    //Gets or sets list of countries,search result
        //    List<AdmCountryMst> loadEntityQry;
        //    //Gets or sets searching,filtering definitions
        //    Action<ServiceUtility> action;
        //    try
        //    {
        //            //Filtering country with pk and active status
        //            loadEntityQry = currentEntity.AdmCountryMsts.Include(DataTableRes.CurrencyMaster)
        //                            .Where(cnt=>cnt.cntPK == (admCountryMstObj.cntPK == 0 ? cnt.cntPK : admCountryMstObj.cntPK) 
        //                            || cnt.cntActive == (admCountryMstObj.cntActive == 0 ? cnt.cntActive : admCountryMstObj.cntActive)
        //                            ).ToList();
        //            //Definition for filtering,utility parameter holds search criteria
        //            action = utility =>
        //            {
        //                //checking for filter options are given
        //                if (utility.FilterBy != null || utility.FilterValue != string.Empty)
        //                {
        //                    //Filter with country name
        //                    if (utility.FilterBy == DataFieldRes.CountryName)
        //                        loadEntityQry = loadEntityQry.Where(cnt=>cnt.cntName.IndexOf(utility.FilterValue, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
        //                    //Filter with country code
        //                    else if (utility.FilterBy == DataFieldRes.CountryCode)
        //                        loadEntityQry = loadEntityQry.Where(cnt=>cnt.cntCode.IndexOf(utility.FilterValue, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();

        //                }
        //            };
        //            //Triggering filtering, operation
        //            action(utilityObj);

        //            //Sets total countries count to service utility
        //            utilityObj.TotalRecords = loadEntityQry.Count;

        //        //Returns service utility object with total country count
        //        return utilityObj;
        //    }
        //    //Handler for unknown exception
        //    catch (Exception ex)
        //    {
        //        //Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name,ex);
        //    }
        //    finally
        //    {
        //        //Disposing used objects
        //        loadEntityQry = null;
        //        action = null;
        //    }
        //}



        /// <summary>
        /// Gets count of country masters
        /// </summary>
        /// <param name="admCountryMstObj" value="Country master object"></param>
        /// <param name="utilityObj" value="Service utility object"></param>
        /// <returns>Count of country masters</returns>
        public ServiceUtility GetAdmCountryMstCount(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj)
        {

            try
            {
                utilityObj.TotalRecords = currentEntity.AdmCountryMsts.Include(DataTableRes.CurrencyMaster)
                                 .Where(cnt =>
                                     (cnt.cntPK == (admCountryMstObj.cntPK == 0 ? cnt.cntPK : admCountryMstObj.cntPK))
                                     && (cnt.cntName.Contains((utilityObj.FilterBy == DataFieldRes.CountryName && !string.IsNullOrEmpty(utilityObj.FilterValue) ? (utilityObj.FilterValue) : cnt.cntName)))
                                     && (cnt.cntCode.Contains((utilityObj.FilterBy == DataFieldRes.CountryCode && !string.IsNullOrEmpty(utilityObj.FilterValue) ? (utilityObj.FilterValue) : cnt.cntCode)))

                                 || cnt.cntActive == (admCountryMstObj.cntActive == 0 ? cnt.cntActive : admCountryMstObj.cntActive)).Count();


                //Returns service utility object with total country count
                return utilityObj;
            }
            //Handler for unknown exception
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Gets new instance of country masters with default values.All country object passing to server must be initilied with this method.
        /// Entity framework does not support serilizing non nullable fields with null values.This method returns county object with default values in non nullable fields
        /// </summary>
        /// <returns>Country master object</returns>
        public AdmCountryMst GetInitilizedAdmCountryMst()
        {
            try
            {
                //Initilizes new country and assign default values
                AdmCountryMst admCountryMstObj = new AdmCountryMst();
                admCountryMstObj.cntPK = -1;
                admCountryMstObj.cntActive = 255;
                admCountryMstObj.cntCode = String.Empty;
                admCountryMstObj.cntCrtdBy = -1;
                admCountryMstObj.cntCrtdDt = DateTime.MinValue;
                admCountryMstObj.cntName = String.Empty;
                admCountryMstObj.cntModBy = -1;
                admCountryMstObj.cntModDt = DateTime.MinValue;
                admCountryMstObj.cntCurrency = -1;

                //Return Initilized country to client
                return admCountryMstObj;
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Gets list of Country after filtering,sorting for filling auto complete list
        /// </summary>
        /// <param name="admCountryMstsObj">Currency object</param>
        /// <param name="utilityObj">Service utility object</param>
        /// <returns>List of Country</returns>
        public List<AdmCountryMst> GetAdmCountryMstAutoCompleteList(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj)
        {
            // Get or set list of AdmCountryMsts for return to client
            List<AdmCountryMst> admCountryMstList = null;
            AdmCountryMst TempAdmCountryMstObj = null;
            // Get or set page size;auto complete limit
            int pageSize;
            try
            {
                // Get page size from service utility object
                pageSize = Convert.ToInt32(utilityObj.PageSize);

                // Checking filter required with code
                if (utilityObj.FilterBy == DataFieldRes.CountryCode)
                {
                    // Take sorted list of all active AdmCountryMsts with code starts with filter value
                    admCountryMstList = currentEntity.AdmCountryMsts
                                        .Where(cnt => cnt.cntActive == 1 && cnt.cntCode.StartsWith(utilityObj.FilterValue)).OrderBy(cnt => cnt.cntCode).ToList();
                }
                // Checking filter required with name
                else if (utilityObj.FilterBy == DataFieldRes.CountryName)
                {
                    // Take sorted list of all active AdmCountryMsts with name starts with filter value
                    admCountryMstList = currentEntity.AdmCountryMsts
                                        .Where(cnt => cnt.cntActive == 1 && cnt.cntName.StartsWith(utilityObj.FilterValue)).OrderBy(cnt => cnt.cntName).ToList();
                }
                // Default filter
                else
                {
                    // Take sorted list of all active AdmCountryMsts
                    admCountryMstList = currentEntity.AdmCountryMsts.Where(cnt => cnt.cntActive == 1).OrderBy(cnt => cnt.cntName).ToList();
                }

                // Checking list of AdmCurrencyMst exceeds the limit
                if (admCountryMstList.Count > pageSize)
                {
                    // Take pasgeSize of records from list of AdmCurrencyMst
                    admCountryMstList = admCountryMstList.Take(pageSize).ToList();
                    // Initilizing new instance of AdmCurrencyMst class for adding ... as last item
                    TempAdmCountryMstObj = new AdmCountryMst();
                    // Make AdmCurrencyMst pk as zero
                    TempAdmCountryMstObj.cntPK = 0;
                    // Make AdmCurrencyMst code as ...
                    TempAdmCountryMstObj.cntCode = ERPManagerRes.MoreAutoComplete;
                    // Make AdmCurrencyMst name as ...
                    TempAdmCountryMstObj.cntName = ERPManagerRes.MoreAutoComplete;
                    // Add temporary AdmCurrencyMst to list of AdmCurrencyMst;And return to client
                    admCountryMstList.Add(TempAdmCountryMstObj);

                    // Return list of AdmCurrencyMst
                    return admCountryMstList;
                }
                else
                {
                    // Return list of AdmCurrencyMst
                    return admCountryMstList;
                }

            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                // Disposing used objects
                admCountryMstList = null;
                TempAdmCountryMstObj = null;
            }
        }
        #endregion
    }
}
