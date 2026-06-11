using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Masters
{
    public class PortMasterBO
    {
        public int PK { get; set; }
        public string Name { get; set; }
        public string DisplayCode { get; set; }
        public int Type { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int IsSalesFrom { get; set; }
        public int IsSalesTo { get; set; }
        public int IsPurFrom { get; set; }
        public int IsPurTo { get; set; }
        public int Active { get; set; }
        public int bizunit { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
        public int USER_PK { get; set; }
        public DateTime PRM_MOD_DT { get; set; }


    }
}
