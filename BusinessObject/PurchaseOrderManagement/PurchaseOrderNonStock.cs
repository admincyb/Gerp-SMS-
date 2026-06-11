using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.PurchaseOrderManagement
{
    [Serializable]
    [XmlRoot("root")]
    public class PONonStock : WorkflowBO
    {
        [XmlElement("POH_VERSION")]
        public string POH_VERSION { get; set; }
        [XmlElement("POH_COMPANY")]
        public int POH_COMPANY { get; set; }
        [XmlElement("POH_TYPE_TEXT")]
        public string POH_TYPE_TEXT { get; set; }
        [XmlElement("POH_ITEM_TYPE")]
        public string POH_ITEM_TYPE { get; set; }
        [XmlElement("POH_ITEM_TYPE_TEXT")]
        public string POH_ITEM_TYPE_TEXT { get; set; }
        [XmlElement("POH_SHIP_CHARGE")]
        public string POH_SHIP_CHARGE { get; set; }
        [XmlElement("POH_VENDOR_TERMS")]
        public string POH_VENDOR_TERMS { get; set; }
        [XmlElement("VENDOR_TERMS")]
        public string VENDOR_TERMS { get; set; }
        [XmlElement("TERMS")]
        public string TERMS { get; set; }
        [XmlElement("POH_SUBMITTED_BY")]
        public string POH_SUBMITTED_BY { get; set; }
        [XmlElement("POH_SUBMITTED_DATE")]
        public string POH_SUBMITTED_DATE { get; set; }
        [XmlElement("POH_APPROVED_BY")]
        public string POH_APPROVED_BY { get; set; }
        [XmlElement("POH_APPROVED_DATE")]
        public string POH_APPROVED_DATE { get; set; }
        [XmlElement("POH_STATUS")]
        public string POH_STATUS { get; set; }
        [XmlElement("POH_ACTIVE")]
        public string POH_ACTIVE { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("VEN_NAME")]
        public string VEN_NAME { get; set; }


        [XmlElement("POH_TYPE")]
        public string POH_TYPE { get; set; }
        [XmlElement("POH_SHIPPING")]
        public string POH_SHIPPING { get; set; }
        [XmlElement("POH_BILLING")]
        public string POH_BILLING { get; set; }
        [XmlElement("POH_EXCHG_RATE")]
        public string POH_EXCHG_RATE { get; set; }
        [XmlElement("BizUnitPk")]
        public string BizUnitPk { get; set; }
        [XmlElement("UserPk")]
        public string UserPk { get; set; }
        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public string AST_DOC_MODE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("POH_VENDOR")]
        public int POH_VENDOR { get; set; }
        [XmlElement("POH_PK")]
        public int POH_PK { get; set; }
        [XmlElement("POH_NO")]
        public string POH_NO { get; set; }
        [XmlElement("POH_CRTD_BY")]
        public int POH_CRTD_BY { get; set; }
        [XmlElement("POH_CURRENCY")]
        public int POH_CURRENCY { get; set; }
        [XmlElement("POH_CURRENCY_BC")]
        public int POH_CURRENCY_BC { get; set; }
        [XmlElement("POH_TOTAL_VALUE_BC")]
        public string POH_TOTAL_VALUE_BC { get; set; }
        [XmlElement("POH_CONTRACT_REF_NO")]
        public string POH_CONTRACT_REF_NO { get; set; }
        [XmlElement("POH_DATE")]
        public string POH_DATE { get; set; }
        [XmlElement("POH_DEPT")]
        public int POH_DEPT { get; set; }
        [XmlElement("POH_SUB_TOTAL")]
        public string POH_SUB_TOTAL { get; set; }
        [XmlElement("POH_DISC_AMT")]
        public string POH_DISC_AMT { get; set; }
        [XmlElement("POH_ADD_TAX_AMT")]
        public string POH_ADD_TAX_AMT { get; set; }
        [XmlElement("POH_PRICE_ADJUST")]
        public string POH_PRICE_ADJUST { get; set; }
        [XmlElement("POH_TOTAL_VALUE")]
        public string POH_TOTAL_VALUE { get; set; }
        [XmlElement("POH_COMMENTS")]
        public string POH_COMMENTS { get; set; }
        [XmlElement("POH_GROUP")]
        public byte POH_GROUP { get; set; }
        [XmlElement("POH_TERMS")]
        public string POH_TERMS { get; set; }
        [XmlElement("POH_IS_AMEND")]
        public byte POH_IS_AMEND { get; set; }
        [XmlElement("USER_PK")]
        public short USER_PK { get; set; }

        [XmlElement("POH_FROM_PORT")]
        public string POH_FROM_PORT { get; set; }
        [XmlElement("POH_FROM_PORT_TEXT")]
        public string POH_FROM_PORT_TEXT { get; set; }
        [XmlElement("POH_TO_PORT")]
        public string POH_TO_PORT { get; set; }
        [XmlElement("POH_TO_PORT_TEXT")]
        public string POH_TO_PORT_TEXT { get; set; }

        [XmlElement("PurchaseOrderList")]
        public List<PurchaseOrderLists> PurchaseOrderList { get; set; }

    }

    [Serializable]
    [XmlRoot("PurchaseOrderList")]
    public class PurchaseOrderLists
    {
        [XmlElement("PODetails")]
        public List<PurchaseOrderDetails> PODetails { get; set; }
        [XmlElement("TaxHeader")]
        public List<PTaxHeader> TaxHdr { get; set; }
    }

    [Serializable]
    public class PurchaseOrderDetails
    {

        [XmlElement("POD_NO")]
        public string POD_NO { get; set; }
        [XmlElement("POD_DATE")]
        public string POD_DATE { get; set; }
        [XmlElement("POD_VERSION")]
        public string POD_VERSION { get; set; }
        [XmlElement("ITM_NAME")]
        public string ITM_NAME { get; set; }
        [XmlElement("ITM_CODE")]
        public string ITM_CODE { get; set; }
        [XmlElement("POD_QTY_RECEIVED")]
        public string POD_QTY_RECEIVED { get; set; }
        [XmlElement("POD_BIZUNIT")]
        public string POD_BIZUNIT { get; set; }
        [XmlElement("POD_QTY_INVOICED")]
        public string POD_QTY_INVOICED { get; set; }

        [XmlElement("POD_PK")]
        public int POD_PK { get; set; }
        [XmlElement("POD_PO")]
        public int POD_PO { get; set; }
        [XmlElement("POD_SL_NO")]
        public string POD_SL_NO { get; set; }
        [XmlElement("POD_ITEM")]
        public int POD_ITEM { get; set; }
        [XmlElement("POD_QTY_REQUESTED")]
        public string POD_QTY_REQUESTED { get; set; }

        [XmlElement("POD_UOM")]
        public string POD_UOM { get; set; }
        [XmlElement("UOM_CODE")]
        public string UOM_CODE { get; set; }
        [XmlElement("ITM_TEXT")]
        public string ITM_TEXT { get; set; }
        [XmlElement("POD_RATE")]
        public string POD_RATE { get; set; }
        [XmlElement("POD_AMOUNT")]
        public string POD_AMOUNT { get; set; }
        [XmlElement("POD_TAX_PERC")]
        public string POD_TAX_PERC { get; set; }
        [XmlElement("POD_TAX")]
        public string POD_TAX { get; set; }
        [XmlElement("POD_DISC_PERC")]
        public string POD_DISC_PERC { get; set; }
        [XmlElement("POD_DISC_AMT")]
        public string POD_DISC_AMT { get; set; }
        [XmlElement("POD_AMT_VALUE")]
        public string POD_AMT_VALUE { get; set; }
        [XmlElement("POD_REMARKS")]
        public string POD_REMARKS { get; set; }
        [XmlElement("POD_REQD_DATE")]
        public string POD_REQD_DATE { get; set; }
        [XmlElement("POD_DEPT")]
        public int POD_DEPT { get; set; }
        [XmlElement("POD_CONV_FACT")]
        public string POD_CONV_FACT { get; set; }
        [XmlElement("TaxDetails")]
        public List<PTaxDetails> TaxDetails { get; set; }

    }

    [Serializable]
    public class PTaxDetails
    {
        [XmlElement("POT_PK")]
        public int POT_PK { get; set; }
        [XmlElement("POT_SL_NO")]
        public int POT_SL_NO { get; set; }
        [XmlElement("POT_TAX")]
        public int POT_TAX { get; set; }
        [XmlElement("POT_PO")]
        public int POT_PO { get; set; }
        [XmlElement("POT_TYPE")]
        public int POT_TYPE { get; set; }
        [XmlElement("POT_NAME")]
        public string POT_NAME { get; set; }
        [XmlElement("POT_TAX_CATEGORY")]
        public int POT_TAX_CATEGORY { get; set; }
        [XmlElement("POT_TAX_FORMULA")]
        public string POT_TAX_FORMULA { get; set; }
        [XmlElement("POT_TAX_CATEGORY_TEXT")]
        public string POT_TAX_CATEGORY_TEXT { get; set; }
        [XmlElement("POT_TAX_TEXT")]
        public string POT_TAX_TEXT { get; set; }
        [XmlElement("POT_TAX_AMT")]
        public double POT_TAX_AMT { get; set; }
        [XmlElement("POT_PO_DTL")]
        public string POT_PO_DTL { get; set; }
        [XmlElement("IsHeader")]
        public int IsHeader { get; set; }
        [XmlElement("ItemPK")]
        public int ItemPK { get; set; }

        public bool Applied { get; set; }
        public bool Delete { get; set; }
    }

    [Serializable]
    public class PTaxHeader
    {
        [XmlElement("PTH_PK")]
        public string PTH_PK { get; set; }
        [XmlElement("PTH_TAX")]
        public string PTH_TAX { get; set; }
        [XmlElement("PTH_TAX_AMT")]
        public double PTH_TAX_AMT { get; set; }
        [XmlElement("PTH_NAME")]
        public string PTH_NAME { get; set; }
        [XmlElement("PTH_TYPE")]
        public string PTH_TYPE { get; set; }
        [XmlElement("PTH_TAX_CATEGORY")]
        public int PTH_TAX_CATEGORY { get; set; }
    }


    [Serializable]
    [XmlRoot("root")]
    public class POHeader
    {
        [XmlElement("POH_PK")]
        public int POH_PK { get; set; }
        [XmlElement("POH_COMPANY")]
        public int POH_COMPANY { get; set; }
        [XmlElement("POH_NO")]
        public string POH_NO { get; set; }
        [XmlElement("POH_VERSION")]
        public string POH_VERSION { get; set; }
        [XmlElement("POH_DATE")]
        public string POH_DATE { get; set; }
        [XmlElement("POH_TYPE")]
        public string POH_TYPE { get; set; }
        [XmlElement("POH_TYPE_TEXT")]
        public string POH_TYPE_TEXT { get; set; }
        [XmlElement("POH_ITEM_TYPE")]
        public string POH_ITEM_TYPE { get; set; }
        [XmlElement("POH_ITEM_TYPE_TEXT")]
        public string POH_ITEM_TYPE_TEXT { get; set; }
        [XmlElement("POH_CONTRACT_REF_NO")]
        public string POH_CONTRACT_REF_NO { get; set; }
        [XmlElement("POH_CURRENCY")]
        public int POH_CURRENCY { get; set; }
        [XmlElement("POH_CURRENCY_TEXT")]
        public string POH_CURRENCY_TEXT { get; set; }
        [XmlElement("POH_EXCHG_RATE")]
        public string POH_EXCHG_RATE { get; set; }
        [XmlElement("POH_CURRENCY_BC")]
        public int POH_CURRENCY_BC { get; set; }
        [XmlElement("POH_VENDOR")]
        public int POH_VENDOR { get; set; }
        [XmlElement("POH_SHIPPING")]
        public string POH_SHIPPING { get; set; }
        [XmlElement("POH_BILLING")]
        public string POH_BILLING { get; set; }
        [XmlElement("POH_SUB_TOTAL")]
        public string POH_SUB_TOTAL { get; set; }
        [XmlElement("POH_DISC_AMT")]
        public string POH_DISC_AMT { get; set; }
        [XmlElement("POH_SHIP_CHARGE")]
        public string POH_SHIP_CHARGE { get; set; }
        [XmlElement("POH_ADD_TAX_AMT")]
        public string POH_ADD_TAX_AMT { get; set; }
        [XmlElement("POH_TOTAL_VALUE")]
        public string POH_TOTAL_VALUE { get; set; }
        [XmlElement("POH_PRICE_ADJUST")]
        public string POH_PRICE_ADJUST { get; set; }
        //[XmlElement("POH_CURRENCY_BC")]
        //public int POH_CURRENCY_BC { get; set; }
        //[XmlElement("POH_EXCHG_RATE")]
        //public string POH_EXCHG_RATE { get; set; }
        [XmlElement("POH_TOTAL_VALUE_BC")]
        public string POH_TOTAL_VALUE_BC { get; set; }
        [XmlElement("POH_REMARKS")]
        public string POH_REMARKS { get; set; }
        [XmlElement("POH_COMMENTS")]
        public string POH_COMMENTS { get; set; }
        [XmlElement("VENDOR_TERMS")]
        public string VENDOR_TERMS { get; set; }
        [XmlElement("POH_TERMS")]
        public string POH_TERMS { get; set; }
        [XmlElement("TERMS")]
        public string TERMS { get; set; }
        [XmlElement("POH_STATUS")]
        public string POH_STATUS { get; set; }
        [XmlElement("POH_ACTIVE")]
        public string POH_ACTIVE { get; set; }
        [XmlElement("POH_DEPT")]
        public int POH_DEPT { get; set; }
        [XmlElement("BizUnitPk")]
        public string BizUnitPk { get; set; }
        [XmlElement("UserPk")]
        public string UserPk { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("POH_CRTD_BY")]
        public string POH_CRTD_BY { get; set; }
        [XmlElement("VEN_NAME")]
        public string VEN_NAME { get; set; }
        [XmlElement("POH_DEL_STATUS")]
        public string POH_DEL_STATUS { get; set; }

        [XmlElement("POH_FROM_PORT")]
        public string POH_FROM_PORT { get; set; }
        [XmlElement("POH_FROM_PORT_TEXT")]
        public string POH_FROM_PORT_TEXT { get; set; }
        [XmlElement("POH_TO_PORT")]
        public string POH_TO_PORT { get; set; }
        [XmlElement("POH_TO_PORT_TEXT")]
        public string POH_TO_PORT_TEXT { get; set; }

        //[XmlElement("POH_SHIP_CHARGE")]
        //public string POH_SHIP_CHARGE { get; set; }
        //[XmlElement("POH_VENDOR_TERMS")]
        //public string POH_VENDOR_TERMS { get; set; }
        //[XmlElement("POH_SUBMITTED_BY")]
        //public string POH_SUBMITTED_BY { get; set; }
        //[XmlElement("POH_SUBMITTED_DATE")]
        //public string POH_SUBMITTED_DATE { get; set; }
        //[XmlElement("POH_APPROVED_BY")]
        //public string POH_APPROVED_BY { get; set; }
        //[XmlElement("POH_APPROVED_DATE")]
        //public string POH_APPROVED_DATE { get; set; }
        //[XmlElement("APT_CODE")]
        //public string APT_CODE { get; set; }
        //[XmlElement("AST_DOC_MODE")]
        //public string AST_DOC_MODE { get; set; }
        //[XmlElement("WKF_FLAG")]
        //public int WKF_FLAG { get; set; }
        //[XmlElement("POH_IS_SERVICE")]
        //public int POH_IS_SERVICE { get; set; }

        [XmlElement("PurchaseOrderList")]
        public List<PurchaseOrderList> PurchaseOrderList { get; set; }

    }

    [Serializable]
    [XmlRoot("PurchaseOrderList")]
    public class PurchaseOrderList
    {
        [XmlElement("PODetails")]
        public List<PurchaseOrderDetails> PODetails { get; set; }
        [XmlElement("TaxHeader")]
        public List<PTaxDetails> TaxDetails { get; set; }
    }


}
