using System.Collections.Generic;
using System.Linq;

namespace DnsDiscovery.Parser
{
    public class TokenWild : iToken
    {
        /// <summary>
        /// All allowed characters.
        /// </summary>
        public static readonly string[] All = TokenAlpha.Alpha.Concat(TokenDigit.Nums.Concat(new[] {"-"})).ToArray();

        /// <summary>
        /// All allowed
        /// </summary>
        public IEnumerable<string> GetValues()
        {
            return All;
        }
    }
}