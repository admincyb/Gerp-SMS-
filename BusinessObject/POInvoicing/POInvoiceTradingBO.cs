using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;

namespace BusinessObject.POInvoicing
{
   public class POInvoiceTradingBO
    {
    }
   [Serializable]
   [XmlRoot("Root")]
   public class DirectPOInvoiceHeader : WorkflowBO
   {
       #region Variables
       [XmlElement("IVH_PK")]
       public int IVH_PK { get; set; }
       [XmlElement("IVH_NO")]
       public string IVH_NO { get; set; }
       [XmlElement("IVH_GROUP")]
       public byte IVH_GROUP { get; set; }
       [XmlElement("IVH_DATE")]
       public string IVH_DATE { get; set; }
       [XmlElement("IVH_TYPE")]
       public string IVH_TYPE { get; set; }
       [XmlElement("IVH_VERSION")]
       public int IVH_VERSION { get; set; }
       [XmlElement("IVH_STATUS")]
       public short IVH_STATUS { get; set; }
       [XmlElement("IVH_CATEGORY")]
       public byte? IVH_CATEGORY { get; set; }

       [XmlElement("IVH_VENDOR")]
       public string IVH_VENDOR { get; set; }
       [XmlElement("IVH_VENDOR_NAME")]
       public string IVH_VENDOR_NAME { get; set; }
       [XmlElement("IVH_VENDOR_TEXT")]
       public string IVH_VENDOR_TEXT { get; set; }
       [XmlElement("IVH_VENDOR_ADDRESS")]
       public string IVH_VENDOR_ADDRESS { get; set; }
       [XmlElement("IVH_VENDOR_COUNTRY")]
       public string IVH_VENDOR_COUNTRY { get; set; }
       [XmlElement("IVH_VENDOR_COUNTRY_TEXT")]
       public string IVH_VENDOR_COUNTRY_TEXT { get; set; }
       [XmlElement("IVH_VENDOR_ZIP")]
       public string IVH_VENDOR_ZIP { get; set; }
       [XmlElement("IVH_VENDOR_PHONE")]
       public string IVH_VENDOR_PHONE { get; set; }
       [XmlElement("IVH_VENDOR_MOBILE")]
       public string IVH_VENDOR_MOBILE { get; set; }
       [XmlElement("IVH_VENDOR_FAX")]
       public string IVH_VENDOR_FAX { get; set; }
       [XmlElement("IVH_VENDOR_EMAIL")]
       public string IVH_VENDOR_EMAIL { get; set; }
       [XmlElement("IVH_SHIP_CHARGE_DED")]
       public string IVH_SHIP_CHARGE_DED { get; set; }

       [XmlElement("IVH_REF_NO")]
       public string IVH_REF_NO { get; set; }
       [XmlElement("IVH_DEL_STATUS")]
       public string IVH_DEL_STATUS { get; set; }
       [XmlElement("IVH_REF_DATE")]
       public string IVH_REF_DATE { get; set; }

       [XmlElement("IVH_SHIPPING_TO")]
       public string IVH_SHIPPING_TO { get; set; }
       [XmlElement("IVH_SHIPPING_NAME")]
       public string IVH_SHIPPING_NAME { get; set; }
       [XmlElement("IVH_SHIPPING_ADDRESS")]
       public string IVH_SHIPPING_ADDRESS { get; set; }
       [XmlElement("IVH_SHIPPING_COUNTRY")]
       public string IVH_SHIPPING_COUNTRY { get; set; }
       [XmlElement("IVH_SHIPPING_COUNTRY_TEXT")]
       public string IVH_SHIPPING_COUNTRY_TEXT { get; set; }
       [XmlElement("IVH_SHIPPING_ZIP")]
       public string IVH_SHIPPING_ZIP { get; set; }
       [XmlElement("IVH_SHIPPING_PHONE")]
       public string IVH_SHIPPING_PHONE { get; set; }
       [XmlElement("IVH_SHIPPING_MOBILE")]
       public string IVH_SHIPPING_MOBILE { get; set; }
       [XmlElement("IVH_SHIPPING_FAX")]
       public string IVH_SHIPPING_FAX { get; set; }
       [XmlElement("IVH_SHIPPING_EMAIL")]
       public string IVH_SHIPPING_EMAIL { get; set; }


