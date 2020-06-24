using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
#pragma warning disable BL0006

namespace BlazorPerfTest
{
    // For real Blazor applications, there are different renderers depending on the host technology
    // - For Blazor Server, we have a renderer subclass whose UpdateDisplayAsync sends messages over
    //   the websocket connection to the browser
    // - For Blazor WebAssembly, we have a renderer subclass whose UpdateDisplayAsync uses JavaScript
    //   interop to tell the JS code about a batch to be applied to the DOM
    class PerfTestRenderer : Renderer
    {
        public PerfTestRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
        }

        public override Dispatcher Dispatcher { get; } = new NullDispatcher();

        protected override void HandleException(Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }

        protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
        {
            // At this point, a real Blazor app would either send the renderbatch over the WebSocket
            // connection (if it's Blazor Server), or would use JS interop to pass it more directly
            // into the JS running in the browser (if it's Blazor WebAssembly).
            // However we're not interested in profiling that aspect of rendering here, so just no-op.
            return Task.CompletedTask;
        }

        // Expose some protected APIs publicly for testing
        public new int AssignRootComponentId(IComponent component)
            => base.AssignRootComponentId(component);
        public new Task RenderRootComponentAsync(int componentId)
            => base.RenderRootComponentAsync(componentId);
    }
}
#pragma warning restore BL0006
