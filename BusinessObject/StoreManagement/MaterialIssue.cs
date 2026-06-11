using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.StoreManagement
{
    public class MaterialIssue
    {
        public int MIH_PK { get; set; }
        public string MIH_NO { get; set; }
        public int MIH_DEPT { get; set; }
        public int MIH_DEPT_TO { get; set; }
        public List<MaterialIssueList> MIList { get; set; }
        public int UserPk { get; set; }
    }

    public class MaterialIssueList
    {
        public int SRSPk { get; set; }
        public int MaterialPk { get; set; }
        public int UOMPk { get; set; }
        public int Store { get; set; }
        
        public float QtyRecieved { get; set; }
     }

    public class MRIssue
    {
        public int ICH_PK { get; set; }
        public string ICH_NO { get; set; }
        public int ICH_DEPT { get; set; }
        public int ICH_REQ_DEPT { get; set; }
        public int ICH_DEPT_TO { get; set; }
        public List<MRIssueList> MIList { get; set; }
        public int UserPk { get; set; }
    }

    public class MRIssueList
    {
        public int SRSPk { get; set; }
        public int MaterialPk { get; set; }
        public int UOMPk { get; set; }
        public int Store { get; set; }

        public float QtyRecieved { get; set; }
    }

}
