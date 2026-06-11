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
using ERPManager;
using ERPManager.Administration;

namespace ERPService.Administration
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CountryService" in code, svc and config file together.
    public class CountryService : ICountryService
    {

        #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public CountryService()
        {
            try
            {
                currentContext = new ERPEntities();
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vendorObj"></param>
        /// <returns></returns>
        public int SaveAdmCountryMst(List<AdmCountryMst> AdmCountryMstList)
        {
            AdmCountryMstManager AdmCountryMstMgr;
            int? admCountryMstPK;
            try
            {
                AdmCountryMstMgr = new AdmCountryMstManager(currentContext);
                admCountryMstPK = AdmCountryMstMgr.SaveAdmCountryMst(AdmCountryMstList);
                currentContext.SaveChanges();
                return admCountryMstPK.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                AdmCountryMstMgr = null;
                admCountryMstPK = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AdmCountryMstList"></param>
        /// <returns></returns>
        public int DeleteAdmCountryMst(List<AdmCountryMst> AdmCountryMstList)
        {
            AdmCountryMstManager AdmCountryMstMgr;
            try
            {
                AdmCountryMstMgr = new AdmCountryMstManager(currentContext);
                AdmCountryMstMgr.DeleteAdmCountryMst(AdmCountryMstList);
                return currentContext.SaveChanges();
            }
            catch (UpdateException ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                AdmCountryMstMgr = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AdmCountryMstObj"></param>
        /// <returns></returns>
        public List<AdmCountryMst> GetAdmCountryMst(AdmCountryMst AdmCountryMstObj, ServiceUtility utilityObj = null)
        {
            AdmCountryMstManager AdmCountryMstMgr;
            try
            {
                AdmCountryMstMgr = new AdmCountryMstManager(currentContext);
                return AdmCountryMstMgr.GetAdmCountryMst(AdmCountryMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                AdmCountryMstMgr = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AdmCountryMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public ServiceUtility GetAdmCountryMstCount(AdmCountryMst AdmCountryMstObj, ServiceUtility utilityObj = null)
        {
            AdmCountryMstManager AdmCountryMstMgr;
            try
            {
                AdmCountryMstMgr = new AdmCountryMstManager(currentContext);
                return AdmCountryMstMgr.GetAdmCountryMstCount(AdmCountryMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                AdmCountryMstMgr = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AdmCountryMst GetInitilizedAdmCountryMst()
        {
            AdmCountryMstManager admCountryMstMgr;
            try
            {
                admCountryMstMgr = new AdmCountryMstManager(currentContext);
                return admCountryMstMgr.GetInitilizedAdmCountryMst();

            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admCountryMstMgr = null;
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="AdmCurrencyMstObj"></param>
        ///// <param name="utilityObj"></param>
        ///// <returns></returns>
        //public List<AdmCurrencyMst> GetAdmCurrencyMst(AdmCurrencyMst AdmCurrencyMstObj, ServiceUtility utilityObj = null)
        //{
        //    AdmCurrencyMstManager admCurrencyMstMgr;
        //    try
        //    {
        //        admCurrencyMstMgr = new AdmCurrencyMstManager(currentContext);
        //        return admCurrencyMstMgr.GetAdmCurrencyMst(AdmCurrencyMstObj, utilityObj);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    finally
        //    {
        //        admCurrencyMstMgr = null;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public AdmCurrencyMst GetInitilizedAdmCurrencyMst()
        //{
        //    AdmCurrencyMstManager admCurrencyMstMgr;
        //    try
        //    {
        //        admCurrencyMstMgr = new AdmCurrencyMstManager(currentContext);
        //        return admCurrencyMstMgr.GetInitilizedAdmCurrencyMst();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
        //    }
        //    finally
        //    {
        //        admCurrencyMstMgr = null;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="admCountryMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<AdmCountryMst> GetAdmCountryMstAutoCompleteList(AdmCountryMst admCountryMstObj, ServiceUtility utilityObj)
        {
            AdmCountryMstManager admCountryMstMgr;
            try
            {
                admCountryMstMgr = new AdmCountryMstManager(currentContext);
                return admCountryMstMgr.GetAdmCountryMstAutoCompleteList(admCountryMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admCountryMstMgr = null;
            }
        }
        #endregion
      
    }
}
