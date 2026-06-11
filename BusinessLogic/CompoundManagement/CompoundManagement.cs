using System.Data;
using BusinessObject;

namespace BusinessLogic.CompoundManagement
{
    # region Methods
    public class CompoundManagement
    {
        /// <summary>
       /// Get Polymer Type Details To Combo
       /// </summary>
       /// <param name="sBU"></param>
       /// <returns></returns>
        public static string GetPolymerType(int sBU)
        {
            DataTable dtPolymer = DataAccess.CompoundManagement.CompoundMasterDL.GetPolymerType(sBU);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtPolymer, GTIService.Constants.Compound.Fields.POLYMERCODE, GTIService.Constants.Compound.Fields.POLYMERID);
            return jString;
        }

        /// <summary>
        /// Get Conversion UOM List
        /// </summary>
        /// <param name="uOM"></param>
        /// <returns></returns>
        public static string GetConversionUOMList(int uOM)
        {
            DataTable dtUOM = DataAccess.CompoundManagement.CompoundMasterDL.GetConversionUOMList(uOM);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtUOM, GTIService.Constants.Compound.Fields.UOMCODE, GTIService.Constants.Compound.Fields.UOMPK);
            return jString;

        }

        /// <summary>
       /// Get Conversion Factor 
       /// </summary>
       /// <param name="uomFrm"></param>
       /// <param name="uomTo"></param>
       /// <returns></returns>
        public static string GetConversionFactor(int uomFrm, int uomTo)
        {
            return DataAccess.CompoundManagement.CompoundMasterDL.GetConversionFactor(uomFrm, uomTo);
        }
     
        /// <summary>
        /// Get Material name By Material Category
        /// </summary>
        /// <param name="sBU"></param>
        /// <param name="catg"></param>
        /// <returns></returns>
        public static string GetMaterialName(int sBU, int catg)
        {
            DataTable dtMaterialName = DataAccess.CompoundManagement.CompoundMasterDL.GetMaterialName(sBU, catg);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialName, GTIService.Constants.Compound.Fields.NAME, GTIService.Constants.Compound.Fields.PK);
            return jString;

        }

        /// <summary>
        /// Get Material Type List, List are Raw Mateial, Compound and Dispersion
        /// </summary>
        /// <param name="sBU"></param>
        /// <returns></returns>
        public static string GetMaterialTypeName(int sBU)
        {
            DataTable dtMaterialTypeName = DataAccess.CompoundManagement.CompoundMasterDL.GetMaterialTypeName(sBU);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialTypeName, GTIService.Constants.Compound.Fields.CATGNAME, GTIService.Constants.Compound.Fields.CATGPK);
            return jString;

        }

        /// <summary>
        /// Get UOPM Type By Material PK
        /// </summary>
        /// <param name="materialPK"></param>
        /// <returns></returns>
        public static string GetUomTypeByMaterialPk(int materialPK)
        {
            DataTable dtUOMType = DataAccess.MaterialManagement.MaterialMasterDL.GetUomDtlsByMaterialPk(materialPK);
            if (dtUOMType.Rows[0][GTIService.Constants.Compound.Fields.UOMTYPENAME].ToString().ToUpper() == GTIService.Constants.Compound.Fields.WEIGHTTYPE)
                return "1";
            else
                return "0";
        }

        /// <summary>
        /// Save Compound Details
        /// </summary>
        /// <param name="compoundDtls"></param>
        /// <returns>string</returns>
        public static string SaveCompoundDtls(string compoundDtls)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(compoundDtls);
            return DataAccess.CompoundManagement.CompoundMasterDL.SaveCompoundDtls(xmlstr).ToString();
        }

        /// <summary>
        /// Get Compound List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>string</returns>
        public static string GetCompoundList(GridPrams grid, int bizUnit)
        {
            DataSet dsUOMList = DataAccess.CompoundManagement.CompoundMasterDL.GetCompoundList(grid, bizUnit);
            string jString = string.Empty;
            if (dsUOMList.Tables.Count > 1 && dsUOMList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsUOMList);
            }
            return jString;
        }

        /// <summary>
        /// Delete Compound Details 
        /// </summary>
        /// <param name="UOMId"></param>
        /// <returns>string</returns>
        public static string DeleteCompoundDetails(int UOMId)
        {
            return DataAccess.CompoundManagement.CompoundMasterDL.DeleteCompoundDtls(UOMId);
        }
        /// <summary>
        /// Activate Compound Details 
        /// </summary>
        /// <param name="compoundPK"></param>
        /// <returns>string</returns>
        public static string ActivateInactivateCompoundDtls(int compoundPK,int status,int userPK)
        {
            return DataAccess.CompoundManagement.CompoundMasterDL.ActivateInactivateCompoundDtls(compoundPK, status, userPK);
        }
        /// <summary>
        /// Get Search Details For Auto Complete
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetSearchValues(string searchBy, string searchValue, int bizUnit)
        {
            DataTable dtSearch = DataAccess.CompoundManagement.CompoundMasterDL.GetSearchValues(searchBy, searchValue, bizUnit);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.UOM.Fields.SEARCHVAL, GTIService.Constants.UOM.Fields.SEARCHPK);
            return jString;
        }

        /// <summary>
        /// Get Compund Details
        /// </summary>
        /// <param name="uOMPK"></param>
        /// <returns>string</returns>
        public static string GetCompoundDtls(int compoundPK)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.CompoundManagement.CompoundMasterDL.GetCompoundDetails(compoundPK));
        }

        /// <summary>
        /// get Formaulation type Name
        /// </summary>
        /// <param name="sBU"></param>
        /// <returns></returns>
        public static string GetFormulationTypeName(int sBU)
        {
            DataTable dtFormulationType = DataAccess.CompoundManagement.CompoundMasterDL.GetFormulationTypeName(sBU);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtFormulationType, GTIService.Constants.Compound.Fields.FORMULATIONNAME, GTIService.Constants.Compound.Fields.FORMULATIONPK);
            return jString;

        }

        /// <summary>
        /// Get UOm Detail for a Item
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="catg"></param>
        /// <param name="uOM"></param>
        /// <returns></returns>
        public static string GetUOMListForItem(int materialPK, int catg)
        {
            DataTable dtUOMList = DataAccess.CompoundManagement.CompoundMasterDL.GetUOMListForItem(materialPK, catg);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtUOMList, GTIService.Constants.Compound.Fields.UOMCODE, GTIService.Constants.Compound.Fields.UOMPK);
            return jString;

        }

        /// <summary>
        /// Get UOM List For Item and Compouund
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="catg"></param>
        /// <param name="uom"></param>
        /// <returns></returns>
        public static string GetUOMListWithCF(int materialPK, int catg, int uom)
        {
            DataTable dtUOMList = DataAccess.CompoundManagement.CompoundMasterDL.GetUOMListWithCF(materialPK, catg, uom);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtUOMList, GTIService.Constants.Compound.Fields.UOMCODE, GTIService.Constants.Compound.Fields.UOMPK);
            return jString;

        }

        /// <summary>
        /// Get Material Uom By Material PK and Category
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="catg"></param>
        /// <returns></returns>
        public static string GetMaterialUOM(int materialPK, int catg)
        {
            DataTable dtUOMList = DataAccess.CompoundManagement.CompoundMasterDL.GetMaterialUOM(materialPK, catg);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtUOMList, GTIService.Constants.Compound.Fields.NAME, GTIService.Constants.Compound.Fields.PK);
            return jString;

        }


    }

    #endregion

}
