using System;
using System.Runtime.Serialization;

namespace ERPData
{
   /* [Serializable()]
    [DataContract]
    public class POInvoice
    {
    }*/

    [Serializable()]
    [DataContract]
    public class POInvoice
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
        public global::System.Int64 IVH_PK
        {
            get
            {
                return _IVH_PK;
            }
            set
            {
                _IVH_PK = value;
            }
        }
        private global::System.Int64 _IVH_PK;

        /// <summary>
        /// Date
        /// </summary>
        [DataMember]
        public global::System.DateTime IVH_DATE
        {
            get
            {
                return _IVH_DATE;
            }
            set
            {
                _IVH_DATE = value;
            }
        }
        private global::System.DateTime _IVH_DATE;

        /// <summary>
        /// Transaction No
        /// </summary>
        [DataMember]
        public global::System.String IVH_NO
        {
            get
            {
                return _IVH_NO;
            }
            set
            {
                _IVH_NO = value;
            }
        }
        private global::System.String _IVH_NO;

        /// <summary>
        /// Vendor PK
        /// </summary>
        /// 
        [DataMember]
        public global::System.Int32 IVH_VENDOR
        {
            get
            {
                return _IVH_VENDOR;
            }
            set
            {
                _IVH_VENDOR = value;
            }
        }
        private global::System.Int32 _IVH_VENDOR;

        /// <summary>
        /// Vendor Name
        /// </summary>
        [DataMember]
        public global::System.String IVH_VENDOR_TEXT
        {
            get
            {
                return _IVH_VENDOR_TEXT;
            }
            set
            {
                _IVH_VENDOR_TEXT = value;
            }
        }
        private global::System.String _IVH_VENDOR_TEXT;


        /// <summary>
        /// Vendor Invoice No
        /// </summary>
        [DataMember]
        public global::System.String IVH_VENDOR_INV_NO
        {
            get
            {
                return _IVH_VENDOR_INV_NO;
            }
            set
            {
                _IVH_VENDOR_INV_NO = value;
            }
        }
        private global::System.String _IVH_VENDOR_INV_NO;


        /// <summary>
        /// Vendor Account
        /// </summary>
        [DataMember]
        public global::System.Int32 IVH_VENDOR_ACCOUNT
        {
            get
            {
                return _IVH_VENDOR_ACCOUNT;
            }
            set
            {
                _IVH_VENDOR_ACCOUNT = value;
            }
        }
        private global::System.Int32 _IVH_VENDOR_ACCOUNT;


        /// <summary>
        /// Refrence
        /// </summary>
        [DataMember]
        public global::System.String IVH_REFERENCE
        {
            get
            {
                return _IVH_REFERENCE;
            }
            set
            {
                _IVH_REFERENCE = value;
            }
        }
        private global::System.String _IVH_REFERENCE;



        /// <summary>
        /// Date received
        /// </summary>
        [DataMember]
        public global::System.DateTime IVH_DATE_RECEIVED
        {
            get
            {
                return _IVH_DATE_RECEIVED;
            }
            set
            {
                _IVH_DATE_RECEIVED = value;
            }
        }
        private global::System.DateTime _IVH_DATE_RECEIVED;

        /// <summary>
        /// Date pay by
        /// </summary>
        [DataMember]
        public global::System.DateTime IVH_DATE_PAY_BY
        {
            get
            {
                return _IVH_DATE_PAY_BY;
            }
            set
            {
                _IVH_DATE_PAY_BY = value;
            }
        }
        private global::System.DateTime _IVH_DATE_PAY_BY;

        /// <summary>
        /// Currency
        /// </summary>
        [DataMember]
        public global::System.Int32 IVH_CURRENCY
        {
            get
            {
                return _IVH_CURRENCY;
            }
            set
            {
                _IVH_CURRENCY = value;
            }
        }
        private global::System.Int32 _IVH_CURRENCY;


