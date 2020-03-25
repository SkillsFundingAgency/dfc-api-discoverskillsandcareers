using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace DFC.Api.DiscoverSkillsAndCareers.Common.Services
{
    public interface IResponseWithCorrelation
    {
        IActionResult ResponseWithCorrelationId(HttpStatusCode statusCode, Guid? correlationId = null);

        IActionResult ResponseObjectWithCorrelationId(object value, Guid? correlationId = null);
    }
}