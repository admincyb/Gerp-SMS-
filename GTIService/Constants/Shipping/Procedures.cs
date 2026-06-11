using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Shipping
{
    public class Procedures
    {
        //Container Release
        public const string GETCONTAINERRELEASEHDR = "SPSAL_CONTAINER_RELEASE_HDR_GET_KV";
        public const string SAVECONTAINERRELEASEHDR = "SPSAL_CONTAINER_RELEASE_HDR_SAVE";

        public const string SPSAL_ORDER_DIR_PEND_GET = "SPSAL_ORDER_DIR_PEND_GET";
        public const string SPINV_STK_BATCH_GET_KV = "SPINV_STK_BATCH_GET_KV";
        public const string SPINV_STK_BATCH_DTL_GET = "SPINV_STK_BATCH_DTL_GET";
        public const string SPSAL_DESPATCH_DIR_SAVE = "SPSAL_DESPATCH_DIR_SAVE";
        public const string SPSAL_DESPATCH_DIR_WKF_SAVE = "SPSAL_DESPATCH_DIR_WKF_SAVE";
        public const string SPSAL_DESPATCH_DIR_GET_LIST = "SPSAL_DESPATCH_DIR_GET_LIST";
        public const string SPSAL_DESPATCH_DIR_DTL_GET = "SPSAL_DESPATCH_DIR_DTL_GET";
        public const string SPSAL_DESPATCH_DIR_GET = "SPSAL_DESPATCH_DIR_GET";
        public const string SPSAL_DESPATCH_DIR_DELETE = "SPSAL_DESPATCH_DIR_DELETE";
        public const string SPSAL_DESPATCH_DIR_AUTO = "SPSAL_DESPATCH_DIR_AUTO";
        public const string SPSAL_DO_DIRECT_CANCEL_CHECK = "SPSAL_DO_DIRECT_CANCEL_CHECK";
        public const string SPSAL_DESPATCH_DIR_SO_VALDATE = "SPSAL_DESPATCH_DIR_SO_VALDATE";
        public const string SPSAL_ORDER_DIR_PEND_AUTO = "SPSAL_ORDER_DIR_PEND_AUTO";
        public const string SPSAL_DESPATCH_CARTON_SAVE = "SPSAL_DESPATCH_CARTON_SAVE";
        public const string SPSAL_DESPATCH_CARTON_DTL_GET = "SPSAL_DESPATCH_CARTON_DTL_GET"; 
    }
}
