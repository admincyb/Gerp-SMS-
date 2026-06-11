using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;

namespace BusinessObject.Sales
{
    [Serializable]
    [XmlRoot("ROOT")]
    public class SalesOrderBO
    {
        [XmlElement("SOH_PK")]
        public string PK { get; set; }
        [XmlElement("SOH_NO")]
        public string ScNoPK { get; set; }
        [XmlElement("SOH_REFERENCE")]
        public string SOH_REFERENCE { get; set; }
        [XmlElement("ACTIVE")]
        public string Active { get; set; }
        [XmlElement("SOD_PK")]
        public string dtlPK { get; set; }
        [XmlElement("SOH_CUSTOMER")]
        public string CustomerPK { get; set; }
        [XmlElement("PDG_PK")]
        public string ProductGroupPK { get; set; }
        [XmlElement("PDT_PK")]
        public string ProductTypePK { get; set; }
        [XmlElement("PRO_PK")]
        public string ProductPK { get; set; }
        [XmlElement("PRT_PK")]
        public string PriorityPK { get; set; }
        [XmlElement("SIZ_PK")]
        public string SizePK { get; set; }
        [XmlElement("CLR_PK")]
        public string ColorPK { get; set; }
        [XmlElement("REQUIRED_BY")]
        public string RequdBy { get; set; }
        [XmlElement("SOH_DATE_FROM")]
        public string RequdByFrom { get; set; }
        [XmlElement("SOH_DATE_TO")]
        public string RequdTo { get; set; }
        [XmlElement("ACT_TYPE")]
        public string ActivityType { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("DEPT_PK")]
        public string DeptPK { get; set; }
        [XmlElement("USER_PK")]
        public string UserPK { get; set; }
        [XmlElement("SESSION")]
        public string Session { get; set; }
        [XmlElement("PAGE_NO")]
        public string PageNo { get; set; }
        [XmlElement("IS_FROM")]
        public string IsFrom { get; set; }
        [XmlElement("PLAN_STAGE")]
        public string PlanStage { get; set; }
        [XmlElement("IS_SELECTED")]
        public string IsSelected { get; set; }
        public int? SOH_STATUS { get; set; }
        public int? SOH_COMPANY { get; set; }
        public int? SOH_TYPE { get; set; }
        [XmlElement("ORDERITEMS")]
        public List<OrderItemsBO> OrderItemsList { get; set; }

        [XmlElement("ORDERS")]
        public List<OrdersBO> Orders { get; set; }

        [XmlElement("PRODUCT_GROUPS")]
        public List<OroductGroupsBO> ProductGroups { get; set; }

        public bool HideConverted { get; set; }
    }


    [Serializable]
    [XmlRoot("Root")]
    public class SaleOrderBO
    {
        [XmlElement("SOH_PK")]
        public int SOH_PK { get; set; }
        [XmlElement("SOH_QUOTATION")]
        public int SOH_QUOTATION { get; set; }
        [XmlElement("SOH_NO")]
        public string SOH_NO { get; set; }
        [XmlElement("SOH_VERSION")]
        public int SOH_VERSION { get; set; }
        [XmlElement("SOH_DATE")]
        public string SOH_DATE { get; set; }
        [XmlElement("SOH_TYPE")]
        public int SOH_TYPE { get; set; }
        [XmlElement("SOH_STATUS")]
        public short SOH_STATUS { get; set; }
        [XmlElement("SOH_CUSTOMER")]
        public int SOH_CUSTOMER { get; set; }
        [XmlElement("SOH_CUSTOMER_TEXT")]
        public string SOH_CUSTOMER_TEXT { get; set; }
        [XmlElement("SOH_CUSTOMER_NAME")]
        public string SOH_CUSTOMER_NAME { get; set; }
        [XmlElement("SOH_CUSTOMER_ADDRESS")]
        public string SOH_CUSTOMER_ADDRESS { get; set; }
        [XmlElement("SOH_CUSTOMER_COUNTRY")]
        public string SOH_CUSTOMER_COUNTRY { get; set; }
        [XmlElement("SOH_CUSTOMER_COUNTRY_TEXT")]
        public string SOH_CUSTOMER_COUNTRY_TEXT { get; set; }
        [XmlElement("SOH_CUSTOMER_ZIP")]
        public string SOH_CUSTOMER_ZIP { get; set; }
        [XmlElement("SOH_CUSTOMER_PHONE")]
        public string SOH_CUSTOMER_PHONE { get; set; }
        [XmlElement("SOH_CUSTOMER_MOBILE")]
        public string SOH_CUSTOMER_MOBILE { get; set; }
        [XmlElement("SOH_CUSTOMER_FAX")]
        public string SOH_CUSTOMER_FAX { get; set; }
        [XmlElement("SOH_CUSTOMER_EMAIL")]
        public string SOH_CUSTOMER_EMAIL { get; set; }

        [XmlElement("SOH_REF_NO")]
        public string SOH_REF_NO { get; set; }
        [XmlElement("SOH_REF_DATE")]
        public string SOH_REF_DATE { get; set; }
        [XmlElement("SOH_PRIORITY")]
        public string SOH_PRIORITY { get; set; }
        [XmlElement("SOH_SHIPPING_TO")]
        public string SOH_SHIPPING_TO { get; set; }
        [XmlElement("SOH_SHIPPING_NAME")]
        public string SOH_SHIPPING_NAME { get; set; }
        [XmlElement("SOH_SHIPPING_ADDRESS")]
        public string SOH_SHIPPING_ADDRESS { get; set; }
        [XmlElement("SOH_SHIPPING_COUNTRY")]
        public string SOH_SHIPPING_COUNTRY { get; set; }
        [XmlElement("SOH_SHIPPING_COUNTRY_TEXT")]
        public string SOH_SHIPPING_COUNTRY_TEXT { get; set; }
        [XmlElement("SOH_SHIPPING_ZIP")]
        public string SOH_SHIPPING_ZIP { get; set; }
        [XmlElement("SOH_SHIPPING_PHONE")]
        public string SOH_SHIPPING_PHONE { get; set; }
        [XmlElement("SOH_SHIPPING_MOBILE")]
        public string SOH_SHIPPING_MOBILE { get; set; }
        [XmlElement("SOH_SHIPPING_FAX")]
        public string SOH_SHIPPING_FAX { get; set; }
        [XmlElement("SOH_SHIPPING_EMAIL")]
        public string SOH_SHIPPING_EMAIL { get; set; }

        [XmlElement("SOH_CURRENCY")]
        public int SOH_CURRENCY { get; set; }
        [XmlElement("SOH_CURRENCY_TEXT")]
        public string SOH_CURRENCY_TEXT { get; set; }
        [XmlElement("SOH_CURRENCY_RATE")]
        public double SOH_CURRENCY_RATE { get; set; }
        [XmlElement("SOH_CURRENCY_BC")]
        public int SOH_CURRENCY_BC { get; set; }

