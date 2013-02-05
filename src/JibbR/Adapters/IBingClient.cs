namespace JibbR.Adapters
{
    public interface IBingClient
    {
        string WebSearch(string term);
        string ImageSearch(string apiKey, string term);
    }
}