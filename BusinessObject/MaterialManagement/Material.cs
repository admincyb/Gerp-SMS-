using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace BusinessObject.MaterialManagement
{
    public class Material
    {

        public int MaterialDetailId
        {
            get;
            set;
        }
        public string ITM_CODE
        {
            get;
            set;
        }
        public int ITC_PK
        {
            get;
            set;
        }
        public int UOM_PK
        {
            get;
            set;
        }
        public int ITM_UOM_PURCHASE
        {
            get;
            set;
        }
        public int ITM_UOM_SALE  
        {
            get;
            set;
        }
        public int STATUS
        {
            get;
            set;
        }
        public string ITM_TYPE_TEXT
        {
            get;
            set;
        }
        public int ITM_TYPE
        {
            get;
            set;
        }
        public string MaterialType
        {
            get;
            set;
        }
        public string ITM_NAME
        {
            get;
            set;
        }
        public float ITM_MIN_STK
        {
            get;
            set;
        }
        public float ITM_ROL_STK
        {
            get;
            set;
        }
        public float ITM_MAX_STK
        {
            get;
            set;
        }
        public string ITM_DESC
        {
            get;
            set;
        }
        public int InactivePeriod { get; set; }
        public int UserID
        {
            get;
            set;
        }
        public int SBU
        {
            get;
            set;
        }
        public int UserPk
        {
            get;
            set;
        }
        public int BizUnitPk
        {
            get;
            set;
        }
        public float? ITM_WEIGHT
        {
            get;
            set;
        }

        public int? IPD_PK
        {
            get;
            set;
        }
        public int? IPD_ITEM
        {
            get;
            set;
        }
        public int? IPD_TYPE
        {
            get;
            set;
        }
        public int? IPD_CLASSIFICATION
        {
            get;
            set;
        }
        public float? IPD_INNER_LENGTH
        {
            get;
            set;
        }
        public float? IPD_INNER_BREADTH
        {
            get;
            set;
        }
        public float? IPD_INNER_HEIGHT
        {
            get;
            set;
        }
        public float? IPD_OUTER_LENGTH
        {
            get;
            set;
        }
        public float? IPD_OUTER_BREADTH
        {
            get;
            set;
        }
        public float? IPD_OUTER_HEIGHT
        {
            get;
            set;
        }
        public string IPD_PLY
        {
            get;
            set;
        }
        //public int? IPD_PLY
        //{
        //    get;
        //    set;
        //}
        public string IPD_PAPER_COLOR
        {
            get;
            set;
        }
        public string IPD_ART_WORK
        {
            get;
            set;
        }
        public int? IPD_CUSTOMER
        {
            get;
            set;
        }
        public int? IPD_ACTIVE
        {
            get;
            set;
        }
        public int? IPD_MOD_BY
        {
            get;
            set;
        }
        public DateTime? IPD_MOD_DT
        {
            get;
            set;
        }
        public float? ITM_MOQ
        {
            get;
            set;
        }
        public float? ITM_MAX_OQ
        {
            get;
            set;
        }
        public float? ITM_PHR
        {
            get;
            set;
        }
        public float? ITM_TSC
        {
            get;
            set;
        }
        public string ITM_BATCH_CODE
        {
            get;
            set;
        }
        public int? ITM_GST_CLASS
        {
            get;
            set;
        }
        public int? DOC_PK { get; set; }
        public string DOC_TITLE { get; set; }
        public string DOC_NAME { get; set; }
        public string DOC_TYPE { get; set; }
        public string DOC_PATH { get; set; }
        public int? DOC_SEQ_NO { get; set; }
        public int? ITM_SET
        {
            get;
            set;
        }

        public string IPD_THICKNESS { get; set; }
        public string IPD_PAPER_TYPE { get; set; }
        public int? P_ISD_SIZE { get; set; }
        public decimal? P_OST_QTY_OPENING { get; set; }
        public int? ITM_GROUP { get; set; }
        public DateTime? P_LAST_MOD_DT { get; set; }
        public string ITM_NEED_QC_INSP { get; set; }
        public string ITM_IS_WORK_ORDER { get; set; }
        public string ITM_NEED_BATCH_STK { get; set; }
        public string ITM_IS_LINKED_ITEM { get; set; }
        public int ProductPK { get; set; }
        public string ProductName { get; set; }
        public string ITM_IS_CONVERSION_REQD { get; set; }
        public string ITM_IS_ASSET { get; set; }
    }
    public class VenMaterial
    {

        public int P_ITV_PK
        {
            get;
            set;
        }
        public int P_ITV_ITEM
        {
            get;
            set;
        }
        public int P_ITV_VENDOR
        {
            get;
            set;
        }
        public string P_ITV_NAME
        {
            get;
            set;
        }
        public double P_ITV_PRICE
        {
            get;
            set;
        }
        public int P_ITV_CURRENCY
        {
            get;
            set;
        }
        public float P_ITV_MOQ
        {
            get;
            set;
        }
        public int P_ITV_MOQ_UOM
        {
            get;
            set;
        }
        public int P_ITV_LEAD_TIME
        {
            get;
            set;
        }
        public int P_ACTIVE
        {
            get;
            set;
        }
        public int P_USER_PK
        {
            get;
            set;
        }
        public int P_BIZUNIT
        {
            get;
            set;
        }
        public string P_LAST_MOD_DT
        {
            get;
            set;
        }

    }
    public class PackingDetails
    {

    }
    public class Vendor
    {
        public List<MappingDetailsList> MappingDetailsList { get; set; }
    }
    public class MappingDetailsList
    {
        public int ITV_VENDOR { get; set; }
        public int ITV_PK { get; set; }
        public string ITV_VENDORNAME { get; set; }
        public string ITV_NAME { get; set; }
        public float ITV_PRICE { get; set; }
        public int ITV_CURRENCY { get; set; }
        public string MaterialCurrencyText { get; set; }
        public float ITV_MOQ { get; set; }
        public int ITV_MOQ_UOM { get; set; }
        public string UOMText { get; set; }
        public string ITV_TAX_PERC { get; set; }
        public int ITV_ITEM { get; set; }
        public int ITV_LEAD_TIME { get; set; }
        public int ITV_ACTIVE { get; set; }
    }

    public class WorkFlowMaterial
    {
        public int MaterialProcessID { get; set; }
        public int MaterialTaskID { get; set; }
        public int MaterialActionID { get; set; }
        public int MaterialReferenceID { get; set; }
        public int MaterialApplicationID { get; set; }
    }

    public class TextValueList
    {
        public string Text
        { get; set; }
        public string Value
        { get; set; }
        public string QcInsp
        { get; set; }
    }

    [Serializable]
    [XmlRoot("root")]
    public class VenMappingHeader
    {
        [XmlElement("ITM_PK")]
        public int ITM_PK { get; set; }
        [XmlElement("UserPk")]
        public int UserPk { get; set; }
        [XmlElement("SBU")]
        public short SBU { get; set; }
        [XmlElement("MappingDetailsList")]
        public List<VenMappingDetails> VenMapList { get; set; }
    }

    [Serializable]
    public class VenMappingDetails
    {
        public int ITV_PK { get; set; }
        public int ITV_VENDOR { get; set; }
        public string ITV_VENDORNAME { get; set; }
        public string ITV_NAME { get; set; }
        public float ITV_PRICE { get; set; }
        public int ITV_CURRENCY { get; set; }
        public string MaterialCurrencyText { get; set; }//ITV_CURRENCY_TEXT
        public float ITV_MOQ { get; set; }
        public int ITV_MOQ_UOM { get; set; }
        public string UOMText { get; set; }     //UOM_TEXT  
        public int ITV_ITEM { get; set; }
        public int ITV_LEAD_TIME { get; set; }
        public int ITV_ACTIVE { get; set; }
        public decimal ITV_TAX_PERC { get; set; }
        public decimal ITV_DISC_PERC { get; set; }
        public int ITV_SL_NO { get; set; }

    }

    [Serializable]
    [XmlRoot("root")]
    public class EMIMultiple : WorkflowBO
    {
        [XmlElement("UserPk")]
        public int UserPk { get; set; }
        [XmlElement("BizUnitPk")]
        public int BizUnitPk { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("ICH_TRX_TYPE")]
        public int ICH_TRX_TYPE { get; set; }
        [XmlElement("ITM_CODE")]
        public string ITM_CODE { get; set; }
        [XmlElement("ICH_STATUS")]
        public int ICH_STATUS { get; set; }
        [XmlElement("AST_VALUE")]
        public int AST_VALUE { get; set; }
        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }
        [XmlElement("ICH_DATE")]
        public string ICH_DATE { get; set; }
        [XmlElement("ICH_NO")]
        public string ICH_NO { get; set; }
        [XmlElement("ICH_ISS_RCV_TYPE")]
        public int ICH_ISS_RCV_TYPE { get; set; }
        [XmlElement("ICH_REF_NO")]
        public string ICH_REF_NO { get; set; }
        [XmlElement("ICH_DEPT")]
        public int ICH_DEPT { get; set; }
        [XmlElement("ICH_ISS_RCV_PK")]
        public int ICH_ISS_RCV_PK { get; set; }
        [XmlElement("ICH_ITEM_TYPE")]
        public int ICH_ITEM_TYPE { get; set; }
        [XmlElement("ICH_ISS_RCV_NAME")]
        public string ICH_ISS_RCV_NAME { get; set; }
        [XmlElement("ICH_COMPANY")]
        public int ICH_COMPANY { get; set; }
        [XmlElement("ICH_PK")]
        public int ICH_PK { get; set; }
        [XmlElement("ICH_CRDR_NOTE_HDR")]
        public int ICH_CRDR_NOTE_HDR { get; set; }
        [XmlElement("ICH_CRDR_FLAG")]
        public int ICH_CRDR_FLAG { get; set; }
        [XmlElement("WKF_REFERENCE")]
        public int WKF_REFERENCE { get; set; }
        [XmlElement("WKF_APPLICATION")]
        public int WKF_APPLICATION { get; set; }
        [XmlElement("WKF_PROCESS")]
        public int WKF_PROCESS { get; set; }
        [XmlElement("WKF_TASK")]
        public int WKF_TASK { get; set; }
        [XmlElement("WKF_TASK_ACTION")]
        public int WKF_TASK_ACTION { get; set; }
        [XmlElement("WKF_COMMENTS")]
        public string WKF_COMMENTS { get; set; }
        [XmlElement("WKF_TRX_FLAG")]
        public int WKF_TRX_FLAG { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("ICD_PK")]
        public int ICD_PK { get; set; }
        [XmlElement("ICH_IS_EDIT")]
        public int ICH_IS_EDIT { get; set; }
        [XmlElement("WRKFACT_ID")]
        public int WRKFACT_ID { get; set; }

        [XmlElement("ICH_VERSION")]
        public int ICH_VERSION { get; set; }
        [XmlElement("ICH_DEPT_TEXT")]
        public string ICH_DEPT_TEXT { get; set; }
        [XmlElement("ICH_ACTIVE")]
        public int ICH_ACTIVE { get; set; }
        [XmlElement("ICH_BIZUNIT")]
        public int ICH_BIZUNIT { get; set; }
        [XmlElement("ICH_CRTD_BY")]
        public int ICH_CRTD_BY { get; set; }
        [XmlElement("ICH_CRTD_DT")]
        public string ICH_CRTD_DT { get; set; }
        [XmlElement("ICH_MOD_BY")]
        public int ICH_MOD_BY { get; set; }


        [XmlElement("ConsumptionDtl")]
        public List<EMIMultipleDetails> EMIMultipleDetails { get; set; }
    }

    [Serializable]
    public class EMIMultipleDetails
    {
        [XmlElement("ROW_NO")]
        public int ROW_NO { get; set; }
        [XmlElement("ICD_PK")]
        public int ICD_PK { get; set; }
        [XmlElement("ICD_SL_NO")]
        public int ICD_SL_NO { get; set; }
        [XmlElement("ICD_ITEM")]
        public int ICD_ITEM { get; set; }
        [XmlElement("ICD_ITEM_TEXT")]
        public string ICD_ITEM_TEXT { get; set; }
        [XmlElement("ITM_NEED_BATCH_STK")]
        public decimal ITM_NEED_BATCH_STK { get; set; }
        [XmlElement("ICD_ITEM_CATEGORY")]
        public int ICD_ITEM_CATEGORY { get; set; }
        [XmlElement("ICD_ITEM_CATEGORY_TEXT")]
        public string ICD_ITEM_CATEGORY_TEXT { get; set; }
        [XmlElement("ICD_STK_BATCH")]
        public int ICD_STK_BATCH { get; set; }
        [XmlElement("ICD_STK_BATCH_NO")]
        public string ICD_STK_BATCH_NO { get; set; }
        [XmlElement("ICD_STK_BATCH_TEXT")]
        public string ICD_STK_BATCH_TEXT { get; set; }
        [XmlElement("ICD_QTY_CONSUMED")]
        public decimal ICD_QTY_CONSUMED { get; set; }
        [XmlElement("ICD_UOM")]
        public int ICD_UOM { get; set; }
        [XmlElement("ICD_UOM_TEXT")]
        public string ICD_UOM_TEXT { get; set; }
        [XmlElement("ICD_ITEM_TYPE")]
        public int ICD_ITEM_TYPE { get; set; }
        [XmlElement("ICD_ISRETURNTEXT")]
        public string ICD_ISRETURNTEXT { get; set; }
        [XmlElement("ICD_REMARKS")]
        public string ICD_REMARKS { get; set; }
        [XmlElement("ICD_CURRENT_STK")]
        public decimal ICD_CURRENT_STK { get; set; }
        [XmlElement("ICD_VALUE_CONSUMED")]
        public decimal ICD_VALUE_CONSUMED { get; set; }
        [XmlElement("ICD_RATE")]
        public decimal ICD_RATE { get; set; }
        [XmlElement("ICD_LOT_NO")]
        public int ICD_LOT_NO { get; set; }
        [XmlElement("ICD_ISS_RCV_TYPE")]
        public int ICD_ISS_RCV_TYPE { get; set; }
        [XmlElement("ICD_ISS_RCV_PK")]
        public int ICD_ISS_RCV_PK { get; set; }
        [XmlElement("ICD_ISS_RCV_SUB_TYPE")]
        public int ICD_ISS_RCV_SUB_TYPE { get; set; }
        [XmlElement("ICD_ISS_RCV_NAME")]
        public string ICD_ISS_RCV_NAME { get; set; }
    }

    [Serializable]
    public class MaterialBO
    {
        public int ITM_PK { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int ITC_PK { get; set; }
        public int UOM_PK { get; set; }
        public int ITM_UOM_PURCHASE { get; set; }
        public int ITM_UOM_SALE { get; set; }      
        public int? ITM_SET { get; set; }      
        public int UserPk { get; set; }
        public int ITM_ACTIVE { get; set; }
        public int SBU { get; set; }
        public int BizUnitPk { get; set; }
        public DateTime? P_LAST_MOD_DT { get; set; }
        public string ITM_NEED_BATCH_STK { get; set; }
        public int ITM_TYPE { get; set; }
        public float ITM_MIN_STK {  get;  set;  }
        public float ITM_ROL_STK {  get;  set;  }
        public float ITM_MAX_STK {  get;  set;  }
        public float? ITM_MOQ    {  get;  set;
        }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class WorkOrderBomBO
    {
        [XmlElement("WIM_PK")]
        public int WomPK { get; set; }

        [XmlElement("WOM_ITEM")]
        public int WomItem { get; set; }
        [XmlElement("WOM_BIZUNIT")]
        public int BizUnit { get; set; }

        [XmlElement("WOM_CRTD_BY")]
        public int UserPK { get; set; }

        //[XmlElement("WOM_CRTD_DT")]
        public DateTime CreatedDate { get; set; }

        [XmlElement("WOM_QTY")]
        public decimal Bom_Qty { get; set; }

        [XmlElement("WOM_BOM_UOMTEXT")]
        public string Bom_UomText { get; set; }

        [XmlElement("WOM_BOM_UOM")]
        public int Bom_Uom { get; set; }

        [XmlElement("WOMModOn")]
        public DateTime WOMModOn { get; set; }

        [XmlElement("WOMModBy")]
        public int WOMModBy { get; set; }

        [XmlElement("WOM_ITEM_TYPE")]
        public int WOMItemType { get; set; }

        [XmlElement("Deatils")]
        public List<BOMDetails> Details { get; set; }

    }
    [Serializable]
    public class BOMDetails
    {
        [XmlElement("SL_NO")]
        public int SlNo { get; set; }

        [XmlElement("WOM_MATERIAL")]
        public int BOMaterialPK { get; set; }

        [XmlElement("WOM_MATERIAL_TEXT")]
        public string BOMaterial { get; set; }

        [XmlElement("WOM_OPERATION")]
        public int Operation { get; set; }
        [XmlElement("WOM_OPERATION_TEXT")]
        public string OperationText { get; set; }

        [XmlElement("WOM_BOM_TYPE")]
        public int BomType { get; set; }

        [XmlElement("WOM_BOM_TYPE_TEXT")]
        public string BomTypeText { get; set; }

        [XmlElement("WOM_UOM")]
        public int UOM { get; set; }

        [XmlElement("WOM_UOM_TEXT")]
        public string UomText { get; set; }

        [XmlElement("WOM_BOM_QTY")]
        public decimal QTY { get; set; }

        [XmlElement("WOM_MATERIAL_CATEGORY")]
        public int BomMaterialCat { get; set; }

        [XmlElement("WOM_MATERIAL_CATEGORY_TEXT")]
        public string BomMaterialCatText { get; set; }

        [XmlElement("WOM_BOM_ITEM_TYPE")]
        public int BomItemType { get; set; }

        [XmlElement("WOM_BOM_ITEM_TYPE_TEXT")]
        public string BomItemTypeText { get; set; }

        [XmlElement("WOM_TOLERENCE")]
        public decimal Tolerance { get; set; }

        [XmlElement("WOM_ISSUE_QTY")]
        public decimal IssueQty { get; set; }

        [XmlElement("WOM_ISSUE_UOM")]
        public int IssueUOM { get; set; }

        [XmlElement("WOM_ISSUE_UOM_TEXT")]
        public string IssueUomText { get; set; }

        [XmlElement("WOM_IS_PRD_MAP")]
        public int IsPrdMapp { get; set; }

        [XmlElement("WOM_CUS_NAME")]
        public string CustomerName { get; set; }

        [XmlElement("WOM_CUSTOMER")]
        public int CustomerPK { get; set; }

        [XmlElement("WOM_BRAND_NAME")]
        public string BrandName { get; set; }

        [XmlElement("WOM_BRAND")]
        public int BrandPK { get; set; }

        [XmlElement("WOM_BRAND_CODE")]
        public string BrandCode { get; set; }
    }

}