        [XmlElement("SOH_TOTAL_QTY")]
        public double SOH_TOTAL_QTY { get; set; }
        [XmlElement("SOH_TOTAL_AMT")]
        public double SOH_TOTAL_AMT { get; set; }
        [XmlElement("SOH_TOTAL_CONV_AMT")]
        public double SOH_TOTAL_CONV_AMT { get; set; }
        [XmlElement("SOH_TOTAL_DISCOUNT")]
        public double SOH_TOTAL_DISCOUNT { get; set; }
        [XmlElement("SOH_TOTAL_TAX")]
        public double SOH_TOTAL_TAX { get; set; }
        [XmlElement("SOH_TOTAL_SHIP_CHARGE")]
        public double SOH_TOTAL_SHIP_CHARGE { get; set; }
        [XmlElement("SOH_TOTAL_ADJUST")]
        public double SOH_TOTAL_ADJUST { get; set; }
        [XmlElement("SOH_NET_AMOUNT")]
        public double SOH_NET_AMOUNT { get; set; }
        [XmlElement("SOH_NET_AMOUNT_BC")]
        public double SOH_NET_AMOUNT_BC { get; set; }
        [XmlElement("SOH_REMARKS")]
        public string SOH_REMARKS { get; set; }
        [XmlElement("SOH_FROM_PORT")]
        public string SOH_FROM_PORT { get; set; }
        [XmlElement("SOH_FROM_PORT_TEXT")]
        public string SOH_FROM_PORT_TEXT { get; set; }
        [XmlElement("SOH_TO_PORT")]
        public string SOH_TO_PORT { get; set; }

        [XmlElement("SOH_SHIP_BY")]
        public string SOH_SHIP_BY { get; set; }
        [XmlElement("SOH_SHIP_BY_TEXT")]
        public string SOH_SHIP_BY_TEXT { get; set; }

        [XmlElement("SOH_TRANSHIPMENT")]
        public string SOH_TRANSHIPMENT { get; set; }
        [XmlElement("SOH_TRANSHIPMENT_TEXT")]
        public string SOH_TRANSHIPMENT_TEXT { get; set; }

        [XmlElement("SOH_BANK")]
        public string SOH_BANK { get; set; }
        [XmlElement("SOH_BANK_TEXT")]
        public string SOH_BANK_TEXT { get; set; }

        [XmlElement("SOH_PAYMENT_TERM")]
        public string SOH_PAYMENT_TERM { get; set; }
        [XmlElement("SOH_PAYMENT_TERM_NAME")]
        public string SOH_PAYMENT_TERM_NAME { get; set; }
        [XmlElement("SOH_PAYMENT_TERM_TEXT")]
        public string SOH_PAYMENT_TERM_TEXT { get; set; }

        [XmlElement("SOH_DEL_TERM")]
        public string SOH_DEL_TERM { get; set; }
        [XmlElement("SOH_DEL_TERM_NAME")]
        public string SOH_DEL_TERM_NAME { get; set; }
        [XmlElement("SOH_DEL_TERM_TEXT")]
        public string SOH_DEL_TERM_TEXT { get; set; }

        [XmlElement("SOH_SPECIAL_TERM")]
        public string SOH_SPECIAL_TERM { get; set; }
        [XmlElement("SOH_SPECIAL_TERM_NAME")]
        public string SOH_SPECIAL_TERM_NAME { get; set; }
        [XmlElement("SOH_SPECIAL_TERM_TEXT")]
        public string SOH_SPECIAL_TERM_TEXT { get; set; }

        [XmlElement("SOH_REFERENCE")]
        public string SOH_REFERENCE { get; set; }

        [XmlElement("SOH_ORG_GOODS")]
        public string SOH_ORG_GOODS { get; set; }
        [XmlElement("SOH_ORG_GOODS_TEXT")]
        public string SOH_ORG_GOODS_TEXT { get; set; }
        [XmlElement("SOH_NEED_ADV_PYMT")]
        public bool SOH_NEED_ADV_PYMT { get; set; }
        [XmlElement("SOH_FEEDER_VESSEL")]
        public string SOH_FEEDER_VESSEL { get; set; }
        [XmlElement("SOH_MOTHER_VESSEL")]
        public string SOH_MOTHER_VESSEL { get; set; }
        [XmlElement("SOH_ETD")]
        public string SOH_ETD { get; set; }
        [XmlElement("SOH_ETA")]
        public string SOH_ETA { get; set; }

        [XmlElement("SOH_SHIPPING_MARK")]
        public string SOH_SHIPPING_MARK { get; set; }
        [XmlElement("SOH_CONTAINER_NO")]
        public string SOH_CONTAINER_NO { get; set; }
        [XmlElement("SOH_FINAL_DESTINATION")]
        public string SOH_FINAL_DESTINATION { get; set; }
        [XmlElement("SOH_FAX")]
        public string SOH_FAX { get; set; }
        [XmlElement("SOH_SHIP_INT_TO")]
        public string SOH_SHIP_INT_TO { get; set; }

        [XmlElement("SOH_INSP_TYPE")]
        public int SOH_INSP_TYPE { get; set; }
        [XmlElement("SOH_EXP_DOC")]
        public int SOH_EXP_DOC { get; set; }

