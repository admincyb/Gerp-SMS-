using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.HRMS.Admin.Masters;
using System.Data;
using DataAccess.HRMS.Admin.Masters;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class BonusTypeMasterBL
    {

        /// <summary>
        /// Method to Save Bonus Type
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveBonusTypeMaster(string xmlstr)
        {
            return BonusTypeMasterDL.SaveBonusTypeMaster(xmlstr);
        }

        /// <summary>
        /// Method to Get Bonus Type List
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetBonusTypeList(int? currPk, int active, int pageNo, int pageSize, int bizUnit, string BonusTypeCode = null, string BonusTypeName = null)
        {
            return BonusTypeMasterDL.GetBonusTypeList(currPk, active, pageNo, pageSize, bizUnit, BonusTypeCode, BonusTypeName);
        }

        /// <summary>
        /// Method to Get Bonus Type List
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetBonusType(int? ltmPK, int active, int bizUnit)
        {
            return BonusTypeMasterDL.GetBonusType(ltmPK, active, bizUnit);
        }

        /// <summary>
        /// Method to Delete Salary Template
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteBonusTypeMaster(int pk, string lastModifiedDate)
        {
            return BonusTypeMasterDL.DeleteBonusTypeMaster(pk, lastModifiedDate);
        }

        /// <summary>
        /// Method to Chnage Bonus Type Status
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int UpdateBonusTypeMasterStatus(int currPK, int status, int userPK, string lastModDate)
        {
            return BonusTypeMasterDL.UpdateBonusTypeMasterStatus(currPK, status, userPK, lastModDate);
        }
    }
}
