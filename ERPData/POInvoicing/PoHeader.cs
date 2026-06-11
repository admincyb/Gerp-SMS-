using System;
using System.Runtime.Serialization;

namespace ERPData
{
    [Serializable()]
    [DataContract]
    public class GRNDetails
    {   /// <summary>
        /// PO Details ID or Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int32 GRH_PK
        {
            get
            {
                return _GRH_PK;
            }
            set
            {
                _GRH_PK = value;
            }
        }
        private global::System.Int32 _GRH_PK;

        /// <summary>
        /// GRN No
        /// </summary>
        [DataMember]
        public global::System.String GRH_NO
        {
            get
            {
                return _GRH_NO;
            }
            set
            {
                _GRH_NO = value;
            }
        }
        private global::System.String _GRH_NO;

        /// <summary>
        /// Date
        /// </summary>
        [DataMember]
        public global::System.DateTime GRH_DATE
        {
            get
            {
                return _GRH_DATE;
            }
            set
            {
                _GRH_DATE = value;
            }
        }
        private global::System.DateTime _GRH_DATE;

        /// <summary>
        /// Department
        /// </summary>
        [DataMember]
        public global::System.Int32 GRH_DEPT
        {
            get
            {
                return _GRH_DEPT;
            }
            set
            {
                _GRH_DEPT = value;
            }
        }
        private global::System.Int32 _GRH_DEPT;

        /// <summary>
        /// Store
        /// </summary>
        [DataMember]
        public global::System.String GRH_DEPT_TEXT
        {
            get
            {
                return _GRH_DEPT_TEXT;
            }
            set
            {
                _GRH_DEPT_TEXT = value;
            }
        }
        private global::System.String _GRH_DEPT_TEXT;
                
        /// <summary>
        /// Created By
        /// </summary>
        [DataMember]
        public global::System.Int32? GRH_SUBMITTED_BY
        {
            get
            {
                return _GRH_SUBMITTED_BY;
            }
            set
            {
                _GRH_SUBMITTED_BY = value;
            }
        }
        private global::System.Int32? _GRH_SUBMITTED_BY;
        /// <summary>
        /// Created By Name
        /// </summary>
        [DataMember]
        public global::System.String GRH_SUBMITTED_BY_TEXT
        {
            get
            {
                return _GRH_SUBMITTED_BY_TEXT;
            }
            set
            {
                _GRH_SUBMITTED_BY_TEXT = value;
            }
        }
        private global::System.String _GRH_SUBMITTED_BY_TEXT;
        /// <summary>
        /// Approved By
        /// </summary>
        [DataMember]
        public global::System.Int32? GRH_APPROVED_BY
        {
            get
            {
                return _GRH_APPROVED_BY;
            }
            set
            {
                _GRH_APPROVED_BY = value;
            }
        }
        private global::System.Int32? _GRH_APPROVED_BY;
        /// <summary>
        /// Approved By Name
        /// </summary>
        [DataMember]
        public global::System.String GRH_APPROVED_BY_TEXT
        {
            get
            {
                return _GRH_APPROVED_BY_TEXT;
            }
            set
            {
                _GRH_APPROVED_BY_TEXT = value;
            }
        }
        private global::System.String _GRH_APPROVED_BY_TEXT;


        /// <summary>
        /// Remarks
        /// </summary>
        [DataMember]
        public global::System.String GRH_COMMENTS
        {
            get
            {
                return _GRH_COMMENTS;
            }
            set
            {
                _GRH_COMMENTS = value;
            }
        }
        private global::System.String _GRH_COMMENTS;
    }

    [Serializable()]
    [DataContract]
    public class GINDetails
    {
        /// <summary>
        /// PO Details ID or Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int32 GIH_PK
        {
            get
            {
                return _GIH_PK;
            }
            set
            {
                _GIH_PK = value;
            }
        }
        private global::System.Int32 _GIH_PK;

        /// <summary>
        /// GIN No
        /// </summary>
        [DataMember]
        public global::System.String GIH_NO
        {
            get
            {
                return _GIH_NO;
            }
            set
            {
                _GIH_NO = value;
            }
        }
        private global::System.String _GIH_NO;

