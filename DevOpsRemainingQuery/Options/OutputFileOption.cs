namespace DevOpsRemainingQuery
{
    using System.CommandLine;

    using Microsoft.Extensions.Options;

    /// <summary>
    /// Output directory Option. Implements the <see cref="Option{String}"/>.
    /// </summary>
    /// <seealso cref="Option{String}"/>
    internal class OutputFileOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFileOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public OutputFileOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--output-file", "-o" },
                  () => defaultValues.Value.OutputFile ?? "DevOpsRemaining.csv",
                  "The output file name.")
        {
        }
    }
}