using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.SaleOrder
{
    public class QuotationBO
    {

    }
    [Serializable]
    [XmlRoot("Root")]
    public class QuotationHeader
    {
        [XmlElement("CEH_PK")]
        public int CEH_PK { get; set; }
        [XmlElement("CEH_NO")]
        public string CEH_NO { get; set; }
        [XmlElement("CEH_VERSION")]
        public int CEH_VERSION { get; set; }
        [XmlElement("CEH_DATE")]
        public string CEH_DATE { get; set; }
        [XmlElement("CEH_BOOKING_DATE")]
        public string CEH_BOOKING_DATE { get; set; }
        [XmlElement("CEH_CUSTOMER")]
        public int CEH_CUSTOMER { get; set; }
        [XmlElement("CEH_CUSTOMER_TEXT")]
        public string CEH_CUSTOMER_TEXT { get; set; }
        [XmlElement("CEH_CUSTOMER_NAME")]
        public string CEH_CUSTOMER_NAME { get; set; }
        [XmlElement("CEH_REF_NO")]
        public string CEH_REF_NO { get; set; }
        [XmlElement("CEH_DESC")]
        public string CEH_DESC { get; set; }
        [XmlElement("CEH_REF_DATE")]
        public string CEH_REF_DATE { get; set; }
        [XmlElement("CEH_REQUIRED_DATE")]
        public string CEH_REQUIRED_DATE { get; set; }
        [XmlElement("CEH_VALID_FROM")]
        public string CEH_VALID_FROM { get; set; }
        [XmlElement("CEH_VALID_TO")]
        public string CEH_VALID_TO { get; set; }
        [XmlElement("CEH_SHIPPING_TO")]
        public string CEH_SHIPPING_TO { get; set; }
        [XmlElement("CEH_SHIPPING_TO_TEXT")]
        public string CEH_SHIPPING_TO_TEXT { get; set; }
        [XmlElement("CEH_SHIPPING_ADDRESS")]
        public string CEH_SHIPPING_ADDRESS { get; set; }
        [XmlElement("CEH_REMARKS")]
        public string CEH_REMARKS { get; set; }
        [XmlElement("CEH_CURRENCY_BASE")]
        public int CEH_BASE_CURR { get; set; }
        [XmlElement("CEH_CURRENCY")]
        public int CEH_CURRENCY { get; set; }
        [XmlElement("CEH_CURRENCY_TEXT")]
        public string CEH_CURRENCY_TEXT { get; set; }
        [XmlElement("CEH_CURRENCY_RATE")]
        public double CEH_CURRENCY_RATE { get; set; }
        [XmlElement("CEH_TOTAL_QTY")]
        public double CEH_TOTAL_QTY { get; set; }
        [XmlElement("CEH_NET_AMOUNT_BC")]
        public double CEH_NET_AMOUNT_BC { get; set; }
        [XmlElement("CEH_TOTAL_AMT")]
        public decimal CEH_TOTAL_AMT { get; set; }
        [XmlElement("CEH_TOTAL_DISCOUNT")]
        public double CEH_TOTAL_DISCOUNT { get; set; }
        [XmlElement("CEH_TOTAL_TAX")]
        public double CEH_TOTAL_TAX { get; set; }
        [XmlElement("CEH_NET_AMOUNT")]
        public double CEH_NET_AMOUNT { get; set; }
        [XmlElement("CEH_STATUS")]
        public short CEH_STATUS { get; set; }
        [XmlElement("CEH_TRX_STATUS")]
        public short CEH_TRX_STATUS { get; set; }
        [XmlElement("CEH_DELETED")]
        public short CEH_DELETED { get; set; }
        [XmlElement("CEH_ACTIVE")]
        public byte CEH_ACTIVE { get; set; }
        [XmlElement("CEH_SUBMITTED_BY")]
        public short CEH_SUBMITTED_BY { get; set; }
        [XmlElement("CEH_SUBMITTED_BY_TEXT")]
        public string CEH_SUBMITTED_BY_TEXT { get; set; }
        [XmlElement("CEH_SUBMITTED_DATE")]
        public string CEH_SUBMITTED_DATE { get; set; }
        [XmlElement("CEH_APPROVED_BY")]
        public short CEH_APPROVED_BY { get; set; }
        [XmlElement("CEH_APPROVED_BY_TEXT")]
        public string CEH_APPROVED_BY_TEXT { get; set; }
        [XmlElement("CEH_APPROVED_DATE")]
        public string CEH_APPROVED_DATE { get; set; }
        [XmlElement("CEH_DEPT")]
        public short CEH_DEPT { get; set; }
        [XmlElement("CEH_DEPT_TEXT")]
        public string CEH_DEPT_TEXT { get; set; }
        [XmlElement("CEH_BIZUNIT")]
        public short CEH_BIZUNIT { get; set; }
        [XmlElement("CEH_BIZUNIT_TEXT")]
        public string CEH_BIZUNIT_TEXT { get; set; }
        [XmlElement("CEH_CRTD_BY")]
        public short CEH_CRTD_BY { get; set; }
        [XmlElement("CEH_CRTD_DT")]
        public DateTime CEH_CRTD_DT { get; set; }
        [XmlElement("CEH_MOD_BY")]
        public short CEH_MOD_BY { get; set; }
        [XmlElement("CEH_MOD_DT")]
        public DateTime CEH_MOD_DT { get; set; }
        [XmlElement("EnquiryDetail")]
        public List<QuotationDetails> QuotationDtl { get; set; }
        [XmlElement("TaxHdr")]
        public List<QuotationTaxHdr> TaxHdr { get; set; }
        [XmlElement("CEH_TOTAL_SHIP_CHARGE")]
        public double CEH_TOTAL_SHIP_CHARGE { get; set; }
        [XmlElement("CEH_TOTAL_ADJUST")]
        public double CEH_TOTAL_ADJUST { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("USER_PK")]
        public short USER_PK { get; set; }
        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }

        //[XmlElement("RRH_RFQ_HDR")]
        //public int RRH_RFQ_HDR { get; set; }
        //[XmlElement("RRH_RFQ_NO")]
        //public string RRH_RFQ_NO { get; set; }
        //[XmlElement("RRH_BASE_CURR")]
        //public int RRH_BASE_CURR { get; set; }        
        //[XmlElement("RRH_PAYMENT_TERMS")]
        //public string RRH_PAYMENT_TERMS { get; set; }
        //[XmlElement("RRH_DELIVERY_TERMS")]
        //public string RRH_DELIVERY_TERMS { get; set; }
        //[XmlElement("RRH_OTHER_DETAILS")]
        //public string RRH_OTHER_DETAILS { get; set; }

        [XmlElement("CEH_SHIP_BY")]
        public string CEH_SHIP_BY { get; set; }
        [XmlElement("CEH_SHIP_BY_TEXT")]
        public string CEH_SHIP_BY_TEXT { get; set; }
        [XmlElement("CEH_TO_PORT")]
        public string CEH_TO_PORT { get; set; }
        [XmlElement("CEH_TRANSHIPMENT")]
        public string CEH_TRANSHIPMENT { get; set; }
        [XmlElement("CEH_TRANSHIPMENT_TEXT")]
        public string CEH_TRANSHIPMENT_TEXT { get; set; }

        [XmlElement("CEH_DEL_TERM")]
        public string CEH_DEL_TERM { get; set; }
        [XmlElement("CEH_DEL_TERM_NAME")]
        public string CEH_DEL_TERM_NAME { get; set; }
        [XmlElement("CEH_DEL_TERM_TEXT")]
        public string CEH_DEL_TERM_TEXT { get; set; }
        [XmlElement("CEH_PAYMENT_TERM")]
        public string CEH_PAYMENT_TERM { get; set; }
        [XmlElement("CEH_PAYMENT_TERM_NAME")]
        public string CEH_PAYMENT_TERM_NAME { get; set; }
        [XmlElement("CEH_PAYMENT_TERM_TEXT")]
        public string CEH_PAYMENT_TERMS { get; set; }
        [XmlElement("CEH_SPECIAL_TERM")]
        public string CEH_SPECIAL_TERM { get; set; }
        [XmlElement("CEH_SPECIAL_TERM_NAME")]
        public string CEH_SPECIAL_TERM_NAME { get; set; }
        [XmlElement("CEH_SPECIAL_TERM_TEXT")]
        public string CEH_SPECIAL_TERM_TEXT { get; set; }
        [XmlElement("CEH_QUOTATION_FLAG")]
        public byte CEH_QUOTATION_FLAG { get; set; }
        [XmlElement("DRAFT_FLAG")]
        public byte DRAFT_FLAG { get; set; }
    }
        [Serializable]
        public class QuotationDetails
        {
            [XmlElement("CED_PK")]
            public int CED_PK { get; set; }
            [XmlElement("CED_ENQUIRY_HDR")]
            public int CED_ENQUIRY_HDR { get; set; }
            [XmlElement("CED_ENQUIRY_HDR_TEXT")]
            public string CED_ENQUIRY_HDR_TEXT { get; set; }
            [XmlElement("CED_VERSION")]
            public int CED_VERSION { get; set; }
            [XmlElement("CED_SL_NO")]
            public int CED_SL_NO { get; set; }
            [XmlElement("CED_CUST_ITEM")]
            public int CED_CUST_ITEM { get; set; }
            [XmlElement("CED_CUST_ITEM_TEXT")]
            public string CED_CUST_ITEM_TEXT { get; set; }
            [XmlElement("CED_CUST_ITEM_CODE")]
            public string CED_CUST_ITEM_CODE { get; set; }
            [XmlElement("CED_CIM_PCS_PER_IP")]
            public double CED_CIM_PCS_PER_IP { get; set; }
            [XmlElement("CED_CIM_PCS_PER_OP")]
            public double CED_CIM_PCS_PER_OP { get; set; }
            [XmlElement("CED_ITEM")]
            public int CED_ITEM { get; set; }
            [XmlElement("CED_ITEM_TEXT")]
            public string CED_ITEM_TEXT { get; set; }
            [XmlElement("CED_ENQ_QTY")]
            public double CED_ENQ_QTY { get; set; }
            [XmlElement("CED_UOM")]
            public int CED_UOM { get; set; }
            [XmlElement("CED_UOM_TEXT")]
            public string CED_UOM_TEXT { get; set; }
            [XmlElement("CED_PACKING_SPEC")]
            public int CED_PACKING_SPEC { get; set; }
            [XmlElement("CED_PACKING_SPEC_TEXT")]
            public string CED_PACKING_SPEC_TEXT { get; set; }
            [XmlElement("PACKING_TEXT")]
            public string PACKING_TEXT { get; set; }
            [XmlElement("APS_TOTAL_PCS")]
            public double APS_TOTAL_PCS { get; set; }
            [XmlElement("CED_REQUIRED_DATE")]
            public string CED_REQUIRED_DATE { get; set; }

            [XmlElement("CED_EXPECTED_RANGE")]
            public string CED_EXPECTED_RANGE { get; set; }  
            [XmlElement("CED_EXP_MIN_RATE")]
            public double? CED_EXP_MIN_RATE { get; set; }
            [XmlElement("CED_EXP_MAX_RATE")]
            public double? CED_EXP_MAX_RATE { get; set; }  
                

            [XmlElement("CED_VALID_FROM")]
            public string CED_VALID_FROM { get; set; }
            [XmlElement("CED_VALID_TO")]
            public string CED_VALID_TO { get; set; }
            [XmlElement("CED_REMARKS")]
            public string CED_REMARKS { get; set; }
            [XmlElement("CED_CUST_QTY")]
            public double CED_CUST_QTY { get; set; }
            [XmlElement("CED_RATE")]
            public double CED_RATE { get; set; }
            [XmlElement("CED_AMOUNT")]
            public double CED_AMOUNT { get; set; }
            [XmlElement("CED_DISCOUNT")]
            public double CED_DISCOUNT { get; set; }
            [XmlElement("CED_TAX")]
            public double CED_TAX { get; set; }
            [XmlElement("CED_AMT_NET_TOTAL")]
            public double CED_AMT_NET_TOTAL { get; set; }
            [XmlElement("CED_COMMENTS")]
            public string CED_COMMENTS { get; set; }
            
            [XmlElement("CED_ACTION")]
            public short CED_ACTION { get; set; }
            [XmlElement("TaxDtl")]
            public List<QuotationTaxHdr> TaxDtl { get; set; }

            [XmlElement("CED_SALE_UOM_TEXT")]
            public string CED_SALE_UOM_TEXT { get; set; }
            [XmlElement("CED_SALE_QTY")]
            public double CED_SALE_QTY { get; set; }
            [XmlElement("CED_SALE_UOM")]
            public int CED_SALE_UOM { get; set; }
            [XmlElement("CED_SALE_UOM_CONV")]
            public double CED_SALE_UOM_CONV { get; set; }
            //[XmlElement("RRD_RFQ_DTL")]
            //public int RRD_RFQ_DTL { get; set; }
            //[XmlElement("RRD_ITEM_DESC")]
            //public string RRD_ITEM_DESC { get; set; }
            //[XmlElement("RRD_ITEM_SPEC")]
            //public string RRD_ITEM_SPEC { get; set; }
            //[XmlElement("RRD_AMT_NET_TOTAL")]
            //public double RRD_AMT_NET_TOTAL { get; set; }
            
        }

        [Serializable]
        public class QuotationTaxHdr
        {
            [XmlElement("ETD_PK")]
            public int ETD_PK { get; set; }
            [XmlElement("ETD_ENQUIRY_DTL")]
            public int ETD_ENQUIRY_DTL { get; set; }
            [XmlElement("ETD_TYPE")]
            public short ETD_TYPE { get; set; }
            [XmlElement("ETD_TAX")]
            public int ETD_TAX { get; set; }
            [XmlElement("ETD_SL_NO")]
            public int ETD_SL_NO { get; set; }
            [XmlElement("ETD_TAX_TEXT")]
            public string ETD_TAX_TEXT { get; set; }
            [XmlElement("ETD_TAX_FORMULA")]
            public string ETD_TAX_FORMULA { get; set; }
            [XmlElement("ETD_NAME")]
            public string ETD_NAME { get; set; }
            [XmlElement("ETD_TAX_AMT")]
            public double ETD_TAX_AMT { get; set; }
            [XmlElement("ETD_TAX_CATEGORY_TEXT")]
            public string ETD_TAX_CATEGORY_TEXT { get; set; }
            [XmlElement("ETD_TAX_CATEGORY")]
            public int ETD_TAX_CATEGORY { get; set; }

           
        }
}
