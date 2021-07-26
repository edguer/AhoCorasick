using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhoCorasick
{
    /// <summary>
    /// Keeps track of a node and its parent. 
    /// Parent/child relationship is only used when calculating the failure link, and it can be inferred during BFS,
    /// so using an intermediate/temporary class for storing the relationship in BFS makes more sense then
    /// having the parent reference in every child.
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
