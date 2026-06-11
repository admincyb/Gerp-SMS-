using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.HRMS.Admin.Masters;
using System.Data;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class OTTemplateBL
    {
        public static DataTable GetOTTemplateList(int bizUnit, int pk = 0, int active = 1)
        {
            return OTTemplateDL.GetOTTemplateList(bizUnit, pk, active);
        }


        public static int UpdateOTTemplateStatus(int currPK, int status, int userPK, string lastModDate)
        {
            return OTTemplateDL.UpdateOTTemplateStatus(currPK, status, userPK, lastModDate);
        }

        /// <summary>
        /// Method to Save OT Template
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveOTTemplate(string xmlStr)
        {
            return OTTemplateDL.SaveOTTemplate(xmlStr);
        }

        /// <summary>
        /// Method to Get OT Templates for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetOTTemplateListPage(BusinessObject.GridPrams gridParam, int bizUnit, string tempalteCode = null)
        {
            return OTTemplateDL.GetOTTemplateListPage(gridParam, bizUnit, tempalteCode);
        }

        /// <summary>
        /// Get OT Template
        /// </summary>
        /// <param name="pk">int</param>
        /// <returns>string</returns>
        public static string GetOTTemplate(int pk)
        {
            return OTTemplateDL.GetOTTemplate(pk);
        }

        /// <summary>
        /// Method to Delete OT Template Details
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteOTTemplate(int pk, string lastModifiedDate)
        {
            return OTTemplateDL.DeleteOTTemplate(pk, lastModifiedDate);
        }
    }
}
