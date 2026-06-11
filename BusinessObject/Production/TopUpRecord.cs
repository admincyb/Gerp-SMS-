using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Production
{
    public class TopUpRecord
    {
        public int TUH_PK { get; set; }

        public int TUH_SHIFT { get; set; }
        public string TUH_SHIFT_NAME { get; set; }

        public int TUH_PLAN { get; set; }

        public string TUH_PLAN_NAME { get; set; }


        public DateTime TUH_DATE { get; set; }
        public DateTime TUH_TIME { get; set; }

        public int UserPk { get; set; }

        public int BizUnitPk { get; set; }

        public List<ItemListDetails> ItemList { get; set; }


        public class ItemListDetails
        {
            public int TUD_PK { get; set; }

            public int SL_NO { get; set; }

            public int TUD_TANK_TYPE { get; set; }
            public string TUD_TANK_TYPE_NAME { get; set; }  //

            public int TUD_TANK { get; set; }
            public string TUD_TANK_NAME { get; set; }  //

            public int TUD_ITEM_TYPE { get; set; }
            public string TUD_ITEM_TYPE_NAME { get; set; } //


            public int TUD_ITEM { get; set; }
            public string TUD_ITEM_NAME { get; set; }  //
            //public int ITEM_UOM { get; set; }     //

            public int TUD_BATCH { get; set; }
            public string TUD_BATCH_NAME { get; set; }  //

            public float TUD_QUANTITY { get; set; }

            public int TUD_QTY_UOM { get; set; }

            public string QUANTITY { get; set; }   //


        }
    }
}
