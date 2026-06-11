using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;

namespace BusinessObject.SaleOrder
{
    public class SalesInvoiceBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class SOInvoiceHeaderMul
    {
        [XmlElement("SO")]
        public List<SOInvoiceHeader> SOMainList { get; set; }
    }


    [Serializable]
    public class SOInvoiceHeader
    {
        [XmlElement("ICH_PK")]
        public int ICH_PK { get; set; }
        [XmlElement("ICH_NO")]
        public string ICH_NO { get; set; }
        [XmlElement("ICH_DATE")]
        public string ICH_DATE { get; set; }
        [XmlElement("ICH_TYPE")]
        public string ICH_TYPE { get; set; }
        [XmlElement("ICH_GST_TYPE")]
        public string ICH_GST_TYPE { get; set; }
        [XmlElement("ICH_VERSION")]
        public int ICH_VERSION { get; set; }
        [XmlElement("ICH_STATUS")]
        public short ICH_STATUS { get; set; }
        [XmlElement("ICH_DESPATCH_HDR")]
        public string ICH_DESPATCH_HDR { get; set; }

        [XmlElement("ICH_CUSTOMER")]
        public string ICH_CUSTOMER { get; set; }
        [XmlElement("ICH_CUSTOMER_NAME")]
        public string ICH_CUSTOMER_NAME { get; set; }
        [XmlElement("ICH_CUSTOMER_TEXT")]
        public string ICH_CUSTOMER_TEXT { get; set; }
        [XmlElement("ICH_CUSTOMER_ADDRESS")]
        public string ICH_CUSTOMER_ADDRESS { get; set; }
        [XmlElement("ICH_CUSTOMER_COUNTRY")]
        public string ICH_CUSTOMER_COUNTRY { get; set; }
        [XmlElement("ICH_CUSTOMER_COUNTRY_TEXT")]
        public string ICH_CUSTOMER_COUNTRY_TEXT { get; set; }
        [XmlElement("ICH_CUSTOMER_ZIP")]
        public string ICH_CUSTOMER_ZIP { get; set; }
        [XmlElement("ICH_CUSTOMER_PHONE")]
        public string ICH_CUSTOMER_PHONE { get; set; }
        [XmlElement("ICH_CUSTOMER_MOBILE")]
        public string ICH_CUSTOMER_MOBILE { get; set; }
        [XmlElement("ICH_CUSTOMER_FAX")]
        public string ICH_CUSTOMER_FAX { get; set; }
        [XmlElement("ICH_CUSTOMER_EMAIL")]
        public string ICH_CUSTOMER_EMAIL { get; set; }
        [XmlElement("ICH_CUSTOMER_CREDIT_DAYS")]
        public int ICH_CUSTOMER_CREDIT_DAYS { get; set; }

        //Adding New fields Type,Branch id and taxid

        [XmlElement("ICH_BRANCH")]
        public string ICH_BRANCH { get; set; }
        [XmlElement("ICH_BRANCH_TYPE")]
        public string ICH_BRANCH_TYPE { get; set; }
        [XmlElement("ICH_BRANCH_TEXT")]
        public string ICH_BRANCH_TEXT { get; set; }
        [XmlElement("ICH_TAX_ID")]
        public string ICH_TAX_ID { get; set; }




        [XmlElement("ICH_REF_NO")]
        public string ICH_REF_NO { get; set; }
        [XmlElement("ICH_REF_DATE")]
        public string ICH_REF_DATE { get; set; }

        [XmlElement("ICH_SHIPPING_TO")]
        public string ICH_SHIPPING_TO { get; set; }
        [XmlElement("ICH_SHIPPING_NAME")]
        public string ICH_SHIPPING_NAME { get; set; }
        [XmlElement("ICH_SHIPPING_ADDRESS")]
        public string ICH_SHIPPING_ADDRESS { get; set; }
        [XmlElement("ICH_SHIPPING_COUNTRY")]
        public string ICH_SHIPPING_COUNTRY { get; set; }
        [XmlElement("ICH_SHIPPING_COUNTRY_TEXT")]
        public string ICH_SHIPPING_COUNTRY_TEXT { get; set; }
        [XmlElement("ICH_SHIPPING_ZIP")]
        public string ICH_SHIPPING_ZIP { get; set; }
        [XmlElement("ICH_SHIPPING_PHONE")]
        public string ICH_SHIPPING_PHONE { get; set; }
        [XmlElement("ICH_SHIPPING_MOBILE")]
        public string ICH_SHIPPING_MOBILE { get; set; }
        [XmlElement("ICH_SHIPPING_FAX")]
        public string ICH_SHIPPING_FAX { get; set; }
        [XmlElement("ICH_SHIPPING_EMAIL")]
        public string ICH_SHIPPING_EMAIL { get; set; }


        [XmlElement("ICH_CURRENCY")]
        public int ICH_CURRENCY { get; set; }
        [XmlElement("ICH_CURRENCY_TEXT")]
        public string ICH_CURRENCY_TEXT { get; set; }
        [XmlElement("ICH_EXCHG_RATE")]
        public double ICH_EXCHG_RATE { get; set; }
        [XmlElement("ICH_BASE_CURR")]
        public int ICH_BASE_CURR { get; set; }

