using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace DevOpsRemainingQuery.DevOps
{
    internal class DevOpsServer : IDisposable
    {
        private readonly string _personalAccessToken;
        private readonly string _serverUrl;
        private VssConnection? _connection;

        public DevOpsServer(string serverUrl, string personalAccessToken)
        {
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                throw new ArgumentException("The server URL is not defined.", nameof(serverUrl));
            }
            _serverUrl = serverUrl;
            _personalAccessToken = personalAccessToken;
        }

        public VssConnection Connection { get => _connection ?? throw new Exception("The connection has not been initialized."); }

        public void Connect()
        {
            VssCredentials credentials;
            if (string.IsNullOrWhiteSpace(_personalAccessToken))
            {
                credentials = new VssCredentials();
            }
            else
            {
                credentials = new VssBasicCredential(string.Empty, _personalAccessToken);
            }
            // Connect to Azure DevOps Services
            _connection = new VssConnection(new Uri(_serverUrl), credentials);
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                var connection = _connection;
                _connection = null;
                connection.Dispose();
            }
        }
    }
}