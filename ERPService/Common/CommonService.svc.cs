using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPManager;
using System.Data;
using System.Diagnostics;
using ERPData;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CommonService" in code, svc and config file together.
    public class CommonService : ICommonService
    {
        #region Private Variables
        private ERPEntities currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// Common Service Constructor
        /// </summary>
        public CommonService()
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

        /// <summary>
        /// Executes Query and get DropdownList Data
        /// </summary>
        /// <param name="query">Query Object</param>
        /// <returns>List of DropdownList Data</returns>
        public List<DDLMaster> ExecuteQuery(string query)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.ExecuteQuery(query);
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
                commonFunctionsManagerObj = null;
            }
        }

        public List<TreeBinder> ExecuteTreeQuery(string query)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.ExecuteTreeQuery(query);
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
                commonFunctionsManagerObj = null;
            }
        }
        /// <summary>
        /// Executes Query and get string Result
        /// </summary>
        /// <param name="query">Query Object</param>
        /// <returns>Text Result</returns>
        public List<string> ExecuteTextQuery(string query)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.ExecuteTextQuery(query);
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
                commonFunctionsManagerObj = null;
            }
        }


        public bool CanShowTaxPopUp(long customerID,long brandItemMapID,string tabCode)
        {
            CommonFunctionsManager commonFunctionsManagerObj;
            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.CanShowTaxPopUp(customerID,brandItemMapID,tabCode);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Execute Stored Procedure
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="methodParams"></param>
        /// <returns></returns>
        public object ExecuteSP(string spName, object[] methodParams)
        {
            CommonFunctionsManager commonFunctionsManagerObj;
            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.ExecuteSP(spName, methodParams);
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
                commonFunctionsManagerObj = null;
            }
        }
       /// <summary>
        /// Gets Report Parameters
       /// </summary>
       /// <param name="aptCode"></param>
       /// <param name="astCode"></param>
       /// <param name="AppvdDate"></param>
       /// <param name="TrxCompanyPK"></param>
       /// <returns></returns>
        public List<SPADM_APP_SUB_TYPE_DATA_GET_Result> GetReportParameters(string aptCode, int astCode, DateTime AppvdDate, int? TrxCompanyPK = null)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetReportParameters(aptCode, astCode, AppvdDate, TrxCompanyPK);
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
                commonFunctionsManagerObj = null;
            }
        }

        /// <summary>
        /// Gets Customer
        /// </summary>
        /// <param name="aptPK"></param>
        /// <param name="astPK"></param>
        /// <returns></returns>
        public List<SPCRM_CUSTOMER_USER_GET_Result> GetCustomerDetails(int P_USER, int P_BIZUNIT)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetCustomerDetails(P_USER, P_BIZUNIT);
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
                commonFunctionsManagerObj = null;
            }
        }

        public List<ADM_CONFIG_MST> GetConfigValues(ADM_CONFIG_MST ADMCONFIGMSTObj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetConfigValues(ADMCONFIGMSTObj);
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

        public List<ADM_CONFIG_MST> GetConfigValues(ADM_CONFIG_MST ADMCONFIGMSTObj, byte[] Values)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetConfigValues(ADMCONFIGMSTObj, Values);
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

        public List<FIN_COA_SUB_TYPE_CFG> GetSubTypeCfgValues(FIN_COA_SUB_TYPE_CFG FINCOASUBTYPECFGObj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetSubTypeCfgValues(FINCOASUBTYPECFGObj);
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

        public string  GetSubTypeQuery(int SubTypePk)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetSubTypeQuery(SubTypePk);
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

        public List<ADM_APP_TYPE_MST> GetAppTypeValues(ADM_APP_TYPE_MST ADMAPPTYPEMSTObj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetAppTypeValues(ADMAPPTYPEMSTObj);
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

        public List<ADM_CONST_MST> GetConstMstValues(int? CON_PK, Int16? CON_ACTIVE, int? CON_GROUP, int? CGT_VALUE, int? CNG_VALUE, int? CON_BIZUNIT)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetConstMstValues(CON_PK, CON_ACTIVE, CON_GROUP, CGT_VALUE, CNG_VALUE, CON_BIZUNIT);
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


        public List<ADM_CONST_MST> GetConstMstAutoCompleteList(int? CON_PK, Int16? CON_ACTIVE, int? CON_GROUP, int? CGT_VALUE, int? CNG_VALUE, int? CON_BIZUNIT, ServiceUtility serviceUtilityObj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetConstMstAutoCompleteList(CON_PK, CON_ACTIVE, CON_GROUP, CGT_VALUE, CNG_VALUE, CON_BIZUNIT, serviceUtilityObj);
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

        public List<INV_ITEM_MST> GetInvItemMstAutoCompleteList(short? ITM_ACTIVE, ServiceUtility utilityObj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetInvItemMstAutoCompleteList(ITM_ACTIVE, utilityObj);
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

        public List<INV_ITEM_MST> GetInvItemMstTypeAutoCompleteList(short? ITM_ACTIVE, int type, ServiceUtility utilityObj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetInvItemMstTypeAutoCompleteList(ITM_ACTIVE, type, utilityObj);
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
        /// <summary>
        /// Gets Product Attributes
        /// </summary>
        /// <param name="aptPK"></param>
        /// <param name="astPK"></param>
        /// <returns></returns>
        public List<SPINV_ITEM_ATTRIBUTES_GET_LIST_Result> GetProductAttributes(int itemPK, int GrpType, int CNG_BIZUNIT,int active)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetProductAttributes(itemPK, GrpType, CNG_BIZUNIT, (byte)active);
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
                commonFunctionsManagerObj = null;
            }
        }

        /// <summary>
        /// Get Conversion Factor
        /// </summary>
        /// <param name="FromCurrency"></param>
        /// <param name="ToCurrency"></param>
        /// <param name="TrxDate"></param>
        /// <param name="BizUnit"></param>
        /// <returns></returns>
        public double GetConversionFactor(int FromCurrency, int ToCurrency, DateTime TrxDate, int BizUnit)
        {
            CommonFunctionsManager commonFunctionsMgr;
            try
            {
                commonFunctionsMgr = new CommonFunctionsManager(currentContext);
                return commonFunctionsMgr.GetConversionFactor(FromCurrency, ToCurrency, TrxDate, BizUnit);
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
                commonFunctionsMgr = null;
            }
        }

        /// <summary>
        /// Gets Control List
        /// </summary>
        /// <param name="aptPK"></param>
        /// <param name="astPK"></param>
        /// <returns></returns>
        public List<ADM_CONTROLS_CFG> GetControlsList(ADM_CONTROLS_CFG ADMCONTROLSCFGobj)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetControlsList(ADMCONTROLSCFGobj);
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
                commonFunctionsManagerObj = null;
            }
        }

        /// <summary>
        /// Gets Control List
        /// </summary>
        /// <param name="aptPK"></param>
        /// <param name="astPK"></param>
        /// <returns></returns>
        public List<ADM_CONST_GRP> GetConstGrpList(ADM_CONST_GRP ADMCONSTGRPobj)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetConstGrpList(ADMCONSTGRPobj);
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
                commonFunctionsManagerObj = null;
            }
        }


        /// <summary>
        /// Gets CheckListTrxHdr List
        /// </summary>
        /// <param name="ADMCHECKLISTTRXHDRobj"></param>
        public List<ADM_CHECK_LIST_TRX_HDR> GetCheckListTrxHdrList(ADM_CHECK_LIST_TRX_HDR ADMCHECKLISTTRXHDRobj)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetCheckListTrxHdrList(ADMCHECKLISTTRXHDRobj);
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
                commonFunctionsManagerObj = null;
            }
        }

        /// <summary>
        /// Save CheckListTrxHdr List
        /// </summary>
        /// <param name="ADMCHECKLISTTRXHDRobj"></param>
        public int SaveCheckListTrxHdr(List<ADM_CHECK_LIST_TRX_HDR> ADMCHECKLISTTRXHDRList)
        {
            CommonFunctionsManager commonFunctionsManagerObj;
            int? CheckListTrxHdrPK;
            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                CheckListTrxHdrPK = commonFunctionsManagerObj.SaveCheckListTrxHdr(ADMCHECKLISTTRXHDRList);
                currentContext.SaveChanges();
                return CheckListTrxHdrPK.Value;
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
                commonFunctionsManagerObj = null;
                CheckListTrxHdrPK = null;
            }
        }

        /// <summary>
        /// Gets CheckListTrxDtl List
        /// </summary>
        /// <param name="ADMCHECKLISTTRXDTLobj"></param>
        public List<ADM_CHECK_LIST_TRX_DTL> GetCheckListTrxDtlList(ADM_CHECK_LIST_TRX_DTL ADMCHECKLISTTRXDTLobj)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetCheckListTrxDtlList(ADMCHECKLISTTRXDTLobj);
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
                commonFunctionsManagerObj = null;
            }
        }

        /// <summary>
        /// Gets CheckListTrxDtl List
        /// </summary>
        /// <param name="ADMCHECKLISTTRXDTLobj"></param>
        public int SaveCheckListTrxDtl(List<ADM_CHECK_LIST_TRX_DTL> ADMCHECKLISTTRXDTLList, int TrxHdrPK)
        {
            CommonFunctionsManager commonFunctionsManagerObj;
            int? CheckListTrxdtlPK;
            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                CheckListTrxdtlPK = commonFunctionsManagerObj.SaveCheckListTrxDtl(ADMCHECKLISTTRXDTLList, TrxHdrPK);
                currentContext.SaveChanges();
                return CheckListTrxdtlPK.Value;
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
                commonFunctionsManagerObj = null;
                CheckListTrxdtlPK = null;
            }
        }

        public List<ADM_APP_TYPE_MST> GetADM_APP_TYPE_MST_Dtls(ADM_APP_SUB_TYPE_MST ADM_APP_SUB_TYPE_MSTobj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetADM_APP_TYPE_MST_Dtls(ADM_APP_SUB_TYPE_MSTobj);
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

        public List<ADM_COUNTRY_MST> GetCountry(ADM_COUNTRY_MST ADM_COUNTRY_MSTobj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetCountry(ADM_COUNTRY_MSTobj);
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

        public List<ADM_STATE_MST> GetStates(ADM_STATE_MST ADM_STATE_MSTobj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetStates(ADM_STATE_MSTobj);
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
        public List<ADM_APP_SUB_TYPE_MST> GetADMAPPSUBTYPEMST_Dtls(ADM_APP_SUB_TYPE_MST ADM_APP_SUB_TYPE_MSTobj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetADMAPPSUBTYPEMST_Dtls(ADM_APP_SUB_TYPE_MSTobj);
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

        public List<ADM_APP_CONFIG_MST> GetADM_APP_CONFIG_MST(ADM_APP_CONFIG_MST ADM_APP_CONFIG_MSTobj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetADM_APP_CONFIG_MST(ADM_APP_CONFIG_MSTobj);
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

        public string GetTrxDocNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK, int? CpmanyPk=null)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetTrxDocNo(aptCode, astVal, dept, date, user, update, appPK,CpmanyPk);
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

        public List<SAL_DESPATCH_HDR> GetGONHdr(int SPPk)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetGONHdr(SPPk);
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

        public List<SAL_ORDER_DTL> GetGONDtl(int SPPk)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetGONDtl(SPPk);
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



        public List<SpWkfTransactionNewCountGet_Result> GetMessageCount(int UserPK, byte? inboxType)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetMessageCount(UserPK, inboxType);
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
                    
        /// <summary>
        /// Get List of WorkFlow Status filtered by App Type and Subtype
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="appSubType"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<SPADM_APP_STATUS_CFG_GET_KV_Result> GetWorkFlowStatus(string appType, byte? appSubType, byte? status)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetWorkFlowStatus(appType, appSubType, status);
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                commonFunctionsManagerObj = null;
            }
        }

        /// <summary>
        /// Get List of WorkFlow Status filtered by App Type and Subtype
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="appSubType"></param>
        /// <param name="status"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<SPADM_APP_STATUS_CFG_GET_KV_Result> GetWorkFlowStatus(string appType, byte? appSubType, byte? status, ServiceUtility utilityObj)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetWorkFlowStatus(appType, appSubType, status, utilityObj);
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                commonFunctionsManagerObj = null;
            }
        }
        /// <summary>
        /// Get If Has Previous Trx in Different Process
        /// </summary>
        /// <param name="refID"></param>
        /// <returns></returns>
        public bool GetHasPreviousTrxDiffProcess(int refID)
        {
            CommonFunctionsManager commonFunctionsManagerObj;
            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetHasPreviousTrxDiffProcess(refID);
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                commonFunctionsManagerObj = null;
            }
        }


        public List<SPSAL_FORECAST_RPT_Result> GetSalesForecast(string xml, string Status, ServiceUtility utilityObj)
        {
            CommonFunctionsManager commonFunctionsManagerObj;
            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetSalesForecast(xml, Status, utilityObj);
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                commonFunctionsManagerObj = null;
            }
        }

        public List<INV_UOM_MST> GetUOM(int PK)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetUOM(PK);
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
                commonFunctionsManagerObj = null;
            }
        }


        public List<PUR_VENDOR_MST> GetVendor(int PK)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetVendor(PK);
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
                commonFunctionsManagerObj = null;
            }
        }

        public List<ADM_APP_CONFIG_MST> GetAlertNotify(ADM_APP_CONFIG_MST ADM_APP_CONFIG_MSTobj)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetAlertNotify(ADM_APP_CONFIG_MSTobj);
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
                commonFunctionsManagerObj = null;
            }
        }

        public List<ADM_CURRENCY_MST> GetCurrency(ADM_CURRENCY_MST ADMCURRENCYMSTObj)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetCurrency(ADMCURRENCYMSTObj);
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
                commonFunctionsManagerObj = null;
            }
        }

        public List<INV_ITEM_VENDOR_MAP> GetVendorItems(int venPk, int itemPk)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetVendorItems(venPk, itemPk);
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
                commonFunctionsManagerObj = null;
            }
        }

        public List<CRM_CUSTOMER_MST> GetCustomers(int cuspk)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetCustomers(cuspk);
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
                commonFunctionsManagerObj = null;
            }
        }
        public List<FIN_CASH_BANK_MST> GetCusBankDT(int cuspk, int? BankPk = null)
        {
            CommonFunctionsManager commonFunctionsManagerObj;

            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                return commonFunctionsManagerObj.GetCusBankDT(cuspk,BankPk);
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
                commonFunctionsManagerObj = null;
            }
        }

        /// <summary>
        /// Save Log
        /// </summary>
        /// <param name="AdmTrxLogList"></param>
        /// <returns></returns>
        public long? SaveLog(List<ADM_APP_TRX_LOG> AdmTrxLogList)
        {
            CommonFunctionsManager commonFunctionsManagerObj;
            long? LogPk;
            try
            {
                commonFunctionsManagerObj = new CommonFunctionsManager(currentContext);
                LogPk = commonFunctionsManagerObj.SaveLog(AdmTrxLogList);
                if (LogPk > 0)
                    currentContext.SaveChanges();
                return LogPk.Value;
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
                commonFunctionsManagerObj = null;
            }
        }
        
        
        #endregion

        /// <summary>
        /// Get All Items
        /// </summary>
        /// <param name="ITM_ACTIVE"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<INV_ITEM_MST> GetAllInvItemMstAutoCompleteList(short? ITM_ACTIVE, ServiceUtility utilityObj)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetAllInvItemMstAutoCompleteList(ITM_ACTIVE, utilityObj);
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

        public INV_ITEM_MST GetItemDetails(int ItemPk)
        {
            CommonFunctionsManager commonFunMgr;
            try
            {
                commonFunMgr = new CommonFunctionsManager(currentContext);
                return commonFunMgr.GetItemDetails(ItemPk);
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
