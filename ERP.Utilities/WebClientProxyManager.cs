using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;


namespace ERP.Utilities
{
    public class WebClientProxyManager : WebClient
    {
        private WebClient _webClient;

        #region Constructors
        /// <summary>
        /// Use Parameterless constructor for Ignoring proxy settings
        /// </summary>
        public WebClientProxyManager()
        {
            Initialize();
        }

        public WebClientProxyManager(bool useConfiguration)
        {
            string proxyServer = ConfigurationManager.AppSettings["ProxyServer"];
            string proxyServerPort = ConfigurationManager.AppSettings["ProxyServerPort"];
            string proxyUserName = ConfigurationManager.AppSettings["ProxyUserName"];
            string proxyPassword = ConfigurationManager.AppSettings["ProxyUserPassword"];

            if (useConfiguration &&
                (proxyServer.IsNullOrEmptyOrWhitespace()
                || proxyServerPort.IsNullOrEmptyOrWhitespace()
                || proxyUserName.IsNullOrEmptyOrWhitespace()
                || proxyPassword.IsNullOrEmptyOrWhitespace())
                ) throw new InvalidOperationException("Invalid Proxy Settings");


            int port = 0;
            if (!Int32.TryParse(proxyServerPort, out port))
                throw new ArgumentException("Port Must be in valid Integer Range", "port");

            if (useConfiguration)
                BuildProxyServer(
                    new ProxyServerSettings
                    {
                        Server = proxyServer,
                        Port = port,
                        UserName = proxyUserName,
                        Password = proxyPassword
                    }
                );
            Initialize();
        }

        public WebClientProxyManager(string proxyServer, int port, string proxyUserName, string proxyPassword)
            : this(new ProxyServerSettings
            {
                Server = proxyServer,
                Port = port,
                UserName = proxyUserName,
                Password = proxyPassword
            }) { }

        public WebClientProxyManager(string proxyServer, string port, string proxyUserName, string proxyPassword)
        {
            int tempPort = 0;
            if (!Int32.TryParse(port, out tempPort))
                throw new ArgumentException("Port Must be in valid Integer Range", "port");

            BuildProxyServer(
                    new ProxyServerSettings
                    {
                        Server = proxyServer,
                        Port = tempPort,
                        UserName = proxyUserName,
                        Password = proxyPassword
                    }
                );

            Initialize();
        }

        public WebClientProxyManager(ProxyServerSettings proxySettings)
        {
            BuildProxyServer(proxySettings);
            Initialize();
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            _webClient = new WebClient();
        }

        private void BuildProxyServer(ProxyServerSettings proxySettings)
        {
            WebProxy proxy = null;
            string proxyServerUrl = string.Format("http://{0}:{1}", proxySettings.Server, proxySettings.Port);

            ICredentials proxyCredential;
            proxyCredential = new NetworkCredential(proxySettings.UserName, proxySettings.Password);
            proxy = new WebProxy(proxyServerUrl, true, null, proxyCredential);

            WebRequest.DefaultWebProxy = proxy;
        }

        #endregion
    }
}
