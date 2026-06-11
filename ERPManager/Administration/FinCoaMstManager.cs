using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;
using BusinessObject.CommonManagement;
using ERP.Utilities;

namespace ERPManager
{
    public class FinCoaMstManager : IFinCoaMstManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Bank COA Master Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public FinCoaMstManager(ERPEntities currentEntity)
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

        public List<ERPManager.Administration.FinCOAMasterManager> GetFinancialCOAMst(FIN_COA_MST FinCoaMstObj, ServiceUtility utilityObj)
        {
            List<ERPManager.Administration.FinCOAMasterManager> FinCoaMstListObj = new List<ERPManager.Administration.FinCOAMasterManager>();
            IQueryable<ERPManager.Administration.FinCOAMasterManager> FIN_COA_MST_qry;
            ERPManager.Administration.FinCOAMasterManager objCOA = new Administration.FinCOAMasterManager();
            try
            {
                FIN_COA_MST_qry = (from coa in this.currentEntity.FIN_COA_MST
                                   where coa.COA_ACTIVE == FinCoaMstObj.COA_ACTIVE
                                           && coa.COA_PK == (FinCoaMstObj.COA_PK > 0 ? FinCoaMstObj.COA_PK : coa.COA_PK)
                                           && coa.COA_CODE.StartsWith(FinCoaMstObj.COA_CODE)
                                           && coa.COA_NAME.StartsWith(FinCoaMstObj.COA_NAME)
                                           && (coa.COA_BIZUNIT == (FinCoaMstObj.COA_BIZUNIT > 0 ? FinCoaMstObj.COA_BIZUNIT : coa.COA_BIZUNIT))
                                           && coa.COA_IS_GROUP == (FinCoaMstObj.COA_IS_GROUP == true ? true : coa.COA_IS_GROUP)
                                   select new ERPManager.Administration.FinCOAMasterManager
                                   {
                                       FinCOAMst = coa,
                                       COA_NAMETEXT = coa.COA_NAME + " (" + coa.COA_CODE + ")"
                                   });

                if (utilityObj.FilterBy == DataFieldRes.CoaCode)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.FinCOAMst.COA_CODE.StartsWith(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.CoaName)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.FinCOAMst.COA_NAME.StartsWith(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.ConfigName)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.FinCOAMst.ADM_CONFIG_MST.CFG_DATA.StartsWith(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.CoaSubTypeName)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.FinCOAMst.FIN_COA_SUB_TYPE_CFG.CST_NAME.StartsWith(utilityObj.FilterValue));

                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = FIN_COA_MST_qry.Count();

                //Apply Paging And Sorting for grid Purpose
                FinCoaMstListObj = FIN_COA_MST_qry.SortRecords<ERPManager.Administration.FinCOAMasterManager>(utilityObj).ToList();
            }
            catch (Exception ex)
            {

                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return FinCoaMstListObj;

        }
        /// <summary>
        /// Gets list of COA Master 
        /// </summary>
        /// <param name="FinCoaMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Bank Master</returns>
        public List<FIN_COA_MST> GetFinCoaMst(FIN_COA_MST FinCoaMstObj, ServiceUtility utilityObj)
        {
            int companyPK = 0;
            companyPK = string.IsNullOrEmpty(utilityObj.Location) ? 0 : Convert.ToInt32(utilityObj.Location);
            List<FIN_COA_MST> FinCoaMstListObj = new List<FIN_COA_MST>();
            IQueryable<FIN_COA_MST> FIN_COA_MST_qry;
            try
            {
                FIN_COA_MST_qry = (from coa in this.currentEntity.FIN_COA_MST
                                   where coa.COA_ACTIVE == FinCoaMstObj.COA_ACTIVE
                                           && coa.COA_PK == (FinCoaMstObj.COA_PK > 0 ? FinCoaMstObj.COA_PK : coa.COA_PK)
                                           && coa.COA_CODE.StartsWith(FinCoaMstObj.COA_CODE)
                                           && coa.COA_NAME.StartsWith(FinCoaMstObj.COA_NAME)
                                           && (coa.COA_BIZUNIT == (FinCoaMstObj.COA_BIZUNIT > 0 ? FinCoaMstObj.COA_BIZUNIT : coa.COA_BIZUNIT))
                                           && coa.COA_IS_GROUP == (FinCoaMstObj.COA_IS_GROUP == true ? true : coa.COA_IS_GROUP)
                                   //&& coa.COA_COMPANY == (companyPK > 0 ? companyPK : coa.COA_COMPANY) ticket id :459
                                   //  select new { coa, COA_NAMETEXT = coa.COA_NAME + " (" + coa.COA_CODE + ")" });
                                   select coa);
                //.OrderBy(c => c.COA_LEVEL).ThenBy(c1 => c1.COA_SEQUENCE).ToList();

                //unable to search using words in between : changed Startswith to contains   -  Bug:23002
                if (utilityObj.FilterBy == DataFieldRes.CoaCode)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.COA_CODE.Contains(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.CoaName)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.COA_NAME.Contains(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.ConfigName)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.ADM_CONFIG_MST.CFG_DATA.Contains(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.CoaSubTypeName)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.FIN_COA_SUB_TYPE_CFG.CST_NAME.Contains(utilityObj.FilterValue));

                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = FIN_COA_MST_qry.Count();

                //Filter Query
                //paymentHdrQuery = FilterEntity(finPaymentVndHdrListObj, paymentHdrQuery, serviceUtilityObj);

                //Apply Paging And Sorting for grid Purpose
                FinCoaMstListObj = FIN_COA_MST_qry.SortRecords<FIN_COA_MST>(utilityObj).ToList();
            }
            catch (Exception ex)
            {

                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return FinCoaMstListObj;

        }

