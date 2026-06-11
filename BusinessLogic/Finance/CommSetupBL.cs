using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.Finance
{
    public class CommSetupBL
    {
        /// <summary>
        /// Get Agents
        /// </summary>
        /// <param name="agentPk"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetAgentDetails(int? agentPk, int bizUnit, int active)
        {
            try
            {
                DataTable dtAgents = DataAccess.Finance.CommSetupDL.GetAgentDetails(agentPk, bizUnit, active);
                return dtAgents;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Delete Customer 
        /// </summary>
        /// <param name></param>
        /// <returns></returns>
        public static int DeleteCustomer(int CustId,int AgentId, int sbu)
        {
            return DataAccess.Finance.CommSetupDL.DeleteCustomer(CustId,AgentId,sbu);
        }

        /// <summary>
        /// Get Mapped Customers
        /// </summary>
        /// <param name="agentPk"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetMappedCustomerList(int agentPk, int bizUnit)
        {
            try
            {
                DataTable dtCustomer = DataAccess.Finance.CommSetupDL.GetMappedCustomerList(agentPk, bizUnit);
                return dtCustomer;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get All Customers
        /// </summary>
        /// <param name="custPk"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetAllCustomerList(int bizUnit)
        {
            try
            {
                DataTable dtCustomer = DataAccess.Finance.CommSetupDL.GetAlldCustomerList(null, bizUnit, 1);
                return dtCustomer;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Rate Type
        /// </summary>
        /// <param name="ratePk"></param>
        /// <param name="active"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetRateType(int? ratePk, int active)
        {
            try
            {
                DataTable dtRateTypes = DataAccess.Finance.CommSetupDL.GetRateType(ratePk, active);
                return dtRateTypes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Mapped Commision List
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="customerId"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetMappedCommisionList(int agentId, int customerId)
        {
            try
            {
                DataTable dtMappedCommisionList = DataAccess.Finance.CommSetupDL.GetMappedCommisionList(agentId, customerId);
                return dtMappedCommisionList;
            }
            catch
            {
                throw;
            }           
        }

        /// <summary>
        ///Save Commision List
        /// </summary>
        /// <param name="pXml"></param>
        /// <returns>int</returns>     
        public static int SaveCommisionDetails(string pXml)
        {
            try
            {
               return DataAccess.Finance.CommSetupDL.SaveCommisionDetails(pXml);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Rate Type
        /// </summary>
        /// <param name="ratePk"></param>
        /// <param name="active"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetMarginSetupType(int? ratePk, int active,int bizunit)
        {
            try
            {
                DataTable dtRateTypes = DataAccess.Finance.CommSetupDL.GetMarginSetupType(ratePk, active,bizunit);
                return dtRateTypes;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetCOAOpeningBalanceList(int bizunit, int finYearPk, int transNoPK, string transDate, BusinessObject.GridPrams gridParamObj)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.GetCOAOpeningBalanceList(bizunit, finYearPk, transNoPK, transDate, gridParamObj);
            }
            catch
            {
                throw;
            }
        }

        public static string GetCOAOpeningBalance(int FinYearPK, int bizunit)
        {
            try
            {
                string strResult = DataAccess.Finance.CommSetupDL.GetCOAOpeningBalance(FinYearPK, bizunit);
                return strResult;
            }
            catch
            {
                throw;
            }
        }

        public static int? SaveCOAFinYearOpeninBalanceWkf(string xmlDoc, out int refID, out string transNo)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.SaveCOAFinYearOpeninBalanceWkf(xmlDoc, out refID, out transNo);
            }
            catch
            {
                throw;
            }
        }

        public static string GetCOAOpeningBalanceByPK(int COH_PK)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.GetCOAOpeningBalanceByPK(COH_PK);
            }
            catch
            {
                throw;
            }
        }

        public static DataTable GetFinYearOpeningNumbers(int bizunit, string searchKey)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.GetFinYearOpeningNumbers(bizunit, searchKey);
            }
            catch
            {
                throw;
            }
        }

        public static int DeleteFinYearOpening(int COH_PK, int UserPK)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.DeleteFinYearOpening(COH_PK, UserPK);
            }
            catch
            {
                throw;
            }
        }

        public static string GetCOATreeNodes(int P_COA_PK, int P_BIZUNIT)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.GetCOATreeNodes(P_COA_PK, P_BIZUNIT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetAccountGroup(int bizunit)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.GetAccountGroup(bizunit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetCOAByParent(int P_COA_PK, int CWIPAssetPK, string SearchValue = "")
        { 
            try
            {
                return DataAccess.Finance.CommSetupDL.GetCOAByParent(P_COA_PK, CWIPAssetPK, SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetCOATransactionList(int P_COA_PK, int CWIPAssetPK ,string poNumber = null)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.GetCOATransactionList(P_COA_PK, CWIPAssetPK, poNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int? SaveCWIPAssetWkf(string xmlDoc, out int refID, out string transNo)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.SaveCWIPAssetWkf(xmlDoc, out refID, out transNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetCWIPAssetList(BusinessObject.GridPrams gridParamObj, int BizunitPK, string pageUrl, int AccPK)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.GetCWIPAssetList(gridParamObj, BizunitPK, pageUrl, AccPK);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetCWIPAsset(int CWIPAssetPK)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.GetCWIPAsset(CWIPAssetPK);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int DeleteCWIPAsset(int CWIPAssetPK)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.DeleteCWIPAsset(CWIPAssetPK);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetCWIPFieldValues(string FieldName, string SearchKey, int Bizunit, int DeptPK)
        {
            try
            {
                return DataAccess.Finance.CommSetupDL.GetCWIPFieldValues(FieldName, SearchKey, Bizunit, DeptPK);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
