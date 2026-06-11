using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.eDocs
{
    [Serializable]
    [XmlRoot("Root")]
    public class FolderMappingBo
    {
        [XmlElement("FUM_FOLDER")]
        public long FolderPk { get; set; }

        [XmlElement("USER_PK")]
        public int UserPk { get; set; }

        [XmlElement("BIZUNIT_PK")]
        public int BizUnit { get; set; }

        [XmlElement("ACTIVE")]
        public int Active { get; set; }

        [XmlElement("Detail")]
        public List<FolderMappedUser> Detail { get; set; }
    }

    [Serializable]
    public class FolderMappedUser
    {
        [XmlElement("FUM_PK")]
        public int MappingPk { get; set; }

        [XmlElement("FUM_USER")]
        public int MappedUserPk { get; set; }
    }
}
