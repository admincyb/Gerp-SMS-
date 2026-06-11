using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Shipping
{
    public class ContainerReleaseBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class ContainerReleaseHeader
    {
        [XmlElement("CRH_PK")]
        public int CRH_PK { get; set; }
        [XmlElement("CRH_SHIPPING_PLAN")]
        public int CRH_SHIPPING_PLAN { get; set; }
        [XmlElement("CRH_DATE")]
        public DateTime CRH_DATE { get; set; }
        [XmlElement("CRH_LOAD_DATE")]
        public DateTime CRH_LOAD_DATE { get; set; }
        [XmlElement("CRH_REMARKS")]
        public string CRH_TRAILER_LIC_NO { get; set; }
        [XmlElement("CRH_TRAILER_LIC_NO")]
        public string CRH_TRUCK_LIC_NO { get; set; }
        [XmlElement("CRH_TRUCK_LIC_NO")]
        public string CRH_REMARKS { get; set; }
        [XmlElement("CRH_ACTIVE")]
        public int CRH_ACTIVE { get; set; }
        [XmlElement("CRH_BIZUNIT")]
        public short CRH_BIZUNIT { get; set; }
        [XmlElement("CRH_DEPT")]
        public int CRH_DEPT { get; set; }
        [XmlElement("CRH_CRTD_BY")]
        public int CRH_CRTD_BY { get; set; }
        [XmlElement("CRH_CRTD_DT")]
        public DateTime CRH_CRTD_DT { get; set; }
        [XmlElement("CRH_MOD_BY")]
        public int CRH_MOD_BY { get; set; }
        [XmlElement("CRH_MOD_DT")]
        public DateTime CRH_MOD_DT { get; set; }
        [XmlElement("SNH_PK")]
        public int SNH_PK { get; set; }
        [XmlElement("SNH_NO")]
        public string SNH_NO { get; set; }
        [XmlElement("CSH_CONTAINER_NO")]
        public string CSH_CONTAINER_NO { get; set; }
        [XmlElement("CSH_SEAL_NO")]
        public string CSH_SEAL_NO { get; set; }
        [XmlElement("CSH_IN_TIME")]
        public DateTime CSH_IN_TIME { get; set; }
        [XmlElement("CSH_IN_TIME_S")]
        public string CSH_IN_TIME_S { get; set; }
        [XmlElement("SNH_SHIP_TO_PORT")]
        public string SNH_SHIP_TO_PORT { get; set; }
        [XmlElement("CRH_COMPANY")]
        public int CRH_COMPANY { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class ContainerReleaseHdr
    {
        [XmlElement("CRH_PK")]
        public int CRH_PK { get; set; }  // P_CRH_PK
        [XmlElement("CRH_SHIPPING_PLAN")]
        public int CRH_SHIPPING_PLAN { get; set; } // P_CRH_SHIPPING_PLAN
        [XmlElement("CRH_DATE")]
        public DateTime CRH_DATE { get; set; } // P_CRH_DATE
        [XmlElement("CRH_LOAD_DATE")]
        public DateTime CRH_LOAD_DATE { get; set; }
        [XmlElement("CRH_TRAILER_LIC_NO")]
        public string CRH_TRAILER_LIC_NO { get; set; }
        [XmlElement("CRH_TRUCK_LIC_NO")]
        public string CRH_TRUCK_LIC_NO { get; set; }
        [XmlElement("CRH_REMARKS")]
        public string CRH_REMARKS { get; set; } // P_CRH_REMARKS
        [XmlElement("CRH_DEPT")]
        public int CRH_DEPT { get; set; } // P_CRH_DEPT
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }  // P_ACTIVE
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; } // P_USER_PK
        [XmlElement("BIZUNIT_PK")]
        public short BIZUNIT_PK { get; set; }  // P_BIZUNIT
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; } // P_LAST_MOD_DT
        [XmlElement("P_RET_VAL")]
        public int P_RET_VAL { get; set; }
        [XmlElement("WKF_PROCESS")]
        public int WKF_PROCESS { get; set; }
        [XmlElement("CRH_COMPANY")]
        public int CRH_COMPANY { get; set; } // P_CRH_COMPANY
        [XmlElement("IS_ENABLE_PAC_COST")]
        public string IS_ENABLE_PAC_COST { get; set; }
        [XmlElement("IS_ENABLE_PRD_COST")]
        public string IS_ENABLE_PRD_COST { get; set; }
        [XmlElement("Detail")]
        public List<ContainerReleaseDetail> ContainerReleaseDetails { get; set; }
    }

    [Serializable]
    [XmlRoot("Details")]
    public class ContainerReleaseDetail
    {
        private decimal cdr_qty_despatched;
        private decimal cdr_do_qty;
        private decimal cdr_do_qty_pcs;

        [XmlElement("CDR_PK")]
        public int CDR_PK { get; set; }
        [XmlElement("CDR_SL_NO")]
        public int CDR_SL_NO { get; set; }
        [XmlElement("CDR_DO_DTL")]
        public int CDR_DO_DTL { get; set; }
        [XmlElement("CDR_SO_DTL")]
        public int CDR_SO_DTL { get; set; }
        [XmlElement("CDR_CUST_ITEM")]
        public int CDR_CUST_ITEM { get; set; }  // Brand Pk
        [XmlElement("CDR_ITEM")]
        public int CDR_ITEM { get; set; } // ITM_PK
        [XmlElement("CDR_QTY_DESPATCHED")]
        public decimal CDR_QTY_DESPATCHED // decimal // DPD_QTY_APPROVED
        {
            get
            {
                return Math.Round(cdr_qty_despatched, 0);
            }
            set
            {
                cdr_qty_despatched = value;
            }
        }
        //[XmlElement("CDR_QTY_APPROVED")]
        //public decimal CDR_QTY_APPROVED { get; set; } // DPD_QTY_APPROVED
        [XmlElement("CDR_UOM")]
        public int CDR_UOM { get; set; }
        [XmlElement("DPH_IS_INVOICE_APPROVED")]
        public int DPH_IS_INVOICE_APPROVED { get; set; }

        [XmlElement("CDR_IS_PACKED_BIN")]
        public string CDR_IS_PACKED_BIN { get; set; }  // Is packed bin or Not

        public string SOD_NO { get; set; }
        public int SOD_SO { get; set; }
        public DateTime SOD_DATE { get; set; }
        public string ITM_CODE { get; set; }
        // public int ITM_PK { get; set; }
        public string ITM_NAME { get; set; }
        public string CIM_BRAND_NAME { get; set; }
        public string UOM_CODE { get; set; }
        public decimal CDR_CARTON_DESPATCHED { get; set; }
        //public int APS_TOTAL_PCS { get; set; }
        public decimal APS_TOTAL_PCS { get; set; }

        public decimal CDR_DO_QTY // decimal
        {
            get
            {
                return Math.Round(cdr_do_qty, 0);
            }
            set
            {
                cdr_do_qty = value;
            }
        }
        public decimal CDR_DO_QTY_PCS // decimal
        {
            get
            {
                return Math.Round(cdr_do_qty_pcs, 0);
            }
            set
            {
                cdr_do_qty_pcs = value;
            }
        }
        public decimal CDR_DO_CARTON { get; set; }

        [XmlElement("SOD_IS_PACK_MAT")]
        public int SOD_IS_PACK_MAT { get; set; }

        [XmlElement("CIM_SALE_UOM")]
        public int SaleUnit { get; set; }

        [XmlElement("CartonDetail")] // CartonDetails
        public List<Cartons> CartonDetails { get; set; }

        //public decimal DPD_QTY_APPROVED { get; set; }
        //public decimal DPD_CARTON_APPROVED { get; set; }

        [XmlElement("ContainerItems")]
        public ContainerItems ContainerItems { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class ContainerReleaseDetailGet
    {
        [XmlElement("Detail")] // ContainerReleaseDetail
        public List<ContainerReleaseDetail> ContainerReleaseDetail { get; set; }
    }
    
    [Serializable]
    public class Cartons
    {
        private decimal crc_qty_despatched;
        [XmlElement("CRC_SL_NO")]//CRC_SL_NO
        public int CRC_SL_NO { get; set; }
        [XmlElement("CRC_PK")]
        public int CRC_PK { get; set; }
        [XmlElement("DSC_PK")]
        public int DSC_PK { get; set; }
        [XmlElement("CRC_CARTON_MST")]
        public int CRC_CARTON_MST { get; set; }
        [XmlElement("BCR_NO")]
        public string BCR_NO { get; set; }
        [XmlElement("BCR_PALLET")]
        public int BCR_PALLET { get; set; }
        [XmlElement("BCR_PALLET_NO")]
        public string BCR_PALLET_NO { get; set; }
        [XmlElement("CRC_QTY_DESPATCHED")]
        public decimal CRC_QTY_DESPATCHED
        {
            get
            {
                return Math.Round(crc_qty_despatched, 0);
            }
            set
            {
                crc_qty_despatched = value;
            }
        }

        [XmlElement("BCR_LOCATION_TEXT")]
        public string BCR_LOCATION_TEXT { get; set; }
        [XmlElement("CRC_CDR_SL_NO")]
        public int CRC_CDR_SL_NO { get; set; }

        [XmlElement("CRC_SL_NO_GRP")]
        public int CRC_SL_NO_GRP { get; set; }

        [XmlElement("BCR_IS_PARTIAL")]
        public int BCR_IS_PARTIAL { get; set; }
    }

    [Serializable]
    public class CartonsGridview
    {
        public int CRC_SL_NO_GRP { get; set; }
       
        public string BCR_PALLET_NO { get; set; }
        public int CartonsCount { get; set; }
        public int QtyPcs { get; set; }
        public string BCR_LOCATION_TEXT { get; set; }       
    }

    [Serializable]
    public class ContainerItems
    {
        [XmlElement("CDR_PK")]
        public int ContainerReleaseDetailPK { get; set; }

        [XmlElement("CRI_ITM")]
        public int ItemPK { get; set; }

        [XmlElement("CRI_ITM_TEXT")]
        public string ItemName { get; set; }

        [XmlElement("CRI_ITM_TYPE")]
        public int ItemType { get; set; }

        [XmlElement("CRI_SOH_PK")]
        public int SaleContractPK { get; set; }

        [XmlElement("CRI_SOH_NO")]
        public string SaleContract { get; set; }

        [XmlElement("CRI_QTY")]
        public decimal Quantity { get; set; }

        [XmlElement("CRI_DPT_STORE")]
        public int StorePK { get; set; }

        [XmlElement("CRI_DPT_STORE_TEXT")]
        public string Store { get; set; }

        public bool IsSameItemExists { get; set; }

        [XmlElement("ItemDetailsList")]
        public List<ContainerItemDetails> ItemDetailsList { get; set; }
    }

    [Serializable]
    public class ContainerItemDetails
    {
        [XmlElement("CRID_BATCH")]
        public int BatchPK { get; set; }

        [XmlElement("CRID_BATCH_NO")]
        public string Batch { get; set; }

        [XmlElement("CRID_BINCARD")]
        public int BincardPK { get; set; }

        [XmlElement("CRID_BIN_CARD_TEXT")]
        public string Bincard { get; set; }

        [XmlElement("CRID_ITM")]
        public int ItemPK { get; set; }

        [XmlElement("ItemType")]
        public int ItemType { get; set; }

        [XmlElement("CRID_UOM")]
        public int UOMPK { get; set; }

        [XmlElement("CRID_UOM_TEXT")]
        public string UOM { get; set; }

        [XmlElement("CRID_QTY")]
        public decimal Quantity { get; set; }

        [XmlElement("CRID_TOT_PCS")]
        public decimal TotalPcs { get; set; }

        [XmlElement("CRID_SLNO")]
        public int SlNo { get; set; }
    }
}
