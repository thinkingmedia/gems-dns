using System.Collections.Generic;

namespace DnsDiscovery.Parser
{
    public interface iToken
    {
        IEnumerable<string> GetValues();
    }
}