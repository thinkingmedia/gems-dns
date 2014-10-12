using System.Collections.Generic;

namespace DnsDiscovery.Parser
{
    public abstract class Token
    {
        public abstract IEnumerable<string> GetValues();
    }
}