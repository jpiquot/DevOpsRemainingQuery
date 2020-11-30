﻿using System.CommandLine;
using System.CommandLine.Invocation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DevOpsRemainingQuery
{
    /// <summary>
    /// Main command. Implements the <see cref="System.CommandLine.RootCommand"/>
    /// </summary>
    /// <seealso cref="System.CommandLine.RootCommand"/>
    internal class QueryCommand : RootCommand
    {
        public QueryCommand(IOptions<QuerySettings> defaultValues)
            : base("Azure DevOps Remaining Work Query")
        {
            AddGlobalOption(new OutputFileOption(defaultValues));
            AddGlobalOption(new AreaPathOption(defaultValues));
            AddGlobalOption(new IterationPathOption(defaultValues));
            AddGlobalOption(new ServerOption(defaultValues));
            AddGlobalOption(new ProjectOption(defaultValues));
            AddGlobalOption(new QueryOption(defaultValues));
            AddGlobalOption(new PersonalAccessTokenOption(defaultValues));

            Handler = CommandHandler.Create<IHost, QueryCommandOptions>(async (host, options) =>
            {
                await new QueryCommandHandler(
                        host.Services,
                        options
                    )
                    .InvokeAsync(host.Services.GetRequiredService<InvocationContext>());
            });
        }
    }
}