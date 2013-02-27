using RestSharp;

namespace JibbR.Adapters.Bing
{
    public class BingClient : IBingClient
    {
        public string WebSearch(string term)
        {
            var client = new RestClient("http://www.bing.com");

            var action = new RestRequest("search", Method.GET);
            action.AddParameter("q", term);

            var result = client.Execute(action);

            return result.Content;
        }

        public string ImageSearch(string apiKey, string term, SafeSearch safeSearch)
        {
            var client = new RestClient("https://api.datamarket.azure.com/Bing/Search/v1/")
            {
                Authenticator = new HttpBasicAuthenticator(apiKey, apiKey)
            };

            var action = new RestRequest("Image", Method.GET);
            action.AddParameter("Query", "'" + term + "'")
                  .AddParameter("$format", "json")
                  .AddParameter("$top", "50")
                  .AddParameter("Adult", "'" + safeSearch + "'");

            var result = client.Execute(action);

            return result.Content;
        }
    }
}