using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPManager.Finance
{
    [Serializable]
    public class CrdrAllocations
    {       
        public DateTime TRX_DATE { get; set; }
        public string TRX_NO { get; set; }
        public decimal TRX_AMOUNT { get; set; }         
    }
}
