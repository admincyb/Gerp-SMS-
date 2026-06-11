using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Administration.Masters;
using BusinessObject.Administration.Masters;
using ERP.Utilities;
using System.Configuration;

namespace BusinessLogic.Administration.Masters
{
   public class DashboardSetupBL
    {
        /// <summary>
        /// For get   lsit
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="p"></param>
        /// <returns></returns>
       public static DataTable GetDashboardSetupList(int IND_USER, string IND_NAME, string IND_CODE, int pageNo, int pageSize, int Status = -1)
        {
            return DashboardSetupDL.GetDashboardSetupList(IND_USER, IND_NAME, IND_CODE, pageNo, pageSize, Status);
        }
       public static DataTable GetMenuDetails(BusinessObject.User authUser,int deptPk)
       {
           Int16 module;
           module = 0;
           Int16.TryParse(ConfigurationManager.AppSettings[ERP.Utilities.ConfigStrings.GERPModule], out module);
           return DashboardSetupDL.GetMenuDetails(authUser.SBUID, deptPk, authUser.PKUser, module);
       }

       public static DataTable GetReportListDetails(int GrpPK)
       {
           //Int16 module;
           //module = 0;
           //Int16.TryParse(ConfigurationManager.AppSettings[ERP.Utilities.ConfigStrings.GERPModule], out module);
           return DashboardSetupDL.GetReportListDetails(GrpPK);
       }

       /// <summary>
       /// method for Get BizUnit and Department
       /// </summary>
       /// <param name="userID" Type=int></param>
       /// <returns>DataTable</returns>
       public static DataTable GetMISReportDtls(int userID, int sbuID, int mngPK,int deptPk)
       {
           return DashboardSetupDL.GetMISReportDtls(userID, sbuID, mngPK,deptPk);
       }

       public static DataTable GetDashletDetails(int userPK, int bizUnit, int currPk)
       {
           return DashboardSetupDL.GetDashletDetails(userPK, bizUnit, currPk);
       }
       //public static MenuMappingDetailsHeader GetMenuDetails(BusinessObject.User authUser)
       //{
       //    Int16 module;
       //    module = 0;
       //    Int16.TryParse(ConfigurationManager.AppSettings[ERP.Utilities.ConfigStrings.GERPModule], out module);
       //    try
       //    {
       //        MenuMappingDetailsHeader objMenuMapping = new MenuMappingDetailsHeader();
       //        string result = DashboardSetupDL.GetMenuDetails(authUser.SBUID, authUser.CurrentDeptPK, authUser.PKUser, module);
       //        if (!string.IsNullOrEmpty(result))
       //        {
       //            objMenuMapping = (MenuMappingDetailsHeader)CommonFunctions.DeserializeObject(result, objMenuMapping);
       //            return objMenuMapping;
       //        }
       //        else
       //        {
       //            return null;
       //        }
       //    }
       //    catch
       //    {
       //        throw;
       //    }
       //}
       
       public static DashboardHeader DashboardSetupByPK(int currPK)
        {
            try
            {
                DashboardHeader addolidayMaster = new DashboardHeader();
                string dtl = DashboardSetupDL.DashboardSetupByPK(currPK);
                if (dtl != string.Empty)
                {
                    addolidayMaster = (DashboardHeader)CommonFunctions.DeserializeObject(dtl, addolidayMaster);
                    return addolidayMaster;
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
       public static int? SaveDashboardSetupDetails(string strxml)
        {
            return DashboardSetupDL.SaveDashboardSetupDetails(strxml);
        }

       public static int? DeleteRecord(int CurrPK, string ModDate)
       {
           return DashboardSetupDL.GetDashletDetails(CurrPK, ModDate);
       }
       public static DataTable GetDashBoardDefultValues(string Query)
       {
           return DashboardSetupDL.GetDashBoardDefultValues(Query);
       }
    }
}
