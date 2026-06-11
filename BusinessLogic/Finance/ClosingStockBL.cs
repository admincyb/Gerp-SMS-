using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using BusinessObject.POInvoicing;
using GTIService;
using DataAccess.Finance;
using BusinessObject;
using System.Data;
using BusinessObject.Finance;

namespace BusinessLogic.Finance
{
    public class ClosingStockBL
    {
        /// <summary>
        /// Get PO Invoice List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="cusID"></param>
        /// <param name="InvPk"></param>
        /// <param name="PoPk"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static DataSet GetFCRList(GridPrams grid, User objUser, string vNo, int Bankpk, int PoPk, int Active = 0, int? Status = null)
        {
            return ClosingStockDL.GetFCRList(grid, objUser, vNo, Bankpk, PoPk, Active, Status);
        }
        /// <summary>
        /// Get Fin Year
        /// </summary>
        /// <returns></returns>
        public static DataTable GetFinYear(string year,int bizunit)
        {
            return ClosingStockDL.GetFinYear(year,bizunit);

        }
        /// <summary>
        /// Get Department
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDepartment(int FinYearPK, int bizunit)
        {
            return ClosingStockDL.GetDepartment(FinYearPK, bizunit);

        }
        /// <summary>
        /// Get FCHold Revert Details
        /// </summary>
        /// <param name="PK"></param>
        /// <param name="accountPK"></param>
        /// <param name="curPK"></param>
        /// <returns></returns>
        public static DataSet GetFCHoldRevertDetails(int PK, int accountPK, int curPK)
        {
            return ClosingStockDL.GetFCHoldRevertDetails(PK, accountPK, curPK);
        }
        /// <summary>
        /// Save FCHold Revert Details 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static long SaveFCHoldRevertDetails(string xmlDoc)
        {
            return ClosingStockDL.SaveFCHoldRevertDetails(xmlDoc);
        }       
        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="hrhPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteFCHoldRevertDetails(int hrhPK, DateTime lastModDate)
        {
            return ClosingStockDL.DeleteFCHoldRevertDetails(hrhPK, lastModDate);
        }

        public static DataTable GetFCVoucherNumberAuto(byte Active, int bizUnit, string searchValue)
        {
            return ClosingStockDL.GetFCVoucherNumberAuto(Active, bizUnit, searchValue);
        }



        public static DataTable GetClosingStockList(int PageNo, int PageSize, DateTime? AsOnDate, string StockNo, int Status, User CurrentUser, int ClosingStkPk = 0)
        {
            try
            {
                return ClosingStockDL.GetClosingStockList(PageNo, PageSize, AsOnDate, StockNo, Status, CurrentUser, ClosingStkPk);
            }
            catch
            {
                throw;
            }
        }
        public static String GetStockDetails(int FinYrPK,int bizunit,int DeptPK)
        {
            try
            {
                return ClosingStockDL.GetStockDetails(FinYrPK,bizunit, DeptPK);
            }
            catch
            {
                throw;
            }
        }
        public static String GetStockDetailsByPK(int IohPK, int bizunit,int finyearpk,int? DeptPK=null)
        {
            try
            {
                return ClosingStockDL.GetStockDetailsByPK(IohPK, bizunit,finyearpk, DeptPK);
            }
            catch
            {
                throw;
            }
        }
        
        public static DataTable GetStockList(int bizunit, int? finyrpk = 0 )
        {
            try
            {
                return ClosingStockDL.GetStockList(bizunit,finyrpk);
            }
            catch
            {
                throw;
            }
        }



        public static StockHeader GetStockItemsList(DateTime AsOnDate, int BizUnit, int LSH_PK)
        {            
            try
            {
                StockHeader stockObj = new StockHeader();
                string stock = ClosingStockDL.GetClosingStockList(AsOnDate, BizUnit, LSH_PK);
                if (stock != string.Empty)
                {
                    stockObj = (StockHeader)CommonFunctions.DeserializeObject(stock, stockObj);
                    return stockObj;
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



        public static int? SaveClosingStockDetails(string xmlDoc, ref string TrxNo)
        {
            try
            {
                return ClosingStockDL.SaveClosingStockDetails(xmlDoc,ref TrxNo);                
            }
            catch
            {
                throw;
            }
        }
        public static int? SaveStockClosingDetails(string xmlDoc,ref string TrxNo)
        {
            try
            {
                return ClosingStockDL.SaveStockClosingDetails(xmlDoc,ref TrxNo);
            }
            catch
            {
                throw;
            }
        }

        public static StockHeader GetClosingStockByPk(int CurrPK, int BizUnit, byte Active)
        {
            try
            {
                StockHeader stockObj = new StockHeader();
                string stock = ClosingStockDL.GetClosingStockByPk(CurrPK, BizUnit, Active);
                if (stock != string.Empty)
                {
                    stockObj = (StockHeader)CommonFunctions.DeserializeObject(stock, stockObj);
                    return stockObj;
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


        public static int? DeleteClosingStock(int CurrPK, DateTime LastModifiedTime)
        {
            try
            {
                return ClosingStockDL.DeleteClosingStock(CurrPK, LastModifiedTime);
            }
            catch
            {
                throw;
            }
        }
        public static int? DeleteStockClosing(int CurrPK,int userPK)
        {
            try
            {
                return ClosingStockDL.DeleteStockClosing(CurrPK, userPK);
            }
            catch
            {
                throw;
            }
        }
        public static DataTable GetClosingStockHistory(int ItemCategoryPk)
        {
            try
            {
                return ClosingStockDL.GetClosingStockHistory(ItemCategoryPk);
            }
            catch
            {
                throw;
            }
        }
    }
}