        [XmlElement("ICH_AMOUNT_TC")]
        public double ICH_AMOUNT_TC { get; set; }
        [XmlElement("ICH_DISCOUNT_TC")]
        public double ICH_DISCOUNT_TC { get; set; }
        [XmlElement("ICH_TAX_TC")]
        public double ICH_TAX_TC { get; set; }
        [XmlElement("ICH_AMOUNT_ADV_DED_TC")]
        public double ICH_AMOUNT_ADV_DED_TC { get; set; }
        [XmlElement("ICH_SHIP_CHARGE")]
        public double ICH_SHIP_CHARGE { get; set; }
        [XmlElement("ICH_SHIP_CHARGE_DED")]
        public string ICH_SHIP_CHARGE_DED { get; set; }
        [XmlElement("ICH_AMOUNT_ADJUST")]
        public double ICH_AMOUNT_ADJUST { get; set; }
        [XmlElement("ICH_AMOUNT_RCVD_TC")]
        public double ICH_AMOUNT_RCVD_TC { get; set; }
        [XmlElement("ICH_AMOUNT_NET_TC")]
        public double ICH_AMOUNT_NET_TC { get; set; }
        [XmlElement("ICH_NET_VALUE_TC")]
        public double ICH_NET_VALUE_TC { get; set; }
        [XmlElement("ICH_AMOUNT_NET_BC")]
        public double ICH_AMOUNT_NET_BC { get; set; }
        [XmlElement("ICH_TOTAL_QTY")]
        public double ICH_TOTAL_QTY { get; set; }
        [XmlElement("ICH_DATE_PAY_BY")]
        public string ICH_DATE_PAY_BY { get; set; }
        [XmlElement("ICH_COMPANY")]
        public int ICH_COMPANY { get; set; }

        [XmlElement("ICH_HAS_JRNL_ENTRY")]
        public bool ICH_HAS_JRNL_ENTRY { get; set; }
        [XmlElement("ICH_AMOUNT_DN_TC")]
        public double ICH_AMOUNT_DN_TC { get; set; }
        [XmlElement("ICH_AMOUNT_CN_TC")]
        public double ICH_AMOUNT_CN_TC { get; set; }



        [XmlElement("ICH_REMARKS")]
        public string ICH_REMARKS { get; set; }
        [XmlElement("ICH_TO_PORT")]
        public string ICH_TO_PORT { get; set; }
        [XmlElement("ICH_FROM_PORT")]
        public string ICH_FROM_PORT { get; set; }
        [XmlElement("ICH_FROM_PORT_TEXT")]
        public string ICH_FROM_PORT_TEXT { get; set; }

        [XmlElement("ICH_DEL_TERM_TEXT")]
        public string ICH_DEL_TERM_TEXT { get; set; }
        [XmlElement("ICH_REFERENCE")]
        public string ICH_REFERENCE { get; set; }
        [XmlElement("ICH_ORG_GOODS")]
        public string ICH_ORG_GOODS { get; set; }
        [XmlElement("ICH_ORG_GOODS_TEXT")]
        public string ICH_ORG_GOODS_TEXT { get; set; }

        [XmlElement("ICH_FEEDER_VESSEL")]
        public string ICH_FEEDER_VESSEL { get; set; }
        [XmlElement("ICH_MOTHER_VESSEL")]
        public string ICH_MOTHER_VESSEL { get; set; }
        [XmlElement("ICH_ETD")]
        public string ICH_ETD { get; set; }
        [XmlElement("ICH_ETA")]
        public string ICH_ETA { get; set; }
        [XmlElement("ICH_SHIPPING_MARK")]
        public string ICH_SHIPPING_MARK { get; set; }
        [XmlElement("ICH_SPECIAL_NOTES")]
        public string ICH_SPECIAL_NOTES { get; set; }
        [XmlElement("ICH_CONTAINER_NO")]
        public string ICH_CONTAINER_NO { get; set; }
        [XmlElement("ICH_FINAL_DESTINATION")]
        public string ICH_FINAL_DESTINATION { get; set; }
        [XmlElement("ICH_PAYMENT_TERM")]
        public string ICH_PAYMENT_TERM { get; set; }
        [XmlElement("ICH_PAYMENT_TERM_TEXT")]
        public string ICH_PAYMENT_TERM_TEXT { get; set; }
        [XmlElement("ICH_HAS_DUE_DTL")]
        public string ICH_HAS_DUE_DTL { get; set; }

        [XmlElement("ICH_SHIPMENT_TERM")]
        public string ICH_SHIPMENT_TERM { get; set; }

        [XmlElement("ICH_SHIPMENT_TERM_PK")]
        public int ICH_SHIPMENT_TERM_PK { get; set; }

        [XmlElement("ICH_SHIPMENT_TERM_PK_VALUE")]
        public int ICH_SHIPMENT_TERM_PK_VALUE { get; set; }

        [XmlElement("ICH_SHIPMENT_TERM_PK_TEXT")]
        public string ICH_SHIPMENT_TERM_PK_TEXT { get; set; }

        [XmlElement("ICH_EFFECT_DATE")]
        public string ICH_EFFECT_DATE { get; set; }

        [XmlElement("ICH_SHIP_TO_ADDRESS")]
        public string ICH_SHIP_TO_ADDRESS { get; set; }

        [XmlElement("ICH_SO_NO")]
        public string ICH_SO_NO { get; set; }
        [XmlElement("ICH_INV_TERM_VALUE")]
        public string ICH_INV_TERM_VALUE { get; set; }
        [XmlElement("ICH_SOH_DT")]
        public string ICH_SOH_DT { get; set; }
        [XmlElement("ICH_SO_AMOUNT_NET_TC")]
        public string ICH_SO_AMOUNT_NET_TC { get; set; }
        [XmlElement("ICH_INV_AMT")]
        public string ICH_INV_AMT { get; set; }
        [XmlElement("ICH_INV_PAMT")]
        public string ICH_INV_PAMT { get; set; }
        [XmlElement("ICH_TYPE_TEXT")]
        public string ICH_TYPE_TEXT { get; set; }

