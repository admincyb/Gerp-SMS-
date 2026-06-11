using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities;
using BusinessObject.Constants;
using BusinessObject;


namespace DataAccess.Shipping
{
    public class ShippingPlanDL
    {

        /// Get Shipping Plan List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetShippingPlanList(GridPrams grid, User objUser, int spPk, int active, int Status, int cusPK, int soPk, int PNO, int PSize, string PlanNo, string SCNo, string CustPoNo, string DONo, int Hide_Draft = 0,int cartnAllocStatus=-1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                         
              new DBService.Parameters(ShippingPlanDA.P_BIZUNIT, objUser.SBUID),              
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? "%" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? "%" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(ShippingPlanDA.SER_NAME,"SNH_NO"),
              new DBService.Parameters(ShippingPlanDA.PLAN_NO,PlanNo==string.Empty?(object)DBNull.Value:PlanNo),
              new DBService.Parameters(ShippingPlanDA.SC_NO,SCNo==string.Empty?(object)DBNull.Value:SCNo),
              new DBService.Parameters(ShippingPlanDA.CUSTPO_NO,CustPoNo==string.Empty?(object)DBNull.Value:CustPoNo),
              new DBService.Parameters(ShippingPlanDA.DO_NO,DONo==string.Empty?(object)DBNull.Value:DONo),
              //new DBService.Parameters(ShippingPlanDA.P_SNH_PK,  spPk == 0 ? (object) DBNull.Value :  spPk),
              //new DBService.Parameters(ShippingPlanDA.P_ACTIVE,  active == 0 ? (object) DBNull.Value :  active),
              new DBService.Parameters(ShippingPlanDA.STATUS,  Status == -1 ? (object) DBNull.Value :  Status),
              new DBService.Parameters(ShippingPlanDA.CUSTOMER,  cusPK == 0 ? (object) DBNull.Value :  cusPK),
              new DBService.Parameters(ShippingPlanDA.P_PAGE_NO,  PNO == 0 ? (object) DBNull.Value :  PNO),
              new DBService.Parameters(ShippingPlanDA.P_PAGE_SIZE,  PSize == 0 ? (object) DBNull.Value :  PSize),
              new DBService.Parameters(ShippingPlanDA.SOPK,  soPk == 0 ? (object) DBNull.Value :  soPk),
              new DBService.Parameters(ShippingPlanDA.P_USER_PK, objUser.PKUser),  
              new DBService.Parameters(ShippingPlanDA.P_HIDE_DRAFT,Hide_Draft), 
              new DBService.Parameters(ShippingPlanDA.P_SNH_COMPANY,grid.CompanyPK>0?grid.CompanyPK:(object)DBNull.Value),
              new DBService.Parameters(ShippingPlanDA.P_CDR_ALLOC_STATUS,  cartnAllocStatus == -1 ? (object) DBNull.Value :  cartnAllocStatus),
             
            };