        /// <summary>
        /// Date
        /// </summary>
        [DataMember]
        public global::System.DateTime GIH_DATE
        {
            get
            {
                return _GIH_DATE;
            }
            set
            {
                _GIH_DATE = value;
            }
        }
        private global::System.DateTime _GIH_DATE;

        /// <summary>
        /// Department
        /// </summary>
        [DataMember]
        public global::System.Int32 GIH_DEPT
        {
            get
            {
                return _GIH_DEPT;
            }
            set
            {
                _GIH_DEPT = value;
            }
        }
        private global::System.Int32 _GIH_DEPT;

        /// <summary>
        /// Store
        /// </summary>
        [DataMember]
        public global::System.String GIH_DEPT_TEXT
        {
            get
            {
                return _GIH_DEPT_TEXT;
            }
            set
            {
                _GIH_DEPT_TEXT = value;
            }
        }
        private global::System.String _GIH_DEPT_TEXT;

        /// <summary>
        /// Created By
        /// </summary>
        [DataMember]
        public global::System.Int32? GIH_SUBMITTED_BY
        {
            get
            {
                return _GIH_SUBMITTED_BY;
            }
            set
            {
                _GIH_SUBMITTED_BY = value;
            }
        }
        private global::System.Int32? _GIH_SUBMITTED_BY;
        /// <summary>
        /// Created By Name
        /// </summary>
        [DataMember]
        public global::System.String GIH_SUBMITTED_BY_TEXT
        {
            get
            {
                return _GIH_SUBMITTED_BY_TEXT;
            }
            set
            {
                _GIH_SUBMITTED_BY_TEXT = value;
            }
        }
        private global::System.String _GIH_SUBMITTED_BY_TEXT;
        /// <summary>
        /// Approved By
        /// </summary>
        [DataMember]
        public global::System.Int32? GIH_APPROVED_BY
        {
            get
            {
                return _GIH_APPROVED_BY;
            }
            set
            {
                _GIH_APPROVED_BY = value;
            }
        }
        private global::System.Int32? _GIH_APPROVED_BY;
        /// <summary>
        /// Approved By Name
        /// </summary>
        [DataMember]
        public global::System.String GIH_APPROVED_BY_TEXT
        {
            get
            {
                return _GIH_APPROVED_BY_TEXT;
            }
            set
            {
                _GIH_APPROVED_BY_TEXT = value;
            }
        }
        private global::System.String _GIH_APPROVED_BY_TEXT;


        /// <summary>
        /// Remarks
        /// </summary>
        [DataMember]
        public global::System.String GIH_COMMENTS
        {
            get
            {
                return _GIH_COMMENTS;
            }
            set
            {
                _GIH_COMMENTS = value;
            }
        }
        private global::System.String _GIH_COMMENTS;

    }
    
    [Serializable()]
    [DataContract]
    public class StockTransferDetails
    {
         /// <summary>
        /// PO Details ID or Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int32 SFH_PK
        {
            get
            {
                return _SFH_PK;
            }
            set
            {
                _SFH_PK = value;
            }
        }
        private global::System.Int32 _SFH_PK;

        /// <summary>
        /// Stock Transfer No
        /// </summary>
        [DataMember]
        public global::System.String SFH_NO
        {
            get
            {
                return _SFH_NO;
            }
            set
            {
                _SFH_NO = value;
            }
        }
        private global::System.String _SFH_NO;

        /// <summary>
        /// Date
        /// </summary>
        [DataMember]
        public global::System.DateTime SFH_DATE
        {
            get
            {
                return _SFH_DATE;
            }
            set
            {
                _SFH_DATE = value;
            }
        }
        private global::System.DateTime _SFH_DATE;

        /// <summary>
        /// Department
        /// </summary>
        [DataMember]
        public global::System.Int32 SFH_DEPT_FROM
        {
            get
            {
                return _SFH_DEPT_FROM;
            }
            set
            {
                _SFH_DEPT_FROM = value;
            }
        }
        private global::System.Int32 _SFH_DEPT_FROM;

