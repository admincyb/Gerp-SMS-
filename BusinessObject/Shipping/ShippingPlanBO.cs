using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Shipping
{
    [Serializable]
    [XmlRoot("Root")]
    public class ShippingPlanBO
    {
        [XmlElement("SNH_PK")]
        public int SNH_PK { get; set; }
        [XmlElement("SNH_NO")]
        public string SNH_NO { get; set; }
        [XmlElement("SNH_DATE")]
        public DateTime SNH_DATE { get; set; }
        [XmlElement("SNH_CUSTOMER")]
        public int SNH_CUSTOMER { get; set; }
        [XmlElement("SNH_SHIP_AGENT")]
        public int SNH_SHIP_AGENT { get; set; }
        [XmlElement("SNH_LOADING_DATE")]
        public string SNH_LOADING_DATE { get; set; }
        [XmlElement("SNH_SHIP_TO_PORT")]
        public string SNH_SHIP_TO_PORT { get; set; }
        [XmlElement("SNH_CONTAINER_TYPE")]
        public string SNH_CONTAINER_TYPE { get; set; }
        [XmlElement("SNH_ETD")]
        public DateTime SNH_ETD { get; set; }
        [XmlElement("SNH_DESC")]
        public string SNH_DESC { get; set; }
        [XmlElement("SNH_PLAN_QTY")]
        public double SNH_PLAN_QTY { get; set; }
        [XmlElement("SNH_CTN_QTY")]
        public double SNH_CTN_QTY { get; set; }
        [XmlElement("SNH_TRX_STATUS")]
        public int SNH_TRX_STATUS { get; set; }
        [XmlElement("SNH_STATUS")]
        public int SNH_STATUS { get; set; }
        [XmlElement("SNH_DEPT")]
        public int SNH_DEPT { get; set; }
        [XmlElement("SNH_COMPANY")]
        public int SNH_COMPANY { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("WKF_PROCESS")]
        public int WKF_PROCESS { get; set; }

        [XmlElement("SC_VALIDATION")]
        public int SC_VALIDATION { get; set; }
       

        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("SNH_BOOKING_NO")]
        public string SNH_BOOKING_NO { get; set; }
        [XmlElement("SNH_FEEDER_VESSEL")]
        public string SNH_FEEDER_VESSEL { get; set; }
        [XmlElement("SNH_MOTHER_VESSEL")]
        public string SNH_MOTHER_VESSEL { get; set; }
        [XmlElement("SNH_CLOSE_DATE")]
        public string SNH_CLOSE_DATE { get; set; }
        [XmlElement("SNH_REMARKS")]
        public string SNH_REMARKS { get; set; }
        [XmlElement("SNH_ETA")]
        public string SNH_ETA { get; set; }
        [XmlElement("Detail")]
        public List<ShippingDetails> ShippingDetails { get; set; }
        [XmlElement("DocDetails")]
        public List<ShippingPlanUploadDetails> DocDetails { get; set; }
        [XmlElement("SNH_CONTAINER_NO")]
        public string SNH_CONTAINER_NO { get; set; }
    }

    [Serializable]
    public class ShippingDetails
    {
        [XmlElement("SND_PK")]
        public int SND_PK { get; set; }
        [XmlElement("SND_SOD")]
        public int SND_SOD { get; set; }
        [XmlElement("SND_PLAN_QTY")]
        public double SND_PLAN_QTY { get; set; }
        [XmlElement("SND_CTN_QTY")]
        public double SND_CTN_QTY { get; set; }
        [XmlElement("SND_CBM")]
        public double SND_CBM { get; set; }
        [XmlElement("SND_ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("SND_MOD_BY")]
        public int SND_MOD_BY { get; set; }
        [XmlElement("SND_MOD_DT")]
        public DateTime SND_MOD_DT { get; set; }
        [XmlElement("SND_SALE_QTY")]
        public double SND_SALE_QTY { get; set; }
        [XmlElement("SND_SALE_UOM")]
        public int SND_SALE_UOM { get; set; }
        [XmlElement("SND_SALE_UOM_CONV")]
        public double SND_SALE_UOM_CONV { get; set; }
        [XmlElement("SND_BOI_STATUS")]
        public int SND_BOI_STATUS { get; set; }
        
    }


    [Serializable]
    public class ShippingPlanUploadDetails
    {
        [XmlElement("SCD_PK")]
        public int SCD_PK { get; set; }
        [XmlElement("SCD_FILE")]
        public string SCD_FILE { get; set; }
        [XmlElement("SCD_TYPE")]
        public int SCD_TYPE { get; set; }
        [XmlElement("SCD_FILE_PATH")]
        public string SCD_FILE_PATH { get; set; }
        [XmlElement("SCD_TITLE")]
        public string SCD_TITLE { get; set; }
        [XmlElement("SCD_DESC")]
        public string SCD_DESC { get; set; }
        [XmlElement("SCD_SEQUENCE")]
        public byte SCD_SEQUENCE { get; set; }
        [XmlElement("SCD_ACTIVE")]
        public byte SCD_ACTIVE { get; set; }
        public string FileExtension { get; set; }
        public string AttachmentFileName { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class ShippingPlanOrder
    {
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BizPk { get; set; }
        [XmlElement("SALORDER")]
        public List<SaleOrderPK> SaleOrderPKs { get; set; }
    }

    [Serializable]
    public class SaleOrderPK
    {
        [XmlElement("SOH_PK")]
        public long SOH_PK { get; set; }
    }

    [Serializable]
    [XmlRoot("FilterParameters")]
    public class QustionNaireBO
    {
        [XmlElement("FromDate")]
        public string FromDate { get; set; }
        [XmlElement("ToDate")]
        public string ToDate { get; set; }
        [XmlElement("BizUnit")]
        public int BizUnit { get; set; }
        [XmlElement("UserPK")]
        public int UserPK { get; set; }
        [XmlElement("Dept")]
        public int Dept { get; set; }
        [XmlElement("CusPK")]
        public int CusPK { get; set; }
        [XmlElement("QstPK")]
        public int QstPK { get; set; }
    }

    [Serializable]
    public class CustomerBrands
    {
        public int SOH_PK { get; set; }
        public string SOH_NO { get; set; }
        public int SOD_PK { get; set; }
        public string SOD_BRAND_NAME { get; set; }
        public double SOD_QTY_DISPATCHED { get; set; }
        public double SOD_QTY { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class ShippingHeaderSO
    {
        [XmlElement("SO")]
        public List<ShippingSO> SOList { get; set; }
        [XmlElement("IS_DISCOUNT_EXIST")]
        public int IS_DISCOUNT_EXIST { get; set; }
    }

    [Serializable]
    public class ShippingSO
    {
        [XmlElement("SOH_PK")]
        public int SOH_PK { get; set; }
    }
}
