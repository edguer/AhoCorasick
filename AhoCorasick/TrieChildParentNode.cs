using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhoCorasick
{
    /// <summary>
    /// Keeps track of a node and its parent.
    /// </summary>
    public class TrieChildParentNode
    {
        /// <summary>
        /// Standard constructor.
        /// </summary>
        /// <param name="parent">Parent node.</param>
        /// <param name="node">Child node.</param>
        public TrieChildParentNode(TrieNode parent, TrieNode node)
        {
            this.Parent = parent;
            this.Node = node;
        }
        /// <summary>
        /// Gets the parent node.
        /// </summary>
        public TrieNode Parent { get; private set; }
        /// <summary>
        /// Gets the child node.
        /// </summary>
        public TrieNode Node { get; private set; }
    }
}