       [XmlElement("IVH_ORGINAL_RCVD")]
       public byte IVH_ORGINAL_RCVD { get; set; }
       [XmlElement("IVH_CURRENCY")]
       public int IVH_CURRENCY { get; set; }
       [XmlElement("IVH_CURRENCY_TEXT")]
       public string IVH_CURRENCY_TEXT { get; set; }
       [XmlElement("IVH_EXCHG_RATE")]
       public double IVH_EXCHG_RATE { get; set; }
       [XmlElement("IVH_BASE_CURR")]
       public int IVH_BASE_CURR { get; set; }

       [XmlElement("IVH_AMOUNT_TC")]
       public double IVH_AMOUNT_TC { get; set; }
       [XmlElement("IVH_DISCOUNT_TC")]
       public double IVH_DISCOUNT_TC { get; set; }
       [XmlElement("IVH_TAX_TC")]
       public double IVH_TAX_TC { get; set; }
       [XmlElement("IVH_AMOUNT_ADV_DED_TC")]
       public double IVH_AMOUNT_ADV_DED_TC { get; set; }
       [XmlElement("IVH_SHIP_CHARGE")]
       public double IVH_SHIP_CHARGE { get; set; }

       [XmlElement("IVH_SHIP_CHARGE_INV")]
       public double IVH_SHIP_CHARGE_INV { get; set; }

       [XmlElement("IVH_AMOUNT_ADJUST")]
       public double IVH_AMOUNT_ADJUST { get; set; }
       [XmlElement("IVH_AMOUNT_RCVD_TC")]
       public double IVH_AMOUNT_RCVD_TC { get; set; }
       [XmlElement("IVH_AMOUNT_NET_TC")]
       public double IVH_AMOUNT_NET_TC { get; set; }
       [XmlElement("IVH_AMOUNT_NET_BC")]
       public double IVH_AMOUNT_NET_BC { get; set; }
       [XmlElement("IVH_AMOUNT_PAID_TC")]
       public double IVH_AMOUNT_PAID_TC { get; set; }
       [XmlElement("IVH_TOTAL_QTY")]
       public double IVH_TOTAL_QTY { get; set; }
       [XmlElement("IVH_DATE_PAY_BY")]
       public string IVH_DATE_PAY_BY { get; set; }
       [XmlElement("IVH_COMPANY")]
       public int IVH_COMPANY { get; set; }

       [XmlElement("IVH_REMARKS")]
       public string IVH_REMARKS { get; set; }
       [XmlElement("IVH_TO_PORT")]
       public string IVH_TO_PORT { get; set; }
       [XmlElement("IVH_FROM_PORT")]
       public string IVH_FROM_PORT { get; set; }
       [XmlElement("IVH_FROM_PORT_TEXT")]
       public string IVH_FROM_PORT_TEXT { get; set; }

       [XmlElement("IVH_DEL_TERM_TEXT")]
       public string IVH_DEL_TERM_TEXT { get; set; }
       [XmlElement("IVH_REFERENCE")]
       public string IVH_REFERENCE { get; set; }
       [XmlElement("IVH_ORG_GOODS")]
       public string IVH_ORG_GOODS { get; set; }
       [XmlElement("IVH_ORG_GOODS_TEXT")]
       public string IVH_ORG_GOODS_TEXT { get; set; }

       [XmlElement("IVH_FEEDER_VESSEL")]
       public string IVH_FEEDER_VESSEL { get; set; }
       [XmlElement("IVH_MOTHER_VESSEL")]
       public string IVH_MOTHER_VESSEL { get; set; }
       [XmlElement("IVH_ETD")]
       public string IVH_ETD { get; set; }
       [XmlElement("IVH_ETA")]
       public string IVH_ETA { get; set; }
       [XmlElement("IVH_SHIPPING_MARK")]
       public string IVH_SHIPPING_MARK { get; set; }
       [XmlElement("IVH_CONTAINER_NO")]
       public string IVH_CONTAINER_NO { get; set; }
       [XmlElement("IVH_FINAL_DESTINATION")]
       public string IVH_FINAL_DESTINATION { get; set; }
       [XmlElement("IVH_PAYMENT_TERM_TEXT")]
       public string IVH_PAYMENT_TERM_TEXT { get; set; }

