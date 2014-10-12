using System.Collections.Generic;

namespace DnsDiscovery.Parser
{
    public class TokenDigit : iToken
    {
        /// <summary>
        /// Numbers
        /// </summary>
        public static readonly string[] Nums = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};

        /// <summary>
        /// Gets a list of numbers
        /// </summary>
        public IEnumerable<string> GetValues()
        {
            return Nums;
        }
    }
}