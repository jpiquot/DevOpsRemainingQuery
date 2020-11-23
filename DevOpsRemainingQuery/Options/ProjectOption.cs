using System;
using System.CommandLine;

using Microsoft.Extensions.Options;

namespace DevOpsRemainingQuery
{
    /// <summary>
    /// DevOps project name. Implements the <see cref="Option{Int32}"/>
    /// </summary>
    /// <seealso cref="Option{Int32}"/>
    internal class ProjectOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFileOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public ProjectOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--project", "-p" },
                  () => defaultValues.Value.Project ?? throw new ApplicationException("The DevOps project name is mandatory."),
                  "The DevOps project name.")
        {
        }
    }
}