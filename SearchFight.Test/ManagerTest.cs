using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using Tranzact.Search;
using Tranzact.Search.Managers;
using Tranzact.Search.Models;
using Assert = NUnit.Framework.Assert;

namespace SearchFight.Test
{
    public class ManagerTest
    {
        private Mock<IConfigurationRoot> _configuration;
        private MockHttpMessageHandler _mockHttp;
        private GoogleSearch _google;
        private BingSearch _bing;
        private SearchManager<SearchResponse> _searchManager;
        private FigthManager<FigthResponse> _figthManager;
        private Mock<ILogger<FigthManager<FigthResponse>>> _logger;

        [SetUp]
        public void Setup()
        {

            ConfigureSearchManager();
            ConfigureFigthManager();
        }

        private void ConfigureSearchManager()
        {

            _mockHttp = new MockHttpMessageHandler();
            _mockHttp.When(
                    "https://www.googleapis.com/customsearch/v1?key=AIzaSyCUiClkLL2F6ddVhN29Zr66uh0IHi04o9w&cx=6dc9392c98db553b6&q=net")
                .Respond("application/json",
                    "{\"searchInformation\":{\"searchTime\":0.562752,\"formattedSearchTime\":\"0.56\",\"totalResults\":\"20000\",\"formattedTotalResults\":\"13,400,000\"}}"); // Respond with JSON
            _mockHttp.When("https://api.bing.microsoft.com/v7.0/search?q=net")
                .Respond("application/json",
                    "{\"webPages\":{\"webSearchUrl\":\"https://www.bing.com/search?q=net\",\"totalEstimatedMatches\":30000}}"); // Respond with JSON

            _mockHttp.When(
                    "https://www.googleapis.com/customsearch/v1?key=AIzaSyCUiClkLL2F6ddVhN29Zr66uh0IHi04o9w&cx=6dc9392c98db553b6&q=java")
                .Respond("application/json",
                    "{\"searchInformation\":{\"searchTime\":0.562752,\"formattedSearchTime\":\"0.56\",\"totalResults\":\"100\",\"formattedTotalResults\":\"100\"}}"); // Respond with JSON
            _mockHttp.When("https://api.bing.microsoft.com/v7.0/search?q=java")
                .Respond("application/json",
                    "{\"webPages\":{\"webSearchUrl\":\"https://www.bing.com/search?q=java\",\"totalEstimatedMatches\":200}}"); // Respond with JSON

            HttpClient client = _mockHttp.ToHttpClient();
            _configuration = new Mock<IConfigurationRoot>();
            _configuration.SetupGet(x => x["Search:Google:UrlBase"]).Returns(
                "https://www.googleapis.com/customsearch/v1?key=AIzaSyCUiClkLL2F6ddVhN29Zr66uh0IHi04o9w&cx=6dc9392c98db553b6");
            _configuration.SetupGet(x => x["Search:Bing:UrlBase"])
                .Returns("https://api.bing.microsoft.com/v7.0/search");
            _google = new GoogleSearch(client, _configuration.Object);
            _bing = new BingSearch(client, _configuration.Object);
            List<ISearch<SearchResponse>> search = new List<ISearch<SearchResponse>> {_bing, _google};
            _searchManager = new SearchManager<SearchResponse>(search.AsEnumerable(), _configuration.Object);

        }

        private void ConfigureFigthManager()
        {

            _logger = new Mock<ILogger<FigthManager<FigthResponse>>>();
            _figthManager = new FigthManager<FigthResponse>(_searchManager, _logger.Object);

        }

        [Test]
        [TestCase(null)]
        public async Task FightManager_ThrowError_Null_SearchQuery(string[] args)
        {
            await _figthManager.FigthResultAsync(args);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Value cannot be null. (Parameter 'searchQuery')",
                        o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        [TestCaseSource("_unsoloElemento")]
        public async Task FightManager_ThrowError_Un_Solo_Competidor(string[] args)
        {
            await _figthManager.FigthResultAsync(args);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("se necesitan dos competidores para una batalla.",
                        o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);


        }
        [Test]
        [TestCaseSource("_batalla")]
        public async Task FightManager_OK(string[] args)
        {
           var response = await _figthManager.FigthResultAsync(args);
           Assert.AreEqual(response.Success , true);
        }
        [Test]
        [TestCaseSource("_unsoloElemento")]
        public async Task FightManager_Error(string[] args)
        {
            var response = await _figthManager.FigthResultAsync(args);
            Assert.AreEqual(response.Mensaje, "Ocurrio un error al procesar la(s) busquedas.");
        }
        static object[] _unsoloElemento =
        {
            new string[] { "net" }
      
        };
        static object[] _batalla =
        {
            new string[] { "net","java" }

        };



    }


}