       [XmlElement("IVH_PO_NO")]
       public string IVH_PO_NO { get; set; }
       [XmlElement("IVH_POH_DT")]
       public string IVH_POH_DT { get; set; }
       [XmlElement("IVH_PO_AMOUNT_NET_TC")]
       public string IVH_PO_AMOUNT_NET_TC { get; set; }
       [XmlElement("IVH_INV_AMT")]
       public string IVH_INV_AMT { get; set; }
       [XmlElement("IVH_TYPE_TEXT")]
       public string IVH_TYPE_TEXT { get; set; }

       [XmlElement("IVH_PR_NO")]
       public string IVH_PR_NO { get; set; }
       [XmlElement("IVH_PRH_PK")]
       public string IVH_PRH_PK { get; set; }

       [XmlElement("IVH_VENDOR_INV_NO")]
       public string IVH_VENDOR_INV_NO { get; set; }
       [XmlElement("IVH_ALLOW_DUP_INV_NO")]
       public byte IVH_ALLOW_DUP_INV_NO { get; set; }
       [XmlElement("IVH_VENDOR_INV_DATE")]
       public string IVH_VENDOR_INV_DATE { get; set; }
       [XmlElement("IVH_CREDIT_DAYS")]
       public string IVH_CREDIT_DAYS { get; set; }
       [XmlElement("IVH_TRANSPORT")]
       public string IVH_TRANSPORT { get; set; }

       [XmlElement("IVH_VENDOR_CONTACT")]
       public string IVH_VENDOR_CONTACT { get; set; }

       [XmlElement("IVH_FAX")]
       public string IVH_FAX { get; set; }
       [XmlElement("IVH_ORDER")]
       public string IVH_ORDER { get; set; }
       [XmlElement("IVH_ACTIVE")]
       public string IVH_ACTIVE { get; set; }
       [XmlElement("ACTIVE")]
       public byte ACTIVE { get; set; }
       [XmlElement("IVH_DEPT")]
       public short IVH_DEPT { get; set; }
       [XmlElement("IVH_DEPT_TEXT")]
       public string IVH_DEPT_TEXT { get; set; }
       [XmlElement("IVH_BIZUNIT")]
       public short IVH_BIZUNIT { get; set; }

       [XmlElement("LAST_MOD_DT")]
       public DateTime LAST_MOD_DT { get; set; }
       [XmlElement("USER_PK")]
       public short USER_PK { get; set; }
       [XmlElement("POH_ITEM_TYPE")]
       public short POH_ITEM_TYPE { get; set; }

       //PODiscount,PoTax,Po ShipCharge       
       [XmlElement("POH_DISCOUNT_TC")]
       public decimal POH_DISCOUNT_TC { get; set; }
       [XmlElement("POH_TAX_TC")]
       public decimal POH_TAX_TC { get; set; }
       [XmlElement("POH_SHIP_CHARGE")]
       public decimal POH_SHIP_CHARGE { get; set; }
       [XmlElement("IVH_IS_OPENING")]
       public int IVH_IS_OPENING { get; set; }
       [XmlElement("IVH_VENDOR_ACCOUNT")]
       public int IVH_VENDOR_ACCOUNT { get; set; }
       [XmlElement("IVH_DATE_RECEIVED")]
       public string IVH_DATE_RECEIVED { get; set; }


       [XmlElement("APT_CODE")]
       public string APT_CODE { get; set; }
       [XmlElement("AST_DOC_MODE")]
       public int AST_DOC_MODE { get; set; }
       [XmlElement("AST_VALUE")]
       public string AST_VALUE { get; set; }
       [XmlElement("WKF_FLAG")]
       public int WKF_FLAG { get; set; }


       [XmlElement("IVH_HAS_JRNL_ENTRY")]
       public bool IVH_HAS_JRNL_ENTRY { get; set; }