        /// <summary>
        /// Amount in TC
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_AMOUNT_TC
        {
            get
            {
                return _IVH_AMOUNT_TC;
            }
            set
            {
                _IVH_AMOUNT_TC = value;
            }
        }
        private global::System.Decimal? _IVH_AMOUNT_TC;

        /// <summary>
        /// Amount in TC
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_DISCOUNT_TC
        {
            get
            {
                return _IVH_DISCOUNT_TC;
            }
            set
            {
                _IVH_DISCOUNT_TC = value;
            }
        }
        private global::System.Decimal? _IVH_DISCOUNT_TC;

        /// <summary>
        /// Tax in TC
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_TAX_TC
        {
            get
            {
                return _IVH_TAX_TC;
            }
            set
            {
                _IVH_TAX_TC = value;
            }
        }
        private global::System.Decimal? _IVH_TAX_TC;


        /// <summary>
        /// Net amount in TC
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_AMOUNT_NET_TC
        {
            get
            {
                return _IVH_AMOUNT_NET_TC;
            }
            set
            {
                _IVH_AMOUNT_NET_TC = value;
            }
        }
        private global::System.Decimal? _IVH_AMOUNT_NET_TC;


        /// <summary>
        /// Exchnage Rate
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_EXCHG_RATE
        {
            get
            {
                return _IVH_EXCHG_RATE;
            }
            set
            {
                _IVH_EXCHG_RATE = value;
            }
        }
        private global::System.Decimal? _IVH_EXCHG_RATE;


        /// <summary>
        /// Amount in BC
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_AMOUNT_BC
        {
            get
            {
                return _IVH_AMOUNT_BC;
            }
            set
            {
                _IVH_AMOUNT_BC = value;
            }
        }
        private global::System.Decimal? _IVH_AMOUNT_BC;

        /// <summary>
        /// Amount in BC
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_DISCOUNT_BC
        {
            get
            {
                return _IVH_DISCOUNT_BC;
            }
            set
            {
                _IVH_DISCOUNT_BC = value;
            }
        }
        private global::System.Decimal? _IVH_DISCOUNT_BC;

        /// <summary>
        /// Tax in BC
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_TAX_BC
        {
            get
            {
                return _IVH_TAX_BC;
            }
            set
            {
                _IVH_TAX_BC = value;
            }
        }
        private global::System.Decimal? _IVH_TAX_BC;


        /// <summary>
        /// Net amount in BC
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_AMOUNT_NET_BC
        {
            get
            {
                return _IVH_AMOUNT_NET_BC;
            }
            set
            {
                _IVH_AMOUNT_NET_BC = value;
            }
        }
        private global::System.Decimal? _IVH_AMOUNT_NET_BC;

        /// <summary>
        ///Remarks
        /// </summary>
        [DataMember]
        public global::System.String IVH_REMARKS
        {
            get
            {
                return _IVH_REMARKS;
            }
            set
            {
                _IVH_REMARKS = value;
            }
        }
        private global::System.String _IVH_REMARKS;


        /// <summary>
        ///Invoiced amount
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_INVOICED_AMOUNT
        {
            get
            {
                return _IVH_INVOICED_AMOUNT;
            }
            set
            {
                _IVH_INVOICED_AMOUNT = value;
            }
        }
        private global::System.Decimal? _IVH_INVOICED_AMOUNT;

        /// <summary>
        ///Paid Amount against this invoice through previous payments
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_PAID_AMOUNT
        {
            get
            {
                return _IVH_PAID_AMOUNT;
            }
            set
            {
                _IVH_PAID_AMOUNT = value;
            }
        }
        private global::System.Decimal? _IVH_PAID_AMOUNT;


        /// <summary>
        ///balance to pay
        /// </summary>
        [DataMember]
        public global::System.Decimal? IVH_BAL_AMOUNT
        {
            get
            {
                return _IVH_BAL_AMOUNT;
            }
            set
            {
                _IVH_BAL_AMOUNT = value;
            }
        }
        private global::System.Decimal? _IVH_BAL_AMOUNT;

