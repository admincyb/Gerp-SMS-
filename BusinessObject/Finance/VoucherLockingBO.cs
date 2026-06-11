using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Finance
{
    [Serializable]    
    public sealed class VoucherLockingBO
    {
        public int FLL_PK { get; set; }
        public int FLL_BIZUNIT { get; set; }
        public int FLL_VERSION { get; set; }
        public DateTime FLL_DATE { get; set; }
        public string FLL_REMARKS { get; set; }
        public int FLL_ACTIVE { get; set; }
        public int FLL_CRTD_BY { get; set; }       
        public DateTime FLL_CRTD_DT { get; set; }
        public int FLL_MODULE { get; set; }       
    } 
    
}
