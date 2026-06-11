using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ERPData
{
    [Serializable()]
    [DataContract]
    public class SalesReceipt
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
        public global::System.String RCH_NO
        {
            get
            {
                return _RCH_NO;
            }
            set
            {
                _RCH_NO = value;
            }
        }
        private global::System.String _RCH_NO;
        /// <summary>
        /// Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int64 RCH_PK
        {
            get
            {
                return _RCH_PK;
            }
            set
            {
                _RCH_PK = value;
            }
        }
        private global::System.Int64 _RCH_PK;

        /// <summary>
        /// Date
        /// </summary>
        /// 
        [DataMember]
        public global::System.DateTime RCH_DATE
        {
            get
            {
                return _RCH_DATE;
            }
            set
            {
                _RCH_DATE = value;
            }
        }
        private global::System.DateTime _RCH_DATE;

        /// <summary>
        /// CUSTOMER PK
        /// </summary>
        [DataMember]
        public global::System.Int32 RCH_CUSTOMER
        {
            get
            {
                return _RCH_CUSTOMER;
            }
            set
            {
                _RCH_CUSTOMER = value;
            }
        }
        private global::System.Int32 _RCH_CUSTOMER;
        /// <summary>
        /// CUSTOMER Name
        /// </summary>
        [DataMember]
        public global::System.String RCH_CUSTOMER_TEXT
        {
            get
            {
                return _RCH_CUSTOMER_TEXT;
            }
            set
            {
                _RCH_CUSTOMER_TEXT = value;
            }
        }
        private global::System.String _RCH_CUSTOMER_TEXT;

       

        /// <summary>
        /// Mode Value
        /// </summary>
        [DataMember]
        public global::System.Decimal? RCH_MODE
        {
            get
            {
                return _RCH_MODE;
            }
            set
            {
                _RCH_MODE = value;
            }
        }
        private global::System.Decimal? _RCH_MODE;

        /// <summary>
        /// Mode Text
        /// </summary>
        [DataMember]
        public global::System.Decimal? RCH_MODE_TEXT
        {
            get
            {
                return _RCH_MODE_TEXT;
            }
            set
            {
                _RCH_MODE_TEXT = value;
            }
        }
        private global::System.Decimal? _RCH_MODE_TEXT;

        /// <summary>
        /// Bank PK
        /// </summary>
        [DataMember]
        public global::System.Int16? RCH_BANK
        {
            get
            {
                return _RCH_BANK;
            }
            set
            {
                _RCH_BANK = value;
            }
        }
        private global::System.Int16? _RCH_BANK;

        /// <summary>
        /// Bank Name
        /// </summary>
        [DataMember]
        public global::System.String RCH_BANK_TEXT
        {
            get
            {
                return _RCH_BANK_TEXT;
            }
            set
            {
                _RCH_BANK_TEXT = value;
            }
        }
        private global::System.String _RCH_BANK_TEXT;

       

        /// <summary>
        /// CUSTOMER account
        /// </summary>
        [DataMember]
        public global::System.Int32 RCH_CUSTOMER_ACCOUNT
        {
            get
            {
                return _RCH_CUSTOMER_ACCOUNT;
            }
            set
            {
                _RCH_CUSTOMER_ACCOUNT = value;
            }
        }
        private global::System.Int32 _RCH_CUSTOMER_ACCOUNT;

        /// <summary>
        /// CUSTOMER account name
        /// </summary>
        [DataMember]
        public global::System.String RCH_CUSTOMER_ACCOUNT_TEXT
        {
            get
            {
                return _RCH_CUSTOMER_ACCOUNT_TEXT;
            }
            set
            {
                _RCH_CUSTOMER_ACCOUNT_TEXT = value;
            }
        }
        private global::System.String _RCH_CUSTOMER_ACCOUNT_TEXT;


        /// <summary>
        /// Bank Acc No
        /// </summary>
        [DataMember]
        public global::System.String RCH_ACC_NO
        {
            get
            {
                return _RCH_ACC_NO;
            }
            set
            {
                _RCH_ACC_NO = value;
            }
        }
        private global::System.String _RCH_ACC_NO;

        /// <summary>
        /// Branch
        /// </summary>
        [DataMember]
        public global::System.String RCH_BRANCH
        {
            get
            {
                return _RCH_BRANCH;
            }
            set
            {
                _RCH_BRANCH = value;
            }
        }
        private global::System.String _RCH_BRANCH;

        /// <summary>
        /// Instrument
        /// </summary>
        [DataMember]
        public global::System.String RCH_INSTRUMENT
        {
            get
            {
                return _RCH_INSTRUMENT;
            }
            set
            {
                _RCH_INSTRUMENT = value;
            }
        }
        private global::System.String _RCH_INSTRUMENT;

        /// <summary>
        /// Ban-Cash Account
        /// </summary>
        [DataMember]
        public global::System.Int32 RCH_BANK_CASH_ACCOUNT
        {
            get
            {
                return _RCH_BANK_CASH_ACCOUNT;
            }
            set
            {
                _RCH_BANK_CASH_ACCOUNT = value;
            }
        }
        private global::System.Int32 _RCH_BANK_CASH_ACCOUNT;


        /// <summary>
        /// Ban-Cash Account name
        /// </summary>
        [DataMember]
        public global::System.String RCH_BANK_CASH_ACCOUNT_TEXT
        {
            get
            {
                return _RCH_BANK_CASH_ACCOUNT_TEXT;
            }
            set
            {
                _RCH_BANK_CASH_ACCOUNT_TEXT = value;
            }
        }
        private global::System.String _RCH_BANK_CASH_ACCOUNT_TEXT;

        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public global::System.Decimal? RCH_RCVD_AMOUNT
        {
            get
            {
                return _RCH_RCVD_AMOUNT;
            }
            set
            {
                _RCH_RCVD_AMOUNT = value;
            }
        }
        private global::System.Decimal? _RCH_RCVD_AMOUNT;

        /// <summary>
        /// Remarks
        /// </summary>
        [DataMember]
        public global::System.String RCH_REMARKS
        {
            get
            {
                return _RCH_REMARKS;
            }
            set
            {
                _RCH_REMARKS = value;
            }
        }
        private global::System.String _RCH_REMARKS;

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public global::System.Int16 RCH_STATUS
        {
            get
            {
                return _RCH_STATUS;
            }
            set
            {
                _RCH_STATUS = value;
            }
        }
        private global::System.Int16 _RCH_STATUS;

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public global::System.DateTime RCH_MOD_DT
        {
            get
            {
                return _RCH_MOD_DT;
            }
            set
            {
                _RCH_MOD_DT = value;
            }
        }
        private global::System.DateTime _RCH_MOD_DT;
        /// <summary>
        /// Active
        /// </summary>
        [DataMember]
        public global::System.Int16 RCH_ACTIVE
        {
            get
            {
                return _RCH_ACTIVE;
            }
            set
            {
                _RCH_ACTIVE = value;
            }
        }
        private global::System.Int16 _RCH_ACTIVE;

        /// <summary>
        /// Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int32 RCH_BIZUNIT
        {
            get
            {
                return _RCH_BIZUNIT;
            }
            set
            {
                _RCH_BIZUNIT = value;
            }
        }
        private global::System.Int32 _RCH_BIZUNIT;
    }
}
