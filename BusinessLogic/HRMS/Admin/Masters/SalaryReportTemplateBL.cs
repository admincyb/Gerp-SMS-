using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.HRMS.Admin.Masters;
using System.Data;
using BusinessObject.HRMS.Admin.Masters;
using ERP.Utilities;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class SalaryReportTemplateBL
    {
        public static int SaveSalaryReportTemplateDetails(string strxml)
        {
            return SalaryReportTemplateDL.SaveSalaryReportTemplateDetails(strxml);
        }

        public static DataSet GetSalaryReportTemplateList(int? currPk, int active, int pageNo, int pageSize, int bizUnit, string TemplateName = null)
        {
            return SalaryReportTemplateDL.GetSalaryReportTemplateList(currPk, active, pageNo, pageSize, bizUnit, TemplateName);
        }

        public static DataTable GetPayElementDetails(int? CurrPK, int active, int bizUnit)
        {
            return SalaryReportTemplateDL.GetPayElementDetails(CurrPK, active, bizUnit);
        }

        public static SalaryReportTemplateHeader GetSalaryReportTemplateDetails(int? currPk)
        {
            try
            {
                SalaryReportTemplateHeader objTemplate = new SalaryReportTemplateHeader();
                string result = SalaryReportTemplateDL.GetSalaryReportTemplateDetails(currPk);
                if (!string.IsNullOrEmpty(result))
                {
                    objTemplate = (SalaryReportTemplateHeader)CommonFunctions.DeserializeObject(result, objTemplate);
                    return objTemplate;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static int DeleteSalaryTemplateMaster(int pk, string lastModifiedDate)
        {
            return SalaryReportTemplateDL.DeleteSalaryTemplateMaster(pk, lastModifiedDate);
        }
    }
}
