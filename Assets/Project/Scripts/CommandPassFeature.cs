using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;



public class CommandPassFeature : ScriptableRendererFeature
{
    public CommandBufferRef commandBufferRefHolder;
    public class CommandPass : ScriptableRenderPass
    {
        private CommandBufferRef commandBufferRefHolder;

        public CommandPass(CommandBufferRef commandBufferRefHolder)
        {
            this.commandBufferRefHolder = commandBufferRefHolder;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {

            if (commandBufferRefHolder.CB != null)
            {
                context.ExecuteCommandBuffer(commandBufferRefHolder.CB);
            }
            else
            {
                var cmd = CommandBufferPool.Get("My Custom Pass");
                context.ExecuteCommandBuffer(cmd);
            }
        }
    }

    private CommandPass _commandPass;
    
    public override void Create()
    {
        _commandPass = new CommandPass(commandBufferRefHolder);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_commandPass);
    }
}

