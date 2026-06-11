using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;

namespace BusinessObject.POInvoicing
{
    public class PaymentAdjn
    {
    }
    [Serializable]
    public class PaymentAdjnAllocation
    {
        [XmlElement("PAA_NO")]
        public string PAA_NO { get; set; }
        [XmlElement("PAA_TYPE")]
        public string PAA_TYPE { get; set; }
        [XmlElement("PAA_DATE")]
        public DateTime PAA_DATE { get; set; }
        [XmlElement("PAA_CRDRPK")]
        public long PAA_CRDRPK { get; set; }
        [XmlElement("PAA_TRXPK")]
        public long PAA_TRXPK { get; set; }
        [XmlElement("PAA_AMOUNT")]
        public decimal PAA_AMOUNT { get; set; }
        [XmlElement("PAA_AMOUNT_PAID")]
        public decimal PAA_AMOUNT_PAID { get; set; }
        [XmlElement("PAA_AMOUNT_BAL")]
        public decimal PAA_AMOUNT_BAL { get; set; }


    }
}
