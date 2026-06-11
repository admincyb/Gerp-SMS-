using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.PurchaseOrderManagement
{
    public class RequestForQuote
    {

    }
    
    public class RFQPurchaseRequest
    {
        public int SerialNo { get; set; }
        public int PRDetailPK { get; set; }
        public string PRHeaderNo { get; set; }
        public int PRDetailItem { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemText { get; set; }
        public string ItemDesc { get; set; }
        public string PRDetailSpec { get; set; }
        public string PRDetailDate { get; set; }
        public string PRDetailUOMText { get; set; }
        public int PRDetailUOM { get; set; }
        public decimal PRDetailBalanceQty { get; set; }
        public string PRDetailReqDate { get; set; }
        public string PRDeptText { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class RFQResponseHeader
    {
        [XmlElement("RRH_PK")]
        public int RRH_PK { get; set; }
        [XmlElement("RRH_NO")]
        public string RRH_NO { get; set; }
        [XmlElement("RRH_VERSION")]
        public int RRH_VERSION { get; set; }
        [XmlElement("RRH_DATE")]
        public string RRH_DATE { get; set; }
        [XmlElement("RRH_RFQ_HDR")]
        public int RRH_RFQ_HDR { get; set; }
        [XmlElement("RRH_RFQ_NO")]
        public string RRH_RFQ_NO { get; set; }
        [XmlElement("RRH_VENDOR")]
        public int RRH_VENDOR { get; set; }
        [XmlElement("RRH_VENDOR_TEXT")]
        public string RRH_VENDOR_TEXT { get; set; }
        [XmlElement("RRH_VEN_REF_NO")]
        public string RRH_VEN_REF_NO { get; set; }
        [XmlElement("RRH_STATUS")]
        public short RRH_STATUS { get; set; }
        [XmlElement("RRH_CURRENCY")]
        public int RRH_CURRENCY { get; set; }
        [XmlElement("RRH_CURRENCY_TEXT")]
        public string RRH_CURRENCY_TEXT { get; set; }
        [XmlElement("RRH_TOTAL_QTY")]
        public double RRH_TOTAL_QTY { get; set; }
        [XmlElement("RRH_AMT_SUB_TOTAL")]
        public double RRH_AMT_SUB_TOTAL { get; set; }
        [XmlElement("RRH_AMT_DISC")]
        public double RRH_AMT_DISC { get; set; }
        [XmlElement("RRH_AMT_TAX")]
        public double RRH_AMT_TAX { get; set; }
        [XmlElement("RRH_AMT_SHIP_CHARGE")]
        public double RRH_AMT_SHIP_CHARGE { get; set; }
        [XmlElement("RRH_AMT_ADJUST")]
        public double RRH_AMT_ADJUST { get; set; }
        [XmlElement("RRH_AMT_NET_TOTAL")]
        public double RRH_AMT_NET_TOTAL { get; set; }
        [XmlElement("RRH_BASE_CURR")]
        public int RRH_BASE_CURR { get; set; }
        [XmlElement("RRH_EXCHG_RATE")]
        public double RRH_EXCHG_RATE { get; set; }
        [XmlElement("RRH_AMT_NET_TOTAL_BC")]
        public double RRH_AMT_NET_TOTAL_BC { get; set; }
        [XmlElement("RRH_PAYMENT_TERMS")]
        public string RRH_PAYMENT_TERMS { get; set; }
        [XmlElement("RRH_DELIVERY_TERMS")]
        public string RRH_DELIVERY_TERMS { get; set; }
        [XmlElement("RRH_OTHER_DETAILS")]
        public string RRH_OTHER_DETAILS { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("RRH_DEPT")]
        public short RRH_DEPT { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public short BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public byte ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public short USER_PK { get; set; }
        [XmlElement("RRH_COMPANY")]
        public string RRH_COMPANY { get; set; }
        [XmlElement("ResponseDtl")]
        public List<RFQResponseDetails> ResponseDtl { get; set; }
        [XmlElement("TaxHdr")]
        public List<RFQTaxHdr> TaxHdr { get; set; }
    }
    [Serializable]
    public class RFQResponseDetails
    {
        [XmlElement("RRD_PK")]
        public int RRD_PK { get; set; }
        [XmlElement("RRD_RESP_HDR")]
        public int RRD_RESP_HDR { get; set; }
        [XmlElement("RRD_RFQ_DTL")]
        public int RRD_RFQ_DTL { get; set; } 
        [XmlElement("RRD_SL_NO")]
        public int RRD_SL_NO { get; set; }
        [XmlElement("RRD_ITEM")]
        public int RRD_ITEM { get; set; }
        [XmlElement("RRD_ITEM_TEXT")]
        public string RRD_ITEM_TEXT { get; set; }
        [XmlElement("RRD_ITEM_DESC")]
        public string RRD_ITEM_DESC { get; set; }
        [XmlElement("RRD_ITEM_SPEC")]
        public string RRD_ITEM_SPEC { get; set; }
        [XmlElement("RRD_QTY_REQUESTED")]
        public double RRD_QTY_REQUESTED { get; set; }
        [XmlElement("RRD_UOM")]
        public int RRD_UOM { get; set; }
        [XmlElement("RRD_UOM_TEXT")]
        public string RRD_UOM_TEXT { get; set; }
        [XmlElement("RRD_RATE")]
        public double RRD_RATE { get; set; }
        [XmlElement("RRD_AMOUNT")]
        public double RRD_AMOUNT { get; set; }
        [XmlElement("RRD_AMT_DISC")]
        public double RRD_AMT_DISC { get; set; }
        [XmlElement("RRD_AMT_TAX")]
        public double RRD_AMT_TAX { get; set; }
        [XmlElement("RRD_AMT_NET_TOTAL")]
        public double RRD_AMT_NET_TOTAL { get; set; }
        [XmlElement("TaxDtl")]
        public List<RFQTaxHdr> TaxDtl { get; set; } 
    }

    [Serializable]
    public class RFQTaxHdr
    {
        [XmlElement("RTD_PK")]
        public int RTD_PK { get; set; }
        [XmlElement("RTD_RESP_DTL")]
        public int RTD_RESP_DTL { get; set; }
        [XmlElement("RTD_TYPE")]
        public short RTD_TYPE { get; set; }
        [XmlElement("RTD_TAX")]
        public int RTD_TAX { get; set; }
        [XmlElement("RTD_SL_NO")]
        public int RTD_SL_NO { get; set; }
        [XmlElement("RTD_TAX_TEXT")]
        public string RTD_TAX_TEXT { get; set; }
        [XmlElement("RTD_TAX_FORMULA")]
        public string RTD_TAX_FORMULA { get; set; }
        [XmlElement("RTD_NAME")]
        public string RTD_NAME { get; set; }
        [XmlElement("RTD_TAX_AMT")]
        public double RTD_TAX_AMT { get; set; }
        [XmlElement("RTD_TAX_CATEGORY_TEXT")]
        public string RTD_TAX_CATEGORY_TEXT { get; set; }
        [XmlElement("RTD_TAX_CATEGORY")]
        public int RTD_TAX_CATEGORY { get; set; }
    }

    //[Serializable]
    //public class RFQTaxDtl
    //{
    //    [XmlElement("RTD_PK")]
    //    public int RTD_PK { get; set; }
    //    [XmlElement("RTD_TYPE")]
    //    public short RTD_TYPE { get; set; }
    //    [XmlElement("RTD_TAX")]
    //    public int RTD_TAX { get; set; }
    //    [XmlElement("RTD_TAX_TEXT")]
    //    public string RTD_TAX_TEXT { get; set; }
    //    [XmlElement("RTD_TAX_FORMULA")]
    //    public string RTD_TAX_FORMULA { get; set; }
    //    [XmlElement("RTD_NAME")]
    //    public string RTD_NAME { get; set; }
    //    [XmlElement("RTD_TAX_AMT")]
    //    public double RTD_TAX_AMT { get; set; }
    //    [XmlElement("RTD_TAX_CATEGORY_TEXT")]
    //    public string RTD_TAX_CATEGORY_TEXT { get; set; }
    //}

    //public class RFQTaxSplit
    //{
    //    public int RRD_PK { get; set; }
    //    public int RTD_PK { get; set; }
    //    public short RTD_TYPE { get; set; }
    //    public int RTD_TAX { get; set; }
    //    public string RTD_TAX_TEXT { get; set; }
    //    public string RTD_TAX_FORMULA { get; set; }
    //    public string RTD_NAME { get; set; }
    //    public double RTD_TAX_AMT { get; set; }
    //    public string RTD_TAX_CATEGORY_TEXT { get; set; }
    //}


    /// <summary>
    /// RFQ Parameters for Item Vendors and PR Items
    /// </summary>
    [Serializable]
    [XmlRoot("Root")]
    public class RFQParameters
    {
        [XmlElement("ItemDetails")]
        public List<RFQItemParameter> ItemParameters { get; set; }
        [XmlElement("VendorDetails")]
        public List<RFQVendorParameter> VendorParameters { get; set; }
    }

    public class RFQItemParameter
    {
        public int ITM_PK { get; set; }
        [XmlIgnore]
        public int? PRD_PK { get; set; }
        [XmlIgnore]
        public int PRD_UOM { get; set; }
        [XmlIgnore]
        public decimal PRD_Qty { get; set; }
        [XmlIgnore]
        public string PRD_Spec { get; set; }
        [XmlIgnore]
        public DateTime? PRD_ReqDate { get; set; }
    }

    public class RFQVendorParameter
    {
        public int VEN_PK { get; set; }
    }

    public class RFQSelectedItem
    {
        public int RFD_PK { get; set; }
        public int RFD_SL_NO { get; set; }
        public int RFD_ITEM { get; set; }
        public string RFD_ITEM_TEXT { get; set; }
        public string RFD_ITEM_DESC { get; set; }
        public string RFD_ITEM_SPEC { get; set; }
        public DateTime RFD_REQD_DATE { get; set; }
        public decimal RFD_QTY_REQUESTED { get; set; }
        public int RFD_UOM { get; set; }
        public string RFD_UOM_TEXT { get; set; }
        public string RFH_COMPANY { get; set; }
        [XmlElement("RFQPRDetail")]
        public List<RFQSelectedItemPR> PRDetails{get;set;}
    }

    public class RFQSelectedVendor
    {
        public int RVM_PK { get; set; }
        public int RVM_VENDOR { get; set; }
        public string RVM_VENDOR_CODE { get; set; }
        public string RVM_VENDOR_NAME { get; set; }
        public string RVM_VENDOR_LOCATION { get; set; }
        public string RVM_VENDOR_PHONE { get; set; }
        public string RVM_VENDOR_EMAIL { get; set; }
    }

    public class RFQSelectedItemPR
    {
        public int RRM_PK { get; set; }
        public int RRM_RFQ_DTL { get; set; }
        public int RRM_PR_DTL { get; set; }
        public int RRM_ITEM { get; set; }
        public int RRM_UOM { get; set; }
        public int RRM_SL_NO { get; set; }
        public decimal RRM_QTY_REQUESTED { get; set; }
        public string RRM_ITEM_SPEC { get; set; }
        public DateTime? RRM_REQD_DATE { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class RFQHeader
    {
        public int RFH_PK { get; set; }
        public string RFH_NO { get; set; }
        public DateTime RFH_DATE { get; set; }
        public decimal RFH_TOTAL_QTY { get; set; }
        public int RFH_VERSION { get; set; }
        public string RFH_PAYMENT_TERMS { get; set; }
        public string RFH_DELIVERY_TERMS { get; set; }
        public string RFH_OTHER_DETAILS { get; set; }
        public int RFH_STATUS { get; set; }
        public int BIZUNIT_PK { get; set; }
        public int RFH_DEPT { get; set; }
        public int ACTIVE { get; set; }
        public int USER_PK { get; set; }
        public int? AST_PK { get; set; }
        public string RFH_COMPANY { get; set; }

        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("RFQDetail")]
        public List<RFQSelectedItem> ItemDetails { get; set; }
        [XmlElement("RFQVendorDetail")]
        public List<RFQSelectedVendor> VendorDetails { get; set; }

        public string APT_CODE { get; set; }
        public int WKF_FLAG { get; set; }
        public int AST_DOC_MODE { get; set; }

    }

    [Serializable]
    [XmlRoot("Root")]
    public class RFQSelectedDetails
    {
        [XmlElement("RFQDetail")]
        public List<RFQSelectedItem> ItemDetails { get; set; }
        [XmlElement("RFQVendorDetail")]
        public List<RFQSelectedVendor> VendorDetails { get; set; }
    }
    [Serializable]
    [XmlRoot("Root")]
    public class RFQDetailParameters
    {
        [XmlElement("PRDetails")]
        public List<PRParameter> PRParameters { get; set; }
        [XmlElement("VendorDetails")]
        public List<RFQVendorParameter> VendorParameters { get; set; }
        [XmlElement("ItemDetails")]
        public List<ItemParameter> ItemParameters { get; set; }
    }
    public class ItemParameter
    {
        public int RFD_ITEM { get; set; }
        public DateTime? RFD_REQD_DATE { get; set; }
        public string RFD_ITEM_SPEC { get; set; }
        public decimal RFD_QTY_REQUESTED { get; set; }
        public int RFD_UOM { get; set; }
    }
    public class PRParameter
    {
        public int PRD_PK { get; set; }
    }

    public enum WorkFlowStatus
    {
        DRAFT = 0,
        APPROVED = 2
    }
}
