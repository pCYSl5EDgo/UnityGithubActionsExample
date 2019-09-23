using NUnit.Framework;

namespace Tests
{
    public class NewTestScript
    {
        [Test] public void IsTrueTest() => Assert.IsTrue(true);

        [Test] public void IsFalseTest() => Assert.IsFalse(false);

        [Test] public void FailProof() => Assert.IsTrue(false);
    }
}
