using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace BusinessObject
{
    [Serializable]
    public class User : System.Security.Principal.IIdentity
    {

        public string Name
        {
            get { return this.UserID; }
        }
        public string UserID
        {
            get;
            set;
        }
        public string EmpCode
        {
            get;
            set;
        }
        public int PKEmployee
        {
            get;
            set;
        }
        public string EmpName
        {
            get;
            set;
        }
        public int PKUser
        {
            get;
            set;
        }
        [Newtonsoft.Json.JsonIgnore]
        public string Roles
        {
            get;
            set;
        }
        public string UserName
        {
            get { return this.UserID; }
        }
        
        public string ThemeName
        {
            get;
            set;
        }
        public string AuthenticationType
        {
            get { return "Forms"; }
        }
        public bool IsAuthenticated
        {
            get { return true; }
        }

        public int SBUID
        {
            get;
            set;
        }
        public int Active
        {
            get;
            set;
        }
      
        public int ActiveDepID
        {
            get;
            set;
        }
        public List<UserDept> UserDept
        {
            get;
            set;
        }
        public int LoginType
        {
            get;
            set;
        }
        public User()
        {
            UserDept = new List<UserDept>();
        }
        // Added on 26-09-2011
        public string UserCulture
        { get; set; }
        public string CurrentSBU
        { get; set; }
        public string CurrentDept
        { get; set; }
        public int CurrentDeptPK
        { get; set; }
        public int IsAlertSound
        { get; set; }
        public bool IsPublicUser
        { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public DateTime LastLoginSuccess
        { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public DateTime LastLoginFailure
        { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public List<UserGroup> GroupList
        { get; set; }
        public float TimeZone
        { get; set; }
        public string Theme
        { get; set; }
        public int BaseCurrency
        { get; set; }
        public int CurrentSBUPK
        { get; set; }
        public bool IsFirstLogin
        { get; set; }
        public int IsSysUser
        { get; set; }
        public DateTime usrPwdModOn
        { get; set; }
    }
    [Serializable]
    public class ERPPrincipal : System.Security.Principal.IPrincipal
    {
        private User identity;
        public string[] Roles
        {
            get;
            set;
        }
        public bool IsInRole(string role)
        {
            return Roles.Contains<string>(role);
        }
        public string GetUserPK()
        {
            return this.identity.PKUser.ToString();
        }
        public string GetUserTheme()
        {
            return this.identity.ThemeName;
        }
        System.Security.Principal.IIdentity System.Security.Principal.IPrincipal.Identity
        {
            get { return this.identity; }
        }
        public ERPPrincipal(User user)
        {
            this.identity = user;
            if (user.Roles!=null && user.Roles.Length>0)
            {
                this.Roles = user.Roles.Split(',');
            }
        }
    }

    [Serializable]
    public class UserDept
    {
        public string DeptName
        {
            get;
            set;
        }
        public string DeptPK
        {
            get;
            set;
        }
        public int BaseDeptPK
        {
            get;
            set;
        }
    }

    // add 0n 26092011
     [Serializable]
    public class UserGroup
    {
        public int GroupPK { get; set; }
        public string GroupName { get; set; }
    }
    
    [Serializable]
    public class UserRightsBO
    {
        public int UserPK
        { get; set; }
        public List<UserRightBO> Rights
        { get; set; }
        public UserRightsBO()
        {
            this.Rights = new List<UserRightBO>();
        }
    }

    [Serializable]
    public class UserRightBO
    {
        public int PagePK
        { get; set; }
        public string PageURL
        { get; set; }
        public string SectionName
        { get; set; }
        public string ActionName
        { get; set; }
        public bool HasSectionRight
        { get; set; }
        public bool HasActionRight
        { get; set; }
        public bool IsTab
        { get; set; }
        public bool UserDeptRight
        { get; set; }
        public string PBIReportID
        { get; set; }
    }

     [Serializable]
    public class SBU
    {
        public string CurrentSBU
        { get; set; }
        public int CurrentSBUPK
        { get; set; }
    }

     [Serializable]
    public class Department
    {
        public string CurrentSBU
        { get; set; }
        public int CurrentSBUPK
        { get; set; }
        public string CurrentDept
        { get; set; }
        public int CurrentDeptPK
        { get; set; }
        public int BaseCurrency
        { get; set; }
        public int BaseCountry
        { get; set; }
         
    }

     [Serializable]
     public class RightGroupNames
     {
         public int GroupID { get; set; }
         public string GroupName { get; set; }
         public int SectionID { get; set; }
         public List<RightLinks> RightLinks { get; set; }
     }

     [Serializable]
     public class RightLinks
     {
         public string LinkUrl { get; set; }
         public string LinkText { get; set; }
         public int GroupID { get; set; }
         public string Description { get; set; }
         public string PostUrl { get; set; }
     }

     [Serializable]
     public class MenuBO
     {
         public int SectionID { get; set; }
         public string SectionHead { get; set; }
         public string ImageUrl { get; set; }
         public string Icon1Link { get; set; }
         public string Icon2Link { get; set; }
         public string Icon3Link { get; set; }
         public string Icon4Link { get; set; }
         public List<RightGroupNames> RightGroupNames { get; set; }
         public List<IconList> IconList { get; set; }
         public List<Roles> roles { get; set; }
     }

     [Serializable]
     public class IconList
     {
         public int SectionID { get; set; }
         public string IconImage { get; set; }
         public string IconLink { get; set; }
     }

     [Serializable]
     public class Roles
     {
         public string URCrole { get; set; }
         public string ROLName { get; set; }
         public int ROLCode { get; set; }
         public string ROLText { get; set; }
     }

}
