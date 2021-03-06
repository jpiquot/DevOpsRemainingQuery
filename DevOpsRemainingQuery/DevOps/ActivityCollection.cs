﻿namespace DevOpsRemainingQuery.DevOps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.Services.Common;

    /// <summary>
    /// Class ActivityCollection. Gives an ordered list of activities.
    /// </summary>
    public class ActivityCollection
    {
        /// <summary>
        /// The activities.
        /// </summary>
        /// <autogeneratedoc/>
        private readonly List<string> orderedActivities;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityCollection"/> class.
        /// </summary>
        /// <param name="activityOrder">The activity order.</param>
        /// <param name="activities">The activities.</param>
        /// <exception cref="ArgumentException">
        /// The activity order list contains activities that do not exist in the system : ".
        /// </exception>
        /// <autogeneratedoc/>
        public ActivityCollection(IEnumerable<string> activityOrder, IEnumerable<string> activities)
        {
            var notFound = activityOrder.Where(p => !activities.Contains(p)).ToList();
            if (notFound.Count > 0)
            {
                throw new ArgumentException("The activity order list contains activities that do not exist in the system : " + string.Join(",", notFound), nameof(activityOrder));
            }

            orderedActivities = new List<string>(activityOrder);
            orderedActivities.AddRange(activities.Where(p => !activityOrder.Contains(p)));
        }

        /// <summary>
        /// Gets the activities.
        /// </summary>
        /// <value>The activities.</value>
        /// <autogeneratedoc/>
        public List<string> Activities => orderedActivities;
    }
}