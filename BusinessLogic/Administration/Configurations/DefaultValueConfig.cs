using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.Administration.Configurations
{
    public class DefaultValueConfig
    {
        
        /// <summary>
        /// Function Used To Save Default Values
        /// </summary>
        /// <param name="defaultDetails"></param>
        /// <returns></returns>
        public static string SaveDefaultValueConfig(string defaultDetails)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(defaultDetails);
            return DataAccess.Administration.Configurations.DefaultValueConfigDL.SaveDefaultValueConfig(xmlstr);
        }

        /// <summary>
        /// Function Used To Get Default Values Based on the business unit,department and group
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="deptPK"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GetDefaultValueConfig(int sbuPK, int deptPK, string group)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.Administration.Configurations.DefaultValueConfigDL.GetDefaultValueConfig(sbuPK, deptPK, group)).ToString();
        }

        /// <summary>
        ///  Function Used To Delete Default Values Based on the business unit,department and group
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="deptPK"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string DeleteDefaultValueConfig(int sbuPK, int deptPK, string group)
        {
            return DataAccess.Administration.Configurations.DefaultValueConfigDL.DeleteDefaultValueConfig(sbuPK, deptPK, group).ToString();
        }

        /// <summary>
        /// Function Used To get the group in the dept. for fill auto complete
        /// </summary>
        /// <param name="group"></param>
        /// <param name="deptPK"></param>
        /// <returns></returns>
        public static string GetAllGroupList(string group, int deptPK)
        {
            DataTable dtSearch = DataAccess.Administration.Configurations.DefaultValueConfigDL.GetAllGroupList(group, deptPK);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Configurations.DefaultConfig.Fields.DEFAULTGROUP,GTIService.Constants.Configurations.DefaultConfig.Fields.DEFAULTGROUP);
            return jString;
        }

        /// <summary>
        /// Function Used To get the Def Value by passing name sbu dep and ID
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <param name="deptPK"></param>
        /// <param name="defName"></param>
        /// <param name="defID"></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>purchase order</for>
        /// Used in getting the default value of Po number format
        /// <returns></returns>
        public static DataTable GetDefaultValue(int sbuPK, int deptPK, string defName, int defID)
        {
            return DataAccess.Administration.Configurations.DefaultValueConfigDL.GetDefaultValue(sbuPK,deptPK,defName,defID);
        }



    }
}
