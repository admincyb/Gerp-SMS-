using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Configurations
{
    public class CostCenterBO
    {
        public int PK { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int LocationPK { get; set; }
        public string Account1 { get; set; }
        public string Account2 { get; set; }
        public string Account3 { get; set; }
        public string OtRates { get; set; }
        public string MxOtHrs { get; set; }
        public string Description { get; set; }
        public string LastModByName { get; set; }
        public string LastModByDate { get; set; }
        public int CurrencyPK { get; set; }
        public int AirportPK { get; set; }
        public int ActiveStatus { get; set; }

        public enum ControlsEnum
        {
            GRID = 1,
            CURRENCY,
            LOCATION,
            AIRPORTS,
            DEFAULT,
            SEARCH

        }
    }
}
