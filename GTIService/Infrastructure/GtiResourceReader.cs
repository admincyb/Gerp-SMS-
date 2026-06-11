using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Resources;

namespace GTIService.Infrastructure
{
    public class GtiResourceReader : IResourceReader 
    {
        private IDictionary _resources;
        public GtiResourceReader(IDictionary resources)
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
