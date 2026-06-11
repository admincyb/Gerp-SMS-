using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.StoreManagement
{
    public class MaterialAccept
    {
        public int MAH_PK { get; set; }
        public string MAH_NO { get; set; }
        public DateTime MAH_DATE { get; set; }
        public int MAH_DEPT { get; set; }
        public List<MaterialAcceptList> MaterialList { get; set; }
        public int UserPk { get; set; }
    }

    public class MaterialAcceptList
    {
        public int MAD_PK { get; set; }
        public int MAD_MI { get; set; }
        public int MAD_NO { get; set; }
        public int MAD_ITEM { get; set; }
        public int MAD_UOM { get; set; }
        public float MAD_QTY_ISSUED { get; set; }
        public float MAD_QTY_APPROVED { get; set; }
        public float MAD_QTY_RECEIVED { get; set; }
    }
}
