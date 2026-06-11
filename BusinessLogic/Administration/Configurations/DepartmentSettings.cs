using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Administration.Configurations
{
    public class DepartmentSettings
    {
        /// <summary>
        /// Function Used To save Department config values
        /// </summary>
        /// <param name="defaultDetails"></param>
        /// <returns></returns>
        public static string SaveConfigValue(string configDetails)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(configDetails);
            return DataAccess.Administration.Configurations.DepartmentSettingsDL.SaveConfigValue(xmlstr);
        }

        /// <summary>
        ///  Function Used To get Department config values based on the dept and sbu
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="deptPK"></param>
        /// <returns></returns>
        public static string GetConfigValue(int sbuPK, int deptPK)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.Administration.Configurations.DepartmentSettingsDL.GetConfigValue(sbuPK, deptPK)).ToString();
        }

        /// <summary>
        /// Function Used To delete Department config values based on the dept and sbu
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="deptPK"></param>
        /// <returns></returns>
        public static string DeleteConfigValue(int sbuPK, int deptPK)
        {
            return DataAccess.Administration.Configurations.DepartmentSettingsDL.DeleteConfigValue(sbuPK, deptPK).ToString();
        }
    }
}
