using System;
using System.Runtime.Serialization;


namespace ERPData
{
    [Serializable()]
    [DataContract]
    public partial class FinTrxData
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
       /// 
       /// </summary>
        [DataMember]
        public global::System.Int64 FTR_PK
        {
            get
            {
                return _FTR_PK;
            }
            set
            {
                _FTR_PK = value;
            }
        }
        private global::System.Int64 _FTR_PK;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public global::System.DateTime FTR_DATE
        {
            get
            {
                return _FTR_DATE;
            }
            set
            {
                _FTR_DATE = value;
            }
        }
        private global::System.DateTime _FTR_DATE;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public global::System.String FTR_REF_TYPE
        {
            get
            {
                return _FTR_REF_TYPE;
            }
            set
            {
                _FTR_REF_TYPE = value;
            }
        }
        private global::System.String _FTR_REF_TYPE;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public global::System.String FTR_REF_NO
        {
            get
            {
                return _FTR_REF_NO;
            }
            set
            {
                _FTR_REF_NO = value;
            }
        }
        private global::System.String _FTR_REF_NO;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public global::System.Decimal FTR_CR_AMT_BC
        {
            get
            {
                return _FTR_CR_AMT_BC;
            }
            set
            {
                _FTR_CR_AMT_BC = value;
            }
        }
        private global::System.Decimal _FTR_CR_AMT_BC;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public global::System.Decimal FTR_DR_AMT_BC
        {
            get
            {
                return _FTR_DR_AMT_BC;
            }
            set
            {
                _FTR_DR_AMT_BC = value;
            }
        }
        private global::System.Decimal _FTR_DR_AMT_BC;

    }
}
