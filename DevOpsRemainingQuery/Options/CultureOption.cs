namespace DevOpsRemainingQuery
{
    using System.CommandLine;

    using Microsoft.Extensions.Options;

    /// <summary>
    /// Area path Option. Implements the <see cref="Option{String}"/>.
    /// </summary>
    /// <seealso cref="Option{String}"/>
    internal class CultureOption : Option<string?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CultureOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public CultureOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--culture", "-c" },
                  () => defaultValues.Value.Culture,
                  "The culture name (en-US; en-GB, fr-FR and etc.).")
        {
        }
    }
}