        [XmlElement("ICH_FAX")]
        public string ICH_FAX { get; set; }
        [XmlElement("ICH_ORDER")]
        public string ICH_ORDER { get; set; }
        [XmlElement("ICH_ACTIVE")]
        public string ICH_ACTIVE { get; set; }
        [XmlElement("ACTIVE")]
        public byte ACTIVE { get; set; }
        [XmlElement("ICH_DEPT")]
        public short ICH_DEPT { get; set; }
        [XmlElement("ICH_DEPT_TEXT")]
        public string ICH_DEPT_TEXT { get; set; }
        [XmlElement("ICH_BIZUNIT")]
        public short ICH_BIZUNIT { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("USER_PK")]
        public short USER_PK { get; set; }
        [XmlElement("ICH_GROUP")]
        public byte ICH_GROUP { get; set; }
        [XmlElement("ICH_CATEGORY")]
        public byte ICH_CATEGORY { get; set; }

        [XmlElement("ICH_TERM1")]
        public string ICH_TERM1 { get; set; }
        [XmlElement("ICH_TERM2")]
        public string ICH_TERM2 { get; set; }

        [XmlElement("ICH_NET_WT")]
        public double ICH_NET_WT { get; set; }
        [XmlElement("ICH_GROSS_WT")]
        public double ICH_GROSS_WT { get; set; }
        [XmlElement("ICH_WT_UOM")]
        public int ICH_WT_UOM { get; set; }
        [XmlElement("ICH_WT_UOM_TEXT")]
        public string ICH_WT_UOM_TEXT { get; set; }
        [XmlElement("ICH_DESPATCH_NO")]
        public string ICH_DESPATCH_NO { get; set; }
        [XmlElement("ICH_DESPATCH_DATE")]
        public string ICH_DESPATCH_DATE { get; set; }
        [XmlElement("ICH_DESPATCH_STATUS")]
        public int ICH_DESPATCH_STATUS { get; set; }
        [XmlElement("ICH_IS_DO_EDITED")]
        public int ICH_IS_DO_EDITED { get; set; }
        [XmlElement("ICH_IS_OPENING")]
        public int ICH_IS_OPENING { get; set; }
        [XmlElement("ICH_ADV_ADJ_DED")]
        public double ICH_ADV_ADJ_DED { get; set; }
        [XmlElement("ICH_INVOICE_AMT_FLAG")]//Flag indicates the following Checking required or not.Invoicing Amount should not be greater than SC Amount 
        public int ICH_INVOICE_AMT_FLAG { get; set; }

        [XmlElement("ICH_INV_TERM")]
        public string ICH_INV_TERM { get; set; }
        [XmlElement("ICH_INV_TERM_TEXT")]
        public string ICH_INV_TERM_TEXT { get; set; }

        [XmlElement("ICH_INV_IS_DUMMY")]
        public short ICH_INV_IS_DUMMY { get; set; }
        [XmlElement("CUS_IS_DUMMY")]
        public short CUS_IS_DUMMY { get; set; }

        [XmlElement("ICH_TO_PORT_PK")]
        public string ICH_TO_PORT_PK { get; set; }

        [XmlElement("ICH_SUB_TYPE")]
        public string ICH_SUB_TYPE { get; set; }

        [XmlElement("APT_CODE")]
        public string AST_CODE { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("ICH_SALES_CATEGORY")]
        public int ICH_SALES_CATEGORY { get; set; }


        [XmlElement("OrderDetail")]
        public List<SOInvoiceDetails> OrderDetail { get; set; }
        [XmlElement("TaxHdr")]
        public List<SOInvoiceTaxHdr> TaxHdr { get; set; }
        [XmlElement("AdvDedDtl")]
        public List<SOAdvDeductionDetails> DeductionDetails { get; set; }
        [XmlElement("DueDetail")]
        public List<DueDetail> DueDetail { get; set; }
        [XmlElement("SOMpg")]
        public List<SOInvoiceMappingDetails> SOMappingDetails { get; set; }
        [XmlElement("FileList")]
        public List<SOInvoiceUploads> FileList { get; set; }
        [XmlElement("CustomsChrg")]
        public List<SOInvoiceCustomsOtherCharge> CustomsChrg { get; set; }

       
    }
    [Serializable]
    public class SOInvoiceDetails
    {
        [XmlElement("CID_PK")]
        public int CID_PK { get; set; }
        [XmlElement("CID_SL_UK")]
        public int CID_SL_UK { get; set; }
        [XmlElement("CID_SO")]
        public string CID_SO { get; set; }
        [XmlElement("CID_SO_NO")]
        public string CID_SO_NO { get; set; }
        [XmlElement("CMP_DISPLAY_CODE")]
        public string CMP_DISPLAY_CODE { get; set; }
        [XmlElement("CID_SO_DTL")]
        public string CID_SO_DTL { get; set; }
        [XmlElement("CID_INVOICE_HDR")]
        public int CID_INVOICE_HDR { get; set; }
        [XmlElement("CID_VERSION")]
        public string CID_VERSION { get; set; }
        [XmlElement("CID_SL_NO")]
        public int CID_SL_NO { get; set; }
        [XmlElement("CID_CUST_ITEM")]
        public string CID_CUST_ITEM { get; set; }
        [XmlElement("CID_CUST_ITEM_TEXT")]
        public string CID_CUST_ITEM_TEXT { get; set; }
        [XmlElement("CID_PACKING_SPEC")]
        public string CID_PACKING_SPEC { get; set; }
        [XmlElement("CID_PACKING_SPEC_TEXT")]
        public string CID_PACKING_SPEC_TEXT { get; set; }

