using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace BusinessObject.Sales
{
    [Serializable]
    [XmlRoot("ROOT")]
    public class PackingTransactionBO
    {
        [XmlElement("PKH_PK")]
        public int PK { get; set; }
        [XmlElement("PKH_DATE")]
        public string  PackingDate { get; set; }
        [XmlElement("PKH_DATE_TO")]
        public string PackingDateTo { get; set; }
        [XmlElement("PKH_DATE_FROM")]
        public string PackingDateFrom { get; set; }
        [XmlElement("PKH_SHIFT")]
        public string ShiftPK { get; set; }
        [XmlElement("PKH_NO")]
        public string Packing_NO { get; set; }
        [XmlElement("PKH_STATUS")]
        public string Status { get; set; }
        [XmlElement("PKH_REF_NO")]
        public string RefNo { get; set; }
        [XmlElement("PKH_REF_DATE")]
        public string RefDate { get; set; }
        [XmlElement("PKH_REMARKS")]
        public string Remarks { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("DEPT_PK")]
        public int DeptPK { get; set; }
        [XmlElement("USER_PK")]
        public int UserPK { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("MODULE")]
        public int Module { get; set; }
        [XmlElement("MODE")]
        public int Mode { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LastModDate { get; set; }
        [XmlElement("PAGE_NO")]
        public string PageNo { get; set; }

        [XmlElement("DETAILS")]
        public List<PackingDetailBO> PackingDetailList { get; set; }
    }
    [Serializable]
    public class PackingDetailBO
    {
        [XmlElement("DETAIL")]
        public List<PackingDetailsBO> PackingDetailsList { get; set; }
    }
    [Serializable]
    public class PackingDetailsBO
    {
        [XmlElement("PKD_PK")]
        public int PK { get; set; }
        [XmlElement("PKD_CUSTOMER")]
        public int CustomerPK { get; set; }
        [XmlElement("PKD_SL_NO")]
        public int SlNo { get; set; }
        [XmlElement("PKD_SALE_ORDER")]
        public int SaleOrderPK { get; set; }
        public string  SaleOrderText { get; set; }
        [XmlElement("PKD_SALE_ORDER_DTL")]
        public int SaleOrderDetailPK { get; set; }
        [XmlElement("PKD_ITEM")]
        public int ItemPK { get; set; }
        public string SaleOrderDetailText { get; set; }
        [XmlElement("PKD_QTY_PACKED")]
        public double QtyPacked { get; set; }
        [XmlElement("PKD_QTY_APPROVED")]
        public double QtyApproved { get; set; }
        [XmlElement("PKD_UOM")]
        public int UOM { get; set; }
        public string UomCode { get; set; }
        [XmlElement("STORE")]
        public int Store { get; set; }
        [XmlElement("PKD_REMARKS")]
        public string Remarks { get; set; }
        [XmlElement("SOD_QTY")]
        public double SaleOrderQty { get; set; }
        [XmlElement("SOD_BAL_TO_PACK")]
        public double BalToPack { get; set; }
        [XmlElement("CIM_CUSTOMER_TEXT")]
        public string CustomerName { get; set; }
    }

}