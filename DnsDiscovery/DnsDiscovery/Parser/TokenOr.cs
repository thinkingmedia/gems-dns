using System.Collections.Generic;
using System.Linq;

namespace DnsDiscovery.Parser
{
    /// <summary>
    /// NOOP token that marks an OR operator in a group.
    /// </summary>
    public class TokenOr : iToken
    {
        public IEnumerable<string> GetValues()
        {
            return Enumerable.Empty<string>();
        }
    }
}