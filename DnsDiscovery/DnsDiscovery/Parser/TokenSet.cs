using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DnsDiscovery.Parser
{
    public class TokenSet : iToken
    {
        /// <summary>
        /// The characters in the set.
        /// </summary>
        private readonly string[] _set;

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenSet(string pSet)
        {
            _set = pSet.Select(pChar=>pChar.ToString(CultureInfo.InvariantCulture)).ToArray();
        }

        /// <summary>
        /// Gets the strings
        /// </summary>
        public IEnumerable<string> GetValues()
        {
            return _set;
        }
    }
}