using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Compound
{
    public class Procedures
    {
        public const string SP_GetPolymer = "SPPRD_POLYMER_MST_GET_KV";
        public const string SP_GetUOMConversionUOM = "SPINV_UOM_CONV_UOM_GET";
        public const string SP_GetConversionFactor ="SPINV_UOM_CONV_FACT";
        public const string SP_GetMaterialName = "SPPRD_COMP_ITM_GET_KV";
        public const string SP_GetMaterialCategoryName = "SPINV_ITEM_CATEGORY_COMP_GET_KV";
        public const string SP_GetMaterialUOM = "SPPRD_COMP_ITM_UOM_GET";
        public const string SP_SaveCompound = "SPPRD_COMP_MST_SAVE";
        public const string SP_GetCompoundDetails = "SPPRD_COMP_MST_GET";
        public const string SP_GetCompoundList = "SPPRD_COMP_MST_LIST";
        public const string SP_DeleteCompoundDtls = "SPPRD_COMP_MST_DELETE";
        public const string SP_CompoundAutoSearch = "SPPRD_COMP_MST_AUTO";
        public const string SP_FormulationTypeName = "SPPRD_PRODUCT_TYPE_MST_GET_KV";
        public const string SP_UOMDetailsofItem = "SPPRD_TOP_UP_UOM_GET_KV";
        public const string SP_ActivateInActivateCompoundDtls = "SPPRD_COMP_MST_ACTIVATE";
        
        public const string SP_UOMListWithConversionFactor = "SPPRD_COMP_UOM_LIST_GET";       
       
    }
}