        /// <summary>
        /// Store
        /// </summary>
        [DataMember]
        public global::System.String SFH_DEPT_FROM_TEXT
        {
            get
            {
                return _SFH_DEPT_FROM_TEXT;
            }
            set
            {
                _SFH_DEPT_FROM_TEXT = value;
            }
        }
        private global::System.String _SFH_DEPT_FROM_TEXT;

        /// <summary>
        /// Department
        /// </summary>
        [DataMember]
        public global::System.Int32 SFH_DEPT_TO
        {
            get
            {
                return _SFH_DEPT_TO;
            }
            set
            {
                _SFH_DEPT_TO = value;
            }
        }
        private global::System.Int32 _SFH_DEPT_TO;

        /// <summary>
        /// Store
        /// </summary>
        [DataMember]
        public global::System.String SFH_DEPT_TO_TEXT
        {
            get
            {
                return _SFH_DEPT_TO_TEXT;
            }
            set
            {
                _SFH_DEPT_TO_TEXT = value;
            }
        }
        private global::System.String _SFH_DEPT_TO_TEXT;
                
        /// <summary>
        /// Created By
        /// </summary>
        [DataMember]
        public global::System.Int32? SFH_SUBMITTED_BY
        {
            get
            {
                return _SFH_SUBMITTED_BY;
            }
            set
            {
                _SFH_SUBMITTED_BY = value;
            }
        }
        private global::System.Int32? _SFH_SUBMITTED_BY;
        /// <summary>
        /// Created By Name
        /// </summary>
        [DataMember]
        public global::System.String SFH_SUBMITTED_BY_TEXT
        {
            get
            {
                return _SFH_SUBMITTED_BY_TEXT;
            }
            set
            {
                _SFH_SUBMITTED_BY_TEXT = value;
            }
        }
        private global::System.String _SFH_SUBMITTED_BY_TEXT;
        /// <summary>
        /// Approved By
        /// </summary>
        [DataMember]
        public global::System.Int32? SFH_APPROVED_BY
        {
            get
            {
                return _SFH_APPROVED_BY;
            }
            set
            {
                _SFH_APPROVED_BY = value;
            }
        }
        private global::System.Int32? _SFH_APPROVED_BY;
        /// <summary>
        /// Approved By Name
        /// </summary>
        [DataMember]
        public global::System.String SFH_APPROVED_BY_TEXT
        {
            get
            {
                return _SFH_APPROVED_BY_TEXT;
            }
            set
            {
                _SFH_APPROVED_BY_TEXT = value;
            }
        }
        private global::System.String _SFH_APPROVED_BY_TEXT;


        /// <summary>
        /// Remarks
        /// </summary>
        [DataMember]
        public global::System.String SFH_COMMENTS
        {
            get
            {
                return _SFH_COMMENTS;
            }
            set
            {
                _SFH_COMMENTS = value;
            }
        }
        private global::System.String _SFH_COMMENTS;


        //Stock Transfer No	Date	Transfer From	Trasfer To	Submitted by 	Approved by	comments

    }
     
    [Serializable()]
    [DataContract]
    public class PoDetails
    {
        /// <summary>
        /// PO Details ID or Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int32 POD_PK
        {
            get
            {
                return _POD_PK;
            }
            set
            {
                _POD_PK = value;
            }
        }
        private global::System.Int32 _POD_PK;
        /// <summary>
        /// PO ID
        /// </summary>
        [DataMember]
        public global::System.Int32 POD_PO
        {
            get
            {
                return _POD_PO;
            }
            set
            {
                _POD_PO = value;
            }
        }
        private global::System.Int32 _POD_PO;
        /// <summary>
        /// PO Item ID
        /// </summary>
        [DataMember]
        public global::System.Int32 POD_ITEM
        {
            get
            {
                return _POD_ITEM;
            }
            set
            {
                _POD_ITEM = value;
            }
        }
        private global::System.Int32 _POD_ITEM;

