using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Sales
{

    [Serializable]
    [XmlRoot("ROOT")]
    public class CustomersBO
    {
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("SOH_CUSTOMER")]
        public string CustomerPK { get; set; }
        [XmlElement("SOH_DATE_FROM")]
        public DateTime FromDate { get; set; }
        [XmlElement("SOH_DATE_TO")]
        public DateTime ToDate { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("SOH_CUSTOMER_TEXT")]
        public string CustomerName { get; set; }


    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class OrderListBO
    {
        [XmlElement("SOH_PK")]
        public int Soh_PK { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        [XmlElement("PAGE_NO")]
        public int PageNo { get; set; }

    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class OrderDetailsListBO
    {
        [XmlElement("SOD_PK")]
        public int Sod_PK { get; set; }
        [XmlElement("BIZUNIT")]
        public int BizUnit { get; set; }
        //[XmlElement("PAGE_NO_ALLOCATION")]
        //public string PageNoAllocation { get; set; }
        //[XmlElement("PAGE_NO_PLANNING")]
        //public string PageNoPlanning { get; set; }
        //[XmlElement("PAGE_NO_DESPATCH")]
        //public string PageNoDespatch { get; set; }

    }

    public class Orders
    {
        private int orderID;
        private string orderName;
        private int orderStatus;

        public int OrderID
        {
            get { return this.orderID; }
            set { this.orderID = value; }
        }

        public string OrderName
        {
            get { return this.orderName; }
            set { this.orderName = value; }
        }
        public int OrderStatus
        {
            get { return this.orderStatus; }
            set { this.orderStatus = value; }
        }

        public Orders(int orderID, string orderName, int orderStatus)
        {
            this.orderID = orderID;
            this.orderName = orderName;
            this.orderStatus = orderStatus;
        }

        public Orders()
        {
        }
    }

    public class Customers
    {
        private int customerID;
        private string customerName;
        private string customerCode;
        private List<Orders> orderList =
                       new List<Orders>();

        public int CustomerID
        {
            get { return this.customerID; }
            set { this.customerID = value; }
        }

        public string CustomerCode
        {
            get { return this.customerCode; }
            set { this.customerCode = value; }
        }

        public string CustomerName
        {
            get { return this.customerName; }
            set { this.customerName = value; }
        }

        public List<Orders> OrdersList
        {
            get { return this.orderList; }
            set { this.orderList = value; }
        }
    }
}