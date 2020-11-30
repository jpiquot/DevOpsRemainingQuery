using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace DevOpsRemainingQuery.DevOps
{
    internal class DevOpsQuery
    {
        private readonly DevOpsProject _project;
        private readonly string _queryName;
        private readonly DevOpsServer _server;
        private QueryHierarchyItem? _queryHierarchyItem;

        public DevOpsQuery(DevOpsServer server, DevOpsProject project, string queryName)
        {
            _server = server;
            _project = project;
            _queryName = queryName;
        }

        public QueryHierarchyItem QueryHierarchyItem => _queryHierarchyItem ??= GetQueryHierarchyItem().GetAwaiter().GetResult();

        public string Wiql => QueryHierarchyItem.Wiql;

        public async Task<QueryHierarchyItem> GetQueryHierarchyItem()
        {
            if (_queryHierarchyItem == null)
            {
                WorkItemTrackingHttpClient witClient = _server.Connection.GetClient<WorkItemTrackingHttpClient>();
                _queryHierarchyItem = await witClient.GetQueryAsync(_project.Id, _queryName);
            }
            return _queryHierarchyItem;
        }

        public async Task<string> GetWiql()
        {
            return (await GetQueryHierarchyItem()).Wiql;
        }
    }
}