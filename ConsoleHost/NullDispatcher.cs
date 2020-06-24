using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorPerfTest
{
    // The dispatcher is what produces the same single-threaded work scheduling on Blazor Server as
    // we have on Blazor WebAssembly. It ensures that, for a given renderer, only one work item is
    // happening at any given time. However for the perf tests that's irrelevant so we can just
    // execute the work items directly.
    class NullDispatcher : Dispatcher
    {
        public override bool CheckAccess()
            => true;

        public override Task InvokeAsync(Action workItem)
        {
            workItem();
            return Task.CompletedTask;
        }

        public override Task InvokeAsync(Func<Task> workItem)
        {
            return workItem();
        }

        public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
        {
            return Task.FromResult(workItem());            
        }

        public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
        {
            return workItem();
        }
    }
}
