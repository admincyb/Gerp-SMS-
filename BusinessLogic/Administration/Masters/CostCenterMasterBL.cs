using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Administration.Masters;
using System.Data;
using BusinessObject.Administration.Masters;
using ERP.Utilities;
using DataAccess;

namespace BusinessLogic.Administration.Masters
{
    public class CostCenterMasterBL
    {
        /// <summary>
        /// Listing Cost Center
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bsu"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="classification"></param>
        /// <param name="status"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static DataTable GetCostCenterList(BusinessObject.GridPrams gridParam, int bizUnit, string code, string name)
        {
            return CostCenterMasterDA.GetCostCenterList(gridParam, bizUnit, code, name);
        }
        /// <summary>
        /// Activity List
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bizUnit"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataTable GetActivityList(BusinessObject.GridPrams gridParam, int bizUnit, string code, string name,int teamPK,int mainActivityPK)
        {
            return CostCenterMasterDA.GetActivityList(gridParam, bizUnit, code, name, teamPK, mainActivityPK);
        }

        /// <summary>
        /// Save Cost Center
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SaveCostCenterMaster(CostCenterMasterBO objCostCenter)
        {
            return CostCenterMasterDA.SaveCostCenterMaster(objCostCenter);
        }

        /// <summary>
        /// Get Cost Center by PK
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bsu"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="classification"></param>
        /// <param name="status"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static DataTable GetCostCenterByPK(int cnmPK, int active, int bizUnit, int IssueSubDept = 0)
        {
            return CostCenterMasterDA.GetCostCenterByPK(cnmPK,active,bizUnit,IssueSubDept);
        }

        /// <summary>
        /// Delete Cost Center
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int? DeleteCostCenter(int CurrPK, DateTime dateTime)
        {
            return CostCenterMasterDA.DeleteCostCenter(CurrPK, dateTime);
        }

        /// <summary>
        /// Update Cost Center Status
        /// </summary>
        /// <param name="PEL_PK"></param>
        /// <param name="Status"></param>
        /// <param name="UserPk"></param>
        /// <param name="LastModDate"></param>
        /// <returns></returns>
        public static int? UpdateCostCenterStatus(int CNM_PK, int Status, int UserPk, DateTime? LastModDate)
        {
            return CostCenterMasterDA.UpdateCostCenterStatus(CNM_PK, Status, UserPk, LastModDate);
        }

        /// <summary>
        /// Get Group
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetCostCenterGroup(int bizUnit, int groupType, int groupValue)
        {
            int active = 1;
            return CostCenterMasterDA.GetCostCenterGroup(bizUnit, groupType, groupValue, active);
        }
        public static DataTable GetCostCenterAccountListByGroup(int Group_PK, string SearchText, int bizUnit)
        {
            return CostCenterMasterDA.GetCostCenterAccountListByGroup(Group_PK, SearchText, bizUnit);
        }

        public static DataTable GetBudgetCostCenterAccountListByGroup(int Group_PK, string SearchText, int bizUnit,int Budget)
        {
            return CostCenterMasterDA.GetBudgetCostCenterAccountListByGroup(Group_PK, SearchText, bizUnit, Budget);
        }

        public static DataTable GetCostCenterAccountListWithOutGroup( string SearchText, int bizUnit)
        {
            return CostCenterMasterDA.GetCostCenterAccountListWithOutGroup( SearchText, bizUnit);
        }
        public static DataTable GetCostCenterAccountList(BusinessObject.GridPrams gridParam, int cost_center_PK)
        {
            return CostCenterMasterDA.GetCostCenterAccountList(gridParam, cost_center_PK);
        }
        public static DataTable GetMainActivityList(int bizunit,int currPk,int ?status)
        {
            return CostCenterMasterDA.GetMainActivityList(bizunit,currPk,status);
        }

        /// <summary>
        /// Save Cost Center
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SaveActivityMaster(string xmlDoc)
        {
            return CostCenterMasterDA.SaveActivityMaster(xmlDoc);
        }


        public static int? DeleteActivity(int CurrPK, DateTime dateTime)
        {
            return CostCenterMasterDA.DeleteActivity(CurrPK, dateTime);
        }

        public static ActivityMasterBO GetActivityDetails(int CurrPK)
        {
            try
            {
                ActivityMasterBO objEmpTransfer = new ActivityMasterBO();
                string xmlResult = CostCenterMasterDA.GetActivityDetails(CurrPK);
                if (xmlResult != string.Empty)
                {
                    objEmpTransfer = (ActivityMasterBO)CommonFunctions.DeserializeObject(xmlResult, objEmpTransfer);
                    return objEmpTransfer;
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

        public static int? UpdateActivityStatus(int ActivityPK, int Status, int UserPk, DateTime? LastModDate)
        {
            return CostCenterMasterDA.UpdateActivityStatus(ActivityPK, Status, UserPk, LastModDate);
        }
    }
}
