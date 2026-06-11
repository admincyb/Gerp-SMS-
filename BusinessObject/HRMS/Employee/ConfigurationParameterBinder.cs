using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.HRMS.Employee
{
    public sealed class ConfigurationParameterBinder
    {
        //Internal / External
        public int? ConfigPk { get; set; }
        public short? Active { get; set; }
        public string ConfigType { get; set; }

        //Issued For
        public int? GroupTypeValue { get; set; }
        public int? GroupValue { get; set; }
    }
}
