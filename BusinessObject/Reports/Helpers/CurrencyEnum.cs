using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Reports.Helpers
{
    internal enum CurrencyEnum
    {
        UNKNOWN = 0, // Assigned when calling default(CurrencyEnum) Method
        INR,
        THB,
        THBLOCALIZE
    }
}
