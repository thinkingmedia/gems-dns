using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DnsDiscovery.Properties;
using GemsCLI;
using GemsCLI.Descriptions;
using GemsCLI.Help;
using GemsCLI.Output;

namespace DnsDiscovery
{
    internal static class Program
    {
        /// <summary>
        /// Entry
        /// </summary>
        /// <param name="pArgs">The arguments from the command line.</param>
        private static void Main(string[] pArgs)
        {
            WriteGreeting();

            CliOptions options = CliOptions.WindowsStyle;

            List<Description> descs = DescriptionFactory.Create(
                options, new HelpResource(Help.ResourceManager),
                "[/domains:string] [/limit:int] pattern"
                );

            ConsoleFactory consoleFactory = new ConsoleFactory();

            if (pArgs.Length == 0)
            {
                OutputHelp outputHelp = new OutputHelp(options, consoleFactory.Create());
                outputHelp.Show(descs);
                return;
            }

            Request req = RequestFactory.Create(options, pArgs, descs, consoleFactory);

            IEnumerable<string> domains = (from topLevel in getDomains(req)
                                           from domain in getPattern(req)
                                           let str = string.Format("{0}.{1}", domain, topLevel)
                                           select str).Distinct();
            foreach (string domain in setLimit(req, domains))
            {
                Debug.WriteLine(domain);
            }
        }

        /// <summary>
        /// Displays program greeting.
        /// </summary>
        private static void WriteGreeting()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            Console.WriteLine(Resource.Greeting_Version, version);
            Console.WriteLine(Resource.Greeting_Company);
            Console.WriteLine(Resource.Greeting_Contact);
            Console.WriteLine("");
        }

        /// <summary>
        /// Generates a list of top-level registry domains.
        /// </summary>
        private static IEnumerable<string> getDomains(Request pReq)
        {
            string domains = pReq.Contains("domains")
                ? pReq.Get<string>("domains")
                : "com,net";
            return domains.Split(new[] {','}).Distinct();
        }

        /// <summary>
        /// Generates a list of domains from an expression.
        /// </summary>
        private static IEnumerable<string> getPattern(Request pReq)
        {
            string pattern = pReq.Get<string>("pattern");
            return new[] {pattern};
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