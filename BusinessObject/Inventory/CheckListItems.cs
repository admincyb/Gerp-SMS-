using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Inventory
{
    public class CheckListItems
    {
        public int CHI_PK { get; set; }
        public string CHI_CODE { get; set; }
        public string CHI_NAME { get; set; }
        public int CHI_GROUP { get; set; }
        public string CHI_DESC { get; set; }
        public int CHI_SEQUENCE { get; set; }
        public int CHI_CONTROL { get; set; }
        public Byte CHI_ACTIVE { get; set; }
        public int CHI_MOD_BY { get; set; }
        public DateTime CHI_MOD_DT { get; set; }
        public int CHI_BIZUNIT { get; set; }
        public int? CHI_CONST_GROUP { get; set; }
        public string CTL_NAME { get; set; }
    }
}
