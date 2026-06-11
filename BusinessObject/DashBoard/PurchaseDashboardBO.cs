using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.DashBoard
{
    public class PurchaseDashboardBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class PurchaseDashboardHdr
    {
        [XmlElement("VendorDetails")]
        public List<VendorDetails> VendorList { get; set; }
    }
    [Serializable]
    public class VendorDetails
    {
        [XmlElement("VEN_NAME")]
        public string VEN_NAME { get; set; }
        [XmlElement("VEN_PK")]
        public int VEN_PK { get; set; }
        [XmlElement("VEN_CODE")]
        public string VEN_CODE { get; set; }
        [XmlElement("PODetails")]
        public List<PODetails> POList { get; set; }
    }
    [Serializable]
    public class PODetails
    {
        [XmlElement("POH_NO")]
        public string POH_NO { get; set; }
        [XmlElement("POH_PK")]
        public int POH_PK { get; set; }
        [XmlElement("PRDetails")]
        public List<PRDetails> PRList { get; set; }
    }   
    [Serializable]
    public class PRDetails
    {
        [XmlElement("PRH_NO")]
        public string PRH_NO { get; set; }
        [XmlElement("PRH_PK")]
        public int PRH_PK { get; set; }
    }

    [Serializable]
    [XmlRoot("root")]
    public class POHeader : PODetails
    {
        [XmlElement("PurchaseOrderList")]
        public POItems POItems { get; set; }
    }
    [Serializable]
    public class POItems
    {
        [XmlElement("PODetails")]
        public List<POItemDetails> POItemDetails { get; set; }
    }
    [Serializable]
    public class POItemDetails
    {
        [XmlElement("POD_PK")]
        public int POD_PK { get; set; }
        [XmlElement("POD_PO")]
        public int POD_PO { get; set; }
        [XmlElement("POD_DATE")]
        public DateTime POD_DATE { get; set; }
        [XmlElement("POD_ITEM")]
        public int POD_ITEM { get; set; }
        [XmlElement("ITM_NAME")]
        public string ITM_NAME { get; set; }
        [XmlElement("ITM_CODE")]
        public string ITM_CODE { get; set; }
        [XmlElement("ITM_TEXT")]
        public string ITM_TEXT { get; set; }
        [XmlElement("POD_QTY_REQUESTED")]
        public double POD_QTY_REQUESTED { get; set; }
        [XmlElement("POD_UOM")]
        public int POD_UOM { get; set; }
        [XmlElement("POD_CONV_FACT")]
        public double POD_CONV_FACT { get; set; }
        [XmlElement("POD_RATE")]
        public double POD_RATE { get; set; }
        [XmlElement("POD_AMOUNT")]
        public double POD_AMOUNT { get; set; }
        [XmlElement("POD_TAX")]
        public double POD_TAX { get; set; }
        [XmlElement("POD_DISC_AMT")]
        public double POD_DISC_AMT { get; set; }
        [XmlElement("POD_AMT_VALUE")]
        public double POD_AMT_VALUE { get; set; }
        [XmlElement("POD_REMARKS")]
        public string POD_REMARKS { get; set; }
        [XmlElement("UOM_CODE")]
        public string UOM_CODE { get; set; }
        [XmlElement("PODetails")]
        public List<PODetails> POList { get; set; }
    }
}
