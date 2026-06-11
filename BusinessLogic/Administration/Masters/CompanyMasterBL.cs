using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using DataAccess;
using BusinessObject.Administration.Masters;
using GTIService;
using BusinessObject.CommonManagement;


namespace BusinessLogic.Administration.Masters
{
    public class CompanyMasterBL
    {
        public static DataSet GetCompanyMaster(int apsPK, short active, int bizUnit)
        {
            return DataAccess.Administration.Masters.CompanyMasterDA.GetCompanyMaster(apsPK, active, bizUnit);
        }

        public static int? SaveCompanyMaster(CompanyMasterBO companyMasterBo)
        {
            return DataAccess.Administration.Masters.CompanyMasterDA.SaveCompanyMaster(companyMasterBo);
        }

        public static int DeleteCompanyMaster(int apsPK, DateTime lastModDate)
        {
            return DataAccess.Administration.Masters.CompanyMasterDA.DeleteCompanyMaster(apsPK,lastModDate);
        }

        public static DataSet GetCountryList()
        {
            return DataAccess.Administration.Masters.CompanyMasterDA.GetCountryList();
        }

        public static DataSet GetCurrency(int bizUnit)
        {
            return DataAccess.Administration.Masters.CompanyMasterDA.GetCurrency(bizUnit);
        }



    }
}
