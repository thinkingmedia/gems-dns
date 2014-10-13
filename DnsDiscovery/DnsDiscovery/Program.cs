using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using DnsDiscovery.Parser;
using DnsDiscovery.Properties;
using GemsCLI;
using GemsCLI.Descriptions;
using GemsCLI.Helper;
using GemsCLI.Output;

namespace DnsDiscovery
{
    internal static class Program
    {
        private const int _MAX_LENGTH = 253;

        /// <summary>
        /// Returns TRUE if any sub-domain part is to long.
        /// </summary>
        private static bool AnyLabelTooLong(string pDomain, int pMax)
        {
            return pDomain.Split(new[] {'.'}).Any(pLabel=>pLabel.Length > pMax);
        }

        /// <summary>
        /// Entry
        /// </summary>
        /// <param name="pArgs">The arguments from the command line.</param>
        private static void Main(string[] pArgs)
        {
            ConsoleFactory consoleFactory = new ConsoleFactory();
            iOutputStream outS = consoleFactory.Create();

            WriteGreeting(outS);

            CliOptions options = CliOptions.WindowsStyle;

            List<Description> descs = DescriptionFactory.Create(
                options, new HelpResource(Help.ResourceManager),
                "[/domains:string] [/limit:int] [/count] [/sort:string] [/desc] [/max:int] [/min:int] pattern"
                );

            if (pArgs.Length == 0)
            {
                OutputHelp outputHelp = new OutputHelp(options, consoleFactory.Create());
                outputHelp.Show(descs);
                return;
            }

            Request req = RequestFactory.Create(options, pArgs, descs, consoleFactory);
            if (!req.Valid)
            {
                return;
            }

            string pattern = req.Get<string>("pattern");
            outS.Standard(pattern);
            outS.Standard("");

            int max = req.Contains("max") ? Math.Min(req.Get<int>("max"), _MAX_LENGTH) :_MAX_LENGTH;
            int min = req.Contains("min") ? Math.Max(req.Get<int>("min"), 1) : 1;

            IEnumerable<string> domains = (from topLevel in getTopLevel(req)
                                           from domain in getPattern(req)
                                           where !domain.StartsWith("-")
                                                 && !domain.EndsWith("-")
                                                 && domain.Length <= max
                                                 && domain.Length >= min
                                           let str = string.Format("{0}.{1}", domain, topLevel)
                                           where str.Length <= _MAX_LENGTH
                                                 && !AnyLabelTooLong(domain, 63)
                                           select str).Distinct();

            if (req.Contains("count"))
            {
                outS.Standard(domains.Count().ToString(CultureInfo.InvariantCulture));
                return;
            }

            if (req.Contains("sort"))
            {
                string sort = req.Get<string>("sort").ToLower();
                domains = sort == "width"
                    ? domains.OrderBy(pDomain=>pDomain.Length)
                    : domains.OrderBy(pDomain=>pDomain);

                if (req.Contains("desc"))
                {
                    domains = domains.Reverse();
                }
            }

            foreach (string domain in setLimit(req, domains))
            {
                outS.Standard(domain);
            }
        }

        /// <summary>
        /// Displays program greeting.
        /// </summary>
        /// <param name="pOutS"></param>
        private static void WriteGreeting(iOutputStream pOutS)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            pOutS.Standard(string.Format(Resource.Greeting_Version, version));
            pOutS.Standard(Resource.Greeting_Company);
            pOutS.Standard(Resource.Greeting_Contact);
            pOutS.Standard("");
        }

        /// <summary>
        /// Generates a list of top-level registry domains.
        /// </summary>
        private static IEnumerable<string> getTopLevel(Request pReq)
        {
            string domains = pReq.Contains("domains")
                ? pReq.Get<string>("domains")
                : "com";
            return domains.Split(new[] {','}).Distinct();
        }

        /// <summary>
        /// Generates a list of domains from an expression.
        /// </summary>
        private static IEnumerable<string> getPattern(Request pReq)
        {
            string pattern = pReq.Get<string>("pattern");
            Engine eng = new Engine(pattern.Replace(" ", ""));
            return eng.Parse();
        }

        /// <summary>
        /// Imposes a limit of the length of a collection.
        /// </summary>
        private static IEnumerable<string> setLimit(Request pReq, IEnumerable<string> pCollection)
        {
            int limit = pReq.Contains("limit")
                ? pReq.Get<int>("limit")
                : 0;
            return limit != 0
                ? pCollection.Take(limit)
                : pCollection;
        }
    }
}