        /// <summary>
        /// Item name
        /// </summary>
        [DataMember]
        public global::System.String POD_ITEM_TEXT
        {
            get
            {
                return _POD_ITEM_TEXT;
            }
            set
            {
                _POD_ITEM_TEXT = value;
            }
        }
        private global::System.String _POD_ITEM_TEXT;
        /// <summary>
        /// Rate
        /// </summary>

        [DataMember]
        public global::System.Decimal POD_RATE
        {
            get
            {
                return _POD_RATE;
            }
            set
            {
                _POD_RATE = value;
            }
        }
        private global::System.Decimal _POD_RATE;
        /// <summary>
        /// Qty
        /// </summary>
        [DataMember]
        public global::System.Double? POD_QTY
        {
            get
            {
                return _POD_QTY;
            }
            set
            {
                _POD_QTY = value;
            }
        }
        private global::System.Double? _POD_QTY;
        /// <summary>
        /// UOM
        /// </summary>
        [DataMember]
        public global::System.Int32 POD_UOM
        {
            get
            {
                return _POD_UOM;
            }
            set
            {
                _POD_UOM = value;
            }
        }
        private global::System.Int32 _POD_UOM;
        /// <summary>
        /// UOM name
        /// </summary>
        [DataMember]
        public global::System.String POD_UOM_TEXT
        {
            get
            {
                return _POD_UOM_TEXT;
            }
            set
            {
                _POD_UOM_TEXT = value;
            }
        }
        private global::System.String _POD_UOM_TEXT;

        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public global::System.Decimal POD_AMOUNT
        {
            get
            {
                return _POD_AMOUNT;
            }
            set
            {
                _POD_AMOUNT = value;
            }
        }
        private global::System.Decimal _POD_AMOUNT;
        /// <summary>
        /// Tax
        /// </summary>
        [DataMember]
        public global::System.Decimal? POD_TAX
        {
            get
            {
                return _POD_TAX;
            }
            set
            {
                _POD_TAX = value;
            }
        }
        private global::System.Decimal? _POD_TAX;
        /// <summary>
        /// Discount
        /// </summary>
        [DataMember]
        public global::System.Decimal POD_DISC_AMT
        {
            get
            {
                return _POD_DISC_AMT;
            }
            set
            {
                _POD_DISC_AMT = value;
            }
        }
        private global::System.Decimal _POD_DISC_AMT;

        /// <summary>
        /// Total
        /// </summary>

        [DataMember]
        public global::System.Decimal POD_AMT_VALUE
        {
            get
            {
                return _POD_AMT_VALUE;
            }
            set
            {
                _POD_AMT_VALUE = value;
            }
        }
        private global::System.Decimal _POD_AMT_VALUE;

        /// <summary>
        /// Remarks
        /// </summary>

        [DataMember]
        public global::System.String POD_REMARKS
        {
            get
            {
                return _POD_REMARKS;
            }
            set
            {
                _POD_REMARKS = value;
            }
        }
        private global::System.String _POD_REMARKS;

        /// <summary>
        /// Required By
        /// </summary>
        [DataMember]
        public global::System.DateTime POD_REQD_DATE
        {
            get
            {
                return _POD_REQD_DATE;
            }
            set
            {
                _POD_REQD_DATE = value;
            }
        }
        private global::System.DateTime _POD_REQD_DATE;

    }
    [Serializable()]
    [DataContract]
    public class PurchaseOrderPks
    {
        /// <summary>
        /// PO ID or Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int32 POH_PK
        {
            get
            {
                return _POH_PK;
            }
            set
            {
                _POH_PK = value;
            }
        }
        private global::System.Int32 _POH_PK;

    }

    [Serializable()]
    [DataContract]
    public class PoHeader : FIN_INVOICE_VND_TRX_MPG 
    {
        /// <summary>
        /// PO Number
        /// </summary>
        [DataMember]
        public global::System.String POH_NO
        {
            get
            {
                return _POH_NO;
            }
            set
            {
                _POH_NO = value;
            }
        }
        private global::System.String _POH_NO;
        /// <summary>
        /// PO ID or Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int32 POH_PK
        {
            get
            {
                return _POH_PK;
            }
            set
            {
                _POH_PK = value;
            }
        }
        private global::System.Int32 _POH_PK;

