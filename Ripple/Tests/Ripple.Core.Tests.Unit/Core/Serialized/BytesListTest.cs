namespace Ripple.Core.Tests.Unit.Core.Serialized
{
    using System.Text;

    using NUnit.Framework;

    using Ripple.Core.Core.Serialized;

    [TestFixture]
    public class BytesListTest
    {
        [Test]
        public void TestNested()
        {
            var ba1 = new BytesList();
            var ba2 = new BytesList();

            ba1.Add(new[] { (byte)'a', (byte)'b', (byte)'c' });
            ba1.Add(new[] { (byte)'d', (byte)'e' });

            ba2.Add(new[] { (byte)'f', (byte)'g' });
            ba2.Add((byte)'h');
            ba2.Add(ba1);

            Assert.AreEqual(8, ba2.BytesLength);

            byte[] bytes = ba2.Bytes();

            var ascii = Encoding.ASCII.GetString(bytes);
            Assert.AreEqual("fghabcde", ascii);
        }
    }
}
