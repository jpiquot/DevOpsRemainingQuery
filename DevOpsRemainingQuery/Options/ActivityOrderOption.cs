namespace DevOpsRemainingQuery
{
    using System.Collections.Generic;
    using System.CommandLine;

    using Microsoft.Extensions.Options;

    /// <summary>
    /// DevOps task activity order. Implements the <see cref="Option"/>.
    /// </summary>
    /// <seealso cref="Option"/>
    internal class ActivityOrderOption : Option<List<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityOrderOption"/> class.
        /// </summary>
        /// <param name="defaultValues">The default values.</param>
        public ActivityOrderOption(IOptions<QuerySettings> defaultValues)
            : base(
                  new[] { "--activity-order", "-ao" },
                  () => defaultValues.Value.ActivityOrder == null ? new List<string>() : new List<string>(defaultValues.Value.ActivityOrder),
                  "The task activity order.")
        {
        }
    }
}