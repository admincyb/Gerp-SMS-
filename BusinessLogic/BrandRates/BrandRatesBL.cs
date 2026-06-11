using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.BrandRates;
using BusinessObject.BrandRate;
using System.Web.Script.Serialization;
using GTIService;
using BusinessObject;

namespace BusinessLogic.BrandRates
{
    public class BrandRatesBL
    {
        /// <summary>
        /// Get Customer List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetCustomerList(int bizUnit, int active, int specialCatId = 0)
        {
            return BrandRatesDL.GetCustomerList(bizUnit, active, specialCatId);
        }
        /// <summary>
        /// Get Product List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetProductList(int bizUnit, int category, int active)
        {
            return BrandRatesDL.GetProductList(bizUnit, category, active);
        }
        /// <summary>
        /// Get Product Details
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetProductDetail(string pXML)
        {
            return BrandRatesDL.GetProductDetail(pXML);
        }

        /// <summary>
        /// Get Customer Details
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetCustomerDetail(string pXML)
        {
            return BrandRatesDL.GetCustomerDetail(pXML);
        }
        /// <summary>
        /// Delete Reocrd
        /// </summary>
        /// <param name="brhPK"></param>
        /// <returns></returns>
        public static int DeleteRecord(int brhPK)
        {
            return BrandRatesDL.DeleteRecord(brhPK);
        }
        /// <summary>
        /// Get Brand Rate
        /// </summary>
        /// <param name="brandPK"></param>
        /// <param name="custPK"></param>
        /// <param name="itemPK"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataTable GetBrandRate(int brandPK, int custPK, int itemPK, DateTime date)
        {
            return BrandRatesDL.GetBrandRate(brandPK, custPK, itemPK, date);
        }
        /// <summary>
        /// Get selected Customers Rate
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataSet GetCustomerRate(string pXML)
        {
            return BrandRatesDL.GetCustomerRate(pXML);
        }

        /// <summary>
        /// Save Brands Rates
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int  SaveBrandsRates(string pXML)
        {
            return BrandRatesDL.SaveBrandsRates(pXML);
        }


        /// <summary>
        /// Save Brands Rates
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int BrandItemInsert(int UserPk,int bizunit,int Active,int CIM_PK, string CIM_BRAND_CODE, string CIM_BRAND_NAME, string CIM_CUSTOMER_PK)
        {
            return BrandRatesDL.BrandItemInsert(UserPk,bizunit,Active,CIM_PK, CIM_BRAND_CODE, CIM_BRAND_NAME, CIM_CUSTOMER_PK);
        }


        /// <summary>
        /// Get selected Brand Product Rate
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        //public static DataSet GetProductRate(string pXML)
        //{
        //    return BrandRatesDL.GetProductRate(pXML);
        //}


        public static CustomerRateNewBO GetProductRate(string pXML)
        {
            try
            {
                CustomerRateNewBO productRateObj = new CustomerRateNewBO();
                string PorductRate = DataAccess.BrandRates.BrandRatesDL.GetProductRate(pXML);
                if (PorductRate != string.Empty)
                {
                    productRateObj = (CustomerRateNewBO)CommonFunctions.DeserializeObject(PorductRate, productRateObj);

                    

                    return productRateObj;
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

        /// <summary>
        /// Get Brand Product Rate History
        /// </summary>
        /// <param name="brandPk"></param>
        /// <returns></returns>
        public static DataSet GetBrandProductRateHistory(int brandPk)
        {
            return BrandRatesDL.GetBrandProductRateHistory(brandPk);
        }




        /// <summary>
        /// Get Brand Rate History
        /// </summary>
        /// <param name="brandPk"></param>
        /// <returns></returns>
        public static DataSet GetBrabdRateHistory(int brandPk)
        {
            return BrandRatesDL.GetBrabdRateHistory(brandPk);
        }
        /// <summary>
        /// Get Brand Details
        /// </summary>
        /// <param name="brhPK"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetBratndDetails(int brhPK, DateTime fromDate, DateTime toDate, int bizUnit, int active)
        {
            return BrandRatesDL.GetBratndDetails(brhPK, fromDate, toDate, bizUnit, active);
        }
        /// <summary>
        /// Save Brand Details
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveBrandDetails(string pXML)
        {
            return BrandRatesDL.SaveBrandDetails(pXML);
        }
        /// <summary>
        /// Import Customer Brands
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int ImportCustomerBrands(string pXML, string SP_Name,int sbu, ref DataTable dtOut)
        {
            return BrandRatesDL.ImportCustomerBrands(pXML, SP_Name,sbu, ref dtOut);
        }

        /// <summary>
        /// Copy Brand details
        /// </summary>
        /// <param name="objBrand"></param>
        /// <returns></returns>
        public static int CopyBrandDetails(CustomerRateCopyBO objBrand)
        {
            return BrandRatesDL.CopyBrandDetails(objBrand);
        }

        /// <summary>
        /// Get Brand Price List
        /// </summary>
        public static DataTable GetBrandPriceList(int CusPk, string fromDt, string toDt)
        {
            return BrandRatesDL.GetBrandPriceList(CusPk, fromDt, toDt);
        }


        public static DataSet GetCustomerLatestRate(string pXML = "", int disableDtls = 0)
        {
            return BrandRatesDL.GetCustomerLatestRate(pXML, disableDtls);

        }
        /// <summary>
        /// Get Customer Details By Currency
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetCustomerListByCurrency(int bizUnit, int active, int currencyId, int specialCatId = 0)
        {
            return BrandRatesDL.GetCustomerListByCurrencyId(bizUnit, active, currencyId, specialCatId);
        }

        /// <summary>
        /// Get Product Properties
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetProductProperties(int bizUnit, int groupType, int groupValue)
        {
            int active = 1;
            return BrandRatesDL.GetProductProperties(bizUnit, groupType, groupValue, active);
        }

        /// <summary>
        /// Get Product List By Properties
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetProductListByProperties(int bizUnit, int category, int active,
            int typeId, int thicknessId, int categoryId, int surfaceId, int shadeId, int classificationId, int sizeId,
            int lengthId, int chlorinationId, int sideId, int adnlSpec05, int adnlSpec06, int adnlSpec07)
        {
            return BrandRatesDL.GetProductList(bizUnit, category, active,
                typeId, thicknessId, categoryId, surfaceId, shadeId, classificationId, sizeId, lengthId, chlorinationId, sideId, adnlSpec05, adnlSpec06, adnlSpec07);
        }

        public static DataTable FilterProductListByProperties(string pXML)
        {
            return BrandRatesDL.FilterProductListByProperties(pXML);
        }

    }
}


