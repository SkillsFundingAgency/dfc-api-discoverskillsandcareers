using DFC.Api.DiscoverSkillsAndCareers.Common.Constants;
using Microsoft.AspNetCore.Http;
using System;

namespace DFC.Api.DiscoverSkillsAndCareers.Common.Services
{
    public class RequestHeaderCorrelationIdProvider : ICorrelationIdProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public RequestHeaderCorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string CorrelationId => !string.IsNullOrWhiteSpace(httpContextAccessor.HttpContext.Request.Headers[HeaderName.RequestId].ToString())
            ? httpContextAccessor.HttpContext.Request.Headers[HeaderName.RequestId].ToString()
            : Guid.NewGuid().ToString();
    }
}