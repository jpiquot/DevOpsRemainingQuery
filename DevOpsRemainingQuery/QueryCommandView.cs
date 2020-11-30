using System;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Reflection;

namespace DevOpsRemainingQuery
{
    /// <summary>
    /// Database backup list view
    /// </summary>
    internal class QueryCommandView : StackLayoutView
    {
        protected readonly QueryCommandOptions _options;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public QueryCommandView(QueryCommandOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected TextSpanFormatter Formatter { get; } = new TextSpanFormatter();

        public virtual void Initialize()
        {
            string authentication;
            if (string.IsNullOrWhiteSpace(_options.PersonalAccessToken))
            {
                authentication = "Windows";
            }
            else
            {
                authentication = "Personal access token";
            }
            Add(new ContentView("\n"));
            Add(new ContentView(Span($"DevOps Remaining Work Query V{Assembly.GetExecutingAssembly().GetName().Version}".Orange())));
            Add(new ContentView(Span($"Jérôme Piquot".DarkOrange())));
            Add(new ContentView("\n"));
            Add(new ContentView(Span($"Server:          {_options.Server?.DarkGrey()}")));
            Add(new ContentView(Span($"Project:         {_options.Project?.DarkGrey()}")));
            Add(new ContentView(Span($"Query:           {_options.Query?.DarkGrey()}")));
            Add(new ContentView(Span($"Ouput file:      {_options.OutputFile?.DarkGrey()}")));
            Add(new ContentView(Span($"Area path:       {_options.AreaPath?.DarkGrey()}")));
            Add(new ContentView(Span($"Iteration path:  {_options.IterationPath?.ToString().DarkGrey()}")));
            Add(new ContentView(Span($"Authentication:  {authentication.DarkGrey()}")));
            Add(new ContentView("\n"));
            Formatter.AddFormatter<DateTime>(d => $"{d:d} {ForegroundColorSpan.DarkGray()}{d:t}");
        }

        protected TextSpan Span(FormattableString formattableString) => Formatter.ParseToSpan(formattableString);

        protected TextSpan Span(object obj) => Formatter.Format(obj);
    }
}