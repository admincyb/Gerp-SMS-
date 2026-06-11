using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Finance;
using System.Data;
using BusinessObject.Finance;

namespace BusinessLogic.Finance
{
    public class GstFileManagerBL
    {
        public static string GetGSTFileAsXML(GafFileViewModel gafFileViewModel)
        {
            StringBuilder _builder = new StringBuilder();
            DataTable dataTable = GstFileManagerDL.GetCompanyInfo(gafFileViewModel);

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    _builder.Append(row[0] ?? string.Empty);
                }
            }
            return _builder.ToString();
        }

        public static string GetGSTFileAsPipe(GafFileViewModel gafFileViewModel)
        {
            StringBuilder _builder = new StringBuilder();
            DataTable dataTable = GstFileManagerDL.GetCompanyInfoPipeformat(gafFileViewModel);
            dataTable.Rows.Cast<DataRow>().ToList().ForEach(dataRow =>
            {
                dataTable.Columns.Cast<DataColumn>().ToList().ForEach(column =>
                {
                    _builder.AppendFormat("{0} ", dataRow[column]);

                });
                _builder.Append(Environment.NewLine);
            });
            return _builder.ToString();


        }
    }
}
