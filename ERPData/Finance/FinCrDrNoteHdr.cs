using System;
using System.Runtime.Serialization;


namespace ERPData
{
    [Serializable()]
    [DataContract]
    public class FinCrDrNoteHdr
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
        /// Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int64 CDH_PK
        {
            get
            {
                return _CDH_PK;
            }
            set
            {
                _CDH_PK = value;
            }
        }
        private global::System.Int64 _CDH_PK;

        /// <summary>
        /// Transaction No
        /// </summary>
        [DataMember]
        public global::System.String CDH_NO
        {
            get
            {
                return _CDH_NO;
            }
            set
            {
                _CDH_NO = value;
            }
        }
        private global::System.String _CDH_NO;
       

        /// <summary>
        /// Date
        /// </summary>
        /// 
        [DataMember]
        public global::System.DateTime CDH_DATE
        {
            get
            {
                return _CDH_DATE;
            }
            set
            {
                _CDH_DATE = value;
            }
        }
        private global::System.DateTime _CDH_DATE;

        /// <summary>
        /// Type - CR or DR
        /// </summary>
        /// 
        [DataMember]
        public global::System.Int16 CDH_TYPE
        {
            get
            {
                return _CDH_TYPE;
            }
            set
            {
                _CDH_TYPE = value;
            }
        }
        private global::System.Int16 _CDH_TYPE;

        /// <summary>
        /// Party Type - Vendor or customer
        /// </summary>
        /// 
        [DataMember]
        public global::System.Int16 CDH_PARTY_TYPE
        {
            get
            {
                return _CDH_PARTY_TYPE;
            }
            set
            {
                _CDH_PARTY_TYPE = value;
            }
        }
        private global::System.Int16 _CDH_PARTY_TYPE;

        /// <summary>
        /// Party PK
        /// </summary>
        [DataMember]
        public global::System.Int32 CDH_PARTY_PK
        {
            get
            {
                return _CDH_PARTY_PK;
            }
            set
            {
                _CDH_PARTY_PK = value;
            }
        }
        private global::System.Int32 _CDH_PARTY_PK;
        /// <summary>
        /// Vendor Name
        /// </summary>
        [DataMember]
        public global::System.String CDH_PARTY_TEXT
        {
            get
            {
                return _CDH_PARTY_TEXT;
            }
            set
            {
                _CDH_PARTY_TEXT = value;
            }
        }
        private global::System.String _CDH_PARTY_TEXT;

        /// <summary>
        /// Vendor account
        /// </summary>
        [DataMember]
        public global::System.Int32 CDH_VENDOR_ACCOUNT
        {
            get
            {
                return _CDH_VENDOR_ACCOUNT;
            }
            set
            {
                _CDH_VENDOR_ACCOUNT = value;
            }
        }
        private global::System.Int32 _CDH_VENDOR_ACCOUNT;

        /// <summary>
        /// Vendor account name
        /// </summary>
        [DataMember]
        public global::System.String CDH_VENDOR_ACCOUNT_TEXT
        {
            get
            {
                return _CDH_VENDOR_ACCOUNT_TEXT;
            }
            set
            {
                _CDH_VENDOR_ACCOUNT_TEXT = value;
            }
        }
        private global::System.String _CDH_VENDOR_ACCOUNT_TEXT;


        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public global::System.Decimal? CDH_AMOUNT_TC
        {
            get
            {
                return _CDH_AMOUNT_TC;
            }
            set
            {
                _CDH_AMOUNT_TC = value;
            }
        }
        private global::System.Decimal? _CDH_AMOUNT_TC;

        /// <summary>
        /// Remarks
        /// </summary>
        [DataMember]
        public global::System.String CDH_REMARKS
        {
            get
            {
                return _CDH_REMARKS;
            }
            set
            {
                _CDH_REMARKS = value;
            }
        }
        private global::System.String _CDH_REMARKS;

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public global::System.Int16 CDH_STATUS
        {
            get
            {
                return _CDH_STATUS;
            }
            set
            {
                _CDH_STATUS = value;
            }
        }
        private global::System.Int16 _CDH_STATUS;

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public global::System.DateTime CDH_MOD_DT
        {
            get
            {
                return _CDH_MOD_DT;
            }
            set
            {
                _CDH_MOD_DT = value;
            }
        }
        private global::System.DateTime _CDH_MOD_DT;
        /// <summary>
        /// Active
        /// </summary>
        [DataMember]
        public global::System.Int16 CDH_ACTIVE
        {
            get
            {
                return _CDH_ACTIVE;
            }
            set
            {
                _CDH_ACTIVE = value;
            }
        }
        private global::System.Int16 _CDH_ACTIVE;

        /// <summary>
        /// Primary Key
        /// </summary>
        [DataMember]
        public global::System.Int32 CDH_BIZUNIT
        {
            get
            {
                return _CDH_BIZUNIT;
            }
            set
            {
                _CDH_BIZUNIT = value;
            }
        }
        private global::System.Int32 _CDH_BIZUNIT;
    }
}
