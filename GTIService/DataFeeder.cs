using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Web.Script.Serialization;

namespace GTIService.Utils.Serializer
{
    public class TextValue
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class JsTreeModel
    {
        public string data;
        public JsTreeAttribute attributes;
        public JsTreeModel[] children;
    }
    public class JsTreeAttribute
    {
        public string id;
        public string selected;
    }
    public class JsonTree
    {
        public JsTreeModel[] TreeDataTable;
    }
}