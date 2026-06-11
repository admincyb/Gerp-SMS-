using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants.Shipping
{
    public class ContainerRelease
    {
        /// <summary>
        /// Sp names
        /// </summary>
        #region SPS
        public const string GETCONTAINERRELEASEHDR = "SPSAL_CONTAINER_RELEASE_HDR_GET_KV";
        public const string SAVECONTAINERRELEASEHDR = "SPSAL_CONTAINER_RELEASE_HDR_SAVE";
        public const string SPSAL_CONTAINER_RELEASE_SAVE = "SPSAL_CONTAINER_RELEASE_SAVE";
        public const string SPSAL_CONTAINER_RELEASE_GET = "SPSAL_CONTAINER_RELEASE_GET";

        //For Pallete Bincard
        public const string SPSAL_CONTAINER_RELEASE_AUTO = "SPSAL_CONTAINER_RELEASE_AUTO";
        public const string SPSAL_CONTAINER_RELEASE_CARTON_GET = "SPSAL_CONTAINER_RELEASE_CARTON_GET";
        public const string SPSAL_DESPATCH_CARTON_GET = "SPSAL_DESPATCH_CARTON_GET";
        public const string SPSAL_CONTAINER_RELEASE_CARTON_AUTO_GET = "SPSAL_CONTAINER_RELEASE_CARTON_AUTO_GET";
        public const string SPSAL_DESPATCH_CARTON_AUTO_GET = "SPSAL_DESPATCH_CARTON_AUTO_GET";
        public const string SPSAL_DESPATCH_CARTON_DTL_GET = "SPSAL_DESPATCH_CARTON_DTL_GET";

        #endregion
        /// <summary>
        /// Bind Fields  //Can avoid
        /// </summary>
        #region Fields
        public const string PCRHPK = "P_CRH_PK";
        public const string PSNHPK = "P_SNH_PK";
        public const string PACTIVE = "P_ACTIVE";
        public const string PBIZUNIT = "P_BIZUNIT";
        public const string P_CRH_PK	="P_CRH_PK";
	    public const string P_CRH_SHIPPING_PLAN	="P_CRH_SHIPPING_PLAN";
	    public const string P_CRH_DATE	="P_CRH_DATE";
	    public const string P_CRH_REMARKS	="P_CRH_REMARKS";
	    public const string P_CRH_DEPT	="P_CRH_DEPT";
	    public const string P_ACTIVE	="P_ACTIVE";
	    public const string P_USER_PK	="P_USER_PK";
	    public const string P_BIZUNIT	="P_BIZUNIT";
	    public const string P_LAST_MOD_DT	="P_LAST_MOD_DT";
	    public const string P_RET_VAL	="P_RET_VAL";
        public const string P_CRH_COMPANY = "P_CRH_COMPANY";

        public const string P_DPH_SHIPPING_PLAN = "P_DPH_SHIPPING_PLAN";

        public const string P_BCR_SO_DTL = "P_BCR_SO_DTL";
        public const string P_BCR_NO_LIST = "P_BCR_NO_LIST";
        public const string P_BCR_PALLET = "P_BCR_PALLET";
        public const string P_BCR_LOCATION = "P_BCR_LOCATION";
        public const string P_BCR_NO_PFX = "P_BCR_NO_PFX";
        public const string P_SOH_PK = "P_SOH_PK";
        public const string P_CDR_PK = "P_CDR_PK";
        public const string P_DPD_PK = "P_DPD_PK";
        public const string P_BRAND_PK = "P_BRAND_PK";
        public const string P_DO_QTY = "P_DO_QTY";

        //Pallete Bincard
        public const string P_PBH_PK = "P_PBH_PK ";
        public const string P_PBH_NO = "P_PBH_NO";
        public const string P_BCR_BRAND = "P_BCR_BRAND";
        public const string P_BCR_QTY = "P_BCR_QTY";
        public const string P_FLD_NAME = "P_FLD_NAME ";
        public const string P_VALUE = "P_VALUE ";
        public const string P_BCR_PALLET_NO = "P_BCR_PALLET_NO";
        #endregion
    }
}
