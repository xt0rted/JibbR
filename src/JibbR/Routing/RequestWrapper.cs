using System;
using System.Collections.Generic;
using System.IO;

using Owin.Types;

namespace JibbR.Routing
{
    public class RequestWrapper : IRequest
    {
        private static readonly char[] CommaSemicolon = new[] { ',', ';' };

        private OwinRequest _request;

        public RequestWrapper(IDictionary<string, object> env)
            : this(new OwinRequest(env))
        {
        }

        public RequestWrapper(OwinRequest request)
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
            get { return _request.GetHeader("Content-Type"); }
        }

        public IDictionary<string, object> Dictionary
        {
            get { return _request.Dictionary; }
        }

        public bool HasFormData
        {
            get
            {
                var mediaType = MediaType;
                return (Method == "POST" && string.IsNullOrEmpty(mediaType))
                    || mediaType == "application/x-www-form-urlencoded"
                        || mediaType == "multipart/form-data";
            }
        }

        public bool HasParseableData
        {
            get
            {
                var mediaType = MediaType;
                return mediaType == "application/x-www-form-urlencoded"
                    || mediaType == "multipart/form-data";
            }
        }

        public IDictionary<string, string[]> Headers
        {
            get { return _request.Headers; }
            set { _request.Headers = value; }
        }

        public string Host
        {
            get { return _request.Host; }
            set { _request.Host = value; }
        }

        public string MediaType
        {
            get
            {
                var contentType = ContentType;
                if (contentType == null)
                {
                    return null;
                }

                var delimiterPos = contentType.IndexOfAny(CommaSemicolon);
                if (delimiterPos < 0)
                {
                    return contentType;
                }

                return contentType.Substring(0, delimiterPos);
            }
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

        public string PathBase
        {
            get { return _request.PathBase; }
            set { _request.PathBase = value; }
        }

        public string Protocol
        {
            get { return _request.Protocol; }
            set { _request.Protocol = value; }
        }

        public string QueryString
        {
            get { return _request.QueryString; }
            set { _request.QueryString = value; }
        }

        public string Scheme
        {
            get { return _request.Scheme; }
            set { _request.Scheme = value; }
        }

        public string GetHeader(string key)
        {
            return _request.GetHeader(key);
        }

        public IDictionary<string, string> ReadForm()
        {
            string body;
            using (var sr = new StreamReader(Body))
            {
                body = sr.ReadToEnd();
            }

            var results = new Dictionary<string, string>();

            foreach (var segment in body.Split('&'))
            {
                var group = segment.Split('=');

                string key = Uri.UnescapeDataString(group[0]);
                string value = null;

                if (group.Length > 1)
                {
                    value = Uri.UnescapeDataString(group[1]);
                }

                results.Add(key, value);
            }

            return results;
        }
    }
}