using System.Resources;
using System.Collections;

namespace Gti.Infrastructure.ResourceManagement
{
    public class GtiGlobalResourceReader : IResourceReader
    {
        private IDictionary _resources;
        public GtiGlobalResourceReader(IDictionary resources)
        {
            _resources = resources;
        }

        public void Close()
        {
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        public void Dispose()
        {
        }
    }
}
