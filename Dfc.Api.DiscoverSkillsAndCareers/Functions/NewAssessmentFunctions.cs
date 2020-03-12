﻿using Dfc.Session.Models;
using DFC.Api.DiscoverSkillsAndCareers.Common.Services;
using DFC.Api.DiscoverSkillsAndCareers.Extensions;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DFC.Api.DiscoverSkillsAndCareers.Functions
{
    public class NewAssessmentFunctions
    {
        private readonly ILogService logService;
        private readonly IResponseWithCorrelation responseWithCorrelation;

        public NewAssessmentFunctions(ILogService logService, IResponseWithCorrelation responseWithCorrelation)
        {
            this.logService = logService;
            this.responseWithCorrelation = responseWithCorrelation;
        }

        [Display(Name = "Get Assessment User Session", Description = "Get Assessment User Session - gets user session")]
        [FunctionName("GetUserSession")]
        [ProducesResponseType(typeof(DfcUserSession), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Retrieved User Session", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "SessionId does not exist", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = "Input validation failed", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is invalid.", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NotFound, Description = "Version header has invalid value, must be set to 'v1'.", ShowSchema = false)]
        [Response(HttpStatusCode = 429, Description = "Too many requests being sent, by default the API supports 150 per minute.", ShowSchema = false)]
        public IActionResult GetUserSession(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "assessment/session/{sessionId}")] HttpRequest request,
            string sessionId)
        {
            request.LogRequestHeaders(logService);

            return responseWithCorrelation.ResponseWithCorrelationId(HttpStatusCode.Created);
        }

        [Display(Name = "Post New Short Assessment", Description = "Post New Short Assessment. Creates a new user session")]
        [FunctionName("CreateNewShortAssessment")]
        [ProducesResponseType(typeof(DfcUserSession), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Created, Description = "Created User Session", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is invalid.", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NotFound, Description = "Version header has invalid value, must be set to 'v1'.", ShowSchema = false)]
        [Response(HttpStatusCode = 429, Description = "Too many requests being sent, by default the API supports 150 per minute.", ShowSchema = false)]
        public IActionResult CreateNewShortAssessment([HttpTrigger(AuthorizationLevel.Function, "post", Route = "assessment/short")] HttpRequest request)
        {
            request.LogRequestHeaders(logService);

            return responseWithCorrelation.ResponseWithCorrelationId(HttpStatusCode.Created);
        }

        [Display(Name = "Post New Skills Assessment", Description = "Post New Skills Assessment. Creates a new user session")]
        [FunctionName("CreateNewSkillsAssessment")]
        [PostRequestBody(typeof(DfcUserSession), "DFC User Session Object")]
        [Response(HttpStatusCode = (int)HttpStatusCode.Created, Description = "Created User Session", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.AlreadyReported, Description = "A User Session with same id already exist in the application.", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = "Input validation failed", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is invalid.", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NotFound, Description = "Version header has invalid value, must be set to 'v1'.", ShowSchema = false)]
        [Response(HttpStatusCode = 429, Description = "Too many requests being sent, by default the API supports 150 per minute.", ShowSchema = false)]
        public IActionResult CreateNewSkillsAssessment([HttpTrigger(AuthorizationLevel.Function, "post", Route = "assessment/skills")] HttpRequest request)
        {
            request.LogRequestHeaders(logService);

            return responseWithCorrelation.ResponseWithCorrelationId(HttpStatusCode.Created);
        }
    }
}