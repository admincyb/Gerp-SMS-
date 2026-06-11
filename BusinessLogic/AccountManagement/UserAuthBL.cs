using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using BusinessObject.AccountManagement;
using DataAccess.AccountManagement;
using BusinessObject;
using System.Data;

namespace BusinessLogic.AccountManagement
{
    public class UserAuthBL
    {
        /// <summary>
        /// Method to Validate User Login
        /// </summary>
        /// <param name="objEmp" Type=Object Employee></param>
        /// <returns>Integer</returns>
        public User ValidateUser(string UserID, string Password)
        {
            User authUser;
            AuthDA authentication;

            authentication = new AuthDA();

            authUser = authentication.ValidateUserLogin(UserID, Password);
            return authUser;

        }
        /// <summary>
        /// Get Authentication Types
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetAuthenticationTypes(int status)
        {
            AuthDA userAuthServices;
            userAuthServices = new AuthDA();
            return userAuthServices.GetAuthenticationTypes(status);
        }
        /// <summary>
        /// Returns User Rights based on UserPK, and Page
        /// </summary>
        /// <param name="UserPK"></param>
        /// <param name="PageURL">Page path from the root folder</param>
        /// <returns></returns>
        public UserRightsBO GetUserRights(int UserPK, string PageURL,int bizUnit,int deptPK=0)
        {
            UserRightsBO userRights;
            AuthDA userAuthServices;
            userAuthServices = new AuthDA();
            userRights = userAuthServices.GetUserRightsInfo(UserPK, PageURL, bizUnit, deptPK);
            return userRights;
        }
        /// <summary>
        /// Funtion will set the httpContext.Current.User.Identity with the supplied property and value
        /// </summary>
        /// <param name="Property"></param>
        /// <param name="value">Supply BO.AccountsMgmt.UserPropertyEnum.CurrentCostCenter object for "CurrentCostCenter" property</param>
        /// <returns></returns>
        public bool SetUserProperty(UserPropertyEnum Property, object value,object value2=null)
        {
            try
            {
                SBU sbu=null;
                Department dept=null;
                Department deptCurrency;
                FormsAuthenticationTicket AuthTicket = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                User ticketUser = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(AuthTicket.UserData);
                ticketUser.GroupList = BusinessLogic.Login.GetUserGroup(ticketUser.PKUser);
                switch (Property)
                {
                    case UserPropertyEnum.CultureInfo:
                        ticketUser.UserCulture = Convert.ToString(value);
                        break;
                    case UserPropertyEnum.CurrentSBU:
                        sbu = (SBU)value;
                        ticketUser.CurrentSBU = sbu.CurrentSBU;
                        ticketUser.SBUID = sbu.CurrentSBUPK;
                        break;
                    case UserPropertyEnum.CurrentDept:
                        dept = (Department)value;
                        deptCurrency = BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetDeptDetailsByID(dept.CurrentDeptPK);
                        ticketUser.CurrentSBU = dept.CurrentSBU;
                        ticketUser.SBUID = dept.CurrentSBUPK;
                        ticketUser.CurrentDept = dept.CurrentDept;
                        ticketUser.CurrentDeptPK = dept.CurrentDeptPK;
                        if (deptCurrency != null)
                            ticketUser.BaseCurrency = deptCurrency.BaseCurrency;
                        else
                            ticketUser.BaseCurrency = dept.BaseCurrency;
                        break;
                    case UserPropertyEnum.SBUDept:
                        sbu = (SBU)value;
                        ticketUser.CurrentSBU = sbu.CurrentSBU;
                        ticketUser.SBUID = sbu.CurrentSBUPK;
                        dept = (Department)value2;
                        deptCurrency = BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetDeptDetailsByID(dept.CurrentDeptPK);
                        ticketUser.CurrentSBU = dept.CurrentSBU;
                        ticketUser.SBUID = dept.CurrentSBUPK;
                        ticketUser.CurrentDept = dept.CurrentDept;
                        ticketUser.CurrentDeptPK = dept.CurrentDeptPK;
                        if (deptCurrency != null)
                            ticketUser.BaseCurrency = deptCurrency.BaseCurrency;
                        else
                            ticketUser.BaseCurrency = dept.BaseCurrency;
                        break;
                    default:
                        break;
                }
                //Serialize the User Object to store it as UserData in Auth Ticket
                string userData = Newtonsoft.Json.JsonConvert.SerializeObject(ticketUser);
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, ticketUser.UserID, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);

                //Encrypt the Auth ticket before adding to cookie
                string encTicket = FormsAuthentication.Encrypt(authTicket);

                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                authCookie.HttpOnly = true;
                //Add the auth cookie to the response
                HttpContext.Current.Response.Cookies.Add(authCookie);
                //Add Rules
                string strRoles = string.Empty;
                foreach (var r in ticketUser.GroupList)
                    strRoles += r.GroupPK + ",";
                strRoles = strRoles.Substring(0, strRoles.Length - 1);
                ticketUser.Roles = strRoles;
                //Set the current Principal User as the new Identity user
                ERPPrincipal principalUser = new ERPPrincipal(ticketUser);
                HttpContext.Current.User = principalUser;
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Logout the current authentication session
        /// </summary>
        public void Logout()
        {
            /* Abandon session object to destroy all session variables */
            string cacheName = HttpContext.Current.User.Identity.Name + "__" + HttpContext.Current.Session.SessionID;
            HttpContext.Current.Cache.Remove(cacheName);


            /* Create new session ticket that expires immediately */
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                    HttpContext.Current.User.Identity.Name,
                    DateTime.Now,
                    DateTime.Now,
                    false,
                    "");

            /* Encrypt the ticket */
            string encrypted_ticket = FormsAuthentication.Encrypt(ticket);

            /* Create cookie */
            HttpCookie cookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                encrypted_ticket);

            /* Add cookie */
            HttpContext.Current.Response.Cookies.Add(cookie);


            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();

            try
            {
                FormsAuthentication.SignOut();
            }
            catch { }

            HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
        }
        /// <summary>
        /// Update Password
        /// </summary>
        /// <param name="UserPK"></param>
        /// <param name="OldPwd"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        static public int ChangePassword(int UserPK, string OldPwd, string NewPwd)
        {
            AuthDA authentication;
            authentication = new AuthDA();
            return authentication.ChangePassword(UserPK, OldPwd, NewPwd);
        }

        /// <summary>
        /// Get Password History
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataTable GetPasswordHistory(string newPassword, int UserPK, int pwdHistoryCount)
        {
            return AuthDA.GetPasswordHistory(newPassword, UserPK, pwdHistoryCount);
        }
    }
}
