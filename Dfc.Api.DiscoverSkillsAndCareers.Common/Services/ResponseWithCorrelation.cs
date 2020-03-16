﻿using DFC.Api.DiscoverSkillsAndCareers.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace DFC.Api.DiscoverSkillsAndCareers.Common.Services
{
    public class ResponseWithCorrelation : IResponseWithCorrelation
    {
        private readonly ICorrelationIdProvider correlationIdProvider;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ResponseWithCorrelation(ICorrelationIdProvider correlationIdProvider, IHttpContextAccessor httpContextAccessor)
        {
            this.correlationIdProvider = correlationIdProvider;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IActionResult ResponseWithCorrelationId(HttpStatusCode statusCode)
        {
            AddCorrelationId();
            return new StatusCodeResult((int)statusCode);
        }

        public IActionResult ResponseObjectWithCorrelationId(object value)
        {
            var settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
            var orderedModel = JsonConvert.SerializeObject(value, Formatting.Indented, settings);

            AddCorrelationId();

            return new OkObjectResult(JsonConvert.DeserializeObject(orderedModel));
        }

        private void AddCorrelationId()
        {
            httpContextAccessor.HttpContext.Response.Headers.Add(HeaderName.CorrelationId, correlationIdProvider.CorrelationId);
        }
    }
}