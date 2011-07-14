using System.Linq;
using Coypu.Tests.TestDoubles;
using Coypu.Tests.When_interacting_with_the_browser;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class When_saving_a_resource_from_the_web
    {
        private StubUrlBuilder stubUrlBuilder;
        private Session session;
        private SpyResourceDownloader stubResourceDownloader;

        [SetUp]
        public void SetUp()
        {
            stubUrlBuilder = new StubUrlBuilder();
            stubResourceDownloader = new SpyResourceDownloader();

            session = TestSessionBuilder.Build(new FakeDriver(), new SpyRobustWrapper(), new FakeWaiter(), stubResourceDownloader, stubUrlBuilder);
        }

        [Test]
        public void It_requests_the_resource_from_the_given_url_and_saves_to_given_location() 
        {
            StubResourceUrl("/resources/someresource", "http://built.by/url_builder", stubUrlBuilder);

            session.SaveWebResource("/resources/someresource", @"T:\saveme\here.please");

            var downloadedFile = stubResourceDownloader.DownloadedFiles.Single();

            Assert.That(downloadedFile.Resource, Is.EqualTo("http://built.by/url_builder"));
            Assert.That(downloadedFile.SaveAs, Is.EqualTo(@"T:\saveme\here.please"));
        }


      
        private void StubResourceUrl(string virtualPath, string fullyQualifiedPath, StubUrlBuilder stubUrlBuilder)
        {
            stubUrlBuilder.SetStubUrl(virtualPath, fullyQualifiedPath);
        }
    }



}