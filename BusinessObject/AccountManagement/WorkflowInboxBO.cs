using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.AccountManagement
{
    public class WorkflowInboxBO
    {
        public int RefID { get; set; }
    }
    [Serializable]
    [XmlRoot("Root")]
    public class SummaryInfo
    {
        [XmlElement("Fields")]
        public List<SummaryFields> Fields { get; set; }
    }
    [Serializable]
    public class SummaryFields
    {
        [XmlElement("Label")]
        public string Label { get; set; }
        [XmlElement("Value")]
        public string Value { get; set; }
    }
}
