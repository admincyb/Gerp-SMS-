using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.AssetService
{
    public class AssetServiceOrderBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class AssetServiceOrderHeader
    {
        [XmlElement("OSH_PK")]
        public int OSH_PK { get; set; }
        [XmlElement("OSH_NO")]
        public string OSH_NO { get; set; }
        [XmlElement("OSH_DATE")]
        public DateTime OSH_DATE { get; set; }
        [XmlElement("OSH_VENDOR")]
        public int OSH_VENDOR { get; set; }
        [XmlElement("OSH_VENDOR_TEXT")]
        public string OSH_VENDOR_TEXT { get; set; }
        [XmlElement("OSH_SERVICE_TYPE")]
        public int OSH_SERVICE_TYPE { get; set; }
        [XmlElement("OSH_SERVICE_TYPE_TEXT")]
        public string OSH_SERVICE_TYPE_TEXT { get; set; }
        [XmlElement("OSH_CURRENCY")]
        public int OSH_CURRENCY { get; set; }
        public string OSH_CURRENCY_TEXT { get; set; }
        [XmlElement("OSH_SUB_TOTAL")]
        public double OSH_SUB_TOTAL { get; set; }
        [XmlElement("OSH_DISCOUNT")]
        public double OSH_DISCOUNT { get; set; }
        [XmlElement("OSH_TAX")]
        public double OSH_TAX { get; set; }
        [XmlElement("OSH_OTH_CHARGE")]
        public double OSH_OTH_CHARGE { get; set; }
        [XmlElement("OSH_PRICE_ADJ")]
        public double OSH_PRICE_ADJ { get; set; }
        [XmlElement("OSH_NET_TOTAL")]
        public double OSH_NET_TOTAL { get; set; }
        [XmlElement("OSH_CURRENCY_BC")]
        public int OSH_CURRENCY_BC { get; set; }
        [XmlElement("OSH_EXCHG_RATE")]
        public double OSH_EXCHG_RATE { get; set; }
        [XmlElement("OSH_NET_TOTAL_BC")]
        public double OSH_NET_TOTAL_BC { get; set; } 
        [XmlElement("OSH_STATUS")]
        public int OSH_STATUS { get; set; }
        [XmlElement("OSH_REMARKS")]
        public string OSH_REMARKS { get; set; }
        [XmlElement("OSH_ACTIVE")]
        public int OSH_ACTIVE { get; set; }
        [XmlElement("OSH_DEPT_STORE")]
        public int OSH_DEPT_STORE { get; set; }
        [XmlElement("OSH_DEPT_STORE_TEXT")]
        public string OSH_DEPT_STORE_TEXT { get; set; }    
        [XmlElement("OSH_DEPT")]
        public int OSH_DEPT { get; set; }      
        [XmlElement("OSH_BIZUNIT")]
        public int OSH_BIZUNIT { get; set; }
        [XmlElement("OSH_DEL_STATUS")]
        public int OSH_DEL_STATUS { get; set; }
        [XmlElement("OSH_CRTD_BY")]
        public int OSH_CRTD_BY { get; set; }
        [XmlElement("OSH_CRTD_DT")]
        public DateTime OSH_CRTD_DT { get; set; }
        [XmlElement("OSH_MOD_BY")]
        public int OSH_MOD_BY { get; set; }
        [XmlElement("OSH_MOD_DT")]
        public DateTime OSH_MOD_DT { get; set; }

        [XmlElement("OSH_TASK1_BY")]
        public int OSH_TASK1_BY { get; set; }
        [XmlElement("OSH_TASK1_DT")]
        public DateTime OSH_TASK1_DT { get; set; }
        [XmlElement("OSH_TASK2_BY")]
        public int OSH_TASK2_BY { get; set; }
        [XmlElement("OSH_TASK2_DT")]
        public DateTime OSH_TASK2_DT { get; set; }
        [XmlElement("OSH_TASK3_BY")]
        public int OSH_TASK3_BY { get; set; }
        [XmlElement("OSH_TASK3_DT")]
        public DateTime OSH_TASK3_DT { get; set; }
        [XmlElement("OSH_STATUS_TEXT")]
        public string OSH_STATUS_TEXT { get; set; }
        [XmlElement("OSH_CSS_CLASS")]
        public string OSH_CSS_CLASS { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

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


        [XmlElement("Details")]
        public List<AssetServiceOrderDetails> Details { get; set; }
        [XmlElement("Tax_HDR")]
        public List<AssetServiceOrderTaxHdr> Tax_HDR { get; set; }

    }

    [Serializable]
    public class AssetServiceOrderDetails
    {
        [XmlElement("OSD_PK")]
        public int OSD_PK { get; set; }
        [XmlElement("OSD_OSH_HDR")]
        public int OSD_OSH_HDR { get; set; }
        [XmlElement("OSD_OSH_NO")]
        public string OSD_OSH_NO { get; set; }
        [XmlElement("OSD_OSH_DATE")]
        public DateTime OSD_OSH_DATE { get; set; }
        [XmlElement("OSD_ASSET_TYPE")]
        public int OSD_ASSET_TYPE { get; set; }
        [XmlElement("OSD_ASSET_TYPE_TEXT")]
        public string OSD_ASSET_TYPE_TEXT { get; set; }
        [XmlElement("OSD_ASSET")]
        public int OSD_ASSET { get; set; }
        [XmlElement("OSD_ASSET_TEXT")]
        public string OSD_ASSET_TEXT { get; set; }
        [XmlElement("OSD_DESC")]
        public string OSD_DESC { get; set; }
        [XmlElement("OSD_AMOUNT")]
        public double OSD_AMOUNT { get; set; }
        [XmlElement("OSD_SRD_DTL")]
        public int OSD_SRD_DTL { get; set; }
        
        [XmlElement("OSD_SL_NO")]
        public int OSD_SL_NO { get; set; }

        [XmlElement("Item_details")]
        public List<AssetServiceOrderItemDetails> Item_details { get; set; }
    }

    [Serializable]
    public class AssetServiceOrderItemDetails
    {
        [XmlElement("OID_PK")]
        public int OID_PK { get; set; }
        [XmlElement("OID_OSD_PK")]
        public int OID_OSD_PK { get; set; }
        [XmlElement("OID_ASR_ITEM")]
        public int OID_ASR_ITEM { get; set; }
        [XmlElement("OID_ASR_ITEM_TEXT")]
        public string OID_ASR_ITEM_TEXT { get; set; }
        [XmlElement("OID_DESC")]
        public string OID_DESC { get; set; }
        [XmlElement("OID_QTY")]
        public double OID_QTY { get; set; }
        [XmlElement("OID_UOM")]
        public int OID_UOM { get; set; }
        [XmlElement("OID_UOM_TEXT")]
        public string OID_UOM_TEXT { get; set; }
        [XmlElement("OID_RATE")]
        public double OID_RATE { get; set; }    
        [XmlElement("OID_TAX")]
        public double OID_TAX { get; set; }
        [XmlElement("OID_DISCOUNT")]
        public double OID_DISCOUNT { get; set; }    
        [XmlElement("OID_AMOUNT")]
        public double OID_AMOUNT { get; set; }
        [XmlElement("OID_NET_AMOUNT")]
        public double OID_NET_AMOUNT { get; set; }
        [XmlElement("OID_REMARKS")]
        public string OID_REMARKS { get; set; }
        [XmlElement("OID_SID_DTL")]
        public int OID_SID_DTL { get; set; }

        [XmlElement("OID_SL_NO")]
        public int OID_SL_NO { get; set; }
        [XmlElement("OID_ITEM_SL_NO")]
        public int OID_ITEM_SL_NO { get; set; }

        [XmlElement("Tax_DTL")]
        public List<AssetServiceOrderTaxHdr> Tax_DTL { get; set; }
    }
    [Serializable]
    public class AssetServiceOrderTaxHdr
    {
        [XmlElement("SSD_PK")]
        public int SSD_PK { get; set; }
        [XmlElement("SSD_OID_PK")]
        public int SSD_OID_PK { get; set; }
        [XmlElement("SSD_TYPE")]
        public short SSD_TYPE { get; set; }
        [XmlElement("SSD_TAX")]
        public int SSD_TAX { get; set; }
        [XmlElement("SSD_TAX_TEXT")]
        public string SSD_TAX_TEXT { get; set; }      
        [XmlElement("SSD_TAX_CATEGORY")]
        public int SSD_TAX_CATEGORY { get; set; }
        [XmlElement("SSD_TAX_CATEGORY_TEXT")]
        public string SSD_TAX_CATEGORY_TEXT { get; set; }     
        [XmlElement("SSD_NAME")]
        public string SSD_NAME { get; set; }
        [XmlElement("SSD_TAX_AMT")]
        public double SSD_TAX_AMT { get; set; }

        [XmlElement("SSD_TAX_FORMULA")]
        public string SSD_TAX_FORMULA { get; set; }
        [XmlElement("SSD_SL_NO")]
        public int SSD_SL_NO { get; set; }

        [XmlElement("SSD_ITEM_SL_NO")]
        public int SSD_ITEM_SL_NO { get; set; }
    }
    //For Pending SR  
    [Serializable]
    public class PendingServiceRequest
    {       
        public bool AddedToStockList { get; set; }
        public bool CheckBoxChecked { get; set; }

        public int SRD_PK { get; set; }     
        public int SRD_SRH_HDR { get; set; }     
        public string SRD_SRH_NO { get; set; }    
        public DateTime SRD_SRH_DATE { get; set; }       
        public int SRD_ASSET_TYPE { get; set; }       
        public string SRD_ASSET_TYPE_TEXT { get; set; }       
        public int SRD_ASSET { get; set; }       
        public string SRD_ASSET_TEXT { get; set; }       
        public string SRD_DESC { get; set; }
        public decimal SRD_AMOUNT { get; set; }   
        public int SRD_ACTIVE { get; set; }     
        public int SRD_SL_NO { get; set; }

        public int SRH_CURRENCY { get; set; }
        public string SRH_CURRENCY_TEXT { get; set; }
        public int SRH_DEPT { get; set; }
        public int SRH_DEPT_STORE { get; set; }
        public int SRH_VENDOR { get; set; }
        public string SRH_VENDOR_TEXT { get; set; }
        public int SRH_SERVICE_TYPE { get; set; }
        public string SRH_SERVICE_TYPE_TEXT { get; set; }

        public int OSD_PK { get; set; }        

    }

    [Serializable]
    [XmlRoot("Root")]
    public class SelectedServiceRequestRoot
    {
        [XmlElement("Details")]
        public List<SelectedServiceReqDtls> SRDPKList { get; set; }
    }

    [Serializable]
    public class SelectedServiceReqDtls
    {
        [XmlElement("SRH_PK")]
        public int SRH_PK { get; set; }       
        public int SRD_PK { get; set; }
        public int SRH_CURRENCY { get; set; }
        public int OSD_PK { get; set; }
    }

    
    #region Multiple Service Request
    [Serializable]
    [XmlRoot("Root")]
    public class MultipleSRHeader
    {
        [XmlElement("SR")]
        public List<AssetServiceOrderHeader> MultipleSRList { get; set; }
    }
    #endregion
}
