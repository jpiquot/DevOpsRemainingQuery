﻿namespace DevOpsRemainingQuery.DevOps
{
    using System;

    using Microsoft.VisualStudio.Services.Common;
    using Microsoft.VisualStudio.Services.WebApi;

    /// <summary>
    /// Class Server. Implements the <see cref="System.IDisposable"/>.
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    /// <autogeneratedoc/>
    internal class Server : IDisposable
    {
        private readonly string personalAccessToken;
        private readonly string serverUrl;
        private VssConnection? connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        /// <param name="personalAccessToken">The personal access token.</param>
        /// <exception cref="ArgumentException">The server URL is not defined. - serverUrl.</exception>
        /// <autogeneratedoc/>
        public Server(string serverUrl, string personalAccessToken)
        {
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                throw new ArgumentException("The server URL is not defined.", nameof(serverUrl));
            }

            this.serverUrl = serverUrl;
            this.personalAccessToken = personalAccessToken;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        /// <exception cref="Exception">The connection has not been initialized.</exception>
        /// <autogeneratedoc/>
        public VssConnection Connection { get => connection ?? throw new InvalidOperationException("The connection has not been initialized."); }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        /// <autogeneratedoc/>
        public void Connect()
        {
            VssCredentials credentials;
            if (string.IsNullOrWhiteSpace(personalAccessToken))
            {
                credentials = new VssCredentials();
            }
            else
            {
                credentials = new VssBasicCredential(string.Empty, personalAccessToken);
            }

            // Connect to Azure DevOps Services
            connection = new VssConnection(new Uri(serverUrl), credentials);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        /// <autogeneratedoc/>
        public void Dispose()
        {
            if (connection != null)
            {
                var connection = this.connection;
                this.connection = null;
                connection.Dispose();
            }
        }
    }
}