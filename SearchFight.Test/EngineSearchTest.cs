using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using Tranzact.Search;
using Assert = NUnit.Framework.Assert;

namespace SearchFight.Test
{
    public class EngineSearchTest
    {
        private Mock<IConfigurationRoot> _configuration;
        private GoogleSearch _google;
        private BingSearch _bing;
        [SetUp]
        public void Setup()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://www.googleapis.com/customsearch/v1?key=AIzaSyCUiClkLL2F6ddVhN29Zr66uh0IHi04o9w&cx=6dc9392c98db553b6&q=net")
                .Respond("application/json", "{\"searchInformation\":{\"searchTime\":0.562752,\"formattedSearchTime\":\"0.56\",\"totalResults\":\"20000\",\"formattedTotalResults\":\"13,400,000\"}}"); // Respond with JSON
            mockHttp.When("https://api.bing.microsoft.com/v7.0/search?q=net")
                .Respond("application/json", "{\"webPages\":{\"webSearchUrl\":\"https://www.bing.com/search?q=net\",\"totalEstimatedMatches\":30000}}"); // Respond with JSON
           
            HttpClient client = mockHttp.ToHttpClient();
            _configuration = new Mock<IConfigurationRoot>();
            _configuration.SetupGet(x => x["Search:Google:UrlBase"]).Returns("https://www.googleapis.com/customsearch/v1?key=AIzaSyCUiClkLL2F6ddVhN29Zr66uh0IHi04o9w&cx=6dc9392c98db553b6");
            _configuration.SetupGet(x => x["Search:Bing:UrlBase"]).Returns("https://api.bing.microsoft.com/v7.0/search");
            _google = new GoogleSearch(client, _configuration.Object);
            _bing = new  BingSearch(client, _configuration.Object);

        }

        [Test]
        [TestCase(null)]
        public void Google_GetSearchInfoAsync_Null_SearchQuery(string args)
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>( () =>  _google.GetSearchInfoAsync(args));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter 'searchQuery')"));
        }
        [Test]
        [TestCase(null)]
        public void Bing_GetSearchInfoAsync_Null_SearchQuery(string args)
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _bing.GetSearchInfoAsync(args));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter 'searchQuery')"));
        }
        [Test]
        [TestCase("net")]
        public async Task Google_GetSearchInfoAsync_Ok(string args)
        {
            SearchResponse ressponse =await _google.GetSearchInfoAsync(args);
            Assert.AreEqual(ressponse.ResultCount , 20000);
          
        }
        [Test]
        [TestCase("net")]
        public async Task Bing_GetSearchInfoAsync_Ok(string args)
        {
            SearchResponse ressponse = await _bing.GetSearchInfoAsync(args);
            Assert.AreEqual(ressponse.ResultCount, 30000);

        }
    }
}