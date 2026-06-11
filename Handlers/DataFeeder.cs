using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Web.Script.Serialization;

namespace Handlers
{
    class KeyValueClass
    {
        public string Value
        {
            get;
            set;
        }
        public string Text
        {
            get;
            set;
        }
    }
    
    class JSONDataRow
    {
        public KeyValueClass[] RowFields
        {
            get;
            set;
        }
    }

    class JSONDatatable
    {
        public KeyValueClass[] ColumnHeaders
        {
            get;
            set;
        }
        public JSONDataRow[] Rows
        {
            get;
            set;
        }
        public string TotalResult
        {
            get;
            set;
        }
    }

    public class TextValue
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    class SerializationUtils
    {
        public JSONDatatable getSerializedDataTable(SerializationDataTable dtb)
        {
            System.Data.DataTable dt = dtb.Table;
            JSONDatatable jDt = new JSONDatatable();
            jDt.TotalResult = dtb.RecordCount;
            jDt.ColumnHeaders = new KeyValueClass[dt.Columns.Count];
            jDt.Rows = new JSONDataRow[dt.Rows.Count];
            // Add the column headers
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                jDt.ColumnHeaders[i] = new KeyValueClass() { Text = "ColumnName", Value = dt.Columns[i].ColumnName };
            }
            // Add the rows
            JSONDataRow jRow;
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                jRow = new JSONDataRow();
                jRow.RowFields = new KeyValueClass[dt.Columns.Count];
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    jRow.RowFields[k] = new KeyValueClass() { Text = dt.Columns[k].ColumnName, Value = Convert.ToString(dt.Rows[j][k]) };
                }
                jDt.Rows[j] = jRow;
            }
            return jDt;
        }

        public JsonTree getSerializedTree(DataSet dsTree, string parentPK, string parentName, string childPk, string childName, bool isCkecked, string cField, string refPK)
        {
            JsonTree jsonTree = new JsonTree();
            jsonTree.TreeDataTable = new JsTreeModel[dsTree.Tables[0].Rows.Count];
            for (int i = 0; i < dsTree.Tables[0].Rows.Count; i++)
            {
                DataRow[] drRowArray = dsTree.Tables[1].Select(parentPK + "=" + dsTree.Tables[0].Rows[i][refPK].ToString());
                if (drRowArray.Length > 0)
                {
                    var Jtree = new JsTreeModel[drRowArray.Length];
                    for (int k = 0; k < drRowArray.Length; k++)
                    {
                        Jtree[k] = new JsTreeModel { data = drRowArray[k][childName].ToString(), attributes = new JsTreeAttribute { id = drRowArray[k][childPk].ToString(), selected = ((isCkecked) ? drRowArray[k][cField].ToString() : "0") } };
                    }
                    jsonTree.TreeDataTable[i] = new JsTreeModel()
                    {
                        data = dsTree.Tables[0].Rows[i][parentName].ToString(),
                        attributes = new JsTreeAttribute { id = dsTree.Tables[0].Rows[i][parentPK].ToString() },
                        children = Jtree
                    };
                }
            }
            return jsonTree;
        }

        public string GetTextValueList(DataTable dt, string TextField, string ValueField)
        {
            JavaScriptSerializer mySerializer = new JavaScriptSerializer();
            List<TextValue> TextValueList = new List<TextValue>();
            foreach (DataRow dr in dt.Rows)
            {
                TextValueList.Add(new TextValue() { Text = Convert.ToString(dr[TextField]), Value = Convert.ToString(dr[ValueField]) });
            }
            return mySerializer.Serialize(TextValueList);
        }
    }

    public class SerializationDataTable
    {
        public string RecordCount
        {
            get;
            set;
        }
        public System.Data.DataTable Table
        {
            get;
            set;
        }
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