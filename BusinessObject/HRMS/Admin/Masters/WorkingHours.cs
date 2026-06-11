using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
    [Serializable]
    public class WorkingHours
    {
        [XmlElement("ESH_PK")]
        public int Pk { get; set; }

        [XmlElement("ESH_EMP_TYPE")]
        public int EmployeeType { get; set; }

        [XmlElement("ESH_WEEK_DAY")]
        public int WeekDay { get; set; }

        //public string WeekDayTest { get { return Enum.GetName(typeof(WeekDys), this.WeekDay); } }
        public string WeekDayTest { get; set; } 

        [XmlElement("ESH_HOURS")]
        public double Hours { get; set; }

        [XmlElement("ESH_ACTIVE")]
        public int Active { get; set; }
    }

    enum WeekDys
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7
    }
}
