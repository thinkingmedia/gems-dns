using System.Collections.Generic;
using System.Linq;

namespace DnsDiscovery.Parser
{
    public class Generator
    {
        /// <summary>
        /// The tokens
        /// </summary>
        private readonly iToken[] _tokens;

        /// <summary>
        /// Joins two collections together.
        /// </summary>
        private static IEnumerable<string> Join(IEnumerable<string> pLeft, string[] pRight)
        {
            return from left in pLeft
                   from right in pRight
                   select left + right;
        }

        /// <summary>
        /// Recursively assembles the pattern from the right to left.
        /// </summary>
        private IEnumerable<string> Walk(int pIndex = 0)
        {
            if (pIndex == _tokens.Length - 1)
            {
                return _tokens[pIndex].GetValues();
            }

            IEnumerable<string> left = _tokens[pIndex].GetValues();
            IEnumerable<string> right = Walk(pIndex + 1);
            return Join(left, right.ToArray());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Generator(IEnumerable<iToken> pTokens)
        {
            _tokens = pTokens.ToArray();
        }

        /// <summary>
        /// Returns an IEnumerable for all the possible strings compiled from the pattern.
        /// </summary>
        public IEnumerable<string> Generate()
        {
            return _tokens.Length == 0
                ? Enumerable.Empty<string>()
                : Walk();
        }
    }
}