        [XmlElement("CID_CIM_PCS_PER_IP")]
        public string CID_CIM_PCS_PER_IP { get; set; }
        [XmlElement("CID_CIM_PCS_PER_OP")]
        public string CID_CIM_PCS_PER_OP { get; set; }

        [XmlElement("CID_ITEM")]
        public int CID_ITEM { get; set; }
        [XmlElement("CID_ITEM_TEXT")]
        public string CID_ITEM_TEXT { get; set; }
        [XmlElement("CID_ORDERED_QTY")]
        public double CID_ORDERED_QTY { get; set; }
        [XmlElement("CID_INV_QTY")]
        public double CID_INV_QTY { get; set; }
        [XmlElement("CID_PINV_QTY")]
        public double CID_PINV_QTY { get; set; }
        [XmlElement("CID_INV_QTY_NOW")]
        public double CID_INV_QTY_NOW { get; set; }
        [XmlElement("CID_QTY_CARTONS")]
        public double CID_QTY_CARTONS { get; set; }
        [XmlElement("CID_UOM")]
        public int CID_UOM { get; set; }
        [XmlElement("CID_UOM_TEXT")]
        public string CID_UOM_TEXT { get; set; }
        [XmlElement("CID_RATE")]
        public double CID_RATE { get; set; }
        [XmlElement("CID_AMOUNT")]
        public double CID_AMOUNT { get; set; }
        [XmlElement("CID_QTY_DO_DISPATCHED")]
        public double CID_QTY_DO_DISPATCHED { get; set; }
        [XmlElement("CID_DISCOUNT")]
        public double CID_DISCOUNT { get; set; }
        [XmlElement("CID_TAX")]
        public double CID_TAX { get; set; }
        [XmlElement("CID_NET_AMOUNT")]
        public double CID_NET_AMOUNT { get; set; }
        [XmlElement("CID_INSTRUCTIONS")]
        public string CID_INSTRUCTIONS { get; set; }
        [XmlElement("CID_QTY_INVOICED")]
        public double CID_QTY_INVOICED { get; set; }
        [XmlElement("CID_REMARKS")]
        public string CID_REMARKS { get; set; }
        [XmlElement("CID_QTY_DISPATCHED")]
        public double CID_QTY_DISPATCHED { get; set; }

        [XmlElement("CID_SALE_QTY")]
        public double CID_SALE_QTY { get; set; }
        [XmlElement("CID_SALE_UOM")]
        public int CID_SALE_UOM { get; set; }
        [XmlElement("CID_SALE_UOM_TEXT")]
        public string CID_SALE_UOM_TEXT { get; set; }
        [XmlElement("CID_SALE_UOM_CONV")]
        public double CID_SALE_UOM_CONV { get; set; }

        [XmlElement("SOH_TOTAL_ADJUST")]
        public double SOH_TOTAL_ADJUST { get; set; }
        [XmlElement("SOH_TOTAL_TAX")]
        public double SOH_TOTAL_TAX { get; set; }
        [XmlElement("SOH_TOTAL_SHIP_CHARGE")]
        public double SOH_TOTAL_SHIP_CHARGE { get; set; }
        [XmlElement("SOH_TOTAL_DISCOUNT")]
        public double SOH_TOTAL_DISCOUNT { get; set; }

        [XmlElement("CID_ITEM_CATEGORY")]
        public int CID_ITEM_CATEGORY { get; set; }
        [XmlElement("CID_ITEM_CATEGORY_TEXT")]
        public string CID_ITEM_CATEGORY_TEXT { get; set; }
        [XmlElement("CURRENCY")]
        public int CURRENCY { get; set; }
        [XmlElement("CURRENCY_TEXT")]
        public string CURRENCY_TEXT { get; set; }

        [XmlElement("CID_ORDERED_DISC")]
        public double CID_ORDERED_DISC { get; set; }
        [XmlElement("CID_ORDERED_TAX")]
        public double CID_ORDERED_TAX { get; set; }

        [XmlElement("SOD_IS_PACK_MAT")]
        public int SOD_IS_PACK_MAT { get; set; }

        [XmlElement("TaxDtl")]
        public List<SOInvoiceTaxHdr> TaxDtl { get; set; }
        [XmlElement("IssueDetail")]
        public List<IssueDetail> IssueDtl { get; set; }
    }

