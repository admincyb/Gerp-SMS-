using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Masters
{
    public  class VendorTerm
    {
        public int TermPK { get; set; }
        public string TermTitle { get; set; }
        public string Termtype { get; set; }
        public string TermDescr { get; set; }
        public int Userpk { get; set; }
        public int Status { get; set; }
        public int ACTIVE { get; set; }
        public int BizUnitPk { get; set; }
        public int SBU { get; set; }
    }
}
