using System.CommandLine;

using Microsoft.Extensions.Options;

namespace DevOpsRemainingQuery
{
    /// <summary>
    /// Iteration path Option. Implements the <see cref="Option{Int32}"/>
    /// </summary>
    /// <seealso cref="Option{Int32}"/>
    internal class IterationPathOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IterationPathOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public IterationPathOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--iteration-path", "-i" },
                  () => defaultValues.Value.IterationPath ?? string.Empty,
                  "Filter on the defined iteration path.")
        {
        }
    }
}