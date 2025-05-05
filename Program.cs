using System.Reflection;
using ConductorJoinIssue;
using ConductorSharp.Engine;
using ConductorSharp.Engine.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddFilter("System.Net.Http.HttpClient.ConductorSharp.Client", LogLevel.None);

builder
    .Services.AddConductorSharp(baseUrl: "http://localhost:48081")
    .AddExecutionManager(1000, 250, 100, null, [Assembly.GetExecutingAssembly()])
    .UseBetaExecutionManager();

builder.Services.RegisterWorkerTask<ForkyDorkyWorker>();
builder.Services.RegisterWorkerTask<SleepyDeepyWorker>();
builder.Services.RegisterWorkflow<ForkyWorkyWorkflow>();
var host = builder.Build();
host.Run();