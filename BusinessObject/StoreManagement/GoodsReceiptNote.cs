using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.StoreManagement
{
    public class GoodsReceiptNote
    {
        public int GRH_PK { get; set; }
        public string GRH_NO { get; set; }
        public int GRH_DEPT { get; set; }
        public int GRH_VENDOR { get; set; }
        public List<GoodsReceiptNoteList> GRNList { get; set; }
        public int UserPk { get; set; }
    }

    public class GoodsReceiptNoteList
    {
        public int POPk { get; set; }
        public int MaterialPk { get; set; }
        public int UOMPk { get; set; }
        public int Store { get; set; }
        public int Vendor { get; set; }
        public float QtyRecieved { get; set; }
     }
}
