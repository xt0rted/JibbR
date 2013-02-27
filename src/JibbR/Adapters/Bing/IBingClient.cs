namespace JibbR.Adapters.Bing
{
    public interface IBingClient
    {
        string WebSearch(string term);
        string ImageSearch(string apiKey, string term, SafeSearch safeSearch);
    }
}