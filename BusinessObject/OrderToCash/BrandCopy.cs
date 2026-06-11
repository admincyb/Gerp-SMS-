using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.OrderToCash
{
    class BrandCopy
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class BrandCopyHeader
    {
        [XmlElement("CUS_PK")]
        public int CUS_PK { get; set; }
        [XmlElement("USER_PK")]
        public short USER_PK { get; set; }
        [XmlElement("Detail")]
        public List<BrandCopyDetails> Details { get; set; }
    }

    [Serializable]
    public class BrandCopyDetails
    {
        [XmlElement("CIM_CUSTOMER")]
        public int CIM_CUSTOMER { get; set; }        
    }
}
