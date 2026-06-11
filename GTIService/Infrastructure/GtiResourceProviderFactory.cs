using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;

namespace GTIService.Infrastructure
{
    public class GtiResourceProviderFactory : ResourceProviderFactory
    {
        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            return new GtiResourceProvider(null, classKey);
        }

        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            virtualPath = System.IO.Path.GetFileName(virtualPath);
            return new GtiResourceProvider(virtualPath, null);
        }

    }
}
