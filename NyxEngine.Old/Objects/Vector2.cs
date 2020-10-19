using System;

namespace NyxEngine.Old.Objects
{
    public class Vector2 : Disposable
    {
        public Vector2()
        {
            X = Zero().X;
            Y = Zero().Y;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        ~Vector2()
        {
            Dispose(false);
        }

        public static Vector2 Zero()
        {
            return new Vector2(0, 0);
        }

        public override void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            finally
            {
                base.Dispose(true);
            }
        }
    }
}