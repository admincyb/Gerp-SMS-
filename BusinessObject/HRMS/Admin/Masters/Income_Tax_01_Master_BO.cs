using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
   public class Income_Tax_01_Master_BO
    {
        [Serializable]
        [XmlRoot("Root")]
        public sealed class Income_Tax_01
        {
            [XmlElement("Type")]
            public int Type { get; set; }

            [XmlElement("Section")]
            public List<Section> Section { get; set; }
        }

        [Serializable]
        [XmlRoot("Section")]
        public sealed class Section
        {
            [XmlElement("Seq")]
            public int Seq { get; set; }
            [XmlElement("SectionID")]
            public int SectionID { get; set; }
            [XmlElement("Title")]
            public string Title { get; set; }
            [XmlElement("Col1")]
            public string Col1 { get; set; }
            [XmlElement("Col2")]
            public string Col2 { get; set; }
            [XmlElement("Col3")]
            public string Col3 { get; set; }
            [XmlElement("Col4")]
            public string Col4 { get; set; }
            [XmlElement("Col5")]
            public string Col5 { get; set; }
            [XmlElement("Col5_Visible")]
            public int Col5_Visible { get; set; }

            [XmlElement("Col3FunId")]
            public string Col3FunId { get; set; }
            [XmlElement("Col3FunExpression")]
            public string Col3FunExpression { get; set; }
            [XmlElement("Col3ExcludeId")]
            public string Col3ExcludeId { get; set; }
            [XmlElement("Col3CopyId")]
            public string Col3CopyId { get; set; }

            [XmlElement("Items")]
            public List<Items> Items { get; set; }
        }

        [Serializable]
        [XmlRoot("Items")]
        public sealed class Items
        {
            [XmlElement("Seq")]
            public int Seq { get; set; }
            [XmlElement("DbId")]
            public int DbId { get; set; }
            [XmlElement("Col1")]
            public string Col1 { get; set; }
            [XmlElement("Col2")]
            public string Col2 { get; set; }
            [XmlElement("Col3")]
            public string Col3 { get; set; }
            [XmlElement("Col4")]
            public string Col4 { get; set; }
            [XmlElement("Col5")]
            public string Col5 { get; set; }
            [XmlElement("Col3_Edit")]
            public int Col3_Edit { get; set; }
            [XmlElement("Col3_Hide")]
            public int Col3_Hide { get; set; }
            [XmlElement("Col4_Hide")]
            public int Col4_Hide { get; set; }
            [XmlElement("DefaultAmount")]
            public string DefaultAmount { get; set; }
            [XmlElement("Col3_Max")]
            public string Col3_Max { get; set; }
            [XmlElement("ChkBoxReq")]
            public int ChkBoxReq { get; set; }
            [XmlElement("ColScript")]
            public string ColScript { get; set; }
        }
    }
}
