using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Configurations
{
    public class DepartmentConfig
    {
        public List<DepartmentActiveList> DepartmentActive { get; set; }
        public int UserPk { get; set; }
    }
    
    public class DepartmentActiveList
    {
        public int Active { get; set; }
        public int Department { get; set; }
    }
}
