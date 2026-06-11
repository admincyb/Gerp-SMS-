using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Masters
{
    public class CurrencyMaster
    {
        public string CurrencyName
        {
            get;
            set;
        }
        public string CurrencyCode
        {
            get;
            set;
        }
        public int CurrencyMasterPk
        {
            get;
            set;
        }
        public string DisplayIn
        {
            get;
            set;
        }
       
        public int Fraction
        {
            get;
            set;
        }
        public string Symbol
        {
            get;
            set;
        }
       
        public int UserID
        {
            get;
            set;
        }
        public int ExchangeMasterPk
        {
            get;
            set;
        }
        public int ExchangeToPk
        {
            get;
            set;
        }
        public string ExchangeToName
        {
            get;
            set;
        }
        public float ExchangeRate
        {
            get;
            set;
        }
        public List<Exchange> ConversionList { get; set; }
    }

    public class Exchange
    {

    }

}
