using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.CompoundManagement
{
    public class Compound
    {
        public int COM_PK { get; set; }
        public string COM_CODE { get; set; }
        public string COM_NAME { get; set; }
        public int COM_TYPE { get; set; }
        public int COM_POLYMER { get; set; }
        public float COM_QUANTITY { get; set; }
        public int COM_QTY_UOM { get; set; }
        public float COM_MAT_PRD { get; set; }
        public int COM_MAT_UOM { get; set; }
        public float COM_EXP_PRD { get; set; }
        public int COM_EXP_UOM { get; set; }
        public int SBU { get; set; }
        public int UserPk { get; set; }
        public List<CompoudMaterials> CompoundMaterialsList {get;set;}
        public List<ConversionInfo> ConversionList { get; set; }
    }


    public class CompoudMaterials
    {
        public int SL_NO { get; set; }
        public int CPD_PK { get; set; }
        public int CPD_ITEM_CATEGORY { get; set; }
        public string MATERIALTYPENAME { get; set; }
        public int CPD_ITEM { get; set; }
        public string MaterialName { get; set; }
        public float CPD_WET_QTY { get; set; }
        public int CPD_QTY_UOM { get; set; }
        public string MATERIALQUANTITYUOM { get; set; }
        public float CPD_DRY_PERC { get; set; }
        public string DRYWEIGHT { get; set; }
        public float CPD_DRY_QTY { get; set; }
        public string CompoundPercentage { get; set; }
        public float CPD_COMP_PERC { get; set; }
        public string ConversionValue { get; set; }
        public float ConvertedWt { get; set; }
        public int MaterialUOMType { get; set; } 

    }
    public class ConversionInfo 
    {

        public int UPC_PK { get; set; }
        public int UMC_UOM_TYPE { get; set; }
        public int UPC_FROM_UOM { get; set; }
        public int UPC_TO_UOM { get; set; }
        public double UPC_CONV_FACT { get; set; }
        public string FROM_UOM_NAME { get; set; }
        public string TO_UOM_NAME { get; set; }
    }
}
