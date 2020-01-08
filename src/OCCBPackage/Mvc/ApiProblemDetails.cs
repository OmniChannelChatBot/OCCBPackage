﻿using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OCCBPackage.Mvc
{
    /// <summary>
    /// https://tools.ietf.org/html/rfc7231
    /// </summary>
    public class ApiProblemDetails : ProblemDetails
    {
        public ApiProblemDetails(HttpContext context, ModelStateDictionary modelState)
            : this(context) =>
            Errors = modelState
                .Where(w => !string.IsNullOrEmpty(w.Key))
                .ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value.Errors.Select(n => !string.IsNullOrEmpty(n.ErrorMessage) ? n.ErrorMessage : n.Exception.Message));

        public ApiProblemDetails(HttpContext context, IList<ValidationFailure> errors)
           : this(context) =>
            Errors = errors
                .GroupBy(g => g.PropertyName)
                .ToDictionary(n => n.Key, n => n.Select(s => s.ErrorMessage));


        public ApiProblemDetails(HttpContext context, Exception exception)
              : this(context)
        {
            Detail = exception.Message;
            Extensions.Add("source", exception.Source);

            if (exception?.InnerException?.Message != default(string))
            {
                Extensions.Add("innerDetail", exception.InnerException.Message);
            }

            if (exception.Data != null && exception.Data[0] != null)
            {
                Extensions.Add("data", exception.Data[0]);
            }
        }

        public ApiProblemDetails(HttpContext context, string title = default)
        {
            if (Constants.ProblemTypes.TryGetValue(context.Response.StatusCode, out var problemType))
            {
                Type = problemType.Item2;
            }

            Title = title ?? Constants.ProblemTypes[context.Response.StatusCode].Item1;
            TraceId = context.TraceIdentifier;
            Status = context.Response.StatusCode;
            Instance = Environments.GetServiceName();
        }

        public string TraceId { get; }

        public IDictionary<string, IEnumerable<string>> Errors { get; }
    }
}
