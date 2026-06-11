using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Production
{
   public  class CompoundPreparation
    {
       public int CTH_PK { get; set; }
       public string CTH_BATCH_NO { get; set; }
       public int CTH_COMPOUND { get; set; }
       public DateTime CTH_COMP_DT { get; set; }
       public int CTH_PLAN { get; set; }
       public DateTime CTH_START_TM { get; set; }
       public DateTime CTH_END_TM { get; set; }
       public float CTH_TOTAL_TM { get; set; }
       public int CTH_TOTAL_TM_UNIT { get; set; }
       public float CTH_QUANTITY { get; set; }
       public int CTH_QUANTITY_UOM { get; set; }
       public int CTH_BIZUNIT { get; set; }
       public int CTH_MOD_DT { get; set; }
       public List<Materials> Materials { get; set; } 
      
       
    }
   public class Materials
   {
       public int CTD_PK { get; set; }
       public int CTD_ITEM_TYPE { get; set; }
       public int CTD_ITEM { get; set; }
       public string CTD_BATCH { get; set; }
       public float CTD_QUANTITY { get; set; }
       public int CTD_QTY_UOM { get; set; }
       public int CTH_MOD_DT { get; set; }
      // public int CTH_MOD_DT { get; set; }

   }

}
