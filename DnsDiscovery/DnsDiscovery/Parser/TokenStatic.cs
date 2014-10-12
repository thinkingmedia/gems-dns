using System.Collections.Generic;
using System.Diagnostics;

namespace DnsDiscovery.Parser
{
    [DebuggerDisplay("Static:{_char}")]
    public class TokenStatic : Token
    {
        /// <summary>
        /// The char
        /// </summary>
        private readonly string _char;

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenStatic(string pChar)
        {
            _char = pChar;
        }

        /// <summary>
        /// Creates a list of one
        /// </summary>
        public override IEnumerable<string> GetValues()
        {
            return new[] {_char};
        }
    }
}