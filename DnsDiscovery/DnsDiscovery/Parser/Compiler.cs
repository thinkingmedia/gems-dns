using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DnsDiscovery.Parser
{
    public class Compiler
    {
        /// <summary>
        /// The regex used to compile the pattern
        /// </summary>
        private readonly Regex _r;

        /// <summary>
        /// Constructor
        /// </summary>
        public Compiler()
        {
            List<string> rules = new List<string>
                                 {
                                     @"(?<number>#)",
                                     @"(?<alpha>@)",
                                     @"(?<wild>\*)",
                                     @"(?<optional>\?)",
                                     @"(?<group>\([^\)]*\))",
                                     @"(?<or>\|)",
                                     @"(?<static>.)"
                                 };

            _r = new Regex(string.Join("|", rules), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Compiles the pattern.
        /// </summary>
        public IList<iToken> Compile(string pPattern)
        {
            IList<iToken> tokens = new List<iToken>();

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
                    iToken prev = tokens.LastOrDefault();
                    if (prev == null)
                    {
                        continue;
                    }
                    tokens.Remove(prev);
                    t = new TokenOptional(prev);
                }
                else if (match.Groups["group"].Success)
                {
                    string group = match.Groups["group"].Value;
                    t = new TokenGroup(group.Substring(1, group.Length - 2));
                }
                else if (match.Groups["or"].Success)
                {
                    t = new TokenOr();
                }
                else if (match.Groups["static"].Success)
                {
                    t = new TokenStatic(match.Groups["static"].Value);
                }
                else
                {
                    continue;
                }

                tokens.Add(t);
            }

            return tokens;
        }
    }
}
