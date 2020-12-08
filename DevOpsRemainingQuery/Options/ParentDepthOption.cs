namespace DevOpsRemainingQuery
{
    using System.CommandLine;

    using Microsoft.Extensions.Options;

    /// <summary>
    /// The maximum parent hierarchy depth Option. Implements the <see cref="Option{Int32}"/>.
    /// </summary>
    /// <seealso cref="Option{Int32}"/>
    internal class ParentDepthOption : Option<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParentDepthOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public ParentDepthOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--parent-depth", "-pd" },
                  () => (defaultValues.Value.ParentDepth == null || defaultValues.Value.ParentDepth == 0) ? 3 : defaultValues.Value.ParentDepth.Value,
                  "The maximum parent hierarchy depth.")
        {
        }
    }
}