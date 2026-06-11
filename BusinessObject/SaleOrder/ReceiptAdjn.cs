using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.SaleOrder
{
    class ReceiptAdjn
    {
    }

    [Serializable]
    public class ReceiptAdjnAllocation
    {
        [XmlElement("RAA_NO")]
        public string RAA_NO { get; set; }
        [XmlElement("RAA_TYPE")]
        public string RAA_TYPE { get; set; }
        [XmlElement("RAA_DATE")]
        public DateTime RAA_DATE { get; set; }
        [XmlElement("RAA_CRDRPK")]
        public long RAA_CRDRPK { get; set; }
        [XmlElement("RAA_TRXPK")]
        public long RAA_TRXPK { get; set; }
        [XmlElement("RAA_AMOUNT")]
        public decimal RAA_AMOUNT { get; set; }
        [XmlElement("RAA_AMOUNT_RCVD")]
        public decimal RAA_AMOUNT_RCVD { get; set; }
        [XmlElement("RAA_AMOUNT_BAL")]
        public decimal RAA_AMOUNT_BAL { get; set; }
        [XmlElement("RAA_INV_NO")]
        public decimal RAA_INV_NO { get; set; }
    }

}
