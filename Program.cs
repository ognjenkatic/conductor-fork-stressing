using System.Reflection;
using ConductorJoinIssue;
using ConductorSharp.Engine.Extensions;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder
    .Services.AddConductorSharp(baseUrl: "http://localhost:48081")
    .AddExecutionManager(1000, 250, 100, null, [Assembly.GetExecutingAssembly()]);

builder.Services.RegisterWorkerTask<ForkyDorkyWorker>();
builder.Services.RegisterWorkerTask<SleepyDeepyWorker>();
builder.Services.RegisterWorkflow<ForkyWorkyWorkflow>();

var host = builder.Build();
host.Run();