       [XmlElement("IVH_BRANCH_TYPE")]
       public int IVH_BRANCH_TYPE { get; set; }
       [XmlElement("IVH_TAX_ID")]
       public string IVH_TAX_ID { get; set; }
       [XmlElement("IVH_BRANCH_TEXT")]
       public string IVH_BRANCH_TEXT { get; set; }
       [XmlElement("IVH_BRANCH_NAME")]
       public string IVH_BRANCH_NAME { get; set; }
       //Declaration No
       [XmlElement("IVH_IMP_DECL_NO")]
       public string IVH_IMP_DECL_NO { get; set; }
       [XmlElement("IVH_AMOUNT_NET_TC_ADJ")]
       public double IVH_AMOUNT_NET_TC_ADJ { get; set; }

       //Alert
       [XmlElement("ALERT_FLAG")]
       public byte ALERT_FLAG { get; set; }
       [XmlElement("ATH_NO")]
       public string ATH_NO { get; set; }
       [XmlElement("ATH_DATE")]
       public string ATH_DATE { get; set; }
       [XmlElement("ATH_TRX_TYPE")]
       public string ATH_TRX_TYPE { get; set; }
       [XmlElement("ATH_TRX_PK")]
       public string ATH_TRX_PK { get; set; }
       [XmlElement("ATH_TRX_DATE")]
       public string ATH_TRX_DATE { get; set; }
       [XmlElement("ATH_DUE_DAYS")]
       public string ATH_DUE_DAYS { get; set; }
       [XmlElement("ATH_DUE_DATE")]
       public string ATH_DUE_DATE { get; set; }
       [XmlElement("ATH_NAME")]
       public string ATH_NAME { get; set; }
       [XmlElement("ATH_BASIS")]
       public string ATH_BASIS { get; set; }
       [XmlElement("ATH_ALERT_TYPE")]
       public string ATH_ALERT_TYPE { get; set; }
       [XmlElement("ATH_NOTIFY_BFR")]
       public string ATH_NOTIFY_BFR { get; set; }
       [XmlElement("ATH_NOTIFY_BFR_UOM")]
       public string ATH_NOTIFY_BFR_UOM { get; set; }
       [XmlElement("ATH_REMARKS")]
       public string ATH_REMARKS { get; set; }
       [XmlElement("ATH_NARRATION")]
       public string ATH_NARRATION { get; set; }
       [XmlElement("ATH_NOTIFY_MESSAGE")]
       public string ATH_NOTIFY_MESSAGE { get; set; }
       [XmlElement("ATH_NOTIFY_EMAIL")]
       public string ATH_NOTIFY_EMAIL { get; set; }
       [XmlElement("ATH_NOTIFY_SMS")]
       public string ATH_NOTIFY_SMS { get; set; }
       [XmlElement("ATH_STATUS")]
       public string ATH_STATUS { get; set; }

       [XmlElement("ATL_ACTION")]
       public byte? ATL_ACTION { get; set; }
       [XmlElement("ATL_APP_TYPE")]
       public string ATL_APP_TYPE { get; set; } 
       #endregion
       
       [XmlElement("OrderDetail")]
       public List<DirectPOInvoiceDetails> OrderDetail { get; set; }
       [XmlElement("TaxHdr")]
       public List<DirectPOInvoiceTaxHdr> TaxHdr { get; set; }
       [XmlElement("AdvDedDtl")]
       public List<DirectPOAdvDeductionDetails> DeductionDetails { get; set; }
       [XmlElement("POMpg")]
       public List<DirectPOInvoiceMappingDetails> POMappingDetails { get; set; }
       [XmlElement("FileList")]
       public List<DirectPOInvoiceUploads> FileList { get; set; }     
   }

