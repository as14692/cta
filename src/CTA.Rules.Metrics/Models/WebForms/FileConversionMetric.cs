﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CTA.Rules.Metrics.Models.WebForms
{
    public class FileConversionMetric : WebFormsActionMetric
    {
        [JsonProperty("actionName", Order = 11)]
        public string ActionName => "FileConversion";

        public FileConversionMetric(MetricsContext context, string childActionName, string projectPath)
        {
            ChildActionName = childActionName;
            SolutionPathHash = context.SolutionPathHash;
            ProjectGuid = context.ProjectGuidMap.GetValueOrDefault(projectPath, "N/A");
        }
    }
}
