using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Shipping;
using System.Data;

namespace BusinessLogic.Shipping
{
    public class ShippingUploadsBL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveShippingUploads(string pXML)
        {
            return ShippingUploadsDL.SaveShippingUploads(pXML);
        }
        /// <summary>
        /// Save Bill of loading
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveBillofLoading(string pXML)
        {
            return ShippingUploadsDL.SaveBillofLoading(pXML);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shippingPK"></param>
        /// <returns></returns>
        public static string GetBillofLoading(int shippingPK,int Type)
        {
            return ShippingUploadsDL.GetBillofLoading(shippingPK, Type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="P_SCD_PK"></param>
        /// <param name="P_SCD_PLAN_HDR"></param>
        /// <param name="P_SCD_ACTIVE"></param>
        /// <returns></returns>
        public static DataSet GetShippingUploads(int P_SCD_PK, int P_SCD_PLAN_HDR, short P_SCD_ACTIVE, int P_SCD_TYPE)
        {
            return ShippingUploadsDL.GetShippingUploads(P_SCD_PK, P_SCD_PLAN_HDR, P_SCD_ACTIVE, P_SCD_TYPE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="P_SCD_PK"></param>
        /// <param name="P_SCD_PLAN_HDR"></param>
        /// <param name="P_SCD_ACTIVE"></param>
        /// <returns></returns>
        public static DataSet GetExportList(int P_SNH_PK)
        {
            return ShippingUploadsDL.GetExportList(P_SNH_PK);
        }
    }
}
