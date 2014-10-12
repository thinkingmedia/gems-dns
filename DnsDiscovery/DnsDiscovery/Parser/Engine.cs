using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DnsDiscovery.Parser
{
    public class Engine
    {
        /// <summary>
        /// The regex used to compile the pattern
        /// </summary>
        private readonly Regex _r;

        /// <summary>
        /// The compiled pattern
        /// </summary>
        private readonly List<iToken> _tokens;

        /// <summary>
        /// Constructor
        /// </summary>
        public Engine()
        {
            List<string> rules = new List<string>
                                 {
                                     @"(?<number>#)",
                                     @"(?<alpha>@)",
                                     @"(?<wild>\*)",
                                     @"(?<optional>\?)",
                                     @"(?<static>.)"
                                 };

            _r = new Regex(string.Join("|", rules), RegexOptions.IgnoreCase);
            _tokens = new List<iToken>();
        }

        /// <summary>
        /// Compiles the pattern.
        /// </summary>
        public bool Compile(string pPattern)
        {
            _tokens.Clear();

            MatchCollection matches = _r.Matches(pPattern);
            foreach (Match match in matches)
            {
                iToken t;
                if (match.Groups["number"].Success)
                {
                    t = new TokenDigit();
                }
                else if (match.Groups["alpha"].Success)
                {
                    t = new TokenAlpha();
                }
                else if (match.Groups["wild"].Success)
                {
                    t = new WildToken();
                }
                else if (match.Groups["optional"].Success)
                {
                    iToken prev = _tokens.LastOrDefault();
                    if (prev == null)
                    {
                        continue;
                    }
                    _tokens.Remove(prev);
                    t = new TokenOptional(prev);
                }
                else if (match.Groups["static"].Success)
                {
                    t = new TokenStatic(match.Groups["static"].Value);
                }
                else
                {
                    return false;
                }
                _tokens.Add(t);
            }
            return true;
        }

        private IEnumerable<string> Join(IEnumerable<string> pLeft, string[] pRight)
        {
            return from left in pLeft
                   from right in pRight
                   select left + right;
        }

        private IEnumerable<string> Walk(int pIndex = 0)
        {
            if (pIndex == _tokens.Count - 1)
            {
                return _tokens[pIndex].GetValues();
            }

            IEnumerable<string> left = _tokens[pIndex].GetValues();
            IEnumerable<string> right = Walk(pIndex + 1);
            return Join(left, right.ToArray());
        }

        /// <summary>
        /// Returns an IEnumerable for all the possible strings compiled from the pattern.
        /// </summary>
        public IEnumerable<string> Parse()
        {
            if (_tokens.Count == 0)
            {
                return Enumerable.Empty<string>();
            }

            return Walk();
        }
    }
}