   [Serializable]
   public class DirectPOInvoiceDetails
   {
       [XmlElement("VID_PK")]
       public int VID_PK { get; set; }
       [XmlElement("VID_SL_UK")]
       public int VID_SL_UK { get; set; }
       [XmlElement("VID_PO")]
       public string VID_PO { get; set; }
       [XmlElement("VID_PO_DTL")]
       public string VID_PO_DTL { get; set; }
       [XmlElement("VID_INVOICE_HDR")]
       public int VID_INVOICE_HDR { get; set; }
       [XmlElement("VID_VERSION")]
       public string VID_VERSION { get; set; }
       [XmlElement("VID_SL_NO")]
       public int VID_SL_NO { get; set; }
       [XmlElement("VID_CUST_ITEM")]
       public string VID_CUST_ITEM { get; set; }
       [XmlElement("VID_CUST_ITEM_TEXT")]
       public string VID_CUST_ITEM_TEXT { get; set; }
       [XmlElement("VID_PACKING_SPEC")]
       public string VID_PACKING_SPEC { get; set; }
       [XmlElement("VID_PACKING_SPEC_TEXT")]
       public string VID_PACKING_SPEC_TEXT { get; set; }

       [XmlElement("VID_CIM_PCS_PER_IP")]
       public string VID_CIM_PCS_PER_IP { get; set; }
       [XmlElement("VID_CIM_PCS_PER_OP")]
       public string VID_CIM_PCS_PER_OP { get; set; }

       [XmlElement("VID_ITEM")]
       public int VID_ITEM { get; set; }
       [XmlElement("VID_ITEM_TEXT")]
       public string VID_ITEM_TEXT { get; set; }
       [XmlElement("VID_ORDERED_QTY")]
       public double VID_ORDERED_QTY { get; set; }
       [XmlElement("VID_INV_QTY")]
       public double VID_INV_QTY { get; set; }
       [XmlElement("VID_QTY_INVOICED")]
       public double VID_QTY_INVOICED { get; set; }
       [XmlElement("VID_QTY_CARTONS")]
       public double VID_QTY_CARTONS { get; set; }
       [XmlElement("VID_UOM")]
       public int VID_UOM { get; set; }
       [XmlElement("VID_UOM_TEXT")]
       public string VID_UOM_TEXT { get; set; }
       [XmlElement("VID_RATE")]
       public double VID_RATE { get; set; }
       [XmlElement("VID_RATE_EFCT")]
       public double VID_RATE_EFCT { get; set; }
       [XmlElement("VID_AMOUNT")]
       public double VID_AMOUNT { get; set; }
       [XmlElement("VID_DISCOUNT")]
       public double VID_DISCOUNT { get; set; }
       [XmlElement("VID_TAX")]
       public double VID_TAX { get; set; }
       [XmlElement("VID_NET_AMOUNT")]
       public double VID_NET_AMOUNT { get; set; }
       [XmlElement("VID_INSTRUCTIONS")]
       public string VID_INSTRUCTIONS { get; set; }
       [XmlElement("VID_REMARKS")]
       public string VID_REMARKS { get; set; }
       [XmlElement("VID_REF_NO")]
       public string VID_REF_NO { get; set; }
       [XmlElement("VID_REF_DATE")]
       public string VID_REF_DATE { get; set; }
       [XmlElement("VID_TAX_ID")]
       public string VID_TAX_ID { get; set; }
       [XmlElement("VID_BRANCH_TEXT")]
       public string VID_BRANCH_TEXT { get; set; }
       [XmlElement("VID_BRANCH")]
       public string VID_BRANCH { get; set; }
       [XmlElement("VID_BRANCH_NAME")]
       public string VID_BRANCH_NAME { get; set; }
       [XmlElement("VID_VENDOR_TEXT")]
       public string VID_VENDOR_TEXT { get; set; }
       [XmlElement("VID_BRANCH_TYPE")]
       public int VID_BRANCH_TYPE { get; set; }
       [XmlElement("VID_VENDOR")]
       public int VID_VENDOR { get; set; }
       [XmlElement("VID_PO_NO")]
       public string VID_PO_NO { get; set; }
       [XmlElement("VID_PR_NO")]
       public string VID_PR_NO { get; set; }
       [XmlElement("VID_PR_PKS")]
       public string VID_PR_PKS { get; set; }
       [XmlElement("VID_REFUND_DATE")]
       public DateTime? VID_REFUND_DATE { get; set; }

       [XmlElement("VID_GRN_QTY")]
       public double VID_GRN_QTY { get; set; }
       [XmlElement("VID_HAS_GRN")]
       public int VID_HAS_GRN { get; set; }


