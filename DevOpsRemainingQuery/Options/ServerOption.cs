namespace DevOpsRemainingQuery
{
    using System;
    using System.CommandLine;

    using Microsoft.Extensions.Options;

    /// <summary>
    /// DevOps server URL. Implements the <see cref="Option{Int32}"/>.
    /// </summary>
    /// <seealso cref="Option{Int32}"/>
    internal class ServerOption : Option<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public ServerOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--server", "-s" },
                  () => string.IsNullOrWhiteSpace(defaultValues.Value.Server) ? throw new ArgumentException("The DevOps server URL is mandatory.", nameof(defaultValues)) : defaultValues.Value.Server,
                  "The DevOps Server URL.")
        {
        }
    }
}