    [Serializable]
    public class SOInvoiceTaxHdr
    {
        [XmlElement("CIT_PK")]
        public int CIT_PK { get; set; }
        [XmlElement("CIT_INVOICE_DTL")]
        public int CIT_INVOICE_DTL { get; set; }
        [XmlElement("CIT_SO_DTL")]
        public string CIT_SO_DTL { get; set; }
        [XmlElement("CIT_TYPE")]
        public short CIT_TYPE { get; set; }
        [XmlElement("CIT_TAX")]
        public int CIT_TAX { get; set; }
        [XmlElement("CIT_SL_NO")]
        public int CIT_SL_NO { get; set; }
        [XmlElement("CIT_TAX_TEXT")]
        public string CIT_TAX_TEXT { get; set; }
        [XmlElement("CIT_TAX_FORMULA")]
        public string CIT_TAX_FORMULA { get; set; }
        [XmlElement("CIT_NAME")]
        public string CIT_NAME { get; set; }
        [XmlElement("CIT_TAX_AMT")]
        public double CIT_TAX_AMT { get; set; }
        [XmlElement("CIT_TAX_CATEGORY_TEXT")]
        public string CIT_TAX_CATEGORY_TEXT { get; set; }
        [XmlElement("CIT_TAX_CATEGORY")]
        public int CIT_TAX_CATEGORY { get; set; }
        [XmlElement("CIT_TAX_CODE")]
        public string CIT_TAX_CODE { get; set; }
        [XmlElement("CIT_TAX_RATE")]
        public double CIT_TAX_RATE { get; set; }

        [XmlElement("CIT_TAX_CID_AMOUNT")] //this is for keeping amount before applying tax
        public string CIT_TAX_CID_AMOUNT { get; set; }
        [XmlElement("CIT_SO")]
        public int CIT_SO { get; set; }
        [XmlElement("CIT_SO_NO")]
        public string CIT_SO_NO { get; set; }
        [XmlElement("CIT_SOH_DT")]
        public string CIT_SOH_DT { get; set; }
        [XmlElement("CIT_SO_TAX_AMT")]
        public double CIT_SO_TAX_AMT { get; set; }  //SO TAX AMOUNT
        [XmlElement("CIT_INV_TAX_AMT")]
        public double CIT_INV_TAX_AMT { get; set; } //INVOICED AMOUNT
        [XmlElement("CIT_TAX_IS_FOB_CAL")]
        public int CIT_TAX_IS_FOB_CAL { get; set; }
        [XmlElement("CIT_IS_FOB")]
        public int CIT_IS_FOB { get; set; }
        [XmlElement("CIT_HAS_SUB_TOTAL")]
        public int CIT_HAS_SUB_TOTAL { get; set; }
        [XmlElement("CIT_HAS_DISCOUNT")]
        public int CIT_HAS_DISCOUNT { get; set; }
        [XmlElement("CIT_HAS_OTHER_CHARGE")]
        public int CIT_HAS_OTHER_CHARGE { get; set; }

        [XmlElement("CIT_AUTO_CALC_OTHER_CHARGE")]
        public int CIT_AUTO_CALC_OTHER_CHARGE { get; set; }
    }

    [Serializable]
   
    public class SOInvoiceCustomsOtherCharge
    {
        [XmlElement("ICC_SL_NO")]
        public int ICC_SL_NO { get; set; }
        [XmlElement("ICC_TYPE_NAME")]
        public string ICC_TYPE_NAME { get; set; }
        [XmlElement("ICC_AMT")]
        public double ICC_AMT { get; set; }
            
    }

    [Serializable]
    public class SOAdvDeductionDetails
    {
        [XmlElement("IAD_PK")]
        public long IAD_PK { get; set; }
        [XmlElement("IAD_INVOICE_ADV")]
        public long IAD_INVOICE_ADV { get; set; }
        [XmlElement("IAD_AMOUNT")]
        public decimal IAD_AMOUNT { get; set; }
        [XmlElement("IAD_OTHER_AMOUNT")]
        public decimal IAD_OTHER_AMOUNT { get; set; }
        [XmlElement("IAD_ADJUST_AMOUNT")]
        public decimal IAD_ADJUST_AMOUNT { get; set; }
        [XmlElement("IAD_TAX_AMOUNT")]
        public decimal IAD_TAX_AMOUNT { get; set; }
        [XmlElement("IAD_DISC_AMOUNT")]
        public decimal IAD_DISC_AMOUNT { get; set; }
        [XmlElement("IAD_REMARKS")]
        public string IAD_REMARKS { get; set; }
        [XmlElement("IAD_ACTIVE")]
        public byte IAD_ACTIVE { get; set; }
        [XmlElement("ICH_DATE")]
        public string ICH_DATE { get; set; }
        [XmlElement("ICH_NO")]
        public string ICH_NO { get; set; }
        [XmlElement("ICH_SO_NO")]
        public string ICH_SO_NO { get; set; }
        [XmlElement("ICH_AMOUNT_NET_TC")]
        public decimal ICH_AMOUNT_NET_TC { get; set; }
        [XmlElement("ICH_AMOUNT_NET_BC")]
        public decimal ICH_AMOUNT_NET_BC { get; set; }
        [XmlElement("ICH_AMOUNT_ALLOCATED")]
        public decimal ICH_AMOUNT_ALLOCATED { get; set; }
        [XmlElement("ICH_OTHER_AMT_ALLOCATED")]
        public decimal ICH_OTHER_AMT_ALLOCATED { get; set; }
        [XmlElement("ICH_TAX_AMT_ALLOCATED")]
        public decimal ICH_TAX_AMT_ALLOCATED { get; set; }
        [XmlElement("ICH_AMOUNT_TC")]
        public decimal ICH_AMOUNT_TC { get; set; }
        [XmlElement("ICH_DISCOUNT_TC")]
        public decimal ICH_DISCOUNT_TC { get; set; }
        [XmlElement("ICH_TAX_TC")]
        public decimal ICH_TAX_TC { get; set; }
        [XmlElement("ICH_EXCHG_RATE")]
        public double ICH_EXCHG_RATE { get; set; }
        [XmlElement("IAD_RECEIPT_HDR")]
        public long IAD_RECEIPT_HDR { get; set; }
        [XmlElement("RCH_DATE")]
        public string RCH_DATE { get; set; }
        [XmlElement("RCH_NO")]
        public string RCH_NO { get; set; }

