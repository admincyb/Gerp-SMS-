using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Configurations
{
    public class NewsManagementBO
    {
        public int NewsPK { get; set; }
        public string Title { get; set; }
        public string ShortDesc { get; set; }
        public DateTime PublishedDt { get; set; }

        public string Details { get; set; }
        public int ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public string LastModDate { get; set; }

        public enum ControlsEnum
        {
            GRID,
            DEFAULT
        }
    }
}
