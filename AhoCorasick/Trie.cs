using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhoCorasick
{
    /// <summary>
    /// Trie with Aho-Corasick implementation for multi-pattern string matching.
    /// </summary>
    [ProtoContract(SkipConstructor = true)]
    public class Trie
    {
        [ProtoMember(1)]
        public readonly TrieNode root;

        [ProtoMember(2)]
        private int trieSize = 0;

        private bool failureLinksProcessed = false;

        /// <summary>
        /// Gets the Trie size, meaning the number of nodes.
        /// </summary>
        public int TrieSize => trieSize;

        /// <summary>
        /// Standard constructor, already adds a root node.
        /// </summary>
        public Trie()
        {
            this.root = new TrieNode();
        }

        /// <summary>
        /// Adds a search pattern.
        /// </summary>
        /// <param name="pattern">Pattern to be searched.</param>
        public void AddPattern(string pattern)
        {
            var currentNode = root;

            foreach (var character in pattern)
            {
                TrieNode nextNode = currentNode.GetChild(character);
                if (nextNode == null)
                {
                    nextNode = new TrieNode(character);
                    currentNode.AddChild(nextNode);
                    trieSize++;
                }

                currentNode = nextNode;
            }

            currentNode.LeafPattern = pattern;
        }

        /// <summary>
        /// Process the failure links.
        /// </summary>
        public void ProcessFailureLinks()
        {
            AssertSearchTerms();

            var queue = new Queue<TrieChildParentNode>(trieSize);

            // Set root's children failure link to root and add their children to BFS queue
            foreach (var rootChild in root.Children)
            {
                rootChild.FailureLink = root;

                foreach (var child in rootChild.Children)
                {
                    queue.Enqueue(new TrieChildParentNode(rootChild, child));
                }
            }

            // BFS: for each child node, add its children to queue and move on.
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                foreach (var child in currentNode.Node.Children)
                {
                    queue.Enqueue(new TrieChildParentNode(currentNode.Node, child));
                }

                // If failure and output links are not null, infer them.
                if (currentNode.Node.FailureLink == null)
                {
                    currentNode.Node.FailureLink = GetSuffixLink(currentNode);
                }

                if (currentNode.Node.OutputLink == null)
                {
                    currentNode.Node.OutputLink = currentNode.Node.FailureLink.IsLeaf ? currentNode.Node.FailureLink : currentNode.Node.FailureLink.OutputLink;
                }
            }

            failureLinksProcessed = true;
        }

        /// <summary>
        /// Finds all matches provided through <see cref="AddPattern(string)"/> on a given text.
        /// </summary>
        /// <param name="text">Text to be searched.</param>
        /// <returns></returns>
        public IList<MatchResult> FindAllMatches(string text, IEqualityComparer<char> comparer = null)
        {
            AssertSearchTerms();

            if (!failureLinksProcessed)
            {
                throw new ArgumentException("Failure links were not processed, please ProcessFailureLinks() before getting matches.");
            }

            var matches = new List<MatchResult>();
            int position = 0;

            var currentNode = this.root;
            foreach (var character in text)
            {
                TrieNode node = null;
                if ((node = currentNode.GetChild(character, comparer)) != null)
                {
                    currentNode = node;
                }
                else
                {
                    while (currentNode != this.root)
                    {
                        currentNode = currentNode.FailureLink;
                        if ((node = currentNode.GetChild(character, comparer)) != null)
                        {
                            currentNode = node;
                            break;
                        }
                    }
                }

                if (currentNode.IsLeaf)
                {
                    AddMatch(currentNode);
                }

                var outputLink = currentNode.OutputLink;
                while (outputLink != null && outputLink != this.root)
                {
                    AddMatch(outputLink);
                    outputLink = outputLink.OutputLink;
                }

                position++;
            }

            return matches;

            void AddMatch(TrieNode node)
            {
                var match = matches.Where(m => m.Pattern.Equals(node.LeafPattern)).FirstOrDefault() ?? new MatchResult(node.LeafPattern);
                match.Indices.Add(position - node.LeafPattern.Length + 1);

                matches.Add(match);
            }
        }

        /// <summary>
        /// Infer the suffix link for a given node.
        /// </summary>
        /// <param name="node">Parent/child node set.</param>
        /// <returns></returns>
        private TrieNode GetSuffixLink(TrieChildParentNode node)
        {
            var suffixLink = node.Parent?.FailureLink;

            TrieNode childNode = suffixLink != null ? suffixLink.GetChild(node.Node.Character) : null;
            while (suffixLink != this.root && childNode == null)
            {
                suffixLink = suffixLink.FailureLink;
                childNode = suffixLink != null ? suffixLink.GetChild(node.Node.Character) : null;
            }

            if (childNode != null)
            {
                suffixLink = suffixLink.GetChild(node.Node.Character);
            }

            return suffixLink;
        }

        /// <summary>
        /// Asserts whether search patterns have been provided.
        /// </summary>
        private void AssertSearchTerms()
        {
            if (trieSize == 0)
            {
                throw new ArgumentException("No search terms have been provided.");
            }
        }

        /// <summary>
        /// Uses Protobuf for serializing the Trie.
        /// </summary>
        /// <returns>Array of bytes with the serialized Trie.</returns>
        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, this);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Deserializes a Trie.
        /// </summary>
        /// <param name="bin">Binary representation of the Protobuf serialized Trie.</param>
        /// <returns>Trie with no failure links processed.</returns>
        public static Trie Deserialize(byte[] bin)
        {
            using (var stream = new MemoryStream(bin))
            {
                return Serializer.Deserialize<Trie>(stream);
            }
        }
    }
}
