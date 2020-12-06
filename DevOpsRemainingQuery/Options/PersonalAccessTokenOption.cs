namespace DevOpsRemainingQuery
{
    using System.CommandLine;

    using Microsoft.Extensions.Options;

    /// <summary>
    /// Personal access token. Implements the <see cref="Option{Int32}"/>.
    /// </summary>
    /// <seealso cref="Option{Int32}"/>
    internal class PersonalAccessTokenOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalAccessTokenOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public PersonalAccessTokenOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--personal-access-token", "-pat" },
                  () => defaultValues.Value.PersonalAccessToken ?? string.Empty,
                  "The personal access token to use for authetication. If not set, Windows authetication will be used.")
        {
        }
    }
}