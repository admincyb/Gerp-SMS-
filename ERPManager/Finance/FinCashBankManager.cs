using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;

namespace ERPManager
{
    public class FinCashBankManager : IFinCashBankManager 
    {

        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// FinCashBankManager Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>

        public FinCashBankManager(ERPEntities currentEntity)
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

        public List<FIN_CASH_BANK_MST> GetFinCashBank(FIN_CASH_BANK_MST FinCashBankMstObj, ServiceUtility utilityObj)
        {
            List<FIN_CASH_BANK_MST> FinCashBankListObj = new List<FIN_CASH_BANK_MST>();

            IQueryable<FIN_CASH_BANK_MST> finCashBankMstQuery;

            try
            {
                finCashBankMstQuery = (from cbm in this.currentEntity.FIN_CASH_BANK_MST
                                       where cbm.CBM_PK == (FinCashBankMstObj.CBM_PK > 0 ? FinCashBankMstObj.CBM_PK : cbm.CBM_PK)
                                        &&( utilityObj.IsSBUSpecific==true ? cbm.CBM_BIZUNIT==FinCashBankMstObj.CBM_BIZUNIT : true)
                                        && cbm.CBM_TYPE == (FinCashBankMstObj.CBM_TYPE > 0 ? FinCashBankMstObj.CBM_TYPE : cbm.CBM_TYPE)
                                        && cbm.CBM_ACTIVE == (FinCashBankMstObj.CBM_ACTIVE > 0 ? FinCashBankMstObj.CBM_ACTIVE : cbm.CBM_ACTIVE)
                                        && cbm.CBM_CODE.StartsWith(FinCashBankMstObj.CBM_CODE)
                                                    && cbm.CBM_NAME.StartsWith(FinCashBankMstObj.CBM_NAME)
                                       select cbm);

                if (utilityObj.FilterBy == DataFieldRes.BankCode)
                    finCashBankMstQuery = finCashBankMstQuery.Where(a => a.CBM_CODE.StartsWith(utilityObj.FilterValue));

                if (utilityObj.FilterBy == DataFieldRes.BankName)
                    finCashBankMstQuery = finCashBankMstQuery.Where(a => a.CBM_NAME.StartsWith(utilityObj.FilterValue));

                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = finCashBankMstQuery.Count();

                //Filter Query
                //paymentHdrQuery = FilterEntity(finPaymentVndHdrListObj, paymentHdrQuery, serviceUtilityObj);

                //Apply Paging And Sorting for grid Purpose
                FinCashBankListObj = finCashBankMstQuery.SortRecords<FIN_CASH_BANK_MST>(utilityObj).ToList();

            }
            catch
            {

            }

            return FinCashBankListObj;
        }