        [XmlElement("SOH_NOTIFY_PARTY")]
        public string SOH_NOTIFY_PARTY { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_NAME")]
        public string SOH_NOTIFY_PARTY_NAME { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_ADDRESS")]
        public string SOH_NOTIFY_PARTY_ADDRESS { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_COUNTRY")]
        public string SOH_NOTIFY_PARTY_COUNTRY { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_COUNTRY_TEXT")]
        public string SOH_NOTIFY_PARTY_COUNTRY_TEXT { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_ZIP")]
        public string SOH_NOTIFY_PARTY_ZIP { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_PHONE")]
        public string SOH_NOTIFY_PARTY_PHONE { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_MOBILE")]
        public string SOH_NOTIFY_PARTY_MOBILE { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_FAX")]
        public string SOH_NOTIFY_PARTY_FAX { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_EMAIL")]
        public string SOH_NOTIFY_PARTY_EMAIL { get; set; }

        [XmlElement("SOH_CONSIGNEE")]
        public string SOH_CONSIGNEE { get; set; }
        [XmlElement("SOH_CONSIGNEE_NAME")]
        public string SOH_CONSIGNEE_NAME { get; set; }
        [XmlElement("SOH_CONSIGNEE_ADDRESS")]
        public string SOH_CONSIGNEE_ADDRESS { get; set; }
        [XmlElement("SOH_CONSIGNEE_COUNTRY")]
        public string SOH_CONSIGNEE_COUNTRY { get; set; }
        [XmlElement("SOH_CONSIGNEE_COUNTRY_TEXT")]
        public string SOH_CONSIGNEE_COUNTRY_TEXT { get; set; }
        [XmlElement("SOH_CONSIGNEE_ZIP")]
        public string SOH_CONSIGNEE_ZIP { get; set; }
        [XmlElement("SOH_CONSIGNEE_PHONE")]
        public string SOH_CONSIGNEE_PHONE { get; set; }
        [XmlElement("SOH_CONSIGNEE_MOBILE")]
        public string SOH_CONSIGNEE_MOBILE { get; set; }
        [XmlElement("SOH_CONSIGNEE_FAX")]
        public string SOH_CONSIGNEE_FAX { get; set; }
        [XmlElement("SOH_CONSIGNEE_EMAIL")]
        public string SOH_CONSIGNEE_EMAIL { get; set; }
        [XmlElement("SOH_SUPP_DTL")]
        public string SOH_SUPP_DTL { get; set; }
        [XmlElement("SOH_PACKING_INSTRN")]
        public string SOH_PACKING_INSTRN { get; set; }

        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }

        [XmlElement("SOH_ACTIVE")]
        public byte SOH_ACTIVE { get; set; }
        [XmlElement("SOH_DEPT")]
        public short SOH_DEPT { get; set; }
        [XmlElement("SOH_DEPT_TEXT")]
        public string SOH_DEPT_TEXT { get; set; }
        [XmlElement("SOH_BIZUNIT")]
        public short SOH_BIZUNIT { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }

        //[XmlElement("SOH_BASE_CURR")]
        //public string SOH_BASE_CURR { get; set; }
        //[XmlElement("SOH_TRX_STATUS")]
        //public string SOH_TRX_STATUS { get; set; }
        //[XmlElement("SOH_DELETED")]
        //public string SOH_DELETED { get; set; }
        //[XmlElement("SOH_SUBMITTED_BY")]
        //public string SOH_SUBMITTED_BY { get; set; }
        //[XmlElement("SOH_SUBMITTED_BY_TEXT")]
        //public string SOH_SUBMITTED_BY_TEXT { get; set; }
        //[XmlElement("SOH_SUBMITTED_DATE")]
        //public string SOH_SUBMITTED_DATE { get; set; }
        //[XmlElement("SOH_APPROVED_BY")]
        //public string SOH_APPROVED_BY { get; set; }
        //[XmlElement("SOH_APPROVED_BY_TEXT")]
        //public string SOH_APPROVED_BY_TEXT { get; set; }
        //[XmlElement("SOH_APPROVED_DATE")]
        //public string SOH_APPROVED_DATE { get; set; }

        //[XmlElement("SOH_BIZUNIT_TEXT")]
        //public string SOH_BIZUNIT_TEXT { get; set; }
        //[XmlElement("SOH_CRTD_BY")]
        //public string SOH_CRTD_BY { get; set; }
        //[XmlElement("SOH_CRTD_DT")]
        //public string SOH_CRTD_DT { get; set; }
        //[XmlElement("SOH_MOD_BY")]
        //public string SOH_MOD_BY { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("TaxHdr")]
        public List<SaleOrderTaxHdr> TaxHdr { get; set; }
        [XmlElement("OrderDetail")]
        public List<SaleOrderDetailsBO> SaleOrderDetails { get; set; }
    }

    [Serializable]
    public class SaleOrderDetailsBO
    {
        [XmlElement("SOD_PK")]
        public int SOD_PK { get; set; }
        [XmlElement("SOD_SO")]
        public string SOD_SO { get; set; }
        [XmlElement("SOD_VERSION")]
        public int SOD_VERSION { get; set; }
        [XmlElement("SOD_SL_NO")]
        public int SOD_SL_NO { get; set; }
        [XmlElement("SOD_CUST_ITEM")]
        public int SOD_CUST_ITEM { get; set; }
        [XmlElement("SOD_CUST_ITEM_TEXT")]
        public string SOD_CUST_ITEM_TEXT { get; set; }
        [XmlElement("SOD_PACKING_SPEC")]
        public int SOD_PACKING_SPEC { get; set; }
        [XmlElement("SOD_PACKING_SPEC_TEXT")]
        public string SOD_PACKING_SPEC_TEXT { get; set; }
        [XmlElement("SOD_ITEM")]
        public int SOD_ITEM { get; set; }
        [XmlElement("SOD_ITEM_TEXT")]
        public string SOD_ITEM_TEXT { get; set; }
        [XmlElement("SOD_QTY")]
        public double SOD_QTY { get; set; }
        [XmlElement("SOD_UOM")]
        public int SOD_UOM { get; set; }
        [XmlElement("SOD_UOM_TEXT")]
        public string SOD_UOM_TEXT { get; set; }
        [XmlElement("SOD_CIM_PCS_PER_IP")]
        public double SOD_CIM_PCS_PER_IP { get; set; }
        [XmlElement("SOD_CIM_PCS_PER_OP")]
        public double SOD_CIM_PCS_PER_OP { get; set; }
        [XmlElement("SOD_QTY_CARTONS")]
        public double SOD_QTY_CARTONS { get; set; }
        [XmlElement("SOD_RATE")]
        public double SOD_RATE { get; set; }
        [XmlElement("SOD_AMOUNT")]
        public double SOD_AMOUNT { get; set; }
        [XmlElement("SOD_DISCOUNT")]
        public double SOD_DISCOUNT { get; set; }
        [XmlElement("SOD_TAX")]
        public double SOD_TAX { get; set; }
        [XmlElement("SOD_NET_AMOUNT")]
        public double SOD_NET_AMOUNT { get; set; }
        [XmlElement("SOD_REQUIRED_BY")]
        public string SOD_REQUIRED_BY { get; set; }
        [XmlElement("SOD_REMARKS")]
        public string SOD_REMARKS { get; set; }
        [XmlElement("SOD_LOT_NO")]
        public string SOD_LOT_NO { get; set; }
        [XmlElement("SOD_LOT_SIZE")]
        public string SOD_LOT_SIZE { get; set; }
        [XmlElement("SOD_ART_WORK")]
        public string SOD_ART_WORK { get; set; }
        [XmlElement("SOD_ART_WORK_TEXT")]
        public string SOD_ART_WORK_TEXT { get; set; }

        //[XmlElement("CED_ENQUIRY_HDR")]
        //public int CED_ENQUIRY_HDR { get; set; }
        //[XmlElement("CED_ENQUIRY_HDR_TEXT")]
        //public string CED_ENQUIRY_HDR_TEXT { get; set; }


        //[XmlElement("CED_VALID_FROM")]
        //public string CED_VALID_FROM { get; set; }
        //[XmlElement("CED_VALID_TO")]
        //public string CED_VALID_TO { get; set; }
        //[XmlElement("CED_CUST_QTY")]
        //public double CED_CUST_QTY { get; set; }
        //[XmlElement("CED_COMMENTS")]
        //public string CED_COMMENTS { get; set; }

        //[XmlElement("CED_ACTION")]
        //public short CED_ACTION { get; set; }
        [XmlElement("TaxDtl")]
        public List<SaleOrderTaxHdr> TaxDtl { get; set; }
    }

    [Serializable]
    public class SaleOrderTaxHdr
    {
        [XmlElement("SLT_PK")]
        public int SLT_PK { get; set; }
        [XmlElement("SLT_SO_DTL")]
        public int SLT_SO_DTL { get; set; }
        [XmlElement("SLT_TYPE")]
        public int SLT_TYPE { get; set; }
        [XmlElement("SLT_TAX")]
        public int SLT_TAX { get; set; }
        [XmlElement("SLT_SL_NO")]
        public int SLT_SL_NO { get; set; }
        [XmlElement("SLT_TAX_TEXT")]
        public string SLT_TAX_TEXT { get; set; }
        [XmlElement("SLT_TAX_FORMULA")]
        public string SLT_TAX_FORMULA { get; set; }
        [XmlElement("SLT_NAME")]
        public string SLT_NAME { get; set; }
        [XmlElement("SLT_TAX_AMT")]
        public double SLT_TAX_AMT { get; set; }
        [XmlElement("SLT_TAX_CATEGORY")]
        public int SLT_TAX_CATEGORY { get; set; }
        [XmlElement("SLT_TAX_CATEGORY_TEXT")]
        public string SLT_TAX_CATEGORY_TEXT { get; set; }
        [XmlElement("SLT_HAS_SUB_TOTAL")]
        public int SLT_HAS_SUB_TOTAL { get; set; }
        [XmlElement("SLT_HAS_DISCOUNT")]
        public int SLT_HAS_DISCOUNT { get; set; }
        [XmlElement("SLT_HAS_OTHER_CHARGE")]
        public int SLT_HAS_OTHER_CHARGE { get; set; }
        [XmlElement("SLT_DISC_PERC")]
        public double SLT_DISC_PERC { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class SaleContractBO : WorkflowBO
    {
        [XmlElement("SOH_PK")]
        public int SOH_PK { get; set; }
        [XmlElement("SOH_QUOTATION")]
        public int SOH_QUOTATION { get; set; }
        [XmlElement("SOH_NO")]
        public string SOH_NO { get; set; }
        [XmlElement("SOH_DATE")]
        public string SOH_DATE { get; set; }

        [XmlElement("SOH_CUSTOMER")]
        public int SOH_CUSTOMER { get; set; }
        [XmlElement("SOH_CUSTOMER_TEXT")]
        public string SOH_CUSTOMER_TEXT { get; set; }
        [XmlElement("SOH_CUSTOMER_NAME")]
        public string SOH_CUSTOMER_NAME { get; set; }
        [XmlElement("SOH_CUSTOMER_ADDRESS")]
        public string SOH_CUSTOMER_ADDRESS { get; set; }
        [XmlElement("SOH_CUSTOMER_COUNTRY")]
        public string SOH_CUSTOMER_COUNTRY { get; set; }
        [XmlElement("SOH_CUSTOMER_COUNTRY_TEXT")]
        public string SOH_CUSTOMER_COUNTRY_TEXT { get; set; }
        [XmlElement("SOH_CUSTOMER_ZIP")]
        public string SOH_CUSTOMER_ZIP { get; set; }
        [XmlElement("SOH_CUSTOMER_PHONE")]
        public string SOH_CUSTOMER_PHONE { get; set; }
        [XmlElement("SOH_CUSTOMER_MOBILE")]
        public string SOH_CUSTOMER_MOBILE { get; set; }
        [XmlElement("SOH_CUSTOMER_FAX")]
        public string SOH_CUSTOMER_FAX { get; set; }
        [XmlElement("SOH_CUSTOMER_EMAIL")]
        public string SOH_CUSTOMER_EMAIL { get; set; }

        [XmlElement("SOH_REFERENCE")]
        public string SOH_REFERENCE { get; set; }
        [XmlElement("SOH_REFERENCE_DATE")]
        public string SOH_REFERENCE_DATE { get; set; }
        [XmlElement("SOH_REF_NO")]
        public string SOH_REF_NO { get; set; }
        [XmlElement("SOH_REF_DATE")]
        public string SOH_REF_DATE { get; set; }
        [XmlElement("SOH_BOOKING_DATE")]
        public string SOH_BOOKING_DATE { get; set; }
        [XmlElement("SOH_TYPE")]
        public int SOH_TYPE { get; set; }
        [XmlElement("SOH_SUB_TYPE")]
        public int SOH_SUB_TYPE { get; set; }

        [XmlElement("SOH_CURRENCY")]
        public int SOH_CURRENCY { get; set; }
        [XmlElement("SOH_CURRENCY_TEXT")]
        public string SOH_CURRENCY_TEXT { get; set; }
        [XmlElement("SOH_CURRENCY_RATE")]
        public double SOH_CURRENCY_RATE { get; set; }
        [XmlElement("SOH_CURRENCY_BC")]
        public int SOH_CURRENCY_BC { get; set; }

        [XmlElement("SOH_SHIP_BY")]
        public string SOH_SHIP_BY { get; set; }
        [XmlElement("SOH_SHIP_BY_TEXT")]
        public string SOH_SHIP_BY_TEXT { get; set; }
        [XmlElement("SOH_DELIVERY_DATE")]
        public string SOH_DELIVERY_DATE { get; set; }
        [XmlElement("SOH_SHIPMENT_DESC")]
        public string SOH_SHIPMENT_DESC { get; set; }
        [XmlElement("SOH_FROM_PORT")]
        public string SOH_FROM_PORT { get; set; }
        [XmlElement("SOH_FROM_PORT_TEXT")]
        public string SOH_FROM_PORT_TEXT { get; set; }
        [XmlElement("SOH_TO_PORT")]
        public string SOH_TO_PORT { get; set; }
        [XmlElement("SOH_TO_PORT_PK")]
        public string SOH_TO_PORT_PK { get; set; }
        [XmlElement("SOH_TRANSHIPMENT")]
        public string SOH_TRANSHIPMENT { get; set; }
        [XmlElement("SOH_TRANSHIPMENT_TEXT")]
        public string SOH_TRANSHIPMENT_TEXT { get; set; }
        [XmlElement("SOH_FINAL_DESTINATION")]
        public string SOH_FINAL_DESTINATION { get; set; }

        [XmlElement("SOH_CONSIGNEE")]
        public string SOH_CONSIGNEE { get; set; }
        [XmlElement("SOH_CONSIGNEE_NAME")]
        public string SOH_CONSIGNEE_NAME { get; set; }
        [XmlElement("SOH_CONSIGNEE_ADDRESS")]
        public string SOH_CONSIGNEE_ADDRESS { get; set; }
        [XmlElement("SOH_CONSIGNEE_COUNTRY")]
        public string SOH_CONSIGNEE_COUNTRY { get; set; }
        [XmlElement("SOH_CONSIGNEE_COUNTRY_TEXT")]
        public string SOH_CONSIGNEE_COUNTRY_TEXT { get; set; }
        [XmlElement("SOH_CONSIGNEE_ZIP")]
        public string SOH_CONSIGNEE_ZIP { get; set; }
        [XmlElement("SOH_CONSIGNEE_PHONE")]
        public string SOH_CONSIGNEE_PHONE { get; set; }
        [XmlElement("SOH_CONSIGNEE_MOBILE")]
        public string SOH_CONSIGNEE_MOBILE { get; set; }
        [XmlElement("SOH_CONSIGNEE_FAX")]
        public string SOH_CONSIGNEE_FAX { get; set; }
        [XmlElement("SOH_CONSIGNEE_EMAIL")]
        public string SOH_CONSIGNEE_EMAIL { get; set; }

        [XmlElement("SOH_SHIPPING_TO")]
        public string SOH_SHIPPING_TO { get; set; }
        [XmlElement("SOH_SHIPPING_NAME")]
        public string SOH_SHIPPING_NAME { get; set; }
        [XmlElement("SOH_SHIPPING_ADDRESS")]
        public string SOH_SHIPPING_ADDRESS { get; set; }
        [XmlElement("SOH_SHIPPING_COUNTRY")]
        public string SOH_SHIPPING_COUNTRY { get; set; }
        [XmlElement("SOH_SHIPPING_COUNTRY_TEXT")]
        public string SOH_SHIPPING_COUNTRY_TEXT { get; set; }
        [XmlElement("SOH_SHIPPING_ZIP")]
        public string SOH_SHIPPING_ZIP { get; set; }
        [XmlElement("SOH_SHIPPING_PHONE")]
        public string SOH_SHIPPING_PHONE { get; set; }
        [XmlElement("SOH_SHIPPING_MOBILE")]
        public string SOH_SHIPPING_MOBILE { get; set; }
        [XmlElement("SOH_SHIPPING_FAX")]
        public string SOH_SHIPPING_FAX { get; set; }
        [XmlElement("SOH_SHIPPING_EMAIL")]
        public string SOH_SHIPPING_EMAIL { get; set; }

        [XmlElement("SOH_NOTIFY_PARTY")]
        public string SOH_NOTIFY_PARTY { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_NAME")]
        public string SOH_NOTIFY_PARTY_NAME { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_ADDRESS")]
        public string SOH_NOTIFY_PARTY_ADDRESS { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_COUNTRY")]
        public string SOH_NOTIFY_PARTY_COUNTRY { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_COUNTRY_TEXT")]
        public string SOH_NOTIFY_PARTY_COUNTRY_TEXT { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_ZIP")]
        public string SOH_NOTIFY_PARTY_ZIP { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_PHONE")]
        public string SOH_NOTIFY_PARTY_PHONE { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_MOBILE")]
        public string SOH_NOTIFY_PARTY_MOBILE { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_FAX")]
        public string SOH_NOTIFY_PARTY_FAX { get; set; }
        [XmlElement("SOH_NOTIFY_PARTY_EMAIL")]
        public string SOH_NOTIFY_PARTY_EMAIL { get; set; }

        [XmlElement("SOH_SHIP_AGENT")]
        public string SOH_SHIP_AGENT { get; set; }
        [XmlElement("SOH_SHIP_AGENT_NAME")]
        public string SOH_SHIP_AGENT_NAME { get; set; }
        [XmlElement("SOH_SHIP_AGENT_ADDRESS")]
        public string SOH_SHIP_AGENT_ADDRESS { get; set; }
        [XmlElement("SOH_SHIP_AGENT_COUNTRY")]
        public string SOH_SHIP_AGENT_COUNTRY { get; set; }
        [XmlElement("SOH_SHIP_AGENT_COUNTRY_TEXT")]
        public string SOH_SHIP_AGENT_COUNTRY_TEXT { get; set; }
        [XmlElement("SOH_SHIP_AGENT_ZIP")]
        public string SOH_SHIP_AGENT_ZIP { get; set; }
        [XmlElement("SOH_SHIP_AGENT_PHONE")]
        public string SOH_SHIP_AGENT_PHONE { get; set; }
        [XmlElement("SOH_SHIP_AGENT_MOBILE")]
        public string SOH_SHIP_AGENT_MOBILE { get; set; }
        [XmlElement("SOH_SHIP_AGENT_FAX")]
        public string SOH_SHIP_AGENT_FAX { get; set; }
        [XmlElement("SOH_SHIP_AGENT_EMAIL")]
        public string SOH_SHIP_AGENT_EMAIL { get; set; }

        [XmlElement("SOH_SHIP_INT_TO")]
        public string SOH_SHIP_INT_TO { get; set; }
        [XmlElement("SOH_FAX")]
        public string SOH_FAX { get; set; }
        [XmlElement("SOH_CONTAINER_SIZE")]
        public string SOH_CONTAINER_SIZE { get; set; }

        [XmlElement("SOH_DEL_TERM")]
        public string SOH_DEL_TERM { get; set; }
        [XmlElement("SOH_DEL_TERM_NAME")]
        public string SOH_DEL_TERM_NAME { get; set; }
        [XmlElement("SOH_DEL_TERM_TEXT")]
        public string SOH_DEL_TERM_TEXT { get; set; }

        [XmlElement("SOH_PAYMENT_TERM")]
        public string SOH_PAYMENT_TERM { get; set; }
        [XmlElement("SOH_PAYMENT_TERM_NAME")]
        public string SOH_PAYMENT_TERM_NAME { get; set; }
        [XmlElement("SOH_PAYMENT_TERM_TEXT")]
        public string SOH_PAYMENT_TERM_TEXT { get; set; }

        [XmlElement("SOH_SPECIAL_TERM")]
        public string SOH_SPECIAL_TERM { get; set; }
        [XmlElement("SOH_SPECIAL_TERM_NAME")]
        public string SOH_SPECIAL_TERM_NAME { get; set; }
        [XmlElement("SOH_SPECIAL_TERM_TEXT")]
        public string SOH_SPECIAL_TERM_TEXT { get; set; }

        [XmlElement("SOH_NEED_ADV_PYMT")]
        public bool SOH_NEED_ADV_PYMT { get; set; }
        [XmlElement("SOH_BANK")]
        public string SOH_BANK { get; set; }
        [XmlElement("SOH_BANK_TEXT")]
        public string SOH_BANK_TEXT { get; set; }
        [XmlElement("SOH_AGENT")]
        public string SOH_AGENT { get; set; }

        [XmlElement("SOH_INSP_TYPE")]
        public string SOH_INSP_TYPE { get; set; }
        [XmlElement("SOH_EXP_DOC")]
        public string SOH_EXP_DOC { get; set; }
        [XmlElement("SOH_PACKING_INSTRN")]
        public string SOH_PACKING_INSTRN { get; set; }
        [XmlElement("SOH_ORG_GOODS")]
        public string SOH_ORG_GOODS { get; set; }
        [XmlElement("SOH_ORG_GOODS_TEXT")]
        public string SOH_ORG_GOODS_TEXT { get; set; }

        [XmlElement("SOH_REMARKS")]
        public string SOH_REMARKS { get; set; }

        [XmlElement("SOH_TOTAL_AMT")]
        public double SOH_TOTAL_AMT { get; set; }
        [XmlElement("SOH_TOTAL_DISCOUNT")]
        public double SOH_TOTAL_DISCOUNT { get; set; }
        [XmlElement("SOH_TOTAL_TAX")]
        public double SOH_TOTAL_TAX { get; set; }
        [XmlElement("SOH_TOTAL_SHIP_CHARGE")]
        public double SOH_TOTAL_SHIP_CHARGE { get; set; }
        [XmlElement("SOH_TOTAL_ADJUST")]
        public double SOH_TOTAL_ADJUST { get; set; }
        [XmlElement("SOH_NET_AMOUNT")]
        public double SOH_NET_AMOUNT { get; set; }
        [XmlElement("SOH_AMT_INVOICED")]
        public double SOH_AMT_INVOICED { get; set; }
        [XmlElement("SOH_NET_AMOUNT_BC")]
        public double SOH_NET_AMOUNT_BC { get; set; }

        [XmlElement("SOH_DEPT")]
        public short SOH_DEPT { get; set; }
        [XmlElement("SOH_DEPT_TEXT")]
        public string SOH_DEPT_TEXT { get; set; }
        [XmlElement("SOH_BIZUNIT")]
        public short SOH_BIZUNIT { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("SOH_CREDIT_CHECK")]
        public int SOH_CREDIT_CHECK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("WKF_PROCESS")]
        public int WKF_PROCESS { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }

        [XmlElement("SOH_VERSION")]
        public int SOH_VERSION { get; set; }
        [XmlElement("SOH_STATUS")]
        public short SOH_STATUS { get; set; }

        [XmlElement("SOH_SHIPMENT_TERM")]
        public int SOH_SHIPMENT_TERM { get; set; }

        [XmlElement("SOH_SHIPMENT_TERM_TEXT")]
        public string SOH_SHIPMENT_TERM_TEXT { get; set; }

        //Not Used
        [XmlElement("SOH_ACTIVE")]
        public byte SOH_ACTIVE { get; set; }
        [XmlElement("SOH_CRTD_BY")]
        public string SOH_CRTD_BY { get; set; }
        [XmlElement("SOH_CRTD_DT")]
        public string SOH_CRTD_DT { get; set; }
        [XmlElement("SOH_MOD_BY")]
        public string SOH_MOD_BY { get; set; }
        [XmlElement("SOH_MOD_DT")]

        public string SOH_MOD_DT { get; set; }

        [XmlElement("SOH_PRIORITY")]
        public string SOH_PRIORITY { get; set; }

        [XmlElement("SOH_TOTAL_QTY")]
        public double SOH_TOTAL_QTY { get; set; }
        [XmlElement("SOH_TOTAL_CONV_AMT")]
        public double SOH_TOTAL_CONV_AMT { get; set; }

        [XmlElement("SOH_IS_AMEND")]
        public byte SOH_IS_AMEND { get; set; }
        [XmlElement("SOH_COMPANY")]
        public int SOH_COMPANY { get; set; }
        [XmlElement("SOH_AMEND_DATE")]
        public string SOH_AMEND_DATE { get; set; }
        [XmlElement("SOH_QTY_GROSS_WT")]
        public double SOH_QTY_GROSS_WT { get; set; }
        [XmlElement("SOH_ADV_INVOICED")]
        public double SOH_ADV_INVOICED { get; set; }
        [XmlElement("SOH_DEL_STATUS")]
        public byte SOH_DEL_STATUS { get; set; }
        [XmlElement("SOH_ADD_NAME")]
        public string SOH_ADD_NAME { get; set; }
        [XmlElement("P_TYPE")]
        public int P_TYPE { get; set; }
        [XmlElement("WKF_MAIL_ATTACH")]
        public int WKF_MAIL_ATTACH { get; set; }


        [XmlElement("SOH_NETTING")]
        public int SOH_NETTING { get; set; }
        [XmlElement("SOH_INSULATION")]
        public int SOH_INSULATION { get; set; }
        [XmlElement("SOH_ADDL_PACKAGE")]
        public int SOH_ADDL_PACKAGE { get; set; }
        [XmlElement("SOH_PRE_SHIPMENT")]
        public int SOH_PRE_SHIPMENT { get; set; }
        [XmlElement("SOH_PROTEIN_TEST")]
        public int SOH_PROTEIN_TEST { get; set; }
        [XmlElement("SOH_STANDARD")]
        public string SOH_STANDARD { get; set; }
        [XmlElement("SOH_INNER")]
        public string SOH_INNER { get; set; }
        [XmlElement("SOH_CARTON")]
        public string SOH_CARTON { get; set; }


        [XmlElement("SOH_CUS_PO_FLAG")]
        public int SOH_CUS_PO_FLAG { get; set; }

        [XmlElement("SOH_CRM_CQH_PK")]
        public int QuotationPK { get; set; }

        [XmlElement("SOH_CRM_CQH_TEXT")]
        public string QuotationNo { get; set; }

        [XmlElement("TaxHdr")]
        public List<SaleOrderTaxHdr> TaxHdr { get; set; }
        [XmlElement("OrderDetail")]
        public List<SaleContractDetailsBO> SaleContractDetails { get; set; }
        [XmlElement("FileList")]
        public List<SaleOrderUploads> FileList { get; set; }
    }

    [Serializable]
    public class SaleContractDetailsBO
    {
        [XmlElement("SOD_PK")]
        public int SOD_PK { get; set; }
        [XmlElement("SOD_SO")]
        public string SOD_SO { get; set; }
        [XmlElement("SOD_VERSION")]
        public int SOD_VERSION { get; set; }
        [XmlElement("SOD_SL_NO")]
        public int SOD_SL_NO { get; set; }
        [XmlElement("SOD_CUST_ITEM")]
        public int SOD_CUST_ITEM { get; set; }
        [XmlElement("SOD_CUST_ITEM_TEXT")]
        public string SOD_CUST_ITEM_TEXT { get; set; }
        [XmlElement("SOD_CUST_ITEM_CODE")]
        public string SOD_CUST_ITEM_CODE { get; set; }
        [XmlElement("SOD_ITEM")]
        public int SOD_ITEM { get; set; }
        [XmlElement("SOD_ITEM_TEXT")]
        public string SOD_ITEM_TEXT { get; set; }
        [XmlElement("SOD_ITEM_CODE")]
        public string SOD_ITEM_CODE { get; set; }

        //Brand related Info
        [XmlElement("APS_NAME")]
        public string APS_NAME { get; set; }

        [XmlElement("CIM_PACKING_SPEC_NAME")]
        public string CIM_PACKING_SPEC_NAME { get; set; }

        [XmlElement("CBM")]
        public double CBM { get; set; }
        [XmlElement("APS_TOTAL_PCS")]
        public double APS_TOTAL_PCS { get; set; }
        [XmlElement("APS_IB_PCS")]
        public double APS_IB_PCS { get; set; }
        [XmlElement("PC_ART_WORK")]
        public string PC_ART_WORK { get; set; }
        [XmlElement("PC_DOC_PATH")]
        public string PC_DOC_PATH { get; set; }
        [XmlElement("IB_ART_WORK")]
        public string IB_ART_WORK { get; set; }
        [XmlElement("IB_DOC_PATH")]
        public string IB_DOC_PATH { get; set; }
        [XmlElement("IC_ART_WORK")]
        public string IC_ART_WORK { get; set; }
        [XmlElement("IC_DOC_PATH")]
        public string IC_DOC_PATH { get; set; }
        [XmlElement("ZB_ART_WORK")]
        public string ZB_ART_WORK { get; set; }
        [XmlElement("ZB_DOC_PATH")]
        public string ZB_DOC_PATH { get; set; }
        [XmlElement("MC_ART_WORK")]
        public string MC_ART_WORK { get; set; }
        [XmlElement("MC_DOC_PATH")]
        public string MC_DOC_PATH { get; set; }
        [XmlElement("SC_ART_WORK")]
        public string SC_ART_WORK { get; set; }
        [XmlElement("SC_DOC_PATH")]
        public string SC_DOC_PATH { get; set; }

        [XmlElement("SOD_PACKING_SPEC")]
        public int SOD_PACKING_SPEC { get; set; }
        [XmlElement("SOD_PACKING_SPEC_TEXT")]
        public string SOD_PACKING_SPEC_TEXT { get; set; }
        [XmlElement("PACKING_TEXT")]
        public string PACKING_TEXT { get; set; }

        [XmlElement("SOD_QTY")]
        public double SOD_QTY { get; set; }
        [XmlElement("SOD_QTY_INVOICED")]
        public double SOD_QTY_INVOICED { get; set; }
        [XmlElement("SOD_QTY_DISPATCHED")]
        public double SOD_QTY_DISPATCHED { get; set; }
        [XmlElement("SOD_UOM")]
        public int SOD_UOM { get; set; }
        [XmlElement("SOD_UOM_TEXT")]
        public string SOD_UOM_TEXT { get; set; }
        [XmlElement("SOD_CIM_PCS_PER_IP")]
        public double SOD_CIM_PCS_PER_IP { get; set; }
        [XmlElement("SOD_CIM_PCS_PER_OP")]
        public double SOD_CIM_PCS_PER_OP { get; set; }
        [XmlElement("SOD_QTY_CARTONS")]
        public double SOD_QTY_CARTONS { get; set; }
        [XmlElement("SOD_RATE")]
        public double SOD_RATE { get; set; }
        [XmlElement("SOD_AMOUNT")]
        public double SOD_AMOUNT { get; set; }
        [XmlElement("SOD_DISCOUNT")]
        public double SOD_DISCOUNT { get; set; }
        [XmlElement("SOD_CARTON_RATE")]
        public string SOD_CARTON_RATE { get; set; }
        [XmlElement("SOD_TAX")]
        public double SOD_TAX { get; set; }
        [XmlElement("SOD_NET_AMOUNT")]
        public double SOD_NET_AMOUNT { get; set; }
        [XmlElement("SOD_REQUIRED_BY")]
        public string SOD_REQUIRED_BY { get; set; }
        [XmlElement("SOD_REMARKS")]
        public string SOD_REMARKS { get; set; }
        [XmlElement("SOD_REMARKS2")]
        public string SOD_REMARKS2 { get; set; }
        [XmlElement("SOD_LOT_NO")]
        public string SOD_LOT_NO { get; set; }
        [XmlElement("SOD_LOT_SIZE")]
        public string SOD_LOT_SIZE { get; set; }
        [XmlElement("SOD_ART_WORK")]
        public string SOD_ART_WORK { get; set; }
        [XmlElement("SOD_ART_WORK_TEXT")]
        public string SOD_ART_WORK_TEXT { get; set; }
        [XmlElement("SOD_ART_WORK_DESC")]
        public string SOD_ART_WORK_DESC { get; set; }
        [XmlElement("SOD_SALE_QTY")]
        public double SOD_SALE_QTY { get; set; }
        [XmlElement("SOD_SALE_UOM")]
        public int SOD_SALE_UOM { get; set; }
        [XmlElement("SOD_SALE_UOM_CONV")]
        public double SOD_SALE_UOM_CONV { get; set; }
        [XmlElement("SOD_SALE_UOM_TEXT")]
        public string SOD_SALE_UOM_TEXT { get; set; }
        [XmlElement("ISD_NAT_SUF")]
        public int ISD_NAT_SUF { get; set; }
        [XmlElement("SOD_CUST_ITEM_ACTIVE")]
        public int SOD_CUST_ITEM_ACTIVE { get; set; }
        [XmlElement("SOD_UOM_IS_PCS")]
        public int SOD_UOM_IS_PCS { get; set; }
        [XmlElement("SOD_CASE_MARK")]
        public string SOD_CASE_MARK { get; set; }

        [XmlElement("SOD_STRAPPING")]
        public string SOD_STRAPPING { get; set; }
        [XmlElement("SOD_STRAPPING_COLOUR")]
        public string SOD_STRAPPING_COLOUR { get; set; }
        [XmlElement("SOD_LAYERING")]
        public string SOD_LAYERING { get; set; }
        [XmlElement("SOD_NO_LAYERS")]
        public double SOD_NO_LAYERS { get; set; }
        [XmlElement("SOD_PIECES_LAYER")]
        public double SOD_PIECES_LAYER { get; set; }
        [XmlElement("SOD_MFG_DATE")]
        public string SOD_MFG_DATE { get; set; }
        [XmlElement("SOD_EXP_DATE")]
        public string SOD_EXP_DATE { get; set; }
        [XmlElement("CIM_SALE_UOM_CONV")]
        public string CIM_SALE_UOM_CONV { get; set; }
        [XmlElement("SOD_IS_PACK_MAT")]
        public int SOD_IS_PACK_MAT { get; set; }
       
        [XmlElement("SOD_ITEM_CATEGORY")]
        public string SOD_ITEM_CATEGORY { get; set; }
        [XmlElement("SOD_ITEM_CATEGORY_TEXT")]
        public string SOD_ITEM_CATEGORY_TEXT { get; set; }

        [XmlElement("SOD_PACK_TYPE_VALUE")]
        public int SOD_PACK_TYPE_VALUE { get; set; }
        [XmlElement("SOD_PACK_TYPE_TEXT")]
        public string SOD_PACK_TYPE_TEXT { get; set; }

        [XmlElement("SOD_HSN_CODE")]
        public int SOD_HSN_CODE { get; set; }

        [XmlElement("SOD_HSN_CODE_TEXT")]
        public string SOD_HSN_CODE_TEXT { get; set; }

        [XmlElement("TaxDtl")]
        public List<SaleOrderTaxHdr> TaxDtl { get; set; }
    }


    [Serializable]
    [XmlRoot("Root")]
    public class ProductDtlBO
    {
        [XmlElement("ProductDtl")]
        public List<ProductDtls> ItemsList { get; set; }
    }

    [Serializable]
    public class ProductDtls
    {
        [XmlElement("CIM_PK")]
        public int CIM_PK { get; set; }

    }

    [Serializable]
    public class OroductGroupsBO
    {
        [XmlElement("PRODUCT_GROUP")]
        public List<OroductGroupBO> ProductGroupList { get; set; }
    }
    [Serializable]
    public class OroductGroupBO
    {
        [XmlElement("PDG_PK")]
        public int ProductGroupPK { get; set; }
    }

    [Serializable]
    public class OrdersBO
    {
        [XmlElement("ORDER")]
        public List<OrderBO> OrderList { get; set; }
    }
    [Serializable]
    public class OrderBO
    {
        [XmlElement("SOH_PK")]
        public int sohPK { get; set; }

    }

    [Serializable]
    public class OrderItemsBO
    {
        [XmlElement("ITEM")]
        public List<ItemsBO> ItemsList { get; set; }
    }

    public class ItemsBO
    {
        [XmlElement("SOD_PK")]
        public int sodPK { get; set; }
        [XmlElement("QTY")]
        public string Qty { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class PlannedDetailsBO
    {
        [XmlElement("SOD_PK")]
        public string sodPK { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("SESSION")]
        public string Session { get; set; }

        [XmlElement("PAGE_NO")]
        public int PageNo { get; set; }

    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class ProductMasterBO
    {
        [XmlElement("PRO_PK")]
        public string ProductPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("PRO_CATEGORY")]
        public int category { get; set; }
        [XmlElement("PDT_PK")]
        public string productTypePK { get; set; }
        [XmlElement("SIZ_PK")]
        public string SizePK { get; set; }
        [XmlElement("CLR_PK")]
        public string ColorPK { get; set; }
        [XmlElement("LNE_PK")]
        public string linePK { get; set; }
        [XmlElement("PDG_PK")]
        public string ProductGroupPK { get; set; }

    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class ItemMasterBO
    {
        [XmlElement("ITM_PK")]
        public string ItemPK { get; set; }
        [XmlElement("ITM_ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class PlanStageBO
    {
        [XmlElement("CFG_PK")]
        public string cfgPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("CFG_TYPE")]
        public string cfgType { get; set; }



    }


    [Serializable]
    [XmlRoot("ROOT")]
    public class SizeMasterBO
    {
        [XmlElement("SIZ_PK")]
        public string SizePK { get; set; }
        [XmlElement("PDG_PK")]
        public string ProductGroupPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }

    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class SaleOrderHeaderBO
    {
        [XmlElement("SOH_PK")]
        public string SaleOrderHdPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }

    }

    [Serializable]
    public class SaleOrderInfo
    {
        public int sodPK { get; set; }
        public bool chkChecked { get; set; }

    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class SessionDtlBO
    {
        [XmlElement("PSN_PK")]
        public string PsnPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("PSN_STATUS")]
        public int Status { get; set; }
        [XmlElement("PSN_DEPT")]
        public int DeptPK { get; set; }


    }

    [Serializable]
    public class SaleOrderUploads
    {
        [XmlElement("DOC_PK")]
        public int DOC_PK { get; set; }
        [XmlElement("DOC_SEQ_NO")]
        public int DOC_SEQ_NO { get; set; }
        [XmlElement("DOC_TITLE")]
        public string DOC_TITLE { get; set; }
        [XmlElement("DOC_NAME")]
        public string DOC_NAME { get; set; }
        [XmlElement("DOC_PATH")]
        public string DOC_PATH { get; set; }
        [XmlElement("DOC_TYPE")]
        public string DOC_TYPE { get; set; }
        [XmlElement("DOC_ACTIVE")]
        public int DOC_ACTIVE { get; set; }
        public string FileExtension { get; set; }
        public string AttachmentFileName { get; set; }
    }

    public class FileDetails
    {
        public int SlNo
        {
            get;
            set;
        }
        public HttpPostedFile SoFile
        {
            get;
            set;
        }

    }

    [Serializable]
    [XmlRoot("Root")]
    public class ItemRefBO
    {       
        [XmlElement("Details")]
        public List<ItemRefDetails> RefDetails { get; set; }

    }
    [Serializable]
    public class ItemRefDetails
    {
        [XmlElement("SOD_PK")]
        public int SOD_PK { get; set; }
        [XmlElement("SOD_CUST_ITEM")]
        public int SOD_CUST_ITEM { get; set; }
        [XmlElement("CIM_BRAND_NAME")]
        public string CIM_BRAND_NAME { get; set; }
        [XmlElement("TRX_TYPE_TEXT")]
        public string TRX_TYPE_TEXT { get; set; }
        [XmlElement("TRX_NO")]
        public string TRX_NO { get; set; }
        [XmlElement("TRX_DATE")]
        public string TRX_DATE { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class SalesCostBO
    {
        [XmlElement("SOH_DATE")]
        public DateTime SOH_DATE { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }

        [XmlElement("Detail")]
        public List<SalesCostPatams> SalesCostPatams { get; set; }
    }
    [Serializable]
    public class SalesCostPatams
    {
        [XmlElement("SOD_CUST_ITEM")]
        public int SOD_CUST_ITEM { get; set; }
        [XmlElement("SOD_QTY")]
        public double SOD_QTY { get; set; }
        [XmlElement("SOD_BRAND_QTY")]
        public double SOD_BRAND_QTY { get; set; }
        [XmlElement("SOD_AMOUNT")]
        public double SOD_AMOUNT { get; set; }
        [XmlElement("SOD_CURRENCY")]
        public int SOD_CURRENCY { get; set; }
        [XmlElement("SOD_SALE_RATE")]
        public double SOD_SALE_RATE { get; set; }
        [XmlElement("SOD_BRAND_UOM")]
        public int SOD_BRAND_UOM { get; set; }
        [XmlElement("SOD_BRAND_UOM_TEXT")]
        public string SOD_BRAND_UOM_TEXT { get; set; }
    }
}