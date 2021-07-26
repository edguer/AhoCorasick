using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhoCorasick
{
    /// <summary>
    /// Represents a match result for a given pattern.
    /// </summary>
    public class MatchResult
    {
        /// <summary>
        /// Pattern string.
        /// </summary>
        public string Pattern { get; private set; }

        /// <summary>
        /// List of start indices for a given pattern.
        /// </summary>
        public IList<int> Indices { get; private set; }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        /// <param name="pattern"></param>
        public MatchResult(string pattern)
        {
            this.Pattern = pattern;
            Indices = new List<int>();
        }
    }
}
