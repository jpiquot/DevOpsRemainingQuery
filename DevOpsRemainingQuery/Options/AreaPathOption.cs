namespace DevOpsRemainingQuery
{
    using System.CommandLine;

    using Microsoft.Extensions.Options;

    /// <summary>
    /// Area path Option. Implements the <see cref="Option{Int32}"/>.
    /// </summary>
    /// <seealso cref="Option{Int32}"/>
    internal class AreaPathOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaPathOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public AreaPathOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--area-path", "-a" },
                  () => defaultValues.Value.AreaPath ?? string.Empty,
                  "Filter on the defined area path.")
        {
        }
    }
}