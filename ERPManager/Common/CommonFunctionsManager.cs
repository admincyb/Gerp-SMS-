using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ERPData;
using System.Data;
using ERPManager;
using System.Data.Objects;
using BusinessObject.CommonManagement;
using System.Reflection;
using System.Threading;

namespace ERPManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CommonFunctionsManager" in both code and config file together.
    public class CommonFunctionsManager : ICommonFunctionsManager
    {
        #region Private Variables
        /// <summary>
        /// Gets or sets current db context
        /// </summary>
        private ERPEntities currentEntity;
        #endregion

        #region Manager Methods

        /// <summary>
        /// Initializes a new instance of the InvoiceListManager class
        /// </summary>
        /// <param name="currentEntity">Current db context</param>
        public CommonFunctionsManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            catch (Exception ex)
            {
                // Handler for unknown exceptions
                // Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public string GetSubTypeQuery(int SubTypePk)
        {
            try
            {
                string Query = string.Empty;
                FIN_COA_SUB_TYPE_CFG finCoaSubTypeCfg=null;
                ADM_QUERIES_CFG admQueriesCfg=null;
                finCoaSubTypeCfg=this.currentEntity.FIN_COA_SUB_TYPE_CFG.SingleOrDefault(sub=>sub.CST_PK==SubTypePk);
                if (finCoaSubTypeCfg != null)
                {
                    admQueriesCfg = this.currentEntity.ADM_QUERIES_CFG.SingleOrDefault(qry => qry.QRY_PK == finCoaSubTypeCfg.CST_QUERY);
                    if (admQueriesCfg != null)
                    {
                        Query = admQueriesCfg.QRY_QUERY;
                    }
                }                
                return Query;
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

        public string GetTrxDocNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK, int? companyPk = null,int? bizunit=null)
        {
            try
            {
                string trxNo = string.Empty;
                List<SPADM_TRX_DOC_NO_GENERATE_Result> resultTrxNo;
                ObjectParameter paramReturn = new ObjectParameter("P_RET_VAL", typeof(int));
                ObjectParameter paramNextNo = new ObjectParameter("P_NEXT_NO", typeof(string));
                resultTrxNo = this.currentEntity.SPADM_TRX_DOC_NO_GENERATE(aptCode.ToString(), astVal, dept, companyPk,bizunit, date, user, update, appPK, paramReturn, paramNextNo).ToList();
                if (resultTrxNo != null && resultTrxNo.Count > 0)
                    trxNo = resultTrxNo.First().NEXT_NO;
                return trxNo;
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

        public double GetConversionFactor(int FromCurrency, int ToCurrency, DateTime TrxDate, int BizUnit)
        {
            try
            {
                TrxDate = TrxDate.Date;

                #region Configuration for Enable/Disable common CURRENCY master for all Bizunits
                try
                {
                    ADM_APP_CONFIG_MST objConfig = new ADM_APP_CONFIG_MST();
                    objConfig.ACF_SETTING = "BIZUNIT SETTINGS";
                    objConfig.ACF_DATA = "CURRENCY";
                    objConfig.ACF_ACTIVE = 1;
                    List<ADM_APP_CONFIG_MST> objConfigList = GetADM_APP_CONFIG_MST(objConfig).Where(cfg => cfg.ACF_DATA == objConfig.ACF_DATA).ToList();
                    if (objConfigList != null && objConfigList.Count() > 0)
                    {
                        if (objConfigList[0].ACF_VALUE == 1) // Enable common CURRENCY master for all Bizunits
                        {
                            BizUnit = 0;
                        }
                    }
                }
                catch { }
                #endregion
               

                double ExchangeRate = 1;

                if (FromCurrency == ToCurrency)
                    return ExchangeRate;
                else
                {
                    //Direct currency conv
                    ExchangeRate = ((from cuf in this.currentEntity.ADM_CURRENCY_CONV
                                     where TrxDate >= cuf.CUC_FROM_DATE
                                      && TrxDate <= cuf.CUC_TO_DATE
                                      && cuf.CUC_ACTIVE == 1
                                      && cuf.CUC_BIZUNIT == (BizUnit > 0 ? BizUnit : cuf.CUC_BIZUNIT)
                                      && cuf.CUC_FROM == FromCurrency
                                      && cuf.CUC_TO == ToCurrency
                                     select cuf
                              ).Max(pvh => (double?)pvh.CUC_CONV_FACT)) ?? 0;

                    if (ExchangeRate == 0)
                    {
                        //Reverse currency conv
                        ExchangeRate = ((from cuf in this.currentEntity.ADM_CURRENCY_CONV
                                         where TrxDate >= cuf.CUC_FROM_DATE
                                          && TrxDate <= cuf.CUC_TO_DATE
                                          && cuf.CUC_ACTIVE == 1
                                          && cuf.CUC_BIZUNIT == (BizUnit > 0 ? BizUnit : cuf.CUC_BIZUNIT)
                                          && cuf.CUC_FROM == ToCurrency
                                          && cuf.CUC_TO == FromCurrency
                                         select cuf
                              ).Max(pvh => (double?)pvh.CUC_CONV_FACT)) ?? -1;
                        ExchangeRate = 1 / ExchangeRate;
                    }
                    int exchRateDecimalDigits = (System.Web.HttpContext.Current.Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(System.Web.HttpContext.Current.Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));

                    ExchangeRate = Math.Round(ExchangeRate, exchRateDecimalDigits);

                    return ExchangeRate;
                }
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

        public List<DDLMaster> ExecuteQuery(string query)
        {
            try
            {
                var ddlresult = GetResults(new DDLMaster(), this.currentEntity, query);
                return ddlresult.ToList();
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

        public List<TreeBinder> ExecuteTreeQuery(string query)
        {
            try
            {
                var ddlresult = GetResults(new TreeBinder(), this.currentEntity, query);
                return ddlresult.ToList();
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

        public List<string> ExecuteTextQuery(string query)
        {
            try
            {
                var ddlresult = GetResults("", this.currentEntity, query);
                return ddlresult;
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

        /// <summary>
        /// Execute Stored Procedure
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="methodParams"></param>
        /// <returns></returns>
        public object ExecuteSP(string spName, object[] methodParams)
        {
            try
            {
                Object retObj = null;
                string namespaceString = "ERPData";
                string className = "ERPEntities";
                className = namespaceString + "." + className;
                Assembly currentAssembly = Assembly.Load(namespaceString);
                Type baseEntity = currentAssembly.GetType(className);
                Object entityObj = Activator.CreateInstance(baseEntity, null);
                MethodInfo spMethod = baseEntity.GetMethod(spName);
                retObj = spMethod.Invoke(entityObj, methodParams);
                return retObj;
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
        /// <summary>
        /// Get Config Values
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<ADM_CONFIG_MST> GetConfigValues(ADM_CONFIG_MST ADMCONFIGMSTObj)
        {
            IQueryable<ADM_CONFIG_MST> qry = null;

            try
            {
                //if (ADMCONFIGMSTObj.CET_PK > 0)
                //    qry = currentEntity.CAM_CONTRACT_EXP_TYPE_CFG.Where(cfg => cfg.CET_PK == ADMCONFIGMSTObj.CET_PK && cfg.CET_Active == ADMCONFIGMSTObj.CET_Active).AsQueryable();
                //else
                //    qry = currentEntity.CAM_CONTRACT_EXP_TYPE_CFG.Where(cfg => cfg.CET_Active == ADMCONFIGMSTObj.CET_Active).AsQueryable();

                qry = from cfg in this.currentEntity.ADM_CONFIG_MST
                      where cfg.CFG_ACTIVE == ADMCONFIGMSTObj.CFG_ACTIVE
                      select cfg;
                qry = from cfg in qry where cfg.CFG_PK == (ADMCONFIGMSTObj.CFG_PK > 0 ? ADMCONFIGMSTObj.CFG_PK : cfg.CFG_PK) select cfg;
                qry = from cfg in qry where cfg.CFG_TYPE == (!string.IsNullOrEmpty(ADMCONFIGMSTObj.CFG_TYPE) ? ADMCONFIGMSTObj.CFG_TYPE : cfg.CFG_TYPE) select cfg;
                qry = from cfg in qry where cfg.CFG_DATA == (!string.IsNullOrEmpty(ADMCONFIGMSTObj.CFG_DATA) ? ADMCONFIGMSTObj.CFG_DATA : cfg.CFG_DATA) select cfg;
                qry = from cfg in qry where cfg.CFG_VALUE == (ADMCONFIGMSTObj.CFG_VALUE > 0 ? ADMCONFIGMSTObj.CFG_VALUE : cfg.CFG_VALUE) select cfg;
                qry = from cfg in qry where string.IsNullOrEmpty(ADMCONFIGMSTObj.CFG_SPL_COND) ? true : cfg.CFG_SPL_COND.Contains(ADMCONFIGMSTObj.CFG_SPL_COND) select cfg;

                ServiceUtility utilityObj = new ServiceUtility();
                //utilityObj.SortBy = "CET_Sequence";
                //utilityObj.SortDirection = "asc";
                //utilityObj.CurrentPage = -1;
                //utilityObj.PageSize = -1;
                return qry.SortRecords<ADM_CONFIG_MST>(utilityObj).ToList();
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

        /// <summary>
        /// Get Config Values
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<ADM_CONFIG_MST> GetConfigValues(ADM_CONFIG_MST ADMCONFIGMSTObj, byte[] Values)
        {
            IQueryable<ADM_CONFIG_MST> qry = null;

            try
            {
                //if (ADMCONFIGMSTObj.CET_PK > 0)
                //    qry = currentEntity.CAM_CONTRACT_EXP_TYPE_CFG.Where(cfg => cfg.CET_PK == ADMCONFIGMSTObj.CET_PK && cfg.CET_Active == ADMCONFIGMSTObj.CET_Active).AsQueryable();
                //else
                //    qry = currentEntity.CAM_CONTRACT_EXP_TYPE_CFG.Where(cfg => cfg.CET_Active == ADMCONFIGMSTObj.CET_Active).AsQueryable();

                qry = from cfg in this.currentEntity.ADM_CONFIG_MST
                      where cfg.CFG_ACTIVE == ADMCONFIGMSTObj.CFG_ACTIVE
                      select cfg;
                qry = from cfg in qry where cfg.CFG_PK == (ADMCONFIGMSTObj.CFG_PK > 0 ? ADMCONFIGMSTObj.CFG_PK : cfg.CFG_PK) select cfg;
                qry = from cfg in qry where cfg.CFG_TYPE == (!string.IsNullOrEmpty(ADMCONFIGMSTObj.CFG_TYPE) ? ADMCONFIGMSTObj.CFG_TYPE : cfg.CFG_TYPE) select cfg;
                qry = from cfg in qry where cfg.CFG_DATA == (!string.IsNullOrEmpty(ADMCONFIGMSTObj.CFG_DATA) ? ADMCONFIGMSTObj.CFG_DATA : cfg.CFG_DATA) select cfg;
                qry = from cfg in qry where cfg.CFG_VALUE == (ADMCONFIGMSTObj.CFG_VALUE > 0 ? ADMCONFIGMSTObj.CFG_VALUE : cfg.CFG_VALUE) select cfg;
                qry = from cfg in qry where string.IsNullOrEmpty(ADMCONFIGMSTObj.CFG_SPL_COND) ? true : cfg.CFG_SPL_COND.Contains(ADMCONFIGMSTObj.CFG_SPL_COND) select cfg;
                qry = from cfg in qry where (Values.Count() <= 0) ? true : Values.Contains(cfg.CFG_VALUE) select cfg;
                ServiceUtility utilityObj = new ServiceUtility();
                //utilityObj.SortBy = "CET_Sequence";
                //utilityObj.SortDirection = "asc";
                //utilityObj.CurrentPage = -1;
                //utilityObj.PageSize = -1;
                return qry.SortRecords<ADM_CONFIG_MST>(utilityObj).ToList();
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

        /// <summary>
        /// Get SubType Cfg Values
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<FIN_COA_SUB_TYPE_CFG> GetSubTypeCfgValues(FIN_COA_SUB_TYPE_CFG FINCOASUBTYPECFGObj)
        {
            IQueryable<FIN_COA_SUB_TYPE_CFG> qry = null;

            try
            {
                ServiceUtility utilityObj = new ServiceUtility();
                qry=(from st in this.currentEntity.FIN_COA_SUB_TYPE_CFG
                     where st.CST_ACTIVE == FINCOASUBTYPECFGObj.CST_ACTIVE 
                        && st.CST_PK == (FINCOASUBTYPECFGObj.CST_PK >= 0 ? FINCOASUBTYPECFGObj.CST_PK : st.CST_PK)
                        orderby st.CST_NAME
                    select st);

                return qry.SortRecords<FIN_COA_SUB_TYPE_CFG>(utilityObj).ToList();
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

        /// <summary>
        /// Get AppType Values
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<ADM_APP_TYPE_MST> GetAppTypeValues(ADM_APP_TYPE_MST ADMCONFIGMSTObj)
        {
            IQueryable<ADM_APP_TYPE_MST> qry = null;

            try
            {
                qry = (from at in this.currentEntity.ADM_APP_TYPE_MST
                       where at.APT_ACTIVE == ADMCONFIGMSTObj.APT_ACTIVE
                          && at.APT_PK == (ADMCONFIGMSTObj.APT_PK > 0 ? ADMCONFIGMSTObj.APT_PK : at.APT_PK)
                          && at.APT_MODULE == (ADMCONFIGMSTObj.APT_MODULE > 0 ? ADMCONFIGMSTObj.APT_MODULE : at.APT_MODULE)
                          && string.IsNullOrEmpty(ADMCONFIGMSTObj.APT_SPL_COND) ? true : at.APT_SPL_COND.Contains(ADMCONFIGMSTObj.APT_SPL_COND)
                          orderby at.APT_CODE
                       select at);

                ServiceUtility utilityObj = new ServiceUtility();
                //utilityObj.SortBy = "CET_Sequence";
                //utilityObj.SortDirection = "asc";
                //utilityObj.CurrentPage = -1;
                //utilityObj.PageSize = -1;
                return qry.SortRecords<ADM_APP_TYPE_MST>(utilityObj).ToList();
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

      
        /// <summary>
        /// Gets Report Parameters
        /// </summary>
        /// <param name="aptPK"></param>
        /// <param name="astPK"></param>
        /// <returns></returns>
        public List<SPADM_APP_SUB_TYPE_DATA_GET_Result> GetReportParameters(string aptCode, int astCode, DateTime AppvdDate,int? TrxCompanyPK=null)
        {
            try
            {
                string trxNo = string.Empty;
                List<SPADM_APP_SUB_TYPE_DATA_GET_Result> resultAppType = null;
                ObjectParameter paramReturn = new ObjectParameter("P_RET_VAL", typeof(int));

                resultAppType = this.currentEntity.SPADM_APP_SUB_TYPE_DATA_GET(aptCode, astCode, AppvdDate, TrxCompanyPK).ToList();

                //if (resultTrxNo != null && resultTrxNo.Count > 0)
                //    trxNo = resultTrxNo.First().NEXT_NO;
                return resultAppType;
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

        /// <summary>
        /// Gets Customer 
        /// </summary>
        /// <param name="aptPK"></param>
        /// <param name="astPK"></param>
        /// <returns></returns>
        public List<SPCRM_CUSTOMER_USER_GET_Result> GetCustomerDetails(int P_USER, int P_BIZUNIT)
        {
            try
            {
                string trxNo = string.Empty;
                List<SPCRM_CUSTOMER_USER_GET_Result> resultCustomer = null;
                
                resultCustomer = this.currentEntity.SPCRM_CUSTOMER_USER_GET(P_USER,P_BIZUNIT).ToList();
                return resultCustomer;
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

        /// <summary>
        /// Gets Product Attributes
        /// </summary>
        /// <param name="aptPK"></param>
        /// <param name="astPK"></param>
        /// <returns></returns>
        public List<SPINV_ITEM_ATTRIBUTES_GET_LIST_Result> GetProductAttributes(int itemPK, int GrpType, int CNG_BIZUNIT, int active)
        {
            try
            {
                List<SPINV_ITEM_ATTRIBUTES_GET_LIST_Result> resultproductAttributes = null;
                resultproductAttributes = this.currentEntity.SPINV_ITEM_ATTRIBUTES_GET_LIST(itemPK, GrpType, CNG_BIZUNIT, (byte)active).ToList();
                return resultproductAttributes;
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



        public List<ADM_CONST_MST> GetConstMstValues(int? CON_PK, Int16? CON_ACTIVE, int? CON_GROUP, int? CGT_VALUE, int? CNG_VALUE, int? CON_BIZUNIT)
        {
            List<ADM_CONST_MST> Obj_ADM_CONST_MST_List;//= new List<ADM_CONST_MST>();
            //IQueryable<ADM_CONST_MST>   ADM_CONST_MST_Qry;

            try
            {
                Obj_ADM_CONST_MST_List = (from cst in this.currentEntity.ADM_CONST_MST
                                          where cst.CON_ACTIVE == (CON_ACTIVE == null ? cst.CON_ACTIVE : CON_ACTIVE)
                                                && cst.ADM_CONST_GRP.ADM_CONST_GRP_TYPE.CGT_ACTIVE == (CON_ACTIVE == null ? cst.ADM_CONST_GRP.ADM_CONST_GRP_TYPE.CGT_ACTIVE : CON_ACTIVE)
                                                && cst.ADM_CONST_GRP.CNG_ACTIVE == (CON_ACTIVE == null ? cst.ADM_CONST_GRP.CNG_ACTIVE : CON_ACTIVE)

                                                && cst.CON_PK == (CON_PK == null ? cst.CON_PK : CON_PK)
                                                && cst.CON_GROUP == (CON_GROUP == null ? cst.CON_GROUP : CON_GROUP)
                                                && cst.ADM_CONST_GRP.ADM_CONST_GRP_TYPE.CGT_VALUE == (CGT_VALUE == null ? cst.ADM_CONST_GRP.ADM_CONST_GRP_TYPE.CGT_VALUE : CGT_VALUE)
                                                && cst.ADM_CONST_GRP.CNG_VALUE == (CNG_VALUE == null ? cst.ADM_CONST_GRP.CNG_VALUE : CNG_VALUE)
                                                && cst.CON_BIZUNIT == (CON_BIZUNIT == null ? cst.CON_BIZUNIT : CON_BIZUNIT)
                                          select cst)
                                          .OrderBy(cst => cst.CON_NAME).ToList();
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

            return Obj_ADM_CONST_MST_List;
        }

        public List<ADM_CONTROLS_CFG> GetControlsList(ADM_CONTROLS_CFG ADMCONTROLSCFGobj)
        {
            List<ADM_CONTROLS_CFG> ADM_CONTROLS_CFG_Obj = new List<ADM_CONTROLS_CFG>();

            try
            {
                ADM_CONTROLS_CFG_Obj = (from   ctl in this.currentEntity.ADM_CONTROLS_CFG
                                        where  ctl.CTL_PK == (ADMCONTROLSCFGobj.CTL_PK > 0 ? ADMCONTROLSCFGobj.CTL_PK : ctl.CTL_PK)
                                               && ctl.CTL_ACTIVE == ADMCONTROLSCFGobj.CTL_ACTIVE
                                               && string.IsNullOrEmpty(ADMCONTROLSCFGobj.CTL_SPL_COND) ? true : ctl.CTL_SPL_COND.Contains(ADMCONTROLSCFGobj.CTL_SPL_COND)
                                        select ctl).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);

            }

            return ADM_CONTROLS_CFG_Obj;
        }

        public List<ADM_CONST_GRP> GetConstGrpList(ADM_CONST_GRP ADMCONSTGRPobj)
        {
            List<ADM_CONST_GRP> Obj_ADM_CONST_GRP_List;
            IQueryable<ADM_CONST_GRP> Obj_ADM_CONST_GRP_Qry;
            try
            {
                Obj_ADM_CONST_GRP_Qry = (from pac in this.currentEntity.ADM_CONST_GRP
                                         where pac.CNG_GRP_TYPE == ADMCONSTGRPobj.CNG_GRP_TYPE
                                               && pac.CNG_ACTIVE == (byte)1
                                         select pac);

                Obj_ADM_CONST_GRP_List = Obj_ADM_CONST_GRP_Qry.ToList();
            }
            catch (Exception ex)
            {

                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return Obj_ADM_CONST_GRP_List;
        }

        public List<ADM_CHECK_LIST_TRX_HDR> GetCheckListTrxHdrList(ADM_CHECK_LIST_TRX_HDR ADMCHECKLISTTRXHDRobj)
        {
            try {
                return (from clh in this.currentEntity.ADM_CHECK_LIST_TRX_HDR
                        where  clh.CLH_ACTIVE == ADMCHECKLISTTRXHDRobj.CLH_ACTIVE 
                            && clh.CLH_PK == (ADMCHECKLISTTRXHDRobj.CLH_PK > 0 ? ADMCHECKLISTTRXHDRobj.CLH_PK : clh.CLH_PK)
                            && clh.CLH_TRX_TYPE == (!string.IsNullOrEmpty(ADMCHECKLISTTRXHDRobj.CLH_TRX_TYPE) ? ADMCHECKLISTTRXHDRobj.CLH_TRX_TYPE : clh.CLH_TRX_TYPE)
                            && clh.CLH_TRX_PK == (ADMCHECKLISTTRXHDRobj.CLH_TRX_PK > 0 ? ADMCHECKLISTTRXHDRobj.CLH_TRX_PK : clh.CLH_TRX_PK)
                                    
                        select clh).ToList();
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

        public int SaveCheckListTrxHdr(List<ADM_CHECK_LIST_TRX_HDR> ADMCHECKLISTTRXHDRList)
        {
            int retval = 0;
            int? max_PK;
            ADM_CHECK_LIST_TRX_HDR Old_ADM_CHECK_LIST_TRX_HDR;
            List<ADM_CHECK_LIST_TRX_DTL> ADM_CHECK_LIST_TRX_DTL_Obj;

            try
            {
                foreach (ADM_CHECK_LIST_TRX_HDR Obj_ADM_CHECK_LIST_TRX_HDR in ADMCHECKLISTTRXHDRList)
                {
                    ADM_CHECK_LIST_TRX_DTL_Obj = Obj_ADM_CHECK_LIST_TRX_HDR.ADM_CHECK_LIST_TRX_DTL.ToList();
                    Obj_ADM_CHECK_LIST_TRX_HDR.ADM_CHECK_LIST_TRX_DTL.Clear();

                    if (Obj_ADM_CHECK_LIST_TRX_HDR.CLH_PK == 0)
                    {
                        // Gets last SAL_DESPATCH_HDR pk
                        max_PK = this.currentEntity.ADM_CHECK_LIST_TRX_HDR.Max(dph => (int?)dph.CLH_PK);

                        // Sets return value as next SAL_DESPATCH_HDR pk
                        retval = (max_PK.HasValue ? max_PK.Value + 1 : 1);

                        Obj_ADM_CHECK_LIST_TRX_HDR.CLH_PK  = retval;
                        Obj_ADM_CHECK_LIST_TRX_HDR.CLH_CRTD_DT  = DateTime.Now;
                        Obj_ADM_CHECK_LIST_TRX_HDR.CLH_MOD_DT   = DateTime.Now;

                        // Add new SAL_DESPATCH_HDR to the db context
                        this.currentEntity.ADM_CHECK_LIST_TRX_HDR.AddObject(Obj_ADM_CHECK_LIST_TRX_HDR);
                        
                        SaveCheckListTrxDtl(ADM_CHECK_LIST_TRX_DTL_Obj, retval);
                    }
                    else
                    {
                        Old_ADM_CHECK_LIST_TRX_HDR = currentEntity.ADM_CHECK_LIST_TRX_HDR.SingleOrDefault(dph => dph.CLH_PK == Obj_ADM_CHECK_LIST_TRX_HDR.CLH_PK );

                        if (Old_ADM_CHECK_LIST_TRX_HDR != null)
                        {
                            Old_ADM_CHECK_LIST_TRX_HDR.CLH_NO = Obj_ADM_CHECK_LIST_TRX_HDR.CLH_NO;
                            Old_ADM_CHECK_LIST_TRX_HDR.CLH_DATE = Obj_ADM_CHECK_LIST_TRX_HDR.CLH_DATE;
                            Old_ADM_CHECK_LIST_TRX_HDR.CLH_TRX_TYPE = Obj_ADM_CHECK_LIST_TRX_HDR.CLH_TRX_TYPE;
                            Old_ADM_CHECK_LIST_TRX_HDR.CLH_TRX_PK = Obj_ADM_CHECK_LIST_TRX_HDR.CLH_TRX_PK;
                            Old_ADM_CHECK_LIST_TRX_HDR.CLH_CHECK_LIST_GROUP = Obj_ADM_CHECK_LIST_TRX_HDR.CLH_CHECK_LIST_GROUP;
                            Old_ADM_CHECK_LIST_TRX_HDR.CLH_DESC = Obj_ADM_CHECK_LIST_TRX_HDR.CLH_DESC;
                            Old_ADM_CHECK_LIST_TRX_HDR.CLH_ACTIVE = Obj_ADM_CHECK_LIST_TRX_HDR.CLH_ACTIVE;
                            Old_ADM_CHECK_LIST_TRX_HDR.CLH_IS_DELETED = Obj_ADM_CHECK_LIST_TRX_HDR.CLH_IS_DELETED;
                            Old_ADM_CHECK_LIST_TRX_HDR.CLH_BIZUNIT = Obj_ADM_CHECK_LIST_TRX_HDR.CLH_BIZUNIT;
                            Old_ADM_CHECK_LIST_TRX_HDR.CLH_MOD_BY = Obj_ADM_CHECK_LIST_TRX_HDR.CLH_MOD_BY;
                            Old_ADM_CHECK_LIST_TRX_HDR.CLH_DATE = DateTime.Now;

                            retval = Obj_ADM_CHECK_LIST_TRX_HDR.CLH_PK;

                            //FinInvoiceVndTrxMpgManagerObj = new FinInvoiceVndTrxMpgManager(this.currentEntity);
                            SaveCheckListTrxDtl(ADM_CHECK_LIST_TRX_DTL_Obj, retval);
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }
                }

                //return Despatch Hdr Pk
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

        public List<ADM_CHECK_LIST_TRX_DTL> GetCheckListTrxDtlList(ADM_CHECK_LIST_TRX_DTL ADMCHECKLISTTRXDTLobj)
        {
            try
            {
                return (from cld in this.currentEntity.ADM_CHECK_LIST_TRX_DTL
                        where cld.CLD_ACTIVE == ADMCHECKLISTTRXDTLobj.CLD_ACTIVE 
                            &&  cld.CLD_PK == (ADMCHECKLISTTRXDTLobj.CLD_PK > 0 ? ADMCHECKLISTTRXDTLobj.CLD_PK : cld.CLD_PK)
                            && cld.CLD_TRX_HDR == (ADMCHECKLISTTRXDTLobj.CLD_TRX_HDR > 0 ? ADMCHECKLISTTRXDTLobj.CLD_TRX_HDR : cld.CLD_TRX_HDR)
                        select cld).ToList();
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

        public int SaveCheckListTrxDtl(List<ADM_CHECK_LIST_TRX_DTL> ADMCHECKLISTTRXDTLList, int TrxHdrPK)
        {
            int retval = 0;
            long? maxPK;
           
            //double DespatchQty = 0;
            ADM_CHECK_LIST_TRX_DTL OLD_ADM_CHECK_LIST_TRX_DTL_Obj;
            try
            {
                //if (ADMCHECKLISTTRXDTLList == null || ADMCHECKLISTTRXDTLList.Count == 0)
                //{
                //    List<ADM_CHECK_LIST_TRX_DTL> objChecklst = this.currentEntity.ADM_CHECK_LIST_TRX_DTL.Where(pk => pk.CLD_TRX_HDR == TrxHdrPK).ToList();
                //    if (objChecklst != null && objChecklst.Count > 0)
                //    {
                //        foreach (ADM_CHECK_LIST_TRX_DTL objDtl in objChecklst)
                //        {
                //            ADM_CHECK_LIST_TRX_DTL objlst = this.currentEntity.ADM_CHECK_LIST_TRX_DTL.SingleOrDefault(pk => pk.CLD_PK == objDtl.CLD_PK);
                //            this.currentEntity.DeleteObject(objlst);
                //        }
                //        retval = objChecklst[0].CLD_PK;
                //    }
                //    else
                //    {
                //        retval = TrxHdrPK;
                //    }                    
                //}
                //else
                //{

                    if (ADMCHECKLISTTRXDTLList != null && ADMCHECKLISTTRXDTLList.Count > 0)
                    {
                        List<int> pks = (from old1 in ADMCHECKLISTTRXDTLList
                                          select old1.CLD_PK).ToList();

                        List<ADM_CHECK_LIST_TRX_DTL> objChecklst = this.currentEntity.ADM_CHECK_LIST_TRX_DTL.Where(pk => pk.CLD_TRX_HDR == TrxHdrPK && !pks.Contains(pk.CLD_PK)).ToList();
                        if (objChecklst != null && objChecklst.Count > 0)
                        {
                            foreach (ADM_CHECK_LIST_TRX_DTL objDtl in objChecklst)
                            {
                                ADM_CHECK_LIST_TRX_DTL objlst = this.currentEntity.ADM_CHECK_LIST_TRX_DTL.SingleOrDefault(pk => pk.CLD_PK == objDtl.CLD_PK);
                                this.currentEntity.DeleteObject(objlst);
                            }
                        }                    
                    }


                    ////Set save status zero,save failed
                    retval = 0;
                    ////Getting last Payment Mpg pk
                    maxPK = currentEntity.ADM_CHECK_LIST_TRX_DTL.Max(v => (int?)v.CLD_PK);
                    maxPK = (maxPK.HasValue) ? maxPK.Value + 1 : 1;

                    foreach (ADM_CHECK_LIST_TRX_DTL ADM_CHECK_LIST_TRX_DTL_Obj in ADMCHECKLISTTRXDTLList)
                    {
                        //check despatch pk is zero,save despatch as new record
                        if (ADM_CHECK_LIST_TRX_DTL_Obj.CLD_PK == 0)
                        {
                            //Set next despatch pk
                            ADM_CHECK_LIST_TRX_DTL_Obj.CLD_PK = (Int32)maxPK;
                            ADM_CHECK_LIST_TRX_DTL_Obj.CLD_MOD_DT = DateTime.Now;

                            //Add new despatch to the db context
                            currentEntity.ADM_CHECK_LIST_TRX_DTL.AddObject(ADM_CHECK_LIST_TRX_DTL_Obj);
                            maxPK++;
                            retval = ADM_CHECK_LIST_TRX_DTL_Obj.CLD_PK;

                            //DespatchQty = ADM_CHECK_LIST_TRX_DTL_Obj.DPD_QTY_DESPATCHED;
                        }
                        //updating despatch details
                        else
                        {
                            //Get current despatch details using despatch  pk
                            OLD_ADM_CHECK_LIST_TRX_DTL_Obj = currentEntity.ADM_CHECK_LIST_TRX_DTL.SingleOrDefault(v => v.CLD_PK == ADM_CHECK_LIST_TRX_DTL_Obj.CLD_PK);
                            if (OLD_ADM_CHECK_LIST_TRX_DTL_Obj != null)
                            {
                                //DespatchQty = ADM_CHECK_LIST_TRX_DTL_Obj.DPD_QTY_DESPATCHED - OLD_ADM_CHECK_LIST_TRX_DTL_Obj.DPD_QTY_DESPATCHED;

                                //Update despatch details
                                OLD_ADM_CHECK_LIST_TRX_DTL_Obj.CLD_TRX_HDR = ADM_CHECK_LIST_TRX_DTL_Obj.CLD_TRX_HDR;
                                OLD_ADM_CHECK_LIST_TRX_DTL_Obj.CLD_CHECK_LIST_ITEM = ADM_CHECK_LIST_TRX_DTL_Obj.CLD_CHECK_LIST_ITEM;
                                OLD_ADM_CHECK_LIST_TRX_DTL_Obj.CLD_CHECK_LIST_VALUE = ADM_CHECK_LIST_TRX_DTL_Obj.CLD_CHECK_LIST_VALUE;
                                OLD_ADM_CHECK_LIST_TRX_DTL_Obj.CLD_LOV_ITEM = ADM_CHECK_LIST_TRX_DTL_Obj.CLD_LOV_ITEM;
                                OLD_ADM_CHECK_LIST_TRX_DTL_Obj.CLD_DESC = ADM_CHECK_LIST_TRX_DTL_Obj.CLD_DESC;
                                OLD_ADM_CHECK_LIST_TRX_DTL_Obj.CLD_ACTIVE = ADM_CHECK_LIST_TRX_DTL_Obj.CLD_ACTIVE;
                                OLD_ADM_CHECK_LIST_TRX_DTL_Obj.CLD_MOD_BY = ADM_CHECK_LIST_TRX_DTL_Obj.CLD_MOD_BY;
                                OLD_ADM_CHECK_LIST_TRX_DTL_Obj.CLD_MOD_DT = DateTime.Now;

                                //Set return value as despatch pk
                                retval = ADM_CHECK_LIST_TRX_DTL_Obj.CLD_PK;
                            }
                            else
                            {
                                //throws exception already deleted or modified by other user
                                //throw new OptimisticConcurrencyException(gComsManagerRes.EditConcurrencyException);
                            }
                        }


                    }
                //}

                //return Invoice Trx Mpg Pk
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
        public List<FIN_CASH_BANK_MST> GetCusBankDT(int cuspk,int? BankPk = null)
        {
            List<FIN_CASH_BANK_MST> CRM_CUSTOMER_MSTLst = new List<FIN_CASH_BANK_MST>();
            try
            {
                //CRM_CUSTOMER_MSTLst = (from B in this.currentEntity.FIN_CASH_BANK_MST
                //                       join C in this.currentEntity.CRM_CUSTOMER_MST on B.CBM_PK equals C.CUS_BANK
                //                       where C.CUS_PK == cuspk
                //                       select B).ToList();
                CRM_CUSTOMER_MSTLst = (from B in this.currentEntity.FIN_CASH_BANK_MST
                                       join C in this.currentEntity.CRM_CUSTOMER_MST on B.CBM_PK equals C.CUS_BANK
                                       where C.CUS_PK == cuspk
                                       && (BankPk.HasValue ? B.CBM_PK == BankPk : B.CBM_ACTIVE == (int)DbActiveStatus.ACTIVE)
                                       select B).ToList();

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

            return CRM_CUSTOMER_MSTLst;
            //throw new NotImplementedException();
        }
        public List<ADM_APP_TYPE_MST> GetADM_APP_TYPE_MST_Dtls(ADM_APP_SUB_TYPE_MST ADM_APP_SUB_TYPE_MSTobj)
        {
            List<ADM_APP_TYPE_MST> ADM_APP_TYPE_MSTLst = new List<ADM_APP_TYPE_MST>();
            try
            {
                ADM_APP_TYPE_MSTLst = (from dpd in this.currentEntity.ADM_APP_TYPE_MST
                                       join sod in this.currentEntity.ADM_APP_SUB_TYPE_MST on dpd.APT_PK equals sod.AST_APP_TYPE
                                       where sod.AST_SPL_COND == ADM_APP_SUB_TYPE_MSTobj.AST_SPL_COND
                                       select dpd).ToList();

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

            return ADM_APP_TYPE_MSTLst;
            //throw new NotImplementedException();
        }

        public List<ADM_APP_SUB_TYPE_MST> GetADMAPPSUBTYPEMST_Dtls(ADM_APP_SUB_TYPE_MST ADM_APP_SUB_TYPE_MSTobj)
        {
            List<ADM_APP_SUB_TYPE_MST> ADM_APP_SUB_TYPE_MSTLst = new List<ADM_APP_SUB_TYPE_MST>();
            try
            {
                ADM_APP_SUB_TYPE_MSTLst=(from st in this.currentEntity.ADM_APP_SUB_TYPE_MST
                                         where st.ADM_APP_TYPE_MST.APT_CODE == (ADM_APP_SUB_TYPE_MSTobj.ADM_APP_TYPE_MST.APT_CODE !=string.Empty ?ADM_APP_SUB_TYPE_MSTobj.ADM_APP_TYPE_MST.APT_CODE :st.ADM_APP_TYPE_MST.APT_CODE)
                                         && st.AST_PK == (ADM_APP_SUB_TYPE_MSTobj.AST_PK > 0 ? ADM_APP_SUB_TYPE_MSTobj.AST_PK : st.AST_PK)
                                         && st.AST_ACTIVE == 1
                                         && st.AST_SPL_COND.Contains(ADM_APP_SUB_TYPE_MSTobj.AST_SPL_COND) 
                                         select st).ToList();
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

            return ADM_APP_SUB_TYPE_MSTLst;
            //throw new NotImplementedException();
        }

        public List<ADM_COUNTRY_MST> GetCountry(ADM_COUNTRY_MST ADM_COUNTRY_MSTobj)
        {
            List<ADM_COUNTRY_MST > ADM_COUNTRY_MSTLst = new List<ADM_COUNTRY_MST >();
            try
            {
                ADM_COUNTRY_MSTLst = (from cty in this.currentEntity.ADM_COUNTRY_MST
                                      where cty.CNT_ACTIVE == ADM_COUNTRY_MSTobj.CNT_ACTIVE
                                      select cty).OrderBy(cty => cty.CNT_NAME).ToList();

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

            return ADM_COUNTRY_MSTLst;
            //throw new NotImplementedException();
        }
        public List<ADM_STATE_MST> GetStates(ADM_STATE_MST ADM_STATE_MSTobj)
        {
            List<ADM_STATE_MST> ADM_STATE_MSTLst = new List<ADM_STATE_MST>();
            try
            {
                ADM_STATE_MSTLst = (from cty in this.currentEntity.ADM_STATE_MST
                                      where cty.STT_ACTIVE == ADM_STATE_MSTobj.STT_ACTIVE
                                            && cty.STT_COUNTRY == ADM_STATE_MSTobj.STT_COUNTRY
                                    select cty).OrderBy(cty => cty.STT_NAME).ToList();

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

            return ADM_STATE_MSTLst;
            //throw new NotImplementedException();
        }

        public List<ADM_APP_CONFIG_MST> GetADM_APP_CONFIG_MST(ADM_APP_CONFIG_MST ADM_APP_CONFIG_MSTobj)
        {
            List<ADM_APP_CONFIG_MST> ADM_APP_CONFIG_MSTLst = new List<ADM_APP_CONFIG_MST>();
            try
            {
                ADM_APP_CONFIG_MSTLst = (from config in this.currentEntity.ADM_APP_CONFIG_MST
                                         where config.ACF_ACTIVE == ADM_APP_CONFIG_MSTobj.ACF_ACTIVE
                                          && (config.ACF_PK == ADM_APP_CONFIG_MSTobj.ACF_PK || config.ACF_SETTING.Contains(ADM_APP_CONFIG_MSTobj.ACF_SETTING))
                                         select config).OrderBy(cty => cty.ACF_SETTING).ToList();

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

            return ADM_APP_CONFIG_MSTLst;
            //throw new NotImplementedException();
        }

        public List<SAL_DESPATCH_HDR > GetGONHdr(int SPPk)
        {
            List<SAL_DESPATCH_HDR> SAL_DESPATCH_HDRLst = new List<SAL_DESPATCH_HDR>();
            try
            {
                SAL_DESPATCH_HDRLst = (from cty in this.currentEntity.SAL_DESPATCH_HDR
                                       where cty.DPH_SHIPPING_PLAN == SPPk
                                      select cty).ToList();

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

            return SAL_DESPATCH_HDRLst;
            //throw new NotImplementedException();
        }



        public List<SAL_ORDER_DTL> GetGONDtl(int SPPk)
        {
            List<SAL_ORDER_DTL> SAL_DESPATCH_DTLLst = new List<SAL_ORDER_DTL>();
            try
            {
                SAL_DESPATCH_DTLLst = (from cty in this.currentEntity.SAL_ORDER_DTL
                                       join city in this.currentEntity.SAL_SHIPPING_PLAN_DTL on cty.SOD_PK equals city.SND_SOD
                                       where city.SND_PLAN_HDR == SPPk
                                       select cty).ToList();

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

            return SAL_DESPATCH_DTLLst;
            //throw new NotImplementedException();
        }


        /// <summary>
        /// Gets Message count
        /// </summary>
        /// <param name="aptPK"></param>
        /// <param name="astPK"></param>
        /// <returns></returns>
        public List<SpWkfTransactionNewCountGet_Result> GetMessageCount(int UserPK,byte? inboxType)
        {
            try
            {
                string trxNo = string.Empty;
                List<SpWkfTransactionNewCountGet_Result> resultAppType = null;


                resultAppType = this.currentEntity.SpWkfTransactionNewCountGet(UserPK, inboxType).ToList();

                //if (resultTrxNo != null && resultTrxNo.Count > 0)
                //    trxNo = resultTrxNo.First().NEXT_NO;
                return resultAppType;
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

        #region Private Methods
        /// <summary>
        /// Executes Query and get result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">Type Argument</param>
        /// <param name="ctx">Current Entity Context</param>
        /// <param name="cmd">SP Name</param>
        /// <returns>List of Type T Objects</returns>
        private List<T> GetResults<T>(T element, ObjectContext ctx, string cmd)
        {
            // List of T elements to be returned.
            List<T> results = null;

            // Execute the query
            if (string.IsNullOrEmpty(cmd))
            {
                results = null;
            }
            else
            {
                results = ctx.ExecuteStoreQuery<T>(cmd, null).ToList();
            }
            // Return the results
            return results;
        }
        #endregion


        public bool CanShowTaxPopUp(long customerID, long brandItemMapID, string tabCode)
        {
            bool result = true;
            if (customerID == 0) return result;
            else if (tabCode == TabType.CIM)
            {
                if (brandItemMapID == 0)
                {
                    var cust=this.currentEntity
                                .CRM_CUSTOMER_MST
                                .SingleOrDefault(x => x.CUS_PK == customerID);

                   // result = (!cust.CRM_CUST_ITEM_MAP.Any()) && (!cust.CRM_CUST_TAX_DTL.Any());
                   result = !cust.CRM_CUST_TAX_DTL.Any();
                }
                else
                {
                    result = !this.currentEntity
                                .CRM_CUSTOMER_MST
                                .SingleOrDefault(x => x.CUS_PK == customerID)
                                .CRM_CUST_TAX_DTL.Any();
                }
            }
            else if (tabCode == TabType.CUS)
            {
                var itmMap = this.currentEntity
                            .CRM_CUSTOMER_MST
                            .SingleOrDefault(x => x.CUS_PK == customerID).CRM_CUST_ITEM_MAP.AsQueryable();
                if (itmMap.Any())
                {
                    foreach (var dtl in itmMap.ToList())
                    {                        
                        if (dtl.CRM_CUST_TAX_DTL.Any())
                        {
                            result = false; 
                            break;
                        }
                    }
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

        public List<ADM_CONST_MST> GetConstMstAutoCompleteList(int? CON_PK, short? CON_ACTIVE, int? CON_GROUP, int? CGT_VALUE, int? CNG_VALUE, int? CON_BIZUNIT, ServiceUtility serviceUtilityObj)
        {
            List<ADM_CONST_MST> Obj_ADM_CONST_MST_List;//= new List<ADM_CONST_MST>();
            //IQueryable<ADM_CONST_MST>   ADM_CONST_MST_Qry;

            try
            {
                Obj_ADM_CONST_MST_List = (from cst in this.currentEntity.ADM_CONST_MST
                                          where cst.CON_ACTIVE == (CON_ACTIVE == null ? cst.CON_ACTIVE : CON_ACTIVE)
                                                && cst.ADM_CONST_GRP.ADM_CONST_GRP_TYPE.CGT_ACTIVE == (CON_ACTIVE == null ? cst.ADM_CONST_GRP.ADM_CONST_GRP_TYPE.CGT_ACTIVE : CON_ACTIVE)
                                                && cst.ADM_CONST_GRP.CNG_ACTIVE == (CON_ACTIVE == null ? cst.ADM_CONST_GRP.CNG_ACTIVE : CON_ACTIVE)
                                                && cst.CON_PK == (CON_PK == null ? cst.CON_PK : CON_PK)
                                                && cst.CON_GROUP == (CON_GROUP == null ? cst.CON_GROUP : CON_GROUP)
                                                && cst.ADM_CONST_GRP.ADM_CONST_GRP_TYPE.CGT_VALUE == (CGT_VALUE == null ? cst.ADM_CONST_GRP.ADM_CONST_GRP_TYPE.CGT_VALUE : CGT_VALUE)
                                                && cst.ADM_CONST_GRP.CNG_VALUE == (CNG_VALUE == null ? cst.ADM_CONST_GRP.CNG_VALUE : CNG_VALUE)
                                                && cst.CON_BIZUNIT == (CON_BIZUNIT == null ? cst.CON_BIZUNIT : CON_BIZUNIT)
                                                && cst.CON_NAME.StartsWith(serviceUtilityObj.FilterValue)
                                          orderby cst.CON_VALUE
                                          select cst).ToList();
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

            return Obj_ADM_CONST_MST_List;
        }


        public List<INV_ITEM_MST> GetInvItemMstAutoCompleteList(short? ITM_ACTIVE, ServiceUtility utilityObj)
        {
            int? invCodePK = null;
            INV_ITEM_CATEGORY invItemCategoryObj = this.currentEntity.INV_ITEM_CATEGORY.SingleOrDefault(itm => itm.ITC_CODE == "FG");
            if (invItemCategoryObj != null)
            {
                invCodePK = invItemCategoryObj.ITC_PK;

            }

            List<INV_ITEM_MST> INV_ITEM_MST_List_Obj = new List<INV_ITEM_MST>();
            try
            {
                if (utilityObj.FilterBy == DataFieldRes.ItemCode)
                {
                    INV_ITEM_MST_List_Obj = (from inv in this.currentEntity.INV_ITEM_MST
                                             where inv.ITM_ACTIVE == ITM_ACTIVE
                                                   && inv.ITM_CODE.StartsWith(utilityObj.FilterValue)
                                                   && inv.ITM_CATEGORY == (invCodePK.HasValue ? invCodePK : inv.ITM_CATEGORY)
                                             select inv).ToList();
                }
                else if (utilityObj.FilterBy == DataFieldRes.ItemName)
                {
                    INV_ITEM_MST_List_Obj = (from inv in this.currentEntity.INV_ITEM_MST
                                             where inv.ITM_ACTIVE == ITM_ACTIVE
                                                   && inv.ITM_NAME.StartsWith(utilityObj.FilterValue)
                                                   && inv.ITM_CATEGORY == (invCodePK.HasValue ? invCodePK : inv.ITM_CATEGORY)
                                             select inv).ToList();
                }

            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return INV_ITEM_MST_List_Obj;
        }


        public List<INV_ITEM_MST> GetInvItemMstTypeAutoCompleteList(short? ITM_ACTIVE,int type, ServiceUtility utilityObj)
        {
            int? invCodePK = 0;
            INV_ITEM_CATEGORY invItemCategoryObj = this.currentEntity.INV_ITEM_CATEGORY.SingleOrDefault(itm => itm.ITC_VALUE == type);
            if (invItemCategoryObj != null)
            {
                invCodePK = invItemCategoryObj.ITC_PK;

            }

            List<INV_ITEM_MST> INV_ITEM_MST_List_Obj = new List<INV_ITEM_MST>();
            try
            {
                if (utilityObj.FilterBy == DataFieldRes.ItemCode)
                {
                    INV_ITEM_MST_List_Obj = (from inv in this.currentEntity.INV_ITEM_MST
                                             where inv.ITM_ACTIVE == ITM_ACTIVE
                                                   && inv.ITM_CODE.StartsWith(utilityObj.FilterValue)
                                                   && inv.ITM_CATEGORY == invCodePK
                                             select inv).ToList();
                }
                else if (utilityObj.FilterBy == DataFieldRes.ItemName)
                {
                    INV_ITEM_MST_List_Obj = (from inv in this.currentEntity.INV_ITEM_MST
                                             where inv.ITM_ACTIVE == ITM_ACTIVE
                                                   && inv.ITM_NAME.StartsWith(utilityObj.FilterValue)
                                                   && inv.ITM_CATEGORY == invCodePK
                                             select inv).ToList();
                }
                else
                {
                    INV_ITEM_MST_List_Obj = (from inv in this.currentEntity.INV_ITEM_MST
                                             where inv.ITM_ACTIVE == ITM_ACTIVE
                                                   && (inv.ITM_NAME.Contains(utilityObj.FilterValue) || inv.ITM_CODE.Contains(utilityObj.FilterValue))
                                                   && inv.ITM_CATEGORY == invCodePK
                                             select inv).ToList();
                }

            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return INV_ITEM_MST_List_Obj;
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
            List<SPADM_APP_STATUS_CFG_GET_KV_Result> resultAppStatus;
            try
            {
                resultAppStatus = this.currentEntity.SPADM_APP_STATUS_CFG_GET_KV(appType, appSubType, status).ToList();
                resultAppStatus = resultAppStatus.OrderBy(ws => ws.ASC_NAME).ToList(); 
                return resultAppStatus;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
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
            List<SPADM_APP_STATUS_CFG_GET_KV_Result> resultAppStatus;
            try
            {
                resultAppStatus = this.currentEntity.SPADM_APP_STATUS_CFG_GET_KV(appType, appSubType, status).ToList();
                if (utilityObj.SortBy == DataFieldRes.AppStatusName)
                {
                    resultAppStatus = resultAppStatus.OrderBy(ws => ws.ASC_NAME).ToList();
                }
                else
                {
                    resultAppStatus = resultAppStatus.OrderBy(ws => ws.ASC_SEQUENCE).ToList();
                }
                return resultAppStatus;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
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

        /// <summary>
        /// Get If Has Previous Trx in Different Process
        /// </summary>
        /// <param name="refID"></param>
        /// <returns></returns>
        public bool GetHasPreviousTrxDiffProcess(int refID)
        {
            try
            {
                string trxNo = string.Empty;
                List<SpWkfTransactionDiffProcess_Result> resultTrxCount;
                resultTrxCount = this.currentEntity.SpWkfTransactionDiffProcess(refID).ToList();
                return resultTrxCount == null || resultTrxCount.Count == 0 || !resultTrxCount.First().IsDiffProcess.HasValue
                    || resultTrxCount.First().IsDiffProcess.Value == 0 ? false : true;
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

        /// <summary>
        /// Get list of SalesForecast
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public List<SPSAL_FORECAST_RPT_Result> GetSalesForecast(string xml,string Status, ServiceUtility utilityObj)
        {
            List<SPSAL_FORECAST_RPT_Result> SPSAL_FORECAST_RPT_Result_Obj;
            IQueryable<SPSAL_FORECAST_RPT_Result> Obj_SPSAL_FORECAST_RPT_Result_Qry;
            try
            {
                List<SPSAL_FORECAST_RPT_Result> list = this.currentEntity.SPSAL_FORECAST_RPT(xml).ToList();
                list = list.Where(c => c.ORD_STATUS == (Status != string.Empty ? Status : c.ORD_STATUS)).ToList();

                Obj_SPSAL_FORECAST_RPT_Result_Qry = list.AsQueryable();
                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = Obj_SPSAL_FORECAST_RPT_Result_Qry.Count();

                //Apply Paging And Sorting for grid Purpose
                SPSAL_FORECAST_RPT_Result_Obj = Obj_SPSAL_FORECAST_RPT_Result_Qry.SortRecords<SPSAL_FORECAST_RPT_Result>(utilityObj).ToList();

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
            return SPSAL_FORECAST_RPT_Result_Obj;
        }


        public List<INV_UOM_MST> GetUOM(int PK)
        {
            List<INV_UOM_MST> resultAppStatus;
            try
            {
                int UomPk = this.currentEntity.INV_ITEM_MST.SingleOrDefault(c => c.ITM_PK == PK).ITM_UOM;
                resultAppStatus = this.currentEntity.INV_UOM_MST.Where(c => c.UOM_PK == UomPk && c.UOM_ACTIVE == 1).ToList();
                return resultAppStatus;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
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

        public List<PUR_VENDOR_MST> GetVendor(int PK)
        {
            List<PUR_VENDOR_MST> resultAppStatus;
            try
            {
                resultAppStatus = this.currentEntity.PUR_VENDOR_MST.Where(c => c.VEN_PK == PK && c.VEN_ACTIVE == 1).ToList();
                return resultAppStatus;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
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

        public List<ADM_APP_CONFIG_MST> GetAlertNotify(ADM_APP_CONFIG_MST ADM_APP_CONFIG_MSTobj)
        {
            List<ADM_APP_CONFIG_MST> ADM_APP_CONFIG_MSTLst = new List<ADM_APP_CONFIG_MST>();
            try
            {
                ADM_APP_CONFIG_MSTLst = (from config in this.currentEntity.ADM_APP_CONFIG_MST
                                         where config.ACF_ACTIVE == ADM_APP_CONFIG_MSTobj.ACF_ACTIVE
                                          && config.ACF_SETTING.Contains(ADM_APP_CONFIG_MSTobj.ACF_SETTING)
                                          && config.ACF_DATA == ADM_APP_CONFIG_MSTobj.ACF_DATA 
                                         select config).OrderBy(cty => cty.ACF_SETTING).ToList();

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

            return ADM_APP_CONFIG_MSTLst;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Get Currency Values
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<ADM_CURRENCY_MST> GetCurrency(ADM_CURRENCY_MST ADMCURRENCYMSTObj)
        {
            List<ADM_CURRENCY_MST> ADM_CURRENCY_MSTLst = new List<ADM_CURRENCY_MST>();
            try
            {
                ADM_CURRENCY_MSTLst = (from cur in this.currentEntity.ADM_CURRENCY_MST
                                       where cur.CUR_ACTIVE == ADMCURRENCYMSTObj.CUR_ACTIVE
                                          && cur.CUR_PK == (ADMCURRENCYMSTObj.CUR_PK > 0 ? ADMCURRENCYMSTObj.CUR_PK : cur.CUR_PK)
                                       select cur).ToList();

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

            return ADM_CURRENCY_MSTLst;
            //throw new NotImplementedException();
        }

        public List<INV_ITEM_VENDOR_MAP> GetVendorItems(int venPk,int itemPk)
        {
            List<INV_ITEM_VENDOR_MAP> resultAppStatus;
            try
            {
                resultAppStatus = this.currentEntity.INV_ITEM_VENDOR_MAP.Where(c => c.ITV_ITEM == itemPk && c.ITV_VENDOR == venPk && c.ITV_ACTIVE == 1).ToList();
                return resultAppStatus;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
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

        public List<CRM_CUSTOMER_MST> GetCustomers(int cuspk)
        {
            List<CRM_CUSTOMER_MST> resultAppStatus;
            try
            {
                resultAppStatus = this.currentEntity.CRM_CUSTOMER_MST.Where(c => c.CUS_PK == cuspk && c.CUS_ACTIVE==  1).ToList();
                return resultAppStatus;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
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


        /// <summary>
        /// Save transaction Log
        /// </summary>
        /// <param name="AdmTrxLogList"></param>
        /// <returns></returns>
        public long? SaveLog(List<ADM_APP_TRX_LOG> AdmTrxLogList)
        {
            long? retval = 0;
            long? maxID;
            try
            {


                if (AdmTrxLogList.Count > 0)
                {
                    maxID = currentEntity.ADM_APP_TRX_LOG.Max(log => (long?)log.ATL_PK);
                    maxID = (maxID.HasValue) ? maxID.Value + 1 : 1;
                    foreach (ADM_APP_TRX_LOG ADM_APP_TRX_LOG_obj in AdmTrxLogList)
                    {
                        ADM_APP_TRX_LOG_obj.ATL_PK = maxID.Value;
                        currentEntity.ADM_APP_TRX_LOG.AddObject(ADM_APP_TRX_LOG_obj);
                        maxID++;
                        retval = ADM_APP_TRX_LOG_obj.ATL_PK;
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
        /// Get all Items
        /// </summary>
        /// <param name="ITM_ACTIVE"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<INV_ITEM_MST> GetAllInvItemMstAutoCompleteList(short? ITM_ACTIVE, ServiceUtility utilityObj)
        {
            List<INV_ITEM_MST> INV_ITEM_MST_List_Obj = new List<INV_ITEM_MST>();
            try
            {
                if (utilityObj.FilterBy == DataFieldRes.ItemCode)
                {
                    INV_ITEM_MST_List_Obj = (from inv in this.currentEntity.INV_ITEM_MST
                                             where inv.ITM_ACTIVE == ITM_ACTIVE
                                                   && inv.ITM_CODE.StartsWith(utilityObj.FilterValue)
                                                  
                                             select inv).ToList();
                }
                else if (utilityObj.FilterBy == DataFieldRes.ItemName)
                {
                    INV_ITEM_MST_List_Obj = (from inv in this.currentEntity.INV_ITEM_MST
                                             where inv.ITM_ACTIVE == ITM_ACTIVE
                                                   && inv.ITM_NAME.StartsWith(utilityObj.FilterValue)

                                             select inv).ToList();
                }
                else
                {
                    INV_ITEM_MST_List_Obj = (from inv in this.currentEntity.INV_ITEM_MST
                                             where inv.ITM_ACTIVE == ITM_ACTIVE
                                                   && (inv.ITM_NAME.Contains(utilityObj.FilterValue) || inv.ITM_CODE.Contains(utilityObj.FilterValue))
                                             select inv).ToList();
                }

            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return INV_ITEM_MST_List_Obj;
        }

        public INV_ITEM_MST GetItemDetails(int ItemPk)
        {
            try
            {

               return this.currentEntity.INV_ITEM_MST.SingleOrDefault(f => f.ITM_PK == ItemPk);
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public List<ADM_COST_CENTER_MST> GetADM_COST_CENTER_MST(ADM_COST_CENTER_MST ADM_COST_CENTER_MSTobj)
        {
            List<ADM_COST_CENTER_MST> ADM_COST_CENTER_MSTLst = new List<ADM_COST_CENTER_MST>();
            try
            {
                ADM_COST_CENTER_MSTLst = (from config in this.currentEntity.ADM_COST_CENTER_MST
                                          where config.CNM_ACTIVE == ADM_COST_CENTER_MSTobj.CNM_ACTIVE
                                          //&& config.CNM_PK == ADM_COST_CENTER_MSTobj.CNM_PK
                                          select config).OrderBy(cty => cty.CNM_NAME).ToList();

                 //var q = (from pd in this.currentEntity.ADM_COST_CENTER_MST
                 //           join od in currentEntity.FIN_COA_COST_CENTER_MPG on pd.CNM_PK equals od.FCM_CNM_PK 
                 //           into t from rt in t.DefaultIfEmpty()
                 //         orderby pd.CNM_PK
                 //         select new {
                 //    pd.CNM_PK,
                 //    pd.CNM_CODE,
                 //    pd.CNM_NAME,
                 //    pd.CNM_DESC,
                 //    pd.CNM_DEPT,
                 //    pd.CNM_BIZUNIT,
                 //    pd.CNM_COMPANY,
                 //    pd.CNM_ACTIVE,
                 //    pd.CNM_CRTD_BY,
                 //    pd.CNM_CRTD_DT,
                 //    pd.CNM_MOD_BY,
                 //    pd.CNM_MOD_DT,
                 //    pd.CNM_GROUP,
                 //    rt.FCM_PK,
                 //    rt.FCM_COA_PK,
                 //    rt.FCM_CNM_PK,
                 //    rt.FCM_VALUE,
                 //    });
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

            return ADM_COST_CENTER_MSTLst;
            //throw new NotImplementedException();
        }
    }
}
