using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hjp.Fair_Die;

namespace ThrowsTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestReset()
        {
            Throws t = new Throws();
            t.Add(1);
            t.Add(6);
            var h = t.Histogram;
            Assert.AreEqual(7, h.Length);
            t.Add(12);
            h = t.Histogram;
            Assert.AreEqual(13, h.Length);

        }

        [TestMethod]
        public void TestTwoThrows()
        {
            Throws t1 = new Throws();
            t1.Add(1);
            t1.Add(6);
            var h1a = t1.Histogram;
            Assert.AreEqual(1, h1a[1]);
            Assert.AreEqual(0, h1a[2]);
            Assert.AreEqual(0, h1a[3]);
            Assert.AreEqual(0, h1a[4]);
            Assert.AreEqual(0, h1a[5]);
            Assert.AreEqual(1, h1a[6]);

            Throws t2 = new Throws();
            t2.Add(2);
            t2.Add(4);
            t2.Add(2);
            var h2b = t2.Histogram;
            Assert.AreEqual(5, h2b.Length);
            Assert.AreEqual(0, h2b[1]);
            Assert.AreEqual(2, h2b[2]);
            Assert.AreEqual(0, h2b[3]);
            Assert.AreEqual(1, h2b[4]);

            var h1b = t1.Histogram;
            Assert.AreEqual(1, h1b[1]);
            Assert.AreEqual(0, h1b[2]);
            Assert.AreEqual(0, h1b[3]);
            Assert.AreEqual(0, h1b[4]);
            Assert.AreEqual(0, h1b[5]);
            Assert.AreEqual(1, h1b[6]);

        }
    }
}
