using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Finance
{
    public struct GafFileViewModel
    {
        public int CompanyPK { get; set; }
        public GstFileTypes SelectedGstFile { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
