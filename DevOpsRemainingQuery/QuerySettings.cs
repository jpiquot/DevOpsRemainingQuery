#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;

namespace DevOpsRemainingQuery
{
    internal class QuerySettings
    {
        public string? OutputFile { get; set; }
        public string? AreaPath { get; set; }
        public string? IterationPath { get; set; }
        public string? Server { get; set; }
        public string? Project { get; set; }
        public string? PersonalAccessToken { get; set; }
        public string? Query { get; set; }
    }
}