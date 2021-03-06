﻿namespace DevOpsRemainingQuery
{
    using System.Collections.Generic;

    /// <summary>
    /// Query command options.
    /// </summary>
    public class QueryCommandOptions
    {
        /// <summary>
        /// Gets or sets the task activity order.
        /// </summary>
        public List<string>? ActivityOrder { get; set; }

        /// <summary>
        /// Gets or sets the area path. If defined, the query will be filtered by this area path.
        /// </summary>
        public string? AreaPath { get; set; }

        /// <summary>
        /// Gets or sets the iteration path. If defined, the query will be filtered by this
        /// iteration path.
        /// </summary>
        public string? IterationPath { get; set; }

        /// <summary>
        /// Gets or sets the output file path.
        /// </summary>
        public string? OutputFile { get; set; }

        /// <summary>
        /// Gets or sets the maximum parent hierarchy depth.
        /// </summary>
        /// <value>The maximum parent hierarchy depth.</value>
        public int? ParentDepth { get; set; }

        /// <summary>
        /// Gets or sets the PAT to use for credentials. If not set, Windows authentication will be used.
        /// </summary>
        public string? PersonalAccessToken { get; set; }

        /// <summary>
        /// Gets or sets the DevOps project name.
        /// </summary>
        public string? Project { get; set; }

        /// <summary>
        /// Gets or sets the DevOps query name.
        /// </summary>
        public string? Query { get; set; }

        /// <summary>
        /// Gets or sets the URL of the DevOps server.
        /// </summary>
        public string? Server { get; set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        public string? Culture { get; set; }

    }
}