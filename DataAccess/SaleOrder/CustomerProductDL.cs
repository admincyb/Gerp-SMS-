using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.SaleOrder
{
    public class CustomerProductDL
    {
        /// <summary>
        /// Get GetBrand Details by PK
        /// </summary>
        /// <param name="brandPK"></param>
        /// <returns></returns>
        public static DataTable GetBrandDetails(int brandPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.CIM_PK, brandPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_BRAND_DETAILS, colParameters).Tables[0];
        }
        public static DataSet GetBrandBySC(int shpPK, int scPK, string brand, int bizUnit, int active, bool? hasSpec = false)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, scPK),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SNH_PK, shpPK),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.BRAND_NAME  , brand == string.Empty ? "%" : brand + "%")
            };
            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_BRANDSBYSC, colParameters);
            return dsDesig;
        }
        public static DataSet GetLotNoByLoadPk(int pk, string lotNo, int bizUnit, int active, bool? hasSpec = false)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_LPD_PK, pk),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_LPD_LOT_NO  , lotNo == string.Empty ? "%" : lotNo + "%")
            };
            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_LOTNOBYLOADPLANPK, colParameters);
            return dsDesig;
        }
        public static DataSet GetLotNoBySphpPlanPk(int pk, string lotNo, int bizUnit, int active, bool? hasSpec = false)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SNH_PK, pk),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SND_PK, (object) DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SOD_LOT_NO  , lotNo == string.Empty ? "%" : lotNo + "%")
            };
            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_LOTNOBYSHPPLANPK, colParameters);
            return dsDesig;
        }
        public static DataSet GetLotNoByBrandPK(int pk, string lotNo, int bizUnit, int active, bool? hasSpec = false)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CIM_PK, pk),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SNH_PK, (object) DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SND_PK, (object) DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_SOD_LOT_NO  , lotNo == string.Empty ? "%" : lotNo + "%")
            };
            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_LOTNOBYSHPPLANPK, colParameters);
            return dsDesig;
        }
        public static DataSet GetCustomerProduct(int pk, int itemPK, int cusPK, string brand, string item, int bizUnit, int active, bool? hasSpec = false)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.CIM_PK, pk),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.ITM_PK , itemPK == 0? (object)DBNull.Value :itemPK ),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.CUST_PK , cusPK == 0? (object)DBNull.Value :cusPK ),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.BRAND_NAME  , brand == string.Empty ? "%" : System.Web.HttpUtility.HtmlEncode(brand) + "%"),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.ITM_NAME  , item == string.Empty ? "%" : item + "%"),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.ITM_FLAG, (hasSpec.HasValue ? hasSpec.Value ? 1 : 0 : 0)),
            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_CUSTOMERPRODUCT, colParameters);
            return dsDesig;
        }

        public static DataSet GetCustomerProductAuto(int cusPK, string brand, int bizUnit, int active, bool? hasSpec = false)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.CUST_PK , cusPK == 0? (object)DBNull.Value :cusPK ),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.BRAND_NAME  , brand == string.Empty ? "%" : System.Web.HttpUtility.HtmlEncode(brand) + "%"),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.ITM_FLAG, (hasSpec.HasValue ? hasSpec.Value ? 1 : 0 : 0)),
            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, "SPCRM_CUST_ITEM_MAP_SO_AUTO", colParameters);
            return dsDesig;
        }

        public static DataSet GetCustomer(int pk, string name, int bizUnit, int? active = null, int? customerType = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active.HasValue? active : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.CUS_NAME  , name == string.Empty ? "%" : name + "%"),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.CUST_PK, pk),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CUS_TYPE, customerType.HasValue ?customerType<0?(object)DBNull.Value:customerType :(object)DBNull.Value),
            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_CUSTOMER, colParameters);
            return dsDesig;
        }
        public static DataSet GetBrandList(string Name, int CustomerID, int BrandID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                                
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.CUS_NAME  , Name == string.Empty ? "%" : Name + "%"), 
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.CUST_PK , CustomerID == 0? (object)DBNull.Value :CustomerID ),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.BRH_PK,BrandID == 0? (object)DBNull.Value :BrandID )
            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_CUSTOMERBRANDLIST, colParameters);
            return dsDesig;
        }
        public static DataSet GetBrandProduct(string Name, int itemPK, int BrandID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                                
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.ITM_NAME  , Name == string.Empty ? "%" : Name + "%"), 
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.ITM_PK , itemPK == 0? (object)DBNull.Value :itemPK ),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.BRH_PK,BrandID == 0? (object)DBNull.Value :BrandID )
            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_ITMBRANDLIST, colParameters);
            return dsDesig;
        }


        /// <summary>
        /// Get Mail Queue Party 
        /// </summary>
        /// <param name="partyType"></param>
        /// <returns></returns>
        public static DataSet GetMailQParty(int partyType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {   
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.MLQ_PARTY_TYPE,partyType),
            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_MAILQ_PARTY, colParameters);
            return dsDesig;
        }

        public static DataSet GetCustomerAddress(int pk, int custPK, int active, int? addressType = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.CAD_PK  , pk),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.CUST_PK, custPK > 0 ? custPK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.CAD_TYPE, addressType.HasValue && addressType.Value > 0 ? addressType : (object)DBNull.Value),
            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETCUSTOMERADDRESS, colParameters);
            return dsDesig;
        }



        public static DataSet GetcommissionAgent(string XML, int active, int BIZUNIT)
        {
            DataSet dsDesig = new DataSet();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                 {     
                     new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
                      new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, BIZUNIT),
                      new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACD_PK, DBNull.Value.ToString()),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML,  XML==string.Empty? DBNull.Value.ToString():XML),
                 };

            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_AGENT_CUSTOMER_MAP_GET_KV, colParameters);
            return dsDesig;

        }
        public static DataTable GetCustomerTerms(int termPK, int custPK, int typeValue, int active, int? IsInvoice = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.TCH_PK, termPK),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.TYPE_VALUE, typeValue>0?typeValue:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.TCH_CUSTOMER, custPK>0?custPK:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_IS_INVOICE , IsInvoice == 0? (object)DBNull.Value :IsInvoice )
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETCUSTOMERTERMS, colParameters).Tables[0];
        }

        public static DataTable GetArtWork(int artPK, int brandPK, int active, string artwork)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CIA_PK, artPK),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.CIM_PK, brandPK > 0 ? brandPK : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CIA_ART_WORK  , string.IsNullOrEmpty(artwork) ? "%" : artwork + "%")
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETARTWORK, colParameters).Tables[0];
        }

        public static DataTable GetCBMWeight(int CIM_PK, int APS_PK, int PIM_PK, double QTY)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CIM_PK, CIM_PK),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_APS_PK, APS_PK > 0 ? APS_PK : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_PIM_PK,PIM_PK > 0 ? PIM_PK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_QTY, QTY),              
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETCBMWEIGHT, colParameters).Tables[0];
        }
        /// <summary>
        /// To get the List of Brands against Customer for bulk delete
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetCustomerBrandList(GridPrams grid, User objUser, int cusID, string brandName, int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber==0?1:grid.PageNumber),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize==0?200: grid.PageSize),             
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CUS_PK,  cusID > 0 ? cusID : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.BRAND_NAME,  brandName == string.Empty ? "%" : brandName + "%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizunit),
            };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPCRM_CUST_ITEM_MAP_BD_GET_LIST, colParameters);
            return dsList;
        }
        /// <summary>
        /// Delete Brand
        /// </summary>
        /// <param name="employeeAttendance"></param>        
        /// <returns>int</returns>     
        public static int GetBrandDeleteList(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPCRM_CUST_ITEM_MAP_BUL_DEL, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

      
    }
}
