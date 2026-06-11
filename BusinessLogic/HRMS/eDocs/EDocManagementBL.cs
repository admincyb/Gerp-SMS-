using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.HRMS.eDoc;
using BusinessObject.HRMS.eDocs;
using System.Data;
using BusinessObject.CommonManagement;

namespace BusinessLogic.HRMS.eDocs
{
    public class EDocManagementBL
    {
        public static int Save(EDocBO eDoc)
        {
            return EdocManagementDL.Save(eDoc);
        }

        public static DataTable GetProjectsOrSite(int? pk, int active = 1)
        {
            return EdocManagementDL.GetProjectsOrSite(pk, active);
        }

        /// <summary>
        /// Only Get Projects Or Sites Used in Any Transactions
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUsedProjectsOrSite()
        {
            return EdocManagementDL.GetUsedProjectsOrSite();
        }

        public static EDocBO GetEDocDetailsByID(long eDocPk, int userPk)
        {
            return EdocManagementDL.GetEDocDetailsByID(eDocPk, userPk);
        }

        public static DataTable GetSendToUsers(int excludedUsrPk, int active)
        {
            return EdocManagementDL.GetSendToUsers(excludedUsrPk, active);
        }

        public static DataTable GetAutoEdocEmployee(string searchValue)
        {
            return EdocManagementDL.GetAutoEdocEmployee(searchValue);
        }

        public static DataTable GetEdocs(EDocSearchParameter parameter)
        {
            return EdocManagementDL.GetEdocs(parameter);
        }

        public static DataTable GetEdocsSummary(int userPk, int bizUnit)
        {
            return EdocManagementDL.GetEdocsSummary(userPk, bizUnit);
        }

        public static DataTable GetDepartments(int? pk, int? active)
        {
            return BusinessLogic.CommonManagement.CommonBL.GetDepartmentList(pk, active);
        }

        public static DataTable GetFromTo(int constPK, int constGroup, ConstGroupType? groupType, int groupValue, int active, int bizUnit)
        {
            return BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(constPK, constGroup, groupType, groupValue, active, bizUnit);
        }

        public static string GetFolderTreeXml(int folderPk, int bizUnit)
        {
            return BusinessLogic.UserControl.FolderExplorerBL.GetFolderTreeXml(folderPk, bizUnit);
        }
    }
}
