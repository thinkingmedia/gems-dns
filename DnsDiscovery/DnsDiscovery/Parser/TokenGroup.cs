using System.Collections.Generic;
using System.Linq;

namespace DnsDiscovery.Parser
{
    /// <summary>
    /// Creates a collection of tokens that are grouped by the | separator.
    /// </summary>
    public class TokenGroup : iToken
    {
        /// <summary>
        /// String generators
        /// </summary>
        private readonly IEnumerable<Generator> _generators;

        /// <summary>
        /// Labels token as to which group it belongs to.
        /// </summary>
        private static IEnumerable<KeyValuePair<iToken, int>> CountGroups(IEnumerable<iToken> pTokens)
        {
            int count = 0;
            foreach (iToken token in pTokens)
            {
                if (token.GetType() == typeof (TokenOr))
                {
                    count++;
                    continue;
                }
                yield return new KeyValuePair<iToken, int>(token, count);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenGroup(string pPattern)
        {
            Compiler c = new Compiler();
            IEnumerable<iToken> tokens = Compiler.Compile(pPattern);

            _generators = CountGroups(tokens)
                .GroupBy(pPair=>pPair.Value)
                .Select(pGroup=>new Generator(pGroup.Select(pItem=>pItem.Key).ToList()));
        }

        /// <summary>
        /// Generates all the strings.
        /// </summary>
        public IEnumerable<string> GetValues()
        {
            return from g in _generators
                   from s in g.Generate()
                   select s;
        }
    }
}