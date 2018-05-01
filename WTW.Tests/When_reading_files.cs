using System.IO;
using NUnit.Framework;

namespace WTW.Tests
{
    [TestFixture]
    public class When_reading_files
    {
        private const string Filename = @"C:\Workspaces\WTW\WTW.Tests\Data\Example.csv";

        [Test]
        public void it_is_readable()
        {
            string csv = File.ReadAllText(Filename);
            int length = csv.Length;
            const int expectedLength = 385;
            Assert.That(length, Is.EqualTo(expectedLength));
        }
    }
}
