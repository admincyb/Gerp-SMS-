using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.UserControl;
using DataAccess;
using System.Data;
using BusinessObject.UserControls;
using BusinessObject.HRMS.eDocs;

namespace BusinessLogic.UserControl
{
    public class FolderExplorerBL
    {
        public static string GetFolderTreeXml(int folderPk, int bizUnit)
        {
            return FolderExplorerDL.GetFolderTreeXml(folderPk, bizUnit);
        }

        public static int Save(FolderBO folderBo)
        {
            return FolderExplorerDL.Save(folderBo);
        }

        public static int Delete(long pk, DateTime lastModifiedDate)
        {
            return FolderExplorerDL.Delete(pk, lastModifiedDate);
        }

        public static DataTable GetMappingUsersList(long folderPk)
        {
            return FolderExplorerDL.GetMappingUsersList(folderPk);
        }

        public static int SaveUserMapping(FolderMappingBo folderMappingBo)
        {
            return FolderExplorerDL.SaveUserMapping(folderMappingBo);
        }

        public static string GetMappingPermissionRole(string configText, int bizUnit)
        {
            DataTable dt= DataAccess.CommonManagement.CommonDA.GetApplicaitonConfiguaration(configText, string.Empty, bizUnit);

            if (dt == null || dt.Rows.Count < 1)
                return string.Empty;

            return  dt.Rows[0].Field<string>("ACF_DATA");
        }
    }
}
