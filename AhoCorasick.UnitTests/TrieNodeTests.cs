using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AhoCorasick.UnitTests
{
    [TestClass]
    public class TrieNodeTests
    {
        [TestMethod]
        public void EmptyNode()
        {
            var node = new TrieNode();

            Assert.AreEqual(node.Character, default);
            Assert.IsNull(node.LeafPattern);
            Assert.IsNull(node.FailureLink);
            Assert.IsNull(node.OutputLink);
            Assert.IsFalse(node.IsLeaf);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(node.Children.Length, 0);
        }

        [TestMethod]
        public void NonLeafNode()
        {
            var node = new TrieNode('A');

            Assert.AreEqual(node.Character, 'A');
            Assert.IsNull(node.LeafPattern);
            Assert.IsNull(node.FailureLink);
            Assert.IsNull(node.OutputLink);
            Assert.IsFalse(node.IsLeaf);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(node.Children.Length, 0);
        }

        [TestMethod]
        public void NodeWithChildren()
        {
            var node = new TrieNode('A');
            var child1 = new TrieNode('B');
            var child2 = new TrieNode('C');

            node.AddChild(child1);
            node.AddChild(child2);

            Assert.AreEqual(node.GetChild('B'), child1);
            Assert.AreEqual(node.GetChild('C'), child2);

            Assert.IsNull(node.LeafPattern);
            Assert.IsNull(node.FailureLink);
            Assert.IsNull(node.OutputLink);
            Assert.IsFalse(node.IsLeaf);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(node.Children.Length, 2);
        }

        [TestMethod]
        public void GrandChildren()
        {
            var node = new TrieNode('A');
            var child = new TrieNode('B');
            var grandchild = new TrieNode('C');

            node.AddChild(child);
            child.AddChild(grandchild);

            Assert.AreEqual(node.GetChild('B'), child);
            Assert.AreEqual(child.GetChild('C'), grandchild);

            Assert.IsNull(node.LeafPattern);
            Assert.IsFalse(node.IsLeaf);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(node.Children.Length, 1);

            Assert.IsNull(child.LeafPattern);
            Assert.IsFalse(child.IsLeaf);
            Assert.IsNotNull(child.Children);
            Assert.AreEqual(child.Children.Length, 1);

            Assert.IsNull(grandchild.LeafPattern);
            Assert.IsFalse(grandchild.IsLeaf);
            Assert.IsNotNull(grandchild.Children);
            Assert.AreEqual(grandchild.Children.Length, 0);
        }

        [TestMethod]
        public void LeafNode()
        {
            var node = new TrieNode('A');

            var child = new TrieNode('B');
            node.AddChild(child);

            var grandchild = new TrieNode('C')
            {
                LeafPattern = "ABC"
            };
            child.AddChild(grandchild);

            Assert.AreEqual(node.GetChild('B'), child);
            Assert.AreEqual(child.GetChild('C'), grandchild);

            Assert.IsNull(node.LeafPattern);
            Assert.IsFalse(node.IsLeaf);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(node.Children.Length, 1);

            Assert.IsNull(child.LeafPattern);
            Assert.IsFalse(child.IsLeaf);
            Assert.IsNotNull(child.Children);
            Assert.AreEqual(child.Children.Length, 1);

            Assert.AreEqual(grandchild.LeafPattern, "ABC");
            Assert.IsTrue(grandchild.IsLeaf);
            Assert.IsNotNull(grandchild.Children);
            Assert.AreEqual(grandchild.Children.Length, 0);
        }
    }
}
