using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Reports.Abstract
{
    public abstract class NumberToWordsConvertor
    {
        protected string _currency;
        public NumberToWordsConvertor(string currency)
        {
            this._currency = currency;
        }
        public abstract string ConvertNumberToWords(string Number);
    }
}
