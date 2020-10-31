using System;
using System.Collections.Generic;
using System.Linq;
using Nyx.Core.Components;

namespace Nyx.Core.Renderer
{
    public class BatchRenderer : IDisposable
    {
        private const int MaxBatchSize = 1000;
        private readonly List<Batch> _batches;

        public BatchRenderer()
        {
            _batches = new List<Batch>();
        }

        public void Dispose()
        {
            foreach (Batch batch in _batches)
            {
                batch.Dispose();
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
            foreach (Batch batch in _batches
                    .Where(batch => batch.HasRoom && (batch.Z_Index == sprite.GameObject.ZIndex)))
                // Make sure to only add same Z_Index sprites to the same batch
            {
                Texture texture = sprite.Texture;
                if (batch.HasTexture(texture) || batch.HasTextureRoom || (texture == null))
                {
                    batch.AddSprite(sprite);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                var newRenderBatch = new Batch(MaxBatchSize, sprite.GameObject.ZIndex);
                newRenderBatch.Start();
                _batches.Add(newRenderBatch);
                newRenderBatch.AddSprite(sprite);
                _batches.Sort();
            }
        }

        public void Update(float deltaTime)
        {
            foreach (Batch batch in _batches)
            {
                batch.Update(deltaTime);
            }
        }

        public void Render()
        {
            foreach (Batch batch in _batches)
            {
                batch.Render();
            }
        }
    }
}