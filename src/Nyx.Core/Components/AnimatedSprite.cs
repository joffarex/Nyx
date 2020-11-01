﻿using System.Collections.Generic;
using System.Numerics;

namespace Nyx.Core.Components
{
    public class AnimatedSprite : GameObject
    {
        private bool _animationStarted;

        private string _currentAnimation = string.Empty;

        private int _currentFrame;
        private float _timeBetweenFramesLeft;

        public Dictionary<string, int[]> Animations = new Dictionary<string, int[]>();

        public AnimatedSprite(SpriteSheet spriteSheet, float timeBetweenFrames, Vector2 position, float scale,
            int zIndex)
            : base("AnimatedSprite", new Transform(position, spriteSheet.SpriteSize * 10), zIndex)
        {
            SpriteSheet = spriteSheet;
            TimeBetweenFrames = timeBetweenFrames;

            // Set first sprite from spritesheed as default component in game object
            AddComponent(new SpriteRenderer(SpriteSheet.Sprites[0]));
        }

        public SpriteSheet SpriteSheet { get; }
        public float TimeBetweenFrames { get; set; }

        public void AddAnimation(string name, int[] frames)
        {
            Animations.Add(name, frames);
        }

        public void PlayAnimation(string name, float deltaTime)
        {
            if (Animations.TryGetValue(name, out int[] frames))
            {
                if (!_animationStarted)
                {
                    _currentFrame = frames[0];
                }

                _currentAnimation = name;

                _animationStarted = true;
                _timeBetweenFramesLeft -= deltaTime;

                if (_timeBetweenFramesLeft <= 0)
                {
                    _timeBetweenFramesLeft = TimeBetweenFrames;
                    _currentFrame++;
                    if (_currentFrame > frames[^1])
                    {
                        _currentFrame = frames[0];
                    }

                    GetComponent<SpriteRenderer>().Sprite = SpriteSheet.Sprites[_currentFrame];
                }
            }
            else
            {
                // Default to first frame
                _currentFrame = 0;
                _animationStarted = false;
            }
        }
    }
}