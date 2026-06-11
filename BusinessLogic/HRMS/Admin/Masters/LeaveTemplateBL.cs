using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.HRMS.Admin.Masters;
using System.Data;
using DataAccess.HRMS.Admin.Masters;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class LeaveTemplateBL
    {
        public static DataTable GetLeaveTemplateList(int bizUnit, int pk = 0, int active = 1)
        {
            return LeaveTemplateDL.GetLeaveTemplateList(bizUnit, pk, active);
        }

        public static int UpdateLeaveTemplateStatus(int currPK, int status, int userPK, string lastModDate)
        {
            return LeaveTemplateDL.UpdateLeaveTemplateStatus(currPK, status, userPK, lastModDate);
        }

        /// <summary>
        /// Method to Save Leave Template
        /// </summary>
        /// <param name="leaveTemplate">BusinessObject.HRMS.Admin.Masters.LeaveTemplateBO.LeaveTemplate</param>
        /// <returns>int</returns>
        public static int SaveLeaveTemplate(BusinessObject.HRMS.Admin.Masters.LeaveTemplateBO.LeaveTemplate leaveTemplate)
        {
            return LeaveTemplateDL.SaveLeaveTemplate(leaveTemplate);
        }

        /// <summary>
        /// Method to Get Leave Templates for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetLeaveTemplateListPage(BusinessObject.GridPrams gridParam, int bizUnit, string tempalteCode = null,string sortOrder=null)
        {
            return LeaveTemplateDL.GetLeaveTemplateListPage(gridParam, bizUnit, tempalteCode, sortOrder);
        }

        /// <summary>
        /// Method to Delete Leave Template Details
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteLeaveTemplate(int pk, string lastModifiedDate)
        {
            return LeaveTemplateDL.DeleteLeaveTemplate(pk, lastModifiedDate);
        }

        /// <summary>
        /// Get LeaveTemplate XML
        /// </summary>
        /// <param name="templatePk">int</param>
        /// <returns>string</returns>
        public static string LeaveTemplateGetXML(int templatePk)
        {
            return LeaveTemplateDL.LeaveTemplateGetXML(templatePk);
        }
    }
}