        public int SaveFinCashBank(List<FIN_CASH_BANK_MST> finCashBankMstList)
        {
            int retval = 0;
            int? maxPK;

            FIN_CASH_BANK_MST Old_FIN_CASH_BANK_MST_Obj;

            try
            {
                foreach (FIN_CASH_BANK_MST FIN_CASH_BANK_MST_Obj in finCashBankMstList)
                {
                    //if (FIN_COA_MST_Obj.COA_PARENT != null)
                    //    Parent_FIN_COA_MST_Obj = this.currentEntity.FIN_COA_MST.SingleOrDefault(a => a.COA_PK == FIN_COA_MST_Obj.COA_PARENT);

                    if (FIN_CASH_BANK_MST_Obj.CBM_PK  == 0) //SAVE
                    {
                        // Gets last pk
                        maxPK = this.currentEntity.FIN_CASH_BANK_MST.Max(a => (int?)a.CBM_PK);

                        // Sets return value as next pk
                        retval = (maxPK.HasValue ? maxPK.Value + 1 : 1);

                        FIN_CASH_BANK_MST_Obj.CBM_PK  = (short)retval;
                        FIN_CASH_BANK_MST_Obj.CBM_CRTD_DT  = DateTime.Now;
                        FIN_CASH_BANK_MST_Obj.CBM_MOD_DT  = DateTime.Now;


                        this.currentEntity.FIN_CASH_BANK_MST.AddObject(FIN_CASH_BANK_MST_Obj);

                    }
                    else//UPDATION
                    {
                        Old_FIN_CASH_BANK_MST_Obj = this.currentEntity.FIN_CASH_BANK_MST.SingleOrDefault(a => a.CBM_PK == FIN_CASH_BANK_MST_Obj.CBM_PK && a.CBM_MOD_DT == FIN_CASH_BANK_MST_Obj.CBM_MOD_DT);

                        if (Old_FIN_CASH_BANK_MST_Obj != null)
                        {
                            Old_FIN_CASH_BANK_MST_Obj.CBM_TYPE = FIN_CASH_BANK_MST_Obj.CBM_TYPE;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_CODE = FIN_CASH_BANK_MST_Obj.CBM_CODE;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_NAME = FIN_CASH_BANK_MST_Obj.CBM_NAME;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_ACC_NO = FIN_CASH_BANK_MST_Obj.CBM_ACC_NO;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_BRANCH = FIN_CASH_BANK_MST_Obj.CBM_BRANCH;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_ADDRESS = FIN_CASH_BANK_MST_Obj.CBM_ADDRESS;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_CITY = FIN_CASH_BANK_MST_Obj.CBM_CITY;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_COUNTRY = FIN_CASH_BANK_MST_Obj.CBM_COUNTRY;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_STATE = FIN_CASH_BANK_MST_Obj.CBM_STATE;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_STATE_OTHER = FIN_CASH_BANK_MST_Obj.CBM_STATE_OTHER;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_ZIP = FIN_CASH_BANK_MST_Obj.CBM_ZIP;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_PHONE = FIN_CASH_BANK_MST_Obj.CBM_PHONE;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_MOBILE = FIN_CASH_BANK_MST_Obj.CBM_MOBILE;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_FAX = FIN_CASH_BANK_MST_Obj.CBM_FAX;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_EMAIL = FIN_CASH_BANK_MST_Obj.CBM_EMAIL;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_IFSC_CODE = FIN_CASH_BANK_MST_Obj.CBM_IFSC_CODE;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_SWIFT_CODE = FIN_CASH_BANK_MST_Obj.CBM_SWIFT_CODE;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_ACCOUNT_TYPE = FIN_CASH_BANK_MST_Obj.CBM_ACCOUNT_TYPE;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_CONTACT = FIN_CASH_BANK_MST_Obj.CBM_CONTACT;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_DESC = FIN_CASH_BANK_MST_Obj.CBM_DESC;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_ACCOUNT = FIN_CASH_BANK_MST_Obj.CBM_ACCOUNT;

                            Old_FIN_CASH_BANK_MST_Obj.CBM_ACTIVE = FIN_CASH_BANK_MST_Obj.CBM_ACTIVE;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_MOD_BY = FIN_CASH_BANK_MST_Obj.CBM_MOD_BY;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_MOD_DT = DateTime.Now;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_DEPT = FIN_CASH_BANK_MST_Obj.CBM_DEPT;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_BIZUNIT = FIN_CASH_BANK_MST_Obj.CBM_BIZUNIT;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_CURRENCY = FIN_CASH_BANK_MST_Obj.CBM_CURRENCY;
                            Old_FIN_CASH_BANK_MST_Obj.CBM_FC_HOLD = FIN_CASH_BANK_MST_Obj.CBM_FC_HOLD;
                            retval = Old_FIN_CASH_BANK_MST_Obj.CBM_PK ;
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

        public int DeleteFinCashBank(List<FIN_CASH_BANK_MST> finCashBankMstList)
        {
            int retval = 0;
            FIN_CASH_BANK_MST Old_FIN_CASH_BANK_MST_Obj;

            try
            {
                foreach (FIN_CASH_BANK_MST FIN_CASH_BANK_MST_Obj in finCashBankMstList)
                {
                    Old_FIN_CASH_BANK_MST_Obj = this.currentEntity.FIN_CASH_BANK_MST.SingleOrDefault(a => a.CBM_PK == FIN_CASH_BANK_MST_Obj.CBM_PK && a.CBM_MOD_DT == FIN_CASH_BANK_MST_Obj.CBM_MOD_DT);

                    if (Old_FIN_CASH_BANK_MST_Obj != null)
                    {
                        this.currentEntity.FIN_CASH_BANK_MST.DeleteObject(Old_FIN_CASH_BANK_MST_Obj);
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
        #endregion
    }
}
