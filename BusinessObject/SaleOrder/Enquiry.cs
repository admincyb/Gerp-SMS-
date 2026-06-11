using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.SaleOrder
{
    public class Enquiry
    {

    }
    [Serializable]
    [XmlRoot("Root")]
    public class EnquiryHeader
    {
        [XmlElement("CEH_PK")]
        public int CEH_PK { get; set; }
        [XmlElement("CEH_CUSTOMER")]
        public int CEH_CUSTOMER { get; set; }
        [XmlElement("CEH_CUSTOMER_NAME")]
        public string CEH_CUSTOMER_NAME { get; set; }
        [XmlElement("CEH_CUSTOMER_TEXT")]
        public string CEH_CUSTOMER_TEXT { get; set; }
        [XmlElement("CEH_NO")]
        public string CEH_NO { get; set; }
        [XmlElement("CEH_DATE")]
        public string CEH_DATE { get; set; }
        [XmlElement("CEH_BOOKING_DATE")]
        public string CEH_BOOKING_DATE { get; set; }
        [XmlElement("CEH_REF_NO")]
        public string CEH_REF_NO { get; set; }
        [XmlElement("CEH_REF_DATE")]
        public string CEH_REF_DATE { get; set; }

        [XmlElement("CEH_VERSION")]
        public int CEH_VERSION { get; set; }
        [XmlElement("CEH_DESC")]
        public string CEH_DESC { get; set; }

        [XmlElement("CEH_FROM_PORT")]
        public string CEH_FROM_PORT { get; set; }
        [XmlElement("CEH_TO_PORT")]
        public string CEH_TO_PORT { get; set; }
        [XmlElement("CEH_TRANSHIPMENT")]
        public string CEH_TRANSHIPMENT { get; set; }
        [XmlElement("CEH_SHIP_BY")]
        public string CEH_SHIP_BY { get; set; }
        [XmlElement("CEH_SHIP_BY_TEXT")]
        public string CEH_SHIP_BY_TEXT { get; set; }
        [XmlElement("CEH_ORG_GOODS")]
        public string CEH_ORG_GOODS { get; set; }
        [XmlElement("CEH_BANK")]
        public string CEH_BANK { get; set; }
        [XmlElement("CEH_REFERENCE")]
        public string CEH_REFERENCE { get; set; }

        [XmlElement("CEH_DEL_TERM")]
        public string CEH_DEL_TERM { get; set; }
        [XmlElement("CEH_DEL_TERM_TEXT")]
        public string CEH_DEL_TERM_TEXT { get; set; }
        [XmlElement("CEH_PAYMENT_TERM")]
        public string CEH_PAYMENT_TERM { get; set; }
        [XmlElement("CEH_PAYMENT_TERM_TEXT")]
        public string CEH_PAYMENT_TERMS { get; set; }
        [XmlElement("CEH_SPECIAL_TERM")]
        public string CEH_SPECIAL_TERM { get; set; }
        [XmlElement("CEH_SPECIAL_TERM_TEXT")]
        public string CEH_SPECIAL_TERM_TEXT { get; set; }
        [XmlElement("CEH_SHIPPING_TO")]
        public string CEH_SHIPPING_TO { get; set; }
        [XmlElement("CEH_SHIPPING_ADDRESS")]
        public string CEH_SHIPPING_ADDRESS { get; set; }

        [XmlElement("CEH_CURRENCY")]
        public int CEH_CURRENCY { get; set; }
        [XmlElement("CEH_CURRENCY_BASE")]
        public int CEH_BASE_CURR { get; set; }
        [XmlElement("CEH_CURRENCY_RATE")]
        public double CEH_CURRENCY_RATE { get; set; }
        [XmlElement("CEH_CURRENCY_TEXT")]
        public string CEH_CURRENCY_TEXT { get; set; }

        [XmlElement("CEH_REMARKS")]
        public string CEH_REMARKS { get; set; }
        [XmlElement("CEH_TRX_STATUS")]
        public byte CEH_TRX_STATUS { get; set; }
        [XmlElement("CEH_STATUS")]
        public byte CEH_STATUS { get; set; }
        [XmlElement("CEH_DELETED")]
        public byte CEH_DELETED { get; set; }
        [XmlElement("CEH_SUBMITTED_BY")]
        public short CEH_SUBMITTED_BY { get; set; }
        [XmlElement("CEH_SUBMITTED_DATE")]
        public DateTime CEH_SUBMITTED_DATE { get; set; }
        [XmlElement("CEH_APPROVED_BY")]
        public short CEH_APPROVED_BY { get; set; }
        [XmlElement("CEH_APPROVED_DATE")]
        public DateTime CEH_APPROVED_DATE { get; set; }
        [XmlElement("CEH_DEPT")]
        public int CEH_DEPT { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public byte ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }
        [XmlElement("EnquiryDetail")]
        public List<EnquiryDetails> ProductDtl { get; set; }
    }
    [Serializable]
    public class EnquiryDetails
    {
        [XmlElement("CED_PK")]
        public int CED_PK { get; set; }
        [XmlElement("CED_VERSION")]
        public int CED_VERSION { get; set; }
        [XmlElement("CED_SL_NO")]
        public int CED_SL_NO { get; set; }
        [XmlElement("CED_CUST_ITEM")]
        public int CED_CUST_ITEM { get; set; }
        [XmlElement("CED_ITEM")]
        public int CED_ITEM { get; set; }
        [XmlElement("CED_ENQ_QTY")]
        public double CED_ENQ_QTY { get; set; }
        [XmlElement("CED_UOM")]
        public int CED_UOM { get; set; }
        [XmlElement("CED_PACKING_SPEC")]
        public string CED_PACKING_SPEC { get; set; }
        [XmlElement("APS_NAME")]
        public string APS_NAME { get; set; }

        [XmlElement("CIM_PACKING_SPEC_NAME")]
        public string CIM_PACKING_SPEC_NAME { get; set; }

        [XmlElement("PACKING_TEXT")]
        public string PACKING_TEXT { get; set; }
        [XmlElement("APS_TOTAL_PCS")]
        public double APS_TOTAL_PCS { get; set; }

        [XmlElement("CED_REQUIRED_DATE")]
        public string CED_REQUIRED_DATE { get; set; }
        [XmlElement("CED_RATE")]
        public double CED_RATE { get; set; }

        [XmlElement("CED_EXP_MIN_RATE")]
        public string CED_EXP_MIN_RATE { get; set; }
        [XmlElement("CED_EXP_MAX_RATE")]
        public string CED_EXP_MAX_RATE { get; set; }

        [XmlElement("CED_AMOUNT")]
        public double CED_AMOUNT { get; set; }        
        [XmlElement("CED_REMARKS")]
        public string CED_REMARKS { get; set; }
        [XmlElement("CIM_BRAND_NAME")]
        public string CIM_BRAND_NAME { get; set; }
        [XmlElement("CIM_BRAND_CODE")]
        public string CIM_BRAND_CODE { get; set; }
        [XmlElement("CIM_ITEM_TEXT")]
        public string CIM_ITEM_TEXT { get; set; }
        [XmlElement("CIM_ITEM_CODE")]
        public string CIM_ITEM_CODE { get; set; }
        [XmlElement("CIM_UOM_TEXT")]
        public string CIM_UOM_TEXT { get; set; }
        [XmlElement("CIM_PCS_PER_IP")]
        public string CIM_PCS_PER_IP { get; set; }
        [XmlElement("CIM_PCS_PER_OP")]
        public string CIM_PCS_PER_OP { get; set; }
        [XmlElement("CBM")]
        public double CBM { get; set; }
        [XmlElement("NET_WT")]
        public double NET_WT { get; set; }
        [XmlElement("CED_SALE_UOM_TEXT")]
        public string CED_SALE_UOM_TEXT { get; set; }
        [XmlElement("CED_SALE_QTY")]
        public double CED_SALE_QTY { get; set; }
        [XmlElement("CED_SALE_UOM")]
        public int CED_SALE_UOM { get; set; }
        [XmlElement("CED_SALE_UOM_CONV")]
        public double CED_SALE_UOM_CONV { get; set; }
    }
}
