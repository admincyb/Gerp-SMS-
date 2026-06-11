using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.Finance;

namespace DataAccess.Finance
{
    public class GstFileManagerDL
    {
        /// <summary>
        /// Get Company Info as XML
        /// </summary>
        /// <param name="companyPK"></param>
        /// <param name="gstFile"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetCompanyInfo(GafFileViewModel gafFileViewModel)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.COMPANY_PK, gafFileViewModel.CompanyPK == 0 ? (object)DBNull.Value : gafFileViewModel.CompanyPK),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.GST_FILE,GstFileTypes.NONE ==  gafFileViewModel.SelectedGstFile ? (object)DBNull.Value : Convert.ToInt32(gafFileViewModel.SelectedGstFile)),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_FROM_DATE, gafFileViewModel.FromDate == DateTime.MinValue ? (object) DBNull.Value : gafFileViewModel.FromDate),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_TO_DATE, gafFileViewModel.ToDate == DateTime.MinValue ? (object) DBNull.Value : gafFileViewModel.ToDate)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETCOMPANYINFOGSTFILE, colParameters).Tables[0];
            return dtProcess;
        }

        public static DataTable GetCompanyInfoPipeformat(GafFileViewModel gafFileViewModel)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.COMPANY_PK, gafFileViewModel.CompanyPK == 0 ? (object)DBNull.Value : gafFileViewModel.CompanyPK),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.GST_FILE,GstFileTypes.NONE ==  gafFileViewModel.SelectedGstFile ? (object)DBNull.Value : Convert.ToInt32(gafFileViewModel.SelectedGstFile)),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_FROM_DATE, gafFileViewModel.FromDate == DateTime.MinValue ? (object) DBNull.Value : gafFileViewModel.FromDate),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_TO_DATE, gafFileViewModel.ToDate == DateTime.MinValue ? (object) DBNull.Value : gafFileViewModel.ToDate)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETCOMPANYINFOGSTFILEFORPIPE, colParameters).Tables[0];
            return dtProcess;
        }
    }
}
