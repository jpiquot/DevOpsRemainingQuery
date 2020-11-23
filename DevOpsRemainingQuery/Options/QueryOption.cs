using System;
using System.CommandLine;

using Microsoft.Extensions.Options;

namespace DevOpsRemainingQuery
{
    /// <summary>
    /// DevOps query name. Implements the <see cref="Option{Int32}"/>
    /// </summary>
    /// <seealso cref="Option{Int32}"/>
    internal class QueryOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFileOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public QueryOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--query", "-q" },
                  () => defaultValues.Value.Query ?? throw new ApplicationException("The DevOps query name is mandatory."),
                  "The DevOps query name.")
        {
        }
    }
}