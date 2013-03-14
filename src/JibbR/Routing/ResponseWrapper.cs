using System.Collections.Generic;
using System.IO;
using System.Text;

using Owin.Types;

namespace JibbR.Routing
{
    public class ResponseWrapper : IResponse
    {
        private OwinResponse _response;

        public ResponseWrapper(IDictionary<string, object> env)
            : this(new OwinResponse(env))
        {
        }

        public ResponseWrapper(OwinResponse response)
        {
            _response = response;
        }

        public Stream Body
        {
            get { return _response.Body; }
            set { _response.Body = value; }
        }

        public string ContentType
        {
            get { return _response.ContentType; }
            set { _response.ContentType = value; }
        }

        public IDictionary<string, object> Dictionary
        {
            get { return _response.Dictionary; }
        }

        public IDictionary<string, string[]> Headers
        {
            get { return _response.Headers; }
            set { _response.Headers = value; }
        }

        public string ReasonPhrase
        {
            get { return _response.ReasonPhrase; }
            set { _response.ReasonPhrase = value; }
        }

        public int StatusCode
        {
            get { return _response.StatusCode; }
            set { _response.StatusCode = value; }
        }

        public IResponse SetHeader(string name, string value)
        {
            _response.SetHeader(name, value);
            return this;
        }

        public void Write(byte[] buffer)
        {
            _response.Write(buffer);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _response.Write(buffer, offset, count);
        }

        public void Write(string text)
        {
            _response.Write(text);
        }

        public void Write(string text, Encoding encoding)
        {
            _response.Write(text, encoding);
        }
    }
}