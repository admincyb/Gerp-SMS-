using DataAccess.Administration.Masters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Administration.Masters
{
    public class ProductionBatchBL
    {
        public static int UpdateProductionBatchStatus(int pk, int Status, int User)
        {
            return ProductionBatchDL.UpdateProductionBatchStatus(pk, Status, User);
        }
        public static string GetProductionBatchDetails(int pk)
        {
            return ProductionBatchDL.GetProductionBatchDetails(pk);
        }
        public static int SaveProductionBatch(string strXml)
        {
            return ProductionBatchDL.SaveProductionBatch(strXml);
        }

        public static int DeleteProductionBatch(int ProductionBatchPK)
        {
            return ProductionBatchDL.DeleteProductionBatch(ProductionBatchPK);
        }
        public static DataSet GetProductionBatchList(int PageNumber, int PageSize, int bizunit, string BatchNo)
        {
            DataSet dsProductionBatchList = ProductionBatchDL.GetProductionBatchList(PageNumber, PageSize, bizunit, BatchNo);
            return dsProductionBatchList;
        }

    }
}
