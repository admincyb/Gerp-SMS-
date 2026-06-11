using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities;
using BusinessObject;

namespace BusinessLogic.Sales
{
    public class DebitCreditBL
    {

        public static DataTable GetPendingInvoiceList(int custPK, long invPK, int invCategory, int invType, DateTime? FromDate, DateTime? ToDate, int bizUnit, int pageNumber, int pageSize)
        {
            return DataAccess.Sales.DebitCreditDL.GetPendingInvoiceList(custPK, invPK, invCategory, invType, FromDate, ToDate, bizUnit, pageNumber, pageSize);
        }


        public static BusinessObject.Sales.DebitCreditHeader GetTradingSalesDebitCreditNote(string xmlDoc, long DebitCreditPk, byte IsTaxForOtherCharge)
        {
            try
            {
                BusinessObject.Sales.DebitCreditHeader resultObj = new BusinessObject.Sales.DebitCreditHeader();
                string result = DataAccess.Sales.DebitCreditDL.GetTradingSalesDebitCreditNote(xmlDoc, DebitCreditPk, IsTaxForOtherCharge);
                if (result != string.Empty)
                {
                    resultObj = (BusinessObject.Sales.DebitCreditHeader)CommonFunctions.DeserializeObject(result, resultObj);
                    return resultObj;
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

        public static long? SaveCreditDebitSales(string xmlDoc, out string crdrNo)
        {
            return DataAccess.Sales.DebitCreditDL.SaveCreditDebitSales(xmlDoc, out crdrNo);
        }

        public static DataSet GetDebitCreditList(GridPrams gridPrams, BusinessObject.User currentUser, int CusromerPk, int CrDrPk, string InvoiceNo, string PageUrl, int InvType, int CrDrType = 0, int status = 0, int cmpPk = 0)
        {
            return DataAccess.Sales.DebitCreditDL.GetDebitCreditList(gridPrams, currentUser, CusromerPk, CrDrPk, InvoiceNo, PageUrl, InvType, CrDrType, status, cmpPk);
        }

        public static long? DeleteCreditDebitDetails(long CrDrPk, DateTime LastModifiedTime)
        {
            return DataAccess.Sales.DebitCreditDL.DeleteCreditDebitDetails(CrDrPk, LastModifiedTime);
        }
    }
}
