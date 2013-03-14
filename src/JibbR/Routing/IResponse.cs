using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JibbR.Routing
{
    public interface IResponse
    {
        // properties
        Stream Body { get; set; }
        string ContentType { get; set; }
        IDictionary<string, object> Dictionary { get; }
        IDictionary<string, string[]> Headers { get; set; }
        string ReasonPhrase { get; set; }
        int StatusCode { get; set; }

        // methods
        IResponse SetHeader(string name, string value);
        void Write(byte[] buffer);
        void Write(byte[] buffer, int offset, int count);
        void Write(string text);
        void Write(string text, Encoding encoding);
    }
}