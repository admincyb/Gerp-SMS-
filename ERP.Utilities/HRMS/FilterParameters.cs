using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.HRMS
{
    public sealed class FilterParameters
    {
        public int? PK { get; set; }
        public int? Status { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? PayElementPk { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int? UserPK { get; set; }
        public int? Active { get; set; }
        public int? BizUnit { get; set; }
        public int? Department { get; set; }
        public int? Designation { get; set; }
        public int? BranchLocation { get; set; }
        public int? TransactionNo { get; set; }
        public int? ToBranchLocation { get; set; }
        public int? EmploymentType { get; set; }
        public int? Company { get; set; }
        public int? Employee { get; set; }
        public int? EmployeeCategory { get; set; }
        public int? EmployeeType { get; set; }
        public int? PaymentMode { get; set; }
        public int? ProcessMode { get; set; }
        public string TrxNo { get; set; }
        public int? EmpCurrency { get; set; }
        public int? EmpBank { get; set; }
        public int? payrollType { get; set; }
        public DateTime? SalaryMonth { get; set; }
        public int? Type { get; set; }
        public string MobileNo { get; set; }

        //Bonus
        public int? Religion { get; set; }
        public int? SubReligion { get; set; }
        public string State { get; set; }
        public int? Currency { get; set; }
        public int? BonusType { get; set; }
        public DateTime? EmpDoj { get; set; }
        public string StatePk { get; set; }

        public DateTime? EmpAppDate { get; set; }

        public int? LvFromHalf { get; set; }
        public int? LvToHalf { get; set; }
        public int? LvHolidaySkip { get; set; }
        public int? LvOffDaySkip { get; set; }

    }
}
