//using System;
//using System.Runtime.Serialization;

//namespace ERPData
//{
//    [Serializable()]
//    [DataContract]
//    public class SaleOrderHeader : FIN_INVOICE_CUS_TRX_MPG 
//    {
//        /// <summary>
//        /// PO Details ID or Primary Key
//        /// </summary>
//        [DataMember]
//        public global::System.Int32 SOH_PK
//        {
//            get
//            {
//                return _SOH_PK;
//            }
//            set
//            {
//                _SOH_PK = value;
//            }
//        }
//        private global::System.Int32 _SOH_PK;

//        [DataMember]
//        public global::System.Int16 SOH_VERSION
//        {
//            get
//            {
//                return _SOH_VERSION;
//            }
//            set
//            {
//                _SOH_VERSION = value;
//            }
//        }
//        private global::System.Int16 _SOH_VERSION;

//        [DataMember]
//        public global::System.String SOH_NO
//        {
//            get
//            {
//                return _SOH_NO;
//            }
//            set
//            {
//                _SOH_NO = value;
//            }
//        }
//        private global::System.String _SOH_NO;

//        [DataMember]
//        public global::System.DateTime SOH_DATE
//        {
//            get
//            {
//                return _SOH_DATE;
//            }
//            set
//            {
//                _SOH_DATE = value;
//            }
//        }
//        private global::System.DateTime _SOH_DATE;

//        [DataMember]
//        public global::System.Int16 SOH_STATUS
//        {
//            get
//            {
//                return _SOH_STATUS;
//            }
//            set
//            {
//                _SOH_STATUS = value;
//            }
//        }
//        private global::System.Int16 _SOH_STATUS;

//        [DataMember]
//        public global::System.Int32 SOH_CUSTOMER
//        {
//            get
//            {
//                return _SOH_CUSTOMER;
//            }
//            set
//            {
//                _SOH_CUSTOMER = value;
//            }
//        }
//        private global::System.Int32 _SOH_CUSTOMER;

//        [DataMember]
//        public global::System.String SOH_CUSTOMER_TEXT
//        {
//            get
//            {
//                return _SOH_CUSTOMER_TEXT;
//            }
//            set
//            {
//                _SOH_CUSTOMER_TEXT = value;
//            }
//        }
//        private global::System.String _SOH_CUSTOMER_TEXT;

//        [DataMember]
//        public global::System.String SOH_REF_NO
//        {
//            get
//            {
//                return _SOH_REF_NO;
//            }
//            set
//            {
//                _SOH_REF_NO = value;
//            }
//        }
//        private global::System.String _SOH_REF_NO;

//        [DataMember]
//        public global::System.Int32? SOH_CURRENCY
//        {
//            get
//            {
//                return _SOH_CURRENCY;
//            }
//            set
//            {
//                _SOH_CURRENCY = value;
//            }
//        }
//        private global::System.Int32? _SOH_CURRENCY;

//        [DataMember]
//        public global::System.Double? SOH_CURRENCY_RATE
//        {
//            get
//            {
//                return _SOH_CURRENCY_RATE;
//            }
//            set
//            {
//                _SOH_CURRENCY_RATE = value;
//            }
//        }
//        private global::System.Double? _SOH_CURRENCY_RATE;

//        [DataMember]
//        public global::System.Double SOH_TOTAL_QTY
//        {
//            get
//            {
//                return _SOH_TOTAL_QTY;
//            }
//            set
//            {
//                _SOH_TOTAL_QTY = value;
//            }
//        }
//        private global::System.Double _SOH_TOTAL_QTY;

//        [DataMember]
//        public global::System.Double SOH_TOTAL_AMT
//        {
//            get
//            {
//                return _SOH_TOTAL_AMT;
//            }
//            set
//            {
//                _SOH_TOTAL_AMT = value;
//            }
//        }
//        private global::System.Double _SOH_TOTAL_AMT;

//        [DataMember]
//        public global::System.Double SOH_TOTAL_CONV_AMT
//        {
//            get
//            {
//                return _SOH_TOTAL_CONV_AMT;
//            }
//            set
//            {
//                _SOH_TOTAL_CONV_AMT = value;
//            }
//        }
//        private global::System.Double _SOH_TOTAL_CONV_AMT;

