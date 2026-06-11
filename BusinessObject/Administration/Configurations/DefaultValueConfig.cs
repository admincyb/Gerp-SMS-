using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Configurations
{
    public class DefaultValueConfig
    {
        public int DFT_BIZUNIT { get; set; }
        public int DFT_DEPT { get; set; }
        public List<DefaultValueConfigList> DFT_LIST { get; set; }
        public int DFT_CRTD_BY {get;set;}
        public int DFT_MOD_BY {get;set;}
    }

    public class DefaultValueConfigList
    {
        public int DFT_SL { get; set; }
        public int DFT_PK { get; set; }
        public string DFT_NAME { get; set; }
        public string DFT_TYPE { get; set; }
        public string DFT_VALUE { get; set; }
        public int DFT_IS_DEFAULT { get; set; }
    }
}
