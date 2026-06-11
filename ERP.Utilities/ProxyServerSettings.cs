using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities
{
    public struct ProxyServerSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
