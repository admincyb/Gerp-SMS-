using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess;
using BusinessObject.DashBoard;
using ERP.Utilities;

namespace BusinessLogic
{
    public class DashboardBL
    {
        /// <summary>
        /// To Get Filter Data
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static DataSet GetFilterData(string procedure, string xml)
        {
            return DashboardDA.GetFilterData(procedure, xml);
        }
        /// <summary>
        /// To Get Report Data
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object[] GetChartData(string procedure, string xml)
        {
            return DashboardDA.GetChartData(procedure, xml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <returns></returns>
        public static string GetVendorContactDetailsReport(int vndID)
        {
            string xml = string.Empty;
            DataTable dtPrcCtrlList = DashboardDA.GetVendorContactDetailsReport(vndID);
            if (dtPrcCtrlList != null)
            {
                if (dtPrcCtrlList.Rows.Count > 0)
                {
                    foreach (DataRow drData in dtPrcCtrlList.Rows)
                    {
                        xml += Convert.ToString(drData[0]);
                    }
                }
            }
            return xml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static string GetStockTransferRptDetails(int pk)
        {
            string xml = string.Empty;
            DataTable dtPrcCtrlList = DashboardDA.GetStockTransferRptDetails(pk);
            if (dtPrcCtrlList != null)
            {
                if (dtPrcCtrlList.Rows.Count > 0)
                {
                    foreach (DataRow drData in dtPrcCtrlList.Rows)
                    {
                        xml += Convert.ToString(drData[0]);
                    }
                }
            }
            return xml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static string GetMaterialIssueDetailsForReport(int pk)
        {
            // return ReportPagesDA.GetMaterialIssueDetails(miID);
            string xml = string.Empty;
            DataTable dtPrcCtrlList = DashboardDA.GetMaterialIssueDetails(pk);
            if (dtPrcCtrlList != null)
            {
                if (dtPrcCtrlList.Rows.Count > 0)
                {
                    foreach (DataRow drData in dtPrcCtrlList.Rows)
                    {
                        xml += Convert.ToString(drData[0]);
                    }
                }
            }
            return xml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static string GetStockAdjustmentDetailsForReport(int pk)
        {
            // return ReportPagesDA.GetStoreAdjustDetails(storeAuditID);
            string xml = string.Empty;
            DataTable dtPrcCtrlList = DashboardDA.GetStoreAdjustDetails(pk);
            if (dtPrcCtrlList != null)
            {
                if (dtPrcCtrlList.Rows.Count > 0)
                {
                    foreach (DataRow drData in dtPrcCtrlList.Rows)
                    {
                        xml += Convert.ToString(drData[0]);
                    }
                }
            }
            return xml;
        }
       
    }
}
