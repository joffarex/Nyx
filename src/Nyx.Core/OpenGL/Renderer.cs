using System;
using System.Collections.Generic;
using Nyx.Core.Components;
using Nyx.SharpTT;

namespace Nyx.Core.OpenGL
{
    public class Renderer : IDisposable
    {
        private const int MaxBatchSize = 1000;
        private readonly List<RenderBatch> _renderBatches;

        public Renderer()
        {
            _renderBatches = new List<RenderBatch>();
        }

        public void Dispose()
        {
            foreach (RenderBatch renderBatch in _renderBatches)
            {
                renderBatch.Dispose();
            }
        }

        public void Add(GameObject gameObject)
        {
            var sprite = gameObject.GetComponent<SpriteRenderer>();

            if (sprite != null)
            {
                Add(sprite);
            }
        }

        private void Add(SpriteRenderer sprite)
        {
            var added = false;
            foreach (RenderBatch renderBatch in _renderBatches)
            {
                if (renderBatch.HasRoom)
                {
                    renderBatch.AddSprite(sprite);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                var newRenderBatch = new RenderBatch(MaxBatchSize);
                newRenderBatch.Start();
                _renderBatches.Add(newRenderBatch);
                newRenderBatch.AddSprite(sprite);
            }
        }

        public void Render()
        {
            foreach (RenderBatch renderBatch in _renderBatches)
            {
                renderBatch.Render();
            }
        }
    }
}