       [XmlElement("TaxDtl")]
       public List<DirectPOInvoiceTaxHdr> TaxDtl { get; set; }
       [XmlElement("GRNDtl")]
       public List<DirectGRNQTYDetails> GRNDtl { get; set; }
   }

   [Serializable]
   public class DirectPOInvoiceTaxHdr
   {
       [XmlElement("VTL_PK")]
       public int VTL_PK { get; set; }
       [XmlElement("VTL_INVOICE_DTL")]
       public int VTL_INVOICE_DTL { get; set; }
       [XmlElement("VTL_PO_DTL")]
       public string VTL_PO_DTL { get; set; }
       [XmlElement("VTL_TYPE")]
       public short VTL_TYPE { get; set; }
       [XmlElement("VTL_TAX")]
       public int VTL_TAX { get; set; }
       [XmlElement("VTL_SL_NO")]
       public int VTL_SL_NO { get; set; }
       [XmlElement("VTL_TAX_TEXT")]
       public string VTL_TAX_TEXT { get; set; }
       [XmlElement("VTL_TAX_FORMULA")]
       public string VTL_TAX_FORMULA { get; set; }
       [XmlElement("VTL_NAME")]
       public string VTL_NAME { get; set; }
       [XmlElement("VTL_TAX_AMT")]
       public double VTL_TAX_AMT { get; set; }
       [XmlElement("VTL_TAX_CATEGORY_TEXT")]
       public string VTL_TAX_CATEGORY_TEXT { get; set; }
       [XmlElement("VTL_TAX_CATEGORY")]
       public int VTL_TAX_CATEGORY { get; set; }      
       [XmlElement("VTL_TAX_CODE")]
       public string VTL_TAX_CODE { get; set; }
       [XmlElement("VTL_TAX_RATE")]
       public string VTL_TAX_RATE { get; set; }
       [XmlElement("VTL_TAX_VID_AMOUNT")] //this is for keeping amount before applying tax
       public string VTL_TAX_VID_AMOUNT { get; set; }

       [XmlElement("VTL_PO")]
       public int VTL_PO { get; set; }
       [XmlElement("VTL_PO_NO")]
       public string VTL_PO_NO { get; set; }
       [XmlElement("VTL_POH_DT")]
       public string VTL_POH_DT { get; set; }
       [XmlElement("VTL_PO_TAX_AMT")]
       public double VTL_PO_TAX_AMT { get; set; }  //SO TAX AMOUNT
       [XmlElement("VTL_INV_TAX_AMT")]
       public double VTL_INV_TAX_AMT { get; set; } //INVOICED AMOUNT
       [XmlElement("VTL_TAX_IS_FOB_CAL")]
       public int VTL_TAX_IS_FOB_CAL { get; set; }
       [XmlElement("VTL_IS_FOB")]
       public int VTL_IS_FOB { get; set; }

   }

