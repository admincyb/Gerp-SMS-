using System.Resources;
using System.Collections;

namespace Gti.Infrastructure.ResourceManagement
{
    public class GtiLocalResourceReader : IResourceReader
    {
        private IDictionary _resources;
        public GtiLocalResourceReader(IDictionary resources)
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
