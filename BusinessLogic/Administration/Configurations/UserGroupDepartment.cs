using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Administration.Configurations
{
    public class UserGroupDepartment
    {
        /// <summary>
        /// Function Used To Save User Group Departmets
        /// </summary>
        /// <param name="defaultDetails"></param>
        /// <returns></returns>
        public static string SaveUserGroupDepartment(string groupDeptDetails)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(groupDeptDetails);
            return DataAccess.Administration.Configurations.UserGroupDepartmentDL.SaveUserGroupDepartment(xmlstr);
        }

        /// <summary>
        /// Function Used To delete User Group Departmets
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string DeleteUserGroupDepartment(int sbuPK, int group)
        {
            return DataAccess.Administration.Configurations.UserGroupDepartmentDL.DeleteUserGroupDepartment(sbuPK, group).ToString();
        }
    }
}
