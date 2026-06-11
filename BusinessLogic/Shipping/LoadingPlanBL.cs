using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Shipping;
using System.Data;

namespace BusinessLogic.Shipping
{
    public class LoadingPlanBL
    {
        /// <summary>
        /// Save Loading Plan
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveLoadingPlan(string pXML)
        {
            return LoadingPlanDL.SavelLoadingPlan(pXML);
        }

        /// <summary>
        /// Get Loading Plan
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataSet GetLoadingPlan(int LPHID, int SBUID, int LPHACTV)
        {
            return LoadingPlanDL.GetLoadingPlan(LPHID, SBUID, LPHACTV);
        }

        /// <summary>
        ///Get Loading Plan Report
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataSet GetLoadingPlanReport(int? LPHPK, int SNHPK)
        {
            return LoadingPlanDL.GetLoadingPlanReport(LPHPK, SNHPK);
        }
        public static DataTable GetSCDetails(int spPk, int active, int bizuit)
        {
            return LoadingPlanDL.GetSCDetails(spPk, active, bizuit);
        }
        public static DataTable GetBrandDetails(int soPk,int sodPk,int shpPlanPk, int active, int bizuit)
        {
            return LoadingPlanDL.GetBrandDetails(soPk, sodPk,shpPlanPk, active, bizuit);
        }
    }
}
