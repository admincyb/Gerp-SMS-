using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Masters
{
    /// <summary>
    /// Class for Template Group
    /// </summary>
    public class TemplateGroupMaster
    {
        public int TemplateGrpPK { get; set; }
        public string TemplateGrpName { get; set; }
        public int BizUnitPk { get; set; }
        public int DeptPk { get; set; }
        public int UserPK { get; set; }

    }
}
