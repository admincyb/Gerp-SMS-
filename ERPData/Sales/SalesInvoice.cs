using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ERPData
{
    [Serializable()]
    [DataContract]
    public  class SalesInvoice
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
        public global::System.Int64 ICH_PK
        {
            get
            {
                return _ICH_PK;
            }
            set
            {
                _ICH_PK = value;
            }
        }
        private global::System.Int64 _ICH_PK;

        /// <summary>
        /// Date
        /// </summary>
        [DataMember]
        public global::System.DateTime ICH_DATE
        {
            get
            {
                return _ICH_DATE;
            }
            set
            {
                _ICH_DATE = value;
            }
        }
        private global::System.DateTime _ICH_DATE;

        /// <summary>
        /// Transaction No
        /// </summary>
        [DataMember]
        public global::System.String ICH_NO
        {
            get
            {
                return _ICH_NO;
            }
            set
            {
                _ICH_NO = value;
            }
        }
        private global::System.String _ICH_NO;

        /// <summary>
        /// CUSTOMER PK
        /// </summary>
        /// 
        [DataMember]
        public global::System.Int32 ICH_CUSTOMER
        {
            get
            {
                return _ICH_CUSTOMER;
            }
            set
            {
                _ICH_CUSTOMER = value;
            }
        }
        private global::System.Int32 _ICH_CUSTOMER;

        /// <summary>
        /// CUSTOMER Name
        /// </summary>
        [DataMember]
        public global::System.String ICH_CUSTOMER_TEXT
        {
            get
            {
                return _ICH_CUSTOMER_TEXT;
            }
            set
            {
                _ICH_CUSTOMER_TEXT = value;
            }
        }
        private global::System.String _ICH_CUSTOMER_TEXT;


        /// <summary>
        /// CUSTOMER Invoice No
        /// </summary>
        [DataMember]
        public global::System.String ICH_CUSTOMER_INV_NO
        {
            get
            {
                return _ICH_CUSTOMER_INV_NO;
            }
            set
            {
                _ICH_CUSTOMER_INV_NO = value;
            }
        }
        private global::System.String _ICH_CUSTOMER_INV_NO;


        /// <summary>
        /// CUSTOMER Account
        /// </summary>
        [DataMember]
        public global::System.Int32 ICH_CUSTOMER_ACCOUNT
        {
            get
            {
                return _ICH_CUSTOMER_ACCOUNT;
            }
            set
            {
                _ICH_CUSTOMER_ACCOUNT = value;
            }
        }
        private global::System.Int32 _ICH_CUSTOMER_ACCOUNT;


        /// <summary>
        /// Refrence
        /// </summary>
        [DataMember]
        public global::System.String ICH_REFERENCE
        {
            get
            {
                return _ICH_REFERENCE;
            }
            set
            {
                _ICH_REFERENCE = value;
            }
        }
        private global::System.String _ICH_REFERENCE;



        /// <summary>
        /// Date received
        /// </summary>
        [DataMember]
        public global::System.DateTime ICH_DATE_RECEIVED
        {
            get
            {
                return _ICH_DATE_RECEIVED;
            }
            set
            {
                _ICH_DATE_RECEIVED = value;
            }
        }
        private global::System.DateTime _ICH_DATE_RECEIVED;

        /// <summary>
        /// Date pay by
        /// </summary>
        [DataMember]
        public global::System.DateTime ICH_DATE_PAY_BY
        {
            get
            {
                return _ICH_DATE_PAY_BY;
            }
            set
            {
                _ICH_DATE_PAY_BY = value;
            }
        }
        private global::System.DateTime _ICH_DATE_PAY_BY;

        /// <summary>
        /// Currency
        /// </summary>
        [DataMember]
        public global::System.Int32 ICH_CURRENCY
        {
            get
            {
                return _ICH_CURRENCY;
            }
            set
            {
                _ICH_CURRENCY = value;
            }
        }
        private global::System.Int32 _ICH_CURRENCY;


        /// <summary>
        /// Amount in TC
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_AMOUNT_TC
        {
            get
            {
                return _ICH_AMOUNT_TC;
            }
            set
            {
                _ICH_AMOUNT_TC = value;
            }
        }
        private global::System.Decimal? _ICH_AMOUNT_TC;

        /// <summary>
        /// Amount in TC
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_DISCOUNT_TC
        {
            get
            {
                return _ICH_DISCOUNT_TC;
            }
            set
            {
                _ICH_DISCOUNT_TC = value;
            }
        }
        private global::System.Decimal? _ICH_DISCOUNT_TC;

        /// <summary>
        /// Tax in TC
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_TAX_TC
        {
            get
            {
                return _ICH_TAX_TC;
            }
            set
            {
                _ICH_TAX_TC = value;
            }
        }
        private global::System.Decimal? _ICH_TAX_TC;


        /// <summary>
        /// Net amount in TC
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_AMOUNT_NET_TC
        {
            get
            {
                return _ICH_AMOUNT_NET_TC;
            }
            set
            {
                _ICH_AMOUNT_NET_TC = value;
            }
        }
        private global::System.Decimal? _ICH_AMOUNT_NET_TC;


        /// <summary>
        /// Exchnage Rate
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_EXCHG_RATE
        {
            get
            {
                return _ICH_EXCHG_RATE;
            }
            set
            {
                _ICH_EXCHG_RATE = value;
            }
        }
        private global::System.Decimal? _ICH_EXCHG_RATE;


        /// <summary>
        /// Amount in BC
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_AMOUNT_BC
        {
            get
            {
                return _ICH_AMOUNT_BC;
            }
            set
            {
                _ICH_AMOUNT_BC = value;
            }
        }
        private global::System.Decimal? _ICH_AMOUNT_BC;

        /// <summary>
        /// Amount in BC
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_DISCOUNT_BC
        {
            get
            {
                return _ICH_DISCOUNT_BC;
            }
            set
            {
                _ICH_DISCOUNT_BC = value;
            }
        }
        private global::System.Decimal? _ICH_DISCOUNT_BC;

        /// <summary>
        /// Tax in BC
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_TAX_BC
        {
            get
            {
                return _ICH_TAX_BC;
            }
            set
            {
                _ICH_TAX_BC = value;
            }
        }
        private global::System.Decimal? _ICH_TAX_BC;


        /// <summary>
        /// Net amount in BC
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_AMOUNT_NET_BC
        {
            get
            {
                return _ICH_AMOUNT_NET_BC;
            }
            set
            {
                _ICH_AMOUNT_NET_BC = value;
            }
        }
        private global::System.Decimal? _ICH_AMOUNT_NET_BC;

        /// <summary>
        ///Remarks
        /// </summary>
        [DataMember]
        public global::System.String ICH_REMARKS
        {
            get
            {
                return _ICH_REMARKS;
            }
            set
            {
                _ICH_REMARKS = value;
            }
        }
        private global::System.String _ICH_REMARKS;


        /// <summary>
        ///Invoiced amount
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_INVOICED_AMOUNT
        {
            get
            {
                return _ICH_INVOICED_AMOUNT;
            }
            set
            {
                _ICH_INVOICED_AMOUNT = value;
            }
        }
        private global::System.Decimal? _ICH_INVOICED_AMOUNT;

        /// <summary>
        ///Paid Amount against this invoice through previous RECEIPTs
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_RCVD_AMOUNT
        {
            get
            {
                return _ICH_RCVD_AMOUNT;
            }
            set
            {
                _ICH_RCVD_AMOUNT = value;
            }
        }
        private global::System.Decimal? _ICH_RCVD_AMOUNT;


        /// <summary>
        ///balance to pay
        /// </summary>
        [DataMember]
        public global::System.Decimal? ICH_BAL_AMOUNT
        {
            get
            {
                return _ICH_BAL_AMOUNT;
            }
            set
            {
                _ICH_BAL_AMOUNT = value;
            }
        }
        private global::System.Decimal? _ICH_BAL_AMOUNT;

        //data members for RECEIPT section

        /// <summary>
        ///RECEIPT Mpg PK
        /// </summary>
        [DataMember]
        public global::System.Int64 RCM_PK
        {
            get
            {
                return _RCM_PK;
            }
            set
            {
                _RCM_PK = value;
            }
        }
        private global::System.Int64 _RCM_PK;


        /// <summary>
        ///RECEIPT Hdr PK
        /// </summary>
        [DataMember]
        public global::System.Int64 RCM_RECEIPT_HDR
        {
            get
            {
                return _RCM_RECEIPT_HDR;
            }
            set
            {
                _RCM_RECEIPT_HDR = value;
            }
        }
        private global::System.Int64 _RCM_RECEIPT_HDR;

        /// <summary>
        ///RECEIPT Trx Type
        /// </summary>
        [DataMember]
        public global::System.Int16 RCM_TRX_TYPE
        {
            get
            {
                return _RCM_TRX_TYPE;
            }
            set
            {
                _RCM_TRX_TYPE = value;
            }
        }
        private global::System.Int16 _RCM_TRX_TYPE;

        /// <summary>
        ///RECEIPT Trx Type
        /// </summary>
        [DataMember]
        public global::System.Int64 RCM_TRX_PK
        {
            get
            {
                return _RCM_TRX_PK;
            }
            set
            {
                _RCM_TRX_PK = value;
            }
        }
        private global::System.Int64 _RCM_TRX_PK;

        /// <summary>
        ///Paid Now
        /// </summary>
        [DataMember]
        public global::System.Decimal? RCM_RCVD_AMOUNT
        {
            get
            {
                return _RCM_RCVD_AMOUNT;
            }
            set
            {
                _RCM_RCVD_AMOUNT = value;
            }
        }
        private global::System.Decimal? _RCM_RCVD_AMOUNT;


        /// <summary>
        /// Date
        /// </summary>
        [DataMember]
        public global::System.DateTime ICH_MOD_DT
        {
            get
            {
                return _ICH_MOD_DT;
            }
            set
            {
                _ICH_MOD_DT = value;
            }
        }
        private global::System.DateTime _ICH_MOD_DT;
    }
}
