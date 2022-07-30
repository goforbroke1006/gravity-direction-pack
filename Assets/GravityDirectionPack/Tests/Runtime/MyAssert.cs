using System;
using UnityEngine;
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

        public enum PositionChangedAxis
        {
            X,
            Y,
            Z
        }

        public enum PositionChangedValueDir
        {
            Equal,
            Increase,
            IncreaseOrEqual,
            Decrease,
            DecreaseOrEqual,
        }

        public static void PositionChanged(
            Vector3 first, Vector3 second,
            PositionChangedAxis axis,
            PositionChangedValueDir valueDir
        )
        {
            float prevVal = 0;
            float currVal = 0;

            switch (axis)
            {
                case PositionChangedAxis.X:
                    prevVal = first.x;
                    currVal = second.x;
                    break;
                case PositionChangedAxis.Y:
                    prevVal = first.y;
                    currVal = second.y;
                    break;
                case PositionChangedAxis.Z:
                    prevVal = first.z;
                    currVal = second.z;
                    break;
            }

            float floatTolerance = 0.01f;
            switch (valueDir)
            {
                case PositionChangedValueDir.Equal:
                    if (Math.Abs(prevVal - currVal) > floatTolerance)
                        throw new AssertionException($"{axis.ToString()}: {prevVal} and {currVal} not equal", "");
                    break;
                case PositionChangedValueDir.Decrease:
                    if (!(currVal < prevVal))
                        throw new AssertionException($"{axis.ToString()}: {currVal} not less than {prevVal}", "");
                    break;
                case PositionChangedValueDir.DecreaseOrEqual:
                    if (!(currVal <= prevVal))
                        throw new AssertionException($"{axis.ToString()}: {currVal} not less than {prevVal}", "");
                    break;
                case PositionChangedValueDir.Increase:
                    if (!(currVal > prevVal))
                        throw new AssertionException($"{axis.ToString()}: {currVal} not greater than {prevVal}", "");
                    break;
                case PositionChangedValueDir.IncreaseOrEqual:
                    if (!(currVal >= prevVal))
                        throw new AssertionException($"{axis.ToString()}: {currVal} not greater than {prevVal}", "");
                    break;
            }
        }
    }
}