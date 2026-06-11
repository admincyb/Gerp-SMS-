using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.DirectoryServices;
using System.Management;
using System.Text;
namespace BusinessObject.Utilities
{
    public class ActiveDirectoryManager
    {
        /// <summary>
        /// Domain Name
        /// </summary>
        private string DomainName
        {
            get
            {
                return ConfigurationManager.AppSettings["ACTIVEDIRECTORY"].ToString();
            }
        }
        /// <summary>
        /// Get all Domain Users
        /// </summary>
        /// <returns>
        /// List of DomainUsers
        /// </returns>
        public List<DomainUsers> GetADUsers()
        {
            List<DomainUsers> domainUsers;
            System.Management.ManagementScope msc;
            string strQuery;
            SelectQuery q;
            ManagementObjectSearcher query;
            ManagementObjectCollection queryCollection;
            domainUsers = null;
            try
            {
                msc = new ManagementScope("root\\cimv2");

                strQuery = "SELECT * FROM Win32_UserAccount WHERE Domain=\"" + this.DomainName + "\"";
                q = new SelectQuery(strQuery);
                query = new ManagementObjectSearcher(msc, q);
                queryCollection = query.Get();
                if (queryCollection.Count > 0)
                {
                    domainUsers = new List<DomainUsers>();
                    foreach (ManagementObject mo in queryCollection)
                    {
                        domainUsers.Add(
                            new DomainUsers()
                            {
                                Name = mo["Name"].ToString(),
                                SID = mo["SID"].ToString()
                            });
                    }
                }
                else
                    domainUsers = null;
            }
            catch
            {

            }
            return domainUsers;
        }
        /// <summary>
        /// Get all Domain Users
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// List of DomainUsers
        /// </returns>
        public List<DomainUsers> GetADUsers(string filter)
        {
            List<DomainUsers> domainUsers;
            System.Management.ManagementScope msc;
            string strQuery;
            SelectQuery q;
            ManagementObjectSearcher query;
            ManagementObjectCollection queryCollection;
            domainUsers = null;
            try
            {
                msc = new ManagementScope("root\\cimv2");

                strQuery = filter.Trim() == string.Empty ? "SELECT * FROM Win32_UserAccount WHERE Domain=\"" + this.DomainName + "\"" :
                    "SELECT * FROM Win32_UserAccount WHERE Domain=\"" + this.DomainName + "\" AND Name LIKE \"" + filter + "%\"";
                q = new SelectQuery(strQuery);
                query = new ManagementObjectSearcher(msc, q);
                queryCollection = query.Get();
                if (queryCollection.Count > 0)
                {
                    domainUsers = new List<DomainUsers>();
                    foreach (ManagementObject mo in queryCollection)
                    {
                        domainUsers.Add(
                            new DomainUsers()
                            {
                                Name = mo["Name"].ToString(),
                                SID = mo["SID"].ToString()
                            });
                    }
                }
                else
                    domainUsers = null;
            }
            catch
            {

            }
            return domainUsers;
        }
        /// <summary>
        /// Authenticate Domain User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>
        /// SID
        /// </returns>
        public string AuthenticateADUSer(string userName, string password)
        {
            string sid;
            string schPath;
            sid = string.Empty;
            try
            {
                DirectoryEntry Entry = new DirectoryEntry("LDAP://" + this.DomainName, userName, password);
                DirectorySearcher Search = new DirectorySearcher(Entry);
                SearchResult results;
                results = Search.FindOne();
                schPath = String.Format("Win32_UserAccount.domain='" + this.DomainName + "',name='" + userName + "'");
                using (ManagementObject mo = new ManagementObject(schPath))
                {
                    mo.Get();
                    sid = mo["SID"].ToString();
                }
                return sid;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Get Domain User by SID
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public DomainUsers GetADUserBySID(string sid)
        {
            DomainUsers domainUser;
            System.Management.ManagementScope msc;
            string strQuery;
            SelectQuery q;
            ManagementObjectSearcher query;
            ManagementObjectCollection queryCollection;
            domainUser = null;
            try
            {
                msc = new ManagementScope("root\\cimv2");
                strQuery = "SELECT * FROM Win32_UserAccount WHERE Domain=\"" + this.DomainName + "\" AND SID= \"" + sid + "\"";
                q = new SelectQuery(strQuery);
                query = new ManagementObjectSearcher(msc, q);
                queryCollection = query.Get();
                if (queryCollection.Count > 0)
                {
                    foreach (ManagementObject mo in queryCollection)
                    {
                        domainUser = new DomainUsers()
                        {
                            Name = mo["Name"].ToString(),
                            SID = mo["SID"].ToString()
                        };
                    }
                }
                else
                    domainUser = null;
            }
            catch
            {

            }
            return domainUser;
        }
    }
    /// <summary>
    /// Domain Users Class
    /// </summary>
    public class DomainUsers
    {
        /// <summary>
        /// Domain User Name
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Domain User SID
        /// </summary>
        public string SID
        {
            get;
            set;
        }
    }
}
