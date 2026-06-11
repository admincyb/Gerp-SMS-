using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
    public class BadDebitBO
    {
        [XmlElement("BDDetails")]
        public List<BDDetails> BadDebitDetails { get; set; }
    }
    [Serializable]  
    public class BDDetails
    {
        [XmlElement("IBD_PK")]
        public int BadDebitPk { get; set; }
        [XmlElement("IBD_INVOICE_HDR")]
        public int InvoicePk { get; set; }
        [XmlElement("IBD_OUT_TC")]
        public double OutstandingAmount { get; set; }
        [XmlElement("IBD_OUT_CLAIM")]
        public double OutAmountClaim { get; set; }
        [XmlElement("IBD_BAD_DEBIT_CLAIM")]
        public double BadDebitReliefClaim { get; set; }

        [XmlElement("IBD_STATUS")]
        public int STATUS { get; set; }
        [XmlElement("IBD_ACTIVE")]
        public byte Active { get; set; }
        [XmlElement("IBD_BIZUNIT")]
        public int Bizunit { get; set; }
        [XmlElement("USER_PK")]
        public short USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
    }

}
