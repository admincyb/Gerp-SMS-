using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Administration.Masters;
using BusinessObject.Administration.Masters;

namespace DataAccess.Administration.Masters
{
    public class ProductionBatchDL
    {
        public static int UpdateProductionBatchStatus(int pk, int Status, int User)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_PBN_PK, pk),
                new DBService.Parameters(Parameters.P_PBN_IS_ACTIVE,  Status),
                new DBService.Parameters(Parameters.P_USER_PK,  User),
               new DBService.Parameters(GTIService.Constants.Material.Parameters.VENDORDETAILVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };
            //dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_PRD_BATCH_NO_INACTIVE, colParameters);
            //return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CompoundFormulation.Parameters.P_RET_VAL]).Value);
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_PRD_BATCH_NO_INACTIVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters.VENDORDETAILVALUE]).Value);
            return result;
        }

        public static string GetProductionBatchDetails(int pk)
        {
            StringBuilder xml = new StringBuilder(String.Empty);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_PBN_PK, pk)
            };
            DataTable dtresult = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_PRD_BATCH_NO_GET_KV, colParameters);
            foreach (DataRow drProcCtrlTrx in dtresult.Rows)
            {
                xml.Append((drProcCtrlTrx[0]).Equals(DBNull.Value) ? String.Empty : drProcCtrlTrx[0]);
            }
            return xml.ToString();

        }
        public static int SaveProductionBatch(string strXml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_XML, strXml),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.VENDORDETAILVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
             };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_PRD_BATCH_NO_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters.VENDORDETAILVALUE]).Value);
            return result;

        }

        public static int DeleteProductionBatch(int ProductionBatchPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters(Parameters.P_PBN_PK, ProductionBatchPk),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.VENDORDETAILVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
             };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_PRD_BATCH_NO_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters.VENDORDETAILVALUE]).Value);
            return result;

        }
        public static DataSet GetProductionBatchList(int PageNumber, int PageSize, int bizunit, string BatchNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizunit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM,  PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,PageSize),
              new DBService.Parameters(Parameters.P_PBN_BATCH_NO,!string.IsNullOrEmpty(BatchNo)?BatchNo: "%")
             };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPADM_PRD_BATCH_NO_LIST, colParameters);
            return dsList;
        }
    }
}
