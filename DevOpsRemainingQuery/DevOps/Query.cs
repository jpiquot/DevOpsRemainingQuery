﻿namespace DevOpsRemainingQuery.DevOps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi;

    /// <summary>
    /// The Query class. Handles all the query informations.
    /// </summary>
    internal class Query : IDisposable
    {
        private readonly string queryName;
        private bool disposedValue;
        private QueryHierarchyItem? queryHierarchyItem;
        private WorkItemTrackingHttpClient? witClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="Query"/> class.
        /// </summary>
        /// <param name="project">The DevOps query instance.</param>
        /// <param name="queryName">The DevOps project name.</param>
        public Query(Project project, string queryName)
        {
            Project = project;
            this.queryName = queryName;
        }

        /// <summary>
        /// Gets the query hierarchy item.
        /// </summary>
        /// <value>The query hierarchy item.</value>
        /// <autogeneratedoc/>
        public QueryHierarchyItem QueryHierarchyItem => queryHierarchyItem ??= GetQueryHierarchyItem().GetAwaiter().GetResult();

        /// <summary>
        /// Gets the wiql.
        /// </summary>
        /// <value>The wiql.</value>
        /// <autogeneratedoc/>
        public string Wiql => QueryHierarchyItem.Wiql;

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <value>The project.</value>
        /// <autogeneratedoc/>
        protected Project Project { get; }

        /// <summary>
        /// Gets the wit client.
        /// </summary>
        /// <value>The wit client.</value>
        /// <autogeneratedoc/>
        protected WorkItemTrackingHttpClient WitClient => witClient ??= Project.Server.Connection.GetClient<WorkItemTrackingHttpClient>();

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>QueryData.</returns>
        /// <autogeneratedoc/>
        public virtual async Task<QueryData> GetData()
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

                records.Add(record);
            }

            return new QueryData(await GetFieldNames(), records);
        }

        /// <summary>
        /// Gets the field names.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        /// <autogeneratedoc/>
        public virtual async Task<List<string>> GetFieldNames()
            => (await GetQueryHierarchyItem()).Columns.Select(p => p.Name).ToList();

        /// <summary>
        /// Gets the field reference names.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        /// <autogeneratedoc/>
        public virtual async Task<List<string>> GetFieldReferenceNames()
            => (await GetQueryHierarchyItem()).Columns.Select(p => p.ReferenceName).ToList();

        /// <summary>
        /// Gets the query hierarchy item.
        /// </summary>
        /// <returns>QueryHierarchyItem.</returns>
        /// <autogeneratedoc/>
        public async Task<QueryHierarchyItem> GetQueryHierarchyItem()
        {
            if (queryHierarchyItem == null)
            {
                queryHierarchyItem = await WitClient.GetQueryAsync(Project.Id, queryName, QueryExpand.All);
            }

            return queryHierarchyItem;
        }

        /// <summary>
        /// Gets the query work items.
        /// </summary>
        /// <returns>List&lt;WorkItem&gt;.</returns>
        /// <exception cref="Exception">Query not found : " + queryName.</exception>
        /// <autogeneratedoc/>
        public async Task<List<WorkItem>> GetQueryWorkItems()
        {
            var qhi = await GetQueryHierarchyItem();
            var result = await WitClient.QueryByIdAsync(qhi.Id);
            if (result == null)
            {
                throw new VssResourceNotFoundException("Query not found : " + queryName);
            }

            var ids = result.WorkItems.Select(item => item.Id).ToArray();

            if (ids.Length == 0)
            {
                return new List<WorkItem>();
            }

            var list = new List<WorkItem>(ids.Length);
            for (int i = 0; i <= ids.Length / 200; i++)
            {
                var idsTrunk = ids.Skip(i * 200).Take(200);
                list.AddRange(await WitClient.GetWorkItemsAsync(idsTrunk, null, result.AsOf, WorkItemExpand.All));
            }

            // get work items for the ids found in query
            return list;
        }

        /// <summary>
        /// Gets the wiql.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc/>
        public async Task<string> GetWiql()
        {
            return (await GetQueryHierarchyItem()).Wiql;
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        /// <autogeneratedoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (witClient != null)
                    {
                        var client = witClient;
                        witClient = null;
                        client.Dispose();
                    }
                }

                disposedValue = true;
            }
        }
    }
}