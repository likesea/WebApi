﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Learning.Web.Controllers;
using Newtonsoft.Json.Serialization;

namespace Learning.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Services.Replace(typeof(IHttpControllerSelector), new LearningControllerSelector((config)));
            config.Routes.MapHttpRoute(
                name: "Courses",
                routeTemplate: "api/courses/{id}",
                defaults: new { controller = "courses", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
            name: "Students",
            routeTemplate: "api/students/{userName}",
            defaults: new { controller = "students", userName = RouteParameter.Optional }
            );
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

    }
}
