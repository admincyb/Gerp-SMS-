using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.Administration.Configurations
{
    public class DepartmentConfig
    {

        /// <summary>
        /// Function Used To save Department agianst sbu
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string SaveSBUDepartmentConfig(string requestData)
        {
            BusinessObject.Administration.Configurations.DepartmentConfig departmentConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Administration.Configurations.DepartmentConfig>(requestData);
            return DataAccess.Administration.Configurations.DepartmentConfigDL.SaveSBUDepartmentConfig(departmentConfig);
        }

        /// <summary>
        /// Function Used To Get Department Details by sbu
        /// </summary>
        /// <param name="gridPrams"></param>
        /// <returns></returns>
        public static string GetSBUDepartmentConfig(int sbuPK)
        {
            DataSet dsDetpList = DataAccess.Administration.Configurations.DepartmentConfigDL.GetSBUDepartmentConfig(sbuPK);
            string jString = string.Empty;
            if (dsDetpList.Tables.Count > 1 && dsDetpList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsDetpList);
            }
            return jString;
        }

        /// <summary>
        /// Function Used To Get Department Details For Fill Combo by sbu
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <returns></returns>
        public static string GetSBUDeptList(int sbuPK)
        {
            DataTable dtCategory = DataAccess.Administration.Configurations.DepartmentConfigDL.GetSBUDeptList(sbuPK);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Configurations.DeptConfig.Fields.DEPTNAME, GTIService.Constants.Configurations.DeptConfig.Fields.DEPTPK);
        }
    }
}
