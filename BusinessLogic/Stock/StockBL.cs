using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.CommonManagement;
using DataAccess.Stock;
using ERP.Utilities.Dashboard; 

namespace BusinessLogic.Stock
{
    public class StockBL
    {
        public static int? StockTakingSave(string Xml, ref string RefNO)
        {
            return StockDL.StockTakingSave(Xml, ref RefNO);
        }

        public static DataSet GetStockTakeList(FilterParameters objFilterParam, string FromDate, string ToDate)
        {
            return StockDL.GetStockTakeList(objFilterParam, FromDate, ToDate);
        }

        public static string GetStockTakeDetails(int currPK, int? bizUnit)
        {
            return StockDL.GetStockTakeDetails(currPK, bizUnit);
        }

        public static int? DeleteStockTaking(int currPK, DateTime lastModDate)
        {
            return StockDL.DeleteStockTaking(currPK, lastModDate);
        }

        public static DataTable GetActiveRecord(int? bizUnit)
        {
            return StockDL.GetActiveRecord(bizUnit);
        }

        public static DataTable GetStockReconciliation(int StockInitPk, int? RecnType)
        {
            return StockDL.GetStockReconciliation(StockInitPk, RecnType);
        }
    }
}
