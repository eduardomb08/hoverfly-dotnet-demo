using Hoverfly.Core.Configuration;
using Hoverfly.Core.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using static Hoverfly.Core.Dsl.DslSimulationSource;
using static Hoverfly.Core.Dsl.HoverflyDsl;
using static Hoverfly.Core.Dsl.ResponseCreators;

namespace hoverfly_dotnet_demo.Tests
{
    [TestClass()]
    public class AzureMapsGeocodeProviderTests
    {
        private static readonly string _simulationFileName = "simulation.json";
        private Hoverfly.Core.Hoverfly _hoverfly;
        
        [TestInitialize]
        public void Initialize()
        {
            _hoverfly = HoverflySetup(Hoverfly.Core.HoverflyMode.Simulate);
            _hoverfly.Start();

            //var simulationPath = GetSimulationFilePath();
            //_hoverfly.ImportSimulation(new FileSimulationSource(simulationPath));
        }

        private static string GetSimulationFilePath()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var simulationPath = Path.Combine(assemblyPath, _simulationFileName);
            return simulationPath;
        }

        private Hoverfly.Core.Hoverfly HoverflySetup(Hoverfly.Core.HoverflyMode hoverflyMode)
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var hoverflyPath = Path.Combine(assemblyPath, @"Hoverfly");

            var cfg = HoverflyConfig.Config();
            cfg = cfg.SetHoverflyBasePath(hoverflyPath);

