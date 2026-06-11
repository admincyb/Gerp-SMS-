using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.Reports.Abstract;
using BusinessObject.Reports.Concrete;
using BusinessObject.Reports.Helpers;

namespace BusinessObject.Reports.Factory
{
    public sealed class NumberToWordsConvertorFactory
    {        
        private string _currencyAbbrv;
        private NumberToWordsConvertor _convertor;
        private CurrencyEnum _currency = default(CurrencyEnum);

        public NumberToWordsConvertorFactory(string currency)
        {
            _currencyAbbrv = currency;
            if (Enum.GetNames(typeof(CurrencyEnum)).Any(x => x.ToLower() == currency.ToLower()))
            {
                _currency = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), currency, true);
            }
        }

        public NumberToWordsConvertor GetNumberToWordsConvertor()
        {
            switch (_currency)
            {
                case CurrencyEnum.INR:
                    _convertor = new InrNumberToWordsConvertor(_currencyAbbrv);
                    break;
                case CurrencyEnum.THB:
                    _convertor = new ThbNumberToWordsConvertor(_currencyAbbrv);
                    break;
                case CurrencyEnum.UNKNOWN:
                    _convertor = new NullNumberToWordsConvertor(_currencyAbbrv);
                    break;
                case CurrencyEnum.THBLOCALIZE:
                    _convertor = new ThbLocalizeNumberToWordsConvertor(_currencyAbbrv);
                    break;
                default:
                    _convertor = new InrNumberToWordsConvertor(_currencyAbbrv);
                    break;
            }
            return _convertor;
        }
    }
}
