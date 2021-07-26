using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AhoCorasick.LoadTests
{
    public class LoadTestBase
    {
        protected enum LogLevel
        {
            Verbose,
            Telemetry,
            None
        }
        protected LogLevel LoggingLevel { get; set; } = LogLevel.None;
        private readonly Random random = new Random();
        protected string GenerateString(int size = 1)
        {
            var builder = new StringBuilder(size);

            for (var i = 0; i < size; i++)
            {
                var @char = (char)random.Next(65, 90);
                builder.Append(@char);
            }

            return builder.ToString();
        }
        protected string GenerateMatchingTerm(string text, int size)
        {
            var index = random.Next(0, text.Length - size - 1);
            return text.Substring(index, size);
        }

        protected void Run(int textSize, int matchingTermsCount, int randomTermsCount, int termSizes)
        {
            if (LoggingLevel == LogLevel.Verbose)
                WriteLog("Creating trie object...");

            var trie = new Trie();

            if (LoggingLevel == LogLevel.Verbose)
                WriteLog("Randomly generating text for search...");

            var text = GenerateString(textSize);
            if (LoggingLevel == LogLevel.Verbose)
                WriteLog(text);

            if (LoggingLevel == LogLevel.Verbose)
                WriteLog("Generating matching terms...");

            var matchingTerms = new string[matchingTermsCount];
            for (int i = 0; i < matchingTermsCount; i++)
            {
                matchingTerms[i] = GenerateMatchingTerm(text, termSizes);
                if (LoggingLevel == LogLevel.Verbose)
                    WriteLog(matchingTerms[i]);
            }

            // guarantee there are no duplicates
            matchingTerms = matchingTerms.Distinct().ToArray();

            if (LoggingLevel == LogLevel.Verbose)
                WriteLog("Generating random terms...");

            var randomTerms = new string[randomTermsCount];
            for (int i = 0; i < randomTermsCount; i++)
            {
                randomTerms[i] = GenerateString(termSizes);
                if (LoggingLevel == LogLevel.Verbose)
                    WriteLog(randomTerms[i]);
            }

            if (LoggingLevel == LogLevel.Verbose)
                WriteLog("Adding all terms to Trie...");

            StopWatch(() =>
            {
                foreach (var term in matchingTerms.Union(randomTerms))
                {
                    trie.AddPattern(term);
                }
            }, "building trie nodes");

            if (LoggingLevel == LogLevel.Verbose)
                WriteLog("Processing failure links...");

            StopWatch(() => trie.ProcessFailureLinks(), "ProcessFailureLinks");

            if (LoggingLevel == LogLevel.Verbose)
                WriteLog("Finding matches...");

            IList<MatchResult> matches = default;
            StopWatch(() => matches = trie.FindAllMatches(text), "FindAllMatches");

            if (LoggingLevel == LogLevel.Verbose)
                WriteLog("Checking matches...");

            foreach (var term in matchingTerms)
            {
                var match = matches.Where(m => m.Pattern.Equals(term)).FirstOrDefault();
                Assert.IsNotNull(match, $"Matching term {term} should be present on the FindAllMatches results, but nothing could be found.");
            }

            if (LoggingLevel == LogLevel.Verbose)
                WriteLog("Serializing Trie...");

            byte[] serializedTrie = new byte[0];
            StopWatch(() => serializedTrie = trie.Serialize(), "serialization");

            if (LoggingLevel == LogLevel.Telemetry || LoggingLevel == LogLevel.Verbose)
                WriteLog($"Trie size after serializing: {serializedTrie.Length / 1024} kb");

            if (LoggingLevel == LogLevel.Verbose)
                WriteLog("Deserialing Trie...");

            Trie newTrie = default;

            StopWatch(() => newTrie = Trie.Deserialize(serializedTrie), "deserialization");

            Assert.IsNotNull(newTrie, "Trie is empty after deserialization.");

            StopWatch(() => trie.ProcessFailureLinks(), "ProcessFailureLinks (after deserialization)");
        }
        private void WriteLog(string message)
        {
            Console.WriteLine(message);
        }
        private void StopWatch(Action action, string metricName)
        {
            var beforeDate = DateTime.Now;
            action.Invoke();
            var ellapsedTime = DateTime.Now - beforeDate;

            if (LoggingLevel == LogLevel.Telemetry || LoggingLevel == LogLevel.Verbose)
                WriteLog($"Total time executed in milliseconds for {metricName}: {ellapsedTime}");
        }

    }
}
