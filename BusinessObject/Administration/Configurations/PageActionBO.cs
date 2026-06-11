using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Configurations
{
    public class PageActionBO
    {
        public int PageActionPK { get; set; }
        public int Page { get; set; }
        public string Section { get; set; }
        public string Action { get; set; }
        public string ActionDesc { get; set; }
        public int BizUnit { get; set; }
        public int User { get; set; }
        public DateTime LastModDt { get; set; }

        public enum ControlsEnum
        {
            DEFAULT,
            PAGES,
            PAGEACTIONS,
            EDITING,
            DELETING,
            SAVE,
            ACTIONS
        }

        public enum SectionsEnum
        {
            EntrySection,
            ListingSection
        }
    }
}
