using System.Collections.Generic;
using System.Linq;

namespace DnsDiscovery.Parser
{
    public class WildToken : Token
    {
        /// <summary>
        /// All allowed characters.
        /// </summary>
        private static readonly string[] _all = TokenAlpha.Alpha.Concat(TokenDigit.Nums.Concat(new[] {"-"})).ToArray();

        /// <summary>
        /// All allowed
        /// </summary>
        public override IEnumerable<string> GetValues()
        {
            return _all;
        }
    }
}