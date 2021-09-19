using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Tests.TestUtility.Fakes
{
    public class FunctionContextFake:FunctionContext
    {
        public override string InvocationId => Guid.NewGuid().ToString();
        public override string FunctionId => Guid.NewGuid().ToString();
        public override TraceContext TraceContext { get; }
        public override BindingContext BindingContext { get; }
        public override IServiceProvider InstanceServices { get; set; }
        public override FunctionDefinition FunctionDefinition { get; }
        public override IDictionary<object, object> Items { get; set; }
        public override IInvocationFeatures Features { get; }
    }

    public class HttpRequestDataFake : HttpRequestData
    {
        private readonly FunctionContext _functionContext;
        private readonly string _method;

        public HttpRequestDataFake(FunctionContext functionContext, string method) : base(functionContext)
        {
            _functionContext = functionContext;
            _method = method;
        }

        public override HttpResponseData CreateResponse()
        {
            return new HttpResponseDataFake(_functionContext);
        }

        public override Stream Body { get; } = new MemoryStream();
        public override HttpHeadersCollection Headers { get; } = new HttpHeadersCollection();
        public override IReadOnlyCollection<IHttpCookie> Cookies { get; } = new List<IHttpCookie>();
        public override Uri Url { get; }
        public override IEnumerable<ClaimsIdentity> Identities { get; }
        public override string Method => _method;

    }

    public class HttpResponseDataFake : HttpResponseData
    {
        public HttpResponseDataFake(FunctionContext functionContext) : base(functionContext)
        {
        }

        public override HttpStatusCode StatusCode { get; set; }
        public override HttpHeadersCollection Headers { get; set; } = new HttpHeadersCollection();
        public override Stream Body { get; set; } = new MemoryStream();
        public override HttpCookies Cookies { get; } 
    }
}