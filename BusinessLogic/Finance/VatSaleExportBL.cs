using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTIService;
using DataAccess.Finance;
using BusinessObject;
using System.Data;
using BusinessObject.Finance;
namespace BusinessLogic.Finance
{
    public class VatSaleExportBL
    {
        public static VatSale GetVatSale(GridPrams grid, User objUser)
        {
            try
            {
                VatSale vatSaleObj = new VatSale();
                string vatSaleList = VatSaleExportDL.GetVatSaleList(grid, objUser);
                if (vatSaleList != string.Empty)
                {
                    vatSaleObj = (VatSale)CommonFunctions.DeserializeObject(vatSaleList, vatSaleObj);
                    return vatSaleObj;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetVatSaleReport(DateTime fromDate,DateTime toDate, User objUser)
        {
            return DataAccess.Finance.VatSaleExportDL.GetVatSaleReport(fromDate, toDate, objUser);
        }
        public static DataSet SaveVatSale(string strxml)
        {
            return DataAccess.Finance.VatSaleExportDL.SaveVatSale(strxml);
        }

        public static DataTable GetVatSaleFieldAuto(byte Active,string itemField, int bizUnit, string searchValue)
        {
            return VatSaleExportDL.GetVatSaleFieldAuto(Active, itemField, bizUnit, searchValue);
        }
        public static DataTable GetVatSaleSearch(GridPrams grid, string customerName, string invoiceNo, User objUser)
        {
            return VatSaleExportDL.GetVatSaleSearchList(grid, customerName, invoiceNo, objUser);
        }
    }
}
