using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.HRMS.Admin.Masters;
using System.Data;
using BusinessObject.HRMS.Admin.Masters;
using GTIService;
namespace BusinessLogic.HRMS.Admin.Masters
{
    public class HolidayMasterBL
    {

        /// <summary>
        /// For get   lsit
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataTable GetHolidayMasterList(int bizUnit, string caption, int status, int pageNo, int pageSize)
        {
            return HolidayMasterDL.GetHolidayMasterList(bizUnit, caption, status, pageNo, pageSize);
        }

        public static int? SaveHolidayMasterDetails(string strxml)
        {
            return HolidayMasterDL.SaveHolidayMasterDetails(strxml);
        }

        public static HolidayMasterHeader GetHolidayMasterByPK(int bizUnit, int status, int itemPK)
        {
            try
            {
                HolidayMasterHeader addolidayMaster = new HolidayMasterHeader();
                string dtl = HolidayMasterDL.GetHolidayMasterByPK(bizUnit, status, itemPK);
                if (dtl != string.Empty)
                {
                    addolidayMaster = (HolidayMasterHeader)CommonFunctions.DeserializeObject(dtl, addolidayMaster);
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

        public static DataTable GetHolidayType(int CurrPK, int status, int bizUnit)
        {
            return HolidayMasterDL.GetHolidayType(CurrPK, status, bizUnit);
        }

        public static int DeleteHolidayMaster(int pk, string lastModifiedDate)
        {
            return HolidayMasterDL.DeleteHolidayMaster(pk, lastModifiedDate);
        }

        public static DataSet GetHolidayListRPT(int currPK)
        {
            return HolidayMasterDL.GetHolidayListRPT(currPK);
        }


    }
}
