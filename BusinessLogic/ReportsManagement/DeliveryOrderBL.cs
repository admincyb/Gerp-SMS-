using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.ReportsManagement
{
   public class DeliveryOrderBL
    {
        /// <summary>
        /// Get Delivery Order Details For Report
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <returns>DataSet</returns>
       public static DataSet GetDeliveryOrderDetails(int dphPK, int SubType = 0)
        {
            return DataAccess.ReportsManagement.DeliveryOrderDL.GetDeliveryOrderDtls(dphPK, SubType);
        }
        public static DataSet GetDeliveryOrderDetailsDOCNOREVISION(int dphPK, int SubType = 0,int reportpk=0)
        {
            return DataAccess.ReportsManagement.DeliveryOrderDL.GetDeliveryOrderDetailsDOCNOREVISION(dphPK, SubType,reportpk);
        }
        public static DataSet GetPackingListDtls(int dphPK)
       {
           return DataAccess.ReportsManagement.DeliveryOrderDL.GetPackingListDtls(dphPK);
       }
    }
}
