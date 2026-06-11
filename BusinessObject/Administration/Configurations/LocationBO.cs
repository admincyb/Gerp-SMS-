using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Configurations
{
    public class LocationBO
    {

        public int PK { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Country { get; set; }
        public int AdmReg { get; set; }
        public int Currency { get; set; }
        public int SalMonths { get; set; }
        public int SalaryMonth1 { get; set; }
        public int SalaryMonth2 { get; set; }
        public decimal SalaryMonth1Perc { get; set; }
        public decimal SalaryMonth2Perc { get; set; }
        public string Account1 { get; set; }
        public string Account2 { get; set; }
        public string Account3 { get; set; }
        public string Description { get; set; }
        public int ActiveStatus { get; set; }
        public string LastModDate { get; set; }

        public enum MonthValue
        {
            TWELEVE = 12,
            THIRTEEN = 13,
            FOURTEEN = 14,
        }

        public enum ControlsEnum
        {
            GRID,
            CURRENCY,
            SEARCH,
            COUNTRY,
            DEFAULT,
            MONTH
        }

    }   
}
