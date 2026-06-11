using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.HRMS.Payroll;
using DataAccess.HRMS.Payroll;

namespace BusinessLogic.HRMS.Payroll
{
    public class AppraisalDetailsBL
    {
        /// <summary>
        /// for get all list
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static DataTable GetList(BusinessObject.GridPrams gridParam, int bsu, string tranName = null, int? type = null)
        {
            return AppraisalDetailsDL.GetList(gridParam, bsu, tranName, type);
        }
        /// <summary>
        /// for get individual record
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <returns></returns>
        public static string GetAppraisalEdit(string CurrPK)
        {
            return AppraisalDetailsDL.GetAppraisalEdit(CurrPK);
        }
        /// <summary>
        /// save appraisal details
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static int? SaveAppraisal(string xmlDoc)
        {
            return AppraisalDetailsDL.SaveAppraisal(xmlDoc);
        }
        /// <summary>
        /// delete appraisal details
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int? DeleteAppraisal(int CurrPK, DateTime dateTime)
        {
            return AppraisalDetailsDL.DeleteAppraisal(CurrPK, dateTime);
        }
    }
}
