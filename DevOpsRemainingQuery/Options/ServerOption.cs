using System;
using System.CommandLine;

using Microsoft.Extensions.Options;

namespace DevOpsRemainingQuery
{
    /// <summary>
    /// DevOps server URL. Implements the <see cref="Option{Int32}"/>
    /// </summary>
    /// <seealso cref="Option{Int32}"/>
    internal class ServerOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFileOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public ServerOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--server", "-s" },
                  () => defaultValues.Value.Server ?? throw new ApplicationException("The DevOps server URL is mandatory."),
                  "The DevOps Server URL.")
        {
        }
    }
}