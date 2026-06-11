using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.Shipping
{
    public class ShippingPlanBL
    {
        public static DataSet GetShippingPlanList(GridPrams grid, User objUser, int spPk, int active, int Status, int cusPK, int soPk, int PNO, int PSize, string PlanNo, string SCNo,string CustPoNo, string DONo, int Hide_Draft=0,int cartnAllocStatus=-1)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetShippingPlanList(grid, objUser, spPk, active, Status, cusPK, soPk, PNO, PSize, PlanNo, SCNo,CustPoNo, DONo, Hide_Draft, cartnAllocStatus);
        }
        public static DataSet GetShippingPlanDetails(int spdPk, int spPk, int active, string itemslist = "")
        {
            return DataAccess.Shipping.ShippingPlanDL.GetShippingPlanDetails(spdPk, spPk, active, itemslist);
        }
        public static DataTable GetShippingPlanWeightDetails(int soPk, string itemCode, double qty, int sodPk = 0)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetShippingPlanWeightDetails(soPk, itemCode, qty,sodPk);
        }
        /// <summary>
        /// Save Summary
        /// </summary>
        /// <param name="snhPK"></param>
        /// <param name="processPK"></param>
        /// <returns></returns>
        public static int SaveSummary(int snhPK, int processPK)
        {
            return DataAccess.Shipping.ShippingPlanDL.SaveSummary(snhPK, processPK);
        }
        public static DataSet GetShippingOrderList(string Xml)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetShippingOrderList(Xml);
        }
        public static List<object> SaveShippingPlan(string xmlDoc)
        {
            return DataAccess.Shipping.ShippingPlanDL.SaveShippingPlan(xmlDoc);
        }
        public static DataSet GetShippingPlanHDR(User objUser, int spPk, int active)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetShippingPlanHDR(objUser, spPk, active);
        }
        public static DataSet GetOrderTrackerList(int SOHPk, int CusID, string FromDate, string ToDate)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetOrderTrackerList(SOHPk, CusID, FromDate, ToDate);
        }
        public static DataSet GetShippingPlanStatus(int SNHPk)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetShippingPlanStatus(SNHPk);
        }
        public static DataSet GetQuestionnaire(int QSTPK, int Status)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetQuestionnaire(QSTPK, Status);
        }
        public static DataSet GetPlanInfo(int shippingPK)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetPlanInfo(shippingPK);
        }
        public static DataSet GetQuestionnaireData(string SP, string P_XML)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetQuestionnaireData(SP, P_XML);
        }
        public static DataTable GetCommericalInvoice(int SPPK)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetCommericalInvoice(SPPK);
        }

        public static DataTable GetSaleOrderHdrByShippingPlanPK(int shippingPlanPK)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetSaleOrderHdrByShippingPlanPK(shippingPlanPK);
        }

        public static int ShippingPlanCancelCheck(int ShippingPlanPk)
        {
            return DataAccess.Shipping.ShippingPlanDL.ShippingPlanCancelCheck(ShippingPlanPk);
        }

        public static bool ShippingPlanAlreadyCreatedCheck(int SoPk)
        {
            return DataAccess.Shipping.ShippingPlanDL.ShippingPlanAlreadyCreatedCheck(SoPk);
        }
        public static DataSet GetShippingPlanReceiptReport(int shippingPK)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetShippingPlanReceiptReport(shippingPK);
        }
        public static int GetPrevCompany(int shippingPK)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetPrevCompany(shippingPK);
        }

        public static int UpdateProductStockDetails(int DesptchPk, int Module, int Mode, int? From_ERP = 0)
        {
            return DataAccess.Shipping.ShippingPlanDL.UpdateProductStockDetails(DesptchPk, Module, Mode, From_ERP);
        }

        public static int UpdateModifyDODetails(int DesptchPk)
        {
            return DataAccess.Shipping.ShippingPlanDL.UpdateModifyDODetails(DesptchPk);
        }
        public static int IsCrDrNoteExist(int DesptchPk)
        {
            return DataAccess.Shipping.ShippingPlanDL.IsCrDrNoteExist(DesptchPk);
        }
        public static int DeleteShippingPlan(int ShippingPlanId, DateTime lastModDate)
        {
            return DataAccess.Shipping.ShippingPlanDL.DeleteShippingPlan(ShippingPlanId, lastModDate);
        }

        public static DataTable GetCustomerSaleOrder(User objUser, int soPk, int active, int cusPk, int snhPk)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetCustomerSaleOrder(objUser, soPk, active, cusPk, snhPk);
        }

        public static DataTable GetCustomerShippingBrands(User objUser, int ScPk, int SnhPk)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetCustomerShippingBrands(objUser, ScPk, SnhPk);
        }

        public static int CheckforValidShippingSO(string strxml)
        {
            return DataAccess.Shipping.ShippingPlanDL.CheckforValidShippingSO(strxml);
        }

        public static bool IsContainerReleaseCartonExist(int ShippingPk)
        {
            return DataAccess.Shipping.ShippingPlanDL.IsContainerReleaseCartonExist(ShippingPk);
        }
        public static DataSet GetPackingMaterialInShippingPlan(int SNHPk,int SubType)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetPackingMaterialInShippingPlan(SNHPk,SubType);
        }
        public static DataTable GetBOIStatusList(string cfgtype)
        {
            return DataAccess.Shipping.ShippingPlanDL.GetBOIStatusList(cfgtype);
        }
    }
}
