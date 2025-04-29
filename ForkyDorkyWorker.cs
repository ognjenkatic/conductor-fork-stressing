using ConductorSharp.Client;
using ConductorSharp.Client.Generated;
using ConductorSharp.Engine;
using ConductorSharp.Engine.Builders.Metadata;
using ConductorSharp.Engine.Util;
using MediatR;
using Newtonsoft.Json.Linq;

namespace ConductorJoinIssue
{
    [OriginalName("forky_dorky")]
    public class ForkyDorkyWorker
        : TaskRequestHandler<ForkyDorkyWorker.Request, ForkyDorkyWorker.Response>
    {
        public record Request : IRequest<Response>
        {
            public int TaskCount { get; set; }
            public int SleepRangeFrom { get; set; }
            public int SleepRangeTo { get; set; }
        }

        public record Response
        {
            public List<WorkflowTask> DynamicTasks { get; set; } = [];

            public Dictionary<string, JObject> DynamicTasksI { get; set; } = [];
        };

        public override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var workerCount = Math.Clamp(request.TaskCount, 1, 500);

            for (int i = 0; i < workerCount; i++)
            {
                response.DynamicTasks.Add(
                    new WorkflowTask
                    {
                        TaskReferenceName = $"sleepy-{i}",
                        Name = NamingUtil.NameOf<SleepyDeepyWorker>(),
                        Type = "SIMPLE",
                    }
                );

                response.DynamicTasksI.Add(
                    $"sleepy-{i}",
                    JObject.FromObject(
                        new SleepyDeepyWorker.Request
                        {
                            SleepRangeFrom = request.SleepRangeFrom,
                            SleepRangeTo = request.SleepRangeTo,
                        },
                        ConductorConstants.IoJsonSerializer
                    )
                );
            }

            return System.Threading.Tasks.Task.FromResult(response);
        }
    }
}
