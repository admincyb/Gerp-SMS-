using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPSMS_v01.Common
{
    public class UserPages : ERP.Store.UI.MyBasePage
    {
        public string PagePath
        { get; set; }
        public int ProcessID
        { get; set; }
        public int TaskID
        { get; set; }
    }
}