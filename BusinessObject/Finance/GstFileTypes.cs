using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Finance
{
    public enum GstFileTypes
    {
        NONE = -1,
        ALL = 0,
        COMPANYINFO = 1,
        SUPPLY = 2,
        PURCHASE = 3,
        GENERALLEDGER = 4
    }
}
