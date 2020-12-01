using System.Collections.Generic;
using System.Numerics;
using Nyx.Core.Components;
using Nyx.Core.Graphics;
using Nyx.Core.Shared;

namespace Nyx.Core.Entities
{
    public class AnimatedSprite : Entity
    {
        public readonly Dictionary<string, int[]> Animations = new();

        private int _currentFrame;
        private float _timeBetweenFramesLeft;

        public AnimatedSprite(string name, SpriteSheet spriteSheet, float timeBetweenFrames, Vector2 position,
            float scale, int zIndex)
            : base(name, new Transform(position, spriteSheet.SpriteSize * scale), zIndex)
        {
            Init(spriteSheet, timeBetweenFrames);
        }

        public bool AnimationStarted { get; private set; }
        public string CurrentAnimation { get; private set; }
        public SpriteSheet SpriteSheet { get; private set; }
        public float TimeBetweenFrames { get; private set; }

        public void Init(SpriteSheet spriteSheet, float timeBetweenFrames)
        {
            SpriteSheet = spriteSheet;
            TimeBetweenFrames = timeBetweenFrames;

            // Set first sprite from spritesheet as default component in entity
            AddComponent(new SpriteRenderer(SpriteSheet.Sprites[0]));
        }

        public void AddAnimation(string name, int[] frames)
        {
            Animations.Add(name, frames);
        }

        public void PlayAnimation(string name, float deltaTime, bool flipped = false)
        {
            if (!CurrentAnimation.Equals(name))
            {
                AnimationStarted = false;
            }

            if (Animations.TryGetValue(name, out int[] frames))
            {
                if (!AnimationStarted)
                {
                    _currentFrame = frames[0];
                }

                CurrentAnimation = name;

                AnimationStarted = true;
                _timeBetweenFramesLeft -= deltaTime;

                if (_timeBetweenFramesLeft <= 0)
                {
                    _timeBetweenFramesLeft = TimeBetweenFrames;
                    _currentFrame++;
                    if (_currentFrame > frames[^1])
                    {
                        _currentFrame = frames[0];
                    }

                    Sprite sprite = SpriteSheet.Sprites[_currentFrame];

                    if (!flipped)
                    {
                        if (sprite.IsFlipped)
                        {
                            sprite.FlipY(true);
                        }

                        GetComponent<SpriteRenderer>().Sprite = sprite;
                    }
                    else
                    {
                        sprite.FlipY();
                        GetComponent<SpriteRenderer>().Sprite = sprite;
                    }
                }
            }
            else
            {
                // Default to first frame
                _currentFrame = 0;
                AnimationStarted = false;
            }
        }
    }
}