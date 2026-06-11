using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Administration.Configurations
{

    [Serializable]
    [XmlRoot("Root")]
    public class InboxUserMapingBO
    {
        [XmlElement("Details")]
        public List<InboxUserMapingDetailsBO> Details { get; set; }
    }
    [Serializable]
    public class InboxUserMapingDetailsBO
    {
        [XmlElement("gumUser")]
        public int gumUser { get; set; }
        [XmlElement("gumGroup")]
        public int gumGroup { get; set; }
        [XmlElement("gumHasInbox")]
        public int gumHasInbox { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class UserLocationMappingBO
    {
        [XmlElement("WUL_USER")]
        public int UserPK { get; set; }
        [XmlElement("Detail")]
        public List<UserLocationBO> LocationDtailList { get; set; }
    }
    [Serializable]
    public class UserLocationBO
    {
        [XmlElement("WUL_PK")]
        public int TransactionPK { get; set; }
        [XmlElement("WUL_LOCATION")]
        public int LocationPK { get; set; }
    }
    [Serializable]
    [XmlRoot("Root")]
    public class UserRoleMappingBO
    {
        [XmlElement("usrPK")]
        public int usrPK { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int bizUnitPK { get; set; }
        [XmlElement("USER_COUNT_TEXT")]
        public string userCountText { get; set; }

        [XmlElement("USER_PK_SRC")]
        public int USER_PK_SRC { get; set; }
        [XmlElement("COPY_TYPE")]
        public int COPY_TYPE { get; set; }
        [XmlElement("MODIFIED_BY")]
        public int MODIFIED_BY { get; set; }
        [XmlElement("UserGroupDtl")]
        public List<UserGroupDtailList> GroupDtailList { get; set; }
    }
    [Serializable]
    public class UserGroupDtailList
    {
        [XmlElement("gumGroup")]
        public int gumGroup { get; set; }
    }

    public class UserManagementBO
    {
        public int PK { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Employee { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string Theme { get; set; }
        public int ModBy { get; set; }
        public DateTime ModOn { get; set; }
        public int HomePage { get; set; }
        public DateTime LastLoginSucc { get; set; }
        public DateTime LastLoginFail { get; set; }
        public int UserType { get; set; }
        public string Sid { get; set; }
        public string Signature { get; set; }
        public string DefaultDepartment { get; set; }
        public int IsAlertSound { get; set; }
        public int IsPublicUser { get; set; }
        public int IsSysUser { get; set; }
        public int usrDefInbox { get; set; }
        public string usrCulture { get; set; }
        public string usrCultureSec { get; set; }
        public string usrPhone { get; set; }
    }

    public class UsersBO
    {
        public int usrPK { get; set; }
        public string usrName { get; set; }
        public string usrEmployeeText { get; set; }
    }

    [Serializable]
    [XmlRoot("root")]
    public class UserMISReportBO
    {
        [XmlElement("Group")]
        public List<UserMISReportGroups> ReportGroupList { get; set; }
    }

    [Serializable]
    public class UserMISReportGroups
    {
        [XmlElement("RPT_GROUP")]
        public int RPT_GROUP { get; set; }

        [XmlElement("RPT_GROUP_TEXT")]
        public string RPT_GROUP_TEXT { get; set; }

        [XmlElement("Report")]
        public List<UserGroupReports> ReportList { get; set; }
    }

    [Serializable]
    public class UserGroupReports
    {
        [XmlElement("RPT_PK")]
        public int RPT_PK { get; set; }

        [XmlElement("RPT_TEXT")]
        public string RPT_TEXT { get; set; }

        [XmlElement("RPT_USER_FLAG")]
        public int RPT_USER_FLAG { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class UserMISReportMappingBO
    {
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("RPT_USER")]
        public int RPT_USER { get; set; }
        [XmlElement("RptDetail")]
        public List<UserReportsList> RptDetail { get; set; }
    }

    [Serializable]
    public class UserReportsList
    {
        [XmlElement("RPT_PK")]
        public int RPT_PK { get; set; }
    }

}
