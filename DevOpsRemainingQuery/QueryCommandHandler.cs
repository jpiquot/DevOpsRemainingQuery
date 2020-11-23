using System;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Threading.Tasks;

using DevOpsRemainingQuery.DevOps;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevOpsRemainingQuery
{
    internal class QueryCommandHandler : ICommandHandler
    {
        public QueryCommandHandler(IServiceProvider serviceProvider, QueryCommandOptions options)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            ConsoleRenderer = serviceProvider.GetRequiredService<ConsoleRenderer>();
            Logger = serviceProvider.GetRequiredService<ILogger<QueryCommandHandler>>();
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected ConsoleRenderer ConsoleRenderer { get; }
        protected ILogger<QueryCommandHandler> Logger { get; }
        protected QueryCommandOptions Options { get; }
        protected IServiceProvider ServiceProvider { get; }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var view = new QueryCommandView(Options);
            view.Initialize();
            _ = context.Console ?? throw new ArgumentException("Console property is null", nameof(context));
            using var screen = new ScreenView(ConsoleRenderer, context.Console)
            {
                Child = view
            };
            screen.Render();
            try
            {
                using DevOpsServer server = new DevOpsServer(Options.Server??"", Options.PersonalAccessToken??"");
                server.Connect();
                await Task.Delay(5000);
            }
            catch (Exception e)
            {
                Exception? inner = e;
                string message = e.Message;
                while ((inner = inner.InnerException) != null)
                {
                    message += "\n" + e.Message;
                }
                Logger.LogError(0, e, message);
                throw;
            }

            return 0;
        }
    }
}