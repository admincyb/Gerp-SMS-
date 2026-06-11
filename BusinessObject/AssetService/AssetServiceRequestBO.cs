using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.AssetService
{
    public class AssetServiceRequestBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class AssetServiceRequestHeader
    {        
        [XmlElement("SRH_PK")]
        public int SRH_PK { get; set; }
        [XmlElement("SRH_NO")]
        public string SRH_NO { get; set; }
        [XmlElement("SRH_DATE")]
        public DateTime SRH_DATE { get; set; }
        [XmlElement("SRH_REF_NO")]
        public string SRH_REF_NO { get; set; }
        [XmlElement("SRH_REF_DATE")]
        public string SRH_REF_DATE { get; set; }  
        [XmlElement("SRH_VENDOR")]
        public int SRH_VENDOR { get; set; }
        [XmlElement("SRH_VENDOR_TEXT")]
        public string SRH_VENDOR_TEXT { get; set; }
        [XmlElement("SRH_SERVICE_TYPE")]
        public int SRH_SERVICE_TYPE { get; set; }
        [XmlElement("SRH_SERVICE_TYPE_TEXT")]
        public string SRH_SERVICE_TYPE_TEXT { get; set; }
        [XmlElement("SRH_STATUS")]
        public int SRH_STATUS { get; set; }
        [XmlElement("SRH_REMARKS")]
        public string SRH_REMARKS { get; set; }
        [XmlElement("SRH_ACTIVE")]
        public int SRH_ACTIVE { get; set; }
        [XmlElement("SRH_DEPT_STORE")]
        public int SRH_DEPT_STORE { get; set; }
        [XmlElement("SRH_DEPT_STORE_TEXT")]
        public string SRH_DEPT_STORE_TEXT { get; set; }        
        [XmlElement("SRH_DEPT")]
        public int SRH_DEPT { get; set; }
        [XmlElement("SRH_CURRENCY")]
        public int SRH_CURRENCY { get; set; }
        public string SRH_CURRENCY_TEXT { get; set; }   
        [XmlElement("SRH_BIZUNIT")]
        public int SRH_BIZUNIT { get; set; }
        [XmlElement("SRH_DEL_STATUS")]
        public int SRH_DEL_STATUS { get; set; }
        [XmlElement("SRH_CRTD_BY")]
        public int SRH_CRTD_BY { get; set; }
        [XmlElement("SRH_CRTD_DT")]
        public DateTime SRH_CRTD_DT { get; set; }
        [XmlElement("SRH_MOD_BY")]
        public int SRH_MOD_BY { get; set; }
        [XmlElement("SRH_MOD_DT")]
        public DateTime SRH_MOD_DT { get; set; }
      
        [XmlElement("SRH_TASK1_BY")]
        public int SRH_TASK1_BY { get; set; }
        [XmlElement("SRH_TASK1_DT")]
        public DateTime SRH_TASK1_DT { get; set; }
        [XmlElement("SRH_TASK2_BY")]
        public int SRH_TASK2_BY { get; set; }
        [XmlElement("SRH_TASK2_DT")]
        public DateTime SRH_TASK2_DT { get; set; }
        [XmlElement("SRH_TASK3_BY")]
        public int SRH_TASK3_BY { get; set; }
        [XmlElement("SRH_TASK3_DT")]
        public DateTime SRH_TASK3_DT { get; set; }
        [XmlElement("SRH_STATUS_TEXT")]
        public string SRH_STATUS_TEXT { get; set; }
        [XmlElement("SRH_CSS_CLASS")]
        public string SRH_CSS_CLASS { get; set; }
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
        public List<AssetServiceRequestDetails> Details { get; set; }

    }

    [Serializable]
    public class AssetServiceRequestDetails
    {
        [XmlElement("SRD_PK")]
        public int SRD_PK { get; set; }
        [XmlElement("SRD_SRH_HDR")]
        public int SRD_SRH_HDR { get; set; }
        [XmlElement("SRD_SRH_NO")]
        public string SRD_SRH_NO { get; set; }
        [XmlElement("SRD_SRH_DATE")]
        public DateTime SRD_SRH_DATE { get; set; }
        [XmlElement("SRD_ASSET_TYPE")]
        public int SRD_ASSET_TYPE { get; set; }
        [XmlElement("SRD_ASSET_TYPE_TEXT")]
        public string SRD_ASSET_TYPE_TEXT { get; set; }
        [XmlElement("SRD_ASSET")]
        public int SRD_ASSET { get; set; }
        [XmlElement("SRD_ASSET_TEXT")]
        public string SRD_ASSET_TEXT { get; set; }
        [XmlElement("SRD_DESC")]
        public string SRD_DESC { get; set; }
        [XmlElement("SRD_AMOUNT")]
        public double SRD_AMOUNT { get; set; }
        [XmlElement("SRD_ACTIVE")]
        public int SRD_ACTIVE { get; set; }

        [XmlElement("SRD_SL_NO")]
        public int SRD_SL_NO { get; set; }
       
        [XmlElement("Item_details")]
        public List<AssetServiceRequestItemDetails> Item_details { get; set; }
    }

    [Serializable]
    public class AssetServiceRequestItemDetails
    {
        [XmlElement("SID_PK")]
        public int SID_PK { get; set; }
        [XmlElement("SID_SRD_PK")]
        public int SID_SRD_PK { get; set; }
        [XmlElement("SID_ASR_ITEM")]
        public int SID_ASR_ITEM { get; set; }
        [XmlElement("SID_ASR_ITEM_TEXT")]
        public string SID_ASR_ITEM_TEXT { get; set; }
        [XmlElement("SID_DESC")]
        public string SID_DESC { get; set; }
        [XmlElement("SID_QTY")]
        public double SID_QTY { get; set; }
        [XmlElement("SID_UOM")]
        public int SID_UOM { get; set; }
        [XmlElement("SID_UOM_TEXT")]
        public string SID_UOM_TEXT { get; set; }
        [XmlElement("SID_RATE")]
        public double SID_RATE { get; set; }
        [XmlElement("SID_AMOUNT")]
        public double SID_AMOUNT { get; set; }
        [XmlElement("SID_REMARKS")]
        public string SID_REMARKS { get; set; }
        [XmlElement("SID_ACTIVE")]
        public int SID_ACTIVE { get; set; }

        [XmlElement("SID_SL_NO")]
        public int SID_SL_NO { get; set; }
    }
}
