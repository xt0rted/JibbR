using System.Collections.Generic;
using System.IO;

namespace JibbR.Routing
{
    public interface IRequest
    {
        // properties
        Stream Body { get; set; }
        string ContentType { get; }
        IDictionary<string, object> Dictionary { get; }
        bool HasFormData { get; }
        bool HasParseableData { get; }
        IDictionary<string, string[]> Headers { get; set; }
        string Host { get; set; }
        string MediaType { get; }
        string Method { get; set; }
        string Path { get; set; }
        string PathBase { get; set; }
        string Protocol { get; set; }
        string QueryString { get; set; }
        string Scheme { get; set; }

        // methods
        string GetHeader(string key);
        IDictionary<string, string> ReadForm();
    }
}