using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DnsDiscovery.Parser
{
    public class TokenAlpha : iToken
    {
        /// <summary>
        /// Alphabet 
        /// </summary>
        public static readonly string[] Alpha = Enumerable.Range('a', 'z' - 'a' + 1)
            .Select(pChar=>((Char)pChar).ToString(CultureInfo.InvariantCulture)).ToArray();

        public IEnumerable<string> GetValues()
        {
            return Alpha;
        }
    }
}