using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Configurations
{
    public class RoleActionBO
    {
        public int PK { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Parent { get; set; }
        public string ParentName { get; set; }
        public string ParentCode { get; set; }
        public int RoleLevel { get; set; }
        public int RolePK { get; set; }
        public string Description { get; set; }
        public int ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModBy { get; set; }
        public string LastModByName { get; set; }
        public string LastModDate { get; set; }
        public int BizUnit { get; set; }

        public enum ControlsEnum
        {
            TREE = 1,
            ROLES,
            DEFAULT
        }        
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class RoleMappingHeader
    {
        public int EMP_PK { get; set; }
        [XmlElement("RoleDtl")]
        public List<RoleMapping> RoleList { get; set; }
    }

    [Serializable]
    //[XmlRoot("root")]
    public class RoleMapping
    {
        public int ermPK { get; set; }
        public int ermRole { get; set; }
        public string EMP_ROLE_TEXT { get; set; }       
        public int ermActive { get; set; }
        public string IS_CHECKED { get; set; }
        public int ERM_EMP { get; set; }
    }
}
