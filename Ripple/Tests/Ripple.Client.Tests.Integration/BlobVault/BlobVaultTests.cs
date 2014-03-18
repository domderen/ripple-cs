using NUnit.Framework;

namespace Ripple.Client.Tests.Integration.BlobVault
{
    [TestFixture]
    public class BlobVaultTests
    {
        [Test]
        public async void GetBlob_ReturnsProperData()
        {
            // Arrange
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            const string username = "";
            const string password = "";
            var blobVault = new Client.BlobVault.BlobVault("https://blobvault.payward.com/");

            // Act
            var result = await blobVault.GetBlob(username, password);

            // Assert
            Assert.IsTrue(result.HasValues);
        }
    }
}