//        [DataMember]
//        public global::System.String SOH_REMARKS
//        {
//            get
//            {
//                return _SOH_REMARKS;
//            }
//            set
//            {
//                _SOH_REMARKS = value;
//            }
//        }
//        private global::System.String _SOH_REMARKS;

//        [DataMember]
//        public global::System.String SOH_COMMENTS
//        {
//            get
//            {
//                return _SOH_COMMENTS;
//            }
//            set
//            {
//                _SOH_COMMENTS = value;
//            }
//        }
//        private global::System.String _SOH_COMMENTS;

//        [DataMember]
//        public global::System.String SOH_TERMS
//        {
//            get
//            {
//                return _SOH_TERMS;
//            }
//            set
//            {
//                _SOH_TERMS = value;
//            }
//        }
//        private global::System.String _SOH_TERMS;

//        [DataMember]
//        public global::System.Int16? SOH_DELIVERY_MODE
//        {
//            get
//            {
//                return _SOH_DELIVERY_MODE;
//            }
//            set
//            {
//                _SOH_DELIVERY_MODE = value;
//            }
//        }
//        private global::System.Int16? _SOH_DELIVERY_MODE;

//        [DataMember]
//        public global::System.DateTime? SOH_DELIVERY_DATE
//        {
//            get
//            {
//                return _SOH_DELIVERY_DATE;
//            }
//            set
//            {
//                _SOH_DELIVERY_DATE = value;
//            }
//        }
//        private global::System.DateTime? _SOH_DELIVERY_DATE;

//        [DataMember]
//        public global::System.Int32? SOH_SUBMITTED_BY
//        {
//            get
//            {
//                return _SOH_SUBMITTED_BY;
//            }
//            set
//            {
//                _SOH_SUBMITTED_BY = value;
//            }
//        }
//        private global::System.Int32? _SOH_SUBMITTED_BY;

//        [DataMember]
//        public global::System.DateTime? SOH_SUBMITTED_DATE
//        {
//            get
//            {
//                return _SOH_SUBMITTED_DATE;
//            }
//            set
//            {
//                _SOH_SUBMITTED_DATE = value;
//            }
//        }
//        private global::System.DateTime? _SOH_SUBMITTED_DATE;

//        [DataMember]
//        public global::System.String SOH_SUBMITTED_BY_TEXT
//        {
//            get
//            {
//                return _SOH_SUBMITTED_BY_TEXT;
//            }
//            set
//            {
//                _SOH_SUBMITTED_BY_TEXT = value;
//            }
//        }
//        private global::System.String _SOH_SUBMITTED_BY_TEXT;

//        [DataMember]
//        public global::System.Int32? SOH_APPROVED_BY
//        {
//            get
//            {
//                return _SOH_APPROVED_BY;
//            }
//            set
//            {
//                _SOH_APPROVED_BY = value;
//            }
//        }
//        private global::System.Int32? _SOH_APPROVED_BY;

//        [DataMember]
//        public global::System.DateTime? SOH_APPROVED_DATE
//        {
//            get
//            {
//                return _SOH_APPROVED_DATE;
//            }
//            set
//            {
//                _SOH_APPROVED_DATE = value;
//            }
//        }
//        private global::System.DateTime? _SOH_APPROVED_DATE;

//        [DataMember]
//        public global::System.String SOH_APPROVED_BY_TEXT
//        {
//            get
//            {
//                return _SOH_APPROVED_BY_TEXT;
//            }
//            set
//            {
//                _SOH_APPROVED_BY_TEXT = value;
//            }
//        }
//        private global::System.String _SOH_APPROVED_BY_TEXT;

//        [DataMember]
//        public global::System.Int32 SOH_ORDER_TYPE
//        {
//            get
//            {
//                return _SOH_ORDER_TYPE;
//            }
//            set
//            {
//                _SOH_ORDER_TYPE = value;
//            }
//        }
//        private global::System.Int32 _SOH_ORDER_TYPE;

