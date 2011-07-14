using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class When_saving_a_resource_from_the_web
    {
        [Test]
        public void It_requests_the_resource_from_the_given_url_and_saves_to_given_location()
        {
            var stubWebResources = new StubWebResources();
            var spyFileSystem = new SpyFileSystem();

            const string resource = "http://myserver/resources/someresource";
            var webResponse = new StubWebResponse();
            stubWebResources.StubResource(resource, webResponse);

            var session = TestSessionBuilder.Build(stubWebResources, spyFileSystem);

            const string saveAs = @"T:\saveme\here.please";

            session.SaveWebResource(resource, saveAs);

            var savedStream = spyFileSystem.SavedStreams.SingleOrDefault(s => s.Path == saveAs);
            Assert.That(savedStream, Is.Not.Null, "No stream was saved to " + saveAs);
            Assert.That(savedStream.Stream, Is.SameAs(webResponse.GetResponseStream()));
        }
    }
}