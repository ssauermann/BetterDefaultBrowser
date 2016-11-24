using System;
using System.Collections.Generic;
using BetterDefaultBrowser.Lib.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Lib.Models
{
    // Name, priority, browser, regex, valid configuration?
    using TestData = Tuple<string, int, BrowserStorage, string, bool>;

    [TestClass]
    public class PlainFilterTest
    {
        private PlainFilter _f;

        [TestInitialize]
        public void SetUp()
        {
            _f = new PlainFilter();
        }

        // Validation tests

        private static readonly string N = "Some name";
        private static readonly BrowserStorage B = new BrowserStorage() { BrowserKey = "Foo", BrowserName = "Bar" };
        private static readonly string R = ".*";

        private readonly List<TestData> _testData =
                new List<TestData>()
                {
                    new TestData(null, 0, null, null, false),
                    new TestData(N, 0, null, null, false),
                    new TestData(null, 0, B, null, false),
                    new TestData(null, 0, null, R, false),
                    new TestData(N, 0, B, null, false),
                    new TestData(N, 0, null, R, false),
                    new TestData(null, 0, B, R, false),
                    new TestData(N, 0, B, R, true),
                    new TestData(N, -1, B, R, false),
                    new TestData(N, 0, B, @"\", false),
                }
            ;

        [TestMethod]
        public void TestValidation()
        {
            foreach (var tuple in _testData)
            {
                var f = new PlainFilter
                {
                    Name = tuple.Item1,
                    Priority = tuple.Item2,
                    Browser = tuple.Item3,
                    Regex = tuple.Item4
                };

                if (tuple.Item5)
                {
                    Assert.IsTrue(f.IsValid, "Was invalid, should be valid: " + tuple);
                }
                else
                {
                    Assert.IsFalse(f.IsValid, "Was valid, should be invalid: " + tuple);
                }
            }
        }

    }
}