//        [DataMember]
//        public global::System.String SOH_ORDER_TYPE_TEXT
//        {
//            get
//            {
//                return _SOH_ORDER_TYPE_TEXT;
//            }
//            set
//            {
//                _SOH_ORDER_TYPE_TEXT = value;
//            }
//        }
//        private global::System.String _SOH_ORDER_TYPE_TEXT;

//        [DataMember]
//        public global::System.String SOH_DESC
//        {
//            get
//            {
//                return _SOH_DESC;
//            }
//            set
//            {
//                _SOH_DESC = value;
//            }
//        }
//        private global::System.String _SOH_DESC;

//        [DataMember]
//        public global::System.Int32? SOH_QUOTATION
//        {
//            get
//            {
//                return _SOH_QUOTATION;
//            }
//            set
//            {
//                _SOH_QUOTATION = value;
//            }
//        }
//        private global::System.Int32? _SOH_QUOTATION;

//        [DataMember]
//        public global::System.Int16 SOH_ACTIVE
//        {
//            get
//            {
//                return _SOH_ACTIVE;
//            }
//            set
//            {
//                _SOH_ACTIVE = value;
//            }
//        }
//        private global::System.Int16 _SOH_ACTIVE;

//        [DataMember]
//        public global::System.Int32? SOH_DEPT
//        {
//            get
//            {
//                return _SOH_DEPT;
//            }
//            set
//            {
//                _SOH_DEPT = value;
//            }
//        }
//        private global::System.Int32? _SOH_DEPT;

//        [DataMember]
//        public global::System.String SOH_DEPT_TEXT
//        {
//            get
//            {
//                return _SOH_DEPT_TEXT;
//            }
//            set
//            {
//                _SOH_DEPT_TEXT = value;
//            }
//        }
//        private global::System.String _SOH_DEPT_TEXT;

//        /// <summary>
//        /// PO Vendor Account ID
//        /// </summary>
//        [DataMember]
//        public global::System.Int32? SOH_CUS_ACCOUNT
//        {
//            get
//            {
//                return _SOH_CUS_ACCOUNT;
//            }
//            set
//            {
//                _SOH_CUS_ACCOUNT = value;
//            }
//        }
//        private global::System.Int32? _SOH_CUS_ACCOUNT;

//        /// <summary>
//        /// Invoiced Amount
//        /// </summary>
//        [DataMember]
//        public global::System.Decimal? SOH_INVOICED_AMT
//        {
//            get
//            {
//                return _SOH_INVOICED_AMT;
//            }
//            set
//            {
//                _SOH_INVOICED_AMT = value;
//            }
//        }
//        private global::System.Decimal? _SOH_INVOICED_AMT;

//        /// <summary>
//        /// Bal To Invoice Amount
//        /// </summary>
//        [DataMember]
//        public global::System.Decimal? SOH_BAL_TO_INV_AMT
//        {
//            get
//            {
//                return _SOH_BAL_TO_INV_AMT;
//            }
//            set
//            {
//                _SOH_BAL_TO_INV_AMT = value;
//            }
//        }
//        private global::System.Decimal? _SOH_BAL_TO_INV_AMT;

//        /// <summary>
//        /// Invoiced Now Amount
//        /// </summary>
//        [DataMember]
//        public global::System.Decimal? SOH_INVOICED_NOW_AMT
//        {
//            get
//            {
//                return _SOH_INVOICED_NOW_AMT;
//            }
//            set
//            {
//                _SOH_INVOICED_NOW_AMT = value;
//            }
//        }
//        private global::System.Decimal? _SOH_INVOICED_NOW_AMT;
//    }

//    [Serializable()]
//    [DataContract]
//    public class SaleOrderDetails
//    {
//        /// <summary>
//        /// PO Details ID or Primary Key
//        /// </summary>
//        [DataMember]
//        public global::System.Int32 SOD_PK
//        {
//            get
//            {
//                return _SOD_PK;
//            }
//            set
//            {
//                _SOD_PK = value;
//            }
//        }
//        private global::System.Int32 _SOD_PK;

//        /// <summary>
//        /// PO ID
//        /// </summary>
//        [DataMember]
//        public global::System.Int32 SOD_PO
//        {
//            get
//            {
//                return _SOD_PO;
//            }
//            set
//            {
//                _SOD_PO = value;
//            }
//        }
//        private global::System.Int32 _SOD_PO;