        public List<FIN_COA_MST> GetFinCoaMstforGL(FIN_COA_MST FinCoaMstObj, ServiceUtility utilityObj)
        {
            int companyPK = 0;
            companyPK = string.IsNullOrEmpty(utilityObj.Location) ? 0 : Convert.ToInt32(utilityObj.Location);
            List<FIN_COA_MST> FinCoaMstListObj = new List<FIN_COA_MST>();
            IQueryable<FIN_COA_MST> FIN_COA_MST_qry;
            try
            {
                FIN_COA_MST_qry = (from coa in this.currentEntity.FIN_COA_MST
                                   where coa.COA_ACTIVE == FinCoaMstObj.COA_ACTIVE
                                           && coa.COA_PK == (FinCoaMstObj.COA_PK > 0 ? FinCoaMstObj.COA_PK : coa.COA_PK)
                                           && coa.COA_CODE.StartsWith(FinCoaMstObj.COA_CODE)
                                           && coa.COA_NAME.StartsWith(FinCoaMstObj.COA_NAME)
                                           && coa.COA_TYPE != 1105 // COA_Name = CAPITAL
                                           && coa.COA_BIZUNIT== FinCoaMstObj.COA_BIZUNIT
                                           && coa.COA_IS_GROUP == (FinCoaMstObj.COA_IS_GROUP == true ? true : coa.COA_IS_GROUP)
                                           && coa.COA_COMPANY == (companyPK > 0 ? companyPK : coa.COA_COMPANY)
                                           
                                   select coa);
                //.OrderBy(c => c.COA_LEVEL).ThenBy(c1 => c1.COA_SEQUENCE).ToList();

                if (utilityObj.FilterBy == DataFieldRes.CoaCode)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.COA_CODE.StartsWith(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.CoaName)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.COA_NAME.StartsWith(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.ConfigName)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.ADM_CONFIG_MST.CFG_DATA.StartsWith(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.CoaSubTypeName)
                    FIN_COA_MST_qry = FIN_COA_MST_qry.Where(a => a.FIN_COA_SUB_TYPE_CFG.CST_NAME.StartsWith(utilityObj.FilterValue));

                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = FIN_COA_MST_qry.Count();

                //Filter Query
                //paymentHdrQuery = FilterEntity(finPaymentVndHdrListObj, paymentHdrQuery, serviceUtilityObj);

                //Apply Paging And Sorting for grid Purpose
                FinCoaMstListObj = FIN_COA_MST_qry.SortRecords<FIN_COA_MST>(utilityObj).ToList();
            }
            catch (Exception ex)
            {

                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return FinCoaMstListObj;

        }

        public int FinCoaBalanceSave(int coaPK, decimal coaAmount)
        {
            FIN_COA_MST oldFinCoaMstObj;
            try
            {
                oldFinCoaMstObj = currentEntity.FIN_COA_MST.SingleOrDefault(coa => coa.COA_PK == coaPK);
                if (oldFinCoaMstObj != null)
                {
                    oldFinCoaMstObj.COA_CURR_BAL = oldFinCoaMstObj.COA_CURR_BAL + coaAmount;
                }
                else
                {
                    // throws exception already deleted or modified by other user
                    throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                }
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return oldFinCoaMstObj.COA_PK;
        }

        public int SaveAccountsMaster(List<FIN_COA_MST> accountMstList,int AddParentChildValidation)
        {
            int retval = 0;
            int? maxPK;
            FIN_COA_MST OLD_FIN_COA_MST_Obj;
            FIN_COA_MST Parent_FIN_COA_MST_Obj = null;
            List<FIN_COA_MST> tempFinCoaListObj;
            FIN_COA_MST tempFinCoaObj;
            try
            {
                foreach (FIN_COA_MST FIN_COA_MST_Obj in accountMstList)
                {
                    if (FIN_COA_MST_Obj.COA_PARENT != null)
                        Parent_FIN_COA_MST_Obj = this.currentEntity.FIN_COA_MST.SingleOrDefault(a => a.COA_PK == FIN_COA_MST_Obj.COA_PARENT);

                    if (FIN_COA_MST_Obj.COA_PK == 0) //SAVE
                    {
                        // Gets last pk
                        maxPK = this.currentEntity.FIN_COA_MST.Max(a => (int?)a.COA_PK);

                        // Sets return value as next pk
                        retval = (maxPK.HasValue ? maxPK.Value + 1 : 1);

                        FIN_COA_MST_Obj.COA_PK = retval;
                        FIN_COA_MST_Obj.COA_CRTD_DT = DateTime.Now;
                        FIN_COA_MST_Obj.COA_MOD_DT = DateTime.Now;
                        FIN_COA_MST_Obj.COA_LEVEL = Parent_FIN_COA_MST_Obj.COA_LEVEL;
                        FIN_COA_MST_Obj.COA_LEVEL += 1;

                        this.currentEntity.FIN_COA_MST.AddObject(FIN_COA_MST_Obj);

                    }
                    else//UPDATION
                    {
                        OLD_FIN_COA_MST_Obj = this.currentEntity.FIN_COA_MST.SingleOrDefault(a => a.COA_PK == FIN_COA_MST_Obj.COA_PK);
                        if (OLD_FIN_COA_MST_Obj != null && OLD_FIN_COA_MST_Obj.COA_MOD_DT == FIN_COA_MST_Obj.COA_MOD_DT)
                        {
                            bool isValid = true;
                            ////

                            tempFinCoaObj = null;
                            tempFinCoaObj = this.currentEntity.FIN_COA_MST.SingleOrDefault(a => a.COA_PK == FIN_COA_MST_Obj.COA_PARENT);

                            int? parent1 = null;
                            if (tempFinCoaObj != null)
                            {
                                parent1 = tempFinCoaObj.COA_PK;
                            }
                            while (tempFinCoaObj != null)
                            {
                                //tempFinCoaObj = this.currentEntity.FIN_COA_MST.SingleOrDefault(a => a.COA_PK == tempFinCoaObj.COA_PARENT);
                                tempFinCoaObj = tempFinCoaObj.FIN_COA_MST2;
                                if (tempFinCoaObj != null)
                                {
                                    parent1 = tempFinCoaObj.COA_PK;
                                }
                            }

                            tempFinCoaObj = null;
                            tempFinCoaObj = this.currentEntity.FIN_COA_MST.SingleOrDefault(a => a.COA_PK == FIN_COA_MST_Obj.COA_PK);
                            int? parent2 = null;
                            if (tempFinCoaObj != null)
                            {
                                parent2 = tempFinCoaObj.COA_PK;
                            }
                            while (tempFinCoaObj != null)
                            {
                                //tempFinCoaObj = this.currentEntity.FIN_COA_MST.SingleOrDefault(a => a.COA_PK == tempFinCoaObj.COA_PARENT);
                                tempFinCoaObj = tempFinCoaObj.FIN_COA_MST2;
                                if (tempFinCoaObj != null)
                                {
                                    parent2 = tempFinCoaObj.COA_PK;
                                }
                            }

                            if (parent1 != null && parent2 != null && parent1 == parent2)
                            {
                                isValid = true;
                            }
                            else
                            {
                                tempFinCoaListObj = null;
                                if (AddParentChildValidation == 1)
                                {
                                    tempFinCoaListObj = this.currentEntity.FIN_COA_MST.Where(a => a.COA_PARENT == FIN_COA_MST_Obj.COA_PK).ToList();
                                    if (tempFinCoaListObj == null || (tempFinCoaListObj != null && tempFinCoaListObj.Count == 0))
                                    {
                                        isValid = true;
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }
                                }
                            }

                            ////
                            if (isValid)
                            {
                                OLD_FIN_COA_MST_Obj.COA_CODE = FIN_COA_MST_Obj.COA_CODE;
                                OLD_FIN_COA_MST_Obj.COA_NAME = FIN_COA_MST_Obj.COA_NAME;
                                OLD_FIN_COA_MST_Obj.COA_SHORT_NAME = FIN_COA_MST_Obj.COA_SHORT_NAME;
                                OLD_FIN_COA_MST_Obj.COA_DESC = FIN_COA_MST_Obj.COA_DESC;
                                OLD_FIN_COA_MST_Obj.COA_TYPE = FIN_COA_MST_Obj.COA_TYPE;
                                OLD_FIN_COA_MST_Obj.COA_SUB_TYPE = FIN_COA_MST_Obj.COA_SUB_TYPE;
                                OLD_FIN_COA_MST_Obj.COA_REPORT_COLUMN = FIN_COA_MST_Obj.COA_REPORT_COLUMN > 0 ? FIN_COA_MST_Obj.COA_REPORT_COLUMN : null;

                                OLD_FIN_COA_MST_Obj.COA_REPORT_TEMPLATE = FIN_COA_MST_Obj.COA_REPORT_TEMPLATE > 0 ? FIN_COA_MST_Obj.COA_REPORT_TEMPLATE : null;

                                OLD_FIN_COA_MST_Obj.COA_PARENT = FIN_COA_MST_Obj.COA_PARENT;
                                if(Parent_FIN_COA_MST_Obj!=null)
                                {
                                    OLD_FIN_COA_MST_Obj.COA_LEVEL = Parent_FIN_COA_MST_Obj.COA_LEVEL;   // FIN_COA_MST_Obj.COA_LEVEL;
                                    OLD_FIN_COA_MST_Obj.COA_LEVEL += 1;

                                }
                                else
                                {
                                    OLD_FIN_COA_MST_Obj.COA_LEVEL = 1;

                                }

                                tempFinCoaListObj = null;
                                tempFinCoaListObj = this.currentEntity.FIN_COA_MST.Where(a => a.COA_PARENT == FIN_COA_MST_Obj.COA_PK).ToList();
                                if (tempFinCoaListObj != null && tempFinCoaListObj.Count > 0)
                                {
                                    UpdateLevel(tempFinCoaListObj, FIN_COA_MST_Obj.COA_PK, OLD_FIN_COA_MST_Obj.COA_LEVEL);
                                }

                                OLD_FIN_COA_MST_Obj.COA_SEQUENCE = FIN_COA_MST_Obj.COA_SEQUENCE;
                                OLD_FIN_COA_MST_Obj.COA_IS_GROUP = FIN_COA_MST_Obj.COA_IS_GROUP;
                                //OLD_FIN_COA_MST_Obj.COA_CURR_BAL = FIN_COA_MST_Obj.COA_CURR_BAL;
                                OLD_FIN_COA_MST_Obj.COA_REMARKS = FIN_COA_MST_Obj.COA_REMARKS;
                                OLD_FIN_COA_MST_Obj.COA_ACTIVE = FIN_COA_MST_Obj.COA_ACTIVE;
                                OLD_FIN_COA_MST_Obj.COA_MOD_BY = FIN_COA_MST_Obj.COA_MOD_BY;
                                OLD_FIN_COA_MST_Obj.COA_MOD_DT = DateTime.Now;  //FIN_COA_MST_Obj.COA_MOD_DT;
                                OLD_FIN_COA_MST_Obj.COA_DEPT = FIN_COA_MST_Obj.COA_DEPT;
                                OLD_FIN_COA_MST_Obj.COA_BIZUNIT = FIN_COA_MST_Obj.COA_BIZUNIT;
                                OLD_FIN_COA_MST_Obj.COA_COMPANY = FIN_COA_MST_Obj.COA_COMPANY;

                                retval = FIN_COA_MST_Obj.COA_PK;
                            }
                            else
                            {
                                retval = -2;
                            }
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }
                }
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

            return retval;
        }


        private void UpdateLevel(List<FIN_COA_MST> tempFinCoaListObj, int parentPK, byte parentLevel)
        {
            if (tempFinCoaListObj != null && tempFinCoaListObj.Count > 0)
            {
                List<FIN_COA_MST> tempList;

                foreach (FIN_COA_MST finObj in tempFinCoaListObj)
                {
                    finObj.COA_LEVEL = parentLevel;
                    finObj.COA_LEVEL += 1;

                    tempList = this.currentEntity.FIN_COA_MST.Where(a => a.COA_PARENT == finObj.COA_PK).ToList();
                    if (tempList != null && tempList.Count > 0)
                    {
                        UpdateLevel(tempList, finObj.COA_PK, finObj.COA_LEVEL);
                    }
                }
            }
        }

        public int DeleteAccountsMaster(List<FIN_COA_MST> accountMstList)
        {
            int retval = 0;
            FIN_COA_MST Old_FIN_COA_MST_Obj;

            try
            {
                foreach (FIN_COA_MST FIN_COA_MST_Obj in accountMstList)
                {
                    Old_FIN_COA_MST_Obj = currentEntity.FIN_COA_MST.SingleOrDefault(sah => sah.COA_PK == FIN_COA_MST_Obj.COA_PK && sah.COA_MOD_DT == FIN_COA_MST_Obj.COA_MOD_DT);

                    if (Old_FIN_COA_MST_Obj != null)
                    {

                        if (!Old_FIN_COA_MST_Obj.COA_PARENT.HasValue)
                        {
                            retval = -3;
                            break;
                        }
                        else
                        {
                            this.currentEntity.FIN_COA_MST.DeleteObject(Old_FIN_COA_MST_Obj);
                            retval = 1;
                        }
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

        /// <summary>
        /// Gets list of Coa Master after filtering,sorting for filling auto complete list
        /// </summary>
        /// <param name="utilityObj"></param>
        /// <returns>List of Coa Master</returns>
        public List<FIN_COA_MST> GetCoaMstAutoCompleteList(FIN_COA_MST finCoaMstObj, string voucherType, int accType, ServiceUtility utilityObj,int companyPK=0)
        {
            List<FIN_COA_MST> finCoaMstList;
            FIN_COA_MST tempfinCoaMstObj = null;
            int pageSize;
            try
            {
                finCoaMstList = new List<FIN_COA_MST>();
                pageSize = Convert.ToInt32(utilityObj.PageSize);



                if (utilityObj.FilterBy == DataFieldRes.CoaCode)
                {
                    finCoaMstList = currentEntity.FIN_COA_MST
                                    .Where(coa => coa.COA_BIZUNIT == ((!((int?)finCoaMstObj.COA_BIZUNIT).HasValue || finCoaMstObj.COA_BIZUNIT <= 0) ? coa.COA_BIZUNIT : finCoaMstObj.COA_BIZUNIT)
                                    && coa.COA_ACTIVE == 1 && coa.COA_CODE.StartsWith(utilityObj.FilterValue)
                                    && coa.COA_IS_GROUP == finCoaMstObj.COA_IS_GROUP
                                    && coa.COA_COMPANY == (companyPK > 0 ? companyPK : coa.COA_COMPANY))
                                    .OrderBy(coa => coa.COA_CODE).ToList();
                }
                else if (utilityObj.FilterBy == DataFieldRes.CoaName)
                {
                    finCoaMstList = currentEntity.FIN_COA_MST
                                    .Where(coa => coa.COA_BIZUNIT == ((!((int?)finCoaMstObj.COA_BIZUNIT).HasValue || finCoaMstObj.COA_BIZUNIT <= 0) ? coa.COA_BIZUNIT : finCoaMstObj.COA_BIZUNIT)
                                    && coa.COA_ACTIVE == 1 && coa.COA_NAME.StartsWith(utilityObj.FilterValue)
                                    && coa.COA_IS_GROUP == finCoaMstObj.COA_IS_GROUP
                                     && coa.COA_COMPANY == (companyPK > 0 ? companyPK : coa.COA_COMPANY))
                                    .OrderBy(coa => coa.COA_NAME).ToList();
                }
                else if (accType == 3)
                {
                    finCoaMstList = currentEntity.FIN_COA_MST
                                    .Where(coa => coa.COA_BIZUNIT == ((!((int?)finCoaMstObj.COA_BIZUNIT).HasValue || finCoaMstObj.COA_BIZUNIT <= 0) ? coa.COA_BIZUNIT : finCoaMstObj.COA_BIZUNIT)
                                    && coa.COA_ACTIVE == 1 && (coa.COA_NAME.StartsWith(utilityObj.FilterValue) || coa.COA_CODE.StartsWith(utilityObj.FilterValue))
                                    && coa.COA_IS_GROUP == finCoaMstObj.COA_IS_GROUP
                                    && coa.COA_TYPE != 1105
                                    && coa.COA_COMPANY == (companyPK > 0 ? companyPK : coa.COA_COMPANY))
                                    .OrderBy(coa => coa.COA_CODE).ToList();
                }
                else

                {
                    finCoaMstList = currentEntity.FIN_COA_MST
                                    .Where(coa => coa.COA_BIZUNIT == ((!((int?)finCoaMstObj.COA_BIZUNIT).HasValue || finCoaMstObj.COA_BIZUNIT <= 0) ? coa.COA_BIZUNIT : finCoaMstObj.COA_BIZUNIT)
                                    && coa.COA_ACTIVE == 1 && (coa.COA_NAME.StartsWith(utilityObj.FilterValue) || coa.COA_CODE.StartsWith(utilityObj.FilterValue))
                                    && coa.COA_IS_GROUP == finCoaMstObj.COA_IS_GROUP
                                    && coa.COA_COMPANY== (companyPK > 0 ? companyPK: coa.COA_COMPANY))
                                    .OrderBy(coa => coa.COA_CODE).ToList();
                }

                if (!string.IsNullOrEmpty(voucherType))
                {
                    FIN_VOUCHER_ENTRY_CFG FinVchrEntryCfgObj = new FIN_VOUCHER_ENTRY_CFG();
                    List<FIN_VOUCHER_ENTRY_CFG> FinVchrEntryCfgObjList;

                    FinVoucherEntryCfgManager FinVoucherEntryCfgManagerObj = new FinVoucherEntryCfgManager(currentEntity);

                    FinVchrEntryCfgObj.VEC_ACTIVE = 1;
                    FinVchrEntryCfgObj.VEC_TYPE = voucherType;
                    FinVchrEntryCfgObjList = FinVoucherEntryCfgManagerObj.GetVcoucherEntryCfg(FinVchrEntryCfgObj, utilityObj);

                    if (FinVchrEntryCfgObjList != null && FinVchrEntryCfgObjList.Count > 0 && accType > 0)
                    {
                        var subTypeList = (from dtl in (accType > 1 ? FinVchrEntryCfgObjList[0].VEC_ACC_TYPES2.Split(',').AsEnumerable() : FinVchrEntryCfgObjList[0].VEC_ACC_TYPES1.Split(',').AsEnumerable())
                                           select (string.IsNullOrEmpty(dtl) ? 0 : Convert.ToInt32(dtl)));

                        finCoaMstList = (from f in finCoaMstList
                                         where subTypeList.Contains(f.COA_SUB_TYPE)
                                         select f).ToList();
                    }
                }
                return finCoaMstList;
                #region Write for page indexing in autocomplete, bt not finished
                //if (finCoaMstList.Count > pageSize)
                //{
                //    finCoaMstList = finCoaMstList.Take(pageSize).ToList();
                //    tempfinCoaMstObj = new FIN_COA_MST();
                //    tempfinCoaMstObj.COA_PK = 0;
                //    tempfinCoaMstObj.COA_CODE = DMSManagerRes.MoreAutoComplete;
                //    tempfinCoaMstObj.COA_NAME = DMSManagerRes.MoreAutoComplete;
                //    finCoaMstList.Add(tempfinCoaMstObj);
                //    return finCoaMstList;
                //}
                //else
                //{
                //    return finCoaMstList;
                //}
                #endregion
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
                finCoaMstList = null;
                tempfinCoaMstObj = null;
            }
        }
        /// <summary>
        /// Gets list of Coa Master after filtering,sorting for filling auto complete list
        /// </summary>
        /// <param name="utilityObj"></param>
        /// <returns>List of Coa Master</returns>
        public List<FIN_COA_MST> GetAccountCoaMstAutoCompleteList(FIN_COA_MST finCoaMstObj, int accType, ServiceUtility utilityObj)
        {
            List<FIN_COA_MST> finCoaMstList;
            try
            {
                finCoaMstList = new List<FIN_COA_MST>();

                if (utilityObj.FilterBy == DataFieldRes.CoaCode)
                {
                    finCoaMstList = currentEntity.FIN_COA_MST
                                    .Where(coa => coa.COA_BIZUNIT == ((!((int?)finCoaMstObj.COA_BIZUNIT).HasValue || finCoaMstObj.COA_BIZUNIT <= 0) ? coa.COA_BIZUNIT : finCoaMstObj.COA_BIZUNIT)
                                    && coa.COA_ACTIVE == 1
                                        //&& coa.COA_CODE.StartsWith(utilityObj.FilterValue)
                                    && (coa.COA_CODE + " - " + coa.COA_NAME).Contains(utilityObj.FilterValue)
                                    && coa.COA_SUB_TYPE == accType
                                    && coa.COA_IS_GROUP == false)
                                    .OrderBy(coa => coa.COA_CODE).ToList();
                }
                else if (utilityObj.FilterBy == DataFieldRes.CoaName)
                {
                    finCoaMstList = currentEntity.FIN_COA_MST
                                    .Where(coa => coa.COA_BIZUNIT == ((!((int?)finCoaMstObj.COA_BIZUNIT).HasValue || finCoaMstObj.COA_BIZUNIT <= 0) ? coa.COA_BIZUNIT : finCoaMstObj.COA_BIZUNIT)
                                    && coa.COA_ACTIVE == 1
                                        // && coa.COA_NAME.StartsWith(utilityObj.FilterValue)
                                    && (coa.COA_CODE + " - " + coa.COA_NAME).Contains(utilityObj.FilterValue)
                                    && accType == 0 ? true : (coa.COA_SUB_TYPE == accType)
                                    && coa.COA_IS_GROUP == false)
                                    .OrderBy(coa => coa.COA_NAME).ToList();
                }
                else
                {
                    finCoaMstList = currentEntity.FIN_COA_MST
                                    .Where(coa => coa.COA_BIZUNIT == ((!((int?)finCoaMstObj.COA_BIZUNIT).HasValue || finCoaMstObj.COA_BIZUNIT <= 0) ? coa.COA_BIZUNIT : finCoaMstObj.COA_BIZUNIT)
                                    && coa.COA_ACTIVE == 1
                                        //&& (coa.COA_NAME.StartsWith(utilityObj.FilterValue) || coa.COA_CODE.StartsWith(utilityObj.FilterValue))
                                    && (coa.COA_NAME.Contains(utilityObj.FilterValue) || coa.COA_CODE.Contains(utilityObj.FilterValue))
                                    && coa.COA_SUB_TYPE == accType
                                    && coa.COA_IS_GROUP == false)
                                    .OrderBy(coa => coa.COA_CODE).ToList();
                }

                return finCoaMstList;


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
                finCoaMstList = null;
            }
        }
        #endregion

        public List<DDLMaster> GetJournalAccountsAutoCompleteList(int accType, ServiceUtility utilityObj)
        {
            IQueryable<FIN_COA_SUB_TYPE_CFG> qry = null;
            List<FIN_COA_SUB_TYPE_CFG> finCoaSubTypeCfgList;
            string query = string.Empty;
            List<DDLMaster> ddlValues;
            ddlValues = new List<DDLMaster>();
            try
            {
                qry = (from st in this.currentEntity.FIN_COA_SUB_TYPE_CFG
                       where st.CST_ACTIVE == 1 &&//Convert.ToByte(DbActiveStatus.ACTIVE) &&
                       st.CST_PK == (accType >= 0 ? accType : st.CST_PK)
                       select st);
                finCoaSubTypeCfgList = qry.ToList();
                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                {
                    query = finCoaSubTypeCfgList[0].ADM_QUERIES_CFG.QRY_QUERY;
                    if (utilityObj.BizUnit.HasValue)
                        query = query.Replace("@P_VALUE", "'%" + utilityObj.FilterValue + "%' AND [COA_BIZUNIT] =" + utilityObj.BizUnit);
                    else
                        query = query.Replace("@P_VALUE", "'%" + utilityObj.FilterValue + "%'");
                }
                if (!string.IsNullOrEmpty(query))
                {
                    CommonFunctionsManager commonFunctionsManagerObj;
                    commonFunctionsManagerObj = new CommonFunctionsManager(this.currentEntity);
                    ddlValues = commonFunctionsManagerObj.ExecuteQuery(query);
                }
                return ddlValues;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
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
                finCoaSubTypeCfgList = null;
            }
        }
        //public List<SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result> GetFinRptTemplateMst(SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result FinRptMstObj, ServiceUtility utilityObj)
        //{
        //    List<SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result> FinRptTempListObj = new List<SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result>();
        //    IQueryable<SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result> FIN_RptTemp_MST_qry = null;
        //    try
        //    {
        //        FIN_RptTemp_MST_qry = (from rpt in this.currentEntity.SPFIN_REPORT_TEMPLATE_CFG_GET_KV(
        //                                   FinRptMstObj.RTC_PK,
        //                                   FinRptMstObj.RTC_ACTIVE == FinRptMstObj.RTC_PK ? (byte)0 : FinRptMstObj.RTC_ACTIVE,
        //                                   FinRptMstObj.RTC_PK > 0 ? FinRptMstObj.RTC_ACTIVE = (byte)0 : FinRptMstObj.RTC_ACTIVE = FinRptMstObj.RTC_ACTIVE,
        //                                   FinRptMstObj.RTC_PK > 0 ? FinRptMstObj.RTC_TEMPLATE = 1 : FinRptMstObj.RTC_TEMPLATE = FinRptMstObj.RTC_TEMPLATE,
        //                                   FinRptMstObj.RTC_IS_GROUP == 0 ? Convert.ToByte(null) : FinRptMstObj.RTC_IS_GROUP, // Group
        //                                   FinRptMstObj.RTC_BIZUNIT > 0 ? FinRptMstObj.RTC_BIZUNIT : 1).ToList()
        //                               select rpt).AsQueryable()
        //                               .OrderBy(c => c.RTC_LEVEL).ThenBy(c1 => c1.RTC_SEQUENCE);
        //        if (utilityObj.FilterBy == DataFieldRes.RTC_NAME)
        //            FIN_RptTemp_MST_qry = FIN_RptTemp_MST_qry.Where(a => a.RTC_NAME.StartsWith(utilityObj.FilterValue));

        //        if (utilityObj.FilterBy == DataFieldRes.RTC_PARENT_TEXT && utilityObj.FilterValue.Length > 0)
        //            FIN_RptTemp_MST_qry = FIN_RptTemp_MST_qry.Where(a => a.RTC_PARENT_TEXT.StartsWith(utilityObj.FilterValue));

        //        FinRptTempListObj = FIN_RptTemp_MST_qry.ToList<SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result>();

        //        //Set page size one if not given
        //        utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
        //        utilityObj.TotalRecords = FinRptTempListObj.Count();

        //        //Apply Paging And Sorting for grid Purpose
        //        FinRptTempListObj = FIN_RptTemp_MST_qry.SortRecords<SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result>(utilityObj).ToList();

        //    }
        //    catch (Exception ex)
        //    {

        //        //Throws a new exception to service class with class name - method name - server side exception process result as exception message
        //        throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }

        //    return FinRptTempListObj;

        //}
        public DataTable GetFinGroup(int pk, int active, int template, int group, int bizunit)
        {
            try
            {
                DataTable dt = new DataTable();
                List<SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result> resultTrxNo;
                resultTrxNo = this.currentEntity.SPFIN_REPORT_TEMPLATE_CFG_GET_KV(pk, (byte)active, (byte)template, (byte)group, bizunit, null, null, string.Empty, null, null).ToList();
                if (resultTrxNo != null && resultTrxNo.Count > 0)
                    dt = resultTrxNo.ToList().ToDataTable();
                return dt;
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

        public List<SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result> GetFinRptTemplateMst(SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result FinRptMstObj, ServiceUtility utilityObj)
        {
            throw new NotImplementedException();
        }
    }
}