            var hoverfly = new Hoverfly.Core.Hoverfly(hoverflyMode, cfg);
            return hoverfly;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _hoverfly.Stop();
        }

        [TestMethod()]
        public void AzureMapsGeocodeProviderTest()
        {
            
        }

        public void GeocodeAddressAsyncTest_GenerateSimulationFile(string simulationPath)
        {
            var hoverfly = HoverflySetup(Hoverfly.Core.HoverflyMode.Capture);

            hoverfly.Start();
            try
            {
                // use for example HttpClient to make a call to a URL, e.g. "http://echo.jsontest.com/key/value/one/two");
                var httpClient = new HttpClient();
                var geo = new AzureMapsGeocodeProvider(httpClient, string.Empty);
                var str = geo.GeocodeAddressAsync("4725 Piedmont Row Dr, Charlotte, NC")
                    .GetAwaiter()
                    .GetResult();

                hoverfly.ExportSimulation(new FileSimulationSource(simulationPath));
            }
            finally
            {
                hoverfly.Stop();
            }
        }

        [TestMethod()]
        public void GeocodeAddressAsyncTest()
        {
            var responseJson = "{\"summary\":{\"query\":\"4725 piedmont row dr charlotte nc\",\"queryType\":\"NON_NEAR\",\"queryTime\":70,\"numResults\":3,\"offset\":0,\"totalResults\":3,\"fuzzyLevel\":1},\"results\":[{\"type\":\"Point Address\",\"id\":\"US/PAD/p0/51820381\",\"score\":12.07795,\"address\":{\"streetNumber\":\"4725\",\"streetName\":\"Piedmont Row Drive\",\"municipalitySubdivision\":\"Charlotte, Barclay Downs\",\"municipality\":\"Charlotte\",\"countrySecondarySubdivision\":\"Mecklenburg\",\"countryTertiarySubdivision\":\"Township 1 Charlotte\",\"countrySubdivision\":\"NC\",\"postalCode\":\"28210\",\"extendedPostalCode\":\"282104241\",\"countryCode\":\"US\",\"country\":\"United States\",\"countryCodeISO3\":\"USA\",\"freeformAddress\":\"4725 Piedmont Row Drive, Charlotte, NC 28210\",\"localName\":\"Charlotte\",\"countrySubdivisionName\":\"North Carolina\"},\"position\":{\"lat\":35.15154,\"lon\":-80.83936},\"viewport\":{\"topLeftPoint\":{\"lat\":35.15244,\"lon\":-80.84046},\"btmRightPoint\":{\"lat\":35.15064,\"lon\":-80.83826}},\"entryPoints\":[{\"type\":\"main\",\"position\":{\"lat\":35.15161,\"lon\":-80.83976}}]},{\"type\":\"Street\",\"id\":\"US/STR/p0/7219964\",\"score\":10.34569,\"address\":{\"streetName\":\"Piedmont Row Drive\",\"municipalitySubdivision\":\"Charlotte, Barclay Downs\",\"municipality\":\"Charlotte\",\"countrySecondarySubdivision\":\"Mecklenburg\",\"countryTertiarySubdivision\":\"Township 1 Charlotte\",\"countrySubdivision\":\"NC\",\"postalCode\":\"28209,28210\",\"extendedPostalCode\":\"2821026,2821042,282102600,282102601,282102602,282102604,282102605,282102606,282102607,282102608,282102609,282102610,282102611,282102612,282102614,282102615,282102616,282102617,282102618,282102619,282102620,282104237,282104238,282104239,282104240,282104241,282104253,282104268,282104269,282104270,282104272,282104275,282104276,282104279,282104280,282104281,282104282,282104283,282104284,282104285,282104286,282104287,282104288,282104289,282104290,282104291,282104292,282104293,282104294,282104295,282104296,282104297,282104298,282104299,282104453,282104454,282104661,282104663,282104664,282104676,282105255,282105287,282105288,282105289\",\"countryCode\":\"US\",\"country\":\"United States\",\"countryCodeISO3\":\"USA\",\"freeformAddress\":\"Piedmont Row Drive, Charlotte, NC\",\"countrySubdivisionName\":\"North Carolina\"},\"position\":{\"lat\":35.15229,\"lon\":-80.8398},\"viewport\":{\"topLeftPoint\":{\"lat\":35.1537,\"lon\":-80.84005},\"btmRightPoint\":{\"lat\":35.15113,\"lon\":-80.83959}}},{\"type\":\"Cross Street\",\"id\":\"US/XSTR/p0/5052116\",\"score\":8.45034,\"address\":{\"streetName\":\"Piedmont Row Drive, J A. Jones Drive & South Executive Park Drive\",\"municipalitySubdivision\":\"Charlotte\",\"municipality\":\"Charlotte\",\"countrySecondarySubdivision\":\"Mecklenburg\",\"countryTertiarySubdivision\":\"Township 1 Charlotte\",\"countrySubdivision\":\"NC\",\"postalCode\":\"28210\",\"countryCode\":\"US\",\"country\":\"United States\",\"countryCodeISO3\":\"USA\",\"freeformAddress\":\"Piedmont Row Drive & South Executive Park Drive, Charlotte, NC 28210\",\"countrySubdivisionName\":\"North Carolina\"},\"position\":{\"lat\":35.14929,\"lon\":-80.83978},\"viewport\":{\"topLeftPoint\":{\"lat\":35.15019,\"lon\":-80.84088},\"btmRightPoint\":{\"lat\":35.14839,\"lon\":-80.83868}}}]}";
            _hoverfly.ImportSimulation(Dsl(
                Service(AzureMapsGeocodeProvider.SearchUriBase)
                    .Get(AzureMapsGeocodeProvider.SearchUriPath)
                    .AnyQueryParams()
                    .WillReturn(Success(responseJson, "application/json"))));

            // use for example HttpClient to make a call to a URL, e.g. "http://echo.jsontest.com/key/value/one/two");
            var httpClient = new HttpClient();
            var geo = new AzureMapsGeocodeProvider(httpClient, string.Empty);
            var str = geo.GeocodeAddressAsync("4725 Piedmont Row Dr, Charlotte, NC")
                .GetAwaiter()
                .GetResult();

            var expectedAddress = "4725 Piedmont Row Drive, Charlotte, NC 28210";
            Assert.AreEqual(str, expectedAddress);
        }

        private string GetFirstResponseFromSimulationFile(string simulationPath)
        {
            var reader = new JsonTextReader(new StreamReader(simulationPath));
            var responses = JObject.LoadAsync(reader)
                .GetAwaiter()
                .GetResult();

            var data = responses["data"]["pairs"][0]["response"]["body"].Value<string>();
            return data;
        }

        private string GetAddressFromResponse(string responseJson)
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(responseJson);
            var address = jobj["results"]
                .OrderByDescending(r => r["score"].Value<double>())
                .FirstOrDefault()
                ["address"]["freeformAddress"]
                .Value<string>();

            return address;
        }
    }
}