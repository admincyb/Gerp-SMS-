using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.Administration.Configurations
{
    public class UserGroupDepartmentDL
    {
        /// <summary>
        /// Function Used To Save User Group Dept Values
        /// </summary>
        /// <param name="defaultDetails"></param>
        /// <returns></returns>
        public static string SaveUserGroupDepartment(string groupDeptDetails)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Configurations.GroupDept.Parameters.USERGROUPDEPTXML ,(object)groupDeptDetails ,DBService.ParameterType.XML),                 
                new DBService.Parameters( GTIService.Constants.Configurations.GroupDept.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.GroupDept.Procedures.SPSAVEUSERGROUPDEPT , colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.GroupDept.Parameters.RETVAL]).Value);
        }

        /// <summary>
        /// Function Used To delete User Group Departmets
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static int DeleteUserGroupDepartment(int sbuPK, int group)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Configurations.GroupDept.Parameters.BIZUNIT , sbuPK),
                new DBService.Parameters(GTIService.Constants.Configurations.GroupDept.Parameters.USERGROUP , group)
            };
            return dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.GroupDept.Procedures.SPDELETEUSERGROUPDEPT, colParameters);
        }
    }
}
