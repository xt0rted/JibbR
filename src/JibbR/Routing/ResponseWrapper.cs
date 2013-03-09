using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Gate;

using JibbR.Annotations;

namespace JibbR.Routing
{
    public class ResponseWrapper : IResponse
    {
        private Response _response;

        public ResponseWrapper(IDictionary<string, object> env)
            : this(new Response(env))
        {
        }

        public ResponseWrapper(Response response)
        {
            _response = response;
        }

        public Stream Body
        {
            get { return _response.Body; }
            set { _response.Body = value; }
        }

        public long ContentLength
        {
            get { return _response.ContentLength; }
            set { _response.ContentLength = value; }
        }

        public string ContentType
        {
            get { return _response.ContentType; }
            set { _response.ContentType = value; }
        }

        public Encoding Encoding
        {
            get { return _response.Encoding; }
            set { _response.Encoding = value; }
        }

        public IDictionary<string, object> Environment
        {
            get { return _response.Environment; }
            set { _response.Environment = value; }
        }

        public IDictionary<string, string[]> Headers
        {
            get { return _response.Headers; }
            set { _response.Headers = value; }
        }

        public Stream OutputStream
        {
            get { return _response.OutputStream; }
            set { _response.OutputStream = value; }
        }

        public string Status
        {
            get { return _response.Status; }
            set { _response.Status = value; }
        }

        public int StatusCode
        {
            get { return _response.StatusCode; }
            set { _response.StatusCode = value; }
        }

        public IResponse SetCookie(string key, string value)
        {
            _response.SetCookie(key, value);
            return this;
        }

        public IResponse SetHeader(string name, string value)
        {
            _response.SetHeader(name, value);
            return this;
        }

        public void Write(ArraySegment<byte> data)
        {
            _response.Write(data);
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

        [StringFormatMethod("format")]
        public void Write(string format, params object[] args)
        {
            _response.Write(format, args);
        }
    }
}