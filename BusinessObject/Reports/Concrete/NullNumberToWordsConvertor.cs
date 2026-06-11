using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.Reports.Abstract;

namespace BusinessObject.Reports.Concrete
{
    internal sealed class NullNumberToWordsConvertor : NumberToWordsConvertor
    {
        public NullNumberToWordsConvertor(string currency) : base(currency) { }

        public override string ConvertNumberToWords(string Number)
        {
            return string.Format("No Convertor implemented for {0}",_currency.ToUpper());
        }
    }
}
