using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Configurations
{
    public class DepartmentSettings
    {
        public int CFG_BIZUNIT { get; set; }
        public int CFG_DEPT { get; set; }
        public List<ConfigList> CFG_LIST { get; set; }
        public int CFG_CRTD_BY { get; set; }
        public int CFG_MOD_BY { get; set; }
    }

    public class ConfigList
    {
        public int CFG_SL { get; set; }
        public int CFG_PK { get; set; }
        public string CFG_NAME { get; set; }
        public string CFG_TYPE { get; set; }
        public string CFG_VALUE { get; set; }
        public int CFG_IS_DEFAULT { get; set; }
    }
}
