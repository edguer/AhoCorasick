using AhoCorasick.UnitTests.Fixtures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhoCorasick.UnitTests
{
    [TestClass]
    public class TrieTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProcessEmptyTrie()
        {
            var trie = new Trie();
            trie.ProcessFailureLinks();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SearchOnUnprocessedTrie()
        {
            var trie = new Trie();
            trie.AddPattern("ABC");

            trie.FindAllMatches("ABC");
        }

        [TestMethod]
        public void SearchOneTerm()
        {
            var textToBeSearched = "Where is Aho-Corasick?";
            var searchTerm = "Aho-Corasick";

            var trie = new Trie();
            trie.AddPattern(searchTerm);
            trie.ProcessFailureLinks();

            var matches = trie.FindAllMatches(textToBeSearched);

            Assert.AreEqual(matches.Count, 1);
            
            var result = matches.First();
            Assert.AreEqual(result.Pattern, searchTerm);
            Assert.AreEqual(result.Indices.First(), textToBeSearched.IndexOf(searchTerm));
        }

        [TestMethod]
        public void Serialization()
        {
            var textToBeSearched = "Where is Aho-Corasick?";
            var searchTerm = "Aho-Corasick";

            var trie = new Trie();
            trie.AddPattern(searchTerm);
            trie.ProcessFailureLinks();

            trie = Trie.Deserialize(trie.Serialize());

            trie.ProcessFailureLinks();
            var matches = trie.FindAllMatches(textToBeSearched);

            Assert.AreEqual(matches.Count, 1);

            var result = matches.First();
            Assert.AreEqual(result.Pattern, searchTerm);
            Assert.AreEqual(result.Indices.First(), textToBeSearched.IndexOf(searchTerm));
        }

        [TestMethod]
        public void SearchOneTermWithComparer()
        {
            var textToBeSearched = "Where is Aho-Corasick?";
            var searchTerm = "AHO-CORASICK";

            var trie = new Trie();
            trie.AddPattern(searchTerm);
            trie.ProcessFailureLinks();

            var matches = trie.FindAllMatches(textToBeSearched, new CharComparerInvariant());

            Assert.AreEqual(matches.Count, 1);
            
            var result = matches.First();
            Assert.AreEqual(result.Pattern, searchTerm);
            Assert.AreEqual(result.Indices.First(), textToBeSearched.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void SearchOneTermNoMatches()
        {
            var textToBeSearched = "Where is Aho-Corasick?";
            var searchTerm = "no-match";

            var trie = new Trie();
            trie.AddPattern(searchTerm);
            trie.ProcessFailureLinks();

            var matches = trie.FindAllMatches(textToBeSearched);

            Assert.AreEqual(matches.Count, 0);
        }

        [TestMethod]
        public void SameTermAddedTwice()
        {
            var textToBeSearched = "Where is Aho-Corasick?";
            var searchTerm = "Aho-Corasick";

            var trie = new Trie();
            trie.AddPattern(searchTerm);
            trie.AddPattern(searchTerm);
            trie.ProcessFailureLinks();

            var matches = trie.FindAllMatches(textToBeSearched);

            Assert.AreEqual(matches.Count, 1);

            var result = matches.First();
            Assert.AreEqual(result.Pattern, searchTerm);
            Assert.AreEqual(result.Indices.First(), textToBeSearched.IndexOf(searchTerm));
        }

        [TestMethod]
        public void SameTermTwoOccurrences()
        {
            var textToBeSearched = "Where is Aho-Corasick? Aho-Corasick is here...";
            var searchTerm = "Aho-Corasick";

            var trie = new Trie();
            trie.AddPattern(searchTerm);
            trie.ProcessFailureLinks();

            var matches = trie.FindAllMatches(textToBeSearched);

            Assert.AreEqual(matches.Count, 2);

            var result1 = matches.First();
            Assert.AreEqual(result1.Pattern, searchTerm);
            Assert.AreEqual(result1.Indices.First(), textToBeSearched.IndexOf(searchTerm));
            Assert.AreEqual(result1.Indices.Last(), textToBeSearched.LastIndexOf(searchTerm));
        }

        [TestMethod]
        public void Subterms()
        {
            var textToBeSearched = "Sometimes a term is a subterm, and we need to use suffix links";
            var searchTerm = "subterm";
            var searchSubterm = "term";

            var trie = new Trie();
            trie.AddPattern(searchTerm);
            trie.AddPattern(searchSubterm);
            trie.ProcessFailureLinks();

            var matches = trie.FindAllMatches(textToBeSearched);

            Assert.AreEqual(matches.Count, 3);

            var resultTerm = matches.Where(t => t.Pattern == searchTerm).First();
            Assert.AreEqual(resultTerm.Pattern, searchTerm);
            Assert.AreEqual(resultTerm.Indices.First(), textToBeSearched.IndexOf(searchTerm));

            var resultSubterm = matches.Where(t => t.Pattern == searchSubterm).First();
            Assert.AreEqual(resultSubterm.Pattern, searchSubterm);
            Assert.AreEqual(resultSubterm.Indices.First(), textToBeSearched.IndexOf(searchSubterm));
            Assert.AreEqual(resultSubterm.Indices.Last(), textToBeSearched.LastIndexOf(searchSubterm));
        }

        [TestMethod]
        public void OverlappingTerms()
        {
            var textToBeSearched = "APDKVWUWOGER";
            var term1 = "APDKVWUWOG";
            var term2 = "DKVWUWOGER";
            var term3 = "PUUWMPVODG";

            var trie = new Trie();
            trie.AddPattern(term1);
            trie.AddPattern(term2);
            trie.AddPattern(term3);
            trie.ProcessFailureLinks();

            var matches = trie.FindAllMatches(textToBeSearched);

            Assert.AreEqual(matches.Count, 2);

            var result1 = matches.Where(t => t.Pattern == term1).First();
            Assert.AreEqual(result1.Pattern, term1);
            Assert.AreEqual(result1.Indices.First(), textToBeSearched.IndexOf(term1));

            var result2 = matches.Where(t => t.Pattern == term2).First();
            Assert.AreEqual(result2.Pattern, term2);
            Assert.AreEqual(result2.Indices.First(), textToBeSearched.IndexOf(term2));
        }

        [TestMethod]
        public void NestedTerms()
        {
            var textToBeSearched = "nested-term-here";
            var nestedTerm = "term";
            var term = "nested-term-here";

            var trie = new Trie();
            trie.AddPattern(nestedTerm);
            trie.AddPattern(term);
            trie.ProcessFailureLinks();

            var matches = trie.FindAllMatches(textToBeSearched);

            Assert.AreEqual(matches.Count, 2);

            var result = matches.Where(t => t.Pattern == term).First();
            Assert.AreEqual(result.Pattern, term);
            Assert.AreEqual(result.Indices.First(), textToBeSearched.IndexOf(term));

            var resultNestedTerm = matches.Where(t => t.Pattern == nestedTerm).First();
            Assert.AreEqual(resultNestedTerm.Pattern, nestedTerm);
            Assert.AreEqual(resultNestedTerm.Indices.First(), textToBeSearched.IndexOf(nestedTerm));
        }
    }
}
