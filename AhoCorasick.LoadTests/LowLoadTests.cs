using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace AhoCorasick.LoadTests
{
    [TestClass]
    public class LowLoadTests : LoadTestBase
    {
        public LowLoadTests()
        {
            base.LoggingLevel = LogLevel.Telemetry;
        }
        [TestMethod]
        public void LowLoadA()
        {
            Run(textSize:               50, 
                matchingTermsCount:     1, 
                randomTermsCount:       0, 
                termSizes:              3);
        }

        [TestMethod]
        public void LowLoadB()
        {
            Run(textSize:               50, 
                matchingTermsCount:     3, 
                randomTermsCount:       2, 
                termSizes:              3);
        }

        [TestMethod]
        public void LowLoadC()
        {
            Run(textSize:               50, 
                matchingTermsCount:     0, 
                randomTermsCount:       10, 
                termSizes:              3);
        }

        [TestMethod]
        public void LowLoadD()
        {
            Run(textSize:               150, 
                matchingTermsCount:     0, 
                randomTermsCount:       10, 
                termSizes:              10);
        }

        [TestMethod]
        public void LowLoadE()
        {
            Run(textSize:               150, 
                matchingTermsCount:     10, 
                randomTermsCount:       5, 
                termSizes:              10);
        }
    }
}
