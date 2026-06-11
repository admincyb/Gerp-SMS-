using System.Web.Compilation;

namespace Gti.Infrastructure.ResourceManagement
{
    public class GtiResourceProviderFactory : ResourceProviderFactory
    {
        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            return new GtiGlobalResourceProvider(classKey);
        }

        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            return new GtiLocalResourceProvider(virtualPath);
        }
    }
}
