using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Web.Compilation;
using System.Collections.Specialized;
using System.Globalization;
using System.Resources;

namespace GTIService.Infrastructure
{
    public class GtiResourceProvider: IResourceProvider
    {
        private string _virtualPath;
        private string _className;
        private IDictionary _resourceCache;
        private static object CultureNeutralKey = new object();
        private string clientName = string.Empty;

        public GtiResourceProvider(string virtualPath, string className)
        {
            _virtualPath = virtualPath;
            _className = className;
        }

        private IDictionary GetResourceCache(string cultureName){
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
                resourceDict = GtiResourceHelper.GetResources(_virtualPath,
                              _className, cultureName, false, null);

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

            var resDic = GetResourceCache(cultureName);

            object value = resDic != null ? resDic[resourceKey] : null;

            if (value == null)
            {
                resDic = GetResourceCache(null);
                value = resDic != null ? resDic[resourceKey] : null;
            }

            return value;
        }

        public IResourceReader ResourceReader
        {
            get { return new GtiResourceReader(GetResourceCache(null)); }
        }
    }
}
