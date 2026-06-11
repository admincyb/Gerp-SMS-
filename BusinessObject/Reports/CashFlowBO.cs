using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Reports
{
    [Serializable]
    [XmlRoot("ROOT")]
    public class CashFlowBO
    {
        [XmlElement("AS_ON_DATE")]
        public string AS_ON_DATE { get; set; }


        [XmlElement("DETAIL")]
        public List<DETAIL> DETAIL { get; set; }
         
       
    }
    [Serializable]
    public class DETAIL
    {
        [XmlElement("TYPE")]
        public string TYPE { get; set; }
        [XmlElement("PK")]
        public int PK { get; set; }
    } 


}
