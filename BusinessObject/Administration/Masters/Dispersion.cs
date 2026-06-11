using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.DispersionManagement
{
    /// <summary>
    /// Dispersion information
    /// </summary>
    public class Dispersion
    {
        public string DSP_PK { get; set; }
        public string DSP_NAME { get; set; }
        public int DSP_EXP_TIME { get; set; }
        public string DSP_EXP_TM_UOM { get; set; }
        public int DSP_QUANTITY { get; set; }
        public string DSP_QTY_UOM { get; set; }
        public int DSP_PREP_TIME { get; set; }
        public string DSP_PREP_TM_UOM { get; set; }
        public string DSP_MACHINE_TYPE { get; set; }
        public int DSP_BIZUNIT { get; set; }
        public List<Material> Materials { get; set; }
        public List<ConversionInfo> ConversionList { get; set; }
    }
    /// <summary>
    /// Products included for  dispersion
    /// </summary>
    public class Material
    {
        public int DSD_PK { get; set; }
        public int DSD_ITEM { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCatagory { get; set; }
        public string MaterialCatagoryName{get;set;}
        public string DSD_QUANTITY { get; set; }
        public string DSD_QTY_UOM { get; set; }
        public string UOMQuantityText { get; set; }

    }

    public class ConversionInfo
    {
        public int UMC_UOM_TYPE { get; set; }
        public int UMC_FROM { get; set; }
        public int UMC_TO { get; set; }
        public double UMC_CONV_FACT { get; set; }
        public string FromUnitText { get; set; }
        public string ToUnitText { get; set; }
    }
}
