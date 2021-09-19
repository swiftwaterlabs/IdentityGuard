﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using IdentityGuard.Api.Tests.TestUtility.TestContexts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityGuard.Api.Tests.TestUtility.Fakes
{
    public class FunctionContextFake:FunctionContext
    {
        private readonly TestContext _context;

        public FunctionContextFake(TestContext context)
        {
            _context = context;
            var services = new ServiceCollection();

            services.AddFunctionsWorkerCore();
            services.AddFunctionsWorkerDefaults();

            InstanceServices = services.BuildServiceProvider();

        }
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
        private readonly TestContext _context;

        public HttpRequestDataFake(FunctionContext functionContext, HttpMethod method, TestContext context) : base(functionContext)
        {
            _functionContext = functionContext;
            _method = method.ToString();
            _context = context;
        }

        public override HttpResponseData CreateResponse()
        {
            return new HttpResponseDataFake(_functionContext);
        }

        public override Stream Body { get; } = new MemoryStream();
        public override HttpHeadersCollection Headers { get; } = new HttpHeadersCollection();
        public override IReadOnlyCollection<IHttpCookie> Cookies { get; } = new List<IHttpCookie>();
        public override Uri Url { get; }
        public override IEnumerable<ClaimsIdentity> Identities => new[] {_context.Identity.AuthenticatedUser};
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