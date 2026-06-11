using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
namespace ERPManager
{
    public class FinYearMstManager : IFinYearMstManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Bank Master Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public FinYearMstManager(ERPEntities currentEntity)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FinCoaMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public short? GetFinYear(DateTime TransDate, Int32 BizUnit,int? CompanyPk = null )
        {
            //List<FIN_YEAR_MST> FIN_YEAR_MST_Obj ;
            short? finYear = null;
            try
            {
                if (CompanyPk.HasValue)
                {
                    List<ADM_COMPANY_MST> objAdmComMstLst = (from commst in this.currentEntity.ADM_COMPANY_MST where commst.CMP_PK == Convert.ToInt32(CompanyPk) select commst).ToList();
                    if (objAdmComMstLst != null && objAdmComMstLst.Count > 0)
                    {
                        if (objAdmComMstLst[0].CMP_BIZUNIT.HasValue)
                            BizUnit = Convert.ToInt32(objAdmComMstLst[0].CMP_BIZUNIT);
                    }
                }
                IQueryable<FIN_YEAR_MST> list = (from fyr in this.currentEntity.FIN_YEAR_MST
                                                 where TransDate >= fyr.FYR_DATE_FROM
                                                  && TransDate <= fyr.FYR_DATE_TO
                                                  && fyr.FYR_ACTIVE == 1
                                                  && fyr.FYR_BIZUNIT == BizUnit
                                                 select fyr
                          );
                if (list.ToList().Count() > 0)
                    finYear = list.Max(pvh => (short)pvh.FYR_PK);
                return finYear;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FinCoaMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<FIN_YEAR_MST> GetCurrentFinPeriod(DateTime TransDate, Int32 BizUnit)
        {
            List<FIN_YEAR_MST> finYearMstList = new List<FIN_YEAR_MST>();
            
            try
            {
                finYearMstList = (from fyr in this.currentEntity.FIN_YEAR_MST
                                  where TransDate >= fyr.FYR_DATE_FROM
                                    && TransDate <= fyr.FYR_DATE_TO
                                    && fyr.FYR_ACTIVE == 1
                                    && fyr.FYR_BIZUNIT == BizUnit
                                  select fyr).ToList();

                return finYearMstList;
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
        #endregion
    }
}
