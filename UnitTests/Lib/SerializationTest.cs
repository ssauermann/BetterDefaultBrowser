using BetterDefaultBrowser.Lib.Models;
using BetterDefaultBrowser.Lib.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YAXLib;

namespace UnitTests.Lib
{
    [TestClass]
    public class SerializationTest
    {
        private static readonly BrowserStorage B = new BrowserStorage { BrowserKey = "My Key", BrowserName = "My Name" };
        private static readonly PlainFilter Fp = new PlainFilter
        {
            Browser = B,
            Name = "My plain Filter",
            Priority = 1,
            Regex = ".*"
        };
        private static readonly ManagedFilter Fm = new ManagedFilter
        {
            Browser = B,
            Name = "My managed filter",
            Protocols = Protocols.HTTP,
            Flags = Ignore.Parameter,
            Priority = 1,
            Url = "www.google.com"
        };

        private static readonly OpenFilter Fo = new OpenFilter
        {
            Browsers = { B },
            Name = "My open filter",
            Priority = 1,
            OnlyOpen = false,
            InnerFilter = Fp
        };

        [TestInitialize]
        public void SetUp()
        {

        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void TestBrowserStorage()
        {
            var b = SerializeAndBack(B);
            Assert.AreEqual(B.BrowserKey, b.BrowserKey);
            Assert.AreEqual(B.BrowserName, b.BrowserName);
        }

        [TestMethod]
        public void TestPlainFilter()
        {
            var fp = SerializeAndBack(Fp);
            FilterEqual(Fp, fp);
            Assert.AreEqual(Fp.Browser, fp.Browser);
            Assert.AreEqual(Fp.Regex, fp.Regex);
        }

        [TestMethod]
        public void TestManagedFilter()
        {
            var fm = SerializeAndBack(Fm);
            FilterEqual(Fm, fm);
            Assert.AreEqual(Fm.Flags, fm.Flags);
            Assert.AreEqual(Fm.Protocols, fm.Protocols);
            Assert.AreEqual(Fm.Url, fm.Url);
        }

        [TestMethod]
        public void TestOpenFilter()
        {
            var fo = SerializeAndBack(Fo);
            FilterEqual(Fo, fo);
            Assert.AreEqual(Fo.OnlyOpen, fo.OnlyOpen);
            FilterEqual(Fo.InnerFilter, fo.InnerFilter);
            foreach (var browser in Fo.Browsers)
            {
                Assert.IsTrue(fo.Browsers.Contains(browser));
            }
        }

        private void FilterEqual(Filter f, Filter f2)
        {
            Assert.AreEqual(f.Name, f2.Name);
            Assert.AreEqual(f.Priority, f2.Priority);
            Assert.AreEqual(f.Id, f2.Id);
        }

        private T SerializeAndBack<T>(T obj)
        {
            var ser = new YAXSerializer(typeof(T));
            var xml = ser.Serialize(obj);
            var b = (T)ser.Deserialize(xml);
            return b;
        }
    }
}
