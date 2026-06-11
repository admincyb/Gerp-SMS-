using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using BusinessObject.Administration.Configurations;
using DataAccess;
using System.Data;
using DataAccess.Administration.Configurations;

namespace BusinessLogic.Administration.Configurations
{
    public class CompanyBL
    {
        /// <summary>
        /// To save company details
        /// </summary>
        /// <param name="objCompany"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static int SaveCompany(CompanyBO objCompany, User objUser)
        {
            return CompanyDA.SaveCompany(objCompany, objUser);
        }
        /// <summary>
        /// To Retrive company details
        /// </summary>
        /// <param name="PK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetCompanyDetails(int PK, int Active, int BizUnit)
        {
            DataTable dtCompany = CompanyDA.GetCompanyList(PK, Active, BizUnit);
            return dtCompany;
        }
        /// <summary>
        /// Get Company Details
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <param name="cmpName"></param>
        /// <param name="bizUnit"></param>
        /// <param name="specialCondition"></param>
        /// <returns></returns>
        public static DataTable GetCompanyDetails(int CompanyPK, int Active, string cmpName, int bizUnit, string specialCondition)
        {
            return CompanyDA.GetCompanyDetails(CompanyPK, Active, cmpName, bizUnit, specialCondition);
        }
        /// <summary>
        /// To delete company
        /// </summary>
        /// <param name="objCompany"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static int DeleteCompany(int companyPK)
        {
            return CompanyDA.DeleteCompany(companyPK);
        }

        public static DataTable GetVersionDetails()
        {
            return CompanyDA.GetVersionDetails();
        }
    }
}
