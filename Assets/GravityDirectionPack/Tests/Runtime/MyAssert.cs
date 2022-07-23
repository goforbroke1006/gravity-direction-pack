using System;
using UnityEngine.Assertions;

namespace GravityDirectionPack.Tests.Runtime
{
    public static class MyAssert
    {
        public static void InDelta(float actual, float expected, float delta)
        {
            delta = Math.Abs(delta);

            float leftBorder = expected - delta;
            float rightBorder = expected + delta;

            if (!(leftBorder <= actual) || !(actual <= rightBorder))
            {
                throw new AssertionException(
                    $"Got {actual}\n  Want in range between ({leftBorder} ... {rightBorder})", "");
            }
        }
    }
}