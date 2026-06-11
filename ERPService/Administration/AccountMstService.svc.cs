using System;
using System.Collections.Generic;
using ERPData;
using System.Diagnostics;
using ERPManager;
using System.Data;

namespace ERPService.Administration
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AccountMstService" in code, svc and config file together.
    public class AccountMstService : IAccountMstService, IFinCoaMstManager
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// Constructor for AccountMstService Service
        /// </summary>
        public AccountMstService()
        {
            try
            {
                currentContext = new ERPEntities();
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }


        }

        public List<FIN_COA_MST> GetFinCoaMst(FIN_COA_MST FinCoaMstObj, ServiceUtility utilityObj)
        {
            FinCoaMstManager accountMstMgr;
            try
            {
                accountMstMgr = new FinCoaMstManager(currentContext);
                return accountMstMgr.GetFinCoaMst(FinCoaMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                accountMstMgr = null;
            }
        }

        public List<ERPManager.Administration.FinCOAMasterManager> GetFinancialCOAMst(FIN_COA_MST FinCoaMstObj, ServiceUtility utilityObj)
        {
            FinCoaMstManager accountMstMgr;
            try
            {
                accountMstMgr = new FinCoaMstManager(currentContext);
                return accountMstMgr.GetFinancialCOAMst(FinCoaMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                accountMstMgr = null;
            }
        }

        public List<FIN_COA_MST> GetFinCoaMstforGL(FIN_COA_MST FinCoaMstObj, ServiceUtility utilityObj)
        {
            FinCoaMstManager accountMstMgr;
            try
            {
                accountMstMgr = new FinCoaMstManager(currentContext);
                return accountMstMgr.GetFinCoaMstforGL(FinCoaMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                accountMstMgr = null;
            }
        }

        public int FinCoaBalanceSave(int coaPK, decimal coaAmount)
        {
            throw new NotImplementedException();
        }

        public int SaveAccountsMaster(List<FIN_COA_MST> accountMstList,int AddParentChildValidation)
        {
            FinCoaMstManager accountMstMgr;
            int? accountMstPK;
            try
            {
                accountMstMgr = new FinCoaMstManager(currentContext);
                accountMstPK = accountMstMgr.SaveAccountsMaster(accountMstList, AddParentChildValidation);
                if (accountMstPK > 0)
                    currentContext.SaveChanges();
                return accountMstPK.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                //throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
                Exception exp = ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
                if (exp.Message.Contains("COA_NAME"))
                    return -3; //Name Exists
                else
                    return -1; //Code Exists
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                accountMstMgr = null;
                accountMstPK = null;
            }
        }

        public int DeleteAccountsMaster(List<FIN_COA_MST> accountMstList)
        {
            FinCoaMstManager accountMstMgr;
            try
            {
                accountMstMgr = new FinCoaMstManager(currentContext);
                int result = accountMstMgr.DeleteAccountsMaster(accountMstList);
                currentContext.SaveChanges();
                return result;
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
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                accountMstMgr = null;
            }
        }

        public List<FIN_COA_MST> GetCoaMstAutoCompleteList(FIN_COA_MST finCoaMstObj, string voucherType, int accType, ServiceUtility utilityObj,int companyPK)
        {
            FinCoaMstManager accountMstMgr;
            try
            {
                accountMstMgr = new FinCoaMstManager(currentContext);
                return accountMstMgr.GetCoaMstAutoCompleteList(finCoaMstObj, voucherType, accType, utilityObj,companyPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                accountMstMgr = null;
            }
        }
        public List<FIN_COA_MST> GetAccountCoaMstAutoCompleteList(FIN_COA_MST finCoaMstObj, int accType, ServiceUtility utilityObj)
        {
            FinCoaMstManager accountMstMgr;
            try
            {
                accountMstMgr = new FinCoaMstManager(currentContext);
                return accountMstMgr.GetAccountCoaMstAutoCompleteList(finCoaMstObj, accType, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                accountMstMgr = null;
            }
        }
        public List<SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result> GetFinRptTemplateMst(SPFIN_REPORT_TEMPLATE_CFG_GET_KV_Result FinRptMstObj, ServiceUtility utilityObj)
        {
            FinCoaMstManager accountMstMgr;
            try
            {
                accountMstMgr = new FinCoaMstManager(currentContext);
                return accountMstMgr.GetFinRptTemplateMst(FinRptMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                accountMstMgr = null;
            }
        }
        public DataTable GetFinGroup(int pk, int active, int template, int group, int bizunit)
        {
            FinCoaMstManager accountMstMgr;
            try
            {
                accountMstMgr = new FinCoaMstManager(currentContext);
                return accountMstMgr.GetFinGroup(pk, active, template, group, bizunit);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                accountMstMgr = null;
            }
        }
        #endregion

        public List<DDLMaster> GetJournalAccountsAutoCompleteList(int accType, ServiceUtility utilityObj)
        {
            FinCoaMstManager accountMstMgr;
            try
            {
                accountMstMgr = new FinCoaMstManager(currentContext);
                return accountMstMgr.GetJournalAccountsAutoCompleteList(accType, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                accountMstMgr = null;
            }
        }

        public List<ADM_COST_CENTER_MST> GetADM_COST_CENTER_MST(ADM_COST_CENTER_MST ADM_COST_CENTER_MSTobj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetADM_COST_CENTER_MST(ADM_COST_CENTER_MSTobj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                commonFunMgr = null;
            }
        }
       
    }
}