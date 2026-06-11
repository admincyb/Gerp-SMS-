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

namespace ERPManager.Employee
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmpEmployeeMstManager" in both code and config file together.
    public class EmpEmployeeMstManager : IEmpEmployeeMstManager
    {
        #region Private Variables
        /// <summary>
        /// Gets or sets current db context
        /// </summary>
        private ERPEntities currentEntity;
        #endregion

        #region Manager Methods

        /// <summary>
        /// Initializes a new instance of theEmpEmployeeMstManagerclass
        /// </summary>
        /// <param name="currentEntity">Current db context</param>
        public EmpEmployeeMstManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Insert or update list of EmpEmployeeMst
        /// </summary>
        /// <param name="empEmployeeMstList">List of EmpEmployeeMst</param>
        /// <returns>primary key of last inserted or updated EmpEmployeeMst</returns>
        public int SaveEmpEmployeeMst(List<EmpEmployeeMst> empEmployeeMstList)
        {
            // Gets or sets save status
            int retval;

            // Gets or sets EmpEmployeeMst pk
            int? maxEmpEmployeeMstPK;

            // Gets or sets old EmpEmployeeMst.Used for updating EmpEmployeeMst and for checking concurrency
            EmpEmployeeMst oldEmpEmployeeMstObj;
            try
            {
                // Sets save status zero,save failed
                retval = 0;

                // Iterating through EmpEmployeeMst list
                foreach (EmpEmployeeMst empEmployeeMstObj in empEmployeeMstList)
                {
                    // check EmpEmployeeMst pk is zero,insert EmpEmployeeMst to db context
                    if (empEmployeeMstObj.empPK == 0)
                    {
                        // Gets last EmpEmployeeMst pk
                        maxEmpEmployeeMstPK = this.currentEntity.EmpEmployeeMsts.Max(v => (int?)v.empPK);

                        // Sets return value as next EmpEmployeeMst pk
                        retval = Convert.ToInt16((maxEmpEmployeeMstPK.HasValue ? maxEmpEmployeeMstPK.Value + 1 : 1));

                        // Sets next EmpEmployeeMst pk
                        empEmployeeMstObj.empPK = retval;

                        // Sets EmpEmployeeMst created date time as current date time
                        empEmployeeMstObj.empCrtdOn = DateTime.Now;

                        // Sets EmpEmployeeMst modified date time as current date time
                        empEmployeeMstObj.empModOn = DateTime.Now;

                        // Add new EmpEmployeeMst to the db context
                        this.currentEntity.EmpEmployeeMsts.AddObject(empEmployeeMstObj);
                    }
                    else
                    {
                        // updating EmpEmployeeMst
                        // Get current EmpEmployeeMst using EmpEmployeeMst pk and last modified date time,used for concurrency checking
                        oldEmpEmployeeMstObj = currentEntity.EmpEmployeeMsts.SingleOrDefault(v => v.empPK == empEmployeeMstObj.empPK);

                        // If oldEmpEmployeeMstMst is null then,anyone modified or deleted the record
                        if (oldEmpEmployeeMstObj != null&& oldEmpEmployeeMstObj.empModOn== empEmployeeMstObj.empModOn)
                        {
                            // Update EmpEmployeeMst
                            oldEmpEmployeeMstObj.empCode = empEmployeeMstObj.empCode;
                            oldEmpEmployeeMstObj.empName = empEmployeeMstObj.empName;
                            oldEmpEmployeeMstObj.empType = empEmployeeMstObj.empType;
                            oldEmpEmployeeMstObj.EmpAddress1 = empEmployeeMstObj.EmpAddress1;
                            oldEmpEmployeeMstObj.EmpCity1 = empEmployeeMstObj.EmpCity1;
                            oldEmpEmployeeMstObj.EmpCountry1 = empEmployeeMstObj.EmpCountry1;
                            oldEmpEmployeeMstObj.empDOB = empEmployeeMstObj.empDOB;
                            oldEmpEmployeeMstObj.empDOJ = empEmployeeMstObj.empDOJ;
                            oldEmpEmployeeMstObj.empDayUtiliznMin = empEmployeeMstObj.empDayUtiliznMin;
                            oldEmpEmployeeMstObj.empDayUtiliznMax = empEmployeeMstObj.empDayUtiliznMax;
                            oldEmpEmployeeMstObj.empActive = empEmployeeMstObj.empActive;
                            oldEmpEmployeeMstObj.empBizUnit = empEmployeeMstObj.empBizUnit;
                            oldEmpEmployeeMstObj.empDesignation = empEmployeeMstObj.empDesignation;
                            oldEmpEmployeeMstObj.empDept = empEmployeeMstObj.empDept;
                            oldEmpEmployeeMstObj.empTFld1 = empEmployeeMstObj.empTFld1;
                            oldEmpEmployeeMstObj.empTFld2 = empEmployeeMstObj.empTFld2;
                            oldEmpEmployeeMstObj.empNFld1 = empEmployeeMstObj.empNFld1;
                            oldEmpEmployeeMstObj.empNFld2 = empEmployeeMstObj.empNFld2;
                            oldEmpEmployeeMstObj.empDFld1 = empEmployeeMstObj.empDFld1;
                            oldEmpEmployeeMstObj.empCrtdBy = empEmployeeMstObj.empCrtdBy;
                            oldEmpEmployeeMstObj.empCrtdOn = empEmployeeMstObj.empCrtdOn;
                            oldEmpEmployeeMstObj.empModBy = empEmployeeMstObj.empModBy;
                            oldEmpEmployeeMstObj.empModOn = DateTime.Now;

                            // Sets return value as EmpEmployeeMst pk
                            retval = empEmployeeMstObj.empPK;
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }
                }

                // return EmpEmployeeMst pk
                return retval;
            }
            catch (OptimisticConcurrencyException ex)
            {
                // Handler for concurrency exception
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Delete list of EmpEmployeeMst
        /// </summary>
        /// <param name="empEmployeeMstList">List of EmpEmployeeMst</param>
        /// <returns>1 if successfully deleted</returns>
        public int DeleteEmpEmployeeMst(List<EmpEmployeeMst> empEmployeeMstList)
        {
            // Gets or sets old EmpEmployeeMst
            EmpEmployeeMst oldEmpEmployeeMstObj;
            try
            {
                // Iterating through EmpEmployeeMst for delete
                foreach (EmpEmployeeMst empEmployeeMstObj in empEmployeeMstList)
                {
                    // Get current EmpEmployeeMst with pk and last modified date time used for concurrency checking
                    oldEmpEmployeeMstObj = currentEntity.EmpEmployeeMsts.SingleOrDefault(v => v.empPK == empEmployeeMstObj.empPK && v.empModOn == empEmployeeMstObj.empModOn);

                    // If oldEmpEmployeeMstObj is null then,anyone modified or deleted the record
                    if (oldEmpEmployeeMstObj != null)
                    {
                        // Delete EmpEmployeeMst details from db context
                        this.currentEntity.EmpEmployeeMsts.DeleteObject(oldEmpEmployeeMstObj);
                    }
                    else
                    {
                        // Throws already modified or deleted by other user exception
                        throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                    }
                }

                // Delete successful
                return 1;
            }
            catch (OptimisticConcurrencyException ex)
            {
                // Handler for concurrency exception
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (ArgumentNullException ex)
            {
                // Exception handler for Delete - Delete Concurrency
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                // Exception handler for Delete - Delete Concurrency
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Gets list of EmpEmployeeMst after filtering,sorting
        /// </summary>
        /// <param name="empEmployeeMstObj">EmpEmployeeMst object</param>
        /// <param name="utilityObj">Service utility object</param>
        /// <returns>List of EmpEmployeeMst</returns>
        public List<EmpEmployeeMst> GetEmpEmployeeMst(EmpEmployeeMst empEmployeeMstObj, ServiceUtility utilityObj)
        {
            // Gets or sets temporary list of EmpEmployeeMst,filtering,sorting results
            List<EmpEmployeeMst> loadEntityQry;



            // Gets or sets searching,filtering definitions
            Action<ServiceUtility> action;

            // Gets or sets current page index and first record position
            int currentPage, firstRec;
            try
            {



                // Set page size one if not given
                utilityObj.PageSize = (utilityObj.PageSize == 0) ? 1 : utilityObj.PageSize;

                // Set current page index one if not given
                currentPage = utilityObj.CurrentPage;

                // Set first record position to previous page index * page size
                firstRec = (currentPage - 1) * utilityObj.PageSize;

                // Fileter EmpEmployeeMst with pk,active status
                //loadEntityQry = this.currentEntity.EmpEmployeeMsts.Include(DataTableRes.CountryMaster).Include(DataTableRes.EmployeeDesignation )
                //    .Include(DataTableRes.EmployeeType)
                //                .Where(emp => emp.empBizUnit == ((empEmployeeMstObj.empBizUnit == -1) ? emp.empBizUnit : empEmployeeMstObj.empBizUnit)
                //                && emp.empPK == (empEmployeeMstObj.empPK == 0 ? emp.empPK : empEmployeeMstObj.empPK)
                //                || emp.empActive == (empEmployeeMstObj.empActive == 0 ? emp.empActive : empEmployeeMstObj.empActive))
                //                .Where(emp => emp.empType == (empEmployeeMstObj.empType > 0 ? empEmployeeMstObj.empType : emp.empType)).ToList();

                loadEntityQry = this.currentEntity.EmpEmployeeMsts.Where(emp => emp.empBizUnit == ((empEmployeeMstObj.empBizUnit == -1) ? emp.empBizUnit : empEmployeeMstObj.empBizUnit)
                                && emp.empPK == (empEmployeeMstObj.empPK == 0 ? emp.empPK : empEmployeeMstObj.empPK)
                                || emp.empActive == (empEmployeeMstObj.empActive == 0 ? emp.empActive : empEmployeeMstObj.empActive))
                                .Where(emp => emp.empType == (empEmployeeMstObj.empType > 0 ? empEmployeeMstObj.empType : emp.empType)).ToList();


                //if (empEmployeeMstObj.EmpEmployeeRoleMpgs != null && empEmployeeMstObj.EmpEmployeeRoleMpgs.Count > 0)
                //{
                //    loadEntityQry = loadEntityQry.Where(emp => emp.EmpEmployeeRoleMpgs.Where(emr => emr.emrRole == empEmployeeMstObj.EmpEmployeeRoleMpgs.First().emrRole).Count() > 0).ToList();
                //}



                // Definition for searching,filtering,utility parameter holds search criteria
                action = utility =>
                {
                    // Checking filtering criteria is given
                    if (utility.FilterBy != null || utility.FilterValue != string.Empty)
                    {
                        // Filter EmpEmployeeMst with EmpEmployeeMst name
                        if (utility.FilterBy == DataFieldRes.EmployeeName)
                        {
                            loadEntityQry = loadEntityQry.Where(o => o.empName.IndexOf(utility.FilterValue, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
                        }

                        // Filter EmpEmployeeMst with EmpEmployeeMst code
                        else if (utility.FilterBy == DataFieldRes.EmployeeCode)
                        {
                            loadEntityQry = loadEntityQry.Where(o => o.empCode.IndexOf(utility.FilterValue, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
                        }

                         // Filter EmpEmployeeMst with EmpEmployeeMsttype
                        else if (utility.FilterBy == DataFieldRes.EmpTypeData)
                        {
                            loadEntityQry = loadEntityQry.Where(o => o.XacEmpType.xetData.StartsWith(utility.FilterValue, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        }

                        // Code block for adding new filter criteria
                    }

                    // Checking sorting criteria is given
                    if (utility.SortBy != null || utility.SortDirection != null)
                    {
                        // Apply Paging And Sorting For EmpEmployeeMst grid Purpose
                        if (utility.CurrentPage != -1 && utility.PageSize != -1)
                        {
                            // Sort with EmpEmployeeMst name and apply paging
                            if (utility.SortBy == DataFieldRes.EmployeeName)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst name;take record from firstRec to page size of records
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empName).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst name;take record from firstRec to page size of records
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empName).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }
                            }

                            // Sort with EmpEmployeeMst code and apply paging
                            else if (utility.SortBy == DataFieldRes.EmployeeCode)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst code;take record from firstRec to page size of records
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empCode).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst code;take record from firstRec to page size of records
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empCode).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }
                            }

                            // Sort with EmpEmployeeMst active and apply paging
                            else if (utility.SortBy == DataFieldRes.EmployeeStatus)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst active;take record from firstRec to page size of records
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empActive).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst active;take record from firstRec to page size of records
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empActive).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }
                            }

                            // Sort with EmpEmployeeMst pk and apply paging
                            else if (utility.SortBy == DataFieldRes.EmployeePK)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst pk;take record from firstRec to page size of records
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empPK).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst pk;take record from firstRec to page size of records
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empPK).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }
                            }
                            // Sort with EmpEmployeeMst Designation Name and apply paging
                            else if (utility.SortBy == DataFieldRes.EmployeeDesignationName)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst Designation Name;take record from firstRec to page size of records
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    //loadEntityQry = loadEntityQry.OrderBy(o => o.EmpDesignationMst.dsgName).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst Designation Name;take record from firstRec to page size of records
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    //loadEntityQry = loadEntityQry.OrderByDescending(o => o.EmpDesignationMst.dsgName).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }
                            }
                            // Sort with EmpEmployeeMst DOB and apply paging
                            else if (utility.SortBy == DataFieldRes.EmployeeDOB)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst DOB;take record from firstRec to page size of records
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empDOB).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst DOB;take record from firstRec to page size of records
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empDOB).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }
                            }
                            // Sort with EmpEmployeeMst DOB and apply paging
                            else if (utility.SortBy == DataFieldRes.EmployeeDOB)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst DOB;take record from firstRec to page size of records
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empDOB).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst DOB;take record from firstRec to page size of records
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empDOB).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }
                            }
                            else if (utility.SortBy == DataFieldRes.EmpTypeData)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpType Data;take record from firstRec to page size of records
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.XacEmpType.xetData).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpType Data;take record from firstRec to page size of records
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.XacEmpType.xetData).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }
                            }
                            // Sort with EmpEmployeeMst DOJ and apply paging
                            else if (utility.SortBy == DataFieldRes.EmployeeDOJ)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst DOJ;take record from firstRec to page size of records
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empDOJ).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst DOJ;take record from firstRec to page size of records
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empDOJ).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }
                            }
                            // Sort with EmpEmployeeMst City and apply paging
                            else if (utility.SortBy == DataFieldRes.EmployeeCity)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst City;take record from firstRec to page size of records
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.EmpCity1).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst City;take record from firstRec to page size of records
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.EmpCity1).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }
                            }
                            // Sort with EmpEmployeeMst Country and apply paging
                            else if (utility.SortBy == DataFieldRes.CountryName)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst Country;take record from firstRec to page size of records
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    //loadEntityQry = loadEntityQry.OrderBy(o => o.AdmCountryMst.cntName).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst Country;take record from firstRec to page size of records
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    //loadEntityQry = loadEntityQry.OrderByDescending(o => o.AdmCountryMst.cntName).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                                }
                            }

                            // Code block for adding new sort criteria
                        }

                        // Current Page is -1 Used to List the all EmpEmployeeMst with sorting and with out paging
                        else
                        {
                            // Sort with EmpEmployeeMst name
                            if (utility.SortBy == DataFieldRes.EmployeeName)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst name
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empName).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst name
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empName).ToList();
                                }
                            }

                            // Sort with EmpEmployeeMst code
                            else if (utility.SortBy == DataFieldRes.EmployeeCode)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst code
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empCode).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst code
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empCode).ToList();
                                }
                            }

                            // Sort with EmpEmployeeMst active
                            else if (utility.SortBy == DataFieldRes.EmployeeStatus)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst active
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empActive).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst active
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empActive).ToList();
                                }
                            }

                            // Sort with EmpEmployeeMst pk
                            else if (utility.SortBy == DataFieldRes.EmployeePK)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst pk
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empPK).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst pk
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empPK).ToList();
                                }
                            }
                            // Sort with EmpEmployeeMst Designation Name
                            else if (utility.SortBy == DataFieldRes.EmployeeDesignationName)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst Designation Name
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    //loadEntityQry = loadEntityQry.OrderBy(o => o.EmpDesignationMst.dsgName).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst Designation Name
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    //loadEntityQry = loadEntityQry.OrderByDescending(o => o.EmpDesignationMst.dsgName).ToList();
                                }
                            }
                            // Sort with EmpEmployeeMst DOB Name
                            else if (utility.SortBy == DataFieldRes.EmployeeDOB)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst DOB
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empDOB).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst DOB
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empDOB).ToList();
                                }
                            }
                            // Sort with EmpEmployeeMst DOJ Name
                            else if (utility.SortBy == DataFieldRes.EmployeeDOJ)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst DOJ
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.empDOJ).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst DOJ
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.empDOJ).ToList();
                                }
                            }
                            // Sort with EmpEmployeeMst City Name
                            else if (utility.SortBy == DataFieldRes.EmployeeCity)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst City
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    loadEntityQry = loadEntityQry.OrderBy(o => o.EmpCity1).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst City
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    loadEntityQry = loadEntityQry.OrderByDescending(o => o.EmpCity1).ToList();
                                }
                            }
                            // Sort with EmpEmployeeMst Country Name
                            else if (utility.SortBy == DataFieldRes.CountryName)
                            {
                                // Sort EmpEmployeeMst in asending order with EmpEmployeeMst Country
                                if (utility.SortDirection.ToLower() == ERPManagerRes.ascending)
                                {
                                    //loadEntityQry = loadEntityQry.OrderBy(o => o.AdmCountryMst.cntName).ToList();
                                }

                                // Sort EmpEmployeeMst in descending order with EmpEmployeeMst Country
                                else if (utility.SortDirection.ToLower() == ERPManagerRes.descending)
                                {
                                    //loadEntityQry = loadEntityQry.OrderByDescending(o => o.AdmCountryMst.cntName).ToList();
                                }
                            }

                            // Code block for adding new sort criteria
                        }
                    }

                    // No sort criteria is given;default sort;
                    else
                    {
                        // Apply Paging And Sorting For EmpEmployeeMst grid Purpose
                        if (utility.SortBy != null || utility.SortDirection != null)
                        {
                            loadEntityQry = loadEntityQry.OrderBy(o => o.empPK).Skip(firstRec).Take(utilityObj.PageSize).ToList();
                        }

                        // Current Page is -1 Used to List the all EmpEmployeeMst with sorting
                        else
                        {
                            loadEntityQry = loadEntityQry.OrderBy(o => o.empPK).ToList();
                        }
                    }
                };

                // Triggering searching,filtering operation
                action(utilityObj);

                // Return EmpEmployeeMst
                return loadEntityQry;
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
                loadEntityQry = null;
                action = null;
            }
        }

        /// <summary>
        /// Gets count of EmpEmployeeMst
        /// </summary>
        /// <param name="empEmployeeMstObj">EmpEmployeeMst object</param>
        /// <param name="utilityObj">Service utility object</param>
        /// <returns>Count of EmpEmployeeMst</returns>
        public ServiceUtility GetEmpEmployeeMstCount(EmpEmployeeMst empEmployeeMstObj, ServiceUtility utilityObj)
        {
            // Gets or sets list of EmpEmployeeMst,search result
            List<EmpEmployeeMst> loadEntityQry;

            // Gets or sets searching,filtering definitions
            Action<ServiceUtility> action;
            try
            {
                // Fileter EmpEmployeeMst with pk,active status
                loadEntityQry = this.currentEntity.EmpEmployeeMsts.Include(DataTableRes.EmpCountryMaster)
                                .Where(emp => emp.empBizUnit == ((empEmployeeMstObj.empBizUnit == -1) ? emp.empBizUnit : empEmployeeMstObj.empBizUnit)
                                && emp.empPK == (empEmployeeMstObj.empPK == 0 ? emp.empPK : empEmployeeMstObj.empPK)
                                || emp.empActive == (empEmployeeMstObj.empActive == 0 ? emp.empActive : empEmployeeMstObj.empActive))
                                .Where(emp => emp.empType == (empEmployeeMstObj.empType > 0 ? empEmployeeMstObj.empType : emp.empType)).ToList();

                // Definition for filtering,utility parameter holds search criteria
                action = utility =>
                {
                    // Checking for filter options are given
                    if (utility.FilterBy != null || utility.FilterValue != string.Empty)
                    {
                        // Filter with EmpEmployeeMst name
                        if (utility.FilterBy == DataFieldRes.EmployeeName)
                        {
                            loadEntityQry = loadEntityQry.Where(o => o.empName.IndexOf(utility.FilterValue, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
                        }

                        // Filter with EmpEmployeeMst code
                        else if (utility.FilterBy == DataFieldRes.EmployeeCode)
                        {
                            loadEntityQry = loadEntityQry.Where(o => o.empCode.IndexOf(utility.FilterValue, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
                        }
                        // Filter EmpEmployeeMst with EmpEmployeeMsttype
                        else if (utility.FilterBy == DataFieldRes.EmpTypeData)
                        {
                            //loadEntityQry = loadEntityQry.Where(o => o.XacEmpType.xetData.IndexOf(utility.FilterValue, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
                            loadEntityQry = loadEntityQry.Where(o => o.XacEmpType.xetData.StartsWith(utility.FilterValue, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        }
                    }
                };

                // Triggering filtering, operation
                action(utilityObj);

                // Sets total EmpEmployeeMst count to service utility
                utilityObj.TotalRecords = loadEntityQry.Count;

                // Returns service utility object with total EmpEmployeeMst count
                return utilityObj;
            }
            catch (Exception ex)
            {
                // Handler for unknown exception
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                // Disposing used objects
                loadEntityQry = null;
                action = null;
            }
        }

        /// <summary>
        /// Gets count of EmpEmployees
        /// </summary>
        /// <param name="empEmployeeMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public ServiceUtility GetEmpEmployesCount(EmpEmployeeMst empEmployeeMstObj, ServiceUtility utilityObj)
        {
            // Gets or sets list of EmpEmployeeMst,search result
            List<EmpEmployeeMst> loadEntityQry;
            try
            {
                // Fileter EmpEmployeeMst with pk,active status
                loadEntityQry = this.currentEntity.EmpEmployeeMsts
                                .Where(emp => emp.empBizUnit == ((empEmployeeMstObj.empBizUnit == -1) ? emp.empBizUnit : empEmployeeMstObj.empBizUnit)
                                && emp.empPK == (empEmployeeMstObj.empPK == 0 ? emp.empPK : empEmployeeMstObj.empPK)
                                && emp.empActive == (empEmployeeMstObj.empActive == 0 ? emp.empActive : empEmployeeMstObj.empActive)
                                && emp.empType == (empEmployeeMstObj.empType > 0 ? empEmployeeMstObj.empType : emp.empType)).ToList();

                // Sets total EmpEmployeeMst count to service utility
                utilityObj.TotalRecords = loadEntityQry.Count;
                // Returns service utility object with total EmpEmployeeMst count
                return utilityObj;
            }
            catch (Exception ex)
            {
                // Handler for unknown exception
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                // Disposing used objects
                loadEntityQry = null;
            }
        }



        /// <summary>
        /// Gets new instance of EmpEmployeeMst with default values.All EmpEmployeeMst object passing to server must be initilied with this method.
        /// </summary>
        /// <returns>EmpEmployeeMst object</returns>
        public EmpEmployeeMst GetInitilizedEmpEmployeeMst()
        {
            try
            {
                // Initilizes new EmpEmployeeMst and assign default values
                EmpEmployeeMst empEmployeeMstObj = new EmpEmployeeMst();

                empEmployeeMstObj.empCode = String.Empty;
                empEmployeeMstObj.empName = String.Empty;
                empEmployeeMstObj.empType = -1;
                empEmployeeMstObj.EmpAddress1 = String.Empty;
                empEmployeeMstObj.EmpCity1 = String.Empty;
                empEmployeeMstObj.empActive = 255;
                empEmployeeMstObj.empTFld1 = String.Empty;
                empEmployeeMstObj.empTFld2 = String.Empty;
                empEmployeeMstObj.empCrtdBy = 1;
                empEmployeeMstObj.empCrtdOn = DateTime.MinValue;
                empEmployeeMstObj.empModBy = 1;
                empEmployeeMstObj.empModOn = DateTime.MinValue;

                // Return Initilized EmpEmployeeMst to client
                return empEmployeeMstObj;
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empEmployeeMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<EmpEmployeeMst> GetEmpEmployeeMstAutoCompleteList(EmpEmployeeMst empEmployeeMstObj, ServiceUtility utilityObj)
        {
            List<EmpEmployeeMst> empEmployeeMstList = null;
            EmpEmployeeMst tempEmpEmployeeMstObj = null;
            int pageSize;
            short rolePK;
            try
            {

                pageSize = Convert.ToInt32(utilityObj.PageSize);
                //if (empEmployeeMstObj.EmpEmployeeRoleMpgs.Count > 0)
                //{
                //    rolePK = empEmployeeMstObj.EmpEmployeeRoleMpgs.First().emrRole;
                //}
                //else
                //{
                //    rolePK = -1;
                //}
                rolePK = -1;

                if (utilityObj.FilterBy == DataFieldRes.EmployeeCode)
                {
                    empEmployeeMstList = currentEntity.EmpEmployeeMsts
                                         .Where(emp => emp.empBizUnit == ((empEmployeeMstObj.empBizUnit == -1) ? emp.empBizUnit : empEmployeeMstObj.empBizUnit)
                                         && emp.empActive == 1 && emp.empCode.StartsWith(utilityObj.FilterValue))
                                         .Where(emp => emp.empType == (empEmployeeMstObj.empType > 0 ? empEmployeeMstObj.empType : emp.empType))
                                         .OrderBy(emp => emp.empCode).ToList();
                }
                else if (utilityObj.FilterBy == DataFieldRes.EmployeeName)
                {

                    //Get All Employees
                    if (rolePK == -1)
                    {
                        empEmployeeMstList = currentEntity.EmpEmployeeMsts
                                           .Where(emp => emp.empBizUnit == ((empEmployeeMstObj.empBizUnit == -1) ? emp.empBizUnit : empEmployeeMstObj.empBizUnit)
                                            && emp.empActive == 1 && emp.empName.StartsWith(utilityObj.FilterValue))
                                            .Where(emp => emp.empType == (empEmployeeMstObj.empType > 0 ? empEmployeeMstObj.empType : emp.empType))
                                           .OrderBy(emp => emp.empName).ToList();
                    }
                    //Get Employee with Specified Roles
                    else
                    {
                        //empEmployeeMstList = currentEntity.EmpEmployeeMsts
                        //                    .Where(emp => emp.empBizUnit == ((empEmployeeMstObj.empBizUnit == -1) ? emp.empBizUnit : empEmployeeMstObj.empBizUnit)
                        //                     && emp.EmpEmployeeRoleMpgs.Where(emr => emr.emrRole == (rolePK == -1 ? emr.emrRole : rolePK)).Count() > 0
                        //                     && emp.empActive == 1 && emp.empName.StartsWith(utilityObj.FilterValue))
                        //                     .Where(emp => emp.empType == (empEmployeeMstObj.empType > 0 ? empEmployeeMstObj.empType : emp.empType))
                        //                    .OrderBy(emp => emp.empName).ToList();
                    }
                }
                else
                {
                    empEmployeeMstList = currentEntity.EmpEmployeeMsts
                                         .Where(emp => emp.empBizUnit == ((empEmployeeMstObj.empBizUnit == -1) ? emp.empBizUnit : empEmployeeMstObj.empBizUnit)
                                          && emp.empActive == 1)
                                         .OrderBy(emp => emp.empName).ToList();
                }

                if (empEmployeeMstList.Count > pageSize)
                {
                    empEmployeeMstList = empEmployeeMstList.Take(pageSize).ToList();
                    tempEmpEmployeeMstObj = new EmpEmployeeMst();
                    tempEmpEmployeeMstObj.empPK = 0;
                    tempEmpEmployeeMstObj.empCode = ERPManagerRes.MoreAutoComplete;
                    tempEmpEmployeeMstObj.empName = ERPManagerRes.MoreAutoComplete;
                    empEmployeeMstList.Add(tempEmpEmployeeMstObj);
                    return empEmployeeMstList;
                }
                else
                {
                    return empEmployeeMstList;
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
                empEmployeeMstList = null;
                tempEmpEmployeeMstObj = null;
            }
        }

        public List<EmpEmployeeMst> GetNonUserEmpEmployeeMstAutoCompleteList(EmpEmployeeMst empEmployeeMstObj, ServiceUtility utilityObj)
        {
            List<EmpEmployeeMst> empEmployeeMstList = null;
            EmpEmployeeMst tempEmpEmployeeMstObj = null;
            int pageSize;
            try
            {
                pageSize = Convert.ToInt32(utilityObj.PageSize);

                //Get All Employees
                //empEmployeeMstList = currentEntity.EmpEmployeeMsts
                //                     .Where(emp => emp.empBizUnit == ((empEmployeeMstObj.empBizUnit == -1) ? emp.empBizUnit : empEmployeeMstObj.empBizUnit)
                //                      && emp.Rmsus.Count <= 0
                //                      && emp.empActive == 1 && emp.empName.StartsWith(utilityObj.FilterValue))
                //                     .OrderBy(emp => emp.empName).ToList();
                if (empEmployeeMstList.Count > pageSize)
                {
                    empEmployeeMstList = empEmployeeMstList.Take(pageSize).ToList();
                    tempEmpEmployeeMstObj = new EmpEmployeeMst();
                    tempEmpEmployeeMstObj.empPK = 0;
                    tempEmpEmployeeMstObj.empCode = ERPManagerRes.MoreAutoComplete;
                    tempEmpEmployeeMstObj.empName = ERPManagerRes.MoreAutoComplete;
                    empEmployeeMstList.Add(tempEmpEmployeeMstObj);
                    return empEmployeeMstList;
                }
                else
                {
                    return empEmployeeMstList;
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
                empEmployeeMstList = null;
                tempEmpEmployeeMstObj = null;
            }
        }

        /// <summary>
        /// Get Employee Type Constant Data
        /// </summary>
        /// <param name="xacEmpTypeObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<XacEmpType> GetXacEmpType(XacEmpType xacEmpTypeObj)
        {
            List<XacEmpType> loadEntityQry;

            try
            {
                // Fileter EmpEmployeeMst with pk,active status
                loadEntityQry = this.currentEntity.XacEmpTypes
                    .Where(xet => xet.xetActive == (xacEmpTypeObj.xetActive > 0 ? xacEmpTypeObj.xetActive : xet.xetActive)
                                && xet.xetSequence == (xacEmpTypeObj.xetSequence > 0 ? xacEmpTypeObj.xetSequence : xet.xetSequence)).ToList();
                return loadEntityQry;
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
                loadEntityQry = null;
            }
        }

        #endregion

    }
}
