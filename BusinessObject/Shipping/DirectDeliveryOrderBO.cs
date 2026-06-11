using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Shipping
{
   public class DirectDeliveryOrderBO
    {
    }


   [Serializable]
   [XmlRoot("Root")]
   public class DespatchBO
   {
       [XmlElement("DPH_PK")]
       public int DPH_PK { get; set; }
       [XmlElement("USER_PK")]
       public int USER_PK { get; set; }
       [XmlElement("DPD_PK")]
       public int DPD_PK { get; set; }
       [XmlElement("CartonDetail")]
       public List<CartonDetails> lstCartonDetail { get; set; }
   }
   [Serializable]
   public class CartonDetails
   {
       [XmlElement("DSC_PK")]
       public int DSC_PK { get; set; }
       [XmlElement("DSC_CARTON_MST")]
       public int DSC_CARTON_MST { get; set; }
       [XmlElement("DSC_QTY_DESPATCHED")]
       public decimal DSC_QTY_DESPATCHED { get; set; }
       public string DSC_CARTON_NO { get; set; }
       [XmlElement("SL_NO")]
       public int SL_NO { get; set; }
       public string DSC_LOCATION_TEXT { get; set; }
   }
   [Serializable]
   [XmlRoot("Root")]
   public class DirectDeliveryOrderHeader : WorkflowBO
   {
       [XmlElement("DPH_PK")]
       public int DPH_PK { get; set; }
       [XmlElement("DPH_NO")]
       public string DPH_NO { get; set; }
       [XmlElement("DPH_VERSION")]
       public string DPH_VERSION { get; set; }
       [XmlElement("DPH_DATE")]
       public DateTime DPH_DATE { get; set; }
       [XmlElement("DPH_CUSTOMER")]
       public int DPH_CUSTOMER { get; set; }
       [XmlElement("DPH_REF_NO")]
       public string DPH_REF_NO { get; set; }
       [XmlElement("DPH_STATUS")]
       public int DPH_STATUS { get; set; }
       [XmlElement("DPH_STATUS_TEXT")]
       public string DPH_STATUS_TEXT { get; set; }
       [XmlElement("ASC_CSS_CLASS")]
       public string ASC_CSS_CLASS { get; set; }
       
       [XmlElement("DPH_TOTAL_QTY")]
       public decimal DPH_TOTAL_QTY { get; set; }
       [XmlElement("DPH_ETD")]
       public DateTime DPH_ETD { get; set; }
       [XmlElement("DPH_SHIPMENT_DATE")]
       public string DPH_SHIPMENT_DATE { get; set; }
       [XmlElement("DPH_REMARKS")]
       public string DPH_REMARKS { get; set; }      

       [XmlElement("DPH_DEPT")]
       public int DPH_DEPT { get; set; }
       [XmlElement("DPH_COMPANY")]
       public int DPH_COMPANY { get; set; }
       [XmlElement("DPH_BIZUNIT")]
       public int DPH_BIZUNIT { get; set; }

       [XmlElement("DPH_CRTD_BY")]
       public int DPH_CRTD_BY { get; set; }
       [XmlElement("DPH_CRTD_DT")]
       public DateTime DPH_CRTD_DT { get; set; }
       [XmlElement("DPH_MOD_BY")]
       public int DPH_MOD_BY { get; set; }
       [XmlElement("DPH_MOD_DT")]
       public DateTime DPH_MOD_DT { get; set; }
       [XmlElement("DPH_CUSTOMER_NAME")]
       public string DPH_CUSTOMER_NAME { get; set; }
       [XmlElement("DPH_CUSTOMER_ADDRESS")]
       public string DPH_CUSTOMER_ADDRESS { get; set; }
       [XmlElement("DPH_DEL_STATUS")]
       public string DPH_DEL_STATUS { get; set; }
       [XmlElement("DPH_TRX_TYPE")]
       public string DPH_TRX_TYPE { get; set; }

       public string DPH_CUSTOMER_TEXT { get; set; }

       [XmlElement("DPH_ACTIVE")]
       public byte DPH_ACTIVE { get; set; }
       [XmlElement("USER_PK")]
       public int USER_PK { get; set; }
       [XmlElement("LAST_MOD_DT")]
       public DateTime LAST_MOD_DT { get; set; }

       [XmlElement("OrderDetail")]
       public List<DirectDeliveryOrderDetails> DirectDODetailsList { get; set; }

       [XmlElement("APT_CODE")]
       public string APT_CODE { get; set; }
       [XmlElement("AST_DOC_MODE")]
       public int AST_DOC_MODE { get; set; }
       [XmlElement("WKF_FLAG")]
       public int WKF_FLAG { get; set; }

       public int IsContinueWithDOQty { get; set; }     

   }

   [Serializable]
   public class DirectDeliveryOrderDetails
   {
       [XmlElement("DPD_PK")]
       public int DPD_PK { get; set; }
       [XmlElement("DPD_NO")]
       public string DPD_NO { get; set; }
       [XmlElement("DPD_DATE")]
       public DateTime DPD_DATE { get; set; }
       [XmlElement("DPD_VERSION")]
       public string DPD_VERSION { get; set; }
       [XmlElement("DPD_DESPATCH_HDR")]
       public int DPD_DESPATCH_HDR { get; set; }
       [XmlElement("DPD_SL_NO")]
       public int DPD_SL_NO { get; set; }
       [XmlElement("DPD_SALE_ORDER")]
       public int DPD_SALE_ORDER { get; set; }
       [XmlElement("DPD_SO_DTL")]
       public int DPD_SO_DTL { get; set; }
       [XmlElement("DPD_CUST_ITEM")]
       public int DPD_CUST_ITEM { get; set; }
       [XmlElement("DPD_QTY_DESPATCHED")]
       public decimal DPD_QTY_DESPATCHED { get; set; }
       [XmlElement("DPD_QTY_APPROVED")]
       public decimal DPD_QTY_APPROVED { get; set; }
       [XmlElement("DPD_ITEM")]
       public int DPD_ITEM { get; set; }
       [XmlElement("DPD_UOM")]
       public int DPD_UOM { get; set; }
       [XmlElement("DPD_SALE_QTY")]
       public decimal DPD_SALE_QTY { get; set; }      
       [XmlElement("DPD_SALE_UOM")]
       public int DPD_SALE_UOM { get; set; }
       [XmlElement("DPD_SALE_UOM_CONV")]
       public decimal DPD_SALE_UOM_CONV { get; set; }
       [XmlElement("DPD_REMARKS")]
       public string DPD_REMARKS { get; set; }
       //[XmlElement("DPD_DEPT")]
       //public int GRD_DEPT { get; set; }
       [XmlElement("DPD_BIZUNIT")]
       public int DPD_BIZUNIT { get; set; }
       [XmlElement("DPD_NET_WT")]
       public decimal DPD_NET_WT { get; set; }
       [XmlElement("DPD_GROSS_WT")]
       public decimal DPD_GROSS_WT { get; set; }
       [XmlElement("DPD_ITEM_CAT_VALUE")]
       public int ProductType { get; set; }

       public string SONumber { get; set; }
       public DateTime SODate { get; set; }
       public int ItemCategoryId { get; set; }
       public int ItemId { get; set; }
       public string ItemName { get; set; } 
       public decimal PendingQty { get; set; }
       public decimal SORate { get; set; }
       public decimal DPD_PRE_DELIVERED_QTY { get; set; }    
       public decimal DPD_BALANCE_QTY_TO_DELIVER { get; set; }
       public byte ITM_NEED_BATCH_STK { get; set; }
       public decimal DPD_CURRENT_STOCK { get; set; }
       public string DPD_SALE_UOM_TEXT { get; set; }
     

       [XmlElement("DespatchStk")]
       public List<DirectDeliveryOrderStockBatchDetails> DOStockBatch_Details { get; set; }
   }

   [Serializable]
   public class DirectDeliveryOrderStockBatchDetails
   {    
       [XmlElement("SIC_PK")]
       public int SIC_PK { get; set; }
       [XmlElement("SIC_DESPATCH_DTL")]
       public int SIC_DESPATCH_DTL { get; set; }     
       [XmlElement("SIC_QTY_CONSUMED")]
       public decimal SIC_QTY_CONSUMED { get; set; }
       [XmlElement("SIC_STK_BATCH")]
       public int SIC_STK_BATCH { get; set; }
       [XmlElement("SIC_SL_NO")]
       public int SIC_SL_NO { get; set; }
       [XmlElement("SIC_ITEM_SL_NO")]
       public int SIC_ITEM_SL_NO { get; set; }

       public string BatchName { get; set; }
       public decimal BatchStock { get; set; }      
       
   }

   [Serializable]
   public class PendingSO
   {
       public int sohPK { get; set; }
       public bool AddedToStockList { get; set; }
       public bool CheckBoxChecked { get; set; }
       public int SODetId { get; set; }


       public int SOId { get; set; }
       public string SONumber { get; set; }
       public DateTime SODate { get; set; }
       public int ItemCatId { get; set; }
       public int ItemId { get; set; }
       public string ItemName { get; set; }      
       public int UOMId { get; set; }
       public string UOM { get; set; }
       public decimal SOQty { get; set; }
       public decimal SOQtyApproved { get; set; }
       public decimal PreDeliveredQty { get; set; }
       public decimal SOQtyAllocated { get; set; }
       public decimal BalanceQtyToDeliver { get; set; }
       public decimal SORate { get; set; }
       public int SOH_CURRENCY { get; set; }
       public byte ITM_NEED_BATCH_STK { get; set; }
       public byte ProductType { get; set; }
       public string ProductTypeCode { get; set; }
       public string ProductTypeText { get; set; }
      

   }

   [Serializable]
   public class SelectedSalesOrderDtls
   {
       [XmlElement("SOH_PK")]
       public int SOH_PK { get; set; }
       public int SOD_PK { get; set; }
       public int SOH_CURRENCY { get; set; }
       public int DPH_PK { get; set; }
   }

   [Serializable]
   [XmlRoot("ROOT")]
   public class SOHeaderBO
   {
       [XmlElement("SO")]
       public List<SOHeaderListBO> SOList { get; set; }
   }

   [Serializable]
   public class SOHeaderListBO
   {
       [XmlElement("SOH_PK")]
       public int SOH_PK { get; set; }     
   }
}
