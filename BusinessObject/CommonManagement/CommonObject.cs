using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.CommonManagement
{
    public class CommonObject
    {

        public class File
        {
            public List<FileList> FILELIST { get; set; }


            public class FileList
            {
                public int DOC_PK { get; set; }
                public string DOC_TITLE { get; set; }
                public string DOC_NAME { get; set; }
                public string DOC_TYPE { get; set; }
                public int DOC_SEQ_NO { get; set; }


            }
        }
     

        public class WorkFlowComment
        {
            public int ActionID { get; set; }
            public int ApplicationID { get; set; }
            public int ProcessID { get; set; }
            public int ReferenceID { get; set; }
            public int TaskID { get; set; }
            public int UserPk { get; set; }
            public string UsersList { get; set; }
            public string WrkfComment { get; set; }
        }
    }
}
