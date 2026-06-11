using System;
using System.Web;
using System.Web.Security;

using BusinessLogic;
using BusinessObject;
using System.Web.UI.WebControls;

namespace LatexERPV2
{
    public partial class AdminSignIn : System.Web.UI.UserControl
    {
        private static NLog.Logger logger = NLog.LogManager.GetLogger("Admin Login");

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Performs the Forms Authentication.
        /// Stores the Login object in Session with the SessionKey = UserID
        /// Redirects the User based on Home page preference.
        /// </summary>
        /// <param name="uLogin"></param>
        private void Authenticate(BusinessObject.User uLogin)
        {
            Session[uLogin.PKUser.ToString()] = uLogin;
            Context.User = new ERPPrincipal(uLogin);
            
            FormsAuthenticationTicket AuthTicket = new FormsAuthenticationTicket(1, uLogin.UserID,DateTime.Now, DateTime.Now.AddMinutes(15), this.LgnAdmin.RememberMeSet, Newtonsoft.Json.JsonConvert.SerializeObject(uLogin));
            string EncTicket = FormsAuthentication.Encrypt(AuthTicket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, EncTicket));
            Response.Redirect("Default.aspx");    
        }

        /// <summary>
        /// Event for Authentication
        /// </summary>
        /// <param name="uLogin"></param>
        protected void LgnAdmin_Authenticate(object sender, System.Web.UI.WebControls.AuthenticateEventArgs e)
        {
            try
            {
                string userName = this.LgnAdmin.UserName;
                string password = this.LgnAdmin.Password;
                BusinessLogic.Login login = new BusinessLogic.Login();
                var uLogin = login.AdminAuthenticate(userName, password);
                if (uLogin != null)
                {
                    this.Authenticate(uLogin);
                }
                else
                {
                    this.LgnAdmin.FailureText = "Authentication Failed !!";

                }
            }
            catch (Exception ex)
            {
                logger.Log(NLog.LogLevel.Fatal, "Error Occured " + ex.Message);
            }
        }
    }
}