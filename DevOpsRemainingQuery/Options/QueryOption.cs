namespace DevOpsRemainingQuery
{
    using System;
    using System.CommandLine;

    using Microsoft.Extensions.Options;

    /// <summary>
    /// DevOps query name. Implements the <see cref="Option{Int32}"/>.
    /// </summary>
    /// <seealso cref="Option{Int32}"/>
    internal class QueryOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public QueryOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--query", "-q" },
                  () => string.IsNullOrWhiteSpace(defaultValues.Value.Query) ? throw new ArgumentException("The DevOps query name is mandatory.", nameof(defaultValues)) : defaultValues.Value.Query,
                  "The DevOps query name.")
        {
        }
    }
}