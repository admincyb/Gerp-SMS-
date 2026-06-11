using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject
{
    public class GridPrams
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FilterStatus { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Fields { get; set; }
        public string SortBy { get; set; }
        public string ThenBy { get; set; }
        public string SortDirection { get; set; }
        public string ThenDirection { get; set; }
        public string SearchBy { get; set; }
        public string SearchValue { get; set; }
        public int Flag { get; set; }
        public int UserPK { get; set; }
        public int DeptPK { get; set; }
        public int P_DeptPK { get; set; }
        public int CancelFlag { get; set; }
        public int ShowAll { get; set; }
        public int statusPk { get; set; }
        public int CompanyPK { get; set; }
        public string SortBy1 { get; set; }
        public string SortDirection1 { get; set; }
        public int TotalRecords { get; set; }
        public int BizUnit { get; set; }
        public int PRH_TYPE { get; set; }
        public int POH_MENU_TYPE { get; set; }
        public int POType { get; set; }
        public int ConvertTo { get; set; }
        public string SCNo { get; set; }
        public string InvNo { get; set; }

        public int PendingWO { get; set; }
        public string PoNumber { get; set; }
    }
}
