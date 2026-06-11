using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.Finance.Administration.Masters;
using DataAccess.Finance.Administration.Masters;

namespace BusinessLogic.Finance.Administration.Masters
{
    public class InvoiceTypeBL
    {
        public static int SaveInvoiceType(InvoiceTypeBO ObjInvoiceType)
        {
            return InvoiceTypeDL.SaveInvoiceType(ObjInvoiceType);
        }
        public static System.Data.DataTable GetInvoiceTypeList(string code, string name, int bizUnit, int pageIndex, int pageSize)
        {
            return InvoiceTypeDL.GetInvoiceTypeList(code, name, bizUnit, pageIndex, pageSize);
        }
        public static System.Data.DataTable GetInvoiceTypeDetailList(int? invPK, int active, int bizUnit)
        {
            return InvoiceTypeDL.GetInvoiceTypeDetailList(invPK, active, bizUnit);
        }
        public static int DeleteInvoiceTypeDetails(int CurrPK, DateTime LastModifiedTime)
        {
            return InvoiceTypeDL.DeleteInvoiceTypeDetails(CurrPK, LastModifiedTime);
        }
    }
}
