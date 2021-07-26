using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhoCorasick
{
    /// <summary>
    /// Represents a Trie node optimized for memory consumption.
    /// </summary>
    [ProtoContract()]
    public class TrieNode
    {
        [ProtoMember(1)]
        private TrieNode[] children;

        /// <summary>
        /// Initializes a node representing a character.
        /// </summary>
        /// <param name="character">The character the node will hold.</param>
        public TrieNode(char character) : this()
        {
            this.Character = character;
        }

        /// <summary>
        /// Initializes an empty node, usually used as the root.
        /// </summary>
        public TrieNode()
        {
            // Children array is initialized with 0 items to save memory in case it is not used.
            this.children = new TrieNode[0];
        }

        /// <summary>
        /// Gets the node character.
        /// </summary>
        [ProtoMember(2)]
        public char Character { get; set; }

        /// <summary>
        /// Tells whether the node is a Trie's leaf or not. A leaf is the end of a word, meaning its last character.
        /// </summary>
        public bool IsLeaf => LeafPattern != null;

        /// <summary>
        /// Gets or sets the term/pattern corresponding to the leaf's word.
        /// </summary>
        [ProtoMember(3)]
        public string LeafPattern { get; set; } = null;

        /// <summary>
        /// Gets child nodes.
        /// </summary>
        public TrieNode[] Children => this.children;

        /// <summary>
        /// Gets the suffix/failure link to the longest possible suffix node.
        /// </summary>
        public TrieNode FailureLink { get; set; }

        /// <summary>
        /// Gets the output link, which points to another possible pattern occuring at that position.
        /// </summary>
        public TrieNode OutputLink { get; set; }

        /// <summary>
        /// Gets a child node.
        /// </summary>
        /// <param name="character">Character being searched for.</param>
        /// <returns>Returns null if no child is found.</returns>
        public TrieNode GetChild(char character)
        {
            // A dumb search, since we are not concerned about execution time.
            foreach (var child in this.children)
            {
                if (child.Character.Equals(character))
                {
                    return child;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds a child node.
        /// </summary>
        /// <param name="child">The child node.</param>
        public void AddChild(TrieNode child)
        {
            // Resizing the array every time by one to keep it small. Memory-wise, this is the best we can do, but it consumes more CPU, makeing the process slower.
            Array.Resize(ref this.children, this.children.Length + 1);
            this.children[this.children.Length - 1] = child;
        }
    }
}
