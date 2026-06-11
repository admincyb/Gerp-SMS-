using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.UserControls
{
    public class FolderBO
    {
        public long FolderPk { get; set; }
        public string FolderName { get; set; }
        public long ParentPk { get; set; }
        public int Level { get; set; }
        public int Sequence { get; set; }
        public int Module { get; set; }
        public int Active { get; set; }
        public int UserPk { get; set; }
        public int BizUnit { get; set; }
        public DateTime LastModDate { get; set; }
    }
}
