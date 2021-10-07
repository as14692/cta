﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CTA.Rules.Models;

namespace CTA.Rules.Actions
{
    public static class ServerConfigTemplates
    {
        // Middleware component registered should be injected in this Order, Add new attribute at the correponding position

        internal static List<string> ConfigAttributes = new List<string>
        {
            "HttpCompression",
            "HttpRedirect",
            "Handlers",
            "Modules",
            "Security"
        };

        internal static List<string> CompresionTypes = new List<string>
        {
            "staticTypes",
            "dynamicTypes"
        };

        internal static Dictionary<string, int> HttpResponseStatus = new Dictionary<string, int>()
        {
            {"Found", 302},
            {"Permanent", 301 },
            {"Temporary", 307 },
            {"PermRedirect", 308 }
        };

        internal const string ConfigureServicesMethod = "ConfigureServices";
        internal const string ConfigureMiddlewareMethod = "Configure";
        internal const string ConfigureHostBuilderMethod = "CreateHostBuilder";
        internal const string LambdaWebBuilderAttribute = "webBuilder";

        internal static string kestrelTemplate = @"webBuilder.UseKestrel(options =>{kestrel_options});";
        internal static string addRedirectTemplate = @".AddRedirect(@""{0}"", ""{1}"", {2})";


        internal static List<string> MiddlewareConfigMethods = new List<string>()
        {
            ConfigureServicesMethod,
            ConfigureMiddlewareMethod
        };

        internal static Dictionary<string, string> ConfigurationExpressionTemplates = new Dictionary<string, string>()
        {
            {WebServerConfigAttributes.HttpCompression.ToString(), "app.UseResponseCaching();"},
            {WebServerConfigAttributes.Authentication.ToString(), "app.UseAuthentication();"},
            {WebServerConfigAttributes.Authorization.ToString(), "app.UseAuthorization();" },
            {WebServerConfigAttributes.Modules.ToString(),"app.UseMiddleware<{0}>();"},
            {WebServerConfigAttributes.Handlers.ToString(), @"app.MapWhen(context => context.Request.Path.ToString().EndsWith(""{0}""),appBranch => {{ appBranch.UseMiddleware<{1}>();}});" },
            {WebServerConfigAttributes.HttpRedirect.ToString(),"app.UseRewriter({0});"},
        };

        internal static Dictionary<string, string> ServiceExpressionTemplates = new Dictionary<string, string>()
        {
            {WebServerConfigAttributes.HttpCompression.ToString(), @"services.AddResponseCompression(options =>{options.Providers.Add<BrotliCompressionProvider>();options.Providers.Add<GzipCompressionProvider>(); options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { mime_types });}); "},
            {WebServerConfigAttributes.Authorization.ToString(), @"services.AddAuthorization(options =>{options.FallbackPolicy = options.DefaultPolicy;});"},
            {WebServerConfigAttributes.WindowsAuthentication.ToString(), @"services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate(options =>{options.Events = new NegotiateEvents(){OnAuthenticationFailed = context => { return Task.CompletedTask}};});" }
        };

        internal static Dictionary<string, string> KestrelOptionsTemplates = new Dictionary<string, string>()
        {
            {WebServerConfigAttributes.RequestLimits.ToString(), @"options.Limits.MaxRequestBodySize = {0};"},
        };

        internal static Dictionary<string, List<string>> Directives = new Dictionary<string, List<string>>()
        {
            {WebServerConfigAttributes.WindowsAuthentication.ToString(),
                                        new List<string>(){"Microsoft.AspNetCore.Authentication.Negotiate"}},
            {WebServerConfigAttributes.HttpCompression.ToString(),
                                        new List<string>(){"Microsoft.AspNetCore.ResponseCompression"}},
            {WebServerConfigAttributes.HttpRedirect.ToString(),
                                        new List<string>{ "Microsoft.AspNetCore.Rewrite"}}
        };

        internal static bool HasAny(this IEnumerable<XElement> element)
        {
            return element != null && element.Any();
        }

        // these handlers are specific to system.web, which is not supported in .net core
        internal static HashSet<string> UnsupportedHandlers = new HashSet<string>
        {
            "System.Web.Handlers.PrecompHandler",
            "System.Web.Handlers.AssemblyResourceLoader",
            "System.Web.Handlers.TraceHandler",
            "System.Web.Handlers.TransferRequestHandler",
            "System.Web.Handlers.WebPartExportHandler"

        };
    }
}
