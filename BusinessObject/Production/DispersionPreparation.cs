using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Production
{
    public class DispersionPreparation
    {
        public int BCH_PK { get; set; }
        public float DTH_QUANTITY { get; set; }
        public int DTH_QTY_UOM { get; set; }
        public string  PLANNAME { get; set; }
        public int DTH_PLAN { get; set; }
        public string  DISPESION { get; set; }
        public int DTH_DISPERSION { get; set; }
        public int DTH_MACHINE { get; set; }
        public DateTime DTH_LOAD_TM { get; set; }
        public DateTime DTH_UNLOAD_TM { get; set; }
        public int DTH_MILL_HRS { get; set; }
        public int DTH_STATUS { get; set; }
        public int DTH_DEPT { get; set; }
        public List<MaterialListDetails> MaterialList { get; set; }
      }

    public class MaterialListDetails
    {
        public int DSD_PK { get; set; }
        public int DSD_ITEM { get; set; }
        public float DSD_QUANTITY { get; set; }
        public int DSD_QTY_UOM { get; set; }
        public float QTY_IN_STOCK { get; set; }
        public string UOM_NAME { get; set; }
        public int DSD_QTY_PERC { get; set; }

        public string DTD_ACTUAL_TSC { get; set; }
    }

}
