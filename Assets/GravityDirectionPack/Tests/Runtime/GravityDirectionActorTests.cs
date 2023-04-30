using System;
using System.Collections;
using GravityDirectionPack.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace GravityDirectionPack.Tests.Runtime
{
    // ReSharper disable once InconsistentNaming
    public class GravityDirectionActorTests_Common
    {
        private const float UpdateFrameFastExec = 1.0f;
        private const float UpdateFrameSlowExec = 25.0f;

        [UnityTest]
        public IEnumerator PropertyRadiusUpdatingWorksFine()
        {
            SceneHelper.ClearScene();

            GameObject cam = Object.Instantiate(SceneHelper.GetCamRes());
            cam.transform.position = new Vector3(-0.45988f, 1.375f, -2.869419f);

            GameObject env = Object.Instantiate(SceneHelper.GetEnvRes());
            env.transform.position = Vector3.zero;

            GameObject ch = Object.Instantiate(SceneHelper.GetCharRes());
            ch.transform.position = new Vector3(-1.711f, 0, 1.130581f);
            yield return null;
            yield return null;
            yield return null;

            float actualExposedRadius = ch.GetComponent<GravityDirectionActor>().radius;
            float expectedRealCapsuleRadius = ch.GetComponent<CapsuleCollider>().radius;
            Assert.IsTrue(Math.Abs(actualExposedRadius - expectedRealCapsuleRadius) < 0.01f);

            SceneHelper.ClearScene();
            yield return null;
        }

        [UnityTest]
        public IEnumerator PropertyCenterUpdatingWorksFine()
        {
            SceneHelper.ClearScene();

            GameObject cam = Object.Instantiate(SceneHelper.GetCamRes());
            cam.transform.position = new Vector3(-0.45988f, 1.375f, -2.869419f);

            GameObject env = Object.Instantiate(SceneHelper.GetEnvRes());
            env.transform.position = Vector3.zero;

            GameObject ch = Object.Instantiate(SceneHelper.GetCharRes());
            ch.transform.position = new Vector3(-1.711f, 0, 1.130581f);
            yield return null;
            yield return null;
            yield return null;

            Vector3 actualCharPosition = ch.GetComponent<GravityDirectionActor>().center;
            Vector3 expectedTransPosition = ch.transform.position;
            Assert.AreEqual(actualCharPosition, expectedTransPosition);
            Assert.AreEqual(actualCharPosition.x, expectedTransPosition.x);
            Assert.AreEqual(actualCharPosition.y, expectedTransPosition.y);
            Assert.AreEqual(actualCharPosition.z, expectedTransPosition.z);

            SceneHelper.ClearScene();
            yield return null;
        }

        [UnityTest]
        public IEnumerator IsGroundedWhenStayOnTheFloor()
        {
            SceneHelper.ClearScene();

            GameObject cam = Object.Instantiate(SceneHelper.GetCamRes());
            cam.transform.position = new Vector3(-0.45988f, 1.375f, -2.869419f);

            GameObject env = Object.Instantiate(SceneHelper.GetEnvRes());
            env.transform.position = Vector3.zero;

            // wait camera
            yield return new WaitForSeconds(UpdateFrameFastExec);

            GameObject ch = Object.Instantiate(SceneHelper.GetCharRes());
            ch.transform.position = new Vector3(-1.711f, 0, 1.130581f);
            yield return null;
            Assert.IsTrue(ch.GetComponent<GravityDirectionActor>().grounded);

            SceneHelper.ClearScene();
            yield return null;
        }

        [UnityTest]
        public IEnumerator IsNotGroundedWhenOverTheFloor()
        {
            SceneHelper.ClearScene();

            GameObject cam = Object.Instantiate(SceneHelper.GetCamRes());
            cam.transform.position = new Vector3(-0.45988f, 5, -2.869419f);

            GameObject env = Object.Instantiate(SceneHelper.GetEnvRes());
            env.transform.position = Vector3.zero;

            GameObject ch = Object.Instantiate(SceneHelper.GetCharRes());
            ch.transform.position = new Vector3(-1.711f, 5, 1.130581f);

            yield return null;

            Assert.IsFalse(ch.GetComponent<GravityDirectionActor>().grounded);

            SceneHelper.ClearScene();
            yield return null;
        }

        [UnityTest]
        public IEnumerator IsGroundedWhenStayOnTheRightWall()
        {
            SceneHelper.ClearScene();

            GameObject cam = Object.Instantiate(SceneHelper.GetCamRes());
            cam.transform.position = new Vector3(1.52f, 1.375f, -8.46f);

            GameObject env = Object.Instantiate(SceneHelper.GetEnvRes());
            env.transform.position = Vector3.zero;

            GameObject ch = Object.Instantiate(SceneHelper.GetCharRes());
            ch.transform.position = new Vector3(5.0f, 1.109f, 1.18f);
            ch.transform.rotation = Quaternion.Euler(0, 0, 90.0f);

            yield return null;

            Assert.IsTrue(ch.GetComponent<GravityDirectionActor>().grounded);

            SceneHelper.ClearScene();
            yield return null;
        }
    }

    /// <summary>
    /// Check character movement with Move methods.
    /// Verify manual collision detecting works fine.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class GravityDirectionActorTests_Move
    {
        private class ActorTestCache
        {
            public string Name;
            public Vector3 CharacterPosition;
            public Quaternion CharacterRotation;
            public Vector3 CharMoveDir;
        }

        private const float MaxStuckInFloorDelta = 0.1f;

        [UnityTest]
        public IEnumerator DontStuckInOtherColliderWhenFallingOnTheFloor()
        {
            SceneHelper.ClearScene();

            GameObject cam = Object.Instantiate(SceneHelper.GetCamRes());
            cam.transform.position = new Vector3(1.52f, 1.375f, -8.46f);

            GameObject env = Object.Instantiate(SceneHelper.GetEnvRes());
            env.transform.position = Vector3.zero;

            GameObject gds = Object.Instantiate(SceneHelper.GetGdsRes());
            gds.transform.position = Vector3.zero;

            GameObject ch = Object.Instantiate(SceneHelper.GetCharRes());
            ch.transform.position = new Vector3(2.03f, 3.109f, 1.18f);
            ch.transform.rotation = Quaternion.Euler(0, 0, 0);

            yield return new WaitForSeconds(2.0f);

            Assert.IsTrue(ch.GetComponent<GravityDirectionActor>().grounded);
            MyAssert.InDelta(ch.transform.position.y, 0, MaxStuckInFloorDelta);

            SceneHelper.ClearScene();
            yield return null;
        }

        [UnityTest]
        public IEnumerator DontStuckInOtherColliderWhenFallingOnTheRightWall()
        {
            SceneHelper.ClearScene();

            GameObject cam = Object.Instantiate(SceneHelper.GetCamRes());
            cam.transform.position = new Vector3(1.52f, 1.375f, -8.46f);

            GameObject env = Object.Instantiate(SceneHelper.GetEnvRes());
            env.transform.position = Vector3.zero;

            GameObject gds = Object.Instantiate(SceneHelper.GetGdsRes());
            gds.transform.position = Vector3.zero;

            GameObject ch = Object.Instantiate(SceneHelper.GetCharRes());
            ch.transform.position = new Vector3(2.03f, 1.109f, 1.18f);
            ch.transform.rotation = Quaternion.Euler(0, 0, 90.0f);

            yield return new WaitForSeconds(2.0f);

            Assert.IsTrue(ch.GetComponent<GravityDirectionActor>().grounded);
            MyAssert.InDelta(ch.transform.position.x, 5.0f, MaxStuckInFloorDelta);

            SceneHelper.ClearScene();
            yield return null;
        }

        private const float NormalCharMovementSpeed = 0.01f;

        [UnityTest]
        public IEnumerator IsNotMoveThroughWalls()
        {
            object[][] dataRows =
            {
                new object[]
                {
                    "Move face forward to front wall, collided perpendicularly",
                    new Vector3(4, 0.0f, 1.18f),
                    Quaternion.Euler(0, 0, 0),
                    Vector3.forward,
                },
                new object[]
                {
                    "Move face forward to right wall, collided perpendicularly",
                    new Vector3(4, 0.0f, 1.18f),
                    Quaternion.Euler(0, 90, 0),
                    Vector3.forward,
                },
                new object[]
                {
                    "Move right side forward to right wall, collided perpendicularly",
                    new Vector3(4f, 0.0f, 1.18f),
                    Quaternion.Euler(0, 0, 0),
                    Vector3.right,
                },
                new object[]
                {
                    "Move front-right size forward to right wall, collided at an angle",
                    new Vector3(4f, 0.0f, 1.18f),
                    Quaternion.Euler(0, 0, 0),
                    (Vector3.right + Vector3.forward).normalized,
                },
                new object[]
                {
                    "Move back-right size forward to right wall, collided at an angle",
                    new Vector3(4f, 0.0f, 1.18f),
                    Quaternion.Euler(0, 0, 0),
                    (Vector3.right + Vector3.back).normalized,
                },
                new object[]
                {
                    "Move face forward to corner between walls, movement angle 45 degrees",
                    new Vector3(2.5f, 0.0f, 2.5f),
                    Quaternion.Euler(0, 45, 0),
                    Vector3.forward,
                },
                new object[]
                {
                    "Move face forward to any wall, movement angle 17.875",
                    new Vector3(2.5f, 0.0f, 2.5f),
                    Quaternion.Euler(0, 17.875f, 0),
                    Vector3.forward,
                },
                new object[]
                {
                    "Move face forward to any wall, movement angle 35.401",
                    new Vector3(2.5f, 0.0f, 2.5f),
                    Quaternion.Euler(0, 35.401f, 0),
                    Vector3.forward,
                },
                new object[]
                {
                    "Move face forward to any wall, movement angle 53.989",
                    new Vector3(2.5f, 0.0f, 2.5f),
                    Quaternion.Euler(0, 53.989f, 0),
                    Vector3.forward,
                },
                new object[]
                {
                    "Move face forward to any wall, movement angle 87.896",
                    new Vector3(2.5f, 0.0f, 2.5f),
                    Quaternion.Euler(0, 87.896f, 0),
                    Vector3.forward,
                },
            };

            SceneHelper.ClearScene();

            GameObject cam = Object.Instantiate(SceneHelper.GetCamRes());
            cam.transform.position = new Vector3(1.52f, 1.375f, -8.46f);

            GameObject env = Object.Instantiate(SceneHelper.GetEnvRes());
            env.transform.position = Vector3.zero;

            GameObject frontWall = GameObject.Find("Wall Forward");
            GameObject rightWall = GameObject.Find("Wall Right");

            GameObject ch = Object.Instantiate(SceneHelper.GetCharRes());
            var chTransform = ch.transform;
            GravityDirectionActor gravityDirectionActor = ch.GetComponent<GravityDirectionActor>();

            foreach (object[] dataRow in dataRows)
            {
                Debug.Log($"Test case '{dataRow[0]}'");

                // reset character
                chTransform.position = (Vector3)dataRow[1];
                chTransform.rotation = (Quaternion)dataRow[2];
                gravityDirectionActor.fallingSpeed = 0;
                gravityDirectionActor.Move(Vector3.zero);
                yield return null; // update frames

                // move right to wall
                Vector3 movement = (Vector3)dataRow[3] * NormalCharMovementSpeed;
                const int frameRate = 60;
                const int movementTimeInSeconds = 10;
                for (int i = 0; i < frameRate * movementTimeInSeconds; i++)
                {
                    gravityDirectionActor.Move(movement);
                    yield return null;
                }

                yield return null;

                float charMaxXWithRadiusPadding = chTransform.position.x + gravityDirectionActor.radius;
                float charMaxZWithRadiusPadding = chTransform.position.z + gravityDirectionActor.radius;

                const float floatFault = 0.015f; // TODO: should assert without correction

                Assert.LessOrEqual(charMaxZWithRadiusPadding - floatFault, frontWall.transform.position.z);
                Assert.LessOrEqual(charMaxXWithRadiusPadding - floatFault, rightWall.transform.position.x);

                yield return null; // update frames
            }

            Object.Destroy(ch);
        }

        [UnityTest]
        public IEnumerator CanMoveSmoothlyAlongTheWall_ReachTheCorner()
        {
            const int frameRate = 60;
            const int movementTimeInSeconds = 10;

            ActorTestCache[] dataRows =
            {
                new()
                {
                    Name = "Move face forward to corner between walls, movement angle 45 degrees",
                    CharacterPosition = new Vector3(-2.0f, 0.0f, 2.0f),
                    CharacterRotation = Quaternion.Euler(0, 45, 0),
                    CharMoveDir = Vector3.forward,
                },
            };

            SceneHelper.ClearScene();

            GameObject cam = Object.Instantiate(SceneHelper.GetCamRes());
            cam.transform.position = new Vector3(1.52f, 1.375f, -8.46f);

            GameObject env = Object.Instantiate(SceneHelper.GetEnvRes());
            env.transform.position = Vector3.zero;

            var ch = Object.Instantiate(SceneHelper.GetCharRes());
            var chTransform = ch.transform;
            var gravityDirectionActor = ch.GetComponent<GravityDirectionActor>();

            foreach (var dataRow in dataRows)
            {
                Debug.Log($"Test case '{dataRow.Name}'");

                // reset character
                chTransform.position = dataRow.CharacterPosition;
                chTransform.rotation = dataRow.CharacterRotation;
                gravityDirectionActor.fallingSpeed = 0;
                gravityDirectionActor.Move(Vector3.zero);
                yield return null; // update frames

                // move right to wall
                Vector3 movement = dataRow.CharMoveDir * NormalCharMovementSpeed;

                for (int i = 0; i < frameRate * movementTimeInSeconds; i++)
                {
                    gravityDirectionActor.Move(movement);
                    yield return null; // update frames
                }

                yield return null; // update frames

                Assert.GreaterOrEqual(ch.transform.position.x, 4.5f);
                Assert.GreaterOrEqual(ch.transform.position.z, 4.5f);

                yield return null; // update frames
            }

            Object.Destroy(ch);
        }
    }
}