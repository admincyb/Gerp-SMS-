using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.AccountManagement
{
   
    ///// <summary>
    ///// Custom Principal for Auth services
    ///// </summary>
    //public class IPrincipal : System.Security.Principal.IPrincipal
    //{
    //    private IdentityUser User;
    //    public System.Security.Principal.IIdentity Identity
    //    {
    //        get { return this.User; }
    //    }
    //    public bool IsInRole(string role)
    //    {
    //        return false;
    //    }
    //    public IPrincipal(IdentityUser user)
    //    {
    //        this.User = user;
    //    }
    //}
    ///// <summary>
    ///// This custom class acts as the IIdentity user for the IPrincipal class.
    ///// Developers can access this object by casting the httpContext.Current.User.Identity object to
    ///// this class type. All properties will be initialized from the default page.
    ///// </summary>
    //public class IdentityUser : System.Security.Principal.IIdentity
    //{
        //public string UserID
        //{ get; set; }
        //public string UserFullName
        //{ get; set; }
        //public int UserPK
        //{ get; set; }
        //public string CurrentSBU
        //{ get; set; }
        //public int CurrentSBUPK
        //{ get; set; }

        //public string CurrentDept
        //{ get; set; }
        //public int CurrentDeptPK
        //{ get; set; }
        //public string AuthenticationType
        //{
        //    get { return "Forms"; }
        //}
        //public bool IsAuthenticated
        //{
        //    get;
        //    set;
        //}
        //public string Name
        //{
        //    get { return this.UserID; }
        //}
        //public float TimeZone
        //{ get; set; }
        //public string UserCulture
        //{ get; set; }
        //public string Theme
        //{ get; set; }
        //public DateTime LastLoginSuccess
        //{ get; set; }
        //public DateTime LastLoginFailure
        //{ get; set; }
        //public List<UserGroup> GroupList
        //{ get; set; }


        // public string UserName
        //{
        //    get;
        //    set;
        //}
        //public string UserID
        //{
        //    get;
        //    set;
        //}
        //public string EmpCode
        //{
        //    get;
        //    set;
        //}
        //public int PKEmployee
        //{
        //    get;
        //    set;
        //}
        //public string EmpName
        //{
        //    get;
        //    set;
        //}
        //public int UserPk
        //{
        //    get;
        //    set;
        //}
        //public string Roles
        //{
        //    get;
        //    set;
        //}
        ////public string ThemeName
        ////{
        ////    get;
        ////    set;
        ////}
        //public string AuthenticationType
        //{
        //    get { return "Forms"; }
        //}
        //public bool IsAuthenticated
        //{
        //    get { return true; }
        //}
        //public string Name
        //{
        //    get { return this.UserName; }
        //}
        //public int SBUID
        //{
        //    get;
        //    set;
        //}
     
      
        //public int ActiveDepID
        //{
        //    get;
        //    set;
        //}
        //public List<Department> UserDept
        //{
        //    get;
        //    set;
        //}
        //public int LoginType
        //{
        //    get;
        //    set;
        //}
       
        //// Added on 26-09-2011
        //public string UserCulture
        //{ get; set; }
        //public string SBUName
        //{ get; set; }
        //public string CurrentDept
        //{ get; set; }
        //public int CurrentDeptPK
        //{ get; set; }
        //public string Theme
        //{ get; set; }
        //public List<UserGroup> GroupList
        //{ get; set; }
    //}

    //public class UserGroup
    //{
    //    public int GroupPK { get; set; }
    //    public string GroupName { get; set; }
    //}


    ///// <summary>
    ///// Used for getting the User Rigts for a User based on Location
    ///// </summary>

    //[Serializable]
    //public class UserRightsBO
    //{
    //    public int UserPK
    //    { get; set; }
    //    public List<UserRightBO> Rights
    //    { get; set; }
    //    public UserRightsBO()
    //    {
    //        this.Rights = new List<UserRightBO>();
    //    }
    //}
    //[Serializable]
    //public class UserRightBO
    //{
    //    public int PagePK
    //    { get; set; }
    //    public string PageURL
    //    { get; set; }
    //    public string SectionName
    //    { get; set; }
    //    public string ActionName
    //    { get; set; }
    //    public bool HasSectionRight
    //    { get; set; }
    //    public bool HasActionRight
    //    { get; set; }
    //}

    //public class SBU
    //{
    //    public string CurrentSBU
    //    { get; set; }
    //    public int CurrentSBUPK
    //    { get; set; }
    //}

    //public class Department
    //{
    //    public string CurrentSBU
    //    { get; set; }
    //    public int CurrentSBUPK
    //    { get; set; }
    //    public string CurrentDept
    //    { get; set; }
    //    public int CurrentDeptPK
    //    { get; set; }
    //}
}