   [Serializable]
   public class DirectPOAdvDeductionDetails
   {
       [XmlElement("VAD_PK")]
       public long VAD_PK { get; set; }
       [XmlElement("VAD_INVOICE_ADV")]
       public long VAD_INVOICE_ADV { get; set; }
       [XmlElement("VAD_AMOUNT")]
       public decimal VAD_AMOUNT { get; set; }
       [XmlElement("VAD_OTHER_AMOUNT")]
       public decimal VAD_OTHER_AMOUNT { get; set; }
       [XmlElement("VAD_ADJUST_AMOUNT")]
       public decimal VAD_ADJUST_AMOUNT { get; set; }
       [XmlElement("VAD_TAX_AMOUNT")]
       public decimal VAD_TAX_AMOUNT { get; set; }
       [XmlElement("VAD_REMARKS")]
       public string VAD_REMARKS { get; set; }
       [XmlElement("VAD_ACTIVE")]
       public byte VAD_ACTIVE { get; set; }
       [XmlElement("VAD_DISC_AMOUNT")]
       public decimal VAD_DISC_AMOUNT { get; set; }
       [XmlElement("IVH_DATE")]
       public string IVH_DATE { get; set; }
       [XmlElement("IVH_NO")]
       public string IVH_NO { get; set; }
       [XmlElement("IVH_AMOUNT_NET_TC")]
       public decimal IVH_AMOUNT_NET_TC { get; set; }
       [XmlElement("IVH_AMOUNT_NET_BC")]
       public decimal IVH_AMOUNT_NET_BC { get; set; }
       [XmlElement("IVH_AMOUNT_ALLOCATED")]
       public decimal IVH_AMOUNT_ALLOCATED { get; set; }
       [XmlElement("IVH_OTHER_AMT_ALLOCATED")]
       public decimal IVH_OTHER_AMT_ALLOCATED { get; set; }
       [XmlElement("IVH_TAX_AMT_ALLOCATED")]
       public decimal IVH_TAX_AMT_ALLOCATED { get; set; }
       [XmlElement("IVH_AMOUNT_TC")]
       public decimal IVH_AMOUNT_TC { get; set; }
       [XmlElement("IVH_DISCOUNT_TC")]
       public decimal IVH_DISCOUNT_TC { get; set; }
       [XmlElement("IVM_ADJUST_AMOUNT")]
       public decimal IVM_ADJUST_AMOUNT { get; set; }
       [XmlElement("IVM_DISCOUNT_AMOUNT")]
       public decimal IVM_DISCOUNT_AMOUNT { get; set; }
       [XmlElement("IVH_TAX_TC")]
       public decimal IVH_TAX_TC { get; set; }
       [XmlElement("IVH_EXCHG_RATE")]
       public double IVH_EXCHG_RATE { get; set; }
       [XmlElement("VAD_PAYMENT_HDR")]
       public long VAD_PAYMENT_HDR { get; set; }
       [XmlElement("PVH_DATE")]
       public string PVH_DATE { get; set; }
       [XmlElement("PVH_NO")]
       public string PVH_NO { get; set; }
       [XmlElement("PVH_PO_PAID_AMT")]
       public string PVH_PO_PAID_AMT { get; set; }
       [XmlElement("PVH_PO_PAID_OTHER_AMT")]
       public string PVH_PO_PAID_OTHER_AMT { get; set; }
       [XmlElement("PVH_PO_PAID_TAX_AMT")]
       public string PVH_PO_PAID_TAX_AMT { get; set; }
       [XmlElement("PVM_PAID_AMOUNT")]
       public decimal PVM_PAID_AMOUNT { get; set; }
       [XmlElement("PVM_DISC_AMOUNT")]
       public decimal PVM_DISC_AMOUNT { get; set; }
       [XmlElement("PVM_TAX_AMOUNT")]
       public decimal PVM_TAX_AMOUNT { get; set; }
       [XmlElement("PVM_PK")]
       public decimal PVM_PK { get; set; }
       [XmlElement("IVM_PO_HDR")]
       public int IVM_PO_HDR { get; set; }
       [XmlElement("VAD_PO")]
       public int VAD_PO { get; set; }
   }  

   [Serializable]
   public class DirectPOInvoiceMappingDetails
   {
       [XmlElement]
       public long IVM_PK { get; set; }
       [XmlElement]
       public int IVM_PO_HDR { get; set; }
       [XmlElement]
       public double IVM_AMOUNT { get; set; }
       [XmlElement]
       public byte IVM_ACTIVE { get; set; }
       [XmlElement]
       public double IVM_OTHER_AMOUNT { get; set; }
       [XmlElement]
       public double IVM_TAX_AMOUNT { get; set; }
       [XmlElement]
       public double IVM_DISCOUNT_AMOUNT { get; set; }
       [XmlElement]
       public double IVM_ADJUST_AMOUNT { get; set; }
       [XmlElement]
       public string POH_NO { get; set; }
       [XmlElement]
       public string POH_DATE { get; set; }
       [XmlElement]
       public decimal PO_OTHER_AMOUNT { get; set; }
       [XmlElement]
       public decimal PO_OTHER_AMOUNT_INVOICED { get; set; }


       [XmlElement]
       public long IVM_INVOICE_HDR { get; set; }
       [XmlElement]
       public decimal POH_GROSS_AMT { get; set; }
       [XmlElement]
       public decimal POH_DISCOUNT_AMT { get; set; }
       [XmlElement]
       public decimal POH_TAX_AMT { get; set; }
       [XmlElement]
       public decimal POH_OTHER_CHARGES { get; set; }
       [XmlElement]
       public decimal POH_AJUST_AMT { get; set; }
       [XmlElement]
       public decimal POH_TOTAL_AMT { get; set; }

