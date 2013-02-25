using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using JibbR.Annotations;

namespace JibbR.Routing
{
    public interface IResponse
    {
        // properties
        Stream Body { get; set; }
        long ContentLength { get; set; }
        string ContentType { get; set; }
        Encoding Encoding { get; set; }
        IDictionary<string, object> Environment { get; set; }
        IDictionary<string, string[]> Headers { get; set; }
        Stream OutputStream { get; set; }
        string Status { get; set; }
        int StatusCode { get; set; }

        // methods
        IResponse SetCookie(string key, string value);
        IResponse SetHeader(string name, string value);
        void Write(ArraySegment<byte> data);
        void Write(byte[] buffer);
        void Write(byte[] buffer, int offset, int count);
        void Write(string text);
        [StringFormatMethod("format")]
        void Write(string format, params object[] args);
    }
}