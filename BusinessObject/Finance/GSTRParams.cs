using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Finance
{
    public class GSTRParams
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int ReportType { get; set; }
        public int BizUnit { get; set; }      
    }
}
