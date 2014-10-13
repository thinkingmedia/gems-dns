using System.Collections.Generic;

namespace DnsDiscovery.Parser
{
    public class Engine
    {
        private readonly Generator _gen;

        /// <summary>
        /// Constructor
        /// </summary>
        public Engine(string pPattern)
        {
            Compiler c = new Compiler();
            _gen = new Generator(Compiler.Compile(pPattern));
        }

        /// <summary>
        /// Returns an IEnumerable for all the possible strings compiled from the pattern.
        /// </summary>
        public IEnumerable<string> Parse()
        {
            return _gen.Generate();
        }
    }
}