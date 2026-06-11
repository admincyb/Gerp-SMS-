using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;

namespace BusinessObject.StoreManagement
{
    public class DirectStockAdmissionBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class DirectStockAdmission : WorkflowBO
        {
            [XmlElement("GRH_PK")]
            public int GRH_PK { get; set; }
            [XmlElement("GRH_NO")]
            public string GRH_NO { get; set; }
            [XmlElement("GRH_VERSION")]
            public string GRH_VERSION { get; set; }
            [XmlElement("GRH_DATE")]
            public string GRH_DATE { get; set; }
            [XmlElement("GRH_VENDOR")]
            public int GRH_VENDOR { get; set; }
            [XmlElement("GRH_VND_REF_NO")]
            public string GRH_VND_REF_NO { get; set; }
            [XmlElement("GRH_VND_REF_DATE")]
            public string GRH_VND_REF_DATE { get; set; }
            //[XmlElement("GRH_TOTAL_QTY")]
            //public decimal GRH_TOTAL_QTY { get; set; }
            //[XmlElement("GRH_REMARKS")]
            //public string GRH_REMARKS { get; set; }
            //[XmlElement("GRH_COMMENTS")]
            //public string GRH_COMMENTS { get; set; }
            [XmlElement("GRH_INV_DEPT")]
            public int GRH_INV_DEPT { get; set; }
            [XmlElement("GRH_COMPANY")]
            public int GRH_COMPANY { get; set; }
            [XmlElement("BIZUNIT_PK")]
            public int BIZUNIT_PK { get; set; }
            [XmlElement("ACTIVE")]
            public int ACTIVE { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public string LAST_MOD_DT { get; set; }
            [XmlElement("Detail")]
            public List<DirectStockAdmissionDetails> StockAdmissionList { get; set; }


            [XmlElement("APT_CODE")]
            public string APT_CODE { get; set; }
            [XmlElement("AST_DOC_MODE")]
            public int AST_DOC_MODE { get; set; }
            [XmlElement("WKF_FLAG")]
            public int WKF_FLAG { get; set; }
            //[XmlElement("WKF_PROCESS")]
            //public int WKF_PROCESS { get; set; }
            [XmlElement("STK_VAL_CONFIRM")]
            public int STK_VAL_CONFIRM { get; set; }
            [XmlElement("GRH_IS_WORK_ORDER")]
            public int GRH_IS_WORK_ORDER { get; set; }

            [XmlElement("GRH_EXCESS_PERC")]
            public double GRH_EXCESS_PERC { get; set; }
            [XmlElement("PO_QTY_VALIDATE")]
            public string PO_QTY_VALIDATE { get; set; }

            [XmlElement("FileList")]
            public List<DirectStockAdmissionUploads> FileList { get; set; }

            [XmlElement("MaterialReturnList")]
            public List<DirectStockAdmissionBO.MaterialReturn> MaterialReturnList { get; set; }
        }

        [Serializable]
        public class DirectStockAdmissionDetails
        {
            [XmlElement("GRD_PK")]
            public int GRD_PK { get; set; }
            [XmlElement("GRD_NO")]
            public string GRD_NO { get; set; }
            [XmlElement("GRD_DATE")]
            public string GRD_DATE { get; set; }
            [XmlElement("GRD_VERSION")]
            public string GRD_VERSION { get; set; }
            [XmlElement("GRD_SL_NO")]
            public int GRD_SL_NO { get; set; }
            [XmlElement("GRD_ITEM")]
            public int GRD_ITEM { get; set; }
            [XmlElement("GRD_QTY_RECEIVED")]
            public decimal GRD_QTY_RECEIVED { get; set; }
            [XmlElement("GRD_QTY_ACCEPTED")]
            public decimal GRD_QTY_ACCEPTED { get; set; }
            [XmlElement("GRD_QTY_REJECTED")]
            public decimal GRD_QTY_REJECTED { get; set; }
            [XmlElement("GRD_UOM")]
            public int GRD_UOM { get; set; }
            [XmlElement("GRD_DOM")]
            public string GRD_DOM { get; set; }
            [XmlElement("GRD_DOE")]
            public string GRD_DOE { get; set; }
            [XmlElement("GRD_PO")]
            public int GRD_PO { get; set; }
            [XmlElement("GRD_PO_DTL")]
            public int GRD_PO_DTL { get; set; }
            [XmlElement("GRD_PO_RATE")]
            public double GRD_PO_RATE { get; set; }
            //<GRD_INSTRUCTIONS></GRD_INSTRUCTIONS>
            //<GRD_REMARKS></GRD_REMARKS>
            [XmlElement("GRD_DEPT")]
            public int GRD_DEPT { get; set; }
            [XmlElement("GRD_BIZUNIT")]
            public int GRD_BIZUNIT { get; set; }
            [XmlElement("GRD_VND_REF_NO")]
            public string GRD_VND_REF_NO { get; set; }  // For Batch No
            [XmlElement("GRD_QA_LOT_NO")]
            public string GRD_QA_LOT_NO { get; set; }
                        
