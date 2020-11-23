using System.Security.Policy;
#pragma warning disable CA1056 // URI-like properties should not be strings

namespace DevOpsRemainingQuery
{
    /// <summary>
    /// Query command options
    /// </summary>
    public class QueryCommandOptions
    {
        /// <summary>
        /// The output file path
        /// </summary>
        public string? OutputFile { get; set; }
        /// <summary>
        /// If defined, the query will be filtered by this area path
        /// </summary>
        public string? AreaPath { get; set; }
        /// <summary>
        /// If defined, the query will be filtered by this iteration path
        /// </summary>
        public string? IterationPath { get; set; }
        /// <summary>
        /// The URL of the DevOps server
        /// </summary>
        public string? Server { get; set; }
        /// <summary>
        /// The DevOps project name
        /// </summary>
        public string? Project { get; set; }
        /// <summary>
        /// The PAT to use for credentials. If not set, Windows authentication will be used.
        /// </summary>
        public string? PersonalAccessToken { get; set; }
        /// <summary>
        /// The DevOps query name
        /// </summary>
        public string? Query { get; set; }
    }
}