using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Masters
{
   public class PushNotificationBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class PushNotificationHd
        {
            [XmlElement("SMQ_TRX_TYPE")]
            public string SMQ_TRX_TYPE { get; set; }
            [XmlElement("SMQ_TRX_PK")]
            public int SMQ_TRX_PK { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("SMQ_BIZUNIT")]
            public int SMQ_BIZUNIT { get; set; }
            [XmlElement("SMS_IMPORT")]
            public int SMS_IMPORT { get; set; }

            [XmlElement("Details")]
            public List<PushNotificationDetails> PushNotDts { get; set; }
        }

        [Serializable]
        [XmlRoot("Details")]
        public class PushNotificationDetails
        {
            [XmlElement("SMQ_PK")]
            public int SMQ_PK { get; set; }
            [XmlElement("SMQ_FROM")]
            public string SMQ_FROM { get; set; }
            [XmlElement("SMQ_TO")]
            public string SMQ_TO { get; set; }
            [XmlElement("SMQ_MESSAGE")]
            public string SMQ_MESSAGE { get; set; }
            [XmlElement("SMQ_STATUS")]
            public int SMQ_STATUS { get; set; }
            [XmlElement("SMQ_ATTEMPT")]
            public int SMQ_ATTEMPT { get; set; }
            [XmlElement("SMQ_SMS_DATE")]
            public DateTime SMQ_SMS_DATE { get; set; }
            [XmlElement("LAST_MOD_DATE")]
            public DateTime LAST_MOD_DATE { get; set; }


        }
    }
}
