using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace AhoCorasick.LoadTests
{
    [TestClass]
    public class MediumLoadTests : LoadTestBase
    {
        public MediumLoadTests()
        {
            base.LoggingLevel = LogLevel.Telemetry;
        }
        [TestMethod]
        public void MediumLoadA()
        {
            Run(textSize:               500, 
                matchingTermsCount:     5, 
                randomTermsCount:       50, 
                termSizes:              10);
        }

        [TestMethod]
        public void MediumLoadB()
        {
            Run(textSize:               500, 
                matchingTermsCount:     10, 
                randomTermsCount:       70, 
                termSizes:              10);
        }

        [TestMethod]
        public void MediumLoadC()
        {
            Run(textSize:               500, 
                matchingTermsCount:     15, 
                randomTermsCount:       200, 
                termSizes:              10);
        }

        [TestMethod]
        public void MediumLoadD()
        {
            Run(textSize:               500, 
                matchingTermsCount:     15, 
                randomTermsCount:       500, 
                termSizes:              15);
        }

        [TestMethod]
        public void MediumLoadE()
        {
            Run(textSize:               1000,
                matchingTermsCount:     30,
                randomTermsCount:       500,
                termSizes:              15);
        }

        [TestMethod]
        public void MediumLoadF()
        {
            Run(textSize:               1000,
                matchingTermsCount:     50,
                randomTermsCount:       1000,
                termSizes:              15);
        }
    }
}
