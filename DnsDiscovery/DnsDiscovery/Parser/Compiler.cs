using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DnsDiscovery.Parser
{
    public class Compiler
    {
        /// <summary>
        /// Reads the contents between two braces. Skip an inner braces.
        /// </summary>
        private static string ReadGroup(TextReader pReader)
        {
            StringBuilder sb = new StringBuilder();
            int o = 1;
            int c;
            while ((c = pReader.Read()) != -1)
            {
                string s = ((char)c).ToString(CultureInfo.InvariantCulture);
                if (s == "(")
                {
                    o++;
                }
                if (s == ")")
                {
                    o--;
                    if (o == 0)
                    {
                        break;
                    }
                }
                sb.Append(s);
            }
            return sb.ToString();
        }

        private static string ReadSet(TextReader pReader)
        {
            StringBuilder sb = new StringBuilder();
            int c;
            while ((c = pReader.Read()) != -1)
            {
                string s = ((char)c).ToString(CultureInfo.InvariantCulture);
                if (s == "]")
                {
                    break;
                }
                sb.Append(s);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Compiles the pattern.
        /// </summary>
        public static IEnumerable<iToken> Compile(string pPattern)
        {
            List<iToken> tokens = new List<iToken>();

            StringReader reader = new StringReader(pPattern);
            int c;
            while ((c = reader.Read()) != -1)
            {
                string s = ((char)c).ToString(CultureInfo.InvariantCulture);
                switch (s)
                {
                    case "#":
                        tokens.Add(new TokenDigit());
                        break;
                    case "@":
                        tokens.Add(new TokenAlpha());
                        break;
                    case "*":
                        tokens.Add(new TokenWild());
                        break;
                    case "|":
                        tokens.Add(new TokenOr());
                        break;
                    case "(":
                        tokens.Add(new TokenGroup(ReadGroup(reader)));
                        break;
                    case "[":
                        tokens.Add(new TokenSet(ReadSet(reader)));
                        break;
                    case "?":
                        iToken prev = tokens.LastOrDefault();
                        if (prev == null)
                        {
                            continue;
                        }
                        tokens.Remove(prev);
                        tokens.Add(new TokenOptional(prev));
                        break;
                    default:
                        if (TokenWild.All.Contains(s)
                            || s == ".")
                        {
                            tokens.Add(new TokenStatic(s));
                        }
                        break;
                }
            }

            return tokens;
        }
    }
}