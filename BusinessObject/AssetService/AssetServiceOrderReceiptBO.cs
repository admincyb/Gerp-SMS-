using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.AssetService
{
   public class AssetServiceOrderReceiptBO
    {
    }
   [Serializable]
   [XmlRoot("Root")]
   public class AssetServiceOrderReceiptHeader
   {
       [XmlElement("RSH_PK")]
       public int RSH_PK { get; set; }
       [XmlElement("RSH_NO")]
       public string RSH_NO { get; set; }
       [XmlElement("RSH_DATE")]
       public DateTime RSH_DATE { get; set; }
       [XmlElement("RSH_VENDOR")]
       public int RSH_VENDOR { get; set; }
       [XmlElement("RSH_VENDOR_TEXT")]
       public string RSH_VENDOR_TEXT { get; set; }
       [XmlElement("RSH_SERVICE_TYPE")]
       public int RSH_SERVICE_TYPE { get; set; }
       [XmlElement("RSH_SERVICE_TYPE_TEXT")]
       public string RSH_SERVICE_TYPE_TEXT { get; set; }
       [XmlElement("RSH_CURRENCY")]
       public int RSH_CURRENCY { get; set; }
       public string RSH_CURRENCY_TEXT { get; set; }
       [XmlElement("RSH_SUB_TOTAL")]
       public double RSH_SUB_TOTAL { get; set; }
       [XmlElement("RSH_DISCOUNT")]
       public double RSH_DISCOUNT { get; set; }
       [XmlElement("RSH_TAX")]
       public double RSH_TAX { get; set; }
       [XmlElement("RSH_OTH_CHARGE")]
       public double RSH_OTH_CHARGE { get; set; }
       [XmlElement("RSH_PRICE_ADJ")]
       public double RSH_PRICE_ADJ { get; set; }
       [XmlElement("RSH_NET_TOTAL")]
       public double RSH_NET_TOTAL { get; set; }
       [XmlElement("RSH_CURRENCY_BC")]
       public int RSH_CURRENCY_BC { get; set; }
       [XmlElement("RSH_EXCHG_RATE")]
       public double RSH_EXCHG_RATE { get; set; }
       [XmlElement("RSH_NET_TOTAL_BC")]
       public double RSH_NET_TOTAL_BC { get; set; }
       [XmlElement("RSH_STATUS")]
       public int RSH_STATUS { get; set; }
       [XmlElement("RSH_REMARKS")]
       public string RSH_REMARKS { get; set; }
       [XmlElement("RSH_ACTIVE")]
       public int RSH_ACTIVE { get; set; }
       [XmlElement("RSH_DEPT_STORE")]
       public int RSH_DEPT_STORE { get; set; }
       [XmlElement("RSH_DEPT_STORE_TEXT")]
       public string RSH_DEPT_STORE_TEXT { get; set; }    
       [XmlElement("RSH_DEPT")]
       public int RSH_DEPT { get; set; }
       [XmlElement("RSH_BIZUNIT")]
       public int RSH_BIZUNIT { get; set; }
       [XmlElement("RSH_DEL_STATUS")]
       public int RSH_DEL_STATUS { get; set; }
       [XmlElement("RSH_CRTD_BY")]
       public int RSH_CRTD_BY { get; set; }
       [XmlElement("RSH_CRTD_DT")]
       public DateTime RSH_CRTD_DT { get; set; }
       [XmlElement("RSH_MOD_BY")]
       public int RSH_MOD_BY { get; set; }
       [XmlElement("RSH_MOD_DT")]
       public DateTime RSH_MOD_DT { get; set; }

       [XmlElement("RSH_TASK1_BY")]
       public int RSH_TASK1_BY { get; set; }
       [XmlElement("RSH_TASK1_DT")]
       public DateTime RSH_TASK1_DT { get; set; }
       [XmlElement("RSH_TASK2_BY")]
       public int RSH_TASK2_BY { get; set; }
       [XmlElement("RSH_TASK2_DT")]
       public DateTime RSH_TASK2_DT { get; set; }
       [XmlElement("RSH_TASK3_BY")]
       public int RSH_TASK3_BY { get; set; }
       [XmlElement("RSH_TASK3_DT")]
       public DateTime RSH_TASK3_DT { get; set; }
       [XmlElement("RSH_STATUS_TEXT")]
       public string RSH_STATUS_TEXT { get; set; }
       [XmlElement("RSH_CSS_CLASS")]
       public string RSH_CSS_CLASS { get; set; }
       [XmlElement("RSH_INVOICE_NO")]
       public string RSH_INVOICE_NO { get; set; }
       [XmlElement("RSH_INVOICE_DATE")]
       public string RSH_INVOICE_DATE { get; set; }

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
       public List<AssetServiceOrderReceiptDetails> Details { get; set; }
       [XmlElement("Tax_HDR")]
       public List<AssetServiceOrderReceiptTaxHdr> Tax_HDR { get; set; }

   }

   [Serializable]
   public class AssetServiceOrderReceiptDetails
   {
       [XmlElement("RSD_PK")]
       public int RSD_PK { get; set; }
       [XmlElement("RSD_RSH_HDR")]
       public int RSD_RSH_HDR { get; set; }
       [XmlElement("RSD_RSH_NO")]
       public string RSD_RSH_NO { get; set; }
       [XmlElement("RSD_RSH_DATE")]
       public DateTime RSD_RSH_DATE { get; set; }
       [XmlElement("RSD_ASSET_TYPE")]
       public int RSD_ASSET_TYPE { get; set; }
       [XmlElement("RSD_ASSET_TYPE_TEXT")]
       public string RSD_ASSET_TYPE_TEXT { get; set; }
       [XmlElement("RSD_ASSET")]
       public int RSD_ASSET { get; set; }
       [XmlElement("RSD_ASSET_TEXT")]
       public string RSD_ASSET_TEXT { get; set; }
       [XmlElement("RSD_DESC")]
       public string RSD_DESC { get; set; }
       [XmlElement("RSD_AMOUNT")]
       public double RSD_AMOUNT { get; set; }
       [XmlElement("RSD_OSD_DTL")]
       public int RSD_OSD_DTL { get; set; }

       [XmlElement("RSD_SL_NO")]
       public int RSD_SL_NO { get; set; }

       [XmlElement("Item_details")]
       public List<AssetServiceOrderReceiptItemDetails> Item_details { get; set; }
   }

   [Serializable]
   public class AssetServiceOrderReceiptItemDetails
   {
       [XmlElement("RID_PK")]
       public int RID_PK { get; set; }
       [XmlElement("RID_RSD_PK")]
       public int RID_RSD_PK { get; set; }
       [XmlElement("RID_ASR_ITEM")]
       public int RID_ASR_ITEM { get; set; }
       [XmlElement("RID_ASR_ITEM_TEXT")]
       public string RID_ASR_ITEM_TEXT { get; set; }
       [XmlElement("RID_DESC")]
       public string RID_DESC { get; set; }
       [XmlElement("RID_QTY")]
       public double RID_QTY { get; set; }
       [XmlElement("RID_UOM")]
       public int RID_UOM { get; set; }
       [XmlElement("RID_UOM_TEXT")]
       public string RID_UOM_TEXT { get; set; }
       [XmlElement("RID_RATE")]
       public double RID_RATE { get; set; }
       [XmlElement("RID_TAX")]
       public double RID_TAX { get; set; }
       [XmlElement("RID_DISCOUNT")]
       public double RID_DISCOUNT { get; set; }
       [XmlElement("RID_AMOUNT")]
       public double RID_AMOUNT { get; set; }
       [XmlElement("RID_NET_AMOUNT")]
       public double RID_NET_AMOUNT { get; set; }
       [XmlElement("RID_REMARKS")]
       public string RID_REMARKS { get; set; }
       [XmlElement("RID_OID_DTL")]
       public int RID_OID_DTL { get; set; }

       [XmlElement("RID_SL_NO")]
       public int RID_SL_NO { get; set; }
       [XmlElement("RID_ITEM_SL_NO")]
       public int RID_ITEM_SL_NO { get; set; }

       [XmlElement("Tax_DTL")]
       public List<AssetServiceOrderReceiptTaxHdr> Tax_DTL { get; set; }
   }
   [Serializable]
   public class AssetServiceOrderReceiptTaxHdr
   {
       [XmlElement("TRD_PK")]
       public int TRD_PK { get; set; }
       [XmlElement("TRD_RID_PK")]
       public int TRD_RID_PK { get; set; }
       [XmlElement("TRD_TYPE")]
       public short TRD_TYPE { get; set; }
       [XmlElement("TRD_TAX")]
       public int TRD_TAX { get; set; }
       [XmlElement("TRD_TAX_TEXT")]
       public string TRD_TAX_TEXT { get; set; }
       [XmlElement("TRD_TAX_CATEGORY")]
       public int TRD_TAX_CATEGORY { get; set; }
       [XmlElement("TRD_TAX_CATEGORY_TEXT")]
       public string TRD_TAX_CATEGORY_TEXT { get; set; }
       [XmlElement("TRD_NAME")]
       public string TRD_NAME { get; set; }
       [XmlElement("TRD_TAX_AMT")]
       public double TRD_TAX_AMT { get; set; }

       [XmlElement("TRD_TAX_FORMULA")]
       public string TRD_TAX_FORMULA { get; set; }
       [XmlElement("TRD_SL_NO")]
       public int TRD_SL_NO { get; set; }

       [XmlElement("TRD_ITEM_SL_NO")]
       public int TRD_ITEM_SL_NO { get; set; }
   }
   //For Pending SR  
   [Serializable]
   public class PendingServiceOrder
   {
       public bool AddedToStockList { get; set; }
       public bool CheckBoxChecked { get; set; }

       public int OSD_PK { get; set; }
       public int OSD_OSH_HDR { get; set; }
       public string OSD_OSH_NO { get; set; }
       public DateTime OSH_DATE { get; set; }
       public int OSD_ASSET_TYPE { get; set; }
       public string OSD_ASSET_TYPE_TEXT { get; set; }
       public int OSD_ASSET { get; set; }
       public string OSD_ASSET_TEXT { get; set; }
       public string OSD_DESC { get; set; }
       public decimal OSD_AMOUNT { get; set; }
       public int OSD_ACTIVE { get; set; }
       public int OSD_SL_NO { get; set; }

       public int OSH_CURRENCY { get; set; }
       public string OSH_CURRENCY_TEXT { get; set; }
       public int OSH_DEPT { get; set; }
       public int OSH_DEPT_STORE { get; set; }
       public int OSH_VENDOR { get; set; }
       public string OSH_VENDOR_TEXT { get; set; }
       public int OSH_SERVICE_TYPE { get; set; }
       public string OSH_SERVICE_TYPE_TEXT { get; set; }

       public int RSD_PK { get; set; }        


   }

   [Serializable]
   [XmlRoot("Root")]
   public class SelectedServiceOrderRoot
   {
       [XmlElement("Details")]
       public List<SelectedServiceOrdrDtls> OSDPKList { get; set; }
   }

   [Serializable]
   public class SelectedServiceOrdrDtls
   {
       [XmlElement("OSH_PK")]
       public int OSH_PK { get; set; }
       public int OSD_PK { get; set; }
       public int OSH_CURRENCY { get; set; }
       public int RSD_PK { get; set; }
   }


   #region Multiple Service Request
   [Serializable]
   [XmlRoot("Root")]
   public class MultipleSOHeader
   {
       [XmlElement("OS")]
       public List<AssetServiceOrderReceiptHeader> MultipleOSList { get; set; }
   }
   #endregion
}
