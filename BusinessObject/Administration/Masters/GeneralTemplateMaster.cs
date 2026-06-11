using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Masters
{
    /// <summary>
    /// Main class for general template master
    /// </summary>
    public class GeneralTemplateMaster
    {
        public int TMH_PK { get; set; }
        public string TMH_NAME { get; set; }
        public int TMH_TERM_GROUP { get; set; }
        public List<TemplateDetail> TemplateDetails { get; set; }
    }
    /// <summary>
    /// Class for terms details
    /// </summary>
    public class TemplateDetail
    {
        public int TMD_PK { get; set; }
        public string TMD_NAME { get; set; }
        public string TMD_DESC { get; set; }
        public bool TMD_REQD{ get; set; }
        public bool TMD_ACTIVE { get; set; }
    }
}
