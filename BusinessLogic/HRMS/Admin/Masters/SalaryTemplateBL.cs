using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.HRMS.Admin.Masters;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class SalaryTemplateBL
    {
        public static DataTable GetSalaryTemplateKv(int bizUnit, int pk = 0, int active = 1)
        {
            return SalaryTemplateDL.GetSalaryTemplateKv(bizUnit, pk, active);
        }

        /// <summary>
        /// Method to Save Salary Template
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveSalaryTemplate(string xmlstr)
        {
            return SalaryTemplateDL.SaveSalaryTemplate(xmlstr);
        }

        /// <summary>
        /// Method to Get Salary Template List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalaryTemplateList(BusinessObject.GridPrams gridParam, int bizUnit, int payrolltype, string tempalteCode = null)
        {
            return DataAccess.HRMS.Admin.Masters.SalaryTemplateDL.GetSalaryTemplateList(gridParam, bizUnit, payrolltype, tempalteCode);
        }

        //Get Salary Template
        public static string GetSalaryTemplate(int pk)
        {
            return DataAccess.HRMS.Admin.Masters.SalaryTemplateDL.GetSalaryTemplate(pk);
        }

        /// <summary>
        /// Method to Delete Salary Template
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteSalaryTemplate(int pk, string lastModifiedDate)
        {
            return DataAccess.HRMS.Admin.Masters.SalaryTemplateDL.DeleteSalaryTemplate(pk, lastModifiedDate);
        }

        public static int UpdateSalaryTemplateStatus(int currPK, int status, int userPK, string lastModDate)
        {
            return DataAccess.HRMS.Admin.Masters.SalaryTemplateDL.UpdateSalaryTemplateStatus(currPK, status, userPK, lastModDate);
        }

        public static DataTable GetEmployeePayElement(int TemplateDetailPk)
        {
            return DataAccess.HRMS.Admin.Masters.SalaryTemplateDL.GetEmployeePayElement(TemplateDetailPk);
        }

        public static int? UpdateEmployeeSalaryTemplate(string xmlstr)
        {
            return DataAccess.HRMS.Admin.Masters.SalaryTemplateDL.UpdateEmployeeSalaryTemplate(xmlstr);
        }
    }
}
