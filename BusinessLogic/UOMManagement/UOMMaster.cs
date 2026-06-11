using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

using GTIService;



namespace BusinessLogic.UOMManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class UOMMaster
    {
        #region Methods
        /// <summary>
        /// Get UOM Types 
        /// </summary>
        /// <returns>string</returns>
        public static string GetUOMType(int sbuPK)
        {
            DataTable dtUOMType = DataAccess.UOMManagement.UOMMaster.GetUOMType(sbuPK);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtUOMType,GTIService.Constants.UOM.Fields.UOMTYPENAME,GTIService.Constants.UOM.Fields.UOMTYPEPK );
            return jString;

        }

        /// <summary>
        /// Get UOMs
        /// </summary>
        /// <param name="UOMTypeID"></param>
        /// <returns>string</returns>
        public static string GetUOM(string UOMTypeID, int uomPK, int sbu, string uomTypeName)
        {
            DataTable dtUnit = DataAccess.UOMManagement.UOMMaster.GetUOM(UOMTypeID, uomPK, sbu, uomTypeName);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtUnit,GTIService.Constants.UOM.Fields.UOMCODE,GTIService.Constants.UOM.Fields.UOMPK);
            return jString;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uomPK"></param>
        /// <param name="Type"></param>
        /// <param name="active"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetUOM(int uomPK, int? uomType, int active, int bizUnit)//------------ganesh // Modified By Biju
        { 
             return DataAccess.UOMManagement.UOMMaster.GetUOM(uomPK,uomType,active,bizUnit);
        }
      
        /// <summary>
        /// Save UOM Details 
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>string</returns>
        public static string SaveUOM(string uOMDtls)
        {
           
            string xmlstr = GTIService.CommonFunctions.JsonToXml(uOMDtls);
            return DataAccess.UOMManagement.UOMMaster.SaveUOM(xmlstr).ToString();
        }

        /// <summary>
        /// Save UOM Type
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="userPk"></param>
        /// <returns>string</returns>
        public static string SaveUOMType(string requestData, int userPk)
        {
            BusinessObject.UOMManagement.UOM UOMDet = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.UOMManagement.UOM>(requestData);
            UOMDet.UserPk = userPk;
            return DataAccess.UOMManagement.UOMMaster.SaveUOMType(UOMDet);
        }
       
        /// <summary>
        /// Get UOM List 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetUOMList(GridPrams grid, int bizUnit)
        {
            DataSet dsUOMList = DataAccess.UOMManagement.UOMMaster.GetUOMList(grid, bizUnit);
            string jString = string.Empty;
            if (dsUOMList.Tables.Count > 1 && dsUOMList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsUOMList);
            }
            return jString;
        }

        /// <summary>
        /// Get UOM Types List
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetUOMTypeList(GridPrams grid, int bizUnit)
        {
            DataSet dsUOMList = DataAccess.UOMManagement.UOMMaster.GetUOMTypeList(grid, bizUnit);
            string jString = string.Empty;
            if (dsUOMList.Tables.Count > 1 && dsUOMList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsUOMList);
            }
            return jString;
        }

        /// <summary>
        /// Delete UOM  
        /// </summary>
        /// <param name="UOMId"></param>
        /// <returns>string</returns>
        public static string DeleteUOM(int UOMId)
        {
            return DataAccess.UOMManagement.UOMMaster.DeleteUOM(UOMId);
        }

        /// <summary>
        /// Delete UOM Type
        /// </summary>
        /// <param name="UOMTypeId"></param>
        /// <returns>string</returns>
        public static string DeleteUOMType(int UOMTypeId)
        {
            return DataAccess.UOMManagement.UOMMaster.DeleteUOMType(UOMTypeId);
        }

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetUOMSearchValues(string searchBy, string searchValue, int bizUnit)
        {
            DataTable dtSearch = DataAccess.UOMManagement.UOMMaster.GetSearchValues(searchBy, searchValue, bizUnit);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.UOM.Fields.SEARCHVAL, GTIService.Constants.UOM.Fields.SEARCHPK);
            return jString;
        }

        /// <summary>
        /// Get UOM Details By UOMPK
        /// </summary>
        /// <param name="machineID"></param>
        /// <returns>string</returns>
        public static string GetUOMDetails(int uOMPK)
        {

            return GTIService.CommonFunctions.XmlToJson(DataAccess.UOMManagement.UOMMaster.GetUOMDetails(uOMPK));
        }

        /// <summary>
        ///  Get given UOM conversion factered UOM's.
        /// </summary>
        /// <param name="machineID"></param>
        /// <returns>string</returns>
        public static string GetUOMConversionsByUOMPK(int uOMPK)
        {
             DataTable dtSearch =DataAccess.UOMManagement.UOMMaster.GetUOMConversionsByUOMPK(uOMPK);
             string jString = GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.UOM.Fields.UOMCODE, GTIService.Constants.UOM.Fields.UOMPK);
             return jString;
        }

        #endregion
    }
}
