using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PicSayAndPlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PicSayAndPlay.Services
{
    public class TranslationService
    {
        private static string _dataMarketUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private static HttpClient _client = new HttpClient();

        public static async Task<List<Translation>> TranslateAsync(AnalysisResult result)
        {
            var originalWords = result.Tags?.Where(p => p.Confidence > 0.5).Select(p => p.Name).ToList();
            List<Translation> translations = new List<Translation>();
            var divider = '*';

            //  Concat list of words
            var text = originalWords?.Aggregate((i, j) => i + divider + j);

            string auth = await GetToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
            var requestUri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text="
                                            + System.Net.WebUtility.UrlEncode(text)
                                            + "&from=en&to=es";
            string strTransText = string.Empty;
            try
            {
                var strTranslated = await _client.GetStringAsync(requestUri);
                var xTranslation = XDocument.Parse(strTranslated);
                strTransText = xTranslation.Root?.FirstNode.ToString();

                //  Split list of words
                var translatedWords = strTransText.Split(divider).ToList();

                //  Fill Translation object's list
                for (int i = 0; i < originalWords.Count; i++)
                {
                    translations.Add(new Translation(originalWords[i], translatedWords[i].Trim(), DateTime.Now));
                }
            }
            catch (Exception)
            { }

            return translations;
        }

        private static async Task<string> GetToken()
        {
            var properties = new Dictionary<string, string>
            {
              { "grant_type", "client_credentials" },
              { "client_id",  "PicSayAndPlayClient"},
              { "client_secret", "aS3B9MWjv+rDo7P9nOCM9M3UwBWGs9+YG1z4sKpM0ew=" },
              { "scope", "http://api.microsofttranslator.com" }
            };

            var authentication = new FormUrlEncodedContent(properties);
            var dataMarketResponse = await _client.PostAsync(_dataMarketUri, authentication);
            string response;
            if (!dataMarketResponse.IsSuccessStatusCode)
            {
                response = await dataMarketResponse.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<JToken>(response);
                var err = error.Value<string>("error");
                var msg = error.Value<string>("error_description");
                throw new HttpRequestException($"Azure market place request failed: {err} {msg}");
            }
            response = await dataMarketResponse.Content.ReadAsStringAsync();
            var accessToken = JsonConvert.DeserializeObject<DataMarketAccessToken>(response);
            return accessToken.access_token;
        }
    }

    public class DataMarketAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
    }
}