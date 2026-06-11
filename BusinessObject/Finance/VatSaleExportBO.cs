using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
    public class VatSale
    {
        [XmlElement("TSH_PK")]
        public int TSH_PK { get; set; }
        [XmlElement("TSH_FROM_DATE")]
        public string TSH_FROM_DATE { get; set; }
        [XmlElement("TSH_TO_DATE")]
        public string TSH_TO_DATE { get; set; }
        [XmlElement("TSH_DESC")]
        public string TSH_DESC { get; set; }
        [XmlElement("TSH_STATUS")]
        public int TSH_STATUS { get; set; }
        [XmlElement("TSH_DEPT")]
        public int TSH_DEPT { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("Detail")]
        public List<VatSaleDetails> listVatSaleDetail { get; set; }
    }
    [Serializable]
    public class VatSaleDetails
    {
        [XmlElement("TSD_PK")]
        public int TSD_PK { get; set; }
        [XmlElement("TSD_TAX_TRX_HDR")]
        public string TSD_TAX_TRX_HDR { get; set; }
        [XmlElement("TSD_SL_NO")]
        public int TSD_SL_NO { get; set; }
        [XmlElement("TSD_INVOICE_HDR")]
        public int TSD_INVOICE_HDR { get; set; }
        [XmlElement("TSD_INVOICE_DATE")]
        public string TSD_INVOICE_DATE { get; set; }
        [XmlElement("TSD_INVOICE_NO")]
        public string TSD_INVOICE_NO { get; set; }
        [XmlElement("TSD_DECLARATION_NO")]
        public string TSD_DECLARATION_NO { get; set; }
        [XmlElement("TSD_CUSTOMER")]
        public int TSD_CUSTOMER { get; set; }
        [XmlElement("TSD_CUSTOMER_NAME")]
        public string TSD_CUSTOMER_NAME { get; set; }
        [XmlElement("TSD_ITEM_TEXT")]
        public string TSD_ITEM_TEXT { get; set; }
        [XmlElement("TSD_CURRENCY")]
        public int TSD_CURRENCY { get; set; }
        [XmlElement("TSD_CURRENCY_TEXT")]
        public string TSD_CURRENCY_TEXT { get; set; }
        [XmlElement("TSD_NET_VALUE_TC")]
        public double TSD_NET_VALUE_TC { get; set; }
        [XmlElement("TSD_BASE_CURR")]
        public string TSD_BASE_CURR { get; set; }
        [XmlElement("TSD_BASE_CURR_TEXT")]
        public string TSD_BASE_CURR_TEXT { get; set; }
        [XmlElement("TSD_EXCHG_RATE")]
        public double TSD_EXCHG_RATE { get; set; }
        [XmlElement("TSD_NET_VALUE_BC")]
        public double TSD_NET_VALUE_BC { get; set; }
    }
}