        [XmlElement("ICM_DISCOUNT_AMOUNT")]
        public decimal ICM_DISCOUNT_AMOUNT { get; set; }
        [XmlElement("ICM_ADJUST_AMOUNT")]
        public decimal ICM_ADJUST_AMOUNT { get; set; }

        [XmlElement("RCH_SO_RCVD_AMT")]
        public string RCH_SO_RCVD_AMT { get; set; }
        [XmlElement("RCH_SO_RCVD_OTHER_AMT")]
        public string RCH_SO_RCVD_OTHER_AMT { get; set; }
        [XmlElement("RCH_SO_RCVD_TAX_AMT")]
        public string RCH_SO_RCVD_TAX_AMT { get; set; }


        [XmlElement("RCM_RCVD_AMOUNT")]
        public decimal RCM_RCVD_AMOUNT { get; set; }
        [XmlElement("RCM_DISC_AMOUNT")]
        public decimal RCM_DISC_AMOUNT { get; set; }
        [XmlElement("RCM_TAX_AMOUNT")]
        public decimal RCM_TAX_AMOUNT { get; set; }
        [XmlElement("RCM_PK")]
        public decimal RCM_PK { get; set; }

        [XmlElement("CUS_ADV_FLAG")]
        public string CUS_ADV_FLAG { get; set; }

        [XmlElement("IAD_SO")]
        public int IAD_SO { get; set; }
        [XmlElement("ICM_SO_HDR")]
        public int ICM_SO_HDR { get; set; }
        [XmlElement("ICH_REFERENCE")]
        public string ICH_REFERENCE { get; set; }

    }

    //For Saving Invoice Due Date Details(popup)
    [Serializable]
    public class DueDetail
    {
        [XmlElement("IDD_PK")]
        public string IDD_PK { get; set; }
        [XmlElement("IDD_INVOICE_HDR")]
        public string IDD_INVOICE_HDR { get; set; }
        [XmlElement("IDD_TERM_DTL")]
        public string IDD_TERM_DTL { get; set; }
        [XmlElement("IDD_DUE_DATE")]
        public string IDD_DUE_DATE { get; set; }
        [XmlElement("IDD_DUE_AMOUNT")]
        public decimal IDD_DUE_AMOUNT { get; set; }
    }

    [Serializable]
    public class IssueDetail
    {
        [XmlElement("CII_PK")]
        public long CII_PK { get; set; }
        [XmlElement("CII_TYPE")]
        public int CII_TYPE { get; set; }
        [XmlElement("CII_INVOICE_DTL")]
        public int CII_INVOICE_DTL { get; set; }
        [XmlElement("CII_ITEM_CONS_DTL")]
        public int CII_ITEM_CONS_DTL { get; set; }
        [XmlElement("CII_BC_CONS_DTL")]
        public int CII_BC_CONS_DTL { get; set; }
        [XmlElement("CII_ISSUE_NO")]
        public string CII_ISSUE_NO { get; set; }
        [XmlElement("CII_ISSUE_DATE")]
        public string CII_ISSUE_DATE { get; set; }
        [XmlElement("CII_ISSUE_PK")]
        public long CII_ISSUE_PK { get; set; }
        [XmlElement("CII_ISSUE_DTL")]
        public long CII_ISSUE_DTL { get; set; }
        [XmlElement("CII_QTY_ISSUED")]
        public double CII_QTY_ISSUED { get; set; }
        [XmlElement("CII_QTY_PRV_INVOICED")]
        public double CII_QTY_PRV_INVOICED { get; set; }
        [XmlElement("CII_QTY_BALANCE")]
        public double CII_QTY_BALANCE { get; set; }
        [XmlElement("CII_QTY_INVOICED")]
        public double CII_QTY_INVOICED { get; set; }
        [XmlElement("CII_UOM")]
        public int CII_UOM { get; set; }
        [XmlElement("CII_UOM_CODE")]
        public string CII_UOM_CODE { get; set; }
        [XmlElement("CII_SL_NO")]
        public int CII_SL_NO { get; set; }


    }

    [Serializable]
    public class SOInvoiceMappingDetails
    {
        [XmlElement]
        public long ICM_PK { get; set; }
        [XmlElement]
        public int ICM_SO_HDR { get; set; }
        [XmlElement]
        public double ICM_AMOUNT { get; set; }
        [XmlElement]
        public byte ICM_ACTIVE { get; set; }
        [XmlElement]
        public double ICM_OTHER_AMOUNT { get; set; }
        [XmlElement]
        public double ICM_TAX_AMOUNT { get; set; }
        [XmlElement]
        public double ICM_DISCOUNT_AMOUNT { get; set; }
        [XmlElement]
        public double ICM_ADJUST_AMOUNT { get; set; }
    }


    [Serializable]
    [XmlRoot("ROOT")]
    public class DueDateDetails
    {
        [XmlElement("TCH_PK")]
        public int TCH_PK { get; set; }
        [XmlElement("TRX_TYPE")]
        public string TRX_TYPE { get; set; }
        [XmlElement("TRX_PK")]
        public int TRX_PK { get; set; }
        [XmlElement("AMOUNT")]
        public decimal AMOUNT { get; set; }
        [XmlElement("ETD")]
        public string ETD { get; set; }
        [XmlElement("ETA")]
        public string ETA { get; set; }
        [XmlElement("ICH_DATE")]
        public string ICH_DATE { get; set; }
        [XmlElement("SOH_DATE")]
        public string SOH_DATE { get; set; }
    }


