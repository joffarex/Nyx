using System;
using System.Collections.Generic;
using System.Linq;
using Nyx.Core.Components;
using Nyx.Core.Entities;

namespace Nyx.Core.Renderer
{
    public class BatchRenderer : IDisposable
    {
        private const int MaxBatchSize = 1000;
        private readonly List<Batch> _batches = new();

        public void Dispose()
        {
            foreach (Batch batch in _batches)
            {
                batch.Dispose();
            }
        }

        public void Add(Entity entity)
        {
            var sprite = entity.GetComponent<SpriteRenderer>();

            if (sprite != null)
            {
                Add(sprite);
            }
        }

        private void Add(SpriteRenderer sprite)
        {
            var added = false;
            foreach (Batch batch in _batches
                    .Where(batch => batch.HasRoom && (batch.ZIndex == sprite.Entity.ZIndex)))
                // Make sure to only add same ZIndex sprites to the same batch
            {
                Texture texture = sprite.Texture;
                if (batch.HasTexture(texture) || batch.HasTextureRoom)
                {
                    batch.AddSprite(sprite);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                var newRenderBatch = new Batch(MaxBatchSize, sprite.Entity.ZIndex);
                newRenderBatch.Start();
                _batches.Add(newRenderBatch);
                newRenderBatch.AddSprite(sprite);
                _batches.Sort();
            }
        }

        public void Update(ref double deltaTime)
        {
            foreach (Batch batch in _batches)
            {
                batch.Update(ref deltaTime);
            }
        }

        public void Render(ref double deltaTime)
        {
            foreach (Batch batch in _batches)
            {
                batch.Render(ref deltaTime);
            }
        }
    }
}