       [XmlElement]
       public decimal IVM_ADV_INV_AMT { get; set; }
       [XmlElement]
       public decimal IVM_INVOICED_AMT { get; set; }
       [XmlElement]
       public decimal IVM_PINVOICED_AMT { get; set; }
       [XmlElement]
       public decimal IVM_BAL_TO_INVOICE { get; set; }
       [XmlElement]
       public decimal IVM_PREV_OTHER_CHARGE { get; set; }   
       [XmlElement]
       public int POH_PK { get; set; }
       [XmlElement]
       public int POH_TYPE { get; set; }
       [XmlElement]
       public int IVH_CATEGORY { get; set; }

       [XmlElement("POTax")]
       public List<DirectPOTaxDetails> POTaxList { get; set; }
      
   }

   [Serializable]
   public class DirectPOTaxDetails
   {
       [XmlElement]
       public int PTH_TAX { get; set; }
       [XmlElement]
       public double PTH_TAX_AMT { get; set; }
       [XmlElement]
       public string PTH_NAME { get; set; }
   }

   [Serializable]
   public class DirectGRNQTYDetails
   {
       [XmlElement]
       public long VGL_PK { get; set; }
       [XmlElement]
       public long VGL_INVOICE_DTL { get; set; }
       [XmlElement]
       public int VGL_GRN_DTL { get; set; }
       [XmlElement]
       public int VGL_GRN_HDR { get; set; }
       [XmlElement]
       public string VGL_GRN_NO { get; set; }
       [XmlElement]
       public string VGL_GRN_DATE { get; set; }
       [XmlElement]
       public double VGL_QTY_INVOICED { get; set; }
       [XmlElement]
       public double VGL_QTY_INVOICED_BAL { get; set; }
       [XmlElement]
       public double GRD_QTY_APPROVED { get; set; }
       [XmlElement]
       public int VGL_UOM { get; set; }
       [XmlElement]
       public int VGL_SL_NO { get; set; }
       [XmlElement]
       public double GRN_QTY_INVOICED { get; set; }
   }
   [Serializable]
   public class DirectFileDetails
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
   //  For Grid Status maintains (Handling issues during paging)
   public class DirectSelectionInfo
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
       public int VendorPK
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

   }
   [Serializable]
   public class DirectPOOtherChargeDetails
   {
       [XmlElement]
       public long IVM_PK { get; set; }
       [XmlElement]
       public int IVM_PO_HDR { get; set; }
       [XmlElement]
       public double IVM_AMOUNT { get; set; }
       [XmlElement]
       public byte IVM_ACTIVE { get; set; }
       [XmlElement]
       public decimal IVM_OTHER_AMOUNT { get; set; }
       [XmlElement]
       public double IVM_TAX_AMOUNT { get; set; }
       [XmlElement]
       public double IVM_DISCOUNT_AMOUNT { get; set; }
       [XmlElement]
       public decimal IVM_ADJUST_AMOUNT { get; set; }
       [XmlElement]
       public string POH_NO { get; set; }
       [XmlElement]
       public string POH_DATE { get; set; }
       [XmlElement]
       public decimal PO_OTHER_AMOUNT { get; set; }
       [XmlElement]
       public decimal PO_OTHER_AMOUNT_INVOICED { get; set; }
   }

   [Serializable]
   [XmlRoot("ROOT")]
   public class DirectPOHeaderBO
   {
       [XmlElement("PO")]
       public List<DirectPOHeaderListBO> POList { get; set; }
   }

   [Serializable]
   public class DirectPOHeaderListBO
   {
       [XmlElement("POH_PK")]
       public int POH_PK { get; set; }
       [XmlElement("IVH_PK")]
       public int IVH_PK { get; set; }
       public int POH_VENDOR { get; set; }
       public int POH_TYPE { get; set; }
       public int POH_CURRENCY { get; set; }
   }

   [Serializable]
   public class DirectPOInvoiceUploads
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
}