        /// <summary>
        /// Vendor ID
        /// </summary>
        /// 
        [DataMember]
        public global::System.Int32 POH_VENDOR
        {
            get
            {
                return _POH_VENDOR;
            }
            set
            {
                _POH_VENDOR = value;
            }
        }
        private global::System.Int32 _POH_VENDOR;

        /// <summary>
        /// Vendor Name
        /// </summary>
        [DataMember]
        public global::System.String POH_VENDOR_TEXT
        {
            get
            {
                return _POH_VENDOR_TEXT;
            }
            set
            {
                _POH_VENDOR_TEXT = value;
            }
        }
        private global::System.String _POH_VENDOR_TEXT;
        /// <summary>
        /// PO Date
        /// </summary>
        [DataMember]
        public global::System.DateTime POH_DATE
        {
            get
            {
                return _POH_DATE;
            }
            set
            {
                _POH_DATE = value;
            }
        }
        private global::System.DateTime _POH_DATE;
        
        /// <summary>
        /// PO Total Value
        /// </summary>
        [DataMember]
        public global::System.Decimal? POH_TOTAL_VALUE
        {
            get
            {
                return _POH_TOTAL_VALUE;
            }
            set
            {
                _POH_TOTAL_VALUE = value;
            }
        }
        private global::System.Decimal? _POH_TOTAL_VALUE;

        /// <summary>
        /// Shipping Dept
        /// </summary>
        [DataMember]
        public global::System.Int32 POH_SHIPPING
        {
            get
            {
                return _POH_SHIPPING;
            }
            set
            {
                _POH_SHIPPING = value;
            }
        }
        private global::System.Int32 _POH_SHIPPING;

        /// <summary>
        /// Shipping Dept Name
        /// </summary>
        [DataMember]
        public global::System.String POH_SHIPPING_TEXT
        {
            get
            {
                return _POH_SHIPPING_TEXT;
            }
            set
            {
                _POH_SHIPPING_TEXT = value;
            }
        }
        private global::System.String _POH_SHIPPING_TEXT;

        /// <summary>
        /// Billing Dept
        /// </summary>
        [DataMember]
        public global::System.Int32 POH_BILLING
        {
            get
            {
                return _POH_BILLING;
            }
            set
            {
                _POH_BILLING = value;
            }
        }
        private global::System.Int32 _POH_BILLING;


        /// <summary>
        /// Billing Dept Name
        /// </summary>
        [DataMember]
        public global::System.String POH_BILLING_TEXT
        {
            get
            {
                return _POH_BILLING_TEXT;
            }
            set
            {
                _POH_BILLING_TEXT = value;
            }
        }
        private global::System.String _POH_BILLING_TEXT;
        /// <summary>
        /// Created By
        /// </summary>
        [DataMember]
        public global::System.Int32 POH_CRTD_BY
        {
            get
            {
                return _POH_CRTD_BY;
            }
            set
            {
                _POH_CRTD_BY = value;
            }
        }
        private global::System.Int32 _POH_CRTD_BY;
        /// <summary>
        /// Created By Name
        /// </summary>
        [DataMember]
        public global::System.String POH_CRTD_BY_TEXT
        {
            get
            {
                return _POH_CRTD_BY_TEXT;
            }
            set
            {
                _POH_CRTD_BY_TEXT = value;
            }
        }
        private global::System.String _POH_CRTD_BY_TEXT;
        /// <summary>
        /// Approved By
        /// </summary>
        [DataMember]
        public global::System.Int32? POH_APPROVED_BY
        {
            get
            {
                return _POH_APPROVED_BY;
            }
            set
            {
                _POH_APPROVED_BY = value;
            }
        }
        private global::System.Int32? _POH_APPROVED_BY;
        /// <summary>
        /// Approved By Name
        /// </summary>
        [DataMember]
        public global::System.String POH_APPROVED_BY_TEXT
        {
            get
            {
                return _POH_APPROVED_BY_TEXT;
            }
            set
            {
                _POH_APPROVED_BY_TEXT = value;
            }
        }
        private global::System.String _POH_APPROVED_BY_TEXT;
        /// <summary>
        /// Type By
        /// </summary>
        [DataMember]
        public global::System.Int16 POH_TYPE
        {
            get
            {
                return _POH_TYPE;
            }
            set
            {
                _POH_TYPE = value;
            }
        }
        private global::System.Int16 _POH_TYPE;