    [Serializable]
    [XmlRoot("Root")]
    public class AgentCommInvHeader
    {
        [XmlElement("IVH_NO")]
        public string IVH_NO { get; set; }
        [XmlElement("IVH_PK")]
        public int IVH_PK { get; set; }
        [XmlElement("IVH_VERSION")]
        public int IVH_VERSION { get; set; }
        [XmlElement("IVH_DATE")]
        public string IVH_DATE { get; set; }
        [XmlElement("IVH_CURRENCY")]
        public int IVH_CURRENCY { get; set; }
        [XmlElement("IVH_CURRENCY_TEXT")]
        public string IVH_CURRENCY_TEXT { get; set; }
        [XmlElement("IVH_VENDOR")]
        public int IVH_VENDOR { get; set; }
        [XmlElement("IVH_TYPE")]
        public int IVH_TYPE { get; set; }
        [XmlElement("IVH_VENDOR_TEXT")]
        public string IVH_VENDOR_TEXT { get; set; }
        [XmlElement("IVH_VENDOR_NAME")]
        public string IVH_VENDOR_NAME { get; set; }
        [XmlElement("IVH_VENDOR_COUNTRY")]
        public int IVH_VENDOR_COUNTRY { get; set; }
        [XmlElement("IVH_VENDOR_COUNTRY_TEXT")]
        public string IVH_VENDOR_COUNTRY_TEXT { get; set; }
        [XmlElement("IVH_VENDOR_ZIP")]
        public string IVH_VENDOR_ZIP { get; set; }
        [XmlElement("IVH_CREDIT_DAYS")]
        public int IVH_CREDIT_DAYS { get; set; }
        [XmlElement("IVH_REMARKS")]
        public string IVH_REMARKS { get; set; }
        [XmlElement("IVH_STATUS")]
        public int IVH_STATUS { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }

        [XmlElement("IVH_BRANCH_TYPE")]
        public int IVH_BRANCH_TYPE { get; set; }
        [XmlElement("IVH_TAX_ID")]
        public string IVH_TAX_ID { get; set; }
        [XmlElement("IVH_BRANCH_TEXT")]
        public string IVH_BRANCH_TEXT { get; set; }
        [XmlElement("IVH_BRANCH_NAME")]
        public string IVH_BRANCH_NAME { get; set; }
        [XmlElement("IVH_VENDOR_CONTACT")]
        public string IVH_VENDOR_CONTACT { get; set; }

        [XmlElement("IVH_DATE_PAY_BY")]
        public string IVH_DATE_PAY_BY { get; set; }
        [XmlElement("IVH_VENDOR_INV_NO")]
        public string IVH_VENDOR_INV_NO { get; set; }
        [XmlElement("IVH_ALLOW_DUP_INV_NO")]
        public byte IVH_ALLOW_DUP_INV_NO { get; set; }
        [XmlElement("IVH_VENDOR_INV_DATE")]
        public string IVH_VENDOR_INV_DATE { get; set; }
        [XmlElement("IVH_COMPANY")]
        public string IVH_COMPANY { get; set; }
        [XmlElement("IVH_IS_SETTLED")]
        public int IVH_IS_SETTLED { get; set; }

        [XmlElement("Detail")]
        public List<AgentCommInvDetails> agtCommInvDetail { get; set; }

    }
    [Serializable]
    public class AgentCommInvDetails
    {
        [XmlElement("AVD_INV_CUS_DTL")]
        public int AVD_INV_CUS_DTL { get; set; }
        [XmlElement("AVD_INV_CUS_NO")]
        public string AVD_INV_CUS_NO { get; set; }
        [XmlElement("AVD_INV_CUS_DATE")]
        public DateTime AVD_INV_CUS_DATE { get; set; }
        [XmlElement("AVD_CUSTOMER")]
        public int AVD_CUSTOMER { get; set; }
        [XmlElement("AVD_CUSTOMER_TEXT")]
        public string AVD_CUSTOMER_TEXT { get; set; }
        [XmlElement("AVD_CUSTOMER_CODE")]
        public string AVD_CUSTOMER_CODE { get; set; }
        [XmlElement("AVD_SO_NO")]
        public string AVD_SO_NO { get; set; }
        [XmlElement("AVD_SO_HDR")]
        public int AVD_SO_HDR { get; set; }
        [XmlElement("AVD_CUST_ITEM")]
        public string AVD_CUST_ITEM { get; set; }
        [XmlElement("AVD_BRAND_NAME")]
        public string AVD_BRAND_NAME { get; set; }
        [XmlElement("VID_VERSION")]
        public int VID_VERSION { get; set; }
        [XmlElement("VID_SL_NO")]
        public int VID_SL_NO { get; set; }
        [XmlElement("VID_INVOICE_HDR")]
        public int VID_INVOICE_HDR { get; set; }
        [XmlElement("ICH_PK")]
        public int ICH_PK { get; set; }

        [XmlElement("VID_ITEM")]
        public int VID_ITEM { get; set; }
        [XmlElement("VID_ITEM_TEXT")]
        public string VID_ITEM_TEXT { get; set; }
        [XmlElement("VID_QTY_CARTON")]
        public double VID_QTY_CARTON { get; set; }
        [XmlElement("VID_QTY_INVOICED")]
        public double VID_QTY_INVOICED { get; set; }

