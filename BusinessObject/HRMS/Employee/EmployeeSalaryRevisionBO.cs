using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.HRMS.Employee
{
    public class EmployeeSalaryRevisionBO
    {
        [Serializable]
        public class RevisionHistory
        {
            public int SlNo { get; set; }
            public int Pk { get; set; }
            public string EffectivePeriod { get; set; }
            public decimal CTC { get; set; }
            public decimal PreviousCTC { get; set; }
            public string Designation { get; set; }
            public string Department { get; set; }
        }
    }
}
