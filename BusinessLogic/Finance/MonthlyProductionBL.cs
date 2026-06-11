using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Finance;
using BusinessObject.Finance;
using GTIService;

namespace BusinessLogic.Finance
{
    public class MonthlyProductionBL
    {
        /// <summary>
        /// For Monthly Production List
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataTable GetMonthlyProductionList(string fromDate, string ToDate, int bizUnit, int pageNo, int pageSize)
        {
            return MonthlyProductionDL.GetMonthlyProductionList(fromDate, ToDate, bizUnit, pageNo, pageSize);
        }

        /// <summary>
        /// For Monthly Production Get
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static MonthlyProductionHdr GetMonthlyProductionItemsList(int FphPk)
        {
            MonthlyProductionHdr ObjMonthlyProductionHdr = new MonthlyProductionHdr();
            string ItemDetails = MonthlyProductionDL.GetMonthlyProductionItemsList(FphPk);
            if (ItemDetails != string.Empty)
            {
                ObjMonthlyProductionHdr = (MonthlyProductionHdr)CommonFunctions.DeserializeObject(ItemDetails, ObjMonthlyProductionHdr);
                return ObjMonthlyProductionHdr;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Save Monthly Production 
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int? SaveMonthlyProductionDetails(string xmlDoc, ref string TrxNo)
        {
            try
            {
                return MonthlyProductionDL.SaveMonthlyProductionDetails(xmlDoc, ref TrxNo);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete Monthly Production 
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int? DeleteMonthlyProduction(int CurrPK, DateTime LastModifiedTime)
        {
            try
            {
                return MonthlyProductionDL.DeleteMonthlyProduction(CurrPK, LastModifiedTime);
            }
            catch
            {
                throw;
            }
        }
    }
}