        /// <summary>
        /// Type Name
        /// </summary>
        [DataMember]
        public global::System.String POH_TYPE_TEXT
        {
            get
            {
                return _POH_TYPE_TEXT;
            }
            set
            {
                _POH_TYPE_TEXT = value;
            }
        }
        private global::System.String _POH_TYPE_TEXT;
        /// <summary>
        /// Active
        /// </summary>
        [DataMember]
        public global::System.Int16 POH_ACTIVE
        {
            get
            {
                return _POH_ACTIVE;
            }
            set
            {
                _POH_ACTIVE = value;
            }
        }
        private global::System.Int16 _POH_ACTIVE;

        /// <summary>
        /// POH_SUB_TOTAL
        /// </summary>
        [DataMember]
        public global::System.Decimal? POH_SUB_TOTAL
        {
            get
            {
                return _POH_SUB_TOTAL;
            }
            set
            {
                _POH_SUB_TOTAL = value;
            }
        }
        private global::System.Decimal? _POH_SUB_TOTAL;

        /// <summary>
        /// POH_DISC_AMT
        /// </summary>
        [DataMember]
        public global::System.Decimal? POH_DISC_AMT
        {
            get
            {
                return _POH_DISC_AMT;
            }
            set
            {
                _POH_DISC_AMT = value;
            }
        }
        private global::System.Decimal? _POH_DISC_AMT;

        /// <summary>
        /// POH_ADD_TAX_AMT
        /// </summary>
        [DataMember]
        public global::System.Decimal? POH_ADD_TAX_AMT
        {
            get
            {
                return _POH_ADD_TAX_AMT;
            }
            set
            {
                _POH_ADD_TAX_AMT = value;
            }
        }
        private global::System.Decimal? _POH_ADD_TAX_AMT;


        /// <summary>
        /// Invoiced Amount
        /// </summary>
        [DataMember]
        public global::System.Decimal? POH_INVOICED_AMT
        {
            get
            {
                return _POH_INVOICED_AMT;
            }
            set
            {
                _POH_INVOICED_AMT = value;
            }
        }
        private global::System.Decimal? _POH_INVOICED_AMT;

        /// <summary>
        /// Bal To Invoice Amount
        /// </summary>
        [DataMember]
        public global::System.Decimal? POH_BAL_TO_INV_AMT
        {
            get
            {
                return _POH_BAL_TO_INV_AMT;
            }
            set
            {
                _POH_BAL_TO_INV_AMT = value;
            }
        }
        private global::System.Decimal? _POH_BAL_TO_INV_AMT;

        /// <summary>
        /// Invoiced Now Amount
        /// </summary>
        [DataMember]
        public global::System.Decimal? POH_INVOICED_NOW_AMT
        {
            get
            {
                return _POH_INVOICED_NOW_AMT;
            }
            set
            {
                _POH_INVOICED_NOW_AMT = value;
            }
        }
        private global::System.Decimal? _POH_INVOICED_NOW_AMT;


        /// <summary>
        /// PO Vendor Account ID
        /// </summary>
        [DataMember]
        public global::System.Int32? POH_VEN_ACCOUNT
        {
            get
            {
                return _POH_VEN_ACCOUNT;
            }
            set
            {
                _POH_VEN_ACCOUNT = value;
            }
        }
        private global::System.Int32? _POH_VEN_ACCOUNT;

        /// <summary>
        /// PO Currency ID
        /// </summary>
        [DataMember]
        public global::System.Int32 POH_CURRENCY
        {
            get
            {
                return _POH_CURRENCY;
            }
            set
            {
                _POH_CURRENCY = value;
            }
        }
        private global::System.Int32 _POH_CURRENCY;

        //public FIN_INVOICE_VND_TRX_MPG FIN_INVOICE_VND_TRX_MPG();
    }
}
