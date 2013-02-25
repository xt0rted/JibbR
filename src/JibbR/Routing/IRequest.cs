using System.Collections.Generic;
using System.IO;

namespace JibbR.Routing
{
    public interface IRequest
    {
        // properties
        Stream Body { get; set; }
        string ContentType { get; }
        IDictionary<string, string> Cookies { get; }
        IDictionary<string, object> Environment { get; set; }
        bool HasFormData { get; }
        bool HasParseableData { get; }
        IDictionary<string, string[]> Headers { get; set; }
        //string Host { get; set; }
        //string HostWithPort { get; set; }
        string MediaType { get; }
        string Method { get; set; }
        string Path { get; set; }
        //string PathBase { get; set; }
        //int Port { get; set; }
        //string Protocol { get; set; }
        IDictionary<string, string> Query { get; }
        string QueryString { get; set; }
        //string Scheme { get; set; }

        // methods
        IDictionary<string, string> ReadForm();
    }
}