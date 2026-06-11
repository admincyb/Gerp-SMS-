using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Sales
{
    [Serializable]
    [XmlRoot("ROOT")]
    public class CustomerOrderBO
    {
        [XmlElement("SOH_PK")]
        public string PK { get; set; }
        [XmlElement("ACTIVE")]
        public string Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("CUS_PK")]
        public string CustomerPK { get; set; }
        [XmlElement("SOH_DATE_FROM")]
        public string DateFrom { get; set; }
        [XmlElement("SOH_DATE_TO")]
        public string DateTo { get; set; }
        [XmlElement("PAGE_NO")]
        public string PageNo { get; set; }
        [XmlElement("SOH_NO")]
        public string SaleOrderNo { get; set; }
        [XmlElement("SOH_CUSTOMER_TEXT")]
        public string CustomerName { get; set; }

    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class CustomerOrdersBO
    {
        [XmlElement("SOH_PK")]
        public string PK { get; set; }
        [XmlElement("SOH_NO")]
        public string SaleOrderNumber { get; set; }
        [XmlElement("SOH_DATE")]
        public string SaleOrderDate { get; set; }
        [XmlElement("SOH_STATUS")]
        public string Status { get; set; }
        [XmlElement("SOH_CUSTOMER")]
        public string CustomerPK { get; set; }
        [XmlElement("SOH_REF_NO")]
        public string RefNo { get; set; }
        [XmlElement("SOH_REF_DATE")]
        public string RefDate { get; set; }
        [XmlElement("SOH_PRIORITY")]
        public string Priority { get; set; }
        [XmlElement("SOH_REMARKS")]
        public string Remarks { get; set; }
        [XmlElement("SOH_DEPT")]
        public string Department { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LastModDate { get; set; }
        [XmlElement("USER_PK")]
        public int UserPK { get; set; }


        [XmlElement("DETAILS")]
        public List<CustomerOrderDetailsBO> CustomerOrder { get; set; }
    }

    [Serializable]
    public class CustomerOrderDetailsBO
    {
        [XmlElement("DETAIL")]
        public List<CustomerOrderDetailBO> ItemsList { get; set; }
    }

    public class CustomerOrderDetailBO
    {
        [XmlElement("SOD_PK")]
        public int saleOrderPK { get; set; }
        [XmlElement("SOD_NO")]
        public string saleOrderNo { get; set; }
        [XmlElement("SOD_DATE")]
        public string saleOrderDate { get; set; }
        [XmlElement("SOD_SL_NO")]
        public string SlNo { get; set; }
        [XmlElement("SOD_ITEM")]
        public string SKU { get; set; }
        [XmlElement("SOD_PRODUCT_GROUP_TEXT")]
        public string SKUTEXT { get; set; }
        [XmlElement("SOD_PRODUCT_SIZE")]
        public string SizePK { get; set; }
        [XmlElement("SOD_PRODUCT_SIZE_TEXT")]
        public string SizeText { get; set; }
        [XmlElement("SOD_QTY")]
        public double Qty { get; set; }
        [XmlElement("SOD_UOM")]
        public string Uom { get; set; }
        [XmlElement("SOD_UOM_TEXT")]
        public string UomText { get; set; }
        [XmlElement("SOD_RATE")]
        public double Rate { get; set; }
        [XmlElement("SOD_AMOUNT")]
        public double Amount { get; set; }
        [XmlElement("SOD_REQUIRED_BY")]
        public string ReqdBy { get; set; }

    }
    [Serializable]
    [XmlRoot("ROOT")]
    public class NumberBO
    {
        [XmlElement("DOC_TYPE")]
        public int DocType { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class CustomerBO
    {
        [XmlElement("CUS_PK")]
        public string CustomerPK { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
    }


}