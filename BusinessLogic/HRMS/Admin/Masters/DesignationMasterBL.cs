using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.HRMS.Admin.Masters;
using System.Data;
using DataAccess.HRMS.Admin.Masters;
using GTIService;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class DesignationMasterBL
    {
        public static DataSet GetDesignationList(int? currPk, int active, int pageNo, int pageSize, int bizUnit, int? jobCateg, int? jobGrd, string DesignationCode = null, string DesignationName = null)
        {
            return DesignationMasterDL.GetDesignationList(currPk, active, pageNo, pageSize, bizUnit,jobCateg, jobGrd, DesignationCode, DesignationName);
        }

        public static DataTable GetSDesignationType(int? ltmPK, int active, int bizUnit)
        {
            return DesignationMasterDL.GetSDesignationType(ltmPK, active, bizUnit);
        }

        /// <summary>
        /// Method to Save Designation
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveDesignationMaster(string xmlstr)
        {
            return DesignationMasterDL.SaveDesignationMaster(xmlstr);
        }

        /// <summary>
        /// Method to Delete Salary Template
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteDesignationTypeMaster(int pk, string lastModifiedDate)
        {
            return DesignationMasterDL.DeleteDesignationTypeMaster(pk, lastModifiedDate);
        }

        public static int UpdateDesignationTypeMasterStatus(int currPK, int status, int userPK, string lastModDate)
        {
            return DesignationMasterDL.UpdateDesignationTypeMasterStatus(currPK, status, userPK, lastModDate);
        }

        public static DesignationMasterBO.DesignationType GetDocTypeList(int pk)
        {
            DesignationMasterBO.DesignationType objDocType = new DesignationMasterBO.DesignationType();
            string user = DesignationMasterDL.GetDocTypeList(pk);
            if (user != string.Empty)
            {
                objDocType = (DesignationMasterBO.DesignationType)CommonFunctions.DeserializeObject(user, objDocType);
                return objDocType;
            }
            else
            {
                return null;
            }
        }
              
    }
}
