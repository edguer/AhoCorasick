using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace AhoCorasick.LoadTests
{
    [TestClass]
    public class HighLoadTests : LoadTestBase
    {
        public HighLoadTests()
        {
            base.LoggingLevel = LogLevel.Telemetry;
        }
        [TestMethod]
        public void HighLoadA()
        {
            Run(textSize:               1500,
                matchingTermsCount:     75,
                randomTermsCount:       5000,
                termSizes:              15);
        }
        [TestMethod]
        public void HighLoadB()
        {
            Run(textSize:               1500,
                matchingTermsCount:     100,
                randomTermsCount:       10000,
                termSizes:              15);
        }
        [TestMethod]
        public void HighLoadC()
        {
            Run(textSize:               2000,
                matchingTermsCount:     150,
                randomTermsCount:       50000,
                termSizes:              15);
        }
        [TestMethod]
        public void HighLoadD()
        {
            Run(textSize:               3000,
                matchingTermsCount:     200,
                randomTermsCount:       100000,
                termSizes:              15);
        }
        [TestMethod]
        public void HighLoadE()
        {
            Run(textSize:               4000,
                matchingTermsCount:     300,
                randomTermsCount:       150000,
                termSizes:              15);
        }
        [TestMethod]
        public void HighLoadF()
        {
            Run(textSize:               5000,
                matchingTermsCount:     400,
                randomTermsCount:       200000,
                termSizes:              15);
        }
        [TestMethod]
        public void HighLoadG()
        {
            Run(textSize:               10000,
                matchingTermsCount:     1000,
                randomTermsCount:       250000,
                termSizes:              50);
        }
    }
}
