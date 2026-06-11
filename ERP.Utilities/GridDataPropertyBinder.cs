using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities
{
    public class FilterUtility
    {
        /// <summary>
        /// Current Listing Page Index
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Listing Page Size
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Sort Listing By
        /// </summary>
        public string SortBy { get; set; }
        /// <summary>
        /// Then Listing By
        /// </summary>
        public string ThenBy { get; set; }
        /// <summary>
        /// Listing Sort Direction
        /// <value>asc</value>
        /// <value>desc</value>
        /// </summary>
        public string SortDirection { get; set; }
        /// <summary>
        /// Count of Total Records after filter
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// Field to filter
        /// </summary>
        public string FilterBy { get; set; }
        /// <summary>
        /// Filter value
        /// </summary>
        public string FilterValue { get; set; }
        /// <summary>
        /// Field to Search Field
        /// </summary>
        public string SearchBy { get; set; }
        /// <summary>
        /// Field to Search VoucherNo
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// Field to search Invoice Type
        /// </summary>
        public int InvoiceType { get; set; }
        /// <summary>
        /// Field to filter Date
        /// </summary>
        public DateTime? FilterDate { get; set; }
        /// <summary>
        /// Field to filter To Date
        /// </summary>
        public DateTime? FilterToDate { get; set; }
        /// <summary>
        /// Used for enabling advance filter in specific cases
        /// </summary>
        public bool NeedAdvanceFilter { get; set; }
        /// <summary>
        /// Field to filter with user mapping
        /// </summary>
        public int User { get; set; }

        public int? BizUnit { get; set; }
        /// <summary>
        /// Any filterations, specifically against SBU
        /// </summary>
        public bool IsSBUSpecific { get; set; }
    }

    [Serializable]
    public sealed class GridDataPropertyBinder
    {
        /// <summary>
        /// Current Listing Page Index
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Listing Page Size
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Sort Listing By
        /// </summary>
        public string SortBy { get; set; }
        /// <summary>
        /// Then Listing By
        /// </summary>
        public string ThenBy { get; set; }
        /// <summary>
        /// Listing Sort Direction
        /// <value>asc</value>
        /// <value>desc</value>
        /// </summary>
        public string SortDirection { get; set; }
        /// <summary>
        /// Count of Total Records after filter
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// Field to filter
        /// </summary>
        public string FilterBy { get; set; }
        /// <summary>
        /// Filter value
        /// </summary>
        public string FilterValue { get; set; }
        /// <summary>
        /// Field to Search Field
        /// </summary>
        public string SearchBy { get; set; }
        /// <summary>
        /// Field to Search VoucherNo
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// Field to filter Date
        /// </summary>
        public DateTime? FilterDate { get; set; }
        /// <summary>
        /// Field to filter To Date
        /// </summary>
        public DateTime? FilterToDate { get; set; }

    }
    /// <summary>
    /// For return Output values from a stored procedure
    /// </summary>
    public class RetValues
    {
        #region Private variables
        int? _pRetVal;
        #endregion

        #region Properties

        public int RetVal
        {
            get { return _pRetVal ?? -1; }
            set { _pRetVal = value; }
        }

        public string RetNumber { get; set; }

        #endregion
    }
}
