using Infrastructure.Test.Unit.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Infrastructure.Test.Unit.Builders
{
    public class HttpClientBuilder
    {
        private readonly string _baseUrl;
        private readonly Dictionary<string, HttpResponseMessage> _responses = new();

        public HttpClientBuilder(string baseUrl = "http://test.com")
        {
            _baseUrl = baseUrl;
        }

        public HttpClientBuilder AddResponse(string url, HttpStatusCode httpStatusCode, object content = null)
        {
            return AddResponse(url, httpStatusCode, JsonConvert.SerializeObject(content));
        }

        public HttpClientBuilder AddResponse(string url, HttpStatusCode httpStatusCode, string json)
        {
            var responseMessage = new HttpResponseMessage(httpStatusCode)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            _responses.Add($"{_baseUrl}{url}", responseMessage);

            return this;
        }

        public HttpClient Build()
        {
            var httpMessageHandlerStub = new HttpMessageHandlerStub
            {
                Responses = _responses
            };

            return new HttpClient(httpMessageHandlerStub)
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }
    }
}
