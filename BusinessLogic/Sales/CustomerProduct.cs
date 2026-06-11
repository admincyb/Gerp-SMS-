using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.SaleOrder;
using BusinessObject;

namespace BusinessLogic.Sales
{
    public class CustomerProduct
    {
        /// <summary>
        /// Get GetBrand Details by PK
        /// </summary>
        /// <param name="brandPK"></param>
        /// <returns></returns>
        public static DataTable GetBrandDetails(int brandPK)
        {
            return CustomerProductDL.GetBrandDetails(brandPK);
        }
        public static DataSet GetCustomerProduct(int pk, int itemPK, int cusPK, string brand, string item, int bizUnit, int active, bool? hasSpec = false)
        {
            return CustomerProductDL.GetCustomerProduct(pk, itemPK, cusPK, brand, item, bizUnit, active, hasSpec);
        }
        public static DataSet GetCustomerProductAuto(int cusPK, string brand, int bizUnit, int active, bool? hasSpec = false)
        {
            return CustomerProductDL.GetCustomerProductAuto(cusPK, brand, bizUnit, active, hasSpec);
        }
        public static DataSet GetBrandBySC(int shpPK, int scPK, string brand, int bizUnit, int active, bool? hasSpec = false)
        {
            return CustomerProductDL.GetBrandBySC(shpPK, scPK, brand, bizUnit, active, hasSpec);
        }
        public static DataSet GetLotNoByLoadPk(int pk, string lotNo, int bizUnit, int active, bool? hasSpec = false)
        {
            return CustomerProductDL.GetLotNoByLoadPk(pk, lotNo, bizUnit, active, hasSpec);
        }
        public static DataSet GetLotNoBySphpPlanPk(int pk, string lotNo, int bizUnit, int active, bool? hasSpec = false)
        {
            return CustomerProductDL.GetLotNoBySphpPlanPk(pk, lotNo, bizUnit, active, hasSpec);
        }
        public static DataSet GetLotNoByBrandPK(int pk, string lotNo, int bizUnit, int active, bool? hasSpec = false)
        {
            return CustomerProductDL.GetLotNoByBrandPK(pk, lotNo, bizUnit, active, hasSpec);
        }
        public static DataSet GetCustomer(int pk, string name, int bizUnit, int? active = null, int? customerType = null)
        {
            return CustomerProductDL.GetCustomer(pk, name, bizUnit, active, customerType);
        }
        public static DataSet GetBrandList(string searchKey, int CustomerID, int BrandID)
        {
            return CustomerProductDL.GetBrandList(searchKey, CustomerID, BrandID);
        }
        public static DataSet GetBrandProduct(string searchKey, int itemPK, int BrandID)
        {
            return CustomerProductDL.GetBrandProduct(searchKey, itemPK, BrandID);
        }   /// <summary>
        /// Get Mail QueuePaty
        /// </summary>
        /// <param name="partyType"></param>
        /// <returns></returns>
        public static DataSet GetMailQParty(int partyType)
        {
            return CustomerProductDL.GetMailQParty(partyType);
        }
        public static DataSet GetCustomerAddress(int pk, int custPK, int active, int? addressType = null)
        {
            return CustomerProductDL.GetCustomerAddress(pk, custPK, active, addressType);
        }
        public static DataSet GetcommissionAgent(string XML, int active, int BIZUNIT)
        {
            return CustomerProductDL.GetcommissionAgent(XML, active, BIZUNIT);
        }
        public static DataTable GetCustomerTerms(int termPK, int custPK, int typeValue, int active, int? IsInvoice = null)
        {
            return CustomerProductDL.GetCustomerTerms(termPK, custPK, typeValue, active, IsInvoice);
        }
        public static DataTable GetArtWork(int artPK, int brandPK, int active, string artwork)
        {
            return CustomerProductDL.GetArtWork(artPK, brandPK, active, artwork);
        }

        public static DataTable GetCBMWeight(int CIM_PK, int APS_PK, int PIM_PK, double QTY)
        {
            return CustomerProductDL.GetCBMWeight(CIM_PK, APS_PK, PIM_PK, QTY);
        }
        /// <summary>
        /// To get the List of Brands against Customer for bulk delete
        /// </summary>      
        public static DataSet GetCustomerBrandList(GridPrams grid, User objUser, int cusID, string brandName, int bizunit)
        {
            return CustomerProductDL.GetCustomerBrandList(grid, objUser, cusID, brandName,bizunit);
        }
        /// <summary>
        /// 
        /// </summary>      
        public static int GetBrandDeleteList(string xmlDoc)
        {
            return CustomerProductDL.GetBrandDeleteList(xmlDoc);
        }

       

    }
}
