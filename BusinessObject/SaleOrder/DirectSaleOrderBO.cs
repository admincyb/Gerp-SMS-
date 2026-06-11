using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;

namespace BusinessObject.SaleOrder
{
    [Serializable]
    [XmlRoot("Root")]
    public class DirectSaleOrderBO : WorkflowBO
    {
        [XmlElement("SOH_PK")]
        public int SOH_PK { get; set; }

        [XmlElement("SOH_NO")]
        public string SOH_NO { get; set; }
        [XmlElement("SOH_DATE")]
        public DateTime SOH_DATE { get; set; }
        [XmlElement("SOH_TYPE")]
        public int SOH_TYPE { get; set; }
        [XmlElement("SOH_TYPE_TEXT")]
        public string SOH_TYPE_TEXT { get; set; }
        [XmlElement("SOH_STATUS")]
        public short SOH_STATUS { get; set; }

        [XmlElement("SOH_CURRENCY")]
        public int SOH_CURRENCY { get; set; }
        [XmlElement("SOH_CURRENCY_TEXT")]
        public string SOH_CURRENCY_TEXT { get; set; }
        [XmlElement("SOH_CURRENCY_RATE")]
        public double SOH_CURRENCY_RATE { get; set; }
        [XmlElement("SOH_CURRENCY_BC")]
        public int SOH_CURRENCY_BC { get; set; }

        [XmlElement("SOH_CUSTOMER")]
        public int SOH_CUSTOMER { get; set; }
        [XmlElement("SOH_CUSTOMER_TEXT")]
        public string SOH_CUSTOMER_TEXT { get; set; }
        [XmlElement("SOH_CUSTOMER_NAME")]
        public string SOH_CUSTOMER_NAME { get; set; }
        [XmlElement("SOH_CUSTOMER_ADDRESS")]
        public string SOH_CUSTOMER_ADDRESS { get; set; }
        [XmlElement("SOH_CUSTOMER_COUNTRY")]
        public int SOH_CUSTOMER_COUNTRY { get; set; }
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
        [XmlElement("CUS_SPECIAL_CAT_TEXT")]
        public string CUS_SPECIAL_CAT_TEXT { get; set; }

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

        [XmlElement("SOH_DELIVERY_DATE")]
        public DateTime SOH_DELIVERY_DATE { get; set; }
        [XmlElement("SOH_REFERENCE")]
        public string SOH_REFERENCE { get; set; }

        [XmlElement("SOH_ACTIVE")]
        public byte SOH_ACTIVE { get; set; }
        [XmlElement("SOH_DEPT")]
        public short SOH_DEPT { get; set; }
        [XmlElement("SOH_BIZUNIT")]
        public short SOH_BIZUNIT { get; set; }
        [XmlElement("SOH_COMPANY")]
        public int SOH_COMPANY { get; set; }
        [XmlElement("SOH_CRTD_BY")]
        public string SOH_CRTD_BY { get; set; }
        [XmlElement("SOH_CRTD_DT")]
        public string SOH_CRTD_DT { get; set; }
        [XmlElement("SOH_MOD_BY")]
        public string SOH_MOD_BY { get; set; }
        [XmlElement("SOH_MOD_DT")]
        public string SOH_MOD_DT { get; set; }
        [XmlElement("SOH_DEL_STATUS")]
        public byte SOH_DEL_STATUS { get; set; }
        [XmlElement("SOH_SUB_TYPE")]
        public int SOH_SUB_TYPE { get; set; }

        [XmlElement("SOH_IS_AMEND")]
        public byte SOH_IS_AMEND { get; set; }
        [XmlElement("SOH_AMEND_DATE")]
        public string SOH_AMEND_DATE { get; set; }
        [XmlElement("SOH_VERSION")]
        public int SOH_VERSION { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
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

        [XmlElement("SOH_TRX_TYPE")]
        public int SOH_TRX_TYPE { get; set; }

        [XmlElement("OrderDetail")]
        public List<DirectSaleOrderDetailsBO> DirectsaleOrderDetails { get; set; } //SELECT * FROM   SAL_ORDER_DTL

        [XmlElement("TaxHdr")]
        public List<DirectSaleOrderTaxHdr> TaxHdrDtl { get; set; } //SELECT * FROM   SAL_ORDER_TAX_DTL

        [XmlElement("FileList")]
        public List<DirectSaleOrderUploads> FileList { get; set; } //SELECT * FROM   ADM_DOC_ATTACH
    }

    [Serializable]
    public class DirectSaleOrderDetailsBO
    {
        [XmlElement("SOD_PK")]
        public int SOD_PK { get; set; }
        [XmlElement("SOD_SL_NO")]
        public int SOD_SL_NO { get; set; }

        //ITEM
        [XmlElement("SOD_ITEM")]
        public int SOD_ITEM { get; set; }
        [XmlElement("SOD_ITEM_TEXT")]
        public string SOD_ITEM_TEXT { get; set; }
        [XmlElement("SOD_ITEM_CATEGORY")]
        public int SOD_ITEM_CATEGORY { get; set; }
        [XmlElement("SOD_ITEM_CATEGORY_TEXT")]
        public string SOD_ITEM_CATEGORY_TEXT { get; set; }
        [XmlElement("SOD_PACK_SPEC")]
        public int SOD_PACK_SPEC { get; set; }
        [XmlElement("SOD_PACK_SPEC_NAME")]
        public string SOD_PACK_SPEC_NAME { get; set; }
        [XmlElement("SOD_ITEM_SUB_TYPE")]
        public int SOD_ITEM_SUB_TYPE { get; set; }
        
        //QUANTITY
        [XmlElement("SOD_QTY")]
        public double SOD_QTY { get; set; }

        //AMOUNT
        [XmlElement("SOD_UOM")]
        public int SOD_UOM { get; set; }
        [XmlElement("SOD_UOM_TEXT")]
        public string SOD_UOM_TEXT { get; set; }
        [XmlElement("SOD_RATE")]
        public double SOD_RATE { get; set; }
        [XmlElement("SOD_AMOUNT")]
        public double SOD_AMOUNT { get; set; }
        [XmlElement("SOD_DISCOUNT")]
        public double SOD_DISCOUNT { get; set; }
        [XmlElement("SOD_DISC_PERC")]
        public double SOD_DISC_PERC { get; set; }
        [XmlElement("SOD_TAX")]
        public double SOD_TAX { get; set; }
        [XmlElement("SOD_NET_AMOUNT")]
        public double SOD_NET_AMOUNT { get; set; }

        //
        [XmlElement("SOD_REMARKS")]
        public string SOD_REMARKS { get; set; }
        [XmlElement("SOD_REMARKS2")]
        public string SOD_REMARKS2 { get; set; }
        [XmlElement("SOD_REQUIRED_BY")]
        public string SOD_REQUIRED_BY { get; set; }
        //

        [XmlElement("TaxDtl")]
        public List<DirectSaleOrderTaxHdr> TaxDtl { get; set; }
    }

    [Serializable]
    public class DirectSaleOrderTaxHdr
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
    public class DirectSaleOrderUploads
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

    public class DirectFileDetails
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
}
