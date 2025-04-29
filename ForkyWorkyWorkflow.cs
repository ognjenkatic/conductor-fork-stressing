using ConductorSharp.Engine.Builders;
using ConductorSharp.Engine.Builders.Metadata;
using ConductorSharp.Engine.Model;

namespace ConductorJoinIssue
{
    public class ForkyWorkyWorkflowInput : WorkflowInput<ForkyWorkyWorkflowOutput>
    {
        public int TaskCount { get; set; }
        public int SleepRangeFrom { get; set; }
        public int SleepRangeTo { get; set; }
    }

    public class ForkyWorkyWorkflowOutput : WorkflowOutput { }

    [OriginalName("forky_worky")]
    public class ForkyWorkyWorkflow
        : Workflow<ForkyWorkyWorkflow, ForkyWorkyWorkflowInput, ForkyWorkyWorkflowOutput>
    {
        public ForkyDorkyWorker GetForkies { get; set; }
        public required DynamicForkJoinTaskModel ForkyForky { get; set; }

        public ForkyWorkyWorkflow(
            WorkflowDefinitionBuilder<
                ForkyWorkyWorkflow,
                ForkyWorkyWorkflowInput,
                ForkyWorkyWorkflowOutput
            > builder
        )
            : base(builder)
        {
            _builder.AddTask(
                w => w.GetForkies,
                w => new ForkyDorkyWorker.Request()
                {
                    SleepRangeFrom = w.Input.SleepRangeFrom,
                    SleepRangeTo = w.Input.SleepRangeTo,
                    TaskCount = w.Input.TaskCount,
                }
            );

            _builder.AddTask(
                wf => wf.ForkyForky,
                wf =>
                    new()
                    {
                        DynamicTasks = wf.GetForkies.Output.DynamicTasks,
                        DynamicTasksI = wf.GetForkies.Output.DynamicTasksI,
                    }
            );
        }
    }
}