        /// <summary>
        /// Date
        /// </summary>
        [DataMember]
        public global::System.DateTime IVH_MOD_DT
        {
            get
            {
                return _IVH_MOD_DT;
            }
            set
            {
                _IVH_MOD_DT = value;
            }
        }
        private global::System.DateTime _IVH_MOD_DT;

        //data members for payment section

        /// <summary>
        ///Payment Mpg PK
        /// </summary>
        [DataMember]
        public global::System.Int64 PVM_PK
        {
            get
            {
                return _PVM_PK;
            }
            set
            {
                _PVM_PK = value;
            }
        }
        private global::System.Int64 _PVM_PK;


        /// <summary>
        ///Payment Hdr PK
        /// </summary>
        [DataMember]
        public global::System.Int64 PVM_PAYMENT_HDR
        {
            get
            {
                return _PVM_PAYMENT_HDR;
            }
            set
            {
                _PVM_PAYMENT_HDR = value;
            }
        }
        private global::System.Int64 _PVM_PAYMENT_HDR;

        /// <summary>
        ///Payment Trx Type
        /// </summary>
        [DataMember]
        public global::System.Int16 PVM_TRX_TYPE
        {
            get
            {
                return _PVM_TRX_TYPE;
            }
            set
            {
                _PVM_TRX_TYPE = value;
            }
        }
        private global::System.Int16 _PVM_TRX_TYPE;

        /// <summary>
        ///Payment Trx Type
        /// </summary>
        [DataMember]
        public global::System.Int64 PVM_TRX_PK
        {
            get
            {
                return _PVM_TRX_PK;
            }
            set
            {
                _PVM_TRX_PK = value;
            }
        }
        private global::System.Int64 _PVM_TRX_PK;

        /// <summary>
        ///Paid Now
        /// </summary>
        [DataMember]
        public global::System.Decimal? PVM_PAID_AMOUNT
        {
            get
            {
                return _PVM_PAID_AMOUNT;
            }
            set
            {
                _PVM_PAID_AMOUNT = value;
            }
        }
        private global::System.Decimal? _PVM_PAID_AMOUNT;


        //data members for CRDR note

        /// <summary>
        ///Mpg PK
        /// </summary>
        [DataMember]
        public global::System.Int64 CDM_PK
        {
            get
            {
                return _CDM_PK;
            }
            set
            {
                _CDM_PK = value;
            }
        }
        private global::System.Int64 _CDM_PK;


        /// <summary>
        ///Hdr PK
        /// </summary>
        [DataMember]
        public global::System.Int64 CDM_CRDR_NOTE_HDR
        {
            get
            {
                return _CDM_CRDR_NOTE_HDR;
            }
            set
            {
                _CDM_CRDR_NOTE_HDR = value;
            }
        }
        private global::System.Int64 _CDM_CRDR_NOTE_HDR;

        /// <summary>
        ///Payment Trx Type
        /// </summary>
        [DataMember]
        public global::System.Int16 CDM_TRX_TYPE
        {
            get
            {
                return _CDM_TRX_TYPE;
            }
            set
            {
                _CDM_TRX_TYPE = value;
            }
        }
        private global::System.Int16 _CDM_TRX_TYPE;

        /// <summary>
        ///Trx Pk
        /// </summary>
        [DataMember]
        public global::System.Int64 CDM_TRX_PK
        {
            get
            {
                return _CDM_TRX_PK;
            }
            set
            {
                _CDM_TRX_PK = value;
            }
        }
        private global::System.Int64 _CDM_TRX_PK;

        /// <summary>
        ///Amount
        /// </summary>
        [DataMember]
        public global::System.Decimal? CDM_AMOUNT
        {
            get
            {
                return _CDM_AMOUNT;
            }
            set
            {
                _CDM_AMOUNT = value;
            }
        }
        private global::System.Decimal? _CDM_AMOUNT;
    }
}
