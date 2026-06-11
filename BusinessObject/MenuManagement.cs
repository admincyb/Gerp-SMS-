using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject
{
    public class MenuManagement
    {
        public string MenuName
        {
            get;
            set;
        }

        public int MenuParent
        {
            get;
            set;
        }

        public string MenuURL
        {
            get;
            set;
        }

        public int MenuPosition
        {
            get;
            set;
        }
        public int MenuID
        {
            get;
            set;
        }
        public int[] relatedItems
        {
            get;
            set;
        }
    }
}