        [XmlElement("VID_SALE_QTY")]
        public double VID_SALE_QTY { get; set; }


        [XmlElement("VID_UOM")]
        public int VID_UOM { get; set; }
        [XmlElement("VID_UOM_TEXT")]
        public string VID_UOM_TEXT { get; set; }
        [XmlElement("AVD_INV_CUS_RATE")]
        public double AVD_INV_CUS_RATE { get; set; }
        [XmlElement("AVD_INV_CUS_AMOUNT")]
        public double AVD_INV_CUS_AMOUNT { get; set; }

        [XmlElement("AVD_COMMISSION_TYPE")]
        public string AVD_COMMISSION_TYPE { get; set; }
        [XmlElement("AVD_COMMISSION_RATE")]
        public double AVD_COMMISSION_RATE { get; set; }
        [XmlElement("AVD_COMMISSION_AMT")]
        public double AVD_COMMISSION_AMT { get; set; }

        [XmlElement("AVD_COMMISSION_FORMULA")]
        public string AVD_COMMISSION_FORMULA { get; set; }

        [XmlElement("AVD_COMMISSION_FORMULA_TEXT")]
        public string AVD_COMMISSION_FORMULA_TEXT { get; set; }

        [XmlElement("AVD_INVOICED_AMT")]
        public double AVD_INVOICED_AMT { get; set; }


    }


    [Serializable]
    [XmlRoot("ROOT")]
    public class SOHeaderBO
    {
        [XmlElement("SO")]
        public List<SOHeaderListBO> SOList { get; set; }
    }

    [Serializable]
    public class SOHeaderListBO
    {
        [XmlElement("SOH_PK")]
        public int SOH_PK { get; set; }
        [XmlElement("DPH_PK")]
        public int DPH_PK { get; set; }
    }

    //  For Grid Status maintains (Handling issues during paging)
    public class SelectionInfo
    {
        public long InvoicePK
        {
            get;
            set;
        }
        public bool chkChecked
        {
            get;
            set;
        }
        public int CustomerPK
        {
            get;
            set;
        }
        public int CurrencyPK
        {
            get;
            set;
        }
        public int ApprovedStatus
        {
            get;
            set;
        }
        public bool IsPosted
        {
            get;
            set;
        }
        public decimal Tax
        {
            get;
            set;
        }
        public decimal BalanceAmt
        {
            get;
            set;
        }
        public int InvoiceType
        {
            get;
            set;
        }
        public int InvoiceTerms
        {
            get;
            set;
        }
        public bool IsCancelled
        {
            get;
            set;
        }


    }

    public class FileDetailsSI
    {
        public int SlNo
        {
            get;
            set;
        }
        public HttpPostedFile PoFile
        {
            get;
            set;
        }
    }
    [Serializable]
    public class SOInvoiceUploads
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
    [Serializable]
    [XmlRoot("Root")]
    public class FinInvoiceHeaderMul
    {
        [XmlElement("FIN_INVOICE_CUS_HDR")]
        public List<FinInvoiceHeader> InvoiceMainList { get; set; }
    }
    [Serializable]
    public class FinInvoiceHeader
    {
        [XmlElement("ICH_NO")]
        public string InvoiceNo { get; set; }
        [XmlElement("ICH_DATE")]
        public string InvoiceDate { get; set; }
        [XmlElement("ICH_TYPE")]
        public int InvoiceType { get; set; }
        [XmlElement("ICH_EXCHG_RATE")]
        public double InvoiceExchangerate { get; set; }
        [XmlElement("ICH_BASE_CURR")]
        public int InvoiceCurrCode { get; set; }
        [XmlElement("ICH_AMOUNT_TC")]
        public double SubTotal { get; set; }
        [XmlElement("ICH_PAYMENT_TERM_NAME")]
        public string PaymentTerm { get; set; }
        [XmlElement("CUS_NAME")]
        public string BuyerName { get; set; }
        [XmlElement("CUS_EMAIL")]
        public string BuyerEmail { get; set; }
        [XmlElement("CUS_PHONE")]
        public string BuyerPhone { get; set; }
        [XmlElement("CUS_TAX_NO")]
        public string BuyerTIN { get; set; }
        [XmlElement("CAD_ADDRESS")]
        public string BuyerAddress { get; set; }
        [XmlElement("CAD_BRN")]
        public string BuyerRegNo { get; set; }
        [XmlElement("CMP_NAME")]
        public string SupplierName { get; set; }
        [XmlElement("CMP_TAX_NO")]
        public string SupplierTIN { get; set; }
        [XmlElement("CMP_EMAIL")]
        public string SupplierEmail { get; set; }
        [XmlElement("CMP_PHONE")]
        public string SupplierPhone { get; set; }
        [XmlElement("CMP_ADDR1")]
        public string SupplierAddress { get; set; }
        [XmlElement("Detail")]
        public List<FinInvoiceDetails> Detail { get; set; }
    }
    [Serializable]
    public class FinInvoiceDetails
    {
        [XmlElement("CID_ORDERED_QTY")]
        public double OrderedQty { get; set; }
        [XmlElement("CID_RATE")]
        public double UnitPrice { get; set; }
        [XmlElement("CID_TAX")]
        public double Tax { get; set; }
        [XmlElement("CID_SALE_UOM_TEXT")]
        public string UOM { get; set; }
        [XmlElement("CIM_DESC")]
        public string ProductDesc { get; set; }
    }
}
