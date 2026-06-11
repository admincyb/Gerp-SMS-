using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessObject.WorkOrder
{
    [Serializable]
    [XmlRoot("Root")]
    public class WorkOrderBO : WorkflowBO
    {
        [XmlElement("WIH_PK")]
        public int WorkOrderPK { get; set; }

        [XmlElement("WIH_NO")]
        public string WorkOrderNo { get; set; }
        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public string AST_DOC_MODE { get; set; }
        

        [XmlElement("WIH_DATE")]
        public DateTime WorkOrderDate { get; set; }

        [XmlElement("WIH_VENDOR")]
        public int VendorPK { get; set; }

        [XmlElement("WIH_VENDOR_TEXT")]
        public string Vendor { get; set; }

        [XmlElement("WIH_REF_NO")]
        public string WorkOrderRefNo { get; set; }

        [XmlElement("WIH_ITEM_TYPE_PK")]
        public int ItemTypePK { get; set; }

        [XmlElement("WIH_DEPT_TO")]
        public int DepartmentTo { get; set; }

        [XmlElement("WIH_DEPT")]
        public int DepartmentPK { get; set; }
        
        //[XmlElement("WIH_LOCATION")]
        //public int LocationPK { get; set; }
        
        [XmlElement("WIH_BIZUNIT")]
        public int BizUnitPK { get; set; }

        [XmlElement("WIH_CRTD_BY")]
        public int CreatedUserPK { get; set; }

        [XmlElement("WIH_CRTD_DT")]
        public DateTime CreatedDate { get; set; }

        [XmlElement("WIH_MOD_BY")]
        public int ModifiedUserPK { get; set; }

        [XmlElement("WIH_MOD_DT")]
        public DateTime ModifiedDate { get; set; }

        [XmlElement("WIH_DEL_STATUS")]
        public bool DeleteStatus { get; set; }

        [XmlElement("WIH_STATUS")]
        public int Status { get; set; }

        [XmlElement("WIH_TOTAL_QTY")]
        public double TotalQuantity { get; set; }

        [XmlElement("WIH_TOTAL_AMOUNT")]
        public double TotalAmount { get; set; }

        [XmlElement("WIH_TOTAL_DISCOUNT")]
        public double TotalDiscount { get; set; }

        [XmlElement("WIH_TOTAL_SHIP_CHARGE")]
        public double TotalShipCharge { get; set; }

        [XmlElement("WIH_TOTAL_TAX")]
        public double TotalTax { get; set; }

        [XmlElement("WIH_TOTAL_ADJUST")]
        public double TotalAdjust { get; set; }

        [XmlElement("WIH_PRICE_ADJUST")]
        public double PriceAdjust { get; set; }

        [XmlElement("WIH_NET_AMOUNT")]
        public double NetAmount { get; set; }

        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("USER_PK")]
        public short USER_PK { get; set; }

        [XmlElement("WIH_COMPANY")]
        public int CompanyPK { get; set; }

        [XmlElement("WIH_CUSTOMER")]
        public int CustomerPK { get; set; }

        [XmlElement("WIH_CUS_NAME")]
        public string Customer { get; set; }

        [XmlElement("WIH_BRAND")]
        public int BrandPK { get; set; }

        [XmlElement("WIH_BRAND_NAME")]
        public string Brand { get; set; }

        [XmlElement("WIH_BRAND_CODE")]
        public string BrandCode { get; set; }
        
        [XmlElement("WIH_VENDOR_TERMS_TEXT")]
        public string VendorTerms { get; set; }

        [XmlElement("WIH_TERMS_TEXT")]
		public string GeneralTerms { get; set; }

        [XmlElement("WIH_VENDOR_TERMS")]
        public int VendorTermsPK { get; set; }

        [XmlElement("WIH_TERMS")]
        public int GeneralTermsPK { get; set; }

        [XmlElement("WIH_COMMENTS")]
        public string Comments { get; set; }

        [XmlElement("WIH_DESC")]
        public string Description { get; set; }

        [XmlElement("WIH_IS_AMEND")]
        public int IsAmend { get; set; }

        [XmlElement("WIH_AMEND_DATE")]
        public DateTime? AmendDate { get; set; }

        [XmlElement("WIH_HAS_INVOICE")]
        public int HasInvoice { get; set; }

        [XmlElement("WIH_EXCHG_RATE")]
        public float ExchangeRate { get; set; }

        [XmlElement("WIH_CURRENCY_BC")]
        public int CurrencyBC { get; set; }

        [XmlElement("WIH_NET_AMOUNT_BC")]
        public double NetAmountBC { get; set; }

        [XmlElement("WIH_REF_DATE")]
        public DateTime? RefDate { get; set; }

        [XmlElement("WIH_DELY_DATE")]
        public DateTime? DelyDate { get; set; }

        [XmlElement("WIH_REMARKS")]
        public string Remarks { get; set; }

        [XmlElement("WIH_ACTIVE")]
        public int Active { get; set; }

        [XmlElement("WoDetails")]
        public List<WorkOrderDetails> WorkOrderDetailList { get; set; }

        [XmlElement("TaxHeader")]
        public List<WorkOrderTaxHdr> TaxHdr { get; set; }

        [XmlElement("FileList")]
        public List<WorkOrderUploads> FileList { get; set; }
    }

    [Serializable]
    public class WorkOrderDetails
    {
        [XmlElement("WID_PK")]
        public int WorkOrderDetailPK { get; set; }

        [XmlElement("WIH_NO")]
        public string WorkOrderNo { get; set; }

        [XmlElement("WID_OPERTAION_PK")]
        public int OperationPK { get; set; }

        [XmlElement("WID_OPERTAION")]
        public string Operation { get; set; }

        [XmlElement("WID_WIH")]
        public int WorkOrderPK { get; set; }

        [XmlElement("WID_ITEM_TYPE_PK")]
        public int ItemTypePK { get; set; }

        [XmlElement("WID_ITEM_TYPE")]
        public string ItemType { get; set; }

        [XmlElement("WID_ITEM_PK")]
        public int ItemPK { get; set; }

        [XmlElement("WID_ITEM")]
        public string Item { get; set; }

        [XmlElement("WID_UOM_PK")]
        public int UOMPK { get; set; }

        [XmlElement("WID_UOM")]
        public string UOM { get; set; }

        [XmlElement("WID_RATE")]
        public double Rate { get; set; }

        [XmlElement("WID_QTY")]
        public double Quantity { get; set; }

        [XmlElement("WID_AMT")]
        public double Amount { get; set; }

        [XmlElement("WID_REQ_DT")]
        public DateTime RequiredDate { get; set; }

        [XmlElement("WID_BIZUNIT")]
        public int BizUnitPK { get; set; }

        [XmlElement("WID_REMARKS")]
        public string Remarks { get; set; }

        [XmlElement("WID_CRTD_BY")]
        public int CreatedUserPK { get; set; }

        [XmlElement("WID_CRTD_DT")]
        public DateTime CreatedDate { get; set; }

        [XmlElement("WID_MOD_BY")]
        public int ModifiedUserPK { get; set; }

        [XmlElement("WID_MOD_DT")]
        public DateTime ModifiedDate { get; set; }

        [XmlElement("BomDetails")]
        public List<BillOfMaterials> BillOfMaterialList { get; set; }

        [XmlElement("WID_SL_NO")]
        public int SlNo { get; set; }

        [XmlElement("Tax")]
        public List<WorkOrderTaxHdr> Tax { get; set; }
    }

    [Serializable]
    public class WorkOrderTaxHdr
    {
        [XmlElement("WTH_PK")]
        public int TaxHeaderPK { get; set; }

        [XmlElement("WTH_WIH")]
        public int WorkOrderPK { get; set; }

        [XmlElement("WTH_TYPE")]
        public int TaxTypePK { get; set; }

        [XmlElement("WTH_TAX")]
        public int TaxPK { get; set; }

        [XmlElement("WTH_TAX_CATEGORY")]
        public int TaxCategoryPK { get; set; }

        [XmlElement("WTH_NAME")]
        public string TaxName { get; set; }

        [XmlElement("WTH_TAX_AMT")]
        public double TaxAmount { get; set; }

        [XmlElement("WTH_DISC_PERC")]
        public double DiscountPercentage { get; set; }

        [XmlElement("WTH_HAS_SUB_TOTAL")]
        public int HasSubTotal { get; set; }

        [XmlElement("WTH_HAS_DISCOUNT")]
        public int HasDiscount { get; set; }

        [XmlElement("WTH_HAS_OTHER_CHARGE")]
        public int HasOtherCharge { get; set; }

        [XmlElement("WTH_SL_NO")]
        public int SlNo { get; set; }

        [XmlElement("WTH_TAX_FORMULA")]
        public string TaxFormula { get; set; }
    }

    [Serializable]
    public class WorkOrderTaxDetails
    {
        [XmlElement("WTD_PK")]
        public int TaxDetailsPK { get; set; }

        [XmlElement("WTD_WID")]
        public int WorkOrderDetailPK { get; set; }

        [XmlElement("WTD_TYPE")]
        public int TaxTypePK { get; set; }

        [XmlElement("WTD_TAX")]
        public int TaxPK { get; set; }

        [XmlElement("WTD_TAX_CATEGORY")]
        public int TaxCategoryPK { get; set; }

        [XmlElement("WTD_NAME")]
        public string TaxName { get; set; }

        [XmlElement("WTD_TAX_AMT")]
        public double TaxAmount { get; set; }

        [XmlElement("WTD_SL_NO")]
        public int SlNo { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class BillOfMaterialRoot
    {
        [XmlElement("BOMDetails")]
        public List<BillOfMaterials> BillOfMaterialList { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class WOIssues
    {
        [XmlElement("IssueDetailList")]
        public List<IssueDetails> IssueDetailList { get; set; }
    }

    [Serializable]
    public class BillOfMaterials
    {
        [XmlElement("WIB_PK")]
        public int BillOfMaterialPK { get; set; }

        [XmlElement("WIB_OPERTAION")]
        public int OperationPK { get; set; }

        [XmlElement("WIB_OPERTAION_TEXT")]
        public string Operation { get; set; }

        [XmlElement("WIB_WID")]
        public int WorkOrderDetailPK { get; set; }

        [XmlElement("WIB_ITEM")]
        public int ItemPK { get; set; }

        [XmlElement("WIB_ITEM_TEXT")]
        public string Item { get; set; }

        [XmlElement("WIB_ITEM_TYPE")]
        public int ItemTypePK { get; set; }

        [XmlElement("WIB_ITEM_TYPE_TEXT")]
        public string ItemType { get; set; }

        [XmlElement("WIB_CATEGORY")]
        public int CategoryPK { get; set; }

        [XmlElement("WIB_CATEGORY_TEXT")]
        public string Category { get; set; }

        [XmlElement("WIB_MATERIAL")]
        public int MaterialPK { get; set; }

        [XmlElement("WIB_MATERIAL_TEXT")]
        public string Material { get; set; }

        [XmlElement("WIB_UOM")]
        public int UOMPK { get; set; }

        [XmlElement("WIB_UOM_TEXT")]
        public string UOM { get; set; }

        [XmlElement("WIB_QTY")]
        public decimal Quantity { get; set; }

        [XmlElement("WIB_BOM_TYPE")]
        public int BOMTypePK { get; set; }

        [XmlElement("WIB_BOM_TYPE_TEXT")]
        public string BOMType { get; set; }

        [XmlElement("WIB_ACT_QTY")]
        public decimal ActualQuantity { get; set; }

        [XmlElement("WIB_ISS_QTY")]
        public decimal IssueQuantity { get; set; }

        [XmlElement("WIB_BIZUNIT")]
        public int BizUnitPK { get; set; }

        [XmlElement("WIB_REMARKS")]
        public string Remarks { get; set; }

        [XmlElement("WIB_CRTD_BY")]
        public int CreatedUserPK { get; set; }

        [XmlElement("WIB_CRTD_DT")]
        public DateTime CreatedDate { get; set; }

        [XmlElement("WIB_MOD_BY")]
        public int ModifiedUserPK { get; set; }

        [XmlElement("WIB_MOD_DT")]
        public DateTime ModifiedDate { get; set; }

        [XmlElement("ROW_NO")]
        public int SlNo { get; set; }

        [XmlElement("WIB_IS_BOM")]
        public int IsBOM { get; set; }

        public decimal ReturnQty { get; set; }

        [XmlElement("WIB_QTY_BALANCE")]
        public decimal BalanceQty { get; set; }

        [XmlElement("WIB_IS_STOCK_EXIST")]
        public bool IsStockExist { get; set; }

        public bool IsFullyAllocated { get; set; }

        [XmlElement("AllocationDetailList")]
        public List<AllocationDetails> AllocationDetailList { get; set; }
    }

    [Serializable]
    public class AllocationDetails
    {
        [XmlElement("WSA_PK")]
        public int AllocationDetailPK { get; set; }

        [XmlElement("WSA_WIB")]
        public int BillOfMaterialPK { get; set; }

        [XmlElement("WSA_SLNO")]
        public int SlNo { get; set; }

        [XmlElement("WSA_MATERIAL")]
        public int ItemPK { get; set; }

        [XmlElement("WSA_MATERIAL_TEXT")]
        public string Item { get; set; }

        [XmlElement("WSA_UOM")]
        public int UOMPK { get; set; }

        [XmlElement("WSA_SBD_PK")]
        public int BatchPK { get; set; }

        [XmlElement("WSA_SBD_PK_TEXT")]
        public string Batch { get; set; }

        [XmlElement("WSA_BIN_CARD")]
        public int BinCardPK { get; set; }

        [XmlElement("WSA_WIH_PK")]
        public int WOPK { get; set; }

        [XmlElement("WSA_WIB_PK")]
        public int BOMPK { get; set; }

        [XmlElement("WSA_CARTON_MST")]
        public int CartonPK { get; set; }

        [XmlElement("WSA_STK_QTY")]
        public decimal StockQty { get; set; }

        [XmlElement("WSA_ACT_QTY")]
        public decimal ActualQty { get; set; }
    }

    [Serializable]
    public class WorkOrderUploads
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

    public class WorkOrderCancel
    {
        public int WOID { get; set; }
        public string Remarks { get; set; }
        public string RefNo { get; set; }
        public int UserPk { get; set; }
    }

    [Serializable]
    public class IssueDetails
    {
        [XmlElement("P_ISSUE_PK")]
        public int IssuePK { get; set; }

        [XmlElement("P_ITEM_TYPE")]
        public int ItemTypePK { get; set; }
    }
    [Serializable]
    [XmlRoot("Root")]
    public class ProjectBO : WorkflowBO
    {
        [XmlElement("WOH_PK")]
        public int WOH_PK { get; set; }

        [XmlElement("WOH_NO")]
        public string WOH_NO { get; set; }

        [XmlElement("WOH_DATE")]
        public string WOH_DATE { get; set; }

        // For SubContract/SubworkOrder
        [XmlElement("WOH_PARENT")]
        public int WOH_PARENT { get; set; }

        [XmlElement("WOH_CUSTOMER")]
        public int WOH_CUSTOMER { get; set; }

        [XmlElement("WOH_CONSULTANT")]
        public int WOH_CONSULTANT { get; set; }

        [XmlElement("WOH_TYPE")]
        public int WOH_TYPE { get; set; }

        // For SubContract/SubworkOrder
        [XmlElement("WOH_CONTRACTOR")]
        public int WOH_CONTRACTOR { get; set; }

        [XmlElement("WOH_WORK")]
        public string WOH_WORK { get; set; }

        [XmlElement("WOH_SCOPE")]
        public string WOH_SCOPE { get; set; }

        [XmlElement("WOH_REF_NO")]
        public string WOH_REF_NO { get; set; }

        [XmlElement("WOH_REF_DATE")]
        public string WOH_REF_DATE { get; set; }

        [XmlElement("WOH_DESC")]
        public string WOH_DESC { get; set; }

        [XmlElement("WOH_SITE")]
        public string WOH_SITE { get; set; }

        [XmlElement("WOH_LOCATION")]
        public string WOH_LOCATION { get; set; }

        [XmlElement("WOH_EST_START_DATE")]
        public string WOH_EST_START_DATE { get; set; }

        [XmlElement("WOH_EST_END_DATE")]
        public string WOH_EST_END_DATE { get; set; }

        [XmlElement("WOH_ACT_START_DATE")]
        public string WOH_ACT_START_DATE { get; set; }

        [XmlElement("WOH_ACT_END_DATE")]
        public string WOH_ACT_END_DATE { get; set; }

        [XmlElement("WOH_RET_PERC")]
        public double? WOH_RET_PERC { get; set; }

        [XmlElement("WOH_RET_LIMIT")]
        public double? WOH_RET_LIMIT { get; set; }

        [XmlElement("WOH_REMARKS")]
        public string WOH_REMARKS { get; set; }

        //[XmlElement("WOH_BOQ_TEMPLATE")]
        //public string WOH_BOQ_TEMPLATE { get; set; }

        [XmlElement("WOH_DFCT_LBTY")]
        public string WOH_DFCT_LBTY { get; set; }

        [XmlElement("WOH_DFCT_LBTY_UOM")]
        public string WOH_DFCT_LBTY_UOM { get; set; }


        [XmlElement("WOH_CURRENCY")]
        public int WOH_CURRENCY { get; set; }

        [XmlElement("WOH_EXCHG_RATE")]
        public decimal WOH_EXCHG_RATE { get; set; }

        [XmlElement("WOH_SUBJECT")]
        public string WOH_SUBJECT { get; set; }

        [XmlElement("WOH_DEPT")]
        public int WOH_DEPT { get; set; }

        [XmlElement("WOH_IS_DELETED")]
        public int WOH_IS_DELETED { get; set; }

        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }

        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }

        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("WOH_CURRENCY_BC")]
        public int WOH_CURRENCY_BC { get; set; }

        [XmlElement("WOH_COMPANY")]
        public int WOH_COMPANY { get; set; }

        [XmlElement("AMEND_FLAG")]
        public int AMEND_FLAG { get; set; }

        [XmlElement("WOH_AMENDMENT_NO")]
        public string WOH_AMENDMENT_NO { get; set; }

        [XmlElement("WOH_AMENDMENT_DATE")]
        public string WOH_AMENDMENT_DATE { get; set; }

        [XmlElement("WOH_IS_LUMP_SUM")]
        public int WOH_IS_LUMP_SUM { get; set; }

        [XmlElement("WOH_LIC_LIMIT")]
        public int WOH_LIC_LIMIT { get; set; }

        [XmlElement("WOH_LIC_ACTIVE_ONLY")]
        public string WOH_LIC_ACTIVE_ONLY { get; set; }

        [XmlElement("WOH_LUMP_SUM_VALUE")]
        public double WOH_LUMP_SUM_VALUE { get; set; }

        [XmlElement("WOH_PROJECT_BUDGET")]
        public double WOH_PROJECT_BUDGET { get; set; }
        
    }
    public class ProjectCancel
    {
        public int P_WOH_PK { get; set; }
        public string P_WOH_SHORT_CLS_REASON { get; set; }
        public string P_WOH_SHORT_CLS_REFNO { get; set; }
        public int P_USER_PK { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class WOStockAdjustment
    {
        [XmlElement("ICH_DEPT")]
        public int DeptPK { get; set; }

        [XmlElement("ICH_DATE")]
        public DateTime TranDate { get; set; }

        [XmlElement("BizUnitPk")]
        public int BizUnitPk { get; set; }

        [XmlElement("UserPk")]
        public int UserPk { get; set; }

        [XmlElement("Remarks")]
        public string Remarks { get; set; }

        [XmlElement("Detail")]
        public List<WOStockAdjustmentDetail> stockAdjustmentDetails { get; set; }
    }

    [Serializable]
    public class WOStockAdjustmentDetail
    {
        [XmlElement("SL_NO")]
        public int SlNo { get; set; }

        [XmlElement("WIH_PK")]
        public int WOPK { get; set; }

        [XmlElement("WIH_BATCH_PK")]
        public int BatchPK { get; set; }

        [XmlElement("WIH_BATCH_NO")]
        public string BatchNo { get; set; }

        [XmlElement("WIH_ITEM_TYPE")]
        public int ItemType { get; set; }

        [XmlElement("WIH_ITEM_TYPE_TEXT")]
        public string ItemTypeText { get; set; }

        [XmlElement("WIH_CONSUME_QTY")]
        public decimal ConsumedQty { get; set; }

        [XmlElement("WIH_ISSUED_QTY")]
        public decimal IssuedQty { get; set; }

        [XmlElement("WIH_RET_QTY")]
        public decimal ReturnedQty { get; set; }

        [XmlElement("WIH_RCV_BAL")]
        public decimal BalanceQty { get; set; }

        [XmlElement("WIH_TRN_QTY")]
        public decimal AdjustNow { get; set; }

        [XmlElement("WIH_MATERIAL")]
        public int MaterialPK { get; set; }

        [XmlElement("WIH_BATCH_BALANCE")]
        public decimal WIH_BATCH_BALANCE { get; set; }

        [XmlElement("WIH_ADJ_QTY")]
        public decimal AdjustedQty { get; set; }

        [XmlElement("WIH_BOM")]
        public int WIH_BOM { get; set; }
    }    
}
