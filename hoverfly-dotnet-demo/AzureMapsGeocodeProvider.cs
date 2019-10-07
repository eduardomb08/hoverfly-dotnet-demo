using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace hoverfly_dotnet_demo
{
    public class AzureMapsGeocodeProvider
    {
        public static readonly string SearchUriBase = "https://atlas.microsoft.com";
        public static readonly string SearchUriPath = "/search/address/json";

        private readonly HttpClient _client;
        private readonly string _mapsKey;

        public AzureMapsGeocodeProvider(HttpClient client, string serviceKey)
        {
            _client = client;
            _mapsKey = string.IsNullOrWhiteSpace(serviceKey) ? "NNqU_UWei9kJEXnBKpyHZHaA0IZ7gThDH0_sJ2B7ezM" : serviceKey;
        }

        public async Task<string> GeocodeAddressAsync(string address)
        {
            // defaulting to US only for now to limit search radius
            var uri = $"{SearchUriBase}{SearchUriPath}?subscription-key={_mapsKey}&api-version=1.0&query={address}&countrySet=US";

            var response = await _client.GetAsync(uri);

            if (!response.IsSuccessStatusCode) return string.Empty;

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                if (stream == null || stream.CanRead == false) return string.Empty;

                using (var sr = new StreamReader(stream))
                {
                    var jtr = await sr.ReadToEndAsync();
                    var searchResult = AzureMaps.SearchAddressResponse.FromJson(jtr);
                    var result = searchResult.Results
                        .OrderByDescending(r => r.Score)
                        .FirstOrDefault()?.Address.FreeformAddress ?? string.Empty;
                    return result;
                }
            }
        }
    }
}
