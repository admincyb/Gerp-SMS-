using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Masters
{
    public class RelatedLinksBO
    {
        public int PageId { get; set; }
        public int Created_By { get; set; }
        public string LastModDate { get; set; }
        public List<RelatedPage> RelatedPages { get; set; }
        public enum ControlsEnum
        {
            PageDropdown,
            BindDataList,
            Default
        }
        public enum SectionsEnum
        {
            EntrySection,
            ListingSection
        }
    }

    public class RelatedPage
    {
        public int RelatedPagePK { get; set; }
    }
}
