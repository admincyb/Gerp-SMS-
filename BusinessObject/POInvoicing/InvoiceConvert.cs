using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.POInvoicing
{
    [Serializable]
    [XmlRoot("ROOT")]
    public class InvoiceConvert
    {
        [XmlElement("Invoices")]
        public List<InvoiceDetails> InvoicePKList { get; set; }
    }
    [Serializable]
    public class InvoiceDetails
    {
        [XmlElement("P_IVH_PK")]
        public int InvPK { get; set; }
    }
}
