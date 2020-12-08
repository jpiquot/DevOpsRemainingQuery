namespace DevOpsRemainingQuery.DevOps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi;

    /// <summary>
    /// Class RemainingWorkQuery. Implements the <see cref="DevOpsRemainingQuery.DevOps.Query"/>.
    /// </summary>
    /// <seealso cref="DevOpsRemainingQuery.DevOps.Query"/>
    internal class EffortQuery : Query
    {
        private readonly IEnumerable<string> activityOrder;
        private readonly int parentDepth;
        private ActivityCollection? activityCollection;
        private WorkItemRelationType? childRelationType;
        private WorkItemRelationType? parentRelationType;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffortQuery"/> class.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="queryName">Name of the query.</param>
        /// <param name="activityOrder">The activity order.</param>
        /// <param name="parentDepth">The maximum parent hierarchy depth.</param>
        public EffortQuery(Project project, string queryName, IEnumerable<string> activityOrder, int parentDepth)
            : base(project, queryName)
        {
            this.activityOrder = activityOrder;
            this.parentDepth = parentDepth;
        }

        /// <summary>
        /// Gets the task field reference names.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetTaskFieldReferenceNames()
            => new List<string>(new string[] { WorkItemFieldType.Activity, WorkItemFieldType.OriginalEstimate, WorkItemFieldType.RemainingWork, WorkItemFieldType.CompletedWork });

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>QueryData.</returns>
        public override async Task<QueryData> GetData()
        {
            var fields = await GetFieldReferenceNames();
            var wis = await GetQueryWorkItems();
            var records = new List<List<object?>>(wis.Count);
            foreach (var wi in wis)
            {
                var record = new List<object?>(fields.Count);
                foreach (var field in fields)
                {
                    var value = wi.Fields.Where(p => p.Key == field).Select(p => p.Value).FirstOrDefault();
                    record.Add(value);
                }

                await AddEffortValues(record, wi);
                await AddParentHierarchyValues(record, wi);
                records.Add(record);
            }

            return new QueryData(await GetFieldNames(), records);
        }

        /// <summary>
        /// Gets the field names.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public override async Task<List<string>> GetFieldNames()
        {
            List<string> fieldNames = await base.GetFieldNames();
            fieldNames.AddRange(new string[] { "Total Estimated", "Total Remaining", "Total Completed" });
            foreach (var activity in (activityCollection ?? await GetOrderedActivityList()).Activities)
            {
                fieldNames.AddRange(new string[] { activity + " Original Estimate", activity + " Remaining", activity + " Completed" });
            }

            for (int i = 0; i < parentDepth; i++)
            {
                fieldNames.Add("Parent " + (i + 1));
            }

            return fieldNames;
        }

        /// <summary>
        /// Gets the linked work item ids.
        /// </summary>
        /// <param name="workItem">The work item.</param>
        /// <param name="relationType">Type of the relation.</param>
        /// <returns>List&lt;System.Int32&gt;.</returns>
        /// <exception cref="DevOpsRemainingQuery.DevOps.InvalidWorkItemUrlException">
        /// The work item url.
        /// </exception>
        private static List<int> GetLinkedWorkItemIds(WorkItem workItem, string relationType)
        {
            var list = new List<int>();
            foreach (string url in workItem
                                    .Relations
                                    .Where(p => p.Rel == relationType)
                                    .Select(p => p.Url)
                                    .ToList())
            {
                try
                {
                    list.Add(Convert.ToInt32(url
                                                .Split('/')
                                                .LastOrDefault()));
                }
                catch (Exception e)
                {
                    throw new InvalidWorkItemUrlException(url, e);
                }
            }

            return list;
        }

        /// <summary>
        /// Adds the effort values.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="workItem">The work item.</param>
        /// <returns>Task.</returns>
        /// <exception cref="VssResourceNotFoundException">Query not found : " + queryName.</exception>
        private async Task AddEffortValues(List<object?> record, WorkItem workItem)
        {
            var childRelType = (childRelationType ?? await GetChildRelationType()).ReferenceName;
            var fieldNames = GetTaskFieldReferenceNames();
            var wis = await WitClient.GetWorkItemsAsync(GetLinkedWorkItemIds(workItem, childRelType), null, null, WorkItemExpand.Fields);
            var tasks = wis
                .Where(p => p.Fields.ContainsKey(WorkItemFieldType.Activity) && p.Fields.ContainsKey(WorkItemFieldType.WorkItemType) && p.Fields[WorkItemFieldType.WorkItemType] as string == WorkItemType.Task)
                .Select(p => new Effort
                {
                    Activity = p.Fields[WorkItemFieldType.Activity] as string ?? string.Empty,
                    OriginalEstimate = p.Fields.ContainsKey(WorkItemFieldType.OriginalEstimate) ? p.Fields[WorkItemFieldType.OriginalEstimate] as double? ?? 0 : 0,
                    RemainingWork = p.Fields.ContainsKey(WorkItemFieldType.RemainingWork) ? p.Fields[WorkItemFieldType.RemainingWork] as double? ?? 0 : 0,
                    CompletedWork = p.Fields.ContainsKey(WorkItemFieldType.CompletedWork) ? p.Fields[WorkItemFieldType.CompletedWork] as double? ?? 0 : 0,
                })
                .GroupBy(p => p.Activity)
                .Select(p => new Effort
                {
                    Activity = p.Key,
                    OriginalEstimate = p.Sum(q => q.OriginalEstimate),
                    RemainingWork = p.Sum(q => q.RemainingWork),
                    CompletedWork = p.Sum(q => q.CompletedWork),
                })
                .ToList();
            record.AddRange(new object[] { tasks.Sum(p => p.OriginalEstimate), tasks.Sum(p => p.RemainingWork), tasks.Sum(p => p.CompletedWork) });
            foreach (var activity in (activityCollection ?? await GetOrderedActivityList()).Activities)
            {
                var query = tasks.Where(p => p.Activity == activity);
                if (query.Any())
                {
                    var effort = query.Single();
                    record.AddRange(new object[] { effort.OriginalEstimate, effort.RemainingWork, effort.CompletedWork });
                }
                else
                {
                    record.AddRange(new object[] { 0, 0, 0 });
                }
            }
        }

        /// <summary>
        /// Adds the parent hierarchy values.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="workItem">The work item.</param>
        /// <autogeneratedoc/>
        private async Task AddParentHierarchyValues(List<object?> record, WorkItem workItem)
        {
            var parents = new List<string>(parentDepth);
            await FindParentHierarchy(workItem, parents);
            parents.Reverse();
            for (int i = parents.Count; i < parentDepth; i++)
            {
                parents.Add(string.Empty);
            }

            record.AddRange(parents.Take(parentDepth));
        }

        /// <summary>
        /// Finds the parent hierarchy.
        /// </summary>
        /// <param name="workItem">The work item.</param>
        /// <param name="parents">The parents.</param>
        /// <autogeneratedoc/>
        private async Task FindParentHierarchy(WorkItem workItem, List<string> parents)
        {
            var parentRelType = (parentRelationType ?? await GetParentRelationType()).ReferenceName;
            var parentIds = GetLinkedWorkItemIds(workItem, parentRelType);
            if (!parentIds.Any())
            {
                return;
            }

            var wis = await WitClient.GetWorkItemsAsync(parentIds, null, null, WorkItemExpand.All);
            var parent = wis
                .Where(
                    p => p.Fields.ContainsKey(WorkItemFieldType.Title) &&
                    p.Fields.ContainsKey(WorkItemFieldType.WorkItemType) &&
                    !string.IsNullOrWhiteSpace(p.Fields[WorkItemFieldType.WorkItemType] as string) &&
                    (p.Fields[WorkItemFieldType.WorkItemType] as string == WorkItemType.Epic ||
                    p.Fields[WorkItemFieldType.WorkItemType] as string == WorkItemType.Feature))
                .FirstOrDefault();
            if (parent != null)
            {
                parents.Add((string)parent.Fields[WorkItemFieldType.Title]);
                await FindParentHierarchy(parent, parents);
            }
        }

        /// <summary>
        /// Gets the type of the child relation.
        /// </summary>
        /// <returns>WorkItemRelationType.</returns>
        /// <autogeneratedoc/>
        private async Task<WorkItemRelationType> GetChildRelationType()
        {
            return childRelationType ??= await WitClient.GetRelationTypeAsync(LinkTypes.Child);
        }

        /// <summary>
        /// Gets the ordered activity list.
        /// </summary>
        /// <returns>ActivityCollection.</returns>
        /// <autogeneratedoc/>
        private async Task<ActivityCollection> GetOrderedActivityList()
        {
            return activityCollection ??= new ActivityCollection(activityOrder, (await WitClient.GetWorkItemTypeFieldWithReferencesAsync(Project.Id, WorkItemType.Task, WorkItemFieldType.Activity, WorkItemTypeFieldsExpandLevel.AllowedValues)).AllowedValues.Where(p => (p as string) != null).Select(p => (string)p));
        }

        /// <summary>
        /// Gets the type of the parent relation.
        /// </summary>
        /// <returns>WorkItemRelationType.</returns>
        /// <autogeneratedoc/>
        private async Task<WorkItemRelationType> GetParentRelationType()
        {
            return parentRelationType ??= await WitClient.GetRelationTypeAsync(LinkTypes.Parent);
        }
    }
}