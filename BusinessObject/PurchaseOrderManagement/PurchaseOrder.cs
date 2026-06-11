using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.PurchaseOrderManagement
{
    public class PurchaseOrder
    {
        public int POH_PK { get; set; }
        public string POH_NO { get; set; }
        public DateTime POH_DATE { get; set; }
        public int POH_SHIPPING { get; set; }
        public int POH_BILLING { get; set; }
        public List<PurchaseOrderMaterials> MaterialDetails { get; set; }
    }
   [Serializable]
   [XmlRoot("root")]
    public class PODetails
    {
        public int POH_PK { get; set; }
        public string POH_NO { get; set; }
        public string POH_DATE { get; set; }
        public string VEN_NAME { get; set; }
    }



    public class PurchaseOrderMaterials
    {
        public int POMaterialID { get; set; }
        public int POH_PK { get; set; }
        public int POD_ITEM { get; set; }
        public string ITV_NAME { get; set; }
        public string ITM_CODE { get; set; }
        public float BaseConversion { get; set; }
        public float POD_RATE { get; set; }
        public float POD_QTY_REQUESTED { get; set; }
        public int POD_UOM { get; set; }
        public string POD_UOM_TEXT { get; set; }
        public float POD_DISC_AMT { get; set; }
        public string POD_REMARKS { get; set; }
        public float POD_AMT_VALUE { get; set; }
    }

    public enum POItemType
    {
        General = 0,
        Latex,
        Chemical,
        Coal,
        Others,
        PackingCartons,
        PackingOthers,
        Cash,
        Credit,
        Former,
    }
}
