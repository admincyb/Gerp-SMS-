using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Configurations
{
    public class CurrenciesBO
    {
        public int CurrencyPK { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Fractions { get; set; }
        public int Decimals { get; set; }
        public string Description { get; set; }
        public int ActiveStatus { get; set; }
        public string LastModDate { get; set; }
    }
}