//        /// <summary>
//        /// PO Item ID
//        /// </summary>
//        [DataMember]
//        public global::System.Int32 SOD_ITEM
//        {
//            get
//            {
//                return _SOD_ITEM;
//            }
//            set
//            {
//                _SOD_ITEM = value;
//            }
//        }
//        private global::System.Int32 _SOD_ITEM;

//        /// <summary>
//        /// Item name
//        /// </summary>
//        [DataMember]
//        public global::System.String SOD_ITEM_TEXT
//        {
//            get
//            {
//                return _SOD_ITEM_TEXT;
//            }
//            set
//            {
//                _SOD_ITEM_TEXT = value;
//            }
//        }
//        private global::System.String _SOD_ITEM_TEXT;

//        /// <summary>
//        /// Rate
//        /// </summary>
//        [DataMember]
//        public global::System.Decimal SOD_RATE
//        {
//            get
//            {
//                return _SOD_RATE;
//            }
//            set
//            {
//                _SOD_RATE = value;
//            }
//        }
//        private global::System.Decimal _SOD_RATE;

//        /// <summary>
//        /// Qty
//        /// </summary>
//        [DataMember]
//        public global::System.Double? SOD_QTY
//        {
//            get
//            {
//                return _SOD_QTY;
//            }
//            set
//            {
//                _SOD_QTY = value;
//            }
//        }
//        private global::System.Double? _SOD_QTY;

//        /// <summary>
//        /// UOM
//        /// </summary>
//        [DataMember]
//        public global::System.Int32 SOD_UOM
//        {
//            get
//            {
//                return _SOD_UOM;
//            }
//            set
//            {
//                _SOD_UOM = value;
//            }
//        }
//        private global::System.Int32 _SOD_UOM;

//        /// <summary>
//        /// UOM name
//        /// </summary>
//        [DataMember]
//        public global::System.String SOD_UOM_TEXT
//        {
//            get
//            {
//                return _SOD_UOM_TEXT;
//            }
//            set
//            {
//                _SOD_UOM_TEXT = value;
//            }
//        }
//        private global::System.String _SOD_UOM_TEXT;

//        /// <summary>
//        /// Amount
//        /// </summary>
//        [DataMember]
//        public global::System.Decimal SOD_AMOUNT
//        {
//            get
//            {
//                return _SOD_AMOUNT;
//            }
//            set
//            {
//                _SOD_AMOUNT = value;
//            }
//        }
//        private global::System.Decimal _SOD_AMOUNT;

//        /// <summary>
//        /// Item name
//        /// </summary>
//        [DataMember]
//        public global::System.String SOD_CATEGORY_TEXT
//        {
//            get
//            {
//                return _SOD_CATEGORY_TEXT;
//            }
//            set
//            {
//                _SOD_CATEGORY_TEXT = value;
//            }
//        }
//        private global::System.String _SOD_CATEGORY_TEXT;

//        [DataMember]
//        public global::System.String SOD_REMARKS
//        {
//            get
//            {
//                return _SOD_REMARKS;
//            }
//            set
//            {
//                _SOD_REMARKS = value;
//            }
//        }
//        private global::System.String _SOD_REMARKS;

//        ///// <summary>
//        ///// Required By
//        ///// </summary>
//        //[DataMember]
//        //public global::System.DateTime SOD_REQD_DATE
//        //{
//        //    get
//        //    {
//        //        return _SOD_REQD_DATE;
//        //    }
//        //    set
//        //    {
//        //        _SOD_REQD_DATE = value;
//        //    }
//        //}
//        //private global::System.DateTime _SOD_REQD_DATE;

//    }
    
//    [Serializable()]
//    [DataContract]
//    public class SaleOrderPks
//    {
//        /// <summary>
//        /// PO ID or Primary Key
//        /// </summary>
//        [DataMember]
//        public global::System.Int32 SOH_PK
//        {
//            get
//            {
//                return _SOH_PK;
//            }
//            set
//            {
//                _SOH_PK = value;
//            }
//        }
//        private global::System.Int32 _SOH_PK;

//    }

   
//}
