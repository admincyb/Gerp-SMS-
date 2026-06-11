using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.Administration.Masters
{
    public class StoreMaterialMapping
    {

        public static string SaveMaterialMappingDetails(string materialMapDetails)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(materialMapDetails);
            return DataAccess.Administration.Masters.StoreMaterialMappingDL.SaveMaterialMappingDetails(xmlstr);
        }
        /// <summary>
        /// get material Map Details
        /// </summary>
        /// <param name="deptPK"></param>
        /// <param name="deptParentPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public static string GetMaterialMappingDtls(int mapParentPK, int bizUnit, int store,string pVal,int type)
        {
            DataTable dtMaterial = DataAccess.Administration.Masters.StoreMaterialMappingDL.GetMaterialMappingDtls(mapParentPK, bizUnit, store, pVal,type);
            string jString = string.Empty;
            if (dtMaterial.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtMaterial, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEPK, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREENAME, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEPARENT, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEHASCHILD, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEISCHECKED, string.Empty, "IS_ITEM");
            }
            return jString;
        }

       
        /// <summary>
        /// Returns MaterialMappingList details list in json string format
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetMaterialMappingList(GridPrams grid, int bizUnit)
        {
            DataSet dsMaterialMappingList = DataAccess.Administration.Masters.StoreMaterialMappingDL.GetMaterialMappingList(grid, bizUnit);
            string jString = string.Empty;
            if (dsMaterialMappingList.Tables.Count > 1 && dsMaterialMappingList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsMaterialMappingList);
            }
            return jString;
        }
        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetMaterialMapSearchValues(string searchValue, string searchBy, int sbuPK)
        {
            DataTable dtSearch = DataAccess.Administration.Masters.StoreMaterialMappingDL.GetMaterialMapSearchValues(searchValue, searchBy, sbuPK);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);

        }
    }
}
