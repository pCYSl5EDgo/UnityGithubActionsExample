using NUnit.Framework;

namespace Tests
{
    public class NewTestScript
    {
        [Test]
        public void IsTrueTest()
        {
            Assert.IsTrue(1 == 1);
        }

        [Test]
        public void IsFalseTest()
        {
            Assert.IsFalse(1 == 100);
        }
    }
}
