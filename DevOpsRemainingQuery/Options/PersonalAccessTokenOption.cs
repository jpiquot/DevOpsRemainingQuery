using System;
using System.CommandLine;

using Microsoft.Extensions.Options;

namespace DevOpsRemainingQuery
{
    /// <summary>
    /// Personal access token. Implements the <see cref="Option{Int32}"/>
    /// </summary>
    /// <seealso cref="Option{Int32}"/>
    internal class PersonalAccessTokenOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFileOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public PersonalAccessTokenOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--pat"},
                  () => defaultValues.Value.PersonalAccessToken??"",
                  "The personal access token to use for authetication. If not set, Windows authetication will be used.")
        {
        }
    }
}