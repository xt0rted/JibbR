using System.Collections.Generic;
using System.IO;

using Gate;

using JibbR.Routing;

namespace JibbR.Web.Routing
{
    public class RequestWrapper : IRequest
    {
        private Request _request;

        public RequestWrapper(IDictionary<string, object> env)
            : this(new Request(env))
        {
        }

        public RequestWrapper(Request request)
        {
            _request = request;
        }

        public Stream Body
        {
            get { return _request.Body; }
            set { _request.Body = value; }
        }

        public string ContentType
        {
            get { return _request.ContentType; }
        }

        public IDictionary<string, string> Cookies
        {
            get { return _request.Cookies; }
        }

        public IDictionary<string, object> Environment
        {
            get { return _request.Environment; }
            set { _request.Environment = value; }
        }

        public bool HasFormData
        {
            get { return _request.HasFormData; }
        }

        public bool HasParseableData
        {
            get { return _request.HasParseableData; }
        }

        public IDictionary<string, string[]> Headers
        {
            get { return _request.Headers; }
            set { _request.Headers = value; }
        }

        public string MediaType
        {
            get { return _request.MediaType; }
        }

        public string Method
        {
            get { return _request.Method; }
            set { _request.Method = value; }
        }

        public string Path
        {
            get { return _request.Path; }
            set { _request.Path = value; }
        }

        public IDictionary<string, string> Query
        {
            get { return _request.Query; }
        }

        public string QueryString
        {
            get { return _request.QueryString; }
            set { _request.QueryString = value; }
        }

        public IDictionary<string, string> ReadForm()
        {
            return _request.ReadForm();
        }
    }
}