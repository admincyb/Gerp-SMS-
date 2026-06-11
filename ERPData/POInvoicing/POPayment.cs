using System;
using System.Runtime.Serialization;

namespace ERPData
{
    [Serializable()]
    [DataContract]
    public class POPayment
    {
        /// <summary>
        /// Total Row Count
        /// </summary>
        [DataMember]
        public global::System.Int32 ROW_COUNT
        {
            get
            {
                return _ROW_COUNT;
            }
            set
            {
                _ROW_COUNT = value;
            }
        }
        private global::System.Int32 _ROW_COUNT;

        /// <summary>
        /// Transaction No
        /// </summary>
        [DataMember]
        public global::System.String PVH_NO
        {
            get
            {
                return _PVH_NO;
            }
            set
            {
                _PVH_NO = value;
            }
        }
        private global::System.String _PVH_NO;
        /// <summary>
        /// Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int64 PVH_PK
        {
            get
            {
                return _PVH_PK;
            }
            set
            {
                _PVH_PK = value;
            }
        }
        private global::System.Int64 _PVH_PK;

        /// <summary>
        /// Date
        /// </summary>
        /// 
        [DataMember]
        public global::System.DateTime PVH_DATE
        {
            get
            {
                return _PVH_DATE;
            }
            set
            {
                _PVH_DATE = value;
            }
        }
        private global::System.DateTime _PVH_DATE;

        /// <summary>
        /// Vendor PK
        /// </summary>
        [DataMember]
        public global::System.Int32 PVH_VENDOR
        {
            get
            {
                return _PVH_VENDOR;
            }
            set
            {
                _PVH_VENDOR = value;
            }
        }
        private global::System.Int32 _PVH_VENDOR;
        /// <summary>
        /// Vendor Name
        /// </summary>
        [DataMember]
        public global::System.String PVH_VENDOR_TEXT
        {
            get
            {
                return _PVH_VENDOR_TEXT;
            }
            set
            {
                _PVH_VENDOR_TEXT = value;
            }
        }
        private global::System.String _PVH_VENDOR_TEXT;

       

        /// <summary>
        /// Mode Value
        /// </summary>
        [DataMember]
        public global::System.Decimal? PVH_MODE
        {
            get
            {
                return _PVH_MODE;
            }
            set
            {
                _PVH_MODE = value;
            }
        }
        private global::System.Decimal? _PVH_MODE;

        /// <summary>
        /// Mode Text
        /// </summary>
        [DataMember]
        public global::System.Decimal? PVH_MODE_TEXT
        {
            get
            {
                return _PVH_MODE_TEXT;
            }
            set
            {
                _PVH_MODE_TEXT = value;
            }
        }
        private global::System.Decimal? _PVH_MODE_TEXT;

        /// <summary>
        /// Bank PK
        /// </summary>
        [DataMember]
        public global::System.Int16? PVH_BANK
        {
            get
            {
                return _PVH_BANK;
            }
            set
            {
                _PVH_BANK = value;
            }
        }
        private global::System.Int16? _PVH_BANK;

        /// <summary>
        /// Bank Name
        /// </summary>
        [DataMember]
        public global::System.String PVH_BANK_TEXT
        {
            get
            {
                return _PVH_BANK_TEXT;
            }
            set
            {
                _PVH_BANK_TEXT = value;
            }
        }
        private global::System.String _PVH_BANK_TEXT;

       

        /// <summary>
        /// Vendor account
        /// </summary>
        [DataMember]
        public global::System.Int32 PVH_VENDOR_ACCOUNT
        {
            get
            {
                return _PVH_VENDOR_ACCOUNT;
            }
            set
            {
                _PVH_VENDOR_ACCOUNT = value;
            }
        }
        private global::System.Int32 _PVH_VENDOR_ACCOUNT;

