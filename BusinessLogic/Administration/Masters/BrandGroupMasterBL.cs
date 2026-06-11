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
    public class BrandGroupMasterBL
    {
        public static int? SaveBrandGroupMaster(BrandGroupMasterBO objBrandGroupMasterBO)
        {
            return DataAccess.Administration.Masters.BrandGroupMasterDA.SaveBrandGroupMaster(objBrandGroupMasterBO);
        }

        public static int? DeleteBrandGroupMaster(int CurrPK, DateTime LastModifiedTime)
        {
            return DataAccess.Administration.Masters.BrandGroupMasterDA.DeleteBrandGroupMaster(CurrPK, LastModifiedTime);
        }

        public static DataTable GetBrandGroupMaster(int CurrPK, short Status, int bizUnit, string BrandGrpCode, string BrandGrpName, int PageNo, int PageSize)
        {
            return DataAccess.Administration.Masters.BrandGroupMasterDA.GetBrandGroupMaster(CurrPK, Status, bizUnit, BrandGrpCode, BrandGrpName, PageNo, PageSize);
        }
    }
}
