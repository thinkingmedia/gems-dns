using System.Collections.Generic;
using System.Diagnostics;

namespace DnsDiscovery.Parser
{
    [DebuggerDisplay("Static:{_char}")]
    public class TokenStatic : iToken
    {
        /// <summary>
        /// The char
        /// </summary>
        private readonly string[] _char;

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenStatic(string pChar)
        {
            _char = new []{pChar};
        }

        /// <summary>
        /// Creates a list of one
        /// </summary>
        public IEnumerable<string> GetValues()
        {
            return _char;
        }
    }
}