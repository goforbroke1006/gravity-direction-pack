using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CharacterController = GravityDirectionPack.Scripts.CharacterController;
using Object = UnityEngine.Object;

namespace GravityDirectionPack.Tests.Runtime
{
    // ReSharper disable once InconsistentNaming
    public class CharacterControllerTests_Common
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

            float actualExposedRadius = ch.GetComponent<CharacterController>().radius;
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

            Vector3 actualCharPosition = ch.GetComponent<CharacterController>().center;
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
            Assert.IsTrue(ch.GetComponent<CharacterController>().grounded);

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

            Assert.IsFalse(ch.GetComponent<CharacterController>().grounded);

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

            Assert.IsTrue(ch.GetComponent<CharacterController>().grounded);

            SceneHelper.ClearScene();
            yield return null;
        }
    }

    /// <summary>
    /// Check character movement with Move methods.
    /// Verify manual collision detecting works fine.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class CharacterControllerTests_Move
    {
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

            Assert.IsTrue(ch.GetComponent<CharacterController>().grounded);
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

            Assert.IsTrue(ch.GetComponent<CharacterController>().grounded);
            MyAssert.InDelta(ch.transform.position.x, 5.0f, MaxStuckInFloorDelta);

            SceneHelper.ClearScene();
            yield return null;
        }

        [UnityTest]
        public IEnumerator IsNotMoveThroughRightWall()
        {
            SceneHelper.ClearScene();

            GameObject cam = Object.Instantiate(SceneHelper.GetCamRes());
            cam.transform.position = new Vector3(1.52f, 1.375f, -8.46f);

            GameObject env = Object.Instantiate(SceneHelper.GetEnvRes());
            env.transform.position = Vector3.zero;

            GameObject ch = Object.Instantiate(SceneHelper.GetCharRes());
            ch.transform.position = new Vector3(4.5f, 0.0f, 1.18f);
            ch.transform.rotation = Quaternion.Euler(0, 0, 0);
            CharacterController characterController = ch.GetComponent<CharacterController>();

            yield return null;

            // move right to wall
            for (int i = 0; i < 60 * 8; i++)
            {
                characterController.Move(new Vector3(0.01f, 0, -0.002f));
                yield return null;
            }

            yield return new WaitForSeconds(2);

            GameObject rightWall = GameObject.Find("Wall Right");
            float charWithRadiusPadding = ch.transform.position.x + characterController.radius;
            Assert.IsTrue(charWithRadiusPadding <= rightWall.transform.position.y);

            SceneHelper.ClearScene();
            yield return null;
        }
    }
}