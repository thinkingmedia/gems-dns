using System.Collections.Generic;
using System.Linq;

namespace DnsDiscovery.Parser
{
    public class TokenOptional : iToken
    {
        /// <summary>
        /// The optional token
        /// </summary>
        private readonly iToken _token;

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenOptional(iToken pToken)
        {
            _token = pToken;
        }

        public IEnumerable<string> GetValues()
        {
            IEnumerable<string> enumString = _token.GetValues();
            string[] values = enumString as string[] ?? enumString.ToArray();
            IEnumerable<string> filler = Enumerable.Repeat("", values.Count());
            return values.Concat(filler);
        }
    }
}