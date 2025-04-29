using ConductorSharp.Engine;
using ConductorSharp.Engine.Builders.Metadata;
using MediatR;

namespace ConductorJoinIssue;

[OriginalName("sleepy_deepy")]
public class SleepyDeepyWorker
    : TaskRequestHandler<SleepyDeepyWorker.Request, SleepyDeepyWorker.Response>
{
    private static readonly Random _random = new();

    public record Request : IRequest<Response>
    {
        public int SleepRangeFrom { get; set; }
        public int SleepRangeTo { get; set; }
    }

    public record Response;

    public override async Task<Response> Handle(
        Request request,
        CancellationToken cancellationToken
    )
    {
        var sleepFrom = Math.Clamp(request.SleepRangeFrom, 0, 60 * 1000);
        var sleepTo = Math.Clamp(request.SleepRangeTo, sleepFrom, 60 * 1000);

        await Task.Delay(_random.Next(sleepFrom, sleepTo), cancellationToken);
        return new();
    }
}
