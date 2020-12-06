namespace DevOpsRemainingQuery
{
    using System;
    using System.CommandLine;

    using Microsoft.Extensions.Options;

    /// <summary>
    /// DevOps project name. Implements the <see cref="Option{Int32}"/>.
    /// </summary>
    /// <seealso cref="Option{Int32}"/>
    internal class ProjectOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public ProjectOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--project", "-p" },
                  () => string.IsNullOrWhiteSpace(defaultValues.Value.Project) ? throw new ArgumentException("The DevOps project name is mandatory.", nameof(defaultValues)) : defaultValues.Value.Project,
                  "The DevOps project name.")
        {
        }
    }
}