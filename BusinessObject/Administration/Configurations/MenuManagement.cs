using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Configurations
{
    public class MenuManagement
    {
        public int MenuPK { get; set; }
        public int MenuParentPK { get; set; }
        public string MenuName { get; set; }
        public string MenuParentName { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIcon { get; set; }
        public int MenuPosition { get; set; }
        public string HasChild { get; set; }
        public int UserPk { get; set; }
        public int SBU { get; set; }
    }
}
