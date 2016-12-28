//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using Coypu.Tests.TestBuilders;
//using Coypu.Tests.TestDoubles;
//using Coypu.Tests.When_interacting_with_the_browser;
//using Coypu.WebRequests;
//using Xunit;

//namespace Coypu.Tests.When_making_direct_web_requests
//{
//    public class When_saving_a_resource_from_the_web
//    {
//        private StubUrlBuilder stubUrlBuilder;
//        private BrowserSession browserSession;
//        private SpyRestrictedResourceDownloader _stubRestrictedResourceDownloader;
//        private FakeDriver driver;
//        private SessionConfiguration SessionConfiguration;

//        public When_saving_a_resource_from_the_web()
//        {
//            stubUrlBuilder = new StubUrlBuilder();
//            _stubRestrictedResourceDownloader = new SpyRestrictedResourceDownloader();

//            driver = new FakeDriver();
//            SessionConfiguration = new SessionConfiguration();
//            browserSession = TestSessionBuilder.Build(SessionConfiguration, driver, new SpyTimingStrategy(), new FakeWaiter(), _stubRestrictedResourceDownloader, stubUrlBuilder);
//        }

//        [Fact]
//        public void It_downloads_the_resource_from_the_given_url_and_saves_to_the_given_location()
//        {
//            StubResourceUrl("/resources/someresource", "http://built.by/url_builder", stubUrlBuilder);

//            browserSession.SaveWebResource("/resources/someresource", @"T:\saveme\here.please");

//            var downloadedFile = _stubRestrictedResourceDownloader.DownloadedFiles.Single();

//            Assert.Equal("http://built.by/url_builder", downloadedFile.Resource);
//            Assert.Equal(@"T:\saveme\here.please", downloadedFile.SaveAs);
//        }

//        [Fact]
//        public void It_passes_the_current_driver_cookies_to_the_resource_downloader()
//        {
//            StubResourceUrl("/resources/someresource", "http://built.by/url_builder", stubUrlBuilder);
//            var cookies = new List<Cookie> { new Cookie("SomeCookie", "some value") };

//            driver.StubCookies(cookies);

//            browserSession.SaveWebResource("/resources/someresource", @"T:\saveme\here.please");

//            var downloadedFile = _stubRestrictedResourceDownloader.DownloadedFiles.Single();

//            Assert.Equal(cookies, downloadedFile.Cookies);
//        }


//        private static void StubResourceUrl(string virtualPath, string fullyQualifiedPath, StubUrlBuilder stubUrlBuilder)
//        {
//            stubUrlBuilder.SetStubUrl(virtualPath, fullyQualifiedPath);
//        }

//        [Fact]
//        public void It_injects_cookies_into_the_web_request()
//        {
//            var requestUri = new Uri("http://cookiemonster.love/cookies/");
//            var expectedCookies = new List<Cookie>
//                                      {
//                                          new Cookie("n1","v1","/","cookiemonster.love"),
//                                          new Cookie("n2","v2","/","cookiemonster.love"),
//                                          new Cookie("n3","v3","/cookies/","cookiemonster.love"),
//                                      };

//            var webClient = new WebClientWithCookiesTestExtensionYuk();

//            webClient.SetCookies(expectedCookies);
//            var webRequest = webClient.GetWebRequest(requestUri);

//            var actualCookies = ((HttpWebRequest)webRequest).CookieContainer.GetCookies(requestUri);

//            Assert.Equal(3, actualCookies.Count);
//            Assert.Contains(expectedCookies[0], actualCookies);
//            Assert.Contains(expectedCookies[1], actualCookies);
//            Assert.Contains(expectedCookies[2], actualCookies);
//        }

//        [Fact]
//        public void It_handles_non_http_requests_without_trying_to_inect_cookies()
//        {
//            var requestUri = new Uri("ftp://cookiemonster.love/cookies/");
//            var expectedCookies = new List<Cookie>{new Cookie("n1","v1","/","cookiemonster.love")};

//            var webClient = new WebClientWithCookiesTestExtensionYuk();

//            webClient.SetCookies(expectedCookies);
//            var webRequest = webClient.GetWebRequest(requestUri);

//            Assert.Same(typeof(FtpWebRequest), webRequest);
//        }
//    }

    
//    internal class WebClientWithCookiesTestExtensionYuk : WebClientWithCookies
//    {
//        internal new WebRequest GetWebRequest(Uri address)
//        {
//            return base.GetWebRequest(address);
//        }
//    }
//}