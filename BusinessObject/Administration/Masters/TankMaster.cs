using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Masters
{
    [Serializable]
    [XmlRoot("Root")]
    public class TankMaster
    {
        public string TankName
        {
            get;
            set;
        }
        public int TankTypePk
        {
            get;
            set;
        }
        public int TankMasterPk
        {
            get;
            set;
        }
        public string TankTypeName
        {
            get;
            set;
        }
       
        public int LocationPk
        {
            get;
            set;
        }
        public int Location
        {
            get;
            set;
        }
        public int TankType
        {
            get;
            set;
        }
        public string LocationName
        {
            get;
            set;
        }
        public string TankCapacity
        {
            get;
            set;
        }
        public string UOM
        {
            get;
            set;
        }
        public int? UOMPk
        {
            get;
            set;
        }
        public int UserPk
        {
            get;
            set;
        }
        public int SBU
        {
            get;
            set;
        }
        public string TankCode
        {
            get;
            set;
        }
        public int Line
        {
            get;
            set;
        }
        public int TNK_PLANT
        {
            get;
            set;
        }
        public string Sequence
        {
            get;
            set;
        }
        public string Remarks
        {
            get;
            set;
        }
        public decimal TankHeight
        {
            get;
            set;
        }
        public decimal CapacityperCM
        {
            get;
            set;
        }
        public decimal TankSlope
        {
            get;
            set;
        }
        public string LAST_MOD_DT
        {
            get;
            set;
        }
        public string TankActive
        {
            get;
            set;
        }
        public string CompoundGenNo
        {
            get;
            set;
        }
        public string HasStock 
        {
            get;
            set;
        }
        [XmlElement("TankDetails")]
        public List<TankDetails> TankDetails
        {
            get;
            set;
        }
        
    }
   
    [Serializable]   
    public class TankDetails
    {
        [XmlElement("TNP_PROCESS")]
        public int TNP_PROCESS { get; set; }
    }

    public class TankType
    {
        public int SBU { get; set; }
        public int UserPk { get; set; }
       
        public int TankTypePk
        {
            get;
            set;
        }
        public string TankTypeName
        {
            get;
            set;
        }
    }
}
