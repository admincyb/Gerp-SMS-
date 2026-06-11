using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.ReportsManagement;


namespace BusinessLogic.ReportsManagement
{
    public class GenerateReportBL
    {
        public static DataSet GetVoucherDtls(string appType, long? RecPK)
        {
            return GenerateReportDL.GetVoucherDtls(appType, RecPK);
        }

        public static DataTable GetReportParameters(string aptCode, int astCode, DateTime AppvdDate)
        {
            return GenerateReportDL.GetReportParameters(aptCode, astCode, AppvdDate);
        }

        public static DataTable GetGeneralLedgerData(string xmlDoc)
        {
            return GenerateReportDL.GetGeneralLedgerData(xmlDoc);
        }
        public static DataTable GetGeneralLedgerFinYearData(string xmlDoc)
        {
            return GenerateReportDL.GetGeneralLedgerFinYearData(xmlDoc);
        }
        public static DataTable GetAccountDtls(int BizUnitPK)
        {
            return GenerateReportDL.GetAccountDtls(BizUnitPK);
        }

        public static DataTable GetCostCenterLedgerData(int astCode, DateTime FromDate, DateTime ToDate, string xmlDoc)
        {
            return GenerateReportDL.GetCostCenterLedgerData(astCode, FromDate, ToDate, xmlDoc);
        }
        public static DataTable GetSFGData(int astCode, DateTime FromDate, DateTime ToDate)
        {
            return GenerateReportDL.GetSFGData(astCode, FromDate, ToDate);
        }
        public static DataTable GetSFGDISPData(int astCode, DateTime FromDate, DateTime ToDate)
        {
            return GenerateReportDL.GetSFGDISPData(astCode, FromDate, ToDate);
        }
        public static DataTable GetPartyLedgerData(string xmlDoc)
        {
            return GenerateReportDL.GetPartyLedgerData(xmlDoc);
        }

        public static DataTable GetSubLedgerData(string xmlDoc)
        {
            return GenerateReportDL.GetSubLedgerData(xmlDoc);
        }

        public static DataTable GetGeneralLedgerConsolidatedData(string xmlDoc)
        {
            return GenerateReportDL.GetGeneralLedgerConsolidatedData(xmlDoc);
        }

        public static DataTable GetProfitReport(string xmlDoc)
        {
            return GenerateReportDL.GetProfitReport(xmlDoc);
        }

        public static int ProcessProfitReport(string xmlDoc)
        {
            return GenerateReportDL.ProcessProfitReport(xmlDoc);
        }

        public static DataTable GetItemCategory(int BizUnitPK)
        {
            return GenerateReportDL.GetItemCategory(BizUnitPK);
        }
        public static DataTable GetItemCategoryFormer(int BizUnitPK)
        {
            return GenerateReportDL.GetItemCategoryFormer(BizUnitPK);
        }
        public static DataTable GetDeptStores(int BizUnitPK)
        {
            return GenerateReportDL.GetDeptStores(BizUnitPK);
        }

        public static DataTable GetItemsByCategory(int CategotyPK, string CategoryPKs = null)
        {
            return GenerateReportDL.GetItemsByCategory(CategotyPK, CategoryPKs);
        }
        public static DataTable GetItemsByCategoryFormer(int CategotyPK, string CategoryPKs = null)
        {
            return GenerateReportDL.GetItemsByCategoryFormer(CategotyPK, CategoryPKs);
        }

        public static DataTable GetTransactionNoByMode(int modepk,string FromDate,string ToDate)
        {
            return GenerateReportDL.GetTransactionNoByMode(modepk,FromDate,ToDate);
        }
        public static DataTable GetTrasactionMode()
        {
            return GenerateReportDL.GetTrasactionMode();
        }
        public static DataTable GetPlant()
        {
            return GenerateReportDL.GetPlant();
        }

        public static DataTable GetClassificationByCategory(int CategotyPK)
        {
            return GenerateReportDL.GetClassificationByCategory(CategotyPK);
        }

        public static DataTable GetFinYear(int BizUnitPK)
        {
            return GenerateReportDL.GetFinYear(BizUnitPK);
        }

        public static DataTable GetDateByFinYear(int FinyearPK)
        {
            return GenerateReportDL.GetDateByFinYear(FinyearPK);
        }

        public static DataTable GetCurrentFinYear(int BizUnitPK)
        {
            return GenerateReportDL.GetCurrentFinYear(BizUnitPK);
        }

        public static DataTable GetCategoryTreeNodes(int BizUnitPK)
        {
            return GenerateReportDL.GetCategoryTreeNodes(BizUnitPK);
        }
        public static DataSet GetBinCardIssueOutputReport(string BatchNo)
        {
            return GenerateReportDL.GetBinCardIssueOutputReport(BatchNo);
        }

        public static DataSet GetIssueOutputReport(string xml)
        {
            return GenerateReportDL.GetIssueOutputReport(xml);
        }

        public static DataSet GetCartonIssueOutputReport(string BatchNo)
        {
            return GenerateReportDL.GetCartonIssueOutputReport(BatchNo);
        }

        public static DataSet GetBinCardTraceReportData(string BatchNo, int SBU)
        {
            return GenerateReportDL.GetBinCardTraceReportData(BatchNo, SBU);
        }
    }
}
