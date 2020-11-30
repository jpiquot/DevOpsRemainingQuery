using System;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.Core.WebApi;

namespace DevOpsRemainingQuery.DevOps
{
    internal class DevOpsProject
    {
        private readonly string _projectName;
        private readonly DevOpsServer _server;
        private TeamProject? _project;

        public DevOpsProject(DevOpsServer server, string projectName)
        {
            _server = server;
            _projectName = projectName;
        }

        public Guid Id => TeamProject.Id;

        private TeamProject TeamProject => _project ??= GetProject().GetAwaiter().GetResult();

        public async Task<TeamProject> GetProject()
        {
            if (_project == null)
            {
                ProjectHttpClient projectClient = _server.Connection.GetClient<ProjectHttpClient>();

                _project = await projectClient.GetProject(_projectName);
            }
            return _project;
        }

        public async Task<Guid> GetProjectId()
        {
            if (_project == null)
            {
                return (await GetProject()).Id;
            }
            return _project.Id;
        }
    }
}