            [XmlElement("GRD_LOCATION")]
            public string GRD_LOCATION { get; set; }
            [XmlElement("GRD_QTY_REAL")]
            public decimal GRD_QTY_REAL { get; set; }

            [XmlElement("GRD_WIH_ITEM_TYPE")]
            public string GRD_WIH_ITEM_TYPE { get; set; }


            //[XmlElement("GRD_BATCH_NO")]
            //public string GRD_BATCH_NO { get; set; }
            [XmlElement("DamageList")]
            public List<DamageItem> DamageList { get; set; }
        }

        [Serializable]
        public class DamageItem
        {
            [XmlElement("GDD_PK")]
            public int GDD_PK { get; set; }

            [XmlElement("GDD_SL_NO")]
            public int GDD_SL_NO { get; set; }

            [XmlElement("GDD_GRN_DTL")]
            public int GDD_GRN_DTL { get; set; }
            [XmlElement("GDD_ITEM")]
            public int GDD_ITEM { get; set; }
            [XmlElement("GDD_DMG_TYPE")]
            public int GDD_DMG_TYPE { get; set; }
            [XmlElement("GDD_DMG_QTY")]
            public decimal GDD_DMG_QTY { get; set; }
            [XmlElement("GDD_DEPT_STORE")]
            public int GDD_DEPT_STORE { get; set; }

            [XmlElement("GDD_ITEM_NAME")]
            public string GDD_ITEM_NAME { get; set; }
            [XmlElement("GDD_DMG_TYPE_TEXT")]
            public string GDD_DMG_TYPE_TEXT { get; set; }
            [XmlElement("GDD_DEPT_STORE_NAME")]
            public string GDD_DEPT_STORE_NAME { get; set; }


        }

        [Serializable]
        public class PendingPO
        {
            public int Pk { get; set; }
            public bool AddedToStockList { get; set; }
            public bool CheckBoxChecked { get; set; }
            public int PODetId { get; set; }


            public int POId { get; set; }
            public string PONumber { get; set; }
            public int ItemId { get; set; }
            public string ItemName { get; set; }
           // public string ItemCode { get; set; }
            public int UOMId { get; set; }
            public string UOM { get; set; }
            public decimal POQty { get; set; }
            public decimal PreRecievedQty { get; set; }
            public decimal BalanceQtyToRecieve { get; set; }
            public double PORate { get; set; }
            public int WIH_ITEM_TYPE { get; set; }
            public string WIH_ITEM_TYPE_TEXT { get; set; }
            public decimal POD_QTY_RETURNED { get; set; }
        }

        [Serializable]
        public class StockAdmissionItem
        {
            public int Pk { get; set; }
            public int SlNo { get; set; }

            public int ItemId { get; set; }
            public string ItemName { get; set; }
            public int UOM { get; set; }
            public decimal POQty { get; set; }
            public decimal PendingQty { get; set; }
            public decimal ReturnQty { get; set; }
            public double PORate { get; set; }
            public string BatchNo { get; set; }
            public string QALotNo { get; set; }
            public decimal Recieved { get; set; }
            public decimal Accepted { get; set; }
            public decimal Rejected { get; set; }
            public int PODetailId { get; set; }

            public int POId { get; set; }
            public string PONumber { get; set; }

            public string DOM { get; set; }
            public string DOE { get; set; }
            public string Location { get; set; }
            public decimal RealWeight { get; set; }
            public int WIH_ITEM_TYPE { get; set; }
            public string WIH_ITEM_TYPE_TEXT { get; set; }

            public List<RejectedDetail> RejectedList { get; set; }
        }

        [Serializable]
        public class RejectedDetail
        {
            public int Pk { get; set; }
            public int SlNo { get; set; }
            public int HdrSlNo { get; set; }
            public int PODetailId { get; set; }

            public int ItemId { get; set; }
            public string ItemText { get; set; }
            public decimal Quantity { get; set; }
            public int ReasonId { get; set; }
            public string ReasonText { get; set; }
            public int RejectedToStoreId { get; set; }
            public string RejectedToStoreText { get; set; }
        }

        [Serializable]
        [XmlRoot("Root")]
        public class WOHeader
        {
            [XmlElement("WOList")]
            public List<WOHeaderList> WOList { get; set; }
        }
        [Serializable]
        public class WOHeaderList
        {
            [XmlElement("WIH_PK")]
            public int WOPK { get; set; }

            [XmlElement("WID_ITEM")]
            public int WOItemPK { get; set; }

            [XmlElement("RECEIVED_QTY")]
            public decimal ReceivedQty { get; set; }
        }

        [Serializable]
        [XmlRoot("Root")]
        public class MaterialReturnRoot
        {
            [XmlElement("MaterialReturnList")]
            public List<MaterialReturn> MaterialReturnList { get; set; }
        }

        [Serializable]
        public class MaterialReturn
        {
         

