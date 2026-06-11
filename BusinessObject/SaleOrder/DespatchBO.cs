using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace BusinessObject.Sales
{
    [Serializable]
    [XmlRoot("ROOT")]
    public class DespatchBO
    {
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("ACTIVE")]
        public string  Active { get; set; }
        [XmlElement("DPH_PK")]
        public string DespatchPK { get; set; }
        [XmlElement("PAGE_NO")]
        public string PageNo { get; set; }
    }

   

     [Serializable]
    [XmlRoot("ROOT")]
    public class DespatchReortBO
    {
        [XmlElement("DPH_PK")]
        public int  DPH_PK { get; set; }
        [XmlElement("DPH_DATE")]
        public DateTime DPH_DATE { get; set; }
        [XmlElement("DPH_NO")]
        public string  DPH_NO { get; set; }
        [XmlElement("DPH_STATUS")]
        public int DPH_STATUS { get; set; }
        [XmlElement("DPH_CUSTOMER")]
        public string DPH_CUSTOMER { get; set; }
        [XmlElement("DPH_REF_NO")]
        public int DPH_REF_NO { get; set; }
        [XmlElement("DPH_REF_DATE")]
        public string  DPH_REF_DATE { get; set; }
        [XmlElement("DPH_REMARKS")]
        public string DPH_REMARKS { get; set; }
        [XmlElement("BIZUNIT")]
        public int BIZUNIT { get; set; }
        [XmlElement("DEPT_PK")]
        public int DEPT_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("MODULE")]
        public int MODULE { get; set; }
        [XmlElement("MODE")]
        public int MODE { get; set; }

        [XmlElement("DETAILS")]
        public List<DespatchDetailBO> DespatchDetailsList { get; set; }
    }
     [Serializable]
    public class DespatchDetailBO
    {
        [XmlElement("DETAIL")]
        public List<DespatchDetailsBO> DespatchDetailList { get; set; }
    }
     [Serializable]
    public class DespatchDetailsBO
    {
        [XmlElement("DPH_PK")]
        public int DPH_PK { get; set; }
        [XmlElement("DPD_PK")]
        public int DPD_PK { get; set; }
        [XmlElement("DPD_SL_NO")]
        public int DPD_SL_NO { get; set; }
        [XmlElement("DPD_SALE_ORDER")]
        public int DPD_SALE_ORDER { get; set; }
        [XmlElement("DPD_SALE_ORDER_NO")]
        public string DPD_SALE_ORDER_NO { get; set; }
        [XmlElement("DPD_SALE_ORDER_DTL")]
        public int DPD_SALE_ORDER_DTL { get; set; }
        [XmlElement("DPD_ITEM")]
        public int DPD_ITEM { get; set; }
        [XmlElement("PRO_TEXT")]
        public string PRO_TEXT { get; set; }
        [XmlElement("DPD_QTY_DESPATCHED")]
        public double DPD_QTY_DESPATCHED { get; set; }
        [XmlElement("DPD_QTY_APPROVED")]
        public double DPD_QTY_APPROVED { get; set; }
        [XmlElement("DPD_UOM")]
        public int DPD_UOM { get; set; }
        [XmlElement("UOM_CODE")]
        public string UOM_CODE { get; set; }
        [XmlElement("PAGE_NO")]
        public string PageNo { get; set; }
        [XmlElement("SOD_BAL_TO_DISPATCH")]
        public double SOD_BAL_TO_DISPATCH { get; set; }
        [XmlElement("BIZUNIT")]
        public string  BizUnit { get; set; }
        [XmlElement("STORE")]
        public int STORE { get; set; }
        [XmlElement("DPD_REMARKS")]
        public String DPD_REMARKS { get; set; }


      
    }
    public class SessionString
    {
        public const string DespatchDetails = "DespatchDetails";
    }
}