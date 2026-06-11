using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Utilities;
using DataAccess;
using BusinessObject.Constants;
using BusinessObject;
using ERP.Utilities.Constants;
using BusinessObject.BrandRate;


namespace DataAccess.BrandRates
{
    public class BrandRatesDL
    {
        /// <summary>
        /// Get Customers List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetCustomerList(int bizUnit, int active, int specialCatId = 0)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(BrandRatesDA.P_BIZUNIT, bizUnit),
                new DBService.Parameters(BrandRatesDA.P_ACTIVE, active),
                new DBService.Parameters(BrandRatesDA.P_CUS_SPECIAL_CAT,specialCatId > 0 ? specialCatId : (object)DBNull.Value)
               
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_GetCustomers, colParameters);
            }
            return ds.Tables[0];

        }
        /// <summary>
        /// Get Product List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetProductList(int bizUnit, int category, int active,
            int typeId = 0, int thicknessId = 0, int categoryId = 0, int surfaceId = 0, int shadeId = 0, int classificationId = 0,
            int sizeId = 0, int lengthId = 0, int chlorinationId = 0, int sideId = 0, int adnlSpec05 = 0, int adnlSpec06 = 0, int adnlSpec07 = 0)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(BrandRatesDA.P_BIZUNIT, bizUnit),
                new DBService.Parameters(BrandRatesDA.ITM_CATEGORY, category),
                new DBService.Parameters(BrandRatesDA.ITEM_ACTIVE, active)                
                };
                if (typeId != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_NATURE, typeId));
                }
                if (thicknessId != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_THICKNESS, thicknessId));
                }
                if (categoryId != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_PROCESS, categoryId));
                }
                if (surfaceId != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_SURFACE, surfaceId));
                }
                if (shadeId != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_COLOUR, shadeId));
                }
                if (classificationId != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_GRADE, classificationId));
                }
                if (sizeId != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_SIZE, sizeId));
                }
                if (lengthId != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_LENGTH, lengthId));
                }
                if (chlorinationId != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_CHLORINATION, chlorinationId));
                }
                if (sideId != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_ADNL_SPEC01, sideId));
                }
                if (adnlSpec05 != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_ADNL_SPEC05, adnlSpec05));
                }
                if (adnlSpec06 != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_ADNL_SPEC06, adnlSpec06));
                }
                if (adnlSpec07 != 0)
                {
                    ResizeArray(ref colParameters, new DBService.Parameters(BrandRatesDA.P_ISD_ADNL_SPEC07, adnlSpec07));
                }
                ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_Products, colParameters);
            }
            return ds.Tables[0];

        }

        public static DataTable FilterProductListByProperties(string pXML)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(BrandRatesDA.P_XML, pXML)
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_FilterProductDetails, colParameters);
            }
            return ds.Tables[0];
        }



        /// <summary>
        /// Get Product Details
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataTable GetProductDetail(string pXML)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(BrandRatesDA.P_XML, pXML)
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_ProductDetails, colParameters);
            }
            return ds.Tables[0];

        }
        /// <summary>
        /// Get Rate for Latest Transaction
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataSet GetCustomerLatestRate(string pXML = "", int disableDtls = 0)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(BrandRatesDA.P_XML, String.IsNullOrEmpty(pXML)? (object)DBNull.Value:pXML),
                new DBService.Parameters(BrandRatesDA.P_DISABLE_DTLS, disableDtls)
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_CusRateLatest, colParameters);
            }
            return ds;
        }
        /// <summary>
        /// Get Customer Detail
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataTable GetCustomerDetail(string pXML)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(BrandRatesDA.P_XML, pXML)
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_CustomerDetails, colParameters);
            }
            return ds.Tables[0];

        }
        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="brhPK"></param>
        /// <returns></returns>
        public static int DeleteRecord(int brhPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(BrandRatesDA.P_BRH_PK, brhPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, BrandRatesDA.SP_DeleteBrandDetails, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Get selected Customers Rate
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataSet GetCustomerRate(string pXML)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(BrandRatesDA.P_XML, pXML)
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_GetCustomerRate, colParameters);
            }
            return ds;
        }



        public static int SaveBrandsRates(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RETR_VAL, 0,DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, BrandRatesDA.SP_SaveBrandsRates, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }




        public static int BrandItemInsert(int UserPk, int bizunit, int Active, int CIM_PK, string CIM_BRAND_CODE, string CIM_BRAND_NAME, string CIM_CUSTOMER_PK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                  new DBService.Parameters(BrandRatesDA.P_BIZUNIT, bizunit),
                  new DBService.Parameters(BrandRatesDA.P_USER_PK, UserPk),
                  new DBService.Parameters(BrandRatesDA.P_ACTIVE, Active),
                  new DBService.Parameters(BrandRatesDA.P_CIM_PK, CIM_PK > 0 ? CIM_PK : (object)DBNull.Value),
                  new DBService.Parameters(BrandRatesDA.P_CIM_BRAND_CODE, CIM_BRAND_CODE!=string.Empty? CIM_BRAND_CODE:(object)DBNull.Value),
                  new DBService.Parameters(BrandRatesDA.P_CIM_BRAND_NAME, CIM_BRAND_NAME!=string.Empty? CIM_BRAND_NAME:(object)DBNull.Value),
                  new DBService.Parameters(BrandRatesDA.P_CIM_CUSTOMER, CIM_CUSTOMER_PK!=string.Empty? CIM_CUSTOMER_PK:(object)DBNull.Value),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RET_VAL, 0,DBService.ParameterType.Number)
        };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, BrandRatesDA.SP_BrandItemInsert, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.P_RET_VAL]).Value);
            return result;
        }




        /// <summary>
        /// Get selected Brand Product Rate
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        //public static DataSet GetProductRate(string pXML)
        //{
        //    DataSet ds = null;
        //    {
        //        DBService dbService = new DBService();
        //        DBService.Parameters[] colParameters = null;
        //        colParameters = new DBService.Parameters[] 
        //        { 
        //        new DBService.Parameters(BrandRatesDA.P_XML, pXML)
        //        };
        //        ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_GetProductRate, colParameters);
        //    }
        //    return ds;
        //}


        public static string GetProductRate(string pXML)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                 new DBService.Parameters(BrandRatesDA.P_XML, pXML)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_GetProductRate, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }



        /// <summary>
        /// Get Brand Rate History
        /// </summary>
        /// <param name="brandPK"></param>
        /// <returns></returns>
        public static DataSet GetBrandProductRateHistory(int brandPK)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(BrandRatesDA.P_ITEM_PK, brandPK)
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_GetBrandpRroductRateHistory, colParameters);
            }
            return ds;
        }

        /// <summary>
        /// Get Brand Rate History
        /// </summary>
        /// <param name="brandPK"></param>
        /// <returns></returns>
        public static DataSet GetBrabdRateHistory(int brandPK)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(BrandRatesDA.P_CIM_PK, brandPK)
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_GetBrandRateHistory, colParameters);
            }
            return ds;

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
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(BrandRatesDA.P_PROD_DATE, date),
                new DBService.Parameters(BrandRatesDA.P_BRAND_PK, brandPK > 0 ? brandPK : (object)DBNull.Value),
                new DBService.Parameters(BrandRatesDA.P_CUS_PK, custPK > 0 ? custPK : (object)DBNull.Value),
                new DBService.Parameters(BrandRatesDA.P_ITEM_PK, itemPK > 0 ? itemPK : (object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_GET_BTANDRATE, colParameters).Tables[0];
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
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(BrandRatesDA.P_BRH_PK, brhPK),
                new DBService.Parameters(BrandRatesDA.P_FROM_DATE, fromDate),
                new DBService.Parameters(BrandRatesDA.P_TO_DATE , toDate),
                new DBService.Parameters(BrandRatesDA.P_BIZUNIT, bizUnit),
                new DBService.Parameters(BrandRatesDA.P_ACTIVE, active),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_GetBrandDetails, colParameters).Tables[0];

        }
        /// <summary>
        /// Save Brand Details
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveBrandDetails(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(BrandRatesDA.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, BrandRatesDA.SP_SaveBrandDetails, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Import Customer Brands
        /// </summary>
        /// <param name="pXML"></param>
        ///// <returns></returns>
        //public static DataSet (string pXML)
        //{
        //    DBService dbService = new DBService();
        //    DBService.Parameters[] colParameters = null;
        //    colParameters = new DBService.Parameters[] 
        //     {                
        //         new DBService.Parameters(BrandRatesDA.P_XML, pXML),
        //         new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
        //     };
        //    return dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_ImportCustomerBrands, colParameters);
        //    //int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, BrandRatesDA.SP_ImportCustomerBrands, colParameters);
        //    //int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        //    //return result;
        //}
        /// <summary>
        ///  Import Customer Brands
        /// </summary>
        /// <param name="pXML"></param>
        /// <param name="dtOut"></param>
        /// <returns></returns>
        public static int ImportCustomerBrands(string pXML, string SP_Name,int sbu, ref DataTable dtOut)
        {
            DBService dbService;
            DBService.Parameters[] colParameters;

            dbService = new DBService();
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(BrandRatesDA.P_XML, pXML),
                  new DBService.Parameters(BrandRatesDA.P_BIZUNIT, sbu),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dtOut = dbService.DataAdapterTable(CommandType.StoredProcedure, SP_Name, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }

        /// <summary>
        /// Copy Brand Details
        /// </summary>
        /// <param name="objBrand"></param>
        /// <returns></returns>
        public static int CopyBrandDetails(CustomerRateCopyBO objBrand)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(BrandRatesDA.P_BRH_PK_SRC, objBrand.BrhPK),
                new DBService.Parameters(BrandRatesDA.P_BRH_DATE_FROM, objBrand.DateFrom),
                new DBService.Parameters(BrandRatesDA.P_BRH_DATE_TO, objBrand.DateTo),
                new DBService.Parameters(BrandRatesDA.P_ACTIVE, objBrand.Active),
                new DBService.Parameters(BrandRatesDA.P_BIZUNIT, objBrand.BizUnit),
                new DBService.Parameters(BrandRatesDA.P_USER, objBrand.UserPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, BrandRatesDA.SP_CopyBrandDetails, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }


        public static DataTable GetBrandPriceList(int CusPk, string fromDt, string toDt)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(BrandRatesDA.P_CUS_PK, CusPk > 0 ? CusPk : (object)DBNull.Value),
                new DBService.Parameters(BrandRatesDA.P_FROM_DATE_BR, fromDt!=string.Empty ? fromDt : (object)DBNull.Value),
                new DBService.Parameters(BrandRatesDA.P_TO_DATE_BR, toDt!=string.Empty ? toDt : (object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SPCRM_CUST_ITEM_RATE_HDR_LIST, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Customers List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetCustomerListByCurrencyId(int bizUnit, int active, int currencyId, int specialCatId = 0)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(BrandRatesDA.P_BIZUNIT, bizUnit),
                new DBService.Parameters(BrandRatesDA.P_ACTIVE, active),
                new DBService.Parameters(BrandRatesDA.P_CUR_PK,currencyId),
                new DBService.Parameters(BrandRatesDA.P_CUS_SPECIAL_CAT,specialCatId > 0 ? specialCatId : (object)DBNull.Value)
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SP_GetCustomers, colParameters);
            }
            return ds.Tables[0];

        }

        /// <summary>
        /// Get Product Properties
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetProductProperties(int bizUnit, int groupType, int groupValue, int active)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                    new DBService.Parameters(BrandRatesDA.P_CON_PK,0),
                new DBService.Parameters(BrandRatesDA.P_CON_BIZUNIT, bizUnit),
                new DBService.Parameters(BrandRatesDA.P_CGT_VALUE, groupType),
                new DBService.Parameters(BrandRatesDA.P_CNG_VALUE, groupValue),
                new DBService.Parameters(BrandRatesDA.P_CON_ACTIVE, active)               
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, BrandRatesDA.SPADM_CONST_MST_GET_KV, colParameters);
            }
            return ds.Tables[0];

        }

        /// <summary>
        /// Resize An Existing Array To Hold One More Element
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="newElement"></param>
        /// <returns></returns>
        private static void ResizeArray(ref DBService.Parameters[] arr, DBService.Parameters newElement)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = newElement;
        }
    }
}


