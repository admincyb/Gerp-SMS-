using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject
{
  [Serializable]
   public class WorkflowBO
    {
        [XmlElement("WKF_REFERENCE")]
        public int WKF_REFERENCE { get; set; }
        [XmlElement("WKF_APPLICATION")]
        public int WKF_APPLICATION { get; set; }
        [XmlElement("WKF_PROCESS")]
        public int WKF_PROCESS { get; set; }
        [XmlElement("WKF_TASK")]
        public int WKF_TASK { get; set; }
        [XmlElement("WKF_TASK_ACTION")]
        public int WKF_TASK_ACTION { get; set; }
        [XmlElement("WKF_COMMENTS")]
        public string WKF_COMMENTS { get; set; }
        [XmlElement("WKF_TRX_FLAG")]
        public int WKF_TRX_FLAG { get; set; }
    }
}
