using System.Web.Compilation;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Resources;

namespace Gti.Infrastructure.ResourceManagement
{
    public class GtiGlobalResourceProvider : IResourceProvider
    {
        private string _className;
        private IDictionary _resourceCache;
        private static object CultureNeutralKey = new object();
        private string clientName = string.Empty;

        public GtiGlobalResourceProvider(string classKey)
        {
            _className = classKey;
        }

        private IDictionary GetResourceCache(string cultureName)
        {
            object cultureKey;

            if (cultureName != null)
            {
                cultureKey = cultureName;
            }
            else
            {
                cultureKey = CultureNeutralKey;
            }

            if (_resourceCache == null)
            {
                _resourceCache = new ListDictionary();
            }


            IDictionary resourceDict = _resourceCache[cultureKey] as IDictionary;

            if (resourceDict == null)
            {
                resourceDict = GtiGlobalResourceHelper.GetResources(_className, cultureName, false, null);
                _resourceCache[cultureKey] = resourceDict;
            }
            return resourceDict;
        }

        public object GetObject(string resourceKey, System.Globalization.CultureInfo culture)
        {
            string cultureName = null;
            resourceKey = resourceKey.ToLower();
            if (culture != null && !string.IsNullOrEmpty(culture.Name))
            {
                cultureName = culture.Name;
            }
            else
            {
                cultureName = CultureInfo.CurrentUICulture.Name;
            }

            object value = GetResourceCache(cultureName)[resourceKey];

            if (value == null)
            {
                value = GetResourceCache(null)[resourceKey];
            }

            return value;
        }

        public IResourceReader ResourceReader
        {
            get { return new GtiGlobalResourceReader(GetResourceCache(null)); }
        }
    }
}