            DataSet dsSPList = new DataSet();
            dsSPList = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SP_GetShippingPlanList, colParameters);
            return dsSPList;
        }
        /// Get Shipping Plan Details 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetShippingPlanDetails(int spdPk, int spPk, int active, string itemslist = "")
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                         
              new DBService.Parameters(ShippingPlanDA.P_SND_PK, spdPk == 0 ? (object) DBNull.Value :  spdPk),              
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : grid.SearchValue+"%"),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? "%" : grid.SortBy),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? "%" : grid.ThenBy),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? "%" : grid.SortDirection),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? "%" : grid.ThenDirection),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(ShippingPlanDA.P_SNH_PK,  spPk == 0 ? (object) DBNull.Value :  spPk),
              new DBService.Parameters(ShippingPlanDA.P_ACTIVE,  active == 0 ? (object) DBNull.Value :  active),
              new DBService.Parameters(ShippingPlanDA.P_SOD_PK, itemslist == string.Empty ? (object) DBNull.Value :  itemslist)             
            };

            DataSet dsSPDList = new DataSet();
            dsSPDList = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SP_GetShippingPlanDetails, colParameters);
            return dsSPDList;
        }
        /// <summary>
        /// Save Summary 
        /// </summary>
        /// <param name="snhPK"></param>
        /// <param name="processPK"></param>
        /// <returns></returns>
        public static int SaveSummary(int snhPK,int processPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(ShippingPlanDA.P_SNH_PK,  snhPK ),
                new DBService.Parameters(ShippingPlanDA.P_SNH_PROCESS,  processPK ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ShippingPlanDA.SP_SHIPPING_PLAN_SUMMARY_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// Get Shipping Plan Details 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataTable GetShippingPlanWeightDetails(int soPk, string itemCode, double qty,int sodPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {           
              new DBService.Parameters(ShippingPlanDA.P_SOH_PK, soPk == 0 ? (object) DBNull.Value :  soPk),
              new DBService.Parameters(ShippingPlanDA.P_SOD_PK, sodPk == 0 ? (object) DBNull.Value :  sodPk),
              new DBService.Parameters(ShippingPlanDA.P_ITM_CODE,string.IsNullOrEmpty(itemCode) ? (object) DBNull.Value : itemCode),
              new DBService.Parameters(ShippingPlanDA.P_SOD_QTY,  qty == 0 ? (object) DBNull.Value :  qty)                         
            };
        
            return dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_ORDER_DTL_LIST_GET, colParameters).Tables[0];            
        }

        /// Get Shipping Plan Details 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetShippingOrderList(string Xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
              new DBService.Parameters(ShippingPlanDA.P_XML, Xml)              
            };
            DataSet dsSPOList = new DataSet();
            dsSPOList = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SP_GetShippingOrders, colParameters);
            return dsSPOList;
        }

        /// Save Shipping Plan
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static List<object> SaveShippingPlan(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(ShippingPlanDA.P_XML, xmlDoc),  
                new DBService.Parameters(ShippingPlanDA.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(ShippingPlanDA.P_RET_NO, string.Empty, 4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ShippingPlanDA.SP_SaveShippingPlan, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ShippingPlanDA.RETVAL]).Value);
            string transNo = ((IDataParameter)dbService.oCommand.Parameters[ShippingPlanDA.P_RET_NO]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(transNo);
            return retvals;
        }

        /// Get Shipping Plan HDR
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetShippingPlanHDR(User objUser, int spPk, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                         
              new DBService.Parameters(ShippingPlanDA.P_BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(ShippingPlanDA.P_SNH_PK,  spPk == 0 ? (object) DBNull.Value :  spPk),
              new DBService.Parameters(ShippingPlanDA.P_ACTIVE,  active == 0 ? (object) DBNull.Value :  active)
             
            };

            DataSet dsSPList = new DataSet();
            dsSPList = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SP_GetShippingPlan, colParameters);
            return dsSPList;
        }

        /// Get Order Tracker List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetOrderTrackerList(int SOHPk, int CusID, string FromDate, string ToDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                         
              new DBService.Parameters(ShippingPlanDA.SOHPK, SOHPk == 0 ? (object) DBNull.Value :  SOHPk),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(ToDate)),
              new DBService.Parameters(ShippingPlanDA.CUSPK,  CusID == 0 ? (object) DBNull.Value :  CusID)
            };

            DataSet dsOrderTrackerList = new DataSet();
            dsOrderTrackerList = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SP_GetOrderTracker, colParameters);
            return dsOrderTrackerList;
        }

        /// Get Shipping Plan Status
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetShippingPlanStatus(int SNHPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {             
              new DBService.Parameters(ShippingPlanDA.P_SNH_PK,  SNHPk == 0 ? (object) DBNull.Value :  SNHPk)
            };

            DataSet dsShippingStatusList = new DataSet();
            dsShippingStatusList = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SP_ShippingStatusList, colParameters);
            return dsShippingStatusList;
        }

        /// Get Questionnaire
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetQuestionnaire(int QSTPK, int Status)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {             
              new DBService.Parameters(ShippingPlanDA.QSTPK,  QSTPK == 0 ? (object) DBNull.Value :  QSTPK),
              new DBService.Parameters(ShippingPlanDA.P_ACTIVE,  Status == 0 ? (object) DBNull.Value :  Status)
            };

            DataSet dsQList = new DataSet();
            dsQList = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SP_Questionnaire, colParameters);
            return dsQList;
        }
        /// <summary>
        /// Get plan information
        /// </summary>
        /// <param name="shippingPK"></param>
        /// <returns></returns>
        public static DataSet GetPlanInfo(int shippingPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {             
              new DBService.Parameters(ShippingPlanDA.P_SHIPPING_PLAN,  shippingPK)
            };

            DataSet dsQList = new DataSet();
            dsQList = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SPSHIPPING_PLAN_INFO_GET, colParameters);
            return dsQList;
        }

        /// Get QuestionnaireData
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetQuestionnaireData(string SP, string P_XML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {             
              new DBService.Parameters(ShippingPlanDA.P_XML, P_XML)        
            };

            DataSet dsQListData = new DataSet();
            dsQListData = dbService.DataAdapter(CommandType.StoredProcedure, SP, colParameters);
            return dsQListData;
        }

        /// Get Commerical Invoice
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataTable GetCommericalInvoice(int SPPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(ShippingPlanDA.P_SNH_PK,  SPPK == 0 ? (object) DBNull.Value :  SPPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SP_GetCommericalInvoice, colParameters).Tables[0];
        }

        public static DataTable GetSaleOrderHdrByShippingPlanPK(int shippingPlanPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {             
              new DBService.Parameters(ShippingPlanDA.P_SNH_PK,  shippingPlanPK == 0 ? (object) DBNull.Value :  shippingPlanPK)
            };

            return dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_SHIPPING_PLAN_SO_GET, colParameters).Tables[0];
        }

        public static int ShippingPlanCancelCheck(int ShippingPlanPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {             
              new DBService.Parameters(ShippingPlanDA.P_SNH_PK,  ShippingPlanPk == 0 ? (object) DBNull.Value :  ShippingPlanPk),
              new DBService.Parameters(ShippingPlanDA.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            //DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_SHIPPING_PLAN_CANCEL_CHECK, colParameters);
            //return dsArchive == null || dsArchive.Tables.Count == 0 || dsArchive.Tables[0].Rows.Count == 0;
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_SHIPPING_PLAN_CANCEL_CHECK, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ShippingPlanDA.RETVAL]).Value);
            return result;
        }

        public static bool ShippingPlanAlreadyCreatedCheck(int SoPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {             
              new DBService.Parameters(ShippingPlanDA.SOHPK,  SoPk == 0 ? (object) DBNull.Value :  SoPk)
            };

            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_ORDER_SHIP_PLAN_CHECK, colParameters);
            return dsArchive != null && dsArchive.Tables.Count > 0 && dsArchive.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Get Shipping plan receipt report
        /// </summary>
        /// <param name="shippingPK"></param>
        /// <returns>Dataset</returns>
        public static DataSet GetShippingPlanReceiptReport(int shippingPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {             
              new DBService.Parameters(ShippingPlanDA.SNH_PK,  shippingPK)
            };
            DataSet dsQList = new DataSet();
            dsQList = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_SHIPPING_PLAN_INVOICE_RPT, colParameters);
            return dsQList;
        }

        /// <summary>
        /// Get Previous saved Company for shipping plan of each shipping level
        /// </summary>
        /// <param name="shippingPK"></param>
        /// <returns>Dataset</returns>
        public static int GetPrevCompany(int shippingPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {             
              new DBService.Parameters(ShippingPlanDA.SNH_PK,  shippingPK)              
            };
            int result = Convert.ToInt32(dbService.ExecuteScalar(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_GET_PREV_COMPANY, colParameters));
            return result;
        }

        public static int UpdateProductStockDetails(int DesptchPk, int Module, int Mode, int? From_ERP = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(ShippingPlanDA.P_TRX_PK, DesptchPk),  
                new DBService.Parameters(ShippingPlanDA.P_MODULE, Module),  
                new DBService.Parameters(ShippingPlanDA.P_MODE, Mode),  
                new DBService.Parameters(ShippingPlanDA.P_FROM_ERP, From_ERP == 0 ? (object) DBNull.Value :  From_ERP),  
                new DBService.Parameters(ShippingPlanDA.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ShippingPlanDA.SPPRD_PLN_PRODUCT_STK_TRX_UPDATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ShippingPlanDA.RETVAL]).Value);
            return result;
        }


        public static int UpdateModifyDODetails(int DesptchPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(ShippingPlanDA.DPH_PK ,  DesptchPk == 0 ? (object) DBNull.Value :  DesptchPk  ),  
                new DBService.Parameters(ShippingPlanDA.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_DESPATCH_UPDATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ShippingPlanDA.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Is CrDrNote Exist
        /// </summary>
        /// <param name="DesptchPk"></param>
        /// <returns></returns>
        public static int IsCrDrNoteExist(int DesptchPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(ShippingPlanDA.DPH_PK ,  DesptchPk == 0 ? (object) DBNull.Value :  DesptchPk  ),
                new DBService.Parameters(ShippingPlanDA.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ShippingPlanDA.SPFIN_INVOICE_CUS_CRDR_EXISTS_CHECK, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ShippingPlanDA.RETVAL]).Value);
            return result;
        }
        

        public static int DeleteShippingPlan(int ShippingPlanId, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(ShippingPlanDA.P_SNH_PK, ShippingPlanId),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(ShippingPlanDA.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ShippingPlanDA.SP_DeleteShippingPlan, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ShippingPlanDA.RETVAL]).Value);
            return result;
        }

        public static DataTable GetCustomerSaleOrder(User objUser, int soPk, int active, int cusPk, int snhPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {             
              new DBService.Parameters(ShippingPlanDA.P_BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(ShippingPlanDA.P_SOH_PK,  soPk == 0 ? (object) DBNull.Value :  soPk),
              new DBService.Parameters(ShippingPlanDA.P_ACTIVE,  active == 0 ? (object) DBNull.Value :  active),
              new DBService.Parameters(ShippingPlanDA.P_SOH_CUSTOMER, cusPk),
              new DBService.Parameters(ShippingPlanDA.P_SNH_PK, snhPk == 0 ? (object) DBNull.Value :snhPk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_ORDER_CUSTOMER_GET, colParameters).Tables[0];
        }

        public static DataTable GetCustomerShippingBrands(User objUser, int ScPk, int SnhPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {             
              //new DBService.Parameters(ShippingPlanDA.P_BIZUNIT, objUser.SBUID),              
              //new DBService.Parameters(ShippingPlanDA.P_SOH_PK,  soPk == 0 ? (object) DBNull.Value :  soPk),
              //new DBService.Parameters(ShippingPlanDA.P_ACTIVE,  active == 0 ? (object) DBNull.Value :  active), P_SNH_PK
              //new DBService.Parameters(ShippingPlanDA.P_SOH_CUSTOMER, cusPk)
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, ScPk),
              new DBService.Parameters(GTIService.Constants.Shipping.Parameters.P_SNH_PK, SnhPk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_ORDER_SHIPPINGBRAND_GET, colParameters).Tables[0];
        }

        #region Check validations for Pick for Multiple SC
        /// <summary>
        /// Check for valid SC
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int CheckforValidShippingSO(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ShippingPlanDA.SPCHECKVALIDSHIPPINGSOs, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        #endregion

        public static bool IsContainerReleaseCartonExist(int ShippingPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(ShippingPlanDA.P_SNH_PK, ShippingPk),
                new DBService.Parameters(ShippingPlanDA.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_CONTAINER_RELEASE_CARTON_EXISTS, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ShippingPlanDA.RETVAL]).Value);
            if (result > 0)
                return true;
            else
                return false;
        }
        public static DataSet GetPackingMaterialInShippingPlan(int SNHPk,int SubType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(ShippingPlanDA.P_SNH_PK,  SNHPk == 0 ? (object) DBNull.Value :  SNHPk),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_AST_VALUE,  SubType)

            };

            DataSet dsShippingList = new DataSet();
            dsShippingList = dbService.DataAdapter(CommandType.StoredProcedure, ShippingPlanDA.SPSAL_SHIPPING_PLAN_PACK_LIST_GET_RPT, colParameters);
            return dsShippingList;
        }
        public static DataTable GetBOIStatusList(string CfgType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {

                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CfgPK , (object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE , Active),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT , sBU),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CFG_TYPE , CfgType)
             };
            DataTable dtBOIStatus = new DataTable();
            dtBOIStatus = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPADM_CONFIG_MST_GET_KV, colParameters).Tables[0];
            return dtBOIStatus;
        }

    }
}

