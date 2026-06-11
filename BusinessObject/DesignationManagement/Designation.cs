using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.DesignationManagement
{
    public class Designation
    {
        public int DSG_PK { get; set; }
        public string DSG_CODE { get; set; }
        public string DSG_NAME { get; set; }
        public string DSG_DESC { get; set; }

        public int DSG_ACTIVE { get; set; }
        public int DSG_DEPT { get; set; }
        public int SBU { get; set; }

        public int UserPk { get; set; }
       
       
        
    }
}
