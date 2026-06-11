using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.PurchaseOrderManagement;
using System.Web;
using System.Web.Script.Serialization;
using BusinessObject.Sales;
using GTIService;
using BusinessObject;


namespace BusinessLogic.Sales
{
    public class SaleOrderBL
    {
        /// <summary>
        /// Get Sale OrderDetails For Report
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet SaleOrderDetails(int saleOrderID)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSaleOrderDtls(saleOrderID);
        }

        public static DataSet SaleOrderDetailsDOCNOREVISION(int saleOrderID,int reportpk)
        {
            return DataAccess.ProductionDL.SaleOrderDL.SaleOrderDetailsDOCNOREVISION(saleOrderID,reportpk);
        }

 
        /// <summary>
        /// Monthly Sales List
        /// </summary>
        /// <returns></returns>
        public static DataSet MonthlySalesList()
        {
            return DataAccess.ProductionDL.SaleOrderDL.MonthlySalesList();
        }


        public static DataSet GetSaleOrderArchiveDtls(int saleOrderID, int versonID)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSaleOrderArchiveDtls(saleOrderID, versonID);
        }
         /// <summary>
        /// Get Sales Invoice OrderDetails For Report
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalesInvoiceDtls(int saleOrderID,string SalesInvRptSp)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSalesInvoiceDtls(saleOrderID, SalesInvRptSp);
        }

        public static DataSet GetSalesInvoiceDtlsDOCNOREVISION(int saleOrderID, string SalesInvRptSp,int reportpk)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSalesInvoiceDtlsDOCNOREVISION(saleOrderID, SalesInvRptSp,reportpk);
        }

        /// <summary>
        /// Get Sales Invoice Deatils for Customs Report
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalesInvoiceCustomsDtls(int saleOrderID, string SalesInvRptSp, int reportpk=0)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSalesInvoiceCustomsDtls(saleOrderID, SalesInvRptSp, reportpk);
        }
        /// <summary>
        /// Get Sales Invoice Deatils for Customs Report in shipping plan
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalesInvoiceCustomsDtlsShipping(int saleOrderID, string SalesInvRptSp)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSalesInvoiceCustomsDtlsShipping(saleOrderID, SalesInvRptSp);
        }
        public static DataSet GetSalesInvoiceCustomsShippingPlan(int saleOrderID, string SalesInvRptSp)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSalesInvoiceCustomsShippingPlan(saleOrderID, SalesInvRptSp);
        }
        /// <summary>
        /// Get Sales Invoice OrderDetails For Trading Report
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalesInvoiceTrdDtls(int saleOrderID, string SalesInvRptSp)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSalesInvoiceTrdDtls(saleOrderID, SalesInvRptSp);
        }
         /// <summary>
        /// Get Sale Order List
        /// </summary>
        /// <param name="objPageFilter"></param>
        /// <param name="objFields"></param>
        /// <returns></returns>
        public static DataSet GetSaleOrderList(ERP.Utilities.FilterUtility objPageFilter, SalesOrderBO objFields)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSaleOrderList(objPageFilter, objFields);
        }
        /// <summary>
        /// Get Sale Order Header
        /// </summary>
        /// <param name="quotPK"></param>
        /// <param name="sohPK"></param>
        /// <returns></returns>
        public static SaleOrderBO GetSaleOrderHeader(int quotPK, int sohPK, int customerPK = 0)
        {
            try
            {
                SaleOrderBO saleOrderHeaderObj = new SaleOrderBO();
                string saleOrder = DataAccess.ProductionDL.SaleOrderDL.GetSaleOrderHeader(quotPK, sohPK, customerPK);
                if (saleOrder != string.Empty)
                {
                    saleOrderHeaderObj = (SaleOrderBO)CommonFunctions.DeserializeObject(saleOrder, saleOrderHeaderObj);
                    return saleOrderHeaderObj;
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
        /// Get Sale Contract Header
        /// </summary>
        /// <param name="quotPK"></param>
        /// <param name="sohPK"></param>
        /// <returns></returns>
        public static SaleContractBO GetSaleContractHeader(int quotPK, int sohPK, int customerPK = 0)
        {
            try
            {
                SaleContractBO saleOrderHeaderObj = new SaleContractBO();
                string saleOrder = DataAccess.ProductionDL.SaleOrderDL.GetSaleOrderHeader(quotPK, sohPK, customerPK);
                if (saleOrder != string.Empty)
                {
                    saleOrderHeaderObj = (SaleContractBO)CommonFunctions.DeserializeObject(saleOrder, saleOrderHeaderObj);
                    return saleOrderHeaderObj;
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
        /// Save Sale Order Short Close
        /// </summary>
        /// <param name="sohPK"></param>
        /// <param name="reason"></param>
        /// <param name="refNo"></param>
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static int? SaveSaleOrderShortClose(int sohPK, string reason, string refNo, int userPK)
        {
            return DataAccess.ProductionDL.SaleOrderDL.SaveSaleOrderShortClose(sohPK, reason, refNo, userPK);
        }
        public static int? SaveSaleOrderDetails(string xmlDoc)
        {
            return DataAccess.ProductionDL.SaleOrderDL.SaveSaleOrderDetails(xmlDoc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static int? SaveSaleOrderWkfDetails(string xmlDoc,out int refID, out string retMsg)
        {
            return DataAccess.ProductionDL.SaleOrderDL.SaveSaleOrderWkfDetails(xmlDoc, out refID, out retMsg);
        }
        /// <summary>
        /// Check Sale Contract Cancel
        /// </summary>
        /// <param name="scPK"></param>
        /// <returns></returns>
        public static bool SaleContractCancelCheck(int scPK)
        {
            return DataAccess.ProductionDL.SaleOrderDL.SaleContractCancelCheck(scPK);
        }
        public static int DeleteSaleContractDetails(int scPK, DateTime lastModDate, string deleteReason = "")
        {
            return DataAccess.ProductionDL.SaleOrderDL.DeleteSaleContractDetails(scPK, lastModDate, deleteReason);
        }
        public static DataSet GetSalesInvoiceList(GridPrams grid, User objUser, int cusID, int InvPk, int SoPk, string Customer, int InvType, string ScNo, string pageUrl, string DrCrNo, int? Status = null, int? Pending = null, string due = null, byte group = 0, byte category = 1)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSalesInvoiceList(grid, objUser, cusID, InvPk, SoPk, Customer, InvType, ScNo, pageUrl, DrCrNo, Status, Pending, group, category, due);
        }
        public static DataSet GetRevisionHistory(int itemPK)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetRevisionHistory(itemPK);
        }
        public static DataSet GetLineitemTaxList(int receiptPK)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetLineitemTaxList(receiptPK);
        }

        public static string GetCustomsMapingDetails(int InvId)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetCustomsMapingDetails(InvId);
        }
        /// <summary>
        /// Gets Sale Order Dtls for Packing Hdr in SC listing
        /// </summary>
        /// <param name="strxml"></param>
        public static DataTable GetSaleOrderDetails(int ScPk)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSaleOrderDetails(ScPk);
        }
           /// <summary>
        /// Gets Packing Dtls for SC listing
        /// </summary>
        /// <param name="strxml"></param>
        public static DataTable GetPackingDtls(int active, int SODtlPk, int pk = 0)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetPackingDtls(active, SODtlPk);
        }

        /// <summary>
        /// Sale Order Mail Save
        /// </summary>
        /// <param name="SCPk">SOH_PK</param>
        /// <param name="RefId">REF_ID</param>
        /// <param name="Type">1:Normal Sc ,2: Amend</param>
        public static int? SaleOrderMailSave(int SCPk, int RefId, int Type)
        {
            return DataAccess.ProductionDL.SaleOrderDL.SaleOrderMailSave(SCPk, RefId, Type);
        }

        /// <summary>
        /// To check receipt is allocated or not in Sales Invoice
        /// </summary>
        /// <param name="strxml"></param>
        public static DataTable CheckReceiptAllocationInvoice(long receiptPk)
        {
            return DataAccess.ProductionDL.SaleOrderDL.CheckReceiptAllocationInvoice(receiptPk);
        }

        public static DataSet GetDirectSalesInvoiceList(GridPrams grid, User objUser, int cusID, int InvPk, int SoPk, string Customer, int InvType, string ScNo, string pageUrl, string DrCrNo, int? Status = null, int? Pending = null, string due = null, byte group = 0, byte category = 1)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetDirectSalesInvoiceList(grid, objUser, cusID, InvPk, SoPk, Customer, InvType, ScNo, pageUrl, DrCrNo, Status, Pending, group, category, due);
        }

        /// <summary>
        /// Update Sale Order Company/Plant
        /// </summary>
        /// <param name="P_SOH_PK"></param>
        /// <param name="P_SOH_COMPANY"></param>     
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static int? UpdateSaleOrderPlant(int sohPK,int userPK,int plantPk)
        {
            return DataAccess.ProductionDL.SaleOrderDL.UpdateSaleOrderPlant(sohPK, userPK, plantPk);
        }

        /// <summary>
        /// To get Sales cost
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static DataTable GetSalesCostDetails(string xmlDoc)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSalesCostDetails(xmlDoc);
        }
        /// <summary>
        /// Method to get sales contract item reference details
        /// </summary>
        /// <param name="SohPk"></param>
        /// <param name="SodPk"></param>
        /// <returns></returns>
        public static ItemRefBO GetSaleContractItemRef(int SohPk, int SodPk)
        {
            try
            {
                ItemRefBO saleOrderItmRefObj = new ItemRefBO();
                string strResult = DataAccess.ProductionDL.SaleOrderDL.GetSaleContractItemRef(SohPk, SodPk);
                if (strResult != string.Empty)
                {
                    saleOrderItmRefObj = (ItemRefBO)CommonFunctions.DeserializeObject(strResult, saleOrderItmRefObj);
                    return saleOrderItmRefObj;
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
        /// To get suspence list for reciept 
        /// </summary>
        /// <param name="Bankpk"></param>
        /// <param name="receiptPk"></param>
        /// <returns></returns>
        public static DataTable GetSuspenceListforReciept(int Bankpk, long receiptPk = 0)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSuspenceListforReciept(Bankpk, receiptPk);
        }

        public static DataTable GetShipmentTermsBySaleOrder(int saleOrderPK)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetShipmentTermsBySaleOrder(saleOrderPK);
        }

        public static DataTable GetCurrency(User objUser,int Bizunit)
        {
            return DataAccess.CommonManagement.CommonDL.GetCurrency(objUser, Bizunit);
        }

        public static int GetDebitCreditPost(string PKXml)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetDebitCreditPost(PKXml);
        }
    }
}