            [XmlElement("GMR_PK")]
            public int MaterialReturnPK { get; set; }

            [XmlElement("GMR_GR")]
            public int StockAdmissionPK { get; set; }

            [XmlElement("GMR_SL_NO")]
            public int SlNo { get; set; }

            [XmlElement("GMR_ITEM_TYPE")]
            public int ItemTypePK { get; set; }

            [XmlElement("WIB_ITEM_TYPE_TEXT")]
            public string ItemType { get; set; }

            [XmlElement("GMR_WO")]
            public int WOPK { get; set; }

            [XmlElement("WIH_NO")]
            public string WONo { get; set; }

            [XmlElement("GMR_ITEM_CATEGORY")]
            public int CategoryPK { get; set; }

            [XmlElement("WIB_CATEGORY_TEXT")]
            public string Category { get; set; }

            [XmlElement("GMR_MATERIAL")]
            public int IssueItemPK { get; set; }

            [XmlElement("MAT_NAME")]
            public string IssueItem { get; set; }

            [XmlElement("MRD_ITEM")]
            public int ItemPK { get; set; }

            [XmlElement("WID_ITEM")]
            public int WOItemPK { get; set; }

            [XmlElement("ITM_NAME_TEXT")]
            public string ItemName { get; set; }

            [XmlElement("GMR_UOM")]
            public int UOMPK { get; set; }

            [XmlElement("UOM_CODE")]
            public string UOM { get; set; }

            public int BatchPK { get; set; }
            public string Batch { get; set; }

            [XmlElement("GMR_QTY_RQRD")]
            public decimal RequiredQty { get; set; }

            [XmlElement("GMR_QTY_RCVD")]
            public decimal PrevReceivedQty { get; set; }

            [XmlElement("GMR_QTY_RTRD")]
            public decimal ReturnQty { get; set; }

            [XmlElement("GMR_QTY_BALANCE")]
            public decimal BalanceQty { get; set; }

            [XmlElement("GMR_BIZUNIT")]
            public int BizUnitPK { get; set; }

            [XmlElement("GMR_SCRAP_OR_BGRADE")]
            public int GMR_SCRAP_OR_BGRADE { get; set; }
            public int IsValueChanged { get; set; }

            [XmlElement("GMR_AFTER_MULTI")]
            public int GMR_AFTER_MULTI { get; set; }

            [XmlElement("BatchDetailList")]
            public List<BatchDetails> BatchDetailList { get; set; }
        }

        [Serializable]
        public class BatchDetails
        {
            [XmlElement("GMD_PK")]
            public int BatcDetailPK { get; set; }

            [XmlElement("GMD_GM")]
            public int MaterialReturnPK { get; set; }

            [XmlElement("GMD_GR")]
            public int StockAdmissionPK { get; set; }

            [XmlElement("GMD_SL_NO")]
            public int SlNo { get; set; }

            [XmlElement("GMD_WO")]
            public int WOPK { get; set; }

            [XmlElement("GMD_WIB_PK")]
            public int BOMPK { get; set; }

            public string WONo { get; set; }

            [XmlElement("GMD_MATERIAL")]
            public int ItemPK { get; set; }

            public string Item { get; set; }

            [XmlElement("GMD_UOM")]
            public int UOMPK { get; set; }

            [XmlElement("GMD_BATCH")]
            public int BatchPK { get; set; }

            [XmlElement("GMD_BATCH_NO")]
            public string Batch { get; set; }

            [XmlElement("GMD_BIN_CARD")]
            public int BinCardPK { get; set; }

            [XmlElement("WIH_PK_TEXT")]
            public string WOPKs { get; set; }

            [XmlElement("WIH_NO_TEXT")]
            public string WONos { get; set; }

            public bool HasRowcolor { get; set; }

            public decimal StockQty { get; set; }

            [XmlElement("GMD_QTY")]
            public decimal ActualQty { get; set; }
        }
    }
    public class FileDetails
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
    public class DirectStockAdmissionUploads
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
    public class GRNConversion
    {
        [XmlElement("GRN_PK")]
        public int GRNPK { get; set; }

        [XmlElement("SBD_PK")]
        public int StockPK { get; set; }

        [XmlElement("USER_PK")]
        public int UserPK { get; set; }

        [XmlElement("BIZUNIT")]
        public int SBUPK { get; set; }

        [XmlElement("CONVERT_DATE")]
        public DateTime ConvertDate { get; set; }

        [XmlElement("ReceivedList")]
        public List<ConvertItems> ReceivedItemList { get; set; }

        [XmlElement("ConvertList")]
        public List<ConvertItems> ConvertItemList { get; set; }
    }

    [Serializable]
    public class ConvertItems
    {
        [XmlElement("ITEM_PK")]
        public int ItemPK { get; set; }

        [XmlElement("UOM_PK")]
        public int UomPK { get; set; }

        [XmlElement("SBD_PK")]
        public int StockPK { get; set; }

        [XmlElement("QTY")]
        public decimal Quantity { get; set; }
    }
}
