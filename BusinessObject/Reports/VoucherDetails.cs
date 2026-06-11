using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Reports
{
    public sealed class VoucherDetails
    {
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public string Description { get; set; }
        public string ExRate { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
    }
}
