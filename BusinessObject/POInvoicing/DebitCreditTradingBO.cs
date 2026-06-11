using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.POInvoicing
{
    [Serializable]
    [XmlRoot("Root")]
    public class DebitCreditTradingBO : WorkflowBO
    {
        #region Variables
        [XmlElement("CDH_PK")]
        public int CDH_PK { get; set; }
        [XmlElement("CDH_DATE")]
        public string CDH_DATE { get; set; }
        [XmlElement("CDH_NO")]
        public string CDH_NO { get; set; }
        [XmlElement("CDH_REF_NO")]
        public string CDH_REF_NO { get; set; }
        [XmlElement("CDH_REF_DATE")]
        public string CDH_REF_DATE { get; set; }
        [XmlElement("CDH_TYPE")]
        public byte CDH_TYPE { get; set; }    
        [XmlElement("CDH_VENDOR")]
        public string CDH_VENDOR { get; set; }
        [XmlElement("CDH_CUSTOMER")]
        public string CDH_CUSTOMER { get; set; }        
        [XmlElement("CDH_VENDOR_TEXT")]
        public string CDH_VENDOR_TEXT { get; set; }     
        [XmlElement("CDH_VND_CUS_ACCOUNT")]
        public int CDH_VND_CUS_ACCOUNT { get; set; }
        [XmlElement("CDH_CURRENCY")]
        public int CDH_CURRENCY { get; set; }
        [XmlElement("CDH_CURRENCY_TEXT")]
        public string CDH_CURRENCY_TEXT { get; set; }
        [XmlElement("CDH_AMOUNT_TC")]
        public decimal CDH_AMOUNT_TC { get; set; }
        [XmlElement("CDH_TAX_AMOUNT")]
        public decimal CDH_TAX_AMOUNT { get; set; }
        [XmlElement("CDH_BASE_CURR")]
        public int CDH_BASE_CURR { get; set; }
        [XmlElement("CDH_BASE_CURR_TEXT")]
        public string CDH_BASE_CURR_TEXT { get; set; }        
        [XmlElement("CDH_EXCHG_RATE")]
        public double CDH_EXCHG_RATE { get; set; }
        [XmlElement("CDH_AMOUNT_BC")]
        public decimal CDH_AMOUNT_BC { get; set; }
        [XmlElement("CDH_REMARKS")]
        public string CDH_REMARKS { get; set; }
        [XmlElement("CDH_HAS_JRNL_ENTRY")]
        public bool CDH_HAS_JRNL_ENTRY { get; set; }
        [XmlElement("CDH_STATUS")]
        public byte CDH_STATUS { get; set; }
        [XmlElement("CDH_ACTIVE")]
        public byte CDH_ACTIVE { get; set; }
        [XmlElement("CDH_MOD_DT")]
        public DateTime CDH_MOD_DT { get; set; }   
        [XmlElement("CDH_DEPT")]
        public int CDH_DEPT { get; set; }
        [XmlElement("CDH_BIZUNIT")]
        public int CDH_BIZUNIT { get; set; }
        [XmlElement("CDH_COMPANY")]
        public int CDH_COMPANY { get; set; }
        [XmlElement("CDH_SHIP_CHARGE")]
        public double CDH_SHIP_CHARGE { get; set; }
        [XmlElement("CDH_OTHER_CHARGE")]
        public double CDH_OTHER_CHARGE { get; set; }
        [XmlElement("CDH_IS_DELETED")]
        public bool CDH_IS_DELETED { get; set; }
        [XmlElement("CDH_IMP_DECL_NO")]
        public string CDH_IMP_DECL_NO { get; set; }
        [XmlElement("CDH_VERSION")]
        public double CDH_VERSION { get; set; }
        [XmlElement("CDH_IS_AFFECT_STK")]
        public byte CDH_IS_AFFECT_STK { get; set; }
        [XmlElement("CDH_REMARKS2")]
        public string CDH_REMARKS2 { get; set; }

        [XmlElement("CDH_IVH_PK")]
        public int CDH_IVH_PK { get; set; }
        [XmlElement("CDH_IVH_CATEGORY")]
        public byte CDH_IVH_CATEGORY { get; set; }
        [XmlElement("CDH_IVH_GROUP")]
        public byte CDH_IVH_GROUP { get; set; }
        [XmlElement("CDH_IVH_TYPE")]
        public int CDH_IVH_TYPE { get; set; }
        [XmlElement("CDH_TYPE_TEXT")]
        public string CDH_TYPE_TEXT { get; set; }    
        [XmlElement("CDH_CMP_DISPLAY_CODE")]
        public string CDH_CMP_DISPLAY_CODE { get; set; }
        [XmlElement("CDH_CMP_LINE_COLOUR")]
        public string CDH_CMP_LINE_COLOUR { get; set; }
        [XmlElement("CDH_INVOICE_DATE")]
        public string CDH_INVOICE_DATE { get; set; }
        [XmlElement("CDH_AMOUNT")]
        public decimal CDH_AMOUNT { get; set; }
        [XmlElement("CDH_BALANCE_AMOUNT")]
        public decimal CDH_BALANCE_AMOUNT { get; set; }
        [XmlElement("CDH_IS_EMR_EXISTS")]
        public int CDH_IS_EMR_EXISTS { get; set; }
        
        

        [XmlElement("ATL_ACTION")]
        public byte? ATL_ACTION { get; set; }
        [XmlElement("ATL_APP_TYPE")]
        public string ATL_APP_TYPE { get; set; }
        [XmlElement("USER_PK")]
        public short USER_PK { get; set; }

        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }
        [XmlElement("AST_VALUE")]
        public string AST_VALUE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }      

        #endregion

        [XmlElement("InvoiceDetail")]
        public List<DebitCreditNoteInvoiceMappingDetails> InvoiceDetail { get; set; }        
        [XmlElement("FileList")]
        public List<DebitCreditNoteUploads> FileList { get; set; }
    }

    [Serializable]
    public class DebitCreditNoteInvoiceMappingDetails
    {
        #region FIN_CRDR_NOTE_MPG table fields
        [XmlElement]
        public long CDM_PK { get; set; }
        [XmlElement]
        public string CDM_CRDR_NOTE_HDR { get; set; }
        [XmlElement]
        public int CDM_INVOICE_VND_HDR { get; set; }
        [XmlElement]
        public decimal CDM_AMOUNT { get; set; }
        [XmlElement]
        public decimal CDM_TAX_AMOUNT { get; set; }
        [XmlElement]
        public byte CDM_ACTIVE { get; set; }
        [XmlElement]
        public decimal CDM_SHIP_CHARGE { get; set; }
        [XmlElement]
        public decimal CDM_OTHER_CHARGE { get; set; }
        #endregion

        #region Invoice fields(FIN_INVOICE_VND_HDR)
        [XmlElement]
        public string IVH_NO { get; set; }
        [XmlElement]
        public string IVH_DATE { get; set; }
        [XmlElement]
        public int IVH_VENDOR { get; set; }
        [XmlElement]
        public string IVH_VENDOR_TEXT { get; set; }
        [XmlElement]
        public byte IVH_IS_LINE_ITEM_TAX { get; set; }
        [XmlElement]
        public decimal IVH_AMOUNT { get; set; }
        [XmlElement]
        public decimal IVH_TAX_AMOUNT { get; set; }
        [XmlElement]
        public decimal IVH_DISC_AMOUNT { get; set; }
        [XmlElement]
        public decimal IVH_SHIP_CHARGE { get; set; }
        [XmlElement]
        public decimal IVH_AMOUNT_PAID_TC { get; set; }
        [XmlElement]
        public decimal IVH_ADJ_AMT { get; set; }
        [XmlElement]
        public decimal IVH_BAL_TO_PAY { get; set; }
        [XmlElement]
        public decimal IVH_TAX_PERC { get; set; }
        [XmlElement]
        public byte IVH_HAS_JRNL_ENTRY { get; set; }
        [XmlElement]
        public byte IVH_GROUP { get; set; }
        [XmlElement]
        public byte IVH_CATEGORY { get; set; }
        [XmlElement]
        public decimal IVH_INVOICE_AMT { get; set; }
        [XmlElement]
        public decimal IVH_AMOUNT_TC { get; set; }
        [XmlElement]
        public string IVH_VENDOR_INV_NO { get; set; }
        [XmlElement]
        public DateTime IVH_DATE_RECEIVED { get; set; }
        #endregion

        [XmlElement("CDM_SL_NO")]
        public int CDM_SL_NO { get; set; }
        [XmlElement("ItemDetail")]
        public List<DebitCreditNoteDetails> ItemDetail { get; set; }
    }

    [Serializable]
    public class DebitCreditNoteDetails
    {
        [XmlElement("CDS_PK")]
        public int CDS_PK { get; set; }
        [XmlElement("CDS_CRDR_NOTE_HDR")]
        public int CDS_CRDR_NOTE_HDR { get; set; }
        [XmlElement("CDS_CRDR_NOTE_MPG")]
        public long CDS_CRDR_NOTE_MPG { get; set; }
        [XmlElement("CDS_INVOICE_VND_DTL")]
        public int CDS_INVOICE_VND_DTL { get; set; }
        [XmlElement("CDS_PO_DTL")]
        public int CDS_PO_DTL { get; set; }
        [XmlElement("CDS_PO_NO")]
        public string CDS_PO_NO { get; set; }        
        [XmlElement("CDS_QTY")]
        public double CDS_QTY { get; set; }
        [XmlElement("CDS_UOM")]
        public int CDS_UOM { get; set; }
        [XmlElement("CDS_UOM_TEXT")]
        public string CDS_UOM_TEXT { get; set; }        
        [XmlElement("CDS_RATE")]
        public double CDS_RATE { get; set; }
        [XmlElement("CDS_AMOUNT")]
        public decimal CDS_AMOUNT { get; set; }
        [XmlElement("CDS_DISCOUNT")]
        public decimal CDS_DISCOUNT { get; set; }
        [XmlElement("CDS_TAX")]
        public decimal CDS_TAX { get; set; }
        [XmlElement("CDS_NET_AMOUNT")]
        public decimal CDS_NET_AMOUNT { get; set; }
        [XmlElement("CDS_REMARKS")]
        public string CDS_REMARKS { get; set; }
        [XmlElement("CDS_ACTIVE")]
        public byte CDS_ACTIVE { get; set; }
        [XmlElement("CDS_IS_AFFECT_STK")]
        public byte CDS_IS_AFFECT_STK { get; set; }

        [XmlElement("CDS_ITEM_TEXT")]
        public string CDS_ITEM_TEXT { get; set; }
        [XmlElement("CDS_VID_QTY_INVOICED")]
        public double CDS_VID_QTY_INVOICED { get; set; }
        [XmlElement("CDS_VID_RATE")]
        public double CDS_VID_RATE { get; set; }
        [XmlElement("CDS_VID_AMOUNT")]
        public decimal CDS_VID_AMOUNT { get; set; }
        [XmlElement("CDS_VID_DISCOUNT")]
        public decimal CDS_VID_DISCOUNT { get; set; }
        [XmlElement("CDS_VID_TAX")]
        public decimal CDS_VID_TAX { get; set; }
        [XmlElement("CDS_VID_NET_AMOUNT")]
        public decimal CDS_VID_NET_AMOUNT { get; set; }
        [XmlElement]
        public decimal CDS_TAX_PERC { get; set; }

        [XmlElement("CDS_SL_NO")]
        public int CDS_SL_NO { get; set; }
        [XmlElement("CDS_CDM_SL_NO")]
        public int CDS_CDM_SL_NO { get; set; }        

        [XmlElement("InvoiceTaxDetail")]
        public List<DebitCreditNoteTaxDtl> TaxDetail { get; set; }
    }
    [Serializable]
    public class DebitCreditNoteTaxDtl
    {
        [XmlElement("NTD_PK")]
        public int NTD_PK { get; set; }       
        [XmlElement("NTD_CRDR_MPG")]
        public int NTD_CRDR_MPG { get; set; }
        [XmlElement("NTD_CRDR_DTL")]
        public int NTD_CRDR_DTL { get; set; }
        [XmlElement("NTD_TAX")]
        public int NTD_TAX { get; set; }
        [XmlElement("NTD_TAX_TEXT")]
        public string NTD_TAX_TEXT { get; set; }        
        [XmlElement("NTD_AMOUNT")]
        public decimal NTD_AMOUNT { get; set; }

        [XmlElement("NTD_INVOICE_VND_HDR")]
        public int NTD_INVOICE_VND_HDR { get; set; }
        [XmlElement("NTD_TAX_CATEGORY_TEXT")]
        public string NTD_TAX_CATEGORY_TEXT { get; set; }
        [XmlElement("NTD_TAX_CATEGORY")]
        public int NTD_TAX_CATEGORY { get; set; }
        [XmlElement("NTD_TAX_CODE")]
        public string VTL_TAX_CODE { get; set; }
        [XmlElement("NTD_VTL_PK")]
        public int NTD_VTL_PK { get; set; }
        [XmlElement("NTD_VTL_INVOICE_DTL")]
        public int NTD_VTL_INVOICE_DTL { get; set; }    
        [XmlElement("NTD_VTL_TAX")]
        public int NTD_VTL_TAX { get; set; }
        [XmlElement("NTD_VTL_TAX_AMT")]
        public decimal NTD_VTL_TAX_AMT { get; set; }

        //[XmlElement("NTD_SL_NO")]
        //public int NTD_SL_NO { get; set; }

        [XmlElement("NTD_CDS_SL_NO")]
        public int NTD_CDS_SL_NO { get; set; }
    }
   
    [Serializable]
    public class DebitCreditNoteUploads
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
    [XmlRoot("ROOT")]
    public class DirectInvHeaderBO
    {
        [XmlElement("INV")]
        public List<DirectInvHeaderListBO> INVList { get; set; }
    }

    [Serializable]
    public class DirectInvHeaderListBO
    {
        [XmlElement("IVH_PK")]
        public int IVH_PK { get; set; }
        [XmlElement("CDH_PK")]
        public int CDH_PK { get; set; }
        public int IVH_VENDOR { get; set; }
        public int IVH_TYPE { get; set; }
        public int IVH_CURRENCY { get; set; }
        public int IVH_GROUP { get; set; }
    }
}
