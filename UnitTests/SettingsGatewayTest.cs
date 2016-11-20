using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;

namespace UnitTests
{
    [TestClass]
    public class SettingsGatewayTest
    {
        private string _path = "myTestSettings.xml";
        private SettingsGateway _settings;
        [TestInitialize]
        public void SetUp()
        {
            File.Delete(_path);
            _settings = new SettingsGateway(_path);
        }

        [TestCleanup]
        public void CleanUp()
        {
            // File.Delete(_path);
        }

        [TestMethod]
        public void TestDefaultBrowser()
        {
            Assert.AreEqual(null, _settings.DefaultBrowser, "Default Browser should be null.");

            var b = new BrowserStorage { BrowserKey = "My Key", BrowserName = "My Name" };
            // Setting
            _settings.DefaultBrowser = b;
            // Getting
            Assert.AreEqual(b, _settings.DefaultBrowser, "Getter has to return set browser.");

            // Must be serialized in file
            var otherGateway = new SettingsGateway(_path);
            Assert.AreEqual(b, otherGateway.DefaultBrowser, "Browser has to be stored in the settings file.");
        }

        [TestMethod]
        public void TestFilters()
        {
            Assert.AreEqual(0, _settings.GetFilters().Count, "Filter list should be empty.");

            var b = new BrowserStorage { BrowserKey = "My Key", BrowserName = "My Name" };
            var f1 = new PlainFilter() { Browser = b, Name = "My Filter", Priority = 1, Regex = ".*" };
            var f2 = new PlainFilter() { Browser = b, Name = "My other filter", Priority = 5, Regex = ".*" };

            // Remove filter from empty list
            _settings.RemoveFilter(f1);
            Assert.AreEqual(0, _settings.GetFilters().Count, "Filter list should be empty.");

            // Add filter
            _settings.UpdateOrAddFilter(f1);
            Assert.AreEqual(1, _settings.GetFilters().Count, "Filter list should have 1 element.");
            Assert.IsTrue(_settings.GetFilters().Contains(f1), "Filter should be in the list.");

            // Assert other filter is not in list
            Assert.IsFalse(_settings.GetFilters().Contains(f2), "Second filter should not be in the list.");

            // Add filter again (should not change anything)
            _settings.UpdateOrAddFilter(f1);
            Assert.AreEqual(1, _settings.GetFilters().Count, "Filter list should have 1 element.");
            Assert.IsTrue(_settings.GetFilters().Contains(f1), "Filter should be in the list.");

            // Remove second filter (should not change anything)
            _settings.RemoveFilter(f2);
            Assert.AreEqual(1, _settings.GetFilters().Count, "Filter list should have 1 element.");
            Assert.IsTrue(_settings.GetFilters().Contains(f1), "Filter should be in the list.");

            // Add second filter
            _settings.UpdateOrAddFilter(f2);
            Assert.AreEqual(2, _settings.GetFilters().Count, "Filter list should have 1 element.");
            Assert.IsTrue(_settings.GetFilters().Contains(f1), "Filter should be in the list.");
            Assert.IsTrue(_settings.GetFilters().Contains(f2), "Second filter should be in the list.");

            // Remove first filter
            _settings.RemoveFilter(f1);
            Assert.AreEqual(1, _settings.GetFilters().Count, "Filter list should have 1 element.");
            Assert.IsFalse(_settings.GetFilters().Contains(f1), "Filter should not be in the list.");
            Assert.IsTrue(_settings.GetFilters().Contains(f2), "Second filter should be in the list.");

            // Remove first filter again (should not change anything)
            _settings.RemoveFilter(f1);
            Assert.AreEqual(1, _settings.GetFilters().Count, "Filter list should have 1 element.");
            Assert.IsFalse(_settings.GetFilters().Contains(f1), "Filter should not be in the list.");
            Assert.IsTrue(_settings.GetFilters().Contains(f2), "Second filter should be in the list.");

            // Filters have to be serialized
            var otherSettings = new SettingsGateway(_path);
            foreach (var filter in _settings.GetFilters())
            {
                Assert.IsTrue(otherSettings.GetFilters().Contains(filter), "Filters are serialized correctly.");
            }
        }
    }
}
