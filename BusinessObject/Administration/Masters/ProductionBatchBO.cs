using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Masters
{
    [Serializable]
    [XmlRoot("Root")]
    public class ProductionBatchBO
    {
        [XmlElement("PBN_PK")]
        public int PBN_PK { get; set; }
        [XmlElement("PBN_BATCH_NO")]
        public string PBN_BATCH_NO { get; set; }

        [XmlElement("PBN_DATE")]
        public DateTime PBN_DATE { get; set; }
        [XmlElement("PBN_USER_PK")]
        public int PBN_USER_PK { get; set; }
        [XmlElement("PBN_BIZUNIT")]
        public int PBN_BIZUNIT { get; set; }
        [XmlElement("PBN_IS_ACTIVE")]
        public int PBN_IS_ACTIVE { get; set; }
        [XmlElement("PBN_BATCH_DESC")]
        public string PBN_BATCH_DESC { get; set; }
        [XmlElement("PBN_MOD_DT")]
        public DateTime PBN_MOD_DT { get; set; }
        

    }
}
