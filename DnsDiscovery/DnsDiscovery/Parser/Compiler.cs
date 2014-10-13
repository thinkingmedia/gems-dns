using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

        /// <summary>
        /// Compiles the pattern.
        /// </summary>
        public IEnumerable<iToken> Compile(string pPattern)
        {
            StringReader reader = new StringReader(pPattern);
            int c;
            while ((c = reader.Read()) != -1)
            {
                string s = ((char)c).ToString(CultureInfo.InvariantCulture);
                switch (s)
                {
                    case "#":
                        yield return new TokenDigit();
                        break;
                    case "@":
                        yield return new TokenAlpha();
                        break;
                    case "*":
                        yield return new TokenWild();
                        break;
                    case "|":
                        yield return new TokenOr();
                        break;
                    case "(":
                        string g = ReadGroup(reader);
                        yield return new TokenGroup(g);
                        break;
                    default:
                        yield return new TokenStatic(s);
                        break;
                }
            }
        }
    }
}