using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.SaleOrder;
using GTIService;
using System.Data;

namespace BusinessLogic.Sales
{
    public class QuotationBL
    {
        public static System.Data.DataSet GetQuotationHeader(int enqPK)
        {
            return null;
            //return DataAccess.SaleOrder.QuotationDL.GetQuotationHeader(enqPK);
        }

        public static QuotationHeader GetQuotation(int enqPK)
        {
            try
            {
                QuotationHeader rfqResponseHeaderObj = new QuotationHeader();
                string quotation = DataAccess.SaleOrder.QuotationDL.GetQuotation(enqPK);
                if (quotation != string.Empty)
                {
                    rfqResponseHeaderObj = (QuotationHeader)CommonFunctions.DeserializeObject(quotation, rfqResponseHeaderObj);
                    return rfqResponseHeaderObj;
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

        public static int? SaveQuotationDetails(string xmlDoc)
        {
            return DataAccess.SaleOrder.QuotationDL.SaveQuotationDetails(xmlDoc);
        }

        public static System.Data.DataSet GetQuotationTaxDetails(int TaxPK, int category, int bizUnit, byte active)
        {
            return DataAccess.SaleOrder.QuotationDL.GetQuotationTaxDetails(TaxPK, category, bizUnit, active);
        }

        public static int? GenerateSaleOrder(int CurrPK, int userPK)
        {
            return DataAccess.SaleOrder.QuotationDL.GenerateSaleOrder(CurrPK, userPK);
        }

        public static DataSet GetItemDetails(int itemPK)
        {
            return DataAccess.SaleOrder.QuotationDL.GetItemDetails(itemPK);
        }

        public static DataSet GetItemRates(int itemPK)
        {
            return DataAccess.SaleOrder.QuotationDL.GetItemRates(itemPK);
        }
        public static DataSet GetQuoationRevisionHistory(int itemPK)
        {
            return DataAccess.SaleOrder.QuotationDL.GetQuoationRevisionHistory(itemPK);
        }

        public static DataTable GetCrmQuoations(string Value, int CustomerPK, int SbuID)
        {
            return DataAccess.SaleOrder.QuotationDL.GetCrmQuoations(Value, CustomerPK, SbuID);
        }

        public static DataTable GetCrmQuoationDetails(int QuoteId)
        {
            return DataAccess.SaleOrder.QuotationDL.GetCrmQuoationDetails(QuoteId);
        }

        public static DataTable ValidateCRMCustomer(int QuoteId)
        {
            return DataAccess.SaleOrder.QuotationDL.ValidateCRMCustomer(QuoteId);
        }
    }
}