        /// <summary>
        /// Vendor account name
        /// </summary>
        [DataMember]
        public global::System.String PVH_VENDOR_ACCOUNT_TEXT
        {
            get
            {
                return _PVH_VENDOR_ACCOUNT_TEXT;
            }
            set
            {
                _PVH_VENDOR_ACCOUNT_TEXT = value;
            }
        }
        private global::System.String _PVH_VENDOR_ACCOUNT_TEXT;


        /// <summary>
        /// Bank Acc No
        /// </summary>
        [DataMember]
        public global::System.String PVH_ACC_NO
        {
            get
            {
                return _PVH_ACC_NO;
            }
            set
            {
                _PVH_ACC_NO = value;
            }
        }
        private global::System.String _PVH_ACC_NO;

        /// <summary>
        /// Branch
        /// </summary>
        [DataMember]
        public global::System.String PVH_BRANCH
        {
            get
            {
                return _PVH_BRANCH;
            }
            set
            {
                _PVH_BRANCH = value;
            }
        }
        private global::System.String _PVH_BRANCH;

        /// <summary>
        /// Instrument
        /// </summary>
        [DataMember]
        public global::System.String PVH_INSTRUMENT
        {
            get
            {
                return _PVH_INSTRUMENT;
            }
            set
            {
                _PVH_INSTRUMENT = value;
            }
        }
        private global::System.String _PVH_INSTRUMENT;

        /// <summary>
        /// Ban-Cash Account
        /// </summary>
        [DataMember]
        public global::System.Int32 PVH_BANK_CASH_ACCOUNT
        {
            get
            {
                return _PVH_BANK_CASH_ACCOUNT;
            }
            set
            {
                _PVH_BANK_CASH_ACCOUNT = value;
            }
        }
        private global::System.Int32 _PVH_BANK_CASH_ACCOUNT;


        /// <summary>
        /// Ban-Cash Account name
        /// </summary>
        [DataMember]
        public global::System.String PVH_BANK_CASH_ACCOUNT_TEXT
        {
            get
            {
                return _PVH_BANK_CASH_ACCOUNT_TEXT;
            }
            set
            {
                _PVH_BANK_CASH_ACCOUNT_TEXT = value;
            }
        }
        private global::System.String _PVH_BANK_CASH_ACCOUNT_TEXT;

        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public global::System.Decimal? PVH_PAID_AMOUNT
        {
            get
            {
                return _PVH_PAID_AMOUNT;
            }
            set
            {
                _PVH_PAID_AMOUNT = value;
            }
        }
        private global::System.Decimal? _PVH_PAID_AMOUNT;

        /// <summary>
        /// Remarks
        /// </summary>
        [DataMember]
        public global::System.String PVH_REMARKS
        {
            get
            {
                return _PVH_REMARKS;
            }
            set
            {
                _PVH_REMARKS = value;
            }
        }
        private global::System.String _PVH_REMARKS;

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public global::System.Int16 PVH_STATUS
        {
            get
            {
                return _PVH_STATUS;
            }
            set
            {
                _PVH_STATUS = value;
            }
        }
        private global::System.Int16 _PVH_STATUS;

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public global::System.DateTime PVH_MOD_DT
        {
            get
            {
                return _PVH_MOD_DT;
            }
            set
            {
                _PVH_MOD_DT = value;
            }
        }
        private global::System.DateTime _PVH_MOD_DT;
        /// <summary>
        /// Active
        /// </summary>
        [DataMember]
        public global::System.Int16 PVH_ACTIVE
        {
            get
            {
                return _PVH_ACTIVE;
            }
            set
            {
                _PVH_ACTIVE = value;
            }
        }
        private global::System.Int16 _PVH_ACTIVE;

        /// <summary>
        /// Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int32 PVH_BIZUNIT
        {
            get
            {
                return _PVH_BIZUNIT;
            }
            set
            {
                _PVH_BIZUNIT = value;
            }
        }
        private global::System.Int32 _PVH_BIZUNIT;
    }
}
