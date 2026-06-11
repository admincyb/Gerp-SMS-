using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.HRMS
{
    public sealed class EmpDocumentFilterParameterBinder
    {
        public int? DocPk { get; set; }
        /// <summary>
        /// Filter For 
        /// </summary>
        public short? IsCheckedOut { get; set; }
        public short? IsCheckedFor { get; set; } // filter For
        public int? DocumentType { get; set; }
        public string EmployeeName { get; set; }
        public string DocNo { get; set; }
        public string EmployeeCode { get; set; }
        public int? Nationality { get; set; }
        public int? Company { get; set; }
        public int? ExpiringIn { get; set; }
        public int? Employee { get; set; }
        public int? BizUnit { get; set; }
        public string ExpiresBefore { get; set; }
        public int? EmployeeType { get; set; }

        //Employee List Filter variables

        public int? Designation { get; set; }

        public int? CostCenter { get; set; }

        public int? Team { get; set; }


        public int? Department { get; set; }
        public int? BranchLoc { get; set; }
        public int? Active { get; set; }
        public int? CurrentStatus { get; set; }
        public string PassportNO { get; set; }
        public string PermitNO { get; set; }       
        public int? EmpCategory { get; set; }

        //Enable Paging And Sorting
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }

    }
}

