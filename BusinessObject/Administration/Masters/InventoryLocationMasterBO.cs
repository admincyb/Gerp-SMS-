using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Masters
{
    public class InventoryLocationMasterBO
    {
        public int Pk{  get;  set;}
        public string Code { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Parent { get; set; }
        public int bizunit { get; set; }
        public int active { get; set; }
        public int userPK { get; set; }
        public int company { get; set; }




    }
}
